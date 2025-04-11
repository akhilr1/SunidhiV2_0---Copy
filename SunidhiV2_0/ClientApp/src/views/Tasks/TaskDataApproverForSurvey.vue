<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0" >
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12" class="py-1 mb-0">
                <div v-for="(item,ind) in MyTask" :key="item.TaskId" class="pa-0 ma-0">
                    <v-row class="d-sm-none pa-0 ma-0 mb-2">
                        <v-col cols="12" align="center" class="pa-0 ma-0">
                            <v-chip color="#E1EFFF">
                                {{new Date(item.TaskDate).toLocaleDateString('en-GB')}}
                            </v-chip>
                        </v-col>
                    </v-row>
                    <v-card class="mb-2 mb-md-4 task-survey-card" @click.native="fnUpdateTask(item)">
                        <v-card-title class="py-0 my-0 pt-2">
                            <v-row no-gutters class="py-0 my-0">
                                <v-col class="py-0 my-0">
                                    <h4 class="font-weight-bold heading-3-dark">
                                        {{item.ActivityName}}
                                    </h4>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <v-chip color="rgba(33,150,243,.2)" class="ml-0 mr-2 task-survey-chip" label dark small>
                                        {{item.TaskType}}
                                    </v-chip>
                                </v-col>
                                <v-col cols="auto" class="d-none d-sm-block py-0 my-0">
                                    <h1 class="font-weight-regular heading-3">
                                        {{new Date(item.TaskSubmittedDate).toLocaleDateString('en-GB')}}
                                    </h1>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <!--<v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right" @click.stop="fnButton()">
                                <v-icon color="black">
                                    mdi-dots-vertical
                                </v-icon>
                            </v-btn>-->
                                    <v-menu offset-y transition="scroll-y-transition" v-model="MyTask[ind].vMenu">
                                        <template v-slot:activator="{ on }">
                                            <v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right; border:none; outline:none"
                                                   @click.stop="MyTask[ind].vMenu = true,IssueReportTaskDetails =item">
                                                <v-icon>
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
                                {{item.Description}}
                            </p>
                        </v-card-text>
                        <v-card-actions class="px-4 py-0 pb-3">
                            <v-row class="py-0">
                                <v-col class="py-0">
                                    <h4 class="font-weight-regular heading-3">
                                        <span>Project Name :</span> {{item.ProjectName}}
                                    </h4>
                                </v-col>
                                <v-spacer></v-spacer>
                                <v-col cols="auto" class="py-0">
                                    <v-chip color="#2C4258" label dark small class="task-bottom-chip">
                                        To Approve
                                    </v-chip>
                                </v-col>
                            </v-row>
                        </v-card-actions>
                    </v-card>
                </div>
            </v-col>
            <!-- report Issue dilog -->
            <v-dialog v-model="ReportIssuedlg" width="800" transition="dialog-bottom-transition">
                <v-card>
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                            <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                        </v-avatar>Report Issue
                        <v-spacer></v-spacer>
                        <v-btn text small @click="ReportIssuedlg=false">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col>
                                <h4 class="text-left heading-3 mb-2">
                                    Issue
                                    <span style="color:red" class="mr-2">*</span>
                                </h4>
                                <v-textarea rows="5" outlined dense label="Description" v-model="vmIssueComment" type="text">
                                </v-textarea>
                                <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" v-model="IssuefileInput" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                              id="file" accept="*"></v-file-input>
                            </v-col>
                        </v-row>
                        <v-row class="py-0 my-2">
                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right">
                                <Secondary-Button class="btn-122x36 mr-2" title="Update" v-bind:disabled="vmIssueComment.length == 0" @click.native="ReportIssue()"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="vmIssueComment ='',IssuefileInput= null,IssueReportTaskDetails =[],ReportIssuedlg = false "></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <!-- Survey Approve-->
            <v-dialog v-model="SurveyApproverdlg" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="fnInitializeTaskApprove()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                            </v-avatar>
                            {{MyTaskType.ActivityName}}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                            <v-chip class="ml-2 task-survey-chip" label dark small>
                                Survey
                            </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2" align="right" class="d-none d-sm-inline">
                            <v-btn text small @click="fnInitializeTaskApprove()">
                                <v-icon color="#707070" size="20">mdi-close</v-icon>
                            </v-btn>
                        </v-col>
                    </v-row>
                    <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
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
                                                <h4 class="font-weight-regular heading-3"> {{MyTaskType.ActivityName}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Action
                                                </h4>
                                                <v-chip color="#2C4258" label dark small>
                                                    To Approve
                                                </v-chip>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{new Date(MyTaskType.TaskDate).toLocaleDateString('en-GB')}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyTaskType.ProjectName}}</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            {{MyTaskType.Description}}
                                        </h4>
                                        <v-row>
                                            <!--<v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Target
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">50</h4>
                                            </v-col>-->
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Periodicity
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">
                                                    {{MyTaskType.Periodicity}}
                                                </h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-tick</v-icon>
                                                </v-avatar>
                                                Updated
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
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
                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0">{{item.TaskUpdateComment}}</h4>
                                                                </v-list-item-subtitle>
                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                    <v-row>
                                                                        <v-col md="8">
                                                                            <v-btn v-show="item.AttachedDocumentName.length >0" color="grey" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDatacollectorDoc(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>
                                                                                <span class="text-truncate" style="max-width:100px">{{item.DocumentName}}</span>
                                                                                <span>{{item.DocumentTypeIcon}}</span>
                                                                            </v-btn>
                                                                            <!--<v-btn v-show="item.AttachedDocumentName.length >0 " color="red" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDatacollectorDoc(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
        <v-icon size="26" color="red">mdi-file-download-outline</v-icon>{{item.AttachedDocumentName}}
    </v-btn>-->
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
                                <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="selectedTaskTypeID =MyTaskType.ActivityId,GetBeneficiary(),IsToReview = true,beneficiaryStepper=1"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="fnInitializeTaskApprove()"></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true && IsReviewFinished == false" v-for="Benficiary in CurrentBeneficiary" :key="Benficiary.BeneficiaryID" class="pa-3">
                        <v-card-text class="pa-0 ma-0" v-for="items in Benficiary.Sections" :key="items.SectionID">
                            <v-card elevation="0" class="pa-0 ma-0">
                                <div class="taskHeight">
                                    <v-card-title class="py-0 my-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pa-0 ma-0">
                                                <h4 class="text-center page-head mt-3">{{Benficiary.BeneficiaryName}} {{beneficiaryStepper}} of {{BeneficiaryList.length}}</h4>
                                                <h4 class="text-center page-head">Section {{QuestionStepper}} of {{selectionList.length}}</h4>
                                            </v-col>
                                        </v-row>
                                    </v-card-title>
                                    <v-card-subtitle class="my-0 py-0">
                                        <v-row>
                                            <v-col cols="6" md="8">
                                                <h4 class="text-left page-head mt-2">{{items.SectionName}}</h4>
                                            </v-col>
                                            <v-spacer class="d-sm-none"></v-spacer>
                                            <v-col cols="6" md="4">
                                                <v-select :items="selectionList" :item-text="i => i.SectionName" :item-value="i => i.SectionID" v-model="selectedsection" hide-details dense="" outlined
                                                          @change="GetSectionQuestionsBasedOnSectionOrder(selectedsection,Benficiary.BeneficiaryID)"></v-select>
                                            </v-col>
                                        </v-row>
                                    </v-card-subtitle>
                                    <v-card-text>
                                        <v-row class="v-scrolling-div scrollBoxHeightforDoubleHead py-0 my-0 mx-1">
                                            <v-col class="py-0 my-0">
                                                <div v-for="question in items.Questions">
                                                    <!-- Text -->
                                                    <!--<v-flex xs12 sm12 md12 class="mt-3">-->
                                                    <label class="text-left heading-3 mb-2 mt-3">{{question.QuestionName}}</label></br>
                                                    <label v-if="question.Answers" class="text-left heading-1 mt-3">{{question.Answers}}</label>
                                                    <label v-else style="color:#757575">No Response</label>
                                                    <sub v-if="question.QuestionType==='Numeric'" style="color:green;font-size:smaller">{{question.UOM}}</sub>

                                                    <!--</v-flex>-->


                                                    <div class="mb-6" v-if="question.QuestionType==='Photo Capture'">
                                                        <v-row>
                                                            <v-col md="8">
                                                                <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                            </v-col>
                                                        </v-row>
                                                    </div>
                                                    <!--GPS Capture-->
                                                    <div class="mb-6" v-if="question.QuestionType==='GPS Capture'">
                                                        <GmapMap :center="Coordinates"
                                                                 :zoom="13"
                                                                 style="width: 100%; height: 300px">
                                                            <GmapMarker :position="Coordinates"
                                                                        :clickable="true"
                                                                        :draggable="true" />

                                                        </GmapMap>
                                                    </div>
                                                    <div class="mb-6" v-if="question.QuestionType==='Signature Capture'">
                                                        <v-card>
                                                            <v-img :src=fileUrl+question.Answers max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                        </v-card>
                                                    </div>
                                                    <audio class="mt-0 mb-6" controls v-if="question.QuestionType==='Audio Capture'">
                                                        <source :src="fileUrl+question.Answers" type="audio/mp3">
                                                    </audio>
                                                    <div v-if="question.QuestionType==='File Upload'">
                                                        <v-row>
                                                            <!--<v-col md="8">
                <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
            </v-col>-->
                                                            <v-col md="8">
                                                                <v-btn color="grey" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDocument(question.Answers)">
                                                                    <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download
                                                                </v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </div>
                                                </div>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                </div>
                                <v-card-actions class="py-0 my-2">
                                    <v-col md="12" class="text-center text-md-right px-0 py-0 my-0">
                                        <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="GetNextBeneficiarySectionQuestion(Benficiary,items,'Next')"></Secondary-Button>
                                        <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="GetNextBeneficiarySectionQuestion(Benficiary,items,'Previous')"></Default-Button-Outlined>
                                    </v-col>
                                </v-card-actions>
                            </v-card>
                        </v-card-text>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsReviewFinished == true" class="px-3 py-1">
                        <div class="threeButtonTaskHeight">
                            <v-row>
                                <v-col cols="12" md="12">
                                    <h4 class="text-left page-head">In Case you have any comments please specify them below</h4>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <h4 class="text-left heading-3-dark mb-2">Add comments</h4>
                                    <v-textarea rows="5" outlined dense label="Description" type="text" v-model="TaskApproveDescription">
                                    </v-textarea>
                                    <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" prepend-inner-icon="mdi-paperclip" v-model="TaskApproveAttachment" prepend-icon="" outlined="" dense=""></v-file-input>
                                </v-col>
                            </v-row>
                        </div>
                            <v-row class="py-0 my-2">
                                <v-col md="12" class="text-center text-md-right py-0 my-2">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="ApproveTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Reject" @click.native="RejectTask()"></Default-Button-Outlined>
                                    <Default-Button-Outlined class="btn-122x36 mt-2 mt-md-0" title="Previous" @click.native="IsReviewFinished = false, IsToReview = true"></Default-Button-Outlined>
                                </v-col>
                            </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>


            <!-- Survey Approver non beneficiary linked-->
            <v-dialog v-model="SurveyNonBeneficiaryApproverdlg" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="fnInitializeTaskApprove()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                            </v-avatar>
                            {{MyTaskType.ActivityName}}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                            <v-chip class="ml-2 task-survey-chip" label dark small>
                                Survey
                            </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2" align="right" class="d-none d-sm-inline">
                            <v-btn text small @click="fnInitializeTaskApprove()">
                                <v-icon color="#707070" size="20">mdi-close</v-icon>
                            </v-btn>
                        </v-col>
                    </v-row>
                    <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
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
                                                <h4 class="font-weight-regular heading-3"> {{MyTaskType.ActivityName}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Action
                                                </h4>
                                                <v-chip color="#2C4258" label dark small>
                                                    To Approve
                                                </v-chip>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{new Date(MyTaskType.TaskDate).toLocaleDateString('en-GB')}}</h4>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyTaskType.ProjectName}}</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            {{MyTaskType.Description}}
                                        </h4>
                                        <v-row>
                                            <!--<v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Target
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">50</h4>
                                            </v-col>-->
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Periodicity
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyTaskType.Periodicity}}</h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" sm="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-tick</v-icon>
                                                </v-avatar>
                                                Updated
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
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
                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0">{{item.TaskUpdateComment}}</h4>
                                                                </v-list-item-subtitle>
                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                    <v-row>
                                                                        <v-col md="8">
                                                                            <v-btn v-show="item.AttachedDocumentName.length >0" color="grey" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDatacollectorDoc(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>
                                                                                <span class="text-truncate" style="max-width:100px">{{item.DocumentName}}</span>
                                                                                <span>{{item.DocumentTypeIcon}}</span>
                                                                            </v-btn>
                                                                            <!--<v-btn v-show="item.AttachedDocumentName.length >0" color="red" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDatacollectorDoc(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                            <v-icon size="26" color="red">mdi-file-download-outline</v-icon>{{item.AttachedDocumentName}}
                                                                            </v-btn>-->
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
                                <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="selectedTaskTypeID =MyTaskType.ActivityId,GetSections(selectedTaskTypeID),IsToReview = true,QuestionStepper=1"></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="fnInitializeTaskApprove()"></Default-Button-Outlined>
                            </v-col>
                        </v-row>
                    </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true && IsReviewFinished == false" v-for="items in CurrentSectionQuestions" :key="items.SectionID" class="pa-3">
                        <v-card elevation="0" class="pa-0 ma-0">
                            <div class="taskHeight">
                                <v-card-title class="my-0 py-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center page-head">Section {{QuestionStepper}} of {{selectionList.length}}</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-subtitle class="py-0 my-0">
                                    <v-row>
                                        <v-col cols="6" md="8">
                                            <h4 class="text-left page-head mb-2">{{items.SectionName}}</h4>
                                        </v-col>
                                        <v-spacer class="d-sm-none"></v-spacer>
                                        <v-col cols="6" md="4">
                                            <v-select :items="selectionList" :item-text="i => i.SectionName" :item-value="i => i.SectionID" v-model="selectedsection" hide-details dense="" outlined
                                                      @change="fnEditQuestion(selectedsection)"></v-select>
                                        </v-col>
                                    </v-row>
                                </v-card-subtitle>
                                <v-card-text>
                                    <v-row class="v-scrolling-div scrollBoxHeight py-0 my-0 mx-1">
                                        <v-col class="py-0 my-0">
                                            <div v-for="question in items.Questions" class="mb-4">
                                                <!-- Text -->
                                                <!--<v-flex xs12 sm12 md12 class="mt-3">-->
                                                <label class="text-left heading-3 mb-2 mt-3">{{question.QuestionName}}</label></br>
                                                <label v-if="question.Answers" class="text-left heading-1 mt-3">{{question.Answers}}</label>
                                                <label v-else style="color:#757575">No Response</label>
                                                <sub v-if="question.QuestionType==='Numeric'" style="color:green;font-size:smaller">{{question.UOM}}</sub>

                                                <!--</v-flex>-->


                                                <div class="mb-6" v-if="question.QuestionType==='Photo Capture'">
                                                    <v-row>
                                                        <v-col md="8">
                                                            <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                        </v-col>
                                                    </v-row>
                                                </div>
                                                <!--GPS Capture-->
                                                <div class="mb-6" v-if="question.QuestionType==='GPS Capture'">

                                                    <GmapMap :center="{lat:parseInt(question.Answers.split(',')[0]), lng:parseInt(question.Answers.split(',')[1])}"
                                                             :zoom="7"
                                                             style="width: 100%; height: 300px">
                                                        <GmapMarker :position="{lat:parseInt(question.Answers.split(',')[0]), lng:parseInt(question.Answers.split(',')[1])}"
                                                                    :clickable="true"
                                                                    :draggable="true" />
                                                    </GmapMap>
                                                    <!--<GmapMap :center="Coordinates"
                 :zoom="13"
                 style="width: 100%; height: 300px">
            <GmapMarker :position="Coordinates"
                        :clickable="true"
                        :draggable="true" />

        </GmapMap>-->
                                                </div>
                                                <div class="mb-6" v-if="question.QuestionType==='Signature Capture'">
                                                    <v-card>
                                                        <v-img :src=fileUrl+question.Answers max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                    </v-card>
                                                </div>
                                                <audio class="mb-6" controls v-if="question.QuestionType==='Audio Capture'">
                                                    <source :src="fileUrl+question.Answers" type="audio/mp3">
                                                </audio>
                                                <div v-if="question.QuestionType==='File Upload'">
                                                    <v-row>
                                                        <!--<v-col md="8">
                <v-img :src=fileUrl+question.Answers width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
            </v-col>-->
                                                        <v-col md="8">
                                                            <v-btn color="grey" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDocument(question.Answers)">
                                                                <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download
                                                            </v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </div>
                                            </div>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </div>
                            <v-card-actions class="py-0 my-2">
                                <v-col md="12" class="text-center text-md-right py-0 my-0">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="fnGetNextSurvey(items)"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="fnGetPreviousSurvey(items)"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsReviewFinished == true" class="px-3 py-1">
                        <div class="threeButtonTaskHeight">
                            <v-row>
                                <v-col cols="12" md="12">
                                    <h4 class="text-left page-head">In Case you have any comments please specify them below</h4>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <h4 class="text-left heading-3-dark mb-2">Add comments</h4>
                                    <v-textarea rows="5" outlined dense label="Comments" type="text" v-model="TaskApproveDescription">
                                    </v-textarea>
                                    <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" prepend-inner-icon="mdi-paperclip" v-model="TaskApproveAttachment" prepend-icon="" outlined="" dense=""></v-file-input>
                                </v-col>
                            </v-row>
                            </div>
                            <v-row class="py-0 my-2">
                                <v-col md="12" class="text-center text-md-right py-0 my-2">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="ApproveTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Reject" @click.native="RejectTask()"></Default-Button-Outlined>
                                    <Default-Button-Outlined class="btn-122x36 mt-2 mt-md-0" title="Previous" @click.native="IsReviewFinished = false, IsToReview = true"></Default-Button-Outlined>
                                </v-col>
                            </v-row>
                    </v-card-text>
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
    export default {
        data() {
            return {
                WorkspaceID: 0,
                lstTask: [],
                lstActivity: [],
                lstSurveyQstns: [],
                lstTasks: [],

                lstAllMyTask :[],
                MyTask: [],
                AuditLog: [],
                userimage: "",

              
                IsToReview: false,
                IsReviewFinished: false,
              
              
                IsToReview: false,
                IsReviewFinished: false,
                ActivityTabs: 'tab-1',
                QuestionStepper: 1,
                beneficiaryStepper:1,
                selectionList: [], 
                BeneficiaryList: [],
                Questions : [],
                selectedsection: "section 1",
                
                
                MyTaskType: {
                    ActivityId: '',
                    ActivityName: '',
                    Description: '',
                    ProjectName: '',
                    CreateDate: '',
                    BeneficiaryType: '',
                    Periodicity: '',
                    //Target: 50,
                    IsPaymentLined: false,
                    WorkflowId: '',
                    Status: '',
                    TaskType : '',               
                    TaskTypeID: '',
                    WFLevel: '',
                    SyncDBID: 0,
                    TypeIDColumn: '',
                    BeneficiarySubmittedList: [],
                    TaskDate:''
                },
                CurrentSectionQuestions: [],
                
                TaskApproveAttachment: null,
                TaskApproveDescription: "",
                ApproveDocName: "",
                CurrentBeneficiary: [],
                

                selectedTaskTypeID:"",
               

                // Survey Approve 
                SurveyApproverdlg: false,
                SurveySectionList: [],
                lstLogFrame: [],
                CurrentOrOverdueTask: '',
                CurrentDate: new Date().toISOString().substr(0, 10),

                fileUrl: "",
                ProjectID: "",
                docName: "",
                docUName: "",
                //GPS Capture
                Coordinates: {
                    lat: 0,
                    lng: 0,
                },
                //======ReportIssue
                ReportIssuedlg: false,
                IssuefileInput: null,
                vmIssueComment: "",
                IssueReportTaskDetails: [],

                //-- non beneficiary linked 
                SurveyNonBeneficiaryApproverdlg: false,

                //snackbar
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                lstSurveyResponse :[],
            }
        },
        props: ['searchcall', 'searchtask', 'Filter', 'DateRange', 'StartDate', 'EndDate', 'ProjectName', 'BeneficiaryName', 'WorkflowName', 'Action', 'FilterGUID'],
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.CurrentOrOverdueTask = this.$route.name;            
            await this.GetActivityApproverTaskData('');           
        },
        watch: {
            searchcall: function () {                
                this.GetActivityApproverTaskData(this.searchcall);
            },
            FilterGUID: function () {
                this.GetActivityApproverTaskData("");
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
           
            async fnUpdateTask(item) {

                var lastitem = await dbImworks.MyTaskList.where("pkId").equals(item.pkId).toArray();
                if (lastitem != null && lastitem.length > 0) {
                    if (lastitem[0].WFLevelStatus != "Pending") {
                        this.infoSnackbarMsg = true;
                        this.Msg = "This task has been already Updated.";
                        return false;
                    }
                }

                if (item.TaskType == 'Survey') {
                     if (item.BeneficiaryList !=null) {
                         this.SurveyApproverdlg = true;
                    } else {
                         this.SurveyNonBeneficiaryApproverdlg = true;
                    }
                }
                this.Questions = [];
                this.BeneficiaryList = [];
                this.selectionList = [];
                this.MyTaskType.ActivityId = item.ActivityId;
                this.MyTaskType.ActivityName = item.ActivityName;
                this.MyTaskType.Description = item.Description;
                this.MyTaskType.ProjectName = item.ProjectName;
                this.MyTaskType.CreateDate = item.CreatedDate;
                this.MyTaskType.BeneficiaryType = item.BeneficiaryTypeName;
                this.MyTaskType.Periodicity = item.Periodicity;
                //this.MyTaskType.Target = 50,
                this.MyTaskType.IsPaymentLined = item.IsPaymentLined;
                this.MyTaskType.WorkflowId = item.WorkflowId;
                this.MyTaskType.TaskType = item.TaskType;
                this.MyTaskType.TaskTypeID = item.ActivityId; 
                this.MyTaskType.TypeIDColumn = item.TypeIDColumn; 
                this.MyTaskType.WFLevel = item.WFLevel;
                this.MyTaskType.SyncDBID = item.SyncDBID;

                this.Questions = item.SurveyQstnList;
                this.BeneficiaryTypeList = item.BeneficiaryList;
                this.BeneficiarySubmittedList = item.BeneficiarySubmittedList;
                
                this.MyTaskType.TaskDate = item.TaskDate;
                this.lstSurveyResponse = item.SurveyResponse;
                //this.lstLogFrame = this.AuditLog.filter(x => x.TypeID == this.MyTaskType.ActivityId && x.Type == "Survey");
                
                var lstfilterlist = await dbImworks.TbTaskAuditLog.toArray();
                this.lstLogFrame = lstfilterlist.filter(x => x.TypeID == item.TaskTypeID && x.Type == item.TaskType);
                this.lstLogFrame.sort((a, b) => b.pkLogID - a.pkLogID);
                
                const map = new Map();
                for (const item of this.Questions) {
                    if (!map.has(item.SectionID)) {
                        map.set(item.SectionID, true);
                        this.selectionList.push({
                            SectionID: item.SectionID,
                            SectionName: item.SectionName
                        });
                    }
                }
                if (this.BeneficiarySubmittedList != null) {
                const mapBeneficiary = new Map();
                for (const item of this.BeneficiarySubmittedList) {
                    if (!mapBeneficiary.has(item.BeneficiaryID)) {
                        mapBeneficiary.set(item.BeneficiaryID, true);
                        this.BeneficiaryList.push({
                            BeneficiaryID: item.BeneficiaryID,
                            BeneficiaryName: item.BeneficiaryName
                        });
                    }
                    }
                }

            },
         
            formatDate(date) {
                if (!date) return null

                const [year, month, daytime] = date.split('-')
                const [day, garbage] = daytime.split('T')
                return `${year}-${month}-${day}`
            },
            // Payment Linked Activity Methods
            fnCreateOrSelectExistingBenficiary(type) {

                if (type == 'New') {
                    this.CreateOrSelectExistingBenSteps= 3;

                } else if (type == 'Exist') {
                    this.CreateOrSelectExistingBenSteps = 2;
                }
            },
            fnFinalStepIncreament(type) {
                this.FinalStepvalue = type;
            },
            fnFinanlSubmit() {
                if (this.FinalStepvalue == 'Add Another') {
                    this.PaymnetLinkedActivitySteps = 1;
                } else if (this.FinalStepvalue == 'Complete') {
                    this.PaymentActvityDataCollectordlg = false;
                } else if (this.FinalStepvalue == 'Exit') {
                    this.PaymentActvityDataCollectordlg = false;
                }
            },
            fnUpdateBenfTasks(type) {
                if (type == 'Import') {
                    this.BeneficiaryUpdateImportOrManualStep = 7;
                } else if (type == 'Manual') {
                    this.BeneficiaryUpdateImportOrManualStep = 5;
                }
            },
            fnAddNewbeneficiary(type){
                if (type == 'Register') {
                    this.RegisterOrImportBeneficiary = 8;
                } else if (type == 'Import') {
                    this.RegisterOrImportBeneficiary = 9;
                }
            },

            filterTask(myListNew) {
                
                var filterTaskArray = [];
                filterTaskArray = myListNew;
                
                if (this.StartDate != "" && this.EndDate != "") {
                    filterTaskArray = filterTaskArray.filter(x => x.TaskSubmittedDate.substr(0, 10) >= this.StartDate && x.TaskSubmittedDate.substr(0, 10) <= this.EndDate);
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

            async GetActivityApproverTaskData(SearchV) {
               
                this.lstTask = [];
                this.lstAllMyTask = [];
                this.lstTask = await dbImworks.MyTaskList.toArray();

                var filterTaskArray = this.filterTask(this.lstTask);

                //Log Frame
                //this.AuditLog = await dbImworks.TbTaskAuditLog.toArray();
                if (this.Action == "To Approve" || this.Action == "" || this.Action == "All") {
                    var lstClientSubmitted = await dbImworks.TbMyTask.toArray();

                    //remove duplicate data
                    if (lstClientSubmitted != null)

                        for (var i = 0; i < lstClientSubmitted.length; i++) {
                            for (var j = 0; j < filterTaskArray.length; j++) {
                                var tasklistjson = JSON.parse(filterTaskArray[j].TaskData);
                                if (lstClientSubmitted[i].WFLevel > 0 && lstClientSubmitted[i].Type == filterTaskArray[j].Type && lstClientSubmitted[i].TaskDate == filterTaskArray[j].TaskDate
                                    && lstClientSubmitted[i].TypeID == filterTaskArray[j].TypeID && lstClientSubmitted[i].WFLevelStatus == "Completed"
                                    && filterTaskArray[j].Status == 'DataApproved' && tasklistjson.Periodicity != "Anytime") {

                                    filterTaskArray.splice(j, 1);
                                    filterTaskArray = [...filterTaskArray];
                                }
                            }
                        }

                    for (var i = 0; i < filterTaskArray.length; i++) {
                        if (filterTaskArray[i].Type == 'Survey' && filterTaskArray[i].WFLevel > 0 && filterTaskArray[i].WFLevelStatus == 'Pending') {

                            var objSurvey = JSON.parse(filterTaskArray[i].TaskData);
                            if (objSurvey != null) {
                                this.lstActivity = [];
                                this.lstActivity.push(objSurvey);

                                var McuntAct = 0;
                                var myactivityDataListN = [];
                                if (SearchV != '') {
                                    var mySurveyList = this.lstActivity.filter(x => ((x.SurveyName.toLowerCase().includes(SearchV.toLowerCase()))));
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct = 1;
                                    };
                                    if (mySurveyList.length > 0) {
                                        myactivityDataListN = mySurveyList.filter(x => ((x.SurveyID == filterTaskArray[i].TypeID)));
                                        if (myactivityDataListN.length > 0) {
                                            McuntAct = 1;
                                        }
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct = 1;
                                }
                                if (McuntAct == 1) {
                                    // debugger;
                                    this.lstAllMyTask.push({
                                        ActivityId: this.lstActivity[0].SurveyID,
                                        ActivityName: this.lstActivity[0].SurveyName, TaskType: this.lstActivity[0].Type, TaskDate: filterTaskArray[i].TaskDate, TaskSubmittedDate: filterTaskArray[i].TaskSubmittedDate,
                                        Description: this.lstActivity[0].Description, ProjectName: this.lstActivity[0].ProjectName, SurveyQstnList: this.lstActivity[0].SurveyQstnList,
                                        BeneficiarySubmittedList: this.lstActivity[0].BeneficiarySubmittedList,
                                        WFLevel: filterTaskArray[i].WFLevel, BeneficiaryList: this.lstActivity[0].BeneficiaryList,
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskType: filterTaskArray[i].Type, TaskTypeID: filterTaskArray[i].TypeID, TypeIDColumn: filterTaskArray[i].TypeIDColumn, vMenu: false, WorkspaceID: filterTaskArray[i].WorkspaceID, Periodicity: this.lstActivity[0].Periodicity, pkId: filterTaskArray[i].pkId, SurveyResponse: this.lstActivity[0].SurveyResponse

                                    });
                                }
                                this.lstActivity = [];
                                this.ProjectID = objSurvey.ProjectId;
                            }

                        }
                    }
                }
                else {
                    this.MyTask = [];
                }
            
                this.fileUrl = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectID.toString() + "/";
             
                if (this.$route.name == "CurrentTasks")
                    this.MyTask = this.lstAllMyTask.filter(x => x.TaskSubmittedDate.substr(0, 10) == new Date().toISOString().substr(0, 10));

                else if (this.$route.name == "OverdueTasks")
                    this.MyTask = this.lstAllMyTask.filter(x => x.TaskSubmittedDate.substr(0, 10) < new Date().toISOString().substr(0, 10));
                this.MyTask.sort(function (a, b) {
                    return new Date(b.date) - new Date(a.date);
                });
            },          
           
            async GetCurrentSection(SectionOrder) {
                
                this.CurrentSectionQuestions = this.SurveySectionList.filter(x => x.SectionOrder == SectionOrder);
                this.selectedsection = this.CurrentSectionQuestions[0].SectionID;
               
            },
            async fnGetNextSurvey(item) {
                if (item.SectionOrder < this.selectionList.length) {
                    var CurrentOrder = item.SectionOrder;
                    CurrentOrder = CurrentOrder + 1;
                    this.QuestionStepper = CurrentOrder;

                    await this.GetCurrentSection(CurrentOrder);
                } else {
                   
                    this.IsReviewFinished = true,
                        this.IsToReview = false
                }
            },
			async fnGetPreviousSurvey(item)
            {
                
                if (item.SectionOrder > 1) {
                    var CurrentOrder = item.SectionOrder;
                    CurrentOrder = CurrentOrder -1;
                    this.QuestionStepper = CurrentOrder;

                    await this.GetCurrentSection(CurrentOrder);
                } else {

                    this.IsReviewFinished = false;
                        this.IsToReview = false;
                }
            },
            async fnEditQuestion(sectionId) {
                var section = this.SurveySectionList.filter(x => x.SectionID == sectionId);
                await this.GetCurrentSection(section[0].SectionOrder);
                this.QuestionStepper = section[0].SectionOrder;
            },

            async fnInitializeTaskApprove() {
                    this.IsReviewFinished = false;
                    this.IsToReview = false;
                   
                    this.SurveyApproverdlg = false;
                    this.TaskApproveDescription = "";
                    this.TaskApproveAttachment = null;
                    this.SurveySectionList = [];
                    this.QuestionStepper = 1;
                this.ActivityTabs = 'tab-1';
                //-- non beneficiary Linked 
                this.SurveyNonBeneficiaryApproverdlg = false;
            },         
           

            async ApproveTask() {
                
                var lstTaskResponse = [];
                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }

                var today = new Date().toLocaleString("en-US");
                var totWF = this.lstTask.filter(x => (x.WFLevel > 0) && (x.Type == this.MyTaskType.TaskType) &&
                                (x.TypeID == this.MyTaskType.TaskTypeID) && (x.TaskDate == this.MyTaskType.TaskDate))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID; //.lstTask.filter(x => (x.WFLevel = this.MyTaskType.WFLevel) && (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].SyncDBID;
                
                var task = this.lstTask.filter(x => (x.WFLevel > 0) && (x.Type == this.MyTaskType.TaskType)
                    && (x.TypeID == this.MyTaskType.TaskTypeID) && (x.TaskDate == this.MyTaskType.TaskDate) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                var SurveyAnswers = JSON.stringify(this.SurveySectionList);
 
                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, TypeIDColumn: this.MyTaskType.TypeIDColumn, Status: 'DataApproved',
                    WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel,WFLevelStatus: "Completed", TaskUpdateComment: this.TaskApproveDescription,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: SurveyAnswers, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID

                });
                
                var TaskStatus = "DataApproved";
                if (totWF == this.MyTaskType.WFLevel) {
                    TaskStatus ="Completed"
                }
                await dbImworks.TaskSyncDB.put({
                    Type: this.MyTaskType.TaskType, Command: 'TaskResponse', TypeID: this.MyTaskType.TaskTypeID, TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: TaskStatus, WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse),
                    Periodicity: this.MyTaskType.Periodicity,lastSyncDBID: lastSyncDBID, IsBeneficiaryLinked: true, TaskDate: this.MyTaskType.TaskDate, WorkspaceID: task.WorkspaceID
                });
                
                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Completed",
                    TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.MyTaskType.ProjectName,
                    TaskName: this.MyTaskType.ActivityName, Description: this.MyTaskType.Description, CreatedDate: today,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID
                });
                if (this.TaskApproveAttachment != null) {
                    await dbImworks.TbTaskDocument.put({

                        SyncTaskID: lastSyncDBID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: this.MyTaskType.WFLevel, AttachedDocument: this.TaskApproveAttachment,
                        AttachedDocumentName: this.TaskApproveAttachment.name, DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                        DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0), DocumentType: this.TaskApproveAttachment.type, DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                        DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                        ProjectID: this.ProjectID, ProjectName: this.MyTaskType.ProjectName, DocLinkedTo: this.MyTaskType.ActivityName, TaskDate: this.MyTaskType.TaskDate
                    });
                }
                
              
                //var CompletedTask = this.MyTask.filter(x => (x.ActivityId == this.MyTaskType.ActivityId) && (x.SyncDBID == this.MyTaskType.SyncDBID)
                //    && (x.TaskDate == this.MyTaskType.TaskDate));
                //if (CompletedTask.length > 0) {
                //    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                //    this.MyTask = [...this.MyTask];
                //}

                var index = this.MyTask.findIndex(x => (x.ActivityId == this.MyTaskType.ActivityId) && (x.SyncDBID == this.MyTaskType.SyncDBID) && (x.TaskDate == this.MyTaskType.TaskDate));
                var CompletedTask = this.MyTask.filter(x => (x.ActivityId == this.MyTaskType.ActivityId) && (x.SyncDBID == this.MyTaskType.SyncDBID) && (x.TaskDate == this.MyTaskType.TaskDate));

                if (index >= 0) {
                    this.MyTask.splice(index, 1);
                    this.MyTask = [...this.MyTask];
                }

                if (CompletedTask != null && CompletedTask != undefined && CompletedTask.length > 0) {
                    await dbImworks.MyTaskList.where("pkId").equals(CompletedTask[0].pkId).modify({ WFLevelStatus: 'Completed' });
                }

                // await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Completed' });
                //await this.GetActivityApproverTaskData();
                this.fnInitializeTaskApprove();
                this.Msg = "Task Approved Successfully";
                this.successSnackbarMsg = true;
               
            } ,
            async RejectTask() {
                
                var lstTaskResponse = [];
                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }

                var today = new Date().toLocaleString("en-US");
                var totWF = this.lstTask.filter(x => (x.WFLevel > 0) && (x.Type == this.MyTaskType.TaskType) &&
                    (x.TypeID == this.MyTaskType.TaskTypeID) && (x.TaskDate == this.MyTaskType.TaskDate))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID; //.lstTask.filter(x => (x.WFLevel = this.MyTaskType.WFLevel) && (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].SyncDBID;

                var task = this.lstTask.filter(x => (x.WFLevel > 0) && (x.Type == this.MyTaskType.TaskType)
                    && (x.TypeID == this.MyTaskType.TaskTypeID) && (x.TaskDate == this.MyTaskType.TaskDate) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, TypeIDColumn: this.MyTaskType.TypeIDColumn, Status: 'Rejected',
                    WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Rejected", TaskUpdateComment: this.TaskApproveDescription,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: null, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID

                });
               
                await dbImworks.TaskSyncDB.put({
                    Type: this.MyTaskType.TaskType, Command: 'TaskResponse', TypeID: this.MyTaskType.TaskTypeID, TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: "Rejected", WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse),
                    lastSyncDBID: lastSyncDBID, IsBeneficiaryLinked: true, TaskDate: this.MyTaskType.TaskDate, WorkspaceID: task.WorkspaceID
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Rejected",
                    TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.MyTaskType.ProjectName,
                    TaskName: this.MyTaskType.ActivityName, Description: this.MyTaskType.Description, CreatedDate: this.MyTaskType.CreateDate,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, WorkspaceID: task.WorkspaceID
                });
                if (this.fileInput != null) {
                    await dbImworks.TbTaskDocument.put({

                        SyncTaskID: lastSyncDBID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: this.MyTaskType.WFLevel, AttachedDocument: this.TaskApproveAttachment,
                        AttachedDocumentName: this.TaskApproveAttachment.name, DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                        DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0), DocumentType: this.TaskApproveAttachment.type, DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                        DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                        ProjectID: this.ProjectID, ProjectName: this.MyTaskType.ProjectName, DocLinkedTo: this.MyTaskType.ActivityName, TaskDate: this.MyTaskType.TaskDate
                    });
                }


                var index = this.MyTask.findIndex(x => (x.ActivityId == this.MyTaskType.ActivityId) && (x.SyncDBID == this.MyTaskType.SyncDBID) && (x.TaskDate == this.MyTaskType.TaskDate));
                var CompletedTask = this.MyTask.filter(x => (x.ActivityId == this.MyTaskType.ActivityId) && (x.SyncDBID == this.MyTaskType.SyncDBID) && (x.TaskDate == this.MyTaskType.TaskDate));

                if (index >= 0) {
                    this.MyTask.splice(index, 1);
                    this.MyTask = [...this.MyTask];
                }

                if (CompletedTask != null && CompletedTask != undefined && CompletedTask.length > 0) {
                    await dbImworks.MyTaskList.where("pkId").equals(CompletedTask[0].pkId).modify({ WFLevelStatus: 'Rejected' });
                }

                //var CompletedTask = this.MyTask.filter(x => x.ActivityId == this.MyTaskType.ActivityId && x.TaskDate == this.MyTaskType.TaskDate);
                //if (CompletedTask.length > 0) {
                //    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                //    this.MyTask = [...this.MyTask];
                //}

               // await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Rejected' });
                //await this.GetActivityApproverTaskData();
                this.IsReviewFinished = false;
                this.IsToReview = false;
                this.SurveyApproverdlg = false;
                this.fnInitializeTaskApprove();
                this.Msg = "Task Rejected Successfully";
                this.successSnackbarMsg = true;

            },
            formatFileSize(bytes, decimalPoint) {

                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            //-------------Survey approval ----------------
            
            async GetBeneficiary() {
                
                this.BeneficiarySectionList = [];
                for (var BeneficiaryCount = 0; BeneficiaryCount <= this.BeneficiaryList.length - 1; BeneficiaryCount++) {
                    var object = new Object();
                    object.SurveyId = this.BeneficiaryList[BeneficiaryCount].SurveyId;
                    object.BeneficiaryID = this.BeneficiaryList[BeneficiaryCount].BeneficiaryID;
                    object.BeneficiaryName = this.BeneficiaryList[BeneficiaryCount].BeneficiaryName;
                    object.BeneficiaryOrder = BeneficiaryCount + 1;
                    object.Sections = await this.GetSectionsForBeneficiary(object.BeneficiaryID, object.BeneficiaryOrder);
                    
                    this.BeneficiarySectionList.push(object);

                }
                
                // ---
                await this.BeneficiaryLinkedSections(1);
                await this.GetCurrentBeneficiary(1);
            },

            // Gettings Sections under each beneficiaries
            async GetSectionsForBeneficiary(BeneficiaryId, BeneficiaryOrder) {
                
                ///this.SurveySectionList = [];
                for (var SectionCount = 0; SectionCount <= this.selectionList.length - 1; SectionCount++) {
                    
                    var SurveyQuestion = this.BeneficiarySubmittedList.filter(x => (x.SectionID == this.selectionList[SectionCount].SectionID) && (x.BeneficiaryID == BeneficiaryId));
                    if (SurveyQuestion.length > 0) {
                        
                        var object = new Object();
                        object.BeneficiaryID = BeneficiaryId;
                        object.BeneficiaryOrder = BeneficiaryOrder;
                        object.SurveyId = this.selectionList[SectionCount].SurveyId;
                        object.SurveyId = this.selectionList[SectionCount].SurveyId;
                        object.SectionID = this.selectionList[SectionCount].SectionID;
                        object.SectionName = this.selectionList[SectionCount].SectionName;
                        object.SectionOrder = SectionCount + 1;
                        object.Questions = await this.GetSectionQuestionsForBeneficiary(object.SectionID, BeneficiaryId);

                        this.SurveySectionList.push(object);
                    }
                }
                await this.GetCurrentBeneficiarySection(1,1);
                return this.CurrentSectionQuestions;
            },

            async GetSectionQuestionsForBeneficiary(SectionID, BeneficiaryID) {
                
                var SectionQuestionList = [];
                var SurveyQuestion = this.BeneficiarySubmittedList.filter(x => (x.SectionID == SectionID) && (x.BeneficiaryID == BeneficiaryID));
               
                for (var i = 0; i <= SurveyQuestion.length - 1; i++) {
                    var QItem = this.MyTask.filter(x => x.ActivityId == this.selectedTaskTypeID)[0].SurveyQstnList.filter(x => (x.SectionID == SectionID) && (x.QuestionID == SurveyQuestion[i].QuestionID));

                    var object = new Object();
                    object.ActivityId = this.MyTaskType.ActivityId;
                    object.SurveyId = SurveyQuestion[i].SurveyID;
                    object.SectionId = SurveyQuestion[i].SectionID;
                    object.QuestionID = SurveyQuestion[i].QuestionID;
                    object.QuestionName = QItem[0].QuestionName;

                    object.QuestionTypeID = QItem[0].QuestionTypeID;
                    object.QuestionType = QItem[0].QuestionType;
                    object.IsMandatory = QItem[0].IsMandatory;
                    object.UOM = QItem[0].UOM;
                    object.Answers = SurveyQuestion[i].Answer1;
                  
                    
                    SectionQuestionList.push(object);
                    if (QItem[0].QuestionType == 'GPS Capture') {  
                        if (SurveyQuestion[i].Answer1 != null) {
                            var arr = SurveyQuestion[i].Answer1.split(",");
                            this.Coordinates.lat = parseInt(arr[0]);
                            this.Coordinates.lng = parseInt(arr[1]);
                        }
                        
                    }
                }
                
                return SectionQuestionList;
            },

            // For gettings Current Beneficiary's current section
            async GetCurrentBeneficiarySection(SectionOrder,BeneficiaryOrder) {
                
                this.CurrentSectionQuestions = this.SurveySectionList.filter(x => (x.SectionOrder == SectionOrder) && (x.BeneficiaryOrder == BeneficiaryOrder));
                this.selectedsection = this.CurrentSectionQuestions[0].SectionID;
            },

            async GetCurrentBeneficiary(BeneficiaryOrder) {
                
                this.CurrentBeneficiary = this.BeneficiarySectionList.filter(x => x.BeneficiaryOrder == BeneficiaryOrder); 
            
            },



            async GetSectionQuestionsBasedOnSectionOrder(sectionId,BeneficiaryId) {
                
                var section = this.SurveySectionList.filter(x => (x.SectionID == sectionId) && (x.BeneficiaryID == BeneficiaryId));
                await this.GetCurrentBeneficiarySection(section[0].SectionOrder, section[0].BeneficiaryOrder);
                this.CurrentBeneficiary[0].Sections = this.CurrentSectionQuestions;
                //var section = currentBeneficiary[0].Sections.filter(x => x.SectionID == sectionId);
                //await this.GetCurrentSection();
                this.QuestionStepper = section[0].SectionOrder;
              //  debugger;
            },
            async GetNextBeneficiarySectionQuestion(CurrentBeneficiary, CurrentSection , Type ) {
                
                if (Type == 'Next') {
                   
                    if (CurrentSection.SectionOrder < this.selectionList.length) {
                        // Change Questions   with in the same Beneficiary
                        var BeneficiaryOrder = CurrentBeneficiary.BeneficiaryOrder;
                        var NextSectionOrder = CurrentSection.SectionOrder + 1;
                        this.QuestionStepper = NextSectionOrder;
                        await this.GetCurrentBeneficiarySection(NextSectionOrder, BeneficiaryOrder);
                        this.CurrentBeneficiary[0].Sections = this.CurrentSectionQuestions;
                    } else if (CurrentSection.SectionOrder == this.selectionList.length) {
                        // Goto Next Beneficiary
                        if (CurrentBeneficiary.BeneficiaryOrder < this.BeneficiarySectionList.length) {
                            var NextBenficiary = CurrentBeneficiary.BeneficiaryOrder + 1;
                            await this.BeneficiaryLinkedSections(NextBenficiary);
                            this.beneficiaryStepper = NextBenficiary;
                            await this.GetCurrentBeneficiary(NextBenficiary);
                            await this.GetCurrentBeneficiarySection(1, NextBenficiary);
                            this.CurrentBeneficiary[0].Sections = this.CurrentSectionQuestions;
                            this.QuestionStepper = 1;
                        } else if (CurrentSection.SectionOrder == this.selectionList.length && CurrentBeneficiary.BeneficiaryOrder == this.BeneficiarySectionList.length) {
                            //  Review  Finished
                            this.IsReviewFinished = true;
                            this.IsToReview = false;
                        }
                    } 
                } 
                if (Type == 'Previous') { 
                    
                    if (CurrentSection.SectionOrder <= this.selectionList.length && CurrentSection.SectionOrder > 1) {
                        // Change Section Questions   with in the same Beneficiary
                        var BeneficiaryOrder = CurrentBeneficiary.BeneficiaryOrder;
                        var PreviousSectionOrder = CurrentSection.SectionOrder - 1;
                        this.QuestionStepper = PreviousSectionOrder;
                        await this.GetCurrentBeneficiarySection(PreviousSectionOrder, BeneficiaryOrder);
                        this.CurrentBeneficiary[0].Sections = this.CurrentSectionQuestions;
                    } else if (CurrentSection.SectionOrder == 1) { 
                        // Goto Previous Beneficiary
                        
                        if (CurrentBeneficiary.BeneficiaryOrder <= this.BeneficiarySectionList.length && CurrentBeneficiary.BeneficiaryOrder >1) {
                            var PreviousBenficiary = CurrentBeneficiary.BeneficiaryOrder - 1;
                            await this.BeneficiaryLinkedSections(PreviousBenficiary);
                            this.beneficiaryStepper = PreviousBenficiary;
                            await this.GetCurrentBeneficiary(PreviousBenficiary);
                            await this.GetCurrentBeneficiarySection(1, PreviousBenficiary);
                            this.CurrentBeneficiary[0].Sections = this.CurrentSectionQuestions;
                            this.QuestionStepper = 1;

                        } 
						else if (CurrentSection.SectionOrder == 1 && CurrentBeneficiary.BeneficiaryOrder == 1) {
                            this.SurveySectionList = [];
                            this.QuestionStepper = 1;
                            this.IsReviewFinished = false;
                            this.IsToReview = false;
                        }
                    }
                }
            },
            async ReportIssue() {

                var lstTaskResponse = [];
                if (this.IssueReportTaskDetails != null) {

                    var SurveyAns = "";
                    var today = new Date().toLocaleString();

                    // Pushing to Task Response
                    lstTaskResponse.push({
                        SyncDBID: 0,
                        Type: "Issue",
                        TypeID: this.IssueReportTaskDetails.TaskTypeID,
                        TypeIDColumn: this.IssueReportTaskDetails.TypeIDColumn,
                        Status: 'Pending',
                        WorkflowID: 0,
                        TotalWFLevel: 0,
                        WFLevel: 0,
                        WFLevelStatus: "Completed",
                        TaskUpdateComment: this.vmIssueComment,
                        AttachedDocumentName: this.IssuefileInput == null ? "" : this.IssuefileInput.name,//this.IssuefileInput.name,
                        SurveyAnswers: SurveyAns,
                        ResponseSyncID: 0,
                        DataCollectedBy: this.IssueReportTaskDetails.DataCollectedBy,
                        NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                        TaskDate: today,
                        TaskSubmittedDate: today,
                        ReportingValue: 0,
                        Command: "ADD", WorkspaceID: this.IssueReportTaskDetails.WorkspaceID

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


                    await dbImworks.TbTaskDocument.put({

                        SyncTaskID: 0, Type: "Issue", TypeID: this.IssueReportTaskDetails.TaskTypeID, WFLevel: 0, AttachedDocument: this.IssuefileInput,
                        AttachedDocumentName: this.IssuefileInput.name, DocumentOriginalName: this.IssuefileInput.name.split('.').slice(0, -1).join('.'),
                        DocumentSize: this.formatFileSize(this.IssuefileInput.size, 0), DocumentType: this.IssuefileInput.type, DocumentTypeIcon: this.IssuefileInput.name.split('.').pop(),
                        DocumentLastModifiedDate: this.IssuefileInput.lastModifiedDate != null ? new Date(this.IssuefileInput.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.IssuefileInput.lastModifiedDate).toISOString().substr(11, 5) : null,
                        ProjectID: this.IssueReportTaskDetails.ProjectId, ProjectName: this.IssueReportTaskDetails.ProjectName,
                        DocLinkedTo: this.IssueReportTaskDetails.TaskName + "-Issue", TaskDate: this.IssueReportTaskDetails.TaskDate
                    });

                    this.vmIssueComment = "";
                    this.IssuefileInput = null;
                    this.ReportIssuedlg = false;
                    this.IssueReportTaskDetails = [];
                    this.IssueReportSuccessfully = true;
                }
            },

            // -- Non Beneficiary linked Survey approval
            async GetSections(TaskId) {
                
                // code to remove unanswered sections in skip logic for non beneficiary survey
                if (this.lstSurveyResponse.length > 0) {
                    //for (var SectionCount = 0; SectionCount <= this.selectionList.length - 1; SectionCount++)
                    for (var SectionCount = this.selectionList.length - 1; SectionCount >= 0; SectionCount--) {

                        // checking each section ID is existing in lstSurveyResponsse Array
                        var Sectiondtls = this.lstSurveyResponse.filter(x => x.SectionID == this.selectionList[SectionCount].SectionID)
                        if (Sectiondtls.length == 0) {
                            // if not exists no need to show that section in the dropdown
                            var Temp = this.selectionList.filter(x => x.SectionID == this.selectionList[SectionCount].SectionID)
                            const index = this.selectionList.findIndex((element, index) => {
                                if (element.SectionID === this.selectionList[SectionCount].SectionID) {
                                    return true
                                }
                            });
                            if (Temp.length > 0) {
                                this.selectionList.splice(index, 1);
                                //this.lstSections = [...this.lstSections];
                            }
                        }
                    }
                }
                // remove code ends here

                this.SurveySectionList = [];
                for (var SecCount = 0; SecCount <= this.selectionList.length - 1; SecCount++) {
                    var object = new Object();
                    object.SurveyId = TaskId;
                    object.SectionID = this.selectionList[SecCount].SectionID;
                    object.SectionName = this.selectionList[SecCount].SectionName;
                    object.SectionOrder = SecCount + 1;
                    object.Questions = await this.GetSectionQuestions(object.SectionID, TaskId)

                    this.SurveySectionList.push(object);


                }
                await this.GetCurrentSection(1);
            },
            async GetSectionQuestions(SectionID, TaskId) {
                var SectionQuestionList = [];
                var SurveyQuestion = this.Questions.filter(x => x.SectionID == SectionID);
                for (var i = 0; i <= SurveyQuestion.length - 1; i++) {
                    var object = new Object();
                    object.ActivityId = this.MyTaskType.ActivityId;
                    object.SurveyId = TaskId;
                    object.SectionId = SurveyQuestion[i].SectionID;
                    object.QuestionID = SurveyQuestion[i].QuestionID;
                    object.QuestionName = SurveyQuestion[i].QuestionName;
                    object.QuestionTypeID = SurveyQuestion[i].QuestionTypeID;
                    object.QuestionType = SurveyQuestion[i].QuestionType;
                    object.IsMandatory = SurveyQuestion[i].IsMandatory;

                    object.Answers = SurveyQuestion[i].Answer1;
                    SectionQuestionList.push(object);
                }
                return SectionQuestionList;
            },
           
            async DownloadDocument(docName) {
                
                var downloadFileName = docName;
                var docUrl = this.fileUrl + docName;

                fetch(docUrl).then(function (t) {
                    return t.blob().then((b) => {
                        var a = document.createElement("a");
                        a.href = URL.createObjectURL(b);
                        a.setAttribute("download", downloadFileName);
                        a.click();
                    });
                });
            },

            async DownloadDatacollectorDoc(docName, docUName) {
                
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
            // this method is used to remove the unanswered skip logic sections , in the case of beneficiary linked survey
            async BeneficiaryLinkedSections(BenficairyIndex) {

                if (this.BeneficiaryList.length >= 1) {
                    
                    if (this.BeneficiarySubmittedList.length > 0)
                    {
                        this.selectionList = [];
                        const map = new Map();
                        for (const item of this.Questions) {
                            if (!map.has(item.SectionID)) {
                                map.set(item.SectionID, true);
                                this.selectionList.push({
                                    SectionID: item.SectionID,
                                    SectionName: item.SectionName
                                });
                            }
                        }
                        
                        // Getting Beneficiary data based on index
                        var CurrentBenSection = this.SurveySectionList.filter(x => x.BeneficiaryOrder == BenficairyIndex); // by index or by order
                        if (this.selectionList.length > 0)
                        {
                            //for (var SectionCount = 0; SectionCount <= this.selectionList.length - 1; SectionCount++)
                            for (var SectionCount = this.selectionList.length - 1; SectionCount >=0 ; SectionCount--)
                            {
                                
                                // checking each section ID is existing in Current Beneficiary 
                                var Sectiondtls = CurrentBenSection.filter(x=> x.SectionID == this.selectionList[SectionCount].SectionID)
                                if (Sectiondtls.length == 0) {
                                    // if not exists no need to show that section in the dropdown
                                    var Temp = this.selectionList.filter(x => x.SectionID == this.selectionList[SectionCount].SectionID)
                                    const index = this.selectionList.findIndex((element, index) => {
                                        if (element.SectionID === this.selectionList[SectionCount].SectionID) {
                                            return true
                                        }
                                    });
                                    if (Temp.length > 0) {
                                        this.selectionList.splice(index, 1);
                                        //this.lstSections = [...this.lstSections];
                                    }
                                }
                            }
                        }

                        // to Reset Section Order 
                        var SurveySectionList = this.SurveySectionList.filter(x => x.BeneficiaryOrder == BenficairyIndex);
                        for (var Cnt1 = 0; Cnt1 <= SurveySectionList.length - 1; Cnt1++) {

                            this.SurveySectionList.filter(x => x.SectionID == SurveySectionList[Cnt1].SectionID && x.BeneficiaryOrder == BenficairyIndex)[0].SectionOrder = Cnt1 + 1;
                        }


                    }

                }
            }
        },
        components: {
          //  'Activity-DataCollector': ActivityDataCollector,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
        },
        computed: {
            FrmDate() {
                return this.formatDate(this.startDate)

            },
        }
    }
</script>

<style scoped>
    .vFileInput {
        width: 35%;
    }

    /*.dialogHeight {
        height: 70vh;
    }*/

    .scrollBoxHeight {
        height: 50vh;
    }
    .scrollBoxHeightforDoubleHead {
        height: 33vh;
    }

    .historyHeight {
        height: 52vh;
    }

    .taskHeight {
        height: 63vh
    }
    .threeButtonTaskHeight {
        height: 63vh
    }

    @media screen and (max-width: 750px) {
        .vFileInput {
            width: 50%;
        }

        .dialogHeight {
            height: 85vh;
        }

        .scrollBoxHeight {
            height: 49vh;
        }

        .scrollBoxHeightforDoubleHead {
            height: 45vh;
        }

        .historyHeight {
            height: 56vh;
        }

        .taskHeight {
            height: 65vh
        }

        .threeButtonTaskHeight {
            height: 58vh
        }
    }
</style>