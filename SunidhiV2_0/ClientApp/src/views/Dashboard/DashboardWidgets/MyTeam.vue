<template>
  <v-card
    style="height: auto; max-height: 620px; min-height: 490px"
    elevation="1"
  >
    <v-container fluid class="pt-0 pb-0">
      <v-row class="pt-1">
        <v-col sm="6" md="9" cols="5" class="pt-5 pl-0">
          <h1 class="header-font page-head-dark font-weight-medium pl-4">
            My Team
          </h1>
        </v-col>
        <v-col sm="6" md="3" cols="7">
          <v-select v-if="dashdata"
            dense
            label="Select Project"
            outlined
            hide-details
            class="mr-2"
            :items="ProjectList"
            item-text="ProjectName"
            item-value="ProjectId"
            v-model="ProjectID"
            @change="getTeamsDetails()"
          ></v-select>
        </v-col>
      </v-row>
      <v-row class="mt-5">

        <!-- Spinner -->
          <!-- <template v-if="loading" class="d-flex justify-center align-center">
              <v-progress-circular
                :size="70"
                color="primary"
                indeterminate
              ></v-progress-circular>
          </template> -->

        <v-col  cols="12" md="12" sm="12" xs="12" class="px-1 py-0">
          <v-data-table
            :headers="headers"
            :items="UsersList"
            :items-per-page="10"
            class="elevation-1"
            fixed-header
            height="45vh"
            :loading="loading"
            loading-text="Loading"
            no-data-text="No Users Found"
          >
            <template v-slot:item.name="{ item }">
              <div class="py-3">
                <v-avatar>
                  <v-img
                    :src="item.ProfileImage ? defurl + item.ProfileImage : profileimg"
                  ></v-img>
                </v-avatar>
                <span class="pl-7">
                  {{ item.Name?item.Name:'Not Defined' }}
                </span>
              </div>
            </template>
            <template v-slot:item.user="{ item }">
              <div>
                <span class="pl-7">
                  {{ item.Role }}
                </span>
              </div>
            </template>
            <template v-slot:item.tasks="{ item }">
              <span>{{item.TaskCount}}</span>
            </template>
            <template v-slot:item.progress="{ item }">
              <v-progress-linear
                color="green"
                rounded
                height="10"
                :value="item.UserProgress"
                style="position: relative; bottom: -22%; width: 50%"
              ></v-progress-linear>
              <span style="position: relative; left: 54%; bottom: 0%"
                >{{ item.UserProgress }}%</span>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>
<script>
import profileimg from "@/images/ic_profile_image.png"
var Dash
async function importscript() {
    return Promise.all([
        import("@/BL/Dashboard.js").then(mod => {
            Dash = new mod.Dashboard();
        })
    ]);
}
export default {
  data() {
    return {
      headers: [
        {
          text: "Name",
          align: "start",
          sortable: false,
          value: "name",
        },
        { text: "User Role", value: "user", sortable: false },
        { text: "Tasks Counts", value: "tasks", sortable: false },
        { text: "Progress", value: "progress", sortable: false },
        // { text: "Actions", value: "action", sortable: false },
      ],
      WorkspaceID: null,
      ProjectID: null,
      UsersList: [],
      loading: false,
      defurl: null,
      profileimg: profileimg,
      ProjectList: [],
      dashdata : null
    };
  },
  async mounted() {
    await importscript();
    this.defurl = window.SERVER_URL + "/Upload/"
    this.WorkspaceID = this.$route.query.WsID;
    await this.getDashboardData()
    this.getInitialTeams() 
  },
  methods: {
    progressColor: function (index) {
      if (this.UsersList[index].UserProgress == 100) {
        return "#F77C7B";
      } else {
        return "#9488FF";
      }
    },

    // getTeamsDetails() :- to get Data for Teams Tile
    // params - <<ProjectId>> <<WorkspaceID>>
    async getTeamsDetails() {

      this.loading = true;
      let formdata = new FormData();
      formdata.append("WorkspaceID", this.WorkspaceID);
      formdata.append("ProjectId", this.ProjectID);
      let result = await Dash.FillDashboardMyTeam(formdata);
      this.UsersList = result.data
      // console.log(this.UsersList, this.ProjectID);
      this.loading = false;

    },

    async getDashboardData() {
      
      let formdata = new FormData();
      formdata.append("WorkspaceID", this.WorkspaceID);
      let result = await Dash.FillDashboard(formdata)
      this.dashdata = result.data[0]
      // console.log(this.dashdata)

    },

    async getInitialTeams() {
      if(this.dashdata){
        this.ProjectList = this.dashdata.ProjectList
        if(this.ProjectList.length > 0){ 
          this.ProjectID = this.ProjectList[0].ProjectId
        }
        // console.log(this.ProjectList, this.ProjectID)
        await this.getTeamsDetails()
      }
    }
  },
};
</script>