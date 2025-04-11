<template>
  <v-container fluid>

    <v-row justify="center">
      <v-col cols="12" xs="12" sm="12" md="11">
        <v-card style="height: 1100px" elevation="1" id="outcome-container">
          <div class="updatedSince" v-if="OutcomesOverview.LastExecutedDateTime">
            <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                    <span v-bind="attrs" v-on="on">
                        last updated: {{timeAgo}} ago
                    </span>
                </template>
                <span>Updation date: {{OutcomesOverview.LastExecutedDateTime}}</span>
            </v-tooltip>
          </div>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
              <v-container class="pb-0" fluid>
                <v-row>
                  <v-col sm="12" cols="12" class="pt-1">
                    <h1 class="pl-4" style="color: #2D2F40; font-size: 17px; font-weight: bold">
                      Outcome Overview
                    </h1>
                  </v-col>
                </v-row>
                <v-row class="pl-4">
                  <v-col sm="5" cols="12">
                    <v-select
                      dense
                      label="Select Project"
                      outlined
                      hide-details
                      class="mr-2"
                      :items="lstProject"
                      :item-value="i => i.Key"
                      :item-text="i => i.Value"
                      v-model="ProjectId"
                      @change="populateOutcomes(ProjectId, sDate, eDate)"
                    ></v-select>
                  </v-col>
                </v-row>
              </v-container>
            </v-col>
          </v-row>

          <!-- Overview Counts -->

          <v-row class="px-3">
            <v-col sm="2" cols="12" class="pt-8 pb-0">
                <v-row>
                    <v-col sm="5" cols="12" class="pt-0" style="text-align: end">
                        <v-avatar>
                            <img :src="total_outcome" alt="" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutcomesOverview.TotalCount}}</span>
                        </v-col>
                        <v-col sm="12" class="pt-0 pl-0">
                            <span style="font-size: 12px">Total Outcomes</span>
                        </v-col>
                    </v-col>
                </v-row>
            </v-col>
            <v-col sm="3" cols="12" class="pt-8 pb-0">
                <v-row>
                    <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
                        <v-avatar>
                            <img :src="pending" alt="" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 ">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutcomesOverview.PendingCount}}</span>
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
                            <img :src="completed" alt="" style="font-size: 60px" />
                        </v-avatar>
                    </v-col>
                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                        <v-col sm="12" class="py-0 pl-0">
                            <span style="font-size: 21px; font-weight: bold">{{OutcomesOverview.CompletedCount}}</span>
                        </v-col>
                        <v-col sm="12" class="pt-0 pl-0">
                            <span style="font-size: 12px">Total Completed</span>
                        </v-col>
                    </v-col>
                </v-row>
            </v-col>

        </v-row>

          <!-- Individual Outcomes Analysis -->

          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <v-container class="pt-0 pb-0" fluid>
                <v-row>
                  <v-col sm="4" cols="12" class="pl-8 pt-0">
                    <v-row>
                      <v-col class="pb-0" sm="12" cols="12">
                        <span style="color: #2D2F40; font-size: 17px; font-weight: bold">Individual Outcome Analysis</span>
                      </v-col>
                    </v-row>
                  </v-col>

                  <v-row class="mx-5">
                    <v-col cols="2">
                      <!-- <v-menu v-model="mStartDate" :close-on-content-click="false" :nudge-right="40" transition="scale-transition"
                        offset-y min-width="290px">

                        <template v-slot:activator="{ on }">
                          <v-text-field v-model="computedStartDate" label="Start Date" outlined dense readonly v-on="on">
                          </v-text-field>
                        </template>
                        <v-date-picker v-model="startDate" no-title @input="mShowChartRow = false, mStartDate = false, IsValidDate()"
                          format="DD-MM-YYYY" :min="ProjectStartDate" :max="ProjectEndDate">
                        </v-date-picker>
                      </v-menu> -->
                      <v-menu v-model="datePicker1" :close-on-content-click="false" offset-y>
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field
                            v-model="sDate"
                            label="Start Date"
                            outlined
                            dense
                            readonly
                            v-on="on"
                            v-bind="attrs"
                          >

                          </v-text-field>
                        </template>
                        <v-date-picker 
                            v-model="sDate"
                            no-title
                            @change="datePicker1 = false"
                            format="DD-MM-YYYY"
                            :min="ProjectStartDate" 
                            :max="ProjectEndDate"
                          >
                          </v-date-picker>
                      </v-menu>
                    </v-col>
                    <v-col cols="2">
                      <v-menu v-model="datePicker2" :close-on-content-click="false" offset-y>
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field
                            v-model="eDate"
                            label="End Date"
                            outlined
                            dense
                            readonly
                            v-on="on"
                            v-bind="attrs"
                          >

                          </v-text-field>
                        </template>
                        <v-date-picker 
                            v-model="eDate"
                            no-title
                            @change="datePicker2 = false"
                            format="DD-MM-YYYY"
                            :min="sDate" 
                          >
                          </v-date-picker>
                      </v-menu>
                    </v-col>
                    <v-col cols="4">
                      <v-select 
                        dense 
                        hide-details 
                        label="Select Outcome" 
                        outlined
                        :items="lstOutcomes"
                        :item-value="i => i.Key"
                        :item-text="i => i.Value"
                        v-model="OutcomeId"
                        @change="populateSurveyIds(OutcomeId)"
                      >
                      </v-select>
                    </v-col>
                    <v-col>
                      <secondary-button title="Generate" class="mx-2" @click.native="GetPivotPresetList()"/>
                      <default-button-outlined title="Clear All" @click.native="clearFields()" class="mx-2"/>
                    </v-col>
                    <!-- <v-col>
                      <v-btn :disabled="!ProjectId" :to="{name: 'OutcomesTable', query: {ProjID: ProjectId} }" color="green" dark>Outcomes Table</v-btn>
                    </v-col> -->
                  </v-row>
                </v-row>
              </v-container>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <v-tabs v-model="tabList" class="pl-4 mb-5" v-show="false">
                  <v-tab href="#sec-preset">Preset</v-tab>
              </v-tabs>
              <v-tabs-items v-model="tabList">
                  <v-tab-item value="sec-preset">
                      <!--<h1>Preset</h1>-->
                      <v-row>
                          <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                              <template>
                                  <v-card class="preset-card elevation-0 mx-5">
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
                                              <v-btn icon outlined @click="prevPreset" :disabled="Boolean(!cursor)" class="mr-1" color="blue" small >
                                                  <v-icon>mdi-chevron-left</v-icon>
                                              </v-btn>
                                              <v-btn icon outlined @click="nextPreset" :disabled="Boolean(!presetOptions.length) || Boolean(presetOptions.length && cursor == presetOptions.length - 1)" class="mr-1" color="blue" small>
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
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
    <v-snackbar app top v-model="snackStatus.value" :color="snackStatus.color">
      {{snackStatus.message}}
    </v-snackbar>
  </v-container>
</template>
<script>
import objUtils from '@/utils.js';
import SecondaryButton from '@/components/secondary-button.vue'
import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

var objProjectOutcomes;

const importScript = async () => {
  return Promise.all([

    import('../../../BL/ProjectOutcomes').then(mod => {
      objProjectOutcomes = new mod.ProjectOutcomes();
    })

  ])
}

export default {
  components: {
    SecondaryButton,
    DefaultButtonOutlined
  },
  data() {
    return {
      lstProject: [],
      ProjectId: '',
      WSID : null,
      datePicker1: false,
      datePicker2: false,
      sDate: null,
      eDate: null,
      ProjectEndDate: null,
      ProjectStartDate: null,
      lstOutcomes: [],
      OutcomeId: null,
      plotlyLayout: {
        width: 500,
        height: 400,
      },
      PresetResponseData: [],
      presetName: "No Presets",
      presetOptions: [],
      cursor: 0,
      currentPreset: '',
      loader : false,
      tabList: 'sec-preset',
      surveyIds: [],
      total_outcome: require("@/assets/Outcomes/totaloutcomes.svg"),
      pending: require("@/assets/Outcomes/totalpending.svg"),
      completed: require("@/assets/Outcomes/totalcompleted.svg"),
      OutcomesOverview: {
        CompletedCount : 0,
        LastExecutedDateTime : false,
        OutcomeID: 0,
        OutcomeProgress: 0,
        PendingCount: 0,
        TotalCount: 0,
      },
      snackStatus: {
        value: false,
        message: "",
        color: ""
      }
    }
  },
  computed: {
    computedStartDate() {
      return this.formatDate(this.sDate)
    },
    computedEndDate() {
      return this.formatDate(this.eDate)
    },
    computedPresetResponse() {
      let data = this.PresetResponseData
      let format = objUtils.common.formatExcelDate

      if(data.length) {
          if(this.sDate && this.eDate){
              let filteredData = data.filter((e)=> {
                  return format(e.SubmittedDate) <= this.eDate && format(e.SubmittedDate) >= this.sDate
              })
              return filteredData
          }
          if(this.sDate) {
              let filteredData = data.filter((e)=> format(e.SubmittedDate) >= this.sDate)
              return filteredData
          }
          if(this.eDate){
              let filteredData = data.filter((e)=> format(e.SubmittedDate) <= this.eDate)
              return filteredData
          }
      }
      return data
    },
    timeAgo(){
      if(this.OutcomesOverview.LastExecutedDateTime){
        return objUtils.common.timeSince(new Date(this.OutcomesOverview.LastExecutedDateTime))
      }
    },
  },
  async mounted(){
    await importScript();
    this.WSID = this.$route.query.WsID
    await this.GetProjectData()
  },
  methods: {
    async GetProjectData(){
      let { data } = await objProjectOutcomes.GetOutComeProjectData(this.WSID)
      if(data.length) {this.lstProject = data}
    },
    formatDate(date) {
      if (!date) return null
      const [year, month, day] = date.split('-')
      return `${day}-${month}-${year}`
    },
    async populateOutcomes(ProjectId, sDate, eDate) {
      let { data, OutcomeOverview } = await objProjectOutcomes.getOutcomeListByProjectId(ProjectId, sDate, eDate)
      if(data.length){
        this.lstOutcomes = data
        this.OutcomesOverview = OutcomeOverview
      }
    },
    async populateSurveyIds(outcomeId) {
      let {status, data} = await objProjectOutcomes.GetOutcomeSurveyIds(outcomeId)
      
      if(status.toLowerCase() == 'success'){
        this.surveyIds.push({ outcomeId, surveyId: data })
      }
    },
    async GetPivotPresetList() {
      this.presetOptions = []
      this.cursor = 0      
      if(this.OutcomeId){
        this.loader = true;
        let result = await objProjectOutcomes.GetOutcomePivotPresetList(this.OutcomeId);
        if (result) {
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
      }
      else {
        return null
      }

    },
    async loadPivotTable() {
      let renderers = {};
      $.extend(renderers, $.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
      this.loader = true;
      try {

        let res = await objProjectOutcomes.GetPivotTableData(this.surveyIds[this.surveyIds.length - 1].surveyId)
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
        console.error(error)
        this.presetName = "Error"
        this.presetOptions = []
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
    clearFields(){
      this.OutcomeId = null
      this.sDate = null
      this.eDate = null
      this.surveyIds = []
      this.presetOptions = []
      this.presetName = "No Presets";
      $('#output').pivotUI({}, [])
      $("#output").hide();
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
};
</script>

<style scoped>
#outcome-container{
  position: relative;
}
.updatedSince{
  position: absolute;
  top: 1.5em;
  right: 1.5em;
}

</style>