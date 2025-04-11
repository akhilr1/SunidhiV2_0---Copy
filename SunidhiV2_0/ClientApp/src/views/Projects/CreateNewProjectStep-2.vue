<template>
    <v-row class="v-scrolling-div ">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-col md="6">
            <h3 class="create-project-steps-title text-center">Select Template</h3>
            <v-row>
                <v-col cols="12" align="center" class="pt-9">
                    <v-carousel cycle height="210px"
                                hide-delimiter-background
                                show-arrows-on-hover>
                        <v-carousel-item>
                            <img src="../../images/Create-Project-Step2-Vector-1.svg" class="create-project-corousal-vector" />
                        </v-carousel-item>
                    </v-carousel>
                </v-col>
                <v-col cols="12">
                    <p class="text-center">
                        Select Template to easily get started<br>with a project
                    </p>
                </v-col>
            </v-row>
        </v-col>

        <v-col md="6">
            <v-row no-gutters="">
                <v-col cols="12">
                    <h4 class="heading-3">
                        <v-avatar size="30" color="#EDF6FF" class="mr-2">
                            <v-icon color="secondary" size="15">mdi-tablet-dashboard</v-icon>
                        </v-avatar>
                        <span class="pointer-cursor" v-show="CreateNewProjectStep2Steps==1 || CreateNewProjectStep2Steps==2 || CreateNewProjectStep2Steps==3" @click="CreateNewProjectStep2Steps=1">Templates</span>
                        <span class="pointer-cursor" v-show="CreateNewProjectStep2Steps==2 || CreateNewProjectStep2Steps==3" @click="CreateNewProjectStep2Steps=2"> > {{ThemeName}}</span>
                        <span v-show="CreateNewProjectStep2Steps==3"> > {{TemplateName}}</span>
                    </h4>
                </v-col>
            </v-row>
            <v-stepper class="elevation-0 v-scrolling-div pa-0" style="height:50vh" v-model="CreateNewProjectStep2Steps">
                <v-stepper-items>
                    <v-stepper-content step="1" class="pt-3 ml-0 pl-0">
                        <v-row no-gutters>
                            <v-col md="3" class="pointer-cursor" v-for="item in BasicThemeList" :key="item.ThemeID" align="center" @click="fnGetTemplatesBasedOnTheme(item)">
                                <v-card outlined width="70" height="70" align="center" class="elevation-1">
                                    <img :src="require('../../newtemplateassets/'+item.ThemeImage)" width="35" height="auto" class="my-4" />
                                </v-card>
                                <h5 class="mt-1 mb-0 pb-0 black--text  text-center font-weight-medium">{{item.Theme}}</h5>
                            </v-col>
                        </v-row>
                    </v-stepper-content>
                    <v-stepper-content step="2" class="pt-0 mx-0 pl-0">
                        <v-row>
                            <v-col md="6" v-for="item in TemplateList">
                                <v-card outlined class="elevation-1">
                                    <div style="width:100%; height:auto; dispaly:block; position:relative; border-radius:5px">
                                        <v-img src="https://picsum.photos/510/300?random" aspect-ratio="1.7" style="height:100px"></v-img>
                                        <input @change="fnSelectTemplate(item,item.ProjectTemplateID,item.IsSelected )" v-model="item.IsSelected" type="radio" value="male" style="position:absolute; top:10px; left:15px" />
                                        <v-card outlined="" width="50" height="50" align="center" style="position:absolute; bottom:-20px; left:15px">
                                            <img :src="require('../../newtemplateassets/'+item.ThemeImage)" width="30" height="auto" class="my-2" />
                                        </v-card>
                                    </div>
                                    <v-card-text>
                                        <h4 class="mt-3 black--text font-weight-medium text-truncate">{{item.TemplateName}}</h4>
                                        <p class="mb-1">
                                            <small>Created by ImWorks Team</small>
                                        </p>
                                        <small class="v-scrolling-div d-block mb-2 black--text font-weight-normal" style="height:40px">
                                            {{item.Description}}
                                        </small>
                                        <v-row>
                                            <v-col cols="3" class="pt-0 mt-0">
                                                <v-card class="elevation-0" width="50" height="50" align="center">
                                                    <img src="../../assets/SDG/E_SDG_PRINT-08.jpg" width="50" height="50" aspect-ratio="1" />
                                                </v-card>
                                            </v-col>
                                            <v-col cols="3" class="pt-0 mt-0">
                                                <v-card class="elevation-0" width="50" height="50" align="center">
                                                    <img src="../../assets/SDG/E_SDG_PRINT-04.jpg" width="50" height="50" />
                                                </v-card>
                                            </v-col>
                                            <v-col cols="3" class="pt-0 mt-0">
                                                <v-card class="elevation-0" width="50" height="50" align="center">
                                                    <img src="../../assets/SDG/E_SDG_PRINT-09.jpg" width="50" height="50" />
                                                </v-card>
                                            </v-col>
                                        </v-row>
                                        <Success-Button-Sm title="View Template" @click.native="fnGetTemplateBasicDetails(item),fnGetTemplateDetais(),CreateNewProjectStep2Steps = 3"></Success-Button-Sm>
                                    </v-card-text>
                                </v-card>
                            </v-col>


                        </v-row>
                    </v-stepper-content>
                    <v-stepper-content step="3">
                        <v-row>
                            <v-col cols="12">
                                <v-row>
                                    <v-col cols="auto">
                                        <v-card class="elevation-1" width="50" height="50" align="center">
                                            <!--<img src="../../images/Basic-Templates-icon.png" width="20" height="auto" class="my-3" />-->
                                            <img :src="ThemeImage" width="20" height="auto" class="my-3" />
                                    </v-card>
                                    </v-col>

                                    <v-col md="6">
                                        <h3 class="mt-1 black--text font-weight-medium">{{TemplateName}}</h3>
                                        <p class="mb-0">
                                            <small>Created by ImWorks Team</small>
                                        </p>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col class="pt-4" md="4" sm="auto" xs="auto">
                                        <Success-Button-Sm v-if="prjTemplateSelected == false" title="Select Template" @click.native="prjTemplateSelected= true"></Success-Button-Sm>
                                        <Success-Button-Sm v-if="prjTemplateSelected == true" title="Deselect Template" @click.native="prjTemplateSelected =false"></Success-Button-Sm>
                                    </v-col>
                                </v-row>
                            </v-col>
                            <v-col cols="12">
                                <h4 class="mt-1 black--text font-weight-medium">Template Description</h4>
                                <p class="mb-0">
                                    {{TemplateDescription}}
                                </p>
                            </v-col>
                            <v-col cols="12">
                                <h4 class="black--text font-weight-medium">SDG Goals</h4>
                                <v-row>
                                    <v-col cols="auto">
                                        <v-card class="elevation-0" width="50" height="50" align="center">
                                            <img src="../../images/SDG-edu.jpg" width="50" height="50" />
                                        </v-card>
                                    </v-col>

                                    <v-col cols="auto">
                                        <v-card class="elevation-0" width="50" height="50" align="center">
                                            <img src="../../images/SDG-edu.jpg" width="50" height="50" />
                                        </v-card>
                                    </v-col>
                                </v-row>
                            </v-col>

                            <v-col cols="12">
                                <template>
                                    <h4 class="black--text font-weight-medium">Outcomes</h4>
                                    <v-row>
                                        <v-col cols="12" class="expansion-panel-flat">
                                            <v-expansion-panels flat>
                                                <v-expansion-panel v-for="(item,index) in TemplatePredefinedData.Outcomes"
                                                                   :key="index">
                                                    <v-expansion-panel-header class="text--secondary py-0 px-0">
                                                        {{item.OutcomeName}}
                                                    </v-expansion-panel-header>
                                                    <v-expansion-panel-content class="px-9">
                                                        <v-row>
                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Start Date</p>
                                                                <p class="text--secondary mb-0">{{item.strStartDate}}</p>
                                                            </v-col>

                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">End Date</p>
                                                                <p class="text--secondary mb-0">{{item.strEndDate}}</p>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Periodicity</p>
                                                                <p class="text--secondary mb-0">{{item.ReportingFrequencyName}}</p>
                                                            </v-col>

                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Survey</p>
                                                                <p class="text--secondary mb-0">{{item.Survey}}</p>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col cols="12">
                                                                <p class="black--text mb-0">SDG Indicators</p>
                                                            </v-col>
                                                            <v-col cols="12" class="py-0" v-for="Titem in item.lstSDGList" :key="item.target">
                                                                <v-row>
                                                                    <v-col cols="auto">
                                                                        <v-card class="elevation-0" width="50" height="50" align="center">
                                                                            <img src="../../images/SDG-edu.jpg" width="50" height="50" />
                                                                        </v-card>
                                                                    </v-col>
                                                                    <v-col cols="auto">
                                                                        <p class="black--text mb-0">{{Titem.Target.length}} Targets</p>
                                                                        <v-btn text small color="secondary" @click="Titem.ViewAllPanel=!Titem.ViewAllPanel" class="px-0">View All</v-btn>
                                                                    </v-col>
                                                                    <v-col cols="12" v-if="Titem.ViewAllPanel" class="expansion-panel-flat">
                                                                        <v-expansion-panels flat>
                                                                            <v-expansion-panel v-for="(item,i) in Titem.Target"
                                                                                               :key="i">
                                                                                <v-expansion-panel-header class="black--text pb-0">
                                                                                    {{item.Target_Name}}
                                                                                </v-expansion-panel-header>
                                                                                <v-expansion-panel-content class="px-9" v-for="(item,i) in item.Target_Indicators">
                                                                                    {{item.Indicator}}
                                                                                </v-expansion-panel-content>
                                                                            </v-expansion-panel>
                                                                        </v-expansion-panels>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-expansion-panel-content>
                                                </v-expansion-panel>
                                            </v-expansion-panels>

                                        </v-col>
                                    </v-row>
                                </template>
                                <template>
                                    <h4 class="black--text font-weight-medium mt-4">Outputs</h4>
                                    <v-row>
                                        <v-col cols="12" class="expansion-panel-flat">
                                            <v-expansion-panels flat>
                                                <v-expansion-panel v-for="(item,index) in TemplatePredefinedData.Outputs"
                                                                   :key="index">
                                                    <v-expansion-panel-header class="text--secondary py-0 px-0">{{item.OutputName}}</v-expansion-panel-header>
                                                    <v-expansion-panel-content class="px-9">
                                                        <v-row>
                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Start Date</p>
                                                                <p class="text--secondary mb-0">{{item.StartDate}}</p>
                                                            </v-col>

                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">End Date</p>
                                                                <p class="text--secondary mb-0">{{item.EndDate}}</p>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">ReportingFrequencyName</p>
                                                                <p class="text--secondary mb-0">{{item.ReportingFrequencyName}}</p>
                                                            </v-col>

                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Survey</p>
                                                                <p class="text--secondary mb-0">{{item.Survey}}</p>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Type</p>
                                                                <p class="text--secondary mb-0">{{item.Type}}</p>
                                                            </v-col>

                                                            <v-col cols="auto">
                                                                <p class="black--text mb-0">Sub Activities</p>
                                                                <p class="text--secondary mb-0">{{item.SubActivities}} - Sub Activities</p>
                                                            </v-col>
                                                        </v-row>
                                                    </v-expansion-panel-content>
                                                </v-expansion-panel>
                                            </v-expansion-panels>
                                        </v-col>
                                    </v-row>
                                </template>
                                <template>
                                    <h4 class="black--text font-weight-medium mt-4">Surveys</h4>
                                    <v-row>
                                        <v-col cols="12" v-for="item in TemplatePredefinedData.SurveyList">
                                            <ol class="text--secondary pl-6">
                                                <li class="mb-2">{{item.SurveyName}}</li>
                                                <!--li class="mb-2">Former Training program</li>-->
                                            </ol>

                                        </v-col>
                                    </v-row>
                                </template>
                                <template>
                                    <h4 class="black--text font-weight-medium mt-4">Beneficiaries</h4>
                                    <v-row>
                                        <!--<v-col cols="12" v-for="item in TemplatePredefinedData.BeneficiaryTypeList[0].BeneficiaryList">
                                      <ol class="text--secondary pl-6">
                                          <li class="mb-2">{{item.BeneficiaryName}}</li>
                                      </ol>

                                    </v-col>-->
                                    </v-row>
                                </template>
                            </v-col>
                        </v-row>
                    </v-stepper-content>
                </v-stepper-items>
            </v-stepper>
        </v-col>
        
        <v-col cols="12" align="right">
            <Secondary-Button title="Next" :loading="loading" :disabled="loading" class="btn-122x36 mr-4" v-on:click.native="fnSaveTemplateDatatoProject()"></Secondary-Button>
            <v-btn depressed class="btn-122x36 mr-4" height="37" v-on:click="GoToCreateNewProjectStepsOneFn">Previous</v-btn>
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
</template>

<script>
    import SuccessButtonSm from'@/components/success-button-sm.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import PreLoader from '@/components/pre-loader.vue'

 
    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/Project.js").then(mod => {
                objProject = new mod.Project();
            })
        ]);
    };
    var objCreateProject;
    async function importCreateNewscript() {
        return Promise.all([
            import("../../BL/CreateProject.js").then(mod => {
                objCreateProject = new mod.CreateProject();
            })
        ]);
    };
  export default{
    
    props:[
    'CreateNewProjectStepsProp'
    ],
    
    data : function(){
        return {
            loading : false,
            WorkspaceId: 0,
            InitiativeId :0,
            CreateNewProjectStep2Steps: 1,
            ThemeID: 0,
            ThemeName: '',
            ThemeImage: '',
            TemplateId : 0,
            TemplateName: '',
            TemplateDescription: '',
            TemplateList: [],
            BasicThemeList: [],
            TemplatePredefinedData: [],
            PreLoader: false,
            prjTemplateSelected: false,
            Msg: "",
            successSnackbarMsg: false,
            errorSnackbarMsg: false,
            infoSnackbarMsg: false,

        }
    },
    mounted: async function () {
            this.InitiativeId = this.$route.query.InitID;
            this.WorkspaceId = this.$route.query.WsID;
            await importscript();
            await importCreateNewscript();
            await this.GetProjectThemeList();
    },
    components: {
        'Success-Button-Sm': SuccessButtonSm,
        'Secondary-Button': SecondaryButton,
        'Pre-Loader': PreLoader,
    },
    
    methods:{
    
      /// <summary>
      ///  GoToCreateNewProjectStepsThreeFn :- Function used to go to new project step three (next step)
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      GoToCreateNewProjectStepsThreeFn(){
        this.CreateNewProjectStepsProp=3;
        this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
      },
      
      /// <summary>
      ///  GoToCreateNewProjectStepsOneFn :- Function used to go to new project step one (previous step)
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      GoToCreateNewProjectStepsOneFn(){
        this.CreateNewProjectStepsProp=1;
        this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
      },

      // Theme and Template fetching methods
        //================Project Template=====================

        /// <summary>
        ///  GetProjectThemeList :- Function used to fetch project theme list
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async GetProjectThemeList() {
            var data = new FormData();
            data.append("InitiativeID", this.InitiativeId);
            var result = await objProject.GetProjectThemeList(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else {
                if (result.data)
                    this.BasicThemeList = result.data;
            }
        },

        /// <summary>
        ///  fnGetTemplatesBasedOnTheme :- Function used to fetch template list based on theme
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        async fnGetTemplatesBasedOnTheme(item) {
            this.ThemeName = item.Theme;
            this.ThemeID = item.ThemeID;
            var data = new FormData();
            data.append("InitiativeID", this.InitiativeId);
            data.append("ThemeID", item.ThemeID);
            data.append("NoOfTemplates", item.NoOfTemplates);
            var result = await objProject.GetProjectTemplateList(data);
            
            if (result.status == "Unhandled Exception")
                this.$router.push({ name: "error" });
            else {
                if (result.data) {
                    
                    this.TemplateList = result.data;
                    for (var i = 0; i < this.TemplateList.length; i++)
                        this.TemplateList[i].IsSelected = 0;
                }
            }
            this.CreateNewProjectStep2Steps = 2;
        },

        /// <summary>
        ///  fnGetTemplateBasicDetails :- Function used to fetch template basic details (Template name, Template description and Theme image)
        /// </summary>
        /// <param name="item"></param>
        /// <returns> </returns>
        async fnGetTemplateBasicDetails(item) {
            this.TemplateId = item.ProjectTemplateID;
            this.TemplateName = item.TemplateName;
            this.TemplateDescription = item.Description;
            this.ThemeImage = require('../../newtemplateassets/' + item.ThemeImage);
        },

        /// <summary>
        ///  fnGetTemplateDetais :- Function used to fetch template predefined data (Data of Outcome, Output, Survey and Beneficiaries)
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnGetTemplateDetais() {
            this.TemplatePredefinedData = [];
            var data = new FormData();
            data.append("ThemeId", 1);
            data.append("TemplateId", 1);
            
            var result = await objCreateProject.FetchTemplatePreDefinedData(data);
          
            if (result.status == "Unhandled Exception")
                this.$router.push({ name: "error" });
            else {
                if (result.data) {
                   this.TemplatePredefinedData = result.data;
                }
            }
        },

        /// <summary>
        ///  fnSelectTemplate :- Function used to check template is selected or not
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ProjectTemplateID"></param>
        /// <param name="isSelected"></param>
        /// <returns> </returns>
        async fnSelectTemplate(item, ProjectTemplateID, isSelected) {
            //await this.fnGetTemplateDetais(item);
            if (this.TemplatePredefinedData!= null) {
                var i = 0;
                if (isSelected) {
                    this.prjTemplateSelected = true;
                    this.TemplateId = ProjectTemplateID;
                }
                else
                    this.prjTemplateSelected = false;
                
                for (i = 0; i < this.TemplateList.length; i++) {
                    if (this.TemplateList[i].ProjectTemplateID != ProjectTemplateID)
                        this.TemplateList[i].IsSelected = false;
                    else
                        this.TemplateList[i].IsSelected = true;
                }
                //this.CreateNewProjectStep2Steps = 3;
            }
        },

        /// <summary>
        ///  fnSaveTemplateDatatoProject :- Function used to save template data to project
        /// </summary>
        /// <param name=""></param>
        /// <returns> </returns>
        async fnSaveTemplateDatatoProject() {
            if (this.prjTemplateSelected == true) {
                this.loading = true;
                this.PreLoader = true;
                await this.fnGetTemplateDetais();
                var data = new FormData();
                data.append("ThemeID", this.ThemeID);
                data.append("ProjectTemplateID", this.ThemeID);
                data.append("ThemeName", this.ThemeID);
                data.append("PrjtStartDate", window.ProjectStartDate)
                data.append("PrjtEndDate", window.ProjectEndDate)
                data.append("strOutcomes", JSON.stringify(this.TemplatePredefinedData.Outcomes));
                data.append("strSurveyList", JSON.stringify(this.TemplatePredefinedData.SurveyList));
                data.append("strBeneficiaryTypeList", JSON.stringify(this.TemplatePredefinedData.BeneficiaryTypeList));
                data.append("strOutputs", JSON.stringify(this.TemplatePredefinedData.Outputs));
                data.append("ProjectId", window.ProjectId)
                var result = await objCreateProject.fnSaveTemplateDatatoProject(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "Success") {
                        this.GoToCreateNewProjectStepsThreeFn();
                    }
                }
                this.loading = false;
                this.PreLoader = false;
            } else {
                this.Msg = "Please Select a Template !";
                this.infoSnackbarMsg = true;
            }
        }
    },
        
  }
</script>