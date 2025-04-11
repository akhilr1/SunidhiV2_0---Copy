USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_AddProjectBudgetPlan`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_AddProjectBudgetPlan`(IN PrjID int(10),IN NoOfBudgetPlan int(10),
IN BudgetPlanList Json,IN loggedUserId INT(10))
BEGIN
DECLARE flag TINYINT;
DECLARE TotalHeadAmt float;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag=0;
        SELECT @flag ; 
        ROLLBACK ;       
    END;      
   
START TRANSACTION; 		
       
		INSERT INTO TbBudgetPlan(ProjectId,BudgetHead,HeadAmount,CreatedDate, CreatedBy, IsDeleted)
		Select PrjID,A.BudgetHeadName  BudgetHead,HeadAmount,now() CreatedDate, loggedUserId CreatedBy, 0 IsDeleted
		From  (SELECT  * FROM  JSON_TABLE(BudgetPlanList, '$[*]' COLUMNS (BudgetHeadName varchar(500) PATH '$.BudgetHeadName',HeadAmount double PATH '$.HeadAmount' )) BudList) A;
        
		Set @TotalHeadAmt = (Select ifnull(sum(ifnull(HeadAmount,0)),0)   From TbBudgetPlan Where IsDeleted =0 And ProjectId =  PrjID); 
        		        
		INSERT INTO TbBudgetPlanItemDetails(ProjectId,BudgetHeadID,ItemName,Amount,CreatedDate,CreatedBy,IsDeleted) 		
		Select PrjID ,Max(ID) BudgetHeadID,AA.ItemName,AA.Amount,now(), loggedUserId ,0   IsDeleted
        From  TbBudgetPlan B Inner join 
			(Select A.* From (SELECT  * FROM  JSON_TABLE(BudgetPlanList, '$[*]' COLUMNS (BudgetHeadName varchar(200)  PATH '$.BudgetHeadName',
				NESTED PATH '$.BudgetPlanSubList[*]' COLUMNS ( ItemName  varchar(200)  PATH '$.SubHeaderName',Amount  double  PATH '$.Amount') ) )SubList) A) AA 
				On  B.BudgetHead =AA.BudgetHeadName  Where B.ProjectId =PrjID  Group by B.ProjectId,AA.ItemName,AA.Amount,AA.BudgetHeadName;
                
         Update TbProject set NoOfBudgetHead = NoOfBudgetPlan ,BudgetHeadAmount =@TotalHeadAmt, UpdatedDate = now(),UpdatedBy=loggedUserId where ProjectId =PrjID; 
		
        -- INSERT INTO TbFinanceAuditLog(ProjectId,Message,CreatedDate,CreatedBy)
        
        INSERT INTO TbProjectAuditLog(ProjectID,Message,AuditFrom,AuditEvent,CreatedDate,CreatedBy)
        Select PrjID, CONCAT('Added a new Budget Plan ''',   A.BudgetHeadName ,'''' )  Message, 'Finance', 'ADD',now() CreatedDate, loggedUserId CreatedBy
		From  (SELECT  * FROM  JSON_TABLE(BudgetPlanList, '$[*]' COLUMNS (BudgetHeadName varchar(500) PATH '$.BudgetHeadName')) BudList) A;
		
        INSERT INTO TbProjectAuditLog(ProjectID,Message,AuditFrom,AuditEvent,CreatedDate,CreatedBy)
        Select PrjID, CONCAT('Added a new Budget Plan SubHead ''' ,   A.ItemName ,''' in  BudgetHead ''',   A.BudgetHeadName )  Message, 'Finance','ADD',now() CreatedDate, loggedUserId CreatedBy
		From (SELECT  * FROM  JSON_TABLE(BudgetPlanList, '$[*]' COLUMNS (BudgetHeadName varchar(200)  PATH '$.BudgetHeadName',
				NESTED PATH '$.BudgetPlanSubList[*]' COLUMNS ( ItemName  varchar(200)  PATH '$.SubHeaderName') ) )SubList) A;
                
		-- ---
        
        -- INSERT INTO TbProjectAuditLog(ProjectID,AuditFrom,AuditEvent,AuditFromID,FromIDColumnName,Message,CreatedDate,CreatedBy)
--         Select PrjID, 'Finance', 'ADD', Max(ID) AuditFromID, 'ID', CONCAT('Added a new Budget Plan ''',   AA.BudgetHeadName ,'''' )  Message, now() CreatedDate, loggedUserId CreatedBy
--         From  TbBudgetPlan B Inner join 
-- 			(SELECT  * FROM  JSON_TABLE(BudgetPlanList, '$[*]' COLUMNS (BudgetHeadName varchar(200)  PATH '$.BudgetHeadName') ) A) AA
-- 				On  B.BudgetHead =AA.BudgetHeadName  Where B.ProjectId =PrjID  Group by B.ProjectId,AA.BudgetHeadName;
-- 		
--         
--         INSERT INTO TbProjectAuditLog(ProjectID,AuditFrom,AuditEvent,AuditFromID,FromIDColumnName,Message,CreatedDate,CreatedBy)
--         Select PrjID, 'Finance', 'ADD',BI.ID,'ID', CONCAT('Added a new Budget Plan SubHead ''' ,   BI.ItemName ,''' in  BudgetHead ''',   A.BudgetHeadName )  Message, now() CreatedDate, loggedUserId CreatedBy
-- 		From (Select Max(ID) BudgetHeadID,AA.BudgetHeadName 
-- 			From  TbBudgetPlan B Inner join 
-- 				(SELECT  * FROM  JSON_TABLE('[{"Index":1,"BudgetHeadName":"HH1","HeadAmount":1200,"BudgetPlanSubList":[{"SubHeaderName":"SH1","Amount":"1000"},{"SubHeaderName":"SH2","Amount":"200"}]},{"Index":1,"BudgetHeadName":"HH2","HeadAmount":3000,"BudgetPlanSubList":[{"SubHeaderName":"SH2","Amount":"3000"}]}]', 
-- 				'$[*]' COLUMNS (BudgetHeadName varchar(200)  PATH '$.BudgetHeadName') ) A) AA
-- 					On  B.BudgetHead =AA.BudgetHeadName  Where B.ProjectId =7  Group by B.ProjectId,AA.BudgetHeadName) A 
-- 		Inner Join TbBudgetPlanItemDetails BI On BI.BudgetHeadID=A.BudgetHeadID;
        -- ---
                
        Set @flag =1 ;    
    COMMIT;
  
select  @flag TrnStatus;
END$$
DELIMITER ;
