<template>
  <v-row>
    <!--<v-col>
      <Budget-and-Funds-Card></Budget-and-Funds-Card>
    </v-col>
    <v-col>
      <Payment-Terms-Card></Payment-Terms-Card>
    </v-col>
    <v-col>
      <Accounts-Card></Accounts-Card>
    </v-col>-->
      <v-col>
          <Project-BudgetAndFunds-Details></Project-BudgetAndFunds-Details>
      </v-col>
    <v-responsive width="100%"></v-responsive>
    <v-col>
      <v-stepper v-model="FinanceDetailsStep">
        <v-stepper-items class="py-0">
          <v-stepper-content step="1" class="pt-0 px-0">
            <v-row class="py-0">
              <v-col cols="12" md="12" class="text-right py-1 pr-4">
                  <div style="position:absolute;top:24px; right:11px;z-index:1">
                       <!--Previous Button-->
                      <v-btn id="AutoTest_IP466" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=1" disabled>
                          <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                      </v-btn>
                       <!--Next Button-->
                      <v-btn id="AutoTest_IP467" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=2">
                          <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                      </v-btn>
                  </div>
              </v-col>
            </v-row>
            <!--<Manage-Project-Finance-Accounts></Manage-Project-Finance-Accounts>-->
              <FinanceAccount></FinanceAccount>
          </v-stepper-content>
          <v-stepper-content step="2" class="px-0 pt-0">
             <v-row class="py-0">
                 <v-col cols="12" md="12" class="text-right py-1 pr-4">
                     <div style="position:absolute;top:24px; right:11px;z-index:1">
                         <!--Previous Button-->
                         <v-btn id="AutoTest_IP469" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=1">
                             <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                         </v-btn>
                         <!--Next Button-->
                         <v-btn id="AutoTest_IP470" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=3">
                             <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                         </v-btn>
                     </div>
                 </v-col>
            </v-row>
            <Ledger-Book-Card></Ledger-Book-Card>
          </v-stepper-content>
          <v-stepper-content step="3" class="px-0 pt-0">
              <v-row class="py-0">
                  <v-col cols="12" md="12" class="text-right py-1 pr-4">
                      <div style="position:absolute;top:24px; right:11px;z-index:1">
                          <!--Previous Button-->
                          <v-btn id="AutoTest_IP471" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=2">
                              <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                          </v-btn>
                          <!--Next Button-->
                          <v-btn id="AutoTest_IP472" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=4">
                              <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                          </v-btn>
                      </div>
                  </v-col>
              </v-row>
              <FinanceActivity></FinanceActivity>
          </v-stepper-content>
          <v-stepper-content step="4" class="px-0 pt-0">
            <v-row class="py-0">
                <v-col cols="12" md="12" class="text-right py-1 pr-4">
                    <div style="position:absolute;top:24px; right:11px;z-index:1">
                        <!--Previous Button-->
                        <v-btn id="AutoTest_IP473" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=3">
                            <v-icon size="15" color="primary">mdi-arrow-left</v-icon>
                        </v-btn>
                        <!--Next Button-->
                        <v-btn id="AutoTest_IP474" fab="" x-small="" class="mr-2 elevation-2" color="#fff" @click="FinanceDetailsStep=4" disabled>
                            <v-icon size="15" color="primary">mdi-arrow-right</v-icon>
                        </v-btn>
                    </div>
                </v-col>
            </v-row>
            <FinanceWorkflow></FinanceWorkflow>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
     
    </v-col>
  </v-row>
</template>
<script>
  import BudgetandFundsCard from'@/views/Projects/BudgetandFundsCard.vue'
  import PaymentTermsCard from'@/views/Projects/PaymentTermsCard.vue'
  import AccountsCard from'@/views/Projects/AccountsCard.vue'
  import LedgerBookCard from '@/views/Projects/LedgerBookCard.vue'
  import MangeProjectBudgetAndFundsDetails from '@/views/Projects/MangeProjectBudgetAndFundsDetails.vue'
  import ManageProjectFinanceAccounts from '@/views/Projects/ManageProjectFinanceAccounts.vue'
  import FinanceActivity from '@/views/Finance/ManageFinanceActivity.vue'
    import FinanceWorkflow from '@/views/Finance/ManageFinanceWorkflow.vue'
    import FinanceAccount from '@/views/Finance/ManageFinanceAccount.vue'

  export default {
  data(){
  return{
  FinanceDetailsStep:1,
  }
  },
  components:{
  'Budget-and-Funds-Card':BudgetandFundsCard,
  'Payment-Terms-Card':PaymentTermsCard,
  'Accounts-Card':AccountsCard,
  'Ledger-Book-Card': LedgerBookCard,
  'Project-BudgetAndFunds-Details': MangeProjectBudgetAndFundsDetails,
  'Manage-Project-Finance-Accounts':ManageProjectFinanceAccounts,
  'FinanceActivity': FinanceActivity,
      'FinanceWorkflow': FinanceWorkflow,
      'FinanceAccount': FinanceAccount,
  },

  }
</script>