<template>
  <v-btn max-width="29" min-width="40" height="37" depressed class="outlined-btn-dark" color="#EEEEEE">
    <v-icon color="#606060">{{icon}}</v-icon>
  </v-btn>
</template>
<script>
  export default{
  props:[
  'icon'
  ],
  }
</script>