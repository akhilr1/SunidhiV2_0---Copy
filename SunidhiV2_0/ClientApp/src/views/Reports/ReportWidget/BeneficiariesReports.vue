<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" xs="12" sm="12" md="11">
        <v-card style="height: 1200px" elevation="1">
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
              <BeneficiariesOverview />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <OverallBeneficiaryProgress />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <IndividualBeneficiaryAnalysis />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
              <BeneficiariesCard />
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import BeneficiariesOverview from "../ReportWidget/BeneficiariesOverview";
import OverallBeneficiaryProgress from "../ReportWidget/OverallBeneficiaryProgress";
import IndividualBeneficiaryAnalysis from "../ReportWidget/IndividualBeneficiaryAnalysis";
import BeneficiariesCard from "../ReportWidget/BeneficiariesCard";
export default {
  components: {
    BeneficiariesOverview: BeneficiariesOverview,
    OverallBeneficiaryProgress: OverallBeneficiaryProgress,
    IndividualBeneficiaryAnalysis: IndividualBeneficiaryAnalysis,
    BeneficiariesCard:BeneficiariesCard
  },
};
</script>