USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateProject`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateProject`(IN INProjectName VARCHAR(500), IN INProjectDescription VARCHAR(2000), IN INProjectCode VARCHAR(6), 
	IN INProjectBudget INT,IN INStartDate datetime,IN INEndDate datetime, IN INCountryID INT,IN INStateID INT,IN INDistrictID INT, IN INProjectTags varchar(2000), 
    IN INProjectTemplateID INT ,IN INInitiativeID INT(10), IN loggedUserId INT(10))
BEGIN
DECLARE flag INT ; -- VARCHAR(2000);
DECLARE ProjectID INT(11);
DECLARE WorkflowID INT(11);
DECLARE WorkflowLevel INT(11);
DECLARE WsID INT(11);

DECLARE ifExists TINYINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag="Failed";
        SELECT @flag ; 
        ROLLBACK ;     
    END;  
    
START TRANSACTION ; 
	
	Set @ifExists = EXISTS(SELECT ProjectId from TbProject WHERE ProjectName =INProjectName and Code =INProjectCode  
							and InitiativeId = INInitiativeID and ProjectTemplateID=INProjectTemplateID);
	 IF IFNULL(@ifExists,0) = 0 THEN
	
		 INSERT INTO TbProject( ProjectName,Description,Code,Budget, StartDate, EndDate, CountryID, StateID, DistrictID,ProjectTemplateID,
							InitiativeId,ProjectStatus,IsActive,IsDeleted,CreatedDate,CreatedBy)
		VALUES(INProjectName,INProjectDescription,INProjectCode,INProjectBudget,INStartDate,INEndDate,INCountryID,INStateID,INDistrictID,INProjectTemplateID,
					INInitiativeID,'Draft',1,0,now(),loggedUserId); 
                    
		Set @ProjectID=  LAST_INSERT_ID();     
		
        set @WorkflowID = (SELECT T.WorkflowID FROM TbInitiativeWorkflow T WHERE  T.InitiativeID = INInitiativeID LIMIT 1);
        set @WorkflowLevel = (SELECT T.WfNoOfLevels FROM TbInitiativeWorkflow T WHERE  T.InitiativeID = INInitiativeID LIMIT 1);
		SET @WorkflowID = ifnull(@WorkflowID,0);
        SET @WorkflowLevel = ifnull(@WorkflowLevel,0);
         -- Tag Details 
		INSERT INTO TbProjectToTagList(ProjectID,TagID,CreatedDate,CreatedBy) 	
		Select @ProjectID ProjectID, A.*,now() CreatedDate,loggedUserId CreatedBy from 
		(SELECT  * FROM  JSON_TABLE(INProjectTags, '$[*]' COLUMNS (TagID Int  PATH '$')) ProjectTagsList) A;
  
	Set @WsID = (Select ifnull(WorkspaceID,0)   From TbInitiative Where InitiativeId = INInitiativeID);
      
    -- insert into TaskSyncDB - created on 29-09-2020
		INSERT INTO TaskSyncDB(`Type`, Command, TypeIDColumn, TypeID,WorkflowID,TotalWFLevel,`Status`,WorkspaceID,CreatedDate,CreatedBy)
		VALUES('Project', 'ADD', 'ProjectId', @ProjectID,@WorkflowID,@WorkflowLevel,'Pending', @WsID,now(), loggedUserId);
 
     -- insert into task Audit log
        INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,Message,CreatedDate,CreatedBy)
        VALUES(@ProjectID,'Project',@ProjectID,'ProjectId',CONCAT ('Created ',INProjectName),NOW(),loggedUserId);
     
    Set @flag="success";
    Else
    Set @flag="Duplicate Entry ... Same project Exists";
   End IF;   
COMMIT;
select  @flag TrnStatus;
END$$
DELIMITER ;
