<template>
  <v-card style="height: 490px;overflow-x:hidden;" class="v-scrolling-div" elevation="1">
    <v-row class="pt-1">
      <v-col sm="12" cols="12" class="pt-5">
        <h1 class="header-font page-head-dark font-weight-medium pl-4">
          Projects Fund Utilisation
        </h1>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm="12" class="d-flex justify-center">
          <Bar-Chart
            style="height: 360px; width: 80%"
            :barData="responsesBarData"
            :barOptions="responsesBarOptions"
          ></Bar-Chart>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm="6" class="pb-0 d-flex justify-center">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#20C997"> </v-avatar>
        <span class="legend-text">Total Budget</span>
        <v-avatar class="mt-1 mx-2" size="15" tile color="#FFB65A"> </v-avatar>
        <span class="legend-text">Amount Used</span>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
import BarChart from "@/components/BarChart.vue";
export default {
  components: {
    BarChart,
  },
  data() {
    return {
      responsesBarData: {
        labels: ["Teach Each", "Mission Home", "Funraisers"],
        datasets: [
          {
            label: "Total Budget",
            data: [500000, 900000, 700000],
            backgroundColor: ["#20C997", "#20C997", "#20C997"],
          },
          {
            label: "Amount Used",
            data: [300000, 500000, 600000],
            backgroundColor: ["#FFB65A", "#FFB65A", "#FFB65A"],
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
    };
  },
};
</script>
<style scoped>
</style>