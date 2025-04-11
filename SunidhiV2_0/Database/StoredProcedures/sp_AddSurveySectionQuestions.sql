USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_AddSurveySectionQuestions`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_AddSurveySectionQuestions`(IN SecQuestionList Json,IN loggedUserId INT(10))
BEGIN
DECLARE flag TINYINT;
DECLARE lastID INT(11);
DECLARE lastQstnOrder INT(11);
DECLARE ifExists TINYINT;

DECLARE SID INT(11);
DECLARE SecID INT(11);
DECLARE QuesName VARCHAR(2000);
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        -- SELECT @flag ; 
        ROLLBACK ;       
    END;       
 START TRANSACTION; 		
				
        Set @SID =( Select SurveyID  FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (SurveyID Int  PATH '$.SurveyID', SectionID Int  PATH '$.SectionID')) QList  );        
		Set @SecID=( Select SectionID  FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (SurveyID Int  PATH '$.SurveyID', SectionID Int  PATH '$.SectionID')) QList  ); 
        Set @QuesName=( Select QuestionName  FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (SurveyID Int  PATH '$.SurveyID', SectionID Int  PATH '$.SectionID',QuestionName varchar(2000)  PATH '$.QuestionName')) QList  );
		Set @lastQstnOrder = (Select max(ifnull(QuestionOrder,0))   From TbProjectSurveyQuestion Where SurveyID =  @SID And SectionID = @SecID);
        
       Set @ifExists = EXISTS(SELECT QuestionID from TbProjectSurveyQuestion WHERE SurveyID =  @SID And SectionID = @SecID and QuestionName=@QuesName);
	 IF IFNULL(@ifExists,0) = 0 THEN
      
		INSERT INTO TbProjectSurveyQuestion(SurveyID, SectionID, QuestionName, QuestionTypeID, QuestionHint,DefaultValue,IsUnique,
				IsSearchable,IsMandatory,IsFormulaQuestion,FormulaOperation,FormulaExpression,FormulaFieldVariables,RegEx,ErrorText,
                IsValidationQuestion,UOM,
				-- ValidationCondition,MinValue,`MaxValue`,TextLength,
                 CreatedDate, CreatedBy, IsActive)
		Select A.* ,now() CreatedDate, loggedUserId CreatedBy, 1 IsActive
		From  (SELECT  * FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (SurveyID Int  PATH '$.SurveyID', SectionID Int  PATH '$.SectionID',
					QuestionName varchar(5000) PATH '$.QuestionName',QuestionTypeID Int  PATH '$.QuestionTypeID', QuestionHint varchar(2000) PATH '$.QuestionHint',
                    DefaultValue varchar(500) PATH '$.DefaultValue', IsUnique TinyInt  PATH '$.IsUnique',IsSearchable TinyInt  PATH '$.IsSearchable',
                    IsMandatory TinyInt  PATH '$.IsMandatory' , IsFormulaQuestion TinyInt  PATH '$.IsFormulaQuestion',FormulaOperation varchar(1000) PATH '$.FormulaOperation',
                    FormulaExpression varchar(2000) PATH '$.FormulaExpression',FormulaFieldVariables varchar(5000) PATH '$.FormulaFieldVariables',
					RegEx varchar(200)  PATH '$.RegEx', ErrorText varchar(500)  PATH '$.ErrorText', IsValidationQuestion TinyInt  PATH '$.IsValidationQuestion'
                    , UOM varchar(10)  PATH '$.UOM'
                     -- ,ValidationCondition varchar(4000) PATH '$.ValidationCondition',  MinValue Int  PATH '$.MinValue',`MaxValue` Int  PATH '$.MaxValue',TextLength Int  PATH '$.TextLength'
                     )) QstnList) A;
        		              
		Set @lastID=  LAST_INSERT_ID();  
          
        -- Update Field Variable &  QuestionOrder        
             
        Update TbProjectSurveyQuestion Set FieldVar = concat('var_',@lastID), QuestionOrder =ifnull(@lastQstnOrder,0) +1   Where QuestionID = @lastID;
        
		-- Question Option List  
          
		INSERT INTO TbProjectSurveyQSTOption(QuestionID,OptionValue,CreatedDate,CreatedBy) 
        Select @lastID QuestionID, A.OptionValue,now(), loggedUserId
        From (SELECT  * FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (NESTED PATH '$.lstQuestionOptions[*]' COLUMNS ( OptionValue  varchar(1000)  PATH '$.OptionValue') ) )OptList) A Where  A.OptionValue is not null;
       
        -- Question Condition List  
       
		INSERT INTO TbProjectSurveyQSTCondition(QuestionID,ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator,CreatedDate,CreatedBy) 
        Select @lastID QuestionID, A.*,now(), loggedUserId
        From (SELECT  * FROM  JSON_TABLE(SecQuestionList, '$[*]' COLUMNS (NESTED PATH '$.lstQuestionValidations[*]' COLUMNS ( ValidationCondition  varchar(1000)  PATH '$.ValidationCondition',
         MinValue Int  PATH '$.MinValue',`MaxValue` Int  PATH '$.MaxValue',ConditionValue varchar(500)  PATH '$.ConditionValue',LogicOperator Int  PATH '$.LogicOperator') ) )ValList) A Where  A.ValidationCondition is not null;
        
       
        Set @flag =1 ; 
          Else
    Set @flag=0;
   End IF;
     COMMIT;
select  @flag TrnStatus;
END$$
DELIMITER ;
