<template>
  <v-card class="card_main_wrapper">
    <!-- information card -->
    <v-card flat class="info__card pl-5" color="#EBF5FF">
      <v-icon large class="info__card-icon"> mdi-map-marker-outline </v-icon>
      <v-row>
        <v-col class="title pt-1 pl-5">
          <v-card-title class="pt-1" style="text-transform: capitalize; font-size: 1.2rem">{{data.StateName || data.DistrictName || data.CardName}}</v-card-title>
          <v-card-subtitle class="pb-1"> {{data.ResponseCount || data.QuestionCount}} • {{type.toLowerCase().includes('responses') ? 'Questions' : 'Responses'}} </v-card-subtitle>
        </v-col>
        <p class="align-self-center mr-10 font-weight-medium blue--text" >{{`${type} Card`}}</p>
      </v-row>
    </v-card>

    <!-- inline element -->
    <v-card flat class="info__subcard" v-if="!custom">
        
        <!-- <div class="d-flex flex-row align-end justify-start ">
            <v-list-item dense >
              <v-list-item-icon>
                <v-icon>mdi-account-outline</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-subtitle>
                  {{data.BeneficiaryName}}
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-list-item dense>
              <v-list-item-content>
                <v-list-item-subtitle>
                  •
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-list-item dense >
              <v-list-item-icon>
                <v-icon>mdi-calendar-blank-outline</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-subtitle>
                  {{getFormattedDate(data.ResponseStartDate) + ' - ' + getFormattedDate(data.ResponseEndDate)}}
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
        </div> -->

        <v-card-subtitle class="d-flex flex-row justify-start subtitle_list-items pb-0">
          <li class="mr-2" style="list-style-type: none" v-if="data.BeneficiaryName"><span><v-icon small>mdi-account-outline</v-icon></span> {{data.BeneficiaryName}} <span class="mx-2" style="list-style-type: none">•</span></li>
          <li class="mr-2" style="list-style-type: none"><span><v-icon small>mdi-calendar-blank-outline</v-icon></span> {{getFormattedDate(data.ResponseStartDate) + ' - ' + getFormattedDate(data.ResponseEndDate)}}</li>
        </v-card-subtitle>
    </v-card>
    
    <!-- slot for reuse -->
    <slot name="custom_card"></slot>
    <!-- Actions -->
    <div class="actions__group" v-if="!custom">
      <v-list-item dense>
        <v-list-item-content>
          <v-list-item-title> View Chart </v-list-item-title>
        </v-list-item-content>
        <v-list-item-icon>
          <v-btn icon small @click="togglePresetWindow" :disabled="!data.isPresetEnabled" class="toggle-btn">
            <v-icon small color="black"> {{!presetToggle ? 'mdi-arrow-right' : 'mdi-close'}} </v-icon>
          </v-btn>
        </v-list-item-icon>
      </v-list-item>
      <v-list-item dense>
        <v-list-item-content>
          <v-list-item-title> View Location </v-list-item-title>
        </v-list-item-content>
        <v-list-item-icon>
          <v-btn icon small @click="loadNext" class="toggle-btn">
            <v-icon small color="black"> mdi-fullscreen </v-icon>
          </v-btn>
        </v-list-item-icon>
      </v-list-item>
    </div>
  </v-card>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('MapReport')
export default {
  props: ['data', 'custom', 'type'],
  data(){
    return {
      presetToggle: false,
    }
  },
  mounted() {
    this.$bus.$on('closePresetWindow', () => {
      this.presetToggle = false
    })
  },
  computed: {
    ...mapGetters([
      "Step",
      "Map",
      "MarkerData",
      "CurrentStateName",
      "loadStatus",
      "DistrictList",
      "GeoDataList",
      "CallCardEvent",
      "Markers"
    ])
  },
  methods: {
    ...mapActions([
      "fnSetStep",
      "fnSetCardState",
      "fnGetDistrictWiseSurveyResponse",
      "fnGetResponseGeoCoordinates",
      "fnRemoveMapFeatures",
      "fnAddStateBounds",
      "fnAddDistrictBounds",
      "fnAddGeoMarkers",
      "fnSetCurrentStateName",
      "fnFilterDistrictGeoJson",
      "fnSetLoading",
      "fnSetTitle",
      "fnLoadStateJson",
      "fnGetCenterPoints"
    ]),
    async loadNext(){
      this.fnSetLoading(true)
      switch(this.Step){
        case 1:
          let stateName = this.data.StateName.trim()
          await this.fnGetDistrictWiseSurveyResponse({StateName: stateName})
          this.fnSetCurrentStateName(stateName)
          this.fnAddStateBounds(stateName)
          let source = await this.fnLoadStateJson()
          this.fnFilterDistrictGeoJson(source)
          let markers = []
          this.MarkerData.forEach(x => {
            this.DistrictList.forEach( z => {
              if(z.DistrictName.trim().toLowerCase() == x.Name.toLowerCase()){
                markers.push(x)
              }
            })
          })
          this.fnAddGeoMarkers(markers);
          this.fnSetStep(this.Step + 1)
          let sbounds = new google.maps.LatLngBounds(); 
          this.Map.data.forEach((feature)=>{
              feature.getGeometry().forEachLatLng((latLng)=>{
                  sbounds.extend(latLng);
              })
          })
          this.Map.fitBounds(sbounds);
          this.Map.setZoom(6.5);
          this.Map.panBy(-200, 0)
          this.fnSetLoading(false)
          break
        case 2:
          let districtName = this.data.DistrictName.trim()
          await this.fnGetResponseGeoCoordinates({DistrictName: districtName})
          let { coords } = this.MarkerData.find(x => {
            return x.Name.toLowerCase() == districtName.toLowerCase()
          })
          if (coords && this.Markers.length){
            this.Markers.forEach((ft)=>{
              ft.setMap(null)
            })
          }
          // let StateJson = await this.fnLoadStateJson()
          // this.fnAddDistrictBounds({DistrictName: districtName, StateJson})
          // let dtData = this.MarkerData.filter((x)=> x.Name.toLowerCase() == districtName.toLowerCase())
          let cMarkers = []
          this.GeoDataList.forEach(x => {
            let geo = x.Geo_coordinates.split(',')
            let c = new google.maps.LatLng(geo[0], geo[1])
            let ob = {
              Name: c.toString(),
              coords: c.toJSON(),
              num: "1"
            }
            cMarkers.push(ob)
          })
          this.fnAddGeoMarkers(cMarkers, 3)
          google.maps.event.addListenerOnce(this.Map, 'zoom_changed', () => {
            this.Map.setZoom(10)
            return
          })
          this.fnSetStep(this.Step + 1)
          let dbounds = new google.maps.LatLngBounds(); 
          this.Markers.forEach(z => {
            dbounds.extend(z.getPosition())
            z.addListener('click', () => {
              this.$bus.$emit('openResponseWindow', z)
              if(z.getLabel().text !== 'X') {
                z.setLabel({
                  text: 'X',
                  color: "white"
                })
                return
              }
              z.setLabel({
                text: '1',
                color:"white"
              })
            })
          })
          this.Map.fitBounds(dbounds);
          this.fnSetTitle(districtName)
          this.Map.panBy(-200, 0)
          this.fnSetLoading(false)
          break
        default:
          this.fnSetLoading(false)
          break
      }
      this.$bus.$emit('closePresetWindow')
    },
    togglePresetWindow(){
      this.presetToggle = !this.presetToggle
      this.fnSetCardState({
        ...this.data,
        type: this.type,
      })
      this.$bus.$emit('togglePresetWindow')
    },
    getFormattedDate(i){
      let date = new Date(i)
      let formattedDate = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`
      return formattedDate
    }
  },
  watch: {
    CallCardEvent: function(val){
      if(!val && typeof val === 'boolean') return
      if(typeof val === 'string' && val.toLowerCase() == (this.data.StateName?.trim().toLowerCase() || this.data.DistrictName?.trim().toLowerCase())){
        this.loadNext()
      }
    },
  }
};
</script>

<style scoped>
.card_main_wrapper {
  padding: 10px;
  border-radius: 16px !important;
  border: 2px solid #DADBE6;
}
.info__card{
  border-radius: 8px !important;
}
.info__subcard {
  justify-content: space-around;
}
.info__card-icon {
  position: absolute;
  top: -3px;
  left: -3px;
  font-size: 15px !important;
  padding: 8px;
  color: white;
  border-radius: 50% !important;
  outline: 4px solid white;
  background-color: #399cff;
}
</style>