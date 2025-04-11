<script>
 import { Line } from 'vue-chartjs'

export default {
  extends: Line,
  props: ['linedata', 'lineOptions'],
  mounted () {
    this.renderChart(this.linedata, this.lineOptions)
  },
}
</script>