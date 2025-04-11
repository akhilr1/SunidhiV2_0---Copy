<template>
    <v-card>
        <v-container fluid class="px-0">
            <v-row class="px-4">
                <v-col>
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary">mdi-folder-edit-outline</v-icon>
                        </v-avatar>
                        Draft Initiative
                    </h3>
                </v-col>
                <v-col cols="12" xs="12" sm="12" md="8">
                    <v-row>
                        <v-spacer></v-spacer>
                        <!--Create Initiative Button-->
                        <v-btn id="AutoTest_I33" height="39" color="success-btn" dark class="elevation-0 mr-4" @click="InitiativeDialog=true" v-bind:disabled ="!screenAccess">Create Initiatives</v-btn>

                        <v-dialog v-model="InitiativeDialog" persistent width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head pb-0">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-hexagon-multiple-outline</v-icon>
                                    </v-avatar>Create Initiative
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_I34" text @click="CloseInitiativeDialog()">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="py-0">
                                    <v-container fluid class="pt-0">
                                        <v-row no-gutters>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-stepper v-model="NewInitiativeSteps" class="elevation-0">
                                                    <v-stepper-header class="elevation-0">
                                                        <v-stepper-step :complete="NewInitiativeSteps > 1" step="1" color="inherit">Add Details</v-stepper-step>
                                                        <v-divider></v-divider>
                                                        <v-stepper-step :complete="NewInitiativeSteps > 2" step="2" color="inherit">Add Users</v-stepper-step>
                                                        <v-divider></v-divider>
                                                        <v-stepper-step :complete="NewInitiativeSteps > 3" step="3" color="inherit">Project Template</v-stepper-step>
                                                        <v-divider></v-divider>
                                                        <v-stepper-step :complete="NewInitiativeSteps > 4" step="4" color="inherit">Workflow</v-stepper-step>
                                                    </v-stepper-header>

                                                    <v-stepper-items>
                                                        <!-- step 1 Add Details-->
                                                        <v-stepper-content step="1" class="pa-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <h4 class="text-center heading-3">Step 1: Please enter the basic details to create a new Initiative</h4>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                        <v-row :justify="justifyCenter">
                                                                            <v-col md="7">
                                                                                <v-text-field outlined dense label="Initiative Name" type="text" v-model="initName" :rules="alphanumericRules">
                                                                                </v-text-field>
                                                                                <v-textarea rows="3" outlined dense label="Description" type="text" v-model="initDescription">
                                                                                </v-textarea>
                                                                                <v-text-field outlined dense label="Code" v-model="initCode" v-bind:disabled="true">
                                                                                </v-text-field>

                                                                                <!-- Tag Section modified-->
                                                                                <v-row class="mb-1">
                                                                                    <v-col class="text-left d-inline-flex" md="12">
                                                                                        <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                                                        <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                            <template v-slot:activator="{ on }">
                                                                                                <v-btn id="AutoTest_I35" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                                                                       @click="fnInitializeSteper()" class="mr-1">
                                                                                                    <v-icon size="15">mdi-plus</v-icon>
                                                                                                </v-btn>
                                                                                            </template>
                                                                                            <v-card>
                                                                                                <v-card-text class="pt-0 pb-1">
                                                                                                    <v-col cols="12" md="12">
                                                                                                        <!-- Header Text  -->
                                                                                                        <v-col cols="12" md="12" class="d-inline-flex">
                                                                                                            <v-btn id="AutoTest_I36" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                                                <v-icon size="15">mdi-chevron-left</v-icon>
                                                                                                            </v-btn>
                                                                                                            <v-spacer></v-spacer>
                                                                                                            <h3 class="text-left page-head pt-2">
                                                                                                                {{TagHeaderName}}
                                                                                                            </h3>
                                                                                                            <v-spacer></v-spacer>
                                                                                                            <v-btn id="AutoTest_I37" text @click="NewTagMenu = false">
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
                                                                                                                            <v-btn id="AutoTest_I38" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                                                        </v-col>
                                                                                                                    </v-row>
                                                                                                                </v-stepper-content>

                                                                                                                <!-- Select from existing Tags-->
                                                                                                                <v-stepper-content step="2" class="pa-0">

                                                                                                                    <v-row style="max-height:30vh;overflow-y:scroll">
                                                                                                                        <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                                                            <p class="mb-2">Search Tags</p>
                                                                                                                            <v-text-field outlined="" dense="" label="Search tag here" hide-details class="mb-3">
                                                                                                                            </v-text-field>
                                                                                                                            <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                                                                                                                                <v-chip id="AutoTest_I39" v-for="index in AllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                                                        label @click.native="fnAddToSelectedTag(index)">
                                                                                                                                    {{ index.TagName }}
                                                                                                                                    <v-icon id="AutoTest_I40" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                                                                </v-chip>
                                                                                                                            </v-chip-group>
                                                                                                                        </v-col>
                                                                                                                    </v-row>
                                                                                                                    <v-col cols="12" md="12" class="text-center mt-3">
                                                                                                                        <v-btn id="AutoTest_I41" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                                                                            <v-btn id="AutoTest_I42" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                                                        <v-btn id="AutoTest_I43" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                                                        </v-btn>
                                                                                        <v-btn id="AutoTest_I44" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                                                    <v-col md="12" class="chip-container mb-0 pt-0">
                                                                                        <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                                                            <v-chip id="AutoTest_I45" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                                                                {{TagItem.TagName}}
                                                                                            </v-chip>
                                                                                        </v-chip-group>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_I46" class="btn-122x36 mr-4" title="Next" @click.native="NewInitiativeSteps = 2,ChangeTab(1),
                                                                                            InitiativeAddUserSteps =1,InitiativeAddUserGroupSteps=1" v-bind:disabled="initName.length == 0 || initName.trim() == ''"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_I47" class="btn-122x36" title="Cancel" @click.native="CloseInitiativeDialog()" "></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-stepper-content>

                                                        <!-- step 2 Add Users-->
                                                        <v-stepper-content step="2" class="pa-0">
                                                            <!-- Select user tab-->
                                                            <v-tabs centered v-model="Tab" color="tab-active-line">
                                                                <v-tab href='#tab-1'> Users </v-tab>
                                                                <v-tab href='#tab-2'>User Groups</v-tab>
                                                                <!-- Users Tab Item-->
                                                                <v-tab-item value="tab-1">
                                                                    <v-card flat>
                                                                        <v-card-text class="pt-0">
                                                                            <v-row no-gutters>
                                                                                <v-col cols="12" md="12" class="pt-0">
                                                                                    <v-row :justify="justifyCenter">
                                                                                        <!--<v-col md="6">-->
                                                                                        <v-container fluid>
                                                                                            <v-stepper hide v-model="InitiativeAddUserSteps" class="elevation-0">

                                                                                                <v-stepper-items>
                                                                                                    <v-stepper-content step="1" class="pa-0">
                                                                                                        <h4 class="text-center heading-3">Step 2 : Select how to add users to a initiative</h4>
                                                                                                        <v-col cols="12" md="12" class="pt-2 px-4">
                                                                                                            <v-row class="heading-3" :justify="justifyCenter">
                                                                                                                <v-radio-group @change="optUser=true" v-model="vmSelectUser">
                                                                                                                    <v-radio label="Invite User" @change="InviteOrSelectUser('Invite')"></v-radio>
                                                                                                                    <v-radio label="Import & Invite User" @change="InviteOrSelectUser('Import')"></v-radio>
                                                                                                                    <v-radio label="Select User" @change="InviteOrSelectUser('Select')"></v-radio>
                                                                                                                </v-radio-group>
                                                                                                            </v-row>
                                                                                                        </v-col>
                                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                                            <Secondary-Button id="AutoTest_I48" class="btn-122x36 mr-4" title="Next" @click.native="NextStep()" v-bind:disabled="!optUser"></Secondary-Button>
                                                                                                            <Default-Button-Outlined id="AutoTest_I49" class="btn-122x36" title="Previous" @click.native="NewInitiativeSteps = 1"></Default-Button-Outlined>
                                                                                                        </v-col>
                                                                                                    </v-stepper-content>
                                                                                                    <!-- if selected Radio Button is invite User-->
                                                                                                    <v-stepper-content step="2" class="pa-0">
                                                                                                        <h4 class="text-center heading-3">Step 2 : Invite people by entering their mail address</h4>
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
                                                                                                                    <div class="chip-container mb-2" v-if="InviteUserListInput.length>0">
                                                                                                                        <v-chip-group multiple
                                                                                                                                      column
                                                                                                                                      active-class="primary--text">
                                                                                                                            <v-chip id="AutoTest_I50" close v-for="(item,idx) in InviteUserListInput" :key="idx"
                                                                                                                                    v-model="chipItem"
                                                                                                                                    @click:close="removeEmail(item)">
                                                                                                                                <v-icon left color="#707070">mdi-email</v-icon>
                                                                                                                                {{ item.EmailId }}
                                                                                                                            </v-chip>

                                                                                                                        </v-chip-group>
                                                                                                                    </div>
                                                                                                                </v-col>
                                                                                                            </v-row>
                                                                                                        </v-col>
                                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                                            <Secondary-Button id="AutoTest_I51" class="btn-122x36 mr-4" title="Next" @click.native="InitiativeAddUserSteps = 3,GetUserRoleDetails()" v-bind:disabled="InviteUserListInput.length ==0"></Secondary-Button>
                                                                                                            <Default-Button-Outlined id="AutoTest_I52" class="btn-122x36" title="Previous" @click.native="InitiativeAddUserSteps = 1"></Default-Button-Outlined>
                                                                                                        </v-col>
                                                                                                    </v-stepper-content>
                                                                                                    <v-stepper-content step="3" class="pa-0">
                                                                                                        <v-row no-gutters>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                                                <h4 class="text-center heading-3">Step 2: Assign user role and set access type for screens</h4>
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
                                                                                                                              item-key="EmailId"
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

                                                                                                                    <template v-slot:item.RowItemCancel="{item}">
                                                                                                                        <v-btn id="AutoTest_I53" text @click="removeRole(item)">
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
                                                                                                                    <v-col cols="12" xs="12" sm="4" md="4" class="pt-0">
                                                                                                                        <v-checkbox label="Notify user" input-value="true" v-model="chkNotify" class="float-right" color="success-check-box"></v-checkbox>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                                                <Secondary-Button id="AutoTest_I54" class="btn-122x36 mr-4" title="Send" @click.native="SendInvite(1)" v-bind:disabled="(roleErrorCount >0) || (!chkNotify) || (!chkEmails)"></Secondary-Button>
                                                                                                                <Default-Button-Outlined id="AutoTest_I55" class="btn-122x36" title="Previous" @click.native="InitiativeAddUserSteps = 2"></Default-Button-Outlined>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>

                                                                                                    <!-- if selected Radio Button is User select User-->
                                                                                                    <v-stepper-content class="pa-0" step="4">
                                                                                                        <v-row no-gutters>
                                                                                                            <v-col cols="12" md="12" class="pt-0">
                                                                                                                <v-container fluid class="pt-2 px-1">
                                                                                                                    <v-row no-gutters>
                                                                                                                        <v-col cols="12" md="12" class="text-right">
                                                                                                                            <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUser">
                                                                                                                            </v-text-field>
                                                                                                                        </v-col>
                                                                                                                        <!-- v-chip for Assign roless-->
                                                                                                                        <div class="chip-container mb-2" v-if="selectedUserList.length>0">
                                                                                                                            <v-chip-group multiple column active-class="primary--text">
                                                                                                                                <v-chip id="AutoTest_I56" close v-for="item in selectedUserList" :key="item.UserID"
                                                                                                                                        @click:close="removeUserItem(item)" class="ma-2" small>
                                                                                                                                    <v-icon left color="#707070" size="15">mdi-account</v-icon>
                                                                                                                                    {{ item.Name }}
                                                                                                                                </v-chip>
                                                                                                                            </v-chip-group>

                                                                                                                        </div>
                                                                                                                        <!--v-chip for Assign roles ends here -->
                                                                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                                                                            <!--User List with checkbox add here-->
                                                                                                                            <v-data-table :headers="userTableheaders"
                                                                                                                                          :items="userTableItems"
                                                                                                                                          show-select
                                                                                                                                          item-key="UserID"
                                                                                                                                          sort-by="Name"
                                                                                                                                          fixed-header
                                                                                                                                          height="25vh"
                                                                                                                                          :search="searchUser"
                                                                                                                                          v-model="selectedUserList"
                                                                                                                                          class="elevation-0">
                                                                                                                            </v-data-table>
                                                                                                                        </v-col>
                                                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                                                            <Secondary-Button id="AutoTest_I57" class="btn-122x36 mr-4" title="Next" @click.native="InitiativeAddUserSteps = 7" v-bind:disabled="selectedUserList.length==0"></Secondary-Button>
                                                                                                                            <Default-Button-Outlined id="AutoTest_I58" class="btn-122x36" title="Previous" @click.native="InitiativeAddUserSteps = 1"></Default-Button-Outlined>
                                                                                                                        </v-col>
                                                                                                                    </v-row>
                                                                                                                </v-container>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>

                                                                                                    <!-- if Selected Radio button is Import user-->
                                                                                                    <!-- Upload section for user-->
                                                                                                    <v-stepper-content step="5" class="pt-0">
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
                                                                                                                                    <v-chip close label id="AutoTest_I59" color="white" @click:close="csvfile.name=null,csvfile.size=0,csvfile.namedisplay=null">
                                                                                                                                        <v-icon size="30" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                                                                        {{ fileInput.name }}
                                                                                                                                    </v-chip>

                                                                                                                                </v-col>
                                                                                                                            </v-row>
                                                                                                                        </v-card-text>
                                                                                                                    </v-card>
                                                                                                                </v-col>
                                                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right">

                                                                                                                    <v-btn id="AutoTest_I60" color="secondary elevation-0" class="mr-2" @click="InitiativeAddUserSteps = 6,fnCSVLoadParse()"
                                                                                                                           v-bind:disabled="fileInput==null">Next</v-btn>
                                                                                                                    <v-btn id="AutoTest_I61" outlined="" color="grey" @click="InitiativeAddUserSteps = 1">Previous</v-btn>
                                                                                                                </v-col>
                                                                                                            </v-row>
                                                                                                        </v-container>
                                                                                                    </v-stepper-content>
                                                                                                    <!-- format checking section-->
                                                                                                    <v-stepper-content step="6" class="pt-0">
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

                                                                                                                <v-btn id="AutoTest_I62" color="secondary elevation-0" class="mr-2" @click="SendInvite(2),InitiativeAddUserSteps = 7"
                                                                                                                       v-bind:disabled="(roleErrorCount >0) || (!chkNotify1)">Send</v-btn>
                                                                                                                <v-btn id="AutoTest_I63" outlined="" color="grey" @click="InitiativeAddUserSteps = 5">Previous</v-btn>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>
                                                                                                    <!-- final Stepper for -->
                                                                                                    <v-stepper-content step="7" class="pa-0">
                                                                                                        <v-row no-gutters>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                                                <h4 class="text-center heading-3">Well Done !</h4>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                                                <v-avatar size="150" tile>
                                                                                                                    <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                                                                                </v-avatar>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                                                <p class="text-medium" v-if="InviteOrSelectStep==2">
                                                                                                                    Your Invites have been sent out successfully.<br />Bask in your glory !
                                                                                                                </p>
                                                                                                                <p class="text-medium" v-if="InviteOrSelectStep==4">
                                                                                                                    Your users have been added to the initiative successfully.<br />Bask in your glory !
                                                                                                                </p>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                                                <Secondary-Button id="AutoTest_I64" class="btn-122x36" title="Next" @click.native="NewInitiativeSteps=3,PrjTempSteps=1"></Secondary-Button>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>
                                                                                                </v-stepper-items>

                                                                                            </v-stepper>
                                                                                        </v-container>
                                                                                        <!--//</v-col>-->
                                                                                    </v-row>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-tab-item>

                                                                <!--User Group Tab Item-->
                                                                <v-tab-item value="tab-2">
                                                                    <v-card flat>
                                                                        <v-card-text class="pt-0">
                                                                            <v-row no-gutters>
                                                                                <v-col cols="12" md="12" class="pt-0">
                                                                                    <v-row :justify="justifyCenter">
                                                                                        <!--<v-col md="6">-->
                                                                                        <v-container fluid>
                                                                                            <v-stepper hide v-model="InitiativeAddUserGroupSteps" class="elevation-0">

                                                                                                <v-stepper-items>
                                                                                                    <!-- List of User Group  -->
                                                                                                    <v-stepper-content class="pa-0" step="1">
                                                                                                        <v-row no-gutters>
                                                                                                            <v-col cols="12" md="12" class="pt-0">
                                                                                                                <v-container fluid class="pt-2 px-1">
                                                                                                                    <v-row no-gutters>
                                                                                                                        <v-col cols="12" md="12" class="text-right">
                                                                                                                            <v-text-field outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="searchUserGroup">
                                                                                                                            </v-text-field>
                                                                                                                        </v-col>
                                                                                                                        <div class="chip-container mb-2" v-if="selectedUserGroupList.length>0">
                                                                                                                            <v-chip-group multiple column active-class="primary--text">
                                                                                                                                <v-chip id="AutoTest_I65" close v-for="(item,idx) in selectedUserGroupList" :key="idx"
                                                                                                                                        @click:close="removeUserGroupItem(item)" class="ma-2" small>
                                                                                                                                    <v-icon left color="#707070" size="15">mdi-account-group</v-icon>
                                                                                                                                    {{ item.GroupName }}
                                                                                                                                </v-chip>
                                                                                                                            </v-chip-group>

                                                                                                                        </div>
                                                                                                                        <!--v-chip for Assign roles ends here -->
                                                                                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                                                                                            <!--UserGroupsList with checkbox add here-->
                                                                                                                            <v-data-table :headers="userGroupTableheaders"
                                                                                                                                          :items="userGroupTableItems"
                                                                                                                                          show-select
                                                                                                                                          item-key="GroupName"
                                                                                                                                          sort-by="GroupName"
                                                                                                                                          fixed-header
                                                                                                                                          height="25vh"
                                                                                                                                          :search="searchUserGroup"
                                                                                                                                          v-model="selectedUserGroupList"
                                                                                                                                          class="elevation-0">

                                                                                                                            </v-data-table>
                                                                                                                        </v-col>
                                                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                                                                            <v-btn id="AutoTest_I66" color="secondary" class="mr-2 elevation-0" @click="InitiativeAddUserGroupSteps = 2"
                                                                                                                                   v-bind:disabled="selectedUserGroupList.length==0">Next</v-btn>
                                                                                                                            <v-btn id="AutoTest_I67" outlined color="grey" @click="NewInitiativeSteps = 1">Previous</v-btn>
                                                                                                                        </v-col>
                                                                                                                    </v-row>
                                                                                                                </v-container>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>
                                                                                                    <!-- final Stepper for User Group -->
                                                                                                    <v-stepper-content step="2" class="pa-0">
                                                                                                        <v-row no-gutters>
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
                                                                                                                    Your User Groups have been assigned to the initiative successfully.<br />Bask in your glory !
                                                                                                                </p>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                                                <v-btn id="AutoTest_I68" color="secondary" class="mr-2 elevation-0" @click="NewInitiativeSteps=3,PrjTempSteps=1">Done</v-btn>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>

                                                                                                </v-stepper-items>

                                                                                            </v-stepper>
                                                                                        </v-container>
                                                                                        <!--//</v-col>-->
                                                                                    </v-row>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                    </v-card>
                                                                </v-tab-item>
                                                            </v-tabs>
                                                        </v-stepper-content>

                                                        <!-- Step 3 Project Template-->
                                                        <v-stepper-content step="3" class="pa-0">
                                                            <v-container fluid class="pt-0">
                                                                <v-stepper hide v-model="PrjTempSteps" class="elevation-0">
                                                                    <v-stepper-items>
                                                                        <!-- Project Theme-->
                                                                        <v-stepper-content step="1" class="pa-0">
                                                                            <v-row no-gutters>
                                                                                <div>
                                                                                    <h3 class="text-left page-head">
                                                                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                            <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                        </v-avatar> Theme
                                                                                    </h3>
                                                                                </div>
                                                                                <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                    <h4 class="text-center heading-3">Step 3:  Please select a theme to see tha available template under it.</h4>
                                                                                </v-col>
                                                                                <v-col md="12" class="py-0">

                                                                                </v-col>
                                                                                <v-row>
                                                                                    <v-col xs="12" sm="12" md="12">
                                                                                        <v-row class="v-scrolling-div mx-4" style="height:40vh">
                                                                                            <v-card v-for="(item,idx) in prjThemeItems" :key="idx" width="150px" class="ma-2" style="float:left">
                                                                                                <v-row no-gutters>
                                                                                                    <!-- Notification-->
                                                                                                    <v-col md="12" class="pb-0">
                                                                                                        <v-avatar color="#52B962" size="25" style="margin-top:5px" class="ma-2" v-if="item.IsSelected">
                                                                                                            <span class="white--text">{{item.SelectedTemplCount}}</span>
                                                                                                        </v-avatar>
                                                                                                        <v-checkbox @change="SelectThemes(item)" v-model="item.IsSelected" color="#52B962" style="float:right;margin-top:5px"
                                                                                                                    label=""></v-checkbox>
                                                                                                    </v-col>
                                                                                                    <!-- Button-->
                                                                                                    <v-col md="12" class="text-center pb-0 pt-0">
                                                                                                        <v-btn id="AutoTest_I69" height="80" @click="GetProjectTemplateItems(item.ThemeID)" text>
                                                                                                            <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                                                                                <v-img v-bind:src="imageUrl + item.TemplateImage" height="80" width="80" v-if="item.TemplateImage.length>0"></v-img>
                                                                                                                <v-img src="../../images/WorkSpace-dummy-icon.png" height="80" width="80" v-else></v-img>
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

                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                    <Secondary-Button id="AutoTest_I70" class="btn-122x36 mr-4" title="Next" @click.native="ConfirmTemplateList()" v-bind:disabled="!ThemeSelected"></Secondary-Button>
                                                                                    <Default-Button-Outlined id="AutoTest_I71" class="btn-122x36" title="Previous" @click.native="ChangeTab(1),NewInitiativeSteps = 2,InitiativeAddUserSteps = 1,InitiativeAddUserGroupSteps=1"></Default-Button-Outlined>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>

                                                                        <!-- Project Template-->
                                                                        <v-stepper-content step="2" class="pa-0">
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <h3 class="text-left page-head">
                                                                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                            <v-icon size="20" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                        </v-avatar> Theme <v-icon size="20">mdi-chevron-right</v-icon> {{ ThemeName }}
                                                                                    </h3>
                                                                                </v-col>
                                                                                <!-- Step Text-->
                                                                                <v-col cols="12" md="12" class="pt-0 mb-1" v-if="selectedTempListByTheme.length>0 ">
                                                                                    <h4 class="text-center heading-3">Step 3:  Please select Multiple template from below</h4>
                                                                                </v-col>
                                                                                <!-- Chip group for Selected Template-->
                                                                                <v-col md="12" class="py-0">
                                                                                    <div class="chip-container mb-2" v-if="selectedTempListByTheme.length>0 ">
                                                                                        <v-chip-group multiple column active-class="primary--text">
                                                                                            <v-chip id="AutoTest_I72" close v-for="(item,idx) in selectedTempListByTheme" :key="idx"
                                                                                                    @click:close="removePrjTempItem(item)" class="ma-2" small>
                                                                                                <v-icon left color="#707070" size="15">mdi-folder-open</v-icon>
                                                                                                {{ item.TemplateName }}
                                                                                            </v-chip>
                                                                                        </v-chip-group>
                                                                                    </div>
                                                                                </v-col>
                                                                                <!-- No Data Text-->
                                                                                <v-col md="12" class="py-0" v-if="TemplateItems.length==0 ">
                                                                                    <v-row :justify="justifyCenter">
                                                                                        <v-col md="8" class="text-center">
                                                                                            <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                                                <v-img src="../../images/ic_no_workspace.svg" height="60" width="60" class="ma-4"></v-img>
                                                                                            </v-avatar>
                                                                                            <h4 class="text-center heading-3 pt-2">
                                                                                                No templates available under the theme - {{ThemeName}} .
                                                                                            </h4>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-col>
                                                                                <!-- Project Template Card-->
                                                                                <v-col md="12">
                                                                                    <v-row class="v-scrolling-div mx-2" style="height:30vh;" v-if="TemplateItems.length>0 ">
                                                                                        <v-col xs="6" sm="6" md="6" v-for="(item,idx) in TemplateItems" :key="idx">
                                                                                            <v-card class="workspace-card" flat>
                                                                                                <v-row no-gutters>
                                                                                                    <v-col md="12" class="pb-0">
                                                                                                        <v-img style="height:150px" v-bind:src="imageUrl + item.TemplateImage" v-if="item.TemplateImage.length>0">
                                                                                                            <v-checkbox color="success-check-box" @change="SelectTemplates(item)" v-model="item.IsSelected" label="" style="float:right;margin-top:10px"></v-checkbox>
                                                                                                        </v-img>
                                                                                                        <v-img style="height:150px" src="https://cdn.vuetifyjs.com/images/cards/docks.jpg" v-else>
                                                                                                            <v-checkbox color="success-check-box" @change="SelectTemplates(item)" v-model="item.IsSelected" label="" style="float:right;margin-top:10px"></v-checkbox>
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
                                                                                                    <v-col class="ml-0 v-scrolling-div" style="height:120px">
                                                                                                        <p>{{ item.Description }}</p>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <v-row no-gutters>
                                                                                                    <v-col md="6" class="text-center pt-1">
                                                                                                        <v-btn id="AutoTest_I73" color="success-btn elevation-0 enter-btn" block dark @click="PrjTempSteps = 4,ViewTemplate(item.ProjectTemplateID)">
                                                                                                            View Template
                                                                                                        </v-btn>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-card>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                    <Secondary-Button id="AutoTest_I74" class="btn-122x36 mr-4" title="Next" @click.native="PrjTempStep2()"></Secondary-Button>
                                                                                    <Default-Button-Outlined id="AutoTest_I75" class="btn-122x36" title="Previous" @click.native="PrjTempStep2()"></Default-Button-Outlined>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>
                                                                        <!-- Review the template under the selected theme-->
                                                                        <v-stepper-content step="3" class="pa-0">
                                                                            <v-row>
                                                                                <v-col>
                                                                                    <h3 class="text-left page-head">
                                                                                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                            <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                        </v-avatar> Theme <v-icon size="20">mdi-chevron-right</v-icon> Selected Themes <v-icon size="20">mdi-chevron-right</v-icon> Templates
                                                                                    </h3>
                                                                                </v-col>

                                                                                <v-col cols="12" md="12" class="pt-2 mb-2">
                                                                                    <h4 class="text-center heading-3">Step 3:  Please review the templates under each theme.</h4>
                                                                                </v-col>

                                                                                <v-row style="overflow-y:scroll;height:35vh" class="px-2">

                                                                                    <v-col cols="12" md="12" close v-for="(ThemeItem,index) in ConfTemplateList" :key="index">
                                                                                        <v-row no-gutters>
                                                                                            <h4 class="text-left heading-1">
                                                                                                <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                                                                    <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                                </v-avatar>  {{  ThemeItem.Theme }}
                                                                                            </h4>
                                                                                        </v-row>
                                                                                        <v-row v-if="ThemeItem.TemplCount>0" class="ml-2">
                                                                                            <v-chip-group multiple column active-class="primary--text">
                                                                                                <v-chip id="AutoTest_I76" close v-for="(item,index) in ConfTemplateList[index].ProjectTemplateList" :key="index"
                                                                                                        @click:close="removeConfPrjTempItem(item)" color="#DBF1FF" label text-color="white" small class="ma-2">
                                                                                                    <v-icon left color="#2196F3">mdi-folder-open</v-icon>
                                                                                                    {{ item.TemplateName }}
                                                                                                </v-chip>
                                                                                            </v-chip-group>
                                                                                        </v-row>

                                                                                        <v-row v-if="ThemeItem.TemplCount==0" class="ml-2 pt-1">
                                                                                            <h4 class="text-left heading-1">
                                                                                                No templates selected.
                                                                                            </h4>
                                                                                        </v-row>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                    <Secondary-Button id="AutoTest_I77" class="btn-122x36 mr-4" title="Next" @click.native="PrjTempSteps=5"></Secondary-Button>
                                                                                    <Default-Button-Outlined id="AutoTest_I78" class="btn-122x36" title="Previous" @click.native="PrjTempSteps=1"></Default-Button-Outlined>
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
                                                                                                    <v-container fluid class="v-scrolling-div" style="height:30vh">
                                                                                                        <v-col cols="12" md="12" class="pt-0 mb-0">
                                                                                                            <h4 class="text-center heading-3">Step 3:  Please view the template.</h4>
                                                                                                        </v-col>
                                                                                                        <!-- Heading with enter Button-->
                                                                                                        <v-list>
                                                                                                            <v-list-item class="pb-2">
                                                                                                                <v-btn id="AutoTest_I79" text width="auto" height="auto" @click="">
                                                                                                                    <v-avatar size="50" tile class="elevation-1 base-border-radius mr-2">
                                                                                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                                                                                    </v-avatar>

                                                                                                                    <p class="text-left mt-2">
                                                                                                                        <br />{{TemplateName }}<br />
                                                                                                                        <span size="12"> Created by ImWorks Team </span>
                                                                                                                    </p>

                                                                                                                    <!-- Private or Public Tabs comes here-->
                                                                                                                </v-btn>

                                                                                                                <v-spacer></v-spacer>
                                                                                                                <p class="mt-4">
                                                                                                                    <v-btn id="AutoTest_I80" color="success-btn elevation-0 enter-btn" dark @click="">
                                                                                                                        View Template
                                                                                                                    </v-btn>
                                                                                                                </p>

                                                                                                            </v-list-item>

                                                                                                        </v-list>
                                                                                                        <v-col>
                                                                                                            <h4 class="font-weight-regular heading-3-dark">About Template</h4>
                                                                                                            <h4 class="font-weight-regular heading-3">{{TemplateDescription }}</h4>
                                                                                                        </v-col>
                                                                                                        <v-col class="pt-1 pb-1">
                                                                                                            <h4 class="font-weight-regular heading-3-dark">Use Cases</h4>
                                                                                                            <v-list>
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
                                                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0 mt-4">
                                                                                                        <Secondary-Button id="AutoTest_I81" class="btn-122x36 mr-4" title="Next" @click.native="PrjTempSteps =3"></Secondary-Button>
                                                                                                        <Default-Button-Outlined id="AutoTest_I82" class="btn-122x36" title="Previous" @click.native="PrjTempSteps=2"></Default-Button-Outlined>

                                                                                                    </v-col>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-card-text>
                                                                            </v-card>
                                                                        </v-stepper-content>

                                                                        <!-- Final Message if Project Template completed-->
                                                                        <v-stepper-content step="5" class="pa-0">
                                                                            <v-row no-gutters>
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
                                                                                        Your template is created successfully.<br />Bask in your glory !
                                                                                    </p>
                                                                                </v-col>
                                                                                <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                                    <Secondary-Button id="AutoTest_I83" class="btn-122x36" title="Next" @click.native="NewInitiativeSteps=4"></Secondary-Button>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>
                                                                    </v-stepper-items>

                                                                </v-stepper>

                                                            </v-container>
                                                        </v-stepper-content>

                                                        <!-- step 4 Work flow -->
                                                        <v-stepper-content step="4" class="pa-0">
                                                            <v-stepper v-model="InitivateWorkflowsteps" class="elevation-0">
                                                                <v-stepper-items>

                                                                    <!-- Select Workflow step-->
                                                                    <v-stepper-content step="1" class="pa-0">
                                                                        <v-row no-gutters>
                                                                            <!-- Text-->
                                                                            <v-col cols="12" md="12" class="pt-0 mb-2">
                                                                                <h4 class="text-center heading-3">Step 4: Please create a project creation approval workflow.</h4>
                                                                            </v-col>
                                                                            <v-col cols="12" md="8" class="pt-1">
                                                                                <h3 class="text-left page-head">
                                                                                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                    </v-avatar> Work Flow
                                                                                </h3>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row no-gutters v-if="InitWorkflowItems.length>0">

                                                                            <v-col md="12" class="pt-1 text-right">
                                                                                <v-btn id="AutoTest_I84" color="success-btn elevation-0 enter-btn" dark @click="CreateWF()">
                                                                                    Create Workflow
                                                                                </v-btn>
                                                                            </v-col>
                                                                            <!-- Work flow group chip-->
                                                                            <v-col md="12" class="py-0">
                                                                                <div class="chip-container mb-2" v-if="selectedWFList.length>0">
                                                                                    <v-chip-group multiple column active-class="primary--text">
                                                                                        <v-chip id="AutoTest_I85" close v-for="(item,idx) in selectedWFList" :key="idx"
                                                                                                @click:close="removeWFItem(item)" class="ma-2" small>
                                                                                            <v-icon left color="#707070">mdi-email</v-icon>
                                                                                            {{ item.WfName }}
                                                                                        </v-chip>
                                                                                    </v-chip-group>
                                                                                </div>
                                                                            </v-col>

                                                                            <!-- Work flow details card-->
                                                                            <v-row style="overflow-y:scroll;height:45vh">

                                                                                <v-col xs="6" sm="6" md="6" v-for="(item,idx) in InitWorkflowItems" :key="idx">
                                                                                    <v-card class="workspace-card" flat>
                                                                                        <v-row>
                                                                                            <v-col xs="10" sm="10" md="10" class="d-inline-flex pt-0">
                                                                                                <div>
                                                                                                    <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>

                                                                                                    </v-avatar>
                                                                                                </div>
                                                                                                <div class="title heading-1 pt-4 px-2 text-limit">
                                                                                                    {{ item.WfName }}
                                                                                                </div>
                                                                                            </v-col>
                                                                                            <v-col xs="2" sm="2" md="2" class="d-inline-flex pt-0 text-right">
                                                                                                <v-checkbox @change="SelectWorkflows(item)" label="" v-model="item.ChkWF"></v-checkbox>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                        <!--Description-->
                                                                                        <v-row>
                                                                                            <v-col class="py-0">
                                                                                                <span class="heading-3  small-text">
                                                                                                    <v-icon size="15">mdi-folder-key-network-outline</v-icon>
                                                                                                    Description
                                                                                                </span>
                                                                                                <p style="overflow-y :auto; height:100px" class="ml-4 pt-1"> {{ item.WfDescription }}</p>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                        <!-- Levels-->
                                                                                        <v-row>
                                                                                            <v-col class="py-0">
                                                                                                <span class="title small-text">
                                                                                                    <v-icon size="12">mdi-tag</v-icon>
                                                                                                    Level : {{ item.WfNoOfLevels }}
                                                                                                </span>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                        <!-- buttons-->
                                                                                        <v-row>
                                                                                            <v-col>
                                                                                                <v-btn id="AutoTest_I86" color="success-btn elevation-0 enter-btn" dark @click="WFStep2(item.WorkflowID ,item.WfName,item.WfDescription,item.WfNoOfLevels) ,editView=1 ">
                                                                                                    View Workflow
                                                                                                </v-btn>
                                                                                            </v-col>
                                                                                            <v-col>
                                                                                                <v-btn id="AutoTest_I87" color="normal elevation-0" class="manage-btn" @click="ManageWF(item.WorkflowID, item.WfName, item.WfDescription,item.WfNoOfLevels),editView=2">
                                                                                                    <v-icon size="20" class="mr-2">mdi-settings</v-icon>
                                                                                                    Manage
                                                                                                </v-btn>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                    </v-card>
                                                                                </v-col>

                                                                            </v-row>
                                                                            <!--Buttons-->
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                <v-btn id="AutoTest_I88" outlined color="grey" class="mr-2 elevation-0" @click="NewInitiativeSteps = 5">Skip</v-btn>
                                                                                <v-btn id="AutoTest_I89" color="secondary" class="mr-2 elevation-0" @click="NewInitiativeSteps = 5"
                                                                                       v-bind:disabled="selectedWFList.length==0">Next</v-btn>
                                                                                <v-btn id="AutoTest_I90" outlined color="grey" @click="NewInitiativeSteps = 3,PrjTempSteps=1">Previous</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <!-- no data text-->
                                                                        <v-row no-gutter v-if="InitWorkflowItems.length==0">
                                                                            <v-col cols="12" sm="12" md="12">
                                                                                <v-row :justify="justifyCenter">
                                                                                    <v-col md="6" class="text-center">

                                                                                        <v-avatar size="60" tile class="elevation-1 base-border-radius mr-2">
                                                                                            <v-img src="../../images/ic_no_workspace.svg" height="60" width="60" class="ma-4"></v-img>
                                                                                        </v-avatar>
                                                                                        <p class="heading-3 pt-3">No workflow available</p>
                                                                                        <v-btn id="AutoTest_I91" height="39" color="success-btn" dark class="elevation-0 mr-2" @click="CreateWF()">
                                                                                            Create Workflow
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                <v-btn id="AutoTest_I92" outlined color="grey" class="mr-2 elevation-0" @click="NewInitiativeSteps = 5">Skip</v-btn>
                                                                                <v-btn id="AutoTest_I93" outlined color="grey" @click="NewInitiativeSteps = 3,PrjTempSteps=1">Previous</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-stepper-content>

                                                                    <!-- Create Workflow-->
                                                                    <v-stepper-content step="2" class="pa-0">
                                                                        <v-container fluid class="pt-0">
                                                                            <v-row no-gutters>
                                                                                <h4 class="text-left heading-1 mb-3">
                                                                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                    </v-avatar> Work Flow > Workflow Details
                                                                                </h4>
                                                                                <v-col cols="12" md="12" class="pt-0">
                                                                                    <h4 class="text-center heading-3">Step 1: Please create a project creation approval workflow</h4>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                                                                                    <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="60" width="60" class="ma-4"></v-img>
                                                                                    </v-avatar>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                                    <v-row :justify="justifyCenter">
                                                                                        <v-col md="6">
                                                                                            <v-text-field outlined dense label="Workflow Name" type="text" v-model="wfName">
                                                                                            </v-text-field>
                                                                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="wfDescription">
                                                                                            </v-textarea>

                                                                                            <v-select :items="level"
                                                                                                      label="No of Levels"
                                                                                                      item-text="value"
                                                                                                      outlined
                                                                                                      dense
                                                                                                      v-model="levelVal"></v-select>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-col>
                                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                    <Secondary-Button id="AutoTest_I94" class="btn-122x36 mr-4" title="Next" @click.native="WFStep2(0,wfName,wfDescription,levelVal)" v-bind:disabled="wfName.length == 0 || wfName.trim() == '' ||levelVal ==0" "></Secondary-Button>
                                                                                    <Default-Button-Outlined id="AutoTest_I95" class="btn-122x36" title="Previous" @click.native="InitivateWorkflowsteps=1"></Default-Button-Outlined>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-container>
                                                                    </v-stepper-content>

                                                                    <!--View Work Flow-->
                                                                    <v-stepper-content step="3" class="pa-0">
                                                                        <v-container fluid class="pt-0">
                                                                            <h3 class="text-left page-head">
                                                                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                                    <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                </v-avatar> <span style="cursor:pointer" @click="InitivateWorkflowsteps=1">Workflow</span>
                                                                                <v-icon size="20">mdi-chevron-right</v-icon>
                                                                                <span id="AutoTest_I96" style="cursor:pointer" @click="InitivateWorkflowsteps=4">Workflow Details</span>
                                                                                <v-icon size="20">mdi-chevron-right</v-icon>{{WFName}}
                                                                            </h3>
                                                                            <!-- Add Level Button-->
                                                                            <v-col cols="12" md="12" class="text-right">
                                                                                <v-btn id="AutoTest_I97" color="success-btn elevation-0 enter-btn" dark @click="btnAssign[levelVal+1] = 'Assign',levelVal+=1,levelsAssigned=false">
                                                                                    Add Level
                                                                                </v-btn>
                                                                            </v-col>
                                                                            <v-row class="v-scrolling-div" style="height:45vh" justify="center">
                                                                                <v-col md="8" v-for="i in levelVal" :key="i">
                                                                                    <v-card outlined flat>
                                                                                        <v-row no-gutters>
                                                                                            <!-- Circular Progress-->
                                                                                            <v-col cols="5" md="5" class="text-center" style="border-right:1px solid rgba(0, 0, 0, 0.12)">
                                                                                                <v-row align="center" justify="center" class="fill-height">
                                                                                                    <v-col>
                                                                                                        <v-progress-circular :rotate="360"
                                                                                                                             :size="100"
                                                                                                                             :width="15"
                                                                                                                             :value="(100/levelVal)*i"
                                                                                                                             color="#1EC2ED">
                                                                                                            <span class="subtitle-1 text--primary">
                                                                                                                {{i}}<br>
                                                                                                                <span class="subtitle-2 text--disabled">Level</span>
                                                                                                            </span>
                                                                                                        </v-progress-circular>
                                                                                                    </v-col>

                                                                                                </v-row>
                                                                                            </v-col>

                                                                                            <v-col cols="7" md="7" class="text-left">
                                                                                                <v-row>
                                                                                                    <v-col cols="12" md="12" class="heading-3">
                                                                                                        <v-icon size="25" class="mx-2">mdi-account</v-icon>Users
                                                                                                        <v-btn id="AutoTest_I98" text @click="RemoveLevel(WFID,i)" class="float-right" height="22" min-width="20">
                                                                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                                                        </v-btn>
                                                                                                        <v-divider class="mt-3"></v-divider>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <!---   Un comment if there is no data-->
                                                                                                <v-row no-gutters v-if="InitWorkflowLevels.length==0 || btnAssign[i] =='Assign' ">
                                                                                                    <v-col cols="12" md="12" class="pt-2 mb-0 text-center">
                                                                                                        <v-col>
                                                                                                            <p class="heading-3"> No Users Added</p>
                                                                                                        </v-col>
                                                                                                    </v-col>
                                                                                                </v-row>

                                                                                                <v-row no-gutters v-if="InitWorkflowLevels.length>0">
                                                                                                    <v-col cols="12" md="12">
                                                                                                        <v-col md="12" class="text-center mb-0 pt-0" v-for="(WFItem,idx) in InitWorkflowLevels" :key="idx">
                                                                                                            <!--<v-avatar size="40" class="team-avatar">
                                                                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                                            </v-avatar>-->

                                                                                                            <v-col v-if="WFItem.WorkflowID==WFID && WFItem.InitUserList.length>0">
                                                                                                                <v-avatar v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i && WFItem.InitUserList.length>=1 " size="27" class="team-avatar" color="red">
                                                                                                                    <span class="white--text headline">{{ WFItem.InitUserList[0].Name.charAt(0) }}</span>
                                                                                                                </v-avatar>
                                                                                                                <v-avatar v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i && WFItem.InitUserList.length>=2 " size="27" class="team-avatar" color="blue">
                                                                                                                    <span class="white--text headline">{{ WFItem.InitUserList[1].Name.charAt(0) }}</span>
                                                                                                                </v-avatar>
                                                                                                                <!--<v-avatar v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i  && WFItem.InitUserList.length>=3 " size="27" class="team-count" color="green">
                                                                                                                    {{WFItem.InitUserList.length-2}}
                                                                                                                </v-avatar>-->
                                                                                                            </v-col>
                                                                                                            <v-col class="text-center mb-0 pt-0">
                                                                                                                <p v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i && WFItem.InitUserList.length==1">{{  WFItem.InitUserList[0].Name }}</p>
                                                                                                                <p v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i && WFItem.InitUserList.length==2">{{  WFItem.InitUserList[0].Name }},{{  WFItem.InitUserList[1].Name }}</p>
                                                                                                                <p v-if="WFItem.WorkflowID==WFID && WFItem.WfLevel == i && WFItem.InitUserList.length>2">{{  WFItem.InitUserList[0].Name }},{{  WFItem.InitUserList[1].Name}} + {{ WFItem.InitUserList.length-2 }}</p>
                                                                                                            </v-col>
                                                                                                        </v-col>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <v-row no-gutters>
                                                                                                    <v-col cols="12" md="12" class="text-center pt-0 mb-0">
                                                                                                        <v-btn id="AutoTest_I99" color="success-btn elevation-0 enter-btn" dark @click="WrokFlowMenu =true,currLevel=i" class="mb-4">
                                                                                                            {{btnAssign[i]}}
                                                                                                        </v-btn>
                                                                                                    </v-col>
                                                                                                </v-row>

                                                                                            </v-col>
                                                                                        </v-row>
                                                                                    </v-card>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                <Secondary-Button id="AutoTest_I100" class="btn-122x36 mr-4" title="Save" @click.native="SaveWF(WFID)" v-bind:disabled="!levelsAssigned || levelVal ==0"></Secondary-Button>
                                                                                <Default-Button-Outlined id="AutoTest_I101" class="btn-122x36" title="Previous" @click.native="PrevStepWF()"></Default-Button-Outlined>
                                                                            </v-col>
                                                                        </v-container>
                                                                        <!-- Dailog Box-->
                                                                        <v-dialog v-model="WrokFlowMenu" width="900">

                                                                            <v-col cols="12" md="12" class="pt-0 mb-0">
                                                                                <v-card>
                                                                                    <v-card-title primary-title class="page-head pb-0 text-center">
                                                                                        Assign Users
                                                                                        <v-spacer></v-spacer>
                                                                                        <v-btn text @click="">
                                                                                            <v-icon id="AutoTest_I102" color="#707070" size="15" @click="WrokFlowMenu=false,selectedWFUserList=[]">mdi-close</v-icon>
                                                                                        </v-btn>
                                                                                    </v-card-title>
                                                                                    <v-row>
                                                                                        <!-- Users-->
                                                                                        <v-col class="px-4">
                                                                                            <v-data-table :headers="userTableheaders"
                                                                                                          :items="selectedUserList"
                                                                                                          show-select
                                                                                                          item-key="Name"
                                                                                                          sort-by="Name"
                                                                                                          fixed-header
                                                                                                          height="25vh"
                                                                                                          v-model="selectedWFUserList"
                                                                                                          class="elevation-0">
                                                                                            </v-data-table>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-row>
                                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-0 pb-3 my-4">
                                                                                            <Secondary-Button id="AutoTest_I103" class="btn-122x36 mr-4" title="Save" @click.native="SaveWFLevel(WFID,currLevel)" v-bind:disabled="!(selectedWFUserList.length!=0 )"></Secondary-Button>
                                                                                            <Default-Button-Outlined id="AutoTest_I104" class="btn-122x36" title="Cancel" @click.native="WrokFlowMenu=false,selectedWFUserList=[]"></Default-Button-Outlined>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-card>
                                                                            </v-col>
                                                                        </v-dialog>

                                                                    </v-stepper-content>

                                                                    <!-- View Or Edit Work Flow-->
                                                                    <v-stepper-content step="4" class="pa-0">

                                                                        <v-col cols="12" md="8" class="pt-1 mb-2">
                                                                            <h4 class="text-left heading-1">
                                                                                <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                                                    <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                                                </v-avatar> <span id="AutoTest_I105" style="cursor:pointer" @click="InitivateWorkflowsteps=1">Workflow</span>
                                                                                <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon>Workflow Details
                                                                            </h4>
                                                                        </v-col>

                                                                        <!-- View Workflow-->
                                                                        <v-row v-if="IsEnableEditWorkFlow == false">
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                                                                                <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="60" width="60" class="ma-4"></v-img>
                                                                                </v-avatar>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                                <v-row no-gutters :justify="justifyCenter">

                                                                                    <v-col xs="6" sm="6" md="6">
                                                                                        <v-card class="workspace-card elevation-0" flat>
                                                                                            <v-row>
                                                                                                <v-col class="py-0">
                                                                                                    <span class="title small-text">
                                                                                                        Workflow Name
                                                                                                    </span>
                                                                                                    <v-btn id="AutoTest_I106" text max-width="13" min-width="13" height="13"
                                                                                                           @click="IsEnableEditWorkFlow = true,wfName=WFName,wfDescription=WFDesc">
                                                                                                        <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                                                                                    </v-btn>
                                                                                                    <p>  {{ WFName }}</p>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                            <!--Description-->
                                                                                            <v-row>
                                                                                                <v-col class="py-0">
                                                                                                    <span class="title small-text">
                                                                                                        Description
                                                                                                    </span>
                                                                                                    <v-btn id="AutoTest_I107" text max-width="13" min-width="13" height="13"
                                                                                                           @click="IsEnableEditWorkFlow = true,wfName=WFName,wfDescription=WFDesc">
                                                                                                        <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                                                                                    </v-btn>
                                                                                                    <p style="overflow-y :scroll; height:100px">
                                                                                                        {{ WFDesc }}
                                                                                                    </p>
                                                                                                </v-col>
                                                                                            </v-row>

                                                                                            <!-- Levels-->
                                                                                            <v-row>
                                                                                                <v-col class="py-0">
                                                                                                    <span class="title small-text">
                                                                                                        <v-icon size="12">mdi-tag</v-icon>
                                                                                                        Levels :  {{ levelVal }}
                                                                                                    </span>
                                                                                                    <v-btn id="AutoTest_I108" text max-width="13" min-width="13" height="13" @click="InitivateWorkflowsteps =3">
                                                                                                        <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                                                                                    </v-btn>
                                                                                                </v-col>
                                                                                            </v-row>
                                                                                        </v-card>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                                                <v-btn id="AutoTest_I109" color="secondary" class="mr-2 elevation-0" @click="NewInitiativeSteps =4,InitivateWorkflowsteps=1">Previous</v-btn>
                                                                            </v-col>
                                                                        </v-row>

                                                                        <!-- Edit Workflow does not have level edit option-->
                                                                        <v-row v-if="IsEnableEditWorkFlow == true">

                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                                                                                <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="60" width="60" class="ma-4"></v-img>
                                                                                </v-avatar>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                                                <v-row :justify="justifyCenter">
                                                                                    <v-col md="6">

                                                                                        <v-text-field outlined dense type="text" v-model="wfName">
                                                                                            {{wfName}}
                                                                                        </v-text-field>
                                                                                        <v-textarea rows="3" outlined dense type="text" v-model="wfDescription">
                                                                                            {{wfDescription}}
                                                                                        </v-textarea>

                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center py-0 mb-0">
                                                                                <v-btn id="AutoTest_I110" color="secondary" class="mr-2" @click="UpdateWF(WFID)">Save</v-btn>
                                                                                <v-btn id="AutoTest_I111" outlined color="grey" @click="IsEnableEditWorkFlow = false">Cancel</v-btn>
                                                                            </v-col>
                                                                        </v-row>

                                                                    </v-stepper-content>

                                                                </v-stepper-items>

                                                            </v-stepper>
                                                        </v-stepper-content>

                                                        <!-- Final Message -->
                                                        <v-stepper-content step="5" class="pa-0">
                                                            <v-container fluid class="pt-0">

                                                                <v-row no-gutters>
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
                                                                            Your template is created successfully.<br />Bask in your glory !
                                                                        </p>
                                                                    </v-col>
                                                                    <v-col cols="12" md="12" class="pt-0 mb-2 text-center">
                                                                        <Secondary-Button id="AutoTest_I112" class="btn-122x36" title="Done" @click.native="fnCreateInitiative()"></Secondary-Button>
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
                                <v-list-item id="AutoTest_I113" @click.stop="fnBulkClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-plus-circle-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Deploy
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item id="AutoTest_I114" @click.stop="fnBulkRemoveClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                            <v-dialog v-model="DeployBulkDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Deploy Initiative
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I115" text="" @click="DeployBulkDailog=false">
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
                                                                <h4 class="text-center heading-3">Are you sure you want to deploy this initiative ?</h4>
                                                            </v-col>
                                                            <v-col>
                                                                <h4 class="text-center heading-3">
                                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                    {{NotifyMessage}}
                                                                </h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_I116" color="secondary elevation-0" class="mr-2" @click="ChangeInitiativeStatus(1)">Deploy</v-btn>
                                                                <v-btn id="AutoTest_I117" outlined color="grey" @click="DeployBulkDailog=false">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            <v-dialog v-model="RemoveBulkInitiativeDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Initiative
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I118" text="" @click="RemoveBulkInitiativeDailog=false">
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
                                                                <h4 class="text-center heading-3">Are you sure want to remove this initiative ?</h4>
                                                            </v-col>
                                                            <div>
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
                                                                                    </span>
                                                                                    <span class="message"> Selected initiatives have {{0}} projects in it and by removing thisinitiative all the active projects and tasks will become inactive.</span>
                                                                                </p>
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12">
                                                                    <!--User List with checkbox add here-->
                                                                    <v-data-table :headers="InitiativeAssignedprojectTableHeader"
                                                                                  :items="InitiativeAssignedprojectTableItems"
                                                                                  hide-default-footer
                                                                                  item-key="Name"
                                                                                  fixed-header
                                                                                  height="25vh"
                                                                                  class="elevation-0">


                                                                    </v-data-table>
                                                                </v-col>

                                                            </div>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_I119" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(1)">Remove</v-btn>
                                                                <v-btn id="AutoTest_I120" outlined color="grey" @click="RemoveBulkInitiativeDailog=false">Cancel</v-btn>
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
                        <v-text-field label="Search" class="table-search mr-4"
                                      dense prepend-inner-icon="mdi-magnify" v-model="searchInitiative"
                                      outlined hide-details></v-text-field>
                    </v-row>
                </v-col>
            </v-row>
            <!--Main Body-->

            <v-row v-if="InitiativeTableItems.length>0">
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="InitiativeTableheaders"
                                  :items="InitiativeTableItems"
                                  v-model="selectedInitiativeID"
                                  show-select=""
                                  item-key="InitiativeId"
                                  :search="searchInitiative"
                                  class="elevation-0">

                        <!-- Draft Initiatives Actions-->
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
                                    <v-list-item id="AutoTest_I121" @click.stop="" link :to="{ name: 'ManageInitiatives', query: { WsID: WorkspaceID, Access:RWAccess,InitiativeID : item.InitiativeId,InitiativeType: 1}}">
                                        <v-list-item-icon>
                                            <v-icon>mdi-pencil-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Edit
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_I122" link @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),DeployDailog=true,InitiativeName =item.InitiativeName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Deploy
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_I123" link="" @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),RemoveInitiativeDailog=true,InitiativeName =item.InitiativeName">
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
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Deploy Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I124" text="" @click="DeployDailog=false">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to deploy this initiative ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{item.InitiativeName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I125" color="secondary elevation-0" class="mr-2" @click="ChangeInitiativeStatus(2)">Deploy</v-btn>
                                                                    <v-btn id="AutoTest_I126" outlined color="grey" @click="DeployDailog=false">Cancel</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>
                                <v-dialog v-model="RemoveInitiativeDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I127" text="" @click="RemoveInitiativeDailog=false,InitiativeName =null">
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
                                                                    <h4 class="text-center heading-3">Are you sure want to remove this initiative ?</h4>
                                                                </v-col>
                                                                <div>
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
                                                                                            {{InitiativeName}}
                                                                                        </span>
                                                                                        <span class="message"> have {{0}} projects in it and by removing this initiative all the active projects and tasks will become inactive.</span>
                                                                                    </p>
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                        </v-list>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                        <!--User List with checkbox add here-->
                                                                        <v-data-table :headers="InitiativeAssignedprojectTableHeader"
                                                                                      :items="InitiativeAssignedprojectTableItems"
                                                                                      hide-default-footer
                                                                                      item-key="Name"
                                                                                      fixed-header
                                                                                      height="25vh"
                                                                                      class="elevation-0">


                                                                        </v-data-table>
                                                                    </v-col>

                                                                </div>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I128" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(2)">Remove</v-btn>
                                                                    <v-btn outlined id="AutoTest_I129" color="grey" @click="RemoveInitiativeDailog=false,InitiativeName =item.InitiativeName">Cancel</v-btn>
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
            <!-- else part if No initiative available-->
            <v-row v-else>
                <v-col cols="12" sm="12" md="12">
                    <v-row :justify="justifyCenter">
                        <v-col md="6" class="text-center">

                            <v-avatar size="80" tile class="elevation-1 base-border-radius mr-2">
                                <v-img src="../../images/ic_no_workspace.svg" height="80" width="80" class="ma-4"></v-img>
                            </v-avatar>
                            <p class="heading-3 pt-3">No initiatives available</p>
                            <v-btn id="AutoTest_I130" height="39" color="success-btn" dark class="elevation-0 mr-2" @click="InitiativeDialog=true" v-bind:disabled ="!screenAccess">
                                Create Initiatives
                            </v-btn>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>
<script type="text/javascript">
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

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
    }


    export default {
        components: {
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
        },
        data() {
            return {

                snackbarMsg: false,
                Msg:"",
                justifyCenter: "center",
                WorkspaceID: 0,
                //=====CreateInitiative Variables

                InitiativeDialog: false,
                NewInitiativeSteps: 0,
                initName: "",
                initDescription: "",
                initCode: "",
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                isNewTag: false,
                newTagID: 0,
                TagList: [],
                initTagItems: [],
                selectedColor: false,

                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                type: 'hex',
                hex: '#FF00FF',
                filterMenu: false,
                TagMenu: false,

                optUser: false,
                vmSelectUser: false,
                NewEmail: "",
                alphanumericRules: [
                    v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name'
                ],
                reg: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/,

                emailRules: [
                    v => /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/.test(v),
                    // || 'Please enter valid E-mail',
                ],
                chipItem: true,
                InviteUserListInput: [],
                InviteUsersInput: [],
                userListInput: [],
                userListItems: [],
                emailErrors: [],
                chkEmails: false,
                clearable: false,
                chkNotify: true,
                chkNotify1: true,

                IsValidData: false,
                sentInvite: null,
                InviteType: 0,

                item: "",
                //Import users====
                fileInput: null,
                csvfile: {
                    name: "",
                    size: 0,
                    namedisplay: ""
                },
                uploadPercentage: 0,
                csvfiledata: [],
                importInviteTableheaders: [
                    { text: "Email", align: "left", value: "EmailId" },
                    { text: "User Role", value: "UserRole" },
                    { text: "", align: "left", color: "red", value: "Message" }
                ],
                roleErrorCount: 0,
                //------------------------------------------
                userRoleTableItems: [],
                userRoleInput: [],
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
                ImportInviteUser: false,

                // initiatives add user sub steps
                InitiativeAddUserSteps: 0,
                InviteOrSelectStep: 2,
                InitiativeAddUserGroupSteps: 0,

                //----Select Users option

                userTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                userTableItems: [],
                searchUser: "",
                selectedUserList: [],

                //----Select Group option
                userGroupTableheaders: [
                    { text: "User Groups", align: "left", value: "GroupName" },
                    { text: "Member Count", value: "MemberCount" },
                ],
                userGroupTableItems: [],
                searchUserGroup: "",
                selectedUserGroupList: [],

                PrjTempSteps: 0,
                prjThemeItems: [],

                selectedTemplateList: [],
                selectedTempListByTheme: [],
                ConfTemplateList: [],
                TemplateItems: [],
                ThemeID: "",
                ThemeName: "",
                templCount: 0,
                ThemeSelectedCount: 0,
                ThemeSelected: false,

                TemplateName: "",
                TemplateDescription: "",
                TemplateUsecases: [],

                wfName: "",
                wfDescription: "",

                level: [],
                WorkflowItems: [],

                WFID: "", WFName: "", WFDesc: "", levelVal: "", currLevel: "",
                btnAssign: [],
                chkWF: [],
                newInit: false,

                Tab: 'Tab-1',
                InitivateWorkflowsteps: 0,
                WrokFlowMenu: false,
                WorkflowUserGrpHeaders: [
                    { text: "User Groups", align: "left", value: "GroupName" },
                ],

                selectedWFUserList: [],
                InitWorkflow: [],
                InitWorkflowLevels: [],
                InitWorkflowItems: [],

                selectedWFList: [],
                isChkWF: false,
                levelsAssigned: false,
                IsEnableEditWorkFlow: false,
                //=================================
                InitiativeTableheaders: [
                    { text: "Name", align: "left", value: "InitiativeName" },
                    { text: "Code", value: "Code" },
                    { text: "Projects", value: "0" },
                    { text: "Beneficiaries", value: "0", },
                    { text: "Actions", value: "Actions", sortable: false }

                ],
                InitiativeTableItems: [],
                selectedInitiativeID: [],
                searchInitiative: "",
                selectedInitiativeName: "",
                RemoveInitiativeDailog: false,
                RemoveBulkInitiativeDailog: false,
                DeployDailog: false,
                DeployBulkDailog: false,
                editView: 0,
                InitiativeAssignedprojectTableHeader: [
                    { text: 'ID', value: 'ID', align: ' d-none' },
                    { text: 'Projects', align: 'left', value: 'Name' },
                    { text: 'Tasks', value: 'Tasks' },

                ],

                InitiativeAssignedprojectTableItems: [],
                InitiativeID: "",
                InitiativeName: "",
                NotifyMessage: "",
                SortMenu: false,

                // Tag Menu .........
                NewTagMenu: false,
                NewTagName: "",
                NewTagList: [],
                model: "",
                // search: null,
                NewTagName: "",
                isTagExisting: true,
                ButtonName: "Add New Tag",


                // Altered Tag Menu
                AllTagListSteps: 0,
                NewTagListSteps: 0,
                TagHeaderName: "Add Tag",
                NewTagMenu: false,
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

                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                imageUrl: "",
                screenAccess: 0,
                RWAccess: 0,
            };
        },
        watch: {

            selectedInitiativeID: function () {
                this.NotifyMessage = "";
                if (this.selectedInitiativeID.length > 2) {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName + "," + this.selectedInitiativeID[1].InitiativeName + " and " + (this.selectedInitiativeID.length - 2) + " more ";
                } else if (this.selectedInitiativeID.length == 2) {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName + "," + this.selectedInitiativeID[1].InitiativeName;
                } else {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName;
                }
            },

            //selectedWFUserList: function () {
            //    if (this.selectedWFUserList.length != 0 && this.selectedWFGroupList.length > 0)
            //        this.selectedWFGroupList = [];
            //},
            //selectedWFGroupList: function () {
            //    if (this.selectedWFGroupList.length != 0 && this.selectedWFUserList.length > 0)
            //        this.selectedWFUserList = [];
            //},

        },


        mounted: async function () {
            this.imageUrl = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await importUserscript();
            await this.fnGetInitiativeList();
            await this.GetUserRoleDetails();
            await this.GetAllUsersList();
            await this.GetUserGroups();
            await this.GetProjectTheme();
            await this.GetLevel();
            await this.OpenCreateInitiativeDialogue();
            await this.fnGetAllTags();
            await this.GetNextInitiativeCode();

        },

        methods: {

            //============ Create Initiative=====================
            //===================================================

            OpenCreateInitiativeDialogue() {
                const initDialog = this.$route.query.InDlg;
                this.InitiativeDialog = initDialog;
            },
            async GetNextInitiativeCode() {
                var result = await objInitiative.GetNextInitiativeCode();

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.initCode = result.data;
                }
            },
            //============Tag =====================
            async GetInitTagDetails() {
                var result = await objInitiative.GetInitTagDetails();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data)
                        this.initTagItems = result.data;
                    this.initTagItems.push({ TagID: 0, TagName: 'Other' });

                    if (this.TagList.length > 0) {
                        for (var i = 0; i < this.TagList.length; i++) {
                            if (this.TagList[i].TagID != 0)
                                this.initTagItems = this.initTagItems.filter(x => x.TagID != this.TagList[i].TagID);
                        }
                    }
                }
            },
            getTag(value) {

                if (value == 'Other') {
                    this.isNewTag = true;
                    this.tagColor = "";
                    this.tagName = "";

                    this.selectedColor = false;
                }
                else {

                    this.isNewTag = false;
                    var tagItem = this.initTagItems.filter(p => p.TagName == value);
                    this.tagID = tagItem[0].TagID;
                    this.tagColor = tagItem[0].TagColor;
                    this.tagName = tagItem[0].TagName;
                    this.selectedColor = true;

                }
            },
            addNewTag(tagColr, tagName) {

                if (tagName.length == 0 || tagName.trim() == '')
                    this.selectedColor = false;
                else {
                    this.tagColor = tagColr;
                    this.tagName = tagName;
                    this.selectedColor = true;
                }
            },
            fnAddTag(tagColr, tagName) {

                var i = 0; var bExistis = false;
                for (i = 0; i < this.TagList.length; i++) {
                    if (this.TagList[i].TagName == tagName) // && this.TagList[i].TagColor == tagColr)
                    {
                        bExistis = true;
                        this.Msg = ("The tag '" + tagName + "' already added");
                        this.snackbarMsg = true;
                        break;
                    }
                }
                if (!bExistis) {
                    var TID = this.isNewTag ? 0 : this.tagID;
                    this.TagList.push({ TagID: TID, TagName: tagName, TagColor: tagColr });
                }

                this.enableSave = true;
                this.CloseTag();
            },
            async remove(item) {
                this.TagList.splice(this.TagList.indexOf(item), 1);
                this.TagList = [...this.TagList];
            },
            //============Step2 : Add Users / Groups ==============
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
            AddToList() {

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

            async GetUserRoleDetails() {

                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objUser.GetUserRoleDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.userRoleTableItems = result.data;
                    }
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
                else {
                    if (result.data) {
                        this.userScreenNameTableItems = result.data;
                    }
                }
            },

            //----Import users

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

                if (this.InitiativeAddUserSteps == 6 && this.csvfile.name) {
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

                    this.InviteType = Type;
                    this.sentInvite = true;
                    this.InitiativeAddUserSteps = 7;
                }

            },
            async GetInitUsersDetails() {

                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetInitUsersDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.userTableItems = result.data;
                    }
                }
            },
            async GetUserGroups() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetInitUserGroups(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.userGroupTableItems = result.data;
                    }
                }
            },
            removeEmail(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];
            },

            removeRole(item) {
                this.InviteUserListInput.splice(this.InviteUserListInput.indexOf(item), 1);
                this.InviteUserListInput = [...this.InviteUserListInput];

                if (item.Message != null)
                    this.roleErrorCount -= 1;
            },
            removeUserItem(item) {
                this.selectedUserList.splice(this.selectedUserList.indexOf(item), 1);
                this.selectedUserList = [...this.selectedUserList];
            },
            removeUserGroupItem(item) {
                this.selectedUserGroupList.splice(this.selectedUserGroupList.indexOf(item), 1);
                this.selectedUserGroupList = [...this.selectedUserGroupList];
            },

            //======Step3 : Add Project Theme & Templates ==============
            async GetProjectTheme() {

                var result = await objInitiative.GetProjectTheme();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.prjThemeItems = result.data;

                    }
                    var i = 0;
                    for (i = 0; i < this.prjThemeItems.length; i++)
                        this.prjThemeItems[i].IsSelected = 0;
                }

            },
            async GetProjectTemplateItems(ThemeID) {

                this.ThemeID = ThemeID;
                this.ThemeName = this.prjThemeItems.filter(p => p.ThemeID == ThemeID)[0].Theme;
                var data = new FormData();
                data.append("ThemeID", ThemeID);
                var result = await objInitiative.GetProjectTemplateItems(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data)
                        this.TemplateItems = result.data;


                    var i = 0; var j = 0;
                    this.selectedTempListByTheme = this.selectedTemplateList.filter(p => p.ThemeID == ThemeID);

                    for (i = 0; i < this.TemplateItems.length; i++) {
                        for (j = 0; j < this.selectedTempListByTheme.length; j++)
                            if (this.TemplateItems[i].ProjectTemplateID == this.selectedTempListByTheme[j].ProjectTemplateID)
                                this.TemplateItems[i].IsSelected = 1;
                    }
                    this.PrjTempSteps = 2;
                }
            },
            async SelectThemes(item) {

                var i = 0;
                this.ThemeName = this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].Theme;

                if (item.IsSelected) {
                    this.ThemeSelected = true;
                    this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].SelectedTemplCount = 0;
                    this.GetProjectTemplateItems(item.ThemeID);
                    this.PrjTempSteps = 2;

                }
                else {
                    while (i < this.selectedTemplateList.length) {
                        if (this.selectedTemplateList[i].ThemeID == item.ThemeID) {
                            this.selectedTemplateList.splice(i, 1);
                            this.selectedTemplateList = [...this.selectedTemplateList];
                            i--;
                            this.ThemeSelectedCount = this.ThemeSelectedCount != 0 ? this.ThemeSelectedCount -= 1 : 0;
                        }
                        i++;
                    }

                    this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].SelectedTemplCount = 0;
                    this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].IsSelected = 0;
                }

            },
            SelectTemplates(item) {

                var i = 0;

                for (i = 0; i < this.selectedTemplateList.length; i++) {
                    if (this.selectedTemplateList[i].ProjectTemplateID == item.ProjectTemplateID) {
                        this.selectedTemplateList.splice(i, 1);
                        this.selectedTemplateList = [...this.selectedTemplateList];
                    }
                }
                if (item.IsSelected) {
                    this.selectedTemplateList.push({ ThemeID: this.ThemeID, Theme: item.Theme, ProjectTemplateID: item.ProjectTemplateID, TemplateName: item.TemplateName });
                    this.ThemeSelected = true;
                }
                else {

                    this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].SelectedTemplCount -= 1;
                    this.ThemeSelectedCount = this.ThemeSelectedCount != 0 ? this.ThemeSelectedCount -= 1 : 0;

                    //if (this.ThemeSelectedCount == 0)
                    //    this.ThemeSelected = false;
                }


                //Filter by ThemeID
                this.selectedTempListByTheme = this.selectedTemplateList.filter(p => p.ThemeID == this.ThemeID);

            },
            ConfirmTemplateList() {

                var i = 0; var j = 0; var bExists = 0; var n = 0;
                var ThID = 0;
                var lstTemp = [];
                this.ConfTemplateList = [];

                for (i = 0; i < this.prjThemeItems.length; i++) {
                    if (this.prjThemeItems[i].IsSelected) {
                        lstTemp = [];
                        this.selectedTempListByTheme = this.selectedTemplateList.filter(p => p.ThemeID == this.prjThemeItems[i].ThemeID);
                        for (j = 0; j < this.selectedTempListByTheme.length; j++)
                            lstTemp.push({
                                ThemeID: this.selectedTempListByTheme[j].ThemeID,
                                ProjectTemplateID: this.selectedTempListByTheme[j].ProjectTemplateID,
                                TemplateName: this.selectedTempListByTheme[j].TemplateName
                            });

                        n = lstTemp.length;
                        this.ConfTemplateList.push({
                            ThemeID: this.prjThemeItems[i].ThemeID, Theme: this.prjThemeItems[i].Theme, ProjectTemplateList: lstTemp,
                            TemplCount: n
                        });
                    }
                }
                this.PrjTempSteps = 3;
            },
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
            PrjTempStep2() {

                var n = this.selectedTempListByTheme.length;
                this.prjThemeItems.filter(p => p.ThemeID == this.ThemeID)[0].SelectedTemplCount = n;

                if (n > 0) {
                    this.prjThemeItems.filter(p => p.ThemeID == this.ThemeID)[0].IsSelected = 1;
                    this.ThemeSelectedCount++;
                    this.ThemeSelected = true;
                }

                this.selectedTempListByTheme = [];
                this.PrjTempSteps = 1;
            },

            removePrjTempItem(item) {
                this.selectedTemplateList.splice(this.selectedTemplateList.indexOf(item), 1);
                this.selectedTemplateList = [...this.selectedTemplateList];

                this.selectedTempListByTheme = this.selectedTemplateList.filter(p => p.ThemeID == item.ThemeID);
                this.TemplateItems.filter(p => p.ProjectTemplateID == item.ProjectTemplateID)[0].IsSelected = 0;

                this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].SelectedTemplCount -= 1;
                this.ThemeSelectedCount = this.ThemeSelectedCount != 0 ? this.ThemeSelectedCount -= 1 : 0;
                if (this.ThemeSelectedCount == 0)
                    this.ThemeSelected = false;
            },
            removeConfPrjTempItem(item) {

                var i = 0;
                for (i = 0; i < this.selectedTemplateList.length; i++) {
                    if (this.selectedTemplateList[i].ProjectTemplateID == item.ProjectTemplateID) {
                        this.selectedTemplateList.splice(i, 1);
                        this.selectedTemplateList = [...this.selectedTemplateList];
                    }

                }
                this.selectedTempListByTheme = this.selectedTemplateList.filter(p => p.ThemeID == item.ThemeID);
                this.prjThemeItems.filter(p => p.ThemeID == item.ThemeID)[0].SelectedTemplCount -= 1;


                this.ThemeSelectedCount = this.ThemeSelectedCount != 0 ? this.ThemeSelectedCount -= 1 : 0;
                if (this.ThemeSelectedCount == 0)
                    this.ThemeSelected = false;

                this.ConfirmTemplateList();
            },

            //======Step4 : Add Workflow ==============
            GetLevel() {
                var value;
                var i = 1;
                for (i = 1; i <= 10; i++) {
                    this.level.push({ value: i })
                }
            },

            CreateWF() {
                this.wfName = "";
                this.wfDescription = "";
                this.levelVal = "";
                this.InitivateWorkflowsteps = 2;
            },
            WFStep2(WfID, WfName, WfDesc, WfLevel) {

                this.WFID = WfID;

                if (WfID == 0) {

                    var nextWFID = (this.InitWorkflow.length == 0) ? 1 : this.InitWorkflow.length + 1;
                    this.InitWorkflow.push({ WorkflowID: nextWFID, WfName: WfName, WfDescription: WfDesc, WfNoOfLevels: WfLevel, ChkWF: 0 });
                    this.WFID = nextWFID;
                    for (var i = 1; i <= WfLevel; i++)
                        this.btnAssign[i] = "Assign";
                    this.levelsAssigned = false;
                    this.newInit = true;

                }
                else
                    this.newInit = false;
                this.WFName = WfName;
                this.WFDesc = WfDesc;
                this.levelVal = WfLevel;
                this.InitivateWorkflowsteps = 3;
            },
            ManageWF(WfID, WfName, WfDesc, WfLevel) {

                this.newInit = false;
                this.WFID = WfID;
                this.WFName = WfName;
                this.WFDesc = WfDesc;
                this.levelVal = WfLevel;
                this.InitivateWorkflowsteps = 4;
                this.IsEnableEditWorkFlow = false;
            },
            UpdateWF(WFID) {

                this.IsEnableEditWorkFlow = false;
                this.InitWorkflow.filter(p => p.WorkflowID == WFID)[0].WfName = this.wfName;
                this.InitWorkflow.filter(p => p.WorkflowID == WFID)[0].WfDescription = this.wfDescription;

                this.InitWorkflowItems.filter(p => p.WorkflowID == WFID)[0].WfName = this.wfName;
                this.InitWorkflowItems.filter(p => p.WorkflowID == WFID)[0].WfDescription = this.wfDescription;

                this.WFName = this.wfName;
                this.WFDesc = this.wfDescription;

            },
            SaveWFLevel(WFID, currLevel) {

                var i = 0;

                for (i = 0; i < this.InitWorkflowLevels.length; i++) {
                    if (this.InitWorkflowLevels[i].WorkflowID == WFID && this.InitWorkflowLevels[i].WfLevel == currLevel) {
                        this.InitWorkflowLevels.splice(i, 1);
                        this.InitWorkflowLevels = [...this.InitWorkflowLevels];
                    }
                }
                this.InitWorkflowLevels.push({ WorkflowID: WFID, WfLevel: currLevel, InitUserList: this.selectedWFUserList });
                this.btnAssign[currLevel] = "Re-Assign";
                this.selectedWFUserList = [];
                this.WrokFlowMenu = false;
                var wfLevelItem = this.InitWorkflow.filter(p => p.WorkflowID == WFID);
                var cnt = 0;

                for (i = 1; i <= wfLevelItem[0].WfNoOfLevels; i++) {
                    if (this.btnAssign[i] == "Re-Assign")
                        cnt++;
                }
                if (cnt == wfLevelItem[0].WfNoOfLevels)
                    this.levelsAssigned = true;
            },

            PrevStepWF() {

                if (this.newInit)
                    this.InitivateWorkflowsteps = 2;
                else {
                    if (this.editView == 1)
                        this.InitivateWorkflowsteps = 1;

                    else if (this.editView == 2) {

                        this.InitivateWorkflowsteps = 4;
                        this.IsEnableEditWorkFlow = false;
                    }

                }

            },
            SaveWF(WFID) {

                var i;
                var wfLevelItem = this.InitWorkflowLevels.filter(p => p.WorkflowID == WFID);
                var wfItem = this.InitWorkflow.filter(p => p.WorkflowID == WFID);

                if (this.levelVal != wfItem[0].WfNoOfLevels)
                    this.InitWorkflow.filter(p => p.WorkflowID == WFID)[0].WfNoOfLevels = this.levelVal;

                for (i = 0; i < this.InitWorkflowItems.length; i++) {
                    if (this.InitWorkflowItems[i].WorkflowID == WFID) {
                        this.InitWorkflowItems.splice(i, 1);
                        this.InitWorkflowItems = [...this.InitWorkflowItems];
                    }
                }
                this.InitWorkflowItems.push({
                    WorkflowID: WFID, WfName: wfItem[0].WfName, WfDescription: wfItem[0].WfDescription,
                    WfNoOfLevels: wfItem[0].WfNoOfLevels, WfLevelItems: wfLevelItem
                });
                this.InitivateWorkflowsteps = 1;
            },
            RemoveLevel(WFID, level) {
                var i, lv;
                var Assgned = false;

                this.levelVal -= 1;
                this.btnAssign[level] = "Assign";

                if (this.InitWorkflowLevels.length > 0 && level <= this.InitWorkflowLevels.length) {

                    for (i = 0; i < this.InitWorkflowLevels.length; i++) {
                        if (this.InitWorkflowLevels[i].WorkflowID == WFID && this.InitWorkflowLevels[i].WfLevel == level) {
                            this.InitWorkflowLevels.splice(i, 1);
                            this.InitWorkflowLevels = [...this.InitWorkflowLevels];
                        }
                    }

                    for (i = 0; i < this.InitWorkflowLevels.length; i++) {
                        lv = this.InitWorkflowLevels.filter(p => p.WorkflowID == WFID)[i].WfLevel;
                        if (lv > level) {
                            this.InitWorkflowLevels.filter(p => p.WorkflowID == WFID)[i].WfLevel -= 1;
                            Assgned = (this.InitWorkflowLevels.filter(p => p.WorkflowID == WFID)[i].InitUserList.length == 0) ? false : true;
                            if (Assgned)
                                this.btnAssign[level] = "Re-Assign";
                        }

                    }
                }

            },
            SelectWorkflows(wfItem) {

                var i = 0;
                var isSelected = false;
                var wfLevelItem = this.InitWorkflowLevels.filter(p => p.WorkflowID == wfItem.WorkflowID);


                if (wfItem.ChkWF) {
                    for (i = 0; i < this.selectedWFList.length; i++) {
                        if (this.selectedWFList[i].WorkflowID == wfItem.WorkflowID) {
                            this.selectedWFList.splice(i, 1);
                            this.selectedWFList = [...this.selectedWFList];
                            break;
                        }
                    }
                    this.selectedWFList.push({
                        WorkflowID: wfItem.WorkflowID, WfName: wfItem.WfName, WfDescription:
                            wfItem.WfDescription, WfNoOfLevels: wfItem.WfNoOfLevels, WfLevelItems: wfLevelItem
                    });

                }
                else {
                    this.selectedWFList.splice(this.selectedWFList.indexOf(wfItem), 1);
                    this.selectedWFList = [...this.selectedWFList];
                }
            },
            removeWFItem(item) {
                this.selectedWFList.splice(this.selectedWFList.indexOf(item), 1);
                this.selectedWFList = [...this.selectedWFList];

            },
            //-----------------------------
            InviteOrSelectUser(type) {

                if (type == "Invite") {
                    this.InviteOrSelectStep = 2;
                }
                if (type == "Select") {
                    this.InviteOrSelectStep = 4;
                    this.GetInitUsersDetails();
                }
                if (type == "Import") {

                    this.InviteOrSelectStep = 5;
                }
            },
            ChangeTab(index) {

                if (index == 1) {
                    this.Tab = 'tab-1';
                }
                if (index == 2) {
                    this.Tab = 'tab-2';
                }

            },
            NextStep() {
                this.InitiativeAddUserSteps = this.InviteOrSelectStep
            },
            async fnCreateInitiative() {

                var i = 0, j = 0;
                //var TagIDList = [], userList = [], groupList = [];
                var TagIDList = "", userList = "", groupList = "";

                if (this.initName == "") {
                    this.Msg = ("Initiative Name is required");
                    this.snackbarMsg = true;
                    return;
                }

                /*Chk Duplicate */

                for (i = 0; i < this.InitiativeTableItems.length; i++) {
                    if (this.InitiativeTableItems[i].InitiativeName == this.initName) {
                        this.Msg = ("Duplicate Entry - same Initiative Name");
                        this.snackbarMsg = true;
                        this.NewInitiativeSteps = 1;
                        return;
                    }
                }

                for (i = 0; i < this.SelectedTag.length; i++) {
                    if (TagIDList == "")
                        TagIDList = this.SelectedTag[i].TagID;
                    else
                        TagIDList = TagIDList + "," + this.SelectedTag[i].TagID;
                }
                //for (i = 0; i < this.selectedUserList.length; i++)
                //    userList[i] = this.selectedUserList[i].UserID;


                //Selected Users List
                for (i = 0; i < this.selectedUserList.length; i++)
                    if (userList == "")
                        userList = this.selectedUserList[i].UserID;
                    else
                        userList = userList + "," + this.selectedUserList[i].UserID;

                //Groups List
                for (i = 0; i < this.selectedUserGroupList.length; i++)
                    if (groupList == "")
                        groupList = this.selectedUserGroupList[i].GroupID;
                    else
                        groupList = groupList + "," + this.selectedUserGroupList[i].GroupID;

                //Project Template List
                this.selectedTemplateList = [];
                for (i = 0; i < this.ConfTemplateList.length; i++) {

                    if (this.ConfTemplateList[i].ProjectTemplateList.length != 0) {
                        for (j = 0; j < this.ConfTemplateList[i].ProjectTemplateList.length; j++)
                            this.selectedTemplateList.push({ ThemeID: this.ConfTemplateList[i].ThemeID, ProjectTemplateID: this.ConfTemplateList[i].ProjectTemplateList[j].ProjectTemplateID });
                    }
                    else
                        this.selectedTemplateList.push({ ThemeID: this.ConfTemplateList[i].ThemeID, ProjectTemplateID: 0 });
                }


                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("InitiativeName", this.initName);
                data.append("Description", this.initDescription);
                data.append("Code", this.initCode);
                data.append("InitiativeTags", TagIDList);
                if (this.InviteType == 1)
                    data.append("InviteUserList", JSON.stringify(this.InviteUserListInput));

                else if (this.InviteType == 2)
                    data.append("InviteUserList", JSON.stringify(this.csvfiledata));

                data.append("InitUserList", userList);
                data.append("InitGroupList", groupList);
                data.append("InitProjectTemplate", JSON.stringify(this.selectedTemplateList));
                data.append("InitWorkflowList", JSON.stringify(this.selectedWFList));
                this.CloseInitiativeDialog();

                var result = await objInitiative.CreateInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success")
                        this.Msg =("Success")
                    else
                        this.Msg = ("Failed");
                    this.snackbarMsg = true;
                }
                TagIDList = "";
                userList = "";
                groupList = "";
                this.fnGetInitiativeList();
            },
            CloseInitiativeDialog() {

                this.initName = "";
                this.initDescription = "";
                this.TagList = [];
                this.InviteUserListInput = [];
                this.csvfiledata = [];
                this.selectedUserList = [];
                this.selectedUserGroupList = [];
                this.optUser = false;
                this.vmSelectUser = false;
                this.prjThemeItems = [];
                this.GetProjectTheme();

                this.selectedTempListByTheme = [];
                this.selectedTemplateList = [];
                this.ConfTemplateList = [];
                this.selectedWFList = [];
                this.selectedWFUserList = [];
                this.InitWorkflowItems = [];
                this.InitWorkflow = [];
                this.InitWorkflowLevels = [];
                this.CloseTag();
                this.NewInitiativeSteps = 1;
                this.InitiativeDialog = false;

            },
            CloseTag() {

                this.vmTag = "";
                this.color = "";
                this.tagName = "";
                this.tagColor = "";
                this.tagID = "";
                this.isNewTag = false;
                this.selectedColor = false;
                this.AddInitTagDialog = false;
                this.TagMenu = false;

            },

            //===============CreateInitiative-Code Ends========================================

            async fnGetInitiativeList() {

                var data = new FormData();
                data.append("stage", 0);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetInitiativeDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.InitiativeTableItems = result.data;

                    }
                }
            },
            async DeleteInitiative(Type) {

                var data = new FormData();
                if (Type == 1) {
                    data.append("InitiativeId", JSON.stringify(this.selectedInitiativeID));
                    data.append("Type", 1);
                }
                else {
                    data.append("InitiativeId", JSON.stringify(this.InitiativeID));
                    data.append("Type", 2);
                }

                var result = await objInitiative.DeleteInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg = ("Successfully deleted");
                        this.snackbarMsg = true;
                    } else {

                        this.Msg = ("Failed to delete");
                        this.snackbarMsg = true;
                    }
                }
                this.fnGetInitiativeList();
                this.InitiativeID = 0;
                this.RemoveInitiativeDailog = false;
                this.RemoveBulkInitiativeDailog = false;

            },
            fnBulkClick() {
                if (this.selectedInitiativeID.length > 0) {
                    this.DeployBulkDailog = true;
                } else {
                    this.Msg = ("Please select the initiatives");
                    this.snackbarMsg = true;
                }
            },
            fnBulkRemoveClick() {
                if (this.selectedInitiativeID.length > 0) {
                    this.RemoveBulkInitiativeDailog = true;
                } else {
                    this.Msg = ("Please select the initiatives");
                    this.snackbarMsg = true;
                }
            },
            async ChangeInitiativeStatus(Type) {

                var data = new FormData();
                if (Type == 1) {
                    if (this.selectedInitiativeID.length == 0) {
                        this.Msg = ("Please select the initiatives");
                        this.snackbarMsg = true;
                        return;
                    }
                    data.append("InitiativeId", JSON.stringify(this.selectedInitiativeID));
                    data.append("Type", 1);
                    data.append("InitiativeStatus", 1)
                }
                else {
                    data.append("InitiativeId", JSON.stringify(this.InitiativeID));
                    data.append("Type", 2);
                    data.append("InitiativeStatus", 1)
                }

                var result = await objInitiative.ChangeInitiativeStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg = ("Successfully deployed");
                        this.snackbarMsg = true;
                    } else {
                        this.Msg = ("Failed to deploy");
                        this.snackbarMsg = true;
                    }
                }
                this.fnGetInitiativeList();
                this.InitiativeID = 0;
                this.DeployDailog = false;
                this.DeployBulkDailog = false;


            },
            async GetInitiative(InitiativeID, InitiativeName) {
                this.InitiativeID = InitiativeID;
                this.InitiativeName = InitiativeName;
            },
            //Tag
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },
            async fnGetAllTags() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetInitTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.AllTagList = result.data;
                } else {
                    this.Msg = ("Failed to get tag list.");
                    this.snackbarMsg = true;
                }
            },
            async fnEditTagDetails(TagItem) {
                this.AllTagListSteps = 3;
                this.TagHeaderName = 'Edit Tag Name';
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;
            },
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();

                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objInitiative.UpdateInitTagDetails(data);
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
            },
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.AddNewInitTag(data);
                
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
                        this.Msg = ("Tag already exists");
                        this.snackbarMsg = true;
                    }

            },
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
            async fnRemoveFromSelectedTagItem(TagItem) {
                this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
                this.SelectedTag = [...this.SelectedTag];
                if (this.SelectedTag.length == 0) {
                    this.allTags = false;
                }
            },
            CloseTag() {
                this.color = "";
                this.TagHeaderName = 'Add Tag';
                this.NewTagName = "";
                this.NewTagMenu = false;
                this.EditTagName = "";
            },
        },

    };
</script>
