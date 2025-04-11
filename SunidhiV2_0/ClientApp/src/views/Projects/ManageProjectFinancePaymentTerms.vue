<template>
    <v-card class="custom-card-1">
        <v-card-title style="height:73px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                <!--if path is ManageProjectFinancePaymentTerms-->
                <router-link v-show="$route.name=='ManageProjectFinancePaymentTerms'" :to="{ name: 'ManageProjectFinance', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeID} }" class="page-head text-link">Inputs</router-link>
                <!--_______________-->
                <!--if path is MyprojectsDraftIndividualFinancePaymentTerms-->
                <router-link v-show="$route.name=='MyprojectsDraftIndividualFinancePaymentTerms'" :to="{ name: 'MyprojectsDraftIndividualFinanceDetails', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeID} }" class="page-head text-link">Inputs</router-link>
                <!--_______________-->
                > Payment Terms
            </h5>
            <v-spacer></v-spacer>
            <Success-Button id="AutoTest_IP521" class="mr-4" title="Add Payment Term" @click.native="CreatePaymentTermsDialog=true" v-bind:disabled="!screenAccess"></Success-Button>
            <!--Bulk Action -->
            <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                    <v-list-item id="AutoTest_IP522" @click.stop="fnBulkRemoveClick()">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            Remove
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
                <!-- Remove Payment Terms-->
                <v-dialog v-model="RemoveBulkPaymentTermDialog" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                            </v-avatar>
                            Remove Paymnet Terms
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_IP523" text="" @click="RemoveBulkPaymentTermDialog=false">
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
                                                    <h4 class="text-center heading-3">Are you sure you want to remove this payment terms ?</h4>
                                                </v-col>

                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                    <v-btn id="AutoTest_IP524" color="secondary elevation-0" class="mr-2" @click="DeletePaymentTerm(1),RemoveBulkPaymentTermDialog = false,RemovePaymentTermSuccessSnackbar = true">Remove</v-btn>
                                                    <v-btn id="AutoTest_IP525" outlined color="grey" @click="RemoveBulkPaymentTermDialog=false">Cancel</v-btn>
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
            <v-text-field label="Search" class="mr-4" v-model="SearchPaymentTerms"
                          dense prepend-inner-icon="mdi-magnify"
                          outlined hide-details style="max-width:300px"></v-text-field>
            <!--<router-link :to="{name:'ManageProjectFinanceLedgerBook',query: { WsID: WorkspaceID}}">
            <Primary-Button-Outlined title="View All"></Primary-Button-Outlined>
        </router-link>-->
            <!--Filter button-->
            <v-menu :close-on-content-click="false" offset-y="" transition="scroll-y-transition">
                <template v-slot:activator="{ on }">
                    <v-btn v-on="on" height="39" width="90" outlined="" dense="" class="outlined-btn-dark d-none d-md-inline" @click="filterContent = !filterContent">
                        <v-icon>mdi-filter-outline</v-icon>
                        <span>Filters</span>
                    </v-btn>
                </template>
            </v-menu>
        </v-card-title>
        <!--filter options-->
        <!--<v-row justify="center" no-gutters v-if="filterContent" class="pb-3">
            <v-col md="10" class="d-none d-md-inline">
                <v-row no-gutters>
                    <v-col md="auto" cols="auto">
                        <h4 class="text-left page-head pr-3 my-1">
                            Filters
                        </h4>
                    </v-col>-->
                    <!--Date range-->
                    <!--<v-col md="auto" cols="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="dateRangeMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Date range
                                    <v-icon class="ml-10">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="900" min-width="50" class="action-button-list pa-2">
                                <v-row justify="center" no-gutters>
                                    <v-col cols="4" class="px-2">
                                        <h3 style="text-align:center">Start Date</h3>
                                        <v-date-picker color="primary" no-title @input=""
                                                       class="elevation-0"></v-date-picker>
                                    </v-col>
                                    <v-col cols="4" class="px-2">
                                        <h3 style="text-align:center">End Date</h3>
                                        <v-date-picker color="primary" no-title
                                                       class="elevation-0"></v-date-picker>
                                    </v-col>
                                    <v-col cols="4">
                                        <v-list class="px-2 mx-4">
                                            <v-header style="font-size:large" class="pl-4">Predefined Periods:</v-header>
                                            <v-list-item-group v-model="selectedItem"
                                                               color="primary">
                                                <v-list-item v-for="(item, i) in items"
                                                             :key="i">
                                                    <v-list-item-content>
                                                        <v-list-item-title v-text="item.text"></v-list-item-title>
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </v-list-item-group>
                                        </v-list>
                                    </v-col>
                                </v-row>
                                <v-divider></v-divider>
                                <v-row justify="center" class="px-2">
                                    <v-col cols="2">
                                        <v-btn height="32" block="" class="elevation-0 " @click="dateRangeMenu = false">
                                            cancel
                                        </v-btn>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col cols="2">
                                        <v-btn height="32" block="" color="primary" class="elevation-0" @click="dateRangeMenu = false">
                                            Done
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-list>
                        </v-menu>
                    </v-col>-->
                    <!--Task-->
                    <!--<v-col md="auto" cols="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="taskMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Task
                                    <v-icon class="ml-12">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="155" max-width="250" min-width="50" class="action-button-list pa-2">
                                <v-checkbox color="success-check-box" input-value="true" label="Output"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Outcome"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Risk"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Survey"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Payment"></v-checkbox>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="taskMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="taskMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>-->
                    <!--Project-->
                    <!--<v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="projectMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Project
                                    <v-icon class="ml-12">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                <v-checkbox color="success-check-box" input-value="true" label="Teach Each"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Mission Home"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="Fundraiser Events"></v-checkbox>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="projectMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="projectMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>-->
                    <!--Action-->
                    <!--<v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="actionMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Action
                                    <v-icon class="ml-12">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                <v-checkbox color="success-check-box" input-value="true" label="To Collect"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" label="To Approve"></v-checkbox>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="actionMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="actionMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>-->
        <!--Payment Term Table-->
        <v-data-table :headers="PaymentTermTableheaders"
                      :items="PaymentTermTableItems"
                      :search="SearchPaymentTerms"
                      show-select
                      item-key="PaymentId"
                      v-model="selectedPaymentTerms">
            <template v-slot:item.PaymentTerms="{item}">
                <v-list class="py-0 table-v-list">
                    <v-list-item class="my-0 px-0">
                        <v-list-item-icon>
                            <v-avatar size="30" color="#fff" class="elevation-1">
                                <v-icon size="20" color="primary">mdi-book</v-icon>
                            </v-avatar>
                        </v-list-item-icon>
                        <v-list-item-content>
                            {{item.PaymentTerms}}
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </template>

            <template v-slot:item.PaymentStatus="{item,ind}">
                <v-icon class="mt-0" size="16" color="#0CBA2F" v-if="item.PaymentStatus == 'Done'">mdi-checkbox-marked-circle</v-icon>
                <v-icon class="mt-0" size="16" color="#DEB514" v-if="item.PaymentStatus == 'Pending'">mdi-minus-circle-outline</v-icon>
                <v-icon class="mt-0" size="16" color="#D50808" v-if="item.PaymentStatus == 'Cancelled'">mdi-close-octagon</v-icon>
                {{item.PaymentStatus}}
                <!--<v-menu offset-y transition="scroll-y-transition" :close-on-content-click="false" v-model="statusMenu[ind]">-->
                <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn text max-width="16" min-width="16" height="20" v-on="on" color="grey" class="elevation-0 mr-1 mt-0" style="margin-top:0px">
                            <v-icon class="mt-0" size="16">mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <div v-for="listItem  in PaymentStatusList" :Key="listItem.StatusID">
                            <v-list-item id="AutoTest_IP526" @click.stop="fnUpdatePaymnetStatus(item.PaymentId,listItem.Status)">
                                <v-list-item-icon>
                                    <v-icon :color="listItem.color">{{listItem.icon}}</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    {{listItem.Status}}
                                </v-list-item-content>
                            </v-list-item>
                        </div>

                    </v-list>
                </v-menu>
            </template>

            <template v-slot:item.PaymentTermActions="{item}">
                <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                    <template v-slot:activator="{ on }">
                        <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                            <v-icon>mdi-dots-vertical</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light width="auto" class="action-button-list">
                        <v-list-item id="AutoTest_IP527" link @click.stop="fnGetPaymentTermsByPaymentId(item.PaymentId)">
                            <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Edit</v-list-item-content>
                        </v-list-item>
                        <v-list-item id="AutoTest_IP528" link @click.stop="RemovePaymentTermDialog = true,PaymentId=item.PaymentId,PaymentTermName=item.PaymentTermName">
                            <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Remove</v-list-item-content>
                        </v-list-item>
                        <v-list-item id="AutoTest_IP529" link @click.stop="GetProjectPTUsers('Donor'),dlgRequestPayment=true">
                            <v-list-item-icon> <v-icon>mdi-check-circle-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Request Payment</v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-menu>
            </template>
        </v-data-table>

        <!-- Add Payment Term Dailog-->
        <!--Create Payment Terms Dialog-->
        <v-dialog v-model="CreatePaymentTermsDialog" persistent width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Create Payment Terms
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP530" text="" @click="ClosepaymentDialog()">
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
                        <v-btn id="AutoTest_IP531" fab="" small="" v-bind:disabled="stepper==1" class="mr-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Minus')">
                            <v-icon size="30" color="primary" style="color:#0964D9 !important">mdi-arrow-left</v-icon>
                        </v-btn>
                        Payment Term {{stepper}} of {{NoOfPayments}}
                        <v-btn id="AutoTest_IP532" fab="" small="" class="ml-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Plus')" v-bind:disabled="stepper==NoOfPayments || !fnCheckMandatory()">
                            <v-icon size="30" color="primary" style="color:#0964D9 !important">mdi-arrow-right</v-icon>
                        </v-btn>
                    </h4>
                    <v-row justify="center" class="pt-4">
                        <v-col md="6" style="height:250px" class="v-scrolling-div">
                            <v-text-field class="mt-4" label="Enter Payment Term Name" outlined="" dense="" v-model="PaymentTermName" :rules="[rules.required]">
                                <template v-slot:append>
                                    <span v-if="PaymentTermName.length==0" class="error--text">*</span>
                                </template>
                            </v-text-field>
                            <v-select dense="" v-model="CreatePaymentTermsLogicModel"
                                      :items="CreatePaymentTermsLogic"
                                      label="Select Logic"
                                      :rules="[rules.required]"
                                      outlined="">
                                <template v-if="CreatePaymentTermsLogicModel == '' || CreatePaymentTermsLogicModel == null" v-slot:append="">
                                    <span class="mandatory-star">*</span>
                                </template>
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
                            <v-text-field type="number" v-show="CreatePaymentTermsLogicModel==='Project Target'" label="Target(%)" outlined="" dense="" v-model="Target" :rules="[rules.required]"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==3) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="Target==0" class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-text-field type="number" label="Type amount" outlined="" dense="" v-model="amount" @change="AddPaymentTermAmt(amount)" :rules="[rules.required]"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==8) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="amount==0" class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-account</v-icon> Notify People
                            </span>
                            <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedNotifyUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                            </v-avatar>
                            <v-avatar v-if="selectedNotifyUsers.length>=4" size="25" class="team-count">
                                +{{selectedNotifyUsers.length-3}}
                            </v-avatar>
                            

                            <v-menu absolute v-model="NotificationSelectPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                <template v-slot:activator="{ on }">
                                    <v-btn id="AutoTest_IP533" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                           @click="GetProjectPTUsers('Donor'),GetProjectPTUsers('Finance')">
                                        <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Notification select pop up-->
                                <v-card>
                                    <v-card-title class="heading-3">
                                        Add people to Notify
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP534" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp=false">
                                            <v-icon size="20">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-divider></v-divider>
                                    <v-card-text class="py-0 mt-1">
                                        <v-row no-gutters>
                                            <v-col class="py-0">
                                                <v-list dense="" class="py-0">
                                                    <v-list-item>
                                                        <v-list-item-content style="font-size:17px">
                                                            Finance Manegers
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                    <v-divider></v-divider>
                                                    <div style="height:150px;" class="v-scrolling-div">
                                                        <!--<template v-for="finUsers in FinanceUsersTemp">
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
                                </template>-->
                                                        <v-data-table :headers="lstuserTableheaders"
                                                                      :items="lstPTFinanceUsers"
                                                                      show-select
                                                                      item-key="UserID"
                                                                      sort-by="UserName"
                                                                      v-model="vmPTFinanceUsers"
                                                                      hide-default-footer
                                                                      hide-default-header>
                                                            <template v-slot:item.UserName="{ item }">
                                                                <v-avatar size="27" light :color="`${getcolor(item.UserName)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="Finance name" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                                {{ item.UserName }}
                                                            </template>
                                                        </v-data-table>
                                                    </div>
                                                </v-list>
                                            </v-col>
                                            <v-divider vertical class="mt-1"></v-divider>
                                            <v-col class="py-0">
                                                <v-list dense="" class="py-0">
                                                    <v-list-item>
                                                        <v-list-item-content style="font-size:17px">
                                                            Donars
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                    <v-divider></v-divider>
                                                    <div style="height:150px;" class="v-scrolling-div">
                                                        <!--<template v-for="DonarUser of DonarUserTemp">
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
                                </template>-->
                                                        <v-data-table :headers="AddDonorTableHeaders"
                                                                      :items="lstPTDonors"
                                                                      show-select
                                                                      item-key="UserID"
                                                                      sort-by="UserName"
                                                                      v-model="vmPTDonors"
                                                                      hide-default-footer
                                                                      hide-default-header>

                                                            <template v-slot:item.UserName="{ item }">
                                                                <v-avatar size="27" light :color="`${getcolor(item.UserName)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="donar name" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                                {{ item.UserName }}
                                                            </template>
                                                        </v-data-table>
                                                    </div>
                                                </v-list>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-divider></v-divider>
                                    <v-card-actions>
                                        <v-row no-gutters="">
                                            <v-col md="12" align="center">
                                                <Success-Button id="AutoTest_IP535" style="width:130px" class="my-1 center-block" title="Save" @click.native="SaveNotifyUsers(),NotificationSelectPopUp=false"></Success-Button>
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
                        <Previous-Button-Outlined id="AutoTest_IP536" v-show="CreatePaymentTermsStepTwo" class="float-right" @click.native="CreatePaymentTermsStepOne=true,CreatePaymentTermsStepTwo=false"></Previous-Button-Outlined>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP537" v-show="CreatePaymentTermsStepOne" class="float-right" @click.native="CreatePaymentTermsDialog=false,NoOfPayments =0"></Cancel-Button-Outlined-Sm>
                        <Next-button-primary-light id="AutoTest_IP538" v-show="CreatePaymentTermsStepOne" class="float-right mr-4" @click.native="CreatePaymentTermsStepTwo=true,CreatePaymentTermsStepOne=false" v-bind:disabled="NoOfPayments==0"></Next-button-primary-light>


                        <v-btn id="AutoTest_IP969" v-show="CreatePaymentTermsStepTwo" v-bind:disabled="stepper!=NoOfPayments || !fnCheckMandatory()" class="elevation-0 float-right mr-4 secondary"
                               height="36" width="122" style="background-color:#0964D9 !important; color:white !important" @click="CreatePaymentTermsDialog=false ,fnInvokePaymentTermList(NoOfPayments),fnSavepaymentTerms()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="CreatePaymentTermSuccessSnackbar" top="" color="primary">
            Payment term successfully created
        </v-snackbar>


        <!-- Edit Paymnet Term Dialog-->
        <v-dialog v-model="EditPaymentTermsDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit Payment Terms
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP540" text @click="CloseEditPaymentTerms()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <!--Step-1-->
                <v-card-text>
                    <v-stepper v-model="CreatePaymentTermsStepper" class="elevation-0">
                        <v-stepper-items>
                            <v-stepper-content step="1">
                                <h4 class="text-center heading-3 mt-4">Edit the basic details of the payment terms.</h4>
                                <v-row justify="center" class="pt-4">
                                    <v-col md="6" style="height:250px" class="v-scrolling-div">
                                        <v-text-field class="mt-4" label="Enter Payment Term Name" outlined dense v-model="PaymentInfo.PaymentTermName"></v-text-field>
                                        <v-select dense v-model="PaymentInfo.Logic"
                                                  :items="CreatePaymentTermsLogic"
                                                  label="Select Logic"
                                                  outlined>
                                        </v-select>

                                        <v-menu v-model="FrmDateMenu"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y=""
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-show="PaymentInfo.Logic=='Date'"
                                                              v-model="new Date(PaymentInfo.paymentdate).toLocaleDateString('en-GB')"
                                                              label="Select Date"
                                                              prepend-inner-icon="mdi-calendar" outlined dense
                                                              persistent-hint
                                                              readonly
                                                              v-on="on" class="mb-2"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="PaymentInfo.paymentdate" no-title @input="FrmDateMenu = false" dense hide-details format="DD-MM-YYYY"
                                                           :max="ProjectEndDate" :min="ProjectStartDate"></v-date-picker>
                                        </v-menu>
                                        <v-text-field type="number" v-show="PaymentInfo.Logic=='Project Target'" label="Target(%)" v-model="PaymentInfo.Target" outlined dense
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==3) return false;" min="0"></v-text-field>

                                        <v-text-field type="number" label="Type amount" v-model="PaymentInfo.Amount" outlined dense
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==8) return false;" min="0"></v-text-field>

                                        <span class="small-text mr-1">
                                            <v-icon size="12">mdi-account</v-icon> Notify People
                                        </span>

                                        <v-avatar size="27" class="team-avatar" v-for="(item,key) in PaymentInfo.users.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                        <v-avatar v-if="PaymentInfo.users.length>=4" size="25" class="team-count">
                                            +{{PaymentInfo.users.length-3}}
                                        </v-avatar>

                                        <v-menu v-model="NotificationSelectPopUp2" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                            <template v-slot:activator="{ on }">
                                                <Add-Button-Fab id="AutoTest_IP971" @click.native="GetPaymentTermsNotifyUsers(),NotificationSelectPopUp2 = true"></Add-Button-Fab>
                                            </template>
                                            <!--Notification select pop up-->
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    Add people to Notify
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP972" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp2=false">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="py-0 mt-1">
                                                    <v-row no-gutters>
                                                        <v-col class="py-0">
                                                            <v-list dense="" class="py-0">
                                                                <v-list-item>
                                                                    <v-list-item-content style="font-size:17px">
                                                                        Finance Manegers
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                                <v-divider></v-divider>
                                                                <div style="height:150px;" class="v-scrolling-div">
                                                                    <v-data-table :headers="lstuserTableheaders"
                                                                                  :items="lstPTFinanceUsers"
                                                                                  show-select
                                                                                  item-key="UserID"
                                                                                  sort-by="UserName"
                                                                                  v-model="vmPTFinanceUsers"
                                                                                  hide-default-footer
                                                                                  hide-default-header>
                                                                        <template v-slot:item.UserName="{item}">
                                                                            <v-avatar size="27" light :color="`${getcolor(item.UserName)}`">
                                                                                <img v-bind:src="userimage + item.ProfileImage" alt="Finance name" v-if="item.ProfileImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            {{item.UserName}}
                                                                        </template>
                                                                    </v-data-table>
                                                                    <!--<template v-for="finUsers in FinanceUsersTemp">
                                    <v-list-item>
                                        <v-list-item-icon style="margin-top:-10px;">
                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(finUsers.ID,'Finance', $event)" v-model="finUsers.IsSelected"></v-checkbox>
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
                                </template>-->
                                                                </div>
                                                            </v-list>
                                                        </v-col>
                                                        <v-divider vertical class="mt-1"></v-divider>
                                                        <v-col class="py-0">
                                                            <v-list dense="" class="py-0">
                                                                <v-list-item>
                                                                    <v-list-item-content style="font-size:17px">
                                                                        Donars
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                                <v-divider></v-divider>
                                                                <div style="height:150px;" class="v-scrolling-div">
                                                                    <v-data-table :headers="AddDonorTableHeaders"
                                                                                  :items="lstPTDonors"
                                                                                  show-select
                                                                                  item-key="UserID"
                                                                                  sort-by="UserName"
                                                                                  v-model="vmPTDonors"
                                                                                  hide-default-footer
                                                                                  hide-default-header>

                                                                        <template v-slot:item.UserName="{item}">
                                                                            <v-avatar size="27" light :color="`${getcolor(item.UserName)}`">
                                                                                <img v-bind:src="userimage + item.ProfileImage" alt="donar name" v-if="item.ProfileImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            {{item.UserName}}
                                                                        </template>
                                                                    </v-data-table>
                                                                    <!--<template v-for="DonarUser of DonarUserTemp">
                                    <v-list-item>
                                        <v-list-item-icon style="margin-top:-10px;">
                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(DonarUser.ID,'Donar', $event)" v-model="DonarUser.IsSelected"></v-checkbox>
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
                                </template>-->
                                                                </div>
                                                            </v-list>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row no-gutters="">
                                                        <v-col md="12" align="center">
                                                            <Success-Button id="AutoTest_IP973" style="width:130px" class="py-1 center-block" title="Save"
                                                                            @click.native="SaveNotifyUsers(),PaymentInfo.users =selectedNotifyUsers, NotificationSelectPopUp2=false"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                            <!--__________________________-->
                                        </v-menu>

                                        <!--<Add-Button-Fab id="AutoTest_IP541" @click.native="GetPaymentTermsNotifyUsers(),NotificationSelectPopUp2 = true"></Add-Button-Fab>-->
                                        <!--Notification select pop up-->
                                        <!--<v-card class="position-absolute-popup-sm" width="50vw" style="right:20px; top:90px" v-if="NotificationSelectPopUp2">

    <v-card-title class="heading-3">
        Add people to Notify
        <v-spacer></v-spacer>
        <v-btn id="AutoTest_IP542" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp2=false">
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
                    <v-divider></v-divider>-->
                                        <!--<template v-for="finUsers in FinanceUsersTemp">
        <v-list-item>
            <v-list-item-icon style="margin-top:-10px;">
                <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(finUsers.ID,'Finance', $event)" v-model="finUsers.IsSelected"></v-checkbox>
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
    </template>-->
                                        <!--<v-data-table :headers="lstuserTableheaders"
                          :items="lstPTFinanceUsers"
                          show-select
                          item-key="UserID"
                          sort-by="UserName"
                          v-model="vmPTFinanceUsers"
                          hide-default-footer
                          hide-default-header>
                <template v-slot:item.UserName="{item}">
                    <v-avatar size="27" left class="my-0 px-0">
                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Finance name" />
                    </v-avatar>
                    {{item.UserName}}
                </template>
            </v-data-table>
        </v-list>
    </v-col>
    <v-col class="py-0">
        <v-list dense="" class="py-0">
            <v-list-item>
                <v-list-item-content style="font-size:17px">
                    Donars
                </v-list-item-content>
            </v-list-item>
            <v-divider></v-divider>-->
                                        <!--<template v-for="DonarUser of DonarUserTemp">
        <v-list-item>
            <v-list-item-icon style="margin-top:-10px;">
                <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(DonarUser.ID,'Donar', $event)" v-model="DonarUser.IsSelected"></v-checkbox>
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
    </template>-->
                                        <!--<v-data-table :headers="AddDonorTableHeaders"
                                      :items="lstPTDonors"
                                      show-select
                                      item-key="UserID"
                                      sort-by="UserName"
                                      v-model="vmPTDonors"
                                      hide-default-footer
                                      hide-default-header>

                            <template v-slot:item.UserName="{item}">
                                <v-avatar size="27" left class="my-0 px-0">
                                    <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="Donor name" />
                                </v-avatar>
                                {{item.UserName}}
                            </template>
                        </v-data-table>
                    </v-list>
                </v-col>
            </v-row>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
            <v-row no-gutters="">
                <v-col md="12" align="center">
                    <Success-Button id="AutoTest_IP543" style="width:130px" class="mb-2 center-block" title="Save" @click.native="SaveNotifyUsers(),PaymentInfo.users =selectedNotifyUsers, NotificationSelectPopUp2=false"></Success-Button>
                </v-col>
            </v-row>
        </v-card-actions>
    </v-card>-->
                                        <!--__________________________-->
                                    </v-col>

                                </v-row>
                            </v-stepper-content>
                        </v-stepper-items>
                    </v-stepper>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Cancel-Button-Outlined-Sm id="AutoTest_IP544" class="float-right" @click.native="CloseEditPaymentTerms()"></Cancel-Button-Outlined-Sm>
                        <v-btn id="AutoTest_IP545" class="elevation-0 float-right mr-4 primary-btn-light" v-bind:disabled="fnCheckEditMandatory()" dark height="36" width="122"
                               @click="EditPaymentTermsDialog=false,fnUpdatepaymentTerms()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="UpdatePaymentTermSuccessSnackbar" top="" color="primary">
            Payment term successfully updated
        </v-snackbar>

        <v-snackbar v-model="snackbarAlertMsg" top="" color="primary">
            {{Msg}}
        </v-snackbar>

        <!-- Remove Payment Terms-->
        <v-dialog v-model="RemovePaymentTermDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Paymnet Terms
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP546" text="" @click="RemovePaymentTermDialog=false">
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
                                            <h4 class="text-center heading-3">Are you sure you want to remove this payment terms ?</h4>
                                        </v-col>
                                        <v-col>
                                            <h4 class="text-center heading-3">
                                                <v-icon class="tmr-2" color="#707070" size="25">mdi-book</v-icon>
                                                {{PaymentTermName}}
                                            </h4>
                                        </v-col>

                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP547" color="secondary elevation-0" class="mr-2" @click="DeletePaymentTerm(2),RemovePaymentTermDialog = false">Remove</v-btn>
                                            <v-btn id="AutoTest_IP548" outlined color="grey" @click="RemovePaymentTermDialog=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="RemovePaymentTermSuccessSnackbar" top="" color="primary">
            Payment term successfully removed
        </v-snackbar>
        <v-snackbar v-model="statusPaymentTermSuccessSnackbar" top="" color="primary">
            Payment status successfully updated
        </v-snackbar>
        <v-snackbar v-model="ErrorPaymentTermSuccessSnackbar" top="" color="error">
            Failed
        </v-snackbar>
        <v-dialog v-model="dlgRequestPayment" width="800" persistent>
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Request Payment
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP549" text="" @click="ClearRequestPayment()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>

                    <h4 class="text-center heading-3 mt-4">Please enter the basic details for requesting payment.</h4>
                    <v-row justify="center" class="mt-4">
                        <v-col md="6" class="v-scrolling-div">
                            <v-text-field outlined="" dense="" label="Amount" v-model="RequestAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==8) return false;" min="0">
                            </v-text-field>
                            <v-textarea rows="3" outlined dense label="Enter comment" type="text" v-model="RequestComment">
                            </v-textarea>

                            <v-file-input label="Upload attachment" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                          v-model="fileInput" id="file"></v-file-input>

                            <v-select dense="" v-model="RequestFundDonorUser"
                                      label="Select Donar" :items="lstPTDonors"
                                      item-text="UserName" item-value="UserID"
                                      outlined="">
                                <!--for option with image-->
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
                            </v-select>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions>
                    <v-col md="12">
                        <Outlined-Button-Dark id="AutoTest_IP550" class="btn-122x36 float-right" title="Cancel" @click.native="ClearRequestPayment()"></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_IP551" class="btn-122x36 float-right mr-4" title="Save" @click.native="SavePaymentRequest()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
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

    var objManageProject;
    async function importscriptManageProject() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };


    var objPrjFinance;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then(mod => {
                objPrjFinance = new mod.ManageProjectFinance();
            })
        ]);
    };

    export default {
        data() {
            return {
                rules: {
                    required: value => !!value || 'Required.',
                },
                 screenAccess: 0,
                RWAccess: 0,
                PaymentTermTableheaders: [
                    { text: 'ID', align: ' d-none', value: 'PaymentId' },
                    { text: "Payment Terms", value: "PaymentTermName" },
                    { text: "Logic", value: "Logic" },
                    { text: "Amount", value: "Amount" },
                    { text: "Payment Status", value: "PaymentStatus" },
                    { text: "Action", value: "PaymentTermActions" },
                ],
                PaymentTermTableItems: [],
                selectedPaymentTerms: [],
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectId: 0,
                PaymentId: 0,
                PaymentTermName: '',
                SearchPaymentTerms: '',
                PaymentStatusList: [
                    { StatusID: 1, Status: 'Done', icon: 'mdi-checkbox-marked-circle', Color: '#0CBA2F' },
                    { StatusID: 2, Status: 'Pending', icon: 'mdi-minus-circle-outline', Color: '#DEB514' },
                    { StatusID: 3, Status: 'Cancelled', icon: 'mdi-close-octagon', Color: '#D50808' }
                ],
                CreatePaymentTermsDialog: false,
                CreatePaymentTermsLogic: ['Date', 'Project Target'],
                CreatePaymentTermsStepOne: true,
                CreatePaymentTermsStepTwo: false,
                CreatePaymentTermsStepThree: false,
                CreatePaymentTermsLogicModel: '',
                PaymentDateMenu: false,
                CreatePaymentTermsStepper: 1,
                NotificationSelectPopUp: false,
                NotificationSelectPopUp2: false,
                EditPaymentTermsDialog: false,
                RemovePaymentTermDialog: false,
                RemoveBulkPaymentTermDialog: false,
                CreatePaymentTermSuccessSnackbar: false,
                UpdatePaymentTermSuccessSnackbar: false,
                RemovePaymentTermSuccessSnackbar: false,
                statusPaymentTermSuccessSnackbar: false,
                ErrorPaymentTermSuccessSnackbar: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                PaymentInfo: {
                    PaymentId: 0,
                    PaymentTermName: "",
                    Logic: "",
                    Target: "",
                    paymentdate: "",
                    Target: 0,
                    Amount: "",
                    users: [],
                },
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

                FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                lstPTFinanceUsers: [],
                lstPTDonors: [],
                vmPTFinanceUsers: [],
                vmPTDonors: [],
                selectedNotifyUsers: [],
                lstProjectUsers: [],
                RequestFundDonorUser: [],
                fileInput: null,
                RequestComment: "",
                RequestAmount: "",
                dlgRequestPayment: false,
                Msg: "",
                snackbarAlertMsg: false,

                lstuserTableheaders: [{ text: "Name", align: "left", value: "UserName" }],
                AddDonorTableHeaders: [
                    { text: "Name", align: "left", value: "UserName" },
                    { text: '', value: 'data-table-expand' },
                ],
                ProjectEndDate: new Date().toISOString().substr(0, 10),
                ProjectStartDate: new Date().toISOString().substr(0, 10),
                statusMenu :[],
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
            'Bulk-Action-Button-Outlined': BulkActionButtonOutlined,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,

        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.InitiativeID = this.$route.query.InitID;
            await importscript();
            await importscriptManageProject();
            await this.fnGetProjectDetailsByID();
            await this.fnGetPaymentTermsByprjID();

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

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },

            async fnGetPaymentTermsByprjID() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objPrjFinance.GetPaymentTermsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.PaymentTermTableItems = result.data;
                        for(var i=0;i<this.PaymentTermTableItems.length;i++)
                            this.PaymentTermTableItems.statusMenu[i] =false;
                    }
            },

            async fnGetPaymentTermsByPaymentId(paymentID) {

                var data = new FormData();
                data.append("PaymentId", paymentID);
                var result = await objPrjFinance.GetPaymentTermsByPaymentId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.PaymentInfo = result.data;
                        this.PaymentInfo.users = JSON.parse(result.data.users);
                        this.PaymentId = this.PaymentInfo.PaymentId;
                        this.PaymentTermName = this.PaymentInfo.PaymentTermName;
                    }
                this.EditPaymentTermsDialog = true;
                this.GetProjectPTUsers('Donor');
                this.GetProjectPTUsers('Finance');

            },

            async GetProjectUsers(Type) {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("GroupID", this.GroupId);
                data.append("Type", Type)
                var result = await objPrjFinance.GetProjectUsers(data);
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

            fnBulkRemoveClick() {
                if (this.selectedPaymentTerms.length > 0) {
                    this.RemoveBulkPaymentTermDialog = true;
                } else {
                    this.Msg = "Please select atleast 1 payment Term";
                    this.snackbarAlertMsg = true;
                }
            },
            async DeletePaymentTerm(Type) {
                var data = new FormData();
                if (Type == 1) {
                    data.append("PaymentId", JSON.stringify(this.selectedPaymentTerms));
                    data.append("Type", 1);
                }
                else {
                    data.append("PaymentId", JSON.stringify(this.PaymentId));
                    data.append("Type", 2);
                }
                var result = await objPrjFinance.DeletePaymentTerm(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.RemovePaymentTermSuccessSnackbar = true;
                    } else {

                        this.ErrorPaymentTermSuccessSnackbar = true;
                    }
                }
                this.fnGetPaymentTermsByprjID();
                this.PaymentId = 0;
                this.PaymentInfo.users = [];
                this.selectedFinanceUser = [];
                this.PaymentTermName = "";
            },
            async fnUpdatePaymnetStatus(paymentId, PaymnetStatus) {

                //this.statusMenu[indx] =false;
                var data = new FormData();
                data.append("PaymentId", paymentId);
                data.append("PaymnetStatus", PaymnetStatus);
                var result = await objPrjFinance.UpdatePaymnetStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    this.statusPaymentTermSuccessSnackbar = true;
                    this.fnGetPaymentTermsByprjID();




                }

            },
            async fnUpdatepaymentTerms() {
                var data = new FormData();
                data.append("PaymentId", this.PaymentId);
                data.append("PaymentTermName", this.PaymentInfo.PaymentTermName);
                data.append("Logic", this.PaymentInfo.Logic);
                data.append("paymentdate", this.PaymentInfo.paymentdate);
                data.append("Target", this.PaymentInfo.Target);
                data.append("Amount", this.PaymentInfo.Amount);
                data.append("users", JSON.stringify(this.PaymentInfo.users));
                data.append("ProjectId", this.ProjectId);
                var result = await objPrjFinance.UpdatepaymentTerms(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.UpdatePaymentTermSuccessSnackbar = true;
                        this.fnGetPaymentTermsByprjID();
                        this.EditPaymentTermsDialog = false;
                        this.PaymentInfo.users = [];
                        this.selectedFinanceUser = [];
                        this.PaymentId = "";
                        this.PaymentTermName = "";
                    }
                }

            },
            fnCheckEditMandatory() {
                if (this.PaymentInfo.PaymentTermName == "" || this.PaymentInfo.Logic == "" || this.PaymentInfo.Amount == "" || this.PaymentInfo.Amount == 0) {

                    if (this.PaymentInfo.Logic == "Date") {
                        this.PaymentInfo.paymentdate == ""
                    }
                    else {
                        this.PaymentInfo.Target == ""
                    }
                    return true;
                }
                else
                    return false;
            },


            async FilterInput(event) {

                var isNotWanted;
                var keyCode = ('which' in event) ? event.which : event.keyCode;

                isNotWanted = (keyCode == 69 || keyCode == 101 || keyCode == 109 || keyCode == 189);
                if (isNotWanted == true) {

                    event.preventDefault();
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
                        this.selectedNotifyUsers = temp[0].NotifyUserList;
                        for (var i = 0; i < this.selectedNotifyUsers.length; i++) {
                            if (this.selectedNotifyUsers[i].UserType == 'F')
                                this.vmPTFinanceUsers.push(this.selectedNotifyUsers[i]);
                            else if (this.selectedNotifyUsers[i].UserType == 'D')
                                this.vmPTDonors.push(this.selectedNotifyUsers[i]);
                        }
                    }
                    else {

                        this.PaymentTerm.PaymentTermList.push({
                            Index: index - 1, PaymentTermName: this.PaymentTermName, Logic: this.CreatePaymentTermsLogicModel,
                            paymentdate: this.paymentdate, Target: this.Target, amount: this.amount, NotifyUserList: this.selectedNotifyUsers
                        });
                        this.PaymentTermName = "";
                        this.paymentdate = new Date().toISOString().substr(0, 10);
                        this.Target = "";
                        this.amount = "";
                        this.CreatePaymentTermsLogicModel = '';
                        this.selectedNotifyUsers = [];
                        this.vmPTDonors = [];
                        this.vmPTFinanceUsers = [];

                    }
                }
            },
            async fnInvokePrevPaymentTermList(index) {

                if (index + 1 == this.NoOfPayments) {
                    if (this.PaymentTermName != "") {
                        this.PaymentTerm.PaymentTermList.push({
                            Index: this.NoOfPayments, PaymentTermName: this.PaymentTermName, Logic: this.CreatePaymentTermsLogicModel,
                            paymentdate: this.paymentdate, Target: this.Target, amount: this.amount, NotifyUserList: this.selectedNotifyUsers
                        });
                        this.vmPTDonors = [];
                        this.vmPTFinanceUsers = [];

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
                        this.selectedNotifyUsers = temp[0].NotifyUserList;
                        for (var i = 0; i < this.selectedNotifyUsers.length; i++) {
                            if (this.selectedNotifyUsers[i].UserType == 'F')
                                this.vmPTFinanceUsers.push(this.selectedNotifyUsers[i]);
                            else if (this.selectedNotifyUsers[i].UserType == 'D')
                                this.vmPTDonors.push(this.selectedNotifyUsers[i]);
                        }
                    }
                }
            },
            async fnSavepaymentTerms() {
                var data = new FormData();
                data.append("NoOfPaymentTerms", this.NoOfPayments);
                data.append("PaymentTermList", JSON.stringify(this.PaymentTerm.PaymentTermList));
                data.append("Notify", this.NotifyCheckbox);
                data.append("ProjectId", this.ProjectId);
                var result = await objPrjFinance.SavepaymentTerms(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.Msg = " Payment term successfully created";
                        this.snackbarAlertMsg = true;
                        this.ClosepaymentDialog();
                        this.fnGetPaymentTermsByprjID();
                    }
                    else {
                        this.Msg = " Failed in adding Payment term ..";
                        this.snackbarAlertMsg = true;
                    }
                }

            },
            fnCheckMandatory() {
                if (this.PaymentTermName == "") {
                    return false;
                }
                if (this.amount == "" || this.amount == "0") {
                    return false;
                }
                if (this.selectedNotifyUsers == 0) { return false; }
                if (this.CreatePaymentTermsLogicModel == "Date") {
                    if (this.paymentdate == "") {
                        return false;
                    }
                } else if (this.CreatePaymentTermsLogicModel == "Project Target") {
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

            fnAddToTempUserList() {
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
            },

            async GetProjectPTUsers(Type) {

                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("GroupID", this.GroupId);
                data.append("Type", Type)
                var result = await objPrjFinance.GetProjectUsers(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {

                        if (Type == 'Finance')
                            this.lstPTFinanceUsers = result.data;
                        else if (Type == 'Donor')
                            this.lstPTDonors = result.data;
                    }

                }

            },
            async fnGetProjectDetailsByID() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objManageProject.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.data) {

                        this.projectInfo = result.data;
                        this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
                        this.ProjectEndDate = this.projectInfo.EndDate.substr(0, 10)//this.formatDate(this.projectInfo.EndDate.substr(0, 10));
                        this.ProjectStartDate = this.projectInfo.StartDate.substr(0, 10)// this.formatDate(this.projectInfo.StartDate.substr(0, 10));
                        this.GroupId = result.data.GroupId;
                    }
                }
            },
            async SavePaymentRequest() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("DonorUser", this.RequestFundDonorUser);
                data.append("RequestAmount", this.RequestAmount);
                data.append("Comment", this.RequestComment);
                data.append("ReqDocument", this.fileInput.name);
                data.append("ProjectName", this.projectInfo.ProjectName);
                var result = await objPrjFinance.SavePaymentRequest(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.UploadDocument();
                        this.Msg = " Payment Requested successfully ";
                        this.snackbarAlertMsg = true;
                    }
                    else {
                        this.Msg = "Failed to add payment request ";
                        this.snackbarAlertMsg = false;
                    }
                    this.dlgRequestPayment = false;
                }
            },
            ClearRequestPayment() {
                this.dlgRequestPayment = false;
                this.RequestFundDonorUser = [];
                this.RequestAmount = 0;
                this.RequestComment = "";
                this.fileInput = null;
            },
            formatFileSize(bytes, decimalPoint) {

                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            async UploadDocument() {

                const formData = new FormData()
                formData.append('myFile', this.fileInput);
                formData.append('DocumentOriginalName', this.fileInput.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', this.formatFileSize(this.fileInput.size, 0));
                formData.append('DocumentType', this.fileInput.type);
                formData.append('DocumentTypeIcon', this.fileInput.name.split('.').pop());
                if (this.fileInput.lastModifiedDate != null) {
                    var lstModDate = new Date(this.fileInput.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.fileInput.lastModifiedDate).toISOString().substr(11, 5);
                    formData.append('DocumentLastModifiedDate', lstModDate);

                }
                formData.append('ProjectId', this.ProjectID);
                formData.append('UploadedFrom', "Project");
                formData.append('LinkedTo', this.ProjectName);

                var result = await objProject.UploadDocument(formData);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status != "success") {
                        this.Msg = "Failed in uploading document...";
                        this.snackbarActions = true;
                    }

                }
            },

            SaveNotifyUsers() {

                var tmpList = [];
                for (var i = 0; i < this.vmPTFinanceUsers.length; i++)
                    tmpList.push(this.vmPTFinanceUsers[i]);

                for (var i = 0; i < this.vmPTDonors.length; i++)
                    tmpList.push(this.vmPTDonors[i]);
                this.selectedNotifyUsers = [];
                for (var i = 0; i < tmpList.length; i++) {
                   // if (this.selectedNotifyUsers.filter(x => x.UserID == tmpList[i].UserID).length == 0)
                        this.selectedNotifyUsers.push(tmpList[i]);
                }
                this.NotificationSelectPopUp = false;

            },
            AddPaymentTermAmt(Amt) {

                this.totermsAmt = parseInt(this.totermsAmt) + parseInt(Amt);
                if (parseInt(this.budgetAmt) < (parseInt(this.projectInfo.PaymentTermAmount) + parseInt(this.totermsAmt))) {
                    this.Msg = "Total Budget Amount should not be greater than the sum of PaymentTerm Amount";
                    this.snackbarAlertMsg = true;
                    this.totermsAmt = parseInt(this.totermsAmt) - parseInt(Amt);
                    this.amount = "";
                }
            },
            CloseEditPaymentTerms() {

                this.NotificationSelectPopUp2 = false;
                this.fnGetPaymentTermsByprjID();
                this.EditPaymentTermsDialog = false;
                this.PaymentInfo.users = [];
                this.selectedNotifyUsers = [];
                this.vmPTDonors = [];
                this.vmPTFinanceUsers = [];
                this.PaymentId = "";
                this.PaymentTermName = "";
            },
            async GetPaymentTermsNotifyUsers() {

                this.vmPTDonors = [];
                this.vmPTFinanceUsers = [];

                for (var i = 0; i < this.PaymentInfo.users.length; i++) {

                    for (var j = 0; j < this.lstPTFinanceUsers.length; j++)
                        if (this.PaymentInfo.users[i].UserID == this.lstPTFinanceUsers[j].UserID)
                            this.vmPTFinanceUsers.push(this.PaymentInfo.users[i]);
                    for (var j = 0; j < this.lstPTDonors.length; j++)
                        if (this.PaymentInfo.users[i].UserID == this.lstPTDonors[j].UserID)
                            this.vmPTDonors.push(this.PaymentInfo.users[i]);
                }

            },

        }

    }

</script>
