<template>
    <v-card>
        <v-container fluid class="px-0">
            <v-row class="px-4">
                <v-col>
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary"> mdi-folder-star-outline</v-icon>
                        </v-avatar>
                        Deployed Initiative
                    </h3>
                </v-col>
                <v-col md="auto">
                    <v-row>
                        <v-spacer></v-spacer>
                        <!--Create Initiative Button-->
                        <v-btn height="39" color="success-btn" dark class="elevation-0 mr-4"
                               link :to="{ name: 'initiatives', query: { WsID: WorkspaceID, Access:RWAccess , InDlg: true }}" v-bind:disabled ="!screenAccess">Create Initiatives</v-btn>

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

                                <v-list-item id="AutoTest_I17" @click.stop="fnBulkRemoveClick">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                            <v-dialog v-model="RemoveBulkInitiativeDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Initiative
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I18" text @click="RemoveBulkInitiativeDailog=false">
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
                                                                                    <span class="message">Selected initiatives have {{0}} projects in it and by removing this initiative all the active projects and tasks will become inactive.</span>
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
                                                                                  item-key="ProjectId"
                                                                                  fixed-header
                                                                                  height="25vh"
                                                                                  class="elevation-0">


                                                                    </v-data-table>
                                                                </v-col>

                                                            </div>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_I19" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(1),RemoveBulkInitiativeDailog=false">Remove</v-btn>
                                                                <v-btn id="AutoTest_I20" outlined color="grey" @click="RemoveBulkInitiativeDailog=false">Cancel</v-btn>
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
                        <v-text-field label="Search" class="mr-4 table-search"
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
                                  show-select=""
                                  v-model="selectedInitiativeID"
                                  item-key="InitiativeId"
                                  :search="searchInitiative"
                                  class="elevation-0">
                        <!--Initiative list column Name-->
                        <template v-slot:item.InitiativeName="{ item }">
                            <router-link class="text--default text-link" link :to="{ name: 'DraftProjects', query: { WsID: WorkspaceID, Access: RWAccess, InitID: item.InitiativeId } }">
                                {{ item.InitiativeName }}
                            </router-link>
                        </template>

                        <!-- column State-->
                        <template v-slot:item.State="{ item }">
                            <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" @change="GetAllUserProjects(item.InitiativeId,item.InitiativeName,item.IsActive),IsActiveToggleDialog = true" v-bind:disabled ="!screenAccess"></v-switch>
                        </template>

                        <!-- Deployed Initiatives Actions-->
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
                                    <v-list-item id="AutoTest_I21" @click.stop="" link :to="{ name: 'ManageInitiatives', query: { WsID: WorkspaceID, Access: RWAccess,InitiativeID : item.InitiativeId,InitiativeType: 2 }}">
                                        <v-list-item-icon>
                                            <v-icon>mdi-pencil-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Edit
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_I22" @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),ArchiveDailog=true,InitiativeName =item.InitiativeName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Archive
                                        </v-list-item-content>
                                    </v-list-item>

                                    <v-list-item id="AutoTest_I23" @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),RemoveInitiativeDailog=true,InitiativeName =item.InitiativeName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>
                                <v-dialog v-model="ArchiveDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Archive Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I24" text="" @click="ArchiveDailog=false,InitiativeName =null">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to Archive this initiative  ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{InitiativeName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I25" color="secondary elevation-0" class="mr-2" @click="archiveInitiative(),ArchiveDailog=false">Archive</v-btn>
                                                                    <v-btn id="AutoTest_I26" outlined color="grey" @click="ArchiveDailog=false,InitiativeName =null">Cancel</v-btn>
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
                                            <v-btn id="AutoTest_I27" text @click="RemoveInitiativeDailog=false,InitiativeName =null">
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
                                                                                      item-key="ProjectId"
                                                                                      fixed-header
                                                                                      height="25vh"
                                                                                      class="elevation-0">


                                                                        </v-data-table>
                                                                    </v-col>

                                                                </div>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I28" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(2),RemoveInitiativeDailog=false">Remove</v-btn>
                                                                    <v-btn id="AutoTest_I29" outlined color="grey" @click="RemoveInitiativeDailog=false,InitiativeName =null">Cancel</v-btn>
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
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
            <v-dialog v-model="IsActiveToggleDialog" width="800">
                <v-card class="elevation-0">
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-electric-switch</v-icon>
                        </v-avatar>
                        Change State
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_I30" text @click="IsActiveToggleDialog=false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="py-0">
                            <v-row>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">Are you sure want to change the state of this initiative ?</h4>
                                            </v-col>
                                            <div style="width:100%">
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
                                                                        {{InitiativeName}}
                                                                    </span>
                                                                    <span class="message">
                                                                        have  {{0}}  projects in it and by changing the state,all the active projects and tasks will became inactive and vice versa.
                                                                    </span>
                                                                </p>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list>
                                                </v-col>

                                                <v-col cols="12" xs="12" sm="12" md="12">

                                                    <v-data-table :headers="InitiativeAssignedprojectTableHeader"
                                                                  :items="InitiativeAssignedprojectTableItems"
                                                                  item-key="ProjectId"
                                                                  fixed-header
                                                                  height="25vh"
                                                                  class="elevation-0">

                                                        <template v-slot:body="{ items }">
                                                            <tbody>
                                                                <tr v-for="item in items" :key="item.ProjectId"
                                                                    v-bind:style="{backgroundColor: ProjectRowColor[item.Status]}">
                                                                    <td>{{ item.ProjectName }}</td>
                                                                    <td>{{ item.Status }}</td>
                                                                    <td>{{ item.Tasks }}</td>
                                                                </tr>
                                                            </tbody>
                                                        </template>
                                                    </v-data-table>
                                                </v-col>


                                            </div>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_I31" color="secondary elevation-0" class="mr-2" @click="ChangeStatus()">Change</v-btn>
                                                <v-btn id="AutoTest_I32" outlined color="grey" @click="IsActiveToggleDialog=false, fnGetInitiativeList()">Cancel</v-btn>
                                            </v-col>

                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                </v-card>
            </v-dialog>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>
<script type="text/javascript">

    var objInitiative;
    async function importscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
       
        ]);
    }
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeTableheaders: [
                    { text: "Name", align: "left", value: "InitiativeName", sortable: false },
                    { text: "Code", value: "Code", sortable: false},
                    { text: "Projects", value: "0", sortable: false},
                    { text: "Beneficiaries", value: "0", sortable: false},
                    { text: "State", value: "State", sortable: false} ,
                    { text: "Actions", value: "Actions", sortable: false }
                ],
                InitiativeTableItems: [],
                selectedInitiativeID: [],
                searchInitiative: "",
                selectedInitiativeName:"",
                RemoveInitiativeDailog: false,
                RemoveBulkInitiativeDailog :false,
                ArchiveDailog: false,
                IsActiveToggleDialog: false,
                InitiativeStateToggleInput: { InitiativeId: 0, InitiativeName: "", IsActive: 0 },
                InitiativeAssignedprojectTableHeader: [
                    { text: "Projects", align: "left", value: "ProjectName" },
                    { text: "State", value: "Status" },
                    { text: "Tasks", value: "Tasks", sortable: "false" }
                ],
                InitiativeAssignedprojectTableItems: [],
                InitiativeID: "",
                InitiativeName:"",
                UserStatus: "",
                ProjectRowColor: { Ongoing: "#fff0c7", Pending: "#f2c64d", Completed: "", Active: "#FFF0C7" },
                NotifyMessage: "",
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
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName + "," + this.selectedInitiativeID[1].InitiativeName ;
                } else {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName ;
                }
            }
        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await this.fnGetInitiativeList();
        },
        methods: {
            async fnGetInitiativeList() {
       
            var data = new FormData();
            data.append("stage", 1);
            data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiative.GetInitiativeDetails(data);
            
                if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else if (result.status == "success" && result.data) {
                this.InitiativeTableItems = result.data;
            }
            },
            fnBulkRemoveClick() {
                if (this.selectedInitiativeID.length > 0) {
                    this.RemoveBulkInitiativeDailog = true;
                } else {
                    this.Msg =("Please select the initiatives");
                    this.snackbarMsg = true;
                }
            },
        //Type 1 is bulk and Type 2 single removal
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
            else if (result.status = "success") {
                this.Msg = ("Successfully deleted");
                this.snackbarMsg = true;
            } else {

                this.Msg = ("Failed to delete");
                this.snackbarMsg = true;
            }
            this.fnGetInitiativeList();
            this.InitiativeID = 0;
            this.RemoveInitiativeDailog = false;
            this.RemoveBulkInitiativeDailog = false;
        },
          //Type 2 single archive
        //InitiativeStatus 0=draft,1=deploy ,2=archive
        async archiveInitiative() {
            
            var data = new FormData();
            data.append("InitiativeId", this.InitiativeID);
            data.append("Type", 2)
            data.append("InitiativeStatus", 2)

            var result = await objInitiative.ChangeInitiativeStatus(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
             else if (result.status = "Success") {
                this.Msg = ("Successfully archived");
                this.snackbarMsg = true;
            } else {
                this.Msg = ("Failed to archived");
                this.snackbarMsg = true;
            }
            this.fnGetInitiativeList();
            this.InitiativeID = 0;
            this.ArchiveDailog = false;
        },

            async ChangeStatus() {
            var data = new FormData();
            data.append("InitiativeId", this.InitiativeID);
            data.append("userStatus", this.UserStatus);
                var result = await objInitiative.ChangeStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
            else if (result.status = "success") {
                    this.Msg = ("Successfully Change State");
                    this.snackbarMsg = true;
            } else {
                    this.Msg = ("Failed to Change State");
                    this.snackbarMsg = true;
            }
            this.fnGetInitiativeList();
            this.InitiativeID = 0;
            this.IsActiveToggleDialog = false;
        },
        async GetAllUserProjects(InitiativeID, InitiativeName, userStatus) {
           
            this.InitiativeID = InitiativeID;
            this.InitiativeName = InitiativeName;
            this.UserStatus = userStatus;
            
            var result = await objInitiative.GetProjectsDetails();
            if (result.data) {
                this.InitiativeAssignedprojectTableItems = result.data;
            }
        },
        async GetInitiative(InitiativeID) {

            this.InitiativeID = InitiativeID;
        },       
},


};
</script>