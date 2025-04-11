<template>
    <div class="py-0">
        <!------------------------------------- Project ledger book section -------------------------------->
        <v-card v-show="RouteName == 'ManageFinanceAccount'" class="elevation-0 px-0" >
            <v-card-title>
                <h5 id="AutoTest_F71" class="page-head" @click="ShowAccountLedger = false">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-library-books</v-icon>
                    </v-avatar>
                    Project Ledger Book
                </h5>
                <v-spacer></v-spacer>
                <v-btn id="AutoTest_F72" class="success-btn mr-4" depressed dark height="39" @click="AddPaymentDialog=true" v-bind:disabled="!screenAccess">
                    Add Payment
                </v-btn>
                <v-select style="max-width:150px;" class="mr-4" hide-selected="" hide-details="" dense=""
                          :items="ProjectExpenseTagList"
                          :menu-props="{ maxHeight: '200' }"
                          :item-text="i => i.TagName" :item-value="i => i.TagID"
                          v-model="filterExpense"
                          label="Select Expense Tag" @change="fnFilterPaymentList()"
                          outlined="">
                </v-select>
                <!-- Bulk Action-->
                <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                            Bulk Actions
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list v-if="selectedPaymentList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_F73" @click.stop="RemovePaymentDialog = true">
                            <v-list-item-icon>
                                <v-icon>mdi-trash-can-outline</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                Remove
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-menu>
                <!-- Export to excel-->
                <v-btn height="39"  color="grey" outlined="" class="elevation-0 mr-4" @click="fnExportPaymentFile()">
                    Export
                    <v-icon>mdi-chevron-down</v-icon>
                </v-btn>
                <!-- Search-->
                <v-text-field label="Search" class="table-search" v-model="SearchfilterPaymentList"
                              dense prepend-inner-icon="mdi-magnify"
                              outlined hide-details></v-text-field>
            </v-card-title>
            <v-card-text class="px-0" >
                <v-row v-if="filterPaymentList.length > 0" class="px-0">
                    <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
                        <v-data-table :headers="LedgerBookTableheaders"
                                      :items="filterPaymentList"
                                      item-key="PaymentId"
                                      :search="SearchfilterPaymentList"
                                      v-model="selectedPaymentList"
                                      show-select
                                      fixed-header
                                      height="55vh">
                            <template v-slot:item.PaymentActivityName="{item}">
                                <v-list class="py-0 table-v-list">
                                    <v-list-item class="my-0 px-0">
                                        <div class="pa-0 text-limit" style="width:120px">
                                            <v-avatar size="20" color="#fff" class="elevation-1">
                                                <v-icon size="12" color="primary">mdi-book</v-icon>
                                            </v-avatar>
                                            {{ item.PaymentActivityName }}
                                        </div>
                                    </v-list-item>
                                </v-list>
                            </template>
                            <template v-slot:item.TransactionId="{item}">
                                <div class="pa-0 text-limit" style="width:80px">
                                    {{ item.TransactionId }}
                                </div>
                            </template>
                            <template v-slot:item.TransactionDate="{item}">
                                <div class="pa-0 text-limit" style="width:80px">
                                    {{ formatDate(item.TransactionDate) }}
                                </div>
                            </template>
                            <template v-slot:item.TransactionAmount="{item}">
                                <div class="pa-0 text-limit" style="width:90px">
                                    <v-icon v-if="item.TransactionType=='Credit'" size="12" color="#0CBA2F">mdi-plus</v-icon>
                                    <v-icon v-else size="12" color="#BA0C0C">mdi-minus</v-icon>
                                    {{ item.TransactionAmount }}
                                </div>
                            </template>

                            <template v-slot:item.IncomeAccountId="{item}">
                                <div class="pa-0 text-limit" style="width:105px">
                                    {{ fnGetAccountName(item.IncomeAccountId) }}
                                </div>
                            </template>
                            <template v-slot:item.ExpenseAccountId="{item}">
                                <div class="pa-0 text-limit" style="width:90px">
                                    {{ fnGetAccountName(item.ExpenseAccountId) }}
                                </div>
                            </template>

                            <!--<template v-slot:item.ExpenseTags="{item}">
        <div v-for="tagItem in item.ExpenseTags">{{ tagItem.TagName }}</div>
    </template>-->

                            <template v-slot:item.TransactionType="{item}">
                                <div class="pa-0 text-limit" style="width:75px">
                                    <div v-if="item.TransactionType=='Credit'">
                                        Credit
                                    </div>
                                    <div v-else>
                                        Debit
                                    </div>
                                </div>
                            </template>

                            <template v-slot:item.TransactionDocName="{item}">
                                <div v-if="item.TransactionDocName" class="pa-0 text-limit handPointer" style="width:90px;" @click="DownloadDocument(item.TransactionDocName,'/Documents/7e23e75d-bd85-4c3d-82ad-a07585551986.jpg')">
                                    <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                                        <v-icon size="10" color="#fff">mdi-download</v-icon>
                                    </v-avatar> {{ item.TransactionDocName }}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.TransactionStatus="{item}">
                                <div v-if="item.TransactionStatus=='Approved'" style="width:90px" class="pa-0 text-limit">
                                    <v-avatar color="#0CBA2F" size="13" class="mr-1">
                                        <v-icon size="10" color="#fff">mdi-check</v-icon>
                                    </v-avatar>Approved
                                </div>
                                <div v-else style="width:70px" class="pa-0 text-limit">
                                    <v-avatar color="#BA0C0C" size="13" class="mr-1">
                                        <v-icon size="10" color="#fff">mdi-close</v-icon>
                                    </v-avatar>Rejected
                                </div>
                            </template>

                            <template v-slot:item.LedgerTableActions="{item}">
                                <div style="width:60px">
                                    <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                        <template v-slot:activator="{ on }">
                                            <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                <v-icon>mdi-dots-vertical</v-icon>
                                            </v-btn>
                                        </template>
                                        <v-list color="white" light="" width="auto" class="action-button-list">
                                            <v-list color="white" light width="auto" class="action-button-list">
                                                <v-list-item id="AutoTest_F74" link @click.stop="EditPaymentDialog = true,fnInitializeEditPaymentItem(item)">
                                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Edit</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_F75" link @click.stop="RemovePaymentDialog = true,RemovePaymentItem=item">
                                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Remove</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-list>
                                    </v-menu>
                                </div>
                            </template>
                        </v-data-table>
                    </v-col>
                </v-row>
                <v-row v-else justify="center">
                    <v-col class="text-center">
                        <p class="mt-2 pt-2">
                            <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="80" width="80" />
                        </p>
                        <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>

        <!---------------------------- Accounts Sections ----------------------------------------->
        <v-card class="mt-0 elevation-0" height="640">
            <!-- Accounts-->
            <v-card-title style="height:60px; margin-right:85px" v-show="ShowAccountLedger == false" >
                <h5 class="page-head">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-book-open-outline</v-icon>
                    </v-avatar>
                    Accounts
                </h5>
                <v-spacer></v-spacer>
                <Success-Button id="AutoTest_F76" class="mr-4" title="Create Account" @click.native="AddAccountsDialog=true"  v-bind:disabled ="!screenAccess"></Success-Button>
                <!--Bulk Action -->
                <v-menu offset-y transition="scroll-y-transition"  v-bind:disabled ="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                            Bulk Actions
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list v-if="selectedAccountList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_F77" @click.stop="RemoveAccountDialog = true">
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
                <v-text-field label="Search" class="table-search" v-model="SearchAccountList"
                              dense prepend-inner-icon="mdi-magnify"
                              outlined hide-details></v-text-field>
            </v-card-title>
            <v-card-text style="height:60px; margin-right:85px" class="px-0 pl-1" v-show="ShowAccountLedger == false">
                <v-row v-if="AccountList.length > 0">
                    <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
                        <v-data-table :headers="AccountListHeaders"
                                      :items="AccountList"
                                      :search="SearchAccountList"
                                      v-model="selectedAccountList"
                                      item-key="AccountId"
                                      show-select
                                      fixed-header
                                      height="520">
                            <!-- Account Name-->
                            <template v-slot:item.AccountName="{item}">
                                <v-list class="py-0 table-v-list">
                                    <v-list-item id="AutoTest_F78" class="my-0 px-0" link @click="ShowAccountLedger = true,HeaderAccountName=item.AccountName,AccountId=item.AccountId,fnGetPaymentList()">
                                        <div class="pa-0 text-limit" style="width:160px">
                                            <v-avatar size="20" color="#fff" class="elevation-1">
                                                <v-icon size="12" color="primary">mdi-book</v-icon>
                                            </v-avatar>
                                            {{item.AccountName}}
                                        </div>
                                    </v-list-item>
                                </v-list>
                            </template>

                            <template v-slot:item.ContactName="{item}">
                                <div v-if="item.ContactName" class="pa-0 pl-1 text-limit" style="width:80px" >
                                    {{item.ContactName}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.ContactEmail="{item}">
                                <div v-if="item.ContactEmail" class="pa-0 pl-1 text-limit" style="width:150px">
                                    {{item.ContactEmail}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.BankName="{item}">
                                <div v-if="item.BankName" class="pa-0 pl-1 text-limit" style="width:180px">
                                   {{item.BankName}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.BankAccountType="{item}">
                                <div v-if="item.BankAccountType" class="pa-0 pl-1 text-limit" style="width:120px">
                                    {{item.BankAccountType}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.BankAccountNumber="{item}">
                                <div v-if="item.BankAccountNumber" class="pa-0 pl-1 text-limit" style="width:120px">
                                    {{item.BankAccountNumber}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.BankIFSCCode="{item}">
                                <div v-if="item.BankIFSCCode" class="pa-0 pl-1 text-limit" style="width:110px">
                                    {{item.BankIFSCCode}}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.AccountsActions="{item}">
                                <div v-if="item.AccountsActions==null || item.AccountsActions=='' || item.AccountsActions=='null'" style="width:60px">
                                    {{item.AccountsActions}}
                                </div>
                            </template>

                            <!-- Row Actions-->
                            <template v-slot:item.AccountsActions="{item}">
                                <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                    <template v-slot:activator="{ on }">
                                        <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                            <v-icon>mdi-dots-vertical</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light width="auto" class="action-button-list">
                                        <v-list-item id="AutoTest_F79" link @click.stop="EditAccountDialog = true,fnInitializeEditAccountItem(item)">
                                            <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                            <v-list-item-content>Edit</v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_F80" link @click.stop="RemoveAccountDialog = true,RemoveAccountItem=item">
                                            <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>
                            </template>
                        </v-data-table>
                    </v-col>
                </v-row>
                <!-- if no data-->
                <v-row v-else justify="center">
                    <v-col class="text-center">
                        <p class="mt-2 pt-2">
                            <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="80" width="80" />
                        </p>
                        <p class="heading-4-dark font-weight-medium pt-0">No accounts available</p>
                    </v-col>
                </v-row>
            </v-card-text>

            <!-- Accounts Ledger Datatable-->
            <v-card-title style="height:60px; margin-right:85px"  v-show="ShowAccountLedger == true">
                <h5 id="AutoTest_F81" class="page-head" @click="ShowAccountLedger = false">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                    </v-avatar>
                    <span style="cursor:pointer">{{HeaderAccountName}}</span><v-icon>mdi-chevron-right</v-icon> Ledger book
                </h5>
                <v-spacer></v-spacer>
                <!--Bulk Action -->
                <v-menu offset-y transition="scroll-y-transition"  v-bind:disabled ="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                            Bulk Actions
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list v-if="selectedPaymentList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_F82" @click.stop="RemovePaymentDialog = true">
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
                <v-text-field label="Search" class="mr-4" v-model="SearchfilterPaymentList"
                              dense prepend-inner-icon="mdi-magnify"
                              outlined hide-details style="max-width:300px"></v-text-field>
            </v-card-title>
            <v-card-text v-show="ShowAccountLedger == true" class="px-0">
                <v-row>
                    <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
                        <v-data-table :headers="LedgerBookTableheaders"
                                      :items="PaymentList"
                                      item-key="PaymentId"
                                      :search="SearchfilterPaymentList"
                                      v-model="selectedPaymentList"
                                      show-select
                                      fixed-header
                                      height="450">
                            <template v-slot:item.PaymentActivityName="{item}">
                                <v-list class="py-0 table-v-list">
                                    <div class="pa-0 pl-1 text-limit" style="width:120px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-book</v-icon>
                                        </v-avatar>
                                        {{ item.PaymentActivityName }}
                                    </div>
                                </v-list>
                            </template>
                            <template v-slot:item.TransactionId="{item}">
                                <div class="pa-0 text-limit" style="width:80px">
                                    {{ item.TransactionId }}
                                </div>
                            </template>
                            <template v-slot:item.TransactionDate="{item}">
                                <div class="pa-0 text-limit" style="width:80px">
                                    {{ formatDate(item.TransactionDate) }}
                                </div>
                            </template>
                            <template v-slot:item.TransactionAmount="{item}">
                                <div class="pa-0 text-limit" style="width:90px">
                                    <v-icon v-if="item.TransactionType=='Credit'" size="12" color="#0CBA2F">mdi-plus</v-icon>
                                    <v-icon v-else size="12" color="#BA0C0C">mdi-minus</v-icon>
                                    {{ item.TransactionAmount }}
                                </div>
                            </template>

                            <template v-slot:item.IncomeAccountId="{item}">
                                <div class="pa-0 text-limit" style="width:105px">
                                    {{ fnGetAccountName(item.IncomeAccountId) }}
                                </div>
                            </template>
                            <template v-slot:item.ExpenseAccountId="{item}">
                                <div class="pa-0 text-limit" style="width:90px">
                                    {{ fnGetAccountName(item.ExpenseAccountId) }}
                                </div>
                            </template>
                            <template v-slot:item.TransactionType="{item}">
                                <div style="width:75px">
                                    <div v-if="item.TransactionType=='Credit'">
                                        Credit
                                    </div>
                                    <div v-else>
                                        Debit
                                    </div>
                                </div>
                            </template>
                            <!--<template v-slot:item.ExpenseTags="{item}">
        <div v-for="tagItem in item.ExpenseTags">{{ tagItem.TagName }}</div>
    </template>-->

                            <template v-slot:item.TransactionDocName="{item}">
                                <div v-if="item.TransactionDocName" class="pa-0 text-limit handPointer" style="width:90px;" @click="DownloadDocument(item.TransactionDocName,'/Documents/7e23e75d-bd85-4c3d-82ad-a07585551986.jpg')">
                                    <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                                        <v-icon size="10" color="#fff">mdi-download</v-icon>
                                    </v-avatar>
                                    {{ item.TransactionDocName }}
                                </div>
                                <div v-else>N/A</div>
                            </template>

                            <template v-slot:item.TransactionStatus="{item}">
                                <div v-if="item.TransactionStatus=='Approved'" style="width:90px" class="pa-0 text-limit">
                                    <v-avatar color="#388E3C" size="13" class="mr-1">
                                        <v-icon size="10" color="#fff">mdi-check</v-icon>
                                    </v-avatar>Approved
                                </div>
                                <div v-else style="width:70px" class="pa-0 text-limit">
                                    <v-avatar color="#BA0C0C" size="13" class="mr-1">
                                        <v-icon size="10" color="#fff">mdi-close</v-icon>
                                    </v-avatar>Rejected
                                </div>
                            </template>

                            <template v-slot:item.LedgerTableActions="{item}">
                                <div style="width:60px">
                                    <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                        <template v-slot:activator="{ on }">
                                            <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                <v-icon>mdi-dots-vertical</v-icon>
                                            </v-btn>
                                        </template>
                                        <v-list color="white" light="" width="auto" class="action-button-list">
                                            <v-list color="white" light width="auto" class="action-button-list">
                                                <v-list-item id="AutoTest_F83" link @click.stop="EditPaymentDialog = true,fnInitializeEditPaymentItem(item)">
                                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Edit</v-list-item-content>
                                                </v-list-item>
                                                <v-list-item id="AutoTest_F84" link @click.stop="RemovePaymentDialog = true,RemovePaymentItem=item">
                                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                                    <v-list-item-content>Remove</v-list-item-content>
                                                </v-list-item>
                                            </v-list>
                                        </v-list>
                                    </v-menu>
                                </div>
                            </template>
                        </v-data-table>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>

        <!---------------------- Accounts actions like Add Edit and Remove -->
        <!-- Add Account Dialog -->
        <v-dialog v-model="AddAccountsDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-book-open-outline</v-icon>
                    </v-avatar>
                    Create Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F85" text="" @click="AddAccountsDialog=false,fnInitializeAddAccount()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="AddAccountForm"
                        v-model="valid"
                        @submit.prevent="fnAddAccount">
                    <v-card-text>
                        <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new account.</h4>
                        <v-row justify="center">
                            <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Account Name" v-model="AddAccount.AccountName">
                                    <template v-slot:append>
                                        <span v-if="AddAccount.AccountName==0" class="error--text">*</span>
                                    </template>
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
                                <!--<v-row>
                    <v-col class="py-0">-->
                                <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="AddAccount.Pincode"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                </v-text-field>
                                <!--</v-col>
                    <v-col class="py-0">-->
                                <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="AddAccount.BankIFSCCode">
                                </v-text-field>
                                <!--</v-col>
                    </v-row>-->
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="AddAccount.BankName" @change="fnGetBranchList(AddAccount.BankName)"
                                          hide-selected>
                                    <template v-slot:append>
                                        <span v-if="AddAccount.BankName==0" class="error--text">*</span>
                                    </template>
                                </v-select>
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="AddAccount.BankBranch" @change="fnGetIFSC(AddAccount.BankName,AddAccount.BankBranch)">
                                    <template v-slot:append>
                                        <span v-if="AddAccount.BankBranch==0" class="error--text">*</span>
                                    </template>
                                </v-select>
                                <!--<v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="AddAccount.BankAccountType">
                    </v-select>-->
                                <v-row>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No." v-model="AddAccount.BankAccountNumber"
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==50) return false;" min="0">
                                            <template v-slot:append>
                                                <span v-if="AddAccount.BankAccountNumber==0" class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </v-col>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="AddAccount.PAN">
                                            <template v-slot:append>
                                                <span v-if="AddAccount.PAN==0" class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                                <!--<v-select dense="" :items="ProjectUserList" label="Select User" outlined="" v-model="selectedUser">
                        <template v-slot:selection="{ item, index }">
                            <v-avatar size="27" light :color="`${getcolor(item.UserName)}`" class="mr-2">
                                <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                            </v-avatar>
                            {{ item.UserName }}
                        </template>
                        <template v-slot:item="{ item }">
                            <v-avatar size="27" light :color="`${getcolor(item.UserName)}`" class="mr-2">
                                <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                            </v-avatar>
                            {{ item.UserName }}
                        </template>
                    </v-select>-->
                                <v-autocomplete :rules="[v => !!v || 'Required']" dense="" :items="ProjectUserList" label="Select User" outlined="" item-text="UserName" item-value="UserID" v-model="selectedUser">
                                    <template v-slot:selection="data">
                                        <v-chip v-bind="data.attrs"
                                                :input-value="data.selected"
                                                close
                                                @click="data.select"
                                                @click:close="remove(data.item)">
                                            <v-avatar size="27" light :color="`${getcolor(data.item.UserName)}`" class="mr-2">
                                                <img v-bind:src="userimage + data.item.ProfileImage" alt="Donor name" v-if="data.item.ProfileImage.length >0" />
                                                <span class="white--text" v-else>  {{data.item.UserName == null ? "" :data.item.UserName.substring(0, 1).toUpperCase()}}</span>
                                            </v-avatar>
                                            {{ data.item.UserName }}
                                        </v-chip>
                                    </template>
                                    <template v-slot:item="data">
                                        <v-avatar size="27" light :color="`${getcolor(data.item.UserName)}`" class="mr-2">
                                            <img v-bind:src="userimage + data.item.ProfileImage" alt="Donor name" v-if="data.item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>  {{data.item.UserName == null ? "" :data.item.UserName.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                        {{ data.item.UserName }}
                                    </template>
                                </v-autocomplete>
                                <!--<span class="small-text mr-1">
                        <v-icon size="12">mdi-account</v-icon> Select User
                    </span>
                    <v-avatar size="27" class="team-avatar" :color="`${getcolor(selectedUserName)}`" v-if="selectedUser > 0">
                        <img v-bind:src="userimage + ProfileImage" alt="User DP" v-if="ProfileImage.length >0" />
                        <span class="white--text" v-else>{{selectedUserName == null ? "" : selectedUserName.substring(0, 1).toUpperCase()}}</span>
                    </v-avatar>

                    <v-menu max-width="300" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                        <template v-slot:activator="{ on }">
                            <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                            </v-btn>
                        </template>-->
                                <!--Assign Account User pop up-->
                                <!--<v-card>
                        <v-card-title class="heading-3">
                            Select the User
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_F86" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AssignAccountUserPopUp=false,AddAccount.UserId=0,selectedUser=0">
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
                                                 :value="userItem.UserID">
                                        </v-radio>
                                    </v-radio-group>
                                    <v-divider></v-divider>
                                </template>
                            </v-list>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                            <v-row no-gutters>
                                <v-col md="12" class="py-0" align="center">
                                    <Success-Button id="AutoTest_F87" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignAccountUser()"></Success-Button>
                                </v-col>
                            </v-row>
                        </v-card-actions>
                    </v-card>-->
                                <!--_______________________________________-->
                                <!--</v-menu>-->

                                <v-checkbox label="80G eligible" color="#52B962" multiple-active="" v-model="AddAccount.Is80Geligible"></v-checkbox>
                            </v-col>

                        </v-row>
                    </v-card-text>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Cancel-Button-Outlined-Sm id="AutoTest_F88" class="float-right" @click.native="AddAccountsDialog=false,fnInitializeAddAccount()"></Cancel-Button-Outlined-Sm>
                            <Secondary-Button id="AutoTest_F89" class="btn-122x36 mr-4 float-right" title="Confirm" @click.native="fnAddAccount()"></Secondary-Button>
                        </v-col>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>
        


        <!-- Edit Account Dialog-->
        <v-dialog v-model="EditAccountDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-book-open-outline</v-icon>
                    </v-avatar>
                    Edit Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F90" text="" @click="EditAccountDialog=false,fnInitializeEditAccount()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="EditAccountForm"
                        v-model="valid"
                        @submit.prevent="fnEditAccount()">
                    <v-card-text>
                        <h4 class="text-center heading-3 mt-4">Edit the basic details of the account.</h4>
                        <v-row justify="center">
                            <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">
                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Account Name" v-model="EditAccount.AccountName">
                                    <template v-slot:append>
                                        <span v-if="EditAccount.AccountName==0" class="error--text">*</span>
                                    </template>
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
                                <!--<v-row>
                        <v-col class="py-0">-->
                                <v-text-field outlined="" dense="" label="Pincode" type="number" v-model="EditAccount.Pincode"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                </v-text-field>
                                <!--</v-col>
                    <v-col class="py-0">-->
                                <v-text-field disabled outlined="" dense="" label="IFSC Code" v-model="EditAccount.BankIFSCCode">
                                </v-text-field>
                                <!--</v-col>
                    </v-row>-->
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankItems" label="Bank" outlined="" v-model="EditAccount.BankName" @change="fnGetBranchList(EditAccount.BankName),EditAccount.BankIFSCCode=''">
                                    <template v-slot:append>
                                        <span v-if="EditAccount.BankName==''" class="error--text">*</span>
                                    </template>
                                </v-select>
                                <v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankBranchItems" label="Branch" outlined="" v-model="EditAccount.BankBranch" @change="fnGetIFSC(EditAccount.BankName,EditAccount.BankBranch)">
                                    <template v-slot:append>
                                        <span v-if="EditAccount.BankBranch=='' || EditAccount.BankBranch==0 || EditAccount.BankBranch==null" class="error--text">*</span>
                                    </template>
                                </v-select>
                                <!--<v-select :rules="[val => (val || '').length > 0 || 'Required field']" dense="" :items="SelectBankAccountTypeItems" label="Account Type" outlined="" v-model="EditAccount.BankAccountType">
                    </v-select>-->
                                <v-row>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" type="number" label="Account No." v-model="EditAccount.BankAccountNumber"
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==50) return false;" min="0">
                                            <template v-slot:append>
                                                <span v-if="EditAccount.BankAccountNumber=='' || EditAccount.BankAccountNumber==0 || EditAccount.BankAccountNumber==null" class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </v-col>
                                    <v-col class="py-0">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="PAN" v-model="EditAccount.PAN">
                                            <template v-slot:append>
                                                <span v-if="EditAccount.PAN==0" class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </v-col>
                                </v-row>

                                <v-autocomplete :rules="[v => !!v || 'Required']" dense="" :items="ProjectUserList" label="Select User" outlined="" item-text="UserName" item-value="UserID" v-model="selectedUser">
                                    <template v-slot:selection="data">
                                        <v-chip v-bind="data.attrs"
                                                :input-value="data.selected"
                                                close
                                                @click="data.select"
                                                @click:close="remove(data.item)">
                                            <v-avatar size="27" light :color="`${getcolor(data.item.UserName)}`" class="mr-2">
                                                <img v-bind:src="userimage + data.item.ProfileImage" alt="Donor name" v-if="data.item.ProfileImage.length >0" />
                                                <span class="white--text" v-else>  {{data.item.UserName == null ? "" :data.item.UserName.substring(0, 1).toUpperCase()}}</span>
                                            </v-avatar>
                                            {{ data.item.UserName }}
                                        </v-chip>
                                    </template>
                                    <template v-slot:item="data">
                                        <v-avatar size="27" light :color="`${getcolor(data.item.UserName)}`" class="mr-2">
                                            <img v-bind:src="userimage + data.item.ProfileImage" alt="Donor name" v-if="data.item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>  {{data.item.UserName == null ? "" :data.item.UserName.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                        {{ data.item.UserName }}
                                    </template>
                                </v-autocomplete>

                                <v-checkbox label="80G eligible" color="success-check-box" multiple-active="" v-model="EditAccount.Is80Geligible"></v-checkbox>
                            </v-col>
                        </v-row>
                    </v-card-text>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Default-Button-Outlined id="AutoTest_F91" class="float-right btn-122x36" title="Cancel" @click.native="EditAccountDialog=false,fnInitializeEditAccount()"></Default-Button-Outlined>
                            <Secondary-Button id="AutoTest_F92" class="btn-122x36 mr-4 float-right" title="Save" @click.native="fnEditAccount()"></Secondary-Button>
                        </v-col>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>
        

        <!-- Remove Account -->
        <v-dialog v-model="RemoveAccountDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Account
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F93" text="" @click="RemoveAccountDialog=false,RemoveAccountItem=null">
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
                                                    <v-icon size="20" color="primary">mdi-book</v-icon>
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
                                            <Secondary-Button id="AutoTest_F94" class="btn-122x36 mr-4" title="Remove" @click.native="fnRemoveAccount()"></Secondary-Button>
                                            <Default-Button-Outlined id="AutoTest_F95" class="btn-122x36" title="Cancel" @click.native="RemoveAccountDialog=false,RemoveAccountItem=null"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        
        <!----------------------- Account Actions ends here --------------------------------------------------------->
        <!-- Add paymnet for ledger book-->
        <!--Add Payment Dialog-->
        <v-dialog v-model="AddPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Add Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F96" text="" @click="AddPaymentDialog=false,fnInitializeAddPayment()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="AddPaymentForm"
                        v-model="valid"
                        @submit.prevent="fnAddPayment">
                    <v-card-text>
                        <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new payment.</h4>
                        <v-row justify="center" class="mt-4">
                            <v-col md="6" class="v-scrolling-div" style="height:50vh">
                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Enter Payment Activity Name" v-model="AddPayment.PaymentActivityName">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.PaymentActivityName == ''"
                                              class="error--text">*</span>
                                    </template>
                                </v-text-field>
                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" type="number" outlined="" dense="" label="Enter Transaction ID" v-model="AddPayment.TransactionId"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.TransactionId == '' || AddPayment.TransactionId == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-text-field>

                                <v-menu ref="AddPaymentDateMenu"
                                        v-model="AddPaymentDateMenu"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y=""
                                        min-width="290px">
                                    <template v-slot:activator="{ on }">
                                        <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']"
                                                      v-model="computedStartDate"
                                                      label="Select Transaction Date"
                                                      prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                      readonly=""
                                                      v-on="on">
                                            <template v-slot:append>
                                                <span v-if=""
                                                      class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </template>
                                    <v-date-picker v-model="AddPaymentInputDate" no-title scrollable="" @input="AddPaymentDateMenu = false,AddPayment.TransactionDate=AddPaymentInputDate">
                                        <v-spacer></v-spacer>
                                        <!--<v-btn id="AutoTest_F97" text="" color="primary" @click="AddPaymentDateMenu = false">Cancel</v-btn>
                        <v-btn id="AutoTest_F98" text="" color="primary" @click="$refs.AddPaymentDateMenu.save(AddPaymentInputDate),AddPayment.TransactionDate=AddPaymentInputDate">OK</v-btn> -->
                                    </v-date-picker>
                                </v-menu>

                                <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" type="number" outlined="" dense="" label="Enter Amount" v-model="AddPayment.TransactionAmount"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.TransactionAmount == '' || AddPayment.TransactionAmount == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-text-field>

                                <v-select dense=""
                                          :rules="[val => (val || '').length > 0 || 'Required field']"
                                          :items="PaymentTransactionType"
                                          label="Select Transaction type"
                                          outlined="" v-model="AddPayment.TransactionType" @input="transactionTypeChanged">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.TransactionType == '' || AddPayment.TransactionType == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-select>

                                <v-select dense=""
                                          :rules="[(value) => !!value || 'Required field']"
                                          :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                          label="Select From Account"
                                          outlined="" v-model="AddPayment.IncomeAccountId">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.IncomeAccountId == '' || AddPayment.IncomeAccountId == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-select>

                                <v-select dense=""
                                          :rules="[(value) => !!value || 'Required field']"
                                          :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                          label="Select To Account"
                                          outlined="" v-model="AddPayment.ExpenseAccountId">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.ExpenseAccountId == '' || AddPayment.ExpenseAccountId == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-select>

                                <v-file-input label="Upload Payment Document" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                              v-model="AddPaymentFileInput"></v-file-input>

                                <v-select dense=""
                                          :rules="[(value) => !!value || 'Required field']"
                                          :items="PaymentStatusItems"
                                          label="Payment Status"
                                          outlined="" v-model="AddPayment.TransactionStatus">
                                    <template v-slot:append>
                                        <span v-if="AddPayment.TransactionStatus == '' || AddPayment.TransactionStatus == 0"
                                              class="error--text">*</span>
                                    </template>
                                </v-select>

                                <!-- Activity Linked -->
                                <!--<span class="small-text mr-1">
                    Activity Linked
                </span>
                <v-switch hide-details v-model="AddPayment.IsActivityLinked" class="ml-2" style="display:inline-block"
                          color="secondary"></v-switch>-->
                                <!--<v-select v-show="AddPayment.IsActivityLinked" dense
                          :items="ActivityList" hide-details :item-text="i => i.ActivityName" :item-value="i => i.ActivityId"
                          label="Select Activity" v-model="AddPayment.ActivityId"
                          outlined>
                </v-select>-->
                                <v-row>
                                    <v-col>

                                        <v-select :items="BudgetHeadList"
                                                  label="Budget Head"
                                                  item-text="BudgetHeadName"
                                                  item-value="BudgetHeadID"
                                                  outlined
                                                  dense
                                                  @change="GetBudgetItemDetails(BudgetHead)"
                                                  v-model="BudgetHead" hide-details
                                                  :rules="[(value) => !!value || 'Required field']">
                                            <template v-slot:append>
                                                <span v-if="BudgetHead == '' || BudgetHead == 0"
                                                      class="error--text">*</span>
                                            </template>
                                        </v-select>
                                    </v-col>
                                    <v-col>
                                        <v-select :items="BudgetItemList"
                                                  label="Budget Item"
                                                  item-text="ItemName"
                                                  item-value="BudgetItemID"
                                                  outlined
                                                  dense
                                                  v-model="BudgetItem" hide-details></v-select>
                                    </v-col>
                                </v-row>
                                <v-responsive width="100%"></v-responsive>

                                <!-- Expense Tags -->
                                <span class="small-text mr-1">
                                    <v-icon size="12">mdi-tag</v-icon> Expense Tags
                                </span>
                                <v-chip id="AutoTest_F99" v-for="(expenseTag, key) in AddPayment.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label close
                                        @click:close="$delete(AddPayment.ExpenseTags, key)">
                                    {{expenseTag.TagName}}
                                </v-chip>

                                <v-menu max-width="300" absolute v-model="AddPaymentExpenseTagPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                            <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Expense Tag pop up-->
                                    <v-stepper v-model="AddPaymentExpenseTagStepper">
                                        <v-stepper-items>
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        Add Expense Tags
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_F100" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false">
                                                            <v-icon size="20">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-divider></v-divider>
                                                    <v-card-text class="v-scrolling-div">
                                                        <v-row>
                                                            <p class="mb-2">Search Tags</p>
                                                            <v-col md="12" align="center">
                                                                <v-text-field outlined="" dense hide-details label="Search Tag" v-model="AddPaymentSearchExpenseTag">
                                                                </v-text-field>
                                                            </v-col>
                                                            <v-col md="12">
                                                                <v-chip id="AutoTest_F101" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
                                                                        @click="fnAddExpenseTagToList(expenseTag,'Add')" class="ma-2" small label close close-icon="mdi-pencil" @click:close="AddPaymentExpenseTagStepper=3,fnInitializeEditExpenseTagItem(expenseTag,'Add')">
                                                                    {{expenseTag.TagName}}
                                                                </v-chip>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                    <v-divider></v-divider>
                                                    <v-card-actions>
                                                        <v-row justify="center">
                                                            <v-col md="6">
                                                                <Success-Button id="AutoTest_F102" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=2"></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                            </v-stepper-content>
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        <v-btn id="AutoTest_F102" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                        </v-btn>
                                                        Create Expense Tags
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_F103" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
                                                            <v-icon size="20">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-divider></v-divider>
                                                    <v-card-text class="v-scrolling-div">
                                                        <v-row>
                                                            <v-col md="12" align="center">
                                                                <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="AddPaymentNewExpenseTagName">
                                                                </v-text-field>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                    <v-divider></v-divider>
                                                    <v-card-actions>
                                                        <v-row justify="center">
                                                            <v-col md="6">
                                                                <Success-Button id="AutoTest_F104" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Add')"></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                            </v-stepper-content>
                                            <v-stepper-content step="3" class="pa-0">
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        <v-btn id="AutoTest_F105" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                        </v-btn>
                                                        Edit Tags
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_F106" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
                                                            <v-icon size="20">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-card-title>
                                                    <v-divider></v-divider>
                                                    <v-card-text class="v-scrolling-div">
                                                        <v-row>
                                                            <v-col md="12" align="center">
                                                                <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="AddPaymentEditExpenseTagItem.TagName">
                                                                </v-text-field>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                    <v-divider></v-divider>
                                                    <v-card-actions>
                                                        <v-row justify="center">
                                                            <v-col md="6">
                                                                <Success-Button id="AutoTest_F107" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddPaymentExpenseTagStepper=1,fnEditExpenseTag('Add')"></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                            </v-stepper-content>
                                        </v-stepper-items>
                                    </v-stepper>
                                </v-menu>
                            </v-col>
                        </v-row>
                    </v-card-text>

                    <v-card-actions class="pb-4">
                        <v-col>
                            <Outlined-Button-Dark id="AutoTest_F108" class="btn-122x36 float-right" title="Cancel" @click.native="AddPaymentDialog=false,fnInitializeAddPayment()"></Outlined-Button-Dark>
                            <Primary-Button-Light id="AutoTest_F109" class="btn-122x36 float-right mr-4" title="Confirm" typeof="submit" @click.native="fnAddPayment()"></Primary-Button-Light>
                        </v-col>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>
        

        <!-- Edit Payment Ledger Book-->
        <!-- Edit Ledger Book Details -->
        <v-dialog v-model="EditPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-square-edit-outline</v-icon>
                    </v-avatar>
                    Edit Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F110" text="" @click="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please edit the basic details of the new payment.</h4>
                    <v-row justify="center" class="mt-4">
                        <!--- View Ledger Book Details  -->
                        <v-col md="7" class="v-scrolling-div" style="height:40vh" v-if="EditPaymentEnable == false">
                            <v-row>
                                <v-col md="12" class="pb-0">
                                    <div class="font-weight-regular heading-3-dark pb-0">
                                        Payment Activity Name
                                        <v-btn id="AutoTest_F111" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
                                            <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </div>
                                    <P class="font-weight-regular heading-3">{{ EditPayment.PaymentActivityName }}</P>

                                    <div class="font-weight-regular heading-3-dark pt-0 pb-0">
                                        Transaction ID
                                        <v-btn id="AutoTest_F112" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
                                            <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </div>
                                    <p class="font-weight-regular heading-3">
                                        {{ EditPayment.TransactionId }}
                                    </p>

                                    <v-row>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">
                                                Transaction Date
                                                <v-btn id="AutoTest_F113" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="font-weight-regular heading-3">{{ EditPayment.TransactionDate }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">Amount</div>
                                            <P class="font-weight-regular heading-3">Rs. {{ EditPayment.TransactionAmount }}</P>
                                        </v-col>
                                    </v-row>

                                    <v-row>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">
                                                Income Account
                                                <v-btn id="AutoTest_F114" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="font-weight-regular heading-3">{{ fnGetAccountName(EditPayment.IncomeAccountId) }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">
                                                Expense Account
                                                <v-btn id="AutoTest_F115" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="font-weight-regular heading-3">{{ fnGetAccountName(EditPayment.ExpenseAccountId) }}</P>
                                        </v-col>
                                    </v-row>

                                    <v-row>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">
                                                Payment Document
                                                <v-btn id="AutoTest_F116" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="font-weight-regular heading-3">{{ EditPayment.TransactionDocName }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="font-weight-regular heading-3-dark">
                                                Payment Status
                                                <v-btn id="AutoTest_F117" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="font-weight-regular heading-3">{{ EditPayment.TransactionStatus }}</P>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-col>

                        <!-- Edit Ledger Book Details-->
                        <v-col md="6" class="v-scrolling-div" style="height:40vh" v-if="EditPaymentEnable == true">
                            <v-text-field :rules="[(value) => !!value || 'Required field']" outlined="" dense="" label="Enter Payment Activity Name" v-model="EditPayment.PaymentActivityName">
                                <template v-slot:append>
                                    <span v-if="EditPayment.PaymentActivityName == ''"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>
                            <v-text-field :rules="[(value) => !!value || 'Required field']" type="number" outlined="" dense="" label="Enter Transaction ID" v-model="EditPayment.TransactionId"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="EditPayment.TransactionId == '' || EditPayment.TransactionId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-menu ref="EditPaymentDateMenu"
                                    v-model="EditPaymentDateMenu"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y=""
                                    min-width="290px">
                                <template v-slot:activator="{ on }">
                                    <v-text-field :rules="[(value) => !!value || 'Required field']"
                                                  v-model="EditPayment.TransactionDate"
                                                  label="Select Transaction Date"
                                                  prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                  readonly=""
                                                  v-on="on">
                                        <template v-slot:append>
                                            <span v-if="EditPayment.TransactionDate==0||EditPayment.TransactionDate==''"
                                                  class="error--text">*</span>
                                        </template>
                                    </v-text-field>
                                </template>
                                <v-date-picker v-model="EditPaymentInputDate" no-title scrollable="" @input="EditPaymentDateMenu = false,EditPayment.TransactionDate=formatDate(EditPaymentInputDate)">
                                    <v-spacer></v-spacer>
                                    <!--<v-btn id="AutoTest_F118" text="" color="primary" @click="EditPaymentDateMenu = false">Cancel</v-btn>
            <v-btn id="AutoTest_F119" text="" color="primary" @click="$refs.EditPaymentDateMenu.save(EditPaymentInputDate),EditPayment.TransactionDate=formatDate(EditPaymentInputDate)">OK</v-btn>-->
                                </v-date-picker>
                            </v-menu>

                            <v-text-field :rules="[(value) => !!value || 'Required field']" type="number" outlined="" dense="" label="Enter Amount" v-model="EditPayment.TransactionAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="EditPayment.TransactionAmount == '' || EditPayment.TransactionAmount == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-select dense=""  :rules="[(value) => !!value || 'Required field']" label="Select Transaction type"  :items="PaymentTransactionType" :item-text="EditPayment.TransactionType" outlined="" v-model="EditPayment.TransactionType">
                                <template v-slot:append>
                                    <span v-if="EditPayment.TransactionType == '' || EditPayment.TransactionType == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense="" :rules="[(value) => !!value || 'Required field']" v-model="EditPayment.IncomeAccountId"
                                      :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Income Account"
                                      outlined="">
                                <template v-slot:append>
                                    <span v-if="EditPayment.IncomeAccountId == '' || EditPayment.IncomeAccountId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense="" :rules="[(value) => !!value || 'Required field']" v-model="EditPayment.ExpenseAccountId"
                                      :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Expense Account"
                                      outlined="">
                                <template v-slot:append>
                                    <span v-if="EditPayment.ExpenseAccountId == '' || EditPayment.ExpenseAccountId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>
                            <v-file-input v-model="EditPaymentFileInput" label="Upload Payment Document" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""></v-file-input>
                            <v-select dense="" :rules="[(value) => !!value || 'Required field']" v-model="EditPayment.TransactionStatus"
                                      :items="PaymentStatusItems"
                                      label="Payment Status"
                                      outlined="">
                                <template v-slot:append>
                                    <span v-if="EditPayment.TransactionStatus == '' || EditPayment.TransactionStatus == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>
                        </v-col>
                    </v-row>

                    <!-- Expense Tag Section-->
                    <v-row justify="center" class="mt-4">
                        <v-col md="6">
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-tag</v-icon> Expense Tags
                            </span>
                            <v-chip id="AutoTest_F120" v-if="EditPayment.ExpenseTags && EditPayment.ExpenseTags.length>0"
                                    v-for="(expenseTag, key) in EditPayment.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label
                                    v-bind:close="EditPaymentEnable==true"
                                    @click:close="$delete(EditPayment.ExpenseTags, key)">
                                {{expenseTag.TagName}}
                            </v-chip>

                            <v-menu max-width="300" absolute v-model="EditPaymentExpenseTagPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                           v-bind:disabled="EditPaymentEnable==false">
                                        <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Expense Tag pop up-->
                                <v-stepper v-model="EditPaymentExpenseTagStepper">
                                    <v-stepper-items>
                                        <v-stepper-content step="1" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    Add Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F121" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <p class="mb-2">Search Tags</p>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined="" dense hide-details label="Search Tag" v-model="EditPaymentSearchExpenseTag">
                                                            </v-text-field>
                                                        </v-col>
                                                        <v-col md="12">
                                                            <v-chip id="AutoTest_F122" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
                                                                    @click="fnAddExpenseTagToList(expenseTag,'Edit')" class="ma-2" small label close close-icon="mdi-pencil" @click:close="EditPaymentExpenseTagStepper=3,fnInitializeEditExpenseTagItem(expenseTag,'Edit')">
                                                                {{expenseTag.TagName}}
                                                            </v-chip>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F123" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=2"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_F124" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Create Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F125" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="EditPaymentNewExpenseTagName">
                                                            </v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F126" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Edit')"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_F127" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Edit Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F128" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="EditPaymentEditExpenseTagItem.TagName">
                                                            </v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F129" style="width:130px" class="mb-2 center-block" title="Save" @click.native="EditPaymentExpenseTagStepper=1,fnEditExpenseTag('Edit')"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col v-if="EditPaymentEnable==true">
                        <Outlined-Button-Dark id="AutoTest_F130" class="btn-122x36 float-right" title="Cancel" @click.native="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_F131" v-bind:disabled="fnCheckMandatoryEditPayment()" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnEditPayment()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        

        <!-- Remove Payment-->
        <v-dialog v-model="RemovePaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Ledger
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F132" text="" @click="RemovePaymentDialog=false,RemovePaymentItem=null">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-4">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col v-if="RemovePaymentItem!=null" cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this Payment ?</h4>
                                        </v-col>
                                        <v-col v-else cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove below Payments ?</h4>
                                        </v-col>
                                        <v-col v-if="RemovePaymentItem!=null">
                                            <h4 class="text-center heading-3">
                                                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                    <v-icon size="20" color="primary">mdi-book</v-icon>
                                                </v-avatar>
                                                {{ RemovePaymentItem.PaymentActivityName }}
                                            </h4>
                                        </v-col>
                                        <v-col v-else>
                                            <v-row v-for="selecteditem in selectedPaymentList" :key="selecteditem.PaymentId">
                                                <v-col md="12">
                                                    <h4 class="text-center heading-3">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="20" color="primary">mdi-book</v-icon>
                                                        </v-avatar>
                                                        {{ selecteditem.PaymentActivityName }}
                                                    </h4>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <Secondary-Button id="AutoTest_F133" class="btn-122x36 mr-4" title="Remove" @click.native="fnRemovePayment()"></Secondary-Button>
                                            <Default-Button-Outlined id="AutoTest_F134" class="btn-122x36" title="Cancel" @click.native="RemovePaymentDialog=false,RemovePaymentItem=null"></Default-Button-Outlined>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
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

<script>
    import SuccessButton from '@/components/success-button.vue'
    import AddButtonFab from '@/components/add-button-fab.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'
    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'


    var objManageProjectLedgerBook;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectLedgerBook.js").then(mod => {
                objManageProjectLedgerBook = new mod.ManageProjectLedgerBook();
            })
        ]);
    };
     var objManageProjectAccount;
    async function importAccountscript() {
        return Promise.all([
            import("../../BL/ManageProjectAccount.js").then(mod => {
                objManageProjectAccount = new mod.ManageProjectAccount();
            })
        ]);
    };
       var objFinance;
    async function importMyFinancescript() {
        return Promise.all([
            import("../../BL/Finance.js").then(mod => {
                objFinance = new mod.Finance();
            })
        ]);
    }
    export default {

        data() {
            return {
                valid:true,
                screenAccess: 0,
                RWAccess: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                ProjectID: 0,
                InitiativeID: 0,
                selectedAccountList:[],
                RemoveAccountDialog: false,
                AddAccountsDialog: false,
                SearchAccountList: '',
                AccountListHeaders: [
                    { text: 'ID', align: ' d-none', value: 'AccountId' },
                    { text: "Name", value: "AccountName" },
                    { text: "Contact Name", value: "ContactName" },
                    { text: "Email", value: "ContactEmail" },
                    { text: "Bank", value: "BankName" },
                    //{ text: "Account Type", value: "BankAccountType" },
                    { text: "Account No.", value: "BankAccountNumber" },
                    { text: "IFSC Code", value: "BankIFSCCode" },
                    { text: "Action", value: "AccountsActions" },
                ],
                AccountList: [],
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
                    //BankAccountType: "",
                    BankAccountNumber: 0,
                    PAN: "",
                    ProjectId: 0,
                    UserId: 0,
                    Is80Geligible: 0,
                },
                AddAccountMessage: "",
                
                SelectBankItems: [],
                SelectBankBranchItems: [],
                SelectBankAccountTypeItems: [],
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],
                ShowAccountLedger: false,
                LedgerBookTableheaders: [
                    { text: "Title", value: "PaymentActivityName" },
                    { text: "Txn ID", value: "TransactionId" },
                    { text: "Txn Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "From Account", value: "IncomeAccountId" },
                    { text: "To Account", value: "ExpenseAccountId" },
                    { text: "Txn Type", value: "TransactionType" },
                    //{ text: "Expense Tag", value: "ExpenseTags" },
                    { text: "Documents", value: "TransactionDocName" },
                    { text: "Txn Status", value: "TransactionStatus" },
                    { text: "Action", value: "LedgerTableActions" },
                ],
                filterPaymentList: [],
                SearchfilterPaymentList: "",
                EditPaymentDialog: false,
                RemovePaymentDialog: false,
                PaymentList: [],
                selectedPaymentList: [],
                IncomeAccountList: [],
                ExpenseAccountList: [],
                PaymentTransactionType: ['Credit', 'Debit'],
                PaymentStatusItems: ['Approved', 'Rejected'],

                offset: true,
                closeonClick: false,
                closeOnContentClick: false,

                AddPayment: {
                    PaymentActivityName: "",
                    TransactionId: 0,
                    TransactionDate: "",
                    TransactionAmount: 0,
                    IncomeAccountId: 0,
                    ExpenseAccountId: 0,
                    TransactionType: "",
                    TransactionDocName: "",
                    TransactionStatus: "",
                    IsActivityLinked: false,
                    ActivityId: 0,
                    ExpenseTags: [],
                },

                AddPaymentFileInput: null,
                AddPaymentInputDate: "",
                AddPaymentMessage: "",
                AddPaymentDialog: false,
                
                AddPaymentDateMenu: false,
                AddPaymentExpenseTagStepper: 1,
                AddPaymentExpenseTagPopUp: false,
                AddPaymentNewExpenseTagName: "",
                AddPaymentSearchExpenseTag: "",
                AddPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },

                filterExpense: 0,
                ProjectExpenseTagList: [{ TagID: 0, TagName: "ALL" }],
                filterProjectExpenseTagList: [],
                BudgetHeadList : [],
                BudgetHead: "",
                BudgetItemList :[],
                BudgetItem: "",

                //Remove Payment both Bulk and Single parameters
                RemovePaymentItem: null,
                RemovePaymentDialog: false,
                
                RemovePaymentMessage: "",

                ActivityList: [],

                // Edit payment variables
                //Edit Payment parameters
                EditPayment: {
                    PaymentId: 0,
                    PaymentActivityName: "",
                    TransactionId: 0,
                    TransactionDate: "",
                    TransactionAmount: 0,
                    IncomeAccountId: 0,
                    ExpenseAccountId: 0,
                    TransactionDocName: "",
                    TransactionStatus: "",
                    IsActivityLinked: false,
                    ActivityId: 0,
                    ExpenseTags: [],
                    TransactionType:''
                },
                EditPaymentFileInput: null,
                EditPaymentInputDate: "",
                EditPaymentMessage: "",
                EditPaymentDialog: false,
                
                EditPaymentDateMenu: false,
                EditPaymentExpenseTagStepper: 1,
                EditPaymentExpenseTagPopUp: false,
                EditPaymentNewExpenseTagName: "",
                EditPaymentSearchExpenseTag: "",
                EditPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },
                EditPaymentEnable: false,

                //Remove Payment both Bulk and Single parameters
                RemovePaymentItem: null,
                RemovePaymentDialog: false,
                
                RemovePaymentMessage: "",
                HeaderAccountName:"",

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
                    //BankAccountType: "",
                    BankAccountNumber: 0,
                    PAN: "",
                    ProjectId: 0,
                    UserId: 0,
                    Is80Geligible: 0,
                },
                EditAccountDialog: false,
                
                EditAccountMessage: "",

                //Remove Account parameters
                RemoveAccountDialog: false,
                
                RemoveAccountMessage: "",
                RemoveAccountItem: null,
                AccountId:0,
                RouteName:'',
                selectedUser: 0,
                ProjectUserList: [],
                AssignAccountUserPopUp:false,
                selectedUserName: "",
                ProfileImage: "",
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
                    GroupId:0,
                },
            userimage: "",
            }
        },
        components: {
            'Add-Button-Fab': AddButtonFab,
            'Success-Button-Md': SuccessButtonMd,
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Primary-Button': PrimaryButton,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Success-Button': SuccessButton,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,

        },
        mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectID = this.$route.query.ProjID;
            this.InitiativeID = this.$route.query.InitID;
            this.RouteName = this.$route.name;
            await importscript();
            await importAccountscript();
            await importMyFinancescript();
            await this.fnGetProjectDetailsByID();
            await this.fnGetPaymentList();
            await this.fnGetIncomeAccounts();
            await this.fnGetExpenseAccounts();
            await this.fnGetAllExpenseTags();
            await this.fnGetProjectAccountsByID();
            await this.fnGetAccountTypeList();
            await this.fnGetBankList();
            await this.GetBudgetHeadDetails();

            await this.fnGetUsersListByGroupId();
            //await this.fnGetProjectDetailsByID();
        },

     computed: {
            computedStartDate() {
                return this.formatDate(this.AddPaymentInputDate)
            },


        },
        methods: {

            getcolor(name) {
                let colors = [
                    "#1abc9c",
                    "#2ecc71",
                    "#3498db",
                    "#9b59b6",
                    "#34495e",
                    "#16a085",
                    "#27ae60",
                    "#2980b9",
                    "#8e44ad",
                    "#2c3e50",
                    "#f1c40f",
                    "#e67e22",
                    "#e74c3c",
                    "#95a5a6",
                    "#f39c12",
                    "#d35400",
                    "#c0392b",
                    "#bdc3c7",
                    "#7f8c8d",
                ];

                let sum = 0;
                for (let j = 0; j < name.length; j++) {
                    sum += name.charCodeAt(j);
                }
                let ascii = sum;
                let colortest = ascii % 19; // ths is the make sure that the colors are based on sum
                return colors[colortest];
            },
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.substr(0, 10).split('-')
                return `${day}-${month}-${year}`
            },
            transactionTypeChanged(item) {
                if (item === "Credit") {
                    this.AddPayment.IncomeAccountId = 87
                    this.AddPayment.ExpenseAccountId = 0
                } else if (item === "Debit") {
                    this.AddPayment.ExpenseAccountId = 87
                    this.AddPayment.IncomeAccountId = 0
                }
            },
            async fnGetProjectDetailsByID() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objManageProjectAccount.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    //alert("Project Name = " + this.projectInfo.ProjectName);
                } else {
                    this.Msg =("Failed to get details of Project " + this.projectInfo.ProjectId);
                    this.errorSnackbarMsg =true;
                }
            },
            fnCheckMandatory() {
                if (this.AddAccount.AccountName == "" ||
                    this.AddAccount.BankIFSCCode == "" ||
                    this.AddAccount.BankName == "" ||
                    this.AddAccount.BankBranch == "" ||
                    this.AddAccount.BankAccountNumber == 0 ||
                    this.AddAccount.PAN == "")
                    return true;
                else return false;
            },
            async fnGetProjectAccountsByID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objManageProjectAccount.GetProjectAccountsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AccountList = result.data;
                    //alert("AccountList = " + this.AccountList.length);
                } else {
                    this.Msg =("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },
             fnInitializeEditPaymentItem(item) {
                this.EditPayment.PaymentId = item.PaymentId;
                this.EditPayment.PaymentActivityName = item.PaymentActivityName;
                this.EditPayment.TransactionId = item.TransactionId;
                this.EditPayment.TransactionDate = this.formatDate(item.TransactionDate);
                 this.EditPayment.TransactionAmount = item.TransactionAmount;
                 this.EditPayment.TransactionType = item.TransactionType;
                this.EditPayment.IncomeAccountId = item.IncomeAccountId;
                this.EditPayment.ExpenseAccountId = item.ExpenseAccountId;
                if (item.TransactionDocName != null && item.TransactionDocName.length > 0)
                    this.EditPaymentFileInput = new File([], item.TransactionDocName);
                this.EditPayment.TransactionDocName = item.TransactionDocName;
                this.EditPayment.TransactionStatus = item.TransactionStatus;
                this.EditPayment.IsActivityLinked = item.IsActivityLinked;
                 this.EditPayment.ActivityId = item.ActivityId;
                if (item.ExpenseTags) {
                    item.ExpenseTags.forEach(u => {
                        this.EditPayment.ExpenseTags.push({ TagID: u.TagID, TagName: u.TagName })
                    });
                }
            },
            //fnGetIncomeAccountName(acctid) {
            //    var temp = this.IncomeAccountList.filter(x => x.AccountId == acctid);
            //    if (temp.length > 0) return temp[0].AccountName;
            //    else return "";
            //},
            //fnGetExpenseAccountName(acctid) {
            //    var temp = this.ExpenseAccountList.filter(x => x.AccountId == acctid);
            //    if (temp.length > 0) return temp[0].AccountName;
            //    else return "";
            //},
             fnGetAccountName(acctid) {
                var temp = this.AccountList.filter(x => x.AccountId == acctid);
                if (temp.length > 0) return temp[0].AccountName;
                else return "";
            },



          async fnGetIncomeAccounts() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("AccountType", "Income Account");
                var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.IncomeAccountList = result.data;
                } else {
                    this.Msg = "Failed";
                    this.errorSnackbarMsg=true;
                }
            },
            async fnGetExpenseAccounts() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("AccountType", "Expense Account");
                var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ExpenseAccountList = result.data;
                } else {
                    this.Msg = "Failed";
                    this.errorSnackbarMsg = true;
                }
            },
           fnLedgerCheckMandatory() {
                if (this.AddPayment.PaymentActivityName == "" ||
                    this.AddPayment.TransactionId == 0 ||
                    this.AddPayment.TransactionDate == "" ||
                    this.AddPayment.TransactionAmount == 0 ||
                    this.AddPayment.IncomeAccountId == 0 ||
                    this.AddPayment.ExpenseAccountId == 0 ||
                    this.AddPayment.TransactionStatus == ""||
                    this.AddPayment.TransactionStatus == "")
                    return true;
                else return false;
            },
             async onFileUpload(addfile, actname) {
                var lstModDate = new Date(addfile.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(addfile.lastModifiedDate).toISOString().substr(11, 5);
                const formData = new FormData()
                formData.append('myFile', addfile);
                formData.append('DocumentOriginalName', addfile.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', this.formatFileSize(addfile.size, 0));
                formData.append('DocumentType', addfile.type);
                formData.append('DocumentTypeIcon', addfile.name.split('.').pop());
                formData.append('DocumentLastModifiedDate', lstModDate);
                formData.append('ProjectId', this.ProjectID);
                formData.append('UploadedFrom', "Payment");
                formData.append('LinkedTo', actname);

                var result = await objManageProjectLedgerBook.UploadDocument(formData);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        return true;
                    else
                        return false;
                }
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
          resetValidation() {
                this.$refs.form.resetValidation()
            },
            async fnAddPayment() {
                if (this.$refs.AddPaymentForm.validate() === true) {
                    this.AddPaymentDialog = false;
                    this.Msg = "Successfully added payment to the project";
                    this.successSnackbarMsg = true;
                    if (this.AddPaymentFileInput) {
                        this.AddPayment.TransactionDocName = this.AddPaymentFileInput.name;
                        var ret = this.onFileUpload(this.AddPaymentFileInput, this.AddPayment.PaymentActivityName);
                        if (ret == false) {
                            this.AddPaymentMessage = "Failed to upload the document from Add Payment";
                            return;
                        }
                    }

                    var data = new FormData();
                    data.append("PaymentActivityName", this.AddPayment.PaymentActivityName);
                    data.append("TransactionId", this.AddPayment.TransactionId);
                    data.append("TransactionDate", this.AddPayment.TransactionDate);
                    data.append("TransactionAmount", this.AddPayment.TransactionAmount);
                    data.append("IncomeAccountId", this.AddPayment.IncomeAccountId);
                    data.append("ExpenseAccountId", this.AddPayment.ExpenseAccountId);
    
                    data.append("TransactionType", this.AddPayment.TransactionType);
                    data.append("TransactionDocName", this.AddPayment.TransactionDocName);
                    data.append("TransactionStatus", this.AddPayment.TransactionStatus);
                    data.append("IsActivityLinked", this.AddPayment.IsActivityLinked);
                    data.append("ActivityId", this.AddPayment.ActivityId);
                    data.append("ExpenseTagsList", JSON.stringify(this.AddPayment.ExpenseTags));
                    data.append("ProjectId", this.ProjectID);
                    data.append("BudgetHeadID", this.BudgetHead);
                    data.append("BudgetItemID", this.BudgetItem);
                    var result = await objManageProjectLedgerBook.AddPayment(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                        this.Msg = "Successfully added payment to the project";
                        this.successSnackbarMsg = true;
                        this.fnInitializeAddPayment();
                        this.fnGetPaymentList();
                        this.$refs.AddPaymentForm.resetValidation();
                        this.fnGetAllExpenseTags();
                    } else {
                        //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                        this.Msg = "Failed";
                        this.errorSnackbarMsg = true;
                    }
                }
            },
           async fnEditPayment() {

               this.EditPaymentDialog = false;
               this.Msg = "Payment updated successfully";
               this.successSnackbarMsg = true;
                this.EditPaymentEnable = false;
                if (this.EditPaymentFileInput != null && this.EditPaymentFileInput.Name != "" && this.EditPaymentFileInput.Name != this.EditPayment.TransactionDocName) {
                    this.EditPayment.TransactionDocName = this.EditPaymentFileInput.name;
                    var ret = this.onFileUpload(this.EditPaymentFileInput, this.EditPayment.PaymentActivityName);
                    if (ret == false) {
                        this.EditPaymentMessage = "Failed to upload the document from Edit Payment";
                        return;
                    }
                }

                var data = new FormData();
                data.append("PaymentId", this.EditPayment.PaymentId);
                data.append("PaymentActivityName", this.EditPayment.PaymentActivityName);
                data.append("TransactionId", this.EditPayment.TransactionId);
                data.append("TransactionDate", this.EditPayment.TransactionDate);
               data.append("TransactionAmount", this.EditPayment.TransactionAmount);
               data.append("TransactionType", this.EditPayment.TransactionType);
                data.append("IncomeAccountId", this.EditPayment.IncomeAccountId);
                data.append("ExpenseAccountId", this.EditPayment.ExpenseAccountId);
                if (this.EditPaymentFileInput != null)
                    data.append("TransactionDocName", this.EditPayment.TransactionDocName);
                data.append("TransactionStatus", this.EditPayment.TransactionStatus);
                data.append("IsActivityLinked", this.EditPayment.IsActivityLinked);
                data.append("ActivityId", this.EditPayment.ActivityId);
                data.append("ExpenseTagsList", JSON.stringify(this.EditPayment.ExpenseTags));
                data.append("ProjectId", this.ProjectID);
                var result = await objManageProjectLedgerBook.EditPayment(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.Msg = "Payment updated successfully";
                    this.successSnackbarMsg = true;
                    this.fnInitializeEditPayment();
                    this.fnGetPaymentList();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.Msg = "Failed";
                    this.errorSnackbarMsg=true;
                }
            },
        async GetBudgetHeadDetails() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objFinance.GetBudgetHeadDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.BudgetHeadList = result.data;
                }
            },
            async GetBudgetItemDetails(BudgetHeadID) {

                var data = new FormData();
                data.append("BudgetHeadID", BudgetHeadID);
                var result = await objFinance.GetBudgetItemDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)

                        this.BudgetItemList = result.data;
                }
            },
            fnInitializeAddPayment() {
                this.AddPayment.PaymentActivityName = "";
                this.AddPayment.TransactionId = 0;
                this.AddPayment.TransactionDate = "";
                this.AddPayment.TransactionAmount = 0;
                this.AddPayment.IncomeAccountId = 0;
                this.AddPayment.ExpenseAccountId = 0;
                this.AddPayment.TransactionType = "";
                this.AddPayment.TransactionDocName = "";
                this.AddPayment.TransactionStatus = "";
                this.AddPayment.IsActivityLinked = false;
                this.AddPayment.ActivityId = 0;
                this.AddPayment.ExpenseTags = [];
                this.AddPaymentFileInput = null;
                this.$refs.AddPaymentForm.resetValidation();
            },
            fnCheckMandatoryEditPayment() {
                if (this.EditPayment.PaymentActivityName == "" ||
                    this.EditPayment.TransactionId == 0 ||
                    this.EditPayment.TransactionDate == "" ||
                    this.EditPayment.TransactionAmount == 0 ||
                    this.EditPayment.IncomeAccountId == 0 ||
                    this.EditPayment.ExpenseAccountId == 0 ||
                    this.EditPayment.TransactionType == ""||
                    this.EditPayment.TransactionStatus == "")
                    return true;
                else return false;
            },

            fnInitializeEditPayment() {
                this.EditPayment.PaymentId = 0;
                this.EditPayment.PaymentActivityName = "";
                this.EditPayment.TransactionId = 0;
                this.EditPayment.TransactionDate = "";
                this.EditPayment.TransactionAmount = 0;
                this.EditPayment.TransactionType = "";
                this.EditPayment.IncomeAccountId = 0;
                this.EditPayment.ExpenseAccountId = 0;
                this.EditPayment.TransactionDocName = "";
                this.EditPayment.TransactionStatus = "";
                this.EditPayment.IsActivityLinked = false;
                this.EditPayment.ActivityId = 0;
                this.EditPayment.ExpenseTags = [];
                this.EditPaymentFileInput = null;
              //  this.$refs.EditAccountForm.resetValidation();
            },

            async fnRemovePayment() {
                this.RemovePaymentDialog = false;
                this.Msg = "Payment removed successfully";
                this.successSnackbarMsg = true;

                var data = new FormData();
                var paylist = [];
                if (this.RemovePaymentItem) {
                    data.append("PaymentIdList", this.RemovePaymentItem.PaymentId);
                } else {
                    for (var i = 0; i < this.selectedPaymentList.length; i++) {
                        paylist.push(this.selectedPaymentList[i].PaymentId);
                    }
                    var payids = paylist.toString();
                    data.append("PaymentIdList", payids);
                }
                var result = await objManageProjectLedgerBook.RemovePayment(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.RemovePaymentMessage = "Successfully deleted the payment.";
                    this.RemovePaymentItem = null;
                    this.selectedPaymentList = [];
                    this.fnGetPaymentList();
                } else {
                    this.Msg = "Failed";
                    this.errorSnackbarMsg=true;;
                }
            },

            async fnGetPaymentList() {

                var result = await objManageProjectLedgerBook.GetPaymentList(this.ProjectID, this.AccountId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                            if (this.AccountId == 0) 
                            {
                                this.filterPaymentList = result.data;
                            }
                            else
                            {
                                this.PaymentList = result.data;
                            }

                } else {
                    this.Msg = "";
                    this.ErrorPaymentTermSuccessSnackbar = true;
                }
            },
            async DownloadDocument(docName, docUName) {
                var downloadFileName = docName;
                var fileUrl = window.SERVER_URL + "/Upload" + docUName;

                fetch(fileUrl).then(function (t) {
                    return t.blob().then((b) => {
                        var a = document.createElement("a");
                        a.href = URL.createObjectURL(b);
                        a.setAttribute("download", downloadFileName);
                        a.click();
                    });
                });
            },
            fnCheckMandatoryEditAccount() {
                if (this.EditAccount.AccountName == "" ||
                    this.EditAccount.BankIFSCCode == "" ||
                    this.EditAccount.BankName == "" ||
                    this.EditAccount.BankBranch == "" ||
                    this.EditAccount.BankAccountNumber == 0 ||
                    this.EditAccount.PAN == "" )
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
                //this.EditAccount.BankAccountType = "";
                this.EditAccount.BankAccountNumber = 0;
                this.EditAccount.PAN = "";
                this.EditAccount.ProjectId = 0;
                this.EditAccount.UserId = 0;
                this.EditAccount.Is80Geligible = 0;
            },
        //Expense Tags functions
            async fnGetAllExpenseTags() {

                var result = await objManageProjectLedgerBook.GetAllExpenseTags(this.ProjectID);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.ProjectExpenseTagList.push({ TagName: 'All', TagID: 0 });
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg =("Failed to get all expense tag list of Project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg =true;
                }
            },
            async fnAddNewExpenseTag(type) {
                var tagname = "";
                switch (type) {
                    case "Add":
                        tagname = this.AddPaymentNewExpenseTagName;
                        break;
                    default:
                        tagname = this.EditPaymentNewExpenseTagName;
                        break;
                }
                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg =("TagName you entered already exists. Please enter a new tag name.");
                    this.infoSnackbarMsg =true;
                    return;
                }
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("TagName", tagname);
                var result = await objManageProjectLedgerBook.AddNewExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        switch (type) {
                            case "Add":
                                this.AddPaymentNewExpenseTagName = "";
                                break;
                            default:
                                this.EditPaymentNewExpenseTagName = "";
                                break;
                        }
                        this.fnGetAllExpenseTags();
                    } else {
                        this.Msg =("Failed to add expense tag.");
                        this.errorSnackbarMsg =true;
                    }
                }
            },
            fnAddExpenseTagToList(item, type) {
                switch (type) {
                    case "Add":
                        if (this.AddPayment.ExpenseTags.length == 0)
                            this.AddPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                        else {
                            var temp = this.AddPayment.ExpenseTags.filter(x => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.AddPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                            }
                        }
                        break;
                    case "Edit":
                        if (this.EditPayment.ExpenseTags.length == 0)
                            this.EditPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                        else {
                            var temp = this.EditPayment.ExpenseTags.filter(x => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.EditPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                            }
                        }
                        break;
                }
            },
            fnInitializeEditExpenseTagItem(exptagitem = null, type) {
                switch (type) {
                    case "Add":
                        if (exptagitem == null) {
                            this.AddPaymentEditExpenseTagItem.TagID = 0;
                            this.AddPaymentEditExpenseTagItem.TagName = "";
                        } else {
                            this.AddPaymentEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.AddPaymentEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                    default:
                        if (exptagitem == null) {
                            this.EditPaymentEditExpenseTagItem.TagID = 0;
                            this.EditPaymentEditExpenseTagItem.TagName = "";
                        } else {
                            this.EditPaymentEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.EditPaymentEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                }
            },
            async fnEditExpenseTag(type) {
                var tagid = 0; var tagname = "";
                switch (type) {
                    case "Add":
                        tagid = this.AddPaymentEditExpenseTagItem.TagID;
                        tagname = this.AddPaymentEditExpenseTagItem.TagName;
                        break;
                    default:
                        tagid = this.EditPaymentEditExpenseTagItem.TagID;
                        tagname = this.EditPaymentEditExpenseTagItem.TagName;
                        break;
                }

                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg =("TagName you entered already exists. Please enter a new tag name.");
                    this.infoSnackbarMsg =true;
                    return;
                }

                var data = new FormData();
                data.append("TagID", tagid);
                data.append("TagName", tagname);
                var result = await objManageProjectLedgerBook.EditExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.fnInitializeEditExpenseTagItem(null, type);
                        this.fnGetAllExpenseTags();
                    } else {
                        this.Msg =("Failed to edit expense tag.");
                        this.errorSnackbarMsg =true;
                    }
                }
            },
            fnFilterPaymentList() {
                if (this.filterExpense != 0) {
                    this.filterPaymentList = [];
                    for (var j = 0; j < this.PaymentList.length; j++) {
                        if (this.PaymentList[j].ExpenseTags.length > 0) {
                            var temp = this.PaymentList[j].ExpenseTags.filter(x => x.TagID == this.filterExpense);
                            if (temp.length > 0) this.filterPaymentList.push(this.PaymentList[j]);
                        }
                    }
                } else {
                    this.filterPaymentList = this.PaymentList;
                }
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
                //this.EditAccount.BankAccountType = item.BankAccountType;
                this.EditAccount.BankAccountNumber = item.BankAccountNumber;
                this.EditAccount.PAN = item.PAN;
                this.EditAccount.ProjectId = item.ProjectId;
                //this.EditAccount.UserId = item.UserId;
                this.selectedUser = item.UserId;
                this.EditAccount.Is80Geligible = item.Is80Geligible;
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
                //this.AddAccount.BankAccountType = "";
                this.AddAccount.BankAccountNumber = 0;
                this.AddAccount.PAN = "";
                this.AddAccount.ProjectId = 0;
                this.AddAccount.UserId = 0;
                this.AddAccount.Is80Geligible = 0;
               this.selectedUser = 0;
               this.$refs.AddAccountForm.resetValidation();
            },
            async fnAddAccount() {
                if (this.$refs.AddAccountForm.validate() === true) {
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
                    this.AddAccount.ProjectId = this.ProjectID;
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
                    //data.append("BankAccountType", this.AddAccount.BankAccountType);
                    data.append("BankAccountNumber", this.AddAccount.BankAccountNumber);
                    data.append("PAN", this.AddAccount.PAN);
                    data.append("ProjectId", this.AddAccount.ProjectId);


                    //data.append("UserId", this.AddAccount.UserId);
                    data.append("UserId", this.selectedUser);
                    data.append("Is80Geligible", this.AddAccount.Is80Geligible);
                    var result = await objManageProjectAccount.AddAccount(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                        this.Msg = "Successfully added account to the project";
                        this.successSnackbarMsg = true;
                        this.fnGetProjectAccountsByID();
                        this.fnGetIncomeAccounts();
                        this.fnGetExpenseAccounts();
                        this.$refs.AddAccountForm.resetValidation();
                        this.fnInitializeAddAccount()
                    } else {
                        //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                        this.Msg = "Failed to add account to the project";
                        this.errorSnackbarMsg = true;
                    }
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
                    this.errorSnackbarMsg =true;
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
                    this.Msg =("Failed to get details of Bank Branch.");
                    this.errorSnackbarMsg =true;
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
                    this.Msg =("Failed to get details of IFSC code.");
                    this.errorSnackbarMsg =true;
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
                    this.errorSnackbarMsg =true;
                }
            },
            async fnEditAccount() {
                if (this.$refs.EditAccountForm.validate() === true) {
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
                    //data.append("BankAccountType", this.EditAccount.BankAccountType);
                    data.append("BankAccountNumber", this.EditAccount.BankAccountNumber);
                    data.append("PAN", this.EditAccount.PAN);
                    data.append("ProjectId", this.EditAccount.ProjectId);
                    //data.append("UserId", this.EditAccount.UserId);
                    data.append("UserId", this.selectedUser);
                    data.append("Is80Geligible", this.EditAccount.Is80Geligible);
                    var result = await objManageProjectAccount.EditAccount(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                        this.Msg = "Successfully updated the account details";
                        this.successSnackbarMsg = true;
                        this.fnGetProjectAccountsByID();
                        this.$refs.EditAccountForm.resetValidation();
                    } else {
                        //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                        this.Msg = "Failed to update the account details";
                        this.errorSnackbarMsg = true;
                    }
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
            remove(item) {
                this.selectedUser = 0;
            },
            AssignAccountUser(){

            this.AssignAccountUserPopUp=false;
            this.AddAccount.UserId=this.selectedUser;
            this.selectedUserName = this.ProjectUserList.filter(x => x.UserID == this.selectedUser)[0].UserName;
            this.ProfileImage = this.ProjectUserList.filter(x => x.UserID == this.selectedUser)[0].ProfileImage;
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
                }
            },

            async fnExportPaymentFile()
            {
                 var data = new FormData();
                 data.append("ProjectId", this.ProjectID);
                 data.append("AccountId", this.AccountId);
                 var result = await objManageProjectLedgerBook.ExportPaymentFile(data)
                 var PaymentFile = [];
                 if (result.status == "Unhandled Exception")
                 {
                    this.$router.push({ name: "error" });
                 }
                 else if (result.status == "Success")
                 {
                        PaymentFile = result.data;
                        var ShowLabel =  true;
                        var CSV = '';
                        if(PaymentFile.length > 0)
                        {
                           //This condition will generate the Label/Header
                            if (ShowLabel) {
                                var row = "";
                                //This loop will extract the label from 1st index of on array
                               // for (i = 0; i < this.ActualPaymentList.length; i++) {
                                    for (var index in PaymentFile[0]) {
                                        //Now convert each value to string and comma-seprated
                                        row += index + ',';
                                    }
                                    row = row.slice(0, -1);
                                    //append Label row with line break
                                    CSV += row + '\r\n';
                              //  }
                            }
                            //1st loop is to extract each row
                            for (var i = 0; i < PaymentFile.length; i++) {
                                var row = "";
                                //2nd loop will extract each column and convert it in string comma-seprated
                                for (var index in PaymentFile[i]) {
                                    row += '"' + PaymentFile[i][index] + '",';
                                }
                                row.slice(0, row.length - 1);
                                //add a line break after each row
                                CSV += row + '\r\n';
                            }
                            if (CSV == '') {
                               // alert("Invalid data");
                                return;
                            }
                            var hiddenElement = document.createElement('a');
                            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(CSV);
                            hiddenElement.target = '_blank';
                            hiddenElement.download = 'Paymentfile_' + Math.round(Math.random() * 1000) + '.csv';
                            hiddenElement.click();
                        }
                  }
            }
        },
  }
</script>

<style scoped>
    .handPointer {
        cursor: pointer;
    }
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