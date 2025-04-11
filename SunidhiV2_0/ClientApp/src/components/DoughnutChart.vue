<script>
 import { Doughnut } from 'vue-chartjs'

export default {
  extends: Doughnut,
  props: ['doughnutChartdata', 'doughnutChartOptions'],
  mounted () {
    this.renderChart(this.doughnutChartdata, this.doughnutChartOptions)
  },
}
</script>