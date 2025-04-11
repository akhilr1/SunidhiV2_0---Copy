<template>
    <v-container fluid>
        <v-row>
            <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
                <v-btn id="AutoTest_I131"  height="39" rounded link :to="{ name: 'initiatives', query: {WsID: WorkspaceID,Access:RWAccess} }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'DraftInitiatives' || $route.query.InitiativeType == 1}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-edit-outline</v-icon>
                    </v-avatar>Draft
                </v-btn>
                <v-btn id="AutoTest_I132"  height="39" rounded link :to="{ name: 'DeployedInitiatives', query: {WsID: WorkspaceID,Access:RWAccess} }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'DeployedInitiatives' || $route.query.InitiativeType == 2 ||
                                                                $route.name == 'Projects' || $route.name == 'DraftProjects' || 
                                                                $route.name == 'DeployedProjects' || $route.name == 'CompletedProjects' || $route.name == 'ArchivedProjects' ||
                                                                $route.name == 'ManageProject' ||
                                                                $route.name == 'ManageProjectDetails' || $route.name == 'ManageProjectTeam' ||
                                                                $route.name == 'ManageProjectFinance' || $route.name == 'ManageProjectFinanceBudget' || $route.name == 'ManageProjectFinancePaymentTerms' || $route.name == 'ManageProjectFinanceAccounts' || $route.name == 'ManageProjectFinanceLedgerBook' || $route.name == 'ManageProjectWorkflow' ||
                                                                $route.name == 'ManageProjectSurveys' || $route.name == 'ManageProjectSurveysIndividual' || $route.name == 'ManageProjectBeneficiaries' || $route.name == 'ManageProjectBeneficiariesTypeIndividual' || $route.name == 'ManageProjectBeneficiariesIndividual' ||
                                                                $route.name == 'ManageProjectActivities' || $route.name == 'ManageProjectActivitiesIndividual' || $route.name == 'ManageProjectKPI' || $route.name == 'ManageProjectKPIActivityLog' ||
                                                                $route.name == 'ManageProjectRisks' || $route.name == 'ManageProjectRiskActivityLog' || $route.name == 'ManageProjectDocuments' ||
                                                                $route.name == 'ManageProjectDiscussion' }"
                       class="mb-1 mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-star-outline</v-icon>
                    </v-avatar>Deployed
                </v-btn>
                <v-btn id="AutoTest_I133" height="39" rounded link :to="{ name: 'ArchivedInitiatives', query: {WsID: WorkspaceID,Access:RWAccess} }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'ArchivedInitiatives'}"
                       class="mb-1 mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-remove-outline</v-icon>
                    </v-avatar>Archived
                </v-btn>
            </v-col>

            <v-col cols="12" xs="12" sm="12" md="12" class="px-4">
                <router-view></router-view>
            </v-col>

            <!--Data table-->
        </v-row>
    </v-container>
</template>

<script>
    export default {
        data() {
            return {
                WorkspaceID: 0,
                RWAccess: 0,
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;   
        },
  };
</script>