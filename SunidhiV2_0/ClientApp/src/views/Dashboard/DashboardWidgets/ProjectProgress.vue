<template>
    <v-card elevation="1" style="height:100%">
        <v-row class="pt-1">
            <v-col md="8"  class="pt-5">
                <h1 class="text-left page-head-dark font-weight-medium pl-4">
                    {{ProjectName}}
                </h1>
            </v-col>
            <v-col md="4" class="d-flex justify-end">
                <v-select dense
                          label="Select Project"
                          outlined hide-details  class="mr-2" :items="dashdata.ProjectList" item-text="ProjectName"
                          item-value="ProjectId" @change="getProjectName(ProjectID),getProjectProgressByProjectID(ProjectID)" v-model="ProjectID"></v-select>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="7">
                <v-list nav dense>
                    <v-list-item style="margin-top:-10px">
                        <v-list-item-content>
                            <h2 class="text-left font-weight-bold">
                                {{ProjectProgress}}%
                            </h2>
                            <span class="caption">Project progress</span>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item>
                        <v-list-item-content>
                            <span class="subtitle-2" v-if="ProjectProgress==='100'">The project completed</span>
                            <span class="subtitle-2" v-else>The project progress is increased to {{ProjectProgress}}% out of 100%</span>

                        </v-list-item-content>

                    </v-list-item>
                </v-list>
            </v-col>
            <v-col cols="5" class="text-center pt-2">
                <v-progress-circular :rotate="270"
                                     :size="120"
                                     :width="10"
                                     :value=ProjectProgress
                                     color="#20C997" class="mb-4">
                    <span style="color:black;font-size:20px;font-weight:600;">{{ProjectProgress}}%</span>
                </v-progress-circular>
            </v-col>
        </v-row>
        <v-col class="pa-0 ma-0 text-right pr-2 py-1">
            <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                    <span v-bind="attrs" v-on="on">
                        last updated: {{timeAgo}} ago
                    </span>
                </template>
                <span>Updation date: {{updatedTime}}</span>
            </v-tooltip>
        </v-col>
    </v-card>
</template>
<script>
    import MyProjectWidget from '@/views/Dashboard/DashboardWidgets/MyProjectWidget.vue'
    import Notification from '@/views/Dashboard/DashboardWidgets/NotificationWidget.vue'
    import { dbImworks } from '../../../ImworksDb'
    import objUtils from '@/utils.js'

    var objDashboard;
    async function importscript() {
        return Promise.all([
            import("../../../BL/Dashboard.js").then(mod => {
                objDashboard = new mod.Dashboard();

            })
        ]);
    };
    export default {
        data() {
            return {
                WorkspaceID: 0,
                ProjectID: 0,
                ProjectList: null,
                ProjectName: 'Total Progress',
                ProjectProgress:0,
            }
        },
        props: ['dashdata', 'updatedTime'],
        async mounted() {
           
            this.WorkspaceID = this.$route.query.WsID;
            await importscript();
            this.ProjectList = this.dashdata.ProjectList
            this.getProjectProgress()
        },
        methods: {

            /// <summary>
            ///  GetProjectName :- Function used to get a project name from the project list when selecting a project based on ProjectId
            /// </summary>
            /// <param name="ProjectId"></param>
            /// <returns> </returns>
            getProjectName(ProjectId) {
                
                var temp = this.ProjectList.filter(x => x.ProjectId == ProjectId)
                if (temp.length > 0) {
                    this.ProjectName = temp[0].ProjectName;
                }
            },

            getProjectProgress() {
                
                let temp = 0;
                let count = 0;
                this.ProjectList.map((x) => {
                    temp += x.Progress;
                    count++;
                })
              
                if (temp > 0 && count > 0) {
                    this.ProjectProgress = Math.round(temp / count);
                }
            },

            getProjectProgressByProjectID(ProjectId) {
                let data = this.ProjectList.filter((x)=> {
                    return x.ProjectId == ProjectId
                })
                this.ProjectProgress = data[0].Progress
            },
        },
        components: {
            ////   'Activity-DataCollector': ActivityDataCollector,
            'MyProjectWidget': MyProjectWidget,
            'Notification': Notification,
        },
        computed: {
            timeAgo(){
                return objUtils.common.timeSince(this.updatedTime)
            }
        }
    }
</script>
