<template>
    <v-card height="100%" elevation="0">
        <v-card-title class="page-head">
          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
            <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
          </v-avatar>
          Finance Details
        </v-card-title>
        <v-card-text>
            <v-row>
                <v-col md="6">
                    <v-card class="custom-card-1">
                        <v-card-title style="height:10vh">
                            {{BudgetTile}}
                            <v-spacer></v-spacer>
                            <h4 class="text-center black--text heading-3">
                                <v-btn id="AutoTest_V20"  fab="" x-small  class="elevation-1 mr-2" color="#fff" @click="fnNextChart('Minus')" v-bind:disabled="BudgetAndFinanceSteps == 1">
                                    <v-icon size="25" color="primary">mdi-arrow-left</v-icon>
                                </v-btn>
                                <v-btn id="AutoTest_V21" fab="" x-small class="elevation-1 ml-2" color="#fff" @click="fnNextChart('Plus')" v-bind:disabled="BudgetAndFinanceSteps == 3 ||BudgetAndFinanceSteps == 2 ">
                                    <v-icon size="25" color="primary">mdi-arrow-right</v-icon>
                                </v-btn>
                            </h4>
                        </v-card-title>
                        <v-card-text style="height:310px" class="v-scrolling-div pa-0">
                            <v-stepper v-model="BudgetAndFinanceSteps" class="elevation-0">
                                <v-stepper-items>
                                    <!-- Budget and Funds-->
                                    <v-stepper-content step="1" class="pa-0">
                                        <v-row>
                                            <v-col md="6">
                                                <v-row>
                                                    <v-col>
                                                        <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                            <canvas id="myChart" style="width:175px; height:150px"></canvas>
                                                        </div>
                                                    </v-col>
                                                </v-row>
                                                <v-row>
                                                      <v-col>
                                                            <h4 class="font-weight-regular heading-3-dark text-center">
                                                                Total budget<br />
                                                                            <span class="bold">{{ getSelectedCurrency.symbol }} {{budgetAmt}}</span>
                                                            </h4>
                                                       </v-col>
                                                       <v-col>
                                                            <h4 class="font-weight-regular heading-3-dark text-center px-0" >
                                                                    Fund Status<br />
                                                                    <span v-if="TotalFundedAmount == budgetAmt && budgetAmt != 0">
                                                                        <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                                                            <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                                                        </v-avatar>
                                                                        Fully Funded
                                                                    </span>
                                                                    <span v-if="TotalFundedAmount< budgetAmt && TotalFundedAmount != 0">
                                                                        <v-avatar color="#FEEEC2" size="15" class="mr-1">
                                                                            <v-icon size="15" color="#FCC735">mdi-minus</v-icon>
                                                                        </v-avatar>
                                                                        Partially Funded
                                                                    </span>
                                                                    <span v-if="TotalFundedAmount == 0 ">
                                                                        <v-avatar color="#FDE4E4" size="20" class="mr-1">
                                                                            <v-icon size="15" color="#F24646">mdi-close-circle-outline</v-icon>
                                                                        </v-avatar>
                                                                        Not Funded
                                                                    </span>
                                                                </h4>
                                                            </v-col>
                                                    </v-row>
                                            </v-col>
                                            <v-col md="6">
                                                <div class="mt-1 mb-3 ml-5" v-if="lstBudgetFinUserInfo.length>0">
                                                    <span class="small-text mr-1 black--text">
                                                        <v-icon size="20" color="#000">mdi-account</v-icon> Finance User
                                                    </span>
                                                    <span>
                                                        <!--<v-avatar v-if="lstBudgetFinUserInfo.length>=1 " size="35" class="team-avatar">
                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Finuser name" />
                                                        </v-avatar>
                                                        <v-avatar v-if="lstBudgetFinUserInfo.length>=2 " size="35" class="team-avatar" color="pink">
                                                            <span class="white--text headline">{{ lstBudgetFinUserInfo[0].UserName.charAt(0) }}</span>
                                                        </v-avatar>
                                                        <v-avatar v-if="lstBudgetFinUserInfo.length>=3 " size="35">
                                                            <span> +{{ lstBudgetFinUserInfo.length-2 }}</span>
                                                        </v-avatar>-->
                                                        <v-avatar size="25" class="team-avatar" v-for="(item,key) in lstBudgetFinUserInfo.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                        <v-avatar v-if="lstBudgetFinUserInfo.length>=4" size="25" class="team-count">
                                                            +{{lstBudgetFinUserInfo.length-3}}
                                                        </v-avatar>
                                                    </span>
                                                </div>
                                                <v-list class="v-scrolling-div py-0 px-0" style="max-height:25vh;overflow-y:auto">
                                                    <v-list-item-group v-model="BudgetAndFundModel" color="primary" class="py-0 px-0">
                                                        <v-list-item v-for="(item, UserID) in VentureList"
                                                                     :key="UserID" dense class="py-0 px-0">
                                                            <div class="pr-2">
                                                                <v-avatar :color="item.color" size="15" tile="tile">
                                                                </v-avatar>
                                                            </div>
                                                            <v-avatar size="27" class="team-avatar ml-0 mr-2" :color="`${getcolor(item.UserName)}`">
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                            <v-list-item-content>
                                                                <v-list-item-title v-text="item.UserName" class="font-weight-regular heading-3-dark"> </v-list-item-title>
                                                                <v-list-item-title class="font-weight-regular heading-3-dark">Amount Funded : {{ getSelectedCurrency.symbol }} {{item.FundedAmount}}</v-list-item-title>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list-item-group>
                                                    <template>
                                                        <h4 class="text-left heading-3-dark py-2">
                                                            <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                                            <v-avatar size="15" class="team-avatar mr-1" color="#E5E5E5" tile></v-avatar>
                                                            Not Funded Amount : {{ getSelectedCurrency.symbol }} {{this.NotFundedAmount}}
                                                        </h4>
                                                    </template>
                                                </v-list>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!--- Fund Utilization -->
                                    <v-stepper-content step="2" class="pa-0">
                                        <!--For Utilization when data exist chart-->
                                        <v-row v-show="ExpenseList.length > 0">
                                            <v-col md="6">
                                                <v-row>
                                                    <v-col>
                                                        <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                            <canvas id="ExpenseChart" style="width:175px; height:150px"></canvas>
                                                        </div>
                                                    </v-col>
                                                </v-row>
                                                <v-row>
                                                    <v-col>
                                                        <h5 class="font-weight-regular text-center heading-3-dark">
                                                            Total budget<br />
                                                            <span class="bold">{{ getSelectedCurrency.symbol }} : {{budgetAmt}}</span>
                                                        </h5>
                                                    </v-col>
                                                    <v-col>
                                                        <h5 class="font-weight-regular text-center heading-3-dark">
                                                            Fund Status<br />
                                                            <span v-if="TotalFundedAmount == budgetAmt">
                                                                <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                                                    <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                                                </v-avatar>
                                                                Fully Funded
                                                            </span>
                                                            <span v-if="TotalFundedAmount< budgetAmt && TotalFundedAmount != 0">
                                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                                    <v-icon size="15" color="#FCC735">mdi-minus</v-icon>
                                                                </v-avatar>
                                                                Partially Funded
                                                            </span>
                                                            <span v-if="TotalFundedAmount == 0 ">
                                                                <v-avatar color="#FDE4E4" size="20" class="mr-1">
                                                                    <v-icon size="15" color="#F24646">mdi-close-circle-outline</v-icon>
                                                                </v-avatar>
                                                                Not Funded
                                                            </span>
                                                        </h5>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="text-left mt-1 px-0 heading-3-dark">Legend Title</h4>
                                                <v-row class="mt-1 mr-2 ml-1 px-0  v-scrolling-div" style="max-height:190px">
                                                    <v-col class="px-0 py-1">
                                                        <template v-for="(item, ExpenseTagID) in ExpenseList">
                                                            <h4 class="text-left font-weight-regular heading-3-dark mb-2 mt-1">
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
                                        <v-row v-show="PaymentfilterList.length > 0">
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
                                                            <template v-slot:item.TransactionId="{ item }">
                                                                <div v-if="item.TransactionId" style="max-width:100px">
                                                                    {{ item.TransactionId}}
                                                                </div>
                                                            </template>
                                                            <template v-slot:item.TransactionDate="{item}">
                                                                <div v-if="item.TransactionDate" style="width:85px">
                                                                    {{ formatDate(item.TransactionDate) }}
                                                                </div>
                                                            </template>
                                                            <template v-slot:item.TransactionAmount="{ item }">
                                                                <div v-if="item.TransactionAmount" style="max-width:90px">
                                                                    {{ getSelectedCurrency.symbol }} {{ item.TransactionAmount }}
                                                                </div>
                                                            </template>
                                                            <template v-slot:item.TransactionDocName="{item}">
                                                                <div v-if="item.TransactionDocName" style="max-width:100px" @click="DownloadDocument(item.TransactionDocName,'/Documents/7e23e75d-bd85-4c3d-82ad-a07585551986.jpg')">
                                                                    {{ item.TransactionDocName }}
                                                                    <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                                                                        <v-icon size="10" color="#fff">mdi-file-download-outline</v-icon>
                                                                    </v-avatar>
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
                                                <p class="mt-6 pt-6">
                                                    <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                                </p>
                                                <p class="heading-4-dark font-weight-medium pt-0">No fund utilizations available</p>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- Budget plan only shows in Search Venture-->
                                    <v-stepper-content step="3" class="pa-0">
                                        <v-row v-show="ProjectHeadTableItem.length >0">
                                            <v-col md="6">
                                                <v-row>
                                                    <v-col>
                                                        <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                                            <canvas id="myBudgetPlan" style="width:175px; height:150px"></canvas>
                                                        </div>
                                                    </v-col>
                                                </v-row>
                                                <v-row>
                                                    <v-col>
                                                        <h5 class="font-weight-regular heading-3-dark text-center">
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
                                        </v-row>
                                        <v-row v-show="ProjectHeadTableItem.length >0">
                                            <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
                                                <!--<h5 class="font-weight-regular heading-3-dark mr-4 mb-2">
        Budget Head
    </h5>
    <tree-table class="table tree-table"
                :columns="ProjectHeadTableHeader"
                :table-data="ProjectHeadTableItem">
    </tree-table>-->
                                                <v-card class="custom-card-1 elevation-0 pa-0">
                                                    <v-card-title class="py-1 px-3">
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
                                                                        </tr>
                                                                    </table>
                                                                </template>
                                                                <!-- Rows with sub-activities -->
                                                                <template #nodeTemplate="nodeProps">
                                                                    <div v-bind="nodeProps" class="ml-12">
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
                                                                            <div class="cell text-limit pt-5 mr-7">
                                                                                {{ nodeProps.rowData.Name }}
                                                                            </div>
                                                                            <div class="cell pt-5 ml-12">
                                                                                {{ getSelectedCurrency.symbol }} {{ nodeProps.rowData.Amount }}
                                                                            </div>
                                                                            
                                                                        </div>
                                                                    </div>
                                                                </template>

                                                                <!-- Rows with no sub-activities -->
                                                                <template #leafTemplate="leafProps">
                                                                    <div v-bind="leafProps" class="ml-12">
                                                                        <div class="row ml-12" style="max-height:55px;">
                                                                            <div v-if="leafProps.depth > 0">
                                                                                <div class="indentation" :style="{ width:(step*leafProps.depth)  + 'px' }">

                                                                                </div>
                                                                            </div>
                                                                            <div class="cell pt-5 text-limit mr-7">
                                                                                {{ leafProps.rowData.Name }}
                                                                            </div>
                                                                            <div class="cell pt-5 ml-12">
                                                                                {{ getSelectedCurrency.symbol }} {{ leafProps.rowData.Amount }}
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
                         </v-stepper-items>
                            </v-stepper>
                        </v-card-text>
                    </v-card>
                </v-col>
                <v-col md="6">
                    <v-card class="custom-card-1">
                        <v-card-title style="height:10vh">
                            Payment Terms
                        </v-card-title>
                        <v-card-text v-if="PaymentTermsitem.length==0" style="height:310px" class="v-scrolling-div">
                            <!--If there is no payment-->
                            <v-row justify="center">
                                <v-col class="text-center">
                                    <p class="mt-2 pt-4">
                                        <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                                    </p>
                                    <p class="heading-4-dark font-weight-medium pt-0">No payment terms available</p>
                                </v-col>
                            </v-row>
                        </v-card-text>
                        <v-card-text v-if="PaymentTermsitem.length >0" style="height:310px" class="v-scrolling-div">
                            <v-row class="py-0">
                                <v-col class="py-0">
                                    <v-timeline align-top dense>
                                        <div v-for="item in PaymentTermsitem" :key="item.PaymentId">
                                            <v-timeline-item color="#47BB76" icon="mdi-check" small v-if="item.PaymentStatus=='Done'">
                                                <p class="black--text" v-if="item.Logic == 'Date'">
                                                    Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}), Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>
                                                <p class="black--text" v-else>
                                                    Name :{{item.PaymentTermName}} <br /> Logic :{{item.Logic}}({{item.Target}}), Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>

                                            </v-timeline-item>
                                            <v-timeline-item color="#D50808" icon="mdi-close" small v-if="item.PaymentStatus=='Cancelled'">
                                                <p class="black--text" v-if="item.Logic == 'Date'">
                                                    Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}),Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>
                                                <p class="black--text" v-else>
                                                    Name : {{item.PaymentTermName}} <br /> Logic :{{item.Logic}}({{item.Target}}), Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>
                                            </v-timeline-item>
                                            <v-timeline-item color="#d4d6d5" icon="" small v-if="item.PaymentStatus=='Pending'">
                                                <p class="black--text" v-if="item.Logic == 'Date'">
                                                    Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{new Date(item.paymentdate).toLocaleDateString('en-GB')}}), Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>
                                                <p class="black--text" v-else>
                                                    Name : {{item.PaymentTermName}} <br /> Logic : {{item.Logic}}({{item.Target}}) , Amount : {{ getSelectedCurrency.symbol }} {{item.Amount}}
                                                </p>
                                            </v-timeline-item>

                                        </div>

                                    </v-timeline>
                                </v-col>
                            </v-row>
                        </v-card-text>
                    </v-card>
                </v-col>
            </v-row>
        </v-card-text>
    </v-card>
</template>
<script>
    import { mapGetters } from "vuex";
    import TreeTable from 'vue-tree-table-component'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import SuccessButtonSm from '@/components/success-button-sm.vue'
    import SuccessButton from '@/components/success-button.vue'
    import DoughnutChart from '@/components/DoughnutChart.vue'

    var objProject;
    async function importFinancescript() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then(mod => {
                objProject = new mod.ManageProjectFinance();
            })
        ]);
    };

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

                //Chart Data
                //BudgetChartData:[],
                userimage: "",
                justifyCenter: "center",
                fixedColor: ["#399CFF", "#00C2B0", "#F1C100", "#00B448", "#2C4258", "#8307CB", "#F98B00", "#D10000", "#E6217B", "#6F00B0", "#464646", "#395FFF", "#038E81", "#C99506", "#76B400", "#2C584A", "#521CA8", "#D5D5D5", "#FF6200", "#88320C", "#CE72B9", "#A00000", "#41AFFF", "#00E209", "#D008A9", "#FFD446"],
                BudgetAndFundChartData: {
                    labels: [],
                    datasets: [{
                        data: [60000, 40000],
                        backgroundColor: [
                            '#388E3C',
                            '#FFA200',
                        ],
                    }],
                },

                BudgetPlanChartData: {
                    labels: [],
                    datasets: [{
                        data: [60000, 40000],
                        backgroundColor: [
                            '#388E3C',
                            '#FFA200',
                        ],
                    }],
                },

                BudgetNotFundedChartData: {
                    labels: [],
                    datasets: [{
                        data: [60000, 40000],
                        backgroundColor: [
                            '#388E3C',
                            '#FFA200',
                        ],
                    }],
                },
                BudgetFundUtilizationchartData: {
                    labels: [],
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
                        position: 'bottom',
                        fullWidth: false,
                        align: 'start',
                    }
                },

                AssignDonorSteps: 1,
                SelectBankAccountItems: ['Sample Account 1', 'Sample Account 2'],
                WorkspaceID: 0,
                lstBudgetFinUserInfo: [],
                budgetAmt: 0,
                GroupID: 0,
                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],
                lstuserTableheaders: [{ text: "Name", align: "left", value: "UserName" }],
                AddDonorTableHeaders: [
                    { text: "Name", align: "left", value: "UserName" },
                    { text: '', value: 'data-table-expand' },
                ],
                expanded: [],
                singleExpand: false,
                singleSelect: false,
                lstFinanceUsers: [],
                vmFinanceUsers: [],
                AssignDonarSteps: 1,
                lstDonors: [],
                vmlstDonors: [],
                vmDonors: [],
                lstDonorAccount: [],
                ChkFinNotify: true,
                Msg: "",
                txtDonorAmt: 0,
                vmDonorAccount: [],
                errorMsg: " * Please add Budget Amount",
                isValidBudget: 0,
                InitiativeID: 0,
                ProjectID: 0,
                DonarUtilizedFund: 0,
                UnUtilizedFund: 0,
                TotalFundedAmount: 0,
                NotFundedAmount: 0,
                PaymentTermsitem: [],
                BudgetAndFundModel: 1,
                VentureType: "",
                mycolor: '#' + (Math.random() * 0xFFFFFF << 0).toString(16),
                VentureList: [],
                ExpenseList: [],
                ExpenseTagList: [],
                BudgetPlanModel: 1,
                BudgetPlanHeadList: [],
                BudgetFundUtilizationModel: 1,


                BudgetAndFinanceSteps: 1,
                BudgetTile: "Budget & Fund",
                //payment listing parameters
                LedgerBookTableheaders: [
                    { text: "Title", value: "PaymentActivityName" },
                    { text: "Trans ID", value: "TransactionId" },
                    { text: "Trans Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "Documents", value: "TransactionDocName" },

                ],

                PaymentList: [],
                PaymentfilterList: [],

                ProjectHeadTableHeader: [
                    { label: 'Name', id: 'Name', props: 'Name' },
                    { label: 'Amount', id: 'Amount', props: 'Amount' },
                ],
                ProjectHeadTableItem: [],
                UnaccountedAmt: 0,
                Steps: 1,
                BudgetHeadList: [],
                BudgetHead: {
                    BudgetHeadName: "All", BudgetHeadID: 0
                },
            }
        },
        components: {
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Success-Button-Sm': SuccessButtonSm,
            'Success-Button': SuccessButton,
            'Doughnut-Chart': DoughnutChart,
            TreeTable
        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeID = this.$route.query.InitID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.ProjectID = this.$route.query.ProjID;
            this.VentureType = this.$route.query.VentureType;
            await importFinancescript();
            await importscript();
            await importVenturescript();
            await importMyFinancescript();
            await this.fnGetPaymentTermsByprjID();
            await this.GetProjectBudgetDetails();
            await this.GetFundedUsersById();
            await this.GetTotalFundById();
            await this.fnGetPaymentList();
            await this.FetchBudgetPlan();
            await this.GetPaymentBudgetHead();
            await this.GetBudgetHeadDetails();


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

            generator: function () {
                this.mycolor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);

            },
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.substr(0, 10).split('-')
                return `${day}-${month}-${year}`
            },
            async fnFetchProjectData(ListItem) {
                this.NotFundedAmount = this.budgetAmt
                if (ListItem.length > 0) {
                    var i = 0;
                    var sum = 0;
                    for (i = 0; i <= ListItem.length - 1; i++) {
                        sum = sum + ListItem[i].FundedAmount
                        this.generator();
                        if (this.mycolor == '#ffffff' || this.mycolor == '#000000') {
                            this.generator();
                        }
                        this.VentureList.push({ UserName: ListItem[i].UserName, color: this.fixedColor[i] || this.mycolor, FundedAmount: ListItem[i].FundedAmount, ProfileImage: ListItem[i].ProfileImage });

                    }
                    this.NotFundedAmount = this.budgetAmt - sum
                    this.fnFillChartData();
                }
                else {


                    this.VentureList.push({
                        UserName: 'Not Funded ', color: '#388E3C', FundedAmount: 0, ProfileImage: ""
                    });
                    this.fnFillChartWithNoData();
                }

            },
            async fnFillChartData() {

                if (this.VentureList.length > 0) {
                    var labelsArray = ["NotFundedAmount"];
                    var amountArray = [this.NotFundedAmount];
                    var labels = ["#E5E5E5"];
                    for (var i = 0; i < this.VentureList.length; i++) {
                        labelsArray[i + 1] = (this.VentureList[i] ? this.VentureList[i].UserName : '');
                        amountArray[i + 1] = (this.VentureList[i] ? this.VentureList[i].FundedAmount : '');
                        labels[i + 1] = (this.VentureList[i] ? this.VentureList[i].color : '');
                    }
                    var ctx = document.getElementById("myChart")
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
            async fnFillChartWithNoData() {

                var labelsArray = ['100'];
                var amountArray = ['#388E3C'];
                var labels = ['0'];

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

            },

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
                    }
            },
            async GetProjectBudgetDetails() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objProject.GetProjectBudgetDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data.length > 0) {

                        this.budgetAmt = result.data[0].Budget;
                        //this.GroupID = result.data[0].GroupId;

                        this.lstBudgetFinUserInfo = result.data.filter(x => x.UserType == 'F');
                        //this.lstBudgetDonorUserInfo = result.data.filter(x => x.UserType == 'D');
                    }

                }
            },
            async GetFundedUsersById() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objVenture.GetFundedUsersById(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {

                        this.fnFetchProjectData(result.data);
                    }

                }
            },
            async GetTotalFundById() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objVenture.GetTotalFundById(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {

                        this.TotalFundedAmount = result.data[0].TotalFundedAmount;

                    }
            },
            fnNextChart(type) {

                if (type == 'Plus') {
                    this.Steps = this.Steps + 1;
                    if (this.Steps == 2 && this.VentureType == 'Search Venture') {

                        this.BudgetAndFinanceSteps = 3;

                    } else {
                        this.BudgetAndFinanceSteps = this.Steps;

                    }
                }
                else if (type == 'Minus') {
                    this.Steps = this.Steps - 1;
                    if (this.Steps == 2 && this.VentureType == 'Search Venture') {

                        this.BudgetAndFinanceSteps = 1;

                    } else {
                        this.BudgetAndFinanceSteps = this.Steps;

                    }

                }
                if (this.BudgetAndFinanceSteps == 3) {
                    this.BudgetTile = "Budget Plan";
                } else if (this.BudgetAndFinanceSteps == 1) {
                    this.BudgetTile = "Budget & Fund";
                } else if (this.BudgetAndFinanceSteps == 2) {
                    this.BudgetTile = "Fund Utilization";
                }
            },
            //Expense Tags functions
            //async fnGetAllExpenseTags() {
            //    var data = new FormData();
            //    data.append("ProjectId", this.ProjectID);
            //    var result = await objVenture.GetLedgerBookExpenseTagsById(data);
            //    if (result.status == "Unhandled Exception") {
            //        this.$router.push({ name: "error" });
            //    } else if (result.data) {
            //        this.fnFetchExpenseTag(result.data);
            //        this.ExpenseTagList = result.data;
            //        this.ExpenseTagList.push({ TagName: 'All', ExpenseTagID: 0 });

            //    }
            //},

            //Fund Utilization
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
            async fnFetchFundHead(listitem) {
                this.ExpenseList = [];
                this.DonarUtilizedFund = 0;
                this.UnUtilizedFund = 0;
                if (listitem.length > 0) {

                    var i = 0;
                    for (i = 0; i <= listitem.length - 1; i++) {
                        this.generator();
                        this.ExpenseList.push({ BudgetHeadName: listitem[i].BudgetHeadName, color: this.fixedColor[i] || this.mycolor, HeadAmount: listitem[i].HeadAmount });

                        this.DonarUtilizedFund = parseInt(this.DonarUtilizedFund) + parseInt(listitem[i].HeadAmount);
                    }
                    //this.UnUtilizedFund = this.TotalFundedAmount - this.DonarUtilizedFund
                    this.UnUtilizedFund = this.budgetAmt - this.DonarUtilizedFund;
                    if (this.UnUtilizedFund <= 0) {
                        this.UnUtilizedFund = 0;
                    }
                }
                this.fnfillExpensechartdata();
            },
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
            async fnGetPaymentList() {
                var result = await objVenture.GetPaymentList(this.ProjectID, 0);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.PaymentList = result.data;
                    this.PaymentfilterList = result.data;
                }
            },
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
                else { this.PaymentfilterList = this.PaymentList; }
            },

            //Budget Plan
            async FetchBudgetPlan() {
                var intCount = 0;
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objFinance.FetchBudgetPlan(data);
                this.UnaccountedAmt = this.budgetAmt
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
                        this.UnaccountedAmt = this.budgetAmt - sum
                    }
                    this.fnFetchBudgetPlanitem(this.ProjectHeadTableItem);
                }
            },

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

            async fnFillBudgetPlan() {

                var labelsArray = ["Unaccounted"];
                var amountArray = [this.UnaccountedAmt];
                var labels = ["#E5E5E5"];
                for (var i = 0; i < this.BudgetPlanHeadList.length; i++) {
                    labelsArray[i + 1] = (this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].Name : '');
                    amountArray[i + 1] = (this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].TransactionAmount : '');
                    labels[i + 1] = (this.BudgetPlanHeadList[i] ? this.BudgetPlanHeadList[i].color : '');
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
<style>
    .row {
        padding-left: 0px;
        border: 0px none !important;
        margin-right: -12px !important;
        margin-left: -12px !important;
    }
</style>
<style scoped>
    .v-timeline::before {
        top: 40px;
    }
</style>