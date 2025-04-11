<template>
  <v-card min-height="70vh">
          <v-row>
              <v-col cols="12" md="12" sm="12" xs="12">
                  <v-data-table :headers="userRolesTableheaders"
                                :items="userRolesTableItems"
                                :search="searchuserrole"
                                item-key="RoleId"
                                fixed-header
                                height="60vh"
                                class="elevation-0"
                                :footer-props="{
                                itemsPerPageOptions:[10,50,100,-1]
                                }"
                                :items-per-page="10">
                      <template v-slot:top>
                          <v-row class="px-4">
                              <v-col cols="12" xs="12" sm="12" md="4">
                                  <h3 class="text-left page-head">
                                      <v-avatar color="white" size="20" class="elevation-1  mr-2 mb-1">
                                          <v-icon size="15" color="primary">mdi-account-tie-outline</v-icon>
                                      </v-avatar>
                                      User Roles
                                  </h3>
                              </v-col>
                              <v-col cols="12" xs="12" sm="12" md="8">
                                  <v-row>
                                      <v-spacer></v-spacer>
                                      <!--Add userRole button-->
                                      <!--<v-menu :offset-y=true v-model="AddRoleMenu" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">-->
                                          <v-menu :offset-y=true v-model="AddRoleMenu"   v-bind:disabled ="!screenAccess">
                                              <template v-slot:activator="{ on }">
                                                  <v-btn height="39" v-on="on" width="160" color="success-btn" dark class="elevation-0 mr-4">
                                                      Add User Role
                                                  </v-btn>
                                              </template>
                                              <!-- Added For List -->
                                              <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                                  <v-list-item id="AutoTest_U149" link @click.stop="NewUserRoleDialog=true" @click="GetAccessTypeList(),GetScreenList(),closeonClick=true,closeOnContentClick =true">
                                                      <v-list-item-icon>
                                                          <v-icon>
                                                              mdi-plus-circle-outline
                                                          </v-icon>
                                                      </v-list-item-icon>
                                                      <v-list-item-content>
                                                          Add User Role
                                                      </v-list-item-content>
                                                  </v-list-item>
                                              </v-list>
                                              <!--Added Ends here -->

                                              <v-dialog v-model="NewUserRoleDialog" width="800" persistent>
                                                  <v-card>
                                                      <v-card-title primary-title class="page-head">
                                                          <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                              <v-icon size="20" color="primary">mdi-account-tie-outline</v-icon>
                                                          </v-avatar>
                                                          New User Role<v-spacer></v-spacer>
                                                          <v-btn id="AutoTest_U150" text @click=" fnCancelSaveUserRole()">
                                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                          </v-btn>
                                                      </v-card-title>
                                                      <v-card-text class="pt-0">
                                                          <v-container fluid class="pt-0">
                                                              <v-row>
                                                                  <v-col cols="12" md="12" class="py-0">
                                                                      <v-stepper v-model="NewUserRoleRegSteps" class="elevation-0">
                                                                          <v-row :justify="justifyCenter" no-gutters>
                                                                              <v-col md="6">
                                                                                  <v-stepper-header class="elevation-0">
                                                                                      <v-stepper-step :complete="NewUserRoleRegSteps > 1" step="1" color="inherit">Basic Details</v-stepper-step>
                                                                                      <v-divider></v-divider>
                                                                                      <v-stepper-step :complete="NewUserRoleRegSteps > 2" step="2" color="inherit">Define Role</v-stepper-step>
                                                                                  </v-stepper-header>
                                                                              </v-col>
                                                                          </v-row>
                                                                          <v-stepper-items>
                                                                              <v-stepper-content step="1" class="pt-0">
                                                                                  <v-form ref="Form"
                                                                                          v-model="valid"
                                                                                          @submit.prevent="fnCheckRoleName">
                                                                                      <v-container fluid class="py-0">
                                                                                          <v-row no-gutters>
                                                                                              <v-col cols="12" md="12" class="pt-0">
                                                                                                  <h4 class="text-center heading-3">Step 1 : Please enter the basic details of the new user role</h4>
                                                                                              </v-col>
                                                                                              <v-col cols="12" md="12" class="pt-0">
                                                                                                  <h4 class="text-center heading-4 mt-4">A User Role should be created to assign to a user<br />so set your role accordingly</h4>
                                                                                              </v-col>
                                                                                              <!-- Adding Avatar Icon-->
                                                                                              <v-col cols="12" md="12" class="text-center pt-2">
                                                                                                  <v-avatar size="75" class="elevation-1 mt-2">
                                                                                                      <v-icon color="grey" size="60">mdi-account-tie-outline</v-icon>
                                                                                                  </v-avatar>
                                                                                              </v-col>
                                                                                              <!-- Adding Ends here -->
                                                                                              <v-col cols="12" xs="12" sm="12" md="12" class="">
                                                                                                  <v-row :justify="justifyCenter" no-gutters class="py-2 my-0">
                                                                                                      <v-col md="6" class="py-0 my-0">
                                                                                                          <v-text-field outlined dense label="User Role Name" type="text" v-model="AddUserRoleScreenInput.RoleName" v-on:keyup="btnStatus" :rules="alphanumericRules">
                                                                                                              <template v-if="AddUserRoleScreenInput.RoleName.length==0" v-slot:append>
                                                                                                                  <span class="mandatory-star">*</span>
                                                                                                              </template>
                                                                                                          </v-text-field>
                                                                                                          <v-textarea rows="3" hide-details outlined dense label="Description" type="text" v-model="AddUserRoleScreenInput.Description">
                                                                                                          </v-textarea>
                                                                                                      </v-col>
                                                                                                  </v-row>
                                                                                              </v-col>
                                                                                              <v-col cols="12" xs="12" sm="12" md="12" class="text-right mt-5 ma-0 py-0">
                                                                                                  <!--<Secondary-Button id="AutoTest_U151" class="btn-122x36 mr-2" title="Next" @click.native="fnCheckRoleName()" v-bind:disabled="btnDisable"></Secondary-Button>-->
                                                                                                  <Secondary-Button id="AutoTest_U151" class="btn-122x36 mr-2" title="Next" @click.native="fnCheckRoleName()"></Secondary-Button>
                                                                                                  <Default-Button-Outlined id="AutoTest_U152" class="btn-122x36" title="Cancel" @click.native="fnCancelSaveUserRole()"></Default-Button-Outlined>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                      </v-container>
                                                                                  </v-form>
                                                                              </v-stepper-content>

                                                                              <v-stepper-content step="2" class="py-0">
                                                                                  <v-container fluid class="py-0">
                                                                                      <v-row no-gutters>
                                                                                          <v-col cols="12" md="12" class="pt-0">
                                                                                              <h4 class="text-center page-sub-head mb-2 mt-0 pt-0">Step 2: Add screens to the user role and define access type</h4>
                                                                                          </v-col>
                                                                                          <v-col cols="12" md="12" class="pt-0 pb-0 py-0">
                                                                                              <v-row justify="center" no-gutters class="py-0">
                                                                                                  <v-col cols="12" md="12" class="py-0">
                                                                                                      <v-text-field v-model="searchscreen" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details>
                                                                                                      </v-text-field>
                                                                                                  </v-col>
                                                                                              </v-row>
                                                                                          </v-col>
                                                                                          <!-- chip Additoin starts here -->
                                                                                          <div class="chip-container" v-if="ScreenList.length>0">
                                                                                              <v-chip-group multiple
                                                                                                            column
                                                                                                            active-class="primary--text">
                                                                                                  <v-chip id="AutoTest_U153" close v-for="item in ScreenList" :key="item.ScreenId"
                                                                                                          class="ma-1" @click:close="remove(item)" small color="#DBF1FF" v-bind:disabled="item.DefaultScreen">
                                                                                                      <v-icon left color="#3374B9" size="15">mdi-network-outline</v-icon> <small>{{ item.ScreenName }}</small>
                                                                                                  </v-chip>

                                                                                              </v-chip-group>
                                                                                          </div>
                                                                                          <!-- chip Addtion ends here-->
                                                                                          <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                                                              <!--Screen List with checkbox add here-->
                                                                                              <v-data-table :headers="roleScreensTableheaders"
                                                                                                            :items="roleScreensTableItems"
                                                                                                            :search="searchscreen"
                                                                                                            show-select=""
                                                                                                            item-key="ScreenName"
                                                                                                            class="elevation-0"
                                                                                                            fixed-header
                                                                                                            hide-default-footer
                                                                                                            height="33vh"
                                                                                                            v-model="ScreenList">
                                                                                                  <template v-slot:item.ScreenName="{item}">
                                                                                                      <div class="pa-0 text-limit" style=" width:100px">{{item.ScreenName}}</div>
                                                                                                  </template>
                                                                                                  <template v-slot:item.AccessId="props">
                                                                                                      <div class="py-0 pt-3 pb-0 mb-0">
                                                                                                          <v-autocomplete dense="" outlined="" class="py-0 " style="width:170px"
                                                                                                                          :items="AccessTypeValues" :item-text="i => i.AccessName" :item-value="i => i.AccessId" v-model="props.item.AccessId"
                                                                                                                          v-bind:disabled="props.item.DefaultScreen===1">
                                                                                                          </v-autocomplete>
                                                                                                      </div>
                                                                                                  </template>
                                                                                                  <template v-slot:item.data-table-select="{ item, isSelected, select }">
                                                                                                      <v-simple-checkbox :value="isSelected"
                                                                                                                         :readonly="item.DefaultScreen"
                                                                                                                         :disabled="item.DefaultScreen"
                                                                                                                         @input="select($event)"></v-simple-checkbox>
                                                                                                  </template>
                                                                                                  <template v-slot:item.ScreenPath="">
                                                                                                      <router-link to="" target="_blank">Proj.jpg</router-link>
                                                                                                  </template>
                                                                                                  <template v-slot:item.screenActions="">
                                                                                                      <v-menu :close-on-content-click="false">
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
                                                                                          <v-col cols="12" xs="12" sm="12" md="12" class="text-right mt-5 py-0 my-0">
                                                                                              <v-btn id="AutoTest_U154" text small @click="fnAddRole(false)">Skip for now ></v-btn>
                                                                                              <Secondary-Button id="AutoTest_U155" class="btn-122x36 mr-2" title="Confirm" @click.native="fnAddRole(true)" v-bind:disabled="ScreenList.length==0"></Secondary-Button>
                                                                                              <Default-Button-Outlined id="AutoTest_U156" class="btn-122x36" title="Previous" @click.native="NewUserRoleRegSteps = 1"></Default-Button-Outlined>
                                                                                          </v-col>
                                                                                      </v-row>
                                                                                  </v-container>
                                                                              </v-stepper-content>

                                                                              <!-- final step starts here -->
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
                                                                                                  Your User Role has been created successfully.<br />Bask in your glory !
                                                                                              </p>
                                                                                          </v-col>
                                                                                          <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                              <Secondary-Button id="AutoTest_U157" class="btn-122x36" title="Done" @click.native="fnClearAfterSave()"></Secondary-Button>
                                                                                          </v-col>
                                                                                      </v-row>
                                                                                  </v-container>
                                                                              </v-stepper-content>
                                                                              <!-- finalizing step ends here -->
                                                                          </v-stepper-items>
                                                                      </v-stepper>
                                                                  </v-col>
                                                              </v-row>
                                                          </v-container>
                                                      </v-card-text>
                                                      <v-divider></v-divider>
                                                  </v-card>
                                              </v-dialog>
                                          </v-menu>

                                          <!--Search-->
                                          <v-text-field v-model=searchuserrole label="Search" class="table-search mr-2"
                                                        hide-details dense prepend-inner-icon="mdi-magnify"
                                                        outlined></v-text-field>
</v-row>
                              </v-col>
                          </v-row>
                      </template>

                      <!--User Role Actions-->
                      <template v-slot:item.userroleActions="{item}">
                          <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition"   v-bind:disabled ="!screenAccess">
                              <template v-slot:activator="{ on }">
                                  <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                      <v-icon>
                                          mdi-dots-vertical
                                      </v-icon>
                                  </v-btn>
                              </template>
                              <v-list color="white" light width="auto" class="action-button-list">
                                  <v-list-item id="AutoTest_U158" link @click="fnEditRoleClick(item)">
                                      <v-list-item-icon>
                                          <v-icon>mdi-pencil-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Edit
                                      </v-list-item-content>
                                  </v-list-item>
                                  <v-list-item id="AutoTest_U159" link @click="GetUsersToAssignRole(item.RoleId,item.RoleName), AddUserDialog = true">
                                      <v-list-item-icon>
                                          <v-icon>mdi-plus-circle-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Assign User
                                      </v-list-item-content>
                                  </v-list-item>
                                  <v-list-item id="AutoTest_U160" link @click="GetRoleBasedUsersList(item.RoleId,item.RoleName), RemoveRoleDialog= true">
                                      <v-list-item-icon>
                                          <v-icon>mdi-trash-can-outline</v-icon>
                                      </v-list-item-icon>
                                      <v-list-item-content>
                                          Remove
                                      </v-list-item-content>
                                  </v-list-item>
                              </v-list>

                              <!--Assign User Dialog-->
                              <v-dialog v-model="AddUserDialog" width="700" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head ml-0">
                                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                              <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                          </v-avatar>
                                          Assign users<v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U161" text="" @click="AddUserDialog = false, UserList = []">
                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                          </v-btn>
                                      </v-card-title>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="pt-0 px-0">
                                              <v-row no-gutters>
                                                  <v-col cols="12" md="12" class="pt-0">
                                                      <v-container fluid class="pt-2 px-0">
                                                          <v-row no-gutters>
                                                              <v-col cols="12" md="12" class="text-right">
                                                                  <h5 class="text-left heading-4 mb-4">
                                                                      <v-icon color="#52B962" class="mr-1">
                                                                          mdi-plus-circle
                                                                      </v-icon>
                                                                      Assign multiple users a user role
                                                                  </h5>
                                                                  <v-text-field v-model="searchuser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify">
                                                                  </v-text-field>
                                                              </v-col>
                                                              <!-- v-chip for Assign roless-->
                                                              <v-col v-if="UserList.length > 0" md="12">
                                                                  <div class="chip-container mb-2">
                                                                      <!-- chip Additoin starts here -->
                                                                      <v-chip-group multiple column active-class="primary--text">
                                                                          <v-chip id="AutoTest_U163" close v-for="item in UserList" :key="item.ID"
                                                                                  class="ma-2" @click:close="RemoveAssginUserChipItems(item)" small color="#DBF1FF" text-color="#DBF1FF">
                                                                              <v-avatar size="12" left light :color="`${getcolor(item.Name)}`">
                                                                                  <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                  <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                              </v-avatar>
                                                                              {{ item.Name }}
                                                                          </v-chip>
                                                                      </v-chip-group>
                                                                      <!-- chip Addtion ends here-->
                                                                  </div>
                                                              </v-col>
                                                              <!--v-chip for Assign roles ends here -->
                                                              <v-col cols="12" xs="12" sm="12" md="12">
                                                                  <!--User List with checkbox add here-->
                                                                  <v-data-table :headers="userTableheaders"
                                                                                :items="userTableItems"
                                                                                :search="searchuser"
                                                                                hide-default-footer
                                                                                show-select
                                                                                item-key="ID"
                                                                                fixed-header
                                                                                height="25vh"
                                                                                class="elevation-0"
                                                                                v-model="UserList">

                                                                      <!-- User Name With Image-->
                                                                      <template v-slot:item.Name="{item}">
                                                                          <!-- ,params: {Name} -->
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
                                                                      <!-- Ends here -->
                                                                  </v-data-table>
                                                              </v-col>

                                                              <v-col v-if="UserList.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                  <v-row>
                                                                      <v-col v-if="UserList.length">
                                                                          <v-avatar size="25" class="team-avatar" v-for="(item,key) in UserList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                              <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                              <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                          </v-avatar>
                                                                          <v-avatar v-if="UserList.length>=4" size="25" class="team-count">
                                                                              +{{UserList.length-3}}
                                                                          </v-avatar>
                                                                          {{AddUserNotifyMsg}}                             
                                                                      </v-col>
                                                                      <v-col cols="12" md="4" class="py-0">
                                                                          <!-- <v-checkbox color="success-check-box" input-value="true" label="Notify user" v-model="NotifyCheckBox" class="float-right"></v-checkbox> -->
                                                                      </v-col>
                                                                  </v-row>
                                                              </v-col>

                                                              <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                  <Secondary-Button id="AutoTest_U164" class="btn-122x36 mr-2" title="Add" @click.native="AssignUsersToRole()" v-bind:disabled="UserList.length==0"></Secondary-Button>
                                                                  <Default-Button-Outlined id="AutoTest_U165" class="btn-122x36" title="Cancel" @click.native="AddUserDialog = false, UserList = []"></Default-Button-Outlined>
                                                              </v-col>
                                                          </v-row>
                                                      </v-container>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>

                              <!-- Remove User role Dialog starts here -->
                              <v-dialog v-model="RemoveRoleDialog" width="700" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head py-4">
                                          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                              <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                          </v-avatar>
                                          Remove User Role
                                          <v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U166" text="" @click="RemoveRoleDialog=false">
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
                                                                  <h4 class="text-center heading-3">Are you sure want to remove this user role ?</h4>
                                                              </v-col>
                                                              <div v-if="!IsHideDeleteMsg">
                                                                  <v-col cols="12" md="12" class="pt-0">
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
                                                                                              <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                                          </v-avatar>
                                                                                          {{RoleName}}
                                                                                      </span>
                                                                                      <span class="message"> : User role has been assigned to {{RoleAssignedUserTableItems.length}} active users by removing this role all these users will be unasssigned and all their active task will be deactivated.</span>
                                                                                  </p>
                                                                              </v-list-item-content>
                                                                          </v-list-item>
                                                                      </v-list>
                                                                  </v-col>
                                                                  <v-col cols="12" xs="12" sm="12" md="12">
                                                                      <!--User List with checkbox add here-->
                                                                      <v-data-table :headers="RoleAssignedUserTableHeader"
                                                                                    :items="RoleAssignedUserTableItems"
                                                                                    hide-default-footer
                                                                                    item-key="ID"
                                                                                    fixed-header
                                                                                    height="25vh"
                                                                                    class="elevation-0">
                                                                          <!-- Assign Role combobox-->
                                                                          <template v-slot:item.RoleId="{item}">
                                                                              <v-autocomplete dense="" outlined="" class="mt-3"
                                                                                              :items="RoleAssignedUsersInput" :item-text="i => i.RoleName" :item-value="i => i.RoleId" v-model="item.RoleId"
                                                                                              label="select user role" solo flat return-object @change="fnCheckRemoveButtonStatus()">
                                                                              </v-autocomplete>
                                                                          </template>
                                                                      </v-data-table>
                                                                  </v-col>
                                                                  <v-col cols="12" xs="12" sm="12" md="12">
                                                                      <label class="float-left mt-4">
                                                                          <v-avatar size="20" color="#FFF0C7" class="elevation-0">
                                                                              <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                          </v-avatar>
                                                                          {{RemoveRoleNotifyMsg}}
                                                                      </label>
                                                                      <!-- <v-checkbox label="Notify user" v-model="NotifyCheckBox" class="float-right"></v-checkbox> -->
                                                                  </v-col>
                                                              </div>
                                                              <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                  <Secondary-Button id="AutoTest_U167" class="btn-122x36 mr-2" title="Remove" @click.native="RemoveRole()" v-bind:disabled="btnRemoveBtnDisable"></Secondary-Button>
                                                                  <Default-Button-Outlined id="AutoTest_U168" class="btn-122x36" title="Cancel" @click.native="RemoveRoleDialog = false, btnRemoveBtnDisable = true, UserRoleAssignCount = 0"></Default-Button-Outlined>
                                                              </v-col>
                                                          </v-row>
                                                      </v-container>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>
                              <!--Remove user role dailog ends here-->
                              <!-- Edit User Role Dailog Starts here -->
                              <v-dialog v-model="EditUserRoleDailog" width="800" persistent>
                                  <v-card>
                                      <v-card-title primary-title class="page-head pt-4">
                                          <h3 class="text-left page-head">
                                              <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                  <v-icon size="20" color="primary">mdi-account-edit-outline</v-icon>
                                              </v-avatar>
                                              Edit User Role Details
                                          </h3>
                                          <v-spacer></v-spacer>
                                          <v-btn id="AutoTest_U169" text @click="fnEditRoleCancelClick()">
                                              <v-icon color="#707070" size="15">mdi-close</v-icon>
                                          </v-btn>
                                      </v-card-title>
                                      <v-card-text class="pt-0">
                                          <v-container fluid class="pt-0">
                                              <v-row no-gutters>
                                                  <v-col cols="12" md="12" class="pt-0">
                                                      <v-container fluid>
                                                          <v-row no-gutters>
                                                              <v-col cols="12" md="12" class="text-center">
                                                                  <v-avatar size="75" class="elevation-1 my-4">
                                                                      <v-icon color="grey" size="60">mdi-account-tie-outline</v-icon>
                                                                  </v-avatar>
                                                              </v-col>
                                                              <v-row :justify="justifyCenter" no-gutters>
                                                                  <v-col cols="6" xs="6" sm="6" md="6">
                                                                      <v-text-field outlined="" dense="" label="User Role Name" type="text" v-model="EditRoleInput.RoleName" :rules="alphanumericRules">
                                                                      </v-text-field>
                                                                      <v-textarea rows="3" outlined="" dense="" label="Description" type="text" v-model="EditRoleInput.Description">
                                                                      </v-textarea>
                                                                  </v-col>
                                                              </v-row>
                                                              <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                  <Secondary-Button id="AutoTest_U170" class="btn-122x36 mr-2" title="Save" @click.native="UpdateRole()"></Secondary-Button>
                                                                  <Default-Button-Outlined id="AutoTest_U171" class="btn-122x36" title="Cancel" @click.native="fnEditRoleCancelClick()"></Default-Button-Outlined>
                                                              </v-col>
                                                          </v-row>
                                                      </v-container>
                                                  </v-col>
                                              </v-row>
                                          </v-container>
                                      </v-card-text>
                                  </v-card>
                              </v-dialog>
                              <!-- Edit User Dailog Ends here -->
                          </v-menu>
                      </template><!-- Coper-->
                      <!-- slot for Role Name -->
                      <template v-slot:item.RoleName="{ item }">
                          <router-link class="text--default" link :to="{ name: 'userroleindividual', query:  { WsID: WorkspaceID, Access : RWAccess, RoleId: item.RoleId, RoleAssignedUsersInput : RoleAssignedUsersInput } }">
                              <div class="pa-0 text-limit" style="width:155px">
                                  {{ item.RoleName }}
                              </div>
                          </router-link>
                      </template>
                      <!--if no data-->
                      <template v-slot:no-data>
                          <v-row class="justify-center" v-if="userRolesTableItems.length == 0">
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
              <!--Data table-->
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
  
    var objRoleBL;
    async function importscript() {
        return Promise.all([
            import("../../BL/UserRole.js").then(mod => {
                objRoleBL = new mod.UserRole()
            })
        ]);
    }
    export default {
      components :{
        'Secondary-Button': SecondaryButton,
        'Default-Button-Outlined': DefaultButtonOutlined,
      },
        data() {
            return {
                valid: true,
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                closeonClick: false,
                closeOnContentClick :false,
                dense: false,
                justifyCenter: 'center',
                NewUserRoleRegSteps: 0,
                NewUserRoleDialog: false,
                AddUserDialog: false,
                RemoveRoleDialog: false,
                EditUserRoleDailog: false,
                EditRoleInput: {
                    RoleId: 0,
                    RoleName: "",
                    Description: ""
                },
                searchuser: '',
                searchscreen: '',
                searchuserrole: '',
                RoleId: 0,
                RoleName : "",
                userRolesTableheaders: [
                    { text: 'ID', align: ' d-none', value: 'RoleId', },
                    { text: 'Name', align: 'left', value: 'RoleName', width: "155px" },
                    { text: 'Last Modified', value: 'UpdatedDate',width:"135px" },
                    { text: 'No. of Screens', value: 'NoOfScreens',width: "135px"  },
                    { text: 'Actions', value: 'userroleActions', sortable: false, width: "100px",align:"center"  },
                ],
                userRolesTableItems: [],
                roleScreensTableheaders: [
                    {
                        text: 'Screen Name',
                        align: 'left',
                        value: 'ScreenName',
                    },
                    { text: 'Access Type', value: 'AccessId' },
                    { text: 'Preview', value: 'ScreenPath' },
                    //{ text: 'Actions', value: 'screenActions', sortable: 'false' },
                ],
                roleScreensTableItems: [],
                AccessTypeValues: [],
                userTableheaders: [
                    { text: 'ID', align: ' d-none', value: 'ID' },
                    {text: 'Name',align: 'left',value: 'Name'},
                    { text: 'Projects Count', value: 'ProjectsCount' },
                    { text: 'EmailID', value: 'EmailID', align: ' d-none' },
                ],
                userTableItems: [],
                AddUserRoleScreenInput: {
                    RoleId: 0,
                    RoleName: "",
                    Description: "",
                    ScreenList: []
                },
                UserList: [],
                
                btnDisable: true,
                RoleAssignedUserTableHeader: [
                    { text: 'ID', value: 'ID', align: ' d-none'},
                    {text: 'Users',align: 'left',value: 'Name' },
                    {text: 'Status',value: 'Status'},
                    { text: 'Tasks', value: 'Tasks' },
                    { text: 'User Role', value: 'RoleId' },
                ],

                RoleAssignedUserTableItems: [],
                IsHideDeleteMsg: false,
                NotifyCheckBox: true,
                RemoveRoleNotifyMsg: "",
                AddUserNotifyMsg: '',
                RoleAssignedUsersInput: [],
                UserRoleAssignCount: 0,
                btnRemoveBtnDisable : true,
                WorkspaceID: 0,
                AddRoleMenu: false,
                ActionMenu: false,
                alphanumericRules: [
                    v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name',
                        (value) => !!value || "Required field.",
                ],
                screenAccess: 0,
                RWAccess: 0,
                userimage: '',
                ScreenList: [],
            }
        },
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await this.GetUserRolesList();
            await this.GetAccessTypeList();
        },
        watch: {
            params: {
                deep: true
            },

            /// <summary>
            ///  UserList :- Function used to notify the users with a message when selecting users from the table
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            UserList: function () {
                this.AddUserNotifyMsg = "";
                if (this.UserList.length > 0) {
                    if (this.UserList.length > 2) {
                        this.AddUserNotifyMsg = this.UserList[0].Name + "," + this.UserList[1].Name + " and " + (this.UserList.length - 2) + " more will be added to User Role : " + this.RoleName;
                    } else if (this.UserList.length == 2) {
                        this.AddUserNotifyMsg = this.UserList[0].Name + "," + this.UserList[1].Name + " will be added to User Role : " + this.RoleName;
                    } else {
                        this.AddUserNotifyMsg = this.UserList[0].Name + " will be added to User Role : " + this.RoleName;
                    }
                }
            },
            
            /// <summary>
            ///  ScreenList :- Function used to add screens to the user role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            ScreenList: function() {
                if (this.ScreenList.length == 0) {
                    this.ScreenList = this.roleScreensTableItems.filter(x => (x.DefaultScreen == 1));
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

            /// <summary>
            ///  fnCheckRoleName :- Function used to validate the role name already exists, if not then move to next step
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCheckRoleName() {
                if (this.$refs.Form.validate() === true) {
                    var temp = this.userRolesTableItems.filter(x => x.RoleName.includes(this.AddUserRoleScreenInput.RoleName));
                    if (temp.length > 0) {
                        this.Msg = ("RoleName you entered already exists. Please enter a new role name.");
                        this.infoSnackbarMsg = true;
                    } else {
                        this.NewUserRoleRegSteps = 2;
                    }
                }
            },

            /// <summary>
            ///  fnEditRoleClick :- Function used to open edit user role dialog when click on edit button
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnEditRoleClick(item) {
                this.EditUserRoleDailog = true;
                this.EditRoleInput.RoleId = item.RoleId;
                this.EditRoleInput.RoleName = item.RoleName;
                this.EditRoleInput.Description = item.Description;
            },

            /// <summary>
            ///  fnEditRoleCancelClick :- Function used to cancel edit user role dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditRoleCancelClick() {
                this.EditUserRoleDailog = false;
                this.EditRoleInput.RoleName = "";
                this.EditRoleInput.Description = "";
                this.EditRoleInput.RoleId = 0;
            },

            /// <summary>
            ///  btnStatus :- Function used to check whether new role name enterd when adding user role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            btnStatus() {
                if (this.AddUserRoleScreenInput.RoleName != "") {
                    this.btnDisable = false;
                } else {
                    this.btnDisable = true;
                }
            },

            /// <summary>
            ///  GetUserRolesList :- Function used to fetch User Roles
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUserRolesList() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.GetUserRolesList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                  this.userRolesTableItems = result.data;
                }
            },

            /// <summary>
            ///  GetScreenList :- Function used to fetch screen list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetScreenList() {
                var result = await objRoleBL.GetScreenList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.roleScreensTableItems = result.data;
                    this.ScreenList = this.roleScreensTableItems.filter(x => (x.DefaultScreen == 1));
                }
            },

            /// <summary>
            ///  GetAccessTypeList :- Function used to fetch all AccessType
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAccessTypeList() {
                var result = await objRoleBL.GetAccessTypeList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AccessTypeValues = result.data;
                }
            },

            /// <summary>
            ///  RemoveRole :- Function used to delete user role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async RemoveRole() {
                this.ActionMenu = false;
                var data = new FormData();
                data.append("RoleId", this.RoleId);
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("UsersList", JSON.stringify(this.RoleAssignedUserTableItems));
                data.append("Notify", this.NotifyCheckBox);
                var result = await objRoleBL.DeleteUserRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully Removed");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Remove");
                    this.errorSnackbarMsg = true;
                }
                this.GetUserRolesList();
                this.RoleId = "0";
                this.RoleAssignedUserTableItems = [];
                this.RemoveRoleDialog = false;
                this.IsHideDeleteMsg =false
            },

            /// <summary>
            ///  GetUsersToAssignRole :- Function used to fetch user list to assign role based on RoleId, RoleName
            /// </summary>
            /// <param name="RoleId"></param>
            /// <param name="RoleName"></param>
            /// <returns> </returns>
            async GetUsersToAssignRole(RoleId, RoleName) {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                this.RoleId = RoleId;
                this.RoleName = RoleName;
                var result = await objRoleBL.GetUsersToAssignRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.userTableItems = result.data;
                }
            },

            /// <summary>
            ///  AssignUsersToRole :- Function used to add/save users to role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AssignUsersToRole() {
                this.ActionMenu = false;
                if (this.UserList.length == 0) {
                    this.Msg = ("Please select at least one User");
                    this.infoSnackbarMsg = true;
                    return;
                }
                
                var data = new FormData();
                data.append("RoleId", this.RoleId);
                data.append("UserList", JSON.stringify(this.UserList));
                data.append("RoleName", this.RoleName)
                data.append("notify", this.NotifyCheckBox)
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.AssignUsersToRole(data)
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully Added");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Add");
                    this.errorSnackbarMsg = true;
                }
                this.AddUserDialog = false;
                this.RoleId = 0;
                this.UserList = [];
            },

            /// <summary>
            ///  fnAddRole :- Function used to add new role
            /// </summary>
            /// <param name="isScreenList"></param>
            /// <returns> </returns>
            async fnAddRole(isScreenList) {
                //this.AddRoleMenu = false;
                if (this.AddUserRoleScreenInput.RoleId == 0 && this.AddUserRoleScreenInput.RoleName == "") {
                    this.Msg = ("User Role Name is required");
                    this.infoSnackbarMsg = true;
                    return;
                }

                if (isScreenList == false) {
                    this.ScreenList = [];
                } else {
                    if (this.ScreenList.length == 0) {
                        this.Msg = ("Please select at least one screens to add to group", this.AddUserRoleScreenInput.RoleName);
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }

                var data = new FormData();
                data.append("RoleId", this.AddUserRoleScreenInput.RoleId);
                data.append("RoleName", this.AddUserRoleScreenInput.RoleName);
                data.append("Description", this.AddUserRoleScreenInput.Description);
                data.append("ScreenList", JSON.stringify(this.ScreenList));
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.AddRole(data);
                
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data == 0) {
                    this.Msg = ("Failed");
                    this.errorSnackbarMsg = true;
                    this.NewUserRoleRegSteps = 2;
                } else if (result.data > 0) {
                    
                    this.NewUserRoleRegSteps = 3
                    this.AddUserRoleScreenInput.RoleName = "";
                    this.AddUserRoleScreenInput.Description = "";
                    this.ScreenList = [];
                    this.selectedusers = [];
                    this.btnDisable = true;
                    this.searchuser = "";
                    this.GetUserRolesList();
                    this.$refs.Form.resetValidation()
                }
            },

            /// <summary>
            ///  remove :- Function used to remove screens
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            remove(item) {
                this.ScreenList.splice(this.ScreenList.indexOf(item), 1);
                this.ScreenList = [...this.ScreenList];
            },

            /// <summary>
            ///  fnClearAfterSave :- Function used to clear after save
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClearAfterSave() {
                this.AddUserRoleScreenInput.RoleName = "";
                this.AddUserRoleScreenInput.Description = "";
                this.ScreenList = [];
                this.selectedusers = [];
                this.NewUserRoleRegSteps = 1;
                this.searchuser = "";
                this.NewUserRoleDialog = false;
                this.GetUserRolesList();
                this.AddUserDialog = false;
            },

            /// <summary>
            ///  RemoveAssginUserChipItems :- Function used to remove user when assign from the chip items
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            RemoveAssginUserChipItems(item) {
                this.UserList.splice(this.UserList.indexOf(item), 1);
                this.UserList = [...this.UserList];
            },

            /// <summary>
            ///  UpdateRole :- Function used to update user role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateRole() {
                this.ActionMenu = false;
                var data = new FormData();
                data.append("RoleId", this.EditRoleInput.RoleId);
                data.append("RoleName", this.EditRoleInput.RoleName);
                data.append("Description", this.EditRoleInput.Description);
                var result = await objRoleBL.UpdateRole(data);
                
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Updated Successfully");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Update");
                    this.errorSnackbarMsg = true;
                }
                this.EditUserRoleDailog = false;
                this.EditRoleInput.RoleId = 0;
                this.EditRoleInput.RoleName = "";
                this.EditRoleInput.Description = "";
                this.GetUserRolesList();
            },

            /// <summary>
            ///  GetRoleBasedUsersList :- Function used to fetch list of assigned users for user section and remove role section based on roleId,RoleName
            /// </summary>
            /// <param name="RoleId"></param>
            /// <param name="RoleName"></param>
            /// <returns> </returns>
            async GetRoleBasedUsersList(RoleId, RoleName) {
                this.RoleId = RoleId;
                this.RoleName = RoleName;
                var data = new FormData();
                data.append("RoleId", RoleId);
                data.append("WorkspaceID", this.WorkspaceID);
                
                var result = await objRoleBL.GetRoleBasedUsersList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.RoleAssignedUserTableItems = result.data;
                }
                
                if (this.RoleAssignedUserTableItems.length > 0) {
                    this.IsHideDeleteMsg= false
                    var UserCount = this.RoleAssignedUserTableItems.length;
                   
                    if (UserCount == 1) {
                        this.RemoveRoleNotifyMsg = this.RoleAssignedUserTableItems[0].Name +" will be notify";
                    } else 
                    if (UserCount == 2) {
                        this.RemoveRoleNotifyMsg = this.RoleAssignedUserTableItems[0].Name + ","
                            + this.RoleAssignedUserTableItems[1].Name + " will be notify";
                    } else 
                    if (UserCount > 2) {
                        this.RemoveRoleNotifyMsg = this.RoleAssignedUserTableItems[0].Name + ","
                            + this.RoleAssignedUserTableItems[1].Name + "+", UserCount-2 +" will be notify";
                    }
                    this.RoleAssignedUsersInput= this.userRolesTableItems.filter(p => p.RoleId != RoleId);
                } else {
                    this.btnRemoveBtnDisable = false;
                    this.IsHideDeleteMsg = true;
                }
            },

            /// <summary>
            ///  fnCancelSaveUserRole :- Function used to cancel user role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCancelSaveUserRole() {
                this.NewUserRoleDialog = false;
                this.AddUserRoleScreenInput.RoleName = "";
                this.AddUserRoleScreenInput.Description = "";
                this.ScreenList = [];
                this.selectedusers = [];
                this.btnDisable = true ;
                this.searchuser = "";
                this.NewUserRoleRegSteps = 1;
                this.$refs.Form.resetValidation()
            },

            /// <summary>
            ///  fnCheckRemoveButtonStatus :- Function used to checks whether new role is assigned for all users or not
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCheckRemoveButtonStatus() {
                if (this.RoleAssignedUserTableItems.length > 0) {
                    var i;
                    this.UserRoleAssignCount = 0; // intiatizing to default
                    for (i = 0; i<= this.RoleAssignedUserTableItems.length-1; i++) {
                        try {
                            if (this.RoleAssignedUserTableItems[i].RoleId.RoleName != "" &&  typeof(this.RoleAssignedUserTableItems[i].RoleId.RoleName) != "undefined" ) {
                                this.UserRoleAssignCount = this.UserRoleAssignCount + 1;
                            }
                        }
                        catch{
                            this.UserRoleAssignCount = 0;
                        }
                    }
                    
                    if (this.UserRoleAssignCount == this.RoleAssignedUserTableItems.length) {
                        this.btnRemoveBtnDisable = false;
                    } else {
                        this.btnRemoveBtnDisable = true;
                    }
                }
            },
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