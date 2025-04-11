<template>
  <div class="full-width-row" v-if="columns && columns.length">
    <div
      style="position: relative"
      class="ag-row"
      :class="{'full-row-selected': selectedNode}"
    >
      <div
        class="ag-cell v-ag-checkbox"
        :style="{left: '2px', width: appendPx(columns[0].actualWidth)}"
      >
        <v-checkbox
          v-model="selectedNode"
          :hide-details="true"
          :ripple="false"
          class="ma-0 pa-0"
          color="primary2"
          dense
        ></v-checkbox>
      </div>
      <div
        class="ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height ag-cell-focus"
        aria-colindex="2"
        tabindex="-1"
        col-id="TaskdataAddedBy"
        role="gridcell"
        :style="{ left: appendPx(columns[1].left), width: appendPx(columns[1].actualWidth) }"
      >
        {{ rowData.TaskdataAddedBy }}
      </div>
      <!-- <div
        class="ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height"
        aria-colindex="3"
        tabindex="-1"
        col-id="0"
        role="gridcell"
        :style="{ left: appendPx(columns[2].left), width: appendPx(columns[2].actualWidth) }"
        v-show="false"
      ></div> -->
      <div
        class="ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height ag-cell-focus"
        aria-colindex="4"
        tabindex="-1"
        col-id="WorkflowStage"
        role="gridcell"
        :style="{ left: appendPx(columns[2].left), width: appendPx(columns[2].actualWidth) }"
      >
        {{ rowData.WorkflowStage }}
      </div>
      <div
        class="ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height ag-cell-focus"
        aria-colindex="5"
        tabindex="-1"
        col-id="1"
        role="gridcell"
        :style="{ left: appendPx(columns[3].left), width: appendPx(columns[3].actualWidth) }"
      >
        {{ new Date(rowData.TaskSubmittedDate).toLocaleDateString() }}
      </div>
      <div
        class="ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height ag-cell-focus"
        aria-colindex="6"
        tabindex="-1"
        col-id="ActionRequired"
        role="gridcell"
        :style="{ left: appendPx(columns[4].left), width: appendPx(columns[4].actualWidth) }"
      >
        {{ rowData.ActionRequired }}
      </div>
      <div
        class="pl-0   ag-cell-value ag-cell ag-cell-not-inline-editing ag-cell-normal-height ag-cell-focus"
        aria-colindex="6"
        tabindex="-1"
        col-id="Comment"
        role="gridcell"
        :style="{ left: appendPx(columns[5].left), width: appendPx(columns[5].actualWidth) }"
      >
        <v-text-field class="ma-0 pa-0 commentBox" solo flat v-model="rowData.TaskComment" placeholder="Comment" :hide-details="true"></v-text-field>
      </div>
    </div>
    <div class="rowContent mx-12">
      <ResponsesGridView
        gridTheme="alpine"
        :rawData="rawData"
        :contextData="responseContext"
        :responseMode="false"
        @gridLoaded="setRowHeight($event)"
        @gridChanged="refreshRowHeight($event)"
        ref="gridRef"
      />
    </div>
  </div>
</template>

<script>
import ResponsesGridView from "./ResponsesGridView.vue";
import { v4 as uuidv4 } from "uuid";
import { createNamespacedHelpers } from "vuex";
const { mapActions: gActions, mapGetters: gGetters } = createNamespacedHelpers("GridContext")
export default {
  name: "FullWidthRowRenderer",
  components: {
    ResponsesGridView,
  },
  data() {
    return {
      gridApi: null,
      gridNodeApi: null,
      rowData: null,
      rawData: null,
      uuid: uuidv4(),
      columns: [],
      rowHeight: 0,
      SyncDBID: null,
    };
  },
  computed: {
    ...gGetters([
      "mappedResponses",
      "responseContext"
    ]),
    selectedNode: {
      get() {
        return this.gridNodeApi?.isSelected();
      },
      set(val) {
        this.gridNodeApi.setSelected(val);
      },
    },
  },
  created() {
    this.gridApi = this.params.api;
    (this.gridNodeApi = this.params.node), (this.rowData = this.params.data);
  },
  mounted() {
    this.SyncDBID = this.params.data.SyncDBID
    this.columns = this.params.columnApi.getAllColumns();
    this.rawData = this.mappedResponses.get(this.SyncDBID);
  },
  methods: {
    ...gActions([
    ]),
    appendPx(val) {
      return `${val}px`;
    },
    setRowHeight(e) {
      this.rowHeight = e + 12;
      this.params.node.setRowHeight(this.rowHeight);
      this.gridApi.onRowHeightChanged();
    },
    refreshRowHeight(e) {
      this.params.node.setRowHeight(e + 12);
      this.gridApi.onRowHeightChanged();
    }
  },
  destroyed() {
    // console.log(this.uuid, 'destroyed')
  },
  watch: {}
};
</script>

<style lang="scss" scoped>
.rowContent {
  background-color: white !important;
}
.full-row-selected {
  background-color: rgba(33, 150, 243, 0.3);
}

.v-ag-checkbox{
  display: grid;
  place-content: center;
}

::v-deep .v-input--selection-controls__input {
  margin-right: 0px !important; 
}

::v-deep .v-text-field.v-text-field--solo .v-input__control{
  min-height: 40px !important;
}
</style>
