<template>
    <div>
        <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
        <v-card class="elevation-0">
            <v-card-title>
                <h5 class="page-head">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                    </v-avatar>
                    <!--If path name is ManageProjectFinanceBudget-->
                    <router-link v-show="$route.name=='ManageProjectFinanceBudget'" :to="{ name: 'ManageProjectFinance', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID,ProjID:ProjectID} }" class="page-head text-link">Inputs</router-link>
                    <!--________________________-->
                    <!--If path name is MyprojectsDraftIndividualFinanceDetailsBudget-->
                    <router-link v-show="$route.name=='MyprojectsDraftIndividualFinanceDetailsBudget'" :to="{ name: 'MyprojectsDraftIndividualFinanceDetails', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID,ProjID:ProjectID} }" class="page-head text-link">Inputs</router-link>
                    <!--________________________-->
                    <!--If path name is MyprojectsDeployedIndividualFinanceDetailsBudget-->
                    <router-link v-show="$route.name=='MyprojectsDeployedIndividualFinanceDetailsBudget'" :to="{ name: 'MyprojectsDeployedIndividualFinanceDetails', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID,ProjID:ProjectID} }" class="page-head text-link">Inputs</router-link>
                    <!--________________________-->
                    <!--If path name is FinanceBudgetfundsViewAll-->
                    <router-link v-show="$route.name=='FinanceBudgetfundsViewAll'" :to="{ name: 'FinanceBudgetAndFunds', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID,ProjID:ProjectID} }" class="page-head text-link">Funds & Utilization</router-link>
                    <!--________________________-->
                    > Budget & Funds
                </h5>
            </v-card-title>
            <v-card-text class="px-4">
                <v-row>
                    <!--total budget and donors-->
                    <v-col md="6">
                        <v-card>
                            <v-row class="px-7 py-3">
                                <div>
                                    <span class="heading-3-dark font-weight-bold">Total Budget</span>
                                    <h3 v-model="budgetAmt" v-if="!editAmount" class="font-weight-bolder title">{{ getSelectedCurrency.symbol }} {{budgetAmt}}</h3>
                                    <v-row>
                                        <v-text-field v-model="budgetAmt" v-if="editAmount" type="number" hide-details="" outlined="" dense=""
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;" min="0">
                                        </v-text-field>
                                        <span justify="center" v-show="SaveandCancelBtns" class="ml-3 mt-2">
                                            <Success-Button id="AutoTest_IP464" style="height:24px" title="Save" @click.native="fnSave()" v-bind:disabled="!screenAccess"></Success-Button>
                                            <Default-Button-Outlined id="AutoTest_IP465" class="ml-3" style="height:24px" title="Cancel" @click.native="editItemsClose(FUserType)"></Default-Button-Outlined>
                                        </span>
                                    </v-row>
                                </div>
                                <v-spacer></v-spacer>

                                <Button-Light-24x24 id="AutoTest_IP444" class="ml-2" v-show="!editAmount" icon="mdi-pencil" @click.native="editAmount=true,SaveandCancelBtns=true" v-bind:disabled="!screenAccess"></Button-Light-24x24>

                            </v-row>

                            <v-row class="px-3 pb-2">
                                <v-col sm="12">
                                    <div class="d-flex">
                                        <div v-for="(item,index) in lstBudgetDonorUserInfo" :key="item.AccountId" :style="{'background-color': fixedColor[index]||`${getcolor(item.UserName)}`,
                                                  'border':'1px',
                                                  'height':'35px',
                                                  'width': ((100*(item.DonorBudgetAmount))/budgetAmt).toFixed(2)+'%'}
                                                ">
                                        </div>
                                        <div :style="{'background-color': '#eeeeee',
                                                  'border':'1px',
                                                  'height':'35px',
                                                  'width': ((100*(notFundedAmount))/budgetAmt).toFixed(2)+'%'}
                                                ">
                                        </div>
                                    </div>
                                </v-col>
                            </v-row>

                            <card class="elevation-0">
                                <v-card-title>
                                    <span class="heading-3-dark font-weight-bold">Donors</span>
                                    <v-spacer></v-spacer>
                                    <v-menu absolute="" v-model="AddDonorPopUp" top=""  :offset-y="offset" :close-on-content-click="closeOnContentClick" v-bind:disabled="!screenAccess">
                                        <template v-slot:activator="{ on }">
                                            <v-btn id="AutoTest_IP450" v-on="on" max-height="24" max-width="100" min-width="24" min-height="24" depressed="" light="" color="#E5F2FF"
                                                   @click="GetProjectUsers('Donor')" v-bind:disabled="!enableAddDonor">
                                                Add Donor
                                            </v-btn>
                                        </template>
                                        <v-stepper class="elevation-0" v-model="AssignDonarSteps">
                                            <v-stepper-items>
                                                <v-stepper-content class="px-1 py-0" step="1">
                                                    <v-card>
                                                        <v-card-title class="heading-3">
                                                            Assign Donors
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP451" text="" height="25" min-width="25" max-width="25" color="#707070" @click.native="CloseAssignDonor()">
                                                                <v-icon size="20">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-divider></v-divider>
                                                        <v-card-text style="height:35vh;width:350px" class="v-scrolling-div">
                                                            <v-data-table :headers="AddDonorTableHeaders"
                                                                          :items="lstDonors"
                                                                          show-select
                                                                          item-key="UserID"
                                                                          sort-by="UserName"
                                                                          v-model="vmlstDonors"
                                                                          hide-default-footer
                                                                          hide-default-header>

                                                                <template v-slot:item.UserName="{ item }">
                                                                    <v-list class="py-0 table-v-list">
                                                                        <v-list-item class="my-0 px-0">
                                                                            <v-list-item-icon>
                                                                                <v-avatar size="25" light :color="`${getcolor(item.UserName)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content>
                                                                                {{ item.UserName }}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </template>
                                                                <template v-slot:no-data>
                                                                    <v-alert :value="true" class="text--white" type="error" dense>
                                                                        Sorry, nothing to display here :(
                                                                    </v-alert>
                                                                </template>
                                                            </v-data-table>
                                                        </v-card-text>
                                                        <v-divider></v-divider>
                                                        <v-card-actions>
                                                            <v-row justify="center">
                                                                <v-col md="6">
                                                                    <v-btn id="AutoTest_IP452" style="width:100%;background:#399CFF;color:white" class="mb-2 center-block" @click.native="AssignDonorStep2()" v-bind:disabled="vmlstDonors.length==0"> Next</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-stepper-content>
                                                <v-stepper-content class="px-1 py-0" step="2">
                                                    <v-card>
                                                        <v-card-title class="heading-3">
                                                            Add Amount & Link Accounts
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP453" text="" height="25" min-width="25" max-width="25" color="#707070" @click.native="CloseAssignDonor()">
                                                                <v-icon size="20">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-divider></v-divider>
                                                        <v-card-text style="height:35vh;width:350px" class="v-scrolling-div">
                                                            <v-data-table :headers="AddDonorTableHeaders"
                                                                          :items="vmlstDonors"
                                                                          :single-expand="singleExpand"
                                                                          :expanded.sync="expanded"
                                                                          item-key="UserID"
                                                                          sort-by="UserName"
                                                                          v-model="vmDonors"
                                                                          show-expand
                                                                          hide-default-header
                                                                          hide-default-footer>
                                                                <template v-slot:item.UserName="{ item }">
                                                                    <v-list class="py-0 table-v-list">
                                                                        <v-list-item class="my-0 px-0">
                                                                            <v-list-item-icon>
                                                                                <v-avatar size="25" light :color="`${getcolor(item.UserName)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content>
                                                                                {{ item.UserName }}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </template>

                                                                <template v-slot:expanded-item="{ headers, item }">
                                                                    <td id="AutoTest_IP455" :colspan="headers.length" @click="GetDonorDetails(item)" style="width:100px">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                Add Amount
                                                                                <!--<v-icon color="red" size="10" class="float-right" v-if="item.DonorBudgetAmount=='0'">mdi-star</v-icon>-->
                                                                                <v-text-field class="mt-2" dense="" type="number" full-width outlined hide-details v-model="item.DonorBudgetAmount"
                                                                                              required @change="AddBudgetAmt(item.UserID,item.DonorBudgetAmount)">
                                                                                    {{item.DonorBudgetAmount}}
                                                                                    <template v-if=" item.DonorBudgetAmount == '' ||item.DonorBudgetAmount == null"  v-slot:append="">
                                                                                        <span class="mandatory-star">*</span>
                                                                                    </template>
                                                                                </v-text-field>
                                                                                <small  style="color:red" v-if="item.DonorBudgetAmount<=0 ">
                                                                                    {{errorMsg}}
                                                                                </small>
                                                                            </v-col>
                                                                            <v-col md="12">
                                                                                Link Bank Account
                                                                                <v-select class="mt-1" hide-details="" dense="" :items="lstDonorAccount"
                                                                                          item-text="AccountName" return-object
                                                                                          item-value="AccountId" required
                                                                                          label="Select Account" outlined="" v-model="item.Account" @change="AddAccount(item.UserID,item.Account)">
                                                                                </v-select>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </td>
                                                                </template>
                                                                <template v-slot:no-data>
                                                                    <v-alert :value="true" class="text--white" type="error">
                                                                        Sorry, nothing to display here :(
                                                                    </v-alert>
                                                                </template>
                                                            </v-data-table>
                                                        </v-card-text>
                                                        <v-divider></v-divider>
                                                        <v-card-actions>
                                                            <v-row justify="center">
                                                                <v-col md="6">
                                                                    <Success-Button id="AutoTest_IP456" style="width:100%" title="Save" class="mb-2 center-block" @click.native="SaveProjectBudgetsDetails('D')"
                                                                                    v-bind:disabled="!isValidBudget">Save</Success-Button>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-stepper-content>
                                            </v-stepper-items>
                                        </v-stepper>
                                    </v-menu>
                                </v-card-title>
                                <v-card-text style="height:250px" class="v-scrolling-div">
                                    <v-row class="py-0 px-0">
                                        <v-col v-for="(item,index) in lstBudgetDonorUserInfo" :key="item.AccountId" md="6" class="py-1 px-2">
                                            <v-card class="" outlined style="width:500px;height:90px;background:#FAFDFF" tile>
                                                <v-row class="px-2">
                                                    <v-col md="3" class="py-4">
                                                        <v-avatar size="60" light :color="fixedColor[index]||`${getcolor(item.UserName)}`">
                                                            <img class="pa-1" v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                    </v-col>
                                                    <v-col md="7" class="py-2">
                                                        <small class="small-text mr-1 black--text">{{item.UserName }}</small><br />
                                                        <span class=" heading-3-dark font-weight-bolder">{{ getSelectedCurrency.symbol }} {{item.DonorBudgetAmount }}</span><br />
                                                        <small class="" v-if="item.AccountId >0">{{item.AccountName }}</small>
                                                        <small class="" v-if="item.AccountId ==0">No Accounts linked</small>
                                                    </v-col>
                                                    <v-col md="2" class="py-1">
                                                        <v-menu close-on-content-click=""
                                                                transition="scroll-y-transition"
                                                                left=""
                                                                offset-y=""
                                                                v-bind:disabled="!screenAccess">
                                                            <template v-slot:activator="{ on }">
                                                                <v-btn min-width="27"
                                                                       height="29"
                                                                       v-on="on"
                                                                       color="grey"
                                                                       outlined=""
                                                                       dark=""
                                                                       style="border:0px"
                                                                       class="elevation-0  mr-2 px-0">
                                                                    <v-icon>mdi-dots-vertical</v-icon>
                                                                </v-btn>
                                                            </template>
                                                            <v-list dense=""
                                                                    color="white"
                                                                    light=""
                                                                    width="auto"
                                                                    class="action-button-list">
                                                                <v-list-item id="" link="" @click.stop="GetEditDonorAccountDetails(item.UserID,item.AccountId),editUserDailog =true,FUserID= item.UserID,FUserName=item.UserName,editDonorAmount=item.DonorBudgetAmount,editAcountName=item.AccountName">
                                                                    <v-list-item-icon>
                                                                        <v-icon>mdi-pencil-outline</v-icon>
                                                                    </v-list-item-icon>
                                                                    <v-list-item-content>Edit</v-list-item-content>
                                                                </v-list-item>

                                                                <v-list-item id="AutoTest_IP932" link="" @click.stop="RemoveUserDailog =true,FUserID= item.UserID,FUserType=item.UserType,FUserName=item.UserName">
                                                                    <v-list-item-icon>
                                                                        <v-icon> mdi-trash-can-outline</v-icon>
                                                                    </v-list-item-icon>
                                                                    <v-list-item-content>Remove</v-list-item-content>
                                                                </v-list-item>
                                                            </v-list>
                                                        </v-menu>
                                                    </v-col>

                                                </v-row>
                                            </v-card>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </card>
                        </v-card>
                    </v-col>

                    <!--fund utilization and finance users-->
                    <v-col md="6">
                        <v-card class="mb-2">
                            <v-row class="px-7 py-3">
                                <div>
                                    <span class="heading-3-dark font-weight-bold">Fund Utilization</span>
                                    <h3 class="font-weight-bolder title">{{ getSelectedCurrency.symbol }} {{utilizedFund}}</h3>
                                </div>
                            </v-row>

                            <v-row class="px-3">
                                <v-col sm="12">
                                    <div class="d-flex">
                                        <div :style="{'background-color': '#0099ff',
                                                  'border':'1px',
                                                  'height':'35px',
                                                  'width': ((100*(utilizedFund))/budgetAmt).toFixed(2)+'%'}
                                                ">
                                        </div>
                                        <div :style="{
                                                  'background-color': '#eeeeee',
                                                  'border':'1px',
                                                  'height':'35px',
                                                  'width': ((100*(budgetAmt-utilizedFund))/budgetAmt).toFixed(2)+'%'}
                                                ">
                                        </div>
                                    </div>
                                </v-col>
                            </v-row>
                        </v-card>
                        <v-card class="elevation-2">
                            <v-card-title>
                                <span class="heading-3-dark font-weight-bold">Finance Users </span>
                                <v-spacer></v-spacer>
                                <v-menu absolute="" v-model="AssignFinUserPopUp" top="" left="" :offset-y="offset"  :close-on-content-click="closeOnContentClick" v-bind:disabled="!screenAccess">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP446" v-on="on" max-height="24" max-width="100" min-width="24" min-height="24" depressed="" light="" color="#E5F2FF"
                                               @click="GetProjectUsers('Finance')" v-bind:disabled="!enableAddFinUser">
                                            Add User
                                        </v-btn>
                                    </template>
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Assign Finance Users
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP447" text="" height="25" min-width="25" max-width="25" color="#707070" @click="CloseAssignFinUser()">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:33vh;width:350px" class="v-scrolling-div">
                                            <v-data-table :headers="lstuserTableheaders"
                                                          :items="lstFinanceUsers"
                                                          show-select
                                                          item-key="UserID"
                                                          sort-by="UserName"
                                                          v-model="vmFinanceUsers"
                                                          hide-default-footer
                                                          hide-default-header>

                                                <template v-slot:item.UserName="{ item }">
                                                    <v-list class="py-0 table-v-list">
                                                        <v-list-item class="my-0 px-0">
                                                            <v-list-item-icon>
                                                                <v-avatar size="25" light :color="`${getcolor(item.UserName)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                            </v-list-item-icon>
                                                            <v-list-item-content>
                                                                {{ item.UserName }}
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list>
                                                </template>
                                                <template v-slot:no-data>
                                                    <v-alert :value="true" class="text--white" type="error" dense>
                                                        Sorry, nothing to display here :(
                                                    </v-alert>
                                                </template>
                                            </v-data-table>

                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row justify="center">
                                                <v-col md="8" class="py-0">
                                                    <v-checkbox color="#52B962" :label="`Notify Finance Users`" v-model="ChkFinNotify"></v-checkbox>
                                                </v-col>
                                                <v-col md="4" class="py-4">
                                                    <Success-Button id="AutoTest_IP448" style="width:20px" class="mb-2 center-block" title="Save"
                                                @click.native="SaveProjectBudgetsDetails('F')" v-bind:disabled="vmFinanceUsers.length==0"></Success-Button>
                                                    <!--<v-btn id="AutoTest_IP448" @click.native="SaveProjectBudgetsDetails('F')" v-bind:disabled="vmFinanceUsers.length==0"
                                                           class="elevation-0 float-right mr-4  success-btn">Save</v-btn>-->

                                                </v-col>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                </v-menu>
                            </v-card-title>
                            <v-card-text style="height:250px" class="v-scrolling-div">
                                <v-row class="py-0 px-0">
                                    <v-col v-for="(item,index) in lstBudgetFinUserInfo" :key="item.AccountId" md="6" class="py-1 px-2">
                                        <v-card class="" outlined style="width:500px;height:90px;background:#FAFDFF" tile>
                                            <v-row class="px-2">
                                                <v-col md="3" class="py-4">
                                                    <v-avatar size="60" light :color="`${getcolor(item.UserName)}`">
                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                </v-col>
                                                <v-col md="7" class="py-7">
                                                    <span class=" heading-3-dark font-weight-bolder"> {{item.UserName }}</span><br />
                                                </v-col>
                                                <v-col md="2" class="py-1">
                                                    <v-menu close-on-content-click=""
                                                            transition="scroll-y-transition"
                                                            left=""
                                                            offset-y=""
                                                            v-bind:disabled="!screenAccess">
                                                        <template v-slot:activator="{ on }">
                                                            <v-btn min-width="27"
                                                                   height="29"
                                                                   v-on="on"
                                                                   color="grey"
                                                                   outlined=""
                                                                   dark=""
                                                                   style="border:0px"
                                                                   class="elevation-0 mr-2 px-0">
                                                                <v-icon>mdi-dots-vertical</v-icon>
                                                            </v-btn>
                                                        </template>
                                                        <v-list dense=""
                                                                color="white"
                                                                light=""
                                                                width="auto"
                                                                class="action-button-list">

                                                            <v-list-item id="AutoTest_IP932" link="" @click.stop="RemoveUserDailog =true,FUserID= item.UserID,FUserType=item.UserType,FUserName=item.UserName">
                                                                <v-list-item-icon>
                                                                    <v-icon> mdi-trash-can-outline</v-icon>
                                                                </v-list-item-icon>
                                                                <v-list-item-content>Remove</v-list-item-content>
                                                            </v-list-item>
                                                        </v-list>
                                                    </v-menu>
                                                </v-col>
                                            </v-row>
                                        </v-card>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                        </v-card>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
        <!--edit user dialog-->
        <v-dialog v-model="editUserDailog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit {{FUserName}}'s Details
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP462" text="" @click="  (editUserDailog = false) ">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text style="height:200px;">
                    Budget
                    <v-text-field outlined dense v-model="editDonorAmount"></v-text-field>
                    Account
                    <v-select dense :items="lstEDonorAccount"
                              item-text="AccountName" return-object
                              item-value="AccountId" required
                              :label="` ${editAcountName}`" outlined="" v-model="vmEDonorAccount"
                              outlined>
                    </v-select>
                </v-card-text>
                <v-card-actions class="py-0">
                    <v-row no-gutters="" class="py-0">
                        <v-col md="12" align="right" class="mb-7">
                            <Secondary-Button id="AutoTest_IP463" style="width:130px" class=" center-block" title="Confirm"
                                              @click.native="UpdateDonorAmontAndAccount(FUserID,editDonorAmount,vmEDonorAccount.AccountId)"></Secondary-Button>
                            <v-btn id="AutoTest_IP462" style="width:120px" outlined class=" mr-4" color="grey" @click=" editUserDailog = false ">Cancel</v-btn>
                        </v-col>
                    </v-row>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <!--remove user dialog-->
        <v-dialog v-model="RemoveUserDailog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Budget Plan
                    <v-spacer></v-spacer>
                    <v-btn id=""
                           text=""
                           @click="
                    (RemoveUserDailog = false)
                ">
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
                                            <h4 class="text-center heading-3">
                                                Are you sure you want to remove {{ FUserName }} ?

                                            </h4>
                                        </v-col>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <v-row justify="center">
                                                <h5 class="page-head mt-4">
                                                    <v-avatar color="white"
                                                              size="25"
                                                              class="elevation-1 mr-2 mb-1">
                                                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                                                    </v-avatar>
                                                    {{ FUserName }}
                                                </h5>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="12"
                                               xs="12"
                                               sm="12"
                                               md="12"
                                               class="text-right py-0">
                                            <v-btn id=""
                                                   color="secondary elevation-0"
                                                   class="mr-2"
                                                   @click="RemoveBudgetUserList(FUserID,FUserType,FUserName)">Remove</v-btn>
                                            <v-btn id=""
                                                   outlined
                                                   color="grey"
                                                   @click="
                              (vmEBudgetHeadID = ''),
                                (vmEBudgetHead = ''),
                                (RemoveUserDailog = false)
                            ">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
    </div>
</template>
<script>
    import ButtonLight24x24 from '@/components/button-light-24x24.vue'
    import FabButtonLightSmall from '@/components/fab-button-light-small.vue'
    import SuccessButton from '@/components/success-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import CloseBtnFabSmall from '@/components/close-btn-fab-small.vue'
    import { mapGetters } from "vuex";
    import SecondaryButton from '@/components/secondary-button.vue';
    import PreLoader from "@/components/pre-loader.vue";

    var objProject;
    async function importscript() {
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
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectID: 0,
                fixedColor: ["#399CFF", "#00C2B0", "#F1C100", "#00B448", "#2C4258", "#8307CB", "#F98B00", "#D10000", "#E6217B", "#6F00B0", "#464646", "#395FFF", "#038E81", "#C99506", "#76B400", "#2C584A", "#521CA8", "#D5D5D5", "#FF6200", "#88320C", "#CE72B9", "#A00000", "#41AFFF", "#00E209", "#D008A9", "#FFD446"],
                expanded: [],
                singleExpand: false,
                singleSelect: false,
                editAmount: false,
                editFinUser: false,
                editDonor: false,
                editUserDailog: false,
                editDonorAmount: 0,
                SaveandCancelBtns: false,
                AssignFinUserPopUp: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                budgetAmt: 0,
                utilizedFund: 0,
                notFundedAmount: 0,
                GroupID: 0,
                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],
                lstuserTableheaders: [{ text: "Name", align: "left", value: "UserName" }],
                AddDonorTableHeaders: [
                    { text: "Name", align: "left", value: "UserName" },
                    { text: '', value: 'data-table-expand' },
                ],
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
                errorMsg: " * Please add Amount",
                isValidBudget: 0,
                snackbarAlertMsg: 0,
                AddDonorPopUp: false,
                lstBudgetFinUserInfo_BforEdit: [],
                lstBudgetDonorUserInfo_BforEdit: [],
                delUserList: [],
                FUserID: 0, FUserName: "", FUserType: "", FUserImage: "", editAcountName: "",
                RemoveUserDailog: false,
                EditDonorPopUp: false,
                lstEditDonorItem: [],
                errorCount: 0,
                txtDonorBudgetAmount: 0,

                lstEDonorAccount: [],
                vmEDonorAccount: [],
                enableAddDonor: true,
                enableAddFinUser: true,
                userimage: "",
                PreLoader: false
            }
        },
        components: {
            'Button-Light-24x24': ButtonLight24x24,
            'Fab-Button-Light-Small': FabButtonLightSmall,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Success-Button': SuccessButton,
            'Close-Btn-Fab-Small': CloseBtnFabSmall,
            'Secondary-Button': SecondaryButton,
            "Pre-Loader": PreLoader
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            await importscript();
            await this.GetProjectBudgetDetails();
            await this.GetFundPaymentDeatils();
        },
        computed: {
            ...mapGetters('DefaultStore',["getSelectedCurrency"]),
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
            async GetFundPaymentDeatils() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objProject.GetFundPaymentDeatils(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        var fundsDetail = result.data;
                        if (fundsDetail.length > 0) {
                            for (var i = 0; i < fundsDetail.length; i++) {
                                this.utilizedFund =
                                    this.utilizedFund + parseInt(fundsDetail[i].HeadAmount);
                            }
                        }
                    }
                }
            },
            async GetProjectBudgetDetails() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objProject.GetProjectBudgetDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.budgetAmt = result.data[0].Budget;
                        this.notFundedAmount = this.budgetAmt;
                        this.GroupID = result.data[0].GroupId;
                        this.lstBudgetFinUserInfo = result.data.filter(x => x.UserType == 'F');
                        this.lstBudgetDonorUserInfo = result.data.filter(x => x.UserType == 'D');
                        var sum = 0;
                        if (this.lstBudgetDonorUserInfo.length > 0) {
                            for (var i = 0; i < this.lstBudgetDonorUserInfo.length; i++) {
                                sum = sum + parseInt(this.lstBudgetDonorUserInfo[i].DonorBudgetAmount);
                            }
                            this.notFundedAmount = this.budgetAmt - sum;
                        }
                    }
                }
            },
            async GetProjectUsers(Type) {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("GroupID", this.GroupID);
                data.append("Type", Type)
                var result = await objProject.GetProjectUsers(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        if (Type == 'Finance')
                            this.lstFinanceUsers = result.data;
                        else if (Type == 'Donor')
                            this.lstDonors = result.data;
                        for (var i = 0; i < this.lstDonors.length; i++)
                            this.lstDonors[i].lstDonorAccount = [];

                    }
                    if (this.lstBudgetFinUserInfo.length > 0) {
                        for (var i = 0; i < this.lstBudgetFinUserInfo.length; i++)
                            this.lstFinanceUsers = this.lstFinanceUsers.filter(x => x.UserID != this.lstBudgetFinUserInfo[i].UserID);
                    }
                    if (this.lstBudgetDonorUserInfo.length > 0) {
                        for (var i = 0; i < this.lstBudgetDonorUserInfo.length; i++)
                            this.lstDonors = this.lstDonors.filter(x => x.UserID != this.lstBudgetDonorUserInfo[i].UserID);
                    }
                }
            },
            async SaveProjectBudgetsDetails(UserType) {
                this.PreLoader = true;
                var usr = "";
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                if (UserType == 'F') {
                    data.append("ProjectUsers", JSON.stringify(this.vmFinanceUsers));
                    usr = "Finance user";
                }
                else {
                    data.append("ProjectUsers", JSON.stringify(this.vmlstDonors));
                    usr = "Donor user";
                }
                data.append("chkNotify", this.ChkFinNotify);
                var result = await objProject.SaveProjectBudgetsDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        this.Msg = usr + " added successfully";
                    else
                        this.Msg = "Failed in adding " + usr + " ...";
                }


                this.snackbarAlertMsg = true;
                this.GetProjectBudgetDetails();
                this.CloseAssignDonor();
                this.CloseAssignFinUser();

                this.PreLoader = false;

            },
            async RemoveBudgetUserList(UserID, UserType,UserName) {
                if (UserType == 'D') {
                    this.lstBudgetDonorUserInfo = this.lstBudgetDonorUserInfo.filter(x => x.UserID != UserID);
                    this.enableAddDonor = false;
                }
                else if (UserType == 'F') {
                    this.lstBudgetFinUserInfo = this.lstBudgetFinUserInfo.filter(x => x.UserID != UserID);
                    this.enableAddFinUser = false;
                }
                this.delUserList.push({ ProjectID: this.ProjectID, UserID: UserID, UserType:UserType, UserName:UserName  });
                this.RemoveBudgetUser();
                this.enableAddDonor = true;
                this.enableAddFinUser = true;
                this.RemoveUserDailog = false;

            },

            AssignDonorStep2() {

                var errCount = 0;
                for (var i = 0; i < this.vmlstDonors.length; i++)
                    if ((this.vmlstDonors[i].DonorBudgetAmount == 0))
                        errCount++;
                if (errCount > 0) {
                    this.isValidBudget = 0;
                    this.errorMsg = " * Please add Amount";
                }
                else {
                    this.isValidBudget = 1;
                    this.errorMsg = "";
                }

                this.AssignDonarSteps = 2;
            },
            async GetDonorDetails(userItem) {
                this.vmDonorAccount.AccountId = this.lstDonors.filter(x => x.UserID == userItem.UserID)[0].AccountId;
                this.vmDonorAccount.AccountName = this.lstDonors.filter(x => x.UserID == userItem.UserID)[0].AccountName;

                //--- Get Account Details ----
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("UserID", userItem.UserID);
                var result = await objProject.GetDonorAccountDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstDonorAccount = result.data;
                        this.lstDonors.filter(x => x.UserID == userItem.UserID)[0].lstDonorAccount = result.data;
                    }
                }
            },
            async GetEditDonorAccountDetails(UserID, AccountID) {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("UserID", UserID);
                var result = await objProject.GetDonorAccountDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.lstEDonorAccount = result.data;
                }
                this.vmEDonorAccount = this.lstEDonorAccount.filter(x => x.AccountId == AccountID);

            },
            AddBudgetAmt(userID, Amt) {

                this.vmlstDonors.filter(x => x.UserID == userID)[0].IsBudgetAmtAdded = true;
                this.errorMsg = "";

                var i = 0; var totDonorsAmt = 0; var existDonorAmt = 0;
                var lstUsers = this.vmlstDonors.filter(x => x.UserID != userID);

                if (this.lstBudgetDonorUserInfo.length > 0) {
                    for (i = 0; i < this.lstBudgetDonorUserInfo.length; i++)
                        existDonorAmt = existDonorAmt + parseInt(this.lstBudgetDonorUserInfo[i].DonorBudgetAmount);
                }

                for (i = 0; i < lstUsers.length; i++) {
                    if (lstUsers[i].IsBudgetAmtAdded) {
                        totDonorsAmt = totDonorsAmt + parseInt(lstUsers[i].DonorBudgetAmount);
                        if (lstUsers[i].DonorBudgetAmount > 0) {
                            lstUsers.splice(i, 1);
                            lstUsers = [...lstUsers];
                            i--;
                        }
                    }
                }

                var sumBudgetAmt = parseInt(existDonorAmt) + parseInt(totDonorsAmt) + parseInt(Amt);

                if (parseInt(this.budgetAmt) > sumBudgetAmt) {
                    var balAmt = (parseInt(this.budgetAmt) - parseInt(sumBudgetAmt)) / lstUsers.length;
                    for (i = 0; i < lstUsers.length; i++)
                        this.vmlstDonors.filter(x => x.UserID == lstUsers[i].UserID)[0].DonorBudgetAmount = balAmt;
                    this.isValidBudget = 1;
                }
                else {
                    this.isValidBudget = 0;
                    this.errorMsg = "Total Amount should not be greater than " + this.budgetAmt;
                    this.vmlstDonors.filter(x => x.UserID == userID)[0].DonorBudgetAmount = 0;
                }

            },
            AddAccount(userID, Account) {
                this.lstDonors.filter(x => x.UserID == userID)[0].AccountId = Account.AccountId; //this.vmDonorAccount.AccountId;
                this.lstDonors.filter(x => x.UserID == userID)[0].AccountName = Account.AccountName;//this.vmDonorAccount.AccountName;

            },

            async fnSave() {
                if (this.editAmount) {
                    this.UpdateBudgetAmont();
                    this.editAmount = false;
                }
                //else if (this.editFinUser || this.editDonor) {
                //    this.RemoveBudgetUser();
                //    this.editFinUser = false;
                //    this.editDonor = false;
                //}
                this.SaveandCancelBtns = false;
                this.enableAddDonor = true;
                this.enableAddFinUser = true;

            },
            async UpdateBudgetAmont() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("TotalBudgetAmt", this.budgetAmt);
                var result = await objProject.UpdateProjectBudget(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        this.Msg = "Project Total Budget Amount updated ... ";
                    else
                        this.Msg = "Failed in updating data ...";
                }
                this.GetProjectBudgetDetails();
            },
            async UpdateDonorAmontAndAccount(UserID, DonorBudgetAmount, AccountID) {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("UserID", UserID);
                data.append("DonorBudgetAmount", DonorBudgetAmount);
                data.append("AccountID", AccountID);

                var result = await objProject.UpdateDonorAmontAndAccount(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        this.Msg = "Donor Budget details updated ... ";
                    else
                        this.Msg = "Failed in updating data ...";
                }
                this.editUserDailog = false
                this.GetProjectBudgetDetails();

            },
            async RemoveBudgetUser() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("ProjectUsers", JSON.stringify(this.delUserList));
                var result = await objProject.RemoveProjectBudgetUser(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        this.Msg = "User removed from the project finance... ";
                    else
                        this.Msg = "Failed in removing user ...";
                }
                this.delUserList = [];
                this.GetProjectBudgetDetails();
            },
            editItemsClose(UserType) {
                this.delUserList = [];
                this.GetProjectBudgetDetails();
                if (UserType == 'F')
                    this.lstBudgetFinUserInfo = this.lstBudgetFinUserInfo_BforEdit;
                else if (UserType == 'D')
                    this.lstBudgetDonorUserInfo = this.lstBudgetDonorUserInfo_BforEdit;
                this.editAmount = false;
                this.editFinUser = false;
                this.SaveandCancelBtns = false;
                this.editDonor = false;
                this.enableAddDonor = true;
                this.enableAddFinUser = true;

            },
            CloseAssignDonor() {

                this.AddDonorPopUp = false;
                this.vmlstDonors = [];
                this.vmDonors = [];
                this.AssignDonarSteps = 1;
            },
            CloseAssignFinUser() {

                this.AssignFinUserPopUp = false;
                this.vmFinanceUsers = [];
            },

        },
    }
</script>