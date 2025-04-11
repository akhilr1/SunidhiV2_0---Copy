<template>
    <v-card class="custom-card-1 elevation-0 " height="640">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-card-title style="margin-right:85px" class="py-4 pb-4 page-head">
            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                <v-icon size="15" color="primary">mdi-book-multiple</v-icon>
            </v-avatar>
            Project Ledger Book
            <v-spacer></v-spacer>
            <!--If there is Ledger Book-->
            <template>
                <Success-Button id="AutoTest_IP112" v-if="filterPaymentList.length>0" class="mr-4" title="Add Payment" @click.native="AddPaymentDialog=true" v-bind:disabled="!screenAccess"></Success-Button>
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
                        <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-0">
                            Bulk Actions
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list v-if="selectedPaymentList.length>0" color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_IP113" @click.stop="RemovePaymentDialog = true">
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
                <v-btn height="39" color="grey" outlined=""  class="elevation-0 mr-0 ml-3" @click="fnExportPaymentFile()">
                    Export
                    <v-icon>mdi-chevron-down</v-icon>
                </v-btn>
                <!-- Search text-->
                <v-text-field label="Search" class="table-search ml-3" v-model="SearchPaymentList"
                              dense prepend-inner-icon="mdi-magnify"
                              outlined hide-details></v-text-field>
            </template>
        </v-card-title>

        <v-card-text v-if="filterPaymentList.length==0">
            <v-row>
                <v-col class="text-center">
                    <p class="mt-2 pt-2">
                        <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                    </p>
                    <p class="heading-4-dark font-weight-medium pt-0">No data available</p>
                    <Success-Button id="AutoTest_IP114" class="" style="width:200px" title="Add Payments" @click.native="AddPaymentDialog=true" v-bind:disabled="!screenAccess"></Success-Button>
                </v-col>
            </v-row>
        </v-card-text>

        <!--Ledger book Table-->
        <v-data-table v-if="filterPaymentList.length>0"
                      :headers="LedgerBookTableheaders"
                      :items="filterPaymentList"
                      :search="SearchPaymentList"
                      :items-per-page="5"
                      item-key="PaymentId"
                      show-select
                      v-model="selectedPaymentList"
                      fixed-header
                      height="520">
            >
            <template v-slot:item.PaymentActivityName="{item}">
                <div class="pa-0 text-limit" style="width:125px">
                    <v-avatar size="25" color="#fff" class="elevation-1">
                        <v-icon size="15" color="primary">mdi-book</v-icon>
                    </v-avatar>
                    {{ item.PaymentActivityName }}
                </div>
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
                <div class="pa-0 text-limit" style="width:70px">
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
                    <v-avatar color="#388E3C" tile="" size="13">
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
                                <v-list-item id="AutoTest_IP115" link @click.stop="EditPaymentDialog = true,fnInitializeEditPaymentItem(item)">
                                    <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Edit</v-list-item-content>
                                </v-list-item>
                                <v-list-item id="AutoTest_IP116" link @click.stop="RemovePaymentDialog = true,RemovePaymentItem=item">
                                    <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                    <v-list-item-content>Remove</v-list-item-content>
                                </v-list-item>
                            </v-list>
                        </v-list>
                    </v-menu>
                </div>
            </template>
        </v-data-table>

        <!--Add Payment Dialog-->
        <v-dialog v-model="AddPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Add Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP117" text="" @click="AddPaymentDialog=false,fnInitializeAddPayment()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="AddPaymentForm"  v-model="valid"  @submit.prevent="fnAddPayment">
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
                                                      v-model="AddPayment.TransactionDate"
                                                      label="Select Transaction Date"
                                                      prepend-inner-icon="mdi-calendar" outlined="" dense=""
                                                      readonly=""
                                                      v-on="on">
                                            <template v-slot:append>
                                                <span v-if="AddPayment.TransactionDate == '' || AddPayment.TransactionDate == 0"
                                                      class="error--text">*</span>
                                            </template>
                                        </v-text-field>
                                    </template>
                                    <v-date-picker v-model="AddPaymentInputDate" no-title="" scrollable="" :max="maxDate">
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP118" text="" color="primary" @click="AddPaymentDateMenu = false">Cancel</v-btn>
                                        <v-btn id="AutoTest_IP119" text="" color="primary" @click="$refs.AddPaymentDateMenu.save(AddPaymentInputDate),AddPayment.TransactionDate=AddPaymentInputDate">OK</v-btn>
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
                                          :rules="[(value) => !!value || 'Required field']"
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
                                                  :menu-props="{ maxHeight: '190' }"
                                                  @change="GetBudgetItemDetails(BudgetHead)"
                                                  v-model="BudgetHead" hide-details></v-select>
                                    </v-col>
                                    <v-col>
                                        <v-select :items="BudgetItemList"
                                                  label="Budget Item"
                                                  item-text="ItemName"
                                                  item-value="BudgetItemID"
                                                  outlined
                                                  dense
                                                  :menu-props="{ maxHeight: '190' }"
                                                  v-model="BudgetItem" hide-details></v-select>
                                    </v-col>
                                </v-row>
                                <v-responsive width="100%"></v-responsive>

                                <!-- Expense Tags -->
                                <span class="small-text mr-1">
                                    <v-icon size="12">mdi-tag</v-icon> Expense Tags
                                </span>
                                <v-chip id="AutoTest_IP120" v-for="(expenseTag, key) in AddPayment.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label close
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
                                                        <v-btn id="AutoTest_IP121" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false">
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
                                                                <v-chip id="AutoTest_IP122" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
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
                                                                <Success-Button id="AutoTest_IP123" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=2"></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                            </v-stepper-content>
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        <v-btn id="AutoTest_IP124" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                        </v-btn>
                                                        Create Expense Tags
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP125" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,AddPaymentNewExpenseTagName=''">
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
                                                                <Success-Button id="AutoTest_IP126" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Add')"></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                            </v-stepper-content>
                                            <v-stepper-content step="3" class="pa-0">
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        <v-btn id="AutoTest_IP127" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
                                                            <v-icon size="20">mdi-arrow-left</v-icon>
                                                        </v-btn>
                                                        Edit Tags
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP128" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddPaymentExpenseTagPopUp=false,AddPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Add')">
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
                                                                <Success-Button id="AutoTest_IP129" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddPaymentExpenseTagStepper=1,fnEditExpenseTag('Add')"></Success-Button>
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
                            <Outlined-Button-Dark id="AutoTest_IP130" class="btn-122x36 float-right" title="Cancel" @click.native="AddPaymentDialog=false,fnInitializeAddPayment()"></Outlined-Button-Dark>
                            <Primary-Button-Light id="AutoTest_IP131" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnAddPayment()"></Primary-Button-Light>
                        </v-col>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>


        <!-- Edit Ledger Book Details -->
        <v-dialog v-model="EditPaymentDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP132" text="" @click="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false">
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
                                        <v-btn id="AutoTest_IP133" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
                                            <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </div>
                                    <P class="heading-1">{{ EditPayment.PaymentActivityName }}</P>

                                    <div class="page-sub-head pt-0 pb-0 text-limit">
                                        Transaction ID
                                        <v-btn id="AutoTest_IP134" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true">
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
                                                <v-btn id="AutoTest_IP135" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
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
                                                <v-btn id="AutoTest_IP136" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ fnGetAccountName(EditPayment.IncomeAccountId) }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Expense Account
                                                <v-btn id="AutoTest_IP137" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ fnGetAccountName(EditPayment.ExpenseAccountId) }}</P>
                                        </v-col>
                                    </v-row>

                                    <v-row no-gutters>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Payment Document
                                                <v-btn id="AutoTest_IP138" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>
                                            </div>
                                            <P class="heading-1">{{ EditPayment.TransactionDocName }}</P>
                                        </v-col>
                                        <v-col md="6">
                                            <div class="page-sub-head pb-1 text-limit">
                                                Payment Status
                                                <v-btn id="AutoTest_IP139" text max-width="16" min-width="16" height="16" @click="EditPaymentEnable = true" class="mb-1">
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
                            <v-text-field :rules="[val => (val || '').length > 0 || 'Required field']" outlined="" dense="" label="Enter Payment Activity Name" v-model="EditPayment.PaymentActivityName">
                                <template v-slot:append>
                                    <span v-if="EditPayment.PaymentActivityName == ''"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>
                            <v-text-field :rules="[(value) => !!value || 'Required field']"  type="number" outlined="" dense="" label="Enter Transaction ID" v-model="EditPayment.TransactionId"
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
                                <v-date-picker v-model="EditPaymentInputDate" no-title="" scrollable="">
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP140" text="" color="primary" @click="EditPaymentDateMenu = false">Cancel</v-btn>
                                    <v-btn id="AutoTest_IP141" text="" color="primary" @click="$refs.EditPaymentDateMenu.save(EditPaymentInputDate),EditPayment.TransactionDate=formatDate(EditPaymentInputDate)">OK</v-btn>
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

                            <v-select dense=""
                                      :rules="[(value) => !!value || 'Required field']"
                                      :items="PaymentTransactionType"
                                      label="Select Transaction type"
                                      outlined="" v-model="EditPayment.TransactionType">
                                <template v-slot:append>
                                    <span v-if="EditPayment.TransactionType == '' || EditPayment.TransactionType == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense="" :rules="[(value) => !!value || 'Required field']" v-model="EditPayment.IncomeAccountId"
                                      :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select From Account"
                                      outlined="">
                                <template v-slot:append>
                                    <span v-if="EditPayment.IncomeAccountId == '' || EditPayment.IncomeAccountId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense="" :rules="[(value) => !!value || 'Required field']" v-model="EditPayment.ExpenseAccountId"
                                      :items="AccountList" :item-text="i => i.AccountName" :item-value="i => i.AccountId"
                                      label="Select To Account"
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
                            <v-chip id="AutoTest_IP142" v-if="EditPayment.ExpenseTags && EditPayment.ExpenseTags.length>0"
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
                                                    <v-btn id="AutoTest_IP143" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false">
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
                                                            <v-chip id="AutoTest_IP144" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
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
                                                            <Success-Button id="AutoTest_IP145" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=2"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP146" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Create Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP147" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,EditPaymentNewExpenseTagName=''">
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
                                                            <Success-Button id="AutoTest_IP148" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="EditPaymentExpenseTagStepper=1,fnAddNewExpenseTag('Edit')"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_IP149" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Edit Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP150" text="" height="25" min-width="25" max-width="25" color="#707070" @click="EditPaymentExpenseTagPopUp=false,EditPaymentExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'Edit')">
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
                                                            <Success-Button id="AutoTest_IP151" style="width:130px" class="mb-2 center-block" title="Save" @click.native="EditPaymentExpenseTagStepper=1,fnEditExpenseTag('Edit')"></Success-Button>
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
                        <Outlined-Button-Dark id="AutoTest_IP152" class="btn-122x36 float-right" title="Cancel" @click.native="EditPaymentDialog=false,fnInitializeEditPayment(),EditPaymentEnable=false"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_IP153" v-bind:disabled="fnCheckMandatoryEditPayment()" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnEditPayment()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>


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
                    <v-btn id="AutoTest_IP154" text="" @click="RemovePaymentDialog=false,RemovePaymentItem=null">
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
                                                            <v-icon size="20" color="primary">mdi-book</v-icon>
                                                        </v-avatar>
                                                        {{ selecteditem.PaymentActivityName }}
                                                    </h4>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP155" color="secondary elevation-0" class="mr-2" @click="fnRemovePayment()">Remove</v-btn>
                                            <v-btn id="AutoTest_IP156" outlined color="grey" @click="RemovePaymentDialog=false,RemovePaymentItem=null">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>


        <!--Request Fund Dialog-->
        <v-dialog v-model="RequestFundDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Request Funds
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP157" text="" @click="RequestFundDialog=false,fnInitializeRequestFund()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please enter the basic details for requesting fund.</h4>
                    <v-row justify="center" class="mt-4">
                        <v-col md="6">
                            <v-select dense="" v-model="RequestFund.DonorUser"
                                      :items="DonorUserList" :item-text="i => i.UserName" :item-value="i => i.UserID"
                                      label="Select Donar"
                                      outlined="">
                            </v-select>

                            <v-text-field type="number" outlined dense label="Enter Amount" v-model="RequestFund.RequestAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                            </v-text-field>
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-account</v-icon> Notify People
                            </span>
                            <v-avatar size="27" class="team-avatar">
                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                            </v-avatar>
                            <v-chip id="AutoTest_IP158" v-if="RequestFund.NotifyUserList.length>0" v-for="(itemUser,key) in RequestFund.NotifyUserList" :key="itemUser.ID"
                                    class="ma-2" x-small="" label="" close="" @click:close="$delete(RequestFund.NotifyUserList, key)">
                                {{ itemUser.Name }}
                            </v-chip>
                            <v-menu absolute v-model="RequestFundNotifyUserPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                        <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Notification pop up-->
                                <v-card>
                                    <v-card-title class="heading-3">
                                        Add people to Notify
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP159" text height="25" min-width="25" max-width="25" color="#707070" @click="RequestFundNotifyUserPopUp=false,selectedProjectUserList=[]">
                                            <v-icon size="20">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-divider></v-divider>
                                    <v-card-text style="height:150px;" class="v-scrolling-div py-0">
                                        <v-row no-gutters>
                                            <v-col>
                                                <v-list dense>
                                                    <template>
                                                        <v-data-table :headers="ProjectUserListTableHeaders"
                                                                      :items="ProjectUserList"
                                                                      item-key="ID"
                                                                      hide-default-header
                                                                      hide-default-footer
                                                                      sort-by="Name"
                                                                      show-select
                                                                      class="elevation-1"
                                                                      v-model="selectedProjectUserList">
                                                            <template v-slot:item.Name="{item}">
                                                                <v-list class="py-0 table-v-list">
                                                                    <v-list-item class="my-0 px-0">
                                                                        <v-list-item-content>
                                                                            <v-list-item-title>
                                                                                <v-avatar size="27" left="">
                                                                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donar name" />
                                                                                </v-avatar>
                                                                                {{ item.Name }}
                                                                            </v-list-item-title>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list>
                                                            </template>
                                                        </v-data-table>
                                                        <v-divider></v-divider>
                                                    </template>
                                                </v-list>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-divider></v-divider>
                                    <v-card-actions>
                                        <v-row no-gutters>
                                            <v-col md="12" class="py-0" align="center">
                                                <Success-Button id="AutoTest_IP160" v-bind:disabled="selectedProjectUserList.length==0" style="width:130px" class="mb-2 center-block" title="Save" @click.native="RequestFundNotifyUserPopUp=false,RequestFund.NotifyUserList=selectedProjectUserList,selectedProjectUserList=[]"></Success-Button>
                                            </v-col>
                                        </v-row>
                                    </v-card-actions>
                                </v-card>
                                <!--__________________________-->
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Outlined-Button-Dark id="AutoTest_IP161" class="btn-122x36 float-right" title="Cancel" @click.native="RequestFundDialog=false,fnInitializeRequestFund()"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_IP162" v-bind:disabled="fnCheckMandatoryRequestFund()" class="btn-122x36 float-right mr-4" title="Confirm" @click.native="fnAddRequestFund()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
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
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'

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
                 screenAccess: 0,
                valid: true,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                SearchPaymentList: '',
                RequestFundDialog: false,
                RequestFundNotifyUserPopUp: false,
                
                //RequestFundMessage: "",
                DonorUserList: [],
                ProjectUserList: [],
                selectedProjectUserList: [],
                RequestFund: {
                    DonorUser: 0,
                    RequestAmount: 0,
                    NotifyUserList: []
                },
                ProjectUserListTableHeaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],

                //below parameters used for both Add and Edit Payment
                //IncomeAccountList: [],
                //ExpenseAccountList: [],
                AccountList:[],
                PaymentTransactionType: ['Credit', 'Debit'],
                PaymentStatusItems: ['Approved', 'Rejected'],
                //ActivityList: [],

                //payment listing parameters
                LedgerBookTableheaders: [
                    { text: "Title",align:"left", value: "PaymentActivityName" },
                    { text: "Txn ID", value: "TransactionId" },
                    { text: "Txn Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "From Account", value: "IncomeAccountId" },
                    { text: "To Account", value: "ExpenseAccountId" },
                    { text: "Txn Type", value: "TransactionType" },
                    //{ text: "Expense Tag", value: "ExpenseTags" },
                    { text: "Documents", value: "TransactionDocName" },
                    { text: "Txn Status", value: "TransactionStatus" },
                    { text: "Action",align:"center", value: "LedgerTableActions" },
                ],
                filterPaymentList: [],
                PaymentList: [],

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
                // EditPaymentMessage: "",
                EditPaymentDialog: false,
                
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

                //Add Payment parameters
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
                //AddPaymentMessage: "",
                AddPaymentDialog: false,
                
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
                
                //RemovePaymentMessage: "",

                BudgetHeadList: [],
                BudgetHead: "",
                BudgetItemList: [],
                BudgetItem: "",
                selectedPaymentList: [],
                maxDate: new Date().toISOString().substr(0, 10),
                PreLoader : false,
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
            'Pre-Loader': PreLoader,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.ProjectID = this.$route.query.ProjID;
            await importscript();
            await importAccountscript();
            await importMyFinancescript();
            await this.fnGetProjectDetailsByID();
            //await this.fnGetActivityList();
            await this.fnGetPaymentList();
            //await this.fnGetIncomeAccounts();
            //await this.fnGetExpenseAccounts();
            await this.fnGetAllExpenseTags();
            await this.fnGetProjectAccountsByID();
            await this.fnGetUsersListByGroupId();
            await this.fnGetDonorUsersListByGroupId();
            await this.GetBudgetHeadDetails();
        },
        methods: {
            /// <summary>
            ///  formatDate :- Function used to formatting a date
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDate(date) {
                return objUtils.common.formatDate(date);
            },

            /// <summary>
            ///  transactionTypeChanged :- Function used to select transaction type (Credit/Debit)
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            transactionTypeChanged(item) {
                if (item === "Credit") {
                    this.AddPayment.IncomeAccountId = 87
                    this.AddPayment.ExpenseAccountId = 0
                } else if (item === "Debit") {
                    this.AddPayment.ExpenseAccountId = 87
                    this.AddPayment.IncomeAccountId = 0
                }
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
                var result = await objManageProjectLedgerBook.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    //alert("Project Name = " + this.projectInfo.ProjectName);
                } else {
                    this.Msg = ("Failed to get details of Project " + this.projectInfo.ProjectId);
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetPaymentList :- Function used to fetch payment details based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetPaymentList() {
                var result = await objManageProjectLedgerBook.GetPaymentList(this.projectInfo.ProjectId, 0);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterPaymentList = result.data;

                    this.PaymentList = result.data;
                    //alert("Project Name = " + this.projectInfo.ProjectName);
                } else {
                    this.Msg = ("Failed to get payment list of Project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  DownloadDocument :- Function used to download document
            /// </summary>
            /// <param name="docName"></param>
            /// <param name="docUName"></param>
            /// <returns></returns>
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

            //async fnGetActivityList() {
            //    var result = await objManageProjectLedgerBook.GetActivityList(this.projectInfo.ProjectId, 0);
      
            //    if (result.status == "Unhandled Exception") {
            //        this.$router.push({ name: "error" });
            //    } else if (result.data) {
            //        this.ActivityList = result.data;
            //    } else {
            //        this.Msg = ("Failed to get activity list of Project " + this.projectInfo.ProjectName);
            //        this.errorSnackbarMsg = true;
            //    }
            //},
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

            /// <summary>
            ///  fnGetAccountName :- Function used to fetch account name based on acctid
            /// </summary>
            /// <param name="acctid"></param>
            /// <returns></returns>
            fnGetAccountName(acctid) {
                var temp = this.AccountList.filter(x => x.AccountId == acctid);
                if (temp.length > 0) return temp[0].AccountName;
                else return "";
            },

            /// <summary>
            ///  fnGetDonorUsersListByGroupId :- Function used to fetch project donor users based on GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetDonorUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("Type", "Donor");
                var result = await objManageProjectLedgerBook.GetDonorUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.DonorUserList = result.data;
                }
            },

            /// <summary>
            ///  fnGetUsersListByGroupId :- Function used to fetch user details based on GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectLedgerBook.GetUsersListByGroupId(data);
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
                data.append("ProjectId", this.ProjectID);
                var result = await objManageProjectAccount.GetProjectAccountsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AccountList = result.data;
                    //alert("AccountList = " + this.AccountList.length);
                } else {
                    this.Msg = ("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnInitializeAddPayment :- Function used to set default values in add payment variables when click on close button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeAddPayment() {
                this.AddPayment.PaymentActivityName = "";
                this.AddPayment.TransactionId = 0;
                this.AddPayment.TransactionDate = "";
                this.AddPayment.TransactionAmount = 0;
                this.AddPayment.IncomeAccountId = 0;
                this.AddPayment.ExpenseAccountId = 0;
                this.AddPayment.TransactionType == "";
                this.AddPayment.TransactionDocName = "";
                this.AddPayment.TransactionStatus = "";
                this.AddPayment.IsActivityLinked = false;
                this.AddPayment.ActivityId = 0;
                this.AddPayment.ExpenseTags = [];
                this.AddPaymentFileInput = null;
                this.$refs.AddPaymentForm.resetValidation();
            },

            //async fnGetIncomeAccounts() {
            //    var data = new FormData();
            //    data.append("ProjectId", this.projectInfo.ProjectId);
            //    data.append("AccountType", "Income Account");
            //    var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

            //    if (result.status == "Unhandled Exception") {
            //        this.$router.push({ name: "error" });
            //    } else if (result.data) {
            //        this.IncomeAccountList = result.data;
            //    } else {
            //        this.Msg = ("Failed to get details of Income Accounts for the project " + this.projectInfo.ProjectName);
            //        this.errorSnackbarMsg = true;
            //    }
            //},

            //async fnGetExpenseAccounts() {
            //    var data = new FormData();
            //    data.append("ProjectId", this.projectInfo.ProjectId);
            //    data.append("AccountType", "Expense Account");
            //    var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

            //    if (result.status == "Unhandled Exception") {
            //        this.$router.push({ name: "error" });
            //    } else if (result.data) {
            //        this.ExpenseAccountList = result.data;
            //        //this.AccountList = this.ExpenseAccountList.concat(this.IncomeAccountList)
            //    } else {
            //        this.Msg = ("Failed to get details of Expense Accounts for the project " + this.projectInfo.ProjectName);
            //        this.errorSnackbarMsg = true;
            //    }
            //},

            /// <summary>
            ///  fnAddPayment :- Function used to add payment to the project
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnAddPayment() {
                if (this.$refs.AddPaymentForm.validate() === true ) {
                    this.AddPaymentDialog = false;
                    //this.AddPaymentSuccessSnackbar = true;
                    if (this.AddPaymentFileInput) {
                        this.AddPayment.TransactionDocName = this.AddPaymentFileInput.name;
                        var ret = this.onFileUpload(this.AddPaymentFileInput, this.AddPayment.PaymentActivityName);
                        if (ret == false) {
                            this.Msg = "Failed to upload the document from Add Payment";
                            this.errorSnackbarMsg = true;
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
                    data.append("ProjectId", this.projectInfo.ProjectId);
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
                        this.Msg = "Failed to add payment to the project";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  onFileUpload :- Function used to upload files
            /// </summary>
            /// <param name="addfile"></param>
            /// <param name="actname"></param>
            /// <returns></returns>
            async onFileUpload(addfile, actname) {
                var lstModDate = new Date(addfile.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(addfile.lastModifiedDate).toISOString().substr(11, 5);
                const formData = new FormData()
                formData.append('myFile', addfile);
                formData.append('DocumentOriginalName', addfile.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', objUtils.common.formatFileSize(addfile.size, 0));
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

            /// <summary>
            ///  fnFilterPaymentList :- Function used to filter payment list
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
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

            /// <summary>
            ///  fnRemovePayment :- Function used to delete payment
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            async fnRemovePayment() {
                this.RemovePaymentDialog = false;
                //this.RemovePaymentSuccessSnackbar = true;

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
                    this.Msg = "Successfully deleted the payment.";
                    this.successSnackbarMsg = true;
                    this.RemovePaymentItem = null;
                    this.selectedPaymentList = [];
                    this.fnGetPaymentList();
                } else {
                    this.Msg = "Failed";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnInitializeEditPayment :- Function used to set default values in edit payment variables when click on close button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeEditPayment() {
                this.EditPayment.PaymentId = 0;
                this.EditPayment.PaymentActivityName = "";
                this.EditPayment.TransactionId = 0;
                this.EditPayment.TransactionDate = "";
                this.EditPayment.TransactionAmount = 0;
                this.EditPayment.IncomeAccountId = 0;
                this.EditPayment.ExpenseAccountId = 0;
                this.EditPayment.TransactionType = "";
                this.EditPayment.TransactionDocName = "";
                this.EditPayment.TransactionStatus = "";
                this.EditPayment.IsActivityLinked = false;
                this.EditPayment.ActivityId = 0;
                this.EditPayment.ExpenseTags = [];
                this.EditPaymentFileInput = null;
            },

            /// <summary>
            ///  fnInitializeEditPaymentItem :- Function used to initialize payment variables for edit payment items
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnInitializeEditPaymentItem(item) {
                this.EditPayment.PaymentId = item.PaymentId;
                this.EditPayment.PaymentActivityName = item.PaymentActivityName;
                this.EditPayment.TransactionId = item.TransactionId;
                this.EditPayment.TransactionDate = this.formatDate(item.TransactionDate);
                this.EditPayment.TransactionAmount = item.TransactionAmount;
                this.EditPayment.IncomeAccountId = item.IncomeAccountId;
                this.EditPayment.ExpenseAccountId = item.ExpenseAccountId;
                this.EditPayment.TransactionType = item.TransactionType;
                if (item.TransactionDocName!=null && item.TransactionDocName.length>0)
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

            /// <summary>
            ///  fnCheckMandatoryEditPayment :- Function used to check required fields in edit payment section
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false</returns>
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

            /// <summary>
            ///  fnEditPayment :- Function used to edit payment
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnEditPayment() {
                this.EditPaymentDialog = false;
                //this.EditPaymentSuccessSnackbar = true;
                this.EditPaymentEnable = false;
                if (this.EditPaymentFileInput != null && this.EditPaymentFileInput.Name != "" && this.EditPaymentFileInput.Name != this.EditPayment.TransactionDocName) {
                    this.EditPayment.TransactionDocName = this.EditPaymentFileInput.name;
                    var ret = this.onFileUpload(this.EditPaymentFileInput, this.EditPayment.PaymentActivityName);
                    if (ret == false) {
                        this.Msg = "Failed to upload the document from Edit Payment";
                        this.errorSnackbarMsg = true;
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
                data.append("TransactionType", this.EditPayment.TransactionType);
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
                    this.Msg = "Successfully updated the payment details.";
                    this.successSnackbarMsg = true;
                    this.fnInitializeEditPayment();
                    this.fnGetPaymentList();
                } else {
                    //alert("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
                    this.Msg = "Failed to update payment details.";
                    this.errorSnackbarMsg = true;
                }
            },

            //Expense Tags functions

            /// <summary>
            ///  fnGetAllExpenseTags :- Function used to fetch all project expense tag based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllExpenseTags() {
                var result = await objManageProjectLedgerBook.GetAllExpenseTags(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.ProjectExpenseTagList.push({ TagName: 'All', TagID: 0 });
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg = ("Failed to get all expense tag list of Project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnAddNewExpenseTag :- Function used to create new project expense tag
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
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
                    this.infoSnackbarMsg = true;
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
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnAddExpenseTagToList :- Function used to add expense tag to the list
            /// </summary>
            /// <param name="item"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnInitializeEditExpenseTagItem :- Function used to open edit expense tag item box, when click on the edit button from the list
            ///  initialize expense tag variables
            /// </summary>
            /// <param name="exptagitem = null"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnEditExpenseTag :- Function used to update project expense tag
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
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
                    this.infoSnackbarMsg = true;
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
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            //Request Fund functions

            /// <summary>
            ///  fnInitializeRequestFund :- Function used to set default values in request fund variables when click on close button
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeRequestFund() {
                this.RequestFund.DonorUser = 0;
                this.RequestFund.RequestAmount = 0;
                this.RequestFund.NotifyUserList = [];
            },

            /// <summary>
            ///  fnCheckMandatoryRequestFund :- Function used to check required fields in fund request section
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false</returns>
            fnCheckMandatoryRequestFund() {
                if (this.RequestFund.DonorUser == 0 ||
                    this.RequestFund.RequestAmount == 0 )
                    return true;
                else return false;
            },

            /// <summary>
            ///  fnAddRequestFund :- Function used to add request fund
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnAddRequestFund() {
                this.RequestFundDialog = false;
                //this.RequestFundSuccessSnackbar = true;

                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("DonorUser", this.RequestFund.DonorUser);
                data.append("RequestAmount", this.RequestFund.RequestAmount);
                data.append("NotifyUserList", JSON.stringify(this.RequestFund.NotifyUserList));
                data.append("ProjectName", this.projectInfo.ProjectName);
                var result = await objManageProjectLedgerBook.AddRequestFund(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = "Successfully added request fund";
                    this.successSnackbarMsg = true;
                    this.fnInitializeRequestFund();
                } else {
                    this.Msg = "Failed to add request fund";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  GetBudgetHeadDetails :- Function used to fetch budget head details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetBudgetHeadDetails() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                var result = await objFinance.GetBudgetHeadDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.BudgetHeadList = result.data;
                }
            },

            /// <summary>
            ///  GetBudgetItemDetails :- Function used to fetch budget item details based on BudgetHeadID
            /// </summary>
            /// <param name="BudgetHeadID"></param>
            /// <returns> </returns>
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

            /// <summary>
            ///  fnExportPaymentFile :- Function used to export payment file
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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
            },
        },
        watch: {
            /// <summary>
            ///  AddPaymentSearchExpenseTag :- Function used to search expense tags based on TagName in add payment section (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddPaymentSearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.AddPaymentSearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.AddPaymentSearchExpenseTag));
            },

            /// <summary>
            ///  EditPaymentSearchExpenseTag :- Function used to search expense tags based on TagName in edit payment section (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            EditPaymentSearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.EditPaymentSearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.EditPaymentSearchExpenseTag));
            },
        }
    }
</script>
<style scoped>
    .handPointer{
        cursor:pointer;
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
