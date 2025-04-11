<template>
    <v-card class="custom-card-1">
        <v-card-title class="pt-1 pb-4">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                <router-link :to="{ name: 'ManageProjectFinance', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId,ProjID: projectInfo.ProjectId} }" class="page-head text-link">Finance Details</router-link>
                <v-icon size="15">mdi-chevron-right</v-icon> Accounts
            </h5>
            <v-spacer></v-spacer>
            <Success-Button id="AutoTest_IP429" class="mr-4" title="Create Account" @click.native="AddAccountsDialog=true" v-bind:disabled ="!screenAccess"></Success-Button>

            <!--Bulk Action -->
            <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list v-if="selectedAccountList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                    <v-list-item id="AutoTest_IP430" @click.stop="RemoveAccountDialog = true">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Remove
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>

            <!--Search -->
            <v-text-field label="Search" v-model="SearchAccountList"
                          dense prepend-inner-icon="mdi-magnify"
                          outlined hide-details class="table-search"></v-text-field>

        </v-card-title>
        <!--Payment Term Table-->
        <v-data-table :headers="AccountListHeaders"
                      :items="AccountList"
                      :search="SearchAccountList"
                      v-model="selectedAccountList"
                      item-key="AccountId"
                      show-select>
            <!-- Account Name-->
            <template v-slot:item.AccountName="{item}">
                <v-list class="py-0 table-v-list">
                    <v-list-item class="my-0 px-0" link :to="{name:'ManageProjectFinanceLedgerBook',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, AccID: item.AccountId }}">
                        <v-list-item-icon>
                            <v-avatar size="30" color="#fff" class="elevation-1">
                                <v-icon size="20" color="primary">mdi-book</v-icon>
                            </v-avatar>
                        </v-list-item-icon>
                        <v-list-item-content>
                            {{item.AccountName}}
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </template>

            <!-- Row Actions-->
            <template v-slot:item.AccountsActions="{item}">
                <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled ="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                            <v-icon>mdi-dots-vertical</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light width="auto" class="action-button-list">
                        <v-list-item id="AutoTest_IP431" link @click.stop="EditAccountDialog = true,fnInitializeEditAccountItem(item)">
                            <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Edit</v-list-item-content>
                        </v-list-item>
                        <v-list-item id="AutoTest_IP432" link @click.stop="RemoveAccountDialog = true,RemoveAccountItem=item">
                            <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Remove</v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-menu>
            </template>
        </v-data-table>

        <!--Add Account Dialog-->
        <v-dialog v-model="AddAccountsDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Create Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP433" text="" @click="AddAccountsDialog=false,fnInitializeAddAccount()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new account.</h4>
                    <v-row justify="center">
                        <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                            <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Account Name" v-model="AddAccount.AccountName">
                            </v-text-field>
                            <v-textarea outlined="" dense="" name="" label="Description" v-model="AddAccount.Description"></v-textarea>
                            <v-text-field outlined="" dense="" label="Contact Name" v-model="AddAccount.ContactName">
                            </v-text-field>
                            <v-text-field outlined="" dense="" type="number" label="Contact Number" v-model="AddAccount.ContactPhone"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>
                            <v-text-field required outlined="" dense="" label="Email" type="email" :rules="emailRules" v-model="AddAccount.ContactEmail">
                            </v-text-field>
                            <v-textarea outlined="" dense="" name="" label="Address" value="" v-model="AddAccount.ContactAddress"></v-textarea>
                            <v-row>
                                <v-col class="py-0">
                                    <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="AddAccount.Pincode"
                                                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                  onKeyPress="if(this.value.length==10) return false;" min="0">
                                    </v-text-field>
                                </v-col>
                                <v-col class="py-0">
                                    <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="AddAccount.BankIFSCCode">
                                    </v-text-field>
                                </v-col>
                            </v-row>
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="AddAccount.BankName" @change="fnGetBranchList(AddAccount.BankName)">
                            </v-select><br />
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="AddAccount.BankBranch" @change="fnGetIFSC(AddAccount.BankName,AddAccount.BankBranch)">
                            </v-select>
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="AddAccount.BankAccountType">
                            </v-select>
                            <v-row>
                                <v-col class="py-0">
                                    <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No."
                                                  v-model="AddAccount.BankAccountNumber" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                  onKeyPress="if(this.value.length==50) return false;" min="0">
                                    </v-text-field>
                                </v-col>
                                <v-col class="py-0">
                                    <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="AddAccount.PAN">
                                    </v-text-field>
                                </v-col>
                            </v-row>
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-link-box</v-icon> Link the user
                            </span>
                            <v-avatar size="27" class="team-avatar">
                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                            </v-avatar>
                            <v-menu max-width="300" absolute="" v-model="AddAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
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
                                        <v-btn id="AutoTest_IP434" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddAccountUserPopUp=false,AddAccount.UserId=0,selectedUser=0">
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
                                                <Success-Button id="AutoTest_IP435" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddAccountUserPopUp=false,AddAccount.UserId=selectedUser"></Success-Button>
                                            </v-col>
                                        </v-row>
                                    </v-card-actions>
                                </v-card>
                                <!--_______________________________________-->
                            </v-menu>

                            <v-checkbox label="80G eligible" color="#52B962" multiple-active="" v-model="AddAccount.Is80Geligible"></v-checkbox>
                        </v-col>

                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP436" class="float-right" @click.native="AddAccountsDialog=false,fnInitializeAddAccount()"></Cancel-Button-Outlined-Sm>
                        <v-btn id="AutoTest_IP437" v-bind:disabled="fnCheckMandatory()" class="elevation-0 float-right mr-4 primary-btn-light" height="36" width="122" @click="fnAddAccount()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="AddAccountSuccessSnackbar" top="" color="primary">
            {{ AddAccountMessage }}
        </v-snackbar>

        <!-- Edit Account Dialog-->
        <v-dialog v-model="EditAccountDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP438" text="" @click="EditAccountDialog=false,fnInitializeEditAccount()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Edit the basic details of the account.</h4>
                    <v-row justify="center">
                        <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                            <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Account Name" v-model="EditAccount.AccountName">
                            </v-text-field>
                            <v-textarea outlined="" dense="" name="" label="Description" v-model="EditAccount.Description"></v-textarea>
                            <v-text-field outlined="" dense="" label="Contact Name" v-model="EditAccount.ContactName">
                            </v-text-field>
                            <v-text-field outlined="" dense="" type="number" label="Contact Number" v-model="EditAccount.ContactPhone"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>
                            <v-text-field required outlined="" dense="" label="Email" type="email" :rules="emailRules" v-model="EditAccount.ContactEmail">
                            </v-text-field>
                            <v-textarea outlined="" dense="" name="" label="Address" value="" v-model="EditAccount.ContactAddress"></v-textarea>
                            <v-row>
                                <v-col class="py-0">
                                    <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="EditAccount.Pincode"
                                                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                  onKeyPress="if(this.value.length==10) return false;" min="0">
                                    </v-text-field>
                                </v-col>
                                <v-col class="py-0">
                                    <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="EditAccount.BankIFSCCode">
                                    </v-text-field>
                                </v-col>
                            </v-row>
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="EditAccount.BankName" @change="fnGetBranchList(EditAccount.BankName),EditAccount.BankIFSCCode=''">
                            </v-select>
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="EditAccount.BankBranch" @change="fnGetIFSC(EditAccount.BankName,EditAccount.BankBranch)">
                            </v-select>
                            <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="EditAccount.BankAccountType">
                            </v-select>
                            <v-row>
                                <v-col class="py-0">
                                    <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No."
                                                  v-model="EditAccount.BankAccountNumber" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                  onKeyPress="if(this.value.length==10) return false;" min="0">
                                    </v-text-field>
                                </v-col>
                                <v-col class="py-0">
                                    <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="EditAccount.PAN">
                                    </v-text-field>
                                </v-col>
                            </v-row>
                            <v-checkbox label="80G eligible" color="#52B962" multiple-active="" v-model="EditAccount.Is80Geligible"></v-checkbox>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP439" class="float-right" @click.native="EditAccountDialog=false,fnInitializeEditAccount()"></Cancel-Button-Outlined-Sm>
                        <v-btn id="AutoTest_IP440" v-bind:disabled="fnCheckMandatoryEditAccount()" class="elevation-0 float-right mr-4 primary-btn-light" height="36" width="122" @click="fnEditAccount()">
                            Save
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="EditAccountSuccessSnackbar" top="" color="primary">
            {{ EditAccountMessage }}
        </v-snackbar>

        <!-- Remove Account Terms-->
        <v-dialog v-model="RemoveAccountDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP441" text="" @click="RemoveAccountDialog=false,RemoveAccountItem=null">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-4">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col v-if="RemoveAccountItem!=null" cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this account ?</h4>
                                        </v-col>
                                        <v-col v-else cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove below accounts ?</h4>
                                        </v-col>
                                        <v-col v-if="RemoveAccountItem">
                                            <h4 class="text-center heading-3">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                </v-avatar>
                                                {{ RemoveAccountItem.AccountName }}
                                            </h4>
                                        </v-col>
                                        <v-col v-else>
                                            <v-row v-for="selecteditem in selectedAccountList" :key="selecteditem.AccountId">
                                                <v-col md="12">
                                                    <h4 class="text-center heading-3">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                        </v-avatar>
                                                        {{ selecteditem.AccountName }}
                                                    </h4>
                                                </v-col>
                                            </v-row>
                                        </v-col>

                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP442" color="secondary elevation-0" class="mr-2" @click="fnRemoveAccount()">Remove</v-btn>
                                            <v-btn id="AutoTest_IP443" outlined color="grey" @click="RemoveAccountDialog=false,RemoveAccountItem=null">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="RemoveAccountSnackbar" top="" color="primary">
            {{ RemoveAccountMessage }}
        </v-snackbar>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </v-card>
</template>
<script>
    import AddButtonFab from '@/components/add-button-fab.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'
    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import BulkActionButtonOutlined from '@/components/bulk-action-button-outlined.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
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
                 screenAccess: 0,
                RWAccess: 0,
                snackbarMsg: false,
                Msg: "",
                NotificationSelectPopUp: false,
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
                AccountListHeaders: [
                    { text: 'ID', align: ' d-none', value: 'AccountId' },
                    { text: "Name", value: "AccountName" },
                    { text: "Contact Name", value: "ContactName" },
                    { text: "Email", value: "ContactEmail" },
                    { text: "Bank", value: "BankName" },
                    { text: "Account Type", value: "BankAccountType" },
                    { text: "Account No.", value: "BankAccountNumber" },
                    { text: "IFSC Code", value: "BankIFSCCode" },
                    { text: "Action", value: "AccountsActions" },
                ],
                AccountList: [],
                selectedAccountList: [],
                SearchAccountList: "",
                //Create Account dialog parameters
                AddAccountsDialog: false,
                AddAccountSuccessSnackbar: false,
                AddAccountUserPopUp: false,
                AddAccount: {
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
                selectedUser: 0,
                AddAccountMessage: "",
                //common parameters for Add and Edit Account
                SelectBankItems: [],
                SelectBankBranchItems: [],
                SelectBankAccountTypeItems: [],
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],

                //Edit Account parameters
                EditAccount: {
                    AccountId: 0,
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
                EditAccountDialog: false,
                EditAccountSuccessSnackbar: false,
                EditAccountMessage: "",

                //Remove Account parameters
                RemoveAccountDialog: false,
                RemoveAccountSnackbar: false,
                RemoveAccountMessage: "",
                RemoveAccountItem: null,
            }
        },

        components: {
            'Add-Button-Fab': AddButtonFab,
            'Success-Button-Md': SuccessButtonMd,
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Primary-Button': PrimaryButton,
            'Bulk-Action-Button-Outlined': BulkActionButtonOutlined,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,

        }, mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
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
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("Type", "Donor");
                var result = await objManageProjectAccount.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectUserList = result.data;
                    console.log("one user name =",result.data[0].UserName);
                }
            },
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
                    this.Msg =("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                     this.snackbarMsg = true;
                }
            },
            fnInitializeAddAccount() {
                this.AddAccount.AccountName = "";
                this.AddAccount.Description = "";
                this.AddAccount.ContactName = "";
                this.AddAccount.ContactPhone = 0;
                this.AddAccount.ContactEmail = "";
                this.AddAccount.ContactAddress = "";
                this.AddAccount.Pincode = 0;
                this.AddAccount.BankIFSCCode = "";
                this.AddAccount.BankName = "";
                this.AddAccount.BankBranch = "";
                this.AddAccount.BankAccountType = "";
                this.AddAccount.BankAccountNumber = 0;
                this.AddAccount.PAN = "";
                this.AddAccount.ProjectId = 0;
                this.AddAccount.UserId = 0;
                this.AddAccount.Is80Geligible = 0;
                this.selectedUser = 0;
            },
            async fnAddAccount() {
                if (this.AddAccount.ContactEmail.length > 0) {
                    const pattern = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                    var emailtest = pattern.test(this.AddAccount.ContactEmail);
                    if (!emailtest) {
                        this.Msg = ("Please enter valid E-mail");
                        this.snackbarMsg = true;
                        return;
                    }
                }
                this.AddAccountsDialog = false;
                this.AddAccountSuccessSnackbar = true;
                var data = new FormData();
                this.AddAccount.ProjectId = this.projectInfo.ProjectId;
                data.append("AccountName", this.AddAccount.AccountName);
                data.append("Description", this.AddAccount.Description);
                data.append("ContactName", this.AddAccount.ContactName);
                data.append("ContactPhone", this.AddAccount.ContactPhone);
                data.append("ContactEmail", this.AddAccount.ContactEmail);
                data.append("ContactAddress", this.AddAccount.ContactAddress);
                data.append("Pincode", this.AddAccount.Pincode);
                data.append("BankIFSCCode", this.AddAccount.BankIFSCCode);
                data.append("BankName", this.AddAccount.BankName);
                data.append("BankBranch", this.AddAccount.BankBranch);
                data.append("BankAccountType", this.AddAccount.BankAccountType);
                data.append("BankAccountNumber", this.AddAccount.BankAccountNumber);
                data.append("PAN", this.AddAccount.PAN);
                data.append("ProjectId", this.AddAccount.ProjectId);
                data.append("UserId", this.AddAccount.UserId);
                data.append("Is80Geligible", this.AddAccount.Is80Geligible);
                var result = await objManageProjectAccount.AddAccount(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.AddAccountMessage = "Successfully added account to the project";
                    this.fnGetProjectAccountsByID();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.AddAccountMessage = "Failed to add account to the project";
                }
            },
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
            async fnGetBranchList(bankname) {
                var data = new FormData();
                data.append("BankName", bankname);
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
            async fnGetIFSC(bankname,branchname) {
                var data = new FormData();
                data.append("BankName", bankname);
                data.append("BranchName", branchname);
                var result = await objManageProjectAccount.GetIFSC(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    if(this.AddAccountsDialog==true)
                        this.AddAccount.BankIFSCCode = result.data;
                    else
                        this.EditAccount.BankIFSCCode = result.data;
                } else {
                    this.Msg = ("Failed to get details of IFSC code.");
                    this.snackbarMsg = true;
                }
            },
            async fnGetAccountTypeList() {
                var result = await objManageProjectAccount.GetAccountTypeList();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SelectBankAccountTypeItems = result.data;
                    //alert("Account Type List = " + this.SelectBankAccountTypeItems.length);
                } else {
                    this.Msg =("Failed to get details of Bank Names.");
                    this.snackbarMsg = true;
                }
            },
            fnCheckMandatory() {
                if (this.AddAccount.AccountName == "" ||
                    this.AddAccount.BankIFSCCode == "" ||
                    this.AddAccount.BankName == "" ||
                    this.AddAccount.BankBranch == "" ||
                    this.AddAccount.BankAccountType == "" ||
                    this.AddAccount.BankAccountNumber == 0 ||
                    this.AddAccount.PAN == "" ||
                    this.AddAccount.UserId == 0)
                    return true;
                else return false;
            },
            //edit Account functions
            fnInitializeEditAccount() {
                this.EditAccount.AccountId = 0;
                this.EditAccount.AccountName = "";
                this.EditAccount.Description = "";
                this.EditAccount.ContactName = "";
                this.EditAccount.ContactPhone = 0;
                this.EditAccount.ContactEmail = "";
                this.EditAccount.ContactAddress = "";
                this.EditAccount.Pincode = 0;
                this.EditAccount.BankIFSCCode = "";
                this.EditAccount.BankName = "";
                this.EditAccount.BankBranch = "";
                this.EditAccount.BankAccountType = "";
                this.EditAccount.BankAccountNumber = 0;
                this.EditAccount.PAN = "";
                this.EditAccount.ProjectId = 0;
                this.EditAccount.UserId = 0;
                this.EditAccount.Is80Geligible = 0;
            },
            fnInitializeEditAccountItem(item) {
                this.fnGetBranchList(item.BankName);
                this.EditAccount.AccountId = item.AccountId;
                this.EditAccount.AccountName = item.AccountName;
                this.EditAccount.Description = item.Description;
                this.EditAccount.ContactName = item.ContactName;
                this.EditAccount.ContactPhone = item.ContactPhone;
                this.EditAccount.ContactEmail = item.ContactEmail;
                this.EditAccount.ContactAddress = item.ContactAddress;
                this.EditAccount.Pincode = item.Pincode;
                this.EditAccount.BankIFSCCode = item.BankIFSCCode;
                this.EditAccount.BankName = item.BankName;
                this.EditAccount.BankBranch = item.BankBranch;
                this.EditAccount.BankAccountType = item.BankAccountType;
                this.EditAccount.BankAccountNumber = item.BankAccountNumber;
                this.EditAccount.PAN = item.PAN;
                this.EditAccount.ProjectId = item.ProjectId;
                this.EditAccount.UserId = item.UserId;
                this.EditAccount.Is80Geligible = item.Is80Geligible;
            },
            fnCheckMandatoryEditAccount() {
                if (this.EditAccount.AccountName == "" ||
                    this.EditAccount.BankIFSCCode == "" ||
                    this.EditAccount.BankName == "" ||
                    this.EditAccount.BankBranch == "" ||
                    this.EditAccount.BankAccountType == "" ||
                    this.EditAccount.BankAccountNumber == 0 ||
                    this.EditAccount.PAN == "" ||
                    this.EditAccount.UserId == 0)
                    return true;
                else return false;
            },
            async fnEditAccount() {
                if (this.EditAccount.ContactEmail && this.EditAccount.ContactEmail.length > 0) {
                    const pattern = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                    var emailtest = pattern.test(this.EditAccount.ContactEmail);
                    if (!emailtest) {
                        this.Msg= ("Please enter valid E-mail");
                        this.snackbarMsg = true;
                        return;
                    }
                }
                this.EditAccountDialog = false;
                this.EditAccountSuccessSnackbar = true;
                var data = new FormData();
                data.append("AccountId", this.EditAccount.AccountId);
                data.append("AccountName", this.EditAccount.AccountName);
                data.append("Description", this.EditAccount.Description);
                data.append("ContactName", this.EditAccount.ContactName);
                data.append("ContactPhone", this.EditAccount.ContactPhone);
                data.append("ContactEmail", this.EditAccount.ContactEmail);
                data.append("ContactAddress", this.EditAccount.ContactAddress);
                data.append("Pincode", this.EditAccount.Pincode);
                data.append("BankIFSCCode", this.EditAccount.BankIFSCCode);
                data.append("BankName", this.EditAccount.BankName);
                data.append("BankBranch", this.EditAccount.BankBranch);
                data.append("BankAccountType", this.EditAccount.BankAccountType);
                data.append("BankAccountNumber", this.EditAccount.BankAccountNumber);
                data.append("PAN", this.EditAccount.PAN);
                data.append("ProjectId", this.EditAccount.ProjectId);
                data.append("UserId", this.EditAccount.UserId);
                data.append("Is80Geligible", this.EditAccount.Is80Geligible);
                var result = await objManageProjectAccount.EditAccount(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.EditAccountMessage = "Successfully updated the account details";
                    this.fnGetProjectAccountsByID();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.EditAccountMessage = "Failed to update the account details";
                }
            },
            //remove account functions
            async fnRemoveAccount() {
                this.RemoveAccountDialog = false;
                this.RemoveAccountSuccessSnackbar = true;

                var data = new FormData();
                var accidlist = [];
                if (this.RemoveAccountItem) {
                    data.append("AccountIdList", this.RemoveAccountItem.AccountId);
                } else {
                    for (var i = 0; i < this.selectedAccountList.length; i++) {
                        accidlist.push(this.selectedAccountList[i].AccountId);
                    }
                    var accids = accidlist.toString();
                    data.append("AccountIdList", accids);
                }
                var result = await objManageProjectAccount.RemoveAccount(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.RemoveAccountMessage = "Successfully deleted the account details";
                    this.RemoveAccountItem = null;
                    this.selectedAccountList = [];
                    this.fnGetProjectAccountsByID();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.RemoveAccountMessage = "Failed to deleted the account details";
                }
            },
        },
    }
</script>