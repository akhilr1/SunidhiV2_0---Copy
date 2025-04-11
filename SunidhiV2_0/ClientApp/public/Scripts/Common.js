
function RepeatVal_Aggregate(strVal) {
   
    var values = strVal.split(",");
    var sum = 0; 
    for (var i = 0; i < values.length; i++) {
        sum = sum + parseInt(values[i]);   
    }
       
    return sum;
}


function RepeatVal_Aggregate_Multiplication(strVal) {

    var values = strVal.split(",");
    var mul = 0;
    for (var i = 0; i < values.length; i++) {
        if (i == 0) {
            mul =  parseInt(values[i]);
        } else {
            mul = mul * parseInt(values[i]);
        }
    }

    return mul;
}


function RepeatVal_Aggregate_Subtraction(strVal) {

    var values = strVal.split(",");
    var sub = 0;
    for (var i = 0; i < values.length; i++) {
        if (i == 0) {
            sub = parseInt(values[i]);
        } else {
            sub = sub - parseInt(values[i]);
        }
    }

    return sub;
}

//===for Testing Purpose====//
function Percentage(Maxval, TotVal) {
  
    var result = (TotVal / Maxval) * 100;

    return result;
}

function RepeatVal_Aggregate_Average(strVal) {
    var values = strVal.split(",");
    var sum = 0;
    for (var i = 0; i < values.length; i++) {
        sum = sum + parseInt(values[i]);
    }
      
    return sum / (values.length);
}