<template>
    <v-row class="mx-4">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-col sm="12" md="6">
            <h3 class="create-project-steps-title text-center">Define Inputs</h3>
            <v-row>
                <v-col cols="12" align="center" class="pt-9">
                    <v-carousel cycle="" height="200px"
                                hide-delimiter-background
                                show-arrows-on-hover>
                        <v-carousel-item>
                            <img src="../../images/Create-Project-Step5-Vector-1.svg" class="create-project-corousal-vector" />
                        </v-carousel-item>
                    </v-carousel>
                </v-col>
                <v-col cols="12">
                    <p class="text-center">
                        Define your project budget and other<br>financials here.
                    </p>
                </v-col>
            </v-row>
        </v-col>

        <v-col sm="12" md="6">
            <v-row>
                <v-col md="auto">
                    <h6 class="heading-3">
                        <v-avatar size="30" color="#EDF6FF" class="mr-2">
                            <v-icon color="secondary" size="15">mdi-clipboard-list</v-icon>
                        </v-avatar> Inputs
                    </h6>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto">
                    <v-menu offset-y transition="scroll-y-transition">
                        <template v-slot:activator="{ on, attrs }">
                            <v-btn class="success-btn" depressed dark height="37" v-bind="attrs" v-on="on">
                                Add Inputs
                                <v-icon>
                                    mdi-chevron-down
                                </v-icon>
                            </v-btn>
                        </template>
                        <v-list dense>
                            <v-list-item @click="addDonorsFn()">
                                <v-list-item-content>
                                    Add Donars
                                </v-list-item-content>
                            </v-list-item>
                            <v-divider></v-divider>
                            <v-list-item @click="addPaymentTermsFn">
                                <v-list-item-content>
                                    Add Payment Terms
                                </v-list-item-content>
                            </v-list-item>
                            <v-divider></v-divider>
                            <v-list-item @click="addBudgetPlanFn()">
                                <v-list-item-content>
                                    Add Budget Plan
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="12" class="v-scrolling-div" style="max-height:40vh">
                    <v-form ref="Form"
                            v-model="valid"
                            @submit.prevent="fnSaveInputs">
                        <v-row class="pt-0">
                            <v-col md="12">
                                <h4 class="font-weight-medium">Project Budget</h4>
                                <v-row>
                                    <v-col cols="12">
                                        <v-text-field dense type="number"
                                                      label="Enter Amount"
                                                      :rules="[(value) => !!value || 'Required field']"
                                                      outlined v-model="ProjectBudget">
                                            <template v-if="ProjectBudget.length==0" v-slot:append>
                                                <span class="mandatory-star">*</span>
                                            </template>
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                            </v-col>


                            <!-- Add Donor -->
                            <v-col md="12" v-bind:style= "[addDonorItems.length>=1 ? {'display':'block'} : {'display':'none'}]">
                                <h4 class="font-weight-medium">Add Donors</h4>
                                <v-row v-for="(item,index) in addDonorItems" :key="item.id">
                                    <v-col>
                                        <v-select :items="Donors" item-text="Name"
                                                  item-value="ID" outlined hide-details dense label="Select User" v-model="item.UserID"></v-select>
                                    </v-col>
                                    <v-col>
                                        <v-text-field dense type="number"
                                                      label="Enter Amount"
                                                      v-model="item.DonorBudgetAmount"
                                                      outlined hide-details
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;"></v-text-field>
                                    </v-col>
                                    <v-col md="auto">
                                        <v-btn fab x-small depressed class="mr-2" max-width="20" @click="deleteDonorsFn(item)">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-col>

                            <!-- Add Paymnet Terms-->
                            <v-col md="12" v-bind:style= "[paymentTermsItems.length>=1 ? {'display':'block'} : {'display':'none'}]">
                                <h4 class="font-weight-medium">Payment Terms</h4>
                                <v-row v-for="item in paymentTermsItems" :key="item.id">
                                    <v-col md="3">
                                        <v-text-field dense
                                                      label="Enter Name"
                                                      outlined hide-details v-model="item.PaymentTermName"></v-text-field>
                                    </v-col>
                                    <v-col>
                                        <v-select v-model="item.Logic" :items="paymentTermsLogic" outlined hide-details dense label="Select Logic"></v-select>
                                    </v-col>
                                    <v-col>
                                        <!--if paymentTermsLogicModel is null-->
                                        <v-text-field dense v-if="item.Logic==null"
                                                      disabled
                                                      outlined hide-details></v-text-field>
                                        <!--if paymentTermsLogicModel is Percentage-->
                                        <v-text-field dense v-if="item.Logic=='Project Target'"
                                                      label="Target (%)"
                                                      outlined hide-details v-model="item.Target"></v-text-field>
                                        <!--paymentTermsLogicModel is Date-->
                                        <v-menu v-if="item.Logic=='Date'"
                                                ref="paymentTermsDateMenu"
                                                v-model="paymentTermsDateMenu"
                                                close-on-content-click
                                                :return-value.sync="date"
                                                transition="scale-transition"
                                                offset-y
                                                min-width="290px">
                                            <template v-slot:activator="{ on, attrs }">
                                                <v-text-field hint="DD/MM/YYYY format" persistent-hint
                                                              v-model="item.paymentdate"
                                                              label="Select Date"
                                                              append-icon="mdi-calendar-month-outline"
                                                              readonly outlined dense
                                                              v-bind="attrs"
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker color="primary" v-model="item.paymentdate" no-title @input="paymentTermsDateMenu = false">
                                            </v-date-picker>
                                        </v-menu>
                                    </v-col>
                                    <v-col>
                                        <v-text-field dense
                                                      label="Enter Amount"
                                                      outlined hide-details v-model="item.Amount"></v-text-field>
                                    </v-col>
                                    <v-col md="auto">
                                        <v-btn fab x-small depressed class="mr-2" max-width="20" @click="deletePaymentTermsFn">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-col>

                            <!-- Add Budget Plan-->
                            <v-col md="12" v-bind:style= "[BudgetPlanItems.length>=1 ? {'display':'block'} : {'display':'none'}]">
                                <h4 class="font-weight-medium">Budget Plan</h4>
                                <v-row v-for="(item,index) in BudgetPlanItems" :key="index">
                                    <v-col>
                                        <v-row>
                                            <v-col cols="12">
                                                <v-text-field dense
                                                              label="Budget Head"
                                                              outlined hide-details
                                                              v-model="item.BudgetHeadName"></v-text-field>
                                            </v-col>
                                        </v-row>
                                        <v-row v-for="(SItem , Index) in item.BudgetPlanSubList">
                                            <v-col>
                                                <v-text-field dense
                                                              label="Item Name"
                                                              outlined hide-details
                                                              v-model="SItem.SubHeaderName"></v-text-field>
                                            </v-col>
                                            <v-col>
                                                <v-text-field dense
                                                              label="Enter Amount" type="number"
                                                              outlined hide-details
                                                              v-model="SItem.Amount"></v-text-field>
                                            </v-col>
                                            <v-col md="auto">
                                                <!--<v-btn v-if="budgetPlanItemField==false" v-bind:disabled="SItem.SubHeaderName == '' || SItem.Amount == 0" fab x-small depressed class="mr-2 mt-1" max-width="20" @click="budgetPlanItemField=!budgetPlanItemField">-->
                                                <v-btn v-show="Index == item.BudgetPlanSubList.length - 1" v-bind:disabled="SItem.SubHeaderName == '' || SItem.Amount == 0" fab x-small depressed class="mr-2 mt-1" max-width="20" @click="AddHeaderSubItem(index,SItem)">
                                                    <v-icon color="#00B448" size="15">mdi-plus</v-icon>
                                                </v-btn>
                                                <v-btn v-show="item.BudgetPlanSubList.length-1 && Index !== item.BudgetPlanSubList.length - 1" fab x-small depressed class="mr-2 mt-1" max-width="20" @click="deleteBudgetPlanSubItemFn(index,SItem)">
                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                </v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col md="auto">
                                        <v-btn fab x-small depressed class="mr-2 mt-2" max-width="20" @click="deleteBudgetPlanFn(item)">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-col>
                                </v-row>

                            </v-col>
                        </v-row>
                    </v-form>
                </v-col>
            </v-row>
        </v-col>

        <v-col cols="12" align="right">
            <!--<Secondary-Button title="Confirm" class="btn-122x36 mr-4" v-on:click.native="fnSaveInputs()" v-bind:disabled="ProjectBudget.length==0"></Secondary-Button>-->
            <Secondary-Button title="Confirm" class="btn-122x36 mr-4" v-on:click.native="fnSaveInputs()"></Secondary-Button>
            <v-btn depressed class="btn-122x36" height="37" v-on:click="GoToCreateNewProjectStepsFourFn">Previous</v-btn>
        </v-col>
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
    </v-row>
</template>
<script>
    import SecondaryButton from '@/components/secondary-button.vue'
    import PreLoader from '@/components/pre-loader.vue'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objCreateproject;
    async function importCreateProjectscript() {
        return Promise.all([
            import("../../BL/CreateProject.js").then(mod => {
                objCreateproject = new mod.CreateProject();
            })
        ]);
    };
     
  export default{

     props:[
      'CreateNewProjectStepsProp', 'CreateNewProjectDialogProp'
        ],


    data : function(){
        return {
            valid: true,
            ProjectId: 0,
            GroupId: 0,
            WorkspaceID: 0,
            date: new Date().toISOString().substr(0, 10),
            addDonorItems:[
                { UserID: '0', DonorBudgetAmount:'0'}
            ],
            Donors:[],
            paymentTermsLogic:[
                'Date','Project Target'
            ],
            paymentTermsLogicModel:null,
            paymentTermsDateMenu:false,
            paymentTermsItems: [{ PaymentTermName: '', Logic: '', LogicType: '', paymentdate: this.getCurrentDate(), Target:'', Amount: '0' }],
            ProjectBudget :'',
            BudgetPlanItems:[{BudgetHeadName :'' , HeadAmount :'',BudgetPlanSubList :[{SubHeaderName :'' , Amount :'0'}]}],
            Amount: 0,
            SelectedDonorItems: [],
            PreLoader: false,
            successSnackbarMsg: false,
            errorSnackbarMsg: false,
            infoSnackbarMsg: false,
            Msg:'',
        }


    },

    components:{
        'Secondary-Button': SecondaryButton,
        'Pre-Loader': PreLoader,
    },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await importCreateProjectscript();


        },
        methods: {
            /// <summary>
            ///  hideCreateNewProjectDialogFn :- Function used to hide create new project dialog box
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            hideCreateNewProjectDialogFn() {
                this.CreateNewProjectDialogProp = false;
                this.$emit('CreateNewProjectDialogPropChange', this.CreateNewProjectDialogProp)
            },

            /// <summary>
            ///  GoToCreateNewProjectStepsFourFn :- Function used to go to new project step four (previous step)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            GoToCreateNewProjectStepsFourFn() {
                this.CreateNewProjectStepsProp = 4;
                this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
            },

            /// <summary>
            ///  addDonorsFn :- Function used to add donors
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            addDonorsFn: function () {
                this.addDonorItems.push({ UserID: '0', DonorBudgetAmount: 0 });
            },

            /// <summary>
            ///  deleteDonorsFn :- Function used to delete donor items
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            deleteDonorsFn: function (item) {
                this.addDonorItems.splice(this.addDonorItems.indexOf(item), 1);
            },

            /// <summary>
            ///  getCurrentDate :- Function used to get current date
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            getCurrentDate()
            {
                const current = new Date();
                const date = current.getDate()+'-'+(current.getMonth()+1)+'-'+current.getFullYear();
            },

            /// <summary>
            ///  addPaymentTermsFn :- Function used to add payment terms
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            addPaymentTermsFn: function () {
                if (this.paymentTermsItems.length > 0) {
                    var Count = this.paymentTermsItems.length - 1;
                    if (this.paymentTermsItems[Count].PaymentTermName != "" && this.paymentTermsItems[Count].Logic != "" && this.paymentTermsItems[Count].Amount != "0") {
                        this.paymentTermsItems.push({ PaymentTermName: '', Logic: '', LogicType: '', paymentdate: this.getCurrentDate(), Target: '', Amount: '0' });
                    } else {
                        this.infoSnackbarMsg = true;
                        this.Msg = "Please enter all payment term feilds";
                    }
                }
                else {
                    this.paymentTermsItems.push({ PaymentTermName: '', Logic: '', LogicType: '', paymentdate: this.getCurrentDate(), Target: '', Amount: '0' });
                }
            },

            /// <summary>
            ///  deletePaymentTermsFn :- Function used to delete payment terms items
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            deletePaymentTermsFn: function (item) {
                this.paymentTermsItems.splice(this.paymentTermsItems.indexOf(item), 1);
            },

            /// <summary>
            ///  addBudgetPlanFn :- Function used to add budget plan
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            addBudgetPlanFn: function () {
                if (this.BudgetPlanItems.length > 0) {
                    var Count = this.BudgetPlanItems.length - 1;
                    if (this.BudgetPlanItems[Count].BudgetHeadName != "") {
                        if (this.BudgetPlanItems[Count].BudgetPlanSubList.length > 0) {
                            var CountSubHead = this.BudgetPlanItems[Count].BudgetPlanSubList.length - 1;
                            if (this.BudgetPlanItems[Count].BudgetPlanSubList[CountSubHead].SubHeaderName != "" && this.BudgetPlanItems[Count].BudgetPlanSubList[CountSubHead].Amount != "0") {
                                this.BudgetPlanItems.push({ BudgetHeadName: '', HeadAmount: '', BudgetPlanSubList: [{ SubHeaderName: '', Amount: '0' }] });
                            } else {
                                this.infoSnackbarMsg = true;
                                this.Msg = "Please enter all Budget Sub Head feilds";
                            }
                        }

                    } else {
                        this.infoSnackbarMsg = true;
                        this.Msg = "Please enter all Budget Head feilds";
                    }
                }
                else {
                    this.BudgetPlanItems.push({ BudgetHeadName: '', HeadAmount: '', BudgetPlanSubList: [{ SubHeaderName: '', Amount: '0' }] });
                }
            },

            /// <summary>
            ///  deleteBudgetPlanFn :- Function used to delete budget plan items
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            deleteBudgetPlanFn: function (item) {
                this.BudgetPlanItems.splice(this.BudgetPlanItems.indexOf(item), 1);
            },

            /// <summary>
            ///  AddHeaderSubItem :- Function used to add header sub items, when clicking on the plus button
            /// </summary>
            /// <param name="i"></param>
            /// <param name="p"></param>
            /// <returns> </returns>
            AddHeaderSubItem(i,p) {
                this.BudgetPlanItems[i].BudgetPlanSubList.push({});
            },

            /// <summary>
            ///  deleteBudgetPlanSubItemFn :- Function used to delete budget plan sub items
            /// </summary>
            /// <param name="i"></param>
            /// <param name="item"></param>
            /// <returns> </returns>
            deleteBudgetPlanSubItemFn: function (i,item) {
                this.BudgetPlanItems[i].BudgetPlanSubList.splice(this.BudgetPlanItems[i].BudgetPlanSubList.indexOf(item), 1);
            },

            /// <summary>
            ///  GetProjectDonors :- Function used to fetch notify users by assigned group (Finance,Donor) based on GroupID,WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectDonors() {
                var data = new FormData();
                data.append("GroupID", this.GroupId);
                data.append("Type", 'Donor');
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetNotifyUsers(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.Donors = result.data;
                    }
                }
            },

            /// <summary>
            ///  fnSaveInputs :- Function used to save inputs (Project Budget, Add Donors, Payment Terms and Budget Plan)
            ///  project budget amount is mandatory to save
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnSaveInputs() {
                if (this.$refs.Form.validate() === true) {
                    this.PreLoader = true;

                    // removing empty payment terms item from array
                    if (this.paymentTermsItems.length > 0) {
                        var Count = this.paymentTermsItems.length - 1
                        if (this.paymentTermsItems[Count].PaymentTermName == "" || this.paymentTermsItems[Count].Logic == "" || this.paymentTermsItems[Count].Amount == "0") {
                            this.paymentTermsItems.splice(this.paymentTermsItems.indexOf(this.paymentTermsItems[Count]), 1);
                        }
                    }

                    // removing empty Budget plan array
                    if (this.BudgetPlanItems.length > 0) {
                        var Count = this.BudgetPlanItems.length - 1;
                        if (this.BudgetPlanItems[Count].BudgetHeadName != "") {
                            if (this.BudgetPlanItems[Count].BudgetPlanSubList.length > 0) {
                                var CountSubHead = this.BudgetPlanItems[Count].BudgetPlanSubList.length - 1;
                                if (this.BudgetPlanItems[Count].BudgetPlanSubList[CountSubHead].SubHeaderName != "" && this.BudgetPlanItems[Count].BudgetPlanSubList[CountSubHead].Amount != "0") {
                                    //  this.BudgetPlanItems.push({BudgetHeadName :'' , HeadAmount :'',BudgetPlanSubList :[{SubHeaderName :'' , Amount :'0'}]});
                                } else {
                                    this.BudgetPlanItems.splice(this.BudgetPlanItems.indexOf(this.BudgetPlanItems[Count]), 1);
                                }
                            }

                        } else {
                            this.BudgetPlanItems.splice(this.BudgetPlanItems.indexOf(this.BudgetPlanItems[Count]), 1);
                        }
                    }

                    var donor = this.addDonorItems.filter(x => x.DonorBudgetAmount == "" && x.DonorBudgetAmount == "0");
                    if (donor.length > 0) {
                        this.PreLoader = false;
                        this.errorSnackbarMsg = true;
                        this.Msg = "Donors Amount cannot be null";
                        return false;
                    }
                    var totalBudgetAmount = 0;
                    for (var i = 0; i < this.BudgetPlanItems.length; i++) {
                        for (var j = 0; j < this.BudgetPlanItems[i].BudgetPlanSubList.length; j++) {
                            totalBudgetAmount = totalBudgetAmount + parseInt(this.BudgetPlanItems[i].BudgetPlanSubList[j].Amount);
                        }
                        this.BudgetPlanItems[i].HeadAmount = totalBudgetAmount;
                        totalBudgetAmount = 0;
                    }
                    var data = new FormData();
                    data.append("BudgetAmount", this.ProjectBudget);
                    data.append("Donors", JSON.stringify(this.addDonorItems));
                    data.append("PaymnetTerms", JSON.stringify(this.paymentTermsItems));
                    data.append("BudgetPlan", JSON.stringify(this.BudgetPlanItems));
                    data.append("NoOfBudgetPlans", this.BudgetPlanItems.length)
                    data.append("ProjectId", this.ProjectId);
                    var result = await objCreateproject.SaveInputs(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else
                        if (result.status == "Success") {
                            window.projectStatus = "Completed";
                            this.Msg = "Project Created Successfully !";
                            this.successSnackbarMsg = true;
                            (function () {
                                setInterval(() => {
                                    this.hideCreateNewProjectDialogFn();
   
                                }, 2000)
                            })(); 
                            this.$router.go();
                        }
                        else {
                            this.errorSnackbarMsg = true;
                            this.Msg = result.status;
                        }
                    
                    this.PreLoader = false;
                    
                }
            },

        },

      watch: {

            CreateNewProjectStepsProp: function () {
                this.ProjectId = window.ProjectId;//this.$route.query.ProjID;
                this.GroupId = window.GroupId;
                this.GetProjectDonors();

            },
        }

  }
</script>