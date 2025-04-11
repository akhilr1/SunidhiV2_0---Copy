<template>
  <v-card style="height: 520px" elevation="1">
    <v-row class="pt-1">
      <v-col sm="9" cols="6" class="pt-5">
        <h1 class="header-font page-head-dark font-weight-medium pl-4">
          Planned & Actual
        </h1>
      </v-col>
      <v-col sm="3" cols="6">
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
      <v-col sm="12">
        <canvas id="myBarCharts" style="height: 360px;width:97%;"></canvas>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm="6" class="pb-0 d-flex justify-center">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#0202AA"> </v-avatar>
        <span class="legend-text">Planned</span>
        <v-avatar class="mt-1 mx-2" size="15" tile color="#89D8B0"> </v-avatar>
        <span class="legend-text">Actual</span>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
export default {
  methods: {
    /// <summary>
    ///  PlannedAndActualCharts :- Function used to show planned and actual chart
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async PlannedAndActualCharts() {
      var ctx = document.getElementById("myBarCharts");
      var myChart = new Chart(ctx, {
        type: "horizontalBar",
        data: {
          datasets: [
            {
              label: ["Planned"],
              data: [700000, 260000, 450000, 850000, 650000],
              backgroundColor: [
                "#0202AA",
                "#0202AA",
                "#0202AA",
                "#0202AA",
                "#0202AA",
              ],
              barPercentage: 0.5,
              barThickness: 15,
              maxBarThickness: 20,
              minBarLength: 2,
            },
            {
              label: ["Actual"],
              data: [200000, 900000, 800000, 600000, 500000],
              backgroundColor: [
                "#89D8B0",
                "#89D8B0",
                "#89D8B0",
                "#89D8B0",
                "#89D8B0",
              ],
              barPercentage: 0.5,
              barThickness: 15,
              maxBarThickness: 20,
              minBarLength: 2,
            },
          ],

          labels: [
            "Activity",
            "Salary",
            "Travel",
            "Audit Fees",
            "Bank Charges",
          ],
        },
        options: {
          legend: {
            display: false,
          },
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            xAxes: [
              {
                ticks: {
                  beginAtZero: true,
                  min: 0,
                  max: 1000000,
                  stepSize: 200000,
                },
              },
            ],
          },
        },
      });
    },
  },
  mounted: async function () {
    await this.PlannedAndActualCharts();
  },
};
</script>
<style scoped>
.legend-text {
  margin-top: 2px;
}
</style>