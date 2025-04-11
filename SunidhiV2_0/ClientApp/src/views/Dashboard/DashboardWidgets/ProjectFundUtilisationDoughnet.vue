<template>
  <v-card class="card" elevation="1">
    <v-row class="pt-1">
      <v-col sm="12" cols="12" class="pt-5">
        <h1 class="page-head-dark font-weight-medium pl-4">
          Projects Fund Utilisation
        </h1>
      </v-col>
    </v-row>
    <v-row style="height: 290px">
      <v-col sm="12" md="6" cols="12" class="d-flex justify-center">
        <div class="chart-container">
          <DoughnutChart
            :doughnutChartdata="chartData"
            :doughnutChartOptions="chartOptions"
          ></DoughnutChart>
        </div>
      </v-col>
      <v-col>
        <v-row style="padding-top: 100px" justify="start">
          <v-col sm="6" class="d-flex justify-md-start justify-center">
            <v-avatar class="mt-1 mx-2" size="15" tile color="#FFA200">
            </v-avatar>
            <span class="legend-text"
              >Teach Each :
              <span class="font-weight-bold text--secondary"
                >Rs 70000</span
              ></span
            >
          </v-col>
        </v-row>
        <v-row justify="start">
          <v-col sm="6" class="pt-0 d-flex justify-md-start justify-center">
            <v-avatar class="mt-1 mx-2" size="15" tile color="#EB5A46">
            </v-avatar>
            <span class="legend-text"
              >Mission Home:
              <span class="font-weight-bold text--secondary"
                >Rs 70000</span
              ></span
            >
          </v-col>
        </v-row>
        <v-row justify="start">
          <v-col sm="6" class="pt-0 d-flex justify-md-start justify-center">
            <v-avatar class="mt-1 mx-2" size="15" tile color="#0099FF">
            </v-avatar>
            <span class="legend-text"
              >Funraisers :
              <span class="font-weight-bold text--secondary"
                >Rs 70000</span
              ></span
            >
          </v-col>
        </v-row>
      </v-col>
    </v-row>
    <v-row class="pt-10">
      <v-col sm="6" class="total-budget">
        <span class="total-budget-val font-weight-bold text--secondary"> Rs. 10000 </span><br/>
        <span class="total-budget-text font-weight-bold text--secondary">Total Budget</span>
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
        responsive: true,
        hoverBorderWidth: 20,
        legend: {
          display: false,
        },
      },
      chartData: {
        hoverBackgroundColor: "red",
        hoverBorderWidth: 10,
        labels: ["Teach Each", "Mission Home", "Funraisers"],
        datasets: [
          {
            label: "Data One",
            backgroundColor: ["#FFA200", "#EB5A46", "#0099FF"],
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
  height: 70%;
  width: 70%;
}
.card {
  height: 490px;
}
.legend-text {
  margin-top: 2px;
}
.total-budget {
  text-align: center;
}
.total-budget-val{
  font-size: 20px;
}
.total-budget-text{
  font-size: 12px;
}
</style>