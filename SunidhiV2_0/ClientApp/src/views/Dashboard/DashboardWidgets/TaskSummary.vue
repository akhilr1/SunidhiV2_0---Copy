<template>
  <v-card
    style="height: 490px; overflow-x: hidden"
    elevation="1"
  >
    <v-row class="pt-1">
      <v-col sm="6" cols="6" class="pt-5">
        <h1 class="header-font page-head-dark font-weight-medium pl-4">
          Task Summary
        </h1>
      </v-col>
    </v-row>
    <template v-if="!renderFlag">
        <v-row >
          <v-col xs="12">
            <v-card-title>
              No Tasks Found
            </v-card-title>
          </v-col>
        </v-row>
    </template>
    <v-row>
      <v-col sm="12" cols="12" class="d-flex justify-center mt-4">
        <div class="chart-container" v-if="doughnutDataLoaded">
          <DoughnutChart
            :doughnutChartdata="chartData"
            :doughnutChartOptions="chartOptions"
          ></DoughnutChart>
        </div>
      </v-col>
    </v-row>
    <v-row justify="center" v-if="renderFlag">
      <v-col sm="6" class="pb-0 d-flex justify-center">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#20C997"> </v-avatar>
        <span class="legend-text">Completed</span>
        <v-avatar class="mt-1 mx-2" size="15" tile color="#FFB65A"> </v-avatar>
        <span class="legend-text">Active</span>
        <v-avatar class="mt-1 mx-2" size="15" tile color="#007BFF"> </v-avatar>
        <span class="legend-text">Assigned</span>
      </v-col>
    </v-row>
  </v-card>
</template>

<script>
import DoughnutChart from "@/components/DoughnutChart.vue";

var Dash
async function importscript() {
    return Promise.all([
        import("@/BL/Dashboard.js").then(mod => {
            Dash = new mod.Dashboard();
        })
    ]);
}

export default {
  components: {
    DoughnutChart,
  },
  data() {
    return {
      doughnutDataLoaded: false,
      renderFlag: true,
      WorkspaceID: null,
      ActiveCount: 0,
      CompletedCount: 0,
      AssignedCount: 0,
      chartOptions: {
        hoverBorderWidth: 20,
        maintainAspectRatio: true,
        legend: {
          display: false,
        },
      },
      chartData: null,
      dashdata: null,
    };
  },
  mounted: async function () {
    await importscript();
    this.WorkspaceID = this.$route.query.WsID;
    await this.fnFillDashboard().then(() => {
        this.ActiveCount = this.dashdata.ActiveList.length > 0 ? this.dashdata.ActiveList[0].ActiveCunt : 0;
        this.AssignedCount = this.dashdata.AssignedList.length > 0 ? this.dashdata.AssignedList[0].AssignedCunt: 0 ;
        this.CompletedCount = this.dashdata.CompletedCount;
    })
    const payload = {
        hoverBackgroundColor: "red",
        hoverBorderWidth: 10,
        labels: ["Completed", "Active", "Assigned"],
        datasets: [
          {
            label: "Data One",
            backgroundColor: ["#20C997", "#FFB65A", "#007BFF"],
            data: [this.CompletedCount, this.ActiveCount, this.AssignedCount],
          },
        ],
      }

      this.chartData = payload
      this.doughnutDataLoaded = true
      // console.log(this.ActiveCount, this.AssignedCount, this.CompletedCount)
      if(this.ActiveCount == 0 && this.AssignedCount == 0 && this.CompletedCount == 0) {
        this.renderFlag = false
      }
  },
  methods: {
    async fnFillDashboard() {
      let formdata = new FormData();
      formdata.append("WorkspaceID", this.WorkspaceID);
      let result = await Dash.FillDashboard(formdata)
      this.dashdata = result.data[0]
    },

  },
};
</script>

<style scoped>
.chart-container {
  height: 55%;
  width: 55%;
}
.card {
  height: 520px;
}
.legend-text {
  margin-top: 2px;
}
.header-font {
  font-family: "Poppins", sans-serif;
}
</style>