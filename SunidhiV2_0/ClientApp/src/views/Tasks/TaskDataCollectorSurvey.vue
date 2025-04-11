<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0">
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12" class="py-1 mb-0">
                <div v-for="(item,ind) in lstMyTask" :key="ind" class="pa-0 ma-0">
                    <v-row class="d-sm-none pa-0 ma-0 mb-2">
                        <v-col cols="12" align="center" class="pa-0 ma-0">
                            <v-chip color="#E1EFFF">
                                {{ new Date(item.TaskDate).toLocaleDateString('en-GB')}}
                            </v-chip>
                        </v-col>
                    </v-row>
                    <v-card class="mb-2 mb-md-4 task-survey-card" @click.native="GetTaskDetails(item)">
                        <v-card-title class="py-0 my-0 pt-2">
                            <v-row no-gutters class="py-0 my-0">
                                <v-col class="py-0 my-0">
                                    <h4 class="font-weight-bold heading-3-dark py-0 my-0">
                                        {{item.TaskName}}
                                    </h4>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <v-chip :color="item.TypeColor" class="ml-0 mr-2 task-survey-chip" label dark small>
                                        {{item.TaskType}}
                                    </v-chip>
                                </v-col>
                                <v-col md="auto" class="d-none d-sm-block py-0 my-0">
                                    <h4 class="font-weight-regular heading-3">
                                        {{ new Date(item.TaskDate).toLocaleDateString('en-GB')}}
                                    </h4>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">

                                    <v-menu offset-y transition="scroll-y-transition" v-model="lstMyTask[ind].vMenu">
                                        <template v-slot:activator="{ on }">
                                            <v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right; border:none; outline:none"
                                                   @click.stop="lstMyTask[ind].vMenu = true,IssueReportTaskDetails =item">
                                                <v-icon color="black">
                                                    mdi-dots-vertical
                                                </v-icon>
                                            </v-btn>
                                        </template>
                                        <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                            <v-list-item @click.stop="ReportIssuedlg = true">
                                                <v-list-item-icon>
                                                    <v-icon>mdi-trash-can-outline</v-icon>
                                                </v-list-item-icon>
                                                <v-list-item-content>
                                                    Report Issue
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list>
                                    </v-menu>
                                </v-col>
                            </v-row>
                        </v-card-title>
                        <v-card-text class="d-none d-sm-block py-0 mb-1">
                            <p class="font-weight-regular heading-3 text-truncate py-0 my-0">
                                {{item.TaskDescription}}
                            </p>
                        </v-card-text>
                        <v-card-actions class="px-4 py-0 pb-3">
                            <v-row>
                                <v-col class="py-0">
                                    <h4 class="font-weight-regular heading-3">
                                        <span>Project Name :</span> {{item.ProjectName}}
                                    </h4>
                                </v-col>
                                <v-spacer></v-spacer>
                                <v-col cols="auto" class="py-0">
                                    <v-chip color="#2C4258" class="task-bottom-chip" label dark small>
                                        To DataCollect
                                    </v-chip>
                                </v-col>
                            </v-row>
                        </v-card-actions>
                    </v-card>
                </div>
            </v-col>

            <!-- report Issue dilog -->
            <v-dialog v-model="ReportIssuedlg" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                            <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                        </v-avatar>Report Issue
                        <v-spacer></v-spacer>
                        <v-btn text @click="ReportIssuedlg=false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col>
                                <h4 class="text-left heading-3 mb-2">Issue</h4>
                                <v-textarea rows="5" outlined dense label="Description" v-model="vmIssueComment" type="text">
                                </v-textarea>
                                <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" v-model="IssuefileInput" prepend-icon="mdi-paperclip" outlined="" dense=""
                                              id="file" accept="*"></v-file-input>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right">
                                <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="ReportIssue()"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="vmIssueComment ='',IssuefileInput= null,IssueReportTaskDetails =[],ReportIssuedlg = false "></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>


            <!-- Survey Beneficiary linked Data Collector-->
            <v-dialog v-model="dlgSurveyType" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="ClearBenefSurveyUpdate()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                            </v-avatar>
                            {{lstSelectedTask.SurveyName}}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                            <v-chip class="ml-2 task-survey-chip" label dark small>
                                Survey
                            </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2" align="right" class="d-none d-sm-inline">
                            <v-btn text small @click="ClearBenefSurveyUpdate()">
                                <v-icon color="#707070" size="20">mdi-close</v-icon>
                            </v-btn>
                        </v-col>
                    </v-row>
                    <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsUpdate_BenSurvey == false && IsInputFinished == false" class="px-3 py-3">
                        <v-tabs grow v-model="ActivityTabs" class="pt-0 taskHeight" color="#52B962">
                            <v-tab href='#tab-1'>Task Details</v-tab>
                            <v-tab href='#tab-2'>History</v-tab>
                            <!-- Task Details-->
                            <v-tab-item value="tab-1">
                                <v-row justify="center" class="mt-4">
                                    <v-col md="12" class="v-scrolling-div px-6 historyHeight">
                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Task Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.SurveyName}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Action
                                                </h4>
                                                <v-chip color="#2C4258" label dark small>
                                                    To DataCollect
                                                </v-chip>
                                            </v-col>
                                        </v-row>

                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{ new Date(lstSelectedTask.TaskDate).toLocaleDateString('en-GB')}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.ProjectName}}</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            {{lstSelectedTask.Description}}
                                        </h4>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Beneficiary Type
                                                </h4>
                                                <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.BeneficiaryType}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-arrow-right</v-icon>
                                                </v-avatar>
                                                Pending
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                            <!-- Task History-->
                            <v-tab-item value="tab-2">
                                <v-row justify="center">
                                    <v-col>
                                        <v-card flat class="py-0">
                                            <v-card-text class="v-scrolling-div historyHeight">
                                                <v-list dense class="py-0">
                                                    <template v-for="(item, index) in lstLogFrame">
                                                        <v-list-item class="py-0">
                                                            <v-list-item-icon>
                                                                <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                            </v-list-item-icon>
                                                            <v-list-item-content>
                                                                <v-list-item-title class="py-0 pb-0">
                                                                    <v-row class="d-lg-inline-flex py-0 pb-0">
                                                                        <v-col class="px-0 pl-3 py-0">
                                                                            <h4 class="font-weight-large   py-0 pb-0 px-0">{{item.Name}}</h4>
                                                                        </v-col>
                                                                        <v-col class="ml-3 px-0 py-0">
                                                                            <h4 class="font-weight-regular  py-0 pb-0">{{item.Message}}</h4>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-list-item-title>
                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                </v-list-item-subtitle>
                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0" style="height:auto;max-height:100px;overflow-y:auto">{{item.TaskUpdateComment}}</h4>
                                                                </v-list-item-subtitle>
                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                    <!--<v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">-->
                                                                    <!--<v-chip v-show="item.AttachedDocumentName.length >0 " small color="red" class="mb-0">
                                                                        {{item.AttachedDocumentName}}
                                                                    </v-chip>-->
                                                                    <!--</v-chip-group>-->
                                                                    <v-row>
                                                                        <!--<v-col md="8">
                                                                            <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                        </v-col>-->
                                                                        <v-col md="8">
                                                                            <v-btn v-show="item.AttachedDocumentName.length >0 " color="red" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDocument(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                <v-icon size="26" color="red">mdi-file-download-outline</v-icon>{{item.AttachedDocumentName}}
                                                                            </v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-list-item-subtitle>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </template>
                                                </v-list>
                                            </v-card-text>
                                        </v-card>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                        </v-tabs>
                        <v-row class="py-0 my-2">
                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="IsUpdate_BenSurvey = true" v-bind:disabled="this.lstSurveyQstns.length==0"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="ClearBenefSurveyUpdate()"></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                    <!-- Task Questions and input feilds -->
                    <v-card-text v-show="IsUpdate_BenSurvey == true && IsInputFinished == false" class="px-3 py-3">
                        <v-stepper v-model="BenefLinkedSurveySteps" class="elevation-0">
                            <v-stepper-items>
                                <!-- Select Create or Existing-->
                                <v-stepper-content step="1" class="pa-0">
                                    <v-row class="taskHeight ml-3" no-gutters>
                                        <v-col>
                                            <v-row>
                                                <v-col cols="12" md="12">
                                                    <h4 class="text-left heading-3">Select how you want to update task</h4>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" md="12" class="py-0 my-0">
                                                    <v-radio-group class="py-0 my-0" v-model="vmAddBeneficiaryChoice">
                                                        <v-radio color="success" label="Search and update existing beneficiary" value="1"></v-radio>
                                                        <v-radio color="success" label="Add a new beneficiary" value="2"></v-radio>
                                                    </v-radio-group>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetBeneficiary()" v-bind:disabled="vmAddBeneficiaryChoice==0"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsUpdate_BenSurvey = false"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- Select from Exisiting beneficiaries-->
                                <v-stepper-content step="2" class="pa-0">
                                    <v-row class="taskHeight">
                                        <v-col cols="12" md="12">
                                            <v-text-field class="mt-3" type="text" prepend-inner-icon="mdi-magnify" label="Search" full-width outlined dense hide-details v-model="vmSearchBeneficiary"></v-text-field>
                                            <v-data-table :headers="BeneficiaryTableheaders" v-model="lstSelectedBeneficiary" :search="vmSearchBeneficiary"
                                                          :items="lstBeneficiary"
                                                          show-select
                                                          item-key="BeneficiaryID"
                                                          sort-by="BeneficiaryName"
                                                          fixed-header height="44vh"
                                                          class="elevation-0 mt-3">
                                                <template v-slot:item.CreatedDate="{ item }">
                                                    <span>{{new Date(item.CreatedDate).toLocaleDateString('en-GB')}}</span>
                                                </template>
                                                <!--<template v-slot:item.data-table-select="{ item, isSelected, select }">
                                                    <v-simple-checkbox :value="isSelected"
                                                                       :readonly="IsSelected(item)"
                                                                       :disabled="IsSelected(item)"
                                                                       @input="select($event)"></v-simple-checkbox>
                                                </template>-->
                                            </v-data-table>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetSelectedBeneficiaries()" v-bind:disabled="!CheckMandatory()"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps =1"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- How to add new beneficiary-->
                                <v-stepper-content step="3" class="pa-0">
                                    <v-row class="taskHeight ml-1">
                                        <v-col cols="12" md="12">
                                            <h4 class="text-left heading-3 mt-3">Select how you want to add new beneficiary</h4>
                                            <v-radio-group v-model="vmBenefRegisterChoice">
                                                <v-radio color="success" label="Register" value="1"></v-radio>
                                                <v-radio color="success" label="Import & Register" value="2"></v-radio>
                                            </v-radio-group>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetBenefRegistrationQstns()"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps = 1"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>

                                </v-stepper-content>
                                <!-- How to update the task - linked from existing beneficiary-->
                                <v-stepper-content step="4" class="pa-0">
                                    <v-row class="taskHeight ml-1">
                                        <v-col cols="12" md="12">
                                            <h4 class="text-left heading-3 ">Select how you want to update the Task</h4>
                                            <v-radio-group v-model="vmBenefSurveyChoice">
                                                <v-radio color="success" label="Import" value="1"></v-radio>
                                                <v-radio color="success" label="Update Manually" value="2"></v-radio>
                                            </v-radio-group>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetBenefSurveyUpdateChoice()" v-bind:disabled="vmBenefSurveyChoice==0"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps = 2"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- Survey Question Update Manually -->
                                <v-stepper-content step="5" class="pa-0 py-0 my-0">
                                    <div class="taskHeight">
                                        <v-row v-for="(benefItem,Index) in vmSelectedBeneficiaries" :key="benefItem.BeneficiaryID">

                                            <v-col cols="12" md="12" class="pt-0 my-0 py-0">
                                                <div v-if="BenefStepper== Index+1 ">
                                                    <!--<h4 class="text-center page-head"> Beneficiary {{BenefStepper}} of {{vmSelectedBeneficiaries.length}}</h4>-->
                                                    <h4 class="text-center page-head"> {{benefItem.BeneficiaryName}}  of {{vmSelectedBeneficiaries.length}}</h4>
                                                    <h4 class="text-center page-head mt-3">Section {{BenefQstnStepper[BenefStepper]}} of {{lstSections.length}}</h4>
                                                </div>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="6" md="8">
                                                <h4 class="text-left page-head mb-2">{{vmSection.SectionName}}</h4>
                                            </v-col>
                                            <v-spacer class="d-sm-none"></v-spacer>
                                            <v-col cols="6" md="4" class="my-0 py-0">
                                                <v-select :items="lstSections"
                                                          item-text="SectionName"
                                                          item-value="SectionID" return-object=""
                                                          hide-details dense="" outlined item-disabled="customDisabled" id="select"
                                                          v-model="vmSection" @change="GetSectionQuestions1($event,BenefStepper,0)">
                                                </v-select>
                                            </v-col>
                                        </v-row>
                                        <v-row class="v-scrolling-div mx-1 my-0 py-0 scrollBoxHeight">
                                            <v-col class="py-0 my-0 ml-2">
                                                <div id="scrollToTop"></div>
                                                <div v-for="(item,QIndex) in lstSectionQstns" :key="QIndex" class="mb-4">

                                                    <label v-bind:id="item.QuestionID.toString()" class="text-left heading-3 mb-2" v-if="item.QuestionID==0"><b>{{item.QuestionName}}</b></label>
                                                    <label v-bind:id="item.QuestionID.toString()" class="text-left heading-3 mb-2" v-if="item.QuestionID>0">{{item.QuestionName}}</label>
                                                    <sub v-if="item.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                    <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{item.QuestionHint}}</sub>
                                                    <!-- Code updated for Survey Cleanup 09-02-2021  IsValidationError-->
                                                    <!--<sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| chkQstnValid ">{{item.ErrorText}}</sub>-->

                                                    <sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| item.IsValidationError ">{{item.ValidationErrorTest}}</sub>

                                                    <!-- Single line text with validation condition -->
                                                    <div v-if="item.QuestionType=='Single Line Text' && (item.IsValidationQuestion || item.IsConditionQuestion) ">
                                                          <!--<v-text-field v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer" :value="item.DefaultValue" :min="item.minVal" :max="item.maxVal" :type="item.qType" v-bind:maxlength="item.textLength" :rules="item.qRules" @focus="onFocusCheckQstnValidation(item)" @change="AddSectionLogic(item.QuestionID,BenefStepper)" />-->
                                                            <input type="text" class="custom-text-field" solo :value="item.Answer" @change="fnBindSurveyAnswer($event, QIndex), AddSectionLogic(item.QuestionID,BenefStepper)" />

                                                    </div>
                                                    <!-- Single line text without validation condition-->
                                                    <div v-if="item.QuestionType=='Single Line Text' && !(item.IsValidationQuestion || item.IsConditionQuestion) ">
                                                        <!-- <v-text-field v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                  :value="item.DefaultValue" @change="CheckQstnValidation(item)" /> -->
                                                        <!--<input type="text" class="custom-text-field" v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer0" v-model="item.Answer"
                   value="item.DefaultValue" @change="CheckQstnValidation(item)">-->
                                                        <input type="text" class="custom-text-field" solo :value="item.Answer" @change="fnBindSurveyAnswer($event, QIndex)" />

                                                    </div>
                                                    <!--Multiline Answer-->
                                                    <!-- <v-textarea v-if="item.QuestionType==='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer" /> -->
                                                    <textarea  class="custom-text-field" v-if="item.QuestionType==='Multi Line Text'" full-width outlined dense label="Type your answer" :value="item.Answer" @change="fnBindSurveyAnswer($event, QIndex)">
                                                    </textarea>
                                                    <!--Numeric-->
                                                    <v-flex xs12 sm12 md12>
                                                        <div v-if="item.QuestionType==='Numeric'">
                                                            <v-layout row>
                                                                <v-flex grow>
                                                                   <!--<v-text-field v-if="item.QuestionType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="item.Answer" class="ml-3" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"onKeyPress="if(this.value.length==10) return false;" :readonly="item.IsFormulaQuestion == true":value="item.DefaultValue" :min="item.minVal" :max="item.maxVal"
                                          @focus="Calculate(item.QuestionID,BenefStepper), onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item),AddSectionLogic(item.QuestionID,BenefStepper)" :rules="item.qRules">
                                                            </v-text-field>-->
                                                                    <input type="number" class="custom-text-field ml-3" solo :value="item.Answer" @focus="Calculate(item.QuestionID,BenefStepper)" @change="fnBindSurveyAnswer($event, QIndex),AddSectionLogic(item.QuestionID,BenefStepper)" />
                                                                </v-flex>
                                                                <v-flex shrink>
                                                                    <sub class="mr-2 ml-2" style="color:green;font-size:large">{{item.UOM}}</sub>
                                                                </v-flex>
                                                            </v-layout>
                                                        </div>
                                                    </v-flex>
                                                    <!--Single Choice Answer-->
                                                    <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                        <v-radio-group class="my-0 py-0" v-model.lasy="item.Answer" @change="AddSectionLogic(item.QuestionID,BenefStepper)">
                                                            <v-radio color="success" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                     :label="optItem.OptionValue"
                                                                     :value="optItem.OptionValue"></v-radio>
                                                        </v-radio-group>
                                                    </div>
                                                    <!--Multiple Choice -->
                                                    <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                        <div class="my-0 py-0" v-if="item.QuestionOptions.length>0">
                                                            <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                        v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,BenefStepper),AddSectionLogicForMultiChoice(optItem,item.QuestionID,BenefStepper,item.QuestionOptions)"></v-checkbox>

                                                        </div>
                                                    </div>
                                                    <!--Dropdown -->
                                                    <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                        <div v-if="item.QuestionOptions.length>0">
                                                            <v-select dense :items="item.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                      item-text="OptionValue"
                                                                      label="Select" outlined hide-details :value="item.Answer" @change="fnBindSurveyAnswer($event, QIndex), AddSectionLogic(item.QuestionID,BenefStepper)">
                                                            </v-select>
                                                        </div>
                                                    </div>
                                                    <!--Date Picker-->
                                                    <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                        <v-menu v-model="item.DateMenu"
                                                                :close-on-content-click="false"
                                                                :nudge-right="40"
                                                                transition="scale-transition"
                                                                offset-y
                                                                min-width="290px">
                                                            <template v-slot:activator="{ on }">
                                                                <v-text-field v-model="item.Answer"
                                                                              prepend-inner-icon="mdi-calendar"
                                                                              outlined
                                                                              readonly
                                                                              dense
                                                                              @click:prepend-inner="item.DateMenu = true"
                                                                              v-on="on" hide-details></v-text-field>
                                                            </template>
                                                            <v-date-picker v-model.lasy="item.Answer" no-title @input="item.DateMenu = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(item)"></v-date-picker>
                                                        </v-menu>
                                                    </div>
                                                    <!--GPS Capture-->
                                                    <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model="item.Answer">
                                                        <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                        <GmapMap :center="Coordinates"
                                                                 :zoom="13"
                                                                 style="width: 100%; height: 200px" @click.native="GetGPSCoordinates(item.QuestionID,false)">
                                                            <GmapMarker :position="Coordinates"
                                                                        :clickable="true"
                                                                        :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                        </GmapMap>
                                                    </div>
                                                    <!--Note-->
                                                    <div class="mb-6" v-if="item.QuestionType==='Note'">
                                                        <!-- <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea> -->
                                                        <textarea class="custom-text-field" outlined dense hide-details v-model="item.Answer" >

                                                        </textarea>
                                                    </div>
                                                    <!--Photo Capture-->
                                                    <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">

                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                            <v-radio color="success" label="Photo Capture" hide-details>
                                                            </v-radio>
                                                            <v-radio color="success" label="Photo Upload" hide-details>
                                                            </v-radio>
                                                        </v-radio-group>
                                                        <div v-show="item.IsUpload===0">
                                                            <v-row>
                                                                <v-col md="12">

                                                                    <v-btn @click="item.startCamera =true,item.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                    <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item.QuestionID,false,0),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                    <v-btn @click="item.startCamera =false,item.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row>
                                                                <v-col md="8" v-if="item.startCamera">
                                                                    <vue-web-cam ref="webcam"
                                                                                 :device-id="deviceId"
                                                                                 width="100%"
                                                                                 height="200px"
                                                                                 @started="onStarted"
                                                                                 @stopped="onStopped"
                                                                                 @error="onError"
                                                                                 @cameras="onCameras"
                                                                                 @camera-change="onCameraChange" />
                                                                    <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                </v-col>

                                                                <v-col md="4" v-if="item.img !=null && item.img != ''">
                                                                    <figure class="figure">
                                                                        <img :src="item.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                    </figure>
                                                                </v-col>

                                                            </v-row>
                                                        </div>
                                                        <div v-show="item.IsUpload===1">
                                                            <v-row>
                                                                <v-col md="4">

                                                                    <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile('Photo',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                </v-col>
                                                                <v-col md="4" v-if="item.uploadedImg !=null">
                                                                    <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                </v-col>
                                                            </v-row>
                                                        </div>

                                                    </div>
                                                    <!--Audio Capture-->
                                                    <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                            <v-radio color="success" label="Audio Capture" hide-details>
                                                            </v-radio>
                                                            <v-radio color="success" label="Audio Upload" hide-details>

                                                            </v-radio>
                                                        </v-radio-group>
                                                        <div v-show="item.IsUpload===0">
                                                            <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                            :play="playSound"
                                                                            :attempts="1"
                                                                            :time="1"
                                                                            :headers="headers"
                                                                            :before-recording="callback"
                                                                            :pause-recording="callback"
                                                                            :after-recording="SaveAudio"
                                                                            :before-upload="callback"
                                                                            :successful-upload="callback"
                                                                            :failed-upload="callback"
                                                                            v-model="item.Answer"
                                                                            @click.native="AnsBlobQID=item.QuestionID" />

                                                        </div>
                                                        <div v-show="item.IsUpload===1">

                                                            <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Audio',item.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                        </div>
                                                    </div>
                                                    <!--Signature Capture-->
                                                    <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                            <v-radio color="success" label="Signature Capture" hide-details>

                                                            </v-radio>
                                                            <v-radio color="success" label="Signature Upload" hide-details>

                                                            </v-radio>
                                                        </v-radio-group>
                                                        <div v-show="item.IsUpload===0">
                                                            <v-card outlined>
                                                                <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                            </v-card>
                                                            <div>
                                                                <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=item.signaturebutton @click="saveSign(item.QuestionID,false,BenefStepper),item.signaturebutton=true">Save</v-btn>
                                                                <v-btn default depressed class="mt-2 mr-2" @click="clearSign(false,item.QuestionID),item.signaturebutton=false">Clear</v-btn>
                                                            </div>
                                                        </div>
                                                        <div v-show="item.IsUpload===1">

                                                            <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Signature',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                        </div>
                                                        <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                                            <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                            <!--max-width="100%" height="200" aspect-ratio="1.7"-->
                                                            <!--<v-img :src=uploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>-->
                                                        </v-card>
                                                    </div>
                                                    <!--Cascading Dropdown-->
                                                    <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                        <v-row class="my-0 py-0">
                                                            <v-col class="my-0 py-0">
                                                                <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                          :label="` ${optItem.OptionValue}`" outlined=""
                                                                          v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                          :items="optItem.CascadingList"
                                                                          item-text="Name"
                                                                          item-value="ID" @change="FilterSubItem(item,optItem,optItem[ind],false,BenefStepper,false,-1)">
                                                                </v-select>


                                                            </v-col>
                                                        </v-row>
                                                    </div>
                                                    <!--File Upload-->
                                                    <div v-if="item.QuestionType==='File Upload'" class="mt-0">

                                                        <v-file-input label="Upload File" outlined dense accept="*" v-model="item.AnsBlob"
                                                                      @change="AnsBlobQID=item.QuestionID,SaveFile('File',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                        <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                        <div v-if="item.uploadedFile !=null && item.uploadedFile != ''">
                                                            <v-img :src=item.uploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                        </div>
                                                    </div>
                                                    <!-- Likert Scale-->
                                                    <div v-if="item.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                        <v-radio-group class="my-0 py-0" v-model="item.Answer">
                                                            <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                     :label="optItem.OptionValue"
                                                                     :value="optItem.OptionValue"></v-radio>
                                                        </v-radio-group>
                                                    </div>
                                                    <!-- Repeat Section Question Details comes here-->
                                                    <div v-if="RepeatSection.length >0 && Ritem.ParentQuestionID == item.QuestionID && BenefStepper == Ritem.benefIndex" v-for="(Ritem,RIndex) in RepeatSection" :key="RIndex" class="mb-4">

                                                        <label class="text-left heading-3 mb-2" v-if="Ritem.QuestionID==0"><b>{{Ritem.QuestionName}}</b></label>
                                                        <label class="text-left heading-3 mb-2" v-if="Ritem.QuestionID>0">{{Ritem.QuestionName}}</label>
                                                        <sub v-if="Ritem.IsMandatory==true" style="color:red;font-size:large"> *</sub>
                                                        <sub style="color:red;font-size:smaller" v-if="Ritem.QuestionHint.length > 0 && Ritem.QuestionHint != null && Ritem.QuestionHint != 'null'">{{Ritem.QuestionHint}}</sub>
                                                        <!-- Error Text-->
                                                        <sub style="color:red;font-size:smaller" v-if="(Ritem.IsMandatory==true && Ritem.Answer ==='')|| Ritem.IsValidationError ">{{Ritem.ValidationErrorTest}}</sub>

                                                        <!-- Single line text with Condition Question-->
                                                        <div v-if="Ritem.QuestionType=='Single Line Text' && (Ritem.IsValidationQuestion || Ritem.IsConditionQuestion) ">
                                                            <!--<v-text-field v-if="Ritem.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer"
                  :value="Ritem.DefaultValue" :min="minVal" :max="maxVal" :type="qType" v-bind:maxlength="txtLength" :rules="qRules"
                  @focus="onFocusCheckQstnValidation(Ritem)" @change="AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)" />-->

                                                            <input type="text" class="custom-text-field" solo :value="Ritem.Answer" @change="fnBindSurveyAnswerLogic($event, RIndex), AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)" />
                                                        </div>
                                                        <!-- Single line text without Condition Question-->
                                                        <div v-if="Ritem.QuestionType=='Single Line Text' && !(Ritem.IsValidationQuestion || Ritem.IsConditionQuestion) ">
                                                            <!-- <v-text-field v-if="Ritem.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer"
                                                                          :value="Ritem.DefaultValue" @change="CheckQstnValidation(item)" /> -->
                                                           <!--<input type="text" class="custom-text-field" v-if="Ritem.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer"
                                                                       value="Ritem.DefaultValue" @change="CheckQstnValidation(item)">-->

                                                            <input type="text" class="custom-text-field" solo :value="Ritem.Answer" @change="fnBindSurveyAnswerLogic($event, RIndex)" />

                                                        </div>

                                                        <!--Multiline Answer-->
                                                        <!-- <v-textarea v-if="Ritem.QuestionType==='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer" /> -->
                                                        <textarea class="custom-text-field" full-width outlined dense v-if="Ritem.QuestionType==='Multi Line Text'" label="Type your answer" :value="Ritem.Answer" @change="fnBindSurveyAnswerLogic($event, RIndex)" >

                                                        </textarea>
                                                        <!-- Numeric text feilds-->
                                                        <v-flex xs12 sm12 md12>
                                                            <div v-if="Ritem.QuestionType==='Numeric'">
                                                                <v-layout row>
                                                                    <v-flex grow>
                                                                        <!-- <v-text-field v-if="Ritem.QuestionType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="Ritem.Answer"
                  class="ml-3"
                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                  onKeyPress="if(this.value.length==10) return false;" :readonly="Ritem.IsFormulaQuestion == true"
                  :value="Ritem.DefaultValue" :min="minVal" :max="maxVal"
                  @focus="Calculate(Ritem.QuestionID,BenefStepper)" @change="onFocusCheckQstnValidation(Ritem),AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)">
    </v-text-field> -->
                                                                        <!--<input type="number" class="custom-text-field" full-width outlined dense v-if="Ritem.QuestionType==='Numeric'" label="Type your answer" v-model="Ritem.Answer"
        onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
        onkeypress="if(this.value.length==10) return false;" :readonly="Ritem.IsFormulaQuestion == true"
        value="Ritem.DefaultValue" min="minVal" max="maxVal"
        @focus="Calculate(Ritem.QuestionID,BenefStepper)" @change="onFocusCheckQstnValidation(Ritem),AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)">-->
                                                                        <input type="number" class="custom-text-field ml-3" solo :value="Ritem.Answer" @focus="Calculate(Ritem.QuestionID,BenefStepper)"  @change="fnBindSurveyAnswerLogic($event, RIndex)" />
                                                                    </v-flex>
                                                                    <v-flex shrink>
                                                                        <sub class="mr-2 ml-2" style="color:green;font-size:large">{{Ritem.UOM}}</sub>
                                                                    </v-flex>
                                                                </v-layout>
                                                            </div>
                                                        </v-flex>
                                                        <!--Single Choice Answer-->
                                                        <div v-if="Ritem.QuestionType==='Single Choice'" class="mt-0">

                                                            <v-radio-group class="my-0 py-0" v-model.lasy="Ritem.Answer" @change="AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                <v-radio color="success" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                         :label="optItem.OptionValue"
                                                                         :value="optItem.OptionValue"></v-radio>
                                                            </v-radio-group>

                                                        </div>
                                                        <!--Multiple Choice -->
                                                        <div v-if="Ritem.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                            <div class="my-0 py-0" v-if="RepeatSection[Ritem.RQstInd].QuestionOptions.length>0">
                                                                <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in RepeatSection[Ritem.RQstInd].QuestionOptions" :key="ind" v-model.lasy="RepeatSection[Ritem.RQstInd].QuestionOptions[ind].IsOptSelected"
                                                                            @change="GetMultiChoiceRepeat(optItem,optItem.IsOptSelected,optItem.OptionValue,Ritem.RQstInd,Ritem.QuestionID,BenefStepper),AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                </v-checkbox>
                                                            </div>
                                                        </div>
                                                        <!--Dropdown -->
                                                        <div v-if="Ritem.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                            <div v-if="Ritem.QuestionOptions.length>0">
                                                                <v-select dense :items="Ritem.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                          item-text="OptionValue"
                                                                          label="Select" outlined hide-details :value="Ritem.Answer" @change="fnBindSurveyAnswerLogic($event, RIndex),AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                </v-select>
                                                            </div>
                                                        </div>
                                                        <!--Date Picker-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Date Picker'">
                                                            <v-menu v-model="Ritem.DateMenu"
                                                                    :close-on-content-click="false"
                                                                    :nudge-right="40"
                                                                    transition="scale-transition"
                                                                    offset-y
                                                                    min-width="290px">
                                                                <template v-slot:activator="{ on }">
                                                                    <v-text-field v-model="Ritem.Answer"
                                                                                  prepend-inner-icon="mdi-calendar"
                                                                                  outlined
                                                                                  readonly
                                                                                  dense
                                                                                  @click:prepend-inner="Ritem.DateMenu = true"
                                                                                  v-on="on" hide-details></v-text-field>
                                                                </template>
                                                                <v-date-picker v-model.lasy="Ritem.Answer" no-title @input="Ritem.DateMenu = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(Ritem)"></v-date-picker>
                                                            </v-menu>
                                                        </div>
                                                        <!--GPS Capture-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='GPS Capture'" v-model="Ritem.Answer">
                                                            <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                            <GmapMap :center="Coordinates"
                                                                     :zoom="13"
                                                                     style="width: 100%; height: 200px" @click.native="GetGPSCoordinatesRepeat(Ritem.QuestionID,false, Ritem.RQstInd)">
                                                                <GmapMarker :position="Coordinates"
                                                                            :clickable="true"
                                                                            :draggable="true" @dragend="updateCoordinates($event.latLng, Ritem)" />
                                                            </GmapMap>
                                                        </div>
                                                        <!--Note-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Note'">
                                                            <!-- <v-textarea outlined dense hide-details v-model="Ritem.Answer"></v-textarea> -->
                                                            <textarea class="custom-text-field" outlined dense hide-details v-model="Ritem.Answer" ></textarea>
                                                        </div>
                                                        <!--Photo Capture-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Photo Capture'">

                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                <v-radio color="success" label="Photo Capture" hide-details>
                                                                </v-radio>
                                                                <v-radio color="success" label="Photo Upload" hide-details>
                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="Ritem.IsUpload===0">
                                                                <v-row>
                                                                    <v-col md="12">

                                                                        <v-btn @click="Ritem.startCamera =true,Ritem.camOn=true,onStartRepeat()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                        <v-btn class="mr-4" color="secondary" depressed @click="onCaptureRepeat(Ritem.QuestionID,false,0,Ritem.RQstInd,Ritem.webcam),Ritem.startCamera =false,Ritem.camOn=false" v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                        <v-btn @click="Ritem.startCamera =false,Ritem.camOn=false,onStop()" v-bind:disabled="!Ritem.camOn" depressed>Stop</v-btn>
                                                                        <v-btn color="primary" depressed class="mr-4" @click="fnflipCamera()">flip</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col md="8" v-if="Ritem.startCamera">
                                                                        <vue-web-cam ref="webcam"
                                                                                     :device-id="deviceId"
                                                                                     width="100%"
                                                                                     height="200px"
                                                                                     @started="onStarted"
                                                                                     @stopped="onStopped"
                                                                                     @error="onError"
                                                                                     @cameras="onCameras"
                                                                                     @camera-change="onCameraChangeRepeat" />
                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->
                                                                    </v-col>
                                                                    <v-col md="4" v-if="Ritem.img !=null">
                                                                        <figure class="figure">
                                                                            <!--<img :src="img" class="center-block my-2" width="250" height="180" aspect-ratio="1.7" />-->
                                                                            <img :src="Ritem.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                        </figure>
                                                                    </v-col>
                                                                </v-row>
                                                            </div>
                                                            <div v-show="Ritem.IsUpload===1">
                                                                <v-row>
                                                                    <v-col md="4">

                                                                        <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="Ritem.AnsBlob"
                                                                                      @change="AnsBlobQID=Ritem.QuestionID,SaveFileRepeat('Photo',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd )"></v-file-input>
                                                                    </v-col>
                                                                    <v-col md="4" v-if="Ritem.uploadedImg !=null && Ritem.uploadedImg !=''">
                                                                        <v-img :src=Ritem.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                    </v-col>
                                                                </v-row>
                                                            </div>

                                                        </div>
                                                        <!--Audio Capture-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Audio Capture'">
                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                <v-radio color="success" label="Audio Capture" hide-details>
                                                                </v-radio>
                                                                <v-radio color="success" label="Audio Upload" hide-details>

                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="Ritem.IsUpload===0">
                                                                <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                :play="playSound"
                                                                                :attempts="1"
                                                                                :time="1"
                                                                                :headers="headers"
                                                                                :before-recording="callback"
                                                                                :pause-recording="callback"
                                                                                :after-recording="SaveAudioRepeat"
                                                                                :before-upload="callback"
                                                                                :successful-upload="callback"
                                                                                :failed-upload="callback"
                                                                                v-model="Ritem.Answer"
                                                                                @click.native="AnsBlobQID=Ritem.QuestionID, RQstInd = Ritem.RQstInd" />

                                                            </div>
                                                            <div v-show="Ritem.IsUpload===1">

                                                                <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="Ritem.AnsBlob"
                                                                              @change="AnsBlobQID=Ritem.QuestionID,SaveFile('Audio',Ritem.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                            </div>
                                                        </div>
                                                        <!--Signature Capture-->
                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Signature Capture'" v-model="Ritem.Answer">
                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                <v-radio color="success" label="Signature Capture" hide-details>

                                                                </v-radio>
                                                                <v-radio color="success" label="Signature Upload" hide-details>

                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="Ritem.IsUpload===0">
                                                                <v-card outlined>
                                                                    <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                                </v-card>
                                                                <div>
                                                                    <v-btn color="secondary" depressed class="mt-2 mr-2" @click="saveSign(Ritem.QuestionID,false,0)">Save</v-btn>
                                                                    <v-btn default depressed class="mt-2 mr-2" @click="clearSignRepeat(false,Ritem.QuestionID,Ritem.RQstInd )">Clear</v-btn>
                                                                </div>
                                                            </div>
                                                            <div v-show="Ritem.IsUpload===1">

                                                                <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="Ritem.AnsBlob"
                                                                              @change="AnsBlobQID=Ritem.QuestionID,SaveFile('Signature',Ritem.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                            </div>
                                                            <v-card v-if="uploadedSign !=null">
                                                                <v-img :src=uploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                            </v-card>
                                                        </div>
                                                        <!--Cascading Dropdown-->
                                                        <div v-if="Ritem.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                            <v-row class="my-0 py-0">
                                                                <v-col class="my-0 py-0">
                                                                    <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem.CascadingList[ind].OptionID"
                                                                              :label="` ${optItem.OptionValue}`" outlined=""
                                                                              v-for="(optItem,ind) in RepeatSection[Ritem.RQstInd].CascadingQuestionOptions" :key="ind"
                                                                              :items="RepeatSection[Ritem.RQstInd].CascadingQuestionOptions[ind].CascadingList"
                                                                              item-text="Name"
                                                                              item-value="ID" @change="FilterSubItem(Ritem,optItem,optItem.CascadingList[ind].OptionID,false,0,false,Ritem.RQstInd)">
                                                                    </v-select>


                                                                </v-col>
                                                            </v-row>
                                                        </div>
                                                        <!--File Upload-->
                                                        <div v-if="Ritem.QuestionType==='File Upload'" class="mt-0">

                                                            <v-file-input label="Upload File" outlined dense accept="*" v-model="Ritem.AnsBlob"
                                                                          @change="AnsBlobQID=Ritem.QuestionID,SaveFileRepeat('File',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd)"></v-file-input>
                                                            <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                            <div v-if="uploadedFile !=null">
                                                                <v-img :src=Ritem.uploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                            </div>
                                                        </div>
                                                        <!-- Likert Scale-->
                                                        <div v-if="Ritem.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                            <v-radio-group class="my-0 py-0" v-model="Ritem.Answer">
                                                                <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                         :label="optItem.OptionValue"
                                                                         :value="optItem.OptionValue"></v-radio>
                                                            </v-radio-group>
                                                        </div>
                                                    </div>
                                                </div>
                                            </v-col>
                                        </v-row>
                                    </div>
                                    <v-row class="py-0 my-2">
                                        <v-col md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="UpdateBenefLinkedSectionQstnAnswers(BenefStepper),scrollToTop(lstSectionQstns)"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="PreviousBenefSectionQstnAnswers(),scrollToTop()"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- Finanl steps -->
                                <v-stepper-content step="6" class="pa-0">
                                    <v-row class="taskHeight ml-1">
                                        <v-col cols="12" md="12">
                                            <h4 class="text-left heading-3">Select final option from below to update the task</h4>
                                            <v-radio-group v-model="vmBenefSurveySubmitOption">
                                                <v-radio color="success" label="Add Another" value="1"></v-radio>
                                                <v-radio color="success" label="Mark as Complete" value="2"></v-radio>
                                                <v-radio color="success" label="Save & Exit" value="3"></v-radio>
                                            </v-radio-group>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="SubmitBenefSurvey()" v-bind:disabled="vmBenefSurveySubmitOption==0"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps=5,PreviousBenefSectionQstnAnswers(BenefStepper)"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- Import Survey Task--->
                                <v-stepper-content step="7" class="pa-0">
                                    <v-row class="pa-0">
                                        <v-col cols="12" md="12" class="py-0">
                                            <v-stepper v-model="ImportBeneficiaryTaskSteps" class="elevation-0 pa-0">
                                                <v-row justify="center" class="pa-0">
                                                    <v-col md="6" class="py-0">
                                                        <v-stepper-header class="elevation-0 py-0">
                                                            <v-stepper-step :complete="ImportBeneficiaryTaskSteps > 1" step="1" color="inherit">Upload </v-stepper-step>
                                                            <v-divider></v-divider>
                                                            <v-stepper-step :complete="ImportBeneficiaryTaskSteps > 2" step="2" color="inherit">Format Check</v-stepper-step>
                                                        </v-stepper-header>
                                                    </v-col>
                                                </v-row>
                                                <v-stepper-items>
                                                    <!-- Upload Survey Task Data-->
                                                    <v-stepper-content step="1" class="px-3 py-0">
                                                        <v-container fluid="" class="pa-0">
                                                            <v-row no-gutters="" class="uploadBoxHeight">
                                                                <v-col cols="12" md="12">
                                                                    <h4 class="text-center heading-3">Step1 : Uploads your CSV file set in our template</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="text-center">
                                                                    <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
                                                                        <v-card-text class="py-0">
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <v-avatar size="100" tile="">
                                                                                        <img src="../../images/Import-icon.svg" alt="Import" />
                                                                                    </v-avatar>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <p class="text-medium">
                                                                                        Your CSV file must map all our mandatory fields<br />
                                                                                        and your files cannot exceed more than 1000 rows
                                                                                    </p>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <v-btn class="downloadButton" icon @click="DownloadSampleBeneficiaryLinkedSurveyFile()"> <v-icon size="24" color="grey">mdi-file-download-outline</v-icon></v-btn>
                                                                                    <v-btn color="grey" outlined="" dark="" class="elevation-0" height="40" @click="DownloadSampleBeneficiaryLinkedSurveyFile()">
                                                                                        Download an example file
                                                                                    </v-btn>
                                                                                </v-col>
                                                                                <v-col align-center="" justify-center="">
                                                                                    <v-file-input label="Upload File" outlined dense
                                                                                                  id="file" v-model="surveyfileInput"
                                                                                                  prepend-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                  @change="OnSurveyFileUpload()">
                                                                                    </v-file-input>

                                                                                </v-col>

                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col align-center="" justify-center="" class="pt-0">
                                                                                    <v-chip close="" label="" color="white" v-if="csvSurveyFileName!=''">
                                                                                        <v-icon size="20" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                        {{ csvSurveyFileName}}
                                                                                    </v-chip>

                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row class="my-2 py-0">
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="ImportBeneficiaryTaskSteps=2" v-bind:disabled="lstImportSurvey.length==0"></Secondary-Button>
                                                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps=4"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-stepper-content>
                                                    <!-- Format Check-->
                                                    <v-stepper-content step="2" class="px-3 py-0">
                                                        <!-- Format Check-->
                                                        <v-row no-gutters class="uploadBoxHeight pa-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0">
                                                                <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                <form ref="form">
                                                                    <vue-excel-editor ref="grid" no-paging disable-panel-setting disable-panel-filter
                                                                                      v-model="lstImportSurvey">
                                                                        <div v-for="Hearder in lstImportSurveyQstnHearderN">
                                                                            <vue-excel-column :field="Hearder.text" :type="Hearder.type" :mandatory="Hearder.mandatory" :options="Hearder.options" :validate="onBeforeNameChange" width="80px" />
                                                                        </div>
                                                                    </vue-excel-editor>
                                                                </form>
                                                            </v-col>
                                                        </v-row>
                                                            <!--Footer Actions-->
                                                            <v-row class="my-2 py-0">
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                    <Default-Button-Outlined id="AutoTest_IP384" class="float-right btn-122x36" title="Previous" @click.native="CancelImport(),ImportBeneficiaryTaskSteps=1"></Default-Button-Outlined>
                                                                    <Secondary-Button id="AutoTest_IP385" class="mr-2 float-right btn-122x36" v-bind:disabled="IsvalidexcelData==false" title="Next" @click.native="ImportSurvey()"></Secondary-Button>
                                                                </v-col>
                                                            </v-row>
                                                    </v-stepper-content>
                                                </v-stepper-items>
                                            </v-stepper>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <!-- Add New beneficiary through register-->
                                <v-stepper-content step="8" class="pa-0">
                                    <v-stepper v-model="RegisterNewBenficiaryStep" class="elevation-0">
                                        <!-- Beneficiaty Registration Question-->
                                        <v-stepper-items>
                                            <v-stepper-content step="1" class="pt-0 px-0 py-0 my-0">
                                                <v-row class="v-scrolling-div taskHeight mx-1">
                                                    <v-col class="ml-2">
                                                        <div id="scrollToTop"></div>
                                                        <div v-for="(item,index) in lstBenefRegQstns">
                                                            <label v-bind:id="item.QuestionID.toString()" class="text-left heading-3 mb-2">{{item.QuestionName}}</label>
                                                            <sub v-if="item.IsMandatoryResponse==true" style="color:red;font-size:large"> *</sub>
                                                            <!-- Question Hint-->
                                                            <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{ item.QuestionHint }}</sub>
                                                            <!-- error text-->
                                                            <sub style="color:red;font-size:smaller" v-if="(item.IsMandatoryResponse==true && item.Answer ==='')|| item.IsValidationError ">{{ item.ValidationErrorTest }}</sub>

                                                            <div v-if="item.QuestionType=='Single Line Text' && item.IsValidationQuestion == 1">
                                                                <input type="text"  class="custom-text-field" solo :value="item.Answer" @change="fnBindValue($event, index)"/>
                                                            </div>
                                                            <div v-if="item.QuestionType=='Single Line Text' && item.IsValidationQuestion == 0">
                                                                <input type="text" class="custom-text-field" v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type Your answer" :value="item.Answer" @change="fnBindValue($event, index)">

                                                            </div>
                                                            <!--Multiline Answer-->
                                                            <textarea class="custom-text-field" full-width outlined dense v-if="item.QuestionType==='Multi Line Text'" label="Type your answer" :value="item.Answer" @change="fnBindValue($event, index)" ></textarea>
                                                            <!--Numeric-->
                                                            <div v-if="item.QuestionType==='Numeric'">
                                                                <input type="number" class="custom-text-field" full-width outlined dense label="Type your answer" :value="item.Answer" @change="fnBindValue($event, index)">
                                                            </div>
                                                            <!--Single Choice -->
                                                            <div v-if="item.QuestionType==='Single Choice'" class="mt-0">

                                                                <v-radio-group class="my-0 py-0" v-model="item.Answer">
                                                                    <v-radio color="success" hide-details v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                             :label="optItem.OptionValue"
                                                                             :value="optItem.OptionValue"></v-radio>
                                                                </v-radio-group>

                                                            </div>
                                                            <!--Multiple Choice -->
                                                            <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                                <div class="my-0 py-0" v-if="item.BTQstnAnsOptionList.length>0">
                                                                    <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                v-model="optItem.IsOptSelected" @change="GetMultiChoice_BT(optItem,optItem.IsOptSelected,optItem.OptionValue)"></v-checkbox>

                                                                </div>
                                                            </div>
                                                            <!--Dropdown -->
                                                            <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                    <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                                              item-text="OptionValue"
                                                                              label="Select" outlined hide-details v-model="item.Answer">
                                                                    </v-select>
                                                                </div>
                                                            </div>
                                                            <!--Cascading Dropdown-->
                                                            <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0 py-0 my-0">
                                                                <v-row class="my-0 py-0">
                                                                    <v-col class="my-0 py-0">
                                                                        <v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                  :label="` ${optItem.OptionValue}`" outlined=""
                                                                                  v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                                  :items="optItem.CascadingList"
                                                                                  item-text="Name"
                                                                                  item-value="ID" @change="FilterSubItem(item,optItem,optItem[ind],true,1,false,-1)">
                                                                        </v-select>


                                                                    </v-col>
                                                                </v-row>
                                                            </div>
                                                            <!--Date Picker-->
                                                            <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                                <v-menu v-model="item.mBTDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="item.Answer"
                                                                                      prepend-inner-icon="mdi-calendar"
                                                                                      outlined
                                                                                      readonly
                                                                                      @click:prepend-inner="item.mBTDate = true"
                                                                                      dense
                                                                                      v-on="on" hide-details></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="item.Answer" no-title @input="item.mBTDate = false" format="DD-MM-YYYY" @change="onFocusBeneficiaryQstnValidation(item)"></v-date-picker>
                                                                </v-menu>
                                                            </div>
                                                            <!--GPS Capture-->
                                                            <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model="item.Answer">
                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                <GmapMap :center="Coordinates"
                                                                         :zoom="13"
                                                                         style="width: 100%; height: 300px" @click.native="GetGPSCoordinates(item.QuestionID,true)">
                                                                    <GmapMarker :position="Coordinates"
                                                                                :clickable="true"
                                                                                :draggable="true" />
                                                                </GmapMap>
                                                            </div>
                                                            <!--Photo Capture-->
                                                            <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">

                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                    <v-radio color="success" label="Photo Capture" hide-details>
                                                                    </v-radio>
                                                                    <v-radio color="success" label="Photo Upload" hide-details>
                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="item.IsUpload===0">
                                                                    <v-row>
                                                                        <v-col md="12" class="my-0 py-0">

                                                                            <v-btn @click="startCamera =true,camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                            <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item.QuestionID,true,0),startCamera =false,camOn=false" v-bind:disabled="!camOn">Capture</v-btn>
                                                                            <v-btn @click="startCamera =false,camOn=false,onStop()" v-bind:disabled="!camOn" depressed>Stop</v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row>
                                                                        <v-col md="8" v-if="startCamera">
                                                                            <vue-web-cam ref="webcam"
                                                                                         :device-id="deviceId"
                                                                                         width="100%"
                                                                                         height="200px"
                                                                                         @started="onStarted"
                                                                                         @stopped="onStopped"
                                                                                         @error="onError"
                                                                                         @cameras="onCameras1"
                                                                                         @camera-change="onCameraChange1" />
                                                                            <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,true,0)">Capture</v-btn>-->

                                                                        </v-col>
                                                                        <v-col md="4" v-if="img1 !=null">
                                                                            <figure class="figure">
                                                                                <img :src="img1" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                            </figure>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>

                                                                <div v-show="item.IsUpload===1">
                                                                    <v-row>
                                                                        <v-col md="4">

                                                                            <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.BTAnsBlob"
                                                                                          @change="BTAnsBlobQID=item.QuestionID,SaveFile('Photo',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                        </v-col>
                                                                        <v-col md="4" v-if="benefUploadedImg !=null">
                                                                            <v-img :src=benefUploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>

                                                            </div>
                                                            <!--Audio Capture-->
                                                            <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                    <v-radio color="success" label="Audio Capture" hide-details>
                                                                    </v-radio>
                                                                    <v-radio color="success" label="Audio Upload" hide-details>

                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="item.IsUpload===0">
                                                                    <audio-recorder upload-url="YOUR_API_URL" id="recAudiobenef"
                                                                                    :play="playSound"
                                                                                    :attempts="1"
                                                                                    :time="1"
                                                                                    :headers="headers"
                                                                                    :before-recording="callback"
                                                                                    :pause-recording="callback"
                                                                                    :after-recording="SaveAudioBT"
                                                                                    :before-upload="callback"
                                                                                    :successful-upload="callback"
                                                                                    :failed-upload="callback"
                                                                                    v-model="item.Answer"
                                                                                    @click.native="BTAnsBlobQID=item.QuestionID" />

                                                                </div>
                                                                <div v-show="item.IsUpload===1">

                                                                    <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.BTAnsBlob"
                                                                                  @change="BTAnsBlobQID=item.QuestionID,SaveFile('Audio',item.BTAnsBlob,true,true,0)"></v-file-input>

                                                                </div>
                                                            </div>
                                                            <!--Signature Capture-->
                                                            <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                    <v-radio color="success" label="Signature Capture" hide-details>

                                                                    </v-radio>
                                                                    <v-radio color="success" label="Signature Upload" hide-details>

                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="item.IsUpload===0">
                                                                    <v-card outlined>
                                                                        <VueSignaturePad width="100%" height="300px" ref="signaturePad1" />
                                                                    </v-card>
                                                                    <div>
                                                                        <v-btn color="secondary" depressed class="mt-2 mr-2" @click="saveSign(item.QuestionID,true,0)">Save</v-btn>
                                                                        <v-btn default depressed class="mt-2 mr-2" @click="clearSign(true,item.QuestionID)">Clear</v-btn>
                                                                    </div>
                                                                </div>
                                                                <div v-show="item.IsUpload===1">

                                                                    <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.BTAnsBlob"
                                                                                  @change="BTAnsBlobQID=item.QuestionID,SaveFile('Signature',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                </div>
                                                                <v-card v-if="benefUploadedSign !=null">
                                                                    <v-img :src=benefUploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                </v-card>
                                                            </div>
                                                            <!--Note-->
                                                            <div v-if="item.QuestionType==='Note'">
                                                                <!-- <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea> -->

                                                                <textarea class="custom-text-field" outlined dense hide-details v-model="item.Answer" ></textarea>
                                                            </div>
                                                            <!--File Upload-->
                                                            <div v-if="item.QuestionType==='File Upload'">

                                                                <v-file-input label="Upload File" outlined dense accept="*" v-model="item.BTAnsBlob"
                                                                              @change="BTAnsBlobQID=item.QuestionID,SaveFile('File',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                <div v-if="benefUploadedFile !=null">
                                                                    <v-img :src=benefUploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </v-col>
                                                </v-row>
                                                <v-row class="py-0 my-2">
                                                    <v-col cols="12" md="12" class="text-center text-md-right py-0 my-0">
                                                        <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="RegisterNewBenficiary(),scrollToTop(lstBenefRegQstns)"></Secondary-Button>
                                                        <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps=3,scrollToTop()"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper-content>
                                            <!-- Successfull Message-->
                                            <v-stepper-content step="2" class="pt-0">
                                                <v-container fluid="" class="pt-0">
                                                    <v-row no-gutters="">
                                                        <v-col cols="12" md="12" class="pt-0 mb-2">
                                                            <h4 class="text-center heading-3">Super !</h4>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <v-avatar size="150" tile="">
                                                                <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                            </v-avatar>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <p class="text-medium">
                                                                Your Beneficiary has been created successfully.
                                                            </p>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <v-btn color="secondary" class="mr-2 elevation-0" @click="GetBenefSurveyQstns(),BenefLinkedSurveySteps =4">Done</v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-stepper-content>
                                        </v-stepper-items>
                                    </v-stepper>

                                </v-stepper-content>
                                <!-- Add new Beneficiary Import and register-->
                                <v-stepper-content step="9" class="pa-0">
                                    <v-row class="pa-0">
                                        <v-col cols="12" md="12" class="py-0">
                                            <v-stepper v-model="ImportNewBeneficiarySteps" class="elevation-0 pa-0">
                                                <v-row justify="center" class="pa-0">
                                                    <v-col md="6" class="py-0">
                                                        <v-stepper-header class="elevation-0 pa-0">
                                                            <v-stepper-step :complete="ImportNewBeneficiarySteps > 1" step="1" color="inherit">Upload </v-stepper-step>
                                                            <v-divider></v-divider>
                                                            <v-stepper-step :complete="ImportNewBeneficiarySteps > 2" step="2" color="inherit">Format Check</v-stepper-step>
                                                        </v-stepper-header>
                                                    </v-col>
                                                </v-row>
                                                <v-stepper-items>
                                                    <v-stepper-content step="1" class="px-3 py-0">
                                                        <v-container fluid="" class="pa-0">
                                                            <v-row no-gutters="" class="uploadBoxHeight">
                                                                <v-col cols="12" md="12">
                                                                    <h4 class="text-center heading-3">Step1 : Upload your CSV file set in our template</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="text-center">
                                                                    <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
                                                                        <v-card-text class="py-0">
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <v-avatar size="100" tile="">
                                                                                        <img src="../../images/Import-icon.svg" alt="Import" />
                                                                                    </v-avatar>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <p class="text-medium">
                                                                                        Your CSV file must map all our mandatory fields<br />
                                                                                        and your files cannot exceed more than 1000 rows
                                                                                    </p>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <v-btn class="downloadButton" icon @click="DownloadSampleBenefRegFile()"> <v-icon size="24" color="grey">mdi-file-download-outline</v-icon></v-btn>
                                                                                    <v-btn color="grey" outlined="" dark="" class="elevation-0" height="40" @click="DownloadSampleBenefRegFile()">
                                                                                       Download an example file
                                                                                    </v-btn>
                                                                                </v-col>
                                                                                <v-col align-center="" justify-center="">
                                                                                    <v-file-input label="Upload File" outlined dense
                                                                                                  id="file" v-model="regfileInput"
                                                                                                  prepend-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                  @change="OnRegFileUpload">
                                                                                    </v-file-input>
                                                                                </v-col>

                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col align-center="" justify-center="" class="pt-0">
                                                                                    <v-chip close="" label="" color="white" v-if="csvFileName!=''">
                                                                                        <v-icon size="20" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                        {{ csvFileName}}
                                                                                    </v-chip>

                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row class="my-2 py-0">
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="ImportNewBeneficiarySteps=2"></Secondary-Button>
                                                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="BenefLinkedSurveySteps=3"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-stepper-content>
                                                    <!-- Format Check-->
                                                    <v-stepper-content step="2" class="px-3 py-0">
                                                        <v-row no-gutters class="uploadBoxHeight pa-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0">
                                                                <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                <vue-excel-editor no-paging :resetOnClick="resetOnClick" disable-panel-setting disable-panel-filter
                                                                                  v-model="lstImportBeneficiary">
                                                                    <div v-for="Hearder in lstImportBeneficiaryHearder">
                                                                        <vue-excel-column :field="Hearder.text" :type="Hearder.type" :mandatory="Hearder.mandatory" :options="Hearder.options" :validate="onBeforeMandatoryChange" width="80px" />
                                                                    </div>
                                                                </vue-excel-editor>
                                                            </v-col>
                                                        </v-row>
                                                        <!--Footer Actions-->
                                                        <v-row class="my-2 py-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                <Default-Button-Outlined id="AutoTest_IP384" class="float-right btn-122x36" title="Previous" @click.native="CancelImport(),ImportNewBeneficiarySteps=1"></Default-Button-Outlined>
                                                                <Secondary-Button id="AutoTest_IP385" class="mr-2 float-right btn-122x36" v-bind:disabled="IsvalidexcelData==false" title="Next" @click.native="ImportRegisterBeneficiary()"></Secondary-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-stepper-content>
                                                    <!-- Final Updated Message-->
                                                    <v-stepper-content step="3" class="pa-0">
                                                        <v-container fluid="" class="pt-0">
                                                            <v-row no-gutters="">
                                                                <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                    <h4 class="text-center heading-3">Super !</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                    <v-avatar size="150" tile="">
                                                                        <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                    </v-avatar>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                    <p class="text-medium">
                                                                        Your Beneficiary has been created successfully.
                                                                    </p>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="pt-0 text-center">
                                                                    <v-btn color="secondary" class="mr-2 elevation-0" @click="BenefLinkedSurveySteps = 4">Done</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-stepper-content>
                                                </v-stepper-items>
                                            </v-stepper>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                            </v-stepper-items>
                        </v-stepper>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsInputFinished == true" class="px-3 py-1">
                        <div class="taskHeight">
                            <v-row>
                                <v-col cols="12" md="12" class="pt-0">
                                    <h4 class="text-left page-head">In Case you have any comments please specify them below</h4>
                                </v-col>
                            </v-row>
                            <v-row>

                                <v-col>
                                    <h4 class="text-left heading-3-dark mb-2">Add comments</h4>
                                    <!-- <v-textarea rows="5" outlined dense label="Description" v-model="vmUpdateComment" type="text">
                                    </v-textarea> -->
                                    <textarea class="custom-text-field" rows="5" outlined dense label="Description" :value="vmUpdateComment" @change="v=> vmUpdateComment = v.value"></textarea>
                                    <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" v-model="fileInput" prepend-icon="mdi-paperclip" outlined="" dense=""
                                                  id="file" accept="*"></v-file-input>
                                </v-col>
                            </v-row>
                        </div>
                        <v-row class="py-0 my-2">
                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="fnTaskUpdate()"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsInputFinished = false,IsUpdate_BenSurvey = true"></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!-- Survey non beneficiary linked data collector-->
            <v-dialog v-model="dlgNonBeneficarySurveyType" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="ClearBenefSurveyUpdate()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                            </v-avatar>
                            {{lstSelectedTask.SurveyName}}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                            <v-chip class="ml-2 task-survey-chip" label dark small>
                                Survey
                            </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2" align="right" class="d-none d-sm-inline">
                            <v-btn text small @click="ClearBenefSurveyUpdate()">
                                <v-icon color="#707070" size="20">mdi-close</v-icon>
                            </v-btn>
                        </v-col>
                    </v-row>
                    <!--<v-card-title>
                        <h5 class="page-head mr-4"></h5>
                        <v-spacer></v-spacer>
                    </v-card-title>-->
                    <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                    <v-card-text class="px-3 py-3">
                        <v-stepper v-model="NonBenefLinkedSurveySteps" class="elevation-0 pa-0">
                            <v-stepper-items class="pa-0">
                                <v-stepper-content step="1" class="pa-0">
                                    <v-tabs grow v-model="ActivityTabs" class="pt-0 elevation-0 taskHeight" color="#52B962">
                                        <v-tab href='#tab-1'>Task Details</v-tab>
                                        <v-tab href='#tab-2'>History</v-tab>
                                        <!-- Task Details-->
                                        <v-tab-item value="tab-1">
                                            <v-row justify="center" class="mt-4">
                                                <v-col md="12" class="v-scrolling-div px-6 historyHeight">
                                                    <v-row>
                                                        <v-col cols="12" sm="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Task Name
                                                            </h4>
                                                            <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.SurveyName}}</h4>
                                                        </v-col>
                                                        <v-col cols="12" sm="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Action
                                                            </h4>
                                                            <v-chip color="#2C4258" label dark small>
                                                                To DataCollect
                                                            </v-chip>
                                                        </v-col>
                                                    </v-row>
                                                    <v-row>
                                                        <v-col cols="12" sm="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Date
                                                            </h4>
                                                            <h4 class="font-weight-regular heading-3">{{ new Date(lstSelectedTask.TaskDate).toLocaleDateString('en-GB')}}</h4>
                                                        </v-col>
                                                        <v-col cols="12" sm="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Project Name
                                                            </h4>
                                                            <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.ProjectName}}</h4>
                                                        </v-col>
                                                    </v-row>
                                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                                        Task Description
                                                    </h4>
                                                    <h4 class="font-weight-regular heading-3">
                                                        {{lstSelectedTask.Description}}
                                                    </h4>
                                                    <v-row>
                                                        <!--<v-col md="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Beneficiary Type
                                                            </h4>
                                                            <h4 class="font-weight-regular heading-3"> {{lstSelectedTask.BeneficiaryType}}</h4>
                                                        </v-col>-->
                                                        <v-col cols="12" sm="6">
                                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                                Status
                                                            </h4>
                                                            <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                                <v-icon size="10" color="##FCC735">mdi-arrow-right</v-icon>
                                                            </v-avatar>
                                                            Pending
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                        </v-tab-item>
                                        <!-- Task History-->
                                        <v-tab-item value="tab-2">
                                            <v-row justify="center">
                                                <v-col>
                                                    <v-card flat class="py-0">
                                                        <v-card-text class="v-scrolling-div historyHeight">
                                                            <v-list dense class="py-0">
                                                                <template v-for="(item, index) in lstLogFrame">
                                                                    <v-list-item class="py-0">
                                                                        <v-list-item-icon>
                                                                            <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>
                                                                        <v-list-item-content>
                                                                            <v-list-item-title class="py-0 pb-0">
                                                                                <v-row class="d-lg-inline-flex py-0 pb-0">
                                                                                    <v-col class="px-0 pl-3 py-0">
                                                                                        <h4 class="font-weight-large   py-0 pb-0 px-0">{{item.Name}}</h4>
                                                                                    </v-col>
                                                                                    <v-col class="ml-3 px-0 py-0">
                                                                                        <h4 class="font-weight-regular  py-0 pb-0">{{item.Message}}</h4>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-list-item-title>
                                                                            <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                            </v-list-item-subtitle>
                                                                            <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                <h4 class="font-weight-regular py-0 pb-0 px-0" style="height:auto;max-height:100px;overflow-y:auto">{{item.TaskUpdateComment}}</h4>
                                                                            </v-list-item-subtitle>
                                                                            <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                <!--<v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">-->
                                                                                <!--<v-chip v-show="item.AttachedDocumentName.length >0 " small color="red" class="mb-0">
                                                                                    {{item.AttachedDocumentName}}
                                                                                </v-chip>-->
                                                                                <!--<v-list-item-icon v-show="item.AttachedDocumentName.length >0 " @click="DownloadDocument(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                    <v-icon>mdi-file-download-outline</v-icon>
                                                                                </v-list-item-icon>-->
                                                                                <v-row>
                                                                                    <!--<v-col md="8">
                                                                                        <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                    </v-col>-->
                                                                                    <v-col md="8">
                                                                                        <v-btn v-show="item.AttachedDocumentName.length >0 " color="red" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDocument(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                            <v-icon size="26" color="red">mdi-file-download-outline</v-icon>{{item.AttachedDocumentName}}
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <!--<v-list-item-content id="AutoTest_IP422" @click="DownloadDocument(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">Download</v-list-item-content>-->
                                                                                <!--</v-chip-group>-->
                                                                            </v-list-item-subtitle>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </template>
                                                            </v-list>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-col>
                                            </v-row>
                                        </v-tab-item>
                                    </v-tabs>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="NonBenefLinkedSurveySteps = 2"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="ClearBenefSurveyUpdate()"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <v-stepper-content step="2" class="pa-0">
                                    <v-row class="taskHeight ml-1">
                                        <v-col cols="12" md="12">
                                            <h4 class="text-left heading-3 ">Select how you want to update the Task</h4>
                                            <v-radio-group v-model="vmNonBenefSurveyChoice">
                                                <v-radio color="success" label="Import" value="1"></v-radio>
                                                <v-radio color="success" label="Update Manually" value="2"></v-radio>
                                            </v-radio-group>
                                        </v-col>
                                    </v-row>
                                    <v-row class="py-0 my-2">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0" v-if="vmNonBenefSurveyChoice == 2">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetBenefSurveyQstns(),NonBenefLinkedSurveySteps=3,IsUpdate_BenSurvey = true" v-bind:disabled="vmNonBenefSurveyChoice==0"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="NonBenefLinkedSurveySteps = 1"></Default-Button-Outlined>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0" v-else-if="vmNonBenefSurveyChoice == 1">
                                            <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetBenefSurveyQstns(),NonBenefLinkedSurveySteps = 4,ImportBeneficiaryTaskSteps=1,IsUpdate_BenSurvey = false" v-bind:disabled="vmNonBenefSurveyChoice==0"></Secondary-Button>
                                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="NonBenefLinkedSurveySteps = 1"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <v-stepper-content step="4" class="pa-0">
                                    <v-row class="pa-0">
                                        <v-col cols="12" md="12" class="py-0">
                                            <v-stepper v-model="ImportBeneficiaryTaskSteps" class="elevation-0 pa-0">
                                                <v-row justify="center" class="pa-0">
                                                    <v-col md="6" class="py-0">
                                                        <v-stepper-header class="elevation-0 py-0">
                                                            <v-stepper-step :complete="ImportBeneficiaryTaskSteps > 1" step="1" color="inherit">Upload </v-stepper-step>
                                                            <v-divider></v-divider>
                                                            <v-stepper-step :complete="ImportBeneficiaryTaskSteps > 2" step="2" color="inherit">Format Check</v-stepper-step>
                                                        </v-stepper-header>
                                                    </v-col>
                                                </v-row>
                                                <v-stepper-items>
                                                    <!-- Upload Survey Task Data-->
                                                    <v-stepper-content step="1" class="px-3 py-0">
                                                        <v-container fluid="" class="pa-0">
                                                            <v-row no-gutters="" class="uploadBoxHeight">
                                                                <v-col cols="12" md="12">
                                                                    <h4 class="text-center heading-3">Step1 : Uploads your CSV file set in our template</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="text-center">
                                                                    <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
                                                                        <v-card-text class="py-0">
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <v-avatar size="100" tile="">
                                                                                        <img src="../../images/Import-icon.svg" alt="Import" />
                                                                                    </v-avatar>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row no-gutters="">
                                                                                <v-col>
                                                                                    <p class="text-medium">
                                                                                        Your CSV file must map all our mandatory fields<br />
                                                                                        and your files cannot exceed more than 1000 rows
                                                                                    </p>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <v-btn class="downloadButton" icon @click="DownloadSampleNonBeneficiaryLinkedSurveyFile()"> <v-icon size="24" color="grey">mdi-file-download-outline</v-icon></v-btn>
                                                                                    <v-btn color="grey" outlined="" dark="" class="elevation-0" height="40" @click="DownloadSampleNonBeneficiaryLinkedSurveyFile()">
                                                                                       Download an example file
                                                                                    </v-btn>
                                                                                </v-col>
                                                                                <v-col align-center="" justify-center="">
                                                                                    <v-file-input label="Upload File" outlined dense
                                                                                                  id="file" v-model="surveyfileInput"
                                                                                                  prepend-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                  @change="OnSurveyFileUpload">
                                                                                    </v-file-input>

                                                                                </v-col>

                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col align-center="" justify-center="" class="pt-0">
                                                                                    <v-chip close="" label="" color="white" v-if="csvSurveyFileName!=''">
                                                                                        <v-icon size="20" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                        {{ csvSurveyFileName}}
                                                                                    </v-chip>

                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row class="my-2 py-0">
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="ImportBeneficiaryTaskSteps=2" v-bind:disabled="lstImportSurvey.length==0"></Secondary-Button>
                                                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="NonBenefLinkedSurveySteps = 2"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-stepper-content>
                                                    <!-- Format Check-->
                                                    <v-stepper-content step="2" class="px-3 py-0">
                                                        <!-- Format Check-->
                                                        <v-row no-gutters class="uploadBoxHeight pa-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0">
                                                                <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                <vue-excel-editor no-paging :resetOnClick="resetOnClick" disable-panel-setting disable-panel-filter
                                                                                  v-model="lstImportSurvey">
                                                                    <div v-for="Hearder in lstImportSurveyQstnHearderN">
                                                                        <vue-excel-column :field="Hearder.text" :type="Hearder.type" :mandatory="Hearder.mandatory" :options="Hearder.options" :validate="onBeforeNameChange" width="80px" />
                                                                    </div>
                                                                </vue-excel-editor>
                                                            </v-col>
                                                        </v-row>
                                                        <!--Footer Actions-->
                                                        <v-row class="my-2 py-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                                <Default-Button-Outlined id="AutoTest_IP384" class="float-right btn-122x36" title="Previous" @click.native="CancelImport(),ImportBeneficiaryTaskSteps=1"></Default-Button-Outlined>
                                                                <Secondary-Button id="AutoTest_IP385" class="mr-2 float-right btn-122x36" title="Next" @click.native="ImportSurvey(), IsUpdate_BenSurvey = false ,IsInputFinished = true"></Secondary-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-stepper-content>
                                                </v-stepper-items>
                                            </v-stepper>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <v-stepper-content step="3" class="pa-0">
                                    <v-card-text class="px-0 py-0" v-show="IsUpdate_BenSurvey == true && IsInputFinished == false">

                                        <div class="pt-0 taskHeight">
                                            <v-row class="py-0">
                                                <!--<v-row v-for="(benefItem,Index) in vmSelectedBeneficiaries" :key="benefItem.BeneficiaryID">-->
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <!--<div v-if="BenefStepper== Index+1 ">-->
                                                    <h4 class="text-center page-head mt-3">Section {{BenefQstnStepper[BenefStepper]}} of {{lstSections.length}}</h4>
                                                    <!--</div>-->
                                                </v-col>
                                            </v-row>
                                            <v-row class="py-0">
                                                <v-col cols="6" md="8">
                                                    <h4 class="text-left page-head mb-2">{{vmSection.SectionName}}</h4>
                                                </v-col>
                                                <v-spacer class="d-sm-none"></v-spacer>
                                                <v-col cols="6" md="4">
                                                    <v-select :items="lstSections"
                                                              item-text="SectionName"
                                                              item-value="SectionID" return-object=""
                                                              hide-details dense="" outlined item-disabled="customDisabled"
                                                              v-model="vmSection" @input="GetSectionQuestions1($event,BenefStepper,0)">
                                                    </v-select>
                                                </v-col>
                                            </v-row>
                                            <v-row class="v-scrolling-div scrollBoxHeight mx-1"> 
                                                <v-col class="ml-2">
                                                    <div id="scrollToTop"></div>
                                                    <div v-for="(item,QIndex) in lstSectionQstns" :key="QIndex" class="mb-4">

                                                        <label v-bind:id="item.QuestionID.toString()" class="text-left heading-3 mb-2" v-if="item.QuestionID==0"><b>{{item.QuestionName}}</b></label>
                                                        <label v-bind:id="item.QuestionID.toString()" class="text-left heading-3 mb-2" v-if="item.QuestionID>0">{{item.QuestionName}}</label>
                                                        <sub v-if="item.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                        <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{item.QuestionHint}}</sub>
                                                        <!-- Code updated for Survey Cleanup 09-02-2021  IsValidationError-->
                                                        <!--<sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| chkQstnValid ">{{item.ErrorText}}</sub>-->

                                                        <sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| item.IsValidationError ">{{item.ValidationErrorTest}}</sub>
                                                        
                                                        <!-- Single line text with validation condition -->
                                                        <div v-if="item.QuestionType=='Single Line Text' && (item.IsValidationQuestion || item.IsConditionQuestion) ">
                                                            <v-text-field v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                                          :value="item.DefaultValue" :min="item.minVal" :max="item.maxVal" :type="item.qType" v-bind:maxlength="item.textLength" :rules="item.qRules"
                                                                          @focus="onFocusCheckQstnValidation(item)" @change="AddSectionLogic(item.QuestionID,1)" />

                                                        </div>
                                                        <!-- Single line text without validation condition-->
                                                        <div v-if="item.QuestionType=='Single Line Text' && !(item.IsValidationQuestion || item.IsConditionQuestion) ">
                                                            <v-text-field v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                                          :value="item.DefaultValue" @change="CheckQstnValidation(item)" />

                                                        </div>

                                                        <!--Multiline Answer-->
                                                        <v-textarea v-if="item.QuestionType==='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer" />
                                                        <!--Numeric-->
                                                        <v-flex xs12 sm12 md12>
                                                            <div v-if="item.QuestionType==='Numeric'">
                                                                <v-layout row>
                                                                    <v-flex grow>
                                                                        <v-text-field v-if="item.QuestionType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="item.Answer"
                                                                                      class="ml-3"
                                                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                      onKeyPress="if(this.value.length==10) return false;" :readonly="item.IsFormulaQuestion == true"
                                                                                      :value="item.DefaultValue" :min="item.minVal" :max="item.maxVal"
                                                                                      @focus="Calculate(item.QuestionID,BenefStepper), onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item),AddSectionLogic(item.QuestionID,1)" :rules="item.qRules">
                                                                        </v-text-field>
                                                                    </v-flex>
                                                                    <v-flex shrink>
                                                                        <!--<v-text-field  class="mr-2 ml-2" v-if="item.QuestionType==='Numeric'" :placeholder="item.UOM" outlined="" dense="" disabled=""></v-text-field>-->
                                                                        <sub class="mr-2 ml-2" v-if="item.QuestionType==='Numeric'" style="color:green;font-size:large">{{item.UOM}}</sub>
                                                                    </v-flex>
                                                                </v-layout>
                                                            </div>
                                                        </v-flex>
                                                        <!--Single Choice Answer-->
                                                        <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                            <v-radio-group class="my-0 py-0" v-model="item.Answer" @change="AddSectionLogic(item.QuestionID,1)">
                                                                <v-radio color="success" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                         :label="optItem.OptionValue"
                                                                         :value="optItem.OptionValue"></v-radio>
                                                            </v-radio-group>
                                                        </div>
                                                        <!--Multiple Choice -->
                                                        <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                            <div class="my-0 py-0" v-if="item.QuestionOptions.length>0">
                                                                <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                            v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,BenefStepper),AddSectionLogic(item.QuestionID,1)"></v-checkbox>

                                                            </div>
                                                        </div>
                                                        <!--Dropdown -->
                                                        <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                            <div v-if="item.QuestionOptions.length>0">
                                                                <v-select dense :items="item.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                          item-text="OptionValue"
                                                                          label="Select" outlined hide-details v-model="item.Answer" @change="AddSectionLogic(item.QuestionID,1)">
                                                                </v-select>
                                                            </div>
                                                        </div>
                                                        <!--Date Picker-->
                                                        <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                            <v-menu v-model="item.DateMenu"
                                                                    :close-on-content-click="false"
                                                                    :nudge-right="40"
                                                                    transition="scale-transition"
                                                                    offset-y
                                                                    min-width="290px">
                                                                <template v-slot:activator="{ on }">
                                                                    <v-text-field v-model="item.Answer"
                                                                                  prepend-inner-icon="mdi-calendar"
                                                                                  outlined
                                                                                  readonly
                                                                                  dense
                                                                                  @click:prepend-inner="item.DateMenu = true"
                                                                                  v-on="on" hide-details></v-text-field>
                                                                </template>
                                                                <v-date-picker v-model="item.Answer" no-title @input="item.DateMenu = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(item)"></v-date-picker>
                                                            </v-menu>
                                                        </div>
                                                        <!--GPS Capture-->
                                                        <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model="item.Answer">
                                                            <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                            <GmapMap :center="Coordinates"
                                                                     :zoom="13"
                                                                     style="width: 100%; height: 200px" @click.native="GetGPSCoordinates(item.QuestionID,false)">
                                                                <GmapMarker :position="Coordinates"
                                                                            :clickable="true"
                                                                            :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                            </GmapMap>
                                                        </div>
                                                        <!--Note-->
                                                        <div class="mb-6" v-if="item.QuestionType==='Note'">
                                                            <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea>
                                                        </div>
                                                        <!--Photo Capture-->
                                                        <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">

                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                <v-radio color="success" label="Photo Capture" hide-details>
                                                                </v-radio>
                                                                <v-radio color="success" label="Photo Upload" hide-details>
                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="item.IsUpload===0">
                                                                <v-row>
                                                                    <v-col md="12">

                                                                        <v-btn @click="item.startCamera =true,item.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                        <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item.QuestionID,false,0),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                        <v-btn @click="item.startCamera =false,item.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col md="8" v-if="item.startCamera">
                                                                        <vue-web-cam ref="webcam"
                                                                                     :device-id="deviceId"
                                                                                     width="100%"
                                                                                     height="200px"
                                                                                     @started="onStarted"
                                                                                     @stopped="onStopped"
                                                                                     @error="onError"
                                                                                     @cameras="onCameras"
                                                                                     @camera-change="onCameraChange" />
                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                    </v-col>

                                                                    <v-col md="4" v-if="item.img !=null && item.img != ''">
                                                                        <figure class="figure">
                                                                            <img :src="item.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                        </figure>
                                                                    </v-col>

                                                                </v-row>
                                                            </div>
                                                            <div v-show="item.IsUpload===1">
                                                                <v-row>
                                                                    <v-col md="4">

                                                                        <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                                      @change="AnsBlobQID=item.QuestionID,SaveFile('Photo',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                    </v-col>
                                                                    <v-col md="4" v-if="item.uploadedImg !=null">
                                                                        <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                    </v-col>
                                                                </v-row>
                                                            </div>

                                                        </div>
                                                        <!--Audio Capture-->
                                                        <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                <v-radio color="success" label="Audio Capture" hide-details>
                                                                </v-radio>
                                                                <v-radio color="success" label="Audio Upload" hide-details>

                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="item.IsUpload===0">
                                                                <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                :play="playSound"
                                                                                :attempts="1"
                                                                                :time="1"
                                                                                :headers="headers"
                                                                                :before-recording="callback"
                                                                                :pause-recording="callback"
                                                                                :after-recording="SaveAudio"
                                                                                :before-upload="callback"
                                                                                :successful-upload="callback"
                                                                                :failed-upload="callback"
                                                                                v-model="item.Answer"
                                                                                @click.native="AnsBlobQID=item.QuestionID" />

                                                            </div>
                                                            <div v-show="item.IsUpload===1">

                                                                <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                                              @change="AnsBlobQID=item.QuestionID,SaveFile('Audio',item.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                            </div>
                                                        </div>
                                                        <!--Signature Capture-->
                                                        <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                <v-radio color="success" label="Signature Capture" hide-details>

                                                                </v-radio>
                                                                <v-radio color="success" label="Signature Upload" hide-details>

                                                                </v-radio>
                                                            </v-radio-group>
                                                            <div v-show="item.IsUpload===0">
                                                                <v-card outlined>
                                                                    <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                                </v-card>
                                                                <div>
                                                                    <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=item.signaturebutton @click="saveSign(item.QuestionID,false,0),item.signaturebutton=true">Save</v-btn>
                                                                    <v-btn default depressed class="mt-2 mr-2" @click="clearSign(false,item.QuestionID),item.signaturebutton=false">Clear</v-btn>
                                                                </div>
                                                            </div>
                                                            <div v-show="item.IsUpload===1">

                                                                <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                              @change="AnsBlobQID=item.QuestionID,SaveFile('Signature',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                            </div>
                                                            <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                                                <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                <!--max-width="100%" height="200" aspect-ratio="1.7"-->
                                                                <!--<v-img :src=uploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>-->
                                                            </v-card>
                                                        </div>
                                                        <!--Cascading Dropdown-->
                                                        <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                            <v-row class="my-0 py-0">
                                                                <v-col class="my-0 py-0">
                                                                    <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                              :label="` ${optItem.OptionValue}`" outlined=""
                                                                              v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                              :items="optItem.CascadingList"
                                                                              item-text="Name"
                                                                              item-value="ID" @change="FilterSubItem(item,optItem,optItem[ind],false,0,false,-1)">
                                                                    </v-select>


                                                                </v-col>
                                                            </v-row>
                                                        </div>
                                                        <!--File Upload-->
                                                        <div v-if="item.QuestionType==='File Upload'" class="mt-0">

                                                            <v-file-input label="Upload File" outlined dense accept="*" v-model="item.AnsBlob"
                                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('File',item.AnsBlob,false,true,0)"></v-file-input>
                                                            <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                            <div v-if="item.uploadedFile !=null && item.uploadedFile != ''">
                                                                <v-img :src=item.uploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                            </div>
                                                        </div>
                                                        <!-- Likert Scale-->
                                                        <div v-if="item.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                            <v-radio-group class="my-0 py-0" v-model="item.Answer">
                                                                <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                         :label="optItem.OptionValue"
                                                                         :value="optItem.OptionValue"></v-radio>
                                                            </v-radio-group>
                                                        </div>
                                                        <!-- Repeat Section Question Details comes here-->
                                                        <div v-if="RepeatSection.length >0 && Ritem.ParentQuestionID == item.QuestionID" v-for="(Ritem,RIndex) in RepeatSection" :key="RIndex" class="mb-4">

                                                            <label class="text-left heading-3 mb-2" v-if="Ritem.QuestionID==0"><b>{{Ritem.QuestionName}}</b></label>
                                                            <label class="text-left heading-3 mb-2" v-if="Ritem.QuestionID>0">{{Ritem.QuestionName}}</label>
                                                            <sub v-if="Ritem.IsMandatory==true" style="color:red;font-size:large"> *</sub>
                                                            <sub style="color:red;font-size:smaller" v-if="Ritem.QuestionHint.length > 0 && Ritem.QuestionHint != null && Ritem.QuestionHint != 'null'">{{Ritem.QuestionHint}}</sub>
                                                            <!-- Error Text-->
                                                            <sub style="color:red;font-size:smaller" v-if="(Ritem.IsMandatory==true && Ritem.Answer ==='')|| Ritem.IsValidationError ">{{Ritem.ValidationErrorTest}}</sub>

                                                            <!-- Single line text with Condition Question-->
                                                            <div v-if="Ritem.QuestionType=='Single Line Text' && (Ritem.IsValidationQuestion || Ritem.IsConditionQuestion) ">
                                                                <v-text-field v-if="Ritem.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer"
                                                                              :value="Ritem.DefaultValue" :min="minVal" :max="maxVal" :type="qType" v-bind:maxlength="txtLength" :rules="qRules"
                                                                              @focus="onFocusCheckQstnValidation(Ritem)" @change="AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)" />

                                                            </div>
                                                            <!-- Single line text without Condition Question-->
                                                            <div v-if="Ritem.QuestionType=='Single Line Text' && !(Ritem.IsValidationQuestion || Ritem.IsConditionQuestion) ">
                                                                <v-text-field v-if="Ritem.QuestionType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer"
                                                                              :value="Ritem.DefaultValue" @change="CheckQstnValidation(item)" />

                                                            </div>
                                                            <!--Multiline Answer-->
                                                            <v-textarea v-if="Ritem.QuestionType==='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="Ritem.Answer" />
                                                            <!-- Numeric text feilds-->
                                                            <v-flex xs12 sm12 md12>
                                                                <div v-if="Ritem.QuestionType==='Numeric'">
                                                                    <v-layout row>
                                                                        <v-flex grow>
                                                                            <v-text-field v-if="Ritem.QuestionType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="Ritem.Answer"
                                                                                          class="ml-3"
                                                                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                          onKeyPress="if(this.value.length==10) return false;" :readonly="Ritem.IsFormulaQuestion == true"
                                                                                          :value="Ritem.DefaultValue" :min="minVal" :max="maxVal"
                                                                                          @focus="Calculate(Ritem.QuestionID,BenefStepper)" @change="onFocusCheckQstnValidation(Ritem),AddSectionLogicRepeat(Ritem.QuestionID,BenefStepper,Ritem.RQstInd,item.QuestionID)">
                                                                            </v-text-field>
                                                                        </v-flex>
                                                                        <v-flex shrink>
                                                                            <sub class="mr-2 ml-2" v-if="Ritem.QuestionType==='Numeric'" style="color:green;font-size:large">{{Ritem.UOM}}</sub>
                                                                        </v-flex>
                                                                    </v-layout>
                                                                </div>
                                                            </v-flex>
                                                            <!--Single Choice Answer-->
                                                            <div v-if="Ritem.QuestionType==='Single Choice'" class="mt-0">

                                                                <v-radio-group class="my-0 py-0" v-model="Ritem.Answer" @change="AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                    <v-radio color="success" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                             :label="optItem.OptionValue"
                                                                             :value="optItem.OptionValue"></v-radio>
                                                                </v-radio-group>

                                                            </div>
                                                            <!--Multiple Choice -->
                                                            <div v-if="Ritem.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                                <div class="my-0 py-0" v-if="RepeatSection[Ritem.RQstInd].QuestionOptions.length>0">
                                                                    <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in RepeatSection[Ritem.RQstInd].QuestionOptions" :key="ind" v-model="RepeatSection[Ritem.RQstInd].QuestionOptions[ind].IsOptSelected"
                                                                                @change="GetMultiChoiceRepeat(optItem,optItem.IsOptSelected,optItem.OptionValue,Ritem.RQstInd,Ritem.QuestionID,BenefStepper),AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                    </v-checkbox>
                                                                </div>
                                                            </div>
                                                            <!--Dropdown -->
                                                            <div v-if="Ritem.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                <div v-if="Ritem.QuestionOptions.length>0">
                                                                    <v-select dense :items="Ritem.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                              item-text="OptionValue"
                                                                              label="Select" outlined hide-details v-model="Ritem.Answer" @change="AddSectionLogic(Ritem.QuestionID,BenefStepper)">
                                                                    </v-select>
                                                                </div>
                                                            </div>
                                                            <!--Date Picker-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='Date Picker'">
                                                                <v-menu v-model="Ritem.DateMenu"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="Ritem.Answer"
                                                                                      prepend-inner-icon="mdi-calendar"
                                                                                      outlined
                                                                                      readonly
                                                                                      dense
                                                                                      @click:prepend-inner="Ritem.DateMenu = true"
                                                                                      v-on="on" hide-details></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="Ritem.Answer" no-title @input="Ritem.DateMenu = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(Ritem)"></v-date-picker>
                                                                </v-menu>
                                                            </div>
                                                            <!--GPS Capture-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='GPS Capture'" v-model="Ritem.Answer">
                                                                <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                                <GmapMap :center="Coordinates"
                                                                         :zoom="13"
                                                                         style="width: 100%; height: 200px" @click.native="GetGPSCoordinatesRepeat(Ritem.QuestionID,false, Ritem.RQstInd)">
                                                                    <GmapMarker :position="Coordinates"
                                                                                :clickable="true"
                                                                                :draggable="true" @dragend="updateCoordinates($event.latLng, Ritem)" />
                                                                </GmapMap>
                                                            </div>
                                                            <!--Note-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='Note'">
                                                                <v-textarea outlined dense hide-details v-model="Ritem.Answer"></v-textarea>
                                                            </div>
                                                            <!--Photo Capture-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='Photo Capture'">

                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                    <v-radio color="success" label="Photo Capture" hide-details>
                                                                    </v-radio>
                                                                    <v-radio color="success" label="Photo Upload" hide-details>
                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="Ritem.IsUpload===0">
                                                                    <v-row>
                                                                        <v-col md="12">

                                                                            <v-btn @click="Ritem.startCamera =true,Ritem.camOn=true,onStartRepeat()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                            <v-btn class="mr-4" color="secondary" depressed @click="onCaptureRepeat(Ritem.QuestionID,false,0,Ritem.RQstInd,Ritem.webcam),Ritem.startCamera =false,Ritem.camOn=false" v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                            <v-btn @click="Ritem.startCamera =false,Ritem.camOn=false,onStop()" v-bind:disabled="!Ritem.camOn" depressed>Stop</v-btn>
                                                                            <v-btn color="primary" depressed class="mr-4" @click="fnflipCamera()">flip</v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row>
                                                                        <v-col md="8" v-if="Ritem.startCamera">
                                                                            <vue-web-cam ref="webcam"
                                                                                         :device-id="deviceId"
                                                                                         width="100%"
                                                                                         height="200px"
                                                                                         @started="onStarted"
                                                                                         @stopped="onStopped"
                                                                                         @error="onError"
                                                                                         @cameras="onCameras"
                                                                                         @camera-change="onCameraChangeRepeat" />
                                                                            <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->
                                                                        </v-col>
                                                                        <v-col md="4" v-if="Ritem.img !=null">
                                                                            <figure class="figure">
                                                                                <!--<img :src="img" class="center-block my-2" width="250" height="180" aspect-ratio="1.7" />-->
                                                                                <img :src="Ritem.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                            </figure>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>
                                                                <div v-show="Ritem.IsUpload===1">
                                                                    <v-row>
                                                                        <v-col md="4">

                                                                            <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="Ritem.AnsBlob"
                                                                                          @change="AnsBlobQID=Ritem.QuestionID,SaveFileRepeat('Photo',Ritem.AnsBlob,false,true,0,Ritem.RQstInd )"></v-file-input>
                                                                        </v-col>
                                                                        <v-col md="4" v-if="Ritem.uploadedImg !=null && Ritem.uploadedImg !=''">
                                                                            <v-img :src=Ritem.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>

                                                            </div>
                                                            <!--Audio Capture-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='Audio Capture'">
                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                    <v-radio color="success" label="Audio Capture" hide-details>
                                                                    </v-radio>
                                                                    <v-radio color="success" label="Audio Upload" hide-details>

                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="Ritem.IsUpload===0">
                                                                    <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                    :play="playSound"
                                                                                    :attempts="1"
                                                                                    :time="1"
                                                                                    :headers="headers"
                                                                                    :before-recording="callback"
                                                                                    :pause-recording="callback"
                                                                                    :after-recording="SaveAudioRepeat"
                                                                                    :before-upload="callback"
                                                                                    :successful-upload="callback"
                                                                                    :failed-upload="callback"
                                                                                    v-model="Ritem.Answer"
                                                                                    @click.native="AnsBlobQID=Ritem.QuestionID, RQstInd = Ritem.RQstInd" />

                                                                </div>
                                                                <div v-show="Ritem.IsUpload===1">

                                                                    <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="Ritem.AnsBlob"
                                                                                  @change="AnsBlobQID=Ritem.QuestionID,SaveFile('Audio',Ritem.AnsBlob,false,true,0)"></v-file-input>

                                                                </div>
                                                            </div>
                                                            <!--Signature Capture-->
                                                            <div class="mb-6" v-if="Ritem.QuestionType==='Signature Capture'" v-model="Ritem.Answer">
                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[RIndex]" @change="Ritem.IsUpload=audUpload[RIndex]">
                                                                    <v-radio color="success" label="Signature Capture" hide-details>

                                                                    </v-radio>
                                                                    <v-radio color="success" label="Signature Upload" hide-details>

                                                                    </v-radio>
                                                                </v-radio-group>
                                                                <div v-show="Ritem.IsUpload===0">
                                                                    <v-card outlined>
                                                                        <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                                    </v-card>
                                                                    <div>
                                                                        <v-btn color="secondary" depressed class="mt-2 mr-2" @click="saveSign(Ritem.QuestionID,false,0)">Save</v-btn>
                                                                        <v-btn default depressed class="mt-2 mr-2" @click="clearSignRepeat(false,Ritem.QuestionID,Ritem.RQstInd )">Clear</v-btn>
                                                                    </div>
                                                                </div>
                                                                <div v-show="Ritem.IsUpload===1">

                                                                    <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="Ritem.AnsBlob"
                                                                                  @change="AnsBlobQID=Ritem.QuestionID,SaveFile('Signature',Ritem.AnsBlob,false,true,0)"></v-file-input>
                                                                </div>
                                                                <v-card v-if="uploadedSign !=null">
                                                                    <v-img :src=uploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                </v-card>
                                                            </div>
                                                            <!--Cascading Dropdown-->
                                                            <div v-if="Ritem.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                <v-row class="my-0 py-0">
                                                                    <v-col class="my-0 py-0">
                                                                        <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem.CascadingList[ind].OptionID"
                                                                                  :label="` ${optItem.OptionValue}`" outlined=""
                                                                                  v-for="(optItem,ind) in RepeatSection[Ritem.RQstInd].CascadingQuestionOptions" :key="ind"
                                                                                  :items="RepeatSection[Ritem.RQstInd].CascadingQuestionOptions[ind].CascadingList"
                                                                                  item-text="Name"
                                                                                  item-value="ID" @change="FilterSubItem(Ritem,optItem,optItem.CascadingList[ind].OptionID,false,0,false,Ritem.RQstInd)">
                                                                        </v-select>


                                                                    </v-col>
                                                                </v-row>
                                                            </div>
                                                            <!--File Upload-->
                                                            <div v-if="Ritem.QuestionType==='File Upload'" class="mt-0">

                                                                <v-file-input label="Upload File" outlined dense accept="*" v-model="Ritem.AnsBlob"
                                                                              @change="AnsBlobQID=Ritem.QuestionID,SaveFileRepeat('File',Ritem.AnsBlob,false,true,0,Ritem.RQstInd)"></v-file-input>
                                                                <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                                <div v-if="uploadedFile !=null">
                                                                    <v-img :src=Ritem.uploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                </div>
                                                            </div>
                                                            <!-- Likert Scale-->
                                                            <div v-if="Ritem.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                                <v-radio-group class="my-0 py-0" v-model="Ritem.Answer">
                                                                    <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                             :label="optItem.OptionValue"
                                                                             :value="optItem.OptionValue"></v-radio>
                                                                </v-radio-group>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </v-col>
                                            </v-row>
                                        </div>
                                        <v-row class="py-0 my-2">
                                            <v-col md="12" class="text-center text-md-right py-0 my-0">
                                                <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="UpdateBenefLinkedSectionQstnAnswers(BenefStepper),scrollToTop(lstSectionQstns)"></Secondary-Button>
                                                <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="PreviousBenefSectionQstnAnswers(),scrollToTop()"></Default-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <!-- Document upload-->
                                    <v-card-text v-show="IsInputFinished == true " class="px-0 py-0">
                                        <div class="taskHeight py-0 my-0">
                                            <v-row>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <h4 class="text-left page-head">In Case you have any comments please specify them below</h4>
                                                </v-col>
                                            </v-row>
                                            <v-row>

                                                <v-col>
                                                    <h4 class="text-left heading-3-dark mb-2">Add comments</h4>
                                                    <v-textarea rows="5" outlined dense label="Description" v-model="vmUpdateComment" type="text">
                                                    </v-textarea>
                                                    <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" v-model="fileInput" prepend-icon="mdi-paperclip" outlined="" dense=""
                                                                  id="file" accept="*"></v-file-input>
                                                </v-col>
                                            </v-row>
                                        </div>
                                        <v-row class="py-0 my-2">
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="fnTaskUpdate()"></Secondary-Button>
                                                <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsInputFinished = false, IsUpdate_BenSurvey = true,CheckPreviousInImport(BenefStepper)"></Default-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                </v-stepper-content>
                            </v-stepper-items>
                        </v-stepper>
                    </v-card-text>
                    <!-- Task Questions and input feilds -->

                </v-card>
            </v-dialog>
            <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
                <template>
                    <div class="pa-0 ma-0">
                        <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                    </div>
                </template>
            </v-snackbar>
            <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
                <template>
                    <div class="pa-0 ma-0">
                        <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                    </div>
                </template>
            </v-snackbar>
            <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
                <template>
                    <div class="pa-0 ma-0">
                        <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                    </div>
                </template>
            </v-snackbar>
        </v-row>
    </v-card>
    <!--</v-col>-->
</template>

<script>
    ///  import ActivityDataCollector from '@/views/Tasks/DataCollector/ActivityDataCollector.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import { dbImworks } from '../../ImworksDb'
    import { WebCam } from "vue-web-cam"
    import { Date } from 'core-js';
    import objUtils from '../../utils.js';

    export default {
        data() {
            return {
                NonBenefLinkedSurveySteps:1,
                vmCascadingOption: [],
                WorkspaceID: 0,
                lstTaskList: [],
                lstMyTask: [],
                lstSelectedTask: [],
                vmUpdateComment: "",
                fileInput: null,
                docName: "",
                docUName: "",
                TaskType: "",
                TaskTypeID: "",

                lstSurveyQstns: [],
                lstSections: [],
                lstSectionQstns: [],
                lstSectionQstns1: [],
                vmSection: [],
                SectionDetails :[],
                lstRepeatSectionQstns: [],

                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                IsUpdate: false,

                IsInputFinished: false,
                TaskDate: '',
                ActivityTabs: 'tab-1',

                lstAllSurvey: [],
                lstAllMyTask: [],
                //===== SurveyType======//
                dlgSurveyType: false,
                lstSurvey: [],
                varIsSubmitted: 1,

                vmAddBeneficiaryChoice: 0,
                vmBenefRegisterChoice: 0,
                BenefLinkedSurveySteps: 1,
                lstBeneficiary: [],
                lstBenefRegQstns: [],
                lstNewBeneficiary: [],
                vmSelectedBeneficiaries: [],
                vmSearchBeneficiary: "",
                BeneficiaryTableheaders: [
                    { text: "Name", align: "left", value: "BeneficiaryName" },
                    { text: "Created Date", value: "CreatedDate" }
                ],
                vmBenefSurveyChoice: 0,
                vmNonBenefSurveyChoice: 0,
                BenefStepper: 1,
                IsBenfInputFinished: [],
                BenefQstnStepper: [],
                lstBenefSurveyQstnAnswers: [],
                vmBenefSurveySubmitOption: 0,
                IsUpdate_BenSurvey: false,
                varIsBeneficiaryLinked: false,
                lstSaved: [],


                //=== Beneficiary Registration===//

                newBenefID: 0,
                multiChoiceAnsBT: [],
                vmBTDate: new Date().toISOString().substr(0, 10),
                mBTDate: "",
                blobFileInputBT: null,
                BTAnsBlob: [],
                BTAnsBlobQID: "",
                delListBT: [],
                cascadingAnsBT: [],
                img1: null,

                regfileInput: null,
                csvFileName: "",
                lstImportBeneficiaryHearder: [],
                lstImportBeneficiary: [],

                ImportNewBeneficiarySteps: 1,
                RegisterNewBenficiaryStep: 1,
                FinalStepvalue: "",
                ImportBeneficiaryTaskSteps: 1,
                //======ImportSurvey=======//
                surveyfileInput: null,
                csvSurveyFileName: "",
                lstImportSurveyQstnHearder: [],
                lstImportSurveyQstnHearderN: [],
                lstImportSurvey: [],
                //=====Survey Answer======//

                multiChoiceAns: [],
                MultiChoiceAnsForRepeat:[],
                //GPS Capture
                Coordinates: {
                    lat: 0,
                    lng: 0,
                },

                //Web cam Data
                dialog: false,
                img: null,
                camera: null,
                deviceId: null,
                devices: [],

                vmDate: "",//new Date().toISOString().substr(0, 10),
                mDate: "",

                blobFileInput: null,
                AnsFileName: "",
                AnsBlob: [],
                AnsBlobQID: "",
                audUpload: [],
                IsNew: [],
                blobFilename: "",

                headers: {
                    'X-Custom-Header': 'some data'
                },
                cascadingAnsArray: [],
                cascadingAns: [],
                fileUrl: "",
                editPhoto: true,
                delList: [],
                lstCountry: [],
                lstStates: [],
                lstDistrict: [],
                lstSubDistrict: [],

                //=========validation========= //
                rules: {
                    required: value => !!value || 'Required.',
                },
                numberRules: [
                    v => !v || /\[0-9]$/.test(v) || 'Please enter valid number'
                ],
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],
                phoneNoRules: [
                    v => !v || /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/.test(v) || 'Please enter valid Phone No'
                ],
                chkQstnValid: false,
                IsQstnValidation: false,
                txtLength: 10000,
                minVal: 0,
                maxVal: 0,
                qType: 'text',
                qRules: [],
                QstType: '',

                SkipLogic: [],
                lstCurr_SectionLogic: [],
                SurveyID: 0,
                uploadedImg: "",
                benefUploadedImg: "",
                uploadedSign: "",
                uploadedFile: "",
                benefUploadedFile: "",
                benefUploadedSign: "",
                ImageUploaded: "",
                savedAudio: "",
                //========= Log Frame=========//
                lstLogFrame: [],
                AuditLog: [],
                Periodicity: '',
                userimage: "",

                //======ReportIssue
                ReportIssuedlg: false,
                IssuefileInput: null,
                vmIssueComment: "",
                IssueReportTaskDetails: [],
                savedAudioBenef: "",


                // non beneficiary survey
                dlgNonBeneficarySurveyType: false,
                startCamera: false,
                camOn: false,
                fileUrl: "",
                ProjectID: "",
                RepeatSection :[],
                AnsBlobRepeat:[],
				RQstInd : -1,
                resetOnClick:0,
                IsvalidexcelData:true,
                lstSelectedBeneficiary :[],

                UnSavedNewBenfList :[], // Data getting directly from Dexie table
                SelectedBenefTypeID: 0,
                NewUnSavedBenefList : [], // Data after filtered using beneficiary TypeID


            }
        },
        created() {
            //GPS Capture
            this.$getLocation({})
                .then(Coordinates => {
                    this.Coordinates = Coordinates;
                })
        },
        computed: {
            //Web Cam Functions
            device: function () {
                return this.devices.find(n => n.deviceId === this.deviceId);
            },

            computedDate(date) {
                return this.formatDate(date)
            },
            computedBTDate() {
                return this.formatDate(this.vmBTDate)
            },
            FrmDate() {
                return this.formatDate(this.startDate)
            },
        },
        props: ['searchcall', 'searchtask', 'Filter', 'DateRange', 'StartDate', 'EndDate', 'ProjectName', 'BeneficiaryName', 'WorkflowName', 'Action', 'FilterGUID'],
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.GetMyTaskList('');
        },
        watch: {
            //Web Cam Functions
            camera: function (id) {
                this.deviceId = id;
            },
            searchcall: function () {
                this.GetMyTaskList(this.searchcall);
            },
            FilterGUID: function () {
                this.GetMyTaskList("");
            },
            //lstSelectedBeneficiary: function() {
            //    if (this.lstSelectedBeneficiary.length == 0) {
            //        this.lstSelectedBeneficiary = this.vmSelectedBeneficiaries;
            //    }
            //},
            devices: function () {
                // Once we have a list select the first one
                const [first, ...tail] = this.devices;
                if (first) {
                    this.camera = first.deviceId;
                    this.deviceId = first.deviceId;
                }
            },

        },
        methods: {

            getcolor(name) {
                let colors = [
                    "#1abc9c",
                    "#2ecc71",
                    "#3498db",
                    "#9b59b6",
                    "#34495e",
                    "#16a085",
                    "#27ae60",
                    "#2980b9",
                    "#8e44ad",
                    "#2c3e50",
                    "#f1c40f",
                    "#e67e22",
                    "#e74c3c",
                    "#95a5a6",
                    "#f39c12",
                    "#d35400",
                    "#c0392b",
                    "#bdc3c7",
                    "#7f8c8d",
                ];

                let sum = 0;
                for (let j = 0; j < name.length; j++) {
                    sum += name.charCodeAt(j);
                }
                let ascii = sum;
                let colortest = ascii % 19; // ths is the make sure that the colors are based on sum
                return colors[colortest];
            },
            CancelImport() {
      this.resetOnClick = 2
      this.dlgImportRegisterBeneficiary = false;
      this.ImportRegBenefSteps = 1;
      this.fileInput = null;
      this.lstImportBeneficiary = [];
      this.resetOnClick = 0
      this.lstImportBeneficiary = []
    },

            formatDateAndTime(date) {
                if (!date) return null;
                const [day, month, year] = date.substr(0, 10).split("-");
                const [hh, mm, ss] = date.substr(11, 19).split(":");
                if (month === "01") {
                    if (hh == 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "02") {
                    if (hh == 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "03") {
                    if (hh == 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "04") {
                    if (hh == 12) {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"APR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "05") {
                    if (hh == 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "06") {
                    if (hh == 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "07") {
                    if (hh == 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "08") {
                    if (hh == 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "09") {
                    if (hh == 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "10") {
                    if (hh == 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "11") {
                    if (hh == 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "12") {
                    if (hh == 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
            },

            filterTask(myListNew) {

                var filterTaskArray = [];
                filterTaskArray = myListNew;

                if (this.StartDate != "" && this.EndDate != "") {
                    filterTaskArray = filterTaskArray.filter(x => x.TaskDate.substr(0, 10) >= this.StartDate && x.TaskDate.substr(0, 10) <= this.EndDate);
                }

                if (this.Filter.length > 0) {
                    var filterTaskArrayTemp = [];
                    filterTaskArrayTemp = filterTaskArray;
                    filterTaskArray = [];
                    var seperateFilter = this.Filter.split(',');
                    for (var i = 0; i < seperateFilter.length; i++) {
                        var temp = filterTaskArrayTemp.filter(x => x.Type == seperateFilter[i]);
                        for (var j = 0; j <= temp.length - 1; j++) {
                            filterTaskArray.push(temp[j]);
                        }
                    }
                }

                if (this.ProjectName.length > 0) {
                    var filterTaskArrayTemp1 = [];
                    filterTaskArrayTemp1 = filterTaskArray;
                    filterTaskArray = [];
                    for (var i = 0; i < filterTaskArrayTemp1.length; i++) {
                        for (var k = 0; k < this.ProjectName.length; k++) {
                            var objSurvey = JSON.parse(filterTaskArrayTemp1[i].TaskData);

                            if (objSurvey.ProjectName !== null) {
                                if (objSurvey.ProjectName == this.ProjectName[k]) {
                                    filterTaskArray.push(filterTaskArrayTemp1[i]);
                                }
                            }
                        }
                    }
                }

                if (this.BeneficiaryName.length > 0) {
                    var filterTaskArrayTemp2 = [];
                    filterTaskArrayTemp2 = filterTaskArray;
                    filterTaskArray = [];
                    for (var i = 0; i < filterTaskArrayTemp2.length; i++) {
                        for (var k = 0; k < this.BeneficiaryName.length; k++) {
                            var objSurvey = JSON.parse(filterTaskArrayTemp2[i].TaskData);
                            if (objSurvey.BeneficiaryTypeID !== null) {
                                if (objSurvey.BeneficiaryTypeID == this.BeneficiaryName[k]) {
                                    filterTaskArray.push(filterTaskArrayTemp2[i]);
                                }
                            }
                        }
                    }
                }

                if (this.WorkflowName.length > 0) {
                    var filterTaskArrayTemp3 = [];
                    filterTaskArrayTemp3 = filterTaskArray;
                    filterTaskArray = [];
                    for (var i = 0; i < this.WorkflowName.length; i++) {
                        var temp = filterTaskArrayTemp3.filter(x => x.WorkflowID == this.WorkflowName[i]);
                        for (var j = 0; j <= temp.length - 1; j++) {
                            filterTaskArray.push(temp[j]);
                        }
                    }
                }

                return filterTaskArray;
            },

            async GetMyTaskList(SearchV) {

                this.lstTaskList = await dbImworks.MyTaskList.toArray();
                var myList = this.lstTaskList.filter(x => (x.WFLevel == 0) && (x.WFLevelStatus == "Pending"));

                var lstSavedList_Var = await dbImworks.SavedTaskDB.toArray(); //
                 //var myList = this.lstTaskList.filter(x => (x.WFLevel == 0) && (x.Status == "Partially DataCollected"));
                var props = ['pkId','SyncDBID', 'Command', 'Type', 'TypeID', 'TypeIDColumn', 'Status', 'WorkflowID', 'TotalWFLevel', 'WFLevel', 'WFLevelStatus', 'ResponseSyncID', 'DataCollectedBy', 'TaskDate', 'TaskSubmittedDate', 'TaskData', 'WorkspaceID'];
                var myListNew = myList.filter(function (o1) {
                    // filter out (!) items in result2
                    return !lstSavedList_Var.some(function (o2) {
                        return o1.SyncDBID === o2.SyncDBID;          // assumes unique id
                    });
                }).map(function (o) {
                    // use reduce to make objects with only the required properties
                    // and map to apply this to the filtered array as a whole
                    return props.reduce(function (newo, name) {
                        newo[name] = o[name];
                        return newo;
                    }, {});
                });

                var filterTaskArray = this.filterTask(myListNew);
                if (this.Action == "To DataCollect" || this.Action == "" || this.Action == "All") {
                }
                else {
                    filterTaskArray = [];
                }

                await this.GetMyList(filterTaskArray, SearchV);

            },

            async GetMyList(myList, SearchV) {

                this.lstMyTask = [];
                this.lstAllMyTask = [];

                var lstClientSubmitted = await dbImworks.TbMyTask.toArray();
                //this.AuditLog = await dbImworks.TbTaskAuditLog.toArray();

                if (lstClientSubmitted != null)

                    for (var i = 0; i < lstClientSubmitted.length; i++) {
                        for (var j = 0; j < myList.length; j++) {

                            if (lstClientSubmitted[i].Periodicity != 'Anytime') {
                                if (lstClientSubmitted[i].WFLevel == 0 && lstClientSubmitted[i].Type == myList[j].Type && lstClientSubmitted[i].TaskDate == myList[j].TaskDate
                                    && lstClientSubmitted[i].TypeID == myList[j].TypeID && lstClientSubmitted[i].WFLevelStatus == "Completed") {
                                    myList.splice(j, 1);
                                    myList = [...myList];
                                }
                            }
                        }

                    }
                for (var i = 0; i < myList.length; i++) {
                    if (myList[i].Type == 'Survey') {

                        var objSurvey = JSON.parse(myList[i].TaskData);
                        objSurvey.pkId = myList[i].pkId;
                        if (objSurvey != null) {
                            this.lstSurvey.push(objSurvey);

                            var McuntAct = 0;
                            var mySurveyDataListN = [];
                            if (SearchV != '') {
                                var mySurveyDataList = this.lstSurvey.filter(x => ((x.SurveyName.toLowerCase().includes(SearchV.toLowerCase()))));
                                if (myList[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                    McuntAct = 1;
                                };
                                if (mySurveyDataList.length > 0) {
                                    mySurveyDataListN = mySurveyDataList.filter(x => ((x.SurveyID == myList[i].TypeID)));
                                    if (mySurveyDataListN.length > 0) {
                                        McuntAct = 1;
                                    }
                                };
                            }
                            else if (SearchV == '') {
                                McuntAct = 1;
                            }

                            if (McuntAct == 1) {

                                this.lstAllMyTask.push({
                                    TaskType: 'Survey', TypeColor: 'rgba(33,150,243,.2)', TaskTypeID: objSurvey.SurveyID, TaskName: objSurvey.SurveyName, TaskDate: myList[i].TaskDate,
                                    TaskDescription: objSurvey.Description, ProjectName: objSurvey.ProjectName, SurveyList: objSurvey.SurveyQstnList,
                                    BeneficiaryTypeList: objSurvey.BeneficiaryTypeList, BeneficiaryRegistrationQstns: objSurvey.BeneficiaryRegistrationQstns,
                                    ProjectId: objSurvey.ProjectId, CreatedDate: objSurvey.CreatedDate, Periodicity: objSurvey.Periodicity, vMenu: false,
                                    TypeIDColumn: myList[i].TypeIDColumn, WorkspaceID: myList[i].WorkspaceID, IsBeneficiaryLinked: objSurvey.BeneficiaryTypeID != null ? true : false, pkId: objSurvey.pkId,BeneficiaryTypeID : objSurvey.BeneficiaryTypeID
                                });

                            this.varIsBeneficiaryLinked = objSurvey.BeneficiaryTypeID != null ? true : false;

                                this.ProjectID = objSurvey.ProjectId;
                            }
                        }
                    }
                }
                this.fileUrl = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectID.toString() + "/";
                if (this.$route.name == "CurrentTasks")
                 {
                    this.lstMyTask = this.lstAllMyTask.filter(x => x.TaskDate.substr(0, 10) == new Date().toISOString().substr(0, 10));
                }
                else if (this.$route.name == "OverdueTasks")
                {
                    this.lstMyTask = this.lstAllMyTask.filter(x => x.TaskDate.substr(0, 10) < new Date().toISOString().substr(0, 10));
                }

                this.lstMyTask.sort(function (a, b) {
                    return new Date(b.date) - new Date(a.date);
                });
            },
            async GetTaskDetails(Item) {

              this.AnsBlob = [];
              this.BTAnsBlob = [];
              var lastitem = await dbImworks.MyTaskList.where("pkId").equals(Item.pkId).toArray();

              if(lastitem != null && lastitem.length >0) {
                    if(lastitem[0].WFLevelStatus != "Pending") {
                        this.infoSnackbarMsg = true;
                        this.Msg = "This task has been already Updated.";
                        return false;
                    }
                }

                this.TaskType = Item.TaskType;
                this.TaskTypeID = Item.TaskTypeID;
                this.SurveyID = Item.TaskTypeID;
                this.Periodicity = Item.Periodicity;
                this.varIsBeneficiaryLinked = Item.IsBeneficiaryLinked != null ? true : false;
                // Log Frame
                //this.lstLogFrame = this.AuditLog.filter(x => x.TypeID == Item.TaskTypeID && x.Type == "Survey");

                var lstfilterlist = await dbImworks.TbTaskAuditLog.toArray();
                this.lstLogFrame = lstfilterlist.filter(x => x.TypeID == Item.TaskTypeID && x.Type == "Survey");
                this.lstLogFrame.sort((a, b) => b.pkLogID - a.pkLogID);


                this.UnSavedNewBenfList = await dbImworks.TbNewBeneficiary.toArray();
                this.NewUnSavedBenefList = this.UnSavedNewBenfList.filter(x => x.BeneficiaryTypeID == Item.BeneficiaryTypeID);

                this.TaskDate = Item.TaskDate;

                if (Item.TaskType == 'Survey') {
                    this.lstSelectedTask = this.lstSurvey.filter(x => (x.SurveyID == Item.TaskTypeID))[0];
                    this.lstSelectedTask.TaskDate = Item.TaskDate;

					if (!Item.IsBeneficiaryLinked)
                        {
                            this.dlgNonBeneficarySurveyType = true;
                        }
                    else
                        {
                            this.newBenefID = 0;
                            this.dlgSurveyType = true;
                        }

                    this.lstSurveyQstns = this.lstSelectedTask.SurveyQstnList != null ? this.lstSelectedTask.SurveyQstnList : [];
                    if (this.lstSurveyQstns.length == 0) {
                        this.Msg = "Survey Sections and Questionnaire are not available...";
                        this.infoSnackbarMsg = true;
                        return;
                    }


                    // this.dlgSurveyType = true;
                    //this.dlgNonBenSurvey = true;
                }

                this.lstSaved = await dbImworks.SavedTaskDB.toArray();

                if (this.lstSaved.length > 0) {

                    this.lstSaved = this.lstSaved.filter(x => (x.Type == this.TaskType) && (x.TypeID == this.TaskTypeID));

                    this.lstBeneficiary = this.lstSelectedTask.BeneficiaryList != null ? this.lstSelectedTask.BeneficiaryList : [];
                    this.lstBenefSurveyQstnAnswers = JSON.parse(this.lstSaved[0].SurveyAnswers);
                    this.vmSelectedBeneficiaries = JSON.parse(this.lstSaved[0].SelectedBeneficiaries);
                    this.lstNewBeneficiary = JSON.parse(this.lstSaved[0].NewBeneficiaries);
                    for (var j = 0; j < this.lstNewBeneficiary.length; j++)
                        this.lstBeneficiary.push(this.lstNewBeneficiary[j]);

                    for( var i = 0 ; i<= this.lstBenefSurveyQstnAnswers.length-1; i++){

                        var temp = this.lstSurveyQstns.filter(x=>x.QuestionID == this.lstBenefSurveyQstnAnswers[i].QuestionID && x.SectionID == this.lstBenefSurveyQstnAnswers[i].SectionID);
                        if(temp.length > 0){
                                temp[0].Answer = this.lstBenefSurveyQstnAnswers[i].Answer;
                            }
                    }
                }


            },

            //======================Beneficiary =======================//
            GetBeneficiary() {

                if (this.vmAddBeneficiaryChoice == 1)
                {

                    this.BenefLinkedSurveySteps = 2;
                    this.lstBeneficiary = this.lstSelectedTask.BeneficiaryList != null ? this.lstSelectedTask.BeneficiaryList : [];
                    for (var j = 0; j < this.lstNewBeneficiary.length; j++) {
                        var temp = this.lstBeneficiary.filter(x => x.BeneficiaryID == this.lstNewBeneficiary[j].BeneficiaryID);
                        if (temp.length == 0) {
                            this.lstBeneficiary.push(this.lstNewBeneficiary[j]);
                        }
                    }
                    
                    // Getting Newly registered beneficiaries form dexie table
                    if(this.NewUnSavedBenefList.length > 0)
                    {
                         for (var j = 0; j < this.NewUnSavedBenefList.length; j++)
                         {
                              var temp = this.lstBeneficiary.filter(x => x.BeneficiaryID == this.NewUnSavedBenefList[j].BeneficiaryID ||  x.TokenValue == this.NewUnSavedBenefList[j].TokenValue);
                             if (temp.length == 0)
                             {
                                this.lstBeneficiary.push(this.NewUnSavedBenefList[j]);
                             }
                          }
                    }

                }
                else if (this.vmAddBeneficiaryChoice == 2)
                    this.BenefLinkedSurveySteps = 3;

            },
            GetBenefRegistrationQstns() {
                var colValue = "";
                // this.lstBenefRegQstns = this.lstSelectedTask.BeneficiaryRegistrationQstns != null ? this.lstSelectedTask.BeneficiaryRegistrationQstns : [];

                if (this.lstSelectedTask.BeneficiaryRegistrationQstns == null) {
                    this.lstSelectedTask.BeneficiaryRegistrationQstns = [];
                }

                 this.lstImportBeneficiaryHearder = [];
                 this.lstBenefRegQstns =[];

                for (var i = 0; i < this.lstSelectedTask.BeneficiaryRegistrationQstns.length; i++) {

                    var obj = new Object();
                        obj.Answer ='',
                        obj.BeneficiaryTypeID= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BeneficiaryTypeID,
                        obj.BTQstnAnsOptionList=this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BTQstnAnsOptionList,
                        obj.CascadingQuestionOptions= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].CascadingQuestionOptions,
                        obj.IsMandatoryResponse= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsMandatoryResponse,
                        obj.IsSearchable= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsSearchable,
                        obj.IsUnique= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsUnique,
                        obj.IsUpload= 0,
                        obj.QuestionHint= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionHint,
                        obj.QuestionID= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionID,
                        obj.QuestionName= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionName,
                        obj.QuestionType= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionType,
                        obj.QuestionTypeID= this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionTypeID, obj.mBTDate = false,
                        obj.ValidationErrorTest ="",
                        obj.IsValidationError = false,
                        obj.IsValidationQuestion = this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsValidationQuestion,
                        obj.RegexExpression=this.lstSelectedTask.BeneficiaryRegistrationQstns[i].RegexExpression,
                        obj.BTQstnValidationList=this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BTQstnValidationList,
                        obj.qType = "",obj.QstType ="",obj.textLength = 10,obj.minVal = 0 ,obj.maxVal = 9999999999,obj.qRules=[],

                        this.lstBenefRegQstns.push(obj);

                        // this.lstBenefRegQstns.push({
                        //      Answer :'',
                        //      BeneficiaryTypeID: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BeneficiaryTypeID,
                        //      BTQstnAnsOptionList: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BTQstnAnsOptionList,
                        //      CascadingQuestionOptions: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].CascadingQuestionOptions,
                        //      IsMandatoryResponse: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsMandatoryResponse,
                        //      IsSearchable: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsSearchable,
                        //      IsUnique: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsUnique,
                        //      IsUpload: 0,
                        //      QuestionHint: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionHint,
                        //      QuestionID: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionID,
                        //      QuestionName: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionName,
                        //      QuestionType: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionType,
                        //      QuestionTypeID: this.lstSelectedTask.BeneficiaryRegistrationQstns[i].QuestionTypeID, mBTDate: false,
                        //      ValidationErrorTest :"",IsValidationError : false,
                        //      IsValidationQuestion : this.lstSelectedTask.BeneficiaryRegistrationQstns[i].IsValidationQuestion,
                        //      RegexExpression:this.lstSelectedTask.BeneficiaryRegistrationQstns[i].RegexExpression,
                        //      BTQstnValidationList:this.lstSelectedTask.BeneficiaryRegistrationQstns[i].BTQstnValidationList,
                        //      qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules:[],
                        //   });
                    this.audUpload[i] = 0;
                    colValue = "Answer" + (i + 1).toString();
                    //this.lstImportBeneficiaryHearder.push({ text: this.lstBenefRegQstns[i].QuestionName, value: colValue })
                    if(this.lstBenefRegQstns[i].QuestionType =="Date Picker")
                    {
                            this.lstImportBeneficiaryHearder.push({
                            text: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                            value: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                            type:"date",
                            //mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse,
                              mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                              QuestionCondition:this.lstBenefRegQstns[i].BTQstnValidationList,
                            });
                    }
                    else if(this.lstBenefRegQstns[i].QuestionType =="Numeric")
                    {
                            this.lstImportBeneficiaryHearder.push({
                                    text: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                                    value: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                                   type:"number",
                                   mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                                   QuestionCondition:this.lstBenefRegQstns[i].BTQstnValidationList,
                                  });
                    }
                    else if(this.lstBenefRegQstns[i].QuestionType =="Dropdown" || this.lstBenefRegQstns[i].QuestionType =="Multi-Choice" || this.lstBenefRegQstns[i].QuestionType =="Single Choice" || this.lstBenefRegQstns[i].QuestionType =="Likert Scale")
                    {
                        var optionlist=[];
                        for (var j = 0; j < this.lstBenefRegQstns[i].BTQstnAnsOptionList.length; j++) {
                            optionlist.push(this.lstBenefRegQstns[i].BTQstnAnsOptionList[j].OptionValue)
                        }
                        this.lstImportBeneficiaryHearder.push({
                                text: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                                value: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                                type:"select",
                                mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                                QuestionCondition:this.lstBenefRegQstns[i].BTQstnValidationList,
                                options:optionlist,
                                });
                    }
                    else
                    {
                        this.lstImportBeneficiaryHearder.push({
                                text: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                                value: this.lstBenefRegQstns[i].QuestionName.concat(' ',this.lstBenefRegQstns[i].QuestionHint),
                               type:"string",
                               mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                               QuestionCondition:this.lstBenefRegQstns[i].BTQstnValidationList,
                              });
                    }
                }
                if (this.vmBenefRegisterChoice == 1) {

                    this.BenefLinkedSurveySteps = 8;
                    var lstCascadingItems = this.lstSelectedTask.CascadingList != null ? this.lstSelectedTask.CascadingList : [];

                    var QItem = this.lstBenefRegQstns.filter(x => (x.QuestionType == "Cascading Dropdown"))[0];
                    if (QItem != null) {
                        for (var i = 0; i < QItem.CascadingQuestionOptions.length; i++) {
                            if (QItem.CascadingQuestionOptions[i].OptionValue == "Country") {
                                this.lstCountry = lstCascadingItems[0].Country.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                QItem.CascadingQuestionOptions[i].CascadingList = this.lstCountry;
                            }
                            else if (QItem.CascadingQuestionOptions[i].OptionValue == "States") {
                                this.lstStates = lstCascadingItems[0].States.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                QItem.CascadingQuestionOptions[i].CascadingList = this.lstStates;
                            }
                            else if (QItem.CascadingQuestionOptions[i].OptionValue == "District") {
                                this.lstDistrict = lstCascadingItems[0].District.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                QItem.CascadingQuestionOptions[i].CascadingList = this.lstDistrict;
                            }
                            else if (QItem.CascadingQuestionOptions[i].OptionValue == "SubDistrict") {
                                this.lstSubDistrict = lstCascadingItems[0].SubDistrict.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                QItem.CascadingQuestionOptions[i].CascadingList = this.lstSubDistrict;
                            }

                        }
                    }
                    this.RegisterNewBenficiaryStep = 1;




                }
                else if (this.vmBenefRegisterChoice == 2) {

                    this.BenefLinkedSurveySteps = 9;
                    this.ImportNewBeneficiarySteps = 1;

                }

            },
            async RegisterNewBenficiary() {

              if(this.fnCheckBeneficiaryMandatoryAndValidation()== false)
                {

                    this.newBenefID--;
                    var lstBenefRegQstnAnswers = [];
                    var Ans = "";
                    var blobFile = [];

                    for (var i = 0; i < this.lstBenefRegQstns.length; i++) {
                        Ans = "";
                        if (this.lstBenefRegQstns[i].QuestionType == 'Multi-Choice')
                            for (var j = 0; j < this.multiChoiceAnsBT.length; j++)
                                Ans = (Ans != '') ? Ans + ',' + this.multiChoiceAnsBT[j] : this.multiChoiceAnsBT[j];
                        else if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown")
                            for (var j = 0; j < this.cascadingAnsBT.length; j++)
                                Ans = (Ans != '') ? Ans + ',' + this.cascadingAnsBT[j] : this.cascadingAnsBT[j];
                        else if ((this.lstBenefRegQstns[i].QuestionType == "Audio Capture") || (this.lstBenefRegQstns[i].QuestionType == "Signature Capture")
                            || (this.lstBenefRegQstns[i].QuestionType == "Photo Capture") || (this.lstBenefRegQstns[i].QuestionType == "File Upload")) {

                            if (this.BTAnsBlob.length != 0) {
                                blobFile = this.BTAnsBlob.filter(x => x.QuestionID == this.lstBenefRegQstns[i].QuestionID)[0].blobData;
                                if (typeof blobFile != "undefined") {
                                    this.FileUpload(blobFile, this.lstBenefRegQstns[i].Answer, true);
                                }
                            }

                            Ans = this.lstBenefRegQstns[i].Answer;
                        }

                        else
                            Ans = this.lstBenefRegQstns[i].Answer;

                        lstBenefRegQstnAnswers.push({ QuestionID: this.lstBenefRegQstns[i].QuestionID, AnswerTypeID: this.lstBenefRegQstns[i].QuestionTypeID, Answer: Ans });

                    }

                    var now = new Date();
                    var TokenValue = "";
                    TokenValue = "Beneficiary_"+now.getFullYear().toString() + now.getMonth().toString() + now.getDate().toString() + now.getHours().toString() + now.getMinutes().toString() + now.getSeconds().toString() + now.getMilliseconds().toString();

                    this.lstNewBeneficiary.push({
                        BeneficiaryID: this.newBenefID,
                        BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer,
                        BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                        CreatedDate: new Date(),TokenValue: TokenValue
                    })
                    this.lstBeneficiary.push({ BeneficiaryID: this.newBenefID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, CreatedDate: Date.now(),TokenValue: TokenValue });

                    this.vmSelectedBeneficiaries.push({
                        BeneficiaryID: this.newBenefID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                        CreatedDate: new Date(),TokenValue: TokenValue
                    });



                    var tempData = await dbImworks.TbNewBeneficiary.toArray();
                    var BenefIndexID = -(tempData.length+1);
                    // Insert into New Dexie table, to get the newly registered beneficiary. when we are trying to update another task;
                    await dbImworks.TbNewBeneficiary.put({
                        BeneficiaryID: BenefIndexID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                        CreatedDate: new Date(), TokenValue: TokenValue, BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                    });


                    this.RegisterNewBenficiaryStep = 2;
                    for (var i = 0; i < this.lstBenefRegQstns.length; i++) {
                        this.lstBenefRegQstns[i].Answer = "";

                        if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown") {
                            for (var j = 0; j < this.lstBenefRegQstns[i].CascadingQuestionOptions.length; j++) {
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[j][0] = null;
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[j][j] = null;
                                //this.FilterSubItem(this.lstBenefRegQstns[i], this.lstBenefRegQstns[i].CascadingQuestionOptions[j], 0, true);

                            }
                        }
                        if (this.lstBenefRegQstns[i].QuestionType == "Photo Capture" || this.lstBenefRegQstns[i].QuestionType == "Signature Capture" ||
                            this.lstBenefRegQstns[i].QuestionType == "File Upload") {

                            this.img1 = null;
                            this.benefUploadedImg = null;
                            this.startCamera = false;
                            this.camOn = false;
                            this.devices = [];
                            this.deviceId = null;
                            this.camera = null;
                        }
                    }

                }
            },
            //=========== Import Benef Register ==========//
            DownloadSampleBenefRegFile() {

                var ColumnHeader = "";
                var ColumnData1 = [];
                var ColumnData = [];
                var strResposnse ="Registration Response here"
                for (var i = 0; i < this.lstImportBeneficiaryHearder.length; i++) {

                    if(this.lstImportBeneficiaryHearder[i].text.includes(','))
                    {
                       ColumnHeader += ColumnHeader == "" ? "\""+this.lstImportBeneficiaryHearder[i].text+"\"" : "," + "\""+this.lstImportBeneficiaryHearder[i].text+"\"";
                    }
                    else
                    {
                        ColumnHeader += ColumnHeader == "" ? this.lstImportBeneficiaryHearder[i].text : "," + this.lstImportBeneficiaryHearder[i].text;
                    }


                    if(this.lstImportBeneficiaryHearder[i].type ==  "date")
                    {
                        ColumnData1.push(strResposnse + " *Please enter date in mm-dd-yyyy format.");
                    }
                    else
                    {
                        ColumnData1.push(strResposnse);
                    }

                    //ColumnData1.push(this.lstImportBeneficiaryHearder[i].text + " value");
                }

                ColumnHeader = ColumnHeader + "\n";
                ColumnData.push(ColumnData1);


                ColumnData.forEach(function (row) {
                    ColumnHeader += row.join(',');
                    ColumnHeader += "\n";
                });

                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_beneficiary_register_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            },
            OnRegFileUpload() {

                this.csvFileName = this.regfileInput.name;
                var vm = this
                if (window.FileReader) {
                    var reader = new FileReader();
                    reader.readAsText(this.regfileInput);
                    // Handle errors load
                    reader.onload = function (event) {
                        var csv = event.target.result;
                        vm.ReadCsvFile(csv)
                    };
                    reader.onerror = function (evt) {
                        if (evt.target.error.name == "NotReadableError") {
                            this.Msg = "Cannot read file !";
                            this.infoSnackbarMsg = true;
                        }

                    };
                }
                else {
                    this.Msg = 'FileReader is not supported in this browser.';
                    this.infoSnackbarMsg = true;
                }

            },
            ReadCsvFile(csv) {
                //this.lstImportBeneficiary =[];
                var flag = true;
                var today = new Date().toLocaleString();
                var lines = csv.split(/\r\n|\n/);

                //var headers = lines[0].split(",");
                var headers = objUtils.common.MergeCommas(lines[0]);
                if (this.lstImportBeneficiaryHearder.length == headers.length) {
                    for (var c = 0; c < this.lstImportBeneficiaryHearder.length; c++) {
                        if (this.lstImportBeneficiaryHearder[c].text != headers[c]) {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag) {
                        this.Msg = ("Beneficiary Registration Questionnaire is not matching for the selected file...  ");
                        this.infoSnackbarMsg = true;
                        this.regfileInput = null;
                        this.csvFileName = "";
                    }
                    else
                    {
                        for (var i = 1; i < lines.length; i++)
                        {
                            var obj = {};
                            //var currentline = lines[i].split(",");
                          var currentline;
                                currentline = objUtils.common.MergeCommas(lines[i]);

                            if (currentline != "")
                            {
                             //for (var j = 0; j < headers.length; j++)
                             //obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                for (var j = 0; j < headers.length; j++)
                                {
                                    var dataType=this.lstImportBeneficiaryHearder[j].type
                                    var QuestionCondition =[];
                                    QuestionCondition = this.lstImportBeneficiaryHearder[j].QuestionCondition != null ? this.lstImportBeneficiaryHearder[j].QuestionCondition : [];
                                     if (dataType =="number")
                                     {
                                         if(isNaN(currentline[j]) ||(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes'))
                                         {
                                             obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                         }
                                         else
                                         {
                                             obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                         }
                                         // validating Question Condition
                                            this.IsvalidexcelData = true;
                                            if(QuestionCondition.length > 0)
                                            {
                                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                {
                                                    if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                                    {
                                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                        if(!currentline[j].match(phoneNum))
                                                        {
                                                            obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                    if (QuestionCondition[Count].ValidationCondition == "Min & Max Range"  && currentline[j] != "")
                                                    {
                                                        if(!(parseInt(currentline[j]) > parseInt(QuestionCondition[Count].MinValue) && parseInt(currentline[j]) < parseInt(QuestionCondition[Count].MaxValue)))
                                                        {
                                                            obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                }
                                            }// if ends here

                                      }
                                      else if(dataType =="date")
                                      {
                                          var dateField=currentline[j];
                                          var timestamp = function(dateField) {
                                          if ( Object.prototype.toString.call(dateField) === "[object Date]" )
                                          return true;
                                          return false;
                                          };

                                          if (timestamp==false  ||(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes'))
                                          {
                                              obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                          }
                                          else
                                          {
                                              obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                          }
                                          // validating Question Condition
                                           this.IsvalidexcelData = true;
                                            if(QuestionCondition.length > 0)
                                            {
                                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                {
                                                    // Date Validation
                                                     if (QuestionCondition[Count].ValidationCondition == "Should be greater than Current date"  && currentline[j] != "")
                                                     {
                                                        if (this.formatExcelDate(dateField) < this.formatExcelDate(today))
                                                        {
                                                            obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                         }
                                                     }
                                                     if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date"  && currentline[j] != "")
                                                     {
                                                        if (this.formatExcelDate(dateField) > this.formatExcelDate(today))
                                                        {
                                                            obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                         }
                                                     }
                                                }
                                            } // if ends here

                                       }
                                       else if(this.lstBenefRegQstns[j].QuestionType =="Dropdown" || this.lstBenefRegQstns[j].QuestionType =="Multi-Choice" || this.lstBenefRegQstns[j].QuestionType =="Single Choice" || this.lstBenefRegQstns[j].QuestionType =="Likert Scale" )
                                       {

                                           if((currentline[j]!="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes') || (!this.lstImportBeneficiaryHearder[j].options.includes(currentline[j]) && currentline[j]!=""))
                                                {
                                                 obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                                }
                                                else
                                                {
                                                 obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                                }
                                        }
                                        else
                                        {
                                                if(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes')
                                                {
                                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                                }
                                                else
                                                {
                                                 obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                                 }
                                                 this.IsvalidexcelData = true;
                                                // Condition Starts here
                                                if(QuestionCondition.length > 0)
                                                {
                                                    for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                    {
                                                        if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                                        {
                                                            var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                            if(!currentline[j].match(phoneNum))
                                                            {
                                                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                                 this.IsvalidexcelData = false;
                                                            }
                                                        }
                                                        if (QuestionCondition[Count].ValidationCondition == "Email" && currentline[j] != "")
                                                        {
                                                            var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                                            if(!currentline[j].match(email))
                                                            {
                                                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                                this.IsvalidexcelData = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                // ends here

                                         }

                                  }
                                  this.lstImportBeneficiary.push(obj);
                            }
                        }
                    }
                }
                else {
                    this.Msg = ("Beneficiary Type format is not matching for the selected file...  ");
                    this.infoSnackbarMsg = true;
                    this.regfileInput = null;
                    this.csvFileName = "";
                }
            },
            searchInvalidInputArray () {
                        var newList = this.lstImportBeneficiary.filter(element => {
                            for (var property in element) {
                                if (element.hasOwnProperty(property)) {
                                    if(element[property] == "Invalid Input") {
                                        return true;
                                    }
                                }
                            }

                    });
                     if(newList.length>0)
                         {
                         return false;
                         }
                     else
                         {
                         return true;
                         }
                    },
            async ImportRegisterBeneficiary() {
               this.newBenefID=0;
                var lstImportList = [];
          if (this.searchInvalidInputArray() == true) {

                for (var i = 0; i < this.lstImportBeneficiary.length; i++) {
                    var lstBenefRegQstnAnswers = [];
                    this.newBenefID--;

                    for (var j = 0; j < this.lstImportBeneficiaryHearder.length; j++)
                        lstBenefRegQstnAnswers.push({
                            QuestionID: this.lstBenefRegQstns[j].QuestionID, AnswerTypeID: this.lstBenefRegQstns[j].QuestionTypeID,
                            Answer: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[j].value]
                        });
                    this.lstNewBeneficiary.push({
                        BeneficiaryID: this.newBenefID,
                        BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, BeneficiaryName: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[0].value],
                        BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                        CreatedDate: new Date()
                    });
                    this.lstBeneficiary.push({
                        BeneficiaryID: this.newBenefID, BeneficiaryName: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[0].value],
                        BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, CreatedDate: Date.now()
                    });
                    this.vmSelectedBeneficiaries.push({
                        BeneficiaryID: this.newBenefID, BeneficiaryName: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[0].value],
                        BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, CreatedDate: new Date()
                    });

                }

                this.regfileInput = null;
                this.lstImportBeneficiary = [];
                for (var i = 0; i < this.lstBenefRegQstns.length; i++)
                    this.lstBenefRegQstns[i].Answer = "";
                this.GetBenefSurveyQstns();
                //this.BenefLinkedSurveySteps = 4;
                this.ImportNewBeneficiarySteps = 3
                }
                  else
                {
                this.Msg = "Please resolve all Invalid Input(s).";
                    this.infoSnackbarMsg = true;
                }

            },
            //========================================//
            GetSelectedBeneficiaries() {


                // Pusgh to vmSelectedBeneficiaries
                for (k = 0 ; k<= this.lstSelectedBeneficiary.length-1 ; k++)
                {
                    var temp = this.vmSelectedBeneficiaries.filter(x=> x.BeneficiaryID == this.lstSelectedBeneficiary[k].BeneficiaryID);
                    if(temp.length == 0)
                    {
                           this.vmSelectedBeneficiaries.push(this.lstSelectedBeneficiary[k])
                    }
                    if(this.lstSelectedBeneficiary[k].BeneficiaryID < 0) // if Beneficiary ID is < 0 then these beneficiaries are New ly added Beneficiary
                    {
                        var temp = this.lstNewBeneficiary.filter(x=> x.BeneficiaryID == this.lstSelectedBeneficiary[k].BeneficiaryID);
                        if(temp.length == 0)
                        {
                            this.lstNewBeneficiary.push(this.lstSelectedBeneficiary[k]);
                        }
                    }
                }

                if (this.lstBenefSurveyQstnAnswers.length > 0) {
                    // when adding a new beneficiary 'Add Another' Option there might be chance of changing the index of beneficiary in 'this.vmSelectedBeneficiaries'
                    // In that case we need to update the index inside the array 'this.lstBenefSurveyQstnAnswers' as well as in 'this.RepeatSection' also
                    var filtArr = this.lstBenefSurveyQstnAnswers.filter(a => this.vmSelectedBeneficiaries.map(b => b.BeneficiaryID).includes(a.ResponseID));
                    for (var i = 0; i < this.vmSelectedBeneficiaries.length; i++) {
                       for (var j = 0; j < filtArr.length; j++)
                            if (filtArr[j].ResponseID == this.vmSelectedBeneficiaries[i].BeneficiaryID)
                               filtArr[j].BeneficiaryInd = i+1;
                    }
                    this.lstBenefSurveyQstnAnswers = filtArr;

                    for (var j= 0; j<= this.lstBenefSurveyQstnAnswers.length-1 ; j++)
                    {
                         for(var k= 0; k <= this.RepeatSection.length-1; k++)
                         {
                            if(this.RepeatSection[k].BeneficiaryID == this.lstBenefSurveyQstnAnswers[j].ResponseID)
                            {
                                 this.RepeatSection[k].benefIndex = this.lstBenefSurveyQstnAnswers[j].BeneficiaryInd;
                            }
                         }
                    }

                }
                this.BenefLinkedSurveySteps = 4;
            },

            GetBenefSurveyUpdateChoice() {

                this.GetBenefSurveyQstns();
                if (this.vmBenefSurveyChoice == 1) {
                    this.BenefLinkedSurveySteps = 7;
                    this.ImportBeneficiaryTaskSteps=1;


                }
                else if (this.vmBenefSurveyChoice == 2) {
                    this.BenefLinkedSurveySteps = 5;
                    this.BenefStepper = 1;

                }

            },
            GetBenefSurveyQstns() {
                var colValue = "";
                this.BenefStepper = 1;
                this.lstSurveyQstns = [];
                this.lstSurveyQstns = this.lstSelectedTask.SurveyQstnList != null ? this.lstSelectedTask.SurveyQstnList : [];

                if (this.lstSurveyQstns.length == 0) {
                    this.Msg = "Survey Sections and Questionnaire are not available...";
                    this.infoSnackbarMsg = true;
                }
                else {

                    var lstCascadingItems = this.lstSelectedTask.CascadingList != null ? this.lstSelectedTask.CascadingList : [];
                    var vaCascadingQuestion = this.lstSurveyQstns.filter(x => (x.QuestionType == "Cascading Dropdown"));
                    if(vaCascadingQuestion.length > 0){
                    for(var QuestionCount = 0 ; QuestionCount <= vaCascadingQuestion.length-1; QuestionCount ++){
                            var QItem = vaCascadingQuestion[QuestionCount];
                            if (QItem != null) {
                                for (var i = 0; i < QItem.CascadingQuestionOptions.length; i++) {
                                    if (QItem.CascadingQuestionOptions[i].OptionValue == "Country") {
                                        this.lstCountry = lstCascadingItems[0].Country.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                        QItem.CascadingQuestionOptions[i].CascadingList = this.lstCountry;
                                    }
                                    else if (QItem.CascadingQuestionOptions[i].OptionValue == "States") {
                                        this.lstStates = lstCascadingItems[0].States.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                        QItem.CascadingQuestionOptions[i].CascadingList = this.lstStates;
                                    }
                                    else if (QItem.CascadingQuestionOptions[i].OptionValue == "District") {
                                        this.lstDistrict = lstCascadingItems[0].District.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                        QItem.CascadingQuestionOptions[i].CascadingList = this.lstDistrict;
                                    }
                                    else if (QItem.CascadingQuestionOptions[i].OptionValue == "SubDistrict") {
                                         this.lstSubDistrict = lstCascadingItems[0].SubDistrict.sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                                         QItem.CascadingQuestionOptions[i].CascadingList = this.lstSubDistrict;
                                    }

                                }
                            }
                        }
                    }

                    this.lstImportSurveyQstnHearder = [];
                    for (var i = 0; i < this.lstSurveyQstns.length; i++) {

                        this.lstSurveyQstns[i].IsUpload = 0;
                        this.audUpload[i] = 0;
                        colValue = "Answer" + (i + 1).toString();
                            //this.lstImportSurveyQstnHearder.push({ text: this.lstSurveyQstns[i].QuestionName, value: colValue })
        if(this.lstSurveyQstns[i].QuestionType =="Date Picker")
        {
              this.lstImportSurveyQstnHearder.push({
                text: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
                value: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
                type:"date",
                //mandatory:this.lstSurveyQstns[i].IsMandatory,
                 mandatory:this.lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                QuestionCondition:this.lstSurveyQstns[i].QuestionCondition,


        });
        }
        else if(this.lstSurveyQstns[i].QuestionType =="Numeric")
        {
        this.lstImportSurveyQstnHearder.push({
                text: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
                value: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
               type:"number",
               mandatory:this.lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
               QuestionCondition:this.lstSurveyQstns[i].QuestionCondition,

              });
        }
        else if(this.lstSurveyQstns[i].QuestionType == 'Single Choice' || this.lstSurveyQstns[i].QuestionType == 'Multi-Choice' || this.lstSurveyQstns[i].QuestionType == 'Dropdown' || this.lstSurveyQstns[i].QuestionType == 'Likert Scale')
        {
        var optionlist=[];
        for (var j = 0; j < this.lstSurveyQstns[i].QuestionOptions.length; j++) {
           optionlist.push(this.lstSurveyQstns[i].QuestionOptions[j].OptionValue)
       }
        this.lstImportSurveyQstnHearder.push({
                text: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
                value: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
               type:"select",
               mandatory:this.lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
               options:optionlist,
              QuestionCondition:this.lstSurveyQstns[i].QuestionCondition,

             });
        }
        else
        {
        this.lstImportSurveyQstnHearder.push({
                text: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
                value: this.lstSurveyQstns[i].QuestionName.concat(' ',this.lstSurveyQstns[i].QuestionHint),
               type:"string",
               mandatory:this.lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
               QuestionCondition:this.lstSurveyQstns[i].QuestionCondition,

              });
        }
                        }

                    this.img = null;
                    this.camera = null;
                    this.deviceId = null;
                    this.devices = [];

                    if (this.dlgSurveyType)
                        for (var i = 1; i <= this.vmSelectedBeneficiaries.length; i++)
                            this.BenefQstnStepper[i] = 1;
                    else {
                        this.BenefQstnStepper[1] = 1;
                    }

                    this.lstSections = [];
                    var customDisabled = true;
                    const map = new Map();
                    for (const item of this.lstSurveyQstns) {
                        if (!map.has(item.SectionID)) {
                            map.set(item.SectionID, true);
                            customDisabled = true;
                            // initialy we only enable the first item in section drop down
                            const index = this.lstSurveyQstns.findIndex((element, index) => {
                                if (element.SectionID === item.SectionID) {
                                    return true
                                }
                            });
                            if(index == 0)
                            {
                                customDisabled = false;
                            }
                            this.lstSections.push({
                                SectionID: item.SectionID,
                                SectionName: item.SectionName,
                                customDisabled: customDisabled
                            });
                            this.SectionDetails.push({
                                SectionID: item.SectionID,
                                SectionName: item.SectionName,
                                disabled: false
                            });
                        }
                    }
                    //// Filtering out all repeat sections
                    for (const item of this.lstSurveyQstns)
                    {
                        if(item.IsConditionQuestion)
                         {

                            var SectionLogic = item.SectionLogic;
                            //Loop Start
                            for(var RCnt = 0 ; RCnt <= SectionLogic.length-1; RCnt++ )
                            {
                                if(SectionLogic[RCnt].SkipSectionID >0)
                                {
                                  var Temp = this.lstSections.filter(x=>x.SectionID == SectionLogic[RCnt].SkipSectionID)
                                    const index = this.lstSections.findIndex((element, index) => {
                                        if (element.SectionID === SectionLogic[RCnt].SkipSectionID) {
                                        return true
                                        }
                                    });
                                    if(Temp.length > 0)
                                    {
                                        this.lstSections.splice(index, 1);
                                        //this.lstSections = [...this.lstSections];
                                    }
                                }
                            }
                            // loop End
                        }
                    }
                    this.UpdateBenefQstnInput(this.BenefStepper);
                }
            },
            GetSectionQuestions1(e,benefIndx, RepeatCount) {
                if(this.fnCheckQuestionValidationAndCondition(this.lstSectionQstns,benefIndx))
				{
                    this.vmSection = this.lstSections[this.BenefQstnStepper[benefIndx] - 1];
					return false;
				}
                if(this.fnCheckRepeatSectionQuestionValidationAndCondition(benefIndx))
                {
                    this.vmSection = this.lstSections[this.BenefQstnStepper[benefIndx] - 1]
                    return false;
                }
                this.lstSectionQstns = [];
                var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == this.vmSection.SectionID));
                var cnt = 0;
                currSecQstns.sort(function (a, b) {
                    return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
                });
                if (RepeatCount == 0) {
                    cnt = 0;
                    for (var i = 0; i < currSecQstns.length; i++) {

                        if(currSecQstns[i].QuestionOptions !=null){

                            currSecQstns[i].QuestionOptions.sort(function(a,b){var x = a.OptionValue < b.OptionValue? -1:1; return x; });
                        }


                        var obj = new Object

                        obj.RQstInd = cnt, obj.SurveyID= this.SurveyID,
                            obj.SectionID= currSecQstns[i].SectionID,
                            obj.SectionName= currSecQstns[i].SectionName, obj.SectionOrder= currSecQstns[i].SectionOrder,
                            obj.QuestionOrder= currSecQstns[i].QuestionOrder, obj.QuestionTypeID= currSecQstns[i].QuestionTypeID,obj.QuestionID= currSecQstns[i].QuestionID, obj.QuestionName= currSecQstns[i].QuestionName,
                            obj.SectionLogic= currSecQstns[i].SectionLogic, obj.QuestionType= currSecQstns[i].QuestionType, obj.IsFormulaQuestion= currSecQstns[i].IsFormulaQuestion, obj.IsConditionQuestion= currSecQstns[i].IsConditionQuestion,
                            obj.QuestionCondition= currSecQstns[i].QuestionCondition, obj.QuestionOptions= currSecQstns[i].QuestionOptions, obj.QuestionHint= currSecQstns[i].QuestionHint, obj.IsValidationQuestion= currSecQstns[i].IsValidationQuestion,
                            obj.RegEx= currSecQstns[i].RegEx, obj.Answer= currSecQstns[i].Answer, obj.FieldVar= currSecQstns[i].FieldVar, obj.IsUnique= currSecQstns[i].IsUnique, obj.IsUpload= currSecQstns[i].IsUpload, obj.ErrorText= currSecQstns[i].ErrorText,
                            obj.DefaultValue= currSecQstns[i].DefaultValue, obj.IsMandatory= currSecQstns[i].IsMandatory, obj.IsSearchable= currSecQstns[i].IsSearchable, obj.QuestionCategory= currSecQstns[i].QuestionCategory, obj.UOM= currSecQstns[i].UOM,
                            obj.FormulaOperation= currSecQstns[i].FormulaOperation, obj.FormulaExpression= currSecQstns[i].FormulaExpression, obj.FormulaFieldVariables= currSecQstns[i].FormulaFieldVariables, obj.CascadingQuestionOptions= currSecQstns[i].CascadingQuestionOptions,obj.IsValidationError = false, obj.ValidationErrorTest ="", obj.DateMenu = false,obj.qRules=[], obj.uploadedSign ="",obj.img='',obj.uploadedImg ='',obj.IndexPos =i+1,obj.startCamera= false,obj.camOn = false,obj.signaturebutton= false,obj.uploadedFile =''

                            this.lstSectionQstns.push(obj)

                        //this.lstSectionQstns.push({
                        //    RQstInd: cnt, SurveyID: this.SurveyID,
                        //    SectionID: currSecQstns[i].SectionID, SectionName: currSecQstns[i].SectionName, SectionOrder: currSecQstns[i].SectionOrder,
                        //    QuestionOrder: currSecQstns[i].QuestionOrder, QuestionTypeID: currSecQstns[i].QuestionTypeID, QuestionID: currSecQstns[i].QuestionID, QuestionName: currSecQstns[i].QuestionName,
                        //    SectionLogic: currSecQstns[i].SectionLogic, QuestionType: currSecQstns[i].QuestionType, IsFormulaQuestion: currSecQstns[i].IsFormulaQuestion, IsConditionQuestion: currSecQstns[i].IsConditionQuestion,
                       //     QuestionCondition: currSecQstns[i].QuestionCondition, QuestionOptions: currSecQstns[i].QuestionOptions, QuestionHint: currSecQstns[i].QuestionHint, IsValidationQuestion: currSecQstns[i].IsValidationQuestion,
                       //     RegEx: currSecQstns[i].RegEx, Answer: currSecQstns[i].Answer, FieldVar: currSecQstns[i].FieldVar, IsUnique: currSecQstns[i].IsUnique, IsUpload: currSecQstns[i].IsUpload, ErrorText: currSecQstns[i].ErrorText,
                       //     DefaultValue: currSecQstns[i].DefaultValue, IsMandatory: currSecQstns[i].IsMandatory, IsSearchable: currSecQstns[i].IsSearchable, QuestionCategory: currSecQstns[i].QuestionCategory, UOM: currSecQstns[i].UOM,
                       //     FormulaOperation: currSecQstns[i].FormulaOperation, FormulaExpression: currSecQstns[i].FormulaExpression, FormulaFieldVariables: currSecQstns[i].FormulaFieldVariables, CascadingQuestionOptions: currSecQstns[i].CascadingQuestionOptions,IsValidationError : false, ValidationErrorTest :"", DateMenu : false,qRules:[], uploadedSign :"",img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,signaturebutton: false,uploadedFile :'',
                       // });
                        cnt++;
                    }
                    var benefSurveyQstn = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.SectionID == this.vmSection.SectionID));

                    if (benefSurveyQstn.length > 0) {
                        for (var i = 0; i < this.lstSectionQstns.length; i++) {
                            this.lstSectionQstns[i].Answer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.SurveyID == this.lstSectionQstns[i].SurveyID)
                                && (x.SectionID == this.lstSectionQstns[i].SectionID) && (x.QuestionID == this.lstSectionQstns[i].QuestionID)
                                && (x.RQstInd == this.lstSectionQstns[i].RQstInd))[0].Answer;
                        }
                    }

                    this.lstCurr_SectionLogic = [];
                    for (var i = 0; i < this.lstSectionQstns.length; i++) {
                        if (this.lstSectionQstns[i].IsConditionQuestion) {
                            //this.lstCurr_SectionLogic.push(this.lstSectionQstns[i].SectionLogic)

                            this.lstCurr_SectionLogic.push({
                                QuestionID: this.lstSectionQstns[i].SectionLogic[0].QuestionID, ConditionValue: this.lstSectionQstns[i].SectionLogic[0].ConditionValue,
                                Answer: this.lstSectionQstns[i].SectionLogic[0].Answer, LogicOperator: this.lstSectionQstns[i].SectionLogic[0].LogicOperator,
                                SkipSectionID: this.lstSectionQstns[i].SectionLogic[0].SkipSectionID, LogicType: this.lstSectionQstns[i].SectionLogic[0].LogicType
                            });

                        }
                    }
                }
                else {

                    cnt = 0;
                    for (var j = 1; j <= RepeatCount; j++) {

                        // this push is to print the section name like (section 2:1 , section 2:2)
                        this.lstSectionQstns.push({
                            RQstInd: cnt, SurveyID: this.SurveyID,
                            SectionID: this.vmSection.SectionID, SectionName: this.vmSection.SectionName, SectionOrder: currSecQstns[0].SectionOrder,
                            QuestionOrder: 1, QuestionTypeID: 0, QuestionID: 0, QuestionName: this.vmSection.SectionName + " : " + j, SectionLogic: [], QuestionType: "",
                            IsFormulaQuestion: 0, IsConditionQuestion: 0, QuestionCondition: null, QuestionOptions: null, QuestionHint: "", IsValidationQuestion: 0,
                            RegEx: "", Answer: null, FieldVar: "", IsUnique: 0, IsUpload: 0, ErrorText: "", DefaultValue: "", IsMandatory: 0, IsSearchable: 0, QuestionCategory: 1,
                            FormulaOperation: "", FormulaExpression: "", FormulaFieldVariables: null, CascadingQuestionOptions: null, UOM: "", IsValidationError : false,ValidationErrorTest:"", DateMenu : false,qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules: [],uploadedSign : "",img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,signaturebutton: false,uploadedFile :'',
                        });
                        cnt++;
                        for (var i = 0; i < currSecQstns.length; i++) {
                            // this push is to print the section questions
                            this.lstSectionQstns.push({
                                RQstInd: cnt, SurveyID: this.SurveyID,
                                SectionID: currSecQstns[i].SectionID, SectionName: currSecQstns[i].SectionName, SectionOrder: currSecQstns[i].SectionOrder,
                                QuestionOrder: currSecQstns[i].QuestionOrder, QuestionTypeID: currSecQstns[i].QuestionTypeID, QuestionID: currSecQstns[i].QuestionID,
                                QuestionName: currSecQstns[i].QuestionName, SectionLogic: currSecQstns[i].SectionLogic, QuestionType: currSecQstns[i].QuestionType,
                                IsFormulaQuestion: currSecQstns[i].IsFormulaQuestion, IsConditionQuestion: currSecQstns[i].IsConditionQuestion, QuestionCondition:currSecQstns[i].QuestionCondition,
                                QuestionOptions: currSecQstns[i].QuestionOptions, QuestionHint: currSecQstns[i].QuestionHint, IsValidationQuestion: currSecQstns[i].IsValidationQuestion,
                                RegEx: currSecQstns[i].RegEx, Answer: currSecQstns[i].Answer, FieldVar: currSecQstns[i].FieldVar, IsUnique: currSecQstns[i].IsUnique, IsUpload: currSecQstns[i].IsUpload,
                                ErrorText: currSecQstns[i].ErrorText, DefaultValue: currSecQstns[i].DefaultValue, IsMandatory: currSecQstns[i].IsMandatory, IsSearchable: currSecQstns[i].IsSearchable,
                                QuestionCategory: currSecQstns[i].QuestionCategory, FormulaOperation: currSecQstns[i].FormulaOperation, FormulaExpression: currSecQstns[i].FormulaExpression, UOM: currSecQstns[i].UOM,
                                FormulaFieldVariables: currSecQstns[i].FormulaFieldVariables, CascadingQuestionOptions: currSecQstns[i].CascadingQuestionOptions,IsValidationError : false,ValidationErrorTest :"", DateMenu : false,qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules: [],uploadedSign : "",img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,signaturebutton: false,uploadedFile :'',
                           });
                            cnt++;
                        }

                    }
                }
                var SectionCurrentIndex = this.lstSections.findIndex(x => (x.SectionID == this.vmSection.SectionID));
                this.BenefQstnStepper[benefIndx] = SectionCurrentIndex + 1;
            },
            async UpdateBenefQstnInput(benefIndx) {
                
                this.vmSection = this.lstSections[this.BenefQstnStepper[benefIndx] - 1];
                this.lstSections[this.BenefQstnStepper[benefIndx] - 1].customDisabled = false;
                this.lstSectionQstns = [];
                var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == this.vmSection.SectionID));
                var cnt = 0;
                currSecQstns.sort(function (a, b) {
                    return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
                });

                var benefSurveyQstn = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.SectionID == this.vmSection.SectionID));
                if (!currSecQstns[0].IsRepeatSection) {
                    for (var i = 0; i <= currSecQstns.length-1; i++) {

                        if(currSecQstns[i].QuestionOptions != null)
                        {
                            currSecQstns[i].QuestionOptions.sort(function(a,b){var x = a.OptionValue < b.OptionValue? -1:1; return x; });
                        }



                        this.lstSectionQstns.push({
                            RQstInd: cnt, SurveyID: this.SurveyID,
                            SectionID: currSecQstns[i].SectionID, SectionName: currSecQstns[i].SectionName, SectionOrder: currSecQstns[i].SectionOrder,
                            QuestionOrder: currSecQstns[i].QuestionOrder, QuestionTypeID: currSecQstns[i].QuestionTypeID, QuestionID: currSecQstns[i].QuestionID, QuestionName: currSecQstns[i].QuestionName,
                            SectionLogic: currSecQstns[i].SectionLogic, QuestionType: currSecQstns[i].QuestionType, IsFormulaQuestion: currSecQstns[i].IsFormulaQuestion, IsConditionQuestion: currSecQstns[i].IsConditionQuestion,
                            QuestionCondition: currSecQstns[i].QuestionCondition, QuestionOptions: currSecQstns[i].QuestionOptions, QuestionHint: currSecQstns[i].QuestionHint, IsValidationQuestion: currSecQstns[i].IsValidationQuestion,
                            RegEx: currSecQstns[i].RegEx, Answer: currSecQstns[i].Answer, FieldVar: currSecQstns[i].FieldVar, IsUnique: currSecQstns[i].IsUnique, IsUpload: currSecQstns[i].IsUpload, ErrorText: currSecQstns[i].ErrorText,
                            DefaultValue: currSecQstns[i].DefaultValue, IsMandatory: currSecQstns[i].IsMandatory, IsSearchable: currSecQstns[i].IsSearchable, QuestionCategory: currSecQstns[i].QuestionCategory, UOM: currSecQstns[i].UOM,
                            FormulaOperation: currSecQstns[i].FormulaOperation, FormulaExpression: currSecQstns[i].FormulaExpression, FormulaFieldVariables: currSecQstns[i].FormulaFieldVariables, CascadingQuestionOptions: currSecQstns[i].CascadingQuestionOptions,IsValidationError : false,ValidationErrorTest:"", DateMenu : false,qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules: [],uploadedSign : "",img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,signaturebutton: false,uploadedFile :'',
                        });
                        cnt++;
                        // New code added -- 19-01-2021
                             if(benefSurveyQstn.length > 0){

                                    var SavedAnswer = benefSurveyQstn.filter(x=>x.QuestionID == currSecQstns[i].QuestionID)[0].Answer;
                                    //currSecQstns[i].Answer = SavedAnswer;
                                    this.lstSectionQstns[i].Answer = SavedAnswer;
                                    var TempQuestionID  = 0;
                                    if (currSecQstns[i].QuestionType == "Cascading Dropdown") {

                                        var AnsOptions = SavedAnswer.split(',');
                                        this.cascadingAns = AnsOptions;
                                        for (var j = 0; j < AnsOptions.length; j++) {
                                            var pID = 0;
                                            if (this.lstSectionQstns[i].CascadingQuestionOptions[j].CascadingList != null)
                                                pID = this.lstSectionQstns[i].CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                            this.lstSectionQstns[i].CascadingQuestionOptions[j][j] = pID;
                                            this.FilterSubItem(this.lstSectionQstns[i], this.lstSectionQstns[i].CascadingQuestionOptions[j], pID, false, benefIndx, false,-1);
                                        }
                                    }

                                if (currSecQstns[i].QuestionType == 'Date Picker')
                                    this.vmDate =SavedAnswer;
                                if (this.lstSectionQstns[i].QuestionType == 'Multi-Choice') {

                                    var AnsOptions = SavedAnswer.split(';');
                                   // this.multiChoiceAns = AnsOptions;
                                    for (var j = 0; j < AnsOptions.length; j++){
                                        this.lstSectionQstns[i].QuestionOptions.filter(x => x.OptionValue == AnsOptions[j])[0].IsOptSelected = true; }
                                        this.lstSectionQstns[i].Answer ="";
                                }
                                if ((currSecQstns[i].QuestionType == "Photo Capture") || (currSecQstns[i].QuestionType == "Signature Capture") ||
                                    (currSecQstns[i].QuestionType == "File Upload") || (currSecQstns[i].QuestionType == "Audio Capture")) {

                                    var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(SavedAnswer).toArray();
                                    var blbFile = "";
                                    if(typeof ansBlobData[0] != "undefined")
                                    {
                                         blbFile = ansBlobData[0].AttachedDocument;
                                    }


                                    // To Read Photo
                                    if (currSecQstns[i].QuestionType == "Photo Capture" && blbFile != "") {

                                        var UploadIndex = benefSurveyQstn.filter(x=>x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                        this.lstSectionQstns[i].IsUpload = UploadIndex;
                                        this.audUpload[i] = UploadIndex;
                                        if (ansBlobData[0].IsUploadedFile) {
                                            const fr = new FileReader()
                                            fr.readAsDataURL(blbFile)
                                           // fr.addEventListener('load', () => {
                                           //     this.uploadedImg = fr.result

                                           // });
                                             await fr.addEventListener('loadend', () => {
                                               //currSecQstns[i].uploadedSign = fr.result
                                                this.lstSectionQstns.filter(x=> x.QuestionID == currSecQstns[i-1].QuestionID )[0].uploadedImg = fr.result;
                                               });
                                        }
                                        else {
                                            const fr = new FileReader()
                                            fr.readAsDataURL(blbFile)
                                           // fr.addEventListener('load', () => {
                                           //     this.img = fr.result
                                           // });
                                            await fr.addEventListener('loadend', () => {
                                               //currSecQstns[i].uploadedSign = fr.result
                                                this.lstSectionQstns.filter(x=> x.QuestionID == currSecQstns[i-1].QuestionID )[0].img = fr.result;
                                               });
                                        }
                                    }
                                    if (currSecQstns[i].QuestionType == "Signature Capture" && blbFile != "") {
                                        var UploadIndex = benefSurveyQstn.filter(x=>x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                        this.lstSectionQstns[i].IsUpload = UploadIndex;
                                        this.audUpload[i] = UploadIndex;
                                        TempQuestionID = currSecQstns[i].QuestionID ;
                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        if(currSecQstns.length == 1)
                                        {
                                            await fr.addEventListener('loadend', () => {
                                               //currSecQstns[i].uploadedSign = fr.result
                                                this.lstSectionQstns.filter(x=> x.QuestionID == TempQuestionID )[0].uploadedSign = fr.result;
                                               });
                                        }
                                        else
                                        {

                                            if(i > currSecQstns.length -1)
                                            {
                                                await fr.addEventListener('loadend', () => {

                                               //currSecQstns[i].uploadedSign = fr.result
                                                this.lstSectionQstns.filter(x=> x.QuestionID == currSecQstns[i-1].QuestionID)[0].uploadedSign = fr.result;
                                              });
                                            }
                                            else
                                            {
                                                await fr.addEventListener('loadend', () => {

                                               //currSecQstns[i].uploadedSign = fr.result
                                                this.lstSectionQstns.filter(x=> x.QuestionID == currSecQstns[i-1].QuestionID)[0].uploadedSign = fr.result;
                                              });
                                            }
                                        }
                                    }
                                    if (this.lstSectionQstns[i].QuestionType == "File Upload" && blbFile != "") {
                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        fr.addEventListener('load', () => {
                                            this.uploadedFile = fr.result

                                        });
                                    }
                                    if (currSecQstns[i].QuestionType == "Audio Capture" && blbFile != "")
                                    {

                                        this.savedAudio = window.URL.createObjectURL(blbFile);
                                        var audioCatpure = this.dlgSurveyType ? document.getElementById("recAudio") : document.getElementById("recAudioOne");

                                          // if (audioCatpure != null) {
                                             //  //audioCatpure.__vue__._data.recordList[0] = blbFile;
                                             //  audioCatpure.__vue__._data.recorder.records[0] = blbFile;
                                             //  audioCatpure.__vue__._data.recorder.records[0].url = this.savedAudio;
                                            //   //audioCatpure.__vue__._data.recorder.records[0] = "";
                                          //}
                                    }
					            }
                            }
                        // New code added -- 19-01-2021 - code ended
                    }
                    // this code is used to automaticaly trigger skip and repeat logic if Question have defaut value
                    if(this.lstSectionQstns.length > 0)
                    {

                        var lstDefaultAnsQuestions  = this.lstSectionQstns.filter(x=> x.Answer !="");
                        if(lstDefaultAnsQuestions.length > 0)
                        {
                            // Checking for condition Questions
                            var lstConditionQuestion = lstDefaultAnsQuestions.filter(x=> x.IsConditionQuestion == 1);
                            if(lstConditionQuestion.length > 0)
                            {
                                for(var Cnt = 0; Cnt <=  lstConditionQuestion.length-1 ; Cnt++)
                                {
                                    var IsExistInSkipLogic =[];
                                    if (this.dlgSurveyType)
                                    {
                                        IsExistInSkipLogic = this.RepeatSection.filter(x=> x.ParentQuestionID  == lstConditionQuestion[Cnt].QuestionID && x.benefIndex == benefIndx);
                                    }
                                    else{ IsExistInSkipLogic = this.RepeatSection.filter(x=> x.ParentQuestionID  == lstConditionQuestion[Cnt].QuestionID); }


                                    if(IsExistInSkipLogic.length == 0)
                                    {
                                        if(lstConditionQuestion[Cnt].QuestionType == 'Multi-Choice')
                                        {

                                           var vaDefaultAnswer = lstConditionQuestion[Cnt].DefaultValue
                                           for(var OptCnt = 0 ; OptCnt <=lstConditionQuestion[Cnt].QuestionOptions.length-1;  OptCnt++)
                                           {
                                               if(lstConditionQuestion[Cnt].QuestionOptions[OptCnt].OptionValue == vaDefaultAnswer)
                                               {
                                                  lstConditionQuestion[Cnt].QuestionOptions[OptCnt].IsOptSelected = true;
                                                  this.multiChoiceAns.push({OptionValue:lstConditionQuestion[Cnt].QuestionOptions[OptCnt].OptionValue , QuestionID : lstConditionQuestion[Cnt].QuestionID,benefIndex : benefIndx});
                                               }
                                           }
                                            await this.AddSectionLogicForMultiChoice(null,lstConditionQuestion[Cnt].QuestionID,benefIndx,lstConditionQuestion[Cnt].QuestionOptions);
                                          //  this.AddSectionLogic(lstConditionQuestion[Cnt].QuestionID, this.BenefStepper)
                                        }
                                        else
                                        {
                                            this.AddSectionLogic(lstConditionQuestion[Cnt].QuestionID, this.BenefStepper)
                                        }
                                    }
                                }
                            }
                        }

                        //this code is used to automaticaly trigger the calculations
                        var lstFormulaQuestion  = this.lstSectionQstns.filter(x=> x.IsFormulaQuestion == 1);
                        if(lstFormulaQuestion.length > 0)
                        {
                            for(var Cnt = 0; Cnt <=  lstFormulaQuestion.length-1 ; Cnt++)
                            {
                                var QID = lstFormulaQuestion[Cnt].QuestionID;
                                this.Calculate(QID, benefIndx)
                            }
                        }

                    }

                }
                else {
                    if (benefSurveyQstn.length > 0)
                      {
                        var arrRepeat = benefSurveyQstn.filter(x => (x.QuestionID == currSecQstns[0].QuestionID));
                        for (var j = 1; j <= arrRepeat.length; j++) {

                            this.lstSectionQstns.push({
                                RQstInd: cnt, SurveyID: this.SurveyID,
                                SectionID: this.vmSection.SectionID, SectionName: this.vmSection.SectionName, SectionOrder: currSecQstns[0].SectionOrder,
                                QuestionOrder: 1, QuestionTypeID: 0, QuestionID: 0, QuestionName: this.vmSection.SectionName + " : " + j, SectionLogic: [], QuestionType: "",
                                IsFormulaQuestion: 0, IsConditionQuestion: 0, QuestionCondition: null, QuestionOptions: null, QuestionHint: "", IsValidationQuestion: 0,
                                RegEx: "", Answer: null, FieldVar: "", IsUnique: 0, IsUpload: 0, ErrorText: "", DefaultValue: "", IsMandatory: 0, IsSearchable: 0, QuestionCategory: 1,
                                FormulaOperation: "", FormulaExpression: "", FormulaFieldVariables: null, CascadingQuestionOptions: null, UOM: "",IsValidationError : false,ValidationErrorTest :"", DateMenu : false, qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules: [],uploadedSign : "",img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,uploadedFile :'',
                            });
                            cnt++;
                            for (var i = 0; i < currSecQstns.length; i++) {

                                this.lstSectionQstns.push({
                                    RQstInd: cnt, SurveyID: this.SurveyID,
                                    SectionID: currSecQstns[i].SectionID, SectionName: currSecQstns[i].SectionName, SectionOrder: currSecQstns[i].SectionOrder,
                                    QuestionOrder: currSecQstns[i].QuestionOrder, QuestionTypeID: currSecQstns[i].QuestionTypeID, QuestionID: currSecQstns[i].QuestionID,
                                    QuestionName: currSecQstns[i].QuestionName, SectionLogic: currSecQstns[i].SectionLogic, QuestionType: currSecQstns[i].QuestionType,
                                    IsFormulaQuestion: currSecQstns[i].IsFormulaQuestion, IsConditionQuestion: currSecQstns[i].IsConditionQuestion, QuestionCondition: currSecQstns[i].QuestionCondition,
                                    QuestionOptions: currSecQstns[i].QuestionOptions, QuestionHint: currSecQstns[i].QuestionHint, IsValidationQuestion: currSecQstns[i].IsValidationQuestion,
                                    RegEx: currSecQstns[i].RegEx, Answer: currSecQstns[i].Answer, FieldVar: currSecQstns[i].FieldVar, IsUnique: currSecQstns[i].IsUnique, IsUpload: currSecQstns[i].IsUpload,
                                    ErrorText: currSecQstns[i].ErrorText, DefaultValue: currSecQstns[i].DefaultValue, IsMandatory: currSecQstns[i].IsMandatory, IsSearchable: currSecQstns[i].IsSearchable,
                                    QuestionCategory: currSecQstns[i].QuestionCategory, FormulaOperation: currSecQstns[i].FormulaOperation, FormulaExpression: currSecQstns[i].FormulaExpression, UOM: currSecQstns[i].UOM,
                                    FormulaFieldVariables: currSecQstns[i].FormulaFieldVariables, CascadingQuestionOptions: currSecQstns[i].CascadingQuestionOptions,IsValidationError : false,ValidationErrorTest:"", DateMenu : false, qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules:[],uploadedSign : "", img:'',uploadedImg :'',IndexPos :i+1,startCamera: false,camOn : false,uploadedFile :'',
                                });
                                cnt++;
                            }

                        }

                        var ans = [];
                        for (var i = 0; i < this.lstSectionQstns.length; i++) {
                            if (this.lstSectionQstns[i].QuestionID > 0) {
                                ans = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.SurveyID == this.lstSectionQstns[i].SurveyID)
                                    && (x.SectionID == this.lstSectionQstns[i].SectionID) && (x.QuestionID == this.lstSectionQstns[i].QuestionID)
                                    && (x.RQstInd == this.lstSectionQstns[i].RQstInd))[0];

                                this.lstSectionQstns[i].Answer = ans != [] || ans != 'undefined' ? ans.Answer : "";


                                if (this.lstSectionQstns[i].QuestionType == 'Date Picker')
                                    this.vmDate = this.lstSectionQstns[i].Answer;
                                if (this.lstSectionQstns[i].QuestionType == 'Multi-Choice') {
                                    var AnsOptions = this.lstSectionQstns[i].Answer.split(',');
                                    this.multiChoiceAns = AnsOptions;
                                    for (var j = 0; j < AnsOptions.length; j++)
                                        this.lstSectionQstns[i].QuestionOptions.filter(x => x.OptionValue == AnsOptions[j])[0].IsOptSelected = true;
                                }
                                if (this.lstSectionQstns[i].QuestionType == "Cascading Dropdown") {
                                    var AnsOptions = this.lstSectionQstns[i].Answer.split(',');
                                    this.cascadingAns = AnsOptions;

                                    for (var j = 0; j < AnsOptions.length; j++) {

                                        var pID = 0;
                                        if (this.lstSectionQstns[i].CascadingQuestionOptions[j].CascadingList != null)
                                            pID = this.lstSectionQstns[i].CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                        this.lstSectionQstns[i].CascadingQuestionOptions[j][j] = pID;
                                        this.FilterSubItem(this.lstSectionQstns[i], this.lstSectionQstns[i].CascadingQuestionOptions[j], pID, false, benefIndx, true,-1);

                                    }
                                }
                                if ((this.lstSectionQstns[i].QuestionType == "Photo Capture") || (this.lstSectionQstns[i].QuestionType == "Signature Capture") ||
                                    (this.lstSectionQstns[i].QuestionType == "File Upload") || (this.lstSectionQstns[i].QuestionType == "Audio Capture")) {

                                    var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(this.lstSectionQstns[i].Answer).toArray();
                                    var blbFile = ansBlobData[0].AttachedDocument;

                                    if (this.lstSectionQstns[i].QuestionType == "Photo Capture") {
                                        if (ansBlobData[0].IsUploadedFile) {
                                            const fr = new FileReader()
                                            fr.readAsDataURL(blbFile)
                                            fr.addEventListener('load', () => {
                                                this.uploadedImg = fr.result

                                            });
                                        }
                                        else {
                                            const fr = new FileReader()
                                            fr.readAsDataURL(blbFile)
                                            fr.addEventListener('load', () => {
                                                this.img = fr.result

                                            });
                                        }
                                    }
                                    if (this.lstSectionQstns[i].QuestionType == "Signature Capture") {

                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        fr.addEventListener('load', () => {
                                            this.uploadedSign = fr.result

                                        });

                                    }
                                    if (this.lstSectionQstns[i].QuestionType == "File Upload") {
                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        fr.addEventListener('load', () => {
                                            this.uploadedFile = fr.result

                                        });
                                    }
                                    if (this.lstSectionQstns[i].QuestionType == "Audio Capture") {

                                        this.savedAudio = window.URL.createObjectURL(blbFile);
                                        //var audioCatpure = this.dlgSurveyType ? document.getElementById("recAudio") : document.getElementById("recAudioOne");

                                        //   if (audioCatpure != null) {
                                        //       //audioCatpure.__vue__._data.recordList[0] = blbFile;
                                        //       audioCatpure.__vue__._data.recorder.records[0] = blbFile;
                                        //       audioCatpure.__vue__._data.recorder.records[0].url = this.savedAudio;
                                        //       //audioCatpure.__vue__._data.recorder.records[0] = "";
                                        //   }
                                    }
                                }

                            }

                        }

                    }
                }


                this.lstCurr_SectionLogic = [];
                for (var i = 0; i < this.lstSectionQstns.length; i++) {
                    if (this.lstSectionQstns[i].IsConditionQuestion) {

                        this.lstCurr_SectionLogic.push({
                            QuestionID: this.lstSectionQstns[i].SectionLogic[0].QuestionID, ConditionValue: this.lstSectionQstns[i].SectionLogic[0].ConditionValue,
                            Answer: this.lstSectionQstns[i].SectionLogic[0].Answer, LogicOperator: this.lstSectionQstns[i].SectionLogic[0].LogicOperator,
                            SkipSectionID: this.lstSectionQstns[i].SectionLogic[0].SkipSectionID, LogicType: this.lstSectionQstns[i].SectionLogic[0].LogicType
                        });

                    }
                }


            },
            CheckMandatory() {
               // if (this.vmAddBeneficiaryChoice == 1 && this.vmSelectedBeneficiaries.length == 0) {
               if (this.vmAddBeneficiaryChoice == 1 && this.lstSelectedBeneficiary.length == 0) {
                    return false;
                }
                else
                    return true;
            },
            onFocusCheckQstnValidation(QItem) {
                //debugger;
                var valConditions = QItem.QuestionCondition;
                var NewErrorMsg ="";
                var OldErrorMsg ="";
                var errMsg = "";
				this.qType = "";
                this.QstType = "";
                this.txtLength = 0;
                this.minVal = 0;
                this.maxVal = 9999999999;
                this.qRules = [];
                var today = new Date().toLocaleDateString('en-GB');
                if (valConditions != null) {
                    this.IsQstnValidation = true;
                    // ValidationCondition
                    for (var i = 0; i < valConditions.length; i++) {
                        if (valConditions[i].ValidationCondition == 'Phone Number') {
                            //if(valConditions.length > 1)
                            //{
                            //    if(QItem.QuestionType  == "Single Line Text")
                            //    {
                            //        QItem.qType = "email";
                            //    }
                            //    if(QItem.QuestionType  == "Numeric")
                            //    {
                            //        QItem.qType = "number";
                            //    }
                           // }
                           // else
                          //  {
                            //    QItem.qType = "number";
                           // }
                            QItem.qType = "number";
                            QItem.QstType = "PhoneNumber";
                            QItem.txtLength = 10;
                            QItem.minVal = 0;
                            QItem.maxVal = 9999999999;
                            QItem.qRules = this.phoneNoRules;
                           // QItem.ErrorText ="";
                            QItem.IsValidationError = false;
                        }
                        else if (valConditions[i].ValidationCondition == 'Email') {

                           // if(valConditions.length > 1)
                           // {
                           //     if(QItem.QuestionType  == "Single Line Text")
                           //     {
                          //          QItem.qType = "email";
                          //      }
                         //      if(QItem.QuestionType  == "Numeric")
                          //      {
                           //         QItem.qType = "number";
                           //     }
                          //  }
                          //  else
                          //  {
                           //     QItem.qType = "email";
                          //  }
                             QItem.qType = "email";
                             QItem.textLength = 1000;
                            QItem.minVal = 0;
                            //QItem.qType = "email";
                            QItem.qRules = this.emailRules;
                           // QItem.ErrorText ="";
                            QItem.IsValidationError = false;

                        }
                        else if (valConditions[i].ValidationCondition == 'Min & Max Range') {

                            QItem.textLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = valConditions[i].MinValue;
                            QItem.maxVal = valConditions[i].MaxValue;
                            QItem.qType = "number";
                            //this.qRules = [
                            //    v => !v || /\[0-9]{txtLength}$/.test(v) || 'Please enter valid number'
                            //];//this.numberRules;
                            if(QItem.Answer !="")
                            {

                                if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {
                                    if(valConditions[i].LogicOperator == 1 && QItem.ValidationErrorTest == "") //-- 1--> Or Condition
                                    {

                                    }
                                    NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    OldErrorMsg = QItem.ErrorText;
                                     if(OldErrorMsg == "")
                                     {
                                          QItem.ValidationErrorTest = NewErrorMsg;
                                     }
                                     else
                                     {
                                          QItem.ValidationErrorTest= OldErrorMsg
                                     }
                                     QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest = "";
                                }
                            }


                        }
                        else if (valConditions[i].ValidationCondition == 'Should be less than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            //this.minVal = new Date().toISOString().substr(0, 10);
                            QItem.maxVal = new Date().toISOString().substr(0, 10);
                           // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer > this.formatDateTodayDate(today))
                                {
                                        NewErrorMsg = "Should be less than Current date";
                                        OldErrorMsg = QItem.ErrorText;

                                        if(OldErrorMsg == "")
                                        {
                                            QItem.ValidationErrorTest = NewErrorMsg;
                                        }
                                        else
                                        {
                                            QItem.ValidationErrorTest = OldErrorMsg
                                        }
                                        QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest="";
                                }



                        }
                        else if (valConditions[i].ValidationCondition == 'Should be greater than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = new Date().toISOString().substr(0, 10);
                            //this.maxVal = valConditions[i].MaxValue;
                           // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer < this.formatDateTodayDate(today))
                                {
                                        NewErrorMsg = "Should be greater than Current date";
                                        OldErrorMsg = QItem.ErrorText;

                                        if(OldErrorMsg == "")
                                        {
                                            QItem.ValidationErrorTest = NewErrorMsg;
                                        }
                                        else
                                        {
                                            QItem.ValidationErrorTest = OldErrorMsg
                                        }
                                        QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest="";
                                }


                        }
                    }

                }
            },
            CheckQstnValidation(QItem) {

                var returnMsg = "";
                var valConditions = QItem.QuestionCondition;

                if (QItem.IsMandatory) {
                    if (QItem.QuestionType == "Cascading Dropdown") {
                        if (this.cascadingAns == null || this.cascadingAns.length == 0) {
                            returnMsg = "Required Field";
                            this.qRules = "[val => (val || '').length > 0 || 'Required field']";
                            this.chkQstnValid = true;
                        }
                    }
                    else if (QItem.Answer == null || QItem.Answer == "") {
                        returnMsg = "Required Field";
                        this.qRules = "[val => (val || '').length > 0 || 'Required field']";
                        this.chkQstnValid = true;

                    }

                    else if (valConditions != null) {
                        for (var i = 0; i < valConditions.length; i++)
                            if (valConditions[i].ValidationCondition == 'Min & Max Range') {
                                if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {

                                    returnMsg = QItem.ErrorText != '' ? QItem.ErrorText : "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    this.qRules = "";
                                    this.chkQstnValid = true;
                                    QItem.Answer = "";

                                }

                            }
                    }
                    else
                        this.chkQstnValid = false;
                }
                else{
                    if (valConditions != null) {
                        for (var i = 0; i < valConditions.length; i++)
                            if (valConditions[i].ValidationCondition == 'Min & Max Range') {
                                if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {

                                    returnMsg = QItem.ErrorText != '' ? QItem.ErrorText : "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    this.qRules = "";
                                    this.chkQstnValid = true;

                                }
                            }
                    }
                    else{
                        this.chkQstnValid = false;
                    }
                }
                // checking for regex expression
                if(QItem.RegEx !="" && QItem.Answer !="")
                {
                    const regExpStr = QItem.RegEx;//"^([a-z0-9]{5,})$";//
                    const result = new RegExp(regExpStr, 'g').test(QItem.Answer) // here I have used 'g' which means global search
                    if (!result) {
                        QItem.IsValidationError = true;
                        QItem.ValidationErrorTest="Please enter a valid string " + QItem.RegEx;
                    }
                    else
                    {
                        QItem.IsValidationError = false;
                        QItem.ValidationErrorTest="";
                    }
                }

                if (returnMsg != '') {
                    QItem.ErrorText = returnMsg;
                    return true;
                }

                else{
                        QItem.ErrorText ="";
                        return false;
                    }


            },
            scrollToTop(a) {
                if (a) {
                    for (var i = 0; i < a.length; i++) {
                        if (a[i].ValidationErrorTest == "Required Field") {
                            document.getElementById(a[i].QuestionID).scrollIntoView()
                            break;
                        } else {
                            document.getElementById("scrollToTop").scrollIntoView()
                        }
                    }
                } else {
                    document.getElementById("scrollToTop").scrollIntoView()
                }
                //this.$refs.afterClick.focus();
            },

            UpdateBenefLinkedSectionQstnAnswers(benefIndx) {
                
                var benefID = 0;
                if (this.dlgSurveyType)
                    benefID = this.vmSelectedBeneficiaries[benefIndx - 1].BeneficiaryID
                var Ans = "";
                var preAns = [];
                var blobFile = "";
                var count = 0;

				if(this.fnCheckQuestionValidationAndCondition(this.lstSectionQstns,benefID))
				{
					return false;
				}
                if(this.fnCheckRepeatSectionQuestionValidationAndCondition(benefID))
                {
                    return false;
                }

                for (var i = 0; i < this.lstSectionQstns.length; i++) {

                    Ans = "";
                    if (this.lstSectionQstns[i].QuestionID > 0) {
                     //if (this.CheckQstnValidation(this.lstSectionQstns[i]))
                        // if(this.fnCheckQuestionValidationAndCondition(this.lstSectionQstns,benefID))
                            // {
                            //    count++;
                          //  }
                      // else {

                            if (this.lstSectionQstns[i].QuestionType == 'Multi-Choice')
                            {

                                var ChoiceAns =this.multiChoiceAns.filter(x=> x.QuestionID == this.lstSectionQstns[i].QuestionID && x.benefIndex == this.BenefStepper);
                                for (var j = 0; j < ChoiceAns.length; j++)
                                    {
                                        Ans = (Ans != '') ? Ans + ';' + ChoiceAns[j].OptionValue : ChoiceAns[j].OptionValue;
                                    }
                            }
                            else if (this.lstSectionQstns[i].QuestionType == "Cascading Dropdown") {
                                this.cascadingAns = this.cascadingAnsArray.filter(x => (x.QID == this.lstSectionQstns[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));

                                for (var j = 0; j < this.cascadingAns.length; j++)
                                    Ans = (Ans != '') ? Ans + ',' + this.cascadingAns[j].Ans : this.cascadingAns[j].Ans;
                            }

                            else if ((this.lstSectionQstns[i].QuestionType == "Audio Capture") || (this.lstSectionQstns[i].QuestionType == "Signature Capture")
                                || (this.lstSectionQstns[i].QuestionType == "Photo Capture") || (this.lstSectionQstns[i].QuestionType == "File Upload")) {

                                if(this.AnsBlob.length > 0)
                                {
                                    blobFile = this.AnsBlob.filter(x => (x.QuestionID == this.lstSectionQstns[i].QuestionID) && (x.BenefID == benefID))[0];
                                    if(typeof blobFile != "undefined")
                                    {
                                        this.FileUpload(blobFile.blobData, this.lstSectionQstns[i].Answer, false, blobFile.IsUploadedFile, benefID);
                                    }
                                }
                                Ans = this.lstSectionQstns[i].Answer;

                            }

                            else
                                Ans = this.lstSectionQstns[i].Answer;

                            preAns = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.ResponseID == benefID) &&
                                (x.SectionID == this.lstSectionQstns[i].SectionID) && (x.QuestionID == this.lstSectionQstns[i].QuestionID) && (x.RQstInd == this.lstSectionQstns[i].RQstInd));

                            if (preAns.length == 0)
                                {
                                this.lstBenefSurveyQstnAnswers.push({
                                    BeneficiaryInd: benefIndx, ResponseID: benefID, SurveyID: this.lstSectionQstns[i].SurveyID,
                                    SectionID: this.lstSectionQstns[i].SectionID, QuestionID: this.lstSectionQstns[i].QuestionID, Answer: Ans,
                                    FieldVar: this.lstSectionQstns[i].FieldVar, RQstInd: this.lstSectionQstns[i].RQstInd,IsUpload :this.lstSectionQstns[i].IsUpload
                                });
                                }
                            else {

                                for (var j = 0; j < this.lstBenefSurveyQstnAnswers.length; j++) {
                                    if (this.lstBenefSurveyQstnAnswers[j].BeneficiaryInd == preAns[0].BeneficiaryInd && this.lstBenefSurveyQstnAnswers[j].ResponseID == preAns[0].ResponseID
                                        && this.lstBenefSurveyQstnAnswers[j].RQstInd == preAns[0].RQstInd &&
                                        this.lstBenefSurveyQstnAnswers[j].SectionID == preAns[0].SectionID && this.lstBenefSurveyQstnAnswers[j].QuestionID == preAns[0].QuestionID) {
                                        this.lstBenefSurveyQstnAnswers.splice(j, 1);
                                        this.lstBenefSurveyQstnAnswers = [...this.lstBenefSurveyQstnAnswers];
                                    }

                                }

                                this.lstBenefSurveyQstnAnswers.push({
                                    BeneficiaryInd: benefIndx, ResponseID: benefID, SurveyID: this.lstSectionQstns[i].SurveyID,
                                    SectionID: this.lstSectionQstns[i].SectionID, QuestionID: this.lstSectionQstns[i].QuestionID, Answer: Ans,
                                    FieldVar: this.lstSectionQstns[i].FieldVar, RQstInd: this.lstSectionQstns[i].RQstInd,IsUpload :this.lstSectionQstns[i].IsUpload
                                });

                            }

                            this.lstSectionQstns[i].Answer = "";
                            if (this.lstSectionQstns[i].QuestionType == "Multi-Choice") {

                              //  this.multiChoiceAns = [];
                                for (var j = 0; j < this.lstSectionQstns[i].QuestionOptions.length; j++)
                                    this.lstSectionQstns[i].QuestionOptions[j].IsOptSelected = false;
                            }
                            if (this.lstSectionQstns[i].QuestionType == "Cascading Dropdown") {
                                this.cascadingAns = [];
                                for (var j = 0; j < this.lstSectionQstns[i].CascadingQuestionOptions.length; j++) {
                                    this.lstSectionQstns[i].CascadingQuestionOptions[j][0] = null;
                                    this.lstSectionQstns[i].CascadingQuestionOptions[j][j] = null;
                                }
                            }

                            if ((this.lstSectionQstns[i].QuestionType == "Photo Capture") || (this.lstSectionQstns[i].QuestionType == "Signature Capture")
                                || (this.lstSectionQstns[i].QuestionType == "File Upload")) {
                                if (this.lstSectionQstns[i].QuestionType == "Photo Capture") {
                                    this.img = null;
                                    this.uploadedImg = null;
                                    this.startCamera = false;
                                    this.camOn = false;
                                    this.devices = [];
                                    this.deviceId = null;
                                    this.camera = null;
                                    this.audUpload[i] =0;
                                }
                                if (this.lstSectionQstns[i].QuestionType == "Signature Capture") {

                                    this.uploadedSign = null;
                                    this.clearSign(false,this.lstSectionQstns[i].QuestionID);
                                }

                                if (this.lstSectionQstns[i].QuestionType == "File Upload")
                                    this.uploadedFile = null;
                                this.lstSectionQstns[i].AnsBlob = [];

                            }


                            if (this.lstSectionQstns[i].QuestionType == "Audio Capture") {
                                var audioCatpure = this.dlgSurveyType ? document.getElementById("recAudio") : document.getElementById("recAudioOne");

                                if (audioCatpure != null) {
                                    audioCatpure.__vue__._data.recordList = [];
                                    audioCatpure.__vue__._data.recorder.records = [];
                                }
                                this.savedAudio = "";
                            }
                      //  }
                    }
                }

                // Repeat Section Questions and Answers
                var RepeatSection = this.RepeatSection.filter(x=> x.benefIndex == benefIndx)
				for (var i = 0; i < RepeatSection.length; i++) {

                    Ans = "";
                    if (RepeatSection[i].QuestionID > 0) {
                     //if (this.CheckQstnValidation(this.lstSectionQstns[i]))
                        // if(this.fnCheckQuestionValidationAndCondition(this.lstSectionQstns,benefID))
                            // {
                            //    count++;
                          //  }
                      // else {

                            if (RepeatSection[i].QuestionType == 'Multi-Choice')
                            {
                                var ChoiceAns =this.MultiChoiceAnsForRepeat.filter(x=> x.QuestionID == RepeatSection[i].QuestionID && x.RQstInd == RepeatSection[i].RQstInd);
                                for (var j = 0; j < ChoiceAns.length; j++)
                                    {
                                        Ans = (Ans != '') ? Ans + ',' + ChoiceAns[j].OptionValue : ChoiceAns[j].OptionValue;
                                    }
                            }
                            else if (RepeatSection[i].QuestionType == "Cascading Dropdown") {
                                this.cascadingAns = this.cascadingAnsArray.filter(x => (x.QID == RepeatSection[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));

                                for (var j = 0; j < this.cascadingAns.length; j++)
                                    Ans = (Ans != '') ? Ans + ',' + this.cascadingAns[j].Ans : this.cascadingAns[j].Ans;
                            }

                            else if ((RepeatSection[i].QuestionType == "Audio Capture") || (RepeatSection[i].QuestionType == "Signature Capture")
                                || (RepeatSection[i].QuestionType == "Photo Capture") || (RepeatSection[i].QuestionType == "File Upload")) {

                                if(this.AnsBlob.length > 0){
                                blobFile = this.AnsBlob.filter(x => (x.QuestionID == RepeatSection[i].QuestionID) && (x.BenefID == benefID) && (x.RQstInd == RepeatSection[i].RQstInd) )[0];
                                this.FileUpload(blobFile.blobData, RepeatSection[i].Answer, false, blobFile.IsUploadedFile, benefID);
                                }
                                Ans = this.RepeatSection[i].Answer;

                            }

                            else
                                Ans = RepeatSection[i].Answer;

                            //preAns = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.ResponseID == benefID) && (x.SectionID == RepeatSection[i].SectionID) && (x.QuestionID == RepeatSection[i].QuestionID) && (x.RQstInd == RepeatSection[i].RQstInd));
                            preAns = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.ResponseID == benefID) && (x.QuestionID == RepeatSection[i].QuestionID));

                            if (preAns.length == 0)
                            {
                                this.lstBenefSurveyQstnAnswers.push({
                                    BeneficiaryInd: benefIndx, ResponseID: benefID, SurveyID: RepeatSection[i].SurveyID,
                                    SectionID: RepeatSection[i].SectionID, QuestionID: RepeatSection[i].QuestionID, Answer: Ans,
                                    FieldVar: RepeatSection[i].FieldVar, RQstInd: RepeatSection[i].RQstInd,IsUpload :RepeatSection[i].IsUpload
                                });
                                }
                            else {

                                for (var j = 0; j < this.lstBenefSurveyQstnAnswers.length; j++) {
                                    if (this.lstBenefSurveyQstnAnswers[j].BeneficiaryInd == preAns[0].BeneficiaryInd && this.lstBenefSurveyQstnAnswers[j].ResponseID == preAns[0].ResponseID
                                        && this.lstBenefSurveyQstnAnswers[j].RQstInd == preAns[0].RQstInd &&
                                        this.lstBenefSurveyQstnAnswers[j].SectionID == preAns[0].SectionID && this.lstBenefSurveyQstnAnswers[j].QuestionID == preAns[0].QuestionID) {
                                        this.lstBenefSurveyQstnAnswers.splice(j, 1);
                                        this.lstBenefSurveyQstnAnswers = [...this.lstBenefSurveyQstnAnswers];
                                    }

                                }

                                this.lstBenefSurveyQstnAnswers.push({
                                    BeneficiaryInd: benefIndx, ResponseID: benefID, SurveyID: RepeatSection[i].SurveyID,
                                    SectionID: RepeatSection[i].SectionID, QuestionID: RepeatSection[i].QuestionID, Answer: Ans,
                                    FieldVar: RepeatSection[i].FieldVar, RQstInd: RepeatSection[i].RQstInd,IsUpload :RepeatSection[i].IsUpload
                                });

                            }

                            //this.RepeatSection[i].Answer = "";
                      //  }
                    }
                }
                if(count>0) {
                    return false;
                 }
                //$(document).scrollTop(0);
                if (this.dlgSurveyType) {

                    if (this.BenefStepper <= this.vmSelectedBeneficiaries.length) {

                        if (this.BenefQstnStepper[benefIndx] < this.lstSections.length) {
                            this.BenefQstnStepper[benefIndx]++;
                            this.UpdateBenefQstnInput(benefIndx);
                        }

                        else {
                            this.BenefStepper++;
                            if (this.BenefStepper > this.vmSelectedBeneficiaries.length)
                                this.BenefLinkedSurveySteps = 6;
                            else {
                                this.BenefQstnStepper[this.BenefStepper] = 1;
                                this.UpdateBenefQstnInput(this.BenefStepper);

                            }
                        }
                    }
                }
                else {

                    if (this.BenefQstnStepper[benefIndx] < this.lstSections.length) {
                        this.BenefQstnStepper[benefIndx]++;
                        this.UpdateBenefQstnInput(benefIndx);
                    }

                    else {
                        //this.BenefStepper++;
                        this.IsUpdate_BenSurvey = false;
                        this.IsInputFinished = true;
                    }
                }
            },
            PreviousBenefSectionQstnAnswers() {
                if (this.dlgSurveyType) {
                    if (this.BenefStepper > this.vmSelectedBeneficiaries.length) {
                        this.BenefStepper--;
                        this.BenefQstnStepper[this.BenefStepper] = this.lstSections.length;
                    }
                    else
                        this.BenefQstnStepper[this.BenefStepper]--;

                    if (this.BenefQstnStepper[this.BenefStepper] == 0) {
                        this.BenefStepper--;
                        if (this.BenefStepper == 0)
                            this.BenefLinkedSurveySteps = 4;
                        else {
                            this.BenefQstnStepper[this.BenefStepper] = this.lstSections.length;
                            this.UpdateBenefQstnInput(this.BenefStepper);
                        }
                    }
                    else
                        this.UpdateBenefQstnInput(this.BenefStepper);
                }
                else {

                    this.BenefQstnStepper[this.BenefStepper]--;

                    if (this.BenefQstnStepper[this.BenefStepper] == 0) {
                        this.IsUpdate_BenSurvey = false;
                        this.IsInputFinished = false;
                        this.NonBenefLinkedSurveySteps = 2// non beneficiary dailog box steps
                    }
                    else
                        this.UpdateBenefQstnInput(this.BenefStepper);


                }

            },
            async SubmitBenefSurvey() {

                if (this.vmBenefSurveySubmitOption == 1) {
                    this.lstBeneficiary = [];
                    this.BenefLinkedSurveySteps = 1;
                    //this.vmSelectedBeneficiaries = []
                }
                else if (this.vmBenefSurveySubmitOption == 2) {
                    this.varIsSubmitted = 1;
                    this.IsUpdate_BenSurvey = false;
                    this.IsInputFinished = true;
                } else if (this.vmBenefSurveySubmitOption == 3) {
                    this.varIsSubmitted = 0;
                    await this.UpdateTask();
                    await this.ClearBenefSurveyUpdate();

                    //this.dlgSurveyType = false;
                }
            },

            //=========== Import Survey ==========//
            DownloadSampleBeneficiaryLinkedSurveyFile() {

                //objUtils.common.MergeCommas(lines[0]);
                var ColumnHeader = "";
                var ColumnData1 = [];
                var ColumnData = [];
                var StrResponse = "Survey Response here";
                var colValue = "";
                var sectionArr = [];
                this.lstImportSurveyQstnHearder = [];
                for (var i = 0; i < this.lstSections.length; i++) {
                    sectionArr.push({ SectionID: this.lstSections[i].SectionID });
                }
                for (var k = 0; k < sectionArr.length; k++) {
                    var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == sectionArr[k].SectionID));
                    //var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == this.vmSection.SectionID));
                    currSecQstns.sort(function (a, b) {
                        return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
                    });

                    for (var i = 0; i < currSecQstns.length; i++) {

                        currSecQstns[i].IsUpload = 0;
                        this.audUpload[i] = 0;
                        colValue = "Answer" + (i + 1).toString();

                        if (currSecQstns[i].QuestionType == "Date Picker") {
                            this.lstImportSurveyQstnHearder.push({
                                text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                type: "date",
                                mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                QuestionCondition: currSecQstns[i].QuestionCondition,
                                QuestionID: currSecQstns[i].QuestionID,
                            });
                        }
                        else if (currSecQstns[i].QuestionType == "Numeric") {
                            this.lstImportSurveyQstnHearder.push({
                                text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                type: "number",
                                mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                QuestionCondition: currSecQstns[i].QuestionCondition,
                                QuestionID: currSecQstns[i].QuestionID,
                            });
                        }
                        else if (currSecQstns[i].QuestionType == 'Single Choice' || currSecQstns[i].QuestionType == 'Multi-Choice' || currSecQstns[i].QuestionType == 'Dropdown' || currSecQstns[i].QuestionType == 'Likert Scale') {
                            var optionlist = [];
                            for (var j = 0; j < currSecQstns[i].QuestionOptions.length; j++) {
                                optionlist.push(currSecQstns[i].QuestionOptions[j].OptionValue)
                            }
                            this.lstImportSurveyQstnHearder.push({
                                text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                type: "select",
                                mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                options: optionlist,
                                QuestionCondition: currSecQstns[i].QuestionCondition,
                                QuestionID: currSecQstns[i].QuestionID,
                            });
                        }
                        else {
                            this.lstImportSurveyQstnHearder.push({
                                text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                                type: "string",
                                mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                QuestionCondition: currSecQstns[i].QuestionCondition,
                                QuestionID: currSecQstns[i].QuestionID,

                            });
                        }
                    }
                }
                for (var j = 0; j < this.vmSelectedBeneficiaries.length; j++) {
                    ColumnHeader = "Beneficiary";
                    if(this.vmSelectedBeneficiaries[j].BeneficiaryName.includes(','))
                    {
                         ColumnData1.push("\""+this.vmSelectedBeneficiaries[j].BeneficiaryName+"\"");
                    }
                    else
                    {
                        ColumnData1.push(this.vmSelectedBeneficiaries[j].BeneficiaryName);
                    }
                    //var string = objUtils.common.MergeCommas(this.lstImportSurveyQstnHearder.toString());
                    for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++) {

                        if(this.lstImportSurveyQstnHearder[i].text.includes(','))
                        {
                            ColumnHeader += ColumnHeader == "" ? "\""+this.lstImportSurveyQstnHearder[i].text+"\"" : "," + "\""+this.lstImportSurveyQstnHearder[i].text+"\"";
                        }
                        else
                        {
                            ColumnHeader += ColumnHeader == "" ? this.lstImportSurveyQstnHearder[i].text : "," + this.lstImportSurveyQstnHearder[i].text;
                        }

                        if(this.lstImportSurveyQstnHearder[i].type ==  "date"){
                            ColumnData1.push(StrResponse + " *Please enter date in mm-dd-yyyy format.");
                        }else{
                            ColumnData1.push("Survey Response here");
                        }


                    }
                   // var string = objUtils.common.MergeCommas(ColumnHeader);
                    ColumnHeader = ColumnHeader + "\n";
                    ColumnData.push(ColumnData1);
                    ColumnData1 = [];

                    ColumnData.forEach(function (row) {
                        ColumnHeader += row.join(',');
                        ColumnHeader += "\n";
                    });
                }
               // ColumnHeader['Beneficiary'].hidden = true;

                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_surveyQuestionnaire_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            },
            DownloadSampleNonBeneficiaryLinkedSurveyFile() {
                
                var ColumnHeader = "";
                var ColumnData1 = [];
                var ColumnData = [];
             var StrResponse = "Survey Response here";
             var colValue = "";
             var sectionArr = [];
             this.lstImportSurveyQstnHearder = [];
             for (var i = 0; i < this.lstSections.length; i++) {
                 sectionArr.push({ SectionID: this.lstSections[i].SectionID });
             }
             for (var k = 0; k < sectionArr.length; k++) {
                 var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == sectionArr[k].SectionID));
                 //var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == this.vmSection.SectionID));
                 currSecQstns.sort(function (a, b) {
                     return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
                 });

                 for (var i = 0; i < currSecQstns.length; i++) {

                     currSecQstns[i].IsUpload = 0;
                     this.audUpload[i] = 0;
                     colValue = "Answer" + (i + 1).toString();

                     if (currSecQstns[i].QuestionType == "Date Picker") {
                         this.lstImportSurveyQstnHearder.push({
                             text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             type: "date",
                             mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                             QuestionCondition: currSecQstns[i].QuestionCondition,
                             QuestionID: currSecQstns[i].QuestionID,
                         });
                     }
                     else if (currSecQstns[i].QuestionType == "Numeric") {
                         this.lstImportSurveyQstnHearder.push({
                             text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             type: "number",
                             mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                             QuestionCondition: currSecQstns[i].QuestionCondition,
                             QuestionID: currSecQstns[i].QuestionID,
                         });
                     }
                     else if (currSecQstns[i].QuestionType == 'Single Choice' || currSecQstns[i].QuestionType == 'Multi-Choice' || currSecQstns[i].QuestionType == 'Dropdown' || currSecQstns[i].QuestionType == 'Likert Scale') {
                         var optionlist = [];
                         for (var j = 0; j < currSecQstns[i].QuestionOptions.length; j++) {
                             optionlist.push(currSecQstns[i].QuestionOptions[j].OptionValue)
                         }
                         this.lstImportSurveyQstnHearder.push({
                             text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             type: "select",
                             mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                             options: optionlist,
                             QuestionCondition: currSecQstns[i].QuestionCondition,
                             QuestionID: currSecQstns[i].QuestionID,
                         });
                     }
                     else {
                         this.lstImportSurveyQstnHearder.push({
                             text: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             value: currSecQstns[i].QuestionName.concat(' ', currSecQstns[i].QuestionHint),
                             type: "string",
                             mandatory: currSecQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                             QuestionCondition: currSecQstns[i].QuestionCondition,
                             QuestionID: currSecQstns[i].QuestionID,

                         });
                     }
                 }
             }
                    for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++) {

                        if(this.lstImportSurveyQstnHearder[i].text.includes(','))
                        {
                            ColumnHeader += ColumnHeader == "" ? "\""+this.lstImportSurveyQstnHearder[i].text+"\"" : "," + "\""+this.lstImportSurveyQstnHearder[i].text+"\"";
                        }
                        else
                        {
                            ColumnHeader += ColumnHeader == "" ? this.lstImportSurveyQstnHearder[i].text : "," + this.lstImportSurveyQstnHearder[i].text;
                        }
                        //ColumnData1.push("Survey Response here");
                       if(this.lstImportSurveyQstnHearder[i].type ==  "date"){
                            ColumnData1.push(StrResponse + " *Please enter date in mm-dd-yyyy format.");
                        }else{
                            ColumnData1.push("Survey Response here");
                        }

                    }

                    ColumnHeader = ColumnHeader + "\n";
                    ColumnData.push(ColumnData1);
                    ColumnData1 = [];

                    ColumnData.forEach(function (row) {
                        ColumnHeader += row.join(',');
                        ColumnHeader += "\n";
                    });

               // ColumnHeader['Beneficiary'].hidden = true;
                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_surveyNonBeneficiaryQuestionnaire_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            },
            OnSurveyFileUpload() {

              if(this.surveyfileInput != null)
              {
                this.csvSurveyFileName = this.surveyfileInput.name;
                var vm = this
                if (window.FileReader) {
                    var reader = new FileReader();
                    reader.readAsText(this.surveyfileInput);
                    // Handle errors load
                    reader.onload = function (event) {
                        var csv = event.target.result;
                        vm.ReadSurveyCsvFile(csv)
                    };
                    reader.onerror = function (evt) {
                        if (evt.target.error.name == "NotReadableError") {
                            this.Msg = "Cannot read file !";
                            this.infoSnackbarMsg = true;
                        }

                    };
                }
                else {
                    this.Msg = 'FileReader is not supported in this browser.';
                    this.infoSnackbarMsg = true;
                }
              }
               else
                    {
                          this.lstImportSurvey = [];
                    }
            },
            ReadSurveyCsvFile(csv) {
                
                var lines = csv.split(/\r\n|\n/);
                //var headers = lines[0].split(",");
                var headers = objUtils.common.MergeCommas(lines[0]);
                var flag = true;
                var today = new Date().toLocaleString();
                //this.resetVueExcelEditor();
                this.lstImportSurveyQstnHearderN = [];
                 //var optionlist=[];
                //for (var j = 0; j < this.vmSelectedBeneficiaries.length; j++) {
                   //optionlist.push(this.vmSelectedBeneficiaries[j].BeneficiaryName)
               //}
                if(this.vmSelectedBeneficiaries.length>0)
                {
                        this.lstImportSurveyQstnHearderN.push({
                          text: "Beneficiary",
                          value: "Beneficiary",
                          type:"string",
                          mandatory:'Yes',
                           //options:optionlist,
                            QuestionCondition :[],
                        });
                     }
                for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++)
                    this.lstImportSurveyQstnHearderN.push(this.lstImportSurveyQstnHearder[i]);

                if (this.lstImportSurveyQstnHearderN.length == headers.length) {
                    for (var c = 0; c < this.lstImportSurveyQstnHearderN.length; c++) {
                        if (this.lstImportSurveyQstnHearderN[c].text.trim != headers[c].trim) {
                            flag = false;
                            break;
                        }

                    }
                    if (!flag) {
                        this.MSg = ("Survey Questionnaire is not matching for the selected file...  ");
                        this.infoSnackbarMsg = true;
                        this.surveyfileInput = null;
                        this.csvSurveyFileName = "";
                    }

                    else {

                        for (var i = 1; i < lines.length; i++) {
                            var obj = {};
                            //var currentline = lines[i].split(",");
                          var currentline;
                                currentline = objUtils.common.MergeCommas(lines[i]);

                            if (currentline != "") {
                                for (var j = 0; j < headers.length; j++)
                                  {
                                   // obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                     var dataType=this.lstImportSurveyQstnHearderN[j].type
                                     var QuestionCondition =[];
                                     QuestionCondition = this.lstImportSurveyQstnHearderN[j].QuestionCondition != null ? this.lstImportSurveyQstnHearderN[j].QuestionCondition : [];

                                    if (dataType =="number")
                                    {
                                           if(isNaN(currentline[j]) ||(currentline[j]=="" && this.lstImportSurveyQstnHearderN[j].mandatory=='Yes'))
                                            {

                                             obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                            }
                                          else
                                           {
                                             obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                           }

                                        // // validating Question Condition
                                            this.IsvalidexcelData = true;
                                            if(QuestionCondition.length > 0)
                                            {
                                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                {
                                                    if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                                    {
                                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                        if(!currentline[j].match(phoneNum))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                    if (QuestionCondition[Count].ValidationCondition == "Min & Max Range" && currentline[j] != "")
                                                    {
                                                        if(!(parseInt(currentline[j]) > parseInt(QuestionCondition[Count].MinValue) && parseInt(currentline[j]) < parseInt(QuestionCondition[Count].MaxValue)))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                }
                                            }
                                       }
                                    else if(dataType =="date")

                                   {
                                       var dateField=currentline[j];
                                        var timestamp = function(dateField) {
                                      if ( Object.prototype.toString.call(dateField) === "[object Date]" )
                                        return true;
                                      return false;
                                        };

                                        if (timestamp==false ||(currentline[j]=="" && this.lstImportSurveyQstnHearderN[j].mandatory=='Yes'))
                                        {
                                         obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                        }
                                        else
                                        {
                                         obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                        }
                                        // validating Question Condition
                                             this.IsvalidexcelData = true;
                                            if(QuestionCondition.length > 0)
                                            {
                                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                {
                                                    // Date Validation
                                                     if (QuestionCondition[Count].ValidationCondition == "Should be greater than Current date" && currentline[j] != "")
                                                     {
                                                        if (this.formatExcelDate(dateField) < this.formatExcelDate(today))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                         }
                                                     }
                                                     if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "")
                                                     {
                                                        if (this.formatExcelDate(dateField) > this.formatExcelDate(today))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                         }
                                                     }
                                                }
                                            }
                                    }
                                else if(dataType =='select')
                                   {

                                         if((currentline[j] !="" && this.lstImportSurveyQstnHearderN[j].mandatory=='Yes') || (!this.lstImportSurveyQstnHearderN[j].options.includes(currentline[j]) && currentline[j] !=""))
                                        {
                                         obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                        }
                                        else
                                        {
                                         obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                        }
                                    }
                                        else
                                        {
                                        if(currentline[j]=="" && this.lstImportSurveyQstnHearderN[j].mandatory=='Yes')
                                        {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                        }
                                        else
                                        {
                                         obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                         }
                                            // validating Question Condition
                                             this.IsvalidexcelData = true;
                                            if(QuestionCondition.length > 0)
                                            {
                                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                                {
                                                    if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                                    {
                                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                        if(!currentline[j].match(phoneNum))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                             this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                    if (QuestionCondition[Count].ValidationCondition == "Email" && currentline[j] != "")
                                                    {
                                                        var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                                        if(!currentline[j].match(email))
                                                        {
                                                            obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                            this.IsvalidexcelData = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                      }
                                     this.lstImportSurvey.push(obj);
                                    }

                            }
                        }
                    }
                else {
                    this.Msg = ("Survey Questionnaire format is not matching for the selected file...  ");
                    this.infoSnackbarMsg = true;
                    this.surveyfileInput = null;
                    this.csvSurveyFileName = "";
                }
            },
      searchStringInArray () {
            var newList = this.lstImportSurvey.filter(element => {
                for (var property in element) {
                    if (element.hasOwnProperty(property)) {
                        if(element[property] == "Invalid Input") {
                            return true;
                        }
                    }
                }

        });
         if(newList.length>0)
             {
             return false;
             }
         else
             {
             return true;
             }
        },
            async ImportSurvey()
            {
                var benefID = 0;
                var benefName = "";
         if (this.searchStringInArray() == true) {

                for (var i = 0; i < this.lstImportSurvey.length; i++) {

                    for (var j = 0; j < this.lstImportSurveyQstnHearder.length; j++) {
                        if (this.dlgSurveyType) // Beneficiaries linked
						{
                            benefName = this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[0].value];
                            if(typeof this.vmSelectedBeneficiaries.filter(x => x.BeneficiaryName == benefName)[0] != "undefined")
                            {
                                benefID = this.vmSelectedBeneficiaries.filter(x => x.BeneficiaryName == benefName)[0].BeneficiaryID;
                            }
                            else
                            {
                                benefID =-1;
                            }

                        if(benefID == -1)
                        {
                           //Adding New Beneficiaries
                            var beneficiaryTypeID = this.lstSelectedTask.BeneficiaryRegistrationQstns[0].BeneficiaryTypeID
                            var lstBenefRegQstnAnswers = [];
                            this.lstNewBeneficiary.push({
                                BeneficiaryID: benefID,
                                BeneficiaryTypeID: beneficiaryTypeID, BeneficiaryName: benefName,
                                BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                                CreatedDate: new Date()
                            });
                        }

						this.lstBenefSurveyQstnAnswers.push({
                            BeneficiaryInd: i + 1, ResponseID: benefID, SurveyID: this.lstSurveyQstns[j].SurveyID,
                            SectionID: this.lstSurveyQstns[j].SectionID, QuestionID: this.lstImportSurveyQstnHearderN[j].QuestionID,
                            Answer: this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[j + 1].value],
                            FieldVar: this.lstSurveyQstns[j].FieldVar, RQstInd: j
                        });

						}
						else // non -benficiary linked
						{

							this.lstBenefSurveyQstnAnswers.push({
                            BeneficiaryInd: i + 1, ResponseID: benefID, SurveyID: this.lstSurveyQstns[j].SurveyID,
                                SectionID: this.lstSurveyQstns[j].SectionID, QuestionID: this.lstImportSurveyQstnHearderN[j].QuestionID,
                            Answer: this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[j].value],
                            FieldVar: this.lstSurveyQstns[j].FieldVar, RQstInd: j
                        });
						}
					}
                }
                 if (this.dlgSurveyType)
                    {
                     this.BenefLinkedSurveySteps = 6;

                    }
                     else
                       {
                        this.NonBenefLinkedSurveySteps = 3;
                        }
                this.surveyfileInput = null;
                this.lstImportSurvey = [];
              }
                else
                {
                this.Msg = "Please resolve all Invalid Input(s).";
                    this.infoSnackbarMsg = true;

                }
            },
            //====================Questionnaire============================//
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            async FilterSubItem(QItem, OptItem, parentID, IsBenefQstn, benefIndx, IsView, RepeatSectionIndexID) {

                //RepeatSectionIndexID -> shows the index of a question inside a repeat section (variable name -> RepeatSection)
                // if it is not in repeat section RepeatSectionIndexID =-1, if yes RepeatSectionIndexID >=0
                var sublist = [];
                if (IsView)
                    this.cascadingAnsArray = [];


                var benfID = 0;
                if (this.dlgSurveyType){
                    if(this.vmSelectedBeneficiaries.length == 0)
                    {
                        benfID = 0;
                    }
                    else
                    {
                        benfID = this.vmSelectedBeneficiaries[benefIndx - 1].BeneficiaryID;
                    }

                }
                var selectedAnsOption = parentID > 0 ? OptItem.CascadingList.filter(x => x.ID == parentID)[0].Name : "";
                var nextHierarchyOrder = OptItem.HierarchyOrder + 1;

                //var ifExists = this.cascadingAnsArray.filter(x => (x.QID == QItem.QuestionID) && (x.IsBenefQstn == IsBenefQstn)
                //    && (x.HierarchyOrder == OptItem.HierarchyOrder) && (x.BenefID == benfID));

                var ifExists = this.cascadingAnsArray.filter(x => (x.QID == QItem.QuestionID) && (x.IsBenefQstn == IsBenefQstn)
                    && (x.HierarchyOrder == OptItem.HierarchyOrder) && (x.BenefID == benfID) && (x.RepeatSectionIndex == RepeatSectionIndexID));

                if (ifExists.length > 0) {
                    var index =0 ;
                    for (var i = 0; i <= this.cascadingAnsArray.length - 1; i++) {
                            if (this.cascadingAnsArray[i].QID ==  QItem.QuestionID && this.cascadingAnsArray[i].IsBenefQstn == IsBenefQstn && this.cascadingAnsArray[i].HierarchyOrder == OptItem.HierarchyOrder && this.cascadingAnsArray[i].BenefID == benfID && (this.cascadingAnsArray[i].RepeatSectionIndex == RepeatSectionIndexID)) {
                                index = i;
                                break;
                            }
                        }
                    this.cascadingAnsArray.splice(this.cascadingAnsArray.indexOf(this.cascadingAnsArray[index]), 1);
                    this.cascadingAnsArray = [...this.cascadingAnsArray];
                }
                this.cascadingAnsArray.push({ QID: QItem.QuestionID, IsBenefQstn: IsBenefQstn, HierarchyOrder: OptItem.HierarchyOrder, Ans: selectedAnsOption, BenefID: benfID, RepeatSectionIndex : RepeatSectionIndexID});



               // var len = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length;
                var len;
                if(RepeatSectionIndexID == -1) // if the cascading question is not comes under a repeat section , then the value of RepeatSectionIndexID == -1
                {
                    len =  IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length;
                }
                else
                {
                    // Fetching the count/ length of the cascading question option using RepeatSectionIndexID
                   len =  IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.RepeatSection.filter(x => x.QuestionID == QItem.QuestionID && x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.length;
                }
                if (len >= nextHierarchyOrder) {

                   // var nxtList = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder) : this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                    var nxtList =[];
                    if(RepeatSectionIndexID == -1) // if the cascading question is not comes under a repeat section , then the value of RepeatSectionIndexID == -1
                    {
                        nxtList = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder) : this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                    }
                    else
                    {
                         nxtList = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder) : this.RepeatSection.filter(x => x.QuestionID == QItem.QuestionID && x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                    }

                    if (nxtList != null && nxtList.length != 0) {
                        if (nxtList[0].OptionValue == "States")
                            {
                            sublist = this.lstStates.filter(x => x.ParentID == parentID).sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                            }
                        else if (nxtList[0].OptionValue == "District"){
                            sublist = this.lstDistrict.filter(x => x.ParentID == parentID).sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                            }
                        else if (nxtList[0].OptionValue == "SubDistrict"){
                            sublist = this.lstSubDistrict.filter(x => x.ParentID == parentID).sort(function(a,b){var x = a.Name < b.Name? -1:1; return x; });
                            }
                        if (IsBenefQstn){
                            this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.
                                filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                            }
                        else if (!IsBenefQstn && RepeatSectionIndexID == -1){
                            this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.
                                filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                            }
                        else
                            {

                               //QItem.filter(x => x.QuestionID == QItem.QuestionID &&  x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.
                                //filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                                QItem.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList =sublist;
                            }
                    }

                }

            },
            GetGPSCoordinates(QID, IsBenefQstn) {

                var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                if (IsBenefQstn)
                    this.lstBenefRegQstns.filter(x => x.QuestionID == QID)[0].Answer = gpsCoordinates;
                else
                    this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].Answer = gpsCoordinates;

            },

            GetGPSCoordinatesRepeat(QID, IsBenefQstn, RQstInd) {

                var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                if (IsBenefQstn)
                    this.lstBenefRegQstns.filter(x => x.QuestionID == QID)[0].Answer = gpsCoordinates;
                else
                    this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0].Answer = gpsCoordinates;

            },

            //=======Audio Capture=========//

            callback(data) {
                console.debug(data)
            },
            b64toBlob(b64Data, contentType) {
                contentType = contentType || '';
                var sliceSize = 512;
                var byteCharacters = atob(b64Data);
                var byteArrays = [];

                for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                    var slice = byteCharacters.slice(offset, offset + sliceSize);

                    var byteNumbers = new Array(slice.length);
                    for (var i = 0; i < slice.length; i++) {
                        byteNumbers[i] = slice.charCodeAt(i);
                    }

                    var byteArray = new Uint8Array(byteNumbers);

                    byteArrays.push(byteArray);
                }

                var blob = new Blob(byteArrays, { type: contentType });
                return blob;
            },

            //=====Signature Capture=======//

            clearSign(IsBenefQstn, QID) {

                if (IsBenefQstn) {
                    this.$refs.signaturePad1[0].signaturePad.clear();
                    this.$refs.signaturePad1[0].signatureData = [];
                }

                else {
                    //Filter the Question Based on Signature Question Type
                    var SignatureQstnList  =  this.lstSectionQstns.filter(x => x.QuestionType == "Signature Capture")
                    // Getting the index of the signature question
                    const Qindex = SignatureQstnList.findIndex((element, index) => {
                          if (element.QuestionID === QID) {
                            return true
                          }
                      })
                    this.lstSectionQstns.filter(x=> x.QuestionID === QID)[0].uploadedSign = "";
                    this.$refs.signaturePad[Qindex].signaturePad.clear();
                    this.$refs.signaturePad[Qindex].signatureData = [];
                }
            },
            // Clear signature pad for repeat section questions
            clearSignRepeat(IsBenefQstn, QID, RQstInd) {

                if (IsBenefQstn) {
                    this.$refs.signaturePad1[0].signaturePad.clear();
                    this.$refs.signaturePad1[0].signatureData = [];
                }

                else {
                    //Filter the Question Based on Signature Question Type
                    var SignatureQstnList  =  this.RepeatSection.filter(x => x.QuestionType == "Signature Capture")
                    // Getting the index of the signature question
                    const Qindex = SignatureQstnList.findIndex((element, index) => {
                          if (element.QuestionID === QID && x.RQstInd === RQstInd) {
                            return true
                          }
                      })
                    this.$refs.signaturePad[Qindex].signaturePad.clear();
                    this.$refs.signaturePad[Qindex].signatureData = [];
                }
            },


            saveSign(QID, IsBenefQstn, benefIndx, QItem) {

                var prevAns = "";
                var signPad = "";

                if (IsBenefQstn) {
                    this.BTAnsBlobQID = QID;
                    prevAns = this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer;
                    if (prevAns != null)
                        this.delListBT.push({ QuestionID: QID, Answer: prevAns });

                    signPad = this.$refs.signaturePad1[0].signaturePad;
                }
                else {

                    this.AnsBlobQID = QID;
                    prevAns = this.lstSectionQstns.filter(x => x.QuestionID == this.AnsBlobQID)[0].Answer;
                    if (prevAns != null && prevAns != "")
                        this.delList.push({ QuestionID: QID, Answer: prevAns });

                    //Filter the Question Based on Signature Question Type
                    var SignatureQstnList  =  this.lstSectionQstns.filter(x => x.QuestionType == "Signature Capture")
                    // Getting the index of the signature question
                    const Qindex = SignatureQstnList.findIndex((element, index) => {
                          if (element.QuestionID === this.AnsBlobQID) {
                            return true
                          }
                      })
                    signPad = this.$refs.signaturePad[Qindex].signaturePad;
                   // signPad = this.$refs.QItem.signaturePad[0].signaturePad;
                }


                if (signPad.isEmpty()) {
                    this.Msg = "Please provide a signature first.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                else {

                    var dataURL = signPad.toDataURL("image/png");
                    var data = dataURL.split(',')[1];
                    let blobFile = this.b64toBlob(data, 'image/png');
                    this.SaveFile("Signature", blobFile, IsBenefQstn, false, benefIndx);
                }
            },

            //=====Photo Capture=======//

            onStarted(stream) {
                console.log("On Started Event", stream);
            },
            onStopped(stream) {
                console.log("On Stopped Event", stream);
            },
            onStop() {
                this.$refs.webcam.stop();
            },
            onStart() {
                this.$refs.webcam[0].start();
            },
            onStartRepeat() {
               // this.$refs.webcam[0].start();
                if(typeof this.$refs.webcam != "undefined" && this.$refs.webcam.length > 0)
                {
                    this.$refs.webcam[0].start();
                }
            },
            onError(error) {
                console.log("On Error Event", error);
            },
            onCameras(cameras) {
                this.devices = cameras;
                console.log("On Cameras Event", cameras);
            },

            onCameraChange(deviceId) {
                this.deviceId = deviceId;
                this.camera = deviceId;
                console.log("On Camera Change Event", deviceId);
            },
            onCameraChangeRepeat(deviceId){

                var selectedcamera = this.devices.filter(x => x.deviceId != this.deviceId);
                if (typeof selectedcamera !="undefined" && selectedcamera.length > 0) {
                    this.deviceId = selectedcamera[0].deviceId;
                    this.camera = selectedcamera[0].deviceId;
                }
                else {
                    this.deviceId = deviceId;
                    this.camera = deviceId;
                }
                console.log("On Camera Change Event", deviceId);
            },
            fnflipCamera(){

                this.onCameraChange(this.deviceId)
            },
            onCameras1(cameras) {
                this.devices = cameras;
                console.log("On Cameras Event", cameras);
            },
            onCameraChange1(deviceId) {
                this.deviceId = deviceId;
                this.camera = deviceId;
                console.log("On Camera Change Event", deviceId);
            },
            onCaptureRepeat(QID, IsBenefQstn, benefIndx,RQstInd,webcamRef) {
                var data = [];
                var img ="";
                if (IsBenefQstn) {
                    this.BTAnsBlobQID = QID;
                    this.img1 = this.$refs.webcam[0].capture();
                    data = this.img1.split(',')[1];
                }
                else {
                    this.AnsBlobQID = QID;
                    //this.img = this.$refs.webcam[0].capture();
                    this.RepeatSection[RQstInd].img = this.$refs.webcam[0].capture();
                    img = this.$refs.webcam[0].capture();
                    data = img.split(',')[1];
                    this.$refs.webcam[0].stop();
                }
                let blobFile = this.b64toBlob(data, 'image/jpg"');
                this.SaveFileRepeat("Photo", blobFile, IsBenefQstn, false, benefIndx, RQstInd);

            },
            async SaveFileRepeat(fileType, blbFile, IsBenefQstn, IsUploadedFile, benefIndx,RQstInd ) {

                var benefID = 0;
                if (this.dlgSurveyType)
                    benefID = this.vmSelectedBeneficiaries[benefIndx - 1].BeneficiaryID;

                const validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                var fname = "";
                var ansType = "";
                var timestamp = new Date().valueOf();
                var prevData = "";

                switch (fileType) {
                    case "Audio":
                        fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
                        ansType = "Audio Capture";
                        break;

                    case "Signature":
                        fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "Signature Capture";
                        break;
                    case "Photo":
                        fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "Photo Capture";
                        break;

                    case "File":
                        fname = "File_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "File Upload";
                        break;
                }

                if (IsBenefQstn) {
                    prevData = this.BTAnsBlob.filter(x => x.QuestionID == this.BTAnsBlobQID);
                    if (prevData.length > 0) {
                        // this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(this.BTAnsBlobQID), 1);
                        this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(prevData), 1);
                        this.BTAnsBlob = [...this.BTAnsBlob];
                    }


                    if (fileType == "Audio")
                        this.savedAudioBenef = window.URL.createObjectURL(blbFile);
                    else {
                        if (fileType == "Signature") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedSign = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedSign = fr.result

                                });
                            }

                        }
                        if (fileType == "File") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedFile = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedFile = fr.result

                                });
                            }

                        }
                        if (fileType == "Photo") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedImg = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedImg = fr.result

                                });
                            }

                        }

                    }
                    this.BTAnsBlob.push({ QuestionID: this.BTAnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname });
                    this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer = fname;

                }
                else {

                    prevData = this.AnsBlob.filter(x => (x.QuestionID == this.AnsBlobQID) && (x.BenefID == benefID) && (x.RQstInd == RQstInd));
                    if (prevData.length > 0) {
                        this.AnsBlob.splice(this.AnsBlob.indexOf(prevData), 1);
                        this.AnsBlob = [...this.AnsBlob];
                    }

                    if (fileType == "Audio") {
                        this.savedAudio = window.URL.createObjectURL(blbFile);

                    }

                    else {
                        if (fileType == "Signature") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.uploadedSign = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.uploadedSign = fr.result

                                });
                            }

                        }
                        if (fileType == "File") {
                            if (!validImageTypes.includes(blbFile['type']))
                                //this.uploadedFile = null;
                                 this.RepeatSection.filter(x => (x.QuestionID == this.AnsBlobQID && x.RQstInd == RQstInd))[0].uploadedFile = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                   // this.uploadedFile = fr.result
                                    this.RepeatSection.filter(x => (x.QuestionID == this.AnsBlobQID && x.RQstInd == RQstInd))[0].uploadedFile = fr.result;

                                });
                            }

                        }
                        if (fileType == "Photo") {

                            if (!validImageTypes.includes(blbFile['type']))
                                //this.uploadedImg = null;
                                this.RepeatSection.filter(x => (x.QuestionID == this.AnsBlobQID && x.RQstInd == RQstInd))[0].uploadedImg = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    //this.uploadedImg = fr.result
                                    this.RepeatSection.filter(x => (x.QuestionID == this.AnsBlobQID && x.RQstInd == RQstInd))[0].uploadedImg = fr.result

                                });
                            }

                        }

                    }

                    this.AnsBlob.push({ QuestionID: this.AnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, IsUploadedFile: IsUploadedFile, BenefID: benefID, RQstInd : RQstInd });
                    this.RepeatSection.filter(x => (x.QuestionID == this.AnsBlobQID && x.RQstInd == RQstInd))[0].Answer = fname;
                }
            },
            onCapture(QID, IsBenefQstn, benefIndx) {
                var data = [];
                var img = "";
                if (IsBenefQstn) {
                    this.BTAnsBlobQID = QID;
                    this.img1 = this.$refs.webcam[0].capture();
                    data = this.img1.split(',')[1];
                }
                else {
                    this.AnsBlobQID = QID;
                    this.lstSectionQstns.filter(x=> x.QuestionID == QID)[0].img = this.$refs.webcam[0].capture();
                    img = this.$refs.webcam[0].capture();
                    data = img.split(',')[1];
                    this.$refs.webcam[0].stop();

                }
                let blobFile = this.b64toBlob(data, 'image/jpg"');
                this.SaveFile("Photo", blobFile, IsBenefQstn, false, benefIndx);

            },
            async SaveFile(fileType, blbFile, IsBenefQstn, IsUploadedFile, benefIndx) {

                var benefID = 0;
                if (this.dlgSurveyType)
                {
                    benefID = this.vmSelectedBeneficiaries[benefIndx-1].BeneficiaryID;
                }
                const validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                var fname = "";
                var ansType = "";
                var timestamp = new Date().valueOf();
                var prevData = "";

                switch (fileType) {
                    case "Audio":
                        fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
                        ansType = "Audio Capture";
                        break;

                    case "Signature":
                        fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "Signature Capture";
                        break;
                    case "Photo":
                        fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "Photo Capture";
                        break;

                    case "File":
                        fname = "File_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                        ansType = "File Upload";
                        break;
                }

                if (IsBenefQstn) {
                    prevData = this.BTAnsBlob.filter(x => x.QuestionID == this.BTAnsBlobQID);
                    if (prevData.length > 0) {
                        // this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(this.BTAnsBlobQID), 1);
                        this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(prevData), 1);
                        this.BTAnsBlob = [...this.BTAnsBlob];
                    }


                    if (fileType == "Audio")
                        this.savedAudioBenef = window.URL.createObjectURL(blbFile);
                    else {
                        if (fileType == "Signature") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedSign = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedSign = fr.result

                                });
                            }

                        }
                        if (fileType == "File") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedFile = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedFile = fr.result

                                });
                            }

                        }
                        if (fileType == "Photo") {
                            if (!validImageTypes.includes(blbFile['type']))
                                this.benefUploadedImg = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    this.benefUploadedImg = fr.result

                                });
                            }

                        }

                    }
                    this.BTAnsBlob.push({ QuestionID: this.BTAnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname });
                    this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer = fname;

                }
                else {

                    prevData = this.AnsBlob.filter(x => (x.QuestionID == this.AnsBlobQID) && (x.BenefID == benefID));
                    if (prevData.length > 0) {
                        this.AnsBlob.splice(this.AnsBlob.indexOf(prevData), 1);
                        this.AnsBlob = [...this.AnsBlob];
                    }

                    if (fileType == "Audio") {
                        this.savedAudio = window.URL.createObjectURL(blbFile);

                    }

                    else {
                        if (fileType == "Signature") {

                            if (!validImageTypes.includes(blbFile['type']))
                                //this.uploadedSign = null;
                                 this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID  ))[0].uploadedSign = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                   // this.uploadedSign = fr.result
                                   this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedSign = fr.result;
                                });

                            }

                        }
                        if (fileType == "File") {

                            if (!validImageTypes.includes(blbFile['type']))
                                this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID  ))[0].uploadedFile = null;
                                //this.uploadedFile = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                    //this.uploadedFile = fr.result
                                    this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedFile = fr.result;
                                });
                            }

                        }
                        if (fileType == "Photo") {
                            if (!validImageTypes.includes(blbFile['type']))
                              //  this.uploadedImg = null;
                                    this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID  ))[0].uploadedImg = null;
                            else {
                                const fr = new FileReader();
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('load', () => {
                                   // this.uploadedImg = fr.result
                                   this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID  ))[0].uploadedImg = fr.result;
                                });
                            }

                        }

                    }

                    this.AnsBlob.push({ QuestionID: this.AnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, IsUploadedFile: IsUploadedFile, BenefID: benefID ,RQstInd : -1});
                    this.lstSectionQstns.filter(x => (x.QuestionID == this.AnsBlobQID))[0].Answer = fname;
                }
            },

            async FileUpload(pblobFileInput, FileName, IsBenefQstn, isUploadedFile, benfID) {

                var TaskDetails = this.lstMyTask.filter(x => (x.TaskType == this.TaskType) && (x.TaskTypeID == this.TaskTypeID))[0];
                var task = this.lstTaskList.filter(x => (x.WFLevel == 0) && (x.Type == this.TaskType) && (x.TypeID == this.TaskTypeID) && (x.TaskDate == this.TaskDate))[0];

                if (pblobFileInput != null) {
                    if (IsBenefQstn) {
                        await dbImworks.TbTaskBenefRegDocument.put({

                            SyncTaskID: task.SyncDBID, TaskDate: this.TaskDate, Type: this.TaskType, TypeID: this.TaskTypeID, WFLevel: 0,
                            AttachedDocument: pblobFileInput, FileName: FileName, DocumentTypeIcon: FileName.split('.').pop(),
                            ProjectID: TaskDetails.ProjectId, DocLinkedTo: TaskDetails.TaskName, IsUploadedFile: isUploadedFile
                        });
                        this.blobFileInputBT = null;
                    }
                    else {
                        await dbImworks.TbTaskResponseDocument.put({

                            SyncTaskID: task.SyncDBID, TaskDate: this.TaskDate, Type: this.TaskType, TypeID: this.TaskTypeID, WFLevel: 0,
                            AttachedDocument: pblobFileInput, FileName: FileName, DocumentTypeIcon: FileName.split('.').pop(),
                            ProjectID: TaskDetails.ProjectId, DocLinkedTo: TaskDetails.TaskName, IsUploadedFile: isUploadedFile,
                            BeneficiaryID: benfID
                        });

                        this.blobFileInput = null;
                    }
                }


            },

            async DownloadDocument(docName, docUName) {

                var downloadFileName = docName;
                var fileUrl = window.SERVER_URL + "/Upload/Documents/" + docUName;

                fetch(fileUrl).then(function (t) {
                    return t.blob().then((b) => {
                        var a = document.createElement("a");
                        a.href = URL.createObjectURL(b);
                        a.setAttribute("download", downloadFileName);
                        a.click();
                    });
                });
            },
            //==============Beneficiary Reg QstnAnswer ==========================//
            GetMultiChoice_BT(optItem, optSelected, optVal) {

                if (optSelected)
                    this.multiChoiceAnsBT.push(optVal);
                else
                    this.multiChoiceAnsBT.splice(this.multiChoiceAnsBT.indexOf(optItem), 1);

            },
            async SaveAudioBT(data) {

                this.blobFileInputBT = data.blob;
                this.SaveFile("Audio", this.blobFileInputBT, true, false, 0);
            },

            //================Survey Qstn Answer ================================//

            GetMultiChoice(optItem, optSelected, optVal, QuestionID,benefIndex) {
               if (optSelected)
               {
                    this.multiChoiceAns.push({OptionValue:optVal, QuestionID : QuestionID,benefIndex : benefIndex});
               }
               else
               {
                   const index = this.multiChoiceAns.findIndex((element, index) => {
                   if (element.QuestionID === optItem.QuestionID && element.OptionValue == optVal && element.benefIndex == benefIndex ) {
                           return true
                       }});
                   this.multiChoiceAns.splice(index,1);
               }
            },
            GetMultiChoiceRepeat(optItem, optSelected, optVal, RQstInd, QuestionID,benefIndex) {
                if (optSelected){
                    optItem.RQstInd = RQstInd;
                    this.MultiChoiceAnsForRepeat.push(optItem);}
               else{
                   this.MultiChoiceAnsForRepeat.splice(this.MultiChoiceAnsForRepeat.indexOf(optItem), 1);}

               //if (optSelected)
               //{
               //     this.MultiChoiceAnsForRepeat.push({OptionValue:optVal, QuestionID : QuestionID,benefIndex : benefIndex,RQstInd : RQstInd});
               //}
               //else
               //{
               //    const index = this.MultiChoiceAnsForRepeat.findIndex((element, index) => {
               //    if (element.QuestionID === optItem.QuestionID && element.OptionValue == optVal && element.benefIndex == benefIndex && element.RQstInd == RQstInd) {
               //            return true
               //        }});
               //    this.MultiChoiceAnsForRepeat.splice(index,1);
              //sssss }
            },
            async SaveAudio(data) {

                this.blobFileInput = data.blob;
                this.SaveFile("Audio", this.blobFileInput, false, false, this.BenefStepper);
            },
			async SaveAudioRepeat(data) {

                this.blobFileInput = data.blob;
                this.SaveFileRepeat("Audio", this.blobFileInput, false, false, this.BenefStepper, this.RQstInd);
            },
            async playSound() {

                if (this.savedAudio) {
                    var audio = new Audio(this.savedAudio);
                    audio.play();
                }
            },
            //===========Question Calculation ====================//
            async Calculate(QID, benefIndx) {

                var resultVal = 0;
                var fieldVar = '';
                var lstfieldVar = [];
                var formulaExp = "";
                if(typeof this.lstSectionQstns.filter(x => x.QuestionID == QID)[0] != 'undefined')
                {
                if (this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].IsFormulaQuestion) {
                    var FormulaQuestion = this.lstSectionQstns.filter(x => x.QuestionID == QID)[0];
                    formulaExp = FormulaQuestion.FormulaExpression;

                    if(FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate' || FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Subtraction'|| FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Average' || FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Multiplication')
                    {
                        this.RepeatVal_Aggregate(QID,benefIndx,FormulaQuestion.FormulaOperation);
                        return false;
                    }
                    if (FormulaQuestion.FormulaFieldVariables != "") {
                        var arr = FormulaQuestion.FormulaFieldVariables.split(",");
                        if (arr.length == 1) {
                            if (this.lstBenefSurveyQstnAnswers.length > 0) {
                                if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[0])).length > 0)
                                    lstfieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[0]));
                            }
                            for (var i = 0; i < lstfieldVar.length; i++)
                                fieldVar = fieldVar == "" ? lstfieldVar[i].Answer : fieldVar + "," + lstfieldVar[i].Answer;
                            fieldVar = "'" + fieldVar + "'";
                            formulaExp = formulaExp.replace(arr[0], fieldVar);
                            fieldVar = '';

                        }
                        else {
                            for (var i = 0; i < arr.length; i++) {

                                //if (this.lstBenefSurveyQstnAnswers.length > 0) {
                                //   if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i])).length > 0)
                                //        fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer;
                                // }

                                if (fieldVar == null || fieldVar == '')
                                 {
                                      if(typeof this.lstSectionQstns.filter(x => x.FieldVar == arr[i])[0]!= "undefined")
                                      {
                                         fieldVar = this.lstSectionQstns.filter(x => x.FieldVar == arr[i])[0].Answer;
                                      }
                                      if(typeof this.RepeatSection.filter(x => x.FieldVar == arr[i])[0]!= "undefined")
                                      {
                                         fieldVar = this.RepeatSection.filter(x => x.FieldVar == arr[i])[0].Answer;
                                      }
                                      if(fieldVar == null || fieldVar == '')
                                      {
                                         //fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer
                                         if (this.lstBenefSurveyQstnAnswers.length > 0) {
                                            if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i])).length > 0)
                                                fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer;
                                        }
                                      }
                                }
                                var FieldVarAns =  fieldVar == "" ? 0: fieldVar; //fieldVar ?"":0
                                formulaExp = formulaExp.replace(arr[i], FieldVarAns);
                                fieldVar = '';
                            }
                        }

                        var strFn = eval(formulaExp);
                        resultVal = eval(strFn);
                        this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].Answer = resultVal;
                    }
                }
              }
            },
            RepeatVal_Aggregate(QID, benefIndex, FormulaOperation)
            {
                 var formulaExp = "";
                 var resultVal = 0;

                if(this.RepeatSection.length > 0)
                {

                    if (this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].IsFormulaQuestion)
                    {
                        var FormulaQuestion = this.lstSectionQstns.filter(x => x.QuestionID == QID)[0];
                        formulaExp = FormulaQuestion.FormulaExpression;
                        if (FormulaQuestion.FormulaFieldVariables != "")
                        {
                            var arr = FormulaQuestion.FormulaFieldVariables.split(",");
                            for(var CondQst=0 ; CondQst <= arr.length-1; CondQst++)
                            {
                                if(typeof this.RepeatSection.filter(x => x.FieldVar == arr[CondQst])[0]!= "undefined" || typeof this.lstSectionQstns.filter(x => x.FieldVar == arr[CondQst])[0]!= "undefined" )
                                {
                                   var RepeatSection = this.RepeatSection.filter(x => x.FieldVar == arr[CondQst] && x.benefIndex == benefIndex);
                                   if(RepeatSection.length == 0)//this.lstSectionQstns
                                   {
                                      RepeatSection = this.lstSectionQstns.filter(x => x.FieldVar == arr[CondQst]);
                                   }
                                   if(RepeatSection.length >0)
                                   {
                                        if(FormulaOperation == "RepeatVal_Aggregate" || FormulaOperation == "RepeatVal_Aggregate_Average") // Calculating Repeat Section Sum / Total
                                        {
                                            for(var i = 0; i<= RepeatSection.length-1;i++)
                                            {
                                                var FieldVarAns =  RepeatSection[i].Answer == "" ? 0: RepeatSection[i].Answer; //fieldVar ?"":0
                                                resultVal = resultVal+Number(FieldVarAns);
                                            }
                                        }
                                        if(FormulaOperation == "RepeatVal_Aggregate_Subtraction")// Calculating Repeat Section Substraction
                                        {
                                            for(var i = 0; i<= RepeatSection.length-1;i++)
                                            {
                                                if(CondQst == 0)
                                                {
                                                   resultVal =RepeatSection[i].Answer == "" ? 0: Number(RepeatSection[i].Answer)
                                                }
                                                else
                                                {
                                                    var FieldVarAns =  RepeatSection[i].Answer == "" ? 0: RepeatSection[i].Answer; //fieldVar ?"":0
                                                    resultVal = resultVal - Number(FieldVarAns);
                                                }

                                            }
                                        }
                                        if(FormulaOperation == "RepeatVal_Aggregate_Multiplication")// Calculating Repeat Section Substraction
                                        {
                                            for(var i = 0; i<= RepeatSection.length-1;i++)
                                            {
                                                if(CondQst == 0 && i== 0)
                                                {
                                                   resultVal =RepeatSection[i].Answer == "" ? 0: Number(RepeatSection[i].Answer)
                                                }
                                                else
                                                {
                                                    var FieldVarAns =  RepeatSection[i].Answer == "" ? 0: RepeatSection[i].Answer; //fieldVar ?"":0
                                                    resultVal = resultVal*Number(FieldVarAns);
                                                }

                                            }
                                        }
                                   }
                                }
                            }
                        }
                    }
                    if(FormulaOperation == "RepeatVal_Aggregate_Average")
                    {
                        resultVal = resultVal /RepeatSection.length
                    }
                    this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].Answer = resultVal;
                }
            },
			async AddSectionLogicForMultiChoice(item,QuestionID,benefIndex,QuestionOptions)
            {


               // if(item.IsOptSelected == true)
               // {
                    if( typeof this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0] != 'undefined')
                    {

                        //QInput = this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].Answer;
                        //SecID = this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].SectionID;
                        var SectionLogic = this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].SectionLogic
                        if(SectionLogic.length > 0)
                        {
                           var SelectedOption = QuestionOptions.filter(x=> x.IsOptSelected == true);
                           var IsLogicExists = SectionLogic.filter(function (o1) {
                                return SelectedOption.some(function (o2) {
                                return o1.Answer === o2.OptionValue; // return the ones with equal id
                            });});
                           //var IsLogicExists = SectionLogic.filter(x=> x.Answer == item.OptionValue)
                           if(IsLogicExists.length > 0)
                           {
                                this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].Answer = IsLogicExists[0].Answer;
                                var IsExistInSkipLogic = this.RepeatSection.filter(x=> x.ParentQuestionID  == QuestionID && x.benefIndex == benefIndex);
                                if(IsExistInSkipLogic.length == 0)
                                {
                                    await this.AddSectionLogic(QuestionID,benefIndex);
                                }
                                this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].Answer = "";
                           }
                           else
                           {
                               this.lstSectionQstns.filter(x => x.QuestionID == QuestionID)[0].Answer = ""
                               await this.AddSectionLogic(QuestionID,benefIndex);
                           }
                        }
                    }
                //}

            },
            async AddSectionLogic(QID, benefIndex)
            {

                //debugger;
                var IsLogicCondition = false;
                var QInput ="";
                var SecID;
                var SectionIndex = -1;
                if( typeof this.lstSectionQstns.filter(x => x.QuestionID == QID)[0] != 'undefined')
                {
                   QInput = this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].Answer;
                   SecID = this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].SectionID;
                }

                if (typeof QInput != 'undefined' && QInput != null) {
                    if (this.lstSectionQstns.filter(x => x.QuestionID == QID)[0].IsConditionQuestion)
                    {
                        var Data = this.lstSectionQstns;
                        var lstSectionLogic = this.lstSectionQstns.filter(x => x.QuestionID == QID && x.SectionID == SecID)[0].SectionLogic;
                        var SectionLogic = lstSectionLogic.filter(x => x.QuestionID == QID);
                        if (SectionLogic != null)
                        {
                            if (SectionLogic[0].LogicType == "Skip Logic")
                            {
                                var IsANDoRLogic = SectionLogic.filter(x=> x.LogicOperator == "AND" || x.LogicOperator == "OR") ;
                                if(IsANDoRLogic.length == 0){
                                 SectionLogic = SectionLogic.filter(x=> x.QuestionID == QID);
                                }

                                SectionLogic.sort(function(a,b){
                                        return a.SkipSectionID - b.SkipSectionID;
                                        }
                                    );
                                // New Skip Logic Code
                                var IsLogicSatisfied = false;
                                var SkipSectionID = 0;
                                for(var LogicCnt = 0 ; LogicCnt <= SectionLogic.length-1 ; LogicCnt++)
                                {
                                    SkipSectionID = SectionLogic[LogicCnt].SkipSectionID;
                                    if (SectionLogic[LogicCnt].ConditionValue == "=")
                                    {
                                        SectionLogic[LogicCnt].ConditionValue ="==";
                                    }
                                    // Get the Input Given by the user
                                    //var InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.QuestionID == QID));
                                    var InputAnswer = "";//this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0].Answer;
                                    if(typeof this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0] != 'undefined')
                                    {
                                        InputAnswer = this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0].Answer;
                                    }
                                    if(InputAnswer == "" && typeof this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0] != 'undefined')
                                    {
                                        InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0].Answer;
                                    }
                                    var InputStringAnswer = SectionLogic[LogicCnt].Answer;
                                    // checking the first condition
                                    if(LogicCnt == 0)
                                    {
                                        if(InputAnswer !="")
                                        {
                                            if(isNaN(InputAnswer)) // is a string
                                            {
                                                InputAnswer = "'" + InputAnswer + "'";
                                                InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                            }
                                            else
                                            {
                                                InputAnswer = InputAnswer;
                                            }
                                            if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                            {
                                                IsLogicSatisfied = true;
                                                SectionIndex = LogicCnt;
                                            }
                                            else
                                            {
                                                IsLogicSatisfied = false;
                                                SectionIndex = -1;
                                            }
                                        }
										else
										{
											IsLogicSatisfied = false;
                                            SectionIndex = -1;
										}
                                    }
                                    else
                                    {
										var LogicOperator = SectionLogic[LogicCnt].LogicOperator; // the variable shows AND Or OR (&& , ||)
										if(LogicOperator == "AND")
										{
											if (IsLogicSatisfied == true)
											{
												if(InputAnswer !="")
												{
													if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
													if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
													{
														IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
													}
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = -1;
                                                    }
												}
												else
												{
													IsLogicSatisfied = false;
                                                    SectionIndex = -1;
												}
											}
										}
                                        if(LogicOperator == "OR")
                                        {
                                            if(IsLogicSatisfied == false)
                                            {
                                                if(InputAnswer !="")
                                                {
                                                    if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
                                                    if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                                    {
                                                        IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = -1;
                                                }
                                            }
                                        }
                                        // if there no logic operator
                                        if(LogicOperator == "" && LogicCnt > 0 && IsLogicSatisfied == false)
                                        {
                                           // if(IsLogicSatisfied == false)
                                           // {
                                                if(InputAnswer !="")
                                                {
                                                    if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
                                                    if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                                    {
                                                        IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = -1;
                                                }
                                            //}
                                        }
                                    }
                                    if(LogicCnt <= SectionLogic.length-1 )
                                    {
                                        var Count = LogicCnt;
                                        var AddLogic = false;
                                        if(Count == SectionLogic.length-1)
                                        {
                                           AddLogic = true;
                                           Count = -1;
                                        }
                                        if(SkipSectionID != SectionLogic[Count+1].SkipSectionID)
                                        {
                                                AddLogic = true;
                                        }

                                        if(AddLogic == true)
                                        {
                                            var QuestionID = QID ;
                                            // Print Skip Section  id Starts
                                            if(IsLogicSatisfied == true)
                                            {

                                                if( SectionLogic[SectionLogic.length-1].LogicOperator == "AND") // if Last logic condition is 'AND' then, Print the skip logic section under the last condition question
                                                {
                                                    QuestionID = SectionLogic[SectionLogic.length-1].QuestionID;
                                                }
                                                var SkipSection ="";
                                                if(SectionIndex >= 0)
                                                {
                                                    SkipSection = SectionLogic[SectionIndex].SkipSectionID;
                                                }
                                                else
                                                {
                                                    SkipSection = SectionLogic[0].SkipSectionID;
                                                }

                                                var IsExistSkipLogic = this.RepeatSection.filter(x=> x.ParentQuestionID === QuestionID && x.benefIndex == benefIndex);
                                                for(var i = 0 ; i<= IsExistSkipLogic.length-1 ; i++)
                                                    {
                                                         const index = this.RepeatSection.findIndex((element, index) => {
                                                            if (element.ParentQuestionID === IsExistSkipLogic[i].ParentQuestionID && element.benefIndex == benefIndex) {
                                                            return true
                                                            }
                                                        });
                                                        this.RepeatSection.splice(index, 1);
                                                    }
                                                this.fnGetRepeatSectionBasedOnQuestion(benefIndex, 1,QuestionID, SkipSection,0);
                                            }
                                            else
                                            {
                                              //  this.RepeatSection =[];
                                                var IsExistSkipLogic = this.RepeatSection.filter(x=> x.ParentQuestionID === QuestionID && x.benefIndex == benefIndex && x.SectionID == SkipSectionID);
                                                for(var i = 0 ; i<= IsExistSkipLogic.length-1 ; i++)
                                                    {
                                                        const index = this.RepeatSection.findIndex((element, index) => {
                                                            if (element.ParentQuestionID === IsExistSkipLogic[i].ParentQuestionID && element.benefIndex == benefIndex && element.SectionID == SkipSectionID)                     {
                                                            return true
                                                            }
                                                        });
                                                        this.RepeatSection.splice(index, 1);
                                                   }
                                                var SkipSection = SectionLogic[0].SkipSectionID;
                                                this.fnGetRepeatSectionBasedOnQuestion(benefIndex, 0,QID, SkipSectionID,0);
                                            }
                                            // if ends
                                        }
                                    }
                                    // For lop end
                                }

                            }
                            else
                            {
                                if (SectionLogic[0].LogicType == "Repeat Logic")
                                {
                                    if(SectionLogic.length > 0)
                                    {
                                        for(var nCnt = 0 ; nCnt <= SectionLogic.length-1; nCnt++)
                                        {
                                           var RepeatSectionID = SectionLogic[nCnt].SkipSectionID;
                                           this.fnGetRepeatSectionBasedOnQuestion(benefIndex, parseInt(QInput),QID, RepeatSectionID,0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            // skip logic for skip section
            async AddSectionLogicRepeat(QID, benefIndex,RQstInd,ParentQuestionID)
            {


                var IsLogicCondition = false;
                var QInput ="";
                var SecID;
                var SectionIndex = -1;
                var ParentID  = 0;
                if(typeof ParentQuestionID == "undefined")
                {
                  ParentID = 0;
                }else{ ParentID = ParentQuestionID}
                if( typeof this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0] != 'undefined')
                {
                   QInput = this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0].Answer;
                   SecID = this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0].SectionID;
                }
                if (typeof QInput != 'undefined' && QInput != null && QInput != "") {
                    if (this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0].IsConditionQuestion)
                    {
                        var SectionLogic = this.RepeatSection.filter(x => x.QuestionID == QID && x.RQstInd == RQstInd)[0].SectionLogic;
                        if (SectionLogic != null)
                        {
                            if (SectionLogic[0].LogicType == "Skip Logic")
                            {


                                // New Skip Logic Code
                                var IsLogicSatisfied = false

                                for(var LogicCnt = 0 ; LogicCnt <= SectionLogic.length-1 ; LogicCnt++)
                                {
                                    if (SectionLogic[LogicCnt].ConditionValue == "=")
                                    {
                                        SectionLogic[LogicCnt].ConditionValue ="==";
                                    }
                                    // Get the Input Given by the user
                                    //var InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.QuestionID == QID));
                                    var InputAnswer = "";//this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0].Answer;
                                    if(typeof this.RepeatSection.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID && x.RQstInd == RQstInd )[0] != 'undefined')
                                    {
                                        InputAnswer = this.RepeatSection.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID  && x.RQstInd == RQstInd)[0].Answer;
                                    }
                                    if(InputAnswer == "" && typeof this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0] != 'undefined')
                                    {
                                        InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0].Answer;
                                    }
                                    var InputStringAnswer = SectionLogic[LogicCnt].Answer;
                                    // checking the first condition
                                    if(LogicCnt == 0)
                                    {
                                        if(InputAnswer !="")
                                        {
                                            if(isNaN(InputAnswer)) // is a string
                                            {
                                                InputAnswer = "'" + InputAnswer + "'";
                                                InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                            }
                                            else
                                            {
                                                InputAnswer = InputAnswer;
                                            }
                                            if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                            {
                                                IsLogicSatisfied = true;
                                                SectionIndex = LogicCnt;
                                            }
                                            else
                                            {
                                                IsLogicSatisfied = false;
                                                SectionIndex = LogicCnt;
                                            }
                                        }
										else
										{
											IsLogicSatisfied = false;
                                            SectionIndex = LogicCnt;
										}
                                    }
                                    else
                                    {
										var LogicOperator = SectionLogic[LogicCnt].LogicOperator; // the variable shows AND Or OR (&& , ||)
										if(LogicOperator == "AND")
										{
											if (IsLogicSatisfied == true)
											{
												if(InputAnswer !="")
												{
													if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
													if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
													{
														IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
													}
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = LogicCnt;
                                                    }
												}
												else
												{
													IsLogicSatisfied = false;
                                                    SectionIndex = LogicCnt;
												}
											}
										}
                                        if(LogicOperator == "OR")
                                        {
                                            if(IsLogicSatisfied == false)
                                            {
                                                if(InputAnswer !="")
                                                {
                                                    if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
                                                    if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                                    {
                                                        IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                }
                                                else
                                                {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = LogicCnt;
                                                }
                                            }
                                        }
                                        // if there no logic operator
                                        if(LogicOperator == "" && LogicCnt > 0 && IsLogicSatisfied == false)
                                        {
                                           // if(IsLogicSatisfied == false)
                                           // {
                                                if(InputAnswer !="")
                                                {
                                                    if(isNaN(InputAnswer)) // is a string
													{
														InputAnswer = "'" + InputAnswer + "'";
                                                        InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
													}
													else
													{
														InputAnswer = InputAnswer;
													}
                                                    if(eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer))
                                                    {
                                                        IsLogicSatisfied = true;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                    else
                                                    {
                                                        IsLogicSatisfied = false;
                                                        SectionIndex = LogicCnt;
                                                    }
                                                }
                                                else
                                                {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = LogicCnt;
                                                }
                                            //}
                                        }
                                    }
                                }
                                if(IsLogicSatisfied == true)
                                {
                                    var QuestionID = QID ;
                                    if( SectionLogic[SectionLogic.length-1].LogicOperator == "AND") // if Last logic condition is 'AND' then, Print the skip logic section under the last condition question
                                    {
                                        QuestionID = SectionLogic[SectionLogic.length-1].QuestionID;
                                    }
                                    var SkipSection ="";
                                    if(SectionIndex >= 0)
                                    {
                                        SkipSection = SectionLogic[SectionIndex].SkipSectionID;
                                    }
                                    else
                                    {
                                        SkipSection = SectionLogic[0].SkipSectionID;
                                    }
                                    // Finding skip logic section of a question already there in RepeatSection, if yes we need to remove from the array
                                    var TempExistData = this.RepeatSection.filter(x=> x.ParentQuestionID === QuestionID && x.benefIndex == benefIndex);
                                    if(TempExistData.length > 0)
                                    {
                                        for(var i = 0 ; i<= TempExistData.length-1 ; i++)
                                        {
                                             const index = this.RepeatSection.findIndex((element, index) => {
                                                if (element.ParentQuestionID === TempExistData[i].ParentQuestionID && element.benefIndex == benefIndex) {
                                                return true
                                                }
                                            });
                                            this.RepeatSection.splice(index, 1);
                                        }

                                    }
                                    this.fnGetRepeatSectionBasedOnQuestion(benefIndex, 1,QuestionID, SkipSection,ParentID);
                                }
                                else
                                {
                                    var QuestionID = QID ;
                                    var SkipSection ="";

                                    var skipSectionID  = SectionLogic.filter(x=>x.QuestionID == QuestionID)[0].SkipSectionID

                                    // Finding skip logic section of a question already there in RepeatSection, if yes we need to remove from the array
                                    var TempExistData = this.RepeatSection.filter(x=> x.SectionID  === skipSectionID && x.benefIndex == benefIndex);
                                    if(TempExistData.length > 0)
                                    {
                                        for(var i = 0 ; i<= TempExistData.length-1 ; i++)
                                        {
                                             const index = this.RepeatSection.findIndex((element, index) => {
                                                if (element.SectionID === skipSectionID) {
                                                return true
                                                }
                                            });
                                            this.RepeatSection.splice(index, 1);
                                        }

                                    }
                                    this.fnGetRepeatSectionBasedOnQuestion(benefIndex, 0,QID, SkipSection,0);
                                }
                            }
                            else
                            {
                                if (SectionLogic[0].LogicType == "Repeat Logic")
                                {
                                    if(SectionLogic.length > 0)
                                    {
                                        for(var nCnt = 0 ; nCnt <= SectionLogic.length-1; nCnt++)
                                        {
                                           var RepeatSectionID = SectionLogic[nCnt].SkipSectionID;
                                           this.fnGetRepeatSectionBasedOnQuestion(benefIndex, parseInt(QInput),QID, RepeatSectionID,0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },

            //================================================//
            fnGetRepeatSectionBasedOnQuestion(benefIndex, RepeatCount,QuestionID,RepeatSectionID,ParentQID )
            {

                //this.lstSectionQstns = [];
                //this.RepeatSection =[];
                // checking the same section question already exist or not, if exist remove
                //for (var i = 0; i <= this.RepeatSection.length-1; i++) {
                //if(benefIndex == 0)

                 for(var i = this.RepeatSection.length-1; i>=0; i--){
                    var obj = this.RepeatSection[i];
                    if (obj.SectionID == RepeatSectionID && obj.benefIndex == benefIndex ) {
                        this.RepeatSection.splice(i, 1);
                    }
                }

                var HirachyParentQID = ParentQID;
                if(HirachyParentQID == 0){
                    HirachyParentQID = QuestionID;
                }
                var currSecQstns = this.lstSurveyQstns.filter(x => (x.SectionID == RepeatSectionID));
                var SectionDetails = this.SectionDetails.filter(x=> x.SectionID == RepeatSectionID);
                var cnt = 0;


                var BeneficiaryID = 0;
                if (this.dlgSurveyType){
                    BeneficiaryID = this.vmSelectedBeneficiaries[benefIndex - 1].BeneficiaryID;}

                currSecQstns.sort(function (a, b) {
                    return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
                });
                if (RepeatCount > 0){

                    cnt = 0;
                    for (var j = 1; j <= RepeatCount; j++) {

                        // this push is to print the section name like (section 2:1 , section 2:2)
                        this.RepeatSection.push({
                            RQstInd: cnt, SurveyID: this.SurveyID,
                            SectionID: SectionDetails[0].SectionID, SectionName: SectionDetails[0].SectionName, SectionOrder: currSecQstns[0].SectionOrder,
                            QuestionOrder: 1, QuestionTypeID: 0, QuestionID: 0, QuestionName: SectionDetails[0].SectionName + " : " + j, SectionLogic: [], QuestionType: "",
                            IsFormulaQuestion: 0, IsConditionQuestion: 0, QuestionCondition: null, QuestionOptions: null, QuestionHint: "", IsValidationQuestion: 0,
                            RegEx: "", Answer: null, FieldVar: "", IsUnique: 0, IsUpload: 0, ErrorText: "", DefaultValue: "", IsMandatory: 0, IsSearchable: 0, QuestionCategory: 1,
                            FormulaOperation: "", FormulaExpression: "", FormulaFieldVariables: null, CascadingQuestionOptions: null, UOM: "", IsValidationError : false,ValidationErrorTest:"", DateMenu : false,ParentQuestionID :HirachyParentQID,IndexPos :i+1,startCamera: false,camOn : false,img:'',uploadedImg :'',uploadedFile :'' ,benefIndex : benefIndex, BeneficiaryID : BeneficiaryID
                        });
                        cnt++;
                        for (var i = 0; i < currSecQstns.length; i++) {
                             if(currSecQstns[i].QuestionType == "Multi-Choice"){
                                var RepeatOptionNew =[];
                                for(var RCnt=0; RCnt<= currSecQstns[i].QuestionOptions.length-1; RCnt++)
                                {
                                    var ObjOptions = new Object();
                                    ObjOptions.OptionID = currSecQstns[i].QuestionOptions[RCnt].OptionID;
                                    ObjOptions.OptionValue = currSecQstns[i].QuestionOptions[RCnt].OptionValue;
                                    ObjOptions.QuestionID = currSecQstns[i].QuestionOptions[RCnt].QuestionID;
                                    ObjOptions.RQstInd = cnt;
                                    ObjOptions.IsOptSelected = false;
                                    RepeatOptionNew.push(ObjOptions);
                                }
                                currSecQstns[i].QuestionOptions = RepeatOptionNew;
                            }
							if(currSecQstns[i].QuestionType == "Cascading Dropdown")
                            {
                                var RepeatOptionCascading =[];
                                for(var RCnt=0; RCnt<= currSecQstns[i].CascadingQuestionOptions.length-1; RCnt++)
                                {

                                    RepeatOptionCascading = [];
                                    for(var CasCnt = 0 ; CasCnt<= currSecQstns[i].CascadingQuestionOptions[RCnt].CascadingList.length-1; CasCnt++ )
                                    {
                                        var CasCadLis = currSecQstns[i].CascadingQuestionOptions[RCnt].CascadingList[CasCnt]
                                        var ObjOptions = new Object();
                                        ObjOptions.ID = CasCadLis.ID;
                                        ObjOptions.Name = CasCadLis.Name;
                                        ObjOptions.ParentID = CasCadLis.ParentID;
                                        ObjOptions.RQstInd = cnt;
                                        ObjOptions.OptionID = 0;
                                        RepeatOptionCascading.push(ObjOptions);
                                    }
                                    currSecQstns[i].CascadingQuestionOptions[RCnt].CascadingList = RepeatOptionCascading;
                                }

                            }
                            // this push is to print the section questions
                            this.RepeatSection.push({
                                RQstInd: cnt, SurveyID: this.SurveyID,
                                SectionID: currSecQstns[i].SectionID, SectionName: currSecQstns[i].SectionName, SectionOrder: currSecQstns[i].SectionOrder,
                                QuestionOrder: currSecQstns[i].QuestionOrder, QuestionTypeID: currSecQstns[i].QuestionTypeID, QuestionID: currSecQstns[i].QuestionID,
                                QuestionName: currSecQstns[i].QuestionName, SectionLogic: currSecQstns[i].SectionLogic, QuestionType: currSecQstns[i].QuestionType,
                                IsFormulaQuestion: currSecQstns[i].IsFormulaQuestion, IsConditionQuestion: currSecQstns[i].IsConditionQuestion, QuestionCondition:currSecQstns[i].QuestionCondition,
                                QuestionOptions: currSecQstns[i].QuestionOptions, QuestionHint: currSecQstns[i].QuestionHint, IsValidationQuestion: currSecQstns[i].IsValidationQuestion,
                                RegEx: currSecQstns[i].RegEx, Answer: currSecQstns[i].Answer, FieldVar: currSecQstns[i].FieldVar, IsUnique: currSecQstns[i].IsUnique, IsUpload: currSecQstns[i].IsUpload,
                                ErrorText: currSecQstns[i].ErrorText, DefaultValue: currSecQstns[i].DefaultValue, IsMandatory: currSecQstns[i].IsMandatory, IsSearchable: currSecQstns[i].IsSearchable,
                                QuestionCategory: currSecQstns[i].QuestionCategory, FormulaOperation: currSecQstns[i].FormulaOperation, FormulaExpression: currSecQstns[i].FormulaExpression, UOM: currSecQstns[i].UOM,
                                FormulaFieldVariables: currSecQstns[i].FormulaFieldVariables, CascadingQuestionOptions: currSecQstns[i].CascadingQuestionOptions,IsValidationError : false,ValidationErrorTest :"", DateMenu : false,ParentQuestionID :HirachyParentQID,IndexPos :i+1,startCamera: false,camOn : false,img:'',uploadedImg :'',uploadedFile :'',benefIndex : benefIndex, BeneficiaryID : BeneficiaryID
                           });
                            cnt++;
                        }

                    }
                }
                //var SectionCurrentIndex = this.lstSections.findIndex(x => (x.SectionID == this.vmSection.SectionID));
                //this.BenefQstnStepper[benefIndx] = SectionCurrentIndex + 1;
            },
            async UpdateTask() {

                var lstTaskResponse = [];
                var task = this.lstTaskList.filter(x => (x.WFLevel == 0) && (x.Type == this.TaskType) && (x.TypeID == this.TaskTypeID) && (x.TaskDate == this.TaskDate))[0];
                var SurveyAns = "";

             if (this.TaskType == 'Survey' && (this.vmNonBenefSurveyChoice==2 ||this.vmBenefSurveyChoice==2)) {

                    const filteredArr = this.lstBenefSurveyQstnAnswers.reduce((acc, current) => {
                        const x = acc.find(item => (item.ResponseID === current.ResponseID) && (item.QuestionID === current.QuestionID)
                            && (item.Answer === current.Answer));

                        if (!x) {
                            return acc.concat([current]);
                        } else {
                            return acc;
                        }
                    }, []);
                   SurveyAns = JSON.stringify(filteredArr);
                }
                else if (this.TaskType == 'Survey' && (this.vmNonBenefSurveyChoice==1 ||this.vmBenefSurveyChoice==1))
                {
                 SurveyAns = JSON.stringify(this.lstBenefSurveyQstnAnswers);
                }
                    
                if (this.varIsSubmitted == 0) {

                    await dbImworks.SavedTaskDB.put({
                        SyncDBID: task.SyncDBID, Type: this.TaskType, Command: 'TaskResponse', TypeID: this.TaskTypeID, TypeIDColumn: task.TypeIDColumn, WorkflowID: task.WorkflowID,
                        TotalWFLevel: task.TotalWFLevel, WFLevel: 0, Status: 'Partially DataCollected', SurveyQstns: JSON.stringify(this.lstSelectedTask.SurveyQstnList),
                        SurveyAnswers: SurveyAns, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy, NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                        SelectedBeneficiaries: JSON.stringify(this.vmSelectedBeneficiaries), TaskDate: this.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID
                    });
                    this.Msg = "Collected data saved...";
                    this.GetMyList(MyTask, this.searchcall);
                }
                if (this.varIsSubmitted == 1) {

                    var today = new Date().toLocaleString("en-US");

                    lstTaskResponse.push({
                        SyncDBID: task.SyncDBID, Type: this.TaskType, TypeID: this.TaskTypeID, TypeIDColumn: task.TypeIDColumn, Status: 'DataCollected',
                        WorkflowID: task.WorkflowID, TotalWFLevel: task.TotalWFLevel, WFLevel: 0, WFLevelStatus: "Completed", TaskUpdateComment: this.vmUpdateComment,
                        AttachedDocumentName: this.fileInput != null ? this.fileInput.name : "", SurveyAnswers: SurveyAns, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy,
                        NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                        TaskDate: this.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID
                    });
                    await dbImworks.TaskSyncDB.put({
                        Type: this.TaskType, Command: 'TaskResponse', TypeID: this.TaskTypeID, TypeIDColumn: task.TypeIDColumn,
                        Status: 'DataCollected', WorkflowID: task.WorkflowID, TotalWFLevel: task.WorkflowID, WFLevel: 0, ClientData: JSON.stringify(lstTaskResponse),
                        lastSyncDBID: task.SyncDBID, IsBeneficiaryLinked: this.varIsBeneficiaryLinked, TaskDate: this.TaskDate, Periodicity: this.Periodicity, WorkspaceID: task.WorkspaceID

                    });

                    var lastClientSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();

                    var TaskDetails = this.lstMyTask.filter(x => (x.TaskType == this.TaskType) && (x.TaskTypeID == this.TaskTypeID))[0];

                    await dbImworks.TbMyTask.put({
                        ClientSyncTaskID: lastClientSyncID, Type: this.TaskType, TypeID: this.TaskTypeID, WFLevel: 0, WFLevelStatus: "Completed",
                        TaskUpdateComment: this.vmUpdateComment, AttachedDocumentName: this.fileInput != null ? this.fileInput.name : "", SyncDBID: task.SyncDBID,
                        ProjectName: TaskDetails.ProjectName, TaskName: TaskDetails.TaskName, Description: TaskDetails.TaskDescription,
                        CreatedDate: today, TaskDate: this.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID, Periodicity: this.Periodicity
                    });
                    
                    if (this.fileInput != null) {

                        await dbImworks.TbTaskDocument.put({

                            SyncTaskID: task.SyncDBID, Type: this.TaskType, TypeID: this.TaskTypeID, WFLevel: 0, AttachedDocument: this.fileInput,
                            AttachedDocumentName: this.fileInput.name, DocumentOriginalName: this.fileInput.name.split('.').slice(0, -1).join('.'),
                            DocumentSize: this.formatFileSize(this.fileInput.size, 0), DocumentType: this.fileInput.type, DocumentTypeIcon: this.fileInput.name.split('.').pop(),
                            DocumentLastModifiedDate: this.fileInput.lastModifiedDate != null ? new Date(this.fileInput.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.fileInput.lastModifiedDate).toISOString().substr(11, 5) : null,
                            ProjectID: TaskDetails.ProjectId, ProjectName: TaskDetails.ProjectName, DocLinkedTo: TaskDetails.TaskName, TaskDate: this.TaskDate
                        });
                    }
                    
                    var MyTask = this.lstTaskList.filter(x => (x.WFLevel == 0) && (x.WFLevelStatus == "Pending"));
                    for (var i = 0; i < MyTask.length; i++) {
                        if (this.Periodicity != 'Anytime') {
                        if (MyTask[i].WFLevel == 0 && MyTask[i].Type == this.TaskType && MyTask[i].TypeID == this.TaskTypeID
                            && MyTask[i].TaskDate == this.TaskDate && MyTask[i].Periodicity != 'Anytime') {

                            MyTask.splice(i, 1);
                            MyTask = [...MyTask];

                        }
						}
                    }

                    //var a = await dbImworks.SavedTaskDB.where("SyncDBID").equals(parseInt(task.SyncDBID)).toArray();
                    var a = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(task.SyncDBID), TaskDate: TaskDetails.TaskDate })
                    if (a.length > 0)
                        await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(a[0].pkId)).delete();


                    this.GetMyList(MyTask, this.searchcall);
                    this.dlgActvityType = false;
                    this.Msg = "Data Collection Completed ...";
                }
                this.vmUpdateComment = "";
                this.fileInput = null;
                this.TaskType = "";
                this.TaskTypeID = "";
                this.successSnackbarMsg = true;
                this.varIsSubmitted = 1;

            },
            ClearBenefSurveyUpdate() {
                this.vmNonBenefSurveyChoice = 0;
                this.NonBenefLinkedSurveySteps =1;
                this.dlgSurveyType = false;
                this.BenefQstnStepper = [];
                this.vmBenefSurveyChoice = 0;
                this.vmAddBeneficiaryChoice = 0;
                this.vmBenefSurveySubmitOption = 0;
                this.BenefStepper = 1;
                this.IsBenfInputFinished = [];
                this.vmSearchBeneficiary = "";
                this.BenefLinkedSurveySteps = 1;
                this.surveyfileInput=null;
                this.lstImportSurveyQstnHearder=[];
                this.lstImportSurvey=[];
                this.lstBenefSurveyQstnAnswers = [];
                this.vmSelectedBeneficiaries = [];
                this.lstNewBeneficiary = [];
                this.lstBeneficiary = [];
                this.img = null;
                this.camera = null;
                this.deviceId = null;
                this.devices = [];
                this.IsUpdate_BenSurvey = false;
                this.IsInputFinished = false;
                this.ActivityTabs = 'tab-1';
                this.cascadingAnsArray =[];
                //-- Non benficiary Linked
                this.dlgNonBeneficarySurveyType = false;
                this.RepeatSection =[];
                this.SectionDetails =[];
                this.lstSectionQstns = [];
                this.csvSurveyFileName = "";
                //this.lstImportSurveyQstnHearderN =[];
            },
            formatFileSize(bytes, decimalPoint) {

                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            //===========ReportIssue=====================================//
            async ReportIssue() {

                var lstTaskResponse = [];
                if (this.IssueReportTaskDetails != null) {

                    var SurveyAns = "";
                    var today = new Date().toLocaleString();

                    // Pushing to Task Response
                    lstTaskResponse.push({
                        SyncDBID: 0,
                        Type: "Issue",
                        Command: "ADD",
                        TypeID: this.IssueReportTaskDetails.TaskTypeID,
                        TypeIDColumn: this.IssueReportTaskDetails.TypeIDColumn,
                        Status: 'Pending',
                        WorkflowID: 0,
                        TotalWFLevel: 0,
                        WFLevel: 0,
                        WFLevelStatus: "Completed",
                        TaskUpdateComment: this.vmIssueComment,
                        AttachedDocumentName: this.IssuefileInput.name,
                        SurveyAnswers: SurveyAns,
                        ResponseSyncID: 0,
                        DataCollectedBy: this.IssueReportTaskDetails.DataCollectedBy,
                        NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                        TaskDate: today,
                        TaskSubmittedDate: today,
                        ReportingValue: 0,
                        WorkspaceID: this.IssueReportTaskDetails.WorkspaceID

                    });

                    // Inserting into dexie table
                    await dbImworks.TaskSyncDB.put({
                        Type: "Issue",
                        Command: 'ADD',
                        TypeID: this.IssueReportTaskDetails.TaskTypeID,
                        TypeIDColumn: this.IssueReportTaskDetails.TypeIDColumn,
                        Status: 'NewIssue',
                        WorkflowID: 0,
                        TotalWFLevel: 0,
                        WFLevel: 0,
                        ClientData: JSON.stringify(lstTaskResponse),
                        lastSyncDBID: 0,
                        IsBeneficiaryLinked: [],
                        TaskDate: today,
                        Periodicity: 'none',
                        ReportingValue: 0,
                        WorkspaceID: this.IssueReportTaskDetails.WorkspaceID

                    });

                    if (this.IssuefileInput != null) {
                        await dbImworks.TbTaskDocument.put({

                            SyncTaskID: 0, Type: "Issue", TypeID: this.IssueReportTaskDetails.TaskTypeID, WFLevel: 0, AttachedDocument: this.IssuefileInput,
                            AttachedDocumentName: this.IssuefileInput.name, DocumentOriginalName: this.IssuefileInput.name.split('.').slice(0, -1).join('.'),
                            DocumentSize: this.formatFileSize(this.IssuefileInput.size, 0), DocumentType: this.IssuefileInput.type, DocumentTypeIcon: this.IssuefileInput.name.split('.').pop(),
                            DocumentLastModifiedDate: this.IssuefileInput.lastModifiedDate != null ? new Date(this.IssuefileInput.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.IssuefileInput.lastModifiedDate).toISOString().substr(11, 5) : null,
                            ProjectID: this.IssueReportTaskDetails.ProjectId, ProjectName: this.IssueReportTaskDetails.ProjectName,
                            DocLinkedTo: this.IssueReportTaskDetails.TaskName + "-Issue", TaskDate: this.IssueReportTaskDetails.TaskDate
                        });
                    }
                    this.vmIssueComment = "";
                    this.IssuefileInput = null;
                    this.ReportIssuedlg = false;
                    this.IssueReportTaskDetails = [];
                    this.IssueReportSuccessfully = true;
                }
            },
            //================================================//

            // -- Added New code on 04-02-2021
            // -- To check Question Validation and Condition under a section before clicking Next Button
            fnCheckQuestionValidationAndCondition(SectionQuestions,benefID){
                // Checking for mandatory questions
                var lstMandatoryQuestion = SectionQuestions.filter(x=> x.IsMandatory == 1);
                //  Checking for Validation Question Min and Max
                var lstValidationQuestion = SectionQuestions.filter(x=> x.IsValidationQuestion == 1);
                var IsValid = false;

                if(lstMandatoryQuestion.length > 0 ){

                    // Selecting from Mandatory Questions without answer
                    var IsQAnswers = lstMandatoryQuestion.filter(x=> x.Answer == "");
                    var QstnWithAnswer =lstMandatoryQuestion.filter(x=> x.Answer != "");

                        QstnWithAnswer.forEach(function(part, index) {
                          QstnWithAnswer[index].ValidationErrorTest = "";
                          QstnWithAnswer[index].IsValidationError = false;
                        });
                    if(IsQAnswers.length > 0){
                        for( var i = 0; i <= IsQAnswers.length -1 ; i++){
                                if(IsQAnswers[i].QuestionType != "Cascading Dropdown" && IsQAnswers[i].QuestionType !="Photo Capture" && IsQAnswers[i].QuestionType != "Multi-Choice"){
                                     this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                     this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                     IsValid = true;
                                }
                                else if(IsQAnswers[i].QuestionType == "Cascading Dropdown")
                                {
                                    var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));
                                        if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)){
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                        }
                                }
                                else if(IsQAnswers[i].QuestionType == "Multi-Choice")
                                {

                                    var SelectedmultiChoiceAns = this.multiChoiceAns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID  && x.benefIndex == this.BenefStepper)
                                    if(typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0)
                                    {
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                    else
                                    {
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                    }
                                }
                                else if(IsQAnswers[i].QuestionType == "Photo Capture")
                                {

                                    if(typeof IsQAnswers[i].AnsBlob != "undefined")
                                    {
                                        if(IsQAnswers[i].AnsBlob.name == "")
                                        {
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else
                                        {
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                        }
                                    }
                                    else
                                    {
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                        this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                }
                                else
                                {
                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                       // return IsValid;
                    }
                }
                if(lstValidationQuestion.length > 0){

                       var IsQAnswers = lstValidationQuestion.filter(x=> x.Answer != "");
                       var valConditions
                       var NewErrorMsg ="";
                       var OldErrorMsg ="";
                        var today = new Date().toLocaleDateString('en-GB');
                           for(var intValQstCnt = 0 ; intValQstCnt<=IsQAnswers.length-1 ; intValQstCnt++)
                            {
                                    valConditions = IsQAnswers[intValQstCnt].QuestionCondition;
                                   for (var i = 0; i < valConditions.length; i++) {
                                     if (valConditions[i].ValidationCondition == 'Min & Max Range') {

                                      if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue))) {

                                                NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                                OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                if(OldErrorMsg == ""){
                                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;

                                                   this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;

                                            }else{this.chkQstnValid = false;
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false}
                                    }
                                    if (valConditions[i].ValidationCondition == "Should be less than Current date")
                                    {

                                        if (IsQAnswers[intValQstCnt].Answer > this.formatDateTodayDate(today))
                                        {
                                                NewErrorMsg = "Should be less than Current date";
                                                OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                if(OldErrorMsg == ""){
                                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            this.chkQstnValid = false;
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                    }
                                    if (valConditions[i].ValidationCondition == "Should be greater than Current date")
                                    {

                                        if (IsQAnswers[intValQstCnt].Answer < this.formatDateTodayDate(today))
                                        {
                                                NewErrorMsg = "Should be greater than Current date";
                                                OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                if(OldErrorMsg == ""){
                                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            this.chkQstnValid = false;
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                    }
					             // Checking Phone number validation
                                    if (valConditions[i].ValidationCondition == "Phone Number")
                                    {

                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                        if(!IsQAnswers[intValQstCnt].Answer.match(phoneNum))
                                        {

                                                NewErrorMsg = "Please enter a valid Phone Number";
                                                OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                                                if(OldErrorMsg == ""){
                                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            this.chkQstnValid = false;
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                    }
                                    // Checking Phone number validation
                                    if (valConditions[i].ValidationCondition == "Email")
                                    {

                                        var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                        if(!IsQAnswers[intValQstCnt].Answer.match(email))
                                        {
                                                NewErrorMsg = "Please enter a valid Email";
                                                OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                                                if(OldErrorMsg == ""){
                                                    this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            this.chkQstnValid = false;
                                            this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                    }


                            }

                        }
                    }
                    // checking regex expressions
                    var lstRegexCondtionQ =  SectionQuestions.filter(x=> x.RegEx !="" && x.Answer !="");
                    if(lstRegexCondtionQ.length >0)
                    {
                        for(var RegCnt = 0; RegCnt<=lstRegexCondtionQ.length-1;RegCnt++)
                        {
                            var QItem = lstRegexCondtionQ[RegCnt];
                            // checking for regex expression
                            if(QItem.RegEx !="" && QItem.Answer !="")
                            {
                                const regExpStr = QItem.RegEx;//"^([a-z0-9]{5,})$";//
                                const result = new RegExp(regExpStr, 'g').test(QItem.Answer) // here I have used 'g' which means global search
                                if (!result) {
                                    this.lstSectionQstns.filter(x=> x.QuestionID == QItem.QuestionID)[0].IsValidationError = true;
                                    this.lstSectionQstns.filter(x=> x.QuestionID == QItem.QuestionID)[0].ValidationErrorTest="Please enter a valid string " + QItem.RegEx;
                                    IsValid = true;
                                }
                                else
                                {
                                    this.lstSectionQstns.filter(x=> x.QuestionID == QItem.QuestionID)[0].IsValidationError = false;
                                    this.lstSectionQstns.filter(x=> x.QuestionID == QItem.QuestionID)[0].ValidationErrorTest ="";
                                        IsValid = false;
                                }
                            }
                        }
                    }
                 return IsValid;
            },
            // checking validations for repeat section
            fnCheckRepeatSectionQuestionValidationAndCondition(benefID){
                // Checking for mandatory questions
                var lstMandatoryQuestion = this.RepeatSection.filter(x=> x.IsMandatory == 1);
                //  Checking for Validation Question Min and Max
                var lstValidationQuestion = this.RepeatSection.filter(x=> x.IsValidationQuestion == 1);
                var IsValid = false;

                if(lstMandatoryQuestion.length > 0 ){

                    // Selecting from Mandatory Questions without answer
                    var IsQAnswers = lstMandatoryQuestion.filter(x=> x.Answer == "");
                    var QstnWithAnswer =lstMandatoryQuestion.filter(x=> x.Answer != "");

                        QstnWithAnswer.forEach(function(part, index) {
                          QstnWithAnswer[index].ValidationErrorTest = "";
                          QstnWithAnswer[index].IsValidationError = false;
                        });
                    if(IsQAnswers.length > 0){
                        for( var i = 0; i <= IsQAnswers.length -1 ; i++){
                                if(IsQAnswers[i].QuestionType != "Cascading Dropdown" && IsQAnswers[i].QuestionType !="Photo Capture" && IsQAnswers[i].QuestionType != "Multi-Choice"){
                                     this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "Required Field";
                                     this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = true;
                                     IsValid = true;
                                }
                                else if(IsQAnswers[i].QuestionType == "Cascading Dropdown")
                                {
                                    var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));
                                        if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)){
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else{this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                        }
                                }
                                else if(IsQAnswers[i].QuestionType == "Multi-Choice")
                                {

                                    var SelectedmultiChoiceAns = this.multiChoiceAns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID  && x.benefIndex == this.BenefStepper)
                                    if(typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0)
                                    {
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "Required Field";
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                    else
                                    {
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "";
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = false;
                                    }
                                }
                                else if(IsQAnswers[i].QuestionType == "Photo Capture")
                                {

                                    if(typeof IsQAnswers[i].AnsBlob != "undefined")
                                    {
                                        if(IsQAnswers[i].AnsBlob.name == "")
                                        {
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "Required Field";
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else
                                        {
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "";
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = false;
                                        }
                                    }
                                    else
                                    {
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "Required Field";
                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                }
                                else
                                {
                                    this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].ValidationErrorTest = "";
                                    this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID && x.RQstInd == IsQAnswers[i].RQstInd)[0].IsValidationError = false;
                                }
                            }
                       // return IsValid;
                    }
                }
                if(lstValidationQuestion.length > 0){

                       var IsQAnswers = lstValidationQuestion.filter(x=> x.Answer != "");
                       var valConditions
                       var NewErrorMsg ="";
                       var OldErrorMsg ="";
                        var today = new Date().toLocaleDateString('en-GB');
                           for(var intValQstCnt = 0 ; intValQstCnt<=IsQAnswers.length-1 ; intValQstCnt++)
                            {
                                    valConditions = IsQAnswers[intValQstCnt].QuestionCondition;
                                   for (var i = 0; i < valConditions.length; i++) {
                                     if (valConditions[i].ValidationCondition == 'Min & Max Range') {

                                      if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue))) {

                                                NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                                OldErrorMsg = this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                if(OldErrorMsg == ""){
                                                    this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;

                                                   this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = true;
                                                IsValid = true;

                                            }else{this.chkQstnValid = false;
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = false}
                                    }
                                    if (valConditions[i].ValidationCondition == "Should be less than Current date")
                                    {

                                        if (IsQAnswers[intValQstCnt].Answer > this.formatDateTodayDate(today))
                                        {
                                                NewErrorMsg = "Should be less than Current date";
                                                OldErrorMsg = this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                if(OldErrorMsg == ""){
                                                    this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = OldErrorMsg}
                                                this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            this.chkQstnValid = false;
                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = false
                                        }
                                    }
                                    if (valConditions[i].ValidationCondition == "Should be greater than Current date")
                                        {

                                            if (IsQAnswers[intValQstCnt].Answer < this.formatDateTodayDate(today))
                                                {
                                                        NewErrorMsg = "Should be greater than Current date";
                                                        OldErrorMsg = this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                                        if(OldErrorMsg == ""){
                                                            this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = NewErrorMsg;
                                                            }else{this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].ValidationErrorTest = OldErrorMsg}
                                                        this.qRules = "";
                                                        //this.chkQstnValid = true;
                                                        this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = true;
                                                        IsValid = true;
                                                }
                                            else
                                            {
                                                this.chkQstnValid = false;
                                                this.RepeatSection.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID && x.RQstInd == IsQAnswers[intValQstCnt].RQstInd)[0].IsValidationError = false
                                            }
                                     }
                            }

                        }
                    }
                    // checking regex expressions
                    var lstRegexCondtionQ =  this.RepeatSection.filter(x=> x.RegEx !="" && x.Answer !="");
                    if(lstRegexCondtionQ.length >0)
                    {
                        for(var RegCnt = 0; RegCnt<=lstRegexCondtionQ.length-1;RegCnt++)
                        {
                            var QItem = lstRegexCondtionQ[RegCnt];
                            // checking for regex expression
                            if(QItem.RegEx !="" && QItem.Answer !="")
                            {
                                const regExpStr = QItem.RegEx;//"^([a-z0-9]{5,})$";//
                                const result = new RegExp(regExpStr, 'g').test(QItem.Answer) // here I have used 'g' which means global search
                                if (!result) {
                                    this.RepeatSection.filter(x=> x.QuestionID == QItem.QuestionID)[0].IsValidationError = true;
                                    this.RepeatSection.filter(x=> x.QuestionID == QItem.QuestionID)[0].ValidationErrorTest="Please enter a valid string " + QItem.RegEx;
                                    IsValid = true;
                                }
                                else
                                {
                                    this.RepeatSection.filter(x=> x.QuestionID == QItem.QuestionID)[0].IsValidationError = false;
                                    this.RepeatSection.filter(x=> x.QuestionID == QItem.QuestionID)[0].ValidationErrorTest ="";
                                        IsValid = false;
                                }
                            }
                        }
                    }
                 return IsValid;
            },
            formatDateTodayDate(date) {
                if (!date) return null
                const [day, month, year] = date.split('/')
                return `${year}-${month}-${day}`
            },
            // to disable the item in selection dropdown
            onSelect(e) {

                if (e.length == 0) {
                  this.lstSections.forEach((item) => item.disabled = false)
                    } else {
                        let chosen = this.lstSections.filter((item) => item.id==e[0])
                        this.lstSections.forEach((item) => {
                            item.disabled = true
                        })
                  }
                  },
            // for validating Beneficary importes excel data
        onBeforeMandatoryChange (...args) {
                this.IsvalidexcelData=true;
                var today = new Date().toLocaleString();
                var list= this.lstImportBeneficiary;
                var header=this.lstImportBeneficiaryHearder;
                var IsMadatoryHearder=header.filter(x=>x.mandatory=='Yes')

                for(var i=0;i<IsMadatoryHearder.length-1;i++)
                {
                    for(var j=0 ;j<= list.length-1 ; j++)
                    {
                        var Ans =   list[j][IsMadatoryHearder[i].value];
                        if(Ans == "")
                        {
                          this.IsvalidexcelData=false;
                        }
                    }
                }
                var argument=args;
                if(argument[3].mandatory=='Yes')
                {
                    if(argument[0]=="")
                    {
                     this.IsvalidexcelData=false;
                     return 'Required field'

                    }
                }
                // for checking Question validations
            var tempData=this.lstImportBeneficiaryHearder.filter(x=>x.value==argument[3].name)
            if(tempData.length>0)
            {
                if(tempData[0].QuestionCondition != null)
                {
                    var phoneNoValidation=tempData[0].QuestionCondition;
                    for (var j = 0; j < phoneNoValidation.length; j++)
                    {
                        if (phoneNoValidation[j].ValidationCondition == "Phone Number" && argument[0]!="")
                        {
                            var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                            if(!argument[0].match(phoneNum))
                            {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Phone Number'
                            }
                        }
                       if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0]!="")
                       {
                            var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                            if(!argument[0].match(email))
                            {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Email'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && argument[0]!="")
                        {
                              if (this.formatExcelDate(argument[0]) < this.formatExcelDate(today))
                              {
                                    this.IsvalidexcelData=false;
                                    return 'Should be greater than Current date'
                              }
                       }
                       if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && argument[0]!="")
                       {
                              if (this.formatExcelDate(argument[0]) > this.formatExcelDate(today))
                              {
                                    this.IsvalidexcelData=false;
                                    return 'Should be less than Current date'
                              }
                       }
                       if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && argument[0]!="")
                       {
                              if(!(parseInt(argument[0]) > parseInt(phoneNoValidation[j].MinValue) && parseInt(argument[0]) < parseInt(phoneNoValidation[j].MaxValue)))
                              {
                                     this.IsvalidexcelData=false;
                                    return "Value should be between " + phoneNoValidation[j].MinValue + " and " + phoneNoValidation[j].MaxValue;

                              }
                       }
                   }
                }
            }
          },
            // for validating Survey imported excel data
              onBeforeNameChange (...args) {
                var today = new Date().toLocaleString();
                this.IsvalidexcelData=true;
               var list= this.lstImportSurvey;
                var header=this.lstImportSurveyQstnHearderN;
                var IsMadatoryHearder=header.filter(x=>x.mandatory=='Yes')

                for(var i=0;i<=IsMadatoryHearder.length-1;i++)
                {
                    for(var j=0 ;j<= list.length-1 ; j++)
                    {
                        var Ans =   list[j][IsMadatoryHearder[i].value];
                        if(Ans == "")
                        {
                          this.IsvalidexcelData=false;
                        }
                    }
                }
                var argument=args;
                if(argument[3].mandatory=='Yes')
                {
                    if(argument[0]=="")
                    {
                    // this.IsvalidexcelData=false;
                argument[0]="Invalid Input";
                     return 'Required field'

                    }
                }
            var tempData=this.lstImportSurveyQstnHearderN.filter(x=>x.value==argument[3].name)
            if(tempData.length>0)
            {
            if(tempData[0].QuestionCondition != null)
            {
              var phoneNoValidation=tempData[0].QuestionCondition;
              for (var j = 0; j < phoneNoValidation.length; j++)
                {
                    if (phoneNoValidation[j].ValidationCondition == "Phone Number" && argument[0]!="")
                        {
                          var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                           if(!argument[0].match(phoneNum))
                            {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Phone Number'
                            }
                        }
                       if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0]!="")
                        {
                         var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                           if(!argument[0].match(email))
                            {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Email'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && argument[0]!="")
                        {
                              if (this.formatExcelDate(argument[0]) < this.formatExcelDate(today))
                              {
                                    this.IsvalidexcelData=false;
                                    return 'Should be greater than Current date'
                              }
                       }
                       if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && argument[0]!="")
                       {
                              if (this.formatExcelDate(argument[0]) > this.formatExcelDate(today))
                              {
                                    this.IsvalidexcelData=false;
                                    return 'Should be less than Current date'
                              }
                       }
                       if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && argument[0]!="")
                       {
                              if(!(parseInt(argument[0]) > parseInt(phoneNoValidation[j].MinValue) && parseInt(argument[0]) < parseInt(phoneNoValidation[j].MaxValue)))
                              {
                                     this.IsvalidexcelData=false;
                                    return "Value should be between " + phoneNoValidation[j].MinValue + " and " + phoneNoValidation[j].MaxValue;

                              }
                       }
                   }
                }

            }
                return ''
            },

            formatExcelDate(date) {
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day;

                return [year, month, day].join('-');
            },
           updateCoordinates(evnt , Qitem) {

            this.Coordinates = {
                lat: evnt.lat(),
                lng: evnt.lng(),
            };
               Qitem.Answer = this.Coordinates.lat +","+this.Coordinates.lng;
        },

            CheckPreviousInImport(BenefStepper)
         {
         if(this.vmNonBenefSurveyChoice==1)
           {
           this.NonBenefLinkedSurveySteps = 2;

            }
            else
            {
            this.UpdateBenefQstnInput(BenefStepper);
            }
        },

            fnCheckBeneficiaryMandatoryAndValidation()
            {


                var IsValid = false;
                if(this.lstBenefRegQstns.length > 0)
                {
                    // Checking for mandatory questions
                    var lstMandatoryQuestion = this.lstBenefRegQstns.filter(x=> x.IsMandatoryResponse == 1);
                    //  Checking for Validation Question Min and Max
                    var lstValidationQuestion = this.lstBenefRegQstns.filter(x=> x.IsValidationQuestion == 1 && (x.BTQstnValidationList != null));


                    if(lstMandatoryQuestion.length > 0 )
                    {
                        // Selecting from Mandatory Questions without answer
                        var IsQAnswers = lstMandatoryQuestion.filter(x=> x.Answer == "");
                        var QstnWithAnswer =lstMandatoryQuestion.filter(x=> x.Answer != "");

                        QstnWithAnswer.forEach(function(part, index) {
                            QstnWithAnswer[index].ValidationErrorTest = "";
                            QstnWithAnswer[index].IsValidationError = false;
                        });

                        for( var i = 0; i <= IsQAnswers.length -1 ; i++){
                                if(IsQAnswers[i].QuestionType != "Cascading Dropdown" && IsQAnswers[i].QuestionType !="Photo Capture" && IsQAnswers[i].QuestionType != "Multi-Choice"){
                                     this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                     this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                     IsValid = true;
                                }
                                else if(IsQAnswers[i].QuestionType == "Cascading Dropdown")
                                {
                                    var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));
                                        if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)){
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                        }
                                }
                                else if(IsQAnswers[i].QuestionType == "Multi-Choice")
                                {

                                    var SelectedmultiChoiceAns = this.multiChoiceAnsBT.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)
                                    if(typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0)
                                    {
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                    else
                                    {
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                    }
                                }
                                else if(IsQAnswers[i].QuestionType == "Photo Capture")
                                {

                                    if(typeof IsQAnswers[i].AnsBlob != "undefined")
                                    {
                                        if(IsQAnswers[i].AnsBlob.name == "")
                                        {
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                        }
                                        else
                                        {
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                        }
                                    }
                                    else
                                    {
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                }
                                else
                                {
                                    this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                    }
                    // Validations Checking
                    if(lstValidationQuestion.length > 0)
                    {
                        var IsQAnswers = lstValidationQuestion.filter(x=> x.Answer != "" && x.Answer != null);
                        var valConditions
                        var NewErrorMsg ="";
                        var OldErrorMsg ="";
                        var today = new Date().toLocaleDateString('en-GB');
                        for(var intValQstCnt = 0 ; intValQstCnt<=IsQAnswers.length-1 ; intValQstCnt++)
                        {
                             valConditions = IsQAnswers[intValQstCnt].BTQstnValidationList;
                             for (var i = 0; i < valConditions.length; i++)
                             {
                                if (valConditions[i].ValidationCondition == 'Min & Max Range') // if Starts here
                                {
                                    if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue)))
                                    {
                                            NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                            //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                            if(OldErrorMsg == ""){
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                }else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}


                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;

                                     }
                                     else
                                     {
                                        this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                     }
                                }// if ends here
                                if (valConditions[i].ValidationCondition == "Should be less than Current date")
                                {
                                     if (IsQAnswers[intValQstCnt].Answer > this.formatDateTodayDate(today))
                                     {
                                            NewErrorMsg = "Should be less than Current date";
                                            //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                            if(OldErrorMsg == ""){
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                }else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                            this.qRules = "";
                                            //this.chkQstnValid = true;
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                            IsValid = true;
                                       }
                                       else
                                       {
                                            //this.chkQstnValid = false;
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                 }// if ends here
                                 if (valConditions[i].ValidationCondition == "Should be greater than Current date")
                                 {
                                        if (IsQAnswers[intValQstCnt].Answer < this.formatDateTodayDate(today))
                                        {
                                                NewErrorMsg = "Should be greater than Current date";
                                                //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                                 if(OldErrorMsg == ""){
                                                    this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                               // this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            //this.chkQstnValid = false;
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                  } // if ends here
                                  // Checking Phone number validation
                                  if (valConditions[i].ValidationCondition == "Phone Number")
                                  {
                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                        if(!IsQAnswers[intValQstCnt].Answer.match(phoneNum))
                                        {

                                                NewErrorMsg = "Please enter a valid Phone Number";
                                                //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                                                if(OldErrorMsg == ""){
                                                    this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                                // this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            //this.chkQstnValid = false;
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                   }
                                   // Checking email  validation
                                   if (valConditions[i].ValidationCondition == "Email")
                                   {

                                        var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                        if(!IsQAnswers[intValQstCnt].Answer.match(email))
                                        {
                                                NewErrorMsg = "Please enter a valid Email";
                                                //OldErrorMsg = this.lstBTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                                                if(OldErrorMsg == ""){
                                                    this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                                    }else{this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg}
                                               // this.qRules = "";
                                                //this.chkQstnValid = true;
                                                this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                                IsValid = true;
                                        }
                                        else
                                        {
                                            //this.chkQstnValid = false;
                                            this.lstBenefRegQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                        }
                                   }
                             }
                        }
                    }

                }

                return IsValid;
            },
            // function used to enable or disble beneficiary rows (if Beneficiaries are selected, then selected beneficiary will be disabled)
            IsSelected(item)
            {
                if(typeof item !="undefined")
                {
                    if(this.vmSelectedBeneficiaries.length > 0)
                    {
                        var temp = this.vmSelectedBeneficiaries.filter(x=> x.BeneficiaryID == item.BeneficiaryID)
                        if(temp.length > 0)
                        {
                            return true;
                        }

                    }
                }
                return false;
            },
            async fnTaskUpdate()
            {
                  await this.UpdateTask();
                  await this.ClearBenefSurveyUpdate()
            },
            async fnBindValue(e,index){
                this.lstBenefRegQstns[index].Answer =e.target.value;
            },
            async fnBindSurveyAnswer(e,index){

                if(this.lstSectionQstns[index].QuestionType == 'Dropdown')
                {
                    this.lstSectionQstns[index].Answer = e;
                }
                else
                {
                    this.lstSectionQstns[index].Answer = e.target.value;
                }

            },
            async fnBindSurveyAnswerLogic(e, index)
            {
                if(this.RepeatSection[index].QuestionType == 'Dropdown')
                {
                    this.RepeatSection[index].Answer = e;
                }
                else
                {
                    this.RepeatSection[index].Answer = e.target.value;
                }
            },
            async fnBindDropdownValue(Option)
            {

                this.lstSectionQstns[index].Answer = Option.value;
            },
            onFocusBeneficiaryQstnValidation(QItem)
            {

                var valConditions = QItem.BTQstnValidationList;
                var NewErrorMsg ="";
                var OldErrorMsg ="";
                var errMsg = "";
				//this.qType = "";
                //this.QstType = "";
               // this.txtLength = 0;
                //this.minVal = 0;
                //this.maxVal = 9999999999;
               // this.qRules = [];
                var today = new Date().toLocaleDateString('en-GB');
                if (valConditions != null) {
                    this.IsQstnValidation = true;
                    // ValidationCondition
                    for (var i = 0; i < valConditions.length; i++) {
                        if (valConditions[i].ValidationCondition == 'Phone Number') {
                            QItem.qType = "number";
                            QItem.QstType = "PhoneNumber";
                            QItem.txtLength = 10;
                            QItem.minVal = 0;
                            QItem.maxVal = 9999999999;
                            QItem.qRules = this.phoneNoRules;
                           // QItem.ErrorText ="";
                            QItem.IsValidationError = false;
                        }
                        else if (valConditions[i].ValidationCondition == 'Email') {
							QItem.textLength = 1000;
                            QItem.minVal = 0;
                            QItem.qType = "email";
                            QItem.qRules = this.emailRules;
                           // QItem.ErrorText ="";
                            QItem.IsValidationError = false;

                        }
                        else if (valConditions[i].ValidationCondition == 'Min & Max Range') {

                            QItem.textLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = valConditions[i].MinValue;
                            QItem.maxVal = valConditions[i].MaxValue;
                            QItem.qType = "number";
                            //this.qRules = [
                            //    v => !v || /\[0-9]{txtLength}$/.test(v) || 'Please enter valid number'
                            //];//this.numberRules;
                            if(QItem.Answer !="")
                            {

                                if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {
                                    if(valConditions[i].LogicOperator == 1 && QItem.ValidationErrorTest == "") //-- 1--> Or Condition
                                    {

                                    }
                                    NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    //OldErrorMsg = QItem.ErrorText;
                                     if(OldErrorMsg == "")
                                     {
                                          QItem.ValidationErrorTest = NewErrorMsg;
                                     }
                                     else
                                     {
                                          QItem.ValidationErrorTest= OldErrorMsg
                                     }
                                     QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest = "";
                                }
                            }


                        }
                        else if (valConditions[i].ValidationCondition == 'Should be less than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            //this.minVal = new Date().toISOString().substr(0, 10);
                            QItem.maxVal = new Date().toISOString().substr(0, 10);
                           // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer > this.formatDateTodayDate(today))
                                {
                                        NewErrorMsg = "Should be less than Current date";
                                        //OldErrorMsg = QItem.ErrorText;

                                        if(OldErrorMsg == "")
                                        {
                                            QItem.ValidationErrorTest = NewErrorMsg;
                                        }
                                        else
                                        {
                                            QItem.ValidationErrorTest = OldErrorMsg
                                        }
                                        QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest="";
                                }
                        }
                        else if (valConditions[i].ValidationCondition == 'Should be greater than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = new Date().toISOString().substr(0, 10);
                            //this.maxVal = valConditions[i].MaxValue;
                           // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer < this.formatDateTodayDate(today))
                                {
                                        NewErrorMsg = "Should be greater than Current date";
                                        //OldErrorMsg = QItem.ErrorText;

                                        if(OldErrorMsg == "")
                                        {
                                            QItem.ValidationErrorTest = NewErrorMsg;
                                        }
                                        else
                                        {
                                            QItem.ValidationErrorTest = OldErrorMsg
                                        }
                                        QItem.IsValidationError = true;
                                }
                                else
                                {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest="";
                                }
                        }
                    }

                }
            },

        },
        components: {

            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'vue-web-cam': WebCam,
        },

    }
</script>

<style scoped>
    .downloadButton:before {
        opacity: 0 !important;
    }

    .v-ripple__container {
        opacity: 0 !important;
    }
    .vFileInput {
        width: 35%;
    }

    /*.dialogHeight {
        height: 85vh;
    }*/

    .scrollBoxHeight {
        height: 46vh;
    }

    .historyHeight {
        height: 52vh;
    }

    .taskHeight {
        height: 63vh
    }

    .uploadBoxHeight {
        height: 53vh;
    }

    .uploadScrollHeight {
        height: 49vh;
    }

    @media screen and (max-width: 750px) {
        .vFileInput {
            width: 50%;
        }

        .dialogHeight {
            height: 85vh;
        }

        .scrollBoxHeight {
            height: 50vh;
        }

        .historyHeight {
            height: 56vh;
        }

        .taskHeight {
            height: 65vh
        }

        .uploadBoxHeight {
            height: 54vh;
        }

        .uploadScrollHeight {
            height: 48vh;
        }
    }

    .custom-text-field {
        width: 100%;
        border: 1px solid #c2c2c2;
        color: rgba(0, 0, 0, 0.6);
        border-radius: 4px;
        transition: border 0.3s cubic-bezier(0.25, 0.8, 0.5, 1);
        margin-bottom: 16px;
        -webkit-transition: border 0.3s;
        padding: 5px 10px 5px 10px;
        min-height: 40px;
        font-size: 16px;
    }

    .custom-text-field:focus {
        border: 2px solid #3374B9;
        outline: none;
    }
</style>
