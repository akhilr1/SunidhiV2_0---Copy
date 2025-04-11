<template>
    <v-container fluid class="pt-0">
        <v-row no-gutters>
            <v-col cols="12" md="12">
                <v-row no-gutters>
                    <v-col v-for="(item, i) in userTableItems" :key="i">
                        <h5 class="page-head pl-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-account-outline</v-icon>
                            </v-avatar>
                            <router-link :to="{ name: 'userlist', query: {WsID: WorkspaceID,Access:RWAccess}}" class="page-head">Users</router-link>
                            <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon>
                            {{item.Name}}
                        </h5>
                    </v-col>
                </v-row>
                <v-row class="px-2">
                    <v-col cols="12" md="6">
                        <v-card>
                            <v-row class="px-4">
                                <v-col v-for="(item, i) in userTableItems" :key="i">
                                    <v-row no-gutters>
                                        <v-col class="d-inline-flex">
                                            <div>
                                                <v-avatar size="20" class="elevation-1 mr-1">
                                                    <v-icon size="12" color="grey">mdi-account-outline</v-icon>
                                                </v-avatar>
                                            </div>
                                            <div>
                                                <h6 class="page-sub-head">
                                                    User Details
                                                </h6>
                                            </div>
                                        </v-col>
                                        <v-col>
                                            <v-row>
                                                <v-spacer></v-spacer>
                                                <!--Edit User No need edit here -->
                                                <!--<v-btn max-width="29" min-width="29" height="27" outlined dark color="grey" class="mr-2">
                                                <v-icon>mdi-pencil-outline</v-icon>
                                            </v-btn>-->
                                                <!--Edit user role ends here -->
                                                <!-- Delete Button-->
                                                <v-btn id="AutoTest_U71" max-width="29" min-width="29" height="27" outlined="" dark="" color="grey" class="mr-2"
                                                       @click="DeleteUserDialog = true ,uName= item.Name, uPrjCount =item.userProjectCount"
                                                       v-if=!item.UserRoleStatus v-bind:disabled="!screenAccess">
                                                    <v-icon>mdi-trash-can-outline</v-icon>
                                                </v-btn>
                                            </v-row>
                                        </v-col>
                                    </v-row>

                                    <v-row>
                                        <v-col>
                                            <v-list class="py-0">
                                                <v-list-item class="my-0">
                                                    <v-list-item-icon>
                                                        <v-avatar size="50" light :color="`${getcolor(item.Name)}`">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="headline white--text" v-else>  {{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                    </v-list-item-icon>
                                                    <v-list-item-content class="pl-2">
                                                        {{item.Name}}
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                            <v-spacer></v-spacer>
                                        </v-col>

                                        <v-col>
                                            <v-spacer></v-spacer>
                                            <p class="pt-3 mt-4">Organization : {{item.userOrganisation}}</p>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col>
                                            <p class="pl-2">Email : {{item.EmailId}}</p>
                                            <p class="pl-2">Role  : {{item.userRole}}</p>
                                        </v-col>
                                    </v-row>

                                </v-col>
                            </v-row>
                        </v-card>

                        <!-- Remove User role Dialog starts here -->

                        <v-dialog v-model="DeleteUserDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head">
                                    <v-avatar color="white" size="30" class="elevation-1 mr-2">
                                        <v-icon size="25" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Remove User
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_U72" text @click="DeleteUserDialog=false">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <v-container fluid class="pt-0">
                                                    <v-row>
                                                        <v-col cols="12" md="12" class="pt-0">
                                                            <h4 class="text-center page-sub-head">Are you sure want to remove this user role ?</h4>
                                                        </v-col>
                                                        <!--<v-col cols="12" md="12" class="pt-0" style="background-color:#ffe9a6">
                                                        <v-avatar color="white" size="30" class="elevation-1 mr-2">
                                                            <v-icon size="16" color="primary">mdi-account-convert</v-icon>
                                                        </v-avatar>
                                                        <h4  class="page-sub-head" style="white-space:nowrap">Data collector </h4><h4 style="color:orange"> User role has been assign to {{9}} active other users by removing this role all these users will be unasssigned and all their active task will be deactivated</h4>
                                                    </v-col>-->
                                                        <v-list-item style="background-color:#ffe9a6">
                                                            <v-list-item-icon>
                                                                <v-avatar size="20" class="elevation-1">
                                                                    <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                </v-avatar>
                                                            </v-list-item-icon>
                                                            <v-list-item-content class="page-sub-head">
                                                                {{uName}}: is part of {{uPrjCount}}  projects by removing this user all active tasks will be inactive and will have to be reassigned at project setup
                                                            </v-list-item-content>
                                                        </v-list-item>

                                                        <v-col cols="12" xs="12" sm="12" md="12">
                                                            <!--User List with checkbox add here-->
                                                            <v-data-table :headers="userProjectsTableheaders"
                                                                          :items="userProjectTableItems"
                                                                          :search="searchuser"
                                                                          hide-default-footer
                                                                          item-key="$('ProjectName'+1)"
                                                                          fixed-header
                                                                          height="25vh"
                                                                          class="elevation-0">

                                                            </v-data-table>
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <!-- <v-checkbox color="success-check-box" label="Notify user" class="float-right"></v-checkbox> -->
                                                        </v-col>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                            <Secondary-Button id="AutoTest_U73" class="btn-122x36 mr-4" title="Remove" @click.native="fnDeleteUser()"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_U74" class="btn-122x36" title="Cancel" @click.native="DeleteUserDialog = false"></Default-Button-Outlined>
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

                        <v-card class="mt-4">
                            <v-row class="px-4">
                                <v-col class="d-inline-flex">
                                    <div>
                                        <v-avatar size="20" class="elevation-1 mr-2">
                                            <v-icon size="12" color="grey">mdi-account-group-outline</v-icon>
                                        </v-avatar>
                                    </div>
                                    <div>
                                        <h4 class="page-sub-head">User Groups</h4>
                                    </div>
                                </v-col>
                                <v-col>
                                    <v-row>
                                        <v-text-field hide-details outlined dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="SearchGroupName" class="filter-search-field mr-2">
                                        </v-text-field>
                                    </v-row>
                                </v-col>
                            </v-row>
                            <v-row>
                                <!-- user groups table -->
                                <v-col>
                                    <v-data-table :headers="userGroupTableheaders"
                                                  :items="userGroupTableItems"
                                                  hide-default-footer
                                                  item-key="GroupName"
                                                  class="elevation-0"
                                                  :search="SearchGroupName"
                                                  fixed-header
                                                  height="35vh">

                                        <template v-slot:item.GroupName="{item}">
                                            <router-link class="text--default" link :to="{ name: 'usergroupindividual', query: { WsID: WorkspaceID,Access:RWAccess ,GroupId: item.GroupId } }">
                                                <v-list class="py-0 table-v-list">
                                                    <v-list-item class="my-0 px-0">
                                                        <v-list-item-icon>
                                                            <v-avatar size="30" light :color="`${getcolor(item.GroupName)}`">
                                                                <img v-bind:src="userimage + item.UserGroupImage" alt="User DP" v-if="item.UserGroupImage.length >0" />
                                                                <span class="white--text" v-else>{{item.GroupName == null ? "" : item.GroupName.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            {{item.GroupName}}
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </v-list>
                                            </router-link>
                                        </template>
                                    </v-data-table>
                                </v-col>
                            </v-row>
                        </v-card>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-card>
                            <v-row class="px-4">
                                <v-col class="d-inline-flex">
                                    <div>
                                        <v-avatar size="20" class="elevation-1 mr-2">
                                            <v-icon size="12" color="grey">mdi-file-document-box-multiple-outline</v-icon>
                                        </v-avatar>
                                    </div>
                                    <div>
                                        <h6 class="page-sub-head">
                                            Projects
                                        </h6>
                                    </div>
                                </v-col>
                                <v-col>
                                    <v-row>
                                        <v-text-field outlined hide-details dense label="Search" type="Text" prepend-inner-icon="mdi-magnify" v-model="SearchPrjName" class="filter-search-field mr-2">
                                        </v-text-field>
                                    </v-row>
                                </v-col>
                            </v-row>
                            <v-row>
                                <!-- Project  table -->
                                <v-col>
                                    <v-data-table :headers="userProjectsTableheaders"
                                                  :items="userProjectTableItems"
                                                  hide-default-footer
                                                  item-key="$('ProjectName'+2)"
                                                  class="elevation-0"
                                                  :search="SearchPrjName"
                                                  fixed-header
                                                  height="35vh">
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
<script  type="text/javascript">
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
                dense: false,
                SearchGroupName: "",
                SearchPrjName: "",
                DeleteUserDialog: false ,

                userTableItems: [],
                userProjectTableItems: [],

                userGroupTableheaders: [
                    {
                        text: "Name",
                        align: "left",
                        value: "GroupName",
                        icon: "mdi-account-multiple"
                    },
                    { text: "ProjectName", value: "ProjectName" },
                    //{ text: "Role", value: "RoleName", sortable: "false" }
                ],

                userGroupTableItems: [],

                userProjectsTableheaders: [
                    { text: "Project Name", align: "left", value: "ProjectName" },
                    { text: "Status", value: "ProjectStatus" },
                   // { text: "Role", value: "RoleName", sortable: "false" }
                ],
                uName: "",
                uPrjCount: 0,
                searchuser: "",
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
            await this.GetUserIndividualDetails();           
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
            ///  GetUserIndividualDetails :- Function used to fetch user individual details based on userID,WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUserIndividualDetails() {
                
                var data = new FormData();
                data.append("userId", this.UserID);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objUser.GetUserIndividualDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) 
                        this.userTableItems = result.data;                 
                }

                result = await objUser.GetAllActiveUserGroup_Ind(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) 
                        this.userGroupTableItems = result.data;
                    
                }

                result = await objUser.GetAllUserProjects(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data)
                    this.userProjectTableItems = result.data;
                }
            },

            /// <summary>
            ///  fnDeleteUser :- Function used to delete user based on userId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnDeleteUser() {
               
                var data = new FormData();
                data.append("userId", this.UserID);
               
                var result = await objUser.DeleteUser(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.DeleteUserDialog = false;
                        this.Msg = ("User is removed successfully");
                        this.successSnackbarMsg = true;
                    }
                }
                this.$router.push({ name: 'userlist', query: { WsID: this.WorkspaceID } });
            },
        }
    };  
 
</script>