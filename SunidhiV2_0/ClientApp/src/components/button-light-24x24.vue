<template>
  <v-btn max-height="24" max-width="24" min-width="24" min-height="24" depressed light color="#E5F2FF">
      <v-icon size="15" light color="#707070">{{icon}}</v-icon>
    </v-btn>
</template>
<script>
  export default{
  props:[
    'icon'
  ],
  }
</script>