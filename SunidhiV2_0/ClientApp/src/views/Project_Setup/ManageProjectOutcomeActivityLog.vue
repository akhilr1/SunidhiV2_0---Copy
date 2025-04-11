<template>
    <v-card class="elevation-0">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-card-title style="height:50px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                <!--if path is ManageProjectKPIActivityLog-->
                <router-link v-show="$route.name=='ManageProjectOutcomeActivityLog'" :to="{ name: 'ManageProjectOutcomes', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Outcome</router-link>
                <!--___________-->
                <!--if path is MyprojectsDraftIndividualKPIActivityLog-->
                <router-link v-show="$route.name=='MyProjectDraftIndividualOutcomeActivityLog'" :to="{ name: 'MyprojectsDraftIndividualKPIOutcomes', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Outcome</router-link>
                <!--___________-->
                <!--if path is MyprojectsDeployedIndividualKPIActivityLog-->
                <router-link v-show="$route.name=='MyProjectDeployedIndividualOutcomeActivityLog'" :to="{ name: 'MyprojectsDeployedIndividualKPIOutcomes', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Outcome</router-link>
                <!--___________-->
                > {{OutcomeInfo.OutcomeName}}
            </h5>
        </v-card-title>
        <v-card-text>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-tabs centered v-model="OutcomeActivityTab" class="pt-0" color="tab-active-line">
                    <v-tab href='#tab-1'>Details</v-tab>
                    <v-tab href='#tab-2' @click="fnGetActivityLog()">Activity Log</v-tab>
                    <v-tab id="AutoTest_IP174" href='#tab-3' @click="fnMountPivotTab();">Pivot Table</v-tab>
                    <!--Details-->
                    <v-tab-item value="tab-1">
                        <v-row justify="center" v-if="IsEditOutCome == false">
                            <v-col md="9" class="pr-10">
                                <!--Edit button-->
                                <Secondary-Button id="AutoTest_IP601" title="Edit Details" class="float-right" @click.native="fnEditOutcomeClick(),IsEditOutCome = true" v-bind:disabled="!screenAccess"></Secondary-Button>
                            </v-col>
                        </v-row>
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="10">
                                <!--- View Outcome Details  -->
                                <v-row :justify="justifyCenter" v-if="IsEditOutCome == false">
                                    <v-col md="10" class="v-scrolling-div" style="height:55vh">
                                        <v-row :justify="justifyCenter">
                                            <v-col md="12" class="pb-0">
                                                <!-- Project Name -->
                                                <h4 class="font-weight-regular heading-3-dark">
                                                    Outcome Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{OutcomeInfo.OutcomeName}}</h4>
                                                <v-row>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-2">
                                                            Start Date
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3">{{formatDate(OutcomeInfo.StartDate.substr(0, 10)) }}</h4>
                                                    </v-col>
                                                    <v-col md="6" class="text-center">
                                                        <h4 class="font-weight-regular heading-3-dark mt-2">
                                                            End Date
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3">{{ formatDate(OutcomeInfo.EndDate.substr(0, 10)) }}</h4>
                                                    </v-col>
                                                </v-row>

                                                <!-- Impact Indicators-->
                                                <v-row>
                                                    <!--<v-col cols="12">
                                                        <p class="font-weight-regular heading-3-dark">SDG Indicators</p>
                                                    </v-col>-->
                                                    <v-col cols="12">
                                                        <p class="font-weight-regular heading-3-dark">SDG Indicators</p>
                                                    </v-col>
                                                </v-row>
                                                <v-row no-gutters class="pt-0" v-for="(datas, index) in SDGIndicators" :key="datas.pk_SDGID">
                                                    <v-col md="auto">
                                                        <v-list-item-icon>
                                                            <v-avatar size="50" tile class="elevation-1" light>
                                                                <img :src="imagesInput[datas.pk_SDGID]" class="team-image" />
                                                            </v-avatar>
                                                        </v-list-item-icon>
                                                    </v-col>
                                                    <v-col class="pt-2">
                                                        <p class="text-left" style="line-height: 17px">
                                                            <span class="mt-2 mb-0"
                                                                  style="max-width: 300px; display: block">
                                                                {{ datas.SDG_Name }}
                                                            </span>
                                                            <small>
                                                                <span> {{datas.Targets.length}} targets </span>
                                                            </small>
                                                        </p>
                                                    </v-col>
                                                    <v-col sm="12">
                                                    </v-col>
                                                    <v-expansion-panels class="mb-1">
                                                        <v-expansion-panel>
                                                            <v-expansion-panel-header class="v-expansion-panel-header">
                                                                <v-row>
                                                                    <v-col sm="10" class="pl-5 pt-6">       {{datas.Targets.length}} Selected Targets</v-col>
                                                                </v-row>
                                                            </v-expansion-panel-header>
                                                            <v-expansion-panel-content class="v-expansion-panel-content__wrap">
                                                                <v-divider></v-divider>
                                                                <v-row class="v-scrolling-divs " no-gutters>
                                                                    <template v-for="(data,index) in datas.Targets">
                                                                        <v-col sm="12" :key="data.fk_SDGID">
                                                                            <v-row justify="center">
                                                                                <v-col sm="10">
                                                                                    <span>{{data.Target_Name}}</span>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-divider></v-divider>
                                                                        </v-col>
                                                                    </template>
                                                                </v-row>
                                                            </v-expansion-panel-content>
                                                        </v-expansion-panel>
                                                    </v-expansion-panels>
                                                </v-row>
                                                <v-row>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-2">
                                                            Periodicity
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3" v-if="OutcomeInfo.ReportingFrequencyName!='Once'">{{OutcomeInfo.ReportingFrequencyName}}</h4>
                                                        <div class="page-sub-head pt-0 pb-0 text-limit" v-if="OutcomeInfo.ReportingFrequencyName=='Once'">
                                                            <h4 class="font-weight-regular heading-3"> {{OutcomeInfo.ReportingFrequencyName}} -  {{OutcomeInfo.ReportingOnceDate.substr(0, 10) }}</h4>
                                                        </div>
                                                    </v-col>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-2">
                                                            Workflow
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3">{{OutcomeInfo.WorkflowName}}</h4>
                                                    </v-col>
                                                </v-row>
                                                <v-row>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                                            Survey
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3">{{OutcomeInfo.SurveyName}}</h4>
                                                    </v-col>
                                                    <v-col md="6">
                                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                                            Datacollection Type
                                                        </h4>
                                                        <h4 class="font-weight-regular heading-3">{{OutcomeInfo.DatacollectionType}}</h4>
                                                    </v-col>
                                                </v-row>
                                                <v-row :justify="justifyCenter" v-if="IsEditOutCome == false">
                                                    <v-col class="text-left d-inline-flex" md="12">

                                                        <span class="small-text mr-1">
                                                            <v-icon size="15">mdi-account</v-icon> Users
                                                        </span>
                                                        <!--<v-tooltip left>-->
                                                        <!--<template v-slot:activator="{ on, attrs }">-->
                                                        <!--<v-avatar size="27" class="team-avatar" color="red" v-on="on">
                                                            <span class="text--white">
                                                                {{OutcomeInfo.UserList[0].Name.charAt(0)}}
                                                            </span>
                                                        </v-avatar>-->



                                                        <v-avatar size="25" class="team-avatar" v-for="(item,key) in OutcomeInfo.UserList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">

                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>

                                                        </v-avatar>
                                                        <v-avatar v-if="OutcomeInfo.UserList.length>=4" size="25" class="team-count">
                                                            +{{OutcomeInfo.UserList.length-3}}

                                                        </v-avatar>

                                                        <!--</template>-->
                                                        <!--<span v-if="OutcomeInfo.UserList.length >= 1">{{OutcomeInfo.UserList[0].Name}}'333333333'</span>-->
                                                        <!--</v-tooltip>-->
                                                        <!--<v-tooltip left>
                                                        <template v-slot:activator="{ on, attrs }">
                                                            <v-avatar size="27" class="team-avatar" color="red" v-if="OutcomeInfo.UserList.length >= 2" v-on="on">
                                                                <span class="text--white">{{OutcomeInfo.UserList[1].Name.charAt(0)}}</span>
                                                            </v-avatar>
                                                        </template>-->
                                                        <!--<span v-if="OutcomeInfo.UserList.length >= 2">{{OutcomeInfo.UserList[1].Name}}</span>-->
                                                        <!--</v-tooltip>-->

                                                        <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                            <template v-slot:activator="{ on }">
                                                                <v-btn id="AutoTest_IP602" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                                       @click="fnEditOutcomeClick(),IsEditOutCome = true" v-bind:disabled="!screenAccess">
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
                                                                                  item-key="ID"
                                                                                  sort-by="Name"
                                                                                  v-model="EditOutcome.UserList">



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
                                                    </v-col>
                                                </v-row>

                                            </v-col>
                                        </v-row>
                                        <v-row :justify="justifyCenter" class="mb-1">
                                            <v-col class="text-left d-inline-flex" md="12">
                                                <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn id="AutoTest_IP605" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                               @click="fnEditOutcomeClick(),IsEditOutCome = true,fnInitializeSteper()" class="mr-1" v-bind:disabled="!screenAccess">
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
                                                                                                  label="Name" v-model="NewTagName" hide-details>
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
                                        <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0" :justify="justifyCenter">
                                            <v-col md="7" class="chip-container mb-0 pt-0">
                                                <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                    <v-chip id="AutoTest_IP615" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0" v-bind:disabled="!screenAccess">
                                                        {{TagItem.TagName}}
                                                    </v-chip>
                                                </v-chip-group>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                                <!-- Only visible edit button click-->

                                <v-row :justify="justifyCenter" v-if="IsEditOutCome == true">
                                    <v-col md="10" style="text-align:end;" v-if="IsEditOutCome == true">
                                        <Success-Button id="AutoTest_IP616" class="btn-122x36 mr-4" title="Save" @click.native="fnUpdateOutcome()"></Success-Button>
                                        <Default-Button-Outlined id="AutoTest_IP617" class="btn-122x36" title="Cancel" @click.native="IsEditOutCome=false,periodicityDate ='' "></Default-Button-Outlined>
                                    </v-col>
                                    <v-col md="10" class="v-scrolling-div" style="height:55vh">
                                        <v-text-field outlined="" dense="" label="Outcome Name" v-model="EditOutcome.OutcomeName" :rules="[rules.required]">
                                        </v-text-field>
                                        <v-row :justify="justifyCenter" v-if="IsEditOutCome == false">
                                            <v-col class="text-left d-inline-flex" md="10">

                                                <span class="small-text mr-1">
                                                    <v-icon size="15">mdi-account</v-icon> Users
                                                </span>
                                                <!--<v-tooltip left>-->
                                                <!--<template v-slot:activator="{ on, attrs }">-->
                                                <!--<v-avatar size="27" class="team-avatar" color="red" v-on="on">
                                                    <span class="text--white">
                                                        {{OutcomeInfo.UserList[0].Name.charAt(0)}}
                                                    </span>
                                                </v-avatar>-->



                                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in OutcomeInfo.UserList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">

                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>

                                                </v-avatar>
                                                <v-avatar v-if="OutcomeInfo.UserList.length>=4" size="25" class="team-count">
                                                    +{{OutcomeInfo.UserList.length-3}}

                                                </v-avatar>

                                                <!--</template>-->
                                                <!--<span v-if="OutcomeInfo.UserList.length >= 1">{{OutcomeInfo.UserList[0].Name}}'333333333'</span>-->
                                                <!--</v-tooltip>-->
                                                <!--<v-tooltip left>
                                                <template v-slot:activator="{ on, attrs }">
                                                    <v-avatar size="27" class="team-avatar" color="red" v-if="OutcomeInfo.UserList.length >= 2" v-on="on">
                                                        <span class="text--white">{{OutcomeInfo.UserList[1].Name.charAt(0)}}</span>
                                                    </v-avatar>
                                                </template>-->
                                                <!--<span v-if="OutcomeInfo.UserList.length >= 2">{{OutcomeInfo.UserList[1].Name}}</span>-->
                                                <!--</v-tooltip>-->

                                                <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn id="AutoTest_IP602" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                               @click="fnEditOutcomeClick(),IsEditOutCome = true" v-bind:disabled="!screenAccess">
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
                                                                          item-key="ID"
                                                                          sort-by="Name"
                                                                          v-model="EditOutcome.UserList">



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
                                            </v-col>
                                        </v-row>
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
                                                                      persistent-hint dense outlined hide-details
                                                                      v-on="on"></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="EditOutcome.StartDate" color="primary" no-title @input="FrmDateMenu = false,IsValidDate()" format="DD-MM-YYYY" dense :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
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
                                                                      persistent-hint outlined dense hide-details
                                                                      v-on="on"></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="EditOutcome.EndDate" color="primary" no-title @input="ToDateMenu = false,IsValidDate()" format="DD-MM-YYYY" dense :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                                </v-menu>
                                            </v-col>
                                        </v-row>

                                        <!-- Impact Indicators --->

                                        <h4 class="text-left heading-3 mt-2 mb-2">
                                            Impact Indicators ({{ lstSGDData.length }} SDG Selected)
                                        </h4>
                                        <v-btn @click="EditImpactIndicator = true" width="450" class="mb-3" outlined dense>Select Impact Indicators</v-btn>
                                        <v-dialog v-model="EditImpactIndicator" width="650" persistent :retain-focus="false">
                                            <v-form ref="entryForm" @submit.prevent="submitHandler">
                                                <v-card>
                                                    <v-card-title primary-title="" class="page-head pb-0">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                        </v-avatar>
                                                        Impact Indicators
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP563" text="" @click="EditImpactIndicator = false,impactError= false">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-card-title>
                                                        <v-chip-group>
                                                            <v-chip v-for="(data,index) in  lstSGDData"
                                                                    :key="index"
                                                                    close
                                                                    small
                                                                    @click:close="fnRemoveSDGIndicators(data.pk_SDGID)">
                                                                <v-avatar left>
                                                                    <img :src="imagesInput[data.pk_SDGID]" class="team-image" />
                                                                </v-avatar>
                                                                <span>{{ lstSGDData.length }} SDG Selected</span>
                                                            </v-chip>
                                                        </v-chip-group>
                                                        <span class="ml-3" style="color:red;font-size:13px;" v-if="impactError == true">Please Select atleast one impact indicator before saving *</span>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                            <!--Screen List with checkbox add here-->
                                                            <v-data-table :headers="impactIndicatorHeader"
                                                                          :items="lstSGD"
                                                                          hide-default-footer
                                                                          class="elevation-0"
                                                                          fixed-header
                                                                          height="53vh"
                                                                          disable-pagination>

                                                                <template v-slot:item.impact="{item}">
                                                                    <v-list-item-action>
                                                                        <v-checkbox v-model="item.Selected"
                                                                                    @change="GetSDGTargetsBasedOnSDGID(item.pk_SDGID,item.SDG_Name)"
                                                                                    color="green"></v-checkbox>
                                                                    </v-list-item-action>
                                                                    <v-list-item-avatar tile class="mr-2">
                                                                        <img :src="imagesInput[item.pk_SDGID]" class="team-image" />
                                                                    </v-list-item-avatar>
                                                                    {{item.SDG_Name}}
                                                                </template>

                                                            </v-data-table>
                                                        </v-col>
                                                    </v-card-title>
                                                    <v-card-title>
                                                        <v-row justify="end">
                                                            <v-col sm="3">
                                                                <v-btn color="#47BB76" style="color:white" width="160" type="submit" @click="submitHandler()">Save</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-title>
                                                </v-card>
                                            </v-form>
                                        </v-dialog>


                                        <v-row no-gutters class="pt-0" v-for="(datas, index) in lstSGDData" :key="datas.pk_SDGID">
                                            <v-col md="auto">
                                                <v-list-item-icon>
                                                    <v-avatar size="50" tile class="elevation-1" light>
                                                        <img :src="imagesInput[datas.pk_SDGID]" class="team-image" />
                                                    </v-avatar>
                                                </v-list-item-icon>
                                            </v-col>
                                            <v-col class="pt-2">
                                                <p class="text-left" style="line-height: 17px">
                                                    <span class="mt-2 mb-0"
                                                          style="max-width: 300px; display: block">
                                                        {{ datas.SDG_Name }}
                                                    </span>
                                                    <small>
                                                        <span> {{fnSelectedTargetCount(datas)}} targets </span>
                                                    </small>
                                                </p>
                                            </v-col>
                                            <v-col sm="12">
                                                <v-btn v-if="fnSelectedTargetCountPerImpact(datas) < 1" @click="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID,AddImpactIndicatorTarget = false" width="450" class="mb-3" outlined dense>Select Targets</v-btn>
                                            </v-col>
                                            <v-expansion-panels v-if="fnSelectedTargetCountPerImpact(datas) > 0" class="mb-1">
                                                <v-expansion-panel>
                                                    <v-expansion-panel-header class="v-expansion-panel-header">
                                                        <v-row>
                                                            <v-col sm="11" class="pl-5 pt-6">       {{fnSelectedTargetCountPerImpact(datas)}} Selected Targets</v-col>
                                                            <v-col sm="1">
                                                                <v-btn class="mx-2"
                                                                       fab
                                                                       dark
                                                                       color="green"
                                                                       elevation="0"
                                                                       x-small
                                                                       @click.native.stop="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID,AddImpactIndicatorTarget = false">
                                                                    <v-icon dark size="20">
                                                                        mdi-plus
                                                                    </v-icon>
                                                                </v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-expansion-panel-header>
                                                    <v-expansion-panel-content class="v-expansion-panel-content__wrap">
                                                        <v-divider></v-divider>
                                                        <v-row class="v-scrolling-divs" no-gutters>
                                                            <template v-for="(data,index) in SelectedSDGTargets">
                                                                <v-col sm="12" :key="data.fk_SDGID" v-if="data.fk_SDGID == datas.pk_SDGID">
                                                                    <v-row justify="center">
                                                                        <v-col sm="12" class="pl-7">
                                                                            <span>{{data.Target_Name}}</span>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-divider></v-divider>
                                                                </v-col>
                                                            </template>
                                                        </v-row>
                                                    </v-expansion-panel-content>
                                                </v-expansion-panel>
                                            </v-expansion-panels>


                                            <v-dialog v-model="addImpactTarget" width="650" persistent :retain-focus="false">
                                                <v-form ref="entryForm" @submit.prevent="submitHandler">
                                                    <v-card>
                                                        <v-card-title primary-title="" class="page-head pb-0">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                                <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                            </v-avatar>
                                                            Targets
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP563" text="" @click="addImpactTarget = false,AddImpactIndicatorTarget = false">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-title>
                                                            <v-chip-group v-for="(data,index) in  SelectedSDGTargets" :key="index">
                                                                <v-chip v-if="data.fk_SDGID == targetId"
                                                                        small>
                                                                    <span>{{ data.Target_Name.substring(0, 5)}} </span>
                                                                </v-chip>
                                                            </v-chip-group>
                                                            <span class="ml-3" style="color:red;font-size:13px;" v-if="AddImpactIndicatorTarget == true">Please Select atleast one target before saving *</span>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">

                                                                <v-data-table :headers="impactTargetIndicatorHeader"
                                                                              :items="targetData"
                                                                              hide-default-footer
                                                                              class="elevation-0"
                                                                              fixed-header
                                                                              height="53vh"
                                                                              :key="datas.pk_SDGID"
                                                                              disable-pagination>

                                                                    <template v-slot:item.impact="{item}">
                                                                        <td>
                                                                            <v-checkbox @change="storeTargets(item,index)"
                                                                                        :input-value="SelectedSDGTargets.some(e=> e.Target_Name == item.Target_Name)"
                                                                                        color="green"></v-checkbox>
                                                                        </td>
                                                                        <td>
                                                                            {{item.Target_Name}}
                                                                        </td>
                                                                    </template>

                                                                </v-data-table>
                                                            </v-col>
                                                        </v-card-title>
                                                        <v-card-title>
                                                            <v-row justify="end">
                                                                <v-col sm="3">
                                                                    <v-btn color="#47BB76" style="color:white" width="160" type="submit" @click="submitHandlerTarget()">Save</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-title>
                                                    </v-card>
                                                </v-form>
                                            </v-dialog>
                                        </v-row>

                                        <!-- Listing selected SDG Targets-->
                                        <!-- <v-row no-gutters v-for="(item, index) in lstSGDData" :key="item.pk_SDGID">
                                            <v-col md="auto">
                                                <v-list-item-icon>
                                                    <v-avatar color="white"
                                                              size="25"
                                                              class="elevation-1 mr-1">
                                                        <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                                    </v-avatar>
                                                </v-list-item-icon>
                                            </v-col>
                                            <v-col>
                                                <p class="text-left" style="line-height: 17px">
                                                    <span class="mt-2 mb-0"
                                                          style="max-width: 300px; display: block">
                                                        {{ item.SDG_Name }}
                                                    </span>
                                                    <small>
                                                        <span> {{fnSelectedTargetCount(item)}} targets </span>
                                                    </small>
                                                </p>
                                            </v-col>
                                            <v-col>
                                                <v-autocomplete v-model="SelectedSDGTargets"
                                                                :items="item.Targets"
                                                                outlined
                                                                dense
                                                                chips
                                                                small-chips
                                                                label="Select Targets"
                                                                item-text="Target_Name"
                                                                item-value="pk_SDGTargetID"
                                                                hide-details
                                                                class="mb-2"
                                                                multiple
                                                                :rules="[rules.required]"
                                                                @change="" return-object="">
                                                </v-autocomplete>
                                            </v-col>
                                        </v-row> -->
                                        <!-- Data Collection -->
                                        <h4 class="heading-3 my-2">Data Collection</h4>
                                        <v-select dense v-model="EditOutcome.DatacollectionType"
                                                  :items="DataCollectionTypeItems"
                                                  label="Select Outcome Type"
                                                  outlined @change="fnGetLogicType(EditOutcome.DatacollectionType)">
                                        </v-select>
                                        <v-select dense
                                                  :items="SurveyList" :item-text="i => i.SurveyName" :item-value="i => i.SurveyID"
                                                  label="Select Survey" v-model="EditOutcome.SurveyId" @change="GetSectionsBySurveyID(EditOutcome.SurveyId)"
                                                  outlined :rules="[rules.required]">
                                        </v-select>
                                        <div v-if="EditOutcome.DatacollectionType=='Quantitative'">
                                            <!-- Section List-->
                                            <v-select :items="SectionList"
                                                      label="Select Section"
                                                      item-text="SectionName"
                                                      item-value="SectionID"
                                                      outlined
                                                      dense
                                                      v-model="EditOutcome.SurveySectionId" @change="GetQstnBySectionID(EditOutcome.SurveySectionId)"></v-select>
                                            <!-- Question List-->
                                            <v-select :items="QuestionList"
                                                      label="Select Question"
                                                      item-text="QuestionName"
                                                      item-value="QuestionID"
                                                      outlined
                                                      dense
                                                      @change="GetResponsesByQstnid(EditOutcome.SurveyQuestionId)"
                                                      v-model="EditOutcome.SurveyQuestionId"></v-select>
                                            <!-- Respose List-->
                                            <!--<v-select :items="ResponseList"
                                            label="Select Response"
                                            item-text="ResponseName"
                                            item-value="ResponseID"
                                            outlined
                                            dense
                                            @change=""
                                            v-model="EditOutcome.ResponseID"></v-select>-->
                                            <!-- Define Logic-->
                                            <v-select dense
                                                      :items="LogicItems"
                                                      item-text="Type"
                                                      item-value="ID"
                                                      label="Define Logic"
                                                      v-model="EditOutcome.SurveyLogic"
                                                      outlined="">
                                            </v-select>
                                        </div>
                                        <v-row>
                                            <v-col v-if="(EditOutcome.SurveyLogic==1 || EditOutcome.SurveyLogic==2 || logicAnsType=='Single Line Text' || logicAnsType=='Numeric')  && (EditOutcome.DatacollectionType == 'Quantitative')">
                                                <v-text-field outlined dense v-model="EditOutcome.SurveyTarget"></v-text-field>
                                            </v-col>
                                            <v-col v-else>
                                                <v-select v-show="EditOutcome.DatacollectionType == 'Quantitative'" dense :items="lstLogicAnswers" v-model="EditOutcome.DefineTarget"
                                                          item-text="OptionValue"
                                                          item-value="OptionID"
                                                          label="Define Target" outlined="">
                                                </v-select>
                                            </v-col>
                                        </v-row>
                                        <v-select hide-details dense="" :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="EditOutcome.Periodicity" :rules="[rules.required]">
                                        </v-select>
                                        <!-- Once -->
                                        <div v-show="EditOutcome.Periodicity === 2">
                                            <v-menu v-model="periodicityDateMenu"
                                                    :close-on-content-click="false"
                                                    transition="scale-transition"
                                                    offset-y
                                                    max-width="290px"
                                                    min-width="290px">
                                                <template v-slot:activator="{ on }">
                                                    <v-text-field v-model="FormattedDateEdit"
                                                                  label="Choose a Date"
                                                                  persistent-hint outlined
                                                                  v-on="on" class="mt-4" hide-details dense></v-text-field>
                                                </template>
                                                <v-date-picker v-model="EditOutcome.ReportingOnceDate" color="primary" no-title @input="periodicityDateMenu = false,IsValidPeriodicityDate()" dense :min="EditOutcome.StartDate" :max="EditOutcome.EndDate"></v-date-picker>
                                            </v-menu>
                                        </div>
                                        <!-- Weekly-->
                                        <div v-show="EditOutcome.Periodicity === 4" class="mt-4">
                                            <v-select dense="" :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="EditOutcome.ReportingWeek" hide-details>
                                            </v-select>
                                        </div>
                                        <!-- Monthly-->
                                        <div v-show="EditOutcome.Periodicity === 5" class="mt-4">
                                            <v-select dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="EditOutcome.ReportingMonthDay" hide-details>
                                            </v-select>
                                        </div>
                                        <!-- Quaterly , Halyearly and Yearly-->
                                        <div class="d-inline-flex mt-4" v-if="EditOutcome.Periodicity == 6|| EditOutcome.Periodicity == 7 ||  EditOutcome.Periodicity == 8">
                                            <v-select hide-details class="mr-1 mb-4" dense="" :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="EditOutcome.ReportingMonth">
                                            </v-select>
                                            <v-select class="mb-4" hide-details dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="EditOutcome.ReportingMonthDateDay">
                                            </v-select>
                                        </div>
                                        <v-select class="mt-4" dense="" label="Workflow" :items="workflows" :item-text="i => i.WfName" :item-value="i => i.WorkflowID" outlined="" v-model="EditOutcome.WorkflowId">
                                        </v-select>
                                        <!-- Users List-->
                                        <v-autocomplete v-model="EditOutcome.UserList"
                                                        :items="lstUsers"
                                                        outlined
                                                        dense
                                                        chips
                                                        small-chips
                                                        label="Select Users"
                                                        item-text="Name"
                                                        item-value="ID"
                                                        hide-details class="mb-2" multiple :rules="[rules.required]" return-object="">
                                            <template v-slot:selection="data">
                                                <v-chip v-bind="data.attrs"
                                                        :input-value="data.selected"
                                                        close
                                                        small
                                                        @click="data.select"
                                                        @click:close="fnRemoveSelectedUsers(data.item)">
                                                    <v-avatar size="12" left light :color="`${getcolor(data.item.Name)}`">
                                                        <img v-bind:src="userimage + data.item.ProfileImage" alt="User DP" v-if="data.item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>  {{data.item.Name == null ? "" :data.item.Name.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                    {{ data.item.Name }}
                                                </v-chip>
                                            </template>
                                            <template v-slot:item="data">
                                                <v-list-item-action>
                                                    <v-checkbox @change="fnGetSelectedUserList(data.item.ID)" v-model="data.item.IsSelected"></v-checkbox>
                                                </v-list-item-action>
                                                <v-list-item-icon>
                                                    <v-avatar size="30" class="elevation-1" light :color="`${getcolor(data.item.Name)}`">
                                                        <img v-bind:src="userimage + data.item.ProfileImage" alt="User DP" v-if="data.item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{data.item.Name == null ? "" : data.item.Name.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                </v-list-item-icon>
                                                <v-list-item-content>
                                                    <v-list-item-title v-html="data.item.Name" class="ml-2"></v-list-item-title>
                                                </v-list-item-content>
                                            </template>
                                        </v-autocomplete>

                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <!-- User Sections and Tag Sections-->
                        <v-row :justify="justifyCenter" class="mt-0">

                            <v-col md="10">
                                <!-- Tags-->
                            </v-col>
                        </v-row>

                    </v-tab-item>
                    <!-- Activity Log-->
                    <v-tab-item value="tab-2">
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="6">
                                <v-card class="elevation-0">
                                    <v-card-text style="height:300px" class="v-scrolling-div">
                                        <v-list dense="" class="py-0">
                                            <template v-for="(item, index) in lstActivityLog">
                                                <v-list-item class="py-0">
                                                    <v-list-item-icon>
                                                        <v-avatar size="40" :color="`${getcolor(item.Name)}`">
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
                    <!-- Pivot Table-->
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
                                                                <v-list-item v-on="on" v-bind="attrs" >
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
                                                            <v-col md="4" class="text-center py-0">
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
        </v-card-text>

        <v-snackbar v-model="UpdateSuccesssnackBar" top="" color="primary">
            Outcome details updated successfully
        </v-snackbar>

        <v-snackbar v-model="OutcomeTagSuccessSnackbar" top="" color="primary">
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
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objManageOutcomes;
    async function importOutcomesscript() {
        return Promise.all([
            import("../../BL/Outcomes.js").then(mod => {
                objManageOutcomes = new mod.Outcomes();
            })
        ]);
    };
    var objManageProjectActivity;
    async function importobjManageProjectscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };

    export default {

        data() {
            return {
                EditImpactIndicator: false,
                addImpactTarget: false,
                //AddImpactIndicator:false,
                impactIndicatorHeader: [
                    {
                        text: 'Select Impact Indicator',
                        align: 'left',
                        value: 'impact',
                    }
                ],
                impactTargetIndicatorHeader: [
                    {
                        text: 'Select Targets',
                        align: 'left',
                        value: 'impact',
                    }
                ],
                //lstSDGTargetIndicatorsData: [],
                AddImpactIndicatorTarget: false,
                imagesInput: {
                    1: require("@/assets/createOutcome/1.png"),
                    2: require("@/assets/createOutcome/2.png"),
                    3: require("@/assets/createOutcome/3.png"),
                    4: require("@/assets/createOutcome/4.png"),
                    5: require("@/assets/createOutcome/5.png"),
                    6: require("@/assets/createOutcome/6.png"),
                    7: require("@/assets/createOutcome/7.png"),
                    8: require("@/assets/createOutcome/8.png"),
                    9: require("@/assets/createOutcome/9.png"),
                    10: require("@/assets/createOutcome/10.png"),
                    11: require("@/assets/createOutcome/11.png"),
                    12: require("@/assets/createOutcome/12.png"),
                    13: require("@/assets/createOutcome/13.png"),
                    14: require("@/assets/createOutcome/14.png"),
                    15: require("@/assets/createOutcome/15.png"),
                    16: require("@/assets/createOutcome/16.png"),
                    17: require("@/assets/createOutcome/17.png"),
                },
                impactError: false,
                PreLoader: false,
                screenAccess: 0,
                RWAccess: 0,
                WorkspaceID: 0,
                ProjectId: 0,
                InitiativeId: 0,
                OutcomeId: 0,
                Type: '',
                Name: '',
                justifyCenter: 'center',
                OutcomeActivityTab: "tab-1",
                IsEditOutCome: false,

                //From Date
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                endDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                // Assign User
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AssignAccountUserPopUp: false,

                OutcomeInfo: {
                    pk_OutcomeID: 0,
                    OutcomeName: '',
                    StartDate: new Date().toISOString().substr(0, 10),
                    EndDate: new Date().toISOString().substr(0, 10),
                    DataCollectionType: '',
                    SurveyId: 0,
                    SurveyName: '',
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyLogic: 0,
                    SurveyTarget: 0,
                    DefineTarget: 0,
                    WorkflowId: 0,
                    WorkflowName: '',
                    Periodicity: '',
                    IsDeleted: 0,
                    CreatedDate: '',
                    UpdatedDate: '',
                    CreatedBy: 0,
                    UpdatedBy: 0,
                    ReportingFrequencyName: '',
                    ReportingMonth: 0,
                    ReportingMonthDay: 0,
                    ReportingWeek: 0,
                    ReportingMonthDateDay: 0,
                    ReportingOnceDate: '',
                    UserList: [],
                    lstSDGList: [],
                    lstSDGTargetsList: [],
                    lstUserList: [],
                },

                EditOutcome: {
                    pk_OutcomeID: 0,
                    OutcomeName: '',
                    StartDate: new Date().toISOString().substr(0, 10),
                    EndDate: new Date().toISOString().substr(0, 10),
                    DatacollectionType: '',
                    SurveyId: 0,
                    SurveyName: '',
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyLogic: 0,
                    SurveyTarget: 0,
                    DefineTarget: 0,
                    WorkflowId: 0,
                    WorkflowName: '',
                    Periodicity: '',
                    IsDeleted: 0,
                    CreatedDate: '',
                    UpdatedDate: '',
                    CreatedBy: 0,
                    UpdatedBy: 0,
                    ReportingFrequencyName: '',
                    ReportingMonth: 0,
                    ReportingMonthDay: 0,
                    ReportingWeek: 0,
                    ReportingMonthDateDay: 0,
                    ReportingOnceDate: new Date().toISOString().substr(0, 10),
                    UserList: [],
                    lstSDGList: [],
                    lstSDGTargetsList: [],
                    lstUserList: [],
                    QuestionName: '',
                    SectionName: '',
                    SurveyLogicName: '',
                    SurveyTargetName: ''
                },

                workflows: [],
                Surveys: [],
                SDGIndicators: [],
                OutcomeTagSuccessSnackbar: false,
                OutcomeSaveSuccessSnackbar: false,
                //OutcomeDetailsTableItems: [],

                //selectedteamUsers: [],
                //closeonClick: false,
                //closeOnContentClick: false,
                lstuserTableheaders: [{ text: "ID", align: ' d-none', value: "ID" }, { text: "Name", align: "left", value: "Name" }, { text: "User Role", value: "UserRole" },],
                lstUsers: [],
                lstActivityLog: [],

                lstPeriodicity: [],
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],


                rules: {
                    required: value => !!value || 'Required.',
                },
                periodicityDateMenu: false,

                // SDG Indicators variables

                lstSGD: [],
                lstSDGTargets: [],
                //lstSDGTargetIndicators: [],
                //SearchOutcomeDetails: '',

                // --
                lstSGDData: [],
                lstSDGTargetsData: [],
                //lstSDGTargetIndicatorsData: [],

                SelectedSDG: [],
                SelectedSDGTargets: [],
                //SelectedSDGTargetIndicators: '',

                // Outcome Data Collection variables
                DataCollectionType: 'Non Target',
                DataCollectionTypeItems: ['Quantitative', 'Qualitative'],// ['Target', 'Non Target'], //,//['Target', 'Non Target'],
                SurveyList: [],
                SurveyId: 0,
                SectionList: [],
                SurveySectionId: 0,
                QuestionList: [],
                SurveyQuestionId: 0,
                LogicItems: [],
                SurveyLogic: '',
                logicAnsType: '',
                SurveyTarget: '',
                DefineTarget: '',
                lstLogicAnswers: [],
                //ResponseList: [],

                UpdateSuccesssnackBar: false,
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
                periodicityDate: "",
                ProjectStartDate: "",
                ProjectEndDate: "",
                userimage: "",
                targetData: [],
                targetId: '',
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
                lstRepeatSections : []
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
                return this.formatDate(this.EditOutcome.StartDate)
            },
            ToDate() {
                return this.formatDate(this.EditOutcome.EndDate)
            },
            FormattedDate() {
                return this.formatDate(this.periodicityDate)
            },
            FormattedDateEdit() {
                return this.formatDate(this.EditOutcome.ReportingOnceDate);
            },

            //FormattedDate() {
            //    return objUtils.common.formatDate(this.FrmDate)
            //},
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
                return [{Key:null, Value: 'Main Form'}, ...this.lstRepeatLogic]
            },
        },
        mounted: async function () {
            this.PreLoader = true;
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.Type = this.$route.query.Type;
            this.OutcomeId = this.$route.query.OutcomeId;
            this.InitiativeId = this.$route.query.InitID;

            await importscript();
            await importOutcomesscript();
            await importobjManageProjectscript();
            await this.FetchAllOutcomeListBasedOnOutComeId();

            await this.ChangeTab();
            await this.GetProjectDate();
            await this.fnGetGroupDetailsByprjID();
            await this.fnGetTeamUsersList();


            await this.GetAllSDG();
            await this.FetchAllSDGTargets();
            //await this.FetchAllSDGTargetsIndicators();

            await this.fnGetActivityLog();
            await this.fnGetAllTags();
            this.PreLoader = false;
            await this.fnGetOutputSurveyIds();

        },
        methods: {
            /// <summary>
            ///  storeTargets :- Function used to select impact targets
            /// </summary>
            /// <param name="target"></param>
            /// <param name="index"></param>
            /// <returns> </returns>
            async storeTargets(target, index) {
                var temp1 = this.SelectedSDGTargets.filter((x) => x.Target_Name == target.Target_Name);
                if (temp1.length == 0) {
                    this.SelectedSDGTargets = [...this.SelectedSDGTargets, target];
                    this.AddImpactIndicatorTarget = false;
                } else {
                    if (temp1.length > 0) {
                        this.SelectedSDGTargets.splice(this.SelectedSDGTargets.indexOf(temp1[0]), 1);
                        this.SelectedSDGTargets = [...this.SelectedSDGTargets];
                    }
                }
            },

            /// <summary>
            ///  fnSelectedTargetCountPerImpact :- Function used to show count of selected targets
            /// </summary>
            /// <param name="item"></param>
            /// <returns> Count </returns>
            fnSelectedTargetCountPerImpact(item) {
                var Count = 0;
                this.lstSGDData.forEach((value, index) => {
                    if (item.pk_SDGID == value.pk_SDGID) {
                        this.SelectedSDGTargets.forEach((tar, index) => {
                            if (tar.fk_SDGID == item.pk_SDGID) {
                                Count++;
                            }
                        })
                    }
                });
                return Count;
            },

            /// <summary>
            ///  submitHandler :- Function used to save SDG data
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            submitHandler() {
                if (this.lstSGDData.length == 0) {
                    this.impactError = true;
                } else if (this.lstSGDData.length > 0) {
                    this.impactError = false;
                    this.EditImpactIndicator = false;
                }
            },

            /// <summary>
            ///  submitHandlerTarget :- Function used to save selected SDG targets
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            submitHandlerTarget() {
                var target = this.SelectedSDGTargets.filter(e => e.fk_SDGID == this.targetId);
                if (target.length > 0) {
                    this.addImpactTarget = false;
                    this.AddImpactIndicatorTarget = false;
                } else {
                    this.AddImpactIndicatorTarget = true;
                }
            },

            /// <summary>
            ///  getcolor :- Function used to get color of users
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
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
            ///  ChangeTab :- Function used to change outcome activity tab based on index
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            async ChangeTab(index) {
                var index;
                if (this.Type == 'Edit') {
                    index = 1;
                } else if (this.Type == 'Log') {
                    index = 2;
                }
                if (index == 1) {
                    this.OutcomeActivityTab = 'tab-1';
                }
                if (index == 2) {
                    this.OutcomeActivityTab = 'tab-2';
                }
            },

            /// <summary>
            ///  GetProjectDate :- Function used to fetch project startDate and project endDate
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectDate() {
                var result = await objManageProject.GetProjectDate(this.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    this.ProjectStartDate = result.PrjStartDate;
                    this.ProjectEndDate = result.PrjEndDate;
                }
            },

            /// <summary>
            ///  IsValidDate :- Function used to check outcome start date and outcome end date is valid
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.EditOutcome.EndDate) < Date.parse(this.EditOutcome.StartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.snackbarMsg = true;
                    this.EditOutcome.EndDate = this.Outcomeinfo.EndDate;
                    return;
                }

                if (Date.parse(this.EditOutcome.StartDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.EditOutcome.StartDate = this.Outcomeinfo.StartDate;
                    return;
                }
                if (Date.parse(this.EditOutcome.StartDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.EditOutcome.StartDate = this.Outcomeinfo.StartDate;
                    return;
                }
                if (Date.parse(this.EditOutcome.StartDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.EditOutcome.StartDate = this.Outcomeinfo.StartDate;
                    return;
                }
                if (Date.parse(this.EditOutcome.EndDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.snackbarMsg = true;
                    this.EditOutcome.EndDate = this.Outcomeinfo.EndDate;
                    return;
                }
                if (Date.parse(this.EditOutcome.EndDate) == Date.parse(this.EditOutcome.StartDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                return true;

            },

            /// <summary>
            ///  IsValidPeriodicityDate :- Function used to check outcome periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidPeriodicityDate() {

                if (this.startDate != null) {
                    if (Date.parse(this.EditOutcome.ReportingOnceDate) < Date.parse(this.EditOutcome.StartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.EditOutcome.ReportingOnceDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }

                if (this.endDate != null) {
                    if (Date.parse(this.EditOutcome.ReportingOnceDate) > Date.parse(this.EditOutcome.EndDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.EditOutcome.ReportingOnceDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                return true;

            },

            /// <summary>
            ///  fnEditOutcomeClick :- Function used to get outcome details for edit
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnEditOutcomeClick() {
                this.fnGetWorkflows();
                this.fnGetSurveys();

                this.GetReportingFrequencyList();
                await this.fnGetSavedSDGDetailsForEdit();

                this.EditOutcome.pk_OutcomeID = this.OutcomeInfo.pk_OutcomeID;
                this.EditOutcome.OutcomeName = this.OutcomeInfo.OutcomeName;
                this.EditOutcome.UserList = this.OutcomeInfo.UserList;
                this.EditOutcome.Periodicity = this.OutcomeInfo.Periodicity;
                this.EditOutcome.ReportingMonthDay = this.OutcomeInfo.ReportingMonthDay;
                this.EditOutcome.ReportingMonth = this.OutcomeInfo.ReportingMonth;
                this.EditOutcome.ReportingMonthDateDay = this.OutcomeInfo.ReportingMonthDateDay;
                this.EditOutcome.ReportingWeek = this.OutcomeInfo.ReportingWeek;
                this.EditOutcome.DatacollectionType = this.OutcomeInfo.DatacollectionType;
                this.EditOutcome.SurveyId = this.OutcomeInfo.SurveyId;
                this.EditOutcome.SurveySectionId = this.OutcomeInfo.SurveySectionId;
                this.EditOutcome.SurveyQuestionId = this.OutcomeInfo.SurveyQuestionId;
                this.EditOutcome.ResponseID = this.OutcomeInfo.ResponseID;
                this.EditOutcome.SurveyLogic = this.OutcomeInfo.SurveyLogic;
                this.EditOutcome.DefineTarget = this.OutcomeInfo.DefineTarget;
                this.EditOutcome.SurveyTarget = this.OutcomeInfo.SurveyTarget;
                this.EditOutcome.WorkflowId = this.OutcomeInfo.WorkflowId;

                this.periodicityDate = this.formatDate(this.OutcomeInfo.ReportingOnceDate);
                this.EditOutcome.ReportingOnceDate = this.formatDate(this.OutcomeInfo.ReportingOnceDate)
                this.EditOutcome.StartDate = this.OutcomeInfo.StartDate.substr(0, 10);
                this.EditOutcome.EndDate = this.OutcomeInfo.EndDate.substr(0, 10);


                this.GetSectionsBySurveyID(this.OutcomeInfo.SurveyId);
                this.GetQstnBySectionID(this.OutcomeInfo.SurveySectionId);
                this.GetResponsesByQstnid(this.OutcomeInfo.SurveyQuestionId);
                this.fnGetLogicType(this.OutcomeInfo.DatacollectionType);
            },

            // SDG

            /// <summary>
            ///  GetAllSDG :- Function used to fetch SDG list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllSDG() {
                var result = await objManageOutcomes.GetAllSDG();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstSGD = result.data;
                }
            },

            /// <summary>
            ///  GetTargets :- Function used to fetch targets based on id
            /// </summary>
            /// <param name="id"></param>
            /// <returns> lstTargetsData </returns>
            async GetTargets(id) {
                var lstTargetsData = [];
                var Targets = this.lstSDGTargets.filter(x => x.fk_SDGID == id);

                for (var i = 0; i <= Targets.length - 1; i++) {
                    var object = new Object();
                    object.pk_SDGTargetID = Targets[i].pk_SDGTargetID;
                    object.fk_SDGID = Targets[i].fk_SDGID
                    object.Target_Name = Targets[i].Target_Name
                    object.Status = Targets[i].Status
                    lstTargetsData.push(object);
                }

                return lstTargetsData;
            },

            /// <summary>
            ///  FetchAllSDGTargets :- Function used to fetch all SDG targets
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async FetchAllSDGTargets() {

                var result = await objManageOutcomes.FetchAllSDGTargets();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstSDGTargets = result.data;
                }
            },


            //async FetchAllSDGTargetsIndicators() {
            //    var result = await objManageOutcomes.FetchAllSDGTargetsIndicators();

            //    if (result.status == "Unhandled Exception") {
            //        this.$router.push({ name: "error" });
            //    } else if (result.data) {
            //        this.lstSDGTargetIndicators = result.data;
            //    }
            //},

            /// <summary>
            ///  GetSDGTargetsBasedOnSDGID :- Function used to fetch SDG targets based on SDG id
            /// </summary>
            /// <param name="id"></param>
            /// <param name="items"></param>
            /// <returns> </returns>
            async GetSDGTargetsBasedOnSDGID(id, items) {
                var example = this.SelectedSDG;
                var temp = this.lstSGD.filter(x => x.pk_SDGID == id)
                var temp1 = this.lstSGDData.filter(x => x.pk_SDGID == id)
                if (temp.length > 0 && temp1.length == 0) {
                    for (var SDGCnt = 0; SDGCnt <= temp.length - 1; SDGCnt++) {
                        var object = new Object();
                        object.pk_SDGID = temp[SDGCnt].pk_SDGID;
                        object.SDG_Name = temp[SDGCnt].SDG_Name;
                        object.Status = temp[SDGCnt].Status;
                        object.Targets = await this.GetTargets(object.pk_SDGID)
                        this.lstSGDData.push(object);
                        this.impactError = false;
                    }
                } else {
                    if (temp1.length > 0) {
                        this.lstSGDData.splice(this.lstSGDData.indexOf(temp1[0]), 1);
                        this.SelectedSDGTargets = SelectedSDGTargets.filter(e => e.Target_Name != name)
                        this.lstSGDData = [...this.lstSGDData];
                        this.SelectedSDGTargets = [];
                    }
                }

            },

            // Remove Section of SDG Indicators

            /// <summary>
            ///  fnRemoveSDGIndicators :- Function used to remove SDG indicators
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnRemoveSDGIndicators(item) {
                for (var i = 0; i <= this.lstSGD.length - 1; i++) {
                    if (item == this.lstSGD[i].pk_SDGID) {
                        this.lstSGD[i].Selected = false;
                    }
                }
                this.lstSGDData = this.lstSGDData.filter(v => v.pk_SDGID != item);
            },

            /// <summary>
            ///  fnGetSavedSDGDetailsForEdit :- Function used to fetch saved SDG details for edit
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetSavedSDGDetailsForEdit() {

                var Selected = false;
                var MainList = [];
                this.SelectedSDGTargets = [];
                if (this.lstSGD.length > 0) {
                    for (var i = 0; i <= this.lstSGD.length - 1; i++) {
                        var temp = this.lstSGDData.filter(x => x.pk_SDGID == this.lstSGD[i].pk_SDGID);
                        if (temp.length > 0) {
                            Selected = true;
                        } else {
                            Selected = false;
                        }
                        var object = new Object();
                        object.pk_SDGID = this.lstSGD[i].pk_SDGID;
                        object.SDG_Name = this.lstSGD[i].SDG_Name;
                        object.Selected = Selected;
                        object.Status = this.lstSGD[i].Status;
                        MainList.push(object);
                    }
                }
                this.lstSGD = [];
                this.lstSGD = MainList;


                ///-------------------
                Selected = false;
                var List = [];
                this.SelectedSDG = [];
                if (this.lstSGDData.length > 0) {
                    for (var i = 0; i <= this.lstSGDData.length - 1; i++) {
                        this.SelectedSDG.push(this.lstSGDData[i].pk_SDGID);
                        var object = new Object();
                        object.pk_SDGID = this.lstSGDData[i].pk_SDGID;
                        object.SDG_Name = this.lstSGDData[i].SDG_Name;
                        object.Status = this.lstSGDData[i].Status;
                        object.Targets = await this.fnGetSavedSDGTargetsBasedOnSDG(object.pk_SDGID)
                        object.TargetsCount = await this.GetSDGTargetCount(object.Targets);

                        List.push(object);
                    }
                }
                this.lstSGDData = [];
                this.lstSGDData = List;
            },

            /// <summary>
            ///  fnGetSavedSDGTargetsBasedOnSDG :- Function used to fetch saved SDG targets based on SDG for edit
            /// </summary>
            /// <param name="id"></param>
            /// <returns> lstTargetsData </returns>
            async fnGetSavedSDGTargetsBasedOnSDG(id) {

                var lstTargetsData = [];
                var Targets = this.lstSDGTargets.filter(x => x.fk_SDGID == id);
                var Selected = false;
                if (this.lstSDGTargetsData != null) {
                    for (var i = 0; i <= Targets.length - 1; i++) {
                        var temp = this.lstSDGTargetsData.filter(x => x.pk_SDGTargetID == Targets[i].pk_SDGTargetID);
                        if (temp.length > 0) {
                            Selected = true;
                            this.SelectedSDGTargets.push(temp[0]);
                        }
                        else {
                            Selected = false;
                        }
                        var object = new Object();
                        object.pk_SDGTargetID = Targets[i].pk_SDGTargetID;
                        object.fk_SDGID = Targets[i].fk_SDGID
                        object.Target_Name = Targets[i].Target_Name
                        object.Status = Targets[i].Status
                        object.Selected = Selected;
                        lstTargetsData.push(object);
                    }
                }

                return lstTargetsData;
            },

            /// <summary>
            ///  GetSDGTargetCount :- Function used to get SDG target count
            /// </summary>
            /// <param name="Targets"></param>
            /// <returns> </returns>
            async GetSDGTargetCount(Targets) {
                var SelectedTarget = Targets.filter(x => x.Selected == true);
                return SelectedTarget.length;
            },

            /// <summary>
            ///  FetchAllOutcomeListBasedOnOutComeId :- Function used to fetch all outcome list based on OutcomeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async FetchAllOutcomeListBasedOnOutComeId() {
                var data = new FormData();
                data.append("OutcomeId", this.OutcomeId);
                data.append("ProjectId", this.ProjectId);
                var result = await objManageOutcomes.FetchAllOutcomeListBasedOnOutComeId(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.OutcomeInfo = result.data;
                        this.OutcomeInfo.UserList = this.OutcomeInfo.lstUserList //JSON.parse(this.OutcomeInfo.UserList);
                        this.OutcomeInfo.lstSDGList = JSON.parse(result.data.SDGList);
                        this.OutcomeInfo.lstSDGTargetsList = JSON.parse(result.data.SDGTargetsList);
                        if (this.OutcomeInfo.lstSDGList != null) {
                            this.lstSGDData = this.OutcomeInfo.lstSDGList;
                        }
                        if (this.OutcomeInfo.lstSDGTargetsList != null) {
                            this.lstSDGTargetsData = this.OutcomeInfo.lstSDGTargetsList;
                        }

                        if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                            this.OutcomeInfo.ReportingOnceDate = result.data.ReportingOnceDate;
                        }
                        if (result.data.Tags != null && result.data.Tags != undefined && result.data.Tags != "") {
                            this.SelectedTag = JSON.parse(result.data.Tags);
                        }
                        if (this.OutcomeInfo.UserList == null) {
                            this.OutcomeInfo.UserList = [];
                        }
                        await this.GetSDGTargetAndIndicators()
                    }
            },

            /// <summary>
            ///  GetSDGTargetAndIndicators :- Function used to get SDG target and indicators
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetSDGTargetAndIndicators() {
                if (this.lstSGDData.length > 0) {
                    this.SDGIndicators = [];
                    for (var i = 0; i <= this.lstSGDData.length - 1; i++) {
                        var object = new Object();
                        object.pk_SDGID = this.lstSGDData[i].pk_SDGID;
                        object.SDG_Name = this.lstSGDData[i].SDG_Name;
                        object.Status = this.lstSGDData[i].Status;
                        object.ViewAllPanel = false;
                        if (this.lstSDGTargetsData != null) {
                            object.Targets = await this.GetTargetsWithId(object.pk_SDGID);
                        }
                        else {
                            object.Targets = await this.GetTargetsWithId(0);
                        }
                        this.SDGIndicators.push(object);
                    }
                }
            },

            /// <summary>
            ///  GetTargetsWithId :- Function used to fetch target list based on id
            /// </summary>
            /// <param name="id"></param>
            /// <returns> lstTargetsData </returns>
            async GetTargetsWithId(id) {

                var lstTargetsData = [];
                if (id != 0) {
                    var Targets = this.lstSDGTargetsData.filter(x => x.fk_SDGID == id);
                    for (var i = 0; i <= Targets.length - 1; i++) {
                        var object = new Object();
                        object.pk_SDGTargetID = Targets[i].pk_SDGTargetID;
                        object.fk_SDGID = Targets[i].fk_SDGID;
                        object.Target_Name = Targets[i].Target_Name;
                        object.Status = Targets[i].Status;
                        lstTargetsData.push(object);
                    }
                }
                else {
                    var object = new Object();
                    object.pk_SDGTargetID = 0;
                    object.fk_SDGID = 0;
                    object.Target_Name = "";
                    object.Status = 0;
                    lstTargetsData.push(object);
                }
                return lstTargetsData;
            },

            /// <summary>
            ///  fnGetWorkflows :- Function used to fetch project workflows
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnGetSurveys :- Function used to fetch survey list based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetSurveys() {

                var result = await objManageProjectActivity.GetProjectSurveyList(this.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterPaymentList = result.data;
                    this.SurveyList = result.data;
                }
            },

            /// <summary>
            ///  GetSectionsBySurveyID :- Function used to fetch sections based on SurveyID
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSectionsBySurveyID(SurveyID) {
                var data = new FormData();
                data.append("SurveyID", SurveyID);
                var result = await objManageProjectActivity.GetSectionsBySurveyID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.SectionList = result.data;
                    this.SurveyQuestionId = 0;
                    this.SurveyLogic = 0;
                    this.SurveyTarget = "";
                    this.DefineTarget = 0
                }
            },

            /// <summary>
            ///  GetQstnBySectionID :- Function used to fetch questions based on SectionID
            /// </summary>
            /// <param name="SectionID"></param>
            /// <returns> </returns>
            async GetQstnBySectionID(SectionID) {

                var data = new FormData();
                data.append("SectionID", SectionID);
                var result = await objManageProjectActivity.GetQstnBySectionID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.QuestionList = result.data;
                    if (this.SurveyQuestionId > 0) {
                        this.GetResponsesByQstnid(SurveyQuestionId);
                    }
                    this.SurveyLogic = 0;
                    this.SurveyTarget = "";
                    this.DefineTarget = 0
                }
            },

            /// <summary>
            ///  GetResponsesByQstnid :- Function used to fetch responses based on Qstnid
            /// </summary>
            /// <param name="Qstnid"></param>
            /// <returns> </returns>
            async GetResponsesByQstnid(Qstnid) {
                if (Qstnid > 0) {
                    this.logicAnsType = this.QuestionList.filter(x => x.QuestionID == Qstnid)[0].QuestionType;

                    if (this.logicAnsType == 'Single Choice' || this.logicAnsType == 'Multi-Choice' || this.logicAnsType == 'Dropdown') {
                        var result = await objManageProjectActivity.GetLogicAnswers(Qstnid);
                        if (result.status == "Unhandled Exception")
                            this.$router.push({ name: "error" });
                        else
                            if (result.data) {
                                this.lstLogicAnswers = result.data;
                            }
                    }
                }
            },

            /// <summary>
            ///  fnGetLogicType :- Function used to fetch logic type based on data collection types (Quantitative / Qualitative)
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnGetLogicType(type) {
                if (type == 'Quantitative') {
                    var result = await objManageProjectActivity.GetLogicType();

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.LogicItems = result.data;
                        this.SurveyTarget = "";
                        this.DefineTarget = 0
                    }
                } else {
                    this.LogicItems = [];
                }
                this.SurveyId = 0;
                this.SurveySectionId = 0;
                this.Workflow = 0;
                this.SurveyQuestionId = 0;
                this.SurveyLogic = 0;
                this.SurveyTarget = "";
                this.DefineTarget = 0
            },

            /// <summary>
            ///  fnGetTeamUsersList :- Function used to fetch team users list based on GroupId
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
            ///  fnGetGroupDetailsByprjID :- Function used to fetch group details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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

            /// <summary>
            /// fnGetActivityLog :- Function used to fetch log details (Outcome) based on ProjectId,OutcomeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetActivityLog() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("OutcomeId", this.OutcomeId);
                var result = await objManageProject.GetKPIActivityLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data) {
                    this.lstActivityLog = result.data;
                }
                else {
                    this.Msg = "Failed to get activity details.";
                    this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  GetReportingFrequencyList :- Function used to fetch reporting frequency list (Week, Month, MonthDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnUpdateOutcome :- Function used to update outcome
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnUpdateOutcome() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("pk_OutcomeID", this.EditOutcome.pk_OutcomeID);
                data.append("OutcomeName", this.EditOutcome.OutcomeName);
                data.append("Periodicity", this.EditOutcome.Periodicity);
                // data.append("KPIRating", this.KPIRating);
                data.append("Startdate", this.EditOutcome.StartDate);
                data.append("Enddate", this.EditOutcome.EndDate);

                data.append("SDGList", JSON.stringify(this.lstSGDData))
                data.append("SDGTargetsList", JSON.stringify(this.SelectedSDGTargets))
                data.append("WorkflowId", this.EditOutcome.WorkflowId);

                data.append("DatacollectionType", this.EditOutcome.DatacollectionType)
                data.append("SurveyId", this.EditOutcome.SurveyId)
                data.append("SurveySectionId", this.EditOutcome.SurveySectionId);
                data.append("SurveyQuestionId", this.EditOutcome.SurveyQuestionId);
                data.append("SurveyLogic", this.EditOutcome.SurveyLogic);
                data.append("SurveyTarget", this.EditOutcome.SurveyTarget);

                data.append("ProjectID", this.ProjectId);
                data.append("UserList", JSON.stringify(this.EditOutcome.UserList));
                data.append("Tags", JSON.stringify(this.SelectedTag));
                data.append("ReportingMonth", this.EditOutcome.ReportingMonth);
                data.append("ReportingMonthDay", this.EditOutcome.ReportingMonthDay);
                data.append("ReportingWeek", this.EditOutcome.ReportingWeek);
                data.append("ReportingOnceDate", this.formatDate(this.EditOutcome.ReportingOnceDate));
                data.append("ReportingMonthDateDay", this.EditOutcome.ReportingMonthDateDay);

                var result = await objManageOutcomes.UpdateOutcome(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "Success") {

                        this.UpdateSuccesssnackBar = true;
                        this.IsEditOutCome = false;
                        this.FetchAllOutcomeListBasedOnOutComeId();
                    }
                    else {
                        if (result.status == "Failed") {
                            this.Message = result.RFStatus;
                        } else {
                            this.Message = "Failed";
                        }
                        this.snackbarMsg = true;

                    }
                this.PreLoader = false;
            },

            // --- Tags Sections all Methods
            /// <summary>
            ///  fnGetAllTags :- Function used to fetch project tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnEditTagDetails :- Function used to get edit tag details
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
            ///  UpdateTagDetails :- Function used to update tag details based on color,TagID,TagName
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

            /// <summary>
            ///  CreateNewTag :- Function used to create a new tag based on color, TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnAddToSelectedTag :- Function used to add a tag to the selected tag items
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
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
            ///  CloseTag :- Function used to close tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseTag() {
                this.color = "";
                this.TagHeaderName = 'Add Tag';
                this.NewTagName = "";
                this.NewTagMenu = false;
                this.EditTagName = "";
            },

            /// <summary>
            ///  fnRemoveSelectedUsers :- Function used to remove selected users
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnRemoveSelectedUsers(item) {
                item.IsSelected = false;
                this.EditOutcome.UserList.splice(this.EditOutcome.UserList.indexOf(item), 1);
                this.EditOutcome.UserList = [...this.EditOutcome.UserList];
            },

            /// <summary>
            ///  fnSelectedTargetCount :- Function used to show selected target count
            /// </summary>
            /// <param name="item"></param>
            /// <returns> Count </returns>
            fnSelectedTargetCount(item) {
                var Count = 0;
                if (this.lstSGDData.length > 0) {
                    Count = this.SelectedSDGTargets.filter(x => x.fk_SDGID === item.pk_SDGID).length;
                }
                return Count;
            },
            async fnGetOutputSurveyIds() {
                var result = await objManageOutcomes.GetOutcomeSurveyIds(this.OutcomeId);

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
                data.append("OutcomeId", this.OutcomeId);
                data.append("PresetData", JSON.stringify(config_copy));
                var result = await objManageOutcomes.SaveOutcomePivotPreset(data);

                this.Msg = "Pivot table preset is saved successfully.";
                this.successSnackbarMsg = true;

            },

            async RestorePivotPreset() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("PresetName", this.presetSelected);
                data.append("OutcomeId", this.OutcomeId);
                var result = await objManageOutcomes.RestoreOutcomePivotPreset(data);

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
                data.append("OutcomeId", this.OutcomeId);
                var result = await objManageOutcomes.GetOutcomePivotPresetList(data);
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
                data.append("OutcomeId", this.OutcomeId);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objManageOutcomes.SaveOutcomePivotPreset(data);

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
                data.append("OutcomeId", this.OutcomeId);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objManageOutcomes.EditOutcomePivotPreset(data);

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
                data.append("OutcomeId", this.OutcomeId);
                var result = await objManageOutcomes.DeleteOutcomePivotPreset(data);

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
                this.repeatLogic = null;
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
                if(self.submittedbystartDate && self.submittedbyendDate) {
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
                else if(data.length){
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
                var surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectId.toString() + "/";
                var beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.ProjectId.toString() + "/";

                if (this.pivotdata != null && this.pivotdata != undefined) {
                    for (var i = 0; i < this.pivotdata.length; i++) {
                        for (let key in this.pivotdata[i]) {
                            if (this.pivotdata[i].hasOwnProperty(key)) {
                                var allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar'];
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
            async fnMountPivotTab() {
                this.clearDateSelection();
                await this.GetPivotPresetList();
                await this.GetOutcomeRepeatSection()
                $("#output").pivotUI([], {});
                await this.getpivottable()
                this.lstRepeatSections = this.fnFilterPresets()
            },
            async GetOutcomeRepeatSection(){
                let { data } = await objManageOutcomes.GetOutcomeRepeatSection(this.outputSurveyIds)
                this.lstRepeatLogic = data
            },
            fnFilterPresets() {
                if(this.repeatLogic){
                    return this.presetLocalStorage.filter(z => z.sectionId == this.repeatLogic)
                }

                return this.presetLocalStorage.filter(x => x.sectionId === 0)
            }
        },
        watch: {
            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0) {
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                }
            },

        }
    }
</script>
<style>
    .v-expansion-panel-content__wrap {
        padding: 0 !important;
    }

    .v-expansion-panel-header {
        padding-top: 0 !important;
        padding-bottom: 0 !important;
    }

    .lop {
        height: 250px;
        overflow-x: hidden;
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