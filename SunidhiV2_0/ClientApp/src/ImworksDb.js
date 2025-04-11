import Dexie from "dexie";

const db = new Dexie('ImWorksIndxdDb');

// Declare tables, IDs and indexes
db.version(1).stores({
    
    //AppSettings: "ServerPKID,LocalSyncDateTime,ClientPKID,UserId",
    User: "UserId,IsLoggedIn,TokenId,UserName,Password,WorkspaceID",
    TaskSettings: "++pkId,ServerPKID,TaskType,TaskTypeID, TaskStatus,LocalSyncDateTime,ClientPKID,UserId,LastSyncDateTime",

    MyTaskList: "++pkId,SyncDBID,Command, Type, TypeID, TypeIDColumn, Status,WorkflowID, TotalWFLevel, WFLevel, WFLevelStatus, ResponseSyncID, DataCollectedBy,TaskDate, TaskSubmittedDate,TaskData,WorkspaceID,TaskdataAddedBy,DataCollectorIds,ApproverIds,ToDelete,DataCollectionSyncDBID",
    TaskSyncDB: "++pkId, Type, Command, TypeID, TypeIDColumn, Status, WorkflowID,TotalWFLevel, WFLevel,ClientData, CreatedDate, CreatedBy,lastSyncDBID,IsBeneficiaryLinked,TaskDate,Periodicity,ReportingValue,WorkspaceID,TaskSubmittedDate,ClientGuid",
    TbMyTask: "++pkId,SyncTaskID,UserID, Type, TypeID, WFLevel, WFLevelStatus, TaskUpdateComment, AttachedDocumentName,ClientSyncTaskID,SyncDBID,ProjectName,TaskName,Description,CreatedDate,TaskDate, TaskSubmittedDate,Periodicity,PeriodicityId,PeriodicityDate,WorkspaceID,TaskSubmittedBy,BeneficiarySubmittedList,AnytimeSubmissionID,ToDelete",

    SavedTaskDB: "++pkId, SyncDBID,Type, Command, TypeID, TypeIDColumn, WorkflowID,TotalWFLevel, WFLevel,Status, SurveyQstns, SurveyAnswers,ResponseSyncID,DataCollectedBy,NewBeneficiaries,SelectedBeneficiaries,TaskDate, TaskSubmittedDate,WorkspaceID,RepeatOrSkipSection,CascadingAnsList,MultiChoiceAnsList,AnytimeSubmissionID",

    TbActivity: "ActivityId, ProjectId, ProjectName,ActivityName,Description,StartDate,EndDate,WeightagePercentage,ActivityType, SurveyId,SurveySectionId, SurveyQuestionId,  SurveyResponseId, SurveyLogic,SurveyTarget, IsPaymentLinked, PaymentLinkedBudget, WorkflowId, FinanceWorkflowId, Periodicity,CreatedDate, SurveyQstnList",
    TbKPI: "KPIId, ProjectId, ProjectName,KPIName,Description,StartDate,EndDate,CreatedDate",
    TbRisk: "RiskId, ProjectId, ProjectName,RiskName,Description,StartDate,EndDate,CreatedDate",
    TbSurvey: "SurveyID, ProjectId, ProjectName,SurveyName,Description,StartDate,EndDate,CreatedDate",
    TbTaskDocument: "++pkId,SyncTaskID, Type, TypeID, WFLevel, AttachedDocument, AttachedDocumentName,DocumentOriginalName,DocumentSize, DocumentType,DocumentTypeIcon,DocumentLastModifiedDate,ProjectID",
    TbTaskResponseDocument: "++pkId,SyncTaskID, Type, TypeID, WFLevel, AttachedDocument, FileName,ProjectID,DocLinkedTo,IsUploadedFile,BeneficiaryID, QuestionID,clientguid",
    TbTaskBenefRegDocument:"++pkId,SyncTaskID, Type, TypeID, WFLevel, AttachedDocument, FileName,ProjectID,DocLinkedTo,IsUploadedFile,QuestionID,clientguid",
   
    //TbSyncDB: "++SyncDBID,Type,Command,TypeIDColumn,TypeID,ClientData",
    TbSurveyQuestions: "++pkId,SurveyID,SectionID,QuestionID,QuestionName,QuestionTypeID,QuestionType",
    //TbSurvey: "++pkId,ClientPKID,SurveyID,SurveyName,CreatedDate,UserId,IsSubmitted,IsSaved,FormulaFunction",
    TbSurveySection: "++SectionID,SurveyID,SectionName,SectionDescription",
    TbSurveyAnswer: "++pkId,Answer,QuestionID,SurveyID,SectionID",
    TbTaskAuditLog: "++pkLogID, ProjectId, TaskType,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy,TaskUpdateComment,AttachedDocumentName,UniqueDocumentName,DocumentName,DocumentTypeIcon",

    TbUpdateTaskData: "++pkId,SyncDBID,Command, Type, TypeID, TypeIDColumn, Status,WorkflowID, TotalWFLevel, SubmittedWFLevel, WFLevel, WFLevelStatus, ResponseSyncID, DataCollectedBy,TaskDate,TaskSubmittedDate,TaskData",

    TbTempResposeData: "++pkId,SyncDBID, Type, TypeID,PeriodicityId,PeriodicityDate",
    TbLoggedUserDetails:"++pkId,ID,Name,FirstName,LastName,EmailId,Phone,Organization,ProfileImageName,Address",
    TbPageAccessList: "++pkId, UserID, PageName",

    TbDashboardProjectList: "++pkId, ProjectID, ProjectName",

    TbNewBeneficiary: "++pkId,BeneficiaryID,BeneficiaryName,BeneficiaryTypeID,CreatedDate,TokenValue,BeneficiaryQstnAnswers,WorkspaceID",
    TbAnyTimeData: "++pkId,TaskData",
    TbAnyTimeTempData: "++pkId,Type,TypeID,TaskData",

    MyTaskDM: "++pkId,SyncDBID,Command, SurveyID ,SurveyName,PeriodicityType,DataCollection,WorkflowID,TotalWorkflow,WorkFlowLevel, BeneficiaryTypeID",
    MyTaskSurveySection: "++pkId,SectionID,SectionName,SectionDescription,SectionOrder,SurveyID",
    MyTaskSurveyQuestion: "++pkId, QuestionID,QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,SurveyID,SectionID,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,IsConditionQuestion,IsRepeatSection,RepeatSectionId,IsUnique,IsSearchable,MinValue,MaxValue,IsActive,RegEx,IsMandatory,TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables,HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,IsOtherOptionSelected",
    MyTaskSurveySectionLogic: "++pkId,SectionID,SectionName,SectionDescription,SectionOrder,SurveyID",
    MyTaskSurveyTrackDate: "++pkId,SurveyID, TrackDate",
    TbMyTaskTable: "++pkId, SyncTaskID,UserID,Type,TypeID,WFLevelStatus,WFLevel,TaskDate ,TaskSubmittedDate,TaskUpdateComment,ParentSyncDBID, DataCollectionSyncDBID",
    MyTaskProjectSurveyQSTCondition: "++pkId, QuestionID, ValidationCondition,ConditionValue,MinValue,MaxValue,RegEx,ErrorText,LogicOperator,SurveyID",
    MyTaskProjectBenSurveyQSTCondition: "++pkId, QuestionID, ValidationCondition,ConditionValue,MinValue,MaxValue,RegEx,ErrorText,LogicOperator,BeneficiaryTypeID",
    MyTaskProjectSurveyQSTOption: "++pkId, OptionID,OptionValue,QuestionID,IsOptSelected,SurveyID",
    MyTaskSurveyCascadingQuestionOptions: "++pkId, OptionGroupID,OptionValue,HierarchyOrder,TableName,ColumnName,PrimaryColumn,ParentItemColumn",
    MyTaskSurveyBeneficiaryCascadingQuestionOptions: "++pkId, OptionGroupID,OptionValue,HierarchyOrder,TableName,ColumnName,PrimaryColumn,ParentItemColumn",
    TbMyTaskProjectLogicConditions: "++pkId, ID,Condition,ConditionValue",
    TbMyTaskProjectBeneficiaryType: "++pkId, BeneficiaryTypeID, Name,Description,ProjectID,SurveyID",
    TbMyTaskBeneficiaryAnswerType: "++pkId, AnswerTypeID,AnswerType",
    TbMyTaskBeneficiaryRegistrationDetails: "++pkId, BeneficiaryRegID,BeneficiaryID,QuestionID,AnswerTypeID, Answer,BeneficiaryTypeID",
    TbMyTaskProjectBeneficiary: "++pkId, BeneficiaryID,BeneficiaryTypeID,BeneficiaryName,CreatedDate,TokenValue",
    MyTaskBeneficiaryTypeQuestion: "++pkId, QuestionID,QuestionName,AnswerTypeID,BeneficiaryTypeID,IsUnique,IsSearchable,IsMandatoryResponse,QuestionHint,IsValidationQuestion,RegexExpression,QuestionOrder",
    MyTaskBeneficiaryTypeQuestionOption: "++pkId, QstnAnsOptionID,BeneficiaryTypeID,QuestionID,AnswerTypeID,OptionValue",
    MyTaskMasterList: "++pkId,ID,Name,ParentID,ItemTable,CensusCode",
    MyTaskQuestionType: "++pkId,ID,Type,Category",
    MyTaskProjectSurveyResponses: "++pkId,ID,ResponseID,Answer,QuestionID,SurveyID,SectionID,OptionID,RepeatOrder,SubRepeatOrder,SyncTaskID"   
});
export let dbImworks = db;

