<template>
    <v-card class="custom-card-1">
        <v-card-title style="height:73px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
              <!--if path is ManageProjectKPIActivityLog-->
                <router-link v-show="$route.name=='ManageProjectKPIActivityLog'" :to="{ name: 'ManageProjectKPI', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">KPI</router-link>
              <!--___________-->
              <!--if path is MyprojectsDraftIndividualKPIActivityLog-->
              <router-link v-show="$route.name=='MyprojectsDraftIndividualKPIActivityLog'" :to="{ name: 'MyprojectsDraftIndividualKPI', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">KPI</router-link>
              <!--___________-->
              > {{KPIInfo.KPIName}}
            </h5>
        </v-card-title>
        <!-- if No Data-->
        <!--<v-card-text>
        <v-row>
            <v-col class="text-center">
                <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                <h4 class="title text-center" color="#585858">No KPIs available</h4>
                <Success-Button class="mt-4" style="width:200px" title="Create KPI" @click.native="CreateKPIDialog=true"></Success-Button>
            </v-col>
        </v-row>
    </v-card-text>-->
        <v-card-text>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-tabs centered v-model="KPIActivityTab" class="pt-0" color="tab-active-line">
                    <v-tab href='#tab-1'>Details</v-tab>
                    <v-tab href='#tab-2'>Activity Log</v-tab>

                    <!--Details-->
                    <v-tab-item value="tab-1">
                        <v-row :justify="justifyCenter" class="mt-4">
                            <!--- View KPI Details  -->
                            <v-col md="4" class="v-scrolling-div" style="height:40vh" v-if="editKPIDetails == false">
                              <v-row justify="center" >
                                <v-col md="12">
                                  <!--Edit button-->
                                  <Secondary-Button id="AutoTest_IP601" title="Edit Details" class="float-right" @click.native="fnEditKPIClick(),editKPIDetails = true" v-bind:disabled ="!screenAccess"></Secondary-Button>
                                </v-col>
                              </v-row>
                                <v-row :justify="justifyCenter">
                                    <v-col md="12" class="pb-0">
                                        <!-- Project Name -->
                                      <h4 class="font-weight-regular heading-3-dark">
                                        KPI Name
                                      </h4>
                                      <h4 class="font-weight-regular heading-3">{{KPIInfo.KPIName}}</h4>
                                        <!-- Project Description -->
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                        KPI Description
                                      </h4>
                                      <h4 class="font-weight-regular heading-3">
                                        {{KPIInfo.Description}}
                                      </h4>
                                        <v-row>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Start Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{formatDate(this.KPIInfo.StartDate.substr(0, 10)) }}</h4>
                                            </v-col>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                End Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{ formatDate(this.KPIInfo.EndDate.substr(0, 10)) }}</h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                KPI Periodicity
                                              </h4>
                                                <h4 class="font-weight-regular heading-3" v-if="KPIInfo.KPIPeriodicityName!='Once'" >{{KPIInfo.KPIPeriodicityName}}</h4>
                                                <div class="page-sub-head pt-0 pb-0 text-limit" v-if="KPIInfo.KPIPeriodicityName=='Once'">
                                                    <h4 class="font-weight-regular heading-3"> {{KPIInfo.KPIPeriodicityName}} -  {{this.KPIInfo.ReportingOnceDate.substr(0, 10) }}</h4>
                                                </div>

                                            </v-col>

                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Workflow
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{GetWFName(KPIInfo.WorkflowID)}}</h4>
                                            </v-col>
                                        </v-row>
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Survey
                                      </h4>
                                      <h4 class="font-weight-regular heading-3">{{GetSurveyName(KPIInfo.SurveyID)}}</h4>
                                    </v-col>
                                </v-row>
                            </v-col>


                            <v-col md="4" class="v-scrolling-div" style="height:40vh" v-if="editKPIDetails == true">
                                <v-text-field outlined="" dense="" label="KPI Name" v-model="EditKPI.KPIName" :rules="[rules.required]">
                                </v-text-field>
                                <v-textarea outlined="" dense="" name="" label="KPI Description" value="" v-model="EditKPI.Description" :rules="[rules.required]"></v-textarea>
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
                                            <v-date-picker v-model="startDate" no-title @input="FrmDateMenu = false,IsValidDate()" format="DD-MM-YYYY" dense :min="ProjectStartDate" :max ="ProjectEndDate"></v-date-picker>
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
                                            <v-date-picker v-model="endDate" no-title @input="ToDateMenu = false,IsValidDate()" format="DD-MM-YYYY" dense :min="startDate" :max ="ProjectEndDate"></v-date-picker>
                                        </v-menu>
                                    </v-col>
                                </v-row>
                                <v-select hide-details dense="" :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="EditKPI.KPIPeriodicity" :rules="[rules.required]">
                                </v-select>
                                <!-- Once -->
                                <div v-show="EditKPI.KPIPeriodicity === 2">
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
                                        <v-date-picker v-model="periodicityDate" no-title @input="periodicityDateMenu = false,IsValidPeriodicityDate()" dense :min="startDate" :max ="endDate"></v-date-picker>
                                    </v-menu>
                                </div>
                                <!-- Weekly-->
                                <div v-show="EditKPI.KPIPeriodicity === 4" class="mt-4">
                                    <v-select dense="" :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="ReportingWeek" hide-details>
                                    </v-select>
                                </div>
                                <!-- Monthly-->
                                <div v-show="EditKPI.KPIPeriodicity === 5" class="mt-4">
                                    <v-select dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDay" hide-details>
                                    </v-select>
                                </div>
                                <!-- Quaterly , Halyearly and Yearly-->
                                <div class="d-inline-flex mt-4" v-if="EditKPI.KPIPeriodicity == 6|| EditKPI.KPIPeriodicity == 7 ||  EditKPI.KPIPeriodicity == 8">
                                    <v-select hide-details class="mr-1 mb-4" dense="" :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="ReportingMonth">
                                    </v-select>
                                    <v-select class="mb-4" hide-details dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDateDay">
                                    </v-select>
                                </div>

                                <v-select class="mt-4" dense="" label="Workflow" :items="workflows" :item-text="i => i.WfName" :item-value="i => i.WorkflowID" outlined="" v-model="editWorkFlow.WorkflowID" >
                                </v-select>
                                <v-select dense="" label="Survey" :items="Surveys" :item-text="i => i.SurveyName" :item-value="i => i.SurveyID" outlined="" v-model="editSurvey.SurveyID" :rules="[rules.required]">
                                </v-select>

                            </v-col>
                        </v-row>
                        <!-- Tags Sections-->
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="4">
                                <span class="small-text mr-1">
                                    <v-icon size="15">mdi-account</v-icon> Users
                                </span>
                                <v-avatar size="27" class="team-avatar" v-if="KPIInfo.UserList == 0">
                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="KPIInfo.UserList.length == 1">
                                    <span class="text--white">{{KPIInfo.UserList[0].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="KPIInfo.UserList.length >= 2">
                                    <span class="text--white">{{KPIInfo.UserList[0].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="KPIInfo.UserList.length >= 2">
                                    <span class="text--white">{{KPIInfo.UserList[1].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="grey" v-if="KPIInfo.UserList.length > 2">
                                    <span class="text--white"> + {{KPIInfo.UserList.length -2}}</span>
                                </v-avatar>
                                <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP602" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                               @click="fnEditKPIClick(),editKPIDetails = true" v-bind:disabled ="!screenAccess">
                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Assign Account User pop up-->
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Add People to Notify
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP603" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AssignAccountUserPopUp=false">
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
                                                          v-model="KPIInfo.UserList">



                                            </v-data-table>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row no-gutters="">
                                                <v-col md="12" class="py-0" align="center">
                                                    <Success-Button id="AutoTest_IP604" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignAccountUserPopUp=false"></Success-Button>
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
                                                <v-btn id="AutoTest_IP605" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                       @click="fnEditKPIClick(),editKPIDetails = true,fnInitializeSteper()" class="mr-1">
                                                    <v-icon size="15">mdi-plus</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-card>
                                                <v-card-text class="pt-0 pb-1">
                                                    <v-col cols="12" md="12">
                                                        <!-- Header Text  -->
                                                        <v-col cols="12" md="12" class="d-inline-flex">
                                                            <v-btn id="AutoTest_IP606" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                <v-icon size="15">mdi-chevron-left</v-icon>
                                                            </v-btn>
                                                            <v-spacer></v-spacer>
                                                            <h3 class="text-left page-head pt-2">
                                                                {{TagHeaderName}}
                                                            </h3>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP607" text @click="CloseTag()">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                        <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                            <v-stepper-items>

                                                                <!-- Create New Tag Option when there is no Tags are -->
                                                                <v-stepper-content step="1" class="pa-0">
                                                                    <v-row>
                                                                        <v-col class="pb-1 ml-2">
                                                                            <v-text-field outlined type="text" dense
                                                                                  label="Name"        v-model="NewTagName" hide-details>
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
                                                                            <v-btn id="AutoTest_IP608" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
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
                                                                                <v-chip id="AutoTest_IP609" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                        label @click.native="fnAddToSelectedTag(index)">
                                                                                    {{ index.TagName }}
                                                                                    <v-icon id="AutoTest_IP610" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                </v-chip>
                                                                            </v-chip-group>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-col cols="12" md="12" class="text-center mt-3">
                                                                        <v-btn id="AutoTest_IP611" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                            <v-btn id="AutoTest_IP612" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                        <v-btn id="AutoTest_IP613" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                        </v-btn>
                                        <v-btn id="AutoTest_IP614" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                        </v-btn>
                                    </v-col>
                                </v-row>
                                <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                    <v-col md="12" class="chip-container mb-0 pt-0">
                                        <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                            <v-chip id="AutoTest_IP615" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                {{TagItem.TagName}}
                                            </v-chip>
                                        </v-chip-group>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <v-row :justify="justifyCenter">
                            <v-col md="6" class="text-center" v-if="editKPIDetails == true">
                              <Success-Button id="AutoTest_IP616" class="btn-122x36 mr-4" title="Save" v-bind:disabled="fnCheckMandatory()" @click.native="fnUpdateKPI()"></Success-Button>
                              <Default-Button-Outlined id="AutoTest_IP617" class="btn-122x36"  title="Cancel" @click.native="editKPIDetails=false,periodicityDate ='',fnGetKPIByKPIId()"></Default-Button-Outlined>
                            </v-col>
                        </v-row>

                    </v-tab-item>
                    <!-- Activity Log-->
                    <v-tab-item value="tab-2">
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="6">
                                <v-card>
                                    <v-divider></v-divider>
                                    <v-card-text style="height:300px" class="v-scrolling-div">
                                        <v-list three-line dense="">
                                            <template v-for="(item, index) in lstActivityLog">
                                                <v-list-item>
                                                    <v-list-item-avatar color="red">
                                                        <span class="text--white">
                                                            {{item.Name.charAt(0)}}
                                                        </span>
                                                    </v-list-item-avatar>
                                                    <v-list-item-content>
                                                        <v-list-item-title class="pb-0 pt-0">
                                                            <v-row class="d-lg-inline-flex">
                                                                <v-col>
                                                                  <h4 class="font-weight-regular heading-3-dark">{{item.Name}}</h4>
                                                                </v-col>
                                                                <v-col>
                                                                  <h4 class="font-weight-regular heading-3">{{item.CreatedDate}}</h4>
                                                                </v-col>
                                                            </v-row>
                                                        </v-list-item-title>
                                                        <v-list-item-subtitle>
                                                                <h4 class="font-weight-regular heading-3-dark">{{item.Message}}</h4>
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

            </v-col>
        </v-card-text>


        <v-snackbar v-model="KPISaveSuccessSnackbar" top="" color="primary">
            KPI details updated successfully
        </v-snackbar>

        <v-snackbar v-model="KPITagSuccessSnackbar" top="" color="primary">
            Tag created successfully
        </v-snackbar>

        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
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
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    
    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };

    export default {

        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                WorkspaceID: 0,
                ProjectId: 0,
                InitiativeId: 0,
                KPIId: 0,
                Type: '',
                Name:'',
                justifyCenter: 'center',
                KPIActivityTab: "",
                editKPIDetails: false,
                KPITagSuccessSnackbar: false,
                KPITagSuccessSnackbar: false,
                KPISaveSuccessSnackbar: false,
                //SelectKPIPeriodicity: ['Never', 'Once', 'Daily', 'Weekly', 'Monthly', 'Quarterly', 'Half Yearly', 'Yearly', 'Forever'],
                workflows: [],
                Surveys:[],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AssignAccountUserPopUp: false,
                //From Date
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                endDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,
               
                EditKPI: {
                    KPIId: 0,
                    KPIName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    KPIPeriodicity: "",
                    KPIPeriodicityName:"",
                    Workflow: "",
                    Survey: "",
                    UserList: [],
                    periodicityDate: new Date().toISOString().substr(0, 10),
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                },
                editWorkFlow: {
                    WorkflowID: 0,
                    WfName: ""
                },
                editSurvey: {
                    SurveyID: 0,
                    SurveyName: ""
                },
                KPIInfo: {
                    KPIId: 0,
                    KPIName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    KPIPeriodicity: "",
                    KPIPeriodicityName: "",
                    WorkflowID: 0,
                    Workflow: "",
                    SurveyID: "",
                    Survey: "",
                    UserList: [],
                    periodicityDate: new Date().toISOString().substr(0, 10),
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                },
                selectedteamUsers: [],
                closeonClick: false,
                closeOnContentClick: false,
                lstuserTableheaders: [{ text: "Name", align: "left", value: "Name" }, { text: "User Role", value: "UserRole" },],
                lstUsers: [],
                lstActivityLog: [],

                lstPeriodicity: [],
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                //ReportingFrequency: 0,
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,
             
                rules: {
                    required: value => !!value || 'Required.',
                },
                periodicityDateMenu: false,

                // Tags ------------
                NewTagMenu: false,
                AllTagListSteps: 0,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

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
                snackbarMsg: false,
                Msg: "",
                periodicityDate :"",
                ProjectStartDate: "",
                ProjectEndDate: "",
                KPIDetailsTableItems :[],
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
            this.Type = this.$route.query.Type;
            this.KPIId = this.$route.query.KPIID;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await this.ChangeTab();
            await this.fnGetGroupDetailsByprjID();
            await this.fnGetTeamUsersList();
            await this.fnGetKPIByKPIId();
            await this.fnGetWorkflows();
            await this.fnGetActivityLog();
            await this.fnGetSurveys();
            await this.GetReportingFrequencyList();
            await this.fnGetAllTags();
            await this.GetProjectDate();


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
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
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
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
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
            fnEditKPIClick() {
              
                this.EditKPI.KPIId = this.KPIInfo.KPIId;
                this.EditKPI.KPIName = this.KPIInfo.KPIName;
                this.EditKPI.Description = this.KPIInfo.Description;
                this.EditKPI.UserList = this.KPIInfo.UserList;
                this.EditKPI.KPIPeriodicity = this.KPIInfo.KPIPeriodicity;
                this.EditKPI.ReportingMonthDay = this.KPIInfo.ReportingMonthDay;
                this.EditKPI.ReportingMonth = this.KPIInfo.ReportingMonth;
                this.EditKPI.ReportingMonthDateDay = this.KPIInfo.ReportingMonthDateDay;
                this.EditKPI.ReportingWeek = this.KPIInfo.ReportingWeek;                
                this.periodicityDate =this.formatDate(this.KPIInfo.ReportingOnceDate);               
                this.startDate = this.KPIInfo.StartDate.substr(0, 10);
                this.endDate = this.KPIInfo.EndDate.substr(0, 10);
                this.editWorkFlow.WorkflowID = this.KPIInfo.WorkflowID;
                this.editSurvey.SurveyID = this.KPIInfo.SurveyID;
                this.editKPIetails = true;
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
            async fnUpdateKPI() {

                /*Chk Duplicate Name*/
                await this.fnGetKPIByprjID();
                for (var i = 0; i < this.KPIDetailsTableItems.length; i++) {
                    if (this.KPIDetailsTableItems[i].KPIId != this.EditKPI.KPIId) {
                        if (this.KPIDetailsTableItems[i].KPIName == this.EditKPI.KPIName) {
                            this.Msg = "Duplicate Entry - Same KPI Name existis..";
                            this.snackbarMsg = true;
                            this.KPIName = "";
                            return;
                        }
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
                data.append("KPIId", this.EditKPI.KPIId);
                data.append("KPIName", this.EditKPI.KPIName);
                data.append("Description", this.EditKPI.Description);
                data.append("KPIPeriodicity", this.EditKPI.KPIPeriodicity);
                data.append("Startdate", this.startDate);
                data.append("Enddate", this.endDate);
                data.append("WorkflowID", this.editWorkFlow.WorkflowID);
                data.append("SurveyID", this.editSurvey.SurveyID);
                data.append("ProjectId", this.ProjectId);
                data.append("Users", JSON.stringify(this.KPIInfo.UserList));
                data.append("Tags", JSON.stringify(this.SelectedTag));
                data.append("ReportingMonth", this.EditKPI.ReportingMonth);
                data.append("ReportingMonthDay", this.EditKPI.ReportingMonthDay);
                data.append("ReportingWeek", this.EditKPI.ReportingWeek);
                
                data.append("ReportingOnceDate", this.formatDate(this.periodicityDate));
                data.append("ReportingMonthDateDay", this.EditKPI.ReportingMonthDateDay);
                var result = await objManageProject.UpdateKPI(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "success") {

                        this.CreateKPIDialog = false,
                        this.KPISaveSuccessSnackbar = true
                        this.fnGetKPIByKPIId();
                        this.editKPIDetails = false;
                    }
                    else {

                        if (result.status == "rptfrequencyfailed") {                           
                            this.Msg = "Reporting Frequency Failed";
                            this.snackbarMsg = true;
                          
                        } else {
                         
                            this.Msg = "Failed";
                            this.snackbarMsg = true;
                        }
                    }
            },
            GetWFName(workflowid) {
                var temp = this.workflows.filter(x => x.WorkflowID == workflowid);
                if (temp.length > 0) {
                    return temp[0].WfName;
                } else {
                    return "Not Defined";
                }
            },
            GetSurveyName(Surveyid) {
                var temp = this.Surveys.filter(x => x.SurveyID == Surveyid);
                if (temp.length > 0) {
                    return temp[0].SurveyName;
                } else {
                    return "Not Defined";
               }
            },
            async ChangeTab(index) {
                var index;
                if (this.Type == 'Edit') {
                    index = 1;
                } else if (this.Type == 'Log') {
                    index = 2;
                }
                if (index == 1) {
                    this.KPIActivityTab = 'tab-1';
                }
                if (index == 2) {
                    this.KPIActivityTab = 'tab-2';
                }

            },
            async fnGetKPIByKPIId() {
             
                var data = new FormData();
                data.append("KPIId", this.KPIId);
                var result = await objManageProject.GetKPIByKPIId(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.KPIInfo = result.data;
                        this.KPIInfo.UserList = JSON.parse(this.KPIInfo.Users);

                        this.KPIInfo.KPIPeriodicity = result.data.KPIPeriodicity;
                        this.KPIInfo.KPIPeriodicityName = result.data.ReportingFrequencyName;
                        this.KPIInfo.ReportingMonth = result.data.ReportingMonth;
                        this.KPIInfo.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.KPIInfo.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.KPIInfo.ReportingWeek = result.data.ReportingWeek;
                       
                        if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                            this.KPIInfo.ReportingOnceDate = result.data.ReportingOnceDate;
                        }
                        if (result.data.Tags != null && result.data.Tags != undefined && result.data.Tags != "") {
                            this.SelectedTag = JSON.parse(result.data.Tags);
                        }
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
            async fnGetActivityLog() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("KPIId", this.KPIId);
                var result = await objManageProject.GetKPIActivityLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstActivityLog = result.data;
                    }
                }
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
            fnCheckMandatory() {
                if (this.EditKPI.KPIName == "" ||
                    this.EditKPI.Description == "" ||
                    this.EditKPI.KPIPeriodicity == 0 ||
                 
                    (this.EditKPI.KPIPeriodicity== 2 && (this.periodicityDate == "" || this.periodicityDate == null)) ||
                    (this.EditKPI.startDate == "" || this.EditKPI.startDate ==null)||
                    (this.EditKPI.endDate == "" || this.EditKPI.endDate == null) ||                  
                    this.KPIInfo.UserList == 0)
                    return false;
                else return true;
            },
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
                        this.KPITagSuccessSnackbar = true;
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags();
                        this.NewTagName = "";
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

            }  ,
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },

            CloseTag() {
  
                this.color = "";
                this.TagHeaderName = 'Add Tag';
                this.NewTagName = "";
                this.NewTagMenu = false;
                this.EditTagName = "";
                
                
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