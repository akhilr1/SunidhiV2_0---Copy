<template>
    <v-row class="mx-4">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-col sm="12" md="6">
            <h3 class="create-project-steps-title text-center">Define Outcomes</h3>
            <v-row>
                <v-col cols="12" align="center" class="pt-9">
                    <v-carousel cycle="" height="200px"
                                hide-delimiter-background=""
                                show-arrows-on-hover="">
                        <v-carousel-item>
                            <img src="../../images/Create-Project-Step3-Vector-1.svg" class="create-project-corousal-vector" />
                        </v-carousel-item>
                    </v-carousel>
                </v-col>
                <v-col cols="12">
                    <p class="text-center">
                        Here you can define outcomes for<br>the project
                    </p>
                </v-col>
            </v-row>
        </v-col>

        <v-col sm="12" md="6">
            <v-row>
                <v-col md="auto">
                    <h6 class="heading-3">
                        <v-avatar size="30" color="#EDF6FF" class="mr-2">
                            <v-icon color="secondary" size="15">mdi-file-chart</v-icon>
                        </v-avatar> Outcomes
                    </h6>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto">
                    <Success-Button title="Add Outcomes" @click.native="AddCoutcomesdlg=true"></Success-Button>
                </v-col>
            </v-row>

            <!-- Add Outcomes-->
            <v-dialog v-model="AddCoutcomesdlg" width="850" persistent>
                <v-card>
                    <v-card-title primary-title="" class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                        </v-avatar>
                        Add Outcome
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP563" text="" @click="fnClearFeilds()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-form ref="form">
                        <v-card-text>
                            <h4 class="text-center heading-3">
                                Please enter the basic details to create a new Outcome.
                            </h4>
                            <v-row justify="center">
                                <v-col md="8" class="mt-1 mb-2" style="overflow-x: hidden">
                                    <h4 class="text-left heading-3 mt-4">Basic Details</h4>
                                    <v-text-field outlined=""
                                                  dense=""
                                                  label="Enter Outcome Statement"
                                                  v-model="OutcomeStmt"
                                                  :rules="[rules.required]">
                                    </v-text-field>

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
                                                                  persistent-hint
                                                                  outlined
                                                                  hide-details
                                                                  dense
                                                                  v-on="on"></v-text-field>
                                                </template>
                                                <v-date-picker v-model="startDate"
                                                               color="primary"
                                                               no-title
                                                               @input="FrmDateMenu = false"
                                                               format="DD-MM-YYYY"
                                                               dense
                                                               :min="ProjectStartDate"
                                                               :max="ProjectEndDate"></v-date-picker>
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
                                                                  persistent-hint
                                                                  outlined
                                                                  hide-details
                                                                  dense
                                                                  v-on="on"></v-text-field>
                                                </template>
                                                <v-date-picker v-model="endDate"
                                                               color="primary"
                                                               no-title
                                                               @input="ToDateMenu = false"
                                                               format="DD-MM-YYYY"
                                                               dense
                                                               :min="startDate"
                                                               :max="ProjectEndDate"></v-date-picker>
                                            </v-menu>
                                        </v-col>
                                    </v-row>

                                    <!-- Tags-->
                                    <v-row class="mb-1">
                                        <v-col class="text-left d-inline-flex" md="12">
                                            <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                            <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                <template v-slot:activator="{ on }">
                                                    <v-btn id="AutoTest_IP73" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                           @click="fnInitializeSteper(),NewTagMenu = false" class="mr-1">
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
                                                                                                <v-icon v-if="index.IsChecked" size="20" left :key="index.TagID">mdi-check</v-icon>
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
                                                                                <v-btn id="AutoTest_IP79" color="success-btn" @click="searchTag()" dark class="text-limit" max-width="200">
                                                                                    Create New
                                                                                    <span v-if="this.SearchTagDetails.length>0 && this.SearchTagDetails.length<=6 && IsSearchTagExist == false" class="pl-1 text-center">"{{this.SearchTagDetails}}"</span>
                                                                                    <span class="pl-1 text-center" v-else v-show="this.SearchTagDetails.length>6 && IsSearchTagExist == false">"{{ this.SearchTagDetails.substring(0,6)+".." }}"</span>
                                                                                </v-btn>
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
                                                
                                                <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[0].TagName}}</v-chip>
                                                <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1 text--white" @click="allTags=true">{{SelectedTag[1].TagName}}</v-chip>
                                                <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1" @click="allTags=true">+{{SelectedTag.length-2}}<span class="px-1">more</span></v-chip>
                                            </div>
                                            <v-chip v-if="allTags == true" label small class="tags mx-1" @click="allTags=false">Show less</v-chip>
                                            
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
                                    <!--*******************************************************************************************************************-->
                                    <!--<v-row class="mb-1">
        <v-col class="text-left d-inline-flex" md="12">
            <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
            <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                <template v-slot:activator="{ on }">
                    <v-btn id="AutoTest_IP304" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                           @click="fnInitializeSteper()" class="mr-1">
                        <v-icon size="15">mdi-plus</v-icon>
                    </v-btn>
                </template>
                <v-card>
                    <v-card-text class="pt-0 pb-1">
                        <v-col cols="12" md="12">-->
                                    <!-- Header Text  -->
                                    <!--<v-col cols="12" md="12" class="d-inline-flex">
        <v-btn id="AutoTest_IP305" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
            <v-icon size="15">mdi-chevron-left</v-icon>
        </v-btn>
        <v-spacer></v-spacer>
        <h3 class="text-left page-head pt-2">
            {{TagHeaderName}}
        </h3>
        <v-spacer></v-spacer>
        <v-btn id="AutoTest_IP306" text @click="NewTagMenu = false,SearchTagDetails=''">
            <v-icon color="#707070" size="15">mdi-close</v-icon>
        </v-btn>
    </v-col>
    <v-stepper v-model="AllTagListSteps" class="elevation-0">
        <v-stepper-items>-->
                                    <!-- Create New Tag Option when there is no Tags are -->
                                    <!--<v-stepper-content step="1" class="pa-0">
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
                <v-btn id="AutoTest_IP307" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn>-->
                                    <!-- Adding To Existing Tag List-->
                                    <!--</v-col>
        </v-row>
    </v-stepper-content>-->
                                    <!-- Select from existing Tags-->
                                    <!--<v-stepper-content step="2" class="pa-0">

        <v-row style="max-height:30vh;overflow-y:scroll">
            <v-col col="12" md="12" class="mb-0 pt-0">
                <p class="mb-2">Search Tags</p>
                <v-text-field outlined="" dense="" v-model="SearchTagDetails" label="Search tag here" hide-details class="mb-3">
                </v-text-field>
                <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                    <v-chip id="AutoTest_IP308" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                            label @click.native="fnAddToSelectedTag(index)">
                        {{ index.TagName }}
                        <v-icon id="AutoTest_IP309" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                    </v-chip>
                </v-chip-group>
            </v-col>
        </v-row>
        <v-col cols="12" md="12" class="text-center mt-3">
            <v-btn id="AutoTest_IP310" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
        </v-col>
    </v-stepper-content>-->
                                    <!-- Edit Tag-->
                                    <!--<v-stepper-content step="3" class="pa-0">
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
                                        <v-btn id="AutoTest_IP311" color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                    </v-col>
                                </v-row>
                            </v-stepper-content>

                        </v-stepper-items>
                    </v-stepper>
                </v-col>
            </v-card-text>
        </v-card>
    </v-menu>
    <div v-if="!allTags">-->
                                    <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                    <!--<v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
            </div>
            <v-btn id="AutoTest_IP312" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
            </v-btn>
            <v-btn id="AutoTest_IP313" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
            </v-btn>
        </v-col>
    </v-row>
    <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
        <v-col md="12" class="chip-container mb-0 pt-0">
            <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                <v-chip id="AutoTest_IP314" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                    {{TagItem.TagName}}
                </v-chip>
            </v-chip-group>
        </v-col>
    </v-row>-->
                                    <!--*******************************************************************************************************************-->
                                    <!-- Impact Indicators --->
                                    <h4 class="text-left heading-3 mt-2 mb-2">Impact Indicators</h4>
                                    <v-btn @click="AddImpactIndicator = true" width="450" class="mb-3" outlined dense>Select Impact Indicators</v-btn>
                                    <v-dialog v-model="AddImpactIndicator" width="650" persistent :retain-focus="false">
                                        <v-form ref="entryForm" @submit.prevent="submitHandler">
                                            <v-card>
                                                <v-card-title primary-title="" class="page-head pb-0">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                    </v-avatar>
                                                    Impact Indicators
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP563" text="" @click="AddImpactIndicator = false,impactError= false">
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
                                                                    <v-checkbox @change="GetSDGTargetsBasedOnSDGID(item.pk_SDGID,item.SDG_Name)"
                                                                                v-model="item.IsSelected"
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

                                    <v-row v-show="AddImpactIndicator == false" no-gutters v-for="(datas, index) in lstSGDData" :key="datas.pk_SDGID">
                                        <v-col md="auto">
                                            <v-avatar size="50" tile class="elevation-1 base-border-radius mt-1 mr-2" light>
                                                <img :src="imagesInput[datas.pk_SDGID]" class="team-image" />
                                            </v-avatar>
                                        </v-col>
                                        <v-col md="auto">
                                            <p class="text-left" style="line-height: 17px">
                                                <span class="mt-2 mb-0"
                                                      style="max-width: 300px; display: block">
                                                    {{ datas.SDG_Name }}
                                                </span>
                                                <small>
                                                    <span>{{fnSelectedTargetCount(datas)}} targets </span>
                                                </small>
                                            </p>
                                        </v-col>
                                        <v-col sm="12">
                                            <!-- <v-autocomplete
              v-model="SelectedSDGTargets"
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

            </v-autocomplete> -->
                                            <v-btn v-if="fnSelectedTargetCountPerImpact(datas) < 1" @click="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID" width="450" class="mb-3" outlined dense>Select Targets</v-btn>
                                            <v-expansion-panels v-if="fnSelectedTargetCountPerImpact(datas) > 0" class="mb-3">
                                                <v-expansion-panel class="mt-2">
                                                    <v-expansion-panel-header class="v-expansion-panel-header">
                                                        <v-row>
                                                            <v-col sm="10" class="pl-5 pt-6"> {{fnSelectedTargetCountPerImpact(datas)}} Selected Targets</v-col>
                                                            <v-col sm="2">
                                                                <v-btn class="mx-2"
                                                                       fab
                                                                       dark
                                                                       color="green"
                                                                       elevation="0"
                                                                       x-small
                                                                       @click.native.stop="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID">
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
                                                                <v-chip v-if="data.fk_SDGID == targetId" small>
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
                                                                                        color="green">
                                                                            </v-checkbox>
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
                                                                    <v-btn color="#47BB76" style="color:white" width="160" type="submit" @click="submitHandlerTarget(item)">Save</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-title>
                                                    </v-card>
                                                </v-form>
                                            </v-dialog>
                                        </v-col>
                                    </v-row>


                                    <!-- Data Collection -->
                                    <h4 class="heading-3 my-2">Data Collection</h4>
                                    <v-select dense
                                              v-model="DataCollectionType"
                                              :items="DataCollectionTypeItems"
                                              label="Select Outcome Type"
                                              outlined
                                              @change="fnGetLogicType(DataCollectionType)">
                                    </v-select>
                                    <v-select dense
                                              :items="SurveyList"
                                              :item-text="(i) => i.SurveyName"
                                              :item-value="(i) => i.SurveyID"
                                              label="Select Survey"
                                              v-model="SurveyId"
                                              @change="GetSectionsBySurveyID(SurveyId)"
                                              outlined
                                              :rules="[rules.required]">
                                    </v-select>
                                    <div v-if="DataCollectionType == 'Quantitative'">
                                        <!-- Section List-->
                                        <v-select :items="SectionList"
                                                  label="Select Section"
                                                  item-text="SectionName"
                                                  item-value="SectionID"
                                                  outlined
                                                  dense
                                                  v-model="SurveySectionId"
                                                  @change="GetQstnBySectionID(SurveySectionId)"></v-select>
                                        <!-- Question List-->
                                        <v-select :items="QuestionList"
                                                  label="Select Question"
                                                  item-text="QuestionName"
                                                  item-value="QuestionID"
                                                  outlined
                                                  dense
                                                  @change="GetResponsesByQstnid(SurveyQuestionId)"
                                                  v-model="SurveyQuestionId"></v-select>
                                        <!-- Respose List-->
                                        <!--<v-select :items="ResponseList"
              label="Select Response"
              item-text="ResponseName"
              item-value="ResponseID"
              outlined
              dense
              @change=""
              v-model="ResponseID"></v-select>-->
                                        <!-- Define Logic-->
                                        <v-select dense
                                                  :items="LogicItems"
                                                  item-text="Type"
                                                  item-value="ID"
                                                  label="Define Logic"
                                                  v-model="SurveyLogic"
                                                  outlined="">
                                        </v-select>
                                    </div>
                                    <div v-if="SurveyLogic==1 || SurveyLogic==2 || logicAnsType=='Single Line Text' || logicAnsType=='Numeric' ">
                                        <!-- Target-->
                                        <v-text-field outlined
                                                      dense
                                                      v-model="SurveyTarget"></v-text-field>
                                    </div>
                                    <div v-else>
                                        <v-select v-show="DataCollectionType == 'Quantitative'"
                                                  dense
                                                  :items="lstLogicAnswers"
                                                  v-model="DefineTarget"
                                                  item-text="OptionValue"
                                                  item-value="OptionID"
                                                  label="Define Target"
                                                  outlined="">
                                        </v-select>
                                    </div>
                                    <!-- Reporting Frequency-->
                                    <v-select dense=""
                                              :items="lstPeriodicity"
                                              :item-text="(i) => i.Value"
                                              :item-value="(i) => i.Key"
                                              label="Select Reporting Frequency"
                                              outlined=""
                                              v-model="ReportingFrequency"
                                              :rules="[rules.required]">
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
                                                              persistent-hint
                                                              outlined
                                                              v-on="on"
                                                              class="mt-4"
                                                              hide-details
                                                              dense></v-text-field>
                                            </template>
                                            <v-date-picker v-model="periodicityDate"
                                                           no-title
                                                           @input="periodicityDateMenu = false"
                                                           dense
                                                           :min="startDate"
                                                           :max="endDate"></v-date-picker>
                                        </v-menu>
                                    </div>
                                    <!-- Weekly-->
                                    <div v-show="ReportingFrequency === 4" class="mt-4">
                                        <v-select dense=""
                                                  :items="lstWeek"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a day"
                                                  outlined=""
                                                  v-model="ReportingWeek"
                                                  hide-details>
                                        </v-select>
                                    </div>
                                    <!-- Monthly-->
                                    <div v-show="ReportingFrequency === 5" class="mt-4">
                                        <v-select dense=""
                                                  :items="lstMonthDate"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Date"
                                                  outlined=""
                                                  v-model="ReportingMonthDay"
                                                  hide-details>
                                        </v-select>
                                    </div>
                                    <!-- Quaterly , Halyearly and Yearly-->
                                    <div class="d-inline-flex mt-4"
                                         v-if="ReportingFrequency == 6 || ReportingFrequency == 7 || ReportingFrequency == 8">
                                        <v-select hide-details
                                                  class="mr-1 mb-4"
                                                  dense=""
                                                  :items="lstMonth"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Month"
                                                  outlined=""
                                                  v-model="ReportingMonth">
                                        </v-select>
                                        <v-select class="mb-4"
                                                  hide-details
                                                  dense=""
                                                  :items="lstMonthDate"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Date"
                                                  outlined=""
                                                  v-model="ReportingMonthDateDay">
                                        </v-select>
                                    </div>
                                    <!--<v-select dense=""
                                              :items="workflows"
                                              :item-text="(i) => i.WfName"
                                              :item-value="(i) => i.WorkflowID"
                                              label="Select Workflow"
                                              outlined=""
                                              v-model="Workflow">
                                    </v-select>-->
                                    <!-- User List-->
                                    <v-autocomplete v-model="SelectedUserList"
                                                    :items="lstUserList"
                                                    outlined
                                                    dense
                                                    chips
                                                    small-chips
                                                    label="Select Users"
                                                    item-text="Name"
                                                    item-value="ID"
                                                    hide-details
                                                    class="mb-2"
                                                    multiple
                                                    :rules="[rules.required]"
                                                    return-object="">
                                        <template v-slot:selection="data">
                                            <v-chip v-bind="data.attrs"
                                                    :input-value="data.selected"
                                                    close
                                                    small
                                                    @click="data.select"
                                                    @click:close="fnRemoveSelectedUsers(data.item)">
                                                <v-avatar size="12"
                                                          left
                                                          light
                                                          :color="`${commonGetColor(data.item.Name)}`">
                                                    <img v-bind:src="userimage + data.item.ProfileImage"
                                                         alt="User DP"
                                                         v-if="data.item.ProfileImage.length > 0" />
                                                    <span class="white--text" v-else>
                                                        {{ data.item.Name == null ? "" : data.item.Name.substring(0, 1).toUpperCase() }}
                                                    </span>
                                                </v-avatar>
                                                {{ data.item.Name }}
                                            </v-chip>
                                        </template>
                                        <template v-slot:item="data">
                                            <v-list-item-action>
                                                <v-checkbox @change="fnGetSelectedUserList(data.item.ID)"
                                                            v-model="data.item.IsSelected"></v-checkbox>
                                            </v-list-item-action>
                                            <v-list-item-icon>
                                                <v-avatar size="30"
                                                          class="elevation-1"
                                                          light
                                                          :color="`${commonGetColor(data.item.Name)}`">
                                                    <img v-bind:src="userimage + data.item.ProfileImage"
                                                         alt="User DP"
                                                         v-if="data.item.ProfileImage.length > 0" />
                                                    <span class="white--text" v-else>{{data.item.Name == null? "": data.item.Name.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                <v-list-item-title v-html="data.item.Name"
                                                                   class="ml-2"></v-list-item-title>
                                            </v-list-item-content>
                                        </template>
                                    </v-autocomplete>
                                </v-col>
                            </v-row>
                        </v-card-text>
                    </v-form>
                    <v-card-actions class="pb-4">
                        <v-col class="text-right">
                            <Secondary-Button id="AutoTest_IP578"
                                              :loading="waitforsave"
                                              :disabled="waitforsave"
                                              class="btn-122x36 mr-2"
                                              title="Confirm"
                                              @click.native="fnCreateOutcome()"
                                              v-bind:disabled="fnCheckMandatory()"></Secondary-Button>
                            <Default-Button-Outlined id="AutoTest_IP579"
                                                     class="btn-122x36"
                                                     title="Cancel"
                                                     @click.native="fnClearFeilds()"></Default-Button-Outlined>
                        </v-col>
                    </v-card-actions>
                </v-card>
            </v-dialog>

            <!-- Edit Outcome-->
            <v-dialog v-model="EditCoutcomesdlg" width="850" persistent :retain-focus="false">
                <v-card>
                    <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
                    <v-card-title primary-title="" class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                        </v-avatar>
                        Edit Outcome
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP563" text="" @click="fnClearFeilds()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-form ref="form">
                        <v-card-text>
                            <h4 class="text-center heading-3">
                                Please enter the basic details to create a new Outcomes.
                            </h4>
                            <v-row justify="center">
                                <v-col md="8" class="v-scrolling-div" style="height: 60vh">
                                    <v-text-field outlined=""
                                                  dense=""
                                                  label="Outcome Name"
                                                  v-model="EditOutcome.OutcomeName"
                                                  :rules="[rules.required]">
                                    </v-text-field>

                                    <!-- Date Between-->
                                    <v-row>
                                        <!-- From Date -->
                                        <v-col>
                                            <v-menu v-model="FrmDateMenuEdit"
                                                    :close-on-content-click="false"
                                                    transition="scale-transition"
                                                    offset-y
                                                    max-width="290px"
                                                    min-width="290px">
                                                <template v-slot:activator="{ on }">
                                                    <v-text-field v-model="FrmDate"
                                                                  label="Start Date"
                                                                  persistent-hint
                                                                  dense
                                                                  outlined
                                                                  hide-details
                                                                  v-on="on"></v-text-field>
                                                </template>
                                                <v-date-picker v-model="startDate"
                                                               color="primary"
                                                               no-title
                                                               @input="FrmDateMenuEdit = false"
                                                               format="DD-MM-YYYY"
                                                               dense
                                                               :min="ProjectStartDate"
                                                               :max="ProjectEndDate"></v-date-picker>
                                            </v-menu>
                                        </v-col>
                                        <!-- To Date -->
                                        <v-col>
                                            <v-menu ref="ToDateMenuEdit"
                                                    v-model="ToDateMenuEdit"
                                                    :close-on-content-click="false"
                                                    transition="scale-transition"
                                                    offset-y
                                                    max-width="290px"
                                                    min-width="290px">
                                                <template v-slot:activator="{ on }">
                                                    <v-text-field v-model="ToDate"
                                                                  label="End Date"
                                                                  persistent-hint
                                                                  outlined
                                                                  dense
                                                                  hide-details
                                                                  v-on="on">

                                                    </v-text-field>
                                                </template>
                                                <v-date-picker v-model="endDate"
                                                               color="primary"
                                                               no-title
                                                               @input="ToDateMenuEdit = false"
                                                               format="DD-MM-YYYY"
                                                               dense
                                                               :min="startDate"
                                                               :max="ProjectEndDate">

                                                </v-date-picker>
                                            </v-menu>
                                        </v-col>
                                    </v-row>
                                    <!-- Impact Indicators --->
                                    <h4 class="text-left heading-3 mt-2 mb-2">
                                        Impact Indicators ({{ lstSGDData.length }} SDG Selected)
                                    </h4>
                                    <v-btn @click="EditImpactIndicator = true" width="450" class="mb-3" outlined  dense>Select Impact Indicators</v-btn>
                                    
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
                                                        <v-chip
                                                            v-for="(data,index) in  lstSGDData"  
                                                            :key="index"
                                                            close
                                                            small
                                                            @click:close="fnRemoveSDGIndicators(data.pk_SDGID)"
                                                        >
                                                            <v-avatar left  >
                                                                <img :src="imagesInput[data.pk_SDGID]" class="team-image"   />
                                                            </v-avatar>
                                                            <span>{{ lstSGDData.length }} SDG Selected</span>
                                                        </v-chip>
                                                    </v-chip-group>
                                                    <span class="ml-3" style="color:red;font-size:13px;" v-if="impactError == true">Please Select atleat one impact indicator before saving *</span>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                        <!--Screen List with checkbox add here-->
                                                        <v-data-table :headers="impactIndicatorHeader"
                                                            :items="lstSGD"
                                                            hide-default-footer
                                                            class="elevation-0"
                                                            fixed-header
                                                            height="53vh"
                                                            disable-pagination
                                                        >
                                                            <template v-slot:item.impact="{item}">
                                                                <v-list-item-action>
                                                                    <v-checkbox
                                                                        v-model="item.Selected"
                                                                        @change="GetSDGTargetsBasedOnSDGID(item.pk_SDGID,item.SDG_Name)"
                                                                        color="green"
                                                                    ></v-checkbox>
                                                                </v-list-item-action>
                                                                <v-list-item-avatar tile class="mr-2">
                                                                    <img :src="imagesInput[item.pk_SDGID]" class="team-image" />
                                                                </v-list-item-avatar> 
                                                                {{item.SDG_Name}}
                                                            </template>
                                                        </v-data-table>
                                                    </v-col>
                                                </v-card-title>
                                                <v-card-title  >
                                                    <v-row justify="end">
                                                        <v-col sm="3">
                                                            <v-btn color="#47BB76" style="color:white" width="160" type="submit"   @click="submitHandler()">Save</v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-title>
                                            </v-card>
                                        </v-form>
                                    </v-dialog>

                                    <!-- Listing selected SDG Targets-->
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
                                                <span class="mt-2 mb-0" style="max-width: 300px; display: block">
                                                    {{ datas.SDG_Name }}
                                                </span>
                                                <small>
                                                    <span> {{fnSelectedTargetCount(datas)}} targets </span>
                                                </small>
                                            </p>
                                        </v-col>
                                        <v-col sm="12">
                    <!-- <v-autocomplete
                      v-model="SelectedSDGTargets"
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
                      @change=""return-object="">
                    </v-autocomplete> -->
                                            <v-btn  v-if="fnSelectedTargetCountPerImpact(datas) < 1" @click="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID" width="450" class="mb-3" outlined  dense>Select Targets</v-btn>
                                        </v-col>   
                                        <v-expansion-panels v-if="fnSelectedTargetCountPerImpact(datas) > 0" class="mb-1">
                                            <v-expansion-panel>
                                                <v-expansion-panel-header class="v-expansion-panel-header">
                                                    <v-row>
                                                        <v-col sm="10" class="pl-5 pt-6"> {{fnSelectedTargetCountPerImpact(datas)}} Selected Targets</v-col>
                                                        <v-col sm="2">       
                                                            <v-btn class="mx-2" fab dark color="green" elevation="0" x-small @click.native.stop="addImpactTarget = true,targetData=datas.Targets,targetId=datas.pk_SDGID">
                                                                <v-icon dark size="20"> mdi-plus </v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content class="v-expansion-panel-content__wrap">
                                                    <v-divider></v-divider>
                                                    <v-row class="v-scrolling-divs"   no-gutters>
                                                        <template  v-for="(data,index) in SelectedSDGTargets" >
                                                            <v-col sm="12" :key="data.fk_SDGID" v-if="data.fk_SDGID == datas.pk_SDGID"  >
                                                                <v-row justify="center">
                                                                    <v-col sm="10">
                                                                        <span >{{data.Target_Name}}</span>
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
                                                        <v-btn id="AutoTest_IP563" text="" @click="addImpactTarget = false,AddImpactIndicatorTarget=false">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-card-title>
                                                        <v-chip-group  v-for="(data,index) in  SelectedSDGTargets"  :key="index">
                                                            <v-chip v-if="data.fk_SDGID == targetId" small>
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
                                                                disable-pagination
                                                            >
                                                                <template v-slot:item.impact="{item}">
                                                                    <td>
                                                                        <v-checkbox @change="storeTargets(item,index)"
                                                                                    :input-value="SelectedSDGTargets.some(e=> e.Target_Name == item.Target_Name)"
                                                                                    color="green"
                                                                        ></v-checkbox>
                                                                    </td>    
                                                                    <td>
                                                                        {{item.Target_Name}}
                                                                    </td>
                                                                </template>
                                                                                               
                                                            </v-data-table>
                                                        </v-col>
                                                    </v-card-title>
                                                    <v-card-title  >
                                                        <v-row justify="end">
                                                            <v-col sm="3">
                                                                <v-btn color="#47BB76" style="color:white" width="160" type="submit"   @click="submitHandlerTarget(targetData)">Save</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-title>
                                                </v-card>
                                            </v-form>
                                        </v-dialog>
                                    </v-row>

                                    <!-- Data Collection -->
                                    <h4 class="heading-3 my-2">Data Collection</h4>
                                    <v-select dense
                                              v-model="EditOutcome.DatacollectionType"
                                              :items="DataCollectionTypeItems"
                                              label="Select Outcome Type"
                                              outlined
                                              @change="fnGetLogicType(EditOutcome.DatacollectionType)">
                                    </v-select>
                                    <v-select dense
                                              :items="SurveyList"
                                              :item-text="(i) => i.SurveyName"
                                              :item-value="(i) => i.SurveyID"
                                              label="Select Survey"
                                              v-model="EditOutcome.SurveyId"
                                              @change="GetSectionsBySurveyID(EditOutcome.SurveyId)"
                                              outlined
                                              :rules="[rules.required]">
                                    </v-select>
                                    <div v-if="EditOutcome.DatacollectionType == 'Quantitative'">
                                        <!-- Section List-->
                                        <v-select :items="SectionList"
                                                  label="Select Section"
                                                  item-text="SectionName"
                                                  item-value="SectionID"
                                                  outlined
                                                  dense
                                                  v-model="EditOutcome.SurveySectionId"
                                                  @change="GetQstnBySectionID(EditOutcome.SurveySectionId)"></v-select>
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
                                    <div v-if="EditOutcome.SurveyLogic == 1 || EditOutcome.SurveyLogic == 2 ||logicAnsType == 'Single Line Text' || logicAnsType == 'Numeric'">
                                        <!-- Target-->
                                        <v-text-field outlined
                                                      dense
                                                      v-model="EditOutcome.SurveyTarget"></v-text-field>
                                    </div>
                                    <div v-else>
                                        <v-select v-show="EditOutcome.DatacollectionType == 'Quantitative'"
                                                  dense
                                                  :items="lstLogicAnswers"
                                                  v-model="EditOutcome.DefineTarget"
                                                  item-text="OptionValue"
                                                  item-value="OptionID"
                                                  label="Define Target"
                                                  outlined="">
                                        </v-select>
                                    </div>

                                    <v-select hide-details
                                              dense=""
                                              :items="lstPeriodicity"
                                              :item-text="(i) => i.Value"
                                              :item-value="(i) => i.Key"
                                              label="Select Reporting Frequency"
                                              outlined=""
                                              v-model="EditOutcome.Periodicity"
                                              :rules="[rules.required]">
                                    </v-select>
                                    <!-- Once -->
                                    <div v-show="EditOutcome.Periodicity === 2">
                                        <v-menu v-model="periodicityDateMenuEdit"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y
                                                max-width="290px"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="FormattedDate12"
                                                              label="Choose a Date"
                                                              persistent-hint
                                                              outlined
                                                              v-on="on"
                                                              class="mt-4"
                                                              hide-details
                                                              dense></v-text-field>
                                            </template>
                                            <v-date-picker v-model="periodicityDateEdit"
                                                           no-title
                                                           @input="periodicityDateMenuEdit = false"
                                                           dense
                                                           :min="startDate"
                                                           :max="endDate"></v-date-picker>
                                        </v-menu>
                                    </div>
                                    <!-- Weekly-->
                                    <div v-show="EditOutcome.Periodicity === 4" class="mt-4">
                                        <v-select dense=""
                                                  :items="lstWeek"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a day"
                                                  outlined=""
                                                  v-model="EditOutcome.ReportingWeek"
                                                  hide-details>
                                        </v-select>
                                    </div>
                                    <!-- Monthly-->
                                    <div v-show="EditOutcome.Periodicity === 5" class="mt-4">
                                        <v-select dense=""
                                                  :items="lstMonthDate"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Date"
                                                  outlined=""
                                                  v-model="EditOutcome.ReportingMonthDay"
                                                  hide-details>
                                        </v-select>
                                    </div>
                                    <!-- Quaterly , Halyearly and Yearly-->
                                    <div class="d-inline-flex mt-4"
                                         v-if="EditOutcome.Periodicity == 6 || EditOutcome.Periodicity == 7 || EditOutcome.Periodicity == 8">
                                        <v-select hide-details
                                                  class="mr-1 mb-4"
                                                  dense=""
                                                  :items="lstMonth"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Month"
                                                  outlined=""
                                                  v-model="EditOutcome.ReportingMonth">
                                        </v-select>
                                        <v-select class="mb-4"
                                                  :items="lstMonthDate"
                                                  :item-text="(i) => i.Value"
                                                  :item-value="(i) => i.Key"
                                                  label="Choose a Date"
                                                  outlined=""
                                                  v-model="EditOutcome.ReportingMonthDateDay">
                                        </v-select>
                                    </div>
                                    <br />
                                    <!--<v-select dense=""
                                              :items="workflows"
                                              :item-text="(i) => i.WfName"
                                              :item-value="(i) => i.WorkflowID"
                                              label="Select Workflow"
                                              outlined=""
                                              v-model="EditOutcome.WorkflowId">
                                    </v-select>-->
                                    <!-- User List-->
                                    <v-autocomplete v-model="EditOutcome.UserList"
                                                    :items="lstUserList"
                                                    outlined
                                                    dense
                                                    chips
                                                    small-chips
                                                    label="Select Users"
                                                    item-text="Name"
                                                    item-value="ID"
                                                    hide-details
                                                    class="mb-2"
                                                    multiple
                                                    :rules="[rules.required]"
                                                    return-object="">
                                        <template v-slot:selection="data">
                                            <v-chip v-bind="data.attrs"
                                                    :input-value="data.selected"
                                                    close
                                                    small
                                                    @click="data.select"
                                                    @click:close="fnRemoveEditUser(data.item)">
                                                <v-avatar size="12"
                                                          left
                                                          light
                                                          :color="`${commonGetColor(data.item.Name)}`">
                                                    <img v-bind:src="userimage + data.item.ProfileImage"
                                                         alt="User DP"
                                                         v-if="data.item.ProfileImage.length > 0" />
                                                    <span class="white--text" v-else>
                                                        {{ data.item.Name == null ? "" : data.item.Name.substring(0, 1).toUpperCase() }}
                                                    </span>
                                                </v-avatar>
                                                {{ data.item.Name }}
                                            </v-chip>
                                        </template>
                                        <template v-slot:item="data">
                                            <v-list-item-action>
                                                <v-checkbox @change="fnGetSelectedUserList(data.item.ID)"
                                                            v-model="data.item.IsSelected"></v-checkbox>
                                            </v-list-item-action>
                                            <v-list-item-icon>
                                                <v-avatar size="30"
                                                          class="elevation-1"
                                                          light
                                                          :color="`${commonGetColor(data.item.Name)}`">
                                                    <img v-bind:src="userimage + data.item.ProfileImage"
                                                         alt="User DP"
                                                         v-if="data.item.ProfileImage.length > 0" />
                                                    <span class="white--text" v-else>
                                                        {{ data.item.Name == null ? "" : data.item.Name.substring(0, 1).toUpperCase() }}
                                                    </span>
                                                </v-avatar>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                <v-list-item-title v-html="data.item.Name"
                                                                   class="ml-2"></v-list-item-title>
                                            </v-list-item-content>
                                        </template>
                                    </v-autocomplete>
                                </v-col>
                            </v-row>
                        </v-card-text>
                    </v-form>
                    <v-card-actions class="pb-4">
                        <v-col class="text-right">
                            <Secondary-Button id="AutoTest_IP578"
                                              :disabled="waitforsave"
                                              class="btn-122x36 mr-2"
                                              title="Confirm"
                                              @click.native="fnUpdateOutcome()"></Secondary-Button>
                            <Default-Button-Outlined id="AutoTest_IP579"
                                                     class="btn-122x36"
                                                     title="Cancel"
                                                     @click.native="fnClearFeilds()"></Default-Button-Outlined>
                        </v-col>
                    </v-card-actions>
                </v-card>
            </v-dialog>

            <!-- Remove Outcome-->
            <v-dialog v-model="RemoveIndividualOutcomedlg" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                        </v-avatar>
                        Remove Outcome
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP593" text="" @click="RemoveIndividualOutcomedlg =false,OutcomeName=''">
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
                                                <h4 class="text-center heading-3">Are you sure you want to remove this Outcome ?</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-row justify="center">
                                                    <h5 class="page-head mt-4">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                        </v-avatar>
                                                        {{OutcomeName}}
                                                    </h5>
                                                </v-row>

                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_IP594" color="secondary elevation-0" class="mr-2" @click="fnRemoveOutcomes(2)">Remove</v-btn>
                                                <v-btn id="AutoTest_IP595" outlined color="grey" @click="RemoveIndividualOutcomedlg=false,OutcomeName=''">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <v-row>
                <v-col cols="12" class="v-scrolling-div" style="max-height:40vh">
                    <v-expansion-panels flat>
                        <v-expansion-panel class="mb-4 pa-2"
                                           v-for="(item,index) in OutcomeList"
                                           :key="index">
                            <v-expansion-panel-header class="text--secondary py-0 px-0">
                                <v-avatar max-width="20">
                                    <v-icon color="secondary" size="20" class="mr-2">
                                        mdi-calendar-text
                                    </v-icon>
                                </v-avatar>
                                {{item.OutcomeName}}
                                <v-spacer></v-spacer>
                                <v-btn fab x-small depressed class="mr-2" max-width="20" @click.native.stop="fnTriggerEditUpdateOutcome(item)">
                                    <v-icon color="#707070" size="15">mdi-pencil</v-icon>
                                </v-btn>
                                <v-btn fab x-small depressed class="mr-2" max-width="20" @click.native="fnRowActionRemoveClick(item)">
                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                </v-btn>
                            </v-expansion-panel-header>
                            <v-expansion-panel-content class="px-9">
                                <v-row>
                                    <v-col cols="auto">
                                        <p class="black--text mb-0">Start Date</p>
                                        <p class="text--secondary mb-0">{{formatDate(item.StartDate.substr(0, 10))}}</p>
                                    </v-col>

                                    <v-col cols="auto">
                                        <p class="black--text mb-0">End Date</p>
                                        <p class="text--secondary mb-0">{{formatDate(item.EndDate.substr(0, 10))}}</p>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="auto">
                                        <p class="black--text mb-0">Periodicity</p>
                                        <p class="text--secondary mb-0">{{item.ReportingFrequencyName}}</p>
                                    </v-col>

                                    <v-col cols="auto">
                                        <p class="black--text mb-0">Target</p>
                                        <p class="text--secondary mb-0">{{item.DatacollectionType}}</p>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12">
                                        <p class="black--text mb-0">Survey</p>
                                        <p class="text--secondary mb-0">{{item.SurveyName}}</p>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12">
                                        <p class="black--text mb-0">Impact Indicators</p>
                                    </v-col>
                                    <v-col cols="12" class="py-0" v-for="SDGItem in item.SDGList" :key="item.target">
                                        <v-row>
                                            <v-col cols="auto">
                                                <v-card class="elevation-0" width="50" height="50" align="center">
                                                    <img src="../../images/SDG-edu.jpg" width="50" height="50" />
                                                </v-card>
                                            </v-col>
                                            <v-col cols="auto">
                                                <p class="black--text mb-0">{{SDGItem.Targets.length}} Indicators</p>
                                                <v-btn text small color="secondary" @click="SDGItem.ViewAllPanel=!SDGItem.ViewAllPanel" class="px-0">View All</v-btn>
                                            </v-col>
                                            <v-col cols="12" v-if="SDGItem.ViewAllPanel" class="expansion-panel-flat">
                                                <v-expansion-panels flat>
                                                    <v-expansion-panel v-for="(Titem,i) in SDGItem.Targets"
                                                                       :key="i">
                                                        <v-expansion-panel-header class="black--text pb-0">
                                                            {{Titem.Target_Name}}
                                                        </v-expansion-panel-header>
                                                    </v-expansion-panel>
                                                </v-expansion-panels>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-expansion-panel-content>
                        </v-expansion-panel>
                    </v-expansion-panels>

                </v-col>
            </v-row>
        </v-col>

        <v-col cols="12" align="right">
            <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="GoToCreateNewProjectStepsFourFn"></Secondary-Button>
            <v-btn depressed="" class="btn-122x36 mr-4" height="37" v-on:click="GoToCreateNewProjectStepsTwoFn">Previous</v-btn>
            <v-btn depressed="" class="btn-122x36" height="37" v-on:click="GoToCreateNewProjectStepsFourFn">Skip for Now</v-btn>
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
    </v-row>
</template>
<script>
    import SuccessButton from '@/components/success-button.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '../../utils.js'

    var objManageOutcomes;
    async function importOutcomesscript() {
        return Promise.all([
            import("../../BL/Outcomes.js").then(mod => {
                objManageOutcomes = new mod.Outcomes();
            })
        ]);
    };
    var objManageProjectActivity;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };
    var objManageProject;
    async function importManageProjectscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
  
  export default{
    props:[
    'CreateNewProjectStepsProp', 'CreateNewProjectDialogProp'
  ],
  
    data : function(){
      return{
          AddImpactIndicator:false,
          impactError:false,
          EditImpactIndicator:false,
          AddImpactIndicatorTarget:false,
          targetData:[],
          targetId:'',
           imagesInput:{
        1:require("@/assets/createOutcome/1.png"),
        2:require("@/assets/createOutcome/2.png"),
        3:require("@/assets/createOutcome/3.png"),
        4:require("@/assets/createOutcome/4.png"),
        5:require("@/assets/createOutcome/5.png"),
        6:require("@/assets/createOutcome/6.png"),
        7:require("@/assets/createOutcome/7.png"),
        8:require("@/assets/createOutcome/8.png"),
        9:require("@/assets/createOutcome/9.png"),
        10:require("@/assets/createOutcome/10.png"),
        11:require("@/assets/createOutcome/11.png"),
        12:require("@/assets/createOutcome/12.png"),
        13:require("@/assets/createOutcome/13.png"),
        14:require("@/assets/createOutcome/14.png"),
        15:require("@/assets/createOutcome/15.png"),
        16:require("@/assets/createOutcome/16.png"),
        17:require("@/assets/createOutcome/17.png"),
      },
          OutcomeList:[],
          //ImpactIndicators: [],
          OutcomeDetailsTableItems: [],
          // Outcomes Data Collection
          //DataCollectionType: 'Non Target',
          //DataCollectionTypeItems: ['Target', 'Non Target'],
          
          DataCollectionType: "Qualitative",
          DataCollectionTypeItems: ["Quantitative", "Qualitative"], //['Target', 'Non Target'],

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
          lstUserList: [],
          lstUserListData: [],
          SelectedUserList: '',
          // --- outcome --
          AddCoutcomesdlg: false,
          lstSGD: [],
          lstSDGTargets: [],
          //lstSDGTargetIndicators: [],
          //SearchOutcomeDetails: '',

          // -- 
          lstSGDData: [],
          lstSDGTargetsData: [],
          lstSDGTargetIndicatorsData: [],

          SelectedSDG: [],
          SelectedSDGTargets: [],
          SelectedSDGTargetIndicators: '',
          //ResponseList: [],
          ResponseID: 0,
          GroupId: 0,
          OutcomeStmt: '',
  
          waitforsave: false,
          // Periodicity
          lstPeriodicity: [],
          lstMonthDate: [],
          lstMonth: [],
          lstWeek: [],
          ReportingFrequency: 0,
          ReportingMonthDay: 0,
          ReportingMonth: 0,
          ReportingMonthDateDay: 0,
          ReportingWeek: 0,
          periodicityDateMenu: false,
          periodicityDate: new Date().toISOString().substr(0, 10),
          periodicityDateEdit: new Date().toISOString().substr(0, 10),
          rules: {
              required: value => !!value || 'Required.',
          },
          workflows: [],
          Workflow: 0,
          offset: true,
          closeonClick: false,
          closeOnContentClick: false,

          startDate: new Date().toISOString().substr(0, 10),
          FrmDateMenu: false,

          // ToDate
          endDate: new Date().toISOString().substr(0, 10),
          ToDateMenu: false,
          ProjectStartDate: "",
          ProjectEndDate: "",

          EditCoutcomesdlg: false,
          
          // Edit Update Outcome
          OutcomeInfo: {
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
              QuestionName: '',
              SectionName: '',
              //SurveyLogicName: '',
              //SurveyTargetName: ''
          },

          vaMultiple: true,
          FrmDateMenuEdit : false,
          ToDateMenuEdit: false,
          PreLoader: false,
          periodicityDateMenuEdit: false,


          
          RemoveIndividualOutcomedlg : false,
          OutcomeName: '',
          OutcomeId : 0,


          // Tags ------------
          NewTagMenu: false,
          AllTagListSteps: 0,
          AllTagList: [],
          TagHeaderName: "Add Tag",
          SearchTag: "",
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
          SelectedTagID: 0,
          IsSearchTagExist: false,
       // Tags Ends here
          userimage: "",
       // snackbar
          Msg: "",
          successSnackbarMsg: false,
          errorSnackbarMsg: false,
          infoSnackbarMsg: false,
           impactIndicatorHeader: [
                    {
                        text: 'Select Impact Indicator',
                        align: 'left',
                        value: 'impact',
                    }
                ],
                addImpactTarget:false,
                impactTargetIndicatorHeader: [
                    {
                        text: 'Select Targets',
                        align: 'left',
                        value: 'impact',
                    }
                ],
      }
    },
    
     components:{
           'Success-Button':SuccessButton,   
           'Secondary-Button': SecondaryButton,
           'Pre-Loader': PreLoader,
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
            FormattedDate12() {
                return this.formatDate(this.periodicityDateEdit)
            },
      },
      mounted: async function () {
            
            this.WorkspaceID = this.$route.query.WsID;
            this.ProjectId = window.ProjectId;//this.$route.query.ProjID;
            this.ProjectStartDate = window.ProjectStartDate;
            this.ProjectEndDate = window.ProjectEndDate;
            this.InitiativeId = this.$route.query.InitID;
            this.userimage = window.SERVER_URL + "/Upload";
            await importscript();
            await importOutcomesscript();
            await importManageProjectscript();
            this.PreLoader = true;

            await this.GetAllSDG();
            await this.FetchAllSDGTargets();
            //await this.FetchAllSDGTargetsIndicators();
            await this.fnGetProjectSurveyList();
            await this.GetReportingFrequencyList();
            await this.fnGetAllTags();
            this.PreLoader = false;
      },
      methods: {

        /// <summary>
        ///  fnSelectedTargetCountPerImpact :- Function used to show count of selected targets
        /// </summary>
        /// <param name="item"></param>
        /// <returns> Count </returns>
        fnSelectedTargetCountPerImpact(item) {
          var Count = 0;
          Count = this.SelectedSDGTargets.filter(x => x.fk_SDGID === item.pk_SDGID).length;
          return Count;
        },

        /// <summary>
        ///  storeTargets :- Function used to select impact targets
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns> </returns>
        async storeTargets(target,index) {
            var temp1 = this.SelectedSDGTargets.filter((x) => x.Target_Name == target.Target_Name);
            if (temp1.length == 0) {
                 this.SelectedSDGTargets = [...this.SelectedSDGTargets,target]
                 this.AddImpactIndicatorTarget=false;
            }
            else {
                 if (temp1.length > 0) {
                      this.SelectedSDGTargets.splice(this.SelectedSDGTargets.indexOf(temp1[0]), 1);
                      this.SelectedSDGTargets = [...this.SelectedSDGTargets];
                 }
            }
        },

        /// <summary>
        ///  submitHandler :- Function used to save SDG data
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        submitHandler() {
            if(this.lstSGDData.length == 0){
                this.impactError = true;
            }
            else if (this.lstSGDData.length > 0) {
                this.impactError= false;
                this.EditImpactIndicator = false;
                this.AddImpactIndicator = false;
            }
        },

        /// <summary>
        ///  submitHandlerTarget :- Function used to save selected SDG targets
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        submitHandlerTarget(item) {
            var target = this.SelectedSDGTargets.filter(e=> e.fk_SDGID == this.targetId);
            if(target.length > 0){
                this.addImpactTarget= false;
                this.AddImpactIndicatorTarget = false;
            }
            else {
                this.AddImpactIndicatorTarget= true;
            }
        },

        /// <summary>
        ///  GoToCreateNewProjectStepsFourFn :- Function used to go to new project step four (next step)
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        GoToCreateNewProjectStepsFourFn(){
            this.CreateNewProjectStepsProp=4;
            this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
        },
      
        /// <summary>
        ///  GoToCreateNewProjectStepsTwoFn :- Function used to go to new project step two (previous step)
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        GoToCreateNewProjectStepsTwoFn(){
            this.CreateNewProjectStepsProp=2;
            this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
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
        ///  commonGetColor :- Function used to get color
        /// </summary>
        /// <param name="name"></param>
        /// <returns> </returns>
        commonGetColor(name) {
            return objUtils.common.getcolor(name);
        },

        /// <summary>
        ///  GetAllSDG :- Function used to fetch all SDG list
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
        /// <param name="name"></param>
        /// <returns> </returns>
        async GetSDGTargetsBasedOnSDGID(id,name) {
           var temp = this.lstSGD.filter((x) => x.pk_SDGID == id);
           var temp1 = this.lstSGDData.filter((x) => x.pk_SDGID == id);
           if (temp.length > 0 && temp1.length == 0) {
               // this.lstSGDData.push(temp[0]);
               for (var SDGCnt = 0; SDGCnt <= temp.length - 1; SDGCnt++) {
                   var object = new Object();
                   object.pk_SDGID = temp[SDGCnt].pk_SDGID;
                   object.SDG_Name = temp[SDGCnt].SDG_Name;
                   object.Status = temp[SDGCnt].Status;
                   object.Targets = await this.GetTargets(object.pk_SDGID);
                   this.lstSGDData.push(object);
                   this.impactError = false;
               }
           }
           else {
               if (temp1.length > 0) {
                   this.lstSGDData.splice(this.lstSGDData.indexOf(temp1[0]), 1);
                   this.SelectedSDGTargets = SelectedSDGTargets.filter(e=> e.Target_Name != name)
                   this.lstSGDData = [...this.lstSGDData];
                   this.SelectedSDGTargets = [];
               }
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
        ///  fnGetSelectedUserList :- Function used to fetch selected user list
        /// </summary>
        /// <param name="id"></param>
        /// <returns> </returns>
        async fnGetSelectedUserList(id) {
            var temp = this.lstUserListData.filter(x => x.ID == id)
            if (temp.length == 0) {
                var object = new Object();
                object.ID = id;
                object.Name = "";
                object.EmailId = "";
                object.RoleID = 0;
                object.Role = "";
                object.RoleType = "";
                object.ProjectCount = 0;
                this.lstUserListData.push(object);
            } else if (temp.length > 0) {
                this.lstUserListData.splice(this.lstUserListData.indexOf(temp[0]), 1);
                this.lstUserListData = [...this.lstUserListData];
                // this.SelectedSDGTargets = ''
            }
        },

        /// <summary>
        ///  FetchAllOutcomeList :- Function used to fetch all outcome list based on ProjectId
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async FetchAllOutcomeList() {
            if (this.ProjectId > 0)
            {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageOutcomes.FetchAllOutcomeList(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.OutcomeDetailsTableItems = result.data;
                        await this.GetSDGIndicators();
                    }
            }
        },

        /// <summary>
        ///  GetSDGIndicators :- Function used to fetch SDG indicators
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async GetSDGIndicators() {
            this.OutcomeList = [];
            if (this.OutcomeDetailsTableItems.length > 0) {
                for (var i = 0; i<= this.OutcomeDetailsTableItems.length - 1; i++) {
                    var object = new Object();
                    object.pk_OutcomeID = this.OutcomeDetailsTableItems[i].pk_OutcomeID;
                    object.OutcomeName = this.OutcomeDetailsTableItems[i].OutcomeName;
                    object.StartDate = this.OutcomeDetailsTableItems[i].StartDate;
                    object.EndDate = this.OutcomeDetailsTableItems[i].EndDate;
                    object.DatacollectionType = this.OutcomeDetailsTableItems[i].DatacollectionType;
                    object.SurveyName = this.OutcomeDetailsTableItems[i].SurveyName;
                    object.WorkflowName = this.OutcomeDetailsTableItems[i].WorkflowName;
                    object.ReportingFrequencyName = this.OutcomeDetailsTableItems[i].ReportingFrequencyName;
                    object.SDGList = await this.GetSDGList(object.pk_OutcomeID)
                    this.OutcomeList.push(object);
                   
                }
            }
        },

        /// <summary>
        ///  GetSDGList :- Function used to fetch SDG list based on OutcomeId
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <returns> SDGIndicators </returns>
        async GetSDGList(OutcomeId) {
            var OutcomeItem = this.OutcomeDetailsTableItems.filter(x => x.pk_OutcomeID == OutcomeId);
            var SDGDataList = [];
            
            if (OutcomeItem.length > 0) {
                SDGDataList = JSON.parse(OutcomeItem[0].SDGList);
                var SDGIndicators = [];
                for (var i = 0; i <= SDGDataList.length - 1; i++) {
                    var object = new Object();
                    object.pk_SDGID = SDGDataList[i].pk_SDGID;
                    object.SDG_Name = SDGDataList[i].SDG_Name;
                    object.Status = SDGDataList[i].Status;
                    object.ViewAllPanel = false;
                    object.Targets = await this.GetTargetsWithId(object.pk_SDGID, OutcomeId)
                    SDGIndicators.push(object);
                }
            }
            return SDGIndicators;
        },

        /// <summary>
        ///  GetTargetsWithId :- Function used to fetch target list based on id and OutcomeId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="OutcomeId"></param>
        /// <returns> lstTargetsData </returns>
        async GetTargetsWithId(id, OutcomeId) {
            var lstTargetsData = [];
            var OutcomeItem = this.OutcomeDetailsTableItems.filter(x => x.pk_OutcomeID == OutcomeId);
            var lstTarget = JSON.parse(OutcomeItem[0].SDGTargetsList);
            var Targets = lstTarget.filter(x => x.fk_SDGID == id);
            
            for (var i = 0; i <= Targets.length - 1; i++) {
                var object = new Object();
                object.pk_SDGTargetID = Targets[i].pk_SDGTargetID;
                object.fk_SDGID = Targets[i].fk_SDGID;
                object.Target_Name = Targets[i].Target_Name;
                object.Status = Targets[i].Status;
                lstTargetsData.push(object);
            }

            return lstTargetsData;
        },


        //----------- Edit and Update Outcomes ---------------- 
        /// <summary>
        ///  fnTriggerEditUpdateOutcome :- Function used to fetch Outcome details based on Outcome Id and for assigning variables for edit
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        async fnTriggerEditUpdateOutcome(item) {
            await this.FetchAllOutcomeListBasedOnOutComeId(item.pk_OutcomeID);
            await this.fnEditOutcomeClick();
            this.EditCoutcomesdlg = true;
            this.vaMultiple = false;
        },

        /// <summary>
        ///  FetchAllOutcomeListBasedOnOutComeId :- Function used to fetch all outcome list based on OutcomeId
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <returns> </returns>
        async FetchAllOutcomeListBasedOnOutComeId(OutcomeId) {
            var data = new FormData();
            data.append("OutcomeId", OutcomeId);
            data.append("ProjectId", this.ProjectId);
            var result = await objManageOutcomes.FetchAllOutcomeListBasedOnOutComeId(data);

            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            } else if (result.data) {
                this.OutcomeInfo = result.data;
                this.OutcomeInfo.UserList = this.OutcomeInfo.lstUserList;
                this.OutcomeInfo.lstSDGList = JSON.parse(result.data.SDGList);
                this.OutcomeInfo.lstSDGTargetsList = JSON.parse(result.data.SDGTargetsList);
                this.lstSGDData = this.OutcomeInfo.lstSDGList;
                this.lstSDGTargetsData = this.OutcomeInfo.lstSDGTargetsList;

                if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                    this.OutcomeInfo.ReportingOnceDate = result.data.ReportingOnceDate;
                }
                if (result.data.Tags != null && result.data.Tags != undefined && result.data.Tags != "") {
                    this.SelectedTag = JSON.parse(result.data.Tags);
                }
                // await this.GetSDGTargetAndIndicators()
            }
        },

        /// <summary>
        ///  fnEditOutcomeClick :- Function used to edit outcome 
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnEditOutcomeClick() {
            await this.fnGetSavedSDGDetailsForEdit();
            this.EditOutcome.pk_OutcomeID = this.OutcomeInfo.pk_OutcomeID;
            this.EditOutcome.OutcomeName = this.OutcomeInfo.OutcomeName;
            this.EditOutcome.UserList = this.OutcomeInfo.UserList;
            this.lstUserListData = this.OutcomeInfo.UserList;
            this.EditOutcome.Periodicity = this.OutcomeInfo.Periodicity;
            this.EditOutcome.ReportingMonthDay = this.OutcomeInfo.ReportingMonthDay;
            this.EditOutcome.ReportingMonth = this.OutcomeInfo.ReportingMonth;
            this.EditOutcome.ReportingMonthDateDay = this.OutcomeInfo.ReportingMonthDateDay;
            this.EditOutcome.ReportingWeek = this.OutcomeInfo.ReportingWeek;
            this.EditOutcome.DatacollectionType = this.OutcomeInfo.DatacollectionType;
            this.EditOutcome.SurveyId = this.OutcomeInfo.SurveyId;

            if (this.OutcomeInfo.SurveyId != 0) {
                await this.GetSectionsBySurveyID(this.EditOutcome.SurveyId );
            }
            this.EditOutcome.SurveySectionId = this.OutcomeInfo.SurveySectionId;
            if (this.EditOutcome.SurveySectionId != 0) {
                await this.GetQstnBySectionID(this.EditOutcome.SurveySectionId);
            }
            this.EditOutcome.SurveyQuestionId = this.OutcomeInfo.SurveyQuestionId;
            if (this.EditOutcome.SurveyQuestionId> 0) {
                await this.GetResponsesByQstnid(this.EditOutcome.SurveyQuestionId);
            }
            this.EditOutcome.ResponseID = this.OutcomeInfo.ResponseID;
            this.EditOutcome.SurveyLogic = this.OutcomeInfo.SurveyLogic;
           
            if (
                this.EditOutcome.SurveyLogic == 1 ||
                this.EditOutcome.SurveyLogic == 2 ||
                this.logicAnsType == "Single Line Text" ||
                this.logicAnsType == "Numeric"
            ) {
                this.EditOutcome.SurveyTarget = this.OutcomeInfo.SurveyTarget;
            } else {
                this.EditOutcome.DefineTarget = parseInt(this.OutcomeInfo.SurveyTarget);
            }

            this.EditOutcome.WorkflowId = this.OutcomeInfo.WorkflowId;
            this.periodicityDate = this.formatDate(this.OutcomeInfo.ReportingOnceDate);
            this.periodicityDateEdit = this.formatDate(this.OutcomeInfo.ReportingOnceDate);
            //this.EditOutcome.ReportingOnceDate = this.FormattedDate12(this.OutcomeInfo.ReportingOnceDate);
            this.startDate = this.OutcomeInfo.StartDate.substr(0, 10);
            this.endDate = this.OutcomeInfo.EndDate.substr(0, 10);

            this.GetSectionsBySurveyID(this.OutcomeInfo.SurveyId);
            this.GetQstnBySectionID(this.OutcomeInfo.SurveySectionId);
            //this.GetResponsesByQstnid(this.OutcomeInfo.SurveyQuestionId);
            this.fnGetLogicType(this.OutcomeInfo.DatacollectionType);
        },

        //async fnGetSelectedTargetsDetails(id, SDGTargetID) {
        //    var temp = this.lstSDGTargetsData.filter(x => x.pk_SDGTargetID == SDGTargetID)
        //    if (temp.length == 0) {
        //        var object = new Object();
        //        object.fk_SDGID = id;
        //        object.pk_SDGTargetID = SDGTargetID;
        //        object.Status = 0;
        //        object.Target_Name = "";
        //        this.lstSDGTargetsData.push(object);
        //    } else if (temp.length > 0) {
        //        this.lstSDGTargetsData.splice(this.lstSDGTargetsData.indexOf(temp[0]), 1);
        //        this.lstSDGTargetsData = [...this.lstSDGTargetsData];
        //        // this.SelectedSDGTargets = ''
        //    }
        //},

        /// <summary>
        ///  fnGetSavedSDGDetailsForEdit :- Function used to fetch saved SDG details for edit
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnGetSavedSDGDetailsForEdit() {
            var Selected = false;
            var MainList = [];
            if (this.lstSGD.length > 0) {
                for (var i = 0; i <= this.lstSGD.length - 1; i++) {
                    //this.SelectedSDG.push(this.lstSGDData[i].pk_SDGID);
                    if (this.lstSGDData != null) {
                        var temp = this.lstSGDData.filter(
                            (x) => x.pk_SDGID == this.lstSGD[i].pk_SDGID
                        );
                        if (temp.length > 0) {
                            Selected = true;
                        } else {
                            Selected = false;
                        }
                    }
                    var object = new Object();
                    object.pk_SDGID = this.lstSGD[i].pk_SDGID;
                    object.SDG_Name = this.lstSGD[i].SDG_Name;
                    object.Selected = Selected;
                    object.Status = this.lstSGD[i].Status;
                    //object.Targets = await this.fnGetSavedSDGTargetsBasedOnSDG(object.pk_SDGID)
                    MainList.push(object);
                }
            }
            this.lstSGD = [];
            this.lstSGD = MainList;

            ///-------------------
            Selected = false;
            var List = [];
            if (this.lstSGDData != null) {
                if (this.lstSGDData.length > 0) {
                    for (var i = 0; i <= this.lstSGDData.length - 1; i++) {
                        this.SelectedSDG.push(this.lstSGDData[i].pk_SDGID);
                        var object = new Object();
                        object.pk_SDGID = this.lstSGDData[i].pk_SDGID;
                        object.SDG_Name = this.lstSGDData[i].SDG_Name;
                        //object.Selected = Selected;
                        object.Status = this.lstSGDData[i].Status;
                        object.Targets = await this.fnGetSavedSDGTargetsBasedOnSDG(object.pk_SDGID)
                        List.push(object);
                    }
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
          
            for (var i = 0; i <= Targets.length - 1; i++) {
                if (this.lstSDGTargetsData != null) {
                    var temp = this.lstSDGTargetsData.filter(x => x.pk_SDGTargetID == Targets[i].pk_SDGTargetID);
                    if (temp.length > 0) {
                        Selected = true;
                        // this.SelectedSDGTargets.push(Targets[i].pk_SDGTargetID);
                        this.SelectedSDGTargets.push(temp[0]);
                    } else { Selected = false; }
                }

                var object = new Object();
                object.pk_SDGTargetID = Targets[i].pk_SDGTargetID;
                object.fk_SDGID = Targets[i].fk_SDGID
                object.Target_Name = Targets[i].Target_Name
                object.Status = Targets[i].Status
                object.Selected = Selected;
                lstTargetsData.push(object);
            }

            return lstTargetsData;
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
            //data.append("Description", this.Description);
            data.append("Periodicity", this.EditOutcome.Periodicity);
            data.append("Startdate", this.startDate);
            data.append("Enddate", this.endDate);

            data.append("SDGList", JSON.stringify(this.lstSGDData));
            data.append("SDGTargetsList", JSON.stringify(this.SelectedSDGTargets));
            data.append("WorkflowId", this.EditOutcome.WorkflowId);
            data.append("DatacollectionType", this.EditOutcome.DatacollectionType);
            data.append("SurveyId", this.EditOutcome.SurveyId);

            if (this.EditOutcome.DatacollectionType == "Quantitative") {
                data.append("SurveySectionId", this.EditOutcome.SurveySectionId);
                data.append("SurveyQuestionId", this.EditOutcome.SurveyQuestionId);
                data.append("SurveyResponseId", 0);
                data.append("SurveyLogic", this.EditOutcome.SurveyLogic);
                if (this.EditOutcome.SurveyLogic == 1 || this.EditOutcome.SurveyLogic == 2 || this.EditOutcome.SurveyLogic == "Single Line Text" || this.logicAnsType == "Numeric") {
                    data.append("SurveyTarget", this.EditOutcome.SurveyTarget);
                } else {
                    data.append("SurveyTarget", this.EditOutcome.DefineTarget);
                }
            } else {
                data.append("SurveySectionId", 0);
                data.append("SurveyQuestionId", 0);
                data.append("SurveyResponseId", 0);
                data.append("SurveyLogic", "");
                data.append("SurveyTarget", "");
            }
            data.append("ProjectID", this.ProjectId);
            data.append("UserList", JSON.stringify(this.EditOutcome.UserList));
            data.append("Tags", JSON.stringify(this.SelectedTag));

            data.append("ReportingMonth", this.EditOutcome.ReportingMonth);
            data.append("ReportingMonthDay", this.EditOutcome.ReportingMonthDay);
            data.append("ReportingWeek", this.EditOutcome.ReportingWeek);
            data.append("ReportingOnceDate", this.formatDate(this.periodicityDateEdit));
            data.append("ReportingMonthDateDay", this.EditOutcome.ReportingMonthDateDay);
            var result = await objManageOutcomes.UpdateOutcome(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            } else if (result.status == "Success") {
                this.FetchAllOutcomeList();
                await this.fnClearFeilds();
                this.Msg = "Outcome details updated successfully";
                this.successSnackbarMsg = true;
            } else {
                if (result.status == "Failed") {
                    this.Msg = result.RFStatus;
                    this.errorSnackbarMsg = true;
                } else {
                    this.Msg = "Failed";
                    this.errorSnackbarMsg = true;
                }
                //this.Msg = "Outcome details updated successfully";
                //this.successSnackbarMsg = true;
            }
            this.PreLoader = false;
        },

        //----Outcome Edit Update Ends here -----------------------

        /// <summary>
        ///  fnGetProjectSurveyList :- Function used to fetch project survey list
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnGetProjectSurveyList() {
            var result = await objManageProjectActivity.GetProjectSurveyList(this.ProjectId);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            } else if (result.data) {
                this.filterPaymentList = result.data;
                this.SurveyList = result.data;
            }
        },
       
        //------- Tag -------

        /// <summary>
        ///  fnGetAllTags :- Function used to fetch all tags
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnGetAllTags() {
            var result = await objManageProjectActivity.GetAllProjectTags(this.InitiativeID);
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
        ///  fnEditTagDetails :- Function used to edit existing tag details based on TagItem
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
        ///  UpdateTagDetails :- Function used to update tag details in edit mode based on color and TagName
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

            var result = await objManageProjectActivity.UpdateTagDetails(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
                if (result.status == "success") {
                    this.TagHeaderName = 'Add Tag';
                    this.AllTagListSteps = 2;
                    this.fnGetAllTags();
                    this.UpdatingTagItem = "";
                }
                else {
                    this.Msg = result.status;
                    this.errorSnackbarMsg = true;
                    return;
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
            data.append("InitiativeId", this.InitiativeID);
            var result = await objManageProjectActivity.CreateNewTag(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
                if (result.status == "success") {
                    this.SearchTagDetails = this.NewTagName;
                    this.NewTagName = "";
                    await this.fnGetAllTags();
                    this.filterAllTagList = this.AllTagList;
                    if (this.SearchTagDetails.length > 0)
                        this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                    if (this.filterAllTagList.length > 0) {
                        this.IsSearchTagExist = true;
                    }
                    this.AllTagListSteps = 2;
                }
                else {
                    this.Msg = "Tag already exists";
                    this.infoSnackbarMsg = true;
                }
        },

        /// <summary>
        ///  fnAddToSelectedTag :- Function used to add tag to selected tag items based on TagItem
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
                    else {

                        const index = this.SelectedTag.findIndex((element, index) => {
                            if (element.TagID === TagItem.TagID) {
                                return true;
                            }
                        });
                        this.SelectedTag.splice(index, 1);
                        if (this.SelectedTag.length == 0) {
                            this.allTags = false;
                        }
                    }
                } else {
                    this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                }
                this.TickSelectedTag();
                this.NewTagMenu = false;
                //this.NewTagMenuedit = false;
            }
        },

        /// <summary>
        ///  fnRemoveFromSelectedTagItem :- Function used to remove tag from selected tag item based on TagItem
        /// </summary>
        /// <param name="TagItem"></param>
        /// <returns> </returns>
        async fnRemoveFromSelectedTagItem(TagItem) {
            const index = this.SelectedTag.findIndex((element, index) => {
                if (element.TagID === TagItem.TagID) {
                    return true;
                }
            });
            this.SelectedTag.splice(index, 1);
            //this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
            //this.SelectedTag = [...this.SelectedTag];
            if (this.SelectedTag.length == 0) {
                this.allTags = false;
            }
            this.TickSelectedTag();
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
                this.SearchTagDetails = "";
                this.AllTagListSteps = 2;
            }
        },

        //------- End Tag ---------

        /// <summary>
        ///  fnGetLogicType :- Function used to fetch logic type based on data collection types (Quantitative / Qualitative)
        /// </summary>
        /// <param name="type"></param>
        /// <returns> </returns>
        async fnGetLogicType(type) {
            if (type == "Quantitative") {
                var result = await objManageProjectActivity.GetLogicType();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.LogicItems = result.data;
                    this.SurveyTarget = "";
                    this.DefineTarget = 0;
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
            this.DefineTarget = 0;
        },

        /// <summary>
        ///  fnGetTeamUsersList :- Function used to fetch team users list
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
                this.lstUserList = result.data;
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
                    this.GetResponsesByQstnid(this.SurveyQuestionId);
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
       
        // Methods

        /// <summary>
        ///  fnCheckMandatory :- Function used to check any mandatory field is showing blank or null
        /// </summary>
        /// <param name=""></param>
        /// <returns> true / false</returns>
        fnCheckMandatory() {
            if (this.OutcomeStmt == "" ||
                this.DataCollectionType == "" ||
                this.ReportingFrequency == 0 ||
                (this.ReportingFrequency == 2 && (this.periodicityDate == "" || this.periodicityDate == null)) ||
                this.startDate == "" ||
                this.endDate == "")// ||
                //this.SelectedUserList == 0)
                return true;
            else return false;
        },

        /// <summary>
        ///  resetValidation :- Function used to reset validation
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        resetValidation() {
            this.$refs.form.resetValidation()
        },

        /// <summary>
        ///  fnCreateOutcome :- Function used to create a new outcome
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnCreateOutcome() {
            this.PreLoader = true;
            var data = new FormData();
            data.append("pk_OutcomeID", this.OutcomeId);
            data.append("OutcomeName", this.OutcomeStmt);
            //data.append("Description", this.Description);
            data.append("Periodicity", this.ReportingFrequency);
            data.append("Startdate", this.startDate);
            data.append("Enddate", this.endDate);

            data.append("SDGList", JSON.stringify(this.lstSGDData))
            //data.append("SDGTargetsList", JSON.stringify(this.lstSDGTargetsData))
            data.append("SDGTargetsList", JSON.stringify(this.SelectedSDGTargets));
            data.append("WorkflowId", this.Workflow);

            data.append("DatacollectionType", this.DataCollectionType)
            data.append("SurveyId", this.SurveyId)

            if (this.DataCollectionType == "Quantitative") {
                data.append("SurveySectionId", this.SurveySectionId);
                data.append("SurveyQuestionId", this.SurveyQuestionId);
                data.append("SurveyResponseId", 0);
                data.append("SurveyLogic", this.SurveyLogic);
                
                if (this.SurveyLogic == 1 ||this.SurveyLogic == 2 ||this.logicAnsType == "Single Line Text" ||this.logicAnsType == "Numeric") {
                    data.append("SurveyTarget", this.SurveyTarget);
                } else {
                    data.append("SurveyTarget", this.DefineTarget);
                }
            } else {
                data.append("SurveySectionId", 0);
                data.append("SurveyQuestionId", 0);
                data.append("SurveyResponseId", 0);
                data.append("SurveyLogic", "");
                data.append("SurveyTarget", "");
            }
            data.append("ProjectID", this.ProjectId);
            data.append("UserList", JSON.stringify(this.lstUserListData));
            data.append("Tags", JSON.stringify(this.SelectedTag));
            data.append("ReportingMonth", this.ReportingMonth);
            data.append("ReportingMonthDay", this.ReportingMonthDay);
            data.append("ReportingWeek", this.ReportingWeek);
            data.append("ReportingOnceDate", this.FormattedDate);
            data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);
            
            var result = await objManageOutcomes.CreateOutcome(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            } else
                if (result.status == "Success") {
                    this.Msg = "Outcome details saved successfully";
                    this.successSnackbarMsg = true;
                    this.FetchAllOutcomeList();
                    await this.fnClearFeilds();

                }
                else {
                    if (result.status == "Failed") {
                        this.Msg = result.RFStatus;
                        this.errorSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed";
                        this.errorSnackbarMsg = true;
                    }
                    //this.snackbarMsg = true;

                }
            this.PreLoader = false;
        },

        /// <summary>
        ///  fnClearFeilds :- Function used to clear all fields
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnClearFeilds() {
            this.AddCoutcomesdlg = false;
            this.OutcomeId = 0
            this.EditCoutcomesdlg = false;
            this.OutcomeStmt = "";
            this.DataCollectionType = "Non Target";
            this.lstSGDData = [];
            this.lstTargetsData = [];
            this.lstUserListData = [];
            this.SelectedUserList = [];
            this.SelectedSDG = [];
            this.SelectedSDGTargets = [];

            this.SurveyId = 0;
            this.SurveySectionId = 0;
            this.Workflow = 0;
            this.SurveyQuestionId = 0;
            this.SurveyLogic = 0;
            this.SurveyTarget = 0;
            this.logicAnsType = "";
            this.ReportingFrequency = 0;
            this.resetValidation();
            this.ReportingMonth = 0;
            this.ReportingMonthDay = 0;
            this.ReportingWeek = 0;

            this.periodicityDate = new Date().toISOString().substr(0, 10);
            this.periodicityDateEdit = new Date().toISOString().substr(0, 10);
            this.startDate = new Date().toISOString().substr(0, 10);
            this.endDate = new Date().toISOString().substr(0, 10);
        },


        // ------------ Remove Outcome

        /// <summary>
        ///  fnRowActionRemoveClick :- Function used to Open the deletion dialog box for outcome, when clicking on the delete button from the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        async fnRowActionRemoveClick(item) {
            this.RemoveIndividualOutcomedlg = true;
            this.OutcomeId = item.pk_OutcomeID;
            this.OutcomeName = item.OutcomeName;
        },

        /// <summary>
        ///  fnRemoveOutcomes :- Function used to delete outcomes based on OutcomeId
        /// </summary>
        /// <param name="Type"></param>
        /// <returns> </returns>
        async fnRemoveOutcomes(Type) {
            var data = new FormData();
            data.append("Outcomes", JSON.stringify(this.OutcomeId));
            data.append("Type", 2);
            var result = await objManageOutcomes.DeleteOutcomes(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else {
                if (result.status = "success") {
                    this.RemoveIndividualOutcomedlg = false;
                    this.Msg = "Outcome Successfully removed";
                    this.successSnackbarMsg = true;
                }
            }
           // this.selectedOutcomes = [];
            this.FetchAllOutcomeList();
            this.OutcomeId = 0;
            this.OutcomeName = '';
        },

        /// <summary>
        ///  fnRemoveSDGIndicators :- Function used to remove section of SDG indicators
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        fnRemoveSDGIndicators(item) {
            for (var i = 0; i <= this.lstSGD.length - 1; i++) {
                this.lstSGD[i].IsSelected = false;
            }

            this.SelectedSDG = '';
            this.SelectedSDGTargets = [];
            this.SelectedSDGTargetIndicators = '';
            this.lstSGDData = [];
            this.lstSDGTargetsData = [];
            this.lstSDGTargetIndicatorsData = [];
        },

        /// <summary>
        ///  fnRemoveEditUser :- Function used to remove user from edit dialog box
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        fnRemoveEditUser(item) {
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
                // {{SelectedSDGTargets.filter(x => x.pk_SDGID === item.pk_SDGID).length}}
                Count = this.SelectedSDGTargets.filter(x => x.fk_SDGID === item.pk_SDGID).length;
            }
            return Count;
        },

        /// <summary>
        ///  TickSelectedTag :- Function used to show tick mark on selected tags
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        TickSelectedTag() {
            for (var cnt = 0; cnt <= this.filterAllTagList.length - 1; cnt++) {
                var isExistTag = this.SelectedTag.filter(x => x.TagID == this.filterAllTagList[cnt].TagID);
                if (isExistTag.length > 0) {
                    this.filterAllTagList[cnt].IsChecked = true;
                } else {
                    this.filterAllTagList[cnt].IsChecked = false;
                }
            }
        },
      },
        watch: {
            CreateNewProjectStepsProp: function () {
                this.ProjectId = window.ProjectId;//this.$route.query.ProjID;
                this.GroupId = window.GroupId;
                this.FetchAllOutcomeList();
                this.fnGetSurveys();
                //this.GetReportingFrequencyList();
                this.fnGetTeamUsersList();

            },

            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.IsSearchTagExist = false;
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0) {
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                }
                if (this.filterAllTagList.length > 0) {
                    this.IsSearchTagExist = true;
                }
            },
        }
  }
</script>
<style >
.v-expansion-panel-header{
   padding-top: 0 !important;
   padding-bottom: 0 !important;
}
.v-expansion-panel-content__wrap {
        padding: 0 !important;
}
</style>