<template>
  <v-card>
    <v-card-title>
      <h3 class="text-left page-head">
        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
          <v-icon size="15" color="primary">mdi mdi-file-document-box-outline</v-icon>
        </v-avatar>
        <router-link :to="{ name: 'MyprojectsDeployed', query: {WsID: WorkspaceID,Access:RWAccess} }" class="page-head"> Deployed Project</router-link> > {{ projectInfo.ProjectName }}
      </h3>
    </v-card-title>
    <v-card-text>
      <v-row>
          <v-col cols="12" md="12" sm="12" xs="12" align="center">
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividual', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Details
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualTeam', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Team
              </v-btn>
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualFinanceDetails', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualFinanceDetailsBudget' || $route.name == 'MyprojectsDeployedIndividualFinancePaymentTerms'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        Inputs
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualActivities', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualActivitiesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Activities & Inputs
              </v-btn>
              <v-btn id="AutoTest_IP170" tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualKPIOutcomes', query: { WsID: WorkspaceID, Access:RWAccess, InitID:InitiativeID,ProjID:ProjectId} }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualKPIOutcomes' || $route.name == 'MyProjectDeployedIndividualOutcomeActivityLog' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Indicators
              </v-btn>
              <v-btn tile="" active-class="active-white-btn" link="" :to="{ name: 'MyprojectsDeployedIndividualSurveys', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualSurveysIndividual' }"
                     class="mb-1 elevation-0">
                  Surveys
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualBeneficiaries', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualBeneficiariesTypeIndividual' || $route.name == 'MyprojectsDeployedIndividualBeneficiariesIndividual' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Beneficiaries
              </v-btn>
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualWorkflow', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
                     v-bind:class="{ 'active-white-btn': $route.name == 'ManageProjectWorkflow' }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Workflow
              </v-btn>


              <!--<v-btn  tile link :to="{ name: 'MyprojectsDeployedIndividualKPI', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualKPIActivityLog'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        KPI
    </v-btn>-->
              <!--<v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualRisks', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDeployedIndividualRiskActivityLog' }"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
      Risks
    </v-btn>-->
              <v-btn tile="" link="" :to="{ name: 'MyprojectsDeployedIndividualDocuments', query: { WsID: WorkspaceID, Access:RWAccess, InitID: InitiativeID, ProjID: ProjectId } }"
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
        <v-col cols="12" xs="12" sm="12" md="12" class="py-0 px-2" style="min-height:60vh;">
          <v-card flat>
            <router-view></router-view>
          </v-card>
        </v-col>
      </v-row>
    </v-card-text>
</v-card>
</template>

<script type="text/javascript">
 
  var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
  
  export default {
    data(){
      return{
        WorkspaceID: 0,
        InitiativeID: 0,
        ProjectId: "",         
        RWAccess: 0,
        projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    StartDate: "",
                    EndDate: "",
                    CountryID: 0,
                    StateID: 0,
                    DistrictID: 0,
                    ProjectTags: "",
                    ProjectTagsList: [],
                },
      }
    },
    components:{
    
    },
   mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
    this.InitiativeID = this.$route.query.InitID;
    this.ProjectId = this.$route.query.ProjID;
    await importscript();
    await this.fnGetProjectDetailsByID();
   },
   
   methods: {
     /// <summary>
     ///  fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId
     /// </summary>
     /// <param name=""></param>
     /// <returns></returns>
     async fnGetProjectDetailsByID() {
        var data = new FormData();
        data.append("ProjectId", this.ProjectId);
        data.append("InitiativeId", this.InitiativeID);
        var result = await objManageProject.GetProjectDetailsByID(data);
        if (result.status == "Unhandled Exception") {
           this.$router.push({ name: "error" });
        } else if (result.data) {
           this.projectInfo = result.data;
           this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
           if(this.SelectedTag == null){
              this.SelectedTag =[];
           }
            //alert("Tags length =" + this.projectInfo.ProjectTagsList.length);
        } else {
            this.Msg = "Failed to get details of Project " + this.projectInfo.ProjectId;
            this.errorSnackbarMsg = true;
        }
     },
   }
  }
</script>