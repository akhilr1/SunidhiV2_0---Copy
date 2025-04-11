<template>
    <div>
        <v-card class="custom-card-1" height="270">
            <v-card-title style="height:73px">
                Payment Terms
                <v-spacer></v-spacer>
                <!--If there is payments-->

                <template v-if="">
                    <Success-Button id="AutoTest_IP982" class="mr-4" title="Add" @click.native="CreatePaymentTermsDialog=true"></Success-Button>
                    <router-link :to="{name:'ManageProjectFinancePaymentTerms',query: { WsID: WorkspaceID, Access:RWAccess, InitID : InitiativeID,ProjID: ProjectId}}">
                        <Primary-Button-Outlined style="width:106px" title="View Terms"></Primary-Button-Outlined>
                    </router-link>
                </template>

            </v-card-title>
            <v-card-text v-if="PaymentTermsitem.length==0">
                <!--If there is no payment-->
                <v-row>
                    <v-col class="text-center">
                        <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                        <h4 class="title text-center" color="#585858">No payment terms available</h4>
                        <Success-Button id="AutoTest_IP983" class="mt-4" style="width:200px" title="Add Payment Terms" @click.native="CreatePaymentTermsDialog=true"></Success-Button>
                    </v-col>
                </v-row>
            </v-card-text>
            <!--
        //If there is payments -->
            <v-card-text v-if="PaymentTermsitem.length >0" class="v-scrolling-div" style="height:150px">
                <v-row>
                    <v-col>
                        <v-timeline align-top dense>
                            <div v-for="item in PaymentTermsitem" :key="item.PaymentId">
                                <v-timeline-item color="#47BB76" small v-if="item.PaymentStatus=='Done'">
                                    <p class="black--text" v-if="item.Logic == 'Date'">
                                        Name : {{item.PaymentTermName}} , Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}), Amount : {{item.Amount}}
                                    </p>
                                    <p class="black--text" v-else>
                                        Name :{{item.PaymentTermName}} , Logic :{{item.Logic}}({{item.Target}}), Amount : {{item.Amount}}
                                    </p>

                                </v-timeline-item>
                                <v-timeline-item color="#D50808" small v-if="item.PaymentStatus=='Cancelled'">
                                    <p class="black--text" v-if="item.Logic == 'Date'">
                                        Name : {{item.PaymentTermName}} , Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}),Amount : {{item.Amount}}
                                    </p>
                                    <p class="black--text" v-else>
                                        Name : {{item.PaymentTermName}} , Logic :{{item.Logic}}({{item.Target}}), Amount : {{item.Amount}}
                                    </p>
                                </v-timeline-item>
                                <v-timeline-item color="#DEB514" small v-if="item.PaymentStatus=='Pending'">
                                    <p class="black--text" v-if="item.Logic == 'Date'">
                                        Name : {{item.PaymentTermName}} , Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}), Amount : {{item.Amount}}
                                    </p>
                                    <p class="black--text" v-else>
                                        Name : {{item.PaymentTermName}} , Logic : {{item.Logic}}({{item.Target}}) , Amount : {{item.Amount}}
                                    </p>
                                </v-timeline-item>

                            </div>


                            <!--<v-timeline-item color="#9B9B9B" small>
                          <p>
                            Payment of ₹40000 is due<br/>80% Project Progress
                          </p>
                        </v-timeline-item>-->
                        </v-timeline>
                    </v-col>
                </v-row>
            </v-card-text>

            <!--Create Payment Terms Dialog-->
            <v-dialog v-model="CreatePaymentTermsDialog" persistent width="800">
                <v-card>
                    <v-card-title primary-title="" class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                        </v-avatar>
                        Create Payment Terms
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP984" text="" @click="ClosepaymentDialog()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <!--Step-1-->
                    <v-card-text v-show="CreatePaymentTermsStepOne">
                        <h4 class="text-center heading-3 mt-4">Please select the no. of payment terms to create.</h4>
                        <v-row justify="center" class="pt-4">
                            <v-col md="6">
                                <v-text-field type="number" min="0" @keydown="FilterInput($event)" @paste.prevent label="Enter no. of payment terms" outlined="" dense="" v-model="NoOfPayments"></v-text-field>

                            </v-col>
                        </v-row>
                    </v-card-text>
                    <!--Step-2-->
                    <v-card-text v-show="CreatePaymentTermsStepTwo">

                        <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new payment terms.</h4>
                        <h4 class="text-center black--text heading-3 mt-4">
                            <v-btn id="AutoTest_IP985" fab="" small="" v-bind:disabled="stepper==1" class="mr-4" color="#fff" @click="fnSetpperIncrement('Minus')">
                                <v-icon size="30" color="primary">mdi-arrow-left</v-icon>
                            </v-btn>
                            Payment Term {{stepper}} of {{NoOfPayments}}
                            <v-btn id="AutoTest_IP986" fab="" small="" class="ml-4" color="#fff" @click="fnSetpperIncrement('Plus')" v-bind:disabled="stepper==NoOfPayments || !fnCheckMandatory()">
                                <v-icon size="30" color="primary">mdi-arrow-right</v-icon>
                            </v-btn>
                        </h4>
                        <v-row justify="center" class="pt-4">
                            <v-col md="6" style="height:250px" class="v-scrolling-div">
                                <v-text-field class="mt-4" label="Enter Payment Term Name" outlined="" dense="" v-model="PaymentTermName"></v-text-field>
                                <v-select dense="" v-model="CreatePaymentTermsLogicModel"
                                          :items="CreatePaymentTermsLogic"
                                          label="Select Logic"
                                          outlined="">
                                </v-select>

                                <v-menu ref="PaymentDateMenu"
                                        v-model="PaymentDateMenu"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y=""
                                        min-width="290px">
                                    <template v-slot:activator="{ on }">
                                        <v-text-field v-show="CreatePaymentTermsLogicModel==='Date'"
                                                      v-model="paymentdate"
                                                      label="Select Date"
                                                      prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                      readonly=""
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="paymentdate" no-title @input="PaymentDateMenu = false" format="DD-MM-YYYY" :max="ProjectEndDate" :min="ProjectStartDate">

                                    </v-date-picker>
                                </v-menu>
                                <v-text-field type="number" v-show="CreatePaymentTermsLogicModel==='Project Target'" label="Target(%)" outlined="" dense="" v-model="Target"></v-text-field>

                                <v-text-field type="number" label="Type amount" outlined="" dense="" v-model="amount" @change="AddPaymentTermAmt(amount)"></v-text-field>

                                <span class="small-text mr-1">
                                    <v-icon size="12">mdi-account</v-icon> Notify People
                                </span>
                                <v-avatar size="27" class="team-avatar" v-if="selectedFinanceUser == 0">
                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="selectedFinanceUser.length == 1">
                                    <span>{{selectedFinanceUser[0].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="selectedFinanceUser.length >= 2">
                                    <span>{{selectedFinanceUser[0].Name.charAt(0)}}</span>
                                </v-avatar>
                                <v-avatar size="27" class="team-avatar" color="red" v-if="selectedFinanceUser.length >= 2">
                                    <span>{{selectedFinanceUser[1].Name.charAt(0)}}</span>
                                </v-avatar>

                                <v-menu absolute v-model="NotificationSelectPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP987" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" @click="GetFinanceOrDonarUsers()">
                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Notification select pop up-->
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Add people to Notify
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP988" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp=false">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:150px;" class="v-scrolling-div">
                                            <v-row>
                                                <v-col class="py-0">
                                                    <v-list dense="" class="py-0">
                                                        <v-list-item>
                                                            <v-list-item-content style="font-size:17px">
                                                                Finance Manegers
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                        <v-divider></v-divider>
                                                        <template v-for="finUsers in FinanceUsersTemp">
                                                            <v-list-item>
                                                                <v-list-item-icon style="margin-top:-10px;">
                                                                    <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserList(finUsers.ID,'Finance', $event)" v-model="finUsers.IsSelected"></v-checkbox>
                                                                </v-list-item-icon>
                                                                <v-list-item-content>
                                                                    <v-list-item-title>
                                                                        <v-avatar size="27" left="">
                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                                        </v-avatar>
                                                                        {{finUsers.Name}}
                                                                    </v-list-item-title>
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                            <v-divider></v-divider>
                                                        </template>
                                                    </v-list>
                                                </v-col>
                                                <v-col class="py-0">
                                                    <v-list dense="" class="py-0">
                                                        <v-list-item>
                                                            <v-list-item-content style="font-size:17px">
                                                                Donars
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                        <v-divider></v-divider>
                                                        <template v-for="DonarUser of DonarUserTemp">
                                                            <v-list-item>
                                                                <v-list-item-icon style="margin-top:-10px;">
                                                                    <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserList(DonarUser.ID,'Donar', $event)" v-model="DonarUser.IsSelected"></v-checkbox>
                                                                </v-list-item-icon>
                                                                <v-list-item-content>
                                                                    <v-list-item-title>
                                                                        <v-avatar size="27" left="">
                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                                        </v-avatar>
                                                                        {{DonarUser.Name}}
                                                                    </v-list-item-title>
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                            <v-divider></v-divider>
                                                        </template>
                                                    </v-list>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row no-gutters="">
                                                <v-col md="12" align="center">
                                                    <Success-Button id="AutoTest_IP989" style="width:130px" class="mb-2 center-block" title="Save" @click.native="NotificationSelectPopUp=false"></Success-Button>
                                                </v-col>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                    <!--__________________________-->
                                </v-menu>
                            </v-col>

                        </v-row>
                        <!--_______________________________-->
                    </v-card-text>

                    <v-card-actions class="pb-4">
                        <v-col>
                            <Previous-Button-Outlined id="AutoTest_IP990" v-show="CreatePaymentTermsStepTwo" class="float-right" @click.native="CreatePaymentTermsStepOne=true,CreatePaymentTermsStepTwo=false"></Previous-Button-Outlined>
                            <Cancel-Button-Outlined-Sm id="AutoTest_IP991" v-show="CreatePaymentTermsStepOne" class="float-right" @click.native="CreatePaymentTermsDialog=false"></Cancel-Button-Outlined-Sm>
                            <Next-button-primary-light id="AutoTest_IP992" v-show="CreatePaymentTermsStepOne" class="float-right mr-4" @click.native="CreatePaymentTermsStepTwo=true,CreatePaymentTermsStepOne=false" v-bind:disabled="NoOfPayments==0"></Next-button-primary-light>


                            <v-btn id="AutoTest_IP993" v-show="CreatePaymentTermsStepTwo" v-bind:disabled="stepper!=NoOfPayments || !fnCheckMandatory()" class="elevation-0 float-right mr-4 primary-btn-light"
                                   dark="" height="36" width="122" @click="CreatePaymentTermsDialog=false ,fnInvokePaymentTermList(NoOfPayments),fnSavepaymentTerms()">
                                Confirm
                            </v-btn>
                        </v-col>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <v-snackbar v-model="CreatePaymentTermSuccessSnackbar" top="" color="primary">
                Payment term successfully created
            </v-snackbar>
            <v-snackbar v-model="ErrorPaymentTermSuccessSnackbar" top="" color="error">
                Failed
            </v-snackbar>
        </v-card>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </div>
</template>
<script>
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import AddButtonFab from '@/components/add-button-fab.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'

    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objProject;
    async function importscriptFinance() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then(mod => {
                objProject = new mod.ManageProjectFinance();
            })
        ]);
    };

    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectId: 0,
                CreatePaymentTermsDialog: false,
                CreatePaymentTermsLogic: ['Date', 'Project Target'],
                CreatePaymentTermsStepOne: true,
                CreatePaymentTermsStepTwo: false,
                CreatePaymentTermsStepThree: false,
                CreatePaymentTermsLogicModel: '',
                PaymentDateMenu: false,
                CreatePaymentTermsStepper: 1,
                NotificationSelectPopUp: false,
                WorkspaceID: 0,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                CreatePaymentTermSuccessSnackbar: false,
                ErrorPaymentTermSuccessSnackbar:false,
                paymentdate: new Date().toISOString().substr(0, 10),
                stepper: 1,
                NoOfPayments: "",
                PaymentId: 0,
                PaymentTermName: "",
                Target: 0,
                amount: 0,
                GroupId: 0,
                PaymentTerm: {
                    PaymentTermList: [],
                },
                PaymentTermsitem: [],
                lstFinanceUsers: [],
                lstDonors: [],
                selectedFinanceUser: [],
                chkBoxValue: true,
                FinanceUsersTemp: [],
                DonarUserTemp: [],
                totermsAmt: 0,
                ProjectStartDate: new Date().toISOString().substr(0, 10),
                ProjectEndDate: new Date().toISOString().substr(0, 10),
          
            }
        },
        components: {
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Add-Button-Fab': AddButtonFab,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await importscriptFinance();
            await this.fnGetProjectDetailsByID();
            await this.fnGetPaymentTermsByprjID();
            await this.fnGetGroupDetailsByprjID();
            await this.fnFetchFinanceAndDonor();
            
        },
        methods: {
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            async GetFinanceOrDonarUsers(index) {

                if (this.PaymentTerm.PaymentTermList.length > 0) {
                    var temp = this.PaymentTerm.PaymentTermList.filter(x => x.Index == this.stepper)
                    if (temp.length > 0) {
                        var id;
                        var finId;
                        var DonId;
                        var i;
                        var j;
                        for (i = 0; i <= this.FinanceUsersTemp.length - 1; i++) {
                            this.FinanceUsersTemp[i].IsSelected = false;
                        }

                        for (j = 0; j <= this.DonarUserTemp.length - 1; j++) {
                            this.DonarUserTemp[j].IsSelected = false;
                        }

                        for (id = 0; id <= this.PaymentTerm.PaymentTermList[this.stepper - 1].UserList.length - 1; id++) {

                            for (finId = 0; finId <= this.FinanceUsersTemp.length - 1; finId++) {
                                if (this.PaymentTerm.PaymentTermList[this.stepper - 1].UserList[id].ID == this.FinanceUsersTemp[finId].ID) {
                                    this.FinanceUsersTemp[finId].IsSelected = true;
                                } else {
                                    //this.FinanceUsersTemp[finId].IsSelected = false;
                                }
                            }

                            for (DonId = 0; DonId <= this.DonarUserTemp.length - 1; DonId++) {
                                if (this.PaymentTerm.PaymentTermList[this.stepper - 1].UserList[id].ID == this.DonarUserTemp[DonId].ID) {
                                    this.DonarUserTemp[DonId].IsSelected = true;
                                } else {
                                    // this.DonarUserTemp[DonId].IsSelected = false;
                                }

                            }

                        }
                    }
                } else {
                    if (this.lstFinanceUsers.length > 0) {
                        var item;
                        for (item = 0; item <= this.lstFinanceUsers.length - 1; item++) {
                            this.FinanceUsersTemp.push({ ID: this.lstFinanceUsers[item].ID, Name: this.lstFinanceUsers[item].Name, IsSelected: false })
                        }
                    }

                    if (this.lstDonors.length > 0) {
                        var item
                        for (item = 0; item <= this.lstDonors.length - 1; item++) {
                            this.DonarUserTemp.push({ ID: this.lstDonors[item].ID, Name: this.lstDonors[item].Name, IsSelected: false })
                        }
                    }
                }
            },
            async GetSelectedUserList(userid, type, event) {
                var temp;
                if (event == true) {
                    if (type == 'Finance') {
                        temp = this.selectedFinanceUser.filter(function (x) {
                            return x.ID == userid;
                        });
                        if (temp.length == 0) {
                            var list = this.lstFinanceUsers.filter((x => x.ID == userid));
                            this.selectedFinanceUser.push(list[0]);
                        }
                    } else if (type == 'Donar') {
                        temp = this.selectedFinanceUser.filter(function (x) {
                            return x.ID == userid;
                        });
                        if (temp.length == 0) {
                            var list = this.lstDonors.filter((x => x.ID == userid));
                            this.selectedFinanceUser.push(list[0]);
                        }
                    }
                }
                else {
                    var temp = this.selectedFinanceUser.filter(function (x) {
                        return x.ID == userid;
                    });
                    if (temp.length > 0) {
                        this.selectedFinanceUser.splice(this.selectedFinanceUser.indexOf(temp), 1);
                        this.selectedFinanceUser = [...this.selectedFinanceUser];
                    }
                }
            },
            async FilterInput(event) {

                var isNotWanted;
                var keyCode = ('which' in event) ? event.which : event.keyCode;

                isNotWanted = (keyCode == 69 || keyCode == 101 || keyCode == 109 || keyCode == 189);
                if (isNotWanted == true) {

                    event.preventDefault();
                }
            },
            async fnGetPaymentTermsByprjID() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetPaymentTermsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.PaymentTermsitem = result.data;
                       
                    }
            },
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objManageProject.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
                    this.ProjectEndDate = this.projectInfo.EndDate.substr(0, 10)//this.formatDate(this.projectInfo.EndDate.substr(0, 10));
                    this.ProjectStartDate = this.projectInfo.StartDate.substr(0, 10)// this.formatDate(this.projectInfo.StartDate.substr(0, 10));
                } else {
                    this.Msg = ("Failed to get details of Project " + this.projectInfo.ProjectId);
                    this.snackbarMsg = true;
                }
            },
            async  fnSetpperIncrement(Mode) {
                if (Mode == 'Plus') {
                    this.stepper = this.stepper + 1;
                    this.fnInvokePaymentTermList(this.stepper)
                } else if (Mode == 'Minus') {
                    this.stepper = this.stepper - 1;
                    this.fnInvokePrevPaymentTermList(this.stepper)
                }
            },
            async fnInvokePaymentTermList(index) {
                if (index > 0) {
                    var temp = this.PaymentTerm.PaymentTermList.filter(x => x.Index == index);
                    if (temp.length > 0) {

                        this.PaymentTermName = temp[0].PaymentTermName;
                        this.CreatePaymentTermsLogicModel = temp[0].Logic;
                        this.paymentdate = temp[0].paymentdate;
                        this.Target = temp[0].Target;
                        this.amount = temp[0].amount;
                        this.selectedFinanceUser = temp[0].UserList;
                    } else {
                        this.PaymentTerm.PaymentTermList.push({ Index: index - 1, PaymentTermName: this.PaymentTermName, Logic: this.CreatePaymentTermsLogicModel, paymentdate: this.paymentdate, Target: this.Target, amount: this.amount, UserList: this.selectedFinanceUser });

                        this.PaymentTermName = "";
                        this.paymentdate = new Date().toISOString().substr(0, 10);
                        this.Target = "";
                        this.amount = "";
                        this.CreatePaymentTermsLogicModel = '';
                        this.selectedFinanceUser = [];
                        var i;
                        var j;
                        for (i = 0; i <= this.FinanceUsersTemp.length - 1; i++) {
                            this.FinanceUsersTemp[i].IsSelected = false;
                        }
                        for (j = 0; j <= this.DonarUserTemp.length - 1; j++) {
                            this.DonarUserTemp[j].IsSelected = false;
                        }
                    }
                }
            },
            async fnInvokePrevPaymentTermList(index) {

                if (index + 1 == this.NoOfPayments) {
                    if (this.PaymentTermName != "") {
                        this.PaymentTerm.PaymentTermList.push({ Index: this.NoOfPayments, PaymentTermName: this.PaymentTermName, Logic: this.CreatePaymentTermsLogicModel, paymentdate: this.paymentdate, Target: this.Target, amount: this.amount, UserList: this.selectedFinanceUser });
                        var i;
                        var j;
                        for (i = 0; i <= this.FinanceUsersTemp.length - 1; i++) {
                            this.FinanceUsersTemp[i].IsSelected = false;
                        }

                        for (j = 0; j <= this.DonarUserTemp.length - 1; j++) {
                            this.DonarUserTemp[j].IsSelected = false;
                        }
                    }
                }
                if (index > 0) {

                    var temp = this.PaymentTerm.PaymentTermList.filter(x => x.Index == index);
                    if (temp.length > 0) {
                        this.PaymentTermName = temp[0].PaymentTermName;
                        this.CreatePaymentTermsLogicModel = temp[0].Logic;
                        this.paymentdate = temp[0].paymentdate;
                        this.Target = temp[0].Target;
                        this.amount = temp[0].amount;
                        this.selectedFinanceUser = temp[0].UserList;
                    }
                }
            },
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.GroupId = result.data.GroupId;
                    }
            },
            async GetProjectUsers(Type) {

                var data = new FormData();
                data.append("GroupID", this.GroupId);
                data.append("Type", Type)
                var result = await objManageProject.GetNotifyUsers(data);

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
            async fnFetchFinanceAndDonor() {
                this.GetProjectUsers('Finance');
                this.GetProjectUsers('Donor');
            },
            async fnSavepaymentTerms() {
                var data = new FormData();
                data.append("NoOfPaymentTerms", this.NoOfPayments);
                data.append("PaymentTermList", JSON.stringify(this.PaymentTerm.PaymentTermList));
                data.append("Notify", this.NotifyCheckbox);
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.SavepaymentTerms(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "success") {
                        this.CreatePaymentTermSuccessSnackbar = true;
                        this.ClosepaymentDialog();
                        this.fnGetPaymentTermsByprjID();
                    }
                    else {
                        this.ErrorPaymentTermSuccessSnackbar = true;
                    }
            },
            fnCheckMandatory() {
                    if (this.PaymentTermName == "") {
                        return false;
                    }
                    if (this.amount == "" || this.amount == "0") {
                        return false;
                }
                if (this.selectedFinanceUser == 0)
                { return false;}
                    if (this.CreatePaymentTermsLogicModel == "Date") {
                        if (this.paymentdate == "") {
                            return false;
                        }
                    } else if (this.CreatePaymentTermsLogicModel== "Project Target") {
                        if (this.Target == "") {
                            return false;
                        }
                    }                                                                               
                return true;
            },
            ClosepaymentDialog() {
                this.NoOfPayments = "";
                this.PaymentTermName = "";
                this.Target = "";
                this.amount = "";
                this.paymentdate = new Date().toISOString().substr(0, 10);
                this.CreatePaymentTermsLogicModel = "";
                this.CreatePaymentTermsDialog = false;
                this.CreatePaymentTermsStepOne = true;
                this.CreatePaymentTermsStepTwo = false;
                this.PaymentTerm.PaymentTermList = [];
                this.stepper = 1;
                this.selectedFinanceUser = [];
            },
            AddPaymentTermAmt(Amt) {
               
                this.totermsAmt = parseInt(this.totermsAmt) + parseInt(Amt);
                if (parseInt(this.projectInfo.Budget) < (parseInt(this.projectInfo.PaymentTermAmount) + parseInt(this.totermsAmt))) {
                    this.Msg = ("Total Budget Amount should not be greater than the sum of PaymentTerm Amount");
                    this.snackbarMsg = true;
                    this.totermsAmt = parseInt(this.totermsAmt) - parseInt(Amt);
                    this.amount = "";
                }
               

            },
       
        },
    }
</script>