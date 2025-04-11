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
                          <v-icon size="15" color="primary">mdi mdi-folder-remove-outline</v-icon>
                      </v-avatar>Archived Workspace
                  </h5>
              </v-col>
              <v-col v-if="ArchivedWorkspaceList.length>0" cols="12" xs="12" sm="12" md="6">
                  <v-row>
                      <v-spacer></v-spacer>
                      <!--Bulk Action -->
                      <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="BulkActionMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn height="39" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-4">
                                  Bulk Actions
                                  <v-icon>
                                      mdi-chevron-down
                                  </v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                              <v-list-item id="AutoTest_WS33" @click.stop="fnUnArchiveWorkspace()">
                                  <v-list-item-icon>
                                      <v-icon>mdi-file-undo-outline</v-icon>
                                  </v-list-item-icon>
                                  <v-list-item-content>
                                      Un-Archive
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS34" @click.stop="fnRemoveWorkspace()">
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
                              <v-btn v-on="on" height="39" color="grey" outlined dark class="elevation-0 mr-4">
                                  <v-icon>mdi-filter-outline</v-icon>Filters
                              </v-btn>
                          </template>
                          <!--<v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                        <p class="title">Filter by :</p>
                        <p class="sub-title">Privacy</p>
                        <v-checkbox color="success-check-box" input-value="true" v-model="checkboxPublic" label="Public"></v-checkbox>
                        <v-checkbox color="success-check-box" input-value="true" v-model="checkboxPrivate" label="Private"></v-checkbox>
                        <p class="sub-title">Tags</p>
                        <div style="height: 18vh;" class="v-scrolling-div">
                            <div v-for="(WSTagitem,index) in wsTagColorItems" v-if="WSTagitem.TagID>0">
                                <v-checkbox color="success-check-box" input-value="WSTagitem.TagID" v-model="checkboxTags[index]" :label="WSTagitem.TagName"></v-checkbox>
                            </div>
                        </div>
                        <v-btn id="AutoTest_WS35" height="32" block color="success-btn" dark class="elevation-0 mr-2" @click="fnClickFilterDone()">
                            Done
                        </v-btn>
                    </v-list>-->
                      </v-menu>

                      <!--Sort by-->
                      <v-menu offset-y transition="scroll-y-transition" v-model="SortMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn min-width="40" height="39" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                  <v-icon>mdi-tune-vertical</v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                              <v-list-item>
                                  <v-list-item-content>
                                      Sort by :
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS36" @click.stop="fnWorkspaceSort('Name-Ascending')">
                                  <v-list-item-content>
                                      Name-Ascending
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS37" @click.stop="fnWorkspaceSort('Name-Descending')">
                                  <v-list-item-content>
                                      Name-Descending
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS38" @click.stop="fnWorkspaceSort('Recently Added')">
                                  <v-list-item-content>
                                      Recently Added
                                  </v-list-item-content>
                              </v-list-item>
                              <v-list-item id="AutoTest_WS39" @click.stop="fnWorkspaceSort('Most Viewed')">
                                  <v-list-item-content>
                                      Most Viewed
                                  </v-list-item-content>
                              </v-list-item>
                          </v-list>
                      </v-menu>
                  </v-row>
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
                              <v-btn id="AutoTest_WS35" height="32" block color="success-btn" dark class="elevation-0 mr-2" @click="fnClickFilterDone(),privacyMenu=false">
                                  Done
                              </v-btn>
                          </v-list>
                      </v-menu>


                      <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="tagDropdownMenu">
                          <template v-slot:activator="{ on }">
                              <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                  Tags
                                  <v-icon class="ml-12">
                                      mdi-chevron-down
                                  </v-icon>
                              </v-btn>
                          </template>
                          <v-list color="white" light="" width="auto" class="action-button-list pa-2">
                              <p class="sub-title">Filter by: Tags</p>
                              <div style="height: 15vh;" class="v-scrolling-div">
                                  <div v-for="(WSTagitem,index) in wsTagColorItems" v-if="WSTagitem.TagID>0">
                                      <v-checkbox input-value="WSTagitem.TagID" v-model="checkboxTags[index]" :label="[`${WSTagitem.TagName.length}`<=8 ? `${WSTagitem.TagName}` : `${WSTagitem.TagName.substring(0,8)+'..'}` ]" color="success-check-box"></v-checkbox>
                                  </div>
                              </div>
                              <v-btn id="AutoTest_WS35" color="success-btn" dark class="elevation-0 mt-2 btn-122x36" @click="fnClickFilterDone(),tagDropdownMenu = false">
                                  Done
                              </v-btn>
                          </v-list>
                      </v-menu>
                  </div>
              </v-col>
          </v-row>

          <!-- Filter chips-->
          <v-row>
              <v-col v-if="filterChipsList.length>0" cols="12" xs="12" sm="12" md="12">
                  <div class="chip-container mb-0">
                      <v-chip-group multiple column active-class="primary--text">
                          <v-chip id="AutoTest_WS40" close v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="ma-2" @click:close="fnChipClose(item)" small>
                              {{ item.ChipName }}
                          </v-chip>
                      </v-chip-group>
                  </div>
              </v-col>
          </v-row>

          <!--Workspace cards-->
          <v-row v-if="filterArchivedWorkspaceList.length>0">
              <v-col xs="6" sm="6" md="3" v-for="(WSitem,index) in filterArchivedWorkspaceList" :key="WSitem.WorkspaceID">
                  <v-card class="workspace-card" flat>
                      <v-row no-gutters>
                          <v-col md="12" class="text-right ml-3">
                              <span class="privacy" v-if="WSitem.WSIsPrivate">
                                  <v-icon size="12">mdi-lock</v-icon>
                                  Private
                              </span>
                              <span v-else class="privacy">
                                  <v-icon size="12">mdi-earth</v-icon>
                                  Public
                              </span>
                              <v-checkbox hide-details dense style="float:right" class="pt-0 mt-0 ml-2 mr-0" color="success-check-box" input-value="true" v-model="checkboxWorkspace[index]" label=""></v-checkbox>
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
                              <v-chip dark="" v-if="WSitem.WSTags.length>=3" label small class="tags tag-count">+{{WSitem.WSTags.length-2}}</v-chip>
                          </v-col>
                      </v-row>
                      <!--Team-->
                      <v-row>
                          <v-col class="py-3">
                              <span class="small-text mr-1">
                                  <v-icon size="12">mdi-account</v-icon> Team
                              </span>
                              <v-avatar size="27" class="team-avatar" v-for="usr in WSitem.WSUsers.slice(0, 3)" :color="`${getcolor(usr.Name)}`">
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
                              <v-btn id="AutoTest_WS41" color="success-btn elevation-0 enter-btn" height="30" depressed block dark @click="fnEnter(WSitem.WorkspaceID)">
                                  <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                  Enter
                              </v-btn>
                          </v-col>
                          <v-col sm="6">
                              <v-btn color="normal elevation-0" class="manage-btn" height="30" depressed block link :to="{ name: 'WorkspaceManage' , query: { WID: WSitem.WorkspaceID,Shared : false } }">
                                  <v-icon size="20" class="mr-2">mdi-settings</v-icon>
                                  Manage
                              </v-btn>
                          </v-col>
                      </v-row>
                  </v-card>
              </v-col>
          </v-row>

          <v-row v-if="filterArchivedWorkspaceList.length==0" justify="center">
              <v-col cols="12" md="12" class="text-center">
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
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'
    var objWorkspace;
    async function importscript() {
        return Promise.all([
            import("../../BL/Workspace.js").then(mod => {
                objWorkspace = new mod.Workspace();
            })
        ]);
    }
    export default {
        data: function () {
            return {
                PreLoader: false,
                snackbarMsg: false,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                checkboxPublic: false,
                checkboxPrivate: false,
                checkboxTags: [],
                filterChipsList: [],
                ArchivedWorkspaceList: [],
                filterArchivedWorkspaceList: [],
                checkboxWorkspace: [],
                BulkActionMenu: false,
                SortMenu: false,
                justifyCenter: 'center',
                wsTagColorItems: [],
                type: 'hex',
                hex: '#FF00FF',
                filterMenu: false,
                RandomColor: "",
                imageUrl: "",
                closeonClick: false,
                closeOnContentClick: false,
                privacyMenu: false,
                tagDropdownMenu: false,
            };
        },

        computed: {
            color: {
                get() {
                    return this[this.type]
                },
                set(v) {
                    this[this.type] = v
                },
            },
        },
        components:{
        'Pre-Loader': PreLoader,
        },
        
        mounted: async function () {
            this.imageUrl = window.SERVER_URL + "/Upload";
            this.RandomColor = sessionStorage.getItem("RandomColor");
            await importscript();
            await this.fnGetArchivedWorkspaceList();
            await this.GetWSTagDetails();
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
                        this.filterArchivedWorkspaceList.sort((a, b) => (a.WSName > b.WSName) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.filterArchivedWorkspaceList.sort((a, b) => (a.WSName < b.WSName) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.filterArchivedWorkspaceList.sort((a, b) => (a.WorkspaceID < b.WorkspaceID) ? 1 : -1);
                        break;
                    default:
                    // code block
                }
            },

            /// <summary>
            ///  fnUnArchiveWorkspace :- Function used to un-archive the selected workspaces
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnUnArchiveWorkspace() {
                this.BulkActionMenu = false;
                if (this.checkboxWorkspace.length == 0) {
                    this.Msg = ("Please select the workspaces you want to un-archive");
                    this.infoSnackbarMsg = true;
                    return;
                }

                var url = window.SERVER_URL + "/Workspace/UnArchiveWorkspace";
                var data = new FormData();
                var wsidlist = [];
                for (var i = 0; i < this.filterArchivedWorkspaceList.length; i++) {
                    if (this.checkboxWorkspace[i]) {
                        wsidlist.push(this.filterArchivedWorkspaceList[i].WorkspaceID);
                        //console.log("added=", this.filterArchivedWorkspaceList[i].WorkspaceID);
                    }
                }
                var wsids = wsidlist.toString();
                data.append("WorkspaceIdList", wsids);

                var result = await objWorkspace.PostData(url, data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = ("Successfully un-archived the selected workspaces");
                        this.successSnackbarMsg = true;
                        this.checkboxWorkspace = [];
                        this.fnGetArchivedWorkspaceList();
                        //clear all filter things also
                        this.checkboxTags = [];
                        this.checkboxPrivate = false;
                        this.checkboxPublic = false;
                        this.filterChipsList = [];
                    }
                }
            },

            /// <summary>
            ///  fnRemoveWorkspace :- Function used to remove the selected workspaces
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
                for (var i = 0; i < this.filterArchivedWorkspaceList.length; i++) {
                    if (this.checkboxWorkspace[i]) {
                        wsidlist.push(this.filterArchivedWorkspaceList[i].WorkspaceID);
                        //console.log("added=", this.filterArchivedWorkspaceList[i].WorkspaceID);
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
                        this.Msg = ("Successfully removed the selected workspaces");
                        this.successSnackbarMsg = true;
                        this.checkboxWorkspace = [];
                        this.fnGetArchivedWorkspaceList();
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
                    this.filterArchivedWorkspaceList = this.ArchivedWorkspaceList;
                else if (this.checkboxPrivate)
                    this.filterArchivedWorkspaceList = this.ArchivedWorkspaceList.filter(x => x.WSIsPrivate);
                else if (this.checkboxPublic) {
                    this.filterArchivedWorkspaceList = this.ArchivedWorkspaceList.filter(x => !x.WSIsPrivate);
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
                    for (var j = 0; j < this.filterArchivedWorkspaceList.length; j++) {
                        var wsadd = false;
                        for (var i = 0; i < tagidlist.length && !wsadd; i++) {
                            var tagid = tagidlist[i];
                            for (var k = 0; k < this.filterArchivedWorkspaceList[j].WSTags.length; k++) {
                                if (this.filterArchivedWorkspaceList[j].WSTags[k].TagID == tagid) {
                                    templist.push(this.filterArchivedWorkspaceList[j]);
                                    wsadd = true;
                                }
                            }
                        }
                    }
                    this.filterArchivedWorkspaceList = templist;
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
            fnEnter(wsid) {
                this.$router.push({ name: "home", query: { WsID: wsid } });
            },

            /// <summary>
            ///  fnGetArchivedWorkspaceList :- Function used to fetch archived workspace list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetArchivedWorkspaceList() {
              this.PreLoader = true;
                var url = window.SERVER_URL + "/Workspace/GetArchivedWorkspaceList";

                var result = await objWorkspace.GetData(url);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    //if (result.data.length > 0) {
                    this.ArchivedWorkspaceList = result.data;
                    this.filterArchivedWorkspaceList = result.data;
                    //} 
                    //alert("Archived Workspace list="+this.ArchivedWorkspaceList);
                }
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
                        var n = this.wsTagColorItems.length;
                        this.wsTagColorItems.push({ WSTagID: 0, TagName: 'Other' });
                    }
                }
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
        }
    };
</script>