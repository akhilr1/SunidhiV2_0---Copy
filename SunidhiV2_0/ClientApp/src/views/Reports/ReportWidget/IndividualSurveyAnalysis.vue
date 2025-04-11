<template>
  <v-container class="pt-0 pb-0" fluid>
    <v-row>
      <v-col sm="4" cols="12" class="pl-8 pt-0">
        <v-row>
          <v-col class="pb-0" sm="12" cols="12">
            <span style="color: #2d2f40; font-size: 17px; font-weight: bold"
              >Individual Survey Analysis</span
            >
          </v-col>
        </v-row>
      </v-col>
    </v-row>
    <v-row class="pl-4 pt-0">
      <v-col class="pt-0" sm="2" cols="12">
        <v-select dense label="Start Date" outlined hide-details></v-select>
      </v-col>
      <v-col class="pt-0" sm="2" cols="12">
        <v-select dense label="End Date" outlined hide-details></v-select>
      </v-col>
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
      <v-col class="pt-0" sm="2" cols="12">
        <v-btn elevation="1" block color="#3374B9" style="color: white"
          >Generate</v-btn
        >
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
          name: "Baseline Farmer Survey",
          image: require("@/assets/Survey/Survey_Icon.svg"),
        },
        { name: "Baseline ", image: require("@/assets/Survey/Survey_Icon.svg") },
        {
          name: "Baseline Farmer ",
          image:  require("@/assets/Survey/Survey_Icon.svg"),
        },
      ],
    };
  },
};
</script>
