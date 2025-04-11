<template>
  <v-container class="pb-0" fluid>
    <v-row>
      <v-col sm="12" cols="12" class="pt-1">
        <h1 class="pl-4" style="color: #2D2F40; font-size: 17px; font-weight: bold">
          Survey Overview
        </h1>
      </v-col>
    </v-row>
    <v-row class="pl-4">
      <v-col sm="5" cols="12">
        <v-select
          dense
          label="Mahila kisan Sashaktikaran Pariyojana (MKSP)"
          outlined
          hide-details
          class="mr-2"
        ></v-select>
      </v-col>
      <v-col cols="6" sm="6">
        <button
          type="button"
          class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V20"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
        <span>1 of 6</span> 
        <button
          type="button"
          class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
          id="AutoTest_V21"
          style="
            background-color: rgb(255, 255, 255);
            border-color: rgb(255, 255, 255);
          "
        >
          <span class="v-btn__content"
            ><i
              aria-hidden="true"
              class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
              style="font-size: 25px"
            ></i
          ></span>
        </button>
      </v-col>
    </v-row>
  </v-container>
</template>