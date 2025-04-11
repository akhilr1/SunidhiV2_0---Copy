<template>
  <div>
    <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
    <v-row>
      <!--Budget And Fund-->
      <v-col md="6">
        <v-card class="custom-card-1">
          <v-card-title style="height:60px">
            {{ BudgetAndFundHeader }}
            <v-spacer></v-spacer>
            <!--If path is ManageProjectFinance-->
            <router-link
              v-show="
                BudgetAndFinanceSteps == 1 &&
                  $route.name == 'ManageProjectFinance'
              "
              :to="{
                name: 'ManageProjectFinanceBudget',
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
            <!--______________________-->
            <!--If path is MyprojectsDraftIndividualFinanceDetails-->
            <router-link
              v-show="
                BudgetAndFinanceSteps == 1 &&
                  $route.name == 'MyprojectsDraftIndividualFinanceDetails'
              "
              :to="{
                name: 'MyprojectsDraftIndividualFinanceDetailsBudget',
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
            <!--______________________-->
            <!--If path is MyprojectsDiployedIndividualFinanceDetails-->
            <router-link
              v-show="
                BudgetAndFinanceSteps == 1 &&
                  ($route.name ==
                    'MyprojectsDiployedIndividualFinanceDetails' ||
                    $route.name == 'MyprojectsDeployedIndividualFinanceDetails')
              "
              :to="{
                name: 'MyprojectsDeployedIndividualFinanceDetailsBudget',
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
            <!--______________________-->
            <h4 class="text-center black--text heading-3">
              <!-- Previous Button-->
              <v-btn
                id="AutoTest_IP907"
                style="height:34px; width:20px;border-radius:50%"
                x-small=""
                class="mr-2 elevation-1"
                color="#fff"
                @click="fnNextChart('Minus')"
                v-bind:disabled="BudgetAndFinanceSteps == 1"
              >
                <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
              </v-btn>
              <!-- Next Button-->
              <v-btn
                id="AutoTest_IP908"
                style="height:34px; width:20px;border-radius:50%"
                x-small=""
                class="ml-2 elevation-1"
                color="#fff"
                @click="fnNextChart('Plus')"
                v-bind:disabled="BudgetAndFinanceSteps == 3"
              >
                <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
              </v-btn>
            </h4>
          </v-card-title>
          <v-card-text style="height:300px" class="v-scrolling-div pa-0">
            <v-stepper v-model="BudgetAndFinanceSteps" class="elevation-0">
              <v-stepper-items>
                <!-- Add Donor and Finance User-->
                <v-stepper-content step="1" class="pa-0">
                  <v-row>
                    <v-col md="6">
                      <v-row>
                        <v-col>
                          <div
                            style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;"
                          >
                            <canvas
                              id="myBudgetAndFundChart"
                              style="width:175px; height:150px"
                            ></canvas>
                          </div>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <h5 class="heading-3-dark text-center">
                            Total budget<br />
                            <span class="bold">{{ getSelectedCurrency.symbol }}{{ budgetAmt }}</span>
                          </h5>
                        </v-col>
                      </v-row>
                    </v-col>
                    <v-col md="6">
                      <v-menu
                        absolute
                        v-model="AssignFinUserPopUp"
                        top=""
                        :offset-y="offset"
                        :close-on-click="closeonClick"
                        :close-on-content-click="closeOnContentClick"
                        v-bind:disabled="!screenAccess"
                      >
                        <template v-slot:activator="{ on }">
                          <v-btn
                            id="AutoTest_IP909"
                            v-on="on"
                            max-width="170px"
                            min-width="170px"
                            class="mt-4 success-btn btn-sm"
                            depressed=""
                            dark=""
                            height="30"
                            @click="GetProjectUsers('Finance')"
                            v-if="lstBudgetFinUserInfo.length == 0"
                          >
                            Add Finance Users
                          </v-btn>
                        </template>
                        <!--Assign finance user pop up-->
                        <v-card>
                          <v-card-title class="heading-3">
                            Assign Finance Users
                            <v-spacer></v-spacer>
                            <v-btn
                              id="AutoTest_IP910"
                              text=""
                              height="25"
                              min-width="25"
                              max-width="25"
                              color="#707070"
                              @click="
                                (vmFinanceUsers = []),
                                  (AssignFinUserPopUp = false)
                              "
                            >
                              <v-icon size="20">mdi-close</v-icon>
                            </v-btn>
                          </v-card-title>
                          <v-divider></v-divider>
                          <v-card-text
                            style="height:100px"
                            class="v-scrolling-div"
                          >
                            <v-data-table
                              :headers="lstuserTableheaders"
                              :items="lstFinanceUsers"
                              show-select
                              item-key="UserID"
                              sort-by="UserName"
                              v-model="vmFinanceUsers"
                              hide-default-footer
                              hide-default-header
                            >
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
                                <v-alert
                                  :value="true"
                                  class="text--white"
                                  type="error"
                                  dense
                                >
                                  Sorry, nothing to display here.
                                </v-alert>
                              </template>
                            </v-data-table>
                          </v-card-text>
                          <v-divider></v-divider>
                          <v-card-actions>
                            <v-row justify="center">
                              <v-col md="9" class="py-0">
                                <v-checkbox
                                  color="#52B962"
                                  :label="`Notify Finance Users`"
                                  v-model="ChkFinNotify"
                                ></v-checkbox>
                              </v-col>
                              <v-col md="3" class="py-4">
                                <!--<Success-Button style="width:130px" class="mb-2 center-block" title="Save"
                                                        @click.native="SaveProjectBudgetsDetails('F')" v-bind:disabled="vmFinanceUsers.length==0"></Success-Button>-->
                                <v-btn
                                  id="AutoTest_IP911"
                                  @click.native="SaveProjectBudgetsDetails('F')"
                                  v-bind:disabled="vmFinanceUsers.length == 0"
                                >
                                  Save</v-btn
                                >
                              </v-col>
                            </v-row>
                          </v-card-actions>
                        </v-card>
                        <!--_______________________________________-->
                      </v-menu>
                      <!--<v-snackbar v-model="AddFinUserSuccessSnackbar" top="" color="primary">
                                        {{Msg}}
                                    </v-snackbar>-->
                      <!--Added Fin User Details-->

                      <div class="mt-1" v-if="lstBudgetFinUserInfo.length > 0">
                        <span class="small-text mr-1 black--text">
                          <v-icon size="12" color="#000">mdi-account</v-icon>
                          Finance User
                        </span>
                        <span>
                            <v-avatar size="25" class="team-avatar" v-for="(item,key) in lstBudgetFinUserInfo.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                            </v-avatar>
                            <v-avatar v-if="lstBudgetFinUserInfo.length>=4" size="25" class="team-count">
                                +{{lstBudgetFinUserInfo.length-3}}
                            </v-avatar>
                        </span>
                        <!--<v-avatar
                          v-if="lstBudgetFinUserInfo.length >= 1"
                          size="20"
                          class="team-avatar"
                        >
                          <img
                            src="https://cdn.vuetifyjs.com/images/john.jpg"
                            class="team-image"
                            alt="Finuser name"
                          />
                        </v-avatar>
                        <v-avatar
                          v-if="lstBudgetFinUserInfo.length >= 2"
                          size="20"
                          class="team-avatar"
                          color="pink"
                        >
                          <span class="white--text headline">{{
                            lstBudgetFinUserInfo[0].UserName.charAt(0)
                          }}</span>
                        </v-avatar>
                        <v-avatar
                          v-if="lstBudgetFinUserInfo.length >= 3"
                          size="20"
                        >
                          <span> +{{ lstBudgetFinUserInfo.length - 2 }}</span>
                        </v-avatar>-->
                      </div>

                      <!--____________________________-->
                      <!--Add Donors Pop up-->

                      <v-menu
                        absolute=""
                        v-model="AddDonorPopUp"
                        top=""
                        :offset-y="offset"
                        :close-on-click="closeonClick"
                        :close-on-content-click="closeOnContentClick"
                        v-bind:disabled="!screenAccess"
                      >
                        <template v-slot:activator="{ on }">
                          <v-btn
                            id="AutoTest_IP912"
                            v-on="on"
                            max-width="170px"
                            min-width="170px"
                            class="mt-4 success-btn btn-sm"
                            depressed=""
                            dark=""
                            height="30"
                            @click="GetProjectUsers('Donor')"
                            v-if="lstBudgetDonorUserInfo.length == 0"
                          >
                            Add Donors
                          </v-btn>
                        </template>
                        <v-stepper
                          class="elevation-0"
                          v-model="AssignDonarSteps"
                        >
                          <v-stepper-items>
                            <v-stepper-content class="px-1 py-0" step="1">
                              <v-card>
                                <v-card-title class="heading-3">
                                  Assign Donors
                                  <v-spacer></v-spacer>
                                  <v-btn
                                    id="AutoTest_IP913"
                                    text=""
                                    height="25"
                                    min-width="25"
                                    max-width="25"
                                    color="#707070"
                                    @click="
                                      (vmlstDonors = []),
                                        (AddDonorPopUp = false)
                                    "
                                  >
                                    <v-icon size="20">mdi-close</v-icon>
                                  </v-btn>
                                </v-card-title>
                                <v-divider></v-divider>
                                <v-card-text
                                  style="max-height:40vh"
                                  class="v-scrolling-div"
                                >
                                  <v-data-table
                                    :headers="AddDonorTableHeaders"
                                    :items="lstDonors"
                                    show-select
                                    item-key="UserID"
                                    sort-by="UserName"
                                    v-model="vmlstDonors"
                                    hide-default-footer
                                    hide-default-header
                                  >
                                    <template v-slot:item.UserName="{ item }">
                                        <v-avatar size="25" light :color="`${getcolor(item.UserName)}`">
                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                      {{ item.UserName }}
                                    </template>
                                    <template v-slot:no-data>
                                      <v-alert
                                        :value="true"
                                        class="text--white"
                                        type="error"
                                        dense
                                      >
                                        Sorry, nothing to display here
                                      </v-alert>
                                    </template>
                                  </v-data-table>
                                </v-card-text>
                                <v-divider></v-divider>
                                <v-card-actions>
                                  <v-row justify="center">
                                    <v-col md="6">
                                      <!--<Success-Button style="width:100%" class="mb-2 center-block" title="Next" @click.native="AssignDonarSteps=2"></Success-Button>-->
                                      <v-btn
                                        id="AutoTest_IP914"
                                        style="width:100%"
                                        class="mb-2 center-block"
                                        @click.native="AssignDonorStep2()"
                                        v-bind:disabled="
                                          vmlstDonors.length == 0
                                        "
                                      >
                                        Next</v-btn
                                      >
                                    </v-col>
                                  </v-row>
                                </v-card-actions>
                              </v-card>
                            </v-stepper-content>
                            <v-stepper-content class="px-1 py-0" step="2">
                              <v-card>
                                <v-card-title class="heading-3">
                                  Add Budget & Link Accounts
                                  <v-spacer></v-spacer>
                                  <v-btn
                                    id="AutoTest_IP915"
                                    text=""
                                    height="25"
                                    min-width="25"
                                    max-width="25"
                                    color="#707070"
                                    @click="
                                      (vmlstDonors = []),
                                        (vmDonors = []),
                                        (AddDonorPopUp = false),
                                        (AssignDonarSteps = 1)
                                    "
                                  >
                                    <v-icon size="20">mdi-close</v-icon>
                                  </v-btn>
                                </v-card-title>
                                <v-divider></v-divider>
                                <v-card-text
                                  style="max-height:40vh"
                                  class="v-scrolling-div"
                                >
                                  <v-data-table
                                    :headers="AddDonorTableHeaders"
                                    :items="vmlstDonors"
                                    :single-expand="singleExpand"
                                    :expanded.sync="expanded"
                                    item-key="UserID"
                                    sort-by="UserName"
                                    v-model="vmDonors"
                                    show-expand
                                    hide-default-header
                                    hide-default-footer
                                  >
                                    <template v-slot:item.UserName="{ item }">
                                      <v-avatar
                                        size="27"
                                        left
                                        class="my-0 px-0"
                                      >
                                        <v-icon size="20">mdi-account</v-icon>
                                      </v-avatar>
                                      {{ item.UserName }}
                                    </template>

                                    <template
                                      v-slot:expanded-item="{ headers, item }"
                                    >
                                      <td
                                        id="AutoTest_IP916"
                                        :colspan="headers.length"
                                        @click="GetDonorDetails(item)"
                                      >
                                        <v-row>
                                          <v-col md="12">
                                            Add budget
                                            <v-icon
                                              color="red"
                                              size="10"
                                              class="float-right"
                                              v-if="
                                                item.DonorBudgetAmount == '0'
                                              "
                                              >mdi-star</v-icon
                                            >
                                            <v-text-field
                                              class="mt-2"
                                              dense=""
                                              type="number"
                                              full-width
                                              outlined
                                              hide-details
                                              v-model="item.DonorBudgetAmount"
                                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                              onKeyPress="if(this.value.length==8) return false;"
                                              min="0"
                                              required
                                              @change="
                                                AddBudgetAmt(
                                                  item.UserID,
                                                  item.DonorBudgetAmount
                                                )
                                              "
                                            >
                                              {{ item.DonorBudgetAmount }}
                                            </v-text-field>
                                            <v-chip
                                              label
                                              small
                                              outlined
                                              dark
                                              color="red"
                                              v-if="
                                                item.DonorBudgetAmount == '0' ||
                                                  errorCount > 1
                                              "
                                            >
                                              {{ errorMsg }}
                                            </v-chip>
                                          </v-col>
                                          <v-col md="12">
                                            Link Bank Account
                                            <v-select
                                              class="mt-1"
                                              hide-details=""
                                              dense=""
                                              :items="item.lstDonorAccount"
                                              item-text="AccountName"
                                              return-object
                                              item-value="AccountId"
                                              required
                                              label="Select Account"
                                              outlined=""
                                              v-model="item.Account"
                                              @change="
                                                AddAccount(
                                                  item.UserID,
                                                  item.Account
                                                )
                                              "
                                            >
                                            </v-select>
                                          </v-col>
                                        </v-row>
                                      </td>
                                    </template>
                                    <template v-slot:no-data>
                                      <v-alert
                                        :value="true"
                                        class="text--white"
                                        type="error"
                                        dense
                                      >
                                        Sorry, nothing to display here 
                                      </v-alert>
                                    </template>
                                  </v-data-table>
                                </v-card-text>
                                <v-divider></v-divider>
                                <v-card-actions>
                                  <v-row justify="center">
                                    <v-col md="6">
                                      <!--<Success-Button style="width:100%" class="mb-2 center-block" title="Save" @click.native="SaveProjectBudgetsDetails('D')"
                                                                    v-bind:disabled="vmDonors.length==0"></Success-Button>-->
                                      <v-btn
                                        id="AutoTest_IP917"
                                        style="width:100%"
                                        class="mb-2 center-block"
                                        @click.native="
                                          SaveProjectBudgetsDetails('D')
                                        "
                                        v-bind:disabled="!isValidBudget"
                                        >Save</v-btn
                                      >
                                    </v-col>
                                  </v-row>
                                </v-card-actions>
                              </v-card>
                            </v-stepper-content>
                          </v-stepper-items>
                        </v-stepper>
                      </v-menu>

                      <!--Added Donor Details-->
                      <!--<div class="mt-1" v-if="lstBudgetDonorUserInfo.length>0">
                                        <span class="small-text mr-1 black--text">
                                            <v-icon size="12" color="#000">mdi-account</v-icon> Donors
                                        </span>
                                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=1 " size="20" class="team-avatar">
                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Donor name" />
                                        </v-avatar>
                                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=2 " size="20" class="team-avatar" color="pink">
                                            <span class="white--text headline">{{ lstBudgetDonorUserInfo[0].UserName.charAt(0) }}</span>
                                        </v-avatar>
                                        <v-avatar v-if="lstBudgetDonorUserInfo.length>=3 " size="20">
                                            <span> +{{ lstBudgetDonorUserInfo.length-2 }}</span>
                                        </v-avatar>
                                    </div>-->
                      <v-row
                        class="mt-1 mr-1 v-scrolling-div"
                        style="max-height:180px"
                      >
                        <v-col>
                          <template
                            v-for="(item, index) in lstBudgetDonorUserInfo"
                          >
                            <h4 class="text-left mb-2 mt-1">
                              <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                              <v-avatar
                                size="15"
                                class="team-avatar mr-2"
                                :color="item.color"
                                tile
                              ></v-avatar>
                              <v-avatar size="27" class="team-avatar mr-2" :color="`${getcolor(item.UserName)}`">
                                  <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                  <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                              </v-avatar>
                              {{ item.UserName }}
                            </h4>
                            <h2 class="text-left heading-3 mb-2 mt-1">
                                Amount funded : {{ getSelectedCurrency.symbol }} {{ item.DonorBudgetAmount }}
                            </h2>
                          </template>
                          <template>
                              <h4 class="text-left heading-3-dark mb-2">
                                  <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                                  <v-avatar size="15" class="team-avatar" color="#E5E5E5" tile></v-avatar>
                                  Not Funded Amount: {{ getSelectedCurrency.symbol }} {{DonorNotFundedFund}}
                              </h4>
                          </template>
                        </v-col>
                      </v-row>
                      <!--____________________________-->

                      
                    </v-col>
                  </v-row>
                </v-stepper-content>
                <!-- Fund Utilization-->
                <v-stepper-content step="2" class="pa-0">
                    <!--For Utilization when data exist chart-->
                    <v-row v-show="lstFundPaymentDeatils.length > 0">
                        <v-col md="6">
                            <v-row>
                                <v-col>
                                    <div style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;">
                                        <div style="width: 100%; height: 40px; position: absolute; top: 43%; left: 0; margin-top: -10px; line-height:19px; text-align: center;">
                                            <!--<span class="heading-3-dark">{{((100*(budgetAmt-UnUtilizedAmt))/budgetAmt).toFixed(2)}}%</span><br/><small style="color:black">Utilized Fund</small>-->
                                        </div>
                                        <canvas id="fundUtilizationChart"
                                                style="width:175px; height:150px"></canvas>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <h5 class="heading-3-dark text-center">
                                        Total budget<br />
                                        <span class="bold">{{ getSelectedCurrency.symbol }} {{ budgetAmt }}</span>
                                    </h5>
                                </v-col>
                            </v-row>
                        </v-col>
                        <v-col md="6">
                            <h4 class="text-left mt-1 px-0 heading-3-dark">Legend Title</h4>
                            <v-row class="mt-1 mr-2 ml-1 px-0  v-scrolling-div"
                                   style="max-height:190px">
                                <v-col class="px-0 py-1">
                                    <template v-for="(item, ind) in lstFundPaymentDeatils">
                                        <h4 class="text-left heading-3 mb-2 mt-1">
                                            <v-avatar size="15"
                                                      class="team-avatar"
                                                      :color="item.color"
                                                      tile></v-avatar>
                                            {{ item.BudgetHead }}: {{ getSelectedCurrency.symbol }} {{ item.HeadAmount }}
                                        </h4>
                                    </template>
                                    <template>
                                        <h4 class="text-left heading-3 mb-2 mt-1">
                                            <v-avatar size="15"
                                                      class="team-avatar"
                                                      color="#E5E5E5"
                                                      tile></v-avatar>
                                            Unutilized Fund: {{ getSelectedCurrency.symbol }} {{ UnUtilizedAmt }}
                                        </h4>
                                    </template>
                                </v-col>
                            </v-row>

                        </v-col>
                    </v-row>
                    <!--For Utilization when no data exist chart-->
                    <v-row v-show="lstFundPaymentDeatils.length == 0" justify="center">
                        <v-col class="text-center">
                            <p class="mt-6 pt-6">
                                <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                            </p>
                            <p class="heading-4-dark font-weight-medium pt-0">No fund utilizations available</p>
                        </v-col>
                    </v-row>
                    <!--For Utilization legend table-->
                    <v-row v-show="lstFundPaymentDeatils.length > 0">
                        <v-col cols="12" md="12" sm="12" xs="12" class="px-0 py-0">
                            <v-card class="custom-card-1 elevation-0">
                                <v-card-title style="height:50px" class="py-0 pl-7">
                                    <h5 class="text-left page-head">
                                        Project Expenses
                                    </h5>
                                    <v-spacer></v-spacer>
                                    <v-select v-model="vmFUBudgetHead"
                                              style="max-width:180px;"
                                              class="mr-2"
                                              hide-selected=""
                                              hide-details=""
                                              dense=""
                                              :items="lstFundPaymentDeatils"
                                              item-text="BudgetHead"
                                              item-value="BudgetHeadID"
                                              :menu-props="{ maxHeight: '200' }"
                                              outlined=""
                                              @change="GetFundUtilisationList()"
                                              :label="` ${IniBudgetHead}`"
                                              return-object>
                                    </v-select>
                                </v-card-title>
                                <v-card-text style="height:100%" class="pa-0">
                                    <v-data-table :headers="FundExpensesListHeader"
                                                  :items="lstFundExpenses"
                                                  class="elevation-0"
                                                  hide-default-footer
                                                  item-key="PaymentId"
                                                  fixed-header
                                                  height="25vh">
                                        <template v-slot:item.Title="{ item }">
                                            <v-list class="py-0 table-v-list" style="max-width:100px">
                                                <div class="px-0 pl-1 py-0 text-truncate" style="width:120px">
                                                    <v-avatar size="20"
                                                              color="#fff"
                                                              class="elevation-1">
                                                        <v-icon size="12" color="primary">mdi-book</v-icon>
                                                    </v-avatar>
                                                    {{ item.Title }}
                                                </div>
                                            </v-list>
                                        </template>
                                        <template v-slot:item.TransactionID="{ item }">
                                            <div v-if="item.TransactionID" class="pa-0 text-limit" style="max-width:100px">
                                                {{ item.TransactionID}}
                                            </div>
                                        </template>
                                        <template v-slot:item.TransactionDate="{ item }">
                                            <div v-if="item.TransactionDate" class="pa-0 text-limit" style="max-width:100px">
                                                {{ formatDate(item.TransactionDate) }}
                                            </div>
                                        </template>
                                        <template v-slot:item.TransactionAmount="{ item }">
                                            <div v-if="item.TransactionAmount" class="pa-0 text-limit" style="max-width:100px">
                                                {{ getSelectedCurrency.symbol }} {{ item.TransactionAmount }}
                                            </div>
                                        </template>
                                        <template v-slot:item.TransactionDocName="{ item }">
                                            <div v-if="item.TransactionDocName" class="pa-0 text-limit handPointer" style="max-width:120px" @click="DownloadDocument(item.TransactionDocName,'/Documents/22bd6a00-dc48-49f8-8b52-49e875dd0d52.pdf')">
                                                <v-avatar color="#388E3C"
                                                          tile=""
                                                          size="13"
                                                          class="ml-1">
                                                    <v-icon size="10" color="#fff">mdi-download</v-icon>
                                                </v-avatar> {{ item.TransactionDocName }}
                                            </div>
                                        </template>
                                    </v-data-table>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>

                </v-stepper-content>
                <!-- Bar chart-->
                <v-stepper-content step="3" class="pa-0 ma-0 px-0 py-0">
                  <v-row class="py-0 px-0">
                    <v-col md="12" class="py-0">
                      <v-row class="py-0">
                        <v-col class="text-left py-0 pl-3 pt-1">
                          <canvas
                            id="myBarChart"
                            style="width:580px; height:300px"
                          ></canvas>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <h5 class="heading-3-dark text-center">
                            Total budget<br />
                            <span class="bold">{{ getSelectedCurrency.symbol }} {{ budgetAmt }}</span>
                          </h5>
                        </v-col>
                      </v-row>
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
              {{ TitleText }}
              <v-spacer></v-spacer>
              <v-btn id="AutoTest_IP918" text="" @click="dlgAddDonor = false">
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
                            {{ Subheadtext }}
                          </h4>
                        </v-col>
                        <v-row :justify="justifyCenter">
                          <v-col md="6">
                            <v-select
                              dense=""
                              :items="lstDonorItems"
                              :item-text="(i) => i.Name"
                              :item-value="(i) => i.ID"
                              label="Select Donor"
                              outlined=""
                              v-model="selectedDonor"
                              @change="GetDonorDetails(selectedDonor)"
                            >
                            </v-select>
                            <v-select
                              dense=""
                              :items="lstAccountItems"
                              :item-text="(i) => i.AccountName"
                              :item-value="(i) => i.AccountId"
                              label="Select Account"
                              outlined=""
                              v-model="selectedAccount"
                            >
                            </v-select>

                            <v-text-field
                              label="Enter Amount"
                              type="number"
                              dense
                              v-model="Amount"
                              outlined
                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                              onKeyPress="if(this.value.length==8) return false;"
                              min="0"
                            ></v-text-field>
                          </v-col>
                        </v-row>
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right py-0"
                        >
                          <v-btn
                            id="AutoTest_IP919"
                            color="secondary elevation-0"
                            class="mr-2"
                            v-bind:disabled="
                              selectedDonor.length <= 0 || Amount <= 0
                            "
                            @click="SaveDonarFund(), (dlgAddDonor = false)"
                            >Confirm</v-btn
                          >
                          <v-btn
                            id="AutoTest_IP920"
                            outlined
                            color="grey"
                            @click="dlgAddDonor = false"
                            >Cancel</v-btn
                          >
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
              <v-btn
                id="AutoTest_IP921"
                text=""
                @click="RemoveUserDailog = false"
              >
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
                            Are you sure you want to remove this user ?
                          </h4>
                        </v-col>
                        <v-col>
                          <h4 class="text-center heading-3">
                            <!--<v-icon class="tmr-2" color="#707070" size="25">mdi-account</v-icon>-->
                            <v-avatar size="26" class="team-avatar">
                              <img
                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                class="team-image"
                                alt="Finuser name"
                              />
                            </v-avatar>
                            {{ DUserName }}
                          </h4>
                        </v-col>

                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right py-0"
                        >
                          <v-btn
                            id="AutoTest_IP922"
                            color="secondary elevation-0"
                            class="mr-2"
                            @click="RemoveBudgetUser(BudgetFundID)"
                            >Remove</v-btn
                          >
                          <v-btn
                            id="AutoTest_IP923"
                            outlined
                            color="grey"
                            @click="RemoveUserDailog = false"
                            >Cancel</v-btn
                          >
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
            {{ TitleTextBudget }}
            <v-spacer></v-spacer>
            <v-btn
              id="AutoTest_IP924"
              height="38"
              color="success-btn"
              dark
              class="elevation-0 mr-4"
              @click="dlgCreateBudgetPlan = true"
              v-show="BudgetPlanSteps == 1"
              v-bind:disabled="!screenAccess"
            >
              Add Budget Plan
            </v-btn>
            <template
              v-if="PaymentTermsitem.length > 0 && BudgetPlanSteps == 2"
            >
              <Success-Button
                id="AutoTest_IP925"
                class="mr-4"
                title="Add"
                @click.native="CreatePaymentTermsDialog = true"
                v-bind:disabled="!screenAccess"
              ></Success-Button>
              <!--if path is ManageProjectFinance-->
              <router-link
                v-show="$route.name == 'ManageProjectFinance'"
                :to="{
                  name: 'ManageProjectFinancePaymentTerms',
                  query: {
                    WsID: WorkspaceID,
                    Access: RWAccess,
                    InitID: InitiativeID,
                    ProjID: ProjectID,
                  },
                }"
              >
                <Primary-Button-Outlined
                  style="width:106px"
                  title="View Terms"
                  class="mr-4"
                ></Primary-Button-Outlined>
              </router-link>
              <!--________________-->
              <!--if path is MyprojectsDraftIndividualFinanceDetails-->
              <router-link
                v-show="
                  $route.name == 'MyprojectsDraftIndividualFinanceDetails'
                "
                :to="{
                  name: 'MyprojectsDraftIndividualFinancePaymentTerms',
                  query: {
                    WsID: WorkspaceID,
                    Access: RWAccess,
                    InitID: InitiativeID,
                    ProjID: ProjectID,
                  },
                }"
              >
                <Primary-Button-Outlined
                  style="width:106px"
                  title="View Terms"
                  class="mr-4"
                ></Primary-Button-Outlined>
              </router-link>
              <!--________________-->
            </template>
            <h4 class="text-center black--text heading-3">
              <!-- Previous Button-->
              <v-btn
                id="AutoTest_IP926"
                style="height:34px; width:20px;border-radius:50%"
                x-small
                class="mr-2 elevation-1 pa-0 ma-0"
                color="#fff"
                @click="fnNextChartBudgetPlan('Minus')"
                v-bind:disabled="BudgetPlanSteps == 1"
              >
                <v-icon size="15"  class="pa-0 ma-0" color="primary">mdi-arrow-left</v-icon>
              </v-btn>
              <!-- Next Button-->
              <v-btn
                id="AutoTest_IP927"
                style="height:34px; width:20px;border-radius:50%"
                x-small=""
                class="ml-2 elevation-1"
                color="#fff"
                @click="fnNextChartBudgetPlan('Plus')"
                v-bind:disabled="BudgetPlanSteps == 3"
              >
                <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
              </v-btn>
            </h4>
          </v-card-title>
          <v-card-text style="height:300px" class="v-scrolling-div pa-0">
            <v-stepper v-model="BudgetPlanSteps" class="elevation-0">
              <v-stepper-items>
                <!-- BudgetPlan -->
                <v-stepper-content step="1" class="pa-0">
                  <!-- if data exists-->
                  <v-row v-show="lstBudgetHeaderDetails.length > 0">
                    <v-col md="6">
                      <v-row>
                        <v-col>
                          <div
                            style="width: 175px; height: 160px; margin-left:auto;margin-right:auto; position: relative;"
                          >
                              <!--<div style="width: 100%; height: 40px; position: absolute; top: 50%; left: 0; margin-top: -10px; line-height:19px; text-align: center;">
                                  {{((100*(budgetAmt-UnaccountedAmt))/budgetAmt).toFixed(2)}}%
                              </div>-->
                            <canvas
                              id="myBudgetPlan"
                              style="width:175px; height:150px"
                            ></canvas>
                          </div>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <h5 class="heading-3-dark text-center">
                            Total budget<br />
                            <span class="bold"
                              >{{ getSelectedCurrency.symbol }}
                              {{ budgetAmt }}</span
                            >
                          </h5>
                        </v-col>
                      </v-row>
                    </v-col>
                    <v-col md="6">
                        <h4 class="text-left mt-1 px-0 heading-3-dark">Legend Title</h4>
                        <v-row class="mt-0 mr-2 ml-1 px-0 v-scrolling-div"
                               style="height:190px">
                            <v-col class="px-0 py-1">
                                <template v-for="(item, ind) in lstBudgetPlanDetails">
                                    <h4 class="text-left heading-3 mb-2 mt-1">
                                        <v-avatar size="15"
                                                  class="team-avatar"
                                                  :color="item.color"
                                                  tile></v-avatar>
                                        {{ item.BudgetHead }}:
                                        {{ getSelectedCurrency.symbol }}
                                        {{ item.HeadAmount }} <!--<small>({{((100*(item.HeadAmount))/budgetAmt).toFixed(2)}}%)</small>-->
                                    </h4>
                                </template>
                                <template >
                                    <h4 class="text-left heading-3 mb-2 mt-1">
                                        <v-avatar size="15"
                                                  class="team-avatar"
                                                  color="#E5E5E5"
                                                  tile></v-avatar>
                                        Unaccounted:
                                        {{ getSelectedCurrency.symbol }}
                                        {{ UnaccountedAmt }} <!--<small>({{((100*(UnaccountedAmt))/budgetAmt).toFixed(2)}}%)</small>-->
                                    </h4>
                                </template>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="12" sm="12" xs="12" class="pa-0">
                      <v-card class="custom-card-1 elevation-0 pa-0">
                        <v-card-title class="py-1 px-3">
                          <h4 class="text-left ml-4 font-weight-medium">
                            Budget Head
                          </h4>
                        </v-card-title>
                        <v-card-text style="height:100%" class="pa-0">
                          <v-col class="pa-0">
                            <tree-table
                              class="table tree-table"
                              :columns="ProjectHeadTableHeader"
                              :table-data="lstBudgetHeaderDetails"
                            >
                              <template
                                #headerTemplate="headerProps"
                                class="pa-0 ma-0"
                              >
                                <table
                                  style="width:100%;background:whitesmoke"
                                  class="py-3"
                                >
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
                                      <div
                                        class="indentation"
                                        :style="{
                                          width: step * nodeProps.depth + 'px',
                                        }"
                                      ></div>
                                    </div>
                                    <div
                                      id="AutoTest_IP928"
                                      class="pt-3"
                                      @click="nodeProps.onToggle"
                                    >
                                      <v-btn
                                        id="AutoTest_IP929"
                                        v-if="
                                          nodeProps.rowData.children.length > 0
                                        "
                                        text=""
                                        @click="
                                          nodeProps.rowData.isOpen = !nodeProps
                                            .rowData.isOpen
                                        "
                                      >
                                        <div v-if="!nodeProps.rowData.isOpen">
                                          <v-icon>mdi-chevron-right</v-icon>
                                        </div>
                                        <div v-if="nodeProps.rowData.isOpen">
                                          <v-icon>mdi-chevron-down</v-icon>
                                        </div>
                                      </v-btn>
                                    </div>
                                    <div
                                      class="pt-3"
                                      v-if="
                                        nodeProps.rowData.children.length == 0
                                      "
                                    >
                                      <v-btn
                                        id="AutoTest_IP930"
                                        text=""
                                        @click="
                                          nodeProps.rowData.isOpen = !nodeProps
                                            .rowData.isOpen
                                        "
                                      >
                                        <div
                                          v-if="!nodeProps.rowData.isOpen"
                                          hidden
                                        >
                                          <v-icon>mdi-chevron-right</v-icon>
                                        </div>
                                        <div
                                          v-if="nodeProps.rowData.isOpen"
                                          hidden
                                        >
                                          <v-icon>mdi-chevron-down</v-icon>
                                        </div>
                                      </v-btn>
                                    </div>
                                    <div class="cell text-limit pt-5">
                                      {{ nodeProps.rowData.HeadName }}
                                    </div>

                                    <div class="cell pt-5 ml-12 mr-12">
                                        {{ getSelectedCurrency.symbol }} {{ nodeProps.rowData.Amount }}
                                    </div>
                                    <div class="cell pt-4 ml-16 pl-10">
                                      <v-menu
                                        close-on-content-click=""
                                        transition="scroll-y-transition"
                                        left=""
                                        offset-y=""
                                        v-bind:disabled="!screenAccess"
                                      >
                                        <template v-slot:activator="{ on }">
                                          <v-btn
                                            min-width="27"
                                            height="29"
                                            v-on="on"
                                            color="grey"
                                            outlined=""
                                            dark=""
                                            v-bind:disabled="nodeProps.rowData.HeadName ==
                                                  'Output'"     
                                            class="elevation-0 mr-2 px-0"
                                          >
                                            <v-icon>mdi-dots-vertical</v-icon>
                                          </v-btn>
                                        </template>
                                        <v-list
                                          dense=""
                                          color="white"
                                          light=""
                                          width="auto"
                                          class="action-button-list"
                                        >
                                          <v-list-item
                                            id="AutoTest_IP931"
                                            link=""
                                            @click.stop="
                                              (vmEBudgetHeadID =
                                                nodeProps.rowData.BudgetHeadID),
                                                (vmEBudgetHead =
                                                  nodeProps.rowData.HeadName),
                                                (dlgEditBudgetPlan = true)
                                            "
                                          >
                                            <v-list-item-icon>
                                              <v-icon
                                                >mdi-pencil-outline</v-icon
                                              >
                                            </v-list-item-icon>
                                            <v-list-item-content
                                              >Edit</v-list-item-content
                                            >
                                          </v-list-item>
                                          <v-list-item
                                            id="AutoTest_IP932"
                                            link=""
                                            @click.stop="
                                              (vmEBudgetHeadID =
                                                nodeProps.rowData.BudgetHeadID),
                                                (vmEBudgetHead =
                                                  nodeProps.rowData.HeadName),
                                                (dlgRemoveHeader = true)
                                            "
                                          >
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
                                      <div
                                        class="indentation"
                                        :style="{
                                          width: step * leafProps.depth + 'px',
                                        }"
                                      ></div>
                                    </div>
                                    <div class="cell pt-5 text-limit">
                                      {{ leafProps.rowData.HeadItemName }}
                                    </div>
                                    <div class="cell pt-5 ml-12">
                                        {{ getSelectedCurrency.symbol }} {{ leafProps.rowData.Amount }}
                                    </div>

                                    <div class="cell pt-4 ml-12 pl-12">
                                      <v-menu
                                        close-on-content-click=""
                                        transition="scroll-y-transition"
                                        left=""
                                        offset-y=""
                                        v-bind:disabled="!screenAccess"
                                      >
                                        <template v-slot:activator="{ on }">
                                          <v-btn
                                            min-width="27"
                                            height="29"
                                            v-on="on"
                                            color="grey"
                                            outlined=""
                                            dark=""
                                            class="elevation-0 mr-2 px-0"
                                          >
                                            <v-icon>mdi-dots-vertical</v-icon>
                                          </v-btn>
                                        </template>
                                        <v-list
                                          dense=""
                                          color="white"
                                          light=""
                                          width="auto"
                                          class="action-button-list"
                                        >
                                          <v-list-item
                                            id="AutoTest_IP933"
                                            link=""
                                            @click.stop="
                                              (vmEBudgetHeadID =
                                                leafProps.rowData.BudgetHeadID),
                                                (vmEBudgetHeadItemID =
                                                  leafProps.rowData
                                                    .BudgetHeadItemID),
                                                (vmEBudgetHeadItemName =
                                                  leafProps.rowData
                                                    .HeadItemName),
                                                (vmEBudgetHeadItemAmount =
                                                  leafProps.rowData.Amount),
                                                (dlgEditBudgetPlanSubHeader = true)
                                            "
                                          >
                                            <v-list-item-icon>
                                              <v-icon
                                                >mdi-pencil-outline</v-icon
                                              >
                                            </v-list-item-icon>
                                            <v-list-item-content
                                              >Edit</v-list-item-content
                                            >
                                          </v-list-item>
                                          <v-list-item
                                            id="AutoTest_IP934"
                                            link=""
                                            @click.stop="
                                              (vmEBudgetHeadID =
                                                leafProps.rowData.BudgetHeadID),
                                                (vmEBudgetHeadItemID =
                                                  leafProps.rowData
                                                    .BudgetHeadItemID),
                                                (vmEBudgetHeadItemName =
                                                  leafProps.rowData
                                                    .HeadItemName),
                                                (dlgRemoveSubItem = true)
                                            "
                                          >
                                            <v-list-item-icon>
                                              <v-icon>
                                                mdi-trash-can-outline
                                              </v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content
                                              >Remove</v-list-item-content
                                            >
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
                  <v-row v-show="lstBudgetHeaderDetails.length == 0" justify="center">
                      <v-col class="text-center">
                          <p class="mt-4 pt-4">
                              <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                          </p>
                          <p class="heading-4-dark font-weight-medium pt-0">No Budget Plan available</p>
                          <v-btn id="AutoTest_IP935"
                                 height="38"
                                 color="success-btn"
                                 dark
                                 class="elevation-0 mr-4"
                                 @click="dlgCreateBudgetPlan = true"
                                 v-bind:disabled="!screenAccess">
                              Add Budget Plan
                          </v-btn>
                      </v-col>
                  </v-row>
                </v-stepper-content>
                <!-- Payment Term-->
                <v-stepper-content step="2" class="pa-0">
                  <v-card class="custom-card-1">
                    <v-card-text v-if="PaymentTermsitem.length == 0">
                      <!--If there is no payment-->
                      <v-row justify="center">
                          <v-col class="text-center">
                              <p class="mt-4 pt-4">
                                  <img src="..\..\..\src\assets\EmptyPlaceholders\finance.svg" height="100" width="100" />
                              </p>
                              <p class="heading-4-dark font-weight-medium pt-0">No payment terms available</p>
                              <Success-Button id="AutoTest_IP936"
                                              class=""
                                              style="width:200px"
                                              title="Add Payment Terms"
                                              @click.native="CreatePaymentTermsDialog = true"
                                              v-bind:disabled="!screenAccess"></Success-Button>
                          </v-col>
                      </v-row>
                    </v-card-text>
                    <!--If there is payments -->
                    <v-card-text
                      v-if="PaymentTermsitem.length > 0"
                      class="v-scrolling-div"
                      style="height:300px"
                    >
                      <v-row class="py-0">
                        <v-col class="py-0">
                          <v-timeline align-top dense>
                            <div
                              v-for="item in PaymentTermsitem"
                              :key="item.PaymentId"
                            >
                              <v-timeline-item
                                :color="item.Color"
                                :icon="item.Icon"
                                small
                                class=""
                              >
                                <v-row class="py-0 my-0">
                                  <v-col md="9" class="py-0 my-0">
                                    <p
                                      class="black--text"
                                      v-if="item.Logic == 'Date'"
                                    >
                                      Name : {{ item.PaymentTermName }} <br />
                                      Logic : {{ item.Logic }}({{
                                        new Date(
                                          item.paymentdate
                                        ).toLocaleDateString("en-GB")
                                      }}), Amount :
                                      {{ getSelectedCurrency.symbol }}
                                      {{ item.Amount }}
                                    </p>
                                    <p class="black--text" v-else>
                                      Name : {{ item.PaymentTermName }} <br />
                                      Logic : {{ item.Logic }}({{
                                        item.Target
                                      }}) , Amount :
                                      {{ getSelectedCurrency.symbol }}
                                      {{ item.Amount }}
                                    </p>
                                  </v-col>
                                  <v-col md="3" class="py-0 my-0">
                                    <v-menu
                                      close-on-content-click=""
                                      transition="scroll-y-transition"
                                      left=""
                                      offset-y=""
                                      v-bind:disabled="!screenAccess"
                                    >
                                      <template v-slot:activator="{ on }">
                                        <v-btn
                                          min-width="27"
                                          height="29"
                                          v-on="on"
                                          color="grey"
                                          outlined=""
                                          dark=""
                                          class="elevation-0 mr-2 px-0 "
                                          style="float:right"
                                        >
                                          <v-icon>mdi-dots-vertical</v-icon>
                                        </v-btn>
                                      </template>
                                      <v-list
                                        color="white"
                                        light
                                        width="auto"
                                        class="action-button-list"
                                        dense
                                      >
                                        <!-- Edit-->
                                        <v-list-item
                                          id="AutoTest_IP937"
                                          link
                                          @click.stop="
                                            fnGetPaymentTermsByPaymentId(
                                              item.PaymentId
                                            )
                                          "
                                        >
                                          <v-list-item-icon
                                            ><v-icon size="20"
                                              >mdi-pencil-outline</v-icon
                                            ></v-list-item-icon
                                          >
                                          <v-list-item-content
                                            >Edit</v-list-item-content
                                          >
                                        </v-list-item>
                                        <v-list-item
                                          id="AutoTest_IP938"
                                          link
                                          @click.stop="
                                            (RemovePaymentTermDialog = true),
                                              (PaymentId = item.PaymentId),
                                              (PaymentTermName =
                                                item.PaymentTermName)
                                          "
                                        >
                                          <v-list-item-icon
                                            ><v-icon
                                              >mdi-trash-can-outline</v-icon
                                            ></v-list-item-icon
                                          >
                                          <v-list-item-content
                                            >Remove</v-list-item-content
                                          >
                                        </v-list-item>
                                        <v-list-item
                                          id="AutoTest_IP939"
                                          link
                                          @click.stop="
                                            GetProjectPTUsers('Donor'),
                                              (PaymentId = item.PaymentId),
                                              (dlgRequestPayment = true)
                                          "
                                        >
                                          <v-list-item-icon>
                                            <v-icon
                                              >mdi-check-circle-outline</v-icon
                                            ></v-list-item-icon
                                          >
                                          <v-list-item-content
                                            >Request
                                            Payment</v-list-item-content
                                          >
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
                <!-- Audit Frame-->
                <v-stepper-content step="3" class="pa-0 py-0">
                  <v-row justify="center" class="py-0">
                    <v-col md="12" class="py-0">
                      <v-card flat class="py-0">
                        <v-card-text
                          style="height:300px"
                          class="v-scrolling-div"
                        >
                          <v-list dense class="py-0">
                            <template v-for="(item, index) in lstLogFrame">
                              <v-list-item class="py-0">
                                  <v-list-item-icon>
                                      <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                          <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                          <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                      </v-avatar>
                                  </v-list-item-icon>
                                <v-list-item-content>
                                  <v-list-item-title class="py-0 pb-0">
                                    <v-row class="d-lg-inline-flex py-0 pb-0">
                                      <v-col class="px-1 pl-3 py-0">
                                        <h4
                                          class="font-weight-large   py-0 pb-0 px-0"
                                        >
                                          {{ item.Name }}
                                        </h4>
                                      </v-col>
                                      <v-col class="px-0 py-0">
                                        <h4
                                          class="font-weight-regular  py-0 pb-0"
                                        >
                                          {{ item.Message }}.
                                        </h4>
                                      </v-col>
                                    </v-row>
                                  </v-list-item-title>
                                  <v-list-item-subtitle
                                    class="py-0 pb-0 mt-1 pt-0"
                                  >
                                    <span
                                      class="font-weight-regular py-0 pb-0 px-0"
                                      >{{
                                        formatDateAndTime(item.CreatedDate)
                                      }}</span
                                    >
                                  </v-list-item-subtitle>
                                </v-list-item-content>
                              </v-list-item>
                            </template>
                          </v-list>
                        </v-card-text>
                      </v-card>
                    </v-col>
                  </v-row>
                </v-stepper-content>
              </v-stepper-items>
            </v-stepper>
          </v-card-text>
        </v-card>
        <!--Add Budget Plan-->
        <v-dialog v-model="dlgCreateBudgetPlan" persistent width="800">
          <v-card >
            <v-card-title primary-title="" class="page-head pb-0">
              <v-avatar color="white" size="25" class="elevation-1 mr-2">
                <v-icon size="20" color="primary">mdi-cash</v-icon>
              </v-avatar>
              Add Budget Plan
              <v-spacer></v-spacer>
              <v-btn
                id="AutoTest_IP940"
                text=""
                @click="CloseBudgetPlanDialog()"
              >
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <!--Step-1-->
            <v-card-text v-show="CreateBudgetAndPlanStepOne">
              <h4 class="text-center heading-3 mt-4">
                Please select the no. of payment terms to create a new budget
                plan.
              </h4>
              <v-row justify="center" class="pt-4">
                <v-col md="6">
                  <v-text-field
                    type="number"
                    min="0"
                    @keydown="FilterInput($event)"
                    @paste.prevent
                    label="Select no. of Budget Head"
                    outlined=""
                    dense=""
                    v-model="NoOfBudgetPlans"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-card-text>
            <!--Step-2-->
            <v-form
              ref="budgetForm"
              v-model="valid"
              @submit.prevent="SaveProjectBudgetPlan">
              <v-card-text v-show="CreateBudgetAndPlanStepTwo">
                <h4 class="text-center heading-3 mt-4">
                  Please enter the basic details of the new budget plan.
                </h4>
                <h4 class="text-center black--text heading-3 mt-4">
                  <v-btn
                    id="AutoTest_IP941"
                    fab=""
                    small=""
                    v-bind:disabled="BudgetAndPlanstepper == 1"
                    class="mr-4 elevation-1"
                    color="#fff"
                    @click="fnSetpperIncrementBudgetAndPlan('Minus')"
                  >
                    <v-icon size="30" color="primary">mdi-arrow-left</v-icon>
                  </v-btn>
                  Budget Head {{ BudgetAndPlanstepper }} of
                  {{ NoOfBudgetPlans }}
                  <v-btn
                    id="AutoTest_IP942"
                    fab=""
                    small=""
                    class="ml-4 elevation-1"
                    color="#fff"
                    @click="fnSetpperIncrementBudgetAndPlan('Plus')"
                    v-bind:disabled="
                      BudgetAndPlanstepper == NoOfBudgetPlans ||
                        !fnChkBudgetPlanMandatory()
                    "
                  >
                    <v-icon size="30" color="primary">mdi-arrow-right</v-icon>
                  </v-btn>
                </h4>
                <v-row justify="center" class="pt-4" >
                  <v-col md="8" style="height:250px" class="v-scrolling-div">
                    <v-row class="pa-0 ma-0">
                        <v-col md="12" class="pa-0 ma-0">
                            <v-text-field label="Enter Budget Head"
                                          outlined=""
                                          dense=""
                                          v-model="BudgetHeadName"
                                          :rules="[rules.budgetHeadErrMesg]"
                                          required>
                                <template v-slot:append>
                                    <span v-if="BudgetHeadName.length == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>
                        </v-col>
                    </v-row>

                    <v-row
                      v-for="(subItem, index) in lstBudgetPlanItems"
                      :key="index"
                      class="pa-0 ma-0 py-0 my-0">
                        <v-col md="12" class=" d-inline-flex pa-0 ma-0 py-0 my-0">
                            <v-text-field style="height:70px"
                                          class="mr-1"
                                          label="Item Name"
                                          outlined=""
                                          dense=""
                                          v-model="subItem.SubHeaderName"
                                          :rules="[rules.NameErrMesg]"
                                          required>
                                <template v-slot:append>
                                    <span class="error--text">*</span>
                                </template>
                            </v-text-field>
                            <v-text-field style="height:50px"
                                          class="mr-1"
                                          @keydown="FilterInput($event)"
                                          v-bind:prefix="getSelectedCurrency.symbol"
                                          label="Amount"
                                          type="number"
                                          v-bind:placeholder="'0 out of '+ UnaccountedAmt"
                                          outlined=""
                                          dense=""
                                          @change="AddBudgetPlanAmt(subItem.Amount)"
                                          v-model="subItem.Amount"
                                          :rules="[rules.amountErrMesg,rules.max]">
                                <template v-slot:append>
                                    <span class="error--text">*</span>
                                </template>
                            </v-text-field>
                                <v-btn id="AutoTest_IP943" class="mb-2"
                                    v-show="lstBudgetPlanItems.length-1 && index !== lstBudgetPlanItems.length - 1"
                                    max-height="40"
                                    max-width="40"
                                    min-width="40"
                                    min-height="40"
                                    depressed=""
                                    fab=""
                                    light=""
                                    color="#EEEFF0"
                                    @click="DeleteSubItem(subItem)">
                                    <v-icon size="25" light="" color="#707070">mdi-close</v-icon>
                                </v-btn>
                                <v-btn id="AutoTest_IP944"
                                       v-show="index == lstBudgetPlanItems.length - 1"
                                       max-height="40"
                                       max-width="40"
                                       min-width="40"
                                       min-height="40"
                                       depressed=""
                                       fab=""
                                       light=""
                                       color="#EEEFF0"
                                       @click="AddHeaderSubItem(subItem)"
                                       class="mr-1"
                                       v-bind:disabled="subItem.SubHeaderName == '' || subItem.Amount == 0">
                                       <v-icon size="25" light="" color="#707070">mdi-plus</v-icon>
                                </v-btn>
                         </v-col>
                       </v-row>
                  </v-col>
                </v-row>
              </v-card-text>
              <v-card-actions class="pb-4">
                <v-col>
                  <Previous-Button-Outlined
                    id="AutoTest_IP945"
                    v-show="CreateBudgetAndPlanStepTwo"
                    class="float-right"
                    @click.native="
                      (CreateBudgetAndPlanStepOne = true),
                        (CreateBudgetAndPlanStepTwo = false)
                       
                    "
                  ></Previous-Button-Outlined>
                  <Cancel-Button-Outlined-Sm
                    id="AutoTest_IP946"
                    v-show="CreateBudgetAndPlanStepOne"
                    class="float-right"
                    @click.native="
                      CloseBudgetPlanDialog()
                    "
                  ></Cancel-Button-Outlined-Sm>
                  <Next-button-primary-light
                    id="AutoTest_IP947"
                    v-show="CreateBudgetAndPlanStepOne"
                    class="float-right mr-4"
                    @click.native="
                      (CreateBudgetAndPlanStepTwo = true),
                        (CreateBudgetAndPlanStepOne = false)
                    "
                    v-bind:disabled="NoOfBudgetPlans == 0"
                  ></Next-button-primary-light>

                  <v-btn
                    id="AutoTest_IP948"
                    v-show="CreateBudgetAndPlanStepTwo"
                    class="elevation-0 float-right mr-4 "
                    color="secondary elevation-0"
                    dark=""
                    height="36"
                    width="122"
                    type="submit"
                    @click="SaveProjectBudgetPlan()"
                  >
                    Confirm
                  </v-btn>
                </v-col>
              </v-card-actions>
            </v-form>
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
              <v-btn
                id="AutoTest_IP949"
                text=""
                @click="
                  (vmEBudgetHead = ''),
                    (vmEBudgetHeadID = ''),
                    (dlgEditBudgetPlan = false)
                "
              >
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-card-text>
              <h4 class="text-center heading-3 mt-4">
                Please edit the basic details to edit budget plan
              </h4>
              <v-row justify="center" class="mt-4">
                <v-col md="6" class="v-scrolling-div">
                  <v-text-field
                    outlined=""
                    dense=""
                    label="Budget Header"
                    v-model="vmEBudgetHead"
                  >
                  </v-text-field>
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions>
              <v-col>
                <Primary-Button-Outlined
                  id="AutoTest_IP950"
                  class="btn-122x36 float-right"
                  title="Cancel"
                  @click.native="
                    (vmEBudgetHead = ''),
                      (vmEBudgetHeadID = ''),
                      (dlgEditBudgetPlan = false)
                  "
                ></Primary-Button-Outlined>
                <Success-Button-Md
                  id="AutoTest_IP951"
                  class="btn-122x36 float-right mr-4"
                  title="Save"
                  @click.native="UpdateBudgetPlanHead()"
                ></Success-Button-Md>
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
              <v-btn
                id="AutoTest_IP952"
                text=""
                @click="ClearEditBudgetPlanItems()"
              >
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-card-text>
              <h4 class="text-center heading-3 mt-4">
                Please edit the basic details of the Item
              </h4>
              <v-row justify="center" class="mt-4">
                <v-col md="6" class="v-scrolling-div">
                  <v-text-field
                    outlined=""
                    dense=""
                    label="Name"
                    v-model="vmEBudgetHeadItemName"
                  >
                  </v-text-field>
                  <v-text-field
                    outlined=""
                    dense=""
                    type="number"
                    label="Amount"
                    v-model="vmEBudgetHeadItemAmount"
                    onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                    onKeyPress="if(this.value.length==8) return false;"
                    min="0"
                  >
                  </v-text-field>
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions>
              <v-col>
                <Primary-Button-Outlined
                  id="AutoTest_IP953"
                  class="btn-122x36 float-right"
                  title="Cancel"
                  @click.native="ClearEditBudgetPlanItems()"
                ></Primary-Button-Outlined>
                <Success-Button-Md
                  id="AutoTest_IP954"
                  class="btn-122x36 float-right mr-4"
                  title="Save"
                  @click.native="UpdateBudgetPlanHeadItems()"
                ></Success-Button-Md>
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
              <v-btn
                id="AutoTest_IP955"
                text=""
                @click="
                  (vmEBudgetHeadID = ''),
                    (vmEBudgetHeadItemID = ''),
                    (vmEBudgetHeadItemName = ''),
                    (dlgRemoveSubItem = false)
                "
              >
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
                            Are you sure you want to remove this Item ?
                          </h4>
                        </v-col>
                        <v-col cols="12" md="12" class="pt-0">
                          <v-row justify="center">
                            <h5 class="page-head mt-4">
                              <v-avatar
                                color="white"
                                size="25"
                                class="elevation-1 mr-2 mb-1"
                              >
                                <v-icon size="15" color="primary"
                                  >mdi-cash-multiple</v-icon
                                >
                              </v-avatar>
                              {{ vmEBudgetHeadItemName }}
                            </h5>
                          </v-row>
                        </v-col>
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right py-0"
                        >
                          <v-btn
                            id="AutoTest_IP956"
                            color="secondary elevation-0"
                            class="mr-2"
                            @click="RemoveBudgetPlanSubList()"
                            >Remove</v-btn
                          >
                          <v-btn
                            id="AutoTest_IP957"
                            outlined
                            color="grey"
                            @click="
                              (vmEBudgetHeadID = ''),
                                (vmEBudgetHeadItemID = ''),
                                (vmEBudgetHeadItemName = ''),
                                (dlgRemoveSubItem = false)
                            "
                            >Cancel</v-btn
                          >
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
              <v-btn
                id="AutoTest_IP958"
                text=""
                @click="
                  (vmEBudgetHeadID = ''),
                    (vmEBudgetHead = ''),
                    (dlgRemoveSubItem = false)
                "
              >
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
                            Are you sure you want to remove this Budget Plan
                            Head?
                          </h4>
                        </v-col>
                        <v-col cols="12" md="12" class="pt-0">
                          <v-row justify="center">
                            <h5 class="page-head mt-4">
                              <v-avatar
                                color="white"
                                size="25"
                                class="elevation-1 mr-2 mb-1"
                              >
                                <v-icon size="15" color="primary"
                                  >mdi-cash-multiple</v-icon
                                >
                              </v-avatar>
                              {{ vmEBudgetHead }}
                            </h5>
                          </v-row>
                        </v-col>
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right py-0"
                        >
                          <v-btn
                            id="AutoTest_IP959"
                            color="secondary elevation-0"
                            class="mr-2"
                            @click="RemoveBudgetPlanHead()"
                            >Remove</v-btn
                          >
                          <v-btn
                            id="AutoTest_IP960"
                            outlined
                            color="grey"
                            @click="
                              (vmEBudgetHeadID = ''),
                                (vmEBudgetHead = ''),
                                (dlgRemoveHeader = false)
                            "
                            >Cancel</v-btn
                          >
                        </v-col>
                      </v-row>
                    </v-container>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
          </v-card>
        </v-dialog>

        <!--Create Payment Terms Dialog-->
        <v-dialog
          v-model="CreatePaymentTermsDialog"
          paymentValid="true"
          persistent
          width="800"
        >
          <v-card>
            <v-card-title primary-title="" class="page-head pb-0">
              <v-avatar color="white" size="25" class="elevation-1 mr-2">
                <v-icon size="20" color="primary">mdi-cash</v-icon>
              </v-avatar>
              Create Payment Terms
              <v-spacer></v-spacer>
              <v-btn id="AutoTest_IP961" text="" @click="ClosepaymentDialog()">
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <!--Step-1-->
            <v-card-text v-show="CreatePaymentTermsStepOne">
              <h4 class="text-center heading-3 mt-4">
                Please select the no. of payment terms to create.
              </h4>
              <v-row justify="center" class="pt-4">
                <v-col md="6">
                  <v-text-field
                    type="number"
                    min="0"
                    @keydown="FilterInput($event)"
                    @paste.prevent
                    label="Enter no. of payment terms"
                    outlined=""
                    dense=""
                    v-model="NoOfPayments"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-card-text>
            <!--Step-2-->
            <v-form
              ref="paymentForm"
              v-model="valid"
              @submit.prevent="fnSavepaymentTerms"
            >
              <v-card-text v-show="CreatePaymentTermsStepTwo">
                <h4 class="text-center heading-3 mt-4">
                  Please enter the basic details of the new payment terms.
                </h4>
                <h4 class="text-center black--text heading-3 mt-4">
                  <v-btn
                    id="AutoTest_IP962"
                    fab=""
                    small=""
                    v-bind:disabled="stepper == 1"
                    class="mr-4 elevation-1"
                    color="#fff"
                    @click="fnSetpperIncrement('Minus')"
                  >
                    <v-icon
                      size="30"
                      color="primary"
                      style="color:#0964D9 !important"
                      >mdi-arrow-left</v-icon
                    >
                  </v-btn>
                  Payment Term {{ stepper }} of {{ NoOfPayments }}
                  <v-btn
                    id="AutoTest_IP963"
                    fab=""
                    small=""
                    class="ml-4 elevation-1"
                    color="#fff"
                    @click="fnSetpperIncrement('Plus')"
                    v-bind:disabled="
                      stepper == NoOfPayments || !fnCheckMandatory()
                    "
                  >
                    <v-icon
                      size="30"
                      color="primary"
                      style="color:#0964D9 !important"
                      >mdi-arrow-right</v-icon
                    >
                  </v-btn>
                </h4>
                <v-row justify="center" class="pt-4">
                    <v-col md="6" style="height:250px" class="v-scrolling-div">
                        <v-text-field class="mt-4"
                                      label="Enter Payment Term Name"
                                      outlined=""
                                      dense=""
                                      v-model="PaymentTermName"
                                      :rules="[rules.NameErrMesg]"
                                      required>
                            <template v-slot:append>
                                <span v-if="PaymentTermName.length == 0"
                                      class="error--text">*</span>
                            </template>
                        </v-text-field>
                        <v-select dense=""
                                  v-model="CreatePaymentTermsLogicModel"
                                  :items="CreatePaymentTermsLogic"
                                  label="Select Logic"
                                  :rules="[rules.LogicErrMesg]"
                                  required
                                  outlined="">
                            <template v-if="
                          CreatePaymentTermsLogicModel == '' ||
                            CreatePaymentTermsLogicModel == null
                        "
                                      v-slot:append="">
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
                                <v-text-field v-show="CreatePaymentTermsLogicModel === 'Date'"
                                              v-model="paymentdate"
                                              label="Select Date"
                                              prepend-inner-icon="mdi-calendar"
                                              outlined=""
                                              dense=""
                                              readonly=""
                                              v-on="on"></v-text-field>
                            </template>
                            <v-date-picker v-model="paymentdate"
                                           no-title
                                           @input="(PaymentDateMenu = false), IsValidPaymentDate()"
                                           format="DD-MM-YYYY"
                                           :max="ProjectEndDate"
                                           :min="ProjectStartDate">
                            </v-date-picker>
                        </v-menu>

                        <v-text-field type="number"
                                      suffix="%"
                                      v-show="CreatePaymentTermsLogicModel === 'Project Target'"
                                      label="Target(%)"
                                      outlined=""
                                      dense=""
                                      v-model="Target"
                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                      onKeyPress="if(this.value.length==3) return false;"
                                      min="0">
                        </v-text-field>
                        <div class="py-0">
                        <v-text-field type="number"
                                      label="Type amount"
                                      outlined=""
                                      dense=""
                                      v-bind:prefix="getSelectedCurrency.symbol"
                                      v-bind:placeholder="'0 out of '+ PaymentTermLeftAmt"
                                      v-model="amount"
                                      @change="AddPaymentTermAmt(amount)"
                                      :rules="[rules.amountErrMesg]"
                                      required
                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                      onKeyPress="if(this.value.length==8) return false;"
                                      min="0">
                            <template v-slot:append>
                                <span v-if="" class="error--text">*</span>
                            </template>
                        </v-text-field>
                         <span style="color:red;font-size:smaller;position:relative;top:-17px" v-show="amountErr == true" class="ml-3 py-0 my-0">The Maximum Amount should be {{PaymentTermLeftAmt}}</span>
                        </div>
                        <span class="small-text mr-1 py-0">
                            <v-icon size="12">mdi-account</v-icon> Notify People
                        </span>
                        <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedNotifyUsers.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                            <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                        </v-avatar>
                        <v-avatar v-if="selectedNotifyUsers.length>=4" size="25" class="team-count">
                            +{{selectedNotifyUsers.length-3}}
                        </v-avatar>

                        <v-menu absolute
                                v-model="NotificationSelectPopUp"
                                top=""
                                :offset-y="offset"
                                :close-on-click="closeonClick"
                                :close-on-content-click="closeOnContentClick">
                            <template v-slot:activator="{ on }">
                                <v-btn id="AutoTest_IP963A"
                                       v-on="on"
                                       max-height="24"
                                       max-width="24"
                                       min-width="24"
                                       min-height="24"
                                       depressed=""
                                       fab=""
                                       light=""
                                       color="#E5F2FF"
                                       @click="
                            GetProjectPTUsers('Finance'),
                              GetProjectPTUsers('Donor')
                          ">
                                    <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                </v-btn>
                            </template>
                            <!--Notification select pop up-->
                            <v-card>
                                <v-card-title class="heading-3">
                                    Add people to Notify
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_IP964"
                                           text=""
                                           height="25"
                                           min-width="25"
                                           max-width="25"
                                           color="#707070"
                                           @click="NotificationSelectPopUp = false">
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
                                                <div style="height:150px;"
                                                     class="v-scrolling-div">
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
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
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
                                                        Donors
                                                    </v-list-item-content>
                                                </v-list-item>
                                                <v-divider></v-divider>
                                                <div style="height:150px;"
                                                     class="v-scrolling-div">
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
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                            {{ item.UserName }}
                                                        </template>
                                                    </v-data-table>
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
                                                </div>
                                            </v-list>
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                                <v-divider></v-divider>
                                <v-card-actions>
                                    <v-row no-gutters="">
                                        <v-col md="12" align="center">
                                            <Success-Button id="AutoTest_IP965"
                                                            style="width:130px"
                                                            class="my-1 center-block"
                                                            title="Save"
                                                            @click.native="SaveNotifyUsers()"></Success-Button>
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
                  <Previous-Button-Outlined
                    id="AutoTest_IP966"
                    v-show="CreatePaymentTermsStepTwo"
                    class="float-right"
                    @click.native="
                      (CreatePaymentTermsStepOne = true),
                        (CreatePaymentTermsStepTwo = false)
                    "
                  ></Previous-Button-Outlined>
                  <Cancel-Button-Outlined-Sm
                    id="AutoTest_IP967"
                    v-show="CreatePaymentTermsStepOne"
                    class="float-right"
                    @click.native="CreatePaymentTermsDialog = false"
                  ></Cancel-Button-Outlined-Sm>
                  <Next-button-primary-light
                    id="AutoTest_IP968"
                    v-show="CreatePaymentTermsStepOne"
                    class="float-right mr-4"
                    @click.native="
                      (CreatePaymentTermsStepTwo = true),
                        (CreatePaymentTermsStepOne = false)
                    "
                    v-bind:disabled="NoOfPayments == 0"
                  ></Next-button-primary-light>

                  <v-btn
                    id="AutoTest_IP969"
                    v-show="CreatePaymentTermsStepTwo"
                    class="elevation-0 float-right mr-4"
                    height="36"
                    width="122"
                    color="secondary elevation-0"
                    type="submit"
                    @click="fnSavepaymentTerms()"
                  >
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
                    <v-btn id="AutoTest_IP970" text @click="CloseEditPaymentTerms()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <!--Step-1-->
                <v-form ref="editPaymentForm"
                        v-model="valid"
                        @submit.prevent="fnUpdatepaymentTerms">
                    <v-card-text>
                        <v-stepper v-model="CreatePaymentTermsStepper"
                                   class="elevation-0">
                            <v-stepper-items>
                                <v-stepper-content step="1">
                                    <h4 class="text-center heading-3 mt-4">
                                        Edit the basic details of the payment terms.
                                    </h4>
                                    <v-row justify="center" class="pt-4">
                                        <v-col md="6"
                                               style="height:250px"
                                               class="v-scrolling-div">
                                            <v-text-field class="mt-4"
                                                          label="Enter Payment Term Name"
                                                          outlined
                                                          dense
                                                          v-model="PaymentInfo.PaymentTermName"
                                                          :rules="[rules.NameErrMesg]"
                                                          required>
                                                <template v-slot:append>
                                                    <span v-if="PaymentInfo.PaymentTermName == 0"
                                                          class="error--text">*</span>
                                                </template>
                                                >
                                            </v-text-field>
                                            <v-select dense
                                                      v-model="PaymentInfo.Logic"
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
                                                    <v-text-field v-show="PaymentInfo.Logic == 'Date'"
                                                                  v-model="
                                new Date(
                                  PaymentInfo.paymentdate
                                ).toLocaleDateString('en-GB')
                              "
                                                                  label="Select Date"
                                                                  prepend-inner-icon="mdi-calendar"
                                                                  outlined
                                                                  dense
                                                                  persistent-hint
                                                                  readonly
                                                                  v-on="on"
                                                                  class="mb-2"></v-text-field>
                                                </template>
                                                <v-date-picker v-model="PaymentInfo.paymentdate"
                                                               no-title
                                                               @input="FrmDateMenu = false"
                                                               dense
                                                               hide-details
                                                               format="DD-MM-YYYY"
                                                               :max="ProjectEndDate"
                                                               :min="ProjectStartDate"></v-date-picker>
                                            </v-menu>
                                            <v-text-field type="number"
                                                          v-show="PaymentInfo.Logic == 'Project Target'"
                                                          label="Target(%)"
                                                          v-model="PaymentInfo.Target"
                                                          outlined
                                                          dense
                                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                          onKeyPress="if(this.value.length==3) return false;"
                                                          min="0"></v-text-field>

                                            <v-text-field type="number"
                                                          label="Type amount"
                                                          v-model="PaymentInfo.Amount"
                                                          outlined
                                                          dense
                                                          :rules="[rules.amountErrMesg]"
                                                          required
                                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                          onKeyPress="if(this.value.length==8) return false;"
                                                          min="0">
                                                <template v-slot:append>
                                                    <span v-if="" class="error--text">*</span>
                                                </template>
                                            </v-text-field>

                                            <span class="small-text mr-1">
                                                <v-icon size="12">mdi-account</v-icon> Notify People
                                            </span>
                                            <v-avatar size="25" class="team-avatar" v-for="(item,key) in PaymentInfo.users.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.UserName)}`">
                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                <span class="white--text" v-else>{{item.UserName == null ? "" : item.UserName.substring(0, 1).toUpperCase()}}</span>
                                            </v-avatar>
                                            <v-avatar v-if="PaymentInfo.users.length>=4" size="25" class="team-count">
                                                +{{PaymentInfo.users.length-3}}
                                            </v-avatar>

                                            <v-menu v-model="NotificationSelectPopUp2"
                                                    top=""
                                                    :offset-y="offset"
                                                    :close-on-click="closeonClick"
                                                    :close-on-content-click="closeOnContentClick">
                                                <template v-slot:activator="{ on }">
                                                    <Add-Button-Fab id="AutoTest_IP971"
                                                                    @click.native="
                                GetPaymentTermsNotifyUsers(),
                                  (NotificationSelectPopUp2 = true)
                              "></Add-Button-Fab>
                                                </template>
                                                <!--Notification select pop up-->
                                                <v-card>
                                                    <v-card-title class="heading-3">
                                                        Add people to Notify
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP972"
                                                               text=""
                                                               height="25"
                                                               min-width="25"
                                                               max-width="25"
                                                               color="#707070"
                                                               @click="NotificationSelectPopUp2 = false">
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
                                                                    <div style="height:150px;"
                                                                         class="v-scrolling-div">
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
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                                {{ item.UserName }}
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
                                                                            Donors
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                    <v-divider></v-divider>
                                                                    <div style="height:150px;"
                                                                         class="v-scrolling-div">
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
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>  {{item.UserName == null ? "" :item.UserName.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                                {{ item.UserName }}
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
                                                                <Success-Button id="AutoTest_IP973"
                                                                                style="width:130px"
                                                                                class="py-1 center-block"
                                                                                title="Save"
                                                                                @click.native="
                                      SaveNotifyUsers(),
                                        (PaymentInfo.users = selectedNotifyUsers),
                                        (NotificationSelectPopUp2 = false)
                                    "></Success-Button>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-actions>
                                                </v-card>
                                                <!--__________________________-->
                                            </v-menu>
                                            <!--<Add-Button-Fab id="AutoTest_IP971" @click.native="GetPaymentTermsNotifyUsers(),NotificationSelectPopUp2 = true"></Add-Button-Fab>-->
                                            <!--Notification select pop up-->
                                            <!--<v-card class="position-absolute-popup-sm" width="50vw" style="right:20px; top:90px" v-if="NotificationSelectPopUp2">

                                                            <v-card-title class="heading-3">
                                                                Add people to Notify
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP972" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp2=false">
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
                                                                            <v-divider></v-divider>
                                                                            <v-data-table :headers="lstuserTableheaders"
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
                                                                            </v-data-table>-->
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
                                            <!--</v-list>
                                                                    </v-col>
                                                                    <v-col class="py-0">
                                                                        <v-list dense="" class="py-0">
                                                                            <v-list-item>
                                                                                <v-list-item-content style="font-size:17px">
                                                                                    Donars
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                            <v-divider></v-divider>
                                                                            <v-data-table :headers="AddDonorTableHeaders"
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
                                                                            </v-data-table>-->
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
                                            <!--</v-list>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row no-gutters="">
                                                                    <v-col md="12" align="center">
                                                                        <Success-Button id="AutoTest_IP973" style="width:130px" class="mb-2 center-block" title="Save"
                                                                                        @click.native="SaveNotifyUsers(),PaymentInfo.users =selectedNotifyUsers, NotificationSelectPopUp2=false"></Success-Button>
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
                            <Cancel-Button-Outlined-Sm id="AutoTest_IP974"
                                                       class="float-right"
                                                       @click.native="CloseEditPaymentTerms()"></Cancel-Button-Outlined-Sm>
                            <v-btn id="AutoTest_IP975"
                                   class="elevation-0 float-right mr-4 primary-btn-light"
                                   dark
                                   height="36"
                                   width="122"
                                   type="submit"
                                   @click="fnUpdatepaymentTerms()">
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
              <v-btn
                id="AutoTest_IP976"
                text=""
                @click="RemovePaymentTermDialog = false"
              >
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
                            Are you sure you want to remove this payment terms ?
                          </h4>
                        </v-col>
                        <v-col>
                          <h4 class="text-center heading-3">
                            <v-icon class="tmr-2" color="#707070" size="25"
                              >mdi-book</v-icon
                            >
                            {{ PaymentTermName }}
                          </h4>
                        </v-col>

                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right py-0"
                        >
                          <v-btn
                            id="AutoTest_IP977"
                            color="secondary elevation-0"
                            class="mr-2"
                            @click="
                              DeletePaymentTerm(),
                                (RemovePaymentTermDialog = false)
                            "
                            >Remove</v-btn
                          >
                          <v-btn
                            id="AutoTest_IP978"
                            outlined
                            color="grey"
                            @click="RemovePaymentTermDialog = false"
                            >Cancel</v-btn
                          >
                        </v-col>
                      </v-row>
                    </v-container>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
          </v-card>
        </v-dialog>
        <!-- Request Payment-->
        <v-dialog v-model="dlgRequestPayment" width="800" persistent>
          <v-card>
            <v-card-title primary-title="" class="page-head pb-0">
              <v-avatar color="white" size="25" class="elevation-1 mr-2">
                <v-icon size="20" color="primary">mdi-cash</v-icon>
              </v-avatar>
              Request Payment
              <v-spacer></v-spacer>
              <v-btn id="AutoTest_IP979" text="" @click="ClearRequestPayment()">
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-card-text>
              <h4 class="text-center heading-3 mt-4">
                Please enter the basic details for requesting payment.
              </h4>
              <v-row justify="center" class="mt-4">
                <v-col md="6" class="v-scrolling-div">
                  <v-text-field
                    outlined=""
                    dense=""
                    label="Amount"
                    v-model="RequestAmount"
                    onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                    onKeyPress="if(this.value.length==8) return false;"
                    type="number"
                    min="0"
                  >
                  </v-text-field>
                  <v-textarea
                    rows="3"
                    outlined
                    dense
                    label="Enter comment"
                    type="text"
                    v-model="RequestComment"
                  >
                  </v-textarea>

                  <v-file-input
                    label="Upload attachment"
                    prepend-inner-icon="mdi-paperclip"
                    prepend-icon=""
                    outlined=""
                    dense=""
                    v-model="fileInput"
                    id="file"
                  ></v-file-input>

                  <v-select
                    dense=""
                    v-model="RequestFundDonorUser"
                    label="Select Donar"
                    :items="lstPTDonors"
                    item-text="UserName"
                    item-value="UserID"
                    outlined=""
                  >
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
                <Outlined-Button-Dark
                  id="AutoTest_IP980"
                  class="btn-122x36 float-right"
                  title="Cancel"
                  @click.native="ClearRequestPayment()"
                ></Outlined-Button-Dark>
                <Primary-Button-Light
                  id="AutoTest_IP981"
                  class="btn-122x36 float-right mr-4"
                  title="Save"
                  @click.native="SavePaymentRequest()"
                  v-bind:disabled="RequestFundDonorUser.length==0  || RequestAmount ==''"
                ></Primary-Button-Light>
              </v-col>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-col>
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
    </v-row>
  </div>
</template>
<script>
    import { mapGetters } from "vuex";
    import TreeTable from "vue-tree-table-component";
    import CloseBtnFabSmall from "@/components/close-btn-fab-small.vue";
    import CancelButtonOutlinedSm from "@/components/cancel-button-outlined-sm.vue";
    import PreviousButtonOutlined from "@/components/previous-button-outlined.vue";
    import AddButtonFab from "@/components/add-button-fab.vue";
    import NextButtonPrimaryLight from "@/components/next-button-blue.vue";
    import SuccessButtonMd from "@/components/success-button-md.vue";

    import SuccessButton from "@/components/success-button.vue";
    import PrimaryButtonOutlined from "@/components/primary-button-outlined.vue";
    import DefaultIconButtonOutlined from "@/components/default-icon-button-outlined.vue";
    import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
    import DoughnutChart from "@/components/DoughnutChart.vue";

    import OutlinedButtonDark from "@/components/outlined-button-dark.vue";
    import PrimaryButtonLight from "@/components/primary-button-light.vue";
    import PreLoader from "@/components/pre-loader.vue";

    var objPrjFinance;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectFinance.js").then((mod) => {
                objPrjFinance = new mod.ManageProjectFinance();
            }),
        ]);
    }
    var objManageProject;
    async function importscriptManageProject() {
        return Promise.all([
            import("../../BL/ManageProject.js").then((mod) => {
                objManageProject = new mod.ManageProject();
            }),
        ]);
    }
    export default {
        data() {
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
                fixedColor: ["#399CFF", "#00C2B0", "#F1C100", "#00B448", "#2C4258", "#8307CB", "#F98B00", "#D10000", "#E6217B", "#6F00B0", "#464646", "#395FFF", "#038E81", "#C99506", "#76B400", "#2C584A", "#521CA8", "#D5D5D5", "#FF6200", "#88320C", "#CE72B9", "#A00000", "#41AFFF", "#00E209", "#D008A9", "#FFD446"],
                screenAccess: 0,
                RWAccess: 0,
                justifyCenter: "center",

                ProjectID: 0,
                InitiativeID: 0,
                PreLoader: false,

                //====Budget&Fund=======//
                dlgAddDonor: false,
                Amount: 0,
                lstDonorItems: [],
                lstAccountItems: [],
                DonorNotFundedFund: 0,
                selectedDonor: "",
                selectedAccount: "",
                TitleText: "Add Donor",
                AddSubHeadText: "Please enter the basic details of donor",
                EditSubHeadText: "Please edit the basic details of donor",
                Subheadtext: "",
                BudgetAndFinanceSteps: 1,
                //payment listing parameters
                FundExpensesListHeader: [
                    { text: "Title", value: "Title" },
                    { text: "Txn ID", value: "TransactionID" },
                    { text: "Txn Date", value: "TransactionDate" },
                    { text: "Amount", value: "TransactionAmount" },
                    { text: "Documents", value: "TransactionDocName" },
                ],
                PaymentList: [],
                lstActivityItems: [],
                selectedActivity: "",
                BudgetPlanSteps: 1,
                CreatePaymentTermsStepOne: true,
                CreatePaymentTermsStepTwo: false,
                CreatePaymentTermsStepThree: false,

                BudgetAndFundHeader: "Budget & Funds",
                //Top bar filter parameters
                filterExpense: 0,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                dlgCreateBudgetPlan: false,
                NoOfPayments: 0,
                stepper: 1,
                CreatePaymentTermsLogicModel: "",

                //========Budgetplan=============//

                BudgetHeadName: "",
                BudgetHeadAmount: 0,
                BudgetHeadItemName: "",

                ProjectHeadTableHeader: [
                    { label: "Name", id: "Name", props: "Name" },
                    { label: "Amount", id: "Amount", props: "Amount" },
                    { label: "Action", id: "action", props: "action" },
                ],

                CreateBudgetAndPlanStepOne: true,
                CreateBudgetAndPlanStepTwo: false,
                BudgetAndPlanstepper: 1,
                NoOfBudgetPlans: 0,

                TitleTextBudget: "Budget Plan",

                //=======PaymentTerms========//
                CreatePaymentTermsDialog: false,
                EditPaymentTermsDialog: false,
                RemovePaymentTermDialog: false,
                dlgRequestPayment: false,
                PaymentTermName: "",
                CreatePaymentTermsLogic: ["Date", "Project Target"],
                PaymentDateMenu: false,
                ProjectEndDate: new Date().toISOString().substr(0, 10),
                ProjectStartDate: new Date().toISOString().substr(0, 10),
                paymentdate: new Date().toISOString().substr(0, 10),
                Target: 0,
                amount: null,
                selectedFinanceUser: [],
                NotificationSelectPopUp: false,
                FinanceUsersTemp: [],
                DonarUserTemp: [],
                
                PaymentTerm: {
                    PaymentTermList: [],
                },
                PaymentTermsitem: [],
                ExpenseList: [],

                PaymentfilterList: [],
                lstBudgetPlan: [],
                budgetAmt: 0,

                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],

                DonarFundList: [],
                DonarUtilizedFund: 0,
                DonarUnUtilizedFund: 0,
                BudgetFundID: 0,
                RemoveUserDailog: false,
                DUserID: 0,
                DUserName: "",
                lstFinanceUsers: [],
                lstDonors: [],
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
                    BudgetItemList: [{ HeaderName: "", Amount: "" }],
                },
                BudgetPlan: {
                    BudgetPlanList: [],
                },
                totermsAmt: 0,
                toBudgetAmt: 0,
                SetasTempFileInput: null,
                Description: "",
                reqAmount: 0,
                NotificationSelectPopUp2: false,
                FrmDateMenu: false,
                CreatePaymentTermsStepper: 1,
                step: 50,

                // Edit Budget Plan
                dlgEditBudgetPlan: false,
                EditBudgetPlanHeader: "",
                

                dlgEditBudgetPlanSubHeader: false,
                EditBudgetAmount: 0,
                EditBudgetPlanSubHeader: "",
                

                dlgRemoveSubItem: false,
                

                dlgRemoveHeader: false,
                

                // Adding Finance users and Donors Variables
                AssignFinUserPopUp: false,
                AddDonorPopUp: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AddDonorPopUp: false,
                AssignDonorSteps: 1,
                SelectBankAccountItems: ["Sample Account 1", "Sample Account 2"],
                userimage: "",
                budgetAmt: 0,
                GroupID: 0,
                lstBudgetFinUserInfo: [],
                lstBudgetDonorUserInfo: [],
                lstuserTableheaders: [{ text: "Name", align: "left", value: "UserName" }],
                AddDonorTableHeaders: [
                    { text: "Name", align: "left", value: "UserName" },
                    { text: "", value: "data-table-expand" },
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

                txtDonorAmt: 0,
                vmDonorAccount: [],
                errorMsg: " * Please add Budget Amount",
                isValidBudget: 0,

                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectID: 0,

                lstBudgetPlanItems: [{ SubHeaderName: "", Amount: null }],
                TotalBudgetPlanAmt: 0,
                budgetHeadAmount: 0,
                lstBudgetPlanDetails: [],
                lstBudgetHeaderDetails: [],
                TotalHeadAmt: 0,
                UnaccountedAmt: 0,
                UnaccountedAmtCpy: 0,
                vmEBudgetHeadID: 0,
                vmEBudgetHead: "",
                vmEBudgetHeadItemID: 0,
                vmEBudgetHeadItemName: "",
                vmEBudgetHeadItemAmount: 0,
                ExpenseTagList: [],

                ExpenseTag: "",
                PaymentTermAmount: 0,
                PaymentTermLeftAmt: 0,

                //=====PaymentTerms======//
                lstPTFinanceUsers: [],
                lstPTDonors: [],
                vmPTFinanceUsers: [],
                vmPTDonors: [],
                selectedNotifyUsers: [],
                lstProjectUsers: [],
                RequestFundDonorUser: [],
                fileInput: null,
                vmFUBudgetHead: [],
                UnUtilizedAmt: 0,
                RequestComment: "",
                RequestAmount: "",
                lstLogFrame: [],
                lstFundExpenses: [],
                IniBudgetHead: "",
                lstFundPaymentDeatils: [],
                amountErr: false,
                dcExistingPymtTermSum: 0,
            };
        },
        components: {
            "Close-Btn-Fab-Small": CloseBtnFabSmall,
            "Cancel-Button-Outlined-Sm": CancelButtonOutlinedSm,
            "Previous-Button-Outlined": PreviousButtonOutlined,
            "Add-Button-Fab": AddButtonFab,
            "Next-button-primary-light": NextButtonPrimaryLight,
            "Success-Button-Md": SuccessButtonMd,
            "Success-Button": SuccessButton,
            "Primary-Button-Outlined": PrimaryButtonOutlined,
            "Default-Icon-Button-Outlined": DefaultIconButtonOutlined,
            "Default-Button-Outlined": DefaultButtonOutlined,
            "Outlined-Button-Dark": OutlinedButtonDark,
            "Primary-Button-Light": PrimaryButtonLight,
            "Pre-Loader": PreLoader,
            TreeTable,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            await importscript();
            await importscriptManageProject();
            this.PreLoader = true;
            await this.GetProjectBudgetDetails();
            await this.GetBudgetPlanDeatils();
            this.PreLoader = false;
            await this.GetFundPaymentDeatils();
            await this.fnGetPaymentTermsByprjID();
            await this.fillPlannedVsActualChart();
            await this.fnGetProjectDetailsByID();
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

            getRandomColor: function () {
                //this.mycolor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
                this.mycolor = "#" + Math.floor(Math.random() * 16777215).toString(16);
            },
            formatDate(date) {
                if (!date) return null;
                const [year, month, day] = date.substr(0, 10).split("-");
                return `${day}-${month}-${year}`;
            },
            formatDateAndTime(date) {
                if (!date) return null;
                const [day, month, year] = date.substr(0, 10).split("-");
                const [hh, mm, ss] = date.substr(11, 19).split(":");
                if (month === "01") {
                    if (hh == 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "02") {
                    if (hh == 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "03") {
                    if (hh == 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "04") {
                    if (hh == 12) {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"APR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "05") {
                    if (hh == 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "06") {
                    if (hh == 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "07") {
                    if (hh == 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "08") {
                    if (hh == 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "09") {
                    if (hh == 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "10") {
                    if (hh == 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "11") {
                    if (hh == 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "12") {
                    if (hh == 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
            },
            async FilterInput(event) {
                var isNotWanted;
                var keyCode = "which" in event ? event.which : event.keyCode;

                isNotWanted =
                    keyCode == 69 || keyCode == 101 || keyCode == 109 || keyCode == 189;
                if (isNotWanted == true) {
                    event.preventDefault();
                }
            },
            fnNextChart(type) {
                if (type == "Plus") {
                    this.BudgetAndFinanceSteps = this.BudgetAndFinanceSteps + 1;
                    if (this.BudgetAndFinanceSteps == 2) {
                        this.BudgetAndFundHeader = "Fund Utilization";
                        this.GetFundPaymentDeatils();
                    } else if (this.BudgetAndFinanceSteps == 3) {
                        this.BudgetAndFundHeader = "Planned & Actual";
                        this.fillPlannedVsActualChart();
                    }
                } else if (type == "Minus") {
                    this.BudgetAndFinanceSteps = this.BudgetAndFinanceSteps - 1;
                    if (this.BudgetAndFinanceSteps == 2) {
                        this.BudgetAndFundHeader = "Fund Utilization";
                        this.GetFundPaymentDeatils();
                    } else if (this.BudgetAndFinanceSteps == 1) {
                        this.BudgetAndFundHeader = "Budget & Funds";
                        this.GetProjectBudgetDetails();
                    }
                }
            },
            fnNextChartBudgetPlan(type) {
                if (type == "Plus") {
                    this.BudgetPlanSteps = this.BudgetPlanSteps + 1;
                    if (this.BudgetPlanSteps == 2) {
                        this.TitleTextBudget = "Payment Terms";
                        this.GetProjectPTUsers("Finance");
                        this.GetProjectPTUsers("Donor");
                    } else if (this.BudgetPlanSteps == 3) {
                        this.TitleTextBudget = "Audit Frame";
                        this.fnGetAuditLog();
                    }
                } else if (type == "Minus") {
                    this.BudgetPlanSteps = this.BudgetPlanSteps - 1;
                    if (this.BudgetPlanSteps == 2) {
                        this.TitleTextBudget = "Payment Terms";
                        this.GetProjectPTUsers("Finance");
                        this.GetProjectPTUsers("Donor");
                    }
                    if (this.BudgetPlanSteps == 1) {
                        this.TitleTextBudget = "Budget Plan";
                        this.GetBudgetPlanDeatils();
                    }
                }
            },

            // ============Budget & Funds ========================================//
            async GetProjectBudgetDetails() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objPrjFinance.GetProjectBudgetDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.budgetAmt = result.data[0].Budget;
                        this.GroupID = result.data[0].GroupId;
                        this.budgetHeadAmount =
                            result.data[0].BudgetHeadAmount != null
                                ? result.data[0].BudgetHeadAmount
                                : 0;

                        this.lstBudgetFinUserInfo = result.data.filter(
                            (x) => x.UserType == "F"
                        );
                        this.lstBudgetDonorUserInfo = result.data.filter(
                            (x) => x.UserType == "D"
                        );
                        var DonorBudgetFund = 0
                        for (var i = 0; i < this.lstBudgetDonorUserInfo.length; i++) {
                            DonorBudgetFund = DonorBudgetFund + this.lstBudgetDonorUserInfo[i].DonorBudgetAmount
                            this.getRandomColor();
                            this.lstBudgetDonorUserInfo[i].color = this.fixedColor[i] || this.mycolor;
                        }
                        this.DonorNotFundedFund = parseInt(this.budgetAmt) - parseInt(DonorBudgetFund);
                        this.UpdateBudgetAndFundChart(this.lstBudgetDonorUserInfo);
                    }
                }
            },
            async UpdateBudgetAndFundChart(lstDonor) {
                var lstLabels = ["DonorNotFundedFund"];
                var lstDonorAmt = [this.DonorNotFundedFund];
                var lstBgColor = ["#E5E5E5"];
                for (var i = 0; i < lstDonor.length; i++) {
                    lstLabels[i + 1] = lstDonor[i] ? lstDonor[i].UserName : "";
                    lstDonorAmt[i + 1] = lstDonor[i] ? lstDonor[i].DonorBudgetAmount : "";
                    lstBgColor[i + 1] = lstDonor[i] ? lstDonor[i].color : "";
                }

                var ctx = document.getElementById("myBudgetAndFundChart");
                var stateOverviewGraph2 = new Chart(ctx, {
                    type: "doughnut",
                    data: {
                        labels: lstLabels,
                        datasets: [
                            {
                                data: lstDonorAmt,
                                backgroundColor: lstBgColor,
                            },
                        ],
                    },
                    options: {
                        legend: {
                            display: false,
                        },
                    },
                });
            },
            async GetProjectUsers(Type) {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("GroupID", this.GroupID);
                data.append("Type", Type);
                var result = await objPrjFinance.GetProjectUsers(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        if (Type == "Finance") this.lstFinanceUsers = result.data;
                        else if (Type == "Donor") this.lstDonors = result.data;
                        for (var i = 0; i < this.lstDonors.length; i++)
                            this.lstDonors[i].lstDonorAccount = [];
                    }
                }
            },
            async SaveProjectBudgetsDetails(UserType) {
                this.PreLoader = true;
                var usr = "";
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                if (UserType == "F") {
                    data.append("ProjectUsers", JSON.stringify(this.vmFinanceUsers));
                    usr = "Finance user";
                } else {
                    data.append("ProjectUsers", JSON.stringify(this.vmlstDonors));
                    usr = "Donor user";
                }
                data.append("chkNotify", this.ChkFinNotify);
                var result = await objPrjFinance.SaveProjectBudgetsDetails(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = usr + " added successfully";
                        this.successSnackbarMsg = true;
                    }

                    else {
                        this.Msg = "Failed in adding " + usr + " ...";
                        this.errorSnackbarMsg = true;
                    }
                    
                }
                this.AssignFinUserPopUp = false;
                this.AddDonorPopUp = false;
                //this.successSnackbarMsg = true;
                this.GetProjectBudgetDetails();
                this.UpdateBudgetAndFundChart(this.vmlstDonors);
                this.PreLoader = false;
            },

            AssignDonorStep2() {
                var errCount = 0;
                for (var i = 0; i < this.vmlstDonors.length; i++)
                    if (this.vmlstDonors[i].DonorBudgetAmount == 0) errCount++;
                if (errCount > 0) {
                    this.isValidBudget = 0;
                    this.errorMsg = " * Please add Budget Amount";
                } else {
                    this.isValidBudget = 1;
                    this.errorMsg = "";
                }

                this.AssignDonarSteps = 2;
            },
            async GetDonorDetails(userItem) {
                this.vmDonorAccount.AccountId = this.lstDonors.filter(
                    (x) => x.UserID == userItem.UserID
                )[0].AccountId;
                this.vmDonorAccount.AccountName = this.lstDonors.filter(
                    (x) => x.UserID == userItem.UserID
                )[0].AccountName;

                //--- Get Account Details ----
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("UserID", userItem.UserID);
                var result = await objPrjFinance.GetDonorAccountDetails(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstDonorAccount = result.data;
                        this.lstDonors.filter(
                            (x) => x.UserID == userItem.UserID
                        )[0].lstDonorAccount = result.data;
                    }
                }
            },
            AddBudgetAmt(userID, Amt) {
                this.vmlstDonors.filter(
                    (x) => x.UserID == userID
                )[0].IsBudgetAmtAdded = true;
                this.errorMsg = "";

                var i = 0;
                var totDonorsAmt = 0;
                var lstUsers = this.vmlstDonors.filter((x) => x.UserID != userID);

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
                var sumBudgetAmt = parseInt(totDonorsAmt) + parseInt(Amt);
                if (parseInt(this.budgetAmt) > sumBudgetAmt) {
                    var balAmt =
                        (parseInt(this.budgetAmt) -
                            (parseInt(totDonorsAmt) + parseInt(Amt))) /
                        lstUsers.length;
                    for (i = 0; i < lstUsers.length; i++)
                        this.vmlstDonors.filter(
                            (x) => x.UserID == lstUsers[i].UserID
                        )[0].DonorBudgetAmount = balAmt;
                    this.isValidBudget = 1;
                } else {
                    this.isValidBudget = 0;
                    this.errorMsg =
                        "Total donor budget amount should not be greater than " +
                        this.budgetAmt;
                    this.vmlstDonors.filter(
                        (x) => x.UserID == userID
                    )[0].DonorBudgetAmount = 0;
                }
            },
            /*

                    AddAccount(userID) {
                        this.lstDonors.filter(x => x.UserID == userID)[0].AccountId = this.vmDonorAccount.AccountId;
                        this.lstDonors.filter(x => x.UserID == userID)[0].AccountName = this.vmDonorAccount.AccountName;
                    },

                    */
            AddAccount(userID, Account) {
                this.lstDonors.filter((x) => x.UserID == userID)[0].AccountId =
                    Account.AccountId; //this.vmDonorAccount.AccountId;
                this.lstDonors.filter((x) => x.UserID == userID)[0].AccountName =
                    Account.AccountName; //this.vmDonorAccount.AccountName;
            },
            //==================================================================//
            //=================Fund Utilization===============================//

            async GetFundPaymentDeatils() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objPrjFinance.GetFundPaymentDeatils(data);
                var totAmount = 0;

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstFundPaymentDeatils = result.data;
                        if (this.lstFundPaymentDeatils.length > 0) {
                            for (var i = 0; i < this.lstFundPaymentDeatils.length; i++) {
                                this.getRandomColor();
                                this.lstFundPaymentDeatils[i].color = this.fixedColor[i] || this.mycolor;
                                totAmount =
                                    totAmount + parseInt(this.lstFundPaymentDeatils[i].HeadAmount);
                            }
                            this.UnUtilizedAmt = this.budgetAmt - totAmount;
                            this.UpdateFundUtilisationChart(this.lstFundPaymentDeatils);
                            this.IniBudgetHead = this.lstFundPaymentDeatils[0].BudgetHead;
                            this.lstFundExpenses = this.lstFundPaymentDeatils[0].FundPaymentList;
                        }
                    }
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
            async UpdateFundUtilisationChart(lstFundPaymentDeatils) {
                var lstLabels = ["Unutilized Fund"];
                var lstPlanAmt = [this.UnUtilizedAmt];
                var lstBgColor = ["#E5E5E5"];
                for (var i = 0; i < lstFundPaymentDeatils.length; i++) {
                    lstLabels[i + 1] =
                        lstFundPaymentDeatils[i].BudgetHead != null
                            ? lstFundPaymentDeatils[i].BudgetHead
                            : "";
                    lstPlanAmt[i + 1] = lstFundPaymentDeatils[i]
                        ? lstFundPaymentDeatils[i].HeadAmount
                        : "";
                    lstBgColor[i + 1] = lstFundPaymentDeatils[i]
                        ? lstFundPaymentDeatils[i].color
                        : "";
                }

                var ctx = document.getElementById("fundUtilizationChart");
                var stateOverviewGraph2 = new Chart(ctx, {
                    type: "doughnut",
                    data: {
                        labels: lstLabels,
                        datasets: [
                            {
                                data: lstPlanAmt,
                                backgroundColor: lstBgColor,
                            },
                        ],
                    },
                    options: {
                        legend: {
                            display: false,
                        },
                    },
                });
            },
            GetFundUtilisationList() {
                this.lstFundExpenses = this.lstFundPaymentDeatils.filter(
                    (x) => x.BudgetHeadID == this.vmFUBudgetHead.BudgetHeadID
                )[0].FundPaymentList;
            },
            //==================================================================//
            //=================Planned & Actual Chart ==========================//
            async fillPlannedVsActualChart() {
                var lstFundExp = this.lstFundPaymentDeatils;
                var lstFundPlan = this.lstBudgetPlanDetails;

                var lstPlanLabels = [];
                var lstPlanAmt = [];
                var lstPlanBgColor = [];

                var lstActualabels = [];
                var lstActualAmt = [];
                var lstActualBgColor = [];

                for (var i = 0; i < lstFundExp.length; i++) {
                    for (var j = 0; j < lstFundPlan.length; j++) {
                        if (lstFundPlan[j].BudgetHead == lstFundExp[i].BudgetHead) {
                            // -- Planned Graph details
                            lstPlanLabels.push(lstFundPlan[j] ? lstFundPlan[j].BudgetHead : "");
                            lstPlanAmt.push(lstFundPlan[j] ? lstFundPlan[j].HeadAmount : "");
                            lstPlanBgColor.push("#0202AA");

                            // -- Actual Grap Details
                            lstActualabels.push(lstFundExp[i] ? lstFundExp[i].BudgetHead : "");
                            lstActualAmt.push(
                                lstFundExp[i] ? parseFloat(lstFundExp[i].HeadAmount) : ""
                            );
                            lstActualBgColor.push("#89D8B0");
                            break;
                        }
                    }
                }

                var ctx = document.getElementById("myBarChart");
                var myChart = new Chart(ctx, {
                    type: "horizontalBar",
                    data: {
                        datasets: [
                            {
                                label: ["Planned"],
                                data: lstPlanAmt,
                                backgroundColor: lstPlanBgColor,
                                barPercentage: 0.5,
                                barThickness: 13,
                                maxBarThickness: 20,
                                minBarLength: 2,
                            },
                            {
                                label: ["Actual"],
                                data: lstActualAmt,
                                backgroundColor: lstActualBgColor,
                                barPercentage: 0.5,
                                barThickness: 17,
                                maxBarThickness: 13,
                                minBarLength: 2,
                            },
                        ],

                        labels: lstActualabels,
                    },
                    options: {
                        responsive: false,
                        scales: {
                            xAxes: [
                                {
                                    ticks: {
                                        maxRotation: 90,
                                        minRotation: 80,
                                        beginAtZero: true,
                                    },
                                },
                            ],
                            yAxes: [
                                {
                                    ticks: {
                                        beginAtZero: true,
                                    },
                                },
                            ],
                        },
                    },
                });
            },

            //==================================================================//
            //=================Budget Plan======================================//
            async GetBudgetPlanDeatils() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objPrjFinance.GetProjectBudgetPlanDetails(data);
                this.UnaccountedAmt = this.budgetAmt
                this.UnaccountedAmtCpy = this.budgetAmt
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstBudgetPlanDetails = result.data;
                        var sum = 0;
                        for (var i = 0; i < this.lstBudgetPlanDetails.length; i++) {
                            sum = sum + this.lstBudgetPlanDetails[i].HeadAmount
                            this.UnaccountedAmt = this.budgetAmt - sum
                            this.UnaccountedAmtCpy = this.budgetAmt - sum
                            this.getRandomColor();
                            this.lstBudgetPlanDetails[i].color = this.fixedColor[i] || this.mycolor;
                        }

                        this.UpdateBudgetPlanChart(this.lstBudgetPlanDetails);

                        // For Tree List
                        this.lstBudgetHeaderDetails = [];
                        for (var i = 0; i < this.lstBudgetPlanDetails.length; i++) {
                            var SubItem = [];
                            for (
                                var j = 0;
                                j < this.lstBudgetPlanDetails[i].BudgetPlanSubList.length;
                                j++
                            )
                                SubItem.push({
                                    BudgetHeadID: this.lstBudgetPlanDetails[i].BudgetHeadID,
                                    BudgetHeadItemID: this.lstBudgetPlanDetails[i]
                                        .BudgetPlanSubList[j].ID,
                                    HeadItemName: this.lstBudgetPlanDetails[i].BudgetPlanSubList[j]
                                        .ItemName,
                                    Amount: this.lstBudgetPlanDetails[i].BudgetPlanSubList[j]
                                        .Amount,
                                });
                            this.lstBudgetHeaderDetails.push({
                                BudgetHeadID: this.lstBudgetPlanDetails[i].BudgetHeadID,
                                HeadName: this.lstBudgetPlanDetails[i].BudgetHead,
                                Amount: this.lstBudgetPlanDetails[i].HeadAmount,
                                children: SubItem,
                            });
                        }
                    }
                }
            },
            async UpdateBudgetPlanChart(lstBudgetPlanDetails) {
                var lstLabels = ["Unaccounted"];
                var lstPlanAmt = [this.UnaccountedAmt];
                var lstBgColor = ["#E5E5E5"];
                for (var i = 0; i < lstBudgetPlanDetails.length; i++) {
                    lstLabels[i + 1] = lstBudgetPlanDetails[i]
                        ? lstBudgetPlanDetails[i].BudgetHead
                        : "";
                    lstPlanAmt[i + 1] = lstBudgetPlanDetails[i]
                        ? lstBudgetPlanDetails[i].HeadAmount
                        : "";
                    lstBgColor[i + 1] = lstBudgetPlanDetails[i]
                        ? lstBudgetPlanDetails[i].color
                        : "";
                }

                var ctx = document.getElementById("myBudgetPlan");
                var stateOverviewGraph2 = new Chart(ctx, {
                    type: "doughnut",
                    data: {
                        labels: lstLabels,
                        datasets: [
                            {
                                data: lstPlanAmt,
                                backgroundColor: lstBgColor,
                            },
                        ],
                    },
                    options: {
                        legend: {
                            display: false,
                        },

                    },
                });
            },

            AddHeaderSubItem(p) {
                if (this.$refs.budgetForm.validate() === true) {
                    this.lstBudgetPlanItems.push({});
                    this.$refs.budgetForm.resetValidation()
                    this.UnaccountedAmt = this.UnaccountedAmt - parseInt(p.Amount)
                }
            },
            DeleteSubItem: function (SubItem) {
                this.UnaccountedAmt = this.UnaccountedAmt + (Number(SubItem.Amount) || 0)
                this.lstBudgetPlanItems.splice(
                    this.lstBudgetPlanItems.indexOf(SubItem),
                    1
                );
            },

            AddBudgetPlanAmt(Amt) {
                var sum = 0;
                for (var i = 0; i < this.lstBudgetPlanItems.length; i++) {
                    sum = sum + parseInt(this.lstBudgetPlanItems[i].Amount || 0);
                }
                this.TotalHeadAmt = sum;
                //this.TotalHeadAmt = parseFloat(this.TotalHeadAmt) + parseFloat(Amt || 0);
                this.TotalBudgetPlanAmt = parseInt(this.TotalBudgetPlanAmt) + parseInt(Amt || 0);
                //if (this.$refs.budgetForm.validate() === true) {
                //this.UnaccountedAmt = this.UnaccountedAmt - parseInt(Amt || 0)
                //}

            },

            async fnSetpperIncrementBudgetAndPlan(Mode) {
                if (Mode == "Plus") {
                    if (this.$refs.budgetForm.validate() === true) {
                        this.AddBudgetPlanAmt()
                        this.BudgetAndPlanstepper = this.BudgetAndPlanstepper + 1;
                        this.GetBudgetPlanItemList(this.BudgetAndPlanstepper);
                        this.$refs.budgetForm.resetValidation()
                    }
                } else if (Mode == "Minus") {
                    this.BudgetAndPlanstepper = this.BudgetAndPlanstepper - 1;
                    this.GetPrevBudgetPlanItemList(this.BudgetAndPlanstepper);
                    this.$refs.budgetForm.resetValidation()
                }
            },
            async GetBudgetPlanItemList(index) {
                if (this.lstBudgetPlan.length > 0) {
                    var LastItemIndex = this.lstBudgetPlan[this.lstBudgetPlan.length - 1].Index
                }
                if (index > 0) {
                    var temp = this.lstBudgetPlan.filter((x) => x.Index == index);
                    if (temp.length > 0) {
                        this.BudgetHeadName = temp[0].BudgetHeadName;
                        this.TotalHeadAmt = temp[0].HeadAmount;
                        this.lstBudgetPlanItems = temp[0].BudgetPlanSubList;
                    } else if ((index - 1) !== this.LastItemIndex) {
                        var subItems = this.lstBudgetPlanItems.filter(
                            (x) => x.SubHeaderName != null
                        );
                        this.lstBudgetPlan.push({
                            Index: index - 1,
                            BudgetHeadName: this.BudgetHeadName,
                            HeadAmount: this.TotalHeadAmt,
                            BudgetPlanSubList: subItems,
                        });
                        this.UnaccountedAmt = this.UnaccountedAmt - parseInt(subItems[subItems.length - 1].Amount)
                        this.BudgetHeadName = "";
                        this.TotalHeadAmt = 0;
                        this.lstBudgetPlanItems = [{ SubHeaderName: "", Amount: "" }];
                    } else {
                        this.BudgetHeadName = "";
                        this.TotalHeadAmt = 0;
                        this.lstBudgetPlanItems = [{ SubHeaderName: "", Amount: "" }];
                    }
                }

            },
            async GetPrevBudgetPlanItemList(index) {
                if (index + 1 == this.NoOfBudgetPlans) {
                    if (this.BudgetHeadName != "") {
                        this.lstBudgetPlan.push({
                            Index: this.NoOfBudgetPlans,
                            BudgetHeadName: this.BudgetHeadName,
                            HeadAmount: this.TotalHeadAmt,
                            BudgetPlanSubList: this.lstBudgetPlanItems,
                        });
                    }
                }
                if (index > 0) {
                    var temp = this.lstBudgetPlan.filter((x) => x.Index == index);
                    if (temp.length > 0) {
                        this.BudgetHeadName = temp[0].BudgetHeadName;
                        this.TotalHeadAmt = temp[0].HeadAmount;
                        this.lstBudgetPlanItems = temp[0].BudgetPlanSubList;
                    }
                }
            },
            fnChkBudgetPlanMandatory() {
                var t = this.lstBudgetPlanItems.filter(
                    (x) => x.SubHeaderName != null || x.Amount != 0
                );
                if (t.length == 0 || this.BudgetHeadName == "") return false;
                else return true;
            },
            async SaveProjectBudgetPlan() {
                if (this.$refs.budgetForm.validate() === true) {
                    this.$refs.budgetForm.resetValidation()
                    this.GetBudgetPlanItemList(this.NoOfBudgetPlans);
                    var TotalNoOfBudgetHead =
                        this.lstBudgetPlanDetails.length + parseInt(this.NoOfBudgetPlans);
                    var data = new FormData();
                    data.append("ProjectID", this.ProjectID);
                    data.append("NoOfBudgetPlan", TotalNoOfBudgetHead);
                    data.append("BudgetPlanList", JSON.stringify(this.lstBudgetPlan));
                    var result = await objPrjFinance.SaveProjectBudgetPlan(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        this.Msg = " Budget Plan successfully added";
                        this.successSnackbarMsg = true;
                        this.lstBudgetHeaderDetails = [];
                        this.GetBudgetPlanDeatils();
                        this.CloseBudgetPlanDialog();
                    } else {
                        this.Msg = " Failed in adding Budget Plan...";
                        this.errorSnackbarMsg = true;
                    }
                }
            },
            CloseBudgetPlanDialog() {
                this.NoOfBudgetPlans = "";
                this.BudgetHeadName = "";
                this.dlgCreateBudgetPlan = false;
                this.CreateBudgetAndPlanStepOne = true;
                this.CreateBudgetAndPlanStepTwo = false;
                this.lstBudgetPlan = [];
                this.BudgetAndPlanstepper = 1;
                this.$refs.budgetForm.resetValidation()
                this.UnaccountedAmt = this.UnaccountedAmtCpy
                this.lstBudgetPlanItems = [{}]
            },
            async UpdateBudgetPlanHead() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("BudgetHeadID", this.vmEBudgetHeadID);
                data.append("BudgetHead", this.vmEBudgetHead);

                var result = await objPrjFinance.UpdateProjectBudgetPlan(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.lstBudgetHeaderDetails = [];
                        this.GetBudgetPlanDeatils();
                        this.Msg = "Budget Head Successfully updated..";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed in updating Budget Head...";
                        this.errorSnackbarMsg = true;
                    }
                        

                    
                    this.dlgEditBudgetPlan = false;
                    this.vmEBudgetHeadID = "";
                    this.vmEBudgetHead = "";
                }
            },
            async UpdateBudgetPlanHeadItems() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("BudgetHeadID", this.vmEBudgetHeadID);
                data.append("BudgetHeadItemID", this.vmEBudgetHeadItemID);
                data.append("BudgetHeadItemName", this.vmEBudgetHeadItemName);
                data.append("ItemAmount", this.vmEBudgetHeadItemAmount);

                var result = await objPrjFinance.UpdateProjectBudgetPlanSubList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.lstBudgetHeaderDetails = [];
                        this.GetBudgetPlanDeatils();
                        this.Msg = "Budget Head SubItem Successfully updated..";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed in updating Budget Head SubItem...";
                        this.errorSnackbarMsg = true;
                    }
                        

                    
                    this.ClearEditBudgetPlanItems();
                }
            },
            ClearEditBudgetPlanItems() {
                this.vmEBudgetHeadItemID = "";
                this.vmEBudgetHeadItemName = "";
                this.vmEBudgetHeadItemAmount = "";
                this.dlgEditBudgetPlanSubHeader = false;
            },
            async RemoveBudgetPlanHead() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("BudgetHeadID", this.vmEBudgetHeadID);

                var result = await objPrjFinance.RemoveBudgetPlanHead(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.lstBudgetHeaderDetails = [];
                        this.GetBudgetPlanDeatils();
                        this.Msg = "Budget Head Successfully removed..";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed in removing Budget Head...";
                        this.errorSnackbarMsg = true;
                    }
                        

                    
                    this.dlgRemoveHeader = false;
                    this.vmEBudgetHeadID = "";
                    this.vmEBudgetHead = "";
                }
            },
            async RemoveBudgetPlanSubList() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("BudgetHeadID", this.vmEBudgetHeadID);
                data.append("BudgetHeadItemID", this.vmEBudgetHeadItemID);

                var result = await objPrjFinance.RemoveBudgetPlanSubList(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.lstBudgetHeaderDetails = [];
                        this.GetBudgetPlanDeatils();
                        this.Msg = "Budget Head SubItem removed..";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed in removing Budget Head SubItem...";
                        this.errorSnackbarMsg = true;
                    }
                       

                    
                    this.dlgRemoveSubItem = false;
                    this.vmEBudgetHeadID = "";
                    this.vmEBudgetHeadItemID = "";
                    this.vmEBudgetHeadItemName = "";
                }
            },
            //==================================================================//
            //====================Payment Terms================================//

            async fnGetPaymentTermsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objPrjFinance.GetPaymentTermsByprjID(data);
                // this.PaymentTermLeftAmt = this.budgetAmt
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.data) this.PaymentTermsitem = result.data;
                    var sum = 0;
                    for (var i = 0; i < this.PaymentTermsitem.length; i++) {
                        sum = sum + parseInt(this.PaymentTermsitem[i].Amount)

                        if (this.PaymentTermsitem[i].PaymentStatus == "Done") {
                            this.PaymentTermsitem[i].Color = "#47BB76";
                            this.PaymentTermsitem[i].Icon = "mdi-check";
                        } else if (this.PaymentTermsitem[i].PaymentStatus == "Cancelled") {
                            this.PaymentTermsitem[i].Color = "#D50808";
                            this.PaymentTermsitem[i].Icon = "mdi-close";
                        } else if (this.PaymentTermsitem[i].PaymentStatus == "Pending") {
                            this.PaymentTermsitem[i].Color = "#d4d6d5";
                            this.PaymentTermsitem[i].Icon = "";
                        }
                    }
                    this.dcExistingPymtTermSum = sum;
                    this.PaymentTermLeftAmt = this.budgetAmt - sum
                }
            },
            async GetProjectPTUsers(Type) {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("GroupID", this.GroupID);
                data.append("Type", Type);
                var result = await objPrjFinance.GetProjectUsers(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        if (Type == "Finance") this.lstPTFinanceUsers = result.data;
                        else if (Type == "Donor") this.lstPTDonors = result.data;
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
                    //if (this.selectedNotifyUsers.filter(x => x.UserID == tmpList[i].UserID).length == 0)
                    this.selectedNotifyUsers.push(tmpList[i]);
                }
                this.NotificationSelectPopUp = false;
            },
            async fnSetpperIncrement(Mode) {
                if (Mode == "Plus") {
                    if (this.$refs.paymentForm.validate() === true && this.fnCheckMandatory() === true) {
                        //this.stepper = this.stepper + 1;
                        this.fnInvokePaymentTermList(this.stepper);
                        this.stepper = this.stepper + 1;
                        this.$refs.paymentForm.resetValidation()
                    }
                } else if (Mode == "Minus") {
                    this.stepper = this.stepper - 1;
                    this.fnInvokePrevPaymentTermList(this.stepper);
                    //this.stepper = this.stepper - 1;
                }
            },
            async fnInvokePaymentTermList(index) {
                // debugger;
                // this.PaymentTermLeftAmt = this.PaymentTermLeftAmt - this.amount
                if (index > 0) {
                    // debugger;
                    var temp = this.PaymentTerm.PaymentTermList.filter(
                        (x) => x.Index == index + 1
                    );
                    if (temp.length > 0) {
                        this.PaymentTermName = temp[0].PaymentTermName;
                        this.CreatePaymentTermsLogicModel = temp[0].Logic;
                        this.paymentdate = temp[0].paymentdate;
                        this.Target = temp[0].Target;
                        this.amount = temp[0].amount;
                        this.selectedNotifyUsers = temp[0].NotifyUserList;
                        for (var i = 0; i < this.selectedNotifyUsers.length; i++) {
                            if (this.selectedNotifyUsers[i].UserType == "F")
                                this.vmPTFinanceUsers.push(this.selectedNotifyUsers[i]);
                            else if (this.selectedNotifyUsers[i].UserType == "D")
                                this.vmPTDonors.push(this.selectedNotifyUsers[i]);
                        }
                    } else {
                        // check data exist in the same index
                        var IsExistTemp = this.PaymentTerm.PaymentTermList.filter((x) => x.Index == index);
                        if (IsExistTemp.length > 0) {
                            const findIndex = this.PaymentTerm.PaymentTermList.findIndex((element, idx) => {
                                if (element.Index === index) {
                                    return true
                                }
                            });
                            if (findIndex > -1) {
                                this.PaymentTerm.PaymentTermList.splice(findIndex, 1);
                                //this.lstSections = [...this.lstSections];
                            }
                        }
                        this.PaymentTerm.PaymentTermList.push({
                            Index: index,
                            PaymentTermName: this.PaymentTermName,
                            Logic: this.CreatePaymentTermsLogicModel,
                            paymentdate: this.paymentdate,
                            Target: this.Target,
                            amount: this.amount,
                            NotifyUserList: this.selectedNotifyUsers,
                        });
                        this.PaymentTermName = "";
                        this.paymentdate = new Date().toISOString().substr(0, 10);
                        this.Target = "";
                        this.amount = "";
                        this.CreatePaymentTermsLogicModel = "";
                        this.selectedNotifyUsers = [];
                        this.vmPTDonors = [];
                        this.vmPTFinanceUsers = [];
                    }
                }
            },
            async fnInvokePrevPaymentTermList(index) {
                // debugger;
                if (index + 1 == this.NoOfPayments) {
                    if (this.PaymentTermName != "") {
                        this.PaymentTerm.PaymentTermList.push({
                            Index: this.NoOfPayments,
                            PaymentTermName: this.PaymentTermName,
                            Logic: this.CreatePaymentTermsLogicModel,
                            paymentdate: this.paymentdate,
                            Target: this.Target,
                            amount: this.amount,
                            NotifyUserList: this.selectedNotifyUsers,
                        });
                        this.vmPTDonors = [];
                        this.vmPTFinanceUsers = [];
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
                        this.selectedNotifyUsers = temp[0].NotifyUserList;
                        for (var i = 0; i < this.selectedNotifyUsers.length; i++) {
                            if (this.selectedNotifyUsers[i].UserType == "F")
                                this.vmPTFinanceUsers.push(this.selectedNotifyUsers[i]);
                            else if (this.selectedNotifyUsers[i].UserType == "D")
                                this.vmPTDonors.push(this.selectedNotifyUsers[i]);
                        }
                    }
                }
            },

            fnCheckMandatory() {
                // debugger;
                //this.amountErr = false;
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
            fnCheckMandatoryBeforeSave() {
                //debugger;
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
                // debugger;
                // to validate the varibale in the last index of an array
                var length = this.PaymentTerm.PaymentTermList.length;
                if (length == 0) {
                    if (this.stepper < this.NoOfPayments) {
                        return false;
                    }
                }
                //debugger;
                return true;
            },
            async fnSavepaymentTerms() {

                if (this.$refs.paymentForm.validate() === true && this.fnCheckMandatoryBeforeSave() === true) {
                    this.CreatePaymentTermsDialog = false;
                    this.fnInvokePaymentTermList(this.NoOfPayments);
                    var data = new FormData();
                    data.append("NoOfPaymentTerms", this.NoOfPayments);
                    data.append(
                        "PaymentTermList",
                        JSON.stringify(this.PaymentTerm.PaymentTermList)
                    );
                    //data.append("Notify",  this.NotifyCheckbox);
                    data.append("ProjectId", this.ProjectID);
                    var result = await objPrjFinance.SavepaymentTerms(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.status == "success") {
                        this.Msg = " Payment term created successfully ";
                        this.successSnackbarMsg = true;
                        this.ClosepaymentDialog();
                        this.fnGetPaymentTermsByprjID();
                        this.$refs.paymentForm.resetValidation();
                    } else {
                        this.Msg = " Failed in adding Payment term ..";
                        this.errorSnackbarMsg = true;
                    }
                }
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
                this.$refs.paymentForm.resetValidation()
                this.PaymentTermLeftAmt = (parseInt(this.budgetAmt) - parseInt(this.dcExistingPymtTermSum));
                this.amountErr = false;
            },
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objManageProject.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
                    this.ProjectEndDate = this.projectInfo.EndDate.substr(0, 10); //this.formatDate(this.projectInfo.EndDate.substr(0, 10));
                    this.ProjectStartDate = this.projectInfo.StartDate.substr(0, 10); // this.formatDate(this.projectInfo.StartDate.substr(0, 10));
                } // else {
                //    alert("Failed to get details of Project " + this.projectInfo.ProjectId);
                //}
            },

            AddPaymentTermAmt(Amt) {

                // debugger;
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
                    // debugger;
                    this.amountErr = true;
                    return false;
                }
                this.PaymentTermLeftAmt = parseInt(this.PaymentTermLeftAmt) - parseInt(Amt || 0);
                this.totermsAmt = parseInt(this.totermsAmt) + parseInt(Amt || 0);
                //if (
                //    parseInt(this.budgetAmt) <
                //    parseInt(this.projectInfo.PaymentTermAmount) + parseInt(this.totermsAmt)
                //){
                //    this.Msg =
                //        "Total Budget Amount should not be greater than the sum of PaymentTerm Amount";
                //    this.snackbarAlertMsg = true;
                //    this.totermsAmt = parseInt(this.totermsAmt) - parseInt(Amt);
                //    this.amount = "";
                //}
            },
            fnCheckEditMandatory() {
                if (
                    this.PaymentInfo.PaymentTermName == "" ||
                    this.PaymentInfo.Logic == "" ||
                    this.PaymentInfo.Amount == "" ||
                    this.PaymentInfo.Amount == 0
                ) {
                    if (this.PaymentInfo.Logic == "Date") {
                        this.PaymentInfo.paymentdate == "";
                    } else {
                        this.PaymentInfo.Target == "";
                    }
                    return true;
                } else return false;
            },
            async GetPaymentTermsNotifyUsers() {
                this.vmPTDonors = [];
                this.vmPTFinanceUsers = [];

                for (var i = 0; i < this.PaymentInfo.users.length; i++) {
                    for (var j = 0; j < this.lstPTFinanceUsers.length; j++)
                        if (
                            this.PaymentInfo.users[i].UserID == this.lstPTFinanceUsers[j].UserID
                        )
                            this.vmPTFinanceUsers.push(this.PaymentInfo.users[i]);
                    for (var j = 0; j < this.lstPTDonors.length; j++)
                        if (this.PaymentInfo.users[i].UserID == this.lstPTDonors[j].UserID)
                            this.vmPTDonors.push(this.PaymentInfo.users[i]);
                }
            },
            async fnGetPaymentTermsByPaymentId(paymentID) {
                var data = new FormData();
                data.append("PaymentId", paymentID);
                var result = await objPrjFinance.GetPaymentTermsByPaymentId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.PaymentInfo = result.data;
                    this.PaymentInfo.users = JSON.parse(result.data.users);
                    this.PaymentId = this.PaymentInfo.PaymentId;
                    this.PaymentTermName = this.PaymentInfo.PaymentTermName;
                }
                this.EditPaymentTermsDialog = true;
            },
            async fnUpdatepaymentTerms() {
                if (this.$refs.editPaymentForm.validate() === true && this.fnCheckEditMandatory() === false) {
                    this.EditPaymentTermsDialog = false;
                    var data = new FormData();
                    data.append("PaymentId", this.PaymentInfo.PaymentId);
                    data.append("PaymentTermName", this.PaymentInfo.PaymentTermName);
                    data.append("Logic", this.PaymentInfo.Logic);
                    data.append("paymentdate", this.PaymentInfo.paymentdate);
                    data.append("Target", this.PaymentInfo.Target);
                    data.append("Amount", this.PaymentInfo.Amount);
                    data.append("users", JSON.stringify(this.PaymentInfo.users));
                    data.append("ProjectId", this.ProjectID);
                    var result = await objPrjFinance.UpdatepaymentTerms(data);
                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else {
                        if ((result.status = "Success")) {
                            this.Msg = "Payment term successfully updated";
                            this.successSnackbarMsg = true;
                            this.fnGetPaymentTermsByprjID();
                            this.EditPaymentTermsDialog = false;
                            this.PaymentInfo.users = [];
                            this.selectedNotifyUsers = [];
                            this.PaymentId = "";
                            this.PaymentTermName = "";
                        }
                    }
                }
            },
            fnCheckEditMandatory() {
                if (
                    this.PaymentInfo.PaymentTermName == "" ||
                    this.PaymentInfo.Logic == "" ||
                    this.PaymentInfo.Amount == ""
                ) {
                    if (this.PaymentInfo.Logic == "Date") {
                        this.PaymentInfo.paymentdate == "";
                    } else {
                        this.PaymentInfo.Target == "";
                    }
                    return true;
                } else return false;
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
            async DeletePaymentTerm() {
                var data = new FormData();
                data.append("PaymentId", JSON.stringify(this.PaymentId));
                data.append("Type", 2);
                var result = await objPrjFinance.DeletePaymentTerm(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if ((result.status = "Success")) {
                        this.Msg = `Payment term ${this.PaymentTermName} removed successfully`;
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed in removing Payment term ...";
                        this.errorSnackbarMsg = true;
                    }
                }
                this.fnGetPaymentTermsByprjID();
                this.PaymentId = 0;
                this.PaymentTermName = "";
            },
            async SavePaymentRequest() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                data.append("PaymentID", this.PaymentId);
                data.append("DonorUser", this.RequestFundDonorUser);
                data.append("RequestAmount", this.RequestAmount);
                data.append("Comment", this.RequestComment);
                if(this.fileInput !=null && this.fileInput != undefined)
                {
                    data.append("ReqDocument", this.fileInput.name);
                }
                data.append("ProjectName", this.projectInfo.ProjectName);
                var result = await objPrjFinance.SavePaymentRequest(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.UploadDocument();
                        this.Msg = " Payment Requested successfully ";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed to add payment request ";
                        this.errorSnackbarMsg = false;
                    }

                    this.ClearRequestPayment();
                }
                this.PreLoader = false;
            },
            ClearRequestPayment() {
                this.dlgRequestPayment = false;
                this.RequestFundDonorUser = [];
                this.RequestAmount = 0;
                this.RequestComment = "";
                this.fileInput = null;
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return "0 Bytes";
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + " " + sizes[i];
            },
            async UploadDocument() {
                const formData = new FormData();
                formData.append("myFile", this.fileInput);
                formData.append(
                    "DocumentOriginalName",
                    this.fileInput.name
                        .split(".")
                        .slice(0, -1)
                        .join(".")
                );
                formData.append(
                    "DocumentSize",
                    this.formatFileSize(this.fileInput.size, 0)
                );
                formData.append("DocumentType", this.fileInput.type);
                formData.append("DocumentTypeIcon", this.fileInput.name.split(".").pop());
                if (this.fileInput.lastModifiedDate != null) {
                    var lstModDate =
                        new Date(this.fileInput.lastModifiedDate)
                            .toISOString()
                            .substr(0, 10) +
                        " " +
                        new Date(this.fileInput.lastModifiedDate).toISOString().substr(11, 5);
                    formData.append("DocumentLastModifiedDate", lstModDate);
                }
                formData.append("ProjectId", this.ProjectID);
                formData.append("UploadedFrom", "Project");
                formData.append("LinkedTo", this.ProjectName);

                var result = await objPrjFinance.UploadDocument(formData);

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
            IsValidPaymentDate() {
                if (
                    Date.parse(this.paymentdate) <
                    Date.parse(this.ProjectStartDate.substr(0, 10))
                ) {
                    this.Msg =
                        "Invalid Date : Payment Date should be greater than Project Start Date  - " +
                        this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.FrmDate = "";
                    return;
                }

                if (
                    Date.parse(this.paymentdate) >
                    Date.parse(this.ProjectEndDate.substr(0, 10))
                ) {
                    this.Msg =
                        "Invalid Date : Payment Date should be less than Project End Date- " +
                        this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.FrmDate = "";
                    return;
                }
                return true;
            },
            //==================================================================//
            //===================Audit Frame===================================//
            async fnGetAuditLog() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objPrjFinance.GetAuditLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstLogFrame = result.data;
                    }
                }
            },
            //==================================================================//
        },
        computed: {
            ...mapGetters('DefaultStore',["getSelectedCurrency"]),
        },
    };
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
