<template>
  <v-btn class="success-btn" depressed dark height="39" width="130">
    {{title}}
  </v-btn>
</template>
<script>
  export default{
  props:['title'],
  }
</script>