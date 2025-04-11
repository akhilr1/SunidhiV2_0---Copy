<template>
    <v-card min-height="70vh">
        <v-card-title style="height:60px">
            <h5 class="text-left page-head">
                <v-avatar color="white" size="20" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary">mdi-file-document-box-plus-outline</v-icon>
                </v-avatar>
                My Ventures
            </h5>
            <v-spacer></v-spacer>
            <!-- Filter combo-->
            <v-select style="max-width:180px;" class="mr-4" hide-selected="" hide-details="" dense=""
                      :items="MyVentureFilterList"
                      v-model="filterMyVenture"
                      label="Select" @change="GetProjectListOfFundedAndWishList()"
                      outlined="">
            </v-select>
            <!--Bulk Action -->
            <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess"> 
                <template v-slot:activator="{ on }">
                    <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>
                            mdi-chevron-down
                        </v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">

                    <v-list-item @click.stop="type=1,fnBulkRemoveClick()">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Remove
                        </v-list-item-content>
                    </v-list-item>
                 
                </v-list>

            </v-menu>
            <!--Search-->
            <v-text-field label="Search" class="mr-4 table-search" v-model="searchProject"
                          dense prepend-inner-icon="mdi-magnify"
                          outlined hide-details></v-text-field>
            <!--Filter button-->
            <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="filterMenu" v-if="filterMyVenture=='Funded Project'">
                <template v-slot:activator="{ on }">
                    <v-btn v-on="on" height="39" width="122" outlined dense class="outlined-btn-dark" >
                        <v-icon>mdi-filter-outline</v-icon>
                        Filters
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                    <h4 class="title">Filter by :</h4>
                    <p class="title">Fund Status</p>
                    <v-checkbox color="success-check-box" input-value="true"  v-model="checkboxFullyFunded" label="Fully Funded"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" v-model="checkboxPartiallyFunded" label="Partially Funded"></v-checkbox>
                    <v-checkbox color="success-check-box" input-value="true" v-model="checkboxNotFunded" label="Not Funded"></v-checkbox>
                    <div class="d-inline-flex">
                        <v-btn id="AutoTest_V42" height="32" width="35" block color="success-btn" dark class="elevation-0 mr-1" @click="fnClickFilterDone()">
                            Apply
                        </v-btn>
                        <v-btn id="AutoTest_V43" height="32" width="35" block color="grey" outlined="" class="elevation-0" @click="fnResetFilter()">
                            Reset
                        </v-btn>
                    </div>

                </v-list>
            </v-menu>
        </v-card-title>
            <!--Main Body-->
        <v-row>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-data-table :headers="ProjectTableheaders"
                              :items="FilterProjectTableItems"
                              v-model="selectedProjectID"
                              show-select=""
                              fixed-header
                              height="60vh"
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
                            <v-list-item class="my-0 px-0" link :to="{ name: 'ManageVentureDetails', query: { WsID: WorkspaceID, Access: RWAccess ,InitID : item.InitiativeId, ProjID : item.ProjectId, VentureType :'MyVenture',FundUtilized : item.FundUtilized} }">
                                <div class="pa-0 text-limit" style="width:130px">
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
                        <div class="pa-0 text-limit" style="width:80px">
                            {{item.Code }}
                        </div>
                    </template>
                    <template v-slot:item.Budget="{ item }">
                        <div class="pa-0 text-limit" style="width:90px">
                            {{item.Budget }}
                        </div>
                    </template>
                    <template v-slot:item.FundedAmount="{ item }">
                        <div class="pa-0 text-limit" style="width:100px">
                            {{item.FundedAmount }}
                        </div>
                    </template>
                    <template v-slot:item.Initiative="{ item }">
                        <div class="pa-0 text-limit" style="width:100px">
                            {{item.Initiative}}
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
                    <template v-slot:item.Actions="{item}">
                        <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                    <v-icon>
                                        mdi-dots-vertical
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light width="auto" class="action-button-list ">
                                <v-list-item id="AutoTest_V44" v-if="filterMyVenture== 'Funded Project'" @click.stop="GetProjectID(item.ProjectId,item.ProjectName,item.Budget,item.FundedAmount),dlgReCommitFund = true">
                                    <v-list-item-icon>
                                        <v-icon>mdi-check-circle-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Recommit Fund
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item id="AutoTest_V45" v-else @click.stop="GetProjectID(item.ProjectId,item.ProjectName,item.Budget,item.FundedAmount), dlgCommitFund= true">
                                    <v-list-item-icon>
                                        <v-icon>mdi-check-circle-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Commit Fund
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item id="AutoTest_V45" @click.stop="GetProjectID(item.ProjectId,item.ProjectName,item.Budget,item.FundedAmount), type=2, RemoveBulkVentureDailog= true">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                        </v-menu>
                    </template>

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
                <!-- Recommit fund -->
                <v-dialog v-model="dlgReCommitFund" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-check-circle-outline</v-icon>
                            </v-avatar>
                            Recommit Fund
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_V46" text="" @click="dlgReCommitFund=false,CommitedFund=''">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-card-text class="pt-0">
                            <v-container fluid class="pt-4">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <v-container fluid class="pt-0">
                                            <v-row>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <h4 class="text-center heading-3">Enter the amount to recommit the fund.</h4>
                                                </v-col>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <p class="text-center black--text">Previously commited fund : <span style="color:#3374B9">Rs. {{FundedAmount}}</span> </p>

                                                </v-col>

                                                <v-row :justify="justifyCenter" no-gutters>
                                                    <v-col md="6">
                                                        <v-text-field label="Enter Amount" class="mr-2"
                                                                      type="number"
                                                                      v-model="CommitedFund"
                                                                      @change="AmtFundedToPrjt(CommitedFund)"
                                                                      outlined></v-text-field>
                                                    </v-col>
                                                </v-row>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                    <Secondary-Button id="AutoTest_V47" class="btn-122x36 mr-4" title="Confirm" @click.native="UpdateCommitFund(),dlgReCommitFund = false" v-bind:disabled="CommitedFund <= 0"></Secondary-Button>
                                                    <Default-Button-Outlined id="AutoTest_V48" class="btn-122x36" title="Cancel" @click.native="dlgReCommitFund=false,CommitedFund=''"></Default-Button-Outlined>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                    </v-card>
                </v-dialog>


                <!-- Commit fund -->
                <v-dialog v-model="dlgCommitFund" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-check-circle-outline</v-icon>
                            </v-avatar>
                            Commit Fund
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_V49" text="" @click="dlgCommitFund=false,CommitedFund=''">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-card-text class="pt-0">
                            <v-container fluid class="pt-4">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <v-container fluid class="pt-0">
                                            <v-row>

                                                <v-col cols="12" md="12" class="pt-0">
                                                    <h4 class="text-center heading-3">Enter the amount to commit the fund.</h4>
                                                </v-col>

                                                <v-row :justify="justifyCenter">
                                                    <v-col md="6">
                                                        <v-text-field label="Enter Amount" class="mr-2"
                                                                      type="number"
                                                                      v-model="CommitedFund"
                                                                      @change="AmtFundedToPrjt(CommitedFund)"
                                                                      outlined></v-text-field>
                                                    </v-col>
                                                </v-row>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                    <v-btn id="AutoTest_V50" color="secondary elevation-0" class="mr-2" v-bind:disabled="CommitedFund <= 0" @click="InsertCommitFund(),dlgCommitFund = false">Confirm</v-btn>
                                                    <v-btn id="AutoTest_V51" outlined color="grey" @click="dlgCommitFund=false,CommitedFund=''">Cancel</v-btn>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-card-text>
                    </v-card>
                </v-dialog>

                <!-- Wishlist dailog-->
                <!--<v-dialog v-model="dlgWhishlist" width="800">
        <v-card>
            <v-card-title primary-title class="page-head py-4">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="20" color="primary">mdi-star</v-icon>
                </v-avatar>
                Whishlist
                <v-spacer></v-spacer>
                <v-btn id="AutoTest_V52" text="" @click="dlgWhishlist=false">
                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text class="pt-0">
                <v-container fluid class="pt-4">
                    <v-row no-gutters>
                        <v-col cols="12" md="12" class="pt-0">
                            <v-container fluid class="pt-0">
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center heading-3">Are you sure you want to wishlist this Project ?</h4>
                                    </v-col>
                                    <v-col>
                                        <h4 class="text-center heading-3">
                                            <v-icon class="tmr-2" color="#707070" size="25">mdi-folder</v-icon>
                                            Sample
                                        </h4>
                                    </v-col>

                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                        <v-btn id="AutoTest_V53" color="secondary elevation-0" class="mr-2" @click="dlgWhishlist = false, WhishlistSuccessSnackbar = true">Deploy</v-btn>
                                        <v-btn id="AutoTest_V54" outlined color="grey" @click="dlgWhishlist=false">Cancel</v-btn>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-col>
                    </v-row>
                </v-container>
            </v-card-text>
        </v-card>
    </v-dialog>-->
                <!-- Remove dailog-->
                <v-dialog v-model="RemoveBulkVentureDailog" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                            </v-avatar>
                            Remove Project
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_IP69" text="" @click="RemoveBulkVentureDailog=false">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-col cols="12" md="12" class="pt-0">
                            <v-container fluid class="pt-0">
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0" v-if="type==1">
                                        <h4 class="text-center heading-3">Are you sure you want to remove this projects ?</h4>
                                    </v-col>

                                    <v-col cols="12" md="12" class="pt-0" v-else>
                                        <h4 class="text-center heading-3">
                                            Are you sure you want to remove this project ?
                                        </h4>
                                    </v-col>
                                    <v-col v-if="type==2">
                                        <h4 class="text-center heading-3">
                                            <v-icon class="tmr-2" color="#707070" size="25">mdi-folder</v-icon>
                                            {{ProjectName}}
                                        </h4>
                                    </v-col>

                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                        <Secondary-Button id="AutoTest_IP70" class="btn-122x36 mr-4" title="Remove" @click.native="DeleteVenture(type)"></Secondary-Button>
                                        <Default-Button-Outlined id="AutoTest_IP71" class="btn-122x36" title="Cancel" @click.native="RemoveBulkVentureDailog=false, type=1"></Default-Button-Outlined>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-col>
                    </v-card>
                </v-dialog>
                <!-- Snackbar -->
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
            </v-col>
            </v-row>
        
    </v-card>
</template>
<script type="text/javascript">

    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

    var objVenture;
    async function importscript() {
        return Promise.all([
            import("../../BL/Venture.js").then(mod => {
                objVenture = new mod.Venture();
            })
        ]);
    }



    export default {
        data() {
            return {
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
                    { text: "Amt Funded", value: "FundedAmount", },
                    { text: "Initiative", value: "Initiative" },
                    { text: "Actions", value: "Actions", sortable: false,align:"center" }

                ],
                ProjectTableItems: [],
                FilterProjectTableItems: [],
                selectedProjectID: [],
                searchProject: "",
                ProjectId: "",
                type: 1,
                ProjectName: "",
                Budget: 0,
                FundedAmount:0,
                State: "",
                dlgSetasTemplate: false,
                
                SetasTempFileInput: null,

                dlgCommitFund: false,
                dlgReCommitFund: false,
                //dlgWhishlist : false,
                RemoveBulkVentureDailog: false,
                CommitedFund: "",
                RemoveBulkVentureDailog: false,
                
                filterMenu: false,
                MyVentureFilterList: ['Funded Project', 'Wishlist'],
                filterMyVenture: 'Funded Project',
                checkboxFullyFunded: false,
                checkboxPartiallyFunded: false,
                checkboxNotFunded: false,
                
                screenAccess: 0,
                RWAccess: 0,
            };
        },

        mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await this.GetProjectListOfFundedAndWishList ();
        },
        methods: {

            /// <summary>
            /// fnBulkRemoveClick :- Function used to Remove selected projects.
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkRemoveClick() {
                if (this.selectedProjectID.length > 0) {
                    this.RemoveBulkVentureDailog = true;
                } else {
                    this.Msg = ("Please select the projects");
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            /// GetProjectListOfFundedAndWishList :- Function used to fetch FundedList and WishList of Projects based on ProjectType and WorkspaceID.
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectListOfFundedAndWishList() {
                var data = new FormData();
                data.append("ProjectType", this.filterMyVenture);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objVenture.GetProjectListOfFundedAndWishList(data);
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
            /// DeleteVenture :- Function used to delete venture
            /// </summary>
            /// <param name="Type"></param> :-Type 1 is bulk and Type 2 single removal
            /// <returns> </returns>
            async DeleteVenture(Type) {
                var data = new FormData();
                if (Type == 1) {
                    data.append("ProjectID", JSON.stringify(this.selectedProjectID));
                    data.append("Type", 1);
                }
                else {
                    data.append("ProjectID", this.ProjectId);
                    data.append("Type", 2);
                }

                var result = await objVenture.DeleteVenture(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "success") {
                        this.Msg = "Successfully deleted";
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = "Failed to delete";
                        this.errorSnackbarMsg = true;
                    }
                    
                }
                this.GetProjectListOfFundedAndWishList();
                this.ProjectId = 0;
                this.type = 1;
                this.RemoveBulkVentureDailog = false;

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
                        this.CommitedFund = '';
                        this.Msg = "Fund commited successfully";
                        this.successSnackbarMsg = true;
                        this.GetProjectListOfFundedAndWishList();
                    } else {

                        this.Msg = ("commit fund adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },

            /// <summary>
            /// UpdateCommitFund :- Function used to Update Commit Fund
            /// </summary>
            /// <param name=""></param> 
            /// <returns> </returns>
            async UpdateCommitFund() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("ProjectType", 'Funded Project');
                data.append("Amount", this.CommitedFund);
                var result = await objVenture.UpdateCommitFund(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status = "Success") {
                       
                        this.GetProjectListOfFundedAndWishList();
                        this.CommitedFund = "";
                        this.Msg = "Fund Recommited successfully";
                        this.successSnackbarMsg = true;
                    } else {

                        this.Msg = ("commit fund adding Failed ");
                        this.errorSnackbarMsg = true;
                    }
            },

            /// <summary>
            /// fnClickFilterDone :- Function used to Filter Ventures
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
            /// fnResetFilter :- Function used to Reset Ventures
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
            /// fnResetFilter :- Function used to add amount funded to project
            /// </summary>
            /// <param name=""></param> 
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
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
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

