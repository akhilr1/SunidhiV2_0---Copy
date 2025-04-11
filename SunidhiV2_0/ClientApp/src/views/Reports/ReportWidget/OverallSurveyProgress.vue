<template>
  <v-container class="py-0" fluid>
    <v-row>
      <v-col sm="4" cols="12" class="pl-8 pb-0">
        <v-row>
          <v-col sm="12" cols="12">
            <span style="color: #5e5e5e">Overall Survey Progress</span>
          </v-col>
          <v-col sm="9" cols="12" class="pt-0">
            <v-progress-linear
              v-model="power"
              color="#0099FF"
              height="25"
            ></v-progress-linear>
          </v-col>
          <v-col sm="3" cols="12" class="pt-0">
            <span style="font-size: 18px; font-weight: bold">{{ power }}%</span>
          </v-col>
        </v-row>
      </v-col>
      <v-col sm="2" cols="12" class="pt-8 pb-0">
        <v-row>
          <v-col sm="5" cols="12" class="pt-0" style="text-align: end">
            <v-avatar>
              <img :src="survey" alt="John" style="font-size: 60px" />
            </v-avatar>
          </v-col>
          <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
            <v-col sm="12" class="py-0 pl-0">
              <span style="font-size: 21px; font-weight: bold">10</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Surveys</span>
            </v-col>
          </v-col>
        </v-row>
      </v-col>
      <v-col sm="3" cols="12" class="pt-8 pb-0">
        <v-row>
          <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
            <v-avatar>
              <img :src="response" alt="John" style="font-size: 60px" />
            </v-avatar>
          </v-col>
          <v-col sm="7" cols="12" class="pt-0 pl-0 ">
            <v-col sm="12" class="py-0 pl-0">
              <span style="font-size: 21px; font-weight: bold">35,000</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Responses</span>
            </v-col>
          </v-col>
        </v-row>
      </v-col>
      <v-col sm="3" cols="12" class="pt-8 pb-0">
        <v-row>
          <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
            <v-avatar>
              <img :src="completed" alt="John" style="font-size: 60px" />
            </v-avatar>
          </v-col>
          <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
            <v-col sm="12" class="py-0 pl-0">
              <span style="font-size: 21px; font-weight: bold">3</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Completed</span>
            </v-col>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
export default {
  data: () => ({
    skill: 20,
    knowledge: 33,
    power: 78,
    survey: require("@/assets/Survey/Total_Surveys.svg"),
    response: require("@/assets/Survey/Total_Responses.svg"),
    completed: require("@/assets/Survey/Total_completed.svg"),
  }),
};
</script>