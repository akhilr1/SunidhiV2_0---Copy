<template>
    <div>
        <v-card class="pa-4">
            <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
            <div v-for="wsItem in WSDetails" :key="WorkspaceID">
                <v-row v-if="!isShared" v-for="wsItem in WSDetails" :key="WorkspaceID">
                    <v-col>
                        <h3 class="text-left page-head">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1 ml-4">
                                <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                            </v-avatar>
                            <router-link :to="{ name: 'workspace'}" class="page-head text-link">My Workspace </router-link>
                            <v-icon size="15">mdi-chevron-right</v-icon>
                            {{wsItem.WSName}}
                        </h3>
                    </v-col>
                </v-row>
                <!-- Header Text Shared-->
                <v-row v-if="isShared" v-for="wsItem in WSDetails" :key="WorkspaceID">
                    <v-col>
                        <h3 class="text-left page-head">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                            </v-avatar>
                            <router-link :to="{ name: 'SharedWorkspace'}" class="page-head text-link">Shared Workspace </router-link>
                            <v-icon size="15">mdi-chevron-right</v-icon>
                            {{wsItem.WSName}}
                        </h3>
                    </v-col>
                </v-row>
            </div>
            <v-container fluid>
                <v-row>
                    <v-tabs hide-slider class="grey-tab" centered v-model="vTab">
                        <v-tab>Details</v-tab>
                        <v-tab>Team</v-tab>
                        <v-tab>Settings</v-tab>
                        <v-tab>Payment</v-tab>

                        <v-tabs-items v-model="vTab">
                            <!--Details-->
                            <v-tab-item>
                                <v-container fluid class="pt-2">
                                    <v-card v-for="wsItem in WSDetails" :key="WorkspaceID" class="px-4 elevation-0">
                                        <v-card-text class="pt-0">
                                            <v-row no-gutters>
                                                <v-col cols="12" xs="12" md="12">
                                                    <v-row :justify="justifyCenter">
                                                        <v-col md="8" xl="4">
                                                            <v-container fluid>
                                                                <v-list>
                                                                    <v-list-item class="pb-2">
                                                                        <v-avatar size="60" tile class="elevation-1 base-border-radius text-left mr-2" light :color="`${getcolor(wsItem.WSName)}`">
                                                                            <v-img v-bind:src="profileeditImage" height="60" width="60" v-if="profileeditImage.length>0"></v-img>
                                                                            <span class="headline white--text" v-else>{{wsItem.WSName == null ? "" : wsItem.WSName.substring(0, 1).toUpperCase()}}</span>
                                                                            <div class="dp-edit-btn" v-if="!isShared">
                                                                                <input class="upload" type="file" id="file" hide-details accept="image/*" @change="previewImage" />
                                                                            </div>
                                                                        </v-avatar>

                                                                        <h5 class="page-head pb-0 ">{{wsItem.WSName}}</h5>

                                                                        <v-spacer></v-spacer>
                                                                        <!--<Secondary-Button id="" title="Edit Details" @click.native="fnEditProjectClick()" v-bind:disabled="!screenAccess"></Secondary-Button>-->
                                                                        <v-btn id="" color="primary elevation-0" dark @click="enableSave =true, cancelTag = 1" v-if="!enableSave" class="mr-4" v-bind:disabled="isShared">
                                                                            <v-icon size="16" class="mr-2">mdi-pencil-outline</v-icon>
                                                                            Edit
                                                                        </v-btn>
                                                                        <v-btn id="AutoTest_WS43" color="success-btn elevation-0" dark @click="fnEnter(wsItem.WorkspaceID)">
                                                                            <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                                                            Enter
                                                                        </v-btn>
                                                                    </v-list-item>

                                                                </v-list>
                                                                <v-col class="pb-0">
                                                                    <div class="heading-3-dark pt-1 text-limit">
                                                                        Workspace Name
                                                                        <v-btn id="AutoTest_WS44" text max-width="16" min-width="16" height="16" v-if="!isShared &&!enableSave">
                                                                            <!--<v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>-->
                                                                        </v-btn>
                                                                    </div>
                                                                    <h5 class="heading-4" v-if="!enableSave">
                                                                        <span v-text="wsItem.WSName"></span>
                                                                    </h5>
                                                                    <v-text-field outlined dense type="text" v-model="deskName" v-if="enableSave" class="pb-0">
                                                                        {{deskName}}
                                                                    </v-text-field>
                                                                </v-col>
                                                                <v-col>
                                                                    <div class="heading-3-dark text-limit">
                                                                        Workspace Description
                                                                        <v-btn id="AutoTest_WS45" text max-width="16" min-width="16" height="16" v-if="!isShared && !enableSave">
                                                                            <!--<v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>-->
                                                                        </v-btn>
                                                                    </div>
                                                                    <h5 class="text-left heading-4 v-scrolling-div" v-if="!enableSave" style="max-height:100px;">
                                                                        <span v-text="wsItem.WSDescription"></span>
                                                                    </h5>
                                                                    <v-textarea outlined dense type="text" v-model="deskDescription" v-if="enableSave">
                                                                        {{deskDescription}}
                                                                    </v-textarea>
                                                                </v-col>
                                                                <!-- Tag Section modified-->
                                                                <v-col>
                                                                    <v-row class="mb-1">
                                                                        <v-col class="text-left d-inline-flex" md="12">
                                                                            <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                                            <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-btn id="AutoTest_WS46" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                                                           @click="fnInitializeSteper()" class="mr-1">
                                                                                        <v-icon size="15">mdi-plus</v-icon>
                                                                                    </v-btn>
                                                                                </template>
                                                                                <v-card class="elevation-1">
                                                                                    <v-row>
                                                                                        <v-col cols="12" md="12" class="pt-0">
                                                                                            <!-- Header Text  -->
                                                                                            <v-row class="pt-1 px-1">
                                                                                                <v-col cols="12" md="12" class="d-inline-flex">
                                                                                                    <v-btn id="AutoTest_WS47" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                                        <v-icon size="15">mdi-chevron-left</v-icon>
                                                                                                    </v-btn>
                                                                                                    <v-spacer></v-spacer>
                                                                                                    <h3 class="text-left page-head pt-2">
                                                                                                        {{TagHeaderName}}
                                                                                                    </h3>
                                                                                                    <v-spacer></v-spacer>
                                                                                                    <v-btn id="AutoTest_WS48" text @click="fnInitializeSteper(),NewTagMenu = false">
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
                                                                                                                <v-btn id="AutoTest_WS49" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-stepper-content>

                                                                                                    <!-- Select from existing Tags-->
                                                                                                    <v-stepper-content step="2" class="pa-0">

                                                                                                        <v-row style="max-height:40vh">
                                                                                                            <v-col class="pb-1">
                                                                                                                <!--<p class="mb-2">Search Tags</p>-->
                                                                                                                <v-text-field outlined dense type="text" label="Search tag here" hide-details class="mb-3" v-model="SearchTagDetails">
                                                                                                                </v-text-field>
                                                                                                                <div style="height: 18vh;" class="v-scrolling-div" v-show="filterAllTagList.length>0">
                                                                                                                    <v-chip-group dark multiple column style="max-width: 220px">
                                                                                                                        <div v-for="index in filterAllTagList" :key="index.TagID" class="d-inline-flex">
                                                                                                                            <v-chip id="AutoTest_WS50" style="width:180px" active-class="active-success-check" :key="index.TagID" :color="index.TagColor" medium
                                                                                                                                    label @click.native="fnAddToSelectedTag(index),NewTagMenu = true">
                                                                                                                                <v-icon v-if="index.IsChecked" size="20" left :key="index.TagID">mdi-check</v-icon>
                                                                                                                                <span class="text-limit text-center">
                                                                                                                                    {{ index.TagName }}
                                                                                                                                </span>
                                                                                                                            </v-chip>
                                                                                                                            <v-icon id="AutoTest_WS51" right color="primary" size="16" @click="fnEditTagDetails(index)" class="mx-2">mdi-pencil-outline</v-icon>
                                                                                                                        </div>

                                                                                                                    </v-chip-group>
                                                                                                                </div>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-col cols="12" md="12" class="text-center mt-3">
                                                                                                            <v-btn id="AutoTest_WS52" depressed @click="searchTag()" color="success-btn" dark class="text-limit" max-width="200">
                                                                                                                Create New
                                                                                                                <span v-if="SearchTagDetails.length>0 && SearchTagDetails.length<=6 && IsSearchTagExist == false" class="pl-1 text-center">"{{SearchTagDetails}}"</span>
                                                                                                                <span class="pl-1 text-center" v-else v-show="SearchTagDetails.length>6 && IsSearchTagExist == false">"{{ SearchTagDetails.substring(0,6)+".." }}'"</span>
                                                                                                            </v-btn>
                                                                                                        </v-col>
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
                                                                                                                <span class="ml-3">Update Color</span>
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
                                                                                                                <v-btn id="AutoTest_WS53" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                                                <!--<v-chip id="AutoTest_WS54" v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                                                                <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1" @click="allTags=true">{{SelectedTag[0].TagName}}</v-chip>
                                                                                <v-chip dark="" v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1" @click="allTags=true">{{SelectedTag[1].TagName}}</v-chip>
                                                                                <v-chip dark="" v-if="SelectedTag.length>2" label small class="tags tag-count mr-1" @click="allTags=true">+{{SelectedTag.length-2}}<span class="px-1">more</span></v-chip>
                                                                            </div>
                                                                            <v-chip v-if="allTags == true" label small class="tags mx-1" @click="allTags=false">Show less</v-chip>
                                                                            <!--<v-btn id="AutoTest_WS55" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                                        </v-btn>
                                                                        <v-btn id="AutoTest_WS56" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                                        </v-btn>-->
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                                                        <v-col md="12" class="mb-0 pt-0">
                                                                            <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                                                <v-chip id="AutoTest_WS57" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                                                    {{TagItem.TagName}}
                                                                                </v-chip>
                                                                            </v-chip-group>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-col>
                                                                <!-- Teams-->
                                                                <!--<v-col style="cursor:pointer">
                                                                <span class="small-text mr-1">
                                                                    <v-icon size="12">mdi-account</v-icon> Team
                                                                </span>
                                                                <v-avatar id="AutoTest_WS58" v-if="WSUserList.length>=1" size="27" class="team-avatar" color="red" @click="vTab=1">
                                                                    <span class="white--text">T</span>
                                                                </v-avatar>
                                                                <v-avatar id="AutoTest_WS59" v-if="WSUserList.length>=2" size="27" class="team-avatar" color="red" @click="vTab=1">
                                                                    <span class="white--text">K</span>
                                                                </v-avatar>
                                                                <v-avatar id="AutoTest_WS60" v-if="WSUserList.length>=3" size="27" class="team-avatar" color="red" @click="vTab=1">
                                                                    <span class="white--text">J</span>
                                                                </v-avatar>
                                                                <v-avatar id="AutoTest_WS61" v-if="WSUserList.length>=4" size="27" class="team-count" color="red" @click="vTab=1">
                                                                    <span class="white--text">+{{WSUserList.length-2}}</span>
                                                                </v-avatar>
                                                            </v-col>-->
                                                                <v-col style="cursor:pointer" class="pt-0">
                                                                    <span class="small-text mr-1">
                                                                        <v-icon size="12">mdi-account</v-icon> Team
                                                                    </span>
                                                                    <v-avatar size="27" class="team-avatar" v-for="(usr,index) in WSUserList.slice(0, 3)" :key="index" :color="`${getcolor(usr.Name)}`" @click="vTab=1">
                                                                        <v-img v-bind:src="userimage + usr.ProfileImage" alt="User DP" height="27" width="27" class="ma-4" v-if="fnCheckProfieImage(usr)"></v-img>
                                                                        <span class="white--text" v-else>{{usr.Name == null ? "" : usr.Name.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    <v-avatar v-if="WSUserList.length>=4" size="27" class="team-count" @click="vTab=1">
                                                                        +{{WSUserList.length-2}}
                                                                    </v-avatar>
                                                                </v-col>

                                                                <!-- Private Or Public section-->
                                                                <v-col class="pt-0">
                                                                    <v-row :justify="justifyCenter" no-gutters>
                                                                        <v-col class="pt-0" v-if="wsItem.WSIsPrivate">
                                                                            <v-icon size="12">mdi-lock </v-icon>Private
                                                                        </v-col>

                                                                        <v-col class="pt-0" v-if="!wsItem.WSIsPrivate">
                                                                            <v-icon size="12">mdi-earth</v-icon>
                                                                            Public
                                                                        </v-col>
                                                                    </v-row>

                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-4" v-if="!isShared">
                                                                    <Success-Button id="AutoTest_WS62" class="btn-122x36 mr-4" title="Save" @click.native="fnUpdateWorkspace()" v-bind:disabled="!enableSave || deskName.length == 0 || deskName.trim() == ''"></Success-Button>
                                                                    <Default-Button-Outlined id="AutoTest_WS63" class="btn-122x36" title="Cancel" @click.native="CloseWS(),fnGetWorkspaceDetails(),enableSave=false"></Default-Button-Outlined>
                                                                </v-col>

                                                            </v-container>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                    </v-card>
                                </v-container>
                            </v-tab-item>

                            <!--Teams-->
                            <v-tab-item>
                                <v-container fluid class="pt-4">
                                    <v-row no-gutters class="pt-0">
                                        <v-col cols="12" md="12" sm="12" xs="12" class="px-4 pt-0" v-for="wsItem in WSDetails" :key="WorkspaceID">
                                            <v-data-table :headers="userTableheaders"
                                                          :items="WSUserList"
                                                          item-key="Name"
                                                          sort-by="Name"
                                                          class="elevation-1"
                                                          :search="SearchName">

                                                <template v-slot:top>
                                                    <v-row class="pa-4">
                                                        <v-col cols="12" xs="9" sm="9" md="9">
                                                            <v-row>
                                                                <v-col md="auto" class="pt-0 pl-0">
                                                                    <v-list dense>
                                                                        <v-list-item>
                                                                            <v-avatar size="60" tile class="elevation-1 base-border-radius mr-2" light :color="`${getcolor(wsItem.WSName)}`">
                                                                                <v-img v-bind:src="uploadedImage" height="60" width="60" class="ma-4" v-if="uploadedImage.length>0"></v-img>
                                                                                <span class="headline white--text text-center" v-else>{{wsItem.WSName == null ? "" : wsItem.WSName.substring(0, 1).toUpperCase()}}</span>
                                                                            </v-avatar>
                                                                            <v-list-item-content class="page-head">
                                                                                {{wsItem.WSName}}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </v-col>
                                                                <v-col>
                                                                    <v-btn id="AutoTest_WS64" height="39" color="success-btn elevation-0 enter-btn mt-4" dark="" @click="fnEnter(wsItem.WorkspaceID)">
                                                                        <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                                                        Enter
                                                                    </v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col cols="12" xs="3" sm="3" md="3" class="mt-3">
                                                            <v-row>
                                                                <v-spacer></v-spacer>
                                                                <!--Search-->
                                                                <v-text-field v-model=SearchName label="Search" class="mr-2 mt-4 table-search" dense prepend-inner-icon="mdi-magnify" outlined></v-text-field>
                                                            </v-row>
                                                        </v-col>
                                                    </v-row>
                                                </template>
                                                <template v-slot:item.Name="{item}">
                                                    <!-- ,params: {Name} -->
                                                    <v-list class="py-0 table-v-list">
                                                        <!--<v-list-item class="my-0 px-0">
                                                        <v-list-item-icon>
                                                            <v-avatar size="30" class="elevation-1" light :color="`${getcolor(item.Name)}`">
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            {{item.Name}}
                                                        </v-list-item-content>
                                                    </v-list-item>-->
                                                        <div class="pa-0 text-limit" style="width:135px">
                                                            <v-avatar size="30" class="elevation-1" light :color="`${getcolor(item.Name)}`">
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                            {{ item.Name }}
                                                        </div>
                                                    </v-list>
                                                </template>
                                                <template v-slot:item.EmailId="{item}">
                                                    <div v-if="item.EmailId" class="pa-0 text-limit" style="width:135px">
                                                        {{ item.EmailId}}
                                                    </div>
                                                    <div v-else>N/A</div>
                                                </template>
                                                <template v-slot:item.userOrganisation="{item}">
                                                    <div v-if="item.userOrganisation" class="pa-0 text-limit" style="width:130px">
                                                        {{ item.userOrganisation }}
                                                    </div>
                                                    <div v-else>N/A</div>
                                                </template>
                                                <template v-slot:item.userGroup="{item}">
                                                    <div v-if="item.userGroup" class="pa-0 text-limit" style="width:90px">
                                                        {{ item.userGroup }}
                                                    </div>
                                                    <div v-else>N/A</div>
                                                </template>
                                                <template v-slot:item.userRole="{item}">
                                                    <div v-if="item.userRole" class="pa-0 text-limit" style="width:80px">
                                                        {{ item.userRole }}
                                                    </div>
                                                    <div v-else>N/A</div>
                                                </template>
                                                <template v-slot:item.userProjectCount="{item}">
                                                    <div v-if="item.userProjectCount" class="pa-0 text-limit" style="width:80px">
                                                        {{ item.userProjectCount}}
                                                    </div>
                                                    <div v-else>N/A</div>
                                                </template>
                                            </v-data-table>
                                        </v-col>
                                    </v-row>
                                </v-container>

                            </v-tab-item>
                            <!--Settings-->
                            <v-tab-item>
                                <v-card v-for="wsItem in WSDetails" :key="WorkspaceID" class="px-4 pt-2 elevation-0">
                                    <v-card-text class="pt-0">
                                        <v-row no-gutters>
                                            <v-col cols="12" xs="12" md="12">
                                                <v-row :justify="justifyCenter">
                                                    <v-col md="5">
                                                        <v-container fluid>

                                                            <v-list class="text-center">

                                                                <v-list-item class="pb-2">
                                                                    <v-avatar size="60" tile class="elevation-1 base-border-radius text-left mr-2" light :color="`${getcolor(wsItem.WSName)}`">
                                                                        <v-img v-bind:src="uploadedImage" height="60" width="60" class="ma-4" v-if="uploadedImage.length>0"></v-img>
                                                                        <span class="headline white--text" v-else>{{wsItem.WSName == null ? "" : wsItem.WSName.substring(0, 1).toUpperCase()}}</span>
                                                                    </v-avatar>
                                                                    <h5 class="page-head pb-0">{{wsItem.WSName}}</h5>

                                                                    <v-spacer></v-spacer>
                                                                    <p class="mt-4">
                                                                        <v-btn id="AutoTest_WS65" color="success-btn elevation-0 enter-btn" dark @click="fnEnter(wsItem.WorkspaceID)">
                                                                            <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                                                            Enter
                                                                        </v-btn>
                                                                    </p>

                                                                </v-list-item>

                                                            </v-list>

                                                            <v-col cols="12" md="12" class="pt-2 px-4" v-if="IsEnableSettingsVisibility">
                                                                <h3 class="text-center heading-3">Select the type of workspace visibility</h3>
                                                                <v-row class="heading-3" :justify="justifyCenter">
                                                                    <v-radio-group v-model="vWSVisibility" @change="isUpdate=true">
                                                                        <v-radio label="Private Workspace" :value='true' color="success-check-box"></v-radio>
                                                                        <v-radio label="Public Workspace" :value='false' color="success-check-box"></v-radio>
                                                                    </v-radio-group>
                                                                </v-row>
                                                            </v-col>
                                                            <v-col cols="12" md="12" class="pt-2 px-4" v-if="!IsEnableSettingsVisibility">
                                                                <h3 class="text-center heading-3">Edit the type of workspace visibility</h3>
                                                                <!-- Private Visibility-->
                                                                <v-row class="heading-3 pt-2" :justify="justifyCenter" v-if="vWSVisibility == true">

                                                                    <h3 class="text-center heading-3 pt-0">Private Workspace</h3>
                                                                    <v-btn id="AutoTest_WS66" text max-width="17" min-width="17" height="17" @click="IsEnableSettingsVisibility=true">
                                                                        <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                                    </v-btn>
                                                                </v-row>
                                                                <!-- Public Visibility-->
                                                                <v-row class="heading-3 pt-2" :justify="justifyCenter" v-if="vWSVisibility == false">

                                                                    <h3 class="text-center heading-3 pt-0">Public Workspace</h3>
                                                                    <v-btn id="AutoTest_WS67" text max-width="17" min-width="17" height="17" @click="IsEnableSettingsVisibility=true">
                                                                        <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                                    </v-btn>
                                                                </v-row>
                                                            </v-col>


                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-2 mr-0">
                                                                <Success-Button id="AutoTest_WS68" class="btn-122x36 mr-4" title="Save" @click.native="fnUpdateWSVisibility(vWSVisibility),IsEnableSettingsVisibility= false" v-bind:disabled="!isUpdate || isShared"></Success-Button>
                                                                <Default-Button-Outlined id="AutoTest_WS69" class="btn-122x36" title="Cancel" @click.native="IsEnableSettingsVisibility= false,vWSVisibility = TemWrokspaceVisibility"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                        </v-row>
                                    </v-card-text>
                                </v-card>

                            </v-tab-item>
                        </v-tabs-items>
                    </v-tabs>
                </v-row>
            </v-container>
        </v-card>
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
    </div>
</template>
<script type="text/javascript">
  import SuccessButton from '@/components/success-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import { dbImworks } from '../../ImworksDb'
   // import syncData from '../../ImworksMobApp.js'
    import SecondaryButton from '@/components/secondary-button.vue'
    import objUtils from '../../utils.js'
    var objWorkspace;
    var objUser;
    

    async function importscript() {
        return Promise.all([
            import("../../BL/Workspace.js").then(mod => {
                objWorkspace = new mod.Workspace();
            })
        ]);
    };
    async function importscriptUser() {
        return Promise.all([
            import("../../BL/User.js").then(mod => {
                objUser = new mod.User();
            })
        ]);
    }
   
    export default {
    
      components:{
          'Success-Button': SuccessButton,
          'Default-Button-Outlined': DefaultButtonOutlined,
          'Pre-Loader': PreLoader,
          'Secondary-Button': SecondaryButton,
      },
    
        data: function() {
            return {
                PreLoader: false,
                snackbarMsg: false,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                WSDetails: [],
                deskName: "",
                deskDescription: "",
                
                WSUserList: [],
                allTags: false,

                enableSave :false,
                vWSVisibility: false,
                TemWrokspaceVisibility : false,
                justifyCenter: 'center',
                cancelTag: 0,


                
                TagIDList: "",
                TagList: [],
                vTab: 0,
                isShared: false,
                isUpdate :false,
                WorkspaceID: 0,
                userimage: "", 

                //===========UsersList==============

                userTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "Email Id", value: "EmailId" },
                    { text: "Organization", value: "userOrganisation" },
                    { text: "User Group", value: "userGroup", },
                    { text: "User Role", value: "userRole" },
                    { text: "Projects", value: "userProjectCount" },
                ],


                //==================================
                SearchName: "",
                IsEnableSettingsVisibility: false,


                // Tag Menu ......... 
                NewTagMenu: false,
                model: "",

                // Altered Tag Menu
                NewTagName: "",
                EditTagName: ",",

                 // Tags ------------
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTagDetails: "",
                swatches: [
                    ['#F98B00', '#00B448'],
                    ['#399CFF', '#D10000'],
                    ['#2C4258', '#00C2B0'],
                    ['#E6217B', '#6F00B0'],
                    ['#F1C100']

                ],
                allTags: false,
                type: 'hex',
                hex: '#FF00FF',
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                RandomColor: "",
                uploadedImage: "",
                profileeditImage: "",
                SelectedTagID: 0,
            };
        },
     
        mounted: async function () {
            this.RandomColor = sessionStorage.getItem("RandomColor");
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WID;
            await importscript();
            await importscriptUser();
            await this.fnGetWorkspaceDetails();  
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
            ///  fnEnter :- Function used to reroute into user page when click on enter button
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
                this.$router.push({ name: "userlist", query: { WsID: this.WorkspaceID, Access: 3 } });
              
            },
       
            /// <summary>
            ///  fnGetWorkspaceDetails :- Function used to fetch all workspace details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetWorkspaceDetails() {
               
                var data = new FormData();                 
                this.isShared = this.$route.query.Shared;    

                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objWorkspace.GetWorkspaceDetails(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.WSDetails = result.data;
                        this.deskName = this.WSDetails[0].WSName;
                        this.deskDescription = this.WSDetails[0].WSDescription;
                        this.vWSVisibility = this.WSDetails[0].WSIsPrivate;
                        this.TemWrokspaceVisibility = this.WSDetails[0].WSIsPrivate;

                        if (this.WSDetails[0].WorkSpaceImage != null &&
                            this.WSDetails[0].WorkSpaceImage.length > 0) {
                            this.uploadedImage = window.SERVER_URL + "/Upload" + this.WSDetails[0].WorkSpaceImage;
                            this.profileeditImage = window.SERVER_URL + "/Upload" + this.WSDetails[0].WorkSpaceImage;
                        } else {
                            this.uploadedImage = "";
                            this.profileeditImage = "";
                        }
                    }
                }
               
                this.GetUsersList();
                this.GetWSTags();
               
            },

            /// <summary>
            ///  GetUsersList :- Function used to fetch all users based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetUsersList() {
                
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objUser.GetAllUsers(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.WSUserList = result.data;
                    }
                }
            },

            /// <summary>
            ///  GetWSTags :- Function used to fetch workspace tag details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetWSTags() {
               
                var data = new FormData(); 
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objWorkspace.GetWSTags(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.data) {
                        this.SelectedTag = result.data;

                    }
                }
            },

            /// <summary>
            ///  fnUpdateWorkspace :- Function used to update workspace & workspace tag details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnUpdateWorkspace() {
               
                var i = 0;
                if (this.deskName == "") {
                    this.Msg = ("Wrokspace Name is required");
                    this.infoSnackbarMsg = true;
                    return;
                }
                for (i = 0; i < this.SelectedTag.length; i++) {
                    if (this.TagIDList == "")
                        this.TagIDList = this.SelectedTag[i].TagID;
                    else
                        this.TagIDList = this.TagIDList + "," + this.SelectedTag[i].TagID;
                }
                var data = new FormData();
                //const wsId = this.$route.query.WsID; 
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("WSName", this.deskName);
                data.append("WSDescription", this.deskDescription);
                data.append("WSTags", this.TagIDList);
                data.append("location", "workspace");

                var input = document.getElementById('file');

                if (input != null && input != undefined) {
                    for (var i = 0; i < input.files.length; i++) {
                        data.append("WorkSpaceImage", input.files[i]);
                    }
                }

                var result = await objWorkspace.UpdateWorkspace(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = ("Success");
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = ("Failed");
                        this.errorSnackbarMsg = true;
                    }
                    
                }
                this.CloseWS();
                this.fnGetWorkspaceDetails();

            },

            /// <summary>
            ///  fnUpdateWSVisibility :- Function used to update workspace visibility based on WorkspaceID
            /// </summary>
            /// <param name="wsVisibility"></param>
            /// <returns></returns>
            async fnUpdateWSVisibility(wsVisibility) {

                var data = new FormData();
               
                 //const wsId = this.$route.query.WsID; 
                 data.append("WorkspaceID", this.WorkspaceID);
                data.append("WSIsPrivate", wsVisibility);
                 var result = await objWorkspace.SaveWSVisibility(data);
                 if (result.status == "Unhandled Exception") {
                     this.$router.push({ name: "error" });
                 }
                 else {
                     if (result.status == "success") {
                         this.Msg = ("Success");
                         this.successSnackbarMsg = true;
                     }
                     else {
                         this.Msg = ("Failed");
                         this.errorSnackbarMsg = true;
                     }
                     
                     this.wsVisibility = !this.wsVisibility;
                 }

            },            

            /// <summary>
            ///  CloseWS :- Function used to close workspace details
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            CloseWS() {
                this.WSDetails = [];
                this.WSUserList = [];
                this.deskName = "";
                this.deskDescription = "";
                this.wVisibility = "";
                this.vWSVisibility = "";
                this.TagIDList = "";
                this.TagList = [];
                this.AddWorkspaceDialog = false;
                this.enableSave = false;
                this.isUpdate = false;
                this.CloseTag();
                if (this.cancelTag == 0) {
                    this.$router.push({ name: "workspace" });
                }
                else {
                    this.cancelTag = 0;
                }
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
            ///  UpdateTagDetails :- Function used to update tag details in edit mode based on color and TagName
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
                        if (this.SearchTagDetails.length > 0)
                            this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
                        if (this.filterAllTagList.length > 0) {
                            this.IsSearchTagExist = true;
                        }
                        this.AllTagListSteps = 2;
                        this.Msg = ("Tag Created Successfully");
                        this.infoSnackbarMsg = true;
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
                self.enableSave = true;
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
            fnCheckProfieImage(data) {
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
            },
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
<style scoped>
    .text-limit {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
     .text-limit:hover {
            white-space: normal;
            overflow: visible;
     }   
</style>