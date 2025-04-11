<template>
  <v-card style="height: 490px;overflow-x:hidden;overflow-y:auto;" elevation="1">
    <v-row class="pt-1">
      <v-col sm="6" cols="6" class="pt-5">
        <h1 class="page-head-dark font-weight-medium pl-4">Budget Plan</h1>
      </v-col>
      <v-col sm="6" cols="6">
        <v-select
          dense
          label="Teach Each"
          outlined
          hide-details
          class="mr-2"
        ></v-select>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm="12" cols="12" class="d-flex justify-center">
        <div class="chart-container">
          <DoughnutChart
            :doughnutChartdata="chartData"
            :doughnutChartOptions="chartOptions"
          ></DoughnutChart>
        </div>
      </v-col>
    </v-row>
    <v-row justify-md="center">
      <v-col sm='7' class="d-flex justify-md-start ">
        <v-avatar class="mt-1 mx-2 ml-5 ml-md-2" size="15" tile color="#388E3C"> </v-avatar>
        <span class="legend-text">Activity Expense : <span class="font-weight-bold text--secondary">Rs 70000</span></span>
      </v-col>
    </v-row>
    <v-row justify-md="center">
      <v-col sm='7'   class="pt-0 d-flex justify-md-start ">
      <v-avatar class="mt-1 mx-2 ml-5 ml-md-2" size="15" tile color="#0099FF"> </v-avatar>
      <span class="legend-text">Travel : <span class="font-weight-bold text--secondary">Rs 70000</span></span>
      </v-col>
    </v-row>
    <v-row justify-md="center">
      <v-col sm='7' class="pt-0 d-flex justify-md-start ">
      <v-avatar class="mt-1 mx-2 ml-5 ml-md-2" size="15" tile color="#FFA200"> </v-avatar>
      <span class="legend-text ">Shopping : <span class="font-weight-bold text--secondary">Rs 70000</span></span>
      </v-col>
    </v-row>
    <v-row   justify-md="center">
      <v-col sm='7'  class="pt-0 d-flex justify-md-start ">
      <span class="legend-text ml-5 ml-md-2">Total Budget : <span class="font-weight-bold text--secondary">Rs 70000</span></span>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
import DoughnutChart from "@/components/DoughnutChart.vue";
export default {
  components: {
    DoughnutChart,
  },
  data() {
    return {
      chartOptions: {
        hoverBorderWidth: 20,
        legend: {
          display: false,
        },
      },
      chartData: {
        hoverBackgroundColor: "red",
        hoverBorderWidth: 10,
        labels: ["Activity Expense", "Travel", "Shopping"],
        datasets: [
          {
            label: "Data One",
            backgroundColor: ["#388E3C", "#0099FF", "#FFA200"],
            data: [25, 25, 25],
          },
        ],
      },
    };
  },
};
</script>
<style scoped>
.chart-container {
  height: 55%;
  width: 55%;
}
.legend-text {
  margin-top: 2px;
}
</style>