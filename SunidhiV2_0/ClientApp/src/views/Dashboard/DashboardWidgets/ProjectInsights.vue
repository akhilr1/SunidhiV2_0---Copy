<template>
  <v-card style="height: 490px" elevation="1">
    <v-row class="pt-1">
      <v-col md="9" class="pt-5">
        <h3 class="page-head-dark font-weight-medium pl-4">
          Projects Insights
        </h3>
      </v-col>
      <v-col md="3">
        <v-select
          dense
          label="Sort Status"
          outlined
          hide-details
          class="mr-2"
        ></v-select>
      </v-col>
    </v-row>
    <v-container class="hidden-lg-and-up pa-0">
      <div
        style="height: 322px; overflow-x: hidden"
        class="px-0 v-scrolling-div"
      >
        <v-row class="px-2" v-for="(item, i) in expansionData" :key="i">
          <v-col sm="12" style="text-align: center">
            <span
              style="background: #e1efff; border-radius: 5px"
              class="pa-2 grey--text text-no-wrap"
              >23/11/2020</span
            >
          </v-col>
          <v-col>
            <v-card>
              <v-row>
                <v-col sm="10" class="d-flex pl-5">
                  <v-icon color="#AEDFFF" large
                    >mdi-file-document-outline</v-icon
                  >
                  <div class="pt-2">
                    <span style="font-size: 15px; font-weight: bold"
                      >{{ item.title.substring(0, 15) }}...</span
                    >
                  </div>
                  <span class="pt-2 ml-9 mr-1">
                    <v-chip
                      class="px-5"
                      :style="{
                        background: item.chipsColor,
                        color: item.chipsColorText,
                        height: '23px',
                      }"
                      label
                    >
                      {{ item.outcome }}
                    </v-chip></span
                  >
                </v-col>
                <v-col
                  sm="6"
                  cols="6"
                  class="pr-1"
                  :style="{
                    'border-bottom':
                      dividerIndex != null
                        ? '1px solid rgba(0,0,0,0.12)'
                        : '0px',
                  }"
                >
                  <v-btn
                    elevation="0"
                    tile
                    :disabled="
                      dividerIndex == i && buttonIndex == 1 ? true : false
                    "
                    :style="{
                      'padding-top': '25px',
                      'padding-bottom': '25px',
                      'border-bottom':
                        dividerIndex == i && buttonIndex == 0
                          ? '3px solid #3374B9'
                          : '0px',
                      background:
                        buttonIndex == 0 ? 'white !important' : 'white',
                    }"
                    @click="
                      onButtonClickMobile(
                        item.title,
                        item.outcome,
                        item.chipsColor,
                        item.chipsColorText,
                        'tab-1',
                        'Insights',
                        '2'
                      )
                    "
                  >
                    <v-avatar>
                      <img :src="insights" alt="John" />
                    </v-avatar>
                    <span class="ml-2" style="font-weight: bold; color: black"
                      >2 Insights</span
                    >
                  </v-btn>
                </v-col>
                <v-col sm="6" cols="6" class="pl-1">
                  <v-btn
                    elevation="0"
                    tile
                    :disabled="
                      dividerIndex == i && buttonIndex == 0 ? true : false
                    "
                    :style="{
                      'padding-top': '25px',
                      'padding-bottom': '25px',
                      'border-bottom':
                        dividerIndex == i && buttonIndex == 1
                          ? '3px solid #3374B9'
                          : '0px',
                      background: 'white',
                    }"
                    @click="
                      onButtonClickMobile(
                        item.title,
                        item.outcome,
                        item.chipsColor,
                        item.chipsColorText,
                        'tab-1',
                        'Suggestions',
                        '2'
                      )
                    "
                  >
                    <v-avatar>
                      <img :src="suggestion" alt="John" />
                    </v-avatar>
                    <span class="ml-0" style="font-weight: bold; color: black"
                      >2 Suggestions</span
                    >
                  </v-btn>
                </v-col>
                <v-dialog
                  v-model="dialog"
                  transition="dialog-bottom-transition"
                  persistent
                >
                  <v-card
                    class="elevation-0"
                    style="height: 75vh; overflow-x: hidden; overflow-y: hidden"
                  >
                    <v-row class="pt-3">
                      <v-row class="mx-4 pt-6" no-gutters>
                        <v-col
                          cols="auto"
                          class="limited-width-CardTitle text-truncate page-head"
                        >
                          <v-btn
                            class="d-sm-none"
                            text
                            small
                            @click="dialog = false"
                          >
                            <v-icon color="#707070" size="30"
                              >mdi-keyboard-backspace</v-icon
                            >
                          </v-btn>
                          {{ dialogSelectedTitle }}
                        </v-col>
                        <v-col cols="4" class="text-right text-md-left">
                          <v-chip
                            class="ml-2"
                            label
                            dark
                            small
                            :style="{
                              background: dialogSelectedChip,
                              color: dialogSelectedChipColor,
                              height: '23px',
                            }"
                          >
                            {{ dialogSelectedOutcome }}
                          </v-chip>
                        </v-col>
                        <v-spacer></v-spacer>
                        <v-col
                          cols="2"
                          align="right"
                          class="d-none d-sm-inline"
                        >
                          <v-btn text small @click="fnInitializeTaskApprove()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                          </v-btn>
                        </v-col>
                      </v-row>
                    </v-row>
                    <v-divider class="mt-3"></v-divider>
                    <!-- Task Details and History-->
                    <v-card-text class="px-3">
                      <v-tabs
                        color="#3374B9"
                        grow
                        class="pt-0"
                        height="9vh"
                        v-model="active_tab"
                      >
                        <v-tab href="#tab-1">
                          <v-avatar>
                            <img :src="dialogSelectedTab == 'Insights' ? insights : suggestion" alt="John" />
                          </v-avatar>
                          <span
                            class="ml-2 mt-1"
                            style="font-weight: bold; color: black;font-size:16px;"
                            >{{dialogSelectedTotal}} {{dialogSelectedTab}}</span
                          >
                        </v-tab>
                        <!-- Task Details-->
                        <v-tab-item value="tab-1">
                          <v-container
                            style="
                              height: 50vh;
                              overflow-x: hidden;
                              overflow-y: auto;
                            "
                          >
                            <v-row>
                              <template v-for="(item, index) in items">
                                <v-col sm="11" class="d-flex pl-8" :key="index">
                                  <v-avatar v-if="item.header">
                                    <img :src="impact" alt="John" />
                                  </v-avatar>
                                  <v-list three-line>
                                    <v-subheader
                                      v-if="item.header"
                                      :key="item.header"
                                      style="height: 10px"
                                    >
                                      <span
                                        style="font-size: 17px; color: black"
                                        >{{ item.header }}</span
                                      >
                                    </v-subheader>
                                    <v-list-item
                                      v-if="item.title"
                                      :key="item.title"
                                    >
                                      <v-list-item-avatar tile>
                                        <v-img :src="item.avatar"></v-img>
                                      </v-list-item-avatar>

                                      <v-list-item-content>
                                        <v-list-item-subtitle
                                          v-html="item.title"
                                        ></v-list-item-subtitle>
                                        <v-list-item-subtitle
                                          v-html="item.subtitle"
                                        ></v-list-item-subtitle>
                                      </v-list-item-content>
                                    </v-list-item>
                                  </v-list>
                                  <v-divider class="mt-3"></v-divider>
                                </v-col>
                              </template>
                            </v-row>
                          </v-container>
                        </v-tab-item>
                        <!-- History -->
                        <v-tab-item value="tab-2">
                          <v-container>
                            <v-row>
                              <template v-for="(item, index) in items">
                                <v-col sm="11" class="d-flex pl-8" :key="index">
                                  <v-avatar v-if="item.header">
                                    <img :src="impact" alt="John" />
                                  </v-avatar>
                                  <v-list three-line>
                                    <v-subheader
                                      v-if="item.header"
                                      :key="item.header"
                                      style="height: 10px"
                                    >
                                      <span
                                        style="font-size: 17px; color: black"
                                        >{{ item.header }}</span
                                      >
                                    </v-subheader>
                                    <v-list-item
                                      v-if="item.title"
                                      :key="item.title"
                                    >
                                      <v-list-item-avatar tile>
                                        <v-img :src="item.avatar"></v-img>
                                      </v-list-item-avatar>

                                      <v-list-item-content>
                                        <v-list-item-subtitle
                                          v-html="item.title"
                                        ></v-list-item-subtitle>
                                        <v-list-item-subtitle
                                          v-html="item.subtitle"
                                        ></v-list-item-subtitle>
                                      </v-list-item-content>
                                    </v-list-item>
                                  </v-list>
                                </v-col>
                              </template>
                            </v-row>
                          </v-container>
                        </v-tab-item>
                      </v-tabs>
                    </v-card-text>
                  </v-card>
                </v-dialog>
              </v-row>
            </v-card>
          </v-col>
        </v-row>
      </div>
    </v-container>
    <v-container fluid class="d-none d-lg-block">
      <v-row>
        <v-col
          style="height: 410px; overflow-x: hidden"
          class="px-0 v-scrolling-div"
        >
          <v-row class="mx-2 mb-1" v-for="(item, i) in expansionData" :key="i">
            <v-col class="pt-0">
              <v-expansion-panels style="box-shadow: none">
                <v-expansion-panel @click="onExpansionPanelClick(i)">
                  <v-expansion-panel-header
                    hide-actions
                    class="pb-0"
                    expand-icon="mdi-border-outside"
                  >
                    <v-container fluid>
                      <v-row>
                        <v-col sm="10" class="d-flex">
                          <v-icon color="#AEDFFF" medium
                            >mdi-file-document-outline</v-icon
                          >
                          <span
                            class="mt-2"
                            style="font-size: 15px; font-weight: bold"
                            >{{ item.title }}</span
                          >
                          <span class="pt-1 ml-9">
                            <v-chip
                              class="px-5"
                              :style="{
                                background: item.chipsColor,
                                color: item.chipsColorText,
                                height: '23px',
                              }"
                              label
                            >
                              {{ item.outcome }}
                            </v-chip></span
                          >
                        </v-col>
                        <v-col sm="2" style="text-align: end; paddingtop: 30px">
                          <span>23/11/2020</span>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col
                          sm="12"
                          class="pb-0"
                          :style="{
                            'border-bottom':
                              dividerIndex != null
                                ? '1px solid rgba(0,0,0,0.12)'
                                : '0px',
                          }"
                        >
                          <v-btn
                            elevation="0"
                            tile
                            :disabled="
                              dividerIndex == i && buttonIndex == 1
                                ? true
                                : false
                            "
                            :style="{
                              'padding-top': '25px',
                              'padding-bottom': '25px',
                              'border-bottom':
                                dividerIndex == i && buttonIndex == 0
                                  ? '3px solid #3374B9'
                                  : '0px',
                              background:
                                buttonIndex == 0 ? 'white !important' : 'white',
                            }"
                            @click="buttonIndex = 0"
                          >
                            <v-avatar>
                              <img :src="insights" alt="John" />
                            </v-avatar>
                            <span
                              class="ml-3"
                              style="font-weight: bold; color: black"
                              >2 Insights</span
                            >
                          </v-btn>
                          <v-btn
                            elevation="0"
                            tile
                            :disabled="
                              dividerIndex == i && buttonIndex == 0
                                ? true
                                : false
                            "
                            :style="{
                              'padding-top': '25px',
                              'padding-bottom': '25px',
                              'border-bottom':
                                dividerIndex == i && buttonIndex == 1
                                  ? '3px solid #3374B9'
                                  : '0px',
                              background: 'white',
                            }"
                            @click="buttonIndex = 1"
                          >
                            <v-avatar>
                              <img :src="suggestion" alt="John" />
                            </v-avatar>
                            <span
                              class="ml-3"
                              style="font-weight: bold; color: black"
                              >2 Suggestions</span
                            >
                          </v-btn>
                          <!-- <v-divider v-if="dividerIndex != null"></v-divider> -->
                        </v-col>
                      </v-row>
                    </v-container>
                  </v-expansion-panel-header>
                  <v-expansion-panel-content>
                    <v-container fluid class="pt-0">
                      <v-row>
                        <template v-for="(item, index) in items">
                          <v-col
                            sm="12"
                            class="d-flex pl-8"
                            :key="index"
                            style="border-bottom: 1px solid rgba(0, 0, 0, 0.12)"
                          >
                            <v-avatar v-if="item.header">
                              <img :src="impact" alt="John" />
                            </v-avatar>
                            <v-list three-line>
                              <v-subheader
                                v-if="item.header"
                                :key="item.header"
                                style="height: 10px"
                              >
                                <span style="font-size: 17px; color: black">{{
                                  item.header
                                }}</span>
                              </v-subheader>
                              <v-list-item v-if="item.title" :key="item.title">
                                <v-list-item-avatar tile>
                                  <v-img :src="item.avatar"></v-img>
                                </v-list-item-avatar>

                                <v-list-item-content>
                                  <v-list-item-subtitle
                                    v-html="item.title"
                                  ></v-list-item-subtitle>
                                  <v-list-item-subtitle
                                    v-html="item.subtitle"
                                  ></v-list-item-subtitle>
                                </v-list-item-content>
                              </v-list-item>
                            </v-list>
                          </v-col>
                        </template>
                      </v-row>
                    </v-container>
                  </v-expansion-panel-content>
                </v-expansion-panel>
              </v-expansion-panels>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>
<script>
export default {
  data() {
    return {
      dialog: false,
      dialogSelectedTitle: "",
      dialogSelectedOutcome: "",
      dialogSelectedChip: "",
      dialogSelectedChipColor: "",
      dialogSelectedTab: "",
      dialogSelectedTotal: "",
      notifications: false,
      sound: true,
      widgets: false,
      active_tab: "1",
      dividerIndex: null,
      buttonIndex: null,
      quality: require("@/assets/dashboard_asset/text-box-outline.png"),
      insights: require("@/assets/dashboard_asset/Insights.svg"),
      suggestion: require("@/assets/dashboard_asset/Suggestions.svg"),
      impact: require("@/assets/dashboard_asset/Impact.svg"),
      expansionData: [
        {
          title: "Overall quality of training interventions",
          chipsColor: "#DFFFEC",
          chipsColorText: "#52B962",
          outcome: "Outputs",
        },
        {
          title: "Overall quality of training interventions",
          chipsColor: "#FFECD4",
          chipsColorText: "#FF5722",
          outcome: "Outcomes",
        },
        {
          title: "Overall quality of training interventions",
          chipsColor: "#FFECD4",
          chipsColorText: "#FF5722",
          outcome: "Outcomes",
        },
      ],
      items: [
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
  methods: {
    /// <summary>
    ///  onExpansionPanelClick :- Function used to show items when click on panel based on index
    /// </summary>
    /// <param name="index"></param>
    /// <returns> </returns>
    onExpansionPanelClick(index) {
      if (this.buttonIndex == null) {
        this.buttonIndex = 0;
      }
      if (index != this.dividerIndex) {
        this.dividerIndex = index;
      } else {
        this.dividerIndex = null;
        this.buttonIndex = null;
      }
    },
   
    /// <summary>
    ///  onButtonClickMobile :- Function used to show items when click on button in mobile
    /// </summary>
    /// <param name="title"></param>
    /// <param name="outcome"></param>
    /// <param name="chip"></param>
    /// <param name="chipsColor"></param>
    /// <param name="tab"></param>
    /// <param name="selectedTab"></param>
    /// <param name="total"></param>
    /// <returns> </returns>
    onButtonClickMobile(title, outcome, chip, chipsColor, tab,selectedTab,total) {
      this.dialog = true;
      this.dialogSelectedTitle = title;
      this.dialogSelectedOutcome = outcome;
      this.dialogSelectedChip = chip;
      this.dialogSelectedChipColor = chipsColor;
      this.active_tab = tab;
      this.dialogSelectedTab = selectedTab;
      this.dialogSelectedTotal = total;
    },
  },
};
</script>
<style scoped>
.v-btn:before {
  opacity: 0 !important;
}

.v-ripple__container {
  opacity: 0 !important;
}
</style>