<template>
  <v-card class="pt-0 elevation-0">
    <v-card class="mt-0 elevation-0" height="640">
      <v-card-title style="height: 60px;margin-right:85px" v-show="ShowAccountLedger == false">
        <h5 class="page-head">
          <v-avatar color="white" size="25" class="elevation-1 mr-2">
            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
          </v-avatar>
          Payment Linked Outputs
        </h5>
        <v-spacer></v-spacer>
        <v-text-field
          label="Search"
          class="float-left table-search"
          v-model="SearchActivity"
          dense=""
          prepend-inner-icon="mdi-magnify"
          outlined=""
          hide-details=""
        >
        </v-text-field>
      </v-card-title>
      <v-card-text v-show="ShowAccountLedger == false">
        <v-row v-if="ActivityList.length == 0" justify="center">
          <!--If there is no Activities-->
          <v-col class="text-center">
            <p class="mt-2 pt-2">
              <img
                src="..\..\..\src\assets\EmptyPlaceholders\project.svg"
                height="80"
                width="80"
              />
            </p>
            <p class="heading-4-dark font-weight-medium pt-0">
              No activities available
            </p>
          </v-col>
          <!--______________________________________-->
        </v-row>
      </v-card-text>
      <v-card-text class="pt-0" v-show="ShowAccountLedger == false">
        <!--Tree table section-->
        <v-row>
          <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
        </v-row>
        <v-row v-if="ActivityList.length > 0">
          <v-col class="pt-0 px-2">
            <tree-table
              class="table tree-table"
              :columns="ActivityListHeader"
              :table-data="paginatedData"
              fixed-header
              style="height:520px"
            >
              <!-- Header Row -->
              <template #headerTemplate="headerProps">
                <div v-bind="headerProps">
                  <div
                    class="row header"
                    style="background-color: whitesmoke; max-height: 50px"
                  >
                    <div class="cell pl-4 pt-0" style="max-width: 50px">
                      <v-checkbox color="#707070"></v-checkbox>
                    </div>

                    <div class="cell" style="max-width: 65px"></div>
                    <div
                      style="font-size: 13px; color: #707070"
                      class="cell mt-2"
                      v-for="column in headerProps.columns"
                      :key="column.id"
                    >
                      <b>{{ column.label }}</b>
                      <!--<div
                        class=""
                        style="display: inline-block; cursor: pointer"
                      >
                        <div
                          id="AutoTest_F135"
                          style="height: 10px"
                          @click="sortTable({ key: column.id, asc: true })"
                        >
                          <v-icon size="10">mdi-arrow-up</v-icon>
                        </div>
                        <div
                          id="AutoTest_F136"
                          style="height: 10px"
                          @click="sortTable({ key: column.id, asc: false })"
                        >
                          <v-icon size="10">mdi-arrow-down</v-icon>
                        </div>
                      </div>-->
                    </div>
                  </div>
                </div>
              </template>

              <!-- Rows with sub-activities -->
              <template #nodeTemplate="nodeProps">
                <div v-bind="nodeProps">
                  <div class="row" style="max-height: 55px">
                    <div class="cell pl-4 pt-0" style="max-width: 50px">
                      <v-checkbox
                        v-model="nodeProps.rowData.checkbox"
                        @change="fnAddRemove(nodeProps.rowData)"
                        color="#52B962"
                      ></v-checkbox>
                    </div>
                    <!--<div v-if="nodeProps.depth > 0">
                      <div
                        class="indentation"
                        :style="{ width: step * nodeProps.depth + 'px' }"
                      ></div>
                    </div>-->
                    <div
                      id="AutoTest_F137"
                      class="pt-3"
                      @click="nodeProps.onToggle"
                    >
                      <v-btn
                        id="AutoTest_F138"
                        v-if="nodeProps.rowData.children.length > 0"
                        text=""
                        @click="
                          (nodeProps.rowData.isOpen = !nodeProps.rowData
                            .isOpen),
                            fnGetGrandChildrenActivity(nodeProps.rowData)
                        "
                      >
                        <div v-if="!nodeProps.rowData.isOpen">
                          <v-icon>mdi-chevron-right</v-icon>
                        </div>
                        <div v-if="nodeProps.rowData.isOpen">
                          <v-icon>mdi-chevron-down</v-icon>
                        </div>
                      </v-btn>
                    </div>

                    <div
                      class="pt-3"
                      v-if="nodeProps.rowData.children.length == 0"
                    >
                      <v-btn
                        id="AutoTest_F139"
                        text=""
                        @click="
                          nodeProps.rowData.isOpen = !nodeProps.rowData.isOpen
                        "
                      >
                        <div v-if="!nodeProps.rowData.isOpen" hidden>
                          <v-icon>mdi-chevron-right</v-icon>
                        </div>
                        <div v-if="nodeProps.rowData.isOpen" hidden>
                          <v-icon>mdi-chevron-down</v-icon>
                        </div>
                      </v-btn>
                    </div>
                    <div class="cell text-limit pt-5">
                      <!--<div
                        id="AutoTest_F140"
                        class="cell text-limit pt-0 pl-0"
                        @click="
                          (ActivityId = nodeProps.rowData.ActivityId),
                            (ActivityName = nodeProps.rowData.ActivityName),
                            GetPaymentListOfActivicty(),
                            (ShowAccountLedger = true)
                        "
                        style="cursor: pointer"
                      >-->
                        <v-avatar
                          color="white"
                          size="25"
                          class="elevation-1 mr-2 mb-1"
                        >
                          <v-icon size="15" color="primary"
                            >mdi-chart-areaspline</v-icon
                          >
                        </v-avatar>
                        {{ nodeProps.rowData.ActivityName }}
                      <!--</div>-->
                      <!--<h4 @click="ShowAccountLedger = true">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ nodeProps.rowData.ActivityName }}
                                        </h4>-->
                    </div>
                    <div class="cell pt-5">
                      {{ nodeProps.rowData.PaymentLinkedBudget }}
                    </div>
                    <div class="cell">
                      <v-progress-circular
                        :rotate="360"
                        :size="40"
                        :width="5"
                        :value="nodeProps.rowData.progress"
                        color="secondary"
                      >
                        {{ nodeProps.rowData.progress }}
                      </v-progress-circular>
                      {{ nodeProps.rowData.progress }}%
                    </div>
                    <div class="cell pt-5 text-limit" style="width:150px">
                        <span v-if="nodeProps.rowData.Workflow">{{ nodeProps.rowData.Workflow }}</span>
                        <span v-else>N/A</span>
                    </div>
                    <!-- Payment Status-->
                    <!--<div class="cell">
                                        <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                            <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                        </v-avatar>{{ nodeProps.rowData.PaymentStatus }}
                                    </div>-->
                    <div class="cell pt-4">
                      <v-menu
                        close-on-content-click=""
                        transition="scroll-y-transition"
                        left=""
                        offset-y=""
                        v-bind:disabled="!screenAccess"
                      >
                        <template v-slot:activator="{ on }">
                          <v-btn
                            min-width="27"
                            height="29"
                            v-on="on"
                            color="grey"
                            outlined=""
                            dark=""
                            class="elevation-0 mr-2 px-0"
                          >
                            <v-icon>mdi-dots-vertical</v-icon>
                          </v-btn>
                        </template>
                        <v-list
                          dense=""
                          color="white"
                          light=""
                          width="auto"
                          class="action-button-list"
                        >
                          <v-list-item
                            id="AutoTest_F141"
                            link=""
                            @click.stop="
                              (SelectWorkflowDialog = true),
                                (ActivityId = nodeProps.rowData.ActivityId),
                                (ActivityName = nodeProps.rowData.ActivityName),
                                GetWorkflowsListToactivity(nodeProps.rowData)
                            "
                          >
                            <v-list-item-icon>
                              <v-icon> mdi-timeline-text-outline </v-icon>
                            </v-list-item-icon>
                            <v-list-item-content
                              >Reassign Workflow</v-list-item-content
                            >
                          </v-list-item>
                          <v-list-item
                            id="AutoTest_F142"
                            link=""
                            @click.stop="
                              (ActivityId = nodeProps.rowData.ActivityId),
                                (ActivityName = nodeProps.rowData.ActivityName),
                                (AddPaymentDialog = true)
                            "
                          >
                            <v-list-item-icon>
                              <v-icon> mdi-cash </v-icon>
                            </v-list-item-icon>
                            <v-list-item-content
                              >Add Payment</v-list-item-content
                            >
                          </v-list-item>
                        </v-list>
                      </v-menu>
                    </div>
                  </div>
                </div>
              </template>

              <!-- Rows with no sub-activities -->
              <template #leafTemplate="leafProps">
                <div v-bind="leafProps">
                  <div class="row" style="max-height: 55px">
                    <div class="cell" style="max-width: 50px">
                      <v-checkbox
                        v-model="leafProps.rowData.checkbox"
                        @change="fnAddRemove(leafProps.rowData)"
                        color="#52B962"
                      ></v-checkbox>
                    </div>
                    <div v-if="leafProps.depth > 0">
                      <div
                        class="indentation"
                        :style="{ width: step * leafProps.depth + 'px' }"
                      ></div>
                    </div>
                    <div class="cell text-limit pt-5">
                      <!--<router-link :to="{name:'ManageProjectActivitiesIndividual',query: { WsID: WorkspaceID, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                            <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                                <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                            </v-avatar>
                                            {{ leafProps.rowData.ActivityName }}
                                        </router-link>-->
                      <h5
                        id="AutoTest_F143"
                        class="page-head"
                        @click="ShowAccountLedger = true"
                        style="cursor: pointer"
                      >
                        <v-avatar
                          color="white"
                          size="25"
                          class="elevation-1 mr-2 mb-1"
                        >
                          <v-icon size="15" color="primary"
                            >mdi-cash-multiple</v-icon
                          >
                        </v-avatar>
                        {{ nodeProps.rowData.ActivityName }}
                      </h5>
                    </div>
                    <div class="cell">
                      {{ leafProps.rowData.PaymentLinkedBudget }}
                    </div>
                    <div class="cell">
                      <v-progress-circular
                        :rotate="360"
                        :size="40"
                        :width="5"
                        :value="leafProps.rowData.progress"
                        color="secondary"
                      >
                        {{ leafProps.rowData.progress }}
                      </v-progress-circular>
                      {{ leafProps.rowData.progress }}%
                    </div>
                    <div class="cell text-limit" style="width:150px">
                      {{ leafProps.rowData.Workflow }}
                    </div>
                    <!-- Payment Status-->
                    <!--<div class="cell">
                                        <v-avatar color="#E0F4E8" size="20" class="mr-1">
                                            <v-icon size="15" color="#47BB76">mdi-check-circle-outline</v-icon>
                                        </v-avatar>{{ leafProps.rowData.PaymentStatus }}
                                    </div>-->
                    <div class="cell pt-4">
                      <v-menu
                        close-on-content-click=""
                        transition="scroll-y-transition"
                        left=""
                        offset-y=""
                        v-bind:disabled="!screenAccess"
                      >
                        <template v-slot:activator="{ on }">
                          <v-btn
                            min-width="27"
                            height="29"
                            v-on="on"
                            color="grey"
                            outlined=""
                            dark=""
                            class="elevation-0 mr-2 px-0"
                          >
                            <v-icon>mdi-dots-vertical</v-icon>
                          </v-btn>
                        </template>
                        <v-list
                          dense=""
                          color="white"
                          light=""
                          width="auto"
                          class="action-button-list"
                        >
                          <v-list-item
                            id="AutoTest_F144"
                            link=""
                            @click.stop="
                              (SelectWorkflowDialog = true),
                                (ActivityId = leafProps.rowData.ActivityId),
                                (ActivityName = nodeProps.rowData.ActivityName),
                                GetWorkflowsListToactivity(leafProps.rowData)
                            "
                          >
                            <v-list-item-icon>
                              <v-icon> mdi-chart-areaspline </v-icon>
                            </v-list-item-icon>
                            <v-list-item-content
                              >Reassign Workflow</v-list-item-content
                            >
                          </v-list-item>
                          <v-list-item
                            id="AutoTest_F145"
                            link=""
                            @click.stop="
                              (ActivityId = leafProps.rowData.ActivityId),
                                (AddPaymentDialog = true)
                            "
                          >
                            <v-list-item-icon>
                              <v-icon> mdi-chart-areaspline </v-icon>
                            </v-list-item-icon>
                            <v-list-item-content
                              >Add Payment</v-list-item-content
                            >
                          </v-list-item>
                        </v-list>
                      </v-menu>
                    </div>
                  </div>
                </div>
              </template>
            </tree-table>
            <!--Pagination-->
            <div class="pagination">
              <div
                id="AutoTest_F146"
                class="pagination--button pagination--button__active"
                v-if="!isFirstPage()"
                v-on:click="currentPage = currentPage - 1"
              >
                &lt;
              </div>
              <div class="pagination--button" v-if="isFirstPage()">-</div>
              <div>{{ currentPage + 1 }} / {{ numberOfPages() }}</div>
              <div
                id="AutoTest_F147"
                class="pagination--button pagination--button__active"
                v-if="!isLastPage()"
                v-on:click="currentPage = currentPage + 1"
              >
                >
              </div>
              <div class="pagination--button" v-if="isLastPage()">-</div>
            </div>
            <!--__________-->
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
    <!-- Activity Ledger Datatable-->
    <v-card-title style="height: 60px" v-show="ShowAccountLedger == true">
      <h5
        id="AutoTest_F148"
        class="page-head"
        @click="ShowAccountLedger = false"
      >
        <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
          <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
        </v-avatar>
        <span style="cursor: pointer">{{ ActivityName }}</span
        ><v-icon>mdi-chevron-right</v-icon> Ledger book
      </h5>
      <v-spacer></v-spacer>
      <!--Bulk Action -->
      <v-menu offset-y transition="scroll-y-transition">
        <template v-slot:activator="{ on }">
          <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
          <v-btn
            height="38"
            v-on="on"
            color="grey"
            outlined=""
            class="elevation-0 mr-4"
          >
            Bulk Actions
            <v-icon>mdi-chevron-down</v-icon>
          </v-btn>
        </template>
        <v-list
          v-if="selectedPaymentList.length > 0"
          color="white"
          light=""
          width="auto"
          max-width="250"
          class="action-button-list"
        >
          <v-list-item
            id="AutoTest_F149"
            @click.stop="RemovePaymentDialog = true"
          >
            <v-list-item-icon>
              <v-icon>mdi-trash-can-outline</v-icon>
            </v-list-item-icon>
            <v-list-item-content> Remove </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-menu>
      <!--Search -->
      <v-text-field
        label="Search"
        class="table-search"
        v-model="SearchfilterPaymentList"
        dense
        prepend-inner-icon="mdi-magnify"
        outlined
        hide-details
      ></v-text-field>
    </v-card-title>
    <v-card-text class="px-3" v-show="ShowAccountLedger == true">
      <v-row>
        <v-col cols="12" md="12" sm="12" xs="12" class="px-0">
          <v-data-table
            :headers="LedgerBookTableheaders"
            :items="filterPaymentList"
            item-key="PaymentId"
            :search="SearchfilterPaymentList"
            v-model="selectedPaymentList"
            show-select
            fixed-header
            height="50vh"
          >
            <template v-slot:item.PaymentActivityName="{ item }">
              <v-list class="py-0 table-v-list">
                <v-list-item class="my-0 px-0">
                  <v-list-item-icon>
                    <v-avatar size="30" color="#fff" class="elevation-1">
                      <v-icon size="20" color="primary">mdi-book</v-icon>
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content>
                    {{ item.PaymentActivityName }}
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </template>

            <template v-slot:item.TransactionDate="{ item }">
              {{ formatDate(item.TransactionDate) }}
            </template>

            <template v-slot:item.IncomeAccountId="{ item }">
              {{ fnGetIncomeAccountName(item.IncomeAccountId) }}
            </template>
            <template v-slot:item.ExpenseAccountId="{ item }">
              {{ fnGetExpenseAccountName(item.ExpenseAccountId) }}
            </template>

            <!--<template v-slot:item.ExpenseTags="{ item }">
              <div v-for="tagItem in item.ExpenseTags">
                {{ tagItem.TagName }}
              </div>
            </template>-->

             <template v-slot:item.TransactionType="{item}">
                <div v-if="item.TransactionType=='Credit'" style="width:100px">
                    <!--<v-avatar color="#0CBA2F" size="13" class="mr-1">
                        <v-icon size="10" color="#fff">mdi-check</v-icon>
                    </v-avatar>-->Credit
                </div>
                <div v-else>
                    <!--<v-avatar color="#BA0C0C" size="13" class="mr-1">
                        <v-icon size="10" color="#fff">mdi-close</v-icon>
                    </v-avatar>-->Debit
                </div>
            </template>

            <template v-slot:item.TransactionDocName="{ item }">
              <div v-if="item.TransactionDocName">
                {{ item.TransactionDocName }}
                <v-avatar color="#388E3C" tile="" size="13" class="ml-1">
                  <v-icon size="10" color="#fff">mdi-check</v-icon>
                </v-avatar>
              </div>
            </template>

            <template v-slot:item.TransactionStatus="{ item }">
              <div v-if="item.TransactionStatus == 'Approved'">
                <v-avatar color="#0CBA2F" size="13" class="mr-1">
                  <v-icon size="10" color="#fff">mdi-check</v-icon> </v-avatar
                >Approved
              </div>
              <div v-else>
                <v-avatar color="#BA0C0C" size="13" class="mr-1">
                  <v-icon size="10" color="#fff">mdi-close</v-icon> </v-avatar
                >Rejected
              </div>
            </template>

            <template v-slot:item.LedgerTableActions="{ item }">
              <v-menu
                close-on-content-click=""
                transition="scroll-y-transition"
                left=""
                offset-y=""
                v-bind:disabled="!screenAccess"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    min-width="27"
                    height="29"
                    v-on="on"
                    color="grey"
                    outlined=""
                    dark=""
                    class="elevation-0 mr-2 px-0"
                  >
                    <v-icon>mdi-dots-vertical</v-icon>
                  </v-btn>
                </template>
                <v-list
                  color="white"
                  light=""
                  width="auto"
                  class="action-button-list"
                >
                  <v-list
                    color="white"
                    light
                    width="auto"
                    class="action-button-list"
                  >
                    <v-list-item
                      id="AutoTest_F150"
                      link
                      @click.stop="
                        (EditPaymentDialog = true),
                          fnInitializeEditPaymentItem(item)
                      "
                    >
                      <v-list-item-icon
                        ><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon
                      >
                      <v-list-item-content>Edit</v-list-item-content>
                    </v-list-item>
                    <v-list-item
                      id="AutoTest_F151"
                      link
                      @click.stop="
                        (RemovePaymentDialog = true), (RemovePaymentItem = item)
                      "
                    >
                      <v-list-item-icon
                        ><v-icon
                          >mdi-trash-can-outline</v-icon
                        ></v-list-item-icon
                      >
                      <v-list-item-content>Remove</v-list-item-content>
                    </v-list-item>
                  </v-list>
                </v-list>
              </v-menu>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-card-text>
    <!-- Activity Ledger Datatable ends here ------->
    <!-- Select Work flow-->
    <v-dialog v-model="SelectWorkflowDialog" width="1100" persistent>
      <v-card>
        <v-card-title primary-title class="page-head pb-0">
          <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
            <img style="width: 70%" :src="Workflow_Tab_icon" /> </v-avatar
          >Add Workflow
          <v-spacer></v-spacer>
          <v-btn
            id="AutoTest_IP641"
            text
            @click="
              (SelectWorkflowDialog = false),
                (checkboxAddWorkflow = []),
                (selectedAddWorkflow = []),
                (SelectWorkflowDlgItems = []),
                (confirmError = false)
            "
          >
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-card-text class="py-0">
          <v-container fluid class="pt-0">
            <v-row no-gutters>
              <v-col cols="12" md="12" class="pt-0">
                <v-row no-gutters>
                  <v-col
                    v-if="
                      SelectWorkflowDlgItems.length > 0 && confirmError == false
                    "
                    cols="12"
                    md="12"
                    class="pt-0 mb-2"
                  >
                    <h4 class="text-center heading-3">
                      Please select workflow from below
                    </h4>
                  </v-col>
                  <v-col
                    v-if="
                      SelectWorkflowDlgItems.length > 0 && confirmError == true
                    "
                    cols="12"
                    md="12"
                    class="pt-0 mb-2"
                  >
                    <h4 class="text-center heading-3 red--text">
                      Please select workflow from below *
                    </h4>
                  </v-col>
                </v-row>
                <v-stepper v-model="SelectWorkflowDlgSteps" class="elevation-0">
                  <v-stepper-items>
                    <!-- Select Workflow step-->
                    <v-stepper-content step="1" class="pa-0">
                      <!-- if workflow available -->
                      <v-row
                        no-gutters
                        v-if="SelectWorkflowDlgItems.length > 0"
                      >
                        <!-- Work flow group chip-->
                        <v-col md="12" class="py-0">
                          <div class="chip-container mb-2 ml-5">
                            <v-chip-group
                              multiple
                              column
                              active-class="primary--text"
                            >
                              <v-chip
                                id="AutoTest_IP642"
                                v-for="(chipitem, key) in selectedAddWorkflow"
                                :key="chipitem.WFitem.WorkflowID"
                                @click:close="fnRemove(chipitem)"
                                close
                                class="ma-2"
                                small
                              >
                                <v-icon left size="15" color="#707070"
                                  >mdi-folder-key-network-outline</v-icon
                                >
                                {{ chipitem.WFitem.WfName }}
                              </v-chip>
                            </v-chip-group>
                          </div>
                        </v-col>

                        <!-- Work flow details card-->
                        <v-row
                          class="mx-4 v-scrolling-div"
                          style="height: 45vh"
                        >
                          <v-col
                            xs="4"
                            sm="6"
                            md="4"
                            v-for="(
                              addWorkflowItem, index
                            ) in SelectWorkflowDlgItems"
                            :key="addWorkflowItem.WorkflowID"
                          >
                            <v-card class="workspace-card" flat>
                              <v-row>
                                <v-col
                                  xs="6"
                                  sm="6"
                                  md="10"
                                  class="d-inline-flex pt-0"
                                  :ripple="false"
                                >
                                  <div>
                                    <v-avatar
                                      size="60"
                                      tile
                                      class="elevation-1 base-border-radius"
                                    >
                                      <v-img
                                        :src="Workflow_Icon"
                                        height="40"
                                        width="40"
                                        class="ma-4"
                                      ></v-img>
                                    </v-avatar>
                                  </div>
                                  <div class="title pt-4 px-2 text-limit">
                                    {{ addWorkflowItem.WfName }}
                                  </div>
                                </v-col>
                                <v-col
                                  xs="2"
                                  sm="2"
                                  md="2"
                                  class="d-inline-flex pt-0 text-right"
                                >
                                  <!-- <v-checkbox
                                    color="success-check-box"
                                    v-model="checkboxAddWorkflow[index]"
                                    @change="fnCheck(addWorkflowItem, index)"
                                    label=""
                                  ></v-checkbox> -->
                                  <input
                                    type="radio"
                                    class="mt-3"
                                    v-if="selectedAddWorkflow.length > 0"
                                    style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                    name="workflow"
                                    :checked="
                                      selectedAddWorkflow[0].checkboxindex ==
                                      index
                                        ? true
                                        : false
                                    "
                                    @click="
                                      (checkboxAddWorkflow[index] = true),
                                        fnCheck(addWorkflowItem, index)
                                    "
                                  />
                                  <input
                                    type="radio"
                                    class="mt-3"
                                    style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                    name="workflow"
                                    v-else
                                    @click="
                                      (checkboxAddWorkflow[index] = true),
                                        fnCheck(addWorkflowItem, index)
                                    "
                                  />
                                </v-col>
                              </v-row>
                              <!--Description-->
                              <!--Description-->
                              <v-row>
                                <v-col
                                  sm="12"
                                  style="display: flex; flex-direction: row"
                                  class="py-0"
                                  :ripple="false"
                                >
                                  <span class="heading-5 black--text mt-2">
                                    <img :src="Description_icon" />
                                  </span>
                                  <div class="pt-2 pl-1">
                                    <span class="heading-5 black--text"
                                      >Description</span
                                    >
                                  </div>
                                </v-col>
                                <v-col sm="12" class="pt-0 pl-0">
                                  <p style="height: 24px" class="ml-3">
                                    {{
                                      addWorkflowItem.WfDescription.length >= 60
                                        ? addWorkflowItem.WfDescription.substring(
                                            0,
                                            60
                                          ) + "..."
                                        : addWorkflowItem.WfDescription
                                    }}
                                  </p>
                                </v-col>
                              </v-row>
                              <!-- Levels-->
                              <v-row>
                                <v-col class="py-0">
                                  <span class="heading-5 black--text ml-1">
                                    <img
                                      :src="Levels_icon"
                                      style="width: 3.5%"
                                    />
                                    Levels : {{ addWorkflowItem.WfNoOfLevels }}
                                  </span>
                                </v-col>
                              </v-row>
                              <!-- buttons-->
                              <v-row>
                                <v-col>
                                  <v-btn
                                    id="AutoTest_IP643"
                                    color="success-btn elevation-0 enter-btn"
                                    dark
                                    @click="showDetail(addWorkflowItem, index)"
                                  >
                                    Show Details
                                  </v-btn>
                                </v-col>
                              </v-row>
                            </v-card>
                          </v-col>
                        </v-row>
                        <!--Buttons-->
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right my-4 py4"
                        >
                          <Secondary-Button
                            id="AutoTest_IP644"
                            v-if="selectedAddWorkflow.length > 0"
                            class="btn-122x36 mr-2"
                            title="Confirm"
                            @click.native="fnAddSelectWorkflow(ActivityId,ActivityName)"
                          ></Secondary-Button>
                          <Secondary-Button
                            id="AutoTest_IP644"
                            v-else
                            class="btn-122x36 mr-2"
                            title="Confirm"
                            @click.native="confirmError = true"
                          ></Secondary-Button>
                          <Default-Button-Outlined
                            id="AutoTest_IP645"
                            class="btn-122x36"
                            title="Cancel"
                            @click.native="
                              (SelectWorkflowDialog = false),
                                (checkboxAddWorkflow = []),
                                (selectedAddWorkflow = []),
                                (SelectWorkflowDlgItems = []),
                                (confirmError = false)
                            "
                          ></Default-Button-Outlined>
                        </v-col>
                      </v-row>
                      <!-- if no workflow available -->
                      <v-row v-else no-gutter>
                        <v-col cols="12" sm="12" md="12">
                          <v-row :justify="justifyCenter">
                            <v-col md="6" class="text-center">
                              <v-list
                                class="base-border-radius warning-div pa-0"
                              >
                                <v-list-item>
                                  <v-list-item-icon class="mt-2">
                                    <v-avatar
                                      size="27"
                                      color="red"
                                      class="elevation-0"
                                    >
                                      <v-icon size="20" color="white"
                                        >mdi-exclamation</v-icon
                                      >
                                    </v-avatar>
                                  </v-list-item-icon>
                                  <v-list-item-content class="pb-0">
                                    <p class="pa-0">
                                      <span class="message">
                                        Sorry could not find any workflow.
                                      </span>
                                    </p>
                                  </v-list-item-content>
                                </v-list-item>
                              </v-list>
                            </v-col>
                          </v-row>
                        </v-col>
                      </v-row>
                    </v-stepper-content>

                    <!-- Show Details of Workflow-->
                    <v-stepper-content step="2" class="pa-0">
                      <v-container fluid class="pt-0">
                        <!-- Work flow group chip-->
                        <v-row>
                          <v-col sm="1">
                            <v-avatar
                              size="55"
                              tile
                              class="elevation-1 base-border-radius"
                            >
                              <v-img
                                :src="Workflow_Icon"
                                height="38"
                                width="38"
                                class="ma-4"
                              ></v-img>
                            </v-avatar>
                          </v-col>
                          <v-col sm="2" class="pt-8">
                            <span style="font-weight: bold; font-size: 18px">{{
                              ViewWorkflowItem.WfName
                            }}</span>
                          </v-col>
                          <v-col
                            style="text-align: end"
                            sm="9"
                            class="pt-6"
                            v-if="
                              selectedAddWorkflow.length > 0 &&
                              selectedAddWorkflow[0].checkboxindex ==
                                radioSelecIndex
                            "
                          >
                            <v-btn
                              elevation="1"
                              class="success-btn white--text"
                              @click="fnRemove(selectedAddWorkflow[0])"
                              >Deselect Workflow</v-btn
                            >
                          </v-col>
                          <v-col
                            style="text-align: end"
                            sm="9"
                            class="pt-6"
                            v-else
                          >
                            <v-btn
                              elevation="1"
                              class="success-btn white--text"
                              @click="
                                (checkboxAddWorkflow[radioSelecIndex] = true),
                                  fnCheck(ViewWorkflowItem, radioSelecIndex)
                              "
                              >Select Workflow</v-btn
                            >
                          </v-col>
                        </v-row>
                        <v-row
                          class="v-scrolling-div mx-4"
                          style="height: 45vh"
                          justify="center"
                        >
                          <v-col
                            md="8"
                            v-for="i in ViewWorkflowItem.WfNoOfLevels"
                            :key="i"
                          >
                            <v-card class="workspace-card pa-0" flat>
                              <v-row>
                                <v-col class="pt-0">
                                  <v-row class="pb-3">
                                    <v-col cols="12" md="12" class="py-0 pr-0">
                                      <v-row>
                                        <v-col
                                          cols="12"
                                          sm="12"
                                          class="pt-2 pb-2 pl-5"
                                        >
                                          Level {{ i }}
                                        </v-col>
                                      </v-row>
                                      <v-col cols="12" sm="12" class="pa-0">
                                        <v-divider></v-divider>
                                      </v-col>
                                      <div class="text-center">
                                        <v-progress-circular
                                          :rotate="360"
                                          :size="85"
                                          class="mt-3"
                                          :width="15"
                                          :value="
                                            (100 /
                                              ViewWorkflowItem.WfNoOfLevels) *
                                            i
                                          "
                                          color="#1EC2ED"
                                        >
                                          <span
                                            class="subtitle-1 text--primary"
                                          >
                                            {{ i }}
                                          </span>
                                        </v-progress-circular>
                                      </div>
                                    </v-col>
                                  </v-row>
                                </v-col>
                                <v-divider vertical></v-divider>
                                <v-col cols="6" md="6" class="text-left">
                                  <!---   if there is no data-->
                                  <v-row
                                    v-if="
                                      ViewWorkflowItem.WfLevelDetails.length ==
                                        0 ||
                                      ViewWorkflowItem.WfLevelDetails[i - 1]
                                        .usersList.length == 0
                                    "
                                    no-gutters
                                  >
                                    <v-col
                                      cols="12"
                                      md="12"
                                      class="pt-2 mb-0 text-center"
                                    >
                                      <p class="heading-1">No Users Added</p>
                                    </v-col>
                                  </v-row>
                                  <!-- if User Assigned-->
                                  <v-row v-else no-gutters>
                                    <v-col
                                      v-if="
                                        ViewWorkflowItem.WfLevelDetails[i - 1]
                                          .usersList.length > 0
                                      "
                                      cols="12"
                                      md="12"
                                    >
                                      <v-col class="text-center mb-0 pt-1">
                                        <v-avatar
                                          size="25"
                                          class="team-avatar"
                                          v-for="(
                                            item, key
                                          ) in ViewWorkflowItem.WfLevelDetails[
                                            i - 1
                                          ].usersList.slice(0, 3)"
                                          v-bind:key="item.ID"
                                          :color="`${getcolor(item.Name)}`"
                                        >
                                          <img
                                            v-bind:src="
                                              userimage + item.ProfileImage
                                            "
                                            alt="User DP"
                                            v-if="item.ProfileImage.length > 0"
                                          />
                                          <span class="white--text" v-else>{{
                                            item.Name == null
                                              ? ""
                                              : item.Name.substring(
                                                  0,
                                                  1
                                                ).toUpperCase()
                                          }}</span>
                                        </v-avatar>
                                        <v-avatar
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length >= 4
                                          "
                                          size="25"
                                          class="team-count"
                                        >
                                          +{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length - 3
                                          }}
                                        </v-avatar>
                                      </v-col>
                                      <v-col class="text-center mb-0 pt-0">
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length == 1
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }}
                                        </p>
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length == 2
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }},{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[1].Name
                                          }}
                                        </p>
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length > 2
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }},{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[1].Name
                                          }}
                                          +
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length
                                          }}
                                        </p>
                                      </v-col>
                                    </v-col>
                                  </v-row>
                                </v-col>
                              </v-row>
                            </v-card>
                          </v-col>
                        </v-row>
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right mt-4 py-4"
                        >
                          <Secondary-Button
                            id="AutoTest_IP648"
                            class="btn-122x36 mr-2"
                            v-if="selectedAddWorkflow.length > 0"
                            title="Confirm"
                            @click.native="fnAddSelectWorkflow(ActivityId,ActivityName)"
                          ></Secondary-Button>
                          <Secondary-Button
                            id="AutoTest_IP648"
                            class="btn-122x36 mr-2"
                            v-else
                            title="Confirm"
                            @click.native="confirmError = true"
                          ></Secondary-Button>

                          <Default-Button-Outlined
                            id="AutoTest_IP649"
                            class="btn-122x36"
                            title="Previous"
                            @click.native="SelectWorkflowDlgSteps = 1"
                          ></Default-Button-Outlined>
                        </v-col>
                      </v-container>
                    </v-stepper-content>
                  </v-stepper-items>
                </v-stepper>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-divider></v-divider>
      </v-card>
    </v-dialog>

    <!--Add Payment Dialog-->
    <v-dialog v-model="AddPaymentDialog" width="800">
        <v-card>
            <v-card-title primary-title="" class="page-head pb-0">
                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                    <v-icon size="20" color="primary">mdi-cash</v-icon>
                </v-avatar>
                Add Payment
                <v-spacer></v-spacer>
                <v-btn id="AutoTest_F160"
                       text=""
                       @click="(AddPaymentDialog = false), fnInitializeAddPayment()">
                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                </v-btn>
            </v-card-title>
            <v-form ref="AddPaymentForm"
                    v-model="valid"
                    @submit.prevent="fnAddPayment">
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">
                        Please enter the basic details of the new payment.
                    </h4>
                    <v-row justify="center" class="mt-4">
                        <v-col md="6" class="v-scrolling-div" style="height: 50vh">
                            <v-text-field outlined=""
                                          :rules="[(value) => !!value || 'Required field']"
                                          dense=""
                                          label="Enter Payment Activity Name"
                                          v-model="AddPayment.PaymentActivityName">
                                <template v-slot:append>
                                    <span v-if="AddPayment.PaymentActivityName == ''"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>
                            <v-text-field type="number"
                                          :rules="[(value) => !!value || 'Required field']"
                                          outlined=""
                                          dense=""
                                          label="Enter Transaction ID"
                                          v-model="AddPayment.TransactionId"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;"
                                          min="0">
                                <template v-slot:append>
                                    <span v-if="AddPayment.TransactionId == '' || AddPayment.TransactionId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-menu ref="AddPaymentDateMenu"
                                    v-model="AddPaymentDateMenu"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y=""
                                    min-width="290px">
                                <template v-slot:activator="{ on }">
                                    <v-text-field v-model="AddPayment.TransactionDate"
                                                  :rules="[(value) => !!value || 'Required field']"
                                                  label="Select Transaction Date"
                                                  prepend-inner-icon="mdi-calendar"
                                                  outlined=""
                                                  dense=""
                                                  readonly=""
                                                  v-on="on">
                                        <template v-slot:append>
                                            <span v-if="AddPayment.TransactionDate == '' || AddPayment.TransactionDate == 0"
                                                  class="error--text">*</span>
                                        </template>
                                    </v-text-field>
                                </template>
                                <v-date-picker v-model="AddPaymentInputDate"
                                               no-title=""
                                               scrollable="">
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_F161"
                                           text=""
                                           color="primary"
                                           @click="AddPaymentDateMenu = false">Cancel</v-btn>
                                    <v-btn id="AutoTest_F162"
                                           text=""
                                           color="primary"
                                           @click="
                      $refs.AddPaymentDateMenu.save(AddPaymentInputDate),
                        (AddPayment.TransactionDate = AddPaymentInputDate)
                    ">OK</v-btn>
                                </v-date-picker>
                            </v-menu>

                            <v-text-field type="number"
                                          :rules="[(value) => !!value || 'Required field']"
                                          outlined=""
                                          dense=""
                                          label="Enter Amount"
                                          v-model="AddPayment.TransactionAmount"
                                          onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                          onKeyPress="if(this.value.length==10) return false;"
                                          min="0">
                                <template v-slot:append>
                                    <span v-if="AddPayment.TransactionAmount == '' || AddPayment.TransactionAmount == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-text-field>

                            <v-select dense=""
                                      :rules="[(value) => !!value || 'Required field']"
                                      :items="PaymentTransactionType"
                                      label="Select Transaction type"
                                      outlined="" v-model="AddPayment.TransactionType" @input="transactionTypeChanged">
                                <template v-slot:append>
                                    <span v-if="AddPayment.TransactionType == '' || AddPayment.TransactionType == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense=""
                                      :rules="[(value) => !!value || 'Required field']"
                                      :items="AllAccountList"
                                      :item-text="(i) => i.AccountName"
                                      :item-value="(i) => i.AccountId"
                                      label="Select From Account"
                                      outlined=""
                                      v-model="AddPayment.IncomeAccountId">
                                <template v-slot:append>
                                    <span v-if="AddPayment.IncomeAccountId == '' || AddPayment.IncomeAccountId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>

                            <v-select dense=""
                                      :rules="[(value) => !!value || 'Required field']"
                                      :items="AllAccountList"
                                      :item-text="(i) => i.AccountName"
                                      :item-value="(i) => i.AccountId"
                                      label="Select To Account"
                                      outlined=""
                                      v-model="AddPayment.ExpenseAccountId">
                                <template v-slot:append>
                                    <span v-if="AddPayment.ExpenseAccountId == '' || AddPayment.ExpenseAccountId == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>
                            <v-file-input label="Upload Payment Document"
                                          prepend-inner-icon="mdi-paperclip"
                                          prepend-icon=""
                                          outlined=""
                                          dense=""
                                          v-model="AddPaymentFileInput"></v-file-input>
                            <v-select dense=""
                                      :rules="[(value) => !!value || 'Required field']"
                                      :items="PaymentStatusItems"
                                      label="Payment Status"
                                      outlined=""
                                      v-model="AddPayment.TransactionStatus">
                                <template v-slot:append>
                                    <span v-if="AddPayment.TransactionStatus == '' || AddPayment.TransactionStatus == 0"
                                          class="error--text">*</span>
                                </template>
                            </v-select>
                            <v-row>
                                <v-col>
                                    <v-select :items="BudgetHeadList"
                                              label="Budget Head"
                                              item-text="BudgetHeadName"
                                              item-value="BudgetHeadID"
                                              outlined
                                              dense
                                              @change="GetBudgetItemDetails(BudgetHead)"
                                              v-model="BudgetHead"
                                              hide-details></v-select>
                                </v-col>
                                <v-col>
                                    <v-select :items="BudgetItemList"
                                              label="Budget Item"
                                              item-text="ItemName"
                                              item-value="BudgetItemID"
                                              outlined
                                              dense
                                              v-model="BudgetItem"
                                              hide-details></v-select>
                                </v-col>
                            </v-row>

                            <v-responsive width="100%"></v-responsive>

                            <!-- Expense Tags -->
                            <span class="small-text mr-1">
                                <v-icon size="12">mdi-tag</v-icon> Expense Tags
                            </span>
                            <v-chip id="AutoTest_F163"
                                    v-for="(expenseTag, key) in AddPayment.ExpenseTags"
                                    :key="expenseTag.TagID"
                                    class="ma-2"
                                    small
                                    label
                                    close
                                    @click:close="$delete(AddPayment.ExpenseTags, key)">
                                {{ expenseTag.TagName }}
                            </v-chip>

                            <v-menu max-width="300"
                                    absolute
                                    v-model="AddPaymentExpenseTagPopUp"
                                    top
                                    :offset-y="offset"
                                    :close-on-click="closeonClick"
                                    :close-on-content-click="closeOnContentClick">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-on="on"
                                           max-height="24"
                                           max-width="24"
                                           min-width="24"
                                           min-height="24"
                                           depressed=""
                                           fab=""
                                           light=""
                                           color="#E5F2FF">
                                        <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Expense Tag pop up-->
                                <v-stepper v-model="AddPaymentExpenseTagStepper">
                                    <v-stepper-items>
                                        <v-stepper-content step="1" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    Add Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F164"
                                                           text=""
                                                           height="25"
                                                           min-width="25"
                                                           max-width="25"
                                                           color="#707070"
                                                           @click="AddPaymentExpenseTagPopUp = false">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <p class="mb-2">Search Tags</p>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined=""
                                                                          dense
                                                                          hide-details
                                                                          label="Search Tag"
                                                                          v-model="AddPaymentSearchExpenseTag">
                                                            </v-text-field>
                                                        </v-col>
                                                        <v-col md="12">
                                                            <v-chip id="AutoTest_F165"
                                                                    v-for="expenseTag in filterProjectExpenseTagList"
                                                                    :key="expenseTag.TagID"
                                                                    @click="
                                  fnAddExpenseTagToList(expenseTag, 'Add')
                                "
                                                                    class="ma-2"
                                                                    small
                                                                    label
                                                                    close
                                                                    close-icon="mdi-pencil"
                                                                    @click:close="
                                  (AddPaymentExpenseTagStepper = 3),
                                    fnInitializeEditExpenseTagItem(
                                      expenseTag,
                                      'Add'
                                    )
                                ">
                                                                {{ expenseTag.TagName }}
                                                            </v-chip>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F166"
                                                                            style="width: 130px"
                                                                            class="mb-2 center-block"
                                                                            title="Create New"
                                                                            @click.native="AddPaymentExpenseTagStepper = 2"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_F167"
                                                           height="25"
                                                           min-width="25"
                                                           max-width="25"
                                                           small
                                                           depressed
                                                           text
                                                           class="mr-4"
                                                           color="#707070"
                                                           @click="
                              (AddPaymentExpenseTagStepper = 1),
                                (AddPaymentNewExpenseTagName = '')
                            ">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Create Expense Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F168"
                                                           text=""
                                                           height="25"
                                                           min-width="25"
                                                           max-width="25"
                                                           color="#707070"
                                                           @click="
                              (AddPaymentExpenseTagPopUp = false),
                                (AddPaymentExpenseTagStepper = 1),
                                (AddPaymentNewExpenseTagName = '')
                            ">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined
                                                                          dense
                                                                          hide-details
                                                                          label="Enter Tag Name"
                                                                          v-model="AddPaymentNewExpenseTagName">
                                                            </v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F169"
                                                                            style="width: 130px"
                                                                            class="mb-2 center-block"
                                                                            title="Create New"
                                                                            @click.native="
                                  (AddPaymentExpenseTagStepper = 1),
                                    fnAddNewExpenseTag('Add')
                                "></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    <v-btn id="AutoTest_F170"
                                                           height="25"
                                                           min-width="25"
                                                           max-width="25"
                                                           small=""
                                                           depressed=""
                                                           text=""
                                                           class="mr-4"
                                                           color="#707070"
                                                           @click="
                              (AddPaymentExpenseTagStepper = 1),
                                fnInitializeEditExpenseTagItem(null, 'Add')
                            ">
                                                        <v-icon size="20">mdi-arrow-left</v-icon>
                                                    </v-btn>
                                                    Edit Tags
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_F171"
                                                           text=""
                                                           height="25"
                                                           min-width="25"
                                                           max-width="25"
                                                           color="#707070"
                                                           @click="
                              (AddPaymentExpenseTagPopUp = false),
                                (AddPaymentExpenseTagStepper = 1),
                                fnInitializeEditExpenseTagItem(null, 'Add')
                            ">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text class="v-scrolling-div">
                                                    <v-row>
                                                        <v-col md="12" align="center">
                                                            <v-text-field outlined
                                                                          dense
                                                                          hide-details
                                                                          label="Enter Tag Name"
                                                                          v-model="AddPaymentEditExpenseTagItem.TagName">
                                                            </v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row justify="center">
                                                        <v-col md="6">
                                                            <Success-Button id="AutoTest_F172"
                                                                            style="width: 130px"
                                                                            class="mb-2 center-block"
                                                                            title="Save"
                                                                            @click.native="
                                  (AddPaymentExpenseTagStepper = 1),
                                    fnEditExpenseTag('Add')
                                "></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Outlined-Button-Dark id="AutoTest_F173"
                                              class="btn-122x36 float-right"
                                              title="Cancel"
                                              @click.native="
                (AddPaymentDialog = false), fnInitializeAddPayment()
              "></Outlined-Button-Dark>
                        <Primary-Button-Light id="AutoTest_F174"
                                              class="btn-122x36 float-right mr-4"
                                              title="Confirm"
                                              typeof="submit"
                                              @click.native="fnAddPayment()"></Primary-Button-Light>
                    </v-col>
                </v-card-actions>
            </v-form>
        </v-card>
    </v-dialog>
    
    <!-- Edit Payment Ledger Book-->

    <v-dialog v-model="EditPaymentDialog" width="800">
      <v-card>
        <v-card-title primary-title="" class="page-head pb-0">
          <v-avatar color="white" size="25" class="elevation-1 mr-2">
            <v-icon size="20" color="primary">mdi-cash</v-icon>
          </v-avatar>
          Edit Payment
          <v-spacer></v-spacer>
          <v-btn
            id="AutoTest_F175"
            text=""
            @click="
              (EditPaymentDialog = false),
                fnInitializeEditPayment(),
                (EditPaymentEnable = false)
            "
          >
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-card-text>
          <h4 class="text-center heading-3 mt-4">
            Please edit the basic details of the new payment.
          </h4>
          <v-row justify="center" class="mt-4">
            <!--- View Ledger Book Details  -->
            <v-col
              md="6"
              class="v-scrolling-div"
              style="height: 40vh"
              v-if="EditPaymentEnable == false"
            >
              <v-row justify="center">
                  <v-col md="12" class="pb-0">
                      <div class="page-sub-head pb-0 text-limit">
                          Payment Activity Name
                          <v-btn id="AutoTest_F176"
                                 text
                                 max-width="16"
                                 min-width="16"
                                 height="16"
                                 @click="EditPaymentEnable = true">
                              <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                          </v-btn>
                      </div>
                      <P class="heading-1">{{ EditPayment.PaymentActivityName }}</P>

                      <div class="page-sub-head pt-0 pb-0 text-limit">
                          Transaction ID
                          <v-btn id="AutoTest_F177"
                                 text
                                 max-width="16"
                                 min-width="16"
                                 height="16"
                                 @click="EditPaymentEnable = true">
                              <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                          </v-btn>
                      </div>
                      <p class="text-left heading-1"
                         style="max-height: 120px; overflow-y: auto">
                          {{ EditPayment.TransactionId }}
                      </p>

                      <v-row no-gutters>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Transaction Date
                                  <v-btn id="AutoTest_F178"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">{{ EditPayment.TransactionDate }}</P>
                          </v-col>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Amount
                                  <v-btn id="AutoTest_F179"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">Rs. {{ EditPayment.TransactionAmount }}</P>
                          </v-col>
                      </v-row>

                      <v-select dense=""
                                :items="PaymentTransactionType"
                                label="Select Transaction type"
                                outlined="" v-model="AddPayment.TransactionType">
                      </v-select>

                      <v-row no-gutters>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Income Account
                                  <v-btn id="AutoTest_F180"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">
                                  {{
                        fnGetIncomeAccountName(EditPayment.IncomeAccountId)
                                  }}
                              </P>
                          </v-col>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Expense Account
                                  <v-btn id="AutoTest_F181"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">
                                  {{
                        fnGetExpenseAccountName(EditPayment.ExpenseAccountId)
                                  }}
                              </P>
                          </v-col>
                      </v-row>

                      <v-row no-gutters>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Payment Document
                                  <v-btn id="AutoTest_F182"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">
                                  {{
                        EditPayment.TransactionDocName
                                  }}
                              </P>
                          </v-col>
                          <v-col md="6">
                              <div class="page-sub-head pb-1 text-limit">
                                  Payment Status
                                  <v-btn id="AutoTest_F183"
                                         text
                                         max-width="16"
                                         min-width="16"
                                         height="16"
                                         @click="EditPaymentEnable = true"
                                         class="mb-1">
                                      <v-icon size="16" color="#3374B9">mdi-pencil-outline</v-icon>
                                  </v-btn>
                              </div>
                              <P class="heading-1">
                                  {{
                        EditPayment.TransactionStatus
                                  }}
                              </P>
                          </v-col>
                      </v-row>
                  </v-col>
              </v-row>
            </v-col>

            <!-- Edit Ledger Book Details-->
            <v-col md="6"
                   class="v-scrolling-div"
                   style="height: 40vh"
                   v-if="EditPaymentEnable == true">
                <v-text-field outlined=""
                              dense=""
                              label="Enter Payment Activity Name"
                              v-model="EditPayment.PaymentActivityName">
                </v-text-field>
                <v-text-field type="number"
                              outlined=""
                              dense=""
                              label="Enter Transaction ID"
                              v-model="EditPayment.TransactionId"
                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                              onKeyPress="if(this.value.length==10) return false;"
                              min="0">
                </v-text-field>

                <v-menu ref="EditPaymentDateMenu"
                        v-model="EditPaymentDateMenu"
                        :close-on-content-click="false"
                        transition="scale-transition"
                        offset-y=""
                        min-width="290px">
                    <template v-slot:activator="{ on }">
                        <v-text-field v-model="EditPayment.TransactionDate"
                                      label="Select Transaction Date"
                                      prepend-inner-icon="mdi-calendar"
                                      outlined=""
                                      dense=""
                                      readonly=""
                                      v-on="on"></v-text-field>
                    </template>
                    <v-date-picker v-model="EditPaymentInputDate"
                                   no-title=""
                                   scrollable="">
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_F184"
                               text=""
                               color="primary"
                               @click="EditPaymentDateMenu = false">Cancel</v-btn>
                        <v-btn id="AutoTest_F185"
                               text=""
                               color="primary"
                               @click="
                      $refs.EditPaymentDateMenu.save(EditPaymentInputDate),
                        (EditPayment.TransactionDate = formatDate(
                          EditPaymentInputDate
                        ))
                    ">OK</v-btn>
                    </v-date-picker>
                </v-menu>

                <v-text-field type="number"
                              outlined=""
                              dense=""
                              label="Enter Amount"
                              v-model="EditPayment.TransactionAmount"
                              onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                              onKeyPress="if(this.value.length==10) return false;"
                              min="0">
                </v-text-field>

                <v-select dense=""
                          :items="PaymentTransactionType"
                          label="Select Transaction type"
                          outlined="" v-model="EditPayment.TransactionType">
                </v-select>

                <v-select dense=""
                          v-model="EditPayment.IncomeAccountId"
                          :items="AllAccountList"
                          :item-text="(i) => i.AccountName"
                          :item-value="(i) => i.AccountId"
                          label="Select From Account"
                          outlined="">
                </v-select>

                <v-select dense=""
                          v-model="EditPayment.ExpenseAccountId"
                          :items="AllAccountList"
                          :item-text="(i) => i.AccountName"
                          :item-value="(i) => i.AccountId"
                          label="Select To Account"
                          outlined="">
                </v-select>
                <v-file-input v-model="EditPaymentFileInput"
                              label="Upload Payment Document"
                              prepend-inner-icon="mdi-paperclip"
                              prepend-icon=""
                              outlined=""
                              dense=""></v-file-input>
                <v-select dense=""
                          v-model="EditPayment.TransactionStatus"
                          :items="PaymentStatusItems"
                          label="Payment Status"
                          outlined="">
                </v-select>
            </v-col>
          </v-row>

          <!-- Expense Tag Section-->
          <v-row justify="center" class="mt-4">
            <v-col md="6">
              <span class="small-text mr-1">
                <v-icon size="12">mdi-tag</v-icon> Expense Tags
              </span>
              <v-chip
                v-if="
                  EditPayment.ExpenseTags && EditPayment.ExpenseTags.length > 0
                "
                v-for="(expenseTag, key) in EditPayment.ExpenseTags"
                :key="expenseTag.TagID"
                class="ma-2"
                small
                label
                v-bind:close="EditPaymentEnable == true"
                @click:close="$delete(EditPayment.ExpenseTags, key)"
              >
                {{ expenseTag.TagName }}
              </v-chip>

              <v-menu
                max-width="300"
                absolute
                v-model="EditPaymentExpenseTagPopUp"
                top
                :offset-y="offset"
                :close-on-click="closeonClick"
                :close-on-content-click="closeOnContentClick"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    v-on="on"
                    max-height="24"
                    max-width="24"
                    min-width="24"
                    min-height="24"
                    depressed=""
                    fab=""
                    light=""
                    color="#E5F2FF"
                    v-bind:disabled="EditPaymentEnable == false"
                  >
                    <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                  </v-btn>
                </template>
                <!--Expense Tag pop up-->
                <v-stepper v-model="EditPaymentExpenseTagStepper">
                  <v-stepper-items>
                    <v-stepper-content step="1" class="pa-0">
                      <v-card>
                        <v-card-title class="heading-3">
                          Add Expense Tags
                          <v-spacer></v-spacer>
                          <v-btn
                            id="AutoTest_F185A"
                            text=""
                            height="25"
                            min-width="25"
                            max-width="25"
                            color="#707070"
                            @click="EditPaymentExpenseTagPopUp = false"
                          >
                            <v-icon size="20">mdi-close</v-icon>
                          </v-btn>
                        </v-card-title>
                        <v-divider></v-divider>
                        <v-card-text class="v-scrolling-div">
                          <v-row>
                            <p class="mb-2">Search Tags</p>
                            <v-col md="12" align="center">
                              <v-text-field
                                outlined=""
                                dense
                                hide-details
                                label="Search Tag"
                                v-model="EditPaymentSearchExpenseTag"
                              >
                              </v-text-field>
                            </v-col>
                            <v-col md="12">
                              <v-chip
                                id="AutoTest_F186"
                                v-for="expenseTag in filterProjectExpenseTagList"
                                :key="expenseTag.TagID"
                                @click="
                                  fnAddExpenseTagToList(expenseTag, 'Edit')
                                "
                                class="ma-2"
                                small
                                label
                                close
                                close-icon="mdi-pencil"
                                @click:close="
                                  (EditPaymentExpenseTagStepper = 3),
                                    fnInitializeEditExpenseTagItem(
                                      expenseTag,
                                      'Edit'
                                    )
                                "
                              >
                                {{ expenseTag.TagName }}
                              </v-chip>
                            </v-col>
                          </v-row>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                          <v-row justify="center">
                            <v-col md="6">
                              <Success-Button
                                id="AutoTest_F187"
                                style="width: 130px"
                                class="mb-2 center-block"
                                title="Create New"
                                @click.native="EditPaymentExpenseTagStepper = 2"
                              ></Success-Button>
                            </v-col>
                          </v-row>
                        </v-card-actions>
                      </v-card>
                    </v-stepper-content>
                    <v-stepper-content step="2" class="pa-0">
                      <v-card>
                        <v-card-title class="heading-3">
                          <v-btn
                            id="AutoTest_F188"
                            height="25"
                            min-width="25"
                            max-width="25"
                            small
                            depressed
                            text
                            class="mr-4"
                            color="#707070"
                            @click="
                              (EditPaymentExpenseTagStepper = 1),
                                (EditPaymentNewExpenseTagName = '')
                            "
                          >
                            <v-icon size="20">mdi-arrow-left</v-icon>
                          </v-btn>
                          Create Expense Tags
                          <v-spacer></v-spacer>
                          <v-btn
                            id="AutoTest_F189"
                            text=""
                            height="25"
                            min-width="25"
                            max-width="25"
                            color="#707070"
                            @click="
                              (EditPaymentExpenseTagPopUp = false),
                                (EditPaymentExpenseTagStepper = 1),
                                (EditPaymentNewExpenseTagName = '')
                            "
                          >
                            <v-icon size="20">mdi-close</v-icon>
                          </v-btn>
                        </v-card-title>
                        <v-divider></v-divider>
                        <v-card-text class="v-scrolling-div">
                          <v-row>
                            <v-col md="12" align="center">
                              <v-text-field
                                outlined
                                dense
                                hide-details
                                label="Enter Tag Name"
                                v-model="EditPaymentNewExpenseTagName"
                              >
                              </v-text-field>
                            </v-col>
                          </v-row>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                          <v-row justify="center">
                            <v-col md="6">
                              <Success-Button
                                id="AutoTest_F190"
                                style="width: 130px"
                                class="mb-2 center-block"
                                title="Create New"
                                @click.native="
                                  (EditPaymentExpenseTagStepper = 1),
                                    fnAddNewExpenseTag('Edit')
                                "
                              ></Success-Button>
                            </v-col>
                          </v-row>
                        </v-card-actions>
                      </v-card>
                    </v-stepper-content>
                    <v-stepper-content step="3" class="pa-0">
                      <v-card>
                        <v-card-title class="heading-3">
                          <v-btn
                            id="AutoTest_F191"
                            height="25"
                            min-width="25"
                            max-width="25"
                            small=""
                            depressed=""
                            text=""
                            class="mr-4"
                            color="#707070"
                            @click="
                              (EditPaymentExpenseTagStepper = 1),
                                fnInitializeEditExpenseTagItem(null, 'Edit')
                            "
                          >
                            <v-icon size="20">mdi-arrow-left</v-icon>
                          </v-btn>
                          Edit Tags
                          <v-spacer></v-spacer>
                          <v-btn
                            id="AutoTest_F192"
                            text=""
                            height="25"
                            min-width="25"
                            max-width="25"
                            color="#707070"
                            @click="
                              (EditPaymentExpenseTagPopUp = false),
                                (EditPaymentExpenseTagStepper = 1),
                                fnInitializeEditExpenseTagItem(null, 'Edit')
                            "
                          >
                            <v-icon size="20">mdi-close</v-icon>
                          </v-btn>
                        </v-card-title>
                        <v-divider></v-divider>
                        <v-card-text class="v-scrolling-div">
                          <v-row>
                            <v-col md="12" align="center">
                              <v-text-field
                                outlined
                                dense
                                hide-details
                                label="Enter Tag Name"
                                v-model="EditPaymentEditExpenseTagItem.TagName"
                              >
                              </v-text-field>
                            </v-col>
                          </v-row>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                          <v-row justify="center">
                            <v-col md="6">
                              <Success-Button
                                id="AutoTest_F193"
                                style="width: 130px"
                                class="mb-2 center-block"
                                title="Save"
                                @click.native="
                                  (EditPaymentExpenseTagStepper = 1),
                                    fnEditExpenseTag('Edit')
                                "
                              ></Success-Button>
                            </v-col>
                          </v-row>
                        </v-card-actions>
                      </v-card>
                    </v-stepper-content>
                  </v-stepper-items>
                </v-stepper>
              </v-menu>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="pb-4">
          <v-col v-if="EditPaymentEnable == true">
            <Outlined-Button-Dark
              id="AutoTest_F194"
              class="btn-122x36 float-right"
              title="Cancel"
              @click.native="
                (EditPaymentDialog = false),
                  fnInitializeEditPayment(),
                  (EditPaymentEnable = false)
              "
            ></Outlined-Button-Dark>
            <Primary-Button-Light
              id="AutoTest_F195"
              v-bind:disabled="fnCheckMandatoryEditPayment()"
              class="btn-122x36 float-right mr-4"
              title="Confirm"
              @click.native="fnEditPayment()"
            ></Primary-Button-Light>
          </v-col>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Remove Payment-->
    <v-dialog v-model="RemovePaymentDialog" width="800">
      <v-card>
        <v-card-title primary-title class="page-head py-4">
          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
          </v-avatar>
          Remove Ledger
          <v-spacer></v-spacer>
          <v-btn
            id="AutoTest_F196"
            text=""
            @click="(RemovePaymentDialog = false), (RemovePaymentItem = null)"
          >
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-card-text class="pt-0">
          <v-container fluid class="pt-4">
            <v-row no-gutters>
              <v-col cols="12" md="12" class="pt-0">
                <v-container fluid class="pt-0">
                  <v-row>
                    <v-col
                      v-if="RemovePaymentItem != null"
                      cols="12"
                      md="12"
                      class="pt-0"
                    >
                      <h4 class="text-center heading-3">
                        Are you sure you want to remove this Payment ?
                      </h4>
                    </v-col>
                    <v-col v-else cols="12" md="12" class="pt-0">
                      <h4 class="text-center heading-3">
                        Are you sure you want to remove below Payments ?
                      </h4>
                    </v-col>
                    <v-col v-if="RemovePaymentItem != null">
                      <h4 class="text-center heading-3">
                        <v-avatar
                          color="white"
                          size="25"
                          class="elevation-1 mr-2 mb-1"
                        >
                          <v-icon size="20" color="primary">mdi-cash</v-icon>
                        </v-avatar>
                        {{ RemovePaymentItem.PaymentActivityName }}
                      </h4>
                    </v-col>
                    <v-col v-else>
                      <v-row
                        v-for="selecteditem in selectedPaymentList"
                        :key="selecteditem.PaymentId"
                      >
                        <v-col md="12">
                          <h4 class="text-center heading-3">
                            <v-avatar
                              color="white"
                              size="25"
                              class="elevation-1 mr-2 mb-1"
                            >
                              <v-icon size="20" color="primary"
                                >mdi-cash</v-icon
                              >
                            </v-avatar>
                            {{ selecteditem.PaymentActivityName }}
                          </h4>
                        </v-col>
                      </v-row>
                    </v-col>
                    <v-col
                      cols="12"
                      xs="12"
                      sm="12"
                      md="12"
                      class="text-right py-0"
                    >
                      <v-btn
                        id="AutoTest_F197"
                        color="secondary elevation-0"
                        class="mr-2"
                        @click="fnRemovePayment()"
                        >Remove</v-btn
                      >
                      <v-btn
                        id="AutoTest_F198"
                        outlined
                        color="grey"
                        @click="
                          (RemovePaymentDialog = false),
                            (RemovePaymentItem = null)
                        "
                        >Cancel</v-btn
                      >
                    </v-col>
                  </v-row>
                </v-container>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
      </v-card>
    </v-dialog>
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
  </v-card>
</template>
<script>
import TreeTable from "vue-tree-table-component";
import SuccessButton from "@/components/success-button.vue";
import SecondaryButton from "@/components/secondary-button.vue";
import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
import PreLoader from "@/components/pre-loader.vue";
import OutlinedButtonDark from "@/components/outlined-button-dark.vue";
import PrimaryButtonLight from "@/components/primary-button-light.vue";
import objUtils from '../../utils.js'

var objManageProjectActivity;
async function importscript() {
  return Promise.all([
    import("../../BL/ManageProjectActivity.js").then((mod) => {
      objManageProjectActivity = new mod.ManageProjectActivity();
    }),
  ]);
}
    var objManageProjectAccount;
    async function importAccountscript() {
        return Promise.all([
            import("../../BL/ManageProjectAccount.js").then(mod => {
                objManageProjectAccount = new mod.ManageProjectAccount();
            })
        ]);
    };

var objFinance;
async function importFinancescript() {
  return Promise.all([
    import("../../BL/Finance.js").then((mod) => {
      objFinance = new mod.Finance();
    }),
  ]);
}
var objManageProjectLedgerBook;
async function importLedgerscript() {
  return Promise.all([
    import("../../BL/ManageProjectLedgerBook.js").then((mod) => {
      objManageProjectLedgerBook = new mod.ManageProjectLedgerBook();
    }),
  ]);
}

export default {
  name: "DefaultTableTree",
  data() {
      return {
      valid: true,
      confirmError: false,
      Description_icon: require("@/assets/ProjectWorkflow/Description_icon.svg"),
      Levels_icon: require("@/assets/ProjectWorkflow/Levels_icon.svg"),
      Workflow_Icon: require("@/assets/ProjectWorkflow/Workflow_Icon.svg"),
      Workflow_Tab_icon: require("@/assets/ProjectWorkflow/Workflow_Tab_icon.svg"),
      radioSelecIndex: "",
      rules: {
        required: (value) => !!value || "Required.",
      },
      screenAccess: 0,
      RWAccess: 0,
      successSnackbarMsg: false,
      errorSnackbarMsg: false,
      infoSnackbarMsg: false,
      Msg: "",
      justifyCenter: "center",
      //Table Pagination
      currentPage: 0,
      recordsPerPage: 10,
      //General parameters
      WorkspaceID: 0,
      initiativeInfo: {
        InitiativeId: 0,
      },
      projectInfo: {
        ProjectId: 0,
        ProjectName: "",
        Description: "",
        Code: "",
        Budget: 0,
        StartDate: "",
        EndDate: "",
        CountryID: 0,
        StateID: 0,
        DistrictID: 0,
        ProjectTags: "",
        ProjectTagsList: [],
        GroupId: 0,
      },

      //Listing parameters
      ActivityList: [],
      filterActivityList: [],
      SearchActivity: "",
      ActivityListHeader: [
        { label: "Name", id: "ActivityName", props: "ActivityName" },
        {
          label: "Budget",
          id: "PaymentLinkedBudget",
          props: "PaymentLinkedBudget",
        },
        { label: "Progress", id: "progress", props: "progress" },
        { label: "Workflow", id: "Workflow", props: "Workflow" },
        { label: "Action", id: "action", props: "action" },
      ],

      step: 50,
      ShowAccountLedger: false,
      LedgerBookTableheaders: [
        { text: "Title", value: "PaymentActivityName" },
        { text: "Transaction ID", value: "TransactionId" },
        { text: "Transaction Date", value: "TransactionDate" },
        { text: "Amount", value: "TransactionAmount" },
        { text: "Income Account", value: "IncomeAccountId" },
        { text: "Expense Account", value: "ExpenseAccountId" },
        { text: "Transaction Type", value: "TransactionType" },
        { text: "Documents", value: "TransactionDocName" },
        { text: "Payment Status", value: "TransactionStatus" },
        { text: "Action", value: "LedgerTableActions" },
      ],
      filterPaymentList: [],
      SearchfilterPaymentList: "",
      RemovePaymentDialog: false,
      PaymentList: [],
      selectedPaymentList: [],
      IncomeAccountList: [],
      AllAccountList:[],
      ExpenseAccountList: [],
      PaymentTransactionType: ['Credit', 'Debit'],
      PaymentStatusItems: ["Approved", "Rejected"],
      ActivityId: 0,
      ActivityName: "",
      offset: true,
      closeonClick: false,
      closeOnContentClick: false,

      //Select workflow starts here
      SelectWorkflowDialog: false,
      SelectWorkflowDlgSteps: 0,
      SelectWorkflowDlgItems: [],
      checkboxAddWorkflow: [],
      selectedAddWorkflow: [],
      
      ViewWorkflowItem: { WfLevelDetails: [] },
      //Select workflow ends here

      // Loader
      PreLoader: false,

      // Activity Payment
      AddPayment: {
        PaymentActivityName: "",
        TransactionId: 0,
        TransactionDate: "",
        TransactionAmount: 0,
        IncomeAccountId: 0,
        ExpenseAccountId: 0,
        TransactionType: "",
        TransactionDocName: "",
        TransactionStatus: "",
        IsActivityLinked: true,
        ActivityId: 0,
        ExpenseTags: [],
      },
      AddPaymentDialog: false,

      AddPaymentFileInput: null,
      AddPaymentInputDate: "",
      
      AddPaymentDateMenu: false,
      AddPaymentExpenseTagStepper: 1,
      AddPaymentExpenseTagPopUp: false,
      AddPaymentNewExpenseTagName: "",
      AddPaymentSearchExpenseTag: "",
      AddPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },
      ProjectExpenseTagList: [],
      filterProjectExpenseTagList: [],
      BudgetHeadList: [],
      BudgetHead: "",
      BudgetItemList: [],
      BudgetItem: "",
      //Edit Payment parameters
      EditPayment: {
        PaymentId: 0,
        PaymentActivityName: "",
        TransactionId: 0,
        TransactionDate: "",
        TransactionAmount: 0,
        IncomeAccountId: 0,
        ExpenseAccountId: 0,
        TransactionType: "",
        TransactionDocName: "",
        TransactionStatus: "",
        IsActivityLinked: false,
        ActivityId: 0,
        ExpenseTags: [],
      },
      EditPaymentFileInput: null,
      EditPaymentInputDate: "",
      EditPaymentMessage: "",
      EditPaymentDialog: false,
      
      EditPaymentDateMenu: false,
      EditPaymentExpenseTagStepper: 1,
      EditPaymentExpenseTagPopUp: false,
      EditPaymentNewExpenseTagName: "",
      EditPaymentSearchExpenseTag: "",
      EditPaymentEditExpenseTagItem: { TagID: 0, TagName: "" },
      EditPaymentEnable: false,
      //Remove Payment both Bulk and Single parameters
      RemovePaymentItem: null,
      
      userimage: "",
    };
  },

  components: {
    "Success-Button": SuccessButton,
    "Secondary-Button": SecondaryButton,
    "Default-Button-Outlined": DefaultButtonOutlined,
    "Outlined-Button-Dark": OutlinedButtonDark,
    "Primary-Button-Light": PrimaryButtonLight,
    "Pre-Loader": PreLoader,
    TreeTable,
  },

  mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
    this.userimage = window.SERVER_URL + "/Upload";
    this.screenAccess = this.$route.query.Access == 3 ? true : false;
    this.initiativeInfo.InitiativeId = this.$route.query.InitID;
    this.projectInfo.ProjectId = this.$route.query.ProjID;
    this.ProjectAccountID = this.$route.query.ProjID;
    await importscript();
    await importAccountscript();
    await importFinancescript();
    await importLedgerscript();
    await this.fnGetProjectAccountsByID();
    await this.fnGetProjectActivityList();
    await this.GetBudgetHeadDetails();
    await this.fnGetIncomeAccounts();
    await this.fnGetExpenseAccounts();
    await this.fnGetAllExpenseTags();
  },
  computed: {
    //Pagination
    paginatedData: function () {
      const firstIndexOnPage = this.currentPage * this.recordsPerPage;
      const firstIndexOnNextPage = (this.currentPage + 1) * this.recordsPerPage;
      return this.filterActivityList.slice(
        firstIndexOnPage,
        firstIndexOnNextPage
      );
    },
  },
  methods: {
    /// <summary>
    ///  showDetail :- Function used to show details of workflow items
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <returns> </returns>
    showDetail(data, index) {
      this.SelectWorkflowDlgSteps = 2;
      this.ViewWorkflowItem = data;
      this.radioSelecIndex = index;
    },

    /// <summary>
    ///  getcolor :- Function used to get color of users
    /// </summary>
    /// <param name="name"></param>
    /// <returns> </returns>
    getcolor(name) {
        return objUtils.common.getcolor(name);
    },

    //Pagination
    /// <summary>
    ///  numberOfPages :- Function used to show total number of pages under the table listed
    /// </summary>
    /// <param name=""></param>
    /// <returns> number of pages </returns>
    numberOfPages() {
      return Math.ceil(this.filterActivityList.length / this.recordsPerPage);
    },

    /// <summary>
    ///  isFirstPage :- Function used to check if the current page is first page
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    isFirstPage() {
      return this.currentPage == 0;
    },

    /// <summary>
    ///  isLastPage :- Function used to check if the current page is last page
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    isLastPage() {
      const firstIndexOnNextPage = (this.currentPage + 1) * this.recordsPerPage;
      return firstIndexOnNextPage > this.filterActivityList.length;
    },

     /// <summary>
     ///  transactionTypeChanged :- Function used to select transaction type (Credit/Debit)
     /// </summary>
     /// <param name="item"></param>
     /// <returns> </returns>
     transactionTypeChanged(item) {
         if (item === "Credit") {
             this.AddPayment.IncomeAccountId = 87
              this.AddPayment.ExpenseAccountId = 0
         } else if (item === "Debit") {
              this.AddPayment.ExpenseAccountId = 87
              this.AddPayment.IncomeAccountId = 0
         }
     },

     /// <summary>
     /// fnGetProjectActivityList :- Function to get Project Activity List
     /// </summary>
     /// <param name=""></param>
     /// <returns> </returns>
     async fnGetProjectActivityList() {
         this.PreLoader = true;
         var data = new FormData();
         data.append("ProjectId", this.projectInfo.ProjectId);
         var result = await objFinance.GetFinanceActivityList(data);

         if (result.status == "Unhandled Exception") {
             this.$router.push({ name: "error" });
         } else if (result.data) {
             this.filterActivityList = result.data;
             this.ActivityList = result.data;
             this.ParentActivityList = this.ActivityList.filter((a) => a.ParentActivityId == 0);
         } else {
             this.Msg = "Failed to get activity list of Project " + this.projectInfo.ProjectName;
             this.errorSnackbarMsg = true;
         }
         this.PreLoader = false;
     },

     /// <summary>
     /// fnGetGrandChildrenActivity :- Function to get Sub Activities of an Activity
     /// </summary>
     /// <param name="actitem"></param>
     /// <returns> </returns>
     async fnGetGrandChildrenActivity(actitem) {
         if (actitem.isOpen == false) {
             return;
         }
         var child = actitem.children;
         for (var i = 0; i < child.length; i++) {
             var result = await objManageProjectActivity.GetSubActivityListByID(child[i].ActivityId);

             if (result.status == "Unhandled Exception") {
                 this.$router.push({ name: "error" });
             } else if (result.data) {
                 child[i].children = result.data;
             } else {
                 this.Msg = "Failed to get sub-activity list of Activity " + oneact.ActivityName;
                 this.errorSnackbarMsg = true;
             }
         }
     },

    // Reassign Workflow

     /// <summary>
     /// GetWorkflowsListToactivity :- Function to fetch Workflows List to Activity
     /// </summary>
     /// <param name="item"></param>
     /// <returns> </returns>
     async GetWorkflowsListToactivity(item) {
         var data = new FormData();
         data.append("ProjectId", this.projectInfo.ProjectId);
         data.append("WorkspaceId", this.WorkspaceID);
         var result = await objFinance.GetWorkflowsListToactivity(data);
         if (result.status == "Unhandled Exception") {
             this.$router.push({ name: "error" });
         } else if (result.data) {
             this.SelectWorkflowDlgItems = result.data;
             this.filterWorkflowsList = result.data;
             var ID;
             var temp = "";
             for (ID = 0; ID <= this.SelectWorkflowDlgItems.length - 1; ID++) {
                 if (this.SelectWorkflowDlgItems[ID].WorkflowID == item.WorkflowId) {
                     temp = this.SelectWorkflowDlgItems.filter((x) => x.WorkflowID == item.WorkflowId);
                 }
                 if (temp.length > 0) {
                     this.selectedAddWorkflow.push({
                         checkboxindex: ID,
                         WFitem: temp[0],
                     });
                     this.checkboxAddWorkflow[ID] = true;
                 }
                 temp = [];
             }
         }
     },

     /// <summary>
     ///  fnAddSelectWorkflow :- Function used to Add Selected Workflow to Activity
     /// </summary>
     /// <param name="ActivityId"></param>
     /// <param name="ActivityName"></param>
     /// <returns> </returns>
     async fnAddSelectWorkflow(ActivityId, ActivityName) {
         var data = new FormData();
         data.append("FinanceWorkflowId", this.selectedAddWorkflow[0].WFitem.WorkflowID);
         data.append("ProjectId", this.projectInfo.ProjectId);
         data.append("ActivityId", ActivityId);
         data.append("ActivityName", ActivityName);
         var result = await objFinance.UpdateFinanceWFToActivity(data);
         if (result.status == "Unhandled Exception") {
             this.$router.push({ name: "error" });
         } else if (result.status == "success") {
             this.Msg = "Workflow(s) successfully assigned to Activity";
             this.successSnackbarMsg = true;
             this.SelectWorkflowDialog = false;
             this.SelectWorkflowDlgSteps = 1;
             this.SelectWorkflowDlgItems = [];
             this.checkboxAddWorkflow = [];
             this.selectedAddWorkflow = [];
             this.fnGetProjectActivityList();
         }
     },

     /// <summary>
     ///  fnCheck :- Function used to Check Workflows 
     /// </summary>
     /// <param name="item"></param>
     /// <param name="index"></param>
     /// <returns> </returns>
     fnCheck(item, index) {
         var i;
         var preval = this.checkboxAddWorkflow[index];
         this.selectedAddWorkflow = [];
         for (i = 0; i <= this.SelectWorkflowDlgItems.length - 1; i++) {
             this.checkboxAddWorkflow[i] = false;
         }
         if (preval) {
             this.checkboxAddWorkflow[index] = true;
             this.selectedAddWorkflow.push({ checkboxindex: index, WFitem: item });
             this.confirmError = false;
         } else {
             if (this.selectedAddWorkflow.length > 0) {
                 this.selectedAddWorkflow = this.selectedAddWorkflow.filter((x) => x.WFitem.WorkflowID != item.WorkflowID);
             }
         }
     },

     /// <summary>
     ///  fnRemove :- Function used to Remove Selected Workflow from Add Workflow Page
     /// </summary>
     /// <param name="item"></param>
     /// <returns> </returns>
     fnRemove(item) {
         this.checkboxAddWorkflow[item.checkboxindex] = false;
         this.selectedAddWorkflow = this.selectedAddWorkflow.filter((x) => x.WFitem.WorkflowID != item.WFitem.WorkflowID);
     },
    // ---------------- Reassign Workflow ends here

    // --- Activity Ledger Methods
     /// <summary>
     ///  fnInitializeEditPayment :- Function used to set default values in Edit Payment Variables when click on close button
     /// </summary>
     /// <param name=""></param>
     /// <returns> </returns>
     fnInitializeEditPayment() {
         this.EditPayment.PaymentId = 0;
         this.EditPayment.PaymentActivityName = "";
         this.EditPayment.TransactionId = 0;
         this.EditPayment.TransactionDate = "";
         this.EditPayment.TransactionAmount = 0;
         this.EditPayment.IncomeAccountId = 0;
         this.EditPayment.ExpenseAccountId = 0;
         this.EditPayment.TransactionType = "";
         this.EditPayment.TransactionDocName = "";
         this.EditPayment.TransactionStatus = "";
         this.EditPayment.IsActivityLinked = false;
         this.EditPayment.ActivityId = 0;
         this.EditPayment.ExpenseTags = [];
         this.EditPaymentFileInput = null;
     },

     /// <summary>
     ///  onFileUpload :- Function used to Upload Files
     /// </summary>
     /// <param name="addfile"></param>
     /// <param name="actname"></param>
     /// <returns></returns>
     async onFileUpload(addfile, actname) {
         var lstModDate = new Date(addfile.lastModifiedDate).toISOString().substr(0, 10) + " " + new Date(addfile.lastModifiedDate).toISOString().substr(11, 5);
         const formData = new FormData();
         formData.append("myFile", addfile);
         formData.append("DocumentOriginalName", addfile.name.split(".").slice(0, -1).join("."));
         formData.append("DocumentSize", objUtils.common.formatFileSize(addfile.size, 0));
         formData.append("DocumentType", addfile.type);
         formData.append("DocumentTypeIcon", addfile.name.split(".").pop());
         formData.append("DocumentLastModifiedDate", lstModDate);
         formData.append("ProjectId", this.ProjectID);
         formData.append("UploadedFrom", "Payment");
         formData.append("LinkedTo", actname);

         var result = await objManageProjectLedgerBook.UploadDocument(formData);

         if (result.status == "Unhandled Exception") {
             this.$router.push({ name: "error" });
         }
         else {
             if (result.status == "success") {
                 return true;
             }
             else {
                 return false;
             }
         }
     },

     /// <summary>
     ///  fnEditPayment :- Function used to Edit Payment
     /// </summary>
     /// <param name=""></param>
     /// <returns> </returns>
     async fnEditPayment() {
         this.EditPaymentDialog = false;
         this.Msg = "Payment updated successfully";
         this.successSnackbarMsg = true;
         this.EditPaymentEnable = false;
         if (this.EditPaymentFileInput != null && this.EditPaymentFileInput.Name != "" &&this.EditPaymentFileInput.Name != this.EditPayment.TransactionDocName) {
             this.EditPayment.TransactionDocName = this.EditPaymentFileInput.name;
             var ret = this.onFileUpload(this.EditPaymentFileInput, this.EditPayment.PaymentActivityName);
             if (ret == false) {
                 this.EditPaymentMessage = "Failed to upload the document from Edit Payment";
                 return;
             }
         }

         var data = new FormData();
         data.append("PaymentId", this.EditPayment.PaymentId);
         data.append("PaymentActivityName", this.EditPayment.PaymentActivityName);
         data.append("TransactionId", this.EditPayment.TransactionId);
         data.append("TransactionDate", this.EditPayment.TransactionDate);
         data.append("TransactionAmount", this.EditPayment.TransactionAmount);
         data.append("IncomeAccountId", this.EditPayment.IncomeAccountId);
         data.append("ExpenseAccountId", this.EditPayment.ExpenseAccountId);
         data.append("TransactionType", this.EditPayment.TransactionType);
         if (this.EditPaymentFileInput != null) {
             data.append("TransactionDocName", this.EditPayment.TransactionDocName);
         }
         data.append("TransactionStatus", this.EditPayment.TransactionStatus);
         data.append("IsActivityLinked", this.EditPayment.IsActivityLinked);
         data.append("ActivityId", this.EditPayment.ActivityId);
         data.append("ExpenseTagsList", JSON.stringify(this.EditPayment.ExpenseTags));
         data.append("ProjectId", this.ProjectID);
         var result = await objManageProjectLedgerBook.EditPayment(data);

         if (result.status == "Unhandled Exception") {
             this.$router.push({ name: "error" });
         } else if (result.status == "success") {
             this.Msg = "Payment updated successfully";
             this.successSnackbarMsg = true;
             this.fnInitializeEditPayment();
             this.GetPaymentListOfActivicty();
         } else {
             this.Msg = "Failed";
             this.errorSnackbarMsg = true;
         }
     },

     /// <summary>
     ///  fnGetIncomeAccountName :- Function used to Get Income Account Name based on acctid
     /// </summary>
     /// <param name="acctid"></param>
     /// <returns> </returns>
     fnGetIncomeAccountName(acctid) {
         var temp = this.IncomeAccountList.filter((x) => x.AccountId == acctid);
         if (temp.length > 0) {
             return temp[0].AccountName;
         }
         else {
             return "";
         }
     },

     /// <summary>
     ///  fnGetExpenseAccountName :- Function used to Get Expense Account Name based on acctid
     /// </summary>
     /// <param name="acctid"></param>
     /// <returns> </returns>
     fnGetExpenseAccountName(acctid) {
         var temp = this.ExpenseAccountList.filter((x) => x.AccountId == acctid);
         if (temp.length > 0) {
             return temp[0].AccountName;
         }
         else {
             return "";
         }
     },

     /// <summary>
     ///  fnInitializeAddPayment :- Function used to set default values in Add Payment variables when click on close button
     /// </summary>
     /// <param name=""></param>
     /// <returns> </returns>
     fnInitializeAddPayment() {
         this.AddPayment.PaymentActivityName = "";
         this.AddPayment.TransactionId = 0;
         this.AddPayment.TransactionDate = "";
         this.AddPayment.TransactionAmount = 0;
         this.AddPayment.IncomeAccountId = 0;
         this.AddPayment.ExpenseAccountId = 0;
         this.AddPayment.TransactionType= "";
         this.AddPayment.TransactionDocName = "";
         this.AddPayment.TransactionStatus = "";
         this.AddPayment.IsActivityLinked = true;
         this.AddPayment.ActivityId = 0;
         this.AddPayment.ExpenseTags = [];
         this.AddPaymentFileInput = null;
         this.BudgetHead = 0;
         this.BudgetItem = 0;
         this.$refs.AddPaymentForm.resetValidation();
     },

     /// <summary>
     ///  fnCheckMandatoryEditPayment :- Function used to check required field is showing blank or null in Edit Payment section
     /// </summary>
     /// <param name=""></param>
     /// <returns> true/false</returns>
     fnCheckMandatoryEditPayment() {
         if (this.EditPayment.PaymentActivityName == "" ||
             this.EditPayment.TransactionId == 0 ||
             this.EditPayment.TransactionDate == "" ||
             this.EditPayment.TransactionAmount == 0 ||
             this.EditPayment.IncomeAccountId == 0 ||
             this.EditPayment.ExpenseAccountId == 0 ||
             this.EditPayment.TransactionType == "" ||
             this.EditPayment.TransactionStatus == "") {
             return true;
         }
         else {
             return false;
         }
     },

     /// <summary>
     ///  fnInitializeEditPaymentItem :- Function used to initialize payment variables for Edit Payment Items
     /// </summary>
     /// <param name="item"></param>
     /// <returns> </returns>
     fnInitializeEditPaymentItem(item) {
        this.EditPayment.PaymentId = item.PaymentId;
        this.EditPayment.PaymentActivityName = item.PaymentActivityName;
        this.EditPayment.TransactionId = item.TransactionId;
        this.EditPayment.TransactionDate = this.formatDate(item.TransactionDate);
        this.EditPayment.TransactionAmount = item.TransactionAmount;
        this.EditPayment.IncomeAccountId = item.IncomeAccountId;
        this.EditPayment.ExpenseAccountId = item.ExpenseAccountId;
        this.EditPayment.TransactionType = item.TransactionType;
        if (item.TransactionDocName != null && item.TransactionDocName.length > 0) {
            this.EditPaymentFileInput = new File([], item.TransactionDocName);
        }
        this.EditPayment.TransactionDocName = item.TransactionDocName;
        this.EditPayment.TransactionStatus = item.TransactionStatus;
        this.EditPayment.IsActivityLinked = item.IsActivityLinked;
        this.EditPayment.ActivityId = item.ActivityId;
        if (item.ExpenseTags) {
           item.ExpenseTags.forEach((u) => {
              this.EditPayment.ExpenseTags.push({
                 TagID: u.TagID,
                 TagName: u.TagName,
              });
           });
        }
     },

     /// <summary>
     ///  fnRemovePayment :- Function used to Delete Payment
     /// </summary>
     /// <param name=""></param>
     /// <returns></returns>
     async fnRemovePayment() {
        this.RemovePaymentDialog = false;
        var data = new FormData();
        var paylist = [];
        if (this.RemovePaymentItem) {
           data.append("PaymentIdList", this.RemovePaymentItem.PaymentId);
        } else {
           for (var i = 0; i < this.selectedPaymentList.length; i++) {
              paylist.push(this.selectedPaymentList[i].PaymentId);
           }
           var payids = paylist.toString();
           data.append("PaymentIdList", payids);
        }
        var result = await objManageProjectLedgerBook.RemovePayment(data);

        if (result.status == "Unhandled Exception") {
           this.$router.push({ name: "error" });
        } else if (result.status == "success") {
             this.Msg = "Payment removed successfully";
             this.successSnackbarMsg = true;
             this.RemovePaymentItem = null;
             this.selectedPaymentList = [];
             this.GetPaymentListOfActivicty();
        } else {
             this.Msg = "Failed";
             this.errorSnackbarMsg = true;
        }
     },

     /// <summary>
     ///  GetPaymentListOfActivicty :- Function used to fetch Payment List of Activity
     /// </summary>
     /// <param name=""></param>
     /// <returns></returns>
     async GetPaymentListOfActivicty() {
        var data = new FormData();
        data.append("ProjectId", this.projectInfo.ProjectId);
        data.append("ActivityId", this.ActivityId);
        var result = await objFinance.GetPaymentListOfActivicty(data);

        if (result.status == "Unhandled Exception") {
           this.$router.push({ name: "error" });
        } else if (result.data) {
           this.filterPaymentList = result.data;
           this.PaymentList = result.data;
        } else {
           this.Msg = "Failed";
           this.errorSnackbarMsg = true;
        }
     },

     /// <summary>
     ///  GetBudgetHeadDetails :- Function used to fetch Budget Head Details based on ProjectId
     /// </summary>
     /// <param name=""></param>
     /// <returns></returns>
     async GetBudgetHeadDetails() {
        var data = new FormData();
        data.append("ProjectId", this.projectInfo.ProjectId);
        var result = await objFinance.GetBudgetHeadDetails(data);
        if (result.status == "Unhandled Exception") {
            this.$router.push({ name: "error" });
        }
        else {
            if (result.data) {
                this.BudgetHeadList = result.data;
            }
        }
     },

     /// <summary>
     ///  GetBudgetItemDetails :- Function used to fetch Budget Item Details based on BudgetHeadID
     /// </summary>
     /// <param name="BudgetHeadID"></param>
     /// <returns> </returns>
     async GetBudgetItemDetails(BudgetHeadID) {
        var data = new FormData();
        data.append("BudgetHeadID", BudgetHeadID);
        var result = await objFinance.GetBudgetItemDetails(data);
        if (result.status == "Unhandled Exception") {
           this.$router.push({ name: "error" });
        }
        else {
           if (result.data) {
               this.BudgetItemList = result.data;
           }
        }
     },

      /// <summary>
      ///  fnAddPayment :- Function used to Add Payment to the Project
      /// </summary>
      /// <param name=""></param>
      /// <returns></returns>
      async fnAddPayment() {
          if (this.$refs.AddPaymentForm.validate() === true) {
              this.AddPaymentDialog = false;
              if (this.AddPaymentFileInput) {
                  this.AddPayment.TransactionDocName = this.AddPaymentFileInput.name;
                  var ret = this.onFileUpload(this.AddPaymentFileInput, this.AddPayment.PaymentActivityName);
                  if (ret == false) {
                      this.Msg = "Failed";
                      this.errorSnackbarMsg = true;
                      return;
                  }
              }

              var data = new FormData();
              data.append("PaymentActivityName", this.AddPayment.PaymentActivityName);
              data.append("TransactionId", this.AddPayment.TransactionId);
              data.append("TransactionDate", this.AddPayment.TransactionDate);
              data.append("TransactionAmount", this.AddPayment.TransactionAmount);
              data.append("IncomeAccountId", this.AddPayment.IncomeAccountId);
              data.append("ExpenseAccountId", this.AddPayment.ExpenseAccountId);
              data.append("TransactionType", this.AddPayment.TransactionType);
              data.append("TransactionDocName", this.AddPayment.TransactionDocName);
              data.append("TransactionStatus", this.AddPayment.TransactionStatus);
              data.append("IsActivityLinked", this.AddPayment.IsActivityLinked);
              data.append("ActivityId", this.ActivityId);
              data.append("ExpenseTagsList", JSON.stringify(this.AddPayment.ExpenseTags));
              data.append("ProjectId", this.projectInfo.ProjectId);
              data.append("BudgetHeadID", this.BudgetHead);
              data.append("BudgetItemID", this.BudgetItem);
              var result = await objManageProjectLedgerBook.AddPayment(data);

              if (result.status == "Unhandled Exception") {
                  this.$router.push({ name: "error" });
              } else if (result.status == "success") {
                  this.Msg = "Successfully added payment to the project";
                  this.successSnackbarMsg = true;
                  this.fnInitializeAddPayment();
                  this.$refs.AddPaymentForm.resetValidation();
                  this.GetPaymentListOfActivicty();
                  this.fnGetAllExpenseTags();
              } else {
                  this.Msg = "Failed";
                  this.errorSnackbarMsg = true;
              }
          }
      },

      /// <summary>
      ///  fnGetProjectAccountsByID :- Function used to fetch Project Account Details based on ProjectId
      /// </summary>
      /// <param name=""></param>
      /// <returns></returns>
      async fnGetProjectAccountsByID() {
          var data = new FormData();
          data.append("ProjectId", this.ProjectAccountID);
          var result = await objManageProjectAccount.GetProjectAccountsByID(data);

          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else if (result.data) {
              this.AllAccountList = result.data;
          } else {
              this.Msg = ("Failed to get details of Account for the project " + this.projectInfo.ProjectName);
              this.errorSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  fnGetIncomeAccounts :- Function used to fetch Project Income Accounts details
      /// </summary>
      /// <param name=""></param>
      /// <returns></returns>
      async fnGetIncomeAccounts() {
          var data = new FormData();
          data.append("ProjectId", this.projectInfo.ProjectId);
          data.append("AccountType", "Income Account");
          var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else if (result.data) {
              this.IncomeAccountList = result.data;
          } else {
              this.Msg = "Failed";
              this.errorSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  fnGetExpenseAccounts :- Function used to fetch Project Expense Accounts details
      /// </summary>
      /// <param name=""></param>
      /// <returns></returns>
      async fnGetExpenseAccounts() {
          var data = new FormData();
          data.append("ProjectId", this.projectInfo.ProjectId);
          data.append("AccountType", "Expense Account");
          var result = await objManageProjectLedgerBook.GetProjectAccountsByType(data);

          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else if (result.data) {
              this.ExpenseAccountList = result.data;
          } else {
              this.Msg = "Failed";
              this.errorSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  formatDate :- Function used to formatting a date
      /// </summary>
      /// <param name="date"></param>
      /// <returns> </returns>
      formatDate(date) {
          return objUtils.common.formatDate(date);
      },

    //Expense Tags functions

      /// <summary>
      ///  fnGetAllExpenseTags :- Function used to fetch All Project Expense Tag based on ProjectId
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async fnGetAllExpenseTags() {
          var result = await objManageProjectLedgerBook.GetAllExpenseTags(this.ProjectID);

          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else if (result.data) {
              this.ProjectExpenseTagList = result.data;
              this.filterProjectExpenseTagList = result.data;
          } else {
              this.Msg = "Failed to get all expense tag list of Project " + this.projectInfo.ProjectName;
              this.errorSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  fnAddNewExpenseTag :- Function used to Create New Project Expense Tag
      /// </summary>
      /// <param name="type"></param>
      /// <returns> </returns>
      async fnAddNewExpenseTag(type) {
          var tagname = "";
          switch (type) {
              case "Add":
                 tagname = this.AddPaymentNewExpenseTagName;
                 break;
              default:
                 tagname = this.EditPaymentNewExpenseTagName;
                 break;
          }
          var temp = this.ProjectExpenseTagList.filter((x) => x.TagName.includes(tagname));
          if (temp.length > 0) {
              this.Msg = "TagName you entered already exists. Please enter a new tag name.";
              this.infoSnackbarMsg = true;
              return;
          }
          var data = new FormData();
          data.append("ProjectId", this.ProjectID);
          data.append("TagName", tagname);
          var result = await objManageProjectLedgerBook.AddNewExpenseTag(data);
          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else {
              if (result.status == "success") {
                  switch (type) {
                      case "Add":
                          this.AddPaymentNewExpenseTagName = "";
                          break;
                      default:
                          this.EditPaymentNewExpenseTagName = "";
                          break;
                  }
                  this.fnGetAllExpenseTags();
              } else {
                  this.Msg = "Failed to add expense tag.";
                  this.errorSnackbarMsg = true;
              }
          }
      },

      /// <summary>
      ///  fnAddExpenseTagToList :- Function used to Add Expense Tag to the List
      /// </summary>
      /// <param name="item"></param>
      /// <param name="type"></param>
      /// <returns> </returns>
      fnAddExpenseTagToList(item, type) {
          switch (type) {
              case "Add":
                  if (this.AddPayment.ExpenseTags.length == 0) {
                      this.AddPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                  }
                  else {
                      var temp = this.AddPayment.ExpenseTags.filter((x) => x.TagID == item.TagID);
                      if (temp.length == 0) {
                          this.AddPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                      }
                  }
                  break;
              case "Edit":
                  if (this.EditPayment.ExpenseTags.length == 0) {
                      this.EditPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                  }
                  else {
                      var temp = this.EditPayment.ExpenseTags.filter((x) => x.TagID == item.TagID);
                      if (temp.length == 0) {
                          this.EditPayment.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName, });
                      }
                  }
                  break;
          }
      },

      /// <summary>
      ///  fnInitializeEditExpenseTagItem :- Function used to open edit expense tag item box, when click on the edit button from the list
      ///  initialize expense tag variables
      /// </summary>
      /// <param name="exptagitem = null"></param>
      /// <param name="type"></param>
      /// <returns> </returns>
      fnInitializeEditExpenseTagItem(exptagitem = null, type) {
          switch (type) {
              case "Add":
                 if (exptagitem == null) {
                     this.AddPaymentEditExpenseTagItem.TagID = 0;
                     this.AddPaymentEditExpenseTagItem.TagName = "";
                 } else {
                     this.AddPaymentEditExpenseTagItem.TagID = exptagitem.TagID;
                      this.AddPaymentEditExpenseTagItem.TagName = exptagitem.TagName;
                 }
                 break;
              default:
                 if (exptagitem == null) {
                     this.EditPaymentEditExpenseTagItem.TagID = 0;
                     this.EditPaymentEditExpenseTagItem.TagName = "";
                 } else {
                     this.EditPaymentEditExpenseTagItem.TagID = exptagitem.TagID;
                     this.EditPaymentEditExpenseTagItem.TagName = exptagitem.TagName;
                 }
                 break;
          }
      },

      /// <summary>
      ///  fnEditExpenseTag :- Function used to Update Project Expense Tag
      /// </summary>
      /// <param name="type"></param>
      /// <returns> </returns>
      async fnEditExpenseTag(type) {
          var tagid = 0;
          var tagname = "";
          switch (type) {
              case "Add":
                 tagid = this.AddPaymentEditExpenseTagItem.TagID;
                 tagname = this.AddPaymentEditExpenseTagItem.TagName;
                 break;
              default:
                 tagid = this.EditPaymentEditExpenseTagItem.TagID;
                 tagname = this.EditPaymentEditExpenseTagItem.TagName;
                 break;
          }

          var temp = this.ProjectExpenseTagList.filter((x) => x.TagName.includes(tagname));
          if (temp.length > 0) {
              this.Msg = "TagName you entered already exists. Please enter a new tag name.";
              this.infoSnackbarMsg = true;
              return;
          }

          var data = new FormData();
          data.append("TagID", tagid);
          data.append("TagName", tagname);
          var result = await objManageProjectLedgerBook.EditExpenseTag(data);
          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else {
              if (result.status == "success") {
                  this.fnInitializeEditExpenseTagItem(null, type);
                  this.fnGetAllExpenseTags();
              } else {
                  this.Msg = "Failed to edit expense tag.";
                  this.errorSnackbarMsg = true;
              }
          }
      },
  },
  watch: {
      /// <summary>
      ///  SearchActivity :- Function used to Search Activity based on ActivityName
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      SearchActivity: function () {
          this.filterActivityList = this.ActivityList;
          if (this.filterActivityList.length > 0 && this.SearchActivity != "") {
              this.filterActivityList = this.filterActivityList.filter((x) => x.ActivityName.toLowerCase().includes(this.SearchActivity.toLowerCase()));
          }
      },
  },
};
</script>

<style>
.row {
  padding-left: 0px;
  border: 0px none !important;
}
</style>
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

