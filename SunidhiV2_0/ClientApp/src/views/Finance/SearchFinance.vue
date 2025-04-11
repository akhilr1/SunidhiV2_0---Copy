<template>
    <div>
        <v-card min-height="70vh">
            <v-card-title style="height:60px">
                <h5 class="text-left page-head">
                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                    </v-avatar>
                    Finance
                </h5>
                <v-spacer></v-spacer>
                <!--Search-->
                <v-text-field label="Search" class="mr-2" v-model="searchProject"
                              dense prepend-inner-icon="mdi-magnify"
                              outlined hide-details style="max-width:300px"></v-text-field>
                <!--Filter button-->
                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="filterMenu"  >
                    <template v-slot:activator="{ on }">
                        <v-btn id="AutoTest_F264" v-on="on" height="38" width="122" outlined dense class="outlined-btn-dark mr-2" @click="">
                            <v-icon>mdi-filter-outline</v-icon>
                            Filters
                        </v-btn>
                    </template>
                    <!--<v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                    <h4 class="title">Filter by :</h4>



                    <p class="title">Fund Status</p>
                    <v-checkbox input-value="true" v-model="checkboxFullyFunded" label="Fully Funded"></v-checkbox>
                    <v-checkbox input-value="true" v-model="checkboxPartiallyFunded" label="Partially Funded"></v-checkbox>
                    <v-checkbox input-value="true" v-model="checkboxNotFunded" label="Not Funded"></v-checkbox>


                    <div class="d-inline-flex">
                        <v-btn height="32" width="35" block color="success-btn" dark class="elevation-0 mr-1" @click="fnClickFilterDone()">
                            Apply
                        </v-btn>
                        <v-btn height="32" width="35" block color="grey" outlined="" class="elevation-0" @click="fnResetFilter()">
                            Reset
                        </v-btn>
                    </div>

                </v-list>-->
                </v-menu>
            </v-card-title>
            <!--Main Body-->
            <v-row>
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="ProjectTableheaders"
                                  :items="FilterProjectTableItems"
                                  v-model="selectedProjectID"
                                  show-select
                                  fixed-header
                                  height="70vh"
                                  item-key="ProjectId"
                                  :search="searchProject"
                                  class="elevation-0"
                                  :footer-props="{
                                  itemsPerPageOptions:[10,50,100,-1]
                                  }"
                                  :items-per-page="10">

                        <!--Projects list column Name-->
                        <template v-slot:item.ProjectName="{item}">
                            <v-list class="py-0 table-v-list">
                                <v-list-item class="my-0 px-0" link :to="{ name: 'FinanceBudgetAndFunds',query: {WsID: WorkspaceID,Access:RWAccess ,InitID : item.InitiativeId ,ProjID : item.ProjectId,}}">
                                    <div class="pa-0 text-limit" style="width:140px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-folder</v-icon>
                                        </v-avatar>
                                        {{ item.ProjectName }}
                                    </div>
                                </v-list-item>
                            </v-list>
                        </template>

                        <!-- DateTime-->
                        <template v-slot:item.StartDate="{ item }">
                            <div class="pa-0 text-limit" style="width:90px">{{new Date(item.StartDate).toLocaleDateString('en-GB')}}</div>
                        </template>
                        <template v-slot:item.EndDate="{ item }">
                            <div class="pa-0 text-limit" style="width:90px">{{new Date(item.EndDate).toLocaleDateString('en-GB')}}</div>
                        </template>
                        <template v-slot:item.Progress="{ item }">
                            <div class="pa-0 text-limit" style="width:80px">
                                <v-progress-circular :rotate="360"
                                                     :size="40"
                                                     :width="5"
                                                     :value=item.FundUtilized
                                                     color="secondary">
                                    {{ item.FundUtilized }}
                                </v-progress-circular>
                                {{ item.FundUtilized }}%
                            </div>
                        </template>
                        <template v-slot:item.Code="{ item }">
                            <div class="pa-0 text-limit" style="width:100px">
                                {{item.Code }}
                            </div>
                        </template>
                        <template v-slot:item.Budget="{ item }">
                            <div class="pa-0 text-limit" style="width:100px">
                                {{item.Budget }}
                            </div>
                        </template>
                        <template v-slot:item.ProjectStatus="{ item }">
                            <div class="pa-0 text-limit" style="width:80px">
                                {{item.ProjectStatus }}
                            </div>
                        </template>

                        <!-- Fund Status-->
                        <template v-slot:item.FundStatus="{item}">
                            <div v-if="item.FundedAmount == item.Budget && item.Budget != 0" class="pa-0 text-limit" style="width:100px">
                                <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                    <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                </v-avatar>
                                Fully Funded
                            </div>
                            <div v-if="item.FundedAmount < item.Budget && item.FundedAmount != 0" class="pa-0 text-limit" style="width:100px">
                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                    <v-icon size="15" color="#FCC735">mdi-minus</v-icon>
                                </v-avatar>
                                Partially Funded
                            </div>
                            <div v-if="item.FundedAmount == 0 " class="pa-0 text-limit" style="width:100px">
                                <v-avatar color="#FDE4E4" size="20" class="mr-1">
                                    <v-icon size="15" color="#F24646">mdi-close-circle-outline</v-icon>
                                </v-avatar>
                                Not Funded
                            </div>
                        </template>

                        <!-- Venture project Actions-->
                        <!--<template v-slot:item.Actions="{item}">
        <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
            <template v-slot:activator="{ on }">
                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                    <v-icon>
                        mdi-dots-vertical
                    </v-icon>
                </v-btn>
            </template>

        </v-menu>
    </template>-->
                        <!--if no data-->
                        <template v-slot:no-data>
                            <v-row class="justify-center" v-show="FilterProjectTableItems.length == 0">
                                <v-col class="text-center" md="12">
                                    <p class="mt-12 pt-12">
                                        <img src="..\..\..\src\assets\EmptyPlaceholders\search.svg" height="150" width="150" />
                                    </p>
                                    <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                                </v-col>
                            </v-row>
                        </template>
                    </v-data-table>


                </v-col>
            </v-row>

        </v-card>
        <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </div>
</template>
<script type="text/javascript">

    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'

    var objFinance;
    async function importscript() {
        return Promise.all([
            import("../../BL/Finance.js").then(mod => {
                objFinance = new mod.Finance();
            })
        ]);
    }


    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeId: 0,
                ProjectTableheaders: [
                    { text: "Name", align: "left", value: "ProjectName" },
                    { text: "Code", value: "Code" },
                    { text: "Start Date", value: "StartDate" },
                    { text: "End Date", value: "EndDate", },
                    { text: "Budget", value: "Budget", },
                    { text: "Progress", value: "Progress" },
                    { text: "Fund Status", value: "FundStatus", },
                    { text: "Status", value: "ProjectStatus", },
                    //{ text: "Initiative", value: "Initiative",},

                ],
                ProjectTableItems: [],
                FilterProjectTableItems: [],
                selectedProjectID: [],
                searchProject: "",
                ProjectId: "",
                ProjectName: "",
                Budget: 0,
                FundedAmount: 0,
                State: "",
                dlgSetasTemplate: false,
                
                SetasTempFileInput: null,

                dlgCommitFund: false,
                dlgWhishlist: false,
                dlgBulkWhishlist: false,
                CommitedFund: 0,
                RemoveBulkVentureDailog: false,
                
                filterMenu: false,
                checkboxFullyFunded: false,
                checkboxPartiallyFunded: false,
                checkboxNotFunded: false,
                
            };
        },

        mounted: async function () {
 
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            //this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await this.GetProjectListOfFinance();
        },
        methods: {

            /// <summary>
            /// GetProjectListOfFinance :- Function used to fetch Project list of Finance based on WorkspaceID.
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectListOfFinance() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objFinance.GetProjectListOfFinance(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.ProjectTableItems = result.data;
                        this.FilterProjectTableItems = result.data;
                    }
            },

            /// <summary>
            /// InsertCommitFund :- Function used to Insert Commit Fund
            /// </summary>
            /// <param name=""></param> 
            /// <returns> </returns>
            async InsertCommitFund() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("ProjectType", 'Funded Project');
                data.append("Amount", this.CommitedFund);
                var result = await objVenture.InsertCommitFund(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status = "Success") {
                        this.Msg = "Fund commited successfully";
                        this.successSnackbarMsg = true;
                        this.GetProjectListOfVenture();
                    } else {

                        this.Msg = ("commit fund adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },

            /// <summary>
            /// AddToWishlist :- Function used to add Project to WishList
            /// </summary>
            /// <param name=""></param> 
            /// <returns> </returns>
            async AddToWishlist(Type) {
                var data = new FormData();
                if (Type == 1) {
                    data.append("ProjectId", JSON.stringify(this.selectedProjectID));
                    data.append("ProjectType", 'Wishlist');
                    data.append("Amount", this.CommitedFund);
                    data.append("Type", 1);
                }
                else {
                    data.append("ProjectId", JSON.stringify(this.ProjectId));
                    data.append("ProjectType", 'Wishlist');
                    data.append("Amount", this.CommitedFund);
                    data.append("Type", 2);
                }
                var result = await objVenture.AddToWishlist(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status = "Success") {
                        this.Msg = "Project whishlisted successfully.";
                        this.successSnackbarMsg = true;
                        this.GetProjectListOfVenture();
                    } else {

                        this.Msg = ("wishlist adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },

            /// <summary>
            /// GetProjectID :- Function used to get ProjectID
            /// </summary>
            /// <param name="ProjectID","ProjectName","Budget","FundedAmount"></param> 
            /// <returns> </returns>
            async GetProjectID(ProjectID, ProjectName, Budget, FundedAmount) {
                this.ProjectId = ProjectID;
                this.ProjectName = ProjectName;
                this.Budget = Budget;
                this.FundedAmount = FundedAmount;

            },

            /// <summary>
            /// fnClickFilterDone :- Function used to Filter Fianace
            /// </summary>
            /// <param name=""></param> 
            /// <returns> </returns>
            fnClickFilterDone() {
                var templist = [];
                for (var j = 0; j < this.ProjectTableItems.length; j++) {
                    if (this.checkboxFullyFunded) {

                        if (this.ProjectTableItems[j].FundedAmount == this.ProjectTableItems[j].Budget && this.ProjectTableItems[j].Budget != 0) {
                            templist.push(this.ProjectTableItems[j]);

                        }
                    }
                    if (this.checkboxPartiallyFunded) {
                        if (this.ProjectTableItems[j].FundedAmount < this.ProjectTableItems[j].Budget && this.ProjectTableItems[j].FundedAmount != 0) {
                            templist.push(this.ProjectTableItems[j]);

                        }
                    }
                    if (this.checkboxNotFunded) {
                        if (this.ProjectTableItems[j].FundedAmount == 0) {
                            templist.push(this.ProjectTableItems[j]);
                        }
                    }
                }
                this.FilterProjectTableItems = templist;
                this.filterMenu = false;
            },

            /// <summary>
            /// fnResetFilter :- Function used to Reset Filter
            /// </summary>
            /// <param name=""></param> 
            /// <returns> </returns>
            fnResetFilter() {
                this.checkboxFullyFunded = false;
                this.checkboxPartiallyFunded = false;
                this.checkboxNotFunded = false;
                this.FilterProjectTableItems = this.ProjectTableItems;
                this.filterMenu = false;


            },

            /// <summary>
            /// AmtFundedToPrjt :- Function used to add amount funded to project
            /// </summary>
            /// <param name="Amt"></param> 
            /// <returns> </returns>
            AmtFundedToPrjt(Amt) {
                if (parseInt(this.Budget) < ((parseInt(Amt) + parseInt(this.FundedAmount)))) {
                   
                    this.CommitedFund = "";
                    this.Msg = "Committed amount should not be greater than Budget Amount :" + this.Budget;
                    this.infoSnackbarMsg = true;
                }
            },

        },
        components: {
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
        },
    };
</script>
<style scoped>
    .text-limit {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

        .text-limit:hover {
            white-space: normal;
            overflow: visible;
        }
</style>

