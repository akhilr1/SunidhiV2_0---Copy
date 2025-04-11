<template>
    <v-card class="elevation-0">
        <v-container fluid class="pt-0">
            <v-stepper hide v-model="ProjectTemplateSteps" class="elevation-0">
                <v-stepper-items>
                    <!-- Project Theme-->
                    <v-stepper-content step="1" class="pa-0">
                        <v-row>
                            <v-col>
                                <h3 class="text-left page-head">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> Selected Themes

                                </h3>
                            </v-col>
                        </v-row>
                        <v-col v-if="ThemeList.length>0" cols="12" xs="12" sm="12" md="12" class="pt-0">
                            <v-row class="text-right">
                                <v-spacer></v-spacer>
                                <!--Add Theme Button-->
                                <v-menu :offset-y=true>
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" height="32" width="160" color="success-btn" dark class="elevation-0 mr-2">
                                            Add Theme
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I155" link @click.stop="AddThemeDialog = true,fnGetAllProjectTheme()">
                                            <v-list-item-icon>
                                                <v-icon>mdi-plus-circle-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Add Theme</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Bulk Action -->
                                <v-menu v-bind:disabled="checkboxTheme.length==0" offset-y transition="scroll-y-transition" v-model="ThemeBulkActionMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" height="32" color="grey" outlined="" class="elevation-0 mr-2">
                                            Bulk Actions
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I156" link @click.stop="ThemeBulkRemoveDialog=true">
                                            <v-list-item-icon>
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Search-->
                                <div class="small-text-field">
                                    <v-text-field label="Search" class="mr-2" v-model="searchTheme"
                                                  dense prepend-inner-icon="mdi-magnify"
                                                  outlined></v-text-field>
                                </div>

                                <!-- Sorting Menu-->
                                <v-menu offset-y transition="scroll-y-transition" v-model="ThemeSortMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" min-width="40" height="32" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                            <v-icon>mdi-tune-vertical</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item>
                                            <v-list-item-content>
                                                Sort by :
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I157" @click.stop="fnThemeSort('Name-Ascending')">
                                            <v-list-item-content>
                                                Name-Ascending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I158" @click.stop="fnThemeSort('Name-Descending')">
                                            <v-list-item-content>
                                                Name-Descending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I159" @click.stop="fnThemeSort('Recently Added')">
                                            <v-list-item-content>
                                                Recently Added
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>
                            </v-row>
                        </v-col>

                        <!-- Theme Card Section-->
                        <!-- cards with themes -->
                        <v-row v-if="ThemeList.length>0">
                            <v-col xs="12" sm="12" md="12">
                                <v-card v-for="(themeItem,index) in filterThemeList" :key="themeItem.ThemeID" width="160px" class="d-inline-flex ma-2">
                                    <v-row no-gutters>
                                        <!-- Notification-->
                                        <v-col md="12" class="pb-0">
                                            <v-avatar color="#52B962" size="25" style="margin-top:5px" class="ma-2">
                                                <span class="white--text">{{ themeItem.ProjectTemplateList.length }}</span>
                                            </v-avatar>
                                            <v-checkbox color="#52B962" style="float:right;margin-top:5px"
                                                        label="" v-model="checkboxTheme[index]"></v-checkbox>
                                        </v-col>
                                        <!-- Button-->
                                        <v-col md="12" class="text-center">
                                            <v-btn id="AutoTest_I160" height="80" @click="ProjectTemplateSteps = 2, clickedTheme=themeItem,
                                                                    TemplateList=clickedTheme.ProjectTemplateList,
                                                                    filterTemplateList=clickedTheme.ProjectTemplateList" text>
                                                <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="80" width="80"></v-img>
                                                </v-avatar>
                                            </v-btn>
                                        </v-col>
                                        <!-- Text Name-->
                                        <v-col md="12" style="overflow:hidden">
                                            <p class="pt-1 px-1 text-limit text-center" style="width:155px">{{ themeItem.ThemeName }}</p>
                                        </v-col>
                                    </v-row>
                                </v-card>
                            </v-col>
                        </v-row>
                        <!-- cards no themes -->
                        <v-row v-else>
                            <v-col cols="12" sm="12" md="12">
                                <v-row :justify="justifyCenter">
                                    <v-col md="6" class="text-center">
                                        <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                            <v-img src="../../images/ic_no_workspace.svg" height="60" width="60"></v-img>
                                        </v-avatar>
                                        <p class="pt-1">No Themes available</p>
                                        <v-btn id="AutoTest_I161" height="32" width="160" color="success-btn" dark class="elevation-0 mr-2"
                                               @click="AddThemeDialog = true,fnGetAllProjectTheme()">
                                            Add Theme
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>

                        <!-- Add Theme Dailog Box-->
                        <v-dialog v-model="AddThemeDialog" width="900">
                            <v-card>
                                <v-card-title primary-title class="text-left page-head">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar>Add Theme
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_I162" text @click="AddThemeDialog = false,checkboxAddTheme=[]">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>

                                <v-card-text class="py-0">
                                    <v-container fluid class="pt-0">
                                        <v-col xs="12" sm="12" md="12">
                                            <v-row>
                                                <!-- Theme cards already present -->
                                                <v-card disabled v-for="(themeItem,index) in ThemeList" :key="themeItem.ThemeID" width="160px" class="d-inline-flex ma-2">
                                                    <v-row no-gutters>
                                                        <!-- Notification-->
                                                        <v-col md="12" class="pb-0 pt-0">
                                                            <v-checkbox value="true" color="#52B962" style="float:right;margin-top:5px"
                                                                        label=""></v-checkbox>
                                                        </v-col>
                                                        <!-- Button-->
                                                        <v-col md="12" class="text-center pt-0">
                                                            <v-btn id="AutoTest_I163" height="80" @click="" text>
                                                                <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="80" width="80"></v-img>
                                                                </v-avatar>
                                                            </v-btn>
                                                        </v-col>
                                                        <!-- Text Name-->
                                                        <v-col md="12" style="overflow:hidden">
                                                            <p class="pt-1 px-1 text-limit text-center" style="width:155px">{{ themeItem.ThemeName }}</p>
                                                        </v-col>
                                                    </v-row>
                                                </v-card>
                                                <!-- Theme cards to be added -->
                                                <v-card v-for="(themeItem,index) in AddThemeList" :key="themeItem.ThemeID" width="160px" class="d-inline-flex ma-2">
                                                    <v-row no-gutters>
                                                        <!-- Notification-->
                                                        <v-col md="12" class="pb-0">
                                                            <v-checkbox color="#52B962" style="float:right;margin-top:5px"
                                                                        label="" v-model="checkboxAddTheme[index]"></v-checkbox>
                                                        </v-col>
                                                        <!-- Button-->
                                                        <v-col md="12" class="text-center">
                                                            <v-btn id="AutoTest_I164" height="80" @click="" text>
                                                                <v-avatar size="80" tile class="elevation-0 base-border-radius">
                                                                    <v-img src="../../images/WorkSpace-dummy-icon.png" height="80" width="80"></v-img>
                                                                </v-avatar>
                                                            </v-btn>
                                                        </v-col>
                                                        <!-- Text Name-->
                                                        <v-col md="12" style="overflow:hidden">
                                                            <p class="pt-1 px-1 text-limit text-center" style="width:155px">{{ themeItem.ThemeName }}</p>
                                                        </v-col>
                                                    </v-row>
                                                </v-card>
                                            </v-row>
                                        </v-col>
                                    </v-container>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-2">
                                        <v-btn id="AutoTest_I165" v-bind:disabled="checkboxAddTheme.length==0" class="mr-2" color="secondary" @click="fnAddThemeConfirm()">Confirm</v-btn>
                                        <v-btn id="AutoTest_I166" outlined color="grey" @click="AddThemeDialog = false,checkboxAddTheme=[]">Cancel</v-btn>
                                    </v-col>
                                </v-card-text>
                                <v-divider></v-divider>
                            </v-card>
                        </v-dialog>

                        <!--Theme Bulk Remove Dialog-->
                        <v-dialog v-model="ThemeBulkRemoveDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Delete Themes from Initiative
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_I167" text="" @click="ThemeBulkRemoveDialog=false">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="pt-0">
                                    <v-container fluid="" class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">
                                                    Are you sure you want to remove themes from this initiative?
                                                </h4>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <v-btn id="AutoTest_I168" color="secondary" class="mr-2 elevation-0" @click="fnRemoveThemeFromInitiative()">Delete</v-btn>
                                                <v-btn id="AutoTest_I169" outlined color="grey" @click="ThemeBulkRemoveDialog=false">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                    </v-stepper-content>

                    <!-- Project Template-->
                    <v-stepper-content step="2" class="pa-0">
                        <!-- Link Buttons-->
                        <v-row>
                            <v-col>
                                <h3 class="text-left page-head">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> <span id="AutoTest_I170" style="cursor:pointer" @click="checkboxTemplate=[],ProjectTemplateSteps=1">Selected Themes</span>
                                    > {{ clickedTheme.ThemeName }}
                                </h3>
                            </v-col>
                        </v-row>

                        <!-- Menu Buttons-->
                        <v-col v-if="TemplateList.length>0" cols="12" xs="12" sm="12" md="12" class="text-right pt-5">
                            <v-row class="text-right">
                                <v-spacer></v-spacer>
                                <!--Add Template Button-->
                                <v-menu :offset-y=true>
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" height="32" width="160" color="success-btn" dark class="elevation-0 mr-2">
                                            Add Template
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I171" link @click.stop="AddTemplateDialog = true,AddTemplateSteps =1,fnGetAllProjectTemplateByThemeID()">
                                            <v-list-item-icon>
                                                <v-icon>mdi-plus-circle-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Add Template</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Bulk Action -->
                                <v-menu v-bind:disabled="checkboxTemplate.length==0" offset-y transition="scroll-y-transition" v-model="TemplateBulkActionMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" height="32" color="grey" outlined="" class="elevation-0 mr-2">
                                            Bulk Actions
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I172" link @click.stop="TemplateBulkRemoveDialog=true">
                                            <v-list-item-icon>
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Search-->
                                <div class="small-text-field">
                                    <v-text-field label="Search" class="mr-2" v-model="searchTemplate"
                                                  dense prepend-inner-icon="mdi-magnify"
                                                  outlined></v-text-field>
                                </div>

                                <!-- Sorting Menu-->
                                <v-menu offset-y transition="scroll-y-transition" v-model="TemplateSortMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-on="on" min-width="40" height="32" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                            <v-icon>mdi-tune-vertical</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item>
                                            <v-list-item-content>
                                                Sort by :
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I173" @click.stop="fnProjectTemplateSort('Name-Ascending')">
                                            <v-list-item-content>
                                                Name-Ascending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I174" @click.stop="fnProjectTemplateSort('Name-Descending')">
                                            <v-list-item-content>
                                                Name-Descending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I175" @click.stop="fnProjectTemplateSort('Recently Added')">
                                            <v-list-item-content>
                                                Recently Added
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>
                            </v-row>
                        </v-col>

                        <!-- Project Template cards section-->
                        <!-- cards with templates -->
                        <v-row v-if="TemplateList.length>0" style="overflow-y:scroll;height:50vh">
                            <v-col xs="3" sm="3" md="3" v-for="(PTitem,index) in filterTemplateList" :key="PTitem.ProjectTemplateID">
                                <v-card class="workspace-card" flat>
                                    <v-row no-gutters>
                                        <v-col md="12" class="pb-0">
                                            <v-img style="height:150px"
                                                   src="https://cdn.vuetifyjs.com/images/cards/docks.jpg">
                                                <v-checkbox color="#52B962" style="float:right;margin-top:10px"
                                                            v-model="checkboxTemplate[index]"
                                                            label=""></v-checkbox>
                                            </v-img>
                                            <v-col md="12" style="margin-top:-30px" class="mb-0">
                                                <v-btn class="ml-2" large color="#FFFFFF">
                                                    <v-icon size="40px" color="#2196F3">mdi-folder-open</v-icon>
                                                </v-btn>
                                            </v-col>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h3 class="title pt-2 ml-3">  {{ PTitem.ProjectTemplateName }} </h3>
                                            <p class="pb-0 ml-3" size="7">Created by ImWorks Team</p>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col class="ml-0" style="overflow-y:scroll">
                                            <p>{{ PTitem.Description }}</p>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters>
                                        <v-col md="6" class="text-center">
                                            <v-btn id="AutoTest_I176" color="success-btn elevation-0 enter-btn" block dark @click="ProjectTemplateSteps=3,ViewTemplateItem=PTitem">
                                                View Template
                                            </v-btn>
                                        </v-col>
                                    </v-row>
                                </v-card>
                            </v-col>
                        </v-row>
                        <!-- cards no templates -->
                        <v-row v-else>
                            <v-col cols="12" sm="12" md="12">
                                <v-row :justify="justifyCenter">
                                    <v-col md="6" class="text-center">
                                        <v-avatar size="60" tile class="elevation-1 base-border-radius pb-2">
                                            <v-img src="../../images/ic_no_workspace.svg" height="60" width="60" class="ma-4"></v-img>
                                        </v-avatar>
                                        <p class="pt-1 text-center">No templates available</p>
                                        <v-btn id="AutoTest_I177" height="32" width="160" color="success-btn" dark class="elevation-0"
                                               @click.stop="AddTemplateDialog = true,AddTemplateSteps =1,fnGetAllProjectTemplateByThemeID()">
                                            Add Template
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>

                        <!-- Add Template Dialog Box-->
                        <v-dialog v-model="AddTemplateDialog" width="900">
                            <v-card>
                                <v-col md="12">
                                    <v-card-title primary-title class="page-head pb-0">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-folder-key-network-outline</v-icon>
                                        </v-avatar>Add Template
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I178" text @click="AddTemplateDialog=false,checkboxAddTemplate=[],selectedAddTemplate=[]">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                </v-col>
                                <v-col class="pt-0 ml-5">
                                    <v-card-subtitle primary-title>
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-folder-key-network-outline</v-icon>
                                        </v-avatar><span id="AutoTest_I179" style="cursor:pointer" @click="AddTemplateSteps=1">{{ clickedTheme.ThemeName }}</span>
                                        <v-icon v-if="AddTemplateSteps==2" size="15" color="primary">mdi-chevron-double-right</v-icon>
                                        <span v-if="AddTemplateSteps==2">{{ ViewTemplateItem.ProjectTemplateName }}</span>
                                    </v-card-subtitle>
                                </v-col>
                                <v-card-text class="py-0">
                                    <v-stepper hide v-model="AddTemplateSteps" class="elevation-0">

                                        <v-col v-if="AddTemplateSteps==1" cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Please select templates from below.</h4>
                                        </v-col>
                                        <v-col v-else cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Please view the template.</h4>
                                        </v-col>
                                        <!-- Selected Template Chip Grup-->
                                        <v-col v-if="selectedAddTemplate.length>0" md="12" class="py-0">
                                            <div class="chip-container mb-2">
                                                <v-chip-group multiple column active-class="primary--text">
                                                    <v-chip id="AutoTest_I180" v-for="(chipitem,key) in selectedAddTemplate" :key="chipitem.PTitem.ProjectTemplateID"
                                                            @click:close="fnRemove(chipitem)" close class="ma-2" small>
                                                        <v-icon left color="#707070">mdi-email</v-icon>
                                                        {{ chipitem.PTitem.ProjectTemplateName }}
                                                    </v-chip>
                                                </v-chip-group>
                                            </div>
                                        </v-col>

                                        <v-stepper-items>
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-row style="overflow-y:scroll;height:50vh">
                                                    <!-- Template cards already present -->
                                                    <v-col xs="4" sm="4" md="4" v-for="(addTemplateItem,index) in TemplateList" :key="addTemplateItem.ProjectTemplateID">
                                                        <v-card disabled class="workspace-card" flat>
                                                            <v-row no-gutters>
                                                                <v-col md="12" class="pb-0">
                                                                    <v-img style="height:150px"
                                                                           src="https://cdn.vuetifyjs.com/images/cards/docks.jpg">
                                                                        <v-checkbox label="" color="#52B962" style="float:right;margin-top:10px"></v-checkbox>

                                                                    </v-img>
                                                                    <v-col md="12" style="margin-top:-30px" class="mb-0">
                                                                        <v-btn class="ml-2" large color="#FFFFFF">
                                                                            <v-icon size="40px" color="#2196F3">mdi-folder-open</v-icon>
                                                                        </v-btn>
                                                                    </v-col>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h3 class="title pt-2 ml-3">  {{ addTemplateItem.ProjectTemplateName }} </h3>
                                                                    <p class="pb-0 ml-3" size="7">Created by ImWorks Team</p>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col class="ml-0" style="overflow-y:scroll">
                                                                    <p>{{ addTemplateItem.Description }}</p>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col md="6" class="text-center">
                                                                    <v-btn id="AutoTest_I181" color="success-btn elevation-0 enter-btn" block dark @click="">
                                                                        View Template
                                                                    </v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card>
                                                    </v-col>
                                                    <!-- Template cards to be added -->
                                                    <v-col xs="4" sm="4" md="4" v-for="(addTemplateItem,index) in AddTemplateList" :key="addTemplateItem.ProjectTemplateID">
                                                        <v-card class="workspace-card" flat>
                                                            <v-row no-gutters>
                                                                <v-col md="12" class="pb-0">
                                                                    <v-img style="height:150px"
                                                                           src="https://cdn.vuetifyjs.com/images/cards/docks.jpg">
                                                                        <v-checkbox label="" color="#52B962" style="float:right;margin-top:10px"
                                                                                    v-model="checkboxAddTemplate[index]"
                                                                                    @change="fnCheck(addTemplateItem,index)"></v-checkbox>

                                                                    </v-img>
                                                                    <v-col md="12" style="margin-top:-30px" class="mb-0">
                                                                        <v-btn class="ml-2" large color="#FFFFFF">
                                                                            <v-icon size="40px" color="#2196F3">mdi-folder-open</v-icon>
                                                                        </v-btn>
                                                                    </v-col>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h3 class="title pt-2 ml-3">  {{ addTemplateItem.ProjectTemplateName }} </h3>
                                                                    <p class="pb-0 ml-3" size="7">Created by ImWorks Team</p>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col class="ml-0" style="overflow-y:scroll">
                                                                    <p>{{ addTemplateItem.Description }}</p>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col md="6" class="text-center">
                                                                    <v-btn id="AutoTest_I182" color="success-btn elevation-0 enter-btn" block dark @click="AddTemplateSteps=2,ViewTemplateItem=addTemplateItem">
                                                                        View Template
                                                                    </v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-1 py-1 mb-2">
                                                    <v-btn id="AutoTest_I183" v-bind:disabled="selectedAddTemplate.length==0" class="mr-2" color="secondary" @click="fnAddTemplateConfirm()">Confirm</v-btn>
                                                    <v-btn id="AutoTest_I184" outlined color="grey" @click="AddTemplateDialog=false,checkboxAddTemplate=[],selectedAddTemplate=[]">Cancel</v-btn>
                                                </v-col>
                                            </v-stepper-content>
                                            <!-- View Template Section-->
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-card class="px-4">
                                                    <v-card-text class="pt-0">
                                                        <v-row :justify="justifyCenter">
                                                            <v-col cols="12" md="12">
                                                                <v-container fluid style="overflow-y :scroll;overflow-x:hidden">
                                                                    <v-row no-gutters>
                                                                        <v-col xs="12" sm="12" md="12">
                                                                            <v-col md="12" class="d-inline-flex pt-0">
                                                                                <v-btn id="AutoTest_I185" text width="auto" height="auto" @click="">
                                                                                    <v-avatar size="50" tile class="elevation-1 base-border-radius mr-2">
                                                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                                                    </v-avatar>
                                                                                    <p>
                                                                                        <br /> <span class="title pt-0 pb-0 text-limit"></span>{{ ViewTemplateItem.ProjectTemplateName }} <br />
                                                                                        <span size="5"> Created by ImWorks Team </span>
                                                                                    </p>
                                                                                </v-btn>
                                                                                <v-spacer></v-spacer>
                                                                                <v-btn color="success-btn elevation-0 enter-btn" dark>
                                                                                    View Template
                                                                                </v-btn>
                                                                            </v-col>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-col>
                                                                        <p class="title pt-0 pb-0 text-limit">About Template </p>
                                                                        <p class="text-left heading-3">{{ ViewTemplateItem.Description }}</p>
                                                                    </v-col>
                                                                    <v-col class="title pt-1 pb-1 text-limit">
                                                                        Use Cases
                                                                    </v-col>
                                                                </v-container>
                                                            </v-col>
                                                        </v-row>
                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-1 py-1 mb-2">
                                                            <v-btn id="AutoTest_I186" v-bind:disabled="selectedAddTemplate.length==0" class="mr-2" color="secondary" @click="fnAddTemplateConfirm()">Confirm</v-btn>
                                                            <v-btn id="AutoTest_I187" outlined color="grey" @click="AddTemplateDialog=false,checkboxAddTemplate=[],selectedAddTemplate=[]">Cancel</v-btn>
                                                        </v-col>
                                                    </v-card-text>
                                                </v-card>
                                            </v-stepper-content>

                                        </v-stepper-items>
                                    </v-stepper>
                                </v-card-text>
                                <v-divider></v-divider>
                            </v-card>
                        </v-dialog>

                        <!--Template Bulk Remove Dialog-->
                        <v-dialog v-model="TemplateBulkRemoveDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Delete Templates from Theme
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_I188" text="" @click="TemplateBulkRemoveDialog=false">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="pt-0">
                                    <v-container fluid="" class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">
                                                    Are you sure you want to remove templates from this theme?
                                                </h4>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <v-btn id="AutoTest_I189" color="secondary" class="mr-2 elevation-0" @click="fnRemoveTemplateFromInitiative()">Delete</v-btn>
                                                <v-btn id="AutoTest_I190" outlined color="grey" @click="TemplateBulkRemoveDialog=false">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                    </v-stepper-content>

                    <!-- View Project Template -->
                    <v-stepper-content step="3" class="pa-0">
                        <v-card class="px-4">
                            <v-card-text class="pt-0">
                                <v-row no-gutters>
                                    <v-col cols="12" xs="12" md="12">
                                        <v-row :justify="justifyCenter">
                                            <v-col cols="12" md="12">
                                                <h4 class="text-left heading-1">
                                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                                    </v-avatar> <span id="AutoTest_I191" style="cursor:pointer" @click="checkboxTemplate=[],ProjectTemplateSteps=1">Selected Themes</span>
                                                    <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon><span id="AutoTest_I192" style="cursor:pointer" @click="ProjectTemplateSteps=2"> {{ clickedTheme.ThemeName }} </span>
                                                    <v-icon size="15" color="primary">mdi-chevron-double-right</v-icon>{{ ViewTemplateItem.ProjectTemplateName }}
                                                </h4>
                                            </v-col>

                                            <v-col cols="6" md="6">
                                                <v-container fluid style="overflow-y :scroll">
                                                    <!-- Heading with enter Button-->
                                                    <v-row no-gutters>
                                                        <v-col xs="12" sm="12" md="12">
                                                            <v-col md="8" class="d-inline-flex pt-0">
                                                                <v-btn id="AutoTest_I193" text width="auto" height="auto" @click="">
                                                                    <v-avatar size="50" tile class="elevation-1 base-border-radius mr-2">
                                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                                    </v-avatar>
                                                                    <p class="text-left mt-2">
                                                                        <br />{{ ViewTemplateItem.ProjectTemplateName }} <br />
                                                                        <span size="6"> Created by ImWorks Team </span>
                                                                    </p>
                                                                </v-btn>
                                                            </v-col>
                                                        </v-col>
                                                    </v-row>
                                                    <v-col>
                                                        <p class="title pt-0 pb-0 text-limit">About Template </p>
                                                        <p class="text-left heading-3">{{ ViewTemplateItem.Description }}</p>
                                                    </v-col>

                                                    <v-col class="title pt-1 pb-1 text-limit">
                                                        Use Cases
                                                    </v-col>
                                                </v-container>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                        </v-card>
                    </v-stepper-content>
                </v-stepper-items>
            </v-stepper>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>

<script>
    var objInitiativeTemplate;
    async function importscript() {
        return Promise.all([
            import("../../BL/InitiativeTemplate.js").then(mod => {
                objInitiativeTemplate = new mod.InitiativeTemplate();
            })
        ]);
    };
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                InitiativeId: 0,
                justifyCenter: 'center',
                ProjectTemplateSteps: 1,

                //Step 1
                ThemeList: [],
                filterThemeList: [],
                checkboxTheme: [],
                ThemeBulkActionMenu: false,
                ThemeBulkRemoveDialog: false,
                searchTheme: "",
                ThemeSortMenu: false,
                //Add Theme dialog variables
                AddThemeDialog: false,
                checkboxAddTheme: [],
                AddThemeList: [],

                //Step 2
                clickedTheme: {
                    ThemeID: 0, ThemeName: "", ProjectTemplateList: []
                },
                TemplateList: [],
                filterTemplateList: [],
                checkboxTemplate: [],
                TemplateBulkActionMenu: false,
                TemplateBulkRemoveDialog: false,
                searchTemplate: "",
                TemplateSortMenu: false,
                //Add Template dialog variables
                AddTemplateDialog: false,
                AddTemplateSteps: 0,
                AddTemplateList: [],
                checkboxAddTemplate: [],
                selectedAddTemplate: [],

                //step 3
                ViewTemplateItem: { ThemeID: 0, ProjectTemplateID: 0, ProjectTemplateName: "", Description: "" },
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeId = this.$route.query.InitiativeID;
            await importscript();
            await this.fnGetProjectThemeByIntitiativeID();
        },
        methods: {
            //step 1 Theme related functions
            async fnGetProjectThemeByIntitiativeID() {
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                var result = await objInitiativeTemplate.GetProjectThemeByIntitiativeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ThemeList = result.data;
                    this.filterThemeList = result.data;
                    
                    if (this.ProjectTemplateSteps == 2) {
                        var temp = this.filterThemeList.filter(x => x.ThemeID == this.clickedTheme.ThemeID);
                        if (temp[0].ProjectTemplateList.length > 0) {
                            this.clickedTheme = temp[0];
                            this.TemplateList = this.clickedTheme.ProjectTemplateList;
                            this.filterTemplateList = this.clickedTheme.ProjectTemplateList;
                        } else {//all templates were removed
                            this.clickedTheme.ThemeID = 0;
                            this.clickedTheme.ThemeName = "";
                            this.clickedTheme.ProjectTemplateList = [];
                            this.ProjectTemplateSteps = 1;
                        }
                    }
                }
            },
            async fnRemoveThemeFromInitiative() {
                this.ThemeBulkActionMenu = false;
                this.ThemeBulkRemoveDialog = false;
                if (this.checkboxTheme.length == 0) {
                    this.Msg = ("Please select the themes you want to remove");
                    this.snackbarMsg = true;
                    return;
                }

                var data = new FormData();
                var themeidlist = [];
                for (var i = 0; i < this.filterThemeList.length; i++) {
                    if (this.checkboxTheme[i]) {
                        themeidlist.push(this.filterThemeList[i].ThemeID);
                    }
                }
                var themeids = themeidlist.toString();
                data.append("ThemeIDList", themeids);
                data.append("InitiativeId", this.InitiativeId);

                var result = await objInitiativeTemplate.RemoveProjectThemeFromInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = ("Successfully removed");
                    this.snackbarMsg = true;
                    this.checkboxTheme = [];
                    this.fnGetProjectThemeByIntitiativeID();
                }
            },
            fnThemeSort(type) {
                this.ThemeSortMenu = false;
                switch (type) {
                    case "Name-Ascending":
                        this.filterThemeList.sort((a, b) => (a.ThemeName > b.ThemeName) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.filterThemeList.sort((a, b) => (a.ThemeName < b.ThemeName) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.filterThemeList.sort((a, b) => (a.ThemeID < b.ThemeID) ? 1 : -1);
                        break;
                    default:
                    // code block
                }
            },
            async fnGetAllProjectTheme() {
                var result = await objInitiativeTemplate.GetAllProjectTheme();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AddThemeList = result.data;
                    for (var i = 0; i < this.ThemeList.length; i++) {
                        var oneitem = this.ThemeList[i];
                        this.AddThemeList = this.AddThemeList.filter(x => x.ThemeID != oneitem.ThemeID);
                    }
                    //alert("AllThemeList="+this.AddThemeList.length);
                }
            },
            async fnAddThemeConfirm() {
                if (this.checkboxAddTheme.length == 0) {
                    this.Msg = ("Please select the themes you want to add");
                    this.snackbarMsg = true;
                    return;
                }

                var data = new FormData();
                var themeidlist = [];
                for (var i = 0; i < this.AddThemeList.length; i++) {
                    if (this.checkboxAddTheme[i]) {
                        themeidlist.push(this.AddThemeList[i]);
                    }
                }
                data.append("ThemeIDList", JSON.stringify(themeidlist));
                data.append("InitiativeId", this.InitiativeId);

                var result = await objInitiativeTemplate.AddProjectThemeToInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = ("Successfully added the themes you slected");
                    this.snackbarMsg = true;
                    this.AddThemeDialog = false;
                    this.AddThemeList = [];
                    this.checkboxAddTheme = [];
                    this.fnGetProjectThemeByIntitiativeID();
                }
            },

            //step 2 Template related functions
            fnCheck(item, index) {
                if (this.checkboxAddTemplate[index]) {
                    this.selectedAddTemplate.push({ checkboxindex: index, PTitem: item });
                } else {
                    if (this.selectedAddTemplate.length > 0) {
                        this.selectedAddTemplate = this.selectedAddTemplate.filter(x => x.PTitem.ProjectTemplateID != item.ProjectTemplateID);
                    }
                }
            },
            fnRemove(item) {
                this.checkboxAddTemplate[item.checkboxindex] = false;
                this.selectedAddTemplate = this.selectedAddTemplate.filter(x => x.PTitem.ProjectTemplateID != item.PTitem.ProjectTemplateID);
            },
            async fnRemoveTemplateFromInitiative() {
                this.TemplateBulkActionMenu = false;
                this.TemplateBulkRemoveDialog = false;
                if (this.checkboxTemplate.length == 0) {
                    this.Msg = ("Please select the templates you want to remove");
                    this.snackbarMsg = true;
                    return;
                }

                var data = new FormData();
                var templateidlist = [];
                for (var i = 0; i < this.filterTemplateList.length; i++) {
                    if (this.checkboxTemplate[i]) {
                        templateidlist.push(this.filterTemplateList[i].ProjectTemplateID);
                    }
                }
                var templateids = templateidlist.toString();

                var data = new FormData();
                data.append("TemplateIDList", templateids);
                data.append("InitiativeId", this.InitiativeId);

                var result = await objInitiativeTemplate.RemoveProjectTemplateFromInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = ("Successfully removed");
                    this.snackbarMsg = true;
                    this.checkboxTemplate = [];
                    this.fnGetProjectThemeByIntitiativeID();
                }
            },
            fnProjectTemplateSort(type) {
                this.TemplateSortMenu = false;
                switch (type) {
                    case "Name-Ascending":
                        this.filterTemplateList.sort((a, b) => (a.ProjectTemplateName > b.ProjectTemplateName) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.filterTemplateList.sort((a, b) => (a.ProjectTemplateName < b.ProjectTemplateName) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.filterTemplateList.sort((a, b) => (a.ProjectTemplateID < b.ProjectTemplateID) ? 1 : -1);
                        break;
                    default:
                    // code block
                }
            },
            async fnGetAllProjectTemplateByThemeID() {
                var data = new FormData();
                data.append("ThemeID", this.clickedTheme.ThemeID);
                var result = await objInitiativeTemplate.GetAllProjectTemplateByThemeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AddTemplateList = result.data;
                    for (var i = 0; i < this.TemplateList.length; i++) {
                        var oneitem = this.TemplateList[i];
                        this.AddTemplateList = this.AddTemplateList.filter(x => x.ProjectTemplateID != oneitem.ProjectTemplateID);
                    }
                    //alert("AddTemplateList=" + this.AddTemplateList.length);
                }
            },
            async fnAddTemplateConfirm() {
                if (this.selectedAddTemplate.length == 0) {
                    this.Msg = ("Please select the templates you want to add");
                    this.snackbarMsg = true;
                    return;
                }

                var templatelist = [];
                for (var i = 0; i < this.selectedAddTemplate.length; i++) {
                    templatelist.push(this.selectedAddTemplate[i].PTitem);
                }
                var data = new FormData();
                data.append("TemplateList", JSON.stringify(templatelist));
                data.append("InitiativeId", this.InitiativeId);
                data.append("ThemeID", this.clickedTheme.ThemeID);

                var result = await objInitiativeTemplate.AddProjectTemplateToInitiative(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = ("Successfully added the templates you slected");
                    this.snackbarMsg = true;
                    this.AddTemplateDialog = false;
                    this.AddTemplateSteps = 1;
                    this.AddTemplateList = [];
                    this.checkboxAddTemplate = [];
                    this.selectedAddTemplate = [];
                    this.fnGetProjectThemeByIntitiativeID();
                }
            },

            //step 3 View Template functions
            fnInitializeViewTemplateItem() {
                this.ViewTemplateItem.ThemeID = 0;
                this.ViewTemplateItem.ProjectTemplateID = 0;
                this.ViewTemplateItem.ProjectTemplateName = "";
                this.ViewTemplateItem.Description = "";
            },
        },
        watch: {
            searchTheme: function () {
                this.checkboxTheme = [];
                if (this.searchTheme == "") this.filterThemeList = this.ThemeList;
                else this.filterThemeList = this.ThemeList.filter(x => x.ThemeName.includes(this.searchTheme));
            },
            searchTemplate: function () {
                this.checkboxTemplate = [];
                if (this.searchTemplate == "") this.filterTemplateList = this.TemplateList;
                else this.filterTemplateList = this.TemplateList.filter(x => x.ProjectTemplateName.includes(this.searchTemplate));
            },
        }
  };
</script>