<template>
    <div>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid class="px-0">
            <v-row>
                <v-col md="auto">
                    <h5 class="page-head">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                        </v-avatar>
                        Activities & Inputs
                    </h5>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto">
                    <v-btn depressed height="39" :to="{name: 'OutputReportTable', query: {ProjID: $route.query.ProjID} }" color="green" dark>Report</v-btn>
                </v-col>
                <v-col v-if="ActivityList.length>0" md="auto">
                    <!--Create Menu-->
                    <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn v-on="on" class="success-btn mr-4 float-left" depressed dark height="39">
                                Create
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list dense="" color="white" light width="auto" max-width="250" class="action-button-list">
                            <v-list-item id="AutoTest_IP1201" link @click="fnCreateOuput()">
                                <v-list-item-content>
                                    Create Activities & Inputs
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item id="AutoTest_IP202" link="" @click="CreateSubActivityDailog = true">
                                <v-list-item-content>
                                    Add Activity
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>

                    <!--Bulk Actions-->
                    <v-menu offset-y="" transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn id="AutoTest_IP203" height="39" v-on="on" color="grey" outlined class="elevation-0 mr-4 float-left" @click="">
                                Bulk Actions
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list v-if="selectedActivityList.length>0" color="white" light width="auto" max-width="250" class="action-button-list">
                            <v-list-item id="AutoTest_IP204" @click.stop="RemoveActivityDialog = true">
                                <v-list-item-icon>
                                    <v-icon>mdi-trash-can-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Remove
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>

                    <!--Search-->
                    <v-text-field label="Search" class="mr-4 float-left" v-model="SearchActivity"
                                  dense="" prepend-inner-icon="mdi-magnify"
                                  outlined="" hide-details="" style="max-width:150px"></v-text-field>
                    <!--Filter-->
                    <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                        <template v-slot:activator="{ on }">
                            <v-btn id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent">
                                <v-icon>mdi-filter-outline</v-icon>
                                Filters
                            </v-btn>
                        </template>
                    </v-menu>
                </v-col>
            </v-row>
            <v-row no-gutters v-if="filterContent">
                <h4 class="mx-4 my-2 page-head">
                    Filters
                </h4>
                <!--Date range-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="dateRangeMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            {{dateRangeHeaderName}}
                            <v-icon class="ml-10">
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" width="auto" class="px-2">
                        <v-row justify="center" no-gutters>
                            <v-col cols="4" class="px-2">
                                <h3 style="text-align:center">Start Date</h3>
                                <v-date-picker color="primary" v-model="FiterStartDate" no-title @input="IsValidDate()"
                                               class="elevation-0"></v-date-picker>
                            </v-col>
                            <v-col cols="4" class="px-2">
                                <h3 style="text-align:center">End Date</h3>
                                <v-date-picker color="primary" v-model="FiterEndDate" no-title :min="FiterStartDate" @input="IsValidDate()"
                                               class="elevation-0"></v-date-picker>
                            </v-col>
                            <v-col cols="4">
                                <v-list class="px-2 mx-4">
                                    <v-header style="font-size:large" class="pl-4">Predefined Periods:</v-header>
                                    <v-list-item-group v-model="selectedItem"
                                                       color="primary">
                                        <v-list-item v-for="(item, i) in items"
                                                     :key="i">
                                            <v-list-item-content>
                                                <v-list-item-title v-text="item.text"></v-list-item-title>
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list-item-group>
                                </v-list>
                            </v-col>
                        </v-row>
                        <v-divider></v-divider>
                        <v-row justify="center" class="px-2">
                            <v-col cols="2">
                                <v-btn height="32" block="" class="elevation-0 " @click="dateRangeMenu = false">
                                    cancel
                                </v-btn>
                            </v-col>
                            <v-spacer></v-spacer>
                            <v-col cols="2">
                                <v-btn height="32" block="" color="primary" class="elevation-0" @click="fnClickFilterDone(),dateRangeMenu = false">
                                    Done
                                </v-btn>
                            </v-col>
                        </v-row>
                    </v-list>
                </v-menu>

                <!--Organisation-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="organisationMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            Organisation
                            <v-icon class="ml-10">
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                        <v-checkbox color="success-check-box" input-value="true" label="Company1"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="Impact Ind"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="Co.Pvt."></v-checkbox>

                        <div class="d-inline-flex">
                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="organisationMenu = false">
                                Cancel
                            </v-btn>
                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="organisationMenu = false">
                                Done
                            </v-btn>
                        </div>
                    </v-list>
                </v-menu>

                <!--Workflow-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="workflowMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            Workflow
                            <v-icon class="ml-12">
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                        <v-checkbox color="success-check-box" input-value="true" label="Evaluation Approval"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="Data Collection Approval"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="Financial Approval"></v-checkbox>

                        <div class="d-inline-flex">
                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="workflowMenu = false">
                                Cancel
                            </v-btn>
                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="workflowMenu = false">
                                Done
                            </v-btn>
                        </div>
                    </v-list>
                </v-menu>

                <!--Assigned User-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="assignedUserMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            Assigned User
                            <v-icon class="ml-12">
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                        <v-checkbox color="success-check-box" input-value="true" label="koushik"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="subhash"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" label="kishore"></v-checkbox>

                        <div class="d-inline-flex">
                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="assignedUserMenu = false">
                                Cancel
                            </v-btn>
                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="assignedUserMenu = false">
                                Done
                            </v-btn>
                        </div>
                    </v-list>
                </v-menu>
            </v-row>
            <!--<v-row no-gutters class="py-2" v-if="filterContent">
                <v-chip class="mx-1" medium label clearable close>31 Jun - 26 Jul</v-chip>
                <v-chip class="mx-1" medium label clearable close>Company1</v-chip>
                <v-chip class="mx-1" medium label clearable close>Evaluation Approval</v-chip>
                <v-chip class="mx-1" medium label clearable close>Subhash</v-chip>
            </v-row>-->
            <v-row v-if="filterChipsList.length>0" class="pa-0">
                <v-col cols="12" xs="12" sm="12" md="12" class="pa-0">
                    <div class="chip-container ma-0 ml-2">
                        <v-chip-group multiple column active-class="primary--text">
                            <v-chip id="" close v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="mx-2" @click:close="fnChipClose(item)" memdium label>
                                {{ item.ChipName }}
                            </v-chip>
                        </v-chip-group>
                    </div>
                </v-col>
            </v-row>
            <v-row>
                <!--If there is no Activities-->
                <v-col v-if="ActivityList.length==0" class="text-center">
                    <p class="mt-12 pt-12">
                        <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                    </p>

                    <p class="heading-3-dark font-weight-medium pt-6">No Activities & Inputs data available</p>
                    <v-btn id="AutoTest_IP205" @click="fnCreateOuput(),AddEditActivityDialogHeader='Create Output',SubActivityTotalWeightage =0;" class="success-btn mt-2" depressed dark height="37" v-bind:disabled="!screenAccess">
                        Create Activities & Inputs
                    </v-btn>
                </v-col>
                <!--______________________________________-->
            </v-row>

            <!--Tree table section-->
            <v-row class="px-0">
                <v-col v-if="filterActivityList.length >0" class="px-0">
                    <tree-table class="table tree-table"
                                fixed-header
                                height="60vh"
                                :columns="ActivityListHeader"
                                :table-data="paginatedData">
                        <!-- Header Row -->
                        <template #headerTemplate="headerProps"
                                  class="py-0">
                            <div v-bind="headerProps">
                                <div class="row header" style="background-color:#F5F5F5;max-height:50px">
                                    <div class="cell pl-4 pt-0" style="max-width:20px">
                                        <v-checkbox color="#52B962" v-model="Commoncheck" @change="CheckFn()"></v-checkbox>
                                    </div>

                                    <div class="cell" style="max-width:65px"></div>
                                    <table style="font-size:13px;color: #707070;" class="cell mt-2 mb-7">
                                        <tr>
                                            <th style="width:190px"><b>{{ActivityListHeader[0].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[1].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[2].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[3].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[4].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[5].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[6].label}}</b></th>
                                            <th style="width:130px"><b>{{ActivityListHeader[7].label}}</b></th>
                                        </tr>
                                    </table>
                                    <!--<div style="font-size:13px;color: #707070;" class="cell mt-2" v-for="column in headerProps.columns" :key="column.id">
                                        <b>{{ column.label }}</b>-->
                                    <!--<div class="" style="display:inline-block; cursor:pointer">
                                        <div id="AutoTest_IP206" style="height:10px" @click="sortTable({key: column.id, asc: true})">
                                            <v-icon size="10">mdi-arrow-up</v-icon>
                                        </div>
                                        <div id="AutoTest_IP207" style="height:10px" @click="sortTable({key: column.id, asc: false})">
                                            <v-icon size="10">mdi-arrow-down</v-icon>
                                        </div>
                                    </div>-->
                                    <!--</div>-->
                                </div>
                            </div>
                        </template>

                        <!-- Rows with sub-activities -->
                        <template #nodeTemplate="nodeProps">
                            <div v-bind="nodeProps">
                                <div class="row" style="max-height:65px;">

                                    <div class="cell pl-4 pt-0" style="max-width:20px">

                                        <div v-if="Commoncheck == true">
                                            <v-checkbox v-model="nodeProps.rowData.checkbox=ActivityList[0].multS" @change="fnAddRemove(nodeProps.rowData)" color="#52B962"></v-checkbox>
                                        </div>
                                        <div v-if="Commoncheck == false && ActivityList[0].multS == false">
                                            <v-checkbox v-model="nodeProps.rowData.checkbox=ActivityList[0].multS" @change="fnAddRemove(nodeProps.rowData)" color="#52B962"></v-checkbox>
                                        </div>

                                        <div v-if="Commoncheck == false && ActivityList[0].multS != false">
                                            <v-checkbox v-model="nodeProps.rowData.checkbox" @change="fnAddRemove(nodeProps.rowData)" color="#52B962"></v-checkbox>
                                        </div>

                                    </div>
                                    <!--<div v-if="nodeProps.depth > 0">
                                        <div class="indentation" :style="{ width:(step*nodeProps.depth) + 'px' }">
                                        </div>
                                    </div>-->
                                    <div id="AutoTest_IP208" class="pt-3 pl-3" @click="nodeProps.onToggle">
                                        <v-btn id="AutoTest_IP209" v-if="nodeProps.rowData.children.length>0" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen">
                                            <div v-if="!nodeProps.rowData.isOpen">
                                                <v-icon>mdi-chevron-right</v-icon>
                                            </div>
                                            <div v-if="nodeProps.rowData.isOpen">
                                                <v-icon>mdi-chevron-down</v-icon>
                                            </div>

                                        </v-btn>
                                    </div>

                                    <div class="pt-3" v-if="nodeProps.rowData.children.length == 0">
                                        <v-btn id="AutoTest_IP210" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen">
                                            <div v-if="!nodeProps.rowData.isOpen" hidden>
                                                <v-icon>mdi-chevron-right</v-icon>
                                            </div>
                                            <div v-if="nodeProps.rowData.isOpen" hidden>
                                                <v-icon>mdi-chevron-down</v-icon>
                                            </div>

                                        </v-btn>
                                    </div>
                                    <!--Projects list column Name-->
                                    <template v-if="nodeProps.rowData.ActivityName">
                                        <v-list class="py-0 pt-2 table-v-list">
                                            <!--if path is ManageProjectActivities-->
                                            <v-list-item class="my-0 px-0" v-if="$route.name=='ManageProjectActivities'" :to="{name:'ManageProjectActivitiesIndividual',query: { WsID: WorkspaceID , Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                                    <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                                </v-avatar>
                                                <v-list-item-content>
                                                    <div class="text-limit" style="width:160px">{{ nodeProps.rowData.ActivityName }}</div>
                                                </v-list-item-content>
                                            </v-list-item>
                                            <!--if path is MyprojectsDraftIndividualActivities-->
                                            <v-list-item class="my-0 px-0" v-if="$route.name=='MyprojectsDraftIndividualActivities'" :to="{name:'MyprojectsDraftIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                                    <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                                </v-avatar>
                                                <v-list-item-content>
                                                    <div class="text-limit" style="width:160px">{{ nodeProps.rowData.ActivityName }}</div>
                                                </v-list-item-content>
                                            </v-list-item>
                                            <!--if path is MyprojectsDeployedIndividualActivities-->
                                            <v-list-item class="my-0 px-0" v-if="$route.name=='MyprojectsDeployedIndividualActivities'" :to="{name:'MyprojectsDeployedIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                                    <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                                </v-avatar>
                                                <v-list-item-content>
                                                    <div class="text-limit" style="width:160px">{{ nodeProps.rowData.ActivityName }}</div>
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list>
                                    </template>
                                    <!--<div v-if="nodeProps.rowData.ActivityName" class="cell text-limit pt-5">-->
                                    <!--if path is ManageProjectActivities-->
                                    <!--<router-link v-if="$route.name=='ManageProjectActivities'" class="text--primary" :to="{name:'ManageProjectActivitiesIndividual',query: { WsID: WorkspaceID , Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                        </v-avatar>
                                        <span style="color:#757575">{{ nodeProps.rowData.ActivityName }}</span>
                                    </router-link>-->
                                    <!--____________-->
                                    <!--if path is MyprojectsDraftIndividualActivities-->
                                    <!--<router-link v-if="$route.name=='MyprojectsDraftIndividualActivities'" class="text--primary" :to="{name:'MyprojectsDraftIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                        </v-avatar>
                                        <span style="color:#757575">{{ nodeProps.rowData.ActivityName }}</span>
                                    </router-link>-->
                                    <!--____________-->
                                    <!--if path is MyprojectsDeployedIndividualActivities-->
                                    <!--<router-link v-if="$route.name=='MyprojectsDeployedIndividualActivities'" class="text--primary" :to="{name:'MyprojectsDeployedIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                        <v-row class="pa-0 ma-0" style="height:46px; width:120px">
                                            <v-col md="2" class="pa-0 ma-0 mr-1">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-1">
                                                    <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                                </v-avatar>
                                            </v-col>
                                            <v-col md="9" class="pa-0 ma-0">
                                                <span style="color:#757575">{{ nodeProps.rowData.ActivityName }}</span>
                                            </v-col>
                                        </v-row>
                                    </router-link>-->
                                    <!--____________-->
                                    <!--</div>-->

                                    <div class="cell pt-5">
                                        {{ formatDate(nodeProps.rowData.StartDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ formatDate(nodeProps.rowData.EndDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ formatDate(nodeProps.rowData.UpdatedDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ nodeProps.rowData.WeightagePercentage }}
                                    </div>

                                    <div class="cell">
                                        <v-progress-circular :rotate="360"
                                                             :size="40"
                                                             :width="5"
                                                             :value="nodeProps.rowData.Progress"
                                                             color="secondary">
                                            {{ nodeProps.rowData.Progress }}
                                        </v-progress-circular>
                                        {{ nodeProps.rowData.Progress }}%
                                    </div>

                                    <!-- Users-->
                                    <div class="cell pt-4">
                                        <v-tooltip left>
                                            <template v-slot:activator="{ on }">
                                                <div v-on="on">
                                                    <v-avatar size="30" v-for="usr in nodeProps.rowData.ActivityUsers.slice(0, 1)" :key="usr.ID" :color="`${commonGetColor(usr.Name)}`" class="team-avatar">
                                                        <v-img v-bind:src="userimage + usr.ProfileImage" alt="User DP" height="30" width="30" class="ma-4" v-if="usr.ProfileImage.length >0"></v-img>
                                                        <span class="white--text" v-else>{{usr.Name == null ? "" : usr.Name.substring(0, 1).toUpperCase()}}</span>
                                                        <!--<span v-if="usr.Name !=null" class="text--white">{{usr.Name.charAt(0)}}</span>-->
                                                    </v-avatar>
                                                    <v-avatar size="30" class="team-count" v-if="nodeProps.rowData.ActivityUsers.length > 1">
                                                        +{{nodeProps.rowData.ActivityUsers.length -1}}
                                                    </v-avatar>
                                                </div>
                                            </template>
                                            <span v-if="nodeProps.rowData.ActivityUsers.length  >=1 && nodeProps.rowData.ActivityUsers.length  >0">{{nodeProps.rowData.ActivityUsers[0].Name}}</span>
                                            <span v-if="nodeProps.rowData.ActivityUsers.length >=2">,{{nodeProps.rowData.ActivityUsers[1].Name}}</span>
                                        </v-tooltip>
                                    </div>

                                    <div class="cell pt-4">
                                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                            <template v-slot:activator="{ on }">
                                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                    <v-icon>mdi-dots-vertical</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-list dense="" color="white" light="" width="auto" class="action-button-list">
                                                <v-list-item id="AutoTest_IP211" link="" @click.stop="AddEditActivityDialog = true,AddEditActivityDialogHeader='Edit Activity',fnInitializeAddEditActivity(nodeProps.rowData), fnEditOutputDlg(nodeProps.rowData)">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-pencil-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Edit</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP749" link @click.stop="fnGetWorkflowsListByProjectID(nodeProps.rowData),SelectWorkflowDialog = true">
                                                    <v-list-item-icon><v-icon>mdi-timeline-text-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Assign Workflow</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP212" link="" @click.stop="RemoveActivityDialog = true,RemoveActivityItem=nodeProps.rowData">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-trash-can-outline
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Remove</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP213" link="" @click.stop="AssignUserDialog = true,fnInitializeAssignUsers(nodeProps.rowData)">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-account-outline
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Assign User</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP214" link="" @click.stop=" /*fnAddSubActivityInSubActivity(nodeProps.rowData.ActivityId,nodeProps.rowData)*/fnCreateOuput(nodeProps.rowData, 'AddSubActivity')">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-subdirectory-arrow-right
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Add Activity</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-menu>
                                    </div>
                                </div>
                            </div>
                        </template>

                        <!-- Rows with no sub-activities -->
                        <template #leafTemplate="leafProps">
                            <div v-bind="leafProps">
                                <div class="row" style="max-height:55px;">
                                    <div class="cell" style="max-width:50px">
                                        <v-checkbox v-model="leafProps.rowData.checkbox" @change="fnAddRemove(leafProps.rowData)" color="#52B962"></v-checkbox>
                                    </div>
                                    <div v-if="leafProps.depth > 0">
                                        <div class="indentation" :style="{ width:(step*leafProps.depth)  + 'px' }">

                                        </div>
                                    </div>
                                    <div class="cell text-limit pt-5">
                                        <!--<router-link :to="{name:'ManageProjectActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ leafProps.rowData.ActivityName }}
                                        </router-link>-->
                                        <!--if path is ManageProjectActivities-->
                                        <router-link v-if="$route.name=='ManageProjectActivities'" :to="{name:'ManageProjectActivitiesIndividual',query: { WsID: WorkspaceID , Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ leafProps.rowData.ActivityName }}
                                        </router-link>


                                        <!--____________-->
                                        <!--if path is MyprojectsDraftIndividualActivities-->
                                        <router-link v-if="$route.name=='MyprojectsDraftIndividualActivities'" :to="{name:'MyprojectsDraftIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ leafProps.rowData.ActivityName }}
                                        </router-link>
                                        <!--________________________-->
                                        <!--if path is MyprojectsDeployedIndividualActivities-->
                                        <router-link v-if="$route.name=='MyprojectsDeployedIndividualActivities'" :to="{name:'MyprojectsDeployedIndividualActivitiesIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ leafProps.rowData.ActivityName }}
                                        </router-link>
                                        <!--________________________-->
                                    </div>

                                    <div class="cell pt-5">
                                        {{ formatDate(leafProps.rowData.StartDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ formatDate(leafProps.rowData.EndDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ formatDate(leafProps.rowData.UpdatedDate) }}
                                    </div>

                                    <div class="cell pt-5">
                                        {{ leafProps.rowData.WeightagePercentage }}
                                    </div>

                                    <div class="cell">
                                        <v-progress-circular :rotate="360"
                                                             :size="40"
                                                             :width="5"
                                                             :value="leafProps.rowData.Progress"
                                                             color="secondary">
                                            {{ leafProps.rowData.Progress }}
                                        </v-progress-circular>
                                        {{ leafProps.rowData.Progress }}%
                                    </div>

                                    <!-- Users-->
                                    <div class="cell">
                                        <v-tooltip left>
                                            <template v-slot:activator="{ on }">
                                                <div v-on="on">
                                                    <v-avatar v-for="usr in leafProps.rowData.ActivityUsers" :key="usr.ID" :color="`${commonGetColor(usr.Name)}`" size="30" class="team-avatar">
                                                        <v-img v-bind:src="userimage + usr.ProfileImage" alt="User DP" height="30" width="30" v-if="usr.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{usr.Name == null ? "" : usr.Name.substring(0, 1).toUpperCase()}}</span>
                                                        <!--<span class="text--white headline">{{usr.Name.charAt(0)}}</span>-->
                                                    </v-avatar>
                                                    <v-avatar size="30" class="team-count" v-if="leafProps.rowData.ActivityUsers.length > 1">
                                                        +{{leafProps.rowData.ActivityUsers.length -1}}
                                                    </v-avatar>
                                                </div>
                                            </template>
                                            <span v-if="leafProps.rowData.ActivityUsers.length  >=1 && leafProps.rowData.ActivityUsers.length  >0">{{leafProps.rowData.ActivityUsers[0].Name}}</span>
                                            <span v-if="leafProps.rowData.ActivityUsers.length >=2">,{{leafProps.rowData.ActivityUsers[1].Name}}</span>
                                        </v-tooltip>
                                    </div>

                                    <div class="cell pt-4">
                                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                            <template v-slot:activator="{ on }">
                                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                    <v-icon>mdi-dots-vertical</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-list dense="" color="white" light="" width="auto" class="action-button-list">
                                                <v-list-item id="AutoTest_IP215" link="" @click.stop="AddEditActivityDialog = true,AddEditActivityDialogHeader='Edit Activity',fnInitializeAddEditActivity(leafProps.rowData), fnEditOutputDlg(leafProps.rowData)">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-pencil-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Edit</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP216" link="" @click.stop="RemoveActivityDialog = true,RemoveActivityItem=leafProps.rowData">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-trash-can-outline
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Remove</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP217" link="" @click.stop="AssignUserDialog = true,fnInitializeAssignUsers(leafProps.rowData)">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-account
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Assign User</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP218" link="" @click.stop="/*fnAddSubActivityInSubActivity(leafProps.rowData.ActivityId,leafProps.rowData)*/ fnCreateOuput(leafProps.rowData, 'AddSubActivity')">
                                                    <v-list-item-icon>
                                                        <v-icon>
                                                            mdi-chart-areaspline
                                                        </v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Add Activity</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-menu>
                                    </div>
                                </div>
                            </div>
                        </template>
                    </tree-table>
                    <!--Pagination-->
                    <div class="pagination">
                        <div id="AutoTest_IP219" class="pagination--button pagination--button__active" v-if="!isFirstPage()" v-on:click="prevPage()">&lt;</div>
                        <div class="pagination--button" v-if="isFirstPage()">-</div>
                        <div>{{ currentPage + 1 }} / {{ numberOfPages() }}</div>
                        <div id="AutoTest_IP220" class="pagination--button pagination--button__active" v-if="!isLastPage()" v-on:click="nextPage()">></div>
                        <div class="pagination--button" v-if="isLastPage()">-</div>
                    </div>
                    <!--__________-->
                </v-col>
            </v-row>

            <!--Create Sub Activity Dialog-->
            <create-output-dialog 
                v-if="createSubActivityDlg" 
                v-model="createSubActivityDlg" 
                :editMode="editActivityMode" 
                :editPayload="editActivityMode ? currentEditActivity : null"
                :activityList="ActivityList"
                :isSub=true
                @updateActivityList="PreLoader = true, fnGetProjectActivityList()"
                @unloadDialog="fnUnloadOutputDlg()"
            ></create-output-dialog>

            <!-- Add Workflow Dialog -->
            <v-dialog v-model="SelectWorkflowDialog" width="1100" persistent>
                <v-card>
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <img style="width: 70%" :src="Workflow_Tab_icon" />
                        </v-avatar>
                        Add Workflow
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP771" text @click="(SelectWorkflowDialog = false),(checkboxAddWorkflow = []),(selectedAddWorkflow = []),(SelectWorkflowDlgItems = []),(confirmError=false)">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="py-0">
                        <v-container fluid class="pt-0">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-row no-gutters>
                                        <v-col v-if="SelectWorkflowDlgItems.length > 0 && confirmError == false && EnableWorkflow == true" cols="12" md="12" class="pt-0 mb-2">
                                            <h4 class="text-center heading-3">Please select workflow from below</h4>
                                        </v-col>
                                        <v-col v-if="SelectWorkflowDlgItems.length > 0 && confirmError == true && EnableWorkflow == true" cols="12" md="12" class="pt-0 mb-2">
                                            <h4 class="text-center heading-3 red--text">Please select workflow from below *</h4>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col v-if="SelectWorkflowDlgItems.length > 0  && EnableWorkflow == false" cols="12" md="12" class="pt-0 mb-2">
                                            <!--<h4 class="text-center heading-3">You cannot change workflow, Survey Task is Approved</h4>-->
                                            <v-row :justify="justifyCenter">
                                                <v-col md="8" class="text-center">
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
                                                                        You cannot change workflow, Activities & Inputs Task is Approved.
                                                                    </span>
                                                                </p>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                    <v-stepper v-model="SelectWorkflowDlgSteps" class="elevation-0">
                                        <v-stepper-items>
                                            <!-- Select Workflow step-->
                                            <v-stepper-content step="1" class="pa-0">
                                                <!-- if workflow available -->
                                                <v-row no-gutters v-if="SelectWorkflowDlgItems.length > 0">
                                                    <!-- Work flow group chip-->
                                                    <v-col md="12" class="py-0">
                                                        <div class="chip-container mb-2 ml-4">
                                                            <v-chip-group multiple column active-class="primary--text" v-bind:disabled="EnableWorkflow == false">
                                                                <v-chip id="AutoTest_IP772" color="#DBF1FF" v-for="(chipitem,key) in selectedAddWorkflow" :key="chipitem.WFitem.WorkflowID"
                                                                        @click:close="fnRemove(chipitem)" close close-icon="mdi-close" class="ma-2" small v-bind:disabled="EnableWorkflow == false">
                                                                    <v-icon left size="15" color="#707070">mdi-folder-key-network-outline</v-icon>
                                                                    {{ chipitem.WFitem.WfName }}
                                                                </v-chip>
                                                            </v-chip-group>
                                                        </div>
                                                    </v-col>

                                                    <!-- Work flow details card-->
                                                    <v-row class="v-scrolling-div mx-4" style="height:45vh">
                                                        <v-col xs="6" sm="6" md="4" v-for="(addWorkflowItem,index) in SelectWorkflowDlgItems" :key="addWorkflowItem.WorkflowID">
                                                            <v-card class="workspace-card" flat>
                                                                <v-row>
                                                                    <v-col xs="6" sm="6" md="10" class="d-inline-flex pt-0" :ripple="false">
                                                                        <div>
                                                                            <v-avatar size="60"
                                                                                      tile
                                                                                      class="elevation-1 base-border-radius">
                                                                                <v-img :src="Workflow_Icon"
                                                                                       height="40"
                                                                                       width="40"
                                                                                       class="ma-4"></v-img>
                                                                            </v-avatar>
                                                                        </div>
                                                                        <div class="title pt-4 px-2 text-limit">
                                                                            {{ addWorkflowItem.WfName }}
                                                                        </div>
                                                                    </v-col>
                                                                    <v-col xs="2" sm="2" md="2" class="d-inline-flex pt-0 text-right">
                                                                        <!-- <v-checkbox color="success-check-box" v-model="checkboxAddWorkflow[index]"
                                                                        @change="fnCheck(addWorkflowItem,index)" label=""></v-checkbox> -->

                                                                        <input type="radio"
                                                                               class="mt-3"
                                                                               v-if="selectedAddWorkflow.length > 0"
                                                                               style="border: 0px; width: 100%; height: 1.4em;"
                                                                               name="workflow"
                                                                               :checked="selectedAddWorkflow[0].checkboxindex == index ? true : false"
                                                                               @click="(checkboxAddWorkflow[index] = true), fnCheck(addWorkflowItem, index)" />
                                                                        <input type="radio"
                                                                               class="mt-3"
                                                                               style="border: 0px; width: 100%; height: 1.4em;"
                                                                               name="workflow"
                                                                               v-else
                                                                               @click="(checkboxAddWorkflow[index] = true), fnCheck(addWorkflowItem, index)" />
                                                                    </v-col>
                                                                </v-row>
                                                                <!--Description-->
                                                                <!--Description-->
                                                                <v-row>
                                                                    <v-col sm="12"
                                                                           style="display: flex; flex-direction: row"
                                                                           class="py-0"
                                                                           :ripple="false">
                                                                        <span class="heading-5 black--text mt-2">
                                                                            <img :src="Description_icon" />
                                                                        </span>
                                                                        <div class="pt-2 pl-1">
                                                                            <span class="heading-5 black--text">Description</span>
                                                                        </div>
                                                                    </v-col>
                                                                    <v-col sm="12" class="pt-0 pl-0">
                                                                        <p style="height: 24px" class="ml-3">
                                                                            {{ addWorkflowItem.WfDescription.length >= 60 ? addWorkflowItem.WfDescription.substring( 0, 60 ) + "..." : addWorkflowItem.WfDescription }}
                                                                        </p>
                                                                    </v-col>
                                                                </v-row>
                                                                <!-- Levels-->
                                                                <v-row>
                                                                    <v-col class="py-0">
                                                                        <span class="heading-5 black--text ml-1">
                                                                            <img :src="Levels_icon"
                                                                                 style="width: 3.5%" />
                                                                            Levels : {{ addWorkflowItem.WfNoOfLevels }}
                                                                        </span>
                                                                    </v-col>
                                                                </v-row>
                                                                <!-- buttons-->
                                                                <v-row>
                                                                    <v-col>
                                                                        <v-btn id="AutoTest_IP773" color="success-btn elevation-0 enter-btn" dark @click="showDetail(addWorkflowItem, index) ">
                                                                            Show Details
                                                                        </v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                        </v-col>
                                                    </v-row>
                                                    <!--Buttons-->
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4 mt-4 mb-0">
                                                        <Secondary-Button v-if="selectedAddWorkflow.length>0 && EnableWorkflow == true" id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddSelectWorkflow()"></Secondary-Button>
                                                        <Secondary-Button v-else id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="(confirmError = true)" v-show="EnableWorkflow == true"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP775" class="btn-122x36" title="Cancel" @click.native="(SelectWorkflowDialog = false),(confirmError=false),(checkboxAddWorkflow = []),(selectedAddWorkflow = []),(SelectWorkflowDlgItems = [])"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                                <!-- if no workflow available -->
                                                <v-row no-gutter v-if="SelectWorkflowDlgItems.length == 0">
                                                    <v-col cols="12" sm="12" md="12">
                                                        <v-row justify="center">
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
                                                <!-- if Survey Task is approved at any level we cannot edit the workflow-->
                                                <!--<v-row no-gutter v-if="SelectWorkflowDlgItems.length > 0 && EnableWorkflow == false">
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
                                                                                    You cannot change workflow, Survey Task is Approved
                                                                                </span>
                                                                            </p>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>-->
                                            </v-stepper-content>
                                            <!-- Show Details of Workflow-->
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row>
                                                        <v-col sm="1">
                                                            <v-avatar size="55"
                                                                      tile
                                                                      class="elevation-1 base-border-radius">
                                                                <v-img :src="Workflow_Icon"
                                                                       height="38"
                                                                       width="38"
                                                                       class="ma-4"></v-img>
                                                            </v-avatar>
                                                        </v-col>
                                                        <v-col sm="2" class="pt-8">
                                                            <span style="font-weight: bold; font-size: 18px">
                                                                {{ ViewWorkflowItem.WfName }}
                                                            </span>
                                                        </v-col>
                                                        <v-col style="text-align: end"
                                                               sm="9"
                                                               class="pt-6"
                                                               v-if="
                                                               selectedAddWorkflow.length>
                                                            0 &&
                                                            selectedAddWorkflow[0].checkboxindex ==
                                                            radioSelecIndex
                                                            "
                                                            >
                                                            <v-btn elevation="1"
                                                                   class="success-btn white--text"
                                                                   @click="fnRemove(selectedAddWorkflow[0])" v-bind:disabled="EnableWorkflow == false">Deselect Workflow</v-btn>
                                                        </v-col>
                                                        <v-col style="text-align: end"
                                                               sm="9"
                                                               class="pt-6"
                                                               v-else>
                                                            <v-btn elevation="1"
                                                                   class="success-btn white--text"
                                                                   @click="(checkboxAddWorkflow[radioSelecIndex] = true), fnCheck(ViewWorkflowItem, radioSelecIndex)"
                                                                   v-bind:disabled="EnableWorkflow == false">
                                                                Select Workflow
                                                            </v-btn>
                                                        </v-col>
                                                    </v-row>
                                                    <v-row class="v-scrolling-div" style="overflow-y :hidden;overflow-x :hidden ;height:45vh" justify="center">
                                                        <v-col md="8" v-for="i in ViewWorkflowItem.WfNoOfLevels" :key="i">
                                                            <v-card class="workspace-card py-0" flat>
                                                                <v-row>
                                                                    <v-col class="text-center py-8">
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
                                                                    <v-divider vertical></v-divider>
                                                                    <v-col class="text-left">
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
                                                                                    <v-avatar size="25" class="team-avatar" v-for="(item,key) in ViewWorkflowItem.WfLevelDetails[i-1].usersList.slice(0,3)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                    </v-avatar>
                                                                                    <v-avatar v-if="ViewWorkflowItem.WfLevelDetails[i-1].usersList.length>=4" size="25" class="team-count">
                                                                                        +{{ViewWorkflowItem.WfLevelDetails[i-1].usersList.length-3}}
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
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4 mt-4 mb-0">
                                                        <Secondary-Button v-if="selectedAddWorkflow.length>0 && EnableWorkflow == true" id="AutoTest_IP778" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddSelectWorkflow()"></Secondary-Button>
                                                        <Secondary-Button v-else id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="(confirmError = true)" v-show="EnableWorkflow == true"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP779" class="btn-122x36" title="Previous" @click.native="SelectWorkflowDlgSteps=1"></Default-Button-Outlined>
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

            <!--Assign User Dialog-->
            <v-dialog v-model="AssignUserDialog" width="800">
                <v-card>
                    <v-card-title primary-title="" class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                        </v-avatar>
                        Assign User
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP250" text="" @click="AssignUserDialog=false,fnInitializeAssignUsers(null)">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="pt-4">
                            <v-row>
                                <v-col md="12">
                                    <v-data-table :headers="ProjectUserListTableHeaders"
                                                  :items="filterProjectUserList"
                                                  show-select
                                                  item-key="ID"
                                                  sort-by="Name"
                                                  :search="searchProjectUserList"
                                                  v-model="AssignUser.selectedProjectUserList"
                                                  height="40vh"
                                                  fixed-header>
                                        <template v-slot:top>
                                            <v-text-field label="Search" v-model="searchProjectUserList"
                                                          dense prepend-inner-icon="mdi-magnify"
                                                          outlined></v-text-field>
                                        </template>
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
                                                        {{item.Name}}
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </template>
                                    </v-data-table>
                                </v-col>
                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                    <Secondary-Button id="AutoTest_IP251" class="btn-122x36 mr-2" title="Add" @click.native="fnAssignUser()" v-bind:disabled="AssignUser.selectedProjectUserList.length==0"></Secondary-Button>
                                    <Default-Button-Outlined id="AutoTest_IP252" class="btn-122x36" title="Cancel" @click.native="AssignUserDialog=false,fnInitializeAssignUsers(null)"></Default-Button-Outlined>

                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!--Remove Activity Dialog-->
            <v-dialog v-model="RemoveActivityDialog" width="800">
                <v-card>
                    <v-card-title primary-title="" class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                        </v-avatar>
                        Remove Activities & Inputs
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP253" text="" @click="RemoveActivityDialog=false,RemoveActivityItem=null">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="pt-4">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col v-if="RemoveActivityItem!=null" cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">Are you sure you want to remove this Activities & Inputs ?</h4>
                                            </v-col>
                                            <v-col v-else cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">Are you sure you want to remove below Activities & Inputs ?</h4>
                                            </v-col>
                                            <v-col v-if="RemoveActivityItem!=null">
                                                <h4 class="text-center heading-3">
                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-chart-areaspline</v-icon>
                                                    {{ RemoveActivityItem.ActivityName }}
                                                </h4>
                                            </v-col>
                                            <v-col v-else>
                                                <v-row v-for="selecteditem in selectedActivityList" :key="selecteditem.ActivityId">
                                                    <v-col md="12">
                                                        <h4 class="text-center heading-3">
                                                            {{ selecteditem.ActivityName }}
                                                        </h4>
                                                    </v-col>
                                                </v-row>
                                            </v-col>

                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_IP254" color="secondary elevation-0" class="mr-2" @click="fnRemoveActivity()">Remove</v-btn>
                                                <v-btn id="AutoTest_IP255" outlined="" color="grey" @click="RemoveActivityDialog=false,RemoveActivityItem=null">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!-- Created  Activity-->
            <create-output-dialog 
                v-if="createActivityDlg" 
                v-model="createActivityDlg" 
                :editMode="editActivityMode"
                :editPayload="editActivityMode ? currentEditActivity : null"
                :activityList="ActivityList"
                :isSub=false 

                @updateActivityList="PreLoader = true, fnGetProjectActivityList()"
                @unloadDialog="fnUnloadOutputDlg()"
            ></create-output-dialog>

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
        </v-container>
    </div>
</template>
<script type="text/javascript">
    import TreeTable from "vue-tree-table-component";
    import CreateOutputDialog from './ProjectComponents/CreateOutputDialog.vue'

    import SuccessButton from "@/components/success-button.vue";
    import SecondaryButton from "@/components/secondary-button.vue";
    import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
    import PreLoader from "@/components/pre-loader.vue";
    import ButtonLight24x24 from '@/components/button-light-24x24.vue'
    import objUtils from '../../utils.js';

    var objManageProject;
    async function importManageProjectscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then((mod) => {
                objManageProject = new mod.ManageProject();
            }),
        ]);
    }
    var objManageProjectActivity;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then((mod) => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            }),
        ]);
    }
    var objManageProjectSurvey;
    async function importscriptSurvey() {
        return Promise.all([
            import("../../BL/ProjectSurvey.js").then((mod) => {
                objManageProjectSurvey = new mod.ProjectSurvey();
            }),
        ]);
    }

    export default {
        name: "DefaultTableTree",
        data() {
            return {
                justifyCenter: "center",
                outputId: 0,
                screenAccess: 0,
                RWAccess: 0,
                SelectWorkflowDialog: false,
                EnableWorkflow: true,
                confirmError: false,
                Description_icon: require("@/assets/ProjectWorkflow/Description_icon.svg"),
                Levels_icon: require("@/assets/ProjectWorkflow/Levels_icon.svg"),
                Workflow_Icon: require("@/assets/ProjectWorkflow/Workflow_Icon.svg"),
                Workflow_Tab_icon: require("@/assets/ProjectWorkflow/Workflow_Tab_icon.svg"),
                alphanumericRules: [
                    (v) => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || "Please enter valid name",
                    (value) => !!value || "Required field.",
                ],
                valid: true,
                percentageRules: [
                    (v) =>
                        !v ||
                        /^\d{1,2}\.\d{1,2}$|^\d{1,3}$/.test(v) ||
                        "Please enter valid percentage value",
                    (v) => v <= 100 || "% should be equal or less than 100",
                ],
                PreLoader: false,
                //Table Pagination
                currentPage: 0,
                recordsPerPage: 5,
                //General parameters
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                },
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                ProjectStartDate: "",
                ProjectEndDate: "",
                mStartDate: false,
                mEndDate: false,
                parentActivityStartDate: "",
                parentActivityEndDate: "",
                selectedItem: "",
                items: [
                    { text: "This month" },
                    { text: "This quarter" },
                    { text: "This Year" },
                ],
                filterContent: false,
                dateRangeHeaderName: 'Date Range',
                dateRangeMenu: false,
                organisationMenu: false,
                workflowMenu: false,
                assignedUserMenu: false,
                filterChipsList: [],
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    StartDate: "",
                    EndDate: "",
                    CountryID: 0,
                    StateID: 0,
                    DistrictID: 0,
                    ProjectTags: "",
                },

                //Listing parameters
                ActivityList: [],
                filterActivityList: [],
                selectedActivityList: [],
                SearchActivity: "",
                ActivityListHeader: [
                    { label: "Name", id: "ActivityName", props: "ActivityName" },
                    { label: "Start Date", id: "StartDate", props: "StartDate" },
                    { label: "End Date", id: "EndDate", props: "EndDate" },
                    { label: "Last updated", id: "UpdatedDate", props: "UpdatedDate" },
                    { label: "Weightage", id: "WeightagePercentage", props: "WeightagePercentage", },
                    { label: "Progress", id: "Progress", props: "Progress" },
                    { label: "Users", id: "ActivityUsers", props: "ActivityUsers" },
                    { label: "Action", id: "action", props: "action" },
                ],

                //parameters used for both Add and Edit Activity
                AddEditActivityDialog: false,
                AddEditActivityDialogHeader: "",
                AddEditActivity: {
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
                    SurveyLogic: 0,
                    SurveyTarget: 0,
                    SurveyTarget1: "",
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
                    PeriodicityMonthDateDay: 0, //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                    BeneficiaryTypeQstnID: 0,
                    OutPutActivityUsers: []
                },

                AddEditActivityNotifyPopUp: false,
                AddEditActivityUserPopUp: false,
                AddEditActivityExpenseTagStepper: 1,
                AddEditActivityExpenseTagPopUp: false,
                AddEditActivityNewExpenseTagName: "",
                AddEditActivitySearchExpenseTag: "",
                AddEditActivityEditExpenseTagItem: { TagID: 0, TagName: "" },
                AddEditActivityPeriodicityDatePicker: false,
                //AddEditActivityPeriodicityDate: "",
                AddEditActivityNewTagMenu: false,
                rules: {
                    required: (value) => !!value || "Required.",
                },
                ActivityTypeItems: ["Quantitative", "Qualitative"], //['Target', 'Non Target'],
                SurveyList: [],
                ProjectWorkflowList: [],
                PeriodicityList: [],
                PeriodicityMonthListDate: [],
                PeriodicityMonthList: [],
                PeriodicityWeekList: [],
                ProjectExpenseTagList: [],
                filterProjectExpenseTagList: [],

                //parameters used in AddActivity, EditActivity and Assign Users
                ProjectUserListTableHeaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                ProjectUserList: [],
                filterProjectUserList: [],
                selectedProjectUserList: [],
                searchProjectUserList: "",
                selectedNotifyUsers: [],

                //Remove Activity parameters
                RemoveActivityDialog: false,
                RemoveActivityItem: null,

                //Assign Users parameters
                AssignUserDialog: false,
                AssignUser: {
                    ActivityId: 0,
                    selectedProjectUserList: [],
                },

                Commoncheck: false,
                PeriodicityChk: "0001-01-01",
                expenseTag: false,
                //tagSwatches: [
                //  ["#34C900", "#FFC914"],
                //  ["#1FC2C8", "#0F36F6"],
                //  ["#EC4554", "#9509FF"],
                //],
                stepper: 1,
                LogicItems: [],
                LogicItemsPrime: [],
                LogicItemsPrimeNew: [],
                //['Sum of Unique Reponses', 'Conditionality', 'Cumallaive Sum'],
                EditActivityDialog: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                // Create Sub Activity

                //Create Sub Activity parameters
                CreateSubActivityDailog: false,
                CreateSubActivitySteps: 0,
                ParentActivityList: [],
                CopyParentActivitySwitch: false,
                CopyBasicDetails: false,
                CopyPaymnetDetails: false,
                CopyActivityType: false,
                CopyAssignment: false,
                SubActivity: {
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
                    SurveyTarget: 0,
                    SurveyTarget1: "",
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
                    PeriodicityMonthDateDay: 0, //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                    BeneficiaryTypeQstnID: 0
                },
                SubActivityStartDatePicker: false,
                SubActivityEndDatePicker: false,
                SubActivityNotifyPopUp: false,
                SubActivityUserPopUp: false,

                SubActivityExpenseTagStepper: 1,
                SubActivityExpenseTagPopUp: false,
                SubActivityNewExpenseTagName: "",
                //SubActivityNewExpenseTagID: -1,
                SubActivitySearchExpenseTag: "",
                SubActivityEditExpenseTagItem: { TagID: 0, TagName: "" },
                SubActivityPeriodicityDatePicker: false,
                //SubActivityPeriodicityDate: "",
                HidePreviousButton: true,

                SectionList: [],
                QstnList: [],
                //ResponseList: [],
                logicAnsType: "Dropdown",
                lstLogicAnswers: [],
                logicType: "",

                // Tags ------------
                SubActivityNewTagMenu: false,
                SearchTagDetails: "",
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                NewTagName: "",
                color: "",
                swatches: [
                    ["#00FF00", "#FF0000"],
                    ["#F0EB06", "#4B0D64"],
                    ["#00FFFF"],
                    ["#0000FF"],
                ],
                allTags: false,
                EditTagName: "",
                //tableDataTest: [],
                step: 50,
                ParentActivityOfSubAct: "",
                userimage: "",
                SelectWorkflowDlgItems: [],
                checkboxAddWorkflow: [],
                selectedAddWorkflow: [],
                radioSelecIndex: "",
                SelectWorkflowDlgSteps: [],
                ViewWorkflowItem: [],


                //Filtration
                FiterStartDate: "",
                FiterEndDate: "",
                //EnableWorkflow: true,
                BeneficiaryTypeQstnID: 0,
                lstBeneficiaryType: [],
                SectionORBeneficiary: '',
                OutputErrorMessage: "",
                SurveyErrorMessage: "",
                SurveyPrimeLogicList: [],
                SurveyLogicPrime: 0,
                SurveyLogicPrimeAnswer: "",
                SubActivityCount: 0,
                SubActivityTotalWeightage: 0,
                IsNumericTextQuestion: 1,
                SurveyLogicPrimeAnswerDB: false,
                SurveyLogicCondition: "",
                lstConditions:[],
                CascadingList: [],
                CascadingQuestionOptions: [],
                lstCountry: [],
                lstStates: [],
                lstDistrict: [],
                lstSubDistrict: [],
                cascadingSelection: false,
                createActivityDlg: false,
                editActivityMode: false,
                currentEditActivity: [],
                createSubActivityDlg: false,
            };
        },

        components: {
            "Success-Button": SuccessButton,
            "Secondary-Button": SecondaryButton,
            "Default-Button-Outlined": DefaultButtonOutlined,
            "Pre-Loader": PreLoader,
            'Button-Light-24x24': ButtonLight24x24,
            TreeTable,
            CreateOutputDialog,
        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            await importscript();
            this.PreLoader = true;
            await importManageProjectscript();
            await importscriptSurvey();
            await this.fnGetProjectDetailsByID();
            await this.fnGetProjectActivityList();
            this.PreLoader = false;
            // await this.fnGetProjectSurveyList();
            // await this.fnGetUsersListByGroupId();
            // await this.fnGetAllProjectTags();
            // await this.fnGetExpenseTagList();
            // await this.fnGetProjectWorkflowsList();
            // await this.fnGetPeriodicityList();
            // await this.fnGetLogicType();
            // await this.fnCreateLogicCondition();
        },
        computed: {
            //Pagination
            paginatedData: function () {
                const firstIndexOnPage = this.currentPage * this.recordsPerPage;
                const firstIndexOnNextPage = (this.currentPage + 1) * this.recordsPerPage;
                return this.filterActivityList.slice(
                    firstIndexOnPage,
                    firstIndexOnNextPage
                );
            },

            computedPeriodicityDate() {
                return this.formatDate(this.AddEditActivity.PeriodicityOnceDate);
            },
            computedStartDate() {
                return this.formatDate(this.AddEditActivity.StartDate);
            },
            computedEndDate() {
                return this.formatDate(this.AddEditActivity.EndDate);
            },
            computedSubStartDate() {
                return this.formatDate(this.SubActivity.StartDate);
            },
            computedSubEndDate() {
                return this.formatDate(this.SubActivity.EndDate);
            },
            computedSubPeriodicityDate() {
                return this.formatDate(this.SubActivity.PeriodicityOnceDate);
            },
        },
        methods: {
            /// <summary>
            ///  fnGetWorkflowsListByProjectID :- Function used to fetch workflows list by ProjectID from the action dropdown menu
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnGetWorkflowsListByProjectID(item) {
                this.CheckSureveyTaskIsAprovedWhileUpdate(item.ActivityId);
                this.outputId = item.ActivityId;
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objManageProject.GetWorkflowsListByProjectID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectWorkflowDlgItems = result.data;
                    var ID;
                    var temp = "";

                    for (ID = 0; ID <= this.SelectWorkflowDlgItems.length - 1; ID++) {
                        if (this.SelectWorkflowDlgItems[ID].WorkflowID == item.WorkflowId) {
                            temp = this.SelectWorkflowDlgItems.filter((x) => x.WorkflowID == item.WorkflowId);
                        }
                        if (temp.length > 0) {
                            this.selectedAddWorkflow.push({ checkboxindex: ID, WFitem: temp[0], });
                            this.checkboxAddWorkflow[ID] = true;
                        }
                        temp = [];
                    }
                    this.SelectWorkflowDlgSteps = 1;
                }
            },

            /// <summary>
            ///  fnCheck :- Function used to check workflows
            /// </summary>
            /// <param name="item"></param>
            /// <param name="index"></param>
            /// <returns> </returns>
            fnCheck(item, index) {
                var i;
                var preval = this.checkboxAddWorkflow[index];
                this.selectedAddWorkflow = [];
                for (i = 0; i <= this.SelectWorkflowDlgItems.length - 1; i++) {
                    this.checkboxAddWorkflow[i] = false;
                }
                if (preval) {
                    this.checkboxAddWorkflow[index] = true;
                    this.selectedAddWorkflow.push({ checkboxindex: index, WFitem: item });
                    this.confirmError = false
                } else {
                    if (this.selectedAddWorkflow.length > 0) {
                        this.selectedAddWorkflow = this.selectedAddWorkflow.filter((x) => x.WFitem.WorkflowID != item.WorkflowID);
                    }
                }
            },

            /// <summary>
            ///  showDetail :- Function used to show details of workflow items
            /// </summary>
            /// <param name="data"></param>
            /// <param name="index"></param>
            /// <returns> </returns>
            showDetail(data, index) {
                this.SelectWorkflowDlgSteps = 2;
                this.ViewWorkflowItem = data;
                this.radioSelecIndex = index;
            },

            /// <summary>
            ///  fnAddSelectWorkflow :- Function used to add selected workflow to activity
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddSelectWorkflow() {
                var data = new FormData();
                data.append("WorkFlowID", this.selectedAddWorkflow[0].WFitem.WorkflowID);
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("OutputId", this.outputId);

                var result = await objManageProjectActivity.UpdateOutputWorkflow(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "Success") {
                    this.Msg = "Successfully added the workflow you selected"; //alert("Successfully added the workflow you selected");
                    this.successSnackbarMsg = true;
                    this.SelectWorkflowDialog = false;
                    this.SelectWorkflowDlgSteps = 1;
                    this.SelectWorkflowDlgItems = [];
                    this.checkboxAddWorkflow = [];
                    this.selectedAddWorkflow = [];
                    this.confirmError = false
                    this.outputId = 0;
                    await this.fnGetProjectActivityList();
                }
            },

            /// <summary>
            ///  CheckSureveyTaskIsAprovedWhileUpdate :- Function used to check survey task is approved or not while update
            /// </summary>
            /// <param name="OutputID"></param>
            /// <returns> </returns>
            async CheckSureveyTaskIsAprovedWhileUpdate(OutputID) {
                this.EnableWorkflow = true;
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("TypeID", OutputID);
                data.append("Type", "Activity")
                var result = await objManageProjectSurvey.CheckSureveyTaskIsAprovedWhileUpdate(data);
                if (result.status == "Failed") {
                    this.EnableWorkflow = false;
                }
            },

            /// <summary>
            ///  fnRemove :- Function used to remove selected workflow from add workflow page
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnRemove(item) {
                this.checkboxAddWorkflow[item.checkboxindex] = false;
                this.selectedAddWorkflow = this.selectedAddWorkflow.filter((x) => x.WFitem.WorkflowID != item.WFitem.WorkflowID);
            },

            /// <summary>
            ///  commonGetColor :- Function used to get color
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },

            //Pagination

            /// <summary>
            ///  nextPage :- Function used to go to next page if the current page is not last page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            nextPage() {
                this.currentPage = this.currentPage + 1;
            },

            /// <summary>
            ///  prevPage :- Function used to go to prevoius page if the current page is not first page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            prevPage() {
                this.currentPage = this.currentPage - 1;
            },

            /// <summary>
            ///  numberOfPages :- Function used to show total number of pages under the table listed
            /// </summary>
            /// <param name=""></param>
            /// <returns> number of pages </returns>
            numberOfPages() {
                return Math.ceil(this.filterActivityList.length / this.recordsPerPage);
            },

            /// <summary>
            ///  isFirstPage :- Function used to check if the current page is first page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            isFirstPage() {
                return this.currentPage == 0;
            },

            /// <summary>
            ///  isLastPage :- Function used to check if the current page is last page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            isLastPage() {
                const firstIndexOnNextPage = (this.currentPage + 1) * this.recordsPerPage;
                return firstIndexOnNextPage > this.filterActivityList.length;
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
            ///  CheckFn :- Function used to select all activities in the table, when clicking on the checkbox
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CheckFn() {
                this.fnGetProjectActivityList();
            },

            /// <summary>
            ///  fnGetProjectActivityList :- Function used to fetch activity details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectActivityList() {
                var result = await objManageProjectActivity.GetProjectActivityList(this.projectInfo.ProjectId, this.Commoncheck);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterActivityList = result.data;
                    //this.filterActivityList[0].children = [{ ActivityName: 'Activity 1.1'}];
                    this.ActivityList = result.data;
                    for (var i = 0; i < this.filterActivityList.length; i++) {
                        if (this.filterActivityList[i].children != null &&
                            this.filterActivityList[i].children != undefined &&
                            this.filterActivityList[i].children.length > 0) {
                            this.filterActivityList[i].SubActivityExists = 1;
                        } else {
                            this.filterActivityList[i].SubActivityExists = 0;
                        }
                    }

                    this.ParentActivityList = this.ActivityList.filter((a) => a.ParentActivityId == 0);
                } else {
                    this.Msg = "Failed to get output list of Project " + this.projectInfo.ProjectName;
                    this.errorSnackbarMsg = true;
                }
                
                this.PreLoader = false
            },

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter activity based on date range option
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {
                this.filterChipsList = [];
                this.filterActivityList = this.ActivityList;

                //date range filter
                if (this.FiterStartDate != '' && this.FiterEndDate != '') {
                    this.filterChipsList.push({ ChipID: 777, ChipName: objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate) });
                    this.dateRangeHeaderName = objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate);
                    this.filterActivityList = this.filterActivityList.filter(x => x.StartDate.substr(0, 10) >= this.FiterStartDate && x.EndDate.substr(0, 10) <= this.FiterEndDate);
                } else {
                    this.dateRangeHeaderName = "Date Range";
                }
            },

            /// <summary>
            ///  fnChipClose :- Function used to close the filter chip list
            /// </summary>
            /// <param name="chipitem"></param>
            /// <returns> </returns>
            fnChipClose(chipitem) {
                this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);
                if (chipitem.ChipName == objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate)) {
                    this.FiterStartDate = "";
                    this.FiterEndDate = "";
                    this.dateRangeHeaderName = "Date Range";
                }
                this.fnClickFilterDone();
            },

            /// <summary>
            ///  fnGetGrandChildrenActivity :- Function used to fetch sub activity details based on ActivityId
            /// </summary>
            /// <param name="actitem"></param>
            /// <returns> </returns>
            async fnGetGrandChildrenActivity(actitem) {
                if (actitem.isOpen == false) return;
                var child = actitem.children;
                if (child.length > 0) {
                    actitem.SubActivityExists = 1;
                } else {
                    actitem.SubActivityExists = 0;
                }
                for (var i = 0; i < child.length; i++) {
                    var result = await objManageProjectActivity.GetSubActivityListByID(child[i].ActivityId);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        child[i].children = result.data;
                        //alert(child[i].ActivityName + " children length=" + child[i].children.length);
                        for (var j = 0; j < child[i].children.length; j++) {
                            if (child[i].children[j].children != null) {
                                if (child[i].children[j].children.length > 0) {
                                    child[i].children[j].SubActivityExists = 1;
                                }
                                else {
                                    child[i].children[j].SubActivityExists = 0;
                                }
                            }
                            else {
                                child[i].children[j].SubActivityExists = 0;
                            }

                        }
                    } else {
                        this.Msg = "Failed to get sub-activity list of Activity " + oneact.ActivityName;
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnGetLogicType :- Function used to fetch logic type
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetLogicType() {
                var result = await objManageProjectActivity.GetLogicType();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    if (result.data != null && result.data != undefined) {
                        this.LogicItems = result.data.filter(x => x.IsPrimeLogic == false);
                        this.LogicItemsPrime = result.data.filter(x => x.IsPrimeLogic == true);
                        this.LogicItemsPrimeNew = result.data.filter(x => x.IsPrimeLogic == true);
                    }
                }
            },
            

            /// <summary>
            ///  GetSectionsBySurveyID :- Function used to fetch sections based on SurveyID
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSectionsBySurveyID(SurveyID) {
                this.AddEditActivity.SurveySectionId = 0;
                this.AddEditActivity.SurveyQuestionId = 0;
                this.AddEditActivity.SurveyLogic = 0;
                this.AddEditActivity.SurveyTarget1 = 0;
                this.AddEditActivity.SurveyTarget = 0;
                var data = new FormData();
                data.append("SurveyID", SurveyID);
                var result = await objManageProjectActivity.GetSectionsBySurveyID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) this.SectionList = result.data;
                }
            },

            /// <summary>
            ///  GetQstnBySectionID :- Function used to fetch questions based on SectionID
            /// </summary>
            /// <param name="SectionID"></param>
            /// <returns> </returns>
            async GetQstnBySectionID(SectionID) {
               
                this.AddEditActivity.SurveyQuestionId = 0;
                this.AddEditActivity.SurveyLogic = 0;
                this.AddEditActivity.SurveyTarget1 = 0;
                this.AddEditActivity.SurveyTarget = 0;
                this.AddEditActivity.BeneficiaryTypeQstnID = 0;

                var data = new FormData();
                data.append("SectionID", SectionID);
                var result = await objManageProjectActivity.GetQstnBySectionID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.QstnList = result.data;
                   
                    if (this.AddEditActivity.SurveyQuestionId > 0) {
                        this.GetResponsesByQstnid(this.AddEditActivity.SurveyQuestionId,0);
                    }
                    if (this.AddEditActivity.BeneficiaryTypeQstnID > 0) {
                        this.GetResponsesByQstnid(0, this.AddEditActivity.BeneficiaryTypeQstnID);
                    }
                }
            },

            /// <summary>
            ///  GetSurveyOrBeneficiaryQuestions :- Function used to fetch survey or beneficiary questions based on SectionID
            /// </summary>
            /// <param name="SectionID"></param>
            /// <returns> </returns>
            async GetSurveyOrBeneficiaryQuestions(SectionID) {
                this.SectionORBeneficiary = this.SectionList.filter(x => x.SectionID == SectionID)[0].SectionORBeneficiary;
                if (this.SectionORBeneficiary == 'Section') {
                    await this.GetQstnBySectionID(SectionID)
                }
                else if (this.SectionORBeneficiary == 'Beneficiary') {
                    var outSurveyId = 0;
                    if (this.SubActivity.SurveyId > 0) {
                        outSurveyId = this.SubActivity.SurveyId;
                    } else {
                        outSurveyId = this.AddEditActivity.SurveyId;
                    }
                    await this.GetSurveyLinkedBeneficairyQuestionDetails(outSurveyId);
                }
            },


            /// <summary>
            ///  GetResponsesByQstnid :- Function used to fetch responses based on Qstnid
            /// </summary>
            /// <param name="Qstnid"></param>
            /// <returns> </returns>
            async GetResponsesByQstnid(Qstnid, BenQstnid) {
       
                if (Qstnid > 0 || BenQstnid > 0) {
                   
                    if (Qstnid > 0 ) {
                        this.logicAnsType = this.QstnList.filter((x) => x.QuestionID == Qstnid)[0].QuestionType;
                    } else if (BenQstnid > 0) {
                        this.logicAnsType = this.lstBeneficiaryType.filter((x) => x.QuestionID == BenQstnid)[0].AnswerType;   
                    }

                    if (this.logicAnsType != "Numeric" && this.logicAnsType != "Date Picker") {
                        this.LogicItemsPrime = this.LogicItemsPrimeNew.filter(x => x.IsNonNumeric == true);
                    } else {
                        this.LogicItemsPrime = this.LogicItemsPrimeNew;
                    }

                    this.SurveyLogicPrimeAnswer = "";

                    if (this.logicAnsType == "Single Choice" || this.logicAnsType == "Multi-Choice" || this.logicAnsType == "Dropdown" || this.logicAnsType == "Likert Scale") {
                        this.IsNumericTextQuestion = 2;
                        var result = "";
                        if (Qstnid > 0) {
                            result = await objManageProjectActivity.GetLogicAnswers(Qstnid);
                        } else if (BenQstnid > 0) {
                            result = await objManageProjectActivity.GetBeneficiaryQuestionOption(BenQstnid);
                        } else {
                            result = await objManageProjectActivity.GetLogicAnswers(Qstnid);
                        }
                       
                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        } else if (result.data) {
                            this.lstLogicAnswers = result.data;
                        }
                    } else if (this.logicAnsType == "Date Picker") {
                        this.IsNumericTextQuestion = 3;
                    } else if (this.logicAnsType == "Cascading Dropdown") {
                        
                        this.IsNumericTextQuestion = 4;
                        result = await objManageProjectActivity.GetSurveyCascadingList();
                        var qstId = 0;
                        var isBeneficiary = false;

                        if (Qstnid > 0) {
                            qstId = Qstnid;
                        } 

                        if (BenQstnid > 0) {
                            qstId = BenQstnid;
                            isBeneficiary = true;
                        }

                        var result1 = await objManageProjectActivity.GetCascadingHeirarchyList(qstId, isBeneficiary);

                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        } else if (result.data) {
                            // this.lstLogicAnswers = result.data;
                            if (result1.data) {
                                this.CascadingQuestionOptions = result1.data;

                                if (this.CascadingQuestionOptions != null) {
                                    for (var nCnt = 0; nCnt < this.CascadingQuestionOptions.length; nCnt++) {
                                        if (this.CascadingQuestionOptions[nCnt].OptionValue == "Country") {
                                            var lstCountry = result.data.filter(x => x.ItemTable == "Country");   
                                            this.lstCountry = lstCountry.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                            // this.CascadingQuestionOptions[nCnt].CascadingList = this.lstCountry;

                                            if (this.CascadingQuestionOptions[0].OptionValue == "Country") {
                                                this.CascadingQuestionOptions[0].CascadingList = this.lstCountry;
                                            } else {
                                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                                            }
                                        }
                                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "States") {
                                            var lstStates = result.data.filter(x => x.ItemTable == "State");
                                            this.lstStates = lstStates.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });

                                            if (this.CascadingQuestionOptions[0].OptionValue == "States") {
                                                this.CascadingQuestionOptions[0].CascadingList = this.lstStates;
                                            } else {
                                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                                            }
                                        }
                                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "District") {
                                            var lstDistrict = result.data.filter(x => x.ItemTable == "District");
                                            this.lstDistrict = lstDistrict.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                        
                                            if (this.CascadingQuestionOptions[0].OptionValue == "District") {
                                                this.CascadingQuestionOptions[0].CascadingList = this.lstDistrict;
                                            } else {
                                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                                            }
                                        }
                                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "SubDistrict") {
                                            var lstSubDistrict = result.data.filter(x => x.ItemTable == "SubDistrict");
                                            this.lstSubDistrict = lstSubDistrict.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                          
                                            if (this.CascadingQuestionOptions[0].OptionValue == "SubDistrict") {
                                                this.CascadingQuestionOptions[0].CascadingList = this.lstSubDistrict;
                                            } else {
                                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else {
                        this.IsNumericTextQuestion = 1;
                    }
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
                    this.Msg = "Failed to get all expense tag list of Project " + this.projectInfo.ProjectName;
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetProjectWorkflowsList :- Function used to fetch project workflow details based on ProjectId,WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectWorkflowsList() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objManageProjectActivity.GetProjectWorkflowsList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectWorkflowList = result.data;
                }
            },

            /// <summary>
            ///  fnGetPeriodicityList :- Function used to fetch periodicity list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetPeriodicityList() {
                var result = await objManageProjectActivity.GetPeriodicityList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                if ((result.status = "success")) {
                    if (result.data.RptFrequencyList.length > 0) {
                        this.PeriodicityList = result.data.RptFrequencyList;
                    }
                    if (result.data.WeekList.length > 0) {
                        this.PeriodicityWeekList = result.data.WeekList;
                    }
                    if (result.data.MonthList.length > 0) {
                        this.PeriodicityMonthList = result.data.MonthList;
                    }
                    if (result.data.MonthDate.length > 0) {
                        this.PeriodicityMonthListDate = result.data.MonthDate;
                    }
                }
            },

            //Add Activity functions

            /// <summary>
            ///  fnInitializeAddEditActivity :- Function used to initialize activity variables for edit/update activity
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnInitializeAddEditActivity(item) {
                this.AddEditActivity.OutPutActivityUsers = [];
                this.OutputErrorMessage = "";
                this.SurveyErrorMessage = "";
                if (item != null) {
                    this.CheckSureveyTaskIsAprovedWhileUpdate(item.ActivityId);
                    this.AddEditActivity.ActivityId = item.ActivityId;
                    this.AddEditActivity.ParentActivityId = item.ParentActivityId;
                    this.AddEditActivity.ActivityName = item.ActivityName;
                    this.AddEditActivity.Description = item.Description;
                    this.SubActivityCount = item.SubActivityCount;
                    //this.AddEditActivity.StartDate = this.formatDate(item.StartDate);
                    //this.AddEditActivity.EndDate = this.formatDate(item.EndDate);
                    this.AddEditActivity.StartDate = item.StartDate;
                    this.AddEditActivity.EndDate = item.EndDate;
                    this.AddEditActivity.WeightagePercentage = item.WeightagePercentage;
                    if (item.ActivityTags) {
                        item.ActivityTags.forEach((u) => {
                            this.AddEditActivity.ActivityTags.push({ TagID: u.TagID, TagName: u.TagName, });
                        });
                    }
                    this.AddEditActivity.ActivityTags = item.ActivityTags;
                    this.AddEditActivity.ActivityType = item.ActivityType;
                    this.AddEditActivity.SurveyId = item.SurveyId;

                    if (item.SurveyId != 0) {
                        await this.GetSurveyBeneficiaryType(item.SurveyId);
                        //await this.GetSurveyLinkedBeneficairyQuestionDetails(item.SurveyId)
                        //await this.GetSectionsBySurveyID(item.SurveyId);
                    }
                    this.AddEditActivity.BeneficiaryTypeQstnID = item.BeneficiaryTypeQuestionID;
                    if (item.SurveySectionId > 0) {
                        this.SectionORBeneficiary = "Section";
                        this.AddEditActivity.SurveySectionId = item.SurveySectionId;
                    }
                    if (item.BeneficiaryTypeQuestionID > 0) {
                        this.SectionORBeneficiary = "Beneficiary";
                        this.AddEditActivity.SurveySectionId = item.BeneficiaryTypeID;
                    }


                    if (this.AddEditActivity.SurveySectionId != 0) {

                        //await this.GetQstnBySectionID(item.SurveySectionId);
                        await this.GetSurveyOrBeneficiaryQuestions(this.AddEditActivity.SurveySectionId);
                    }
                    this.AddEditActivity.SurveyQuestionId = item.SurveyQuestionId;
               
                    if (item.SurveyQuestionId > 0) {
                        await this.GetResponsesByQstnid(item.SurveyQuestionId,0);
                    }

                    if (item.BeneficiaryTypeQstnID > 0) {
                        await this.GetResponsesByQstnid(0, item.BeneficiaryTypeQstnID);
                    }

                    this.AddEditActivity.SurveyLogic = item.SurveyLogic;
                    if (item.SurveyLogic == 11 || item.SurveyLogic == 1 || item.SurveyLogic == 2 || this.logicAnsType == "Single Line Text" || this.logicAnsType == "Numeric") {
                        this.AddEditActivity.SurveyTarget1 = item.SurveyTarget;
                    } else {
                        this.AddEditActivity.SurveyTarget = parseInt(item.SurveyTarget);
                    }

                    this.AddEditActivity.IsPaymentLinked = item.IsPaymentLinked;
                    this.AddEditActivity.PaymentLinkedBudget = item.PaymentLinkedBudget;
                    if (item.ExpenseTags) {
                        item.ExpenseTags.forEach((u) => {
                            this.AddEditActivity.ExpenseTags.push({
                                TagID: u.TagID,
                                TagName: u.TagName,
                            });
                        });
                    }
                    this.AddEditActivity.ExpenseTagsNotifyUsers = item.ExpenseTagsNotifyUsers;
                    this.AddEditActivity.WorkflowId = item.WorkflowId;
                    this.AddEditActivity.Periodicity = item.Periodicity;
                    this.AddEditActivity.PeriodicityOnceDate = item.PeriodicityOnceDate.substr(0, 10);
                    //this.formatDate(item.PeriodicityOnceDate);
                    this.AddEditActivity.PeriodicityWeek = item.PeriodicityWeek;
                    this.AddEditActivity.PeriodicityMonthDay = item.PeriodicityMonthDay;
                    this.AddEditActivity.PeriodicityMonth = item.PeriodicityMonth;
                    this.AddEditActivity.PeriodicityMonthDateDay = item.PeriodicityMonthDateDay;
                    this.AddEditActivity.ActivityUsers = item.ActivityUsers;
                    this.selectedProjectUserList = item.ActivityUsers;
                    this.AddEditActivity.SubActivityExists = item.SubActivityExists;
                 
                    if (item.OutPutQuantitativeLogic != null && item.OutPutQuantitativeLogic != undefined && item.OutPutQuantitativeLogic.length > 0) {
                        for (var i = 0; i < item.OutPutQuantitativeLogic.length; i++) {
                            const obj = new Object();
                            obj.QuestionId = item.OutPutQuantitativeLogic[i].QuestionId;
                            obj.SectionId = item.OutPutQuantitativeLogic[i].SectionId;
                            obj.SurveyId = item.OutPutQuantitativeLogic[i].SurveyId;
                            obj.QuestionName = item.OutPutQuantitativeLogic[i].QuestionName;
                            obj.SectionName = item.OutPutQuantitativeLogic[i].SectionName;
                            obj.SurveyName = item.OutPutQuantitativeLogic[i].SurveyName;
                            obj.BeneficiaryTypeQstnID = item.OutPutQuantitativeLogic[i].BeneficiaryTypeQstnID;
                            obj.SurveyPrimeLogic = [];
                           
                            if (item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic != null && item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic != undefined && item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic.length > 0) {
                                for (var k = 0; k < item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic.length; k++) {
                                    const obj1 = new Object();

                                    obj1.SurveyLogicPrimeID = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrime;
                                    obj1.SurveyLogicPrime = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrimeType;
                                    obj1.SurveyLogicPrimeAnswer = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrimeAnswer;
                                    obj1.SurveyLogicCondition = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicCondition;
                                    obj1.id = k + 1;

                                    obj.SurveyPrimeLogic.push(obj1);
                                }
                            }
                            this.AddEditActivity.OutPutActivityUsers.push(obj);
                        }
                    }
                } else {
                    this.AddEditActivity.ActivityId = 0;
                    this.AddEditActivity.ParentActivityId = 0;
                    this.AddEditActivity.ActivityName = "";
                    this.AddEditActivity.Description = "";
                    this.AddEditActivity.StartDate = "";
                    this.AddEditActivity.EndDate = "";
                    this.AddEditActivity.WeightagePercentage = 0;
                    this.AddEditActivity.ActivityTags = [];
                    this.AddEditActivity.ActivityType = "";
                    this.AddEditActivity.SurveyId = 0;
                    this.AddEditActivity.SurveySectionId = 0;
                    this.AddEditActivity.SurveyQuestionId = 0;
                    this.AddEditActivity.SurveyResponseId = 0;
                    this.AddEditActivity.SurveyLogic = "";
                    this.AddEditActivity.SurveyTarget = 0;
                    this.AddEditActivity.SurveyTarget1 = "";
                    this.AddEditActivity.IsPaymentLinked = false;
                    this.AddEditActivity.PaymentLinkedBudget = 0;
                    this.AddEditActivity.ExpenseTags = [];
                    this.AddEditActivity.ExpenseTagsNotifyUsers = [];
                    this.AddEditActivity.WorkflowId = "";
                    this.AddEditActivity.Periodicity = 0;
                    this.AddEditActivity.PeriodicityOnceDate = "";
                    this.AddEditActivity.PeriodicityWeek = 0;
                    this.AddEditActivity.PeriodicityMonthDay = 0;
                    this.AddEditActivity.PeriodicityMonth = 0;
                    this.AddEditActivity.PeriodicityMonthDateDay = 0;
                    this.AddEditActivity.ActivityUsers = [];
                    this.logicAnsType = "";
                    this.QstnList = [];
                    this.AddEditActivity.SubActivityExists = 0;
                    this.EnableWorkflow = true;
                    this.lstBeneficiaryType = [];
                    this.SectionORBeneficiary = '';
                }
            },

            /// <summary>
            ///  fnAddNewExpenseTag :- Function used to create new expense tag for payment linked activity based on ProjectId and tagname
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnAddNewExpenseTag(type) {
                var tagname = "";
                switch (type) {
                    case "AddSubActivity":
                        tagname = this.SubActivityNewExpenseTagName;
                        break;
                    default:
                        tagname = this.AddEditActivityNewExpenseTagName;
                        break;
                }
                var temp = this.ProjectExpenseTagList.filter((x) => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("TagName", tagname);
                var result = await objManageProjectActivity.AddNewExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        switch (type) {
                            case "AddSubActivity":
                                this.SubActivityNewExpenseTagName = "";
                                break;
                            default:
                                this.AddEditActivityNewExpenseTagName = "";
                                break;
                        }
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
            /// <param name="type"></param>
            /// <returns> </returns>
            fnAddExpenseTagToList(item, type) {
                switch (type) {
                    case "AddSubActivity":
                        if (this.SubActivity.ExpenseTags.length == 0)
                            this.SubActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                        else {
                            var temp = this.SubActivity.ExpenseTags.filter((x) => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.SubActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                            }
                        }
                        break;
                    default:
                        if (this.AddEditActivity.ExpenseTags.length == 0)
                            this.AddEditActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                        else {
                            var temp = this.AddEditActivity.ExpenseTags.filter((x) => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.AddEditActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                            }
                        }
                        break;
                }
            },

            /// <summary>
            ///  fnInitializeEditExpenseTagItem :- Function used to open edit expense tag item box, when clicking on the edit button from the list
            ///  initialize expense tag variables
            /// </summary>
            /// <param name="exptagitem = null"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            fnInitializeEditExpenseTagItem(exptagitem = null, type) {
                switch (type) {
                    case "AddSubActivity":
                        if (exptagitem == null) {
                            this.SubActivityEditExpenseTagItem.TagID = 0;
                            this.SubActivityEditExpenseTagItem.TagName = "";
                        } else {
                            this.SubActivityEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.SubActivityEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                    default:
                        if (exptagitem == null) {
                            this.AddEditActivityEditExpenseTagItem.TagID = 0;
                            this.AddEditActivityEditExpenseTagItem.TagName = "";
                        } else {
                            this.AddEditActivityEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.AddEditActivityEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                }
            },

            /// <summary>
            ///  fnEditExpenseTag :- Function used to edit expense tag item, when clicking on the save button
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnEditExpenseTag(type) {
                var tagid = 0;
                var tagname = "";
                switch (type) {
                    case "AddSubActivity":
                        tagid = this.SubActivityEditExpenseTagItem.TagID;
                        tagname = this.SubActivityEditExpenseTagItem.TagName;
                        break;
                    default:
                        tagid = this.AddEditActivityEditExpenseTagItem.TagID;
                        tagname = this.AddEditActivityEditExpenseTagItem.TagName;
                        break;
                }

                var temp = this.ProjectExpenseTagList.filter((x) => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }

                var data = new FormData();
                data.append("TagID", tagid);
                data.append("TagName", tagname);
                var result = await objManageProjectActivity.EditExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.fnInitializeEditExpenseTagItem(null, type);
                        this.fnGetExpenseTagList();
                    } else {
                        this.Msg = "Failed to edit expense tag.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  IsValidPeriodicityDate :- Function used to check activity periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidPeriodicityDate() {
                if (this.AddEditActivity.StartDate != null) {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) < Date.parse(this.AddEditActivity.StartDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                } else {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) < Date.parse(this.ProjectStartDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                }

                if (this.AddEditActivity.EndDate != null) {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) > Date.parse(this.AddEditActivity.EndDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                } else {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                            this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                }
                return true;
            },

            /// <summary>
            ///  IsValidDate :- Function used to check activity start date and activity end date is valid (between project start date and project end date)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.AddEditActivity.EndDate) < Date.parse(this.AddEditActivity.StartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.StartDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.StartDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) == Date.parse(this.AddEditActivity.StartDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
            },
            fnDefaultWeightage(IsSubActivity) {
             
                if (IsSubActivity) {
                    var startDate = Date.parse(this.SubActivity.StartDate);
                    var endDate = Date.parse(this.SubActivity.EndDate);
                    this.SubActivity.WeightagePercentage = Math.round((endDate - startDate) / (1000 * 60 * 60 * 24));
                } else {
                    var startDate = Date.parse(this.AddEditActivity.StartDate);
                    var endDate = Date.parse(this.AddEditActivity.EndDate);
                    this.AddEditActivity.WeightagePercentage = Math.round((endDate - startDate) / (1000 * 60 * 60 * 24));
                }
                //debugger;
                //alert(Math.round((endDate - startDate) / (1000 * 60 * 60 * 24)));
            },

            /// <summary>
            ///  IsValidSubActivityPeriodicityDate :- Function used to check sub activity periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidSubActivityPeriodicityDate() {
                if (this.SubActivity.StartDate != null) {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.SubActivity.PeriodicityOnceDate) < Date.parse(this.SubActivity.StartDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                } else {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.SubActivity.PeriodicityOnceDate) < Date.parse(this.parentActivityStartDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be greater than parent activity Start Date - " + this.formatDate(this.parentActivityStartDate.substr(0, 10));
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                }

                if (this.SubActivity.EndDate != null) {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.SubActivity.PeriodicityOnceDate) > Date.parse(this.SubActivity.EndDate)) {
                            this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                } else {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) != Date.parse(this.PeriodicityChk)) {
                        if (Date.parse(this.SubActivity.PeriodicityOnceDate) > Date.parse(this.parentActivityEndDate.substr(0, 10))) {
                            this.Msg = "Invalid Date : Report Frequency Date should be less than parent activity End Date- " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                            this.infoSnackbarMsg = true;
                            this.FrmDate = "";
                            return;
                        }
                    }
                }
                return true;
            },

            /// <summary>
            ///  IsValidSubActivityDate :- Function used to check sub activity start date and activity end date is valid (between parent activity start date and parent activity end date)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidSubActivityDate() {
                if (Date.parse(this.SubActivity.EndDate) < Date.parse(this.SubActivity.StartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.SubActivity.StartDate) < Date.parse(this.parentActivityStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Parent activity Start Date - " + this.formatDate(this.parentActivityStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.SubActivity.StartDate) > Date.parse(this.parentActivityEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Parent activity End Date - " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) > Date.parse(this.parentActivityEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than parent activity End Date - " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) < Date.parse(this.parentActivityStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than parent activity Start Date (" + this.formatDate(this.parentActivityStartDate.substr(0, 10)) + ")";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) == Date.parse(this.SubActivity.StartDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
            },

            /// <summary>
            ///  fnAddActivity :- Function used for AddActivity, EditActivity, AddSubActivity and EditSubActivity
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnAddActivity(type) {
                var SubActivityTotalWeightage = 0;

                if (this.SubActivityTotalWeightage != undefined && this.SubActivityTotalWeightage != null && this.SubActivityTotalWeightage != "") {
                    SubActivityTotalWeightage =  parseFloat(this.SubActivityTotalWeightage);
                } 
             
                var item;
                if (this.$refs.Form.validate() === true) {
                    switch (type) {
                        case "AddSubActivity":
                            item = this.SubActivity;
                            var totalWeightage = SubActivityTotalWeightage + parseFloat(item.WeightagePercentage);
                           
                            //if (totalWeightage > 1) {
                            //    if (!confirm("Total Weightage exceeds 1. So calculation may be incorrect. Do you want to proceed?")) {
                            //        return false;
                            //    }
                            //}
                            
                            if (item.Periodicity == 2) {
                                if (!this.IsValidSubActivityPeriodicityDate()) {
                                    this.Msg = " Invalid PeriodicityDate ";
                                    this.infoSnackbarMsg = true;
                                    this.SubActivity.PeriodicityOnceDate = "";
                                    return;
                                }
                            }

                            break;
                        default:
                            item = this.AddEditActivity;
                            var totalWeightage = SubActivityTotalWeightage + parseFloat(item.WeightagePercentage);
                            //if (totalWeightage > 1) {
                            //    if (!confirm("Total Weightage exceeds 1. So calculation may be incorrect. Do you want to proceed?")) {
                            //        return false;
                            //    }
                            //}

                            if (item.Periodicity == 2) {
                                if (!this.IsValidPeriodicityDate()) {
                                    this.Msg = " Invalid PeriodicityDate ";
                                    this.infoSnackbarMsg = true;
                                    this.AddEditActivity.PeriodicityOnceDate = "";
                                    return;
                                }
                            }
                            break;
                    }
                    if (this.ActivityList.length > 0 && item.ActivityId == 0) {
                        if (this.ActivityList.find((x) => x.ActivityName == item.ActivityName)) {
                            this.Msg = "ActivityName you entered already exists. Please enter a new activity name.";
                            this.infoSnackbarMsg = true;
                            return;
                        }
                    }

                    this.PreLoader = true;
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
                    if (item.ActivityType == "Quantitative") {
                       
                        if (this.SectionORBeneficiary == 'Beneficiary') {
                            data.append("BeneficiaryTypeQuestionID", item.BeneficiaryTypeQstnID);
                            data.append("SurveySectionId", 0);
                            data.append("SurveyQuestionId", 0);
                        }
                        else if (this.SectionORBeneficiary == 'Section') {
                            data.append("BeneficiaryTypeQuestionID", 0);
                            data.append("SurveySectionId", item.SurveySectionId);
                            data.append("SurveyQuestionId", item.SurveyQuestionId);
                        }

                        data.append("SurveyResponseId", item.SurveyResponseId);
                        data.append("SurveyLogic", item.SurveyLogic);
                     
                        if (item.SurveyLogic == 1 || item.SurveyLogic == 2 || this.logicAnsType == "Single Line Text" || this.logicAnsType == "Numeric") {
                            data.append("SurveyTarget", item.SurveyTarget1);
                        } else {
                            if (item.SurveyTarget > 0) {
                                data.append("SurveyTarget", item.SurveyTarget);
                            } else {
                                data.append("SurveyTarget", item.SurveyTarget1);
                            }
                        }

                        if (this.AddEditActivity.OutPutActivityUsers != null && this.AddEditActivity.OutPutActivityUsers.length > 0) {
                            for (var i = 0; i < this.AddEditActivity.OutPutActivityUsers.length; i++) {
                                data.append('OutPutQuantitativeLogic[' + i + '].QuestionId', this.AddEditActivity.OutPutActivityUsers[i].QuestionId);
                                data.append('OutPutQuantitativeLogic[' + i + '].SectionId', this.AddEditActivity.OutPutActivityUsers[i].SectionId);
                                data.append('OutPutQuantitativeLogic[' + i + '].SurveyId', this.AddEditActivity.OutPutActivityUsers[i].SurveyId);
                                data.append('OutPutQuantitativeLogic[' + i + '].BeneficiaryTypeQstnID', this.AddEditActivity.OutPutActivityUsers[i].BeneficiaryTypeQstnID);

                                if (this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic != null && this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic != undefined && this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic.length > 0) {
                                    for (var k = 0; k < this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic.length; k++) {
                                        data.append('OutPutQuantitativeLogic[' + i + '].lstSurveyPrimeLogic[' + k + '].SurveyLogicPrime', this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic[k].SurveyLogicPrimeID);
                                        data.append('OutPutQuantitativeLogic[' + i + '].lstSurveyPrimeLogic[' + k + '].SurveyLogicPrimeAnswer', this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic[k].SurveyLogicPrimeAnswer);
                                        data.append('OutPutQuantitativeLogic[' + i + '].lstSurveyPrimeLogic[' + k + '].SurveyLogicCondition', this.AddEditActivity.OutPutActivityUsers[i].SurveyPrimeLogic[k].SurveyLogicCondition);
                                    }
                                }
                            }
                            this.OutputErrorMessage = "";
                        } else {
                            this.OutputErrorMessage = "Please add atleast One survey Questions";
                            this.PreLoader = false;
                            return false;
                        }

                    } else {
                        data.append("BeneficiaryTypeQuestionID", 0);
                        data.append("SurveySectionId", 0);
                        data.append("SurveyQuestionId", 0);
                        data.append("SurveyResponseId", 0);
                        data.append("SurveyLogic", "");
                        data.append("SurveyTarget", "");
                        if (this.AddEditActivity.SurveyId <= 0) {
                            this.SurveyErrorMessage = "Field Required";
                            this.PreLoader = false;
                            return false;
                        } else {
                            this.SurveyErrorMessage = "";
                        }
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

                    switch (type) {
                        case "AddSubActivity":
                            this.CreateSubActivityDailog = false;
                            this.CreateSubActivitySteps = 1;
                            break;
                        default:
                            this.AddEditActivityDialog = false;
                            break;
                    }


                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else {
                        if (result.status == "success") {
                            if (type == "AddSubActivity") {
                                var result = await objManageProjectActivity.GetSubActivityListByID(item.ParentActivityId);

                                if (result.status == "Unhandled Exception") {
                                    this.$router.push({ name: "error" });
                                } else if (result.data) {
                                    if (this.ParentActivityOfSubAct != "") {
                                        this.ParentActivityOfSubAct.children = result.data;
                                        this.ParentActivityOfSubAct.isOpen = false;
                                    } else {
                                        await this.fnGetProjectActivityList();
                                    }
                                } else {
                                    this.Msg = "Failed to get sub-activity list of Activity " + oneact.ActivityName;
                                    this.errorSnackbarMsg = true;
                                }
                                this.Msg = "Sub Activity created successfully.";
                                this.successSnackbarMsg = true;
                                this.fnInitializeSubActivity(null);
                                this.$refs.Form.resetValidation()
                            } else if (item.ActivityId > 0) {
                                this.Msg = "Activity Successfully updated";
                                this.successSnackbarMsg = true;
                                this.fnInitializeAddEditActivity(null);
                                await this.fnGetProjectActivityList();
                            } else {
                                this.Msg = "Activity Successfully added";
                                this.successSnackbarMsg = true;
                                this.fnInitializeAddEditActivity(null);
                                await this.fnGetProjectActivityList();
                                this.$refs.Form.resetValidation()
                            }
                        } else if (result.status == "PeriodicityFailed") {
                            this.Msg = "Periodicity Failed";
                            this.errorSnackbarMsg = true;
                            this.AddEditActivityDialog = true;
                        } else {
                            this.Msg = "Failed to add an activity.";
                            this.errorSnackbarMsg = true;
                        }
                        this.PreLoader = false;
                    }
                }
            },

            /// <summary>
            ///  fnAddRemove :- Function used to remove activity as bulk
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnAddRemove(item) {
                if (item.checkbox) {
                    //add it to the list when checkbox selected
                    this.selectedActivityList.push(item);
                } else {
                    //remove it from the list when checkbox de-selected
                    if (this.selectedActivityList.length > 0) {
                        this.selectedActivityList = this.selectedActivityList.filter((x) => x.ActivityId != item.ActivityId);
                    }
                }
            },

            /// <summary>
            ///  fnRemoveActivity :- Function used to remove activity
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnRemoveActivity() {
                this.PreLoader = true;
                this.RemoveActivityDialog = false;
                var data = new FormData();
                var actlist = [];
                if (this.RemoveActivityItem) {
                    data.append("ActivityIdList", this.RemoveActivityItem.ActivityId);
                } else {
                    for (var i = 0; i < this.selectedActivityList.length; i++) {
                        actlist.push(this.selectedActivityList[i].ActivityId);
                    }
                    var actids = actlist.toString();
                    data.append("ActivityIdList", actids);
                }
                var result = await objManageProjectActivity.RemoveActivity(data);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {

                    this.RemoveActivityItem = null;
                    this.selectedActivityList = [];
                    await this.fnGetProjectActivityList();
                    this.PreLoader = false;
                    this.successSnackbarMsg = true;
                    this.Msg = "Activity Successfully removed";
                } else {
                    this.Preloader = false;
                    this.Msg = "Failed to remove an activities.";
                    this.errorSnackbarMsg = true;
                }
            },

            //Assign Users functions

            /// <summary>
            ///  fnInitializeAssignUsers :- Function used to initialize assign users variables
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnInitializeAssignUsers(item) {
                if (item != null) {
                    this.AssignUser.ActivityId = item.ActivityId;
                    this.AssignUser.selectedProjectUserList = item.ActivityUsers;
                } else {
                    this.AssignUser.ActivityId = 0;
                    this.AssignUser.selectedProjectUserList = [];
                }
            },

            /// <summary>
            ///  fnAssignUser :- Function used to add user to activity based on ActivityId, ActivityUsersList
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAssignUser() {
                this.AssignUserDialog = false;
                var data = new FormData();
                data.append("ActivityId", this.AssignUser.ActivityId);
                data.append("ActivityUsersList", JSON.stringify(this.AssignUser.selectedProjectUserList));
                var result = await objManageProjectActivity.AssignUserToActivity(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = "Users added successfully.";
                    this.successSnackbarMsg = true;
                    this.fnInitializeAssignUsers(null);
                    this.fnGetProjectActivityList();
                } else {
                    this.Msg = "Failed to add an activity.";
                    this.errorSnackbarMsg = true;
                }
            },

            //Add SubActivity functions

            /// <summary>
            ///  fnCopyParentActivitySwitch :- Function used to clear all the checkboxes while copy parent activity (Basic Details, Payment Details,Activity Type, Assignment)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCopyParentActivitySwitch() {
                if (this.CopyParentActivitySwitch == false) {
                    this.CopyBasicDetails = false;
                    this.CopyPaymnetDetails = false;
                    this.CopyActivityType = false;
                    this.CopyAssignment = false;
                }
            },

            /// <summary>
            ///  fnInitializeSubActivity :- Function used to set default values in sub activity variables
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeSubActivity() {
                this.CopyParentActivitySwitch = false;
                this.CopyBasicDetails = false;
                this.CopyPaymnetDetails = false;
                this.CopyActivityType = false;
                this.CopyAssignment = false;

                this.SubActivity.ActivityId = 0;
                this.SubActivity.ParentActivityId = 0;
                this.SubActivity.ActivityName = "";
                this.SubActivity.Description = "";
                this.SubActivity.StartDate = "";
                this.SubActivity.EndDate = "";
                this.SubActivity.WeightagePercentage = 0;
                this.SubActivity.ActivityTags = [];
                this.SubActivity.ActivityType = "";
                this.SubActivity.SurveyId = 0;
                this.SubActivity.SurveySectionId = 0;
                this.SubActivity.SurveyQuestionId = 0;
                this.SubActivity.SurveyResponseId = 0;
                this.SubActivity.SurveyLogic = "";
                this.SubActivity.SurveyTarget = 0;
                this.SubActivity.SurveyTarget1 = "";
                this.SubActivity.IsPaymentLinked = false;
                this.SubActivity.PaymentLinkedBudget = 0;
                this.SubActivity.ExpenseTags = [];
                this.SubActivity.ExpenseTagsNotifyUsers = [];
                this.SubActivity.WorkflowId = "";
                this.SubActivity.Periodicity = 0;
                this.SubActivity.PeriodicityOnceDate = "";
                this.SubActivity.PeriodicityWeek = 0;
                this.SubActivity.PeriodicityMonthDay = 0;
                this.SubActivity.PeriodicityMonth = 0;
                this.SubActivity.PeriodicityMonthDateDay = 0;
                this.SubActivity.ActivityUsers = [];
                this.SubActivity.BeneficiaryTypeQstnID = 0;
            },

            /// <summary>
            ///  fnInitializeSubActivityDetails :- Function used to add parent output details to create a new sub activity
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSubActivityDetails() {

                var temp = this.ActivityList.find((a) => a.ActivityId == this.SubActivity.ParentActivityId);
                this.parentActivityStartDate = temp.StartDate;
                this.parentActivityEndDate = temp.EndDate;
                if (temp != null) {
                    if (this.CopyBasicDetails == true) {
                        this.SubActivity.ActivityName = temp.ActivityName;
                        this.SubActivity.Description = temp.Description;
                        this.SubActivity.StartDate = temp.StartDate; //this.formatDate(temp.StartDate);
                        this.SubActivity.EndDate = temp.EndDate; //this.formatDate(temp.EndDate);
                        this.SubActivity.WeightagePercentage = temp.WeightagePercentage;
                        if (temp.ActivityTags) {
                            temp.ActivityTags.forEach((u) => {
                                this.SubActivity.ActivityTags.push({ TagID: u.TagID, TagName: u.TagName, TagColor: u.TagColor, });
                            });
                        }
                    }
                    else {
                        this.SubActivity.ActivityName = "";
                        this.SubActivity.Description = "";
                        this.SubActivity.StartDate = "";
                        this.SubActivity.EndDate = "";
                        this.SubActivity.WeightagePercentage = 0;
                        this.SubActivity.ActivityTags = [];
                    }

                    if (this.CopyActivityType == true) {

                        this.SubActivity.ActivityType = temp.ActivityType;
                        this.SubActivity.SurveyId = temp.SurveyId;
                        if (this.SubActivity.SurveyId > 0) {
                            await this.GetSurveyBeneficiaryType(temp.SurveyId);
                        }
                        //await this.GetSectionsBySurveyID(temp.SurveyId);
                        this.SubActivity.BeneficiaryTypeQstnID = temp.BeneficiaryTypeQuestionID;
                        this.BeneficiaryTypeQstnID = temp.BeneficiaryTypeQuestionID;
                        if (temp.SurveySectionId > 0) {
                            this.SectionORBeneficiary = "Section";
                            this.SubActivity.SurveySectionId = temp.SurveySectionId;
                        }
                        if (temp.BeneficiaryTypeQuestionID > 0) {
                            this.SectionORBeneficiary = "Beneficiary";
                            this.SubActivity.SurveySectionId = temp.BeneficiaryTypeID;
                        }
                        if (this.SubActivity.SurveySectionId != 0) {
                            //await this.GetQstnBySectionID(item.SurveySectionId);
                            await this.GetSurveyOrBeneficiaryQuestions(this.SubActivity.SurveySectionId);
                        }

                        // await this.GetQstnBySectionID(temp.SurveySectionId);

                      
                        this.SubActivity.SurveyQuestionId = temp.SurveyQuestionId;
                        await this.GetResponsesByQstnid(temp.SurveyQuestionId, temp.BeneficiaryTypeQstnID);
                        this.SubActivity.SurveyResponseId = temp.SurveyResponseId;
                        this.SubActivity.SurveyLogic = temp.SurveyLogic;
                        await this.GetSurveyLinkedBeneficairyQuestionDetails(temp.SurveyId)


                        if (temp.SurveyLogic == 1 || temp.SurveyLogic == 2 || this.logicAnsType == "Single Line Text" || this.logicAnsType == "Numeric") {
                            this.SubActivity.SurveyTarget1 = temp.SurveyTarget;
                        } else {
                            this.SubActivity.SurveyTarget = parseInt(temp.SurveyTarget);
                        }
                    } else {
                        this.SubActivity.ActivityType = "";
                        this.SubActivity.SurveyId = 0;
                        this.SubActivity.SurveySectionId = 0;
                        this.SubActivity.SurveyQuestionId = 0;
                        this.SubActivity.SurveyResponseId = 0;
                        this.SubActivity.SurveyLogic = "";
                        this.SubActivity.SurveyTarget = "";
                    }

                    if (this.CopyPaymnetDetails == true) {
                        this.SubActivity.IsPaymentLinked = temp.IsPaymentLinked;
                        this.SubActivity.PaymentLinkedBudget = temp.PaymentLinkedBudget;
                        if (temp.ExpenseTags) {
                            temp.ExpenseTags.forEach((u) => {
                                this.SubActivity.ExpenseTags.push({ TagID: u.TagID, TagName: u.TagName, });
                            });
                        }
                        this.SubActivity.ExpenseTagsNotifyUsers = temp.ExpenseTagsNotifyUsers;
                    } else {
                        this.SubActivity.IsPaymentLinked = false;
                        this.SubActivity.PaymentLinkedBudget = 0;
                        this.SubActivity.ExpenseTags = [];
                        this.SubActivity.ExpenseTagsNotifyUsers = [];
                    }

                    if (this.CopyAssignment == true) {
                        this.SubActivity.WorkflowId = temp.WorkflowId;
                        this.SubActivity.Periodicity = temp.Periodicity;
                        this.SubActivity.PeriodicityOnceDate = temp.PeriodicityOnceDate.substr(0, 10); //this.formatDate(temp.PeriodicityOnceDate);
                        this.SubActivity.PeriodicityWeek = temp.PeriodicityWeek;
                        this.SubActivity.PeriodicityMonthDay = temp.PeriodicityMonthDay;
                        this.SubActivity.PeriodicityMonth = temp.PeriodicityMonth;
                        this.SubActivity.PeriodicityMonthDateDay = temp.PeriodicityMonthDateDay;
                        this.SubActivity.ActivityUsers = temp.ActivityUsers;
                        this.selectedProjectUserList = temp.ActivityUsers;
                    } else {
                        this.SubActivity.WorkflowId = "";
                        this.SubActivity.Periodicity = 0;
                        this.SubActivity.PeriodicityOnceDate = "";
                        this.SubActivity.PeriodicityWeek = 0;
                        this.SubActivity.PeriodicityMonthDay = 0;
                        this.SubActivity.PeriodicityMonth = 0;
                        this.SubActivity.PeriodicityMonthDateDay = 0;
                        this.SubActivity.ActivityUsers = [];
                    }
                }
            },

            /// <summary>
            ///  fnAddSubActivityInSubActivity :- Function used to add sub activity under parent activity/sub activity
            /// </summary>
            /// <param name="actid"></param>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnAddSubActivityInSubActivity(actid, item) {
                this.AddEditActivity.OutPutActivityUsers = [];
                this.CreateSubActivitySteps = 2;
                this.HidePreviousButton = true;
                this.AddEditActivityDialog = true;
                this.createActivityDlg = true
                //this.CreateSubActivityDailog = true;
                this.fnInitializeSubActivity();
                this.SubActivity.ParentActivityId = actid;
                this.ParentActivityOfSubAct = item;
                this.SubActivityTotalWeightage = item.SubActivityTotalWeightage;

                this.SubActivity.ActivityName = item.ActivityName;
                this.SubActivity.Description = item.Description;
                this.SubActivity.StartDate = item.StartDate;
                this.SubActivity.EndDate = item.EndDate;
                this.SubActivityCount = item.SubActivityCount;
                this.SubActivity.ActivityType = item.ActivityType;
                this.fnDefaultWeightage(true);

                if (item.OutPutQuantitativeLogic != null && item.OutPutQuantitativeLogic != undefined && item.OutPutQuantitativeLogic.length > 0) {
                    for (var i = 0; i < item.OutPutQuantitativeLogic.length; i++) {
                        const obj = new Object();
                        obj.QuestionId = item.OutPutQuantitativeLogic[i].QuestionId;
                        obj.SectionId = item.OutPutQuantitativeLogic[i].SectionId;
                        obj.SurveyId = item.OutPutQuantitativeLogic[i].SurveyId;
                        obj.QuestionName = item.OutPutQuantitativeLogic[i].QuestionName;
                        obj.SectionName = item.OutPutQuantitativeLogic[i].SectionName;
                        obj.SurveyName = item.OutPutQuantitativeLogic[i].SurveyName;
                        obj.BeneficiaryTypeQstnID = item.OutPutQuantitativeLogic[i].BeneficiaryTypeQstnID;
                        obj.SurveyPrimeLogic = [];
                        
                        if (item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic != null && item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic != undefined && item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic.length > 0) {
                            for (var k = 0; k < item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic.length; k++) {
                                const obj1 = new Object();

                                obj1.SurveyLogicPrimeID = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrime;
                                obj1.SurveyLogicPrime = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrimeType;
                                obj1.SurveyLogicPrimeAnswer = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicPrimeAnswer;
                                obj1.SurveyLogicCondition = item.OutPutQuantitativeLogic[i].lstSurveyPrimeLogic[k].SurveyLogicCondition;
                                obj1.id = k + 1;
                               
                                obj.SurveyPrimeLogic.push(obj1);
                            }
                        }

                        this.AddEditActivity.OutPutActivityUsers.push(obj);
                    }
                }

                this.parentActivityStartDate = this.ActivityList.find((a) => a.ActivityId == this.SubActivity.ParentActivityId).StartDate;
                this.parentActivityEndDate = this.ActivityList.find((a) => a.ActivityId == this.SubActivity.ParentActivityId).EndDate;
            },

            //Add Tag functions

            /// <summary>
            ///  fnInitializeSteper :- Function used to initialize stepper for tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                } else {
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
                var temp = this.AllTagList.filter((x) => x.TagName.includes(TagName));
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
                } else if (result.status == "success") {
                    this.TagHeaderName = "Add Tag";
                    this.AllTagListSteps = 2;
                    this.fnGetAllProjectTags();
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
            ///  fnAddToSubActivityTags :- Function used for add sub activity tag to the selected tag items based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToSubActivityTags(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.SubActivity.ActivityTags.length > 0) {
                        var temp = this.SubActivity.ActivityTags.filter((x) => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.SubActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor, });
                        }
                    } else {
                        this.SubActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor, });
                    }
                }
            },

            /// <summary>
            ///  fnAddToAddEditActivityTags :- Function used for add activity tag to the selected tag items based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToAddEditActivityTags(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.AddEditActivity.ActivityTags.length > 0) {
                        var temp = this.AddEditActivity.ActivityTags.filter((x) => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.AddEditActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor, });
                        }
                    } else {
                        this.AddEditActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor, });
                    }
                }
            },

            /// <summary>
            ///  fnEditTagDetails :- Function used to get tag details for edit
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnEditTagDetails(TagItem) {
                this.AllTagListSteps = 3;
                this.TagHeaderName = "Edit Tag Name";
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;
            },

            /// <summary>
            ///  UpdateTagDetails :- Function used to update tag details in edit mode based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var temp = this.AllTagList.filter((x) => x.TagName.includes(TagName));
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
                } else if (result.status == "success") {
                    this.TagHeaderName = "Add Tag";
                    this.AllTagListSteps = 2;
                    this.fnGetAllProjectTags();
                    this.UpdatingTagItem = "";
                }
            },

            /// <summary>
            ///  fnClearSurveydetails :- Function used to clear survey details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClearSurveydetails() {
                this.AddEditActivity.SurveyId = 0;
                this.AddEditActivity.SurveySectionId = 0;
                this.AddEditActivity.SurveyQuestionId = 0;
                this.AddEditActivity.SurveyLogic = 0;
                this.AddEditActivity.SurveyTarget1 = 0;
                this.AddEditActivity.SurveyTarget = 0;
            },

            /// <summary>
            ///  GetSurveyLinkedBeneficairyQuestionDetails :- Function used to fetch Survey Linked Beneficairy Question Details
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSurveyLinkedBeneficairyQuestionDetails(SurveyID) {
                var data = new FormData();
                data.append("surveyId", SurveyID)
                this.lstBeneficiaryType = [];
                var result = await objManageProjectActivity.GetSurveyLinkedBeneficairyQuestionDetails(data);
                
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data)
                    this.lstBeneficiaryType = result.data;
            },

            /// <summary>
            ///  GetSurveyBeneficiaryType :- Function used to fetch Survey beneficiary type based on SurveyID
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSurveyBeneficiaryType(SurveyID) {
                var data = new FormData();
                data.append("surveyId", SurveyID)
                this.lstBeneficiaryType = [];
                var result = await objManageProjectActivity.GetSurveyBeneficiaryType(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data)
                    this.SectionList = result.data;
            },
            async AddOptionItems() {
              
                var SurveyName = "", SectionName = "", QuestionName = "";

                if (this.AddEditActivity.SurveyQuestionId > 0 || this.AddEditActivity.BeneficiaryTypeQstnID > 0) {
                    this.OutputErrorMessage = "";
                } else {
                    this.OutputErrorMessage = "Please select Combinations";
                    return false;
                }

                if (this.AddEditActivity.OutPutActivityUsers != null && this.AddEditActivity.OutPutActivityUsers.length > 0) {
                    var index = -1;
                    if (this.AddEditActivity.SurveyQuestionId > 0) {
                        index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.QuestionId === this.AddEditActivity.SurveyQuestionId && a.SectionId === this.AddEditActivity.SurveySectionId && this.AddEditActivity.SurveyId);
                    } else {
                        index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.BeneficiaryTypeQstnID === this.AddEditActivity.BeneficiaryTypeQstnID && a.SectionId === this.AddEditActivity.SurveySectionId && this.AddEditActivity.SurveyId);
                    }

                    if (index >= 0) {
                        this.OutputErrorMessage = "Selected Combination Already existing. Please select another Combination.";
                        setTimeout(() => {
                            this.OutputErrorMessage = ""
                        }, 3000)
                        return false;
                    } else {
                        this.OutputErrorMessage = "";
                    }
                } else {
                    this.OutputErrorMessage = "";
                }


                if (this.AddEditActivity.SurveyQuestionId > 0) {
                    var questions = this.QstnList.filter(word => word.QuestionID == this.AddEditActivity.SurveyQuestionId);
                    if (questions != null && questions != undefined && questions.length > 0) {
                        QuestionName = questions[0].QuestionName;
                    }
                } else {
                    var questions = this.lstBeneficiaryType.filter(word => word.QuestionID == this.AddEditActivity.BeneficiaryTypeQstnID);
                    if (questions != null && questions != undefined && questions.length > 0) {
                        QuestionName = questions[0].QuestionName;
                    }
                }


                var sections = this.SectionList.filter(word => word.SectionID == this.AddEditActivity.SurveySectionId);

                if (sections != null && sections != undefined && sections.length > 0) {
                    SectionName = sections[0].SectionName;
                }

                var dd = this.SurveyList;
                var Surveys = this.SurveyList.filter(word => word.SurveyID == this.AddEditActivity.SurveyId);

                if (Surveys != null && Surveys != undefined && Surveys.length > 0) {
                    SurveyName = Surveys[0].SurveyName;
                }


                const obj = new Object();
                obj.QuestionId = this.AddEditActivity.SurveyQuestionId;
                obj.BeneficiaryTypeQstnID = (this.AddEditActivity.BeneficiaryTypeQstnID > 0 ? this.AddEditActivity.BeneficiaryTypeQstnID : 0);
                obj.SectionId = this.AddEditActivity.SurveySectionId;
                obj.SurveyId = this.AddEditActivity.SurveyId;
                obj.QuestionName = QuestionName;
                obj.SectionName = SectionName;
                obj.SurveyName = SurveyName;
                obj.SurveyPrimeLogic = [];
             
                if (this.SurveyPrimeLogicList != null && this.SurveyPrimeLogicList != undefined && this.SurveyPrimeLogicList.length > 0) {
                    for (var k = 0; k < this.SurveyPrimeLogicList.length; k++) {
                        obj.SurveyPrimeLogic.push(this.SurveyPrimeLogicList[k]);
                    }
                }

                this.AddEditActivity.OutPutActivityUsers.push(obj);
                this.AddEditActivity.SurveyId = 0;
                this.AddEditActivity.SurveySectionId = 0;
                this.AddEditActivity.SurveyQuestionId = 0;
                this.AddEditActivity.BeneficiaryTypeQstnID = 0;
                this.SurveyPrimeLogicList = [];
                this.IsNumericTextQuestion = 1;
            },

            async AddOptionItemsForSubActivity() {
              
                var SurveyName = "", SectionName = "", QuestionName = "";

                if (this.SubActivity.SurveyQuestionId > 0 || this.SubActivity.BeneficiaryTypeQstnID > 0) {
                    this.OutputErrorMessage = "";
                } else {
                    this.OutputErrorMessage = "Please select Combinations";
                    return false;
                }

                if (this.AddEditActivity.OutPutActivityUsers != null && this.AddEditActivity.OutPutActivityUsers.length > 0) {
                    var index = -1;
                    if (this.SubActivity.SurveyQuestionId > 0) {
                        index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.QuestionId === this.SubActivity.SurveyQuestionId && a.SectionId === this.SubActivity.SurveySectionId);
                    } else {
                        index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.BeneficiaryTypeQstnID === this.SubActivity.BeneficiaryTypeQstnID && a.SectionId === this.SubActivity.SurveySectionId);
                    }

                    if (index >= 0) {
                        this.OutputErrorMessage = "Selected Combination Already existing. Please select another Combination.";
                        setTimeout(() => {
                            this.OutputErrorMessage = ""
                        }, 3000)
                        return false;
                    } else {
                        this.OutputErrorMessage = "";
                    }
                } else {
                    this.OutputErrorMessage = "";
                }


                if (this.SubActivity.SurveyQuestionId > 0) {
                    var questions = this.QstnList.filter(word => word.QuestionID == this.SubActivity.SurveyQuestionId);
                    if (questions != null && questions != undefined && questions.length > 0) {
                        QuestionName = questions[0].QuestionName;
                    }
                } else {
                    var questions = this.lstBeneficiaryType.filter(word => word.QuestionID == this.SubActivity.BeneficiaryTypeQstnID);
                    if (questions != null && questions != undefined && questions.length > 0) {
                        QuestionName = questions[0].QuestionName;
                    }
                }


                var sections = this.SectionList.filter(word => word.SectionID == this.SubActivity.SurveySectionId);

                if (sections != null && sections != undefined && sections.length > 0) {
                    SectionName = sections[0].SectionName;
                }

                var dd = this.SurveyList;
                var Surveys = this.SurveyList.filter(word => word.SurveyID == this.SubActivity.SurveyId);

                if (Surveys != null && Surveys != undefined && Surveys.length > 0) {
                    SurveyName = Surveys[0].SurveyName;
                }


                const obj = new Object();
                obj.QuestionId = this.SubActivity.SurveyQuestionId;
                obj.BeneficiaryTypeQstnID = (this.SubActivity.BeneficiaryTypeQstnID > 0 ? this.SubActivity.BeneficiaryTypeQstnID : 0);
                obj.SectionId = this.SubActivity.SurveySectionId;
                obj.SurveyId = this.SubActivity.SurveyId;
                obj.QuestionName = QuestionName;
                obj.SectionName = SectionName;
                obj.SurveyName = SurveyName;
                obj.SurveyPrimeLogic = [];
            
                if (this.SurveyPrimeLogicList != null && this.SurveyPrimeLogicList != undefined && this.SurveyPrimeLogicList.length > 0) {
                    for (var k = 0; k < this.SurveyPrimeLogicList.length; k++) {
                        obj.SurveyPrimeLogic.push(this.SurveyPrimeLogicList[k]);
                    }
                }

                this.AddEditActivity.OutPutActivityUsers.push(obj);
                this.SubActivity.SurveyId = 0;
                this.SubActivity.SurveySectionId = 0;
                this.SubActivity.SurveyQuestionId = 0;
                this.SubActivity.BeneficiaryTypeQstnID = 0;
                this.SurveyPrimeLogicList = [];
            }, async AddEachSurveyLogic() {
                var id = 0;
              
                if (this.SurveyPrimeLogicList != null && this.SurveyPrimeLogicList != undefined) {
                    id = this.SurveyPrimeLogicList.length;
                }
                var sortedItem = this.LogicItemsPrime.filter(x => x.ID == this.SurveyLogicPrime);

                const obj = new Object();
                obj.SurveyLogicPrimeID = this.SurveyLogicPrime;
                obj.SurveyLogicPrime = sortedItem[0].Type;
                obj.SurveyLogicPrimeAnswer = this.SurveyLogicPrimeAnswer;
                obj.SurveyLogicCondition = this.SurveyLogicCondition;
                obj.id = id + 1;
                this.SurveyPrimeLogicList.push(obj);
                this.SurveyLogicPrime = 0;
                this.SurveyLogicPrimeAnswer = "";
                //this.IsNumericTextQuestion = 1;
                this.SurveyLogicCondition = "";
               
                if (this.CascadingQuestionOptions != null && this.CascadingQuestionOptions.length > 0) {
                    var lng = this.CascadingQuestionOptions.length;
                    for (var nCnt = 0; nCnt < this.CascadingQuestionOptions.length; nCnt++) {
                        if (this.CascadingQuestionOptions[nCnt].OptionValue == "Country") {
                            this.CascadingQuestionOptions[nCnt][lng-4] = 0;
                            if (this.CascadingQuestionOptions[0].OptionValue == "Country") {
                                this.CascadingQuestionOptions[0].CascadingList = this.lstCountry;
                            } else {
                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                            }
                        }
                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "States") {
                            this.CascadingQuestionOptions[nCnt][lng - 3] = 0;
                            if (this.CascadingQuestionOptions[0].OptionValue == "States") {
                                this.CascadingQuestionOptions[0].CascadingList = this.lstStates;
                            } else {
                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                            }
                        }
                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "District") {
                            this.CascadingQuestionOptions[nCnt][lng - 2] = 0;
                            if (this.CascadingQuestionOptions[0].OptionValue == "District") {
                                this.CascadingQuestionOptions[0].CascadingList = this.lstDistrict;
                            } else {
                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                            }
                        }
                        else if (this.CascadingQuestionOptions[nCnt].OptionValue == "SubDistrict") {
                            this.CascadingQuestionOptions[nCnt][lng - 1] = 0;
                            if (this.CascadingQuestionOptions[0].OptionValue == "SubDistrict") {
                                this.CascadingQuestionOptions[0].CascadingList = this.lstSubDistrict;
                            } else {
                                this.CascadingQuestionOptions[nCnt].CascadingList = [];
                            }
                        }
                    }
                }
            },

            async DeleteSurveyPrimeLogic(logicId, logicAnswer, id) {
                if (confirm("Are you sure you want to delete this item?") == true) {
                    if (id > 0) {
                        var index = this.SurveyPrimeLogicList.findIndex(a => a.id === id && a.SurveyLogicPrimeID === logicId && a.SurveyLogicPrimeAnswer === logicAnswer);
                        this.SurveyPrimeLogicList.splice(index, 1);
                    }
                }
            },
            async DeleteOutPutOptionItem(QuestionId, BeneficiaryTypeQstnID, SectionId, SurveyId) {

                if (confirm("Are you sure you want to delete this item?") == true) {
                    if (QuestionId > 0) {
                        var index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.QuestionId === QuestionId && a.SectionId === SectionId && a.SurveyId === SurveyId);
                        this.AddEditActivity.OutPutActivityUsers.splice(index, 1);
                    } else {
                        var index = this.AddEditActivity.OutPutActivityUsers.findIndex(a => a.BeneficiaryTypeQstnID === BeneficiaryTypeQstnID && a.SectionId === SectionId && a.SurveyId === SurveyId);
                        this.AddEditActivity.OutPutActivityUsers.splice(index, 1);
                    }
                }
            },
            async fnCreateOuput(item, mode) {
                if (mode && mode == 'AddSubActivity') {
                    this.editActivityMode = mode;
                    item.ParentActivityId = item.ActivityId;
                    item.ActivityId = 0;
                    this.currentEditActivity = item
                    this.createSubActivityDlg = true
                }   
                else {
                    //this.AddEditActivity.WeightagePercentage = 1;
                    this.SubActivityTotalWeightage = 0;
                    this.AddEditActivityDialog = true;
                    this.AddEditActivityDialogHeader = 'Create Output';
                    this.AddEditActivity.OutPutActivityUsers = [];
                    this.OutputErrorMessage = "";
                    this.SurveyErrorMessage = "";
                    this.createActivityDlg = true
                }
            },

            async fnCreateLogicCondition() {
                var object = new Object();
                object.OptionID = 1;
                object.OptionValue = "AND";
                this.lstConditions.push(object);
                var object1 = new Object();
                object1.OptionID = 2;
                object1.OptionValue = "OR";
                this.lstConditions.push(object1);
            },
            // Function Used to Filter Cascading dropdown based on parent cascading dropdown
            async FilterCascadingItems(OptItem, parentID) {
               
                var sublist = [];
                var nextHierarchyOrder = OptItem.HierarchyOrder + 1;
                var len;

                len = this.CascadingQuestionOptions[this.CascadingQuestionOptions.length - 1].HierarchyOrder;

                if (len >= nextHierarchyOrder) {
                    var nxtList = [];
                    nxtList = this.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);

                    if (nxtList != null && nxtList.length != 0) {
                        if (nxtList[0].OptionValue == "States") {
                            this.SurveyLogicPrimeAnswer = "";
                            var lst = this.lstCountry.filter(x => x.ID == parentID);
                            if (lst != null && lst.length > 0) {
                                this.SurveyLogicPrimeAnswer = lst[0].Name;
                            }

                            sublist = this.lstStates.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                        }
                        else if (nxtList[0].OptionValue == "District") {
                            var lst1 = this.lstStates.filter(x => x.ID == parentID);
                            if (lst1 != null && lst1.length > 0) {
                                var arrcount = this.SurveyLogicPrimeAnswer.split(',');

                                if (arrcount != null && arrcount.length > 1) {
                                    this.SurveyLogicPrimeAnswer = arrcount[0];
                                } 

                                if (this.SurveyLogicPrimeAnswer != null && this.SurveyLogicPrimeAnswer.length > 0) {
                                    this.SurveyLogicPrimeAnswer = this.SurveyLogicPrimeAnswer + "," + lst1[0].Name;
                                } else {
                                    this.SurveyLogicPrimeAnswer = lst1[0].Name;
                                }
                            }

                            sublist = this.lstDistrict.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                        }
                        else if (nxtList[0].OptionValue == "SubDistrict") {
                            var lst2 = this.lstDistrict.filter(x => x.ID == parentID);
                            if (lst2 != null && lst2.length > 0) {
                                var arrcount = this.SurveyLogicPrimeAnswer.split(',');

                                if (arrcount != null && arrcount.length > 2) {
                                    this.SurveyLogicPrimeAnswer = arrcount[0] + ","+arrcount[1];
                                } 

                                if (this.SurveyLogicPrimeAnswer != null && this.SurveyLogicPrimeAnswer.length > 0) {
                                    this.SurveyLogicPrimeAnswer = this.SurveyLogicPrimeAnswer + "," + lst2[0].Name;
                                } else {
                                    this.SurveyLogicPrimeAnswer = lst2[0].Name;
                                }
                            }

                            sublist = this.lstSubDistrict.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                        }

                        this.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = [];
                        this.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                    }
                } else {
                    var lst2 = this.lstSubDistrict.filter(x => x.ID == parentID);
                    if (lst2 != null && lst2.length > 0) {

                        var arrcount = this.SurveyLogicPrimeAnswer.split(',');

                        if (arrcount != null && arrcount.length > 3) {
                            this.SurveyLogicPrimeAnswer = arrcount[0] + "," + arrcount[1] + "," + arrcount[2];
                        } 

                        if (this.SurveyLogicPrimeAnswer != null && this.SurveyLogicPrimeAnswer.length > 0) {
                            this.SurveyLogicPrimeAnswer = this.SurveyLogicPrimeAnswer + "," + lst2[0].Name;
                        } else {
                            this.SurveyLogicPrimeAnswer = lst2[0].Name;
                        }
                    }
                }
            },
            fnUnloadOutputDlg() {
                this.editActivityMode = false
                this.currentEditActivity = null
                this.createActivityDlg = false
                this.createSubActivityDlg = false
            },
            fnEditOutputDlg(item) {
                this.editActivityMode = true
                this.currentEditActivity = item
                this.createActivityDlg = true
            }
        },
        watch: {
            /// <summary>
            ///  AddEditActivitySearchExpenseTag :- Function used to search activity expense tags based on TagName (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddEditActivitySearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.AddEditActivitySearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter((x) => x.TagName.includes(this.AddEditActivitySearchExpenseTag));
            },

            /// <summary>
            ///  SubActivitySearchExpenseTag :- Function used to search sub activity expense tags based on TagName (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SubActivitySearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.SubActivitySearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter((x) => x.TagName.includes(this.SubActivitySearchExpenseTag));
            },

            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0) {
                    this.filterAllTagList = this.filterAllTagList.filter((x) => x.TagName.includes(this.SearchTagDetails));
                }
            },

            /// <summary>
            ///  SearchActivity :- Function used to search activity based on ActivityName
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchActivity: function () {
                this.filterActivityList = this.ActivityList;
                if (this.filterActivityList.length > 0 && this.SearchActivity != "") {
                    this.filterActivityList = this.filterActivityList.filter((x) => x.ActivityName.toLowerCase().includes(this.SearchActivity.toLowerCase()));
                }
            },

            /// <summary>
            ///  searchProjectUserList :- Function used to search project users while assign user to activity based on Name
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            searchProjectUserList: function () {
                this.filterProjectUserList = this.ProjectUserList;
                if (this.filterProjectUserList.length > 0 && this.searchProjectUserList != "") {
                    this.filterProjectUserList = this.filterProjectUserList.filter((x) => x.Name.includes(this.searchProjectUserList));
                }
            },
        },
    };
</script>

<style>
    .row {
        padding-left: 0px;
        border: 0px none !important;
    }
</style>
<style scoped>
    .text-limit {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

        .text-limit:hover {
            white-space: normal;
            overflow: visible;
        }
</style>


