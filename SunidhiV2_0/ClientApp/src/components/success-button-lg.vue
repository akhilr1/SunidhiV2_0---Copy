<template>
  <v-btn class="success-btn btn-lg" depressed dark height="50">
    {{title}}
  </v-btn>
</template>
<script>
  export default{
  props:['title'],
  }
</script>