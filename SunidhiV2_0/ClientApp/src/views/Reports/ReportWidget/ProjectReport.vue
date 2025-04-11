<template>
    <v-container fluid>
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12">
                <v-card style="height: 780px" elevation="1">
                    <v-row justify="center">
                        <v-col >
                            <!--<p style="margin-top:250px">
                            <img src="..\..\..\..\src\assets\EmptyPlaceholders\Graph.svg" height="150" width="150" />
                        </p>
                        <p class="heading-4-dark font-weight-medium pt-0">No data available</p>-->
                                    <v-row justify="center">
                                        <v-col sm="11" cols="12" class="pt-1 pl-0 mb-3 mt-2">
                                            <h1 style="color: #2D2F40; font-size: 20px; font-weight: bold">
                                                Geographic Project Overview
                                            </h1>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-row class="pt-0 pl-12" justify="space-evenly">
                                            <v-col class="pt-0" sm="4" cols="12">
                                                <v-select dense label="Search Projects" outlined hide-details></v-select>
                                            </v-col>
                                            <v-col class="pt-0" sm="2" cols="12">
                                                <v-select dense label="Filter 1" outlined hide-details></v-select>
                                            </v-col>
                                            <v-col class="pt-0" sm="2" cols="12">
                                                <v-select dense
                                                          label="Filter 2"
                                                          outlined
                                                          hide-details
                                                          item-text="name">
                                                </v-select>
                                            </v-col>
                                            <v-col class="pt-0" sm="2" cols="12">
                                                <v-select dense
                                                          label="Filter 3"
                                                          outlined
                                                          hide-details
                                                          item-text="name">
                                                </v-select>
                                            </v-col>
                                        </v-row>
                                    </v-row>
                                    <v-row  justify="center">
                                        <v-col sm="11" cols="12" class="mr-3">
                                            <v-row >
                                                <v-col sm="4" cols="12" class="pt-1   mb-3 mt-2 px-0" >
                                                    <v-card class="v-scrolling-div"
                            style="overflow-x: hidden; height: 530px">
                                                    <template>
                                                        <v-row justify="center" >
                                                            <v-expansion-panels accordion>
                                                                <v-expansion-panel  @click="onExpansionPanelClick(i)" v-for="(item,i) in 5"
                                                                                   :key="i" class="pb-0 mb-4" >
                                                                    <v-expansion-panel-header hide-actions expand-icon="mdi-arrow-expand-all" class="v-expansion-panel-header"  style="border-bottom: 1px solid rgba(0, 0, 0, 0.12)">
                                                                        <v-row class="pt-1">
                                                                            <v-row >
                                                                                <v-col sm="2" cols="12" class="pt-4 pb-0 pr-0" style="text-align:end;">
                                                                                    <v-btn fab dark small color="white" class="elevation-2">
                                                                                        <v-icon color="#2196F3">mdi-card</v-icon>
                                                                                    </v-btn>

                                                                                </v-col>
                                                                                <v-col sm="10" cols="12" class="pt-0 pb-0">
                                                                                    <v-row>
                                                                                        <v-col sm="8" class="pb-0">
                                                                                            <p style="color: #484848; font-size: 16px; ">Mahila Kisan Sashatika...</p>
                                                                                            <p style="color: #707070;font-size:13px;">Span : 10-08-20 to 20-08-21</p>
                                                                                        </v-col>
                                                                                        <v-col sm="4" class="pt-5 pb-0" style="text-align:center;">
                                                                                            <v-btn color="#EEEEEE" tile fab dark small  class="elevation-2">
                                                                                                <v-icon color="#606060">mdi-arrow-expand-all</v-icon>
                                                                                            </v-btn>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    

                                                                                </v-col>
                                                                                <v-col sm="12" class="px-0" v-if="buttonIndex != i"><v-divider></v-divider></v-col>
                                                                                
                                                                            </v-row>
                                                                            <v-col sm="7" class="py-0" v-if="buttonIndex != i">
                                                                                <v-row>
                                                                                    <v-col class="pl-4">
                                                                                        <span class="mr-4" style="color: #707070;font-size:15px">
                                                                                            <v-icon size="12" color="#707070">mdi-account</v-icon> Team
                                                                                        </span>
                                                                                        <v-avatar size="27" class="team-avatar" v-for="n in 3" :key="n">
                                                                                            <v-img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRIYqm5RNH-EKtCos6WnvVcsQPtMONXa0aLQ&usqp=CAU" alt="User DP" height="27" width="27" class="ma-4"></v-img>

                                                                                        </v-avatar>
                                                                                        <p class="mt-3" style="color: #707070;font-size:15px"> <v-icon size="12" color="#707070">mdi-credit-card</v-icon> Budget: ₹2100 </p>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col sm="5" class="py-0 " style="text-align:center;" v-if="buttonIndex != i">
                                                                                <v-progress-circular :rotate="360"
                                                                                                     :size="80"
                                                                                                     :width="10"
                                                                                                     :value=70
                                                                                                     color="#20C997" class="mb-4">
                                                                                    <span style="color:black;font-size:15px;font-weight:600;">70%</span>
                                                                                </v-progress-circular>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-expansion-panel-header>
                                                                    <v-expansion-panel-content class="v-expansion-panel-content__wrap">
                                                                        <v-row justify-md="center">
            <v-col sm="12">
              <v-tabs v-model="MapTab" grow class="pt-0">
                <v-tabs-slider color="teal lighten-3"></v-tabs-slider>
                <v-tab href="#tabs-4"> Details </v-tab>
                <v-tab href="#tabs-5"> Insights </v-tab>
                <v-tab href="#tabs-6"> Activity </v-tab>
                <v-tab-item value="tabs-4">
                    <v-row justify="center"  class="pt-1" style="overflow-x:hidden">
                        <v-col md="11" class="pb-0 v-scrolling-div" style="height:60vh">
                            <v-row no-gutters>
                                <v-col md="12">
                                    <!-- Project Name -->
                                    <h4 class="font-weight-regular heading-3-dark">Project Name</h4>
                                    <h4 class="font-weight-regular heading-3"> Project Dummy </h4>
                                    <!-- Project Description -->
                                    <h4 class="font-weight-regular heading-3-dark mt-4">Project Description</h4>
                                    <h4 class="font-weight-regular heading-3">
                                         project Info 
                                    </h4>
                                    <!-- Initiative Name-->
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Initiative
                                    </h4>
                                    <h4 class="font-weight-regular heading-3">
                                         InitiativeName 
                                    </h4>
                                </v-col>
                            </v-row>

                            <!-- Project Code & Budget -->
                            <v-row no-gutters>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Code
                                    </h4>
                                    <h4 class="font-weight-regular heading-3"> projectInfo </h4>
                                </v-col>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Budget
                                    </h4>
                                    <h4 class="font-weight-regular heading-3"> Pro Budget </h4>
                                </v-col>
                            </v-row>

                            <v-row no-gutters>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Start Date
                                    </h4>
                                    <h4 class="font-weight-regular heading-3">12-12-12 </h4>
                                </v-col>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        End Date
                                    </h4>
                                    <h4 class="font-weight-regular heading-3"> 12-12-12 </h4>
                                </v-col>
                            </v-row>

                            <v-row no-gutters>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Country
                                    </h4>
                                    <h4 class="font-weight-regular heading-3">+91 </h4>
                                </v-col>
                                <v-col md="6" class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        State
                                    </h4>
                                    <h4 class="font-weight-regular heading-3">431002</h4>
                                </v-col>
                            </v-row>

                            <v-row no-gutters>
                                <v-col class="pt-0">
                                    <h4 class="font-weight-regular heading-3-dark mt-4">
                                        District
                                    </h4>
                                    <h4 class="font-weight-regular heading-3"> 456711 </h4>
                                </v-col>
                            </v-row>                          
                        </v-col>
                    </v-row>
                </v-tab-item>
                <v-tab-item value="tabs-5">
                    <v-card elevation="0"
                            class="v-scrolling-div"
                            style="overflow-x: hidden; height: 400px">
                        <v-container>
                            <v-row>
                                <v-col class="pl-9" sm="3" cols="12">
                                    <v-avatar>
                                        <img :src="insights" alt="John" />
                                    </v-avatar>
                                </v-col>
                                <v-col class="pl-1 pt-6" sm="4" cols="12">
                                    <span style="font-size: 18px; font-weight: bold">INSIGHTS</span>
                                </v-col>
                                <v-col sm="5" cols="12" class="pt-5">
                                    <button type="button"
                                            class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                            id="AutoTest_V20"
                                            style="
                              background-color: rgb(255, 255, 255);
                              border-color: rgb(255, 255, 255);
                            ">
                                        <span class="v-btn__content">
                                            <i aria-hidden="true"
                                               class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
                                               style="font-size: 25px"></i>
                                        </span>
                                    </button>
                                    <span>1 of 6</span>
                                    <button type="button"
                                            class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                            id="AutoTest_V21"
                                            style="
                              background-color: rgb(255, 255, 255);
                              border-color: rgb(255, 255, 255);
                            ">
                                        <span class="v-btn__content">
                                            <i aria-hidden="true"
                                               class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
                                               style="font-size: 25px"></i>
                                        </span>
                                    </button>
                                </v-col>
                            </v-row>
                            <v-row>
                                <template v-for="(item, index) in insigh">
                                    <v-col sm="11" class="d-flex pl-8" :key="index">
                                        <v-avatar v-if="item.header" class="mt-4">
                                            <img :src="impact" alt="John" />
                                        </v-avatar>
                                        <v-list three-line>
                                            <v-list-item v-if="item.title" :key="item.title">
                                                <v-list-item-content class="pt-0">
                                                    <v-row>
                                                        <v-col sm="4" cols="12">
                                                            <v-avatar tile>
                                                                <v-img :src="item.avatar"></v-img>
                                                            </v-avatar>
                                                        </v-col>
                                                        <v-col sm="8" class="pl-0" cols="12">
                                                            <v-list-item-subtitle v-html="item.title"></v-list-item-subtitle>
                                                        </v-col>
                                                    </v-row>
                                                    <v-row>
                                                        <v-col sm="12" class="pt-0" cols="12">
                                                            <span style="color: rgba(0, 0, 0, 0.6)"
                                                                  v-html="item.subtitle"></span>
                                                            <!-- <v-list-item-subtitle
                                              v-html="item.subtitle"
                                            ></v-list-item-subtitle> -->
                                                        </v-col>
                                                    </v-row>
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list>
                                    </v-col>
                                </template>
                            </v-row>
                            <v-row>
                                <v-col class="pl-12" sm="8" cols="12">
                                    <span style="color: #afafaf">Did you find this helpful ?</span>
                                </v-col>
                                <v-col sm="4" class="pt-1" cols="12">
                                    <v-btn icon>
                                        <v-icon dark
                                                style="font-size: 18px; color: #cfcfcf">
                                            mdi-thumb-up
                                        </v-icon>
                                    </v-btn>
                                    <v-btn icon>
                                        <v-icon dark
                                                style="font-size: 18px; color: #cfcfcf">
                                            mdi-thumb-down
                                        </v-icon>
                                    </v-btn>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col sm="12" cols="12">
                                    <v-select class="ml-8 mr-3"
                                              v-model="select"
                                              dense
                                              :items="data_audit_list"
                                              label="Standard"
                                              outlined
                                              hide-details
                                              item-text="name">
                                        <template v-slot:selection="{ item, index }">
                                            <img :src="item.image" />
                                            <span class="ml-3" style="font-size:14px;">
                                                {{
                                item.name
                                                }}
                                            </span>
                                        </template>
                                        <template v-slot:item="prop">
                                            <img :src="prop.item.image" />
                                            <span class="ml-3"
                                                  style="font-size:14px;">{{ prop.item.name }}</span>
                                        </template>
                                    </v-select>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card>
                </v-tab-item>
                <v-tab-item value="tabs-6">
                    <v-card elevation="0"
                            class="v-scrolling-div"
                            style="overflow-x: hidden; height: 400px">
                        <v-row justify="center">
                            <v-col cols="12" sm="11">
                                <v-row>
                                    <v-col sm="12" cols="12">
                                        <v-list three-line>
                                            <template v-for="(item, index) in items">
                                                <v-subheader v-if="item.header"
                                                             :key="item.header"></v-subheader>

                                                <v-divider v-else-if="item.divider"
                                                           :key="index"
                                                           :inset="item.inset"></v-divider>

                                                <v-list-item v-else :key="item.title">
                                                    <v-list-item-avatar class="pt-3" size="45">
                                                        <v-img 
                                                               :src="item.avatar"></v-img>
                                                    </v-list-item-avatar>

                                                    <v-list-item-content>
                                                        <v-list-item-title style="
                                        font-weight: bold;
                                        font-size: 13px;
                                        color: black;
                                      "
                                                                           v-html="item.title"></v-list-item-title>
                                                        <v-list-item-subtitle style="
                                        font-weight: bold;
                                        font-size: 13px;
                                        color: black;
                                      "
                                                                              v-html="item.subtitle"></v-list-item-subtitle>
                                                    </v-list-item-content>
                                                    <v-list-item-action>
                                                        <span style="font-size: 13px">7/21/20 at 3.51 pm</span>
                                                    </v-list-item-action>
                                                </v-list-item>
                                            </template>
                                        </v-list>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-tab-item>
              </v-tabs>
            </v-col>
          </v-row>
                                                                    </v-expansion-panel-content>
                                                                </v-expansion-panel>
                                                            </v-expansion-panels>
                                                        </v-row>
                                                    </template>
                                                        </v-card>
                                                </v-col>
                                                <v-col sm="6" cols="12" class="pt-1  mb-3 mt-2 ">

                                                    <template>
                                                           <img style="height:530px;" src="https://www.thestatesman.com/wp-content/uploads/2020/04/googl_ED.jpg"/>
                                                   
                                                    </template>
                                                </v-col>
                                        </v-row>
                                                </v-col>
                                        
</v-row>
                                
                        </v-col>
                    </v-row>
                    </v-card>
                </v-col>
            </v-row>
        </v-container>
</template>
<script>
    export default {
        data() {
            return {
                select: '',
                ActivityTab: 'tab-2',
                dividerIndex: null,
                buttonIndex: null,
                MapTab: 'tabs-4',
                printer: require("@/assets/Survey/Print.svg"),
                excel: require("@/assets/Survey/Export_Excel.svg"),
                impact: require("@/assets/dashboard_asset/Impact.svg"),
                insights: require("@/assets/dashboard_asset/Insights.svg"),
                items: [
                    {
                        avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                        title: "Varun Mittal",
                        subtitle: "Updated Task",
                    },
                    {
                        avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                        title: "Varun Mittal",
                        subtitle: "Updated Task",
                    },
                    {
                        avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                        title: "Varun Mittal",
                        subtitle: "Updated Task",
                    },
                ],
                data_survey_list: [
                    {
                        name: "Monthly plantation supplies & utillities cost",
                        image: require("@/assets/Survey/Question_Icon.svg"),
                    },
                    {
                        name: "Planned",
                        image: require("@/assets/Survey/Question_Icon.svg"),
                    },
                    {
                        name: "Planned v/s ",
                        image: require("@/assets/Survey/Question_Icon.svg"),
                    },
                ],
                data_audit_list: [
                    {
                        name: "Your perfectly balanced",
                        image: require("@/assets/Inputs/Data_Audit.svg"),
                    },
                    { name: "Planned", image: require("@/assets/Inputs/Data_Audit.svg") },
                    {
                        name: "Planned v/s ",
                        image: require("@/assets/Inputs/Data_Audit.svg"),
                    },
                ],
                insigh: [
                    {
                        header: "Impact Indicators",
                        avatar: require("@/assets/SDG/E_SDG_PRINT-01.jpg"),
                        title: "SDG 4 Target 4.2",
                        subtitle: `Given the data till date , a <span style="color:#52B962;">positive change of 12% </span> has beem acheived`,
                    },
                    {
                        header: "Impact Indicators",
                        avatar: require("@/assets/SDG/E_SDG_PRINT-15.jpg"),
                        title: "SDG 15 Target 15.1",
                        subtitle: `Given the data till date , a <span style="color:#52B962;">positive change of 18% </span> has beem acheived`,
                    },
                    {
                        header: "Impact Indicators",
                        avatar: require("@/assets/SDG/E_SDG_PRINT-15.jpg"),
                        title: "SDG 15 Target 15.1",
                        subtitle: `Given the data till date , a <span style="color:#52B962;">positive change of 18% </span> has beem acheived`,
                    },
                    {
                        header: "Impact Indicators",
                        avatar: require("@/assets/SDG/E_SDG_PRINT-15.jpg"),
                        title: "SDG 15 Target 15.1",
                        subtitle: `Given the data till date , a <span style="color:#52B962;">positive change of 18% </span> has beem acheived`,
                    },
                ],
            };
        },
        mounted: async function () { },
        methods: {
            onExpansionPanelClick(index) {
                if (index == this.buttonIndex) {
                    this.buttonIndex = null;
                    console.log(this.buttonIndex)
                } else {
                    console.log(index)
                    console.log("hey")
                    this.buttonIndex = index;
                    console.log(this.buttonIndex)
                }
            },
            onButtonClick(index) {
                if (index == this.buttonIndex) {
                    this.buttonIndex = null;
                    console.log(this.buttonIndex)
                } else {
                    console.log(index)
                    console.log("hey1")
                    this.buttonIndex = index;
                    console.log(this.buttonIndex)
                }
            },
        }
    };
</script>
<style >
    .v-expansion-panel-header {
        padding-bottom: 0 !important;
        padding-top: 0 !important;
    }
    .v-expansion-panel-content__wrap {
        padding: 0 !important;
    }
</style>