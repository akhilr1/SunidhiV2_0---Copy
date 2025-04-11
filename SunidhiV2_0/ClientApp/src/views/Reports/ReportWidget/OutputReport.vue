<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" xs="12" sm="12" md="11">
        <v-card style="height: 1100px" elevation="1">
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
              <OutputOverview />
            </v-col>
          </v-row>
          <!--<v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <OverallOutputProgress />
            </v-col>
          </v-row>-->
          <!--<v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <IndividualOutputAnalysis />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <OutputCard />
            </v-col>
          </v-row>-->
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import OutputOverview from "../ReportWidget/OutputOverview";
import OverallOutputProgress from "../ReportWidget/OverallOutputProgress";
import IndividualOutputAnalysis from "../ReportWidget/IndividualOutputAnalysis";
import OutputCard from "../ReportWidget/OutputCard";
export default {
  components: {
    OutputOverview: OutputOverview,
    OverallOutputProgress: OverallOutputProgress,
    IndividualOutputAnalysis: IndividualOutputAnalysis,
    OutputCard:OutputCard
  },
};
</script>