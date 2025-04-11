<template>
  <v-card min-height="70vh">
          <v-row>
              <v-col cols="12" md="12" sm="12" xs="12">
                  <v-data-table :headers="groupsTableHeaders"
                                :items="groupsTableItems"
                                :search="searchgroups"
                                v-model="selectedGroups"
                                show-select=""
                                fixed-header
                                height="60vh"
                                item-key="GroupId"
                                class="elevation-0"
                                :footer-props="{
                                itemsPerPageOptions:[10,50,100,-1]
                                }"
                                :items-per-page="10">
                      <!--Group list top-->
                      <template v-slot:top>
                          <v-row class="px-4">
                              <v-col cols="12" xs="12" sm="12" md="3">
                                  <h4 class="page-head">
                                      <v-avatar color="white" size="20" class="elevation-1 mr-1 mb-1">
                                          <v-icon size="15" color="primary">mdi-account-group-outline</v-icon>
                                      </v-avatar>
                                      User Groups
                                  </h4>
                              </v-col>
                              <v-col cols="12" xs="12" sm="12" md="9">
                                  <v-row>
                                      <v-spacer></v-spacer>
                                      <!--Add userGroup button-->
                                      <v-menu offset-y transition="scroll-y-transition" close-on-content-click internal-activator v-bind:disabled="!screenAccess">
                                          <template v-slot:activator="{ on }">
                                              <v-btn height="39" v-on="on" color="success-btn" dark class="elevation-0 mr-4">
                                                  Add Group
                                              </v-btn>
                                          </template>
                                          <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                              <v-list-item id="AutoTest_U41" link @click.stop="AddGroupDialog=true">
                                                  <v-list-item-icon>
                                                      <v-icon>mdi-plus-circle-outline</v-icon>
                                                  </v-list-item-icon>
                                                  <v-list-item-content>Add Group</v-list-item-content>
                                              </v-list-item>
                                          </v-list>
                                      </v-menu>

                                      <!--Add Group dialog-->
                                      <v-dialog v-model="AddGroupDialog" width="800" persistent>
                                          <v-card>
                                              <v-card-title primary-title class="page-head pb-0">
                                                  <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                      <v-icon size="20" color="primary">mdi-account-group-outline</v-icon>
                                                  </v-avatar>
                                                  New User Group
                                                  <v-spacer></v-spacer>
                                                  <v-btn id="AutoTest_U42" text @click="AddGroupDialog=false,$refs.Form.resetValidation()">
                                                      <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                  </v-btn>
                                              </v-card-title>
                                              <v-card-text class="pt-0 pb-1">
                                                  <v-container fluid class="pt-0">
                                                      <v-row no-gutters>
                                                          <v-col cols="12" md="12" class="pt-0">
                                                              <v-stepper v-model="AddGroupRegSteps" class="elevation-0">
                                                                  <v-row :justify="justifyCenter">
                                                                      <v-col md="7">
                                                                          <v-stepper-header class="elevation-0">
                                                                              <v-stepper-step :complete="AddGroupRegSteps > 1" step="1" color="inherit">Basic Details</v-stepper-step>
                                                                              <v-divider></v-divider>
                                                                              <v-stepper-step :complete="AddGroupRegSteps > 2" step="2" color="inherit">Add Members</v-stepper-step>
                                                                          </v-stepper-header>
                                                                      </v-col>
                                                                  </v-row>
                                                                  <v-stepper-items>
                                                                      <v-stepper-content step="1" class="pt-0">
                                                                          <v-form ref="Form"
                                                                                  v-model="valid"
                                                                                  @submit.prevent="fnNextButton">
                                                                              <v-container fluid class="py-0">
                                                                                  <v-row no-gutters>
                                                                                      <v-col cols="12" md="12" class="pt-0">
                                                                                          <h4 class="text-center heading-3">Step1 : Please enter the basic details of the new user group</h4>
                                                                                      </v-col>

                                                                                      <v-col cols="12" md="12" class="text-center">
                                                                                          <v-avatar size="75" class="elevation-1 mt-2">
                                                                                              <v-img v-bind:src="profileeditImage" height="75" width="75" v-if="profileeditImage.length>0"></v-img>
                                                                                              <v-icon color="grey" size="60" v-else>mdi-account-group</v-icon>
                                                                                              <div class="dp-edit-btn" style="top:30px">
                                                                                                  <input class="upload" type="file" id="file" hide-details accept="image/*" @change="previewImage" />
                                                                                              </div>
                                                                                          </v-avatar>
                                                                                      </v-col>
                                                                                      <v-col cols="12" xs="12" sm="12" md="12">
                                                                                          <v-row :justify="justifyCenter" class="py-2 my-0">
                                                                                              <v-col md="6" class="py-0 my-0">
                                                                                                  <v-text-field id="groupnametext" outlined dense label="User Group Name" type="text" v-model="AddGroupInput.GroupName" :rules="alphanumericRules">
                                                                                                      <template v-if="AddGroupInput.GroupName.length==0" v-slot:append>
                                                                                                          <span class="mandatory-star">*</span>
                                                                                                      </template>
                                                                                                  </v-text-field>
                                                                                                  <v-textarea rows="2" outlined dense hide-details label="Description" type="text" v-model="AddGroupInput.GroupDescription">
                                                                                                  </v-textarea>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                      </v-col>
                                                                                      <v-col cols="12" md="12" class="pt-0"> 
                                                                                          <h6 class="text-center heading-4">A User Group should be created to use for project, so choose your members accordingly</h6>
                                                                                      </v-col>
                                                                                      <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-4">
                                                                                          <!--<Secondary-Button id="AutoTest_U43" class="btn-122x36 mr-4" title="Next" @click.native="AddGroupRegSteps = 2" v-bind:disabled="AddGroupInput.GroupName.length==0"></Secondary-Button>-->
                                                                                          <Secondary-Button id="AutoTest_U43" class="btn-122x36 mr-4" title="Next" @click.native="fnNextButton()"></Secondary-Button>
                                                                                          <Default-Button-Outlined id="AutoTest_U44" class="btn-122x36" title="Cancel" @click.native="AddGroupDialog=false;selectedUsers=[];NotifyMessage=null,$refs.Form.resetValidation()"></Default-Button-Outlined>
                                                                                      </v-col>
                                                                                  </v-row>
                                                                              </v-container>
                                                                          </v-form>
                                                                      </v-stepper-content>

                                                                      <v-stepper-content step="2" class="py-0">
                                                                          <v-container fluid class="py-0">
                                                                              <v-row no-gutters>
                                                                                  <v-col cols="12" md="12" class="pt-0 mb-4">
                                                                                      <h4 class="text-center heading-3">Step 2: Add your users to one or more groups from below</h4>
                                                                                  </v-col>
                                                                                  <v-col cols="12" md="12" class="py-0 mb-2">
                                                                                      <v-text-field v-model="searchuser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details>
                                                                                      </v-text-field>
                                                                                  </v-col>
                                                                                  <v-col v-if="selectedUsers.length>0" md="12" class="py-0">
                                                                                      <div class="chip-container mb-2">
                                                                                          <v-chip-group multiple
                                                                                                        column
                                                                                                        active-class="primary--text">
                                                                                              <v-chip id="AutoTest_U45" close v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                                                      @click:close="$delete(selectedUsers, key)" small>
                                                                                                  <v-avatar size="12" left :color="`${getcolor(item.Name)}`">
                                                                                                      <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                                      <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                  </v-avatar>
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
                                                                                                    hide-default-footer
                                                                                                    class="elevation-0"
                                                                                                    v-model="selectedUsers"
                                                                                                    fixed-header
                                                                                                    height="27vh">
                                                                                          <template v-slot:item.Name="{item}">
                                                                                              <div class="pa-0 text-limit" style=" width:100px">{{item.Name}}</div>
                                                                                          </template>
                                                                                      </v-data-table>
                                                                                  </v-col>
                                                                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                                      <v-row v-if="selectedUsers.length">
                                                                                          <v-col cols="12" md="8" class="py-0">
                                                                                              <v-list class="py-0">
                                                                                                  <v-list-item>
                                                                                                      <v-list-item-icon>
                                                                                                          <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                                                              <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                                              <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                          </v-avatar>
                                                                                                          <v-avatar v-if="selectedUsers.length>=4" size="25" class="team-count">
                                                                                                              +{{selectedUsers.length-3}}
                                                                                                          </v-avatar>
                                                                                                      </v-list-item-icon>
                                                                                                      <v-list-item-content>
                                                                                                          {{NotifyMessage}} the group {{ AddGroupInput.GroupName }}
                                                                                                      </v-list-item-content>
                                                                                                  </v-list-item>
                                                                                              </v-list>
                                                                                          </v-col>
                                                                                          <v-col cols="12" md="4" class="py-0">
                                                                                              <!-- <v-checkbox color="success-check-box" dense input-value="true" v-model="NotifyCheckbox" label="Notify user" class="float-right"></v-checkbox> -->
                                                                                          </v-col>
                                                                                      </v-row>
                                                                                  </v-col>
                                                                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-right mb-4">
                                                                                      <v-btn id="AutoTest_U46" text small class="mr-4" @click="fnAddGroup(false)">Skip for now ></v-btn>
                                                                                      <Secondary-Button id="AutoTest_U47" class="btn-122x36 mr-4" title="Confirm" @click.native="fnAddGroup(true)" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                                                      <Default-Button-Outlined id="AutoTest_U48" class="btn-122x36" title="Previous" @click.native="AddGroupRegSteps = 1"></Default-Button-Outlined>
                                                                                  </v-col>
                                                                              </v-row>
                                                                          </v-container>
                                                                      </v-stepper-content>

                                                                      <v-stepper-content step="3" class="pa-0">
                                                                          <v-container fluid class="pt-0">
                                                                              <v-row no-gutters>
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
                                                                                          Your group has been created successfully.<br />Bask in your glory !
                                                                                      </p>
                                                                                  </v-col>
                                                                                  <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                      <v-btn id="AutoTest_U49" color="secondary" class="mr-2 elevation-0" @click="AddGroupDialog=false,AddGroupRegSteps=1">Done</v-btn>
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

                                      <!--Bulk Action still pending from Melvin just added same as user for now-->
                                      <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                                          <template v-slot:activator="{ on }">
                                              <v-btn height="39" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-4">
                                                  Bulk Actions
                                                  <v-icon>
                                                      mdi-chevron-down
                                                  </v-icon>
                                              </v-btn>
                                          </template>
                                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                              <v-list-item id="AutoTest_U50" @click.stop="fnBulkAddUserClick()">
                                                  <v-list-item-icon>
                                                      <v-icon>mdi-plus-circle-outline</v-icon>
                                                  </v-list-item-icon>
                                                  <v-list-item-content>
                                                      Add User
                                                  </v-list-item-content>
                                              </v-list-item>
                                          </v-list>

                                          <!--Bulk Add Users Dialog-->
                                          <v-dialog v-model="BulkAddUserDialog" width="800" persistent>
                                              <v-card>
                                                  <v-card-title primary-title class="page-head">
                                                      <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                          <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                                      </v-avatar>
                                                      Bulk Add Users
                                                      <v-spacer></v-spacer>
                                                      <v-btn id="AutoTest_U51" text @click="BulkAddUserDialog=false">
                                                          <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                      </v-btn>
                                                  </v-card-title>
                                                  <v-card-text class="pt-0">
                                                      <v-container fluid class="py-0">
                                                          <v-row>
                                                              <v-col cols="12" md="12" class="pt-0">
                                                                  <h4 class="text-left heading-3">
                                                                      <v-avatar size="20" class="elevation-1 mr-1">
                                                                          <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                                      </v-avatar>
                                                                      Add multiple users to the selected user groups
                                                                  </h4>
                                                              </v-col>
                                                          </v-row>
                                                      </v-container>
                                                  </v-card-text>
                                                  <v-card-text class="pt-0">
                                                      <v-container fluid="" class="pt-0">
                                                          <v-row no-gutters>
                                                              <v-col cols="12" md="12" class="pt-0">
                                                                  <v-text-field v-model="searchuser" outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details class="mb-2">
                                                                  </v-text-field>
                                                              </v-col>
                                                              <v-col v-if="selectedUsers.length>0" md="12">
                                                                  <div class="chip-container mb-2">
                                                                      <v-chip-group multiple
                                                                                    column
                                                                                    active-class="primary--text">
                                                                          <v-chip id="AutoTest_U52" close v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                                  @click:close="$delete(selectedUsers, key)" small>
                                                                              <v-avatar size="12" left light :color="`${getcolor(item.Name)}`">
                                                                                  <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                  <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                              </v-avatar>
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
                                                                                height="25vh"
                                                                                v-model="selectedUsers">

                                                                      <template v-slot:item.Name="{item}">
                                                                          <v-list class="py-0 table-v-list">
                                                                              <v-list-item class="my-0 px-0">
                                                                                  <v-list-item-icon>
                                                                                      <v-avatar size="27" light :color="`${getcolor(item.Name)}`">
                                                                                          <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
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
                                                              <v-col v-if="selectedUsers.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                  <v-row>
                                                                      <v-col v-if="selectedUsers.length">
                                                                          <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                              <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                              <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                          </v-avatar>
                                                                          <v-avatar v-if="selectedUsers.length>=4" size="25" class="team-count">
                                                                              +{{selectedUsers.length-3}}
                                                                          </v-avatar>
                                                                          {{NotifyMessage}} the selected user groups
                                                                      </v-col>
                                                                      <v-col>
                                                                          <!-- <v-checkbox color="success-check-box" class="float-right" input-value="true" v-model="NotifyCheckbox" label="Notify user"></v-checkbox> -->
                                                                      </v-col>
                                                                  </v-row>
                                                              </v-col>
                                                              <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                  <Secondary-Button id="AutoTest_U53" class="btn-122x36 mr-4" title="Add" @click.native="fnBulkAddUsersToGroups()" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                                  <Default-Button-Outlined id="AutoTest_U54" class="btn-122x36" title="Cancel" @click.native="BulkAddUserDialog = false;selectedUsers=[];NotifyMessage=null"></Default-Button-Outlined>
                                                              </v-col>
                                                          </v-row>
                                                      </v-container>
                                                  </v-card-text>
                                              </v-card>
                                          </v-dialog>
                                      </v-menu>

                                      <!--Search-->
                                      <v-text-field v-model="searchgroups" label="Search" class="table-search mr-4"
                                                    dense hide-details prepend-inner-icon="mdi-magnify"
                                                    outlined></v-text-field>
                                  </v-row>
                              </v-col>
                          </v-row>
                      </template>

                      <!--Group list column Name-->
                      <template v-slot:item.GroupName="{ item }">
                          <router-link class="text--default" link :to="{ name: 'usergroupindividual', query: { WsID: WorkspaceID, Access : RWAccess, GroupId: item.GroupId } }">
                              <!--<v-avatar size="30">
                <v-icon size="15" color="primary">mdi-account-group</v-icon>
            </v-avatar>{{ item.GroupName }}-->

                              <v-list class="py-0 table-v-list">
                                  <v-list-item class="my-0 px-0">
                                      <div class="pa-0 text-limit" style="width:150px">
                                          <v-avatar size="30" class="elevation-1" light :color="`${getcolor(item.GroupName)}`">
                                              <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                              <span class="white--text" v-else>{{item.GroupName == null ? "" : item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                          </v-avatar>
                                          {{ item.GroupName }}
                                      </div>
                                  </v-list-item>
                              </v-list>

                          </router-link>
                      </template>

                      <!--Group list column State-->
                      <template v-slot:item.State="{ item }">
                          <div class="pa-0 text-limit" style="width:115px">
                              <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" @change="fnGroupStateClick(item)"
                                        v-bind:disabled="!screenAccess">
                              </v-switch>
                          </div>
                      </template>
                      <template v-slot:item.UsersCount="{item}">
                          <div class="pa-0 text-limit" style="width:120px">{{item.UsersCount}}</div>
                      </template>
                      <template v-slot:item.ProjectsCount="{item}">
                          <div class="pa-0 text-limit" style="width:130px">{{item.ProjectsCount}}</div>
                      </template>
                      <template v-slot:item.SubGroupsCount="{item}">
                          <div class="pa-0 text-limit" style="width:130px">{{item.SubGroupsCount}}</div>
                      </template>

                      <!--User Group Actions-->
                      <template v-slot:item.usergroupActions="{item}">
                          <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                              <template v-slot:activator="{ on }">
                                  <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                      <v-icon>
                                          mdi-dots-vertical
                                      </v-icon>
                                  </v-btn>
                              </template>
                              <v-list color="white" light width="auto" class="action-button-list">
                                  <v-list-item id="AutoTest_U55" @click.stop="fnEditGroupClick(item)">
                                      <v-list-item-icon>
                                          <v-icon>mdi-pencil-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Edit
                                      </v-list-item-content>
                                  </v-list-item>
                                  <v-list-item id="AutoTest_U56" link @click.stop="fnAddUserClick(item)">
                                      <v-list-item-icon>
                                          <v-icon>mdi-plus-circle-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Add User
                                      </v-list-item-content>
                                  </v-list-item>
                                  <v-list-item id="AutoTest_U57" link="" @click.stop="fnDeleteGroupClick(item)">
                                      <v-list-item-icon>
                                          <v-icon>mdi-trash-can-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Remove
                                      </v-list-item-content>
                                  </v-list-item>
                              </v-list>

                              <!--Edit Group Dialog-->
                              <v-dialog v-model="EditGroupDialog" width="700" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head">
                                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                              <v-icon size="20" color="primary">mdi-account-edit-outline</v-icon>
                                          </v-avatar>
                                          Edit Group Details
                                          <v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U58" text @click="EditGroupDialog=false">
                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                          </v-btn>
                                      </v-card-title>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="pt-0">
                                              <v-row>
                                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                      <v-avatar size="75" class="elevation-1 mt-2" light v-bind:color="RandomColor">
                                                          <v-img v-bind:src="profileeditImage" height="75" width="75" v-if="profileeditImage.length>0"></v-img>
                                                          <span class="headline white--text" v-else>  {{EditGroupInput.GroupName == null ? "" : EditGroupInput.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                          <div class="dp-edit-btn" style="top:30px">
                                                              <input class="upload" type="file" id="fileedit" hide-details accept="image/*" @change="previewImage" />
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
                                                      <Secondary-Button id="AutoTest_U59" class="btn-122x36 mr-4" title="Save" @click.native="fnEditGroupSave()"></Secondary-Button>
                                                      <Default-Button-Outlined id="AutoTest_U60" class="btn-122x36" title="Cancel" @click.native="fnEditGroupCancelClick()"></Default-Button-Outlined>

                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>

                              <!--Add Users Dialog-->
                              <v-dialog v-model="AddUserDialog" width="800" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head">
                                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 ml-2">
                                              <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                          </v-avatar>
                                          Add Users
                                          <v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U61" text="" @click="AddUserDialog=false">
                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                          </v-btn>
                                      </v-card-title>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="py-0 px-0">
                                              <v-row no-gutters>
                                                  <v-col cols="12" md="12" class="py-0  ml-2">
                                                      <h4 class="text-left heading-3">
                                                          <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                          Add Users to a user group
                                                      </h4>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="py-0">
                                              <v-row no-gutters>
                                                  <v-col cols="12" md="12" class="pt-0 pb-2">
                                                      <v-text-field v-model="searchuser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details="">
                                                      </v-text-field>
                                                  </v-col>
                                                  <v-col v-if="selectedUsers.length>0" md="12">
                                                      <div class="chip-container mb-2">
                                                          <v-chip-group multiple
                                                                        column
                                                                        active-class="primary--text">
                                                              <v-chip id="AutoTest_U62" close v-for="(item,key) in selectedUsers" v-bind:key="item.ID"
                                                                      @click:close="$delete(selectedUsers, key)" small>
                                                                  <v-avatar size="12" left light :color="`${getcolor(item.Name)}`">
                                                                      <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                      <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                  </v-avatar>
                                                                  {{ item.Name }}
                                                              </v-chip>
                                                          </v-chip-group>
                                                      </div>
                                                  </v-col>
                                                  <v-col cols="12" xs="12" sm="12" md="12">
                                                      <!--User List with checkbox add here-->
                                                      <v-data-table :headers="userTableHeaders"
                                                                    :items="AddUserItems"
                                                                    :search="searchuser"
                                                                    show-select=""
                                                                    item-key="ID"
                                                                    class="elevation-0"
                                                                    fixed-header
                                                                    height="25vh"
                                                                    v-model="selectedUsers">

                                                          <template v-slot:item.Name="{item}">
                                                              <v-list class="py-0 table-v-list">
                                                                  <v-list-item class="my-0 px-0">
                                                                      <v-list-item-icon>
                                                                          <v-avatar size="27" light :color="`${getcolor(item.Name)}`">
                                                                              <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
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
                                                  <v-col v-if="selectedUsers.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                      <v-row>
                                                          <v-col v-if="selectedUsers.length">
                                                              <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                  <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                  <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                              </v-avatar>
                                                              <v-avatar v-if="selectedUsers.length>=4" size="25" class="team-count">
                                                                  +{{selectedUsers.length-3}}
                                                              </v-avatar>
                                                              {{NotifyMessage}} the group {{ AddGroupInput.GroupName }}
                                                          </v-col>
                                                          <v-col>
                                                              <!-- <v-checkbox color="success-check-box" input-value="true" class="float-right" v-model="NotifyCheckbox" label="Notify user"></v-checkbox> -->
                                                          </v-col>
                                                      </v-row>
                                                  </v-col>
                                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                      <Secondary-Button id="AutoTest_U63" class="btn-122x36 mr-4" title="Add" @click.native="fnAddGroup(true)" v-bind:disabled="selectedUsers.length==0"></Secondary-Button>
                                                      <Default-Button-Outlined id="AutoTest_U64" class="btn-122x36" title="Cancel" @click.native="AddUserDialog = false;selectedUsers=[];NotifyMessage=null"></Default-Button-Outlined>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>

                              <!--Delete Group Dialog-->
                              <v-dialog v-model="DeleteGroupDialog" width="800" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head">
                                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                              <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                          </v-avatar>
                                          Delete User Group
                                          <v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U65" text @click="DeleteGroupDialog=false">
                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                          </v-btn>
                                      </v-card-title>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="pt-0">
                                              <v-row>
                                                  <v-col cols="12" md="12">
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
                                                  <div v-if="projectsTableItems.length" style="width:100%">
                                                      <v-col cols="12" md="12" class="pt-0 mb-2 text-left">
                                                          <v-list class="base-border-radius warning-div pa-0">
                                                              <v-list-item>
                                                                  <v-list-item-icon class="mt-2">
                                                                      <v-avatar size="27" color="#F2C64D" class="elevation-0">
                                                                          <v-icon size="20" color="#fff">mdi-exclamation</v-icon>
                                                                      </v-avatar>
                                                                  </v-list-item-icon>
                                                                  <v-list-item-content class="pb-0">
                                                                      <p class="pa-0">
                                                                          <span class="name">
                                                                              <v-avatar size="20" color="#fff" class="elevation-0">
                                                                                  <v-icon size="12" color="grey">mdi-account-group-outline</v-icon>
                                                                              </v-avatar>
                                                                              {{ item.GroupName }}
                                                                          </span>
                                                                          <span class="message">
                                                                              is part of {{ projectsTableItems.length }} projects.
                                                                              By removing this group all the active projects will become inactive.
                                                                          </span>
                                                                      </p>
                                                                  </v-list-item-content>
                                                              </v-list-item>
                                                          </v-list>
                                                      </v-col>
                                                      <v-col cols="12" xs="12" sm="12" md="12">
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
                                                  </div>
                                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                      <Secondary-Button id="AutoTest_U66" class="btn-122x36 mr-4" title="Delete" @click.native="fnDeleteGroup(item)"></Secondary-Button>
                                                      <Default-Button-Outlined id="AutoTest_U67" class="btn-122x36" title="Cancel" @click.native="DeleteGroupDialog=false"></Default-Button-Outlined>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>
                          </v-menu>
                      </template>
                      <!--if no data-->
                      <template v-slot:no-data>
                          <v-row class="justify-center" v-if="groupsTableItems.length == 0">
                              <v-col class="text-center" md="12">
                                  <p class="mt-12 pt-12">
                                      <img src="..\..\..\src\assets\EmptyPlaceholders\Usergroup.svg" height="150" width="150" />
                                  </p>
                                  <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                              </v-col>
                          </v-row>
                      </template>
                  </v-data-table>
                  
              </v-col>
              <!--Toggle user group -->
              <v-dialog v-model="GroupStateToggleDialog" width="800">
                  <v-card>
                      <v-card-title primary-title class="page-head">
                          <v-avatar color="white" size="25" class="elevation-1 mr-2">
                              <v-icon size="20" color="primary">mdi-electric-switch</v-icon>
                          </v-avatar>
                          Change User Group Status
                          <v-spacer></v-spacer>
                          <v-btn id="AutoTest_U68" text="" @click="GroupStateToggleDialog=false, fnGetUserGroupsList()">
                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                          </v-btn>
                      </v-card-title>
                      <v-card-text class="pt-0">
                          <v-container fluid class="pt-0">
                              <v-row>
                                  <v-col cols="12" md="12">
                                      <h4 class="text-center heading-3">
                                          Are you sure you want to change user group status ?
                                      </h4>
                                  </v-col>
                              </v-row>
                          </v-container>
                      </v-card-text>
                      <v-card-text>
                          <v-container fluid class="pt-0">
                              <v-row>
                                  <div v-if="projectsTableItems.length" style="width:100%">
                                      <v-col cols="12" md="12" class="pt-0 mb-2 text-left">
                                          <v-list class="base-border-radius warning-div pa-0">
                                              <v-list-item>
                                                  <v-list-item-icon class="mt-2">
                                                      <v-avatar size="27" color="#F2C64D" class="elevation-0">
                                                          <v-icon size="20" color="#fff">mdi-exclamation</v-icon>
                                                      </v-avatar>
                                                  </v-list-item-icon>
                                                  <v-list-item-content class="pb-0">
                                                      <p class="pa-0">
                                                          <span class="name">
                                                              <v-avatar size="20" color="#fff" class="elevation-0">
                                                                  <v-icon size="12" color="grey">mdi-account-group-outline</v-icon>
                                                              </v-avatar>
                                                              {{ GroupStateToggleInput.GroupName }}
                                                          </span>
                                                          <span class="message">
                                                              is part of {{ projectsTableItems.length }} projects.
                                                              By removing this group all the active projects will become inactive.
                                                          </span>
                                                      </p>
                                                  </v-list-item-content>
                                              </v-list-item>
                                          </v-list>
                                      </v-col>
                                      <v-col cols="12" xs="12" sm="12" md="12">
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
                                  </div>
                                  <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                      <Secondary-Button id="AutoTest_U69" class="btn-122x36 mr-4" title="Confirm" @click.native="fnGroupChangeState(item)"></Secondary-Button>
                                      <Default-Button-Outlined id="AutoTest_U70" class="btn-122x36" title="Cancel" @click.native="GroupStateToggleDialog=false,fnGetUserGroupsList()"></Default-Button-Outlined>
                                  </v-col>
                              </v-row>

                          </v-container>
                      </v-card-text>
                  </v-card>
              </v-dialog>
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
  </v-card>
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
                valid: true,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg:"",
                //groups list section
                groupsTableHeaders: [
                    { text: 'Name', align: 'left', value: 'GroupName', },
                    { text: 'No. of Users', value: 'UsersCount' },
                    { text: 'No. of Projects', value: 'ProjectsCount' },
                    { text: 'No. of Sub Groups', value: 'SubGroupsCount' },
                    { text: 'State', value: 'State' },
                    { text: 'Actions', value: 'usergroupActions',align:"center", sortable: false },
                ],
                groupsTableItems: [],
                selectedGroups: [],
                searchgroups: '',
                //Add Group
                AddGroupDialog: false,
                AddGroupRegSteps: 0,
                AddGroupInput: { GroupId: 0, GroupName: "", GroupDescription: "" },
                NotifyCheckbox: true,
                //Bulk Add User
                BulkAddUserDialog: false,
                //Actions Edit,Add User,Delete
                EditGroupInput: { GroupId: 0, GroupName: "", GroupDescription: "" },
                EditGroupDialog: false,
                AddUserDialog: false,
                AddUserItems: [],
                DeleteGroupDialog: false,
                //Group State Toggle
                GroupStateToggleDialog: false,
                GroupStateToggleInput: { GroupId: 0, GroupName: "", IsActive: 0 },
                
               //users table used in Add Group, actions Add User, Bulk Add User
                userTableHeaders: [
                    { text: 'Name', align: 'left', value: 'Name', },
                    { text: 'User Role', value: 'userRole' },
                    { text: 'Project Count', value: 'userProjectCount' },
                ],
                userTableItems: [],
                selectedUsers: [],
                searchuser: '',
                //projects table used in Delete Group dialog    
                projectsTableHeaders: [
                    {   text: "Project Name",
                        align: "left",
                        value: "ProjectName",
                        icon: "mdi-account-multiple"
                    },
                    { text: "Status", value: "Status" }
                ],
                projectsTableItems: [],
                //searchprojects: "",
                ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "" },

                //used with Notify checkbix
                NotifyMessage: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                userimage: "",
                RandomColor: "",
                uploadedImage: "",
                profileeditImage: "",
                alphanumericRules: [
                    v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name',
                    (value) => !!value || "Required field.",
                ],
                screenAccess: 0,
                RWAccess: 0,
            }
        },
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await this.fnGetUserGroupsList();
            await this.fnGetUsersList();
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

            /// <summary>
            ///  fnNextButton :- Function used to move to the next step for new group creation
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnNextButton() {
                if (this.$refs.Form.validate() === true) {
                    this.AddGroupRegSteps = 2
                }
            },

            /// <summary>
            ///  fnGetUserGroupsList :- Function used to fetch user group details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUserGroupsList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);//sessionStorage.getItem('WorkspaceID'));
                var url = window.SERVER_URL + "/UserGroups/GetUserGroupsList";
                var result = await group.PostData(url,data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.groupsTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnGetUsersList :- Function used to fetch user details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);//sessionStorage.getItem('WorkspaceID'));
                var url = window.SERVER_URL + "/UserAccount/GetAllUserDetails";
                var result = await group.PostData(url,data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.userTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnAddGroup :- Function used to add new user group
            /// </summary>
            /// <param name="isUserList"></param>
            /// <returns> </returns>
            async fnAddGroup(isUserList) {
                if (this.AddGroupInput.GroupId == 0 && this.AddGroupInput.GroupName == "") {
                    this.Msg = "Group Name is required";
                    this.infoSnackbarMsg = true;
                    return;
                }
                if (isUserList == false) {
                    this.selectedUsers = [];
                } else {
                    if (this.selectedUsers.length == 0) {
                        this.Msg = "No Users selected.Please select the users to add to group " + this.AddGroupInput.GroupName;
                        this.infoSnackbarMsg = true;                        
                        return;
                    }
                }
                var url = window.SERVER_URL + "/UserGroups/AddUserGroup";
                var data = new FormData();
                data.append("GroupId", this.AddGroupInput.GroupId);
                data.append("GroupName", this.AddGroupInput.GroupName);
                data.append("Description", this.AddGroupInput.GroupDescription);
                data.append("UserList", JSON.stringify(this.selectedUsers));
                data.append("Notify", this.NotifyCheckbox);
                data.append("WorkspaceId", this.WorkspaceID);//sessionStorage.getItem('WorkspaceID'));
                data.append("location", "usergroup");

                var input = document.getElementById('file');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("UserGroupImage", input.files[i]);
                    }
                }

                var result = await group.PostData(url,data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data == this.AddGroupInput.GroupId) {
                   
                    this.Msg = "Successfully added users to the group " + this.AddGroupInput.GroupName;
                    this.successSnackbarMsg = true;
                }
                else if (result.data && isUserList == false) {
                    this.Msg = ("Successfully added the group " + this.AddGroupInput.GroupName);
                    this.successSnackbarMsg = true;
                }
                else if (result.data && isUserList == true) {
                    this.Msg = ("Successfully added the group " + this.AddGroupInput.GroupName + " and users");
                    this.successSnackbarMsg = true;
                }
                else if (result.data == 0) {
                    this.Msg = ("Failed to add the group " + this.AddGroupInput.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.AddGroupInput.GroupName = "";
                this.AddGroupInput.GroupDescription = "";
                this.selectedUsers = [];
                this.AddGroupRegSteps = 3;
                this.searchuser = "";
                this.fnGetUserGroupsList();
                this.AddUserDialog = false;
                this.NotifyCheckbox = true;
                this.$refs.Form.resetValidation()
            },

            /// <summary>
            ///  fnDeleteGroup :- Function used to delete user group
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnDeleteGroup(item) {
                var url = window.SERVER_URL + "/UserGroups/DeleteUserGroup";
                var data = new FormData();
                data.append("GroupId", item.GroupId);
                var result = await group.PostData(url,data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully deleted " + item.GroupName);
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to delete " + item.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.fnGetUserGroupsList();
                this.DeleteGroupDialog = false;
            },

            /// <summary>
            ///  fnGroupStateClick :- Function used to get project list when click on switch button in group state
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnGroupStateClick(item) {
                this.GroupStateToggleDialog = true;
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectsTableItems = result.data;
                }
                this.GroupStateToggleInput.GroupId = item.GroupId;
                this.GroupStateToggleInput.GroupName = item.GroupName;
                this.GroupStateToggleInput.IsActive = item.IsActive;
            },

            /// <summary>
            ///  fnGroupChangeState :- Function used to change user group state
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGroupChangeState() {
                var url = window.SERVER_URL + "/UserGroups/ChangeGroupState";
                var data = new FormData();
                data.append("GroupId", this.GroupStateToggleInput.GroupId);
                data.append("IsActive", this.GroupStateToggleInput.IsActive);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully changed the state " + this.GroupStateToggleInput.GroupName);
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to change the state " + this.GroupStateToggleInput.GroupName);
                    this.errorSnackbarMsg = true;
                }
                this.fnGetUserGroupsList();
                this.GroupStateToggleDialog = false;
                this.GroupStateToggleInput.GroupId = 0;
                this.GroupStateToggleInput.GroupName = "";
                this.GroupStateToggleInput.IsActive = 0;
            },

            /// <summary>
            ///  fnEditGroupClick :- Function used to open edit group dialog when click on edit button
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnEditGroupClick(item) {
                this.EditGroupDialog = true;
                this.EditGroupInput.GroupId = item.GroupId;
                this.EditGroupInput.GroupName = item.GroupName;
                this.EditGroupInput.GroupDescription = item.Description;

                if (item.UserGroupImage != null &&
                    item.UserGroupImage != "" &&
                    item.UserGroupImage != undefined) {
                    this.profileeditImage = window.SERVER_URL + "/Upload" + item.UserGroupImage;
                } else {
                    this.profileeditImage = "";
                }

            },

            /// <summary>
            ///  fnEditGroupCancelClick :- Function used to cancel edit group dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditGroupCancelClick() {
                this.EditGroupDialog = false;
                this.EditGroupInput.GroupId = 0;
                this.EditGroupInput.GroupName = "";
                this.EditGroupInput.GroupDescription = "";
                this.profileeditImage = "";
            },

            /// <summary>
            ///  fnEditGroupSave :- Function used to update user group
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

                var input = document.getElementById('fileedit');

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
                this.fnEditGroupCancelClick();
                this.fnGetUserGroupsList();
            },

            /// <summary>
            ///  fnAddUserClick :- Function used to add users to a user group 
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnAddUserClick(item) {
                this.AddUserDialog = true;
                this.AddGroupInput.GroupId = item.GroupId;
                this.AddGroupInput.GroupName = item.GroupName;
                var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AddUserItems = this.userTableItems;
                    for (var i = 0; i < result.data.length; i++) {
                        var oneitem = result.data[i];
                        this.AddUserItems = this.AddUserItems.filter(x => x.ID != oneitem.ID);
                    }
                }
            },

            /// <summary>
            ///  fnDeleteGroupClick :- Function used to get project list when click on delete button
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async fnDeleteGroupClick(item) {
                this.DeleteGroupDialog = true;
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectsTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnBulkAddUserClick :- Function used to open add user dialog/ print a message if group is not selected to add users
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkAddUserClick() {
                if (this.selectedGroups.length > 0) {
                    this.BulkAddUserDialog = true;
                } else {
                    this.Msg = ("Please select the groups");
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnBulkAddUsersToGroups :- Function used to add bulk users to bulk groups 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnBulkAddUsersToGroups() {
                if (this.selectedGroups.length == 0 ) {
                    this.Msg = ("Please select the groups");
                    this.infoSnackbarMsg = true;
                    return;
                }
                if (this.selectedUsers.length == 0) {
                    this.Msg = ("Please select the users");
                    this.infoSnackbarMsg = true;
                    return;
                }
                var url = window.SERVER_URL + "/UserGroups/BulkAddUsersToGroups";
                var data = new FormData();
                data.append("GroupList", JSON.stringify(this.selectedGroups));
                data.append("UserList", JSON.stringify(this.selectedUsers));
                data.append("Notify", this.NotifyCheckbox);

                var result = await group.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg =("Successfully added bulk users to bulk groups ");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg =("Failed to add bulk users to bulk groups ");
                    this.errorSnackbarMsg = true;
                }
                this.BulkAddUserDialog = false;
                this.selectedGroups = [];
                this.selectedUsers = [];
                this.fnGetUserGroupsList();
                this.NotifyCheckbox = true;
            },

            /// <summary>
            ///  previewImage :- Function used to show image preview before image get upload
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            previewImage: function (event) {
                var self = this;
                var input = event.target;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        self.dataURItoBlob(e.target.result, "image/jpeg");
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
            dataURItoBlob(dataURI, type) {
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
                this.profileeditImage = URL.createObjectURL(bb);
            }
        },
        watch: {
            /// <summary>
            ///  selectedUsers :- Function used to notify the users with a message when selecting users from the table
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            selectedUsers: function () {
                this.NotifyMessage = "";
               
                if (this.selectedUsers.length > 2) {
                    this.NotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " and " + (this.selectedUsers.length-2) + " more will be added to ";
                } else if (this.selectedUsers.length == 2) {
                    this.NotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " will be added to ";
                } else if (this.selectedUsers.length > 0){
                    this.NotifyMessage = this.selectedUsers[0].Name + " will be added to ";
                }
            }
        }
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
