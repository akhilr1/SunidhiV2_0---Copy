<template>
  <v-container fluid>
    <pre-loader v-show="preloader"></pre-loader>
    <v-card width="100%" class="parentWrapper">
        <v-card-title>
            <v-btn class="mr-3" large icon @click="$router.back()" color="primary"><v-icon>mdi-chevron-left</v-icon></v-btn>
            Activities & Inputs Table
            <v-spacer></v-spacer>
            <v-btn depressed color="green" dark @click="ExportToExcel('xlsx')">
                <v-icon dense class="px-2">mdi-export-variant</v-icon> Export
            </v-btn>
        </v-card-title>
      <v-card-text>
        <div class="tableContainer v-scrolling-div" @scroll="onScroll">
          <table ref="output_table" class="v-data-table">
            <thead class="v-data-table-header fixed-row">
              <tr>
                <template v-for="(head, hid) in cHeaders">
                  <th
                    v-if="!head.hasSubHeaders"
                    rowspan="2"
                    :id="head.value"
                    :class="head.sticky ? { 'fixed-col col-bg': isScrollLeft } : ''"
                    :style="head.value === 'ActivityName' &&
                         `left: ${getWidth('ActivityCode')}px;padding-left:120px;padding-right:120px;`"


                  >
                    {{ head.text }}
                  </th>
                  </template>
                  <th colspan="3" class="text-center">Total</th>
                  <template v-for="(head, hid) in cHeaders">
                  <th
                    class="text-center"
                    v-if="head.hasSubHeaders"
                    rowspan="1"
                    colspan="4"
                    :key="hid"
                  >
                    {{ head.text }}
                  </th>
                </template>
              </tr>
              <tr>
                <th class="text-center">Phy Achieved</th>
                  <th class="text-center">Expenditure</th>
                  <th class="text-center">Remaining Budget</th>
                <template v-for="(head, hid) in cHeaders">
                  <template v-if="head.hasSubHeaders">
                    <th :key="hid" class="text-center">Units</th>
                    <th class="text-center">Budget</th>
                    <th class="text-center">Phy Achieved</th>
                    <th class="text-center">Expenditure</th>
                  </template>
                </template>

              </tr>
            </thead>

            <tbody>
              <template v-for="(elem, ids) in flatList">
                <tr class="row-hover" :key="ids">
                  <td class="text-start" :class="{ 'fixed-col td-bg': isScrollLeft }">
                    {{ elem.ActivityCode }}
                  </td>
                  <td
                    class="text-start wordBreak outputName"
                    style="max-width: 600px"
                    :class="{ 'fixed-col td-bg': isScrollLeft }"
                    :style="isScrollLeft ? `left: ${getWidth(`ActivityCode`)}px;` : ''"
                  >
                    {{ elem.ActivityName }}
                  </td>
                  <template
                    v-if="
                      elem.lstOutputMonthlyReport && elem.lstOutputMonthlyReport.length
                    "
                  >
                    <td class="text-center">{{ elem.NumberOfUnits }}</td>
                    <td class="text-center">{{ elem.UnitCost }}</td>
                    <td class="text-center">{{ elem.TotalCost }}</td>
                    <td class="text-center">
                      <strong>{{ elem.TotalPhysicalAchieved }}</strong>
                    </td>
                    <td class="text-center">
                      <strong>{{ elem.TotalExpenditure }}</strong>
                    </td>
                    <td class="text-center">
                      <strong>{{ elem.TotalCost - elem.TotalExpenditure }}</strong>
                    </td>
                    <template v-for="(x, i) in getValues(elem.lstOutputMonthlyReport)">
                      <td class="text-center" :key="i">{{ x.Units }}</td>
                      <td class="text-center">{{ x.Budget }}</td>
                      <td class="text-center">{{ x.PhysicalAchieved }}</td>
                      <td class="text-center">{{ x.Expenditure }}</td>
                    </template>

                  </template>
                </tr>
              </template>
            </tbody>
          </table>
        </div>
      </v-card-text>
    </v-card>
    <v-snackbar v-model="snackbar.value" :color="snackbar.color">
      {{ snackbar.message }}
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
      headers: [
        {
          text: "SL",
          value: "ActivityCode",
          sticky: true,
        },
        {
          text: "Expense Heads",
          value: "ActivityName",
          width: "650",
          sticky: true,
        },
        {
          text: "No. of Units",
          value: "NumberOfUnits",
        },
        {
          text: "Unit Cost",
          value: "UnitCost",
        },
        {
          text: "Total Cost",
          value: "TotalCost",
        },
      ],
      listItems: [],
      preloader: false,
      snackbar: {
        message: "",
        value: false,
        color: "",
      },
      isScrollLeft: false,
      isScrollTop: false,
    };
  },
  computed: {
    cHeaders() {
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

      let data = this.headers
        .sort((a, b) => {
          if (a.hasSubHeaders && b.hasSubHeaders) {
            let dA = convertToDate(a.text);
            let dB = convertToDate(b.text);
            return dA - dB;
          }
        })
        .map((x) => JSON.stringify(x));
      let xdata = new Set(data);
      let ydata = [];
      for (let val of xdata) {
        ydata.push(JSON.parse(val));
      }
      return ydata;
    },
    flatList() {
      let headers = this.headers;
      function flatten(items) {
        return items.flatMap((elem) => {
          if (elem.lstOutputReport && elem.lstOutputReport.length) {
            return [{ ...elem, hasOutput: false }, ...flatten(elem.lstOutputReport)];
          } else if (elem.lstOutputMonthlyReport) {
            elem.lstOutputMonthlyReport.forEach((month) => {
              if (!headers.find((z) => z.text == month.Month_Year)) {
                headers.push({
                  text: month.Month_Year,
                  value: month.Month_Year,
                  hasSubHeaders: true,
                });
              }
            });
            return [{ ...elem, hasOutput: true }];
          } else return [{ ...elem, hasOutput: false }];
        });
      }
      return flatten(this.listItems);
    },
  },
  async mounted() {
    await importscript();
    await this.getOutputReportData();
  },
  methods: {
    async getOutputReportData() {
      this.preloader = true;
      try {
        let { data } = await objManageProjectActivity.GetOutputReports(this.ProjectId);
        if (!data || !data.length) {
          throw new Error("Invalid Response");
        }
        if (data && data.length) {
          this.listItems = data;
          this.preloader = false;
        }
      } catch (error) {
        console.log(error);
        this.snackbar = {
          message: error,
          color: "error",
          value: true,
        };
        this.preloader = false;
      }

      setTimeout(() => {
        if (this.preloader) {
          this.snackbar = {
            message: "Request Timed out",
            color: "error",
            value: true,
          };
        }
        this.preloader = false;
        return;
      }, 60000);
    },
    async  ExportToExcel(type, fn, dl) 
    {
      var elt= this.$refs.output_table;
      var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1",cellDates:true, dateNF: "mmm-yy"});
      return dl ?
          XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
          XLSX.writeFile(wb, fn || ('Output_Report.' + (type || 'xlsx')));
    },
    getValues(item) {
      let clenFiltered = this.cHeaders.filter((x) => x.hasSubHeaders);
      let clen = clenFiltered.length;
      let modifiedData = [];

      for (let i = 0; i < clen; i++) {
        let currentItem = item.find((x) => x.Month_Year === clenFiltered[i].text);

        if (currentItem) {
          modifiedData.push(currentItem);
        } else {
          modifiedData.push({
            Units: "-",
            PhysicalAchieved: "-",            
            Expenditure: "-",
            Budget: "-",
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
      // must be a id selector
      console.log(selector, $("#" + selector));
      return $("#" + selector).outerWidth();
    },
  },
};
</script>

<style scoped lang="scss">
/* .rowHover:hover{
  background: #eeeeee;
}

td, th {
  color: black;
} */

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

    .fixed-col {
      position: sticky;
      left: 0px;
    }

    .outputName.fixed-col {
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
