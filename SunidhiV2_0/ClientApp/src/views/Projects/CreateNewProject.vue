
  <template>
    <v-dialog persistent v-model="CreateNewProjectDialogProp" >
      <v-card class="right-fixed-panel" >
        <v-card-title>
          <v-avatar size="30" color="#EDF6FF" class="mr-2">
            <v-icon color="secondary" size="15">mdi-file</v-icon>
          </v-avatar> Project Setup
        </v-card-title>
        <v-card-text>
          <v-stepper v-model="CreateNewProjectSteps" class="elevation-0">
          
            
            <v-row no-gutters class="mt-10">
              <!--step header 1-->
              <v-col md="auto">
                <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>0 }" >
                  <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>0}">
                    mdi-file-document
                  </v-icon><br>
                  Define Project
                </p>
              </v-col>
              <v-col>
                <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>1}"></v-divider>
              </v-col>
             <!--__________________-->

              <!--step header 2-->
              <v-col md="auto">
                <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>1}" >
                  <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>1}">
                    mdi-tablet-dashboard
                  </v-icon>
                  <br>
                  Select Template
                
                </p>
              </v-col>
              <v-col>
                <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>2}"></v-divider>
              </v-col>
              <!--__________________-->

              <!--step header 3-->
              <v-col md="auto">
                <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>2}" >
                  <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>2}">
                    mdi-file-chart
                  </v-icon>
                  <br>
                 Define Outcomes
                </p>
              </v-col>
              <v-col>
                <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>3}"></v-divider>
              </v-col>
              <!--__________________-->

              <!--step header 4-->
              <v-col md="auto">
                <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>3}" >
                  <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>3}">
                    mdi-clipboard-list
                  </v-icon>
                  <br>
                Define Outputs
                </p>
              </v-col>
              <v-col>
                <v-divider class="mt-4" :class="{'secondary':CreateNewProjectSteps>4}"></v-divider>
              </v-col>
              <!--__________________-->

              <!--step header 5-->
              <v-col md="auto">
                <p class="text-center" :class="{'secondary--text':CreateNewProjectSteps>4}" >
                  <v-icon size="40" :class="{'secondary--text':CreateNewProjectSteps>4}">
                    mdi-cash-multiple
                  </v-icon>
                  <br>
                Define Inputs
                </p>
              </v-col>
        
              <!--__________________-->
           
            
            </v-row>
            <v-stepper-items>
              <v-stepper-content step="1">
                <Create-New-Project-Step1></Create-New-Project-Step1>
                <v-row>
               <v-col cols="12" align="right">
                 <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="CreateNewProjectSteps=2"></Secondary-Button>
               <v-btn depressed class="btn-122x36"  v-on:click="$emit('hide-Create-New-Project-Dialog')">Cancel</v-btn>
              </v-col>
                </v-row>
              </v-stepper-content>

              <v-stepper-content step="2">
                <Create-New-Project-Step2></Create-New-Project-Step2>
                <v-row>
               <v-col cols="12" align="right">
                 <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="CreateNewProjectSteps=3"></Secondary-Button>
                 <v-btn depressed class="btn-122x36 mr-4"  v-on:click="CreateNewProjectSteps=1">Previous</v-btn>
              </v-col>
                </v-row>
              </v-stepper-content>
            
              <v-stepper-content step="3">
                <Create-New-Project-Step3></Create-New-Project-Step3>
                <v-row>
               <v-col cols="12" align="right">
                 <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="CreateNewProjectSteps=4"></Secondary-Button>
                 <v-btn depressed class="btn-122x36 mr-4"  v-on:click="CreateNewProjectSteps=2">Previous</v-btn>
                 <v-btn depressed="" class="btn-122x36"  v-on:click="CreateNewProjectSteps=4">Skip for Now</v-btn>
               </v-col>
                </v-row>
              </v-stepper-content>
            
              <v-stepper-content step="4">
                <Create-New-Project-Step4></Create-New-Project-Step4>
                <v-row>
               <v-col cols="12" align="right">
                 <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="CreateNewProjectSteps=5"></Secondary-Button>
                 <v-btn depressed class="btn-122x36 mr-4"  v-on:click="CreateNewProjectSteps=3">Previous</v-btn>
                 <v-btn depressed="" class="btn-122x36"  v-on:click="CreateNewProjectSteps=5">Skip for Now</v-btn>
              </v-col>
                </v-row>
              </v-stepper-content>
            
              <v-stepper-content step="5">
                <Create-New-Project-Step5></Create-New-Project-Step5>
                <v-row>
               <v-col cols="12" align="right">
                 <Secondary-Button title="Confirm" class="btn-122x36 mr-4" v-on:click.native="$emit('hide-Create-New-Project-Dialog')"></Secondary-Button>
                 <v-btn depressed class="btn-122x36"  v-on:click="CreateNewProjectSteps=4">Previous</v-btn>
              </v-col>
                </v-row>
              </v-stepper-content>
            </v-stepper-items>
          </v-stepper>

        </v-card-text>
      </v-card>
  </v-dialog>
  </template>

  <script>
    import CreateNewProjectStep1 from '@/views/Projects/CreateNewProjectStep-1.vue'
    import CreateNewProjectStep2 from '@/views/Projects/CreateNewProjectStep-2.vue'
    import CreateNewProjectStep3 from '@/views/Projects/CreateNewProjectStep-3.vue'
    import CreateNewProjectStep4 from '@/views/Projects/CreateNewProjectStep-4.vue'
    import CreateNewProjectStep5 from '@/views/Projects/CreateNewProjectStep-5.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
     
     
    export default {
    
      props:{
        CreateNewProjectDialogProp:{
          type: Boolean,
          default: true
        }
      
      },
      
      data () {
        return {
          CreateNewProjectSteps: 1,
        }
      },
      
      components: {
        'Create-New-Project-Step1':CreateNewProjectStep1,
        'Create-New-Project-Step2':CreateNewProjectStep2,
        'Create-New-Project-Step3':CreateNewProjectStep3,
        'Create-New-Project-Step4':CreateNewProjectStep4,
        'Create-New-Project-Step5':CreateNewProjectStep5,
        'Secondary-Button':SecondaryButton,
        'Default-Button-Outlined':DefaultButtonOutlined,
      },
      
      methods:{
        
      },
      
         
   
    }
  </script>
