USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_AddProjectBudgetPlan`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_AddProjectBudgetPlan`(IN BenefName VARCHAR(500),IN BenefTID INT(11), 
				 IN BenefQstnsAns Json,IN loggedUserId INT(10))
BEGIN
DECLARE flag INT;
DECLARE lastBenfID INT(11);
DECLARE ifExists TINYINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        SELECT @flag ; 
        ROLLBACK ;       
    END;   
    
START TRANSACTION ;  
	
		INSERT INTO TbProjectBeneficiary( BeneficiaryName,BeneficiaryTypeID,IsActive,IsDeleted,CreatedDate,CreatedBy, UpdatedDate,UpdatedBy)
				VALUES(BenefName,BenefTID,1,0,now(),loggedUserId,now(),loggedUserId); 
		 Set @lastBenfID=  LAST_INSERT_ID();   
              
		-- QuestionAnswerList         
	
		Set @flag=@lastBenfID; 
	
		INSERT INTO TbBeneficiaryRegistrationDetails(BeneficiaryID,QuestionID,AnswerTypeID,Answer,CreatedDate,CreatedBy,IsDeleted) 
		Select @lastBenfID BeneficiaryID, A.*,now() CreatedDate,loggedUserId CreatedBy,0 IsDeleted from  
		(SELECT  * FROM  JSON_TABLE(BenefQstnsAns, '$[*]' COLUMNS (QuestionID int  PATH '$.QuestionID', AnswerTypeID Int  PATH '$.AnswerTypeID', 
			Answer varchar(2000)  PATH '$.Answer')) QstnList) A;		
    
COMMIT;
select  @flag TrnStatus;
END$$
DELIMITER ;
