<template>
    <v-container fluid>
        <v-row no-gutters>
            <v-col>
                <v-card class="pa-4">
                    <v-row>
                        <v-col v-if="AccessType=='Initiative'">
                            <h3 class="text-left page-head">
                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                    <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                                </v-avatar>
                                <router-link :to="{ name: 'DeployedInitiatives', query: {WsID: WorkspaceID,Access:RWAccess} }" class="page-head text-link">
                                    Deployed Initiative
                                </router-link>
                                <v-icon size="15">mdi-chevron-right</v-icon>
                                <router-link :to="{ name: 'Projects', query: {WsID: WorkspaceID,Access:RWAccess, InitID:initiativeInfo.InitiativeId} }" class="page-head text-link">
                                    {{ initiativeInfo.InitiativeName }}
                                </router-link>
                                <v-icon size="15">mdi-chevron-right</v-icon> {{ projectInfo.ProjectName }}
                            </h3>
                        </v-col>
                        <v-col v-if="AccessType=='MyProject'">
                            <h3 class="text-left page-head">
                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                    <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                                </v-avatar>

                                <router-link :to="{ name: 'MyprojectsDraft' ,query: { WsID: WorkspaceID,Access:RWAccess} }" class="page-head text-link">
                                    {{ projectInfo.ProjectName }}
                                </router-link>

                            </h3>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="12" sm="12" xs="12" align="center">
                            <v-btn id="AutoTest_IP163" tile="" link="" :to="{ name: 'ManageProject', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectDetails' || $route.name == 'ManageProject' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Details
                            </v-btn>
                            <v-btn id="AutoTest_IP164" tile="" link="" :to="{ name: 'ManageProjectTeam', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectTeam' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Team
                            </v-btn>
                            <v-btn id="AutoTest_IP165" tile="" link="" :to="{ name: 'ManageProjectFinance', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectFinance' || $route.name == 'ManageProjectFinanceBudget' || $route.name == 'ManageProjectFinancePaymentTerms' || $route.name == 'ManageProjectFinanceAccounts' || $route.name == 'ManageProjectFinanceLedgerBook' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Finance Details
                            </v-btn>
                            <v-btn id="AutoTest_IP166" tile="" link="" :to="{ name: 'ManageProjectWorkflow', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectWorkflow' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Workflow
                            </v-btn>
                            <v-btn name="btnSurvey" id="AutoTest_IP167" tile active-class="active-white-btn" link :to="{ name: 'ManageProjectSurveys', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectSurveysIndividual' }"
                                   class="mb-1 elevation-0">
                                Surveys
                            </v-btn>
                            <v-btn id="AutoTest_IP168" tile="" link="" :to="{ name: 'ManageProjectBeneficiaries', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectBeneficiaries' || $route.name == 'ManageProjectBeneficiariesTypeIndividual' || $route.name == 'ManageProjectBeneficiariesIndividual' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Beneficiaries
                            </v-btn>
                            <v-btn id="AutoTest_IP169" tile="" link="" :to="{ name: 'ManageProjectActivities', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectActivities' || $route.name == 'ManageProjectActivitiesIndividual' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Outputs
                            </v-btn>
                            <!--<v-btn id="AutoTest_IP170" tile="" link="" :to="{ name: 'ManageProjectKPI', query: { WsID: WorkspaceID,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                               v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectKPI' || $route.name == 'ManageProjectKPIActivityLog' }"
                               class="mb-1 elevation-0"
                               active-class="active-white-btn">
                            KPI
                        </v-btn>-->
                            <v-btn id="AutoTest_IP170" tile="" link="" :to="{ name: 'ManageProjectOutcomes', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectOutcomes' || $route.name == 'ManageProjectOutcomeActivityLog' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Outcomes
                            </v-btn>
                            <v-btn id="AutoTest_IP171" tile="" link="" :to="{ name: 'ManageProjectRisks', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectRisks' || $route.name == 'ManageProjectRiskActivityLog' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Risks
                            </v-btn>
                            <v-btn id="AutoTest_IP172" tile="" link="" :to="{ name: 'ManageProjectDocuments', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectDocuments' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn">
                                Documents
                            </v-btn>
                            <!--<v-btn tile="" link="" :to="{ name: 'ManageProjectDiscussion', query: { WsID: WorkspaceID,InitID:initiativeInfo.InitiativeId,ProjID:projectInfo.ProjectId} }"
                             v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectDiscussion' }"
                             class="mb-1 elevation-0"
                             active-class="active-white-btn">
                          Discussion
                        </v-btn>-->
                        </v-col>
                        <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                            <v-card>
                                <router-view></router-view>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-card>
            </v-col>
        </v-row>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </v-container>
</template>

<script>
    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                AccessType:'Initiative',
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: ""
                },
                RWAccess: 0,
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: ""
                },
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;  
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.AccessType = this.$route.query.AccessType;
            await importscript();
            await this.fnGetInitiativeDetailsByID();
            await this.fnGetProjectDetailsByID();
        },
        methods: {
            async fnGetInitiativeDetailsByID() {
                var data = new FormData();
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetInitiativeDetailsByID(data);
                if (result.data) {
                    this.initiativeInfo = result.data;
                } else {
                   // this.Msg = ("Failed to get details of Initiative " + this.initiativeInfo.InitiativeId);
                    //this.snackbarMsg = true;
                }
            },
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.GetProjectDetailsByID(data);
                if (result.data) {
                    this.projectInfo = result.data;
                } else {
                   // this.Msg = ("Failed to get details of Project " + this.projectInfo.InitiativeId);
                   // this.snackbarMsg = true;
                }
            },
        }
    };
</script>