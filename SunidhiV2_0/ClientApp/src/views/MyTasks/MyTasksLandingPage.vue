<template>
    <v-container fluid>
        <v-row no-gutters justify="center">
            <v-col cols="12" md="12" class="pa-0 ma-0">
                <v-card class="myTasksScreenHeight pa-0 ma-0">
                    <v-card-title :class="[checkStepper==2 ? 'd-none' : '']">
                        <h5 class="text-left page-head mb-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-file-document-box-check-outline</v-icon>
                            </v-avatar>
                            MyTasks
                        </h5>
                        <v-spacer class="hidden-sm-and-down"></v-spacer>
                        <div class="hidden-md-and-up"></div>
                        <div class="d-flex d-inline">
                        <!--Filter Chips-->
                        <v-sheet  class="mx-auto hidden-sm-and-down" max-width="500" >
                            <v-slide-group class="justify-content: center" multiple show-arrows >
                                <v-row justify="end" align="center" class="my-1 ml-2 mr-2">
                                    <v-slide-item  v-for="items in projectFilter" :key="items" >
                                        <v-chip class=" ml-1 py-0" 
                                            color="#3D7EFE"
                                            text-color="white"
                                            close
                                            @click:close="projectFilter.splice(projectFilter.indexOf(items), 1)">{{items}}
                                        </v-chip>
                                    </v-slide-item>
                                    <v-slide-item  v-for="items in  benFilter" :key="items">
                                        <v-chip   class="ml-1 py-0" 
                                            color="#3D7EFE"
                                            text-color="white"
                                            close
                                            @click:close="benFilter.splice(benFilter.indexOf(items), 1)">{{items}}
                                        </v-chip>    
                                    </v-slide-item>
                                    <v-slide-item  v-for="items in reccuringFilter" :key="items">
                                        <v-chip   class="ml-1  py-0" 
                                            color="#3D7EFE"
                                            text-color="white"
                                            close  @click:close="reccuringFilter.splice(reccuringFilter.indexOf(items), 1)">{{items}}
                                        </v-chip>
                                    </v-slide-item>
                                </v-row>
                            </v-slide-group>
                        </v-sheet>
                        <v-menu class="pa-0 mb-0"  
                            v-if="benFilter.length > 0 || reccuringFilter.length > 0 || projectFilter.length > 0"
                            left
                            :close-on-content-click="false"
                            offset-y="" 
                            transition="scroll-y-transition"
                        >
                            <template  v-slot:activator="{ on }">
                                <v-chip 
                                    v-on="on"
                                    class="px-3 my-1 mx-3"
                                    color="#3D7EFE"
                                    text-color="white"
                                >
                                See all
                                </v-chip>
                            </template>
                            <v-list rounded nav dense style="background: #ebf1f8">
                                <v-subheader >
                                    <h3 class="menuTitle">Filters Applied</h3>
                                </v-subheader>
                               <v-list-item  
                                    style="background-color: #3d7efe"
                                    v-for="items in projectFilter"
                                    :key="items">
                                    <v-list-item-content class="px-1"
                                    style="color: white; font-family: Poppins">
                                    {{ items }}
                                    </v-list-item-content>
                                    <v-list-item-icon>
                                    <v-icon
                                        small
                                        style="color: white"
                                        @click="
                                        projectFilter.splice(projectFilter.indexOf(items), 1)
                                        "
                                        >mdi-close-circle</v-icon
                                    >
                                    </v-list-item-icon>
                                </v-list-item>
                                
                                <v-divider
                                    v-if="benFilter.length > 0"
                                    class="pa-1 ma-2"
                                ></v-divider>
                                <v-list-item  
                                    style="background-color: #3d7efe"
                                    v-for="items in benFilter"
                                    :key="items">
                                    <v-list-item-content class="px-1"
                                    style="color: white; font-family: Poppins"
                                    >
                                    {{ items }}
                                    </v-list-item-content>
                                    <v-list-item-icon>
                                    <v-icon
                                        small
                                        style="color: white"
                                        @click="benFilter.splice(benFilter.indexOf(items), 1)"
                                        >mdi-close-circle</v-icon
                                    >
                                    </v-list-item-icon>
                                </v-list-item>
                                <v-divider
                                    v-if="reccuringFilter.length > 0"
                                    class="pa-1 ma-2"
                                ></v-divider>


                                <v-list-item 
                                    style="background-color: #3d7efe"
                                    v-for="items in reccuringFilter"
                                    :key="items"
                                >
                                    <v-list-item-content class="px-1"
                                    style="color: white; font-family: Poppins"
                                    >
                                    {{ items }}
                                    </v-list-item-content>
                                    <v-list-item-icon>
                                    <v-icon
                                        small
                                        style="color: white"
                                        @click="
                                        reccuringFilter.splice(
                                            reccuringFilter.indexOf(items),
                                            1
                                        )
                                        "
                                        >mdi-close-circle</v-icon
                                    >
                                    </v-list-item-icon>
                                </v-list-item>
                            </v-list>
                        </v-menu>
                        <!--Search-->
                        <v-text-field
                            name="txtSearchSurveyName"
                            label="Search"
                            class="mr-4"
                            style="max-width: 250px"
                            v-model="vmSearchVal"
                            dense
                            clearable
                            prepend-inner-icon="mdi-magnify"
                            @change="searchtaskval = vmSearchVal"
                            outlined
                            hide-details
                            clear-icon="mdi-close"
                            @click:clear="searchtaskval = ''"
                        >
                        </v-text-field>
                        <!--Filter button-->
                            <v-menu :close-on-content-click="false" offset-y="" transition="scroll-y-transition" v-model="filterMenu">
                                <template v-slot:activator="{ on }">
                                    <v-btn name="btnFilter" v-on="on" height="39" width="90" outlined="" dense="" class="outlined-btn-dark hidden-sm-and-down d-md-inline mr-4"  >
                                        <v-icon>mdi-filter-outline</v-icon>
                                        <span>Filters</span>
                                    </v-btn>
                                    <!--mobile filter btn-->
                                    <v-btn v-on="on" height="39" outlined="" dense="" class="outlined-btn-dark hidden-md-and-up mr-2"  >
                                        <v-icon>mdi-filter-outline</v-icon>
                                    </v-btn>
                                </template>
                                <v-card  width="500"   class="pa-2 ma=1">
                 
                              <v-row class="justify-space-between">
                                <v-col cols="6" class="pa-4 ma-2">
                                  <div
                                    class="font-weight-bold pa-0 mb-0"
                                    style="font-size: 20px; text-decoration: none"
                                  >
                                    Filters
                                  </div></v-col
                                >
                                <v-col cols="2" class="ma-2">
                                  <v-btn icon
                                    ><v-icon @click="filterMenu = false"
                                      >mdi-close</v-icon
                                    ></v-btn
                                  >
                                </v-col>
                              </v-row>
                              <v-row align="center" no-gutters>
                                <v-card-text>
                                   <v-col cols="12" class="pa-0 ma-0">
                                    <div class="font-weight-bold pa-0 mb-2">Project</div>
                                  </v-col>
                                  <v-col cols="12" class="pa-0 ma-0 d-inline-block">
                                    <v-select
                                      dense
                                      multiple
                                      v-model="projectFilter"
                                      :items="projectItems"
                                      chips
                                      prepend-inner-icon="mdi-magnify"
                                      @click="projectFilter.push(item)"
                                    >
                                      <template v-slot:selection="data">
                                        <v-chip
                                          small
                                          :key="JSON.stringify(data.item)"
                                          color="#3D7EFE"
                                          v-bind="data.attrs"
                                          :input-value="data.selected"
                                          :disabled="data.disabled"
                                        >
                                          <v-chip-content
                                            style="color: white; font-family: Poppins"
                                            >{{ data.item }}
                                            <v-icon
                                              small
                                              style="color: white"
                                              @click="data.parent.selectItem(data.item)"
                                              >mdi-close-circle</v-icon
                                            ></v-chip-content
                                          >
                                        </v-chip>
                                      </template></v-select
                                    >
                                  </v-col>
                                  <v-col cols="12" class="pa-0 ma-0">
                                    <div class="font-weight-bold pa-0 mb-2">
                                      Beneficiary
                                    </div></v-col
                                  ><v-col cols="12" class="pa-0 ma-0">
                                    <v-select
                                      dense
                                      multiple
                                      v-model="benFilter"
                                      :items="benItems"
                                      chips
                                      prepend-inner-icon="mdi-magnify"
                                      @click="benFilter.push(item)"
                                    >
                                      <template v-slot:selection="data">
                                        <v-chip
                                          small
                                          :key="JSON.stringify(data.item)"
                                          color="#3D7EFE"
                                          v-bind="data.attrs"
                                          :input-value="data.selected"
                                          :disabled="data.disabled"
                                        >
                                          <v-chip-content
                                            style="color: white; font-family: Poppins"
                                            >{{ data.item }}
                                            <v-icon
                                              small
                                              style="color: white"
                                              @click="data.parent.selectItem(data.item)"
                                              >mdi-close-circle</v-icon
                                            ></v-chip-content
                                          >
                                        </v-chip>
                                      </template></v-select
                                    >
                                  </v-col>
                                  <v-col cols="12" class="pa-0 ma-0">
                                    <div class="font-weight-bold pa-0 mb-2">
                                      Reccuring
                                    </div>
                                  </v-col>
                                  <v-col cols="12" class="pa-0 ma-0">
                                    <v-select
                                      dense
                                      multiple
                                      v-model="reccuringFilter"
                                      :items="reccuringItems"
                                      chips
                                      prepend-inner-icon="mdi-magnify"
                                      @click="reccuringFilter.push(item)"
                                    >
                                      <template v-slot:selection="data">
                                        <v-chip
                                          small
                                          :key="JSON.stringify(data.item)"
                                          color="#3D7EFE"
                                          v-bind="data.attrs"
                                          :input-value="data.selected"
                                          :disabled="data.disabled"
                                        >
                                          <v-chip-content
                                            style="color: white; font-family: Poppins"
                                            >{{ data.item }}
                                            <v-icon
                                              small
                                              style="color: white"
                                              @click="data.parent.selectItem(data.item)"
                                              >mdi-close-circle</v-icon
                                            ></v-chip-content
                                          >
                                        </v-chip>
                                      </template></v-select
                                    >

                                  </v-col></v-card-text
                                >   </v-row
                              >
                              
                            </v-card>
                            <v-container class="d-flex" style="background: #EFEFEF; justify-content:center" > 
                                <!-- <v-col cols="6" class="justify-content-end"> -->
                                  <v-card-action   class="  pa-1 mx-auto">
                                    <v-btn    @click="clearFilters" class="mr-4"
                                                            
            
                                      >Clear all</v-btn
                                    >

                                    <v-btn   @click="saveFilter"    
                                   

                                    >Save</v-btn>
                                  </v-card-action>
                                  <!-- </v-col> -->
                                
                              </v-container
                            >
          

                            </v-menu>
                            <!--Sort button-->
                            <v-menu offset-y transition="scroll-y-transition">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on" max-width="29" min-width="40" height="37" outlined class="outlined-btn-dark">
                                        <v-icon>mdi-tune-vertical</v-icon>
                                    </v-btn>
                                </template>
                                <v-list dense color="white" light="" width="auto" max-width="250" class="action-button-list">
                                    <v-list-item>
                                        <v-list-item-content>
                                            Sort by :
                                        </v-list-item-content>
                                    </v-list-item>                                  
                                  <v-list-item-group>
                                    <v-list-item v-for="items in vmSort" :key="items">
                                      <v-list-item-content @click.stop="fnsortType(items)">
                                        {{ items }}
                                      </v-list-item-content>
                                    </v-list-item>
                                  </v-list-item-group>        
                                </v-list>
                            </v-menu>
                        </div>

                    </v-card-title>
                    <v-divider :class="[checkStepper==2 ? 'd-none' : '']"></v-divider>
                    <!--MyTask screen parent body-->
                    <v-row v-if="TaskListStatus" justify="center" class="d-md-flex" no-gutters>
                        <v-col cols="12 " md="12" xs="12" class="d-md-flex justify-center px-4 d-none my-1" style="font-size: 13px;">
                            <v-col  class="text-left">Name</v-col>
                            <v-col  class="text-center">Project Name</v-col>
                            <v-col  class="text-center">Assigned Date</v-col>
                            <v-col  class="text-center">Reccuring</v-col>
                            <v-col class="text-center">Assigned By</v-col>
                            <v-col class="text-center">Beneficiary</v-col>
                            <!-- <v-col class="text-center">More</v-col> -->
                        </v-col>
                        <v-col cols="12" md="12" xs="12" class="mt-2" :class="[checkStepper==2 ? '' : 'taskCardScrollHeight v-scrolling-div']">
                        <MyTasksSurveyCard
                            :searchtask="searchtaskval"
                            :benFilterList="benFilterList"
                            :reccuringFilterList="reccuringFilterList"
                            :projectFilterList="projectFilterList"
                            :sortType="sortType"
                            @getFilters="getFilterItems($event)"
                            :key="filtercall"
                            @checkMyStepper="checkMyStepper"
                        ></MyTasksSurveyCard>
                        </v-col>
                    </v-row>
                    <v-row v-else justify="center" class="d-md-flex" no-gutters>
                        <v-col cols="12 " md="12" xs="12" justify="center" class="mt-10">
                            <p class="text-center">
                                <img src="..\..\..\src\assets\EmptyPlaceholders\task.svg" height="150" width="250" />
                            </p>
                            <p class="heading-4-dark font-weight-medium pt-0 text-center">No task available</p>
                        </v-col>
                    </v-row>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    import MyTasksSurveyCard from "./MyTasksSurveyCard.vue"
    import { dbImworks } from '../../ImworksDb'
    import { mapGetters} from 'vuex'

    export default {
        data() {
            return {
                searchtaskval: "",
                vmSearchVal: "",
                checkStepper: 1,
                benFilter: [],
                benItems: [],
                reccuringFilter: [],
                reccuringItems: [],
                projectFilter: [],
                projectItems: [],
                filterMenu: false,
                filtercall: 1,
                benFilterList: [],
                reccuringFilterList: [],
                projectFilterList: [],
                vmSort: ["Name - Ascending", "Name - Descending", "Recently Added"],
                sortType: "",
                sortMenu: false,
                selectedItem: 1,
                filterMaster: [],
                showChips:false,
                  }
        },
        components: {
            'MyTasksSurveyCard': MyTasksSurveyCard,
        },
        created() {
            this.$store.commit('SyncBinders/updateInitState', true)
        },
        async mounted(){
            let data = await dbImworks.MyTaskList.toArray();
            if(data.length == 0 && this.TaskListStatus == true){
                this.$store.commit('SyncBinders/updateTaskListStatus', false)
            }
            if(data.length > 0 && this.TaskListStatus == false) {
                this.$store.commit('SyncBinders/updateTaskListStatus', true)
            }
        },
        computed: {
            ...mapGetters('SyncBinders',[
                'TaskListStatus'
            ]),
            isMobile() {
                return this.$vuetify.breakpoint.smAndDown
            },
        },
        methods: {
            async checkMyStepper(arg1) {
                //console.log(arg1);
                this.checkStepper = arg1;
            },
            fnsortType(type) {
                this.sortType = type;
                this.sortMenu = false;
              },
            clearFilters() {
                this.benFilter = [];
                this.reccuringFilter = [];
                this.projectFilter = [];
                this.benFilterList = [];
                this.reccuringFilterList = [];
                this.projectFilterList = [];

              },

            saveFilter() {
              if(this.benFilter.length > 0 || this.reccuringFilter.length > 0 || this.projectFilter.length > 0){
                if (this.benFilter.length > 0) {
                  this.benFilterList = this.benFilter;
                }
                if (this.reccuringFilter.length > 0) {
                  this.reccuringFilterList = this.reccuringFilter;
                }
                if (this.projectFilter.length > 0) {
                  this.projectFilterList = this.projectFilter;
                }
                this.filterMenu = false;
                  this.showChips = true;
              //  this.filtercall += 1;
              }
              },

            getFilterItems(filterItems) {
                this.benItems = filterItems.benTypes;
                this.reccuringItems = filterItems.periodTypes;
                this.projectItems = filterItems.projects;
              },

   
  
        },
        destroyed() {
            this.$store.commit('SyncBinders/updateInitState', false)
        }
    }
</script>

<style scoped>
    .menuTitle {
      font-family: "Poppins";
      font-style: normal;
      font-weight: 500;
      font-size: 20px;
      line-height: 28px;
      padding: 10px;
      margin: 10px;
      /* identical to box height, or 127% */

      text-transform: capitalize;
      color: #399cff;
    }

    .overdueCardHeight {
        height: 66vh;
    }

    /*Extra small devices (phones, 600px and down)*/
    @media only screen and (max-width: 750px) {
        .overdueCardHeight {
            height: 78vh;
        }
    }

    /*Small devices (portrait tablets and large phones, 600px and up)*/
    @media only screen and (min-width: 750px) {
        .overdueCardHeight {
            height: 62vh;
        }

    }

    /*Medium devices (landscape tablets, 768px and up)*/
    @media only screen and (min-width: 768px) {
        .overdueCardHeight {
            height: 80vh;
        }
    }

    /*Large devices (laptops/desktops, 992px and up)*/
    @media only screen and (min-width: 992px) {
        .overdueCardHeight {
            height: 64vh;
        }
    }

    /*Extra large devices (large laptops and desktops, 1200px and up)*/
    @media only screen and (min-width: 1400px) {
        .overdueCardHeight {
            height: 70vh;
        }
    }
</style>
