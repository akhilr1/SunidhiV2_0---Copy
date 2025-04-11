<template>
    <v-container fluid class="pt-0">
        <v-row no-gutters>
            <v-col cols="12" md="12">
                <v-row no-gutters>
                    <v-col>
                        <h6 class="page-head pl-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                <v-icon size="15" color="primary">mdi-account-group-outline</v-icon>
                            </v-avatar>
                            <router-link :to="{ name: 'usersgroup' , query: { WsID: WorkspaceID, Access : RWAccess}}" class="page-head">User Groups</router-link>
                            <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon>
                            {{ this.groupInfo.GroupName }}
                        </h6>
                    </v-col>
                    <v-col v-if="this.SubGroupTableItems.length==0">
                        <v-btn id="AutoTest_U1" depressed color="success-btn" dark class="pull-right" @click.stop="AddSubGroupDialog = true;" v-bind:disabled="!screenAccess">
                            Add Sub Group
                        </v-btn>
                    </v-col>
                    <v-dialog v-model="AddSubGroupDialog" width="800">
                        <v-card>
                            <v-card-title primary-title class="page-head pb-0">
                                <v-avatar color="white" size="30" class="elevation-1 mr-2">
                                    <v-icon size="25" color="primary">mdi-account-group-outline</v-icon>
                                </v-avatar>
                                New User Sub Group
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_U2" text="" @click="fnAddSubGroupCancelClick()">
                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                </v-btn>
                            </v-card-title>
                            <v-card-text class="pt-0">
                                <v-container fluid class="py-0">
                                    <v-row no-gutters>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <v-stepper v-model="AddSubGroupRegSteps" class="elevation-0">
                                                <v-row justify="center" no-gutters>
                                                    <v-col md="6">
                                                        <v-stepper-header style="height:55px" class="elevation-0">
                                                            <v-stepper-step :complete="AddSubGroupRegSteps > 1" step="1" color="inherit">Basic Details</v-stepper-step>
                                                            <v-divider></v-divider>
                                                            <v-stepper-step :complete="AddSubGroupRegSteps > 2" step="2" color="inherit">Add Members</v-stepper-step>
                                                        </v-stepper-header>
                                                    </v-col>
                                                </v-row>
                                                <v-row>
                                                    <v-col md="12">
                                                        <v-stepper-items>
                                                            <v-stepper-content step="1" class="pt-0">
                                                                <v-container fluid="" class="pt-0">
                                                                    <v-row no-gutters="">
                                                                        <v-col cols="12" md="12" class="pt-0">
                                                                            <h4 class="text-center page-sub-head">Step1 : Please enter the basic details of the new user sub group</h4>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="text-center">
                                                                            <v-avatar size="75">
                                                                                <v-avatar size="75" class="elevation-1 mt-2" light="" v-bind:color="RandomColor">
                                                                                    <v-img v-bind:src="profilesubgroupImage" height="75" width="75" v-if="profilesubgroupImage.length>0"></v-img>
                                                                                    <v-icon color="grey" size="60" v-else="">mdi-account-group</v-icon>
                                                                                    <div class="dp-edit-btn" style="top:30px">
                                                                                        <input class="upload" type="file" id="filesubgroup" hide-details="" accept="image/*" @change="previewImageSubgroup" />
                                                                                    </div>
                                                                                </v-avatar>
                                                                            </v-avatar>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="text-center">
                                                                            <h6 class="text-center">Parent Group : {{ this.groupInfo.GroupName }}</h6>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                            <v-row :justify="justifyCenter">
                                                                                <v-col md="6">
                                                                                    <v-text-field outlined="" dense="" label="User Sub Group Name" type="text" v-model="AddSubGroupInput.GroupName" :rules="alphanumericRules">
                                                                                    </v-text-field>
                                                                                    <v-textarea rows="3" outlined="" dense="" label="Description" type="text" v-model="AddSubGroupInput.GroupDescription">
                                                                                    </v-textarea>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                            <Secondary-Button id="AutoTest_U3" class="btn-122x36 mr-4" title="Next" @click.native="AddSubGroupRegSteps = 2" v-bind:disabled="AddSubGroupInput.GroupName.length==0"></Secondary-Button>
                                                                            <Default-Button-Outlined id="AutoTest_U4" class="btn-122x36" title="Cancel" @click.native="fnAddSubGroupCancelClick()"></Default-Button-Outlined>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-container>
                                                            </v-stepper-content>

                                                            <v-stepper-content step="2" class="py-0">
                                                                <v-container fluid="" class="py-0">
                                                                    <v-row no-gutters="">
                                                                        <v-col cols="12" md="12" class="pt-0 pb-2">
                                                                            <h4 class="text-center page-sub-head">Step 2: Add members to your user sub group from the user list</h4>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="pt-0">
                                                                            <v-row :justify="justifyCenter" no-gutters="">
                                                                                <v-col cols="12" md="auto" lg="11" class="px-4 mb-2">
                                                                                    <v-text-field hide-details v-model="searchuser" outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify">
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-col>
                                                                        <v-col v-if="selectedUsers.length>0" md="12">
                                                                            <div class="chip-container mb-2">
                                                                                <v-chip-group multiple=""
                                                                                              column=""
                                                                                              active-class="primary--text">
                                                                                    <v-chip id="AutoTest_U5" close="" v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                                            @click:close="$delete(selectedUsers, key)">
                                                                                        <v-icon left="" color="#707070">mdi-account</v-icon>
                                                                                        {{ item.Name }}
                                                                                    </v-chip>
                                                                                </v-chip-group>
                                                                            </div>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                            <!--User List with checkbox add here-->
                                                                            <v-data-table :headers="userTableHeaders"
                                                                                          :items="subUserTableItems"
                                                                                          :search="searchuser"
                                                                                          show-select=""
                                                                                          item-key="ID"
                                                                                          class=""
                                                                                          v-model="selectedUsers"
                                                                                          fixed-header=""
                                                                                          height="22vh">
                                                                                <template v-slot:item.userActions="">
                                                                                    <v-menu close-on-content-click="">
                                                                                        <template v-slot:activator="{ on }">
                                                                                            <v-btn min-width="27"
                                                                                                   height="29"
                                                                                                   v-on="on"
                                                                                                   color="success"
                                                                                                   outlined=""
                                                                                                   dark=""
                                                                                                   class="elevation-0 mr-2 px-0">
                                                                                                <v-icon>mdi-plus-circle</v-icon>
                                                                                            </v-btn>
                                                                                        </template>
                                                                                    </v-menu>
                                                                                </template>
                                                                            </v-data-table>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                            <v-row v-if="selectedUsers.length">
                                                                                <!--<v-col v-if="selectedUsers.length">
                                                                                <v-avatar size="40">
                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                         alt="User DP" />
                                                                                </v-avatar> {{NotifyMessage}} the {{ AddSubGroupInput.GroupName }}
                                                                            </v-col>-->

                                                                                <v-col cols="12" md="8" class="py-0">
                                                                                    <v-list class="py-0">
                                                                                        <v-list-item>
                                                                                            <v-list-item-icon>
                                                                                                <v-avatar size="25" class="elevation-1">
                                                                                                    <v-icon>mdi-account-group-outline</v-icon>
                                                                                                </v-avatar>
                                                                                            </v-list-item-icon>
                                                                                            <v-list-item-content>
                                                                                                {{NotifyMessage}} the group {{ AddSubGroupInput.GroupName }}
                                                                                            </v-list-item-content>
                                                                                        </v-list-item>
                                                                                    </v-list>
                                                                                </v-col>
                                                                                <v-col cols="12" md="4" class="py-0">
                                                                                    <!-- <v-checkbox color="success-check-box" v-model="NotifyCheckbox" label="Notify user"></v-checkbox> -->
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right ">
                                                                            <v-btn id="AutoTest_U6" text="" small="" @click="fnAddSubGroup(false)">Skip for now ></v-btn>
                                                                            <Secondary-Button id="AutoTest_U7" class="btn-122x36 mr-4" title="Confirm" @click.native="fnAddSubGroup(true)" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                                            <Default-Button-Outlined id="AutoTest_U8" class="btn-122x36" title="Previous" @click.native="AddSubGroupRegSteps = 1;selectedUsers=[];NotifyMessage=null"></Default-Button-Outlined>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-container>
                                                            </v-stepper-content>

                                                            <v-stepper-content step="3" class="pa-0">
                                                                <v-container fluid="" class="pt-0">
                                                                    <v-row no-gutters="">
                                                                        <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                            <h4 class="text-center heading-3">Super !</h4>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                            <v-avatar size="150" tile="" rounded="">
                                                                                <img color="success" src="../../images/many-people-outline.svg" />
                                                                            </v-avatar>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                            <p class="text-medium">
                                                                                Your sub group has been created successfully.<br />Bask in your glory !
                                                                            </p>
                                                                        </v-col>
                                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                            <Secondary-Button id="AutoTest_U9" class="btn-122x36" title="Done" @click.native="AddSubGroupDialog=false;AddSubGroupRegSteps=1"></Secondary-Button>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-container>
                                                            </v-stepper-content>
                                                        </v-stepper-items>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-card-text>

                            <v-divider></v-divider>
                        </v-card>
                    </v-dialog>
                </v-row>
                <v-row class="px-2">
                    <v-col cols="12" md="6">
                        <!--Group Details section 1-->
                        <v-card>
                            <v-row class="px-4">
                                <v-col>
                                    <v-row no-gutters>
                                        <v-col class="d-inline-flex pt-1">
                                            <div>
                                                <v-avatar size="20" class="elevation-1 mr-1">
                                                    <v-icon size="12">mdi-account-group-outline</v-icon>
                                                </v-avatar>
                                            </div>
                                            <div>
                                                <h6 class="page-sub-head">
                                                    User Group Details
                                                </h6>
                                            </div>
                                        </v-col>
                                        <v-col>
                                            <v-row>
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U10" max-width="29" min-width="29" height="27" outlined=""
                                                       dark="" color="grey" class="mr-4" @click="fnEditGroupClick()" v-bind:disabled="!screenAccess">
                                                    <v-icon>mdi-pencil-outline</v-icon>
                                                </v-btn>
                                                <v-btn id="AutoTest_U11" max-width="29" min-width="29" height="27" outlined
                                                       dark="" color="grey" class="mr-2" @click="DeleteGroupDialog=true" v-bind:disabled="!screenAccess">
                                                    <v-icon>mdi-trash-can-outline</v-icon>
                                                </v-btn>
                                            </v-row>

                                            <!--Edit Group Dialog-->
                                            <v-dialog v-model="EditGroupDialog" width="800">
                                                <v-card>
                                                    <v-card-title primary-title class="page-head">
                                                        <v-avatar color="white" size="30" class="elevation-1 mr-2">
                                                            <v-icon size="25" color="primary">mdi-account-edit-outline</v-icon>
                                                        </v-avatar>
                                                        Edit Group Details
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_U12" text="" @click="fnEditGroupCancelClick()">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-card-text class="pt-0">
                                                        <v-container fluid="" class="pt-0">
                                                            <v-row>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                    <v-avatar size="100" class="elevation-1 mr-2" light :color="`${getcolor(groupInfo.GroupName)}`">
                                                                        <v-img v-bind:src="profileeditImage" height="100" width="100" v-if="profileeditImage.length>0"></v-img>
                                                                        <span class="headline white--text" v-else>  {{groupInfo.GroupName == null ? "" :groupInfo.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                        <div class="dp-edit-btn" style="top:50px">
                                                                            <input class="upload" type="file" id="file" hide-details accept="image/*" @change="previewImage" />
                                                                        </div>
                                                                    </v-avatar>
                                                                </v-col>
                                                            </v-row>
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <v-row :justify="justifyCenter">
                                                                    <v-col md="6">
                                                                        <v-text-field outlined="" dense="" label="User Group Name" type="text" v-model="EditGroupInput.GroupName">
                                                                        </v-text-field>
                                                                        <v-textarea rows="3" outlined="" dense="" label="Description" type="text" v-model="EditGroupInput.GroupDescription">
                                                                        </v-textarea>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <v-row>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                    <Secondary-Button id="AutoTest_U13" class="btn-122x36 mr-4" title="Save" @click.native="fnEditGroupSave()"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_U14" class="btn-122x36" title="Cancel" @click.native="fnEditGroupCancelClick()"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-card-text>
                                                </v-card>
                                            </v-dialog>

                                            <!--Delete Group Dialog-->
                                            <v-dialog v-model="DeleteGroupDialog" width="800">
                                                <v-card>
                                                    <v-card-title primary-title class="page-head">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                        </v-avatar>
                                                        Delete User Group
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_U15" text @click="DeleteGroupDialog=false">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-card-text class="pt-0">
                                                        <v-container fluid="" class="pt-0">
                                                            <v-row>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h4 class="text-center heading-3">
                                                                        Are you sure you want to remove this user group?
                                                                    </h4>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-card-text>
                                                    <v-card-text class="pt-0">
                                                        <v-container fluid class="pt-0">
                                                            <v-row>
                                                                <v-col v-if="projectsTableItems.length>0" cols="12" md="12" class="pt-0 mb-2 text-left">
                                                                    <v-list class="base-border-radius warning-div pa-0">
                                                                        <v-list-item>
                                                                            <v-list-item-icon class="mt-2">
                                                                                <v-icon size="30" color="#F2C64D">mdi-alert-circle</v-icon>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content class="pb-0">
                                                                                <p class="pa-0">
                                                                                    <span class="name">
                                                                                        <v-avatar size="20" color="#fff" class="elevation-0">
                                                                                            <v-icon size="12" color="grey">mdi-account-group-outline</v-icon>
                                                                                        </v-avatar>
                                                                                        {{ groupInfo.GroupName }}
                                                                                    </span>
                                                                                    <span class="message">
                                                                                        is part of {{projectsTableItems.length}} projects.
                                                                                        By removing this group all the active projects will become inactive.
                                                                                    </span>
                                                                                </p>
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </v-col>
                                                                <v-col v-if="projectsTableItems.length>0" cols="12" xs="12" sm="12" md="12">
                                                                    <!--User List with checkbox add here-->
                                                                    <v-data-table :headers="projectsTableHeaders"
                                                                                  :items="projectsTableItems"
                                                                                  item-key="ProjectId"
                                                                                  class="elevation-0"
                                                                                  fixed-header
                                                                                  height="28vh">
                                                                        <template v-slot:body="{ items }">
                                                                            <tbody>
                                                                                <tr v-for="item in items" :key="item.ProjectId" v-bind:style="{backgroundColor: ProjectRowColor[item.Status]}">
                                                                                    <td>{{ item.ProjectName }}</td>
                                                                                    <td>{{ item.Status }}</td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </template>
                                                                    </v-data-table>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                    <Secondary-Button id="AutoTest_U16" class="btn-122x36 mr-4" title="Delete" @click.native="fnDeleteGroup()"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_U17" class="btn-122x36" title="Cancel" @click.native="DeleteGroupDialog=false"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-card-text>
                                                </v-card>
                                            </v-dialog>
                                        </v-col>
                                    </v-row>

                                    <v-row>
                                        <v-col>
                                            <v-list class="py-0">
                                                <v-list-item class="my-0">
                                                    <v-list-item-icon>
                                                        <v-avatar size="50" class="elevation-1" light :color="`${getcolor(groupInfo.GroupName)}`">
                                                            <img v-bind:src="userimage + groupInfo.UserGroupImage" alt="User DP" v-if="groupInfo.UserGroupImage.length >0" />
                                                            <span class="headline white--text" v-else>  {{groupInfo.GroupName == null ? "" :groupInfo.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                    </v-list-item-icon>
                                                    <v-list-item-content class="pl-2">
                                                        {{ this.groupInfo.GroupName }}
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                            <v-spacer></v-spacer>
                                        </v-col>
                                    </v-row>

                                    <v-row>
                                        <v-col cols="12" md="12" class="py-0">
                                            <p class="pl-2" v-bind:class="{ 'text-limit': DiscriExpandBtn }">
                                                <span class="font-weight-medium">Description:</span>
                                                {{ this.groupInfo.Description }}
                                            </p>
                                            <v-btn small="" text="" color="primary" class="pull-right" v-model="DiscriExpandBtn" v-on:click="DiscriExpandBtn=!DiscriExpandBtn">
                                                <span v-show="DiscriExpandBtn">More</span>
                                                <span v-show="!DiscriExpandBtn">Less</span>
                                            </v-btn>
                                        </v-col>
                                        <v-col cols="12" md="12" class="py-0">
                                            <p class="pl-2">
                                                <span class="font-weight-medium">Sub Groups Count :</span>
                                                {{ this.groupInfo.SubGroupsCount }}
                                            </p>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-card>

                        <!--Members section 2-->
                        <v-card class="mt-4">
                            <v-row no-gutters>
                                <!-- members table -->
                                <v-col>
                                    <v-data-table :headers="membersTableHeaders"
                                                  :items="membersTableItems"
                                                  item-key="ID"
                                                  class="elevation-0"
                                                  :search="searchmemberuser"
                                                  fixed-header
                                                  height="25vh">
                                        <!--Table top slot-->
                                        <template v-slot:top>
                                            <v-row class="px-4">
                                                <v-col class="d-inline-flex pt-4">
                                                    <div>
                                                        <v-avatar size="20" class="elevation-1 mr-1">
                                                            <v-icon size="12" color="grey">mdi-account-outline</v-icon>
                                                        </v-avatar>
                                                    </div>
                                                    <div>
                                                        <h5 class="page-sub-head">
                                                            Members
                                                        </h5>
                                                    </div>
                                                </v-col>
                                                <v-col md="auto">
                                                    <v-btn id="AutoTest_U18" color="success-btn" dark class="elevation-0"
                                                           @click="AddUserDialog=true,fnGetUsersList()" v-bind:disabled="!screenAccess">Add User</v-btn>
                                                </v-col>

                                                <!--Add Users Dialog-->
                                                <v-dialog v-model="AddUserDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head">
                                                            <v-avatar color="white" size="30" class="elevation-1 mr-2">
                                                                <v-icon size="25" color="primary">mdi-account</v-icon>
                                                            </v-avatar>
                                                            Add Users
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_U19" text="" @click="AddUserDialog=false;selectedUsers=[];NotifyMessage=null">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="py-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <h4 class="text-left page-sub-head">
                                                                            <v-avatar size="20" class="elevation-1 mr-1">
                                                                                <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                                            </v-avatar>
                                                                            Add Users to the group {{groupInfo.GroupName}}
                                                                        </h4>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <v-row :justify="justifyCenter" no-gutters>
                                                                            <v-col cols="12" md="auto" lg="12" class="px-4">
                                                                                <v-text-field hide-details v-model="searchuser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col v-if="selectedUsers.length>0" md="12">
                                                                        <div class="chip-container">
                                                                            <v-chip-group multiple
                                                                                          column
                                                                                          active-class="primary--text">
                                                                                <v-chip id="AutoTest_U20" close v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                                        @click:close="$delete(selectedUsers, key)">
                                                                                    <v-icon left color="#707070">mdi-account</v-icon>
                                                                                    {{ item.Name }}
                                                                                </v-chip>
                                                                            </v-chip-group>
                                                                        </div>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                        <!--User List with checkbox add here-->
                                                                        <v-data-table :headers="userTableHeaders"
                                                                                      :items="userTableItems"
                                                                                      :search="searchuser"
                                                                                      show-select=""
                                                                                      item-key="ID"
                                                                                      class="elevation-0"
                                                                                      fixed-header
                                                                                      height="20vh"
                                                                                      v-model="selectedUsers">
                                                                        </v-data-table>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-left py-0">
                                                                        <v-row v-if="selectedUsers.length">
                                                                            <!--<v-col v-if="selectedUsers.length">
                                                                            <v-avatar size="40">
                                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                     alt="User DP" />
                                                                            </v-avatar> {{NotifyMessage}} the {{ groupInfo.GroupName }}
                                                                        </v-col>-->

                                                                            <v-col cols="12" md="8" class="py-0">
                                                                                <v-list class="py-0">
                                                                                    <v-list-item>
                                                                                        <v-list-item-icon>
                                                                                            <v-avatar size="25" class="elevation-1">
                                                                                                <v-icon>mdi-account-group-outline</v-icon>
                                                                                            </v-avatar>
                                                                                        </v-list-item-icon>
                                                                                        <v-list-item-content>
                                                                                            {{NotifyMessage}} the group {{ groupInfo.GroupName }}
                                                                                        </v-list-item-content>
                                                                                    </v-list-item>
                                                                                </v-list>
                                                                            </v-col>
                                                                            <v-col cols="12" md="4" class="py-0">
                                                                                <!-- <v-checkbox color="success-check-box" v-model="NotifyCheckbox" label="Notify user"></v-checkbox> -->
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pb-0">
                                                                        <Secondary-Button id="AutoTest_U21" class="btn-122x36 mr-4" title="Add" @click.native="fnAddAssignedUsers()" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U22" class="btn-122x36" title="Cancel" @click.native="AddUserDialog=false;selectedUsers=[];NotifyMessage=null"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-dialog>

                                                <!--Seacrh Member User-->
                                                <v-col>
                                                    <v-text-field outlined dense v-model="searchmemberuser" label="Search"
                                                                  type="Text" prepend-inner-icon="mdi-magnify" hide-details class="filter-search-field">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </template>

                                        <!--members name row-->
                                        <template v-slot:item.Name="{item}">
                                            <router-link class="text--default" link="" :to="{ name: 'userindividual', query: { WsID: WorkspaceID, Access : RWAccess ,UID: item.ID}  }">
                                                <v-avatar size="30" class="mr-2" light :color="`${getcolor(item.Name)}`">
                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>
                                                {{ item.Name }}
                                            </router-link>
                                        </template>

                                        <!--members table action row-->
                                        <template v-slot:item.memberTableActions="{item}">
                                            <v-btn id="AutoTest_U23" max-width="29" min-width="29" height="27" outlined=""
                                                   dark color="grey" class="mr-2" @click.stop="DeleteMemberUserDialog=true,DeleteMemberUserID=item.ID,MemberUser =item.Name" v-bind:disabled="!screenAccess">
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-btn>
                                        </template>
                                    </v-data-table>

                                    <!--Delete Member user Dialog-->
                                    <v-dialog v-model="DeleteMemberUserDialog" width="800">
                                        <v-card>
                                            <v-card-title primary-title class="page-head">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                </v-avatar>
                                                Delete User from Group
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U24" text="" @click="DeleteMemberUserDialog=false,DeleteMemberUserID=0,MemberUser=''">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>
                                            <v-card-text class="pt-0">
                                                <v-container fluid="" class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <h4 class="text-center heading-3">
                                                                Are you sure you want to remove this user from group?
                                                            </h4>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <Secondary-Button id="AutoTest_U25" class="btn-122x36 mr-4" title="Delete" @click.native="fnDeleteAssignedUser(),DeleteMemberUserDialog=false"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_U26" class="btn-122x36" title="Cancel" @click.native="DeleteMemberUserDialog=false,DeleteMemberUserID=0,MemberUser=''"></Default-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-card-text>
                                        </v-card>
                                    </v-dialog>
                                </v-col>
                            </v-row>
                        </v-card>
                    </v-col>

                    <v-col cols="12" md="6">
                        <!--Sub Group section 3-->
                        <v-card class="mb-4" v-if="this.SubGroupTableItems.length>0">
                            <v-row no-gutters>
                                <!-- sub groups table -->
                                <v-col>
                                    <v-row class="px-4">
                                        <v-col class="d-inline-flex pt-4">
                                            <div>
                                                <v-avatar size="20" class="elevation-1 mx-2">
                                                    <v-icon size="12" color="grey">mdi-account-multiple-outline</v-icon>
                                                </v-avatar>
                                            </div>
                                            <div>
                                                <h5 class="page-sub-head">Sub Groups</h5>
                                            </div>
                                        </v-col>
                                        <v-col md="auto">
                                            <v-btn id="AutoTest_U27" color="success-btn" dark @click.stop="profilesubgroupImage='';AddSubGroupDialog=true"
                                                   class="elevation-0">Add Sub Group</v-btn>
                                        </v-col>
                                        <v-col>
                                            <v-text-field outlined dense label="Search" v-model="searchsubgroup" type="Text" prepend-inner-icon="mdi-magnify" hide-details class="filter-search-field">

                                            </v-text-field>
                                        </v-col>
                                    </v-row>

                                    <v-data-table :headers="SubGroupTableHeaders"
                                                  :items="SubGroupTableItems"
                                                  :search="searchsubgroup"
                                                  item-key="GroupId"
                                                  class="elevation-0"
                                                  fixed-header height="25vh">

                                        <template v-slot:item.GroupName="{ item }">
                                            <router-link class="text--default" link :to="{ name: 'usergroupindividual', query: { WsID: WorkspaceID , Access : RWAccess,GroupId: item.GroupId } }">
                                                <v-avatar class="mr-1" size="30" light :color="`${getcolor(item.GroupName)}`">
                                                    <span class="white--text">{{item.GroupName == null ? "" : item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>{{ item.GroupName }}
                                            </router-link>
                                        </template>

                                        <!-- Sub Group Actions-->

                                        <template v-slot:item.subGroupActions="{item}">
                                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                                                <template v-slot:activator="{ on }">
                                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                                        <v-icon>
                                                            mdi-dots-vertical
                                                        </v-icon>
                                                    </v-btn>
                                                </template>
                                                <v-list color="white" light width="auto" class="action-button-list">
                                                    <v-list-item id="AutoTest_U28" @click.stop="fnEditSubGroupClick(item)">
                                                        <v-list-item-icon>
                                                            <v-icon>mdi-pencil-outline</v-icon>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            Edit
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                    <v-list-item id="AutoTest_U29" link @click.stop="fnAddUserSubGroupClick(item)">
                                                        <v-list-item-icon>
                                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            Add User
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                    <v-list-item id="AutoTest_U30" link="" @click.stop="fnDeleteSubGroupClick(item)">
                                                        <v-list-item-icon>
                                                            <v-icon>mdi-trash-can-outline</v-icon>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            Delete
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </v-list>

                                                <!--Edit Sub Group Dialog-->
                                                <v-dialog v-model="EditSubGroupDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                <v-icon size="20" color="primary">mdi-account-edit-outline</v-icon>
                                                            </v-avatar>
                                                            Edit Sub Group Details
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_U31" text @click="EditSubGroupDialog=false">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid="" class="pt-0">
                                                                <v-row>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                        <v-avatar size="75" class="elevation-1" light :color="RandomColor">
                                                                            <v-img v-bind:src="profilesubgroupImage" height="75" width="75" v-if="profilesubgroupImage.length>0"></v-img>
                                                                            <span class="headline white--text" v-else>  {{EditSubGroupInput.GroupName == null ? "" : EditSubGroupInput.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                            <div class="dp-edit-btn" style="top:30px">
                                                                                <input class="upload" type="file" id="fileeditsubgroup" hide-details accept="image/*" @change="previewImageSubgroup" />
                                                                            </div>
                                                                        </v-avatar>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-col cols="12" xs="12" sm="12" md="12">
                                                                    <v-row :justify="justifyCenter">
                                                                        <v-col md="6">
                                                                            <v-text-field outlined dense label="Sub User Group Name" type="text" v-model="EditSubGroupInput.GroupName">
                                                                            </v-text-field>
                                                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="EditSubGroupInput.GroupDescription">
                                                                            </v-textarea>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-col>
                                                                <v-row>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_U32" class="btn-122x36 mr-4" title="Save" @click.native="fnEditSubGroupSave()"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U33" class="btn-122x36" title="Cancel" @click.native="EditSubGroupDialog=false"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-dialog>

                                                <!--Add Users Dialog for sub group-->
                                                <v-dialog v-model="AddUserSubGroupDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                                            </v-avatar>
                                                            Add Users
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_U34" text @click="fnAddUserSubGroupCancelClick()">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="py-0">
                                                            <v-container fluid class="py-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0 pb-4">
                                                                        <h4 class="text-left heading-3">
                                                                            <v-avatar size="20" class="elevation-1 mr-1">
                                                                                <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                                            </v-avatar>
                                                                            Add Users to a user group
                                                                        </h4>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid class="py-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <v-row :justify="justifyCenter" no-gutters>
                                                                            <v-col cols="12">
                                                                                <v-text-field v-model="searchuser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details class="mb-2">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col v-if="selectedUsers.length>0" md="12">
                                                                        <div class="chip-container">
                                                                            <v-chip-group multiple
                                                                                          column
                                                                                          active-class="primary--text">
                                                                                <v-chip id="AutoTest_U35" close v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                                        @click:close="$delete(selectedUsers, key)">
                                                                                    <v-icon left color="#707070">mdi-account-outline</v-icon>
                                                                                    {{ item.Name }}
                                                                                </v-chip>
                                                                            </v-chip-group>
                                                                        </div>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                        <!--User List with checkbox add here-->
                                                                        <v-data-table :headers="userTableHeaders"
                                                                                      :items="userTableItems"
                                                                                      :search="searchuser"
                                                                                      show-select=""
                                                                                      item-key="ID"
                                                                                      class="elevation-0"
                                                                                      fixed-header
                                                                                      height="20vh"
                                                                                      v-model="selectedUsers">
                                                                        </v-data-table>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                        <v-row v-if="selectedUsers.length">
                                                                            <!--<v-col v-if="selectedUsers.length">
                                                                            <v-avatar size="40">
                                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                     alt="User DP" />
                                                                            </v-avatar> {{NotifyMessage}} the group {{ AddSubGroupInput.GroupName }}
                                                                        </v-col>-->
                                                                            <v-col cols="12" md="8" class="py-0">
                                                                                <v-list class="py-0">
                                                                                    <v-list-item>
                                                                                        <v-list-item-icon>
                                                                                            <v-avatar size="25" class="elevation-1">
                                                                                                <v-icon>mdi-account-group-outline</v-icon>
                                                                                            </v-avatar>
                                                                                        </v-list-item-icon>
                                                                                        <v-list-item-content>
                                                                                            {{NotifyMessage}} the group {{ AddSubGroupInput.GroupName }}
                                                                                        </v-list-item-content>
                                                                                    </v-list-item>
                                                                                </v-list>
                                                                            </v-col>
                                                                            <v-col cols="12" md="4" class="py-0">
                                                                                <!-- <v-checkbox color="success-check-box" v-model="NotifyCheckbox" label="Notify user"></v-checkbox> -->
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_U36" class="btn-122x36 mr-4" title="Add" @click.native="fnAddUserSubGroup()" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U37" class="btn-122x36" title="Cancel" @click.native="fnAddUserSubGroupCancelClick()"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-dialog>

                                                <!--Delete Sub Group Dialog-->
                                                <v-dialog v-model="DeleteSubGroupDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                            </v-avatar>
                                                            Delete User Sub Group
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_U38" text="" @click="DeleteSubGroupDialog = false, DeleteSubGroupProjectsItems = []">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid="" class="pt-0">
                                                                <v-row>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <h4 class="text-center heading-3">
                                                                            Are you sure you want to remove this user group?
                                                                        </h4>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid="" class="pt-0">
                                                                <v-row>
                                                                    <div v-if="DeleteSubGroupProjectsItems.length">
                                                                        <v-col cols="12" md="12" class="pt-0 mb-2 text-left">
                                                                            <v-chip color="#FFF0C7" class="ma-2" label text-color="#E29E29">
                                                                                <v-icon left color="#F2C64D">mdi-alert-circle</v-icon>
                                                                                <v-icon size="25" color="#00000029" class="mr-2">mdi-account-group-outline</v-icon>
                                                                                {{ item.GroupName }} is part of {{ DeleteSubGroupProjectsItems.length }} projects.
                                                                                By removing this group all the active projects will become inactive.
                                                                            </v-chip>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                            <!--User List with checkbox add here-->
                                                                            <v-data-table :headers="projectsTableHeaders"
                                                                                          :items="DeleteSubGroupProjectsItems"
                                                                                          item-key="ProjectId"
                                                                                          class="elevation-1"
                                                                                          fixed-header
                                                                                          height="28vh">
                                                                                <template v-slot:body="{ items }">
                                                                                    <tbody>
                                                                                        <tr v-for="item in items" :key="item.ProjectId" v-bind:style="{backgroundColor: ProjectRowColor[item.Status]}">
                                                                                            <td>{{ item.ProjectName }}</td>
                                                                                            <td>{{ item.Status }}</td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </template>
                                                                            </v-data-table>
                                                                        </v-col>
                                                                    </div>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_U39" class="btn-122x36 mr-4" title="Delete" @click.native="fnDeleteSubGroup(item)"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U40" class="btn-122x36" title="Cancel" @click.native="DeleteSubGroupDialog = false, DeleteSubGroupProjectsItems = []"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-dialog>

                                            </v-menu>
                                        </template>
                                        <!-- Sub Group Action Ends here -->

                                    </v-data-table>
                                </v-col>
                            </v-row>
                        </v-card>

                        <!--Project section 4-->
                        <v-card>
                            <v-row>
                                <!-- user groups table -->
                                <v-col>
                                    <v-data-table :headers="projectsTableHeaders"
                                                  :items="projectsTableItems"
                                                  :search="searchprojects"
                                                  item-key="ProjectId"
                                                  class="elevation-0"
                                                  fixed-header
                                                  height="25vh">
                                        <template v-slot:top>
                                            <v-row class="px-4">
                                                <v-col class="d-inline-flex pt-2">
                                                    <div>
                                                        <v-avatar size="20" class="elevation-1 mr-1">
                                                            <v-icon size="12" color="grey">mdi-file-document-box-multiple-outline</v-icon>
                                                        </v-avatar>
                                                    </div>
                                                    <div>
                                                        <h5 class="page-sub-head">
                                                            Projects
                                                        </h5>
                                                    </div>
                                                </v-col>
                                                <v-col md="auto" class="pt-1">
                                                    <v-text-field outlined dense label="Search" type="Text"
                                                                  v-model="searchprojects" prepend-inner-icon="mdi-magnify" hide-details class="filter-search-field">

                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </template>
                                    </v-data-table>
                                </v-col>
                            </v-row>
                        </v-card>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
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
  import SecondaryButton from '@/components/secondary-button.vue'
  import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
  import objUtils from '../../utils.js'

    var group;
    async function importscript() {
        return Promise.all([
            import("../../BL/UserGroup.js").then(mod => {
                group = new mod.UserGroup();
            })
        ]);
    }
    export default {
      components:{
        'Secondary-Button': SecondaryButton,
        'Default-Button-Outlined': DefaultButtonOutlined,
      },
        data() {
            return {
                snackbarMsg: false,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                //groups section
                groupInfo: {
                    Description: "",
                    GroupName: "",
                    GroupId: 0,
                    SubGroupsCount: 0,
                    UserGroupImage: ""
                },
                EditGroupInput: {
                    GroupId: 0,
                    GroupName: "",
                    GroupDescription: ""
                },
                EditGroupDialog: false,
                DeleteGroupDialog: false,
                DiscriExpandBtn: true,

                //members section
                membersTableHeaders: [
                    { text: 'Name', align: 'left', value: 'Name' },
                    { text: 'User Role', value: 'UserRole' },
                    { text: "Actions", value: "memberTableActions", sortable: false }
                ],
                membersTableItems: [],
                searchmemberuser: "",
                AddUserDialog: false,
                DeleteMemberUserDialog: false,
                DeleteMemberUserID: 0,
                MemberUser :"",

                //sub group section
                SubGroupTableHeaders: [
                    {
                        text: 'Name',
                        align: 'left',
                        value: 'GroupName',
                    },
                    { text: 'Members Count', value: 'UsersCount' },
                    { text: 'Actions', value: 'subGroupActions', sortable: false },
                ],
                SubGroupTableItems: [],
                searchsubgroup: "",
                AddSubGroupInput: {
                    GroupId: 0,
                    GroupName: "",
                    GroupDescription: "",
                    ParentGroupId: 0
                },
                AddSubGroupRegSteps: 0,
                AddSubGroupDialog: false,
                //actions on Sub Group
                EditSubGroupDialog: false,
                EditSubGroupInput: {
                    GroupId: 0,
                    GroupName: "",
                    GroupDescription: ""
                },
                AddUserSubGroupDialog: false,
                DeleteSubGroupDialog: false,
                DeleteSubGroupProjectsItems: [],
                //projects section
                projectsTableHeaders: [
                    {
                        text: "Project Name",
                        align: "left",
                        value: "ProjectName",
                        icon: "mdi-account-multiple-outline"
                    },
                    { text: "Status", value: "Status" }
                ],
                projectsTableItems: [],
                searchprojects: "",
                ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "" },

                //users table list comes in both members and sub group sections
                userTableHeaders: [
                    { text: 'Name', align: 'left', value: 'Name' },
                    { text: 'User Role', value: 'userRole' },
                    { text: 'Project Count', value: 'userProjectCount' },
                ],
                userTableItems: [],
                selectedUsers: [],
                searchuser: '',

                //used with Notify checkbox
                NotifyMessage: "",
                NotifyCheckbox: true,
                justifyCenter: "center",
                WorkspaceID: 0,
                userimage: "",
                RandomColor: "",
                profileeditImage: "",
                profilesubgroupImage: "",
                subUserTableItems: [],
                alphanumericRules: [
                    v => !v ||  /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name'
                ],
                screenAccess: 0,
                RWAccess: 0,
            };
        },//data end
        created() {
            if (this.$route.query.GroupId) {
                this.groupInfo.GroupId = this.$route.query.GroupId;
            }
        },
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";

            this.WorkspaceID = this.$route.query.WsID;
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            await importscript();
            await this.fnGetGroupDetails(); //get the group details by GroupId
            await this.fnGetSubGroupUsersList(); // for adding sub group userlist
            await this.fnGetAssignedUsersList(); //assigned user list for a group(parent group always)
            await this.fnGetSubGroupsList(); //get the sub groups list by GroupId
            await this.fnGetProjectsList(); //get projects list by GroupId
        },
        watch: {
            $route() {
                if (this.$route.query.GroupId) {
                    this.groupInfo.GroupId = this.$route.query.GroupId;
                    this.fnGetGroupDetails();
                    this.fnGetAssignedUsersList();
                    this.fnGetSubGroupsList();
                    this.fnGetProjectsList();
                }
            },
            selectedUsers: function () {
                this.NotifyMessage = "";
                if (this.selectedUsers.length > 0) {
                    if (this.selectedUsers.length > 2) {
                        this.NotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " and " + (this.selectedUsers.length - 2) + " more will be added to ";
                    } else if (this.selectedUsers.length == 2) {
                        this.NotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " will be added to ";
                    } else {
                        this.NotifyMessage = this.selectedUsers[0].Name + " will be added to ";
                    }
                }
            }
        },
        methods: {

            /// <summary>
            ///  getcolor :- Function used to get color of users
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
                return objUtils.common.getcolor(name);
            },

            //================== Group details section functions ===================
            //================== Edit icon function =================
            /// <summary>
            ///  fnEditGroupClick :- Function used to open user group details dialog to edit when click on edit icon
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditGroupClick() {
                this.EditGroupDialog = true;
                this.EditGroupInput.GroupId = this.groupInfo.GroupId;
                this.EditGroupInput.GroupName = this.groupInfo.GroupName;
                this.EditGroupInput.GroupDescription = this.groupInfo.Description;
            },

            /// <summary>
            ///  fnEditGroupCancelClick :- Function used to cancel user group dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditGroupCancelClick() {
                this.EditGroupDialog = false;
                this.EditGroupInput.GroupId = 0;
                this.EditGroupInput.GroupName = "";
                this.EditGroupInput.GroupDescription = "";
            },

            /// <summary>
            ///  fnEditGroupSave :- Function used to update user group details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnEditGroupSave() {
                var url = window.SERVER_URL + "/UserGroups/UpdateUserGroup";
                var data = new FormData();
                data.append("GroupId", this.EditGroupInput.GroupId);
                data.append("GroupName", this.EditGroupInput.GroupName);
                data.append("Description", this.EditGroupInput.GroupDescription);
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("location", "usergroup");

                var input = document.getElementById('file');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("UserGroupImage", input.files[i]);
                    }
                }

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully updated " + this.EditGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to update " + this.EditGroupInput.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.fnGetGroupDetails()
                this.fnEditGroupCancelClick();
            },

            //==================== Delete icon function ==========================
            /// <summary>
            ///  fnDeleteGroup :- Function used to delete user group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnDeleteGroup() {
                this.DeleteGroupDialog = false;
                var url = window.SERVER_URL + "/UserGroups/DeleteUserGroup";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);
                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully deleted the group " + this.groupInfo.GroupName);
                    this.successSnackbarMsg = true;
                    this.$router.push({ name: "usersgroup", query: { WsID: this.WorkspaceID, Access: RWAccess} });
                } else {
                    this.Msg = ("Failed to delete the group " + this.groupInfo.GroupName);
                    this.errorSnackbarMsg = true;
                }
            },

            //======================= Member details section functions =================
            //======================= Add User button function ========================
            /// <summary>
            ///  fnAddAssignedUsers :- Function used to add users to the group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddAssignedUsers() {
                if (this.selectedUsers.length == 0) {
                    this.Msg = ("No Users selected. Please select the users to add to group" + this.groupInfo.GroupName);
                    this.infoSnackbarMsg = true;
                    return;
                }
                var url = window.SERVER_URL + "/UserGroups/AddUserGroup";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);
                data.append("GroupName", this.groupInfo.GroupName);
                data.append("UserList", JSON.stringify(this.selectedUsers));
                data.append("Notify", this.NotifyCheckbox);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data == this.groupInfo.GroupId) {
                    this.Msg = ("Successfully added users to the group " + this.groupInfo.GroupName);
                    this.successSnackbarMsg = true;
                }
                else if (result.data == 0) {
                    this.Msg = ("Failed to add users to the group " + this.groupInfo.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.searchuser = "";
                this.NotifyMessage = "";
                this.NotifyCheckbox = true;
                this.fnGetAssignedUsersList();
                this.AddUserDialog = false;
                this.selectedUsers = [];
            },

            /// <summary>
            ///  fnDeleteAssignedUser :- Function used to delete user from the group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnDeleteAssignedUser() {
                var url = window.SERVER_URL + "/UserGroups/DeleteAssignedUser";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);
                data.append("UserId", this.DeleteMemberUserID);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully deleted member user " + this.MemberUser + " from the group " + this.groupInfo.GroupName);
                    this.successSnackbarMsg = true;
                    this.fnGetAssignedUsersList();
                    this.fnGetUsersList();
                } else {
                    this.Msg = ("Failed to delete member user from the group " + this.groupInfo.GroupName);
                    this.errorSnackbarMsg = true;
                }
            },

            //=============== SubGroup details section functions =================
            //=============== Add Sub Group button function ======================
            /// <summary>
            ///  fnAddSubGroupCancelClick :- Function used to cancel sub group dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnAddSubGroupCancelClick() {
                this.AddSubGroupDialog = false;
                this.selectedUsers = [];
                this.NotifyMessage = null;
                this.NotifyCheckbox = true;
                this.AddSubGroupRegSteps = 1;
                this.AddSubGroupInput.GroupId = 0;
                this.AddSubGroupInput.GroupName = "";
                this.AddSubGroupInput.GroupDescription = "";
                this.AddSubGroupInput.ParentGroupId = 0;
            },

            /// <summary>
            ///  fnAddSubGroup :- Function used to add new user sub group
            /// </summary>
            /// <param name="isUserList"></param>
            /// <returns> </returns>
            async fnAddSubGroup(isUserList) {
                if (this.AddSubGroupInput.GroupId == 0 && this.AddSubGroupInput.GroupName == "") {
                    this.Msg = ("Sub Group Name is required");
                    this.infoSnackbarMsg = true;
                    return;
                }

                if (isUserList == false) {
                    this.selectedUsers = [];
                } else {
                    if (this.selectedUsers.length == 0) {
                        this.Msg = ("No Users selected. Please select the users to add to sub group", this.AddSubGroupInput.GroupName);
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }

                var url = window.SERVER_URL + "/UserGroups/AddUserSubGroup";
                var data = new FormData();
                data.append("GroupId", this.AddSubGroupInput.GroupId);
                data.append("GroupName", this.AddSubGroupInput.GroupName);
                data.append("Description", this.AddSubGroupInput.GroupDescription);
                data.append("ParentGroupId", this.groupInfo.GroupId);
                data.append("UserList", JSON.stringify(this.selectedUsers));
                data.append("Notify", this.NotifyCheckbox);
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("location", "usergroup");

                var input = document.getElementById('filesubgroup');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("UserGroupImage", input.files[i]);
                    }
                }

                var result = await group.PostData(url, data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data == this.AddSubGroupInput.GroupId) {
                    this.Msg = ("Successfully added users to the sub group" + this.AddSubGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                }
                else if (result.data && isUserList == false) {
                    this.Msg = ("Successfully added the sub group " + this.AddSubGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                }
                else if (result.data && isUserList == true) {
                    this.Msg = ("Successfully added the sub group " + this.AddSubGroupInput.GroupName + " and users");
                    this.successSnackbarMsg = true;
                }
                else if (result.data == 0) {
                    this.Msg = ("Failed to add the sub group " + this.AddSubGroupInput.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.AddSubGroupInput.GroupId = 0;
                this.AddSubGroupInput.GroupName = "";
                this.AddSubGroupInput.GroupDescription = "";
                this.AddSubGroupInput.ParentGroupId = 0;
                this.selectedUsers = [];
                this.searchuser = "";
                this.NotifyMessage = null;
                this.NotifyCheckbox = true;
                this.AddSubGroupRegSteps = 3;
                await this.fnGetSubGroupsList(); //refresh the sub group list
            },

            /// <summary>
            ///  fnGetSubGroupsList :- Function used to fetch sub groups list by GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetSubGroupsList() {
                var url = window.SERVER_URL + "/UserGroups/GetSubGroupsList";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);

                var result = await group.PostData(url, data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SubGroupTableItems = result.data;
                }
            },
            //=============== Edit Sub Group action functions ================
            /// <summary>
            ///  fnEditSubGroupClick :- Function used to open sub group details to edit under sub group action
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnEditSubGroupClick(item) {
                this.EditSubGroupDialog = true
                this.EditSubGroupInput.GroupId = item.GroupId;
                this.EditSubGroupInput.GroupName = item.GroupName;
                this.EditSubGroupInput.GroupDescription = item.Description;

                if (item.UserGroupImage != null &&
                    item.UserGroupImage != "" &&
                    item.UserGroupImage != undefined) {
                    this.profilesubgroupImage = window.SERVER_URL + "/Upload" + item.UserGroupImage;
                } else {
                    this.profilesubgroupImage = "";
                }
            },

            /// <summary>
            ///  fnEditSubGroupSave :- Function used to update sub group under sub group action
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnEditSubGroupSave() {
                var url = window.SERVER_URL + "/UserGroups/UpdateUserGroup";
                var data = new FormData();
                data.append("GroupId", this.EditSubGroupInput.GroupId);
                data.append("GroupName", this.EditSubGroupInput.GroupName);
                data.append("Description", this.EditSubGroupInput.GroupDescription);
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("location", "usergroup");

                var input = document.getElementById('fileeditsubgroup');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("UserGroupImage", input.files[i]);
                    }
                }
               
                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg =("Successfully updated " + this.EditSubGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg =("Failed to update " + this.EditSubGroupInput.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.fnGetSubGroupsList()
                this.fnEditSubGroupCancelClick();
                this.profilesubgroupImage = "";
            },

            /// <summary>
            ///  fnEditSubGroupCancelClick :- Function used to cancel sub group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditSubGroupCancelClick() {
                this.EditSubGroupDialog = false;
                this.EditSubGroupInput.GroupId = 0;
                this.EditSubGroupInput.GroupName = "";
                this.EditSubGroupInput.GroupDescription = "";
            },

            //=============== Add User Sub Group action functions ==================
            /// <summary>
            ///  fnAddUserSubGroupClick :- Function used to open assign user sub group dialog under sub group action
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnAddUserSubGroupClick(item) {
                this.AddUserSubGroupDialog = true;
                this.AddSubGroupInput.GroupId = item.GroupId;
                this.AddSubGroupInput.GroupName = item.GroupName;
            },

            /// <summary>
            ///  fnAddUserSubGroupCancelClick :- Function used to clear add user subgroup dialog when click on cancel button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnAddUserSubGroupCancelClick() {
                this.AddUserSubGroupDialog = false;
                this.AddSubGroupInput.GroupId = 0;
                this.AddSubGroupInput.GroupName = "";
                this.selectedUsers = [];
                this.NotifyMessage = null;
                this.NotifyCheckbox = true;
            },

            /// <summary>
            ///  fnAddUserSubGroup :- Function used to add users to the sub group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddUserSubGroup() {
                if (this.selectedUsers.length == 0) {
                    this.Msg = ("No Users selected. Please select the users to add to Sub group");
                    this.infoSnackbarMsg = true;
                    return;
                }
                var url = window.SERVER_URL + "/UserGroups/AddUserGroup";
                var data = new FormData();
                data.append("GroupId", this.AddSubGroupInput.GroupId);
                data.append("GroupName", this.AddSubGroupInput.GroupName);
                data.append("UserList", JSON.stringify(this.selectedUsers));
                data.append("Notify", this.NotifyCheckbox);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data == this.AddSubGroupInput.GroupId) {
                    this.Msg = ("Successfully added users to the sub group " + this.AddSubGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                }
                else if (result.data == 0) {
                    this.Msg = ("Failed to add users to the sub group " + this.AddSubGroupInput.GroupName);
                    this.errorSnackbarMsg = true;
                }

                this.fnAddUserSubGroupCancelClick();
                this.fnGetSubGroupsList();
            },
            //==============Delete Sub Group action functions===============
            /// <summary>
            ///  fnDeleteSubGroupClick :- Function used to open sub group details to remove when click on remove button
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnDeleteSubGroupClick(item) {
                this.DeleteSubGroupDialog = true;
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.DeleteSubGroupProjectsItems = result.data;
                }
            },

            /// <summary>
            ///  fnDeleteSubGroup :- Function used to delete user sub group
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnDeleteSubGroup(item) {
                var url = window.SERVER_URL + "/UserGroups/DeleteUserGroup";
                var data = new FormData();
                data.append("GroupId", item.GroupId);
                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg =("Successfully deleted the sub group " + item.GroupName);
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg =("Failed to delete the sub group " + item.GroupName);
                    this.errorSnackbarMsg = true;
                }
                await this.fnGetSubGroupsList();
                this.DeleteSubGroupDialog = false;
                this.DeleteSubGroupProjectsItems = [];
            },

            /// <summary>
            ///  fnGetGroupDetails :- Function used to fetch group details by GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetGroupDetails() {
                var url = window.SERVER_URL + "/UserGroups/GetGroupDetails";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.groupInfo = result.data;

                    if (result.data.UserGroupImage != "" &&
                        result.data.UserGroupImage != null &&
                        result.data.UserGroupImage != undefined) {
                        this.profileeditImage = window.SERVER_URL + "/Upload/" + result.data.UserGroupImage;
                    }
                }
            },

            /// <summary>
            ///  fnGetSubGroupUsersList :- Function used to fetch sub group user details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetSubGroupUsersList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var url = window.SERVER_URL + "/UserAccount/GetAllUserDetails";
                var result = await group.PostData(url, data);
               
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {                  

                    this.subUserTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnGetUsersList :- Function used to fetch details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var url = window.SERVER_URL + "/UserAccount/GetAllUserDetails";
                var result = await group.PostData(url, data);
                
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                   
                    
                    this.userTableItems = result.data;
                    if (this.membersTableItems.length > 0) {
                        for (var i = 0; i < this.membersTableItems.length; i++) {
                            if (this.membersTableItems[i].ID != 0)
                                this.userTableItems = this.userTableItems.filter(x => x.ID != this.membersTableItems[i].ID);
                        }
                    }
                    
                }
            },

            /// <summary>
            ///  fnGetAssignedUsersList :- Function used to fetch assigned user list for a group (parent group always)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAssignedUsersList() {
                var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await group.PostData(url, data);
               
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.membersTableItems = result.data;
                    
                }
            },

            /// <summary>
            ///  fnGetProjectsList :- Function used to fetch projects list by GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectsList() {
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", this.groupInfo.GroupId);

                var result = await group.PostData(url, data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectsTableItems = result.data;
                }
            },

            /// <summary>
            ///  previewImage :- Function used to show image preview before image get upload in group section
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            previewImage: function (event) {
                var self = this;
                var input = event.target;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        self.dataURItoBlob(e.target.result, "image/jpeg", false);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            },

            /// <summary>
            ///  previewImage :- Function used to show image preview before image get upload in sub group section
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            previewImageSubgroup: function (event) {
                var self = this;
                var input = event.target;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        self.dataURItoBlob(e.target.result, "image/jpeg", true);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            },

            /// <summary>
            ///  dataURItoBlob :- Function used to convert data URI to file
            /// </summary>
            /// <param name="dataURI"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            dataURItoBlob(dataURI, type, issubgroup) {
                // convert base64 to raw binary data held in a string
                var byteString = atob(dataURI.split(',')[1]);

                // separate out the mime component
                var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

                // write the bytes of the string to an ArrayBuffer
                var ab = new ArrayBuffer(byteString.length);
                var ia = new Uint8Array(ab);
                for (var i = 0; i < byteString.length; i++) {
                    ia[i] = byteString.charCodeAt(i);
                }

                // write the ArrayBuffer to a blob, and you're done
                var bb = new Blob([ab], { type: type });
                if (issubgroup) {
                    this.profilesubgroupImage = URL.createObjectURL(bb);
                } else {
                    this.profileeditImage = URL.createObjectURL(bb);
                }
            }
        }
    };
</script>
