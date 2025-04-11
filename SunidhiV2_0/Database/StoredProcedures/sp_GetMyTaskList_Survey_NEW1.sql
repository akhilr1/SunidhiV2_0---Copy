-- USE `SUNIDHI_CORE_V2_0`;
 DROP procedure IF EXISTS `sp_GetMyTaskList_Survey_NEW1`;
 DELIMITER $$

CREATE DEFINER=`user`@`%` PROCEDURE `sp_GetMyTaskList_Survey_NEW1`(IN loggedUserID int(10),IN SyncDbID int(10),IN  varWorkspaceID  int(10))
BEGIN
DECLARE done INT DEFAULT FALSE;
	DECLARE varTaskData  json DEFAULT NULL; 	
	DECLARE jsonTask json DEFAULT NULL; 	   
    
	DECLARE varType  varchar(50);
	DECLARE varTypeID  INT;	
	DECLARE varResponseSynID  INT;     
    DECLARE varDataCollector varchar(500);
    DECLARE varApproverIds varchar(500);
    
	DECLARE curTask CURSOR FOR SELECT TypeID,Type,ResponseSyncID FROM tempMyTask;  
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    
	 -- DROP TABLE tempMyTask;
	CREATE TEMPORARY TABLE IF NOT EXISTS tempMyTask
	(SyncDBID Int,Command varchar(100),Type varchar(100),TypeID INT,TypeIDColumn varchar(100) , Status varchar(100), WorkflowID Int, TotalWFLevel Int, -- SubmittedWFLevel INT, SubmittedUser Int,	
		WFLevel Int,WFLevelStatus varchar(100) ,ResponseSyncID INT ,DataCollectedBy Int,TaskDate DateTime, TaskSubmittedDate DateTime,TaskdataAddedBy varchar(200),FinanceWorkflowId INT ,ReportingValue INT,
        CommittedFund INT ,CommittedBy varchar(200),TaskData json,DataCollectorIds varchar(500),ApproverIds varchar(500));

	TRUNCATE TABLE tempMyTask;  	    
	INSERT INTO tempMyTask  
    Select Temp.*, null,0,0,null,'[{}]',null,null From (
	Select Distinct T.ID , Command,'Survey',T.TypeID,T.TypeIDColumn ,Status ,WorkflowID,TotalWFLevel, -- -1 as SubmittedWFLevel, -1 as SubmittedUser ,
    0 as WFLevel, 'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy ,TaskDate ,null TaskSubmittedDate, U.Name as TaskdataAddedBy  
	From TaskSyncDB T 
	Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID= loggedUserID ) TA On  T.TypeID = TA.TypeID 
	Inner Join (Select Distinct  A.SurveyID TypeID   ,ReportingFrequency,TaskDate,EndDate From (Select S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate,
				Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate,Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate 
				From TbProjectSurvey S Inner Join TbProject P On P.ProjectId= S.ProjectId Left Join (Select * from TbProjectSurveyTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1) A
				Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID 
                inner join TbUser U on U.ID = T.CreatedBy
				Where ifnull(T.IsActive,0) =1 And binary T.Type= 'Survey' AND T.Command ='ADD' And T.ID > SyncDbID   And T.WorkspaceID = varWorkspaceID 
                 And
                 case when TD.ReportingFrequency = 9 then 1=1
             else
              TD.TypeID NOT IN (
					Select MSK.TypeID FROM TbMyTask MSK
					JOIN TbProjectSurveyUser OTASN ON  MSK.UserID = OTASN.UserID and MSK.WFLevel = 0 AND MSK.TypeID = OTASN.SurveyID
					WHERE  OTASN.IsDeleted = 0 AND MSK.IsActive = 1 AND  MSK.WFLevelStatus = 'Completed'  AND MSK.Type = 'Survey'
					AND  MSK.TypeID = TypeID AND MSK.TaskDate = DATE_FORMAT(TD.TaskDate, "%Y-%m-%d"))
                    end
                 and T.ID > SyncDbID
            -- T.TypeID Not In (Select TypeID From TbMyTask  Where ifnull(IsActive,0) =1 And Type = 'Survey'  And  WFLevelStatus = 'Completed'  And  WFLevel =0 And UserID = AssgUserID And TaskDate =TD.TaskDate) 

    UNION 
			Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus, ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate,  U.Name as TaskdataAddedBy 
	From TaskSyncDB T 	
	Inner Join (Select Max(T.ID) ID , 'Survey',T.TypeID ,DataCollectedBy	,Resp.SyncTaskID   ResponseSyncID, ST.TaskDate
			From TaskSyncDB T            
			Inner Join  (Select SyncTaskID,TypeID,Type,UserID ,TaskDate From TbMyTask MT Where ifnull(MT.IsActive,0) =1 And Type = 'Survey' And WFLevelStatus = 'Completed' And   WFLevel =0 
						And TypeID In ( Select TypeID From  TaskSyncDB  Where Command = 'TaskResponse' And Type = 'Survey' And ifnull(IsActive,0) =1)) 
						Resp On Resp.UserID = T.DataCollectedBy And binary T.Type= binary Resp.Type and T.TypeID = Resp.TypeID 
			Left Join (Select SyncTaskID,TypeID,Type,TaskDate From TbMyTask Where Type = 'Survey' And ifnull(IsActive,0) =1) ST On ST.SyncTaskID =T.ID And ST.TaskDate = Resp.TaskDate			
            Where ifnull(T.IsActive,0) =1 And T.Type = 'Survey' Group By T.TypeID ,DataCollectedBy	,Resp.SyncTaskID,TaskDate) TT On TT.ID =T.ID                 
	Inner Join TbMyTask MT  On MT.SyncTaskID  =T.ID And MT.TaskDate= TT.TaskDate  
	inner join TbProjectSurvey srv ON  MT.TypeID = srv.SurveyID
	Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID= loggedUserID ) TA On  T.TypeID = TA.TypeID 
	Inner join TbUser U on U.ID = T.CreatedBy
	Where ifnull(T.IsActive,0) =1  And Command= 'TaskResponse' And T.Status != 'Completed'  And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID And TA.AssgUserID =loggedUserID And T.Type ='Survey'
    -- AND srv.ReportingFrequency != 9  And TotalWFLevel >0--
    
    
    UNION
    -- for approvval 1
	Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus, ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate , U.Name as TaskdataAddedBy 
	From TaskSyncDB T 	
	Inner Join (Select Max(T.ID) ID , 'Survey',T.TypeID ,DataCollectedBy	,Resp.SyncTaskID   ResponseSyncID, ST.TaskDate
	From TaskSyncDB T            
	Inner Join  (Select SyncTaskID,TypeID,Type,UserID ,TaskDate From TbMyTask MT Where ifnull(MT.IsActive,0) =1 And Type = 'Survey' And WFLevelStatus = 'Completed' And   WFLevel =0 
				And TypeID In ( Select TypeID From  TaskSyncDB  Where Command = 'TaskResponse' And Type = 'Survey' And ifnull(IsActive,0) =1)) 
				Resp On Resp.UserID = T.DataCollectedBy And binary T.Type= binary Resp.Type and T.TypeID = Resp.TypeID 
			Left Join (Select SyncTaskID,TypeID,Type,TaskDate From TbMyTask Where Type = 'Survey' And ifnull(IsActive,0) =1) ST On ST.SyncTaskID =T.ID And ST.TaskDate = Resp.TaskDate			
            Where ifnull(T.IsActive,0) =1 And T.Type = 'Survey' Group By T.TypeID ,DataCollectedBy	,Resp.SyncTaskID,TaskDate) TT On TT.ID =T.ID                 
	Inner Join TbMyTask MT  On MT.SyncTaskID  =T.ID And MT.TaskDate= TT.TaskDate  
	inner join TbProjectSurvey srv ON  MT.TypeID = srv.SurveyID
    Inner join TbUser U on U.ID = T.CreatedBy
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where ifnull(T.IsActive,0) =1 And TotalWFLevel >0 And Command= 'TaskResponse' And T.Status != 'Completed'  And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID 
         And W.WFLevel=(MT.WFLevel +1) And W.AssgUserID =loggedUserID And T.Type ='Survey'
    AND srv.ReportingFrequency != 9 
    
    UNION
    -- Approval 1.1
    Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			MT.WFLevel as WFLevel  , 'Completed' WFLevelStatus, ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate , U.Name as TaskdataAddedBy 
	From TaskSyncDB T 	
	Inner Join (Select Max(T.ID) ID , 'Survey',T.TypeID ,DataCollectedBy	,Resp.SyncTaskID   ResponseSyncID, ST.TaskDate
	From TaskSyncDB T            
	Inner Join  (Select SyncTaskID,TypeID,Type,UserID ,TaskDate From TbMyTask MT Where ifnull(MT.IsActive,0) =1 And Type = 'Survey' And WFLevelStatus = 'Completed' And   WFLevel =0 
				And TypeID In ( Select TypeID From  TaskSyncDB  Where Command = 'TaskResponse' And Type = 'Survey' And ifnull(IsActive,0) =1)) 
				Resp On Resp.UserID = T.DataCollectedBy And binary T.Type= binary Resp.Type and T.TypeID = Resp.TypeID 
			Left Join (Select SyncTaskID,TypeID,Type,TaskDate From TbMyTask Where Type = 'Survey' And ifnull(IsActive,0) =1) ST On ST.SyncTaskID =T.ID And ST.TaskDate = Resp.TaskDate			
            Where ifnull(T.IsActive,0) =1 And T.Type = 'Survey' Group By T.TypeID ,DataCollectedBy	,Resp.SyncTaskID,TaskDate) TT On TT.ID =T.ID                 
	Inner Join TbMyTask MT  On MT.SyncTaskID  =T.ID And MT.TaskDate= TT.TaskDate  
	inner join TbProjectSurvey srv ON  MT.TypeID = srv.SurveyID
    Inner join TbUser U on U.ID = T.CreatedBy
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where ifnull(T.IsActive,0) =1 And TotalWFLevel >0 And Command= 'TaskResponse' And T.Status != 'Completed' And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID 
           And W.WFLevel=(MT.WFLevel) And W.AssgUserID =loggedUserID And T.Type ='Survey'
    -- AND srv.ReportingFrequency != 9 
    
  UNION
     -- for approvval 1  - anytime
     Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus,case when ifnull(MT.DataCollectionSyncDBID,T.ID) = 0 then T.ID else ifnull(MT.DataCollectionSyncDBID,T.ID) end AS ResponseSyncID, 
            T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate, U.Name as TaskdataAddedBy 
	From TaskSyncDB T 	
	Inner Join TbMyTask MT  On MT.SyncTaskID  =T.ID 
    inner join TbProjectSurvey srv ON  MT.TypeID = srv.SurveyID
    Inner join TbUser U on U.ID = T.CreatedBy
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where ifnull(T.IsActive,0) =1 And TotalWFLevel >0 And Command= 'TaskResponse' And T.Status != 'Completed' And W.WFLevel=(MT.WFLevel +1)  And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID  And W.AssgUserID =loggedUserID And T.Type ='Survey'
	AND srv.ReportingFrequency = 9 AND MT.SyncTaskID NOT IN 
    (
     select ParentSyncDBID from TbMyTask where  WFLevel = (MT.WFLevel +1) and WFLevelStatus = 'Completed' and TaskDate  = MT.TaskDate
    )
    
  ) As Temp ;
  
  
  
    OPEN curTask;
  read_loop: LOOP
    FETCH curTask INTO varTypeID, varType,varResponseSynID;    
    IF done THEN
      LEAVE read_loop;
    END IF;
	IF varType='Survey'  THEN
    Begin    
			
			Set @varTaskData=(SELECT  JSON_OBJECT( 'SurveyID' ,SurveyID ,'ProjectId', A.ProjectId,'ProjectName', P.ProjectName ,'SurveyName' , SurveyName,'Description', A.SurveyDescription,
				'StartDate',A.StartDate,'EndDate', A.EndDate,'BeneficiaryTypeID' , BT.BeneficiaryTypeID,'BeneficiaryType',  BT.Name,
                'Periodicity', (Case When ReportingFrequency =1 then 'Never'	When ReportingFrequency =2 then 'Once'  When ReportingFrequency =3 then 'Daily'
							When ReportingFrequency =4 then 'Weekly'  When ReportingFrequency =5 then 'Monthly'	When ReportingFrequency =6 then 'Quarterly'
							When ReportingFrequency =7 then 'Half_Yearly'	When ReportingFrequency =8 then 'Yearly' When ReportingFrequency =9 then 'Anytime' End ) ,'CreatedDate', A.CreatedDate)	
			FROM TbProjectSurvey A Inner Join TbProject P On A.ProjectId =P.ProjectId      
            Left Join TbProjectBeneficiaryType BT On  BT.BeneficiaryTypeID=A.BeneficiaryTypeID Where SurveyID=varTypeID and P.ProjectStatus='Deploy');
			
              SET @varDataCollectorIds =  (SELECT GROUP_CONCAT(UserID) user_ids FROM TbProjectSurveyUser where SurveyID = varTypeID and ifnull(IsDeleted,0) = 0 and UserID >0 GROUP BY SurveyID);
 
              SET @varApproverIds =  (select GROUP_CONCAT(WFLDetails.UserID) user_ids from TbProjectWorkflowDetails WFLDetails JOIN TbProjectWorkflow WFL ON WFLDetails.WorkflowID = WFL.WorkflowID
									join TbProjectSurvey SRV on SRV.WorkflowID = WFL.WorkflowID WHERE SRV.SurveyID = varTypeID and ifnull(WFL.IsDeleted,0) =0 and ifnull(WFLDetails.IsDeleted,0) =0 GROUP BY WFLDetails.WorkflowID);
                          
           
		End;
    End IF ;
   
    Update tempMyTask Set TaskData = @varTaskData,DataCollectorIds=@varDataCollectorIds,ApproverIds=@varApproverIds    Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;   
   
  END LOOP; 
  CLOSE curTask;

  
Select A.*,varWorkspaceID WorkspaceID from tempMyTask A Order By   A.SyncDBID desc,A.TaskDate DESC;
DROP TABLE tempMyTask;
END$$

DELIMITER ;

