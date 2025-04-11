<template>
    <v-card class="elevation-0">
        <v-card-title style="height:50px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                </v-avatar>
                <!--if path is ManageProjectActivitiesIndividual-->
                <router-link v-show="$route.name=='ManageProjectActivitiesIndividual'" :to="{ name: 'ManageProjectActivities', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId} }" class="page-head text-link">Output</router-link>
                <!--________________-->
                <!--if path is MyprojectsDraftIndividualActivitiesIndividual-->
                <router-link v-show="$route.name=='MyprojectsDraftIndividualActivitiesIndividual'" :to="{ name: 'MyprojectsDraftIndividualActivities', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId} }" class="page-head text-link">Output</router-link>
                <!--________________-->
                <!--if path is MyprojectsDeployedIndividualActivitiesIndividual-->
                <router-link v-show="$route.name=='MyprojectsDeployedIndividualActivitiesIndividual'" :to="{ name: 'MyprojectsDeployedIndividualActivities', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId} }" class="page-head text-link">Output</router-link>
                <!--________________-->
                > {{ activityInfo.ActivityName }}
            </h5>
        </v-card-title>

        <v-card-text>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-tabs centered v-model="ActivityTab" class="pt-0 elevation-0" color="tab-active-line">
                    <v-tab href='#tab-1'>Details</v-tab>
                    <v-tab id="AutoTest_IP173" href='#tab-2' @click="fnGetActivityLogList()">Output Log</v-tab>
                    <v-tab id="AutoTest_IP174" href='#tab-3' @click="fnMountPivotTab()">Pivot Table</v-tab>
                    <!--Details-->
                    <v-tab-item value="tab-1">
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="10">
                                <!--- View Activity Details  -->
                                <v-row :justify="justifyCenter" v-if="editActivityDetails == false">
                                    <v-col md="10" class="v-scrolling-div mt-0" style="height:55vh">
                                        <v-row>
                                            <v-col md="8">
                                                <small>Project Progress</small>
                                                <v-progress-linear dark rounded
                                                                   background-color="#0A8C45"
                                                                   color="#52B962"
                                                                   height="15"
                                                                   :value=activityInfo.Progress>
                                                    {{activityInfo.Progress}}%
                                                </v-progress-linear>
                                            </v-col>
                                            <v-spacer></v-spacer>
                                            <v-col md="4">
                                                <Secondary-Button id="AutoTest_IP174" title="Edit Details" class="float-right" @click.native="editActivityDetails = true,fnEditActivityClick()" v-bind:disabled="!screenAccess"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                        <v-row :justify="justifyCenter">
                                            <v-col md="12" class="pb-0">
                                                <h4 class="font-weight-medium heading-3-dark">
                                                    Basic Details
                                                </h4>
                                                <!-- Activity  Name -->
                                                <h4 class="font-weight-regular heading-3-dark mt-4">Output Name</h4>
                                                <h4 class="font-weight-regular heading-3">{{ activityInfo.ActivityName}}</h4>

                                                <!-- Project Description -->
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Output Description
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">
                                                    {{ activityInfo.Description}}
                                                </h4>

                                                <v-row>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-4">Start Date</h4>
                                                        <h4 class="font-weight-regular heading-3">{{ formatDate(activityInfo.StartDate) }}</h4>
                                                    </v-col>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-4">End Date</h4>
                                                        <h4 class="font-weight-regular heading-3">{{ formatDate(activityInfo.EndDate) }}</h4>
                                                    </v-col>
                                                </v-row>
                                                <h4 class="font-weight-regular heading-3-dark mt-4">Weigthage</h4>
                                                <h4 class="font-weight-regular heading-3">{{ activityInfo.WeightagePercentage }}%</h4>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                                <v-row :justify="justifyCenter" v-if="editActivityDetails == true">
                                    <v-col md="10" class="text-right">
                                        <Success-Button id="AutoTest_IP199" class="btn-122x36 mr-4" title="Save" @click.native="editActivityDetails =false,fnAddActivity()"></Success-Button>
                                        <Default-Button-Outlined id="AutoTest_IP200" class="btn-122x36" title="Cancel" @click.native="editActivityDetails =false"></Default-Button-Outlined>
                                    </v-col>
                                </v-row>

                                <!-- Edit Ledger Book Details-->
                                <v-row :justify="justifyCenter" v-if="editActivityDetails == true">
                                    <v-col md="10">
                                        <v-text-field outlined dense label="Output Name" class="mb-3" v-model="editActivity.ActivityName">
                                        </v-text-field>
                                        <v-textarea outlined dense name="" label="Output Description" class="mb-3" v-model="editActivity.Description"></v-textarea>
                                        <v-row class="py-0">
                                            <!-- From Date -->
                                            <v-col>
                                                <v-menu v-model="FrmDateMenu"
                                                        :close-on-content-click="false"
                                                        :nudge-right="40"
                                                        transition="scale-transition"
                                                        offset-y
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model="computedEditStartDate"
                                                                      label="Start Date"
                                                                      outlined dense
                                                                      readonly
                                                                      prepend-inner-icon="mdi-calendar"
                                                                      v-on="on"></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="editActivity.StartDate" color="primary" no-title @input="FrmDateMenu = false" format="DD-MM-YYYY"
                                                                   :min="ProjectStartDate" :max="ProjectEndDate" dense></v-date-picker>
                                                </v-menu>
                                            </v-col>
                                            <!-- To Date -->
                                            <v-col>
                                                <!--<v-menu v-model=""
                                                        :close-on-content-click="false"
                                                        transition="scale-transition"
                                                        offset-y
                                                        max-width="290px"
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model=""
                                                                      label="Start Date"
                                                                      persistent-hint outlined hide-details
                                                                      v-on="on"></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="ToDate" no-title @input="ToDateMenu=false,editActivity.EndDate=formatDate(ToDate)" dense :min="FrmDate"></v-date-picker>
                                                </v-menu>-->
                                                <v-menu v-model="ToDateMenu"
                                                        :close-on-content-click="false"
                                                        :nudge-right="40"
                                                        transition="scale-transition"
                                                        offset-y
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model="computedEditEndDate"
                                                                      label="Start Date"
                                                                      outlined dense
                                                                      readonly
                                                                      prepend-inner-icon="mdi-calendar"
                                                                      v-on="on"></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="editActivity.EndDate" color="primary" no-title @input="ToDateMenu = false" format="DD-MM-YYYY"
                                                                   :min="editActivity.StartDate" :max="ProjectEndDate" dense></v-date-picker>
                                                </v-menu>

                                            </v-col>
                                        </v-row>
                                        <v-text-field outlined="" dense="" label="Weightage (100 %)" hide-details class="mb-3" v-model="editActivity.WeightagePercentage" type="number"
                                                      onkeydown="javascript: return event.keyCode == 69 ? false : true" onKeyPress="if(this.value.length==2 ) return false;" min="0" max="100">
                                        </v-text-field>
                                    </v-col>
                                </v-row>

                                <!-- Non Editable Sections-->
                                <v-row :justify="justifyCenter" class="mt-0">
                                    <v-col md="10">
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Datacollection Type
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">{{ activityInfo.ActivityType }}</h4>

                                        <h4 class="font-weight-regular heading-3-dark mt-4">Payment</h4>
                                        <div class="page-sub-head pb-0 d-inline-flex">
                                            <h4 class="font-weight-regular heading-3">Payment Linked</h4>
                                            <v-switch v-if="editActivityDetails== false" v-model="activityInfo.IsPaymentLinked" class="ml-2 my-0" label="" hide-details v-bind:disabled="editActivityDetails== false"></v-switch>
                                            <v-switch v-else v-model="editActivity.IsPaymentLinked" class="ml-2 my-0" label="" hide-details v-bind:disabled="editActivityDetails== false"></v-switch>
                                        </div>

                                        <!-- Payment Budget -->
                                        <div v-if="activityInfo.IsPaymentLinked == true">
                                            <div v-if="editActivityDetails == false" class="page-sub-head pb-0 text-limit">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Budget
                                                    <v-btn id="AutoTest_IP175" text max-width="16" min-width="16" height="16" @click="editActivityDetails = true,fnEditActivityClick()">
                                                        <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                    </v-btn>
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{ activityInfo.PaymentLinkedBudget }}</h4>
                                            </div>
                                            <div v-else>
                                                Budget
                                                <v-text-field outlined="" dense="" label="Budget" hide-details class="mb-3" v-model="editActivity.PaymentLinkedBudget">
                                                </v-text-field>
                                            </div>
                                        </div>

                                        <!-- Expense Tag-->
                                        <v-row v-if="activityInfo.IsPaymentLinked==true" class="mb-4">
                                            <v-col class="text-left d-inline-flex" md="12">
                                                <span class="small-text mr-1">
                                                    <v-icon size="15">mdi-account</v-icon> Expense Tags
                                                </span>
                                                <div v-if="editActivityDetails== false">
                                                    <v-chip v-for="(expenseTag, key) in activityInfo.ExpenseTags" :key="expenseTag.TagID" class="tags mr-2" small label>
                                                        {{expenseTag.TagName}}
                                                    </v-chip>
                                                </div>
                                                <div v-else>
                                                    <v-chip id="AutoTest_IP176" v-for="(expenseTag, key) in editActivity.ExpenseTags" :key="expenseTag.TagID" class="tags mr-2" small label close
                                                            @click:close="$delete(activityInfo.ExpenseTags, key)">
                                                        {{expenseTag.TagName}}
                                                    </v-chip>
                                                </div>
                                                <v-menu max-width="300" absolute="" v-model="ExpenseTagPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab=""
                                                               light="" color="#E5F2FF" v-bind:disabled="editActivityDetails == false">
                                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                        </v-btn>
                                                    </template>
                                                    <!--Expense Tag pop up-->
                                                    <v-stepper v-model="ExpenseTagStepper">
                                                        <v-stepper-items>
                                                            <v-stepper-content step="1" class="pa-0">
                                                                <v-card>
                                                                    <v-card-title class="heading-3">
                                                                        Add Expense Tags
                                                                        <v-spacer></v-spacer>
                                                                        <v-btn id="AutoTest_IP177" text="" height="25" min-width="25" max-width="25" color="#707070" @click="ExpenseTagPopUp=false">
                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                        </v-btn>
                                                                    </v-card-title>
                                                                    <v-divider></v-divider>
                                                                    <v-card-text class="v-scrolling-div">
                                                                        <v-row>
                                                                            <p class="mb-2">Search Tags</p>
                                                                            <v-col md="12" align="center">
                                                                                <v-text-field outlined="" dense hide-details label="Search Tag" v-model="SearchExpenseTag">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                            <v-col md="12">
                                                                                <v-chip id="AutoTest_IP178" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
                                                                                        @click="fnAddExpenseTagToList(expenseTag)" class="ma-2" small label close close-icon="mdi-pencil" @click:close="ExpenseTagStepper=3,fnInitializeEditExpenseTagItem(expenseTag)">
                                                                                    {{expenseTag.TagName}}
                                                                                </v-chip>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                    <v-divider></v-divider>
                                                                    <v-card-actions>
                                                                        <v-row justify="center">
                                                                            <v-col md="6">
                                                                                <Success-Button id="AutoTest_IP179" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="ExpenseTagStepper=2"></Success-Button>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-actions>
                                                                </v-card>
                                                            </v-stepper-content>
                                                            <v-stepper-content step="2" class="pa-0">
                                                                <v-card>
                                                                    <v-card-title class="heading-3">
                                                                        <v-btn id="AutoTest_IP180" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="ExpenseTagStepper=1,NewExpenseTagName=''">
                                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                                        </v-btn>
                                                                        Create Expense Tags
                                                                        <v-spacer></v-spacer>
                                                                        <v-btn id="AutoTest_IP181" text="" height="25" min-width="25" max-width="25" color="#707070" @click="ExpenseTagPopUp=false,ExpenseTagStepper=1,NewExpenseTagName=''">
                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                        </v-btn>
                                                                    </v-card-title>
                                                                    <v-divider></v-divider>
                                                                    <v-card-text class="v-scrolling-div">
                                                                        <v-row>
                                                                            <v-col md="12" align="center">
                                                                                <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="NewExpenseTagName">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                    <v-divider></v-divider>
                                                                    <v-card-actions>
                                                                        <v-row justify="center">
                                                                            <v-col md="6">
                                                                                <Success-Button id="AutoTest_IP182" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="ExpenseTagStepper=1,fnAddNewExpenseTag()"></Success-Button>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-actions>
                                                                </v-card>
                                                            </v-stepper-content>
                                                            <v-stepper-content step="3" class="pa-0">
                                                                <v-card>
                                                                    <v-card-title class="heading-3">
                                                                        <v-btn id="AutoTest_IP183" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="ExpenseTagStepper=1,fnInitializeEditExpenseTagItem()">
                                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                                        </v-btn>
                                                                        Edit Tags
                                                                        <v-spacer></v-spacer>
                                                                        <v-btn id="AutoTest_IP184" text="" height="25" min-width="25" max-width="25" color="#707070" @click="ExpenseTagPopUp=false,ExpenseTagStepper=1,fnInitializeEditExpenseTagItem()">
                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                        </v-btn>
                                                                    </v-card-title>
                                                                    <v-divider></v-divider>
                                                                    <v-card-text class="v-scrolling-div">
                                                                        <v-row>
                                                                            <v-col md="12" align="center">
                                                                                <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="EditExpenseTagItem.TagName">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                    <v-divider></v-divider>
                                                                    <v-card-actions>
                                                                        <v-row justify="center">
                                                                            <v-col md="6">
                                                                                <Success-Button id="AutoTest_IP185" style="width:130px" class="mb-2 center-block" title="Save" @click.native="ExpenseTagStepper=1,fnEditExpenseTag()"></Success-Button>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-actions>
                                                                </v-card>
                                                            </v-stepper-content>
                                                        </v-stepper-items>
                                                    </v-stepper>
                                                </v-menu>
                                            </v-col>
                                        </v-row>

                                        <!-- Notify Tag-->
                                        <v-row v-if="activityInfo.IsPaymentLinked == true" class="mt-2 mb-4">
                                            <v-col class="text-left d-inline-flex" md="12">
                                                <span class="small-text mr-1">
                                                    <v-icon size="12">mdi-account</v-icon> Notify Users
                                                </span>
                                                <span v-if="editActivity.ExpenseTagsNotifyUsers">
                                                    <v-avatar size="25" class="team-avatar" v-for="(item,key) in editActivity.ExpenseTagsNotifyUsers.slice(0,3)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                    <v-avatar v-if="editActivity.ExpenseTagsNotifyUsers.length>=4" size="25" class="team-count">
                                                        +{{editActivity.ExpenseTagsNotifyUsers.length-3}}
                                                    </v-avatar>
                                                </span>
                                                <v-menu min-width="100" absolute="" v-model="ExpenseTagsNotifyUsersPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn id="AutoTest_IP186" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                               v-bind:disabled="editActivityDetails == false" @click="if(editActivity.ExpenseTagsNotifyUsers) selectedNotifyUsers=editActivity.ExpenseTagsNotifyUsers">
                                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                        </v-btn>
                                                    </template>
                                                    <!-- user pop up-->
                                                    <v-card>
                                                        <v-card-title class="heading-3">
                                                            Select Users
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP187" text="" height="25" min-width="25" max-width="25" color="#707070" @click="ExpenseTagsNotifyUsersPopUp=false,selectedNotifyUsers=editActivity.ExpenseTagsNotifyUsers">
                                                                <v-icon size="20">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-divider></v-divider>
                                                        <v-card-text style="max-height:150px" class="v-scrolling-div">
                                                            <v-data-table :headers="ProjectUserListTableHeaders"
                                                                          :items="ProjectUserList"
                                                                          item-key="ID"
                                                                          hide-default-header
                                                                          sort-by="Name"
                                                                          show-select
                                                                          class="elevation-1"
                                                                          v-model="selectedNotifyUsers">
                                                                <template v-slot:item.Name="{item}">
                                                                    <v-list class="py-0 table-v-list">
                                                                        <v-list-item class="my-0 px-0">
                                                                            <v-list-item-icon>
                                                                                <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content>
                                                                                {{ item.Name }}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </template>
                                                                <template v-slot:no-data="">
                                                                    <v-alert :value="true" color="error" icon="mdi-warning">
                                                                        Sorry, nothing to display here :(
                                                                    </v-alert>
                                                                </template>
                                                            </v-data-table>
                                                        </v-card-text>
                                                        <v-divider></v-divider>
                                                        <v-card-actions>
                                                            <v-row no-gutters>
                                                                <v-col md="12" class="py-0" align="center">
                                                                    <Success-Button id="AutoTest_IP188" v-bind:disabled="selectedNotifyUsers.length==0" style="width:130px" class="mb-2 center-block" title="Save"
                                                                                    @click.native="ExpenseTagsNotifyUsersPopUp=false,editActivity.ExpenseTagsNotifyUsers=selectedNotifyUsers,selectedNotifyUsers=[]"></Success-Button>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-menu>
                                            </v-col>
                                        </v-row>

                                        <!-- Tags-->
                                        <v-row class="mb-1">
                                            <v-col class="text-left d-inline-flex" md="12">
                                                <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn id="AutoTest_IP189" v-on="on" max-width="15" min-width="15" height="25" outlined="" color="grey"
                                                               v-bind:disabled="editActivityDetails == false"
                                                               @click="fnInitializeSteper()" class="mr-1">
                                                            <v-icon size="15">mdi-plus</v-icon>
                                                        </v-btn>
                                                    </template>
                                                    <v-card>
                                                        <v-card-text class="pt-0 pb-1">
                                                            <v-col cols="12" md="12">
                                                                <!-- Header Text  -->
                                                                <v-col cols="12" md="12" class="d-inline-flex">
                                                                    <v-btn id="AutoTest_IP190" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                        <v-icon size="15">mdi-chevron-left</v-icon>
                                                                    </v-btn>
                                                                    <v-spacer></v-spacer>
                                                                    <h3 class="text-left page-head pt-2">
                                                                        {{TagHeaderName}}
                                                                    </h3>
                                                                    <v-spacer></v-spacer>
                                                                    <v-btn id="AutoTest_IP191" text @click="NewTagMenu = false">
                                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                    </v-btn>
                                                                </v-col>
                                                                <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                                    <v-stepper-items>
                                                                        <!-- Create New Tag Option when there is no Tags are -->
                                                                        <v-stepper-content step="1" class="pa-0">
                                                                            <v-row>
                                                                                <v-col class="pb-1 ml-2">
                                                                                    <v-text-field outlined type="text" dense label="Name"
                                                                                                  v-model="NewTagName" hide-details>
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row class="pt-0">
                                                                                <v-col class="pb-1">
                                                                                    <span class="ml-3">Select Color</span>
                                                                                    <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                    :swatches="swatches"
                                                                                                    hide-canvas
                                                                                                    hide-inputs
                                                                                                    hide-mode-switch
                                                                                                    show-swatches
                                                                                                    v-model="color"
                                                                                                    @input="">
                                                                                    </v-color-picker>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col cols="12" md="12" class="text-center">
                                                                                    <v-btn id="AutoTest_IP192" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>

                                                                        <!-- Select from existing Tags-->
                                                                        <v-stepper-content step="2" class="pa-0">
                                                                            <v-row style="max-height:30vh;overflow-y:scroll">
                                                                                <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                    <p class="mb-2">Search Tags</p>
                                                                                    <v-text-field outlined="" dense="" v-model="SearchTagDetails" label="Search tag here" hide-details class="mb-3">
                                                                                    </v-text-field>
                                                                                    <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                                                                                        <v-chip id="AutoTest_IP193" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                label @click.native="fnAddToeditActivityTags(index)">
                                                                                            {{ index.TagName }}
                                                                                            <v-icon id="AutoTest_IP194" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                        </v-chip>
                                                                                    </v-chip-group>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-col cols="12" md="12" class="text-center mt-3">
                                                                                <v-btn id="AutoTest_IP195" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
                                                                            </v-col>
                                                                        </v-stepper-content>

                                                                        <!-- Edit Tag-->
                                                                        <v-stepper-content step="3" class="pa-0">
                                                                            <v-row>
                                                                                <v-col class="pb-1 ml-2">
                                                                                    <v-text-field outlined type="text" dense
                                                                                                  v-model="EditTagName" hide-details>
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row class="pt-0">
                                                                                <v-col class="pb-1">
                                                                                    <span class="ml-3">Select Color</span>
                                                                                    <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                    :swatches="swatches"
                                                                                                    hide-canvas
                                                                                                    hide-inputs
                                                                                                    hide-mode-switch
                                                                                                    show-swatches
                                                                                                    v-model="color"
                                                                                                    @input="">
                                                                                    </v-color-picker>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col cols="12" md="12" class="text-center">
                                                                                    <v-btn id="AutoTest_IP196" color="success-btn" depressed dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>
                                                                    </v-stepper-items>
                                                                </v-stepper>
                                                            </v-col>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-menu>
                                                <div v-if="!allTags && editActivityDetails==true">
                                                    <v-chip dark v-if="editActivity.ActivityTags.length>0" :color="editActivity.ActivityTags[0].TagColor" label small class="tags mr-1">{{editActivity.ActivityTags[0].TagName}}</v-chip>
                                                    <v-chip dark v-if="editActivity.ActivityTags.length>1" :color="editActivity.ActivityTags[1].TagColor" label small class="tags mr-1">{{editActivity.ActivityTags[1].TagName}}</v-chip>
                                                    <v-chip dark v-if="editActivity.ActivityTags.length>2" label small class="tags tag-count mr-1">+{{editActivity.ActivityTags.length-2}}</v-chip>
                                                </div>
                                                <div v-if="!allTags && editActivityDetails==false">
                                                    <v-chip dark v-if="activityInfo.ActivityTags.length>0" :color="activityInfo.ActivityTags[0].TagColor" label small class="tags mr-1">{{activityInfo.ActivityTags[0].TagName}}</v-chip>
                                                    <v-chip dark v-if="activityInfo.ActivityTags.length>1" :color="activityInfo.ActivityTags[1].TagColor" label small class="tags mr-1">{{activityInfo.ActivityTags[1].TagName}}</v-chip>
                                                    <v-chip dark v-if="activityInfo.ActivityTags.length>2" label small class="tags tag-count mr-1">+{{activityInfo.ActivityTags.length-2}}</v-chip>
                                                </div>
                                                <v-btn id="AutoTest_IP197" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                    <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                </v-btn>
                                                <v-btn id="AutoTest_IP198" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                    <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                </v-btn>
                                            </v-col>
                                        </v-row>
                                        <v-row no-gutter v-if="allTags" class="mt-0">
                                            <v-col v-if="editActivityDetails==true && editActivity.ActivityTags && editActivity.ActivityTags.length > 0" md="12" class="chip-container mb-0 pt-0">
                                                <v-chip-group dark v-for="(TagItem, key) in editActivity.ActivityTags" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                    <v-chip id="AutoTest_IP1198A" small :color="TagItem.TagColor" close @click:close="$delete(editActivity.ActivityTags, key)" class="mb-0">
                                                        {{TagItem.TagName}}
                                                    </v-chip>
                                                </v-chip-group>
                                            </v-col>
                                            <v-col v-if="editActivityDetails==false && activityInfo.ActivityTags && activityInfo.ActivityTags.length > 0" md="12" class="chip-container mb-0 pt-0">
                                                <v-chip-group dark v-for="(TagItem, key) in activityInfo.ActivityTags" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                    <v-chip small :color="TagItem.TagColor" class="mb-0">
                                                        {{TagItem.TagName}}
                                                    </v-chip>
                                                </v-chip-group>
                                            </v-col>
                                        </v-row>

                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Assignment
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">These fields has been transferred to the leaf level activity Sub Activity 1.1.1.1</h4>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <!--<v-row :justify="justifyCenter" v-if="editActivityDetails == true">
                          <v-col md="12" class="text-center">
                            <Success-Button id="AutoTest_IP199" class="btn-122x36 mr-4" title="Save" @click.native="editActivityDetails =false,fnAddActivity()"></Success-Button>
                            <Default-Button-Outlined id="AutoTest_IP200"  class="btn-122x36"  title="Cancel" @click.native="editActivityDetails =false"></Default-Button-Outlined>
                          </v-col>
                        </v-row>-->

                    </v-tab-item>

                    <!-- Activity Log-->
                    <v-tab-item value="tab-2">
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="6">
                                <v-card class="elevation-0 pa-0">
                                    <v-card-text style="height:300px" class="v-scrolling-div">
                                        <v-list dense="" class="py-0">
                                            <template v-for="(item, index) in ActivityLogList">
                                                <v-list-item class="py-0">
                                                    <v-list-item-icon>
                                                        <v-avatar size="40" :color="`${commonGetColor(item.Name)}`">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
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
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </template>
                                        </v-list>
                                    </v-card-text>
                                </v-card>
                            </v-col>
                        </v-row>
                    </v-tab-item>
                    <v-tab-item class="mt-4" value="tab-3">
                        <v-card elevation="0">
                            <v-card-text>
                                <v-row>
                                    <v-col cols="12" class="pt-0 py-0">
                                        <!--If Response Type Pivot Table-->
                                        <v-row class="py-0">
                                            <v-col cols="12" class="py-0">
                                                <v-row justify="start" class="py-0">
                                                    <v-col cols="12" md="4" class="py-0">
                                                        <v-select :items="lstRepeatSections"
                                                                  v-model="presetSelected"
                                                                  item-value="presetName"
                                                                  item-text="presetName"
                                                                  @change="RestorePivotPreset()"
                                                                  label="Preset"
                                                                  dense
                                                                  outlined></v-select>
                                                    </v-col>
                                                    <v-col cols="12" md="4" class="pt-0">
                                                        <v-row class="pt-0 mt-0">
                                                            <v-col cols="4" class="pt-0">
                                                                <v-btn outlined block depressed @click="addPresetDialogOpen()"><v-icon dense>mdi-plus</v-icon> Preset</v-btn>
                                                            </v-col>
                                                            <v-col cols="4" class="pt-0">
                                                                <v-btn outlined block depressed @click="editPresetDialogOpen()"><v-icon dense>mdi-pencil-outline</v-icon>Edit</v-btn>
                                                            </v-col>
                                                            <v-col cols="4" class="pt-0">
                                                                <v-btn outlined block depressed @click="deletePresetDialog=true"><v-icon dense>mdi-trash-can</v-icon>Delete</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>
                                                <v-row class="py-0">
                                                    <v-col cols="12" md="4" class="py-0">
                                                        <v-row class="pa-0 ma-0">
                                                            <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                                                                <v-menu v-model="submittedmStartDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        style="border:solid; border-color:red; width:100px;">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="submittedStartDate" label="Start Date"
                                                                                      outlined dense
                                                                                      readonly
                                                                                      v-on="on"></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="submittedbystartDate" no-title @input="submittedmStartDate = false" format="DD-MM-YYYY"
                                                                                   :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>

                                                            </v-col>
                                                            <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                                                                <v-menu v-model="submittedmEndDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="submittedEndDate" label="End Date"
                                                                                      outlined dense
                                                                                      readonly
                                                                                      v-on="on"></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="submittedbyendDate" no-title @input="submittedmEndDate = false"
                                                                                   :min="submittedbystartDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col md="2" class="py-0" v-if="lstRepeatLogic.length">
                                                        <v-select
                                                            outlined
                                                            label="Sections"
                                                            dense
                                                            v-model="repeatLogic"
                                                            :items="computedLstRepeatLogic"
                                                            :item-value="i => i.Key"
                                                            :item-text="i => i.Value"
                                                            :menu-props="{
                                                                closeOnContentClick: true,
                                                            }"
                                                        >
                                                            <template v-slot:item="{ item, on, attrs }">
                                                                <v-list-item v-on="on" v-bind="attrs">
                                                                    <v-list-item-content>
                                                                        <v-list-item-title>
                                                                            Section - {{item.Value}}
                                                                        </v-list-item-title>
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                            </template>
                                                        </v-select>
                                                    </v-col>
                                                    <v-col cols="12" md="4" class="py-0">
                                                        <v-row class="pa-0 ma-0">
                                                            <v-col cols="4" class="py-0">
                                                                <v-btn outlined block depressed @click="submitpivottable()">Submit</v-btn>
                                                            </v-col>
                                                            <v-col cols="4" class="py-0 px-2">
                                                                <v-btn outlined block depressed @click="clearDateSelection()">Clear Preset</v-btn>
                                                            </v-col>
                                                            <v-col cols="12" md="4" class="text-center py-0">
                                                                <v-btn outlined depressed @click="ExportToExcel('xlsx')">
                                                                    <v-icon dense class="px-2">mdi-export-variant</v-icon> Export
                                                                </v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                            <v-col cols="12" class="pt-0">
                                                <div id="output" class="pivot_table_container">
                                                </div>
                                            </v-col>
                                            <!--dialod for add preset functionality-->
                                            <v-dialog v-model="addPresetDialog"
                                                      persistent
                                                      max-width="600">
                                                <v-form ref="entryForm" @submit.prevent="submitHandler">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head pb-0">
                                                            <span style="font-weight:900;">Preset</span>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP641"
                                                                   text
                                                                   @click="addPresetDialogCloseButton()">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="py-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters justify="center">
                                                                    <v-col cols="12" md="12" class="pt-7">
                                                                        <v-text-field v-model="presetName" :rules="rules.name" label="Name this Preset"
                                                                                      outlined>
                                                                            <template v-slot:append>
                                                                                <span class="error--text">*</span>
                                                                            </template>
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>

                                                                <v-row no-gutters justift="center" style="height:50px;" class="mb-4 v-scrolling-div">
                                                                    <v-col cols="2" class="pl-0 pb-0 pt-1" style="text-align:center;">
                                                                        <span style="font-size:15px;color:black;">X-Axis</span>
                                                                    </v-col>
                                                                    <v-col cols="9" class="pl-0 ">
                                                                        <v-menu v-for="(i,ind) in rowDialog" open-on-click="xAxisButton == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                            <template v-slot:activator="{ on, attrs }">
                                                                                <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue(i)" class="mr-2 mb-2" :key="ind" color="#439bde" id="AutoTest_I145" small
                                                                                        label>
                                                                                    <span style="color:white;">{{i}}</span>
                                                                                    <v-icon id="AutoTest_I154" right color="white" size="20">mdi-chevron-down</v-icon>
                                                                                </v-chip>
                                                                            </template>
                                                                            <v-card>
                                                                                <v-list>
                                                                                    <v-list-item @click="changeSelectedPivotValue(item,ind,'x')" v-for="(item, index) in nonSelectedData"
                                                                                                 :key="index">
                                                                                        <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                    </v-list-item>
                                                                                </v-list>

                                                                            </v-card>
                                                                        </v-menu>
                                                                        <v-btn @click="changeSelectedPivotValue(null,null,'lastIndexX')" class="mb-2" :disabled="nonSelectedData.length == 0 ? true : false" outlined style="width:40px;height:30px;min-width:40px;" small><v-icon>mdi-plus</v-icon></v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row no-gutters justift="center" style="height:50px;" class="mb-4 v-scrolling-div">
                                                                    <v-col cols="2" class="pl-0  pt-1" style="text-align:center;">
                                                                        <span style="font-size:15px;color:black;">Y-Axis</span>
                                                                    </v-col>
                                                                    <v-col cols="9" class="pl-0 ">
                                                                        <v-menu v-for="(i,ind) in colsDialog" open-on-click="yAxisButton == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                            <template v-slot:activator="{ on, attrs }">
                                                                                <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue(i)" class="mr-2 mb-2" :key="ind" color="#439bde" id="AutoTest_I145" small
                                                                                        label>
                                                                                    <span style="color:white;">{{i}}</span>
                                                                                    <v-icon id="AutoTest_I154" right color="white" size="20">mdi-chevron-down</v-icon>
                                                                                </v-chip>
                                                                            </template>
                                                                            <v-card>
                                                                                <v-list>
                                                                                    <v-list-item @click="changeSelectedPivotValue(item,ind,'y')" v-for="(item, index) in nonSelectedData"
                                                                                                 :key="index">
                                                                                        <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                    </v-list-item>
                                                                                </v-list>

                                                                            </v-card>
                                                                        </v-menu>
                                                                        <v-btn class="mb-2" @click="changeSelectedPivotValue(null,null,'lastIndexY')" :disabled="nonSelectedData.length == 0" outlined style="width:40px;height:30px;min-width:40px;" small><v-icon>mdi-plus</v-icon></v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row no-gutters justify="end">
                                                                    <v-col cols="6" md="3" class="pl-0 pb-0 pt-1">
                                                                        <v-btn centered
                                                                               color="success-btn elevation-0 enter-btn"
                                                                               class="mr-2"
                                                                               dark
                                                                               block
                                                                               type="submit">Save</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-form>
                                            </v-dialog>
                                            <!--dialog for edit functionality-->
                                            <v-dialog v-model="editPresetDialog"
                                                      persistent
                                                      max-width="800">
                                                <v-form ref="editForm" @submit.prevent="submitEditHandler">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head pb-0">
                                                            <span style="font-weight:900;">Preset</span>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id=""
                                                                   text
                                                                   @click="editPresetDialog=false">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="py-5 ma-0" style="background:#f5f5f5">
                                                            <v-row no-gutters justify="center">
                                                                <v-col cols="12" md="12" class="pt-7 px-3">
                                                                    <v-select :items="lstRepeatSections"
                                                                              v-model="presetSelected_Edit"
                                                                              item-value="presetName"
                                                                              item-text="presetName"
                                                                              @change="GetPivotPresetData_Edit()"
                                                                              label="Select Preset"
                                                                              dense
                                                                              background-color="white"
                                                                              outlined></v-select>
                                                                </v-col>
                                                            </v-row>

                                                            <v-row no-gutters justift="center" align="center" class="mb-4 px-3">
                                                                <v-col cols="2" class="pa-2" style="text-align:center;background:#e6EEEE">
                                                                    <span style="font-size:15px;color:black;">X-Axis</span>
                                                                </v-col>
                                                                <v-col cols="10" class="pa-2 ma-0 v-scrolling-div" style="background:white;max-height:70px">
                                                                    <v-menu v-for="(i,ind) in rowDialog_Edit" open-on-click="xAxisButton_Edit == i" absolute :close-on-click="true" :close-on-content-click="true">

                                                                        <template v-slot:activator="{ on, attrs }">
                                                                            <v-chip v-bind="attrs" class="mr-2" :key="ind" color="#e6EEEE" id="" small label>
                                                                                <span style="color:black;">{{i}}</span>
                                                                                <v-icon id="" v-on="on" @click="nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-chevron-down</v-icon>
                                                                                <v-icon id="" @click="Remove_nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-minus-circle-outline</v-icon>
                                                                            </v-chip>
                                                                        </template>
                                                                        <v-card style="height:400px;" class=" v-scrolling-div">
                                                                            <v-list>
                                                                                <v-list-item @click="changeSelectedPivotValue_Edit(item,ind,'x')" v-for="(item, index) in nonSelectedData_Edit"
                                                                                             :key="index">
                                                                                    <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                </v-list-item>
                                                                            </v-list>

                                                                        </v-card>
                                                                    </v-menu>
                                                                    <v-btn @click="changeSelectedPivotValue_Edit(null,null,'lastIndexX')" :disabled="nonSelectedData_Edit.length == 0 ? true : false" style="width:5%;height:20px;min-width:5%;background:#e6EEEE" small><v-icon size="20">mdi-plus</v-icon></v-btn>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters justift="center" align="center" class="mb-4 px-3">
                                                                <v-col cols="2" class="pa-2 ma-0" style="text-align:center;background:#e6EEEE">
                                                                    <span style="font-size:15px;color:black;">Y-Axis</span>
                                                                </v-col>
                                                                <v-col cols="10" class="pa-2 ma-0 v-scrolling-div" style="background:white;max-height:70px">
                                                                    <v-menu v-for="(i,ind) in colsDialog_Edit" open-on-click="yAxisButton_Edit == i" absolute :close-on-click="true" :close-on-content-click="true">

                                                                        <template v-slot:activator="{ on, attrs }">
                                                                            <v-chip v-bind="attrs" class="mr-2" :key="ind" color="#e6EEEE" id="" small label>
                                                                                <span style="color:black;">{{i}}</span>
                                                                                <v-icon id="" v-on="on" @click="nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-chevron-down</v-icon>
                                                                                <v-icon id="" @click="Remove_nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-minus-circle-outline</v-icon>
                                                                            </v-chip>
                                                                        </template>
                                                                        <v-card style="height:400px;" class=" v-scrolling-div">
                                                                            <v-list>
                                                                                <v-list-item @click="changeSelectedPivotValue_Edit(item,ind,'y')" v-for="(item, index) in nonSelectedData_Edit"
                                                                                             :key="index">
                                                                                    <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                </v-list-item>
                                                                            </v-list>

                                                                        </v-card>
                                                                    </v-menu>
                                                                    <v-btn @click="changeSelectedPivotValue_Edit(null,null,'lastIndexY')" :disabled="nonSelectedData_Edit.length == 0" style="width:5%;height:20px;min-width:5%;background:#e6EEEE" small><v-icon size="20">mdi-plus</v-icon></v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-text>
                                                        <v-card-actions>
                                                            <v-row no-gutters justify="end">
                                                                <v-col cols="6" md="3" class="pl-0 pb-4 pt-1 pr-md-3">
                                                                    <v-btn centered
                                                                           color="primary elevation-0 enter-btn"
                                                                           class="mr-2"
                                                                           dark
                                                                           block
                                                                           type="submit">Save Preset</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-form>
                                            </v-dialog>
                                            <!--dialog for delete functionality-->
                                            <v-dialog v-model="deletePresetDialog"
                                                      persistent
                                                      max-width="800">
                                                <v-form ref="deleteForm" @submit.prevent="submitDeleteHandler">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head pb-0">
                                                            <span style="font-weight:900;">Preset</span>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id=""
                                                                   text
                                                                   color="black"
                                                                   @click="deletePresetDialog=false">
                                                                <v-icon color="#707070" size="20">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="py-5 ma-0" style="background:#f5f5f5">
                                                            <v-row class="pa-0 ma-0">
                                                                <v-col class="text-center pa-0 ma-0">
                                                                    <span>Presets once deleted cannot be restored. Are you sure want to delete the presets ?</span>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters justify="center">
                                                                <v-col cols="12" md="12" class="py-7">
                                                                    <v-select :items="lstRepeatSections"
                                                                              v-model="presetSelected_Delete"
                                                                              item-value="presetName"
                                                                              item-text="presetName"
                                                                              label="Select Preset"
                                                                              dense
                                                                              background-color="white"
                                                                              outlined></v-select>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-text>
                                                        <v-card-actions>
                                                            <v-row no-gutters justify="end">
                                                                <v-col cols="6" md="3" class="pl-0 pb-4 pt-1 pr-md-3">
                                                                    <v-btn centered
                                                                           color="primary elevation-0 enter-btn"
                                                                           class="mr-2"
                                                                           dark
                                                                           block
                                                                           type="submit">Delete Preset</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-form>
                                            </v-dialog>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                        </v-card>
                    </v-tab-item>
                </v-tabs>

            </v-col>
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
        </v-card-text>
    </v-card>

</template>
<script>

    import AddButtonFab from '@/components/add-button-fab.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'
    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import BulkActionButtonOutlined from '@/components/bulk-action-button-outlined.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '../../utils.js'

    var objManageProjectActivity;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };

    var objSurvey;
    async function importscriptSurvey() {
        return Promise.all([
            import("../../BL/ProjectSurvey.js").then(mod => {
                objSurvey = new mod.ProjectSurvey();
            })
        ]);
    };

    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                //General parameters
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                },
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    GroupId: 0,
                },
                activityInfo: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },
                editActivity: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },

                ExpenseTagStepper: 1,
                ExpenseTagPopUp: false,
                NewExpenseTagName: "",
                SearchExpenseTag: "",
                EditExpenseTagItem: { TagID: 0, TagName: "" },
                ProjectExpenseTagList: [],
                filterProjectExpenseTagList: [],
                ProjectUserListTableHeaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                ProjectUserList: [],
                selectedNotifyUsers: [],
                ExpenseTagsNotifyUsersPopUp: false,

                //Activity Log tab parameters
                ActivityLogList: [],

                justifyCenter: 'center',
                ActivityTab: "tab-1",
                editActivityDetails: false,
                PaymentLinked: false,
                // Tags ------------
                NewTagMenu: false,
                SearchTagDetails: "",
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                NewTagName: "",
                color: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],
                ],
                allTags: false,
                EditTagName: "",

                // Expense Tag
                ExpenseTagPopUp: false,
                offset: false,
                closeonClick: false,
                closeOnContentClick: false,

                //FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                //ToDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                // Notify User
                //NotificationSelectPopUp: false,
                //ProjectStartDate: "",
                //ProjectEndDate: "",
                //userimage: "",
                //presetName: '',
                //presetSelected: {},
                //pivotjsondata: [],
                //presetLocalStorage: [],
                //presetStoreSave: false,
                //attributes: [],
                //rows: [],
                //cols: [],
                //vals: [],
                //submittedbystartDate: null,
                //submittedbyendDate: null,
                //submittedmStartDate: false,
                //submittedmEndDate: false,
                //addPresetDialog: '',
                //editPresetDialog: '',
                //deletePresetDialog: '',
                //rules: [],
                //nonSelectedData: [],
                //rowDialog: [],
                //colsDialog: [],
                outputSurveyIds: "",

                rules: [],
                nonSelectedData: [],
                nonSelectedData_Edit: [],
                colsDialog: [],
                colsDialog_Edit: [],
                rowDialog: [],
                rowDialog_Edit: [],
                xAxisButton: '',
                xAxisButton_Edit: '',
                yAxisButton: '',
                yAxisButton_Edit: '',
                addPresetDialog: '',
                editPresetDialog: '',
                deletePresetDialog: '',
                items: [],
                //Pivot table dummy data
                valueFilter: {
                    Meal: {
                        Dinner: true
                    }
                },
                config: {},
                filteredData: [],
                presetName: '',
                presetSelected: {},
                presetSelected_Edit: {},
                presetSelected_Delete: {},
                pivotjsondata: [],
                presetLocalStorage: [],
                presetStoreSave: false,
                attributes: [],
                rows: [],
                cols: [],
                vals: [],
                disabledFromDragDrop: [],
                hiddenFromDragDrop: [],
                sortonlyFromDragDrop: [],
                pivotColumns: [],
                loading: false,
                aggregatorName: 'Count',
                rendererName: 'Table',
                localeStrings: {
                    en: {
                        renderError: 'An error occurred rendering the PivotTable results.',
                        computeError: 'An error occurred computing the PivotTable results.',
                        uiRenderError: 'An error occurred rendering the PivotTable UI.',
                        selectAll: 'Select All',
                        selectNone: 'Select None',
                        tooMany: 'too many values to show',
                        filterResults: 'Filter values',
                        totals: 'Totals',
                        only: 'only',
                        rendererNames: {
                            Table: 'Table',
                            'Table Heatmap': 'Table Heatmap',
                            'Table Col Heatmap': 'Table Col Heatmap',
                            'Table Row Heatmap': 'Table Row Heatmap',
                            'Export Table TSV': 'Export Table TSV',
                            'Grouped Column Chart': 'Grouped Column Chart',
                            'Stacked Column Chart': 'Stacked Column Chart',
                            'Grouped Bar Chart': 'Grouped Bar Chart',
                            'Stacked Bar Chart': 'Stacked Bar Chart',
                            'Line Chart': 'Line Chart',
                            'Dot Chart': 'Dot Chart',
                            'Area Chart': 'Area Chart',
                            'Scatter Chart': 'Scatter Chart',
                            'Multiple Pie Chart': 'Multiple Pie Chart'
                        },
                        aggregatorMap: {
                            Count: 'Count',
                            'Count Unique Values': 'Count Unique Values',
                            'List Unique Values': 'List Unique Values',
                            Sum: 'Sum',
                            'Integer Sum': 'Integer Sum',
                            Average: 'Average',
                            Median: 'Median',
                            'Sample Variance': 'Sample Variance',
                            'Sample Standard Deviation': 'Sample Standard Deviation',
                            Minimum: 'Minimum',
                            Maximum: 'Maximum',
                            First: 'First',
                            Last: 'Last',
                            'Sum over Sum': 'Sum over Sum',
                            'Sum as Fraction of Total': 'Sum as Fraction of Total',
                            'Sum as Fraction of Rows': 'Sum as Fraction of Rows',
                            'Sum as Fraction of Columns': 'Sum as Fraction of Columns',
                            'Count as Fraction of Total': 'Count as Fraction of Total',
                            'Count as Fraction of Rows': 'Count as Fraction of Rows',
                            'Count as Fraction of Columns': 'Count as Fraction of Columns'
                        }
                    }
                },
                locale: 'en',
                //Bar Chart Dummy Data
                likerIndex: '',
                likerSection: '',
                responsesBarData: {
                    labels: ['Organic Method', 'Organic Method', 'Other Method'],
                    datasets: [{
                        label: 'Options',
                        barThickness: 35,
                        data: [10, 60, 85],
                        backgroundColor: [
                            '#F1C100',
                            '#F1C100',
                            '#F1C100',
                        ],
                    }]
                },
                responsesBarOptions: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }

                },
                //_____________
                //Line Chart Dummy Data
                responsesLineData: {
                    labels: ['Organic Method', 'Organic Method', 'Other Method'],
                    datasets: [{
                        label: 'Options',
                        data: [10, 60, 85],
                        backgroundColor: 'transparent',
                        borderColor: '#F1C100'
                    }]
                },
                responsesLineOptions: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }

                },
                //_____________
                //Likert default
                Likerts: [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ],
                LikertsDefault: [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ],
                ResponseType: 'Pivot Table',
                rules: {
                    required: value => !!value || 'Required.',
                },
                rows_Edit: [],
                cols_Edit: [],
                pivotdata: [],
                submittedbystartDate: null,
                submittedbyendDate: null,
                submittedmStartDate: false,
                submittedmEndDate: false,
                ProjectStartDate: "",
                ProjectEndDate: "",
                repeatLogic: null,
                lstRepeatLogic: [],
                lstRepeatSections: [],
            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.activityInfo.ActivityId = this.$route.query.ActID;
            await importscript();
            await importscriptSurvey();
            await this.fnGetActivityDetailsByID();
            await this.fnGetOutputSurveyIds();
            await this.fnGetProjectDetailsByID();
            await this.fnGetUsersListByGroupId();
            await this.fnGetExpenseTagList();
            await this.fnGetAllProjectTags();
        },
        computed: {
            computedEditStartDate() {
                return this.formatDate(this.editActivity.StartDate)
            },
            computedEditEndDate() {
                return this.formatDate(this.editActivity.EndDate)
            },
            FormattedDate() {
                return objUtils.common.formatDate(this.FrmDate)
            },
            computedStartDate() {
                return objUtils.common.formatDate(this.startDate)
            },
            computedEndDate() {
                return objUtils.common.formatDate(this.endDate)
            },

            unusedAttrs() {
                return this.config.unusedAttrs
            },
            submittedStartDate() {
                return objUtils.common.formatDate(this.submittedbystartDate)
            },
            submittedEndDate() {
                return objUtils.common.formatDate(this.submittedbyendDate)
            },
            computedLstRepeatLogic() {
                return [{Key: null, Value: 'Main Form'}, ...this.lstRepeatLogic]
            },
        },

        methods: {

            /// <summary>
            ///  commonGetColor :- Function used to get color
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            ///  formatDateAndTime :- Function used to formatting date and time
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDateAndTime(date) {
                return objUtils.common.formatDateAndTime(date);
            },

            /// <summary>
            ///  formatDate :- Function used to formatting a date
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDate(date) {
                return objUtils.common.formatDate(date);
            },

            /// <summary>
            ///  fnGetProjectDetailsByID :- Function used to fetch project details based on prjID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProjectActivity.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    this.ProjectStartDate = result.data.StartDate;
                    this.ProjectEndDate = result.data.EndDate;

                } else {
                    this.Msg = "Failed to get details of Project " + this.projectInfo.ProjectId;
                    this.errorSnackbarMsg = true;

                }
            },

            /// <summary>
            ///  fnGetActivityDetailsByID :- Function used to fetch activity details based on ActivityId
            /// </summary>
            /// <param name="ActivityId"></param>
            /// <returns> </returns>
            async fnGetActivityDetailsByID() {
                var result = await objManageProjectActivity.GetActivityDetailsByID(this.activityInfo.ActivityId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.activityInfo = result.data;
                    console.log(this.activityInfo)

                } else {
                    this.Msg = "Failed to get activity details.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetUsersListByGroupId :- Function used to fetch users list based on GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectActivity.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectUserList = result.data;
                    this.filterProjectUserList = result.data;
                }
            },

            /// <summary>
            ///  fnEditActivityClick :- Function used to fetch activity details for edit
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditActivityClick() {
                this.editActivity.ActivityId = this.activityInfo.ActivityId;
                this.editActivity.ParentActivityId = this.activityInfo.ParentActivityId;
                this.editActivity.ActivityName = this.activityInfo.ActivityName;
                this.editActivity.Description = this.activityInfo.Description;
                this.editActivity.StartDate = this.activityInfo.StartDate;
                this.editActivity.EndDate = this.activityInfo.EndDate;
                this.editActivity.WeightagePercentage = this.activityInfo.WeightagePercentage;
                if (this.activityInfo.ActivityTags) {
                    this.activityInfo.ActivityTags.forEach(u => {
                        this.editActivity.ActivityTags.push({ TagID: u.TagID, TagName: u.TagName, TagColor: u.TagColor })
                    });
                }
                this.editActivity.ActivityType = this.activityInfo.ActivityType;
                this.editActivity.SurveyId = this.activityInfo.SurveyId;
                this.editActivity.SurveySectionId = this.activityInfo.SurveySectionId;
                this.editActivity.SurveyQuestionId = this.activityInfo.SurveyQuestionId;
                this.editActivity.SurveyResponseId = this.activityInfo.SurveyResponseId;
                this.editActivity.SurveyLogic = this.activityInfo.SurveyLogic;
                this.editActivity.SurveyLogic = this.activityInfo.SurveyLogic;
                this.editActivity.SurveyTarget = this.activityInfo.SurveyTarget;
                this.editActivity.IsPaymentLinked = this.activityInfo.IsPaymentLinked;
                this.editActivity.PaymentLinkedBudget = this.activityInfo.PaymentLinkedBudget;
                if (this.activityInfo.ExpenseTags) {
                    this.activityInfo.ExpenseTags.forEach(u => {
                        this.editActivity.ExpenseTags.push({ TagID: u.TagID, TagName: u.TagName })
                    });
                }
                this.editActivity.ExpenseTagsNotifyUsers = this.activityInfo.ExpenseTagsNotifyUsers;
                this.editActivity.WorkflowId = this.activityInfo.WorkflowId;
                this.editActivity.Periodicity = this.activityInfo.Periodicity;
                this.editActivity.PeriodicityOnceDate = this.formatDate(this.activityInfo.PeriodicityOnceDate);
                this.editActivity.PeriodicityWeek = this.activityInfo.PeriodicityWeek;
                this.editActivity.PeriodicityMonthDay = this.activityInfo.PeriodicityMonthDay;
                this.editActivity.PeriodicityMonth = this.activityInfo.PeriodicityMonth;
                this.editActivity.PeriodicityMonthDateDay = this.activityInfo.PeriodicityMonthDateDay;
                this.editActivity.ActivityUsers = this.activityInfo.ActivityUsers;
            },

            /// <summary>
            ///  fnGetExpenseTagList :- Function used to fetch all expense tag list based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetExpenseTagList() {
                var result = await objManageProjectActivity.GetAllExpenseTags(this.projectInfo.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg = "Failed to get all expense tag list.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnAddNewExpenseTag :- Function used to create new expense tag for payment linked activity based on ProjectId and tagname
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddNewExpenseTag() {
                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(this.NewExpenseTagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("TagName", this.NewExpenseTagName);
                data.append("ProjectId", this.projectInfo.ProjectId);
                var result = await objManageProjectActivity.AddNewExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.NewExpenseTagName = "";
                        this.fnGetExpenseTagList();
                    } else {
                        this.Msg = "Failed to add expense tag.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnAddExpenseTagToList :- Function used to add expense tag to the list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnAddExpenseTagToList(item) {
                if (this.editActivity.ExpenseTags.length == 0)
                    this.editActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                else {
                    var temp = this.editActivity.ExpenseTags.filter(x => x.TagID == item.TagID);
                    if (temp.length == 0) {
                        this.editActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                    }
                }
            },

            /// <summary>
            ///  fnInitializeEditExpenseTagItem :- Function used to initialize expense tag variables
            /// </summary>
            /// <param name="exptagitem = null"></param>
            /// <returns> </returns>
            fnInitializeEditExpenseTagItem(exptagitem = null) {
                if (exptagitem == null) {
                    this.EditExpenseTagItem.TagID = 0;
                    this.EditExpenseTagItem.TagName = "";
                } else {
                    this.EditExpenseTagItem.TagID = exptagitem.TagID;
                    this.EditExpenseTagItem.TagName = exptagitem.TagName;
                }
            },

            /// <summary>
            ///  fnEditExpenseTag :- Function used to edit expense tag item, when clicking on the save button
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnEditExpenseTag(type) {
                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(this.EditExpenseTagItem.TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("TagID", this.EditExpenseTagItem.TagID);
                data.append("TagName", this.EditExpenseTagItem.TagName);
                var result = await objManageProjectActivity.EditExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.fnInitializeEditExpenseTagItem(null);
                        this.fnGetExpenseTagList();
                    } else {
                        this.Msg = "Failed to edit expense tag.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnAddActivity :- Function used to update activity after edit
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddActivity() {
                var item = this.editActivity;
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("ActivityId", item.ActivityId);
                data.append("ParentActivityId", item.ParentActivityId);
                data.append("ActivityName", item.ActivityName);
                data.append("Description", item.Description);
                data.append("StartDate", item.StartDate);
                data.append("EndDate", item.EndDate);
                data.append("WeightagePercentage", item.WeightagePercentage);
                data.append("ActivityTagsList", JSON.stringify(item.ActivityTags));
                data.append("ActivityType", item.ActivityType);
                data.append("SurveyId", item.SurveyId);
                if (item.ActivityType == "Target") {
                    data.append("SurveySectionId", item.SurveySectionId);
                    data.append("SurveyQuestionId", item.SurveyQuestionId);
                    data.append("SurveyResponseId", item.SurveyResponseId);
                    data.append("SurveyLogic", item.SurveyLogic);
                    data.append("SurveyTarget", item.SurveyTarget);
                } else {
                    data.append("SurveySectionId", 0);
                    data.append("SurveyQuestionId", 0);
                    data.append("SurveyResponseId", 0);
                    data.append("SurveyLogic", "");
                    data.append("SurveyTarget", "");
                }
                data.append("IsPaymentLinked", item.IsPaymentLinked);
                if (item.IsPaymentLinked) {
                    data.append("PaymentLinkedBudget", item.PaymentLinkedBudget);
                    data.append("ExpenseTagsList", JSON.stringify(item.ExpenseTags));
                    data.append("ExpenseTagsNotifyUsersList", JSON.stringify(item.ExpenseTagsNotifyUsers));
                } else {
                    data.append("PaymentLinkedBudget", 0);
                    data.append("ExpenseTagsList", null);
                    data.append("ExpenseTagsNotifyUsersList", null);
                }
                data.append("WorkflowId", item.WorkflowId);
                data.append("Periodicity", item.Periodicity);
                data.append("PeriodicityOnceDate", item.PeriodicityOnceDate);
                data.append("PeriodicityWeek", item.PeriodicityWeek);
                data.append("PeriodicityMonthDay", item.PeriodicityMonthDay);
                data.append("PeriodicityMonth", item.PeriodicityMonth);
                data.append("PeriodicityMonthDateDay", item.PeriodicityMonthDateDay);
                data.append("ActivityUsersList", JSON.stringify(item.ActivityUsers));

                var result = await objManageProjectActivity.AddActivity(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.Msg = "Successfully updated activity.";
                        this.successSnackbarMsg = true;
                        this.fnGetActivityDetailsByID();
                    } else if (result.status == "PeriodicityFailed") {
                        this.Msg = "Periodicity update Failed";
                        this.errorSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed to update activity.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnGetActivityLogList :- Function used to fetch activity log details based on ProjectId,ActivityId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetActivityLogList() {
                var result = await objManageProjectActivity.GetActivityLogList(this.projectInfo.ProjectId, this.activityInfo.ActivityId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ActivityLogList = result.data;
                } else {
                    this.Msg = "Failed to get activity details.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnInitializeSteper :- Function used to initialize stepper for tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create new tag based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var temp = this.AllTagList.filter(x => x.TagName.includes(TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);

                var result = await objManageProjectActivity.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllProjectTags()
                    }
            },

            /// <summary>
            ///  fnGetAllProjectTags :- Function used to fetch all project tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllProjectTags() {
                var result = await objManageProjectActivity.GetAllProjectTags(this.initiativeInfo.InitiativeId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = "Failed to get tag list.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnAddToeditActivityTags :- Function used to add tag to the existing tag items in edit mode based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToeditActivityTags(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.editActivity.ActivityTags.length > 0) {
                        var temp = this.editActivity.ActivityTags.filter(x => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.editActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                        }
                    } else {
                        this.editActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                }
            },

            /// <summary>
            ///  fnEditTagDetails :- Function used to get activity tag details for edit
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnEditTagDetails(TagItem) {
                this.AllTagListSteps = 3;
                this.TagHeaderName = 'Edit Tag Name';
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;
            },

            /// <summary>
            ///  UpdateTagDetails :- Function used to update activity tag details in edit mode based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var temp = this.AllTagList.filter(x => x.TagName.includes(TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();
                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objManageProjectActivity.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllProjectTags()
                        this.UpdatingTagItem = "";
                    }
            },

            async fnGetOutputSurveyIds() {
                var result = await objManageProjectActivity.GetOutputSurveyIds(this.activityInfo.ActivityId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    var surveyIds = result.data;
                    this.outputSurveyIds = surveyIds;
                } else {
                    this.Msg = "Failed to get activity details.";
                    this.errorSnackbarMsg = true;
                }
            },
            async getpivottable() {

                //if ($.pivotUtilities != undefined) {
                //    var dateFormat = $.pivotUtilities.derivers.dateFormat;
                //    var sortAs = $.pivotUtilities.sortAs;
                //}
                var renderers = $.extend($.pivotUtilities.renderers,
                    $.pivotUtilities.plotly_renderers);

                var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                url += "?url=getpivotdata?surveyid=" + this.outputSurveyIds + "&repeatenable=1";

                var self = this;


                $.getJSON(url, function (tdata) {
                    var data = tdata;
                    self.pivotdata = [];
                    self.pivotdata = tdata;
                    self.appendFilePath();

                    var temparray = [];
                    for (var i = 0; i < data.length; i++) {
                        temparray.push(...Object.keys(data[i]));
                    }
                    let arr2 = [...new Set(temparray.flat(1))];
                    //self.rows = Object.keys(data[0]);
                    //self.rows = arr2;
                    //self.cols = arr2;
                    self.attributes = arr2;
                    self.nonSelectedData = arr2;
                    self.nonSelectedData_Edit = arr2;
                    if (data != null) {
                        $("#output").pivotUI(data, {

                            //hiddenAttributes: ["SubmittedDate"],
                            hiddenAttributes: ["BeneficiaryName"],
                            //derivedAttributes: {
                            //    "Year": dateFormat("SubmittedDate", "%y", true),
                            //    "Quarter": function (record) {
                            //        var d = new Date(record.SubmittedDate);
                            //        var q = [4, 1, 2, 3];
                            //        return q[Math.floor(d.getMonth() / 3)];
                            //    },

                            //    "Month": dateFormat("SubmittedDate", "%n", true),
                            //    "Week": function (record) {
                            //        var d = new Date(record.SubmittedDate);
                            //        return d.getWeek()
                            //    },
                            //    "Day": dateFormat("SubmittedDate", "%w", true)
                            //},
                            //sorters: {
                            //    "Month": sortAs(["Jan", "Feb", "Mar", "Apr", "May",
                            //        "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]),
                            //    "Day": sortAs(["Mon", "Tue", "Wed", "Thu", "Fri",
                            //        "Sat", "Sun"])
                            //},

                            renderers: renderers,

                            rows: ["Answer"],
                            cols: ["QuestionName"],
                            rendererName: "Table",
                            rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                        });
                    }

                })


            },

            async SavePivotPreset() {

                var config = $("#output").data("pivotUIOptions");
                var config_copy = JSON.parse(JSON.stringify(config));
                //delete some values which will not serialize to JSON
                delete config_copy["aggregators"];
                delete config_copy["renderers"];
                //localStorage.setItem('pivotpreset', JSON.stringify(config_copy));

                var data = new FormData();
                data.append("PresetName", this.presetName);
                data.append("ActivityId", this.activityInfo.ActivityId);
                data.append("PresetData", JSON.stringify(config_copy));
                var result = await objManageProjectActivity.SaveOutPutPivotPreset(data);

                this.Msg = "Pivot table preset is saved successfully.";
                this.successSnackbarMsg = true;

            },

            async RestorePivotPreset() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("PresetName", this.presetSelected);
                data.append("ActivityId", this.activityInfo.ActivityId);
                var result = await objManageProjectActivity.RestoreOutPutPivotPreset(data);

                if (result !== null && result != undefined && result != '') {
                    var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                    url += "?url=getpivotdata?surveyid=" + this.outputSurveyIds + "&repeatenable=1";

                    var self = this;

                    //$.getJSON(url, function (tdata) {
                    //    var data = tdata;
                    //    self.pivotdata = tdata;
                    //    self.appendFilePath();
                    //    if (data != null) {
                    //        $("#output").pivotUI(data, result, true);
                    //    }
                    //})

                    $.getJSON(url, function (tdata) {
                        self.pivotdata = [];
                        self.pivotdata = tdata;
                        self.appendFilePath();

                    })
                    if (self.pivotdata != null) {
                        $("#output").pivotUI(self.pivotdata, result, true);
                    }
                }
                else {
                    this.Msg = "Preset does not exist.";
                    this.infoSnackbarMsg = true;
                }

                this.PreLoader = false;

            },

            async GetPivotPresetList() {
                var data = new FormData();
                data.append("ActivityId", this.activityInfo.ActivityId);
                var result = await objManageProjectActivity.GetOutputPivotPresetList(data);
                if (result !== null && result != undefined && result != '') {
                    this.presetLocalStorage = result;
                }

            },

            changeSelectedPivotValue: function (newData, oldIndex, axis) {
                if (axis == 'y') {
                    this.colsDialog[oldIndex] = newData
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.yAxisButton = '';
                } else if (axis == 'x') {
                    this.rowDialog[oldIndex] = newData
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.xAxisButton = '';
                } else if (axis == 'lastIndexX') {
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.rowDialog.push(this.nonSelectedData[this.nonSelectedData.length - 1])
                    this.nonSelectedData.pop()
                } else if (axis == 'lastIndexY') {
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.colsDialog.push(this.nonSelectedData[this.nonSelectedData.length - 1])
                    this.nonSelectedData.pop()
                }

            },

            nonSelectedPivotValue: function (index) {
                let totalSelected = this.rowDialog + this.colsDialog;
                this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton = index;
            },

            changeSelectedPivotValue_Edit: function (newData, oldIndex, axis) {
                if (axis == 'y') {
                    this.colsDialog_Edit[oldIndex] = newData;
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.yAxisButton_Edit = '';
                } else if (axis == 'x') {
                    this.rowDialog_Edit[oldIndex] = newData;
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.xAxisButton_Edit = '';
                } else if (axis == 'lastIndexX') {
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.rowDialog_Edit.push(this.nonSelectedData_Edit[this.nonSelectedData_Edit.length - 1]);
                    this.nonSelectedData_Edit.pop();
                } else if (axis == 'lastIndexY') {
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.colsDialog_Edit.push(this.nonSelectedData_Edit[this.nonSelectedData_Edit.length - 1]);
                    this.nonSelectedData_Edit.pop();
                }

            },

            nonSelectedPivotValue_Edit: function (index) {
                let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton_Edit = index;
            },

            Remove_nonSelectedPivotValue_Edit: function (index) {
                this.rowDialog_Edit = this.rowDialog_Edit.filter(e => e !== index);
                this.colsDialog_Edit = this.colsDialog_Edit.filter(e => e !== index);
                let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton_Edit = index;
            },

            onCopy: function (e) {
                this.infoSnackbarMsg = true;
                this.Msg = 'You just copied : ' + e.text;
            },
            addPresetDialogOpen: function () {
                this.rows = $("#output").data("pivotUIOptions").rows
                this.cols = $("#output").data("pivotUIOptions").cols
                let totalSelected = this.rows + this.cols;
                this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                this.colsDialog = [...this.cols];
                this.rowDialog = [...this.rows];
                this.presetStoreSave = false;
                this.addPresetDialog = true;
            },
            addPresetDialogCloseButton: function () {
                this.colsDialog = this.cols
                this.rowDialog = this.rows
                this.cols = [];
                this.rows = [];
                this.cols = [...this.colsDialog];
                this.rows = [...this.rowDialog];
                this.rules = [];
                this.presetName = '';
                this.addPresetDialog = false;
            },
            addPresetDialogSaveButton: async function () {
                this.PreLoader = true;

                this.cols = [];
                this.rows = [];
                this.cols = [...this.colsDialog];
                this.rows = [...this.rowDialog];

                //
                $("#output").data("pivotUIOptions").rows = this.rows;
                $("#output").data("pivotUIOptions").cols = this.cols;
                var config = $("#output").data("pivotUIOptions");
                var config_copy = JSON.parse(JSON.stringify(config));
                //delete some values which will not serialize to JSON
                delete config_copy["aggregators"];
                delete config_copy["renderers"];
                //localStorage.setItem('pivotpreset', JSON.stringify(config_copy));

                var data = new FormData();
                data.append("PresetName", this.presetName);
                data.append("ActivityId", this.activityInfo.ActivityId);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objManageProjectActivity.SaveOutPutPivotPreset(data);

                if (result) {
                    //this.presetLocalStorage.push(this.presetName);
                    this.Msg = "Pivot table preset is saved successfully.";
                    this.successSnackbarMsg = true;
                    this.GetPivotPresetList();
                }
                else {
                    this.Msg = "An unknown error has occured.";
                    this.infoSnackbarMsg = true;
                }
                //

                this.rules = [];
                this.presetName = '';
                this.addPresetDialog = false;
                this.PreLoader = false;
            },
            submitHandler() {
                this.rules = {
                    name: [(v) => !!v || "Preset Name is required"]
                };

                var isPresetExists = this.presetLocalStorage.includes(this.presetName);
                if (isPresetExists) {
                    this.Msg = "Preset name already exists.";
                    this.infoSnackbarMsg = true;
                    return false;
                }
                let self = this;
                setTimeout(() => {
                    if (self.$refs.entryForm.validate()) {
                        this.addPresetDialogSaveButton()
                    }
                });
            },

            //for edit preset
            editPresetDialogOpen: function () {
                //this.rows = $("#output").data("pivotUIOptions").rows
                //this.cols = $("#output").data("pivotUIOptions").cols


                //let totalSelected = this.rows + this.cols;
                //this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                //this.colsDialog = [...this.cols];
                //this.rowDialog = [...this.rows];
                //this.presetStoreSave = false;
                this.editPresetDialog = true;

                this.presetSelected_Edit = '';
                this.colsDialog_Edit = [];
                this.rowDialog_Edit = [];
                this.rows_Edit = [];
                this.cols_Edit = [];

            },

            GetPivotPresetData_Edit() {
                var selectedpreset_Edit = this.presetSelected_Edit;

                this.rows_Edit = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData).rows;
                this.cols_Edit = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData).cols;
                let totalSelected = this.rows_Edit + this.cols_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.colsDialog_Edit = [...this.cols_Edit];
                this.rowDialog_Edit = [...this.rows_Edit];
            },

            async submitEditHandler() {

                var selectedpreset_Edit = this.presetSelected_Edit;

                //var ExistingPresetData = this.presetLocalStorage.find(obj => {
                //    return obj.presetName === selectedpreset_Edit;
                //});
                var ExistingPresetData = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData);

                ExistingPresetData.rows = this.rowDialog_Edit;
                ExistingPresetData.cols = this.colsDialog_Edit;
                var config_copy = ExistingPresetData;
                var data = new FormData();
                data.append("PresetName", selectedpreset_Edit);
                data.append("ActivityId", this.activityInfo.ActivityId);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objManageProjectActivity.EditOutputPivotPreset(data);

                this.Msg = "Pivot table preset is saved successfully.";
                this.successSnackbarMsg = true;
                this.GetPivotPresetList();
                this.editPresetDialog = false;

            },

            //for Delete preset
            async submitDeleteHandler() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("PresetName", this.presetSelected_Delete);
                data.append("ActivityId", this.activityInfo.ActivityId);
                var result = await objManageProjectActivity.DeleteOutputPivotPreset(data);

                if (result) {
                    //var x = this.presetLocalStorage.filter(a => a.PresetName !== this.presetSelected_Delete);
                    //this.presetLocalStorage = x;
                    this.presetLocalStorage = [];
                    this.GetPivotPresetList();
                    this.clearDateSelection();
                    this.Msg = "Pivot table preset is deleted successfully.";
                    this.successSnackbarMsg = true;
                    this.deletePresetDialog = false;

                }
                else {
                    this.Msg = "Preset does not exist.";
                    this.infoSnackbarMsg = true;
                }

                this.PreLoader = false;
            },

            clearDateSelection() {
                this.submittedbystartDate = null;
                this.submittedbyendDate = null;
                this.submittedmStartDate = false;
                this.submittedmEndDate = false;
                this.presetSelected = '';
                this.repeatLogic = null
                $("#output").pivotUI([], {});
                this.lstRepeatSections = this.fnFilterPresets();
            },
            async submitpivottable() {
                this.PreLoader = true
                var renderers = $.extend($.pivotUtilities.renderers,
                    $.pivotUtilities.plotly_renderers);

                var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                url += "?url=getpivotdata?surveyid=" + this.outputSurveyIds + "&repeatenable=1";

                if(this.repeatLogic){
                    url += `&sectionid=${this.repeatLogic}`
                }

                var self = this;

                let response = await fetch(url)
                let data = await response.json()
                self.pivotdata = data
                self.appendFilePath()

                var temparray = [];
                for (var i = 0; i < data.length; i++) {
                    temparray.push(...Object.keys(data[i]));
                }
                let arr2 = [...new Set(temparray.flat(1))];
                self.attributes = arr2;
                self.nonSelectedData = arr2;
                if(self.submittedbystartDate && self.submittedbyendDate){
                    var newArray = data.filter(function (el) {
                        return objUtils.common.formatExcelDate(el.SubmittedDate) <= self.submittedbyendDate &&
                            objUtils.common.formatExcelDate(el.SubmittedDate) >= self.submittedbystartDate;
                    });
                    if (newArray != null) {
                        $("#output").pivotUI(newArray, {

                            renderers: renderers,
                            rows: ["Answer"],
                            cols: ["QuestionName"],
                            rendererName: "Table",
                            rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                        });
                    }
                }
                else if (data.length) {
                    $("#output").pivotUI(data, {
                        renderers: renderers,
                        rows: ["Answer"],
                        cols: ["QuestionName"],
                        rendererName: "Table",
                        rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                    });
                }
                this.lstRepeatSections = this.fnFilterPresets()
                this.PreLoader = false
            },
            async appendFilePath() {
                var surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.projectInfo.ProjectId.toString() + "/";
                var beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.projectInfo.ProjectId.toString() + "/";

                if (this.pivotdata != null && this.pivotdata != undefined) {
                    for (var i = 0; i < this.pivotdata.length; i++) {
                        for (let key in this.pivotdata[i]) {
                            if (this.pivotdata[i].hasOwnProperty(key)) {
                                var allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg', 'zip', 'rar'];
                                if (this.pivotdata[i][key] != null && this.pivotdata[i][key] != undefined && this.pivotdata[i][key] != 0 && typeof this.pivotdata[i][key] === 'string') {
                                    let filextension = this.pivotdata[i][key].split('.').pop();
                                    if (allowedExtensions.indexOf(filextension) >= 0) {
                                        //var data = new FormData();
                                        //data.append("FileName", this.pivotdata[i][key]);
                                        //data.append("ProjectID", this.ProjectID);
                                        //console.log('key :' + key);
                                        //console.log('i :' + i);
                                        //console.log('this.pivotdata[i][key] :' + this.pivotdata[i][key]);
                                        //console.log('ProjectID :' + this.ProjectID);
                                        //var isFileExists = await objSurvey.checkFileExists(data);
                                        //console.log('isFileExists :' + isFileExists);
                                        //switch (isFileExists) {
                                        //    case 0: // file not exists
                                        //        this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                                        //        break;
                                        //    case 1:
                                        //        this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                                        //        console.log('this.pivotdata[i][key] in switch :' + this.pivotdata[i][key]);
                                        //        break;
                                        //    case 2:
                                        //        this.pivotdata[i][key] = beneficiaryfilesURL + this.pivotdata[i][key];
                                        //        break;
                                        //    default: // file not exists

                                        //}

                                        //temporary

                                        var questionname = key;

                                        if (questionname.indexOf('(Beneficiary)') == -1) {
                                            this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                                        }

                                        if (questionname.indexOf('(Beneficiary)') >= 0) {
                                            this.pivotdata[i][key] = beneficiaryfilesURL + this.pivotdata[i][key];
                                        }

                                        //var surveyfilePath = surveyfilesURL + this.pivotdata[i][key];
                                        //var beneficiaryfilePath = beneficiaryfilesURL + this.pivotdata[i][key];
                                        //if (this.UrlExists(surveyfilePath))
                                        //{
                                        //    this.pivotdata[i][key] = surveyfilePath;
                                        //}
                                        //else if (this.UrlExists(beneficiaryfilePath)) {
                                        //    this.pivotdata[i][key] = beneficiaryfilePath;
                                        //}



                                        //temporary ends
                                    }

                                    if (objUtils.common.checkIfValidlatitudeAndlongitude(this.pivotdata[i][key])) {
                                        if (this.pivotdata[i][key] != '0,0') {
                                            this.pivotdata[i][key] = objUtils.GPS_TARGET_URL + this.pivotdata[i][key] + "?hl=en";
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            },
            async ExportToExcel(type, fn, dl) {
                var elt = document.querySelector("#output .pvtTable")
                var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
                return dl ?
                    XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
                    XLSX.writeFile(wb, fn || ('Pivot_Table.' + (type || 'xlsx')));
            },
            async GetOutputRepeatSection(){
                let { data } = await objSurvey.GetSurveyRepeatSection(this.outputSurveyIds)
                this.lstRepeatLogic = data
            },
            async fnMountPivotTab() {
                this.clearDateSelection();
                await this.fnGetOutputSurveyIds();
                await this.GetPivotPresetList();
                await this.GetOutputRepeatSection();
                $("#output").pivotUI([], {});
                await this.getpivottable()
                this.lstRepeatSections = this.fnFilterPresets()
            },
            fnFilterPresets() {
                if(this.repeatLogic){
                    return this.presetLocalStorage.filter((x) => x.sectionId == this.repeatLogic)
                }
                return this.presetLocalStorage.filter((y) => y.sectionId === 0)
            }
        },
        components: {
            'Add-Button-Fab': AddButtonFab,
            'Success-Button-Md': SuccessButtonMd,
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Primary-Button': PrimaryButton,
            'Bulk-Action-Button-Outlined': BulkActionButtonOutlined,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
        },
        watch: {
            /// <summary>
            ///  SearchExpenseTag :- Function used to search activity expense tags based on TagName (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.SearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.SearchExpenseTag));
            },

            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.includes(this.SearchTagDetails));
            },
        }

    }
</script>

<style>
    .v-expansion-panel-content__wrap {
        padding: 0 !important;
    }

    .table-responsive {
        display: block;
        width: 100%;
        overflow-x: auto;
    }
</style>
<style>

    .pvtUnused.pvtVertList {
        max-width: 500px;
        display: inline-block;
        overflow: scroll;
    }

    .pivot_table_container {
        overflow-x: scroll;
        border: 1px solid rgb(162, 177, 198);
        border-radius: 5px;
    }
</style>