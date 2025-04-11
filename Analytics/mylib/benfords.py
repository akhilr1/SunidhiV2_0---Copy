import collections;
import random;
import matplotlib.pyplot as plt

#import db
import pandas 

BENFORD_PERCENTAGES = [0, 0.301, 0.176, 0.125, 0.097, 0.079, 0.067, 0.058, 0.051, 0.046]

import mylib.db as db;


def analyze_benford(data):
    
    """
    Calculates a set of values from the numeric list
    input data showing how closely the first digits
    fit the Benford Distribution.
    Results are returned as a list of dictionaries.
    """

    results = []

    first_digits = list(map(lambda n: str(n)[0], data))
    first_digit_frequencies = collections.Counter(first_digits)

    for n in range(1, 10):

        data_frequency = first_digit_frequencies[str(n)]
        data_frequency_percent = data_frequency / len(data)
        benford_frequency = len(data) * BENFORD_PERCENTAGES[n]
        benford_frequency_percent = BENFORD_PERCENTAGES[n]
        difference_frequency = data_frequency - benford_frequency
        difference_frequency_percent = data_frequency_percent - benford_frequency_percent

        results.append({"n": n,
                        "data_frequency":               data_frequency,
                        "data_frequency_percent":       data_frequency_percent,
                        "benford_frequency":            benford_frequency,
                        "benford_frequency_percent":    benford_frequency_percent,
                        "difference_frequency":         difference_frequency,
                        "difference_frequency_percent": difference_frequency_percent})

    return results 



def get_random_data(n):

    """
    Returns a list of 1000 numbers approximately
    following the uniform distribution NOT the
    Benford Distribution.
    """

    random_data = [0] * 1000

    random_data = list(map(lambda l: l + random.randint(1, n), random_data))

    return random_data


def get_benford_survey_results(prjid,surveyid):
    con=db.connect_analytics_db();
    data=db.get_data(con,"select * from BenFordResults where projectid=%s and SurveyId =%s"%(prjid,surveyid));
    return data;

    

def get_benford_data(n):

    """
    Returns a list of about 1000 (n) numbers
    approximately following the Benford Distribution.
    """

    benford_data = []

    for first_digit in range(1, 10):
        random_factor = random.uniform(0.8, 1.2)
        for num_count in range(1, int(n * BENFORD_PERCENTAGES[first_digit] * random_factor)):
            start = first_digit * n
            benford_data.append(random.randint(start, start + n))

    return benford_data


#get_benford_data(100)

#print(get_benford_data(100))
data={};

def get_benford_data_from_db():
    
    con=db.connect_analytics_db();
    global data
    data=db.get_data(con,"select projectid,surveyid,questionid,surveyname,questionname,answer from SurveyData where type='numeric'")
    #df=pandas.DataFrame.from_dict(data);
    
    #print(df.groupby(["projectid", "surveyname"]).count());
    #d=[x for x in data.]
    dic={}
    #print(data)
    for prj in data:
         if not prj['answer'] or prj['answer'].strip()=='':
            continue;
         dic.setdefault(prj['projectid'],{});
       
         dic[prj['projectid']].setdefault(prj['surveyid'],{});
        
         dic[prj['projectid']][prj['surveyid']].setdefault(prj['questionid'],{});
        
         dic[prj['projectid']][prj['surveyid']][prj['questionid']].setdefault('answers',[])
        
         dic[prj['projectid']][prj['surveyid']][prj['questionid']]['answers'].append(prj['answer']);   
         
         
         
         
        
    lst=[];
    for k1,v1 in dic.items():
        for k2,v2 in v1.items():
            for k3,v3 in v2.items():
                #print(v2)
                #Per questioid answers are not considered yet. All wneric type answers per surbey are bunched together.
                t=get_benford_anlysis(v3['answers'])
                d1={'projectid':k1,
                    'surveyid':k2,
                    'questionid':k3,
                    'chisquarescore':t['chi_sqr_score']['ChiSquareScore'],
                    'isfraudentry':t['chi_sqr_score']['IsFraudEntry']}
                lst.append(d1)
                
                
        
    #print(dic);.
    #print(lst);
    db.save_data(con,"insert ignore into BenFordResults(ProjectId,SurveyID,QuestionID,ChiSquareScore,IsFraudEntry) values(%s,%s,%s,%s,%s)",[tuple(x.values()) for x in lst ]);
    


def print_as_table(benford_table):

    width = 59

    print("-" * width)
    print("|   |      Data       |    Benford      |    Difference   |")
    print("| n |  Freq     Pct   |  Freq     Pct   |  Freq     Pct   |")
    print("-" * width)

    for item in benford_table:

        print("| {} | {:6.0f} | {:6.2f} | {:6.0f} | {:6.2f} | {:6.0f} | {:6.2f} |".format(item["n"],
                                   item["data_frequency"],
                                   item["data_frequency_percent"] * 100,
                                   item["benford_frequency"],
                                   item["benford_frequency_percent"] * 100,
                                   item["difference_frequency"],
                                   item["difference_frequency_percent"] * 100))

    print("-" * width)


def print_as_graph(benford_table):

    REDBG = "\x1B[41m"
    GREENBG = "\x1B[42m"
    RESET = "\x1B[0m"

    print("  0%       10%       20%       30%       40%       50%       60%       70%       80%       90%       100%")
    print("  |         |         |         |         |         |         |         |         |         |         |\n")

    for item in benford_table:

        print(" {} {}\n   {}\n  ".format(str(item["n"]),
                                         GREENBG + (" " * int(round(item["benford_frequency_percent"] * 100))) + RESET,
                                         REDBG + (" " * int(round(item["data_frequency_percent"] * 100))) + RESET))



def chisq_stat(O, E):
        #Critical value at a P-value of 0.05 is 15.51 .Degrees of freedom =8
        val=sum( (o - e)**2/e for (o, e) in zip(O, E) )
        return {"ChiSquareScore":val,"IsFraudEntry":val>15.51}


def get_benford_anlysis(data=None):
    if data is None:
        data =get_benford_data(1000) 
    benford_table = analyze_benford(data)
    chi_result=chisq_stat([x['data_frequency'] for x in benford_table],[x["benford_frequency"] for x in benford_table]);
    return {'data_freq_per':[x['data_frequency_percent']*100 for x in benford_table],
            'benford_freq_per':[x['benford_frequency_percent']*100 for x in benford_table],
            'chi_sqr_score':chi_result
            
            }
 
    
    

#data   =get_benford_data(1000) #
    #data = get_benford_data()
    #get_random_data(1000) 
    #=get_random_data(1000)

#print(len(data))

#benford_table = analyze_benford(data)
#print_as_table(benford_table)
#print()
#print_as_graph(benford_table)
#chi_result=chisq_stat([x['data_frequency'] for x in benford_table],[x["benford_frequency"] for x in benford_table]);
                      

#print("chis quare result:"+str(chi_result));

#print(benford_table)
#x=[1,2,3,4,5,6,7,8,9]
#plt.plot(x, [x['data_frequency_percent']*100 for x in benford_table], label = "data")

#plt.plot(x, [x['benford_frequency_percent']*100 for x in benford_table], label = "benford")
#plt.xlabel('Digits')
#plt.ylabel('Frequency (%)');
#plt.legend()
#plt.show()



  

#print(db.get_data("select 1 as one"));

#analyze_benford([1,2,3,4])