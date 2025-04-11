<template>
    <v-row>
        <v-container fluid>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-tabs centered v-model="UsersAndGroupsTab" class="pt-0" color="tab-active-line">
                    <v-tab>User</v-tab>
                    <v-tab id="AutoTest_I234" @click="fnGetGroupsListByIntitiativeID()">Users Groups</v-tab>
                </v-tabs>
                <!-- Users-->
                <v-tabs-items v-model="UsersAndGroupsTab">
                    <!-- User-->
                    <v-tab-item>
                        <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
                            <v-row class="text-right">
                                <v-spacer></v-spacer>
                                <!--Add User Button-->
                                <v-menu :offset-y=true>
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" width="160" color="success-btn" dark class="elevation-0 mr-2">
                                            Add User
                                            <v-icon>mdi-chevron-down</v-icon>
                                        </v-btn>
                                    </template>
                                    <!-- Added For List -->
                                    <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                        <!-- Invite User-->
                                        <v-list-item id="AutoTest_I235" link @click.stop="" @click="InviteUserDialog = true">
                                            <v-list-item-icon>
                                                <v-icon>
                                                    mdi-plus-circle-outline
                                                </v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                Invite User
                                            </v-list-item-content>
                                        </v-list-item>
                                        <!-- Invite and Import User-->
                                        <v-list-item id="AutoTest_I236" link @click.stop="ImportInviteUser = true">
                                            <v-list-item-icon>
                                                <v-icon>mdi-file-account-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Import & Invite User</v-list-item-content>
                                        </v-list-item>
                                        <!-- Select User-->
                                        <v-list-item id="AutoTest_I237" link @click.stop="GetInitUsersDetails()">
                                            <v-list-item-icon>
                                                <v-icon>
                                                    mdi-account-outline
                                                </v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                Select User
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Bulk Action -->
                                <v-menu offset-y transition="scroll-y-transition">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" color="grey" outlined="" class="elevation-0 mr-2">
                                            Bulk Actions
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I238" link @click.stop="fnBulkRemoveClick()">
                                            <v-list-item-icon>
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                    <v-dialog v-model="DeleteBulkUserDialog" width="800">
                                        <v-card>
                                            <v-card-title primary-title class="page-head py-4">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                </v-avatar>
                                                Bulk Remove User
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_I239" text @click="DeleteBulkUserDialog=false">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>
                                            <v-card-text class="pt-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row no-gutters>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <h4 class="text-center heading-3">Are you sure want to remove this selected user ?</h4>
                                                                    </v-col>
                                                                    <div v-if="!isHidden" style="width:100%">
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
                                                                                                <v-avatar size="20">
                                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                                         alt="User DP" />
                                                                                                    <span class="black--text bottom"
                                                                                                          style="position:absolute; bottom:0px !important"></span>
                                                                                                </v-avatar>
                                                                                                {{adduserNotifyMessage}}<v-spacer></v-spacer>
                                                                                            </span>
                                                                                            <span class="message">  is part of {{0}}  projects by removing this user all active tasks will be inactive and will have to be reassigned at project setup.</span>
                                                                                        </p>
                                                                                    </v-list-item-content>
                                                                                </v-list-item>
                                                                            </v-list>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                            <v-data-table :headers="userProjectsTableheaders"
                                                                                          :items="userProjectTableItems"
                                                                                          item-key="Name"
                                                                                          fixed-header
                                                                                          height="25vh"
                                                                                          class="elevation-0">

                                                                                <template v-slot:body="{ items }">
                                                                                    <tbody>
                                                                                        <tr v-for="item in items" :key="item.ProjectId"
                                                                                            v-bind:style="{backgroundColor: ProjectRowColor[item.ProjectStatus]}">
                                                                                            <td>{{ item.ProjectName }}</td>
                                                                                            <td>{{ item.ProjectStatus }}</td>
                                                                                            <td>{{ item.Tasks }}</td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </template>
                                                                            </v-data-table>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" v-if="isHidden">
                                                                            <label class="float-left mt-4">
                                                                                <v-avatar size="20">
                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                         alt="User DP" />
                                                                                    <span class="black--text bottom"
                                                                                          style="position:absolute; bottom:0px !important"></span>
                                                                                </v-avatar>
                                                                                {{uName}} will be notified
                                                                            </label>
                                                                            <v-checkbox label="Notify user" class="float-right" v-model="isNotify"></v-checkbox>
                                                                        </v-col>
                                                                    </div>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                        <v-btn id="AutoTest_I240" color="secondary elevation-0" class="mr-2"
                                                                               @click="fnDeleteBulkUserFromInitiative()">Confirm</v-btn>
                                                                        <v-btn id="AutoTest_I241" outlined color="grey" @click="DeleteBulkUserDialog = false">Cancel</v-btn>
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
                                <div class="small-text-field">
                                    <v-text-field label="Search"
                                                  class="mr-2"
                                                  dense
                                                  prepend-inner-icon="mdi-magnify"
                                                  style="max-width:200px;"
                                                  outlined
                                                  v-model="searchuserList"></v-text-field>
                                </div>

                                <!-- ---------   Invite and Import User dailogues ----------   -->
                                <!-- Invite User -->
                                <v-dialog persistent v-model="InviteUserDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                            </v-avatar>Invite User
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I243" text @click="CloseInviteUserDialog(1)">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text class="py-0">
                                            <v-container fluid class="pt-0">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-stepper v-model="NewUserRegSteps" class="elevation-0">
                                                            <v-stepper-header class="elevation-0">
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

                                                                                    </v-col>
                                                                                    <v-col md="12">
                                                                                        <div class="chip-container mb-2">
                                                                                            <v-chip-group multiple
                                                                                                          column
                                                                                                          active-class="primary--text">
                                                                                                <v-chip id="AutoTest_I244" close v-for="item in InviteUserListInput" :key="item.EmailId"
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
                                                                                <v-btn id="AutoTest_I245" color="secondary" class="mr-2 elevation-0" @click="NewUserRegSteps = 2"
                                                                                       v-bind:disabled="InviteUserListInput.length ==0">Next</v-btn>
                                                                                <v-btn id="AutoTest_I246" outlined color="grey" @click="CloseInviteUserDialog(1)">Cancel</v-btn>
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
                                                                                        <v-btn id="AutoTest_I247" text @click="removeRole(InviteUserListInput.indexOf(item))">
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
                                                                                    <v-col cols="12" xs="12" sm="8" md="8" class="py-0">
                                                                                        <p>
                                                                                            Invited users will be notified that they have added under respective user roles.
                                                                                        </p>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="4" md="4" class="pt-2">
                                                                                        <v-checkbox label="Notify user" input-value="true" v-model="chkNotify" class="float-right"></v-checkbox>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                <v-btn id="AutoTest_I248" color="secondary" class="mr-2 elevation-0" @click="SendInvite(1),NewUserRegSteps = 3"
                                                                                       v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)">Send</v-btn>
                                                                                <v-btn id="AutoTest_I249" outlined color="grey" @click="NewUserRegSteps = 1">Previous</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-container>
                                                                </v-stepper-content>

                                                                <v-stepper-content step="3" class="pa-0">
                                                                    <v-container fluid class="pt-0">
                                                                        <v-row no-gutters v-if="sentInvite==null">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Sending ....<br />
                                                                                </p>

                                                                            </v-col>

                                                                        </v-row>
                                                                        <v-row no-gutters v-if="sentInvite">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                <h4 class="text-center heading-3">Well Done !</h4>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-avatar size="150" tile>
                                                                                    <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                                </v-avatar>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Your Invites have been sent out successfully.<br />Bask in your glory !
                                                                                </p>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-btn id="AutoTest_I250" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1)">Done</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row no-gutters v-else-if="!sentInvite && sentInvite!=null">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Sorry .. Failed to send Invites...<br />
                                                                                </p>
                                                                                <p class="text-medium">
                                                                                    {{retMsg}}<br />
                                                                                </p>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-btn id="AutoTest_I251" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1)">OK</v-btn>
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

                                <!-- Import User -->
                                <v-dialog v-model="ImportInviteUser" width="800">
                                    <v-card>
                                        <v-card-title primary-title="" class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-account</v-icon>
                                            </v-avatar>Import & Invite User
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I252" text="" @click="CloseInviteUserDialog(2)">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>

                                        <v-card-text class="pt-0">
                                            <v-container fluid="" class="pt-0">
                                                <v-row>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-stepper v-model="ImportInviteUserSteps" class="elevation-0">
                                                            <v-stepper-header class="elevation-0">

                                                                <v-stepper-step :complete="ImportInviteUserSteps > 1" step="1" color="inherit">
                                                                    <!--<v-icon color="success">mdi-file-upload-outline</v-icon>-->
                                                                    Upload
                                                                </v-stepper-step>

                                                                <v-divider></v-divider>
                                                                <v-stepper-step :complete="ImportInviteUserSteps > 2" step="2" color="inherit">
                                                                    <!--<v-icon color="success">mdi-file-check-outline</v-icon>-->
                                                                    Format Check
                                                                </v-stepper-step>
                                                            </v-stepper-header>
                                                            <v-stepper-items>
                                                                <v-stepper-content step="1" class="pt-0">
                                                                    <v-container fluid class="pt-0 pb-0">
                                                                        <v-row no-gutters="">
                                                                            <v-col cols="12" md="12">
                                                                                <h4 class="text-center heading-3">Step1 : Upload your CSV file set in our template</h4>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="text-center">
                                                                                <v-card class="elevation-0">
                                                                                    <v-card-text class="py-0">
                                                                                        <v-row no-gutters>
                                                                                            <v-col>
                                                                                                <v-avatar size="100" tile>
                                                                                                    <img src="../../images/Import-icon.svg" alt="Import" />
                                                                                                </v-avatar>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                        <v-row no-gutters>
                                                                                            <v-col>
                                                                                                <p class="text-medium">
                                                                                                    Your CSV file must map all our mandatory fields<br />
                                                                                                    and your files cannot exceed more than 1000 rows
                                                                                                </p>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                        <v-row>
                                                                                            <v-col align-center justify-center>
                                                                                                <v-btn color="grey" outlined="" dark="" class="elevation-0" height="40">
                                                                                                    <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                                                </v-btn>
                                                                                            </v-col>
                                                                                            <v-col align-center justify-center>
                                                                                                <v-file-input prepend-icon="" label="Upload File" outlined dense
                                                                                                              id="file" v-model="fileInput"
                                                                                                              prepend-inner-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                              @change="handleFileUpload">
                                                                                                </v-file-input>
                                                                                            </v-col>

                                                                                        </v-row>
                                                                                        <v-row v-if="csvfile.name">

                                                                                            <v-col align-center="" justify-center="">
                                                                                                <v-chip id="AutoTest_I253" close label color="white" @click:close="csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null">
                                                                                                    <v-icon size="30" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                                    {{ fileInput.name }}
                                                                                                </v-chip>

                                                                                            </v-col>
                                                                                        </v-row>
                                                                                    </v-card-text>
                                                                                </v-card>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                <v-btn id="AutoTest_I254" color="secondary elevation-0" class="mr-2" @click="ImportInviteUserSteps=2,fnCSVLoadParse(),GetUserRoleDetails()" v-bind:disabled="fileInput==null">Next</v-btn>
                                                                                <v-btn id="AutoTest_I255" outlined="" color="grey" @click="CloseInviteUserDialog(2)">Cancel</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-container>
                                                                </v-stepper-content>
                                                                <v-stepper-content step="2" class="pt-0">
                                                                    <v-row no-gutters>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                            <h4 class="heading-3"> We are running a format check this will take a moment</h4>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                            <v-chip color="#FFE9E9" label dark v-if="roleErrorCount >0">
                                                                                <v-icon size="30" color="red" height="100">mdi-alert-circle</v-icon>
                                                                                <span style="color:#F55D5D">{{ roleErrorCount }} User Roles have not been set to proceed</span>
                                                                            </v-chip>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                            <v-data-table :headers="importInviteTableheaders"
                                                                                          :items="csvfiledata"
                                                                                          item-key="EmailId"
                                                                                          height="25vh"
                                                                                          fixed-header
                                                                                          class="elevation-0">
                                                                                <template v-slot:item.UserRole="{item}">
                                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                                        <v-select :items="userRoleTableItems"
                                                                                                  item-text="userRole"
                                                                                                  item-value="userRole"
                                                                                                  v-model="item.UserRole"
                                                                                                  label="Select User Role"
                                                                                                  outlined
                                                                                                  solo
                                                                                                  dense
                                                                                                  flat
                                                                                                  return-object
                                                                                                  @change="ChangeNewRole(item,2)"
                                                                                                  class="mt-3"></v-select>
                                                                                    </v-col>
                                                                                </template>

                                                                                <template v-slot:item.Message="{item}">
                                                                                    <v-col style="color:red;font-weight: bold;font-size:small">
                                                                                        {{item.Message}}
                                                                                    </v-col>
                                                                                </template>

                                                                            </v-data-table>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                                            <v-row no-gutters>
                                                                                <v-col cols="12" xs="12" sm="8" md="8" class="py-0">
                                                                                    <p>
                                                                                        Invited users will be notified that they have added under respective user roles.
                                                                                    </p>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="4" md="4" class="pt-2">
                                                                                    <v-checkbox label="Notify user" input-value="true" v-model="chkNotify1" class="float-right"></v-checkbox>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-col>
                                                                        <!--Footer Actions-->
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                            <v-btn id="AutoTest_I256" color="secondary elevation-0" class="mr-2" @click="SendInvite(2)"
                                                                                   v-bind:disabled="(roleErrorCount >0) || (!chkNotify1)">Send</v-btn>
                                                                            <v-btn id="AutoTest_I257" outlined="" color="grey" @click="ImportInviteUserSteps= 1">Previous</v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-stepper-content>
                                                                <v-stepper-content step="3" class="pa-0">
                                                                    <v-container fluid class="pt-0">
                                                                        <v-row no-gutters v-if="sentInvite==null">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Sending ....<br />
                                                                                </p>
                                                                            </v-col>

                                                                        </v-row>
                                                                        <v-row no-gutters v-if="sentInvite">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                <h4 class="text-center heading-3">Well Done !</h4>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-avatar size="150" tile>
                                                                                    <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                                </v-avatar>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Your Invites have been sent out successfully.<br />Bask in your glory !
                                                                                </p>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-btn id="AutoTest_I258" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(2)">Done</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row no-gutters v-else-if="!sentInvite  && sentInvite!=null">
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <p class="text-medium">
                                                                                    Sorry .. Failed to send Invites...<br />
                                                                                </p>
                                                                                <p class="text-medium">
                                                                                    {{retMsg}}<br />
                                                                                </p>
                                                                            </v-col>
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                <v-btn id="AutoTest_I259" color="secondary" class="mr-2 elevation-0"
                                                                                       @click="CloseInviteUserDialog(2)">OK</v-btn>
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

                                <!-- Select User -->
                                <v-dialog v-model="SelectUserDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-0">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                            </v-avatar>Select User
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I260" text @click="SelectUserDialog=false">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text>
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-2 px-1">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" md="12" class="text-right pt-5">
                                                                <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUser">
                                                                </v-text-field>
                                                            </v-col>

                                                            <div class="chip-container mb-2" v-if="SelectUserSelectedUsersList.length>0">
                                                                <v-chip-group multiple column active-class="primary--text">
                                                                    <v-chip id="AutoTest_I261" close v-for="item in SelectUserSelectedUsersList" :key="item.ID"
                                                                            v-model="chipUsrItem" @click:close="removeSelectedUserItem(item)" class="ma-2" small>
                                                                        <v-icon left color="#707070">mdi-email</v-icon>
                                                                        {{ item.Name }}
                                                                    </v-chip>
                                                                </v-chip-group>

                                                            </div>
                                                            <!--v-chip for Assign roles ends here -->
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <!--User List with checkbox add here-->
                                                                <v-data-table :headers="SelectUserTableheaders"
                                                                              :items="SelectUserTableItems"
                                                                              show-select
                                                                              item-key="UserID"
                                                                              sort-by="Name"
                                                                              fixed-header
                                                                              height="25vh"
                                                                              :search="searchUser"
                                                                              v-model="SelectUserSelectedUsersList"
                                                                              class="elevation-0">
                                                                </v-data-table>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                <v-btn id="AutoTest_I262" v-bind:disabled="SelectUserSelectedUsersList.length==0" color="secondary" class="mr-2 elevation-0" @click="SaveSelectUser()">Save</v-btn>
                                                                <v-btn id="AutoTest_I263" outlined color="grey" @click="SelectUserDialog=false">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-divider></v-divider>
                                    </v-card>
                                </v-dialog>
                            </v-row>
                        </v-col>
                        <v-container fluid>
                            <v-row no-gutters>
                                <v-col cols="12" md="12" sm="12" xs="12">
                                    <v-data-table :headers="userListTableheaders"
                                                  :items="userListTableItems"
                                                  show-select
                                                  item-key="ID"
                                                  sort-by="Name"
                                                  v-model="userListTableItemsSelected"
                                                  class="elevation-0"
                                                  :search="searchuserList">
                                        <!--user list actions-->
                                        <template v-slot:item.userActions="{item}">
                                            <v-menu close-on-content-click transition="scroll-y-transition" left offset-y>
                                                <template v-slot:activator="{ on }">
                                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                                        <v-icon>mdi-dots-vertical</v-icon>
                                                    </v-btn>
                                                </template>
                                                <v-list color="white" light width="auto" class="action-button-list">
                                                    <!-- Remove-->
                                                    <v-list-item id="AutoTest_I264" link @click.stop="GetAllUserProjects(item.ID),DeleteUserDialog = true,
                                                                                                    uName =item.Name,uEmail= item.EmailId,uPrjCount =item.userProjectCount">
                                                        <v-list-item-icon>
                                                            <v-icon>mdi-trash-can-outline</v-icon>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>Remove</v-list-item-content>
                                                    </v-list-item>
                                                </v-list>
                                                <v-dialog v-model="DeleteUserDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head py-4">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                            </v-avatar>
                                                            Remove User
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_I265" text @click="DeleteUserDialog=false,uName =null,uEmail= null,uPrjCount=null">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <v-container fluid class="pt-0">
                                                                            <v-row>
                                                                                <v-col cols="12" md="12" class="pt-0">
                                                                                    <h4 class="text-center heading-3">Are you sure want to remove this user ?</h4>
                                                                                </v-col>
                                                                                <div v-if="!isHidden" style="width:100%">
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
                                                                                                            <v-avatar size="20">
                                                                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                                                     alt="User DP" />
                                                                                                                <span class="black--text bottom"
                                                                                                                      style="position:absolute; bottom:0px !important"></span>
                                                                                                            </v-avatar>
                                                                                                            {{uName}}<v-spacer></v-spacer>
                                                                                                        </span>
                                                                                                        <span class="message">  is part of {{uPrjCount}}  projects by removing this user all active tasks will be inactive and will have to be reassigned at project setup.</span>
                                                                                                    </p>
                                                                                                </v-list-item-content>
                                                                                            </v-list-item>
                                                                                        </v-list>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                                        <v-data-table :headers="userProjectsTableheaders"
                                                                                                      :items="userProjectTableItems"
                                                                                                      item-key="Name"
                                                                                                      fixed-header
                                                                                                      height="25vh"
                                                                                                      class="elevation-0">

                                                                                            <template v-slot:body="{ items }">
                                                                                                <tbody>
                                                                                                    <tr v-for="item in items" :key="item.ProjectId"
                                                                                                        v-bind:style="{backgroundColor: ProjectRowColor[item.ProjectStatus]}">
                                                                                                        <td>{{ item.ProjectName }}</td>
                                                                                                        <td>{{ item.ProjectStatus }}</td>
                                                                                                        <td>{{ item.Tasks }}</td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </template>
                                                                                        </v-data-table>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                                        <label class="float-left mt-4">
                                                                                            <v-avatar size="20">
                                                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                                     alt="User DP" />
                                                                                                <span class="black--text bottom"
                                                                                                      style="position:absolute; bottom:0px !important"></span>
                                                                                            </v-avatar>
                                                                                            {{uName}} will be notified
                                                                                        </label>
                                                                                        <v-checkbox label="Notify user" class="float-right" v-model="isNotify"></v-checkbox>
                                                                                    </v-col>
                                                                                </div>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                                    <v-btn id="AutoTest_I266" color="secondary elevation-0" class="mr-2"
                                                                                           @click="fnDeleteUserFromInitiative(item.ID,item.Name,item.EmailId,isNotify,addGroupUserTableItems.indexOf(item))">Confirm</v-btn>
                                                                                    <v-btn id="AutoTest_I267" outlined color="grey" @click="DeleteUserDialog = false">Cancel</v-btn>
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
                        </v-container>
                    </v-tab-item>

                    <!-- User Group-->
                    <v-tab-item>
                        <v-col cols="12" xs="12" sm="12" md="12">
                            <v-row class="text-right">
                                <v-spacer></v-spacer>
                                <!--Add Group Button-->
                                <v-menu :offset-y=true>
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" width="160" color="success-btn" dark class="elevation-0 mr-2">
                                            Add Group
                                            <v-icon>mdi-chevron-down</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                        <!-- Invite User-->
                                        <v-list-item id="AutoTest_I268" link @click.stop="" @click="AddGroupDialog=true,GetUserGroups()">
                                            <v-list-item-icon>
                                                <v-icon>
                                                    mdi-plus-circle-outline
                                                </v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>
                                                Add Group
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Add Group dialog-->
                                <v-dialog v-model="AddGroupDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head pb-2">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                            </v-avatar>Add User Group
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I269" text @click="AddGroupDialog=false">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text>
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-2 px-1">
                                                        <v-row>

                                                            <v-col cols="12" md="12" class="pt-2">
                                                                <h4 class="text-center heading-3">Select user groups to add them to an initiatives</h4>
                                                            </v-col>

                                                            <v-col cols="12" md="12" class="py-0">
                                                                <v-text-field v-model="addGroupSearchUser" outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" hide-details>
                                                                </v-text-field>
                                                            </v-col>

                                                            <v-col v-if="selectedUserGroup.length>0" md="12" class="py-0">
                                                                <div class="chip-container mb-2">
                                                                    <v-chip-group multiple
                                                                                  column
                                                                                  active-class="primary--text">
                                                                        <v-chip id="AutoTest_I270" close v-for="(item,key) in selectedUserGroup" v-bind:key="item.GroupID"
                                                                                @click:close="$delete(selectedUserGroup, key)">
                                                                            <v-icon left color="#707070">mdi-account</v-icon>
                                                                            {{ item.GroupName }}
                                                                        </v-chip>
                                                                    </v-chip-group>
                                                                </div>
                                                            </v-col>

                                                            <!--v-chip for Assign roles ends here -->
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <!--User List with checkbox add here-->
                                                                <v-data-table :headers="addGroupUserTableHeaders"
                                                                              :items="addGroupUserTableItems"
                                                                              :search="addGroupSearchUser"
                                                                              show-select=""
                                                                              item-key="GroupID"
                                                                              class="elevation-0"
                                                                              v-model="selectedUserGroup"
                                                                              fixed-header
                                                                              height="20vh">
                                                                </v-data-table>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                <v-btn id="AutoTest_I271" v-bind:disabled="selectedUserGroup.length==0" color="secondary" class="mr-2 elevation-0" @click="fnAddGroupToInitiative()">Save</v-btn>
                                                                <v-btn id="AutoTest_I272" outlined color="grey" @click="AddGroupDialog=false,selectedUserGroup=[]">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-divider></v-divider>
                                    </v-card>
                                </v-dialog>

                                <!--Bulk Action -->
                                <v-menu offset-y transition="scroll-y-transition">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" color="grey" outlined="" class="elevation-0 mr-2">
                                            Bulk Actions
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I273" link @click.stop="fnBulkGrouoRemoveClick()">
                                            <v-list-item-icon>
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                    <v-dialog v-model="DeleteBulkGroupDialog" width="800">
                                        <v-card>
                                            <v-card-title primary-title class="page-head">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                </v-avatar>
                                                Delete User Group
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_I274" text @click="DeleteBulkGroupDialog=false">
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
                                                            <v-btn id="AutoTest_I275" color="secondary" class="mr-2 elevation-0" @click="fnDeleteBulkGroupFromInitiative">Delete</v-btn>
                                                            <v-btn id="AutoTest_I276" outlined color="grey" @click="DeleteBulkGroupDialog=false">Cancel</v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-card-text>
                                        </v-card>
                                    </v-dialog>
                                </v-menu>

                                <!--Search-->
                                <div class="small-text-field">
                                    <v-text-field label="Search" class="mr-2" v-model="searchgroups"
                                                  dense prepend-inner-icon="mdi-magnify"
                                                  outlined></v-text-field>
                                </div>
                            </v-row>
                        </v-col>
                        <v-container fluid>
                            <v-row no-gutters>
                                <v-col cols="12" md="12" sm="12" xs="12">
                                    <v-data-table :headers="groupsTableHeaders"
                                                  :items="groupsTableItems"
                                                  :search="searchgroups"
                                                  v-model="selectedGroups"
                                                  show-select
                                                  item-key="GroupId"
                                                  class="elevation-0">

                                        <!--Group list column State-->
                                        <template v-slot:item.State="{ item }">
                                            <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" @change="fnGroupStateClick(item)"></v-switch>
                                        </template>

                                        <!-- Groups actions -->
                                        <template v-slot:item.usergroupActions="{item}">
                                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition">
                                                <template v-slot:activator="{ on }">
                                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                                        <v-icon>
                                                            mdi-dots-vertical
                                                        </v-icon>
                                                    </v-btn>
                                                </template>
                                                <v-list color="white" light width="auto" class="action-button-list">
                                                    <v-list-item id="AutoTest_I277" link="" @click.stop="fnDeleteGroupClick(item)">
                                                        <v-list-item-icon>
                                                            <v-icon>mdi-trash-can-outline</v-icon>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            Remove
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </v-list>
                                                <!--Delete Group Dialog-->
                                                <v-dialog v-model="DeleteGroupDialog" width="800">
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                            </v-avatar>
                                                            Delete User Group
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_I278" text @click="DeleteGroupDialog=false">
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
                                                                        <v-btn id="AutoTest_I279" color="secondary" class="mr-2 elevation-0" @click="fnDeleteGroupFromInitiative(item)">Delete</v-btn>
                                                                        <v-btn id="AutoTest_I280" outlined color="grey" @click="DeleteGroupDialog=false">Cancel</v-btn>
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

                                <!--Toggle user group state-->
                                <v-dialog v-model="GroupStateToggleDialog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-electric-switch</v-icon>
                                            </v-avatar>
                                            Change User Group Status
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I281" text="" @click="GroupStateToggleDialog=false, fnGetUserGroupsList()">
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
                                                        <v-btn id="AutoTest_I282" color="secondary" class="mr-2 elevation-0" @click="fnGroupChangeState(item)">Confirm</v-btn>
                                                        <v-btn id="AutoTest_I283" outlined color="grey" @click="GroupStateToggleDialog=false,fnGetUserGroupsList()">Cancel</v-btn>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>
                            </v-row>
                        </v-container>
                    </v-tab-item>
                </v-tabs-items>
            </v-col>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-row>
</template>
<script>
    var objInitiative;
    async function importscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
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
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                InitiativeId: 0,
                justifyCenter: 'center',
                UsersAndGroupsTab: 0,
                //Users and Groups Tab
                //users sub tab
                NewUserRegSteps: 0,
                SearchRoleName: "",
                roleErrorCount: 0,
                InviteUserDialog: false,
                ImportInviteUser: false,
                ImportInviteUserSteps: 0,
                chipItem: "",
                AssignRoleTableHeaders: [
                    { text: 'Email', value: 'EmailId' },
                    { text: 'User Role', value: 'UserRole' },
                    { text: 'Screen Info', value: 'data-table-expand' },
                    { text: '', align: "left", color: "red", value: "Message" },
                    { text: '', value: 'RowItemCancel' }

                ],
                fileInput: null,
                csvfile: {
                    name: "",
                    size: 0,
                    namedisplay: ""
                },
                csvfiledata: [],
                importInviteTableheaders: [
                    { text: "Email", align: "left", value: "EmailId" },
                    { text: "User Role", value: "UserRole" },
                    { text: "", align: "left", color: "red", value: "Message" }
                ],
                userListTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "EmailId", value: "EmailId" },
                    { text: "Organization", value: "userOrganisation" },
                    { text: "User Group", value: "userGroup", },
                    { text: "User Role", value: "userRole" },
                    { text: "Projects", value: "userProjectCount" },
                    { text: "Actions", value: "userActions", sortable: false }
                ],
                userListTableItems: [],
                userListTableItemsSelected: [],
                searchuserList: "",
                userProjectsTableheaders: [
                    { text: "Project Name", align: "left", value: "ProjectName" },
                    { text: "Status", value: "ProjectStatus" },
                    { text: "Tasks", value: "Tasks", sortable: "false" }
                ],
                userProjectTableItems: [],
                tUserID: 0,
                tUserStatus: 0,
                tUserName: "",
                tEmail: "",
                uName: "",
                uPrjCount: 0,
                isHidden: false,
                isNotify: true,
                DeleteUserDialog: false,
                DeleteBulkUserDialog: false,
                /* Invite,Importand Select User details starts here */
                //----Select Users option
                SelectUserDialog: false,
                SelectUserTableheaders: [
                    { text: "UserID", align: ' d-none', value: "UserID" },
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                SelectUserTableItems: [],
                searchUser: "",
                SelectUserSelectedUsersList: [],
                chipUsrItem: "",
                //--------------------
                //===========Invite Users==============
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
                userListItems:[],

                clearable: false,
                chkNotify: true,
                chkNotify1: true,

                IsValidData: false,
                sentInvite: null,

                ScreenNameTableheaders: [
                    { text: "Screen Name", align: "left", value: "userScreenName" },
                    { text: "Access Type", value: "userAccessType" },
                    { text: "Preview", value: "Preview" }
                ],
                userScreenNameTableItems: [],
                adduserNotifyMessage: "",
                /* Invite,Importand Select User details ends here */

                //Groups sub tab
                //groups list section
                groupsTableHeaders: [
                    { text: 'Name', align: 'left', value: 'GroupName', },
                    { text: 'No. of Users', value: 'UsersCount' },
                    { text: 'No. of Projects', value: 'ProjectsCount' },
                    { text: 'No. of Sub Groups', value: 'SubGroupsCount' },
                    { text: 'State', align: ' d-none', value: 'State' },  //toggle state currenly invisible backend functionality not deleted
                    { text: 'Actions', value: 'usergroupActions', sortable: false },
                ],
                groupsTableItems: [],
                selectedGroups: [],
                searchgroups: '',
                //Add Group dialog related
                AddGroupDialog: false,
                AddGroupRegSteps: 0,
                AddGroupInput: { GroupId: 0, GroupName: "", GroupDescription: "" },
                addGroupNotifyMessage: "",
                addGroupNotifyCheckbox: true,
                //Add Group button starts here
                addGroupUserTableHeaders: [

                    { text: 'GroupId', align: ' d-none', value: 'GroupID', },
                    { text: 'User Groups', align: 'left', value: 'GroupName', },
                    { text: 'Member Count', value: 'MemberCount' },

                ],
                addGroupUserTableItems: [],
                selectedUserGroup: [],
                addGroupSearchUser: '',
                  //Add Group button ends here

                selectedUsers: [],
               
                //Remove/Delete Group dialog related
                DeleteBulkGroupDialog: false,
                DeleteGroupDialog: false,
                //Group State Toggle
                GroupStateToggleDialog: false,
                GroupStateToggleInput: { GroupId: 0, GroupName: "", IsActive: 0 },
                //projects table used in Delete Group dialog and group state change   
                projectsTableHeaders: [
                    {
                        text: "Project Name",
                        align: "left",
                        value: "ProjectName",
                        icon: "mdi-account-multiple"
                    },
                    { text: "Status", value: "Status" }
                ],
                projectsTableItems: [],
                searchprojects: "",
                ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "" },

            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeId = this.$route.query.InitiativeID;
            await importscript();
            await importUserscript();
            await this.fnGetUsersListByIntitiativeID();
            await this.fnGetGroupsListByIntitiativeID();
            await this.GetUserRoleDetails();
            await this.GetAllUsersList();
        },
        methods: {
            //Users and Groups tab -- Groups sub-tab functions starts here
            async fnGetGroupsListByIntitiativeID() {
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.GetGroupsListByIntitiativeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data)
                 {
                    this.groupsTableItems = result.data;
                }
            },
            async GetUserGroups() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.GetInitUserGroups(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                    this.addGroupUserTableItems = result.data;
                }
            },
            async fnAddGroupToInitiative() {
                if (this.selectedUserGroup.length == 0) {
                    this.Msg = ("No Users selected. Please select the users to add to group" + this.AddGroupInput.GroupName);
                    this.snackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("GroupList", JSON.stringify(this.selectedUserGroup));
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.AddGroupToInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.Msg = ("Successfully added ");
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to add ");
                        this.snackbarMsg = true;
                }
                this.AddGroupDialog = false;
                this.addGroupSearchUser = "";
                this.fnGetGroupsListByIntitiativeID();
                this.selectedUserGroup = [];  
            },
            //  Add group button
            async fnDeleteGroupClick(item) {
               
                this.DeleteGroupDialog = true;
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);

                var result = await objInitiative.PostData(url, data);
              
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                    this.projectsTableItems = result.data;
                }
            },
            async fnDeleteGroupFromInitiative(item) {
               
                var data = new FormData();
                data.append("GroupId", item.GroupId);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.DeleteGroupFromInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.Msg = ("Successfully deleted " + item.GroupName);
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to delete " + item.GroupName);
                        this.snackbarMsg = true;
                }
                this.fnGetGroupsListByIntitiativeID();
                this.DeleteGroupDialog = false;
            },
            async fnGroupStateClick(item) {
                this.GroupStateToggleDialog = true;
                var url = window.SERVER_URL + "/UserGroups/GetProjectsList";
                var data = new FormData();
                data.append("GroupId", item.GroupId);

                var result = await objInitiative.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                    this.projectsTableItems = result.data;
                }
                this.GroupStateToggleInput.GroupId = item.GroupId;
                this.GroupStateToggleInput.GroupName = item.GroupName;
                this.GroupStateToggleInput.IsActive = item.IsActive;
            },
            async fnGroupChangeState() {
                var url = window.SERVER_URL + "/UserGroups/ChangeGroupState";
                var data = new FormData();
                data.append("GroupId", this.GroupStateToggleInput.GroupId);
                data.append("IsActive", this.GroupStateToggleInput.IsActive);

                var result = await objInitiative.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.Msg = ("Successfully changed the state " + this.GroupStateToggleInput.GroupName);
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to change the state " + this.GroupStateToggleInput.GroupName);
                        this.snackbarMsg = true;
                }
                this.GroupStateToggleDialog = false;
                this.GroupStateToggleInput.GroupId = 0;
                this.GroupStateToggleInput.GroupName = "";
                this.GroupStateToggleInput.IsActive = 0;
                this.fnGetGroupsListByIntitiativeID();
            },
            //Users and Groups tab -- Groups sub-tab functions ends here

            //Users and Groups tab -- Users sub-tab functions starts here
            async fnGetUsersListByIntitiativeID() {
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetUsersListByIntitiativeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                    this.userListTableItems = result.data;
                }
            },
            async GetAllUserProjects(Id) {
                var result = await objUser.GetAllUserProjects(Id);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data){
                    this.userProjectTableItems = result.data;
                }
                if (this.userProjectTableItems.length == 0) {
                    this.isHidden = true;
                }
                else {
                    this.isHidden = false;
                }
            },
            async fnDeleteUserFromInitiative(Id, uName, uEmail, ChkNotified, Index) {
                var data = new FormData();
                data.append("UserId", Id);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.DeleteUserFromInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.Msg = ("Successfully deleted " + uName);
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to delete " + uName);
                        this.snackbarMsg = true;
                }
                this.fnGetUsersListByIntitiativeID();
                this.DeleteUserDialog = false;
            },
            //bulk user and group Remove
            fnBulkRemoveClick() {
                if (this.userListTableItemsSelected.length > 0) {
                    this.DeleteBulkUserDialog = true;
                } else {
                    this.Msg = ("Please select the User");
                    this.snackbarMsg = true;
                }
            },
            async fnDeleteBulkUserFromInitiative() {
                if (this.userListTableItemsSelected.length > 0) {
                    var data = new FormData();
                    data.append("UserId", JSON.stringify(this.userListTableItemsSelected));
                    data.append("InitiativeId", this.InitiativeId);
                    var result = await objInitiative.DeleteBulkUserFromInitiative(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.status == "success"){
                            this.Msg = ("Successfully deleted ");
                            this.snackbarMsg = true;
                    } else {
                            this.Msg = ("Failed to delete ");
                            this.snackbarMsg = true;
                    }
                    this.fnGetUsersListByIntitiativeID();
                    this.DeleteBulkUserDialog = false;
                    this.userListTableItemsSelected= [];
                }
            },
            fnBulkGrouoRemoveClick() {
                if (this.selectedGroups.length > 0) {
                    this.DeleteBulkGroupDialog = true;
                } else {
                    this.Msg = ("Please select the Group");
                    this.snackbarMsg = true;
                }
            },
            async fnDeleteBulkGroupFromInitiative() {
             
                var data = new FormData();
                data.append("GroupId", JSON.stringify(this.selectedGroups));
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiative.DeleteBulkGroupFromInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success" ) {
                        this.Msg = ("Successfully deleted ");
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to delete ");
                        this.snackbarMsg = true;
                }
                this.fnGetGroupsListByIntitiativeID();
                this.DeleteBulkGroupDialog = false;
                this.selectedGroups= [];
            },
            //*** Invite,Importand Select User details Starts here ***//
            async GetAllUsersList() {
                
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objUser.GetAllUsers(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else if (result.data)
                    this.userListItems = result.data;
            },
            async AddToList() {

                var msg = "The EmailId already exisits...";
                var usrEmail = this.userListItems.filter(x => x.EmailId == this.NewEmail);

                if ((usrEmail.length == 0) || (usrEmail.length > 0 && usrEmail[0].IsDeleted == 1)) {
                    if ((this.NewEmail == "") ? false : (this.reg.test(this.NewEmail)) ? true : false) {

                        this.InviteUserListInput.push({ EmailId: this.NewEmail, Message: "Select User Role" });
                        this.roleErrorCount += 1;
                        this.chkEmails = true;
                    }
                }
                else if (usrEmail.length > 0) {
                    if (usrEmail[0].IsDeleted == 0) {
                        if (usrEmail[0].IsRegisteredUser == 0)
                            msg = msg + " But not a Registered User ...";
                        if (usrEmail[0].IsActive == 0)
                            msg = msg + " But user status is Inactive...";
                        else
                            msg = msg + " Please choose 'Select User'option..."
                    }
                    this.Msg = (msg);
                    this.snackbarMsg = true;
                }
                NewEmail: undefined;
                this.NewEmail = "";
                this.clearable = true;
            },
            async removemailfromchip(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
            },
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
            async SendInvite(Type) {
                this.IsValidInput(Type);
                if (this.IsValidData) {
                    var data = new FormData();

                    if (Type == 1) {
                        data.append("userInviteList", JSON.stringify(this.InviteUserListInput));
                        data.append("WorkspaceId", this.WorkspaceID);//sessionStorage.getItem('WorkspaceID'));//window.WorkspaceID);
                        data.append("inviteFrom", "Initiative")
                        data.append("InitiativeId", this.InitiativeId)
                    }
                    else if (Type == 2) {
                        data.append("userInviteList", JSON.stringify(this.csvfiledata));
                        data.append("WorkspaceId", this.WorkspaceID);//sessionStorage.getItem('WorkspaceID'));//window.WorkspaceID);
                        data.append("inviteFrom", "Initiative")
                        data.append("InitiativeId", this.InitiativeId)
                        this.ImportInviteUserSteps = 3;
                    }

                    var result = await objUser.SendInvite(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.status == "success") {
                        this.sentInvite = true;
                    }
                    else {
                        this.sentInvite = false;
                        this.retMsg = result.status;
                    }
                    this.fnGetUsersListByIntitiativeID();

                }
            },
            CloseInviteUserDialog(Type) {
                switch (Type) {
                    case 1:
                        this.InviteUserListInput = [];
                        this.NewEmail = "";
                        this.NewUserRegSteps = 1;
                        this.userScreenNameTableItems = [];
                        this.InviteUserDialog = false;
                        break;

                    case 2:
                        document.getElementById('file').value = "";
                        this.fileInput = [];
                        this.csvfile.name = null;
                        this.csvfile.size = 0;
                        this.csvfile.namedisplay = null;
                        this.csvfiledata = [];
                        this.ImportInviteUserSteps = 1;
                        this.ImportInviteUser = false;
                        break;
                }
                this.roleErrorCount = 0;
            },
            getValue(value, rName) {
                //this.AddUserListInput.userRoleId = value;
                this.selectedNewRole = rName;
                this.userScreenNameTableItems = [];
                this.GetUserScreenDetails(value);
                this.isRoleActive = true;
            },
            OpenAssignRole() {
                this.userScreenNameTableItems = [];
                this.AssignUserRoleDialog = true;
                var te = this.userRoleSelector.value;
                this.userRoleSelector = "";
            },
            removeRole(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
                this.roleErrorCount -= 1;
            },
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
            async ChangeNewRole(objRole, InvOrImp) {
                if (objRole.Message != null) {
                    objRole.Message = null;
                    this.roleErrorCount -= 1;
                }

                if (InvOrImp == 1)
                    this.GetUserScreenDetails(objRole.UserRole.userRoleID);
            },
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
            //-------------------Import users starts here
            openFileDialog() {
                document.getElementById('file').click();
            },
            handleFileUpload() {
                this.csvfile.name = this.fileInput.name;
                this.csvfile.size = this.fileInput.size;
                this.csvfile.namedisplay = this.csvfile.name + "\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0" + this.formatFileSize(this.csvfile.size, 0);
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            fnCSVLoadParse() {
                if (this.ImportInviteUserSteps == 2 && this.csvfile.name) {
                    var vm = this;
                    if (window.FileReader) {
                        var reader = new FileReader();
                        reader.readAsText(this.fileInput);
                        // Handle errors load
                        reader.onload = function (event) {
                            var csv = event.target.result;
                            vm.csvfiledata = vm.csvJSON(csv); 

                        };
                        reader.onerror = function (evt) {
                            if (evt.target.error.name == "NotReadableError") {
                                this.Msg = ("Cannot read file !");
                                this.snackbarMsg = true;
                            }
                        };
                    } else {
                        this.Msg = ('FileReader is not supported in this browser.');
                        this.snackbarMsg = true;
                    }
                }
            },
            csvJSON(csv) {
                var vm = this;
                vm.roleErrorCount = 0; //reset before loading the file datas
                var lines = csv.split(/\r\n|\n/);
                var result = [];
                var headers = lines[0].split(",");
                //console.log("headers = ", headers[0], headers[1]);
                for (var i = 1; i < lines.length; i++) {
                    var obj = {};
                    var errEmail = false;
                    var currentline = lines[i].split(",");
                    if (currentline != "") {
                        for (var j = 0; j < headers.length; j++) {
                            if (!this.reg.test(currentline[0])) {
                                errEmail = true;
                                break;
                            }
                            obj[headers[j]] = currentline[j];
                        }
                        var filterrolename = vm.userRoleTableItems.filter(x => x.userRole == obj["UserRole"]);

                        if (!errEmail) {
                            if (filterrolename.length == 0) {//If role doesnt match our data, reset to blank and ask to set in the table

                                vm.roleErrorCount += 1;
                                obj["UserRole"] = "";
                                obj["Message"] = "Select User Role";
                                //obj["Message"] = "*";
                            } else {
                                obj["Message"] = "";
                                obj["UserRole"] = filterrolename[0];
                            }
                            result.push(obj);
                        }
                    }
                }
                return result;
            },
            //--------------------Select User
            async GetInitUsersDetails() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("InitiativeId", this.InitiativeId)
                var result = await objInitiative.GetInitUsersDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                    this.SelectUserTableItems = result.data;
                }
                this.SelectUserDialog = true;
            },
            async removeSelectedUserItem(item) {
                this.SelectUserSelectedUsersList.splice(this.SelectUserSelectedUsersList.indexOf(item), 1);
                this.SelectUserSelectedUsersList = [...this.SelectUserSelectedUsersList];
            },
            async SaveSelectUser() {
             
                if (this.SelectUserSelectedUsersList.length == 0) {
                    this.Msg = ("Please select at least one User");
                    this.snackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                data.append("userInviteList", JSON.stringify(this.SelectUserSelectedUsersList));

                var result = await objInitiative.AddUserInitiative(data)
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.Msg = ("Successfully Added");
                        this.snackbarMsg = true;
                } else {
                        this.Msg = ("Failed to Add");
                        this.snackbarMsg = true;
                }
                this.SelectUserDialog = false;
                this.SelectUserSelectedUsersList = [];
                this.fnGetUsersListByIntitiativeID();
            },
            //*** Invite,Importand Select User details ends here ***//
            //Users and Groups tab -- Users sub-tab functions ends here

        },
        watch: {
            selectedUsers: function () {
                this.addGroupNotifyMessage = "";
                if (this.selectedUsers.length > 2) {
                    this.addGroupNotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " and " + (this.selectedUsers.length - 2) + " more will be added to ";
                } else if (this.selectedUsers.length == 2) {
                    this.addGroupNotifyMessage = this.selectedUsers[0].Name + "," + this.selectedUsers[1].Name + " will be added to ";
                } else {
                    this.addGroupNotifyMessage = this.selectedUsers[0].Name + " will be added to ";
                }
            },
            userListTableItemsSelected: function () {
                this.adduserNotifyMessage = "";
                if (this.userListTableItemsSelected.length > 2) {
                    this.adduserNotifyMessage = this.userListTableItemsSelected[0].Name + "," + this.userListTableItemsSelected[1].Name + " and " + (this.userListTableItemsSelected.length - 2) ;
                } else if (this.userListTableItemsSelected.length == 2) {
                    this.adduserNotifyMessage = this.userListTableItemsSelected[0].Name + "," + this.userListTableItemsSelected[1].Name ;
                } else {
                    this.adduserNotifyMessage = this.userListTableItemsSelected[0].Name ;
                }
            },
        }
  };
</script>