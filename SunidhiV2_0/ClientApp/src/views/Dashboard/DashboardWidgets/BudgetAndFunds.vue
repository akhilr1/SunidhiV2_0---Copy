<template>
  <v-card style="height: 520px;overflow-x:hidden;" class="v-scrolling-div" elevation="1">
    <v-row class="pt-1">
      <v-col sm="6" cols="6" class="pt-5">
        <h1 class="header-font page-head-dark font-weight-medium pl-4">Budget & Funds</h1>
      </v-col>
      <v-col sm="6" cols="6">
        <v-select
          dense
          label="Donors"
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
    <v-row justify="center">
      <v-col sm='6' class="pb-0  d-flex justify-md-start ">
        <v-avatar class="mt-1 mx-2 ml-5 ml-md-2" size="15" tile color="#00B448"> </v-avatar>
        <span class="legend-text">Isabella Ayala</span>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm='6'  class="pt-0  d-flex justify-md-start ">
      <span class="legend-text  ml-10 ml-md-8">Amount Funded : <span class="font-weight-bold text--secondary">Rs 40000</span></span>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm='6' class="pt-0 pb-0 d-flex justify-md-start ">
        <v-avatar class="mt-1 mx-2 ml-5 ml-md-2" size="15" tile color="#FFA200"> </v-avatar>
        <span class="legend-text">Sam John</span>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm='6'  class="pt-0 d-flex justify-md-start ">
      <span class="legend-text ml-10 ml-md-8">Amount Funded : <span class="font-weight-bold text--secondary">Rs 60000</span></span>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm='6'  class="pt-0 d-flex justify-md-start">
      <span class="legend-text ml-10 ml-md-8">Total Budget : <span class="font-weight-bold text--secondary">Rs 10000</span></span>
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
        labels: ["Isabella Ayala", "Sam John"],
        datasets: [
          {
            label: "Data One",
            backgroundColor: ["#00B448", "#FFA200"],
            data: [85, 25],
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
.header-font{
  font-family: 'Poppins', sans-serif;
}
</style>