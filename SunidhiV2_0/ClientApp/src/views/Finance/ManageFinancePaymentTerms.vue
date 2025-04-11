<template>
    <v-card class="custom-card-1">
        <v-card-title style="height:73px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary">mdi-cash-marker</v-icon>
                </v-avatar>
                <router-link :to="{ name: 'FinanceBudgetAndFunds', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeID} }" class="page-head text-link">Finance Details</router-link></span> > Payment Terms
            </h5>
            <v-spacer></v-spacer>
            <Success-Button id="AutoTest_F199" class="mr-4" title="Add Payment Term" @click.native="CreatePaymentTermsDialog=true" v-bind:disabled="!screenAccess"></Success-Button>
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
                    <v-list-item id="AutoTest_F200" @click.stop="fnBulkRemoveClick()">
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
                            <v-btn id="AutoTest_F201" text="" @click="RemoveBulkPaymentTermDialog=false">
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
                                                    <v-btn id="AutoTest_F202" color="secondary elevation-0" class="mr-2" @click="DeletePaymentTerm(1),RemoveBulkPaymentTermDialog = false">Remove</v-btn>
                                                    <v-btn id="AutoTest_F203" outlined color="grey" @click="RemoveBulkPaymentTermDialog=false">Cancel</v-btn>
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
                          outlined hide-details></v-text-field>
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

            <template v-slot:item.PaymentStatus="{item}">
                <v-icon class="mt-0" size="16" color="#0CBA2F" v-if="item.PaymentStatus == 'Done'">mdi-checkbox-marked-circle</v-icon>
                <v-icon class="mt-0" size="16" color="#DEB514" v-if="item.PaymentStatus == 'Pending'">mdi-minus-circle-outline</v-icon>
                <v-icon class="mt-0" size="16" color="#D50808" v-if="item.PaymentStatus == 'Cancelled'">mdi-close-octagon</v-icon>
                {{item.PaymentStatus}}
                <v-menu offset-y transition="scroll-y-transition">
                    <template v-slot:activator="{ on }">
                        <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                        <v-btn text max-width="16" min-width="16" height="20" v-on="on" color="grey" class="elevation-0 mr-1 mt-0" style="margin-top:0px">
                            <v-icon class="mt-0" size="16">mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                        <v-list-item id="AutoTest_F204" @click.stop="fnUpdatePaymnetStatus(item.PaymentId,listItem.Status)" v-for="(listItem,ind) in PaymentStatusList" :key="ind">
                            <v-list-item-icon>
                                <v-icon :color="listItem.color">{{listItem.icon}}</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>
                                {{listItem.Status}}
                            </v-list-item-content>
                        </v-list-item>
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
                        <v-list-item id="AutoTest_F205" link @click.stop="fnGetPaymentTermsByPaymentId(PaymentId=item.PaymentId),EditPaymentTermsDialog = true">
                            <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Edit</v-list-item-content>
                        </v-list-item>
                        <v-list-item id="AutoTest_F206" link @click.stop="RemovePaymentTermDialog = true,PaymentId=item.PaymentId,PaymentTermName=item.PaymentTermName">
                            <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                            <v-list-item-content>Remove</v-list-item-content>
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
                    <v-btn id="AutoTest_F207" text="" @click="ClosepaymentDialog()">
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
                        <v-btn id="AutoTest_F208" fab="" small="" v-bind:disabled="stepper==1" class="mr-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Minus')">
                            <v-icon size="30" color="primary" style="color:#0964D9 !important">mdi-arrow-left</v-icon>
                        </v-btn>
                        Payment Term {{stepper}} of {{NoOfPayments}}
                        <v-btn id="AutoTest_F209" fab="" small="" class="ml-4 elevation-1" color="#fff" @click="fnSetpperIncrement('Plus')" v-bind:disabled="stepper==NoOfPayments || !fnCheckMandatory()">
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
                                <v-date-picker v-model="paymentdate" no-title @input="PaymentDateMenu = false" format="DD-MM-YYYY">

                                </v-date-picker>
                            </v-menu>
                            <v-text-field type="number" v-show="CreatePaymentTermsLogicModel==='Project Target'" label="Target(%)" outlined="" dense="" v-model="Target" :rules="[rules.required]"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==3) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="Target==0" class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-text-field type="number" label="Type amount" outlined="" dense="" v-model="amount" :rules="[rules.required]"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;" min="0">
                                <template v-slot:append>
                                    <span v-if="amount==0" class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <span class="small-text mr-1">
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
                                    <v-btn id="AutoTest_F210" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" @click="GetFinanceOrDonarUsers()">
                                        <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Notification select pop up-->
                                <v-card>
                                    <v-card-title class="heading-3">
                                        Add people to Notify
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_F211" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp=false">
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
                                                            Donars
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
                                                <Success-Button id="AutoTest_F212" style="width:130px" class="my-1 center-block" title="Save" @click.native="NotificationSelectPopUp=false"></Success-Button>
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
                        <Previous-Button-Outlined id="AutoTest_F213" v-show="CreatePaymentTermsStepTwo" class="float-right" @click.native="CreatePaymentTermsStepOne=true,CreatePaymentTermsStepTwo=false"></Previous-Button-Outlined>
                        <Cancel-Button-Outlined-Sm id="AutoTest_F214" v-show="CreatePaymentTermsStepOne" class="float-right" @click.native="CreatePaymentTermsDialog=false"></Cancel-Button-Outlined-Sm>
                        <Next-button-primary-light id="AutoTest_F215" v-show="CreatePaymentTermsStepOne" class="float-right mr-4" @click.native="CreatePaymentTermsStepTwo=true,CreatePaymentTermsStepOne=false" v-bind:disabled="NoOfPayments==0"></Next-button-primary-light>


                        <v-btn id="AutoTest_F216" v-show="CreatePaymentTermsStepTwo" v-bind:disabled="stepper!=NoOfPayments || !fnCheckMandatory()" class="elevation-0 float-right mr-4 secondary"
                               dark="" height="36" width="122" style="background-color:#0964D9 !important; color:white !important" @click="CreatePaymentTermsDialog=false ,fnInvokePaymentTermList(NoOfPayments),fnSavepaymentTerms()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
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
                    <v-btn id="AutoTest_F217" text @click="EditPaymentTermsDialog=false">
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
                                            <v-date-picker v-model="PaymentInfo.paymentdate" no-title @input="FrmDateMenu = false" dense hide-details format="DD-MM-YYYY"></v-date-picker>
                                        </v-menu>
                                        <v-text-field type="number" v-show="PaymentInfo.Logic=='Project Target'" label="Target(%)" v-model="PaymentInfo.Target" outlined dense
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==3) return false;" min="0"></v-text-field>

                                        <v-text-field type="number" label="Type amount" v-model="PaymentInfo.Amount" outlined dense
                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;" min="0"></v-text-field>

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
                                                <Add-Button-Fab id="AutoTest_F218" @click.native="GetFinanceOrDonarUsersInEdit(),NotificationSelectPopUp2=true"></Add-Button-Fab>
                                            </template>
                                            <!--Notification select pop up-->
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    Add people to Notify
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F219" text="" height="25" min-width="25" max-width="25" color="#707070" @click="NotificationSelectPopUp2=false">
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
                                                                        Donars
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
                                                            <Success-Button id="AutoTest_F220" style="width:130px" class="my-1 center-block" title="Save" @click.native="NotificationSelectPopUp2=false"></Success-Button>
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
                        <Cancel-Button-Outlined-Sm id="AutoTest_F221" class="float-right" @click.native="EditPaymentTermsDialog=false"></Cancel-Button-Outlined-Sm>
                        <v-btn id="AutoTest_F222" class="elevation-0 float-right mr-4 primary-btn-light" v-bind:disabled="fnCheckEditMandatory()" dark height="36" width="122" @click="EditPaymentTermsDialog=false,fnUpdatepaymentTerms()">
                            Confirm
                        </v-btn>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
       



        <!-- Remove Payment Terms-->
        <v-dialog v-model="RemovePaymentTermDialog" width="800">
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Paymnet Terms
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_F223" text="" @click="RemovePaymentTermDialog=false">
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
                                            <v-btn id="AutoTest_F224" color="secondary elevation-0" class="mr-2" @click="DeletePaymentTerm(2),RemovePaymentTermDialog = false">Remove</v-btn>
                                            <v-btn id="AutoTest_F225" outlined color="grey" @click="RemovePaymentTermDialog=false">Cancel</v-btn>
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
      import objUtils from '../../utils.js'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };

   export default {
      data() {
          return {
              rules: {
                  required: value => !!value || 'Required.',
              },
              successSnackbarMsg: false,
              errorSnackbarMsg: false,
              infoSnackbarMsg: false,
              Msg: "",
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
              SearchPaymentTerms:'',
              PaymentStatusList: [
                  { Status: 'Done', icon: 'mdi-checkbox-marked-circle', Color:'#0CBA2F' },
                  { Status: 'Pending', icon: 'mdi-minus-circle-outline', Color: '#DEB514'},
                  { Status: 'Cancelled', icon: 'mdi-close-octagon', Color: '#D50808' }
              ],
              CreatePaymentTermsDialog: false,
              CreatePaymentTermsLogic: ['Date', 'Project Target'],
              CreatePaymentTermsStepOne: true,
              CreatePaymentTermsStepTwo: false,
              CreatePaymentTermsLogicModel: '',
              PaymentDateMenu: false,
              CreatePaymentTermsStepper: 1,
              NotificationSelectPopUp: false,
              NotificationSelectPopUp2: false,
              EditPaymentTermsDialog: false,
              RemovePaymentTermDialog: false,
              RemoveBulkPaymentTermDialog:false,
              
              offset: true,
              closeonClick:false,
              closeOnContentClick: false,
              PaymentInfo: {
                  PaymentId: 0,
                  PaymentTermName: "",
                  Logic: "",
                  Target:"",
                  paymentdate: "",
                  Target: 0,
                  Amount: "",
                  users:[],
              },
              paymentdate: new Date().toISOString().substr(0, 10),
              stepper: 1,
              NoOfPayments: "",
              Target: 0,
              amount: 0,
              GroupId: 0,
              PaymentTerm: {
                  PaymentTermList: [],
              },
              lstFinanceUsers: [],
              lstDonors: [],
              selectedFinanceUser: [],

              FinanceUsersTemp: [],
              DonarUserTemp: [],

              FrmDate: new Date().toISOString().substr(0, 10),
              FrmDateMenu: false,
              screenAccess: 0,
              RWAccess: 0,
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
            await this.fnGetPaymentTermsByprjID();
            await this.fnGetGroupDetailsByprjID();
            await this.fnFetchFinanceAndDonor();
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
            ///  fnGetPaymentTermsByprjID :- Function used to fetch Payment Terms Details based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetPaymentTermsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetPaymentTermsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.PaymentTermTableItems = result.data;
                }
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
            ///  GetProjectUsers :- Function used to fetch Project User List in Budget & Funds section based on type (Finance/Donor)
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
            async GetProjectUsers(Type) {

                var data = new FormData();
                data.append("GroupID", this.GroupId);
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
                    }
                }
            },

            /// <summary>
            ///  fnFetchFinanceAndDonor :- Function used to fetch Finance and Donor Users
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnFetchFinanceAndDonor() {
                this.GetProjectUsers('Finance');
                this.GetProjectUsers('Donor');
            },

            /// <summary>
            ///  fnBulkRemoveClick :- Function used to open remove dialog box, print a message if payment term is not selected to remove
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnBulkRemoveClick() {
                if (this.selectedPaymentTerms.length > 0) {
                    this.RemoveBulkPaymentTermDialog = true;
                } else {
                    this.Msg = ("Please select the paymentTerm");
                    this.infoSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  DeletePaymentTerm :- Function used to Delete Payment Term based on Type (Bulk Remove/Individual Remove)
            /// </summary>
            /// <param name="Type"></param>
            /// <returns> </returns>
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
                var result = await objManageProject.DeletePaymentTerm(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status = "Success") {
                        this.Msg = "Payment term successfully removed";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed";
                        this.errorSnackbarMsg = true;
                    }
                }
                this.fnGetPaymentTermsByprjID();
                this.PaymentId = 0;
            },

            /// <summary>
            ///  fnUpdatePaymnetStatus :- Function used to Update Payment Status based on PaymentId
            /// </summary>
            /// <param name="paymentId"></param>
            /// <param name="PaymnetStatus"></param>
            /// <returns> </returns>
            async fnUpdatePaymnetStatus(paymentId, PaymnetStatus) {
                var data = new FormData();
                data.append("PaymentId", paymentId);
                data.append("PaymnetStatus", PaymnetStatus);
                var result = await objManageProject.UpdatePaymnetStatus(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    this.Msg = "Payment status successfully updated";
                    this.successSnackbarMsg = true;
                    this.fnGetPaymentTermsByprjID();
                }

            },

            /// <summary>
            ///  fnUpdatepaymentTerms :- Function used to Update Payment Terms
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
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
                    }
                }

            },

            /// <summary>
            ///  fnCheckEditMandatory :- Function used to check any mandatory field is showing blank or null before Updating Payment Term
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false </returns>
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
                else {
                    return false;
                }
            },

            /// <summary>
            ///  GetFinanceOrDonarUsers :- Function used to fetch Finance/Donar Users when click on plus button in Add Payment Terms section
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            async GetFinanceOrDonarUsers(index) {

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
            ///  GetSelectedUserList :- Function used to fetch Selected User List in Add Payment Terms section
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

            /// <summary>
            ///  fnSetpperIncrement :- Function used to increment the step in Payment Terms creation based on Mode (Plus/Minus)
            /// </summary>
            /// <param name="Mode"></param>
            /// <returns> </returns>
            async  fnSetpperIncrement(Mode) {
                if (Mode == 'Plus') {
                    this.stepper = this.stepper + 1;
                    this.fnInvokePaymentTermList(this.stepper)
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
                        this.PaymentTerm.PaymentTermList.push({ Index: this.NoOfPayments, PaymentTermName: this.PaymentTermName, Logic: this.CreatePaymentTermsLogicModel, paymentdate: this.paymentdate, Target: this.Target, amount: this.amount, UserList: this.selectedFinanceUser });
                        
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
                if (index > 0) {

                    var temp = this.PaymentTerm.PaymentTermList.filter(x => x.Index == index);
                    if (temp.length > 0) {
                        this.PaymentTermName = temp[0].PaymentTermName;
                        this.CreatePaymentTermsLogicModel = temp[0].Logic;
                        this.paymentdate = temp[0].paymentdate;
                        this.Target = temp[0].Target;
                        this.amount = temp[0].amount;
                        this.selectedFinanceUser = temp[0].UserList;
                    }
                }
            },

            /// <summary>
            ///  fnGetGroupDetailsByprjID :- Function used to fetch group details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.GroupId = result.data.GroupId;
                    }
            },

            /// <summary>
            ///  fnSavepaymentTerms :- Function used to Add/Save New Payment Terms
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnSavepaymentTerms() {
                var data = new FormData();
                data.append("NoOfPaymentTerms", this.NoOfPayments);
                data.append("PaymentTermList", JSON.stringify(this.PaymentTerm.PaymentTermList));
                data.append("Notify", this.NotifyCheckbox);
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.SavepaymentTerms(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "success") {
                        this.Msg = "Payment term successfully created";
                        this.successSnackbarMsg = true;
                        this.ClosepaymentDialog();
                        this.fnGetPaymentTermsByprjID();
                    }
                    else {
                        this.Msg = "Failed";
                        this.errorSnackbarMsg = true;
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
                if (this.selectedFinanceUser == 0) { return false; }
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
                this.paymentdate = new Date().toISOString().substr(0, 10);
                this.CreatePaymentTermsLogicModel = "";
                this.CreatePaymentTermsDialog = false;
                this.CreatePaymentTermsStepOne = true;
                this.CreatePaymentTermsStepTwo = false;
                this.PaymentTerm.PaymentTermList = [];
                this.stepper = 1;
                this.selectedFinanceUser = [];
            },

            /// <summary>
            ///  fnAddToTempUserList :- Function used to add Finance/Donar Users to Temp User List
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnAddToTempUserList() {
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

        }
  }
</script>
