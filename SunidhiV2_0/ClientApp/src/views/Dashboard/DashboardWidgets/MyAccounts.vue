<template>
  <v-card style="height: auto;max-height: 620px;min-height: 490px;" elevation="1">
    <v-row class="pt-1">
      <v-col sm="10" cols="6" class="pt-5">
        <h1 class="page-head-dark font-weight-medium pl-4">My Accounts</h1>
      </v-col>
      <v-col sm="2" cols="6">
        <v-select
          dense
          label="Teach Each"
          outlined
          hide-details
          class="mr-2"
        ></v-select>
      </v-col>
    </v-row>
    <v-row>
      <v-col class="px-4">
        <v-data-table
          :headers="headers"
          :items="datas"
          :items-per-page="10"
          class="elevation-0"
          fixed-header
          height="45vh"
        >
          <template v-slot:item.name="{ item }">
            <div class="py-3">
              <v-avatar size="30" color="#fff" class="elevation-0">
                <v-icon size="20" color="primary">mdi-folder</v-icon>
              </v-avatar>
              <span class="pl-3">{{ item.name }}</span>
            </div>
          </template>
          <template v-slot:item.bank="{ item }">
            <div>
              <span class="pl-7">
                {{ item.bank }}
              </span>
            </div>
          </template>
          <template v-slot:item.accountType="{ item }">
            <span> {{ item.accountType }}</span>
          </template>
          <template v-slot:item.accountNum="{ item }">
            {{ item.accountNum }}
          </template>
          <template v-slot:item.ifsc="{ item }">
            {{ item.ifsc }}
          </template>
          <!-- <template v-slot:item.actions="{ item }">
            <v-btn icon>
              <v-icon class="arrow-btn">mdi-menu-down</v-icon>
            </v-btn>
          </template> -->
        </v-data-table>
      </v-col>
    </v-row>
  </v-card>
</template>
<script>
export default {
  data() {
    return {
      headers: [
        {
          text: "Name",
          sortable: false,
          value: "name",
        },
        { text: "Bank", value: "bank", sortable: false },
        { text: "Account Type", value: "accountType", sortable: false },
        { text: "Account No.", value: "accountNum", sortable: false },
        { text: "IFSC Code", value: "ifsc", sortable: false },
        // { text: "Actions", value: "actions", sortable: false },
      ],
      datas: [
        {
          name: "Mahindra Ventures",
          bank: "Kotak Mahindra Bank",
          accountType: "Income Account",
          accountNum: "012345678",
          ifsc: "KKBK000261",
        },
        {
          name: "Regional Account",
          bank: "State Bank of India",
          accountType: "Income Account",
          accountNum: "012345678",
          ifsc: "KKBK000261",
        },
      ],
    };
  },
};
</script>
<style scoped>
</style>