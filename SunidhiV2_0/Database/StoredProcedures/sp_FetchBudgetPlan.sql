USE SUNIDHI_CORE_V2_0;
DROP procedure IF EXISTS `sp_FetchBudgetPlan`;

DELIMITER $$
CREATE DEFINER=`user`@`%` PROCEDURE `sp_FetchBudgetPlan`(IN ProjectId INT)
BEGIN
DECLARE flag INT ; -- VARCHAR(2000);

DECLARE ifExists TINYINT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		Set @flag="Failed";
        SELECT @flag ; 
        ROLLBACK ;     
    END;  
    
START TRANSACTION ; 
	

SET SESSION group_concat_max_len = 100000;

 select json_object(
  'Name', BudgetHead,'Amount',sum(Amount),'BudgetHeadID',BP.ID
  ,'children',(select CAST(CONCAT('[',
                GROUP_CONCAT(
                  JSON_OBJECT(
                    'Name', ItemName,'Amount',Amount,'BudgetItemID',LB.ID)),
                ']')
         AS JSON) from  TbBudgetPlanItemDetails LB 
                                where LB.BudgetHeadID=BP.ID and LB.IsDeleted=0 ) 

  )js
 from TbBudgetPlan BP inner join TbBudgetPlanItemDetails LB 
                                on BP.ID=LB.BudgetHeadID
                                where BP.ProjectId=ProjectId and BP.IsDeleted=0 group by BudgetHead,BP.ID;
END$$
DELIMITER ;
