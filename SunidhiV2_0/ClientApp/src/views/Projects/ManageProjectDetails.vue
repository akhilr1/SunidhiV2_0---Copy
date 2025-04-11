<template>
    <v-container fluid class="pt-0">
        <v-row class="pt-2">
            <v-col md="12" class="pt-0">
                <v-row>
                    <v-col md="5" class="pt-1">
                        <h5 class="page-head pl-2">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="15" color="primary">mdi-menu</v-icon>
                            </v-avatar>
                            Details
                        </h5>
                    </v-col>
                    <v-spacer></v-spacer>
                    <v-col v-if="editProjectEnableSave != true" md="6" class="mr-10 pt-1" align="center">
                        <!-- Save and Cancel Buttons-->
                        <Save-Button-Success-Small id="AutoTest_IP409" class="mr-2" @click.native="fnUpdateEditProjectDetails()"></Save-Button-Success-Small>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP410" @click.native="editProjectEnableSave=true"></Cancel-Button-Outlined-Sm>
                    </v-col>
                </v-row>

                <v-row v-show="editProjectEnableSave" :justify="justifyCenter" no-gutters>
                    <v-col md="8">
                        <v-row>
                            <v-col md="8">
                                <small>Project Progress</small>
                                <v-progress-linear dark rounded
                                                   background-color="#0A8C45"
                                                   color="#52B962"
                                                   height="15"
                                                   :value=Progress>
                                {{Progress}}%
                                </v-progress-linear>
                            </v-col>
                            <v-spacer></v-spacer>
                            <v-col md="2" class="pt-5 mr-1">
                                <Secondary-Button id="AutoTest_IP397" title="Edit Details" @click.native="fnEditProjectClick()" v-bind:disabled="!screenAccess"></Secondary-Button>
                            </v-col>
                        </v-row>
                    </v-col>
                </v-row>

                <!-- View Project Details-->
                <v-row v-if="editProjectEnableSave == true" :justify="justifyCenter" class="pt-1" style="overflow-x:hidden">
                    <v-col md="8" class="pb-0 v-scrolling-div" style="height:70vh">
                        <v-row  no-gutters>
                            <v-col md="12">
                                <!-- Project Name -->
                                <h4 class="font-weight-regular heading-3-dark">Project Name</h4>
                                <h4 class="font-weight-regular heading-3">{{ projectInfo.ProjectName }}</h4>
                                <!-- Project Description -->
                                <h4 class="font-weight-regular heading-3-dark mt-4">Project Description</h4>
                                <h4 class="font-weight-regular heading-3">
                                    {{ projectInfo.Description }}
                                </h4>
                                <!-- Initiative Name-->
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    Initiative
                                </h4>
                                <h4 class="font-weight-regular heading-3">
                                    {{ initiativeInfo.InitiativeName }}
                                </h4>
                            </v-col>
                        </v-row>

                        <!-- Project Code & Budget -->
                        <v-row  no-gutters>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    Code
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ projectInfo.Code }}</h4>
                            </v-col>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    Budget
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ projectInfo.Budget }}</h4>
                            </v-col>
                        </v-row>

                        <v-row  no-gutters>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    Start Date
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{formatDate(this.projectInfo.StartDate.substr(0, 10)) }}</h4>
                            </v-col>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    End Date
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ formatDate(this.projectInfo.EndDate.substr(0, 10)) }}</h4>
                            </v-col>
                        </v-row>

                        <v-row  no-gutters>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    Country
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ GetCountryName(projectInfo.CountryID) }}</h4>
                            </v-col>
                            <v-col md="6" class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    State
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ GetStateName(projectInfo.StateID) }}</h4>
                            </v-col>
                        </v-row>

                        <v-row  no-gutters>
                            <v-col class="pt-0">
                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                    District
                                </h4>
                                <h4 class="font-weight-regular heading-3">{{ GetDistrictName(projectInfo.DistrictID) }}</h4>
                            </v-col>
                        </v-row>

                        <!-- Tag Section modified-->
                        <v-row  no-gutters>
                            <v-col md="12" class="pb-0 pt-0">
                                <v-row class="mb-1">
                                    <v-col class="text-left d-inline-flex" md="12">
                                        <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                        <!--<v-menu  v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                            <template v-slot:activator="{ on }">
                                                <v-btn id="AutoTest_IP398" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                       @click="fnEditProjectClick(),fnInitializeSteper()" class="mr-1" v-bind:disabled="!screenAccess">
                                                    <v-icon size="15">mdi-plus</v-icon>
                                                </v-btn>
                                            </template>
                                        </v-menu>-->
                                        <div v-if="!allTags">
                                            <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                            <v-chip v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[0].TagName}}</v-chip>
                                            <v-chip v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[1].TagName}}</v-chip>
                                            <v-chip v-if="SelectedTag.length>2" label small class="tags tag-count mr-1" @click="allTags=true">+{{SelectedTag.length-2}}<span class="px-1">more</span></v-chip>
                                        </div>
                                        <!--<v-btn id="AutoTest_IP406" max-width="15" min-width="15" height="25" outlined dark color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
        <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
    </v-btn>-->
                                        <v-chip v-if="allTags == true" label small class="tags mx-1" @click="allTags=false">Show less</v-chip>
                                        <!--<v-btn id="AutoTest_IP407" max-width="15" min-width="15" height="25" outlined dark color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                        </v-btn>-->
                                    </v-col>
                                </v-row>
                                <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0 mb-4">
                                    <v-col md="12" class="mb-0 pt-0">
                                        <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                            <v-chip id="AutoTest_IP408" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0" v-bind:disabled="!screenAccess">
                                                {{TagItem.TagName}}
                                            </v-chip>
                                        </v-chip-group>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>

                    </v-col>
                </v-row>

                <!-- Edit Project Details-->
                <v-row v-if="editProjectEnableSave != true" :justify="justifyCenter" class="pt-2">
                    <v-col md="8" class="v-scrolling-div" style="height:70vh">
                        <v-row>
                            <v-col>
                                <v-text-field outlined dense label="Project Name" type="text" v-model="editProjectInfo.ProjectName"></v-text-field>
                                <v-textarea rows="3" outlined dense label="Project Description" type="text" hide-details v-model="editProjectInfo.Description"></v-textarea>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field outlined dense label="Code" v-bind:disabled="true" hide-details v-model="editProjectInfo.Code"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field outlined dense label="Budget" type="number" class="text-right" hide-details v-model="editProjectInfo.Budget"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0"></v-text-field>
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
                                        <v-text-field v-model="editStartDate"
                                                      label="Start Date"
                                                      persistent-hint outlined hide-details
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="FrmDate" color="primary" no-title @input="FrmDateMenu = false,endDate = startDate" dense></v-date-picker>
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
                                        <v-text-field v-model="editEndDate"
                                                      label="Date"
                                                      persistent-hint outlined hide-details
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="ToDate" color="primary" no-title @input="ToDateMenu = false" dense :min="FrmDate"></v-date-picker>
                                </v-menu>
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-col>
                                <v-select :items="CountryList" :item-text="i => i.CountryName" :item-value="i => i.CountryID" v-model="editCountry"
                                          label="Country" outlined solo dense flat
                                          return-object
                                          required
                                          class="mb-0"
                                          @change="fnCountryChange()" hide-details></v-select>
                            </v-col>
                            <v-col>
                                <v-select :items="filterStatesList" :item-text="i => i.StateName" :item-value="i => i.StateID" v-model="editState"
                                          label="State" outlined solo dense flat
                                          return-object
                                          required
                                          class="mb-0"
                                          @change="fnStateChange()" hide-details></v-select>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-select :items="filterDistrictsList" :item-text="i => i.DistrictName" :item-value="i => i.DistrictID" v-model="editDistrict"
                                          label="District"
                                          outlined
                                          solo
                                          dense
                                          flat
                                          return-object
                                          required
                                          @change="" hide-details></v-select>
                            </v-col>
                        </v-row>

                        <!-- Tag Section modified-->
                        <v-row class="mb-1">
                            <v-col class="text-left d-inline-flex" md="12">
                                <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP73" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                               @click="fnInitializeSteper()" class="mr-1">
                                            <v-icon size="15">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-card class="elevation-1">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <!-- Header Text  -->
                                                <v-row class="pt-1 px-1">
                                                    <v-col cols="12" md="12" class="d-inline-flex">
                                                        <v-btn id="AutoTest_IP74" text @click="AllTagListSteps = 2 ,TagHeaderName ='Search Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                            <v-icon size="15">mdi-chevron-left</v-icon>
                                                        </v-btn>
                                                        <v-spacer></v-spacer>
                                                        <h3 class="text-left page-head pt-2">
                                                            {{TagHeaderName}}
                                                        </h3>
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP75" text @click="NewTagMenu = false">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-col>
                                                </v-row>
                                                <v-divider></v-divider>
                                                <v-stepper v-model="AllTagListSteps" class="elevation-0 px-4">
                                                    <v-stepper-items>
                                                        <!-- Create New Tag Option when there is no Tags are -->
                                                        <v-stepper-content step="1" class="pa-0">
                                                            <v-row>
                                                                <v-col class="pb-1 ml-2">
                                                                    <v-text-field outlined type="text" dense
                                                                                  v-model="NewTagName" hide-details
                                                                                  label="Enter new tag">
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
                                                            <v-row v-show="NewTagName.length>0">
                                                                <v-col cols="12" md="12" class="text-center">
                                                                    <v-btn id="AutoTest_IP76" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                </v-col>
                                                            </v-row>
                                                        </v-stepper-content>
                                                        <!-- Select from existing Tags-->
                                                        <v-stepper-content step="2" class="pa-0">

                                                            <v-row style="max-height:40vh">
                                                                <v-col class="pb-1">
                                                                    <!--<p class="mb-2">Search Tags</p>-->
                                                                    <v-text-field outlined="" dense="" label="Search tag here" hide-details class="mb-3" v-model="SearchTagDetails">
                                                                    </v-text-field>
                                                                    <div style="height: 18vh;" class="v-scrolling-div" v-show="this.filterAllTagList.length>0">
                                                                        <v-chip-group dark multiple column style="max-width: 220px">
                                                                            <div v-for="index in filterAllTagList" :key="index.TagID" class="d-inline-flex">
                                                                                <v-chip id="AutoTest_IP77" style="width: 180px" active-class="active-success-check" :key="index.TagID" :color="index.TagColor" medium
                                                                                        label @click.native="fnAddToSelectedTag(index),NewTagMenu = true">
                                                                                    <!--close @click:close="fnRemoveFromAllTagList(index)"-->
                                                                                    <v-icon size="20" left :color="index.TagColor" :key="index.TagID">mdi-check</v-icon>
                                                                                    <span class="text-limit text-center">{{ index.TagName }}</span>
                                                                                </v-chip>
                                                                                <v-icon id="AutoTest_IP78" right color="primary" size="16" @click="fnEditTagDetails(index)" class="mx-2">mdi-pencil-outline</v-icon>
                                                                            </div>
                                                                        </v-chip-group>
                                                                    </div>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row>
                                                                <v-col cols="12" md="12" class="text-center mt-3">
                                                                    <v-btn id="AutoTest_IP79" depressed color="success-btn" @click="searchTag()" dark class="text-limit" max-width="200">Create New <span v-if="this.SearchTagDetails.length>0" class="pl-1">'{{SearchTagDetails}}'</span></v-btn>
                                                                </v-col>
                                                            </v-row>
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
                                                                    <span class="ml-3">Edit Color</span>
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
                                                                <v-col cols="6" md="6" class="text-center">
                                                                    <v-btn id="AutoTest_IP80" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                </v-col>
                                                                <v-col cols="6" md="6" class="text-center">
                                                                    <v-btn id="" depressed color="red" dark @click="fnRemoveFromAllTagList()" v-bind:disabled="EditTagName.length== 0">Delete</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-stepper-content>
                                                    </v-stepper-items>
                                                </v-stepper>
                                            </v-col>
                                        </v-row>
                                    </v-card>
                                </v-menu>
                                <div v-if="!allTags">
                                    <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                    <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[0].TagName}}</v-chip>
                                    <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[1].TagName}}</v-chip>
                                    <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1" @click="allTags=true">+{{SelectedTag.length-2}}<span class="px-1">more</span></v-chip>
                                </div>
                                <v-chip v-if="allTags == true" label small class="tags mx-1" @click="allTags=false">Show less</v-chip>
                                <!--<v-btn id="AutoTest_IP81" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
            </v-btn>
            <v-btn id="AutoTest_IP82" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
            </v-btn>-->
                            </v-col>
                        </v-row>
                        <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0 mb-4">
                            <v-col md="12" class="mb-0 pt-0">
                                <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                    <v-chip id="AutoTest_IP83" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                        {{TagItem.TagName}}
                                    </v-chip>
                                </v-chip-group>
                            </v-col>
                        </v-row>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
        
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
</template>
<script>
    import AddTagButton from '@/components/add-tag-button.vue'
    import SaveButtonSuccessSmall from '@/components/save-button-success-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import SecondaryButton from'@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '../../utils.js';

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
                justifyCenter: 'center',
                //step 2 showing the project details
                WorkspaceID: 0,
                Type:'Initiative',
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: ""
                },
                editProjectEnableSave: true,
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
                    ProjectTagsList: [],
                },

                CountryList: [],
                StatesList: [],
                DistrictsList: [],
                filterStatesList: [],
                filterDistrictsList: [],
                editProjectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    ProjectTagsList: [],
                },
                editCountry: {
                    CountryID: 0,
                    CountryName: ""
                },
                editState: {
                    StateID: 0,
                    StateName: ""
                },
                editDistrict: {
                    DistrictID: 0,
                    DistrictName: ""
                },
                //Description: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Description:"Description",
                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",

                allTags: false,

                TagList: [],

           // ToDate
                FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                ToDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                // Altered Tag Menu
                AllTagListSteps: 0,
                //NewTagListSteps: 0,
                TagHeaderName: "Search Tag",
                NewTagMenu: false,
                AllTagList: [],
                NewTagName: "",
                swatches: [
                    ['#F98B00', '#00B448'],
                    ['#399CFF', '#D10000'],
                    ['#2C4258', '#00C2B0'],
                    ['#E6217B', '#6F00B0'],
                    ['#F1C100']

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                filterAllTagList: [],
                SearchTagDetails: "",
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                attr: "",
                selected: "",
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                screenAccess :0,
                Progress:0,
                SelectedTagID :0,
            };
        },
        components: {
            'Add-Tag-Button': AddTagButton,
            'Save-Button-Success-Small': SaveButtonSuccessSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Secondary-Button':SecondaryButton,
            'Default-Button-Outlined':DefaultButtonOutlined,
        },
        computed: {

            editStartDate() {
                return this.formatDate(this.FrmDate)
            },
            editEndDate() {
                return this.formatDate(this.ToDate)
            },
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
             this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.Type = this.$route.query.Type;
            await importscript();
            await this.fnGetCountryStatesDistricts();
            await this.fnGetProjectDetailsByID();
            await this.fnGetAllTags();

        },
        methods: {

            /// <summary>
            ///  fnGetCountryStatesDistricts :- Function used to fetch details of Country, States and Districts
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetCountryStatesDistricts() {
                var result = await objManageProject.GetCountryStatesDistricts();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.CountryList = result.data.countries;
                    this.StatesList = result.data.states;
                    this.DistrictsList = result.data.districts;
                    this.filterStatesList = result.data.states;
                    this.filterDistrictsList = result.data.districts;
                } else {
                    this.Msg = "Failed to get details of States and Districts";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    this.Progress=this.projectInfo.Progress;
                    this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
                    if(this.SelectedTag == null){
                            this.SelectedTag =[];
                        }
                    //alert("Tags length =" + this.projectInfo.ProjectTagsList.length);
                } else {
                    this.Msg = "Failed to get details of Project " + this.projectInfo.ProjectId;
                    this.errorSnackbarMsg = true;
                }
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
            ///  GetCountryName :- Function used to fetch country name based on countryid
            /// </summary>
            /// <param name="countryid"></param>
            /// <returns> </returns>
            GetCountryName(countryid) {
                var temp = this.CountryList.filter(x => x.CountryID == countryid);
                if (temp.length > 0) {
                    return temp[0].CountryName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            ///  GetStateName :- Function used to fetch state name based on stateid
            /// </summary>
            /// <param name="stateid"></param>
            /// <returns> </returns>
            GetStateName(stateid) {
                var temp = this.StatesList.filter(x => x.StateID == stateid);
                if (temp.length > 0) {
                    return temp[0].StateName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            ///  GetDistrictName :- Function used to fetch district name based on districtid
            /// </summary>
            /// <param name="districtid"></param>
            /// <returns> </returns>
            GetDistrictName(districtid) {
                var temp = this.DistrictsList.filter(x => x.DistrictID == districtid);
                if (temp.length > 0) {
                    return temp[0].DistrictName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            ///  fnEditProjectClick :- Function used to fetch project details when click on edit button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditProjectClick() {
                this.editProjectEnableSave = false;
                this.editProjectInfo.ProjectId = this.projectInfo.ProjectId;
                this.editProjectInfo.ProjectName = this.projectInfo.ProjectName;
                this.editProjectInfo.Description = this.projectInfo.Description;
                this.editProjectInfo.Code = this.projectInfo.Code;
                this.editProjectInfo.Budget = this.projectInfo.Budget;
                this.editProjectInfo.ProjectTagsList = this.projectInfo.ProjectTagsList;

                this.FrmDate = this.projectInfo.StartDate.substr(0, 10);
                this.ToDate = this.projectInfo.EndDate.substr(0, 10);

                this.editCountry.CountryID = this.projectInfo.CountryID;
                this.editState.StateID = this.projectInfo.StateID;
                if (this.editState.StateID) this.filterStatesList = this.StatesList.filter(x => x.CountryID == this.editCountry.CountryID);
                this.editDistrict.DistrictID = this.projectInfo.DistrictID;
                if (this.editDistrict.DistrictID) this.filterDistrictsList = this.DistrictsList.filter(x => x.StateID == this.editState.StateID);
            },

            /// <summary>
            ///  fnCountryChange :- Function used to change country 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCountryChange() {
                this.editState.StateID = 0; this.editState.StateName = "";
                this.editDistrict.DistrictID = 0; this.editDistrict.DistrictName = "";
                this.filterStatesList = this.StatesList.filter(x => x.CountryID == this.editCountry.CountryID);
                this.filterDistrictsList = [];
            },

            /// <summary>
            ///  fnStateChange :- Function used to change state
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnStateChange() {
                this.editDistrict.DistrictID = 0; this.editDistrict.DistrictName = "";
                this.filterDistrictsList = this.DistrictsList.filter(x => x.StateID == this.editState.StateID);
            },

            /// <summary>
            ///  fnUpdateEditProjectDetails :- Function used to update the details of project
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnUpdateEditProjectDetails() {
                this.editProjectEnableSave = true;
                var data = new FormData();
                data.append("ProjectId", this.editProjectInfo.ProjectId);
                data.append("ProjectName", this.editProjectInfo.ProjectName);
                data.append("Description", this.editProjectInfo.Description);
                data.append("Code", this.editProjectInfo.Code);
                data.append("Budget", this.editProjectInfo.Budget);
                data.append("StartDate", this.FrmDate);
                data.append("EndDate", this.ToDate);
                data.append("CountryID", this.editCountry.CountryID);
                data.append("StateID", this.editState.StateID);
                data.append("DistrictID", this.editDistrict.DistrictID);
                data.append("ProjectTags", JSON.stringify(this.SelectedTag));

                var result = await objManageProject.UpdateEditProjectDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = "Successfully updated the details of Project";
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = "Failed to update the details of Project";
                    this.errorSnackbarMsg = true;
                }


                //re-initialize the edit fileds
                this.fnGetCountryStatesDistricts();
                this.fnGetProjectDetailsByID();
                this.editProjectInfo.ProjectId = 0;
                this.editProjectInfo.ProjectName = "";
                this.editProjectInfo.Description = "";
                this.editProjectInfo.Code = "";
                this.editProjectInfo.Budget = 0;
                this.editCountry.CountryID = 0;
                this.editState.StateID = 0;
                this.editDistrict.DistrictID = 0;
                this.NewTagName = "";
                this.SearchTagDetails = "";
            },

            //Tags

            /// <summary>
            ///  fnGetAllTags :- Function used to fetch all project tag list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objManageProject.GetAllProjectTags(this.WorkspaceID);
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
            ///  searchTag :- Function used to create a new tag if not exist while searching
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async searchTag() {
                this.AllTagListSteps = 1;
                this.TagHeaderName = 'Add New Tag';
                this.NewTagName = this.SearchTagDetails;
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
                this.SelectedTagID = TagItem.TagID;
            },

            /// <summary>
            ///  UpdateTagDetails :- Function used to update tag details after edit
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
                        this.AllTagListSteps = 2;
                        this.TagHeaderName = 'Search Tag';
                        this.fnGetAllTags()
                        this.UpdatingTagItem = "";
                    }
                    else {
                        this.Msg = "Tag already exists";
                        this.infoSnackbarMsg = true;
                    }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create new tag 
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
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.NewTagName = "";
                        this.SearchTagDetails = "";
                        this.AllTagListSteps = 2;
                        this.TagHeaderName = 'Search Tag';
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
            ///  fnRemoveFromAllTagList :- Function used to delete project tag from all tag list based on TagID
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            async fnRemoveFromAllTagList(index) {
                var data = new FormData();
                data.append("TagID", this.SelectedTagID);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.DeleteProjectTag(data);
                console.log(result);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == true) {
                        this.NewTagName = "";
                        this.SearchTagDetails = "";
                        this.AllTagListSteps = 2;
                        this.TagHeaderName = 'Search Tag';
                        this.SelectedTagID  = 0,
                        this.Msg = "Tag deleted successfully";
                        this.infoSnackbarMsg = true;
                        this.fnGetAllTags();

                    }
                    else
                    {
                        this.Msg = "Tag already used by another project";
                        this.infoSnackbarMsg = true;
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
        },
        watch: {
            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
            },

        }
    };
</script>