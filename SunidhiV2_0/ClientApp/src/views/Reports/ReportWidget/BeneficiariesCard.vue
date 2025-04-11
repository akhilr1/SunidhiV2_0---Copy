<template>
  <v-container fluid>
    <v-row>
      <v-col sm="5" class="pl-7" cols="12">
       <v-select
          v-model="select"
          dense
          :items="items1"
          label="Standard"
          outlined
          hide-details
          item-text="name"
        >
          <template v-slot:selection="{ item, index }">
            <v-avatar color="red" size="25">
              <span class="white--text">C</span>
            </v-avatar>
            <span class="ml-3" style="font-size: 14px">{{ item.title }}</span>
          </template>
          <template v-slot:item="prop" class="pl-0">
            <v-avatar color="red" size="35">
              <span class="white--text">C</span>
            </v-avatar>
            <span class="ml-3" style="font-size: 14px">{{
              prop.item.title
            }}</span>
          </template>
        </v-select>
      </v-col>
      <v-col cols="6" sm="6">
        <button
          type="button"
          class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V20"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
        <span>1 of 6</span>
        <button
          type="button"
          class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V21"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm="5" class="pl-7" cols="12">
        <v-select
          v-model="select"
          dense
          :items="data_survey_list"
          label="Standard"
          outlined
          hide-details
          item-text="name"
        >
          <template v-slot:selection="{ item, index }">
            <img :src="item.image" /><span
              class="ml-3"
              style="font-size: 14px"
              >{{ item.name }}</span
            >
          </template>
          <template v-slot:item="prop">
            <img :src="prop.item.image" /><span
              class="ml-3"
              style="font-size: 14px"
              >{{ prop.item.name }}</span
            >
          </template>
        </v-select>
      </v-col>
      <v-col cols="6" sm="6">
        <button
          type="button"
          class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V20"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
        <span>1 of 6</span>
        <button
          type="button"
          class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V21"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm="5" class="pl-7" cols="12">
         <v-select
          v-model="select"
          dense
          :items="data_survey_list1"
          label="Standard"
          outlined
          hide-details
          item-text="name"
        >
          <template v-slot:selection="{ item, index }">
            <img :src="item.image" /><span
              class="ml-3"
              style="font-size: 14px"
              >{{ item.name }}</span
            >
          </template>
          <template v-slot:item="prop">
            <img :src="prop.item.image" /><span
              class="ml-3"
              style="font-size: 14px"
              >{{ prop.item.name }}</span
            >
          </template>
        </v-select>
      </v-col>
      <v-col cols="6" sm="6">
        <button
          type="button"
          class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V20"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
        <span>1 of 6</span>
        <button
          type="button"
          class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V21"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
      </v-col>
    </v-row>
    <v-row>
      <v-col class="pt-7" sm="7" cols="12">
        <v-row>
          <v-col sm="10" cols="12">
            <v-row>
              <v-col sm="7" cols="12" class="pl-8">
                <span style="font-size: 13px; font-weight: bold"
                  >Type of agricultural produce/crop cultivated</span
                >
              </v-col>
              <v-col sm="5" cols="12" class="pt-3">
                <v-row>
                  <v-col sm="12" cols="12" class="pt-0 px-0">
                    <v-chip color="black" label dark small class="ml-2">
                      Beneficiaries
                    </v-chip>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-col>
          <v-col sm="2" cols="12">
            <v-row justify="start">
              <v-col sm="5" cols="12" class="pt-2 pl-0">
                <v-btn elevation="1" tile style="border-radius:3px;" color="#AFAFAF"   icon>
                  <v-avatar size="18">
                    <img :src="excel" />
                  </v-avatar>
                </v-btn>
              </v-col>
              <v-col sm="5" cols="12" class="pt-2 pl-0">
                <v-btn elevation="1" style="border-radius:3px;" color="#EEEEEE" tile icon >
                  <v-avatar size="18">
                    <img :src="printer" />
                  </v-avatar>
                </v-btn>
              </v-col>
            </v-row>
          </v-col>
          <v-col sm="12" cols="12" class="pt-0">
            <!-- <v-row>
              <v-col sm="12" cols="12">
                <Line-Chart
                  :linedata="responsesLineData"
                  :lineOptions="responsesLineOptions"
                  style="width: 100%; height: auto"
                ></Line-Chart>
              </v-col>
              <v-col sm="12" cols="12">
                <v-row justify="center">
                  <v-col sm="6" cols="12" style="text-align: center">
                    <v-avatar
                      class="mt-1 mx-2 ml-5 ml-md-2"
                      size="15"
                      tile
                      color="#F1C100"
                    >
                    </v-avatar>
                    <span style="position: relative; top: 5px"
                      >Amount of Cardamom in hectors</span
                    >
                  </v-col>
                </v-row>
              </v-col>
            </v-row> -->
            <v-row>
              <v-col sm="12" cols="12">
                <Bar-Chart
                  :barData="responsesBarData"
                  :barOptions="responsesBarOptions"
                  style="width: 100%; height: auto"
                ></Bar-Chart>
              </v-col>
              <v-col sm="12" cols="12">
                <v-row justify="center">
                  <v-col sm="6" cols="12" style="text-align: center">
                    <v-avatar
                      class="mt-1 mx-2 ml-5 ml-md-2"
                      size="15"
                      tile
                      color="#F1C100"
                    >
                    </v-avatar>
                    <span style="position: relative; top: 5px"
                      >Cummulative</span
                    >
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
      </v-col>
      <v-col sm="4" cols="12" class="pt-11 ml-2">
        <v-card style="height: 480px">
          <v-row justify-md="center">
            <v-col sm="11">
              <v-tabs grow class="pt-0">
                <v-tabs-slider color="teal lighten-3"></v-tabs-slider>
                <v-tab href="#tabs-1"> Details </v-tab>
                <v-tab href="#tabs-2"> Activity </v-tab>
                <v-tab-item value="tabs-1">
                  <v-card
                    elevation="0"
                    class="v-scrolling-div"
                    style="overflow-x: hidden; height: 400px"
                  >
                    <v-row justify="center">
                      <v-col cols="12" sm="11">
                        <v-row>
                          <v-col cols="12" sm="12">
                            <span style="font-weight: bold; font-size: 17px"
                              >Full Name</span
                            >
                          </v-col>
                          <v-col cols="12" sm="2" class="py-0">
                            <v-avatar>
                              <img
                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                alt="John"
                              />
                            </v-avatar>
                          </v-col>
                          <v-col cols="12" sm="10" class="pb-0 pt-4">
                            <span>Sayek Lakhadwala</span>
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="12" sm="12" class="pb-0">
                            <span style="font-weight: bold; font-size: 17px"
                              >Date of Birth</span
                            >
                          </v-col>
                          <v-col cols="12" sm="12" class="pt-0">
                            <span style="color: #585858"
                              >7 November 1984</span
                            >
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="12" sm="12" class="pb-0">
                            <span style="font-weight: bold; font-size: 17px"
                              >Gender</span
                            >
                          </v-col>
                          <v-col cols="12" sm="12" class="pt-0">
                            <span style="color: #585858"
                              >Male</span
                            >
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="12" sm="12" class="pb-0">
                            <span style="font-weight: bold; font-size: 17px"
                              >Photo ID</span
                            >
                          </v-col>
                          <v-col cols="12" sm="12" class="pt-0">
                            <span style="color: #585858"
                              >Aadhar.pdf</span
                            >
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="12" sm="6">
                            <v-row>
                              <v-col class="py-0" sm="12">
                                <span style="font-weight: bold; font-size: 17px"
                                  >Created Date</span
                                >
                              </v-col>
                              <v-col sm="12" cols="12" class="pt-0 pr-0 pb-0">
                                <v-col
                                  sm="12"
                                  cols="12"
                                  style="color: #585858"
                                  class="pt-0 pr-0 pb-0 pl-0"
                                  >28/10/2020</v-col
                                >
                              </v-col>
                            </v-row>
                          </v-col>
                          <v-col sm="6" cols="12" class="pl-0">
                            <v-col class="py-0 pl-0" sm="12">
                              <span style="font-weight: bold; font-size: 17px"
                                >Last Updated</span
                              >
                            </v-col>
                            <v-col
                              sm="12"
                              cols="12"
                              class="pt-0 pr-0 pb-0 pl-0"
                            >
                              <v-col
                                sm="12"
                                cols="12"
                                class="pt-0 pr-0 pb-0 pl-0"
                                >28/10/2020</v-col
                              >
                            </v-col>
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="12" sm="6">
                            <v-row>
                              <v-col class="py-0" sm="12">
                                <span style="font-weight: bold; font-size: 17px"
                                  >Surveys</span
                                >
                              </v-col>
                              <v-col sm="12" cols="12" class="pt-0 pr-0 pb-0">
                                <v-col
                                  sm="12"
                                  cols="12"
                                  style="color: #585858"
                                  class="pt-0 pr-0 pb-0 pl-0"
                                  >3</v-col
                                >
                              </v-col>
                            </v-row>
                          </v-col>
                          <v-col sm="6" cols="12" class="pl-0">
                            <v-col class="py-0 pl-0" sm="12">
                              <span style="font-weight: bold; font-size: 17px"
                                >Beneficiary Type</span
                              >
                            </v-col>
                            <v-col
                              sm="12"
                              cols="12"
                              class="pt-0 pr-0 pb-0 pl-0"
                            >
                              <v-col
                                sm="12"
                                cols="12"
                                style="color: #585858"
                                class="pt-0 pr-0 pb-0 pl-0"
                                >Farmers</v-col
                              >
                            </v-col>
                          </v-col>
                        </v-row>
                      </v-col>
                    </v-row>
                  </v-card>
                </v-tab-item>
                <v-tab-item value="tabs-2">
                  <v-card
                    elevation="0"
                    class="v-scrolling-div"
                    style="overflow-x: hidden; height: 400px"
                  >
                    <v-row justify="center">
                      <v-col cols="12" sm="11">
                        <v-row>
                          <v-col sm="12" cols="12">
                            <v-list three-line>
                              <template v-for="(item, index) in items">
                                <v-subheader
                                  v-if="item.header"
                                  :key="item.header"
                                ></v-subheader>

                                <v-divider
                                  v-else-if="item.divider"
                                  :key="index"
                                  :inset="item.inset"
                                ></v-divider>

                                <v-list-item v-else :key="item.title">
                                  <v-list-item-avatar class="pt-3">
                                    <v-img
                                      style="height: 50px"
                                      :src="item.avatar"
                                    ></v-img>
                                  </v-list-item-avatar>

                                  <v-list-item-content>
                                    <v-list-item-title
                                    style="font-weight:bold;font-size:13px;color:black;"
                                      v-html="item.title"
                                    ></v-list-item-title>
                                    <v-list-item-subtitle
                                    style="font-weight:bold;font-size:13px;color:black;"
                                      v-html="item.subtitle"
                                    ></v-list-item-subtitle>
                                  </v-list-item-content>
                                  <v-list-item-action>
                                    <span style="font-size:13px;">7/21/20 at 3.51 pm</span>
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
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import BarChart from "@/components/BarChart.vue";
import LineChart from "@/components/LineChart.vue";
export default {
  data() {
    return {
      printer: require("@/assets/Survey/Print.svg"),
      excel: require("@/assets/Survey/Export_Excel.svg"),
      items1: [
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
          name: "Baseline Farmer Survey",
          image: require("@/assets/Beneficieries/Survey_Icon.svg"),
        },
        {
          name: "Planned",
          image: require("@/assets/Beneficieries/Survey_Icon.svg"),
        },
        {
          name: "Planned v/s ",
          image: require("@/assets/Beneficieries/Survey_Icon.svg"),
        },
      ],
      data_survey_list1: [
        {
          name: "Baseline Farmer Survey",
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
      responsesBarData: {
        labels: ["Teach Each", "Mission Home", "Funraisers"],
        datasets: [
          {
            label: "Total Budget",
            data: [500000, 900000, 700000],
            backgroundColor: ["#F1C100", "#F1C100", "#F1C100"],
          },
        ],
      },
      responsesBarOptions: {
        responsive: true,
        maintainAspectRatio: false,
        legend: {
          display: false,
        },
        scales: {
          yAxes: [
            {
              barPercentage: 1,
              categoryPercentage: 1,
              ticks: {
                beginAtZero: true,
                min: 0,
                max: 1000000,
                stepSize: 200000,
              },
            },
          ],
          xAxes: [
            {
              barThickness: 15,
              categoryPercentage: 0.55,
            },
          ],
        },
      },
      responsesLineData: {
        labels: ["Organic Method", "Organic Method", "Other Method"],
        datasets: [
          {
            label: "Options",
            data: [10, 60, 85],
            backgroundColor: "transparent",
            borderColor: "#F1C100",
          },
        ],
      },
      responsesLineOptions: {
        responsive: true,
        maintainAspectRatio: false,
        legend: {
          display: false,
        },
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true,
              },
            },
          ],
        },
      },
      msExcel: require("@/assets/microsoft-excel.png"),
      items: [
        {
          avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
          title: "Varun Mittal",
          subtitle: "Updated Task",
        },
        { divider: true, inset: true },
        {
          avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
          title: "Varun Mittal",
          subtitle: "Updated Task",
        },
        { divider: true, inset: true },
        {
          avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
          title: "Varun Mittal",
          subtitle: "Updated Task",
        },
        { divider: true, inset: true },
      ],
    };
  },
  components: {
    "Bar-Chart": BarChart,
    "Line-Chart": LineChart,
  },
};
</script>