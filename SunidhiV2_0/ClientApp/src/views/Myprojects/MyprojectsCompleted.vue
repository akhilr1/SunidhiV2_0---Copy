<template>
    <v-card>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid style="min-height:70vh">
            <v-row>
                <v-col cols="4">
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary">mdi-file-document-box-check</v-icon>
                        </v-avatar>
                        Completed Project
                    </h3>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto">
                    <v-row>
                        <v-spacer></v-spacer>
                        <!--Bulk Action -->
                        <v-menu offset-y="" transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                                    Bulk Actions
                                    <v-icon>
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">

                                <v-list-item id="AutoTest_MP27" @click.stop="fnBulkRemoveClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>

                            <v-dialog v-model="RemoveBulkProjectDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title="" class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Project
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_MP28" text="" @click="RemoveBulkProjectDailog=false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text class="pt-0">
                                        <v-container fluid="" class="pt-4">
                                            <v-row no-gutters="">
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid="" class="pt-0">
                                                        <v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <h4 class="text-center heading-3">Are you sure you want to remove this projects ?</h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <Secondary-Button id="AutoTest_MP29" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(1)"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_MP30" class="btn-122x36" title="Cancel" @click.native="RemoveBulkProjectDailog=false"></Default-Button-Outlined>
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
                        <!--Search-->
                        <v-text-field label="Search" class="mr-4 table-search"
                                      dense="" prepend-inner-icon="mdi-magnify" v-model="searchProject"
                                      outlined hide-details></v-text-field>
                        <!--Filter-->
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                            <template v-slot:activator="{ on }">
                                <v-btn id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent">
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
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            {{dateRangeHeaderName}}
                            <v-icon>
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="900" min-width="50" class="action-button-list pa-2">
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

                <!-- initiative-->
                <!--<v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="initiativeMenu">
            <template v-slot:activator="{ on }">
                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                    Initiative
                    <v-icon class="ml-12">
                        mdi-chevron-down
                    </v-icon>
                </v-btn>
            </template>
            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                <v-checkbox color="success-check-box" input-value="true" label="Digital Marketing Platform"></v-checkbox>
                <v-checkbox color="success-check-box" input-value="true" label="Event Planning Checklist"></v-checkbox>

                <div class="d-inline-flex">
                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="initiativeMenu = false">
                        Cancel
                    </v-btn>
                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="initiativeMenu = false">
                        Done
                    </v-btn>
                </div>
            </v-list>
        </v-menu>-->
                <!--status-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="statusMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                            Status
                            <v-icon class="ml-12">
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                        <v-checkbox color="success-check-box" v-model="checkactive" input-value="true" label="Active"></v-checkbox>
                        <v-checkbox color="success-check-box" v-model="checkinactive" input-value="true" label="Inactive"></v-checkbox>

                        <div class="d-inline-flex">
                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="statusMenu = false">
                                Cancel
                            </v-btn>
                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnClickFilterDone(),statusMenu = false">
                                Done
                            </v-btn>
                        </div>
                    </v-list>
                </v-menu>
            </v-row>
            <v-row v-if="filterChipsList.length>0" class="pa-0">
                <v-col cols="12" xs="12" sm="12" md="12" class="pa-0">
                    <div class="chip-container ma-0">
                        <v-chip-group multiple column active-class="primary--text">
                            <v-chip id="" close v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="mx-2" @click:close="fnChipClose(item)" memdium label>
                                {{ item.ChipName }}
                            </v-chip>
                        </v-chip-group>
                    </div>
                </v-col>
            </v-row>
            <!--Main Body-->
            <v-row v-if="FilterProjectTableItems.length>0">
                <!--<v-col class="pa-0 ma-0 text-right pr-3">
                    <v-tooltip bottom>
                        <template v-slot:activator="{ on, attrs }">
                            <span v-bind="attrs" v-on="on">
                                last updated: {{timeAgo}} ago
                            </span>
                        </template>
                        <span>Updation date: {{ProjectTableItems[0].ExecutedDateTime}}</span>
                    </v-tooltip>
                </v-col>-->
                <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
                    <v-data-table :headers="ProjectTableheaders"
                                  :items="FilterProjectTableItems"
                                  v-model="selectedProjectID"
                                  show-select=""
                                  item-key="ProjectId"
                                  :search="searchProject"
                                  fixed-header
                                  height="60vh"
                                  class="elevation-0"
                                  :footer-props="{
                                  itemsPerPageOptions:[10,50,100,-1]
                                  }"
                                  :items-per-page="10">
                        <template v-slot:item.ProjectName="{ item }">
                            <!--<router-link class="text--default text-link" link="" :to="{ name: 'MyprojectsCompletedIndividual', query: { WsID: item.WorkspaceID,Access:RWAccess, InitID: item.InitiativeID, ProjID: item.ProjectId,AccessType:'MyProject' } }">
                      {{ item.ProjectName }}
                    </router-link>-->
                            <div class="pa-0 text-limit" style="width:130px">
                                <v-avatar size="25" color="#fff" class="elevation-1">
                                    <v-icon size="15" color="primary">mdi-folder</v-icon>
                                </v-avatar>
                                {{ item.ProjectName }}
                            </div>
                        </template>
                        <!-- column State-->
                        <!--<template v-slot:item.State="{ item }">
                            <div class="pa-0 text-limit" style="width:130px">
                                <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" v-bind:disabled="!screenAccess"
                                          @change="GetProjectID(item.ProjectId,item.ProjectName,item.IsActive),IsActiveToggleDialog = true,ProjectName=item.ProjectName">
                                </v-switch>
                            </div>
                        </template>-->
                        <!-- DateTime-->
                        <template v-slot:item.StartDate="{ item }">
                            <div class="pa-0 text-limit" style="width:90px">{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</div>
                        </template>
                        <template v-slot:item.EndDate="{ item }">
                            <div class="pa-0 text-limit" style="width:90px">{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</div>
                        </template>
                        <template v-slot:item.Progress="{ item }">
                            <div class="pa-0 text-limit" style="width:80px">
                                <v-progress-circular :rotate="360"
                                                     :size="34"
                                                     :width="5"
                                                     :value=item.Progress
                                                     color="secondary">
                                    <span class="text--secondary"><small>{{ item.Progress }}</small></span>
                                </v-progress-circular>
                                {{ item.Progress }}%
                            </div>
                        </template>
                        <template v-slot:item.Budget="{ item }">
                            <div class="pa-0 text-limit" style="width:100px">
                                {{item.Budget }}
                            </div>
                        </template>
                        <template v-slot:item.FundUtilization="{ item }">
                            <div v-if="item.FundUtilization " class="pa-0 text-limit" style="width:120px">
                                {{item.FundUtilization }}
                            </div>
                            <div style="width:120px" v-else>N/A</div>
                        </template>
                        <!-- Draft project Actions-->
                        <template v-slot:item.Actions="{item}">
                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">

                                    <v-list-item id="AutoTest_MP31" link @click.stop="GetProjectID(item.ProjectId,item.ProjectName),ArchiveDailog=true,ProjectName=item.ProjectName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-file-document-box-minus-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Archive
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_MP32" link @click.stop="GetProjectID(item.ProjectId,item.ProjectName),GetProjectTemplateByprjtID(),GetProjectTheme()">
                                        <v-list-item-icon>
                                            <v-icon>mdi-file-star-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Set as template
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_MP33" link="" @click.stop="GetProjectID(item.ProjectId,item.ProjectName),RemoveProjectDailog=true,ProjectName=item.ProjectName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>

                                <!-- Archive project -->
                                <v-dialog v-model="ArchiveDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-file-document-box-minus-outline</v-icon>
                                            </v-avatar>
                                            Archive Project
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_MP34" text="" @click="ArchiveDailog=false">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to archive this project ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-avatar class="mr-2 elevation-1" size="40" color="#fff">
                                                                            <v-icon color="#707070" size="25">mdi-file-document-box-check</v-icon>
                                                                        </v-avatar>
                                                                        {{ProjectName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <Secondary-Button id="AutoTest_MP35" class="btn-122x36 mr-4" title="Archive" @click.native="ChangeProjectStatus('Archived')"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_MP36" class="btn-122x36" title="Cancel" @click.native="ArchiveDailog=false"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>

                                <v-dialog v-model="RemoveProjectDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove Project
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_MP37" text="" @click="RemoveProjectDailog=false">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to remove this project ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-avatar class="mr-2 elevation-1" size="40" color="#fff">
                                                                            <v-icon color="#707070" size="25">mdi-file-document-box-check</v-icon>
                                                                        </v-avatar>
                                                                        {{ProjectName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <Secondary-Button id="AutoTest_MP38" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(2)"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_MP39" class="btn-122x36" title="Cancel" @click.native="RemoveProjectDailog=false"></Default-Button-Outlined>
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
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
            <!-- else part if No project available-->
            <v-row v-else>
                <v-col cols="12" sm="12" md="12">
                    <v-row :justify="justifyCenter">
                        <v-col md="12" class="text-center">
                            <p class="mt-12 pt-12">
                                <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                            </p>

                            <p class="heading-3-dark font-weight-medium pt-6">No project data available</p>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>

            <v-dialog v-model="IsActiveToggleDialog" width="800">
                <v-card class="elevation-0">
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-electric-switch</v-icon>
                        </v-avatar>
                        Change State
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_MP41" text @click="IsActiveToggleDialog=false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="py-0">
                            <v-row>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">Are you sure want to change the state of this project ?</h4>
                                            </v-col>
                                            <v-col>
                                                <h4 class="text-center heading-3">
                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                    {{ProjectName}}
                                                </h4>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_MP42" color="secondary elevation-0" class="mr-2" @click="ChangeState()">Change</v-btn>
                                                <v-btn id="AutoTest_MP43" outlined color="grey" @click="IsActiveToggleDialog=false, GetProjectListByInitID()">Cancel</v-btn>
                                            </v-col>

                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!-- Set as Template-->
            <!-- Set as Template-->
            <v-dialog v-model="dlgSetasTemplate" width="800" persistent>
                <v-card>
                    <v-card-title primary-title="" class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-file-star-outline</v-icon>
                        </v-avatar>
                        Save as Template
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_MP44" text="" @click="dlgSetasTemplate = false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text>
                        <v-stepper v-model="AddProjectTempSteps" class="elevation-0">
                            <v-stepper-items>
                                <v-stepper-content step="1" class="pa-0">
                                    <h4 class="text-center heading-3 mt-4">Please enter the basic details to save it as template.</h4>
                                    <v-row justify="center" class="mt-4">
                                        <v-col md="6" class="v-scrolling-div">
                                            <v-text-field outlined="" dense="" label="Template Name" v-model="Template">
                                            </v-text-field>
                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="TemplateDescription">
                                            </v-textarea>

                                            <v-file-input label="Upload Cover Image" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                                          v-model="SetasTempFileInput" id="file"></v-file-input>

                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <v-stepper-content step="2" class="pa-0">

                                    <v-row no-gutters>
                                        <div>
                                            <h3 class="text-left page-head">
                                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                </v-avatar> Theme
                                            </h3>
                                        </div>
                                        <v-col cols="12" md="12" class="pt-0 mb-2">
                                            <h4 class="text-center heading-3">Step 3:  Please select a theme to see tha available template under it.</h4>
                                        </v-col>
                                        <v-col md="12" class="py-0">

                                        </v-col>
                                        <v-row>
                                            <v-col xs="12" sm="12" md="12">
                                                <v-row style="overflow-y:scroll;height:50vh">
                                                    <v-card v-for="(item,idx) in ThemeItems" :key="idx" width="150px" class="ma-2" style="float:left">
                                                        <v-row no-gutters>
                                                            <!-- Notification-->
                                                            <v-col md="12" class="pb-0">
                                                                <v-avatar color="#52B962" size="25" style="margin-top:5px" class="ma-2" v-if="item.IsSelected">
                                                                    <span class="white--text">{{item.SelectedTemplCount}}</span>
                                                                </v-avatar>
                                                                <v-checkbox v-model="checkboxAddTheme[idx]" color="#52B962" style="float:right;margin-top:5px"
                                                                            @change="fnCheck(item,idx)" label=""></v-checkbox>
                                                            </v-col>
                                                            <!-- Button-->
                                                            <v-col md="12" class="text-center pb-0 pt-0">
                                                                <v-btn height="80" text>
                                                                    <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                                        <v-img :src="require('../../newtemplateassets/'+item.TemplateImage)" height="80" width="80"></v-img>
                                                                    </v-avatar>
                                                                </v-btn>
                                                            </v-col>
                                                            <!-- Text Name-->
                                                            <v-col md="12" style="overflow:hidden">
                                                                <p class="pt-1 px-1 text-limit text-center" style="width:145px">{{item.Theme}}</p>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card>
                                                </v-row>
                                            </v-col>
                                        </v-row>


                                    </v-row>
                                </v-stepper-content>
                            </v-stepper-items>
                        </v-stepper>


                    </v-card-text>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Outlined-Button-Dark id="AutoTest_MP45" class="btn-122x36 float-right" title="Cancel" @click.native="dlgSetasTemplate=false"></Outlined-Button-Dark>
                            <Primary-Button-Light id="AutoTest_MP46" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="dlgSetasTemplate=false,InsertProjectTemplate() " v-show="AddProjectTempSteps==2"></Primary-Button-Light>
                            <v-btn id="AutoTest_MP47" outlined color="grey" class="btn-122x36 float-right" @click="AddProjectTempSteps=1" v-show="AddProjectTempSteps==2">Previous</v-btn>
                            <Primary-Button-Light id="AutoTest_MP48" class="btn-122x36 float-right mr-4" title="Next" @click.native="AddProjectTempSteps=2" v-show="AddProjectTempSteps==1" v-bind:disabled="Template.length == 0 || Template.trim() == ''"></Primary-Button-Light>
                        </v-col>
                    </v-card-actions>
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
        </v-container>
    </v-card>
</template>
<script type="text/javascript">

    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'

    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/MyProject.js").then(mod => {
                objProject = new mod.MyProject();
            })
        ]);
    };

    export default {
        data() {
            return {
                PreLoader: false,
                selectedItem: "",
                items: [
                    { text: 'This month' },
                    { text: 'This quarter' },
                    { text: 'This Year' },
                ],
                //chipContent: [],
                filterContent: false,
                dateRangeHeaderName: 'Date Range',
                dateRangeMenu: false,
                //initiativeMenu: false,
                statusMenu: false,
                filterChipsList: [],
                checkactive: false,
                checkinactive: false,
                startDate: new Date().toISOString().substr(0, 10),
                endDate: new Date().toISOString().substr(0, 10),
                screenAccess: 0,               
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeID: 0,
                RWAccess: 0,

                ProjectTableheaders: [
                    { text: "Name", align: "left", value: "ProjectName" },
                    { text: "Code", value: "Code", align:" d-none" },
                    { text: "Start Date", value: "StartDate" },
                    { text: "End Date", value: "EndDate", },
                    { text: "Budget", value: "Budget", },
                    { text: "Progress", value: "Progress" },
                    { text: "Fund Utilization", value: "FundUtilization" },
                    //{ text: "State", value: "State", sortable: false },
                    { text: "Initiative", value: "Initiative", align:" d-none" },
                    { text: "Actions", value: "Actions", sortable: false,align:"center" }

                ],
                ProjectTableItems: [],
                FilterProjectTableItems: [],
                selectedProjectID: [],
                searchProject: "",
                ProjectId: "",
                ProjectName: "",
                State: "",
                RemoveProjectDailog: false,
                RemoveBulkProjectDailog: false,
                ArchiveBulkDailog: false,
                ArchiveDailog: false,
                IsActiveToggleDialog: false,
                ArchiveDailog: false,
                dlgSetasTemplate : false,
                SetasTempFileInput: null,
                AddProjectTempSteps: 1,
                checkboxAddTheme: [],
                selectedTheme: [],
                ThemeItems: [],
                Template: "",
                TemplateDescription: "",

                //Filtration
                FiterStartDate: "",
                FiterEndDate: "",
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.PreLoader = true;

           // this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await this.GetProjectListByInitID();
            this.PreLoader = false;
            await this.GetTheme();
        },
        computed: {
            timeAgo(){
                return objUtils.common.timeSince(this.ProjectTableItems[0].ExecutedDateTime)
            }
        },
        methods: {
            /// <summary>
            ///  IsValidDate :- Function used to check project start date and project end date is valid
            ///  startdate/ Enddate  not < todays date
            ///  enddate  any valid range - Max value
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = ("Invalid Date : End date should be greater than Start Date");
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
            },

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter projects based on date range and status option
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {
                this.filterChipsList = [];

                // status menu filtration
                if ((this.checkactive && this.checkinactive) || (!this.checkactive && !this.checkinactive)) {
                    this.FilterProjectTableItems = this.ProjectTableItems;
                }
                else if (this.checkactive) {
                    this.FilterProjectTableItems = this.ProjectTableItems.filter(x => x.IsActive);
                }
                else if (this.checkinactive) {
                    this.FilterProjectTableItems = this.ProjectTableItems.filter(x => !x.IsActive);
                }
                if (this.checkactive) {
                    this.filterChipsList.push({ ChipID: 999, ChipName: "Active" });
                }
                if (this.checkinactive) {
                    this.filterChipsList.push({ ChipID: 888, ChipName: "Inactive" });
                }
                // status menu filtration ends here

                //date range filter
                if (this.FiterStartDate != '' && this.FiterEndDate != '') {

                    this.filterChipsList.push({ ChipID: 777, ChipName: objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate) });
                    this.dateRangeHeaderName = objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate);

                    this.FilterProjectTableItems = this.FilterProjectTableItems.filter(x => x.StartDate.substr(0, 10) >= this.FiterStartDate && x.EndDate.substr(0, 10) <= this.FiterEndDate);

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

                if (chipitem.ChipName == "Active")
                    this.checkactive = false;
                if (chipitem.ChipName == "Inactive")
                    this.checkinactive = false;
                if (chipitem.ChipName == objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate)) {
                    this.FiterStartDate = "";
                    this.FiterEndDate = "";
                    this.dateRangeHeaderName = "Date Range";
                }
                this.fnClickFilterDone();
            },

            /// <summary>
            ///  GetProjectListByInitID :- Function used to fetch project details based on InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectListByInitID() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("stage", 'Completed');
                //data.append("InitiativeID", this.InitiativeID);
                var result = await objProject.GetProjectListByInitID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                if (result.data) {
                    this.ProjectTableItems = result.data;
                    this.FilterProjectTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnBulkRemoveClick :- Function used to open remove dialog box, print a message if project is not selected to remove
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkRemoveClick() {
                if (this.selectedProjectID.length > 0) {
                    this.RemoveBulkProjectDailog = true;
                } else {
                    this.Msg = ("Please select the projects");
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  GetProjectID :- Function used to fetch ProjectID
            /// </summary>
            /// <param name="ProjectID"></param>
            /// <param name="ProjectName"></param>
            /// <param name="State"></param>
            /// <returns> </returns>
            async GetProjectID(ProjectID, ProjectName, State) {
                this.ProjectID = ProjectID;
                this.ProjectName = ProjectName;
                this.State = State;
            },

            /// <summary>
            ///  DeleteProject :- Function used to delete project details based on ProjectID
            ///  Type 1 is bulk and Type 2 single removal
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            async DeleteProject(Type) {

                var data = new FormData();
                if (Type == 1) {
                    data.append("ProjectID", JSON.stringify(this.selectedProjectID));
                    data.append("Type", 1);
                }
                else {
                    data.append("ProjectID", JSON.stringify(this.ProjectID));
                    data.append("Type", 2);
                }

                var result = await objProject.DeleteProject(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg = ("Successfully deleted");
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = ("Failed to delete");
                        this.errorSnackbarMsg = true;
                    }
                }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.RemoveProjectDailog = false;
                this.RemoveBulkProjectDailog = false;

            },

            /// <summary>
            ///  ChangeState :- Function used to change state based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ChangeState() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("State", this.State);
                var result = await objProject.ChangeState(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else if (result.status = "success") {
                    this.Msg = ("Successfully Change State");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Change State");
                    this.errorSnackbarMsg = true;
                }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.IsActiveToggleDialog = false;
            },

            /// <summary>
            ///  ChangeProjectStatus :- Function used to change project status based on ProjectID
            ///  ProjectStatus 0=draft, 1=deploy, 2=archive, 3=completed
            /// </summary>
            /// <param name="ProjectStatus"></param>
            /// <returns> </returns>
            async ChangeProjectStatus(ProjectStatus) {
                var data = new FormData();
                data.append("ProjectID", JSON.stringify(this.ProjectID));
                data.append("ProjectStatus", ProjectStatus)
                var result = await objProject.ChangeProjectStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else

                    if (result.status = "Success") {
                        if (ProjectStatus == 'Archived') {
                            this.Msg = ("Successfully Archived");
                            this.successSnackbarMsg = true;
                        }
                        else
                            if (ProjectStatus == 'Completed') {
                                this.Msg = ("Successfully Completed");
                                this.successSnackbarMsg = true;
                            }
                            else
                                if (ProjectStatus == 'Archived') {
                                    this.Msg = ("Failed to Archive");
                                    this.errorSnackbarMsg = true;
                                }
                                else
                                    if (ProjectStatus == 'Completed') {
                                        this.Msg = ("Failed to Complete");
                                        this.errorSnackbarMsg = true;
                                    }

                    }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.ArchiveDailog = false;

            },

            /// <summary>
            ///  fnCheck :- Function used to select a theme when click on checkbox in set as template section
            /// </summary>
            /// <param name="item"></param>
            /// <param name="index"></param>
            /// <returns> </returns>
            fnCheck(item, index) {

                var i;
                var preval = this.checkboxAddTheme[index]
                this.selectedTheme = [];
                for (i = 0; i <= this.ThemeItems.length - 1; i++) {
                    this.checkboxAddTheme[i] = false;
                }
                if (preval) {
                    this.checkboxAddTheme[index] = true;
                    this.selectedTheme.push({ checkboxindex: item.ThemeID });
                } else {

                    if (this.selectedTheme.length > 0) {
                        this.selectedTheme = this.selectedTheme.filter(x => x.checkboxindex != item.ThemeID);
                    }
                }
            },

            /// <summary>
            ///  GetTheme :- Function used to fetch project themelist
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetTheme() {
                var result = await objProject.GetTheme();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.ThemeList = result.data;
                }
            },

            /// <summary>
            ///  InsertProjectTemplate :- Function used to add project template
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async InsertProjectTemplate() {

                var data = new FormData();
                data.append('myFile', this.SetasTempFileInput);
                data.append("ThemeID", this.selectedTheme[0].checkboxindex);
                data.append("ProjectTemplateName", this.Template);
                data.append("Description", this.TemplateDescription);
                data.append("ProjectID", this.ProjectID);
                var result = await objProject.InsertProjectTemplate(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "Project successfully set as template";
                        this.successSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  GetProjectTheme :- Function used to fetch project theme details when click on set as template button under actions
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectTheme() {
                var result = await objProject.GetProjectTheme();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.ThemeItems = result.data;

                    }
                    var i = 0;
                    for (i = 0; i < this.ThemeItems.length; i++)
                        this.ThemeItems[i].IsSelected = 0;
                }

            },

            /// <summary>
            ///  GetProjectTemplateByprjtID :- Function used to fetch project template based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectTemplateByprjtID() {

                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                var result = await objProject.GetProjectTemplateByprjtID(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else

                    if (result.data.length > 0) {
                        this.Msg = ("Already set as Template");
                        this.infoSnackbarMsg = true;
                        this.dlgSetasTemplate = false;
                    }
                    else {
                        this.dlgSetasTemplate = true;
                        var i;
                        for (i = 0; i <= this.ThemeItems.length - 1; i++) {
                            this.checkboxAddTheme[i] = false;
                        }
                        this.Template = "";
                        this.TemplateDescription = "";
                        this.AddProjectTempSteps = 1;
                    }
            },
        },
        components: {
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
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
</style>