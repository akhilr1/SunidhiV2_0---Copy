<template>
    <v-card class="elevation-0">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid>
            <v-row v-if="GroupId> 0 ">
                <v-col cols="12" xs="12" sm="12" md="12" class="mb-0">
                    <v-row no-gutters>
                        <h3 class="text-left page-head pl-3">
                            <v-avatar color="white" size="20" class="elevation-1 mr-1 mb-1">
                                <v-icon size="15" color="primary">mdi-account-group-outline</v-icon>
                            </v-avatar>
                            Team
                        </h3>
                        <v-spacer></v-spacer>
                        <!--Add User-->
                        <v-menu :offset-y=true v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="success-btn" dark class="elevation-0 mr-4">
                                    Add User
                                    <v-icon>mdi-chevron-down</v-icon>
                                </v-btn>
                            </template>
                            <!-- Added For List -->
                            <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                <!-- Invite User-->
                                <v-list-item id="AutoTest_IP799" link @click.stop="" @click="InviteUserDialog = true">
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
                                <v-list-item id="AutoTest_IP800" link @click.stop="ImportInviteUser = true">
                                    <v-list-item-icon>
                                        <v-icon>mdi-file-account-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>Import & Invite User</v-list-item-content>
                                </v-list-item>
                                <!-- Select User-->
                                <v-list-item id="AutoTest_IP801" link @click.stop="GetUsersListNotInTeam(),SelectUserDialog = true">
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
                        <!--Change Team-->
                        <v-btn id="AutoTest_IP802" height="39" color="success-btn" dark class="elevation-0 mr-4" @click="ChangeTeamDailog=true" v-bind:disabled="!screenAccess">
                            Change Team
                        </v-btn>
                        <!--Bulk Action -->
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                                    Bulk Actions
                                    <v-icon>
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item id="AutoTest_IP803" @click.stop="fnBulkRemoveClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                            <v-dialog v-model="RemoveBulkTeamDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove User
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP804" text="" @click="RemoveBulkTeamDailog =false,ProjectUserSelected=[],usrsProjectCount=0">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text class="pt-0">
                                        <v-container fluid class="pt-4">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-0">
                                                        <v-row>
                                                            <v-row v-if="usrsProjectCount>1" class="base-border-radius warning-div pa-0">
                                                                <v-col>
                                                                    <span class="message">  The selected users are part of multiple projects .. Deleting  thees users will affect to all other projects linked to .</span>

                                                                </v-col>
                                                            </v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <h4 class="text-center heading-3">Are you sure you want to remove selected users ?</h4>

                                                            </v-col>


                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_IP805" color="secondary elevation-0" class="mr-2" @click="fnDeleteTeamUser(1,0)">Remove</v-btn>
                                                                <v-btn id="AutoTest_IP806" outlined color="grey" @click="RemoveBulkTeamDailog =false,ProjectUserSelected=[],usrsProjectCount=0">Cancel</v-btn>
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
                        <v-text-field hide-details label="Search" class="mr-4 table-search" v-model="searchTeamMembers"
                                      dense prepend-inner-icon="mdi-magnify"
                                      outlined></v-text-field>
                        <!--Filter-->
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition">
                            <template v-slot:activator="{ on }">
                                <v-btn id="" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-2" @click="filterContent = !filterContent,removeOrganizationDuplicates(ProjectUserTeamTableitems),removeUserRoleDuplicates(ProjectUserTeamTableitems)">
                                    <v-icon>mdi-filter-outline</v-icon>
                                    Filters
                                </v-btn>
                            </template>
                        </v-menu>
                        <v-col md='12' v-if="filterContent">
                            <v-row class="mt-2">
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
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <div class="form-group form-check" v-for="item in uniqueOrganizationArray" v-bind:key="item">
                                            <v-div v-if="item">
                                                <v-checkbox color="success-check-box" input-value="true" v-model="userOrganizationCheckedCategories" :id="item" :value="item" :label="item"></v-checkbox>

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
                                    </v-list>
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
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <div class="form-group form-check" v-for="item in uniqueUserRoleArray" v-bind:key="item">
                                            <v-div v-if="item">
                                                <v-checkbox color="success-check-box" input-value="true" v-model="userRoleCheckedCategories" :id="item" :value="item" :label="item"></v-checkbox>
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
                                    </v-list>
                                </v-menu>
                            </v-row>                           
                        </v-col>
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
                        

                        <!-- ---------   Invite and Import User dailogues ----------   -->
                        <!-- Invite User -->
                        <v-dialog persistent v-model="InviteUserDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head pb-0">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                    </v-avatar>Invite User
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP807" text @click="CloseInviteUserDialog(1)">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="py-0">
                                    <v-container fluid class="pt-0">
                                        <v-row no-gutters>
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
                                                                        <Default-Button-Outlined id="AutoTest_IP810" class="btn-122x36" title="Cancel" @click.native="CloseInviteUserDialog(1)"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-stepper-content>

                                                        <v-stepper-content step="2" class="pa-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters class="py-0">
                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 py-0">
                                                                        <h4 class="text-center heading-3">Step 2: Assign user role and set access type for screens</h4>
                                                                    </v-col>
                                                                    <v-col cols="12" md="12" class="text-center py-0">
                                                                        <v-text-field outlined
                                                                                      dense
                                                                                      label="Search"
                                                                                      type="Text"
                                                                                      hide-details
                                                                                      class="mb-2"
                                                                                      prepend-inner-icon="mdi-magnify"
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
                                                                                      :single-expand="singleExpand"
                                                                                      :expanded.sync="expanded"
                                                                                      item-key="EmailId"
                                                                                      :search="SearchRoleName"
                                                                                      v-model="InviteUsersInput"
                                                                                      show-expand
                                                                                      hide-default-footer
                                                                                      class="elevation-0"
                                                                                      fixed-header
                                                                                      height="33vh">

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
                                                                                          style="width:170px"
                                                                                          @change="ChangeNewRole(item,1)"></v-select>

                                                                            </template>

                                                                            <template v-slot:expanded-item="{ headers }">
                                                                                <td :colspan="headers.length">
                                                                                    <v-data-table :headers="ScreenNameTableheaders"
                                                                                                  :items="userScreenNameTableItems"
                                                                                                  item-key="userScreenName"
                                                                                                  hide-default-footer
                                                                                                  class="elevation-0">
                                                                                        <template v-slot:item.Preview="">
                                                                                            <router-link to="" target="_blank">Proj.jpg</router-link>
                                                                                        </template>
                                                                                    </v-data-table>
                                                                                </td>
                                                                            </template>

                                                                            <template v-slot:item.RowItemCancel>
                                                                                <v-btn id="AutoTest_IP811" text @click="removeRole(InviteUserListInput.indexOf(item))">
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
                                                                        <Secondary-Button id="AutoTest_IP812" class="btn-122x36 mr-2" title="Next" @click.native="SendInvite(1),NewUserRegSteps = 3" v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_IP813" class="btn-122x36" title="Previous" @click.native="NewUserRegSteps = 1"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-stepper-content>

                                                        <v-stepper-content step="3" class="pa-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters v-if="sentInvite==null">
                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                        <p class="text-medium blinker">
                                                                            Sending...<br />
                                                                        </p>
                                                                        <v-progress-circular :size="50"
                                                                                             color="primary"
                                                                                             indeterminate>
                                                                        </v-progress-circular>

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
                                                                        <Secondary-Button id="AutoTest_IP814" class="btn-122x36" title="Done" @click.native="CloseInviteUserDialog(1)"></Secondary-Button>
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
                                                                        <Secondary-Button id="AutoTest_IP815" class="btn-122x36" title="OK" @click.native="CloseInviteUserDialog(1)"></Secondary-Button>
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
                        <v-dialog persistent v-model="ImportInviteUser" width="800">
                            <v-card>
                                <v-card-title primary-title="" class="page-head pb-0">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-account</v-icon>
                                    </v-avatar>Import & Invite User
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP816" text="" @click="CloseInviteUserDialog(2)">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>

                                <v-card-text class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-stepper v-model="ImportInviteUserSteps" class="elevation-0">
                                                    <v-stepper-header class="elevation-0 center-block" style="width:50%;">

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
                                                                                <v-row style="padding-left:4vw; padding-right:4vw">
                                                                                    <v-col align-center justify-center md="6">
                                                                                        <v-btn id="AutoTest_IP817" color="grey" outlined="" dark="" class="elevation-0" height="40" @click.stop=" downloadSampleFile()">
                                                                                            <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                    <v-col align-center justify-center md="6">
                                                                                        <v-file-input prepend-icon="" label="Upload File" outlined dense
                                                                                                      id="file" v-model="fileInput"
                                                                                                      prepend-inner-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                                      @change="handleFileUpload">
                                                                                        </v-file-input>
                                                                                    </v-col>

                                                                                </v-row>
                                                                                <v-row v-if="csvfile.name">

                                                                                    <v-col align-center="" justify-center="">
                                                                                        <v-chip id="AutoTest_IP818" close label color="white" @click:close="csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null">
                                                                                            <v-icon size="30" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                            {{ fileInput.name }}
                                                                                        </v-chip>

                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-card-text>
                                                                        </v-card>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_IP819" class="btn-122x36 mr-2" title="Next" @click.native="ImportInviteUserSteps=2,fnCSVLoadParse(),GetUserRoleDetails()" v-bind:disabled="fileInput==null"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_IP820" class="btn-122x36" title="Cancel" @click.native="CloseInviteUserDialog(2)"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-stepper-content>
                                                        <v-stepper-content step="2" class="pt-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                                        <h4 class="text-center heading-3"> We are running a format check this will take a moment</h4>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                        <v-chip color="#FFE9E9" label dark v-if="roleErrorCount >0">
                                                                            <v-icon size="30" color="red" height="100">mdi-alert-circle</v-icon>
                                                                            <span style="color:#F55D5D">{{ roleErrorCount }} User Roles have not been set to proceed</span>
                                                                        </v-chip>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12">
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
                                                                            <v-col cols="12" xs="12" sm="8" md="8" class="pt-4 pl-4">
                                                                                <p>
                                                                                    Invited users will be notified that they have added under respective user roles.
                                                                                </p>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="4" md="4" class="pt-2 pr-4">
                                                                                <!-- <v-checkbox color="success-check-box" label="Notify user" input-value="true" v-model="chkNotify1" class="float-right"></v-checkbox> -->
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <!--Footer Actions-->
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                        <Secondary-Button id="AutoTest_IP821" class="btn-122x36 mr-2" title="Send" @click.native="SendInvite(2)" v-bind:disabled="(roleErrorCount >0) || (!chkNotify1)"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_IP822" class="btn-122x36" title="Previous" @click.native="ImportInviteUserSteps= 1"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>                                                  
                                                        </v-stepper-content>
                                                        <v-stepper-content step="3" class="pa-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters v-if="sentInvite==null">
                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                        <p class="text-medium">
                                                                            Sending...<br />
                                                                        </p>
                                                                        <v-progress-circular :size="50"
                                                                                             color="primary"
                                                                                             indeterminate>
                                                                        </v-progress-circular>
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
                                                                        <Secondary-Button id="AutoTest_IP823" class="btn-122x36" title="Done" @click.native="CloseInviteUserDialog(2)"></Secondary-Button>
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
                                                                        <Secondary-Button id="AutoTest_IP824" class="btn-122x36" title="OK" @click.native="CloseInviteUserDialog(2)"></Secondary-Button>
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
                                    <v-btn id="AutoTest_IP825" text @click="SelectUserDialog=false">
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
                                                            <v-chip id="AutoTest_IP826" close v-for="item in SelectUserSelectedUsersList" :key="item.ID"
                                                                    v-model="chipUsrItem" @click:close="removeSelectedUserItem(item)" small>
                                                                <!--<v-icon left color="#707070">mdi-email</v-icon>-->
                                                                <v-avatar size="12" left :color="`${getcolor(item.Name)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
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
                                                                      item-key="ID"
                                                                      sort-by="Name"
                                                                      fixed-header
                                                                      height="32vh"
                                                                      :search="searchUser"
                                                                      v-model="SelectUserSelectedUsersList"
                                                                      class="elevation-0">

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
                                                            <!--if no data-->
                                                            <template v-slot:no-data>
                                                                <v-row class="justify-center" v-if="SelectUserTableItems.length == 0">
                                                                    <v-col class="text-center" md="12">
                                                                        <p class="mt-12 pt-12">
                                                                            <img src="..\..\..\src\assets\EmptyPlaceholders\Usergroup.svg" height="80" width="80" />
                                                                        </p>
                                                                        <p class="heading-4-dark font-weight-medium pt-0">No users available</p>
                                                                    </v-col>
                                                                </v-row>
                                                            </template>
                                                        </v-data-table>
                                                    </v-col>

                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-left" v-if="SelectUserSelectedUsersList.length>0">
                                                        <v-row>
                                                            <v-col cols="12" md="8" class="py-0" v-if="SelectUserSelectedUsersList.length">
                                                                <!--<v-list class="py-0">
                                                                    <v-list-item>
                                                                        <v-list-item-icon>
                                                                            <v-avatar size="20" color="#FFF0C7" class="elevation-0">
                                                                                <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>
                                                                        <v-list-item-content>
                                                                            {{SelectedUserNotifyMessage}}
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list>-->
                                                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in SelectUserSelectedUsersList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0"/>
                                                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                                <v-avatar v-if="SelectUserSelectedUsersList.length>=4" size="25" class="team-count">
                                                                    +{{SelectUserSelectedUsersList.length-3}}
                                                                </v-avatar>
                                                                <p>{{SelectedUserNotifyMessage}}</p>                
                                                            </v-col>
                                                            <v-spacer></v-spacer>
                                                            <v-col cols="12" md="4" class="py-0">
                                                                <!-- <v-checkbox input-value="true" color="success-check-box" label="Notify user" class="float-right"></v-checkbox> -->
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                        <Secondary-Button id="AutoTest_IP827" class="btn-122x36 mr-2" title="Save" @click.native="fnAddUserToGroup()" v-bind:disabled="SelectUserSelectedUsersList.length==0"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_IP828" class="btn-122x36" title="Cancel" @click.native="SelectUserDialog=false"></Default-Button-Outlined>
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
                <v-col md="12" class="py-0 pl-4" v-if="ProjectUserTeamTableitems.length>0">
                    <h3 class="text-left page-head">
                        {{GroupName}}
                    </h3>
                </v-col>
            </v-row>
            <!--Main Body-->

            <v-row v-if="GroupId> 0 && FilterProjectUserTeamTableitems.length>0">
                <v-col cols="12" md="12" sm="12" xs="12" class="mx-0 px-0">
                    <v-data-table :headers="ProjectUserTeamTableheaders"
                                  :items="FilterProjectUserTeamTableitems"
                                  show-select=""
                                  item-key="ID"
                                  v-model="ProjectUserSelected"
                                  :search="searchTeamMembers"
                                  fixed-header
                                  height="60vh"
                                  class="elevation-0">

                        <template v-slot:item.Name="{item}">
                            <v-list class="py-0 table-v-list">
                                <v-list-item class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:200px">
                                        <v-avatar size="30" light :color="`${getcolor(item.Name)}`">
                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                        {{item.Name}}
                                    </div>
                                </v-list-item>
                            </v-list>
                        </template>
                        <template v-slot:item.Organization="{ item }">
                            <div class="pa-0 text-limit" style="width:140px">
                                {{item.Organization == '' || item.Organization == undefined ? 'N/A':item.Organization }}
                            </div>
                        </template>
                        <template v-slot:item.Role="{ item }">
                            <div class="pa-0 text-limit" style="width:140px">
                                {{item.Role == '' || item.Role == undefined ? 'N/A':item.Role }}
                            </div>
                        </template>
                        <template v-slot:item.TaskCounts="{ item }">
                            <div class="pa-0 text-limit" style="width:100px">
                                {{item.TaskCounts == '' || item.TaskCounts == undefined ? 'N/A':item.TaskCounts }}
                            </div>
                        </template>
                        <template v-slot:item.Progress="{ item }">
                            <div class="pa-0 text-limit" style="width:150px">
                                {{item.Progress == '' || item.Progress == undefined ? 'N/A':item.Progress }}
                            </div>
                        </template>

                        <!--  Actions-->
                        <template v-slot:item.Actions="{item}">
                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">
                                    <v-list-item id="AutoTest_IP829" @click.stop="RemoveTeamDailog=true,usrProjectCount=item.ProjectCount,usrName=item.Name,usrID=item.ID ">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>
                                <v-dialog v-model="RemoveTeamDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove User
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP830" text="" @click="RemoveTeamDailog=false">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text class="pt-0">
                                            <v-container fluid class="pt-4">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-container fluid class="pt-0">

                                                            <v-row>
                                                                <v-row v-if="usrProjectCount>1" class="base-border-radius warning-div pa-0">
                                                                    <v-col>
                                                                        <span class="message">  {{usrName}}  is part of {{usrProjectCount}}  projects .. Deleting  the user will affect to all other projects linked to .</span>

                                                                    </v-col>
                                                                </v-row>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h3 class="text-center heading-2">Are you sure you want to remove this user ?</h3>
                                                                </v-col>

                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{usrName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <Secondary-Button id="AutoTest_IP831" class="btn-122x36 mr-2" title="Remove" @click.native="fnDeleteTeamUser(2,usrID)"></Secondary-Button>
                                                                    <Default-Button-Outlined id="AutoTest_IP832" class="btn-122x36" title="Cancel" @click.native="CancelRemove()"></Default-Button-Outlined>
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
            <!-- else part if No Team available-->
            <v-row v-else>
                <v-col cols="12" sm="12" md="12">
                    <v-row :justify="justifyCenter">
                        <v-col md="6" class="text-center">
                            <p class="mt-12 pt-12">
                                <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                            </p>

                            <p class="heading-3-dark font-weight-medium pt-6">No team data available</p>
                            <v-btn id="AutoTest_IP833" height="32" width="160" color="success-btn" dark class="elevation-0 mr-2" @click="ProjectAddTeamDialog = true" v-bind:disabled="!screenAccess">
                                Add Team
                            </v-btn>


                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
            <!-- Add Team dialog box-->
            <v-dialog v-model="ProjectAddTeamDialog" persistent width="800">
                <v-card>
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                            <v-icon size="20" color="primary">mdi-account-group-outline</v-icon>
                        </v-avatar>
                        Add Team
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP834" text="" @click="CloseTeamDialog()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-stepper v-model="AddTeamSteps" class="elevation-0">
                            <v-stepper-items>
                                <!----- ---------------  Create Group Section Starts here -->
                                <!--- Select Radio Button for Create Group or select Group-->
                                <v-stepper-content step="1" class="pa-0">
                                    <v-container fluid class="pt-4">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-2 px-4">
                                                            <h3 class="text-center heading-3">Step 1: Select how to add team to a project</h3>
                                                            <v-row class="heading-3" :justify="justifyCenter">
                                                                <v-radio-group @change="optUser=true" v-model="vmSelectUser">
                                                                    <v-radio label="Create Group" @change="CreateOrSelectGroup('Create')"></v-radio>
                                                                    <v-radio label="Select Group" @change="CreateOrSelectGroup('Select')"></v-radio>
                                                                </v-radio-group>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                            <Next-button-primary-light id="AutoTest_IP835" class="mt-0 mr-2" @click.native="NextStep()" v-bind:disabled="!optUser"></Next-button-primary-light>
                                                            <Cancel-Button-Outlined-Sm id="AutoTest_IP836" @click.native="ProjectAddTeamDialog= false"></Cancel-Button-Outlined-Sm>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>

                                <!-- Step 2 If Create Group Radio Button is selected-->
                                <v-stepper-content step="2" class="pa-0">
                                    <v-container fluid class="pt-2">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="mb-2">
                                                <h4 class="text-center heading-3">Step 2 : Select users to add them to the group</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="text-right">
                                                <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUser">
                                                </v-text-field>
                                            </v-col>
                                            <!--v-chip start here -->
                                            <v-col v-if="selectedUserList.length>0" md="12">
                                                <div class="chip-container mb-2">
                                                    <v-chip-group multiple
                                                                  column
                                                                  active-class="primary--text">
                                                        <v-chip id="" close v-for="(item,key) in selectedUserList" v-bind:key="item.ID"
                                                                @click:close="$delete(selectedUserList, key)" small>
                                                            <v-avatar size="12" left :color="`${getcolor(item.Name)}`">
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                            {{ item.Name }}
                                                        </v-chip>
                                                    </v-chip-group>
                                                </div>
                                            </v-col>
                                            <!--v-chip ends here -->
                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                <!--User List with checkbox add here-->
                                                <v-data-table :headers="userTableheaders"
                                                              :items="userTableItems"
                                                              show-select
                                                              item-key="UserID"
                                                              sort-by="Name"
                                                              :search="searchUser"
                                                              v-model="selectedUserList"
                                                              fixed-header
                                                              height="25vh"
                                                              class="elevation-0">

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
                                            <!-- Notify User List-->
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-left" v-if="selectedUserList.length>0">
                                                <v-row>
                                                    <v-col v-if="selectedUserList.length">
                                                        <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedUserList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                        <v-avatar v-if="selectedUserList.length>=4" size="25" class="team-count">
                                                            +{{selectedUserList.length-3}}
                                                        </v-avatar>
                                                        {{NotifyMessage}}
                                                    </v-col>
                                                    <v-col cols="12" md="4" class="py-0">
                                                        <!-- <v-checkbox input-value="true" label="Notify user" class="float-right"></v-checkbox> -->
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <Next-button-primary-light id="AutoTest_IP837" v-bind:disabled="selectedUserList.length==0" class="mt-0 mr-2" @click.native="AddTeamSteps = 3"></Next-button-primary-light>
                                                <Previous-Button-Outlined id="AutoTest_IP838" @click.native="AddTeamSteps = 1"></Previous-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>

                                <!-- Step 3 continue with Create Group-->
                                <v-stepper-content step="3" class="pa-0">
                                    <v-container fluid class="pt-4">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-2 px-4">
                                                            <h3 class="text-center heading-3">Step 3: Please add a Team name</h3>
                                                            <!-- Adding Avatar Icon-->
                                                            <v-col cols="12" md="12" class="text-center pt-2">
                                                                <v-avatar size="75" class="elevation-1 mt-2">
                                                                    <v-icon color="grey" size="60">mdi-account-group</v-icon>
                                                                </v-avatar>
                                                            </v-col>
                                                            <v-row class="heading-3" :justify="justifyCenter">
                                                                <v-col md="6">
                                                                    <v-text-field outlined label="Team name" type="Text" v-model="TeamName">
                                                                    </v-text-field>
                                                                </v-col>

                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                            <Next-button-primary-light id="AutoTest_IP839" v-bind:disabled="TeamName.length == 0 || TeamName.trim() == ''" class="mt-0 mr-2"
                                                                                       @click.native="AddTeamSteps = 5,fnCreateTeam()"></Next-button-primary-light>
                                                            <Previous-Button-Outlined id="AutoTest_IP840" @click.native="AddTeamSteps = 2"></Previous-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>

                                <!---- --------------- Create Group Section ends here -->
                                <!-- ----------  Select Group section starts here  ------------------------>
                                <!-- Select Group steps begin with stepper count 4 onwards upto 3 is for create Group-->

                                <v-stepper-content step="4" class="pa-0">
                                    <v-container fluid class="pt-2">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="mb-2">
                                                <h4 class="text-center heading-3">Step 2 : Select any one user group to add them to the project</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="text-right">
                                                <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchGroup">
                                                </v-text-field>
                                            </v-col>

                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                <v-data-table :headers="userGroupTableheaders"
                                                              :items="userGroupTableItems"
                                                              show-select
                                                              item-key="GroupId"
                                                              sort-by="GroupName"
                                                              single-select
                                                              :search="searchGroup"
                                                              v-model="selectedGroupList"
                                                              fixed-header
                                                              height="25vh"
                                                              class="elevation-0">

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
                                            <!-- Notify User List-->
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-left">
                                                <v-row>
                                                    <v-col cols="12" md="8" class="py-0" v-if="selectedGroupList.length>0">
                                                        <v-list class="py-0">
                                                            <v-list-item>
                                                                <v-list-item-icon>
                                                                    <v-avatar size="20" color="#FFF0C7" class="elevation-0">
                                                                        <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                    </v-avatar>
                                                                </v-list-item-icon>
                                                                <v-list-item-content>
                                                                    {{NotifyGroupMessage}}
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                        </v-list>
                                                    </v-col>

                                                </v-row>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <Next-button-primary-light id="AutoTest_IP841" v-bind:disabled="selectedGroupList.length==0" class="mt-0 mr-2"
                                                                           @click.native="AddTeamSteps = 5,fnUpdateGroupInProject()"></Next-button-primary-light>
                                                <Previous-Button-Outlined id="AutoTest_IP842" @click.native="AddTeamSteps = 1"></Previous-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>

                                <!---Select Group Steps ends here            -->
                                <!-- Invite or Skip users-->
                                <v-stepper-content step="5" class="pa-0">
                                    <v-container fluid class="pt-4">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-2 px-4">
                                                            <h3 class="text-center heading-3">Do you want to invite more users?</h3>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <v-btn id="AutoTest_IP843" color="secondary" class="mr-2 elevation-0" @click="AddTeamSteps = 6">Invite</v-btn>
                                                            <v-btn id="AutoTest_IP844" outlined color="grey" @click="CloseTeamDialog()">Skip</v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>


                                <v-stepper-content step="6" class="pa-0">
                                    <h4 class="text-center heading-3">Invite people by entering their mail address</h4>
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
                                                        <v-chip id="AutoTest_IP846" close v-for="item in InviteUserListInput" :key="item.EmailId"
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
                                        <v-btn id="AutoTest_IP847" v-bind:disabled="InviteUserListInput.length ==0" color="secondary" class="mr-2 elevation-0" @click="AddTeamSteps = 7">Next</v-btn>
                                        <v-btn id="AutoTest_IP848" outlined color="grey" @click="AddTeamSteps = 5">Previous</v-btn>

                                    </v-col>
                                </v-stepper-content>
                                <v-stepper-content step="7" class="pa-0">
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
                                                        <v-btn id="AutoTest_IP849" text @click="removeRole(InviteUserListInput.indexOf(item))">
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
                                                        <!-- <v-checkbox label="Notify user" input-value="true" v-model="chkNotify" class="float-right"></v-checkbox> -->
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                <v-btn id="AutoTest_IP850" color="secondary" class="mr-2 elevation-0" @click="SendInvite(1),AddTeamSteps = 8"
                                                       v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)">Send</v-btn>
                                                <v-btn id="AutoTest_IP851" outlined color="grey" @click="AddTeamSteps = 6">Previous</v-btn>

                                            </v-col>
                                        </v-row>
                                    </v-container>                                   
                                </v-stepper-content>
                                <v-stepper-content step="8" class="pa-0">
                                    <v-container fluid class="pt-0">
                                        <v-row no-gutters v-if="sentInvite==null">
                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                <p class="text-medium blinker">
                                                    Sending...<br />
                                                </p>
                                                <v-progress-circular :size="50"
                                                                     color="primary"
                                                                     indeterminate>
                                                </v-progress-circular>

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
                                                <v-btn id="AutoTest_IP852" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1),ProjectAddTeamDialog=false">Done</v-btn>
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
                                                <v-btn id="AutoTest_IP853" color="secondary" class="mr-2 elevation-0" @click="CloseInviteUserDialog(1),ProjectAddTeamDialog=false">OK</v-btn>
                                            </v-col>
                                        </v-row>

                                    </v-container>
                                </v-stepper-content>

                            </v-stepper-items>
                        </v-stepper>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <!-- Change Team dialog box-->
            <v-dialog v-model="ChangeTeamDailog" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-swap-horizontal</v-icon>
                        </v-avatar>
                        Change Team
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP854" text="" @click="ChangeTeamDailog=false">
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
                                                <h4 class="text-center heading-3">Are you sure you want to change this team ?</h4>
                                            </v-col>

                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_IP855" color="secondary elevation-0" class="mr-2" @click="ProjectAddTeamDialog=true,ChangeTeamDailog=false">Confirm</v-btn>
                                                <v-btn id="AutoTest_IP856" outlined color="grey" @click="ChangeTeamDailog=false">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
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
        </v-container>
    </v-card>
   
</template>
<script>
    import AddTagButton from '@/components/add-tag-button.vue'
    import SaveButtonSuccessSmall from '@/components/save-button-success-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'

    var objManageProject;
    async function importscript() {
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
    var objgroup;
    async function importGroupscript() {
        return Promise.all([
            import("../../BL/UserGroup.js").then(mod => {
                objgroup = new mod.UserGroup();
            })
        ]);
    };
    export default {
        data() {
            return {
                screenAccess: 0,               
                WorkspaceID: 0,
                InitiativeId: 0,
                ProjectId: 0,
                justifyCenter: 'center',
                PreLoader: false,
                searchInitiative: "",
                ImportInviteUser: false,
                InviteUserDialog: false,
                //InitiativeTableItems: [],
                ProjectAddTeamDialog: false,
                RemoveBulkTeamDailog: false,
                RemoveTeamDailog: false,
                ChangeTeamDailog: false,
                SelectUserDialog: false,
                filterContent: false,
                organisationMenu: false,
                userRoleMenu: false,
                AddTeamSteps: 0,
                AddTeamSelectedSteps: 0,
                optUser: false,
                vmSelectUser: false,
                searchUser: "",
                searchTeamMembers: "",
                selectedUserList: [],
                TeamName: "",
                GroupId: 0,
                GroupName: "",
                ProjectUserSelected: [],
                searchGroup: "",
                selectedGroupList: [],
                NotifyMessage: "",
                NotifyGroupMessage: "",
                SelectedUserNotifyMessage: "",
                /* Invite,Importand Select User details starts here */
                //----Select Users option
                SelectUserDialog: false,
                SearchRoleName: "",
                roleErrorCount: 0,
                SelectUserTableheaders: [
                    { text: "UserID", align: ' d-none', value: "UserID" },
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "userRole" },
                ],
                SelectUserTableItems: [],
                searchUser: "",
                SelectUserSelectedUsersList: [],
                chipUsrItem: "",
                //--------------------
                //===========Invite Users==============
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
                chkNotify1: true,
                ImportInviteUserSteps: 0,
                IsValidData: false,
                sentInvite: null,
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
                ProjectUserTeamTableheaders: [
                    { text: "UserID", align: ' d-none', value: "UserID" },
                    { text: "Name", align: "left", value: "Name" },
                    { text: "Organization", value: "Organization"},
                    { text: "User Role", value: "Role" },
                    { text: "Task Counts", value: "TaskCounts" },
                    { text: "Progress", value: "Progress" },
                    { text: "Actions", align: "center", value: "Actions",width:"100px" },
                ],
                ProjectUserTeamTableitems: [],
                FilterProjectUserTeamTableitems:[],
                userTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                userTableItems: [],
                userGroupTableheaders: [
                    { text: "User Groups", align: "left", value: "GroupName" },
                    { text: "Member Count", value: "UsersCount" },
                ],
                userGroupTableItems: [],
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
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                usrProjectCount: 0,
                usrID:0,
                usrName: "",
                usrsProjectCount: 0,
                userimage: "",
                userRoleArray : [],
                uniqueUserRoleArray: [],
                userOrganizationArray: [],
                uniqueOrganizationArray: [],
                filterChipsList: [],
                userRoleCheckedCategories: [],
                userOrganizationCheckedCategories: [],
            };
        },
        components: {
            'Add-Tag-Button': AddTagButton,
            'Save-Button-Success-Small': SaveButtonSuccessSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;           
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeId = this.$route.query.InitID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.ProjectId = this.$route.query.ProjID;
            await importscript();
            await importUserscript();
            await importGroupscript();
            this.PreLoader = true;
            await this.fnGetGroupDetailsByprjID();
            await this.fnGetTeamUsersList();
            await this.GetUserRoleDetails();
            this.PreLoader = false;
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
            ///  fnGetUsersListByID :- Function used to fetch user details based on WorkspaceID,InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersListByID() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("InitiativeId", this.InitiativeId);
                var result = await objManageProject.GetUsersListByID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {            
                        this.userTableItems = result.data;
                    }
            },

            /// <summary>
            ///  fnCreateTeam :- Function used to create a team 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateTeam() {
                var data = new FormData();
                data.append("GroupName", this.TeamName);
                data.append("UserList", JSON.stringify(this.selectedUserList));
                data.append("Notify", this.NotifyCheckbox);
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("ProjectId", this.ProjectId);
                data.append("InitID", this.InitiativeId);

                var result = await objManageProject.CreateTeam(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.fnGetGroupDetailsByprjID();
                    this.fnGetTeamUsersList()
                }
            },

            /// <summary>
            ///  fnGetTeamUsersList :- Function used to fetch groupwise user details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetTeamUsersList() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("GroupId", this.GroupId);
                data.append("InitiativeId", this.InitiativeId);
               // var result = await objManageProject.GetTeamUsersList(data);

                var result = await objManageProject.GetGroupUsersList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.ProjectUserTeamTableitems = result.data;
                        this.FilterProjectUserTeamTableitems = result.data;
                    }
            },

            /// <summary>
            ///  fnGetGroupDetailsByprjID :- Function used to fetch group details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.GroupId = result.data.GroupId;
                        this.GroupName = result.data.GroupName;
                    }
            },

            /// <summary>
            ///  fnBulkRemoveClick :- Function used to open remove dialog box, print a message if user is not selected to remove
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkRemoveClick() {
                
                if (this.ProjectUserSelected.length > 0) {
                    this.RemoveBulkTeamDailog = true;
                    for (var i = 0; i < this.ProjectUserSelected.length; i++)
                        if (this.ProjectUserSelected[i].ProjectCount > 1) {
                            this.usrsProjectCount +=1 ;
                           
                        }
                            
                } else {
                    this.Msg = "Please select the users";
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  CancelRemove :- Function used to clear remove dialog 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CancelRemove() {
                this.RemoveTeamDailog = false;
                this.usrProjectCount = 0;
                this.usrName = "";
            },

            /// <summary>
            ///  fnDeleteTeamUser :- Function used to delete team user details based on UserId
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="UserId"></param>
            /// <returns> </returns>
            async fnDeleteTeamUser(Type, UserId) {
              
                var data = new FormData();
                data.append("ProjectID", this.ProjectId);
                data.append("GroupId", this.GroupId);
                if (Type == 1)
                    data.append("UserID", JSON.stringify(this.ProjectUserSelected));
                else {
                    var lstUser = [];
                    lstUser.push({ ID: UserId });
                    data.append("UserID", JSON.stringify(lstUser)); 
                }      
                                   

                var result = await objManageProject.DeleteTeamUser(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {                        
                        this.Msg = "Successfully deleted";
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = "Failed to delete";
                        this.errorSnackbarMsg = true;
                    }
                }
                this.fnGetTeamUsersList();
                this.RemoveTeamDailog = false;
                this.RemoveBulkTeamDailog = false;
                this.ProjectUserSelected = [];

            },

            /// <summary>
            ///  fnGetGroupsListToProject :- Function used to fetch group details based on WorkspaceID when click on select group radio button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetGroupsListToProject() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetGroupsListToProject(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.userGroupTableItems = result.data;
                    }
            },

            /// <summary>
            ///  fnUpdateGroupInProject :- Function used to update group based on ProjectId 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnUpdateGroupInProject() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("GroupId", this.selectedGroupList[0].GroupId);
                var result = await objManageProject.UpdateGroupInProject(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.fnGetGroupDetailsByprjID();
                    }
                }
            },

            /// <summary>
            ///  fnAddUserToGroup :- Function used to add users to the group
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddUserToGroup() {
                var url = window.SERVER_URL + "/UserGroups/AddUserGroup";
                var data = new FormData();
                data.append("GroupId", this.GroupId);
                data.append("GroupName", this.GroupName);
                data.append("UserList", JSON.stringify(this.SelectUserSelectedUsersList));
                data.append("Notify", this.NotifyCheckbox);
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("ProjectId", this.ProjectId)
                var result = await objgroup.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.Msg = "Successfully added users to the group ";
                    this.successSnackbarMsg = true;
                }
                this.SelectUserDialog = false;
                this.SelectUserSelectedUsersList = [];
                this.fnGetTeamUsersList();
            },

            /// <summary>
            ///  CreateOrSelectGroup :- Function used to add a team based on type (Create group/Seclect group) 
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            CreateOrSelectGroup(type) {

                if (type == "Create") {
                    this.AddTeamSelectedSteps = 2;
                    this.fnGetUsersListByID();
                }
                if (type == "Select") {
                    this.AddTeamSelectedSteps = 4;
                    this.fnGetGroupsListToProject();
                }
            },

            /// <summary>
            ///  NextStep :- Function used to move to the next page with the selected steps
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            NextStep() {
                this.AddTeamSteps = this.AddTeamSelectedSteps;
            },

            /// <summary>
            ///  CloseTeamDialog :- Function used to clear add team dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseTeamDialog() {

                this.TeamName = "";
                this.selectedUserList = [];
                this.selectedGroupList = [];
                this.optUser = false;
                this.vmSelectUser = false;
                this.ProjectAddTeamDialog = false;
                this.fnGetTeamUsersList();
                this.AddTeamSteps = 1;
                this.ProjectUserSelected = [];

            },
            //*** Invite,Importand Select User details Starts here ***//

            /// <summary>
            ///  AddToList :- Function used to add user email to the list while inviting user
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddToList() {

                var msg = "The EmailId already exisits...";
                var usrEmail = this.ProjectUserTeamTableitems.filter(x => x.EmailId == this.NewEmail);

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
                    this.Msg = msg;
                    this.infoSnackbarMsg = true;
                }
                NewEmail: undefined;
                this.NewEmail = "";
                this.clearable = true;
            },

            /// <summary>
            ///  removemailfromchip :- Function used to remove email from chip
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async removemailfromchip(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
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

                    if (Type == 1) {
                        data.append("userInviteList", JSON.stringify(this.InviteUserListInput));                       
                    }
                    else if (Type == 2) {
                        data.append("userInviteList", JSON.stringify(this.csvfiledata));                        
                        this.ImportInviteUserSteps = 3;
                    }
                    data.append("WorkspaceId", this.WorkspaceID);
                    data.append("inviteFrom", "Project");
                    data.append("InitiativeId", this.InitiativeId);
                    data.append("ProjectID", this.ProjectId);

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

                    if (result.status == "success") {

                        this.GetUsersListNotInTeam();

                    }

                    this.fnGetTeamUsersList();


                }
            },

            /// <summary>
            ///  CloseInviteUserDialog :- Function used to close invite user dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseInviteUserDialog(Type) {
                switch (Type) {
                    case 1:
                        this.InviteUserListInput = [];
                        this.NewEmail = "";
                        this.NewUserRegSteps = 1;
                        this.userScreenNameTableItems = [];
                        this.InviteUserDialog = false;
                        this.ProjectAddTeamDialog = false;
                        this.AddTeamSteps = 1;
                        this.fnGetGroupDetailsByprjID
                        this.fnGetTeamUsersList();
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
            ///  GetUserRoleDetails :- Function used to fetch user role details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
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
            //-------------------Import users starts here

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
                                this.Msg = "Cannot read file !";
                                this.infoSnackbarMsg = true;
                            }
                        };
                    } else {
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
            /// <summary>
            ///  GetUsersListNotInTeam :- Function used to fetch users details not included in the group based on WorkspaceID,GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUsersListNotInTeam() {

                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                data.append("InitiativeId", this.InitiativeId)
                data.append("GroupId", this.GroupId);
                var result = await objManageProject.GetUsersListNotInTeam(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {                       
                        this.SelectUserTableItems = result.data;                       
                        this.AddInviteUserToTeam();
                    }

            },

            /// <summary>
            ///  AddInviteUserToTeam :- Function used to add invited users to the team
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddInviteUserToTeam() {
                if (this.SelectUserTableItems.length > 0 && this.InviteUserListInput.length > 0) {
                    var i = 0;
                    var j = 0;
                    for (i = 0; i <= this.SelectUserTableItems.length - 1; i++) {

                        for (j = 0; j <= this.InviteUserListInput.length - 1; j++) {
                            if (this.SelectUserTableItems[i].EmailId == this.InviteUserListInput[j].EmailId) {

                                this.SelectUserSelectedUsersList.push(this.SelectUserTableItems[i])
                            }
                        }
                    }
                    if (this.SelectUserSelectedUsersList.length > 0) {
                        this.fnAddUserToGroup();
                    }
                }
                else if (this.SelectUserTableItems.length > 0 && this.csvfiledata.length > 0) {
                    var i = 0;
                    var j = 0;
                    for (i = 0; i <= this.SelectUserTableItems.length - 1; i++) {

                        for (j = 0; j <= this.csvfiledata.length - 1; j++) {
                            if (this.SelectUserTableItems[i].EmailId == this.csvfiledata[j].EmailId) {

                                this.SelectUserSelectedUsersList.push(this.SelectUserTableItems[i])
                            }
                        }
                    }
                    if (this.SelectUserSelectedUsersList.length > 0) {
                        this.fnAddUserToGroup();
                    }
                }
            },

            /// <summary>
            ///  removeSelectedUserItem :- Function used to remove selected user from the list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async removeSelectedUserItem(item) {
                this.SelectUserSelectedUsersList.splice(this.SelectUserSelectedUsersList.indexOf(item), 1);
                this.SelectUserSelectedUsersList = [...this.SelectUserSelectedUsersList];
            },

            /// <summary>
            ///  downloadSampleFile :- Function used to download a sample file while user import
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            downloadSampleFile() {

                var csv = 'EmailId,UserRole\n';
                this.data.forEach(function (row) {
                    csv += row.join(',');
                    csv += "\n";
                });

                // console.log(Math.round(Math.random() * 1000));
                var hiddenElement = document.createElement('a');
                hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
                hiddenElement.target = '_blank';
                hiddenElement.download = 'sample_project_User_' + Math.round(Math.random() * 1000) + '.csv';
                hiddenElement.click();
            },
            //*** Invite,Importand Select User details ends here ***//

            removeUserRoleDuplicates(users) {
                
                this.userRoleArray = [];
                this.uniqueUserRoleArray = [];
                users.forEach((item, value) => {
                    this.userRoleArray.push(item.Role);
                });

                this.userRoleArray.forEach((c) => {
                    if (!this.uniqueUserRoleArray.includes(c)) {
                        this.uniqueUserRoleArray.push(c);
                    }
                });
            },

            removeOrganizationDuplicates(users) {

                this.userOrganizationArray = [];
                this.uniqueOrganizationArray = [];
                users.forEach((item, value) => {
                    if(item.Organization != '')
                    this.userOrganizationArray.push(item.Organization);
                });
                
                this.userOrganizationArray.forEach((c) => {
                    if (!this.uniqueOrganizationArray.includes(c)) {
                        this.uniqueOrganizationArray.push(c);
                    }
                });
            },

            fnClickFilterDone() {
                this.filterChipsList = [];


                if (this.userRoleCheckedCategories.length == 0 && this.userOrganizationCheckedCategories.length == 0) {
                    this.FilterProjectUserTeamTableitems = this.ProjectUserTeamTableitems;
                }
                //user Organization Filter
                if (this.userOrganizationCheckedCategories.length > 0 && this.userRoleCheckedCategories.length == 0) {
                    this.FilterProjectUserTeamTableitems = this.ProjectUserTeamTableitems.filter((x) => this.userOrganizationCheckedCategories.includes(x.Organization));
                }

                else if (this.userOrganizationCheckedCategories.length > 0 && this.userRoleCheckedCategories.length > 0) {
                    this.FilterProjectUserTeamTableitems = this.FilterProjectUserTeamTableitems.filter((x) => this.userOrganizationCheckedCategories.includes(x.Organization));
                }

                if (this.userOrganizationCheckedCategories.length > 0) {
                    this.userOrganizationCheckedCategories.forEach((value, index) => {
                        this.filterChipsList.push({ ChipID: 888, ChipName: value });

                    })
                }

                // user Role Filter
                if (this.userRoleCheckedCategories.length > 0 && this.userOrganizationCheckedCategories.length == 0) {
                    this.FilterProjectUserTeamTableitems = this.ProjectUserTeamTableitems.filter((x) => this.userRoleCheckedCategories.includes(x.Role));
                }

                else if (this.userRoleCheckedCategories.length > 0) {
                    this.FilterProjectUserTeamTableitems = this.FilterProjectUserTeamTableitems.filter((x) => this.userRoleCheckedCategories.includes(x.Role));
                }
                if (this.userRoleCheckedCategories.length > 0) {
                   this.userRoleCheckedCategories.forEach((value, index) => {
                   this.filterChipsList.push({ ChipID: 999, ChipName: value });

                   })
                }

            },

            fnChipClose(chipitem) {
                this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);

                this.userOrganizationCheckedCategories.forEach((value, index) => {
                    if (chipitem.ChipName == value) {
                        this.userOrganizationCheckedCategories.splice(this.userOrganizationCheckedCategories.indexOf(value), 1)

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

        
        watch: {
            /// <summary>
            ///  selectedUserList :- Function used to notify the users with a message when selecting users from the table under create group section
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            selectedUserList: function () {
                this.NotifyMessage = "";
                if (this.selectedUserList.length > 2) {
                    this.NotifyMessage = this.selectedUserList[0].Name + "," + this.selectedUserList[1].Name + " and " + (this.selectedUserList.length - 2) + " Other users will be added to the group";
                } else if (this.selectedUserList.length == 2) {
                    this.NotifyMessage = this.selectedUserList[0].Name + "," + this.selectedUserList[1].Name + " will be added to the group ";
                } else if (this.selectedUserList.length == 1) {
                    this.NotifyMessage = this.selectedUserList[0].Name + " will be added to the group";
                }
            },

            /// <summary>
            ///  SelectUserSelectedUsersList :- Function used to notify the users with a message when selecting users from the table under select user button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SelectUserSelectedUsersList: function () {
                this.SelectedUserNotifyMessage = "";
                if (this.SelectUserSelectedUsersList.length > 2) {
                    this.SelectedUserNotifyMessage = this.SelectUserSelectedUsersList[0].Name + "," + this.SelectUserSelectedUsersList[1].Name + " and " + (this.SelectUserSelectedUsersList.length - 2) + "Other users will be added to the team";
                } else if (this.SelectUserSelectedUsersList.length == 2) {
                    this.SelectedUserNotifyMessage = this.SelectUserSelectedUsersList[0].Name + "," + this.SelectUserSelectedUsersList[1].Name + " will be added to the team ";
                } else if (this.SelectUserSelectedUsersList.length == 1) {
                    this.SelectedUserNotifyMessage = this.SelectUserSelectedUsersList[0].Name + " will be added to the team";
                }
            },

            /// <summary>
            ///  selectedGroupList :- Function used to notify the users with a message when selecting users from the table under select group section
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            selectedGroupList: function () {
                this.NotifyGroupMessage = "";
                if (this.selectedGroupList.length > 0) {
                    this.NotifyGroupMessage = this.selectedGroupList[0].GroupName + " group will be added to the project";
                }
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
