<template>
    <v-card elevation="1">
        <v-row class="pt-1" >
            <v-col md="8"  class="pt-5">
                <h1 class="text-left page-head-dark font-weight-medium pl-4">
                    Fund Utilisation
                </h1>
            </v-col>
            <v-col md="4"  class="d-flex justify-end">
                <v-select dense
                          label="Select Project"
                          outlined hide-details  class="mr-2" :items="ProjectList" item-text="ProjectName"
                                                                          item-value="ProjectId"  @change="GetPaymentBudgetHead(ProjectID)" v-model="ProjectID"></v-select>

            </v-col>
        </v-row>
        <v-row>
            <v-col cols="7">
                <v-list nav dense>
                    <v-list-item>
                        <v-list-item-content>
                            <h2 class="text-left font-weight-bold">
                                <!--<v-icon>mdi-currency-inr</v-icon>-->
                                {{DonarUtilizedFund}}
                            </h2>
                            <span class="caption">Fund Utilisation</span>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item>
                        <v-list-item-content>
                            <h2 class="text-left font-weight-bold">
                                <!-- <v-icon>mdi-currency-inr</v-icon>-->
                                {{budgetAmt}}
                            </h2>
                            <span class="caption">Total Budget</span>
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-col>
            <v-col cols="5" class="text-center pt-2">
                <v-progress-circular :rotate="360"
                                     :size="120"
                                     :width="10"
                                     :value=FundUtilizationPercentage
                                     color="red" class="mb-4">
                    <span style="color:black;font-size:20px;font-weight:600;">{{FundUtilizationPercentage}}%</span>
                </v-progress-circular>
            </v-col>
        </v-row>
    </v-card>
</template>
<script>
    import MyProjectWidget from '@/views/Dashboard/DashboardWidgets/MyProjectWidget.vue'
    import Notification from '@/views/Dashboard/DashboardWidgets/NotificationWidget.vue'
    import { dbImworks } from '../../../ImworksDb'

    var objDashboard;
    async function importscript() {
        return Promise.all([
            import("../../../BL/Dashboard.js").then(mod => {
                objDashboard = new mod.Dashboard();

            })
        ]);
    };
    var objFinance;
    async function importMyFinancescript() {
        return Promise.all([
            import("../../../BL/Finance.js").then(mod => {

                objFinance = new mod.Finance();
            })
        ]);
    };
    var objProject;
    async function importFinancescript() {
        return Promise.all([
            import("../../../BL/ManageProjectFinance.js").then(mod => {
                objProject = new mod.ManageProjectFinance();
            })
        ]);
    };
    export default {
        data() {
            return {
                WorkspaceID: 0,
                ProjectID: 0,
                BudgetHeadList: [],
                ProjectList: [],
                budgetAmt: 0,
                DonarUtilizedFund: 0,
                FundUtilizationPercentage: 0,
                BudgetDetails: [],
                Budget : 0,


            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            await importscript();
            await importMyFinancescript();
            await importFinancescript();
            await this.GetProjectListByWSID();
            await this.GetPaymentBudgetHead();



        },
        methods: {
            /// <summary>
            ///  GetProjectListByWSID :- Function used to fetch project details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectListByWSID() {
                var dashboardProjectList = await dbImworks.TbDashboardProjectList.toArray();

                if (dashboardProjectList.length > 0) {
                    this.ProjectList = dashboardProjectList;
                    this.ProjectID = dashboardProjectList[0].ProjectId;
                }
                else {
                    var data = new FormData();
                    data.append("WorkspaceID", this.WorkspaceID);
                    var result = await objDashboard.GetProjectListByWSID(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.data.length > 0) {
                            this.ProjectList = result.data;
                            this.ProjectID = result.data[0].ProjectId;
                        }
                    }
                }
            },

            /// <summary>
            ///  GetPaymentBudgetHead :- Function used to fetch payment budget head details when selecting a project from dropdown based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetPaymentBudgetHead() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objFinance.GetPaymentBudgetHead(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.BudgetHeadList = result.data;
                    this.DonarUtilizedFund = 0;

                    if (this.BudgetHeadList.length > 0) {

                        var i = 0;
                        for (i = 0; i <= this.BudgetHeadList.length - 1; i++) {
                            this.DonarUtilizedFund = parseInt(this.DonarUtilizedFund) + parseInt(this.BudgetHeadList[i].HeadAmount);
                        }
                    }
                }
                this.GetProjectBudgetDetails();
            },

            /// <summary>
            ///  GetProjectBudgetDetails :- Function used to fetch project budget details based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectBudgetDetails() {
                this.FundUtilizationPercentage = 0;
                this.budgetAmt = 0;
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objProject.GetProjectBudgetDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data) {
                    this.BudgetDetails = result.data;
                    if (this.BudgetDetails.length > 0) {
                        var i = 0;
                        for (i = 0; i <= this.BudgetDetails.length - 1; i++) {
                            this.Budget = this.BudgetDetails[i].Budget
                            if (this.Budget > 0) {
                                this.budgetAmt = this.Budget;
                                this.FundUtilizationPercentage = ((this.DonarUtilizedFund * 100) / this.budgetAmt).toFixed();
                            }
                        }
                    }

                }
            },
        },
        components: {
            ////   'Activity-DataCollector': ActivityDataCollector,
            'MyProjectWidget': MyProjectWidget,
            'Notification': Notification,
        },
    }
</script>
