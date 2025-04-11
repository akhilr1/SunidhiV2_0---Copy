<template>
    <div>
        <v-card class="elevation-0">
            <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
            <v-card-title style="height:60px">
                <h5 class="text-left page-head">
                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-cash-marker</v-icon>
                    </v-avatar>
                    Funds & Utilization
                </h5>
            </v-card-title>
            <v-card class="elevation-0 pt-0">
                <v-card-text class="pt-0 pb-0" style="height:100%">
                    <v-row>
                        <!--Budget And Fund-->
                        <v-col md="6" class="pb-0" >
                            <v-card class="custom-card-1 ">
                                <v-card-title style="height:60px">
                                    {{BudgetAndFundHeader}}
                                    <v-spacer></v-spacer>
                                         <router-link
                                          :to="{
                                            name: 'FinanceBudgetfundsViewAll',
                                            name: 'FinanceBudgetfundsViewAll',
                                            query: {
                                              WsID: WorkspaceID,
                                              Access: RWAccess,
                                              InitID: InitiativeID,
                                              ProjID: ProjectID,
                                            },
                                          }"
                                        >
                                          <Primary-Button-Outlined
                                            title="View All"
                                            class="mr-4"
                                          ></Primary-Button-Outlined>
                                        </router-link>
                                    <h4 class="text-center black--text heading-3">
                                        <!-- Previous Button-->
                                        <v-btn id="AutoTest_F1"  style="height:34px; width:20px;border-radius:50%" x-small="" class="mr-2 elevation-1" color="#fff" @click="fnNextChart('Minus')" v-bind:disabled="BudgetAndFinanceSteps == 1">
                                            <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                                        </v-btn>
                                        <!-- Next Button-->
                                        <v-btn id="AutoTest_F2"  style="height:34px; width:20px;border-radius:50%" x-small="" class="ml-2 elevation-1" color="#fff" @click="fnNextChart('Plus')" v-bind:disabled="BudgetAndFinanceSteps == 3">
                                            <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                                        </v-btn>
                                    </h4>
                                </v-card-title>
                                <v-card-text style="height:300px" class="v-scrolling-div pa-0">
                                    <v-stepper v-model="BudgetAndFinanceSteps" class="elevation-0">
                                        <v-stepper-items>
                                            <!--  Add Donor and Finance User-->
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-row>
                                                    <v-col md="6">
                                                        <v-row>
                                                            <v-col>
                                                                <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                                    <canvas id="myBudgetAndFundChart" style="width:175px; height:150px"></canvas>
                                                                </div>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col>
                                                                <h5 class="heading-3-dark text-center">
                                                                    Total budget<br />
                                                                    <span class="bold">{{ getSelectedCurrency.symbol }} {{budgetAmt}}</span>
                                                                </h5>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col md="6">

                                                        <v-row>
                                                            <v-col class="heading-3-dark">
                                                                <v-icon size="20" color="#000" class="mr-1">mdi-account</v-icon>Donors
                                                                <v-btn id="AutoTest_F3" class="ml-2" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                                       @click="dlgAddDonor = true,GetProjectUsers('Donor'), TitleText ='Add Donor', Subheadtext = AddSubHeadText" v-bind:disabled="!screenAccess">
                                                                    <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                                </v-btn>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row class="mr-1 v-scrolling-div" style="max-height:180px">
                                                            <v-col class="px-0 py-1">
                                                                <template v-for="(item,index) in DonarFundList">
                                                                    <h4 class="heading-3-dark text-left mb-2 mt-1">
                                                                        <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                                        <v-avatar size="15" class="team-avatar mr-2" :color="item.color" tile>
                                                                        </v-avatar>
                                                                        <v-avatar size="27" light :color="`${getcolor(item.UserName)}`" class="mr-2">
                                                                            <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                                                            <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                        </v-avatar>
                                                                        {{item.UserName }}
                                                                        <v-btn id="AutoTest_F4" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                                               @click="EditDonor(item), TitleText ='Edit Donor', Subheadtext = EditSubHeadText" v-bind:disabled="!screenAccess">
                                                                            <v-icon size="15" light="" color="#707070">mdi-pencil</v-icon>
                                                                        </v-btn>&nbsp;&nbsp;&nbsp;
                                                                        <v-btn id="AutoTest_F5" class="mr-2" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                                               @click="RemoveUserDailog=true,DUserID=item.UserID,DUserName=item.UserName,DUserImage=item.ProfileImage,BudgetFundID=item.BudgetFundID" v-bind:disabled="!screenAccess">
                                                                            <v-icon size="15" light="" color="#707070">mdi-close</v-icon>
                                                                        </v-btn>&nbsp;&nbsp;&nbsp;
                                                                    </h4>
                                                                    <h2 class="text-left heading-3-dark mb-2 mt-1">Amount funded : {{ getSelectedCurrency.symbol }} {{item.DonorBudgetAmount }}</h2>
                                                                </template>
                                                                <template>
                                                                    <h4 class="text-left heading-3-dark mb-2">
                                                                        <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                                        <v-avatar size="15" class="team-avatar" color="#E5E5E5" tile></v-avatar>
                                                                        Not Funded Amount: {{ getSelectedCurrency.symbol }} {{DonarNotFundedFund}}
                                                                    </h4>
                                                                </template>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>

                                            </v-stepper-content>
                                            <!-- Fund Utilization-->
                                            <v-stepper-content step="2" class="pa-0">
                                                <!--For Utilization when data exist chart-->
                                                <v-row v-show="ExpenseList.length > 0">
                                                    <v-col md="6">
                                                        <v-row>
                                                            <v-col>
                                                                <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                                    <div style="width: 100%; height: 40px; position: absolute; top: 50%; left: 0; margin-top: -10px; line-height:19px; text-align: center;">

                                                                    </div>
                                                                    <canvas id="ExpenseChart" style="width:175px; height:150px"></canvas>
                                                                </div>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col>
                                                                <h5 class="heading-3-dark text-center">
                                                                    Total budget<br />
                                                                    <span class="bold">{{ getSelectedCurrency.symbol }} {{budgetAmt}}</span>
                                                                </h5>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col md="6">
                                                        <h4 class="text-left mt-1 px-0 heading-3-dark">Legend Title</h4>
                                                        <v-row class="mt-1 mr-2 ml-1 px-0  v-scrolling-div" style="max-height:190px">
                                                            <v-col class="px-0 py-1">
                                                                <template v-for="(item, ExpenseTagID) in ExpenseList">
                                                                    <h4 class="text-left heading-3-dark mb-2 mt-1">
                                                                        <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                                        <v-avatar size="15" class="team-avatar" :color="item.color" tile></v-avatar>

                                                                        {{item.BudgetHeadName}}:  {{ getSelectedCurrency.symbol }} {{item.HeadAmount}}
                                                                    </h4>
                                                                </template>
                                                                <template>
                                                                    <h4 class="text-left heading-3 mb-2 mt-1">
                                                                        <v-avatar size="15"
                                                                                  class="team-avatar"
                                                                                  color="#E5E5E5"
                                                                                  tile></v-avatar>
                                                                        Unutilized Fund: {{ getSelectedCurrency.symbol }} {{ UnUtilizedFund }}
                                                                    </h4>
                                                                </template>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>
                                                <!--For Utilization legend table-->
                                                <v-row v-show="PaymentfilterList.length > 0" >
                                                    <v-col cols="12" md="12" sm="12" xs="12" class="px-0 py-0">
                                                        <v-card class="custom-card-1 elevation-0">
                                                            <v-card-title style="height:50px" class="py-0 pl-7">
                                                                Project Expenses
                                                                <v-spacer></v-spacer>
                                                                <v-select :items="BudgetHeadList"
                                                                          item-text="BudgetHeadName"
                                                                          item-value="BudgetHeadID"
                                                                          style="max-width:180px;"
                                                                          outlined=""
                                                                          hide-selected=""
                                                                          dense=""
                                                                          class="mr-2"
                                                                          :menu-props="{ maxHeight: '200' }"
                                                                          @change="fnClickFilterDone(BudgetHead)"
                                                                          v-model="BudgetHead" hide-details></v-select>
                                                            </v-card-title>
                                                            <v-card-text style="height:100%" class="pa-0">
                                                                <v-data-table :headers="LedgerBookTableheaders"
                                                                              :items="PaymentfilterList"
                                                                              class="elevation-0"
                                                                              hide-default-footer
                                                                              item-key="PaymentId"
                                                                              fixed-header
                                                                              height="25vh">
                                                                    <template v-slot:item.PaymentActivityName="{item}">
                                                                        <v-list class="py-0 table-v-list" style="max-width:100px">
                                                                            <div class="pa-0 text-limit" style="width:120px">
                                                                                <v-avatar size="20" color="#fff" class="elevation-1">
                                                                                    <v-icon size="12" color="primary">mdi-book</v-icon>
                                                                                </v-avatar>
                                                                                    {{ item.PaymentActivityName }}
                                                                            </div>
                                                                        </v-list>
                                                                    </template>
                                                                    <template v-slot:item.TransactionId="{ item }" >
                                                                        <div v-if="item.TransactionId" class="pa-0 text-limit" style="max-width:100px">
                                                                            {{ item.TransactionId}}
                                                                        </div>
                                                                    </template>
                                                                    <template v-slot:item.TransactionDate="{item}">
                                                                        <div v-if="item.TransactionDate" class="pa-0 text-limit" style="width:85px">
                                                                            {{ formatDate(item.TransactionDate) }}
                                                                        </div>
                                                                    </template>
                                                                    <template v-slot:item.TransactionAmount="{ item }" >
                                                                        <div v-if="item.TransactionAmount" class="pa-0 text-limit" style="max-width:90px">
                                                                            {{ getSelectedCurrency.symbol }} {{ item.TransactionAmount }}
                                                                         </div>
                                                                    </template>
                                                                    <template v-slot:item.TransactionDocName="{item}">
                                                                        <div v-if="item.TransactionDocName" class="pa-0 text-limit handPointer" style="max-width:100px" @click="DownloadDocument(item.TransactionDocName,'/Documents/7e23e75d-bd85-4c3d-82ad-a07585551986.jpg')">
                                                                            <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                                                                                <v-icon size="10" color="#fff">mdi-download</v-icon>
                                                                            </v-avatar> {{ item.TransactionDocName }}
                                                                        </div>
                                                                        <!--<div v-if="item.TransactionDocName" style="max-width:100px">
                                                                            <a v-bind:href="userimage + '/Documents/22bd6a00-dc48-49f8-8b52-49e875dd0d52.pdf'" target="_blank" download>
                                                                                {{ item.TransactionDocName }}
                                                                                <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                                                                                    <v-icon size="10" color="#fff">mdi-check</v-icon>
                                                                                </v-avatar>
                                                                            </a>
                                                                        </div>-->
                                                                    </template>
                                                                </v-data-table>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <!--For Utilization when no data exist chart-->
                                                <v-row v-show="ExpenseList.length == 0">
                                                    <v-col class="text-center">
                                                        <!--<v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                        <h4 class="title text-center" color="#585858">No fund utilizations available</h4>-->
                                                        <p class="mt-6 pt-6">
                                                            <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                                        </p>

                                                        <p class="heading-4-dark font-weight-medium pt-0">No fund utilizations available</p>
                                                    </v-col>
                                                </v-row>

                                            </v-stepper-content>
                                            <!-- Bar chart-->
                                            <v-stepper-content step="3" class="pa-0 ma-0 px-0 py-0">
                                                <v-row v-show="ExpenseList.length >0" class="py-0 px-0">
                                                    <v-col md="12" class="py-0">
                                                        <v-row class="py-0">
                                                            <v-col class="text-left py-0 pl-3 pt-1">
                                                                <canvas id="myBarChart" style="width:580px; height:300px"></canvas>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col>
                                                                <h5 class="heading-3-dark text-center">
                                                                    Total budget<br />
                                                                    <span class="bold">{{budgetAmt}}</span>
                                                                </h5>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>
                                                <v-row v-show="ExpenseList.length == 0" justify="center">
                                                    <v-col class="text-center">
                                                        <p class="mt-6 pt-6">
                                                            <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                                        </p>
                                                        <p class="heading-4-dark font-weight-medium pt-0">No data to show</p>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper-content>
                                        </v-stepper-items>
                                    </v-stepper>
                                </v-card-text>
                            </v-card>
                            <!-- Add donor dailog box-->
                            <v-dialog v-model="dlgAddDonor" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-account</v-icon>
                                        </v-avatar>
                                        {{TitleText}}
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F6" text="" @click="dlgAddDonor=false">
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
                                                                <h4 class="text-center heading-3">{{Subheadtext}}</h4>
                                                            </v-col>
                                                            <v-row :justify="justifyCenter">
                                                                <v-col md="6">
                                                                    <v-select dense="" :items="lstDonorItems" :item-text="i => i.Name" :item-value="i => i.ID" label="Select Donor" outlined="" v-model="selectedDonor" @change="GetDonorDetails(selectedDonor)">
                                                                        <template v-slot:selection="{ item, index }">
                                                                            <v-avatar size="27" light :color="`${getcolor(item.Name)}`" class="mr-2">
                                                                                <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            {{ item.Name }}
                                                                        </template>
                                                                        <template v-slot:item="{ item }">
                                                                            <v-avatar size="27" light :color="`${getcolor(item.Name)}`" class="mr-2">
                                                                                <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                                                                <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            {{ item.Name }}
                                                                        </template>
                                                                    </v-select>
                                                                    <v-select dense="" :items="lstAccountItems" :item-text="i => i.AccountName" :item-value="i => i.AccountId" label="Select Account" outlined="" v-model="selectedAccount">
                                                                    </v-select>

                                                                    <v-text-field label="Enter Amount"
                                                                                  type="number"
                                                                                  dense
                                                                                  v-model="Amount"
                                                                                  outlined
                                                                                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                  onKeyPress="if(this.value.length==10) return false;" min="0"></v-text-field>
                                                                </v-col>
                                                            </v-row>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <Secondary-Button id="AutoTest_F7" class="btn-122x36 mr-4" title="Confirm" @click.native="SaveDonarFund(),dlgAddDonor = false" v-bind:disabled="selectedDonor.length <=0 || Amount <=0"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_F8" class="btn-122x36" title="Cancel" @click.native="dlgAddDonor=false"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            
                            <v-dialog v-model="RemoveUserDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove User
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F9" text="" @click="RemoveUserDailog=false">
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
                                                                <h4 class="text-center heading-3">Are you sure you want to remove this user ?</h4>
                                                            </v-col>
                                                            <v-col>
                                                                <h4 class="text-center heading-3">
                                                                    <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                                    <v-avatar size="27" light :color="`${getcolor(DUserName)}`" class="mr-2">
                                                                        <img v-bind:src="userimage + DUserImage" alt="Donor name" v-if="DUserImage.length >0" />
                                                                        <span class="white--text" v-else>  {{DUserName == null ? "" :DUserName.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    {{DUserName}}
                                                                </h4>

                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <Secondary-Button id="AutoTest_F10" class="btn-122x36 mr-4" title="Remove" @click.native="RemoveBudgetUser(BudgetFundID)"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_F11" class="btn-122x36" title="Cancel" @click.native="RemoveUserDailog=false"></Default-Button-Outlined>
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
                        <!--Budget Plan and Payment terms-->
                        <v-col md="6">
                            <v-card class="custom-card-1">
                                <v-card-title style="height:60px">
                                    {{TitleTextBudget}}
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_F12" v-show="BudgetPlanSteps == 1 && ProjectHeadTableItem.length > 0" height="38" color="success-btn" dark class="elevation-0 mr-4"
                                           @click="dlgCreateBudgetPlan = true" v-bind:disabled="!screenAccess">
                                        Add Budget Plan
                                    </v-btn>
                                    <Success-Button id="AutoTest_F13" v-show="BudgetPlanSteps == 2 && PaymentTermsitem.length > 0" class="mr-4" title="Add" @click.native="CreatePaymentTermsDialog=true,fnFetchFinanceAndDonor()" v-bind:disabled="!screenAccess"></Success-Button>
                                    <router-link :to="{name:'ManageFinancePaymentTerms',query: { WsID: WorkspaceID, Access:RWAccess, InitID : InitiativeID,ProjID: ProjectID}}">
                                        <Primary-Button-Outlined class="mr-4" v-show="BudgetPlanSteps == 2 && PaymentTermsitem.length > 0" style="width:106px" title="View Terms"></Primary-Button-Outlined>
                                    </router-link>
                                    <h4 class="text-center black--text heading-3">
                                        <!-- Previous Button-->
                                        <v-btn id="AutoTest_F14"  style="height:34px; width:20px;border-radius:50%" x-small="" class="mr-2 elevation-1" color="#fff" @click="fnNextChartBudgetPlan('Minus')" v-bind:disabled="BudgetPlanSteps == 1">
                                            <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                                        </v-btn>
                                        <!-- Next Button-->
                                        <v-btn id="AutoTest_F15"  style="height:34px; width:20px;border-radius:50%" x-small="" class="ml-2 elevation-1" color="#fff" @click="fnNextChartBudgetPlan('Plus')" v-bind:disabled="BudgetPlanSteps == 2">
                                            <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                                        </v-btn>
                                    </h4>
                                </v-card-title>
                                <v-card-text style="height:300px" class="v-scrolling-div pa-0">
                                    <v-stepper v-model="BudgetPlanSteps" class="elevation-0">
                                        <v-stepper-items>
                                            <v-stepper-content step="1" class="pa-0">
                                                <!-- if data exists-->
                                                <v-row v-show="ProjectHeadTableItem.length >0">
                                                    <v-col md="6">
                                                        <v-row>
                                                            <v-col>
                                                                <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                                    <div style="width: 100%; height: 40px; position: absolute; top: 50%; left: 0; margin-top: -10px; line-height:19px; text-align: center;">

                                                                    </div>
                                                                    <canvas id="myBudgetPlan" style="width:175px; height:150px"></canvas>
                                                                </div>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col>
                                                                <h5 class="heading-3-dark text-center">
                                                                    Total budget<br />
                                                                                <span class="bold">{{ getSelectedCurrency.symbol }} {{budgetAmt}}</span>
                                                                </h5>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col md="6">
                                                        <h4 class="text-left mt-1 px-0 heading-3-dark">Legend Title</h4>
                                                        <v-row class="mt-1 mr-2 ml-1 px-0 v-scrolling-div" style="height:190px">
                                                            <v-col class="px-0 py-1">
                                                                <template v-for="(item, ExpenseTagID) in BudgetPlanHeadList">
                                                                    <h4 class="text-left heading-3-dark mb-2 mt-1">
                                                                        <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                                        <v-avatar size="15" class="team-avatar" :color="item.color" tile></v-avatar>

                                                                        {{item.Name}}:  {{ getSelectedCurrency.symbol }} {{item.TransactionAmount}}
                                                                    </h4>
                                                                </template>
                                                                <template>
                                                                    <h4 class="text-left heading-3 mb-2 mt-1">
                                                                        <v-avatar size="15"
                                                                                  class="team-avatar"
                                                                                  color="#E5E5E5"
                                                                                  tile></v-avatar>
                                                                        Unaccounted:
                                                                        {{ getSelectedCurrency.symbol }}
                                                                        {{ UnaccountedAmt }}
                                                                    </h4>
                                                                </template>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col cols="12" md="12" sm="12" xs="12" class="pa-0">
                                                        <v-card class="custom-card-1 elevation-0 pa-0">
                                                            <v-card-title  class="py-1 px-3">
                                                                <h4 class="text-left ml-4 font-weight-medium">
                                                                    Budget Head
                                                                </h4>
                                                            </v-card-title>
                                                            <v-card-text style="height:100%" class="pa-0">
                                                                <v-col class="pa-0">
                                                                    <tree-table class="table tree-table"
                                                                                :columns="ProjectHeadTableHeader"
                                                                                :table-data="ProjectHeadTableItem">
                                                                        <template #headerTemplate="headerProps"
                                                                                  class="pa-0 ma-0">
                                                                            <table style="width:100%;background:whitesmoke"
                                                                                   class="py-3">
                                                                                <tr>
                                                                                    <th>Name</th>
                                                                                    <th>Amount</th>
                                                                                    <th>Action</th>
                                                                                </tr>
                                                                            </table>
                                                                        </template>
                                                                        <!-- Rows with sub-activities -->
                                                                        <template #nodeTemplate="nodeProps">
                                                                            <div v-bind="nodeProps" class="ml-7">
                                                                                <div class="row">
                                                                                    <div v-if="nodeProps.depth > 0">
                                                                                        <div class="indentation" :style="{ width:(step*nodeProps.depth) + 'px' }">
                                                                                        </div>
                                                                                    </div>
                                                                                    <div id="AutoTest_F16" class="pt-3" @click="nodeProps.onToggle">
                                                                                        <v-btn id="AutoTest_F17" v-if="nodeProps.rowData.children.length>0" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen">
                                                                                            <div v-if="!nodeProps.rowData.isOpen">
                                                                                                <v-icon>mdi-chevron-right</v-icon>
                                                                                            </div>
                                                                                            <div v-if="nodeProps.rowData.isOpen">
                                                                                                <v-icon>mdi-chevron-down</v-icon>
                                                                                            </div>

                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <div class="pt-3" v-if="nodeProps.rowData.children.length == 0">
                                                                                        <v-btn id="AutoTest_F18" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen">
                                                                                            <div v-if="!nodeProps.rowData.isOpen" hidden>
                                                                                                <v-icon>mdi-chevron-right</v-icon>
                                                                                            </div>
                                                                                            <div v-if="nodeProps.rowData.isOpen" hidden>
                                                                                                <v-icon>mdi-chevron-down</v-icon>
                                                                                            </div>

                                                                                        </v-btn>
                                                                                    </div>
                                                                                    <div class="cell text-limit pt-5">
                                                                                        {{ nodeProps.rowData.Name }}
                                                                                    </div>
                                                                                    <div class="cell pt-5 ml-12 mr-12">
                                                                                        {{ getSelectedCurrency.symbol }} {{ nodeProps.rowData.Amount }}
                                                                                    </div>
                                                                                    <div class="cell pt-4  ml-16 pl-10">
                                                                                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                                                                            <template v-slot:activator="{ on }">
                                                                                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0" v-bind:disabled="nodeProps.rowData.Name=='Output'">
                                                                                                    <v-icon>mdi-dots-vertical</v-icon>
                                                                                                </v-btn>
                                                                                            </template>
                                                                                            <v-list dense="" color="white" light="" width="auto" class="action-button-list">
                                                                                                <v-list-item id="AutoTest_F19" link="" @click.stop="fnFetchBudgetHeader(nodeProps.rowData), dlgEditBudgetPlan = true">
                                                                                                    <v-list-item-icon>
                                                                                                        <v-icon>mdi-pencil-outline</v-icon>
                                                                                                    </v-list-item-icon>
                                                                                                    <v-list-item-content>Edit</v-list-item-content>
                                                                                                </v-list-item>
                                                                                                <v-list-item id="AutoTest_F20" link="" @click.stop="fnFetchBudgetHeader(nodeProps.rowData),dlgRemoveHeader = true">
                                                                                                    <v-list-item-icon>
                                                                                                        <v-icon>
                                                                                                            mdi-trash-can-outline
                                                                                                        </v-icon>
                                                                                                    </v-list-item-icon>
                                                                                                    <v-list-item-content>Remove</v-list-item-content>
                                                                                                </v-list-item>
                                                                                            </v-list>
                                                                                        </v-menu>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </template>

                                                                        <!-- Rows with no sub-activities -->
                                                                        <template #leafTemplate="leafProps">
                                                                            <div v-bind="leafProps" class="ml-11">
                                                                                <div class="row" style="max-height:55px;">
                                                                                    <div v-if="leafProps.depth > 0">
                                                                                        <div class="indentation" :style="{ width:(step*leafProps.depth)  + 'px' }">

                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="cell pt-5 text-limit">
                                                                                        {{ leafProps.rowData.Name }}
                                                                                    </div>
                                                                                    <div class="cell pt-5 ml-12">
                                                                                        {{ getSelectedCurrency.symbol }} {{ leafProps.rowData.Amount }}
                                                                                    </div>

                                                                                    <div class="cell pt-4 ml-12 pl-12">
                                                                                        <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">

                                                                                            <template v-slot:activator="{ on }">
                                                                                                <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                                                                                    <v-icon>mdi-dots-vertical</v-icon>
                                                                                                </v-btn>
                                                                                            </template>
                                                                                            <v-list dense="" color="white" light="" width="auto" class="action-button-list">
                                                                                                <v-list-item id="AutoTest_F21" link="" @click.stop="fnCheckForParentNode(leafProps)">
                                                                                                    <v-list-item-icon>
                                                                                                        <v-icon>mdi-pencil-outline</v-icon>
                                                                                                    </v-list-item-icon>
                                                                                                    <v-list-item-content>Edit</v-list-item-content>
                                                                                                </v-list-item>
                                                                                                <v-list-item id="AutoTest_F22" link="" @click.stop="fnRemoveForParentNode(leafProps)">
                                                                                                    <v-list-item-icon>
                                                                                                        <v-icon>
                                                                                                            mdi-trash-can-outline
                                                                                                        </v-icon>
                                                                                                    </v-list-item-icon>
                                                                                                    <v-list-item-content>Remove</v-list-item-content>
                                                                                                </v-list-item>
                                                                                            </v-list>
                                                                                        </v-menu>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </template>
                                                                    </tree-table>
                                                                </v-col>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <!-- if data not exists -->
                                                <!-- <v-row v-show="ProjectHeadTableItem.length ==0"> -->
                                                <v-row v-show="ProjectHeadTableItem.length ==0" justify="center">
                                                    <v-col class="text-center">
                                                        <!--<v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                        <h4 class="title text-center" color="#585858">No Budget Plan available</h4>-->
                                                        <p class="mt-3 pt-4">
                                                            <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                                        </p>

                                                        <p class="heading-4-dark font-weight-medium pt-1">No Budget Plan available</p>
                                                        <v-btn id="AutoTest_F23" height="38" color="success-btn" dark class="elevation-0 mr-4" @click="dlgCreateBudgetPlan = true" v-bind:disabled="!screenAccess">
                                                            Add Budget Plan
                                                        </v-btn>
                                                    </v-col>
                                                </v-row>
                                            </v-stepper-content>
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-card class="custom-card-1">
                                                    <v-card-text v-if="PaymentTermsitem.length==0">
                                                        <!--If there is no payment-->
                                                        <v-row justify="center">
                                                            <v-col class="text-center">
                                                                <p class="mt-2 pt-4">
                                                                    <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                                                </p>
                                                                <p class="heading-4-dark font-weight-medium pt-0">No payment terms available</p>
                                                                <Success-Button id="AutoTest_F24" class="" style="width:200px" title="Add Payment Term" @click.native="fnFetchFinanceAndDonor(),CreatePaymentTermsDialog=true" v-bind:disabled="!screenAccess"></Success-Button>

                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                    <!--If there is payments -->
                                                    <v-card-text v-if="PaymentTermsitem.length >0" class="v-scrolling-div" style="height:300px">
                                                        <v-row class="py-0">
                                                            <v-col class="py-0">
                                                                <v-timeline align-top dense>
                                                                    <div v-for="item in PaymentTermsitem" :key="item.PaymentId">

                                                                        <v-timeline-item small :color="item.Color" :icon="item.Icon">

                                                                            <v-row no-gutters class="py-0 my-0">
                                                                                <v-col md="9" class="py-0 my-0">
                                                                                    <p class="black--text" v-if="item.Logic == 'Date'">
                                                                                        Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}), Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                                                    </p>
                                                                                    <p class="black--text" v-else>
                                                                                        Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{item.Target}}) , Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                                                    </p>
                                                                                </v-col>
                                                                                <v-col md="3">
                                                                                    <v-menu close-on-content-click="" transition="scroll-y-transition" left="" offset-y="" v-bind:disabled="!screenAccess">
                                                                                        <template v-slot:activator="{ on }">
                                                                                            <v-btn min-width="27" height="29" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0 " style="float:right">
                                                                                                <v-icon>mdi-dots-vertical</v-icon>
                                                                                            </v-btn>
                                                                                        </template>
                                                                                        <v-list color="white" light width="auto" class="action-button-list" dense>
                                                                                            <!-- Edit-->
                                                                                            <v-list-item id="AutoTest_F25" link @click.stop="fnGetPaymentTermsByPaymentId(PaymentId=item.PaymentId),EditPaymentTermsDialog = true,fnFetchFinanceAndDonor()">
                                                                                                <v-list-item-icon><v-icon size="20">mdi-pencil-outline</v-icon></v-list-item-icon>
                                                                                                <v-list-item-content>Edit</v-list-item-content>
                                                                                            </v-list-item>
                                                                                            <v-list-item id="AutoTest_F26" link @click.stop="RemovePaymentTermDialog = true,PaymentId=item.PaymentId,PaymentTermName=item.PaymentTermName">
                                                                                                <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                                                                                <v-list-item-content>Remove</v-list-item-content>
                                                                                            </v-list-item>
                                                                                            <v-list-item id="AutoTest_F27" link @click.stop="dlgRequestPayment=true,PaymentId=item.PaymentId,GetProjectUsers('Donor')">
                                                                                                <v-list-item-icon> <v-icon>mdi-check-circle-outline</v-icon></v-list-item-icon>
                                                                                                <v-list-item-content>Request Payment</v-list-item-content>
                                                                                            </v-list-item>
                                                                                        </v-list>
                                                                                    </v-menu>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-timeline-item>
                                                                    </div>
                                                                </v-timeline>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                </v-card>
                                            </v-stepper-content>
                                        </v-stepper-items>
                                    </v-stepper>
                                </v-card-text>
                            </v-card>
                            <!--Add Budget Plan-->
                            <v-dialog v-model="dlgCreateBudgetPlan" persistent width="800">
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Add Budget Plan
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F28" text="" @click="CloseBudgetPlanDialog()">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <!--Step-1-->
                                    <v-card-text v-show="CreateBudgetAndPlanStepOne">
                                        <h4 class="text-center heading-3 mt-4">Please select the no. of payment terms to create a new budget plan.</h4>
                                        <v-row justify="center" class="pt-4">
                                            <v-col md="6">
                                                <v-text-field type="number" min="0" @keydown="FilterInput($event)" @paste.prevent label="Select no. of Budget Head" outlined="" dense="" v-model="NoOfBudgetPlans"></v-text-field>

                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <!--Step-2-->
                                    <v-form ref="budgetForm"
                                            v-model="valid"
                                            @submit.prevent="fnSaveBudgetPlan">
                                        <v-card-text v-show="CreateBudgetAndPlanStepTwo">

                                            <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new budget plan.</h4>
                                            <h4 class="text-center black--text heading-3 mt-4">
                                                <v-btn id="AutoTest_F29" fab="" small="" v-bind:disabled="BudgetAndPlanstepper==1" class="mr-4 elevation-1" color="#fff" @click="fnSetpperIncrementBudgetAndPlan('Minus')">
                                                    <v-icon size="30" color="primary">mdi-arrow-left</v-icon>
                                                </v-btn>
                                                Budget Head {{BudgetAndPlanstepper}} of {{NoOfBudgetPlans}}
                                                <v-btn id="AutoTest_F30" fab="" small="" class="ml-4 elevation-1" color="#fff" @click="fnSetpperIncrementBudgetAndPlan('Plus')" v-bind:disabled="BudgetAndPlanstepper==NoOfBudgetPlans || fnCheckBudgetMandatory()">
                                                    <v-icon size="30" color="primary">mdi-arrow-right</v-icon>
                                                </v-btn>
                                            </h4>
                                            <v-row justify="center" class="pt-4">
                                                <v-col md="8" style="height:250px" class="v-scrolling-div">
                                                    <v-row class="pa-0 ma-0">
                                                        <v-col class="pa-0 ma-0">
                                                            <v-text-field  class="mt-1 mb-2" label="Enter Budget Head" outlined="" dense="" v-model="BudgetHeadName" :rules="[rules.budgetHeadErrMesg]">
                                                                <template v-slot:append>
                                                                    <span v-if="BudgetHeadName.length == 0"
                                                                          class="error--text">*</span>
                                                                </template>
                                                            </v-text-field>
                                                        </v-col>
                                                    </v-row>

                                                    <v-row v-for="(HeaderItem, index) in BudgetItem.BudgetItemList" :key="index" class="pa-0 ma-0 py-0 my-0">
                                                        <v-col md="12" class=" d-inline-flex pa-0 ma-0 py-0 my-0">
                                                            <v-text-field  class="mr-1" label="Item Name" outlined="" dense="" v-model="HeaderItem.SubHeaderName" :rules="[rules.NameErrMesg]">
                                                                <template v-slot:append>
                                                                    <span class="error--text">*</span>
                                                                </template>
                                                            </v-text-field>
                                                            <v-text-field  class="mr-1" label="Amount" v-bind:prefix="getSelectedCurrency.symbol" type="Number" min="0" outlined="" dense="" @change="AddBudgetPlanAmt(HeaderItem.Amount)" v-model="HeaderItem.Amount"
                                                                          v-bind:placeholder="'0 out of '+ UnaccountedAmt" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                          onKeyPress="if(this.value.length==10) return false;" :rules="[rules.amountErrMesg,rules.max]">
                                                                <template v-slot:append>
                                                                    <span class="error--text">*</span>
                                                                </template>
                                                            </v-text-field>
                                                            <v-btn id="AutoTest_F31" v-show="index == BudgetItem.BudgetItemList.length -1" max-height="40" max-width="40" min-width="40" min-height="40" depressed="" fab="" light="" color="#EEEFF0"
                                                                   @click="AddHeaderItem(HeaderItem)" class="mr-1">
                                                                <v-icon size="25" light="" color="#707070">mdi-plus</v-icon>
                                                            </v-btn>
                                                            <v-btn id="AutoTest_F32" v-show=" BudgetItem.BudgetItemList.length - 1 && index !== BudgetItem.BudgetItemList.length -1" class="mr-1" max-height="40" max-width="40" min-width="40" min-height="40" depressed="" fab="" light="" color="#EEEFF0"
                                                                   @click="DeleteHeaderItem(HeaderItem)">
                                                                <v-icon size="25" light="" color="#707070">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                        <v-card-actions class="pb-4">
                                            <v-col>
                                                <Previous-Button-Outlined id="AutoTest_F33" v-show="CreateBudgetAndPlanStepTwo" class="float-right" @click.native="CreateBudgetAndPlanStepOne=true,CreateBudgetAndPlanStepTwo=false"></Previous-Button-Outlined>
                                                <Cancel-Button-Outlined-Sm id="AutoTest_F34" v-show="CreateBudgetAndPlanStepOne" class="float-right" @click.native="dlgCreateBudgetPlan=false"></Cancel-Button-Outlined-Sm>
                                                <Next-button-primary-light id="AutoTest_F35" v-show="CreateBudgetAndPlanStepOne" class="float-right mr-4" @click.native="CreateBudgetAndPlanStepTwo=true,CreateBudgetAndPlanStepOne=false" v-bind:disabled="NoOfBudgetPlans==0"></Next-button-primary-light>
                                                <!--<v-btn id="AutoTest_F36" v-show="CreateBudgetAndPlanStepTwo"
                                                       v-bind:disabled="BudgetAndPlanstepper!=NoOfBudgetPlans || fnCheckBudgetMandatory()|| BudgetItem.BudgetItemList[0].SubHeaderName.length ==0 || BudgetItem.BudgetItemList[0].Amount.length ==0 || BudgetItem.BudgetItemList[0].Amount ==0"
                                                       class="elevation-0 float-right mr-4 primary-btn-light"
                                                       dark="" height="36" width="122" @click="fnInvokeBudgetItemList(NoOfBudgetPlans),fnSaveBudgetPlan()">
                                                    Confirm
                                                </v-btn>-->
                                                <v-btn id="AutoTest_F36" v-show="CreateBudgetAndPlanStepTwo"
                                                       class="elevation-0 float-right mr-4 primary-btn-light"
                                                       dark="" height="36" width="122" type="submit" @click="fnSaveBudgetPlan()">
                                                    Confirm
                                                </v-btn>
                                            </v-col>
                                        </v-card-actions>
                                    </v-form>
                                </v-card>
                            </v-dialog>
                            

                            <!--Create Payment Terms Dialog-->
                            <v-dialog v-model="CreatePaymentTermsDialog" persistent width="800">
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Create Payment Terms
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F37" text="" @click="ClosepaymentDialog()">
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
                                    <v-form ref="paymentForm"
                                            v-model="valid"
                                            @submit.prevent="fnSavepaymentTerms">
                                        <v-card-text v-show="CreatePaymentTermsStepTwo">

                                            <h4 class="text-center heading-3 mt-4">Please enter the basic details of the new payment terms.</h4>
                                            <h4 class="text-center black--text heading-3 mt-4">
                                                <v-btn id="AutoTest_F38" fab="" small="" v-bind:disabled="stepper==1" class="mr-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Minus')">
                                                    <v-icon size="30" color="primary">mdi-arrow-left</v-icon>
                                                </v-btn>
                                                Payment Term {{stepper}} of {{NoOfPayments}}
                                                <v-btn id="AutoTest_F39" fab="" small="" class="ml-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Plus')" v-bind:disabled="stepper==NoOfPayments || !fnCheckMandatory()">
                                                    <v-icon size="30" color="primary" >mdi-arrow-right</v-icon>
                                                </v-btn>
                                            </h4>
                                            <v-row justify="center" class="pt-4">
                                                <v-col md="6" style="height:250px" class="v-scrolling-div">
                                                    <v-text-field class="mt-4" label="Enter Payment Term Name" outlined="" dense="" v-model="PaymentTermName" :rules="[rules.NameErrMesg]">
                                                        <template v-slot:append>
                                                            <span v-if="PaymentTermName.length==0" class="error--text">*</span>
                                                        </template>
                                                    </v-text-field>
                                                    <v-select dense="" v-model="CreatePaymentTermsLogicModel"
                                                              :items="CreatePaymentTermsLogic"
                                                              label="Select Logic"
                                                              :rules="[rules.LogicErrMesg]"
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
                                                    <v-text-field type="number" v-show="CreatePaymentTermsLogicModel==='Project Target'" label="Target(%)" outlined="" dense="" v-model="Target"
                                                                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                  onKeyPress="if(this.value.length==3) return false;" min="0">
                                                    </v-text-field>
                                                    <div class="py-0">
                                                        <v-text-field type="number" v-bind:prefix="getSelectedCurrency.symbol" label="Type amount" outlined="" dense="" v-model="amount" @change="AddPaymentTermAmt(amount)" :rules="[rules.amountErrMesg]"
                                                                      v-bind:placeholder="'0 out of '+ PaymentTermLeftAmt" required onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                      onKeyPress="if(this.value.length==10) return false;" min="0">
                                                            <template v-slot:append>
                                                                <span v-if="" class="error--text">*</span>
                                                            </template>
                                                        </v-text-field>
                                                        <span style="color:red;font-size:smaller;position:relative;top:-17px" v-show="amountErr == true" class="ml-3 py-0 my-0">The Maximum Amount should be {{PaymentTermLeftAmt}}</span>
                                                    </div>
                                                    <span class="small-text mr-1 py-0">
                                                        <v-icon size="12">mdi-account</v-icon> Notify People
                                                    </span>
                                                    <v-avatar size="27" class="team-avatar" v-for="(item,key) in selectedFinanceUser.slice(0,2)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                    <v-avatar v-if="selectedFinanceUser.length>=3" size="27" class="team-count">
                                                        +{{selectedFinanceUser.length-2}}
                                                    </v-avatar>
                                                    <v-menu absolute v-model="NotificationSelectPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                        <template v-slot:activator="{ on }">
                                                            <v-btn id="AutoTest_F40" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" @click="GetFinanceOrDonarUsers()">
                                                                <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                            </v-btn>
                                                        </template>
                                                        <!--Notification select pop up-->
                                                        <v-card>
                                                            <v-card-title class="heading-3">
                                                                Add people to Notify
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_F41" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp=false">
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
                                                                                    Finance Managers
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                            <v-divider></v-divider>
                                                                            <div style="height:150px;" class="v-scrolling-div">
                                                                                <template v-for="finUsers in FinanceUsersTemp">
                                                                                    <v-list-item>
                                                                                        <v-list-item-icon style="margin-top:-10px;">
                                                                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserList(finUsers.ID,'Finance', $event)" v-model="finUsers.IsSelected"></v-checkbox>
                                                                                        </v-list-item-icon>
                                                                                        <v-list-item-content>
                                                                                            <v-list-item-title>
                                                                                                <v-avatar size="27" light :color="`${getcolor(finUsers.Name)}`">
                                                                                                    <img v-bind:src="userimage + finUsers.ProfileImage" alt="User DP" v-if="finUsers.ProfileImage.length >0" />
                                                                                                    <span class="white--text" v-else>  {{finUsers.Name == null ? "" :finUsers.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                </v-avatar>
                                                                                                {{finUsers.Name}}
                                                                                            </v-list-item-title>
                                                                                        </v-list-item-content>
                                                                                    </v-list-item>
                                                                                    <v-divider></v-divider>
                                                                                </template>
                                                                            </div>
                                                                        </v-list>
                                                                    </v-col>
                                                                    <v-col class="py-0">
                                                                        <v-list dense="" class="py-0">
                                                                            <v-list-item>
                                                                                <v-list-item-content style="font-size:17px">
                                                                                    Donors
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                            <v-divider></v-divider>
                                                                            <div style="height:150px;" class="v-scrolling-div">
                                                                                <template v-for="DonarUser of DonarUserTemp">
                                                                                    <v-list-item>
                                                                                        <v-list-item-icon style="margin-top:-10px;">
                                                                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserList(DonarUser.ID,'Donar', $event)" v-model="DonarUser.IsSelected"></v-checkbox>
                                                                                        </v-list-item-icon>
                                                                                        <v-list-item-content>
                                                                                            <v-list-item-title>
                                                                                                <v-avatar size="27" light :color="`${getcolor(DonarUser.Name)}`">
                                                                                                    <img v-bind:src="userimage + DonarUser.ProfileImage" alt="User DP" v-if="DonarUser.ProfileImage.length >0" />
                                                                                                    <span class="white--text" v-else>  {{DonarUser.Name == null ? "" :DonarUser.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                </v-avatar>
                                                                                                {{DonarUser.Name}}
                                                                                            </v-list-item-title>
                                                                                        </v-list-item-content>
                                                                                    </v-list-item>
                                                                                    <v-divider></v-divider>
                                                                                </template>
                                                                            </div>
                                                                        </v-list>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row no-gutters="">
                                                                    <v-col md="12" align="center">
                                                                        <Success-Button id="AutoTest_F42" style="width:130px" class="my-1 center-block" title="Save" @click.native="NotificationSelectPopUp=false"></Success-Button>
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
                                                <Previous-Button-Outlined id="AutoTest_F43" v-show="CreatePaymentTermsStepTwo" class="float-right" @click.native="CreatePaymentTermsStepOne=true,CreatePaymentTermsStepTwo=false"></Previous-Button-Outlined>
                                                <Cancel-Button-Outlined-Sm id="AutoTest_F44" v-show="CreatePaymentTermsStepOne" class="float-right" @click.native="CreatePaymentTermsDialog=false"></Cancel-Button-Outlined-Sm>
                                                <Next-button-primary-light id="AutoTest_F45" v-show="CreatePaymentTermsStepOne" class="float-right mr-4" @click.native="CreatePaymentTermsStepTwo=true,CreatePaymentTermsStepOne=false" v-bind:disabled="NoOfPayments==0"></Next-button-primary-light>


                                                <v-btn id="AutoTest_F46" v-show="CreatePaymentTermsStepTwo" class="elevation-0 float-right mr-4 secondary"
                                                       dark="" height="36" width="122" style="background-color:#0964D9 !important; color:white !important" @click="fnSavepaymentTerms()">
                                                    Confirm
                                                </v-btn>
                                            </v-col>
                                        </v-card-actions>
                                    </v-form>
                                </v-card>
                            </v-dialog>

                            <!-- Edit Paymnet Term Dialog-->
                            <v-dialog v-model="EditPaymentTermsDialog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Edit Payment Terms
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F47" text @click="EditPaymentTermsDialog=false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <!--Step-1-->
                                    <v-form ref="editPaymentForm"
                                            v-model="valid"
                                            @submit.prevent="fnUpdatepaymentTerms">
                                            <v-card-text>
                                                <v-stepper v-model="CreatePaymentTermsStepper" class="elevation-0">
                                                    <v-stepper-items>
                                                        <v-stepper-content step="1">
                                                            <h4 class="text-center heading-3 mt-4">Edit the basic details of the payment terms.</h4>
                                                            <v-row justify="center" class="pt-4">
                                                                <v-col md="6" style="height:250px" class="v-scrolling-div">
                                                                    <v-text-field class="mt-4" label="Enter Payment Term Name" outlined dense v-model="PaymentInfo.PaymentTermName" :rules="[rules.NameErrMesg]"
                                                                                  required>
                                                                        <span v-if="PaymentInfo.PaymentTermName == 0"
                                                                              class="error--text">*</span>
                                                                    </v-text-field>
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
                                                                        <v-date-picker v-model="PaymentInfo.paymentdate" no-title @input="FrmDateMenu = false" dense hide-details format="DD-MM-YYYY"></v-date-picker>
                                                                    </v-menu>
                                                                    <v-text-field type="number" v-show="PaymentInfo.Logic=='Project Target'" label="Target(%)" v-model="PaymentInfo.Target" outlined dense
                                                                                  onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                  onKeyPress="if(this.value.length==3) return false;" min="0"></v-text-field>

                                                                    <v-text-field type="number" label="Type amount" v-model="PaymentInfo.Amount" :rules="[rules.amountErrMesg]"
                                                                                  required outlined dense onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                  onKeyPress="if(this.value.length==10) return false;" min="0">
                                                                        <template v-slot:append>
                                                                            <span v-if="" class="error--text">*</span>
                                                                        </template>
                                                                    </v-text-field>

                                                                    <span class="small-text mr-1">
                                                                        <v-icon size="12">mdi-account</v-icon> Notify People
                                                                    </span>

                                                                    <v-avatar size="27" class="team-avatar" v-for="(item,key) in PaymentInfo.users.slice(0,2)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    <v-avatar v-if="PaymentInfo.users.length>=3" size="27" class="team-count">
                                                                        +{{PaymentInfo.users.length-2}}
                                                                    </v-avatar>
                                                                    <v-menu v-model="NotificationSelectPopUp2" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                        <template v-slot:activator="{ on }">
                                                                            <Add-Button-Fab id="AutoTest_F48" @click.native="GetFinanceOrDonarUsersInEdit(),NotificationSelectPopUp2 = true"></Add-Button-Fab>
                                                                        </template>
                                                                        <!--Notification select pop up-->
                                                                        <v-card>
                                                                            <v-card-title class="heading-3">
                                                                                Add people to Notify
                                                                                <v-spacer></v-spacer>
                                                                                <v-btn id="AutoTest_F49" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp2=false">
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
                                                                                                    Finance Managers
                                                                                                </v-list-item-content>
                                                                                            </v-list-item>
                                                                                            <v-divider></v-divider>
                                                                                            <div style="height:150px;" class="v-scrolling-div">
                                                                                                <template v-for="finUsers in FinanceUsersTemp">
                                                                                                    <v-list-item>
                                                                                                        <v-list-item-icon style="margin-top:-10px;">
                                                                                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(finUsers.ID,'Finance', $event)" v-model="finUsers.IsSelected"></v-checkbox>
                                                                                                        </v-list-item-icon>
                                                                                                        <v-list-item-content>
                                                                                                            <v-list-item-title>
                                                                                                                <v-avatar size="27" light :color="`${getcolor(finUsers.Name)}`">
                                                                                                                    <img v-bind:src="userimage + finUsers.ProfileImage" alt="User DP" v-if="finUsers.ProfileImage.length >0" />
                                                                                                                    <span class="white--text" v-else>  {{finUsers.Name == null ? "" :finUsers.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                                </v-avatar>
                                                                                                                {{finUsers.Name}}
                                                                                                            </v-list-item-title>
                                                                                                        </v-list-item-content>
                                                                                                    </v-list-item>
                                                                                                    <v-divider></v-divider>
                                                                                                </template>
                                                                                            </div>
                                                                                        </v-list>
                                                                                    </v-col>
                                                                                    <v-col class="py-0">
                                                                                        <v-list dense="" class="py-0">
                                                                                            <v-list-item>
                                                                                                <v-list-item-content style="font-size:17px">
                                                                                                    Donors
                                                                                                </v-list-item-content>
                                                                                            </v-list-item>
                                                                                            <v-divider></v-divider>
                                                                                            <div style="height:150px;" class="v-scrolling-div">
                                                                                                <template v-for="DonarUser of DonarUserTemp">
                                                                                                    <v-list-item>
                                                                                                        <v-list-item-icon style="margin-top:-10px;">
                                                                                                            <v-checkbox hide-details="" color="#52B962" multiple-active="" @change="GetSelectedUserListInEdit(DonarUser.ID,'Donar', $event)" v-model="DonarUser.IsSelected"></v-checkbox>
                                                                                                        </v-list-item-icon>
                                                                                                        <v-list-item-content>
                                                                                                            <v-list-item-title>
                                                                                                                <v-avatar size="27" light :color="`${getcolor(DonarUser.Name)}`">
                                                                                                                    <img v-bind:src="userimage + DonarUser.ProfileImage" alt="User DP" v-if="DonarUser.ProfileImage.length >0" />
                                                                                                                    <span class="white--text" v-else>  {{DonarUser.Name == null ? "" :DonarUser.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                                </v-avatar>
                                                                                                                {{DonarUser.Name}}
                                                                                                            </v-list-item-title>
                                                                                                        </v-list-item-content>
                                                                                                    </v-list-item>
                                                                                                    <v-divider></v-divider>
                                                                                                </template>
                                                                                            </div>
                                                                                        </v-list>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-card-text>
                                                                            <v-divider></v-divider>
                                                                            <v-card-actions>
                                                                                <v-row no-gutters="">
                                                                                    <v-col md="12" align="center">
                                                                                        <Success-Button id="AutoTest_F50" style="width:130px" class="my-1 center-block" title="Save" @click.native="NotificationSelectPopUp2=false"></Success-Button>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-card-actions>
                                                                        </v-card>
                                                                        <!--__________________________-->
                                                                    </v-menu>
                                                                </v-col>

                                                            </v-row>
                                                        </v-stepper-content>
                                                    </v-stepper-items>
                                                </v-stepper>
                                            </v-card-text>
                                            <v-card-actions class="pb-4">
                                                <v-col>
                                                    <Cancel-Button-Outlined-Sm id="AutoTest_F51" class="float-right" @click.native="EditPaymentTermsDialog=false"></Cancel-Button-Outlined-Sm>
                                                    <v-btn id="AutoTest_F52" class="elevation-0 float-right mr-4 primary-btn-light" dark height="36" type="submit" width="122" @click="fnUpdatepaymentTerms()">
                                                        Confirm
                                                    </v-btn>
                                                </v-col>
                                            </v-card-actions>
                                    </v-form>
                                </v-card>
                            </v-dialog>
                            

                            <!-- Remove Payment Terms-->
                            <v-dialog v-model="RemovePaymentTermDialog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Payment Terms
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F53" text="" @click="RemovePaymentTermDialog=false">
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
                                                                <Secondary-Button id="AutoTest_F54" class="btn-122x36 mr-4" title="Remove" @click.native="DeletePaymentTerm(),RemovePaymentTermDialog = false"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_F55" class="btn-122x36" title="Cancel" @click.native="RemovePaymentTermDialog=false"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            

                            <v-dialog v-model="dlgRequestPayment" width="800" persistent>
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Request Payment
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F56" text="" @click="dlgRequestPayment = false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text>

                                        <h4 class="text-center heading-3 mt-4">Please enter the basic details for requesting payment.</h4>
                                        <v-row justify="center" class="mt-4">
                                            <v-col md="6" class="v-scrolling-div">
                                                <v-text-field outlined="" dense="" label="Amount" v-model="reqAmount">
                                                </v-text-field>
                                                <v-textarea rows="3" outlined dense label="Enter comment" type="text" v-model="Description">
                                                </v-textarea>

                                                <v-file-input label="Upload attachment" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""
                                                              v-model="fileInput" id="file"></v-file-input>


                                                <v-select dense="" :items="lstDonorItems" :item-text="i=>i.Name" :item-value="i => i.ID" label="Select Donor" outlined="" v-model="selectedDonor" @change="GetDonorDetails(selectedDonor)">
                                                    <template v-slot:selection="{ item, index }">
                                                        <v-avatar size="27" light :color="`${getcolor(item.Name)}`" class="mr-2">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                        {{ item.Name }}
                                                    </template>
                                                    <template v-slot:item="{ item }">
                                                        <v-avatar size="27" light :color="`${getcolor(item.Name)}`" class="mr-2">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="Donor name" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                        {{ item.Name }}
                                                    </template>                   
                                                </v-select>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-card-actions>
                                        <v-col md="12">
                                            <Outlined-Button-Dark id="AutoTest_F57" class="btn-122x36 float-right" title="Cancel" @click.native="ClearRequestPayment()"></Outlined-Button-Dark>
                                            <Primary-Button-Light id="AutoTest_F58" class="btn-122x36 float-right mr-4" title="Save" @click.native="SavePaymentRequest()"  v-bind:disabled="selectedDonor.length==0  || reqAmount ==''"></Primary-Button-Light>
                                        </v-col>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                            

                            <!-- Budget Plan Actions Starts here ----- Edit and Remove-->
                            <v-dialog v-model="dlgEditBudgetPlan" width="800" persistent>
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Edit Budget Plan
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F59" text="" @click="dlgEditBudgetPlan = false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text>
                                        <h4 class="text-center heading-3 mt-4">Please edit the basic details to edit budget plan</h4>
                                        <v-row justify="center" class="mt-4">
                                            <v-col md="6" class="v-scrolling-div">
                                                <v-text-field outlined="" dense="" label="Budget Header" v-model="EditBudgetPlanHeader">
                                                </v-text-field>

                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-card-actions>
                                        <v-col>
                                            <Primary-Button-Outlined id="AutoTest_F60" class="btn-122x36 float-right" title="Cancel" @click.native="dlgEditBudgetPlan=false"></Primary-Button-Outlined>
                                            <Success-Button-Md id="AutoTest_F61" class="btn-122x36 float-right mr-4" title="Save" @click.native="fnUpdateBudgetPlan()"></Success-Button-Md>
                                        </v-col>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                            

                            <!-- Edit Budget and Plan Sub Headers-->
                            <v-dialog v-model="dlgEditBudgetPlanSubHeader" width="800" persistent>
                                <v-card>
                                    <v-card-title primary-title="" class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-cash</v-icon>
                                        </v-avatar>
                                        Edit Item
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F62" text="" @click="dlgEditBudgetPlanSubHeader = false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text>
                                        <h4 class="text-center heading-3 mt-4">Please edit the basic details of the Item</h4>
                                        <v-row justify="center" class="mt-4">
                                            <v-col md="6" class="v-scrolling-div">
                                                <v-text-field outlined="" dense="" label="Name" v-model="EditBudgetPlanSubHeader">
                                                </v-text-field>
                                                <v-text-field outlined="" dense="" type="number" label="Amount" v-model="EditBudgetAmount"
                                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                              onKeyPress="if(this.value.length==10) return false;" min="0">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                    <v-card-actions>
                                        <v-col>
                                            <Primary-Button-Outlined id="AutoTest_F63" class="btn-122x36 float-right" title="Cancel" @click.native="dlgEditBudgetPlanSubHeader=false"></Primary-Button-Outlined>
                                            <Success-Button-Md id="AutoTest_F64" class="btn-122x36 float-right mr-4" title="Save" @click.native="fnUpdateBudgetPlanItem()"></Success-Button-Md>
                                        </v-col>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                            

                            <!-- Remove Sub Header Item-->
                            <v-dialog v-model="dlgRemoveSubItem" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Item
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F65" text="" @click="dlgRemoveSubItem=false">
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
                                                                <h4 class="text-center heading-3">Are you sure you want to remove this Item ?</h4>
                                                            </v-col>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-row justify="center">
                                                                    <h5 class="page-head mt-4">
                                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                                            <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                                        </v-avatar>
                                                                        {{EditBudgetPlanSubHeader}}
                                                                    </h5>
                                                                </v-row>

                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_F66" color="secondary elevation-0" class="mr-2" @click="RemoveBudgetPlanItems()">Remove</v-btn>
                                                                <v-btn id="AutoTest_F67" outlined color="grey" @click="dlgRemoveSubItem=false">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            

                            <!-- Remove Header -->
                            <v-dialog v-model="dlgRemoveHeader" width="800" persistent>
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Budget Plan
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F68" text="" @click="dlgRemoveHeader=false">
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
                                                                <h4 class="text-center heading-3">Are you sure you want to remove this Budget Plan ?</h4>
                                                            </v-col>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-row justify="center">
                                                                    <h5 class="page-head mt-4">
                                                                        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                                                            <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                                        </v-avatar>
                                                                        {{EditBudgetPlanHeader}}
                                                                    </h5>
                                                                </v-row>

                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <Secondary-Button id="AutoTest_F69" class="btn-122x36 mr-4" title="Remove" @click.native="RemoveBudgetPlan()"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_F70" class="btn-122x36" title="Cancel" @click.native="dlgRemoveHeader=false"></Default-Button-Outlined>
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
<script>
    import { mapGetters } from "vuex";
    import TreeTable from 'vue-tree-table-component'
    import CloseBtnFabSmall from '@/components/close-btn-fab-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import AddButtonFab from '@/components/add-button-fab.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'

    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import DefaultIconButtonOutlined from '@/components/default-icon-button-outlined.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import DoughnutChart from '@/components/DoughnutChart.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import objUtils from '../../utils.js'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objVenture;
    async function importVenturescript() {
        return Promise.all([
            import("../../BL/Venture.js").then(mod => {
                objVenture = new mod.Venture();
            })
        ]);
    };
    var objProject;
    async function importFinancescript() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then(mod => {
                objProject = new mod.ManageProjectFinance();
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

    export default
        {
        data()
        {
                return {
                    valid: true,
                    rules: {
                        required: (value) => !!value || "this field is required.",
                        max: v => v <= this.UnaccountedAmt || `The Maximum Amount should be ${this.UnaccountedAmt}`,
                        NameErrMesg: (value) => !!value || "Name is required.",
                        LogicErrMesg: (value) => !!value || "Logic selection is required.",
                        budgetHeadErrMesg: (value) => !!value || "Budget Head is required.",
                        amountErrMesg: (value) => !!value || "Amount is required.",
                    },
                fixedColor:["#399CFF", "#00C2B0", "#F1C100", "#00B448", "#2C4258", "#8307CB", "#F98B00", "#D10000", "#E6217B", "#6F00B0", "#464646", "#395FFF", "#038E81", "#C99506", "#76B400", "#2C584A", "#521CA8", "#D5D5D5", "#FF6200", "#88320C", "#CE72B9", "#A00000", "#41AFFF", "#00E209", "#D008A9", "#FFD446"],
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                ProjectID: 0,
                InitiativeID: 0,
                dlgAddDonor: false,
                Amount: 0,
                lstDonorItems: [],
                lstAccountItems: [],

                selectedDonor: '',
                selectedAccount: '',
                selectedAccountName :'',
                selectedDonorName :'',
                TitleText: 'Add Donor',
                AddSubHeadText: 'Please enter the basic details of donor',
                EditSubHeadText:'Please edit the basic details of donor',
                Subheadtext: '',
                BudgetAndFinanceSteps: 1,
                //payment listing parameters
                LedgerBookTableheaders: [
                    { text: "Title", value: "PaymentActivityName" },
                    { text: "Trans ID", value: "TransactionId" },
                    { text: "Trans Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "Documents", value: "TransactionDocName" },

                ],
                PaymentList: [],
                BudgetPlanSteps: 1,
                CreatePaymentTermsStepOne: true,
                CreatePaymentTermsStepTwo: false,

                BudgetAndFundHeader: "Budget & Funds",
                //Top bar filter parameters
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                dlgCreateBudgetPlan: false,
                NoOfPayments:0,
                stepper:1,
                CreatePaymentTermsLogicModel: '',
                AddPayment: {
                    ExpenseTags : [],
                },

                BudgetHeadName: '',
                ProjectHeadTableHeader: [
                    { label: 'Name', id: 'Name', props: 'Name' },
                    { label: 'Amount', id: 'Amount', props: 'Amount' },
                    { label: 'Action', id: 'action', props: 'action' },
                ],
                ProjectHeadTableItem: [],
                UnaccountedAmt: 0,
                UnaccountedAmtCpy:0,
                CreateBudgetAndPlanStepOne:true,
                CreateBudgetAndPlanStepTwo:false,
                BudgetAndPlanstepper:1,
                NoOfBudgetPlans: 0,
                TitleTextOne: "Budget Plan",
                TitleTextTwo :"Payment Terms",
                TitleTextBudget: "Budget Plan",
                CreatePaymentTermsDialog: false,
                EditPaymentTermsDialog: false,
                RemovePaymentTermDialog: false,
                dlgRequestPayment:false,
                PaymentTermName:'',
                CreatePaymentTermsLogic: ['Date', 'Project Target'],
                PaymentDateMenu:false,
                ProjectEndDate: new Date().toISOString().substr(0, 10),
                ProjectStartDate: new Date().toISOString().substr(0, 10),
                paymentdate : new Date().toISOString().substr(0, 10),
                Target:0,
                amount: null,
                PaymentTermLeftAmt:0,
                selectedFinanceUser:[],
                NotificationSelectPopUp:false,
                FinanceUsersTemp:[],
                DonarUserTemp:[],

                PaymentTerm: {
                    PaymentTermList: [],
                },
                PaymentTermsitem: [],
                ExpenseList: [],
                ExpenseTagList: [],
                PaymentfilterList: [],
                ExpenseTag: {
                    TagName: "All", ExpenseTagID: 0
                },
                BudgetPlanHeadList:[],
                budgetAmt: 0,
                GroupID: 0,
                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],
                GroupID: 0,
                DonarFundList: [],
                DonarUtilizedFund: 0,
                UnUtilizedFund: 0,
                DonarNotFundedFund: 0,
                DonarBudgetFund:0,
                //TotalFundedAmount:0,
                BudgetFundID: 0,
                RemoveUserDailog: false,
                DUserID: 0,
                DUserName: "",
                DUserImage: "",
                lstFinanceUsers: [],
                lstDonors: [],
                projectInfo: [],
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
                BudgetItem: {
                    BudgetItemList: [{ SubHeaderName: '', Amount: '' }],
                },
                BudgetPlan: {
                    BudgetPlanList: [],
                },

                totermsAmt: 0,
                toBudgetAmt: 0,
                TotalBudgetPlanAmt: 0,
                fileInput: null,
                Description: '',
                reqAmount: 0,
                NotificationSelectPopUp2: false,
                FrmDateMenu: false,
                CreatePaymentTermsStepper: 1,
                step: 50,

                // Edit Budget Plan
                dlgEditBudgetPlan : false,
                EditBudgetPlanHeader: '',
                EditBudgetId:0,

                dlgEditBudgetPlanSubHeader: false,
                EditBudgetAmount: 0,
                EditBudgetPlanSubHeader: '',
                EditBudgetItemID: 0,

                dlgRemoveSubItem: false,
                dlgRemoveHeader: false,

                editDonor:0,
                PreLoader: false,
                BudgetHeadList: [],
                BudgetHead: {
                    BudgetHeadName: "All", BudgetHeadID: 0
                },
                screenAccess: 0,
                RWAccess: 0,
                userimage: "",
                amountErr: false,
                dcExistingPymtTermSum : 0,
      }

  },

            mounted: async function ()
            {
                this.WorkspaceID = this.$route.query.WsID;
                this.RWAccess = this.$route.query.Access;
                this.userimage = window.SERVER_URL + "/Upload";
                this.screenAccess = this.$route.query.Access == 3 ? true : false;
                this.ProjectID = this.$route.query.ProjID;
                this.InitiativeID = this.$route.query.InitID;
                await importscript();
                await importVenturescript();
                await importFinancescript();
                await importMyFinancescript();
                this.PreLoader = true;
                await this.fnGetProjectDetailsByID();
                //await this.GetTotalFundById();
                await this.GetProjectBudgetDetails();
                await this.FetchBudgetPlan();
                this.PreLoader = false;
        },
        components: {
            'Close-Btn-Fab-Small': CloseBtnFabSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Add-Button-Fab': AddButtonFab,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Default-Icon-Button-Outlined': DefaultIconButtonOutlined,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Primary-Button-Light': PrimaryButtonLight,
            'Secondary-Button': SecondaryButton,
            'Pre-Loader': PreLoader,
            TreeTable,
            },

            methods: {

                /// <summary>
                ///  getcolor :- Function used to get color of users
                /// </summary>
                /// <param name="name"></param>
                /// <returns> </returns>
                getcolor(name) {
                    return objUtils.common.getcolor(name);
                },

                /// <summary>
                ///  generator :- Function used to select random colors for graphical representation
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                generator: function () {
                    this.mycolor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);

                },

                /// <summary>
                ///  formatDate :- Function used to formatting a date
                /// </summary>
                /// <param name="date"></param>
                /// <returns> </returns>
                formatDate(date) {
                    return objUtils.common.formatDate(date);
                },

                /// <summary>
                ///  fnNextChart :- Function used to move to the next step and show details of Budget & Fund/Fund Utilization/Planned & Actual when click on next/previous button
                /// </summary>
                /// <param name="type"></param>
                /// <returns> </returns>
                fnNextChart(type) {
                    if (type == 'Plus') {
                        this.BudgetAndFinanceSteps = this.BudgetAndFinanceSteps + 1;
                        if (this.BudgetAndFinanceSteps == 2) {
                            this.BudgetAndFundHeader = "Fund Utilization";
                            this.GetPaymentBudgetHead();
                            this.fnGetLedgerBookExpenseTagsById();
                            this.fnGetPaymentList();
                            this.GetBudgetHeadDetails();
                        } else if (this.BudgetAndFinanceSteps == 3) {
                            this.BudgetAndFundHeader = "Planned & Actual"
                            this.fillBarchartData();
                        }
                    }
                    else if (type == 'Minus') {
                        this.BudgetAndFinanceSteps = this.BudgetAndFinanceSteps - 1;
                        if (this.BudgetAndFinanceSteps == 2) {
                            this.BudgetAndFundHeader = "Fund Utilization";
                            this.GetPaymentBudgetHead();
                        } else if (this.BudgetAndFinanceSteps == 1) {
                            this.BudgetAndFundHeader = "Budget & Fund"
                        }
                    }
                },

                /// <summary>
                ///  fnNextChartBudgetPlan :- Function used to move to the next step and show details of Budget Plan/Payment Terms when click on next/previous button
                /// </summary>
                /// <param name="type"></param>
                /// <returns> </returns>
                fnNextChartBudgetPlan(type) {
                    if (type == 'Plus') {
                        this.BudgetPlanSteps = this.BudgetPlanSteps + 1;
                        this.TitleTextBudget = this.TitleTextTwo;
                        this.fnGetPaymentTermsByprjID();
                    }
                    else if (type == 'Minus') {
                        this.BudgetPlanSteps = this.BudgetPlanSteps - 1;
                        this.TitleTextBudget = this.TitleTextOne

                    }
                },

                /// <summary>
                ///  fnSetpperIncrement :- Function used to increment the step in Payment Terms creation based on Mode (Plus/Minus)
                /// </summary>
                /// <param name="Mode"></param>
                /// <returns> </returns>
                async  fnSetpperIncrement(Mode) {
                    if (Mode == 'Plus') {
                        if (this.$refs.paymentForm.validate() === true) {
                            this.stepper = this.stepper + 1;
                            this.fnInvokePaymentTermList(this.stepper)
                            this.$refs.paymentForm.resetValidation()
                        }
                    } else if (Mode == 'Minus') {
                        this.stepper = this.stepper - 1;
                        this.fnInvokePrevPaymentTermList(this.stepper)
                    }
                },

                /// <summary>
                ///  fnInvokePaymentTermList :- Function used to show Payment Term data if data present otherwise shows as empty when click on the right arrow based on index
                /// </summary>
                /// <param name="index"></param>
                /// <returns> </returns>
                async fnInvokePaymentTermList(index) {
                    //this.PaymentTermLeftAmt = this.PaymentTermLeftAmt - this.amount
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

                /// <summary>
                ///  fnInvokePrevPaymentTermList :- Function used to show Prevoius Payment Term data when click on the left arrow based on index
                /// </summary>
                /// <param name="index"></param>
                /// <returns> </returns>
                async fnInvokePrevPaymentTermList(index) {
                    if (index + 1 == this.NoOfPayments) {
                        if (this.PaymentTermName != "") {
                            this.PaymentTerm.PaymentTermList.push({
                                Index: this.NoOfPayments,
                                PaymentTermName: this.PaymentTermName,
                                Logic: this.CreatePaymentTermsLogicModel,
                                paymentdate: this.paymentdate,
                                Target: this.Target,
                                amount: this.amount,
                                UserList: this.selectedFinanceUser
                            });
                            this.FinanceUsersTemp = [];
                            this.DonarUserTemp = [];
                        }
                    }
                    if (index > 0) {
                        var temp = this.PaymentTerm.PaymentTermList.filter((x) => x.Index == index);
                        if (temp.length > 0) {
                            this.PaymentTermName = temp[0].PaymentTermName;
                            this.CreatePaymentTermsLogicModel = temp[0].Logic;
                            this.paymentdate = temp[0].paymentdate;
                            this.Target = temp[0].Target;
                            this.amount = temp[0].amount;
                            this.selectedFinanceUser = temp[0].UserList;
                            for (var i = 0; i < this.selectedFinanceUser.length; i++) {
                                if (this.selectedFinanceUser[i].UserType == "F")
                                    this.FinanceUsersTemp.push(this.selectedFinanceUser[i]);
                                else if (this.selectedFinanceUser[i].UserType == "D")
                                    this.DonarUserTemp.push(this.selectedFinanceUser[i]);
                            }
                        }
                    }
                },

                /// <summary>
                ///  GetProjectBudgetDetails :- Function used to fetch Project Budget Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async GetProjectBudgetDetails() {

                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objProject.GetProjectBudgetDetails(data);

                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.data.length > 0) {
                            this.budgetAmt = result.data[0].Budget;
                            this.GroupID = result.data[0].GroupId;
                            this.lstBudgetFinUserInfo = result.data.filter(x => x.UserType == 'F');
                            this.lstBudgetDonorUserInfo = result.data.filter(x => x.UserType == 'D');
                            this.fnFetchDonarFund(this.lstBudgetDonorUserInfo);
                        }

                    }
                },

                /// <summary>
                ///  fnFetchDonarFund :- Function used to fetch Donar Fund details
                /// </summary>
                /// <param name="listitem"></param>
                /// <returns> </returns>
                async fnFetchDonarFund(listitem) {

                    this.DonarFundList = [];
                    this.DonarNotFundedFund = this.budgetAmt
                    this.DonarBudgetFund = 0;
                    if (listitem.length > 0) {

                        var i = 0;
                        for (i = 0; i <= listitem.length - 1; i++) {
                            this.generator();
                            this.DonarFundList.push({ UserName: listitem[i].UserName, ProfileImage: listitem[i].ProfileImage, color: this.fixedColor[i] ||this.mycolor, DonorBudgetAmount: listitem[i].DonorBudgetAmount, UserID: listitem[i].UserID, AccountId: listitem[i].AccountId, AccountName: listitem[i].AccountName, BudgetFundID: listitem[i].BudgetFundID});
                            this.DonarBudgetFund = this.DonarBudgetFund + listitem[i].DonorBudgetAmount;
                        }
                        this.DonarNotFundedFund = this.budgetAmt - this.DonarBudgetFund;
                    }
                    this.fnFillBudgetAndFundChartData();
                },

                /// <summary>
                ///  fnFillBudgetAndFundChartData :- Function used to fill Budget & Fund chart data
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnFillBudgetAndFundChartData() {
                    var labelsArray = ["DonarNotFundedFund"];
                    var amountArray = [this.DonarNotFundedFund];
                    var labels = ["#E5E5E5"];
                    for (var i = 0; i < this.DonarFundList.length; i++) {
                        labelsArray.push(this.DonarFundList[i] ? this.DonarFundList[i].UserName : '');
                        amountArray.push(this.DonarFundList[i] ? this.DonarFundList[i].DonorBudgetAmount : '');
                        labels.push(this.DonarFundList[i] ? this.DonarFundList[i].color : '');

                    }
                    var ctx = document.getElementById("myBudgetAndFundChart")
                    var stateOverviewGraph2 = new Chart(ctx, {
                        type: 'doughnut',
                        data: {
                            labels: labelsArray,
                            datasets: [{
                                data: amountArray,
                                backgroundColor: labels
                            }],
                        },
                        options: {
                            legend: {
                                display: false,
                            },
                        }
                    });
                },

                /// <summary>
                ///  fillBarchartData :- Function used to fill Planned and Actual bar chart data
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fillBarchartData() {
                    var labelsArray = [];
                    var PlannedlabelsArray = [];
                    var PlannedamountArray = [];
                    var Plannedlabels = [];

                    var ActuallabelsArray = [];
                    var ActualamountArray = [];
                    var Actuallabels = [];

                    PlannedlabelsArray = [];
                    PlannedamountArray = [];
                    Plannedlabels = [];

                    ActuallabelsArray = [];
                    ActualamountArray = [];
                    Actuallabels = [];

                    for (var ExpId = 0; ExpId <= this.ExpenseList.length - 1; ExpId++) {
                        for (var PlanID = 0; PlanID <= this.BudgetPlanHeadList.length - 1; PlanID++) {
                            if (this.BudgetPlanHeadList[PlanID].Name == this.ExpenseList[ExpId].BudgetHeadName) {

                                // -- Planned Graph details
                                PlannedlabelsArray.push(this.BudgetPlanHeadList[PlanID] ? this.BudgetPlanHeadList[PlanID].Name : '');
                                PlannedamountArray.push(this.BudgetPlanHeadList[PlanID] ? this.BudgetPlanHeadList[PlanID].TransactionAmount : '');
                                Plannedlabels.push('#0202AA');

                                // -- Actual Grap Details
                                ActuallabelsArray.push(this.ExpenseList[ExpId] ? this.ExpenseList[ExpId].BudgetHeadName : '');
                                ActualamountArray.push(this.ExpenseList[ExpId] ? parseFloat(this.ExpenseList[ExpId].HeadAmount) : '');
                                Actuallabels.push('#89D8B0');
                                break;
                            }

                        }
                    }

                    if (ActuallabelsArray.length > PlannedlabelsArray.length) {
                        labelsArray = PlannedlabelsArray;

                    }
                    else {
                        labelsArray = ActuallabelsArray;

                    }

                    var ctx = document.getElementById("myBarChart");
                    var myChart = new Chart(ctx, {
                        type: 'horizontalBar',
                        data: {
                            datasets: [
                                {
                                    label: ['Planned'],
                                    data: PlannedamountArray,
                                    backgroundColor: Plannedlabels,
                                    barPercentage: 0.5,
                                    barThickness: 10,
                                    maxBarThickness: 15,
                                    minBarLength: 2,
                                },
                                {
                                    label: ['Actual'],
                                    data: ActualamountArray,
                                    backgroundColor: Actuallabels,
                                    barPercentage: 0.5,
                                    barThickness: 10,
                                    maxBarThickness: 15,
                                    minBarLength: 2,
                                }
                            ],

                            labels: labelsArray,

                        },
                        options: {
                            scales: {
                                xAxes: [{
                                    ticks: {
                                        beginAtZero: true
                                    }
                                }]
                            }
                        }
                    });
                },

                /// <summary>
                ///  FilterInput :- Function used to filter input based on event
                /// </summary>
                /// <param name="event"></param>
                /// <returns> </returns>
                async FilterInput(event) {
                    var isNotWanted;
                    var keyCode = ('which' in event) ? event.which : event.keyCode;

                    isNotWanted = (keyCode == 69 || keyCode == 101 || keyCode == 109 || keyCode == 189);
                    if (isNotWanted == true) {
                        event.preventDefault();
                    }
                },
                //====================Payment Terms================================//
                /// <summary>
                ///  GetSelectedUserListInEdit :- Function used to fetch Selected User List in Edit Payment Terms Section
                /// </summary>
                /// <param name="userid"></param>
                /// <param name="type"></param>
                /// <param name="event"></param>
                /// <returns> </returns>
                async GetSelectedUserListInEdit(userid, type, event) {
                    var temp;
                    if (event == true) {
                        if (type == 'Finance') {
                            temp = this.PaymentInfo.users.filter(function (x) {
                                return x.ID == userid;
                            });
                            if (temp.length == 0) {
                               var list = this.lstFinanceUsers.filter((x => x.ID == userid));
                               this.PaymentInfo.users.push(list[0]);
                            }
                        } else if (type == 'Donar') {
                            temp = this.PaymentInfo.users.filter(function (x) {
                                return x.ID == userid;
                            });
                            if (temp.length == 0) {
                                var list = this.lstDonors.filter((x => x.ID == userid));
                                this.PaymentInfo.users.push(list[0]);
                            }
                        }
                    }
                    else {
                        var temp = this.PaymentInfo.users.filter(function (x) {
                            return x.ID == userid;
                        });
                        if (temp.length > 0) {
                            this.PaymentInfo.users.splice(this.PaymentInfo.users.indexOf(temp), 1);
                            this.PaymentInfo.users = [...this.PaymentInfo.users];
                        }
                    }
                },

                /// <summary>
                ///  fnAddToTempUserList :- Function used to add Finance/Donar Users to Temp User List
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                fnAddToTempUserList() {
                    this.FinanceUsersTemp=[];
                    this.DonarUserTemp=[];
                    if (this.lstFinanceUsers.length > 0) {
                        var item;
                        for (item = 0; item <= this.lstFinanceUsers.length - 1; item++) {
                            this.FinanceUsersTemp.push({ ID: this.lstFinanceUsers[item].ID, Name: this.lstFinanceUsers[item].Name, ProfileImage: this.lstFinanceUsers[item].ProfileImage, IsSelected: false })
                        }
                    }

                    if (this.lstDonors.length > 0) {
                        var item;
                        for (item = 0; item <= this.lstDonors.length - 1; item++) {
                            this.DonarUserTemp.push({ ID: this.lstDonors[item].ID, Name: this.lstDonors[item].Name, ProfileImage: this.lstDonors[item].ProfileImage, IsSelected: false })
                        }
                    }
                },

                /// <summary>
                ///  GetFinanceOrDonarUsersInEdit :- Function used to fetch Finance/Donar Users in Edit Payment Terms section
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async GetFinanceOrDonarUsersInEdit() {
                    this.fnAddToTempUserList();
                    this.NotificationSelectPopUp2 = true;
                    if (this.PaymentInfo.users.length > 0) {
                        if (this.PaymentInfo.users.length > 0) {
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

                            for (id = 0; id <= this.PaymentInfo.users.length - 1; id++) {
                                for (finId = 0; finId <= this.FinanceUsersTemp.length - 1; finId++) {
                                    if (this.PaymentInfo.users[id].ID == this.FinanceUsersTemp[finId].ID) {
                                        this.FinanceUsersTemp[finId].IsSelected = true;
                                    } else {
                                        //this.FinanceUsersTemp[finId].IsSelected = false;
                                    }
                                }

                                for (DonId = 0; DonId <= this.DonarUserTemp.length - 1; DonId++) {
                                    if (this.PaymentInfo.users[id].ID == this.DonarUserTemp[DonId].ID) {
                                        this.DonarUserTemp[DonId].IsSelected = true;
                                    } else {
                                        // this.DonarUserTemp[DonId].IsSelected = false;
                                    }
                                }
                            }
                        }
                    } else {
                        this.FinanceUsersTemp=[];
                        this.DonarUserTemp=[];
                        if (this.lstFinanceUsers.length > 0) {
                            var item;
                            for (item = 0; item <= this.lstFinanceUsers.length - 1; item++) {
                                this.FinanceUsersTemp.push({ ID: this.lstFinanceUsers[item].ID, Name: this.lstFinanceUsers[item].Name, ProfileImage: this.lstFinanceUsers[item].ProfileImage, IsSelected: false })
                            }
                        }

                        if (this.lstDonors.length > 0) {
                            var item
                            for (item = 0; item <= this.lstDonors.length - 1; item++) {
                                this.DonarUserTemp.push({ ID: this.lstDonors[item].ID, Name: this.lstDonors[item].Name, ProfileImage: this.lstDonors[item].ProfileImage, IsSelected: false })
                            }
                        }
                    }
                },

                /// <summary>
                ///  fnSavepaymentTerms :- Function used to Add/Save New Payment Terms
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnSavepaymentTerms() {
                    if (this.$refs.paymentForm.validate() === true && this.fnCheckMandatoryBeforeSave() === true) {
                        this.CreatePaymentTermsDialog = false,
                            this.fnInvokePaymentTermList(this.NoOfPayments);
                        var data = new FormData();
                        data.append("NoOfPaymentTerms", this.NoOfPayments);
                        data.append("PaymentTermList", JSON.stringify(this.PaymentTerm.PaymentTermList));
                        data.append("ProjectId", this.ProjectID);
                        var result = await objManageProject.SavepaymentTerms(data);
                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        } else
                            if (result.status == "success") {
                                this.Msg = "Payment term created successfully";
                                this.successSnackbarMsg = true;
                                this.ClosepaymentDialog();
                                this.$refs.paymentForm.resetValidation();
                                this.fnGetPaymentTermsByprjID();
                            }
                            else {
                                this.Msg = "Failed";
                                this.errorSnackbarMsg = true;
                            }
                    }
                },

                /// <summary>
                ///  fnCheckMandatory :- Function used to check any mandatory field is showing blank or null before moving to add another Payment Term
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                fnCheckMandatory() {
                    if (this.PaymentTermName == "") {
                        return false;
                    }
                    if (this.amount == "" || this.amount == "0") {
                        return false;
                    }
                    if (this.CreatePaymentTermsLogicModel == "Date") {
                        if (this.paymentdate == "") {
                            return false;
                        }
                    } else if (this.CreatePaymentTermsLogicModel == "Project Target") {
                        if (this.Target == "") {
                            return false;
                        }
                    }
                    if (this.amountErr == true) {
                        return false
                    }
                    return true;
                },

                /// <summary>
                ///  fnCheckMandatoryBeforeSave :- Function used to check any mandatory field is showing blank or null before Save Payment Term
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                fnCheckMandatoryBeforeSave() {
                    if (this.PaymentTermName == "") {
                        return false;
                    }
                    if (this.amount == "" || this.amount == "0") {
                        return false;
                    }
                    if (this.CreatePaymentTermsLogicModel == "Date") {
                        if (this.paymentdate == "") {
                            return false;
                        }
                    } else if (this.CreatePaymentTermsLogicModel == "Project Target") {
                        if (this.Target == "") {
                            return false;
                        }
                    }
                    if (this.amountErr == true) {
                        return false
                    }
                    // to validate the varibale in the last index of an array
                    var length = this.PaymentTerm.PaymentTermList.length;
                    if (length == 0) {
                        if (this.stepper < this.NoOfPayments) {
                            return false;
                        }
                    }
                    return true;
                },

                /// <summary>
                ///  fnUpdatepaymentTerms :- Function used to Update Payment Terms
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnUpdatepaymentTerms() {
                    if (this.$refs.editPaymentForm.validate() === true) {
                        this.EditPaymentTermsDialog = false;
                        var data = new FormData();
                        data.append("PaymentId", this.PaymentId);
                        data.append("PaymentTermName", this.PaymentInfo.PaymentTermName);
                        data.append("Logic", this.PaymentInfo.Logic);
                        data.append("paymentdate", this.PaymentInfo.paymentdate);
                        data.append("Target", this.PaymentInfo.Target);
                        data.append("Amount", this.PaymentInfo.Amount);
                        data.append("users", JSON.stringify(this.PaymentInfo.users));
                        data.append("ProjectId", this.ProjectID);
                        var result = await objManageProject.UpdatepaymentTerms(data);
                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        }
                        else {
                            if (result.status = "Success") {
                                this.Msg = "Payment term successfully updated";
                                this.successSnackbarMsg = true;
                                this.fnGetPaymentTermsByprjID();
                                this.EditPaymentTermsDialog = false;
                                this.PaymentInfo.users = [];
                                this.selectedFinanceUser = [];
                                this.$refs.editPaymentForm.resetValidation();
                            }
                        }
                    }
                },

                /// <summary>
                ///  DeletePaymentTerm :- Function used to Delete Payment Term
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async DeletePaymentTerm() {
                    var data = new FormData();
                    data.append("PaymentId", JSON.stringify(this.PaymentId));
                    data.append("Type", 2);
                    var result = await objManageProject.DeletePaymentTerm(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.status = "Success") {
                            this.Msg = `Payment term ${this.PaymentTermName} removed successfully`;
                            this.successSnackbarMsg = true;
                        } else {
                            this.Msg = "Failed in removing Payment term ...";
                            this.errorSnackbarMsg = true;
                        }
                    }
                    this.fnGetPaymentTermsByprjID();
                    this.PaymentId = 0;
                },

                /// <summary>
                ///  ClosepaymentDialog :- Function used to clear variables when click on close button in Payment Terms Dialog
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                ClosepaymentDialog() {
                    this.NoOfPayments = "";
                    this.PaymentTermName = "";
                    this.Target = "";
                    this.amount = "";
                    this.amountErr = false;
                    this.paymentdate = new Date().toISOString().substr(0, 10);
                    this.CreatePaymentTermsLogicModel = "";
                    this.CreatePaymentTermsDialog = false;
                    this.CreatePaymentTermsStepOne = true;
                    this.CreatePaymentTermsStepTwo = false;
                    this.PaymentTerm.PaymentTermList = [];
                    this.stepper = 1;
                    this.selectedFinanceUser = [];
                    this.PaymentTermLeftAmt = (parseInt(this.budgetAmt) - parseInt(this.dcExistingPymtTermSum));
                    this.$refs.paymentForm.resetValidation();
                },

                /// <summary>
                ///  AddPaymentTermAmt :- Function used to Add Payment Term Amount
                /// </summary>
                /// <param name="Amt"></param>
                /// <returns> </returns>
                AddPaymentTermAmt(Amt) {
                    this.amountErr = false;
                    this.PaymentTermLeftAmt = (parseInt(this.budgetAmt) - parseInt(this.dcExistingPymtTermSum));
                    if (this.PaymentTerm.PaymentTermList.length > 0) {
                        var Temp = this.PaymentTerm.PaymentTermList.filter((x) => x.Index != this.stepper);
                        var TotalAmt = 0;
                        if (Temp.length > 0) {
                            for (var i = 0; i <= Temp.length - 1; i++) {
                                TotalAmt += parseInt(Temp[i].amount);
                            }
                        }
                        this.PaymentTermLeftAmt = (parseInt(this.budgetAmt) - parseInt(this.dcExistingPymtTermSum)) - parseInt(TotalAmt);
                    }
                    if (parseInt(this.amount) > this.PaymentTermLeftAmt) {
                        this.amountErr = true;
                        return false;
                    }
                    this.PaymentTermLeftAmt = parseInt(this.PaymentTermLeftAmt) - parseInt(Amt||0);
                    this.totermsAmt = parseInt(this.totermsAmt) + parseInt(Amt||0);
                },

                /// <summary>
                ///  fnGetPaymentTermsByPaymentId :- Function used to fetch Payment Terms Details based on PaymentId
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnGetPaymentTermsByPaymentId() {
                    var data = new FormData();
                    data.append("PaymentId", this.PaymentId);
                    var result = await objManageProject.GetPaymentTermsByPaymentId(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.data) {
                            this.PaymentInfo = result.data;
                            this.PaymentInfo.users = JSON.parse(result.data.users);
                        }
                },

                /// <summary>
                ///  GetFinanceOrDonarUsers :- Function used to fetch Finance/Donar Users in Create Payment Terms section
                /// </summary>
                /// <param name="index"></param>
                /// <returns> </returns>
                async GetFinanceOrDonarUsers(index) {
                      this.fnAddToTempUserList();
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
                        this.FinanceUsersTemp=[];
                        this.DonarUserTemp=[];
                        if (this.lstFinanceUsers.length > 0) {
                            var item;
                            for (item = 0; item <= this.lstFinanceUsers.length - 1; item++) {
                                this.FinanceUsersTemp.push({ ID: this.lstFinanceUsers[item].ID, Name: this.lstFinanceUsers[item].Name, ProfileImage: this.lstFinanceUsers[item].ProfileImage, IsSelected: false })
                            }
                        }

                        if (this.lstDonors.length > 0) {
                            var item
                            for (item = 0; item <= this.lstDonors.length - 1; item++) {
                                this.DonarUserTemp.push({ ID: this.lstDonors[item].ID, Name: this.lstDonors[item].Name, ProfileImage: this.lstDonors[item].ProfileImage, IsSelected: false })
                            }
                        }
                    }
                },

                /// <summary>
                ///  GetSelectedUserList :- Function used to fetch Selected User List in Create Payment Terms section
                /// </summary>
                /// <param name="userid"></param>
                /// <param name="type"></param>
                /// <param name="event"></param>
                /// <returns> </returns>
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

                /// <summary>
                ///  fnGetProjectDetailsByID :- Function used to fetch Project Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnGetProjectDetailsByID() {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    data.append("InitiativeId", this.InitiativeID);
                    var result = await objManageProject.GetProjectDetailsByID(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.projectInfo = result.data;
                        this.ProjectEndDate = this.projectInfo.EndDate.substr(0, 10)
                        this.ProjectStartDate = this.projectInfo.StartDate.substr(0, 10)
                    } else {
                        this.Msg = ("Failed to get details of Project " + this.ProjectID);
                        this.errorSnackbarMsg = true;
                    }
                },

                /// <summary>
                ///  fnGetPaymentList :- Function used to fetch Payment Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnGetPaymentList() {

                    var result = await objVenture.GetPaymentList(this.ProjectID, 0);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.PaymentList = result.data;
                        this.PaymentfilterList = result.data;
                    }
                },

                /// <summary>
                ///  DownloadDocument :- Function used to Download Transaction Focument in Fund Utilization section
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

                /// <summary>
                ///  fnClickFilterDone :- Function used to filter Budget based on BudgetHeadID
                /// </summary>
                /// <param name="BudgetHeadID"></param>
                /// <returns> </returns>
                fnClickFilterDone(BudgetHeadID) {
                    if (BudgetHeadID != 0) {
                        var templist = [];
                        for (var j = 0; j < this.PaymentList.length; j++) {
                            if (BudgetHeadID == this.PaymentList[j].BudgetHeadID) {
                                templist.push(this.PaymentList[j]);
                            }
                        }
                        this.PaymentfilterList = templist;
                    }
                    else {
                        this.PaymentfilterList = this.PaymentList;
                    }
                },

                /// <summary>
                ///  GetProjectUsers :- Function used to fetch Project User List in Budget & Funds section based on type (Finance/Donor)
                /// </summary>
                /// <param name="Type"></param>
                /// <returns> </returns>
                async GetProjectUsers(Type) {
                    var data = new FormData();
                    data.append("GroupID", this.GroupID);
                    data.append("Type", Type)
                    data.append("WorkspaceID", this.WorkspaceID)
                    var result = await objManageProject.GetNotifyUsers(data);

                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.data) {
                            if (Type == 'Finance')
                                this.lstFinanceUsers = result.data;
                            else if (Type == 'Donor')
                                this.lstDonors = result.data;
                            this.lstDonorItems = result.data;
                        }

                    }
                },

                /// <summary>
                ///  fnFetchFinanceAndDonor :- Function used to fetch Finance and Donor
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnFetchFinanceAndDonor() {
                    this.GetProjectUsers('Finance');
                    this.GetProjectUsers('Donor');
                },

                /// <summary>
                ///  GetDonorDetails :- Function used to fetch Donor Account Details based on ProjectID,UserID
                /// </summary>
                /// <param name="UserID"></param>
                /// <returns> </returns>
                async GetDonorDetails(UserID) {
                    var data = new FormData();
                    data.append("ProjectID", this.ProjectID);
                    data.append("UserID", UserID);
                    var result = await objProject.GetDonorAccountDetails(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.data)
                        this.lstAccountItems = result.data;
                    }
                },

                /// <summary>
                ///  fnGetPaymentTermsByprjID :- Function used to fetch Payment Terms Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnGetPaymentTermsByprjID() {

                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objManageProject.GetPaymentTermsByprjID(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.data) {
                            this.PaymentTermsitem = result.data;
                            var sum = 0;
                            for (var i = 0; i < this.PaymentTermsitem.length; i++) {
                                sum = sum + parseInt(this.PaymentTermsitem[i].Amount)
                                if (this.PaymentTermsitem[i].PaymentStatus == 'Done') {
                                    this.PaymentTermsitem[i].Color = '#47BB76';
                                    this.PaymentTermsitem[i].Icon = 'mdi-check';
                                }
                                else if (this.PaymentTermsitem[i].PaymentStatus == 'Cancelled') {
                                    this.PaymentTermsitem[i].Color = '#D50808';
                                    this.PaymentTermsitem[i].Icon = 'mdi-close';
                                }

                                else if (this.PaymentTermsitem[i].PaymentStatus == 'Pending') {
                                    this.PaymentTermsitem[i].Color = '#d4d6d5';
                                    this.PaymentTermsitem[i].Icon = '';
                                }

                            }
                            this.dcExistingPymtTermSum = sum;
                            this.PaymentTermLeftAmt = this.budgetAmt - sum
                        }
                },

                /// <summary>
                ///  SavePaymentRequest :- Function used to Save Payment Request
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async SavePaymentRequest() {
                            this.PreLoader = true;
                            var data = new FormData();
                            data.append("ProjectId", this.ProjectID);
                            data.append("PaymentID", this.PaymentId);
                            data.append("DonorUser", this.selectedDonor);
                            data.append("RequestAmount", this.reqAmount);
                            data.append("Comment", this.Description);
                            if(this.fileInput !=null && this.fileInput != undefined)
                            {
                               data.append("ReqDocument", this.fileInput.name);
                            }
                            data.append("ProjectName", this.projectInfo.ProjectName);
                            var result = await objProject.SavePaymentRequest(data);

                            if (result.status == "Unhandled Exception") {
                                this.$router.push({ name: "error" });
                            } else {
                                if (result.status == "success") {
                                    this.UploadDocument();
                                    this.Msg = "Payment Requested successfully";
                                    this.successSnackbarMsg = true;

                                }
                                else {
                                    this.Msg = "Failed";
                                    this.errorSnackbarMsg = true;
                                }

                                this.ClearRequestPayment();
                            }
                            this.PreLoader = false;
                },

                /// <summary>
                ///  UploadDocument :- Function used to Upload Document
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
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
                    formData.append('LinkedTo', this.projectInfo.ProjectName);

                    var result = await objProject.UploadDocument(formData);

                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.status != "success") {
                            //this.Msg = "Document uploaded successfully...";
                            this.Msg = "Failed in uploading document...";
                            this.errorSnackbarMsg = true;
                        }

                    }

                },

                /// <summary>
                ///  ClearRequestPayment :- Function used to close Request Payment dialog and clear variables
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                ClearRequestPayment() {
                    this.dlgRequestPayment = false;
                    this.selectedDonor = [];
                    this.reqAmount = 0;
                    this.Description = "";
                    this.fileInput = null;
                },
                       //====================Budget Donor Fund================================//

                /// <summary>
                ///  SaveDonarFund :- Function used to Save/Edit Donor details based on BudgetFundID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async SaveDonarFund() {
                    if (this.BudgetFundID == 0) {
                        this.SaveDonorDetails();
                    } else if (this.BudgetFundID > 0) {
                        this.EditDonarDetail();
                    }
                },

                /// <summary>
                ///  SaveDonorDetails :- Function used to Add/Save Donor in Budget & Funds section
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async SaveDonorDetails() {
                    var temp;
                    temp = this.DonarFundList.filter(x => x.UserID == this.selectedDonor);

                    if (temp.length <=0) {
                        var data = new FormData();
                        data.append("ProjectID", this.ProjectID);
                        data.append("UserID", this.selectedDonor);
                        data.append("DonorBudgetAmount", this.Amount);
                        data.append("AccountId", this.selectedAccount);
                        data.append("BudgetFundID", this.BudgetFundID);
                        data.append("UserName", this.selectedAccount);
                        data.append("AccountName", this.BudgetFundID);


                        var result = await objFinance.SaveDonarFund(data);
                        if (result.status == "Unhandled Exception")
                            this.$router.push({ name: "error" });
                        else {
                            if (result.status == "success") {
                                this.Msg = "Donor added successfully";
                                this.successSnackbarMsg = true;
                            }

                            this.CloseAssignDonor();
                            this.GetProjectBudgetDetails();
                        }
                    }
                    else {
                        this.Msg = "Donor already exist"
                        this.errorSnackbarMsg = true;
                    }
                },

                /// <summary>
                ///  EditDonarDetail :- Function used to Update Donor details in Budget & Funds section
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async  EditDonarDetail() {
                    var temp;
                    temp = this.DonarFundList.filter(x => x.UserID == this.selectedDonor);

                    if (this.editDonor == this.selectedDonor) {
                        temp.length =0
                    }
                        if (temp.length <= 0) {
                            var data = new FormData();
                            data.append("ProjectID", this.ProjectID);
                            data.append("UserID", this.selectedDonor);
                            data.append("DonorBudgetAmount", this.Amount);
                            data.append("AccountId", this.selectedAccount);
                            data.append("BudgetFundID", this.BudgetFundID);
                            data.append("UserName", this.selectedDonorName);
                            data.append("AccountName", this.selectedAccountName);

                            var result = await objFinance.SaveDonarFund(data);
                            if (result.status == "Unhandled Exception")
                                this.$router.push({ name: "error" });
                            else {
                                if (result.status == "success") {
                                    this.Msg = "Donor details Updated successfully";
                                    this.successSnackbarMsg = true;
                                }

                                this.CloseAssignDonor();
                                this.GetProjectBudgetDetails();
                            }
                        }

                        else {
                            this.Msg = "Donor already exist";
                            this.errorSnackbarMsg = true;
                        }
                },

                /// <summary>
                ///  CloseAssignDonor :- Function used to close AssignDonor dialog in Budget & Funds section
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                CloseAssignDonor() {
                    this.selectedDonor = '';
                    this.Amount = 0;
                    this.selectedAccount = '';
                    this.BudgetFundID = 0;
                    this.DonarFundList = [];
                },

                /// <summary>
                ///  EditDonor :- Function used to Edit Selected Donor Details in Budget & Funds section
                /// </summary>
                /// <param name="item"></param>
                /// <returns> </returns>
                EditDonor(item) {
                    this.GetProjectUsers('Donor');
                    this.selectedDonor = item.UserID;
                    this.selectedDonorName = item.UserName;
                    this.editDonor = item.UserID;
                    this.Amount = item.DonorBudgetAmount;
                    this.selectedAccount = item.AccountId;
                    this.selectedAccountName = item.AccountName
                    this.dlgAddDonor = true;
                    this.BudgetFundID = item.BudgetFundID;
                    this.GetDonorDetails(this.selectedDonor);

                },

                /// <summary>
                ///  RemoveBudgetUser :- Function used to Delete selected donor in Budget & Funds section based on BudgetFundID
                /// </summary>
                /// <param name="BudgetFundID"></param>
                /// <returns> </returns>
                async RemoveBudgetUser(BudgetFundID) {
                    var data = new FormData();
                    data.append("ProjectID", this.ProjectID);
                    data.append("BudgetFundID", BudgetFundID);
                    data.append("DonorName", this.DUserName);
                    var result = await objFinance.RemoveBudgetUser(data);

                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.status == "success")
                            this.RemoveUserDailog = false;
                        this.Msg = "Donor removed successfully";
                        this.successSnackbarMsg = true;
                        this.DonarFundList= [],
                        this.GetProjectBudgetDetails();
                    }
                },

                //async GetTotalFundById() {

                //    var data = new FormData();
                //    data.append("ProjectId", this.ProjectID);
                //    var result = await objVenture.GetTotalFundById(data);
                //    if (result.status == "Unhandled Exception") {
                //        this.$router.push({ name: "error" });
                //    }
                //    else
                //        if (result.data) {
                //            this.TotalFundedAmount = result.data[0].TotalFundedAmount;
                //        }
                //},

                /// <summary>
                ///  fnGetLedgerBookExpenseTagsById :- Function used to fetch Ledger Book Expense Tag List based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnGetLedgerBookExpenseTagsById() {
                    this.ExpenseTagList = [];
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objVenture.GetLedgerBookExpenseTagsById(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.ExpenseTagList = result.data;
                        this.ExpenseTagList.push({ TagName: 'All', ExpenseTagID: 0 });
                    }
                },

                /// <summary>
                ///  GetPaymentBudgetHead :- Function used to fetch Payment Budget Head Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async GetPaymentBudgetHead() {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objFinance.GetPaymentBudgetHead(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.fnFetchFundHead(result.data);
                    }
                },

                /// <summary>
                ///  fnFetchFundHead :- Function used to fetch Head Amount based on listitem
                /// </summary>
                /// <param name="listitem"></param>
                /// <returns> </returns>
                async fnFetchFundHead(listitem) {
                    this.ExpenseList = [];
                    this.DonarUtilizedFund = 0;
                    this.UnUtilizedFund = 0;
                    if (listitem.length > 0) {
                        var i = 0;
                        for (i = 0; i <= listitem.length - 1; i++) {
                            this.generator();
                            this.ExpenseList.push({ BudgetHeadName: listitem[i].BudgetHeadName, color: this.fixedColor[i]||this.mycolor, HeadAmount: listitem[i].HeadAmount });
                            this.DonarUtilizedFund = parseInt(this.DonarUtilizedFund) + parseInt(listitem[i].HeadAmount);
                        }
                        this.UnUtilizedFund = this.budgetAmt - this.DonarUtilizedFund;
                        if (this.UnUtilizedFund <= 0) {
                            this.UnUtilizedFund = 0;
                        }
                    }
                    this.fnfillExpensechartdata();
                },

                /// <summary>
                ///  fnfillExpensechartdata :- Function used to fill data in Expense Chart
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnfillExpensechartdata() {
                    if (this.ExpenseList.length > 0) {
                        var labelsArray = ["Unutilized Fund"];
                        var amountArray = [this.UnUtilizedFund];
                        var labels = ["#E5E5E5"];
                        for (var i = 0; i < this.ExpenseList.length; i++) {
                            labelsArray.push(this.ExpenseList[i] ? this.ExpenseList[i].BudgetHeadName : '');
                            amountArray.push(this.ExpenseList[i] ? this.ExpenseList[i].HeadAmount : '');
                            labels.push(this.ExpenseList[i] ? this.ExpenseList[i].color : '');
                        }
                        var ctx = document.getElementById("ExpenseChart")
                        var stateOverviewGraph2 = new Chart(ctx, {
                            type: 'doughnut',
                            data: {
                                labels: labelsArray,
                                datasets: [{
                                    data: amountArray,
                                    backgroundColor: labels
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

                /// <summary>
                ///  GetBudgetHeadDetails :- Function used to fetch Budget Head Details based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async GetBudgetHeadDetails() {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objFinance.GetBudgetHeadDetails(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.data)
                            this.BudgetHeadList = result.data;
                        this.BudgetHeadList.push({ BudgetHeadName: 'All', BudgetHeadID: 0 });
                    }
                },

                //Budget Plan

                /// <summary>
                ///  AddHeaderItem :- Function used to Add Header Item when click on plus button in Add Budget Plan Section
                /// </summary>
                /// <param name="HeaderItem"></param>
                /// <returns> </returns>
                AddHeaderItem(HeaderItem) {
                    if (this.$refs.budgetForm.validate() === true) {
                        this.BudgetItem.BudgetItemList.push({ SubHeaderName: '', Amount: null });
                        this.$refs.budgetForm.resetValidation()
                        this.UnaccountedAmt = this.UnaccountedAmt - parseInt(HeaderItem.Amount)
                    }
                },

                /// <summary>
                ///  DeleteHeaderItem :- Function used to Delete Header Item in Add Budget Plan Section
                /// </summary>
                /// <param name="HeaderItem"></param>
                /// <returns> </returns>
                DeleteHeaderItem: function (HeaderItem) {
                    this.UnaccountedAmt = this.UnaccountedAmt + (Number(HeaderItem.Amount) || 0)
                    this.BudgetItem.BudgetItemList.splice(this.BudgetItem.BudgetItemList.indexOf(HeaderItem), 1);
                },

                /// <summary>
                ///  fnSetpperIncrementBudgetAndPlan :- Function used to Increment the Step in Add Budget Plan section
                /// </summary>
                /// <param name="Mode"></param>
                /// <returns> </returns>
                async  fnSetpperIncrementBudgetAndPlan(Mode) {
                    if (Mode == 'Plus') {
                        if (this.$refs.budgetForm.validate() === true && this.UnaccountedAmt > 0) {
                            this.BudgetAndPlanstepper = this.BudgetAndPlanstepper + 1;
                            this.fnInvokeBudgetItemList(this.BudgetAndPlanstepper)
                            this.$refs.budgetForm.resetValidation()
                        }
                    } else if (Mode == 'Minus') {
                        this.BudgetAndPlanstepper = this.BudgetAndPlanstepper - 1;
                            this.fnInvokePrevBudgetItemList(this.BudgetAndPlanstepper)
                            this.$refs.budgetForm.resetValidation()

                    }
                },

                /// <summary>
                ///  fnSaveBudgetPlan :- Function used to Create New Budget Plan
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnSaveBudgetPlan() {
                    if (this.$refs.budgetForm.validate() === true) {
                        this.fnInvokeBudgetItemList(this.NoOfBudgetPlans)
                        var data = new FormData();
                        data.append("NoOfBudgetPlans", this.NoOfBudgetPlans);
                        data.append("BudgetDetails", JSON.stringify(this.BudgetPlan.BudgetPlanList));
                        data.append("ProjectId", this.ProjectID);
                        var result = await objFinance.SaveBudgetPlan(data);
                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        } else
                            if (result.status == "success") {
                                this.Msg = "Budget Plan created successfully";
                                this.successSnackbarMsg = true;
                                this.CloseBudgetPlanDialog();
                                this.FetchBudgetPlan();
                                this.fnGetProjectDetailsByID();
                                this.$refs.budgetForm.resetValidation()
                            }
                            else {
                                this.Msg = "Failed";
                                this.errorSnackbarMsg = true;
                            }
                    }
                },

                /// <summary>
                ///  fnUpdateBudgetPlan :- Function used to Update Budget Plan
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnUpdateBudgetPlan() {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    data.append("BudgetId", this.EditBudgetId);
                    data.append("BudgetPlanHeader", this.EditBudgetPlanHeader);
                    data.append("ProjectId", this.ProjectID);
                    var result = await objFinance.UpdateBudgetPlan(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else
                        if (result.status == "success") {
                            this.FetchBudgetPlan();
                            this.Msg = "Successfully updated";
                            this.successSnackbarMsg = true;
                            this.dlgEditBudgetPlan = false;
                            this.EditBudgetPlanHeader = "";


                        }
                        else {
                            this.Msg = "Failed";
                            this.errorSnackbarMsg = true;
                        }
                },

                /// <summary>
                ///  fnUpdateBudgetPlanItem :- Function used to Update Budget Plan Sub Item
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnUpdateBudgetPlanItem() {

                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    data.append("BudgetItemID", this.EditBudgetItemID);
                    data.append("ItemName", this.EditBudgetPlanSubHeader);
                    data.append("Amount", this.EditBudgetAmount);
                    var result = await objFinance.UpdateBudgetPlanitem(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else
                        if (result.status == "success") {
                            this.FetchBudgetPlan();
                            this.Msg = "Successfully updated";
                            this.successSnackbarMsg = true;
                            this.dlgEditBudgetPlanSubHeader = false;
                            this.EditBudgetPlanSubHeader = "";
                            this.EditBudgetAmount = 0;


                        }
                        else {
                            this.Msg = "Failed";
                            this.errorSnackbarMsg = true;
                        }
                },

                /// <summary>
                ///  CloseBudgetPlanDialog :- Function used to close Budget Plan dialog
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                CloseBudgetPlanDialog() {
                    this.NoOfBudgetPlans = "";
                    this.BudgetHeadName = "";
                    this.dlgCreateBudgetPlan = false;
                    this.CreateBudgetAndPlanStepOne = true;
                    this.CreateBudgetAndPlanStepTwo = false;
                    this.BudgetPlan.BudgetPlanList = [];
                    this.AddPayment.ExpenseTags = [];
                    this.BudgetAndPlanstepper = 1;
                    this.UnaccountedAmt = this.UnaccountedAmtCpy;
                    this.BudgetItem.BudgetItemList = [{}];
                    this.$refs.budgetForm.resetValidation();
                },

                /// <summary>
                ///  AddBudgetPlanAmt :- Function used to Add Budget Plan Amount
                /// </summary>
                /// <param name="Amt"></param>
                /// <returns> </returns>
                AddBudgetPlanAmt(Amt) {
                    this.toBudgetAmt = parseFloat(this.toBudgetAmt) + parseFloat(Amt||0);
                    this.TotalBudgetPlanAmt = parseInt(this.TotalBudgetPlanAmt) + parseInt(Amt||0);
                },

                /// <summary>
                ///  fnCheckBudgetMandatory :- Function used to check any mandatory field is showing blank or null before moving to create another Budget Head
                /// </summary>
                /// <param name=""></param>
                /// <returns> true/false </returns>
                fnCheckBudgetMandatory() {
                    if (this.BudgetItem.BudgetItemList.length > 0 && this.BudgetHeadName.length > 0) {
                        return false;
                    }
                    return true;
                },

                /// <summary>
                ///  fnInvokeBudgetItemList :- Function used to show Budget Item data if data present otherwise shows as empty when click on right the arrow
                /// </summary>
                /// <param name="index"></param>
                /// <returns> </returns>
                async fnInvokeBudgetItemList(index) {
                    if (this.BudgetPlan.BudgetPlanList.length > 0) {
                        var LastItemIndex = this.BudgetPlan.BudgetPlanList[this.BudgetPlan.BudgetPlanList.length - 1].Index;
                    }
                    if (index > 0) {
                        var temp = this.BudgetPlan.BudgetPlanList.filter(x => x.Index == index);
                        if (temp.length > 0) {

                            this.BudgetHeadName = temp[0].BudgetHeadName;
                            this.toBudgetAmt = temp[0].HeadAmount;
                            this.BudgetItem.BudgetItemList = temp[0].BudgetPlanSubList;
                        } else if ((index - 1) !== LastItemIndex) {
                            this.BudgetPlan.BudgetPlanList.push({ Index: index - 1, BudgetHeadName: this.BudgetHeadName, HeadAmount: this.toBudgetAmt, BudgetPlanSubList: this.BudgetItem.BudgetItemList });
                            this.UnaccountedAmt = this.UnaccountedAmt - parseInt(this.BudgetItem.BudgetItemList[this.BudgetItem.BudgetItemList.length - 1].Amount)
                            this.BudgetHeadName = "";
                            this.BudgetItem.BudgetItemList = "",
                                this.toBudgetAmt = 0;
                            this.BudgetItem = {
                                BudgetItemList: [{ SubHeaderName: "", Amount: "" }]
                            }
                        } else {
                            this.BudgetHeadName = "";
                            this.BudgetItem.BudgetItemList = "",
                                this.toBudgetAmt = 0;
                            this.BudgetItem = {
                                BudgetItemList: [{ SubHeaderName: "", Amount: "" }]
                            }
                        }
                    }
                },

                /// <summary>
                ///  fnInvokePrevBudgetItemList :- Function used to show Previous Budget Item data
                /// </summary>
                /// <param name="index"></param>
                /// <returns> </returns>
                async fnInvokePrevBudgetItemList(index) {
                    if (index + 1 == this.NoOfBudgetPlans) {
                        if (this.BudgetHeadName != "") {
                            this.BudgetPlan.BudgetPlanList.push({ Index: this.NoOfBudgetPlans, BudgetHeadName: this.BudgetHeadName, HeadAmount: this.toBudgetAmt, BudgetPlanSubList: this.BudgetItem.BudgetItemList });

                        }
                    }
                    if (index > 0) {

                        var temp = this.BudgetPlan.BudgetPlanList.filter(x => x.Index == index);
                        if (temp.length > 0) {
                            this.BudgetHeadName = temp[0].BudgetHeadName;
                            this.toBudgetAmt = temp[0].HeadAmount;
                            this.BudgetItem.BudgetItemList = temp[0].BudgetPlanSubList;

                        }
                    }
                },
                
                /// <summary>
                ///  fnCheckForParentNode :- Function used to edit Budget Plan Header / Budget Plan Sub Header based on Props
                ///  i.e. Budget Plan with sub-activities/Budget Plan with no sub-activities
                /// </summary>
                /// <param name="Props"></param>
                /// <returns> </returns>
                fnCheckForParentNode(Props) {
                    if (Props.depth > 0) {
                        this.fnFetchBudgetSubHeader(Props.rowData);
                        this.dlgEditBudgetPlanSubHeader = true;
                    }
                    else {
                        this.fnFetchBudgetHeader(Props.rowData);
                        this.dlgEditBudgetPlan = true;
                    }
                },

                /// <summary>
                ///  fnRemoveForParentNode :- Function used to delete Budget Plan Header / Budget Plan Sub Header based on Props
                ///  i.e. Budget Plan with sub-activities/Budget Plan with no sub-activities
                /// </summary>
                /// <param name="Props"></param>
                /// <returns> </returns>
                fnRemoveForParentNode(Props) {
                    if (Props.depth > 0) {
                        this.fnFetchBudgetSubHeader(Props.rowData);
                        this.dlgRemoveSubItem = true;
                    }
                    else {
                        this.fnFetchBudgetHeader(Props.rowData);
                        this.dlgRemoveHeader = true;
                    }

                },

                /// <summary>
                ///  fnFetchBudgetHeader :- Function used to fetch Budget Header Name and ID based on RowData
                /// </summary>
                /// <param name="RowData"></param>
                /// <returns> </returns>
                async fnFetchBudgetHeader(RowData) {
                    this.EditBudgetPlanHeader = RowData.Name;
                    this.EditBudgetId = RowData.BudgetHeadID;

                },

                /// <summary>
                ///  fnFetchBudgetSubHeader :- Function used to fetch Budget Sub Header Name, Amount and ID based on RowData
                /// </summary>
                /// <param name="RowData"></param>
                /// <returns> </returns>
                async fnFetchBudgetSubHeader(RowData) {
                    this.EditBudgetPlanSubHeader = RowData.Name;
                    this.EditBudgetAmount = RowData.Amount;
                    this.EditBudgetItemID = RowData.BudgetItemID;

                },

                /// <summary>
                ///  RemoveBudgetPlan :- Function used to Remove Budget Plan Header
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async RemoveBudgetPlan() {
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    data.append("BudgetId", this.EditBudgetId);
                    data.append("BudgetPlanHeader", this.EditBudgetPlanHeader);
                    var result = await objFinance.RemoveBudgetPlan(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success" && result.data == 0) {
                        this.Msg = "Budget and Plan Header removed succesfully";
                        this.successSnackbarMsg = true;
                        this.dlgRemoveHeader = false;
                        this.FetchBudgetPlan();
                    }else{
                        this.dlgRemoveHeader = false;
                        this.Msg = "Cannot Remove Budget Plan Header, Payment already added";
                        this.infoSnackbarMsg = true;
                        }
                },

                /// <summary>
                ///  RemoveBudgetPlanItems :- Function used to Remove Budget Plan Sub Header Item
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async RemoveBudgetPlanItems() {

                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    data.append("BudgetItemId", this.EditBudgetItemID);
                    data.append("ItemName", this.EditBudgetPlanSubHeader);
                    var result = await objFinance.RemoveBudgetPlanItems(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success" && result.data == 0) {
                        this.Msg = "Item removed succesfully";
                        this.successSnackbarMsg = true;
                        this.dlgRemoveSubItem = false;
                        this.FetchBudgetPlan();
                    }else{
                        this.dlgRemoveHeader = false;
                        this.Msg = "Cannot Remove Budget Plan Header, Payment already added";
                        this.infoSnackbarMsg = true;
                    }
                },

                /// <summary>
                ///  FetchBudgetPlan :- Function used to fetch Budget Plan based on ProjectID
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async FetchBudgetPlan() {
                    var intCount = 0;
                    var data = new FormData();
                    data.append("ProjectId", this.ProjectID);
                    var result = await objFinance.FetchBudgetPlan(data);
                    this.UnaccountedAmt = this.budgetAmt;
                    this.UnaccountedAmtCpy = this.budgetAmt;
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        var jsonString = "";
                        this.ProjectHeadTableItem = [];
                        for (intCount = 0; intCount <= result.data.length - 1; intCount++) {
                            jsonString = result.data[intCount].js;
                            var obj = JSON.parse(jsonString);
                            this.ProjectHeadTableItem.push(obj);
                        }
                        var sum = 0;
                        for (var i = 0; i < this.ProjectHeadTableItem.length; i++) {
                            sum = sum + this.ProjectHeadTableItem[i].Amount
                            this.UnaccountedAmt = this.budgetAmt - sum;
                            this.UnaccountedAmtCpy = this.budgetAmt - sum;
                        }
                        this.fnFetchBudgetPlanitem(this.ProjectHeadTableItem);
                    }
                    console.log(this.ProjectHeadTableItem);
                },

                /// <summary>
                ///  fnFetchBudgetPlanitem :- Function used to fetch Budget Plan Item
                /// </summary>
                /// <param name="listitem"></param>
                /// <returns> </returns>
                async fnFetchBudgetPlanitem(listitem) {
                    this.BudgetPlanHeadList = [];
                    if (listitem.length > 0) {
                        var i = 0;
                        var j = 0;
                        for (i = 0; i <= listitem.length - 1; i++) {
                            this.generator();
                            this.BudgetPlanHeadList.push({ Name: listitem[i].Name, color: this.fixedColor[i] || this.mycolor, TransactionAmount: listitem[i].Amount });
                        }
                    }
                    this.fnFillBudgetPlan();
                },

                /// <summary>
                ///  fnFillBudgetPlan :- Function used to fill Budget Plan Chart
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                async fnFillBudgetPlan() {
                    var labelsArray = ["Unaccounted"];
                    var amountArray = [this.UnaccountedAmt];
                    var labels = ["#E5E5E5"];
                    for (var i = 0; i < this.BudgetPlanHeadList.length; i++) {
                        labelsArray[i+1] = (this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].Name : '');
                        amountArray[i + 1]=(this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].TransactionAmount : '');
                        labels[i + 1]=(this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].color : '');
                    }
                    var ctx = document.getElementById("myBudgetPlan")
                    var stateOverviewGraph2 = new Chart(ctx, {
                        type: 'doughnut',
                        data: {
                            labels: labelsArray,
                            datasets: [{
                                data: amountArray,
                                backgroundColor: labels
                            }],
                        },
                        options: {
                            legend: {
                                display: false,
                            },
                        }
                    });
                },
            },
            computed: {
                ...mapGetters('DefaultStore',["getSelectedCurrency"]),
            },
  }
</script>
<style scoped>
    .v-timeline::before {
        top: 40px;
    }
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