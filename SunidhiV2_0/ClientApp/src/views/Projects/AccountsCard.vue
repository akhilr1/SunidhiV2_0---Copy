<template>
    <v-card class="custom-card-1" height="270">
        <v-card-title style="height:73px">
            Accounts
            <v-spacer></v-spacer>
            <!--If there is account-->

            <template v-if="AccountList.length>0">
                <Success-Button id="AutoTest_IP1" class="mr-4" title="Add" @click.native="AddAccountsDialog=true"></Success-Button>
                <router-link :to="{name:'ManageProjectFinanceAccounts',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId,ProjID: projectInfo.ProjectId}}">
                    <Primary-Button-Outlined style="width:130px" title="View Accounts"></Primary-Button-Outlined>
                </router-link>
            </template>
        </v-card-title>

        <v-card-text v-if="AccountList.length==0">
            <v-row>
                <v-col class="text-center">
                    <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                    <h4 class="title text-center" color="#585858">No accounts available</h4>
                    <Success-Button id="AutoTest_IP2" class="mt-4" style="width:200px" title="Add Accounts" @click.native="AddAccountsDialog=true"></Success-Button>
                </v-col>
            </v-row>
        </v-card-text>
        <v-card-text v-else class="v-scrolling-div" style="height:150px;">
            <v-row v-for="item in AccountList" :key="item.ID">
                <v-col md="12">
                    <h5 class="heading-3-dark">{{ item.AccountName }}</h5>
                    <p class="mb-0">
                        {{ item.BankName }}<br />
                        {{ item.BankAccountNumber }}<br />
                        {{ item.BankIFSCCode }}
                    </p>
                </v-col>
            </v-row>
        </v-card-text>

        <!--Add Account Dialog-->
        <v-dialog v-model="AddAccountsDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Create Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP3" text="" @click="AddAccountsDialog=false,fnInitializeCreateAccount()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="form">
                    <v-card-text>
                        <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new account.</h4>
                        <v-row justify="center">
                            <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Account Name" v-model="CreateAccount.AccountName">
                                </v-text-field>
                                <v-textarea outlined="" dense="" name="" label="Description" v-model="CreateAccount.Description"></v-textarea>
                                <v-text-field outlined="" dense="" label="Contact Name" v-model="CreateAccount.ContactName">
                                </v-text-field>
                                <v-text-field outlined="" dense="" type="number" label="Contact Number" v-model="CreateAccount.ContactPhone"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                </v-text-field>
                                <v-text-field required outlined="" dense="" label="Email" type="email" :rules="emailRules" v-model="CreateAccount.ContactEmail">
                                </v-text-field>
                                <v-textarea outlined="" dense="" name="" label="Address" value="" v-model="CreateAccount.ContactAddress"></v-textarea>
                                <v-row>
                                    <v-col class="py-0">
                                        <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="CreateAccount.Pincode"
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;" min="0">
                                        </v-text-field>
                                    </v-col>
                                    <v-col class="py-0">
                                        <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="CreateAccount.BankIFSCCode">
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="CreateAccount.BankName" @change="fnGetBranchList()">
                                </v-select>
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="CreateAccount.BankBranch" @change="fnGetIFSC()">
                                </v-select>
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="CreateAccount.BankAccountType">
                                </v-select>
                                <v-row>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No." v-model="CreateAccount.BankAccountNumber"
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==30) return false;" min="0">
                                        </v-text-field>
                                    </v-col>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="CreateAccount.PAN">
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                                <span class="small-text mr-1">
                                    <v-icon size="12">mdi-link-box</v-icon> Link the user
                                </span>
                                <v-avatar size="27" class="team-avatar">
                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                                </v-avatar>
                                <v-menu max-width="300" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Assign Account User pop up-->
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Select the User
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP4" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AssignAccountUserPopUp=false,CreateAccount.UserId=0,selectedUser=0">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:150px" class="v-scrolling-div">
                                            <v-list dense>
                                                <template>
                                                    <v-radio-group v-model="selectedUser">
                                                        <v-radio v-for="userItem in ProjectUserList"
                                                                 :key="userItem.UserID"
                                                                 :label="userItem.UserName"
                                                                 :value="userItem.UserID"></v-radio>
                                                    </v-radio-group>
                                                    <!--v-list-item v-for="userItem in ProjectUserList" :key="userItem.ID">
                                                    <v-list-item-icon style="margin-top:-10px;">
                                                        <v-checkbox hide-details color="#52B962"></v-checkbox>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>
                                                        <v-list-item-title>
                                                            <v-avatar size="27" left>
                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                            </v-avatar>
                                                            {{ userItem.Name }}
                                                        </v-list-item-title>
                                                    </v-list-item-content>
                                                </v-list-item-->
                                                    <v-divider></v-divider>
                                                </template>
                                            </v-list>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row no-gutters>
                                                <v-col md="12" class="py-0" align="center">
                                                    <Success-Button id="AutoTest_IP5" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignAccountUserPopUp=false,CreateAccount.UserId=selectedUser"></Success-Button>
                                                </v-col>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                    <!--_______________________________________-->
                                </v-menu>

                                <v-checkbox label="80G eligible" color="#52B962" multiple-active="" v-model="CreateAccount.Is80Geligible"></v-checkbox>
                            </v-col>

                        </v-row>
                    </v-card-text>
                </v-form>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP6" class="float-right" @click.native="AddAccountsDialog=false,fnInitializeCreateAccount()"></Cancel-Button-Outlined-Sm>
                        <v-btn id="AutoTest_IP7" v-bind:disabled="fnCheckMandatory()" class="elevation-0 float-right mr-4 primary-btn-light" height="36" width="122" @click="fnCreateAccount()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <v-snackbar v-model="AccountSaveSuccessSnackbar" top="" color="primary">
            {{ message }}
        </v-snackbar>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </v-card>
</template>

<script>
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'

    var objManageProjectAccount;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectAccount.js").then(mod => {
                objManageProjectAccount = new mod.ManageProjectAccount();
            })
        ]);
    };
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                AccountSaveSuccessSnackbar: false,
                AddAccountsDialog: false,
                SelectBankItems: [],
                SelectBankBranchItems: [],
                SelectBankAccountTypeItems: [],
                AssignAccountUserPopUp: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,

                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                },
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    StartDate: "",
                    EndDate: "",
                    CountryID: 0,
                    StateID: 0,
                    DistrictID: 0,
                    ProjectTags: "",
                },
                ProjectUserList: [],
                AccountList: [],
                CreateAccount: {
                    AccountName: "",
                    Description: "",
                    ContactName: "",
                    ContactPhone: 0,
                    ContactEmail: "",
                    ContactAddress: "",
                    Pincode: 0,
                    BankIFSCCode: "",
                    BankName: "",
                    BankBranch: "",
                    BankAccountType: "",
                    BankAccountNumber: 0,
                    PAN: "",
                    ProjectId: 0,
                    UserId: 0,
                    Is80Geligible: 0,
                },
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],
                selectedUser: 0,
                message: "",
            }
        },
        components: {
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button': SuccessButton,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            await importscript();
            await this.fnGetProjectDetailsByID();
            await this.fnGetUsersListByGroupId();
            await this.fnGetProjectAccountsByID();
            await this.fnGetBankList();
            await this.fnGetAccountTypeList();
        },
        methods: {
            /// <summary>
            ///  resetValidation :- Function used to reset validation
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            resetValidation() {
                this.$refs.form.resetValidation()
            },

            /// <summary>
            ///  fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProjectAccount.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    //alert("Project Name = " + this.projectInfo.ProjectName);
                } else {
                    this.Msg = ("Failed to get details of Project " + this.projectInfo.ProjectId);
                     this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetUsersListByGroupId :- Function used to fetch project users based on GroupID
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("Type", "Donor");
                var result = await objManageProjectAccount.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectUserList = result.data;
                }
            },

            /// <summary>
            ///  fnGetProjectAccountsByID :- Function used to fetch project account details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetProjectAccountsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                var result = await objManageProjectAccount.GetProjectAccountsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AccountList = result.data;
                    //alert("AccountList = " + this.AccountList.length);
                } else {
                    this.Msg = ("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                     this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnInitializeCreateAccount :- Function used to set default values in account variables when click on close button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeCreateAccount() {
                this.CreateAccount.AccountName = "";
                this.CreateAccount.Description = "";
                this.CreateAccount.ContactName = "";
                this.CreateAccount.ContactPhone = 0;
                this.CreateAccount.ContactEmail = "";
                this.CreateAccount.ContactAddress = "";
                this.CreateAccount.Pincode = 0;
                this.CreateAccount.BankIFSCCode = "";
                this.CreateAccount.BankName = "";
                this.CreateAccount.BankBranch = "";
                this.CreateAccount.BankAccountType = "";
                this.CreateAccount.BankAccountNumber = 0;
                this.CreateAccount.PAN = "";
                this.CreateAccount.ProjectId = 0;
                this.CreateAccount.UserId = 0;
                this.CreateAccount.Is80Geligible = 0;
                this.selectedUser = 0;
               this.resetValidation();
            },

            /// <summary>
            ///  fnCreateAccount :- Function used to add new account to the project
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnCreateAccount() {
                if (this.CreateAccount.ContactEmail.length > 0) {
                    const pattern = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                    var emailtest = pattern.test(this.CreateAccount.ContactEmail);
                    if (!emailtest) {
                        this.Msg = ("Please enter valid E-mail");
                         this.snackbarMsg = true;
                        return;
                    }
                }

                this.AddAccountsDialog = false;
                this.AccountSaveSuccessSnackbar = true;
                var data = new FormData();
                this.CreateAccount.ProjectId = this.projectInfo.ProjectId;
                data.append("AccountName", this.CreateAccount.AccountName);
                data.append("Description", this.CreateAccount.Description);
                data.append("ContactName", this.CreateAccount.ContactName);
                data.append("ContactPhone", this.CreateAccount.ContactPhone);
                data.append("ContactEmail", this.CreateAccount.ContactEmail);
                data.append("ContactAddress", this.CreateAccount.ContactAddress);
                data.append("Pincode", this.CreateAccount.Pincode);
                data.append("BankIFSCCode", this.CreateAccount.BankIFSCCode);
                data.append("BankName", this.CreateAccount.BankName);
                data.append("BankBranch", this.CreateAccount.BankBranch);
                data.append("BankAccountType", this.CreateAccount.BankAccountType);
                data.append("BankAccountNumber", this.CreateAccount.BankAccountNumber);
                data.append("PAN", this.CreateAccount.PAN);
                data.append("ProjectId", this.CreateAccount.ProjectId);
                data.append("UserId", this.CreateAccount.UserId);
                data.append("Is80Geligible", this.CreateAccount.Is80Geligible);
                var result = await objManageProjectAccount.AddAccount(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.message = "Successfully added account to the project";
                    this.fnGetProjectAccountsByID();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.message = "Failed to add account to the project";
                }
                this.fnInitializeCreateAccount();
            },

            /// <summary>
            ///  fnGetBankList :- Function used to fetch details of bank names
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetBankList() {
                var result = await objManageProjectAccount.GetBankList();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectBankItems = result.data;
                    //alert("Bank List = " + this.SelectBankItems.length);
                } else {
                    this.Msg = ("Failed to get details of Bank Names.");
                     this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetBranchList :- Function used to fetch details of bank branch based on BankName
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetBranchList() {
                var data = new FormData();
                data.append("BankName", this.CreateAccount.BankName);
                var result = await objManageProjectAccount.GetBranchList(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectBankBranchItems = result.data;
                    //alert("Bank Branch List = " + this.SelectBankBranchItems.length);
                } else {
                    this.Msg = ("Failed to get details of Bank Branch.");
                     this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetIFSC :- Function used to fetch details of IFSC code based on BankName,BankBranch
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetIFSC() {
                var data = new FormData();
                data.append("BankName", this.CreateAccount.BankName);
                data.append("BranchName", this.CreateAccount.BankBranch);
                var result = await objManageProjectAccount.GetIFSC(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.CreateAccount.BankIFSCCode = result.data;
                } else {
                    this.Msg =("Failed to get details of IFSC code.");
                     this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetAccountTypeList :- Function used to fetch bank account type details
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetAccountTypeList() {
                var result = await objManageProjectAccount.GetAccountTypeList();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectBankAccountTypeItems = result.data;
                    //alert("Account Type List = " + this.SelectBankAccountTypeItems.length);
                } else {
                    this.Msg = ("Failed to get details of Bank Names.");
                    this.snackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnCheckMandatory :- Function used to check required fields in create account section
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false</returns>
            fnCheckMandatory() {
                if (this.CreateAccount.AccountName == "" ||
                    this.CreateAccount.BankIFSCCode == "" ||
                    this.CreateAccount.BankName == "" ||
                    this.CreateAccount.BankBranch == "" ||
                    this.CreateAccount.BankAccountType == "" ||
                    this.CreateAccount.BankAccountNumber == 0 ||
                    this.CreateAccount.PAN == "" ||
                    this.CreateAccount.UserId == 0)
                    return true;
                else return false;
            }
        },
    }
</script>