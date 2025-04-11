<template>
    <v-card min-height="70vh">
        <v-row no-gutters>
            <!--User list-->
            <v-row v-if="FilterUserTableItems.length>0" no-gutters>
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="userTableheaders"
                                  :items="FilterUserTableItems"
                                  show-select=""
                                  item-key="ID"
                                  sort-by="Name"
                                  v-model="userListInput"
                                  class="elevation-0"
                                  :search="SearchName"
                                  fixed-header
                                  height="60vh"
                                  @item-selected="isBulkAction=false"
                                  :footer-props="{
                              itemsPerPageOptions:[10,50,100,-1]
                              }"
                                  :items-per-page="10">


                        <template v-slot:top="">
                            <v-row class="px-4">
                                <v-col cols="12" xs="12" sm="12" md="3">
                                    <h5 class="page-head">
                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                            <v-icon size="15" color="primary">mdi-account-outline</v-icon>
                                        </v-avatar>Users
                                    </h5>
                                </v-col>
                                <v-col cols="12" xs="12" sm="12" md="9">
                                    <v-row>
                                        <v-spacer></v-spacer>
                                        <!--Add user button-->
                                        <v-menu offset-y="" transition="scroll-y-transition" close-on-content-click="" internal-activator="" v-bind:disabled="!screenAccess">
                                            <template v-slot:activator="{ on }">
                                                <v-btn height="39" v-on="on" color="success-btn" dark="" class="elevation-0 mr-4">
                                                    Add User
                                                    <v-icon>mdi-chevron-down</v-icon>
                                                </v-btn>
                                            </template>
                                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                                <v-list-item id="AutoTest_U75" link="" @click.stop="InviteUserDialog = true">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-account-plus-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Invite User</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_U76" link="" @click.stop="ImportInviteUser = true">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-file-account-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Import & Invite User</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-menu>
                                        <!--Assign New User Dialog-->
                                        <v-dialog v-model="InviteUserDialog" width="800" persistent="">
                                            <v-card>
                                                <v-card-title primary-title="" class="page-head pb-0">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                        <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                                    </v-avatar>Invite User
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_U77" text="" @click="CloseInviteUserDialog(1)">
                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-card-text class="py-0">
                                                    <v-container fluid="" class="pt-0">
                                                        <v-row no-gutters="">
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-stepper v-model="NewUserRegSteps" class="elevation-0">
                                                                    <v-row :justify="justifyCenter" class="pa-0 ma-0">
                                                                        <v-col md="6" class="pa-0 ma-0">
                                                                            <v-stepper-header class="elevation-0">
                                                                                <v-stepper-step :complete="NewUserRegSteps > 1" step="1" color="inherit">Add Email</v-stepper-step>
                                                                                <v-divider></v-divider>
                                                                                <v-stepper-step :complete="NewUserRegSteps > 2" step="2" color="inherit">Assign Role </v-stepper-step>
                                                                            </v-stepper-header>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-stepper-items>
                                                                        <!-- Email Adding section-->
                                                                        <v-stepper-content step="1" class="pa-0">
                                                                            <v-container fluid="" class="pt-0">
                                                                                <v-row no-gutters="">
                                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                                        <h4 class="text-center heading-3">Step 1: Inivite people by enter their email addresses below</h4>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                                        <v-row :justify="justifyCenter">
                                                                                            <v-col md="12">
                                                                                                <v-text-field id="txtEmail"
                                                                                                              outlined=""
                                                                                                              dense=""
                                                                                                              label="Enter Email addresses"
                                                                                                              type="NewEmail"
                                                                                                              v-model="NewEmail"
                                                                                                              :error-messages="emailErrors"
                                                                                                              required=""
                                                                                                              :rules="emailRules"
                                                                                                              :clearable="clearable"
                                                                                                              @change="AddToList()"></v-text-field>
                                                                                                <small class="error--text" v-model="emailIdError" v-if="emailIdError=='' && NewEmail.length > 0">Please enter a valid e-mail</small>
                                                                                                <div class="chip-container mb-2" v-if="InviteUserListInput.length >0">
                                                                                                    <v-chip-group multiple=""
                                                                                                                  column=""
                                                                                                                  active-class="primary--text">
                                                                                                        <v-chip id="AutoTest_U78" close="" v-for="(item,idx) in InviteUserListInput" :key="idx"
                                                                                                                v-model="chipItem"
                                                                                                                @click:close="remove(item)" small="">
                                                                                                            <v-icon left="" color="#707070" size="20">mdi-email</v-icon>
                                                                                                            {{ item.EmailId }}
                                                                                                        </v-chip>

                                                                                                    </v-chip-group>
                                                                                                </div>
                                                                                            </v-col>

                                                                                        </v-row>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                        <Secondary-Button id="AutoTest_U79" class="btn-122x36 mr-4" title="Next" @click.native="NewUserRegSteps = 2" v-bind:disabled="InviteUserListInput.length ==0"></Secondary-Button>
                                                                                        <Default-Button-Outlined id="AutoTest_U80" class="btn-122x36" title="Cancel" @click.native="CloseInviteUserDialog(1)"></Default-Button-Outlined>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-container>
                                                                        </v-stepper-content>
                                                                        <!-- Assign Role section-->
                                                                        <v-stepper-content step="2" class="pa-0">
                                                                            <v-container fluid="" class="pt-0">
                                                                                <v-row no-gutters="" class="py-0">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 py-0">
                                                                                        <h4 class="text-center heading-3">Step 2: Assign user role and set access type for screens</h4>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="text-center py-0">
                                                                                        <v-text-field outlined=""
                                                                                                      dense=""
                                                                                                      label="Search"
                                                                                                      type="Text"
                                                                                                      hide-details
                                                                                                      prepend-inner-icon="mdi-magnify"
                                                                                                      class="mb-2"
                                                                                                      v-model="SearchRoleName"></v-text-field>
                                                                                    </v-col>
                                                                                    <v-col cols="7" xs="7" sm="7" md="7" class="pt-0 py-0 mb-2" v-if="roleErrorCount >0">
                                                                                        <!--<v-alert dense="" outlined="" color="#F4282D">-->
                                                                                        <v-icon size="15" color="red" height="">mdi-alert-circle</v-icon>
                                                                                        <small style="color:red"> {{ roleErrorCount }} user roles have not been set to proceed</small>
                                                                                        <!--</v-alert>-->
                                                                                    </v-col>

                                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                                        <v-data-table :headers="AssignRoleTableHeaders"
                                                                                                      :items="InviteUserListInput"
                                                                                                      hide-default-footer
                                                                                                      :single-expand="singleExpand"
                                                                                                      :expanded.sync="expanded"
                                                                                                      item-key="EmailId"
                                                                                                      :search="SearchRoleName"
                                                                                                      v-model="InviteUsersInput"
                                                                                                      show-expand=""
                                                                                                      class="elevation-0"
                                                                                                      fixed-header=""
                                                                                                      height="33vh"
                                                                                                      :footer-props="{
                                                                                                  itemsPerPageOptions:[10,50,100,-1]
                                                                                                  }"
                                                                                                      :items-per-page="10">

                                                                                            <template v-slot:item.UserRole="{item}">
                                                                                                <v-select :items="userRoleTableItems"
                                                                                                          item-text="userRole"
                                                                                                          item-value="userRoleID"
                                                                                                          label="Select User Role"
                                                                                                          v-model="item.UserRole"
                                                                                                          outlined=""
                                                                                                          solo=""
                                                                                                          dense=""
                                                                                                          flat=""
                                                                                                          return-object=""
                                                                                                          required=""
                                                                                                          hide-details
                                                                                                          style="width:170px"
                                                                                                          @change="ChangeNewRole(item,1)"></v-select>

                                                                                            </template>

                                                                                            <template v-slot:expanded-item="{ headers }">
                                                                                                <td :colspan="headers.length">
                                                                                                    <v-data-table :headers="ScreenNameTableheaders"
                                                                                                                  :items="userScreenNameTableItems"
                                                                                                                  hide-default-footer
                                                                                                                  item-key="userScreenName"
                                                                                                                  class="elevation-0">
                                                                                                        <template v-slot:item.Preview="">
                                                                                                            <router-link to="" target="_blank">Proj.jpg</router-link>
                                                                                                        </template>
                                                                                                    </v-data-table>
                                                                                                </td>
                                                                                            </template>
                                                                                            <template v-slot:item.RowItemCancel="{item}">
                                                                                                <v-btn id="AutoTest_U81" text="" @click="removeRole(item)">
                                                                                                    <v-icon size="15">mdi-close</v-icon>
                                                                                                </v-btn>
                                                                                            </template>
                                                                                            <template v-slot:item.EmailId="{item}">
                                                                                                <div class="pa-0 text-limit" style=" width:100px">{{item.EmailId}}</div>
                                                                                            </template>
                                                                                            <template v-slot:item.Message="{item}">
                                                                                                <div class="pa-0 text-limit" style="color:red;width:100px;font-weight: bold;font-size:small;padding-left:0px;padding-right:0px">
                                                                                                    {{item.Message}}
                                                                                                </div>
                                                                                            </template>
                                                                                        </v-data-table>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0 pt-3">
                                                                                        <v-row no-gutters="" class="py-0">
                                                                                            <v-col cols="9" xs="12" class="py-0" style="font-size: small">
                                                                                                Invited users will be notified that they have added under respective user roles.
                                                                                            </v-col>
                                                                                            <v-col cols="3" xs="12" class="py-0">
                                                                                                <!-- <v-checkbox color="success-check-box" label="Notify user" input-value="true" v-model="chkNotify" class="float-right py-0 ma-0"></v-checkbox> -->
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                        <Secondary-Button id="AutoTest_U82" class="btn-122x36 mr-4" title="Send" @click.native="SendInvite(1),NewUserRegSteps = 3" v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)"></Secondary-Button>
                                                                                        <Default-Button-Outlined id="AutoTest_U83" class="btn-122x36" title="Previous" @click.native="NewUserRegSteps = 1"></Default-Button-Outlined>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-container>
                                                                        </v-stepper-content>

                                                                        <v-stepper-content step="3" class="pa-0">
                                                                            <v-container fluid="" class="pt-0">
                                                                                <v-row no-gutters="" v-if="sentInvite==null">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium blinker">
                                                                                            Sending... <br />
                                                                                        </p>
                                                                                        <v-progress-circular :size="50"
                                                                                                             color="primary"
                                                                                                             indeterminate>
                                                                                        </v-progress-circular>
                                                                                    </v-col>

                                                                                </v-row>
                                                                                <v-row no-gutters="" v-if="sentInvite">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                        <h4 class="text-center heading-3">Well Done !</h4>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-avatar size="150" tile="">
                                                                                            <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                                        </v-avatar>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium">
                                                                                            Your Invites have been sent out successfully.<br />Bask in your glory !
                                                                                        </p>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-btn id="AutoTest_U84" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1)">Done</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row no-gutters="" v-else-if="!sentInvite && sentInvite!=null">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium">
                                                                                            Sorry .. Failed to send Invites...<br />
                                                                                        </p>
                                                                                        <p class="text-medium">
                                                                                            {{retMsg}}<br />
                                                                                        </p>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-btn id="AutoTest_U85" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1)">OK</v-btn>
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
                                        <!-- Added Dialogue box for Import user -->
                                        <v-dialog v-model="ImportInviteUser" width="800" persistent="">
                                            <v-card>
                                                <v-card-title primary-title="" class="page-head pb-0">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                        <v-icon size="20" color="primary">mdi-account</v-icon>
                                                    </v-avatar>Import & Invite User
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_U86" text="" @click="CloseInviteUserDialog(2)">
                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-card-text class="pt-0">
                                                    <v-container fluid class="pt-0 pb-0">
                                                        <v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-stepper v-model="ImportInviteUserSteps" class="elevation-0">

                                                                    <v-row :justify="justifyCenter">
                                                                        <v-col md="6">
                                                                            <v-stepper-header class="elevation-0">
                                                                                <v-stepper-step :complete="ImportInviteUserSteps > 1" step="1" color="inherit">Upload </v-stepper-step>
                                                                                <v-divider></v-divider>
                                                                                <v-stepper-step :complete="ImportInviteUserSteps > 2" step="2" color="inherit">Format Check</v-stepper-step>
                                                                            </v-stepper-header>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-stepper-items>
                                                                        <v-stepper-content step="1" class="pt-0">
                                                                            <v-container fluid="" class="pt-0 pb-0">
                                                                                <v-row no-gutters="">
                                                                                    <v-col cols="12" md="12">
                                                                                        <h4 class="text-center heading-3">Step1 : Upload your CSV file set in our template</h4>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="text-center">
                                                                                        <v-card class="elevation-0">
                                                                                            <v-card-text class="py-0">
                                                                                                <v-row no-gutters="">
                                                                                                    <v-col>
                                                                                                        <v-avatar size="100" tile="">
                                                                                                            <img src="../../images/Import-icon.svg" alt="Import" />
                                                                                                        </v-avatar>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <v-row no-gutters="">
                                                                                                    <v-col>
                                                                                                        <p class="text-medium">
                                                                                                            Your CSV file must map all our mandatory fields<br />
                                                                                                            and your files cannot exceed more than 1000 rows
                                                                                                        </p>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <v-row>
                                                                                                    <v-col>
                                                                                                        <v-btn id="AutoTest_U87" color="grey" outlined="" dark="" class="elevation-0" height="40" @click="DownloadSampleFile()">
                                                                                                            <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                                                        </v-btn>
                                                                                                    </v-col>
                                                                                                    <v-col align-center="" justify-center="">
                                                                                                        <v-file-input prepend-icon="" label="Upload File" outlined="" dense=""
                                                                                                                      id="file" v-model="fileInput"
                                                                                                                      prepend-inner-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                                      @change="handleFileUpload">
                                                                                                        </v-file-input>
                                                                                                    </v-col>

                                                                                                </v-row>
                                                                                                <v-row v-if="csvfile.name">
                                                                                                    <v-col align-center="" justify-center="" class="pt-0">
                                                                                                        <v-chip id="AutoTest_U88" close="" label="" color="white" @click:close="csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null" small="">
                                                                                                            <v-icon size="20" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                                            {{ fileInput.name }}
                                                                                                        </v-chip>

                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-card-text>
                                                                                        </v-card>
                                                                                    </v-col>
                                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-1">
                                                                                        <Secondary-Button id="AutoTest_U89" class="btn-122x36 mr-4" title="Next" @click.native="ImportInviteUserSteps=2,fnCSVLoadParse()" v-bind:disabled="fileInput==null"></Secondary-Button>
                                                                                        <Default-Button-Outlined id="AutoTest_U90" class="btn-122x36" title="Cancel" @click.native="CloseInviteUserDialog(2)"></Default-Button-Outlined>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-container>
                                                                        </v-stepper-content>
                                                                        <v-stepper-content step="2" class="pt-0">
                                                                            <v-row no-gutters="">
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                                    <h4 class="heading-3"> We are running a format check this will take a moment</h4>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                                    <v-chip color="#FFE9E9" label="" dark="" v-if="roleErrorCount >0">
                                                                                        <v-icon size="30" color="red" height="100">mdi-alert-circle</v-icon>
                                                                                        <span style="color:#F55D5D">{{ roleErrorCount }} User Roles have not been set to proceed</span>
                                                                                    </v-chip>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                                    <v-data-table :headers="importInviteTableheaders"
                                                                                                  :items="csvfiledata"
                                                                                                  item-key="EmailId"
                                                                                                  height="25vh"
                                                                                                  fixed-header=""
                                                                                                  class="elevation-0"
                                                                                                  :footer-props="{
                                                                                              itemsPerPageOptions:[10,50,100,-1]
                                                                                              }"
                                                                                                  :items-per-page="10">
                                                                                        <template v-slot:item.UserRole="{item}">
                                                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                                                <v-select :items="userRoleTableItems"
                                                                                                          item-text="userRole"
                                                                                                          item-value="userRole"
                                                                                                          v-model="item.UserRole"
                                                                                                          label="Select User Role"
                                                                                                          outlined=""
                                                                                                          solo=""
                                                                                                          dense=""
                                                                                                          flat=""
                                                                                                          return-object=""
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
                                                                                    <v-row no-gutters="">
                                                                                        <v-col cols="12" xs="12" sm="8" md="8" class="py-0">
                                                                                            <p>
                                                                                                Invited users will be notified that they have added under respective user roles.
                                                                                            </p>
                                                                                        </v-col>
                                                                                        <v-col cols="12" xs="12" sm="4" md="4" class="pt-2">
                                                                                            <!-- <v-checkbox color="success-check-box" label="Notify user" input-value="true" v-model="chkNotify1" class="float-right"></v-checkbox> -->
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-col>
                                                                                <!--Footer Actions-->
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                    <Secondary-Button id="AutoTest_U91" class="btn-122x36 mr-4" title="Send" @click.native="SendInvite(2)" v-bind:disabled="(roleErrorCount >0) || (!chkNotify1)"></Secondary-Button>
                                                                                    <Default-Button-Outlined id="AutoTest_U92" class="btn-122x36" title="Previous" @click.native="ImportInviteUserSteps= 1"></Default-Button-Outlined>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>
                                                                        <v-stepper-content step="3" class="pa-0">
                                                                            <v-container fluid="" class="pt-0">
                                                                                <v-row no-gutters="" v-if="sentInvite==null">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium blinker">
                                                                                            Sending ....<br />
                                                                                        </p>
                                                                                        <v-progress-circular :size="50"
                                                                                                             color="primary"
                                                                                                             indeterminate>
                                                                                        </v-progress-circular>
                                                                                    </v-col>

                                                                                </v-row>
                                                                                <v-row no-gutters="" v-if="sentInvite">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                        <h4 class="text-center heading-3">Well Done !</h4>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-avatar size="150" tile="">
                                                                                            <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                                        </v-avatar>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium">
                                                                                            Your Invites have been sent out successfully.<br />Bask in your glory !
                                                                                        </p>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-btn id="AutoTest_U93" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(2)">Done</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row no-gutters="" v-else-if="!sentInvite  && sentInvite!=null">
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <p class="text-medium">
                                                                                            Sorry .. Failed to send Invites...<br />
                                                                                        </p>
                                                                                        <p class="text-medium">
                                                                                            {{retMsg}}<br />
                                                                                        </p>
                                                                                    </v-col>
                                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                        <v-btn id="AutoTest_U94" color="secondary" class="mr-2 elevation-0"
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
                                        <!--______________________________________-->
                                        <!--Bulk Action-->
                                        <v-menu offset-y="" transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                                            <template v-slot:activator="{ on }">
                                                <v-btn id="AutoTest_U95" height="39" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-4" @click="isBulkAction=true" v-bind:disabled="!screenAccess">
                                                    Bulk Actions
                                                    <v-icon>mdi-chevron-down</v-icon>
                                                </v-btn>

                                            </template>

                                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                                <v-list-item id="AutoTest_U96" link="" @click.stop="GetAllActiveUserGroup(),AssignBulkUserGroupDialog = true"
                                                             v-bind:disabled="isBulkAction && (userListInput==null ||  userListInput.length ==0)">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-account-multiple-plus-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Assign Group</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_U97" link="" @click.stop="AssignBulkUserRoleDialog = true" v-bind:disabled=" isBulkAction && (userListInput==null ||  userListInput.length ==0)">
                                                    <v-list-item-icon>
                                                        <v-icon>mdi-account-tie-outline</v-icon>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>Assign Role</v-list-item-content>
                                                </v-list-item>
                                            </v-list>

                                            <!-- Bulk Assign Group Dailog Box Starts here -->
                                            <v-dialog v-model="AssignBulkUserGroupDialog" width="800" persistent="">
                                                <v-card>
                                                    <v-card-title primary-title="" class="page-head">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                            <v-icon size="20" color="primary">mdi-account-multiple-outline</v-icon>
                                                        </v-avatar>
                                                        Bulk Assign User Group<v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_U98" text="" @click="CloseAssignGroup(2)">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>

                                                    <v-card-text>
                                                        <v-row no-gutters="">
                                                            <v-col cols="12" md="12" class="text-right">
                                                                <h5 class="text-left heading-4 mb-4 mt-2">
                                                                    <v-icon color="#52B962">
                                                                        mdi-plus-circle
                                                                    </v-icon>
                                                                    Add your users to one or more groups from below
                                                                </h5>
                                                                <v-text-field outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUserGroup"></v-text-field>
                                                            </v-col>

                                                            <v-col v-if="userGroupListInput.length>0" md="12">
                                                                <div class="chip-container mb-2">
                                                                    <!-- chip Additoin starts here -->
                                                                    <v-chip-group multiple="" column="" active-class="primary--text">
                                                                        <v-chip id="AutoTest_U99" close="" v-for="(item,idx) in userGroupListInput" :key="idx"
                                                                                class="ma-2" @click:close="removeUserGroup(item)" small="">
                                                                            <v-avatar size="12" left light :color="`${getcolor(item.GroupName)}`">
                                                                                <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.GroupName == null ? "" :item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            {{ item.GroupName }}
                                                                        </v-chip>
                                                                    </v-chip-group>
                                                                    <!-- chip Addtion ends here-->
                                                                </div>
                                                            </v-col>

                                                            <!-- chip Addtion ends here-->
                                                            <!--User Groups List-->
                                                            <v-col cols="12" md="12" sm="12" xs="12">
                                                                <v-data-table :headers="userGroupTableheaders"
                                                                              :items="userGroupTableItems"
                                                                              icon="mdi-account-multiple"
                                                                              show-select=""
                                                                              hide-default-footer=""
                                                                              item-key="GroupId"
                                                                              item-text="GroupName"
                                                                              class="elevation-0"
                                                                              v-model="userGroupListInput"
                                                                              :search="searchUserGroup"
                                                                              fixed-header=""
                                                                              height="25vh">

                                                                    <template v-slot:item.GroupName="{item}">
                                                                        <v-list class="py-0 table-v-list">
                                                                            <v-list-item class="my-0 px-0">
                                                                                <v-list-item-icon>
                                                                                    <v-avatar size="27" light :color="`${getcolor(item.GroupName)}`">
                                                                                        <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                                        <span class="white--text" v-else>  {{item.GroupName == null ? "" :item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                                    </v-avatar>
                                                                                </v-list-item-icon>
                                                                                <v-list-item-content>
                                                                                    {{item.GroupName}}
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                        </v-list>
                                                                    </template>
                                                                </v-data-table>

                                                            </v-col>
                                                            <v-col v-if="userGroupListInput.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                <v-row>
                                                                    <v-col v-if="userGroupListInput.length">
                                                                        <v-avatar size="25" class="team-avatar" v-for="(item,key) in userGroupListInput.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.GroupName)}`">
                                                                            <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                            <span class="white--text" v-else>{{item.GroupName == null ? "" : item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                        </v-avatar>
                                                                        <v-avatar v-if="userGroupListInput.length>=4" size="25" class="team-count">
                                                                            +{{userGroupListInput.length-3}}
                                                                        </v-avatar>
                                                                        The users will be added to  {{userGroupListInput.length}}  user groups.
                                                                    </v-col>
                                                                    <v-col>
                                                                        <!-- <v-checkbox color="success-check-box" class="float-right" label="Notify user" v-model="isNotify"></v-checkbox> -->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-0 mt-0">
                                                                <Secondary-Button id="AutoTest_U100" class="btn-122x36 mr-4" title="Confirm" @click.native="AssignUserToGroup(2,isNotify)" v-bind:disabled="userGroupListInput.length==0"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_U101" class="btn-122x36" title="Cancel" @click.native="CloseAssignGroup(2)"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                </v-card>
                                            </v-dialog>
                                            <!-- Bulk Assign Group Dailog Ends here -->
                                            <!-- Assign Bulk User Role New Dailogue starts here  -->
                                            <v-dialog v-model="AssignBulkUserRoleDialog" width="700" persistent="">
                                                <v-card>
                                                    <v-card-title primary-title="" class="page-head">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1 ml-3">
                                                            <v-icon size="20" color="primary">mdi-account-tie-outline</v-icon>
                                                        </v-avatar>
                                                        Bulk Assign User Role<v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_U103" text="" @click="CloseAssignUserRole(2),AssignUserRoleDialog=false">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>

                                                    <v-card-text class="pt-0">
                                                        <v-container fluid="" class="pt-0">
                                                            <v-row no-gutters="">
                                                                <h3 class="text-left heading-4 mb-3 mt-2">
                                                                    <v-icon color="#52B962">
                                                                        mdi-plus-circle
                                                                    </v-icon>
                                                                    Please assign a role to a user by selecting only one form the below
                                                                </h3>
                                                                <v-col cols="12" md="12" class="text-center mb-2">
                                                                    <v-text-field outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchRole" hide-details></v-text-field>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12">
                                                                    <v-data-table :headers="NewUserAssignRoleTableheaders"
                                                                                  :items="userRoleTableItems"
                                                                                  :single-expand="singleExpand"
                                                                                  :expanded.sync="expanded"
                                                                                  item-key="userRole"
                                                                                  :search="searchRole"
                                                                                  show-expand=""
                                                                                  class="elevation-0"
                                                                                  fixed-header=""
                                                                                  height="25vh">

                                                                        <template v-slot:item.userRole="{item}">
                                                                            <v-radio-group v-model="userRoleSelector"
                                                                                           class="custom-radio-btn-list-group">
                                                                                <v-radio color="success"
                                                                                         :label="` ${item.userRole}`"
                                                                                         :value="` ${item.userRoleID}`"
                                                                                         @change="getValue(item.userRoleID,item.userRole)"></v-radio>

                                                                            </v-radio-group>
                                                                        </template>


                                                                        <template v-slot:expanded-item="{ headers }">
                                                                            <td :colspan="headers.length">
                                                                                <v-data-table :headers="ScreenNameTableheaders"
                                                                                              :items="userScreenNameTableItems"
                                                                                              item-key="userScreenName"
                                                                                              class="elevation-0">
                                                                                    <template v-slot:item.Preview="">
                                                                                        <router-link to="">Proj.jpg</router-link>
                                                                                    </template>
                                                                                </v-data-table>
                                                                            </td>
                                                                        </template>

                                                                    </v-data-table>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="py-0" v-if="selectedNewRole!=''">
                                                                    <v-row no-gutters="">
                                                                        <v-col cols="12" xs="12" sm="8" md="8" class="py-0 my-0">
                                                                            <v-list>
                                                                                <v-list-item>
                                                                                    <v-list-item-icon>
                                                                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                            <v-icon size="15" color="primary">mdi-account-outline</v-icon>
                                                                                        </v-avatar>
                                                                                    </v-list-item-icon>

                                                                                    <v-list-item-content v-if="selectedNewRole!=''"> The users will be assigned the role of {{selectedNewRole}} </v-list-item-content>
                                                                                </v-list-item>
                                                                            </v-list>
                                                                        </v-col>
                                                                        <v-col cols="12" xs="12" sm="4" md="4">
                                                                            <!-- <v-checkbox color="success-check-box" dense="" label="Notify user" v-model="isNotify" class="float-right"></v-checkbox> -->
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-0 mt-0">
                                                                    <Secondary-Button id="AutoTest_U104" class="btn-122x36 mr-4" title="Confirm" @click.native="fnAssignUserRole(2,isNotify)" v-bind:disabled="!isRoleActive"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_U105" class="btn-122x36" title="Cancel" @click.native="CloseAssignUserRole(2),AssignBulkUserRoleDialog = false"></Default-Button-Outlined>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-card-text>
                                                    <v-divider></v-divider>
                                                </v-card>
                                            </v-dialog>
                                            <!-- Assign Bulk User Role Dailogue ends here ---->


                                        </v-menu>
                                        <!--Search-->
                                        <v-text-field label="Search"
                                                      class="mr-4 table-search"
                                                      dense hide-details
                                                      prepend-inner-icon="mdi-magnify"
                                                      outlined
                                                      v-model="SearchName"></v-text-field>
                                        <!--Filter-->
                                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                                            <template v-slot:activator="{ on }">
                                                <v-btn id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent,removeUserRoleDuplicates(userTableItems),removeUserOrganisationDuplicates(userTableItems)">
                                                    <v-icon>mdi-filter-outline</v-icon>
                                                    Filters
                                                </v-btn>
                                            </template>
                                        </v-menu>
                                    </v-row>
                                </v-col>
                            </v-row>

                            <v-row v-if="filterContent" no-gutters>
                                <h4 class="mx-4 my-2 page-head">
                                    Filters
                                </h4>
                                <!--Organisation-->
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="organisationMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Organisation
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>

                                    <div class="form-group form-check" v-for="item in uniqueOrganisationArray" v-bind:key="item">
                                        <v-div v-if="item">
                                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                                <v-checkbox color="success-check-box" input-value="true" v-model="userOrganisationcheckedCategories" :id="item" :value="item" :label="item"></v-checkbox>
                                            </v-list>
                                        </v-div>
                                    </div>
                                    <div class="d-inline-flex">
                                        <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="organisationMenu = false">
                                            Cancel
                                        </v-btn>
                                        <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnClickFilterDone(),organisationMenu = false">
                                            Done
                                        </v-btn>
                                    </div>

                                </v-menu>

                                <!-- User Role-->
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="userRoleMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            User Role
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>

                                    <div class="form-group form-check" v-for="item in uniqueUserRoleArray" v-bind:key="item">
                                        <v-div v-if="item">
                                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                                <v-checkbox color="success-check-box" input-value="true" v-model="userRoleCheckedCategories" :id="item" :value="item" :label="item"></v-checkbox>
                                            </v-list>
                                        </v-div>
                                    </div>

                                    <div class="d-inline-flex">
                                        <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-3" @click="userRoleMenu = false">
                                            Cancel
                                        </v-btn>
                                        <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnClickFilterDone(),userRoleMenu = false">
                                            Done
                                        </v-btn>
                                    </div>

                                </v-menu>
                                <!--state-->
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="stateMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            State
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-1">
                                        <v-checkbox color="success-check-box" v-model="checkactive" input-value="true" label="Active"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkinactive" input-value="true" label="Inactive"></v-checkbox>

                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="stateMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnClickFilterDone(),stateMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
                                    </v-list>
                                </v-menu>
                                <!--Invitation Status-->
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess" v-model="invitationStatusMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Invitation Status
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <v-checkbox color="success-check-box" input-value="true" label="Accepted"></v-checkbox>
                                        <v-checkbox color="success-check-box" input-value="true" label="Rejected"></v-checkbox>

                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="invitationStatusMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="invitationStatusMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
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

                            <v-row>
                                <v-col md="12" class="pt-2 text-left ml-2 py-0">
                                    <v-chip label="" outlined="" dark="" color="red"
                                            v-if="(isBulkAction && (userListInput ==null ||  userListInput.length ==0))">
                                        * Please select users from the list
                                    </v-chip>
                                </v-col>
                            </v-row>
                        </template>
                        <!--Group list column State-->
                        <template v-slot:item.State="{ item }">
                            <div class="pa-0 text-limit" style="width:115px">
                                <v-switch color="primary" v-model="item.UserStatus" :label="item.UserStatus ? 'ACTIVE': 'INACTIVE'"
                                          @change="GetAllUserProjects(item.ID,item.UserStatus,item.Name,item.EmailId),IsActiveToggleDialog = true,
                                     uName= item.Name, uPrjCount =item.userProjectCount" v-bind:disabled="!screenAccess">
                                </v-switch>
                            </div>
                        </template>
                        <template v-slot:item.Name="{item}">
                            <router-link class="text--default"
                                         link=""
                                         :to="{ name: 'userindividual', query: { WsID: WorkspaceID, Access:RWAccess, UID: item.ID} }">
                                <!-- ,params: {Name} -->
                                <v-list class="py-0 table-v-list">
                                    <v-list-item class="my-0 px-0">
                                        <div class="pa-0 text-limit" style="width:105px">
                                            <v-avatar size="30" class="elevation-1" light :color="`${getcolor(item.Name)}`">
                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                            </v-avatar>
                                            {{item.Name}}
                                        </div>
                                    </v-list-item>
                                </v-list>
                            </router-link>
                        </template>
                        <template v-slot:item.userActions="{item}">
                            <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                        <v-icon>mdi-dots-vertical</v-icon>
                                    </v-btn>
                                </template>

                                <v-list color="white" light="" width="auto" class="action-button-list">

                                    <!-- ---------- -->
                                    <!-- Assign Group -->
                                    <v-list-item id="AutoTest_U106" link="" @click.stop="GetAllActiveUserGroup(),AssignGroupDialog = true, uName =item.Name,uEmail= item.EmailId">
                                        <v-list-item-icon>
                                            <v-icon>mdi-account-multiple-plus-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>Assign Group</v-list-item-content>
                                    </v-list-item>
                                    <v-dialog v-model="AssignGroupDialog" width="800" persistent="">
                                        <v-card>
                                            <v-card-title primary-title="" class="page-head">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                    <v-icon size="20" color="primary">mdi-account-multiple-outline</v-icon>
                                                </v-avatar>
                                                Assign User Group<v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U107" text="" @click="CloseAssignGroup(1)">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>

                                            <v-card-text>
                                                <v-row no-gutters="">
                                                    <v-col cols="12" md="12" class="text-right">
                                                        <h5 class="text-left heading-4 mb-4 mt-2">
                                                            <v-icon color="#52B962">
                                                                mdi-plus-circle
                                                            </v-icon>
                                                            Add your users to one or more groups from below
                                                        </h5>
                                                        <v-text-field outlined dense hide-details label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUserGroup"></v-text-field>
                                                    </v-col>

                                                    <v-col v-if="userGroupListInput.length>0" md="12">
                                                        <div class="chip-container mb-2">
                                                            <!-- chip Additoin starts here -->
                                                            <v-chip-group multiple="" column="" active-class="primary--text">
                                                                <v-chip id="AutoTest_U108" close="" v-for="(item,idx) in userGroupListInput" :key="idx"
                                                                        class="ma-2" @click:close="removeUserGroup(item)" small="">
                                                                    <v-avatar size="12" left light :color="`${getcolor(item.GroupName)}`">
                                                                        <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                        <span class="white--text" v-else>  {{item.GroupName == null ? "" :item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    {{ item.GroupName }}
                                                                </v-chip>
                                                            </v-chip-group>
                                                            <!-- chip Addtion ends here-->
                                                        </div>
                                                    </v-col>

                                                    <!-- chip Addtion ends here-->
                                                    <!--User Groups List-->
                                                    <v-col cols="12" md="12" sm="12" xs="12" class="py-0">
                                                        <v-data-table :headers="userGroupTableheaders"
                                                                      :items="userGroupTableItems"
                                                                      icon="mdi-account-multiple"
                                                                      show-select=""
                                                                      hide-default-footer=""
                                                                      item-key="GroupId"
                                                                      class="elevation-0"
                                                                      v-model="userGroupListInput"
                                                                      :search="searchUserGroup"
                                                                      fixed-header=""
                                                                      height="25vh">

                                                            <template v-slot:item.GroupName="{item}">
                                                                <v-list class="py-0 table-v-list">
                                                                    <v-list-item class="my-0 px-0">
                                                                        <v-list-item-icon>
                                                                            <v-avatar size="27" light :color="`${getcolor(item.GroupName)}`">
                                                                                <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.GroupName == null ? "" :item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>
                                                                        <v-list-item-content>
                                                                            {{item.GroupName}}
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list>
                                                            </template>
                                                        </v-data-table>
                                                    </v-col>
                                                    <v-col v-if="userGroupListInput.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                        <v-row>
                                                            <v-col v-if="userGroupListInput.length">
                                                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in userGroupListInput.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.GroupName)}`">
                                                                    <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                    <span class="white--text" v-else>{{item.GroupName == null ? "" : item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                                <v-avatar v-if="userGroupListInput.length>=4" size="25" class="team-count">
                                                                    +{{userGroupListInput.length-3}}
                                                                </v-avatar>
                                                                {{uName}} will be added to  {{userGroupListInput.length}}  user groups
                                                                <!--<v-list>
                                                        <v-list-item>
                                                            <v-list-item-icon>
                                                                <v-avatar size="40">
                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                         alt="User DP" />
                                                                    <span class="black--text bottom"
                                                                          style="position:absolute; bottom:0px !important"></span>
                                                                </v-avatar>
                                                            </v-list-item-icon>
                                                            <v-list-item-content v-if="userGroupListInput.length>0">
                                                                {{uName}} will be added to  {{userGroupListInput.length}}  user groups
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list>-->
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="4" md="4">
                                                                <!-- <v-checkbox color="success-check-box" class="float-right" label="Notify user" v-model="isNotify"></v-checkbox> -->
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>

                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pb-4">
                                                        <Secondary-Button id="AutoTest_U109" class="btn-122x36 mr-4" title="Confirm" @click.native="AssignUserToGroup(1,isNotify,item.ID,item.Name,item.EmailId)" v-bind:disabled="userGroupListInput.length==0"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_U110" class="btn-122x36" title="Cancel" @click.native="CloseAssignGroup(1)"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-card-text>
                                        </v-card>
                                    </v-dialog>
                                    <!-- ---------- -->

                                    <v-list-item id="AutoTest_U111" link="" @click.stop="AssignUserRoleDialog = true,uName =item.Name,uEmail= item.EmailId" v-if="!item.UserRoleStatus">
                                        <!-- v-if = item.UserRoleStatus-->
                                        <v-list-item-icon>
                                            <v-icon>mdi-account-tie-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>Assign Role</v-list-item-content>
                                    </v-list-item>


                                    <!-- Assign User Role New Dailogue starts here  -->
                                    <v-dialog v-model="AssignUserRoleDialog" width="800" persistent="">
                                        <v-card>
                                            <v-card-title primary-title="" class="page-head ml-3">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-account-tie-outline</v-icon>
                                                </v-avatar>
                                                Assign User Role<v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U112" text="" @click="CloseAssignUserRole(1),AssignUserRoleDialog=false">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>

                                            <v-card-text class="pt-0">
                                                <v-container fluid="" class="py-0">
                                                    <v-row no-gutters="">
                                                        <h5 class="text-left heading-4 mb-4 mt-1">
                                                            <v-icon color="#52B962">
                                                                mdi-plus-circle
                                                            </v-icon>
                                                            Please assign a role to a user by selecting only one form the below
                                                        </h5>
                                                        <v-col cols="12" md="12" class="text-center mb-1">
                                                            <v-text-field outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchRole"></v-text-field>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                            <v-data-table :headers="NewUserAssignRoleTableheaders"
                                                                          :items="userRoleTableItems"
                                                                          :single-expand="singleExpand"
                                                                          :expanded.sync="expanded"
                                                                          item-key="userRole"
                                                                          :search="searchRole"
                                                                          show-expand=""
                                                                          class="elevation-0"
                                                                          fixed-header=""
                                                                          height="30vh">

                                                                <template v-slot:item.userRole="{item}">
                                                                    <v-radio-group hide-details dense v-model="userRoleSelector"
                                                                                   class="custom-radio-btn-list-group my-0">
                                                                        <v-radio color="success"
                                                                                 :label="` ${item.userRole}`"
                                                                                 :value="` ${item.userRoleID}`"
                                                                                 @change="getValue(item.userRoleID,item.userRole)"></v-radio>

                                                                    </v-radio-group>
                                                                </template>


                                                                <template v-slot:expanded-item="{ headers }">
                                                                    <td :colspan="headers.length">
                                                                        <v-data-table :headers="ScreenNameTableheaders"
                                                                                      :items="userScreenNameTableItems"
                                                                                      item-key="userScreenName"
                                                                                      class="elevation-0">
                                                                            <template v-slot:item.Preview="">
                                                                                <router-link to="">Proj.jpg</router-link>
                                                                            </template>
                                                                        </v-data-table>
                                                                    </td>
                                                                </template>

                                                                <!--<template v-slot:item.RowItemCancel>
                                                        <v-btn text @click="">
                                                            <v-icon size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </template>-->
                                                            </v-data-table>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="py-0" v-if="selectedNewRole!=''">
                                                            <v-row no-gutters="">
                                                                <v-col cols="12" xs="12" sm="8" md="8" class="py-0 my-0">
                                                                    <v-list class="py-0">
                                                                        <v-list-item>
                                                                            <v-list-item-icon>
                                                                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                    <v-icon size="15" color="primary">mdi-account-outline</v-icon>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content>
                                                                                {{uName}} will be assigned the role of {{selectedNewRole}}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="4" md="4">
                                                                    <!-- <v-checkbox color="success-check-box" label="Notify user" v-model="isNotify" class="float-right"></v-checkbox> -->

                                                                </v-col>
                                                            </v-row>
                                                        </v-col>

                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 my-0">
                                                            <Secondary-Button id="AutoTest_U113" class="btn-122x36 mr-4" title="Confirm" @click.native="fnAssignUserRole(1,isNotify,item.ID,item.Name,item.EmailId,userRoleSelector.value),snackbar =true" v-bind:disabled="!isRoleActive"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_U114" class="btn-122x36" title="Cancel" @click.native="CloseAssignUserRole(1),AssignUserRoleDialog = false"></Default-Button-Outlined>
                                                            <!--<v-snackbar v-model="snackbar"
                                                            :multi-line="multiLine">
                                                    {{ alertMsg }}
                                                    <v-btn color="red"
                                                            text
                                                            @click="snackbar = false">
                                                        Close
                                                    </v-btn>
                                                </v-snackbar>-->
                                                        </v-col>
                                                    </v-row>
                                                </v-container>


                                            </v-card-text>
                                            <v-divider></v-divider>
                                        </v-card>
                                    </v-dialog>
                                    <!-- Assign User Role Dailogue ends here ---->
                                    <!-- ---------- -->
                                    <v-list-item id="AutoTest_U115" link="" @click.stop="GetAllUserProjects(item.ID,item.UserStatus),DeleteUserDialog = true,
                uName =item.Name,uEmail= item.EmailId,uPrjCount =item.userProjectCount" v-if="!item.UserRoleStatus">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>Remove</v-list-item-content>
                                    </v-list-item>

                                    <!-- Remove User role Dialog starts here -->

                                    <v-dialog v-model="DeleteUserDialog" width="800" persistent="">
                                        <v-card>

                                            <v-card-title primary-title="" class="page-head py-4">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                </v-avatar>
                                                Remove User
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U116" text="" @click="DeleteUserDialog=false,uName =null,uEmail= null,uPrjCount=null">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>
                                            <v-card-text class="pt-0">
                                                <v-container fluid="" class="pt-0">
                                                    <v-row no-gutters="">
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <v-container fluid="" class="pt-0">
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

                                                                                                <!--<v-avatar size="20">
                                                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                                             alt="User DP" />
                                                                                        <span class="black--text bottom"
                                                                                              style="position:absolute; bottom:0px !important"></span>
                                                                                    </v-avatar>-->
                                                                                                {{uName}}
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
                                                                                          item-key="ProjectId"
                                                                                          fixed-header=""
                                                                                          height="25vh"
                                                                                          class="elevation-0">

                                                                                <template v-slot:body="{ items }">
                                                                                    <tbody>
                                                                                        <tr v-for="item in items" :key="item.ProjectId"
                                                                                            v-bind:style="{backgroundColor: ProjectRowColor[item.ProjectStatus]}">
                                                                                            <td>{{ item.ProjectName }}</td>
                                                                                            <td>{{ item.ProjectStatus }}</td>
                                                                                            <td>{{ item.Tasks }}</td>
                                                                                            <td>{{ item.Workflow }}</td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </template>

                                                                            </v-data-table>
                                                                        </v-col>

                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                            <label class="float-left mt-4">
                                                                                <!--<v-avatar size="20" color="#FFF0C7" class="elevation-0">
                                                                        <v-icon size="12" color="grey">mdi-account-outline</v-icon>
                                                                    </v-avatar>-->
                                                                                <!--<v-avatar size="20">
                                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                             alt="User DP" />
                                                                        <span class="black--text bottom"
                                                                              style="position:absolute; bottom:0px !important"></span>
                                                                    </v-avatar>-->
                                                                                {{uName}} will be notified
                                                                            </label>
                                                                            <!-- <v-checkbox color="success-check-box" label="Notify user" class="float-right" v-model="isNotify"></v-checkbox> -->
                                                                        </v-col>
                                                                    </div>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                        <Secondary-Button id="AutoTest_U117" class="btn-122x36 mr-4" title="Confirm" @click.native="fnDeleteUser(item.ID,item.Name,item.EmailId,isNotify,userTableItems.indexOf(item))"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U118" class="btn-122x36" title="Cancel" @click.native="DeleteUserDialog = false"></Default-Button-Outlined>
                                                                    </v-col>

                                                                </v-row>
                                                            </v-container>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-card-text>
                                        </v-card>
                                    </v-dialog>

                                    <v-dialog v-model="DeleteUserConfirmDialog" width="500">
                                        <v-card>
                                            <v-card-text class="pt-4">
                                                <v-alert v-model="delAlert" type="success">{{uName}} is removed.</v-alert>
                                            </v-card-text>
                                            <v-card-text class="text-right">
                                                <v-btn id="AutoTest_U119" color="primary" @click="fnDeleteItemfromList()">OK</v-btn>
                                            </v-card-text>
                                        </v-card>
                                    </v-dialog>
                                </v-list>
                            </v-menu>
                        </template>
                        <template v-slot:item.userGroup="{item}">
                            <div class="pa-0 text-limit user-group-cl" style="width:90px">{{item.userGroup}}</div>
                        </template>
                        <template v-slot:item.EmailId="{item}">
                            <div class="pa-0 text-limit " style="width:125px">{{item.EmailId}}</div>
                        </template>
                        <template v-slot:item.userOrganisation="{item}">
                            <div class="pa-0 text-limit " style="width:120px">{{item.userOrganisation}}</div>
                        </template>
                        <template v-slot:item.userRole="{item}">
                            <div class="pa-0 text-limit " style="width:90px">{{item.userRole}}</div>
                        </template>
                        <template v-slot:item.userProjectCount="{item}">
                            <div class="pa-0 text-limit " style="width:70px">{{item.userProjectCount}}</div>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>



                <!-- Remove User role Dialog for toggle state starts here -->
                <v-dialog v-model="IsActiveToggleDialog" width="800" persistent="">
                    <v-card class="elevation-0">
                        <v-card-title primary-title="" class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                <v-icon size="20" color="primary">mdi-electric-switch</v-icon>
                            </v-avatar>
                            Change User Status
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_U120" text="" @click="CloseStatusBtn()">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-card-text class="pt-0">
                            <v-container fluid="" class="py-0">
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <v-container fluid="" class="pt-0">
                                            <v-row>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <h4 class="text-center heading-3">Are you sure want to change user status ?</h4>
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
                                                                            {{uName}}
                                                                        </span>
                                                                        <span class="message">
                                                                            is part of  {{uPrjCount}}  projects by removing this user all active tasks will be inactive and will have to be reassigned at project setup.
                                                                        </span>
                                                                    </p>
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                        </v-list>
                                                    </v-col>

                                                    <v-col cols="12" xs="12" sm="12" md="12">

                                                        <v-data-table :headers="userProjectsTableheaders"
                                                                      :items="userProjectTableItems"
                                                                      item-key="ProjectId"
                                                                      fixed-header=""
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
                                                            </v-avatar>
                                                            {{uName}} will be notified
                                                        </label>
                                                        <!-- <v-checkbox label="Notify user" class="float-right" v-model="isNotify"></v-checkbox> -->
                                                    </v-col>
                                                </div>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                    <v-btn id="AutoTest_U121" color="secondary elevation-0" class="mr-2" @click="fnChangeActiveStatus(isNotify)">Confirm</v-btn>
                                                    <v-btn id="AutoTest_U122" outlined="" color="grey" @click="CloseStatusBtn()">Cancel</v-btn>
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
</v-card>
</template>


<script type="text/javascript">
  import SecondaryButton from '@/components/secondary-button.vue'
  import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
  import objUtils from '../../utils.js'

var objUser;
async function importscript() {
    return Promise.all([
        import("../../BL/User.js").then(mod => {
            objUser = new mod.User();
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
                filterContent: false,
                organisationMenu: false,
                userRoleMenu: false,
                stateMenu: false,
                invitationStatusMenu: false,
                snackbarMsg: false,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                dense: false,
                justifyCenter: "center",
                checkactive: false,
                checkinactive: false,
                filterChipsList: [],
                userRoleArray: [],
                uniqueUserRoleArray: [],
                userOrganisationArray: [],
                uniqueOrganisationArray: [],
                userOrganisationcheckedCategories: [],
                userRoleCheckedCategories: [],
                


                //===========UsersList==============

                userTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "Email Id", value: "EmailId" },
                    { text: "Organization", value: "userOrganisation" },
                    { text: "User Group", value: "userGroup", },
                    { text: "User Role", value: "userRole" },
                    { text: "Projects", value: "userProjectCount" },
                    { text: "State", value: "State", },
                    { text: "Actions", value: "userActions",align:"center", sortable: false }
                ],
                userTableItems: [],
                userListItems: [],
                FilterUserTableItems: [],
                
                //==================================

                expanded: [],
                singleExpand: false,

                //===========Invite Users==============
                emailIdError: 1,
                InviteUserDialog: false,
                NewEmail: "",
                reg: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/,

                emailRules: [
                    v => /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/.test(v),
                    // || 'Please enter valid E-Mail',
                ],

                chipItem: true,
                InviteUserListInput: [],
                InviteUsersInput: [],
                userListInput: [],
                emailErrors: [],

                clearable: false,
                chkNotify: true,
                chkNotify1: true,

                IsValidData: false,
                sentInvite: null,

                //===========Import users==============
                fileInput: null,
                csvfile: {
                    name: "",
                    size: 0,
                    namedisplay: ""
                },
                //uploadPercentage: 0,
                csvfiledata: [],
                importInviteTableheaders: [
                    { text: "Email", align: "left", value: "EmailId" },
                    { text: "User Role", value: "UserRole" },
                    { text: "", align: "left", color: "red", value: "Message" }
                ],
                roleErrorCount: 0,

                //------------------------------------------
                userRoleSelector: "",
                userRoleTableItems: [],
                //userRoleInput: [],
                AssignRoleTableHeaders: [
                    { text: 'Email', value: 'EmailId', },
                    { text: 'User Role', value: 'UserRole', },
                    { text: 'Screen Info', value: 'data-table-expand', width: "120px" },
                    { text: '', align: "left", color: "red", value: "Message", },
                    { text: '', value: 'RowItemCancel', align: "right", width: "10px" }

                ],
                ScreenNameTableheaders: [
                    { text: "Screen Name", align: "left", value: "userScreenName" },
                    { text: "Access Type", value: "userAccessType" },
                    { text: "Preview", value: "Preview" }
                ],
                userScreenNameTableItems: [],

                NewUserRegSteps: 0,
                ImportInviteUserSteps: 0,
                ImportInviteUser: false,
                //=================================
                delAlert: false,
                AssignRole: true,
                SearchName: "",
                SearchRoleName: "",
                searchUserGroup: "",
                searchRole: "",
                
                //===============Assign Group/Role || Bulk Actions===========================

                AssignBulkUserRoleDialog: false,
                AssignBulkUserGroupDialog: false,
                AssignUserRoleDialog: false,
                AssignGroupDialog: false,
                userGroupListInput: [],

                NewUserAssignRoleTableheaders: [
                    { text: "User Role", align: "left", value: "userRole", },
                    { text: 'Screen Info', value: 'data-table-expand' },
                ],

                userGroupTableheaders: [
                    {
                        text: "User Groups", align: "left", value: "GroupName"
                    },
                    { text: "Member Count", align: "center", value: "UsersCount" },
                    { text: "Project Count", align: "center", value: "ProjectCount" },

                ],
                userGroupTableItems: [],

                //===========Remove User==================================

                DeleteUserDialog: false,
                DeleteUserConfirmDialog: false,

                userProjectsTableheaders: [
                    { text: "Project Name", align: "left", value: "ProjectName" },
                    { text: "Status", value: "ProjectStatus" },
                    { text: "Tasks", value: "Tasks", sortable: "false" },
                    { text: "Workflow", value: "Tasks", sortable: "false" }
                ],
                userProjectTableItems: [],



                IsActiveToggleDialog: false,
                //ToggleDeleteDialog: false,

                //for toggle state changing
                tUserID: 0,
                tUserStatus: 0,
                tUserName: "",
                tEmail: "",

                isHidden: false,

                isBulkAction: false,
                chkEmails: false,
                isNotify: true,
                ChkNotified: false,
                isRoleActive: false,
                selectedNewRole: "",
                uName: "",
                uPrjCount: 0,
                uEmail: "",
                uGroups: "",
                //==========================================
                ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "" },
                //csvFileRowColor: { SelectUserRole: "##F55D5D" },
                Index: 0,
                retMsg: "",
                snackbar: false,
                alertMsg: "",
                opMsg: "",
                WorkspaceID: 0,
                UserID: 0,
                userimage: "",
                RandomColor: "",
                screenAccess: 0,
                RWAccess: 0,
               
            };
        },
        
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.UserID = this.$route.query.UID;
            await importscript();
            await this.GetAllUsers();
            await this.GetAllUsersList();
            await this.GetUserRoleDetails();
          
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
            ///  GetAllUsers :- Function used to fetch all users based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllUsers() {        
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID); 
                var result = await objUser.GetAllUsers(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else if (result.data) {
                    this.userTableItems = result.data;             
                    this.FilterUserTableItems = result.data;
                }
         
            },

            /// <summary>
            ///  GetAllUsersList :- Function used to fetch all user list based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  GetUserRoleDetails :- Function used to fetch user role details based on WorkspaceID
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
                else if (result.data) 
                    this.userRoleTableItems = result.data;        

            },

            /// <summary>
            ///  GetAllActiveUserGroup :- Function used to fetch user group details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllActiveUserGroup() {
    
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID); 
                var result = await objUser.GetAllActiveUserGroup(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else if (result.data) 
                    this.userGroupTableItems = result.data;
            },

            /// <summary>
            ///  ChangeNewRole :- Function used to change user role 
            /// </summary>
            /// <param name="objRole"></param>
            /// <param name="InvOrImp"></param>
            /// <returns> </returns>
            async ChangeNewRole(objRole,InvOrImp) {
        
                if (objRole.Message != null) {
                    objRole.Message = null;
                    this.roleErrorCount -= 1;
                }            
                if (InvOrImp ==1)
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
                else if (result.data) 
                    this.userScreenNameTableItems = result.data;  
                //console.log(this.userScreenNameTableItems)
            },

            /// <summary>
            ///  GetAllUserProjects :- Function used to fetch user project details based on userID
            /// </summary>
            /// <param name="Id"></param>
            /// <param name="userStatus"></param>
            /// <param name="uName"></param>
            /// <param name="uEmail"></param>
            /// <returns> </returns>
            async GetAllUserProjects(Id, userStatus, uName, uEmail) {
                this.tUserID = Id;
                this.tUserStatus = userStatus;
                this.tUserName = uName;
                this.tEmail = uEmail;

                var data = new FormData();
                data.append("userID", Id);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objUser.GetAllUserProjects(data);
       
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data)
                        this.userProjectTableItems = result.data;
                    if (this.userProjectTableItems.length == 0)
                        this.isHidden = true;
                    else
                        this.isHidden = false;
                }
            },

            /// <summary>
            ///  IsValidEmail :- Function used to check emailId is valid or not
            /// </summary>
            /// <param name="emailField"></param>
            /// <returns> </returns>
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
            ///  AddToList :- Function used to add emailId to the list while assign user by invite
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddToList() {
                if (this.IsValidEmail(this.NewEmail)) {
                    var usrEmail = this.userListItems.filter(x => x.EmailId == this.NewEmail);
                    if ((usrEmail.length == 0) || (usrEmail.length > 0 && usrEmail[0].IsDeleted == 1)) {
                        if ((this.NewEmail == "") ? false : (this.reg.test(this.NewEmail)) ? true : false) {
                            var isExist = this.InviteUserListInput.filter(x => x.EmailId == this.NewEmail);
                            if (isExist.length == 0) {
                                this.InviteUserListInput.push({ EmailId: this.NewEmail, Message: "Select User Role" });
                                this.roleErrorCount += 1;
                                this.chkEmails = true;
                            }
                            else {
                                this.Msg = "EmailId Already Added...";
                                this.infoSnackbarMsg = true;
                            }
                        }
                    }
                    else if (usrEmail.length > 0) {
                        if (usrEmail[0].IsDeleted == 0) {
                            if (usrEmail[0].IsRegisteredUser == 0)
                                this.Msg = "The EmailId already exisits...But not a Registered User ...";
                            else
                                this.Msg = "The EmailId already exisits...";
                            this.infoSnackbarMsg = true;
                        }
                    }
                    NewEmail: undefined;
                    this.NewEmail = "";
                    this.clearable = true;
                }
            },

            /// <summary>
            ///  remove :- Function used to remove email address
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async remove(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
                this.roleErrorCount -= 1;
            },

            /// <summary>
            ///  removeUserRoleDuplicates :- Function used to remove user role duplicates
            /// </summary>
            /// <param name="users"></param>
            /// <returns> </returns>
            removeUserRoleDuplicates(users) {
                this.userRoleArray = [];
                this.uniqueUserRoleArray = [];
                users.forEach((item, value) => {
                    
                    this.userRoleArray.push(item.userRole);
                });
               
                this.userRoleArray.forEach((c) => {
                    if (!this.uniqueUserRoleArray.includes(c)) {
                        this.uniqueUserRoleArray.push(c);
                    }
                    
                });

            },

            /// <summary>
            ///  removeUserOrganisationDuplicates :- Function used to remove user organisation duplicates
            /// </summary>
            /// <param name="users"></param>
            /// <returns> </returns>
            removeUserOrganisationDuplicates(users) {
                this.userOrganisationArray = [];
                this.uniqueOrganisationArray = [];
                users.forEach((item, value) => {

                    this.userOrganisationArray.push(item.userOrganisation);
                });

                this.userOrganisationArray.forEach((c) => {
                    if (!this.uniqueOrganisationArray.includes(c)) {
                        this.uniqueOrganisationArray.push(c);
                    }

                });

            },

            /// <summary>
            ///  IsValidInput :- Function used to check invite/import user validation 
            ///  invited/imported user(s) should have role
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
                    else {
                        if (result.status == "success") {
                            this.sentInvite = true;
                            this.retMsg = "Invitation Sent Successfully";
                            this.Msg = this.retMsg;
                            this.successSnackbarMsg = true;
                        }
                        else {
                            this.sentInvite = false;
                            this.retMsg = result.status;
                            this.Msg = this.retMsg;
                            this.errorSnackbarMsg = true;
                        }
                        this.GetAllUsers();
                        this.GetAllUsersList();
                    }
                }
            },

            /// <summary>
            ///  CloseInviteUserDialog :- Function used to close invite user dialog
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  getValue :- Function used to get selected user role id and name when click on radio button to assign new user role
            /// </summary>
            /// <param name="value"></param>
            /// <param name="rName"></param>
            /// <returns> </returns>
            getValue(value, rName) {    
                this.selectedNewRole = rName;
                this.userScreenNameTableItems = [];
                this.GetUserScreenDetails(value);
                this.isRoleActive = true;
            },

            /// <summary>
            ///  removeRole :- Function used to remove user role
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            removeRole(item) {        
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];

                if (item.Message != null)
                    this.roleErrorCount -= 1;        
            },

            /// <summary>
            /// fnAssignUserRole :- Function used to assign user role
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="ChkNotified"></param>
            /// <param name="Id"></param>
            /// <param name="uName"></param>
            /// <param name="uEmail"></param>
            /// <returns></returns>
            async fnAssignUserRole(Type, ChkNotified, Id, uName, uEmail) {
      
                var data = new FormData();
                var userRoleListInput = [];

                switch (Type) {
                    case 1: {
                        userRoleListInput.push({ userId: Id, userName: uName, EmailId: uEmail, userRoleID: this.userRoleSelector, userRole: this.selectedNewRole });
                        break;
                    }
                    case 2: {
                        if (this.userListInput.length != 0) {
                            for (var i = 0; i < this.userListInput.length; i++) {
                                userRoleListInput.push({
                                    userId: this.userListInput[i].ID,
                                    userName: this.userListInput[i].Name, EmailId: this.userListInput[i].EmailId,
                                    userRoleID: this.userRoleSelector,
                                    userRole: this.selectedNewRole
                                });
                            }
                        }
                        else
                            this.opMsg = "Please Select Users from List";
                        break;
                    }
                }
                if (this.opMsg == "") {
                    data.append("isNotified", ChkNotified);
                    data.append("userRoleListInput", JSON.stringify(userRoleListInput));
                    data.append("WorkspaceID", this.WorkspaceID);
                    var result = await objUser.AssignUserRole(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.status == "success")
                           this.opMsg = "User Role is assigned Successfully";
                        else
                           this.opMsg = "Failed :" + result.status;
                    }
                }
                this.Msg = this.opMsg;
                this.successSnackbarMsg = true;
                this.CloseAssignUserRole(Type);
            },

            /// <summary>
            ///  CloseAssignUserRole :- Function used to close assign user role dialog
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            CloseAssignUserRole(Type) {

                switch (Type) {
                    case 1:
                        this.AssignUserRoleDialog = false;
                        break;

                    case 2:
                        this.userListInput = [];                
                        this.isBulkAction = false;
                        this.AssignBulkUserRoleDialog = false;
                        break;
                }        
                this.userScreenNameTableItems = [];
                this.userRoleSelector = null;
                this.isRoleActive = false;
                this.selectedNewRole = "";
                this.uName = "";
                this.uEmail = "";
                this.GetAllUsers();
                this.opMsg = "";
            },

            /// <summary>
            ///  removeUserGroup :- Function used to remove user group from the list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            removeUserGroup(item) {
                this.userGroupListInput.splice(this.userGroupListInput.indexOf(item), 1);
                this.userGroupListInput = [...this.userGroupListInput];
            },

            /// <summary>
            ///  AssignUserToGroup :- Function used to assign users to group
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="ChkNotified"></param>
            /// <param name="Id"></param>
            /// <param name="uName"></param>
            /// <param name="uEmail"></param>
            /// <returns> </returns>
            async AssignUserToGroup(Type, ChkNotified, Id, uName, uEmail) {
                var userGroupInput = []; //out param
                var data = new FormData();
                var buserGroupInput = [];

                for (var i = 0; i < this.userGroupListInput.length; i++) {
                    buserGroupInput.push({ uGroupId: this.userGroupListInput[i].GroupId, uGroupName: this.userGroupListInput[i].GroupName });
                }
                switch (Type) {
                    case 1: {
                         userGroupInput.push({ userId: Id, userName: uName, EmailId: uEmail, uGroupList: buserGroupInput });
                         break;
                    }
                    case 2: {
                        if (this.userListInput.length != 0) {
                            for (var i = 0; i < this.userListInput.length; i++) {
                                userGroupInput.push({
                                    userId: this.userListInput[i].ID,
                                    userName: this.userListInput[i].Name,
                                    EmailId: this.userListInput[i].EmailId,
                                    uGroupList: buserGroupInput
                                });
                            }
                        }
                        else
                            this.opMsg = "Please Select Users from List";
                        break;
                    }
                }
                if (this.opMsg == "") {
                    data.append("isNotified", ChkNotified);
                    data.append("userGroupList", JSON.stringify(userGroupInput));
                    data.append("WorkspaceID", this.WorkspaceID);
                    var result = await objUser.AssignUserToGroup(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.status == "success") {
                            this.opMsg = "Successfully Assigned the Groups";
                            this.Msg = this.opMsg;
                            this.successSnackbarMsg = true;
                        }
                        else {
                            this.opMsg = "Failed :" + result.status;
                            this.Msg = this.opMsg;
                            this.errorSnackbarMsg = true;
                        }
                    }
                }
                this.CloseAssignGroup(Type);
            },

            /// <summary>
            ///  CloseAssignGroup :- Function used to close assign group
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            CloseAssignGroup(Type) {
                this.userGroupListInput = [];
                this.userGroupTableItems = [];
                switch (Type) {
                    case 1:
                        this.AssignGroupDialog = false;
                        break;

                    case 2:
                        this.userListInput = [];
                        this.isBulkAction = false;
                        this.AssignBulkUserGroupDialog = false;
                        break;
                }
                this.uName = "";
                this.uEmail = "";
                this.opMsg = "";
                this.GetAllUsers();
            },

            /// <summary>
            ///  fnChangeActiveStatus :- Function used to change status active / not based on userId
            /// </summary>
            /// <param name="isNotify"></param>
            /// <returns> </returns>
            async fnChangeActiveStatus(isNotify) {
       
                var data = new FormData();
                data.append("userId", this.tUserID);
                data.append("userStatus", this.tUserStatus);
                data.append("userName", this.tUserName);
                data.append("userEmailId", this.tEmail);
                data.append("ChkNotified", this.isNotify);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objUser.ChangeActiveStatus(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.opMsg = "User is" + ((this.tUserStatus == true) ? " Activated." : " Deactivated");
                        this.Msg = this.opMsg;
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.opMsg = "Failed : " + result.status;
                        this.Msg = this.opMsg;
                        this.errorSnackbarMsg = true;
                    }
                }
                this.CloseStatusBtn();
            },

            /// <summary>
            ///  CloseStatusBtn :- Function used to close status button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseStatusBtn() {       
                this.tUserID = 0;
                this.tUserStatus = null;
                this.tUserName = "";
                this.tEmail = "";
                this.uName = "";
                this.uEmail = "";
                this.uPrjCount = 0;
                this.isHidden = false;
                this.opMsg = "";

                this.IsActiveToggleDialog = false;
                this.GetAllUsers();
            },

            /// <summary>
            ///  fnDeleteUser :- Function used to delete user based on Id
            /// </summary>
            /// <param name="Id"></param>
            /// <param name="uName"></param>
            /// <param name="uEmail"></param>
            /// <param name="ChkNotified"></param>
            /// <param name="Index"></param>
            /// <returns> </returns>
            async fnDeleteUser(Id, uName, uEmail, ChkNotified, Index) {

                var data = new FormData();
                data.append("userId", Id);
                data.append("ChkNotified", ChkNotified);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objUser.DeleteUser(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.DeleteUserDialog = false;
                        this.DeleteUserConfirmDialog = true;
                        this.delAlert = true;
                        this.Index = Index;
                        this.GetAllUsersList();                
                    }
                    else 
                        this.delAlert = false;            
                }
            },

            /// <summary>
            ///  fnDeleteItemfromList :- Function used to close delete user confirm dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnDeleteItemfromList() {
                this.DeleteUserConfirmDialog = false;
                this.userTableItems.splice(this.Index, 1);
                this.uName = "";
                this.uEmail = "";
                this.uPrjCount = 0;
                this.delAlert = false;
                this.isHidden = false;
                this.opMsg = "";
            },

            //-------------------Import users

            /// <summary>
            ///  DownloadSampleFile :- Function used to download a sample file while user import
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            DownloadSampleFile() {
        
                var ColumnHeader = "EmailId,UserRole";
                var ColumnData1 = ["Email here","UserRole here"];
                var ColumnData = [];     

                ColumnHeader = ColumnHeader + "\n";
                ColumnData.push(ColumnData1);
                ColumnData.forEach(function (row) {
                   ColumnHeader += row.join(',');
                   ColumnHeader += "\n";
                });
                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_userslist_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            },

            /// <summary>
            ///  handleFileUpload :- Function used to handle file upload
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            handleFileUpload() {
                this.csvfile.name = this.fileInput.name;
                this.csvfile.size = this.fileInput.size;
                this.csvfile.namedisplay = this.csvfile.name + "\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0" + objUtils.common.formatFileSize(this.csvfile.size, 0);
            },

            /// <summary>
            ///  fnCSVLoadParse :- Function used to read CSV file
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCSVLoadParse() {

                if (this.ImportInviteUserSteps == 2 && this.csvfile.name) {
                    var vm = this
                    if (window.FileReader) {
                        var reader = new FileReader();
                        reader.readAsText(this.fileInput);
                        // Handle errors load
                        reader.onload = function (event) {
                            var csv = event.target.result;
                            vm.csvfiledata = vm.csvJSON(csv)
                        };
                        reader.onerror = function (evt) {
                            if (evt.target.error.name == "NotReadableError") {
                                this.Msg = "Cannot read file !";
                                this.infoSnackbarMsg = true;
                            }
                        };
                    }
                    else {
                        this.Msg = 'FileReader is not supported in this browser.';
                        this.infoSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  csvJSON :- Function used to read a csv file from excel
            /// </summary>
            /// <param name="csv"></param>
            /// <returns> </returns>
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
                        // if (filterrolename["userRoleID"] == "")

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

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter user based on user organisation, user role and status menu
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {

                this.filterChipsList = [];
                if (this.userOrganisationcheckedCategories.length == 0 && this.userRoleCheckedCategories.length == 0 && !this.checkactive && !this.checkinactive) {
                    this.FilterUserTableItems = this.userTableItems;
                }

                if ((!this.checkactive && this.checkinactive) || (this.checkactive && !this.checkinactive) || (this.checkactive && this.checkinactive) || ((this.userOrganisationcheckedCategories.length == 0) && (this.userRoleCheckedCategories.length == 0))) {
                    this.FilterUserTableItems = this.userTableItems;
                }


                // user Organization filter


                if (this.userOrganisationcheckedCategories.length > 0) {
                this.FilterUserTableItems = this.userTableItems.filter((x) => this.userOrganisationcheckedCategories.includes(x.userOrganisation));
                
                }
                
                if (this.userOrganisationcheckedCategories.length > 0) {
                    this.userOrganisationcheckedCategories.forEach((value, index) => {
                        this.filterChipsList.push({ ChipID: 999, ChipName: value });
                    
                })
            }
                 
                // user role filtration


                if (this.userOrganisationcheckedCategories.length == 0 && this.userRoleCheckedCategories.length > 0) {
                    this.FilterUserTableItems = this.userTableItems.filter((x) => this.userRoleCheckedCategories.includes(x.userRole));
                }
                else if (this.userRoleCheckedCategories.length > 0) {
                    this.FilterUserTableItems = this.FilterUserTableItems.filter((x) => this.userRoleCheckedCategories.includes(x.userRole));
                    
                }
                if (this.userRoleCheckedCategories.length > 0) {
                    this.userRoleCheckedCategories.forEach((value, index) => {
                        this.filterChipsList.push({ ChipID: 888, ChipName: value });

                    })
                }
                // status menu filtration


                if (this.checkactive && this.checkinactive && this.userOrganisationcheckedCategories.length == 0 && this.userRoleCheckedCategories.length == 0) {
                    this.FilterUserTableItems = this.userTableItems;
                }
                else if (this.checkactive) {
                    this.FilterUserTableItems = this.FilterUserTableItems.filter(x => x.UserStatus);
                }
                else if (this.checkinactive) {
                    this.FilterUserTableItems = this.FilterUserTableItems.filter(x => !x.UserStatus);
                }
                
                               
                if (this.checkactive) {
                    this.filterChipsList.push({ ChipID: 666, ChipName: "Active" });
                }
                if (this.checkinactive) {
                    this.filterChipsList.push({ ChipID: 777, ChipName: "Inactive" });
                }
                // status menu filtration ends here

                
                
            },

            /// <summary>
            ///  fnChipClose :- Function used to close the filter chip list
            /// </summary>
            /// <param name="chipitem"></param>
            /// <returns> </returns>
            fnChipClose(chipitem) {
                this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);

                if (chipitem.ChipName == "Active")
                    this.checkactive = false;
                if (chipitem.ChipName == "Inactive")
                    this.checkinactive = false;

                

                this.userOrganisationcheckedCategories.forEach((value, index) => {
                    if (chipitem.ChipName == value) {
                    this.userOrganisationcheckedCategories.splice(this.userOrganisationcheckedCategories.indexOf(value), 1)
                       
                    }
                    
                })

                this.userRoleCheckedCategories.forEach((value, index) => {
                    if (chipitem.ChipName == value) {
                        this.userRoleCheckedCategories.splice(this.userRoleCheckedCategories.indexOf(value), 1)

                    }

                })
                
                this.fnClickFilterDone();
                
            },
   
},


};
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
        
    .blinker {
        animation: blinker 1s linear infinite;
    }

    @keyframes blinker {
        50% {
            opacity: 0;
        }
    }
</style>








