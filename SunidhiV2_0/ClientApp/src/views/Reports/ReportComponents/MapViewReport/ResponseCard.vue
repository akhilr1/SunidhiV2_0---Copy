<template>
  <v-card class="card_main_wrapper">
    <!-- information card -->
    <v-card flat class="info__card pl-5" color="#EBF5FF">
      <v-icon large class="info__card-icon"> mdi-map-marker-outline </v-icon>
      <v-row>
        <!-- <v-col class="title pt-1 pl-5">
          <v-card-title class="pt-1">Responses</v-card-title>
        </v-col>
        <v-col right>
          <v-card-subtitle class="pb-1">
            {{ ResponseCount}} • Responses
          </v-card-subtitle>
        </v-col> -->
        <v-col class="pa-0 ma-0 ml-5">
          <v-card-title> Responses </v-card-title>
        </v-col>
        <p class="align-self-center mr-10 font-weight-medium blue--text mb-0" v-if="ResponseCount">
          {{ `${ResponseCount} Responses` }}
        </p>
      </v-row>
    </v-card>

    <!-- Response nodes -->
    <v-list>
      <response-node v-for="(item) in responseStack" :data="item" :key="item.syncDBID"/>
    </v-list>
  </v-card>
</template>

<script>
import ResponseNode from './ResponseNode.vue'
import { createNamespacedHelpers } from "vuex";
const { mapGetters, mapActions } = createNamespacedHelpers("MapReport");
export default {
  name: "ResponseCard",
  props: ['searchKey'],
  components:{
    ResponseNode
  },
  data() {
    return {
      responseToggle: false,
    };
  },
  computed: {
    ...mapGetters(["GeoDataList"]),
    responseStack() {
      let data = this.GeoDataList
      if(!data?.length) return []
      if(this.searchKey){
        let filteredList = data.filter(x => {
          return x.Geo_coordinates.includes(this.searchKey)
        })
        return filteredList
      }
      return data
    },
    ResponseCount() {
      if(this.GeoDataList && this.GeoDataList.length > 0) {
        let length = this.GeoDataList.length
        return length
      } 
      return 0
    },
  },
};
</script>

<style scoped>
.card_main_wrapper {
  padding: 10px;
  border-radius: 16px !important;
  border: 2px solid #dadbe6;
}
.info__card {
  border-radius: 8px !important;
}
.info__subcard {
  justify-content: space-around;
}
.info__card-icon {
  position: absolute;
  top: -3px;
  left: -3px;
  font-size: 15px !important;
  padding: 8px;
  color: white;
  border-radius: 50% !important;
  outline: 4px solid white;
  background-color: #399cff;
}
</style>