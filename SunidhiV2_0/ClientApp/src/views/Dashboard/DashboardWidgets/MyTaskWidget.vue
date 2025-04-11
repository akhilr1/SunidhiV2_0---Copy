<template>
  <v-container class="d-flex flex-column flex-nowrap ma-0 pa-0">
      <PreLoader v-if="preloader"></PreLoader>
      <v-card style="height: auto; max-height: 620px; min-height: 490px;">
        <v-row class="shrink">
          <v-col>
            <h3 class="text-left page-head-dark font-weight-medium pl-4 pt-3">
              My Tasks
            </h3>
          </v-col>
          <v-spacer class="hidden-xs-only"></v-spacer>
          <v-col class="col-2 pt-4 pr-4 hidden-sm-and-down">
            <v-select
              dense
              outlined
              label="Rows"
              hide-details
              :items="itemCount"
              v-model="itemCounter"
              @change="setItemsCount(itemCounter)"
              class="hidden-sm-and-down"
            >
            </v-select>
          </v-col>
          <v-col class="col-md-3 col-xs-5 pt-4 pr-4">
            <v-select
                v-if="dashdata"
                dense
                :items="dashdata.ProjectList"
                item-text="ProjectName"
                item-value="ProjectId"
                label="Select Project"
                hide-details
                outlined
                @change="getProjectTasks(ProjectID)"
                v-model="ProjectID"
                class="mr-2"
            ></v-select>
          </v-col>
        </v-row>
        
        <div class="hidden-xs-only">
          <v-row class="shrink" v-if="datasets.length > 0">
            <v-col cols="12" class="mb-0">
              <v-card elevation="0" >
                <v-row >
                  <v-col cols="4" class="text-center">Project Name</v-col>
                  <v-col cols="2">Assigned Date</v-col>
                  <v-col cols="2">Frequency</v-col>
                  <v-col cols="2">Assigned To</v-col>
                  <v-col cols="2">Beneficiary</v-col>
                </v-row>
              </v-card>
            </v-col>
          </v-row>

          <v-row class="px-2 py-0 v-scrolling-div" style="height: 45vh" >
            <v-col cols="12" v-if="datasets.length > 0">
              <v-card v-for="(item, index) in datasets" :key="index" class="mb-2 pl-4 taskcard" @click="fnRouteToMyTask()">
                  <div>
                    <v-card-title class="py-1">
                      <h6>
                        {{item.TaskName}}
                      </h6>
                    </v-card-title>
                  </div>
                  <v-row>
                    <v-col cols="4" class="py-2">
                      <v-card-subtitle class="d-flex align-center">
                        <v-chip color="grey darken-4" class="text--white mr-3">
                          {{item.SubmittedCount}}
                        </v-chip>
                        <li class="text-limit">{{item.ProjectName}}</li>
                      </v-card-subtitle>
                    </v-col>
                    <v-col cols="2" class="d-flex align-center">
                      <li class="text-limit">{{changeDateFormat(item.AssignedDate)}}</li>
                    </v-col>
                    <v-col cols="2" class="d-flex align-center">
                      <li class="text-limit">{{item.TaskFrequency}}</li>
                    </v-col>
                    <v-col cols="2" class="d-flex align-center">
                      <li class="text-limit">{{item.AssignedByUser}}</li>
                    </v-col>
                    <v-col cols="2" class="d-flex align-center">
                      <li class="text-limit">{{!item.Beneficiary.length ? 'N/A' : item.Beneficiary}}</li>
                    </v-col>
                  </v-row>
              </v-card>
            </v-col>



            <v-col v-else>
              <v-card elevation="0">
                <v-card-title>
                  No Tasks Found
                </v-card-title>
              </v-card>
            </v-col>
          </v-row>
        </div>

        <div class="hidden-sm-and-up">
          <v-data-table
            :headers="headers"
            :items="tasklist"
            :items-per-page="5"
            class="elevation-1 mobileOnlyTable"
            height="45vh"
            :loading="loader"
            loading-text="Loading"
            no-data-text="Nothing Found"
            hide-default-header
            @click:row="fnRouteToMyTask"
          > 
          <template #item.TaskName="{item}" class="text-limit">
            {{item.TaskName}}  <v-chip color="grey darken-4" class="text--white"> {{item.SubmittedCount}}</v-chip>
          </template>
          <template #item.AssignedDate="{item}">
            {{changeDateFormat(item.AssignedDate)}}
          </template>
          </v-data-table>
        </div>
      </v-card>
  </v-container>
</template>
<script>
import PrimaryButtonOutlined from "@/components/primary-button-outlined.vue";
import { dbImworks } from "@/ImworksDb";
import syncData from "@/ImworksMobApp.js";
import PreLoader from "@/components/pre-loader.vue";
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
      ProjectID: null,
      itemCount: [5,10,15,'All'],
      itemCounter: null,
      tasklist: [],
      loader: null,
      headers: [
        {text: "Task Name", align: "start", sortable: true, value: "TaskName"},
        { text: "Project Name", sortable: false, value: "ProjectName", },
        { text: "Assigned Date", value: "AssignedDate", sortable: false },
        { text: "Frequency", value: "TaskFrequency", sortable: false },
        { text: "Assigned By", value: "AssignedByUser", sortable: false },
        { text: "Beneficiary", value: "Beneficiary", sortable: false },
      ],
      datasets: [],
      preloader: true,
    };
  },
  props: ['dashdata', 'taskAccess'],
  async mounted() {
    await importscript();
    this.WorkspaceID = this.$route.query.WsID;
    await this.getMyTaskList()
    this.datasets = this.tasklist
    this.preloader = false
  },
  methods: {
    /// <summary>
    ///  getMyTaskList() :- Function used to get Task based on workspaceID
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async getMyTaskList() {
      let formdata = new FormData();
      formdata.append("WorkspaceID",this.WorkspaceID);
      let result = await Dash.FillDashboardMyTask(formdata)
      this.tasklist = result.data
    },

    getProjectTasks(ProjectID){
      let data = this.tasklist.filter(x=>x.ProjectId == ProjectID)
      this.datasets = data
    },

    changeDateFormat(DateFormat){
      let data = new Date(DateFormat).toLocaleDateString("en-IN")
      return data
    },

    setItemsCount(i) {
      if(i == 'All'){
        this.datasets = this.tasklist
      } else {
        this.datasets = this.tasklist.slice(0, i)
      }
    },

    // Routing to MyTask Page

    async fnRouteToMyTask() {
      this.preloader = true;
      // this.resetTimer();
      var lstTaskList = await dbImworks.MyTaskList.toArray();
      if (lstTaskList.length == 0) {
          await this.GetMyTaskFromServer();
          this.preloader = false;
          this.$router.push({
              name: "MyTasks",
              query: { WsID: this.WorkspaceID, Access: this.taskAccess },
          });
      } else {
          this.preloader = false;
          this.$router.push({
              name: "MyTasks",
              query: { WsID: this.WorkspaceID, Access: this.taskAccess },
          });
      }
    },

    async GetMyTaskFromServer() {
        let obj = syncData;
        await obj.syncData.SyncFromServer();
    },

  },
  components: {
    "Primary-Button-Outlined": PrimaryButtonOutlined,
    "PreLoader": PreLoader,
  },
};
</script>

<style scoped>
.taskcard{
  border-left: 6px solid lightblue;
}

.text-limit {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.text-limit:hover {
    white-space: normal;
    overflow: visible;
}

.v-data-table.v-data-table.v-data-table >>> tbody > tr.v-data-table__mobile-table-row {
  display: block !important;
  background: rgb(253, 253, 253);
  margin: 5px 5px;
  border-radius: 3px;
  border-left: lightblue solid 6px;
}

.v-scrolling-div::-webkit-scrollbar {
    width: 0px;
    display: none;
}

</style>
