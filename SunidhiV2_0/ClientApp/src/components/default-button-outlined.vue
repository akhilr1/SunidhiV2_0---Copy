<template>
  <v-btn height="37" outlined class="outlined-btn-dark">
    {{title}}
  </v-btn>
</template>
<script>
  export default {
  props:['title']
  }
</script>