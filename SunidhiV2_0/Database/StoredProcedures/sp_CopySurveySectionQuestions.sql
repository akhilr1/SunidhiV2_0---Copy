USE `SUNIDHI_CORE_V2_0`;
DROP procedure IF EXISTS `sp_CopySurveySectionQuestions`;

DELIMITER $$
USE `SUNIDHI_CORE_V2_0`$$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_CopySurveySectionQuestions`(IN NewSurveyID INT(10), IN OldSurveyID INT(10),IN loggedUserId INT(10))
BEGIN
	DECLARE flag TINYINT;
	DECLARE SID INT(11);
	DECLARE SecID INT(11);
    
	DECLARE n INT DEFAULT NULL;
	DECLARE firstID TEXT DEFAULT NULL;
	DECLARE firstln INT DEFAULT NULL;
	DECLARE TempValue TEXT DEFAULT NULL;
	DECLARE NewValue TEXT DEFAULT NULL;    
	DECLARE done INT DEFAULT FALSE;
	DECLARE QID Int ;
	DECLARE FFVar varchar(200);
 
	DECLARE cur1 CURSOR FOR SELECT QuestionID ,FormulaFieldVariables FROM TbProjectSurveyQuestion Where SurveyID=NewSurveyID and  Ifnull(FormulaFieldVariables,'') != '';
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;  
	DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        SELECT @flag ; 
        ROLLBACK ;       
    END;       
 START TRANSACTION; 		
		Insert Into TbProjectSurveyCloneHistory(OldSurveyID,NewSurveyID,CreatedBy,CreatedDate)
        Select OldSurveyID,NewSurveyID,loggedUserId,now();
        
		Insert Into TbProjectSurveySection(SectionName,SectionDescription ,SectionOrder ,SurveyID ,IsActive ,CreatedDate ,UpdatedDate ,CreatedBy ,UpdatedBy)
		Select SectionName,SectionDescription ,SectionOrder ,NewSurveyID ,IsActive ,now() ,now() ,loggedUserId ,loggedUserId from TbProjectSurveySection Where SurveyID=OldSurveyID And IsActive =1;

		Insert Into TbProjectSurveyQuestion (SurveyID, SectionID,QuestionName, QuestionTypeID, QuestionOrder, DefaultValue, CreatedBy, UpdatedBy, CreatedDate,UpdatedDate, 
			IsConditionQuestion, IsRepeatSection, RepeatSectionId, IsUnique, IsSearchable, IsActive, RegEx, IsMandatory, TextLength, XMLTag, ErrorText,  IsFormulaQuestion,  
            FormulaOperation,FormulaExpression, FormulaFieldVariables,HierarchyQuestionOrder, RepeatCondition, QuestionHint,IsValidationQuestion,UOM)
		Select NewSurveyID, S.NSectionID SectionID, QuestionName, QuestionTypeID, QuestionOrder, DefaultValue, CreatedBy, UpdatedBy, now(), now(), 
			IsConditionQuestion, IsRepeatSection, RepeatSectionId, IsUnique, IsSearchable, IsActive, RegEx, IsMandatory, TextLength, XMLTag, ErrorText, 
            IsFormulaQuestion,  FormulaOperation, FormulaExpression, FormulaFieldVariables,HierarchyQuestionOrder, 	RepeatCondition, QuestionHint ,IsValidationQuestion,UOM
		FRom TbProjectSurveyQuestion Q
		Inner Join (Select A.SectionID OldSectionID ,B.SectionID NSectionID   from TbProjectSurveySection A 
		left Join TbProjectSurveySection B On A.SectionName =B.SectionName where A.SurveyID =OldSurveyID And A.IsActive=1 And B.SurveyID=NewSurveyID) S On S.OldSectionID =Q.SectionID
		Where SurveyID =OldSurveyID And IsActive =1;

		-- Update Field variables
        Update TbProjectSurveyQuestion Set FieldVar = Concat('var_',QuestionID) Where SurveyID = NewSurveyID and FieldVar is null ;
	
		-- Option Qstns values
        
        Insert Into TbProjectSurveyQSTOption (QuestionID,OptionValue,CreatedBy,CreatedDate)
		Select  B.QuestionID , OptionValue ,loggedUserId ,now() From 
		(Select * FRom TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
		Left Join 
		(Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
					A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And
					A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
					Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And
					Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'')
		Left Join TbProjectSurveyQSTOption C On C.QuestionID=A.QuestionID
		Where C.QuestionID Is not null;
        
        -- Question Validations
		Insert Into TbProjectSurveyQSTCondition (QuestionID,ValidationCondition, ConditionValue, MinValue, `MaxValue`,CreatedBy,CreatedDate)
		Select  B.QuestionID , C.ValidationCondition, ConditionValue, C.MinValue, C.`MaxValue` ,loggedUserId ,now() From 
			(Select * FRom TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
			Left Join 
			(Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
						A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
						Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And
						Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'')
			Left Join TbProjectSurveyQSTCondition C On C.QuestionID=A.QuestionID
			Where C.QuestionID Is not null;
			
       
        -- Section Logic
         -- Old Query 
			/*Insert  Into TbProjectSurveySectionLogic(SurveyID, SectionID, LogicType, QuestionID, ConditionID, Answer, LogicOperator, CreatedDate,UpdatedDate, CreatedBy, UpdatedBy, IsActive)
			Select  NewSurveyID, NSectionID, LogicType, (select t.QuestionID from TbProjectSurveyQuestion t where t.SurveyID in (NewSurveyID) and t.QuestionName in (select p.QuestionName from TbProjectSurveySectionLogic t inner join TbProjectSurveyQuestion p on p.QuestionID = t.QuestionID where t.SurveyID in (OldSurveyID) And t.IsActive =1)) NQstnID, ConditionID, Answer, LogicOperator, now(), now(), loggedUserId, loggedUserId, IsActive from TbProjectSurveySectionLogic A 
			Inner Join 
			( Select  A.sectionID OldSectionID,A.QuestionID OldQstnID,B.SectionID NSectionID,B.QuestionID NQstnID From 
					(Select * FRom TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
					Left Join 
					(Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
								A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And
								A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
								Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And
								Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'')) B On B.OldSectionID=A.SectionID
			 where SurveyID =OldSurveyID And IsActive =1 ; */
             
             Insert  Into TbProjectSurveySectionLogic(SurveyID, SectionID, LogicType, QuestionID, ConditionID, Answer, LogicOperator, CreatedDate,UpdatedDate, CreatedBy, UpdatedBy, IsActive)
             Select  NewSurveyID, B.NSectionID, LogicType, 
					PQ.NQstnID as QuestionID, ConditionID, Answer, LogicOperator, now(), now(), loggedUserId,loggedUserId, IsActive from TbProjectSurveySectionLogic A 
			Inner Join ( Select  A.sectionID OldSectionID,A.QuestionID OldQstnID,B.SectionID NSectionID,B.QuestionID NQstnID From 
				(Select * From TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
					Left Join 
					(Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
				A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And
				A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
				Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'')) B On B.OldSectionID=A.SectionID
				inner join 
					( Select  A.sectionID OldSectionID,A.QuestionID OldQstnID,B.SectionID NSectionID,B.QuestionID NQstnID From 
					(Select * FRom TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
					Left Join 
				(Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
				A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And
				A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
				Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'')) PQ On PQ.OldQstnID=A.QuestionID
				where SurveyID =OldSurveyID And IsActive =1 ; 
             
             
             
            
		/*Update FormulaField Variables*/			
		OPEN cur1; 
		Read_Loop: LOOP 
		FETCH cur1 INTO QID,FFVar;

		IF done THEN
		  LEAVE Read_Loop;
		END IF;    
		iterator:
			LOOP
				Set @n=LENGTH(TRIM(FFVar));
				IF LENGTH(TRIM(FFVar)) = 0 OR FFVar IS NULL THEN
					LEAVE iterator;
				END IF;

				SET @firstID = SUBSTRING_INDEX(FFVar,',',1);
				SET @firstln = LENGTH(@firstID);
				SET @TempValue = TRIM(@firstID);
			
				Set @NewValue=	(Select   B.FieldVar  From 	(Select * FRom TbProjectSurveyQuestion  Where SurveyID =OldSurveyID And IsActive =1) A
								Left Join (Select * FRom TbProjectSurveyQuestion  Where SurveyID =NewSurveyID) B On  A.QuestionName=B.QuestionName And   A.QuestionTypeID=B.QuestionTypeID And A.QuestionOrder=B.QuestionOrder And
									A.DefaultValue=B.DefaultValue And   A.IsUnique=B.IsUnique And A.IsSearchable=B.IsSearchable And  A.IsMandatory=B.IsMandatory And Ifnull(A.RegEx,'')= Ifnull(B.RegEx,'') And
									Ifnull(A.ErrorText,'')=Ifnull(B.ErrorText,'') And   Ifnull(A.IsFormulaQuestion,0)=Ifnull(B.IsFormulaQuestion,0) And Ifnull(A.FormulaOperation,'')=Ifnull(B.FormulaOperation,'') And Ifnull(A.QuestionHint,'')=Ifnull(B.QuestionHint ,'') 
								Where A.FieldVar =@TempValue);			
				
				Set FFVar=SUBSTRING(FFVar,@firstln + 2,@n);
				
				Update TbProjectSurveyQuestion Set FormulaFieldVariables =  Replace( FormulaFieldVariables,@TempValue,@NewValue) , FormulaExpression= Replace(FormulaExpression,@TempValue,@NewValue)
				Where SurveyID=NewSurveyID and QuestionID = QID; 
					
			END LOOP;			
		END LOOP Read_Loop;
		CLOSE cur1;
        Set @flag =1 ;    
    COMMIT;
select  @flag TrnStatus;
END$$

DELIMITER ;