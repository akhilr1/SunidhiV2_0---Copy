<template>
  <v-btn class="primary-btn-light" depressed dark >
    {{title}}
  </v-btn>
</template>
<script>
  export default{
    props:[
      'title'
    ]
  }
</script>