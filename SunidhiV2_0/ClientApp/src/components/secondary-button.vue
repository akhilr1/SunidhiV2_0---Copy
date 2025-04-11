<template>
  <v-btn class="secondary" height="37" depressed dark >
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