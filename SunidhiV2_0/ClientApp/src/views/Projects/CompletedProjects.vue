<template>
    <v-card>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid class="px-0">
            <v-row class="px-4">
                <v-col >
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary">mdi-account-group-outline</v-icon>
                        </v-avatar>
                        Completed Project
                    </h3>
                </v-col>
              
                    <v-col md="auto">
                      <v-row>
                        <!--Bulk Action -->
                        <v-menu offset-y="" transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                          <template v-slot:activator="{ on }">
                            <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                              Bulk Actions
                              <v-icon>
                                mdi-chevron-down
                              </v-icon>
                            </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">

                            <v-list-item id="AutoTest_IP33" @click.stop="fnBulkRemoveClick()">
                              <v-list-item-icon>
                                <v-icon>mdi-trash-can-outline</v-icon>
                              </v-list-item-icon>
                              <v-list-item-content>
                                Remove
                              </v-list-item-content>
                            </v-list-item>
                          </v-list>

                          <v-dialog v-model="RemoveBulkProjectDailog" width="800">
                            <v-card>
                              <v-card-title primary-title="" class="page-head py-4">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                  <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                </v-avatar>
                                Remove Project
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_IP34" text="" @click="RemoveBulkProjectDailog=false">
                                  <v-icon color="#707070" size="15">mdi-close</v-icon>
                                </v-btn>
                              </v-card-title>
                              <v-card-text class="pt-0">
                                <v-container fluid="" class="pt-4">
                                  <v-row no-gutters="">
                                    <v-col cols="12" md="12" class="pt-0">
                                      <v-container fluid="" class="pt-0">
                                        <v-row>
                                          <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this projects ?</h4>
                                          </v-col>

                                          <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                           <Secondary-Button id="AutoTest_IP35" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(1)"></Secondary-Button>
                                          <Default-Button-Outlined id="AutoTest_IP36" class="btn-122x36" title="Cancel" @click.native="RemoveBulkProjectDailog=false"></Default-Button-Outlined>
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
                                      dense prepend-inner-icon="mdi-magnify" v-model="searchProject"
                                      outlined hide-details></v-text-field>
                      </v-row>
                    </v-col>
            </v-row>
            <!--Main Body-->
            <v-row v-if="ProjectTableItems.length>0">
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="ProjectTableheaders"
                                  :items="ProjectTableItems"
                                  v-model="selectedProjectID"
                                  show-select=""
                                  item-key="ProjectId"
                                  :search="searchProject"
                                  class="elevation-0">

                        <!-- column State-->
                        <template v-slot:item.State="{ item }">
                            <v-switch color="primary" v-model="item.IsActive" :label="item.IsActive ? 'ACTIVE': 'INACTIVE'" v-bind:disabled ="!screenAccess"
                                      @change="GetProjectID(item.ProjectId,item.ProjectName,item.IsActive),IsActiveToggleDialog = true,ProjectName=item.ProjectName"></v-switch>
                        </template>
                        <!-- DateTime-->
                        <template v-slot:item.StartDate="{ item }">
                            <span>{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</span>
                        </template>
                        <template v-slot:item.EndDate="{ item }">
                            <span>{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</span>
                        </template>
                        <!-- Draft project Actions-->
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

                                    <v-list-item id="AutoTest_IP37" link="" @click.stop="GetProjectID(item.ProjectId,item.ProjectName),RemoveProjectDailog=true,ProjectName=item.ProjectName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>

                                <v-dialog v-model="RemoveProjectDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove Project
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP38" text="" @click="RemoveProjectDailog=false">
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
                                                                    <h4 class="text-center heading-3">Are you sure you want to remove this project ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{ProjectName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                  <Secondary-Button id="AutoTest_IP39" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteProject(2)"></Secondary-Button>
                                                                  <Default-Button-Outlined id="AutoTest_IP40" class="btn-122x36" title="Cancel" @click.native="RemoveProjectDailog=false"></Default-Button-Outlined>
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
            <!-- else part if No project available-->
            <v-row v-else>
                <v-col cols="12" sm="12" md="12">
                    <v-row :justify="justifyCenter">
                        <v-col md="12" class="text-center">
                            <v-avatar size="80" tile class="elevation-1 base-border-radius mr-2">
                                <v-img src="../../images/ic_no_workspace.svg" height="80" width="80" class="ma-4"></v-img>
                            </v-avatar>
                            <p class="heading-3 pt-3">No projects available</p>
                            <v-btn id="AutoTest_IP41" height="32" color="success-btn" dark class="elevation-0 mr-2" @click="" v-bind:disabled ="!screenAccess">
                                Create project
                            </v-btn>
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
                        <v-btn id="AutoTest_IP42" text @click="IsActiveToggleDialog=false">
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
                                                <h4 class="text-center heading-3">Are you sure want to change the state of this project ?</h4>
                                            </v-col>
                                            <v-col>
                                                <h4 class="text-center heading-3">
                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                    {{ProjectName}}
                                                </h4>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <v-btn id="AutoTest_IP43" color="secondary elevation-0" class="mr-2" @click="ChangeState()">Change</v-btn>
                                                <v-btn id="AutoTest_IP44" outlined color="grey" @click="IsActiveToggleDialog=false, GetProjectListByInitID()">Cancel</v-btn>
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
    import PreLoader from '@/components/pre-loader.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/Project.js").then(mod => {
                objProject = new mod.Project();
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
        data() {
            return {
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeId: 0,
                PreLoader: false,

                ProjectTableheaders: [
                    { text: "Name", align: "left", value: "ProjectName" },
                    { text: "Code", value: "Code" },
                    { text: "Start Date", value: "StartDate" },
                    { text: "End Date", value: "EndDate", },
                    { text: "Budget", value: "Budget", },
                    { text: "Progress", value: "0" },
                    { text: "Fund Utilization", value: "0", },
                    { text: "State", value: "State", sortable: false },
                    { text: "Actions", value: "Actions", sortable: false }

                ],
                ProjectTableItems: [],
                selectedProjectID: [],
                searchProject: "",
                ProjectId: "",
                ProjectName: "",
                State: "",
                RemoveProjectDailog: false,
                RemoveBulkProjectDailog: false,
                ArchiveBulkDailog: false,
                ArchiveDailog: false,
                IsActiveToggleDialog: false,
                snackbarMsg: false,
                Msg: "",
                screenAccess: 0,
              
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;           
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await importUserscript();
            this.PreLoader = true;
            await this.GetProjectListByInitID();
            this.PreLoader = false;
        },
        components: {
            'Pre-Loader': PreLoader,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,

        },
        methods: {
            async GetProjectListByInitID() {
                var data = new FormData();
                data.append("stage", 'Completed');
                data.append("InitiativeId", this.InitiativeId);
                var result = await objProject.GetProjectListByInitID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else 
                if (result.data) {
                    this.ProjectTableItems = result.data;
                }
            },
            fnBulkClick() {
                if (this.selectedProjectID.length > 0) {
                    this.ArchiveBulkDailog = true;
                } else {
                    this.Msg = "Please select the projects";
                    this.snackbarMsg = true;
                }
            },
            fnBulkRemoveClick() {
                if (this.selectedProjectID.length > 0) {
                    this.RemoveBulkProjectDailog = true;
                } else {
                    this.Msg = "Please select the projects";
                    this.snackbarMsg = true;
                }
            },
            async GetProjectID(ProjectID, ProjectName, State) {
                this.ProjectID = ProjectID;
                this.ProjectName = ProjectName;
                this.State = State;
            },
            //Type 1 is bulk and Type 2 single removal
            async DeleteProject(Type) {

                var data = new FormData();
                if (Type == 1) {
                    data.append("ProjectID", JSON.stringify(this.selectedProjectID));
                    data.append("Type", 1);
                }
                else {
                    data.append("ProjectID", JSON.stringify(this.ProjectID));
                    data.append("Type", 2);
                }

                var result = await objProject.DeleteProject(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg= "Successfully deleted";
                    } else {

                        this.Msg = "Failed to delete";
                    }
                    this.snackbarMsg = true;
                }
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.RemoveProjectDailog = false;
                this.RemoveBulkProjectDailog = false;

            },
            async ChangeState() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("State", this.State);
                var result = await objProject.ChangeState(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else if (result.status = "success") {
                    this.Msg ="Successfully Change State";
                } else {
                    this.Msg = "Failed to Change State";
                }
                this.snackbarMsg = true;
                this.GetProjectListByInitID();
                this.ProjectId = 0;
                this.IsActiveToggleDialog = false;
            },
        },
    };
</script>

