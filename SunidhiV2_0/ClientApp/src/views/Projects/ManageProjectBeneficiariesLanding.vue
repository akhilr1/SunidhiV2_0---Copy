<template>
    <div>
        <!--Pre-loader-->
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <!--________________________-->

        <v-container fluid="">
            <v-row v-if="lstAllBeneficiaryTypes.length!=0">
                <v-col md="auto">
                    <h5 class="page-head" style="width:inherit">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="15" color="primary">mdi-database</v-icon>
                        </v-avatar>
                        Beneficiary Type
                    </h5>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto">
                    <v-row>
                        <Success-Button id="AutoTest_IP296" class="mr-4 float-left" title="Create Beneficiary Type" @click.native="dlgCreateBeneficiaryType=true" v-bind:disabled="!screenAccess"></Success-Button>
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                                <v-btn id="AutoTest_IP297" height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4 float-left" @click="isBulk =true,snackbarBulkActions =true">
                                    Bulk Actions
                                    <v-icon>mdi-chevron-down</v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item id="AutoTest_IP298" @click.stop="dlgRemoveBenefType = true" v-bind:disabled="isBulk && vmBeneficiaryType.length ==0">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                        </v-menu>
                        <v-text-field label="Search" class="mr-4 float-left" v-model="searchBenefType"
                                      dense prepend-inner-icon="mdi-magnify"
                                      outlined hide-details style="max-width:150px"></v-text-field>
                        <!--Filter-->
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                            <template v-slot:activator="{ on }">
                                <v-btn id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent">
                                    <v-icon>mdi-filter-outline</v-icon>
                                    Filters
                                </v-btn>
                            </template>
                        </v-menu>
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="SortMenu">
                            <template v-slot:activator="{ on }">
                                <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                                <v-btn v-on="on" max-width="29" min-width="40" height="37" outlined class="outlined-btn-dark">
                                    <v-icon>mdi-tune-vertical</v-icon>
                                </v-btn>
                            </template>
                            <v-list dense color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item>
                                    <v-list-item-content>
                                        Sort by :
                                    </v-list-item-content>
                                </v-list-item>
                                
                                <v-list-item>
                                    <v-list-item-content @click.stop="fnBeneficiaryTypeSort('Name-Ascending')">
                                        Name - Ascending
                                    </v-list-item-content>
                                </v-list-item>
                                
                                <v-list-item>
                                    <v-list-item-content @click.stop="fnBeneficiaryTypeSort('Name-Descending')">
                                        Name - Descending
                                    </v-list-item-content>
                                </v-list-item>
                                
                                <v-list-item>
                                    <v-list-item-content @click.stop="fnBeneficiaryTypeSort('Recently Added')">
                                        Recently Added
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
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
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-2">
                            {{dateRangeHeaderName}}
                            <v-icon>
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
            <!--<v-row no-gutters>
        <v-col cols="12" xs="12" sm="12" md="12">
            <v-chip label outlined dark color="red"
                    v-if="(isBulk && vmBeneficiaryType.length ==0)">
                * Please select Beneficiary Type from the list
            </v-chip>
        </v-col>
    </v-row>-->

            <!--Main Body-->
            <v-row v-if="FilterlstAllBeneficiaryTypes.length!=0">
                <v-col class="px-0">
                    <v-data-table :headers="BenTypeTableheaders"
                                  :items="FilterlstAllBeneficiaryTypes"
                                  :search="searchBenefType"
                                  item-key="BeneficiaryTypeID"
                                  v-model="vmBeneficiaryType"
                                  fixed-header
                                  height="60vh"
                                  show-select>
                        <template v-slot:item.CreatedDate="{ item }">
                            <div class="pa-0 text-limit" style="width:150px">
                                {{formatDate(new Date(item.CreatedDate).toISOString().substr(0, 10))}}
                            </div>
                        </template>
                        <template v-slot:item.UpdatedDate="{ item }">
                            <div class="pa-0 text-limit" style="width:150px">
                                {{formatDate(new Date(item.UpdatedDate).toISOString().substr(0, 10))}}
                            </div>
                        </template>
                        <template v-slot:item.Name="{item}">
                            <v-list dense class="py-0 table-v-list">
                                <!--if path is ManageProjectBeneficiaries-->
                                <v-list-item v-if="$route.name=='ManageProjectBeneficiaries'" link :to="{ name: 'ManageProjectBeneficiariesTypeIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,  BenfTID: item.BeneficiaryTypeID}}" class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.Name}}
                                    </div>
                                </v-list-item>
                                <!--____________________-->
                                <!--if path is MyprojectsDraftIndividualBeneficiaries-->
                                <v-list-item v-if="$route.name=='MyprojectsDraftIndividualBeneficiaries'" link="" :to="{ name: 'MyprojectsDraftIndividualBeneficiariesTypeIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,  BenfTID: item.BeneficiaryTypeID}}" class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.Name}}
                                    </div>
                                </v-list-item>
                                <!--____________________-->
                                <!--if path is MyprojectsDeployedIndividualBeneficiaries-->
                                <v-list-item v-if="$route.name=='MyprojectsDeployedIndividualBeneficiaries'" link="" :to="{ name: 'MyprojectsDeployedIndividualBeneficiariesTypeIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,  BenfTID: item.BeneficiaryTypeID}}" class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.Name}}
                                    </div>
                                </v-list-item>
                                <!--____________________-->
                            </v-list>
                        </template>
                        <template v-slot:item.BenTypeActions="{item}">
                            <v-menu close-on-content-click transition="scroll-y-transition" left="" offset-y v-bind:disabled="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>mdi-dots-vertical</v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">
                                    <v-list-item id="AutoTest_IP300" link @click.stop="GetBeneficiaryTypeDetails(item.BeneficiaryTypeID)">
                                        <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                        <v-list-item-content>Edit</v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_IP301" link @click.stop="dlgRemoveBenefType = true,isBulk =false,BenefTypeID= item.BeneficiaryTypeID, BenefTypeName =item.Name  ">
                                        <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                        <v-list-item-content>Remove</v-list-item-content>
                                    </v-list-item>
                                </v-list>
                            </v-menu>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>

            <v-row v-else>
                <!--If there is no Beneficiary Type-->
                <v-col class="text-center">
                    <p class="mt-12 pt-12">
                        <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                    </p>

                    <p class="heading-3-dark font-weight-medium pt-6">No Benificiary Type data available</p>
                    <Success-Button id="AutoTest_IP299" class="mt-4" style="width:250px" title="Create Benificiary Type" @click.native="dlgCreateBeneficiaryType=true" v-bind:disabled="!screenAccess"></Success-Button>
                </v-col>
                <!--______________________________________-->
            </v-row>

            <!--Create BeneficiaryType Dialog-->
            <v-dialog v-model="dlgCreateBeneficiaryType" eager persistent>
                <v-card class="right-fixed-panel">
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-database</v-icon>
                        </v-avatar>Create Beneficiary Type
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP302" text @click="dlgCreateBeneficiaryType=false">
                            <v-icon id="AutoTest_IP303" color="#707070" size="15" @click="CloseBTDialogue()">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="py-0">
                        <v-container fluid="" class="pt-0">
                            <v-row no-gutters="">
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-stepper v-model="CreateBeneficiaryTypeSteps" class="elevation-0">
                                        <v-row justify="center">
                                            <v-col md="6">
                                                <v-stepper-header class="elevation-0">
                                                    <v-stepper-step :complete="CreateBeneficiaryTypeSteps > 1" step="1" color="inherit">Add Details</v-stepper-step>
                                                    <v-divider></v-divider>
                                                    <v-stepper-step :complete="CreateBeneficiaryTypeSteps > 2" step="2" color="inherit">Registration Form</v-stepper-step>
                                                </v-stepper-header>
                                            </v-col>
                                        </v-row>
                                        <v-stepper-items>
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row no-gutters>
                                                        <v-col cols="12" md="12" class="pt-4">
                                                            <h4 class="text-center heading-3">Step 1: Please enter the basic details to create a new beneficiary type.</h4>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                            <v-row justify="center">
                                                                <v-col md="6">
                                                                    <v-text-field class="mt-4" outlined dense label="Enter Beneficiary Type Name" v-model="txtBTypeName">
                                                                    </v-text-field>
                                                                    <v-textarea class="mt-4" outlined dense label="Description" v-model="txtBTypeDescription">
                                                                    </v-textarea>
                                                                    <!-- Tags-->
                                                                    <v-row class="mb-1">
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
                                                                                        <v-col cols="12" md="12">
                                                                                            <!-- Header Text  -->
                                                                                            <v-col cols="12" md="12" class="d-inline-flex">
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
                                                                                                                <v-btn id="AutoTest_IP307" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
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
                                                                            <div v-if="!allTags">
                                                                                <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                                                                <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
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
                                                                    </v-row>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">

                                                            <!--<Next-button-primary-light class="mr-2" @click.native="CreateBeneficiaryTypeSteps = 2,GetAnswerTypes()"
                                                                               v-bind:disabled="txtBTypeName.length==0"></Next-button-primary-light>

                                                    <Cancel-Button-Outlined-Sm @click.native="CloseBTDialogue()"></Cancel-Button-Outlined-Sm>-->
                                                            <Secondary-Button id="AutoTest_IP315" v-bind:disabled="txtBTypeName.length==0" class="btn-122x36 mr-4" title="Next" @click.native="CreateBeneficiaryTypeSteps = 2,GetAnswerTypes()"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_IP316" class="btn-122x36" title="Cancel" @click.native="CloseBTDialogue()"></Default-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-stepper-content>
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row no-gutters>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <h4 class="text-center heading-3">Step 2: Please enter questions to create a new registration form.</h4>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                            <v-row justify="center">
                                                                <v-col md="12">
                                                                    <v-card outlined>
                                                                        <v-col class="v-scrolling-div" style="max-height:50vh">
                                                                            <draggable v-model="lstQstns" @update="UpdateNewQstnOrder(lstQstns)">
                                                                                <transition-group type="transition" name="flip-list">
                                                                                    <!--<v-card outlined>-->

                                                                                    <div class="drag-cursor" v-for="(item, index) in lstQstns" :key="item.QuestionOrder">

                                                                                        <!--<template v-for="(item, index) in lstQstns">-->
                                                                                            <!--Update added qstns this section shows when click on edit pencil button-->
                                                                                            <v-row v-if="addNewisEdit && item.IsEditQstn">

                                                                                                <!---->
                                                                                                <v-col md="12" class="active-section">
                                                                                                    <v-row>
                                                                                                        <v-col md="6">
                                                                                                            <v-text-field filled dense v-model="vmEQstn[index]"> </v-text-field>

                                                                                                            <!--    Answer Types   -->
                                                                                                            <!----=================-->

                                                                                                            <v-text-field v-if="vmAnswerTypeInEdit.AnswerType=='Single Line Text'" dense label="Type your answer" disabled />
                                                                                                            <!--Multiline Answer-->
                                                                                                            <v-textarea v-if="vmAnswerTypeInEdit.AnswerType==='Multi Line Text'" dense label="Type your answer" disabled />
                                                                                                            <!--Single Choice / Multi choice Answer / DropDown-->
                                                                                                            <div v-if="(vmAnswerTypeInEdit.AnswerType==='Single Choice' || vmAnswerTypeInEdit.AnswerType==='Multi-Choice' || vmAnswerTypeInEdit.AnswerType==='Dropdown')" class="mt-0">
                                                                                                                <v-row v-for="(SingleChoice, index) in SingleChoices" :key="index">
                                                                                                                    <v-col>
                                                                                                                        <v-text-field dense label="Enter Option" v-model="SingleChoice.OptionValue">
                                                                                                                        </v-text-field>
                                                                                                                    </v-col>
                                                                                                                    <v-col md="auto">
                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP320" v-show="SingleChoices.length>1" class="mr-4" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(SingleChoice)"></Default-Icon-Button-Outlined>
                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP321" v-show="index == SingleChoices.length - 1" icon="mdi-plus" @click.native="AddOptionItem()"></Default-Icon-Button-Outlined>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </div>
                                                                                                            <!--Numeric-->
                                                                                                            <v-text-field v-if="vmAnswerTypeInEdit.AnswerType==='Numeric'" dense type="number" label="Type your answer" disabled>
                                                                                                            </v-text-field>

                                                                                                            <!--Cascading Dropdown -->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                                                <v-row>
                                                                                                                    <v-col>
                                                                                                                        <v-select v-model="vmCascadingOption" single-line
                                                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                                                  item-text="OptionValue"
                                                                                                                                  item-value="ID"
                                                                                                                                  :menu-props="{ maxHeight: '300' }"
                                                                                                                                  outlined dense
                                                                                                                                  label="<-- Select -->" return-object @change="SaveCascadingOptions(vmCascadingOption.ID)">
                                                                                                                        </v-select>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </div>
                                                                                                            <!--GPS Capture-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='GPS Capture'">
                                                                                                                <v-btn outlined disabled>
                                                                                                                    <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                                                    Capture Location
                                                                                                                </v-btn>
                                                                                                            </div>
                                                                                                            <!--Photo Capture-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Photo Capture'">
                                                                                                                <v-btn outlined disabled>
                                                                                                                    <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                                                    Photo Capture
                                                                                                                </v-btn>
                                                                                                            </div>
                                                                                                            <!--Audio Capture-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Audio Capture'" disabled>
                                                                                                                <audio controls></audio>
                                                                                                            </div>
                                                                                                            <!--Signature Capture-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Signature Capture'">
                                                                                                                <v-btn outlined disabled>
                                                                                                                    <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                                                    Upload Signature
                                                                                                                </v-btn>
                                                                                                            </div>
                                                                                                            <!--Date Picker-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Date Picker'">
                                                                                                                <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                                                        v-model="SelectAnswerTypeDatepicker"
                                                                                                                        :close-on-content-click="false"
                                                                                                                        transition="scale-transition"
                                                                                                                        offset-y=""
                                                                                                                        min-width="290px">
                                                                                                                    <template v-slot:activator="{ on }">
                                                                                                                        <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                                                      label="Select Date" disabled
                                                                                                                                      prepend-inner-icon="mdi-calendar" dense
                                                                                                                                      readonly=""
                                                                                                                                      v-on="on"></v-text-field>
                                                                                                                    </template>
                                                                                                                    <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                                                        <v-spacer></v-spacer>
                                                                                                                        <v-btn id="AutoTest_IP323" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                                                        <v-btn id="AutoTest_IP324" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                                                    </v-date-picker>
                                                                                                                </v-menu>
                                                                                                            </div>
                                                                                                            <!--Note-->
                                                                                                            <div v-if="vmAnswerTypeInEdit.AnswerType==='Note'" disabled>
                                                                                                                <v-textarea disabled></v-textarea>
                                                                                                            </div>
                                                                                                        </v-col>
                                                                                                        <!-- Question Type Combobox-->
                                                                                                        <v-col md="3">
                                                                                                            <v-select outlined v-model="vmAnswerTypeInEdit" single-line :label="` ${item.AnswerType}`"
                                                                                                                      :items="lstAnswerTypes"
                                                                                                                      item-text="AnswerType"
                                                                                                                      item-value="AnswerTypeID"
                                                                                                                      :menu-props="{ maxHeight: '300' }"
                                                                                                                      dense
                                                                                                                      return-object=""
                                                                                                                      @change="lstOptions=[],vmCascadingOption=[],SingleChoices = [{ OptionValue: '' }],preCasOption = 0">
                                                                                                            </v-select>
                                                                                                        </v-col>
                                                                                                        <!-- Validation menu popup-->
                                                                                                        <v-col>
                                                                                                            <v-menu absolute mx-width="100" v-model="item.CreateQuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                                                <template v-slot:activator="{ on }">
                                                                                                                    <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed color="#EEEEEE" class="outlined-btn-dark float-right">
                                                                                                                        <v-icon>mdi-settings</v-icon>
                                                                                                                    </v-btn>
                                                                                                                </template>
                                                                                                                <v-card>
                                                                                                                    <v-card-title class="heading-4">
                                                                                                                        <v-spacer></v-spacer>
                                                                                                                        <v-btn id="AutoTest_IP325" text height="25" min-width="25" max-width="25" color="#707070" @click="item.CreateQuestionHintPopUp=false">
                                                                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                                                                        </v-btn>
                                                                                                                    </v-card-title>
                                                                                                                    <v-divider></v-divider>
                                                                                                                    <v-card-text class="pa-0">
                                                                                                                        <v-tabs fixed-tabs color="tab-active-line">
                                                                                                                            <v-tab href="#AddNewQstSettTab-1">Setting</v-tab>
                                                                                                                            <v-tab href="#AddNewQstSettTab-2">Validation</v-tab>
                                                                                                                            <!-- Settings Tab-->
                                                                                                                            <v-tab-item class="mt-4 v-scrolling-div" value="AddNewQstSettTab-1" style="width:350px; max-height:250px;">
                                                                                                                                <v-list dense>
                                                                                                                                    <v-list-item>
                                                                                                                                        <h3 class="heading-4 ">Question Hint</h3>
                                                                                                                                    </v-list-item>
                                                                                                                                    <v-list-item>
                                                                                                                                        <v-text-field filled dense label="Hint" v-model="item.QuestionHint"></v-text-field>
                                                                                                                                    </v-list-item>
                                                                                                                                    <v-list-item>
                                                                                                                                        <v-list-item-content>
                                                                                                                                            Make the Question receive a mandatory response
                                                                                                                                        </v-list-item-content>
                                                                                                                                        <v-list-item-action>
                                                                                                                                            <v-switch color="#707070" v-model="item.IsMandatoryResponse"></v-switch>
                                                                                                                                        </v-list-item-action>
                                                                                                                                    </v-list-item>
                                                                                                                                    <v-list-item>
                                                                                                                                        <v-list-item-content>
                                                                                                                                            Make the Question and its answers unique
                                                                                                                                        </v-list-item-content>
                                                                                                                                        <v-list-item-action>
                                                                                                                                            <v-switch color="#707070" v-model="item.IsUnique"></v-switch>
                                                                                                                                        </v-list-item-action>
                                                                                                                                    </v-list-item>
                                                                                                                                    <v-list-item>
                                                                                                                                        <v-list-item-content>
                                                                                                                                            Make the Question and its answers searchable.
                                                                                                                                        </v-list-item-content>
                                                                                                                                        <v-list-item-action>
                                                                                                                                            <v-switch color="#707070" v-model="item.IsSearchable"></v-switch>
                                                                                                                                        </v-list-item-action>
                                                                                                                                    </v-list-item>
                                                                                                                                </v-list>
                                                                                                                            </v-tab-item>
                                                                                                                            <!-- Validation Tab-->
                                                                                                                            <v-tab-item class="mt-4 px-4 v-scrolling-div" value="AddNewQstSettTab-2" style="width:350px; max-height:250px;">
                                                                                                                                <v-row>
                                                                                                                                    <v-col md="12">
                                                                                                                                        <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                                                        <v-textarea filled="" label="Enter validation REGEX expression..." v-model="item.RegexExpression"></v-textarea>
                                                                                                                                        <h3 class="heading-4 mb-2">Validation Condition</h3>
                                                                                                                                        <v-row no-gutters v-for="(Valitem,idx) in item.lstQuestionValidations" :key="idx">
                                                                                                                                            <v-col>
                                                                                                                                                <!-- And or OR Condtions-->
                                                                                                                                                <v-row no-gutters v-if="idx >0">
                                                                                                                                                    <div class="mx-auto">
                                                                                                                                                        <v-radio-group row v-model="Valitem.vmOperatorValue">
                                                                                                                                                            <v-radio label="AND" hide-details></v-radio>
                                                                                                                                                            <v-radio label="OR" hide-details></v-radio>
                                                                                                                                                        </v-radio-group>
                                                                                                                                                    </div>
                                                                                                                                                </v-row>
                                                                                                                                                <v-row>
                                                                                                                                                    <v-col>
                                                                                                                                                        <v-select dense :items="QstValidationItems" style="width:200px"
                                                                                                                                                                  item-text="ValidationCondition" v-model="Valitem.ValidationCondition"
                                                                                                                                                                  label="Select validation condition" outlined hide-details return-object>
                                                                                                                                                        </v-select>
                                                                                                                                                        <v-row v-show="Valitem.ValidationCondition==='Min & Max Range'">
                                                                                                                                                            <v-col>
                                                                                                                                                                <v-text-field filled dense label="Enter min range" type="number" hide-details="" v-model="Valitem.MinValue"></v-text-field>
                                                                                                                                                            </v-col>
                                                                                                                                                            <v-col>
                                                                                                                                                                <v-text-field hide-details filled dense label="Enter max range" type="number" v-model="Valitem.MaxValue"></v-text-field>
                                                                                                                                                            </v-col>
                                                                                                                                                        </v-row>
                                                                                                                                                    </v-col>
                                                                                                                                                    <v-col md="auto">
                                                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline" @click.native="RemoveValidationItem(index,idx,'Edit')"></Default-Icon-Button-Outlined>
                                                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1" @click.native="AddValidationItem('Edit',index)"></Default-Icon-Button-Outlined>
                                                                                                                                                    </v-col>
                                                                                                                                                </v-row>
                                                                                                                                            </v-col>
                                                                                                                                        </v-row>
                                                                                                                                    </v-col>
                                                                                                                                </v-row>
                                                                                                                                <v-row>
                                                                                                                                    <v-col>
                                                                                                                                        <Default-Button-Outlined id="AutoTest_IP737" title="Cancel" class="float-right btn-122x36" @click.native="item.CreateQuestionHintPopUp = false"></Default-Button-Outlined>
                                                                                                                                        <Secondary-Button id="AutoTest_IP738" title="Save" class="float-right mr-4 btn-122x36" @click.native="item.CreateQuestionHintPopUp = false"></Secondary-Button>
                                                                                                                                    </v-col>
                                                                                                                                </v-row>
                                                                                                                            </v-tab-item>
                                                                                                                        </v-tabs>
                                                                                                                    </v-card-text>
                                                                                                                </v-card>
                                                                                                            </v-menu>
                                                                                                        </v-col>
                                                                                                        <!-- Save and Cancel Button-->
                                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-5">

                                                                                                            <Secondary-Button class="btn-122x36 mr-4" title="Save" @click.native="item.Question = vmEQstn[index],fnUpdateQuestion(index, item)" v-bind:disabled="fnCheckMandatory(vmEQstn[index],vmAnswerTypeInEdit.AnswerType)"></Secondary-Button>
                                                                                                            <Default-Button-Outlined class="btn-122x36 mr-4" title="Cancel" @click.native="fnFinishEdit()"></Default-Button-Outlined>
                                                                                                        </v-col>
                                                                                                    </v-row>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                            <!--Added qst this section show already added questions list on disabled-->
                                                                                            <v-row v-else v-bind:class="{ 'active-section py-4 pl-4 mx-0': item.IsEditQstn }">

                                                                                                <v-col md="6">
                                                                                                    <p class="font-weight-medium">{{item.Question}}</p>
                                                                                                    <!-- <v-text-field filled dense v-model="item.Question" label="Question" v-bind:disabled="true">
                </v-text-field>-->
                                                                                                    <!--    Answer Types    -->
                                                                                                    <!----=================-->

                                                                                                    <v-text-field v-if="item.AnswerType=='Single Line Text'" dense label="Type your answer" v-bind:disabled="true" />
                                                                                                    <!--Multiline Answer-->
                                                                                                    <v-textarea v-if="item.AnswerType==='Multi Line Text'" dense label="Type your answer" v-bind:disabled="true" />

                                                                                                    <!--Single Choice -->
                                                                                                    <div v-if="item.AnswerType==='Single Choice'" class="mt-0">
                                                                                                        <v-radio-group disabled>
                                                                                                            <v-radio v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                                     :label="optItem.OptionValue"></v-radio>
                                                                                                        </v-radio-group>

                                                                                                    </div>
                                                                                                    <!--Multiple Choice -->
                                                                                                    <div v-if="item.AnswerType==='Multi-Choice'" class="mt-0">
                                                                                                        <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                                            <v-checkbox input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                                        disabled></v-checkbox>

                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <!--Dropdown -->
                                                                                                    <div v-if="item.AnswerType==='Dropdown'" class="mt-0">
                                                                                                        <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                                            <v-row>
                                                                                                                <v-col md="auto">
                                                                                                                    <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                                                                                              item-text="OptionValue"
                                                                                                                              label="<-- Select -->" outlined="">
                                                                                                                    </v-select>
                                                                                                                </v-col>
                                                                                                            </v-row>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <!--Cascading Dropdown -->
                                                                                                    <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                                        <v-row>
                                                                                                            <v-col>
                                                                                                                <v-select dense :menu-props="{ maxHeight: '300' }" filled disabled
                                                                                                                          :label="` ${optItem.OptionValue}`" outlined=""
                                                                                                                          v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind">
                                                                                                                </v-select>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </div>
                                                                                                    <!--Numeric-->
                                                                                                    <v-text-field v-if="item.AnswerType==='Numeric'" dense type="number" label="Type your answer" v-bind:disabled="true">
                                                                                                    </v-text-field>

                                                                                                    <!--GPS Capture-->
                                                                                                    <div v-if="item.AnswerType==='GPS Capture'" v-bind:disabled="true">
                                                                                                        <v-btn outlined disabled>
                                                                                                            <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                                            Location
                                                                                                        </v-btn>
                                                                                                    </div>
                                                                                                    <!--Photo Capture-->
                                                                                                    <div v-if="item.AnswerType==='Photo Capture'" v-bind:disabled="true">
                                                                                                        <v-btn outlined disabled>
                                                                                                            <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                                            Photo Capture
                                                                                                        </v-btn>
                                                                                                    </div>
                                                                                                    <!--Audio Capture-->
                                                                                                    <div v-if="item.AnswerType==='Audio Capture'" v-bind:disabled="true">
                                                                                                        <v-btn outlined disabled>
                                                                                                            <v-icon class="mr-2">mdi-audiobook</v-icon>
                                                                                                            Audio Capture
                                                                                                        </v-btn>
                                                                                                    </div>
                                                                                                    <!--Signature Capture-->
                                                                                                    <div v-if="item.AnswerType==='Signature Capture'" v-bind:disabled="true">
                                                                                                        <v-btn outlined disabled>
                                                                                                            <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                                            Signature Capture
                                                                                                        </v-btn>
                                                                                                    </div>
                                                                                                    <!--Date Picker-->
                                                                                                    <div v-if="item.AnswerType==='Date Picker'" v-bind:disabled="true">
                                                                                                        <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                                                v-model="SelectAnswerTypeDatepicker"
                                                                                                                :close-on-content-click="false"
                                                                                                                transition="scale-transition"
                                                                                                                offset-y=""
                                                                                                                min-width="290px">
                                                                                                            <template v-slot:activator="{ on }">
                                                                                                                <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                                              label="Select Date"
                                                                                                                              prepend-inner-icon="mdi-calendar" dense
                                                                                                                              readonly disabled
                                                                                                                              v-on="on"></v-text-field>
                                                                                                            </template>
                                                                                                            <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                                                <v-spacer></v-spacer>
                                                                                                                <v-btn id="AutoTest_IP317" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                                                <v-btn id="AutoTest_IP318" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                                            </v-date-picker>
                                                                                                        </v-menu>
                                                                                                    </div>
                                                                                                    <!--Note-->
                                                                                                    <div v-if="item.AnswerType==='Note'" v-bind:disabled="true">
                                                                                                        <v-textarea disabled></v-textarea>
                                                                                                    </div>
                                                                                                </v-col>
                                                                                                <v-col md="3">
                                                                                                    <v-select dense :menu-props="{ maxHeight: '300' }" outlined
                                                                                                              :label="` ${item.AnswerType}`" disabled>
                                                                                                    </v-select>
                                                                                                </v-col>
                                                                                                <!-- Validation And Settings Popup-->
                                                                                                <v-col>
                                                                                                    <v-menu absolute mx-width="100" v-model="item.QuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                                        <template v-slot:activator="{ on }">
                                                                                                            <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed class="outlined-btn-dark float-right" color="#EEEEEE">
                                                                                                                <v-icon>mdi-settings</v-icon>
                                                                                                            </v-btn>
                                                                                                        </template>
                                                                                                        <v-card>
                                                                                                            <v-card-title class="heading-4">
                                                                                                                <v-spacer></v-spacer>
                                                                                                                <v-btn id="AutoTest_IP319" text height="25" min-width="25" max-width="25" color="#707070" @click="item.QuestionHintPopUp=false">
                                                                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                                                                </v-btn>
                                                                                                            </v-card-title>
                                                                                                            <v-divider></v-divider>
                                                                                                            <v-card-text class="pa-0">
                                                                                                                <v-tabs fixed-tabs color="tab-active-line">
                                                                                                                    <v-tab href="#AddNewQstPreviewSettTab-1">Setting</v-tab>
                                                                                                                    <v-tab href="#AddNewQstPreviewSettTab-2">Validation</v-tab>
                                                                                                                    <!-- Settings Tab-->
                                                                                                                    <v-tab-item class="mt-4 v-scrolling-div" value="AddNewQstPreviewSettTab-1" style="width:350px; max-height:250px;">
                                                                                                                        <v-list dense>
                                                                                                                            <v-list-item>
                                                                                                                                <h3 class="heading-4 ">Question Hint</h3>
                                                                                                                            </v-list-item>
                                                                                                                            <v-list-item>
                                                                                                                                <v-text-field disabled filled dense label="Hint" v-model="item.QuestionHint"></v-text-field>
                                                                                                                            </v-list-item>
                                                                                                                            <v-list-item>
                                                                                                                                <v-list-item-content>
                                                                                                                                    Make the Question receive a mandatory response
                                                                                                                                </v-list-item-content>
                                                                                                                                <v-list-item-action>
                                                                                                                                    <v-switch disabled color="#707070" v-model="item.IsMandatoryResponse"></v-switch>
                                                                                                                                </v-list-item-action>
                                                                                                                            </v-list-item>
                                                                                                                            <v-list-item>
                                                                                                                                <v-list-item-content>
                                                                                                                                    Make the Question and its answers unique
                                                                                                                                </v-list-item-content>
                                                                                                                                <v-list-item-action>
                                                                                                                                    <v-switch disabled color="#707070" v-model="item.IsUnique"></v-switch>
                                                                                                                                </v-list-item-action>
                                                                                                                            </v-list-item>
                                                                                                                            <v-list-item>
                                                                                                                                <v-list-item-content>
                                                                                                                                    Make the Question and its answers searchable.
                                                                                                                                </v-list-item-content>
                                                                                                                                <v-list-item-action>
                                                                                                                                    <v-switch disabled color="#707070" v-model="item.IsSearchable"></v-switch>
                                                                                                                                </v-list-item-action>
                                                                                                                            </v-list-item>
                                                                                                                        </v-list>
                                                                                                                    </v-tab-item>
                                                                                                                    <!-- Validations Tab-->
                                                                                                                    <v-tab-item class="mt-4 px-4 v-scrolling-div" value="AddNewQstPreviewSettTab-2" style="width:350px; max-height:250px;">
                                                                                                                        <v-row>
                                                                                                                            <v-col md="12">
                                                                                                                                <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                                                <v-textarea filled disabled label="Enter validation REGEX expression..." v-model="item.RegexExpression"></v-textarea>
                                                                                                                                <h3 class="heading-4 mb-2">Validation Condition</h3>
                                                                                                                                <v-row no-gutters v-for="(Valitem,idx) in item.lstQuestionValidations">
                                                                                                                                    <v-col>
                                                                                                                                        <!-- And Or Or Radio Buttons-->
                                                                                                                                        <v-row no-gutters="" v-if="idx >0">
                                                                                                                                            <div class="mx-auto">
                                                                                                                                                <v-radio-group row="" v-model="Valitem.vmOperatorValue">
                                                                                                                                                    <v-radio disabled label="AND" hide-details=""></v-radio>
                                                                                                                                                    <v-radio disabled label="OR" hide-details=""></v-radio>
                                                                                                                                                </v-radio-group>
                                                                                                                                            </div>
                                                                                                                                        </v-row>
                                                                                                                                        <v-row>
                                                                                                                                            <v-col>
                                                                                                                                                <v-select disabled dense :items="QstValidationItems" style="width:200px"
                                                                                                                                                          item-text="ValidationCondition" v-model="Valitem.ValidationCondition"
                                                                                                                                                          label="Select validation condition" outlined hide-details return-object>
                                                                                                                                                </v-select>
                                                                                                                                                <v-row v-show="Valitem.ValidationCondition==='Min & Max Range'">
                                                                                                                                                    <v-col>
                                                                                                                                                        <v-text-field filled dense label="Enter min range" type="number" disabled hide-details="" v-model="Valitem.MinValue"></v-text-field>
                                                                                                                                                    </v-col>
                                                                                                                                                    <v-col>
                                                                                                                                                        <v-text-field hide-details filled dense label="Enter max range" disabled type="number" v-model="Valitem.MaxValue"></v-text-field>
                                                                                                                                                    </v-col>
                                                                                                                                                </v-row>
                                                                                                                                            </v-col>
                                                                                                                                            <!-- Plus and Delete Button-->
                                                                                                                                            <v-col md="auto">
                                                                                                                                                <Default-Icon-Button-Outlined disabled id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline"></Default-Icon-Button-Outlined>
                                                                                                                                                <Default-Icon-Button-Outlined disabled id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1" @click.native="AddValidationItem()"></Default-Icon-Button-Outlined>
                                                                                                                                            </v-col>
                                                                                                                                        </v-row>
                                                                                                                                    </v-col>
                                                                                                                                </v-row>
                                                                                                                            </v-col>
                                                                                                                        </v-row>
                                                                                                                    </v-tab-item>
                                                                                                                </v-tabs>
                                                                                                            </v-card-text>
                                                                                                        </v-card>
                                                                                                    </v-menu>
                                                                                                    <!--Remove Question Button-->
                                                                                                    <Default-Icon-Button-Outlined class="mr-4 float-right" icon="mdi-trash-can-outline" @click.native="dlgRemoveQuest = true, QuestionID=item.QuestionID, TempQustId=index"></Default-Icon-Button-Outlined>
                                                                                                    <!-- Copy Question Button-->
                                                                                                    <Default-Icon-Button-Outlined class="mr-4 float-right" icon="mdi-content-copy" @click.native="CurrQstnIndex = index,QuestionID = item.QuestionID,dlgCopyQuestion = true"></Default-Icon-Button-Outlined>
                                                                                                    <!-- Edit Question-->
                                                                                                    <Default-Icon-Button-Outlined class="mr-4 float-right" icon="mdi-pencil-outline" @click.native="AddeditQustion(index)"></Default-Icon-Button-Outlined>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                            <v-divider></v-divider>
                                                                                        <!--</template>-->
                                                                                        <!--</v-card>-->
                                                                                    </div>
                                                                                </transition-group>
                                                                            </draggable>
                                                                            <!-- Initial UI when Loads - to create questions-->
                                                                            <v-row>

                                                                                <!---->
                                                                                <v-col md="12" class="active-section">
                                                                                    <v-row>
                                                                                        <v-col md="6">
                                                                                            <v-text-field filled dense label="Untitled Question" v-model="vmQstn"> </v-text-field>

                                                                                            <!--    Answer Types   -->
                                                                                            <!----=================-->

                                                                                            <v-text-field v-if="vmAnswerType.AnswerType=='Single Line Text'" dense label="Type your answer" disabled />
                                                                                            <!--Multiline Answer-->
                                                                                            <v-textarea v-if="vmAnswerType.AnswerType==='Multi Line Text'" dense label="Type your answer" disabled />

                                                                                            <!--Single Choice / Multi choice Answer / DropDown-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Single Choice' || vmAnswerType.AnswerType==='Multi-Choice' || vmAnswerType.AnswerType==='Dropdown'" class="mt-0">
                                                                                                <v-row v-for="(SingleChoice, index) in SingleChoices" :key="index">

                                                                                                    <v-col>
                                                                                                        <v-text-field dense label="Enter Option" v-model="SingleChoice.OptionValue">
                                                                                                        </v-text-field>
                                                                                                    </v-col>
                                                                                                    <v-col md="auto">
                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP320" v-show="SingleChoices.length>1" class="mr-4" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(SingleChoice)"></Default-Icon-Button-Outlined>
                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP321" v-show="index == SingleChoices.length - 1" icon="mdi-plus" @click.native="AddOptionItem()"></Default-Icon-Button-Outlined>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>
                                                                                            <!--Numeric-->
                                                                                            <v-text-field v-if="vmAnswerType.AnswerType==='Numeric'" dense type="number" label="Type your answer" disabled>
                                                                                            </v-text-field>

                                                                                            <!--Cascading Dropdown -->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                                <v-row>
                                                                                                    <v-col>
                                                                                                        <v-select v-model="vmCascadingOption" single-line
                                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                                  item-text="OptionValue"
                                                                                                                  item-value="ID"
                                                                                                                  :menu-props="{ maxHeight: '300' }"
                                                                                                                  outlined dense
                                                                                                                  label="<-- Select -->" return-object @change="SaveCascadingOptions(vmCascadingOption.ID)">
                                                                                                        </v-select>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>
                                                                                            <!--GPS Capture-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='GPS Capture'">
                                                                                                <v-btn outlined disabled>
                                                                                                    <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                                    Capture Location
                                                                                                </v-btn>
                                                                                            </div>
                                                                                            <!--Photo Capture-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Photo Capture'">
                                                                                                <v-btn outlined disabled>
                                                                                                    <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                                    Photo Capture
                                                                                                </v-btn>
                                                                                            </div>
                                                                                            <!--Audio Capture-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Audio Capture'" disabled>
                                                                                                <audio controls></audio>
                                                                                            </div>
                                                                                            <!--Signature Capture-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Signature Capture'">
                                                                                                <v-btn outlined disabled>
                                                                                                    <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                                    Upload Signature
                                                                                                </v-btn>
                                                                                            </div>
                                                                                            <!--Date Picker-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Date Picker'">
                                                                                                <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                                        v-model="SelectAnswerTypeDatepicker"
                                                                                                        :close-on-content-click="false"
                                                                                                        transition="scale-transition"
                                                                                                        offset-y=""
                                                                                                        min-width="290px">
                                                                                                    <template v-slot:activator="{ on }">
                                                                                                        <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                                      label="Select Date" disabled
                                                                                                                      prepend-inner-icon="mdi-calendar" dense
                                                                                                                      readonly=""
                                                                                                                      v-on="on"></v-text-field>
                                                                                                    </template>
                                                                                                    <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                                        <v-spacer></v-spacer>
                                                                                                        <v-btn id="AutoTest_IP323" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                                        <v-btn id="AutoTest_IP324" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                                    </v-date-picker>
                                                                                                </v-menu>
                                                                                            </div>
                                                                                            <!--Note-->
                                                                                            <div v-if="vmAnswerType.AnswerType==='Note'" disabled>
                                                                                                <v-textarea disabled></v-textarea>
                                                                                            </div>
                                                                                        </v-col>
                                                                                        <v-col md="3">
                                                                                            <v-select outlined v-model="vmAnswerType" single-line
                                                                                                      :items="lstAnswerTypes"
                                                                                                      item-text="AnswerType"
                                                                                                      item-value="AnswerTypeID"
                                                                                                      :menu-props="{ maxHeight: '300' }"
                                                                                                      dense
                                                                                                      label="Select Question Type"
                                                                                                      return-object
                                                                                                      @change="lstOptions=[],vmCascadingOption=[],SingleChoices = [{ OptionValue: '' }],preCasOption = 0">
                                                                                            </v-select>
                                                                                        </v-col>
                                                                                        <!-- Settings and Validation menu popup starts here-->
                                                                                        <v-col>
                                                                                            <v-menu absolute mx-width="100" v-model="CreateQuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                                <template v-slot:activator="{ on }">
                                                                                                    <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed color="#EEEEEE" class="outlined-btn-dark float-right" @click="GetQuestionValidation(-1)">
                                                                                                        <v-icon>mdi-settings</v-icon>
                                                                                                    </v-btn>
                                                                                                </template>
                                                                                                <v-card>
                                                                                                    <v-card-title class="heading-4">
                                                                                                        <v-spacer></v-spacer>
                                                                                                        <v-btn id="AutoTest_IP325" text height="25" min-width="25" max-width="25" color="#707070" @click="CreateQuestionHintPopUp=false">
                                                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                                                        </v-btn>
                                                                                                    </v-card-title>
                                                                                                    <v-divider></v-divider>
                                                                                                    <v-card-text class="pa-0">
                                                                                                        <v-tabs fixed-tabs color="tab-active-line">
                                                                                                            <v-tab href="#AddNewQstSettTab-1">Setting</v-tab>
                                                                                                            <v-tab href="#AddNewQstSettTab-2">Validation</v-tab>
                                                                                                            <!-- Settings Tab-->
                                                                                                            <v-tab-item class="mt-4 v-scrolling-div" value="AddNewQstSettTab-1" style="width:350px; max-height:250px;">
                                                                                                                <v-list dense>
                                                                                                                    <v-list-item>
                                                                                                                        <h3 class="heading-4 ">Question Hint</h3>
                                                                                                                    </v-list-item>
                                                                                                                    <v-list-item>
                                                                                                                        <v-text-field filled dense label="Hint" v-model="vmQstnHint"></v-text-field>
                                                                                                                    </v-list-item>
                                                                                                                    <v-list-item>
                                                                                                                        <v-list-item-content>
                                                                                                                            Make the Question receive a mandatory response
                                                                                                                        </v-list-item-content>
                                                                                                                        <v-list-item-action>
                                                                                                                            <v-switch color="#707070" v-model="vmISMandatoryResponse"></v-switch>
                                                                                                                        </v-list-item-action>
                                                                                                                    </v-list-item>
                                                                                                                    <v-list-item>
                                                                                                                        <v-list-item-content>
                                                                                                                            Make the Question and its answers unique
                                                                                                                        </v-list-item-content>
                                                                                                                        <v-list-item-action>
                                                                                                                            <v-switch color="#707070" v-model="vmISUnique"></v-switch>
                                                                                                                        </v-list-item-action>
                                                                                                                    </v-list-item>
                                                                                                                    <v-list-item>
                                                                                                                        <v-list-item-content>
                                                                                                                            Make the Question and its answers searchable.
                                                                                                                        </v-list-item-content>
                                                                                                                        <v-list-item-action>
                                                                                                                            <v-switch color="#707070" v-model="vmISSearchable"></v-switch>
                                                                                                                        </v-list-item-action>
                                                                                                                    </v-list-item>
                                                                                                                </v-list>
                                                                                                            </v-tab-item>
                                                                                                            <!-- Validation Tab-->
                                                                                                            <v-tab-item class="mt-4 px-4 v-scrolling-div" value="AddNewQstSettTab-2" style="width:350px; max-height:250px;">
                                                                                                                <v-row>
                                                                                                                    <v-col md="12">
                                                                                                                        <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                                        <v-textarea filled="" label="Enter validation REGEX expression..." v-model="vmRegexExpression"></v-textarea>
                                                                                                                        <h3 class="heading-4 mb-2">Validation Condition</h3>
                                                                                                                        <v-row no-gutters v-for="(item,index) in lstValidationItems">
                                                                                                                            <v-col>
                                                                                                                                <!-- And Or Or Radio Buttons-->
                                                                                                                                <v-row no-gutters v-if="index >0">
                                                                                                                                    <div class="mx-auto">
                                                                                                                                        <v-radio-group row v-model="item.vmOperatorValue" @change="ChangeLogicOperator(item.vmOperatorValue, index)">
                                                                                                                                            <v-radio label="AND" hide-details></v-radio>
                                                                                                                                            <v-radio label="OR" hide-details></v-radio>
                                                                                                                                        </v-radio-group>
                                                                                                                                    </div>
                                                                                                                                </v-row>
                                                                                                                                <v-row>
                                                                                                                                    <v-col>
                                                                                                                                        <v-select dense :items="QstValidationItems" style="width:200px"
                                                                                                                                                  item-text="ValidationCondition" v-model="item.ValidationCondition"
                                                                                                                                                  label="Select validation condition" outlined hide-details return-object>
                                                                                                                                        </v-select>
                                                                                                                                        <v-row v-show="item.ValidationCondition==='Min & Max Range'">
                                                                                                                                            <v-col>
                                                                                                                                                <v-text-field filled dense label="Enter min range" type="number" hide-details="" v-model="item.MinValue"></v-text-field>
                                                                                                                                            </v-col>
                                                                                                                                            <v-col>
                                                                                                                                                <v-text-field hide-details filled dense label="Enter max range" type="number" v-model="item.MaxValue"></v-text-field>
                                                                                                                                            </v-col>
                                                                                                                                        </v-row>
                                                                                                                                    </v-col>
                                                                                                                                    <!-- Plus and Delete Button-->
                                                                                                                                    <v-col md="auto">
                                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline" @click.native="RemoveValidationItem(0,index,'ADD')"></Default-Icon-Button-Outlined>
                                                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1" @click.native="AddValidationItem('ADD',0)"></Default-Icon-Button-Outlined>
                                                                                                                                    </v-col>
                                                                                                                                </v-row>
                                                                                                                            </v-col>
                                                                                                                        </v-row>
                                                                                                                    </v-col>
                                                                                                                </v-row>

                                                                                                            </v-tab-item>
                                                                                                        </v-tabs>
                                                                                                    </v-card-text>
                                                                                                    <v-card-actions>
                                                                                                        <v-row>
                                                                                                            <v-col>
                                                                                                                <Default-Button-Outlined id="AutoTest_IP737" title="Cancel" class="float-right btn-122x36" @click.native="CreateQuestionHintPopUp = false"></Default-Button-Outlined>
                                                                                                                <Secondary-Button id="AutoTest_IP738" title="Save" class="float-right mr-4 btn-122x36" @click.native="CreateQuestionHintPopUp = false"></Secondary-Button>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-card-actions>
                                                                                                </v-card>
                                                                                            </v-menu>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-col>
                                                                            </v-row>

                                                                        </v-col>
                                                                        <v-col class="text-center py-4">
                                                                            <Primary-Button id="AutoTest_IP326" title="Add Questions" @click.native="AddQuestion()" v-bind:disabled="fnCheckMandatory(vmQstn,vmAnswerType.AnswerType)"></Primary-Button>
                                                                        </v-col>
                                                                    </v-card>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <Next-button-primary-light id="AutoTest_IP327" class="mr-4" @click.native="CreateBeneficiaryType()" v-bind:disabled="fnCheckMandatory(vmQstn,vmAnswerType.AnswerType)"></Next-button-primary-light>
                                                            <Cancel-Button-Outlined-Sm id="AutoTest_IP328" @click.native="CloseBTDialogue(),dlgCreateBeneficiaryType=false"></Cancel-Button-Outlined-Sm>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-stepper-content>

                                            <v-stepper-content step="3" class="pa-0">
                                                <v-container fluid>
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
                                                                Your Beneficiary Type has been created successfully. Bask in your glory !
                                                            </p>
                                                        </v-col>
                                                        <v-col md="12" class="text-center">
                                                            <Secondary-Button id="AutoTest_IP329" class="btn-122x36" title="Done" @click.native="dlgCreateBeneficiaryType=false,CreateBeneficiaryTypeSteps = 1"></Secondary-Button>
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

            <!--Edit Beneficiary Type Dialog-->
            <v-dialog v-model="EditBeneficiaryDialog" eager persistent>
                <v-card class="right-fixed-panel">
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-database</v-icon>
                        </v-avatar>Edit Beneficiary Type
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP330" text @click="CloseEditBeneficiaryType()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="py-0">
                        <v-tabs centered v-model="Tab" color="tab-active-line">
                            <v-tab href='#tab-1'>Details</v-tab>
                            <v-tab href='#tab-2'>Registration form</v-tab>

                            <v-tab-item class="mt-4" value="tab-1">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center heading-3 mt-4">Please edit the basic details of the beneficiary type.</h4>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12">
                                        <v-row justify="center">
                                            <v-col md="6">

                                                <v-text-field class="mt-4" outlined dense label="Enter Beneficiary Type Name" v-model="EbenefTypeName">
                                                </v-text-field>
                                                <v-textarea class="mt-4" outlined dense label="Description" v-model="EbenefTypeDesc">
                                                </v-textarea>
                                                <!-- Tags-->
                                                <v-row class="mb-1">
                                                    <v-col class="text-left d-inline-flex" md="12">
                                                        <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                        <v-menu absolute v-model="NewTagMenuedit" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                            <template v-slot:activator="{ on }">
                                                                <v-btn id="AutoTest_IP331" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                                       @click="fnInitializeSteper()" class="mr-1">
                                                                    <v-icon size="15">mdi-plus</v-icon>
                                                                </v-btn>
                                                            </template>
                                                            <v-card>
                                                                <v-card-text class="pt-0 pb-1">
                                                                    <v-col cols="12" md="12">
                                                                        <!-- Header Text  -->
                                                                        <v-col cols="12" md="12" class="d-inline-flex">
                                                                            <v-btn id="AutoTest_IP332" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                <v-icon size="15">mdi-chevron-left</v-icon>
                                                                            </v-btn>
                                                                            <v-spacer></v-spacer>
                                                                            <h3 class="text-left page-head pt-2">
                                                                                {{TagHeaderName}}
                                                                            </h3>
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP333" text @click="NewTagMenuedit = false">
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
                                                                                            <v-btn id="AutoTest_IP334" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
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
                                                                                                <v-chip id="AutoTest_IP335" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                        label @click.native="fnAddToSelectedTag(index)">
                                                                                                    {{ index.TagName }}
                                                                                                    <v-icon id="AutoTest_IP336" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                                </v-chip>
                                                                                            </v-chip-group>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-col cols="12" md="12" class="text-center mt-3">
                                                                                        <v-btn id="AutoTest_IP337" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                                            <v-btn id="AutoTest_IP338" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                        <v-btn id="AutoTest_IP339" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                        </v-btn>
                                                        <v-btn id="AutoTest_IP340" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                        </v-btn>
                                                    </v-col>
                                                </v-row>
                                                <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                    <v-col md="12" class="chip-container mb-0 pt-0">
                                                        <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                            <v-chip id="AutoTest_IP341" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                                {{TagItem.TagName}}
                                                            </v-chip>
                                                        </v-chip-group>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right mb-4">
                                        <Secondary-Button id="AutoTest_IP342" title="Save" class="mr-2 btn-122x36" @click.native="UpdateBeneficiaryTypeDetails(),EditBeneficiaryDialog = false"></Secondary-Button>
                                        <Cancel-Button-Outlined-Sm id="AutoTest_IP343" @click.native="EditBeneficiaryDialog=false,SelectedTag=[]"></Cancel-Button-Outlined-Sm>
                                    </v-col>
                                </v-row>

                            </v-tab-item>

                            <v-tab-item class="mt-4" value="tab-2">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center heading-3">Please edit the questions of the registration form.</h4>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="v-scrolling-div mb-4" style="max-height:80vh; overflow-x:hidden;">
                                        <v-row justify="center">
                                            <v-col md="12">
                                                <v-card outlined>
                                                    <!-- New UI Change Strarts here -->
                                                    <v-col class="v-scrolling-div" style="max-height:65vh">
                                                        <draggable v-model="lstBTQuestions" @update="UpdateQstnOrder(lstBTQuestions)">
                                                            <transition-group type="transition" name="flip-list">
                                                                <div class="drag-cursor" v-for="(item, index) in lstBTQuestions" :key="item.QuestionID">
                                                                    <!--<template v-for="(item,index) in lstBTQuestions">-->
                                                                    <v-row v-if="item.IsEditQuestion == false">
                                                                        <v-col md="5">
                                                                            <p class="font-weight-medium">{{item.QuestionName}}</p>

                                                                            <!--    Answer Types    -->
                                                                            <!----=================-->

                                                                            <v-text-field v-if="item.AnswerType=='Single Line Text'" dense label="Type your answer" disabled />
                                                                            <!--Multiline Answer-->
                                                                            <v-textarea v-if="item.AnswerType==='Multi Line Text'" dense label="Type your answer" disabled />

                                                                            <!--Single Choice -->
                                                                            <div v-if="item.AnswerType==='Single Choice'" class="mt-0">
                                                                                <v-radio-group disabled>
                                                                                    <v-radio v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                             :label="optItem.OptionValue"></v-radio>
                                                                                </v-radio-group>
                                                                            </div>
                                                                            <!--Multiple Choice -->
                                                                            <div v-if="item.AnswerType==='Multi-Choice'" class="mt-0">
                                                                                <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                    <v-checkbox input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                disabled></v-checkbox>

                                                                                </div>
                                                                            </div>
                                                                            <!--Dropdown -->
                                                                            <div v-if="item.AnswerType==='Dropdown'" class="mt-0">
                                                                                <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                    <v-row>
                                                                                        <v-col md="auto">
                                                                                            <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                                                                      item-text="OptionValue"
                                                                                                      label="<-- Select -->" outlined="">
                                                                                            </v-select>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </div>
                                                                            </div>
                                                                            <!--Numeric-->
                                                                            <v-text-field v-if="item.AnswerType==='Numeric'" dense type="number" label="Type your answer" disabled>
                                                                            </v-text-field>

                                                                            <!--Cascading Dropdown -->
                                                                            <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                <v-row>
                                                                                    <v-col>
                                                                                        <v-select dense :menu-props="{ maxHeight: '300' }" filled disabled
                                                                                                  :label="` ${optItem.OptionValue}`" outlined=""
                                                                                                  v-for="(optItem,ind) in item.lstCascadingOptions" :key="ind">
                                                                                        </v-select>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                            <!--GPS Capture-->
                                                                            <div v-if="item.AnswerType==='GPS Capture'">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                    Location
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Photo Capture-->
                                                                            <div v-if="item.AnswerType==='Photo Capture'">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                    Photo Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Audio Capture-->
                                                                            <div v-if="item.AnswerType==='Audio Capture'">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-audiobook</v-icon>
                                                                                    Audio Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Signature Capture-->
                                                                            <div v-if="item.AnswerType==='Signature Capture'">
                                                                                <v-btn outlined="" disabled="">
                                                                                    <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                    Signature Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Date Picker-->
                                                                            <div v-if="item.AnswerType==='Date Picker'">
                                                                                <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                        v-model="SelectAnswerTypeDatepicker"
                                                                                        :close-on-content-click="false"
                                                                                        transition="scale-transition"
                                                                                        offset-y=""
                                                                                        min-width="290px">
                                                                                    <template v-slot:activator="{ on }">
                                                                                        <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                      label="Select Date"
                                                                                                      prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                                                                      readonly disabled
                                                                                                      v-on="on"></v-text-field>
                                                                                    </template>
                                                                                    <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                        <v-spacer></v-spacer>
                                                                                        <v-btn id="AutoTest_IP344" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                        <v-btn id="AutoTest_IP345" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                    </v-date-picker>
                                                                                </v-menu>
                                                                            </div>
                                                                            <!--Note-->
                                                                            <div v-if="item.AnswerType==='Note'"><v-textarea outlined disabled></v-textarea></div>
                                                                        </v-col>
                                                                        <v-col md="3">
                                                                            <v-select dense :menu-props="{ maxHeight: '300' }" filled
                                                                                      :label="` ${item.AnswerType}`" outlined
                                                                                      disabled>
                                                                            </v-select>

                                                                        </v-col>
                                                                        <v-col>
                                                                            <!-- Option Button-->
                                                                            <v-menu absolute v-model="item.QuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed color="#EEEEEE" class="outlined-btn-dark float-right ml-4">
                                                                                        <v-icon>mdi-settings</v-icon>
                                                                                    </v-btn>
                                                                                </template>
                                                                                <v-card>
                                                                                    <v-card-title class="heading-4">
                                                                                        <v-spacer></v-spacer>
                                                                                        <v-btn id="AutoTest_IP347" text height="25" min-width="25" max-width="25" color="#707070" @click="item.QuestionHintPopUp=false">
                                                                                            <v-icon size="20">mdi-close</v-icon>
                                                                                        </v-btn>
                                                                                    </v-card-title>
                                                                                    <v-divider></v-divider>
                                                                                    <v-card-text class="pa-0">
                                                                                        <v-tabs fixed-tabs color="tab-active-line">
                                                                                            <v-tab href="#QstSettTab-1">Setting</v-tab>
                                                                                            <v-tab href="#QstSettTab-2">Validation</v-tab>
                                                                                            <!-- Settings-->
                                                                                            <v-tab-item class="mt-4 v-scrolling-div" value="QstSettTab-1" style="width:350px; max-height:250px;">
                                                                                                <v-list dense>
                                                                                                    <v-list-item>
                                                                                                        <h3 class="heading-4 ">Question Hint</h3>
                                                                                                    </v-list-item>
                                                                                                    <v-list-item>
                                                                                                        <v-text-field filled dense label="Hint" v-model="item.QuestionHint" disabled></v-text-field>
                                                                                                    </v-list-item>
                                                                                                    <v-list-item>
                                                                                                        <v-list-item-content>
                                                                                                            Make the Question receive a mandatory response
                                                                                                        </v-list-item-content>
                                                                                                        <v-list-item-action>
                                                                                                            <v-switch disabled color="#707070" v-model="item.IsMandatoryResponse"></v-switch>
                                                                                                        </v-list-item-action>
                                                                                                    </v-list-item>
                                                                                                    <v-list-item>
                                                                                                        <v-list-item-content>
                                                                                                            Make the Question and its answers unique
                                                                                                        </v-list-item-content>
                                                                                                        <v-list-item-action>
                                                                                                            <v-switch disabled color="#707070" v-model="item.IsUnique"></v-switch>
                                                                                                        </v-list-item-action>
                                                                                                    </v-list-item>
                                                                                                    <v-list-item>
                                                                                                        <v-list-item-content>
                                                                                                            Make the Question and its answers searchable.
                                                                                                        </v-list-item-content>
                                                                                                        <v-list-item-action>
                                                                                                            <v-switch disabled color="#707070" v-model="item.IsSearchable"></v-switch>
                                                                                                        </v-list-item-action>
                                                                                                    </v-list-item>
                                                                                                </v-list>
                                                                                            </v-tab-item>
                                                                                            <!-- Validations-->
                                                                                            <v-tab-item class="mt-4  px-4 v-scrolling-div" value="QstSettTab-2" style="width:350px; max-height:250px;">
                                                                                                <v-row>
                                                                                                    <v-col md="12">
                                                                                                        <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                        <v-textarea disabled filled label="Enter validation REGEX expression..." v-model="item.RegexExpression"></v-textarea>
                                                                                                        <h3 class="heading-4 mb-2">Validation Condition</h3>                                                                            <v-row no-gutters v-for="(ValItem, idx) in item.BTQstnValidationList">
                                                                                                            <v-col>
                                                                                                                <!-- AND Or OR Radio Buttons-->                                                                                              <v-row no-gutters v-if="idx >0">                                                                                                 <div class="mx-auto">                                                                                                            <v-radio-group row v-model="ValItem.LogicOperator">                                                                              <v-radio label="AND" disabled hide-details></v-radio>                                                                         <v-radio label="OR" disabled hide-details></v-radio>                                                                       </v-radio-group>                                                                                                           </div>                                                                                                                     </v-row>                                                                                                                      <v-row>
                                                                                                                    <v-col>
                                                                                                                        <v-select dense :items="QstValidationItems" style="width:200px" item-text="ValidationCondition" v-model="ValItem.ValidationCondition" disabled label="Select validation condition" outlined hide-details return-object>                                                 </v-select>                                                                                                                   <v-row v-show="ValItem.ValidationCondition==='Min & Max Range'">
                                                                                                                            <v-col>                                                                                                                          <v-text-field disabled filled dense label="Enter min range" type="number" v-model="ValItem.MinValue" hide-details></v-text-field>                                                                                                                        </v-col>                                                                                                                      <v-col>
                                                                                                                                <v-text-field disabled hide-details filled dense label="Enter max range" type="number" v-model="ValItem.MaxValue"></v-text-field>
                                                                                                                            </v-col>
                                                                                                                        </v-row>
                                                                                                                    </v-col>                                                                                                                      <v-col md="auto">                                                                                                                <Default-Icon-Button-Outlined disabled id="AutoTest_IP735" hide-details="" class="float-right" icon="mdi-trash-can-outline"></Default-Icon-Button-Outlined>                                                                                                    <Default-Icon-Button-Outlined disabled id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1"></Default-Icon-Button-Outlined>                                                                                                                                  </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-tab-item>
                                                                                        </v-tabs>
                                                                                    </v-card-text>
                                                                                </v-card>
                                                                            </v-menu>
                                                                            <Default-Icon-Button-Outlined class="mr-4 float-right" icon="mdi-trash-can-outline" @click.native="dlgRemoveQuest = true, QuestionID=item.QuestionID"></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined class="mr-4 float-right" icon="mdi-content-copy" @click.native="CurrQstnIndex = index, QuestionID = item.QuestionID,dlgCopyQuestion = true"></Default-Icon-Button-Outlined>
                                                                            <!-- Edit Button-->
                                                                            <Default-Icon-Button-Outlined icon="mdi-pencil-outline" id="AutoTest_IP346" max-width="29" min-width="40" height="37" outlined class="outlined-btn-dark float-right mr-4"
                                                                                                          @click.native="EditQuestion(item,index)" v-bind:disabled="item.IsRegistrationDone==1">
                                                                            </Default-Icon-Button-Outlined>
                                                                        </v-col>
                                                                        <div class="indicator">
                                                                            <v-icon>mdi-blur</v-icon>
                                                                        </div>
                                                                        <v-col md="12">
                                                                            <v-divider></v-divider>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row v-else v-if="item.IsEditQuestion">
                                                                        <v-col md="12" class="active-section">
                                                                            <v-row>
                                                                                <v-col md="6">
                                                                                    <div v-if="!item.IsEditQuestion">
                                                                                        <p class="font-weight-medium">{{item.QuestionName}}</p>
                                                                                    </div>
                                                                                    <div v-if="item.IsEditQuestion">
                                                                                        <v-text-field outlined dense v-model="vmEQuestion[index]" label="Question">
                                                                                        </v-text-field>
                                                                                    </div>
                                                                                    <!--    Answer Types    -->
                                                                                    <!----=================-->

                                                                                    <v-text-field v-if="item.AnswerType=='Single Line Text'" dense label="Type your answer" disabled />
                                                                                    <!--Multiline Answer-->
                                                                                    <v-textarea v-if="item.AnswerType==='Multi Line Text'" dense label="Type your answer" disabled />

                                                                                    <!--Single Choice / Multi choice Answer / DropDown / Cascading Dropdown-->
                                                                                    <div v-if="!item.IsEditQuestion">
                                                                                        <!--Single Choice -->
                                                                                        <div v-if="item.AnswerType==='Single Choice'" class="mt-0">
                                                                                            <v-radio-group disabled>
                                                                                                <v-radio v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                         :label="optItem.OptionValue"></v-radio>
                                                                                            </v-radio-group>
                                                                                        </div>
                                                                                        <!--Multiple Choice -->
                                                                                        <div v-if="item.AnswerType==='Multi-Choice'" class="mt-0">
                                                                                            <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                                <v-checkbox input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                            disabled></v-checkbox>

                                                                                            </div>
                                                                                        </div>
                                                                                        <!--Dropdown -->
                                                                                        <div v-if="item.AnswerType==='Dropdown'" class="mt-0">
                                                                                            <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                                <v-row>
                                                                                                    <v-col md="auto">
                                                                                                        <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                                                                                  item-text="OptionValue"
                                                                                                                  label="<-- Select -->" outlined="">
                                                                                                        </v-select>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>
                                                                                        </div>
                                                                                        <!--Cascading Dropdown -->
                                                                                        <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                            <v-row>
                                                                                                <v-col>
                                                                                                    <v-select dense :menu-props="{ maxHeight: '300' }" filled disabled
                                                                                                              :label="` ${optItem.OptionValue}`" outlined=""
                                                                                                              v-for="(optItem,ind) in item.lstCascadingOptions" :key="ind">
                                                                                                    </v-select>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div v-if="item.IsEditQuestion">
                                                                                        <div v-if="item.AnswerType==='Single Choice' || item.AnswerType==='Multi-Choice' || item.AnswerType==='Dropdown'" class="mt-0">
                                                                                            <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                                <v-row v-for="(SingleChoice, index) in  item.BTQstnAnsOptionList" :key="index">
                                                                                                    <v-col>
                                                                                                        <v-text-field outlined dense label="Enter Option" v-model="SingleChoice.OptionValue">
                                                                                                        </v-text-field>
                                                                                                    </v-col>
                                                                                                    <v-col md="auto">
                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP348" v-show="item.BTQstnAnsOptionList.length>1" class="mr-4" icon="mdi-trash-can-outline" @click.native="EditDeleteOptionItem(SingleChoice,item.QuestionID)"></Default-Icon-Button-Outlined>
                                                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP349" v-show="index == item.BTQstnAnsOptionList.length - 1" icon="mdi-plus" @click.native="EditQstnOptionItem(item.QuestionID)"></Default-Icon-Button-Outlined>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>
                                                                                        </div>

                                                                                        <!--Cascading Dropdown-->
                                                                                        <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                                            <div v-if="item.lstCascadingOptions==null">
                                                                                                <v-row>
                                                                                                    <v-col>
                                                                                                        <v-select v-model="vmECascadingOption"
                                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                                  item-text="OptionValue"
                                                                                                                  item-value="ID"
                                                                                                                  dense :menu-props="{ maxHeight: '300' }" filled
                                                                                                                  label="Select " outlined=""
                                                                                                                  return-object @change="SaveCascadingOptions(vmECascadingOption.ID)">
                                                                                                        </v-select>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>
                                                                                            <div v-else>
                                                                                                <v-row v-for="(optItem, index) in item.lstCascadingOptions.slice(0, 1)" :key="index">
                                                                                                    <v-col>
                                                                                                        <v-select v-model="vmECascadingOption"
                                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                                  item-text="OptionValue"
                                                                                                                  item-value="ID"
                                                                                                                  dense :menu-props="{ maxHeight: '300' }" filled
                                                                                                                  :label="` ${optItem.OptionValue}`" outlined=""
                                                                                                                  return-object @change="SaveCascadingOptions(vmECascadingOption.ID),preCasOption = 1">
                                                                                                        </v-select>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </div>

                                                                                        </div>
                                                                                    </div>
                                                                                    <!--Numeric-->
                                                                                    <v-text-field v-if="item.AnswerType==='Numeric'" dense type="number" label="Type your answer" disabled>
                                                                                    </v-text-field>
                                                                                    <!--GPS Capture-->
                                                                                    <div v-if="item.AnswerType==='GPS Capture'">
                                                                                        <v-btn outlined disabled>
                                                                                            <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                            Location
                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <!--Photo Capture-->
                                                                                    <div v-if="item.AnswerType==='Photo Capture'">
                                                                                        <v-btn outlined disabled>
                                                                                            <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                            Photo Capture
                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <!--Audio Capture-->
                                                                                    <div v-if="item.AnswerType==='Audio Capture'">
                                                                                        <v-btn outlined disabled>
                                                                                            <v-icon class="mr-2">mdi-audiobook</v-icon>
                                                                                            Audio Capture
                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <!--Signature Capture-->
                                                                                    <div v-if="item.AnswerType==='Signature Capture'">
                                                                                        <v-btn outlined disabled>
                                                                                            <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                            Signature Capture
                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <!--Date Picker-->
                                                                                    <div v-if="item.AnswerType==='Date Picker'">
                                                                                        <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                                v-model="SelectAnswerTypeDatepicker"
                                                                                                :close-on-content-click="false"
                                                                                                transition="scale-transition"
                                                                                                offset-y=""
                                                                                                min-width="290px">
                                                                                            <template v-slot:activator="{ on }">
                                                                                                <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                              label="Select Date"
                                                                                                              prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                                                                              readonly disabled
                                                                                                              v-on="on"></v-text-field>
                                                                                            </template>
                                                                                            <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                                <v-spacer></v-spacer>
                                                                                                <v-btn id="AutoTest_IP350" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                                <v-btn id="AutoTest_IP351" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                            </v-date-picker>
                                                                                        </v-menu>
                                                                                    </div>
                                                                                    <!--Note-->
                                                                                    <div v-if="item.AnswerType==='Note'"><v-textarea disabled></v-textarea></div>
                                                                                </v-col>
                                                                                <v-col md="3" v-show="item.IsEditQuestion">
                                                                                    <v-select dense :items="lstAnswerTypes" :menu-props="{ maxHeight: '300' }" filled
                                                                                              item-text="AnswerType"
                                                                                              item-value="AnswerTypeID" return-object
                                                                                              :label="` ${item.AnswerType}`" outlined=""
                                                                                              v-model="vmEAnswerType[index]"
                                                                                              @change="item.AnswerType=vmEAnswerType[index].AnswerType,
                                                                                item.AnswerTypeID=vmEAnswerType[index].AnswerTypeID,lstOptions=[],vmECascadingOption=[],
                                                                                GetCascadingQuestionOptions(),  item.BTQstnAnsOptionList=[{ OptionValue: '' }],preCasOption = 0">
                                                                                    </v-select>
                                                                                </v-col>
                                                                                <v-col md="3" v-show="!item.IsEditQuestion">
                                                                                    <v-select dense :menu-props="{ maxHeight: '300' }" filled
                                                                                              :label="` ${item.AnswerType}`" outlined="" disabled>
                                                                                    </v-select>
                                                                                </v-col>
                                                                                <v-col>
                                                                                    <v-menu absolute mx-width="100" v-model="item.CreateQuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                        <template v-slot:activator="{ on }">
                                                                                            <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed color="#EEEEEE" class="outlined-btn-dark float-right">
                                                                                                <v-icon>mdi-settings</v-icon>
                                                                                            </v-btn>
                                                                                        </template>
                                                                                        <v-card>
                                                                                            <v-card-title class="heading-4">
                                                                                                <v-spacer></v-spacer>
                                                                                                <v-btn id="AutoTest_IP325" text height="25" min-width="25" max-width="25" color="#707070" @click="item.CreateQuestionHintPopUp=false">
                                                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                                                </v-btn>
                                                                                            </v-card-title>
                                                                                            <v-divider></v-divider>
                                                                                            <v-card-text class="pa-0">
                                                                                                <v-tabs fixed-tabs color="tab-active-line">
                                                                                                    <v-tab href="#AddNewQstSettTab-1">Setting</v-tab>
                                                                                                    <v-tab href="#AddNewQstSettTab-2">Validation</v-tab>
                                                                                                    <!-- Settings Tab-->
                                                                                                    <v-tab-item class="mt-4 v-scrolling-div" value="AddNewQstSettTab-1" style="width:350px; max-height:250px;">
                                                                                                        <v-list dense>
                                                                                                            <v-list-item>
                                                                                                                <h3 class="heading-4 ">Question Hint</h3>
                                                                                                            </v-list-item>
                                                                                                            <v-list-item>
                                                                                                                <v-text-field filled dense label="Hint" v-model="item.QuestionHint"></v-text-field>
                                                                                                            </v-list-item>
                                                                                                            <v-list-item>
                                                                                                                <v-list-item-content>
                                                                                                                    Make the Question receive a mandatory response
                                                                                                                </v-list-item-content>
                                                                                                                <v-list-item-action>
                                                                                                                    <v-switch color="#707070" v-model="item.IsMandatoryResponse"></v-switch>
                                                                                                                </v-list-item-action>
                                                                                                            </v-list-item>
                                                                                                            <v-list-item>
                                                                                                                <v-list-item-content>
                                                                                                                    Make the Question and its answers unique
                                                                                                                </v-list-item-content>
                                                                                                                <v-list-item-action>
                                                                                                                    <v-switch color="#707070" v-model="item.IsUnique"></v-switch>
                                                                                                                </v-list-item-action>
                                                                                                            </v-list-item>
                                                                                                            <v-list-item>
                                                                                                                <v-list-item-content>
                                                                                                                    Make the Question and its answers searchable.
                                                                                                                </v-list-item-content>
                                                                                                                <v-list-item-action>
                                                                                                                    <v-switch color="#707070" v-model="item.IsSearchable"></v-switch>
                                                                                                                </v-list-item-action>
                                                                                                            </v-list-item>
                                                                                                        </v-list>
                                                                                                    </v-tab-item>
                                                                                                    <!-- Validation Tab-->
                                                                                                    <v-tab-item class="mt-4 px-4 v-scrolling-div" value="AddNewQstSettTab-2" style="width:350px; max-height:250px;">
                                                                                                        <v-row>
                                                                                                            <v-col md="12">
                                                                                                                <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                                <v-textarea filled="" label="Enter validation REGEX expression..." v-model="item.RegexExpression"></v-textarea>
                                                                                                                <h3 class="heading-4 mb-2">Validation Condition</h3>
                                                                                                                <v-row no-gutters v-for="(Valitem,idx) in item.BTQstnValidationList" :key="idx">
                                                                                                                    <v-col>
                                                                                                                        <!-- And or OR Condtions-->
                                                                                                                        <v-row no-gutters v-if="idx >0">
                                                                                                                            <div class="mx-auto">
                                                                                                                                <v-radio-group row v-model="Valitem.LogicOperator">
                                                                                                                                    <v-radio label="AND" hide-details></v-radio>
                                                                                                                                    <v-radio label="OR" hide-details></v-radio>
                                                                                                                                </v-radio-group>
                                                                                                                            </div>
                                                                                                                        </v-row>
                                                                                                                        <v-row>
                                                                                                                            <v-col>
                                                                                                                                <v-select dense :items="QstValidationItems" style="width:200px"
                                                                                                                                          item-text="ValidationCondition" v-model="Valitem.ValidationCondition"
                                                                                                                                          label="Select validation condition" outlined hide-details return-object>
                                                                                                                                </v-select>
                                                                                                                                <v-row v-show="Valitem.ValidationCondition==='Min & Max Range'">
                                                                                                                                    <v-col>
                                                                                                                                        <v-text-field filled dense label="Enter min range" type="number" hide-details="" v-model="Valitem.MinValue"></v-text-field>
                                                                                                                                    </v-col>
                                                                                                                                    <v-col>
                                                                                                                                        <v-text-field hide-details filled dense label="Enter max range" type="number" v-model="Valitem.MaxValue"></v-text-field>
                                                                                                                                    </v-col>
                                                                                                                                </v-row>
                                                                                                                            </v-col>
                                                                                                                            <v-col md="auto">
                                                                                                                                <Default-Icon-Button-Outlined id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline" @click.native="RemoveValidatinItemInEdit(index,idx)"></Default-Icon-Button-Outlined>
                                                                                                                                <Default-Icon-Button-Outlined id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1" @click.native="AddValidationItemInEdit(index)"></Default-Icon-Button-Outlined>
                                                                                                                            </v-col>
                                                                                                                        </v-row>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-tab-item>
                                                                                                </v-tabs>
                                                                                            </v-card-text>
                                                                                            <v-card-actions>
                                                                                                <!-- Save and Cancel Button-->
                                                                                                <v-row>
                                                                                                    <v-col>
                                                                                                        <Default-Button-Outlined id="AutoTest_IP737" title="Cancel" class="float-right btn-122x36" @click.native="FnUpdateValidation(index)"></Default-Button-Outlined>
                                                                                                        <Secondary-Button id="AutoTest_IP738" title="Save" class="float-right mr-4 btn-122x36" @click.native="item.CreateQuestionHintPopUp = false"></Secondary-Button>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-card-actions>
                                                                                        </v-card>
                                                                                    </v-menu>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right mb-4">

                                                                                    <Secondary-Button id="AutoTest_IP353" title="Save" class="mr-2 btn-122x36" @click.native="item.QuestionName =vmEQuestion[index] ,UpdateBeneficiaryTypeQuestion(item)"
                                                                                                      v-bind:disabled="fnCheckMandatory(vmEQuestion[index],item.AnswerType,item)"></Secondary-Button>
                                                                                    <Cancel-Button-Outlined-Sm id="AutoTest_IP354" @click.native="CancelEditQuestion(item)"></Cancel-Button-Outlined-Sm>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <!--</template>-->
                                                                </div>
                                                            </transition-group>
                                                        </draggable>
                                                        <v-row>
                                                            <v-col class="py-4 active-section">
                                                                <v-row>
                                                                    <v-col md="6">
                                                                        <v-text-field filled dense label="Untitled Question" v-model="vmQstnE" v-bind:disabled="!IsAddNew"> </v-text-field>

                                                                        <!--    Answer Types   -->
                                                                        <!----=================-->

                                                                        <v-text-field v-if="vmAnswerTypeE.AnswerType=='Single Line Text'" dense label="Type your answer" disabled />
                                                                        <!--Multiline Answer-->
                                                                        <v-textarea v-if="vmAnswerTypeE.AnswerType==='Multi Line Text'" dense label="Type your answer" disabled />
                                                                        <!--Single Choice / Multi choice Answer / DropDown-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Single Choice' || vmAnswerTypeE.AnswerType==='Multi-Choice' || vmAnswerTypeE.AnswerType==='Dropdown'" class="mt-0">
                                                                            <v-row v-for="(SingleChoice, index) in SingleChoices" :key="index">
                                                                                <v-col>
                                                                                    <v-text-field dense label="Enter Option" v-model="SingleChoice.OptionValue">
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                                <v-col md="auto">
                                                                                    <Default-Icon-Button-Outlined id="AutoTest_IP355" v-show="SingleChoices.length>1" class="mr-4" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(SingleChoice)"></Default-Icon-Button-Outlined>
                                                                                    <Default-Icon-Button-Outlined id="AutoTest_IP356" v-show="index == SingleChoices.length - 1" icon="mdi-plus" @click.native="AddOptionItem()"></Default-Icon-Button-Outlined>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                        <!--Numeric-->
                                                                        <v-text-field v-if="vmAnswerTypeE.AnswerType==='Numeric'" dense type="number" label="Type your answer" disabled>
                                                                        </v-text-field>


                                                                        <!--Cascading Dropdown-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <v-select v-model="vmCascadingOption" single-line
                                                                                              :items="lstCascadingQstnOptions"
                                                                                              item-text="OptionValue"
                                                                                              item-value="ID"
                                                                                              :menu-props="{ maxHeight: '300' }"
                                                                                              outlined dense
                                                                                              label="<-- Select -->" return-object @change="SaveCascadingOptions(vmCascadingOption.ID)">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                        <!--GPS Capture-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='GPS Capture'">
                                                                            <v-btn outlined disabled>
                                                                                <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                Location
                                                                            </v-btn>
                                                                        </div>
                                                                        <!--Photo Capture-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Photo Capture'">
                                                                            <v-btn outlined disabled>
                                                                                <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                Photo Capture
                                                                            </v-btn>
                                                                        </div>
                                                                        <!--Audio Capture-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Audio Capture'">
                                                                            <v-btn outlined disabled>
                                                                                <v-icon class="mr-2">mdi-audiobook</v-icon>
                                                                                Audio Capture
                                                                            </v-btn>
                                                                        </div>
                                                                        <!--Signature Capture-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Signature Capture'">
                                                                            <v-btn outlined disabled>
                                                                                <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                Signature Capture
                                                                            </v-btn>
                                                                        </div>
                                                                        <!--Date Picker-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Date Picker'">
                                                                            <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                    v-model="SelectAnswerTypeDatepicker"
                                                                                    :close-on-content-click="false"
                                                                                    transition="scale-transition"
                                                                                    offset-y=""
                                                                                    min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                  label="Select Date"
                                                                                                  prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                                                                  readonly disabled
                                                                                                  v-on="on"></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                    <v-spacer></v-spacer>
                                                                                    <v-btn id="AutoTest_IP357" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                    <v-btn id="AutoTest_IP358" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(date)">OK</v-btn>
                                                                                </v-date-picker>
                                                                            </v-menu>
                                                                        </div>
                                                                        <!--Note-->
                                                                        <div v-if="vmAnswerTypeE.AnswerType==='Note'"><v-textarea disabled></v-textarea></div>
                                                                    </v-col>
                                                                    <v-col md="4">
                                                                        <v-select v-model="vmAnswerTypeE" single-line
                                                                                  :items="lstAnswerTypes"
                                                                                  item-text="AnswerType"
                                                                                  item-value="AnswerTypeID"
                                                                                  :menu-props="{ maxHeight: '300' }"
                                                                                  outlined dense
                                                                                  label="Select Question Type"
                                                                                  return-object v-bind:disabled="!IsAddNew" @change="lstOptions=[],vmCascadingOption=[],SingleChoices = [{ OptionValue: '' }],preCasOption = 0">
                                                                        </v-select>
                                                                    </v-col>
                                                                    <v-col>
                                                                        <v-menu absolute mx-width="100" v-model="EditQuestionHintPopUp" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick" v-bind:disabled="!IsAddNew">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-btn v-on="on" max-width="29" min-width="40" height="37" depressed color="#EEEEEE" class="outlined-btn-dark float-right">
                                                                                    <v-icon>mdi-settings</v-icon>
                                                                                </v-btn>
                                                                            </template>
                                                                            <v-card>
                                                                                <v-card-title class="heading-4">
                                                                                    <v-spacer></v-spacer>
                                                                                    <v-btn id="AutoTest_IP359" text height="25" min-width="25" max-width="25" color="#707070" @click="EditQuestionHintPopUp=false">
                                                                                        <v-icon size="20">mdi-close</v-icon>
                                                                                    </v-btn>
                                                                                </v-card-title>
                                                                                <v-divider></v-divider>
                                                                                <v-card-text class="pa-0">
                                                                                    <v-tabs fixed-tabs color="tab-active-line">
                                                                                        <v-tab href="#AddNewQstSettTab-1">Setting</v-tab>
                                                                                        <v-tab href="#AddNewQstSettTab-2">Validation</v-tab>
                                                                                        <!-- Settings Tab-->
                                                                                        <v-tab-item class="mt-4 v-scrolling-div" value="AddNewQstSettTab-1" style="width:350px; max-height:250px;">
                                                                                            <v-list dense>
                                                                                                <v-list-item>
                                                                                                    <h3 class="heading-4 ">Question Hint</h3>
                                                                                                </v-list-item>
                                                                                                <v-list-item>
                                                                                                    <v-text-field filled dense label="Hint" v-model="vmQstnHintE"></v-text-field>
                                                                                                </v-list-item>
                                                                                                <v-list-item>
                                                                                                    <v-list-item-content>
                                                                                                        Make the Question receive a mandatory response
                                                                                                    </v-list-item-content>
                                                                                                    <v-list-item-action>
                                                                                                        <v-switch color="#707070" v-model="vmISMandatoryResponseE"></v-switch>
                                                                                                    </v-list-item-action>
                                                                                                </v-list-item>
                                                                                                <v-list-item>
                                                                                                    <v-list-item-content>
                                                                                                        Make the Question and its answers unique
                                                                                                    </v-list-item-content>
                                                                                                    <v-list-item-action>
                                                                                                        <v-switch color="#707070" v-model="vmISUniqueE"></v-switch>
                                                                                                    </v-list-item-action>
                                                                                                </v-list-item>
                                                                                                <v-list-item>
                                                                                                    <v-list-item-content>
                                                                                                        Make the Question and its answers searchable.
                                                                                                    </v-list-item-content>
                                                                                                    <v-list-item-action>
                                                                                                        <v-switch color="#707070" v-model="vmISSearchableE"></v-switch>
                                                                                                    </v-list-item-action>
                                                                                                </v-list-item>
                                                                                            </v-list>
                                                                                        </v-tab-item>
                                                                                        <!-- Validation Tab-->
                                                                                        <v-tab-item class="mt-4 px-4 v-scrolling-div" value="AddNewQstSettTab-2" style="width:350px; max-height:250px;">
                                                                                            <v-row>
                                                                                                <v-col md="12">
                                                                                                    <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                                                                                    <v-textarea filled="" label="Enter validation REGEX expression..." v-model="vmRegexExpressionE"></v-textarea>
                                                                                                    <h3 class="heading-4 mb-2">Validation Condition</h3>
                                                                                                    <v-row no-gutters v-for="(item,index) in lstValidationItems">
                                                                                                        <v-col>
                                                                                                            <!-- And Or Or Radio Buttons-->
                                                                                                            <v-row no-gutters v-if="index >0">
                                                                                                                <div class="mx-auto">
                                                                                                                    <v-radio-group row v-model="item.vmOperatorValue" @change="ChangeLogicOperator(item.vmOperatorValue, index)">
                                                                                                                        <v-radio label="AND" hide-details></v-radio>
                                                                                                                        <v-radio label="OR" hide-details></v-radio>
                                                                                                                    </v-radio-group>
                                                                                                                </div>
                                                                                                            </v-row>
                                                                                                            <v-row>
                                                                                                                <v-col>
                                                                                                                    <v-select dense :items="QstValidationItems" style="width:200px"
                                                                                                                              item-text="ValidationCondition" v-model="item.ValidationCondition"
                                                                                                                              label="Select validation condition" outlined hide-details return-object>
                                                                                                                    </v-select>
                                                                                                                    <v-row v-show="item.ValidationCondition==='Min & Max Range'">
                                                                                                                        <v-col>
                                                                                                                            <v-text-field filled dense label="Enter min range" type="number" hide-details="" v-model="item.MinValue"></v-text-field>
                                                                                                                        </v-col>
                                                                                                                        <v-col>
                                                                                                                            <v-text-field hide-details filled dense label="Enter max range" type="number" v-model="item.MaxValue"></v-text-field>
                                                                                                                        </v-col>
                                                                                                                    </v-row>
                                                                                                                </v-col>
                                                                                                                <!-- Plus and Delete Button-->
                                                                                                                <v-col md="auto">
                                                                                                                    <Default-Icon-Button-Outlined id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline" @click.native="RemoveValidationItem(0,index,'ADD')"></Default-Icon-Button-Outlined>
                                                                                                                    <Default-Icon-Button-Outlined id="AutoTest_IP736" icon="mdi-plus" class="ml-1 mr-1" @click.native="AddValidationItem('ADD',0)"></Default-Icon-Button-Outlined>
                                                                                                                </v-col>
                                                                                                            </v-row>
                                                                                                        </v-col>
                                                                                                    </v-row>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                            <v-row>
                                                                                                <v-col>
                                                                                                    <Default-Button-Outlined id="AutoTest_IP737" title="Cancel" class="float-right btn-122x36" @click.native="EditQuestionHintPopUp = false"></Default-Button-Outlined>
                                                                                                    <Secondary-Button id="AutoTest_IP738" title="Save" class="float-right mr-4 btn-122x36" @click.native="EditQuestionHintPopUp = false"></Secondary-Button>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                        </v-tab-item>
                                                                                    </v-tabs>
                                                                                </v-card-text>
                                                                            </v-card>
                                                                        </v-menu>
                                                                    </v-col>
                                                                </v-row>
                                                                <!--<Primary-Button title="Add Questions" @click.native="AddNewQuestion()" v-bind:disabled="IsUpdateQuestion"></Primary-Button>-->
                                                                <v-row>
                                                                    <v-col md="12" class="text-center">
                                                                        <Primary-Button id="AutoTest_IP360" title="Add Questions" @click.native="IsAddNew=true" v-bind:disabled="IsUpdateQuestion || IsAddNew "></Primary-Button>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>

                                                    </v-col>

                                                </v-card>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right mb-4" v-if="!IsUpdateQuestion">

                                        <Secondary-Button id="AutoTest_IP361" title="Save" class="mr-4 btn-122x36" @click.native="UpdateBeneficiaryTypeQuestionsAddNew()"
                                                          v-bind:disabled="(!(IsAddNew && vmAnswerTypeE.AnswerTypeID >0) ||
                                                      fnCheckMandatory(vmQstnE,vmAnswerTypeE.AnswerType))"></Secondary-Button>
                                        <Cancel-Button-Outlined-Sm id="AutoTest_IP362" @click.native="CloseEditBeneficiaryType()"></Cancel-Button-Outlined-Sm>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                        </v-tabs>
                    </v-card-text>
                    <v-card-actions>
                        <!--<v-col cols="12" xs="12" sm="12" md="12" class="text-right mb-4">
                    <Secondary-Button title="Save" class="mr-2 btn-122x36" @click.native="UpdateBeneficiaryType(),EditBeneficiaryDialog = false"></Secondary-Button>
                    <Cancel-Button-Outlined-Sm @click.native="EditBeneficiaryDialog=false"></Cancel-Button-Outlined-Sm>
                </v-col>-->
                    </v-card-actions>
                </v-card>
            </v-dialog>

            <!-- Remove Beneficiary Type-->
            <v-dialog v-model="dlgRemoveBenefType" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                        </v-avatar>
                        Remove Beneficiary
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP363" text="" @click="dlgRemoveBenefType=false,isBulk =false,vmBeneficiaryType=[]">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="pt-4">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0" v-if="!isBulk">
                                                <h4 class="text-center heading-3">Are you sure you want to remove this beneficiary type ?</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="pt-0" v-if="isBulk">
                                                <h4 class="text-center heading-3">Are you sure you want to remove the selected beneficiary types ?</h4>
                                            </v-col>
                                            <v-col v-if="!isBulk">
                                                <h4 class="text-center heading-3">
                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-database</v-icon>
                                                    {{BenefTypeName}}
                                                </h4>
                                            </v-col>

                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_IP364" color="secondary elevation-0" class="mr-2" @click="RemoveBeneficiaryType(isBulk)">Remove</v-btn>
                                                <v-btn id="AutoTest_IP365" outlined color="grey" @click="dlgRemoveBenefType=false,isBulk =false,vmBeneficiaryType=[]">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <v-snackbar v-model="snackbarBulkActions" top color="primary" v-if="isBulk && vmBeneficiaryType.length ==0">
                <template>
                    <div class="pa-0 ma-0">
                        <v-alert dense type="info" color="primary" class="pa-0 ma-0">* Please select Beneficiary Type from the list</v-alert>
                    </div>
                </template>
            </v-snackbar>

        </v-container>
    
        <!-- Remove Section Question-->
        <v-dialog v-model="dlgRemoveQuest" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Question
                    <v-spacer></v-spacer>
                    <v-btn  text @click="dlgRemoveQuest=false">
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
                                            <h4 class="text-center heading-3">Are you sure you want to remove this question ?</h4>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn color="secondary elevation-0" class="mr-4" @click="RemoveBeneficiaryTypeQuestion(), dlgRemoveQuest=false">Remove</v-btn>
                                            <v-btn outlined color="grey" @click="dlgRemoveQuest=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
         <!-- Question Copy Section-->
        <v-dialog v-model="dlgCopyQuestion" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-content-copy</v-icon>
                    </v-avatar>
                    Copy Question
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP729" text @click="dlgCopyQuestion=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-4">
                        <v-row no-gutters="">
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Please select the details you want to copy to create a duplicate question</h4>
                                        </v-col>
                                        <v-row justify-center>
                                            <v-col cols="6" md="6" class="pt-0">
                                                <v-checkbox hide-details input-value="true" label="Copy same settings" v-model="vmCopySettings"></v-checkbox>
                                                <v-checkbox hide-details input-value="true" label="Copy same validations" v-model="vmCopyValidation"></v-checkbox>
                                            </v-col>
                                        </v-row>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn color="secondary elevation-0" class="mr-4" @click="fnCopyQuestion()">Confirm</v-btn>
                                            <v-btn outlined color="grey" @click="dlgCopyQuestion=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>

        <!--snckbar-->
        <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0" v-bind:disabled="isBulk && vmBeneficiaryType.length ==0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0" v-bind:disabled="isBulk && vmBeneficiaryType.length ==0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0" v-bind:disabled="isBulk && vmBeneficiaryType.length ==0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </div>
</template>
<script type="text/javascript">
    import DefaultIconButtonOutlined from '@/components/default-icon-button-outlined.vue'
    import AddTag from '@/components/add-tag.vue'
    import SuccessButton from '@/components/success-button.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import SuccessButtonSm from '@/components/success-button-sm.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import draggable from 'vuedraggable'
    import objUtils from '../../utils.js'

    var objPB;
    async function importscript() {
        return Promise.all([
            import("../../BL/ProjectBeneficiary.js").then(mod => {
                objPB = new mod.ProjectBeneficiary();
            })
        ]);
    };

    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                PreLoader: false,
                snackbarBulkActions :false,
                EditBeneficiaryDialog: false,
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectID: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                selectedItem: "",
                items: [
                    { text: 'This month' },
                    { text: 'This quarter' },
                    { text: 'This Year' },
                ],
                filterContent: false,
                dateRangeHeaderName: 'Date Range',
                dateRangeMenu: false,
                filterChipsList: [],
                SortMenu: false,
                //--- Create Beneficiary Type ---//

                dlgCreateBeneficiaryType: false,
                CreateBeneficiaryTypeSteps: 1,

                txtBTypeName: "",
                txtBTypeDescription: "",
                TagList: [],
                benefTypeTagItems: [],

                lstQstns: [],
                lstAnswerTypes: [],
                vmAnswerType: [],
                QuestionID: 0,
                TempQustId: 0,
                CurrQstnIndex: -1,

                vmQstn: "",
                vmQstnHint: "",
                vmISMandatoryResponse: false,
                vmISUnique: false,
                vmISSearchable: false,
                CreateQuestionHintPopUp: false,
                EditQuestionHintPopUp: false,
                QuestionHintPopUp: false,
                SelectAnswerTypeDate: null,

                // Tags ------------
                NewTagMenu: false,
                NewTagMenuedit: false,
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
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                SearchTagDetails: "",

                //-- Beneficiary Types Listing-----//
                BenTypeTableheaders: [
                    { text: 'Name', value: 'Name' },
                    { text: 'Created Date', value: 'CreatedDate' },
                    { text: 'Last Modified', value: 'UpdatedDate' },
                    { text: 'Action',align:"center", value: 'BenTypeActions', width:"100px" },
                ],
                lstAllBeneficiaryTypes: [],
                FilterlstAllBeneficiaryTypes: [],
                searchBenefType: "",
                vmBeneficiaryType: [],
                filterAllTagList: [],

                //-- Beneficiary Types Removing-----//
                isBulk: false,
                BenefTypeID: 0,
                BenefTypeName: "",
                dlgRemoveBenefType: false,
                Msg: "",
                removeList: [],

                EbenefTypeName: "",
                EbenefTypeDesc: "",
                EbenefTypeTagItems: [],

                vmQstnE: "", vmAnswerTypeE: [], vmQstnHintE: "",
                vmISMandatoryResponseE: false,
                vmISUniqueE: false, vmISSearchableE: false,

                vmEAnswerType: [{ AnswerTypeID: 0, AnswerType: "" }],
                vmAnswerTypeInEdit: [{ AnswerTypeID: 0, AnswerType: "" }],
                lstBTQuestions: [],
                lstOptions: [],
                SingleChoices: [
                    { OptionValue: "" }
                ],               
                lstQAnswerOptions: [],
                SelectAnswerTypeDatepicker: false,
                IsUpdateQuestion: false,
                IsViewQuestion: true,
                IsAddNew: false,
                Tab: 'tab-1',
                lstCascadingQstnOptions: [],
                lstCascadingQstnSubOptions:[],
                vmCascadingOption: [],
                vmECascadingOption: [],
                vmEQuestion: [],
                vmEQstn: [],
                preQstn: "",
                preAnsType: "",
                preQstn: 0,
                preCasOption :0,

                //Filtration
                FiterStartDate: new Date().toISOString().substr(0, 10),
                FiterEndDate: new Date().toISOString().substr(0, 10),
                ValidationCondition:'',
                QstValidationItems: [
                    'Email', 'Phone Number', 'Should be less than Current date', 'Should be greater than Current date', 'Min & Max Range', 'Aadhaar Number'
                ],
                dlgRemoveQuest:false,
                dlgCopyQuestion:false,
                addNewisEdit: false,
                // Beneficiary Question validation
                lstValidationItems :[{
                    QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                    MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName : '', AnswerTypeID : 0
                }],
                vmRegexExpression: '',
                vmCopySettings: true,
                vmCopyValidation: true,
                vmRegexExpressionE: '',
                maxQuestionOrder:0

            }
        },
        components: {
            'Default-Icon-Button-Outlined': DefaultIconButtonOutlined,
            'add-tag': AddTag,
            'Success-Button': SuccessButton,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Primary-Button': PrimaryButton,
            'Success-Button-Sm': SuccessButtonSm,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
            draggable
        },
        mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            await importscript();
            await this.GetAllBeneficiaryTypes();
            await this.GetAnswerTypes();
            await this.GetCascadingQuestionOptions();
            await this.fnGetAllTags();
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

        },
        methods: {      
            /// <summary>
            ///  AddeditQustion :- Function used to edit question in create beneficiary type section
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            AddeditQustion(index) {
                this.addNewisEdit = true;
                this.vmAnswerTypeInEdit = [];
                for (var idx = 0; idx <= this.lstQstns.length - 1; idx++) {
                    this.lstQstns[idx].IsEditQstn = false;
                }
                this.lstQstns[index].IsEditQstn = true;
                this.vmEQstn[index] = this.lstQstns[index].Question;
                var AnswerTypeID = this.lstQstns[index].AnswerTypeID;
                this.vmAnswerTypeInEdit = this.lstAnswerTypes.filter(x => x.AnswerTypeID === AnswerTypeID)[0];
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
            ///  GetAllBeneficiaryTypes :- Function used to fetch all beneficiary types based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllBeneficiaryTypes() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                var result = await objPB.GetAllBeneficiaryTypes(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstAllBeneficiaryTypes = result.data;
                        this.FilterlstAllBeneficiaryTypes = result.data;
                    }
                
                this.PreLoader = false;
            },

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter beneficiary types based on date range option
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {
                this.filterChipsList = [];
                this.FilterlstAllBeneficiaryTypes = this.lstAllBeneficiaryTypes;

                //date range filter
                if (this.FiterStartDate != '' && this.FiterEndDate != '') {

                    this.filterChipsList.push({ ChipID: 777, ChipName: objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate) });
                    this.dateRangeHeaderName = objUtils.common.filterdateformat(this.FiterStartDate) + "-" + objUtils.common.filterdateformat(this.FiterEndDate);
                    this.FilterlstAllBeneficiaryTypes = this.FilterlstAllBeneficiaryTypes.filter(x => x.CreatedDate.substr(0, 10) >= this.FiterStartDate && x.UpdatedDate.substr(0, 10) <= this.FiterEndDate);
                } else {
                    this.dateRangeHeaderName = "Date Range";
                }
            },

            ///sorting beneficiary types

            fnBeneficiaryTypeSort(type) {
                this.SortMenu = false;
                switch (type) {
                    case "Name-Ascending":
                        this.FilterlstAllBeneficiaryTypes.sort((a, b) => (a.Name > b.Name) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.FilterlstAllBeneficiaryTypes.sort((a, b) => (a.Name < b.Name) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.FilterlstAllBeneficiaryTypes.sort((a, b) => (a.BeneficiaryTypeID < b.BeneficiaryTypeID) ? 1 : -1);
                        break;
                    default:
                    // code block
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
            ///  IsValidDate :- Function used to check filter start date and filter end date is valid 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.FiterEndDate) < Date.parse(this.FiterStartDate)) {
                    this.Msg = ("Invalid Date : End date should be greater than Start Date");
                    this.infoSnackbarMsg = true;
                    this.FiterEndDate = "";
                    return;
                }
            },

            /// <summary>
            ///  GetBeneficiaryTypeDetails :- Function used to fetch individual beneficiary type details for edit based on BeneficiaryTypeID
            /// </summary>
            /// <param name="BenefTID"></param>
            /// <returns> </returns>
            async GetBeneficiaryTypeDetails(BenefTID) {
                this.maxQuestionOrder = 0;
                this.PreLoader = true;
                this.BenefTypeID = BenefTID;
                var result = await objPB.GetBeneficiaryTypeDetails(BenefTID);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                      
                        const ids = result.data[0].BTQuestions.map(object => {
                            return object.QuestionOrder;
                        });

                        this.maxQuestionOrder = Math.max(...ids);

                        this.lstBeneficiaryTypeDetails = result.data;
                        this.SelectedTag = JSON.parse(this.lstBeneficiaryTypeDetails[0].BeneficiaryTypeTags);
                        this.EbenefTypeName = this.lstBeneficiaryTypeDetails[0].Name;
                        this.EbenefTypeDesc = this.lstBeneficiaryTypeDetails[0].Description == null ? "" : this.lstBeneficiaryTypeDetails[0].Description;
                        this.lstBTQuestions = this.lstBeneficiaryTypeDetails[0].BTQuestions;

                        for (var i = 0; i < this.lstBTQuestions.length; i++) {
                            this.lstBTQuestions[i].EAnswerType = this.lstBTQuestions[i].AnswerType;
                            this.lstBTQuestions[i].IsEditQuestion = false;
                            if (this.lstBTQuestions[i].BTQstnAnsOptionList.length == 0)
                                this.lstBTQuestions[i].BTQstnAnsOptionList = [{ OptionValue: "" }];

                            if (this.lstBTQuestions[i].EAnswerType == "Cascading Dropdown") {
                                this.lstBTQuestions[i].lstCascadingOptions = this.lstBTQuestions[i].BTQstnAnsOptionList;
                                this.lstBTQuestions[i].BTQstnAnsOptionList = [{ OptionValue: "" }];
                            }
                            this.lstBTQuestions[i].IsEditQstn = false;
                        }
                        this.PreLoader = false;
                        this.EditBeneficiaryDialog = true;
                    }
            },

            /// <summary>
            ///  GetAnswerTypes :- Function used to fetch answer types while create new beneficiary type
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAnswerTypes() {
                var result = await objPB.GetAnswerTypes();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        this.lstAnswerTypes = result.data;
            },

            /// <summary>
            ///  GetCascadingQuestionOptions :- Function used to fetch cascading question options (Country, States, District, SubDistrict)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetCascadingQuestionOptions() {
                var result = await objPB.GetCascadingQuestionOptions();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstCascadingQstnOptions = result.data;
                    }
            },

            /// <summary>
            ///  SaveCascadingOptions :- Function used to save cascading question sub options based on OptionID
            /// </summary>
            /// <param name="OptionID"></param>
            /// <returns> </returns>
            async SaveCascadingOptions(OptionID) {
                var result = await objPB.GetCascadingQuestionSubOptions(OptionID);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstCascadingQstnSubOptions = result.data;
                    }
                this.lstOptions = [];
               
                for (var i = 0; i < this.lstCascadingQstnSubOptions.length; i++)
                    this.lstOptions.push({ OptionValue: this.lstCascadingQstnSubOptions[i].OptionValue });

            },

            /// <summary>
            ///  AddQuestion :- Function used to add new questions while beneficiary type creation
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddQuestion() {
                var IsValidationQuestion = this.lstValidationItems.length > 0 ? 1 : 0;
                if (this.lstValidationItems.length == 1) {
                    if (this.lstValidationItems[0].ValidationCondition == "") {
                        IsValidationQuestion = 0;
                    }
                }
                var CurrentQuestionOrder = (this.lstQstns != null && this.lstQstns != undefined && this.lstQstns.length > 0) ? (this.lstQstns.length + 1) : 1;
                this.lstQstns.push({
                    QuestionID:0, Question: this.vmQstn, AnswerType: this.vmAnswerType.AnswerType, AnswerTypeID: this.vmAnswerType.AnswerTypeID,
                    QuestionHint: this.vmQstnHint, IsMandatoryResponse: this.vmISMandatoryResponse,
                    IsUnique: this.vmISUnique, IsSearchable: this.vmISSearchable, BTQstnAnsOptionList: this.lstOptions, lstQuestionValidations: this.lstValidationItems, IsValidationQuestion: IsValidationQuestion, RegexExpression: this.vmRegexExpression, IsEditQstn: false, QuestionOrder: CurrentQuestionOrder
                });
                
                this.ClearQuestionDetails();               
            },

            /// <summary>
            ///  ClearQuestionDetails :- Function used to clear question details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            ClearQuestionDetails() {
                this.vmQstn = "";
                this.vmAnswerType = [];
                this.SingleChoices = [{ OptionValue: "" }];
                this.vmISMandatoryResponse = false;
                this.vmISUnique = false;
                this.vmISSearchable = false;
                this.vmQstnHint = "";
                this.lstOptions = [];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.lstValidationItems = [];
                this.lstValidationItems.push({
                    QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                    MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                });
                this.vmRegexExpression = "";
            },

            /// <summary>
            ///  fnCheckMandatory :- Function used to check mandatory field
            /// </summary>
            /// <param name="NewQstn"></param>
            /// <param name="AnsType"></param>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            fnCheckMandatory(NewQstn, AnsType, QItem) {
               
                 var IsNotValid = false;
                if (!this.IsUpdateQuestion) {
                   
                    if (AnsType == null || NewQstn == null)
                        IsNotValid = true;
                    if (AnsType == 'Cascading Dropdown' || AnsType == 'Dropdown' || AnsType == 'Multi-Choice' || AnsType == 'Single Choice') {
                        if (this.lstOptions.filter(x => x.OptionValue != null).length == 0 || this.lstOptions.filter(x => x.OptionValue != []).length == 0)
                            IsNotValid = true;
                    }
                }
                else {
                    if (!(AnsType == 'Cascading Dropdown' || AnsType == 'Dropdown' || AnsType == 'Multi-Choice' || AnsType == 'Single Choice')) {
                        // if (this.preQstn == NewQstn && this.preAnsType == AnsType)
                        //    IsNotValid = true;
                    }
                    if (AnsType == 'Dropdown' || AnsType == 'Multi-Choice' || AnsType == 'Single Choice') {
                        
                        if (QItem.BTQstnAnsOptionList.filter(x => x.OptionValue != null).length == 0 || QItem.BTQstnAnsOptionList.filter(x => x.OptionValue != []).length == 0)
                            IsNotValid = true;

                    }
                    if (AnsType == 'Cascading Dropdown') {
                        if (this.preAnsType != AnsType) {
                            if (this.lstOptions.filter(x => x.OptionValue != null).length == 0 || this.lstOptions.filter(x => x.OptionValue != []).length == 0)
                                IsNotValid = true;
                        }
                        else if (this.preQstn == NewQstn && this.preCasOption == 0)
                            IsNotValid = true;
                    }
                }
                if(NewQstn == '')
                    {
                    IsNotValid = true;
                    }
                 return IsNotValid;
            },

            /// <summary>
            ///  CreateBeneficiaryType :- Function used to create a new beneficiary type
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async CreateBeneficiaryType() {
                for (var i = 0; i < this.lstAllBeneficiaryTypes.length; i++) {
                    if (this.lstAllBeneficiaryTypes[i].Name == this.txtBTypeName) {
                        this.Msg = "Duplicate Entry - Same Beneficiary Type Name exists.. ";
                        this.infoSnackbarMsg = true;
                        this.CreateBeneficiaryTypeSteps = 1;
                        return;
                    }
                }
                if (this.vmQstn != "" && this.vmAnswerType != []) {
                    if (this.vmAnswerType.AnswerType == 'Cascading Dropdown' || this.vmAnswerType.AnswerType == 'Dropdown' ||
                        this.vmAnswerType.AnswerType == 'Multi-Choice' || this.vmAnswerType.AnswerType == 'Single Choice') {
                        if (this.lstOptions.filter(x => x.OptionValue != null).length == 0) {
                            this.Msg = "Please Add/Select  Options"
                            this.infoSnackbarMsg = true;
                            return;
                        }
                    }
                }
                var IsValidationQuestion = this.lstValidationItems.length > 0 ? 1 : 0;
                if (this.lstValidationItems.length == 1) {
                    if (this.lstValidationItems[0].ValidationCondition == "") {
                        IsValidationQuestion = 0;
                    }
                }

                var CurrentQuestionOrder = (this.lstQstns != null && this.lstQstns != undefined && this.lstQstns.length > 0) ? (this.lstQstns.length + 1) : 1;

                this.lstQstns.push({
                    QuestionID :0,Question: this.vmQstn, AnswerType: this.vmAnswerType.AnswerType, AnswerTypeID: this.vmAnswerType.AnswerTypeID,
                    QuestionHint: this.vmQstnHint, IsMandatoryResponse: this.vmISMandatoryResponse,
                    IsUnique: this.vmISUnique, IsSearchable: this.vmISSearchable, BTQstnAnsOptionList: this.lstOptions, lstQuestionValidations: this.lstValidationItems, IsValidationQuestion: IsValidationQuestion, RegexExpression: this.vmRegexExpression, IsEditQstn: false, QuestionOrder: CurrentQuestionOrder
                });
              
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("Name", this.txtBTypeName);
                data.append("Description", this.txtBTypeDescription);
                data.append("BeneficiaryTypeTags", JSON.stringify(this.SelectedTag));
                data.append("BeneficiaryTypeQuestions", JSON.stringify(this.lstQstns));
                var result = await objPB.CreateBeneficiaryType(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else 
                    if (result.status == "success") {
                        this.CreateBeneficiaryTypeSteps = 3;
                        this.Msg = "Beneficiary Type has been created successfully";
                        this.successSnackbarMsg = true;
                        this.CloseBTDialogue();
                    }
                    else {
                        this.Msg = "Failed in adding new Beneficiary Type ";
                        this.errorSnackbarMsg = true;
                        this.CloseBTDialogue();
                    }               
                this.GetAllBeneficiaryTypes();
            },

            /// <summary>
            ///  RemoveBeneficiaryType :- Function used to delete beneficiary type as bulk/individual based on ProjectID,BeneficiaryTypeID
            /// </summary>
            /// <param name="isBulk"></param>
            /// <returns> </returns>
            async RemoveBeneficiaryType(isBulk) {
                if (!isBulk) {
                    this.removeList.push({ ProjectID: this.ProjectID, BeneficiaryTypeID: this.BenefTypeID, Name: this.BenefTypeName });
                }
                else {
                    for (var i = 0; i < this.vmBeneficiaryType.length; i++) {
                        this.removeList.push({ ProjectID: this.ProjectID, BeneficiaryTypeID: this.vmBeneficiaryType[i].BeneficiaryTypeID, Name: this.vmBeneficiaryType[i].Name });
                    }
                }
                var str = JSON.stringify(this.removeList);
                var data = new FormData();
                data.append("BeneficiaryTypeList", JSON.stringify(this.removeList));
                var result = await objPB.RemoveBeneficiaryType(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = "Beneficiary Type removed successfully..";
                        this.successSnackbarMsg = true;
                    }
                    else if (result.status == "Failed") {
                        this.Msg = "You cannot delete, Beneficiary Type(s) already used in Survey";
                        this.errorSnackbarMsg = true;
                    } else {
                        this.Msg = "You cannot delete Beneficiary Type(s)";
                        this.errorSnackbarMsg = true;
                    }
                }
                this.removeList = [];
                this.dlgRemoveBenefType = false;
                this.GetAllBeneficiaryTypes();
                this.vmBeneficiaryType = [];
                this.isBulk = false;
            },

            /// <summary>
            ///  CloseBTDialogue :- Function used to close beneficiary type dialog box
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseBTDialogue() {
                this.dlgCreateBeneficiaryType = false;
                this.CreateBeneficiaryTypeSteps = 1;
                this.txtBTypeName = "";
                this.txtBTypeDescription = "";
                this.SelectedTag = [];
                this.lstQstns = [];  
                this.lstOptions = [];
                this.lstQAnswerOptions = [];
                this.ClearQuestionDetails();
            }, 

            /// <summary>
            ///  UpdateBeneficiaryTypeDetails :- Function used to update beneficiary type details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateBeneficiaryTypeDetails() {
                var data = new FormData();
                data.append("BeneficiaryTypeID", this.BenefTypeID);
                data.append("Name", this.EbenefTypeName);
                data.append("Description", this.EbenefTypeDesc );
                data.append("BeneficiaryTypeTags", JSON.stringify(this.SelectedTag));

                var result = await objPB.UpdateBeneficiaryTypeDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Beneficiary Type details updated ... ";
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in adding new Beneficiary Type ";
                        this.errorSnackbarMsg = true;
                    }
                this.GetAllBeneficiaryTypes();
                this.EbenefTypeName = "";
                this.EbenefTypeDesc = "";
                this.SelectedTag = [];
            },

            /// <summary>
            ///  AddOptionItem :- Function used to add option value when question type as single choice/multi choice/dropdown 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddOptionItem() {
                this.SingleChoices.push({});
                this.lstOptions = this.SingleChoices.filter(x => x.OptionValue != "")
            },

            /// <summary>
            ///  DeleteOptionItem :- Function used to delete option items  
            /// </summary>
            /// <param name="SingleChoice"></param>
            /// <returns> </returns>
            DeleteOptionItem: function (SingleChoice) {
                this.SingleChoices.splice(this.SingleChoices.indexOf(SingleChoice), 1);
                this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);
            },

            /// <summary>
            ///  EditQstnOptionItem :- Function used to edit question option items
            /// </summary>
            /// <param name="QID"></param>
            /// <returns> </returns>
            EditQstnOptionItem(QID) {           
                var optList = this.lstBTQuestions.filter(x => x.QuestionID == QID)[0].BTQstnAnsOptionList;
                optList.push({});
                this.lstOptions = optList.filter(x => x.OptionValue != "");
                this.lstBTQuestions.filter(x => x.QuestionID == QID)[0].BTQstnAnsOptionList = optList;
            },

            /// <summary>
            ///  EditDeleteOptionItem :- Function used to delete edited question option items
            /// </summary>
            /// <param name="SingleChoice"></param>
            /// <param name="QID"></param>
            /// <returns> </returns>
            EditDeleteOptionItem: function (SingleChoice, QID) {
                var optList = this.lstBTQuestions.filter(x => x.QuestionID == QID)[0].BTQstnAnsOptionList;
                this.lstOptions = optList.filter(x => x.OptionValue != "");
                optList.splice(optList.indexOf(SingleChoice), 1);
                this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);
                this.lstBTQuestions.filter(x => x.QuestionID == QID)[0].BTQstnAnsOptionList = optList;
            },

            /// <summary>
            ///  RemoveBeneficiaryTypeQuestion :- Function used to delete beneficiary type questions
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async RemoveBeneficiaryTypeQuestion() {
                if (this.QuestionID == 0 || typeof this.QuestionID == "undefined") {
                    this.lstQstns.splice(this.TempQustId, 1);
                }
                else {
                    var data = new FormData();
                    data.append("QuestionID", this.QuestionID);
                    var result = await objPB.RemoveBeneficiaryTypeQuestion(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else
                        if (result.status == "success") {
                            this.Msg = "Beneficiary Type Questions removed Successfully ... ";
                            this.successSnackbarMsg = true;
                        }
                        else {
                            this.Msg = "Failed in removing BeneficiaryType Questions.. ";
                            this.errorSnackbarMsg = true;
                        }
                        this.dlgRemoveBenefType = false;
                        this.ClearQuestionDetails();
                        this.GetAllBeneficiaryTypes();
                        this.GetBeneficiaryTypeDetails(this.BenefTypeID); 
                }
            },

            /// <summary>
            ///  EditQuestion :- Function used to edit question in edit beneficiary type section
            /// </summary>
            /// <param name="QItem"></param>
            /// <param name="idx"></param>
            /// <returns> </returns>
            EditQuestion(QItem, idx) {  
               
                this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].IsEditQuestion = true;
                this.preQstn = this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].QuestionName;
                this.preAnsType = this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].AnswerType;
                
                this.vmEAnswerType[idx] = [{ AnswerTypeID: 0, AnswerType: "" }];

                this.vmEQuestion[idx] = this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].QuestionName;
                
                this.vmEAnswerType[idx].AnswerTypeID = this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].AnswerTypeID;
                this.vmEAnswerType[idx].AnswerType = this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].AnswerType;

                if (QItem.BTQstnValidationList.length == 0) {
                    this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].BTQstnValidationList = [{
                        QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                        MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                    }]
                }
                this.IsViewQuestion = false;
                this.IsUpdateQuestion = true;               
            },

            /// <summary>
            ///  CancelEditQuestion :- Function used to cancel edit question
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            async CancelEditQuestion(QItem) {   
                this.IsViewQuestion = true;
                this.IsUpdateQuestion = false;
                this.preCasOption = 0;
                this.lstBTQuestions.filter(x => x.QuestionID == QItem.QuestionID)[0].IsEditQuestion = false;
                this.GetBeneficiaryTypeDetails(QItem.BeneficiaryTypeID);
            },        

            /// <summary>
            ///  UpdateBeneficiaryTypeQuestion :- Function used to update beneficiary type question in edit beneficiary type dialog
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async UpdateBeneficiaryTypeQuestion(item) {
              
                var lstQstn = [];
                if ((item.AnswerType == 'Dropdown' || item.AnswerType == 'Multi-Choice' || item.AnswerType == 'Single Choice') && (this.lstOptions.length == 0))
                    this.lstOptions = item.BTQstnAnsOptionList;
                else if ((item.AnswerType == 'Cascading Dropdown') && (this.lstOptions.length == 0))
                    this.lstOptions = item.lstCascadingOptions;
                      
                var lstQOptions = this.lstOptions.filter(x => x.OptionValue !=null)
                if ((item.AnswerType == 'Dropdown' || item.AnswerType == 'Multi-Choice' || item.AnswerType == 'Single Choice' || item.AnswerType == 'Cascading Dropdown')
                        && (lstQOptions.length == 0)) {
                    this.Msg = "Please Add/Select  Options"
                    this.infoSnackbarMsg = true;
                    return;
                }
                               
                var IsValidationQuestion = item.BTQstnValidationList.length > 0 ? 1 : 0
                if (item.BTQstnValidationList.length == 1) {
                    if (item.BTQstnValidationList[0].ValidationCondition == "") {
                        IsValidationQuestion = 0;
                    }
                }
               
                var questionOrder = this.maxQuestionOrder > 0 ? this.maxQuestionOrder +1 : 0;

                if (item.QuestionOrder > 0) {
                    questionOrder = item.QuestionOrder;
                }

                lstQstn.push({
                    QuestionID: item.QuestionID, QuestionName: item.QuestionName, AnswerType: item.AnswerType, AnswerTypeID: item.AnswerTypeID,
                    Answer: "", QuestionHint: item.QuestionHint, IsMandatoryResponse: item.IsMandatoryResponse, IsUnique: item.IsUnique, IsSearchable: item.IsSearchable,
                    BeneficiaryTypeID: item.BeneficiaryTypeID, BeneficiaryID: 0, RegexExpression: item.RegexExpression, IsValidationQuestion: IsValidationQuestion,BTQstnAnsOptionList: lstQOptions, BTQstnAnsOptions: JSON.stringify(lstQOptions),
                    BTQstnValidation: JSON.stringify(item.BTQstnValidationList), QuestionOrder: questionOrder
                });
                
                var data = new FormData();
                data.append("BeneficiaryTypeID", this.BenefTypeID);
                data.append("BeneficiaryTypeQuestions", JSON.stringify(lstQstn));
                var result = await objPB.UpdateBeneficiaryTypeQuestions(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Beneficiary Type Questions updated ... ";
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in adding new Beneficiary Type ";
                        this.errorSnackbarMsg = true;
                    }
                              
                this.lstBTQuestions = [];  
                this.IsViewQuestion = true;
                this.IsUpdateQuestion = false;
                this.vmEAnswerType = [{ AnswerTypeID: 0, AnswerType: "" }];
                this.SingleChoices = [{ OptionValue: "" }];  
                this.lstOptions = [];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.GetBeneficiaryTypeDetails(item.BeneficiaryTypeID);
                this.preCasOption = 0;
            },

            /// <summary>
            ///  UpdateBeneficiaryTypeQuestionsAddNew :- Function used to update beneficiary type question by adding new question in edit beneficiary type dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateBeneficiaryTypeQuestionsAddNew() {
                
                var lstQstn = [];
                if (this.vmQstnE != "" && this.vmAnswerTypeE != [])
                    if (this.vmAnswerTypeE.AnswerType == 'Cascading Dropdown' || this.vmAnswerTypeE.AnswerType == 'Dropdown' ||
                        this.vmAnswerTypeE.AnswerType == 'Multi-Choice' || this.vmAnswerTypeE.AnswerType == 'Single Choice') {
                        if (this.lstOptions.filter(x => x.OptionValue != null).length == 0) {
                            this.Msg = "Please Add/Select  Options"
                            this.infoSnackbarMsg = true;
                            return;
                        }
                    }         

                var questionOrder = this.maxQuestionOrder > 0 ? this.maxQuestionOrder : 0;

                    lstQstn.push({
                        QuestionID: 0, QuestionName: this.vmQstnE, AnswerType: this.vmAnswerTypeE.AnswerType, AnswerTypeID: this.vmAnswerTypeE.AnswerTypeID,
                        Answer: "", QuestionHint: this.vmQstnHintE, IsMandatoryResponse: this.vmISMandatoryResponseE,
                        IsUnique: this.vmISUniqueE, IsSearchable: this.vmISSearchableE, BeneficiaryTypeID: this.BenefTypeID, BeneficiaryID: 0,
                        BTQstnAnsOptionList: this.lstOptions, BTQstnAnsOptions: JSON.stringify(this.lstOptions), BTQstnValidation: JSON.stringify(this.lstValidationItems), IsValidationQuestion: this.lstValidationItems.length > 0 ? 1 : 0, RegexExpression: this.vmRegexExpressionE, QuestionOrder: questionOrder+1
                    });
                var data = new FormData();
                data.append("BeneficiaryTypeID", this.BenefTypeID);
                data.append("BeneficiaryTypeQuestions", JSON.stringify(lstQstn));
                var result = await objPB.UpdateBeneficiaryTypeQuestions(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Beneficiary Type Questions updated ... ";
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in adding new Beneficiary Type ";
                        this.errorSnackbarMsg = true;
                    }
                this.lstBTQuestions = [];
                this.ClearQuestions();               
                this.GetBeneficiaryTypeDetails(this.BenefTypeID); 
                this.preCasOption = 0;
            },

            /// <summary>
            ///  ClearQuestions :- Function used to clear questions
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            ClearQuestions() {
                this.vmQstnE = "";
                this.vmAnswerTypeE = [];
                this.vmQstnHintE = "";
                this.vmISMandatoryResponseE = 0;
                this.vmISUniqueE = 0;
                this.vmISSearchableE = 0;
                this.lstOptions = [];
                this.SingleChoices = [{ OptionValue: "" }];
                this.IsAddNew = false;
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.preCasOption = 0;
            },

            /// <summary>
            ///  CloseEditBeneficiaryType :- Function used to close edit beneficiary type dialog box
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async CloseEditBeneficiaryType() {                
                this.SelectedTag = [];
                this.ClearQuestions();
                this.IsAddNew = false;
                this.Tab = 'tab-1';
                this.EditBeneficiaryDialog = false;
                this.GetAllBeneficiaryTypes();
            },

            //Tags

            /// <summary>
            ///  fnGetAllTags :- Function used to fetch all project tag list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objPB.GetAllProjectTags(this.InitiativeID);
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
            ///  fnEditTagDetails :- Function used to get tag details for edit
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

                var result = await objPB.UpdateTagDetails(data);
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
                var result = await objPB.CreateNewTag(data);
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
                    this.NewTagMenuedit = false;
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
            ///  AddValidationItem :- Function used to add validation item in the create beneficiary type section while Add/Edit question
            /// </summary>
            /// <param name="type"></param>
            /// <param name="index"></param>
            /// <returns> </returns>
            AddValidationItem(type,index) {
                if (type == "ADD") {
                    if (this.lstValidationItems.length > 0) {
                        this.lstValidationItems.push({
                            QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                            MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                        });
                    }
                }
                if (type == "Edit") {
                    if (this.lstQstns[index].lstQuestionValidations.length > 0) {
                        this.lstQstns[index].lstQuestionValidations.push({
                            QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                            MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                        });
                    }
                }
            },

            /// <summary>
            ///  AddValidationItemInEdit :- Function used to add validation item in the edit beneficiary type section
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            AddValidationItemInEdit(index) {
                if (this.lstBTQuestions[index].BTQstnValidationList.length > 0) {
                    this.lstBTQuestions[index].BTQstnValidationList.push({
                        QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                        MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                    });
                }
            },

            /// <summary>
            ///  ChangeLogicOperator :- Function used to change logic operator in validations tab (AND/OR)
            /// </summary>
            /// <param name="oprVal"></param>
            /// <param name="indx"></param>
            /// <returns> </returns>
            async ChangeLogicOperator(oprVal, indx) {
                if (oprVal == 0) // oprVal = 0-- AND ,, oprVal = 1-- OR
                    this.lstValidationItems[indx].LogicOperator = 1; // 1-- AND
                else if (oprVal == 1)
                    this.lstValidationItems[indx].LogicOperator = 2; // 2 -- OR
            },

            /// <summary>
            ///  GetQuestionValidation :- Function used to fetch question validation details, when click on the settings button
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            async GetQuestionValidation(index) {
                if (index >= 0) {
                    // Clearing and Pushing Blank data
                    this.lstValidationItems = [];
                    this.lstValidationItems.push({
                        QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                        MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                    });
                    var lstValidation = [];
                    for (var i = 0; i <= this.lstQstns.length - 1; i++) {
                        if (index == i) {
                            lstValidation = this.lstQstns[i].lstQuestionValidations;
                        }
                    }
                    this.lstValidationItems = lstValidation;
                }
            },

            /// <summary>
            ///  fnFinishEdit :- Function used to cancel the question edit option
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnFinishEdit() {
                this.addNewisEdit = false;
                this.vmAnswerTypeInEdit = [];
                this.SingleChoices = [{ OptionValue: "" }];
                this.lstOptions = [];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                for (var idx = 0; idx <= this.lstQstns.length - 1; idx++) {
                    this.lstQstns[idx].IsEditQstn = false;
                }
            },

            /// <summary>
            ///  fnUpdateQuestion :- Function used to update the added questions, when click on edit pencil button in the create beneficiary type section
            /// </summary>
            /// <param name="index"></param>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnUpdateQuestion(index, item) {
             
                this.lstQstns[index].Question = item.Question;
                this.lstQstns[index].AnswerType = this.vmAnswerTypeInEdit.AnswerType; //item.AnswerType;
                this.lstQstns[index].AnswerTypeID = this.vmAnswerTypeInEdit.AnswerTypeID;  //item.AnswerTypeID;
                this.lstQstns[index].QuestionHint = item.QuestionHint;
                this.lstQstns[index].IsMandatoryResponse = item.IsMandatoryResponse;
                this.lstQstns[index].IsUnique = item.IsUnique;
                this.lstQstns[index].IsSearchable = item.IsSearchable;
                if (this.lstOptions.length > 0) {
                    this.lstQstns[index].BTQstnAnsOptionList = this.lstOptions;
                } else {
                    this.lstQstns[index].BTQstnAnsOptionList = item.BTQstnAnsOptionList;
                }
                this.lstQstns[index].lstQuestionValidations = item.lstQuestionValidations;
                this.lstQstns[index].IsValidationQuestion = item.IsValidationQuestion;
                this.lstQstns[index].RegexExpression = item.RegexExpression;
                this.lstQstns[index].IsEditQstn = item.IsEditQstn;
                this.fnFinishEdit();
            },

            /// <summary>
            ///  fnCopyQuestion :- Function used to copy questions 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCopyQuestion() {
               
               if (this.CurrQstnIndex >= 0 && this.QuestionID == 0) {
                    var CopyQstn = [];
                    for (var i = 0; i <= this.lstQstns.length - 1; i++) {
                        if (this.CurrQstnIndex == i) {
                            CopyQstn = this.lstQstns[i];
                        }
                    }
                    if (CopyQstn !=null)
                    { 
                        var QuestionName = CopyQstn.Question;
                        var IsCopyExist = this.lstQstns.filter(x => x.Question .includes(CopyQstn.Question));
                        if (IsCopyExist.length > 0) {
                            QuestionName = QuestionName + "-Copy(" + IsCopyExist.length + ")";
                        }

                        var IsValidationQuestion = this.lstValidationItems.length > 0 ? 1 : 0;
                        if (this.lstValidationItems.length == 1) {
                            if (this.lstValidationItems[0].ValidationCondition == "") {
                                IsValidationQuestion = 0;
                            }
                        }
                        
                        var CurrentQuestionOrder = (this.lstQstns != null && this.lstQstns != undefined && this.lstQstns.length > 0) ? (this.lstQstns.length + 1) : 1;
                        this.lstQstns.push({
                            QuestionID: 0, Question: QuestionName, AnswerType: CopyQstn.AnswerType, AnswerTypeID: CopyQstn.AnswerTypeID,
                            QuestionHint: this.vmCopySettings == true ? CopyQstn.QuestionHint : '', IsMandatoryResponse: this.vmCopySettings == true ?CopyQstn.IsMandatoryResponse :0,
                            IsUnique: this.vmCopySettings == true ? CopyQstn.IsUnique : 0, IsSearchable: this.vmCopySettings == true ? CopyQstn.IsSearchable : 0, BTQstnAnsOptionList: CopyQstn.BTQstnAnsOptionList, lstQuestionValidations: this.vmCopyValidation == true ? CopyQstn.lstQuestionValidations : [], IsValidationQuestion: IsValidationQuestion, RegexExpression: CopyQstn.RegexExpression, IsEditQstn: false, QuestionOrder: CurrentQuestionOrder
                        });
                    }

                } else if (this.CurrQstnIndex >= 0 && this.QuestionID > 0){
                    // insert into database
                    var lstCopyQuestion = this.lstBTQuestions.filter(x => x.QuestionID == this.QuestionID);
                    var QuestionName = lstCopyQuestion[0].QuestionName;
                    var IsCopyExist = this.lstBTQuestions.filter(x => x.QuestionName.includes(QuestionName));
                    if (IsCopyExist.length > 0) {
                        QuestionName = QuestionName + "-Copy(" + IsCopyExist.length + ")";
                    }
                    lstCopyQuestion[0].QuestionName = QuestionName;
                    if (this.vmCopyValidation == false) {
                        lstCopyQuestion[0].BTQstnValidationList = [];
                        lstCopyQuestion[0].RegexExpression = "";
                        lstCopyQuestion[0].IsValidationQuestion = 0;
                    }
                    if (this.vmCopySettings == false) {
                        lstCopyQuestion[0].IsMandatoryResponse = false;
                        lstCopyQuestion[0].QuestionHint = "";
                        lstCopyQuestion[0].IsUnique = false;
                        lstCopyQuestion[0].IsSearchable = false;
                    }
                    var data = new FormData();
                    data.append("BeneficiaryTypeID", this.BenefTypeID);
                    data.append("BeneficiaryTypeQuestions", JSON.stringify(lstCopyQuestion));
                    var result = await objPB.CopyBeneficiaryQuestion(data);
                    if (result.status == true) {
                        await this.GetBeneficiaryTypeDetails(this.BenefTypeID)
                        this.Msg = "Question Copied Successfully";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed to Copy Question";
                        this.errorSnackbarMsg = true;
                    }
                }
                this.dlgCopyQuestion = false;
                this.CurrQstnIndex = -1;
                this.QuestionID = 0;
            },

            /// <summary>
            ///  RemoveValidationItem :- Function used to remove validation items in the beneficiary type create section (2 type - Add,Edit)
            ///  ItemIndex - selected index of lstQstn list.... 
            ///  valIndex - selected index of lstValidationItems
            /// </summary>
            /// <param name="ItemIndex"></param>
            /// <param name="valIndex"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            RemoveValidationItem(ItemIndex, valIndex, type) 
            {
                if (type == "ADD") {
                    if (valIndex >= 0) {
                        if (valIndex == 0) {
                            this.lstValidationItems.splice(valIndex, 1);
                            if (this.lstValidationItems.length == 0) {
                                this.lstValidationItems.push({
                                    QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                                    MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                                })
                            }
                        }
                        else {
                            this.lstValidationItems.splice(valIndex, 1);
                        }
                    }
                }
                if (type == "Edit")
                {
                    if (ItemIndex >= 0 && valIndex >= 0) {
                        if (valIndex == 0) {
                            this.lstQstns[ItemIndex].lstQuestionValidations.splice(valIndex, 1);
                            if (this.lstQstns[ItemIndex].lstQuestionValidations.length == 0) {
                                this.lstQstns[ItemIndex].lstQuestionValidations.push({
                                    QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                                    MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                                });
                            }
                        }
                        else {
                            this.lstQstns[ItemIndex].lstQuestionValidations.splice(valIndex, 1);
                        }
                    }
                }
            },

            /// <summary>
            ///  RemoveValidatinItemInEdit :- Function used to remove validation item in the edit beneficiary type section
            /// </summary>
            /// <param name="ItemIndex"></param>
            /// <param name="valIndex"></param>
            /// <returns> </returns>
            RemoveValidatinItemInEdit(ItemIndex, valIndex) {
                if (ItemIndex >= 0 && valIndex >= 0) {
                    if (valIndex == 0) {
                        this.lstBTQuestions[ItemIndex].BTQstnValidationList.splice(valIndex, 1);
                        if (this.lstBTQuestions[ItemIndex].BTQstnValidationList.length == 0) {
                            this.lstBTQuestions[ItemIndex].BTQstnValidationList.push({
                                QuestionID: 0, ValidationCondition: "", ConditionValue: "",
                                MinValue: 0, MaxValue: 0, LogicOperator: 0, QuestionName: '', AnswerTypeID: 0
                            });
                        }
                    }
                    else {
                        this.lstBTQuestions[ItemIndex].BTQstnValidationList.splice(valIndex, 1);
                    }
                }
            },
            async UpdateQstnOrder(QItem) {
               
                var lstQstnOrder = []
                for (var i = 0; i < QItem.length; i++)
                    lstQstnOrder.push({ QuestionID: QItem[i].QuestionID, QuestionOrder: (i + 1) })
                var data = new FormData();
                data.append("QstnList", JSON.stringify(lstQstnOrder));
                var result = await objPB.UpdateQuestionOrder(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Question Order Changed";
                        this.successSnackbarMsg = true;
                    }

            },
            async UpdateNewQstnOrder(QItem) {
               
                try {
                    if (QItem != null && QItem.length > 0) {
                        for (var i = 0; i < QItem.length; i++) {
                            this.lstQstns[i].QuestionOrder = i + 1;
                        }
                    }
                    this.Msg = "Question Order Changed";
                    this.successSnackbarMsg = true;
                } catch (err) {
                    this.$router.push({ name: "error" });
                }
            },
        },
    }
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