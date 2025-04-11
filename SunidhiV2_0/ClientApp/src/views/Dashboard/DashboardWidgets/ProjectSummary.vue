<template>
  <v-card style="height: 490px" elevation="1">
    <v-row class="pt-1">
      <v-col sm="6" cols="6" class="pt-5">
        <h1 class="header-font page-head-dark font-weight-medium pl-4">
          Project Summary
        </h1>
      </v-col>
      <!-- <v-col sm="6" cols="6">
        <v-select
          dense
          label="Teach Each"
          outlined
          hide-details
          class="mr-2"
        ></v-select>
      </v-col> -->
    </v-row>
    <v-row>
      <v-col sm="12" class="pl-7">
        <span style="font-weight: bold; font-size: 35px">{{projectTotal}}</span>
        <span class="pl-3">Total Projects</span>
      </v-col>
    </v-row>
    <v-row class="pl-4 pr-2">
      <v-col sm="12">
        <div class="d-flex">
          
          <div
            :style="{
              'background-color': '#20c997',
              'border-radius':'5px 0px 0px 5px',
              'height':'35px',
              'width': completedPercent+'%'}
            "
          ></div>
          <div
             :style="{
              'background-color': '#f1c100',
              'height':'35px',
              'width': deployedPercent+'%'}
            "
          ></div>
          <div
            :style="{
              'background-color': '#e6217b',
              'height':'35px',
              'width': archivedPercent+'%'}
            "
          ></div>
          <div
            :style="{
              'background-color': '#eeeeee',
              'border-radius':'0px 5px 5px 0px',
              'border':'1px',
              'height':'35px',
              'width': draftPercent+'%'}
            "
          ></div>
        </div>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm='6' class="d-flex pl-5">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#20c997"> </v-avatar>
        <span style="margin-top: 2px;">Completed</span>
      </v-col>
      <v-col sm='6' class="d-flex justify-end pr-5">
         <span style="font-size:16px;" class="font-weight-bold text--secondary mr-2">{{completed}}</span>
         <span style="margin-top: 3px;font-size:13px;">Tasks</span>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm='6' class="d-flex pl-5">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#f1c100"> </v-avatar>
        <span style="margin-top: 2px;">Deployed</span>
      </v-col>
      <v-col sm='6' class="d-flex justify-end pr-5">
         <span style="font-size:16px;" class="font-weight-bold text--secondary mr-2">{{deployed}}</span>
         <span style="margin-top: 3px;font-size:13px;">Tasks</span>
      </v-col>
    </v-row>
    <v-row>
      <v-col sm='6' class="d-flex pl-5">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#e6217b"> </v-avatar>
        <span style="margin-top: 2px;">Archived</span>
      </v-col>
      <v-col sm='6' class="d-flex justify-end pr-5">
         <span style="font-size:16px;" class="font-weight-bold text--secondary mr-2">{{archived}}</span>
         <span style="margin-top: 3px;font-size:13px;">Tasks</span>
      </v-col>
    </v-row>
        <v-row>
      <v-col sm='6' class="d-flex pl-5">
        <v-avatar class="mt-1 mx-2" size="15" tile color="#eeeeee"> </v-avatar>
        <span style="margin-top: 2px;">Draft</span>
      </v-col>
      <v-col sm='6' class="d-flex justify-end pr-5">
         <span style="font-size:16px;" class="font-weight-bold text--secondary mr-2">{{draft}}</span>
         <span style="margin-top: 3px;font-size:13px;">Tasks</span>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
export default {
  props: ['dashdata'],
  data(){
    return{
      total:0,
      projectTotal:0,
      completed:0,
      deployed:0,
      archived:0,
      draft:0,
      completedPercent: null,
      deployedPercent: null,
      archivedPercent: null,
      draftPercent: null,
    }
  },
  mounted(){
    this.setData()
  },
  methods: {
    setData() {
        this.projectTotal = this.dashdata.ProjectCunt;
        this.completed = this.dashdata.CompCount
        this.deployed = this.dashdata.DeployCount
        this.archived = this.dashdata.ArchivedCount
        this.draft = this.dashdata.DraftCount
        this.total = (this.completed + this.deployed + this.archived + this.draft)
        this.completedPercent = this.completed == 0 ? 2 : (this.completed/this.total)*100
        this.deployedPercent = this.deployed == 0 ? 2 : (this.deployed / this.total)*100
        this.archivedPercent = this.archived == 0 ? 2 : (this.archived / this.total)*100
        this.draftPercent = this.draft == 0 ? 2 : (this.draft / this.total) *100
    }
  }
};
</script>