<template>
    <v-card>
      <!--Pre-loader-->
      <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
      <!--________________________-->
      <v-container fluid style="padding-left:2vw;padding-right:2vw;">
          <!--Workspace heading -->
          <v-row>
              <v-col cols="12" xs="12" sm="12" md="6">
                  <h5 class="page-head">
                      <v-avatar color="white" size="25" class="elevation-1 mr-2">
                          <v-icon size="15" color="primary">mdi-folder-key-outline</v-icon>
                      </v-avatar>My Workspace
                  </h5>
              </v-col>
              <v-col cols="12" xs="12" sm="12" md="6">
                  
                      

                      <v-btn id="AutoTest_WS1" height="39" color="success-btn" dark class="elevation-0 mr-4" @click="AddWorkspaceDialog=true">Create Workspace</v-btn>
                      <v-dialog v-model="AddWorkspaceDialog" width="800" persistent>
                          <v-card>
                              <v-card-title primary-title class="page-head pb-0">
                                  <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                      <v-icon size="20" color="primary">mdi-account-network-outline</v-icon>
                                  </v-avatar>
                                  Create Workspace
                                  <v-spacer></v-spacer>
                                  <v-btn id="AutoTest_WS2" text @click="CloseWSDialog()">
                                      <v-icon color="#707070" size="15">mdi-close</v-icon>
                                  </v-btn>
                              </v-card-title>
                              <v-card-text class="pt-0 pb-1">
                                  <v-container fluid class="pt-0">
                                      <v-row no-gutters>
                                          <v-col cols="12" md="12" class="pt-4">
                                              <h4 class="text-center heading-3">Please enter the basic details to create new workspace</h4>
                                          </v-col>

                                          <v-col cols="12" md="12" class="text-center">
                                              <v-avatar size="90" class="elevation-1 mt-2" light color="white">
                                                  <v-img v-bind:src="profileeditImage" height="90" width="90" v-if="profileeditImage.length>0"></v-img>
                                                  <v-icon color="primary" size="50" v-else>mdi-account-network-outline</v-icon>
                                                  <div class="dp-edit-btn mt-12">
                                                      <input class="upload" type="file" id="file" hide-details accept="image/*" @change="previewImage" />
                                                  </div>
                                              </v-avatar>
                                          </v-col>


                                          <v-col cols="12" xs="12" sm="12" md="12">
                                              <v-row :justify="justifyCenter">
                                                  <v-col md="7">
                                                      <v-text-field outlined dense label="Workspace Name" type="text" v-model="deskName" :rules="alphanumericRules">
                                                      </v-text-field>
                                                      <v-textarea rows="3" outlined dense label="Enter Description" type="text" v-model="deskDescription">
                                                      </v-textarea>
                                                      <v-row>
                                                          <v-col md="12" class="pt-0">
                                                              <v-select :items="wsVisibilityItems"
                                                                        label="Workspace Visibility"
                                                                        item-value="value"
                                                                        item-text="wsVisibility"
                                                                        outlined
                                                                        solo
                                                                        dense
                                                                        flat
                                                                        required
                                                                        v-model="wVisibility" hide-details></v-select>

                                                          </v-col>

                                                          <!-- Tags-->
                                                          <v-col md="12" class="text-left d-inline-flex pt-2">
                                                              <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-2">Tags</span>
                                                              <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                  <template v-slot:activator="{ on }">
                                                                      <v-btn id="AutoTest_WS3" v-on="on" @click="fnInitializeSteper()" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" class="mr-1">
                                                                          <v-icon size="15">mdi-plus</v-icon>
                                                                      </v-btn>
                                                                  </template>
                                                                  <v-card>
                                                                      <v-row>
                                                                          <v-col cols="12" md="12" class="pt-0">
                                                                              <!-- Header Text  -->
                                                                              <v-row class="pt-1 px-1">
                                                                                  <v-col cols="12" md="12" class="d-inline-flex">
                                                                                      <v-btn id="AutoTest_WS4" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                          <v-icon size="15">mdi-chevron-left</v-icon>
                                                                                      </v-btn>
                                                                                      <v-spacer></v-spacer>
                                                                                      <h3 class="text-left page-head pt-2">
                                                                                          {{TagHeaderName}}
                                                                                      </h3>
                                                                                      <v-spacer></v-spacer>
                                                                                      <v-btn id="AutoTest_WS5" text @click="fnInitializeSteper(),NewTagMenu = false">
                                                                                          <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                                      </v-btn>
                                                                                  </v-col>
                                                                              </v-row>
                                                                              <v-divider></v-divider>
                                                                              <v-stepper v-model="AllTagListSteps" class="elevation-0 px-4">
                                                                                  <v-stepper-items>

                                                                                      <!-- Create New Tag Option when there is no Tags are -->
                                                                                      <v-stepper-content step="1" class="pa-0">
                                                                                          <v-row>
                                                                                              <v-col class="pb-1 ml-2">
                                                                                                  <v-text-field outlined type="text" dense
                                                                                                                label="Name" v-model="NewTagName" hide-details>
                                                                                                  </v-text-field>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                          <v-row class="pt-0">
                                                                                              <v-col class="pb-1">
                                                                                                  <span class="ml-3">Select Color</span>
                                                                                                  <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                                  :swatches="swatches"
                                                                                                                  hide-canvas
                                                                                                                  hide-inputs
                                                                                                                  hide-mode-switch
                                                                                                                  show-swatches
                                                                                                                  v-model="color"
                                                                                                                  @input="">
                                                                                                  </v-color-picker>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                          <v-row v-show="NewTagName.length>0">
                                                                                              <v-col cols="12" md="12" class="text-center">
                                                                                                  <v-btn id="AutoTest_WS6" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                      </v-stepper-content>

                                                                                      <!-- Select from existing Tags-->
                                                                                      <v-stepper-content step="2" class="pa-0">

                                                                                          <v-row style="max-height:40vh">
                                                                                              <v-col class="pb-1">
                                                                                                  <!--<p class="mb-2">Search Tags</p>-->
                                                                                                  <v-text-field outlined="" type="text" dense="" label="Search tag here" hide-details class="mb-3" v-model="SearchTagDetails">
                                                                                                  </v-text-field>
                                                                                                  <div style="height: 18vh;" class="v-scrolling-div" v-show="this.filterAllTagList.length>0">
                                                                                                      <v-chip-group dark multiple column style="max-width: 220px">
                                                                                                          <div v-for="index in filterAllTagList" :key="index.TagID" class="d-inline-flex">

                                                                                                              <v-chip id="AutoTest_WS7" style="width: 180px" medium :color="index.TagColor" :key="index.TagID" active-class="active-success-check"
                                                                                                                      label @click.native="fnAddToSelectedTag(index),NewTagMenu = true">
                                                                                                                  <!--close @click:close="fnRemoveFromAllTagList(index)"-->
                                                                                                                  <v-icon v-if="index.IsChecked" size="20" left :key="index.TagID">mdi-check</v-icon>
                                                                                                                  <span class="text-limit text-center">{{ index.TagName }}</span>
                                                                                                              </v-chip>
                                                                                                              <v-icon id="AutoTest_WS8" right color="primary" size="16" @click="fnEditTagDetails(index)" class="mx-2">mdi-pencil-outline</v-icon>
                                                                                                          </div>
                                                                                                      </v-chip-group>
                                                                                                  </div>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                          <v-row>
                                                                                              <v-col cols="12" md="12" class="text-center mt-3">
                                                                                                  <v-btn id="AutoTest_WS9" class="text-limit" color="success-btn" @click="searchTag()" dark max-width="200">
                                                                                                      Create New
                                                                                                      <span v-if="this.SearchTagDetails.length>0 && this.SearchTagDetails.length<=6 && IsSearchTagExist == false" class="pl-1 text-center">"{{this.SearchTagDetails}}"</span>
                                                                                                      <span class="pl-1 text-center" v-else v-show="this.SearchTagDetails.length>6 && IsSearchTagExist == false">"{{ this.SearchTagDetails.substring(0,6)+".." }}"</span>
                                                                                                  </v-btn>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                      </v-stepper-content>

                                                                                      <!-- Edit Tag-->
                                                                                      <v-stepper-content step="3" class="pa-0">
                                                                                          <v-row>
                                                                                              <v-col class="pb-1 ml-2">
                                                                                                  <v-text-field outlined type="text" dense
                                                                                                                v-model="EditTagName" hide-details>
                                                                                                  </v-text-field>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                          <v-row class="pt-0">
                                                                                              <v-col class="pb-1">
                                                                                                  <span class="ml-3">Edit Color</span>
                                                                                                  <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                                  :swatches="swatches"
                                                                                                                  hide-canvas
                                                                                                                  hide-inputs
                                                                                                                  hide-mode-switch
                                                                                                                  show-swatches
                                                                                                                  v-model="color"
                                                                                                                  @input="">
                                                                                                  </v-color-picker>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                          <v-row>
                                                                                              <v-col cols="6" md="6" class="text-center">
                                                                                                  <v-btn id="AutoTest_WS10" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                                              </v-col>
                                                                                              <v-col cols="6" md="6" class="text-center">
                                                                                                  <v-btn id="" depressed color="red" dark @click="fnRemoveFromAllTagList()" v-bind:disabled="EditTagName.length== 0">Delete</v-btn>
                                                                                              </v-col>
                                                                                          </v-row>
                                                                                      </v-stepper-content>

                                                                                  </v-stepper-items>
                                                                              </v-stepper>

                                                                          </v-col>
                                                                      </v-row>
                                                                  </v-card>
                                                              </v-menu>
                                                              <div v-if="!allTags">
                                                                  <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                                                  <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1" @click="allTags=true">{{SelectedTag[0].TagName}}</v-chip>
                                                                  <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1" @click="allTags=true">{{SelectedTag[1].TagName}}</v-chip>
                                                                  <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1" @click="allTags=true">+{{SelectedTag.length-2}}<span class="px-1">more</span></v-chip>
                                                              </div>
                                                              <v-chip v-if="allTags == true" label small class="tags ml-1" @click="allTags=false">less</v-chip>
                                                              <!--<v-btn id="AutoTest_WS11" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                        </v-btn>
                                                        <v-btn id="AutoTest_WS12" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                        </v-btn>-->
                                                          </v-col>
                                                      </v-row>
                                                      <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                          <v-col md="12" class="mb-0 pt-0">
                                                              <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                                  <v-chip id="AutoTest_WS13" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                                      {{TagItem.TagName}}
                                                                  </v-chip>
                                                              </v-chip-group>
                                                          </v-col>
                                                      </v-row>
                                                  </v-col>
                                              </v-row>
                                          </v-col>
                                          <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-4">
                                              <Secondary-Button id="AutoTest_WS15" title="Create" class="btn-122x36 mr-4" @click.native="fnCreateWorkspace()" v-bind:disabled="deskName.length == 0 || deskName.trim() == ''|| wVisibility==null "></Secondary-Button>
                                              <Default-Button-Outlined id="AutoTest_WS14" class="btn-122x36 mr-4" title="Cancel" @click.native="CloseWSDialog()"></Default-Button-Outlined>
                                          </v-col>

                                      </v-row>
                                  </v-container>
                              </v-card-text>
                          </v-card>
                      </v-dialog>

                      <!--Bulk Action -->
                      <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="BulkActionMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn v-on="on" height="39" outlined class="outlined-btn-dark mr-4">
                                  Bulk Actions
                                  <v-icon>
                                      mdi-chevron-down
                                  </v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                              <v-list-item id="AutoTest_WS16" @click.stop="fnArchiveWorkspace()">
                                  <v-list-item-icon>
                                      <v-icon>mdi-plus-circle-outline</v-icon>
                                  </v-list-item-icon>
                                  <v-list-item-content>
                                      Archive
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS17" @click.stop="fnRemoveWorkspace()">
                                  <v-list-item-icon>
                                      <v-icon>mdi-trash-can-outline</v-icon>
                                  </v-list-item-icon>
                                  <v-list-item-content>
                                      Remove
                                  </v-list-item-content>
                              </v-list-item>
                          </v-list>
                      </v-menu>

                      <!--Filter button-->
                      <v-menu :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick" offset-y transition="scroll-y-transition" v-model="filterMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn v-on="on" height="39" outlined class="outlined-btn-dark mr-4" @click="filterMenu = !filterMenu">
                                  <v-icon>mdi-filter-outline</v-icon>
                                  Filters
                              </v-btn>
                          </template>
                          <!--<v-list color="white" light="" width="auto"  max-width="250" min-width="50" class="action-button-list pa-2">
                        <p class="title">Filter by :</p>
                        <p class="sub-title">Privacy</p>
                        <v-checkbox input-value="true" v-model="checkboxPublic" label="Public" color="success-check-box"></v-checkbox>
                        <v-checkbox input-value="true" v-model="checkboxPrivate" label="Private" color="success-check-box"></v-checkbox>
                        <p class="sub-title">Tags</p>
                        <div style="height: 18vh;" class="v-scrolling-div">
                            <div v-for="(WSTagitem,index) in wsTagColorItems" v-if="WSTagitem.TagID>0">
                                <v-checkbox input-value="WSTagitem.TagID" v-model="checkboxTags[index]" :label="WSTagitem.TagName" color="success-check-box"></v-checkbox>
                            </div>
                        </div>
                        <v-btn id="AutoTest_WS18" height="32" block color="success-btn" dark class="elevation-0 mr-2" @click="fnClickFilterDone()">
                            Done
                        </v-btn>
                    </v-list>-->
                      </v-menu>

                      <!--Sort by-->
                      <v-menu offset-y transition="scroll-y-transition" v-model="SortMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn v-on="on" max-width="29" min-width="40" height="39" outlined="" class="outlined-btn-dark mr-3">
                                  <v-icon>mdi-tune-vertical</v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                              <v-list-item>
                                  <v-list-item-content>
                                      Sort by :
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS19" @click.stop="fnWorkspaceSort('Name-Ascending')">
                                  <v-list-item-content>
                                      Name-Ascending
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS20" @click.stop="fnWorkspaceSort('Name-Descending')">
                                  <v-list-item-content>
                                      Name-Descending
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS21" @click.stop="fnWorkspaceSort('Recently Added')">
                                  <v-list-item-content>
                                      Recently Added
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS22" @click.stop="fnWorkspaceSort('Most Viewed')">
                                  <v-list-item-content>
                                      Most Viewed
                                  </v-list-item-content>
                              </v-list-item>
                          </v-list>
                      </v-menu>

                  
              </v-col>
          </v-row>

          <!--Filter dropdowns-->
          <v-row v-if="filterMenu">
              <v-col cols="12" xs="12" sm="12" md="12">
                  <div class="d-inline-flex">
                      <h4 class="text-left page-head mr-2">Filters</h4>

                      <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="privacyMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                  Privacy
                                  <v-icon class="ml-2">
                                      mdi-chevron-down
                                  </v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                              <p class="sub-title">Filter by : Privacy</p>
                              <v-checkbox input-value="true" v-model="checkboxPublic" label="Public" color="success-check-box"></v-checkbox>
                              <v-checkbox input-value="true" v-model="checkboxPrivate" label="Private" color="success-check-box"></v-checkbox>
                              <v-divider></v-divider>
                              <v-btn id="AutoTest_WS18" height="32" block color="success-btn" dark class="elevation-0 mr-2" @click="fnClickFilterDone(),privacyMenu = false">
                                  Done
                              </v-btn>
                          </v-list>
                      </v-menu>


                      <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="tagDropdownMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                  Tags
                                  <v-icon class="ml-2">
                                      mdi-chevron-down
                                  </v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" class="action-button-list pa-2">
                              <p class="sub-title">Filter by: Tags</p>
                              <div style="height: 18vh;" class="v-scrolling-div">
                                  <v-list-tile v-for="(WSTagitem,index) in wsTagColorItems" v-if="WSTagitem.TagID>0">
                                      <v-checkbox input-value="WSTagitem.TagID" v-model="checkboxTags[index]" color="success-check-box" :label="[`${WSTagitem.TagName.length}`<=8 ? `${WSTagitem.TagName}` : `${WSTagitem.TagName.substring(0,8)+'..'}` ]">
                                      </v-checkbox>
                                  </v-list-tile>
                              </div>
                              <div align="center" justify="space-around">
                                  <v-btn id="AutoTest_WS18" color="success-btn" dark class="elevation-0 mt-2 btn-122x36" @click="fnClickFilterDone(),tagDropdownMenu=false">
                                      Done
                                  </v-btn>
                              </div>
                          </v-list>
                      </v-menu>
                  </div>
              </v-col>
          </v-row>

          <!-- Filter chips-->
          <v-row no-gutters>
              <v-col v-if="filterChipsList.length>0" cols="12" xs="12" sm="12" md="12">
                  <div class="chip-container mb-0">
                      <v-chip-group multiple column active-class="primary--text">
                          <v-chip id="AutoTest_WS25" v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="ma-2" @click:close="fnChipClose(item)" medium label>
                              <v-icon small left @click="fnChipClose(item)">mdi-close</v-icon>{{ item.ChipName }}
                          </v-chip>
                      </v-chip-group>
                  </div>
              </v-col>
          </v-row>

          <!--Workspace cards-->
          <v-row v-if="filterWorkspaceList.length>0">
              <v-col xs="12" sm="12" md="3" v-for="(WSitem,index) in filterWorkspaceList" :key="WSitem.WorkspaceID">
                  <v-card class="workspace-card" flat>
                      <v-row no-gutters>
                          <v-col md="12" class="text-right ml-3">
                              <span v-if="WSitem.WSIsPrivate" class="privacy pt-2 text-right mr-0">
                                  <v-icon size="12">mdi-lock</v-icon> Private
                              </span>
                              <span v-else class="privacy text-right pt-2 mr-0">
                                  <v-icon size="12">mdi-earth</v-icon>
                                  Public
                              </span>
                              <v-checkbox dense hide-details style="float:right" color="success-check-box"
                                          class="pt-0 mt-0 ml-2 mr-0" input-value="true" v-model="checkboxWorkspace[index]" label=""></v-checkbox>
                          </v-col>

                      </v-row>
                      <v-row>
                          <v-col md="12" class="d-inline-flex pt-0">
                              <div style="margin-top:-20px">
                                  <v-avatar size="55" tile class="elevation-1 base-border-radius" light :color="`${getcolor(WSitem.WSName)}`">
                                      <v-img v-bind:src="imageUrl + WSitem.WorkSpaceImage" height="55" width="55" class="ma-4" v-if="WSitem.WorkSpaceImage.length>0"></v-img>
                                      <span class="headline white--text" v-else>{{WSitem.WSName == null ? "" : WSitem.WSName.substring(0, 1).toUpperCase()}}</span>
                                  </v-avatar>
                              </div>
                              <div class="title px-2 text-limit" style="margin-top:-6px">
                                  {{ WSitem.WSName }}
                              </div>
                          </v-col>
                      </v-row>
                      <!--Tags-->
                      <v-row>
                          <v-col class="py-0">
                              <span class="small-text">
                                  <v-icon size="12">mdi-tag-multiple</v-icon>
                                  Tags
                              </span>
                              <v-chip dark v-if="WSitem.WSTags.length>=1" :color="WSitem.WSTags[0].TagColor" label small class="tags">
                                  <span v-if="WSitem.WSTags[0].TagName.length>0 && WSitem.WSTags[0].TagName.length<=7">{{WSitem.WSTags[0].TagName}}</span>
                                  <span v-show="WSitem.WSTags[0].TagName.length>7">{{WSitem.WSTags[0].TagName.substring(0,7)+".."}}</span>
                              </v-chip>
                              <v-chip dark v-if="WSitem.WSTags.length>=2" :color="WSitem.WSTags[1].TagColor" label small class="tags">
                                  <span v-if="WSitem.WSTags[1].TagName.length>0 && WSitem.WSTags[1].TagName.length<=7">{{WSitem.WSTags[1].TagName}}</span>
                                  <span v-show="WSitem.WSTags[1].TagName.length>7">{{WSitem.WSTags[1].TagName.substring(0,7)+".."}}</span>
                              </v-chip>
                              <v-chip dark v-if="WSitem.WSTags.length>=3" label small class="tags tag-count">+{{WSitem.WSTags.length-2}}</v-chip>
                          </v-col>
                      </v-row>
                      <!--Team-->
                      <v-row>
                          <v-col class="py-3">
                              <span class="small-text mr-1">
                                  <v-icon size="12">mdi-account</v-icon> Team
                              </span>
                              <v-avatar size="27" class="team-avatar" v-for="(usr,index) in WSitem.WSUsers.slice(0, 3)" :key="index" :color="`${getcolor(usr.Name)}`">
                                  <v-img v-bind:src="imageUrl + usr.ProfileImage" alt="User DP" height="27" width="27" class="ma-4" v-if="fnCheckProfieImage(usr)"></v-img>
                                  <span class="white--text" v-else>{{usr.Name == null ? "" : usr.Name.substring(0, 1).toUpperCase()}}</span>
                              </v-avatar>
                              <v-avatar v-if="WSitem.WSUsers.length>=4" size="27" class="team-count">
                                  {{WSitem.WSUsers.length-2}}
                              </v-avatar>
                          </v-col>
                      </v-row>
                      <!--buttons-->
                      <v-row>
                          <v-col sm="6">
                              <v-btn name="btnWorkspaceEnter" id="AutoTest_WS23" color="success-btn enter-btn" height="30" depressed block dark @click="fnEnter(WSitem.WorkspaceID)">
                                  <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                  Enter
                              </v-btn>
                          </v-col>
                          <v-col sm="6">
                              <v-btn color="normal" depressed class="manage-btn text--default" height="30" block link :to="{ name: 'WorkspaceManage' , query: { WID: WSitem.WorkspaceID,Shared : false } }">
                                  <v-icon size="20" class="mr-2">mdi-settings</v-icon>
                                  Manage
                              </v-btn>
                          </v-col>
                      </v-row>
                  </v-card>
              </v-col>

              <!-- + workspace -->
              <v-col xs="12" sm="12" md="3">
                  <v-card class="workspace-card" flat>
                      <v-row>
                          <v-col class="text-center" style="min-height:200px;padding-top:20%">
                              <v-btn id="AutoTest_WS24" height="70" @click="AddWorkspaceDialog=true" text>
                                  <v-icon color="#E9E9E9" size="70">mdi-plus</v-icon>
                              </v-btn>
                          </v-col>
                      </v-row>
                  </v-card>
              </v-col>
          </v-row>
          <v-row v-if="filterWorkspaceList.length==0" justify="center">
              <v-col center cols="12" sm="12" class="text-center">

                  <p class="mt-12 pt-12">
                      <img src="..\..\..\src\assets\EmptyPlaceholders\dashboard.svg" height="150" width="150" />
                  </p>
                  <p class="heading-4-dark font-weight-medium pt-0">No workspace available</p>
              </v-col>
          </v-row>
          <v-snackbar v-model="snackbarMsg" top color="primary">
              {{Msg}}
          </v-snackbar>
          <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
              <template>
                  <div class="pa-0 ma-0">
                      <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                  </div>
              </template>
          </v-snackbar>
          <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
              <template>
                  <div class="pa-0 ma-0">
                      <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                  </div>
              </template>
          </v-snackbar>
          <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
              <template>
                  <div class="pa-0 ma-0">
                      <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                  </div>
              </template>
          </v-snackbar>
      </v-container>
    </v-card>
</template>

<script type="text/javascript">
    import SecondaryButton from '@/components/secondary-button.vue'  
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import { dbImworks } from '../../ImworksDb'
    import objUtils from '../../utils.js'
    //import syncData from '../../ImworksMobApp.js'
   

    var objWorkspace;
    async function importscript() {
        return Promise.all([
            import("../../BL/Workspace.js").then(mod => {
                objWorkspace = new mod.Workspace();
            })
        ]);
    }
    export default {

        components: {
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader
        },
        data: function () {
            return {
                snackbarMsg: false,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                checkboxPublic: false,
                checkboxPrivate: false,
                checkboxTags: [],
                filterChipsList: [],
                WorkspaceList: [],
                filterWorkspaceList: [],
                checkboxWorkspace: [],
                BulkActionMenu: false,
                SortMenu: false,

                deskName: "",
                deskDescription: "",
                PreLoader: false,
                AddWorkspaceDialog: false,
                wVisibility: null,
                justifyCenter: 'center',
                newTag: false,
                wsVisibilityItems: [
                    { wsVisibility: 'Private Workspace', value: 1 },
                    { wsVisibility: 'Public Workspace', value: 0 }
                ],

                TagIDList: "",
                TagList: [],
                wsTagColorItems: [],
                filterMenu: false,

                // Tags ------------
                NewTagMenu: false,
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTagDetails: "",
                NewTagName: "",
                swatches: [
                    ['#F98B00', '#00B448'],
                    ['#399CFF', '#D10000'],
                    ['#2C4258', '#00C2B0'],
                    ['#E6217B', '#6F00B0'],
                    ['#F1C100']
                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                profileeditImage: "",
                RandomColor: "",
                imageUrl: "",
                alphanumericRules: [
                    v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name'
                ],
                SelectedTagID: 0,
                IsSearchTagExist: false,
                privacyMenu: false,
                tagDropdownMenu: false,
            };
        },



        mounted: async function () {
            this.imageUrl = window.SERVER_URL + "/Upload";
            this.RandomColor = sessionStorage.getItem("RandomColor");
            await importscript();
            await this.fnGetMyWorkspaceList();
            await this.GetWSTagDetails();
            await this.OpenWSDialogue();
            await this.fnGetAllTags();
        },
       
        methods: {

            /// <summary>
            ///  getcolor :- Function used to get color of users
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            ///  fnWorkspaceSort :- Function used to sort workspace based on type (Name-Ascending, Name-Descending, Recently Added)
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            fnWorkspaceSort(type) {
                this.SortMenu = false;
                switch (type) {
                    case "Name-Ascending":
                        this.filterWorkspaceList.sort((a, b) => (a.WSName > b.WSName) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.filterWorkspaceList.sort((a, b) => (a.WSName < b.WSName) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.filterWorkspaceList.sort((a, b) => (a.WorkspaceID < b.WorkspaceID) ? 1 : -1);
                        break;
                    default:
                    // code block
                }
            },

            /// <summary>
            ///  fnArchiveWorkspace :- Function used to archive the selected workspaces
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnArchiveWorkspace() {
                this.BulkActionMenu = false;
                if (this.checkboxWorkspace.length == 0) {
                    this.Msg = ("Please select the workspaces you want to archive");
                    this.infoSnackbarMsg = true;
                    return;
                }

                var url = window.SERVER_URL + "/Workspace/ArchiveWorkspace";
                var data = new FormData();
                var wsidlist = [];
                for (var i = 0; i < this.filterWorkspaceList.length; i++) {
                    if (this.checkboxWorkspace[i]) {
                        wsidlist.push(this.filterWorkspaceList[i].WorkspaceID);
                        //console.log("added=", this.filterWorkspaceList[i].WorkspaceID);
                    }
                }
                var wsids = wsidlist.toString();
                //console.log("ws list=", wsids);
                data.append("WorkspaceIdList", wsids);

                var result = await objWorkspace.PostData(url, data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = ("Successfully archived selected workspaces");
                        this.successSnackbarMsg = true;
                        this.checkboxWorkspace = [];
                        this.fnGetMyWorkspaceList();
                        //clear all filter things also
                        this.checkboxTags = [];
                        this.checkboxPrivate = false;
                        this.checkboxPublic = false;
                        this.filterChipsList = [];
                    }
                }
            },

            /// <summary>
            ///  fnRemoveWorkspace :- Function used to remove the selected workspace
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnRemoveWorkspace() {
                this.BulkActionMenu = false;
                if (this.checkboxWorkspace.length == 0) {
                    this.Msg = ("Please select the workspaces you want to remove");
                    this.infoSnackbarMsg = true;
                    return;
                }

                var url = window.SERVER_URL + "/Workspace/RemoveWorkspace";
                var data = new FormData();
                var wsidlist = [];
                for (var i = 0; i < this.filterWorkspaceList.length; i++) {
                    if (this.checkboxWorkspace[i]) {
                        wsidlist.push(this.filterWorkspaceList[i].WorkspaceID);
                        //console.log("added=", this.filterWorkspaceList[i].WorkspaceID);
                    }
                }
                var wsids = wsidlist.toString();
                console.log("ws list=", wsids);
                data.append("WorkspaceIdList", wsids);

                var result = await objWorkspace.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = ("Successfully removed selected workspaces");
                        this.successSnackbarMsg = true;
                        this.checkboxWorkspace = [];
                        this.fnGetMyWorkspaceList();
                        //clear all filter things also
                        this.checkboxTags = [];
                        this.checkboxPrivate = false;
                        this.checkboxPublic = false;
                        this.filterChipsList = [];
                    }
                }
            },

            /// <summary>
            ///  fnClickFilterDone :- Function used to filter workspaces based on workspace privacy and workspace tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClickFilterDone() {
                this.filterChipsList = [];
                //this.filterMenu = false;
                if ((this.checkboxPrivate && this.checkboxPublic) || (!this.checkboxPrivate && !this.checkboxPublic))
                    this.filterWorkspaceList = this.WorkspaceList;
                else if (this.checkboxPrivate)
                    this.filterWorkspaceList = this.WorkspaceList.filter(x => x.WSIsPrivate);
                else if (this.checkboxPublic) {
                    this.filterWorkspaceList = this.WorkspaceList.filter(x => !x.WSIsPrivate);
                }
                if (this.checkboxPublic)
                    this.filterChipsList.push({ ChipID: 999, ChipName: "Public" });
                if (this.checkboxPrivate)
                    this.filterChipsList.push({ ChipID: 888, ChipName: "Private" });

                var tagidlist = [];
                for (var i = 0; i < this.checkboxTags.length; i++) {
                    if (this.checkboxTags[i]) {
                        tagidlist.push(this.wsTagColorItems[i].TagID);
                        this.filterChipsList.push({ ChipID: this.wsTagColorItems[i].TagID, ChipName: this.wsTagColorItems[i].TagName });
                    }
                }
                if (tagidlist.length > 0) {
                    var templist = [];
                    for (var j = 0; j < this.filterWorkspaceList.length; j++) {
                        var wsadd = false;
                        for (var i = 0; i < tagidlist.length && !wsadd; i++) {
                            var tagid = tagidlist[i];
                            for (var k = 0; k < this.filterWorkspaceList[j].WSTags.length; k++) {
                                if (this.filterWorkspaceList[j].WSTags[k].TagID == tagid) {
                                    templist.push(this.filterWorkspaceList[j]);
                                    wsadd = true;
                                }
                            }
                        }
                    }
                    this.filterWorkspaceList = templist;
                }
            },

            /// <summary>
            ///  fnChipClose :- Function used to close the filter chip list
            /// </summary>
            /// <param name="chipitem"></param>
            /// <returns> </returns>
            fnChipClose(chipitem) {
                this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);

                if (chipitem.ChipName == "Public")
                    this.checkboxPublic = false;
                else if (chipitem.ChipName == "Private")
                    this.checkboxPrivate = false;
                else {
                    for (var i = 0; i < this.checkboxTags.length; i++) {
                        if (this.checkboxTags[i] && this.wsTagColorItems[i].TagID == chipitem.ChipID) {
                            this.checkboxTags[i] = false;
                        }
                    }
                }
                this.fnClickFilterDone();
            },

            /// <summary>
            ///  fnEnter :- Function used to reroute into home page when click on enter button
            /// </summary>
            /// <param name="wsid"></param>
            /// <returns> </returns>
            async fnEnter(wsid) {
                this.PreLoader = true;
                await dbImworks.User.where("IsLoggedIn").equals(1).modify({ WorkspaceID: wsid });
                await dbImworks.TaskSettings.clear();
                await dbImworks.MyTaskList.clear();
                await dbImworks.TaskSyncDB.clear();
                await dbImworks.TbMyTask.clear();
                await dbImworks.SavedTaskDB.clear();
                await dbImworks.TbTaskAuditLog.clear();
                await dbImworks.TbDashboardProjectList.clear();

                await dbImworks.MyTaskDM.clear();
                await dbImworks.MyTaskSurveySection.clear();
                await dbImworks.MyTaskSurveyQuestion.clear();
                await dbImworks.MyTaskSurveySectionLogic.clear();
                await dbImworks.MyTaskSurveyTrackDate.clear();
                await dbImworks.TbMyTaskTable.clear();
                await dbImworks.MyTaskProjectSurveyQSTCondition.clear();
                await dbImworks.MyTaskProjectBenSurveyQSTCondition.clear();
                await dbImworks.MyTaskProjectSurveyQSTOption.clear();
                await dbImworks.MyTaskSurveyCascadingQuestionOptions.clear();
                await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.clear();
                await dbImworks.TbMyTaskProjectLogicConditions.clear();
                await dbImworks.TbMyTaskProjectBeneficiaryType.clear();
                await dbImworks.TbMyTaskBeneficiaryAnswerType.clear();
                await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.clear();
                await dbImworks.TbMyTaskProjectBeneficiary.clear();
                await dbImworks.MyTaskBeneficiaryTypeQuestion.clear();
                await dbImworks.MyTaskBeneficiaryTypeQuestionOption.clear();
                await dbImworks.MyTaskMasterList.clear();
                await dbImworks.MyTaskQuestionType.clear();
                await dbImworks.MyTaskProjectSurveyResponses.clear();
                this.PreLoader = false;
                this.$router.push({ name: "home", query: { WsID: wsid } });    
                
            },

            OpenWSDialogue() {
                const wsDialog = this.$route.query.WSdialog;
                this.AddWorkspaceDialog = wsDialog;
            },

            /// <summary>
            ///  fnGetMyWorkspaceList :- Function used to fetch MyWorkspace list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetMyWorkspaceList() {
                this.PreLoader = true;
                var url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                var result = await objWorkspace.GetData(url);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success" && result.data.length > 0) {
                        
                        this.WorkspaceList = result.data;
                        this.filterWorkspaceList = result.data;
                    } else if (result.status == "success" && result.data.length == 0) {
                        //console.log("result data =", result.data.length);
                        url = window.SERVER_URL + "/Workspace/CreateWorkspaceTemplate";
                        result = await objWorkspace.GetData(url);
                        //console.log("after template result data =", result.data.length);
                        //if (result.data.length > 0) {
                        if (result.status == "Unhandled Exception") {
                            this.$router.push({ name: "error" });
                        }
                        else {
                            this.WorkspaceList = result.data;
                            this.filterWorkspaceList = result.data;
                        }
                    }
                }
                //alert("Workspace list="+this.WorkspaceList);
                 this.PreLoader = false;
            },

            /// <summary>
            ///  GetWSTagDetails :- Function used to fetch workspace tag details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetWSTagDetails() {
                var result = await objWorkspace.GetWSTagDetails();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.wsTagColorItems = result.data;

                        this.wsTagColorItems.push({ TagID: 0, TagName: 'Other' });
                        if (this.TagList.length > 0) {
                            for (var i = 0; i < this.TagList.length; i++) {
                                if (this.TagList[i].TagID != 0)
                                    this.wsTagColorItems = this.wsTagColorItems.filter(x => x.TagID != this.TagList[i].TagID);
                            }
                        }
                    }
                }
            },

            /// <summary>
            ///  fnCreateWorkspace :- Function used to add new workspace
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnCreateWorkspace() {
                var i = 0;
                if (this.deskName == "") {
                    this.Msg = ("Wrokspace Name is required");
                    this.infoSnackbarMsg = true;
                    return;
                }

                if (this.wVisibility == null) {
                    this.Msg = ("Wrokspace Visibility is required");
                    this.infoSnackbarMsg = true;
                    return;
                }
                /*Chk Duplicate */

                for (i = 0; i < this.WorkspaceList.length; i++) {
                    if ((this.WorkspaceList[i].WSName == this.deskName) && (this.WorkspaceList[i].WSIsPrivate == this.wVisibility)) {
                        this.Msg = ("Duplicate Entry - same Desk Name & Visibility");
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }
                
                for (i = 0; i < this.SelectedTag.length; i++) {
                    if (this.TagIDList == "")
                        this.TagIDList = this.SelectedTag[i].TagID;
                    else
                        this.TagIDList = this.TagIDList + "," + this.SelectedTag[i].TagID;
                }
                var data = new FormData();
                data.append("WSName", this.deskName);
                data.append("WSDescription", this.deskDescription);
                data.append("WSIsPrivate", Boolean(this.wVisibility));
                data.append("WSTags", (this.TagIDList));
                data.append("location", "workspace");

                var input = document.getElementById('file');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("WorkSpaceImage", input.files[i]);
                    }
                }

                var result = await objWorkspace.CreateWorkspace(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = ("Successfully added new workspace...");
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = ("Failed in adding workspace .. ");
                        this.errorSnackbarMsg = true;
                    }
                    
                }
                this.CloseWSDialog();
                this.fnGetMyWorkspaceList();
            },

            /// <summary>
            ///  CloseWSDialog :- Function used to close workspace dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            CloseWSDialog() {
                this.deskName = "";
                this.deskDescription = "";
                this.wVisibility = null;
                this.TagIDList = "";
                this.TagList = [];
                this.AddWorkspaceDialog = false;
                this.CloseTag();
                this.SelectedTag = [];
            },

            //Tag

            /// <summary>
            ///  fnInitializeSteper :- Function used to initialize stepper for tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSteper() {
                
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.SearchTagDetails = "";
                    this.AllTagListSteps = 2;
                }
            },

            /// <summary>
            ///  fnGetAllTags :- Function used to fetch all workspace tag list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objWorkspace.GetWSTagDetails();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = ("Failed to get tag list.");
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  searchTag :- Function used to create a new tag if not exist while searching
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async searchTag() {
                this.AllTagListSteps = 1;
                this.TagHeaderName = 'Add New Tag';
                this.NewTagName = this.SearchTagDetails;
            },

            /// <summary>
            ///  fnEditTagDetails :- Function used to open edit tag dialog when click on edit button based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnEditTagDetails(TagItem) {
                
                this.AllTagListSteps = 3;
                this.TagHeaderName = 'Edit Tag Name';
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;
                this.SelectedTagID = TagItem.TagID;
            },

            /// <summary>
            ///  UpdateTagDetails :- Function used to update tag details based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();

                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objWorkspace.UpdateWSTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "Success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags()
                        this.UpdatingTagItem = "";
                    }
                    else {
                        this.Msg = result.status;
                        this.infoSnackbarMsg = true;
                            
                    }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create New Tag when there is no tags are based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                var result = await objWorkspace.AddNewWsTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.SearchTagDetails = this.NewTagName;
                        this.NewTagName = "";
                        await this.fnGetAllTags();
                        this.filterAllTagList = this.AllTagList;
                        if (this.SearchTagDetails.length > 0)
                            this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                        if (this.filterAllTagList.length > 0) {
                            this.IsSearchTagExist = true;
                        }
                        this.AllTagListSteps = 2;
                        this.Msg = ("Tag Created Successfully");
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = ("Tag already exists");
                        this.infoSnackbarMsg = true;
                    }

            },

            /// <summary>
            ///  fnAddToSelectedTag :- Function used to add tag to selected tag items based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToSelectedTag(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.SelectedTag.length > 0) {
                        var temp = this.SelectedTag.filter(x => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                        } else {

                            const index = this.SelectedTag.findIndex((element, index) => {
                                if (element.TagID === TagItem.TagID) {
                                    return true
                                }
                            });
                            this.SelectedTag.splice(index, 1);
                            if (this.SelectedTag.length == 0) {
                                this.allTags = false;
                            }
                        }
                    } else {
                        this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                    this.TickSelectedTag();
                    this.NewTagMenu = false;
                    
                }
            },

            /// <summary>
            ///  fnRemoveFromSelectedTagItem :- Function used to remove tag from selected tag item based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnRemoveFromSelectedTagItem(TagItem) {

                const index = this.SelectedTag.findIndex((element, index) => {
                    if (element.TagID === TagItem.TagID) {
                        return true
                    }
                });
                this.SelectedTag.splice(index, 1);
                //this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
                //this.SelectedTag = [...this.SelectedTag];
                if (this.SelectedTag.length == 0) {
                    this.allTags = false;
                }
                this.TickSelectedTag();
            },

            /// <summary>
            ///  fnRemoveFromAllTagList :- Function used to delete tag from all tag list in edit mode
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnRemoveFromAllTagList() {
                
                this.AllTagList.splice(this.AllTagList.indexOf(this.SelectedTagID), 1);
                this.AllTagList = [...this.AllTagList];
                if (this.AllTagList == 0) {
                    this.AllTagListSteps = 1;
                    this.TagHeaderName = 'Add New Tag';
                }
                var data = new FormData();
                data.append("TagID", this.SelectedTagID);
                var result = await objWorkspace.DeleteWSTagDetails(data);
                
                if (result.status == true) {
                    this.Msg = "Tag Removed Successfully";
                    this.successSnackbarMsg = true;
                    this.SelectedTagID = 0;
                    this.NewTagName = "";
                    this.AllTagListSteps = 2;
                    this.fnGetAllTags();
                } else {
                    this.Msg = "Tag already used by another workspace";
                    this.infoSnackbarMsg = true;
                    
                }
            },

            /// <summary>
            ///  CloseTag :- Function used to close tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseTag() {
                this.color = "";
                this.TagHeaderName = 'Add Tag';
                this.NewTagName = "";
                this.NewTagMenu = false;
                this.EditTagName = "";
            },

            /// <summary>
            ///  previewImage :- Function used to show image preview before image get upload
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            previewImage: function (event) {
                var self = this;
                var input = event.target;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        self.dataURItoBlob(e.target.result, "image/jpeg");
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            },

            /// <summary>
            ///  dataURItoBlob :- Function used to convert data URI to file
            /// </summary>
            /// <param name="dataURI"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            dataURItoBlob(dataURI, type) {
                // convert base64 to raw binary data held in a string
                var byteString = atob(dataURI.split(',')[1]);

                // separate out the mime component
                var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

                // write the bytes of the string to an ArrayBuffer
                var ab = new ArrayBuffer(byteString.length);
                var ia = new Uint8Array(ab);
                for (var i = 0; i < byteString.length; i++) {
                    ia[i] = byteString.charCodeAt(i);
                }

                // write the ArrayBuffer to a blob, and you're done
                var bb = new Blob([ab], { type: type });
                this.profileeditImage = URL.createObjectURL(bb);
            },

            /// <summary>
            ///  fnCheckProfieImage :- Function used to check profile image of user
            /// </summary>
            /// <param name="data"></param>
            /// <returns> true/false </returns>
            fnCheckProfieImage(data)
            {
                if (data != null) {
                    if (data.ProfileImage != null && data.ProfileImage.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },

            /// <summary>
            ///  TickSelectedTag :- Function used to show tick mark on selected tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            TickSelectedTag() {
                    for (var cnt = 0; cnt <= this.filterAllTagList.length - 1; cnt++) {
                        var isExistTag = this.SelectedTag.filter(x => x.TagID == this.filterAllTagList[cnt].TagID);
                        if (isExistTag.length > 0) {
                            this.filterAllTagList[cnt].IsChecked = true;
                        } else {
                            this.filterAllTagList[cnt].IsChecked = false;
                   }
               }   
            }
        },
        watch: {
            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.IsSearchTagExist = false;
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                if (this.filterAllTagList.length > 0) {
                    this.IsSearchTagExist = true;
                }
            },

        },
    };
</script>