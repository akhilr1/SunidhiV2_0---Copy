<template>
    <!-- Left SideBar Card Variant -->
    <v-card class="left-sidebar" flat tile v-if="app_drawer">
        <!-- DetailBar-->
        <v-card class="px-4 py-2 detail-header" flat tile>
            <v-row >
                <v-col cols="1">
                    <v-btn depressed icon @click="fnBackStep" v-if="!disableFlag">
                        <v-icon>mdi-arrow-left</v-icon>
                    </v-btn>
                </v-col>
                <v-col class="d-flex flex-column justify-start pa-0 ma-0">
                    <v-col xs="12" class="pb-0">
                        <span>{{Response}}</span> •
                        <span>Responses</span>
                    </v-col>
                    <v-col xs="12" class="pa-0 pl-3 ma-0">
                        <h2 style="text-transform: capitalize">
                            <!-- need to work on this -->
                            {{mainTitle}} 
                        </h2>
                    </v-col>
                </v-col>
            </v-row>
            <v-row class="px-4">
                <v-col class="pa-0 ma-0">
                    <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1" v-if="GuestMode">{{CurrentVersionName.length > 25 ? `${CurrentVersionName.substring(0,25)}...` : CurrentVersionName }}</v-chip>
                    <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1">{{SelectedFilters.Project.ProjectName.length > 25 ? `${SelectedFilters.Project.ProjectName.substring(0,25)}...` : SelectedFilters.Project.ProjectName}}</v-chip>
                    <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1">{{SelectedFilters.Survey.SurveyName.length > 25 ? `${SelectedFilters.Survey.SurveyName.substring(0, 25)}...` : SelectedFilters.Survey.SurveyName}}</v-chip>
                    <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1">India</v-chip>
                </v-col>
            </v-row>
        </v-card>

        <!-- SearchBar -->
        <search-bar @search="fnSearch" class="pl-5" />
        
        <!-- Card Stack -->
        <v-stepper v-model="stepCursor" class="elevation-0 pl-4 pr-1">
            <v-stepper-content step="1" class="pa-1" tile>
                <!-- State Cards ? -->
                <v-sheet tile color="rgba(255,255,255, 0)" class="v-scrolling-div cards-wrapper" height="63vh">
                    <Cards v-for="(item, id) in stateContent" :key="id" :data="item" class="my-2" type="State"/>
                </v-sheet>
            </v-stepper-content>
            <v-stepper-content step="2" class="pa-1">
                <!-- District Cards -->
                <v-sheet tile color="rgba(255,255,255, 0)" class="v-scrolling-div cards-wrapper" height="63vh">
                    <Cards v-for="(item, id) in districtContent" :key="id" :data="item" class="my-2" type="District"/>
                </v-sheet>
            </v-stepper-content>
            <v-stepper-content step="3" class="pa-1">
                <!-- Individual Leaf Cards -->
                <v-sheet tile color="rgba(255,255,255, 0)" class="v-scrolling-div cards-wrapper" height="63vh">
                    <Cards :data="{
                        CardName:'Preset Card',
                        Response_Count: 80,
                    }" class="my-2" type="Leaf" custom="true" v-if="isPreset">
                    </Cards>
                    <ResponseCard :searchKey="searchField"/>
                </v-sheet>
            </v-stepper-content>
        </v-stepper>


        <v-app-bar-nav-icon
          id="navbar-icon"
          @click="closeDrawer()"
          v-if="!GuestMode"
        >
          <v-icon>
            mdi-menu
          </v-icon>
        </v-app-bar-nav-icon>

        <preset-window class="preset-window" v-if="presetToggle"/>
        <response-window class="preset-window" :data="responseData" v-if="rspWindowToggle"/>
    </v-card>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('MapReport')
import Cards from './MapCards.vue'
import SearchBar from './SearchBar.vue'
import ResponseCard from './ResponseCard.vue'
import PresetWindow from './PresetWindow.vue'
import ResponseWindow from './ResponseWindow.vue'
export default {
    components: {
        Cards,
        SearchBar,
        ResponseCard,
        PresetWindow,
        ResponseWindow
    },
    data(){
        return {
            app_drawer : false,
            searchField: '',
            topBarTitle: 'India',
            isPreset: false,
            presetToggle: false,
            rspWindowToggle: false,
            disableFlag: false,
            responseData: []
        }
    },
    async mounted(){
        this.$bus.$on('toggleAppDrawer', ()=>{
            this.app_drawer = true
        })  
        this.$bus.$on('next-step', (val)=> {
            this.topBarTitle = val
        })
        this.$bus.$on('togglePresetWindow', ()=>{
            this.presetToggle = !this.presetToggle
        })
        this.$bus.$on('toggleResponseWindow', (val)=>{
            this.rspWindowToggle = !this.rspWindowToggle
            this.responseData = val
        })
        this.$bus.$on('closeResponseWindow', ()=> {
            this.rspWindowToggle = false
        })
        this.$bus.$on('closePresetWindow', ()=>{
            this.presetToggle = false
        })
    },
    computed: {
        ...mapGetters([
            'StatesCount',
            'StatesList',
            'DistrictList',
            'DistrictCount',
            'Step',
            'SelectedFilters',
            'GeoDataList',
            'CurrentStateName',
            'GuestMode',
            'mainTitle',
            'Map',
            'MarkerData',
            'CurrentVersionName'
        ]),
        Response(){
            switch(this.Step){
                case 1:
                    let data = this.stateContent.reduce((total, item)=> total + parseInt(item.ResponseCount), 0)
                    return data
                case 2:
                    data = 0
                    data = this.districtContent.reduce((total, item)=> total + parseInt(item.ResponseCount), 0)
                    return data
                case 3:
                    // data = this.GeoDataList.reduce((total, item)=> total + parseInt(item.ResponseCount), 0)
                    return this.GeoDataList.length
                default: '0'
            }
        },
        stateContent(){
            // let data = this.StatesList.filter(item => item.isPresetEnabled)

            let data = this.StatesList
            let filteredList=[]
            if(!data.length) return []
            if(this.searchField){
               filteredList = data.filter((x)=> x.StateName.toLowerCase().includes(this.searchField.toLowerCase()))
            //   return filteredList
            } else {
                filteredList= data
            }
            return filteredList.sort((a, b) => (a.StateName > b.StateName) ? 1 : -1);
        },
        districtContent(){
            // let data = this.DistrictList.filter(item => item.isPresetEnabled)
            let data = this.DistrictList
            let filteredList=[]

            if(!data.length) return []
            if(this.searchField){
               filteredList = data.filter((x)=> x.DistrictName.trim().toLowerCase().includes(this.searchField.toLowerCase()))
            //    return filteredList
            } else {
                filteredList= data
            }
            return filteredList.sort((a, b) => (a.DistrictName > b.DistrictName) ? 1 : -1);

        },
        geoDataContent(){
            return this.GeoDataList
        },
        stepCursor: {
            get() {
                return this.Step
            },
            set(val) {
                this.fnSetStep(val)
            }
        },
    },
    methods: {
        ...mapActions([
            'fnSetStep',
            "fnGetDistrictWiseSurveyResponse",
            "fnGetResponseGeoCoordinates",
            "fnRemoveMapFeatures",
            "fnAddGeoBounds",
            "fnSetCurrentStateName",
            "fnSetTitle",
            "fnAddStateBounds",
            "fnLoadStateJson",
            "fnFilterDistrictGeoJson",
            "fnAddGeoMarkers",
            "fnFilterStateGeoJson"
        ]),
        toggleAppDrawer() {
            this.$bus.$emit("closedrawer")
            this.app_drawer = false
        },
        fnSearch(val){
            this.searchField = val
        },
        async fnBackStep(){
            if(this.Step <= 1){
                this.$bus.$emit('closedrawer')
                this.app_drawer = false
            }
            this.fnSetStep(this.Step - 1)
            this.rspWindowToggle = false
            this.presetToggle = false
            this.fnRemoveMapFeatures()
            this.Map.panBy(-200, 0)
            switch (this.Step) {
                case 1:
                    this.fnFilterStateGeoJson()
                    let sMarkers = []
                    this.MarkerData.forEach(x => {
                        this.StatesList.forEach(y => {
                        if(y.StateName.trim().toLowerCase() == x.Name.toLowerCase()){
                            sMarkers.push(x)
                        }
                        })
                    })
                    this.fnAddGeoMarkers(sMarkers);
                    let mBounds = new google.maps.LatLngBounds();
                    this.MarkerData.forEach((i) => {
                        mBounds.extend(i.coords);
                    });
                    this.Map.setOptions({ maxZoom: 7 })
                    this.Map.fitBounds(mBounds);
                    this.Map.setOptions({ maxZoom: undefined})
                    this.Map.panBy(-200, 0)
                    break;
                case 2:
                    this.fnAddStateBounds(this.CurrentStateName)
                    // this.fnFilterDistrictGeoJson(await this.fnLoadStateJson())
                    let dMarkers = []
                    this.MarkerData.forEach(x => {
                        this.DistrictList.forEach( z => {
                        if(z.DistrictName.trim().toLowerCase() == x.Name.toLowerCase()){
                            dMarkers.push(x)
                        }
                        })
                    })
                    this.fnAddGeoMarkers(dMarkers);
                    let sbounds = new google.maps.LatLngBounds(); 
                    this.Map.data.forEach((feature)=>{
                        feature.getGeometry().forEachLatLng((latLng)=>{
                            sbounds.extend(latLng);
                        })
                    })
                    this.Map.fitBounds(sbounds);
                    this.Map.panBy(-200, 0)
                    break;
                default:
                    break;
            }
        },
        closeDrawer(){
            this.$bus.$emit('closedrawer')
            this.app_drawer = false
            this.fnSetStep(0)
            this.fnSetCurrentStateName('')
            this.rspWindowToggle = false
            this.presetToggle = false
        }
    },
    watch: {
        app_drawer: function(val){
            if(!val){
                this.fnSetStep(0)
                this.fnSetCurrentStateName('')
            }
        },
        Step: function(val){
            if(val == 1)this.fnSetTitle('India')
            if(val == 2)this.fnSetTitle(this.CurrentStateName)
            if(val <= 1 && this.GuestMode){
                this.disableFlag = true
            } else {
                this.disableFlag = false
            }
        }
    }
}
</script>

<style scoped>
.left-sidebar{
    position: absolute;
    top: 0;
    bottom: 0;
    width: min-content;
    max-width: 40vw;
    min-width: 33%;
    background: rgba(255,255,255, 0.5);
}

/* .v-scrolling-div.cards-wrapper:hover::-webkit-scrollbar{
    display: none !important;
} */

::v-deep .v-stepper {
    background: rgba(255, 255, 255, 0.3);
    border-radius: 0;
}

#navbar-icon{
    position: absolute;
    top: 10px;
    right: -10%;
    padding: 5px;
    background: white;
    border-radius: 8px;
    box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.16);
}

.detail-header{
    border: 1px solid #DADBE6;
}

.cards-wrapper .card_main_wrapper:last-child {
    margin-bottom: 200px !important;
}

.preset-window{
    position: absolute;
    bottom: 2.5%;
    right: -5%;
    transform: translateX(100%);
    border: 2px solid #DADBE6;
    box-shadow: 0px 0px 32px rgba(55, 65, 81, 0.08);
    border-radius: 16px !important;
}
</style>