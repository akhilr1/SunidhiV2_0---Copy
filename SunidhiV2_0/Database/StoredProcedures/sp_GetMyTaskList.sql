-- USE `SUNIDHI_CORE_V2_0`;
DROP procedure IF EXISTS `sp_GetMyTaskList`;
DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_GetMyTaskList`(IN loggedUserID int(10),IN varSyncDbID int(10),IN  varWorkspaceID  int(10))
BEGIN
	DECLARE done INT DEFAULT FALSE;
	DECLARE varTaskData  json; 	
	DECLARE jsonTask json;    
    
	DECLARE varType  varchar(50);
	DECLARE varTypeID  INT;
	DECLARE varWFLevelStatus  varchar(50);
	DECLARE varResponseSynID  INT; 
    DECLARE intDataCollectedBy INT;
     DECLARE varSyncDBID1 INT;
    
    DECLARE curTask CURSOR FOR SELECT TypeID,Type,ResponseSyncID,SyncDBID,DataCollectedBy FROM tempMyTask; 
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
   
    -- DROP TEMPORARY TABLE tempMyTask;
	CREATE TEMPORARY TABLE IF NOT EXISTS tempMyTask
	(SyncDBID Int,Command varchar(100),Type varchar(100),TypeID INT,TypeIDColumn varchar(100) , Status varchar(100), WorkflowID Int, TotalWFLevel Int, -- SubmittedWFLevel INT,SubmittedUser Int,	
		WFLevel Int,WFLevelStatus varchar(100) ,ResponseSyncID INT ,DataCollectedBy Int ,TaskDate datetime, TaskSubmittedDate datetime, FinanceWorkflowId INT ,
        ReportingValue INT, CommittedFund INT ,CommittedBy varchar(200), TaskData json);
        
   
	Truncate Table tempMyTask;  	    
	Insert Into tempMyTask
	Select Temp.*, '[{}]' From (
	Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,WorkflowID,TotalWFLevel, -- -1 as SubmittedWFLevel, -1 as SubmittedUser ,
    0 as WFLevel, 'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy
    ,DATE_FORMAT(TrackDate, "%Y-%m-%d") as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate ,0 as FinanceWorkflowId , 0  as RiskReportingValue, 0 as CommittedFund ,'' as CommittedBy
	From TaskSyncDB T 
	Inner Join (
    Select Distinct Type, TypeID , UserID As  AssgUserID , TrackDate From(
				Select Distinct 'Outcome' Type,A.pk_OutcomeID TypeID ,OU.UserID ,TrackDate From (Select S.pk_OutcomeID ,Periodicity ,
				Case When (Periodicity =1 OR Periodicity =9 ) then date(now()) Else STrack.TrackDate End AS TrackDate,
			S.StartDate,S.EndDate From TbOutcomes S Left Join (Select * from TbOutcomeTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.OutcomeId = S.pk_OutcomeID 
             inner join TbProject Tb on S.ProjectId = Tb.ProjectId and Tb.ProjectStatus='Deploy' and Tb.IsActive = 1 and Tb.IsDeleted = 0 and Tb.IsCreated = 1
            ) A
			inner join TbOutcomeAssignUsers OU On A.pk_OutcomeID = OU.OutcomeId Where TrackDate Between StartDate And EndDate And TrackDate Is not null and OU.UserID = loggedUserID and OU.IsDeleted = 0
			AND A.pk_OutcomeID NOT IN (
					Select MSK.TypeID FROM TbMyTask MSK
					JOIN TbOutcomeAssignUsers OTASN ON  MSK.UserID = OTASN.UserID AND MSK.TypeID = OTASN.OutcomeId
					WHERE  OTASN.IsDeleted = 0 AND MSK.IsActive = 1 AND  MSK.WFLevelStatus = 'Completed'  AND MSK.Type = 'Outcome'
					AND  MSK.TypeID = TypeID AND MSK.TaskDate = DATE_FORMAT(A.TrackDate, "%Y-%m-%d"))
            
            union
            Select 'RISK'  Type,TbRisk.ID TypeID , UserID as AssgUserID , 
			case when (TbRisk.RiskPeriodicity = 9 OR TbRisk.RiskPeriodicity  =1) Then date(now()) else TbRiskTrackDate.TrackDate end as TrackDate 
            From TbRisk TbRisk
		    inner join TbProject Tb on TbRisk.ProjectID = Tb.ProjectId 
            Inner Join TbRiskAssignUsers TbRiskAssignUsers On TbRisk.ID = TbRiskAssignUsers.riskId
			left join TbRiskTrackDate TbRiskTrackDate on TbRiskTrackDate.RiskID = TbRisk.ID 
			Where TbRisk.IsDeleted =0   And TbRiskAssignUsers.IsDeleted =0 and UserID= loggedUserID  and TbRiskTrackDate.TrackStatus = 0
            and TbRiskTrackDate.TrackDate <= date(now()) and TbRiskTrackDate.TrackDate <= TbRisk.EndDate and Tb.ProjectStatus='Deploy' and Tb.IsActive = 1 and Tb.IsDeleted = 0 and Tb.IsCreated = 1
            union
            Select Distinct 'Activity' Type,A.ActivityId TypeID ,OU.UserID as AssgUserID ,TrackDate 
			From (Select S.ActivityId ,Periodicity ,Case When (Periodicity =1 OR Periodicity =9 ) then date(now()) Else STrack.TrackDate End AS TrackDate,
			S.StartDate,S.EndDate From TbProjectActivity S Left Join (Select * from TbProjectActivityTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.ActivityId = S.ActivityId 
             and S.SurveyId is not null inner join TbProject Tb on S.ProjectId = Tb.ProjectId and Tb.ProjectStatus='Deploy' and Tb.IsActive = 1 and Tb.IsDeleted = 0 and Tb.IsCreated = 1
            ) A
			inner join TbProjectActivityToUsers OU On A.ActivityId = OU.ActivityId Where TrackDate Between StartDate And EndDate And TrackDate Is not null and OU.UserID = loggedUserID and OU.IsDeleted = 0
			 ) AU ) TA On binary T.Type= binary TA.Type and T.TypeID = TA.TypeID 
	 Where T.Command = 'ADD' And T.Status !='Completed' And T.ID > varSyncDbID  And T.WorkspaceID = varWorkspaceID 
     -- And T.TypeID Not  In (Select TypeID From TbMyTask  Where Type = T.Type And  WFLevelStatus = 'Completed'  And  WFLevel =0 And UserID = AssgUserID ) 
	  and T.TypeID Not  In (Select TypeID From TbMyTask  Where Type = T.Type And  WFLevelStatus = 'Completed' 
       And UserID = AssgUserID And TaskDate = TA.TrackDate and TbMyTask.IsActive = 1) 
      
	Union 
    -- Fetching for Task response
	Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus, ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate,0 as FinanceWorkflowId, MT.RiskReportingValue as RiskReportingValue,
            0 as CommittedFund,'' as CommittedBy
	From TaskSyncDB T 	
	Inner Join (Select Max(T.ID) ID , T.Type,T.TypeID ,DataCollectedBy	,Resp.SyncTaskID   ResponseSyncID, ST.TaskDate
			From TaskSyncDB T            
			Inner Join  (Select SyncTaskID,TypeID,Type,UserID ,TaskDate From TbMyTask MT Where ifnull(MT.IsActive,0) =1 AND WFLevelStatus = 'Completed' And   WFLevel =0 
						And TypeID In ( Select TypeID From  TaskSyncDB  Where Command = 'TaskResponse' And Type = MT.Type AND ifnull(MT.IsActive,0) =1 )) 
						Resp On Resp.UserID = T.DataCollectedBy And binary T.Type= binary Resp.Type and T.TypeID = Resp.TypeID 
			Left Join (Select SyncTaskID,TypeID,Type,TaskDate From TbMyTask where ifnull(IsActive,0) =1 ) ST On ST.SyncTaskID =T.ID And ST.TaskDate = Resp.TaskDate			
            where ifnull(T.IsActive,0) =1 Group By T.Type,T.TypeID ,DataCollectedBy	,Resp.SyncTaskID,TaskDate) TT On TT.ID =T.ID                 
	Inner Join  TbMyTask MT  On MT.SyncTaskID  =T.ID And MT.TaskDate= TT.TaskDate    
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where TotalWFLevel >0 And Command= 'TaskResponse' And T.Status !='Completed'  And T.ID > varSyncDbID And T.WorkspaceID = varWorkspaceID And W.WFLevel=(MT.WFLevel +1) 
    And W.AssgUserID =loggedUserID And T.Type !='Survey' and MT.IsActive = 1 and ifnull(T.IsActive,0) =1 
    
    union
    -- Fetch for getting activity linked payment
	Select Distinct T.ID , Command, 'Payment' as Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
    W.WFLevel,'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , W.WorkflowID as FinanceWorkflowId , 
                0 as ReportingValue , 0 as CommittedFund,'' as CommittedBy
	From TaskSyncDB T 
	left join  TbMyTask MT  On MT.SyncTaskID  =T.ID 
	left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbFinancetWorkflowDetails Where IsDeleted =0 ) W 
     On W.WorkflowID = T.FinanceWorkflowID 
	 Where 
	  T.TypeID Not  In (Select TypeID From TbMyTask  Where Type = 'Payment' And  WFLevelStatus = 'Completed'  And W.WFLevel=(ifnull(MT.WFLevel,0) +1) And UserID = AssgUserID ) AND  
      T.TotalWFLevel >0  And  T.ID > varSyncDbID  And W.AssgUserID =loggedUserID And T.WorkspaceID = varWorkspaceID
      and Status != 'DataCollected'
      And  (W.WFLevel=(ifnull(MT.WFLevel,0) +1))
      
      union
      -- fetch for getting payment term request paymnet
      Select Distinct T.ID , Command, 'Payment Request' as Type,T.TypeID,T.TypeIDColumn ,T.Status ,-1 as WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
      0,'Pending' WFLevelStatus ,  -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
                -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	From TaskSyncDB T 
	inner join  TbProjectRequestFund ProjectRequestFund  On T.TypeID  =ProjectRequestFund.ID
	 Where  T.Status ='Pending' and DonorUser=loggedUserID  and T.ID > varSyncDbID And T.WorkspaceID = varWorkspaceID and ProjectRequestFund.PaymentStatus = 'Pending' 
	and T.TypeID Not  In (Select TypeID From TbMyTask  Where Type = T.Type And  WFLevelStatus = 'Completed'  And  WFLevel =1 And UserID = loggedUserID)
    
    union 
    -- Fetch for Donor commit fund request ---
    Select Distinct T.ID , Command, 'Fund Request' as Type,T.TypeID,T.TypeIDColumn ,Status ,-1 as WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
    0,'Pending' WFLevelStatus ,-1 ResponseSyncID , T.CreatedBy as DataCollectedBy,now() as TaskDate, 
            DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , -1 as FinanceWorkflowId , 0 as ReportingValue , 
            T.CommitedAmount as CommittedFund ,(select Name From TbUser Where ID = T.CreatedBy) as CommittedBy
	From TaskSyncDB T 
	inner join  TbProjectBudgetAndFundDetails TbProjectBudgetAndFundDetails  On T.TypeID  =TbProjectBudgetAndFundDetails.ProjectId
    inner join TbProject TbProject on TbProject.ProjectId = TbProjectBudgetAndFundDetails.ProjectId
	 Where  Status ='Pending' And T.WorkspaceID = varWorkspaceID and TbProject.ProjectStatus='Deploy' 
     and TbProjectBudgetAndFundDetails.ProjectId in 
     (select  ProjectId from  TbProjectBudgetAndFundDetails where UserID = loggedUserID and UserType = 'F' and IsDeleted = 0) 
     and T.ID > varSyncDbID and  T.Type = 'Fund Request' and T.ID Not  In (Select SyncTaskID From TbMyTask  Where Type = 'Fund Request' And  WFLevelStatus = 'Completed'  And  WFLevel =1 
    And TypeID = TbProjectBudgetAndFundDetails.ProjectId and  TbMyTask.IsActive = 0) 
	
   	
     UNION 
    -- project--
       Select Distinct T.ID , Command, 'Project' as Type,T.TypeID,T.TypeIDColumn ,Status , T.WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
       0,'Pending' WFLevelStatus ,
                -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
                -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	From TaskSyncDB T  Left Join TbInitiativeWorkflowLevelDetails W On W.WorkflowID = T.WorkflowID 
    Where  T.`Command` = 'ADD'  AND  T.`Type` = 'Project' AND T.ID > varSyncDbID AND W.UserID = loggedUserID AND ifnull(W.IsDeleted,0) = 0 
	and T.TypeID Not  In (Select TypeID From TbMyTask  Where Type =  'Project' And  WFLevelStatus = 'Completed' and ifnull(IsActive,0) = 1) And T.WorkspaceID = varWorkspaceID
	
    UNION 
	-- project--
       Select Distinct T.ID , Command, 'Project' as Type,T.TypeID,T.TypeIDColumn ,Status , T.WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
       0,'Pending' WFLevelStatus ,
                -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
                -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	From TaskSyncDB T  Left Join TbInitiativeWorkflowLevelDetails W On W.WorkflowID = T.WorkflowID 
    Where  T.`Command` = 'ADD'  AND  T.`Type` = 'Project' AND T.ID > varSyncDbID AND W.UserID = loggedUserID AND ifnull(W.IsDeleted,0) = 0 AND ifnull(T.IsActive,0) = 1   AND W.WFLevel =1 
	and T.TypeID Not  In (Select TypeID From TbMyTask  Where Type =  'Project' And  WFLevelStatus = 'Completed' and ifnull(IsActive,0) = 1)  And T.WorkspaceID = varWorkspaceID
	
    UNION 
	-- project--
	select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
				(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus, T2.ID AS ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate,0 as FinanceWorkflowId, MT.RiskReportingValue as RiskReportingValue,
				0 as CommittedFund,'' as CommittedBy
	 from TaskSyncDB T JOIN TbMyTask MT ON MT.SyncTaskID = T.ID
	JOIN
	(
	  SELECT MAX(T1.ID) ID  FROM TaskSyncDB T1 JOIN TbMyTask MT1 ON MT1.SyncTaskID = T1.ID
	  Left Join (Select WorkflowID, WFLevel, UserID AssgUserID From TbInitiativeWorkflowLevelDetails Where IsDeleted =0) W1 On W1.WorkflowID = T1.WorkflowID
	  WHERE T1.Type = 'Project' AND T1.Status != 'Completed' AND  W1.AssgUserID = loggedUserID AND IFNULL(MT1.IsActive,0)=1 AND IFNULL(T1.IsActive,0)=1 GROUP BY T1.TypeID
	) T2  ON T2.ID = T.ID
	Join (Select WorkflowID, WFLevel, UserID AssgUserID From TbInitiativeWorkflowLevelDetails Where IsDeleted =0) W On W.WorkflowID = T.WorkflowID
	WHERE  TotalWFLevel >0  AND T.Type = 'Project' AND Command= 'TaskResponse' AND  T.Status != 'Completed'
	AND W.WFLevel=(MT.WFLevel +1) AND  W.AssgUserID = loggedUserID AND IFNULL(MT.IsActive,0)=1 AND  ifnull(T.IsActive,0) = 1 And T.WorkspaceID = varWorkspaceID
	   
   
   -- issue report
	 UNION 
	 Select Distinct T.ID , Command, T.`Type` as Type,T.TypeID,T.TypeIDColumn ,Status ,0 AS WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
	 0,'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
	 -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	 From TaskSyncDB T 
	 JOIN TbProjectActivity prjact ON prjact.ActivityId = T.TypeID
	 JOIN TbProject tbprj ON tbprj.ProjectId = prjact.ProjectId 
	 JOIN TbInitiative tbin ON  tbin.InitiativeId = tbprj.InitiativeId
	 JOIN TbWorkspace wrk ON wrk.WorkspaceID = tbin.WorkspaceID
	 WHERE tbprj.ProjectStatus='Deploy' and 
     T.`Type` = 'Issue' AND T.ID > varSyncDbID AND T.Status = 'Pending' AND wrk.CreatedBy = loggedUserID And T.WorkspaceID = varWorkspaceID
     AND T.`TypeIDColumn` = 'ActivityId' AND IFNULL(T.IsActive,0)=1 and T.ID NOT IN (
	 SELECT ParentSyncDBID  FROM TaskSyncDB PRNT WHERE PRNT.ParentSyncDBID = T.ID)
	  
	 UNION 
	 Select Distinct T.ID , Command, T.`Type` as Type,T.TypeID,T.TypeIDColumn ,Status ,0 AS WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
	 0,'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
	 -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	 From TaskSyncDB T 
	 JOIN TbOutcomes prjOutcome ON prjOutcome.pk_OutcomeID = T.TypeID
	 JOIN TbProject tbprj ON tbprj.ProjectId = prjOutcome.ProjectId 
	 JOIN TbInitiative tbin ON  tbin.InitiativeId = tbprj.InitiativeId
	 JOIN TbWorkspace wrk ON wrk.WorkspaceID = tbin.WorkspaceID
	 WHERE tbprj.ProjectStatus='Deploy' and  T.`Type` = 'Issue' AND T.ID > varSyncDbID AND T.Status = 'Pending' AND wrk.CreatedBy = loggedUserID And T.WorkspaceID = varWorkspaceID
     AND T.`TypeIDColumn` = 'pk_OutcomeID' AND IFNULL(T.IsActive,0)=1 and T.ID NOT IN (
	 SELECT ParentSyncDBID  FROM TaskSyncDB PRNT WHERE PRNT.ParentSyncDBID = T.ID)
	  
	 
	 UNION 
 
	 Select Distinct T.ID , Command, T.`Type` as Type,T.TypeID,T.TypeIDColumn ,Status ,0 AS WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
	 0,'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
	 -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	 From TaskSyncDB T 
	 JOIN TbRisk prjRISK ON prjRISK.ID = T.TypeID
	 JOIN TbProject tbprj ON tbprj.ProjectId = prjRISK.ProjectId 
	 JOIN TbInitiative tbin ON  tbin.InitiativeId = tbprj.InitiativeId
	 JOIN TbWorkspace wrk ON wrk.WorkspaceID = tbin.WorkspaceID
	 WHERE tbprj.ProjectStatus='Deploy' and  T.`Type` = 'Issue' AND T.ID > varSyncDbID AND T.Status = 'Pending' AND wrk.CreatedBy = loggedUserID And T.WorkspaceID = varWorkspaceID
     AND T.`TypeIDColumn` = 'RISKID' AND IFNULL(T.IsActive,0)=1 AND T.ID NOT IN (
	 SELECT ParentSyncDBID  FROM TaskSyncDB PRNT WHERE PRNT.ParentSyncDBID = T.ID)
	  
	 UNION
	 Select Distinct T.ID , Command, T.`Type` as Type,T.TypeID,T.TypeIDColumn ,Status ,0 AS WorkflowID,TotalWFLevel ,-- -1 as SubmittedWFLevel, -1 as SubmittedUser,
	 0,'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy,now() as TaskDate, DATE_FORMAT(T.CreatedDate, "%Y-%m-%d") as TaskSubmittedDate , 
	 -1 as FinanceWorkflowId , 0 as ReportingValue, 0 as CommittedFund,'' as CommittedBy
	 From TaskSyncDB T 
	 JOIN TbProjectSurvey prjSRY ON prjSRY.SurveyID = T.TypeID
	 JOIN TbProject tbprj ON tbprj.ProjectId = prjSRY.ProjectId 
	 JOIN TbInitiative tbin ON  tbin.InitiativeId = tbprj.InitiativeId
	 JOIN TbWorkspace wrk ON wrk.WorkspaceID = tbin.WorkspaceID
	 WHERE tbprj.ProjectStatus='Deploy' and  T.`Type` = 'Issue' AND T.ID > varSyncDbID AND T.Status = 'Pending' AND wrk.CreatedBy = loggedUserID And T.WorkspaceID = varWorkspaceID
     AND T.`TypeIDColumn` = 'SurveyID' AND IFNULL(T.IsActive,0)=1 AND T.ID NOT IN (
	 SELECT ParentSyncDBID  FROM TaskSyncDB PRNT WHERE PRNT.ParentSyncDBID = T.ID)
	  
    ) As Temp ;

    OPEN curTask;
  read_loop: LOOP
   FETCH curTask INTO varTypeID, varType,varResponseSynID,varSyncDBID1,intDataCollectedBy;   
    IF done THEN
      LEAVE read_loop;
    END IF;
    
    IF varType='Activity'  THEN
    Begin    
            
		Set @varTaskData=( SELECT  JSON_OBJECT( 
			'ActivityId' ,A.ActivityId ,'ProjectId', A.ProjectId,'ProjectName', P.ProjectName ,'ActivityName' , ActivityName,'Description', A.Description,
			'StartDate',A.StartDate,'EndDate', A.EndDate,'WeightagePercentage' , WeightagePercentage, 'ActivityType',ActivityType,'SurveyId', A.SurveyId, 
			'SurveySectionId',SurveySectionId,'SurveyQuestionId', SurveyQuestionId,'SurveyResponseId', SurveyResponseId,'SurveyLogic', SurveyLogic,
			'SurveyTarget',SurveyTarget,'IsPaymentLinked', IsPaymentLinked, 'PaymentLinkedBudget',PaymentLinkedBudget,'WorkflowId', A.WorkflowId, 
            'BeneficiaryTypeID' , BT.BeneficiaryTypeID,'BeneficiaryType',  BT.Name,
			'FinanceWorkflowId',FinanceWorkflowId,'Periodicity', (Case When Periodicity =1 then 'Never'	When Periodicity =2 then 'Once'  When Periodicity =3 then 'Daily'
						When Periodicity =4 then 'Weekly'  When Periodicity =5 then 'Monthly'	When Periodicity =6 then 'Quarterly'
                        When Periodicity =7 then 'Half_Yearly'	When Periodicity =8 then 'Yearly' When Periodicity =9 then 'Anytime' End ) 
                        ,'CreatedDate', A.CreatedDate,
            	'SurveyQstnList', (SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SQ.SurveyID, 'SectionOrder' ,SectionOrder,'SectionID' ,SQ.SectionID, 'SectionName' ,SS.SectionName,
											'QuestionCategory',Category,'QuestionOrder',QuestionOrder, 'QuestionID', SQ.QuestionID, 'QuestionName', SQ.QuestionName,'QuestionTypeID',SQ.QuestionTypeID ,'QuestionType',QT.`Type`,
											'QuestionHint', (Case When QuestionHint ='null' Then null Else QuestionHint End),'DefaultValue' ,  (Case When DefaultValue ='null' Then null Else DefaultValue End),'IsMandatory' ,IsMandatory , 
                                            'IsUnique' ,IsUnique,  'IsSearchable' ,IsSearchable ,'IsFormulaQuestion' , IsFormulaQuestion ,'FormulaExpression',  (Case When FormulaExpression ='null' Then null Else FormulaExpression End),
                                            'FormulaOperation',(Case When FormulaOperation ='null' Then null Else FormulaOperation End),'FormulaFieldVariables', (Case When FormulaFieldVariables ='null' Then null Else FormulaFieldVariables End),
											 'RegEx' , (Case When RegEx ='null' Then null Else RegEx End) , 'ErrorText' ,Case When ErrorText ='null' Then null Else ErrorText End,'FieldVar',FieldVar ,'IsValidationQuestion',IsValidationQuestion,'IsUpload',0,'Answer',IFNULL(SA.Answer,''),'UOM' , UOM,
											 'QuestionCondition',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbProjectSurveyQSTCondition QC Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QC.QuestionID  Where QQ.QuestionID = SQ.QuestionID And  QQ.SurveyID =  A.SurveyId ),
											'QuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',OptionValue )) 
															From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID  Where QQ.QuestionID = SQ.QuestionID And QQ.SurveyID =  A.SurveyId ),
											'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder))  
														From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID 
                                                        Inner Join TbSurveyCascadingQuestionOptions CS On CS.OptionValue =QOpt.OptionValue
														Where QQ.QuestionID = SQ.QuestionID And SQ.QuestionTypeID =8  And QQ.SurveyID =  A.SurveyId ),'IsConditionQuestion',IsConditionQuestion,
											'SectionLogic',(Select JSON_ARRAYAGG(JSON_OBJECT('LogicType',LogicType,'QuestionID',QQ.QuestionID,'ConditionID', ConditionID,'ConditionValue',ConditionValue,'Answer',IFNULL(Answer,''), 'LogicOperator',LogicOperator,'SkipSectionID',SL.SectionID)) 
														From (Select LogicType,QuestionID,ConditionID,`Condition`, ConditionValue,Answer, Case When LogicOperator=0 Then 'AND' When LogicOperator=1 then 'OR' Else '' End as LogicOperator,SectionID From TbProjectSurveySectionLogic SL Left Join TbProjectLogicConditions L On L.ID =SL.ConditionID Where SL.IsActive =1) SL 
														Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = SL.QuestionID  Where QQ.QuestionID =SQ.QuestionID And QQ.SurveyID =  A.SurveyId )
                                            ))
					From TbProjectSurveyQuestion SQ  Inner Join TbProjectActivity A On A.SurveyId =SQ.SurveyId 
                    Inner Join TbProjectSurveySection SS On SS.SectionID =SQ.SectionID And A.SurveyId =SS.SurveyId
					Inner Join TbQuestionType QT On QT.ID = SQ.QuestionTypeID
                    left join (Select * from TbProjectSurveyResponses Where SyncTaskID=varResponseSynID )SA  On SA.QuestionID=SQ.QuestionID And A.SurveyId =SA.SurveyId 
					Where SS.IsActive = 1 And SQ.IsActive =1 And A.ActivityId =  varTypeID), 
				'CascadingList',Case When (Select  EXISTS(Select QuestionID  from TbProjectSurveyQuestion Where QuestionTypeId= 8 and SurveyID=A.SurveyId ) =0) then null  Else 
								(Select JSON_ARRAYAGG(JSON_OBJECT ('Country' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',CountryID,'Name',CountryName,'ParentID',0 )) from TbCountry where CountryID=1  Order By CountryName) ,
									'States' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',StateID,'Name',StateName,'ParentID',CountryID )) from TbStates  Order By StateName) ,
									'District',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',DistrictID,'Name',DistrictName,'ParentID',StateID )) from TbDistrict  Order By DistrictName),
									'SubDistrict',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',SubDistrictID,'Name',SubDistrictName,'ParentID',DistrictID )) from TbSubDistrict  Order By SubDistrictName)  
									 ))) End,
				'BeneficiaryList', ( SELECT JSON_ARRAYAGG(JSON_OBJECT( 	'BeneficiaryTypeID' ,BT.BeneficiaryTypeID ,'BeneficiaryID',BeneficiaryID, 'BeneficiaryName' ,BeneficiaryName, 'CreatedDate' ,CreatedDate ))
									From TbProjectBeneficiary  Where BeneficiaryTypeID = BT.BeneficiaryTypeID And BT.IsActive =1 And IsDeleted =0),
				'BeneficiaryRegistrationQstns', ( SELECT JSON_ARRAYAGG(JSON_OBJECT( 	'BeneficiaryTypeID',BeneficiaryTypeID, 'QuestionID' ,QuestionID , 'QuestionName' ,QuestionName,'QuestionTypeID' ,BTQ.AnswerTypeID,
									'QuestionType' ,BTA.AnswerType,	'IsUnique' ,IsUnique,'IsSearchable',IsSearchable, 'IsMandatoryResponse' ,IsMandatoryResponse, 'QuestionHint' ,QuestionHint ,'RegexExpression' , (Case When RegexExpression ='null' Then null Else RegexExpression End),
                                     'IsValidationQuestion',IsValidationQuestion,
                                     'BTQstnAnsOptionList',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',BTQ1.QuestionID,'OptionID', QstnAnsOptionID,'OptionValue',OptionValue ))
											From TbBeneficiaryTypeQstnAnswerOptions  QOpt Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QOpt.QuestionID  
											Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID),
                                      'BTQstnValidationList',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbBeneficiaryQSTCondition QC Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QC.QuestionID  
                                                                Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID),'IsUpload',0,
                                     'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',BTQ1.QuestionID,'OptionID', QstnAnsOptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder ))
											From TbBeneficiaryTypeQstnAnswerOptions  QOpt Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QOpt.QuestionID  
                                            Inner Join TbBeneficiaryCascadingQstnOptions BCS On BCS.OptionValue =QOpt.OptionValue
											Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID)  
                                     ))
									From TbBeneficiaryTypeQuestion BTQ Inner Join TbBeneficiaryAnswerType BTA On BTA.AnswerTypeID = BTQ.AnswerTypeID
									Where BeneficiaryTypeID =BT.BeneficiaryTypeID And BTQ.IsActive =1 And BTQ.IsDeleted =0),
				'BeneficiarySubmittedList', (SELECT JSON_ARRAYAGG(JSON_OBJECT( 'BeneficiaryTypeID' ,BT.BeneficiaryTypeID ,'BeneficiaryID',BeneficiaryID, 'BeneficiaryName' ,BeneficiaryName, 'CreatedDate' ,B.CreatedDate ,
									'SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'QuestionName', QuestionName,'Answer' ,Answer)) From TbProjectBeneficiary  B 
                                    Inner Join (Select R.*,SQ.QuestionName  from TbProjectSurveyResponses R Inner Join TbProjectSurveyQuestion SQ On SQ.QuestionID  = R.QuestionID   Where SyncTaskID = varResponseSynID) R On R.ResponseID = B.BeneficiaryID
									Where B.BeneficiaryTypeID = BT.BeneficiaryTypeID And BT.IsActive =1 And IsDeleted =0),
                                    'SurveyResponse',(SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'Answer' ,Answer, 'Answer1' ,Answer))
										From TbProjectSurveyResponses R Where R.IsActive =1 and R.SyncTaskID = varResponseSynID and R.ResponseID =0))
					FROM TbProjectActivity A Inner Join TbProject P On A.ProjectId =P.ProjectId
                    inner join  TbProjectSurvey S on S.SurveyID = A.SurveyId      
					Left Join TbProjectBeneficiaryType BT On  BT.BeneficiaryTypeID=S.BeneficiaryTypeID 
					Where A.ActivityId=varTypeID and P.ProjectStatus='Deploy');
		
		-- Set @varTaskData =(SELECT jsonTask); 
		
       -- Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;
	End ;
    END IF;
	IF varType='Payment'  THEN
    Begin    
            
		Set @varTaskData=(SELECT  JSON_OBJECT( 
			'ActivityId' ,ActivityId ,'ProjectId', A.ProjectId,'ProjectName', P.ProjectName ,'ActivityName' , ActivityName,'Description', A.Description,
			'StartDate',A.StartDate,'EndDate', A.EndDate,'WeightagePercentage' , WeightagePercentage, 'ActivityType',ActivityType,'SurveyId', SurveyId, 
			'SurveySectionId',SurveySectionId,'SurveyQuestionId', SurveyQuestionId,'SurveyResponseId', SurveyResponseId,'SurveyLogic', SurveyLogic,
			'SurveyTarget',SurveyTarget,'IsPaymentLinked', IsPaymentLinked, 'PaymentLinkedBudget',PaymentLinkedBudget,'WorkflowId', WorkflowId, 
			'FinanceWorkflowId',FinanceWorkflowId,'Periodicity', (Case When Periodicity =1 then 'Never'	When Periodicity =2 then 'Once'  When Periodicity =3 then 'Daily'
						When Periodicity =4 then 'Weekly'  When Periodicity =5 then 'Monthly'	When Periodicity =6 then 'Quarterly'
                        When Periodicity =7 then 'Half_Yearly'	When Periodicity =8 then 'Yearly' When Periodicity =9 then 'Anytime' End ) 
                        ,'CreatedDate', A.CreatedDate, 'intCreatedBy', A.CreatedBy, 'vaCreatedBy',(select Name from TbUser Where ID = A.CreatedBy),
			'TaskDate', (SELECT JSON_ARRAYAGG(JSON_OBJECT( 'ActivityId' , A1.ActivityId,'TrackDate',DATE_FORMAT(ADate.TrackDate, "%Y-%m-%d"),'PeriodicityId',ADate.ID)) FROM TbProjectActivity A1 
							inner Join TbProjectActivityTrackDate ADate On A1.ActivityId =ADate.ActivityId
                            inner join TaskSyncDB  TaskSyncDB on ADate.ActivityId = TaskSyncDB.TypeID
                            Where A1.ActivityId=A.ActivityId and ADate.TrackDate <=now() and TaskSyncDB.Command= 'ADD'
                            and TaskSyncDB.TypeID not in (select TypeID from TbMyTask where TypeID = TaskSyncDB.TypeID and TaskDate = ADate.TrackDate and UserId = loggedUserID )),
            'SurveyQstnList', (SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SQ.SurveyID, 'SectionOrder' ,SectionOrder,'SectionID' ,SQ.SectionID, 'SectionName' ,SS.SectionName,
											'QuestionCategory',Category,'QuestionOrder',QuestionOrder, 'QuestionID', SQ.QuestionID, 'QuestionName', SQ.QuestionName,'QuestionTypeID',SQ.QuestionTypeID ,'QuestionType',QT.`Type`,
											'QuestionHint', (Case When QuestionHint ='null' Then null Else QuestionHint End),'DefaultValue' ,  (Case When DefaultValue ='null' Then null Else DefaultValue End),'IsMandatory' ,IsMandatory , 
                                            'IsUnique' ,IsUnique,  'IsSearchable' ,IsSearchable ,'IsFormulaQuestion' , IsFormulaQuestion ,'FormulaExpression',  (Case When FormulaExpression ='null' Then null Else FormulaExpression End),
                                            'FormulaOperation',(Case When FormulaOperation ='null' Then null Else FormulaOperation End),'FormulaFieldVariables', (Case When FormulaFieldVariables ='null' Then null Else FormulaFieldVariables End),
											 'RegEx' , (Case When RegEx ='null' Then null Else RegEx End) , 'ErrorText' ,Case When ErrorText ='null' Then null Else ErrorText End,'FieldVar',FieldVar ,'IsValidationQuestion',IsValidationQuestion,'IsUpload',0,'Answer',SA.Answer,'UOM' , UOM,
											 'QuestionCondition',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbProjectSurveyQSTCondition QC Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QC.QuestionID  Where QQ.QuestionID = SQ.QuestionID And  QQ.SurveyID =  A.SurveyId ),
											'QuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',OptionValue )) 
															From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID  Where QQ.QuestionID = SQ.QuestionID And QQ.SurveyID =  A.SurveyId ),
											'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder))  
														From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID 
                                                        Inner Join TbSurveyCascadingQuestionOptions CS On CS.OptionValue =QOpt.OptionValue
														Where QQ.QuestionID = SQ.QuestionID And SQ.QuestionTypeID =8  And QQ.SurveyID =  A.SurveyId ),'IsConditionQuestion',IsConditionQuestion,
											'SectionLogic',(Select JSON_ARRAYAGG(JSON_OBJECT('LogicType',LogicType,'QuestionID',QQ.QuestionID,'ConditionID', ConditionID,'ConditionValue',ConditionValue,'Answer',Answer, 'LogicOperator',LogicOperator,'SkipSectionID',SL.SectionID)) 
														From (Select LogicType,QuestionID,ConditionID,`Condition`, ConditionValue,Answer, Case When LogicOperator=0 Then 'AND' When LogicOperator=1 then 'OR' Else '' End as LogicOperator,SectionID From TbProjectSurveySectionLogic SL Left Join TbProjectLogicConditions L On L.ID =SL.ConditionID Where IsActive =1) SL 
														Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = SL.QuestionID  Where QQ.QuestionID =SQ.QuestionID And QQ.SurveyID =  A.SurveyId )
                                            ))
					From TbProjectSurveyQuestion SQ  Inner Join TbProjectActivity A On A.SurveyId =SQ.SurveyId 
                    Inner Join TbProjectSurveySection SS On SS.SectionID =SQ.SectionID And A.SurveyId =SS.SurveyId
					Inner Join TbQuestionType QT On QT.ID = SQ.QuestionTypeID
                    left join (Select * from TbProjectSurveyResponses Where SyncTaskID=varResponseSynID )SA  On SA.QuestionID=SQ.QuestionID And A.SurveyId =SA.SurveyId 
					Where SS.IsActive = 1 And SQ.IsActive =1 And A.ActivityId =  varTypeID), 
				'CascadingList',Case When (Select  EXISTS(Select QuestionID  from TbProjectSurveyQuestion Where QuestionTypeId= 8 and SurveyID=A.SurveyId ) =0) then null  Else 
								(Select JSON_ARRAYAGG(JSON_OBJECT ('Country' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',CountryID,'Name',CountryName,'ParentID',0 )) from TbCountry where CountryID=1 Order By CountryName) ,
									'States' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',StateID,'Name',StateName,'ParentID',CountryID )) from TbStates  Order By StateName) ,
									'District',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',DistrictID,'Name',DistrictName,'ParentID',StateID )) from TbDistrict  Order By DistrictName),
									'SubDistrict',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',SubDistrictID,'Name',SubDistrictName,'ParentID',DistrictID )) from TbSubDistrict  Order By SubDistrictName)  
									 ))) End)
		FROM TbProjectActivity A Inner Join TbProject P On A.ProjectId =P.ProjectId         
		Where ActivityId=varTypeID and P.ProjectStatus='Deploy');
		
		-- Set @varTaskData =(SELECT jsonTask); 
		
       -- Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;
	End ;
    END IF;
    IF varType='Payment Request'  THEN
    Begin 
		Set @varTaskData=( SELECT  JSON_OBJECT( 
			'REQUESTID' ,TbProjectRequestFund.ID ,'ProjectId', TbProject.ProjectId,'ProjectName', TbProject.ProjectName ,'PaymentTerm' , TbPaymentTerms.PaymentTermName,
            'Description', TbProjectRequestFund.Comment,
			'StartDate',TbProject.StartDate,'EndDate', TbProject.EndDate, 
			'CreatedDate', DATE_FORMAT(TbProjectRequestFund.CreatedDate, "%Y-%m-%d"),
            'AdvanceAmount' , TbPaymentTerms.Amount , 'RequestedAmount', TbProjectRequestFund.RequestAmount,
            'RequestedBy',(select Name from TbUser Where ID = TbProjectRequestFund.CreatedBy),
            'Condition', Case when TbPaymentTerms.Logic = 'Date' Then CONCAT('Project Target Date-', DATE_FORMAT(TbPaymentTerms.paymentdate, "%d-%m-%Y")) 
            ELSE  CONCAT('Project Target-', TbPaymentTerms.Target,'%') END
            ) 
		FROM TbProjectRequestFund TbProjectRequestFund 
        Inner Join TbPaymentTerms TbPaymentTerms On TbProjectRequestFund.PaymentID = TbPaymentTerms.ID
        Inner Join TbProject TbProject On TbProjectRequestFund.ProjectId =TbProject.ProjectId      
		Where TbProjectRequestFund.ID=varTypeID and TbProject.ProjectStatus='Deploy');
		
		-- Set @varTaskData =(SELECT jsonTask); 
		
       -- Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;
	End ;
    END IF;
    IF varType='Fund Request'  THEN
    Begin 
		Set @varTaskData=( SELECT  JSON_OBJECT( 
			'RQSTID' ,BudgetAndFundDetails.ID ,'ProjectId', TbProject.ProjectId,'ProjectName', TbProject.ProjectName ,
            'Description', TbProject.Description,
            'intDataCollectedBy',intDataCollectedBy,
			'StartDate',TbProject.StartDate,'EndDate', TbProject.EndDate, 
			'CreatedDate', DATE_FORMAT(TbProject.CreatedDate, "%Y-%m-%d"),
            'OtherDonors', (select GROUP_CONCAT(distinct Name ORDER BY Name ASC SEPARATOR ', ') from TbUser TbUser inner join TbProjectBudgetAndFundDetails TbProjectBudgetAndFundDetails
            on TbUser.ID = TbProjectBudgetAndFundDetails.UserID where UserID <> intDataCollectedBy 
            and TbProjectBudgetAndFundDetails.ProjectId = varTypeID and TbProjectBudgetAndFundDetails.UserType='D'),
            'OtherDonorFund',(select Sum(TbProjectBudgetAndFundDetails.DonorBudgetAmount) from TbProjectBudgetAndFundDetails 
            TbProjectBudgetAndFundDetails where TbProjectBudgetAndFundDetails.UserID <> intDataCollectedBy 
            and TbProjectBudgetAndFundDetails.ProjectId = varTypeID  and TbProjectBudgetAndFundDetails.UserType='D'),
			'DonorAccount', (select AccountName from TbAccount where AccountId = BudgetAndFundDetails.AccountID),
            'TotalBudget' , TbProject.Budget,
            'FundedAmount' , BudgetAndFundDetails.DonorBudgetAmount
            ) 
		From TbProject TbProject  inner join TbProjectBudgetAndFundDetails BudgetAndFundDetails on BudgetAndFundDetails.ProjectID = TbProject.ProjectId
        Where TbProject.ProjectStatus='Deploy' and TbProject.ProjectId=varTypeID and BudgetAndFundDetails.UserID = intDataCollectedBy and BudgetAndFundDetails.IsDeleted = 0);
		
		-- Set @varTaskData =(SELECT jsonTask); 
		
       -- Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;
	End ;
    END IF;
    IF varType='RISK'  THEN
		Begin        
			Set @varTaskData=(Select JSON_OBJECT(
			'RISKID' ,ID ,'ProjectId', A.ProjectId,'ProjectName', P.ProjectName ,'RiskName' , RiskName,'Description', A.Description,
			'StartDate',A.StartDate,'EndDate', A.EndDate, 'SurveyId', 0, 'WorkflowId', A.WorkflowID,  'CreatedDate', A.CreatedDate,
            'Periodicity', (Case When RiskPeriodicity =1 then 'Never'	When RiskPeriodicity =2 then 'Once'  When RiskPeriodicity =3 then 'Daily'
						When RiskPeriodicity =4 then 'Weekly'  When RiskPeriodicity =5 then 'Monthly'	When RiskPeriodicity =6 then 'Quarterly'
                        When RiskPeriodicity =7 then 'Half_Yearly'	When RiskPeriodicity =8 then 'Yearly' When RiskPeriodicity =9 then 'Anytime' End ), 
             'SurveyQstnList','','ReportingValue', A.RiskRating
             -- 'UpdatedRepotingValue' , (select RiskReportingValue from TbMyTask where SyncTaskID = )
             
            ) FROM TbRisk A Inner Join TbProject P On A.ProjectId =P.ProjectId         
		Where A.ID=varTypeID and P.ProjectStatus='Deploy');
			
                                    
		End;
    End IF ;
    IF varType='Outcome'  THEN
		Begin
        Set @varTaskData=(Select JSON_OBJECT(
			'OutcomeID' ,pk_OutcomeID ,'ProjectId', A.ProjectId,'ProjectName', P.ProjectName ,'OutcomeName' , OutcomeName,'Description', '',
			'StartDate',A.StartDate,'EndDate', A.EndDate, 'SurveyId', A.SurveyId, 'WorkflowId', A.WorkflowId,  'CreatedDate', A.CreatedDate,
            'SurveyTarget',SurveyTarget,'BeneficiaryTypeID' , BT.BeneficiaryTypeID,'BeneficiaryType',  BT.Name,'Periodicity', (Case When Periodicity =1 then 'Never'	When Periodicity =2 then 'Once'  When Periodicity =3 then 'Daily'
						When Periodicity =4 then 'Weekly'  When Periodicity =5 then 'Monthly'	When Periodicity =6 then 'Quarterly'
                        When Periodicity =7 then 'Half_Yearly'	When Periodicity =8 then 'Yearly' When Periodicity =9 then 'Anytime' End ), 
             	'SurveyQstnList', (SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SQ.SurveyID, 'SectionOrder' ,SectionOrder,'SectionID' ,SQ.SectionID, 'SectionName' ,SS.SectionName,
											'QuestionCategory',Category,'QuestionOrder',QuestionOrder, 'QuestionID', SQ.QuestionID, 'QuestionName', SQ.QuestionName,'QuestionTypeID',SQ.QuestionTypeID ,'QuestionType',QT.`Type`,
											'QuestionHint', (Case When QuestionHint ='null' Then null Else QuestionHint End),'DefaultValue' ,  (Case When DefaultValue ='null' Then null Else DefaultValue End),'IsMandatory' ,IsMandatory , 
                                            'IsUnique' ,IsUnique,  'IsSearchable' ,IsSearchable ,'IsFormulaQuestion' , IsFormulaQuestion ,'FormulaExpression',  (Case When FormulaExpression ='null' Then null Else FormulaExpression End),
                                            'FormulaOperation',(Case When FormulaOperation ='null' Then null Else FormulaOperation End),'FormulaFieldVariables', (Case When FormulaFieldVariables ='null' Then null Else FormulaFieldVariables End),
											 'RegEx' , (Case When RegEx ='null' Then null Else RegEx End) , 'ErrorText' ,Case When ErrorText ='null' Then null Else ErrorText End,'FieldVar',FieldVar ,'IsValidationQuestion',IsValidationQuestion,'IsUpload',0,'Answer',SA.Answer,'UOM' , UOM,
											 'QuestionCondition',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbProjectSurveyQSTCondition QC Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QC.QuestionID  Where QQ.QuestionID = SQ.QuestionID And  QQ.SurveyID =  A.SurveyId ),
											'QuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',OptionValue )) 
															From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID  Where QQ.QuestionID = SQ.QuestionID And QQ.SurveyID =  A.SurveyId ),
											'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder))  
														From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID 
                                                        Inner Join TbSurveyCascadingQuestionOptions CS On CS.OptionValue =QOpt.OptionValue
														Where QQ.QuestionID = SQ.QuestionID And SQ.QuestionTypeID =8  And QQ.SurveyID =  A.SurveyId ),'IsConditionQuestion',IsConditionQuestion,
											'SectionLogic',(Select JSON_ARRAYAGG(JSON_OBJECT('LogicType',LogicType,'QuestionID',QQ.QuestionID,'ConditionID', ConditionID,'ConditionValue',ConditionValue,'Answer',Answer, 'LogicOperator',LogicOperator,'SkipSectionID',SL.SectionID)) 
														From (Select LogicType,QuestionID,ConditionID,`Condition`, ConditionValue,Answer, Case When LogicOperator=0 Then 'AND' When LogicOperator=1 then 'OR' Else '' End as LogicOperator,SectionID From TbProjectSurveySectionLogic SL Left Join TbProjectLogicConditions L On L.ID =SL.ConditionID Where SL.IsActive =1) SL 
														Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = SL.QuestionID  Where QQ.QuestionID =SQ.QuestionID And QQ.SurveyID =  A.SurveyId )
                                            ))
					From TbProjectSurveyQuestion SQ  Inner Join TbOutcomes A On A.SurveyId =SQ.SurveyID
                    Inner Join TbProjectSurveySection SS On SS.SectionID =SQ.SectionID And A.SurveyId =SS.SurveyID
					Inner Join TbQuestionType QT On QT.ID = SQ.QuestionTypeID
                    left join (Select * from TbProjectSurveyResponses Where SyncTaskID=varResponseSynID )SA  On SA.QuestionID=SQ.QuestionID And A.SurveyId =SA.SurveyID 
					Where SS.IsActive = 1 And SQ.IsActive =1 And A.pk_OutcomeID =  varTypeID),   
               'CascadingList',Case When (Select  EXISTS(Select QuestionID  from TbProjectSurveyQuestion Where QuestionTypeId= 8 and SurveyID=A.SurveyId ) =0) then null  Else 
								(Select JSON_ARRAYAGG(JSON_OBJECT ('Country' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',CountryID,'Name',CountryName,'ParentID',0 )) from TbCountry where CountryID=1  Order By CountryName) ,
									'States' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',StateID,'Name',StateName,'ParentID',CountryID )) from TbStates  Order By StateName) ,
									'District',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',DistrictID,'Name',DistrictName,'ParentID',StateID )) from TbDistrict  Order By DistrictName),
									'SubDistrict',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',SubDistrictID,'Name',SubDistrictName,'ParentID',DistrictID )) from TbSubDistrict  Order By SubDistrictName)  
									 ))) End,
                 'BeneficiaryList', ( SELECT JSON_ARRAYAGG(JSON_OBJECT( 	'BeneficiaryTypeID' ,BT.BeneficiaryTypeID ,'BeneficiaryID',BeneficiaryID, 'BeneficiaryName' ,BeneficiaryName, 'CreatedDate' ,CreatedDate ))
									From TbProjectBeneficiary  Where BeneficiaryTypeID = BT.BeneficiaryTypeID And BT.IsActive =1 And IsDeleted =0),
                 'BeneficiaryRegistrationQstns', ( SELECT JSON_ARRAYAGG(JSON_OBJECT( 	'BeneficiaryTypeID',BeneficiaryTypeID, 'QuestionID' ,QuestionID , 'QuestionName' ,QuestionName,'QuestionTypeID' ,BTQ.AnswerTypeID,
									'QuestionType' ,BTA.AnswerType,	'IsUnique' ,IsUnique,'IsSearchable',IsSearchable, 'IsMandatoryResponse' ,IsMandatoryResponse, 'QuestionHint' ,QuestionHint 
                                     ,'RegexExpression' , (Case When RegexExpression ='null' Then null Else RegexExpression End),'IsValidationQuestion',IsValidationQuestion,
                                     'BTQstnAnsOptionList',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',BTQ1.QuestionID,'OptionID', QstnAnsOptionID,'OptionValue',OptionValue ))
											From TbBeneficiaryTypeQstnAnswerOptions  QOpt Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QOpt.QuestionID  
											Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID),
                                     'BTQstnValidationList',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbBeneficiaryQSTCondition QC Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QC.QuestionID  
                                                                Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID),'IsUpload',0,
                                      'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',BTQ1.QuestionID,'OptionID', QstnAnsOptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder ))
											From TbBeneficiaryTypeQstnAnswerOptions  QOpt Inner Join  TbBeneficiaryTypeQuestion BTQ1  On BTQ1.QuestionID = QOpt.QuestionID  
                                            Inner Join TbBeneficiaryCascadingQstnOptions BCS On BCS.OptionValue =QOpt.OptionValue
											Where BTQ1.QuestionID = BTQ.QuestionID And BTQ1.BeneficiaryTypeID=BT.BeneficiaryTypeID)                           
                                    ))
									From TbBeneficiaryTypeQuestion BTQ Inner Join TbBeneficiaryAnswerType BTA On BTA.AnswerTypeID = BTQ.AnswerTypeID
									Where BeneficiaryTypeID =BT.BeneficiaryTypeID And BTQ.IsActive =1 And BTQ.IsDeleted =0),
                'BeneficiarySubmittedList', (SELECT JSON_ARRAYAGG(JSON_OBJECT( 'BeneficiaryTypeID' ,BT.BeneficiaryTypeID ,'BeneficiaryID',BeneficiaryID, 'BeneficiaryName' ,BeneficiaryName, 'CreatedDate' ,B.CreatedDate ,
									'SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'QuestionName', QuestionName,'Answer' ,Answer)) From TbProjectBeneficiary  B 
                                    Inner Join (Select R.*,SQ.QuestionName  from TbProjectSurveyResponses R Inner Join TbProjectSurveyQuestion SQ On SQ.QuestionID  = R.QuestionID   Where SyncTaskID = varResponseSynID) R On R.ResponseID = B.BeneficiaryID
									Where B.BeneficiaryTypeID = BT.BeneficiaryTypeID And BT.IsActive =1 And IsDeleted =0),
                                    'SurveyResponse',(SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'Answer' ,Answer, 'Answer1' ,Answer))
										From TbProjectSurveyResponses R Where R.IsActive =1 and R.SyncTaskID = varResponseSynID and R.ResponseID =0))
                                    FROM TbOutcomes A Inner Join TbProject P On A.ProjectId =P.ProjectId 
                                    inner join  TbProjectSurvey S on S.SurveyID = A.SurveyId      
									Left Join TbProjectBeneficiaryType BT On  BT.BeneficiaryTypeID=S.BeneficiaryTypeID 
									Where A.pk_OutcomeID=varTypeID and P.ProjectStatus='Deploy');  
        End;
    End IF ;
      IF varType='Project'  THEN
		Begin  
        
         Set @varTaskData=(Select JSON_OBJECT(
            'SyncDb' , varSyncDBID1,
             'intDataCollectedBy', intDataCollectedBy,
			'ID' ,0 ,'ProjectId', P.ProjectId,'ProjectName', P.ProjectName ,'Description', P.Description,
			'StartDate',P.StartDate,'EndDate', P.EndDate,   'CreatedDate', DATE_FORMAT(P.CreatedDate, "%Y-%m-%d") ,
            'Budget',P.Budget,'CreatedBy',(select ifnull(`Name`,'') Name from TbUser usr where  usr.ID = P.CreatedBy),
            'OtherUser',(SELECT GROUP_CONCAT(usr.Name) from TbUser usr JOIN TbInitiativeToUsers intusr ON 
            intusr.UserID = usr.ID WHERE InitiativeId = P.InitiativeId GROUP BY InitiativeId),
            'WorkSpaceId',(select WorkspaceID from TbInitiative where  InitiativeId = P.InitiativeId )
            ) FROM  TbProject P Where P.ProjectId=varTypeID and P.ProjectStatus='Deploy');
        
		End;
    End IF ;
    
     IF varType='Issue'  THEN
		Begin  
            Set @varTaskData=(Select JSON_OBJECT(
			'TaskId',SNC.TypeId, 'CreatedDate', DATE_FORMAT(P.TaskDate, "%Y-%m-%d") ,
			'CreatedBy',(select ifnull(`Name`,'') Name from TbUser usr where  usr.ID = P.CreatedBy),
            'ReportedIssue',P.TaskUpdateComment,
			 'TaskName',(select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select ActivityName from TbProjectActivity where ActivityId = SNC.TypeID  )
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select OutcomeName from TbOutcomes where pk_OutcomeID = SNC.TypeID )
              when SNC.TypeIDColumn = 'RISKID' then (select RiskName from TbRisk where ID= SNC.TypeID )
			  when SNC.TypeIDColumn = 'SurveyID' then (select SurveyName from TbProjectSurvey where SurveyID= SNC.TypeID)
              END
             )),'TaskDescription',
             (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select Description from TbProjectActivity where ActivityId = SNC.TypeID  )
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select '' from TbOutcomes where pk_OutcomeID = SNC.TypeID)
              when SNC.TypeIDColumn = 'RISKID' then (select Description from TbRisk where ID= SNC.TypeID)
			  when SNC.TypeIDColumn = 'SurveyID' then (select SurveyDescription from TbProjectSurvey where SurveyID= SNC.TypeID)
              END
             )),
             'ProjectName',
             (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select PRJ.ProjectName from TbProject PRJ JOIN TbProjectActivity ACT ON PRJ.ProjectId = ACT.ProjectId  where ActivityId = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select PRJ.ProjectName from TbProject PRJ JOIN TbOutcomes ACT ON PRJ.ProjectId = ACT.ProjectID  where pk_OutcomeID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'RISKID' then (select PRJ.ProjectName from TbProject PRJ JOIN TbRisk ACT ON PRJ.ProjectId = ACT.ProjectID  where ID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
			  when SNC.TypeIDColumn = 'SurveyID' then (select PRJ.ProjectName from TbProject PRJ JOIN TbProjectSurvey ACT ON PRJ.ProjectId = ACT.ProjectId  where ACT.SurveyID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
             END
             )),
             'Periodicity',
             (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select PRD.`PeriodicityType` from TbPeriodicity PRD JOIN TbProjectActivity ACT ON PRD.PeriodicityId = ACT.Periodicity  where ActivityId = SNC.TypeID)
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select PRD.`PeriodicityType` from TbPeriodicity PRD JOIN TbOutcomes ACT ON  PRD.PeriodicityId= ACT.Periodicity  where pk_OutcomeID = SNC.TypeID)
              when SNC.TypeIDColumn = 'RISKID' then (select PRD.`PeriodicityType` from TbPeriodicity PRD JOIN TbRisk ACT ON PRD.PeriodicityId = ACT.RiskPeriodicity  where ID = SNC.TypeID)
			  when SNC.TypeIDColumn = 'SurveyID' then (select PRD.`PeriodicityType` from TbPeriodicity PRD JOIN TbProjectSurvey ACT ON PRD.PeriodicityId = ACT.ReportingFrequency  where ACT.SurveyID = SNC.TypeID)
              END
             )),
           'WorkSpaceId', (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select TBINT.WorkspaceID FROM TbProjectActivity ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId
			  JOIN TbInitiative TBINT ON TBINT.InitiativeId = PRJ.InitiativeId where ACT.ActivityId = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select TBINT.WorkspaceID FROM TbOutcomes ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId
			  JOIN TbInitiative TBINT ON TBINT.InitiativeId = PRJ.InitiativeId where ACT.pk_OutcomeID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'RISKID' then (select TBINT.WorkspaceID FROM TbRisk ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId
			  JOIN TbInitiative TBINT ON TBINT.InitiativeId = PRJ.InitiativeId where ACT.ID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'SurveyID' then (select TBINT.WorkspaceID FROM TbProjectSurvey ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId
			  JOIN TbInitiative TBINT ON TBINT.InitiativeId = PRJ.InitiativeId where ACT.SurveyID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              END
             )),
           'InitiativeId', (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select PRJ.InitiativeId FROM 
              TbProjectActivity ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId where ACT.ActivityId = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select PRJ.InitiativeId FROM 
			  TbOutcomes ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId where ACT.pk_OutcomeID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              when SNC.TypeIDColumn = 'RISKID' then (select PRJ.InitiativeId FROM 
			  TbRisk ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId where ACT.ID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
			  when SNC.TypeIDColumn = 'SurveyID' then (select PRJ.InitiativeId FROM 
			  TbProjectSurvey ACT JOIN TbProject PRJ ON ACT.ProjectId = PRJ.ProjectId where ACT.SurveyID = SNC.TypeID and PRJ.ProjectStatus='Deploy')
              END
             )),
           'ProjID', (select ( 
              case when SNC.TypeIDColumn = 'ActivityId' then (select ACT.ProjectID from TbProjectActivity ACT  where ACT.ActivityId = SNC.TypeID)
              when SNC.TypeIDColumn = 'pk_OutcomeID' then (select  ACT.ProjectID from TbOutcomes ACT where ACT.pk_OutcomeID = SNC.TypeID)
              when SNC.TypeIDColumn = 'RISKID' then (select ACT.ProjectID from TbRisk ACT where ACT.ID = SNC.TypeID)
			  when SNC.TypeIDColumn = 'SurveyID' then (select ACT.ProjectId from TbProjectSurvey ACT where ACT.SurveyID = SNC.TypeID)
              END
             )),'TypeIDColumnName',SNC.TypeIdColumn
            ) FROM  TbMyTask P 
            JOIN TaskSyncDB SNC ON P.SyncTaskID  = SNC.ID
            Where SNC.ID = varSyncDBID1  and P.TypeID = varTypeID and P.Type = 'Issue' and ifnull(SNC.ParentSyncDBID,0)=0);
        -- SNC.ID = varSyncDBID1
		End;
    End IF ;
    
    Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID AND SyncDBID = varSyncDBID1;
   
  END LOOP; 
  CLOSE curTask;
Select A.*,varWorkspaceID WorkspaceID from tempMyTask A Order By A.SyncDBID desc,A.TaskDate DESC;
END$$
DELIMITER ;