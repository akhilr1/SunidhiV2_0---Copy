<template>
    <div>
        <v-card class="custom-card-1">
            <v-card-title style="height:73px">
                <h5 class="page-head">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                    </v-avatar>
                    <router-link :to="{ name: 'ManageVentureKPI', query: {WsID: WorkspaceID, Access:RWAccess , ProjID : ProjectId }}" class="page-head text-link">KPI</router-link> > {{KPIInfo.KPIName}}
                </h5>
            </v-card-title>
            <v-card-text>
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-tabs color="tab-active-line" centered v-model="KPIActivityTab" class="pt-0">
                        <v-tab href='#tab-1'>Details</v-tab>
                        <v-tab href='#tab-2'>Activity Log</v-tab>

                        <!--Details-->
                        <v-tab-item value="tab-1">
                            <v-row :justify="justifyCenter" class="mt-4">
                                <!--- View KPI Details  -->
                                <v-col md="4" class="v-scrolling-div" style="height:40vh" v-if="editKPIDetails == false">
                                    <v-row :justify="justifyCenter">
                                        <v-col md="12" class="pb-0">
                                            <!-- Project Name -->
                                            <h4 class="font-weight-regular heading-3-dark">
                                                KPI Name
                                            </h4>
                                            <h4 class="font-weight-regular heading-3 mb-4">{{KPIInfo.KPIName}}</h4>
                                            <!-- Project Description -->
                                            <h4 class="font-weight-regular heading-3-dark">
                                                KPI Description
                                            </h4>
                                            <h4 class="font-weight-regular heading-3 mb-4">
                                                {{KPIInfo.Description}}
                                            </h4>
                                            <v-row>
                                                <v-col md="6">
                                                    <h4 class="font-weight-regular heading-3-dark">
                                                        Start Date
                                                    </h4>
                                                    <h4 class="font-weight-regular heading-3 mb-4">{{formatDate(this.KPIInfo.StartDate.substr(0, 10)) }}</h4>
                                                </v-col>
                                                <v-col md="6">
                                                    <h4 class="font-weight-regular heading-3-dark">
                                                        End Date
                                                    </h4>
                                                    <h4 class="font-weight-regular heading-3 mb-4">{{ formatDate(this.KPIInfo.EndDate.substr(0, 10)) }}</h4>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col md="6">
                                                    <h4 class="font-weight-regular heading-3-dark">
                                                        KPI Periodicity
                                                    </h4>
                                                    <h4 class="font-weight-regular heading-3 mb-4">{{KPIInfo.KPIPeriodicityName}}</h4>
                                                </v-col>

                                                <v-col md="6">
                                                    <h4 class="font-weight-regular heading-3-dark">
                                                        Workflow
                                                    </h4>
                                                    <h4 class="font-weight-regular heading-3 mb-4">{{GetWFName(KPIInfo.WorkflowID)}}</h4>
                                                </v-col>
                                            </v-row>
                                            <h4 class="font-weight-regular heading-3-dark">
                                                Survey
                                            </h4>
                                            <h4 class="font-weight-regular heading-3 mb-4">{{GetSurveyName(KPIInfo.SurveyID)}}</h4>

                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                            <!-- Tags Sections-->
                            <v-row :justify="justifyCenter">
                                <v-col md="4">
                                    <!-- Users-->
                                    <span class="font-weight-regular heading-3-dark mr-1">
                                        <v-icon size="15">mdi-account</v-icon> Users
                                    </span>
                                    <v-avatar size="35" class="team-avatar" v-if="KPIInfo.UserList == 0">
                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                                    </v-avatar>
                                    <v-avatar size="35" class="team-avatar" color="red" v-if="KPIInfo.UserList.length == 1">
                                        <span class="text--white">{{KPIInfo.UserList[0].Name.charAt(0)}}</span>
                                    </v-avatar>
                                    <v-avatar size="35" class="team-avatar" color="red" v-if="KPIInfo.UserList.length >= 2">
                                        <span class="text--white">{{KPIInfo.UserList[0].Name.charAt(0)}}</span>
                                    </v-avatar>
                                    <v-avatar size="35" class="team-avatar" color="red" v-if="KPIInfo.UserList.length >= 2">
                                        <span class="text--white">{{KPIInfo.UserList[1].Name.charAt(0)}}</span>
                                    </v-avatar>
                                    <!-- Tags-->
                                    <v-row class="mb-1">
                                        <v-col class="text-left  d-inline-flex" md="12">
                                            <span class="font-weight-regular heading-3-dark mr-2"> <v-icon size="16">mdi-tag-multiple</v-icon> Tags</span>
                                            <div v-if="!allTags">
                                                <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                                <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                                                <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                                                <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
                                            </div>
                                            <v-btn id="AutoTest_V22" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                            </v-btn>
                                            <v-btn id="AutoTest_V23" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                            </v-btn>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                        <v-col md="12" class="chip-container mb-0 pt-0">
                                            <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                <v-chip id="AutoTest_V24" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                    {{TagItem.TagName}}
                                                </v-chip>
                                            </v-chip-group>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-tab-item>
                        <!-- Activity Log-->
                        <v-tab-item value="tab-2">
                            <v-row :justify="justifyCenter" class="mt-1">
                                <v-col md="6">
                                    <v-card>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:300px" class="v-scrolling-div">
                                            <v-list three-line dense="">
                                                <template v-for="(item, index) in lstActivityLog">

                                                    <v-list-item>

                                                        <v-list-item-avatar color="red">
                                                            <span>
                                                                {{item.Name.charAt(0)}}
                                                            </span>
                                                        </v-list-item-avatar>
                                                        <v-list-item-content>
                                                            <v-list-item-title class="pb-0 pt-0">
                                                                <v-col md="12" class="d-lg-inline-flex font-weight-regular heading-3-dark pb-0">
                                                                    <v-col md="6">
                                                                        {{item.Name}}
                                                                    </v-col>
                                                                    <v-col md="6" class="text-right font-weight-regular heading-3-dark">
                                                                        {{item.CreatedDate}}
                                                                    </v-col>
                                                                </v-col>
                                                            </v-list-item-title>
                                                            <v-list-item-subtitle class="font-weight-regular heading-3-dark ml-3">
                                                                <v-col>
                                                                    {{item.Message}}
                                                                </v-col>
                                                            </v-list-item-subtitle>
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </template>
                                            </v-list>
                                        </v-card-text>
                                    </v-card>
                                </v-col>
                            </v-row>


                        </v-tab-item>
                    </v-tabs>

                </v-col>
            </v-card-text>
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
                RWAccess: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                ProjectId: 0,
                InitiativeId: 0,
                KPIId: 0,
                Type: '',
                Name: '',
                justifyCenter: 'center',
                KPIActivityTab: "",
                editKPIDetails: false,
                //SelectKPIPeriodicity: ['Never', 'Once', 'Daily', 'Weekly', 'Monthly', 'Quarterly', 'Half Yearly', 'Yearly', 'Forever'],
                workflows: [],
                Surveys: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AssignAccountUserPopUp: false,
                //From Date
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                endDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                EditKPI: {
                    KPIId: 0,
                    KPIName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    KPIPeriodicity: "",
                    KPIPeriodicityName: "",
                    Workflow: "",
                    Survey: "",
                    UserList: [],
                    periodicityDate: new Date().toISOString().substr(0, 10),
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                },
                editWorkFlow: {
                    WorkflowID: 0,
                    WfName: ""
                },
                editSurvey: {
                    SurveyID: 0,
                    SurveyName: ""
                },
                KPIInfo: {
                    KPIId: 0,
                    KPIName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    KPIPeriodicity: "",
                    KPIPeriodicityName: "",
                    WorkflowID: 0,
                    Workflow: "",
                    SurveyID: "",
                    Survey: "",
                    UserList: [],
                    periodicityDate: new Date().toISOString().substr(0, 10),
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                },
                selectedteamUsers: [],
                closeonClick: false,
                closeOnContentClick: false,
                lstuserTableheaders: [{ text: "Name", align: "left", value: "Name" }, { text: "User Role", value: "UserRole" },],
                lstUsers: [],
                lstActivityLog: [],

                lstPeriodicity: [],
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                //ReportingFrequency: 0,
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,

                rules: {
                    required: value => !!value || 'Required.',
                },
                periodicityDateMenu: false,

                // Tags ------------
                NewTagMenu: false,
                AllTagListSteps: 0,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                filterAllTagList: [],
                SearchTagDetails: "",
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
        computed: {
            FrmDate() {
                return this.formatDate(this.startDate)
            },
            ToDate() {
                return this.formatDate(this.endDate)
            },
            FormattedDate() {
                return this.formatDate(this.EditKPI.periodicityDate)
            },
        },
        mounted: async function () {

            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.ProjectId = this.$route.query.ProjID;
            this.Type = this.$route.query.Type;
            this.KPIId = this.$route.query.KPIID;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await this.ChangeTab();
            await this.fnGetKPIByKPIId();
            await this.fnGetWorkflows();
            await this.fnGetActivityLog();
            await this.fnGetSurveys();
            await this.fnGetAllTags();
        },
        methods: {
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            IsValidDate() {

                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = ("Invalid Date : End date should be greater than Start Date");
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
            },
            
            async ChangeTab(index) {
                var index;
                if (this.Type == 'Edit') {
                    index = 1;
                } else if (this.Type == 'Log') {
                    index = 2;
                }
                if (index == 1) {
                    this.KPIActivityTab = 'tab-1';
                }
                if (index == 2) {
                    this.KPIActivityTab = 'tab-2';
                }

            },
            async fnGetKPIByKPIId() {
                var data = new FormData();
                data.append("KPIId", this.KPIId);
                var result = await objManageProject.GetKPIByKPIId(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        
                        this.KPIInfo = result.data;
                        this.KPIInfo.UserList = JSON.parse(this.KPIInfo.Users);

                        this.KPIInfo.KPIPeriodicity = result.data.KPIPeriodicity;
                        this.KPIInfo.KPIPeriodicityName = result.data.ReportingFrequencyName;
                        this.KPIInfo.ReportingMonth = result.data.ReportingMonth;
                        this.KPIInfo.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.KPIInfo.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.KPIInfo.ReportingWeek = result.data.ReportingWeek;
                       
                        if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                            this.KPIInfo.ReportingOnceDate = result.data.ReportingOnceDate;
                        }
                        if (result.data.Tags != null && result.data.Tags != undefined && result.data.Tags != "") {
                            this.SelectedTag = JSON.parse(result.data.Tags);
                        }
                    }
            },
             GetWFName(workflowid) {
                var temp = this.workflows.filter(x => x.WorkflowID == workflowid);
                if (temp.length > 0) {
                    return temp[0].WfName;
                } else {
                    return "Not Defined";
                }
            },
             GetSurveyName(Surveyid) {
                
                var temp = this.Surveys.filter(x => x.SurveyID == Surveyid);
                if (temp.length > 0) {
                    return temp[0].SurveyName;
                } else {
                    return "Not Defined";
                }
            },
             async fnGetWorkflows() {

                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetWorkflows(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.workflows = result.data.workflows;

                }
            },
             async fnGetSurveys() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetSurveyName(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    
                    this.Surveys = result.data.Survey;
                }
            },
            async fnGetAllTags() {
                var result = await objManageProject.GetAllProjectTags(this.InitiativeId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = ("Failed to get tag list.");
                    this.errorSnackbarMsg = true;
                }
            },
            async fnGetActivityLog() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("KPIId", this.KPIId);
                var result = await objManageProject.GetKPIActivityLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstActivityLog = result.data;
                    }
                }
            },
        },
        watch: {
        }
    }
</script>