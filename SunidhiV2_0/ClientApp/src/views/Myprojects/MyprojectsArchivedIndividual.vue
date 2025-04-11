<template>
  <v-card>
    <v-card-title>
      <h3 class="text-left page-head">
        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
          <v-icon size="15" color="primary">mdi mdi-file-document-box-outline</v-icon>
        </v-avatar>
        <router-link :to="{ name: 'MyprojectsArchived', query: {WsID: WorkspaceID,Access:RWAccess} }" class="page-head"> Archived Project</router-link> 
      </h3>
    </v-card-title>
    <v-card-text>
      <v-row>
          <v-col cols="12" md="12" sm="12" xs="12" align="center">
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Details
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualTeam', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Team
              </v-btn>
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualFinanceDetails', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
       v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualFinanceDetailsBudget' || $route.name == 'MyprojectsArchivedIndividualFinancePaymentTerms'}"
       class="mb-1 elevation-0"
       active-class="active-white-btn">
    Inputs
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualActivities', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualActivitiesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Activities & Inputs
              </v-btn>
              <v-btn id="AutoTest_IP170" tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualKPIOutcomes', query: { WsID: WorkspaceID, Access:RWAccess, InitID:InitiativeID,ProjID:ProjectId} }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualKPIOutcomes' || $route.name == 'MyProjectArchivedIndividualOutcomeActivityLog' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Indicators
              </v-btn>
              <v-btn tile="" active-class="active-white-btn" link="" :to="{ name: 'MyprojectsArchivedIndividualSurveys', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualSurveysIndividual' }"
                     class="mb-1 elevation-0">
                  Surveys
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualBeneficiaries', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualBeneficiariesTypeIndividual' || $route.name == 'MyprojectsArchivedIndividualBeneficiariesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Beneficiaries
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualWorkflow', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectWorkflow' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Workflow
              </v-btn>


              <!--<v-btn  tile link :to="{ name: 'MyprojectsArchivedIndividualKPI', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualKPIActivityLog'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        KPI
    </v-btn>-->
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualRisks', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsArchivedIndividualRiskActivityLog' }"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
      Risks
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsArchivedIndividualDocuments', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
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