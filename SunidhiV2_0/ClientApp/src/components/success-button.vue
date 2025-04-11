<template>
  <v-btn class="success-btn" depressed dark height="37">
    {{title}}
  </v-btn>
</template>
<script>
  export default{
  props:['title'],
  }
</script>