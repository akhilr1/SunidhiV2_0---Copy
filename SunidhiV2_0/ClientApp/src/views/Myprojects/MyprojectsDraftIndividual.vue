<template>
  <v-card>
    <v-card-title>
      <h3 class="text-left page-head">
        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
          <v-icon size="15" color="primary"
            >mdi mdi-file-document-box-outline</v-icon
          >
        </v-avatar>
        <router-link
          :to="{
            name: 'MyprojectsDraft',
            query: { WsID: WorkspaceID, Access: RWAccess },
          }"
          class="page-head"
        >
          Draft Project
        </router-link>
        > {{ projectInfo.ProjectName }}
      </h3>
    </v-card-title>
    <v-card-text>
      <v-row >
          <v-col cols="12" md="11" sm="11" xs="11" align="end">
              <v-btn :hidden="unhide.includes('Details') == false ? true:false"
                     tile
                     link
                     :to="{
              name: 'MyprojectsDraftIndividual',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Details
              </v-btn>
              <v-btn tile
                     link
                     :hidden="unhide.includes('Team')== false ? true:false"
                     :to="{
              name: 'MyprojectsDraftIndividualTeam',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Team
              </v-btn>
              <v-btn tile
                     :hidden="unhide.includes('Outputs')== false ? true:false"
                     link
                     :to="{
              name: 'MyprojectsDraftIndividualActivities',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn':
                $route.name == 'MyprojectsDraftIndividualActivitiesIndividual',
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Activities & Inputs
              </v-btn>
              <v-btn id="AutoTest_IP170"
                     tile=""
                     link=""
                     :hidden="unhide.includes('Outcomes')== false ? true:false"
                     :to="{
              name: 'MyprojectsDraftIndividualKPIOutcomes',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn':
                $route.name == 'MyprojectsDraftIndividualKPIOutcomes' ||
                $route.name == 'MyProjectDraftIndividualOutcomeActivityLog',
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Indicators
              </v-btn>
              <!--<v-btn tile
           link
           :hidden="unhide.includes('Finance Details')== false ? true:false"
           :to="{
        name: 'MyprojectsDraftIndividualFinanceDetails',
        query: {
          WsID: WorkspaceID,
          Access: RWAccess,
          InitID: InitiativeId,
          ProjID: ProjectId,
        },
      }"
           v-bind:class="{
        'active-white-btn':
          $route.name ==
            'MyprojectsDraftIndividualFinanceDetailsBudget' ||
          $route.name == 'MyprojectsDraftIndividualFinancePaymentTerms',
      }"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        Inputs
    </v-btn>-->
              <v-btn name="btnSurvey"
                     tile
                     :hidden="unhide.includes('Surveys')== false ? true:false"
                     active-class="active-white-btn"
                     link
                     :to="{
              name: 'MyprojectsDraftIndividualSurveys',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn':
                $route.name == 'MyprojectsDraftIndividualSurveysIndividual',
            }"
                     class="mb-1 elevation-0">
                  Surveys
              </v-btn>
              <v-btn tile
                     :hidden="unhide.includes('Beneficiaries')== false ? true:false"
                     link
                     :to="{
              name: 'MyprojectsDraftIndividualBeneficiaries',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn':
                $route.name ==
                  'MyprojectsDraftIndividualBeneficiariesTypeIndividual' ||
                $route.name ==
                  'MyprojectsDraftIndividualBeneficiariesIndividual',
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Beneficiaries
              </v-btn>
              <v-btn tile
                     link
                     :hidden="unhide.includes('Workflow')== false ? true:false"
                     :to="{
              name: 'MyprojectsDraftIndividualWorkflow',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn': $route.name == 'ManageProjectWorkflow',
            }"
                     class="mb-1 elevation-0"
                     active-class="active-white-btn">
                  Workflow
              </v-btn>


              <!--<v-btn  tile link :to="{ name: 'MyprojectsDraftIndividualKPI', query: { WsID: WorkspaceID, InitID: InitiativeId, ProjID: ProjectId } }"
           v-bind:class="{ 'active-white-btn': $route.name == 'MyprojectsDraftIndividualKPIActivityLog'}"
           class="mb-1 elevation-0"
           active-class="active-white-btn">
        KPI
    </v-btn>-->
              <!--<v-btn
      tile
      :hidden="unhide.includes('Risks') == false? true:false"
      link
      :to="{
        name: 'MyprojectsDraftIndividualRisks',
        query: {
          WsID: WorkspaceID,
          Access: RWAccess,
          InitID: InitiativeId,
          ProjID: ProjectId,
        },
      }"
      v-bind:class="{
        'active-white-btn':
          $route.name == 'MyprojectsDraftIndividualRiskActivityLog',
      }"
      class="mb-1 elevation-0"
      active-class="active-white-btn"
    >
      Risks
    </v-btn>-->
              <v-btn tile
                     :hidden="unhide.includes('Documents')== false ? true:false"
                     link
                     :to="{
              name: 'MyprojectsDraftIndividualDocuments',
              query: {
                WsID: WorkspaceID,
                Access: RWAccess,
                InitID: InitiativeId,
                ProjID: ProjectId,
              },
            }"
                     v-bind:class="{
              'active-white-btn': $route.name == 'ManageProjectDocuments',
            }"
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
        <v-col cols="12" md="1" sm="1" xs="1">
          <v-dialog v-model="dialog" persistent max-width="800">
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                elevation="1"
                style="border-radius: 3px; min-width: 10"
                tile
                icon
                v-bind="attrs"
                v-on="on"
                ><v-icon style="font-size: 13px">mdi-grid</v-icon></v-btn>
            </template>
            <v-card style="overflow-x: hidden">
              <v-card-title primary-title class="page-head pb-0">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                  <v-icon size="20" color="primary">mdi-file-document-box-outline</v-icon> 
                </v-avatar>Hide / Unhide Tabs
                <v-spacer></v-spacer>
                <v-btn id="AutoTest_MP82" text @click="dialog = false, selectedTabs = prevselectedTabs">
                  <v-icon color="#707070" size="15">mdi-close</v-icon>
                </v-btn>
              </v-card-title>
              <v-card-text class="py-0 mb-3">
                <div style="text-align:center">
                  <span style="font-size:18px;color:black">Select checkbox to show tabs</span>
                </div>
                <div class="chip-container mb-4" v-if="selectedTabs.length > 0">
                  <v-chip-group multiple column active-class="primary--text">
                    <v-chip
                      id="AutoTest_U153"
                      close
                      v-for="item in selectedTabs"
                      :key="item"
                      class="ma-2"
                      @click:close="remove(item)"
                      small
                      color="#DBF1FF"
                    >
                      <v-icon left color="#3374B9" size="15">mdi-network-outline</v-icon>
                      {{ item.text }}
                    </v-chip>
                  </v-chip-group>
                </div>
                <v-container class="px-0">
                  <v-row
                    no-gutters
                    class="v-scrolling-div"
                    style="height: 400px"
                  >
                    <v-col sm="12">
                      <v-data-table
                        v-model="selectedTabs"
                        :headers="headers"
                        hide-default-footer
                        fixed-header
                        :items="allTab"
                        :expanded="expanded"
                        item-key="id"
                        show-select
                        show-expand
                        class="elevation-1"
                        @click:row="clicked"
                      >
                        <template v-slot:items="props">
                          <tr @click="props.expanded = !props.expanded">
                            <td>
                              <!-- This one should select every items under the expand slot -->
                              <v-checkbox
                                @click.stop="props.selected = !props.selected"
                                v-model="selectedTabs"
                                primary
                                hide-details
                              ></v-checkbox>
                            </td>
                            <td class="text-xs-right">
                              {{ props.item.text }}
                            </td>
                          </tr>
                        </template>
                        <template v-slot:expanded-item="{ headers }">
                          <td :colspan="headers.length">Peek-a-boo!</td>
                        </template>
                      </v-data-table>
                    </v-col>
                  </v-row>
                </v-container>
                <v-container>
                  <v-row>
                    <v-col
                      cols="12"
                      xs="12"
                      sm="12"
                      md="12"
                      class="text-right py-0 my-0"
                    >
                    <v-btn color="white"  @click="dialog = false, selectedTabs = prevselectedTabs" elevation="1" class="mr-3">
                       <span style="color:#707070">Cancel</span> 
                     </v-btn>
                     <v-btn color="#439bde" class="white--text" elevation="1" @click="unhideTabs()">
                       Confirm
                     </v-btn>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>
            </v-card>
          </v-dialog>
        </v-col>
        <v-col
          cols="12"
          xs="12"
          sm="12"
          md="12"
          class="py-0 px-0"
          style="min-height: 80vh"
        >
          <router-view></router-view>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script type="text/javascript">
var objManageProject;
async function importscript() {
  return Promise.all([
    import("../../BL/ManageProject.js").then((mod) => {
      objManageProject = new mod.ManageProject();
    }),
  ]);
}

export default {
  data() {
    return {
      expanded: [],
      headers: [
        { text: "Screen Name", align: "start",sortable: true, value: "text",},
        { text: "Info", value: "data-table-expand" },
      ],
      unhide:[
        "Details","Team","Finance Details","Workflow", "Surveys","Beneficiaries","Outputs","Outcomes","Risks","Documents"
      ],
      selectedTabs: [
        { id: 0, text: "Details", },
        { id: 1, text: "Team", },
        { id: 2, text: "Finance Details", },
        { id: 3, text: "Workflow", },
        { id: 4, text: "Surveys", },
        { id: 5, text: "Beneficiaries", },
        { id: 6, text: "Outputs", },
        { id: 7, text: "Outcomes", },
        { id: 8, text: "Risks", },
        { id: 9, text: "Documents", },
      ],
      prevselectedTabs: [
        { id: 0, text: "Details", },
        { id: 1, text: "Team", },
        { id: 2, text: "Finance Details", },
        { id: 3, text: "Workflow", },
        { id: 4, text: "Surveys", },
        { id: 5, text: "Beneficiaries", },
        { id: 6, text: "Outputs", },
        { id: 7, text: "Outcomes", },
        { id: 8, text: "Risks", },
        { id: 9, text: "Documents", },
      ],
      dialog: false,
      allTab: [
        { id: 0, text: "Details", },
        { id: 1, text: "Team", },
        { id: 2, text: "Finance Details", },
        { id: 3, text: "Workflow", },
        { id: 4, text: "Surveys", },
        { id: 5, text: "Beneficiaries", },
        { id: 6, text: "Outputs", },
        { id: 7, text: "Outcomes", glutenfree: false, },
        { id: 8, text: "Risks", },
        { id: 9, text: "Documents", },
      ],
      WorkspaceID: 0,
      InitiativeId: 0,
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
    };
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
    ///  remove :- Function used to remove tabs from the selected tab list
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    remove(item){
      this.selectedTabs = this.selectedTabs.filter((v,i)=>v.id != item.id)
    },

    /// <summary>
    ///  unhideTabs :- Function used to unhide tabs when click on confirm button
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    unhideTabs(){
      this.unhide = []
      this.prevselectedTabs = this.selectedTabs
      this.selectedTabs.map((val,index)=>{
        this.unhide[index] = val.text
      })
      this.dialog = false
    },

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
        if (this.SelectedTag == null) {
          this.SelectedTag = [];
        }
        //alert("Tags length =" + this.projectInfo.ProjectTagsList.length);
      } else {
        this.Msg =
          "Failed to get details of Project " + this.projectInfo.ProjectId;
        this.errorSnackbarMsg = true;
      }
    },
  },
};
</script>