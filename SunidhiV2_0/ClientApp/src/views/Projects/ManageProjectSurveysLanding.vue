<template>
    <div>
        <!--Pre-loader-->
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <!--________________________-->
        <v-row v-if="SurveyDetailsTableItems.length>0">
            <v-col cols="4">
                <h5 class="page-head ml-2">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                    </v-avatar>
                    Surveys
                </h5>
            </v-col>
            <v-spacer></v-spacer>
            <v-col md="auto">
                <v-row class="mr-1">

                    <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn v-on="on" name="btnCreateSurvey" class="success-btn mr-4 float-left" depressed dark height="39">
                                Create Survey
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list dense color="white" light width="auto" max-width="250" class="action-button-list">
                            <v-list-item name="listItemCreateSurvey" id="AutoTest_IP739" link @click="OpenCreateSurveyDialog()">
                                <v-list-item-icon>
                                    <v-icon>mdi-plus-circle</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Create Survey
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item id="AutoTest_IP740" link @click="ImportSurveyDialogBox = true">
                                <v-list-item-icon>
                                    <v-icon>
                                        mdi-file-account-outline
                                    </v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Import Survey
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                    <!-- Bulk Actions-->
                    <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn name="btnBulkActionsDropdown" id="AutoTest_IP741" height="39" v-on="on" color="grey" outlined class="elevation-0 mr-4 float-left" @click="">
                                Bulk Actions
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                            <v-list-item name="btnBulkRemove" id="AutoTest_IP742" @click.stop="fnBulkRemoveClick()">
                                <v-list-item-icon>
                                    <v-icon>mdi-trash-can-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Remove
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                        <!-- Remove Survey-->
                        <v-dialog v-model="RemoveBulkSurveyDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head py-4">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Remove Survey
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP743" text="" @click="RemoveBulkSurveyDialog=false,SelectedSurvey=[]">
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
                                                            <h4 class="text-center heading-3">Are you sure you want to remove this Survey(s) ?</h4>
                                                        </v-col>

                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                            <v-btn id="AutoTest_IP744" color="secondary elevation-0" class="mr-2" @click="fnDeleteProjectSurvey(1),RemoveBulkSurveyDialog=false">Remove</v-btn>
                                                            <v-btn id="AutoTest_IP745" outlined color="grey" @click="RemoveBulkSurveyDialog=false,SelectedSurvey=[]">Cancel</v-btn>
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
                    <v-text-field name="txtSearchSurveyName" label="Search" class="mr-4 float-left" v-model="SearchSurveyDetails"
                                  dense="" prepend-inner-icon="mdi-magnify"
                                  outlined hide-details style="max-width:150px"></v-text-field>
                    <!--Filter-->
                    <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                        <template v-slot:activator="{ on }">
                            <v-btn name="btnFilter" id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent">
                                <v-icon>mdi-filter-outline</v-icon>
                                Filters
                            </v-btn>
                        </template>
                    </v-menu>
                </v-row>
            </v-col>
        </v-row>
        <v-row v-if="filterContent">
            <h4 class="mx-4 my-2 page-head">
                Filters
            </h4>
            <!--Date range-->
            <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="dateRangeMenu">
                <template v-slot:activator="{ on }">
                    <v-btn name="drpDateRangeFilter" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                        {{dateRangeHeaderName}}
                        <v-icon class="ml-10">
                            mdi-chevron-down
                        </v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="900" min-width="50" class="action-button-list pa-2">
                    <v-row justify="center" no-gutters>
                        <v-col cols="4" class="px-2">
                            <h3 style="text-align:center">Start Date</h3>
                            <v-date-picker name="dtStartDate" color="primary" v-model="FiterStartDate" no-title @input="IsValidDate()"
                                           class="elevation-0"></v-date-picker>
                        </v-col>
                        <v-col cols="4" class="px-2">
                            <h3 style="text-align:center">End Date</h3>
                            <v-date-picker name="dtEndDate" color="primary" v-model="FiterEndDate" no-title :min="FiterStartDate" @input="IsValidDate()"
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
                            <v-btn name="btnCancelFilter" height="32" block="" class="elevation-0 " @click="dateRangeMenu = false">
                                cancel
                            </v-btn>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col cols="2">
                            <v-btn name="btnDoneFilter" height="32" block="" color="primary" class="elevation-0" @click="fnClickFilterDone(),dateRangeMenu = false">
                                Done
                            </v-btn>
                        </v-col>
                    </v-row>
                </v-list>
            </v-menu>

            <!-- Frequency -->
            <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="frequencyMenu">
                <template v-slot:activator="{ on }">
                    <v-btn name="drpFrequencyFilter" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                        Frequency
                        <v-icon class="ml-10">
                            mdi-chevron-down
                        </v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                    <v-checkbox color="success-check-box" input-value="true" label="Never"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Once"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Monthly"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Quarterly"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Yearly"></v-checkbox>

                    <div class="d-inline-flex">
                        <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="frequencyMenu = false">
                            Cancel
                        </v-btn>
                        <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="frequencyMenu = false">
                            Done
                        </v-btn>
                    </div>
                </v-list>
            </v-menu>
            <!--Workflow-->
            <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="workflowMenu">
                <template v-slot:activator="{ on }">
                    <v-btn name="drpWorkflowFilter" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
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
                    <v-btn name="drpAssignedUserFilter" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
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
            <!-- Beneficiary type -->
            <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="beneficiaryTypeMenu">
                <template v-slot:activator="{ on }">
                    <v-btn name="drpBeneficiaryTypeFilter" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                        Beneficiary type
                        <v-icon>
                            mdi-chevron-down
                        </v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                    <v-checkbox color="success-check-box" input-value="true" label="Students"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Farmers"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Schools"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" label="Other Utilities"></v-checkbox>

                    <div class="d-inline-flex">
                        <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="beneficiaryTypeMenu = false">
                            Cancel
                        </v-btn>
                        <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="beneficiaryTypeMenu = false">
                            Done
                        </v-btn>
                    </div>
                </v-list>
            </v-menu>
        </v-row>
        <!--<v-row v-if="filterContent">
            <v-chip class="mx-1" medium label clearable close>31 Jun - 26 Jul</v-chip>
            <v-chip class="mx-1" medium label clearable close>Never</v-chip>
            <v-chip class="mx-1" medium label clearable close>Evaluation Approval</v-chip>
            <v-chip class="mx-1" medium label clearable close>Subhash</v-chip>
            <v-chip class="mx-1" medium label clearable close>Schools</v-chip>
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

        <!--Main body-->
        <v-row v-if="FilterSurveyDetailsTableItems.length>0">
            <!--Survey Table-->
            <v-col cols="12" md="12" sm="12" xs="12" class="px-2">
                <v-data-table :headers="SurveyDetailsTableheaders"
                              :items="FilterSurveyDetailsTableItems"
                              :search="SearchSurveyDetails"
                              show-select
                              item-key="SurveyID"
                              v-model="SelectedSurvey"
                              fixed-header
                              height="60vh"
                              class="elevation-0">

                    <!-- Survey Name-->
                    <template v-slot:item.SurveyName="{item}">
                        <v-list class="py-0 table-v-list">
                            <!--if path is ManageProjectSurveys-->
                            <v-list-item v-if="$route.name=='ManageProjectSurveys'" class="my-0 px-0" link :to="{name:'ManageProjectSurveysIndividual',query: { WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeId, ProjID: ProjectId,  SID: item.SurveyID ,ResCount :item.ResponseCount}}">
                                <div class="pa-0 text-limit" style="width:220px">
                                    <v-avatar size="25" color="#fff" class="elevation-1">
                                        <v-icon size="15" color="primary">mdi-book</v-icon>
                                    </v-avatar>
                                    {{item.SurveyName == '' || item.SurveyName == undefined ? 'N/A':item.SurveyName}}
                                </div>
                            </v-list-item>
                            <!--__________________________-->
                            <!--if path is MyprojectsDraftIndividualSurveysIndividual-->
                            <v-list-item v-if="$route.name=='MyprojectsDraftIndividualSurveys'" class="my-0 px-0" link="" :to="{name:'MyprojectsDraftIndividualSurveysIndividual',query: { WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeId, ProjID: ProjectId,  SID: item.SurveyID ,ResCount :item.ResponseCount}}">
                                <div class="pa-0 text-limit" style="width:220px">
                                    <v-avatar size="25" color="#fff" class="elevation-1">
                                        <v-icon size="15" color="primary">mdi-book</v-icon>
                                    </v-avatar>
                                    {{item.SurveyName == '' || item.SurveyName == undefined ? 'N/A':item.SurveyName}}
                                </div>
                            </v-list-item>
                            <!--______________-->
                            <!--__________________________-->
                            <!--if path is MyprojectsDeployedIndividualSurveysIndividual-->
                            <v-list-item v-if="$route.name=='MyprojectsDeployedIndividualSurveys'" class="my-0 px-0" link="" :to="{name:'MyprojectsDeployedIndividualSurveysIndividual',query: { WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeId, ProjID: ProjectId,  SID: item.SurveyID ,ResCount :item.ResponseCount}}">
                                <div class="pa-0 text-limit" style="width:220px">
                                    <v-avatar size="25" color="#fff" class="elevation-1">
                                        <v-icon size="15" color="primary">mdi-book</v-icon>
                                    </v-avatar>
                                    {{item.SurveyName == '' || item.SurveyName == undefined ? 'N/A':item.SurveyName}}
                                </div>
                            </v-list-item>
                            <!--______________-->
                        </v-list>
                    </template>

                    <template v-slot:item.CreatedDate="{item}">
                        <div class="pa-0 text-limit" style="width:80px">
                            {{item.CreatedDate == '' || item.CreatedDate == undefined ? 'N/A':item.CreatedDate}}
                        </div>
                    </template>

                    <template v-slot:item.UpdatedDate="{item}">
                        <div class="pa-0 text-limit" style="width:80px">
                            {{item.UpdatedDate == '' || item.UpdatedDate == undefined ? 'N/A':item.UpdatedDate}}
                        </div>
                    </template>

                    <template v-slot:item.WorkFlowName="{item}">
                        <div class="pa-0 text-limit" style="width:100px">
                            {{item.WorkFlowName == '' || item.WorkFlowName == undefined ? 'N/A':item.WorkFlowName}}
                        </div>
                    </template>
                    <template v-slot:item.BeneficiaryTypeName="{item}">
                        <div class="pa-0 text-limit" style="width:100px">
                            {{item.BeneficiaryTypeName == '' || item.BeneficiaryTypeName == undefined ? 'N/A':item.BeneficiaryTypeName}}
                        </div>
                    </template>

                    <!-- Users-->
                    <template v-slot:item.AssignedUsers="{item}">

                        <!--<div v-for="usr in item.AssignedUsers.split(',')" class="d-inline-flex">
                          <v-avatar color="#0CBA2F" size="25" class="mr-1" v-if="usr.length >0">
                              <span>{{usr.charAt(0)}}</span>
                          </v-avatar>
                        </div> -->
                        <div v-if="item.AssignedUsers.split(',').length >0" class="px-0 py-0 mt-3 ml-1" style="width:80px">
                            <v-tooltip left>
                                <template v-slot:activator="{ on }">
                                    <div v-on="on" class="d-inline-flex">
                                        <v-avatar :color="`${commonGetColor(item.AssignedUsers.split(',')[0])}`" size="30" class="team-avatar" v-if="item.AssignedUsers.split(',').length >=1 && item.AssignedUsers.split(',')[0].length >0">
                                            <v-img v-bind:src="userimage + item.ProfileImage.split(',')[0]" alt="User DP" height="30" width="30" class="ma-4" v-if="item.ProfileImage.split(',')[0].length >0" />
                                            <span class="white--text" v-else>{{item.AssignedUsers == null ? "" : item.AssignedUsers.substring(0, 1).toUpperCase()}}</span>
                                            <!--<span class="text--white">{{item.AssignedUsers.split(',')[0].charAt(0)}}</span>-->
                                        </v-avatar>
                                        <v-avatar size="30" class="team-count" v-if="item.AssignedUsers.split(',').length > 1">
                                            +{{item.AssignedUsers.split(',').length -1}}
                                        </v-avatar>
                                    </div>
                                </template>
                                <span v-if=" item.AssignedUsers.split(',').length >=1 && item.AssignedUsers.split(',')[0].length >0">{{item.AssignedUsers.split(',')[0]}}</span>
                                <span v-if="item.AssignedUsers.split(',').length >=2">,{{item.AssignedUsers.split(',')[1]}}</span>
                            </v-tooltip>
                        </div>
                    </template>
                    <!-- Row Actions-->
                    <template v-slot:item.Actions="{item}">
                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn name="btnActionSurvey" min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                    <v-icon>mdi-dots-vertical</v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light width="auto" class="action-button-list">
                                <!-- Edit-->
                                <v-list-item name="listItemEditSurvey" id="AutoTest_IP748" link @click.stop="fnEditSurveyDetails(item.SurveyID)">
                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Edit</v-list-item-content>
                                </v-list-item>
                                <!-- Assign Workflow-->
                                <v-list-item name="listItemAssignWorkflow" id="AutoTest_IP749" link @click.stop="SurveyId=item.SurveyID,fnGetWorkflowsListByProjectID(item),SelectWorkflowDialog = true">
                                    <v-list-item-icon><v-icon>mdi-timeline-text-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Assign Workflow</v-list-item-content>
                                </v-list-item>
                                <!-- Assign User-->
                                <v-list-item name="listItemAssignUsers" id="AutoTest_IP750" link @click.stop="OpenAssignUserDeatilsToSurveyDialog(item.SurveyID)">
                                    <v-list-item-icon><v-icon>mdi-account-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Assign User</v-list-item-content>
                                </v-list-item>
                                <!-- Link Beneficiary TYpe-->
                                <v-list-item name="listItemLinkBeneficiaryType" id="AutoTest_IP751" link @click.stop="OpenLinkBeneficiaryPopup(item.SurveyID)">
                                    <v-list-item-icon><v-icon>mdi-link-box-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Link Beneficiary Type</v-list-item-content>
                                </v-list-item>
                                <v-list-item name="listItemDuplicateSurvey" id="AutoTest_IP752" link @click.stop="fnGetSurveyDetails(item.SurveyID),CreateDuplicateSurveyDialog = true">
                                    <v-list-item-icon><v-icon>mdi-note-multiple-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Duplicate</v-list-item-content>
                                </v-list-item>
                                <!-- Remove-->
                                <v-list-item name="listItemRemoveSurvey" id="AutoTest_IP753" link @click.stop="RemoveSurveyDialog = true,SurveyId=item.SurveyID,SurveyName=item.SurveyName">
                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Remove</v-list-item-content>
                                </v-list-item>
                                <v-list-item name="listImportTemplate" id="AutoTest_IP753" link @click.stop="fnGetSurveyDetails(item.SurveyID),ImportTemplateDialog = true,ImportSurveyId=item.SurveyID">
                                <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                <v-list-item-content>Import Template</v-list-item-content>
                                </v-list-item>
                            </v-list>
                    </v-menu>
                </template>
            </v-data-table>
        </v-col>
    </v-row>

    <!-- If no data-->
                                <v-row v-else>
                                    <v-col class="text-center">
                                        <p class="mt-12 pt-12">
                                            <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                                        </p>

                                        <p class="heading-3-dark font-weight-medium pt-6">No survey data available</p>
                                        <v-menu offset-y transition="scroll-y-transition" close-on-content-click internal-activator v-bind:disabled="!screenAccess">
                                            <template v-slot:activator="{ on }">
                                                <v-btn height="32" v-on="on" color="success-btn" dark class="elevation-0 mr-2">
                                                    Create Survey
                                                    <v-icon>mdi-chevron-down</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                                <v-list-item id="AutoTest_IP746" link @click.stop="OpenCreateSurveyDialog()">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-plus-circle</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Create Survey</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_IP747" link @click.stop="ImportSurveyDialogBox = true">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-file-account-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Import Survey</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-menu>
                                    </v-col>
                                </v-row>

                                <!--Create Survey Dialog-->
                                <v-dialog v-model="CreateSurveyDialog" width="800" persistent>
                                    <v-form ref="form">
                                        <v-card>
                                            <v-card-title primary-title="" class="page-head pb-0">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-file-document-edit-outline</v-icon>
                                                </v-avatar>
                                                {{creatEditTitle}}
                                                <v-spacer></v-spacer>
                                                <v-btn name="btnCloseSurvey" id="AutoTest_IP754" text="" @click="CloseProjectSurveyDialog()">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>

                                            <v-card-text>
                                                <h4 class="text-center heading-3 mt-4"> {{creatEditDescription}}</h4>

                                                <v-row justify="center">
                                                    <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                                        <!--<sub v-if="SurveyName.length==0" style="color:red;font-size:large" right>*</sub>-->
                                                        <v-text-field name="txtCreateSurveyName" outlined="" dense="" v-model="SurveyName" :rules="alphanumericRules" label="Survey Name">
                                                            <template v-slot:append>
                                                                <span v-if="SurveyName.length==0" class="error--text">*</span>
                                                            </template>
                                                        </v-text-field>
                                                        <v-textarea outlined="" dense="" name="txtCreateSurveyDescription" label="Enter Description" value="" v-model="SurveyDescription"></v-textarea>

                                                        <v-select name="selectionWorkflow" dense :items="lstWorkFlow" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Workflow" outlined hide-details v-model="WorkflowID" v-bind:disabled="!EnableWorkflow">
                                                        </v-select>
                                                        <v-row>
                                                            <v-col md="6">
                                                                <v-menu v-model="mStartDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field name="dtSurveyStartDate" v-model="computedStartDate"
                                                                                      label="Start Date"
                                                                                      outlined dense
                                                                                      readonly
                                                                                      prepend-inner-icon="mdi-calendar"
                                                                                      v-on="on" hide-details>
                                                                            <template v-if="startDate == '' || startDate == null" v-slot:append="">
                                                                                <span class="mandatory-star">*</span>
                                                                            </template>
                                                                        </v-text-field>

                                                                    </template>
                                                                    <v-date-picker color="primary" v-model="startDate" no-title @input="mStartDate = false,IsValidDate()" format="DD-MM-YYYY"
                                                                                   :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>

                                                            </v-col>
                                                            <v-col md="6">
                                                                <v-menu v-model="mEndDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field name="dtSurveyEndDate" v-model="computedEndDate"
                                                                                      label="End Date"
                                                                                      outlined dense
                                                                                      readonly
                                                                                      prepend-inner-icon="mdi-calendar"
                                                                                      v-on="on" hide-details>
                                                                            <template v-if="endDate == '' || endDate == null" v-slot:append="">
                                                                                <span class="mandatory-star">*</span>
                                                                            </template>
                                                                        </v-text-field>
                                                                    </template>
                                                                    <v-date-picker color="primary" v-model="endDate" no-title @input="mEndDate = false,IsValidDate()"
                                                                                   :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>
                                                            </v-col>
                                                        </v-row>
                                                        <v-select name="selectionReportingFrquency" id="selectionReportingFrquencyid" dense="" :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="ReportingFrequency" :rules="[rules.required]">
                                                            <template v-if="ReportingFrequency == '' || ReportingFrequency == null"
                                                                      v-slot:append="">
                                                                <span class="mandatory-star">*</span>
                                                            </template>
                                                        </v-select>
                                                        <!-- Once -->
                                                        <div v-show="ReportingFrequency === 2">
                                                            <v-menu v-model="FrmDateMenu"
                                                                    :close-on-content-click="false"
                                                                    transition="scale-transition"
                                                                    offset-y
                                                                    max-width="290px"
                                                                    min-width="290px">
                                                                <template v-slot:activator="{ on }">
                                                                    <v-text-field v-model="FormattedDate"
                                                                                  label="Choose a Date"
                                                                                  persistent-hint outlined hide-details
                                                                                  prepend-inner-icon="mdi-calendar"
                                                                                  v-on="on" dense>
                                                                    </v-text-field>
                                                                </template>
                                                                <v-date-picker color="primary" v-model="FrmDate" no-title @input="FrmDateMenu = false,IsValidPeriodicityDate()"
                                                                               dense :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                                            </v-menu>
                                                        </div>

                                                        <!-- Weekly-->
                                                        <div v-show="ReportingFrequency === 4">
                                                            <v-select dense="" :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="ReportingWeek" hide-details>
                                                            </v-select>
                                                        </div>

                                                        <!-- Monthly-->
                                                        <div v-show="ReportingFrequency === 5">
                                                            <v-select dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDay" hide-details>
                                                            </v-select>
                                                        </div>

                                                        <!-- Quaterly , Halyearly and Yearly-->
                                                        <div class="d-inline-flex">
                                                            <v-select class="mr-1" hide-details dense :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="ReportingMonth" v-show="ReportingFrequency === 6|| ReportingFrequency === 7 ||  ReportingFrequency === 8">
                                                            </v-select>
                                                            <v-select hide-details dense :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDateDay" v-show="ReportingFrequency === 6|| ReportingFrequency === 7 ||  ReportingFrequency === 8">
                                                            </v-select>
                                                        </div>
                                                        <p>
                                                            Beneficiary Linked
                                                            <v-switch name="toggleBtnBeneficiaryLinked" hide-details class="ml-2" style="display:inline-block"
                                                                      color="secondary" v-model="BeneficiaryLinked"></v-switch>
                                                        </p>
                                                        <v-select id="drpBeneficiaryType" v-show="BeneficiaryLinked" dense :items="lstBeneficiaryType" :item-text="i => i.Name" :item-value="i => i.ID" label="Select Beneficiary Type" outlined="" v-model="BeneficiaryTypeID">
                                                        </v-select>
                                                        <!-- Select User -->
                                                        <span class="small-text mr-1">
                                                            <v-icon size="15">mdi-account</v-icon> Users <span class="mandatory-star" v-if="selectedteamUsers.length == 0">*</span>
                                                        </span>

                                                        <v-tooltip left v-if="selectedteamUsers.length">
                                                            <template v-slot:activator="{ on }">
                                                                <span v-on="on">
                                                                    <v-avatar size="27" class="team-avatar" v-for="(item,key) in selectedteamUsers.slice(0,2)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                                                        <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" height="27" width="27" class="ma-4" v-if="item.ProfileImage.length >0" />
                                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    <v-avatar v-if="selectedteamUsers.length>=3" size="27" class="team-count">
                                                                        +{{selectedteamUsers.length-2}}
                                                                    </v-avatar>
                                                                </span>
                                                            </template>
                                                            <span v-if="selectedteamUsers.length >= 1">{{selectedteamUsers[0].Name}}</span>
                                                            <span v-if="selectedteamUsers.length >= 2">,{{selectedteamUsers[1].Name}}</span>
                                                        </v-tooltip>
                                                        <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                            <template v-slot:activator="{ on }">
                                                                <v-btn name="btnAddUser" id="AutoTest_IP755" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" @click="OpenAssignUserDialog()">
                                                                    <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                                </v-btn>
                                                            </template>
                                                            <!--Assign Account User pop up-->
                                                            <v-card>
                                                                <!--<v-card-title class="heading-3">
                                        Select User(s)
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP756" text="" height="25" min-width="25" max-width="25" color="#707070" @click="CloseAssignUserDialog()">
                                            <v-icon size="20">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-divider></v-divider>-->
                                                                <v-card-text style="height:300px">
                                                                    <v-col cols="12" md="12" sm="12" xs="12" class="pa-0 ma-0">
                                                                        <v-data-table :headers="lstuserTableheaders"
                                                                                      :items="lstUsers"
                                                                                      show-select
                                                                                      item-key="ID"
                                                                                      sort-by="Name"
                                                                                      height="32vh"
                                                                                      fixed-header
                                                                                      v-model="selectedteamUsers1">
                                                                            <template v-slot:item.Name="{item}">
                                                                                <v-list class="py-0 table-v-list">
                                                                                    <v-list-item class="my-0 px-0">
                                                                                        <v-list-item-icon>
                                                                                            <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                                                <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                                <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                            </v-avatar>
                                                                                        </v-list-item-icon>
                                                                                        <v-list-item-content>
                                                                                            {{ item.Name }}
                                                                                        </v-list-item-content>
                                                                                    </v-list-item>
                                                                                </v-list>
                                                                            </template>
                                                                        </v-data-table>
                                                                    </v-col>
                                                                </v-card-text>
                                                                <v-divider></v-divider>
                                                                <v-card-actions>
                                                                    <v-row no-gutters="">
                                                                        <v-col md="12" class="" align="right">
                                                                            <Success-Button id="AutoTest_IP757" style="width:130px" class="btn-122x36" title="Save" @click.native="AssignUserToSurvey()"></Success-Button>
                                                                            <Default-Button-Outlined id="AutoTest_CL01" class="btn-122x36" title="Cancel" @click.native="CloseAssignUserDialog()"></Default-Button-Outlined>
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
                                                                        <v-btn id="AutoTest_IP758" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                                               @click="fnInitializeSteper()" class="mr-1">
                                                                            <v-icon size="15">mdi-plus</v-icon>
                                                                        </v-btn>
                                                                    </template>
                                                                    <v-card>
                                                                        <v-card-text class="pt-0 pb-1">
                                                                            <v-col cols="12" md="12">
                                                                                <!-- Header Text  -->
                                                                                <v-col cols="12" md="12" class="d-inline-flex">
                                                                                    <v-btn id="AutoTest_IP759" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                        <v-icon size="15">mdi-chevron-left</v-icon>
                                                                                    </v-btn>
                                                                                    <v-spacer></v-spacer>
                                                                                    <h3 class="text-left page-head pt-2">
                                                                                        {{TagHeaderName}}
                                                                                    </h3>
                                                                                    <v-spacer></v-spacer>
                                                                                    <v-btn id="AutoTest_IP760" text @click="NewTagMenu = false,SearchTagDetails=[]">
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
                                                                                                    <v-btn id="AutoTest_IP761" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
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
                                                                                                        <v-chip id="AutoTest_IP762" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                                label @click.native="fnAddToSelectedTag(index)">
                                                                                                            {{ index.TagName }}
                                                                                                            <v-icon id="AutoTest_IP763" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                                        </v-chip>
                                                                                                    </v-chip-group>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                            <v-col cols="12" md="12" class="text-center mt-3">
                                                                                                <v-btn id="AutoTest_IP764" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                                                    <v-btn id="AutoTest_IP765" color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                                <v-btn id="AutoTest_IP766" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                                    <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                                </v-btn>
                                                                <v-btn id="AutoTest_IP767" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                                    <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                                </v-btn>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                            <v-col md="12" class="chip-container mb-0 pt-0">
                                                                <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                                    <v-chip id="AutoTest_IP768" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                                        {{TagItem.TagName}}
                                                                    </v-chip>
                                                                </v-chip-group>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>

                                                </v-row>
                                            </v-card-text>
                                            <v-card-actions class="pb-4">
                                                <v-col>
                                                    <Default-Button-Outlined name="btnCancelSurvey" id="AutoTest_IP769" class="btn-122x36 float-right" title="Cancel" @click.native="CloseProjectSurveyDialog()"></Default-Button-Outlined>
                                                    <Secondary-Button name="btnConfirmSurvey" id="AutoTest_IP770" class="btn-122x36 mr-2 float-right" title="Confirm" @click.native="fnCreateProjectSurvey()" v-bind:disabled="fnCheckMandatory()"></Secondary-Button>
                                                </v-col>
                                            </v-card-actions>
                                        </v-card>
                                    </v-form>
                                </v-dialog>


                                <!-- Select Work flow-->
                                <v-dialog v-model="SelectWorkflowDialog" width="1100" persistent>
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-0">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                <img style="width: 70%" :src="Workflow_Tab_icon" />
                                            </v-avatar>Add Workflow
                                            <v-spacer></v-spacer>
                                            <v-btn name="btnCloseWorkflow" id="AutoTest_IP771" text @click="(SelectWorkflowDialog = false),
                (checkboxAddWorkflow = []),
                (selectedAddWorkflow = []),
                (SelectWorkflowDlgItems = []),(confirmError=false)">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text class="py-0">
                                            <v-container fluid class="pt-0">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-row no-gutters>
                                                            <v-col v-if="SelectWorkflowDlgItems.length > 0  && EnableWorkflow == true && confirmError==false" cols="12" md="12" class="pt-0 mb-2">
                                                                <h4 class="text-center heading-3">Please select workflow from below</h4>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row no-gutters>
                                                            <v-col v-if="SelectWorkflowDlgItems.length > 0  && EnableWorkflow == true && confirmError==true" cols="12" md="12" class="pt-0 mb-2">
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
                                                                                            You cannot change workflow, Survey Task is Approved.
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
                                                                                                   style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                                                                                   name="workflow"
                                                                                                   :checked="
                                      selectedAddWorkflow[0].checkboxindex ==
                                      index
                                        ? true
                                        : false
                                    "
                                                                                                   @click="
                                      (checkboxAddWorkflow[index] = true),
                                        fnCheck(addWorkflowItem, index)
                                    " />
                                                                                            <input type="radio"
                                                                                                   class="mt-3"
                                                                                                   style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                                                                                   name="workflow"
                                                                                                   v-else
                                                                                                   @click="
                                      (checkboxAddWorkflow[index] = true),
                                       fnCheck(addWorkflowItem, index)
                                    " />
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
                                                                                                {{
                                      addWorkflowItem.WfDescription.length >= 60
                                        ? addWorkflowItem.WfDescription.substring(
                                            0,
                                            60
                                          ) + "..."
                                        : addWorkflowItem.WfDescription
                                                                                                }}
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
                                                                            <Secondary-Button name="btnAddWorkflow" v-if="selectedAddWorkflow.length>0 " id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddSelectWorkflow()" v-show="EnableWorkflow != false"></Secondary-Button>
                                                                            <Secondary-Button v-else id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="confirmError=true"></Secondary-Button>
                                                                            <Default-Button-Outlined id="AutoTest_IP775" class="btn-122x36" title="Cancel" @click.native="(SelectWorkflowDialog = false),(confirmError=false),
                (checkboxAddWorkflow = []),
                (selectedAddWorkflow = []),
                (SelectWorkflowDlgItems = [])"></Default-Button-Outlined>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <!-- if no workflow available -->
                                                                    <v-row no-gutter v-if="SelectWorkflowDlgItems.length == 0">
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
                                                                                    {{
                              ViewWorkflowItem.WfName
                                                                                    }}
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
                                                                                       @click="
                                (checkboxAddWorkflow[radioSelecIndex] = true),
                                  fnCheck(ViewWorkflowItem, radioSelecIndex)
                              ">Select Workflow</v-btn>
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
                                                                                                            <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
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
                                                                            <Secondary-Button v-if="selectedAddWorkflow.length>0 " id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddSelectWorkflow()" v-show="EnableWorkflow != false"></Secondary-Button>
                                                                            <Secondary-Button v-else id="AutoTest_IP774" class="btn-122x36 mr-2" title="Confirm" @click.native="confirmError=true"></Secondary-Button>
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


                                <!-- Assign Users-->
                                <v-dialog v-model="AssignUserDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                            </v-avatar>Assign Users
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP780" text @click="AssignUserDialog=false">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text>
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-2 px-1">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" md="12" class="text-right pt-5">
                                                                <v-text-field name="txtUserName" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUser">
                                                                </v-text-field>
                                                            </v-col>

                                                            <div class="chip-container mb-2" v-if="selectedteamUsers.length>0">
                                                                <v-chip-group multiple column active-class="primary--text">
                                                                    <v-chip id="AutoTest_IP781" color="#DBF1FF" close close-icon="mdi-close" v-for="item in selectedteamUsers" :key="item.ID"
                                                                            v-model="chipUsrItem" @click:close="RemoveUser(item)" class="ma-2" small>
                                                                        <v-avatar size="12" left light :color="`${commonGetColor(item.Name)}`">
                                                                            <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                        </v-avatar>
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
                                                                              item-key="ID"
                                                                              sort-by="Name"
                                                                              fixed-header
                                                                              height="25vh"
                                                                              :search="searchUser"
                                                                              v-model="selectedteamUsers"
                                                                              class="elevation-0">

                                                                    <template v-slot:item.Name="{item}">
                                                                        <v-list class="py-0 table-v-list">
                                                                            <v-list-item class="my-0 px-0">
                                                                                <v-list-item-icon>
                                                                                    <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                                        <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
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
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-left" v-if="selectedteamUsers.length>0">
                                                                <v-row>
                                                                    <v-col cols="12" md="8" class="py-0">
                                                                        <v-list class="py-0">
                                                                            <v-list-item>
                                                                                <v-list-item-icon>
                                                                                    <v-avatar size="20" color="#FFF0C7" class="elevation-0">
                                                                                        <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                                    </v-avatar>
                                                                                </v-list-item-icon>
                                                                                <v-list-item-content>
                                                                                    Selected Users will be notify.
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                        </v-list>
                                                                    </v-col>
                                                                    <v-col cols="12" md="4" class="py-0">
                                                                        <!-- <v-checkbox color="success-check-box" input-value="true" label="Notify user" class="float-right"></v-checkbox> -->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                <Secondary-Button name="btnAddUsers" id="AutoTest_IP782" class="btn-122x36 mr-2" title="Add" @click.native="AssignUserDeatilsToSurvey()" v-bind:disabled="selectedteamUsers.length==0"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_IP783" class="btn-122x36" title="Cancel" @click.native="AssignUserDialog=false"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-divider></v-divider>
                                    </v-card>
                                </v-dialog>

                                <!--Edit S -->
                                <!--Edit Work flow -->
                                <!-- Remove Survey -->
                                <v-dialog v-model="RemoveSurveyDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove Survey
                                            <v-spacer></v-spacer>
                                            <v-btn name="btnCloseOperation" id="AutoTest_IP784" text="" @click="RemoveSurveyDialog=false,SelectedSurvey=[]">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to remove this Survey ?</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <v-row justify="center">
                                                                        <h5 class="page-head mt-4">
                                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                                                <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                                            </v-avatar>
                                                                            {{SurveyName}}
                                                                        </h5>
                                                                    </v-row>

                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn name="btnRemoveSurvey" id="AutoTest_IP785" color="secondary elevation-0" class="mr-2" @click="fnDeleteProjectSurvey(2)">Remove</v-btn>
                                                                    <v-btn name="btnCancelOperation" id="AutoTest_IP786" outlined color="grey" @click="fnCancelDeleteProjectSurvey(),SelectedSurvey=[]">Cancel</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>
                                <v-dialog v-model="ImportTemplateDialog" width="800" persistent>
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Import Survey
                                            <v-spacer></v-spacer>
                                            <v-btn name="btnCloseOperation" id="AutoTest_IP784" text="" @click="ImportTemplateDialog=false,SelectedSurvey=[]">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text class="pt-0">
                                            <v-container fluid class="pt-4">
                                                <v-row no-gutters style="background-color: #F5F6F8; height: 100%">
                                                    <v-col cols="12">
                                                        <v-col cols="12" class="v-scrolling-div px-4 myTasksInnerCardHeight">
                                                            <v-row class="d-flex justify-center">
                                                                <v-col md="12" class="mt-6" align="center">
                                                                    <!-- Drag and Drop Section-->
                                                                    <div class="text-center box_outline" @dragover.prevent
                                                                         @drop.prevent v-if="SurveyFile.length == 0"
                                                                         @click="handleFileImport">
                                                                        <div @drop="dragFile_Survey">
                                                                            <div>
                                                                                <v-icon color="#535353" size="70">mdi-file</v-icon>
                                                                            </div>
                                                                            <div v-if="SurveyFile.length == 0">
                                                                                <a>Drag & Drop The Filled Out Form</a><br />
                                                                                <a>Upload Only Xls File (Not Above 15mb Of Size)</a>
                                                                            </div>
                                                                            <div v-if="SurveyFile.length" class="mt-4">
                                                                                <div v-for="(file, ind) in SurveyFile" :key="ind">
                                                                                    <v-chip close="" label="" color="white">
                                                                                        <v-icon size="20" color="success"
                                                                                                height="100">mdi-file-delimited-outline</v-icon>
                                                                                        {{ file.name }}
                                                                                    </v-chip>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="mt-4" align="center"
                                                                         v-if="SurveyFile.length == 0">
                                                                        <input ref="uploader" class="d-none" type="file" accept=""
                                                                               @change="OnFileUploadsurvey($event)">
                                                                      
                                                                        <Secondary-Button class="btn-122x36 mr-2" title="Download"
                                                                                          @click.native="ImportTemplate(ImportSurveyId)"></Secondary-Button>

                                                                    </div>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" align="right" style="background-color:white" class="py-2"
                                                               v-if="SurveyFile.length > 0">
                                                            <Default-Button-Outlined class="btn-122x36 mr-2" title="Cancel"
                                                                                     @click.native="CancelImport()"></Default-Button-Outlined>
                                                            <Secondary-Button class="btn-122x36 mr-2"
                                                                              v-bind:disabled="IsvalidexcelData == false" title="Next"
                                                                              @click.native="ImportSurvey()"></Secondary-Button>
                                                        </v-col>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>

                                </v-dialog>

                                <!-- Import User -->
                                <v-dialog v-model="ImportSurveyDialogBox" width="800" persistent>
                                    <v-card width="100%" class="elevation-0" style="overflow:hidden">
                                        <v-card-title primary-title="" class="page-head pa-4 pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1">
                                                <v-icon size="20" color="primary">mdi-database</v-icon>
                                            </v-avatar>Import & Register
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP380" text @click="ImportSurveyDialogBox = false, CancelImport()">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider class="mt-3"></v-divider>
                                        <v-card-text class="pa-3 v-scrolling-div" style="height:54vh">
                                            <v-stepper v-model="ImportRegSurveySteps" class="elevation-0 py-0">
                                                <!-- stepper Header -->
                                                <v-row :justify="justifyCenter">
                                                    <v-col md="6" class="px-6 pa-0">
                                                        <v-stepper-header class="elevation-0">
                                                            <v-stepper-step :complete="ImportRegSurveySteps > 1" step="1" color="inherit">Upload
                                                            </v-stepper-step>
                                                            <v-divider></v-divider>
                                                            <v-stepper-step :complete="ImportRegSurveySteps > 2" step="2" color="inherit">Format
                                                                Check</v-stepper-step>
                                                        </v-stepper-header>
                                                    </v-col>
                                                </v-row>
                                                <!-- stepper Items -->
                                                <v-stepper-items>
                                                    <v-stepper-content step="1" class="px-3 py-0">
                                                        <v-container fluid class="pa-0">
                                                            <v-row no-gutters class="uploadBoxHeight pa-0">
                                                                <v-col cols="12" md="12" class="pa-0">
                                                                    <h4 class="text-center heading-3">Step 1: Upload your CSV file set create a
                                                                        new survey.</h4>
                                                                </v-col>
                                                                <v-col cols="12" md="12" class="text-center">
                                                                    <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
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
                                                                                        Your CSV file must map all our mandatory fields<br />and
                                                                                        your file cannot exceed more than 1000 rows
                                                                                    </p>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <v-btn id="AutoTest_IP381" color="grey" outlined dark
                                                                                        class="elevation-0" height="40"
                                                                                        @click="downloadSampleFile()">
                                                                                        <v-icon size="26"
                                                                                            color="grey">mdi-file-download-outline</v-icon>Download
                                                                                        an example file
                                                                                    </v-btn>
                                                                                </v-col>
                                                                                <v-col align-center justify-center>
                                                                                    <v-file-input prepend-icon="" label="Upload File" outlined
                                                                                        dense id="file" v-model="fileInput"
                                                                                        prepend-inner-icon="mdi-file-upload-outline"
                                                                                        accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                        @change="OnFileUpload($event)">
                                                                                    </v-file-input>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-stepper-content>
                                                    <v-stepper-content step="2" class="px-3 py-0">
                                                        <v-row no-gutters class="pa-0">
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0 pb-3">
                                                                <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0 px-sm-3">
                                                                <vue-excel-editor no-paging :resetOnClick="resetOnClick" disable-panel-setting
                                                                    disable-panel-filter v-model="lstImportSurvey">
                                                                    <div v-for="Hearder in lstSurveyHearder">
                                                                        <vue-excel-column :field="Hearder.text" :type="Hearder.type"
                                                                            :mandatory="Hearder.mandatory" :options="Hearder.options"
                                                                            width="80px" />
                                                                    </div>
                                                                </vue-excel-editor>
                                                            </v-col>
                                                        </v-row>
                                                    </v-stepper-content>
                                                    <v-stepper-content step="3" class="px-3 py-0">
                                                        <v-row>
                                                            <v-col md="12">
                                                                <h4 class="text-center heading-3">Super !</h4>
                                                            </v-col>
                                                            <v-col md="12" class="text-center">
                                                                <v-avatar size="150" tile="">
                                                                    <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                </v-avatar>
                                                            </v-col>
                                                            <v-col md="12">
                                                                <p class="text-medium text-center">
                                                                    Your Survey has been created successfully. Bask in your glory !
                                                                </p>
                                                            </v-col>
                                                        </v-row>
                                                    </v-stepper-content>
                                                </v-stepper-items>
                                            </v-stepper>
                                        </v-card-text>
                                        <v-card-actions class="pb-4 pr-4">
                                            <v-row class="my-0 py-0">
                                                <template v-if="ImportRegSurveySteps == 1">
                                                    <!-- step1 -->
                                                    <v-col cols="12" xs="12" sm="12" md="12"
                                                        class="text-center text-md-right py-0 my-0">
                                                        <Default-Button-Outlined id="AutoTest_IP382" class="float-right btn-122x36"
                                                            title="Cancel" @click.native="CancelImport()"></Default-Button-Outlined>
                                                        <Secondary-Button id="AutoTest_IP383" class="mr-2 float-right btn-122x36"
                                                            title="Next" @click.native="(ImportRegSurveySteps = 2)"
                                                            v-bind:disabled="lstImportSurvey.length == 0"></Secondary-Button>
                                                    </v-col>
                                                </template>
                                                <template v-if="ImportRegSurveySteps == 2">
                                                    <!-- step 2 -->
                                                    <v-col cols="12" xs="12" sm="12" md="12"
                                                        class="text-center text-md-right py-0 my-0">
                                                        <Default-Button-Outlined id="AutoTest_IP384" class="float-right btn-122x36"
                                                            title="Cancel" @click.native="CancelImport()"></Default-Button-Outlined>
                                                        <Secondary-Button id="AutoTest_IP385" class="mr-2 float-right btn-122x36"
                                                            title="Next" @click.native="ImportSurveyList()"></Secondary-Button>
                                                    </v-col>
                                                </template>
                                                <template v-if="ImportRegSurveySteps == 3">
                                                    <!-- step 3 -->
                                                    <v-col md="12" class="text-center">
                                                        <Secondary-Button id="AutoTest_IP386" class="btn-122x36" title="Done"
                                                            @click.native="ImportRegSurveySteps = 1,ImportSurveyDialogBox=false"></Secondary-Button>
                                                    </v-col>
                                                </template>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                </v-dialog>

                                <!-- Duplicate-->
                                <v-dialog v-model="CreateDuplicateSurveyDialog" width="800" persistent>
                                    <v-card>
                                        <v-card-title primary-title="" class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                <v-icon size="15" color="primary">mdi-note-multiple-outline</v-icon>
                                            </v-avatar>
                                            Duplicate
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP792" text @click="CreateDuplicateSurveyDialog=false">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text>
                                            <h4 class="text-center heading-3 mt-4">Please enter the basic details to create a duplicate survey</h4>
                                            <v-row justify="center">
                                                <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                                    <v-text-field name="txtDuplicateSurveyName" outlined="" dense="" label="Survey Name" v-model="SurveyName">
                                                    </v-text-field>
                                                    <v-textarea name="txtDuplicateSurveyDescription" outlined dense label="Enter Description" value="" hide-details v-model="SurveyDescription"></v-textarea>
                                                    <p>
                                                        Beneficiary Linked
                                                        <v-switch name="toggleBtnBeneficiaryLinked" hide-details class="ml-2" style="display:inline-block"
                                                                  color="secondary" v-model="BeneficiaryLinked"></v-switch>
                                                    </p>
                                                    <v-select id="drpBeneficiaryType" v-show="BeneficiaryLinked" dense="" :items="lstBeneficiaryType" :item-text="i => i.Name" :item-value="i => i.ID" label="Select Beneficiary Type" outlined="" v-model="BeneficiaryTypeID">
                                                    </v-select>
                                                    <v-checkbox name="checkboxAssignToSamePeople" color="success-check-box" label="Assign to same People" hide-details v-model="vmChkSameUsers"></v-checkbox>
                                                    <v-checkbox name="checkboxAssignToSameWorkflow" color="success-check-box" label="Assign  same Workflow" hide-details v-model="vmChkSameWF"></v-checkbox>
                                                    <v-checkbox name="checkboxCopySectionsAndQuestions" color="success-check-box" label="Copy sections and question settings" hide-details v-model="vmChkSameSecQstn"></v-checkbox>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                        <v-card-actions class="pb-4">
                                            <v-col>
                                                <!-- </v-btn> -->
                                                <Default-Button-Outlined id="AutoTest_IP793" class="btn-122x36 float-right" title="Cancel" @click.native="CreateDuplicateSurveyDialog=false"></Default-Button-Outlined>
                                                <Secondary-Button name="btnConfirmDuplicateSurvey" id="AutoTest_IP794" class="btn-122x36 mr-2 float-right" title="Confirm" @click.native="fnCreateDuplicateSurvey()"></Secondary-Button>

                                            </v-col>
                                        </v-card-actions>
                                    </v-card>
                                </v-dialog>

                                <!-- Link Beneficiary Type-->

                                <v-dialog v-model="BeneficiaryTypeLinkDialog" width="800" persistent>
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                <v-icon size="20" color="primary">mdi-link-box-outline</v-icon>
                                            </v-avatar>Relink Beneficiary Type
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP795" text @click="CancelBeneficiaryTypeLinkDialog()">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text>
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-2 px-1">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" md="12" class="text-right pt-5">
                                                                <v-text-field name="txtBeneficiaryTypeName" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="SearchBeneficaryType">
                                                                </v-text-field>
                                                            </v-col>

                                                            <div class="chip-container mb-2" v-if="SelectedBeneficiaryTypeList.length>0">
                                                                <v-chip-group multiple column active-class="primary--text">
                                                                    <v-chip id="AutoTest_IP796" color="#DBF1FF" close close-icon="mdi-close" v-for="item in SelectedBeneficiaryTypeList" :key="item.ID" @click:close="RemoveBeneficiary(item)" class="ma-2" small>
                                                                        <v-icon left color="#707070">mdi-account</v-icon>
                                                                        {{ item.Name }}
                                                                    </v-chip>
                                                                </v-chip-group>

                                                            </div>
                                                            <!--v-chip for Assign roles ends here -->
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <!--User List with checkbox add here-->
                                                                <v-data-table :headers="BeneficiaryTypeTableheaders"
                                                                              :items="BeneficiaryTypeTableItems"
                                                                              show-select
                                                                              item-key="ID"
                                                                              sort-by="Name"
                                                                              single-select
                                                                              fixed-header
                                                                              height="25vh"
                                                                              :search="SearchBeneficaryType"
                                                                              v-model="SelectedBeneficiaryTypeList"
                                                                              class="elevation-0">
                                                                </v-data-table>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right mt-4">
                                                                <Secondary-Button name="btnAddBeneficiaryType" id="AutoTest_IP797" class="btn-122x36 mr-2" title="Add" @click.native="fnAddSelectedBeneficiryType()" v-bind:disabled="SelectedBeneficiaryTypeList.length==0"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_IP798" class="btn-122x36" title="Cancel" @click.native="CancelBeneficiaryTypeLinkDialog()"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-divider></v-divider>
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
</div>
</template>
<script>
    import AddButtonFab from "@/components/add-button-fab.vue";
    import SuccessButtonMd from "@/components/success-button-md.vue";
    import PrimaryButtonLight from "@/components/primary-button-light.vue";
    import OutlinedButtonDark from "@/components/outlined-button-dark.vue";
    import SuccessButton from "@/components/success-button.vue";
    import PrimaryButton from "@/components/primary-button-normal.vue";
    import BulkActionButtonOutlined from "@/components/bulk-action-button-outlined.vue";
    import CancelButtonOutlinedSm from "@/components/cancel-button-outlined-sm.vue";
    import PreviousButtonOutlined from "@/components/previous-button-outlined.vue";
    import NextButtonPrimaryLight from "@/components/next-button-blue.vue";
    import PrimaryButtonOutlined from "@/components/primary-button-outlined.vue";
    import SecondaryButton from "@/components/secondary-button.vue";
    import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
    import PreLoader from "@/components/pre-loader.vue";
    import objUtils from '../../utils.js';
   

    var objManageProjectSurvey;
    async function importscript() {
        return Promise.all([
            import("../../BL/ProjectSurvey.js").then((mod) => {
                objManageProjectSurvey = new mod.ProjectSurvey();
            }),
        ]);
    }
    var objManageProjectActivity;
    async function importscriptActivity() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then((mod) => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            }),
        ]);
    }
    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                alphanumericRules: [
                    (v) => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || "Please enter valid name",
                ],
                PreLoader: false,
                WorkspaceID: 0,
                InitiativeId: 0,
                ProjectId: 0,
                SurveyId: 0,
                GroupId: 0,
                SID: 0,
                ResCount: 0,
                justifyCenter: "center",
                SurveyDetailsTableheaders: [
                    { text: "ID", align: " d-none", value: "SurveyID" },
                    { text: "Survey Name", align: "left", value: "SurveyName" },
                    { text: "Created", value: "CreatedDate" },
                    { text: "Last Modified", value: "UpdatedDate" },
                    { text: "Workflow", value: "WorkFlowName" },
                    { text: "Beneficiary Type", value: "BeneficiaryTypeName" },
                    { text: "Users", value: "AssignedUsers" },
                    { text: "Response Count", align: "center", value: "ResponseCount" },
                    { text: "Action", align: "center", value: "Actions", width: "100px" },
                ],
                FilterSurveyDetailsTableItems: [],
                //SurveyDetailsTableItems: [
                //    {
                //        ID: '1',
                //        SurveyName: 'GPS Demo Survey',
                //        Created: '04/03/2020',
                //        LastModified: '04/05/2021',
                //        Workflow: "Sample Workflow 1",
                //        BeneficiaryType: "Beneficiary 1",
                //        assignedUsers: [{ Name: 'John' }, { Name: 'Arathy' }],
                //        ResponseCount: 1
                //    }
                //],
                selectedItem: "",
                items: [
                    { text: "This month" },
                    { text: "This quarter" },
                    { text: "This Year" },
                ],
                filterContent: false,
                dateRangeHeaderName: 'Date Range',
                dateRangeMenu: false,
                frequencyMenu: false,
                workflowMenu: false,
                assignedUserMenu: false,
                beneficiaryTypeMenu: false,
                filterChipsList: [],
                lstuserTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                SearchSurveyDetails: "",
                SelectedSurvey: [],
                EditSurveyDialog: false,
                SelectWorkflowDialog: false,
                SelectWorkflowDlgSteps: 0,
                SelectWorkflowDlgItems: [],
                checkboxAddWorkflow: [],
                selectedAddWorkflow: [],
                ViewWorkflowItem: { WfLevelDetails: [] },
                AssignUserDialog: false,
                LinkBeneficiaryType: false,
                DuplicatedialogBox: false,
                RemoveBulkSurveyDialog: false,

                SelectWorkflowDlgSteps: 0,

                offset: false,
                closeonClick: false,
                closeOnContentClick: false,
                AssignAccountUserPopUp: false,

                SelectUserSelectedUsersList: [],
                searchUser: "",
                SelectUserTableheaders: [
                    { text: "UserID", align: " d-none", value: "ID" },
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "Role" },
                ],
                SelectUserTableItems: [
                    {
                        UserID: "1",
                        Name: "Sam",
                        UserRole: "Admin",
                    },
                ],
                chipUsrItem: "",

                AssignAccountUserPopUpEdit: false,
                RemoveSurveyDialog: false,

                // Import Survey
                ImportSurveyDialogBox: false,
                ImportSurveySteps: 0,
                fileInput: null,
                csvfile: {
                    name: "",
                    size: 0,
                    namedisplay: "",
                },
                csvfiledata: [],

                CreateSurveyDialog: false,
                BeneficiaryLinked: false,
                SurveyDetailsTableItems: [],
                selectedteamUsers: [],
                selectedteamUsers1: [],
                lstPeriodicity: [],
                lstWorkFlow: [],
                lstBeneficiaryType: [],
                lstUsers: [],
                SurveyName: "",
                SurveyDescription: "",
                WorkflowID: 0,
                ReportingFrequency: 0,
                ReportingFrequencyName:"",
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,
                TotalWFLevel:0,
                BeneficiaryTypeID: 0,
                rules: {
                    required: (value) => !!value || "Required.",
                },
                creatEditTitle: "",
                creatEditDescription: "",
                data: [
                    [
                        "Survey Name Here",
                        "Survey description here",
                        "Workflow Name here",
                        "*Please enter StartDate in mm-dd-yyyy format",
                        "*Please enter EndDate in mm-dd-yyyy format EndDate",
                        //"Never/Once/Daily/Weekly etc",
                        "Beneficiary Type Name here",
                    ],
                ],
                FrmDate: null, //new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,
                //ReportingFrequency: "",
                CreateDuplicateSurveyDialog: false,
                ImportTemplateDialog: false,
                BeneficiaryTypeLinkDialog: false,
                BeneficiaryTypeTableheaders: [
                    { text: "ID", align: " d-none", value: "ID" },
                    { text: "Name", value: "Name" },
                    { text: "Created Date", value: "CreatedDate" },
                ],
                BeneficiaryTypeTableItems: [],
                SearchBeneficaryType: "",
                SelectedBeneficiaryTypeList: [],

                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                SelectBeneficiaryType: [
                    "Beneficiary 1",
                    "Beneficiary 2",
                    "Beneficiary 3",
                    "Beneficiary 4",
                ],
                PaymentLinked: "",

                // Tags ------------
                NewTagMenu: false,
                SearchTagDetails: "",
                AllTagListSteps: 0,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ["#00FF00", "#FF0000"],
                    ["#F0EB06", "#4B0D64"],
                    ["#00FFFF"],
                    ["#0000FF"],
                ],
                allTags: false,
                EditTagName: "",
                type: "hex",
                hex: "#FF00FF",

                tagColor: "",
                tagName: "",
                tagID: "",
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                filterAllTagList: [],
                vmChkSameUsers: true,
                vmChkSameWF: true,
                vmChkSameSecQstn: true,
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                //startDate: new Date().toISOString().substr(0, 10),
                startDate: "",
                mStartDate: false,
                //endDate: new Date().toISOString().substr(0, 10),
                endDate: "",
                mEndDate: false,
                ProjectStartDate: "",
                ProjectEndDate: "",
                EnableWorkflow: true,
                userimage: "",
                confirmError: false,
                Description_icon: require("@/assets/ProjectWorkflow/Description_icon.svg"),
                Levels_icon: require("@/assets/ProjectWorkflow/Levels_icon.svg"),
                Workflow_Icon: require("@/assets/ProjectWorkflow/Workflow_Icon.svg"),
                Workflow_Tab_icon: require("@/assets/ProjectWorkflow/Workflow_Tab_icon.svg"),
                radioSelecIndex: "",

                //Filtration
                FiterStartDate: "",
                FiterEndDate: "",
                ImportRegSurveySteps: 1,
                lstSurveyHearder: [
                    {
                        mandatory: 'true',
                        text: "SurveyName",
                        type: "string",
                        value: "SurveyName"
                    },
                    {
                        mandatory: 'false',
                        text: "SurveyDescription",
                        type: "string",
                        value: "SurveyDescription"
                    },
                    {
                        mandatory: 'false',
                        text: "WorkFlowName",
                        type: "select",
                        value: "WorkFlowName",
                        options: []
                    },
                    {
                        mandatory: 'true',
                        text: "StartDate",
                        type: "date",
                        value: "StartDate"
                    },
                    {
                        mandatory: 'true',
                        text: "EndDate",
                        type: "date",
                        value: "EndDate"
                    },
                    //{
                    //    mandatory: false,
                    //    text: "ReportingFrequencyName",
                    //    type: "select",
                    //    value: "ReportingFrequencyName",
                    //    options: []
                    //},
                    {
                        mandatory: 'false',
                        text: "BeneficiaryTypeName",
                        type: "select",
                        value: "BeneficiaryTypeName",
                        options: []

                    }],
                lstImportSurvey: [],
                resetOnClick: 0,
                lstSurveySections: [],
                SurveyFile: [],
                selectedFile: null,
                ImportSurveyId:0

            };
        },
        components: {
            "Add-Button-Fab": AddButtonFab,
            "Success-Button-Md": SuccessButtonMd,
            "Primary-Button-Light": PrimaryButtonLight,
            "Outlined-Button-Dark": OutlinedButtonDark,
            "Primary-Button-Outlined": PrimaryButtonOutlined,
            "Primary-Button": PrimaryButton,
            "Bulk-Action-Button-Outlined": BulkActionButtonOutlined,
            "Cancel-Button-Outlined-Sm": CancelButtonOutlinedSm,
            "Previous-Button-Outlined": PreviousButtonOutlined,
            "Next-button-primary-light": NextButtonPrimaryLight,
            "Success-Button-Md": SuccessButtonMd,
            "Success-Button": SuccessButton,
            "Secondary-Button": SecondaryButton,
            "Default-Button-Outlined": DefaultButtonOutlined,
            "Pre-Loader": PreLoader,
        },
        computed: {
            //color: {
            //    get() {
            //        return this[this.type]
            //    },
            //    set(v) {
            //        this[this.type] = v
            //    },
            //},
            FormattedDate() {
                return objUtils.common.formatDate(this.FrmDate);
            },
            computedStartDate() {
                return objUtils.common.formatDate(this.startDate);
            },
            computedEndDate() {
                return objUtils.common.formatDate(this.endDate);
            },
        },
        watch: {
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter((x) =>
                        x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase())
                    );
            },
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.InitiativeId = this.$route.query.InitID;

            await importscript();
            await importscriptActivity();
            await this.GetProjectSurveyList();
            await this.fnGetGroupDetailsByprjID();
            await this.GetProjectDate();
            await this.fnGetWorkflows();
            await this.GetReportingFrequencyList();
            await this.GetProjectBeneficiaryType(1);
            await this.fnGetTeamUsersList();
            await this.fnGetAllTags();
            await this.GetSurveyExcelOptions();


        },
        methods: {
           
          
            /// <summary>
            ///  CancelImport :- Function used to cancel import survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CancelImport() {
                this.resetOnClick = 2
                this.ImportSurveyDialogBox = false;
                this.ImportRegSurveySteps = 1;
                this.fileInput = null;
                this.lstImportSurvey = [];
                this.resetOnClick = 0
                this.lstImportSurvey = []
            },

            /// <summary>
            ///  OnFileUpload :- Function used to upload a csv file to create a new survey
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            OnFileUpload(event) {
                this.csvFileName = this.fileInput.name;
                var vm = this;
                if (window.FileReader) {
                    var reader = new FileReader();
                    reader.readAsText(this.fileInput);
                    // Handle errors load
                    reader.onload = function (event) {
                        var csv = event.target.result;
                        vm.ReadCsvFile(csv);
                    };
                    reader.onerror = function (evt) {
                        if (evt.target.error.name == "NotReadableError") {
                            this.Msg = "Cannot read file !";
                            this.infoSnackbarMsg = true;
                        }
                    };
                } else {
                    this.Msg = "FileReader is not supported in this browser.";
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  ReadCsvFile :- Function used to read a csv file
            /// </summary>
            /// <param name="csv"></param>
            /// <returns> </returns>
            ReadCsvFile(csv) {
           
                var lines = csv.split(/\r\n|\n/);
                var headers = lines[0].split(",");
                //var headers = objUtils.common.MergeCommas(lines[0]);
                if (this.lstSurveyHearder.length == headers.length) {
                    for (var i = 1; i < lines.length; i++) {
                        var obj = {};

                        //var currentline = lines[i].split(",");
                        var currentline;
                        currentline = objUtils.common.MergeCommas(lines[i]);
                        //

                        if (currentline != "") {
                            for (var j = 0; j < headers.length; j++) {
                                var dataType = this.lstSurveyHearder[j].type;
                                if (dataType == "date") {
                                    var timestamp = Date.parse(currentline[j]);

                                    if (isNaN(timestamp) || (currentline[j] == "" && this.lstSurveyHearder[j].mandatory == 'true')) {
                                        obj[this.lstSurveyHearder[j].value] = "Invalid Input";
                                    }
                                    else {
                                        obj[this.lstSurveyHearder[j].value] = currentline[j];
                                    }
                                }
                                else {
                                    if (currentline[j] == "" && this.lstSurveyHearder[j].mandatory == 'true') {
                                        obj[this.lstSurveyHearder[j].value] = "Invalid Input";
                                    }
                                    else if (typeof this.lstSurveyHearder[j].options != 'undefined' && currentline[j] != "") {
                                        if (!this.lstSurveyHearder[j].options.includes(currentline[j])) {
                                            obj[this.lstSurveyHearder[j].value] = "Invalid Input";
                                        }
                                        else {
                                            obj[this.lstSurveyHearder[j].value] = currentline[j];
                                        }
                                    }

                                    else {
                                        obj[this.lstSurveyHearder[j].value] = currentline[j];
                                    }
                                }

                            }
                            this.lstImportSurvey.push(obj);
                        }
                    }
                } else {
                    this.Msg =
                        "Survey format is not matching for the selected file...  ";
                    this.infoSnackbarMsg = true;
                }
            },

            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },

            resetValidation() {
                this.$refs.form.resetValidation();
            },

            /// <summary>
            ///  fnInitializeSteper :- Function used to initialize stepper
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
            ///  IsValidPeriodicityDate :- Function used to check the periodicity date is valid date or invalid date (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidPeriodicityDate() {
                if (this.startDate != null) {
                    if (Date.parse(this.FrmDate) < Date.parse(this.startDate)) {
                        this.Msg =
                            "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                } else {
                    if (Date.parse(this.FrmDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg =
                            "Invalid Date : Report Frequency Date should be greater than Project Start Date - " +
                            objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }

                if (this.endDate != null) {
                    if (Date.parse(this.FrmDate) > Date.parse(this.endDate)) {
                        this.Msg =
                            "Invalid Date : Report Frequency Date should be less than End Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                } else {
                    if (
                        Date.parse(this.FrmDate) >
                        Date.parse(this.ProjectEndDate.substr(0, 10))
                    ) {
                        this.Msg =
                            "Invalid Date : Report Frequency Date should be less than Project End Date- " +
                            objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                return true;
            },

            /// <summary>
            ///  IsValidDate :- Function used to check the date is valid date or invalid date (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg =
                        "Invalid Date : Start date should not be less than Project Start Date - " +
                        objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg =
                        "Invalid Date : Start date should not be greater than Project End Date - " +
                        objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (
                    Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))
                ) {
                    this.Msg =
                        "Invalid Date : End date should not be greater than Project End Date - " +
                        objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg =
                        "Invalid Date : End date should be greater than Project Start Date (" +
                        objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10)) +
                        ")";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                // startdate/ Enddate  not < todays date
                //enddate  any valid range - Max value
            },

            /// <summary>
            ///  GetProjectDate :- Function used to fetch project startDate and project endDate
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectDate() {
                var result = await objManageProjectSurvey.GetProjectDate(this.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    this.ProjectStartDate = result.PrjStartDate;
                    this.ProjectEndDate = result.PrjEndDate;
                }
            },

            /// <summary>
            ///  fnGetWorkflows :- Function used to fetch project workflows
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetWorkflows() {
                var result = await objManageProjectSurvey.GetProjectWorkFlow(
                    this.ProjectId
                );
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstWorkFlow = result.data;
                }
            },

            /// <summary>
            ///  GetReportingFrequencyList :- Function used to fetch reporting frequency list (Week, Month, MonthDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetReportingFrequencyList() {
                var result = await objManageProjectSurvey.GetReportingFrequencyList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if ((result.status = "success")) {
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

            /// <summary>
            ///  GetProjectBeneficiaryType :- Function used to get project beneficiary type
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async GetProjectBeneficiaryType(type) {
                var data = new FormData();
                data.append("projectId", this.ProjectId);
                data.append("surveyId", this.SurveyId);

                this.lstBeneficiaryType = [];
                var result = await objManageProjectSurvey.GetProjectBeneficiaryType(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    if (type == 1) {
                        this.lstBeneficiaryType = result.data;
                    } else {
                        if (result.data != null && result.data != undefined) {
                            this.BeneficiaryTypeTableItems = result.data;
                            var rst = result.data.filter((x) => x.IsSelected == 1);
                            this.SelectedBeneficiaryTypeList = rst;
                        }
                    }
                }
            },

            /// <summary>
            ///  GetProjectSurveyList :- Function used to fetch project survey list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectSurveyList() {
                this.PreLoader = true;
                var result = await objManageProjectSurvey.GetProjectSurveyList(
                    this.ProjectId
                );
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SurveyDetailsTableItems = result.data;
                    this.FilterSurveyDetailsTableItems = result.data;
                }
                this.PreLoader = false;
            },

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter survey based on date range option
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {
                this.filterChipsList = [];

                this.FilterSurveyDetailsTableItems = this.SurveyDetailsTableItems;

                //date range filter
                if (this.FiterStartDate != '' && this.FiterEndDate != '') {

                    this.filterChipsList.push({ ChipID: 777, ChipName: objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate) });
                    this.dateRangeHeaderName = objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate);
                    this.FilterSurveyDetailsTableItems = this.FilterSurveyDetailsTableItems.filter(x => x.StartDate.substr(0, 10) >= objUtils.common.formatDate(this.FiterStartDate) && x.EndDate.substr(0, 10) <= objUtils.common.formatDate(this.FiterEndDate));
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
            ///  fnGetTeamUsersList :- Function used to fetch team user list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetTeamUsersList() {
                var data = new FormData();
                data.append("GroupId", this.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectActivity.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstUsers = result.data;
                }
            },

            /// <summary>
            ///  fnGetGroupDetailsByprjID :- Function used to fetch group details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProjectSurvey.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.GroupId = result.data.GroupId;
                }
            },

            /// <summary>
            ///  fnCheckMandatory :- Function used to check mandatory fields in the new survey creation page
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false </returns>
            fnCheckMandatory() {
                if (
                    this.SurveyName == "" ||
                    //  this.WorkflowID == 0 ||this.selectedteamUsers == 0 ||
                    this.ReportingFrequency == 0 ||
                    (this.ReportingFrequency == 2 &&
                        (this.FrmDate == "" || this.FrmDate == null)) ||
                    (this.BeneficiaryLinked == true && this.BeneficiaryTypeID == 0) ||
                    this.startDate == "" ||
                    this.startDate == null ||
                    this.endDate == "" ||
                    this.endDate == null
                ) {
                    return true;
                } else {
                    return false;
                }
            },

            /// <summary>
            ///  CloseProjectSurveyDialog :- Function used to close project survey dialog box
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseProjectSurveyDialog() {
                this.SurveyName = "";
                this.SurveyDescription = "";
                this.WorkflowID = 0;
                this.ReportingFrequency = 0;
                this.BeneficiaryTypeID = 0;
                this.CreateSurveyDialog = false;
                this.selectedteamUsers = [];
                this.SelectedTag = [];
                this.startDate = null;
                this.endDate = null;
                this.resetValidation();
            },

            /// <summary>
            ///  fnCreateProjectSurvey :- Function used to create a new project survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateProjectSurvey() {
                this.PreLoader = true;
                if (this.ReportingFrequency == 2) {
                    if (!this.IsValidPeriodicityDate()) {
                        this.Msg = " Invalid PeriodicityDate ";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }

                if (!this.BeneficiaryLinked) {
                    this.BeneficiaryTypeID = 0;
                }

                var data = new FormData();
                data.append("SurveyID", this.SurveyId);
                data.append("SurveyName", this.SurveyName);
                data.append("SurveyDescription", this.SurveyDescription);
                data.append("StartDate", this.startDate);
                data.append("EndDate", this.endDate);

                data.append("ProjectId", this.ProjectId);
                data.append("WorkflowID", this.WorkflowID);
                data.append("ReportingFrequency", this.ReportingFrequency);
                data.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                data.append("AssignedUsers", JSON.stringify(this.selectedteamUsers));
                data.append("AssignedTags", JSON.stringify(this.SelectedTag));

                data.append("ReportingMonth", this.ReportingMonth);
                data.append("ReportingMonthDay", this.ReportingMonthDay);
                data.append("ReportingWeek", this.ReportingWeek);
                data.append("ReportingOnceDate", this.FormattedDate);
                data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);

                var result = await objManageProjectSurvey.SaveProjectSurvey(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.CreateSurveyDialog = false;
                        if (this.SurveyId > 0) {
                            this.Msg = "Survey Successfully updated";
                            this.successSnackbarMsg = true;
                        } else {
                            this.Msg = "Survey Successfully Created";
                            this.successSnackbarMsg = true;
                        }
                        this.selectedteamUsers = [];
                        await this.ClearBindData();
                        this.GetProjectSurveyList();
                    } else {
                        if (result.status == "nameexists") {
                            this.Msg = "Survey Name Exists";
                            this.infoSnackbarMsg = true;
                        } else if (result.status == "rptfrequencyfailed") {
                            this.Msg = "Reporting Frequency Failed";
                            this.errorSnackbarMsg = true;
                        } else {
                            this.Msg = "Failed";
                            this.errorSnackbarMsg = true;
                        }

                    }
                }
                this.PreLoader = false;
            },

            /// <summary>
            ///  CloseAssignUserDialog :- Function used to close assignuser dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseAssignUserDialog() {
                this.AssignAccountUserPopUp = false;
                this.selectedteamUsers1 = [];
            },

            /// <summary>
            ///  AssignUserToSurvey :- Function used to assign users to survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AssignUserToSurvey() {
                this.AssignAccountUserPopUp = false;
                this.selectedteamUsers = this.selectedteamUsers1;
                this.selectedteamUsers1 = [];
            },

            /// <summary>
            ///  OpenAssignUserDialog :- Function used to open assignuser dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            OpenAssignUserDialog() {
                this.AssignAccountUserPopUp = true;
                this.selectedteamUsers1 = this.selectedteamUsers;
            },

            /// <summary>
            ///  fnDeleteProjectSurvey :- Function used to remove project survey as bulk or individually
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            async fnDeleteProjectSurvey(Type) {
                var data = new FormData();

                if (Type == 1) {
                    data.append("SurveyID", JSON.stringify(this.SelectedSurvey));
                    data.append("Type", 1);
                } else {
                    data.append("SurveyID", JSON.stringify(this.SurveyId));
                    data.append("Type", 2);
                }

                var result = await objManageProjectSurvey.DeleteProjectSurvey(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        //   this.GroupId = result.data.GroupId;
                        this.RemoveSurveyDialog = false;
                        this.Msg = "Survey Successfully removed";
                        this.successSnackbarMsg = true;
                        this.GetProjectSurveyList();
                        this.SurveyId = 0;
                        this.SurveyName = "";
                    } else {
                        this.Msg = result.status;//"Failed";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnCancelDeleteProjectSurvey :- Function used to cancel project survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCancelDeleteProjectSurvey() {
                this.SurveyId = 0;
                this.SurveyName = "";
                this.RemoveSurveyDialog = false;
                this.Msg = "Survey Successfully removed";
                this.successSnackbarMsg = false;
            },

            /// <summary>
            ///  fnEditSurveyDetails :- Function used to edit survey details
            /// </summary>
            /// <param name="surveyId"></param>
            /// <returns> </returns>
            async fnEditSurveyDetails(surveyId) {
                if (surveyId > 0) {
                    this.CheckSureveyTaskIsAprovedWhileUpdate(surveyId);
                    this.SurveyId = surveyId;
                    this.creatEditTitle = "Edit Survey";
                    this.creatEditDescription =
                        "Please enter the basic details to edit a survey";
                    await this.GetProjectBeneficiaryType(1);
                    this.CreateSurveyDialog = true;
                    var result = await objManageProjectSurvey.GetProjectSurveyBySurveyId(
                        surveyId
                    );
                    if (result != null && result.data != null) {
                        this.SurveyName = result.data.SurveyName;
                        this.SurveyDescription = result.data.SurveyDescription;

                        //if (result.data.StartDate != null)
                        //    this.startDate = this.formatDate(result.data.StartDate.toString("dd-MM-yyyy").substr(0, 10));
                        //if (result.data.EndDate != null)
                        //    this.endDate = this.formatDate(result.data.EndDate.toString("dd-MM-yyyy").substr(0, 10));

                        if (result.data.StartDate != null)
                            this.startDate = objUtils.common.formatDate(result.data.StartDate);
                        if (result.data.EndDate != null)
                            this.endDate = objUtils.common.formatDate(result.data.EndDate);

                        this.WorkflowID = result.data.WorkflowID;
                        this.ReportingFrequency = result.data.ReportingFrequency;

                        this.ReportingMonth = result.data.ReportingMonth;
                        this.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.ReportingWeek = result.data.ReportingWeek;

                        if (
                            result.data.ReportingOnceDate != null &&
                            result.data.ReportingOnceDate != ""
                        ) {
                            this.FrmDate = objUtils.common.formatDate(result.data.ReportingOnceDate);
                        }

                        if (result.data.BeneficiaryTypeID > 0) {
                            this.BeneficiaryLinked = true;
                            this.BeneficiaryTypeID = result.data.BeneficiaryTypeID;
                        }

                        if (
                            result.data.AssignedUsers != null &&
                            result.data.AssignedUsers != undefined &&
                            result.data.AssignedUsers != ""
                        ) {
                            this.selectedteamUsers = JSON.parse(result.data.AssignedUsers);
                        }
                        if (
                            result.data.AssignedTags != null &&
                            result.data.AssignedTags != undefined &&
                            result.data.AssignedTags != ""
                        ) {
                            this.SelectedTag = JSON.parse(result.data.AssignedTags);
                        }
                    }
                }
            },

            /// <summary>
            ///  OpenCreateSurveyDialog :- Function used to open createSurvey dialog box
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async OpenCreateSurveyDialog() {
                await this.ClearBindData();
                await this.GetProjectBeneficiaryType(1);
                this.creatEditTitle = "Create Survey";
                this.creatEditDescription =
                    "Please enter the basic details to create a survey";
                this.CreateSurveyDialog = true;
                this.SelectedTag = [];
            },

            /// <summary>
            ///  ClearBindData :- Function used to clear bind data
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ClearBindData() {
                this.SurveyId = 0;
                this.SurveyName = "";
                this.SurveyDescription = "";
                (this.startDate = null), (this.endDate = null), (this.WorkflowID = 0);
                this.ReportingFrequency = 0;
                this.BeneficiaryTypeID = 0;
                this.selectedteamUsers = [];
                this.selectedteamUsers1 = [];
                this.BeneficiaryLinked = false;
                this.SelectedBeneficiaryTypeList = [];
                this.SelectedTag = [];
                this.ReportingMonth = 0;
                this.ReportingMonthDateDay = 0;
                this.ReportingMonthDay = 0;
                this.ReportingWeek = 0;
                this.EnableWorkflow = true;
            },

            /// <summary>
            ///  OpenAssignUserDeatilsToSurveyDialog :- Function used to open assignUserDetails dialog from action dropdown menu
            /// </summary>
            /// <param name="surveyId"></param>
            /// <returns> </returns>
            async OpenAssignUserDeatilsToSurveyDialog(surveyId) {
                this.AssignUserDialog = true;
                this.SurveyId = surveyId;
                this.selectedteamUsers = [];
                var result = await objManageProjectSurvey.GetSurveyAssignedUsers(
                    surveyId
                );

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.selectedteamUsers = JSON.parse(result.data);
                }
            },

            /// <summary>
            ///  AssignUserDeatilsToSurvey :- Function used to assign user details to survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AssignUserDeatilsToSurvey() {
                var data = new FormData();
                data.append("SurveyID", this.SurveyId);
                data.append("SurveyName", this.SurveyName);
                data.append("SurveyDescription", this.SurveyDescription);
                data.append("ProjectId", this.ProjectId);
                data.append("WorkflowID", this.WorkflowID);
                data.append("ReportingFrequency", this.ReportingFrequency);
                data.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                data.append("AssignedUsers", JSON.stringify(this.selectedteamUsers));

                var result = await objManageProjectSurvey.AssignUserstoSurvey(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.AssignUserDialog = false;
                        this.Msg = "User(s) successfully assigned to Survey";
                        this.successSnackbarMsg = true;
                        this.selectedteamUsers = [];
                        this.GetProjectSurveyList();
                    } else {
                        this.Msg = "Failed";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnGetWorkflowsListByProjectID :- Function used to fetch workflow list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnGetWorkflowsListByProjectID(item) {
                this.CheckSureveyTaskIsAprovedWhileUpdate(item.SurveyID);
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objManageProjectSurvey.GetWorkflowsListByProjectID(
                    data
                );
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectWorkflowDlgItems = result.data;
                    this.filterWorkflowsList = result.data;
                    var ID;
                    var temp = "";

                    for (ID = 0; ID <= this.SelectWorkflowDlgItems.length - 1; ID++) {
                        if (this.SelectWorkflowDlgItems[ID].WorkflowID == item.WorkflowID) {
                            temp = this.SelectWorkflowDlgItems.filter(
                                (x) => x.WorkflowID == item.WorkflowID
                            );
                        }
                        if (temp.length > 0) {
                            this.selectedAddWorkflow.push({
                                checkboxindex: ID,
                                WFitem: temp[0],
                            });
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
                    this.confirmError = false;
                } else {
                    if (this.selectedAddWorkflow.length > 0) {
                        this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
                            (x) => x.WFitem.WorkflowID != item.WorkflowID
                        );
                    }
                }
            },

            /// <summary>
            ///  fnRemove :- Function used to remove selected workflow from add workflow page
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnRemove(item) {
                this.checkboxAddWorkflow[item.checkboxindex] = false;
                this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
                    (x) => x.WFitem.WorkflowID != item.WFitem.WorkflowID
                );
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
            ///  fnAddSelectWorkflow :- Function used to add selected workflow to survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddSelectWorkflow() {
                var data = new FormData();
                data.append("WorkFlowID", this.selectedAddWorkflow[0].WFitem.WorkflowID);
                data.append("ProjectId", this.ProjectId);
                data.append("SurveyID", this.SurveyId);

                var result = await objManageProjectSurvey.UpdateSurveyWorkflow(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = "Workflow successfully assigned to Survey";  //alert("Successfully added the workflow you selected");
                    this.successSnackbarMsg = true;
                    this.SelectWorkflowDialog = false;
                    this.SelectWorkflowDlgSteps = 1;
                    this.SelectWorkflowDlgItems = [];
                    this.checkboxAddWorkflow = [];
                    this.selectedAddWorkflow = [];
                    this.GetProjectSurveyList();
                    await this.ClearBindData();
                }
            },

            /// <summary>
            ///  searchStringInArray :- Function used to search an array which contains any invalid input string
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false </returns>
            searchStringInArray() {
                var newList = this.lstImportSurvey.filter(element => {
                    for (var property in element) {
                        if (element.hasOwnProperty(property)) {
                            if (element[property] == "Invalid Input") {
                                return true;
                            }
                        }
                    }

                });
                if (newList.length > 0) {
                    return false;
                }
                else {
                    return true;
                }
            },

            /// <summary>
            ///  ImportSurveyList :- Function used to import survey list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ImportSurveyList() {
                if (this.searchStringInArray() == true) {
                    this.PreLoader = true;
                    var lstImportList = [];

                    for (var i = 0; i < this.lstImportSurvey.length; i++) {

                        lstImportList.push({
                            SurveyName: this.lstImportSurvey[i].SurveyName,
                            SurveyDescription: this.lstImportSurvey[i].SurveyDescription,
                            WorkFlowName: this.lstImportSurvey[i].WorkFlowName,
                            StartDate: objUtils.common.formatExcelDate(this.lstImportSurvey[i].StartDate),
                            EndDate: objUtils.common.formatExcelDate(this.lstImportSurvey[i].EndDate),
                            //ReportingFrequencyName: this.lstImportSurvey[i].ReportingFrequencyName,
                            BeneficiaryTypeName: this.lstImportSurvey[i].BeneficiaryTypeName
                        });
                    }

                    var data = new FormData();
                    data.append("ImportSurveyList", JSON.stringify(lstImportList));
                    data.append("ProjectId", this.ProjectId);

                    this.ImportInviteUserSteps = 3;

                    var result = await objManageProjectSurvey.ImportProjectSurvey(data);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else {
                        if (result.status == "success") {
                            this.CancelImport();
                            this.GetProjectSurveyList();
                            this.Msg = "Survey Successfully imported";
                            this.successSnackbarMsg = true;
                            this.ImportSurveyDialogBox = false;
                            this.PreLoader = false;
                            this.Msg = "Successfully Imported";
                            this.successSnackbarMsg = true;
                        } else {
                            this.PreLoader = false;
                            this.Msg = "Wrong file / data ";
                            this.errorSnackbarMsg = true;
                        }
                        this.csvfile.name = "";
                        this.csvfile.size = "";
                        this.csvfile.namedisplay = "";
                        this.fileInput = null;
                    }
                }
                else {
                    this.Msg = "Please resolve all Invalid Input(s).";
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnBulkRemoveClick :- Function used to remove selected surveys in bulk actions
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkRemoveClick() {
                if (this.SelectedSurvey.length > 0) {
                    this.RemoveBulkSurveyDialog = true;
                } else {
                    this.Msg = "Please select the Survey";
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  downloadSampleFile :- Function used to download a sample file while survey import
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            downloadSampleFile() {
                var csv =
                    "SurveyName,SurveyDescription,WorkFlowName,StartDate,EndDate,BeneficiaryTypeName\n";
                this.data.forEach(function (row) {
                    csv += row.join(",");
                    csv += "\n";
                });

                // console.log(Math.round(Math.random() * 1000));
                var hiddenElement = document.createElement("a");
                hiddenElement.href = "data:text/csv;charset=utf-8," + encodeURI(csv);
                hiddenElement.target = "_blank";
                hiddenElement.download =
                    "sample_project_survey_" + Math.round(Math.random() * 1000) + ".csv";
                hiddenElement.click();
            },

            /// <summary>
            ///  OpenLinkBeneficiaryPopup :- Function used to open popup of link beneficiary type
            /// </summary>
            /// <param name="surveyId"></param>
            /// <returns> </returns>
            async OpenLinkBeneficiaryPopup(surveyId) {
                this.SurveyId = surveyId;
                await this.GetProjectBeneficiaryType(2);
                this.BeneficiaryTypeLinkDialog = true;
            },

            /// <summary>
            ///  fnAddSelectedBeneficiryType :- Function used to add selected beneficiary type to survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddSelectedBeneficiryType() {
                var data = new FormData();
                data.append("BenficiaryTypeId", this.SelectedBeneficiaryTypeList[0].ID);
                data.append("SurveyID", this.SurveyId);

                var result = await objManageProjectSurvey.UpdateSurveyBeneficiaryType(
                    data
                );
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = "Successfully re-linked Beneficiary Type";//alert("Successfully added the beneficiary type you selected");
                    this.BeneficiaryTypeLinkDialog = false;
                    this.successSnackbarMsg = true;
                    this.BeneficiaryTypeLinkDialog = false;
                    this.SelectedBeneficiaryTypeList = [];
                    this.GetProjectSurveyList();
                    await this.ClearBindData();
                }
            },

            /// <summary>
            ///  CancelBeneficiaryTypeLinkDialog :- Function used to close beneficiary type link dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CancelBeneficiaryTypeLinkDialog() {
                this.BeneficiaryTypeLinkDialog = false;
                this.SurveyId = 0;
                this.SelectedBeneficiaryTypeList = [];
            },

            /// <summary>
            ///  fnGetAllTags :- Function used to fetch all project tag list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objManageProjectSurvey.GetAllProjectTags(
                    this.InitiativeId
                );
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
            ///  fnEditTagDetails :- Function used to edit tag details
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
            ///  UpdateTagDetails :- Function used to update tag details
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();

                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objManageProjectSurvey.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.TagHeaderName = "Add Tag";
                    this.AllTagListSteps = 2;
                    this.fnGetAllTags();
                    this.UpdatingTagItem = "";
                }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create new tag
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color.hex);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objManageProjectSurvey.CreateNewTag(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.TagHeaderName = "Add Tag";
                    this.AllTagListSteps = 2;
                    this.fnGetAllTags();
                }
            },

            /// <summary>
            ///  fnAddToSelectedTag :- Function used to add a tag to selected tag items
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToSelectedTag(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.SelectedTag.length > 0) {
                        var temp = this.SelectedTag.filter((x) => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.SelectedTag.push({
                                TagID: TagItem.TagID,
                                TagName: TagItem.TagName,
                                TagColor: TagItem.TagColor,
                            });
                        }
                    } else {
                        this.SelectedTag.push({
                            TagID: TagItem.TagID,
                            TagName: TagItem.TagName,
                            TagColor: TagItem.TagColor,
                        });
                    }

                    this.NewTagMenu = false;
                }
            },

            /// <summary>
            ///  fnRemoveFromSelectedTagItem :- Function used to remove a tag from selected tag items
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnRemoveFromSelectedTagItem(TagItem) {
                this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
                this.SelectedTag = [...this.SelectedTag];
                if (this.SelectedTag.length == 0) {
                    this.allTags = false;
                }
            },

            // ======== Duplicate Survey ======//

            /// <summary>
            ///  fnGetSurveyDetails :- Function used to fetch survey details for the survey duplication
            /// </summary>
            /// <param name="surveyId"></param>
            /// <returns> </returns>
            async fnGetSurveyDetails(surveyId) {
                if (surveyId > 0) {
                    this.SurveyId = surveyId;
                    await this.GetProjectBeneficiaryType(1);
                    var result = await objManageProjectSurvey.GetProjectSurveyBySurveyId(
                        surveyId
                    );
                    if (result != null && result.data != null) {
                        this.SurveyName = result.data.SurveyName + "_duplicate";
                        this.SurveyDescription = result.data.SurveyDescription;
                        this.WorkflowID = result.data.WorkflowID;
                        this.ReportingFrequency = result.data.ReportingFrequency;
                        this.ReportingFrequencyName = result.data.ReportingFrequencyName;

                        this.ReportingMonth = result.data.ReportingMonth;
                        this.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.ReportingWeek = result.data.ReportingWeek;
                        this.TotalWFLevel = result.data.TotalWFLevel;

                        if (
                            result.data.ReportingOnceDate != null &&
                            result.data.ReportingOnceDate != ""
                        ) {
                            this.FrmDate = objUtils.common.formatDate(result.data.ReportingOnceDate);
                        }

                        if (result.data.BeneficiaryTypeID > 0) {
                            this.BeneficiaryLinked = true;
                            this.BeneficiaryTypeID = result.data.BeneficiaryTypeID;
                        }

                        if (
                            result.data.AssignedUsers != null &&
                            result.data.AssignedUsers != undefined &&
                            result.data.AssignedUsers != ""
                        ) {
                            this.selectedteamUsers = JSON.parse(result.data.AssignedUsers);
                        }
                        if (
                            result.data.AssignedTags != null &&
                            result.data.AssignedTags != undefined &&
                            result.data.AssignedTags != ""
                        ) {
                            this.SelectedTag = JSON.parse(result.data.AssignedTags);
                        }
                    }
                }
            },

            /// <summary>
            ///  RemoveUser :- Function used to remove selected team users from the user table list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            RemoveUser(item) {
                this.selectedteamUsers.splice(this.selectedteamUsers.indexOf(item), 1);
                this.selectedteamUsers = [...this.selectedteamUsers];
            },


            /// <summary>
            ///  fnCreateDuplicateSurvey :- Function used to create duplicate survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateDuplicateSurvey() {

                this.PreLoader = true;
                if (this.BeneficiaryLinked == false) {
                    this.BeneficiaryTypeID = 0;
                }
                var data = new FormData();
                data.append("SurveyID", 0);
                data.append("SurveyName", this.SurveyName);
                data.append("SurveyDescription", this.SurveyDescription);
                data.append("ProjectId", this.ProjectId);
                data.append("ReportingFrequency", this.ReportingFrequency);
                data.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                data.append("AssignedTags", JSON.stringify(this.SelectedTag));
                data.append("ReportingMonth", this.ReportingMonth);
                data.append("ReportingMonthDay", this.ReportingMonthDay);
                data.append("ReportingWeek", this.ReportingWeek);
                data.append("ReportingOnceDate", this.FormattedDate);
                data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);

                if (this.vmChkSameWF) data.append("WorkflowID", this.WorkflowID);
                if (this.vmChkSameUsers)
                    data.append("AssignedUsers", JSON.stringify(this.selectedteamUsers));
                data.append("duplicateSecAndQstns", this.vmChkSameSecQstn);
                data.append("OldSurveyID", this.SurveyId);
                var result = await objManageProjectSurvey.SaveDuplicateProjectSurvey(
                    data
                );

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.CreateDuplicateSurveyDialog = false;
                        this.Msg = "Survey Successfully Duplicated";
                        this.successSnackbarMsg = true;
                        this.selectedteamUsers = [];
                        await this.ClearBindData();
                        this.GetProjectSurveyList();
                        this.PreLoader = false;
                    } else {
                        if (result.status == "nameexists") {
                            this.PreLoader = false;
                            this.Msg = "Survey Name Exists";
                            this.infoSnackbarMsg = true;
                        } else if (result.status == "rptfrequencyfailed") {
                            this.PreLoader = false;
                            this.Msg = "Reporting Frequency Failed";
                            this.errorSnackbarMsg = true;
                        } else {
                            this.PreLoader = false;
                            this.Msg = "Failed";
                            this.errorSnackbarMsg = true;
                        }
                    }
                }
            },

            /// <summary>
            ///  CheckSureveyTaskIsAprovedWhileUpdate :- Function used to check survey task is approved or not while update
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async CheckSureveyTaskIsAprovedWhileUpdate(SurveyID) {
                this.EnableWorkflow = true;
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("TypeID", SurveyID);
                data.append("Type", "Survey")
                var result = await objManageProjectSurvey.CheckSureveyTaskIsAprovedWhileUpdate(data);
                if (result.status == "Failed") {
                    this.EnableWorkflow = false;
                }
            },

            /// <summary>
            ///  RemoveBeneficiary :- Function used to remove selected beneficiary type from the list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            RemoveBeneficiary(item) {
                this.SelectedBeneficiaryTypeList.splice(
                    this.SelectedBeneficiaryTypeList.indexOf(item),
                    1
                );
                this.SelectedBeneficiaryTypeList = [...this.SelectedBeneficiaryTypeList];
            },

            /// <summary>
            ///  GetSurveyExcelOptions :- Function used to fetch survey excel options
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetSurveyExcelOptions() {
                for (var i = 0; i < this.lstWorkFlow.length; i++) {
                    this.lstSurveyHearder[2].options.push(this.lstWorkFlow[i].Value);
                }
                //for (var i = 0; i < this.lstPeriodicity.length; i++) {
                //    this.lstSurveyHearder[5].options.push(this.lstPeriodicity[i].Value);
                //}
                if (this.lstBeneficiaryType.length > 0) {
                    for (var i = 0; i < this.lstBeneficiaryType.length; i++) {
                        this.lstSurveyHearder[5].options.push(this.lstBeneficiaryType[i].Name);
                    }
                }
            },


            ImportTemplate(SurveyID) {
              
                const link = document.createElement('a');
                const surveyId = SurveyID;
                link.href = window.SERVER_URL + "/SurveyResponseImport/GetQuestionDetails?param1=" + SurveyID;
                
                    link.setAttribute('download', 'sample_surveyQuestionnaire.xlsm');
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
          
            },

            async GetBenefSurveyQstnsForImport() {
  
                var lstSurveyQstns = [];

                this.PreLoader = true;
                var result = await objManageProjectSurvey.GetAllSectionsBySurveyID(this.SurveyID);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                   
                        this.lstSurveySections = result.data;



                        if (lstSurveyQstns.length == 0) {
                            this.SnackbarMsg = "Survey Sections and Questionnaire are not available...";
                            this.InfoSnackbarMsg = true;
                        }
                        else {

                            for (var j = 0; j < this.lstSurveySections[i].SurveySecQuestions.length; j++) {
                                lstSurveyQstns = this.lstSurveySections[i].SurveySecQuestions
                                this.lstImportSurveyQstnHearder = [];
                                for (var i = 0; i < lstSurveyQstns.length; i++) {

                                    if (lstSurveyQstns[i].QuestionType == "Date Picker") {
                                        this.lstImportSurveyQstnHearder.push({
                                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            type: "date",
                                            //mandatory:this.lstSurveyQstns[i].IsMandatory,
                                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,
                                        });
                                    }
                                    else if (lstSurveyQstns[i].QuestionType == "Numeric") {
                                        this.lstImportSurveyQstnHearder.push({
                                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            type: "number",
                                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,
                                        });
                                    }
                                    else if (lstSurveyQstns[i].QuestionType == 'Single Choice' || lstSurveyQstns[i].QuestionType == 'Multi-Choice' || lstSurveyQstns[i].QuestionType == 'Dropdown' || lstSurveyQstns[i].QuestionType == 'Likert Scale') {
                                        var optionlist = [];
                                        for (var j = 0; j < lstSurveyQstns[i].QuestionOptions.length; j++) {
                                            optionlist.push(lstSurveyQstns[i].QuestionOptions[j].OptionValue)
                                        }
                                        this.lstImportSurveyQstnHearder.push({
                                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                            type: "select",
                                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                            options: optionlist,
                                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,

                                        });
                                    }
                                    else {
                                        if (lstSurveyQstns[i].QuestionType != 'Note') {
                                            this.lstImportSurveyQstnHearder.push({
                                                text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                                value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                                type: "string",
                                                mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                                QuestionCondition: lstSurveyQstns[i].QuestionCondition,

                                            });

                                        }
                                    }
                                }
                            }
                        }
                    }
                this.BenefLinkedSurveySteps = 7;
                this.NonBenefLinkedSurveySteps = 2;
            },
            // Function Triggers when drag and drop happens for Survey Import
            async dragFile_Survey(e) {
                const file = e.dataTransfer.files[0];
                this.selectedFile = file;
              
                await this.uploadFile();
             
            },
          
            //survey data import
            handleFileImport() {
                this.isSelecting = true;

                // After obtaining the focus when closing the FilePicker, return the button state to normal
                window.addEventListener('focus', () => {
                    this.isSelecting = false
                }, { once: true });

                // Trigger click on the FileInput
                this.$refs.uploader.click();
            },
           
            async uploadFile() {
                this.PreLoader = true;
                var loggedUser = window.LoggedUserID;
                const formData = new FormData();
                formData.append("file", this.selectedFile);
                formData.append("SurveyId", this.ImportSurveyId);
                formData.append("ReportingFrequency", this.ReportingFrequency);
                formData.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                formData.append("BeneficiaryLinked", this.BeneficiaryLinked)
                formData.append("TotalWFLevel", this.TotalWFLevel);
                formData.append("WorkflowID", this.WorkflowID);
                formData.append("WorkspaceID", this.WorkspaceID);
                formData.append("ReportingFrequencyName", this.ReportingFrequencyName);
                formData.append("LoggedUserId", loggedUser);
                fetch(window.SERVER_URL + "/SurveyResponseImport/Import", {
                    method: "POST",
                    body: formData
                })
                    .then(response => {
                        if (response.ok) {
                            this.PreLoader = false;
                            this.ImportTemplateDialog = false;
                            this.Msg = "Survey response Successfully Imported";
                            this.successSnackbarMsg = true;
                            this.GetProjectSurveyList();
                        } else {
                            this.PreLoader = false;
                            this.Msg = "Survey response Import failed";
                            this.errorSnackbarMsg = true;
                        }
                    })
                   
            }
          
        },
       

    };

 
</script>

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

    .dialogHeight {
        height: 70vh;
    }

    .uploadBoxHeight {
        height: 39vh;
    }

    .uploadScrollHeight {
        height: 35vh;
    }

    @media screen and (max-width: 750px) {

        .dialogHeight {
            height: 85vh;
        }


        .uploadBoxHeight {
            height: 54vh;
        }

        .uploadScrollHeight {
            height: 48vh;
        }
    }
</style>
