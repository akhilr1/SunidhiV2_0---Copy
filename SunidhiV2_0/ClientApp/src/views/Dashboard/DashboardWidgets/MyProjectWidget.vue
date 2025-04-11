<template>
  <v-card style="height: auto;max-height: 600px;min-height: 490px;" elevation="1">
    <v-row class="pt-1">
      <v-col md='9' class="pt-5">
        <h3 class="page-head-dark font-weight-medium pl-4">My Projects</h3>
      </v-col>
      <v-col md='3'>
        <v-select
          dense
          :items="lstSortStatus"
          label="Sort Status"
          outlined
          hide-details
         @change="updateProjectListByStatus(Status)"
         v-model="Status"
          class="mr-2"
        ></v-select>
      </v-col>
    </v-row>
    <!--Main Body-->
    <v-row v-show="ProjectTableItems.length > 0">
      <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
        <div >
          <v-data-table
            :headers="ProjectTableheaders"
            fixed-header
            :items="ProjectTableItems"
            item-key="ProjectId"
            class="elevation-0"
            height="45vh"
          >
            <!-- Column Name -->
            <template v-slot:item.ProjectName="{ item }">
              <v-list dense class="py-0 table-v-list">
                <!--if path is ManageProjectBeneficiaries-->
                <v-list-item class="my-0 px-0">
                  <v-list-item-icon>
                    <v-avatar size="30" color="#fff" class="elevation-0">
                      <v-icon size="20" color="primary">mdi-folder</v-icon>
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content>
                    {{ item.ProjectName }}
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </template>
            <!-- column status-->
            <template v-slot:item.Status="{ item }">
                <div class="d-inline-flex" v-if="item.ProjectStatus == 'Completed' || item.Progress === 100">
                    <v-chip color="#DFFFEC" class="ml-0 mt-0" small>
                        <span style="color: #20c997">Completed</span>
                    </v-chip>
                </div>
                <div class="d-inline-flex" v-if="item.Progress === 0 ">
                    <v-chip color="#DFFFEC" class="ml-0 mt-0" small>
                        <span style="color: #20c997">Deployed</span>
                    </v-chip>
                </div>
                <div class="d-inline-flex" v-if="item.Progress > 0 && item.Progress != 100">
                    <v-chip color="#DFFFEC" class="ml-0 mt-0" small>
                        <span style="color: #0099ff">In Progress</span>
                    </v-chip>
                </div>
                <div class="d-inline-flex" v-if="item.ProjectStatus == 'Archived'">
                    <v-chip color="#DFFFEC" class="ml-0 mt-0" small>
                        <span style="color: #e6217b">Archived</span>
                    </v-chip>
                </div>
            </template>
            <!-- DateTime-->
            <template v-slot:item.StartDate="{ item }">
                <span>{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</span>
            </template>
            <template v-slot:item.EndDate="{ item }">
                <span>{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</span>
            </template>

            <template v-slot:item.Progress="{ item }">
              <!-- <v-progress-linear color="green" height="25">
                <span>{{ 50 }}%</span>
              </v-progress-linear> -->
              <v-progress-linear
                color="green"
                rounded
                height="10"
                :value=item.Progress
                style="position: relative; bottom: -22%; width: 50%"
              ></v-progress-linear>
              <span style="position: relative; left: 54%; bottom: 10%">{{ item.Progress }}%</span>
            </template>
            <!-- project Actions-->
            <template v-slot:item.Actions="{ item }">
              <v-menu
                close-on-content-click
                offset-y
                left
                transition="scroll-y-transition"
              >
               
              </v-menu>
            </template>
          </v-data-table>
        </div>
      </v-col>
    </v-row>
    <!-- else part if No project available-->
    <v-row v-show="ProjectTableItems.length == 0">
      <v-col cols="12" sm="12" md="12">
        <v-row :justify="justifyCenter">
          <v-col md="12" class="text-center">
            <v-avatar
              size="80"
              tile
              class="elevation-1 base-border-radius mr-2"
            >
              <v-img
                src="../../../images/ic_no_workspace.svg"
                height="80"
                width="80"
                class="ma-4"
              ></v-img>
            </v-avatar>
            <p class="heading-3 pt-3">No projects available</p>
       
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
import PrimaryButtonOutlined from "@/components/primary-button-outlined.vue";
    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../../BL/MyProject.js").then(mod => {
                objProject = new mod.MyProject();
            })
        ]);
    };
export default {
  data() {
    return {
      justifyCenter: "center",
      ProjectTableheaders: [
        { text: "Name", align: "left", value: "ProjectName" },
        { text: "Code", value: "Code" },
        { text: "Start Date", value: "StartDate" },
        { text: "End Date", value: "EndDate" },
        { text: "Status", value: "Status", sortable: false },
        { text: "Progress", value: "Progress" },
        // { text: "Actions", value: "Actions", sortable: false },
      ],
      lstSortStatus: ["Completed", "Deployed", "Archived", "Draft"  ],
      ProjectId: 0,
      Status: 'Deploy',
    };
  },
  props: ['ProjectTableItems'],
  components: {
    "Primary-Button-Outlined": PrimaryButtonOutlined,
  },
  mounted: async function () {
    
      this.WorkspaceID = this.$route.query.WsID;
      await importscript();


  },
  methods: {
    async updateProjectListByStatus(Status) {
      this.$emit('statusChange', Status )
    },
  },
};
</script>
