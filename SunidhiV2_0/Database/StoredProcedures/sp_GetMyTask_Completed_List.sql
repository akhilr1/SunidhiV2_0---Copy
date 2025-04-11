USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_GetMyTask_Completed_List`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_GetMyTask_Completed_List`(IN loggedUserID int(10),IN varSyncDbID int(10),IN varLastSyncDatetime VARCHAR(500),IN varWorkspaceID int(10))
BEGIN

SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
	SELECT DISTINCT ASUSER.UserID FROM  TbOutcomeAssignUsers ASUSER JOIN 
	(
		SELECT DISTINCT OutcomeId  FROM TbOutcomeAssignUsers outuser WHERE outuser.UserID = loggedUserID
	) OUTCM ON OUTCM.OutcomeId = ASUSER.OutcomeId
) FFF ON FFF.UserID = MSK.UserID
WHERE MSK.Type = 'Outcome' and MSK.WFLevel = 0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse'  
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END


UNION

SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
SELECT DISTINCT WRKFLW.UserID FROM  TbProjectWorkflowDetails WRKFLW JOIN 
(
  SELECT  OUTC.WorkflowId WorkflowId FROM  TbOutcomes OUTC JOIN TbProjectWorkflowDetails WRKFLW1 ON  OUTC.WorkflowId =  WRKFLW1.WorkflowID
 WHERE WRKFLW1.UserID = loggedUserID
) FFF ON FFF.WorkflowId = WRKFLW.WorkflowID
) FFF1 ON FFF1.UserID = MSK.UserID
WHERE MSK.Type = 'Outcome' and MSK.WFLevel >0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse'  
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND  CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END

UNION


SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
	SELECT DISTINCT ASUSER.UserId UserID FROM  TbProjectActivityToUsers ASUSER JOIN 
	(
		SELECT DISTINCT ActivityId  FROM TbProjectActivityToUsers outuser WHERE outuser.UserId = loggedUserID
	) OUTCM ON OUTCM.ActivityId = ASUSER.ActivityId
) FFF ON FFF.UserID = MSK.UserID
WHERE MSK.Type = 'Activity' and MSK.WFLevel = 0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse'  and ifnull(MSK.TaskDate,0) !=0 
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END

UNION

SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
SELECT DISTINCT WRKFLW.UserID FROM  TbProjectWorkflowDetails WRKFLW JOIN 
(
  SELECT  OUTC.WorkflowId WorkflowId FROM  TbProjectActivity OUTC JOIN TbProjectWorkflowDetails WRKFLW1 ON  OUTC.WorkflowId =  WRKFLW1.WorkflowID
 WHERE WRKFLW1.UserID = loggedUserID
) FFF ON FFF.WorkflowId = WRKFLW.WorkflowID
) FFF1 ON FFF1.UserID = MSK.UserID
WHERE MSK.Type = 'Activity' and MSK.WFLevel >0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse' and ifnull(MSK.TaskDate,0) !=0   
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND  CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END

UNION

SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
	SELECT DISTINCT ASUSER.UserId UserID FROM  TbProjectSurveyUser ASUSER JOIN 
	(
		SELECT DISTINCT SurveyID FROM TbProjectSurveyUser srvyuser WHERE srvyuser.UserID = loggedUserID
	) SRVY ON SRVY.SurveyID = ASUSER.SurveyID
) FFF ON FFF.UserID = MSK.UserID
WHERE MSK.Type = 'Survey' and MSK.WFLevel = 0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse'  and ifnull(MSK.TaskDate,0) !=0 
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END

UNION

SELECT MSK.TypeID,MSK.Type,MSK.TaskDate,MSK.WFLevelStatus,MSK.WFLevel FROM TbMyTask MSK
JOIN TaskSyncDB SYNC ON  SYNC.ID = MSK.SyncTaskID
 LEFT JOIN 
(
SELECT DISTINCT WRKFLW.UserID FROM  TbProjectWorkflowDetails WRKFLW JOIN 
(
  SELECT  OUTC.WorkflowId WorkflowId FROM  TbProjectSurvey OUTC JOIN TbProjectWorkflowDetails WRKFLW1 ON  OUTC.WorkflowID =  WRKFLW1.WorkflowID
 WHERE WRKFLW1.UserID = loggedUserID
) FFF ON FFF.WorkflowId = WRKFLW.WorkflowID
) FFF1 ON FFF1.UserID = MSK.UserID
WHERE MSK.Type = 'Survey' and MSK.WFLevel >0 and  MSK.WFLevelStatus = 'Completed' AND SYNC.Command = 'TaskResponse' and ifnull(MSK.TaskDate,0) !=0   
AND SYNC.WorkspaceID = varWorkspaceID AND CASE varSyncDbID  WHEN  0 then  1=2   ELSE SYNC.ID  > varSyncDbID END
AND  CASE LENGTH(varLastSyncDatetime) WHEN 0 then 1=1 ELSE SYNC.CreatedDate > STR_TO_DATE(varLastSyncDatetime,'%Y-%m-%d %H:%i:%s') END;

END$$
DELIMITER ;
