<template>
    <v-card>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid class="px-0">
            <v-row class="px-4">
                <v-col>
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary">mdi-file-document-box-outline</v-icon>
                        </v-avatar>
                        Draft Project
                    </h3>
                </v-col>
                <v-col>
                    <v-row v-if="ProjectTableItems.length>0">
                        <v-spacer></v-spacer>
                     <!-- <v-btn @click="CreateNewProjectDialog=true" height="39" class="mr-4">New</v-btn>-->
                        <!--Create Project Button-->
                        <v-btn id="AutoTest_IP67" height="39" color="success-btn" dark class="elevation-0 mr-4" @click="CreateNewProjectDialog=true,GetProjectListByInitID()" v-bind:disabled ="!screenAccess">Create Project</v-btn>
                        <!--Bulk Action -->
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                                    Bulk Actions
                                    <v-icon>
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">

                                <v-list-item id="AutoTest_IP68" @click.stop="fnBulkRemoveClick()">
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
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Project
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP69" text="" @click="RemoveBulkProjectDailog=false">
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
                                                                <h4 class="text-center heading-3">Are you sure you want to remove this projects ?</h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                               <Secondary-Button id="AutoTest_IP70" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(1)"></Secondary-Button>
                                                              <Default-Button-Outlined id="AutoTest_IP71" class="btn-122x36" title="Cancel" @click.native="RemoveBulkProjectDailog=false"></Default-Button-Outlined>
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
                                          dense prepend-inner-icon="mdi-magnify" v-model="searchProject"
                                          outlined hide-details></v-text-field>
                    </v-row>
                </v-col>
            </v-row>
            <v-dialog v-model="CreateProjectDialog" persistent width="800">
                <v-card>
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-file-document-box-outline</v-icon>
                        </v-avatar>Create Project
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP72" text @click="CloseProjectDialog()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="py-0">
                        <v-container fluid class="pt-0">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-stepper v-model="NewProjectSteps" class="elevation-0">
                                        <v-row :justify="justifyCenter">
                                            <v-col md="6">
                                                <v-stepper-header class="elevation-0">
                                                    <v-stepper-step :complete="NewProjectSteps > 1" step="1" color="inherit">Add Details</v-stepper-step>
                                                    <v-divider></v-divider>
                                                    <v-stepper-step :complete="NewProjectSteps > 2" step="2" color="inherit">Project Template</v-stepper-step>
                                                </v-stepper-header>
                                            </v-col>
                                        </v-row>
                                        <v-stepper-items>
                                            <!-- step 1 Add Details-->
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row no-gutters>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <h4 class="text-center heading-3">Step 1: Please enter the basic details of the new Project</h4>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                            <v-row :justify="justifyCenter" style="height:50vh;" class="v-scrolling-div">
                                                                <v-col md="7">
                                                                    <v-row>
                                                                        <v-col>
                                                                            <v-text-field outlined dense label="Project Name" type="text" v-model="prjName" :rules="alphanumericRules">
                                                                            </v-text-field>
                                                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="prjDescription">
                                                                            </v-textarea>
                                                                            <v-text-field outlined dense label="Code" v-model="prjCode" v-bind:disabled="true">
                                                                            </v-text-field>
                                                                            <v-text-field outlined dense label="Budget" type="number" v-model="prjBudget" hide-details
                                                                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                                                                            </v-text-field>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row>
                                                                        <v-col md="6">
                                                                            <v-menu v-model="mStartDate"
                                                                                    :close-on-content-click="false"
                                                                                    :nudge-right="40"
                                                                                    transition="scale-transition"
                                                                                    offset-y
                                                                                    min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field v-model="computedStartDate"
                                                                                                  label="Start Date"
                                                                                                  outlined dense
                                                                                                  readonly
                                                                                                  v-on="on" hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model="startDate" no-title @input="mStartDate = false,IsValidDate()" format="DD-MM-YYYY"></v-date-picker>
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
                                                                                    <v-text-field v-model="computedEndDate"
                                                                                                  label="End Date"
                                                                                                  outlined dense
                                                                                                  readonly
                                                                                                  v-on="on" hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model="endDate" no-title @input="mEndDate = false,IsValidDate()"></v-date-picker>
                                                                            </v-menu>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row>
                                                                        <v-col>
                                                                            <v-select :items="countryList"
                                                                                      label="Select Country"
                                                                                      item-text="CountryName"
                                                                                      item-value="CountryID"
                                                                                      outlined
                                                                                      dense
                                                                                      @change="GetStateDetails(Country)"
                                                                                      v-model="Country" hide-details></v-select>
                                                                        </v-col>

                                                                    </v-row>
                                                                    <v-row>
                                                                        <v-col>
                                                                            <v-select :items="stateList"
                                                                                      label="State"
                                                                                      item-text="StateName"
                                                                                      item-value="StateID"
                                                                                      outlined
                                                                                      dense
                                                                                      @change="GetDistrictDetails(State)"
                                                                                      v-model="State" hide-details></v-select>
                                                                        </v-col>
                                                                        <v-col>
                                                                            <v-select :items="districtList"
                                                                                      label="District"
                                                                                      item-text="DistrictName"
                                                                                      item-value="DistrictID"
                                                                                      outlined
                                                                                      dense
                                                                                      v-model="District" hide-details></v-select>

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
                                                                                <v-card>
                                                                                    <v-card-text class="pt-0 pb-1">
                                                                                        <v-col cols="12" md="12">
                                                                                            <!-- Header Text  -->
                                                                                            <v-col cols="12" md="12" class="d-inline-flex">
                                                                                                <v-btn id="AutoTest_IP74" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
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
                                                                                            <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                                                                <v-stepper-items>

                                                                                                    <!-- Create New Tag Option when there is no Tags are -->
                                                                                                    <v-stepper-content step="1" class="pa-0">
                                                                                                        <v-row>
                                                                                                            <v-col class="pb-1 ml-2">
                                                                                                                <v-text-field outlined type="text" dense
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
                                                                                                                <v-btn id="AutoTest_IP76" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>

                                                                                                    <!-- Select from existing Tags-->
                                                                                                    <v-stepper-content step="2" class="pa-0">

                                                                                                        <v-row style="max-height:30vh;" class="v-scrolling-div">
                                                                                                            <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                                                <p class="mb-2">Search Tags</p>
                                                                                                                <v-text-field outlined="" dense="" label="Search tag here" hide-details class="mb-3" v-model="SearchTagDetails">
                                                                                                                </v-text-field>
                                                                                                                <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                                                                                                                    <v-chip id="AutoTest_IP77" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                                            label @click.native="fnAddToSelectedTag(index)">
                                                                                                                        {{ index.TagName }}
                                                                                                                        <v-icon id="AutoTest_IP78" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                                                    </v-chip>
                                                                                                                </v-chip-group>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-col cols="12" md="12" class="text-center mt-3">
                                                                                                            <v-btn id="AutoTest_IP79" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
                                                                                                        </v-col>
                                                                                                    </v-stepper-content>

                                                                                                    <!-- Edit Tag-->
                                                                                                    <v-stepper-content step="3" class="pa-0">
                                                                                                        <v-row>
                                                                                                            <v-col class="pb-1 ml-2">
                                                                                                                <v-text-field outlined type="text" dense
                                                                                                                     label="Name"         v-model="EditTagName" hide-details>
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
                                                                                                                <v-btn id="AutoTest_IP80" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                                            <v-btn id="AutoTest_IP81" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                                                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                                            </v-btn>
                                                                            <v-btn id="AutoTest_IP82" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                                                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                                            </v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                                        <v-col md="12" class="chip-container mb-0 pt-0">
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
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-4">
                                                          <Secondary-Button id="AutoTest_IP84" class="btn-122x36 mr-4" title="Next" @click.native="NewProjectSteps = 2" v-bind:disabled="prjName.length == 0 || prjName.trim() == '' || startDate =='' || endDate== '' "></Secondary-Button>
                                                          <Default-Button-Outlined id="AutoTest_IP85" class="btn-122x36" title="Cancel" @click.native="CloseProjectDialog()"></Default-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-stepper-content>
                                            <!-- Step 2 Project Template-->
                                            <!-- Project Theme-->
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
                                                        <h4 class="text-center heading-3">Step 2:  Please select a theme to see tha available template under it.</h4>
                                                    </v-col>
                                                    <v-col md="12" class="py-0">

                                                    </v-col>
                                                    <v-row>
                                                        <v-col xs="12" sm="12" md="12">
                                                            <v-row style="max-height:30vh;" class="v-scrolling-div mx-4">
                                                                <v-card v-for="(item,idx) in prjThemeItems" :key="idx" width="150px" class="ma-2" style="float:left">
                                                                    <v-row no-gutters>
                                                                        <!-- Notification-->
                                                                        <v-col md="12" class="pb-0">
                                                                            <v-avatar color="#52B962" size="25" style="margin-top:5px" class="ma-2">
                                                                                <span class="white--text">{{item.NoOfTemplates}}</span>
                                                                            </v-avatar>
                                                                            <v-checkbox @change="SelectThemes(item)" v-model="item.IsSelected" color="#52B962" style="float:right;margin-top:5px"
                                                                                        label=""></v-checkbox>
                                                                        </v-col>
                                                                        <!-- Button-->
                                                                        <v-col md="12" class="text-center pb-0 pt-0">
                                                                            <v-btn id="AutoTest_IP86" height="80" @click="GetProjectTemplatesList(item.ThemeID,item.NoOfTemplates)" text>
                                                                                <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="80" width="80"></v-img>
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

                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4">
                                                      <Secondary-Button id="AutoTest_IP87" class="btn-122x36 mr-4" title="Next" @click.native="NewProjectSteps = 3" v-bind:disabled="!ThemeSelected"></Secondary-Button>
                                                      <Default-Button-Outlined id="AutoTest_IP88" class="btn-122x36" title="Previous" @click.native="NewProjectSteps = 1"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper-content>

                                            <!-- Project Template-->
                                            <v-stepper-content step="3" class="pa-0">
                                                <v-row>
                                                    <v-col>
                                                        <h3 class="text-left page-head">
                                                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                <v-icon size="20" color="primary">mdi-folder-key-network-outline</v-icon>
                                                            </v-avatar> Theme <v-icon size="20">mdi-chevron-right</v-icon> {{ ThemeName }}
                                                        </h3>
                                                    </v-col>
                                                    <!-- Step Text-->
                                                    <v-col cols="12" md="12" class="pt-0 mb-1">
                                                        <h4 class="text-center heading-3">Step 2: Please select a template from below to add the project in the initiative</h4>
                                                    </v-col>

                                                    <!-- Project Template Card-->
                                                    <v-col md="12">
                                                        <v-row class="v-scrolling-div mx-4" style="height:35vh;overflow-x:hidden" v-if="TemplateItems.length>0 ">
                                                            <v-col xs="6" sm="6" md="6" v-for="(item,idx) in TemplateItems" :key="idx">
                                                                <v-card class="workspace-card" flat>
                                                                    <v-row no-gutters>
                                                                        <v-col md="12" class="pb-0">
                                                                            <v-img style="height:150px"
                                                                                   src="https://cdn.vuetifyjs.com/images/cards/docks.jpg">

                                                                                <v-checkbox id="AutoTest_IP89" color="success-check-box" @click="SelectTemplates(item.ProjectTemplateID,item.IsSelected)" v-model="item.IsSelected" label="" style="float:right;margin-top:10px"></v-checkbox>

                                                                            </v-img>
                                                                            <v-col md="12" style="margin-top:-30px" class="mb-0">
                                                                                <v-btn class="ml-2" large color="#FFFFFF">
                                                                                    <v-icon size="40px" color="#2196F3">mdi-folder-open</v-icon>
                                                                                </v-btn>
                                                                            </v-col>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters>
                                                                        <v-col cols="12" md="12" class="pt-0">
                                                                            <h3 class="heading-1 pt-2 ml-3">  {{ item.TemplateName }}</h3>
                                                                            <p class="pb-0 ml-3" size="8">Created by ImWorks Team</p>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters>
                                                                        <v-col class="ml-0" style="overflow-y:auto ;height:120px">
                                                                            <p>{{ item.Description }}</p>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters>
                                                                        <v-col md="6" class="text-center pt-1">
                                                                            <v-btn id="AutoTest_IP90" color="success-btn elevation-0 enter-btn" block dark @click="NewProjectSteps = 4,ViewTemplate(item.ProjectTemplateID)">
                                                                                View Template
                                                                            </v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-card>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4">
                                                       <Secondary-Button id="AutoTest_IP91" class="btn-122x36 mr-4" title="Next" @click.native="fnCreateProject()" v-bind:disabled="!prjTemplateSelected"></Secondary-Button>
                                                      <Default-Button-Outlined id="AutoTest_IP92" class="btn-122x36" title="Previous" @click.native="NewProjectSteps = 2"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper-content>
                                            <!-- View Project Template -->
                                            <v-stepper-content step="4" class="pa-0">
                                                <v-card class="px-4">
                                                    <v-card-text class="pt-0">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" xs="12" md="12">
                                                                <v-row :justify="justifyCenter">
                                                                    <v-col cols="12" md="12">
                                                                        <v-col cols="12" md="12">
                                                                            <h4 class="text-left heading-1">
                                                                                <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                                                    <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                </v-avatar> Theme > {{ ThemeName }} >{{TemplateName }}
                                                                            </h4>
                                                                        </v-col>
                                                                        <v-container fluid style="height:30vh" class="v-scrolling-div">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-0">
                                                                                <h4 class="text-center heading-3">Step 3:  Please view the template.</h4>
                                                                            </v-col>
                                                                            <!-- Heading with enter Button-->
                                                                            <v-list>
                                                                                <v-list-item class="pb-2">
                                                                                    <v-btn id="AutoTest_IP93" text width="auto" height="auto" @click="">
                                                                                        <v-avatar size="50" tile class="elevation-1 base-border-radius mr-2">
                                                                                            <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                                                        </v-avatar>

                                                                                        <p class="text-left mt-2">
                                                                                            <br />{{TemplateName }}<br />
                                                                                            <span size="12"> Created by ImWorks Team </span>
                                                                                        </p>
                                                                                    </v-btn>
                                                                                    <v-spacer></v-spacer>
                                                                                    <p class="mt-4">
                                                                                        <v-btn id="AutoTest_IP94" color="success-btn elevation-0 enter-btn" dark @click="">
                                                                                            View Template
                                                                                        </v-btn>
                                                                                    </p>
                                                                                </v-list-item>

                                                                            </v-list>
                                                                            <v-col>
                                                                                <p class="title pt-0 pb-0 text-limit">About Template </p>
                                                                                <p class="text-left heading-3">{{TemplateDescription }}</p>
                                                                            </v-col>
                                                                            <v-col class="title pt-1 pb-1 text-limit">
                                                                                Use Cases
                                                                                <v-list style="overflow-y :scroll">
                                                                                    <v-list-item-group v-model="item" color="primary">
                                                                                        <v-list-item v-for="(item,i) in TemplateUsecases" :key="i" dense>
                                                                                            <v-list-item-icon>
                                                                                                <v-icon>mdi-star</v-icon>
                                                                                            </v-list-item-icon>
                                                                                            <v-list-item-content>
                                                                                                <v-list-item-title v-text="item.Usecase"></v-list-item-title>
                                                                                            </v-list-item-content>
                                                                                        </v-list-item>
                                                                                    </v-list-item-group>
                                                                                </v-list>
                                                                            </v-col>
                                                                        </v-container>
                                                                    </v-col>

                                                                    <v-col cols="12" md="12">
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                           <Secondary-Button id="AutoTest_IP95" class="btn-122x36 mr-4" title="Next" @click.native="NewProjectSteps=3"></Secondary-Button>
                                                                           <Default-Button-Outlined id="AutoTest_IP96" class="btn-122x36" title="Previous" @click.native="NewProjectSteps=3"></Default-Button-Outlined>
                                                                        </v-col>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                </v-card>
                                            </v-stepper-content>

                                            <!-- Final Message -->
                                            <v-stepper-content step="5" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row no-gutters v-if="retStatus">
                                                        <v-col cols="12" md="12" class="pt-0 mb-2">
                                                            <h4 class="text-center heading-3">Super !</h4>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <v-avatar size="150" tile>
                                                                <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                            </v-avatar>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <p class="text-medium">
                                                                Your project has been created successfully.<br />Bask in your glory !
                                                            </p>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <v-btn id="AutoTest_IP97" color="secondary" class="mr-2 elevation-0" @click="CloseProjectDialog()">Done</v-btn>
                                                        </v-col>
                                                    </v-row>
                                                    <v-row no-gutters v-if="!retStatus">
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                            <p class="text-medium">
                                                                Failed while creating new project ..
                                                            </p>
                                                        </v-col>
                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                          <Secondary-Button id="AutoTest_IP98" class="btn-122x36" title="Done" @click.native="CloseProjectDialog()"></Secondary-Button>
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

                    <v-divider></v-divider>
                </v-card>
            </v-dialog>

        
           <!--Create New Project New UI-->
            <template>
              <v-dialog persistent v-model="CreateNewProjectDialog" >
                <v-card class="right-fixed-panel" >
                    <v-card-title>
                        <v-avatar size="30" color="#EDF6FF" class="mr-2">
                            <v-icon color="secondary" size="15">mdi-file</v-icon>
                        </v-avatar> Project Setup
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP109" text @click="CreateNewProjectDialog=false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                  <v-card-text>
                    <v-stepper v-model="CreateNewProjectSteps" class="elevation-0">
          
            
                      <v-row no-gutters class="mt-10">
                        <!--step header 1-->
                        <v-col md="auto">
                          <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>0 }" >
                            <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>0}">
                              mdi-file-document
                            </v-icon><br>
                            Define Project
                          </p>
                        </v-col>
                        <v-col>
                          <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>1}"></v-divider>
                        </v-col>
                       <!--__________________-->

                        <!--step header 2-->
                        <v-col md="auto">
                          <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>1}" >
                            <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>1}">
                              mdi-tablet-dashboard
                            </v-icon>
                            <br>
                            Select Template
                
                          </p>
                        </v-col>
                        <v-col>
                          <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>2}"></v-divider>
                        </v-col>
                        <!--__________________-->

                        <!--step header 3-->
                        <v-col md="auto">
                          <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>2}" >
                            <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>2}">
                              mdi-file-chart
                            </v-icon>
                            <br>
                           Define Outcomes
                          </p>
                        </v-col>
                        <v-col>
                          <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>3}"></v-divider>
                        </v-col>
                        <!--__________________-->

                        <!--step header 4-->
                        <v-col md="auto">
                          <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>3}" >
                            <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>3}">
                              mdi-clipboard-list
                            </v-icon>
                            <br>
                          Define Outputs
                          </p>
                        </v-col>
                        <v-col>
                          <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>4}"></v-divider>
                        </v-col>
                        <!--__________________-->

                        <!--step header 5-->
                        <v-col md="auto">
                          <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>4}" >
                            <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>4}">
                              mdi-cash-multiple
                            </v-icon>
                            <br>
                          Define Inputs
                          </p>
                        </v-col>
        
                        <!--__________________-->
           
            
                      </v-row>
                      <v-stepper-items>
                        <v-stepper-content step="1">
                          <Create-New-Project-Step1 
                                                    :CreateNewProjectStepsProp="CreateNewProjectSteps" @CreateNewProjectStepsPropChange="CreateNewProjectSteps=$event"
                                                    :CreateNewProjectDialogProp="CreateNewProjectDialog"
                                                    @CreateNewProjectDialogPropChange="CreateNewProjectDialog=$event"
                                                    ></Create-New-Project-Step1>
                        </v-stepper-content>

                        <v-stepper-content step="2">
                          <Create-New-Project-Step2 
                           :CreateNewProjectStepsProp="CreateNewProjectSteps"
                            @CreateNewProjectStepsPropChange="CreateNewProjectSteps=$event"
                          ></Create-New-Project-Step2>
                          <v-row>
                          </v-row>
                        </v-stepper-content>
            
                        <v-stepper-content step="3">
                          <Create-New-Project-Step3 :CreateNewProjectStepsProp="CreateNewProjectSteps" @CreateNewProjectStepsPropChange="CreateNewProjectSteps = $event"></Create-New-Project-Step3>
                        </v-stepper-content>
            
                        <v-stepper-content step="4">
                          <Create-New-Project-Step4 :CreateNewProjectStepsProp="CreateNewProjectSteps" @CreateNewProjectStepsPropChange="CreateNewProjectSteps=$event"></Create-New-Project-Step4>
                        </v-stepper-content>
            
                        <v-stepper-content step="5">
                          <Create-New-Project-Step5 
                            :CreateNewProjectDialogProp="CreateNewProjectDialog"
                            @CreateNewProjectDialogPropChange="CreateNewProjectDialog=$event"
                            :CreateNewProjectStepsProp="CreateNewProjectSteps"
                            @CreateNewProjectStepsPropChange="CreateNewProjectSteps=$event"
                            ></Create-New-Project-Step5>
                        </v-stepper-content>
                      </v-stepper-items>
                    </v-stepper>

                  </v-card-text>
                </v-card>
            </v-dialog>
            </template>
          <!--________________________________________-->
          <!--Main Body-->
            <v-row v-if="ProjectTableItems.length>0">
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="ProjectTableheaders"
                                  :items="ProjectTableItems"
                                  v-model="selectedProjectID"
                                  show-select=""
                                  item-key="ProjectId"
                                  :search="searchProject"
                                  class="elevation-0">
                        <!--Projects list column Name-->
                        <template v-slot:item.ProjectName="{ item }">
                            <router-link class="text--default text-link" link :to="{ name: 'ManageProjectDetails', query: { WsID: WorkspaceID,Access:RWAccess, InitID: InitiativeID, ProjID: item.ProjectId,AccessType:'Initiative' } }">
                                {{ item.ProjectName }}
                            </router-link>
                        </template>
                        <!-- column State-->
                        <template v-slot:item.State="{ item }">
                            <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" v-bind:disabled ="!screenAccess"
                                      @change="GetProjectID(item.ProjectId,item.ProjectName,item.IsActive),IsActiveToggleDialog = true,ProjectName=item.ProjectName"></v-switch>
                        </template>
                        <!-- DateTime-->
                        <template v-slot:item.StartDate="{ item }">
                            <span>{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</span>
                        </template>
                        <template v-slot:item.EndDate="{ item }">
                            <span>{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</span>
                        </template>
                        <template v-slot:item.Progress="{ item }">
                            <span>0%</span>
                        </template>
                        <template v-slot:item.FundUtilization="{ item }">
                            <span>0%</span>
                        </template>
                        <!-- Draft project Actions-->
                        <template v-slot:item.Actions="{item}">
                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">
                                    <v-list-item id="AutoTest_IP99" @click.stop="" link :to="{ name: 'ManageProjectDetails', query: { WsID: WorkspaceID,Access:RWAccess, InitID: InitiativeID, ProjID: item.ProjectId,AccessType:'Initiative' } }">
                                        <v-list-item-icon>
                                            <v-icon>mdi-pencil-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Edit
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_IP100" link @click.stop="GetProjectID(item.ProjectId,item.ProjectName),DeployDailog=true,ProjectName=item.ProjectName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Deploy
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_IP101" link="" @click.stop="GetProjectID(item.ProjectId,item.ProjectName),RemoveProjectDailog=true,ProjectName=item.ProjectName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>
                                <v-dialog v-model="DeployDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-folder-account-outline</v-icon>
                                            </v-avatar>
                                            Deploy Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP102" text="" @click="DeployDailog=false">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to deploy this project ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{ProjectName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <Secondary-Button id="AutoTest_IP103" class="btn-122x36 mr-4" title="Deploy" @click.native="ChangeProjectStatus()"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_IP104" class="btn-122x36" title="Cancel" @click.native="DeployDailog=false"></Default-Button-Outlined>
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
                                            <v-btn id="AutoTest_IP105" text="" @click="RemoveProjectDailog=false">
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
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{ProjectName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <Secondary-Button id="AutoTest_IP106" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(2)"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_IP107" class="btn-122x36" title="Cancel" @click.native="RemoveProjectDailog=false"></Default-Button-Outlined>
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
                            <v-avatar size="80" tile class="elevation-1 base-border-radius mr-2">
                                <v-img src="../../images/ic_no_workspace.svg" height="80" width="80" class="ma-4"></v-img>
                            </v-avatar>
                            <p class="heading-3 pt-3">No projects available</p>
                            <v-btn id="AutoTest_IP108" height="32" color="success-btn" dark class="elevation-0 mr-2" v-bind:disabled ="!screenAccess"
                                   @click="CreateNewProjectDialog=true,GetProjectListByInitID()">
                                Create project
                            </v-btn>
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
                        <v-btn id="AutoTest_IP109" text @click="IsActiveToggleDialog=false">
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
                                                <v-btn id="AutoTest_IP110" color="secondary elevation-0" class="mr-2" @click="ChangeState()">Change</v-btn>
                                                <v-btn id="AutoTest_IP111" outlined color="grey" @click="IsActiveToggleDialog=false, GetProjectListByInitID()">Cancel</v-btn>
                                            </v-col>

                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <v-snackbar v-model="snackbarActions" bottom color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>
<script type="text/javascript">
    import PreLoader from '@/components/pre-loader.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import CreateNewProject from '@/views/Projects/CreateNewProject.vue'

    import CreateNewProjectStep1 from '@/views/Projects/CreateNewProjectStep-1.vue'
    import CreateNewProjectStep2 from '@/views/Projects/CreateNewProjectStep-2.vue'
    import CreateNewProjectStep3 from '@/views/Projects/CreateNewProjectStep-3.vue'
    import CreateNewProjectStep4 from '@/views/Projects/CreateNewProjectStep-4.vue'
    import CreateNewProjectStep5 from '@/views/Projects/CreateNewProjectStep-5.vue'
    import objUtils from '../../utils.js'

    var objInitiative;
    async function importInitscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
        ]);
    };
    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/Project.js").then(mod => {
                objProject = new mod.Project();
            })
        ]);
    };

    export default {
        data() {
            return {
             CreateNewProjectSteps: 1,
            CreateNewProjectDialog:null,
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeID: 0,
                PreLoader: false,

                ProjectTableheaders: [
                    { text: "Name", align: "left", value: "ProjectName" },
                    { text: "Code", value: "Code" },
                    { text: "Start Date", value: "StartDate" },
                    { text: "End Date", value: "EndDate", },
                    { text: "Budget", value: "Budget", },
                    { text: "Progress", value: "Progress" },
                    { text: "Fund Utilization", value: "FundUtilization", },
                    { text: "State", value: "State", sortable: false },
                    { text: "Actions", value: "Actions", sortable: false }

                ],
                ProjectTableItems: [],
                selectedProjectID: [],
                searchProject: "",
                ProjectId: "",
                ProjectName: "",
                State:"",
                RemoveProjectDailog: false,
                RemoveBulkProjectDailog: false,
                DeployDailog: false,
                IsActiveToggleDialog: false,

                //=====CreateProject ===========

                CreateProjectDialog: false,
                NewProjectSteps: 0,

                prjName: "",
                prjDescription: "",
                prjCode: "",
                prjBudget: 0,

                prjThemeItems: [],
                TemplateItems: [],
                TemplateUsecases: [],

                ThemeSelected: false,
                prjTemplateSelected: false,
                ProjectTemplateID: null,
                TemplateName: "",
                ThemeName: "",
                TemplateDescription: "",
                item: "",
                startDate: new Date().toISOString().substr(0, 10),
                mStartDate: false,
                endDate: new Date().toISOString().substr(0, 10),
                mEndDate: false,

                countryList: [],
                Country: "",
                stateList: [],
                State: "",
                districtList: [],
                District: "",

                TagMenu: false,
                //TagItems: [],
                TagList: [],
                //isNewTag: false,
                tagColor: "",
                tagName: "",
                tagID: "",
                //selectedColor: false,
                vmTag: "",
                allTags: false,

                retStatus: false,
                isValid: true,


                // Altered Tag Menu
                AllTagListSteps: 0,
                //NewTagListSteps: 0,
                TagHeaderName: "Add Tag",
                NewTagMenu: false,
                AllTagList: [],
                //TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
               // allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                filterAllTagList: [],
                SearchTagDetails: "",
                Msg :"",
                snackbarActions :false,
                alphanumericRules: [
                    v => !v ||  /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name'
                ],
                 screenAccess: 0,
                RWAccess: 0,

            };
        },
        computed: {
            computedStartDate() {
                return objUtils.common.formatDate(this.startDate)
            },
            computedEndDate() {
                return objUtils.common.formatDate(this.endDate)
            },

        },

        mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;   
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await importInitscript();

            this.PreLoader = true;
            await this.GetProjectListByInitID();
            await this.GetNextProjectCode();
            await this.GetProjectThemeList();
            await this.GetCountryDetails();
            await this.fnGetAllTags();
            this.PreLoader = false;

            //await this.fnInitializeSteper();

        },
    components: {
            'Pre-Loader': PreLoader,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,

             'Create-New-Project-Step1':CreateNewProjectStep1,
            'Create-New-Project-Step2':CreateNewProjectStep2,
            'Create-New-Project-Step3':CreateNewProjectStep3,
            'Create-New-Project-Step4':CreateNewProjectStep4,
            'Create-New-Project-Step5':CreateNewProjectStep5,
            },
        methods: {
            /// <summary>
            ///  GetProjectListByInitID :- Function used to fetch project details based on InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectListByInitID() {
                var data = new FormData();
                data.append("stage",'Draft');
                data.append("InitiativeId", this.InitiativeID);
                var result = await objProject.GetProjectListByInitID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                if (result.data) {
                    this.ProjectTableItems = result.data;
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
                        this.Msg  ="Please select the projects";
                        this.snackbarActions =true;
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
                       this.Msg="Successfully deleted";
                    } else {

                        this.Msg = "Failed to delete";
                    }
                    this.snackbarActions =true;
                }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.RemoveProjectDailog = false;
                this.RemoveBulkProjectDailog = false;

            },

            /// <summary>
            ///  ChangeProjectStatus :- Function used to change project status based on ProjectID
            ///  ProjectStatus 0=draft, 1=deploy, 2=archive, 3=completed
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ChangeProjectStatus() {
                var data = new FormData();
                data.append("ProjectID", JSON.stringify(this.ProjectID));
                data.append("ProjectStatus", 'Deploy')
                var result = await objProject.ChangeProjectStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg= "Successfully deployed";
                    } else {
                        this.Msg = "Failed to deploy";
                    }
                    this.snackbarActions =true;
                }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.DeployDailog = false;
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
                    this.Msg = "Successfully Change State";
                } else {
                    this.Msg = "Failed to Change State";
                }
                this.snackbarActions =true;
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.IsActiveToggleDialog = false;
            },

            //============== New Project Creation =================
            //===Step: 1===========================================
            /// <summary>
            ///  IsValidDate :- Function used to check project start date and project end date is valid
            ///  startdate/ Enddate  not < todays date
            ///  enddate  any valid range - Max value
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {

                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.snackbarActions =true;
                    this.endDate = "";
                    return;
                }
            },
            //==========NextProjectCode =================
            /// <summary>
            ///  GetNextProjectCode :- Function used to fetch project code based on ProjectId
            ///  It increment project code with maximum 6 length
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetNextProjectCode() {
                var result = await objProject.GetNextProjectCode();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.prjCode = result.data;
                }
            },
           
            //=========Country/State/District=========
            /// <summary>
            ///  GetCountryDetails :- Function used to fetch country details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetCountryDetails() {
                var result = await objProject.GetCountryDetails();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.countryList = result.data;
                }
            },

            /// <summary>
            ///  GetStateDetails :- Function used to fetch state details based on CountryID
            /// </summary>
            /// <param name="CountryID"></param>
            /// <returns> </returns>
            async GetStateDetails(CountryID) {
                var data = new FormData();
                data.append("CountryID", CountryID);
                var result = await objProject.GetStateDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.stateList = result.data;
                }
            },

            /// <summary>
            ///  GetDistrictDetails :- Function used to fetch district details based on StateID
            /// </summary>
            /// <param name="StateID"></param>
            /// <returns> </returns>
            async GetDistrictDetails(StateID) {
                var data = new FormData();
                data.append("StateID", StateID);
                var result = await objProject.GetDistrictDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.districtList = result.data;
                }
            },

            //===Step: 2===========================================
            //================Project Template=====================
            /// <summary>
            ///  GetProjectThemeList :- Function used to fetch project themelist
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectThemeList() {
                var data = new FormData();
                data.append("InitiativeID", this.InitiativeID);
                var result = await objProject.GetProjectThemeList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data)
                        this.prjThemeItems = result.data;

                    var i = 0;
                    for (i = 0; i < this.prjThemeItems.length; i++)
                        this.prjThemeItems[i].IsSelected = 0;
                }

            },

            /// <summary>
            ///  GetProjectTemplatesList :- Function used to fetch project template list based on ThemeID
            /// </summary>
            /// <param name="ThemeID"></param>
            /// <param name="NoOfTemplates"></param>
            /// <returns> </returns>
            async GetProjectTemplatesList(ThemeID, NoOfTemplates) {
                var i = 0;
                this.ThemeID = ThemeID;
                this.ThemeName = this.prjThemeItems.filter(p => p.ThemeID == ThemeID)[0].Theme;
                this.prjThemeItems.filter(p => p.ThemeID == ThemeID)[0].IsSelected = 1;
                this.ThemeSelected = true;

                var data = new FormData();
                data.append("InitiativeID", this.InitiativeID);
                data.append("ThemeID", ThemeID);
                data.append("NoOfTemplates", NoOfTemplates);
                var result = await objProject.GetProjectTemplateList(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.TemplateItems = result.data;
                    for (i = 0; i < this.TemplateItems.length; i++)
                        this.TemplateItems[i].IsSelected = 0;
                }
                for (i = 0; i < this.prjThemeItems.length; i++)
                    if (this.prjThemeItems[i].ThemeID != ThemeID)
                        this.prjThemeItems[i].IsSelected = 0;

                this.NewProjectSteps = 3;
            },

            /// <summary>
            ///  SelectThemes :- Function used to select themes
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async SelectThemes(item) {
                var i = 0;
                this.ThemeName = this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].Theme;

                if (item.IsSelected) {
                    this.GetProjectTemplatesList(item.ThemeID, item.NoOfTemplates);
                    this.NewProjectSteps = 3;
                }

            },

            /// <summary>
            ///  ViewTemplate :- Function used to fetch project template details based on TemplateID
            /// </summary>
            /// <param name="TemplateID"></param>
            /// <returns> </returns>
            async ViewTemplate(TemplateID) {
                var data = new FormData();
                data.append("TemplateID", TemplateID);
                var result = await objInitiative.GetProjectTemplateDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.TemplateUsecases = result.data;
                        this.TemplateName = this.TemplateUsecases[0].TemplateName;
                        this.TemplateDescription = this.TemplateUsecases[0].Description;
                    }
                }
            },

            /// <summary>
            ///  SelectTemplates :- Function used to select templates based on ProjectTemplateID
            /// </summary>
            /// <param name="ProjectTemplateID"></param>
            /// <param name="isSelected"></param>
            /// <returns> </returns>
            SelectTemplates(ProjectTemplateID, isSelected) {
                var i = 0;
                if (isSelected) {
                    this.prjTemplateSelected = true;
                    this.ProjectTemplateID = ProjectTemplateID;
                }
                else
                    this.prjTemplateSelected = false;

                for (i = 0; i < this.TemplateItems.length; i++) {
                    if (this.TemplateItems[i].ProjectTemplateID != ProjectTemplateID)
                        this.TemplateItems[i].IsSelected = false;
                    else
                        this.TemplateItems[i].IsSelected = true;
                }
            },

            /// <summary>
            ///  fnCreateProject :- Function used to add new project
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateProject() {

                var i = 0;
                var TagIDList = [];

                if (this.prjName == "") {
                    this.Msg = "Project Name is required";
                    this.snackbarActions =true;
                    return;
                }

                /*Chk Duplicate*/

                for (i = 0; i < this.ProjectTableItems.length; i++) {
                    if (this.ProjectTableItems[i].ProjectName == this.prjName && this.ProjectTableItems[i].CountryID == this.Country
                        && this.ProjectTableItems[i].StateID == this.State    && this.ProjectTableItems[i].District == this.District
                        && this.ProjectTableItems[i].StartDate == this.startDate && this.ProjectTableItems[i].EndDate == this.endDate
                        && this.ProjectTableItems[i].Budget == this.prjBudget && this.ProjectTableItems[i].ProjectTemplateID == this.ProjectTemplateID)
                    {
                        this.Msg ="Duplicate Entry - Same Project existis..";
                        this.snackbarActions =true;
                        this.NewProjectSteps = 1;
                        return;
                    }
                }


                for (i = 0; i < this.SelectedTag.length; i++) {
                    TagIDList[i] = this.SelectedTag[i].TagID;
                }
                var data = new FormData();
                data.append("ProjectName", this.prjName);
                data.append("Description", this.prjDescription);
                data.append("Code", this.prjCode);
                data.append("Budget", this.prjBudget);
                data.append("StartDate", this.startDate);
                data.append("EndDate", this.endDate);
                data.append("CountryID", this.Country);
                data.append("StateID", this.State);
                data.append("DistrictID", this.District);
                data.append("ProjectTags", JSON.stringify(TagIDList));
                data.append("ProjectTemplateID", this.ProjectTemplateID);
                data.append("InitiativeId", this.InitiativeID);

                var result = await objProject.CreateProject(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success")
                        this.retStatus = true;
                    else
                        this.retStatus = false;
                }
                TagIDList = [];
                this.NewProjectSteps = 5;
            },

            /// <summary>
            ///  CloseProjectDialog :- Function used to close project dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseProjectDialog() {
                this.prjName = "";
                this.prjDescription = "";
                this.prjCode = "";
                this.prjBudget = "";
                this.startDate = new Date().toISOString().substr(0, 10);
                this.endDate = new Date().toISOString().substr(0, 10);
                this.Country = "";
                this.State = "";
                this.District = "";

                this.stateList = [];
                this.districtList = [];
                this.TagList = [];
                this.ProjectTemplateID = "";
                for (var i = 0; i < this.prjThemeItems.length; i++)
                    this.prjThemeItems[i].IsSelected = 0;
                this.ThemeSelected = false;
                this.prjTemplateSelected = false;

                this.CreateProjectDialog = false;
                this.NewProjectSteps = 1;
                this.GetProjectListByInitID();
                this.GetNextProjectCode();
            },
            //=====================================================Tags
            /// <summary>
            ///  fnGetAllTags :- Function used to fetch project tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objProject.GetAllProjectTags(this.InitiativeID);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });

                } else if (result.data) {
                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg= "Failed to get tag list.";
                    this.snackbarActions =true;
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
                var result = await objProject.UpdateTagDetails(data);
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
                     else {
                        this.Msg = "Tag already exists";
                        this.snackbarActions =true;
                    }
            },


            /// <summary>
            ///  CreateNewTag :- Function used to create new project tag 
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objProject.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.NewTagName = "";
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags();
                    }
                    else {
                        this.Msg = "Tag already exists";
                        this.snackbarActions =true;
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
