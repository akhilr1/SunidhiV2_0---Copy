import mysql.connector;
import config;

def connect_mysql_db(host,user,pwd,db_name):
    con = mysql.connector.connect(
    host=host,
    user=user,
      
    passwd=pwd,
    auth_plugin='mysql_native_password',
    database=db_name,
    compress=True, #use mysql-connector-python instead of mysql-connector
    allow_local_infile = True,
    raise_on_warnings = True,
      #autocommit=True
    #use_pure=True
    )
    return con;
     
def connect_csr_demo_db():
     db_con=connect_mysql_db("10.0.10.1","user","HWf6frqGoR61","SUNIDHI_CSR_DEMO")
     return db_con;

def connect_analytics_db():
    db_con=connect_mysql_db("10.0.10.1","user","HWf6frqGoR61","IMWORKS_ANALYTICS_TEST")
    return db_con;

def connect_src_db():
    db_con=connect_mysql_db("10.0.10.1","user","HWf6frqGoR61","SUNIDHI_CORE_V2_0_DEMO") #"SUNIDHI_CORE_V2_0_TEST")#
    return db_con;

def connect_test_db():
    db_con=connect_mysql_db("10.0.10.1","user","HWf6frqGoR61","SUNIDHI_CORE_V2_0_TEST")#
    return db_con;

def connect_db():
    db_con = connect_mysql_db(config.db["host"], config.db["user"], config.db["password"], config.db["database"])
    return db_con;


def get_data(con,query):
    db_cursor=con.cursor(dictionary=True);
    db_cursor.execute(query);
    results=db_cursor.fetchall()
    #print(results)
  
    return results;

def save_data(con,query,data):
    cursor=con.cursor(dictionary=True);
    try:
        cursor.executemany(query,data)
        con.commit();
    finally:
        con.rollback();
    #cursor.commit();
    