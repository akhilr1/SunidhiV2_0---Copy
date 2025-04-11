USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateInitiative`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateInitiative`(IN InitName VARCHAR(500), IN vDescription VARCHAR(2000), 
	IN vCode varchar(8), IN vWorkspaceID INT(10),IN InitTags VARCHAR(500),IN UserList VARCHAR(500),IN GroupList VARCHAR(500),
	IN loggedUserId INT(10))
BEGIN
DECLARE flag INT;
DECLARE InitID INT(11);
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

        
	Set @ifExists = EXISTS(SELECT ifnull(InitiativeId,0)  InitiativeId from TbInitiative WHERE InitiativeName =InitName and 
							Code =vCode  and WorkspaceID = vWorkspaceID);		
	IF IFNULL(@ifExists,0) = 0 THEN
	
		INSERT INTO TbInitiative( InitiativeName,Description,Code,WorkspaceID,InitiativeStatus,IsActive,IsDeleted,CreatedDate,CreatedBy)
					VALUES(InitName,vDescription,vCode,vWorkspaceID,0,1,0,now(),loggedUserId); 
		Set @InitID=  LAST_INSERT_ID();                
		
	-- Tag Details 
iterator:
LOOP

	Set @n=LENGTH(TRIM(InitTags));
	IF LENGTH(TRIM(InitTags)) = 0 OR InitTags IS NULL THEN
	LEAVE iterator;
	END IF;

	SET @firstID = SUBSTRING_INDEX(InitTags,',',1);
	SET @firstln = LENGTH(@firstID);
	SET @TempValue = TRIM(@firstID);
	
	INSERT INTO TbInitiativeToTagList(InitiativeId,TagID,CreatedDate,CreatedBy)    
	VALUES(@InitID,@TempValue,now(),loggedUserId);
	Set InitTags=SUBSTRING(InitTags,@firstln + 2,@n);
	

END LOOP;
    	
		-- Userist  
    
    iterator:
    LOOP 
        	
		Set @n=LENGTH(TRIM(UserList));
		IF LENGTH(TRIM(UserList)) = 0 OR UserList IS NULL THEN
			LEAVE iterator;
		END IF;
                        
		SET @firstID = SUBSTRING_INDEX(UserList,',',1);
		SET @firstln = LENGTH(@firstID); 
        SET @TempValue = TRIM(@firstID);
        
        INSERT INTO TbInitiativeToUsers(InitiativeId,UserID,CreatedDate,CreatedBy) 
				VALUES(@InitID,@TempValue,now(),loggedUserId);
        Set UserList=SUBSTRING(UserList,@firstln + 2,@n);
       
        
	END LOOP;
    
	-- GroupList  
    
    iterator:
    LOOP 
        	
		Set @n=LENGTH(TRIM(GroupList));
		IF LENGTH(TRIM(GroupList)) = 0 OR GroupList IS NULL THEN
			LEAVE iterator;
		END IF;
                        
		SET @firstID = SUBSTRING_INDEX(GroupList,',',1);
		SET @firstln = LENGTH(@firstID); 
        SET @TempValue = TRIM(@firstID);      
        
        INSERT INTO TbInitiativeToGroups(InitiativeId,GroupID,CreatedDate,CreatedBy) 
				VALUES(@InitID,@TempValue,now(),loggedUserId);
        Set GroupList=SUBSTRING(GroupList,@firstln + 2,@n);
       
        
	END LOOP;
    Set @flag=@InitID;
   End IF;
   
COMMIT;
select  @flag TrnStatus;
END$$
DELIMITER ;
