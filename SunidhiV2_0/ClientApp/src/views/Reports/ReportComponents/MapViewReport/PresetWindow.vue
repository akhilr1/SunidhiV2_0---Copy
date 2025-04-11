<template>
  <v-card class="preset-card">
    <template v-if="loader" class="mx-auto">
      <v-progress-linear
        :size="70"
        color="primary"
        indeterminate
      ></v-progress-linear>
    </template>
    <div class="d-flex flex-row">
      <v-card-title>
        {{loader ? 'Loading...' : presetName}}
      </v-card-title>
      <div id="controlx" class="ml-auto mt-4 mr-2" v-if="!loader">
        <v-btn icon outlined @click="prevPreset" class="mr-1" color="blue" small :disabled="Boolean(!cursor)">
          <v-icon>mdi-chevron-left</v-icon>   
        </v-btn>
        <v-btn icon outlined @click="nextPreset" class="mr-1" color="blue" small :disabled="Boolean(!presetOptions.length) || Boolean(presetOptions.length && cursor == presetOptions.length - 1)">
          <v-icon>mdi-chevron-right</v-icon>
        </v-btn>
      </div>
    </div>
    <v-divider/>
    <div id="output" class="pa-5 v-scrolling-div" style="overflow-x:scroll"></div>
  </v-card>
</template>

<script>
import { createNamespacedHelpers } from "vuex";
const { mapGetters, mapActions } = createNamespacedHelpers("MapReport");
import PreLoader from '@/components/pre-loader.vue';
import objUtils from '@/utils.js'
var objSurvey;
async function importscript() {
    return Promise.all([
        import("@/BL/ProjectSurvey.js").then(mod => {
            objSurvey = new mod.ProjectSurvey();
        })
    ]);
};
export default {
  name: "PresetWindow",
  components: {
    PreLoader
  },
  data() {
    return {
      cardTitle: "Preset Chart",
      presetName: "No Presets",
      presetOptions: [],
      cursor: 0,
      currentPreset: '',
      loader: false,
      plotlyLayout: {
        width: 500,
        height: 400,
      }
    };
  },
  async mounted() {
    await importscript();
    this.loader = true
    this.Map.panBy(-200, 0);
    await this.fnGetPivotPresetList()
    .then(async ()=> {
      this.PivotPresetList &&
      this.PivotPresetList.forEach((r) => {
        this.presetOptions.push({
          PresetName: r.PresetName,
          PresetData: JSON.parse(r.PresetData),
        });
      })
      if(this.presetOptions.length) {
        this.presetName = this.presetOptions[this.cursor].PresetName
        this.currentPreset = this.presetOptions[this.cursor].PresetData
        await this.loadPivotTable()
      } else this.loader = false
    })
    .catch(error => {
      console.error(error)
    })
  },
  destroyed() {
    this.$bus.$emit("closePresetWindow");
    this.Map.panBy(200, 0);
  },
  computed: {
    ...mapGetters([
      "Map",
      "PivotPresetList",
      "PresetResponseData",
      "CurrentUser",
      "CardState",
      "CurrentStateName",
      "SelectedFilters"
    ]),
  },
  methods: {
    ...mapActions([
      "fnGetPivotPresetList", 
      "fnGetPresetResponseDetails"
      ]),
    async loadPivotTable() {
      let { type } = this.CardState;
      let renderers = {} 
      $.extend(renderers, $.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
      this.loader = true
      if(type.toLowerCase() == 'state'){
        await this.fnGetPresetResponseDetails({
          stateName: this.CardState.StateName.trim().toLowerCase(),
        })
      }
      if(type.toLowerCase() == 'district'){
        await this.fnGetPresetResponseDetails({
          stateName: this.CurrentStateName,
          districtName: this.CardState.DistrictName.trim().toLowerCase(),
        })
      }
      if(type.toLowerCase() == 'leaf'){
        await this.fnGetPresetResponseDetails({
          stateName: this.CurrentStateName,
          syncDBID: this.CardState.SyncDBID,
        })
      }

      if(this.PresetResponseData.length){
        await this.appendFilePath(this.PresetResponseData)
        let runOptions = {...this.currentPreset, renderers, rendererOptions: {...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false}
        this.$nextTick(() => {
          $("#output").pivotUI(this.PresetResponseData, runOptions, true);
        })
        this.loader = false
      } else {
        this.presetName = "No Presets"
        this.loader = false
      }
    },
    nextPreset(){
      if(this.cursor == this.presetOptions.length - 1){
        return
      }
      this.cursor++
      if(this.presetOptions.length && this.PresetResponseData.length) {
        this.currentPreset = this.presetOptions[this.cursor].PresetData
        this.presetName = this.presetOptions[this.cursor].PresetName
        let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
        let runOptions = {...this.currentPreset, renderers, rendererOptions: {...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false}
        this.$nextTick(() => {
          $("#output").pivotUI(this.PresetResponseData, runOptions, true);
        })
      }
    },
    prevPreset(){
      if(!this.cursor) {
        return
      }
      this.cursor--
      if(this.presetOptions.length && this.PresetResponseData.length) {
        this.currentPreset = this.presetOptions[this.cursor].PresetData
        this.presetName = this.presetOptions[this.cursor].PresetName
        let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
        let runOptions = {...this.currentPreset, renderers, rendererOptions: {...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false}
        this.$nextTick(() => {
          $("#output").pivotUI(this.PresetResponseData, runOptions, true);
        })
      }
    },
    async appendFilePath(input){
      const surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.SelectedFilters.Project.ProjectID.toString() + "/";
      const beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.SelectedFilters.Project.ProjectID.toString() + "/";
      const allowedExtensions = ['jpg','jpeg','gif','png','txt','mp3','pdf','doc','docx','xlx','xlsx','csv','sveg','zip','rar'];
      
      for(let [i, v] of input.entries()){
        for(let key of Object.keys(v)){
          if(v[key]){
            let cursor = v[key].toString()
            let filextension = cursor.split('.').pop();
            if (objUtils.common.checkIfValidlatitudeAndlongitude(v[key])){
                v[key] = objUtils.GPS_TARGET_URL + v[key] + "?hl=en";
            }
            if(filextension && allowedExtensions.includes(filextension)){
              // let data = new FormData();
              // data.append("FileName", v[key])
              // data.append("ProjectID", this.SelectedFilters.Project.ProjectID);
              // let isFileExists = await objSurvey.checkFileExists(data);
              // // let isFileExists = 2;
              // // if(!isFileExists){
              // //   v[key] = "File not found"
              // // }
              // if(isFileExists && isFileExists == 1){
              //   v[key] = surveyfilesURL + v[key];
              // } else if(isFileExists && isFileExists == 2){
              //   v[key] = beneficiaryfilesURL + v[key];
              // }
              if(key.includes('Beneficiary')){
                v[key] = beneficiaryfilesURL + v[key]
              } else {
                v[key] = surveyfilesURL + v[key]
              }
            }
          }
        }
      }
    },
  },
  watch: {
    cursor: function(val){
      if(val > this.presetOptions.length - 1){
        this.cursor = this.presetOptions.length - 1
      }

      if(val <= 0){
        this.cursor = 0
      }
    },
  }
};
</script>

<style scoped>
.preset-card {
  /* min-width: 400px;
  max-width: 600px;
  min-height: fit-content;
  max-height: 550px; */
  overflow: hidden;
  max-width: 55vw;
  min-width: 20vw;
}

#control{
  width: fit-content;
  float: right;
}

#output{
  min-height: 40vh;
  max-height: 60vh;
}
</style>