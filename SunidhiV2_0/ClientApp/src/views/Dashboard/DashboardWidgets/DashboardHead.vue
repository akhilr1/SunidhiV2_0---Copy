<template>
  <v-container fluid class="px-0">
    <v-card >
      <v-row class="d-flex justify-space-between">
        <v-col cols="12" xs="12" sm="2" style="min-width: min-content" class="py-0">
          <v-list two-line class="pt-0 pb-0">
            <v-list-item class="pl-2">
              <v-list-item-icon>
                  <v-avatar size="50"
                            light
                            tile
                            :color="`${getcolor(currentWSName)}`"
                            class="elevation-1 base-border-radius">
                      <v-img v-bind:src="currentWSImage"
                             height="50"
                             width="50"
                             v-if="currentWSImage.length > 0"></v-img>
                      <span class="headline white--text" v-else>
                          {{
                            currentWSName == null ? "" : currentWSName.substring(0, 1).toUpperCase()
                          }}
                      </span>
                  </v-avatar>
                
              </v-list-item-icon>
              <v-list-item-content class="pt-0">
                <v-list-item-subtitle
                  ><span style="font-size: 16px; color: black">{{
                    currentWSName
                  }}</span></v-list-item-subtitle
                >
                <v-list-item-subtitle class="d-flex">
                  <v-icon
                    style="font-size: 13px"
                    v-if="currentWSIsPrivate == false"
                    >mdi-earth</v-icon
                  >
                  <v-icon style="font-size: 13px" v-else>mdi-lock</v-icon>
                  <div class="pt-0">
                    <span style="font-size: 13px" class="pl-1">{{
                      currentWSIsPrivate == false ? "Public" : "Private"
                    }}</span>
                  </div>
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-col>

        <v-col sm="12" md="9" class="py-0">
          <v-row>
            <v-col  xs="12"  class="py-0"  >
              <v-list two-line class="pt-0 pb-0" style="height: 90%">
                <v-list-item>
                  <v-list-item-icon>
                    <v-avatar>
                      <img :src="project" alt="John" />
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content class="pt-0">
                    <v-list-item-subtitle v-if="dashdata"
                      ><span
                        style="font-size: 16px; font-weight: bold; color: black; flex-wrap: nowrap"
                        >{{dashdata.ProjectCunt}}</span
                      ></v-list-item-subtitle
                    >
                    <v-list-item-subtitle
                      ><span style="font-size: 12px; color: black"
                        >Total Projects</span
                      ></v-list-item-subtitle
                    >
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-col>

            <v-col  xs="12"  class="py-0" >
              <!-- <v-avatar>
                <img :src="beneficiaries" alt="John" />
              </v-avatar> -->
              <v-list two-line class="pt-0 pb-0" style="height: 90%">
                <v-list-item>
                  <v-list-item-icon>
                    <v-avatar>
                      <img :src="beneficiaries" alt="John" />
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content class="pt-0">
                    <v-list-item-subtitle v-if="dashdata"
                      ><span
                        style="font-size: 16px; font-weight: bold; color: black"
                        >{{dashdata.TotalBeneficiary}}</span
                      ></v-list-item-subtitle
                    >
                    <v-list-item-subtitle
                      ><span style="font-size: 12px; color: black"
                        >Total Beneficiaries</span
                      ></v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-col>

            <v-col  xs="12"  class="py-0" >
              <!-- <v-avatar>
                <img :src="donor" alt="John" />
              </v-avatar> -->
              <v-list two-line class="pt-0 pb-0" style="height: 90%">
                <v-list-item>
                  <v-list-item-icon>
                    <v-avatar>
                      <img :src="donor" alt="John" />
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content class="pt-0">
                    <v-list-item-subtitle
                      ><span
                        style="font-size: 16px; font-weight: bold; color: black"
                        >12</span
                      ></v-list-item-subtitle
                    >
                    <v-list-item-subtitle
                      ><span style="font-size: 12px; color: black"
                        >Total Donors</span
                      ></v-list-item-subtitle
                    >
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-col>

            <v-col  xs="12"  class="py-0" >
              <!-- <v-avatar>
                <img :src="funds" alt="John" />
              </v-avatar> -->
              <v-list two-line class="pt-0 pb-0" style="height: 90%">
                <v-list-item>
                  <v-list-item-icon>
                    <v-avatar>
                      <img :src="funds" alt="John" />
                    </v-avatar>
                  </v-list-item-icon>
                  <v-list-item-content class="pt-0">
                    <v-list-item-subtitle
                      ><span style="font-size: 16px; font-weight: bold; color: black">₹210,100,000</span>
                      </v-list-item-subtitle>
                    <v-list-item-subtitle>
                      <span style="font-size: 12px; color: black">Total Funds</span>
                      </v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-col>

            <v-col style="min-width: min-content"  >
              <v-list two-line class="pt-0">
                  <v-list-item>
                      <v-list-item-content class="pt-0">
                          <v-list-item-subtitle>
                              <span style="font-size: 12px; color: black">Impact Indicator</span>
                          </v-list-item-subtitle>
                          <v-list-item-subtitle>
                              <img :src="sdg" style="height:30px" alt="sdg" />
                          </v-list-item-subtitle>
                      </v-list-item-content>
                  </v-list-item>
              </v-list>
            </v-col>

          </v-row>
        </v-col>

      </v-row>
    </v-card>
  </v-container>
</template>

<script>
    import objUtils from '../../../utils.js'
    var Dash
    async function importscript() {
        return Promise.all([
            import("@/BL/Dashboard.js").then(mod => {
                Dash = new mod.Dashboard();
            })
        ]);
    }

    export default {
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            await importscript();
            await this.fnGetWorkspaceDetails();
        },
        props: ['dashdata'],
        methods: {
            /// <summary>
            ///  getcolor :- Function used to get color of current workspace
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            ///  fnGetWorkspaceDetails :- Function used to fetch workspace details based on WorkspaceID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetWorkspaceDetails() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                var url = window.SERVER_URL + "/Workspace/GetWorkspaceDetails";
                var response = await fetch(url, {
                    method: "POST",
                    credentials: "include",
                    body: data,
                });
                var result = await response.json();
                if (result.data.length > 0) {
                    this.currentWSName = result.data[0].WSName;
                    this.currentWSIsPrivate = result.data[0].WSIsPrivate;

                    if (result.data[0].WorkSpaceImage != null) 
                    {
                        if (result.data[0].WorkSpaceImage.length > 0) {
                            this.currentWSImage = window.SERVER_URL + "/Upload" + result.data[0].WorkSpaceImage;
                        }
                    } 
                    else 
                    {
                        this.currentWSImage = "";
                    }
                }
            },
        },
        data() {
            return {
                WorkspaceID: null,
                currentWSIsPrivate: null,
                currentWSName: null,
                currentWSImage: "",
                ProjectCount: null,
                TotalBeneficiary: null,
                project: require("@/assets/dashboard_asset/Projects.svg"),
                beneficiaries: require("@/assets/dashboard_asset/Beneficiaries.svg"),
                donor: require("@/assets/dashboard_asset/Donors.svg"),
                funds: require("@/assets/dashboard_asset/Funds.svg"),
                sdg: require("@/assets/dashboard_asset/sdg-header-en.svg"),
            };
        },
    };
</script>
