<template>
  <v-container fluid class="px-0 pb-0">
    <pre-loader v-if="loadStatus" @dblclick.stop="closeLoader()"/>
    <v-card style="background: black; height: 85vh; overflow: hidden; border-radius:0">
      <div id="map" style="height: 85vh; background: black"></div>
      <v-col>
        <v-app-bar-nav-icon
          id="navbar-icon"
          @click="!app_drawer ? '' : (app_drawer = !app_drawer), clearSelectors()"
          style="box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.16);"
        >
          <v-icon>
            {{ app_drawer ? "mdi-menu" : "mdi-close" }}
          </v-icon>
        </v-app-bar-nav-icon>
      </v-col>
      
      <v-col v-if="!app_drawer">
        <v-menu
          offset-x
          rounded="lg"
        >
          <template v-slot:activator="{ on, attrs}">
            <v-btn
              v-bind="attrs"
              v-on="on"
              min-width="250"
              :ripple="false"
              plain
              color="white"
              class="justify-space-between project-selector"
            >
              {{project ? getProjectName(project) : 'Project'}}
              <v-icon color="blue">mdi-chevron-right</v-icon>
            </v-btn>
          </template>
          <v-list dense max-height="500" style="background: height" class="v-scrolling-div" v-if="ProjectsList.length > 0">
            <v-list-item-group v-model="project" @change="fnGetSurveyList(project)">
              <v-list-item v-for="(i, id) in ProjectsList" :key="id" :value="i.ProjectID">
                <v-list-item-icon>
                  <v-icon color="blue">
                    {{project == i.ProjectID ? 'mdi-radiobox-marked' : 'mdi-radiobox-blank'}}
                  </v-icon>
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title v-text="i.ProjectName"></v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-item-group>
          </v-list>
          <v-list v-else>
            <v-list-item>
              <v-list-item-content>
                No surveys found
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-col>

      <v-col v-if="!app_drawer">
        <v-menu
          offset-x
          rounded="lg"
          :disabled="!project"
        >
          <template v-slot:activator="{ on, attrs}">
            <v-btn
              v-bind="attrs"
              v-on="on"
              min-width="250"
              :ripple="false"
              plain
              color="white"
              class="justify-space-between survey-selector"
            >
              {{survey ? getSurveyName(survey) : 'Survey'}}
              <v-icon color="blue">mdi-chevron-right</v-icon>
            </v-btn>
          </template>
          <v-list dense max-height="500" class="v-scrolling-div" v-if="SurveyList.length > 0">
            <v-list-item-group v-model="survey">
              <v-list-item v-for="(i, id) in SurveyList" :key="id" :value="i.SurveyID">
                <v-list-item-icon>
                  <v-icon color="blue">
                    {{survey == i.SurveyID ? 'mdi-radiobox-marked' : 'mdi-radiobox-blank'}}
                  </v-icon>
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title v-text="i.SurveyName"></v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-item-group>
          </v-list>
          <v-list v-else>
            <v-list-item>
              <v-list-item-content>
                No surveys found
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-col>


      <v-col cols="auto" v-if="!app_drawer">
        <v-btn
          color="blue"
          class="white--text"
          @click="loadEvent"
          :disabled="!survey"
        >
          Load Map
        </v-btn>
      </v-col>

      <v-btn class="share-btn" @click="toggleShareDrawer" color="white" v-show="!GuestMode">
        <span>Share Versions <v-icon color="blue"> mdi-share-variant </v-icon></span> 
      </v-btn>

      <v-row>
        <left-side-bar />
      </v-row>

      <v-row>
        <right-side-bar/>
      </v-row>
    </v-card>
  </v-container>
</template>

<script>
import LeftSideBar from "../ReportComponents/MapViewReport/LeftSideBar.vue";
import RightSideBar from "../ReportComponents/MapViewReport/RightSideBar.vue";
import PreLoader from "@/components/pre-loader.vue";
import { createNamespacedHelpers } from "vuex";
const { mapGetters, mapActions } = createNamespacedHelpers("MapReport");

export default {
  components: {
    LeftSideBar,
    RightSideBar,
    PreLoader,
  },
  data() {
    return {
      project: "",
      survey: "",
      app_drawer: false,
      markers: [],
      WsId: "",
    };
  },
  async mounted() {
    this.fnSetLoading(true);
    await this.fnLoadMap(); 
    await this.getCurrentUser();
    this.WsId = this.$route.query.WsID;
    this.WsId && this.$store.commit('MapReport/SET_WSID', this.WsId);
    if(this.$route.query.VersionId){
      this.$store.commit('MapReport/SET_VERSIONUNIQUEID', this.$route.query.VersionId)
    }
    await this.fnGetProjectList();
    if(this.GuestMode){
      let VersionData = await this.fnGetVersionData(this.CurrentVersionId)
      this.project = VersionData.ProjectID;
      this.fnGetSurveyList(this.project);
      this.survey = VersionData.SurveyID;
      if(this.project && this.survey){
        this.loadEvent();
      } else {
        this.fnSetLoading(false);
      }
    }
    !this.GuestMode && this.fnSetLoading(false);
    this.$bus.$on("closedrawer", (e) => {
      this.app_drawer = false;
      this.clearSelectors();
      this.fnSetStep(1);
    });

    setTimeout(()=> {
      this.fnSetLoading(false)
    }, 30000)
  },
  computed: {
    ...mapGetters([
      "GmapCenter",
      "GmapOptions",
      "StatesList",
      "StatesCount",
      "ProjectsList",
      "SurveyList",
      "Map",
      "MarkerData",
      "Step",
      "GuestMode",
      "VersionList",
      "loadStatus",
      "VersionData",
      "CurrentVersionId"
    ]),
    isMobile() {
      return this.$vuetify.breakpoint.smAndDown;
    },
  },
  methods: {
    ...mapActions([
      "fnSetLoading",
      "getCurrentUser",
      "fnGetProjectList",
      "fnGetSurveyList",
      "fnGetStateWiseSurveyResponse",
      "fnSetStep",
      "fnSetSelectedFilters",
      "fnSetMap",
      "fnLoadMap",
      "fnAddGeoBounds",
      "fnGetCenterPoints",
      "fnAddGeoMarkers",
      "fnRemoveMapFeatures",
      "fnGetGeoJson",
      'fnFilterStateGeoJson',
      "fnGetVersionData"
    ]),
    // to move it to vuex or bus ???
    async loadEvent() {
      this.fnSetLoading(true);
      this.fnSetSelectedFilters({
        project: this.project,
        survey: this.survey,
      });
      await this.fnGetStateWiseSurveyResponse()
      .then(() => {
        this.fnSetStep(1);
      })
      this.app_drawer = true;
      this.$bus.$emit("toggleAppDrawer");
      // this.fnSetStep(this.Step + 1); // need checking
      // await this.fnAddGeoBounds();
      // this.fnGetCenterPoints();
      this.fnFilterStateGeoJson()
      let markers = []
      this.MarkerData.forEach(x => {
        this.StatesList.forEach(y => {
          if(y.StateName.trim().toLowerCase() == x.Name.toLowerCase()){
            markers.push(x)
          }
        })
      })
      this.fnAddGeoMarkers(markers);
      let mBounds = new google.maps.LatLngBounds();
      this.MarkerData.forEach((i) => {
        mBounds.extend(i.coords);
      });
      this.Map.setOptions({ maxZoom: 7 })
      this.Map.fitBounds(mBounds);
      this.Map.setOptions({ maxZoom: undefined})
      this.Map.panBy(-200, 0)
      this.fnSetLoading(false);
    },
    toggleShareDrawer() {
      this.$bus.$emit("toggleShareDrawer");
      // if Configuration already exists load Configuration view Panel instead of shareDrawer Panel
      // this is if after the first time user has saved a configuration to the server in a session or
      // or if the user has saved configurations based on the UserId, WsID and other parameters?
    },
    clearSelectors() {
      this.project = "";
      this.survey = "";
      this.fnRemoveMapFeatures();
    },
    getProjectName(i){
      let data = this.ProjectsList.find(x => x.ProjectID == i);
      return data.ProjectName;
    },
    getSurveyName(i){
      let data = this.SurveyList.find(x => x.SurveyID == i);
      return data.SurveyName;
    },
    closeLoader(){
      console.log('close loader')
    }
  },
  watch: {
    Step: function (val) {
      switch (val) {
        case 0:
          this.app_drawer = false;
          break;
        default:
          break;
      }
    },
  },
};
</script>

<style scoped>
#map {
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  z-index: 0;
}
.main_wrapper {
  position: absolute;
  top: 0;
  left: 0;
  padding-left: 15px !important;
}
#navbar-icon {
  background: white;
  border: 1px solid #F1F3F4;
  box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.16);
  border-radius: 8px;
}
.share-btn {
  margin: 20px;
  position: absolute;
  top: 0px;
  right: 0px;
  border-radius: 8px;
  border: 1px solid #F1F3F4;
}
.project-selector{
  box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.16);
  border: 1px solid #F1F3F4;
  border-radius: 8px;
  padding: 12px 16px;
}
.survey-selector{
  box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.16);
  border: 1px solid #F1F3F4;
  border-radius: 8px;
  padding: 12px 16px;

}
</style>
