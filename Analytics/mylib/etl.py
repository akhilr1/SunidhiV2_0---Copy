import mylib.db as db;
import mysql.connector;
import collections
from collections import OrderedDict
#db.get_data('select 100');


def get_login_data():
    src_db=db.connect_csr_demo_db()
    #data= db.get_data(src_db,"select Name,EmailId,Phone,UpdatedDate from TbUser order by UpdatedDate desc limit 100;")
    data= db.get_data(src_db,'''SELECT ID,
    Name,
    EmailID,
    PhoneCode,
    PhoneNumber,
    Organisation,
    CurrentPosition,
    CreatedDate
    FROM IMWORK_FORM order by createddate desc ''')
    return data
    
    

def save_benford_analyisis_to_db(prjid,surveyid,questionid,chisquare_score,ISFraudEntry):
     ana_db=db.connect_analytics_db();
     #db.save_data(ana_db,


    
     
def get_survey_data_transposed_direct(surveyid):
    src_db=db.connect_db()
    surveyidArr=str(surveyid).split(",")
    tsurveyidArr=[str(int(x)) for x in surveyidArr]
    strsurveyids="";
    if(len(tsurveyidArr)>1):
        strsurveyids=",".join(tsurveyidArr)
    else:
        strsurveyids=tsurveyidArr[0]


    q=f'''
SELECT DISTINCT  survey.ProjectId
		, survey.SurveyID
        , Qstn.QuestionID
        , section.SectionID
        , response.ID answerid
        , survey.SurveyName
		, CONCAT('(',section.SectionName , ')' , Qstn.QuestionName ) AS QuestionName
		, response.Answer
		, Benef.BeneficiaryID
		, IFNULL(Benef.BeneficiaryName,'') AS BeneficiaryName
		, U.Name AS SubmittedByName
		,U.ID AS SubmittedByUserID
		-- , IFNULL(subTask.WFLevel,0) AS WFLevel
        , IFNULL(WFdetails.WorkflowLevelName,'') AS WFLevel
		, mytask.WFLevelStatus
		, response.CreatedDate AS SubmittedDate
		, response.SyncTaskID
		/*, Qstn.QuestionName AS Beneficiary_Question*/
        , response.Answer AS BenRegQustnAnswer
FROM	TbProjectSurveyResponses response
INNER JOIN	TbMyTask  mytask 
	ON	mytask.SyncTaskID = response.SyncTaskID
LEFT JOIN	TbMyTask subTask 
	ON	response.SyncTaskID = subTask.ParentSyncDBID 
    AND subTask.ParentSyncDBID >0
INNER JOIN	TbProjectSurveyQuestion Qstn 
	ON	Qstn.QuestionID = response.QuestionID
    
LEFT JOIN	TbProjectBeneficiary Benef 
	ON	Benef.BeneficiaryID = response.ResponseID
    AND Benef.IsDeleted=0
INNER JOIN	TbUser U 
	ON	U.ID = mytask.UserID
INNER JOIN	TbProjectSurvey survey 
	ON	survey.SurveyID = response.SurveyID
LEFT JOIN TbProjectWorkflowDetails WFdetails
	ON survey.WorkflowID = WFdetails.WorkflowID
INNER JOIN TbProjectSurveySection section
	ON section.SectionID = Qstn.SectionID


WHERE	mytask.Type ='Survey' 
		AND response.IsActive = 1 
        AND survey.IsActive = 1  
        AND mytask.IsActive =1 
        AND mytask.IsDeleted=0
        AND U.IsDeleted =0
		AND mytask.TypeID IN (%s)
ORDER BY section.SectionID,Qstn.QuestionID
; '''%(strsurveyids)#,surveyid);

    ben_reg_query='''
    SELECT DISTINCT  
		 CONCAT('(Beneficiary) ' ,benquestion.QuestionName  )  AS Question
		, Benef.BeneficiaryID
        ,BenQstnAnswer.Answer as Answer
FROM TbProjectBeneficiary  Benef
JOIN TbUser usr ON usr.ID = Benef.CreatedBy
JOIN TbProjectBeneficiaryType bnftype ON bnftype.BeneficiaryTypeID = Benef.BeneficiaryTypeID
JOIN TbProjectSurvey survey ON survey.BeneficiaryTypeID = bnftype.BeneficiaryTypeID
JOIN TbBeneficiaryTypeQuestion benquestion
ON Benef.BeneficiaryTypeID = benquestion.BeneficiaryTypeID
JOIN TbBeneficiaryRegistrationDetails BenQstnAnswer
ON benquestion.QuestionID = BenQstnAnswer.QuestionID 
AND Benef.BeneficiaryID=BenQstnAnswer.BeneficiaryID
JOIN TbQuestionType QType ON  QType.ID = benquestion.AnswerTypeID

INNER JOIN TbProjectSurveyResponses surveyresponse 
ON surveyresponse.ResponseID=BenQstnAnswer.BeneficiaryID

WHERE ifnull(Benef.IsActive,0)=1 AND ifnull(bnftype.IsActive,0)=1 
AND ifnull(survey.IsActive,0)=1  AND ifnull(survey.IsActive,0)=1 
AND ifnull(benquestion.IsDeleted,0)=0 
AND ifnull(surveyresponse.IsActive,0)=1
-- AND Benef.BeneficiaryID = 1700
AND survey.SurveyID IN (%s)
    '''%(strsurveyids);

    survey_data= db.get_data(src_db,q)
    reg_data=db.get_data(src_db,ben_reg_query)
    dic=OrderedDict();
    lst=[]
    
    dic_reg=OrderedDict()
    for item in reg_data:
        tben=dic_reg.setdefault('reg_'+str(item['BeneficiaryID']),OrderedDict())
        tben[item['Question']]= item['Answer']

    for item in survey_data:
        dic.setdefault(item['SyncTaskID'],{})
        ben = dic[item['SyncTaskID']].setdefault(item['BeneficiaryID'], collections.OrderedDict())
        ben[item['QuestionName']]=item['Answer']
          # Beneficiary questions

        #ben['Beneficiary_Question']=item['Beneficiary_Question']
        #ben['BenRegQustnAnswer']=item['BenRegQustnAnswer']
        #ben[item['Beneficiary_Question']]=item['BenRegQustnAnswer']
        #ben['Type']=item['Type']
    for item in survey_data:
        ben = dic[item['SyncTaskID']].setdefault(item['BeneficiaryID'], collections.OrderedDict())
        if 'reg_'+str(item['BeneficiaryID']) in dic_reg:
            for (key, value) in dic_reg['reg_'+str(item['BeneficiaryID'])].items():
                ben[key] = value;

    for item in survey_data:
        ben = dic[item['SyncTaskID']].setdefault(item['BeneficiaryID'], collections.OrderedDict())
        ben['WFLevel'] = item['WFLevel']
        ben['WFLevelStatus'] = item['WFLevelStatus']
        ben['BeneficiaryName'] = item['BeneficiaryName']
        ben['SubmittedByName'] = item['SubmittedByName']
        ben['SubmittedDate'] = item['SubmittedDate']
        ben['SurveyName'] = item['SurveyName']
        ben["SubmittedByUserID"] = item['SubmittedByUserID']
        ben['SyncTaskID'] = item['SyncTaskID']
        ben['SurveyID'] = item['SurveyID']

        
    for k1 in dic:
        for k2 in dic[k1]:
            lst.append(dic[k1][k2]);
    
    
    return lst;
        
    
    
