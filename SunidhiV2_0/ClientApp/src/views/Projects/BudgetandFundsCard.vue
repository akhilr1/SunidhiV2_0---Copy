<template>
    <v-card class="custom-card-1" height="270">
        <v-card-title>
            Budget & Funds
            <v-spacer></v-spacer>

            <router-link :to="{name:'ManageProjectFinanceBudget',query: { WsID: WorkspaceID, Access:RWAccess, InitID : InitiativeID,ProjID: ProjectID}}">
                <Primary-Button-Outlined title="View All"></Primary-Button-Outlined>
            </router-link>
        </v-card-title>
        <v-card-text>
            <v-row>
                <v-col md="6">
                    <Doughnut-Chart :doughnutChartdata="BudgetChartData1" :doughnutChartOptions="BudgetChartOptions" style="width:100%; height:300px"></Doughnut-Chart>
                </v-col>
                <!--<v-col md="6">
                    <Doughnut-Chart :doughnutChartdata="BudgetChartData" :doughnutChartOptions="BudgetChartOptions" style="width:100%; height:300px"></Doughnut-Chart>
                </v-col>-->
                <v-col md="6">
                    <h5 class="heading-3-dark">
                        Total budget<br />
                        <span class="bold">Rs {{budgetAmt}}</span>
                    </h5>
                    <v-menu max-width="300" absolute v-model="AssignFinUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                        <template v-slot:activator="{ on }">
                            <v-btn id="AutoTest_IP24" v-on="on" max-width="170px" min-width="170px" class="mt-4 success-btn btn-sm" depressed="" dark="" height="30"
                                   @click="GetProjectUsers('Finance')" v-if="lstBudgetFinUserInfo.length==0">
                                Add Finance Users
                            </v-btn>
                        </template>
                        <!--Assign finance user pop up-->
                        <v-card>
                            <v-card-title class="heading-3">
                                Assign Finance Users
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_IP25" text="" height="25" min-width="25" max-width="25" color="#707070" @click="vmFinanceUsers=[],AssignFinUserPopUp=false">
                                    <v-icon size="20">mdi-close</v-icon>
                                </v-btn>
                            </v-card-title>
                            <v-divider></v-divider>
                            <v-card-text style="height:150px" class="v-scrolling-div">
                                <v-data-table :headers="lstuserTableheaders"
                                              :items="lstFinanceUsers"
                                              show-select
                                              item-key="UserID"
                                              sort-by="UserName"
                                              v-model="vmFinanceUsers"
                                              hide-default-footer
                                              hide-default-header>

                                    <template v-slot:item.UserName="{item}">
                                        <v-list class="py-0 table-v-list">
                                            <v-list-item class="my-0 px-0">
                                                <v-list-item-icon>
                                                    <v-avatar size="27" class="elevation-1">
                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="User DP" />
                                                    </v-avatar>
                                                </v-list-item-icon>
                                                <v-list-item-content>
                                                    {{item.UserName}}
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list>
                                    </template>
                                    <template v-slot:no-data>
                                        <v-alert :value="true" color="error" icon="warning">
                                            Sorry, nothing to display here :(
                                        </v-alert>
                                    </template>
                                </v-data-table>

                            </v-card-text>
                            <v-divider></v-divider>
                            <v-card-actions>
                                <v-row no-gutters="">
                                    <v-col md="12" class="py-0">
                                        <v-checkbox color="#52B962" :label="`Notify Finance Users`" v-model="ChkFinNotify"></v-checkbox>
                                    </v-col>
                                    <v-col md="12" class="py-0" align="center">
                                        <!--<Success-Button style="width:130px" class="mb-2 center-block" title="Save"
                                            @click.native="SaveProjectBudgetsDetails('F')" v-bind:disabled="vmFinanceUsers.length==0"></Success-Button>-->
                                        <v-btn id="AutoTest_IP26" @click.native="SaveProjectBudgetsDetails('F')" v-bind:disabled="vmFinanceUsers.length==0"> Save</v-btn>
                                    </v-col>
                                </v-row>
                            </v-card-actions>
                        </v-card>
                        <!--_______________________________________-->
                    </v-menu>
                    <!--<v-snackbar v-model="AddFinUserSuccessSnackbar" top="" color="primary">
            {{Msg}}
        </v-snackbar>-->
                    <!--Added Fin User Details-->

                    <div class="mt-1" v-if="lstBudgetFinUserInfo.length>0">
                        <span class="small-text mr-1 black--text">
                            <v-icon size="12" color="#000">mdi-account</v-icon> Finance User
                        </span>
                        <v-avatar v-if="lstBudgetFinUserInfo.length>=1 " size="20" class="team-avatar">
                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Finuser name" />
                        </v-avatar>
                        <v-avatar v-if="lstBudgetFinUserInfo.length>=2 " size="20" class="team-avatar" color="pink">
                            <span class="white--text headline">{{ lstBudgetFinUserInfo[0].UserName.charAt(0) }}</span>
                        </v-avatar>
                        <v-avatar v-if="lstBudgetFinUserInfo.length>=3 " size="20">
                            <span> +{{ lstBudgetFinUserInfo.length-2 }}</span>
                        </v-avatar>
                    </div>

                    <!--____________________________-->
                    <!--Add Donors Pop up-->

                    <v-menu max-width="400" absolute="" v-model="AddDonorPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                        <template v-slot:activator="{ on }">
                            <v-btn id="AutoTest_IP27" v-on="on" max-width="170px" min-width="170px" class="mt-4 success-btn btn-sm" depressed="" dark="" height="30"
                                   @click="GetProjectUsers('Donor')" v-if="lstBudgetDonorUserInfo.length==0">
                                Add Donors
                            </v-btn>
                        </template>
                        <v-stepper class="elevation-0" v-model="AssignDonarSteps">
                            <v-stepper-items>
                                <v-stepper-content class="px-1 py-0" step="1">
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Assign Donors
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP28" text="" height="25" min-width="25" max-width="25" color="#707070" @click="vmlstDonors=[],AddDonorPopUp=false">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="max-height:40vh" class="v-scrolling-div">
                                            <v-data-table :headers="AddDonorTableHeaders"
                                                          :items="lstDonors"
                                                          show-select
                                                          item-key="UserID"
                                                          sort-by="UserName"
                                                          v-model="vmlstDonors"
                                                          hide-default-footer
                                                          hide-default-header>

                                                <template v-slot:item.UserName="{item}">
                                                    <v-avatar size="27" left class="my-0 px-0">
                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                    </v-avatar>
                                                    {{item.UserName}}
                                                </template>
                                                <template v-slot:no-data>
                                                    <v-alert :value="true" color="error" icon="warning">
                                                        Sorry, nothing to display here :(
                                                    </v-alert>
                                                </template>
                                            </v-data-table>
                                        </v-card-text>
                                        <v-row justify="center">
                                            <v-col md="12">
                                                <v-divider></v-divider>
                                            </v-col>
                                            <v-col md="6 pt-2">
                                                <!--<Success-Button style="width:100%" class="mb-2 center-block" title="Next" @click.native="AssignDonarSteps=2"></Success-Button>-->
                                                <v-btn id="AutoTest_IP29" style="width:100%" class="mb-2 center-block" @click.native="AssignDonorStep2()" v-bind:disabled="vmlstDonors.length==0"> Next</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-card>
                                </v-stepper-content>
                                <v-stepper-content class="px-1 py-0" step="2">
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Add Budget & Link Accounts
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP30" text="" height="25" min-width="25" max-width="25" color="#707070" @click="vmlstDonors=[],vmDonors=[],AddDonorPopUp=false,AssignDonarSteps=1">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="max-height:40vh" class="v-scrolling-div">
                                            <v-data-table :headers="AddDonorTableHeaders"
                                                          :items="vmlstDonors"
                                                          :single-expand="singleExpand"
                                                          :expanded.sync="expanded"
                                                          item-key="UserID"
                                                          sort-by="UserName"
                                                          v-model="vmDonors"
                                                          show-expand
                                                          hide-default-header
                                                          hide-default-footer>

                                                <template v-slot:item.UserName="{item}">

                                                    <v-avatar size="27" left>
                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                    </v-avatar>
                                                    {{item.UserName}}
                                                </template>

                                                <template v-slot:expanded-item="{ headers, item }">
                                                    <td id="AutoTest_IP31" :colspan="headers.length" @click="GetDonorDetails(item)">
                                                        <v-row>
                                                            <v-col md="12">
                                                                Add budget
                                                                <v-icon color="red" size="10" class="float-right" v-if="item.DonorBudgetAmount=='0'">mdi-star</v-icon>
                                                                <v-text-field class="mt-2" dense="" type="number" full-width outlined hide-details v-model="item.DonorBudgetAmount"
                                                                              onkeydown="javascript: return event.keyCode == 69 ? false : true" onKeyPress="if(this.value.length==10) return false;" min="0"
                                                                              required @change="AddBudgetAmt(item.UserID,item.DonorBudgetAmount)">
                                                                    {{item.DonorBudgetAmount}}
                                                                </v-text-field>
                                                                <v-chip label small outlined dark color="red" v-if="item.DonorBudgetAmount=='0' || errorCount>1 ">
                                                                    {{errorMsg}}
                                                                </v-chip>
                                                            </v-col>
                                                            <v-col md="12">
                                                                Link Bank Account
                                                                <v-select class="mt-1" hide-details="" dense="" :items="lstDonorAccount"
                                                                          item-text="AccountName" return-object
                                                                          item-value="AccountId" required
                                                                          label="Select Account" outlined="" v-model="vmDonorAccount" @change="AddAccount(item.UserID)">
                                                                </v-select>

                                                            </v-col>
                                                        </v-row>
                                                    </td>
                                                </template>
                                                <template v-slot:no-data>
                                                    <v-alert :value="true" color="error" icon="warning">
                                                        Sorry, nothing to display here :(
                                                    </v-alert>
                                                </template>
                                            </v-data-table>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-action>
                                            <v-row justify="center">
                                                <v-col md="12">
                                                    <v-divider></v-divider>
                                                </v-col>
                                                <v-col md="6 pt-2">
                                                    <!--<Success-Button style="width:100%" class="mb-2 center-block" title="Save" @click.native="SaveProjectBudgetsDetails('D')"
                                                        v-bind:disabled="vmDonors.length==0"></Success-Button>-->
                                                    <v-btn id="AutoTest_IP32" style="width:100%" class="mb-2 center-block" @click.native="SaveProjectBudgetsDetails('D')"
                                                           v-bind:disabled="!isValidBudget">Save</v-btn>
                                                </v-col>
                                            </v-row>
                                        </v-card-action>
                                    </v-card>
                                </v-stepper-content>
                            </v-stepper-items>
                        </v-stepper>
                    </v-menu>

                    <!--Added Donor Details-->
                    <div class="mt-1" v-if="lstBudgetDonorUserInfo.length>0">
                        <span class="small-text mr-1 black--text">
                            <v-icon size="12" color="#000">mdi-account</v-icon> Donors
                        </span>
                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=1 " size="20" class="team-avatar">
                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Donor name" />
                        </v-avatar>
                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=2 " size="20" class="team-avatar" color="pink">
                            <span class="white--text headline">{{ lstBudgetDonorUserInfo[0].UserName.charAt(0) }}</span>
                        </v-avatar>
                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=3 " size="20">
                            <span> +{{ lstBudgetDonorUserInfo.length-2 }}</span>
                        </v-avatar>
                    </div>

                    <!--____________________________-->

                    <v-snackbar v-model="snackbarAlertMsg" top="" color="primary">
                        {{Msg}}
                    </v-snackbar>

                </v-col>
            </v-row>
        </v-card-text>
    </v-card>
</template>
<script>
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import SuccessButtonSm from '@/components/success-button-sm.vue'
    import SuccessButton from '@/components/success-button.vue'
    import DoughnutChart from '@/components/DoughnutChart.vue'

    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then(mod => {
                objProject = new mod.ManageProjectFinance();
            })
        ]);
    };

    export default {
        data() {
            return {

                //Chart Data
                //BudgetChartData:[],
                 BudgetChartData: {
                    labels: [],
                    datasets: [{ data: [], backgroundColor: []}],
                },

                BudgetChartData1: {
                    labels: ['Donor 1 : Rs. 60000', 'Donor 2 : Rs. 40000',],
                    datasets: [{
                        data: [60000, 40000],

                        backgroundColor: [
                            '#388E3C',
                            '#FFA200',
                        ],
                    }],
                },

                //Chart Options
                BudgetChartOptions: {
                    legend: {
                        position: 'bottom',
                        fullWidth: false,
                        align: 'start',
                    }
                },


                AssignFinUserPopUp: false,
                AddDonorPopUp: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AddDonorPopUp: false,
                AssignDonorSteps: 1,
                SelectBankAccountItems: ['Sample Account 1', 'Sample Account 2'],
                WorkspaceID: 0,
                snackbarAlertMsg: false,

                budgetAmt: 0,
                GroupID: 0,
                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],
                lstuserTableheaders: [{ text: "Name", align: "left", value: "UserName" }],
                AddDonorTableHeaders: [
                    { text: "Name", align: "left", value: "UserName" },
                    { text: '', value: 'data-table-expand' },
                ],
                expanded: [],
                singleExpand: false,
                singleSelect: false,
                lstFinanceUsers: [],
                vmFinanceUsers: [],
                AssignDonarSteps: 1,
                lstDonors: [],
                vmlstDonors: [],
                vmDonors: [],
                lstDonorAccount: [],
                ChkFinNotify: true,
                Msg: "",
                txtDonorAmt: 0,
                vmDonorAccount: [],
                errorMsg: " * Please add Budget Amount",
                isValidBudget: 0,
                InitiativeID: 0,
                ProjectID: 0,
            }
        },
        components: {
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Success-Button-Sm': SuccessButtonSm,
            'Success-Button': SuccessButton,
            'Doughnut-Chart': DoughnutChart,
        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            await importscript();
            await this.GetProjectBudgetDetails();
            this.FillBudgetData();
        },
        methods: {
            FillBudgetData(){            
            this.BudgetChartData=this.BudgetChartData1;
            },
            async GetProjectBudgetDetails() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objProject.GetProjectBudgetDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.budgetAmt = result.data[0].Budget;
                        this.GroupID = result.data[0].GroupId;
                        this.lstBudgetFinUserInfo = result.data.filter(x => x.UserType == 'F');
                        this.lstBudgetDonorUserInfo = result.data.filter(x => x.UserType == 'D');
                    }

                }
            },
            async GetProjectUsers(Type) {
                var data = new FormData();
                data.append("GroupID", this.GroupID);
                data.append("Type", Type)
                var result = await objProject.GetProjectUsers(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        if (Type == 'Finance')
                            this.lstFinanceUsers = result.data;
                        else if (Type == 'Donor')
                            this.lstDonors = result.data;
                    }

                }
            },
            async SaveProjectBudgetsDetails(UserType) {
                var usr = "";
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                if (UserType == 'F') {
                    data.append("ProjectUsers", JSON.stringify(this.vmFinanceUsers));
                    usr = "Finance user";
                }
                else {
                    data.append("ProjectUsers", JSON.stringify(this.vmlstDonors));
                    usr = "Donor user";
                }
                data.append("chkNotify", this.ChkFinNotify);
                var result = await objProject.SaveProjectBudgetsDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        this.Msg = usr + " added successfully";
                    else
                        this.Msg = "Failed in adding " + usr + " ...";
                }
                this.AssignFinUserPopUp = false;
                this.AddDonorPopUp = false;
                this.snackbarAlertMsg = true;
                this.GetProjectBudgetDetails();
                this.UpdateChart(this.vmlstDonors);

            },
            UpdateChart(lstDonors) {               
                var lstlabel = [];
                var lstAmt = [];
                var lstdataset = [];
                var lstChartData = [];

                for (var i = 0; i < this.vmlstDonors.length; i++) {
                    lstlabel.push("Donor " + (i + 1) + ": Rs. " + this.vmlstDonors[i].DonorBudgetAmount);
                    lstAmt.push(parseInt(this.vmlstDonors[i].DonorBudgetAmount));
                }
                lstdataset.push({ data: lstAmt, backgroundColor: ['#388E3C', '#FFA200'] });
                lstChartData.push({ labels: lstlabel, datasets: lstdataset });
                this.BudgetChartData = lstChartData;

                //this.BudgetChartData.labels = lstlabel;
                //this.BudgetChartData.datasets = lstdataset;


            },
            AssignDonorStep2() {

                var errCount = 0;
                for (var i = 0; i < this.vmlstDonors.length; i++)
                    if ((this.vmlstDonors[i].DonorBudgetAmount == 0))
                        errCount++;
                if (errCount > 0) {
                    this.isValidBudget = 0;
                    this.errorMsg = " * Please add Budget Amount";
                }
                else {
                    this.isValidBudget = 1;
                    this.errorMsg = "";
                }

                this.AssignDonarSteps = 2;
            },
            async GetDonorDetails(userItem) {

                this.vmDonorAccount.AccountId = this.lstDonors.filter(x => x.UserID == userItem.UserID)[0].AccountId;
                this.vmDonorAccount.AccountName = this.lstDonors.filter(x => x.UserID == userItem.UserID)[0].AccountName;

                //--- Get Account Details ----
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("UserID", userItem.UserID);
                var result = await objProject.GetDonorAccountDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.lstDonorAccount = result.data;
                }
            },
            AddBudgetAmt(userID, Amt) {

                this.vmlstDonors.filter(x => x.UserID == userID)[0].IsBudgetAmtAdded = true;
                this.errorMsg = "";

                var i = 0; var totDonorsAmt = 0;
                var lstUsers = this.vmlstDonors.filter(x => x.UserID != userID);

                for (i = 0; i < lstUsers.length; i++) {
                    if (lstUsers[i].IsBudgetAmtAdded) {
                        totDonorsAmt = totDonorsAmt + parseInt(lstUsers[i].DonorBudgetAmount);
                        if (lstUsers[i].DonorBudgetAmount > 0) {
                            lstUsers.splice(i, 1);
                            lstUsers = [...lstUsers];
                            i--;
                        }
                    }
                }
                if (parseInt(this.budgetAmt) >= (totDonorsAmt + Amt)) {
                    var balAmt = (parseInt(this.budgetAmt) - (totDonorsAmt + Amt)) / lstUsers.length;
                    for (i = 0; i < lstUsers.length; i++)
                        this.vmlstDonors.filter(x => x.UserID == lstUsers[i].UserID)[0].DonorBudgetAmount = balAmt;
                    this.isValidBudget = 1;
                }
                else {
                    this.isValidBudget = 0;
                    this.errorMsg = "Total Donor Budget Amount should not be greater than " + this.budgetAmt;
                    this.vmlstDonors.filter(x => x.UserID == userID)[0].DonorBudgetAmount = 0;
                }

            },
            AddAccount(userID) {
                this.lstDonors.filter(x => x.UserID == userID)[0].AccountId = this.vmDonorAccount.AccountId;
                this.lstDonors.filter(x => x.UserID == userID)[0].AccountName = this.vmDonorAccount.AccountName;
            },

        }
    }
</script>