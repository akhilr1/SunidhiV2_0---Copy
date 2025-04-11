<template>
  <v-container class="pt-0 pb-0" fluid>
    <v-row>
      <v-col sm="4" cols="12" class="pl-8 pt-0">
        <v-row>
          <v-col class="pb-0" sm="12" cols="12">
            <span style="color: #2D2F40; font-size: 17px; font-weight: bold"
              >Individual Output Analysis</span
            >
          </v-col>
        </v-row>
      </v-col>
    </v-row>
    <v-row class="pl-4 pt-0">
       <v-col class="pt-0" sm="4" cols="12">
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
              >{{ item.name.length >= 30 ?item.name.substring(0, 30)+'...':item.name}}</span
            >
          </template>
          <template v-slot:item="prop">
            <img :src="prop.item.image" /><span
              class="ml-3"
              style="font-size: 14px"
              >{{ prop.item.name.length >= 30 ?prop.item.name.substring(0, 30)+'...':prop.item.name}}</span
            >
          </template>
        </v-select>
      </v-col>
       <v-col cols="12" class="pt-0" sm="2">
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
      <v-col class="pt-0" sm="2" cols="12">
        <v-btn elevation="1" block color="#3374B9" style="color:white;">Generate</v-btn>
      </v-col>
      <v-col class="pt-0" sm="2" cols="12">
        <v-btn elevation="1" block color="#EEEEEE">Clear All</v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      data_survey_list: [
        {
          name: "Increase in the overall annual income of farmers.",
          image: require("@/assets/Outputs/Output_Icon.svg"),
        },
        { name: "Baseline ", image: require("@/assets/Outputs/Output_Icon.svg") },
        {
          name: "Baseline Farmer ",
          image:  require("@/assets/Outputs/Output_Icon.svg"),
        },
      ],
    };
  },
};
</script>