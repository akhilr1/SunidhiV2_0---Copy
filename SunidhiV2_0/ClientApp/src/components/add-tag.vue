<template>
<div>
  <span class="small-text mr-1">
    <v-icon size="12">mdi-tag</v-icon> Tags
  </span>
  <v-chip class="mr-2" v-for="basicTag in basicTags" :key="basicTag.id" v-model="basicTag.id" v-show="!basicTag.id" small dark color="#388E3C" label="" close="" @click:close="(basicTag.id=true)">{{basicTag.name}}</v-chip>
  <!--add tag menu-->
  <v-menu absolute v-model="AddTagPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed fab light color="#E5F2FF">
        <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
      </v-btn>
    </template>
    <!--Tag select pop up-->
    <v-stepper v-model="AddTagSteps" class="elevation-0">
      <v-stepper-items>
        <v-stepper-content step="1" class="pa-0">
          <v-card>
            <v-card-title class="heading-3">
              Add tag
              <v-spacer></v-spacer>
              <v-btn text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddTagPopUp=false">
                <v-icon size="20">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text style="height:300px;" class="v-scrolling-div">
              <v-row>
                <v-col md="12">
                  <v-text-field label="Search Tag" hide-details="" outlined="" dense=""></v-text-field>
                </v-col>
                <v-col md="12" class="py-0">
                  <v-chip class="mr-2" v-for="basicTag in basicTags" :key="basicTag.id" v-model="basicTag.id" v-show="basicTag.id" small="" dark="" color="#388E3C" label="" close="" close-icon="mdi-pencil" @click="(basicTag.id=false)" @click:close="AddTagSteps=3">{{basicTag.name}}</v-chip>
                </v-col>
              </v-row>
            </v-card-text>
            <v-divider></v-divider>
            <v-card-actions>
              <v-row no-gutters="">
                <v-col md="12" align="center">
                  <Success-Button style="width:130px" class="mb-2 center-block" title="Create Tag" @click.native="AddTagSteps=2"></Success-Button>
                </v-col>
              </v-row>
            </v-card-actions>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="2" class="pa-0">
          <v-card>
            <v-card-title class="heading-3">
               <v-btn height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddTagSteps=1">
                <v-icon size="20">mdi-arrow-left</v-icon>
              </v-btn>
              Create tag
              <v-spacer></v-spacer>
              <v-btn text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddTagPopUp=false">
                <v-icon size="20">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text style="height:300px;" class="v-scrolling-div">
              <v-row>
                <v-col md="12">
                  <v-text-field label="Name" hide-details outlined dense></v-text-field>
                </v-col>
                <v-col md="12" class="py-0">
                  <label class="ml-2 pa-0">Select Color</label>
                  <v-color-picker class="ma-2" hide-canvas hide-inputs flat hide-mode-switch="" show-swatches="" disabled="" :swatches="tagSwatches"></v-color-picker>
                </v-col>
              </v-row>
            </v-card-text>
            <v-divider></v-divider>
            <v-card-actions>
              <v-row no-gutters="">
                <v-col md="12" align="center">
                  <Success-Button style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddTagSteps=1"></Success-Button>
                </v-col>
              </v-row>
            </v-card-actions>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="3" class="pa-0">
          <v-card>
            <v-card-title class="heading-3">
              <v-btn height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddTagSteps=1">
                <v-icon size="20">mdi-arrow-left</v-icon>
              </v-btn>
              Edit tag
              <v-spacer></v-spacer>
              <v-btn text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddTagPopUp=false">
                <v-icon size="20">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text style="height:300px;" class="v-scrolling-div">
              <v-row>
                <v-col md="12">
                  <v-text-field label="Name" hide-details="" outlined="" dense=""></v-text-field>
                </v-col>
                <v-col md="12" class="py-0">
                  <label class="ml-2 pa-0">Select Color</label>
                  <v-color-picker class="ma-2" hide-canvas="" hide-inputs="" flat="" hide-mode-switch="" show-swatches="" disabled="" :swatches="tagSwatches"></v-color-picker>
                </v-col>
              </v-row>
            </v-card-text>
            <v-divider></v-divider>
            <v-card-actions>
              <v-row no-gutters="">
                <v-col md="12" align="center">
                  <Success-Button style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddTagSteps=1"></Success-Button>
                </v-col>
              </v-row>
            </v-card-actions>
          </v-card>
        </v-stepper-content>
      </v-stepper-items>

    </v-stepper>

    <!--__________________________-->
  </v-menu>
</div>
</template>
<script>
  import SuccessButton from'@/components/success-button.vue'
  
  export default{
  data(){
  return{

  offset: true,
  closeonClick:false,
  closeOnContentClick:false,

  AddTagPopUp:false,
  AddTagSteps:'',
  basicTag:false,
  basicTags:[
  {name:'Tag1', id:'Tag1', value:false},
  {name:'Tag2', id:'Tag2', value:false},
  ],

  tagSwatches: [
  ['#34C900', '#FFC914',],
  ['#1FC2C8','#0F36F6'],
  ['#EC4554', '#9509FF'],
  ],

  }
  },

  components:{
  'Success-Button':SuccessButton,
  }
  }
</script>