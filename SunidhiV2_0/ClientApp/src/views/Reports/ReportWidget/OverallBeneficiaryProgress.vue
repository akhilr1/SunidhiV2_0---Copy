<template>
  <v-container class="py-0" fluid>
    <v-row>
      <v-col sm="3" cols="12" class="pt-8 pb-0">
        <v-row>
          <v-col sm="5" cols="12" class="pt-0" style="text-align: end">
            <v-avatar>
              <img :src="Beneficiary_Type" alt="John" style="font-size: 60px" />
            </v-avatar>
          </v-col>
          <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
            <v-col sm="12" class="py-0 pl-0">
              <span style="font-size: 21px; font-weight: bold">145</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Beneficiaries</span>
            </v-col>
          </v-col>
        </v-row>
      </v-col>
      <v-col sm="3" cols="12" class="pt-8 pb-0">
        <v-row>
          <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
            <v-avatar>
              <img :src="Total_" alt="John" style="font-size: 60px" />
            </v-avatar>
          </v-col>
          <v-col sm="7" cols="12" class="pt-0 pl-0 ">
            <v-col sm="12" class="py-0 pl-0">
              <span style="font-size: 21px; font-weight: bold">4</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Beneficiary Types</span>
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
              <span style="font-size: 21px; font-weight: bold">10</span>
            </v-col>
            <v-col sm="12" class="pt-0 pl-0">
              <span style="font-size: 12px">Total Survey Collected</span>
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
    Beneficiary_Type: require("@/assets/Beneficieries/Beneficiary_Type.svg"),
    Total_: require("@/assets/Beneficieries/Total_.svg"),
    completed: require("@/assets/Outcomes/totalcompleted.svg"),
  }),
};
</script>