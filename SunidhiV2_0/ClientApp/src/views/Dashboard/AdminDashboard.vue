<template>
  <v-container fluid>
    <PreLoader v-show="preloader"></PreLoader>
    <v-row>
      <v-col>
        <h1 class="page-head-dark font-weight-medium">
          <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
            <v-icon size="15" color="primary">mdi-view-dashboard</v-icon>
          </v-avatar>
          Dashboard
        </h1>
      </v-col>
    </v-row>
    <v-row>
      <!-- Default -->
      <v-col cols="12" sm="12" xs="12" md="12">
        <DashboardHead :dashdata="dashBoardData"></DashboardHead> 
      </v-col>
    </v-row>
    <v-row v-if="isProjectAccess">
      <!-- if Project Enabled -->
      <v-col cols="12" sm="12" xs="12" md="4">
        <ProjectProgress :dashdata="dashBoardData" :updatedTime="ProjectTableItems.length && ProjectTableItems[0].ExecutedDateTime"></ProjectProgress>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="4">
        <ProjectTaskSummary :dashdata="dashBoardData"></ProjectTaskSummary>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="4">
        <ProjectFundUtilization></ProjectFundUtilization>
      </v-col>
    </v-row>
    <v-row v-if="isProjectAccess">
      <!-- if Project Enabled -->
      <v-col cols="12" md="4" sm="12" xs="12">
        <ProjectSummary :dashdata="dashBoardData"></ProjectSummary>
      </v-col>
      <v-col cols="12" md="8" sm="12" xs="12">
        <MyProjectWidget :ProjectTableItems="ProjectTableItems" @statusChange="updateProjectList"></MyProjectWidget>
      </v-col>
    </v-row>
    <v-row>
      <!-- Default -->
      <v-col cols="12" sm="12" xs="12" md="4">
        <TaskSummary></TaskSummary>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="8">
        <MyTaskWidget :dashdata="dashBoardData" :taskAccess="taskAccess"></MyTaskWidget>
      </v-col>
    </v-row>
    <v-row v-if="isFinanceAccess" >
      <!-- Set 3 -->
      <v-col cols="12" sm="4" md="4">
        <Notification></Notification>
      </v-col>
      <v-col cols="12" sm="8" md="8">
        <ProjectFundUtilisationBar></ProjectFundUtilisationBar>
      </v-col>
    </v-row>
    <v-row v-if="isFinanceAccess">
      <!-- Set 3 -->
      <v-col cols="12" sm="12" xs="12" md="4">
        <BudgetPlan></BudgetPlan>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="8">
        <ProjectInsights></ProjectInsights>
      </v-col>
    </v-row>
    <v-row v-if="isFinanceAccess">
      <!-- Set 3 -->
      <v-col cols="12" sm="12" xs="12" md="4">
        <BudgetAndFunds></BudgetAndFunds>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="8">
        <PlannedAndActual></PlannedAndActual>
      </v-col>
    </v-row>
    <v-row v-if="isFinanceAccess">
      <!-- Set 3 -->
      <v-col cols="12" sm="12" md="12">
        <MyAccounts></MyAccounts>
      </v-col>
    </v-row>
    <v-row>
      <!-- Set 3 -->
      <v-col cols="12" sm="12" xs="12" md="4">
        <HelpDocumentation></HelpDocumentation>
      </v-col>
      <v-col cols="12" sm="12" xs="12" md="8">
        <MyTeam></MyTeam>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import MyProjectWidget from "@/views/Dashboard/DashboardWidgets/MyProjectWidget.vue";
import Notification from "@/views/Dashboard/DashboardWidgets/NotificationWidget.vue";
import ProjectProgress from "@/views/Dashboard/DashboardWidgets/ProjectProgress.vue";
import ProjectTaskSummary from "@/views/Dashboard/DashboardWidgets/ProjectTaskSummary.vue";
import ProjectFundUtilization from "@/views/Dashboard/DashboardWidgets/ProjectFundUtilization.vue";
import MyTaskWidget from "@/views/Dashboard/DashboardWidgets/MyTaskWidget.vue";
import HelpDocumentation from "@/views/Dashboard/DashboardWidgets/HelpDocumentation.vue";
import BudgetPlan from "@/views/Dashboard/DashboardWidgets/BudgetPlan.vue";
import ProjectFundUtilisationDoughnet from "@/views/Dashboard/DashboardWidgets/ProjectFundUtilisationDoughnet.vue";
import BudgetAndFunds from "@/views/Dashboard/DashboardWidgets/BudgetAndFunds.vue";
import PlannedAndActual from "@/views/Dashboard/DashboardWidgets/PlannedAndActual.vue";
import ProjectFundUtilisationBar from "@/views/Dashboard/DashboardWidgets/ProjectFundUtilisationBar.vue";
import MyAccounts from "@/views/Dashboard/DashboardWidgets/MyAccounts.vue";
import ProjectSummary from "@/views/Dashboard/DashboardWidgets/ProjectSummary.vue";
import TaskSummary from "@/views/Dashboard/DashboardWidgets/TaskSummary.vue";
import MyTeam from "@/views/Dashboard/DashboardWidgets/MyTeam.vue";
import ProjectInsights from "@/views/Dashboard/DashboardWidgets/ProjectInsights.vue";
import DashboardHead from "@/views/Dashboard/DashboardWidgets/DashboardHead.vue";
import PreLoader from "@/components/pre-loader.vue"

var Dash,objProject;
async function importscript() {
    return Promise.all([
        import("@/BL/Dashboard.js").then(mod => {
            Dash = new mod.Dashboard();
        }),
        import("@/BL/MyProject.js").then(mod => {
          objProject = new mod.MyProject();
        })
    ]);
}

export default {
  data() {
    return {
      currentWsID: null,
      isProjectAccess: false,
      projectAccess: 0,
      isFinanceAccess: false,
      financeAccess: 0,
      isTaskAccess: false,
      taskAccess: 0,
      dashBoardData: null,
      preloader: true,
      ProjectTableItems: []
    };
  },
  components: {
    // 'Activity-DataCollector': ActivityDataCollector,
    MyProjectWidget: MyProjectWidget,
    Notification: Notification,
    ProjectProgress: ProjectProgress,
    ProjectTaskSummary: ProjectTaskSummary,
    ProjectFundUtilization: ProjectFundUtilization,
    MyTaskWidget: MyTaskWidget,
    HelpDocumentation: HelpDocumentation,
    BudgetPlan: BudgetPlan,
    ProjectFundUtilisationDoughnet: ProjectFundUtilisationDoughnet,
    BudgetAndFunds: BudgetAndFunds,
    PlannedAndActual: PlannedAndActual,
    ProjectFundUtilisationBar: ProjectFundUtilisationBar,
    MyAccounts: MyAccounts,
    ProjectSummary: ProjectSummary,
    TaskSummary: TaskSummary,
    MyTeam: MyTeam,
    ProjectInsights: ProjectInsights,
    DashboardHead: DashboardHead,
    PreLoader,
  },
  async mounted() {
    this.currentWsID = this.$route.query.WsID;
    await importscript();
    await this.fnFillDashboard();
    await this.GetProjectListByInitID()
    await this.getUserRoles()
    this.preloader = false
    
  },
  methods: {
    async getUserRoles() {
      let url = window.SERVER_URL + "/UserRoles/GetRoleBasedScreenList"
      let formdata = new FormData();
      formdata.append("WorkspaceID", this.currentWsID);
      let response = await fetch(url, {
        method: "POST",
        credentials: "include",
        body: formdata,
      });
      let result = await response.json();
      let data = result.data
      if (data.length > 0) {
        
        // Switch Statements

        for(let a of data){
          switch(a.ScreenName){
            case 'Projects':
                if(a.AccessId == 3 || a.AccessId == 2){
                  this.isProjectAccess = true
                  this.projectAccess = a.AccessId
                } else {
                  this.isProjectAccess = false
                  this.projectAccess = a.AccessId
                }
              break;
            case 'Tasks':
                if(a.AccessId == 3 || a.AccessId == 2){
                  this.isTaskAccess = true
                  this.projectAccess = a.AccessId
                } else {
                  this.isTaskAccess = false
                  this.taskAccess = a.AccessId
                }
              break;
          }
        }

      }
    },

    // function to populate data on all Tiles
    /// <summary>
    /// fnFillDashboard :- Function used to populate Dashboard Tiles
    /// </summary>

    async fnFillDashboard() {
      let formdata = new FormData();
      formdata.append("WorkspaceID", this.currentWsID);
      let result = await Dash.FillDashboard(formdata)
      this.dashBoardData = result.data[0]
    },

    async GetProjectListByInitID(Status) {
        if (Status == undefined || Status == "Deployed") {
            Status = 'Deploy'
        }
        var data = new FormData();
        data.append("WorkspaceID", this.$route.query.WsID);
        data.append("stage", Status);
        var result = await objProject.GetProjectListByInitID(data);
        if (result.status == "Unhandled Exception") {
            this.$router.push({ name: "error" });
        }
        else
            if (result.data) {
                this.ProjectTableItems = result.data;
            }
    },

    updateProjectList(e){
      this.GetProjectListByInitID(e)
    }
  },
};
</script>
