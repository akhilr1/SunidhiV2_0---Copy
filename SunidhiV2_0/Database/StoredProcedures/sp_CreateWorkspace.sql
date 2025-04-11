USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateWorkspace`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateWorkspace`(IN vWSName VARCHAR(500), IN vWSDescription VARCHAR(2000), 
	IN vWSIsPrivate TINYINT, IN WSTags VARCHAR(500),	IN loggedUserId INT(10),IN WorkSpaceImage VARCHAR(500))
BEGIN
DECLARE flag TINYINT;
DECLARE wsID INT(11);
DECLARE ifExists TINYINT;

DECLARE n INT DEFAULT NULL;
DECLARE firstID TEXT DEFAULT NULL;
DECLARE firstln INT DEFAULT NULL;
DECLARE TempValue TEXT DEFAULT NULL;

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        SELECT @flag ; 
        ROLLBACK ;
       
    END;
   
    
START TRANSACTION ;  

	 INSERT INTO TbWorkspace( WSName,WSDescription,WSIsPrivate,CreatedDate,CreatedBy)
	 	VALUES(vWSName,vWSDescription,vWSIsPrivate,now(),loggedUserId); 
     Set @wsID=  LAST_INSERT_ID();  
   
     update TbWorkspace set WorkSpaceImage = REPLACE(WorkSpaceImage, 'workspaceid', @wsID) where WorkspaceID = @wsID;

	 INSERT INTO TbWorkspaceTeam( WorkspaceID,UserID,RoleID,CreatedDate,CreatedBy) 
         VALUES(@wsID,loggedUserId,1,now(),loggedUserId);	
        
	
	-- Tag Details 
    
    iterator:
    LOOP 
        	
		Set @n=LENGTH(TRIM(WSTags));
		IF LENGTH(TRIM(WSTags)) = 0 OR WSTags IS NULL THEN
			LEAVE iterator;
		END IF;
                        
		SET @firstID = SUBSTRING_INDEX(WSTags,',',1);
		SET @firstln = LENGTH(@firstID); 
        SET @TempValue = TRIM(@firstID);
        
        
        
        Set @ifExists = (SELECT ifnull(ID,0) ID from TbWorkspaceTagDetails WHERE WorkspaceID= @wsID and WSTagID =@TempValue);
		
		IF ifnull(@ifExists,0) = 0 THEN
			INSERT INTO TbWorkspaceTagDetails(WorkspaceID,WSTagID,CreatedDate,CreatedBy) 
				VALUES(@wsID,@TempValue,now(),loggedUserId);
                
		End IF;
        
        Set WSTags=SUBSTRING(WSTags,@firstln + 2,@n);
       
        
	END LOOP;
    
     
COMMIT;
Set @flag=1;
select  @wsID TrnStatus;
END$$
DELIMITER ;
