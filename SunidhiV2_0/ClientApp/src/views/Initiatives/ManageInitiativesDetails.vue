<template>
    <div style="{height: 600px}">

        <v-col cols="12" md="12" sm="12" xs="12">
            <v-card class="px-4  elevation-0">
                <v-card-text class="pt-0">
                    <v-row no-gutters>
                        <v-col cols="12" xs="12" md="12">
                            <v-row :justify="justifyCenter">
                                <v-col md="4">
                                    <v-container fluid>
                                        <div v-if="editInitiativeEnableSave != true">
                                            <!--- Initiative Name-->
                                            <v-col class="pb-0">
                                                <div class="title pb-1 text-limit">
                                                    Initiative Name
                                                    <v-btn id="AutoTest_I138" text max-width="16" min-width="16" height="16" @click="fnEditInitiativeClick()">
                                                        <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                    </v-btn>
                                                </div>
                                                <P class="heading-3">
                                                    {{ initiativeInfo.InitiativeName }}
                                                </P>

                                                <!-- Initiative Decrtiption-->
                                                <div class="title pt-1 pb-1 text-limit">
                                                    Initiative Description
                                                    <v-btn id="AutoTest_I139" text max-width="16" min-width="16" height="16" @click="fnEditInitiativeClick()">
                                                        <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                    </v-btn>
                                                </div>
                                                <p class="text-left heading-3">
                                                    {{ initiativeInfo.Description }}
                                                </p>

                                                <!-- Code-->
                                                <div class="title pt-1 pb-0 text-limit">
                                                    Code
                                                    <!--<v-btn id="AutoTest_I140" text max-width="16" min-width="16" height="16" @click="fnEditInitiativeClick()">
                                                    <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                                </v-btn>-->
                                                </div>
                                                <p class="heading-3">
                                                    {{ initiativeInfo.Code }}
                                                </p>


                                            </v-col>


                                        </div>

                                        <!-- this division only visible when edit button is clicked-->
                                        <div v-if="editInitiativeEnableSave == true">
                                            <v-col class="pb-0">
                                                <v-text-field outlined dense label="Initiative Name" type="text" v-model="editInitiativeInfo.InitiativeName">
                                                </v-text-field>
                                                <v-textarea rows="3" outlined dense label="Description" type="text" v-model="editInitiativeInfo.Description">
                                                </v-textarea>
                                                <v-text-field outlined dense label="Code" v-model="editInitiativeInfo.Code" disabled>
                                                </v-text-field>
                                            </v-col>
                                        </div>

                                        <!-- Tag Section modified-->
                                        <v-row class="mb-1">
                                            <v-col class="text-left d-inline-flex" md="12">
                                                <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                                <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn id="AutoTest_I141" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                               @click="fnEditInitiativeClick(),fnInitializeSteper()" class="mr-1">
                                                            <v-icon size="15">mdi-plus</v-icon>
                                                        </v-btn>
                                                    </template>
                                                    <v-card>
                                                        <v-card-text class="pt-0 pb-1">
                                                            <v-col cols="12" md="12">
                                                                <!-- Header Text  -->
                                                                <v-col cols="12" md="12" class="d-inline-flex">
                                                                    <v-btn id="AutoTest_I142" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                        <v-icon size="15">mdi-chevron-left</v-icon>
                                                                    </v-btn>
                                                                    <v-spacer></v-spacer>
                                                                    <h3 class="text-left page-head pt-2">
                                                                        {{TagHeaderName}}
                                                                    </h3>
                                                                    <v-spacer></v-spacer>
                                                                    <v-btn id="AutoTest_I143" text @click="NewTagMenu = false">
                                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                    </v-btn>
                                                                </v-col>
                                                                <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                                    <v-stepper-items>

                                                                        <!-- Create New Tag Option when there is no Tags are -->
                                                                        <v-stepper-content step="1" class="pa-0">
                                                                            <v-row>
                                                                                <v-col class="pb-1 ml-2">
                                                                                    <v-text-field outlined type="text" dense
                                                                                                  v-model="NewTagName" hide-details>
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
                                                                            <v-row>
                                                                                <v-col cols="12" md="12" class="text-center">
                                                                                    <v-btn id="AutoTest_I144" color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>

                                                                        <!-- Select from existing Tags-->
                                                                        <v-stepper-content step="2" class="pa-0">

                                                                            <v-row style="max-height:30vh;overflow-y:scroll">
                                                                                <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                    <p class="mb-2">Search Tags</p>
                                                                                    <v-text-field outlined="" dense="" label="Search tag here" hide-details class="mb-3">
                                                                                    </v-text-field>
                                                                                    <v-chip-group multiple column active-class="primary--text" style="max-width:350px">
                                                                                        <v-chip id="AutoTest_I145" v-for="index in AllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                                label @click.native="fnAddToSelectedTag(index)">
                                                                                            {{ index.TagName }}
                                                                                            <v-icon id="AutoTest_I154" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                        </v-chip>
                                                                                    </v-chip-group>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-col cols="12" md="12" class="text-center mt-3">
                                                                                <v-btn id="AutoTest_I146" color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
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
                                                                            <v-row>
                                                                                <v-col cols="12" md="12" class="text-center">
                                                                                    <v-btn id="AutoTest_I147" color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-stepper-content>

                                                                    </v-stepper-items>
                                                                </v-stepper>
                                                            </v-col>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-menu>
                                                <div v-if="!allTags">
                                                    <!--<v-chip id="AutoTest_I148" v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                                    <v-chip v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                                                    <v-chip v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                                                    <v-chip v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
                                                </div>
                                                <v-btn id="AutoTest_I149" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                    <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                </v-btn>
                                                <v-btn id="AutoTest_I150" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                    <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                </v-btn>
                                            </v-col>
                                        </v-row>
                                        <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                            <v-col md="12" class="chip-container mb-0 pt-0">
                                                <v-chip-group v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                    <v-chip id="AutoTest_I151" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                                        {{TagItem.TagName}}
                                                    </v-chip>
                                                </v-chip-group>
                                            </v-col>
                                        </v-row>

                                        <!-- End Here -->
                                        <!-- Save and Cancel Buttons-->
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-4" v-if="editInitiativeEnableSave == true">
                                            <v-btn id="AutoTest_I152" color="success-btn" class="mr-4 elevation-0" @click="fnEditInitiativeSave()">Save</v-btn>
                                            <v-btn id="AutoTest_I153" outlined color="grey" @click="editInitiativeEnableSave=false,GetInitiativeTags()">Cancel</v-btn>
                                        </v-col>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-col>
                    </v-row>
                </v-card-text>
            </v-card>
        </v-col>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </div>
</template>

<script>
    var objInitiative;
    async function importscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
        ]);
    };
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                justifyCenter: 'center',
                //Details Tab
                WorkspaceID: 0,
                ManageInitiative: "",
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: "",
                    Description: "",
                    Code: "",
                    ThemeID: 0,
                    WorkspaceID: 0,
                    IsActive: false,
                    IsArchived: false,
                    IsDeleted: false,
                    CreatedBy: 0,
                },
                editInitiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: "",
                    Description: "",
                    Code: "",
                },
                editInitiativeEnableSave: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",

                isNewTag: false,
                allTags: false,
                newTagID: 0,
                TagIDList: "",

                TagList: [],
                initTagItems: [],
                selectedColor: false,

                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                type: 'hex',
                hex: '#FF00FF',
                filterMenu: false,
                TagMenu: false,


                // Altered Tag Menu
                AllTagListSteps: 0,
                NewTagListSteps: 0,
                TagHeaderName: "Add Tag",
                NewTagMenu: false,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,


            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitiativeID;
            if (this.$route.query.InitiativeType == 1) this.ManageInitiative = "Draft Initiative";
            if (this.$route.query.InitiativeType == 2) this.ManageInitiative = "Deployed Initiative";
            await importscript();
            await this.fnGetInitiativeDetailsByID();
            await this.fnGetAllTags();
        },
        methods: {

      
            //Details Tab functions starts here
            async fnGetInitiativeDetailsByID() {
            
                var data = new FormData();
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objInitiative.GetInitiativeDetailsByID(data);
         
         
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success" && result.data) {
                    this.initiativeInfo = result.data;
                } else {
                        this.Msg = ("Failed to get details of Initiative ", this.initiativeInfo.InitiativeId);
                        this.snackbarMsg = true;
                }
                this.GetInitiativeTags();
                this.allTags = false;
            },
            fnEditInitiativeClick() {
                this.editInitiativeEnableSave = true;
                this.editInitiativeInfo.InitiativeId = this.initiativeInfo.InitiativeId;
                this.editInitiativeInfo.InitiativeName = this.initiativeInfo.InitiativeName;
                this.editInitiativeInfo.Description = this.initiativeInfo.Description;
                this.editInitiativeInfo.Code = this.initiativeInfo.Code;
            },
              //Tag initiative tags 
            async GetInitiativeTags() {
                var data = new FormData();
                //const wsId = this.$route.query.WID; 
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objInitiative.GetInitiativeTags(data);
              
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.SelectedTag = result.data;
                }
            },
            async fnEditInitiativeSave() {
                var i = 0;
                if (this.editInitiativeInfo.InitiativeName == "") {
                    this.Msg = ("Initiative Name is required");
                    this.snackbarMsg = true;
                    return;
                }
                //for (i = 0; i < this.TagList.length; i++) {

                //    if (this.TagList[i].TagID == 0) {

                //        var dataTag = new FormData();
                //        dataTag.append("TagName", this.TagList[i].TagName);
                //        dataTag.append("TagColor", this.TagList[i].TagColor);
                //        var resultTag = await objInitiative.AddNewInitTag(dataTag);

                //        this.newTagID = resultTag.data;
                //        this.TagList[i].TagID = this.newTagID;
                //    }


                //    if (this.TagIDList == "")
                //        this.TagIDList = this.TagList[i].TagID;
                //    else
                //        this.TagIDList = this.TagIDList + "," + this.TagList[i].TagID;
                //}

                for (i = 0; i < this.SelectedTag.length; i++) {
                    if (this.TagIDList == "")
                        this.TagIDList = this.SelectedTag[i].TagID;
                    else
                        this.TagIDList = this.TagIDList + "," + this.SelectedTag[i].TagID;
                }
                var data = new FormData();
                data.append("InitiativeId", this.editInitiativeInfo.InitiativeId);
                data.append("InitiativeName", this.editInitiativeInfo.InitiativeName);
                data.append("Description", this.editInitiativeInfo.Description);
                data.append("Code", this.editInitiativeInfo.Code);
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("InitiativeTags", this.TagIDList);
                var result = await objInitiative.UpdateInitiativeDetails(data);
       
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                
                if (result.status == "success") {
                    this.Msg = ("Successfully updated the details of Initiative ", this.editInitiativeInfo.InitiativeId);
                    this.snackbarMsg = true;
                } else {
                    this.Msg = ("Failed to update details of Initiative ", this.editInitiativeInfo.InitiativeId);
                    this.snackbarMsg = true;
                }
                this.allTags = false;
                this.TagIDList = "";
                this.editInitiativeEnableSave = false;
                this.fnGetInitiativeDetailsByID();//refresh the details
            },
            //Details Tab functions ends here

            //============Tag =====================
            async GetInitTagDetails() {
               
                var result = await objInitiative.GetInitTagDetails();
         
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {

                        this.initTagItems = result.data;
                        this.initTagItems.push({ TagID: 0, TagName: 'Other' });

                        if (this.TagList.length > 0) {
                            for (var i = 0; i < this.TagList.length; i++) {
                                if (this.TagList[i].TagID != 0) {

                                    this.initTagItems = this.initTagItems.filter(x => x.TagID != this.TagList[i].TagID);
                                }
                            }

                        }
                    }
                
            },

            getTag(value) {

                if (value == 'Other') {
                    this.isNewTag = true;
                    this.tagColor = "";
                    this.tagName = "";

                    this.selectedColor = false;
                }
                else {

                    this.isNewTag = false;
                    var tagItem = this.initTagItems.filter(p => p.TagName == value);
                    this.tagID = tagItem[0].TagID;
                    this.tagColor = tagItem[0].TagColor;
                    this.tagName = tagItem[0].TagName;
                    this.selectedColor = true;

                }
            },
            addNewTag(tagColr, tagName) {
               
                if (tagName.length == 0 || tagName.trim() == '')
                    this.selectedColor = false;
                else {
                    this.tagColor = tagColr;
                    this.tagName = tagName;
                    this.selectedColor = true;
                }
            },

            fnAddTag(tagColr, tagName) {
                if (!this.isNewTag) {
                    this.TagList.push({ TagID: this.tagID, TagName: this.tagName, TagColor: this.tagColor });
                }
                else {
                    if (this.TagList.length >= 0) {
                        var oneitem = this.TagList.filter(x => x.TagName == tagName);
                    }
                    if (oneitem.length == 0) {
                        this.TagList.push({ TagID: 0, TagName: this.tagName, TagColor: this.tagColor });
                    }
                    else {
                        this.Msg = ("Tag name already exists");
                        this.snackbarMsg = true;
                    }
                }
                this.CloseTag();
            },
            async remove(item) {
                this.TagList.splice(this.TagList.indexOf(item), 1);
                this.TagList = [...this.TagList];
            },
            CloseTag() {

                this.vmTag = "";
                this.color = "";
                this.tagName = "";
                this.tagColor = "";
                this.tagID = "";
                this.isNewTag = false;
                this.selectedColor = false;
                this.AddInitTagDialog = false;
                this.TagMenu = false;

            },

        //Tag
        async fnInitializeSteper() {
            if (this.AllTagList.length == 0) {
                this.AllTagListSteps = 1;
            }
            else {
                this.AllTagListSteps = 2;
            }
        },
        async fnGetAllTags() {
            var data = new FormData();
            data.append("WorkspaceId", this.WorkspaceID);
            var result = await objInitiative.GetInitTagDetails(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            } else if (result.data) {

                this.AllTagList = result.data;
            } else {
                this.Msg = ("Failed to get tag list.");
                this.snackbarMsg = true;
            }
        },
        async fnEditTagDetails(TagItem) {
            this.AllTagListSteps = 3;
            this.TagHeaderName = 'Edit Tag Name';
            this.EditTagName = TagItem.TagName;
            this.color = TagItem.TagColor;
            this.UpdatingTagItem = TagItem;
        },
        async UpdateTagDetails(color, TagName) {
            if (this.UpdatingTagItem == "") {
                return;
            }
            var TagID = this.UpdatingTagItem.TagID;
            var data = new FormData();

            data.append("TagID", TagID);
            data.append("TagName", TagName);
            data.append("TagColor", color);

            var result = await objInitiative.UpdateInitTagDetails(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
                if (result.status == "success") {
                    this.TagHeaderName = 'Add Tag';
                    this.AllTagListSteps = 2;
                    this.fnGetAllTags();
                    this.UpdatingTagItem = "";
                }
        },
        async CreateNewTag(color, TagName) {
            var data = new FormData();
            data.append("TagName", TagName);
            data.append("TagColor", color);
            data.append("WorkspaceId", this.WorkspaceID);
            var result = await objInitiative.AddNewInitTag(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
            if (result.status == "success") {
                this.NewTagName = "";
                this.AllTagListSteps = 2;
                this.fnGetAllTags();
            }
            else {
                this.Msg = ("Tag already exists");
                this.snackbarMsg = true;
            }

        },
        async fnAddToSelectedTag(TagItem) {
            // check for existence . if Tag already exist do not add
            if (this.AllTagListSteps != 3) {
                if (this.SelectedTag.length > 0) {
                    var temp = this.SelectedTag.filter(x => x.TagID == TagItem.TagID);
                    if (temp.length == 0) {
                        this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                } else {
                    this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                }
                this.NewTagMenu = false;
            }
        },
        async fnRemoveFromSelectedTagItem(TagItem) {
            this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
            this.SelectedTag = [...this.SelectedTag];
            if (this.SelectedTag.length == 0) {
                this.allTags = false;
            }
        },
        CloseTag() {
            this.color = "";
            this.TagHeaderName = 'Add Tag';
            this.NewTagName = "";
            this.NewTagMenu = false;
            this.EditTagName = "";
            },
        },
  };
</script>