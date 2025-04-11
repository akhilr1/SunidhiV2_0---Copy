<template>
    <v-container fluid>
        <v-row no-gutters>
            <v-col>
                <v-card class="pa-4">
                    <v-row>
                        <v-col>
                            <h3 class="text-left page-head">
                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                    <v-icon size="15" color="primary">mdi-file-document-box-multiple-outline</v-icon>
                                </v-avatar>
                                <router-link :to="{ name: 'SearchVenture', query: {WsID: WorkspaceID , Access: RWAccess} }" class="page-head text-link" v-if="VentureType == 'Search Venture'">
                                    {{VentureType}}
                                </router-link>
                                <router-link :to="{ name: 'MyVenture', query: {WsID: WorkspaceID , Access: RWAccess} }" class="page-head text-link" v-if="VentureType == 'MyVenture'">
                                    MyVenture
                                </router-link>
                                <v-icon size="15">mdi-chevron-right</v-icon> {{ projectInfo.ProjectName }}
                            </h3>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col v-if="VentureType == 'Search Venture'" class="text-right">
                            <v-btn id="AutoTest_V1" height="38" color="success-btn" dark class="elevation-0 mr-4" @click="dlgCommitFund=true"  v-bind:disabled ="!screenAccess">Commit Fund</v-btn>
                            <v-btn id="AutoTest_V2" height="38" color="success-btn" dark class="elevation-0 mr-2" @click="dlgWhishlist=true"  v-bind:disabled ="!screenAccess">Whishlist</v-btn>
                        </v-col>
                    </v-row>
                    <!-- Commit fund -->
                    <v-dialog v-model="dlgCommitFund" width="800">
                        <v-card>
                            <v-card-title primary-title class="page-head py-4">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                    <v-icon size="20" color="primary">mdi-check-circle-outline</v-icon>
                                </v-avatar>
                                Commit Fund
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_V3" text="" @click="dlgCommitFund=false">
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
                                                        <h4 class="text-center heading-3">Enter the amount to commit found</h4>
                                                    </v-col>
                                                    <v-row :justify="justifyCenter">
                                                        <v-col md="6">
                                                            <v-text-field dense label="Enter Amount" class="mr-2"
                                                                          type="number"
                                                                          v-model="CommitedFund"
                                                                          outlined></v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                        <Secondary-Button id="AutoTest_V4" class="btn-122x36 mr-4" title="Confirm" @click.native="InsertCommitFund('Funded Project'),dlgCommitFund = false"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_V5" class="btn-122x36" title="Cancel" @click.native="dlgCommitFund=false"></Default-Button-Outlined>

                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-card-text>
                        </v-card>
                    </v-dialog>

                    <!-- Wishlist dailog-->
                    <v-dialog v-model="dlgWhishlist" width="800">
                        <v-card>
                            <v-card-title primary-title class="page-head py-4">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                    <v-icon size="20" color="primary">mdi-star-outline</v-icon>
                                </v-avatar>
                                Whishlist
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_V6" text="" @click="dlgWhishlist=false">
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
                                                        <h4 class="text-center heading-3">Are you sure you want to wishlist this Project ?</h4>
                                                    </v-col>
                                                    <v-col>
                                                        <h4 class="text-center heading-3">
                                                            <v-avatar color="#fff" class="elevation-1 mr-2" size="40">
                                                                <v-icon color="#707070" size="25">mdi-folder</v-icon>
                                                            </v-avatar>
                                                            {{projectInfo.ProjectName}}
                                                        </h4>
                                                    </v-col>

                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                        <Secondary-Button id="AutoTest_V7" class="btn-122x36 mr-4" title="Confirm" @click.native="AddToWishlist(2),dlgWhishlist = false"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_V8" class="btn-122x36" title="Cancel" @click.native="dlgWhishlist=false"></Default-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-card-text>
                        </v-card>
                    </v-dialog>
                    
                    <v-row>
                        <v-col cols="12" md="12" sm="12" xs="12" align="center">
                            <v-btn tile="" link="" :to="{ name: 'ManageVentureDetails', query: {WsID: WorkspaceID, Access: RWAccess, ProjID :projectInfo.ProjectId, InitID:initiativeInfo.InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageVentureDetails' || $route.name == 'ManageVenture' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Details
                            </v-btn>
                            <v-btn tile="" link="" :to="{ name: 'ManageVentureFinanceDetails', query: {WsID: WorkspaceID, Access: RWAccess, ProjID :projectInfo.ProjectId, InitID:initiativeInfo.InitiativeId,VentureType:VentureType} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageVentureFinanceDetails' || $route.name == 'ManageVenture'}"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Finance Details
                            </v-btn>
                            <v-btn tile="" link="" :to="{ name: 'ManageVentureActivities', query: {WsID: WorkspaceID, Access: RWAccess, ProjID :projectInfo.ProjectId, InitID:initiativeInfo.InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageVentureActivities' || $route.name == 'ManageVenture' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Activities
                            </v-btn>
                            <v-btn tile="" link="" :to="{ name: 'ManageVentureKPI', query: {WsID: WorkspaceID, Access: RWAccess, ProjID :projectInfo.ProjectId, InitID:initiativeInfo.InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageVentureKPI' || $route.name == 'ManageVentureKPIActivityLog' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Outcomes
                            </v-btn>
                            <v-btn tile="" link="" :to="{ name: 'ManageVentureDocuments',query: {WsID: WorkspaceID, Access: RWAccess, ProjID :projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageVentureDocuments' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Documents
                            </v-btn>
                        </v-col>
                        <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                            <v-card elevation="0" min-height="55vh">
                                <router-view></router-view>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-card>
            </v-col>
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
    </v-container>
</template>

<script>
  import SecondaryButton from '@/components/secondary-button.vue'
  import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objVenture;
    async function importVenturescript() {
        return Promise.all([
            import("../../BL/Venture.js").then(mod => {
                objVenture = new mod.Venture();
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
                screenAccess: 0,
                RWAccess: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                VentureType :"",
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: ""
                },
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: ""
                },
               
                dlgCommitFund: false,
                dlgWhishlist: false,
                CommitedFund: 0,
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.VentureType = this.$route.query.VentureType;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            await importscript();
            await importVenturescript();
            await this.fnGetProjectDetailsByID();

        },
        methods: {
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.GetProjectDetailsByID(data);
                if (result.data) {
                    this.projectInfo = result.data;
                } else {
                    this.Msg = ("Failed to get details of Project " + this.projectInfo.InitiativeId);
                    this.errorSnackbarMsg = true;
                }
            },
            async InsertCommitFund() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("ProjectType", 'Funded Project');
                data.append("Amount", this.CommitedFund);
                var result = await objVenture.InsertCommitFund(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status = "Success") {
                        this.Msg = "Fund commited successfully.";
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = ("commit fund adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },
            async AddToWishlist(Type) {
                var data = new FormData();
                data.append("ProjectId", JSON.stringify(this.projectInfo.ProjectId));
                data.append("ProjectType", 'Wishlist');
                data.append("Amount", this.CommitedFund);
                data.append("Type", 2);
                var result = await objVenture.AddToWishlist(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status = "Success") {
                        this.Msg = "Project whishlisted successfully.";
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = ("wishlist adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },
        }
    };
</script>