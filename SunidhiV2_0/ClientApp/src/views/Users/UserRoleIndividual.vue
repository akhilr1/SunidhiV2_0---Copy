<template>
    <v-container fluid class="pt-0">
        <v-row no-gutters>
            <v-col cols="12" md="12">
                <v-row no-gutters>
                    <v-col>
                        <h6 class="page-head pl-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                <v-icon size="15" color="primary">mdi-account-tie-outline</v-icon>
                            </v-avatar>
                            <router-link :to="{ name: 'usersroles', query: { WsID: WorkspaceID, Access : RWAccess}}" class="page-head">User Roles</router-link>
                            <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon>
                            {{ this.roleInfo.RoleName }}
                        </h6>
                    </v-col>
                </v-row>
                <v-row class="px-2">
                    <v-col cols="12" md="6">
                        <!--Role Details section 1-->
                        <v-card>
                            <v-row class="px-4">
                                <v-col>
                                    <v-row no-gutters>
                                        <v-col class="d-inline-flex">
                                            <div class="ml-2">
                                                <v-avatar size="20" class="elevation-1 mr-1">
                                                    <v-icon size="12">mdi-account-tie-outline</v-icon>
                                                </v-avatar>
                                            </div>
                                            <div>
                                                <h5 class="page-sub-head">Role Details</h5>
                                            </div>
                                        </v-col>
                                        <v-col>
                                            <v-row>
                                                <v-spacer></v-spacer>
                                                <!-- Edit User Role -->
                                                <v-btn id="AutoTest_U123" max-width="29" min-width="29" height="27" outlined="" dark="" color="grey" class="mr-4"
                                                       @click="fnEditRoleClick()" v-bind:disabled="!screenAccess">
                                                    <v-icon>mdi-pencil-outline</v-icon>
                                                </v-btn>
                                                <!--Delete User Role-->
                                                <v-btn id="AutoTest_U124" max-width="29" min-width="29" height="27" outlined="" dark="" color="grey" class="mr-4"
                                                       @click="fnRemoveRoleClick()" v-bind:disabled="!screenAccess">
                                                    <v-icon>mdi-trash-can-outline</v-icon>
                                                </v-btn>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col cols="12" md="12" class="py-0 pt-3">
                                            <p class="pl-2" v-bind:class="{ 'text-limit': DiscriExpandBtn }">
                                                <span class="font-weight-medium">Name:</span>
                                                {{ this.roleInfo.RoleName }}
                                            </p>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col cols="12" md="12" class="py-0">
                                            <p class="pl-2" v-bind:class="{ 'text-limit': DiscriExpandBtn }">
                                                <span class="font-weight-medium">Description:</span>
                                                {{ this.roleInfo.Description }}
                                                <v-btn small text color="primary" class="pull-right" v-model="DiscriExpandBtn" v-on:click="DiscriExpandBtn=!DiscriExpandBtn">
                                                    <span v-show="DiscriExpandBtn">More</span>
                                                    <span v-show="!DiscriExpandBtn">Less</span>
                                                </v-btn>
                                            </p>

                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-card>

                        <!-- Edit User Role Dailog Starts here -->
                        <v-dialog v-model="EditRoleDailog" width="800" persistent>
                            <v-card>
                                <v-card-title primary-title class="page-head">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-account-tie-outline</v-icon>
                                    </v-avatar>
                                    Edit User Role Details
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_U125" text @click="EditRoleDailog=false">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center">
                                                <v-avatar size="75" class="elevation-1 mt-2" style="overflow:hidden">
                                                    <v-icon color="grey" size="60">mdi-account-tie-outline</v-icon>
                                                    <!--<v-btn color="#C4C4C4" height="25" style="position:absolute; bottom:0px !important">
                                                    <v-icon color="white">mdi-pencil</v-icon>
                                                </v-btn>-->
                                                </v-avatar>
                                            </v-col>
                                        </v-row>
                                        <v-col cols="12" xs="12" sm="12" md="12">
                                            <v-row justify="center">
                                                <v-col md="6">
                                                    <v-text-field outlined dense label="User Role Name" type="text" v-model="EditRoleInput.RoleName" :rules="alphanumericRules">
                                                    </v-text-field>
                                                    <v-textarea rows="3" outlined dense label="Description" type="text" v-model="EditRoleInput.Description">
                                                    </v-textarea>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-row>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <Secondary-Button id="AutoTest_U126" class="btn-122x36 mr-4" title="Save" @click.native="UpdateRole(), EditRoleDailog=false"></Secondary-Button>
                                                <Default-Button-Outlined id="AutoTest_U127" class="btn-122x36" title="Cancel" @click.native="EditRoleDailog=false"></Default-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                        <!-- Edit User Dailog Ends here -->
                        <!-- Remove role Dialog starts here -->
                        <v-dialog v-model="RemoveRoleDialog" width="800" persistent>
                            <v-card>
                                <v-card-title primary-title class="page-head py-4">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Remove User Role
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_U128" text="" @click="RemoveRoleDialog=false">
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
                                                                                    {{roleInfo.RoleName}}
                                                                                </span>
                                                                                <span class="message"> : User role has been assigned to {{RoleAssignedUsersList.length}} active users by removing this role all these users will be unasssigned and all their active task will be deactivated.</span>
                                                                            </p>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <v-col cols="12" xs="12" sm="12" md="12">
                                                                    <!--User List with checkbox add here-->
                                                                    <v-data-table :headers="RoleAssignedUsersListTableHeader"
                                                                                  :items="RoleAssignedUsersList"
                                                                                  hide-default-footer
                                                                                  item-key="ID"
                                                                                  fixed-header
                                                                                  height="25vh"
                                                                                  class="elevation-0">
                                                                        <!-- Assign Role combobox-->
                                                                        <template v-slot:item.RoleId="{item}">
                                                                            <v-autocomplete dense="" outlined="" class="mt-3"
                                                                                            :items="NewRoleAssignInput" :item-text="i => i.RoleName" :item-value="i => i.RoleId" v-model="item.RoleId"
                                                                                            label="select user role" solo flat return-object @change="fnCheckRemoveButtonStatus()">
                                                                            </v-autocomplete>
                                                                        </template>
                                                                    </v-data-table>
                                                                </v-col>
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
                                                            <Secondary-Button id="AutoTest_U129" class="btn-122x36 mr-4" title="Remove" @click.native="RemoveRole()" v-bind:disabled="btnRemoveBtnDisable"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_U130" class="btn-122x36" title="Cancel" @click.native="RemoveRoleDialog=false"></Default-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                        <!--Remove role dailog ends here-->
                        <!--Users section 2-->
                        <v-card class="mt-4">
                            <v-row no-gutters>
                                <v-col>
                                    <v-data-table :headers="usersByRoleTableHeaders"
                                                  :items="RoleAssignedUsersList"
                                                  hide-default-footer
                                                  item-key="ID"
                                                  class="elevation-0"
                                                  :search="searchaddscreen"
                                                  fixed-header height="25vh">
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
                                                        <h5 class="page-sub-head">Users</h5>
                                                    </div>
                                                </v-col>
                                                <v-col md="auto">
                                                    <v-btn id="AutoTest_U131" color="success-btn" dark class="mt-1 elevation-0" @click="GetUsersToAssignRole(), AssignUsersDialog = true" v-bind:disabled="!screenAccess">Assign User</v-btn>
                                                </v-col>
                                                <!--Assign User Dialog-->
                                                <v-dialog v-model="AssignUsersDialog" width="700" persistent>
                                                    <v-card>
                                                        <v-card-title primary-title class="page-head ml-0 ml-2">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                                <v-icon size="20" color="primary">mdi-account-outline</v-icon>
                                                            </v-avatar>
                                                            Assign users<v-spacer></v-spacer>
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_U132" text="" @click="AssignUsersDialog=false;selectedAssignUsers=[];NotifyMessage=null;searchassignuser=null">
                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-card-text class="pt-0">
                                                            <v-container fluid="" class="pt-0">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <v-row no-gutters>
                                                                            <v-col cols="12" md="auto" lg="12" class="px-0">
                                                                                <h5 class="text-left heading-4 mb-4">
                                                                                    <v-avatar size="20" class="elevation-1 mr-1">
                                                                                        <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                                                    </v-avatar>
                                                                                    Assign multiple users to the role : {{roleInfo.RoleName}}
                                                                                </h5>
                                                                                <v-text-field hide-details v-model="searchassignuser" outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify" class="mb-2">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col v-if="selectedAssignUsers.length>0" md="12">
                                                                        <div class="chip-container mb-2">
                                                                            <v-chip-group multiple
                                                                                          column
                                                                                          active-class="primary--text">
                                                                                <v-chip id="AutoTest_U133" close v-for="(item,key) in selectedAssignUsers" v-bind:key="item.ID"
                                                                                        @click:close="$delete(selectedAssignUsers, key)" small class="ma-2" color="#DBF1FF" text-color="#DBF1FF">
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
                                                                        <v-data-table :headers="assignUserTableheaders"
                                                                                      :items="assignUserTableItems"
                                                                                      :search="searchassignuser"
                                                                                      hide-default-footer
                                                                                      show-select
                                                                                      item-key="ID"
                                                                                      fixed-header
                                                                                      height="25vh"
                                                                                      class="elevation-0" v-model="selectedAssignUsers">

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
                                                                    <v-col v-if="selectedAssignUsers.length>0" cols="12" xs="12" sm="12" md="12" class="text-left">
                                                                        <v-row>
                                                                            <v-col v-if="selectedAssignUsers.length">
                                                                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedAssignUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                                <v-avatar v-if="selectedAssignUsers.length>=4" size="25" class="team-count">
                                                                                    +{{selectedAssignUsers.length-3}}
                                                                                </v-avatar>
                                                                                <p>{{NotifyMessage}} {{ roleInfo.RoleName }}</p>
                                                                            </v-col>
                                                                            <v-col>
                                                                                <!-- <v-checkbox class="float-right" color="success-check-box" v-model="NotifyCheckBox" label="Notify user"></v-checkbox> -->
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                                        <Secondary-Button id="AutoTest_U134" class="btn-122x36 mr-4" title="Add" @click.native="AssignUsersToRole()" v-bind:disabled="selectedAssignUsers.length==0"></Secondary-Button>
                                                                        <Default-Button-Outlined id="AutoTest_U135" class="btn-122x36" title="Cancel" @click.native="AssignUsersDialog=false;selectedAssignUsers=[];NotifyMessage=null;searchassignuser=null"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-dialog>
                                                <!-- Assign User Dailogue Ends here -->

                                                <v-col class="pt-4">
                                                    <v-text-field hide-details outlined dense v-model="searchaddscreen" label="Search" type="Text" prepend-inner-icon="mdi-magnify" class="filter-search-field">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </template>

                                        <!--users name row-->
                                        <template v-slot:item.Name="{item}">
                                            <router-link class="text--default" link="" :to="{ name: 'userindividual', query: { WsID: WorkspaceID , Access : RWAccess,UID: item.ID} }">
                                                <v-avatar size="30" class="elevation-1 mr-1" light :color="`${getcolor(item.Name)}`">
                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>
                                                {{ item.Name }}
                                            </router-link>
                                        </template>

                                        <!--users table action row-->
                                        <template v-slot:item.userActions="{item}">
                                            <v-btn id="AutoTest_U136" max-width="29" min-width="29" height="27" outlined="" dark="" color="grey" class="mr-2"
                                                   @click="GetAllUserProjects(item),DeleteAssignUserDialog=true,delUser=item.Name,delUserID= item.ID" v-bind:disabled="!screenAccess">
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-btn>


                                        </template>
                                    </v-data-table>
                                </v-col>
                            </v-row>
                        </v-card>
                    </v-col>

                    <v-col cols="12" md="6">
                        <!--Screen section 3-->
                        <v-card>
                            <v-row>
                                <!-- screens table -->
                                <v-col>
                                    <v-row class="px-4">
                                        <v-col class="d-inline-flex">
                                            <div>
                                                <v-avatar size="20" class="elevation-1 mr-1">
                                                    <v-icon size="12" color="grey">mdi-monitor-screenshot</v-icon>
                                                </v-avatar>
                                            </div>
                                            <div>
                                                <h5 class="page-sub-head">Screens</h5>
                                            </div>
                                        </v-col>

                                        <v-col md="auto" class="pt-0">
                                            <v-btn id="AutoTest_U140" color="success-btn" dark class="mt-1 elevation-0" @click="GetUserWithoutscreens(), AddScreensDialog=true" v-bind:disabled="!screenAccess">Add Screen</v-btn>
                                        </v-col>

                                        <v-dialog v-model="AddScreensDialog" width="700" persistent>
                                            <v-card>
                                                <v-card-title primary-title class="page-head ml-2">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                        <v-icon size="20" color="primary">mdi-monitor-screenshot</v-icon>
                                                    </v-avatar>
                                                    Add Screens
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_U141" text="" @click="AddScreensDialog=false;selectedaddscreen=[];AddScreenNotifyMessage=null;searchaddscreen=null">
                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>

                                                <v-card-text class="pt-0">
                                                    <v-container fluid="" class="pt-0">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-row no-gutters="">
                                                                    <v-col cols="12" md="auto" lg="12">
                                                                        <h5 class="text-left heading-4 mb-4 pt-1">
                                                                            <v-avatar size="20" class="elevation-1 mr-1">
                                                                                <v-icon size="25" color="success">mdi-plus-circle</v-icon>
                                                                            </v-avatar>
                                                                            Assign multiple Screens to the role : {{roleInfo.RoleName}}
                                                                        </h5>
                                                                        <v-text-field hide-details class="mb-2" v-model="searchaddscreen" outlined="" dense="" label="Search" type="Text" prepend-inner-icon="mdi-magnify">
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <div class="chip-container mb-2" v-if="selectedaddscreen.length>0">
                                                                <v-chip-group multiple
                                                                              column
                                                                              active-class="primary--text">
                                                                    <v-chip id="AutoTest_U142" close v-for="(item,key) in selectedaddscreen" v-bind:key="item.screenId"
                                                                            @click:close="$delete(selectedaddscreen, key)" small class="ma-2">
                                                                        <v-icon left color="#3374B9" size="15">mdi-monitor-screenshot</v-icon>
                                                                        {{ item.ScreenName }}
                                                                    </v-chip>
                                                                </v-chip-group>
                                                            </div>
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <!--User List with checkbox add here-->
                                                                <v-data-table :headers="addScreenTableheaders"
                                                                              :items="addScreenTableItems"
                                                                              :search="searchaddscreen"
                                                                              hide-default-footer
                                                                              show-select
                                                                              item-key="ScreenId"
                                                                              fixed-header
                                                                              height="25vh"
                                                                              class="elevation-0" v-model="selectedaddscreen">
                                                                    <template v-slot:item.AccessId="props">
                                                                        <v-autocomplete dense outlined class="mt-3"
                                                                                        :items="AccessTypeValues" :item-text="i => i.AccessName" :item-value="i => i.AccessId" v-model="props.item.AccessId">
                                                                        </v-autocomplete>
                                                                    </template>
                                                                </v-data-table>
                                                            </v-col>
                                                            <v-col v-if="selectedaddscreen.length>0" cols="12" xs="12" sm="12" md="12" class="text-left pt-1">
                                                                <v-row>
                                                                    <v-col v-if="selectedaddscreen.length">
                                                                        <!--<v-avatar size="20" class="mb=1">
                                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                                             alt="User DP" />
                                                                    </v-avatar> -->
                                                                        {{AddScreenNotifyMessage}} {{ roleInfo.RoleName }}
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right mt-4">
                                                                <Secondary-Button id="AutoTest_U143" class="btn-122x36 mr-4" title="Add" @click.native="AddScreens()" v-bind:disabled="selectedaddscreen.length==0"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_U144" class="btn-122x36" title="Cancel" @click.native="AddScreensDialog=false;selectedaddscreen=[];AddScreenNotifyMessage=null;searchaddscreen=null"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-card-text>
                                            </v-card>
                                        </v-dialog>

                                        <v-col class="pt-0">
                                            <v-text-field hide-details outlined dense label="Search" v-model="searchscreen" type="Text" prepend-inner-icon="mdi-magnify" class="filter-search-field mt-1">
                                            </v-text-field>
                                        </v-col>
                                    </v-row>

                                    <v-data-table :headers="roleScreensTableheaders"
                                                  :items="roleScreensTableItems"
                                                  :search="searchscreen"
                                                  hide-default-footer
                                                  item-key="ScreenName"
                                                  class="elevation-0"
                                                  fixed-header height="38vh">
                                        <template v-slot:item.screenActions="{item}">
                                            <v-btn id="AutoTest_U145" max-width="29" min-width="29" height="27" outlined="" dark="" color="grey" class="mr-2"
                                                   @click.stop="DeleteScreenDialog=true,DeleteScreenID=item.ScreenId" v-bind:disabled="!screenAccess">
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-btn>
                                        </template>
                                    </v-data-table>

                                    <!--Delete Screen from Role Dialog-->
                                    <v-dialog v-model="DeleteScreenDialog" width="700" persistent>
                                        <v-card>
                                            <v-card-title primary-title class="page-head">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                                </v-avatar>
                                                Delete Screen from Role
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_U146" text="" @click="DeleteScreenDialog=false,DeleteScreenID=0">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>
                                            <v-card-text class="pt-0">
                                                <v-container fluid="" class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <h4 class="text-center heading-3">
                                                                Are you sure you want to remove this screen from role?
                                                            </h4>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <Secondary-Button id="AutoTest_U147" class="btn-122x36 mr-4" title="Delete" @click.native="fnDeleteScreenFromRole()"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_U148" class="btn-122x36" title="Cancel" @click.native="DeleteScreenDialog=false,DeleteScreenID=0"></Default-Button-Outlined>
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
                </v-row>
            </v-col>
        </v-row>
        <!-- Remove User dailog box -->
        <v-dialog v-model="DeleteAssignUserDialog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove User from User Role
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_U137" text="" @click="DeleteAssignUserDialog=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-0">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row no-gutters>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure want to remove this user from this role ?</h4>
                                            <h4 class="text-center heading-3">If Yes, then please slect a new role for this user.</h4>
                                        </v-col>
                                        <v-row>
                                            <v-col v-if="userProjectTableItems.length>0" cols="12" md="12" class="pt-2">
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
                                                                    {{delUser}}<v-spacer></v-spacer>
                                                                </span>
                                                                <span class="message">  is part of {{userProjectTableItems.length}}  projects by removing this user all active tasks will be inactive and will have to be reassigned at project setup.</span>
                                                            </p>
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </v-list>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                <v-select dense="" outlined="" hide-details :items="NewRoleAssignInput" :item-text="i => i.RoleName" :item-value="i => i.RoleId"
                                                          label="select user role" solo flat return-object v-model="NewAssignedRoleId" @change="fnConfirmDeleteBtnStatus()"></v-select>
                                            </v-col>
                                            <!-- Project table list-->
                                            <v-col v-if="userProjectTableItems.length>0" cols="12" xs="12" sm="12" md="12">
                                                <v-data-table :headers="userProjectsTableheaders"
                                                              :items="userProjectTableItems"
                                                              item-key="ProjectId"
                                                              fixed-header
                                                              height="20vh"
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
                                            <!-- Notify User-->
                                            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                <label class="float-left mt-4">
                                                    <!--<v-avatar size="20" class="mb=1">
                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                         alt="User DP" />
                                                    <span class="black--text bottom"
                                                style="position:absolute; bottom:0px !important"></span>
                                                </v-avatar>-->
                                                    {{delUser}} will be notified
                                                </label>
                                                <!-- <v-checkbox color="success-check-box" label="Notify user" class="float-right" v-model="NotifyCheckBox"></v-checkbox> -->
                                            </v-col>
                                        </v-row>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <Secondary-Button id="AutoTest_U138" class="btn-122x36 mr-4" title="Confirm" @click.native="fnDeleteAssignUser(delUserID)" v-bind:disabled="btnRemoveUserStatus"></Secondary-Button>
                                            <Default-Button-Outlined id="AutoTest_U139" class="btn-122x36" title="Cancel" @click.native="DeleteAssignUserDialog=false"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <!-- Remove a User from a UserRole dailog box Ends here -->
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

    var objRoleBL;
    async function importscript() {
        return Promise.all([
            import("../../BL/UserRole.js").then(mod => {
                objRoleBL = new mod.UserRole()
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
              Msg: "",
              snackbarMsg:false,
              successSnackbarMsg: false,
              errorSnackbarMsg: false,
              infoSnackbarMsg: false,
              delUser: "",
              delUserID :0,
              //Role Details section
              roleInfo: {
                  RoleId: 0,
                  RoleName: "",
                  UpdatedDate: "",
                  Description: "",
                  WorkspaceID: 0,
              },
              DiscriExpandBtn: true,
              AllRolesList: [], //all role list needed when assigning new role for Remove Role click
              //Edit Role in role section
              EditRoleInput: {
                  RoleId: 0,
                  RoleName: "",
                  Description: "",
                  ScreenList: [],
                  WorkspaceID: 0
              },
              EditRoleDailog: false,
              //Remove Role in role section
              RemoveRoleDialog: false,
              IsHideDeleteMsg: false,
              RoleAssignedUsersList: [],
              RemoveRoleNotifyMsg: "",
              btnRemoveBtnDisable: true,
              NewRoleAssignInput: [],
              RoleAssignedUsersListTableHeader: [
                  { text: 'ID', value: 'ID', align: ' d-none' },
                  { text: 'Users', align: 'left', value: 'Name' },
                  { text: 'Status', value: 'Status' },
                  { text: 'Tasks', value: 'Tasks' },
                  { text: 'User Role', value: 'RoleId' },
              ],

              //Users section details
              usersByRoleTableHeaders: [
                  { text: 'Name', align: 'left', value: 'Name' },
                  { text: 'Project Count', value: 'ProjectsCount' },
                  { text: "Actions", value: "userActions", sortable: false }
              ],
              //usersByRoleTableItems: [],
              searchaddscreen: "",
              AssignUsersDialog: false,
              assignUserTableItems: [], //Assign Users button click dialog
              assignUserTableheaders: [
                  {
                      text: 'Name',
                      align: 'left',
                      value: 'Name',
                  },
                  { text: 'Projects Count', value: 'ProjectsCount' },
                  { text: 'EmailID', value: 'EmailID', align: ' d-none' },
              ],
              searchassignuser: "",
              selectedAssignUsers: [],
              NotifyMsg: "",
              DeleteAssignUserDialog: false,
              userProjectsTableheaders: [
                  { text: "Project Name", align: "left", value: "ProjectName" },
                  { text: "Status", value: "ProjectStatus" },
                  { text: "Tasks", value: "Tasks", sortable: "false" }
              ],
              userProjectTableItems: [],
              ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "" },
              NewAssignedRoleId: 0, //when removing user, need to assign new role other than already assigned
              btnRemoveUserStatus: true,

              //screens section data variables
              roleScreensTableheaders: [
                  {
                      text: 'Screen Name',
                      align: 'left',
                      value: 'ScreenName',
                  },
                  { text: 'Access Type', value: 'AccessName' },
                  { text: 'Preview', value: 'ScreenPath' },
                  { text: 'Actions', value: 'screenActions', sortable: false },
              ],
              roleScreensTableItems: [],
              AccessTypeValues: [],
              searchscreen: "",

              AddScreensDialog: false,
              addScreenTableheaders: [
                  {
                      text: 'Screen Name',
                      align: 'left',
                      value: 'ScreenName',
                  },
                  { text: 'Access Type', value: 'AccessId' },
                  { text: 'Preview', value: 'ScreenPath' },

              ],
              addScreenTableItems: [],
              searchaddscreen: "",
              selectedaddscreen: [],
              AddScreenNotifyMessage: "",
              DeleteScreenDialog: false,
              DeleteScreenID: 0,

              WorkspaceID: 0,
              NotifyCheckBox: true,
              alphanumericRules: [
                  v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name'
              ],
              screenAccess: 0,
              RWAccess: 0,
              userimage: "",
          };
      },
      created() {
           if (this.$route.query.RoleId) {
                this.roleInfo.RoleId = this.$route.query.RoleId;
          }
      },
      mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.userimage = window.SERVER_URL + "/Upload";
            await importscript();
            await this.GetRoleDetails(); 
            await this.GetRoleBasedUsersList();
            await this.GetAllRolesList(); //get all roles list for WsID
            await this.GetScreensListByRole(); // screen list by RoleId
            await this.GetAccessTypeList(); // get Access Type values
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
            ///  GetRoleDetails :- Function used to fetch role details for role section
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetRoleDetails() {
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                var result = await objRoleBL.GetRoleDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.roleInfo = result.data;
                }
            },

            /// <summary>
            ///  fnEditRoleClick :- Function used to open role details for edit
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnEditRoleClick() {
                this.EditRoleDailog = true;
                this.EditRoleInput.RoleId = this.roleInfo.RoleId;
                this.EditRoleInput.RoleName = this.roleInfo.RoleName;
                this.EditRoleInput.Description = this.roleInfo.Description;
                this.EditRoleInput.WorkspaceID = this.roleInfo.WorkspaceID;
            },

            /// <summary>
            ///  UpdateRole :- Function used to update user role details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateRole() {
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
                this.EditRoleDailog = false;
                this.EditRoleInput.RoleId = 0;
                this.EditRoleInput.RoleName = "";
                this.EditRoleInput.Description = "";
                this.GetRoleDetails();
            },

            /// <summary>
            ///  GetRoleBasedUsersList :- Function used to fetch list of assigned users for user section and remove role section
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns> 
            async GetRoleBasedUsersList() {
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                data.append("WorkspaceID", this.roleInfo.WorkspaceID);

                var result = await objRoleBL.GetRoleBasedUsersList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.RoleAssignedUsersList = result.data;
                }
            },

            /// <summary>
            ///  fnRemoveRoleClick :- Function used to remove Role in role section
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnRemoveRoleClick() {
                this.RemoveRoleDialog = true;
                if (this.RoleAssignedUsersList.length > 0) {
                    this.IsHideDeleteMsg = false
                    var UserCount = this.RoleAssignedUsersList.length;

                    if (UserCount == 1) {
                        this.RemoveRoleNotifyMsg = this.RoleAssignedUsersList[0].Name + " will be notify";
                    } else
                    if (UserCount == 2) {
                        this.RemoveRoleNotifyMsg = this.RoleAssignedUsersList[0].Name + ","
                             + this.RoleAssignedUsersList[1].Name + " will be notify";
                    } else
                     if (UserCount > 2) {
                         this.RemoveRoleNotifyMsg = this.RoleAssignedUsersList[0].Name + ","
                             + this.RoleAssignedUsersList[1].Name + "+", UserCount - 2 + " will be notify";
                     }
                } else {
                    this.btnRemoveBtnDisable = false;
                    this.IsHideDeleteMsg = true;
                }
            },

            /// <summary>
            ///  fnCheckRemoveButtonStatus :- Function used to checks whether new role is assigned for all users or not
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns> 
            fnCheckRemoveButtonStatus() {
                if (this.RoleAssignedUsersList.length > 0) {
                    var NewRoleAssignCount = 0; // intiatizing to default
                    for (var i = 0; i <= this.RoleAssignedUsersList.length - 1; i++) {
                        try {
                            if (this.RoleAssignedUsersList[i].RoleId.RoleName != "" && typeof (this.RoleAssignedUsersList[i].RoleId.RoleName) != "undefined") {
                                NewRoleAssignCount += 1;
                            }
                        }
                        catch{
                            this.NewRoleAssignCount = 0;
                        }
                    }

                    if (NewRoleAssignCount == this.RoleAssignedUsersList.length) {
                        this.btnRemoveBtnDisable = false;
                    } else {
                        this.btnRemoveBtnDisable = true;
                    }
                }
            },

            /// <summary>
            ///  RemoveRole :- Function used to remove role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async RemoveRole() {
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("UsersList", JSON.stringify(this.RoleAssignedUsersList));
                data.append("Notify", this.NotifyCheckBox);
                
                var result = await objRoleBL.DeleteUserRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully Removed");
                    this.successSnackbarMsg = true;
                    this.$router.push({ name: "usersroles", query: { WsID: this.WorkspaceID, Access: RWAccess} });
                } else {
                    this.Msg = ("Failed to Remove");
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  GetAllRolesList :- Function used to fetch all roles list for WsID to use it in assigning new role when Remove Role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllRolesList() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.GetUserRolesList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AllRolesList = result.data;
                    this.NewRoleAssignInput = this.AllRolesList.filter(p => p.RoleId != this.roleInfo.RoleId);
                }
            },

            /// <summary>
            ///  GetUsersToAssignRole :- Function used to fetch user list from WsID to assign role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUsersToAssignRole() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.GetUsersToAssignRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.assignUserTableItems = result.data;
                    
                }
            },

            /// <summary>
            ///  AssignUsersToRole :- Function used to add/save users to role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AssignUsersToRole() {
                if (this.selectedAssignUsers.length == 0) {
                    this.Msg = ("Please select at least one User");
                    this.infoSnackbarMsg = true;
                    return;
                }

                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                data.append("UserList", JSON.stringify(this.selectedAssignUsers));
                data.append("RoleName", this.roleInfo.RoleName);
                data.append("notify", this.NotifyCheckBox);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objRoleBL.AssignUsersToRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully Assigned Users");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Assign Users");
                    this.errorSnackbarMsg = true;
                }
                this.AssignUsersDialog = false;
                this.selectedAssignUsers = [];
                this.GetRoleBasedUsersList(); //refresh the users section after assigning users
            },

            /// <summary>
            ///  GetAllUserProjects :- Function used to fetch all projects based on UserID when click on remove button in users section list
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            async GetAllUserProjects(item) {
                var result = await objRoleBL.GetAllUserProjects(item.ID);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.userProjectTableItems = result.data;
                }
                if (this.userProjectTableItems.length == 0) {
                    this.isHidden = true;
                }
                else {
                    this.isHidden = false;
                }
            },

            /// <summary>
            ///  fnConfirmDeleteBtnStatus :- Function used to check whether new role assigned when removing user 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnConfirmDeleteBtnStatus() {
                if (this.NewAssignedRoleId.RoleId != 0) {
                    this.btnRemoveUserStatus = false;
                } else {
                    this.btnRemoveUserStatus = true;
                }
            },

            /// <summary>
            ///  fnDeleteAssignUser :- Function used to remove user from the role and assign new role to the user
            /// </summary>
            /// <param name="delID"></param>
            /// <returns> </returns>
            async fnDeleteAssignUser(delID) {
               
                var item = this.RoleAssignedUsersList.filter(x => x.ID == delID)[0];
                var data = new FormData();
                data.append("userID", delID);
                data.append("userName", this.delUser);
                data.append("userEmail", item.EmailID);
                data.append("NewRoleId", this.NewAssignedRoleId.RoleId);
                data.append("NewRoleName", this.NewAssignedRoleId.RoleName);
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("notify", this.NotifyCheckBox);
                var result = await objRoleBL.DeleteAssignedUser(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg =("Successfully Removed and assigned new role");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg =("Failed to Remove and assigned new role");
                    this.errorSnackbarMsg = true;
                }
                this.GetRoleBasedUsersList();
                this.NewAssignedRoleId = 0;
                this.btnRemoveUserStatus = true;
                this.DeleteAssignUserDialog = false;
            },

            //Screens section -- 
            /// <summary>
            ///  GetScreensListByRole :- Function used to fetch screen list based on RoleId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetScreensListByRole() {
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                var result = await objRoleBL.GetScreensListByRole(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.roleScreensTableItems = result.data;
                }
            },

            /// <summary>
            ///  GetAccessTypeList :- Function used to fetch all AccessType values
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
            ///  GetUserWithoutscreens :- Function used to fetch User without using screen based on RoleId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUserWithoutscreens() {
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                var result = await objRoleBL.GetUserWithoutScreens(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.addScreenTableItems = result.data;
                }
            },

            /// <summary>
            ///  AddScreens :- Function used to add screens to the role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async AddScreens() {
                if (this.selectedaddscreen.length == 0) {
                    this.Msg = ("Please select at least one User");
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                data.append("RoleName", this.roleInfo.RoleName);
                data.append("ScreenList", JSON.stringify(this.selectedaddscreen));
                data.append("WorkspaceID", this.WorkspaceID);

                var result = await objRoleBL.AddRole(data)
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully Added");
                    this.successSnackbarMsg = true;
                } else {
                    this.Msg = ("Failed to Add");
                    this.errorSnackbarMsg = true;
                }
                this.AddScreensDialog = false;
                this.selectedaddscreen = [];
                this.GetScreensListByRole();
            },

            /// <summary>
            ///  fnDeleteScreenFromRole :- Function used to delete screen from role
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnDeleteScreenFromRole() {
                this.DeleteScreenDialog = false;
                var data = new FormData();
                data.append("RoleId", this.roleInfo.RoleId);
                data.append("ScreenId", this.DeleteScreenID);
                var result = await objRoleBL.DeleteScreenFromRole(data);
                if (result.status = "success") {
                    this.Msg = ("Successfully Removed the screen from role");
                    this.successSnackbarMsg = true;
                    this.GetScreensListByRole(); //refresh the screen after removing
                } else {
                    this.Msg = ("Failed to Remove the screen from role");
                    this.errorSnackbarMsg = true;
                }
            },
        },
        watch: {
            /// <summary>
            ///  selectedaddscreen :- Function used to notify the users with a message when selecting screen from the table
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            selectedaddscreen: function () {
                this.AddScreenNotifyMessage = "";
                if (this.selectedaddscreen.length > 0) {
                    if (this.selectedaddscreen.length > 2) {
                        this.AddScreenNotifyMessage = this.selectedaddscreen[0].ScreenName + "," + this.selectedaddscreen[1].ScreenName + " and " + (this.selectedaddscreen.length - 2) + " more screens will be assigne to the role ";
                    } else if (this.selectedaddscreen.length == 2) {
                        this.AddScreenNotifyMessage = this.selectedaddscreen[0].ScreenName + "," + this.selectedaddscreen[1].ScreenName + " screens will be assigned to the role ";
                    } else if (this.selectedaddscreen.length == 1) {
                        this.AddScreenNotifyMessage = this.selectedaddscreen[0].ScreenName + " screen will be assigned the role ";
                    }
                }
            },

            /// <summary>
            ///  selectedAssignUsers :- Function used to notify the users with a message when selecting users from the table
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            selectedAssignUsers: function () {
                this.NotifyMessage = "";
                if (this.selectedAssignUsers.length > 0) {
                    if (this.selectedAssignUsers.length > 2) {
                        this.NotifyMessage = this.selectedAssignUsers[0].Name + "," + this.selectedAssignUsers[1].Name + " and " + (this.selectedAssignUsers.length - 2) + " more will be assigne the role ";
                    } else if (this.selectedAssignUsers.length == 2) {
                        this.NotifyMessage = this.selectedAssignUsers[0].Name + "," + this.selectedAssignUsers[1].Name + " will be assigned the role ";
                    } else if (this.selectedAssignUsers.length == 1) {
                        this.NotifyMessage = this.selectedAssignUsers[0].Name + " will be assigned the role ";
                    }
                }
            }
        },
  };
</script>
