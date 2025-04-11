<script>
import { Bar } from 'vue-chartjs'

export default {
  extends: Bar,
  props: ['barData', 'barOptions'],
  mounted () {
    this.renderChart(this.barData,this.barOptions)
  },
  
    
}
</script>