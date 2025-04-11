<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12" md="12" sm="12" xs="12" class="px-4" v-if="$route.name == 'MyprojectsDraft'
             || $route.name == 'MyprojectsDeployed'
             || $route.name == 'MyprojectsCompleted'
             || $route.name == 'MyprojectsArchived'">
        <v-btn id="AutoTest_MP1" height="39" rounded link :to="{ name: 'MyprojectsDraft' ,query: { WsID: WorkspaceID,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyprojectsDraft' || $route.name == 'MyprojectsDraftIndividual'
                       || $route.name == 'MyprojectsDraftIndividualTeam' || $route.name == 'MyprojectsDraftIndividualFinanceDetails' || $route.name == 'MyprojectsDraftIndividualFinanceDetailsBudget'
                       || $route.name == 'MyprojectsDraftIndividualWorkflow' || $route.name == 'MyprojectsDraftIndividualSurveys'
                       || $route.name == 'MyprojectsDraftIndividualBeneficiaries' || $route.name == 'MyprojectsDraftIndividualActivities'
                       || $route.name == 'MyprojectsDraftIndividualKPI' || $route.name == 'MyprojectsDraftIndividualRisks'
                       || $route.name == 'MyprojectsDraftIndividualDocuments' || $route.name == 'MyprojectsDraftIndividualFinancePaymentTerms'
                       || $route.name == 'MyprojectsDraftIndividualSurveysIndividual' || $route.name == 'MyprojectsDraftIndividualBeneficiariesTypeIndividual'
                       || $route.name == 'MyprojectsDraftIndividualBeneficiariesIndividual' || $route.name == 'MyprojectsDraftIndividualActivitiesIndividual'
                       || $route.name == 'MyprojectsDraftIndividualKPIActivityLog' || $route.name == 'MyprojectsDraftIndividualRiskActivityLog'
                       || $route.name == 'MyprojectsDraftIndividualKPIOutcomes'
                       || $route.name == 'MyProjectDraftIndividualOutcomeActivityLog'}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-outline</v-icon>
                    </v-avatar>Draft
                </v-btn>
      
      <v-btn id="AutoTest_MP2" height="39" rounded link :to="{ name: 'MyprojectsDeployed' ,query: { WsID: WorkspaceID ,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyprojectsDeployed' || $route.name == 'MyprojectsDeployedIndividual'
                       || $route.name == 'MyprojectsDeployedIndividualTeam'
                       || $route.name == 'MyprojectsDeployedIndividualFinanceDetails'
                       || $route.name == 'MyprojectsDeployedIndividualFinanceDetailsBudget'
                       || $route.name == 'MyprojectsDeployedIndividualFinancePaymentTerms'
                       || $route.name == 'MyprojectsDeployedIndividualWorkflow'
                       || $route.name == 'MyprojectsDeployedIndividualSurveys'
                       || $route.name == 'MyprojectsDeployedIndividualSurveysIndividual'
                       || $route.name == 'MyprojectsDeployedIndividualBeneficiaries'
                       || $route.name == 'MyprojectsDeployedIndividualBeneficiariesTypeIndividual'
                       || $route.name == 'MyprojectsDeployedIndividualBeneficiariesIndividual'
                       || $route.name == 'MyprojectsDeployedIndividualActivities'
                       || $route.name == 'MyprojectsDeployedIndividualActivitiesIndividual'
                       || $route.name == 'MyprojectsDeployedIndividualKPI'
                       || $route.name == 'MyprojectsDeployedIndividualKPIOutcomes'
                       || $route.name == 'MyProjectDeployedIndividualOutcomeActivityLog'
                       || $route.name == 'MyprojectsDeployedIndividualKPIActivityLog'
                       || $route.name == 'MyprojectsDeployedIndividualRisks'
                       || $route.name == 'MyprojectsDeployedIndividualRiskActivityLog'
                       || $route.name == 'MyprojectsDeployedIndividualDocuments'}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-plus-outline</v-icon>
                    </v-avatar>Deployed
                </v-btn>
      <v-btn id="AutoTest_MP3" height="39" rounded link :to="{ name: 'MyprojectsCompleted' ,query: { WsID: WorkspaceID,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyprojectsCompleted'
                       || $route.name == 'MyprojectsCompletedIndividual'
                       || $route.name == 'MyprojectsCompletedIndividualTeam'
                       || $route.name == 'MyprojectsCompletedIndividualFinanceDetails'
                       || $route.name == 'MyprojectsCompletedIndividualFinanceDetailsBudget'
                       || $route.name == 'MyprojectsCompletedIndividualFinancePaymentTerms'
                       || $route.name == 'MyprojectsCompletedIndividualWorkflow'
                       || $route.name == 'MyprojectsCompletedIndividualSurveys'
                       || $route.name == 'MyprojectsCompletedIndividualSurveysIndividual'
                       || $route.name == 'MyprojectsCompletedIndividualBeneficiaries'
                       || $route.name == 'MyprojectsCompletedIndividualBeneficiariesTypeIndividual'
                       || $route.name == 'MyprojectsCompletedIndividualBeneficiariesIndividual'
                       || $route.name == 'MyprojectsCompletedIndividualActivities'
                       || $route.name == 'MyprojectsCompletedIndividualActivitiesIndividual'
                       || $route.name == 'MyprojectsCompletedIndividualKPI'
                       || $route.name == 'MyprojectsCompletedIndividualKPIOutcomes'
                       || $route.name == 'MyProjectCompletedIndividualOutcomeActivityLog'
                       || $route.name == 'MyprojectsCompletedIndividualKPIActivityLog'
                       || $route.name == 'MyprojectsCompletedIndividualRisks'
                       || $route.name == 'MyprojectsCompletedIndividualRiskActivityLog'
                       || $route.name == 'MyprojectsCompletedIndividualDocuments'
                        }"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-check</v-icon>
                    </v-avatar>Completed
                </v-btn>
      <v-btn id="AutoTest_MP4" height="39" rounded link :to="{ name: 'MyprojectsArchived' ,query: { WsID: WorkspaceID,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyprojectsArchived'
                       || $route.name == 'MyprojectsArchivedIndividual'
                       || $route.name == 'MyprojectsArchivedIndividualTeam'
                       || $route.name == 'MyprojectsArchivedIndividualFinanceDetails'
                       || $route.name == 'MyprojectsArchivedIndividualFinanceDetailsBudget'
                       || $route.name == 'MyprojectsArchivedIndividualFinancePaymentTerms'
                       || $route.name == 'MyprojectsArchivedIndividualWorkflow'
                       || $route.name == 'MyprojectsArchivedIndividualSurveys'
                       || $route.name == 'MyprojectsArchivedIndividualSurveysIndividual'
                       || $route.name == 'MyprojectsArchivedIndividualBeneficiaries'
                       || $route.name == 'MyprojectsArchivedIndividualBeneficiariesTypeIndividual'
                       || $route.name == 'MyprojectsArchivedIndividualBeneficiariesIndividual'
                       || $route.name == 'MyprojectsArchivedIndividualActivities'
                       || $route.name == 'MyprojectsArchivedIndividualActivitiesIndividual'
                       || $route.name == 'MyprojectsArchivedIndividualKPI'
                       || $route.name == 'MyprojectsArchivedIndividualKPIOutcomes'
                       || $route.name == 'MyProjectArchivedIndividualOutcomeActivityLog'
                       || $route.name == 'MyprojectsArchivedIndividualKPIActivityLog'
                       || $route.name == 'MyprojectsArchivedIndividualRisks'
                       || $route.name == 'MyprojectsArchivedIndividualRiskActivityLog'
                       || $route.name == 'MyprojectsArchivedIndividualDocuments'
                       }"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-minus-outline</v-icon>
                    </v-avatar>Archived
                </v-btn>
      </v-col>

      <v-col cols="12" xs="12" sm="12" md="12" class="px-4">
        <router-view></router-view>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
  export default{
  data(){
  return{
      WorkspaceID: 0,
      RWAccess: 0,
  }
  },
  mounted: async function () {
      this.WorkspaceID = this.$route.query.WsID;
      this.RWAccess = this.$route.query.Access; 
  },
  }
</script>