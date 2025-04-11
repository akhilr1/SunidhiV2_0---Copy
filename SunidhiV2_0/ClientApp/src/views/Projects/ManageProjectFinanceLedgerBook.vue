<template>
    <v-card class="custom-card-1">
        <v-card-title style="height:73px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                <router-link :to="{ name: 'ManageProjectFinance', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId,ProjID: projectInfo.ProjectId} }" class="page-head text-link">Finance Details </router-link>
                <v-icon size="15">mdi-chevron-right</v-icon>
                <router-link :to="{ name: 'ManageProjectFinanceAccounts', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId,ProjID: projectInfo.ProjectId} }" class="page-head text-link">Accounts</router-link>
                <v-icon size="15">mdi-chevron-right</v-icon> Ledger Book
            </h5>
            <v-spacer></v-spacer>
            <Success-Button id="AutoTest_IP475" v-if="filterPaymentList.length>0" class="mr-4" title="Add Payment" @click.native="AddPaymentDialog=true" v-bind:disabled ="!screenAccess"></Success-Button>
            <v-select v-if="filterPaymentList.length>0" style="max-width:150px;" class="mr-4" hide-selected="" hide-details="" dense=""
                      :items="ProjectExpenseTagList"
                      :item-text="i => i.TagName" :item-value="i => i.TagID"
                      v-model="filterExpense"
                      label="Select Expense Tag" @change="fnFilterPaymentList()"
                      outlined="">
            </v-select>

            <!-- Bulk Action-->
            <v-menu v-if="filterPaymentList.length>0" offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list v-if="selectedPaymentList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                    <v-list-item id="AutoTest_IP476" @click.stop="RemovePaymentDialog = true">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Remove
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>

            <!-- Search-->
            <v-text-field v-if="filterPaymentList.length>0" label="Search" class="mr-4" v-model="SearchfilterPaymentList"
                          dense prepend-inner-icon="mdi-magnify"
                          outlined hide-details style="max-width:150px"></v-text-field>
        </v-card-title>

        <!--Ledger book Table-->
        <v-data-table v-if="filterPaymentList.length>0" :headers="LedgerBookTableheaders"
                      :items="filterPaymentList"
                      item-key="PaymentId"
                      :search="SearchfilterPaymentList"
                      v-model="selectedPaymentList"
                      show-select>
            <template v-slot:item.PaymentActivityName="{item}">
                <v-list class="py-0 table-v-list">
                    <v-list-item class="my-0 px-0">
                        <v-list-item-icon>
                            <v-avatar size="30" color="#fff" class="elevation-1">
                                <v-icon size="20" color="primary">mdi-book</v-icon>
                            </v-avatar>
                        </v-list-item-icon>
                        <v-list-item-content>
                            {{ item.PaymentActivityName }}
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </template>

            <template v-slot:item.TransactionDate="{item}">
                {{ formatDate(item.TransactionDate) }}
            </template>

            <template v-slot:item.IncomeAccountId="{item}">
                {{ fnGetIncomeAccountName(item.IncomeAccountId) }}
            </template>
            <template v-slot:item.ExpenseAccountId="{item}">
                {{ fnGetExpenseAccountName(item.ExpenseAccountId) }}
            </template>

            <template v-slot:item.ExpenseTags="{item}">
                <div v-for="tagItem in item.ExpenseTags">{{ tagItem.TagName }}</div>
            </template>

            <template v-slot:item.TransactionDocName="{item}">
                <div v-if="item.TransactionDocName">
                    {{ item.TransactionDocName }}
                    <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                        <v-icon size="10" color="#fff">mdi-check</v-icon>
                    </v-avatar>
                </div>
            </template>

            <template v-slot:item.TransactionStatus="{item}">
                <div v-if="item.TransactionStatus=='Approved'">
                    <v-avatar color="#0CBA2F" size="13" class="mr-1">
                        <v-icon size="10" color="#fff">mdi-check</v-icon>
                    </v-avatar>Approved
                </div>
                <div v-else>
                    <v-avatar color="#BA0C0C" size="13" class="mr-1">
                        <v-icon size="10" color="#fff">mdi-close</v-icon>
                    </v-avatar>Rejected
                </div>
            </template>

            <template v-slot:item.LedgerTableActions="{item}">
                <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled ="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                            <v-icon>mdi-dots-vertical</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" class="action-button-list">
                        <v-list color="white" light width="auto" class="action-button-list">
                            <v-list-item id="AutoTest_IP477" link @click.stop="EditPaymentDialog = true,fnInitializeEditPaymentItem(item)">
                                <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                <v-list-item-content>Edit</v-list-item-content>
                            </v-list-item>
                            <v-list-item id="AutoTest_IP478" link @click.stop="RemovePaymentDialog = true,RemovePaymentItem=item">
                                <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                <v-list-item-content>Remove</v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-list>
                </v-menu>
            </template>
        </v-data-table>

        <v-card-text v-if="filterPaymentList.length==0">
            <v-row>
                <v-col class="text-center">
                    <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                    <h4 class="title text-center" color="#585858">No ledger book available.</h4>
                    <Success-Button id="AutoTest_IP479" class="mt-4" style="width:200px" title="Add Payments" @click.native="AddPaymentDialog=true" v-bind:disabled ="!screenAccess"></Success-Button>
                </v-col>
            </v-row>
        </v-card-text>

        <!--Add Payment Dialog-->
        <v-dialog v-model="AddPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Add Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP480" text="" @click="AddPaymentDialog=false,fnInitializeAddPayment()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new payment.</h4>
                    <v-row justify="center" class="mt-4">
                        <v-col md="6" class="v-scrolling-div" style="height:50vh">
                            <v-text-field outlined="" dense="" label="Enter Payment Activity Name" v-model="AddPayment.PaymentActivityName">
                            </v-text-field>
                            <v-text-field type="number" outlined="" dense="" label="Enter Transaction ID" v-model="AddPayment.TransactionId"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>

                            <v-menu ref="AddPaymentDateMenu"
                                    v-model="AddPaymentDateMenu"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y=""
                                    min-width="290px">
                                <template v-slot:activator="{ on }">
                                    <v-text-field v-model="AddPayment.TransactionDate"
                                                  label="Select Transaction Date"
                                                  prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                  readonly=""
                                                  v-on="on"></v-text-field>
                                </template>
                                <v-date-picker v-model="AddPaymentInputDate" no-title="" scrollable="">
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP481" text="" color="primary" @click="AddPaymentDateMenu = false">Cancel</v-btn>
                                    <v-btn id="AutoTest_IP482" text="" color="primary" @click="$refs.AddPaymentDateMenu.save(AddPaymentInputDate),AddPayment.TransactionDate=AddPaymentInputDate">OK</v-btn>
                                </v-date-picker>
                            </v-menu>

                            <v-text-field type="number" outlined="" dense="" label="Enter Amount" v-model="AddPayment.TransactionAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>

                            <v-select dense=""
                                      :items="IncomeAccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Income Account"
                                      outlined="" v-model="AddPayment.IncomeAccountId">
                            </v-select>

                            <v-select dense=""
                                      :items="ExpenseAccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Expense Account"
                                      outlined="" v-model="AddPayment.ExpenseAccountId">
                            </v-select>
                            <v-file-input label="Upload Payment Document" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                          v-model="AddPaymentFileInput"></v-file-input>
                            <v-select dense=""
                                      :items="PaymentStatusItems"
                                      label="Payment Status"
                                      outlined="" v-model="AddPayment.TransactionStatus">
                            </v-select>

                            <!-- Activity Linked -->
                            <span class="small-text mr-1">
                                Activity Linked
                            </span>
                            <v-switch hide-details v-model="AddPayment.IsActivityLinked" class="ml-2" style="display:inline-block"
                                      color="secondary"></v-switch>
                            <v-select v-show="AddPayment.IsActivityLinked" dense
                                      :items="ActivityList" hide-details :item-text="i => i.ActivityName" :item-value="i => i.ActivityId"
                                      label="Select Activity" v-model="AddPayment.ActivityId"
                                      outlined>
                            </v-select>
                            <v-responsive width="100%"></v-responsive>

                            <!-- Expense Tags -->
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-tag</v-icon> Expense Tags
                            </span>
                            <v-chip id="AutoTest_IP483" v-for="(expenseTag, key) in AddPayment.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label close
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
                                                    <v-btn id="AutoTest_IP484" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false">
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
                                                            <v-chip id="AutoTest_IP485" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
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
                                                            <Success-Button id="AutoTest_IP486" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=2"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP487" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Create Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP488" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
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
                                                            <Success-Button id="AutoTest_IP489" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Add')"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP490" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Edit Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP491" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
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
                                                            <Success-Button id="AutoTest_IP493" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddPaymentExpenseTagStepper=1,fnEditExpenseTag('Add')"></Success-Button>
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
                        <Outlined-Button-Dark id="AutoTest_IP494" class="btn-122x36 float-right" title="Cancel" @click.native="AddPaymentDialog=false,fnInitializeAddPayment()"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_IP495" v-bind:disabled="fnCheckMandatory()" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnAddPayment()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="AddPaymentSuccessSnackbar" top="" color="primary">
            {{ AddPaymentMessage }}
        </v-snackbar>

        <!-- Edit Ledger Book Details -->
        <v-dialog v-model="EditPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP496" text="" @click="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please edit the basic details of the new payment.</h4>
                    <v-row justify="center" class="mt-4">
                        <!--- View Ledger Book Details  -->
                        <v-col md="6" class="v-scrolling-div" style="height:40vh" v-if="EditPaymentEnable == false">
                            <v-row justify="center">
                                <v-col md="12" class="pb-0">
                                    <div class="page-sub-head pb-0 text-limit">
                                        Payment Activity Name
                                        <v-btn id="AutoTest_IP497" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
                                            <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </div>
                                    <P class="heading-1">{{ EditPayment.PaymentActivityName }}</P>

                                    <div class="page-sub-head pt-0 pb-0 text-limit">
                                        Transaction ID
                                        <v-btn id="AutoTest_IP498" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
                                            <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </div>
                                    <p class="text-left heading-1" style="max-height:120px;overflow-y:auto">
                                        {{ EditPayment.TransactionId }}
                                    </p>

                                    <v-row no-gutters>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Transaction Date
                                                <v-btn id="AutoTest_IP499" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ EditPayment.TransactionDate }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">Amount</div>
                                            <P class="heading-1">Rs. {{ EditPayment.TransactionAmount }}</P>
                                        </v-col>
                                    </v-row>

                                    <v-row no-gutters>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Income Account
                                                <v-btn id="AutoTest_IP500" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ fnGetIncomeAccountName(EditPayment.IncomeAccountId) }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Expense Account
                                                <v-btn id="AutoTest_IP501" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ fnGetExpenseAccountName(EditPayment.ExpenseAccountId) }}</P>
                                        </v-col>
                                    </v-row>

                                    <v-row no-gutters>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Payment Document
                                                <v-btn id="AutoTest_IP502" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ EditPayment.TransactionDocName }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Payment Status
                                                <v-btn id="AutoTest_IP503" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ EditPayment.TransactionStatus }}</P>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-col>

                        <!-- Edit Ledger Book Details-->
                        <v-col md="6" class="v-scrolling-div" style="height:40vh" v-if="EditPaymentEnable == true">
                            <v-text-field outlined="" dense="" label="Enter Payment Activity Name" v-model="EditPayment.PaymentActivityName">
                            </v-text-field>
                            <v-text-field type="number" outlined="" dense="" label="Enter Transaction ID" v-model="EditPayment.TransactionId"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>

                            <v-menu ref="EditPaymentDateMenu"
                                    v-model="EditPaymentDateMenu"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y=""
                                    min-width="290px">
                                <template v-slot:activator="{ on }">
                                    <v-text-field v-model="EditPayment.TransactionDate"
                                                  label="Select Transaction Date"
                                                  prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                  readonly=""
                                                  v-on="on"></v-text-field>
                                </template>
                                <v-date-picker v-model="EditPaymentInputDate" no-title="" scrollable="">
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP504" text="" color="primary" @click="EditPaymentDateMenu = false">Cancel</v-btn>
                                    <v-btn id="AutoTest_IP505" text="" color="primary" @click="$refs.EditPaymentDateMenu.save(EditPaymentInputDate),EditPayment.TransactionDate=formatDate(EditPaymentInputDate)">OK</v-btn>
                                </v-date-picker>
                            </v-menu>

                            <v-text-field type="number" outlined="" dense="" label="Enter Amount" v-model="EditPayment.TransactionAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>

                            <v-select dense="" v-model="EditPayment.IncomeAccountId"
                                      :items="IncomeAccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Income Account"
                                      outlined="">
                            </v-select>

                            <v-select dense="" v-model="EditPayment.ExpenseAccountId"
                                      :items="ExpenseAccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select Expense Account"
                                      outlined="">
                            </v-select>
                            <v-file-input v-model="EditPaymentFileInput" label="Upload Payment Document" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""></v-file-input>
                            <v-select dense="" v-model="EditPayment.TransactionStatus"
                                      :items="PaymentStatusItems"
                                      label="Payment Status"
                                      outlined="">
                            </v-select>
                        </v-col>
                    </v-row>

                    <!-- Expense Tag Section-->
                    <v-row justify="center" class="mt-4">
                        <v-col md="6">
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-tag</v-icon> Expense Tags
                            </span>
                            <v-chip id="AutoTest_IP506" v-if="EditPayment.ExpenseTags && EditPayment.ExpenseTags.length>0"
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
                                                    <v-btn id="AutoTest_IP507" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false">
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
                                                            <v-chip id="AutoTest_IP508" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
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
                                                            <Success-Button id="AutoTest_IP509" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=2"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP510" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Create Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP511" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
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
                                                            <Success-Button id="AutoTest_IP512" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Edit')"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP513" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Edit Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP514" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
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
                                                            <Success-Button id="AutoTest_IP515" style="width:130px" class="mb-2 center-block" title="Save" @click.native="EditPaymentExpenseTagStepper=1,fnEditExpenseTag('Edit')"></Success-Button>
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
                        <Outlined-Button-Dark id="AutoTest_IP516" class="btn-122x36 float-right" title="Cancel" @click.native="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_IP517" v-bind:disabled="fnCheckMandatoryEditPayment()" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnEditPayment()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="EditPaymentSuccessSnackbar" top="" color="primary">
            {{ EditPaymentMessage }}
        </v-snackbar>

        <!-- Remove Ledger-->
        <!-- Remove Payment Terms-->
        <v-dialog v-model="RemovePaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Ledger
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP518" text="" @click="RemovePaymentDialog=false,RemovePaymentItem=null">
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
                                                    <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                </v-avatar>
                                                {{ RemovePaymentItem.PaymentActivityName }}
                                            </h4>
                                        </v-col>
                                        <v-col v-else>
                                            <v-row v-for="selecteditem in selectedPaymentList" :key="selecteditem.PaymentId">
                                                <v-col md="12">
                                                    <h4 class="text-center heading-3">
                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                                        </v-avatar>
                                                        {{ selecteditem.PaymentActivityName }}
                                                    </h4>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP519" color="secondary elevation-0" class="mr-2" @click="fnRemovePayment()">Remove</v-btn>
                                            <v-btn id="AutoTest_IP520" outlined color="grey" @click="RemovePaymentDialog=false,RemovePaymentItem=null">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="RemovePaymentSuccessSnackbar" top="" color="primary">
            {{ RemovePaymentMessage }}
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
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'

    var objManageProjectLedgerBook;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectLedgerBook.js").then(mod => {
                objManageProjectLedgerBook = new mod.ManageProjectLedgerBook();
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
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,

                //below parameters used for both Add and Edit Payment
                IncomeAccountList: [],
                ExpenseAccountList: [],
                PaymentStatusItems: ['Approved', 'Rejected'],
                ExpenseTagName: "",

                //payment listing parameters
                LedgerBookTableheaders: [
                    { text: "Title", value: "PaymentActivityName" },
                    { text: "Transaction ID", value: "TransactionId" },
                    { text: "Transaction Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "Income Account", value: "IncomeAccountId" },
                    { text: "Expense Account", value: "ExpenseAccountId" },
                    { text: "Expense Tag", value: "ExpenseTags" },
                    { text: "Documents", value: "TransactionDocName" },
                    { text: "Payment Status", value: "TransactionStatus" },
                    { text: "Action", value: "LedgerTableActions" },
                ],
                filterPaymentList: [],
                PaymentList: [],
                selectedPaymentList: [],
                SearchfilterPaymentList: "",
                ActivityList: [],

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
                },
                EditPaymentFileInput: null,
                EditPaymentInputDate: "",
                EditPaymentMessage: "",
                EditPaymentDialog: false,
                EditPaymentSuccessSnackbar: false,
                EditPaymentDateMenu: false,
                EditPaymentExpenseTagStepper: 1,
                EditPaymentExpenseTagPopUp: false,
                EditPaymentNewExpenseTagName: "",
                EditPaymentSearchExpenseTag: "",
                EditPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },
                EditPaymentEnable: false,

                //General parameters
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
                AccountId: 0,

                //Add Payment parameters
                AddPayment: {
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
                },
                AddPaymentFileInput: null,
                AddPaymentInputDate: "",
                AddPaymentMessage: "",
                AddPaymentDialog: false,
                AddPaymentSuccessSnackbar: false,
                AddPaymentDateMenu: false,
                AddPaymentExpenseTagStepper: 1,
                AddPaymentExpenseTagPopUp: false,
                AddPaymentNewExpenseTagName: "",
                AddPaymentSearchExpenseTag: "",
                AddPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },

                //Top bar filter parameters
                filterExpense: 0,
                ProjectExpenseTagList: [],
                filterProjectExpenseTagList: [],

                //Remove Payment both Bulk and Single parameters
                RemovePaymentItem: null,
                RemovePaymentDialog: false,
                RemovePaymentSuccessSnackbar: false,
                RemovePaymentMessage: "",
            }
        },
        components: {
            'Add-Button-Fab': AddButtonFab,
            'Success-Button-Md': SuccessButtonMd,
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,

            'Success-Button': SuccessButton,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Primary-Button': PrimaryButton,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.AccountId = this.$route.query.AccID;
            await importscript();
            await this.fnGetProjectDetailsByID();
            await this.fnGetActivityList();
            await this.fnGetPaymentList();
            await this.fnGetIncomeAccounts();
            await this.fnGetExpenseAccounts();
            await this.fnGetAllExpenseTags();
        },
        methods: {
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.substr(0, 10).split('-')
                return `${day}-${month}-${year}`
            },
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProjectLedgerBook.GetProjectDetailsByID(data);

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
            async fnGetPaymentList() {
                /*var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                if (this.AccountId != 0) {
                    data.append("AccountId", this.AccountId);
                }*/
                var result = await objManageProjectLedgerBook.GetPaymentList(this.projectInfo.ProjectId, this.AccountId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterPaymentList = result.data;
                    this.PaymentList = result.data;
                } else {
                    this.Msg = ("Failed to get payment list of Project " + this.projectInfo.ProjectName);
                    this.snackbarMsg = true;
                }
            },
            async fnGetActivityList() {
                var result = await objManageProjectLedgerBook.GetActivityList(this.projectInfo.ProjectId, 0);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ActivityList = result.data;
                } else {
                    this.Msg = ("Failed to get activity list of Project " + this.projectInfo.ProjectName);
                    this.snackbarMsg = true;
                }
            },
            fnGetIncomeAccountName(acctid) {
                var temp = this.IncomeAccountList.filter(x => x.AccountId == acctid);
                if (temp.length > 0) return temp[0].AccountName;
                else return "";
            },
            fnGetExpenseAccountName(acctid) {
                var temp = this.ExpenseAccountList.filter(x => x.AccountId == acctid);
                if (temp.length > 0) return temp[0].AccountName;
                else return "";
            },
            fnCheckMandatory() {
                if (this.AddPayment.PaymentActivityName == "" ||
                    this.AddPayment.TransactionId == 0 ||
                    this.AddPayment.TransactionDate == "" ||
                    this.AddPayment.TransactionAmount == 0 ||
                    this.AddPayment.IncomeAccountId == 0 ||
                    this.AddPayment.ExpenseAccountId == 0 ||
                    this.AddPayment.TransactionStatus == "")
                    return true;
                else return false;
            },
            fnInitializeAddPayment() {
                this.AddPayment.PaymentActivityName = "";
                this.AddPayment.TransactionId = 0;
                this.AddPayment.TransactionDate = "";
                this.AddPayment.TransactionAmount = 0;
                this.AddPayment.IncomeAccountId = 0;
                this.AddPayment.ExpenseAccountId = 0;
                this.AddPayment.TransactionDocName = "";
                this.AddPayment.TransactionStatus = "";
                this.AddPayment.IsActivityLinked = false;
                this.AddPayment.ActivityId = 0;
                this.AddPayment.ExpenseTags = [];
                this.AddPaymentFileInput = null;
            },
            async fnGetIncomeAccounts() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("AccountType", "Income Account");
                var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.IncomeAccountList = result.data;
                } else {
                    this.Msg = ("Failed to get details of Income Accounts for the project " + this.projectInfo.ProjectName);
                    this.snackbarMsg = true;
                }
            },
            async fnGetExpenseAccounts() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("AccountType", "Expense Account");
                var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ExpenseAccountList = result.data;
                } else {
                    this.Msg = ("Failed to get details of Expense Accounts for the project " + this.projectInfo.ProjectName);
                    this.snackbarMsg = true;
                }
            },
            async fnAddPayment() {
                this.AddPaymentDialog = false;
                this.AddPaymentSuccessSnackbar = true;
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
                data.append("TransactionDocName", this.AddPayment.TransactionDocName);
                data.append("TransactionStatus", this.AddPayment.TransactionStatus);
                data.append("IsActivityLinked", this.AddPayment.IsActivityLinked);
                data.append("ActivityId", this.AddPayment.ActivityId);
                data.append("ExpenseTagsList", JSON.stringify(this.AddPayment.ExpenseTags));
                data.append("ProjectId", this.projectInfo.ProjectId);
                var result = await objManageProjectLedgerBook.AddPayment(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.AddPaymentMessage = "Successfully added payment to the project";
                    this.fnInitializeAddPayment();
                    this.fnGetPaymentList();
                    this.fnGetAllExpenseTags();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.AddPaymentMessage = "Failed to add payment to the project";
                }

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
                formData.append('ProjectId', this.projectInfo.ProjectId);
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
            async fnRemovePayment() {
                this.RemovePaymentDialog = false;
                this.RemovePaymentSuccessSnackbar = true;

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
                    this.RemovePaymentMessage = "Failed to deleted the payment.";
                }
            },
            fnInitializeEditPayment() {
                this.EditPayment.PaymentId = 0;
                this.EditPayment.PaymentActivityName = "";
                this.EditPayment.TransactionId = 0;
                this.EditPayment.TransactionDate = "";
                this.EditPayment.TransactionAmount = 0;
                this.EditPayment.IncomeAccountId = 0;
                this.EditPayment.ExpenseAccountId = 0;
                this.EditPayment.TransactionDocName = "";
                this.EditPayment.TransactionStatus = "";
                this.EditPayment.IsActivityLinked = false;
                this.EditPayment.ActivityId = 0;
                this.EditPayment.ExpenseTags = [];
                this.EditPaymentFileInput = null;
            },
            fnInitializeEditPaymentItem(item) {
                this.EditPayment.PaymentId = item.PaymentId;
                this.EditPayment.PaymentActivityName = item.PaymentActivityName;
                this.EditPayment.TransactionId = item.TransactionId;
                this.EditPayment.TransactionDate = this.formatDate(item.TransactionDate);
                this.EditPayment.TransactionAmount = item.TransactionAmount;
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
            fnCheckMandatoryEditPayment() {
                if (this.EditPayment.PaymentActivityName == "" ||
                    this.EditPayment.TransactionId == 0 ||
                    this.EditPayment.TransactionDate == "" ||
                    this.EditPayment.TransactionAmount == 0 ||
                    this.EditPayment.IncomeAccountId == 0 ||
                    this.EditPayment.ExpenseAccountId == 0 ||
                    this.EditPayment.TransactionStatus == "")
                    return true;
                else return false;
            },
            async fnEditPayment() {
                this.EditPaymentDialog = false;
                this.EditPaymentSuccessSnackbar = true;
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
                data.append("IncomeAccountId", this.EditPayment.IncomeAccountId);
                data.append("ExpenseAccountId", this.EditPayment.ExpenseAccountId);
                if (this.EditPaymentFileInput != null)
                    data.append("TransactionDocName", this.EditPayment.TransactionDocName);
                data.append("TransactionStatus", this.EditPayment.TransactionStatus);
                data.append("IsActivityLinked", this.EditPayment.IsActivityLinked);
                data.append("ActivityId", this.EditPayment.ActivityId);
                data.append("ExpenseTagsList", JSON.stringify(this.EditPayment.ExpenseTags));
                data.append("ProjectId", this.projectInfo.ProjectId);
                var result = await objManageProjectLedgerBook.EditPayment(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    //alert("Successfully added account to the project " + this.projectInfo.ProjectName);
                    this.EditPaymentMessage = "Successfully updated the payment details.";
                    this.fnInitializeEditPayment();
                    this.fnGetPaymentList();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.EditPaymentMessage = "Failed to update payment details.";
                }
            },

            //Expense Tags functions
            async fnGetAllExpenseTags() {
                var result = await objManageProjectLedgerBook.GetAllExpenseTags(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg = ("Failed to get all expense tag list of Project " + this.projectInfo.ProjectName);
                    this.snackbarMsg = true;
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
                    this.Msg = ("TagName you entered already exists. Please enter a new tag name.");
                    this.snackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
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
                        this.Msg = ("Failed to add expense tag.");
                        this.snackbarMsg = true;
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
                    this.Msg = ("TagName you entered already exists. Please enter a new tag name.");
                    this.snackbarMsg = true;
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
                        this.Msg = ("Failed to edit expense tag.");
                        this.snackbarMsg = true;
                    }
                }
            },
        },
        watch: {
            AddPaymentSearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.AddPaymentSearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.AddPaymentSearchExpenseTag));
            },
            EditPaymentSearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.EditPaymentSearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.EditPaymentSearchExpenseTag));
            },
        }
    }
</script>