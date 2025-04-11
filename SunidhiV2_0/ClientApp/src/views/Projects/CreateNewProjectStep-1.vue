<template>
    <v-row class="mx-4">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-col sm="12" md="6">
            <h3 class="create-project-steps-title text-center">Define Project</h3>
            <v-row>
                <v-col cols="12" align="center" class="pt-9">
                    <v-carousel cycle height="200px"
                                hide-delimiter-background
                                show-arrows-on-hover>
                        <v-carousel-item>
                            <img src="../../images/Create-Project-Step1-Vector-1.svg" class="create-project-corousal-vector" />
                        </v-carousel-item>
                    </v-carousel>
                </v-col>
                <v-col cols="12">
                    <p class="text-center">
                        Please enter the basic details<br>to create a project.
                    </p>
                </v-col>
            </v-row>
        </v-col>
        <v-col sm="12" md="6" class="v-scrolling-div" style="height:55vh">
            <h6 class="heading-3">
                <v-avatar size="30" color="#EDF6FF" class="mr-2">
                    <v-icon color="secondary" size="15">mdi-file</v-icon>
                </v-avatar> Project Setup
            </h6>
            <v-row class="pt-3">
                <v-col md="12">
                    <v-text-field outlined dense label="Project Name" type="text" v-model="prjName" :rules="[(value) => !!value || 'Required field']">
                        <template v-slot:append>
                            <span v-if="prjName.length==0" class="error--text">*</span>
                        </template>
                    </v-text-field>
                </v-col>
                <v-col md="6" class="pb-0 pt-0">
                    <v-menu ref="mStartDate"
                            v-model="mStartDate"
                            :close-on-content-click="false"
                            :return-value.sync="date"
                            transition="scale-transition"
                            offset-y
                            min-width="290px">
                        <template v-slot:activator="{ on, attrs }">
                            <v-text-field hint="DD-MM-YYYY format" persistent-hint
                                          v-model="computedStartDate"
                                          label="Start Date"
                                          append-icon="mdi-calendar-month-outline"
                                          readonly outlined dense
                                          :rules="[(value) => !!value || 'Required field']"
                                          v-bind="attrs"
                                          v-on="on">
                                <template v-if="startDate == '' || startDate == null" v-slot:append="">
                                    <span class="mandatory-star">*</span>
                                </template>
                            </v-text-field>
                        </template>
                        <v-date-picker color="primary" v-model="startDate" no-title @input="mStartDate = false,IsValidDate()">
                        </v-date-picker>
                    </v-menu>
                </v-col>
                <v-col md="6" class="pb-0 pt-0">
                    <v-menu ref="mEndDate"
                            v-model="mEndDate"
                            :close-on-content-click="false"
                            :return-value.sync="date"
                            transition="scale-transition"
                            offset-y
                            min-width="290px">
                        <template v-slot:activator="{ on, attrs }">
                            <v-text-field hint="DD-MM-YYYY format" persistent-hint
                                          v-model="computedEndDate"
                                          label="End Date"
                                          append-icon="mdi-calendar-month-outline"
                                          readonly outlined dense
                                          :rules="[(value) => !!value || 'Required field']"
                                          v-bind="attrs"
                                          v-on="on">
                                <template v-if="endDate == '' || endDate == null" v-slot:append="">
                                    <span class="mandatory-star">*</span>
                                </template>
                            </v-text-field>
                        </template>
                        <v-date-picker color="primary" v-model="endDate" no-title @input="mEndDate = false,IsValidDate()" :min="startDate">
                        </v-date-picker>
                    </v-menu>
                </v-col>
                <v-col cols="12" class="pb-0">

                    <v-textarea rows="3" outlined dense label="Project Impact Statement" type="text" v-model="prjDescription">
                    </v-textarea>
                </v-col>
                
                <v-col cols="12" class="pt-0">
                    <!-- Select User -->
                    <span class="small-text mr-1">
                        <v-icon size="15">mdi-account</v-icon>Select Users <span class="mandatory-star" v-if="selectedUsers2.length == 0">*</span>
                    </span>

                    <v-tooltip left v-if="selectedUsers2.length">
                        <template v-slot:activator="{ on }">
                            <span v-on="on">
                                <v-avatar size="27" class="team-avatar" v-for="(item,key) in selectedUsers2.slice(0,2)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                </v-avatar>
                                <v-avatar v-if="selectedUsers2.length>=3" size="27" class="team-count">
                                    +{{selectedUsers2.length-2}}
                                </v-avatar>
                            </span>
                        </template>
                        <span v-if="selectedUsers2.length >= 1">{{selectedUsers2[0].Name}}</span>
                        <span v-if="selectedUsers2.length >= 2">,{{selectedUsers2[1].Name}}</span>
                    </v-tooltip>

                    <v-menu style="height:500px" absolute="" v-model="SelectUserListPopUp" top="" :offset-y="offset" :close-on-click="false" :close-on-content-click="false">
                                        <template v-slot:activator="{ on }">
                                            <v-btn id="AutoTest_SU37" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" >
                                                <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                            </v-btn>
                                        </template>
                                        <!--Select User pop up-->
                                        <v-card v-if="!InviteUserDialog">
                                            <v-row no-gutters="" style="height:290px">
                                                <v-col cols="12" md="12" sm="12" xs="12">
                                                    <v-data-table :headers="selectUserTableheaders"
                                                                  :items="lstUserList"
                                                                  show-select
                                                                  fixed-header
                                                                  height="32vh"
                                                                  item-key="ID"
                                                                  sort-by="Name"
                                                                  v-model="selectedUsers1">
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
                                                                        {{ item.Name }}
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                            </v-list>
                                                        </template>
                                                        <template v-slot:item.UserRole="{item}">
                                                            <div class="pa-0 text-limit" style="width:140px">
                                                                {{item.Role == '' || item.Role == undefined ? 'N/A':item.Role }}
                                                            </div>
                                                        </template>
                                                    </v-data-table>
                                                </v-col>
                                            </v-row>
                                            <v-divider></v-divider>
                                            <v-card-actions>
                                                <v-row no-gutters="">
                                                    <v-col md="12" class="py-0" align="right">
                                                        <v-btn depressed class="btn-122x36" height="37" color="primary" @click.native="InviteUserDialog = true">Invite <span v-if="InviteUserListInput.length">({{InviteUserListInput.length}})</span> </v-btn>
                                                        <v-btn depressed class="btn-122x36 mr-2" height="37" color="success" @click="AssignUsersToProject()">Save</v-btn>  <!--v-bind:disabled="selectedUsers1.length == 0"-->
                                                        <v-btn depressed class="btn-122x36" height="37" v-on:click="CloseSelectUserPopUp()">Cancel</v-btn>
                                                    </v-col>
                                                </v-row>
                                            </v-card-actions>
                                        </v-card>
                                        <!--_______________________________________-->
                    </v-menu>
                </v-col>
            </v-row>

            <!-- Tag Section modified-->
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
                                                        <v-btn id="AutoTest_IP79"  color="success-btn" @click="searchTag()" dark class="text-limit" max-width="200">
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
        <v-col cols="12" align="right">
            <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="fnCreateProject(),NewTagMenu = false" :disabled="(selectedUsers1.length ==0 || this.prjName.length ==0) || (startDate =='' || endDate =='')"></Secondary-Button>
            <v-btn depressed class="btn-122x36" height="37" v-on:click="fnCloseNewProjectSetup()">Cancel</v-btn>
        </v-col>
        

        <!-- Invite User -->
        <v-dialog persistent v-model="InviteUserDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                    </v-avatar>Invite User
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP807" text @click="CloseInviteUserDialog()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="py-0">
                    <v-container fluid class="pt-0">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-stepper v-model="NewUserRegSteps" class="elevation-0">
                                    <v-stepper-header class="elevation-0 center-block" style="width:50%;">
                                        <v-stepper-step :complete="NewUserRegSteps > 1" step="1" color="inherit">Add Email</v-stepper-step>
                                        <v-divider></v-divider>
                                        <v-stepper-step :complete="NewUserRegSteps > 2" step="2" color="inherit">Assign Role</v-stepper-step>
                                    </v-stepper-header>

                                    <v-stepper-items>
                                        <v-stepper-content step="1" class="pa-0">
                                            <v-container fluid class="pt-0">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <h4 class="text-center heading-3">Step 1: Inivite people by enter their email addresses below</h4>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                        <v-row :justify="justifyCenter">
                                                            <v-col md="12">
                                                                <v-text-field id="txtEmail"
                                                                              outlined
                                                                              dense
                                                                              label="Enter Email addresses"
                                                                              type="NewEmail"
                                                                              v-model="NewEmail"
                                                                              :error-messages="emailErrors"
                                                                              required
                                                                              :rules="emailRules"
                                                                              :clearable="clearable"
                                                                              @change="AddToList()"></v-text-field>
                                                                <small class="error--text" v-model="emailIdError" v-if="emailIdError=='' && NewEmail.length > 0">Please enter a valid e-mail</small>
                                                            </v-col>
                                                            <v-col md="12">
                                                                <div class="chip-container mb-2">
                                                                    <v-chip-group multiple
                                                                                  column
                                                                                  active-class="primary--text">
                                                                        <v-chip id="AutoTest_IP808" close v-for="item in InviteUserListInput" :key="item.EmailId"
                                                                                v-model="chipItem"
                                                                                @click:close="removemailfromchip(item)">
                                                                            <v-icon left color="#707070">mdi-email</v-icon>
                                                                            {{ item.EmailId }}
                                                                        </v-chip>

                                                                    </v-chip-group>
                                                                </div>
                                                            </v-col>

                                                        </v-row>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                        <Secondary-Button id="AutoTest_IP809" class="btn-122x36 mr-2" title="Next" @click.native="NewUserRegSteps = 2" v-bind:disabled="InviteUserListInput.length ==0"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP810" class="btn-122x36" title="Cancel" @click.native="CloseInviteUserDialog()"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-stepper-content>

                                        <v-stepper-content step="2" class="pa-0">
                                            <v-container fluid class="pt-0">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0 mb-2">
                                                        <h4 class="text-center heading-3">Step 2: Assign user role and set access type for screens</h4>
                                                    </v-col>
                                                    <v-col cols="12" md="12" class="text-center">
                                                        <v-text-field outlined
                                                                      dense
                                                                      label="Search"
                                                                      type="Text"
                                                                      prepend-inner-icon="mdi-magnify"
                                                                      v-model="SearchRoleName"></v-text-field>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                        <v-chip color="#FFE9E9" label dark v-if="roleErrorCount >0">
                                                            <v-icon size="30" color="red" height="100">mdi-alert-circle</v-icon>
                                                            <span style="color:#F55D5D"> {{ roleErrorCount }} User Roles have not been set to proceed</span>
                                                        </v-chip>
                                                    </v-col>

                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                        <v-data-table :headers="AssignRoleTableHeaders"
                                                                      :items="InviteUserListInput"
                                                                      :single-expand="singleExpand"
                                                                      :expanded.sync="expanded"
                                                                      item-key="EmailId"
                                                                      :search="SearchRoleName"
                                                                      v-model="InviteUsersInput"
                                                                      show-expand
                                                                      class="elevation-0"
                                                                      fixed-header
                                                                      height="20vh">

                                                            <template v-slot:item.UserRole="{item}">

                                                                <v-select :items="userRoleTableItems"
                                                                          item-text="userRole"
                                                                          item-value="userRoleID"
                                                                          label="Select User Role"
                                                                          v-model="item.UserRole"
                                                                          outlined
                                                                          solo
                                                                          dense
                                                                          flat
                                                                          return-object
                                                                          class="mt-3"
                                                                          required
                                                                          @change="ChangeNewRole(item,1)"></v-select>

                                                            </template>

                                                            <template v-slot:expanded-item="{ headers }">
                                                                <td :colspan="headers.length">
                                                                    <v-data-table :headers="ScreenNameTableheaders"
                                                                                  :items="userScreenNameTableItems"
                                                                                  item-key="userScreenName"
                                                                                  class="elevation-0">
                                                                        <template v-slot:item.Preview>
                                                                            <router-link to="">Proj.jpg</router-link>
                                                                        </template>
                                                                    </v-data-table>
                                                                </td>
                                                            </template>

                                                            <template v-slot:item.RowItemCancel>
                                                                <v-btn id="AutoTest_IP811" text @click="removeRole(InviteUserListInput.indexOf(item))">
                                                                    <v-icon size="15">mdi-close</v-icon>
                                                                </v-btn>
                                                            </template>
                                                            <template v-slot:item.Message="{item}">
                                                                <v-col style="color:red;font-weight: bold;font-size:small;padding-left:0px;padding-right:0px">
                                                                    {{item.Message}}
                                                                </v-col>
                                                            </template>
                                                        </v-data-table>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" xs="12" sm="8" md="8" class="pt-4 pl-4">
                                                                <p>
                                                                    Invited users will be notified that they have added under respective user roles.
                                                                </p>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="4" md="4" class="pt-2 pr-4">
                                                                <!-- <v-checkbox color="success-check-box" label="Notify user" input-value="true" v-model="chkNotify" class="float-right"></v-checkbox> -->
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                        <Secondary-Button id="AutoTest_IP812" class="btn-122x36 mr-2" title="Next" @click.native="/*SendInvite(1)*/NewUserRegSteps = 3" v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP813" class="btn-122x36" title="Previous" @click.native="NewUserRegSteps = 1"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-stepper-content>

                                        <v-stepper-content step="3" class="pa-0">
                                            <v-container fluid class="pt-0">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                        <v-avatar size="150" tile>
                                                            <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                        </v-avatar>
                                                    </v-col>
                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                        <p class="text-medium">
                                                            Your Invites will be sent once the project is finished creating
                                                        </p>
                                                    </v-col>
                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                        <Secondary-Button id="AutoTest_IP814" class="btn-122x36" title="Done" @click.native="InviteUserDialog=false, NewUserRegSteps=1"></Secondary-Button>
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
    import SecondaryButton from '@/components/secondary-button.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '../../utils.js'
  
    var objCreateProject;
    async function importCreatescript() {
        return Promise.all([
            import("../../BL/CreateProject.js").then(mod => {
                objCreateProject = new mod.CreateProject();
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
    var objManageProject;
    async function importscriptManageProject() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objUser;
    async function importUserscript() {
        return Promise.all([
            import("../../BL/User.js").then(mod => {
                objUser = new mod.User();
            })
        ]);
    };
  export default{
     
  props:[
    'CreateNewProjectStepsProp', 'CreateNewProjectDialogProp'
  ],
  
    data : function(){
      return{
       date: new Date().toISOString().substr(0, 10),

       
          //=====CreateProject ===========
          justifyCenter: 'center',
          ProjectId: 0,
          GroupId:0,
          prjName: "",
          prjDescription: "", 
          emailIdError: 1,
          item: "",
          //startDate: new Date().toISOString().substr(0, 10),
          startDate: "",
          mStartDate: false,
          //endDate: new Date().toISOString().substr(0, 10),
          endDate: "",
          mEndDate: false,
          selectUserTableheaders: [
              { text: "Name", align: "left", value: "Name" },
              { text: "User Role", value: "UserRole" },
          ],
          lstUserList: [],
          lstUserListData: [],
          SelectedUserList: '',
          SelectedUsers : [],
          selectedUsers1: [],
          selectedUsers2: [],
          SelectUserListPopUp: false,
          rules: {
              required: value => !!value || 'Required.',
          },
          selectusercheckbox: [],
          PreLoader: false,
          // Tag Menu
          AllTagListSteps: 0,
          //NewTagListSteps: 0,
          TagHeaderName: "Add Tag",
          NewTagMenu: false,
          AllTagList: [],
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
          offset: true,
          closeonClick: false,
          closeOnContentClick: false,
          filterAllTagList: [],
          SearchTagDetails: "",
          Msg: "",
          

          //===========Invite Users==============
          //SelectUserDialog: false,
          SearchRoleName: "",
          roleErrorCount: 0,
          SelectUserTableheaders: [
              { text: "UserID", align: ' d-none', value: "UserID" },
              { text: "Name", align: "left", value: "Name" },
              { text: "User Role", value: "userRole" },
          ],
          SelectUserTableItems: [],
          data: [['EmailId Here', 'UserRole here']],
          chkEmails: false,
          expanded: [],
          singleExpand: false,
          InviteUserDialog: false,
          NewEmail: "",
          reg: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/,

          emailRules: [
              v => /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/.test(v),
              // || 'Please enter valid E-mail',
          ],
          chipItem: true,
          InviteUserListInput: [],
          InviteUsersInput: [],
          userListInput: [],
          emailErrors: [],
          userListItems: [],

          clearable: false,
          chkNotify: true,
          //chkNotify1: true,
          ImportInviteUserSteps: 0,
          IsValidData: false,
          sentInvite: null,
          //fileInput: null,
          csvfile: {
              name: "",
              size: 0,
              namedisplay: ""
          },
          csvfiledata: [],
          ProjectUserTeamTableitems: [],
          AssignRoleTableHeaders: [
              { text: 'Email', value: 'EmailId' },
              { text: 'User Role', value: 'UserRole' },
              { text: 'Screen Info', value: 'data-table-expand' },
              { text: '', align: "left", color: "red", value: "Message" },
              { text: '', value: 'RowItemCancel' }

          ],
          ScreenNameTableheaders: [
              { text: "Screen Name", align: "left", value: "userScreenName" },
              { text: "Access Type", value: "userAccessType" },
              { text: "Preview", value: "Preview" }
          ],
          userScreenNameTableItems: [],
          NewUserRegSteps: 0,
          
          Msg: "",
          successSnackbarMsg: false,
          errorSnackbarMsg: false,
          infoSnackbarMsg: false,
          userRoleTableItems: [],
          userimage: "",
          SelectedTagID :0,
          IsSearchTagExist: false,

      }
        },
          mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await importCreatescript();
            await importUserscript();
            await importscriptManageProject();
            await this.fnGetUsersListByIntitiativeID();
            await this.fnGetAllTags();
            await this.GetUserRoleDetails();
              

        },
        components:{
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
        },
        computed: {
            computedStartDate() {
                return objUtils.common.formatDate(this.startDate)
            },
            computedEndDate() {
                return objUtils.common.formatDate(this.endDate)
            },
            cInviteesList() {
                let saves = this.selectedUsers2.map(x => {
                    return {
                        emailId: x.EmailId,
                        UserRole: JSON.stringify({
                            userRoleID: x.RoleID,
                            userRole: x.Role
                        }),
                        WorkspaceID: this.WorkspaceID,
                        TypeOfUser: 'save',
                        UserID: x.ID
                    }
                })

                let invites = this.InviteUserListInput.map(i => {
                    return {
                        emailId: i.EmailId,
                        UserRole : JSON.stringify(i.UserRole),
                        WorkspaceID: this.WorkspaceID,
                        TypeOfUser : 'invite'
                    }
                })

                return [...saves, ...invites]
            }
        },
     
        methods: {

            /// <summary>
            ///  commonGetColor :- Function used to get color
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },
            
          /// <summary>
          ///  GoToCreateNewProjectStepsTwoFn :- Function used to go to new project step two (next step)
          /// </summary>
          /// <param name=""></param>
          /// <returns> </returns>
          GoToCreateNewProjectStepsTwoFn(){
            this.CreateNewProjectStepsProp=2;
            this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
          },
          
          /// <summary>
          ///  CreateNewProjectDialogHideFn :- Function used to hide new project creation dialog box
          /// </summary>
          /// <param name=""></param>
          /// <returns> </returns>
          CreateNewProjectDialogHideFn(){
            this.CreateNewProjectDialogProp=false;
            this.$emit('CreateNewProjectDialogPropChange', this.CreateNewProjectDialogProp)
          },
        
            //===Step: 1===========================================

            /// <summary>
            ///  IsValidDate :- Function used to check project start date and project end date is valid  
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

                if (Date.parse(this.endDate) === Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End Date should be different";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                
            },

            /// <summary>
            ///  fnGetUsersListByIntitiativeID :- Function used to get user list by IntitiativeID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersListByIntitiativeID() {
                if (this.$route.name == 'MyprojectsDraft') {
                    this.InitiativeID = 0;
                }
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objCreateProject.GetUserListInWorkspace(data);
                
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstUserList = result.data;
                }
            },

            /// <summary>
            ///  CloseSelectUserPopUp :- Function used to cancel select user pop up 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseSelectUserPopUp() {
                this.SelectUserListPopUp = false;
                //this.selectedUsers2 = [];
            },

            /// <summary>
            ///  AssignUsersToProject :- Function used to assign users to project
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AssignUsersToProject() {
                this.SelectUserListPopUp = false;
                this.selectedUsers2 = this.selectedUsers1;
                
            },

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
                var result = await objProject.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Search Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags();
                        this.UpdatingTagItem = "";
                    }
                    else {
                        this.Msg = "Tag already exists";
                        this.infoSnackbarMsg = true;
                    }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create New Tag when there is no tags are based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("WorkspaceID", this.WorkspaceID)
                data.append("InitiativeId", this.InitiativeID);
                var result = await objManageProject.CreateNewTag(data);
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
            ///  fnRemoveFromAllTagList :- Function used to delete tag from all tag list in edit mode
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnRemoveFromAllTagList() {
                this.AllTagList.splice(this.AllTagList.indexOf(this.SelectedTagID), 1);
                this.AllTagList = [...this.AllTagList];
                if (this.AllTagList == 0) {
                    this.AllTagListSteps = 1;
                    this.TagHeaderName = 'Add New Tag';
                }
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
                        this.SelectedTagID = 0,
                        this.Msg = "Tag deleted successfully";
                        this.successSnackbarMsg = true;
                        this.fnGetAllTags();

                    }
                    else {
                        this.Msg = "Tag already used by another project";
                        this.infoSnackbarMsg = true;
                    }
            },
            /*async fnRemoveFromAllTagList(index) {
                this.filterAllTagList.splice(this.filterAllTagList.indexOf(index), 1);
                this.filterAllTagList = [...this.filterAllTagList];
                if (this.filterAllTagList == 0) {
                    this.AllTagListSteps = 1;
                    this.TagHeaderName = 'Add New Tag';
                }

                var data = new FormData();
                data.append("TagName", index.TagName);
                data.append("TagColor", index.color);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.fnRemoveFromAllTagList(data);
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
            },*/

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

            /// <summary>
            ///  fnCreateProject :- Function used to create a new project
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateProject() {
                this.PreLoader = true;
                if (this.ProjectId == 0) {
                    var i = 0;
                    var TagIDList = [];

                    if (this.prjName == "") {
                        this.Msg = "Project Name is required";
                        this.infoSnackbarMsg = true;
                        return;
                    }

                    for (i = 0; i < this.SelectedTag.length; i++) {
                        TagIDList[i] = this.SelectedTag[i].TagID;
                    }
                    var data = new FormData();
                    data.append("ProjectName", this.prjName);
                    data.append("Description", this.prjDescription);
                    data.append("StartDate", this.startDate);
                    data.append("EndDate", this.endDate);
                    data.append("ProjectTags", JSON.stringify(TagIDList));
                    data.append("InitiativeId", this.InitiativeID);
                    data.append("WorkspaceID", this.WorkspaceID);
                    data.append("UserList", JSON.stringify(this.selectedUsers1)); //JSON.stringify(this.lstUserListData));
                    data.append("AssignedUserListDuringProjectCreation", JSON.stringify(this.cInviteesList))
                    
                    var result = await objCreateProject.CreateProjectAsync(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.status == "success") {
                            this.ProjectId = result.ID
                            window.ProjectId = this.ProjectId;
                            window.ProjectStartDate = result.PrjtStartDate;
                            window.ProjectEndDate = result.PrjtEndDate;
                            sessionStorage.setItem('ProjectId', this.ProjectId);

                            this.GroupId = result.GroupID
                            window.GroupId = this.GroupId;
                            sessionStorage.setItem('GroupId', this.GroupId);
                            // including both Project Start date and end data
                            sessionStorage.setItem('ProjectStartDate', result.PrjtStartDate )
                            sessionStorage.setItem('ProjectEndDate', result.PrjtEndDate)
                            
                            this.GoToCreateNewProjectStepsTwoFn();
                        }
                        else {
                            this.Msg = "Project Name already Exists";
                            this.infoSnackbarMsg = true;
                            this.PreLoader = false;
                            return;
                        }

                    }
                }
                else {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectId);
                    data.append("ProjectName", this.prjName);
                    data.append("Description", this.prjDescription);
                    data.append("StartDate", this.startDate);
                    data.append("EndDate", this.endDate);
                    data.append("ProjectTags", JSON.stringify(this.SelectedTag));
                    data.append("UserList", JSON.stringify(this.selectedUsers1));
                    data.append("GroupId", this.GroupId);

                    var result = await objCreateProject.UpdateEditProjectDetails(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        this.GoToCreateNewProjectStepsTwoFn();
                    } 
                }
                this.PreLoader = false;
            },

            /// <summary>
            ///  fnCloseNewProjectSetup :- Function used to close project set up
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCloseNewProjectSetup() {
                this.CreateNewProjectDialogHideFn();
                this.$router.go();
            },

            //RemoveUsers(item) {
            //    this.SelectedUserList.splice(this.SelectedUserList.indexOf(item.ID), 1);
            //    this.SelectedUserList = [...this.SelectedUserList];
            //    this.selectusercheckbox[item.ID] = false;
            //},
        
            /// <summary>
            ///  CloseInviteUserDialog :- Function used to close invite user dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseInviteUserDialog() {
                this.InviteUserListInput = [];
                this.NewEmail = "";
                this.NewUserRegSteps = 1;
                this.userScreenNameTableItems = [];
                this.InviteUserDialog = false;
                this.roleErrorCount = 0;
            },

            /// <summary>
            ///  IsValidEmail :- Function used to check email is valid
            /// </summary>
            /// <param name="emailField"></param>
            /// <returns> true / false </returns>
            IsValidEmail(emailField) {
                var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                var reg1 = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4}\s)$/;

                if (reg.test(emailField) == false && reg1.test(emailField) == false) {
                    this.emailIdError = '';
                    return false;
                }

                this.emailIdError = 1;
                return true;
            },

            /// <summary>
            ///  AddToList :- Function used to add user email to list while inviting user
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddToList() {
                if (this.IsValidEmail(this.NewEmail)) {
                    var usrEmail = this.lstUserList.filter(x => x.EmailId == this.NewEmail);

                    if ((usrEmail.length == 0) || (usrEmail.length > 0 && usrEmail[0].IsDeleted == 1)) {
                        if ((this.NewEmail == "") ? false : (this.reg.test(this.NewEmail)) ? true : false) {

                            var isExist = this.InviteUserListInput.filter(x => x.EmailId == this.NewEmail);

                            if (isExist.length == 0) {
                                this.InviteUserListInput.push({ EmailId: this.NewEmail, Message: "Select User Role" });
                                this.roleErrorCount += 1;
                                this.chkEmails = true;
                            }
                            else {
                                this.Msg = "EmailId already added.";
                                this.infoSnackbarMsg = true;
                            }
                        }
                    }
                    else if (usrEmail.length > 0) {
                        if (usrEmail[0].IsDeleted == 0) {
                            if (usrEmail[0].IsRegisteredUser == 0)
                                this.Msg = "The emailId already exists, But not a registered user.";
                            else
                                this.Msg = "The emailId already exists.";
                            this.infoSnackbarMsg = true;
                        }
                    }
                    NewEmail: undefined;
                    this.NewEmail = "";
                    this.clearable = true;
                }
            },

            /// <summary>
            ///  removemailfromchip :- Function used to remove email from chip
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async removemailfromchip(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
                this.roleErrorCount -= 1;
            },

            /// <summary>
            ///  removeRole :- Function used to remove user role
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            removeRole(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
                this.roleErrorCount -= 1;
            },

            /// <summary>
            ///  GetUserRoleDetails :- Function used to fetch user role details 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUserRoleDetails() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objUser.GetUserRoleDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.userRoleTableItems = result.data;
                    }
            },

            /// <summary>
            ///  ChangeNewRole :- Function used to change user role 
            /// </summary>
            /// <param name="objRole"></param>
            /// <param name="InvOrImp"></param>
            /// <returns> </returns>
            async ChangeNewRole(objRole, InvOrImp) {
                if (objRole.Message != null) {
                    objRole.Message = null;
                    this.roleErrorCount -= 1;
                }

                if (InvOrImp == 1)
                    this.GetUserScreenDetails(objRole.UserRole.userRoleID);
            },

            /// <summary>
            ///  GetUserScreenDetails :- Function used to fetch user screen details based on RoleId
            /// </summary>
            /// <param name="RoleId"></param>
            /// <returns> </returns>
            async GetUserScreenDetails(RoleId) {
                var data = new FormData();
                data.append("RoleID", RoleId);
                var result = await objUser.GetUserScreenDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.userScreenNameTableItems = result.data;
                    }
            },

            /// <summary>
            ///  IsValidInput :- Function used to check invite/import user validation 
            /// invited/imported user(s) should have role
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            async IsValidInput(Type) {
                var errorCount = 0;

                switch (Type) {
                    case 1:
                        for (var i = 0; i < this.InviteUserListInput.length; i++) {
                            var filterrolename = this.InviteUserListInput[i].UserRole["userRole"];

                            if (this.InviteUserListInput[i].UserRole == "" || filterrolename.length == 0) {
                                errorCount += 1;
                            }
                            this.roleErrorCount = errorCount;
                        }
                        if (this.roleErrorCount == 0 || this.roleErrorCount < 0) {
                            this.IsValidData = true;
                        }
                        break;
                    case 2:
                        for (var i = 0; i < this.csvfiledata.length; i++) {
                            var filterrolename = this.csvfiledata[i].UserRole["userRole"];

                            if (this.csvfiledata[i].UserRole == "" || filterrolename.length == 0) {
                                errorCount += 1;
                                this.csvfiledata[i].Message = "Select User Role";
                            }
                            else {
                                this.csvfiledata[i].Message = "";
                            }
                            this.roleErrorCount = errorCount;
                        }
                        if (this.roleErrorCount == 0) {
                            this.IsValidData = true;
                        }
                        break;
                }
            },

            /// <summary>
            ///  SendInvite :- Function used to send/create invite based on type (Invite user list input/CSV file data)
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            async SendInvite(Type) {
                this.IsValidInput(Type);
                if (this.IsValidData) {
                    var data = new FormData();
                    if (Type == 1)
                        data.append("userInviteList", JSON.stringify(this.InviteUserListInput));

                    else if (Type == 2) {
                        data.append("userInviteList", JSON.stringify(this.csvfiledata));
                        this.ImportInviteUserSteps = 3;
                    }
                    data.append("WorkspaceId", this.WorkspaceID);
                    var result = await objUser.SendInvite(data);

                    
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.status == "success") {
                            this.sentInvite = true;
                            this.fnGetUsersListByIntitiativeID();
                        }
                        else {
                            this.sentInvite = false;
                            this.retMsg = result.status;
                        }

                    if (result.status == "success") {

                       //this.GetUsersListNotInTeam();
                    }
                   //this.fnGetTeamUsersList();


                }
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

        },
  } 
</script>