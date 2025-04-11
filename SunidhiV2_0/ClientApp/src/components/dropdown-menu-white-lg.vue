<template>
  <v-menu offset-y transition="scroll-y-transition" close-on-content-click internal-activator>
  <template v-slot:activator="{ on }">
    <v-btn color="#FFFFFF" class="elevation-1"  light height="54" width="300" v-on="on">
      Notification Preferences
      <v-icon>mdi-chevron-down</v-icon>
    </v-btn>
  </template>
  <v-list color="white" light width="300" class="action-button-list">
    <v-list-item link @click="">
      <v-list-item-content>Text</v-list-item-content>
    </v-list-item>
    <v-list-item link @click="">
      <v-list-item-content>Text2</v-list-item-content>
    </v-list-item>
  </v-list>
</v-menu>
</template>
