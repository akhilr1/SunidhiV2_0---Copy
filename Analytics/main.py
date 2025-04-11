from flask import Flask,Response,request,render_template,Blueprint,json
from flask import jsonify
from mylib import benfords
from flask import render_template,make_response
import io
import mylib.etl as etl;
import ujson
import random
import gzip
from matplotlib.backends.backend_agg import FigureCanvasAgg as FigureCanvas
from matplotlib.figure import Figure

bp = Blueprint('burritos', __name__,
                        template_folder='templates')
app = Flask(__name__, static_url_path='/analytics/static/')
#prevent sorting ordered dictionary
app.config['JSON_SORT_KEYS'] = False


from functools import wraps
from flask import request

def http_login_required(f):
    @wraps(f)
    def wrapped_view(**kwargs):
        auth = request.authorization
        if not (auth and check_auth(auth.username, auth.password)):
            return ('Unauthorized', 401, {
                'WWW-Authenticate': 'Basic realm="Login Required"'
            })

        return f(**kwargs)

    return wrapped_view

def check_auth(user,pwd):
    if user == 'blbi' and pwd=='billionlives':
        return True;
    return False;
    
    

@app.route('/')
def index():
    return "<h1>Welcome</h1>"

@bp.route('/')
def index_analytics():
    return "<h1>Welcome to Analytics</h1>"

@bp.route('/getbenford')
def get_benford():
    prjid = request.args.get('prjid')
    
    surveyid = request.args.get('surveyid')
    
   
    return  jsonify(benfords.get_benford_survey_results(prjid,surveyid)); #benfords.get_benford_anlysis();
    
@bp.route('/plotbenford')
def plot_benford():
    
    fig = Figure()
    output = io.BytesIO()
   
    plt= fig.add_subplot(1, 1, 1);
    x=[1,2,3,4,5,6,7,8,9]
    
    benford_table=benfords.get_benford_anlysis()
    
    plt.plot(x, [t*100 for t in benford_table['data_freq_per']], label = "data")

    plt.plot(x, [t*100 for t in benford_table['benford_freq_per']], label = "benford")
    plt.set_xlabel('Digits')
    plt.set_ylabel('Frequency (%)');
    plt.legend()
    FigureCanvas(fig).print_png(output)
    return Response(output.getvalue(), mimetype='image/png')
    

@bp.route('/getpivotdata')
def get_pivot_data():
    surveyid = request.args.get('surveyid')
    #print(surveyid);
    #848
    compression_level=5
    data=etl.get_survey_data_transposed_direct(surveyid) #get_survey_data(surveyid);
    content = gzip.compress(json.dumps(data).encode('utf8'),
                            compression_level)  # ujson.dumps(data).encode('utf8'), compression_level)
    response = make_response(content)
    response.headers['Content-length'] = len(content)
    response.headers['Content-Encoding'] = 'gzip'
    return jsonify(data)

    
@bp.route('/pivot')
def pivot_grid():
    return render_template('pivot.html')


@bp.route("/reports/loginreport")
@http_login_required
def report_login_data_page():
    return render_template('loginreport.html')

@bp.route("/reports/getlogindata")
@http_login_required
def get_report_login_data_json():
    compression_level = 5
    data=etl.get_login_data();
    content = gzip.compress(ujson.dumps(data).encode('utf8'), compression_level)
    response = make_response(content)
    response.headers['Content-length'] = len(content)
    response.headers['Content-Encoding'] = 'gzip'
    #return jsonify(data)
    return response


#This should be at bottom after all routes are registered (?)
app.register_blueprint(bp, url_prefix='/analytics')

if __name__ == "__main__":
    
    print("hello");
    app.run(port=8003,debug=True)