using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using NuGet;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System.Globalization;

namespace SunidhiV2_0.Controllers
{
    public class SurveyResponseImportController : Controller
    {

        public IActionResult GetQuestionDetails()
        {
           

        
            // try 
            //{
            string param1 = Request.Query["param1"];
            int surveyId;

            if (!int.TryParse(param1, out surveyId))
            {
                // Handle invalid parameter value for param1
            }
            List<KeyValuePair<int, string>> lstBeneficiaryType = new List<KeyValuePair<int, string>>();



            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();

            SurveyResponseImportDAL objSurvey = new SurveyResponseImportDAL();
            List<SurveySectionLogic> lstSurveySectionLogic = new List<SurveySectionLogic>();
            List<QuestionCondition> lstQuestionCondition = new List<QuestionCondition>();
            var logicQuestionString = "";
            string dropdownRangeInStr = "A1:A1";
            string ConditionQuestionAnswer = "";
            string SkipLogicConditionOperator = "=";
            string PhoneValidationRangeInStr = "A1:A1,";
            string AadhaarValidationRangeInStr = "A1:A1,";
            string EmailValidationRangeInStr = "A1:A1,";
            string GPSValidationRangeInStr = "A1:A1,";
            string MinMaxValidationRangeInStr = "A1:A1,";
            string NumericRangeInStr = "A1:A1,";
            string DateRangeInStr = "A1:A1,";
            string greaterValidationRangeInStr = "A1:A1,";
            string lessValidationRangeInStr = "A1:A1,";
            string ConditionQuestionRange = "A1:A1";
            string RdropdownRangeInStr = "A1:A1";
            string RPhoneValidationRangeInStr = "A1:A1,";
            string RAadhaarValidationRangeInStr = "A1:A1,";
            string REmailValidationRangeInStr = "A1:A1,";
            string RGPSValidationRangeInStr = "A1:A1,";
            string RMinMaxValidationRangeInStr = "A1:A1,";
            string RNumericRangeInStr = "A1:A1,";
            string RDateRangeInStr = "A1:A1,";
            string RgreaterValidationRangeInStr = "A1:A1,";
            string RlessValidationRangeInStr = "A1:A1,";

            string REmailStr = "A1:A1,";
            string RGPSStr = "A1:A1,";
            string RNumericStr = "A1:A1,";
            string RDateStr = "A1:A1,";

            string EmailStr = "A1:A1,";
            string GPSStr = "A1:A1,";
            string NumericStr = "A1:A1,";
            string DateStr = "A1:A1,";
            string MandatoryStr = "A1:A1,";


            Double minValue = 0;
            Double maxValue = 0;
            var newCol = 0;
            string baseName = "Repeat";
            string newName = baseName;
            int counter = 1;
            int colunmrange = 10;
           // List<ExcelRange> rangeList = new List<ExcelRange>();
            //ExcelRange[] rangeArray = rangeList.ToArray();
            
            List<SurveyQuestion> lstSurveyQuestion = objSurvey.GetQuestionDetails(surveyId);
            //var stream = new MemoryStream();
            lstBeneficiaryType = objProjectSurveyDAL.GetBeneficiaryData(surveyId);


            // ExcelPackage excelPackage = new ExcelPackage(memoryStream);
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.CreateVBAProject();
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Main");
                // setting the propertiess
                // of the work sheet 
                worksheet.TabColor = System.Drawing.Color.Black;
                worksheet.DefaultRowHeight = 12;

                // Setting the properties
                // of the first row
                worksheet.Row(1).Height = 20;
                worksheet.Row(1).Height = 20;
                worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(1).Style.Font.Bold = true;

                //  string path = _hostingEnvironment.WebRootPath;
                if (lstBeneficiaryType.Count > 0)
                {
                    newCol = 3;
                    colunmrange = lstBeneficiaryType.Count + 1;
                    worksheet.Cells[1, 1].Value = "BeneficiaryID";
                    worksheet.Cells[1, 2].Value = "BeneficiaryName";
                    worksheet.Cells[1, 3].Value = "SubmittedDate";
                    ExcelRange DateRange = worksheet.Cells[worksheet.Cells[2, newCol] + ":" + worksheet.Cells[colunmrange, newCol]];
                    DateStr += DateRange + ",";
                    DateRangeInStr = DateStr.ToString();

                    for (int i = 1; i < lstBeneficiaryType.Count + 1; i++)
                    {
                        KeyValuePair<int, string> kvp = lstBeneficiaryType[i - 1];
                        worksheet.Cells[i + 1, 1].Value = kvp.Key;
                        worksheet.Cells[i + 1, 2].Value = kvp.Value;
                    }


                }
                else
                {
                    newCol = 1;
                    worksheet.Cells[1, 1].Value = "SubmittedDate";
                    ExcelRange DateRange = worksheet.Cells[worksheet.Cells[2, newCol] + ":" + worksheet.Cells[colunmrange, newCol]];
                    DateStr += DateRange + ",";
                    DateRangeInStr = DateStr.ToString();
                }
                
                for (int col = 0; col < lstSurveyQuestion.Count; col++)
                {
                    lstQuestionCondition = lstSurveyQuestion[col].lstQuestionCondition;
                    var lstMandatoryQuestion = lstSurveyQuestion[col].IsMandatory;
                    worksheet.Cells[1, newCol + 1].Value = lstSurveyQuestion[col].QuestionName;
                     if ( lstSurveyQuestion[col].IsMandatory == true)
                    {
                        ExcelRange MandatoryRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                        MandatoryStr += MandatoryRange + ",";
                    }
                    if (lstSurveyQuestion[col].QuestionType == "Single Line Text")
                    {
                       

                        if (lstQuestionCondition.Count > 0)
                        {

                            if (lstQuestionCondition[0].ValidationCondition == "Email")
                            {
                                ExcelRange ValidationRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                                EmailStr += ValidationRange + ",";
                                EmailValidationRangeInStr = EmailStr.ToString();

                            }
                        }   

                    }

                    if (lstSurveyQuestion[col].QuestionType == "Numeric")
                    {
                        ExcelRange ValidationRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                        NumericStr += ValidationRange + ",";


                        if (lstQuestionCondition.Count > 0)
                        {

                            if (lstQuestionCondition[0].ValidationCondition == "Phone Number")
                            {

                                PhoneValidationRangeInStr += ValidationRange + ",".ToString();

                            }
                            else if (lstQuestionCondition[0].ValidationCondition == "Aadhaar Number")
                            {
                                AadhaarValidationRangeInStr = ValidationRange + ",".ToString();
                            }
                           

                            else if (lstQuestionCondition[0].ValidationCondition == "Min & Max Range")
                            {
                                minValue = lstQuestionCondition[0].MinValue;
                                maxValue = lstQuestionCondition[0].MaxValue;
                                MinMaxValidationRangeInStr = ValidationRange + ",".ToString();
                            }
                        }
                        else
                        {
                            NumericRangeInStr = NumericStr.ToString();

                        }

                    }

                    if (lstSurveyQuestion[col].QuestionType == "Date Picker")
                    {
                        
                        ExcelRange DateRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];

                        DateStr += DateRange + ",";
                        DateRangeInStr = DateStr.ToString();
                        
                        

                        if (lstQuestionCondition.Count > 0)
                        {

                            if (lstQuestionCondition[0].ValidationCondition == "Should be greater than Current date")
                            {

                                greaterValidationRangeInStr = DateRange + ",".ToString();

                            }
                            else if (lstQuestionCondition[0].ValidationCondition == "Should be less than Current date")
                            {
                                lessValidationRangeInStr = DateRange + ",".ToString();
                            }
                        }
                    }
                    if (lstSurveyQuestion[col].QuestionType == "GPS Capture")
                    {
                        ExcelRange GpsRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];
                        GPSStr += GpsRange + ",";
                        GPSValidationRangeInStr = GPSStr.ToString();
                    }

                    if (lstSurveyQuestion[col].QuestionType == "Dropdown")
                    {
                        List<string> optionlist = new List<string>();

                        for (var j = 0; j < lstSurveyQuestion[col].lstQuestionOptions.Count; j++)
                        {
                            optionlist.Add(lstSurveyQuestion[col].lstQuestionOptions[j].OptionValue);

                        }

                        ExcelRange dropdownRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];

                        // Create a data validation object
                        var dataValidation = worksheet.DataValidations.AddListValidation(dropdownRange.Address);
                        // var dataValidation = worksheet.DataValidations.AddListValidation("D2:D100");
                        dataValidation.ShowErrorMessage = true;
                        dataValidation.ErrorTitle = "Invalid Value";
                        dataValidation.Error = "Please select a value from the dropdown list.";
                        dataValidation.Formula.Values.AddRange(optionlist);

                    }

                    if (lstSurveyQuestion[col].QuestionType == "Single Choice" || lstSurveyQuestion[col].QuestionType == "Likert Scale")
                    {
                        List<string> optionlist = new List<string>();

                        for (var j = 0; j < lstSurveyQuestion[col].lstQuestionOptions.Count; j++)
                        {
                            optionlist.Add(lstSurveyQuestion[col].lstQuestionOptions[j].OptionValue);

                        }
                        // set the cell range where you want to create the dropdown
                        ExcelRange dropdownRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];

                        // create a data validation object
                        var dataValidation = worksheet.DataValidations.AddListValidation(dropdownRange.Address);

                        // set the formula to restrict the input to a single choice

                        //string formula = string.Join(",", optionlist.Select(option => "'" + option + "'"));
                        //dataValidation.Formula.ExcelFormula = $"=COUNTIF({dropdownRange.Address},\"={formula}\")=1";

                        // optional: set a custom input message for the dropdown
                        dataValidation.ShowErrorMessage = true;
                        dataValidation.ErrorTitle = "invalid value";
                        dataValidation.Error = "please select a value from the dropdown.";
                        dataValidation.Formula.Values.AddRange(optionlist);

                    }

                    if (lstSurveyQuestion[col].QuestionType == "Multi-Choice")
                    {
                        List<string> multiChoiceOptions = new List<string>();

                        for (var j = 0; j < lstSurveyQuestion[col].lstQuestionOptions.Count; j++)
                        {
                            multiChoiceOptions.Add(lstSurveyQuestion[col].lstQuestionOptions[j].OptionValue);
                        }
                        // set the cell range where you want to create the dropdown

                        ExcelRange dropdownRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];
                        dropdownRangeInStr = dropdownRange.ToString();
                        var validationCell = worksheet.DataValidations.AddListValidation(dropdownRange.Address);
                        //validationCell.Formula.Values.Add("a");
                        //validationCell.Formula.Values.Add("b");
                        //validationCell.Formula.Values.Add("c");
                        validationCell.Formula.Values.AddRange(multiChoiceOptions);

                    }
                    if (lstSurveyQuestion[col].IsConditionQuestion == true)
                    {

                        lstSurveySectionLogic = lstSurveyQuestion[col].lstSurveySectionLogic;
                        var benTypes = lstSurveySectionLogic.Where(element => element.LogicType == "Repeat Logic");
                        if (benTypes.Any())
                        {
                            bool worksheetExists = excelPackage.Workbook.Worksheets.Any(ws => ws.Name == "Repeat");

                            if (worksheetExists)
                            {
                                counter++;
                                newName = $"{baseName}{counter}";
                            }

                            string worksheetSubName = newName;
                            ExcelWorksheet worksheetsub = excelPackage.Workbook.Worksheets.Add(worksheetSubName);

                            // worksheet.Cells[1, col + 1].Value = lstSurveyQuestion[col].QuestionName;
                            string worksheetName = newName;

                            //worksheet.Cells["A2"].Hyperlink = new ExcelHyperLink(....)
                            ExcelRange dropdownRange = worksheet.Cells["A2"];

                            ExcelRange cell = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];

                            // Define the name and target range
                            //string rangeName = "LogicList_9837";
                            ExcelRange targetRange = worksheetsub.Cells[dropdownRange.Address];

                            // Create the named range
                            ExcelNamedRange namedRange = worksheet.Names.Add(worksheetSubName, targetRange);

                            // Set the hyperlink to the cell using the named range
                            cell.Hyperlink = new ExcelHyperLink(worksheetSubName, "Open Logic List");

                            worksheetsub.Cells[1, 1].Value = "MainRowNo";
                            for (int i = 0; i < lstSurveySectionLogic.Count; i++)
                            {
                                worksheetsub.Cells[1, i + 2].Value = lstSurveySectionLogic[i].QuestionName;
                                if (lstSurveySectionLogic[i].QuestionType == "Single Line Text")
                                {
                                   

                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Email")
                                        {
                                            ExcelRange ValidationRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];
                                            REmailStr += ValidationRange + ",";
                                            REmailValidationRangeInStr = REmailStr.ToString();


                                        }
                                    }

                                }
                                if (lstSurveySectionLogic[i].QuestionType == "Numeric")
                                {
                                    ExcelRange ValidationRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];
                                    RNumericStr += ValidationRange + ",";

                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Phone Number")
                                        {

                                            RPhoneValidationRangeInStr  += ValidationRange + ",".ToString();

                                        }
                                        else if (lstQuestionCondition[0].ValidationCondition == "Aadhaar Number")
                                        {
                                            RAadhaarValidationRangeInStr  += ValidationRange + ",".ToString();
                                        }
                                        else if (lstQuestionCondition[0].ValidationCondition == "Email")
                                        {
                                            REmailValidationRangeInStr += ValidationRange + ",".ToString();
                                        }

                                        else if (lstQuestionCondition[0].ValidationCondition == "Min & Max Range")
                                        {
                                            minValue = lstSurveyQuestion[col].MinValue;
                                            maxValue = lstSurveyQuestion[col].MaxValue;
                                            RMinMaxValidationRangeInStr += ValidationRange + ",".ToString();
                                        }
                                    }
                                    else
                                    {
                                        RNumericRangeInStr = RNumericStr.ToString();

                                    }

                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Date Picker")
                                {
                                    ExcelRange DateRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];
                                    RDateStr += DateRange + ",";
                                    RDateRangeInStr = RDateStr.ToString();
                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Should be greater than Current date")
                                        {

                                            RgreaterValidationRangeInStr += DateRange + ",".ToString();

                                        }
                                        else if (lstQuestionCondition[0].ValidationCondition == "Should be less than Current date")
                                        {
                                            RlessValidationRangeInStr += DateRange + ",".ToString();
                                        }
                                    }
                                }
                                if (lstSurveySectionLogic[i].QuestionType == "GPS Capture")
                                {
                                    ExcelRange GpsRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];
                                    RGPSStr += GpsRange + ",";
                                    RGPSValidationRangeInStr = RGPSStr.ToString();
                                }
                                // ExcelRange range = worksheetsub.Cells["A1:A10"];
                                if (lstSurveySectionLogic[i].QuestionType == "Dropdown")
                                {
                                    List<string> optionlist = new List<string>();

                                    for (var j = 0; j < lstSurveySectionLogic[i].lstQuestionOptions.Count; j++)
                                    {
                                        optionlist.Add(lstSurveySectionLogic[i].lstQuestionOptions[j].OptionValue);

                                    }

                                    ExcelRange dropdownRangeNew = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];

                                    // Create a data validation object
                                    var dataValidation = worksheetsub.DataValidations.AddListValidation(dropdownRangeNew.Address);
                                    // var dataValidation = worksheet.DataValidations.AddListValidation("D2:D100");
                                    dataValidation.ShowErrorMessage = true;
                                    dataValidation.ErrorTitle = "Invalid Value";
                                    dataValidation.Error = "Please select a value from the dropdown list.";
                                    dataValidation.Formula.Values.AddRange(optionlist);

                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Single Choice" || lstSurveySectionLogic[i].QuestionType == "Likert Scale")
                                {
                                    List<string> optionlist = new List<string>();

                                    for (var j = 0; j < lstSurveySectionLogic[i].lstQuestionOptions.Count; j++)
                                    {
                                        optionlist.Add(lstSurveySectionLogic[i].lstQuestionOptions[j].OptionValue);

                                    }

                                    // set the cell range where you want to create the dropdown
                                    dropdownRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];

                                    // create a data validation object
                                    var dataValidation = worksheetsub.DataValidations.AddListValidation(dropdownRange.Address);

                                    // set the formula to restrict the input to a single choice

                                    //string formula = string.Join(",", optionlist.Select(option => "'" + option + "'"));
                                    //dataValidation.Formula.ExcelFormula = $"=COUNTIF({dropdownRange.Address},\"={formula}\")=1";

                                    // optional: set a custom input message for the dropdown
                                    dataValidation.ShowErrorMessage = true;
                                    dataValidation.ErrorTitle = "invalid value";
                                    dataValidation.Error = "please select a value from the dropdown.";
                                    dataValidation.Formula.Values.AddRange(optionlist);


                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Multi-Choice")
                                {
                                    List<string> multiChoiceOptions = new List<string>();

                                    for (var j = 0; j < lstSurveySectionLogic[i].lstQuestionOptions.Count; j++)
                                    {
                                        multiChoiceOptions.Add(lstSurveySectionLogic[i].lstQuestionOptions[j].OptionValue);
                                    }
                                    // set the cell range where you want to create the dropdown

                                    dropdownRange = worksheetsub.Cells[worksheetsub.Cells[2, i + 2] + ":" + worksheetsub.Cells[100, i + 2]];
                                    RdropdownRangeInStr = dropdownRange.ToString();
                                    var validationCell = worksheetsub.DataValidations.AddListValidation(dropdownRange.Address);
                                    //validationCell.Formula.Values.Add("a");
                                    //validationCell.Formula.Values.Add("b");
                                    //validationCell.Formula.Values.Add("c");
                                    validationCell.Formula.Values.AddRange(multiChoiceOptions);

                                }
                            }
                            worksheetsub.TabColor = System.Drawing.Color.Black;
                            worksheetsub.DefaultRowHeight = 12;

                            ////// setting the properties
                            ////// of the first worksheetsub
                            worksheetsub.Row(1).Height = 20;
                            worksheetsub.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheetsub.Row(1).Style.Font.Bold = true;

                            REmailValidationRangeInStr = REmailValidationRangeInStr.TrimEnd(',');
                            RNumericRangeInStr = RNumericRangeInStr.TrimEnd(',');
                            RPhoneValidationRangeInStr = RPhoneValidationRangeInStr.TrimEnd(',');
                            RAadhaarValidationRangeInStr = RAadhaarValidationRangeInStr.TrimEnd(',');
                            REmailValidationRangeInStr = REmailValidationRangeInStr.TrimEnd(',');
                            RMinMaxValidationRangeInStr = RMinMaxValidationRangeInStr.TrimEnd(',');
                            RDateRangeInStr = RDateRangeInStr.TrimEnd(',');
                            RgreaterValidationRangeInStr = RgreaterValidationRangeInStr.TrimEnd(',');
                            RlessValidationRangeInStr = RlessValidationRangeInStr.TrimEnd(',');
                            RGPSValidationRangeInStr = RGPSValidationRangeInStr.TrimEnd(',');
                            #region RepeatVBA
                            string codeRepeat = @"Sub Worksheet_Change(ByVal Target As Range)

                                            Dim rng As Range
                                                     Dim rngAadhaar As Range
                                                     Dim rngEmail As Range
                                                     Dim regex As Object
                                                        Dim regexEmail As Object
                                                        Dim regexGPS As Object
                                                     Dim regexAadhaar As Object
                                                     Dim minimumValue As Double
                                                     Dim maximumValue As Double
                                                     Dim rngMinMax As Range
                                                     Dim rngNumeric As Range
                                                     Dim rngDate As Range
                                                     Dim regexPattern As String
                                                    Dim currentDate As Date
                                                    Dim rngDategreater As Range
                                                    Dim rngDateless As Range
                                                    Dim rngGps As Range
                                                    Dim dropdownRangeAdd As Range



                                                 Set rng =Range (""PhoneValidationRangeInStr"")'
                                                 Set regex = CreateObject(""VBScript.RegExp"")
                                                 regex.Pattern = ""^\d{10}$"" 
    
                                                 Set rngAadhaar =Range (""AadhaarValidationRangeInStr"")' 
                                                 Set regexAadhaar = CreateObject(""VBScript.RegExp"")
                                                 regexAadhaar.Pattern = ""^\d{12}$""

                                                 Set rngEmail =Range (""EmailValidationRangeInStr"")' 
                                                 Set regexEmail = CreateObject(""VBScript.RegExp"")
                                                 regexEmail.Pattern = ""^[\w\.-]+@[\w\.-]+\.\w+$""

                                                 Set rng =Range (""PhoneValidationRangeInStr"")'
                                                 Set regex = CreateObject(""VBScript.RegExp"")
                                                 regex.Pattern = ""^\d{10}$""

                                                 Set rngGps =Range (""GPSValidationRangeInStr"")'
                                                 Set regexGPS = CreateObject(""VBScript.RegExp"")
                                                 regexGPS.Pattern = ""^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$""

                                                 Set rngMinMax =Range (""MinMaxValidationRangeInStr"")' 
                                                minimumValue = minValue ' Specify the minimum value
                                                maximumValue = maxValue ' Specify the maximum value

                                            set rngNumeric=Range (""NumericRangeInStr"")

                                            set rngDate=Range (""DateRangeInStr"")
                                            set rngDategreater=Range (""greaterValidationRangeInStr"")
                                            set rngDateless=Range (""lessValidationRangeInStr"")



                                                ' Set the range of cells to be protected
                                                Set dropdownRangeAdd = Range (""dropdownRangeInStr"")'
                                                On Error GoTo Exitsub
 If Not Intersect(Target, dropdownRangeAdd) Is Nothing Then
            If Target.SpecialCells(xlCellTypeAllValidation) Is Nothing Then
            GoTo Exitsub
            Else: If Target.Value = """" Then GoTo Exitsub Else
                Application.EnableEvents = False
                Newvalue = Target.Value
                Application.Undo
                Oldvalue = Target.Value
                If Oldvalue = """" Then
                    Target.Value = Newvalue
                Else
                    Target.Value = Oldvalue & "", "" & Newvalue
                End If
            End If
        End If




                                       'PhoneNumber validation
                                                 If Not Intersect(Target, rng) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rng)
                                                            If Not regex.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid phone number in cell "" & cell.Address & "". Please enter a valid phone number.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If
                                      'Aadhaar validation
                                                 If Not Intersect(Target, rngAadhaar) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rngAadhaar)
                                                            If Not regexAadhaar.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid Aadhaar number in cell "" & cell.Address & "". Please enter a valid Aadhaar number.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If

                            'Email validation
                                                 If Not Intersect(Target, rngEmail) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rngEmail)
                                                            If Not regexEmail.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid Email in cell "" & cell.Address & "". Please enter a valid Email.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If

                                        'MinMax validation
                                        If Not Intersect(Target, rngMinMax) Is Nothing Then
                                                        For Each cell In Intersect(Target, rngMinMax)
                                                                    ' Check if the value is outside the valid range
                                                                    If cell.Value < minimumValue Or cell.Value > maximumValue Then
                                                                        ' Invalid value, display an error message
                                                                        MsgBox ""Invalid value in cell "" & cell.Address & "". Value should be between "" & minimumValue & "" and "" & maximumValue & "".""
                                                                        cell.Value = """" ' Clear the invalid value from the cell
                                                                    End If
                                                                Next cell
                                         End If
                                        'Numeric validation
                                          If Not Intersect(Target, rngNumeric) Is Nothing Then
                                                Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                For Each cell In Intersect(Target, rngNumeric)
                                                    ' Check if the value is not numeric
                                                    If Not IsNumeric(cell.Value) Then
                                                        ' Invalid value, display an error message
                                                        MsgBox ""Invalid value in cell "" & cell.Address & "". Please enter a numeric value.""
                                                        cell.Value = """" ' Clear the invalid value from the cell
                                                    End If
                                                Next cell
        
                                                Application.EnableEvents = True ' Enable events
                                            End If
                                        'Date validation
                                         If Not Intersect(Target, rngDate) Is Nothing Then
                                                Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                ' Define the regular expression pattern for ""dd-mm-yyyy"" format
                                                regexPattern = ""^(0?[1-9]|[1-2]\d|3[01])-(0?[1-9]|1[0-2])-(\d{4})$""
        
                                                For Each cell In Intersect(Target, rngDate)

                                                          ' cell.NumberFormat = ""dd-mm-yyyy""
                                                    ' Check if the value matches the ""dd-mm-yyyy"" format using the regular expression
                                                    If Not RegExpMatch(cell.Value, regexPattern) Then
                                                        ' Invalid date, display an error message
                                                        MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date in the 'dd-mm-yyyy' format.""
                                                        cell.Value = """" ' Clear the invalid value from the cell
                                                    End If
                                                Next cell
        
                                               Application.EnableEvents = True ' Enable events
                                            End If
                                        'GPS Validation
                                        If Not Intersect(Target, rngGps) Is Nothing Then
                                                ' Validate the GPS coordinates using the C# regular expression pattern
                                            For Each cell In Intersect(Target, rngGps)
                                               If Not regexGPS.Test(cell.Value) Then
                                                    MsgBox ""Invalid coordinates""
                                                    Target.Value = """" ' Clear the invalid value
                                                End If
                                                 Next cell
                                            End If
                                        'Should be greater than Current date validation
                                         If Not Intersect(Target, rngDategreater) Is Nothing Then
                                                Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                ' Get the current date
                                                currentDate = Date
        
                                                For Each cell In Intersect(Target, rngDategreater)
                                                    ' Check if the value is a valid date and greater than the current date
                                                    If Not IsDate(cell.Value) Or cell.Value <= currentDate Then
                                                        ' Invalid date, display an error message
                                                        MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date greater than the current date.""
                                                        cell.Value = """" ' Clear the invalid value from the cell
                                                    End If
                                                Next cell
        
                                                Application.EnableEvents = True ' Enable events
                                            End If
                                        'Should be less than Current date
                                         If Not Intersect(Target, rngDateless) Is Nothing Then
                                                Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                ' Get the current date
                                                currentDate = Date
        
                                                For Each cell In Intersect(Target, rngDateless)
                                                    ' Check if the value is a valid date and less than Current date
                                                    If Not IsDate(cell.Value) Or cell.Value >= currentDate Then
                                                        ' Invalid date, display an error message
                                                        MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date less than the Current date.""
                                                        cell.Value = """" ' Clear the invalid value from the cell
                                                    End If
                                                Next cell
        
                                                Application.EnableEvents = True ' Enable events
                                            End If

                                                ' If Target.Row = 2 Then Exit Sub
                                             Dim targetSheetName As String
                                                targetSheetName = Target.Worksheet.Name
                                                 Dim ws As Worksheet
                                                Set ws = Worksheets(targetSheetName)
                                                 Dim ColName As Range
                                                 Set ColName = ws.Cells.Find(""MainRowNo"")
    
                                                Application.EnableEvents = False
                                                If Not IsNumeric(ws.Range(ColName.Address)(Target.Row)) Or ws.Range(ColName.Address)(Target.Row) = """" Then
                                                    ws.Range(ColName.Address)(Target.Row) = ws.Range(ColName.Address)(Target.Row - 1)
                                                End If
                                                Application.EnableEvents = True
                                               Exitsub:
    
                                                Application.EnableEvents = True
                                         

                                            End Sub
                                        Private Function RegExpMatch(ByVal text As String, ByVal pattern As String) As Boolean
                                                    Dim regex As Object
                                                    Set regex = CreateObject(""VBScript.RegExp"")
    
                                                    regex.pattern = pattern
                                                    RegExpMatch = regex.Test(text)
                                                End Function";
                            #endregion RepeatVBA  

                            codeRepeat = codeRepeat.Replace("PhoneValidationRangeInStr", RPhoneValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("AadhaarValidationRangeInStr", RAadhaarValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("EmailValidationRangeInStr", REmailValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("MinMaxValidationRangeInStr", RMinMaxValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("NumericRangeInStr", RNumericRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("DateRangeInStr", RDateRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("greaterValidationRangeInStr", RgreaterValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("lessValidationRangeInStr", RlessValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("GPSValidationRangeInStr", RGPSValidationRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("dropdownRangeInStr", RdropdownRangeInStr.ToString());
                            codeRepeat = codeRepeat.Replace("minValue", minValue.ToString());
                            codeRepeat = codeRepeat.Replace("maxValue", maxValue.ToString());
                            worksheetsub.CodeModule.Code = codeRepeat;
                        }

                        int SkipNewCol = newCol + 1;
                        for (int i = 0; i < lstSurveySectionLogic.Count; i++)
                        {

                            if (lstSurveySectionLogic[i].LogicType == "Skip Logic")
                            {

                                ExcelRange dropdownRangeSkip = worksheet.Cells[worksheet.Cells[2, SkipNewCol] + ":" + worksheet.Cells[100, SkipNewCol]];
                                ConditionQuestionRange = dropdownRangeSkip.ToString();
                                newCol = newCol + 1;
                                worksheet.Cells[1, newCol + 1].Value = lstSurveySectionLogic[i].QuestionName;
                                //var SurveySkipLogic = lstSurveyQuestion[col].lstSurveySectionLogic;
                                logicQuestionString += lstSurveySectionLogic[i].QuestionName + "''";
                                ConditionQuestionAnswer = lstSurveySectionLogic[i].Answer;
                                SkipLogicConditionOperator = lstSurveySectionLogic[i].Condition; // =,>, >= , <= , !=
                                if (lstSurveySectionLogic[i].Condition == "!=")
                                {
                                    SkipLogicConditionOperator = "<>";
                                }
                                // newCol = newCol - 1;
                                if (lstSurveySectionLogic[i].QuestionType == "Single Line Text")
                                {
                                  

                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Email")
                                        {
                                            ExcelRange ValidationRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                                            EmailStr += ValidationRange + ",";
                                            EmailValidationRangeInStr = EmailStr.ToString();

                                        }
                                    }

                                }
                                if (lstSurveySectionLogic[i].QuestionType == "Numeric")
                                {
                                    ExcelRange ValidationRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                                    NumericStr += ValidationRange + ",";

                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Phone Number")
                                        {

                                            PhoneValidationRangeInStr += ValidationRange + ",".ToString();

                                        }
                                        else if (lstQuestionCondition[0].ValidationCondition == "Aadhaar Number")
                                        {
                                            AadhaarValidationRangeInStr += ValidationRange + ",".ToString();
                                        }


                                        else if (lstQuestionCondition[0].ValidationCondition == "Min & Max Range")
                                        {
                                            minValue = lstQuestionCondition[0].MinValue;
                                            maxValue = lstQuestionCondition[0].MaxValue;
                                            MinMaxValidationRangeInStr += ValidationRange + ",".ToString();
                                        }
                                    }
                                    else
                                    {
                                        NumericRangeInStr = NumericStr.ToString();

                                    }

                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Date Picker")
                                {
                                    ExcelRange DateRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[colunmrange, newCol + 1]];
                                    DateStr += DateRange + ",";
                                    DateRangeInStr = DateStr.ToString();
                                    if (lstQuestionCondition.Count > 0)
                                    {

                                        if (lstQuestionCondition[0].ValidationCondition == "Should be greater than Current date")
                                        {

                                            greaterValidationRangeInStr += DateRange + ",".ToString();

                                        }
                                        else if (lstQuestionCondition[0].ValidationCondition == "Should be less than Current date")
                                        {
                                            lessValidationRangeInStr += DateRange + ",".ToString();
                                        }
                                    }
                                }
                                if (lstSurveySectionLogic[i].QuestionType == "GPS Capture")
                                {
                                    ExcelRange GpsRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];
                                    GPSStr += GpsRange + ",";
                                    GPSValidationRangeInStr = GPSStr .ToString();
                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Dropdown")
                                {
                                    List<string> optionlist = new List<string>();

                                    for (var j = 0; j < lstSurveySectionLogic[i].lstQuestionOptions.Count; j++)
                                    {
                                        optionlist.Add(lstSurveySectionLogic[i].lstQuestionOptions[j].OptionValue);

                                    }

                                    ExcelRange dropdownRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];

                                    // Create a data validation object
                                    var dataValidation = worksheet.DataValidations.AddListValidation(dropdownRange.Address);
                                    // var dataValidation = worksheet.DataValidations.AddListValidation("D2:D100");
                                    dataValidation.ShowErrorMessage = true;
                                    dataValidation.ErrorTitle = "Invalid Value";
                                    dataValidation.Error = "Please select a value from the dropdown list.";
                                    dataValidation.Formula.Values.AddRange(optionlist);

                                }

                                if (lstSurveySectionLogic[i].QuestionType == "Single Choice" || lstSurveySectionLogic[i].QuestionType == "Likert Scale")
                                {
                                    List<string> optionlist = new List<string>();

                                    for (var j = 0; j < lstSurveySectionLogic[i].lstQuestionOptions.Count; j++)
                                    {
                                        optionlist.Add(lstSurveySectionLogic[i].lstQuestionOptions[j].OptionValue);

                                    }
                                    // set the cell range where you want to create the dropdown
                                    ExcelRange dropdownRange = worksheet.Cells[worksheet.Cells[2, newCol + 1] + ":" + worksheet.Cells[100, newCol + 1]];

                                    // create a data validation object
                                    var dataValidation = worksheet.DataValidations.AddListValidation(dropdownRange.Address);

                                    dataValidation.ShowErrorMessage = true;
                                    dataValidation.ErrorTitle = "invalid value";
                                    dataValidation.Error = "please select a value from the dropdown.";
                                    dataValidation.Formula.Values.AddRange(optionlist);

                                }
                            }

                        }

                    }

                    newCol = newCol + 1;

                }
                //ExcelRangeBase rngNumeric = worksheet.Cells["A1"];
                //foreach (ExcelRange range in rangeList)
                //{
                //    // Copy the values from each ExcelRange to the destination range
                //    rngNumeric.Value = range.Value;
                //    // Move the destination range to the next row
                //    rngNumeric = rngNumeric.Offset(1, 0);
                //}


   

                logicQuestionString = logicQuestionString.TrimEnd(',');
                EmailValidationRangeInStr = EmailValidationRangeInStr.TrimEnd(',');
                PhoneValidationRangeInStr = PhoneValidationRangeInStr.TrimEnd(',');
                AadhaarValidationRangeInStr = AadhaarValidationRangeInStr.TrimEnd(',');
                MinMaxValidationRangeInStr = MinMaxValidationRangeInStr.TrimEnd(',');
                NumericRangeInStr = NumericRangeInStr.TrimEnd(',');
                DateRangeInStr = DateRangeInStr.TrimEnd(',');
                greaterValidationRangeInStr = greaterValidationRangeInStr.TrimEnd(',');
                lessValidationRangeInStr = lessValidationRangeInStr.TrimEnd(',');
                GPSValidationRangeInStr = GPSValidationRangeInStr.TrimEnd(',');

                #region VBA
                string code = @"Sub Worksheet_Change(ByVal Target As Range)
         Dim SkipRange As Range
        Dim Oldvalue As String
        Dim Newvalue As String
        Dim rngProtected As Range
        Dim cell As Range
        Dim dropdownRangeAdd As Range
        Dim findName As Range
        Dim Result() As String
         Dim rng As Range
         Dim rngAadhaar As Range
         Dim rngEmail As Range
         Dim regex As Object
            Dim regexEmail As Object
            Dim regexGPS As Object
         Dim regexAadhaar As Object
         Dim minimumValue As Double
         Dim maximumValue As Double
         Dim rngMinMax As Range
         Dim rngNumeric As Range
         Dim rngDate As Range
         Dim regexPattern As String
        Dim currentDate As Date
        Dim rngDategreater As Range
        Dim rngDateless As Range
        Dim rngGps As Range

     Set rng =Range (""PhoneValidationRangeInStr"")'
     Set regex = CreateObject(""VBScript.RegExp"")
     regex.Pattern = ""^\d{10}$"" 
    
     Set rngAadhaar =Range (""AadhaarValidationRangeInStr"")' 
     Set regexAadhaar = CreateObject(""VBScript.RegExp"")
     regexAadhaar.Pattern = ""^\d{12}$""

     Set rngEmail =Range (""EmailValidationRangeInStr"")' 
     Set regexEmail = CreateObject(""VBScript.RegExp"")
     regexEmail.Pattern = ""^[\w\.-]+@[\w\.-]+\.\w+$""

     Set rng =Range (""PhoneValidationRangeInStr"")'
     Set regex = CreateObject(""VBScript.RegExp"")
     regex.Pattern = ""^\d{10}$""

     Set rngGps =Range (""GPSValidationRangeInStr"")'
     Set regexGPS = CreateObject(""VBScript.RegExp"")
     regexGPS.Pattern = ""^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$""

     Set rngMinMax =Range (""MinMaxValidationRangeInStr"")' 
    minimumValue = minValue ' Specify the minimum value
    maximumValue = maxValue ' Specify the maximum value

set rngNumeric=Range (""NumericRangeInStr"")

set rngDate=Range (""DateRangeInStr"")
set rngDategreater=Range (""greaterValidationRangeInStr"")
set rngDateless=Range (""lessValidationRangeInStr"")



    ' Set the range of cells to be protected
    Set rngProtected = Range(""B1:B10"")
    Set dropdownRangeAdd = Range (""dropdownRangeInStr"")' getting dropdown range as address
        
        Dim SkipLogicQuestionRange as string
        Dim ConditionQstRange as Range     
        Dim SkipLogicAnswer as String 
        Set ConditionQstRange = Range(""conditionQstRangeInStr"")
        SkipLogicAnswer = ""skipLogicAnswerInStr""
        SkipLogicQuestionRange = ""skipLogicQuestionsInStr""
    

        On Error GoTo Exitsub
        If Not Intersect(Target, dropdownRangeAdd) Is Nothing Then
            If Target.SpecialCells(xlCellTypeAllValidation) Is Nothing Then
            GoTo Exitsub
            Else: If Target.Value = """" Then GoTo Exitsub Else
                Application.EnableEvents = False
                Newvalue = Target.Value
                Application.Undo
                Oldvalue = Target.Value
                If Oldvalue = """" Then
                    Target.Value = Newvalue
                Else
                    Target.Value = Oldvalue & "", "" & Newvalue
                End If
            End If
        End If
        
'PhoneNumber validation
                                                 If Not Intersect(Target, rng) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rng)
                                                            If Not regex.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid phone number in cell "" & cell.Address & "". Please enter a valid phone number.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If
'Aadhaar validation
                                                 If Not Intersect(Target, rngAadhaar) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rngAadhaar)
                                                            If Not regexAadhaar.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid Aadhaar number in cell "" & cell.Address & "". Please enter a valid Aadhaar number.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If

'Email validation
                                                 If Not Intersect(Target, rngEmail) Is Nothing Then
                                                        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
                                                        For Each cell In Intersect(Target, rngEmail)
                                                            If Not regexEmail.test(cell.Value) Then
                                                                ' Invalid phone number, display an error message
                                                                MsgBox ""Invalid Email in cell "" & cell.Address & "". Please enter a valid Email.""
                                                                cell.Value = """" ' Clear the invalid value from the cell
                                                            End If
                                                        Next cell
        
                                                        Application.EnableEvents = True ' Enable events
                                                    End If

'MinMax validation
If Not Intersect(Target, rngMinMax) Is Nothing Then
                For Each cell In Intersect(Target, rngMinMax)
                            ' Check if the value is outside the valid range
                            If cell.Value < minimumValue Or cell.Value > maximumValue Then
                                ' Invalid value, display an error message
                                MsgBox ""Invalid value in cell "" & cell.Address & "". Value should be between "" & minimumValue & "" and "" & maximumValue & "".""
                                cell.Value = """" ' Clear the invalid value from the cell
                            End If
                        Next cell
 End If
'Numeric validation
  If Not Intersect(Target, rngNumeric) Is Nothing Then
        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
        For Each cell In Intersect(Target, rngNumeric)
            ' Check if the value is not numeric
            If Not IsNumeric(cell.Value) Then
                ' Invalid value, display an error message
                MsgBox ""Invalid value in cell "" & cell.Address & "". Please enter a numeric value.""
                cell.Value = """" ' Clear the invalid value from the cell
            End If
        Next cell
        
        Application.EnableEvents = True ' Enable events
    End If
'Date validation

           ' Define the regular expression pattern for ""dd-mm-yyyy"" format
        regexPattern = ""^(0?[1-9]|[1-2]\d|3[01])-(0?[1-9]|1[0-2])-(\d{4})$""
    
 If Not Intersect(Target, rngDate) Is Nothing Then
        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
     

        For Each cell In Intersect(Target, rngDate)
         ' If Not IsEmpty(cell.Value) Then
                'cell.Value = Format(cell.Value, customDateFormat)
                'cell.NumberFormat = ""@"" ' Set the cell format to text explicitly
            'End If
              ' cell.NumberFormat = ""dd-mm-yyyy""
            ' Check if the value matches the ""dd-mm-yyyy"" format using the regular expression
            If Not RegExpMatch(cell.text, regexPattern) Then
                ' Invalid date, display an error message
                MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date in the 'dd-mm-yyyy' format.""
                cell.Value = """" ' Clear the invalid value from the cell
            End If
        Next cell
        
       Application.EnableEvents = True ' Enable events
    End If
'GPS Validation
If Not Intersect(Target, rngGps) Is Nothing Then
        ' Validate the GPS coordinates using the C# regular expression pattern
    For Each cell In Intersect(Target, rngGps)
       If Not regexGPS.Test(cell.Value) Then
            MsgBox ""Invalid coordinates""
            Target.Value = """" ' Clear the invalid value
        End If
         Next cell
    End If
'Should be greater than Current date validation
 If Not Intersect(Target, rngDategreater) Is Nothing Then
        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
        ' Get the current date
        currentDate = Date
        
        For Each cell In Intersect(Target, rngDategreater)
            ' Check if the value is a valid date and greater than the current date
            If Not IsDate(cell.Value) Or cell.Value <= currentDate Then
                ' Invalid date, display an error message
                MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date greater than the current date.""
                cell.Value = """" ' Clear the invalid value from the cell
            End If
        Next cell
        
        Application.EnableEvents = True ' Enable events
    End If
'Should be less than Current date
 If Not Intersect(Target, rngDateless) Is Nothing Then
        Application.EnableEvents = False ' Disable events to avoid triggering another change event
        
        ' Get the current date
        currentDate = Date
        
        For Each cell In Intersect(Target, rngDateless)
            ' Check if the value is a valid date and less than Current date
            If Not IsDate(cell.Value) Or cell.Value >= currentDate Then
                ' Invalid date, display an error message
                MsgBox ""Invalid date in cell "" & cell.Address & "". Please enter a date less than the Current date.""
                cell.Value = """" ' Clear the invalid value from the cell
            End If
        Next cell
        
        Application.EnableEvents = True ' Enable events
    End If
'Skip Logic
        If Target.Row = 1 Then Exit Sub
    Dim ColumnName As String
    Dim temp As Range
    Application.EnableEvents = False
   If Main.Range(ConditionQstRange.Address)(Target.Row-1) skipLogicConditionOperatorStr ""skipLogicAnswerInStr"" Then

        Result = Split(SkipLogicQuestionRange, ""''"")
    For Each Item In Result
            Set findName = ActiveSheet.Cells.Find(What:=Item, LookIn:=xlValues, LookAt:=xlWhole)
            ColumnName = Main.Cells(findName.Row, findName.Column)
            Set SkipRange = Application.Intersect(findName.EntireColumn, Main.Rows(Target.Row))
            SkipRange.Locked = False
            SkipRange.Font.Color = RGB(0, 0, 0)
        Next
        Set SkipRange = Application.Intersect(Main.Range(findName.Address), Main.Rows(Target.Row))
     ElseIf Main.Range(ConditionQstRange.Address)(Target.Row - 1) = """" Then
        Result = Split(SkipLogicQuestionRange, ""''"")
    
        For Each Item In Result
            Set findName = ActiveSheet.Cells.Find(What:=Item, LookIn:=xlValues, LookAt:=xlWhole)
            ColumnName = Main.Cells(findName.Row, findName.Column)
            Set SkipRange = Application.Intersect(findName.EntireColumn, Main.Rows(Target.Row))
            SkipRange.Locked = True
            SkipRange.ClearContents
            SkipRange.Font.Color = RGB(220, 220, 220)
        Next
        Else
         
        Result = Split(SkipLogicQuestionRange, ""''"")
    
        For Each Item In Result
            Set findName = ActiveSheet.Cells.Find(What:=Item, LookIn:=xlValues, LookAt:=xlWhole)
            ColumnName = Main.Cells(findName.Row, findName.Column)
            Set SkipRange = Application.Intersect(findName.EntireColumn, Main.Rows(Target.Row))
            SkipRange.Locked = True
            SkipRange.ClearContents
            SkipRange.Font.Color = RGB(220, 220, 220)
        Next
        
    End If

               Exitsub:

    
                Application.EnableEvents = True
              End Sub

             Sub AddRepeatRow(MainRowNo As Long, sheetName As String)
              Dim ws As Worksheet
              Set ws = Worksheets(sheetName)
                Dim i As Integer
                i = 1
                Dim ColName As Range
                Set ColName = ws.Cells.Find(""MainRowNo"")
                While ws.Range(ColName.Address)(i) <> """"
                    i = i + 1
                Wend
                ws.Range(ColName.Address)(i) = MainRowNo
                'Repeat.Cells(i, 2).Select
            End Sub
        Private Function RegExpMatch(ByVal text As String, ByVal pattern As String) As Boolean
            Dim regex As Object
            Set regex = CreateObject(""VBScript.RegExp"")
    
            regex.pattern = pattern
            RegExpMatch = regex.Test(text)
        End Function

        Sub Worksheet_FollowHyperlink(ByVal Target As Hyperlink)
                      Dim targetSheetName As String
                        If Target.Type = msoHyperlinkRange Then
                            targetSheetName = Target.SubAddress
                            ' Extract the sheet name from the SubAddress
                            targetSheetName = Split(targetSheetName, ""!"")(1)
                            ' Continue with your code using the targetSheetName
                        End If
                        AddRepeatRow Target.Range.Row, targetSheetName
          
          

            End Sub";
                #endregion VBA 
                code = code.Replace("dropdownRangeInStr", dropdownRangeInStr.ToString());
                code = code.Replace("skipLogicQuestionsInStr", logicQuestionString.ToString());
                code = code.Replace("skipLogicAnswerInStr", ConditionQuestionAnswer);
                code = code.Replace("conditionQstRangeInStr", ConditionQuestionRange.ToString());
                code = code.Replace("skipLogicConditionOperatorStr", SkipLogicConditionOperator);
                code = code.Replace("PhoneValidationRangeInStr", PhoneValidationRangeInStr.ToString());
                code = code.Replace("AadhaarValidationRangeInStr", AadhaarValidationRangeInStr.ToString());
                code = code.Replace("EmailValidationRangeInStr", EmailValidationRangeInStr.ToString());
                code = code.Replace("MinMaxValidationRangeInStr", MinMaxValidationRangeInStr.ToString());
                code = code.Replace("NumericRangeInStr", NumericRangeInStr.ToString());
                code = code.Replace("DateRangeInStr", DateRangeInStr.ToString());
                code = code.Replace("greaterValidationRangeInStr", greaterValidationRangeInStr.ToString());
                code = code.Replace("lessValidationRangeInStr", lessValidationRangeInStr.ToString());
                code = code.Replace("GPSValidationRangeInStr", GPSValidationRangeInStr.ToString());
                code = code.Replace("minValue", minValue.ToString());
                code = code.Replace("maxValue", maxValue.ToString());


                //ConditionQuestionRange
                // excelPackage.Workbook.CreateVBAProject();
                worksheet.CodeModule.Code = code;

                MandatoryStr = MandatoryStr.TrimEnd(',');

                string thisWorkbookVbaCode = @"Sub Workbook_BeforeSave(ByVal SaveAsUI As Boolean, Cancel As Boolean)
    Dim ws As Worksheet
    Dim lastRow As Long
    Dim mandatoryColumns As Range
    Dim cell As Range
    Dim isMissingFields As Boolean
    Dim column As Range


    ' Set the worksheet you want to check
    Set ws = ThisWorkbook.Worksheets(""Main"")

    ' Define a range of mandatory columns
    Set mandatoryColumns = ws.Range(""MandatoryStr"")

    ' Get the last used row in the worksheet
    lastRow = ws.Cells(ws.Rows.Count, 1).End(xlUp).Row

    ' Reset cell formatting for all cells in the mandatory columns
    For Each column In mandatoryColumns.Columns
        column.Interior.Pattern = xlNone
    Next column

    ' Check if any mandatory field is empty and highlight the corresponding cells
    For Row = 2 To lastRow
        For Each column In mandatoryColumns.Columns
            For Each cell In column.Cells
                ' Check if the cell is empty
                If IsEmpty(ws.Cells(Row, cell.Column).Value) Then
                    ' The mandatory field is empty, highlight the cell
                    ws.Cells(Row, cell.Column).Interior.Color = RGB(192, 192, 192) ' Set the highlight color to light grey
                    isMissingFields = True
                End If
            Next cell
        Next column
    Next Row

    If isMissingFields Then
        MsgBox ""Please fill in all mandatory fields before saving.""
        Cancel = True
    End If

  

End Sub";

                thisWorkbookVbaCode = thisWorkbookVbaCode.Replace("MandatoryStr", MandatoryStr.ToString());
                excelPackage.Workbook.CodeModule.Code = thisWorkbookVbaCode;


                excelPackage.Save();


                int startColumnIndex = 1; // Column A
                int endColumnIndex = lstSurveyQuestion.Count + lstSurveySectionLogic.Count;   // Column D

                // Autofit the columns
                worksheet.Cells[worksheet.Dimension.Start.Row, startColumnIndex, worksheet.Dimension.End.Row, endColumnIndex].AutoFitColumns();

                // excelPackage.SaveAs(new FileInfo(@"C:\\Users\\HP\\Desktop\\demo.xlsm"));
                string currentDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // Create the file name with the current date and time
                // string fileName = $"sample_surveyQuestionnaire_{currentDateTime}.xlsm";

                // Return the file
                //return File(fileContents, "application/vnd.ms-excel.sheet.macroEnabled.12", fileName);

                //string path = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\sample_surveyQuestionnaire_" + currentDateTime + ".xlsm";
                // excelPackage.SaveAs(new FileInfo(path));
                //byte[] fileContents = excelPackage.GetAsByteArray();
                //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StudentData.xlsx");
                //// return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StudentData.xlsx");
                // stream.Position = 0;
                // string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsm";
                // File(fileContents, "application/vnd.ms-excel.sheet.macroEnabled.12", "StudentData.xlsm");
                //return File(stream, "application/octet-stream", excelName);  
                // return File(stream, "application/vnd.ms-excel.sheet.macroEnabled.12", excelName);
                byte[] fileContents = excelPackage.GetAsByteArray();
                string fileName = $"sample_surveyQuestionnaire_{currentDateTime}.xlsm";

                // Set the response headers
                Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";

                // Return the Excel file data as the response content
                return File(fileContents, "application/vnd.ms-excel.sheet.macroEnabled.12", fileName);


            }
            // }
            //  catch (Exception ex)
            //{
            // }


        }



        public IActionResult demo()
        {

            ExcelPackage package = new ExcelPackage();

            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("11");
                var validationCell = worksheet.DataValidations.AddListValidation("A1");
                validationCell.Formula.Values.Add("a");
                validationCell.Formula.Values.Add("b");
                validationCell.Formula.Values.Add("c");

                string code = @"Sub Worksheet_Change(ByVal Target As Range)

        Dim Oldvalue As String
        Dim Newvalue As String

        On Error GoTo Exitsub
        If Target.Address = ""$A$1"" Then
            If Target.SpecialCells(xlCellTypeAllValidation) Is Nothing Then
            GoTo Exitsub
            Else: If Target.Value = """" Then GoTo Exitsub Else
                Application.EnableEvents = False
                Newvalue = Target.Value
                Application.Undo
                Oldvalue = Target.Value
                If Oldvalue = """" Then
                    Target.Value = Newvalue
                Else
                    Target.Value = Oldvalue & "", "" & Newvalue
                End If
            End If
        End If
        Application.EnableEvents = True
        Exitsub:
        Application.EnableEvents = True
    End Sub";
                package.Workbook.CreateVBAProject();
                worksheet.CodeModule.Code = code;
                package.Save();
                //string p_strPath = @"C:\\Users\\HP\\Desktop\\demo1.xlsm";
                // Save the Excel package
                //package.SaveAs(new System.IO.FileInfo(p_strPath));
                byte[] fileContents = package.GetAsByteArray();
                // return File(fileContents, "application/vnd.ms-excel.sheet.macroEnabled.12", "MacroEnabledWorkbook.xlsm");
                return File(fileContents, "application/vnd.ms-excel.sheet.macroEnabled.12", "StudentData.xlsm");

                // string p_strPath = @"demo.xlsm";
                // Save the Excel package
                //excelPackage.SaveAs(new System.IO.FileInfo(p_strPath));

                // MemoryStream stream = new MemoryStream();
                // Save the workbook to the file stream
                //excelPackage.SaveAs(stream);
                // After you save to the stream, you need to reset the stream back to the origin at position 0
                //stream.Seek(0, SeekOrigin.Begin);

                //// Response to the user
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.Headers["content-disposition"] = "attachment; filename=HelloWord.xlsx";
                //Response.Body.WriteAsync(stream.ToArray());


                // excelPackage.SaveAs(new FileInfo("demo.xlsm"));
                //  byte[] fileBytes = excelPackage.GetAsByteArray();

                // MemoryStream stream = new MemoryStream();
                // excelPackage.SaveAs(stream);
                //stream.Position = 0;

                // Return the MemoryStream contents as a downloadable file
                //return new FileContentResult(fileBytes, "application/vnd.ms-excel.sheet.macroEnabled.12")
                //{
                //    FileDownloadName = "MacroEnabledWorkbook.xlsm"
                //};
                //string fileName = "sample_excel_file.xlsx";

                //// Download the file
                // return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                // Reset the stream position to the beginning
                // stream.Position = 0;

                // Create a byte array from the stream
                //byte[] fileBytes = stream.ToArray();

                // Prompt the user to download the file
                // DownloadFile(fileBytes, "SampleFile.xlsx");

            }
            // return true;
        }

        #region Excel Read 

        [HttpPost]
        public ActionResult Import(IFormFile file, ProjectSurvey objProjectSurvey,int LoggedUserId)
        {
            var data = new System.Data.DataTable();
            int questionId = 0, sectionID = 0, SurveyQuestionID = 0;
            string fieldVar;
            SurveyResponseImportDAL objSurvey = new SurveyResponseImportDAL();
            MobileWebServiceDAL objWebService = new MobileWebServiceDAL();
            List<SurveyQuestion> lstSurveyQuestion = objSurvey.GetQuestionDetails(objProjectSurvey.SurveyID);
            List<ExcelSurvayQuestion> lstExcelSurveyQuestion = new List<ExcelSurvayQuestion>();
            List<SurveySectionLogic> lstSurveySectionLogic = new List<SurveySectionLogic>();
            List<MyTask> lstTaskData = new List<MyTask>();
            MyTask ObjSurveyResponse = new MyTask();
            bool Beneficiarylinked = false;
            var TaskSubmittedDate = DateTime.Now;
            var ResponseId = 0;
            var Index = 1;
            var SurveyQuestionName = "";
            var LogicResponseID = 0;
            var RepeatTaskSubmittedDate = DateTime.Now;
            List<int> repeatQuestion = new List<int>();
            // SurvayResponse ObjSurveyResponse = new SurvayResponse();
            try
            {
                // ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial for commercial use

                if (file != null && file.Length > 0)
                {
                    using (var package = new ExcelPackage(file.OpenReadStream()))
                    {
                        if (package.Workbook.Worksheets.Count > 0)
                        {
                            for (int sheet = 0; sheet < package.Workbook.Worksheets.Count; sheet++)
                            {

                                var worksheet = package.Workbook.Worksheets[sheet]; // Assuming the data is on the first worksheet


                                int rowCount = worksheet.Dimension.Rows;
                                int columnCount = worksheet.Dimension.Columns;

                                if (objProjectSurvey.BeneficiaryTypeID > 0)
                                {
                                    Beneficiarylinked = true;

                                    List<SurveySectionLogic> logicSectionQustns = new List<SurveySectionLogic>();
                                    //if (worksheet.Name == "Main")
                                    //{
                                    //    for (int i = 0; i < lstSurveyQuestion.Count; i++)
                                    //    {
                                    //        lstSurveySectionLogic = lstSurveyQuestion[i].lstSurveySectionLogic;
                                    //        if (lstSurveySectionLogic.Count() > 0)
                                    //        {
                                    //            if (lstSurveySectionLogic[0].LogicType == "Repeat Logic")
                                    //            {
                                    //                SurveyQuestionID = lstSurveyQuestion[i].QuestionID;
                                    //                SurveyQuestionName = lstSurveyQuestion.Where(p => p.QuestionID == SurveyQuestionID).Select(p => p.QuestionName).FirstOrDefault();
                                    //            }
                                    //        }
                                    //    }
                                    //    int count = ExcludeRowsWithValue(file, worksheet, SurveyQuestionName);
                                    //    rowCount = count + 1;

                                    //}
                                    // Read the Excel data into a DataTable


                                    if (worksheet.Name == "Main")
                                    {
                                        for (int col = 4; col <= columnCount; col++)
                                        {
                                            
                                            if (DateTime.TryParse(Convert.ToString(worksheet.Cells[2, 3].Value), out DateTime dateTest))
                                            {
                                                // It's a valid date, retrieve the displayed text
                                                TaskSubmittedDate = Convert.ToDateTime(worksheet.Cells[2, 3].Value);
                                           }
                                           

                                            questionId = lstSurveyQuestion.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).Select(p => p.QuestionID).FirstOrDefault();
                                            sectionID = lstSurveyQuestion.Where(p => p.QuestionID == questionId).Select(p => p.SectionID).FirstOrDefault();
                                            fieldVar = lstSurveyQuestion.Where(p => p.QuestionID == questionId).Select(p => p.FieldVar).FirstOrDefault();
                                            var QuestionDtls = lstSurveyQuestion.Where(p => p.QuestionID == questionId).ToArray();
                                            if (QuestionDtls.Count() > 0)
                                            {
                                                if (QuestionDtls[0].IsConditionQuestion == true)
                                                {
                                                    if (QuestionDtls[0].lstSurveySectionLogic[0].LogicType != "Repeat Logic")
                                                    {
                                                        logicSectionQustns = QuestionDtls[0].lstSurveySectionLogic.ToList();
                                                    }
                                                }
                                            }
                                            if (logicSectionQustns.Count() > 0)
                                            {
                                                var isExist = lstExcelSurveyQuestion.Where(p => p.QuestionID == questionId).ToArray();
                                                if (isExist.Count() > 0)
                                                {
                                                    var IsExistInLogic = logicSectionQustns.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).ToArray();
                                                    if (IsExistInLogic.Count() > 0)
                                                    {
                                                        questionId = IsExistInLogic[0].QuestionID;
                                                        sectionID = IsExistInLogic[0].SectionID;
                                                    }
                                                }
                                                else
                                                {
                                                    if (questionId <= 0) //lstSurveyQuestion does not have logic question details 
                                                    {
                                                        var IsExistInLogic = logicSectionQustns.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).ToArray();
                                                        if (IsExistInLogic.Count() > 0)
                                                        {
                                                            questionId = IsExistInLogic[0].QuestionID;
                                                            sectionID = IsExistInLogic[0].SectionID;
                                                        }
                                                    }
                                                }
                                            }
                                            for (int row = 2; row <= rowCount; row++)
                                            {

                                                ExcelSurvayQuestion ExcelObj = new ExcelSurvayQuestion();
                                                if (worksheet.Cells[row, 3].Value != null)
                                                {
                                                   
                                                 if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, 3].Text), out DateTime dateValue))
                                                    {

                                                        ExcelObj.TaskDate = Convert.ToDateTime(worksheet.Cells[row, 3].Text);
                                                        ExcelObj.TaskSubmittedDate = Convert.ToDateTime(worksheet.Cells[row, 3].Text);
                                                    }

                                                }

                                                ExcelObj.BeneficiaryInd = 0;
                                                ExcelObj.ResponseID = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                                                ExcelObj.SurveyID = objProjectSurvey.SurveyID;
                                                ExcelObj.SectionID = sectionID;
                                                ExcelObj.QuestionID = questionId;
                                                //ExcelObj.FieldVar = fieldVar;
                                                ExcelObj.RQstInd = Index;
                                                // ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                               // object cellValue = worksheet.Cells[row, col].Value;
                                                if (worksheet.Cells[row, col].Value != null)
                                                {
                                                    // Check if the cell value represents a date
                                                    if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, col].Text), out DateTime dateValue))
                                                    {
                                                        // It's a valid date, retrieve the displayed text
                                                        ExcelObj.Answer = worksheet.Cells[row, col].Text;
                                                    }
                                                    else
                                                    {
                                                        // It's not a date, use the underlying value as a string
                                                        ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                                    }
                                                }
                                                else
                                                {
                                                    ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                                }
                                                lstExcelSurveyQuestion.Add(ExcelObj);
                                                Index++;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        var Readsheet = false;

                                        for (int i = 0; i < lstSurveyQuestion.Count; i++)
                                        {
                                            lstSurveySectionLogic = lstSurveyQuestion[i].lstSurveySectionLogic;
                                            if (lstSurveySectionLogic.Count() > 0)
                                            {
                                                if (lstSurveySectionLogic[0].LogicType == "Repeat Logic")
                                                {
                                                    SurveyQuestionID = lstSurveyQuestion[i].QuestionID;
                                                   
                                                    repeatQuestion.Add(SurveyQuestionID);
                                                    //var repeartQuestionCount = repeatQuestion.Where(x => x.Equals(SurveyQuestionID)).Count();
                                                    var repeatQuestionCount = repeatQuestion.Distinct().Count();
                                                    if (repeatQuestionCount == sheet)
                                                    {
                                                        lstExcelSurveyQuestion.Where(w => w.QuestionID == SurveyQuestionID).ToList().ForEach(w => w.Answer = "0");
                                                        Readsheet = true;
                                                        break;
                                                       
                                                    }
                                                }
                                            }
                                        }
                                        if (Readsheet == true)
                                        {
                                            for (int Logiccol = 2; Logiccol <= columnCount; Logiccol++)
                                            {
                                                var RQstInd = 0;

                                                questionId = lstSurveySectionLogic.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, Logiccol].Value)).Select(p => p.QuestionID).FirstOrDefault();
                                                sectionID = lstSurveySectionLogic.Where(p => p.QuestionID == questionId).Select(p => p.SectionID).FirstOrDefault();
                                                // fieldVar = lstSurveySectionLogic.Where(p => p.QuestionID == questionId).Select(p => p.FieldVar).FirstOrDefault();
                                                // if (repeatQuestion.Count > 0)
                                                // {


                                                // break;
                                                // }

                                                // setting the default answer as 0

                                                List<int> pageCount = new List<int>();
                                                for (int row = 2; row <= rowCount; row++)
                                                {

                                                    //LogicResponseID = RepeatDetails[mainRowIndex - 2].ResponseID;

                                                    pageCount.Add(Convert.ToInt32(worksheet.Cells[row, 1].Value));
                                                    ExcelSurvayQuestion ExcelObj = new ExcelSurvayQuestion();

                                                    ExcelObj.BeneficiaryInd = 0;
                                                    ExcelObj.ResponseID = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                                                    ExcelObj.SurveyID = objProjectSurvey.SurveyID;
                                                    ExcelObj.SectionID = sectionID;
                                                    ExcelObj.QuestionID = questionId;
                                                    //ExcelObj.FieldVar = fieldVar;
                                                    ExcelObj.RQstInd = Index;
                                                    // ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                    if (worksheet.Cells[row, Logiccol].Value != null)
                                                    {
                                                        // Check if the cell value represents a date
                                                        if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, Logiccol].Text), out DateTime dateValue))
                                                        {
                                                            // It's a valid date, retrieve the displayed text
                                                            ExcelObj.Answer = worksheet.Cells[row, Logiccol].Text;
                                                        }
                                                        else
                                                        {
                                                            // It's not a date, use the underlying value as a string
                                                            ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                    }
                                                    lstExcelSurveyQuestion.Add(ExcelObj);
                                                    Index++;
                                                }

                                                for (int row = 2; row <= rowCount; row++)
                                                {
                                                    var mainRowIndex = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                                                    var RepeatDetails = lstExcelSurveyQuestion.Where(w => w.QuestionID == SurveyQuestionID).ToList();

                                                    RQstInd = RepeatDetails[mainRowIndex - 2].RQstInd;
                                                    LogicResponseID = RepeatDetails[mainRowIndex - 2].ResponseID;
                                                    RepeatTaskSubmittedDate = RepeatDetails[mainRowIndex - 2].TaskSubmittedDate;
                                                    if (Logiccol == 2)
                                                    {
                                                     
                                                        var repeartCount = pageCount.Where(x => x.Equals(Convert.ToInt32(worksheet.Cells[row, 1].Value))).Count();
                                                        lstExcelSurveyQuestion.Where(w => w.RQstInd == RQstInd).ToList().ForEach(w => w.Answer = repeartCount.ToString());


                                                    }
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.TaskSubmittedDate = RepeatTaskSubmittedDate);
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.TaskDate = RepeatTaskSubmittedDate);
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.ResponseID = LogicResponseID);

                                                    

                                                }


                                            }


                                        }
                                        
                                    }

                                        
                                }


                                else
                                {
                                    List<SurveySectionLogic> logicSectionQustns = new List<SurveySectionLogic>();
                                    if (worksheet.Name == "Main" && sheet > 0)
                                    {
                                        for (int i = 0; i < lstSurveyQuestion.Count; i++)
                                        {
                                            lstSurveySectionLogic = lstSurveyQuestion[i].lstSurveySectionLogic;
                                            if (lstSurveySectionLogic.Count() > 0)
                                            {
                                                if (lstSurveySectionLogic[0].LogicType == "Repeat Logic")
                                                {
                                                    SurveyQuestionID = lstSurveyQuestion[i].QuestionID;
                                                    SurveyQuestionName = lstSurveyQuestion.Where(p => p.QuestionID == SurveyQuestionID).Select(p => p.QuestionName).FirstOrDefault();
                                                }
                                            }
                                        }
                                        int count = ExcludeRowsWithValue(file, worksheet, SurveyQuestionName);
                                        rowCount = count;
                                        
                                    }


                                    if (worksheet.Name == "Main")
                                    {
                                        if (DateTime.TryParse(Convert.ToString(worksheet.Cells[2, 1].Value), out DateTime dateTest))
                                        {
                                            // It's a valid date, retrieve the displayed text
                                            TaskSubmittedDate = Convert.ToDateTime(worksheet.Cells[2, 1].Value);
                                        }


                                        for (int col = 2; col <= columnCount-1; col++)
                                        {
                                            questionId = lstSurveyQuestion.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).Select(p => p.QuestionID).FirstOrDefault();
                                            sectionID = lstSurveyQuestion.Where(p => p.QuestionID == questionId).Select(p => p.SectionID).FirstOrDefault();
                                            fieldVar = lstSurveyQuestion.Where(p => p.QuestionID == questionId).Select(p => p.FieldVar).FirstOrDefault();

                                            var QuestionDtls = lstSurveyQuestion.Where(p => p.QuestionID == questionId).ToArray();
                                            if (QuestionDtls.Count() > 0)
                                            {
                                                if (QuestionDtls[0].IsConditionQuestion == true)
                                                {
                                                    if (QuestionDtls[0].lstSurveySectionLogic[0].LogicType != "Repeat Logic")
                                                    {
                                                        logicSectionQustns = QuestionDtls[0].lstSurveySectionLogic.ToList();
                                                    }
                                                }
                                            }
                                            if (logicSectionQustns.Count() > 0)
                                            {
                                                // var isExist = lstExcelSurveyQuestion.Where(p => p.QuestionID == questionId).ToArray();
                                                // if (isExist.Count() > 0)
                                                // {
                                                var IsExistInLogic = logicSectionQustns.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).ToArray();
                                                if (IsExistInLogic.Count() > 0)
                                                {
                                                    questionId = IsExistInLogic[0].QuestionID;
                                                    sectionID = IsExistInLogic[0].SectionID;
                                                }
                                                // }
                                                else
                                                {
                                                    if (questionId <= 0) //lstSurveyQuestion does not have logic question details 
                                                    {
                                                        //var IsExistInLogic = logicSectionQustns.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, col].Value)).ToArray();
                                                        if (IsExistInLogic.Count() > 0)
                                                        {
                                                            questionId = IsExistInLogic[0].QuestionID;
                                                            sectionID = IsExistInLogic[0].SectionID;
                                                        }
                                                    }
                                                }
                                            }
                                            for (int row = 2; row <= rowCount; row++)
                                            {

                                                ExcelSurvayQuestion ExcelObj = new ExcelSurvayQuestion();
                                                if (worksheet.Cells[row, 1].Value != null)
                                                {

                                                    if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, 1].Text), out DateTime dateValue))
                                                    {

                                                        ExcelObj.TaskDate = Convert.ToDateTime(worksheet.Cells[row, 1].Text);
                                                        ExcelObj.TaskSubmittedDate = Convert.ToDateTime(worksheet.Cells[row, 1].Text);
                                                    }

                                                }
                                                ExcelObj.BeneficiaryInd = 0;
                                                ExcelObj.ResponseID = 0;
                                                ExcelObj.SurveyID = objProjectSurvey.SurveyID;
                                                ExcelObj.SectionID = sectionID;
                                                ExcelObj.QuestionID = questionId;
                                                //ExcelObj.FieldVar = fieldVar;
                                                ExcelObj.RQstInd = Index;
                                                // ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                                if (worksheet.Cells[row, col].Value != null)
                                                {
                                                    // Check if the cell value represents a date
                                                    if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, col].Text), out DateTime dateValue))
                                                    {
                                                        // It's a valid date, retrieve the displayed text
                                                        ExcelObj.Answer = worksheet.Cells[row, col].Text;
                                                    }
                                                    else
                                                    {
                                                        // It's not a date, use the underlying value as a string
                                                        ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                                    }
                                                }
                                                else
                                                {
                                                    ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, col].Value);
                                                }
                                                lstExcelSurveyQuestion.Add(ExcelObj);
                                                Index++;
                                            }


                                        }

                                      
                                    }
                                    else
                                    {
                                        var Readsheet = false;
                                        for (int i = 0; i < lstSurveyQuestion.Count; i++)
                                        {
                                           
                                            lstSurveySectionLogic = lstSurveyQuestion[i].lstSurveySectionLogic;
                                            if (lstSurveySectionLogic.Count() > 0)
                                            {
                                                if (lstSurveySectionLogic[0].LogicType == "Repeat Logic")
                                                {
                                                    SurveyQuestionID = lstSurveyQuestion[i].QuestionID;
                                                    repeatQuestion.Add(SurveyQuestionID);
                                                    //var repeartQuestionCount = repeatQuestion.Where(x => x.Equals(SurveyQuestionID)).Count();
                                                    var repeatQuestionCount = repeatQuestion.Distinct().Count();
                                                    if (repeatQuestionCount == sheet)
                                                    {
                                                        lstExcelSurveyQuestion.Where(w => w.QuestionID == SurveyQuestionID).ToList().ForEach(w => w.Answer = "0");
                                                        Readsheet = true;
                                                        break;

                                                    }
                                                   // lstExcelSurveyQuestion.Where(w => w.QuestionID == SurveyQuestionID).ToList().ForEach(w => w.Answer = "0");
                                                }
                                            }
                                        }
                                        if (Readsheet == true)
                                        {
                                            for (int Logiccol = 2; Logiccol <= columnCount + 1; Logiccol++)
                                            {
                                                var RQstInd = 0;

                                                questionId = lstSurveySectionLogic.Where(p => p.QuestionName == Convert.ToString(worksheet.Cells[1, Logiccol].Value)).Select(p => p.QuestionID).FirstOrDefault();
                                                sectionID = lstSurveySectionLogic.Where(p => p.QuestionID == questionId).Select(p => p.SectionID).FirstOrDefault();
                                                // fieldVar = lstSurveySectionLogic.Where(p => p.QuestionID == questionId).Select(p => p.FieldVar).FirstOrDefault();



                                                // setting the default answer as 0

                                                List<int> pageCount = new List<int>();
                                                for (int row = 2; row <= rowCount; row++)
                                                {

                                                    pageCount.Add(Convert.ToInt32(worksheet.Cells[row, 1].Value));
                                                    ExcelSurvayQuestion ExcelObj = new ExcelSurvayQuestion();

                                                    ExcelObj.BeneficiaryInd = 0;
                                                    ExcelObj.ResponseID = Convert.ToInt32(worksheet.Cells[row, 1].Value); ;
                                                    ExcelObj.SurveyID = objProjectSurvey.SurveyID;
                                                    ExcelObj.SectionID = sectionID;
                                                    ExcelObj.QuestionID = questionId;
                                                    //ExcelObj.FieldVar = fieldVar;
                                                    ExcelObj.RQstInd = Index;
                                                    // ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                    if (worksheet.Cells[row, Logiccol].Value != null)
                                                    {
                                                        // Check if the cell value represents a date
                                                        if (DateTime.TryParse(Convert.ToString(worksheet.Cells[row, Logiccol].Text), out DateTime dateValue))
                                                        {
                                                            // It's a valid date, retrieve the displayed text
                                                            ExcelObj.Answer = worksheet.Cells[row, Logiccol].Text;
                                                        }
                                                        else
                                                        {
                                                            // It's not a date, use the underlying value as a string
                                                            ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ExcelObj.Answer = Convert.ToString(worksheet.Cells[row, Logiccol].Value);
                                                    }
                                                    lstExcelSurveyQuestion.Add(ExcelObj);
                                                    Index++;
                                                }
                                                for (int row = 2; row <= rowCount; row++)
                                                {
                                                    var RepeatDetails = lstExcelSurveyQuestion.Where(w => w.QuestionID == SurveyQuestionID).ToList();
                                                    var mainRowIndex = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                                                    RepeatTaskSubmittedDate = RepeatDetails[mainRowIndex - 2].TaskSubmittedDate;
                                                    RQstInd = RepeatDetails[mainRowIndex - 2].RQstInd;
                                                    if (Logiccol == 2)
                                                    {
                                                        

                                                        var repeartCount = pageCount.Where(x => x.Equals(Convert.ToInt32(worksheet.Cells[row, 1].Value))).Count();
                                                        lstExcelSurveyQuestion.Where(w => w.RQstInd == RQstInd).ToList().ForEach(w => w.Answer = repeartCount.ToString());

                                                    }
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.TaskSubmittedDate = RepeatTaskSubmittedDate);
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.TaskDate = RepeatTaskSubmittedDate);
                                                    lstExcelSurveyQuestion.Where(w => w.ResponseID == Convert.ToInt32(worksheet.Cells[row, 1].Value)).ToList().ForEach(w => w.ResponseID = 0);

                                                }

                                            }

                                        }  
                                        
                                    }
                                        

                                }
                            }
                            //int LoggedUserId = 519;
                            //var claimlist = User.Claims.ToList();
                            //if (claimlist.Count > 0)
                            //{
                            //    LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                            //}
                            // int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                            // Convert the DataTable to JSON
                            string jsonData = JsonConvert.SerializeObject(lstExcelSurveyQuestion, Formatting.Indented);


                            ObjSurveyResponse.SyncDBID = 0;
                            ObjSurveyResponse.Type = "Survey";
                            ObjSurveyResponse.TypeID = objProjectSurvey.SurveyID;
                            ObjSurveyResponse.TypeIDColumn = "SurveyID";
                            ObjSurveyResponse.Status = "DataCollected";

                            ObjSurveyResponse.WorkflowID = objProjectSurvey.WorkflowID;
                            ObjSurveyResponse.TotalWFLevel = objProjectSurvey.TotalWFLevel;
                            ObjSurveyResponse.WFLevel = 0;
                            ObjSurveyResponse.WFLevelStatus = "Completed";
                            ObjSurveyResponse.TaskUpdateComment = null;
                            ObjSurveyResponse.AttachedDocumentName = null;
                            ObjSurveyResponse.SurveyAnswers = jsonData;
                            ObjSurveyResponse.ResponseSyncID = 0;
                            ObjSurveyResponse.DataCollectedBy = LoggedUserId;
                            ObjSurveyResponse.NewBeneficiaries = "[]";
                            ObjSurveyResponse.TaskDate = TaskSubmittedDate;
                            ObjSurveyResponse.TaskSubmittedDate = TaskSubmittedDate;
                            ObjSurveyResponse.WorkspaceID = objProjectSurvey.WorkspaceID;
                            ObjSurveyResponse.SurveyAnswers = jsonData;
                            lstTaskData.Add(ObjSurveyResponse);





                            string jsonSurvey = JsonConvert.SerializeObject(lstTaskData, Formatting.Indented);

                            Guid guid = Guid.NewGuid();
                            string clientGuid = guid.ToString();

                            objWebService.SyncToServer_Survey(0, Beneficiarylinked, jsonSurvey, objProjectSurvey.ReportingFrequencyName, LoggedUserId, clientGuid);

                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No worksheets found in the Excel file.";
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred.";
                return StatusCode(400, errorMessage);
            }
        }
        public int ExcludeRowsWithValue(IFormFile file, ExcelWorksheet worksheet, string columnName)
        {
            string excludeValue = "Open Logic List"; // Replace with the value you want to exclude
            //int excludeColumn = 15; // Replace with the index of the specific column to exclude

            var package = new ExcelPackage(file.OpenReadStream());// Replace with your ExcelWorksheet object reference
                                                                  // ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Main");
            int rowCount = worksheet.Dimension.Rows;
            int excludeCount = 0;
            int excludeColumn = worksheet.Cells["1:1"].First(c => c.Value.ToString() == columnName).Start.Column;

            for (int i = 1; i <= rowCount; i++)
            {
                int emptyColumnCount = 0;

                for (int j = 1; j <= worksheet.Dimension.Columns; j++)
                {
                    // Check if the column is not the specific column to exclude and is not empty
                    if (j != excludeColumn && string.IsNullOrEmpty(worksheet.Cells[i, j].Value?.ToString()))
                    {
                        emptyColumnCount++;
                        //break;
                    }
                }

                // Check if the row has only the exclude value in the specific column and all other columns are empty
                if (emptyColumnCount == worksheet.Dimension.Columns-1 && worksheet.Cells[i, excludeColumn].Value?.ToString() == excludeValue)
                {
                    excludeCount++;
                }
            }

            int rowCountExcludingValues = rowCount - excludeCount;
            return rowCountExcludingValues;
            // Use the rowCountExcludingValues as needed
        }

        #endregion 

    }
}
