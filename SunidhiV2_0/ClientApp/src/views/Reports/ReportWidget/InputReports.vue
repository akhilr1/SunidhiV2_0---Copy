<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" xs="12" sm="12" md="11">
        <v-card style="height: 1100px" elevation="1">
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
              <InputOverview />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <OverallInputProgress />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <IndividualInputAnalysis />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <InputCard />
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import InputOverview from "../ReportWidget/InputOverview";
import OverallInputProgress from "../ReportWidget/OverallInputProgress";
import IndividualInputAnalysis from "../ReportWidget/IndividualInputAnalysis";
import InputCard from "../ReportWidget/InputCard";
export default {
  components: {
    InputOverview: InputOverview,
    OverallInputProgress: OverallInputProgress,
    IndividualInputAnalysis: IndividualInputAnalysis,
    InputCard:InputCard
  },
};
</script>