USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateImportBeneficiary`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateImportBeneficiary`(IN BenefImportList Json,IN loggedUserId INT(10))
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
START TRANSACTION; 
        
		INSERT INTO TbProjectBeneficiary( BeneficiaryTypeID,BeneficiaryName,IsActive,IsDeleted,CreatedDate,CreatedBy, UpdatedDate,UpdatedBy)
		Select A.*,1,0,now() CreatedDate,loggedUserId CreatedBy,now() UpdatedDate,loggedUserId UpdatedBy from  
		(SELECT  * FROM  JSON_TABLE(BenefImportList, '$[*]' COLUMNS (BeneficiaryTypeID Int  PATH '$.BeneficiaryTypeID', BeneficiaryName varchar(500) PATH '$.BeneficiaryName')) ImportList) A;
		
        -- QuestionAnswerList      
      
        INSERT INTO TbBeneficiaryRegistrationDetails(BeneficiaryID,QuestionID,AnswerTypeID,Answer,CreatedDate,CreatedBy,IsDeleted) 
        Select Max(BeneficiaryID) BeneficiaryID,AA.QuestionID,AA.AnswerTypeID,AA.Answer,now(), loggedUserId, 0 
        From  TbProjectBeneficiary B Inner join 
			(Select A.* From (SELECT  * FROM  JSON_TABLE(BenefImportList, '$[*]' COLUMNS (BeneficiaryTypeID Int  PATH '$.BeneficiaryTypeID', BeneficiaryName varchar(500) PATH '$.BeneficiaryName',
				NESTED PATH '$.BenefQuestions[*]' COLUMNS ( QuestionID int  PATH '$.QuestionID', AnswerTypeID Int  PATH '$.AnswerTypeID',Answer varchar(2000)  PATH '$.Answer') ) )ImportList) A) AA 
				On B.BeneficiaryTypeID=AA.BeneficiaryTypeID And B.BeneficiaryName =AA.BeneficiaryName Group by AA.BeneficiaryTypeID,AA.BeneficiaryName,AA.QuestionID,AA.AnswerTypeID,AA.Answer;
        
        Set @flag =1 ;    
    COMMIT;
select  @flag TrnStatus;
END$$
DELIMITER ;
