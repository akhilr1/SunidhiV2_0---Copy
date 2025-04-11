<template>
  <v-btn class="success-btn btn-sm" depressed dark height="30">
    {{title}}
  </v-btn>
</template>
<script>
  export default{
  props:['title'],
  }
</script>