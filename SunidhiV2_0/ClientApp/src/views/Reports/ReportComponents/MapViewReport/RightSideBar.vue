<template>
  <v-navigation-drawer v-model="share_drawer" app temporary right width="480" class="pa-5 pt-10 share-drawer">
    <!-- topbar -->
    <v-card flat v-show="!editToggle">
      <v-btn icon style="float: right" @click="closeShareDrawer">
        <v-icon>
          mdi-close
        </v-icon>
      </v-btn>
      <v-col cols="auto" class="pa-0 pl-2 ma-0">
      <v-col xs="12" class="pa-0 pl-3 ma-0">
          <h2 v-if="savedVersionsList">
            Saved Versions
          </h2>
          <h2 v-else>
            {{shareConfigName ? shareConfigName : 'Share Version'}}
          </h2>
        </v-col>
        <v-col xs="12" class="pa-0 pl-3 ma-0">
          <v-card-subtitle class="pl-0" v-if="savedVersionsList">
            List of Saved Map Version
          </v-card-subtitle>
          <v-card-subtitle class="pl-0" v-else>
            Show or hide preset and other information
          </v-card-subtitle>
        </v-col>
      </v-col>
    </v-card>
    <div class="pa-0 ma-0"  v-show="!editToggle">
      <div v-if="savedVersionsList" class="pa-0 ma-0">
        <v-card flat class="pa-0 ma-0 d-flex flex-column v-scrolling-div" style="height: 70vh" >
          <v-card class="mx-1 my-2 mr-2 tile version--card" v-for="(i, id) in VersionList" :key="id" flat style="border: 3px solid rgba(218, 219, 230, 1); border-radius: 8px">
            <v-card-title class="pt-2" style="width: 80%; word-break: break-word; white-space: normal">
              {{i.VersionName}}
            </v-card-title>
            <v-card-subtitle class="d-flex flex-row justify-start subtitle_list-items pb-0">
              <li class="mr-2">{{i.Name}}</li>
              <li class="mr-2">{{formatDate(i.CreatedDate)}}</li>
            </v-card-subtitle>
            <v-card-actions>
              <v-card width="98%" class="py-3 px-5 mx-auto" color="rgba(61, 126, 254, 0.16)" flat style="border-radius: 8px">
                <v-hover v-slot="{}">
                  <v-card-subtitle class="pa-0 ma-0" style="text-overflow: ellipsis; overflow: hidden; white-space: nowrap">
                    <a :href="getShareLink(i.VersionUniqueId)" id="sharelink" target="_blank" class="blue--text">{{getShareLink(i.VersionUniqueId)}}</a>
                  </v-card-subtitle>
                </v-hover>
              </v-card>
            </v-card-actions>
            <div class="version--actions">
              <v-btn icon fab small class="mr-1 mt-1" @click="copyLink(i.VersionUniqueId)">
                <v-icon >mdi-content-copy </v-icon>
              </v-btn>
              <v-btn icon fab small class="mr-1 mt-1" @click="loadEditScreen(i)">
                <v-icon >mdi-pencil-outline</v-icon>
              </v-btn>
            </div>
          </v-card>
        </v-card>
        <v-card-actions class="mt-auto">
          <v-btn class="mx-auto text--white pa-7" depressed color="blue" @click="savedVersionsList = false">
            Create New Share link
          </v-btn>
        </v-card-actions>
      </div>
      <v-stepper class="ma-1 elevation-0 pa-0" v-model="step" v-else>
        <v-stepper-items>
          <v-stepper-content step="1">
            <v-card class="pa-0 ma-0 d-flex flex-column" style="height: 70vh">
              <div>
                <v-text-field 
                  outlined 
                  v-model="shareConfigName" 
                  label="Config Name" 
                  class="py-3" 
                  required
                  :rules="[v => !!v || 'Name is required']" 
                />
                <v-select
                  outlined
                  dense
                  label="Project"
                  :menu-props="{ closeOnContentClick: true , offsetY: true}"
                  :items="ProjectsList"
                  item-text="ProjectName"
                  item-value="ProjectID"
                  v-model="shareProject"
                  @change="fnGetSurveyList(shareProject)"
                  :rules="[val => !!val || 'Select a Project']"
                >
                  <v-icon slot="append" color="blue">mdi-chevron-down</v-icon>
                </v-select>
                <v-select
                  outlined
                  dense
                  label="Survey"
                  :menu-props="{ closeOnContentClick: true , offsetY: true}"
                  :items="SurveyList"
                  item-text="SurveyName"
                  item-value="SurveyID"
                  v-model="shareSurvey"
                  :disabled="!shareProject"
                  :rules="[val => !!val || 'Select a Survey']"
                >
                  <v-icon slot="append" color="blue">mdi-chevron-down</v-icon>
                </v-select>
              </div>
              <v-card-actions class="mt-auto">
                <v-btn class="mx-auto text--white pa-7" depressed color="blue" @click="loadPanel">
                  Configure Panel
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-stepper-content>
          <v-stepper-content step="2" class="pa-0">
              <v-btn depressed icon @click="previousStep">
                  <v-icon>mdi-arrow-left</v-icon>
              </v-btn>
              <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1">{{
                SelectedFilters.Project.ProjectName
              }}</v-chip>
              <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1">{{
                SelectedFilters.Survey.SurveyName
              }}</v-chip>
              <v-chip color="#EBF5FF" label text-color="#399CFF" class="ma-1"
                >India</v-chip
              >
              
            <search-bar outlined class="my-3 px-1" @search="updateSearchField" />
            
            <v-treeview
              :items="tree"
              return-object
              expand-icon="mdi-chevron-down"
              transition
              dense
              hoverable
              v-model="selection"
              open-on-click
            >
            <!-- The Selectable icon can't be moved to the left. Need to do workaround -->
              <!-- Workaround 1 -->
              <!-- <template v-slot:append="{item, selected}">
                <v-btn icon @click="debug(selected)">
                  <v-icon>
                    {{ selected ? 'mdi-eye-outline' : 'mdi-eye-off-outline' }}
                  </v-icon>
                </v-btn>
              </template> -->

              <template v-slot:prepend="{item}">
                <v-icon color="#399CFF" dense v-if="item.type == 'response'">
                  mdi-help-circle-outline
                </v-icon>
                <v-icon color="#399CFF" dense v-if="item.type == 'preset'">
                  mdi-circle-slice-2
                </v-icon>
                <v-icon color="#399CFF" dense v-if="(item.type == 'State' || item.type == 'District' || item.type == 'rootnode') ">
                  mdi-map-marker-outline
                </v-icon>
              </template>

              <template v-slot:label="{item}" >
                <v-list-item dense>
                  <v-list-item-content dense>
                    {{item.name}}
                  </v-list-item-content>

                  <v-list-item-action>
                      <v-btn small icon @click="toggleRecursively(item)">
                      <v-icon  class="pr-0">
                      {{item.active ? 'mdi-eye-outline' : 'mdi-eye-off-outline'}}
                    </v-icon>
                    </v-btn>
                  </v-list-item-action>

                </v-list-item>
              </template>
            </v-treeview>
            
            <v-card class="mx-1 elevation-0" >
              <v-card-actions>
                <v-btn depressed color="blue" class="text--white mx-auto pa-7" @click="fnSaveVersion">
                  Save Share Link
                </v-btn>
                <!-- <v-btn depressed color="grey" class="text--white mx-auto pa-4" @click="toggleRecursively()">
                  Clear
                </v-btn> -->
              </v-card-actions>
            </v-card>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </div>
        
    <!-- Edit Screen for Share Version Tree -->

    <div v-if="editToggle">
      <v-card elevation="0">
        <v-btn icon style="float: right" @click="closeShareDrawer">
          <v-icon>
            mdi-close
          </v-icon>
        </v-btn>
        
        <v-col cols="auto" class="pa-0 pl-2 ma-0">
          <v-col xs="12" class="pa-0 pl-3 ma-0">
            <h2>
              Edit Share Version
            </h2>
          </v-col>

          <v-col xs="12" class="pa-0 ma-0 mt-3">
            <v-text-field
              outlined
              v-model="editShareConfigName"
              label="Config Name"
              class="py-3 ma-0"
              required
              :rules="[v => !!v || 'Name is required']"
              hide-details
            />
          </v-col>

          <v-treeview
            :items="tree"
            return-object
            expand-icon="mdi-chevron-down"
            transition
            dense
            hoverable
            open-on-click
          >
            <template v-slot:prepend="{item}">
              <v-icon color="#399CFF" dense v-if="item.type == 'response'">
                mdi-help-circle-outline
              </v-icon>
              <v-icon color="#399CFF" dense v-if="item.type == 'preset'">
                mdi-circle-slice-2
              </v-icon>
              <v-icon color="#399CFF" dense v-if="(item.type == 'State' || item.type == 'District' || item.type == 'rootnode') ">
                mdi-map-marker-outline
              </v-icon>
            </template>

            <template v-slot:label="{item}" >
              <v-list-item dense>
                <v-list-item-content dense>
                  {{item.name}}
                </v-list-item-content>

                <v-list-item-action>
                    <v-btn small icon @click="toggleRecursively(item)">
                    <v-icon  class="pr-0">
                    {{item.active ? 'mdi-eye-outline' : 'mdi-eye-off-outline'}}
                  </v-icon>
                  </v-btn>
                </v-list-item-action>

              </v-list-item>
            </template>
          </v-treeview>
        </v-col>
        <v-card-actions class="mt-10">
          <v-btn depressed color="blue" class="text--white mx-auto pa-7" @click="fnSaveVersion">
            Save Share Link
          </v-btn>
        </v-card-actions>
      </v-card>
    </div>
    <v-snackbar v-model="snackToggle" light text color="primary" app>
      {{SnackStatus}}
    </v-snackbar>
    <v-snackbar v-model="StatusSnack" :timeout="1500" top light text color="primary" app>
      {{StatusMessage}}
    </v-snackbar>
  </v-navigation-drawer>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('MapReport')
import {v4 as uuidv4, v1 as _id} from 'uuid';
import { dbImworks } from '@/ImworksDb'


// components
import SearchBar from "./SearchBar.vue";
export default {
  components: {
    SearchBar,
  },
  data() {
    return {
      UserId: null,
      share_drawer: false,
      step : 1,
      shareConfigName: "",
      shareProject: "",
      shareSurvey: "",
      tempConfig: {},
      snackToggle: false,
      tree: [],
      selection: [],
      searchField: '',
      savedVersionsList: null,
      StatusSnack: false,
      StatusMessage: "",
      _SelectedFilters: {
        "Project": {
          "ProjectID": "",
          "ProjectName": ""
        },
        "Survey": {
          "SurveyID": "",
          "SurveyName": ""
        }
      },
      editToggle: true,
      editShareConfigName: "",
      CurrentVersionUniqueId: "",
    };
  },
  computed: {
    ...mapGetters([
        "ShareConfig",
        "ProjectsList", 
        "SurveyList",
        "SnackStatus",
        "StatesList",
        "DistrictList",
        "GeoDataList",
        "VersionList",
        "WsID",
        "WsID",
        "SelectedFilters",
    ]),
    tree2() {
      this.fnGetStateWiseSurveyResponse()
      let data = this.loadTree(this.StatesList)
      // if(this.searchField){
      //   let data2 = data.filter(item => item.name.toLowerCase().includes(this.searchField.toLowerCase()))
      //   console.log(data2)
      //   return data2
      // }
      
      let searchTree = (t) => {
        return t.filter(item => item.name.toLowerCase().includes(this.searchField.toLowerCase()))
      }

     if(this.searchField){
        let data2 = searchTree(data)
        return data2
     }
      return data
    }
  },
  async mounted() {
    this.editToggle = false
    let UserTable = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
    if (UserTable && UserTable.length > 0) {
        this.UserId = UserTable[0].UserId;
    }
    this.$bus.$on("toggleShareDrawer", async () => {
      await this.fnGetVersionDetails()
      .then(()=> {
        // console.log(this.VersionList)
        if(this.VersionList.length > 0){
          this.savedVersionsList = true
        } else {
          this.savedVersionsList = false
        }
      })

      this.share_drawer = !this.share_drawer;
      // this.savedVersionsList = true
    });
    
  },
  methods: {
    ...mapActions([
      "fnGetSurveyList",
      "fnSaveShareConfig",
      "fnClearShareConfig",
      "fnSaveShareConfig",
      "fnGetStateWiseSurveyResponse",
      "fnGetDistrictWiseSurveyResponse",
      "fnGetResponseGeoCoordinates",
      "fnGetVersionDetails",
      "fnGetVersionData",
      "fnSetLoading",
      "fnSetSelectedFilters",
      "fnSetLoading",
      "fnSetSelectedFilters",
      "fnGetShareVersionTree",
      "fnGetSurveyList"
    ]),
    debug(i){
      console.log(i);
    },
    formatDate(i){
      //format date to MMM d, yyyy
      const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
      let date = new Date(i)
      return `${months[date.getMonth()]} ${date.getDate()}, ${date.toLocaleString([], {hour: '2-digit', minute:'2-digit'})}`
      
    },
    getShareLink(i){
      let url
      if(i != undefined){
          url = `${window.location.origin}/app/index.html#/MapViewReport?WsID=${this.WsID}&VersionId=${i}`
        return url
      } else {
        return "#"
      }
    },
    copyLink(i){
      let url = this.getShareLink(i)
      navigator.clipboard.writeText(url)
      this.StatusSnack = true
      this.StatusMessage = "Copied to clipboard"
    },
    async loadPanel(){
      this.fnSetLoading(true)
      if(!this.shareConfigName){
        this.StatusSnack = true
        this.StatusMessage = "Please enter a name"
        return
      }
      if(!this.shareProject){
        this.StatusSnack = true
        this.StatusMessage = "Please select a project"
        return
      }
      if(!this.shareSurvey){
        this.StatusSnack = true
        this.StatusMessage = "Please select a survey"
        return
      }
      this.fnSetLoading(true)
      let ProjectName = this.ProjectsList.find(item => item.ProjectID == this.shareProject).ProjectName; 
      let SurveyName = this.SurveyList.find(item => item.SurveyID == this.shareSurvey).SurveyName;

      this.fnSetSelectedFilters({
        project: this.shareProject,
        survey: this.shareSurvey,
      });

      this._SelectedFilters = {
        "Project": {
          "ProjectID": this.shareProject,
          "ProjectName": ProjectName
        },
        "Survey": {
          "SurveyID": this.shareSurvey,
          "SurveyName": SurveyName
        }
      }
     
      await this.fnGetShareVersionTree()
      .then((data)=> {
        this.fnSetLoading(false)
        this.tree = data
        this.step++
      })
    },
    updateSearchField(val){
      this.searchField = val
    },
    toggleRecursively(item) {
      item.active = !item.active
      if (item.children) {
        item.children.forEach(child => {
          this.toggleRecursively(child)
        })
      }
    },
    async fnSaveVersion(){
      // console.log(this.CurrentVersionUniqueId)
      this.fnSetLoading(true)
      let configData = []
      const nestIterator = (ob) => {
        ob.forEach(item => {
          if(item.children){
            item.children.forEach(i=> {
              i.parent = item
            })
            let presetObj = item.children.find(i => i.type == "preset")
            if(item.type.toLowerCase() == 'state'){
              let data = {
                StateName: item.name,
                DistrictName: "",
                Preset: presetObj.active,
                ResponseCoordinates: "",
                Type: item.type.toLowerCase()
              }
              if(!presetObj.active){
                configData.push(data)
              }
            }

            if(item.type.toLowerCase() == 'district'){
              let data = {
                StateName: item.parent.name,
                DistrictName: item.name,
                Preset: presetObj.active,
                ResponseCoordinates: "",
                Type: item.type.toLowerCase()
              }
              if(!presetObj.active){
                configData.push(data)
              } 
            }

            if(item.type.toLowerCase() == 'responses'){
              const getCoordinates = (a) => {
                const data = []
                a.forEach(i => {
                  if(!i.active){
                    data.push({
                      Coordinates: i.name
                    })
                  }
                })
                return data.length ? JSON.stringify(data) : ""
              }

              let data = {
                StateName: item.parent.parent.name,
                DistrictName: item.parent.name,
                Preset: false,
                Type: item.type.toLowerCase(),
                ResponseCoordinates: getCoordinates(item.children),
              }
              if(data.ResponseCoordinates){
                let flag = false
                let d = JSON.parse(data.ResponseCoordinates)
                d.forEach(c => {
                  if(!c.active){
                    flag = true
                  }
                })

                if(flag){
                  configData.push(data)
                }
              }
            }

            nestIterator(item.children)
          }
        })
      }

      nestIterator(this.tree)

      let VersionUniqueID = uuidv4();
    
      let payload = {
        VersionUniqueID: this.CurrentVersionUniqueId ? this.CurrentVersionUniqueId : VersionUniqueID,
        ConfigName: this.editShareConfigName ? this.editShareConfigName : this.shareConfigName,
        ConfigData: JSON.stringify(configData),
        WorkspaceID: this.WsID,
        loggedUserId: this.UserId,
      }
      console.log(payload)
      console.log(configData)
      // this.fnSetLoading(false)
      this.fnSaveShareConfig(payload).then(()=>{
        this.fnSetLoading(false)
        this.snackToggle = true
        setTimeout(() => {
          this.snackToggle = false
          this.closeShareDrawer()
        }, 2000);
      })      
    },
    closeShareDrawer(){
      this.share_drawer = false
      this.step = 1
      this.fnClearShareConfig()
      this.clearSelectors();
      this.editToggle = false
      this.editShareConfigName  = ''
      this.CurrentVersionUniqueId = ''
    },
    previousStep(){
      this.step--
      console.log(this.step)
      // if(this.step == 1){
      //   this.clearSelectors()
      //   this.fnClearShareConfig()
      // }
    },
    clearSelectors(){
      this.shareProject = ''
      this.shareSurvey = ''
      this.shareConfigName = ''
    },
    async loadEditScreen(itm){
      this.fnSetLoading(true)
      let version = []
      this.CurrentVersionUniqueId = itm.VersionUniqueId
      let VData = await this.fnGetVersionData(itm.VersionUniqueId)

      const nestIterator = (ob) => {
        ob.forEach((x) => {
          version.forEach((y)=> {
            if(x.type.toLowerCase() == 'state' && x.name.trim().toLowerCase() == y.StateName.trim().toLowerCase()){
              let flag = 0
              x.children.forEach((a)=> {
                if(a.type == "preset"){
                  if(!y.Preset) a.active = false
                }
                if(a.type != "preset" && a.active){
                  flag++
                }
              })
              if(!flag) x.active = false
              
            }
            if(y.DistrictName && x.type.toLowerCase() == 'district' && x.name.trim().toLowerCase() == y.DistrictName.trim().toLowerCase()){
              let flag = 0
              x.children.forEach((a)=> {
                if(a.type == "preset"){
                  if(!y.Preset) a.active = false
                }
                if(a.type != "preset" && a.active){
                  flag++
                }
              })
              if(!flag) x.active = false
            }

            if(x.type == "responses"){
              let flag = false
              x.children.forEach((c)=> {
                if(c.active) flag = true
              })
              if(!flag) x.active = false
            }
            if(y.response_coordinates && x.type.toLowerCase() == 'rootnode'){
              if(x.name == y.response_coordinates){
                if(!y.Preset) x.active = false
              }
            }

            if(x.children) {
              nestIterator(x.children)
            }
          })
        })
      }

      if(VData && VData.VersionData.length > 0){

        for(let v of VData.VersionData){
          if(v.StateName && v.DistrictName && v.response_coordinates){
            version.push({
              StateName: v.StateName.trim(),
              DistrictName: v.DistrictName.trim(),
              response_coordinates: v.response_coordinates,
              Preset: v.Preset,
              type: "rootnode",
              ProjectID: v.ProjectID,
              SurveyID: v.SurveyID
            })
          }
          if(v.StateName && v.DistrictName && !v.response_coordinates){
            version.push({
              StateName: v.StateName.trim(),
              DistrictName: v.DistrictName.trim(),
              response_coordinates: null,
              Preset: v.Preset,
              type: "district",
              ProjectID: v.ProjectID,
              SurveyID: v.SurveyID
            })
          }
          if(v.StateName && !v.DistrictName){
            version.push({
              StateName: v.StateName.trim(),
              DistrictName: null,
              response_coordinates: null,
              Preset: v.Preset,
              type: "state",
              ProjectID: v.ProjectID,
              SurveyID: v.SurveyID
            })
          }
        }
      }

      this.editShareConfigName = itm.VersionName
      this.fnGetSurveyList(VData.ProjectID)
      await this.fnSetSelectedFilters({
        project: VData.ProjectID,
        survey: VData.SurveyID
      })

      await this.fnGetShareVersionTree()
      .then((data)=> {
        nestIterator(data)
        this.tree = data
        this.editToggle = true
      })
      this.fnSetLoading(false)
    }
  },

};
</script>

<style scoped>
.subtitle_list-items li::marker{
  color: #399CFF;
}
.version--card{
  position: relative
}
.version--actions{
  position: absolute;
  top: 0;
  right: 0;
}
</style>