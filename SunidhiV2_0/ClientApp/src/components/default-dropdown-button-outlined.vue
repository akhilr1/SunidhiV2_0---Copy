<template>
  <v-btn v-on="on"  height="37" outlined class="outlined-btn-dark">
    {{title}}
    <v-icon>
      mdi-chevron-down
    </v-icon>
  </v-btn>
</template>
<script>
  export default {
  props:['title']
  }
</script>