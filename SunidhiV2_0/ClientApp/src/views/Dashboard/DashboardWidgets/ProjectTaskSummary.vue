<template>
    <v-card elevation="1">
        <v-row class="pt-1">
            <v-col md="8" class="pt-5">
                <h1 class="text-left page-head-dark font-weight-medium pl-4">
                    Pending Activities
                </h1>
            </v-col>
            <v-col md="4"  class="d-flex justify-end">
                <v-select dense
                          label="Select Project"
                          outlined hide-details  class="mr-2" :items="dashdata.ProjectList" item-text="ProjectName"
                          item-value="ProjectId" @change="getPendingActivitesByProjectId(ProjectID), getTotalActivitiesByProjectId(ProjectID)" v-model="ProjectID"></v-select>

            </v-col>
        </v-row>
        <v-row>
            <v-col cols="7">
                <v-list nav dense>
                    <v-list-item>
                        <v-list-item-content>
                            <h2 class="text-left font-weight-bold">
                                {{PendingActivites}}
                            </h2>
                            <span class="caption">Pending Activites</span>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item>
                        <v-list-item-content>
                            <h2 class="text-left font-weight-bold">
                                {{TotalActivities}}
                            </h2>
                            <span class="caption">Total Activities</span>
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-col>
            <v-col cols="5" class="text-center pt-2">
                <v-progress-circular :rotate="270"
                                     :size="120"
                                     :width="10"
                                     :value=Percentage
                                     color="secondary" class="mb-4">
                    <span style="color:black;font-size:20px;font-weight:600;">{{Percentage}}%</span>
                </v-progress-circular>
            </v-col>
        </v-row>
    </v-card>
</template>
<script>
    import MyProjectWidget from '@/views/Dashboard/DashboardWidgets/MyProjectWidget.vue'
    import Notification from '@/views/Dashboard/DashboardWidgets/NotificationWidget.vue'
    import { dbImworks } from '../../../ImworksDb'

    export default {
        data() {
            return {
                WorkspaceID: 0,
                ProjectID: 0,
                ProjectList: [],
                PendingActivites: 0,
                TotalActivities: 0,
                CompletedActivities:0,
                ActiveList: [],
                AssignedList: []

            }
        },
        props: ['dashdata'],
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.getData()
            this.getPendingActivities()
            this.getTotalActivities()
            this.TotalActivities = this.getTotalActivities()
            this.PendingActivites = this.getPendingActivities()
        },
        methods: {

            getData() {
                this.ActiveList = this.dashdata.ActiveList
                this.AssignedList = this.dashdata.AssignedList
            },

            getPendingActivities() {
                let temp = 0;
                this.ActiveList.map((x)=> {
                    temp += x.ActiveCunt
                })
                return temp
            },

            getTotalActivities() {
                let temp = 0;
                this.AssignedList.map((x)=> {
                    temp += x.AssignedCunt
                })
                return temp
            },

            getPendingActivitesByProjectId(ProjectId) {
                let data = this.ActiveList.filter((x)=> {
                    return x.ProjectId == ProjectId
                })
                if(data.length > 0) {
                    this.PendingActivites = data[0].ActiveCunt
                } else {
                    this.PendingActivites = 0
                }
            },

            getTotalActivitiesByProjectId(ProjectId) {
                let data = this.AssignedList.filter((x)=> {
                    return x.ProjectId == ProjectId
                })
                if(data.length > 0) {
                    this.TotalActivities =  data[0].AssignedCunt
                } else {
                    this.TotalActivities = 0
                }
            },
        },
        computed: {
            Percentage() {
                let perc = (this.PendingActivites/this.TotalActivities)*100
                if(Number.isNaN(perc)){
                    return 0
                } else if(perc == 100 || perc == 0){
                    return perc.toFixed(0)
                } else {
                    return perc.toFixed(1)
                }
            }
        },
        components: {
            ////   'Activity-DataCollector': ActivityDataCollector,
            'MyProjectWidget': MyProjectWidget,
            'Notification': Notification,
        },
    }
</script>
