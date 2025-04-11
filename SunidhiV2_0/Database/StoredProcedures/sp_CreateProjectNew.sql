USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateProjectNew`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateProjectNew`(IN INProjectName VARCHAR(500), IN INProjectDescription VARCHAR(2000),
	IN INStartDate datetime,IN INEndDate datetime, IN INProjectTags varchar(2000), 
    IN INInitiativeID INT(10), IN loggedUserId INT(10), IN WorkspaceId INT(10))
BEGIN
DECLARE flag INT ; -- VARCHAR(2000);
DECLARE ProjectID INT(11);
DECLARE WorkflowID INT(11);
DECLARE WorkflowLevel INT(11);
DECLARE WsID INT(11);
DECLARE ProjectCode varchar(6);
DECLARE PCode varchar(10);

DECLARE ifExists INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag="Failed";
       -- SELECT @flag ; 
        set @ProjectID =0;
        ROLLBACK ;     
    END;  
    
START TRANSACTION ; 
	set @ProjectID =0;
    set @ifExists  = 0;
	-- Set @ifExists = EXISTS(SELECT ProjectId from TbProject WHERE ProjectName =INProjectName  
							-- and InitiativeId = INInitiativeID );
      set @ifExists = EXISTS(SELECT ProjectId from TbProject Tb WHERE Tb.ProjectName =INProjectName AND Tb.WSID = WorkspaceId AND Tb.IsDeleted=0 AND Tb.IsCreated=1 );
	 IF IFNULL(@ifExists,0) = 0 THEN
	
		 INSERT INTO TbProject( ProjectName,Description, StartDate, EndDate,
							InitiativeId,ProjectStatus,IsActive,IsDeleted,CreatedDate,CreatedBy,WSID)
		VALUES(INProjectName,INProjectDescription,INStartDate,INEndDate,
					INInitiativeID,'Draft',1,0,now(),loggedUserId,WorkspaceId); 
                    
		Set @ProjectID=  LAST_INSERT_ID();     
		
         
        -- Created Project Code 
        -- string Code = n == 5 ? "P" : (n == 4 ? "P0" : (n == 3 ? "P00" : (n == 2 ? "P000" : (n == 1 ? "P0000" : "P00001"))));
        SET @PCode = LENGTH(@ProjectID);
        SET @ProjectCode = (Select (Case when @PCode = 5 then 'P' when  @PCode = 4 then 'PO' when @PCode = 3 then 'POO' when @PCode= 2 then 'POOO'
           when @PCode = 1 then 'POOOO' End));
		SET @ProjectCode  = concat(@ProjectCode ,@ProjectID);
        
        update TbProject Project SET Project.Code = @ProjectCode  where Project.ProjectId = @ProjectID;

        
        
        set @WorkflowID = (SELECT T.WorkflowID FROM TbInitiativeWorkflow T WHERE  T.InitiativeID = INInitiativeID LIMIT 1);
        set @WorkflowLevel = (SELECT T.WfNoOfLevels FROM TbInitiativeWorkflow T WHERE  T.InitiativeID = INInitiativeID LIMIT 1);
		SET @WorkflowID = ifnull(@WorkflowID,0);
        SET @WorkflowLevel = ifnull(@WorkflowLevel,0);
         -- Tag Details 
		INSERT INTO TbProjectToTagList(ProjectID,TagID,CreatedDate,CreatedBy) 	
		Select @ProjectID ProjectID, A.*,now() CreatedDate,loggedUserId CreatedBy from 
		(SELECT  * FROM  JSON_TABLE(INProjectTags, '$[*]' COLUMNS (TagID Int  PATH '$')) ProjectTagsList) A;
  
	Set @WsID = WorkspaceId ;-- (Select ifnull(WorkspaceID,0)   From TbInitiative Where InitiativeId = INInitiativeID);
      
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
select  @ProjectID ProjectID;
END$$
DELIMITER ;
