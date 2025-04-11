<template>
    <v-container class="pb-0" fluid id="output-container">
        <v-row>
            <v-col sm="12" cols="12" class="pt-1">
                <h1 class="pl-4" style="color: #2D2F40; font-size: 17px; font-weight: bold">
                    Output Overview
                </h1>
            </v-col>
        </v-row>
        <v-row class="pl-4">
            <v-col sm="5" cols="12">

                <v-select dense :items="lstproject"
                          :item-text="i => i.Value"
                          :item-value="i => i.Key"
                          label="Select Project"
                          outlined hide-details class="mr-2"
                          v-model="ProjectId"
                          @change="mShowChartRow = false,fnGetProjectActivityData(ProjectId,startDate,endDate)
                      ">
                </v-select>
            </v-col>
        </v-row>

        <v-row>
            <v-col sm="2" cols="12" class="pt-8 pb-0">
                <v-row>
                    <v-col sm="5" cols="12" class="pt-0" style="text-align: end">
                        <v-avatar>
                            <img :src="total_output" alt="" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutputOverview.TotalCount}}</span>
                        </v-col>
                        <v-col sm="12" class="pt-0 pl-0">
                            <span style="font-size: 12px">Total Outputs</span>
                        </v-col>
                    </v-col>
                </v-row>
            </v-col>
            <v-col sm="3" cols="12" class="pt-8 pb-0">
                <v-row>
                    <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
                        <v-avatar>
                            <img :src="pending" alt="John" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 ">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutputOverview.PendingCount}}</span>
                        </v-col>
                        <v-col sm="12" class="pt-0 pl-0">
                            <span style="font-size: 12px">Total Pending</span>
                        </v-col>
                    </v-col>
                </v-row>
            </v-col>
            <v-col sm="3" cols="12" class="pt-8 pb-0">
                <v-row>
                    <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
                        <v-avatar>
                            <img :src="completed" alt="John" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutputOverview.CompletedCount}}</span>
                        </v-col>
                        <v-col sm="12" class="pt-0 pl-0">
                            <span style="font-size: 12px">Total Completed</span>
                        </v-col>
                    </v-col>
                </v-row>
            </v-col>

        </v-row>
        <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                <template>
                    <v-container class="pt-0 pb-0" fluid>
                        <v-row>
                            <v-col sm="4" cols="12" class="pl-8 pt-0">
                                <v-row>
                                    <v-col class="pb-0" sm="12" cols="12">
                                        <span style="color: #2d2f40; font-size: 17px; font-weight: bold">Individual Output Analysis</span>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <v-row class="pl-4 pt-0">
                            <v-col class="pt-0" sm="2" cols="12">
                                <!--<v-select dense label="Start Date" outlined hide-details></v-select>-->

                                <v-menu v-model="mStartDate"
                                        :close-on-content-click="false"
                                        :nudge-right="40"
                                        transition="scale-transition"
                                        offset-y
                                        min-width="290px">

                                    <template v-slot:activator="{ on }">
                                        <v-text-field v-model="computedStartDate"
                                                      label="Start Date"
                                                      outlined dense
                                                      readonly
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="startDate" no-title @input="mShowChartRow = false,mStartDate = false,IsValidDate()" format="DD-MM-YYYY"
                                                   :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                </v-menu>

                            </v-col>
                            <v-col class="pt-0" sm="2" cols="12">
                                <!--<v-select dense label="End Date" outlined hide-details></v-select>-->
                                <v-menu v-model="mEndDate"
                                        :close-on-content-click="false"
                                        :nudge-right="40"
                                        transition="scale-transition"
                                        offset-y
                                        min-width="290px">
                                    <template v-slot:activator="{ on }">
                                        <v-text-field v-model="computedEndDate"
                                                      label="End Date"
                                                      outlined dense
                                                      readonly
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="endDate" no-title @input="mShowChartRow = false,mEndDate = false,IsValidDate()"
                                                   :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                </v-menu>
                            </v-col>

                            <v-col class="pt-0" sm="4" cols="12">
                                <v-select dense :items="lstActivity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Activity" outlined hide-details class="mr-2" v-model="ActivityId" @change="mShowChartRow = false">
                                </v-select>
                            </v-col>

                            <v-col class="pt-0 d-flex justify-space-around" sm="3" cols="12">
                                <Secondary-Button title="Generate" @click.native="GetPivotPresetList()"></Secondary-Button>
                                <!-- <Default-Button-Outlined title="Clear All" @click.native="fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(SurveyID),fnGetBeneficiaryData(SurveyID)"></Default-Button-Outlined> -->
                                <Default-Button-Outlined title="Clear All" @click.native="(fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(SurveyID),fnClearSelections())"></Default-Button-Outlined>
                            </v-col>

                            <!-- <v-col class="pt-0" sm="3" cols="12">
                                <v-btn :disabled="!ProjectId" :to="{name: 'OutputTable', query: {ProjID: ProjectId} }" color="green" dark>Report Table</v-btn>
                            </v-col> -->
                        </v-row>

                        <v-tabs v-model="tabList" class="pl-4 mb-5" v-if="mShowChartRow">
                            <v-tab href="#sec-preset">Preset</v-tab>
                        </v-tabs>
                        <v-tabs-items v-model="tabList">
                            <v-tab-item value="sec-preset">
                                <!--<h1>Preset</h1>-->
                                <v-row>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                        <template>

                                            <v-card class="preset-card elevation-0">
                                                <template v-if="loader" class="mx-auto">
                                                    <v-progress-linear :size="70"
                                                                       color="primary"
                                                                       indeterminate></v-progress-linear>
                                                </template>
                                                <div class="d-flex flex-row">
                                                    <v-card-title>
                                                        {{loader ? 'Loading...' : presetName}}
                                                    </v-card-title>
                                                    <div id="controlx" class="ml-auto mt-4 mr-2" v-if="!loader">
                                                        <v-btn icon outlined @click="prevPreset" class="mr-1" color="blue" small :disabled="Boolean(!cursor)">
                                                            <v-icon>mdi-chevron-left</v-icon>
                                                        </v-btn>
                                                        <v-btn icon outlined @click="nextPreset" class="mr-1" color="blue" small :disabled="Boolean(!presetOptions.length) || Boolean(presetOptions.length && cursor == presetOptions.length -1)">
                                                            <v-icon>mdi-chevron-right</v-icon>
                                                        </v-btn>
                                                    </div>
                                                </div>
                                                <v-divider />
                                                <div id="output" class="pa-5 v-scrolling-div" style="overflow-x:scroll"></div>
                                            </v-card>
                                        </template>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                        </v-tabs-items>
                    </v-container>
                </template>
            </v-col>
        </v-row>
        <v-row>

        </v-row>
        <div class="updatedSince" v-if="OutputOverview.LastExecutedDateTime">
          <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                  <span v-bind="attrs" v-on="on">
                      last updated: {{timeAgo}} ago
                  </span>
              </template>
              <span>Updation date: {{OutputOverview.LastExecutedDateTime}}</span>
          </v-tooltip>
        </div>
        <v-snackbar app top v-model="snackStatus.value" :color="snackStatus.color">
            {{snackStatus.message}}
        </v-snackbar>
    </v-container>
</template>

<script>
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '@/utils.js';

    var objManageProjectSurvey;
    var objDashboard;
    var objManageProjectActivity;

    async function importscript() {
        return Promise.all([
            import("../../../BL/ProjectSurvey.js").then((mod) => {
                objManageProjectSurvey = new mod.ProjectSurvey();
            }),

            import("../../../BL/Dashboard.js").then(mod => {
                objDashboard = new mod.Dashboard();
            })
        ]);
    };


    export default {
        data() {
            return {
                lstproject: [], 
                lstActivity: [],
                ActivityId : 0,
                skill: 20,
                knowledge: 33,
                power: 78,
                mShowChartRow: false,
                TotResponse: 0,
                TotComplete: 0,
                TotResponseCount: 0,
                TotCompleteCount: 0,
                lstLogFrame: [],
                AuditLog: [],
                FiterStartDate: "",
                FiterEndDate: "",
                ProjectStartDate: "",
                ProjectEndDate: "",
                userimage: "",
                QuestTotResponse: 0,
                QuestCreateDate: "",
                QuestUpdateDate: "",
                SurveyBeneficiary: "",
                SurveyPeriodicity: "",
                QuestionTypeBarLine: "",
                mStartDate: false,
                mEndDate: false,
                endDate: null,
                startDate: null,
                Msg: "",
                SurveyProgress: 0,
                SurveyID: 0,
                BeneficiaryID: 0,
                QuestionID: 0,
                Numb: 0,
                PositionLR: 0,
                offset: 0,
                QuestionN1: "",
                QuestionN2: "",
                Beneficiarymin: 0,
                Projectmin: 0,
                Questionmin: 0,
                WorkspaceID: 0,
                ProjectId: 0,
                outputSurveyIds : 0,
                total_output: require("@/assets/Outputs/totaloutput.svg"),
                pending: require("@/assets/Outputs/totalpending.svg"),
                completed: require("@/assets/Outputs/totalcompleted.svg"),
                tabList: 'sec-preset',
                presetName: "No Presets",
                presetOptions: [],
                cursor: 0,
                currentPreset: '',
                loader: false,
                plotlyLayout: {
                    width: 500,
                    height: 400,
                },
                PresetResponseData: [],
                OutputOverview: {
                  ActivityID: 0,
                  ActivityProgress: 0,
                  CompletedCount: 0,
                  PendingCount: 0,
                  TotalCount: 0,
                  LastExecutedDateTime: false,
                },
                snackStatus : {
                    value: false,
                    message: "",
                    color: ""
                }
            }
           
        },
        mounted: async function () {
            await importscript();
            this.WorkspaceID = this.$route.query.WsID;
            this.userimage = window.SERVER_URL + "/Upload";
            await this.fnGetProjectData(this.WorkspaceID);

        },
        computed: {
            FormattedDate() {
                return this.formatDate(this.FrmDate)
            },
            computedStartDate() {
                return this.formatDate(this.startDate)
            },
            computedEndDate() {
                return this.formatDate(this.endDate)
            },
            computedPresetResponse() {
                let data = this.PresetResponseData
                let format = objUtils.common.formatExcelDate

                if(data.length) {
                    if(this.startDate && this.endDate){
                        let filteredData = data.filter((e)=> {
                            return format(e.SubmittedDate) <= this.endDate && format(e.SubmittedDate) >= this.startDate
                        })
                        return filteredData
                    }
                    if(this.startDate) {
                        let filteredData = data.filter((e)=> format(e.SubmittedDate) >= this.startDate)
                        return filteredData
                    }
                    if(this.endDate){
                        let filteredData = data.filter((e)=> format(e.SubmittedDate) <= this.endDate)
                        return filteredData
                    }
                }
                return data
            },
            timeAgo(){
              if(this.OutputOverview.LastExecutedDateTime){
                return objUtils.common.timeSince(new Date(this.OutputOverview.LastExecutedDateTime))
              }
            }
        },
        methods: {
            async fnGetProjectData(WorkspaceID) {
                this.PreLoader = true;
                this.Projectmin = 0;
                this.Questionmin = 0;
                this.Beneficiarymin = 0;
                var result = await objManageProjectSurvey.GetProjectData(WorkspaceID);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstproject = result.data;
                    if (this.lstproject.length != 0) {
                        this.Projectmin = 1;
                    }
                }
                this.PreLoader = false;
            },

            formatDate(date) {
                if (!date) return null
                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg =
                        "Invalid Date : Start date should not be less than Project Start Date - " +
                        this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg =
                        "Invalid Date : Start date should not be greater than Project End Date - " +
                        this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (
                    Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))
                ) {
                    this.Msg =
                        "Invalid Date : End date should not be greater than Project End Date - " +
                        this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg =
                        "Invalid Date : End date should be greater than Project Start Date (" +
                        this.formatDate(this.ProjectStartDate.substr(0, 10)) +
                        ")";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                // startdate/ Enddate  not < todays date
                //enddate  any valid range - Max value
            },
            async fnGetSurveyData(ProjectId, startDate, endDate) {
                this.PreLoader = true;
                this.SurveyID = 0;
                this.QuestionID = 0;
                this.BeneficiaryID = 0;
                this.Questionmin = 0;
                this.Beneficiarymin = 0;

                var result = await objManageProjectSurvey.GetSurveyData(ProjectId, startDate, endDate);

                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    const index = this.lstproject.findIndex((element, index) => {
                        if (element.Key === ProjectId) {
                            return true
                        }
                    });
                    this.Projectmin = (index + 1);
                    this.lstsurvey = result.data;
                }
                this.PreLoader = false;
            },

            
            async fnGetProjectActivityData(ProjectId, startDate, endDate) {
                this.PreLoader = true;
                var result = await objManageProjectSurvey.GetProjectActivityData(ProjectId, startDate, endDate);

                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    const index = this.lstproject.findIndex((element, index) => {
                        if (element.Key === ProjectId) {
                            return true
                        }
                    });
                    this.Projectmin = (index + 1);
                    this.lstActivity = result.data;
                    this.OutputOverview = result.OutputOverview
                }
                this.PreLoader = false;
            },

            async GetSurveyProgress(ProjectId) {
                this.PreLoader = true;
                this.SurveyProgress = 0;
                var result = await objManageProjectSurvey.GetSurveyProgress(ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {

                        this.SurveyProgress = result.data.toFixed();
                    }
                }
                this.PreLoader = false;
            },
            async fnGetQuestionData(SurveyID) {
                this.PreLoader = true;
                this.QuestionID = 0;
                this.BeneficiaryID = 0;
                this.Questionmin = 0;
                var result = await objManageProjectSurvey.GetQuestionData(SurveyID);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstquestion = result.data;
                    if (this.lstquestion.length != 0) {
                        this.Questionmin = 1;
                        this.QuestionID = this.lstquestion[0].Key;
                    }
                }
                this.PreLoader = false;

            },
            async fnGetQuestionname(QuestionID) {

                if (this.QuestionID != 0) {
                    if (this.lstquestion.length != 0) {

                        const index = this.lstquestion.findIndex((element, index) => {
                            if (element.Key === QuestionID) {
                                return true
                            }
                        });
                        this.Questionmin = (index + 1);

                        var QuestTemp = this.lstquestion.filter(x => x.Key == this.QuestionID);
                        this.QuestionN1 = QuestTemp[0].Value;
                        this.QuestionN2 = QuestTemp[0].Value;
                    }
                }

            },
            async fnGetBeneficiaryPos(BeneficiaryID) {
                if (this.BeneficiaryID != 0) {
                    if (this.lstbeneficiary.length != 0) {
                        const index = this.lstbeneficiary.findIndex((element, index) => {
                            if (element.Key === BeneficiaryID) {
                                return true
                            }
                        });
                        this.Beneficiarymin = (index + 1);
                    }
                }
            },
            async FetchAllProjectDet(ProjectId) {
                this.PreLoader = true;
                var result = await objManageProjectSurvey.GetDetailProjectData(ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.TotResponse = result.data.SurveyTotResponse;
                        this.TotComplete = result.data.SurveyTotComplete;
                        this.TotResponseCount = result.data.SurveyTotResponseCount;
                        this.TotCompleteCount = result.data.SurveyTotCompleteCount;
                    }

                var resultA = await objManageProjectSurvey.GetSurveyData(ProjectId, null, null);
                this.lstsurveyTot = resultA.data;

                this.PreLoader = false;
            },

            async fnGetOutputSurveyIds() {
                var result = await objManageProjectSurvey.GetOutputSurveyIds(this.ActivityId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    var surveyIds = result.data;
                    this.outputSurveyIds = parseInt(surveyIds);
                } else {
                    this.Msg = "Failed to get activity details.";
                    this.errorSnackbarMsg = true;
                }
            },

            async GetPivotPresetList() {
                this.presetOptions = []
                this.cursor = 0        
                this.loader = true;
                var data = new FormData();
                var abc = this.ActivityId;
                data.append("ActivityId", this.ActivityId);
                var result = await objManageProjectSurvey.GetOutputPivotPresetList(data);
                if (result !== null && result != undefined && result != '') {
                    $("#output").show();
                    result.forEach((r) => {
                        this.presetOptions.push({
                            PresetName: r.presetName,
                            PresetData: JSON.parse(r.presetData),
                        });
                    });

                    if (this.presetOptions.length) {
                        this.presetName = this.presetOptions[this.cursor].PresetName;
                        this.currentPreset = this.presetOptions[this.cursor].PresetData;
                        await this.loadPivotTable();
                        this.loader = false;
                    }
                    else {
                        this.loader = false;
                        this.PresetResponseData = "";
                    }
                }
                else {
                    this.presetName = "No Presets";
                    this.loader = false;
                    this.outputSurveyIds = 0;
                    $("#output").hide();
                }

            },
            async loadPivotTable() {
                let renderers = {};
                $.extend(renderers, $.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                this.loader = true;
                await this.fnGetOutputSurveyIds();
                var acc = this.outputSurveyIds;
                var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                url += "?url=getpivotdata?surveyid=" + this.outputSurveyIds;

                try {
                    let data = await fetch(url, {
                        method: "POST",
                        body: ''
                    })
                    if(!data.ok){
                        throw new Error(data.statusText)
                    }
                    let res = await data.json();
                    this.PresetResponseData = res;

                    if (this.PresetResponseData.length) {
                        $("#output").show();
                        await this.appendFilePath(this.PresetResponseData);
                        let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                        this.$nextTick(() => {
                            $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                        })
                        this.loader = false
                    } else {
                        this.presetName = "No Presets";
                        this.loader = false;
                        this.PresetResponseData = "";
                        $("#output").hide();
                    }
                } catch (error) {
                    console.error(error);
                    this.presetOptions = []
                    this.presetName = "Error"
                    this.snackStatus = {
                        value : true,
                        message: "An Error occurred while fetching Pivot Table Data",
                        color: 'error'
                    }
                    this.loader = false
                }
            },
            nextPreset() {
                if(this.cursor == this.presetOptions.length - 1){
                    return
                }
                this.cursor++;
                if (this.presetOptions.length && this.PresetResponseData.length) {
                    this.currentPreset = this.presetOptions[this.cursor].PresetData;
                    this.presetName = this.presetOptions[this.cursor].PresetName;
                    let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                    let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                    this.$nextTick(() => {
                        $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                    })
                }
            },
            prevPreset() {
                if(this.cursor == 0) {
                    return
                }
                this.cursor--;
                if (this.presetOptions.length && this.PresetResponseData.length) {
                    this.currentPreset = this.presetOptions[this.cursor].PresetData;
                    this.presetName = this.presetOptions[this.cursor].PresetName;
                    let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                    let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                    this.$nextTick(() => {
                        $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                    })
                }
            },
            async appendFilePath(input) {
                const surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectId.toString() + "/";
                const beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.ProjectId.toString() + "/";
                const allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar'];

                for (let [i, v] of input.entries()) {
                    for (let key of Object.keys(v)) {
                        if (v[key]) {
                            let cursor = v[key].toString()
                            let filextension = cursor.split('.').pop();
                            if (objUtils.common.checkIfValidlatitudeAndlongitude(v[key])) {
                                v[key] = objUtils.GPS_TARGET_URL + v[key] + "?hl=en";
                            }
                            if (filextension && allowedExtensions.includes(filextension)) {

                                if (key.includes('Beneficiary')) {
                                    v[key] = beneficiaryfilesURL + v[key]
                                } else {
                                    v[key] = surveyfilesURL + v[key]
                                }
                            }
                        }
                    }
                }
            },
            fnClearSelections(){
              this.mStartDate = null
              this.mEndDate = null
              this.ActivityId = 0,
              this.presetName = "No Presets"
              this.presetOptions = []
              $('#output').pivotUI({}, [])
              $('#output').hide()
              this.cursor = 0
              this.loader = false
            }
        },
        watch: {
            cursor: function (val) {
                if (val > this.presetOptions.length - 1) {
                    this.cursor = this.presetOptions.length - 1;
                }

                if (val <= 0) {
                    this.cursor = 0;
                }
            },
        },
        components: {
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined
        },
       
    }
   
</script>

<style scoped>
#output-container{
  position: relative;
}
.updatedSince{
  position: absolute;
  top: 1.5em;
  right: 1.5em;
}

</style>