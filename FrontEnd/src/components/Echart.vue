<template>
  <v-card>
    <template v-slot:title>{{ title }} </template>
    <template v-slot:subtitle> {{ subtitle }}</template>
    <div class="w-full h-full">
      <div ref="chart" class="chart" />
    </div>
  </v-card>
</template>

<script>
import * as echarts from 'echarts'

export default {
  props: {
    chartData: {
      type: Object,
      required: true,
    },
    title: {
      type: String,
      default: 'ECharts Example',
    },
    subtitle: {
      type: String,
      default: 'A simple example of ECharts with Vue',
    },
  },
  mounted() {
    this.initChart()
    window.addEventListener('resize', this.resizeChart)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.resizeChart)
  },
  methods: {
    initChart() {
      const chart = echarts.init(this.$refs.chart)
      chart.setOption(this.chartData)
      window.onresize = function () {
        chart.resize()
      }
    },
    resizeChart() {
      const chart = echarts.getInstanceByDom(this.$refs.chart)
      if (chart) {
        chart.resize()
      }
    },
  },
}
</script>

<style>
.chart {
  width: 100%;
  height: 400px; /* Adjust as needed */
}
</style>
