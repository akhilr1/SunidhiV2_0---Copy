<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0">
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12" class="py-1 mb-0">
                <div v-for="item in MyTask" :key="item.TaskId" class="pa-0 ma-0">
                  <v-row  class="d-sm-none pa-0 ma-0 mb-2">
                    <v-col cols="12" align="center" class="pa-0 ma-0">
                      <v-chip color="#E1EFFF">
                        {{new Date(item.CreatedDate).toLocaleDateString('en-GB')}}
                      </v-chip>
                    </v-col>
                  </v-row>
                    <v-card
                      :class="{'task-issue-card' :item.TaskType=='Issue',
                            'task-activity-card' :item.TaskType=='Activity',
                            'task-kpi-card' :item.TaskType=='KPI',
                            'task-risk-card' :item.TaskType=='RISK',
                            'task-survey-card' :item.TaskType=='Survey'
                            }"
                      class="mb-2 mb-md-4" @click.native="fnUpdateTask(item)">
                        <v-card-title class="py-0 my-0 pt-2">
                            <v-row no-gutters class="py-0 my-0">
                                <v-col class="py-0 my-0">
                                    <h4 class="font-weight-bold heading-3-dark py-0 my-0">
                                        {{item.ActivityName}}
                                    </h4>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <v-chip :color="item.TypeColor" class="ml-0 mr-2" label dark small>
                                        Payment
                                    </v-chip>
                                </v-col>
                                <v-col cols="auto" class="d-none d-sm-block py-0 my-0">
                                    <h1 class="font-weight-regular heading-3">
                                        {{new Date(item.CreatedDate).toLocaleDateString('en-GB')}}
                                    </h1>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right; border:none; outline:none" @click.stop="fnButton()">
                                        <v-icon color="black">
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
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
                              <v-chip color="#2C4258" class="task-bottom-chip" label dark small>
                                Payment Approve
                              </v-chip>
                            </v-col>
                          </v-row>
                        </v-card-actions>
                    </v-card>
                </div>
            </v-col>

            <!-- Actvity Finance Approver dialog-->
            <v-dialog v-model="PaymentLinkedActivityApprovedlg" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="fnResetDailog()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary" >mdi-chart-areaspline</v-icon>
                            </v-avatar>
                            {{MyTaskType.ActivityName}}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                          <v-chip color="#00BBA2" class="ml-2" label dark small>
                            {{MyTaskType.TaskType}}
                          </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2" align="right" class="d-none d-sm-inline">
                          <v-btn text small @click="fnResetDailog()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                          </v-btn>
                        </v-col>
                     </v-row>
                  <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                  <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
                      <v-tabs color="#52B962" grow v-model="ActivityTabs" class="pt-0 elevation-0 taskHeight" >
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
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.ActivityName}}</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Action
                                              </h4>
                                              <v-chip color="#2C4258" label dark small>
                                                  Payment Approve
                                              </v-chip>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.CreateDate}}</h4>
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
                                      <h4 class="font-weight-regular heading-3" style="height:auto;max-height:100px;overflow-y:auto">
                                          {{MyTaskType.Description}}
                                      </h4>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Target
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">50</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Beneficiary Type
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.BeneficiaryType}}</h4>
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
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Advance Amount
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">
                                                  Rs. {{MyTaskType.PaymentLinkedBudget}}
                                              </h4>
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
                                                                  <!--<v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">-->
                                                                  <!--<v-chip v-show="item.AttachedDocumentName.length >0 " small color="red" class="mb-0">
            {{item.AttachedDocumentName}}
        </v-chip>-->
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
                      <v-row class="py-0 my-0" >
                          <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                              <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true,QuestionStepper=1"></Secondary-Button>
                              <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="PaymentLinkedActivityApprovedlg = false"></Default-Button-Outlined>
                          </v-col>
                      </v-row>
                  </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true" class="pa-3" >
                        <v-card elevation="0" class="pa-0 ma-0">
                                <v-card-title>
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0 ">
                                            <h4 class="text-left page-head">Do you want to approve the advance request?</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-text class="v-scrolling-div scrollBoxHeight">
                                    <v-row>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.PaymentLinkedBudget}}</h4>
                                        </v-col>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested by
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.CreatedBy}}</h4>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col>
                                            <h4 class="text-left heading-3-dark mb-2">Add comments</h4>
                                            <v-textarea rows="5" outlined dense label="Description" type="text" v-model="TaskApproveComments">
                                            </v-textarea>
                                            <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" prepend-inner-icon="mdi-paperclip" v-model="TaskApproveAttachment" prepend-icon="" outlined="" dense=""></v-file-input>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            <v-card-actions class="pa-0 pt-0 pt-md-1">
                                <v-col md="12" class="text-center text-md-right pa-0">
                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Previous" @click.native="PaymentTermFinanceApproverdlg = false,IsToReview = false"></Default-Button-Outlined>
                                    <Secondary-Button class="btn-122x36 mr-0 mr-md-2" title="Approve" @click.native="PaymentLinkedActivityApprovedlg = false,IsToReview = false, ApproveTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36 mt-2 mt-md-0" title="Reject" @click.native="IsToReview = false, PaymentLinkedActivityApprovedlg = false, RejectTask()"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                </v-card>
            </v-dialog>
            

            <!-- Payment Terms finanace approver-->
            <v-dialog v-model="PaymentTermFinanceApproverdlg" width="800" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="fnResetDailog()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                            </v-avatar>
                            {{MyTaskType.ActivityName}}
                        </v-col>
                         <v-col cols="4" class="text-right text-md-left">
                             <v-chip color="#00BBA2" class="ml-0" label dark small>
                              Payment
                            </v-chip>
                         </v-col>
                         <v-spacer></v-spacer>
                         <v-col cols="2" align="right" class="d-none d-sm-inline">
                           <v-btn text small @click="fnResetDailog()">
                             <v-icon color="#707070" size="20">mdi-close</v-icon>
                           </v-btn>
                        </v-col>
                 </v-row>
                  <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                  <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
                      <v-tabs color="#52B962" grow v-model="ActivityTabs" class="pt-0 elevation-0 taskHeight">
                          <v-tab href='#tab-1'>Task Details</v-tab>
                          <v-tab href='#tab-2'>History</v-tab>

                          <!-- Task Details-->
                          <v-tab-item value="tab-1">
                              <v-row justify="center" class="mt-4">
                                  <v-col md="12" class="v-scrolling-div historyHeight">
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Task Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.ActivityName}}</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Action
                                              </h4>
                                              <v-chip color="#2C4258" label dark small>
                                                  Payment Approve
                                              </v-chip>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.CreateDate}}</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Project Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.ProjectName}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Progress
                                              </h4>
                                              <v-progress-linear dark rounded
                                                                 background-color="#0A8C45"
                                                                 color="#52B962"
                                                                 height="15"
                                                                 value="15">15%</v-progress-linear>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Condition
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.Condition}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Advance Amount
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">
                                                  {{MyTaskType.AdvanceAmount}}
                                              </h4>
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
                                                                  <!--<v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">-->
                                                                  <v-chip v-show="item.AttachedDocumentName.length >0 " small color="red" class="mb-0">
                                                                      {{item.AttachedDocumentName}}
                                                                  </v-chip>
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
                      <v-row class="py-0 my-0">
                          <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                              <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true,QuestionStepper=1"></Secondary-Button>
                              <Default-Button-Outlined class="btn-122x36 mt-1 mt-md-0" title="Cancel" @click.native="PaymentTermFinanceApproverdlg = false"></Default-Button-Outlined>
                          </v-col>
                      </v-row>
                  </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true" class="pa-3">
                        <v-card elevation="0" class="pa-0 ma-0">
                                <v-card-title>
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0 ">
                                            <h4 class="text-left page-head">Do you want to approve the advance request?</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-text class="v-scrolling-div scrollBoxHeight">
                                    <v-row>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.RequestedAmount}}</h4>
                                        </v-col>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested by
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.CreatedBy}}</h4>
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
                                </v-card-text>
                            <v-card-actions class="pa-0 pt-0 pt-md-1">
                                <v-col md="12" class="text-center text-md-right pa-0">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="PaymentTermFinanceApproverdlg = false,IsToReview = false,ApproveTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36 mr-0 mr-md-2" title="Previous" @click.native="IsToReview = false"></Default-Button-Outlined>
                                    <Default-Button-Outlined class="btn-122x36 mt-2 mt-md-0" title="Reject" @click.native="IsToReview = false, PaymentTermFinanceApproverdlg = false,RejectTask()"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!-- Fund Approval-->
            <v-dialog v-model="FundApproverdlg" width="800" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="FundApproverdlg = false,IsToReview = false">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                            </v-avatar>
                            {{MyTaskType.ActivityName}}
                        </v-col>
                         <v-col cols="4" class="text-right text-md-left">
                            <v-chip color="#00BBA2" class="ml-2" label dark small>
                              Payment
                            </v-chip>
                         </v-col>
                         <v-spacer></v-spacer>
                         <v-col cols="2" align="right" class="d-none d-sm-inline">
                           <v-btn text small @click="FundApproverdlg = false,IsToReview = false">
                             <v-icon color="#707070" size="20">mdi-close</v-icon>
                           </v-btn>
                        </v-col>
                 </v-row>
                  <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                  <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
                      <v-tabs color="#52B962" grow v-model="ActivityTabs" class="pt-0 elevation-0 taskHeight">
                          <v-tab href='#tab-1'>Task Details</v-tab>
                          <v-tab href='#tab-2'>History</v-tab>

                          <!-- Task Details-->
                          <v-tab-item value="tab-1">
                              <v-row justify="center" class="mt-4">
                                  <v-col md="12" class="v-scrolling-div historyHeight">
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Project Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.ProjectName}}</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Action
                                              </h4>
                                              <v-chip color="#2C4258" label dark small>
                                                  Payment Approve
                                              </v-chip>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Project Description
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.Description}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.CreateDate}}</h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Total Budget
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.TotalBudget}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Status
                                              </h4>
                                              <div v-if="MyTaskType.FundedAmount == MyTaskType.TotalBudget && MyTaskType.TotalBudget != 0">
                                                  <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                                      <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                                  </v-avatar>
                                                  Fully Funded
                                              </div>
                                              <div v-if="MyTaskType.FundedAmount < MyTaskType.TotalBudget && MyTaskType.FundedAmount  != 0">
                                                  <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                      <v-icon size="15" color="#FCC735">mdi-minus</v-icon>
                                                  </v-avatar>
                                                  Partially Funded
                                              </div>
                                              <div v-if="MyTaskType.FundedAmount == 0 ">
                                                  <v-avatar color="#FDE4E4" size="20" class="mr-1">
                                                      <v-icon size="15" color="#F24646">mdi-close-circle-outline</v-icon>
                                                  </v-avatar>
                                                  Not Funded
                                              </div>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Donor Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.CreatedBy}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Donor Account
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">
                                                  {{MyTaskType.DonorAccount}}
                                              </h4>
                                          </v-col>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Fund Requested
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.RequestedAmount}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col cols="12" sm="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Other Donors
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">
                                                  {{MyTaskType.OtherDonors}}
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">
                                                  Fund Distributed: {{MyTaskType.OtherDonorsFund}}
                                              </h4>
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
                                                      <v-list-item>
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
                                                                  <!--<v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">-->
                                                                  <v-chip v-show="item.AttachedDocumentName.length >0 " small color="red" class="mb-0">
                                                                      {{item.AttachedDocumentName}}
                                                                  </v-chip>
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
                      <v-row class="py-0 my-0">
                          <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                              <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true,QuestionStepper=1"></Secondary-Button>
                              <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="PaymentTermFinanceApproverdlg = false"></Default-Button-Outlined>
                          </v-col>
                      </v-row>
                  </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true" class="pa-3">
                        <v-card elevation="0" class="pa-0 ma-0">
                                <v-card-title class="py-0 my-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0 pa-0 ma-0 ">
                                            <h4 class="text-left page-head">Do you want to approve the fund request?</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-text class="v-scrolling-div scrollBoxHeight">
                                    <v-row>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.RequestedAmount}}</h4>
                                        </v-col>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Fund Requested by
                                            </h4>
                                            <h4 class="font-weight-regular heading-3">{{MyTaskType.CreatedBy}}</h4>
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
                                </v-card-text>
                            <v-card-actions class="pa-0 pt-0 pt-md-1">
                                <v-col md="12" class="text-center text-md-right pa-0">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="FundApproverdlg = false,IsToReview = false,ApproveTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36 mr-0 mr-md-2" title="Previous" @click.native="IsToReview = false"></Default-Button-Outlined>
                                    <Default-Button-Outlined class="btn-122x36 mt-2 mt-md-0" title="Reject" @click.native="IsToReview = false, FundApproverdlg = false,RejectTask()"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
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
                lstPaymentTerm :[],
                lstSurveyQstns: [],
                lstTasks: [],

                MyTask: [],
                AuditLog: [],
                userimage: "",

                //---------------- Activity Apporover -----------//

                ActvityApproverdlg: false,
                IsToReview: false,
                IsReviewFinished: false,

                KPIApproverdlg: false,
                IsToReview: false,
                IsReviewFinished: false,
                ActivityTabs: 'tab-1',
                QuestionStepper: 1,
                beneficiaryStepper: 1,
                selectionList: [],
                BeneficiaryList: [],
                Questions: [],
                selectedsection: "section 1",
                MyTaskType: {
                    ActivityId: '',
                    ActivityName: '',
                    Description: '',
                    ProjectName: '',
                    CreateDate: '',
                    BeneficiaryType: '',
                    Periodicity: 'Once',
                    Target: 50,
                    IsPaymentLined: false,
                    WorkflowId: '',
                    Status: '',
                    TaskType: '',
                    TaskTypeID: '',
                    WFLevel: '',
                    SyncDBID: 0,
                    TypeIDColumn: '',
                    BeneficiarySubmittedList: [],
                    TaskDate: '',
                    PaymentLinkedBudget: 0,
                    CreatedBy: '',
                    RequestedAmount: 0,
                    AdvanceAmount: 0,
                    Condition: '',
                    TotalBudget: 0,
                    OtherDonors:'',
                    DonorAccount : '',
                    OtherDonorsFund: 0,
                    FundedAmount: 0,
                    DataCollectedBy :'',
                    FinanceWorkflowId : 0,
                    ResponseSyncID: 0,
                    TotalWFLevel :0,
                },
                CurrentSectionQuestions: [],
                
                TaskApproveAttachment: null,
                TaskApproveDescription: "",
                ApproveDocName: "",
                CurrentBeneficiary: [],
                docName: "",
                docUName: "",

                // Payment Linked Acticity Approve
                PaymentLinkedActivityApprovedlg: false,
                MyTaskPaymentLinkedActivity: {
                    TaskId: 1,
                    TaskName: 'Well Contruction',
                    TaskType: "Payment",
                    TaskDescription: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy  text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It    has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    TaskCreatedDate: "13/08/2020",
                    ProjectName: "Sample Project 1",
                    BeneficiaryType: "Sample Beneficiary 1",
                    Target: 50,
                    Periodicity: "Once",
                    Status: "Updated",
                    AdvanceAmount: "Rs.40000"
                },
                

                // Risk 
                RiskApproverdlg: false,

                // Survey Approve 
                SurveyApproverdlg: false,
                SurveySectionList: [],
                lstLogFrame: [],
                CurrentOrOverdueTask: '',
                CurrentDate: new Date().toISOString().substr(0, 10),

                ActvityFinanceApproverdlg: false,
                TaskApproveComments: '',
                TaskApproveAttachment: null,

                // Payment Terms approver
                PaymentTermFinanceApproverdlg: false,

                // Fund Approver
                FundApproverdlg: false,

                //snackbar
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
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

            fnButton() {
                alert("Button Clicked");
            },
            async fnUpdateTask(item) {
               
                if (item.TaskType == 'Activity') {
                    this.ActvityApproverdlg = true;
                } else if (item.TaskType == 'KPI') {
                    this.KPIApproverdlg = true;
                } else if (item.TaskType == 'Survey') {
                    this.SurveyApproverdlg = true;
                } else if (item.TaskType == 'Payment') {
                    this.PaymentLinkedActivityApprovedlg = true;
                } else if (item.TaskType == 'Payment Request') {
                    this.PaymentTermFinanceApproverdlg = true;
                } else if (item.TaskType == 'Fund Request') {
                    this.FundApproverdlg = true;
                }
                
                this.Questions = [];
                this.BeneficiaryList = [];
                this.selectionList = [];
                this.MyTaskType.ActivityId = item.ActivityId;
                this.MyTaskType.ActivityName = item.ActivityName;
                this.MyTaskType.Description = item.Description;
                this.MyTaskType.ProjectId = item.ProjectId;
                this.MyTaskType.ProjectName = item.ProjectName;
                this.MyTaskType.CreateDate = item.CreatedDate;
                this.MyTaskType.BeneficiaryType = item.BeneficiaryTypeName;
                this.MyTaskType.Periodicity = item.Periodicity;
                this.MyTaskType.Target = 50,
                this.MyTaskType.IsPaymentLined = item.IsPaymentLined;
                this.MyTaskType.WorkflowId = item.WorkflowId;
                this.MyTaskType.TaskType = item.TaskType;
                this.MyTaskType.TaskTypeID = item.TaskTypeID;
                this.MyTaskType.TypeIDColumn = item.TypeIDColumn;
                this.MyTaskType.WFLevel = item.WFLevel;
                this.MyTaskType.SyncDBID = item.SyncDBID;
                this.MyTaskType.PaymentLinkedBudget = item.PaymentLinkedBudget;
                this.MyTaskType.CreatedBy = item.CreatedBy;
                this.MyTaskType.RequestedAmount = item.RequestedAmount;
                this.MyTaskType.Condition = item.Condition;
                this.MyTaskType.TotalBudget = item.TotalBudget;
                this.MyTaskType.OtherDonors = item.OtherDonors;
                this.MyTaskType.DonorAccount = item.DonorAccount;
                this.MyTaskType.OtherDonorsFund = item.OtherDonorsFund; 
                this.MyTaskType.AdvanceAmount = item.AdvanceAmount;
                this.MyTaskType.FundedAmount = item.FundedAmount;
                this.MyTaskType.DataCollectedBy = item.DataCollectedBy;
                this.MyTaskType.FinanceWorkflowId = item.FinanceWorkflowId;
                this.MyTaskType.ResponseSyncID = item.ResponseSyncID;
                this.MyTaskType.TotalWFLevel = item.TotalWFLevel;
                this.Questions = [];//item.SurveyQstnList;
                this.BeneficiaryTypeList = [];//item.BeneficiaryList;
                this.BeneficiarySubmittedList = [];//item.BeneficiarySubmittedList;
                

                this.MyTaskType.TaskDate = item.TaskDate;
               
                this.lstLogFrame = this.AuditLog.filter(x => x.TypeID == this.MyTaskType.ActivityId && x.Type == this.MyTaskType.TaskType);
            },
            async fnResetDailog() {
                this.PaymentLinkedActivityApprovedlg = false;
                this.ActivityTabs = 'tab-1';
                this.QuestionStepper = 1;
                this.IsToReview = false;
                this.IsReviewFinished = false;
                this.PaymentTermFinanceApproverdlg = false;
                this.TaskApproveComments = "";
                this.TaskApproveAttachment = null;             
                
            },

            filterTask(lstTask) {
                var filterTaskArray = [];
                filterTaskArray = lstTask;

                //if (this.StartDate != "" && this.EndDate != "") {
                //    filterTaskArray = filterTaskArray.filter(x => x.CreatedDate.substr(0, 10) >= this.StartDate && x.CreatedDate.substr(0, 10) <= this.EndDate);
                //}

                
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
                this.MyTask = [];
                this.lstTask = await dbImworks.MyTaskList.toArray();
                var filterTaskArray = this.filterTask(this.lstTask);
                
                //Log Frame
                
                if (this.Action == "To Approve" || this.Action == "" || this.Action == "All") {
                    this.AuditLog = await dbImworks.TbTaskAuditLog.toArray();
                    var lstClientSubmitted = await dbImworks.TbMyTask.toArray();

                    //remove duplicate data
                    if (lstClientSubmitted != null)
                        for (var i = 0; i < lstClientSubmitted.length; i++) {
                            for (var j = 0; j < filterTaskArray.length; j++) {

                                if (lstClientSubmitted[i].WFLevel > 0 && lstClientSubmitted[i].Type == filterTaskArray[j].Type
                                    && lstClientSubmitted[i].TypeID == filterTaskArray[j].TypeID && lstClientSubmitted[i].WFLevelStatus == "Completed"
                                    && filterTaskArray[j].Status == 'DataApproved') {
                                    filterTaskArray.splice(j, 1);
                                    filterTaskArray = [...filterTaskArray];
                                }
                            }
                        }

                    for (var i = 0; i < filterTaskArray.length; i++) {
                        if (filterTaskArray[i].Type == 'Payment Request') {

                            var objPaymentTerm = JSON.parse(filterTaskArray[i].TaskData);
                            this.lstPaymentTerm.push(objPaymentTerm);
                            if (this.CurrentOrOverdueTask == 'CurrentTasks' && this.formatDate(this.lstPaymentTerm[0].CreatedDate) == this.CurrentDate) {

                                var McuntAct = 0;
                                var myactivityDataListN = [];
                                if (SearchV != '') {
                                    var myActivityList = this.lstPaymentTerm.filter(x => ((x.PaymentTerm.toLowerCase().includes(SearchV.toLowerCase()))));
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct = 1;
                                    };
                                    if (myActivityList.length > 0) {
                                        myactivityDataListN = myActivityList.filter(x => ((x.REQUESTID == filterTaskArray[i].TypeID)));
                                        if (myactivityDataListN.length > 0) {
                                            McuntAct = 1;
                                        }
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct = 1;
                                }

                                if (McuntAct == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstPaymentTerm[0].REQUESTID,
                                        ActivityName: this.lstPaymentTerm[0].PaymentTerm,
                                        TaskType: filterTaskArray[i].Type,
                                        CreatedDate: this.lstPaymentTerm[0].CreatedDate,
                                        Description: this.lstPaymentTerm[0].Description,
                                        ProjectName: this.lstPaymentTerm[0].ProjectName,
                                        SurveyQstnList: [],
                                        WFLevel: 0,
                                        BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: 'none',
                                        PaymentLinkedBudget: '0',
                                        CreatedBy: this.lstPaymentTerm[0].RequestedBy,
                                        RequestedAmount: this.lstPaymentTerm[0].RequestedAmount,
                                        AdvanceAmount: this.lstPaymentTerm[0].AdvanceAmount,
                                        Condition: this.lstPaymentTerm[0].Condition,

                                    });
                                }
                            } else if (this.CurrentOrOverdueTask == 'OverdueTasks' && this.formatDate(this.lstPaymentTerm[0].CreatedDate) < this.CurrentDate) {

                                var McuntAct1 = 0;
                                var myactivityDataListN = [];
                                if (SearchV != '') {
                                    var myActivityList = this.lstPaymentTerm.filter(x => ((x.PaymentTerm.toLowerCase().includes(SearchV.toLowerCase()))));
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct1 = 1;
                                    };
                                    if (myActivityList.length > 0) {
                                        myactivityDataListN = myActivityList.filter(x => ((x.REQUESTID == filterTaskArray[i].TypeID)));
                                        if (myactivityDataListN.length > 0) {
                                            McuntAct1 = 1;
                                        }
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct1 = 1;
                                }

                                if (McuntAct1 == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstPaymentTerm[0].REQUESTID,
                                        ActivityName: this.lstPaymentTerm[0].PaymentTerm,
                                        TaskType: filterTaskArray[i].Type,
                                        CreatedDate: this.lstPaymentTerm[0].CreatedDate,
                                        Description: this.lstPaymentTerm[0].Description,
                                        ProjectName: this.lstPaymentTerm[0].ProjectName,
                                        SurveyQstnList: [],
                                        WFLevel: 0,
                                        BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: '',
                                        PaymentLinkedBudget: '0',
                                        CreatedBy: this.lstPaymentTerm[0].RequestedBy,
                                        RequestedAmount: this.lstPaymentTerm[0].RequestedAmount,
                                        AdvanceAmount: this.lstPaymentTerm[0].AdvanceAmount,
                                        Condition: this.lstPaymentTerm[0].Condition,
                                    });
                                }
                            }

                            this.lstPaymentTerm = [];
                        }
                        if (filterTaskArray[i].Type == 'Payment') {

                            var objActivity = JSON.parse(filterTaskArray[i].TaskData);
                            this.lstActivity.push(objActivity);
                            if (this.CurrentOrOverdueTask == 'CurrentTasks' && this.formatDate(filterTaskArray[i].TaskSubmittedDate) == this.CurrentDate) {

                                var McuntAct2 = 0;
                                var myactivityDataListN = [];
                                if (SearchV != '') {
                                    var myActivityList = this.lstActivity.filter(x => ((x.ActivityName.toLowerCase().includes(SearchV.toLowerCase()))));
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct2 = 1;
                                    };
                                    if (myActivityList.length > 0) {
                                        myactivityDataListN = myActivityList.filter(x => ((x.ActivityId == filterTaskArray[i].TypeID)));
                                        if (myactivityDataListN.length > 0) {
                                            McuntAct2 = 1;
                                        }
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct2 = 1;
                                }
                                if (McuntAct2 == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstActivity[0].ActivityId,
                                        ActivityName: this.lstActivity[0].ActivityName,
                                        TaskType: this.lstActivity[0].Type,
                                        CreatedDate: filterTaskArray[i].TaskSubmittedDate,
                                        Description: this.lstActivity[0].Description,
                                        ProjectName: this.lstActivity[0].ProjectName,
                                        SurveyQstnList: this.lstActivity[0].SurveyQstnList,
                                        WFLevel: filterTaskArray[i].WFLevel, BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskType: filterTaskArray[i].Type,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: this.lstActivity[0].Periodicity,
                                        PaymentLinkedBudget: this.lstActivity[0].PaymentLinkedBudget,
                                        CreatedBy: this.lstActivity[0].vaCreatedBy
                                    });
                                }
                            } else if (this.CurrentOrOverdueTask == 'OverdueTasks' && this.formatDate(filterTaskArray[i].TaskSubmittedDate) < this.CurrentDate) {

                                var McuntAct3 = 0;
                                var myactivityDataListN = [];
                                if (SearchV != '') {
                                    var myActivityList = this.lstActivity.filter(x => ((x.ActivityName.toLowerCase().includes(SearchV.toLowerCase()))));
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct3 = 1;
                                    };
                                    if (myActivityList.length > 0) {
                                        myactivityDataListN = myActivityList.filter(x => ((x.ActivityId == filterTaskArray[i].TypeID)));
                                        if (myactivityDataListN.length > 0) {
                                            McuntAct3 = 1;
                                        }
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct3 = 1;
                                }
                                if (McuntAct3 == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstActivity[0].ActivityId,
                                        ActivityName: this.lstActivity[0].ActivityName,
                                        TaskType: this.lstActivity[0].Type,
                                        CreatedDate: filterTaskArray[i].TaskSubmittedDate,
                                        Description: this.lstActivity[0].Description,
                                        ProjectName: this.lstActivity[0].ProjectName,
                                        SurveyQstnList: this.lstActivity[0].SurveyQstnList,
                                        WFLevel: filterTaskArray[i].WFLevel, BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskType: filterTaskArray[i].Type,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: this.lstActivity[0].Periodicity,
                                        PaymentLinkedBudget: this.lstActivity[0].PaymentLinkedBudget,
                                        CreatedBy: this.lstActivity[0].vaCreatedBy
                                    });
                                }
                            }

                            this.lstActivity = [];
                        }
                        if (filterTaskArray[i].Type == 'Fund Request') {

                            var objPaymentTerm = JSON.parse(filterTaskArray[i].TaskData);
                            this.lstPaymentTerm.push(objPaymentTerm);
                            if (this.CurrentOrOverdueTask == 'CurrentTasks' && this.formatDate(this.lstPaymentTerm[0].CreatedDate) == this.CurrentDate) {

                                var McuntAct4 = 0;
                                if (SearchV != '') {
                                    var actname = "FUND APPROVAL";
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct4 = 1;
                                    };
                                    if (actname.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct4 = 1;
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct4 = 1;
                                }

                                if (McuntAct4 == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstPaymentTerm[0].RQSTID,
                                        ActivityName: "FUND APPROVAL",
                                        TaskType: filterTaskArray[i].Type,
                                        CreatedDate: this.lstPaymentTerm[0].CreatedDate,
                                        Description: this.lstPaymentTerm[0].Description,
                                        ProjectName: this.lstPaymentTerm[0].ProjectName,
                                        SurveyQstnList: [],
                                        WFLevel: 0,
                                        BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: 'none',
                                        PaymentLinkedBudget: '0',
                                        CreatedBy: filterTaskArray[i].CommittedBy,
                                        RequestedAmount: filterTaskArray[i].CommittedFund,
                                        AdvanceAmount: 0,
                                        TotalBudget: this.lstPaymentTerm[0].TotalBudget,
                                        OtherDonors: this.lstPaymentTerm[0].OtherDonors,
                                        DonorAccount: this.lstPaymentTerm[0].DonorAccount,
                                        OtherDonorsFund: this.lstPaymentTerm[0].OtherDonorFund,
                                        TotalWFLevel: filterTaskArray[i].TotalWFLevel

                                    });
                                }
                            } else if (this.CurrentOrOverdueTask == 'OverdueTasks' && this.formatDate(this.lstPaymentTerm[0].CreatedDate) < this.CurrentDate) {


                                var McuntAct5 = 0;
                                if (SearchV != '') {
                                    var actname = "FUND APPROVAL";
                                    if (filterTaskArray[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct5 = 1;
                                    };
                                    if (actname.toLowerCase().includes(SearchV.toLowerCase())) {
                                        McuntAct5 = 1;
                                    };
                                }
                                else if (SearchV == '') {
                                    McuntAct5 = 1;
                                }
                                if (McuntAct5 == 1) {
                                    this.MyTask.push({
                                        ActivityId: this.lstPaymentTerm[0].RQSTID,
                                        ActivityName: "FUND APPROVAL",
                                        TaskType: filterTaskArray[i].Type,
                                        CreatedDate: this.lstPaymentTerm[0].CreatedDate,
                                        Description: this.lstPaymentTerm[0].Description,
                                        ProjectName: this.lstPaymentTerm[0].ProjectName,
                                        SurveyQstnList: [],
                                        WFLevel: 0,
                                        BeneficiaryList: [],
                                        SyncDBID: filterTaskArray[i].SyncDBID,
                                        TaskTypeID: filterTaskArray[i].TypeID,
                                        TypeIDColumn: filterTaskArray[i].TypeIDColumn,
                                        TaskDate: filterTaskArray[i].TaskDate,
                                        Periodicity: 'none',
                                        PaymentLinkedBudget: '0',
                                        CreatedBy: filterTaskArray[i].CommittedBy,
                                        RequestedAmount: filterTaskArray[i].CommittedFund,
                                        AdvanceAmount: 0,
                                        TotalBudget: this.lstPaymentTerm[0].TotalBudget,
                                        OtherDonors: this.lstPaymentTerm[0].OtherDonors,
                                        DonorAccount: this.lstPaymentTerm[0].DonorAccount,
                                        FundedAmount: this.lstPaymentTerm[0].FundedAmount,
                                        OtherDonorsFund: this.lstPaymentTerm[0].OtherDonorFund,
                                        TotalWFLevel: filterTaskArray[i].TotalWFLevel
                                    });
                                }
                            }

                            this.lstPaymentTerm = [];
                        }
                    }
                }
                else {
                    this.MyTask = [];
                }
            },

            async ApproveTask() {

                
                var lstTaskResponse = [];
                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }

                var today = new Date().toLocaleString();
                //var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID; //.lstTask.filter(x => (x.WFLevel = this.MyTaskType.WFLevel) && (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].SyncDBID;

                var task = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                var TotalWorkFlowLevel = 0;
                var WorkFlowLevel = this.MyTaskType.WFLevel; //
                if (this.MyTaskType.TaskType == 'Payment Request' || this.MyTaskType.TaskType == 'Fund Request') {
                    TotalWorkFlowLevel = 1;
                    WorkFlowLevel = 1;
                } else {
                    var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                    TotalWorkFlowLevel = totWF;
                }
                
                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID,
                    Type: this.MyTaskType.TaskType,
                    TypeID: this.MyTaskType.TaskTypeID,
                    TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: 'PaymentApproved',
                    WorkflowID: this.MyTaskType.WorkflowId,
                    TotalWFLevel: TotalWorkFlowLevel,
                    WFLevel: WorkFlowLevel,
                    WFLevelStatus: "Completed",
                    TaskUpdateComment: this.TaskApproveDescription,
                    AttachedDocumentName: this.ApproveDocName,
                    SurveyAnswers: null,
                    ResponseSyncID: this.MyTaskType.ResponseSyncID, 
                    DataCollectedBy: this.MyTaskType.DataCollectedBy,
                    TaskDate: today,
                    TaskSubmittedDate: today,
                    FinanceWorkflowId: this.MyTaskType.FinanceWorkflowId,
                    CommittedFund: this.MyTaskType.RequestedAmount,
                    RequestId: this.MyTaskType.ActivityId,
                    Command: 'TaskResponse',

                });

                var TaskStatus = "PaymentApproved";
                if (totWF == this.MyTaskType.WFLevel) {
                    TaskStatus = "Completed"
                }
                
                await dbImworks.TaskSyncDB.put({

                    Type: this.MyTaskType.TaskType,
                    Command: 'TaskResponse',
                    TypeID: this.MyTaskType.TaskTypeID,
                    TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: TaskStatus,
                    WorkflowID: this.MyTaskType.WorkflowId,
                    TotalWFLevel: TotalWorkFlowLevel,
                    WFLevel: WorkFlowLevel,
                    ClientData: JSON.stringify(lstTaskResponse),
                    lastSyncDBID: lastSyncDBID,
                    Periodicity: 'none',
                    TaskDate: this.MyTaskType.TaskDate,
                    ReportingValue: 0,
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: WorkFlowLevel, WFLevelStatus: "Completed",
                    TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.MyTaskType.ProjectName, TaskName: this.MyTaskType.ActivityName, Description: this.MyTaskType.Description, CreatedDate: this.MyTaskType.CreateDate
                });

                //;
                // For Documnet Upload
                await dbImworks.TbTaskDocument.put({
                    SyncTaskID: task.SyncDBID,
                    Type: this.TaskType,
                    TypeID: this.TaskTypeID,
                    WFLevel: 0,
                    AttachedDocument: this.TaskApproveAttachment,
                    AttachedDocumentName: this.TaskApproveAttachment.name,
                    DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                    DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0),
                    DocumentType: this.TaskApproveAttachment.type,
                    DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                    DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                    ProjectID: this.MyTaskType.ProjectId,
                    ProjectName: this.MyTaskType.ProjectName,
                    DocLinkedTo: this.MyTaskType.ActivityName
                });

                var CompletedTask = this.MyTask.filter(x => x.ActivityId == this.MyTaskType.ActivityId);
                if (CompletedTask.length > 0) {
                    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                    this.MyTask = [...this.MyTask];
                }

                //this.fnSendNotification();
                this.fnInitializeTaskApprove();
                this.Msg = "Payment Approved Successfully";
                this.successSnackbarMsg = true;
                //this.GetActivityApproverTaskData();
            },

            async RejectTask() {

               
                var lstTaskResponse = [];
                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }

                var today = new Date().toLocaleString();
                //var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID; //.lstTask.filter(x => (x.WFLevel = this.MyTaskType.WFLevel) && (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].SyncDBID;

                var task = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                var TotalWorkFlowLevel = 0;
                var WorkFlowLevel = this.MyTaskType.WFLevel; //
                if (this.MyTaskType.TaskType == 'Payment Request' || this.MyTaskType.TaskType == 'Fund Request') {
                    TotalWorkFlowLevel = 1;
                    WorkFlowLevel = 1;
                } else {
                    var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                    TotalWorkFlowLevel = totWF;
                }
                
                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID,
                    Type: this.MyTaskType.TaskType,
                    TypeID: this.MyTaskType.TaskTypeID,
                    TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: 'PaymentRejected',
                    WorkflowID: this.MyTaskType.WorkflowId,
                    TotalWFLevel: TotalWorkFlowLevel,
                    WFLevel: WorkFlowLevel,
                    WFLevelStatus: "Rejected",
                    TaskUpdateComment: this.TaskApproveDescription,
                    AttachedDocumentName: this.ApproveDocName,
                    SurveyAnswers: null,
                    ResponseSyncID: this.MyTaskType.ResponseSyncID,
                    DataCollectedBy: this.MyTaskType.DataCollectedBy,
                    TaskDate: today,
                    TaskSubmittedDate: today,
                    FinanceWorkflowId: this.MyTaskType.FinanceWorkflowId,
                    CommittedFund: this.MyTaskType.RequestedAmount,
                    RequestId: this.MyTaskType.ActivityId,
                    Command: 'TaskResponse',

                });

                var TaskStatus = "PaymentRejected";
                if (totWF == this.MyTaskType.WFLevel) {
                    TaskStatus = "Rejected"
                }

                await dbImworks.TaskSyncDB.put({

                    Type: this.MyTaskType.TaskType,
                    Command: 'TaskResponse',
                    TypeID: this.MyTaskType.TaskTypeID,
                    TypeIDColumn: this.MyTaskType.TypeIDColumn,
                    Status: TaskStatus,
                    WorkflowID: this.MyTaskType.WorkflowId,
                    TotalWFLevel: TotalWorkFlowLevel,
                    WFLevel: WorkFlowLevel,
                    ClientData: JSON.stringify(lstTaskResponse),
                    lastSyncDBID: lastSyncDBID,
                    Periodicity: 'none',
                    TaskDate: this.MyTaskType.TaskDate,
                    ReportingValue: 0,
                });

                //;
                // For Documnet Upload
                await dbImworks.TbTaskDocument.put({
                    SyncTaskID: task.SyncDBID,
                    Type: this.TaskType,
                    TypeID: this.TaskTypeID,
                    WFLevel: 0,
                    AttachedDocument: this.TaskApproveAttachment,
                    AttachedDocumentName: this.TaskApproveAttachment.name,
                    DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                    DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0),
                    DocumentType: this.TaskApproveAttachment.type,
                    DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                    DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                    ProjectID: this.MyTaskType.ProjectId,
                    ProjectName: this.MyTaskType.ProjectName,
                    DocLinkedTo: this.MyTaskType.ActivityName
                });


                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID, Type: this.MyTaskType.TaskType, TypeID: this.MyTaskType.TaskTypeID, WFLevel: WorkFlowLevel, WFLevelStatus: "Rejected",
                    TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.MyTaskType.ProjectName, TaskName: this.MyTaskType.ActivityName, Description: this.MyTaskType.Description, CreatedDate: this.MyTaskType.CreateDate
                });

                
                var CompletedTask = this.MyTask.filter(x => x.ActivityId == this.MyTaskType.ActivityId);
                if (CompletedTask.length > 0) {
                    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                    this.MyTask = [...this.MyTask];
                }

                //this.fnSendNotification();
                this.fnInitializeTaskApprove();
                this.Msg = "Payment Rejected Successfully";
                this.successSnackbarMsg = true;
                //this.GetActivityApproverTaskData();
            },


            async onFileUpload(addfile, actname) {
                var lstModDate = new Date(addfile.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(addfile.lastModifiedDate).toISOString().substr(11, 5);
                const formData = new FormData()
                formData.append('myFile', addfile);
                formData.append('DocumentOriginalName', addfile.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', this.formatFileSize(addfile.size, 0));
                formData.append('DocumentType', addfile.type);
                formData.append('DocumentTypeIcon', addfile.name.split('.').pop());
                formData.append('DocumentLastModifiedDate', lstModDate);
                formData.append('ProjectId', this.projectInfo.ProjectId);
                formData.append('UploadedFrom', "Payment");
                formData.append('LinkedTo', actname);

                var result = await objManageProjectLedgerBook.UploadDocument(formData);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        return true;
                    else
                        return false;
                }
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            formatDate(date) {
                if (!date) return null

                const [year, month, daytime] = date.split('-')
                const [day, garbage] = daytime.split('T')
                return `${year}-${month}-${day}`
            },
            async fnInitializeTaskApprove() {
                this.IsReviewFinished = false;
                this.IsToReview = false;
                this.ActvityFinanceApproverdlg = false;
                this.PaymentTermFinanceApproverdlg = false;
                this.TaskApproveDescription = "";
                this.TaskApproveAttachment = null;
                this.SurveySectionList = [];
                this.QuestionStepper = 1;
                this.ActivityTabs = 'tab-1';
            },
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
        height: 38vh;
    }

    .historyHeight {
        height: 39vh;
    }
    .taskHeight {
        height: 50vh
    }

    @media screen and (max-width: 750px) {
        .vFileInput {
            width: 50%;
        }
        .dialogHeight {
            height: 85vh;
        }
        .scrollBoxHeight{
            height:43vh
        }

        .historyHeight {
            height: 56vh;
        }
        .taskHeight {
            height: 65vh
        }
    }
</style>