USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_CreateBeneficiaryType`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CreateBeneficiaryType`(IN pProjectId INT, IN BenefName VARCHAR(500), IN BenefDesc VARCHAR(5000), 
						IN BenefTagsList Json,IN BenefQstns Json,IN loggedUserId INT(10))
BEGIN
DECLARE flag INT;
DECLARE lastBTypeID INT(11);
DECLARE ifExists TINYINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        Set @ifExists =0;
        SELECT @flag ; 
        ROLLBACK ;       
    END;   
    
START TRANSACTION ;  
	
	Set @ifExists = (Select EXISTS(SELECT ifnull(BeneficiaryTypeID,0) from TbProjectBeneficiaryType WHERE Name =BenefName and ProjectID = pProjectId));	                         
	
     IF @ifExists = 0 THEN
	
		 INSERT INTO TbProjectBeneficiaryType( Name,Description,ProjectID,IsActive,IsDeleted,CreatedDate,CreatedBy, UpdatedDate,UpdatedBy)
				VALUES(BenefName,BenefDesc,pProjectId,1,0,now(),loggedUserId,now(),loggedUserId); 
		 Set @lastBTypeID=  LAST_INSERT_ID();           
		
		-- Tag Details 
        IF BenefTagsList is not null Then
           
		INSERT INTO TbBeneficiaryTypeTagDetails(BeneficiaryTypeID,TagID,CreatedDate,CreatedBy,IsDeleted) 	
		Select @lastBTypeID BeneficiaryTypeID, A.TagID,now() CreatedDate,loggedUserId CreatedBy,0  from 
		(SELECT  * FROM  JSON_TABLE(BenefTagsList, '$[*]' COLUMNS (TagID Int  PATH '$.TagID')) TagList) A;
        
		End IF;
        
		-- QuestionList         
		INSERT INTO TbBeneficiaryTypeQuestion(BeneficiaryTypeID,QuestionName,AnswerTypeID,IsUnique,IsSearchable,IsMandatoryResponse,QuestionHint,IsValidationQuestion,RegexExpression,CreatedDate,CreatedBy,IsDeleted,IsActive) 
		Select @lastBTypeID BeneficiaryTypeID, A.*,now() CreatedDate,loggedUserId CreatedBy,0 IsDeleted,1 IsActive from  
		(SELECT  * FROM  JSON_TABLE(BenefQstns, '$[*]' COLUMNS (QuestionName varchar(500)  PATH '$.Question', AnswerTypeID Int  PATH '$.AnswerTypeID', 
			IsUnique TinyInt PATH '$.IsUnique', IsSearchable TinyInt PATH '$.IsSearchable',  IsMandatoryResponse TinyInt PATH '$.IsMandatoryResponse',
            QuestionHint varchar(1000)  PATH '$.QuestionHint' , IsValidationQuestion TinyInt PATH '$.IsValidationQuestion',RegexExpression varchar(1000)  PATH '$.RegexExpression' )) QstnList) A;		
    
		-- Question Option List    
        
		INSERT INTO TbBeneficiaryTypeQstnAnswerOptions(BeneficiaryTypeID,QuestionID,AnswerTypeID,OptionValue,CreatedDate,CreatedBy,IsDeleted,IsActive) 		
		Select @lastBTypeID BeneficiaryTypeID ,Max(QuestionID) QuestionID,AA.AnswerTypeID,AA.OptionValue,now(), loggedUserId ,0 ,1    
        From  TbBeneficiaryTypeQuestion B Inner join 
			(Select A.* From (SELECT  * FROM  JSON_TABLE(BenefQstns, '$[*]' COLUMNS (QuestionName varchar(500)  PATH '$.Question', AnswerTypeID Int  PATH '$.AnswerTypeID',
				NESTED PATH '$.BTQstnAnsOptionList[*]' COLUMNS ( OptionValue  varchar(1000)  PATH '$.OptionValue') ) )OptList) A) AA 
				On  B.QuestionName =AA.QuestionName And B.AnswerTypeID= AA.AnswerTypeID 
                Where AA.OptionValue Is Not null  Group by AA.QuestionName,AA.AnswerTypeID,AA.OptionValue;
             
        -- insert into TbBeneficiaryQSTCondition
		INSERT INTO TbBeneficiaryQSTCondition(QuestionID,ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator,CreatedDate,CreatedBy) 
        Select Max(BTQ.QuestionID) QuestionID, VV.ValidationCondition,VV.MinValue,VV.`MaxValue`, VV.ConditionValue, VV.LogicOperator,now(), loggedUserId
        From TbBeneficiaryTypeQuestion BTQ inner join 
        (Select V.* FROM (SELECT  * FROM  JSON_TABLE(BenefQstns, '$[*]' COLUMNS (QuestionName varchar(500)  PATH '$.Question', AnswerTypeID Int  PATH '$.AnswerTypeID', 
         NESTED PATH '$.lstQuestionValidations[*]' COLUMNS ( ValidationCondition  varchar(1000)  PATH '$.ValidationCondition',
         MinValue Int  PATH '$.MinValue',`MaxValue` Int  PATH '$.MaxValue',ConditionValue varchar(500)  PATH '$.ConditionValue',
         LogicOperator Int  PATH '$.LogicOperator') ) )ValList) V) VV 
         On  BTQ.QuestionName =VV.QuestionName And BTQ.AnswerTypeID= VV.AnswerTypeID 
         Where  VV.ValidationCondition is not null and VV.ValidationCondition != '' Group by VV.QuestionName,VV.AnswerTypeID,VV.ValidationCondition,VV.MinValue,VV.`MaxValue`, VV.ConditionValue, VV.LogicOperator;
             
             
    Set @flag=@lastBTypeID;
   End IF;
   
COMMIT;
 select  @flag TrnStatus;
END$$
DELIMITER ;
