<template>
    <v-card>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-card-title style="height:73px" v-show="KPIDetailsTableItems.length>0">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                KPI
            </h5>
            <v-spacer></v-spacer>
            <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="39" v-on="on" color="success-btn" dark class="elevation-0 mr-4">
                        Create KPI
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                    <v-list-item id="AutoTest_IP552" @click.stop="CreateKPIDialog=true">
                        <v-list-item-icon>
                            <v-icon>mdi-plus-circle</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Create KPI
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item id="AutoTest_IP553" @click.stop="ImportKPIDialogBox = true">
                        <v-list-item-icon>
                            <v-icon>mdi-file-account-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Import KPI
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>
            <!--Bulk Action -->
            <v-menu offset-y transition="scroll-y-transition"  :close-on-content-click="false" v-model="closeMenu" v-bind:disabled ="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                    <v-list-item id="AutoTest_IP554" @click.stop="fnBulkRemoveClick()">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Remove
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
                <!-- Remove KPI-->
                <v-dialog v-model="RemoveBulkKPIDialog" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                            </v-avatar>
                            Remove KPI
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_IP555" text="" @click="RemoveBulkKPIDialog=false">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-card-text class="pt-0">
                            <v-container fluid class="pt-4">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <v-container fluid class="pt-0">
                                            <v-row>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <h4 class="text-center heading-3">Are you sure you want to remove this KPIs ?</h4>
                                                </v-col>

                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                    <v-btn id="AutoTest_IP556" color="secondary elevation-0" class="mr-2" @click="fnDeleteKPI(1),RemoveBulkKPIDialog=false">Remove</v-btn>
                                                    <v-btn id="AutoTest_IP557" outlined color="grey" @click="RemoveBulkKPIDialog=false,selectedKPI=[]">Cancel</v-btn>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                    </v-card>
                </v-dialog>
            </v-menu>
            <v-text-field label="Search" class="mr-4 table-search" v-model="SearchKPIDetails"
                          dense prepend-inner-icon="mdi-magnify"
                          outlined hide-details></v-text-field>
        </v-card-title>

        <v-row v-if="KPIDetailsTableItems.length>0">
            <!--KPI Table-->
            <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
                <v-data-table :headers="KPIDetailsTableheaders"
                              :items="KPIDetailsTableItems"
                              :search="SearchKPIDetails"
                              show-select
                              item-key="KPIId"
                              v-model="selectedKPI"
                              class="elevation-0">
                    <!-- DateTime-->
                    <template v-slot:item.StartDate="{ item }">
                        <span>{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</span>
                    </template>
                    <template v-slot:item.EndDate="{ item }">
                        <span>{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</span>
                    </template>
                    <!-- KPI Name-->
                    <template v-slot:item.KPIName="{item}">
                        <v-list class="py-0 table-v-list">
                          <!--if path is ManageProjectKPI-->
                            <v-list-item v-if="$route.name=='ManageProjectKPI'" class="my-0 px-0" link :to="{name:'ManageProjectKPIActivityLog',query: {  WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Edit'}}">
                                <v-list-item-icon>
                                    <v-avatar size="30" color="#fff" class="elevation-1">
                                        <v-icon size="20" color="primary">mdi-book</v-icon>
                                    </v-avatar>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    {{item.KPIName}}
                                </v-list-item-content>
                            </v-list-item>
                          <!--____________________-->
                          <!--if path is MyprojectsDraftIndividualKPI-->
                          <v-list-item v-if="$route.name=='MyprojectsDraftIndividualKPI'" class="my-0 px-0" link="" :to="{name:'MyprojectsDraftIndividualKPIActivityLog',query: {  WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Edit'}}">
                            <v-list-item-icon>
                              <v-avatar size="30" color="#fff" class="elevation-1">
                                <v-icon size="20" color="primary">mdi-book</v-icon>
                              </v-avatar>
                            </v-list-item-icon>
                            <v-list-item-content>
                              {{item.KPIName}}
                            </v-list-item-content>
                          </v-list-item>
                          <!--____________________-->
                        </v-list>
                    </template>

                    <!-- Users-->
                    <template v-slot:item.UserList="{item}">
                        <!--<div v-for="(i,index) in item.UserList" class="d-inline-flex">-->
                        <!--<v-avatar color="#0CBA2F" size="25" class="mr-1">
                        <span>{{i.Name.charAt(0)}}</span>
                    </v-avatar>-->
                        <!--<v-avatar dark color="#0CBA2F" size="25" class="mr-1" v-if="index >= 0">
                        <span>{{item.UserList[index].Name.charAt(0)}}</span>
                    </v-avatar>
                    <v-avatar color="grey" size="25" class="mr-1" v-if="index > 1">
                        <span>+{{item.UserList.length-2}}</span>
                    </v-avatar>-->
                        <!--</div>-->
                        <div v-if=" item.UserList.length >0">
                            <v-avatar color="#0CBA2F" size="25" class="mr-1" v-if=" item.UserList.length >=1 && item.UserList[0].Name.length >0">
                                <span class="text--white">{{item.UserList[0].Name.charAt(0)}}</span>
                            </v-avatar>
                            <v-avatar color="#0CBA2F" size="25" class="mr-1" v-if="item.UserList.length >=2">
                                <span class="text--white">{{item.UserList[1].Name.charAt(0)}}</span>
                            </v-avatar>

                            <v-avatar size="25" class="mr-1" color="grey" v-if="item.UserList.length > 2">
                                <span class="text--white"> +{{item.UserList.length -2}}</span>
                            </v-avatar>
                        </div>
                    </template>

                    <!-- Row Actions-->
                    <template v-slot:item.Actions="{item}">
                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled ="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                    <v-icon>mdi-dots-vertical</v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light width="auto" class="action-button-list">
                                <!-- Edit-->
                             <!--if path is ManageProjectKPI-->
                                <v-list-item v-if="$route.name=='ManageProjectKPI'" link :to="{name:'ManageProjectKPIActivityLog',query: { WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Edit'}}">
                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Edit</v-list-item-content>
                                </v-list-item>
                              <!--___________________-->
                              <!--if path is MyprojectsDraftIndividualKPI-->
                              <v-list-item v-if="$route.name=='MyprojectsDraftIndividualKPI'" link="" :to="{name:'MyprojectsDraftIndividualKPIActivityLog',query: { WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Edit'}}">
                                <v-list-item-icon>
                                  <v-icon>mdi-pencil-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>Edit</v-list-item-content>
                              </v-list-item>
                              <!--___________________-->
                                <!-- Assign Workflow-->
                                <v-list-item id="AutoTest_IP558" link @click.stop="KPIId=item.KPIId,fnGetWorkflowsListByProjectID(item),SelectWorkflowDialog = true">
                                    <v-list-item-icon><v-icon>mdi-timeline-text-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Assign Workflow</v-list-item-content>
                                </v-list-item>
                                <!-- Assign User-->
                                <v-list-item id="AutoTest_IP559" link @click.stop="AssignUserDialog = true,KPIId=item.KPIId,fnGetKPIByKPIId(KPIId=item.KPIId)">
                                    <v-list-item-icon><v-icon>mdi-account-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Assign User</v-list-item-content>
                                </v-list-item>
                                <!-- View Activity Log-->
                              <!--if path is ManageProjectKPI-->
                                <v-list-item v-show="$route.name=='ManageProjectKPI'" link :to="{name:'ManageProjectKPIActivityLog',query: { WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Log'}}">
                                    <v-list-item-icon><v-icon>mdi-eye-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>View Activity Log</v-list-item-content>
                                </v-list-item>
                              <!--______________________-->
                              <!--if path is MyprojectsDraftIndividualKPI-->
                              <v-list-item v-show="$route.name=='MyprojectsDraftIndividualKPI'" link="" :to="{name:'MyprojectsDraftIndividualKPIActivityLog',query: { WsID: WorkspaceID, Access:RWAccess,KPIID:item.KPIId,ProjID:ProjectId,InitID:InitiativeId, Type :'Log'}}">
                                <v-list-item-icon>
                                  <v-icon>mdi-eye-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>View Activity Log</v-list-item-content>
                              </v-list-item>
                              <!--______________________-->
                                <!-- Remove-->
                                <v-list-item id="AutoTest_IP560" link @click.stop="RemoveKPIDialog = true,KPIId=item.KPIId,KPIName=item.KPIName">
                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Remove</v-list-item-content>
                                </v-list-item>
                            </v-list>
                        </v-menu>
                    </template>
                </v-data-table>
            </v-col>
        </v-row>

        <!-- if No Data-->
        <v-row v-else>
            <v-col class="text-center">
                <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                <h4 class="title text-center" style="color:#585858">No KPIs available</h4>
                <!--<Success-Button class="mt-4" style="width:200px" title="Create KPI" @click.native="CreateKPIDialog=true"></Success-Button>-->
                <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn height="38" v-on="on" color="success-btn" dark class="elevation-0 mr-4">
                            Create KPI
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_IP561" @click.stop="CreateKPIDialog=true">
                            <v-list-item-icon>
                                <v-icon>mdi-plus-circle</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                Create KPI
                            </v-list-item-content>
                        </v-list-item>
                        <v-list-item id="AutoTest_IP562" @click.stop="ImportKPIDialogBox = true">
                            <v-list-item-icon>
                                <v-icon>mdi-file-account-outline</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                Import KPI
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-menu>
            </v-col>
        </v-row>


        <!--Add KPI Dialog-->
        <v-dialog v-model="CreateKPIDialog" width="800" persistent>
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Create KPI
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP563" text="" @click="CloseKPIDialog()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="form">
                    <v-card-text>
                        <h4 class="text-center heading-3 mt-4">Please enter the basic details to create a new KPI.</h4>
                        <v-row justify="center">
                            <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                <v-text-field outlined="" dense="" label="KPI Name" v-model="KPIName" :rules="[rules.required]">
                                </v-text-field>
                                <v-textarea outlined="" dense="" name="" label="KPI Description" value="" hide-details v-model="Description" :rules="[rules.required]"></v-textarea>
                                <!-- Date Between-->
                                <v-row>
                                    <!-- From Date -->
                                    <v-col>
                                        <v-menu v-model="FrmDateMenu"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y
                                                max-width="290px"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="FrmDate"
                                                              label="Start Date"
                                                              persistent-hint outlined hide-details
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="startDate" no-title @input="FrmDateMenu = false,IsValidDate()" format="DD-MM-YYYY"
                                                           dense :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                        </v-menu>
                                    </v-col>
                                    <!-- To Date -->
                                    <v-col>
                                        <v-menu ref="ToDateMenu"
                                                v-model="ToDateMenu"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y
                                                max-width="290px"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="ToDate"
                                                              label="End Date"
                                                              persistent-hint outlined hide-details
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="endDate" no-title @input="ToDateMenu = false,IsValidDate()" format="DD-MM-YYYY" dense
                                                           :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                        </v-menu>
                                    </v-col>
                                </v-row>

                                <v-select hide-details dense="" :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="ReportingFrequency" :rules="[rules.required]">
                                </v-select>
                                <!-- Once -->
                                <div v-show="ReportingFrequency === 2">
                                    <v-menu v-model="periodicityDateMenu"
                                            :close-on-content-click="false"
                                            transition="scale-transition"
                                            offset-y
                                            max-width="290px"
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field v-model="FormattedDate"
                                                          label="Choose a Date"
                                                          persistent-hint outlined
                                                          v-on="on" class="mt-4" hide-details></v-text-field>
                                        </template>
                                        <v-date-picker v-model="periodicityDate" no-title @input="periodicityDateMenu = false,IsValidDate(),IsValidPeriodicityDate()"
                                                       dense :min="startDate" :max="endDate"></v-date-picker>
                                    </v-menu>
                                </div>
                                <!-- Weekly-->
                                <div v-show="ReportingFrequency === 4" class="mt-4">
                                    <v-select dense="" :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="ReportingWeek" hide-details>
                                    </v-select>
                                </div>
                                <!-- Monthly-->
                                <div v-show="ReportingFrequency === 5" class="mt-4">
                                    <v-select dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDay" hide-details>
                                    </v-select>
                                </div>
                                <!-- Quaterly , Halyearly and Yearly-->
                                <div class="d-inline-flex mt-4" v-if="ReportingFrequency == 6|| ReportingFrequency == 7 ||  ReportingFrequency == 8">
                                    <v-select hide-details class="mr-1 mb-4" dense="" :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="ReportingMonth">
                                    </v-select>
                                    <v-select class="mb-4" hide-details dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDateDay">
                                    </v-select>
                                </div>

                                <v-select class="mt-4" dense="" :items="workflows" :item-text="i => i.WfName" :item-value="i => i.WorkflowID" label="Select Workflow" outlined="" v-model="Workflow">
                                </v-select>
                                <v-select dense="" :items="Surveys" :item-text="i => i.SurveyName" :item-value="i => i.SurveyID" label="Select Survey" outlined="" v-model="Survey" :rules="[rules.required]">
                                </v-select>
                                <span class="small-text mr-1">
                                    <v-icon size="15">mdi-account</v-icon> Users
                                </span>
                                <v-avatar size="27" class="team-avatar" v-if="selectedteamUsers == 0">
                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="selectedteamUsers.length > 0">
                                    <span>{{selectedteamUsers[0].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="selectedteamUsers.length > 1">
                                    <span>{{selectedteamUsers[1].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="grey" v-if="selectedteamUsers.length > 2">
                                    <span>+{{selectedteamUsers.length-2}}</span>
                                </v-avatar>
                                <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP564" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                               @click="">
                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Assign Account User pop up-->
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Add People to Notify
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP565" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AssignAccountUserPopUp=false">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:150px" class="v-scrolling-div">
                                            <v-data-table :headers="lstuserTableheaders"
                                                          :items="lstUsers"
                                                          show-select
                                                          item-key="UserID"
                                                          sort-by="Name"
                                                          v-model="selectedteamUsers">



                                            </v-data-table>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row no-gutters="">
                                                <v-col md="12" class="py-0" align="center">
                                                    <Success-Button id="AutoTest_IP566" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignAccountUserPopUp=false"></Success-Button>
                                                </v-col>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                    <!--_______________________________________-->
                                </v-menu>

                                <!-- Tags-->
                                <v-row class="mb-1">
                                    <v-col class="text-left d-inline-flex" md="12">
                                        <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                        <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                            <template v-slot:activator="{ on }">
                                                <v-btn id="AutoTest_IP567" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                       @click="fnInitializeSteper()" class="mr-1">
                                                    <v-icon size="15">mdi-plus</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-card>
                                                <v-card-text class="pt-0 pb-1">
                                                    <v-col cols="12" md="12">
                                                        <!-- Header Text  -->
                                                        <v-col cols="12" md="12" class="d-inline-flex">
                                                            <v-btn id="AutoTest_IP568" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                <v-icon size="15">mdi-chevron-left</v-icon>
                                                            </v-btn>
                                                            <v-spacer></v-spacer>
                                                            <h3 class="text-left page-head pt-2">
                                                                {{TagHeaderName}}
                                                            </h3>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP569" text @click="NewTagMenu = false">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                        <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                            <v-stepper-items>

                                                                <!-- Create New Tag Option when there is no Tags are -->
                                                                <v-stepper-content step="1" class="pa-0">
                                                                    <v-row>
                                                                        <v-col class="pb-1 ml-2">
                                                                            <v-text-field outlined type="text" dense label="Enter Tag Name"
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
                                                                            <v-btn id="AutoTest_IP570" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-stepper-content>

                                                                <!-- Select from existing Tags-->
                                                                <v-stepper-content step="2" class="pa-0">

                                                                    <v-row style="max-height:30vh;overflow-y:scroll">
                                                                        <v-col col="12" md="12" class="mb-0 pt-0">
                                                                            <p class="mb-2">Search Tags</p>
                                                                            <v-text-field outlined="" dense="" label="Search tag here" hide-details class="mb-3" v-model="SearchTagDetails">
                                                                            </v-text-field>
                                                                            <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                                                                                <v-chip id="AutoTest_IP571" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                        label @click.native="fnAddToSelectedTag(index)">
                                                                                    {{ index.TagName }}
                                                                                    <v-icon id="AutoTest_IP572" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                </v-chip>
                                                                            </v-chip-group>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-col cols="12" md="12" class="text-center mt-3">
                                                                        <v-btn id="AutoTest_IP573" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                            <v-btn id="AutoTest_IP574" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-stepper-content>

                                                            </v-stepper-items>
                                                        </v-stepper>
                                                    </v-col>
                                                </v-card-text>
                                            </v-card>
                                        </v-menu>
                                        <div v-if="!allTags">
                                            <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                           
                                            <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                                            <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                                            <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
                                        </div>
                                        <v-btn id="AutoTest_IP575" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                        </v-btn>
                                        <v-btn id="AutoTest_IP576" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                        </v-btn>
                                    </v-col>
                                </v-row>
                                <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                    <v-col md="12" class="chip-container mb-0 pt-0">
                                        <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                            <v-chip id="AutoTest_IP577" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                {{TagItem.TagName}}
                                            </v-chip>
                                        </v-chip-group>
                                    </v-col>
                                </v-row>
                            </v-col>

                        </v-row>
                    </v-card-text>
                </v-form>
                <v-card-actions class="pb-4">
                    <v-col class="text-right">
                        <Secondary-Button id="AutoTest_IP578" class="btn-122x36 mr-2" title="Confirm" @click.native="fnCreateKPI()" v-bind:disabled="fnCheckMandatory()"></Secondary-Button>
                        <Default-Button-Outlined id="AutoTest_IP579" class="btn-122x36" title="Cancel" @click.native="CloseKPIDialog()"></Default-Button-Outlined>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="KPISaveSuccessSnackbar" top="" color="primary">
            KPI details saved successfully
        </v-snackbar>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>

        <!-- Assign WorkflowKPI
    <!-- Select Work flow-->
        <v-dialog v-model="SelectWorkflowDialog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-hexagon-multiple-outline</v-icon>
                    </v-avatar>Add Workflow
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP580" text @click="SelectWorkflowDialog=false,checkboxAddWorkflow=[],selectedAddWorkflow=[]">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="py-0">
                    <v-container fluid class="pt-0">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-stepper v-model="SelectWorkflowDlgSteps" class="elevation-0">
                                    <v-stepper-items>
                                        <!-- Select Workflow step-->
                                        <v-stepper-content step="1">
                                            <v-row no-gutters>
                                                <v-col v-if="SelectWorkflowDlgItems.length > 0" cols="12" md="12" class="pt-0 mb-2">
                                                    <h4 class="text-center heading-3">Please select workflow from below</h4>
                                                </v-col>
                                            </v-row>
                                            <!-- if workflow available -->
                                            <v-row no-gutters v-if="SelectWorkflowDlgItems.length > 0">
                                                <!-- Work flow group chip-->
                                                <v-col md="12" class="py-0">
                                                    <div class="chip-container mb-2">
                                                        <v-chip-group multiple column active-class="primary--text">
                                                            <v-chip id="AutoTest_IP581" v-for="(chipitem,key) in selectedAddWorkflow" :key="chipitem.WFitem.WorkflowID"
                                                                    @click:close="fnRemove(chipitem)" close class="ma-2" small>
                                                                <v-icon left size="15" color="#707070">mdi-folder-key-network-outline</v-icon>
                                                                {{ chipitem.WFitem.WfName }}
                                                            </v-chip>
                                                        </v-chip-group>
                                                    </div>
                                                </v-col>

                                                <!-- Work flow details card-->
                                                <v-row class="v-scrolling-div mx-1" style="height:45vh">
                                                    <v-col xs="6" sm="6" md="6" v-for="(addWorkflowItem,index) in SelectWorkflowDlgItems" :key="addWorkflowItem.WorkflowID">
                                                        <v-card class="workspace-card" flat>
                                                            <v-row>
                                                                <v-col xs="6" sm="6" md="10" class="d-inline-flex pt-0" :ripple="false">
                                                                    <div>
                                                                        <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                            <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                                        </v-avatar>
                                                                    </div>
                                                                    <div class="title pt-4 px-2 text-limit">
                                                                        {{ addWorkflowItem.WfName }}
                                                                    </div>
                                                                </v-col>
                                                                <v-col xs="2" sm="2" md="2" class="d-inline-flex pt-0 text-right">
                                                                    <v-checkbox v-model="checkboxAddWorkflow[index]"
                                                                                @change="fnCheck(addWorkflowItem,index)" label color="success-check-box"></v-checkbox>
                                                                </v-col>
                                                            </v-row>
                                                            <!--Description-->
                                                            <!--Description-->
                                                            <v-row>
                                                                <v-col class="py-0" :ripple="false">
                                                                    <span class="title small-text">
                                                                        <v-icon size="12">mdi-folder-key-network-outline</v-icon>
                                                                        Description
                                                                    </span>
                                                                    <p style="height:100px" class="ml-3 v-scrolling-div">{{ addWorkflowItem.WfDescription }}</p>
                                                                </v-col>
                                                            </v-row>
                                                            <!-- Levels-->
                                                            <v-row>
                                                                <v-col class="py-0">
                                                                    <span class="small-text">
                                                                        <v-icon size="12">mdi-tag</v-icon>
                                                                        Level : {{ addWorkflowItem.WfNoOfLevels }}
                                                                    </span>
                                                                </v-col>
                                                            </v-row>
                                                            <!-- buttons-->
                                                            <v-row>
                                                                <v-col>
                                                                    <v-btn id="AutoTest_IP582" color="success-btn elevation-0 enter-btn" dark @click="SelectWorkflowDlgSteps=2,ViewWorkflowItem=addWorkflowItem">
                                                                        Show Details
                                                                    </v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <!--Buttons-->
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4">
                                                    <Secondary-Button id="AutoTest_IP583" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddSelectWorkflow()" v-bind:disabled="selectedAddWorkflow.length==0"></Secondary-Button>
                                                    <Default-Button-Outlined id="AutoTest_IP584" class="btn-122x36" title="Cancel" @click.native="SelectWorkflowDialog=false,checkboxAddWorkflow=[],selectedAddWorkflow=[]"></Default-Button-Outlined>
                                                </v-col>
                                            </v-row>
                                            <!-- if no workflow available -->
                                            <v-row v-else no-gutter>
                                                <v-col cols="12" sm="12" md="12">
                                                    <v-row :justify="justifyCenter">
                                                        <v-col md="6" class="text-center">
                                                            <v-list class="base-border-radius warning-div pa-0">
                                                                <v-list-item>
                                                                    <v-list-item-icon class="mt-2">
                                                                        <v-avatar size="27" color="red" class="elevation-0">
                                                                            <v-icon size="20" color="white">mdi-exclamation</v-icon>
                                                                        </v-avatar>
                                                                    </v-list-item-icon>
                                                                    <v-list-item-content class="pb-0">
                                                                        <p class="pa-0">
                                                                            <span class="message">
                                                                                Sorry could not find any workflow.
                                                                            </span>
                                                                        </p>
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                            </v-list>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>

                                        <!-- Show Details of Workflow-->
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-container fluid class="pt-0">
                                                <v-col>
                                                    <h3 class="text-left page-head">
                                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                        </v-avatar><span id="AutoTest_IP585" style="cursor:pointer" @click="SelectWorkflowDlgSteps=1">Workflow</span>
                                                        <v-icon size="20">mdi-chevron-right</v-icon>
                                                        <span>Workflow Details</span>
                                                        <v-icon size="20">mdi-chevron-right</v-icon>{{ ViewWorkflowItem.WfName }}
                                                    </h3>
                                                </v-col>
                                                <!-- Work flow group chip-->
                                                <v-col md="12" class="py-0">
                                                    <div class="chip-container mb-2">
                                                        <v-chip-group multiple column active-class="primary--text">
                                                            <v-chip id="AutoTest_IP586" v-for="(chipitem,key) in selectedAddWorkflow" :key="chipitem.WFitem.WorkflowID"
                                                                    @click:close="fnRemove(chipitem)" close class="ma-2" small>
                                                                <v-icon left size="15" color="#707070">mdi-folder-key-network-outline</v-icon>
                                                                {{ chipitem.WFitem.WfName }}
                                                            </v-chip>
                                                        </v-chip-group>
                                                    </div>
                                                </v-col>
                                                <v-row class="v-scrolling-div" style="height:45vh" justify="center">
                                                    <v-col md="8" v-for="i in ViewWorkflowItem.WfNoOfLevels" :key="i">
                                                        <v-card class="workspace-card  py-0" flat>
                                                            <v-row>
                                                                <v-row :justify="justifyCenter">
                                                                    <v-col cols="6" md="6" class="text-center py-8">
                                                                        <v-progress-circular :rotate="360"
                                                                                             :size="100"
                                                                                             :width="15"
                                                                                             :value="(100/ViewWorkflowItem.WfNoOfLevels)*i"
                                                                                             color="#1EC2ED">
                                                                            <span class="subtitle-1 text--primary">
                                                                                {{i}}
                                                                                <br>
                                                                                <span class="subtitle-2 text--disabled">Level</span>
                                                                            </span>
                                                                        </v-progress-circular>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-divider vertical></v-divider>
                                                                <v-col cols="6" md="6" class="text-left">
                                                                    <!---   if there is no data-->
                                                                    <v-row v-if="ViewWorkflowItem.WfLevelDetails.length==0 ||
                                                                                         ViewWorkflowItem.WfLevelDetails[i-1].usersList.length==0" no-gutters>
                                                                        <v-col cols="12" md="12" class="pt-2 mb-0 text-center">
                                                                            <p class="heading-1"> No Users Added</p>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <!-- if User Assigned-->
                                                                    <v-row v-else no-gutters>
                                                                        <v-col v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>0" cols="12" md="12">
                                                                            <v-col class="text-center mb-0 pt-1">
                                                                                <v-avatar v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>=1" size="30" class="team-avatar">
                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                </v-avatar>
                                                                                <v-avatar v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>=2" size="30" class="team-avatar">
                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                </v-avatar>
                                                                                <v-avatar v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>2" size="30" class="team-count" color="red">
                                                                                    +{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList.length }}
                                                                                </v-avatar>
                                                                            </v-col>
                                                                            <v-col class="text-center mb-0 pt-0">
                                                                                <p v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length==1">{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList[0].Name }}</p>
                                                                                <p v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length==2">{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList[0].Name }},{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList[1].Name }}</p>
                                                                                <p v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>2">{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList[0].Name }},{{ ViewWorkflowItem.WfLevelDetails[i-1].usersList[1].Name }} + {{ ViewWorkflowItem.WfLevelDetails[i-1].usersList.length }}</p>
                                                                            </v-col>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                    <v-btn id="AutoTest_IP587" color="secondary" class="mr-2 elevation-0" @click="fnAddSelectWorkflow()">Confirm</v-btn>
                                                    <v-btn id="AutoTest_IP588" outlined color="grey" @click="SelectWorkflowDlgSteps=1">Previous</v-btn>
                                                </v-col>
                                            </v-container>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
                <v-divider></v-divider>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="AssignWorkflowSuccessSnackbar" top="" color="primary">
            Workflow(s) successfully assigned to KPI
        </v-snackbar>

        <!-- Assign Users-->
        <!-- Select User -->
        <v-dialog v-model="AssignUserDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                    </v-avatar>Add Users
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP589" text @click="AssignUserDialog=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <v-row no-gutters>
                        <v-col cols="12" md="12" class="pt-0">
                            <v-container fluid class="pt-2 px-1">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="text-right pt-5">
                                        <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUser">
                                        </v-text-field>
                                    </v-col>

                                    <div class="chip-container mb-2" v-if="selectedteamUsers.length>0">
                                        <v-chip-group multiple column active-class="primary--text">
                                            <v-chip id="AutoTest_IP590" close v-for="item in selectedteamUsers" :key="item.ID"
                                                    v-model="chipUsrItem" @click:close="RemoveUser(item)" class="ma-2" small>
                                                <v-icon left color="#707070">mdi-account</v-icon>
                                                {{ item.Name }}
                                            </v-chip>
                                        </v-chip-group>

                                    </div>
                                    <!--v-chip for Assign roles ends here -->
                                    <v-col cols="12" xs="12" sm="12" md="12">
                                        <!--User List with checkbox add here-->
                                        <v-data-table :headers="lstuserTableheaders"
                                                      :items="lstUsers"
                                                      show-select
                                                      item-key="UserID"
                                                      sort-by="Name"
                                                      fixed-header
                                                      height="25vh"
                                                      :search="searchUser"
                                                      v-model="selectedteamUsers"
                                                      class="elevation-0">
                                        </v-data-table>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-left" v-if="selectedteamUsers.length>0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="text-right">
                                                <!-- <v-checkbox input-value="true" label="Notify user" class="float-right"></v-checkbox> -->
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                        <v-btn id="AutoTest_IP591" v-bind:disabled="selectedteamUsers.length==0" color="secondary" class="mr-2 elevation-0" @click="fnAssignUserstoKPI(),AssignUserDialog = false, AssignUsersSuccessSnackbar = true">Save</v-btn>
                                        <v-btn id="AutoTest_IP592" outlined color="grey" @click="AssignUserDialog=false">Cancel</v-btn>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-col>
                    </v-row>
                </v-card-text>

                <v-divider></v-divider>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="AssignUsersSuccessSnackbar" top="" color="primary">
            User(s) successfully assigned to KPI
        </v-snackbar>

        <!-- Remove KPI-->
        <v-dialog v-model="RemoveKPIDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove KPI
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP593" text="" @click="RemoveKPIDialog=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-4">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this KPI ?</h4>
                                        </v-col>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <v-row justify="center">
                                                <h5 class="page-head mt-4">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                    </v-avatar>
                                                    {{KPIName}}
                                                </h5>
                                            </v-row>

                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP594" color="secondary elevation-0" class="mr-2" @click="fnDeleteKPI(2)">Remove</v-btn>
                                            <v-btn id="AutoTest_IP595" outlined color="grey" @click="RemoveKPIDialog=false,KPIName='',selectedKPI=[]">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="KPIRemoveSuccessSnackbar" top="" color="primary">
            KPI Successfully removed
        </v-snackbar>

        <!-- Import User -->
        <v-dialog v-model="ImportKPIDialogBox" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-account</v-icon>
                    </v-avatar>Import KPI
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP596" text="" @click="ImportKPIDialogBox = false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>

                <v-card-text class="pt-0">
                    <v-container fluid="" class="pt-0">
                        <v-row>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-stepper v-model="ImportKPISteps" class="elevation-0">
                                    <v-stepper-items>
                                        <v-stepper-content step="1" class="pt-0">
                                            <v-container fluid class="pt-0 pb-0">
                                                <v-row no-gutters="">
                                                    <v-col cols="12" md="12">
                                                        <h4 class="text-center heading-3">Step1 : Upload your CSV file</h4>
                                                    </v-col>
                                                    <v-col cols="12" md="12" class="text-center">
                                                        <v-card class="elevation-0">
                                                            <v-card-text class="py-0">
                                                                <v-row no-gutters>
                                                                    <v-col>
                                                                        <v-avatar size="100" tile>
                                                                            <img src="../../images/Import-icon.svg" alt="Import" />
                                                                        </v-avatar>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row no-gutters>
                                                                    <v-col>
                                                                        <p class="text-medium">
                                                                            Your CSV file must map all our mandatory fields<br />
                                                                            and your files cannot exceed more than 1000 rows
                                                                        </p>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col align-center justify-center>
                                                                        <v-btn id="AutoTest_IP597" color="grey" outlined="" dark="" class="elevation-0" height="40" @click.stop=" downloadSampleFile()">
                                                                            <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                        </v-btn>
                                                                    </v-col>
                                                                    <v-col align-center justify-center>
                                                                        <v-file-input prepend-icon="" label="Upload File" outlined dense
                                                                                      id="file" v-model="fileInput"
                                                                                      prepend-inner-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                      @change="handleFileUpload">
                                                                        </v-file-input>
                                                                    </v-col>

                                                                </v-row>
                                                                <v-row v-if="csvfile.name">

                                                                    <v-col align-center="" justify-center="">
                                                                        <v-chip id="AutoTest_IP598" close label color="white" @click:close="csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null">
                                                                            <v-icon size="30" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                            {{ fileInput.name }}
                                                                        </v-chip>

                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                        <Secondary-Button id="AutoTest_IP599" class="btn-122x36 mr-2" title="Confirm" @click.native="ImportKPI()" v-bind:disabled="fileInput==null"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP600" class="btn-122x36" title="Cancel" @click.native="ImportKPIDialogBox = false,csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null,fileInput=[]"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="ImportKPISuccessSnackbar" top="" color="primary">
            KPI Successfully imported
        </v-snackbar>


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
    import PreLoader from '@/components/pre-loader.vue'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objInitiative;
    async function importInitscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
        ]);
    };

    export default {

        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                closeMenu :false,
                justifyCenter: 'center',
                WorkspaceID: 0,
                ProjectId: 0,
                PreLoader: false,
                KPIDetailsTableheaders: [

                    { text: 'ID', align: ' d-none', value: 'KPIId' },
                    { text: "Name", value: "KPIName" },
                    { text: "Start date", value: "StartDate" },
                    { text: "End Date", value: "EndDate" },
                    { text: "Last Updated Date", value: "UpdatedDate" },
                    {text: "Work Flow", value: "WorkflowName" },
                    {text: "Survey", value: "SurveyName" },
                    { text: "Users", value: "UserList" },
                    { text: "Action", value: "Actions" },
                ],
                KPIDetailsTableItems: [],

                CreateKPIDialog: false,
                RemoveKPIDialog: false,
                RemoveBulkKPIDialog: false,
                SearchKPIDetails: "",
                EditKPIDialog: false,
                AssignUserDialog: false,
                //SelectKPIPeriodicity: ['Never', 'Once', 'Daily', 'Weekly', 'Monthly', 'Quarterly', 'Half Yearly', 'Yearly', 'Forever'],
                AssignAccountUserPopUp: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                KPISaveSuccessSnackbar: false,
                KPIUpdateSuccessSnackbar: false,
                SelectWorkflowDialog: false,
                SelectWorkflowDlgSteps: 0,
                AssignWorkflowSuccessSnackbar: false,
                AssignUsersSuccessSnackbar: false,
                searchUser: "",
                SelectUserTableheaders: [
                    { text: "UserID", align: ' d-none', value: "UserID" },
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                SelectUserTableItems: [{
                    UserID: '1',
                    Name: 'Sam',
                    UserRole: 'Admin'
                }],
                SelectUserSelectedUsersList: [],
                chipUsrItem: "",
                KPIRemoveSuccessSnackbar: false,


                //From Date
                startDate:"",// new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                endDate: "",//new Date().toISOString().substr(0, 10),
                ToDateMenu: false,
                workflows: [],
                Surveys: [],
                KPIName: "",
                Description: "",
                KPIPeriodicity: "",
                KPIRating: "",
                Workflow: "",
                Survey: "",
                UserList: [],
                selectedKPI: [],
                KPIId: "",
                KPIName: "",
                GroupId: 0,
                selectedteamUsers: [],
                lstuserTableheaders: [{ text: "Name", align: "left", value: "Name" }, { text: "User Role", value: "UserRole" },],
                lstUsers: [],
                //Select workflow starts here
                SelectWorkflowDialog: false,
                SelectWorkflowDlgSteps: 0,
                SelectWorkflowDlgItems: [],
                checkboxAddWorkflow: [],
                selectedAddWorkflow: [],
                //step 2 of Select Workflow
                ViewWorkflowItem: { WfLevelDetails: [] },
                //Select workflow ends here

                //Import KPI
                ImportKPIDialogBox: false,
                ImportKPISteps: 0,
                fileInput: null,
                csvfile: {
                    name: "",
                    size: 0,
                    namedisplay: ""

                },
                csvfiledata: [],
                ImportKPISuccessSnackbar: false,
                data: [['KPI Name Here', 'KPI description here', 'Never/Once/Daily/Weekly etc', 'Workflow Name here', 'Survey Name here', 'Start Date (dd/mm/yyyy) here', 'End Date (dd/mm/yyyy) here']],
                periodicityDate: new Date().toISOString().substr(0, 10),
                DateMenu: "",
                lstPeriodicity: [],
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                ReportingFrequency: 0,
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,
                rules: {
                    required: value => !!value || 'Required.',
                },
                periodicityDateMenu:false,

                // Tags ------------
                NewTagMenu: false,
                AllTagListSteps: 0,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#0F36F6', '#1FC2C8'],
                    ['#34C900', '#FFC914'],
                    ['#EC4554'],
                    ['#3374B9'],

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                color: "",
  
                UpdatingTagItem: "",
                SelectedTag: [],
                filterAllTagList: [],
                SearchTagDetails: "",
                Msg: "",
                snackbarMsg: false,
                ProjectStartDate: "",
                ProjectEndDate: "",
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
            'Pre-Loader': PreLoader,
        },

        computed: {
            FrmDate() {
                return this.formatDate(this.startDate)
            },
            ToDate() {
                return this.formatDate(this.endDate)
            },
            FormattedDate() {
                return this.formatDate(this.periodicityDate)
            },
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await importInitscript();
            this.PreLoader = true;
            await this.fnGetWorkflows();
            await this.fnGetKPIByprjID();
            await this.fnGetGroupDetailsByprjID();
            await this.fnGetTeamUsersList();
            await this.fnGetAllTags();
            await this.fnGetSurveys();
            await this.GetReportingFrequencyList();
            await this.GetProjectDate();
            this.PreLoader = false;
        },
        methods: {
            formatDate(date) {
                if (!date) return null
                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            async GetProjectDate() {

                var result = await objManageProject.GetProjectDate(this.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {

                    this.ProjectStartDate = result.PrjStartDate;
                    this.ProjectEndDate = result.PrjEndDate;
                    this.startDate = this.ProjectStartDate.substr(0, 10);
                    this.endDate = this.ProjectEndDate.substr(0, 10);
                }
            },
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
            
                if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10)) ;
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10))+")" ;
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same" ;
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }   
                return true;

            },
            IsValidPeriodicityDate() {
                if (this.startDate != null) {
                    if (Date.parse(this.periodicityDate) < Date.parse(this.startDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.periodicityDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }

                if (this.endDate != null) {
                    if (Date.parse(this.periodicityDate) > Date.parse(this.endDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.periodicityDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                return true;

            },
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },
            resetValidation() {
                this.$refs.form.resetValidation()
            },
            async fnCreateKPI() {

                /*Chk Duplicate Name*/

                for (var i = 0; i < this.KPIDetailsTableItems.length; i++) {
                    if (this.KPIDetailsTableItems[i].KPIName == this.KPIName ) {
                        this.Msg = "Duplicate Entry - Same KPI Name existis..";
                        this.snackbarMsg = true; 
                        this.KPIName = "";
                        return;
                    }
                }
                if (!this.IsValidDate()) {
                    this.Msg = " Invalid Date ";
                    this.snackbarMsg = true; 
                    return;
                }
                if (this.ReportingFrequency == 2) {
                    if (!this.IsValidPeriodicityDate()) {
                        this.Msg = " Invalid PeriodicityDate ";
                        this.periodicityDate = "";
                        this.snackbarMsg = true;
                        return;
                    }
                }
                   
                var data = new FormData();
                data.append("KPIId", this.KPIId);
                data.append("KPIName", this.KPIName);
                data.append("Description", this.Description);
                data.append("KPIPeriodicity", this.ReportingFrequency);
                data.append("KPIRating", this.KPIRating);
                data.append("Startdate", this.startDate);
                data.append("Enddate", this.endDate);
                data.append("WorkflowID", this.Workflow);
                data.append("SurveyID", this.Survey);
                data.append("ProjectId", this.ProjectId);
                data.append("Users", JSON.stringify(this.selectedteamUsers));
                data.append("Tags", JSON.stringify(this.SelectedTag));
                data.append("ReportingMonth", this.ReportingMonth);
                data.append("ReportingMonthDay", this.ReportingMonthDay);
                data.append("ReportingWeek", this.ReportingWeek);
                data.append("ReportingOnceDate", this.FormattedDate);
                data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);
                
                var result = await objManageProject.CreateKPI(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "success") {
                        this.CreateKPIDialog = false;
                        this.KPISaveSuccessSnackbar = true;
                        this.fnGetKPIByprjID();
                        this.selectedteamUsers = [];
                        this.CloseKPIDialog();
                    }
                     else {

                         if (result.status == "rptfrequencyfailed") {
                             this.Msg ="Reporting Frequency Failed";
                            } else {
                             this.Msg ="Failed";
                        }
                        this.snackbarMsg = true;
                    }
            },
            async fnAssignUserstoKPI() {
                var data = new FormData();
                data.append("KPIId", this.KPIId);
                data.append("ProjectId", this.ProjectId);
                data.append("Users", JSON.stringify(this.selectedteamUsers));
                
                var result = await objManageProject.AssignUserstoKPI(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                if (result.status == "success") {
                    this.AssignUsersSuccessSnackbar = true;
                    this.fnGetKPIByprjID();
                    this.selectedteamUsers = [];
                }
                else {
                    this.Msg = "Failed";
                    this.snackbarMsg = true;
                    }
            },
            async fnGetWorkflows() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetWorkflows(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.workflows = result.data.workflows;
                }
            },
            async fnGetSurveys() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetSurveyName(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.Surveys = result.data.Survey;
                }
            },
            async fnGetKPIByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetKPIByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.KPIDetailsTableItems = result.data;


                    }
            },
            fnBulkRemoveClick() {
                
                if (this.selectedKPI.length > 0) {
                    this.RemoveBulkKPIDialog = true;
                } else {
                    this.Msg = "Please select the KPI";
                    this.snackbarMsg = true;
                }
                this.closeMenu = true;
            },
            async fnDeleteKPI(Type) {
               
                var data = new FormData();
                if (Type == 1) {
                    data.append("KPIId", JSON.stringify(this.selectedKPI));
                    data.append("Type", 1);
                }
                else {
                    data.append("KPIId", JSON.stringify(this.KPIId));
                    data.append("Type", 2);
                }
                var result = await objManageProject.DeleteKPI(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.RemoveKPIDialog = false;
                        this.KPIRemoveSuccessSnackbar = true;
                    } else {

                        this.ErrorPaymentTermSuccessSnackbar = true;
                    }
                }
                this.selectedKPI = [];
                this.fnGetKPIByprjID();
                this.KPIId = 0;
            },
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.GroupId = result.data.GroupId;
                    }
            },
            async fnGetTeamUsersList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("GroupId", this.GroupId);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objManageProject.GetTeamUsersList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.lstUsers = result.data;
                    }
            },
            async fnGetKPIByKPIId(KPIId) {
                this.selectedUsers = [];
                var data = new FormData();
                data.append("KPIId", KPIId);
                var result = await objManageProject.GetKPIByKPIId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.selectedteamUsers = JSON.parse(result.data.Users);
                    }
            },
            fnCheckMandatory() {
                if (this.KPIName == "" ||
                    this.KPIype == "" ||
                    this.ReportingFrequency == 0 ||
                    (this.ReportingFrequency == 2 && (this.periodicityDate == "" || this.periodicityDate == null)) ||
                    //this.KPIRating == "" ||
                    this.startDate == "" ||
                    this.endDate == "" ||                   
                    this.selectedteamUsers == 0)
                    return true;
                else return false;
            },
            CloseKPIDialog() {

               
                //this.KPIRating = "";
                this.Workflow = "";
                this.Survey = "";
                this.startDate = new Date().toISOString().substr(0, 10);
                this.endDate = new Date().toISOString().substr(0, 10);
                this.selectedteamUsers = [];
                this.SelectedTag = [];
                this.resetValidation();
                this.KPIName = '';
                this.Description = "";
                this.ReportingFrequency = 0;
                this.CreateKPIDialog = false
            },
            async GetReportingFrequencyList() {
                var result = await objManageProject.GetReportingFrequencyList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    if (result.data.RptFrequencyList.length > 0) {
                        this.lstPeriodicity = result.data.RptFrequencyList;
                    }
                    if (result.data.WeekList.length > 0) {
                        this.lstWeek = result.data.WeekList;
                    }
                    if (result.data.MonthList.length > 0) {
                        this.lstMonth = result.data.MonthList;
                    }
                    if (result.data.MonthDate.length > 0) {
                        this.lstMonthDate = result.data.MonthDate;
                    }
                }
            },
            //*** Select Workflow functions starts here ***//

            async fnGetWorkflowsListByProjectID(item) {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objManageProject.GetWorkflowsListByProjectID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectWorkflowDlgItems = result.data;
                    this.filterWorkflowsList = result.data;
                    var ID;
                    var temp = "";

                    for (ID = 0; ID <= this.SelectWorkflowDlgItems.length - 1; ID++) {
                        if (this.SelectWorkflowDlgItems[ID].WorkflowID == item.WorkflowID) {
                            temp = this.SelectWorkflowDlgItems.filter(x => x.WorkflowID == item.WorkflowID);
                        }
                        if (temp.length > 0) {
                            this.selectedAddWorkflow.push({ checkboxindex: ID, WFitem: temp[0] });
                            this.checkboxAddWorkflow[ID] = true;
                        }
                        temp = [];

                    }

                }
            },
            fnCheck(item, index) {

                var i;
                var preval = this.checkboxAddWorkflow[index]
                this.selectedAddWorkflow = [];
                for (i = 0; i <= this.SelectWorkflowDlgItems.length - 1; i++) {
                    this.checkboxAddWorkflow[i] = false;
                }
                if (preval) {
                    this.checkboxAddWorkflow[index] = true;
                    this.selectedAddWorkflow.push({ checkboxindex: index, WFitem: item });
                } else {

                    if (this.selectedAddWorkflow.length > 0) {
                        this.selectedAddWorkflow = this.selectedAddWorkflow.filter(x => x.WFitem.WorkflowID != item.WorkflowID);
                    }
                }
            },
            fnRemove(item) {
                this.checkboxAddWorkflow[item.checkboxindex] = false;
                this.selectedAddWorkflow = this.selectedAddWorkflow.filter(x => x.WFitem.WorkflowID != item.WFitem.WorkflowID);
            },
            async fnAddSelectWorkflow() {
                var data = new FormData();
                data.append("WorkFlowID", this.selectedAddWorkflow[0].WFitem.WorkflowID);
                data.append("ProjectId", this.ProjectId);
                data.append("KPIId", this.KPIId);
                var result = await objManageProject.UpdateKPIWorkflow(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.AssignWorkflowSuccessSnackbar = true;
                    this.SelectWorkflowDialog = false;
                    this.SelectWorkflowDlgSteps = 1;
                    this.SelectWorkflowDlgItems = [];
                    this.checkboxAddWorkflow = [];
                    this.selectedAddWorkflow = [];
                    this.fnGetKPIByprjID();
                    this.fnGetWorkflowsListByProjectID();
                }
            },
            //*** Select Workflow functions ends here ***//
            // workflow filter
            async fnFilterWorkflowBasedOnUser() {
                var ID = 0;
                var userId = 0;
                var WorkFlowID = 0;
                var WorkflowLevel = 0;
                var UserListID = 0;
                var find = false;
                var RemovingWorflow = [];
                for (WorkFlowID = 0; WorkFlowID <= this.SelectWorkflowDlgItems.length - 1; WorkFlowID++) {
                    for (WorkflowLevel = 0; WorkflowLevel <= this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails.length - 1; WorkflowLevel++) {     // level
                        for (UserListID = 0; UserListID <= this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[WorkflowLevel].usersList.length - 1; UserListID++) {         // user
                            ID = this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[WorkflowLevel].usersList[UserListID].ID;
                            for (userId = 0; userId <= this.WorkflowAssignUsersTableItems.length - 1; userId++) {
                                if (this.WorkflowAssignUsersTableItems[userId].ID != ID) {

                                    find = false;
                                } else {
                                    find = true;
                                    break;
                                }
                            }
                            if (find == false) {
                                RemovingWorflow.push({ WFID: this.SelectWorkflowDlgItems[WorkFlowID].WorkflowID });
                            }
                        }
                    }
                }
                if (RemovingWorflow.length > 0) {
                    var WFID = 0
                    for (WFID = 0; WFID <= RemovingWorflow.length - 1; WFID++) {
                        this.SelectWorkflowDlgItems = this.SelectWorkflowDlgItems.filter(x => x.WorkflowID != RemovingWorflow[WFID].WFID);
                    }
                }
            },
            async ImportKPI() {
                var data = new FormData();
                data.append("ImportKPIList", JSON.stringify(this.csvfiledata));
                data.append("ProjectId", this.ProjectId);
                this.ImportInviteUserSteps = 3;
                var result = await objManageProject.ImportKPI(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {

                        this.fnGetKPIByprjID();
                        this.ImportKPISuccessSnackbar = true;
                        this.ImportKPIDialogBox = false;
                       
                    }
                    else {
                        this.Msg = "Wrong file / data ";
                        this.snackbarMsg = true;
                    }
                    this.csvfile.name = "";
                    this.csvfile.size = "";
                    this.csvfile.namedisplay = "";
                    this.fileInput = [];
                }
                    
            },
            handleFileUpload() {
                this.csvfile.name = this.fileInput.name;
                this.csvfile.size = this.fileInput.size;
                this.csvfile.namedisplay = this.csvfile.name + "\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0" + this.formatFileSize(this.csvfile.size, 0);
                this.fnCSVLoadParse();
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            fnCSVLoadParse() {
                if (this.csvfile.name) {
                    var vm = this;
                    if (window.FileReader) {
                        var reader = new FileReader();
                        reader.readAsText(this.fileInput);
                        // Handle errors load
                        reader.onload = function (event) {
                            var csv = event.target.result;
                            vm.csvfiledata = vm.csvJSON(csv)
                        };
                        reader.onerror = function (evt) {
                            if (evt.target.error.name == "NotReadableError") {
                                this.Msg = "Cannot read file !";
                                this.snackbarMsg = true;
                            }


                        };
                    }
                    else {
                        this.Msg = 'FileReader is not supported in this browser.';
                        this.snackbarMsg = true;
                    }
                        
                }
            },
            csvJSON(csv) {
               
                var vm = this;
                vm.roleErrorCount = 0; //reset before loading the file datas
                var lines = csv.split(/\r\n|\n/);
                var result = [];
                var headers = lines[0].split(",");
               
                var reqHeading1 = 'KPIName,Description,ReportingFrequencyName,WorkflowName,SurveyName,StartDate,EndDate\n';
                var headers1 = reqHeading1.split(/\r\n|\n/)[0].split(",");

                if (headers1.length != headers.length) {
                    this.Msg = "Wrong file selected ...columns are not matching .. Please check sample file format";
                    this.snackbarMsg = true;
                    this.fileInput = [];
                    this.csvfile.name = null;
                    this.csvfile.size = 0;
                    this.csvfile.namedisplay = null;
                    return;
                }
                    

                //console.log("headers = ", headers[0], headers[1]);
                for (var i = 1; i < lines.length; i++) {
                    var obj = {};
                    var errEmail = false;
                    var currentline = lines[i].split(",");
                    if (currentline != "") {
                        for (var j = 0; j < headers.length; j++) {
                            
                            if (j == 5 || j == 6)
                                currentline[j] = this.formatDate(currentline[j]);
                            obj[headers[j]] =currentline[j];
                        }
                        result.push(obj);
                    }
                }
                return result;
            },
            downloadSampleFile() {
              
                var csv = 'KPIName,Description,ReportingFrequencyName,WorkflowName,SurveyName,StartDate,EndDate\n';
                this.data.forEach(function (row) {
                    csv += row.join(',');
                    csv += "\n";
                });

                // console.log(Math.round(Math.random() * 1000));
                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_project_KPI_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            }  ,
            //Tags
            async fnGetAllTags() {
                var result = await objManageProject.GetAllProjectTags(this.InitiativeId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = "Failed to get tag list.";
                    this.snackbarMsg = true;
                }
            },
            async fnEditTagDetails(TagItem) {
                this.AllTagListSteps = 3;
                this.TagHeaderName = 'Edit Tag Name';
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;
            },
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();

                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objManageProject.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags()
                        this.UpdatingTagItem = "";
                    }
            },
            async CreateNewTag(color, TagName) {
                var temp = this.AllTagList.filter(x => x.TagName.includes(TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.snackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objManageProject.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.NewTagName = "";
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags();
                    }
            },
            async fnAddToSelectedTag(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                if (this.SelectedTag.length > 0) {
                    var temp = this.SelectedTag.filter(x => x.TagID == TagItem.TagID);
                    if (temp.length == 0) {
                        this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                } else {
                    this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                }
                    this.NewTagMenu = false;
                }
            },
            async fnRemoveFromSelectedTagItem(TagItem) {
                this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
                this.SelectedTag = [...this.SelectedTag];
                if (this.SelectedTag.length == 0) {
                    this.allTags = false;
                }
            },
            CloseTag() {
                this.color = "";
                this.TagHeaderName = 'Add Tag';
                this.NewTagName = "";
                this.NewTagMenu = false;
                this.EditTagName = "";
            },
            RemoveUser(item) {
                this.selectedteamUsers.splice(this.selectedteamUsers.indexOf(item), 1);
                this.selectedteamUsers = [...this.selectedteamUsers];
            },
        },
        watch: {

            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
            },

        }
       
    }
</script>