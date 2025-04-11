<template>
  <v-card class="response-card v-scrolling-div" style="overflow:scroll; max-height: 60vh; min-height: fit-content; width: 50vw" >
    <template v-if="loader" class="mx-auto">
      <v-progress-linear
        :size="70"
        color="primary"
        indeterminate
      ></v-progress-linear>
    </template>
    <v-card-title>
      {{SurveyResponseDetails ? SurveyResponseDetails.surveyName : 'Loading..'}}
    </v-card-title>
    <hr>
    <v-expansion-panels class="pa-1" v-if="SurveyResponseDetails" v-model="panel">
      <v-expansion-panel
        v-for="(item,i) in SurveyResponseDetails.surveySectionList"
        :key="i"
      >
        <v-expansion-panel-header>
          {{item.sectionName}}
        </v-expansion-panel-header>
        <v-expansion-panel-content class="py-1">
          <v-card v-for="(aitem, a) in item.surveyQuestionList" :key="a" class="my-2">
            <v-card-subtitle>
              {{`${aitem.questionName}`}}
            </v-card-subtitle>

            <!-- GPS -->

            <v-card-text v-if="aitem.questionType == 'GPS Capture' && loadMiniMap(aitem.answer)">
              <div id="minimap" ref="minimapref" style="width: 100%; height: 300px"></div>
            </v-card-text>
            
            <!-- END GPS -->

            <!-- Photo -->
            <v-card-text v-else-if="aitem.questionType == 'Photo Capture' || aitem.questionType == 'File Upload' && getPhotoUrl(aitem.answer)" >
              <v-img
                :src="getPhotoUrl(aitem.answer)"
                class="mx-auto"
              >
              </v-img>
            </v-card-text>
            <!-- End Photo -->

            <!-- Other Files -->
            <v-card-text v-else-if="aitem.questionType == 'File Upload' || aitem.questionType == 'Audio Capture' && getFileURL(aitem.answer)">
              <v-btn depressed :href="getFileURL(aitem.answer)" target="_blank" color="primary" block>
                {{aitem.answer}}
                <v-icon class="ml-3" small>
                  mdi-download
                </v-icon>
              </v-btn>
            </v-card-text>
            <!-- end -->
            <v-card-text v-else>
              <span class="px-3">
                  Ans.
                </span>
                <v-text-field hide-details readonly outlined class="pa-2" v-model="aitem.answer"/>
            </v-card-text>
            <v-card-subtitle v-if="!aitem.answer">
              "No Answer"
            </v-card-subtitle>
          </v-card>
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>
</template>

<script>
import { Loader } from "@googlemaps/js-api-loader";
import { createNamespacedHelpers } from "vuex";
const { mapGetters, mapActions } = createNamespacedHelpers("MapReport");

const loader = new Loader({
    // apiKey: "AIzaSyA1nBl8IF-i6Lxc9L65EGqMe9EauXJJF4Y",
    apiKey: "AIzaSyBDC7AC24l6C-hBzxM_z5cqUDFAAs2X4Oc",
});
export default {
    name: 'ResponseWindow',
    props: ['data'],
    computed: {
      ...mapGetters([
        "Map",
        "SurveyResponseDetails",
        "SelectedFilters"
      ])
    },
    data(){
        return {
          ResponseData : [],
          loader: false,
          panel: [],
        }
    },
    async mounted(){
        this.loader = true
        this.Map.panBy(-200, 0);      
        this.loadResponseDetails() 
    },
    destroyed() {
        this.$bus.$emit('closeResponseWindow')
        this.Map.panBy(200, 0);
    },
    methods:{
      ...mapActions([
        "fnGetSurveyResponseDetails"
      ]),
      async loadResponseDetails(){
        await this.fnGetSurveyResponseDetails({
          SurveyID: this.data.SurveyID,
          SyncDBID: this.data.SyncDBID,
          SubmissionCount: this.data.SubmissionCount
        }).then(()=> {
          this.loader = false
        })
      },
      getCenterCoordinates(coordinates){
        let lat,lng
        lat = parseFloat(coordinates.split(',')[0])
        lng = parseFloat(coordinates.split(',')[1])
        let dat = new google.maps.LatLng(lat,lng)
        return dat
      },
      getPhotoUrl(file){
        let imageExtensions = ['jpg','jpeg','gif','png','bmp','tiff','tif','ico','svg','webp']
        let filextension = file.split('.').pop()
        if(imageExtensions.includes(filextension)){
          let surveyURL = `${window.SERVER_URL}/Upload/SureyQuestionnaire/${this.SelectedFilters.Project.ProjectID.toString()}/`
          return `${surveyURL}${file}`
        } else return ''
      },
      getFileURL(file){
        let fileExtensions = ['pdf','txt','doc','docx','mp3','xlx','xlsx','csv']
        let filextension = file.split('.').pop()
        if(fileExtensions.includes(filextension)){
          let surveyURL = `${window.SERVER_URL}/Upload/SureyQuestionnaire/${this.SelectedFilters.Project.ProjectID.toString()}/`
          return `${surveyURL}${file}`
        } else return ''
      },
      loadMiniMap(coordinates){
        let minimap
        loader.load().then((google)=> {
          minimap = new google.maps.Map(document.getElementById('minimap'), {
            center: this.getCenterCoordinates(coordinates),
            zoom: 13,
            disableDefaultUI: true,
            zoomControl: false,
            mapTypeControl: false,
            scaleControl: false,
            streetViewControl: false,
            rotateControl: false,
            fullscreenControl: false,
            disableDefaultUi: false
          });

          new google.maps.Marker({
            position: this.getCenterCoordinates(coordinates),
            map: minimap,
            draggable: true,
            animation: google.maps.Animation.DROP,
          });
        })
        return true
      }
    },
    watch:{},
}
</script>

<style>
.resonse-card {
  min-width: 400px;
  max-width: 600px;
  min-height: 450px;
  max-height: 550px;
}
</style>