<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0">
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0 mb-0">
                <div  v-for="item in MyTask" :key="item.TaskId" class="pa-0 ma-0">
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
                                        {{item.TaskName}}
                                    </h4>
                                </v-col>
                                <v-col cols="auto" class="py-0 my-0">
                                    <v-chip :class="{'task-activity-chip' :item.TaskType=='Activity',
                                      'task-issue-chip' :item.TaskType=='Issue',
                                      'task-survey-chip' :item.TaskType=='Survey',
                                      'task-risk-chip' :item.TaskType=='RISK',
                                      'task-kpi-chip' :item.TaskType=='KPI',
                                      }"
                                            class="ml-0 mr-2" label dark small>
                                        {{item.TaskType}}
                                    </v-chip>
                                </v-col>
                                <v-col cols="auto" class="d-none d-sm-block py-0 my-0">
                                    <h1 class="font-weight-regular heading-3">
                                        {{new Date(item.CreatedDate).toLocaleDateString('en-GB')}}
                                    </h1>
                                </v-col>
                                <!--<v-col md="auto">
        <v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right; border:none; outline:none" @click.stop="fnButton()">
            <v-icon color="black">
                mdi-dots-vertical
            </v-icon>
        </v-btn>
    </v-col>-->
                            </v-row>
                        </v-card-title>
                        <v-card-text class="d-none d-sm-block py-0 mb-1">
                            <p class="font-weight-regular heading-3 text-truncate py-0 my-0">
                                {{item.TaskDescription}}
                            </p>
                        </v-card-text>
                        <v-card-actions class="px-4 py-0 pb-3">
                            <h3 class="font-weight-regular heading-3-dark">
                                Project Name : {{item.ProjectName}}
                            </h3>
                        </v-card-actions>
                    </v-card>
                </div>
            </v-col>

            <!-- Project Creation Approver dialog-->
            <v-dialog v-model="ProjectApprovaldlg" width="800" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                  <v-row class="mx-4 pt-6" no-gutters="">
                      <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                          <v-btn class="d-sm-none" text small @click="fnCancelTaskAction()">
                              <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                          </v-btn>
                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                              <v-icon size="15" color="primary">mdi-checkbox-marked-outline</v-icon>
                          </v-avatar>
                          Project creation approval
                      </v-col>
                    <v-col cols="4" class="text-right text-md-left">
                      <v-chip color="#2C4258" class="ml-2" label="" dark="" small="" outlined="">
                        General
                      </v-chip>
                    </v-col>
                    <v-spacer></v-spacer>
                    <v-col cols="2" align="right" class="d-none d-sm-inline">
                      <v-btn text small @click="fnCancelTaskAction()">
                        <v-icon color="#707070" size="20">mdi-close</v-icon>
                      </v-btn>
                    </v-col>
                  </v-row>
                  <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                  <v-card-text v-show="IsToReview == false && IsReviewFinished == false" class="pa-3">
                      <v-tabs color="#52B962" grow v-model="ActivityTabs" class="pt-0 taskHeight">
                          <v-tab href='#tab-1'>Task Details</v-tab>
                          <v-tab href='#tab-2'>History</v-tab>

                          <!-- Task Details-->
                          <v-tab-item value="tab-1">
                              <v-row justify="center" class="mt-4">
                                  <v-col md="12" class="v-scrolling-div px-6 historyHeight">
                                      <v-row>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Task Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.TaskName}}</h4>
                                          </v-col>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3"> {{new Date(MyTaskType.CreateDate).toLocaleDateString('en-GB')}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col md="6">
                                              <v-row>
                                                  <v-col>
                                                      <h4 class="font-weight-regular heading-3-dark mt-0">
                                                          Project Name
                                                      </h4>
                                                      <h4 class="font-weight-regular heading-3">{{MyTaskType.TaskName}}</h4>
                                                  </v-col>
                                                  <v-col>
                                                      <v-btn color="#52B962" outlined @click.native="fnRedirectTask()">View Project</v-btn>
                                                  </v-col>
                                              </v-row>
                                          </v-col>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-2">
                                                  Budget
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.Budget}}</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Project Creator
                                              </h4>
                                              <v-avatar size="26" class="team-avatar" light v-bind:color="RandomColor">
                                                  <!--<img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />-->
                                                  {{MyTaskType.CreatedOwner == null ? "" : MyTaskType.CreatedOwner.substring(0, 1).toUpperCase()}}
                                              </v-avatar>{{MyTaskType.CreatedOwner}}
                                          </v-col>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Other Users
                                              </h4>
                                              <span v-for="item in MyTaskType.lstInitiativeUser">
                                                  <v-avatar size="26" v-bin class="team-avatar" v-bind:color="RandomColor">
                                                      {{item == null ? "" : item.substring(0, 1).toUpperCase()}}
                                                  </v-avatar>
                                              </span>
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
                                                                  <h4 class="font-weight-regular py-0 pb-0 px-0" style="height:auto;max-height:100px;overflow-y:auto">{{item.TaskUpdateComment}}</h4>
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
                      <v-row class="py-0 my-2">
                          <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                              <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true,QuestionStepper=1"></Secondary-Button>
                              <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="fnCancelTaskAction()"></Default-Button-Outlined>
                          </v-col>
                      </v-row>
                  </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true" class="pa-3">
                        <v-card elevation="0" class="pa-0 ma-0">
                            <div class="taskHeight">
                                <v-card-title>
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0 ">
                                            <h4 class="text-left page-head">Do you want to approve the project creation?</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-text class="v-scrolling-div scrollBoxHeight">
                                    <v-row>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-0">
                                                Project Name
                                            </h4>
                                            <h4 class="mt -3 font-weight-regular heading-3">{{MyTaskType.TaskName}}</h4>
                                        </v-col>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-0">
                                                Project Creator
                                            </h4>
                                            <v-avatar size="26" class="team-avatar mt -3" light v-bind:color="RandomColor">
                                                <!--<img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />-->
                                                {{MyTaskType.CreatedOwner == null ? "" : MyTaskType.CreatedOwner.substring(0, 1).toUpperCase()}}
                                            </v-avatar>{{MyTaskType.CreatedOwner}}
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col>
                                            <h4 class="text-left heading-3 mb-2">Add comments</h4>
                                            <v-textarea class="mb-2" rows="5" hide-details outlined dense label="Enter Comments" type="text" v-model="TaskApproveComments">
                                            </v-textarea>
                                            <v-file-input class="vFileInput" small-chips truncate-length="10" hide-details label="Upload Attachment" prepend-inner-icon="mdi-paperclip" v-model="TaskApproveAttachment" prepend-icon="" outlined="" dense=""></v-file-input>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </div>
                            <v-card-actions class="py-0 my-2">
                                <v-col md="12" class="text-center text-md-right py-0 my-0">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="ApproveTask();"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Reject" @click.native="RejectTask()"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                </v-card>
            </v-dialog>


            <!-- Issue Raised-->
            <v-dialog v-model="IssueRaiseldlg" width="650" transition="dialog-bottom-transition" persistent>
                <v-card class="elevation-0 dialogHeight">
                    <v-row class="mx-4 pt-6" no-gutters>
                        <v-col cols="auto" class="limited-width-CardTitle text-truncate page-head">
                            <v-btn class="d-sm-none" text small @click="fnCancelTaskAction()">
                                <v-icon color="#707070" size="30">mdi-keyboard-backspace</v-icon>
                            </v-btn>
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 d-none d-sm-inline">
                                <v-icon size="15" color="primary">mdi-alert-outline</v-icon>
                            </v-avatar>
                            Issue Raised
                        </v-col>
                           <v-col cols="4" class="text-right text-md-left">
                             <v-chip color="#E62E00" class="ml-2" label="" dark="" small="" outlined="">
                               Issue
                             </v-chip>
                           </v-col>
                           <v-spacer></v-spacer>
                           <v-col cols="2" align="right" class="d-none d-sm-inline">
                             <v-btn text small @click="fnCancelTaskAction()">
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
                                          <v-col md="6">
                                              <v-row>
                                                  <v-col>
                                                      <h4 class="font-weight-regular heading-3-dark mt-0">
                                                          Task Name ({{this.MyTaskType.TypeofTask}})
                                                      </h4>
                                                      <h4 class="font-weight-regular heading-3">{{MyTaskType.TaskName}} </h4>
                                                  </v-col>
                                                  <v-col>
                                                      <v-btn color="#52B962" outlined @click.native="fnRedirectTask()">View Task</v-btn>
                                                  </v-col>
                                              </v-row>
                                          </v-col>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3"> {{new Date(MyTaskType.CreateDate).toLocaleDateString('en-GB')}} </h4>
                                          </v-col>
                                      </v-row>
                                      <!-- Task Description-->
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                          Task Description
                                      </h4>
                                      <h4 class="font-weight-regular heading-3" style="height:auto;max-height:100px;overflow-y:auto">
                                          {{MyTaskType.TaskDescription}}
                                      </h4>
                                      <v-row>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-0">
                                                  Project Name
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.ProjectName}}</h4>
                                          </v-col>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-2">
                                                  Target
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">0</h4>
                                          </v-col>
                                      </v-row>
                                      <v-row>
                                          <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                  Priodicity
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{MyTaskType.Periodicity}}</h4>
                                          </v-col>
                                          <v-col md="6">
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
                      <v-row class="py-0 my-2">
                          <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                              <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true,QuestionStepper=1"></Secondary-Button>
                              <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="fnCancelTaskAction()"></Default-Button-Outlined>
                          </v-col>
                      </v-row>
                  </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true" class="pa-3" >
                        <v-card elevation="0" class="pa-0 ma-0">
                            <div class="taskHeight">
                                <v-card-title>
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0 ">
                                            <h4 class="text-left page-head">Do you want to resolve the issue</h4>
                                        </v-col>
                                    </v-row>
                                </v-card-title>
                                <v-card-text>
                                    <v-row>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-0">
                                                Activity Name
                                            </h4>
                                            <h4 class="mt -3 font-weight-regular heading-3">{{MyTaskType.TaskName}}</h4>
                                        </v-col>
                                        <v-col md="6">
                                            <h4 class="font-weight-regular heading-3-dark mt-0">
                                                Project Creator
                                            </h4>
                                            <!--<v-avatar size="26" class="team-avatar mt -3">
                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />
                </v-avatar>Olivia Parat-->

                                            <v-avatar size="26" class="team-avatar mt -3" light v-bind:color="RandomColor">
                                                <!--<img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />-->
                                                {{MyTaskType.CreatedOwner == null ? "" : MyTaskType.CreatedOwner.substring(0, 1).toUpperCase()}}
                                            </v-avatar>{{MyTaskType.CreatedOwner}}
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col>
                                            <h4 class="text-left heading-3 mb-2">Add comments</h4>
                                            <v-textarea class="mb-2" rows="5" hide-details outlined dense label="Enter Comments" type="text" v-model="TaskIssueApproveComments">
                                            </v-textarea>
                                            <v-file-input hide-details label="Upload Attachment" prepend-inner-icon="mdi-paperclip" v-model="TaskIssueApproveAttachment" prepend-icon="" outlined="" dense=""></v-file-input>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </div>
                            <v-card-actions class="py-0 my-2">
                                <v-col md="12" class="text-center text-md-right py-0 my-0">
                                    <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native="ApproveIssueTask()"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Reject" @click.native="RejectIssueTask()"></Default-Button-Outlined>
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
                //---------------- Activity Apporover -----------//

                ActvityFinanceApproverdlg: false,
                IsToReview: false,
                IsReviewFinished: false,
                
                TaskApproveComments: '',
                TaskApproveAttachment: null,
                TaskIssueApproveComments: '',
                TaskIssueApproveAttachment: null,
                ActivityTabs: 'tab-1',
                MyTask: [],
                lstTask: [],
                lstProject: [],

                // Log Frame
                lstLogFrame: [],
                AuditLog: [],
                userimage: "",

                // Project approver
                ProjectApprovaldlg: false,
                IssueRaiseldlg: false,
                CurrentOrOverdueTask: '',
                MyTaskType: {
                    TaskId: 0,
                    TaskName: '',
                    TaskDescription: '',
                    CreateDate: '',
                    TaskType: '',
                    TaskDate: '',
                    SyncDBID: 0,
                    WorkflowId: '',
                    WFLevel: '',
                    TaskTypeID: '',
                    CreatedOwner: '',
                    OtherUser: '',
                    lstInitiativeUser: [],
                    ProjectName: '',
                    Periodicity: '',
                    TypeIDColumnName: '',
                    TypeofTask: '',
                    PageUrl: '',
                    WorkSpaceId: '',
                    InitiativeId: '',
                    ProjectID: ''
                },
                ApproveDocName: "",
                RandomColor: "",

                //snackbar
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
            }
        },
        props: ['searchcall', 'searchtask'],
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.CurrentOrOverdueTask = this.$route.name;
            await this.GetProjectTaskData('');
        },       
        watch: {
            searchcall: function () {
                this.GetProjectTaskData(this.searchcall);
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

            fnUpdateTask(item) {
                this.MyTaskType.TaskTypeID = item.ActivityId;
                this.MyTaskType.TaskId = item.ActivityId;
                this.MyTaskType.TaskName = item.TaskName;
                this.MyTaskType.TaskDescription = item.TaskDescription;
                this.MyTaskType.CreateDate = item.CreatedDate;
                this.MyTaskType.TaskType = item.TaskType;
                this.MyTaskType.TaskDate = item.CreatedDate;
                this.MyTaskType.Budget = item.Budget;
                this.MyTaskType.SyncDBID = item.SyncDBID;
                this.MyTaskType.CreatedOwner = item.CreatedBy;
                this.MyTaskType.WFLevel = (item.WFLevel == 0 ? 1 : item.WFLevel);
                this.MyTaskType.OtherUser = item.OtherUser;

                if (item.OtherUser != null && item.OtherUser != "") {
                    this.MyTaskType.lstInitiativeUser = item.OtherUser.split(',');
                }
                this.MyTaskType.ProjectName = item.ProjectName;
                this.MyTaskType.Periodicity = item.Periodicity;
                this.MyTaskType.TypeIDColumnName = item.TypeIDColumnName;
                this.MyTaskType.WorkSpaceId = item.WorkSpaceId;
                this.MyTaskType.InitiativeId = item.InitiativeId;
                this.MyTaskType.ProjectID = item.ProjectID;

                var url = window.SERVER_URL;
                switch (this.MyTaskType.TypeIDColumnName) {
                    case "ActivityId":
                        this.MyTaskType.TypeofTask = "Activity";
                        this.MyTaskType.PageUrl = url + "/app/ManageProjectActivitiesIndividual?WsID=" + this.MyTaskType.WorkSpaceId + "&InitID=" + this.MyTaskType.InitiativeId + "&ProjID=" + this.MyTaskType.ProjectID + "&ActID=" + this.MyTaskType.TaskId;
                        break;
                    case "KPIID":
                        this.MyTaskType.TypeofTask = "KPI";
                        this.MyTaskType.PageUrl = url + "/app/ManageProjectKPIActivityLog?WsID=" + this.MyTaskType.WorkSpaceId + "&KPIID=" + this.MyTaskType.TaskId + "&ProjID=" + this.MyTaskType.ProjectID + "&InitID=" + this.MyTaskType.InitiativeId + "&Type=Edit";
                        break;
                    case "RISKID":
                        this.MyTaskType.TypeofTask = "Risk";
                        this.MyTaskType.PageUrl = url + "/app/ManageProjectRiskActivityLog?WsID=" + this.MyTaskType.WorkSpaceId + "&riskId=" + this.MyTaskType.TaskId + "&ProjID=" + this.MyTaskType.ProjectID + "&InitID=" + this.MyTaskType.InitiativeId + "&Type=Edit";
                        break;
                    case "SurveyID":
                        this.MyTaskType.TypeofTask = "Survey";
                        this.MyTaskType.PageUrl = url + "/app/ManageProjectSurveys?WsID=" + this.MyTaskType.WorkSpaceId + "&ProjID=" + this.MyTaskType.ProjectID + "&InitID=" + this.MyTaskType.InitiativeId;
                        break;
                }

                if (item.TaskType == "Issue") {
                    this.IssueRaiseldlg = true;
                    this.lstLogFrame = this.AuditLog.filter(x => (x.TypeID == this.MyTaskType.TaskTypeID) && (x.Type == "Issue"));
                } else {
                    this.ProjectApprovaldlg = true;
                    this.lstLogFrame = this.AuditLog.filter(x => (x.TypeID == this.MyTaskType.TaskTypeID) && (x.Type == "Project"));
                    this.MyTaskType.PageUrl = url + "/app/MyprojectsDraft?WsID=" + this.MyTaskType.WorkSpaceId;
                }
            },
            fnButton() {
                alert("Button Clicked");
            },
            async GetProjectTaskData(SearchV) {
                this.lstTask = [];
                this.MyTask = [];
                this.AuditLog = await dbImworks.TbTaskAuditLog.toArray();
                this.lstTask = await dbImworks.MyTaskList.toArray();

                for (var i = 0; i < this.lstTask.length; i++) {

                    if (this.lstTask[i].Type == 'Project' && this.lstTask[i].WFLevelStatus == 'Pending') {
                        var objActivity = JSON.parse(this.lstTask[i].TaskData);
                        this.lstProject.push(objActivity);

                        var McuntAct = 0;
                        if (SearchV != '') {
                            if (this.lstTask[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                McuntAct = 1;
                            };                           
                        }
                        else if (SearchV == '') {
                            McuntAct = 1;
                        }
                        if (McuntAct == 1) {
                            this.MyTask.push({
                                ActivityId: this.lstProject[0].ProjectId, TaskName: this.lstProject[0].ProjectName, CreatedDate: this.lstProject[0].CreatedDate,
                                TaskDescription: this.lstProject[0].Description, ProjectName: this.lstProject[0].ProjectName, StartDate: this.lstProject[0].StartDate,
                                EndDate: this.lstProject[0].EndDate, TaskType: "Project", Budget: this.lstProject[0].Budget, SyncDBID: this.lstTask[i].SyncDBID,
                                WFLevel: this.lstTask[i].WFLevel, CreatedBy: this.lstProject[0].CreatedBy, OtherUser: this.lstProject[0].OtherUser, Periodicity: '',
                                TypeIDColumnName: '', WorkSpaceId: this.lstProject[0].WorkSpaceId, InitiativeId: 0, ProjectID: 0, TypeColor: '#FF00FF',
                            });
                        }
                        this.lstProject = [];
                    }
                    else if (this.lstTask[i].Type == 'Issue' && this.lstTask[i].WFLevelStatus == 'Pending') {
                        var objActivity = JSON.parse(this.lstTask[i].TaskData);
                        this.lstProject.push(objActivity);

                        var McuntAct1 = 0;
                        if (SearchV != '') {
                            if (this.lstTask[i].Type.toLowerCase().includes(SearchV.toLowerCase())) {
                                McuntAct1 = 1;
                            };                            
                        }
                        else if (SearchV == '') {
                            McuntAct1 = 1;
                        }

                        if (McuntAct1 == 1) {
                            this.MyTask.push({
                                ActivityId: this.lstProject[0].TaskId, TaskName: this.lstProject[0].TaskName, CreatedDate: this.lstProject[0].CreatedDate,
                                TaskDescription: this.lstProject[0].TaskDescription, ProjectName: this.lstProject[0].ProjectName, StartDate: '',
                                EndDate: '', TaskType: "Issue", Budget: '0', SyncDBID: this.lstTask[i].SyncDBID,
                                WFLevel: 0, CreatedBy: this.lstProject[0].CreatedBy, OtherUser: '', Periodicity: this.lstProject[0].Periodicity,
                                TypeIDColumnName: this.lstProject[0].TypeIDColumnName, WorkSpaceId: this.lstProject[0].WorkSpaceId,
                                InitiativeId: this.lstProject[0].InitiativeId, ProjectID: this.lstProject[0].ProjID, TypeColor: '#BCC6CC',
                            });
                        }
                        this.lstProject = [];
                    }
                }
            },
            async ApproveTask() {

                var lstTaskResponse = [];

                //if (this.TaskApproveComments == null || this.TaskApproveComments != "") {
                //    return false;
                //}

                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }
               
                var today = new Date().toLocaleString();

                var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID;
                var task = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: "Project", TypeID: this.MyTaskType.TaskId, TypeIDColumn: "ProjectId", Status: 'DataApproved',
                    WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Completed", TaskUpdateComment: this.TaskApproveComments,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: null, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, ReportingValue: 0
                });

                var TaskStatus = "DataApproved";
                if (totWF == this.MyTaskType.WFLevel) {
                    TaskStatus = "Completed"
                }

                await dbImworks.TaskSyncDB.put({
                    Type: "Project", Command: 'TaskResponse', TypeID: this.MyTaskType.TaskId, TypeIDColumn: "ProjectId",
                    Status: TaskStatus, WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse), lastSyncDBID: 0,
                    Periodicity: 0, TaskDate: this.MyTaskType.TaskDate, ReportingValue: 0
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();

                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID,
                    Type: "Project",
                    TypeID: this.MyTaskType.TaskId,
                    WFLevel: this.MyTaskType.WFLevel,
                    WFLevelStatus: "Completed",
                    TaskUpdateComment: this.TaskApproveComments,
                    AttachedDocumentName: this.ApproveDocName,
                    SyncDBID: lastSyncDBID,
                    ProjectName: this.MyTaskType.TaskName,
                    TaskName: this.MyTaskType.TaskName,
                    Description: this.MyTaskType.TaskDescription,
                    CreatedDate: this.MyTaskType.CreateDate,
                    PeriodicityId: 0,
                    PeriodicityDate: this.MyTaskType.TaskDate
                });


                //var CompletedTask = this.MyTask.filter(x => (x.ActivityId == this.MyTaskType.TaskId) && (x.SyncDBID == this.MyTaskType.SyncDBID));
                //if (CompletedTask.length > 0) {
                //    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                //    this.MyTask = [...this.MyTask];
                //}

                //remove submitted items from the array list
                var PendingTask = this.MyTask.filter(x => (x.SyncDBID != this.MyTaskType.SyncDBID));
                this.MyTask = PendingTask;
                this.MyTask = [...this.MyTask];

                //update  WFLevelStatus for updated item
                await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Completed' });

                this.ProjectApprovaldlg = false;
                this.IsToReview = false;
                this.Msg = "Task Approved Successfully";
                this.successSnackbarMsg = true;
            },

            async ApproveIssueTask() {

                var lstTaskResponse = [];

                //if (this.TaskApproveComments == null || this.TaskApproveComments != "") {
                //    return false;
                //}

                if (this.TaskIssueApproveAttachment) {
                    this.ApproveDocName = this.TaskIssueApproveAttachment.name;
                }
               
                var today = new Date().toLocaleString();

                var totWF = 1;
                var lastSyncDBID = this.MyTaskType.SyncDBID;

                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: "Issue", TypeID: this.MyTaskType.TaskId, TypeIDColumn: this.MyTaskType.TypeIDColumnName, Status: 'DataApproved',
                    WorkflowID: 0, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Completed", TaskUpdateComment: this.TaskIssueApproveComments,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: null, ResponseSyncID: 0, DataCollectedBy: 0,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, ReportingValue: 0
                });

                var TaskStatus = "DataApproved";

                await dbImworks.TaskSyncDB.put({
                    Type: "Issue", Command: 'TaskResponse', TypeID: this.MyTaskType.TaskId, TypeIDColumn: this.MyTaskType.TypeIDColumnName,
                    Status: TaskStatus, WorkflowID: 0, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse), lastSyncDBID: 0,
                    Periodicity: 0, TaskDate: this.MyTaskType.TaskDate, ReportingValue: 0
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();

                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID,
                    Type: "Issue",
                    TypeID: this.MyTaskType.TaskId,
                    WFLevel: this.MyTaskType.WFLevel,
                    WFLevelStatus: "Completed",
                    TaskUpdateComment: this.TaskIssueApproveComments,
                    AttachedDocumentName: this.ApproveDocName,
                    SyncDBID: lastSyncDBID,
                    ProjectName: this.MyTaskType.ProjectName,
                    TaskName: this.MyTaskType.TaskName,
                    Description: this.MyTaskType.TaskDescription,
                    CreatedDate: this.MyTaskType.CreateDate,
                    PeriodicityId: 0,
                    PeriodicityDate: this.MyTaskType.TaskDate
                });

                //var CompletedTask = this.MyTask.filter(x => (x.ActivityId == this.MyTaskType.TaskId) && (x.SyncDBID == this.MyTaskType.SyncDBID));
                //if (CompletedTask.length > 0) {
                //    this.MyTask.splice(this.MyTask.indexOf(CompletedTask), 1);
                //    this.MyTask = [...this.MyTask];
                //}
                //remove submitted items from the array list
                var PendingTask = this.MyTask.filter(x => (x.SyncDBID != this.MyTaskType.SyncDBID));
                this.MyTask = PendingTask;
                this.MyTask = [...this.MyTask];

                //update  WFLevelStatus for updated item
                await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Completed' });

                this.ProjectApprovaldlg = false;
                this.IsToReview = false;
                this.IssueRaiseldlg = false;
                this.Msg = "Task Approved Successfully";
                this.successSnackbarMsg = true;
            },
            async fnRedirectTask() {
                window.open(this.MyTaskType.PageUrl, "_blank");
            },
            async fnCancelTaskAction() {
                this.ProjectApprovaldlg = false;
                this.IsToReview = false;
                this.ProjectApprovaldlg = false;
                this.IssueRaiseldlg = false;

                this.MyTaskType.TaskTypeID = '';
                this.MyTaskType.TaskId = 0;
                this.MyTaskType.TaskName = '';
                this.MyTaskType.TaskDescription = '';
                this.MyTaskType.CreateDate = '';
                this.MyTaskType.TaskType = '';
                this.MyTaskType.TaskDate = '';
                this.MyTaskType.Budget = '';
                this.MyTaskType.SyncDBID = 0;
                this.MyTaskType.CreatedOwner = '';
                this.MyTaskType.WFLevel = '';
                this.MyTaskType.OtherUser = '';
                this.MyTaskType.lstInitiativeUser = [];
                this.MyTaskType.ProjectName = '';
                this.MyTaskType.Periodicity = '';
                this.MyTaskType.TypeIDColumnName = '';
                this.MyTaskType.WorkSpaceId = '';
                this.MyTaskType.InitiativeId = '';
                this.MyTaskType.ProjectID = '';
                this.TaskApproveComments = '',
                this.TaskApproveAttachment = null;
                this.TaskIssueApproveComments = '';
                this.TaskIssueApproveAttachment = null;
            },
            async RejectIssueTask() {
                var lstTaskResponse = [];

                //if (this.TaskApproveComments == null || this.TaskApproveComments != "") {
                //    return false;
                //}

                if (this.TaskIssueApproveAttachment) {
                    this.ApproveDocName = this.TaskIssueApproveAttachment.name;
                }

                var today = new Date().toLocaleString();

                var totWF = 1;
                var lastSyncDBID = this.MyTaskType.SyncDBID;

                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: "Issue", TypeID: this.MyTaskType.TaskId, TypeIDColumn: this.MyTaskType.TypeIDColumnName, Status: 'DataRejected',
                    WorkflowID: 0, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Rejected", TaskUpdateComment: this.TaskIssueApproveComments,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: null, ResponseSyncID: 0, DataCollectedBy:0,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, ReportingValue: 0
                });

                var TaskStatus = "Rejected";

                await dbImworks.TaskSyncDB.put({
                    Type: "Issue", Command: 'TaskResponse', TypeID: this.MyTaskType.TaskId, TypeIDColumn: this.MyTaskType.TypeIDColumnName,
                    Status: TaskStatus, WorkflowID: 0, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse), lastSyncDBID: 0,
                    Periodicity: 0, TaskDate: this.MyTaskType.TaskDate, ReportingValue: 0
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();

                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID,
                    Type: "Issue",
                    TypeID: this.MyTaskType.TaskId,
                    WFLevel: this.MyTaskType.WFLevel,
                    WFLevelStatus: "Rejected",
                    TaskUpdateComment: this.TaskIssueApproveComments,
                    AttachedDocumentName: this.ApproveDocName,
                    SyncDBID: lastSyncDBID,
                    ProjectName: this.MyTaskType.ProjectName,
                    TaskName: this.MyTaskType.TaskName,
                    Description: this.MyTaskType.TaskDescription,
                    CreatedDate: this.MyTaskType.CreateDate,
                    PeriodicityId: 0,
                    PeriodicityDate: this.MyTaskType.TaskDate
                });

                //remove submitted items from the array list
                var PendingTask = this.MyTask.filter(x => (x.SyncDBID != this.MyTaskType.SyncDBID));
                this.MyTask = PendingTask;
                this.MyTask = [...this.MyTask];

                //update  WFLevelStatus for updated item
                await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Completed' });

                this.ProjectApprovaldlg = false;
                this.IsToReview = false;
                this.IssueRaiseldlg = false;
                this.Msg = "Task Rejected Successfully";
                this.successSnackbarMsg = true;
            },
            async RejectTask() {
              
                var lstTaskResponse = [];

                //if (this.TaskApproveComments == null || this.TaskApproveComments != "") {
                //    return false;
                //}

                if (this.TaskApproveAttachment) {
                    this.ApproveDocName = this.TaskApproveAttachment.name;
                }

                var today = new Date().toLocaleString();

                var totWF = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID))[0].TotalWFLevel;
                var lastSyncDBID = this.MyTaskType.SyncDBID;
                var task = this.lstTask.filter(x => (x.Type == this.MyTaskType.TaskType) && (x.TypeID == this.MyTaskType.TaskTypeID) && x.SyncDBID == this.MyTaskType.SyncDBID)[0];

                lstTaskResponse.push({
                    SyncDBID: lastSyncDBID, Type: "Project", TypeID: this.MyTaskType.TaskId, TypeIDColumn: "ProjectId", Status: 'DataRejected',
                    WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, WFLevelStatus: "Rejected", TaskUpdateComment: this.TaskApproveComments,
                    AttachedDocumentName: this.ApproveDocName, SurveyAnswers: null, ResponseSyncID: task.ResponseSyncID, DataCollectedBy: task.DataCollectedBy,
                    TaskDate: this.MyTaskType.TaskDate, TaskSubmittedDate: today, ReportingValue: 0
                });

                var TaskStatus = "Rejected";
               
                await dbImworks.TaskSyncDB.put({
                    Type: "Project", Command: 'TaskResponse', TypeID: this.MyTaskType.TaskId, TypeIDColumn: "ProjectId",
                    Status: TaskStatus, WorkflowID: task.WorkflowID, TotalWFLevel: totWF, WFLevel: this.MyTaskType.WFLevel, ClientData: JSON.stringify(lstTaskResponse), lastSyncDBID: 0,
                    Periodicity: 0, TaskDate: this.MyTaskType.TaskDate, ReportingValue: 0
                });

                var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();

                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastSyncID,
                    Type: "Project",
                    TypeID: this.MyTaskType.TaskId,
                    WFLevel: this.MyTaskType.WFLevel,
                    WFLevelStatus: "Rejected",
                    TaskUpdateComment: this.TaskApproveComments,
                    AttachedDocumentName: this.ApproveDocName,
                    SyncDBID: lastSyncDBID,
                    ProjectName: this.MyTaskType.TaskName,
                    TaskName: this.MyTaskType.TaskName,
                    Description: this.MyTaskType.TaskDescription,
                    CreatedDate: this.MyTaskType.CreateDate,
                    PeriodicityId: 0,
                    PeriodicityDate: this.MyTaskType.TaskDate
                });

                //remove submitted items from the array list
                var PendingTask = this.MyTask.filter(x => (x.SyncDBID != this.MyTaskType.SyncDBID));
                this.MyTask = PendingTask;
                this.MyTask = [...this.MyTask];

                //update  WFLevelStatus for updated item
                await dbImworks.MyTaskList.where("SyncDBID").equals(this.MyTaskType.SyncDBID).modify({ WFLevelStatus: 'Completed' });

                this.ProjectApprovaldlg = false;
                this.IsToReview = false;
                this.Msg = "Task Rejected Successfully";
                this.successSnackbarMsg = true;
            },
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

    .historyHeight {
        height: 52vh;
    }

    .taskHeight {
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
            height: 43vh
        }

        .historyHeight {
            height: 56vh;
        }

        .taskHeight {
            height: 65vh
        }
    }
</style>