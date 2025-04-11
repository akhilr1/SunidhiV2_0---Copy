<template>
  <v-card>
    <v-card-title>
      <h3 class="text-left page-head">
        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
          <v-icon size="15" color="primary">mdi mdi-file-document-box-outline</v-icon>
        </v-avatar>
        <router-link :to="{ name: 'MyprojectsCompleted', query: {WsID: WorkspaceID,Access:RWAccess} }" class="page-head"> Completed Project</router-link> 
      </h3>
    </v-card-title>
    <v-card-text>
      <v-row>
          <v-col cols="12" md="12" sm="12" xs="12" align="center">
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Details
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualTeam', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Team
              </v-btn>
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualFinanceDetails', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualFinanceDetailsBudget' || $route.name == 'MyprojectsCompletedIndividualFinancePaymentTerms'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        Inputs
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualActivities', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualActivitiesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Activities & Inputs
              </v-btn>
              <v-btn id="AutoTest_IP170" tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualKPIOutcomes', query: { WsID: WorkspaceID, Access:RWAccess, InitID:InitiativeID,ProjID:ProjectId} }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualKPIOutcomes' || $route.name == 'MyProjectCompletedIndividualOutcomeActivityLog' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Indicators
              </v-btn>
              <v-btn tile="" active-class="active-white-btn" link="" :to="{ name: 'MyprojectsCompletedIndividualSurveys', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualSurveysIndividual' }"
                     class="mb-1 elevation-0">
                  Surveys
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualBeneficiaries', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualBeneficiariesTypeIndividual' || $route.name == 'MyprojectsCompletedIndividualBeneficiariesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Beneficiaries
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualWorkflow', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectWorkflow' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Workflow
              </v-btn>


              <!--<v-btn  tile link :to="{ name: 'MyprojectsCompletedIndividualKPI', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualKPIActivityLog'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        KPI
    </v-btn>-->
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualRisks', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsCompletedIndividualRiskActivityLog' }"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
      Risks
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsCompletedIndividualDocuments', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectDocuments' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Documents
              </v-btn>
              <!--<v-btn tile="" link="" :to="{ name: 'ManageProjectDiscussion', query: { WsID: WorkspaceID,Access:RWAccess,InitID:initiativeInfo.InitiativeID,ProjID:projectInfo.ProjectId} }"
           v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectDiscussion' }"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        Discussion
    </v-btn>-->
          </v-col>
        <v-col cols="12" xs="12" sm="12" md="12" class="py-0 px-2">
          <v-card>
            <router-view></router-view>
          </v-card>
        </v-col>
      </v-row>
    </v-card-text>
</v-card>
</template>

<script type="text/javascript">
 
  
  
  export default {
    data(){
      return{
        WorkspaceID: 0,
        InitiativeID: 0,
        ProjectId: "",
        RWAccess: 0,
      }
    },
    components:{
    
    },
   mounted: async function () {
   this.WorkspaceID = this.$route.query.WsID;
   this.RWAccess = this.$route.query.Access;
   this.InitiativeID = this.$route.query.InitID;
   this.ProjectId = this.$route.query.ProjID;
   },
  }
</script>