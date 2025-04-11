<template>
    <div>
        <v-card class="pb-1">
            <v-card-title style="height:60px">
                <h5 class="text-left page-head">
                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi mdi-account-cash-outline</v-icon>
                    </v-avatar>
                    My Finance
                </h5>
            </v-card-title>
            <v-card class="elevation-0">
                <!--<v-card-title style="height:50px">
                Fund and Fund Utilized
            </v-card-title>-->
                <v-card-text style="height:100%" class="pt-0">
                    <v-row>
                        <!-- Chart-->
                        <v-col md="6">
                            <v-card class="custom-card-1">
                                <v-card-title style="height:50px">
                                    Fund and Fund Utilized
                                </v-card-title>
                                <v-card-text style="height:300px">
                                    <v-row v-show="ProjectList.length > 0">
                                        <v-col md="6">
                                            <v-row>
                                                <v-col>
                                                    <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                        <div style="width: 100%; height: 40px; position: absolute; top: 50%; left: 0; margin-top: -10px; line-height:19px; text-align: center;">
                                                            {{TotalFundUtilizedPerc}}%
                                                        </div>
                                                        <canvas id="myChart" style="width:175px; height:150px"></canvas>
                                                    </div>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col>
                                                    <h5 class="heading-3-dark text-center">
                                                        Total budget<br />
                                                        <span class="bold">Rs. {{TotalFundedAmount}}</span>
                                                    </h5>
                                                </v-col>
                                                <v-col>
                                                    <h5 class="heading-3-dark text-center">
                                                        Fund Utilized<br />
                                                        <span class="bold">Rs. {{TotalUtilizedFund}}</span>
                                                    </h5>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col md="6">
                                            <v-list class="v-scrolling-div" style="max-height:180px">
                                                <v-list-item-group v-model="item" color="primary">
                                                    <v-list-item v-for="(item, i) in ProjectList"
                                                                 :key="i" dense>
                                                        <v-list-item-avatar class="mr-0">
                                                            <v-avatar :color="item.color" size="20" tile="tile">
                                                            </v-avatar>
                                                        </v-list-item-avatar>
                                                        <v-list-item-content>
                                                            <v-list-item-title v-text="item.text"> </v-list-item-title>
                                                        </v-list-item-content>
                                                        <span class="bold">:  Rs.</span>
                                                        <v-list-item-action class="ml-0 pt-1">
                                                            <v-list-item-title v-text="item.FundedAmount"></v-list-item-title>
                                                        </v-list-item-action>
                                                    </v-list-item>
                                                </v-list-item-group>
                                            </v-list>
                                            <v-row class="mt-3 ml-2">
                                                <h5 class="heading-3-dark text-center">
                                                    Unutilized Fund: <span class="bold">Rs {{UnutilizedFund}}</span>
                                                </h5>
                                            </v-row>

                                        </v-col>
                                    </v-row>
                                    <v-row v-show="ProjectList.length == 0" justify="center">
                                        <v-col class="text-center">
                                            <p class="mt-6 pt-6">
                                                <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                            </p>
                                            <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <!-- Project Expenses-->
                        <v-col md="6">
                            <v-card class="custom-card-1">
                                <v-card-title style="height:50px">
                                    Project Expenses
                                </v-card-title>
                                <v-card-text style="height:300px; padding-left:0px;padding-right:0px;">
                                    <v-row no-gutters v-show="ProjectList.length > 0">
                                        <v-data-table :headers="ProjectExpensesTableheaders"
                                                      :items="ProjectExpenseTableItems"
                                                      v-model="selectedProjectID"
                                                      item-key="ProjectId"
                                                      class="elevation-0"
                                                      hide-default-footer
                                                      fixed-header
                                                      height="250px" style="width:100%">

                                            <!--Projects list column Name-->
                                            <!--<template v-slot:item.Title="{item}">
        <v-list class="py-0 table-v-list">
            <v-list-item class="my-0 px-0">
                <v-list-item-icon>
                    <v-avatar size="30" color="#fff" class="elevation-1">
                        <v-icon size="20" color="primary">mdi-folder</v-icon>
                    </v-avatar>
                </v-list-item-icon>
                <v-list-item-content>
                    {{ item.Title }}
                </v-list-item-content>
            </v-list-item>
        </v-list>
    </template>-->
                                            <template v-slot:item.ProjectName="{item}">
                                                <div class="pa-0 text-limit" style="width:100px">
                                                    <v-avatar size="25" color="#fff" class="elevation-1">
                                                        <v-icon size="15" color="primary">mdi-folder</v-icon>
                                                    </v-avatar>
                                                    {{ item.ProjectName }}
                                                </div>
                                            </template>
                                            <template v-slot:item.FundUtilized="{item}">
                                                <div class="pa-0 text-limit" style="width:120px">
                                                    {{ item.FundUtilized }}
                                                </div>
                                            </template>
                                            <template v-slot:item.FundedAmount="{item}">
                                                <div class="pa-0 text-limit" style="width:120px">
                                                    {{ item.FundedAmount }}
                                                </div>
                                            </template>
                                            <template v-slot:item.UtilizedFund="{item}">
                                                <div class="pa-0 text-limit" style="width:100px">
                                                    {{ item.UtilizedFund }}
                                                </div>
                                            </template>
                                        </v-data-table>
                                    </v-row>
                                    <v-row v-show="ProjectList.length == 0" justify="center">
                                        <v-col class="text-center">
                                            <p class="mt-6 pt-6">
                                                <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                            </p>
                                            <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-card-text>
            </v-card>
            <v-card class="mx-4 mb-4">
                <v-card-title style="height:60px">
                    Accounts
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_V25" height="39" color="success-btn" dark class="elevation-0 mr-4" @click="AddAccountsDialog = true, fnTestFunct()" v-bind:disabled ="!screenAccess">
                        Create Account
                    </v-btn>
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

                            <v-list-item id="AutoTest_V26" @click.stop="fnBulkRemoveClick()">
                                <v-list-item-icon>
                                    <v-icon>mdi-trash-can-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Remove
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                        <v-dialog v-model="RemoveBulkAccountDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head py-4">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Remove Accounts
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_V27" text="" @click="RemoveBulkAccountDialog=false">
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
                                                            <h4 class="text-center heading-3">Are you sure you want to remove this accounts ?</h4>
                                                        </v-col>

                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                            <Secondary-Button id="AutoTest_V28" class="btn-122x36 mr-4" title="Remove" @click.native="fnRemoveAccount()"></Secondary-Button>
                                                            <Default-Button-Outlined id="AutoTest_V29" class="btn-122x36" title="Cancel" @click.native="RemoveBulkAccountDialog=false"></Default-Button-Outlined>
                                                        </v-col>
                                                    </v-row>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                    </v-menu>
                    <!--Search-->
                    <v-text-field label="Search" class="table-search" v-model="searchProject"
                                  dense prepend-inner-icon="mdi-magnify"
                                  outlined hide-details></v-text-field>
                </v-card-title>
                <v-card-text style="padding-left:0px; padding-right:0px">
                    <v-row v-if="LedgerBookTableItems.length>0">
                        <v-col cols="12" md="12" sm="12" xs="12">
                            <v-data-table :headers="LedgerBookTableheaders"
                                          :items="LedgerBookTableItems"
                                          hide-default-footer
                                          :search="searchProject"
                                          v-model="selectedAccountList"
                                          show-select=""
                                          item-key="AccountId"
                                          fixed-header
                                          height="50vh">
                                <template v-slot:item.AccountName="{item}">
                                    <v-list class="py-0 table-v-list">
                                        <div class="pa-0 text-limit" style="width:135px">
                                            <v-avatar size="25" color="#fff" class="elevation-1">
                                                <v-icon size="15" color="primary">mdi-book</v-icon>
                                            </v-avatar>
                                            {{ item.AccountName }}
                                        </div>
                                    </v-list>
                                </template>
                                <template v-slot:item.ContactName="{item}">
                                    <div v-if="item.ContactName" class="pa-0 text-limit" style="width:110px">
                                        {{ item.ContactName }}
                                    </div>
                                    <div v-else>N/A</div>
                                </template>
                                <template  v-slot:item.ContactEmail="{item}">
                                    <div v-if="item.ContactEmail" class="pa-0 text-limit" style="width:135px">
                                        {{ item.ContactEmail }}
                                    </div>
                                    <div v-else>N/A</div>
                                </template>
                                <template v-slot:item.BankName="{item}">
                                    <div  v-if="item.BankName" class="pa-0 text-limit" style="width:150px">
                                        {{ item.BankName }}
                                    </div>
                                    <div v-else>N/A</div>
                                </template>
                                <template v-slot:item.BankAccountType="{item}">
                                    <div v-if="item.BankAccountType" class="pa-0 text-limit" style="width:110px">
                                        {{ item.BankAccountType }}
                                    </div>
                                </template>
                                <template v-slot:item.BankAccountNumber="{item}">
                                    <div v-if="item.BankAccountNumber" class="pa-0 text-limit" style="width:100px">
                                        {{ item.BankAccountNumber }}
                                    </div>
                                    <div v-else>N/A</div>
                                </template>
                                <template v-slot:item.BankIFSCCode="{item}">
                                    <div v-if="item.BankIFSCCode"  class="pa-0 text-limit" style="width:100px">
                                        {{ item.BankIFSCCode }}
                                    </div>
                                    <div v-else>N/A</div>
                                </template>
                                <template v-slot:item.LedgerTableActions="{item}">
                                    <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                        <template v-slot:activator="{ on }">
                                            <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                <v-icon>mdi-dots-vertical</v-icon>
                                            </v-btn>
                                        </template>
                                        <v-list color="white" light="" width="auto" class="action-button-list">
                                            <v-list color="white" light width="auto" class="action-button-list">
                                                <v-list-item id="AutoTest_V30" link @click.stop="EditAccountDialog = true,fnInitializeEditAccountItem(item)">
                                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Edit</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_V31" link @click.stop="RemoveAccountDialog = true,RemoveAccountItem=item">
                                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Remove</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-list>

                                    </v-menu>
                                </template>
                            </v-data-table>


                            <!-- Edit Account Dialog-->
                            <v-dialog v-model="EditAccountDialog" width="800">
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Edit Account
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_V32" text="" @click="EditAccountDialog=false,fnInitializeEditAccount()">
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
                                                <v-text-field outlined="" dense="" type="number" label="Contact Number" v-model="EditAccount.ContactPhone" onkeydown="javascript: return event.keyCode == 69 ? false : true">
                                                </v-text-field>
                                                <v-text-field required outlined="" dense="" label="Email" type="email" :rules="emailRules" v-model="EditAccount.ContactEmail">
                                                </v-text-field>
                                                <v-textarea outlined="" dense="" name="" label="Address" value="" v-model="EditAccount.ContactAddress"></v-textarea>
                                                <v-row>
                                                    <v-col class="py-0">
                                                        <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="EditAccount.Pincode">
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
                                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No." v-model="EditAccount.BankAccountNumber">
                                                        </v-text-field>
                                                    </v-col>
                                                    <v-col class="py-0">
                                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="EditAccount.PAN">
                                                        </v-text-field>
                                                    </v-col>
                                                </v-row>
                                                <v-checkbox label="80G eligible" color="success-check-box" multiple-active="" v-model="EditAccount.Is80Geligible"></v-checkbox>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-card-actions class="pb-4">
                                        <v-col>
                                            <Cancel-Button-Outlined-Sm id="AutoTest_V33" class="float-right" @click.native="EditAccountDialog=false,fnInitializeEditAccount()"></Cancel-Button-Outlined-Sm>
                                            <Secondary-Button id="AutoTest_V34" class="btn-122x36 mr-4 float-right" title="Save" @click.native="fnEditAccount()" v-bind:disabled="fnCheckMandatoryEditAccount()"></Secondary-Button>
                                        </v-col>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>

                            <!-- Remove Account Terms-->
                            <v-dialog v-model="RemoveAccountDialog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Account
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_V35" text="" @click="RemoveAccountDialog=false,RemoveAccountItem=null">
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
                                                                <Secondary-Button id="AutoTest_V36" class="btn-122x36 mr-4" title="Remove" @click.native="fnRemoveAccount()"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_V37" class="btn-122x36" title="Cancel" @click.native="RemoveAccountDialog=false,RemoveAccountItem=null"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>

                        </v-col>
                    </v-row>
                    <v-row v-else>
                        <v-col cols="12" sm="12" md="12">
                            <v-row :justify="justifyCenter">
                                <v-col md="12" class="text-center">
                                    <p class="mt-2 pt-2">
                                        <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="80" width="80" />
                                    </p>
                                    <p class="heading-4-dark font-weight-medium pt-0">No accounts available</p>
                                    <v-btn id="AutoTest_V38" height="32" color="success-btn" dark class="elevation-0" @click="AddAccountsDialog = true" v-bind:disabled="!screenAccess">
                                        Create Account
                                    </v-btn>
                                </v-col>
                            </v-row>
                        </v-col>
                    </v-row>
                    <!--Add Account Dialog-->
                    <v-dialog v-model="AddAccountsDialog" width="750">
                        <v-card>
                            <v-card-title primary-title="" class="page-head pb-0">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                    <v-icon size="20" color="primary">mdi-cash</v-icon>
                                </v-avatar>
                                Create Account
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_V39" text="" @click="AddAccountsDialog=false,fnInitializeAddAccount()">
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
                                        <v-text-field outlined="" dense="" type="number" label="Contact Number" v-model="AddAccount.ContactPhone" onkeydown="javascript: return event.keyCode == 69 ? false : true">
                                        </v-text-field>
                                        <v-text-field required outlined="" dense="" label="Email" type="email" :rules="emailRules" v-model="AddAccount.ContactEmail">
                                        </v-text-field>
                                        <v-textarea outlined="" dense="" name="" label="Address" value="" v-model="AddAccount.ContactAddress"></v-textarea>
                                        <v-row>
                                            <v-col class="py-0">
                                                <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="AddAccount.Pincode">
                                                </v-text-field>
                                            </v-col>
                                            <v-col class="py-0">
                                                <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="AddAccount.BankIFSCCode">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                        <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="AddAccount.BankName" @change="fnGetBranchList(AddAccount.BankName)">
                                        </v-select>
                                        <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="AddAccount.BankBranch" @change="fnGetIFSC(AddAccount.BankName,AddAccount.BankBranch)">
                                        </v-select>
                                        <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="AddAccount.BankAccountType">
                                        </v-select>
                                        <v-row>
                                            <v-col class="py-0">
                                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No." v-model="AddAccount.BankAccountNumber">
                                                </v-text-field>
                                            </v-col>
                                            <v-col class="py-0">
                                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="AddAccount.PAN">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>

                                        <v-select class="mt-4" dense="" :items="Project" :item-text="i => i.ProjectName" :item-value="i => i.ProjectId" label="Select Project" outlined="" v-model="Projects">
                                        </v-select>
                                        <v-checkbox label="80G eligible" color="success-check-box" multiple-active="" v-model="AddAccount.Is80Geligible"></v-checkbox>
                                    </v-col>

                                </v-row>
                            </v-card-text>
                            <v-card-actions class="pb-4">
                                <v-col>
                                    <Cancel-Button-Outlined-Sm id="AutoTest_V40" class="float-right" @click.native="AddAccountsDialog=false,fnInitializeAddAccount()"></Cancel-Button-Outlined-Sm>
                                    <Secondary-Button id="AutoTest_V41" class="float-right btn-122x36 mr-4" title="Confirm" @click.native="fnAddAccount()" v-bind:disabled="fnCheckMandatory()"></Secondary-Button>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>

                </v-card-text>
            </v-card>
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
    import DoughnutChart from '@/components/DoughnutChart.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
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
      var objManageProjectAccount;
    async function importAccountscript() {
        return Promise.all([
            import("../../BL/ManageProjectAccount.js").then(mod => {
                objManageProjectAccount = new mod.ManageProjectAccount();
            })
        ]);
    };


    export default {
        data() {
            return {
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
               offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeId: 0,
                ProjectExpensesTableheaders: [
                    { text: "Title", align: "left", value: "ProjectName" },
                    { text: "Fund utilized(%)", value: "FundUtilized" },
                    { text: "Funded Amount", value: "FundedAmount" },
                    { text: "Utilized Fund", value: "UtilizedFund"},

                ],
                ProjectExpenseTableItems: [],
                TotalFundUtilizedPerc: 0,
                TotalUtilizedFund:0,
                TotalFundedAmount:0 ,
                 UnutilizedFund:0,
                selectedProjectID: [],
                selectedAccountList:[],
                searchProject: "",
                ProjectId: "",
                ProjectName: "",
                State: "",
                Project: [],
                Projects:[],
                dlgSetasTemplate: false,
                
                SetasTempFileInput: null,
                RemoveAccountDialog:false,
                EditAccountDialog:false,
                RemoveBulkAccountDialog:false,
                dlgCommitFund : false,
                dlgWhishlist : false,
                CommitedFund: "",
                RemoveBulkVentureDailog: false,
                
                filterMenu: false,
                MyVentureFilterList: ['Funded Project', 'Wishlist'],
                filterMyVenture: "",
                BudgetChartData1 :[],

                BudgetChartData: {
                    //labels: ['Donor 1 : Rs. 60000', 'Donor 2 : Rs. 40000',],
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
                        position: 'right',
                        fullWidth: false,
                        align: 'start',
                    }
                },
                item :1,
                items: [],
                LedgerBookTableheaders: [
                    { text: "Name", value: "AccountName" },
                    { text: "Contact Name", value: "ContactName" },
                    { text: "Email", value: "ContactEmail" },
                    { text: "Bank", value: "BankName" },
                    { text: "Account Type", value: "BankAccountType" },
                    { text: "Account No.", value: "BankAccountNumber" },
                    { text: "IFSC Code", value: "BankIFSCCode" },
                    { text: "Action", align:"center", value: "LedgerTableActions",sortable:"false",width:"90px" },
                ],
                LedgerBookTableItems: [],
             //Create Account dialog parameters
                AddAccountsDialog: false,
                
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
                
                EditAccountMessage: "",
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],

                
                SelectBankItems: [],
                SelectBankBranchItems: [],
                SelectBankAccountTypeItems: [],

                //Remove Account parameters
                RemoveAccountDialog: false,
                
                RemoveAccountMessage: "",
                RemoveAccountItem: null,

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
                mycolor: '#'+(Math.random()*0xFFFFFF<<0).toString(16)  ,
                ProjectList :[],
                tile : true,
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
            await importAccountscript();
            await this.GetTotalProjectExpenses();
            await this.GetProjectExpenses();
            await this.fnGetProjectAccountsByID();
            await this.GetProjectListOfVenture();
            await this.fnGetBankList();
            await this.fnGetAccountTypeList();
        },
        methods: {
                      generator: function(){
                        this.mycolor = '#'+(Math.random()*0xFFFFFF<<0).toString(16);

                      }  ,

            resetValidation() {
                this.$refs.form.resetValidation()
            },
           async GetTotalProjectExpenses() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objVenture.GetTotalProjectExpenses(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                this.TotalFundUtilizedPerc= result.data[0].TotalFundUtilized;
                this.TotalUtilizedFund=result.data[0].TotalUtilizedFund;
                this.TotalFundedAmount=result.data[0].TotalFundedAmount;
                this.UnutilizedFund= result.data[0].UnutilizedFund;

                    }
            },
            async GetProjectExpenses() {

                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objVenture.GetProjectExpenses(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.ProjectExpenseTableItems = result.data;
                        this.items=  result.data;
                        this.fnFetchProjectData(this.items);
                    }
            },
         async GetProjectListOfVenture() {

                var result = await objVenture.GetProjectListOfVenture();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.Project = result.data;
                    }
            },
           async fnFetchProjectData(ListItem){
                 if (ListItem.length > 0){

                    var i =0;
                    for (i =0 ; i<=  ListItem.length-1 ; i++){
                             this.generator();
                             this.ProjectList.push({text:ListItem[i].ProjectName, color:this.mycolor, FundedAmount:ListItem[i].FundedAmount})  ;
                        }
                       }
                       this.fnFillChartData();
                },
                        async fnFillChartData(){

                     if (this.ProjectList.length > 0){
                           var labelsArray = [];
                           var amountArray = [];
                           var labels =[];
                            for (var i = 0; i < this.ProjectList.length; i++) {
                                labelsArray.push(this.ProjectList[i] ? this.ProjectList[i].FundedAmount : '');
                                amountArray.push(this.ProjectList[i] ? this.ProjectList[i].color : '');
                                labels.push(this.ProjectList[i] ? this.ProjectList[i].text : '');
                            }

                   var ctx = document.getElementById("myChart")
                        var stateOverviewGraph2 = new Chart(ctx, {
                            type: 'doughnut',
                            data: {
                                labels: labels,
                                datasets: [{
                                    data: labelsArray,
                                    backgroundColor: amountArray
                                }],
                            },
                            options: {
                                legend: {
                                    display: false,
                                },
                            }
                        });


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
            fnBulkRemoveClick() {
                if (this.selectedAccountList.length > 0) {
                    this.RemoveBulkAccountDialog = true;
                } else {
                    this.Msg = ("Please select an account");
                    this.infoSnackbarMsg = true;
                }
            },
            FillBudgetData() {
                this.BudgetChartData = this.BudgetChartData1;
            },
             fnCheckMandatory() {

                if (this.AddAccount.AccountName == "" ||
                    this.AddAccount.BankIFSCCode == "" ||
                    this.AddAccount.BankName == "" ||
                    this.AddAccount.BankBranch == "" ||
                    this.AddAccount.BankAccountType == "" ||
                    this.AddAccount.BankAccountNumber == 0 ||
                    this.AddAccount.PAN == "" ||
                    this.Projects == "")
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
                    this.EditAccount.PAN == "")
                    return true;
                else return false;
            },
               async fnEditAccount() {
                if (this.EditAccount.ContactEmail && this.EditAccount.ContactEmail.length > 0) {
                    const pattern = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                    var emailtest = pattern.test(this.EditAccount.ContactEmail);
                    if (!emailtest) {
                        this.Msg = ("Please enter valid E-mail");
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }
                this.EditAccountDialog = false;
                //this.EditAccountSuccessSnackbar = true;
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
                    this.Msg = "Successfully updated the account details";
                    this.successSnackbarMsg = true;
                    this.fnGetProjectAccountsByID();
                } else {

                    this.Msg = "Failed to update the account details";
                    this.errorSnackbarMsg = true;
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
                this.Projects ="";
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
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }
                this.AddAccountsDialog = false;
                //this.AddAccountSuccessSnackbar = true;
                var data = new FormData();
                this.AddAccount.ProjectId = this.Projects;
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
                data.append("ProjectId", this.Projects);
                //data.append("UserId", this.AddAccount.UserId);
                data.append("Is80Geligible", this.AddAccount.Is80Geligible);
                var result = await objVenture.AddAccount(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {

                    this.Msg = "Successfully added account to the project";
                    this.successSnackbarMsg = true;
                    this.fnGetProjectAccountsByID();
                    this.fnInitializeAddAccount();
                } else {

                    this.Msg = "Failed to add account to the project";
                    this.errorSnackbarMsg = true;
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
                    this.errorSnackbarMsg = true;
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
                    this.errorSnackbarMsg = true;
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
                    this.errorSnackbarMsg = true;
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
                    this.Msg = ("Failed to get details of Bank Names.");
                    this.errorSnackbarMsg = true;
                }
            },
              //remove account functions
            async fnRemoveAccount() {
                this.RemoveAccountDialog = false;
                //this.RemoveAccountSuccessSnackbar = true;

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
                    this.Msg = "Successfully deleted the account details";
                    this.successSnackbarMsg = true;
                    this.RemoveAccountItem = null;
                    this.selectedAccountList = [];
                    this.fnGetProjectAccountsByID();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.Msg = "Failed to deleted the account details";
                    this.errorSnackbarMsg = true;
                }
            },
             async fnGetProjectAccountsByID() {

                var result = await objVenture.GetProjectAccounts();

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.LedgerBookTableItems = result.data;
                } else {
                    this.Msg = ("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },
    async fnTestFunct()
    {
            //alert("Clicked");
            //this.AddAccountsDialog = true;
    },

        },
        components: {
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Doughnut-Chart': DoughnutChart,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
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

