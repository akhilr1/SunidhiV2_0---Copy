<template>
    <v-card class="elevation-0">
        <v-row justify="center" no-gutters class="my-0 py-0">
            <v-col md="10" class="my-0 py-0">
                <v-card-title class="px-1 py-1 my-0">
                    <v-row no-gutters>
                        <v-col md="auto" class="d-none d-md-block">
                            <h6 class="text-left page-head">
                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                    <v-icon size="15" color="primary">mdi-file-document-box-check-outline</v-icon>
                                </v-avatar>
                                Overdue Tasks
                            </h6>
                        </v-col>
                        <v-spacer class="d-none d-md-block"></v-spacer>
                        <v-col md="auto" xs="6">
                            <!--Search-->
                            <v-text-field label="Enter" class="mr-4"
                                          dense="" prepend-inner-icon="mdi-magnify" v-model="searchProject"  @change ="searchval = searchProject"
                                          outlined="" hide-details></v-text-field>
                        </v-col>
                        <v-col md="auto" cols="auto">
                            <!--Filter button-->
                            <v-menu :close-on-content-click="false" offset-y="" transition="scroll-y-transition">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on" height="39" width="90" outlined="" dense="" class="outlined-btn-dark d-none d-md-inline mr-4" @click="filterContent = !filterContent">
                                        <v-icon>mdi-filter-outline</v-icon>
                                        <span>Filters</span>
                                    </v-btn>
                                    <v-btn v-on="on" height="39" outlined="" dense="" class="outlined-btn-dark d-sm-none" @click="mobileFilterContent = !mobileFilterContent">
                                        <v-icon>mdi-filter-outline</v-icon>
                                    </v-btn>
                                </template>
                            </v-menu>
                        </v-col>
                        <v-col md="auto" cols="auto">
                            <!--Reset Filter button-->
                            <v-menu :close-on-content-click="false" offset-y="" transition="scroll-y-transition">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on" max-width="29" min-width="40" height="39" outlined="" dense="" class="outlined-btn-dark d-none d-md-inline" @click="resetFilter()">
                                        <v-icon>mdi-filter-remove-outline</v-icon>
                                        <!--<span>Reset</span>-->
                                    </v-btn>
                                </template>
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-card-title>
            </v-col>
        </v-row>
        <!--filter options-->
        <v-row justify="center" v-if="filterContent">
            <v-col md="10" class="d-none d-md-inline">
                <v-row no-gutters>
                    <v-col md="auto" cols="auto">
                        <h4 class="text-left page-head pr-3 my-1">
                            Filters
                        </h4>
                    </v-col>
                    <!--Date range-->
                    <v-col md="auto" cols="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="dateRangeMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    {{dateRangeHeaderName}} <!--dateRangeHeaderName-->
                                    <v-icon class="ml-4">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="900" min-width="50" class="action-button-list pa-2">
                                <v-row justify="center" no-gutters>
                                    <v-col cols="4" class="px-2">
                                        <h3 style="text-align:center">Start Date</h3>
                                        <v-date-picker color="primary" no-title @input="IsValidDate()"
                                                       class="elevation-0" v-model="checkStartDate"></v-date-picker>
                                    </v-col>
                                    <v-col cols="4" class="px-2">
                                        <h3 style="text-align:center">End Date</h3>
                                        <v-date-picker color="primary" no-title @input="IsValidDate()"
                                                       class="elevation-0" v-model="checkEndDate" :min="checkStartDate"></v-date-picker>
                                    </v-col>
                                    <v-col cols="4">
                                        <v-list class="px-2 mx-4">
                                            <v-header style="font-size:large" class="pl-4">Predefined Periods:</v-header>
                                            <v-list-item-group v-model="selectedItem"
                                                               color="primary">
                                                <v-list-item v-for="(item, i) in items"
                                                             :key="i">
                                                    <v-list-item-content>
                                                        <v-list-item-title v-text="item.text"></v-list-item-title>
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </v-list-item-group>
                                        </v-list>
                                    </v-col>
                                </v-row>
                                <v-divider></v-divider>
                                <v-row justify="center" class="px-2">
                                    <v-col cols="2">
                                        <v-btn height="32" block="" class="elevation-0 " @click="dateRangeMenu = false">
                                            <!--fnClickDateRangeCancel('Date Range')-->
                                            cancel
                                        </v-btn>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col cols="2">
                                        <v-btn height="32" block="" color="primary" class="elevation-0" @click="fnFilterDone(), dateRangeMenu = false">
                                            Done <!--fnFilterDone('Date Range'),-->
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-list>
                        </v-menu>
                    </v-col>
                    <!--Task-->
                    <v-col md="auto" cols="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="taskMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Task
                                    <v-icon class="ml-12">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="155" class="action-button-list pa-2">
                                <v-checkbox color="success-check-box" v-model="checkOutput" input-value="true" label="Output"></v-checkbox>
                                <v-checkbox color="success-check-box" v-model="checkOutcome" input-value="true" label="Outcome"></v-checkbox>
                                <v-checkbox color="success-check-box" v-model="checkRisk" input-value="true" label="Risk"></v-checkbox>
                                <v-checkbox color="success-check-box" v-model="checkSurvey" input-value="true" label="Survey"></v-checkbox>
                                <v-checkbox color="success-check-box" v-model="checkPayment" input-value="true" label="Payment"></v-checkbox>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="taskMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), taskMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                    <!--Project-->
                    <v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="projectMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Project
                                    <v-icon class="ml-10">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="155" class="action-button-list pa-2">
                                <div style="height: 18vh;" class="v-scrolling-div">
                                    <v-list-tile v-for="(item,index) in ProjectTableItems" :key="index">
                                        <v-checkbox color="success-check-box" v-model="selectedProjectID[index]" input-value="true" :label="[`${item.ProjectName.length}`<=10 ? `${item.ProjectName}` : `${item.ProjectName.substring(0,10)+'..'}`]"></v-checkbox><!--`${item.ProjectName}`-->
                                    </v-list-tile>
                                </div>
                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="projectMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(),projectMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                    <!--Beneficiary Type-->
                    <v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="beneficiaryTypeMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Beneficiary Type
                                    <v-icon class="ml-10">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" class="action-button-list pa-2">
                                <div style="height: 18vh;" class="v-scrolling-div">
                                    <v-list-tile v-for="(item,index) in lstBType" :key="index">
                                        <v-checkbox color="success-check-box" v-model="selectedBeneficiaryTypeID[index]" input-value="true" :label="[`${item.Name.length}`<=10 ? `${item.Name}` : `${item.Name.substring(0,10)+'..'}`]"></v-checkbox>
                                    </v-list-tile>
                                </div>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="beneficiaryTypeMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), beneficiaryTypeMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                    <!--Workflow-->
                    <v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="workflowMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Workflow
                                    <v-icon class="ml-12">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                <div style="height: 18vh;" class="v-scrolling-div">
                                    <v-list-tile v-for="(WFitem, index) in lstProjectWorkflows" :key="index">
                                        <v-checkbox color="success-check-box" v-model="selectedcheckboxWorkflow[index]" input-value="true" :label="[`${WFitem.WfName.length}`<=10 ? `${WFitem.WfName}` : `${WFitem.WfName.substring(0,10)+'..'}`]"></v-checkbox>
                                    </v-list-tile>
                                </div>
                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="workflowMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), workflowMenu = false,SaveSelectedFilter(SelectedFilter)">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                    <!--Action-->
                    <v-col md="auto">
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="actionMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                    Action
                                    <v-icon class="ml-10">
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" class="action-button-list pa-2">
                                <v-checkbox color="success-check-box" v-model="checkCollect" input-value="true" label="To Collect"></v-checkbox>
                                <v-checkbox color="success-check-box" v-model="checkApprove" input-value="true" label="To Approve"></v-checkbox>

                                <div class="d-inline-flex">
                                    <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="actionMenu = false">
                                        Cancel
                                    </v-btn>
                                    <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), actionMenu = false">
                                        Done
                                    </v-btn>
                                </div>
                            </v-list>
                        </v-menu>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
        <!--filter chips-->
        <v-row v-if="filterChipsList.length>0" justify="center" no-gutters class="pa-0">
            <v-col md="10" class="d-none d-md-inline">
                <div class="chip-container ma-0">
                    <v-chip-group multiple column active-class="primary--text">
                        <v-chip id="" v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="mx-2" medium label>
                            <v-icon small left @click="fnChipClose(item)">mdi-close</v-icon>{{ item.ChipName }}
                        </v-chip>
                    </v-chip-group>
                </div>
            </v-col>
        </v-row>
        <!--filter dialog options for small screen-->
        <v-dialog v-model="mobileFilterContent" width="800" transition="dialog-bottom-transition">
            <v-card style="height:85vh">
                <v-card-title primary-title class="page-head heading-3 pb-0">
                    Filter
                </v-card-title>
                <v-divider class="mt-3"></v-divider>
                <v-card-text>
                    <v-row style="height:65vh">
                        <v-col>
                            <!--Date range-->
                            <v-col md="auto" cols="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="dateRangeMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            {{dateRangeHeaderName}}<!--Date range-->
                                            <v-icon class="ml-10">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="900" min-width="50" class="action-button-list pt-10" style="height:50vh">
                                        <v-row justify="center" no-gutters>
                                            <v-col cols="12">
                                                <h3 style="text-align:center">Start Date</h3>
                                                <v-date-picker color="primary" no-title @input="IsValidDate()"
                                                               class="elevation-0" v-model="checkStartDate"></v-date-picker>
                                            </v-col>
                                            <v-col cols="12">
                                                <h3 style="text-align:center">End Date</h3>
                                                <v-date-picker color="primary" no-title @input="IsValidDate()"
                                                               class="elevation-0" v-model="checkEndDate" :min="checkStartDate"></v-date-picker>
                                            </v-col>
                                            <v-col cols="12">
                                                <v-list>
                                                    <v-header style="font-size:large" class="pl-4">Predefined Periods:</v-header>
                                                    <v-list-item-group v-model="selectedItem"
                                                                       color="primary">
                                                        <v-list-item v-for="(item, i) in items"
                                                                     :key="i">
                                                            <v-list-item-content>
                                                                <v-list-item-title v-text="item.text"></v-list-item-title>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list-item-group>
                                                </v-list>
                                            </v-col>
                                        </v-row>
                                        <v-divider></v-divider>
                                        <v-row justify="center" class="px-2">
                                            <v-col cols="4">
                                                <v-btn height="32" x-large block="" class="elevation-0 " @click="dateRangeMenu = false">
                                                    cancel <!--fnClickDateRangeCancel('Date Range'), -->
                                                </v-btn>
                                            </v-col>
                                            <v-spacer></v-spacer>
                                            <v-col cols="4">
                                                <v-btn height="32" x-large block="" color="primary" class="elevation-0" @click="fnFilterDone(), dateRangeMenu = false">
                                                    Done <!--fnFilterDone('Date Range')-->
                                                </v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-list>
                                </v-menu>
                            </v-col>
                            <!--Task-->
                            <v-col md="auto" cols="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="taskMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Task
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="155" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <v-checkbox color="success-check-box" v-model="checkOutput" input-value="true" label="Output"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkOutcome" input-value="true" label="Outcome"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkRisk" input-value="true" label="Risk"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkSurvey" input-value="true" label="Survey"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkPayment" input-value="true" label="Payment"></v-checkbox>

                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="taskMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), taskMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
                                    </v-list>
                                </v-menu>
                            </v-col>
                            <!--Project-->
                            <v-col md="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="projectMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Project
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list">
                                        <div class="v-scrolling-div" style="height:20vh">
                                            <v-list-item v-for="(item,index) in ProjectTableItems" :key="index">
                                                <v-checkbox color="success-check-box" v-model="selectedProjectID[index]" input-value="true" :label="item.ProjectName"></v-checkbox>

                                            </v-list-item>
                                        </div>

                                        <v-row no-gutters class="pa-2">
                                            <v-col cols="4">
                                                <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0" @click="projectMenu = false">
                                                    Cancel
                                                </v-btn>
                                            </v-col>
                                            <v-spacer></v-spacer>
                                            <v-col cols="4">
                                                <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(),projectMenu = false">
                                                    Done
                                                </v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-list>
                                </v-menu>
                            </v-col>
                            <!--Beneficiary Type-->
                            <v-col md="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="beneficiaryTypeMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Beneficiary Type
                                            <v-icon class="ml-10">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <div style="height: 18vh;" class="v-scrolling-div">
                                            <v-list-tile v-for="(item,index) in lstBType" :key="index">
                                                <v-checkbox color="success-check-box" v-model="selectedBeneficiaryTypeID[index]" input-value="true" :label="[`${item.Name.length}`<=10 ? `${item.Name}` : `${item.Name.substring(0,10)+'..'}`]"></v-checkbox>
                                            </v-list-tile>
                                        </div>

                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="beneficiaryTypeMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), beneficiaryTypeMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
                                    </v-list>
                                </v-menu>
                            </v-col>
                            <!--Workflow-->
                            <v-col md="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="workflowMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Workflow
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <div style="height: 18vh;" class="v-scrolling-div">
                                            <v-list-tile v-for="(WFitem, index) in lstProjectWorkflows" :key="index">
                                                <v-checkbox color="success-check-box" v-model="selectedcheckboxWorkflow[index]" input-value="true" :label="[`${WFitem.WfName.length}`<=10 ? `${WFitem.WfName}` : `${WFitem.WfName.substring(0,10)+'..'}`]"></v-checkbox>
                                            </v-list-tile>
                                        </div>
                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="workflowMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), workflowMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
                                    </v-list>
                                </v-menu>
                            </v-col>
                            <!--Action-->
                            <v-col md="auto">
                                <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="actionMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mx-1">
                                            Action
                                            <v-icon class="ml-12">
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                        <v-checkbox color="success-check-box" v-model="checkCollect" input-value="true" label="To Collect"></v-checkbox>
                                        <v-checkbox color="success-check-box" v-model="checkApprove" input-value="true" label="To Approve"></v-checkbox>

                                        <div class="d-inline-flex">
                                            <v-btn id="" height="32" width="32" block color="#BEBEBE" dark class="elevation-0 mr-1" @click="actionMenu = false">
                                                Cancel
                                            </v-btn>
                                            <v-btn id="" height="32" width="32" block color="primary" class="elevation-0" @click="fnFilterDone(), actionMenu = false">
                                                Done
                                            </v-btn>
                                        </div>
                                    </v-list>
                                </v-menu>
                            </v-col>

                            <v-row v-if="filterChipsList.length>0" justify="center" no-gutters class="pa-0 my-0">
                                <v-col md="10" class="d-flex">
                                    <div class="mobile-chip-container ma-0 ">
                                        <v-chip-group multiple column active-class="primary--text">
                                            <v-chip id="" close v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="mx-2" @click:close="fnChipClose(item)" small label>
                                                {{ item.ChipName }}
                                            </v-chip>
                                        </v-chip-group>
                                    </div>
                                </v-col>
                            </v-row>
                        </v-col>
                    </v-row>
                    
                    <v-row class="py-0 my-0">
                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center py-0 my-0">
                            <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="mobileFilterContent = false"></Secondary-Button>
                            <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="mobileFilterContent = false"></Default-Button-Outlined>
                        </v-col>
                    </v-row>
                </v-card-text>
            </v-card>
        </v-dialog>
        
        <!--sync divider line-->
        <v-row justify="center" no-gutters style="height:5vh" class="mb-2 mb-sm-0">
            <v-col md="10">
                <v-row no-gutters>
                    <v-col cols="12" md="4" class="pt-3"><v-divider class="d-none d-md-block"></v-divider></v-col>
                    <v-col cols="12" md="4" align="center">
                        <p class="black--text">
                            <v-icon left color="green">mdi-sync</v-icon>
                            <span>{{this.syncLabelTxt}}</span>
                            <!--<span v-if="syncLabel==2">Pending Sync Items(1)</span>
    <span v-if="syncLabel==1">Syncing...</span>
    <span v-if="syncLabel==0">All Synced</span>-->
                        </p>
                    </v-col>
                    <v-col cols="12" md="4" class="pt-3"><v-divider class="d-none d-md-block"></v-divider></v-col>
                </v-row>
            </v-col>
        </v-row>
        <!--all type card-->
        <v-row justify="center">
            <v-col md="10" class="v-scrolling-div py-0 my-0 overdueCardHeight">
                <TaskDataCollector :searchcall="searchval" :searchtask="searchtaskval" :Filter=filtervalue :DateRange="daterange" :StartDate="startdate" :EndDate="enddate" :ProjectName="projectname" :BeneficiaryName="beneficiaryname" :WorkflowName="workflowname" :Action="action" :FilterGUID="filterGUID" ref="cc1"></TaskDataCollector>
                <Survey :searchcall="searchval" :searchtask="searchtaskval" :Filter=filtervalue :DateRange="daterange" :StartDate="startdate" :EndDate="enddate" :ProjectName="projectname" :BeneficiaryName="beneficiaryname" :WorkflowName="workflowname" :Action="action" :FilterGUID="filterGUID" ref="cc2"></Survey>
                <SurveyApprover :searchcall="searchval" :searchtask="searchtaskval" :Filter=filtervalue :DateRange="daterange" :StartDate="startdate" :EndDate="enddate" :ProjectName="projectname" :BeneficiaryName="beneficiaryname" :WorkflowName="workflowname" :Action="action" :FilterGUID="filterGUID" ref="cc3"></SurveyApprover>
                <TaskDataApprover :searchcall="searchval" :searchtask="searchtaskval" :Filter=filtervalue :DateRange="daterange" :StartDate="startdate" :EndDate="enddate" :ProjectName="projectname" :BeneficiaryName="beneficiaryname" :WorkflowName="workflowname" :Action="action" :FilterGUID="filterGUID" ref="cc4"></TaskDataApprover>
                <TaskFinanceApprover :searchcall="searchval" :searchtask="searchtaskval" :Filter=filtervalue :DateRange="daterange" :StartDate="startdate" :EndDate="enddate" :ProjectName="projectname" :BeneficiaryName="beneficiaryname" :WorkflowName="workflowname" :Action="action" :FilterGUID="filterGUID" ref="cc5"></TaskFinanceApprover>
                <TaskGeneralApprover :searchcall="searchval" :searchtask="searchtaskval" ref="cc6"></TaskGeneralApprover>
            </v-col>
        </v-row>
        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </v-card>      
</template>
<script type="text/javascript">
    // import ActivityDataCollector from '@/views/Tasks/DataCollector/ActivityDataCollector.vue'
    import { dbImworks } from '../../ImworksDb'
    import { mapState, mapMutations, mapGetters, mapActions } from 'vuex'
    import TaskDataApprover from '@/views/Tasks/TaskDataApprover.vue'
    import TaskDataCollector from '@/views/Tasks/TaskDataCollector.vue'
    import TaskFinanceApprover from '@/views/Tasks/TaskDataFinanceApprover.vue'
    import TaskGeneralApprover from '@/views/Tasks/TaskGeneralDataApproval.vue'
    import Survey from '@/views/Tasks/TaskDataCollectorSurvey.vue'
    import SurveyApprover from '@/views/Tasks/TaskDataApproverForSurvey.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'

    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/MyProject.js").then(mod => {
                objProject = new mod.MyProject();
            })
        ]);
    };

    var objManageProject;
    async function importscriptprojectworkflow() {
        return Promise.all([
            import("../../BL/ManageProject.js").then((mod) => {
                objManageProject = new mod.ManageProject();
            }),
        ]);
    }

    var objPB;
    async function importscriptbeneficiarytype() {
        return Promise.all([
            import("../../BL/ProjectBeneficiary.js").then(mod => {
                objPB = new mod.ProjectBeneficiary();
            })
        ]);
    }

    export default {
        
        data() {
            return {
                WorkspaceID: 0,
                ProjectId: 0,
                BeneficiaryTypeID: 0,
                WorkflowID: 0,
                syncLabel: 0,
                infoSnackbarMsg: '',
                Msg: '',
                //filter-piece-data-variables
                filterContent: false,
                mobileFilterContent: false,
                dateRangeMenu: false,
                dateRangeHeaderName: "Date Range",
                filterChipsList: [],
                selectedItem: "",
                syncLabelTxt: "",
                SetCunt: 0,
                items: [
                    { text: 'This month' },
                    { text: 'This quarter' },
                    { text: 'This Year' },
                ],
                ProjectTableItems: [],
                FilterProjectTableItems: [],

                //workflowslist: [],
                selectedcheckboxWorkflow: [],
                workflowidlist: [],
                selectedworkflow: [],
                lstProjectWorkflows: [],
                workflowname: "",
                workflowMenu: false,

                lstBType: [],
                selectedBeneficiaryTypeID: [],
                selectedbeneficiary: [],
                beneficiaryTypeMenu: false,
                beneficiaryname: "",
                //beneficiaryidlist: [],

                selectedProjectID: [],
                selected: [],
                
                projectMenu: false,
                projectname: "",
                projectidlist: [],
                
                taskMenu: false,
                actionMenu: false,
                searchval: "",
                searchtaskval: "",
                searchProject: "", 
                checkOutput: false,
                checkOutcome: false,
                checkRisk: false,
                checkSurvey: false,
                checkPayment: false,
                filtervalue: "",
                daterange: "",
                action: "",
                startdate: "",
                enddate: "",
                tocollect: "",
                toapprove: "",
                checkCollect: false,
                checkApprove: false,
                //checkStartDate: new Date().toISOString().substr(0, 10),
                //checkEndDate: new Date().toISOString().substr(0, 10),
                checkStartDate: "",
                checkEndDate: "",
                filterGUID: "",
                res: "",
                checkFilter: "",
                filterGet: "",
                SelectedFilter: [
                    {
                        startdate: "",
                        enddate: "",
                        taskfiltervalue: [],
                        workflowid: [],
                        beneficiaryid: [],
                        projectname: [],
                        action: []
                    }
                  
                ],
            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            await this.GetCompletedTaskData();
            await importscript();
            await importscriptprojectworkflow();
            await importscriptbeneficiarytype();
            await this.GetProjectListByInitID();
            await this.GetWorkflowsListForFilter();
            await this.GetProjectBeneficiaryTypeForFilter();
            this.filterget = JSON.parse(window.localStorage.getItem('SelectedFilter'));
            await this.getFilter();
        },
        methods:
        {
            IsValidDate() {

                if (Date.parse(this.checkEndDate) < Date.parse(this.checkStartDate)) {
                    this.Msg = ("Invalid Date : End date should be greater than Start Date");
                    this.infoSnackbarMsg = true;
                    this.checkEndDate = "";
                    return;
                }
            },

            async GetCompletedTaskData() {              
                var PendingSyncVar = await dbImworks.TaskSyncDB.toArray();
                this.SetCunt = PendingSyncVar.length;                
                if (this.SetCunt != 0) {
                    this.syncLabelTxt = "Pending Sync Items(" + this.SetCunt + ")";
                }
                else if (this.SetCunt == 0) {
                    this.syncLabelTxt = "All Synced"; 
                }
            },

            async GetProjectListByInitID() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("stage", 'Deploy');

                var result = await objProject.GetProjectListByInitID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.ProjectTableItems = result.data;
                        this.FilterProjectTableItems = result.data;
                    }
            },

            async GetProjectBeneficiaryTypeForFilter() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                this.lstBType = [];
                var result = await objPB.GetProjectBeneficiaryTypeForFilter(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstBType = result.data;
                }
            },

            async GetWorkflowsListForFilter() {
                var data = new FormData();
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objManageProject.GetWorkflowsListForFilter(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.lstProjectWorkflows = result.data;
                }
            },

            ...mapMutations('DefaultStore',['SaveSelectedFilter']),
            ...mapActions('DefaultStore',["SaveSelectedFilter"]),

            fnFilterDone() {
                this.filterChipsList = [];
                //date range filter
                //debugger;
                if (this.checkStartDate != '' && this.checkEndDate != '') {
                    this.startdate = this.checkStartDate;
                    this.enddate = this.checkEndDate;

                    this.filterChipsList.push({ ChipID: 1003, ChipName: this.filterdateformat(this.checkStartDate) + "-" + this.filterdateformat(this.checkEndDate) });
                    this.dateRangeHeaderName = this.filterdateformat(this.checkStartDate) + "-" + this.filterdateformat(this.checkEndDate);

                } else {
                    this.startdate = "";
                    this.enddate = "";
                    this.dateRangeHeaderName = "Date Range";
                }
                //end of date filter

                //start of task filter
                this.checkFilter = "";
                if (this.checkOutput == true) {
                    this.checkFilter = "Activity";
                    this.filterChipsList.push({ ChipID: 1004, ChipName: "Output" });
                }
                if (this.checkOutcome == true) {
                    this.checkFilter += ",Outcome";
                    this.filterChipsList.push({ ChipID: 1005, ChipName: "Outcome" });
                }
                if (this.checkRisk == true) {
                    this.checkFilter += ",RISK";
                    this.filterChipsList.push({ ChipID: 1006, ChipName: "Risk" });
                }
                if (this.checkPayment == true) {
                    this.checkFilter += ",Fund Request";//Fund Request
                    this.filterChipsList.push({ ChipID: 1007, ChipName: "Payment" });
                }
                if (this.checkSurvey == true) {
                    this.checkFilter += ",Survey";
                    this.filterChipsList.push({ ChipID: 1008, ChipName: "Survey" });
                }
                else {
                    this.filtervalue = "Blank";
                }
                this.res = this.checkFilter.charAt(0);
                if (this.res == ",") {
                    this.checkFilter = this.checkFilter.substring(1);
                }
                this.filtervalue = this.checkFilter;

                
                //start of project filter
                this.projectidlist = [];
                for (var i = 0; i < this.selectedProjectID.length; i++) {
                    if (this.selectedProjectID[i]) {
                        this.projectidlist.push(this.ProjectTableItems[i].ProjectId);
                        this.filterChipsList.push({ ChipID: this.ProjectTableItems[i].ProjectId, ChipName: this.ProjectTableItems[i].ProjectName });
                        this.selected.push(this.ProjectTableItems[i].ProjectName);
                        
                    }
                    
                }
                
                this.projectname = this.selected;
                this.selected = [];
                //end of project filter

                //start of beneficiary type filter
                for (var i = 0; i < this.selectedBeneficiaryTypeID.length; i++) {
                    if (this.selectedBeneficiaryTypeID[i]) {
                        this.filterChipsList.push({ ChipID: this.lstBType[i].BeneficiaryTypeID, ChipName: this.lstBType[i].Name });
                        this.selectedbeneficiary.push(this.lstBType[i].BeneficiaryTypeID,this.lstBType[i].Name);

                    }
                }

                this.beneficiaryname = this.selectedbeneficiary;
                this.selectedbeneficiary = [];
                //end of beneficiary type filter
                
                //start of workflow filter
                for (var i = 0; i < this.selectedcheckboxWorkflow.length; i++) {
                    if (this.selectedcheckboxWorkflow[i]) {
                        this.filterChipsList.push({ ChipID: this.lstProjectWorkflows[i].WorkflowID, ChipName: this.lstProjectWorkflows[i].WfName });
                        this.selectedworkflow.push(this.lstProjectWorkflows[i].WorkflowID);

                    }
                }

                this.workflowname = this.selectedworkflow;
                this.selectedworkflow = [];
                //end of workflow filter

                //start of Action filter
                var checkAction = "";

                if (this.checkCollect == true) {
                    checkAction = "To DataCollect";
                    this.filterChipsList.push({ ChipID: 1001, ChipName: "To DataCollect" });
                }
                
                if (this.checkApprove == true) {
                    checkAction = "To Approve";
                    this.filterChipsList.push({ ChipID: 1002, ChipName: "To Approve" });

                }
                if (this.checkCollect == true && this.checkApprove == true) {
                    checkAction = "All";
                }
                this.action = checkAction;

                this.filterGUID = this.Generateuuidv4();
                
                this.SelectedFilter = [];
                this.SelectedFilter.push({
                    workflowid: this.workflowname, beneficiaryid: this.beneficiaryname, projectname: this.projectname, taskfiltervalue: this.filtervalue,
                    startdate: this.startdate, enddate: this.enddate, action: this.action
                });
                this.SaveSelectedFilter(this.SelectedFilter);
            },

            async getFilter() {
                //date range
                if (this.filterget != null) {
                    if (this.filterget[0].startdate.length > 0) {
                        this.checkStartDate = this.filterget[0].startdate;
                    }
                    if (this.filterget[0].enddate.length > 0) {
                        this.checkEndDate = this.filterget[0].enddate;
                    }
                }
                

                //task
                if (this.filterget[0].taskfiltervalue.length > 0) {
                    var temp = this.filterget[0].taskfiltervalue.split(',');
                    for (var i = 0; i < temp.length; i++) {
                        if (temp[i] == "Activity") {
                            this.checkOutput = true;
                        }
                        if (temp[i] == "Outcome") {
                            this.checkOutcome = true;
                        }
                        if (temp[i] == "RISK") {
                            this.checkRisk = true;
                        }
                        if (temp[i] == "Fund Request") {
                            this.checkPayment = true;
                        }
                        if (temp[i] == "Survey") {
                            this.checkSurvey = true;
                        }
                    }
                }

                //project
                for (var i = 0; i < this.filterget[0].projectname.length; i++) {
                    const index2 = this.ProjectTableItems.findIndex((element, index) => {
                        if (element.ProjectName === this.filterget[0].projectname[i]) {
                            return true
                        }
                    });
                    if (index2 >= 0) {
                        this.selectedProjectID[index2] = this.filterget[0].projectname[i];
                    }
                }

                //beneficiary type
                for (var i = 0; i < this.filterget[0].beneficiaryid.length; i++) {
                    const index1 = this.lstBType.findIndex((element, index) => {
                        if (element.BeneficiaryTypeID === this.filterget[0].beneficiaryid[i]) {
                            return true
                        }
                    });
                    if (index1 >= 0) {
                        this.selectedBeneficiaryTypeID[index1] = this.filterget[0].beneficiaryid[i];
                    }
                }

                //workflow
                for (var i = 0; i < this.filterget[0].workflowid.length; i++) {
                    const index = this.lstProjectWorkflows.findIndex((element, index) => {
                        if (element.WorkflowID === this.filterget[0].workflowid[i]) {
                            return true
                        }
                    });
                    if (index >= 0) {
                        this.selectedcheckboxWorkflow[index] = this.filterget[0].workflowid[i];
                    }
                }

                //action
                if (this.filterget[0].action.length > 0) {
                    var temp1 = this.filterget[0].action.split(',');
                    for (var i = 0; i < temp1.length; i++) {
                        if (temp1[i] == "To DataCollect") {
                            this.checkCollect = true;
                        }
                        if (temp1[i] == "To Approve") {
                            this.checkApprove = true;
                        }
                    }
                }

                await this.fnFilterDone();
            },

            resetFilter() {

                this.checkStartDate = '',
                this.checkEndDate = '',

                this.checkFilter = "";
                this.checkOutput = false;
                this.checkOutcome = false;
                this.checkRisk = false;
                this.checkPayment = false;
                this.checkSurvey = false;
                this.filtervalue = "Blank";

                this.projectidlist = [];
                for (var i = 0; i < this.selectedProjectID.length; i++) {
                    if (this.selectedProjectID[i]) {
                        this.selectedProjectID[i] = false;
                    }
                }

                for (var i = 0; i < this.selectedBeneficiaryTypeID.length; i++) {
                    if (this.selectedBeneficiaryTypeID[i]) {
                        this.selectedBeneficiaryTypeID[i] = false;
                    }
                }

                for (var i = 0; i < this.selectedcheckboxWorkflow.length; i++) {
                    if (this.selectedcheckboxWorkflow[i]) {
                        this.selectedcheckboxWorkflow[i] = false;
                    }
                }


                this.checkAction = "";
                this.checkCollect = false;
                this.checkApprove = false;

                
                this.fnFilterDone(); 

            },

            fnChipClose(chipitem) {
                this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);

                //date chip deletion
                if (chipitem.ChipName == this.filterdateformat(this.checkStartDate) + "-" + this.filterdateformat(this.checkEndDate)) {
                    this.checkStartDate = "";
                    this.checkEndDate = "";
                    this.dateRangeHeaderName = "Date Range";
                }

                //Task chip deletion
                //Outcome,Risk,Payment,Survey,
                //checkOutcome,checkRisk,checkPayment,checkSurvey
                if (chipitem.ChipName == "Output")
                    this.checkOutput = false;
                if (chipitem.ChipName == "Outcome")
                    this.checkOutcome = false;
                if (chipitem.ChipName == "Risk")
                    this.checkRisk = false;
                if (chipitem.ChipName == "Payment")
                    this.checkPayment = false;
                if (chipitem.ChipName == "Survey")
                    this.checkSurvey = false;

                //project chip deletion
                if (this.projectidlist != []) {
                    for (var i = 0; i < this.selectedProjectID.length; i++) {
                        if (this.selectedProjectID[i] && this.ProjectTableItems[i].ProjectId == chipitem.ChipID) {
                            this.selectedProjectID[i] = false;
                        }
                    }
                }
                
                //beneficiary type chip deletion
                for (var i = 0; i < this.selectedBeneficiaryTypeID.length; i++) {
                    if (this.selectedBeneficiaryTypeID[i] && this.lstBType[i].BeneficiaryTypeID == chipitem.ChipID) {
                        this.selectedBeneficiaryTypeID[i] = false;
                    }
                }

                //workflow chip deletion
                for (var i = 0; i < this.selectedcheckboxWorkflow.length; i++) {
                    if (this.selectedcheckboxWorkflow[i] && this.lstProjectWorkflows[i].WorkflowID == chipitem.ChipID) {
                        this.selectedcheckboxWorkflow[i] = false;
                    }
                }


                //Action chip deletion
                if (chipitem.ChipName == "To DataCollect")
                    this.checkCollect = false;
                if (chipitem.ChipName == "To Approve")
                    this.checkApprove = false;
                
                

                this.fnFilterDone();
            },

            filterdateformat(date) {
                if (!date) return null;
                const [year, month, day] = date.substr(0, 10).split("-");
                if (month === "01") {
                    return `${day} ${"Jan"}`;
                }
                if (month === "02") {
                    return `${day} ${"Feb"}`;
                }
                if (month === "03") {
                    return `${day} ${"Mar"}`;
                }
                if (month === "04") {
                    return `${day} ${"Apr"}`;
                }
                if (month === "05") {
                    return `${day} ${"May"}`;
                }
                if (month === "06") {
                    return `${day} ${"Jun"}`;
                }
                if (month === "07") {
                    return `${day} ${"Jul"}`;
                }
                if (month === "08") {
                    return `${day} ${"Aug"}`;
                }
                if (month === "09") {
                    return `${day} ${"Sep"}`;
                }
                if (month === "10") {
                    return `${day} ${"Oct"}`;
                }
                if (month === "11") {
                    return `${day} ${"Nov"}`;
                }
                if (month === "12") {
                    return `${day} ${"Dec"}`;
                }
            },

            /*fnClickDateRangeCancel(FilterAll) {
                if (FilterAll == "Date Range") {
                    this.startdate = "";
                    this.enddate = "";
                }

                this.filterGUID = this.Generateuuidv4();
            },*/
            
            Generateuuidv4() {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });
            }
        },

        computed: {
            ...mapState('DefaultStore',['SelectedFilter']),
            ...mapGetters('DefaultStore',["getSelectedFilter"]),
        },

        components: {
         ////   'Activity-DataCollector': ActivityDataCollector,
            'TaskDataCollector': TaskDataCollector,
            'TaskDataApprover': TaskDataApprover,
            'TaskFinanceApprover': TaskFinanceApprover,
            'TaskGeneralApprover': TaskGeneralApprover,
            'Survey': Survey,
            'SurveyApprover': SurveyApprover,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
        },
    }
</script>
<style scoped>
    .overdueCardHeight {
        height: 54vh;
    }

    /*Extra small devices (phones, 600px and down)*/
    @media only screen and (max-width: 750px) {
        .overdueCardHeight {
            height: 61vh;
        }
    }

    /*Small devices (portrait tablets and large phones, 600px and up)*/
    @media only screen and (min-width: 750px) {
        .overdueCardHeight {
            height: 62vh;
        }
    }

    /*Medium devices (landscape tablets, 768px and up)*/
    @media only screen and (min-width: 768px) {
    }

    /*Large devices (laptops/desktops, 992px and up)*/
    @media only screen and (min-width: 992px) {
        .overdueCardHeight {
            height: 54vh;
        }
    }

    /*Extra large devices (large laptops and desktops, 1200px and up)*/
    @media only screen and (min-width: 1400px) {
        .overdueCardHeight {
            height: 66vh;
        }
    }
</style>