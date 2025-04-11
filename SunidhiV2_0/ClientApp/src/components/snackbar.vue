<template>
  <v-snackbar v-model="Model">
    {{ title }}
  </v-snackbar>
</template>
<script>
  props:[
  'title', 'Model'
  ]
</script>