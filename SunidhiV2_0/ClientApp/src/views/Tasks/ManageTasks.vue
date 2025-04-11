<template>
    <v-container fluid class="pb-0 mb-0">
        <v-row no-gutters>
            <v-col>
                <v-card>
                    <v-card-title style="height:60px" class="pt-4">
                        <h5 class="text-left page-head">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-file-document-box-check-outline</v-icon>
                            </v-avatar>
                            Tasks
                        </h5>
                    </v-card-title>   
                        <v-row no-gutters>
                          <v-col cols="12">
                            <v-row no-gutters align="center" justify="center">
                              <v-slide-group>
                                <v-slide-item  v-slot="{ active, toggle }">
                                  <v-btn tile="" link="" :to="{ name: 'OverdueTasks',query: {WsID: WorkspaceID}}"
                                      v-bind:class="{ 'active-white-btn': $route.name == 'OverdueTasks' || $route.name == 'ManageTasks' }"
                                      class="mb-1 elevation-0"
                                      active-class="active-white-btn" @click="toggle">
                                    Over Due
                                  </v-btn>
                                </v-slide-item>

                                <v-slide-item  v-slot="{ active, toggle }">
                                  <v-btn tile="" link="" :to="{ name: 'CurrentTasks' ,query: {WsID: WorkspaceID}}"
                                      v-bind:class="{ 'active-white-btn': $route.name == 'CurrentTasks' || $route.name == 'ManageTasks'}"
                                      class="mb-1 elevation-0"
                                      active-class="active-white-btn" @click="toggle">
                                    Current
                                  </v-btn>
                                </v-slide-item>

                                <v-slide-item  v-slot="{ active, toggle }">
                                  <v-btn tile="" link="" :to="{ name: 'CompletedTasks' ,query: {WsID: WorkspaceID}}"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'CompletedTasks' || $route.name == 'ManageTasks' }"
                                   class="mb-1 elevation-0"
                                   active-class="active-white-btn" @click="toggle">
                                    Completed
                                  </v-btn>
                                </v-slide-item>

                                <v-slide-item  v-slot="{ active, toggle }">
                                  <v-btn tile="" link="" :to="{ name: 'SavedTasks' ,query: {WsID: WorkspaceID}}"
                                          v-bind:class="{ 'active-white-btn': $route.name == 'SavedTasks' || $route.name == 'ManageTasks' }"
                                          class="mb-1 elevation-0"
                                          active-class="active-white-btn" @click="toggle">
                                    Saved
                                  </v-btn>
                                </v-slide-item>
                              </v-slide-group>
                            </v-row>
                          </v-col>
                            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                <v-card class="elevation-0">
                                    <router-view></router-view>
                                </v-card>
                            </v-col>
                        </v-row>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };

    export default {
        data() {
            return {
                justifyCenter: "center",
                VentureType: "",
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: ""
                },
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: ""
                },
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            //this.projectInfo.ProjectId = this.$route.query.ProjID;
            //this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            //await importscript();
            //await this.fnGetProjectDetailsByID();

        },
        methods: {
            //async fnGetProjectDetailsByID() {
            //    var data = new FormData();
            //    data.append("ProjectId", this.projectInfo.ProjectId);
            //    data.append("InitiativeId", this.initiativeInfo.InitiativeId);
            //    var result = await objManageProject.GetProjectDetailsByID(data);
            //    if (result.data) {
            //        this.projectInfo = result.data;
            //    } else {
            //        alert("Failed to get details of Project ", this.projectInfo.InitiativeId);
            //    }
            //},
        }
    };
</script>