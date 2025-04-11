USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_GetMyTaskList_Survey`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_GetMyTaskList_Survey`(IN loggedUserID int(10),IN SyncDbID int(10),IN  varWorkspaceID  int(10))
BEGIN
	DECLARE done INT DEFAULT FALSE;
	DECLARE varTaskData  json DEFAULT NULL; 	
	DECLARE jsonTask json DEFAULT NULL; 	   
    
	DECLARE varType  varchar(50);
	DECLARE varTypeID  INT;	
	DECLARE varResponseSynID  INT;     
    
    
	DECLARE curTask CURSOR FOR SELECT TypeID,Type,ResponseSyncID FROM tempMyTask;  
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    
	-- DROP TABLE tempMyTask;
	CREATE TEMPORARY TABLE IF NOT EXISTS tempMyTask
	(SyncDBID Int,Command varchar(100),Type varchar(100),TypeID INT,TypeIDColumn varchar(100) , Status varchar(100), WorkflowID Int, TotalWFLevel Int, -- SubmittedWFLevel INT, SubmittedUser Int,	
		WFLevel Int,WFLevelStatus varchar(100) ,ResponseSyncID INT ,DataCollectedBy Int,TaskDate DateTime, TaskSubmittedDate DateTime,FinanceWorkflowId INT ,ReportingValue INT,
        CommittedFund INT ,CommittedBy varchar(200),TaskData json);

	TRUNCATE TABLE tempMyTask;  	    
	INSERT INTO tempMyTask  
    Select Temp.*, null,0,0,null,'[{}]' From (
	Select Distinct T.ID , Command,'Survey',T.TypeID,T.TypeIDColumn ,Status ,WorkflowID,TotalWFLevel, -- -1 as SubmittedWFLevel, -1 as SubmittedUser ,
    0 as WFLevel, 'Pending' WFLevelStatus , -1 ResponseSyncID ,-1 as DataCollectedBy ,TaskDate ,null TaskSubmittedDate 
	From TaskSyncDB T 
	Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID= loggedUserID ) TA On  T.TypeID = TA.TypeID 
	Inner Join (Select Distinct  A.SurveyID TypeID   ,ReportingFrequency,TaskDate,EndDate From (Select S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate,
				Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate,Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate 
				From TbProjectSurvey S Inner Join TbProject P On P.ProjectId= S.ProjectId Left Join (Select * from TbProjectSurveyTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1) A
				Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID 
				Where ifnull(T.IsActive,0) =1 And binary T.Type= 'Survey' And T.Command = 'ADD' And T.Status !='Completed' And ID > SyncDbID   And T.WorkspaceID = varWorkspaceID 
			And
              /*  case when TD.ReportingFrequency = 9 then
                 T.TypeID Not In (Select TypeID From TbMyTask  Where ifnull(IsActive,0) =1 And Type = 'Survey'  And  WFLevelStatus = 'Completed'  And  WFLevel =0 And UserID = AssgUserID And TaskDate =TD.TaskDate and TD.EndDate = date(now())) 
                 else
			     T.TypeID Not In (Select TypeID From TbMyTask  Where ifnull(IsActive,0) =1 And Type = 'Survey'  And  WFLevelStatus = 'Completed'  And  WFLevel =0 And UserID = AssgUserID And TaskDate =TD.TaskDate)         
                 end  
		     */
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
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus, ResponseSyncID,T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate
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
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where ifnull(T.IsActive,0) =1 And TotalWFLevel >0 And Command= 'TaskResponse' And T.Status != 'Completed'  And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID And W.WFLevel=(MT.WFLevel +1) And W.AssgUserID =loggedUserID And T.Type ='Survey'
    AND srv.ReportingFrequency != 9 
    
    UNION
    
     Select Distinct T.ID , Command, T.Type,T.TypeID,T.TypeIDColumn ,Status ,T.WorkflowID,TotalWFLevel ,-- MT.WFLevel SubmittedWFLevel,	MT.UserID SubmittedUser ,
			(MT.WFLevel +1) as WFLevel  , 'Pending' WFLevelStatus,case when ifnull(MT.DataCollectionSyncDBID,T.ID) = 0 then T.ID else ifnull(MT.DataCollectionSyncDBID,T.ID) end AS ResponseSyncID, T.DataCollectedBy ,MT.TaskDate ,MT.TaskSubmittedDate
	From TaskSyncDB T 	
	Inner Join TbMyTask MT  On MT.SyncTaskID  =T.ID 
    inner join TbProjectSurvey srv ON  MT.TypeID = srv.SurveyID
	Left Join  (Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted =0  )   W On W.WorkflowID = T.WorkflowID
	Where ifnull(T.IsActive,0) =1 And TotalWFLevel >0 And Command= 'TaskResponse' And T.Status != 'Completed'  And T.ID > SyncDbID  And T.WorkspaceID =varWorkspaceID And W.WFLevel=(MT.WFLevel +1) And W.AssgUserID =loggedUserID And T.Type ='Survey'
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
							When ReportingFrequency =7 then 'Half_Yearly'	When ReportingFrequency =8 then 'Yearly' When ReportingFrequency =9 then 'Anytime' End ) ,'CreatedDate', A.CreatedDate,
				'SurveyQstnList', (SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SQ.SurveyID, 'SectionOrder' ,SectionOrder,'SectionID' ,SQ.SectionID, 'SectionName' ,SS.SectionName,
											'QuestionCategory',Category,'QuestionOrder',QuestionOrder, 'QuestionID', SQ.QuestionID, 'QuestionName', SQ.QuestionName,'QuestionTypeID',SQ.QuestionTypeID ,'QuestionType',QT.`Type`,
											'QuestionHint', (Case When QuestionHint ='null' Then null Else QuestionHint End),'DefaultValue' ,  (Case When DefaultValue ='null' Then null Else DefaultValue End),'IsMandatory' ,IsMandatory , 
                                            'IsUnique' ,IsUnique,  'IsSearchable' ,IsSearchable ,'IsFormulaQuestion' , IsFormulaQuestion ,'FormulaExpression',  (Case When FormulaExpression ='null' Then null Else FormulaExpression End),
                                            'FormulaOperation',(Case When FormulaOperation ='null' Then null Else FormulaOperation End),'FormulaFieldVariables', (Case When FormulaFieldVariables ='null' Then null Else FormulaFieldVariables End),
											 'RegEx' , (Case When RegEx ='null' Then null Else RegEx End) , 'ErrorText' ,Case When ErrorText ='null' Then null Else ErrorText End,'FieldVar',FieldVar ,'IsValidationQuestion',IsValidationQuestion,'IsUpload',0,
                                              'Answer',  (Case When DefaultValue ='null' Then  SA.Answer Else DefaultValue End) ,'Answer1', SA.Answer,'UOM' , UOM,'IsRepeatSection',IsRepeatSection,
											 'QuestionCondition',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QC.QuestionID,'ValidationCondition', QC.ValidationCondition,
																'MinValue',QC.MinValue,'MaxValue',QC.`MaxValue`,'ConditionValue',ConditionValue,'LogicOperator',LogicOperator )) 
																From TbProjectSurveyQSTCondition QC Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QC.QuestionID  Where QQ.QuestionID = SQ.QuestionID And  QQ.SurveyID =  varTypeID),
											'QuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',OptionValue, 'IsOptSelected' , false )) 
															From TbProjectSurveyQSTOption   QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID  
                                                            Where QQ.QuestionID = SQ.QuestionID And QQ.SurveyID =  varTypeID order by OptionValue Asc),
											'CascadingQuestionOptions',(Select JSON_ARRAYAGG(JSON_OBJECT('QuestionID',QQ.QuestionID,'OptionID', OptionID,'OptionValue',QOpt.OptionValue,'OptionGroupID',OptionGroupID,'HierarchyOrder',HierarchyOrder))  
														From TbProjectSurveyQSTOption  QOpt Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = QOpt.QuestionID 
                                                        Inner Join TbSurveyCascadingQuestionOptions CS On CS.OptionValue =QOpt.OptionValue
														Where QQ.QuestionID = SQ.QuestionID And SQ.QuestionTypeID =8  And QQ.SurveyID =  varTypeID),'IsConditionQuestion',IsConditionQuestion,
											'SectionLogic',(Select JSON_ARRAYAGG(JSON_OBJECT('LogicType',LogicType,'QuestionID',QQ.QuestionID,'ConditionID', ConditionID,'ConditionValue',ConditionValue,'Answer',Answer, 'LogicOperator',LogicOperator,'SkipSectionID',SL.SectionID)) 
														From (Select LogicType,QuestionID,ConditionID,`Condition`, ConditionValue,Answer, Case When LogicOperator=0 Then 'AND' When LogicOperator=1 then 'OR' Else '' End as LogicOperator,SectionID From TbProjectSurveySectionLogic SL 
																Left Join TbProjectLogicConditions L On L.ID =SL.ConditionID Where SL.IsActive =1 And SurveyID = varTypeID) SL 
														Inner Join  TbProjectSurveyQuestion QQ  On QQ.QuestionID = SL.QuestionID  Where  QQ.SurveyID =  varTypeID)
                                            ))
								From TbProjectSurveyQuestion SQ  Inner Join TbQuestionType QT On QT.ID = SQ.QuestionTypeID
								Inner Join (Select SectionOrder, SectionID, SectionName  From  TbProjectSurveySection Where IsActive =1 And SurveyID =  varTypeID) SS On SS.SectionID =SQ.SectionID 									
								left join (Select QuestionID ,SurveyID,SectionID,Answer from TbProjectSurveyResponses Where SyncTaskID=varResponseSynID )SA  On SA.QuestionID=SQ.QuestionID And SA.SurveyId =SQ.SurveyId                                
                                Where SQ.IsActive =1 And SQ.SurveyID =  varTypeID Order By SectionOrder,QuestionOrder), 
				'CascadingList',Case When ((Select  EXISTS(Select QuestionID  from TbProjectSurveyQuestion Where QuestionTypeId= 8 and SurveyID=varTypeID) =0) And 
											(Select  EXISTS(Select QuestionID  from TbBeneficiaryTypeQuestion Where AnswerTypeID= 8 and BeneficiaryTypeID=BT.BeneficiaryTypeID) =0)) then null  Else 
								(Select JSON_ARRAYAGG(JSON_OBJECT ('Country' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',CountryID,'Name',CountryName,'ParentID',0 )) from TbCountry WHERE CountryID=1  Order By CountryName) ,
									'States' , (Select JSON_ARRAYAGG(JSON_OBJECT('ID',StateID,'Name',StateName,'ParentID',CountryID )) from TbStates  Order By StateName) 
							 		,'District',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',DistrictID,'Name',DistrictName,'ParentID',StateID )) from TbDistrict  Order By DistrictName)
							 		,'SubDistrict',(Select JSON_ARRAYAGG(JSON_OBJECT('ID',SubDistrictID,'Name',SubDistrictName,'ParentID',DistrictID )) from TbSubDistrict  Order By SubDistrictName)  
									 ))) End,
				'BeneficiaryList', ( SELECT JSON_ARRAYAGG(JSON_OBJECT( 	'BeneficiaryTypeID' ,BT.BeneficiaryTypeID ,'BeneficiaryID',BeneficiaryID, 'BeneficiaryName' ,BeneficiaryName, 'CreatedDate' ,CreatedDate,'TokenValue',TokenValue  ))
									From TbProjectBeneficiary  Where BeneficiaryTypeID = BT.BeneficiaryTypeID And IsActive =1 And IsDeleted =0),
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
									'SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'Answer' ,Answer, 'Answer1' ,Answer)) From TbProjectBeneficiary  B 
                                    Inner Join (Select R.*  from TbProjectSurveyResponses R Where R.IsActive =1 And SyncTaskID = varResponseSynID) R On R.ResponseID = B.BeneficiaryID
									Where B.BeneficiaryTypeID = BT.BeneficiaryTypeID And B.IsActive =1 And IsDeleted =0),
                'SurveyResponse',(SELECT JSON_ARRAYAGG(JSON_OBJECT('SurveyID',SurveyID, 'SectionID' ,SectionID, 'QuestionID', R.QuestionID, 'Answer' ,Answer, 'Answer1' ,Answer))
										From TbProjectSurveyResponses R Where R.IsActive =1 and R.SyncTaskID = varResponseSynID and R.ResponseID =0))	
			FROM TbProjectSurvey A Inner Join TbProject P On A.ProjectId =P.ProjectId      
            Left Join TbProjectBeneficiaryType BT On  BT.BeneficiaryTypeID=A.BeneficiaryTypeID Where SurveyID=varTypeID and P.ProjectStatus='Deploy');
			
                           
		End;
    End IF ;
   
    Update tempMyTask Set TaskData = @varTaskData   Where TypeID= varTypeID  And ResponseSyncID =varResponseSynID;   
   
  END LOOP; 
  CLOSE curTask;
  
Select A.*,varWorkspaceID WorkspaceID from tempMyTask A Order By   A.SyncDBID desc,A.TaskDate DESC;

END$$
DELIMITER ;
