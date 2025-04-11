<template>
  <v-container fluid>
    <pre-loader v-show="PreLoader"> </pre-loader>
    <v-row>
      <v-card width="100%" class="parentWrapper">
          <v-card-title>
              <v-btn class="mr-3" large icon @click="$router.back()" color="primary"><v-icon>mdi-chevron-left</v-icon></v-btn>
              Indicators Table
              <v-spacer></v-spacer>
              <v-btn depressed color="green" dark @click="ExportToExcel('xlsx')">
                  <v-icon dense class="px-2">mdi-export-variant</v-icon> Export
              </v-btn>
          </v-card-title>

        <v-card-text>
          <div class="tableContainer v-scrolling-div" @scroll="onScroll">
            <table id="outcomeTable" ref="outcome_table">
              <thead class="v-data-table-header fixed-row">
                <tr class="header-row-1">
                  <th
                    id="tbcol-sl"
                    rowspan="2"
                    class="px-2"
                    :class="{ 'fixed-col col-bg': isScrollLeft }"
                  >
                    SL
                  </th>
                  <th
                    id="tbcol-1"
                    rowspan="2"
                    class="paddingExtra"
                    :class="{ 'fixed-col col-bg': isScrollLeft }"
                    :style="
                      isScrollLeft ? `left: ${getWidth('#tbcol-sl')}px;` : 'left: 0px;'
                    "
                  >
                    Project Outcome Indicator
                  </th>
                  <th id="tbcol-3" rowspan="2" class="px-4">Total Planned</th>
                  <th id="tbcol-4" rowspan="2" class="px-4">Total Achieved</th>
                  <th id="tbcol-4" rowspan="2" class="px-4">SDG</th>

                  <!-- <th id="tbcol-5" class="text-center px-4" colspan="2">
                    Total
                  </th>
                  <th id="tbcol-6" colspan="2"></th> -->
                  <th
                    :id="`monthCol-${id}`"
                    colspan="2"
                    v-for="(i, id) in monthHeaders"
                    :key="id"
                    class="text-center px-4 py-3"
                  >
                    {{ i.label }}
                  </th>
                </tr>
                <tr class="header-row-2">
                  <!-- <th id="subcol-1" class="px-3 py-1">Plan</th>
                  <th id="subcol-2" class="px-3 py-1">Ach</th>
                  <th id="subcol-3" class="text-center px-3 py-1">
                    Target 2022-23
                  </th>
                  <th id="subcol-4" class="text-center px-3 py-1">
                    Target 2023-24
                  </th> -->
                  <template :id="`${val.Month_Year}`" v-for="(val, x) in cMonthHeaders">
                    <th :id="`subcol-${x}.1`" class="text-center pa-4">Plan</th>
                    <th :id="`subcol-${x}.2`" class="text-center pa-4">Ach</th>
                  </template>
                </tr>
              </thead>
              <tbody>
                <tr
                  class="text-center row-hover"
                  :id="`row-${id}`"
                  style="min-height: 40px; height: 40px"
                  v-for="(row, id) in OutcomeReportData"
                  :key="row.OutcomeID"
                >
                  <td :id="`cell-sl-${id}`" :class="{ 'fixed-col td-bg': isScrollLeft }">
                    {{ id + 1 }}
                  </td>
                  <td 
                    :id="`cell-OutcomeName-${id}`"
                    class="outcomeName wordBreak text-start pl-2"
                    :class="{ 'fixed-col td-bg': isScrollLeft }"
                    :style="isScrollLeft ? 'left: 32px;' : 'left: 0px;z-index: 4'"
                  >
                    {{ row.OutcomeName }}
                  </td>
                  <td :id="`cell-AchievedTillValue-${id}`">
                    {{ row.SurveyTarget }}
                  </td>
                  <td :id="`cell-TargetForNextYear-${id}`">
                    {{ row.TotalAchieved }}
                  </td>
                  <td :id="`sdg-${id}`">
                    <template v-if="row.SDG_Goal_Image">
                    <v-avatar tile size="40" class="ma-2 "><img class="sdgImage" :src="require('@/assets/SDG_Icons/'+row.SDG_Goal_Image)"></v-avatar><span class="pr-2"
                    style="z-index: 1;">{{ row.SDG_Goal }}</span>
                  </template>
                  <template v-else>
                    -
                  </template>
                  </td>
                  <template>
                    <!-- <td :id="`cell-total1-${id}`">{{ row.TargetForTheyear == 0 ? 0 : row.TargetForTheyear  - row.AchievedTillValue }}</td>
                    <td :id="`cell-total2-${id}`">{{ row.AchievedTillValue }}</td> -->
                  </template>
                  <template>
                    <!-- <td :id="`cell-target1-${id}`">{{ !row.TargetForTheyear ? 0 : row.TargetForTheyear - row.AchievedTillValue }}</td>
                    <td :id="`cell-target2-${id}`">{{ !row.TargetForTheyear ? 0 : row.TargetForNextYear - row.AchievedTillValue }}</td> -->
                  </template>
                  <template v-for="item in getValues(row.lstOutcomeMonthlyReport)">
                    <td>{{ item.Planned }}</td>
                    <td>{{ item.Achieved }}</td>
                  </template>
                </tr>
              </tbody>
            </table>
          </div>
        </v-card-text>
      </v-card>
    </v-row>
    <v-snackbar v-model="snackBar.value" :color="snackBar.color">
      {{ snackBar.message }}
    </v-snackbar>
  </v-container>
</template>

<script>
import PreLoader from "@/components/pre-loader.vue";
var objManageProjectActivity;
async function importscript() {
  return Promise.all([
    import("@/BL/ManageProjectActivity.js").then((mod) => {
      objManageProjectActivity = new mod.ManageProjectActivity();
    }),
  ]);
}
export default {
  components: {
    PreLoader,
  },
  data() {
    return {
      ProjectId: this.$route.query.ProjID,
      monthHeaders: [],
      OutcomeReportData: [],
      snackBar: {
        message: "",
        value: false,
        color: "",
      },
      achievedTillText: "",
      rowHovered: false,
      PreLoader: false,
      isScrollLeft: false,
      isScrollTop: false,
    };
  },
  computed: {
    cMonthHeaders() {
      const convertToDate = (date) => {
        const months = [
          "JAN",
          "FEB",
          "MAR",
          "APR",
          "MAY",
          "JUN",
          "JUL",
          "AUG",
          "SEP",
          "OCT",
          "NOV",
          "DEC",
        ];
        const [month, year] = date.split(" ");
        const monthIndex = months.indexOf(month);
        return new Date(year, monthIndex, 1);
      };

      this.OutcomeReportData.forEach((x) => {
        if (x.lstOutcomeMonthlyReport && x.lstOutcomeMonthlyReport.length) {
          x.lstOutcomeMonthlyReport.forEach((y) => {
            if (!this.monthHeaders.find((z) => z.label == y.Month_Year)) {
              this.monthHeaders.push({
                label: y.Month_Year,
                id: y.Month_Year.replace(" ", "").toLowerCase(),
                subHeaders: [
                  {
                    label: "Planned",
                  },
                  {
                    label: "Achieved",
                  },
                ],
              });
            }
          });
        }
      });

      return this.monthHeaders.sort((a, b) => {
        if (a.subHeaders.length && b.subHeaders.length) {
          let dA = convertToDate(a.label);
          let dB = convertToDate(b.label);
          return dA - dB;
        }
      });
    },
  },
  async mounted() {
    await importscript();
    await this.getOutcomeReportData();
    // $('tr:odd').not('[class^="header-row-"]').css('background-color', "rgb(128,128,128, 0.07)")
  },
  methods: {
    async getOutcomeReportData() {
      this.PreLoader = true;
      try {
        let { data } = await objManageProjectActivity.GetOutcomeReports(this.ProjectId);
        if (!data || !data.length) throw new Error("Invalid Response");
        if (data && data.length) {
          this.OutcomeReportData = data;
          this.PreLoader = false;
        }
      } catch (error) {
        console.error(error);
        this.snackBar = {
          message: error,
          value: true,
          color: "success",
        };
        this.PreLoader = false;
      }

      setTimeout(() => {
        this.PreLoader = false;
      }, 60000);
    },
    async  ExportToExcel(type, fn, dl) 
    {
      // var elt = document.querySelector("#output .pvtTable")
      var elt= this.$refs.outcome_table;
      var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1",cellDates:true, dateNF: "mmm-yy"});
      return dl ?
          XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
          XLSX.writeFile(wb, fn || ('Outcome_Report.' + (type || 'xlsx')));
    },
    setAchieved(i) {
      this.achievedTillText = `Achieved Till - ${i}`;
      return true;
    },
    getValues(item) {
      let clenFiltered = this.cMonthHeaders.filter((x) => x.subHeaders.length);
      let clen = clenFiltered.length;
      let modifiedData = [];

      for (let i = 0; i < clen; i++) {
        let currentItem = item.find((x) => x.Month_Year === clenFiltered[i].label);

        if (currentItem) {
          modifiedData.push(currentItem);
        } else {
          modifiedData.push({
            Planned: "-",
            Achieved: "-",
            Month_Year: clenFiltered[i].text,
          });
        }
      }
      return modifiedData;
    },
    onScroll(e) {
      let { scrollTop, scrollLeft } = e.target;
      scrollLeft > 5 ? (this.isScrollLeft = true) : (this.isScrollLeft = false);
      scrollTop > 5 ? (this.isScrollTop = true) : (this.isScrollTop = false);
    },
    getWidth(selector) {
      return $(selector).outerWidth();
    },
  },
};
</script>

<style scoped lang="scss">
.parentWrapper {
  overflow: hidden !important;
  .tableContainer {
    overflow: auto;
    width: 100%;
    height: 75vh;
  }
  table {
    white-space: nowrap;
    border-collapse: separate;
    border-spacing: 1;
    table-layout: fixed;
    border: 1px solid white;
    .row-hover {
      &:hover {
        background: #eeeeee !important;
        .col-bg {
          background-color: #eeeeee;
        }
        .td-bg {
          background-color: #eeeeee;
        }
      }
    }
    .paddingExtra{
      padding-left: 80px;
      padding-right: 80px;

    }
    .fixed-col {
      position: sticky;
      left: 0px;
      z-index: 4;
    }

    .outcomeName.fixed-col {
      border-right: 1px solid rgb(184, 184, 184);
    }
    // #tbcol-1.fixed-col {
    //   border-right: 1px solid black;
    // }

    td.fixed-col {
      background: white;
    }

    th.fixed-col {
      background: #f5f5f5;
    }

    .fixed-row {
      position: sticky;
      top: 0px;
      z-index: 6;
    }
    .wordBreak{
      white-space: pre-line
    }
  }
}
</style>
