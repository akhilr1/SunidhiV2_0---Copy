import { Loader } from "@googlemaps/js-api-loader";
import { dbImworks } from "../../ImworksDb";
import { v1 as _id } from "uuid";

const loader = new Loader({
    // apiKey: "AIzaSyA1nBl8IF-i6Lxc9L65EGqMe9EauXJJF4Y",
    apiKey: "AIzaSyBDC7AC24l6C-hBzxM_z5cqUDFAAs2X4Oc",
});

const state = {
    loadStatus: false,
    CurrentUser: 0,
    GmapOptions: {
        mapTypeControl: false,
        fullscreenControl: false,
        streetViewControl: false,
        zoomControl: false,
        zoom : 4.8,
        center :  {lat:22.9719009, lng:77.5943941},
    },
    ProjectsList: [],
    SurveyList : [],
    PresetList : [],
    StatesList: [],
    DistrictList : [],
    Step: 0,
    SelectedFilters: {
        Project: '',
        Survey: '',
        Preset: '',
        Country: 'India',
        State: '',
        District: ''
    },
    Map: null,
    MarkerData: [],
    Markers: [],
    IndiaBounds: null,
    CurrentStateName: null,
    Geo_coordinate_list: null,
    ShareConfig: {
        Project: '',
        Survey: '',
        ConfigName: ''
    },
    SnackStatus: 'Saving Share Config...',
    VersionList: [],
    GuestMode: false,
    VersionUniqueId: 0,
    VersionData: null,
    PivotPresetList: [],
    PresetResponseData: [],
    VersionUserID: null,
    CardState: null,
    WsID: null,
    SurveyResponseDetails: [],
    ShareVersionTree: [],
    mainTitle: 'India',
    CallCardEvent: null,
    CurrentVersionName: "Version Name"
}

const getters = {
    GmapCenter : state => state.GmapCenter,
    GmapOptions : state => state.GmapOptions,
    StatesList : state => state.StatesList,
    StatesCount : state => state.StatesList.length,
    DistrictList : state => state.DistrictList,
    DistrictCount : state => state.DistrictCount,
    ProjectsList: state => state.ProjectsList,
    SurveyList : state => state.SurveyList,
    Step: state => state.Step,
    SelectedFilters: state => state.SelectedFilters,
    Map : state => state.Map, 
    IndiaBounds : state => state.IndiaBounds,
    MarkerData: state => state.MarkerData,
    CurrentStateName: state => state.CurrentStateName,
    GeoDataList : state => state.Geo_coordinate_list,
    ShareConfig: state => state.ShareConfig,
    SnackStatus: state => state.SnackStatus,
    GuestMode: state => state.GuestMode,
    VersionList: state => state.VersionList,
    CurrentVersionId: state => state.VersionUniqueId,
    VersionData: state => state.VersionData,
    PivotPresetList: state => state.PivotPresetList,
    PresetResponseData: state => state.PresetResponseData,
    VersionUserID: state => state.VersionUserID,
    CurrentUser: state => state.CurrentUser,
    CardState: state => state.CardState,
    WsID : state => state.WsID,
    loadStatus: state => state.loadStatus,
    SurveyResponseDetails: state => state.SurveyResponseDetails,
    ShareVersionTree: state => state.ShareVersionTree,
    mainTitle: state => state.mainTitle,
    CallCardEvent: state => state.CallCardEvent,
    CurrentVersionName: state => state.CurrentVersionName,
    Markers: state => state.Markers,
}

const actions = {
    fnSetTitle({commit}, title) {
        commit('SET_MAINTITLE', title)
    },
    fnSetLoading({commit}, status) {
        commit('SET_STATUS', status);
    },
    async fnLoadMap(context){
        loader
        .load()
        .then((google) => {
            context.dispatch('fnSetMap', {
                map: new google.maps.Map(document.getElementById("map"), context.state.GmapOptions)
            });
            if (!google.maps.Polygon.prototype.getBounds) {
                google.maps.Polygon.prototype.getBounds = function () {
                    var bounds = new google.maps.LatLngBounds();
                    this.getPath().forEach(function (element, index) { bounds.extend(element); });
                    return bounds;
                }
            }
        })
        .catch(e => {
            console.log(e)
        });

        await fetch(`/MapFiles/indiaStatesBoundaries.json`)
        .then((res) => res.json())
        .then((data) => {
            context.commit('SET_GEOJSON', data)
        })
        .catch(e => console.log(e))

    },
    fnSetMap({commit}, map){
        commit('UPDATE_MAP', null)
        if(map){
            commit('UPDATE_MAP', map.map)
        }
    },
    fnSetCurrentStateName({commit}, val){
        if(val){
            commit('SET_STATENAME', val)
        }
    },
    fnSetSelectedFilters({commit}, payload){
        commit('SET_FILTERS', [])

        if(payload){
            commit('SET_FILTERS', payload)
        }
    },
    fnSetStep({commit, state}, val){
        commit('UPDATE_STEP', val)
    },
    async fnGetProjectList({state, commit}) {
        // get data asynchronously
        var url = `${window.SERVER_URL}/MapView/GetprojectswithGPSinsurvey`

        let formdata = new FormData();
        formdata.append("WorkspaceID",state.WsID);
        formdata.append("VersionUniqueID", state.VersionUniqueId)

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        }) 

        let response = await data.json();
        // mutate state with that data
        if(response.status.toLowerCase() == 'success'){
            commit('UPDATE_PROJECT_LIST', response.data.Project_list)
            commit('UPDATE_VERSIONDATA', response.data.VersionData)
        }
    },
    fnGetSurveyList({commit, state}, projectID){
        if(projectID){
            let data = state.ProjectsList.filter(p => p.ProjectID == projectID)
            commit('UPDATE_SURVEY_LIST', data[0].Survey)
        }
    },
    fnGetPresetList({commit, state}, projectID){
        if(projectID){
            let data = state.ProjectsList.filter(p => p.ProjectID == projectID)
            commit('UPDATE_PRESET_LIST', data[0].Preset_List)
        }
    },
    async fnGetStateWiseSurveyResponse({commit, state}, payload){
        // /Map/GetStateWiseSurveyResponse

        // Parameters : SurveyID, ProjectID

        // Method Type : GET
        if(state.StatesList.length){
            commit('UPDATE_STATES_LIST', [])
        }

        let url = `${window.SERVER_URL}/MapView/GetStateWiseSurveyResponse`

        let formdata = new FormData();

        // instead of passing project and survey id as params, we are passing them from selectedfilters from state
        let SurveyID, ProjectID
        if(payload){
            SurveyID = payload.Survey.SurveyID
            ProjectID = payload.Project.ProjectID
        } else {
            SurveyID = state.SelectedFilters.Survey.SurveyID
            ProjectID = state.SelectedFilters.Project.ProjectID
        }
        // formdata.append("SurveyID", SurveyID);
        formdata.append("SurveyID", SurveyID);
        formdata.append("ProjectID", ProjectID);
        state.VersionUniqueId && formdata.append("versionUniqueId", state.VersionUniqueId)

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()

        if(res.data?.length > 0){
            // join res.data.key and res.data into one array
            function getBenefNameList(key){
                return key.BeneficiaryName?.split(',').filter(d => d != '')
            }
            function getPresetStatus(stateName){
                let VersionData = state.VersionData
                if(VersionData && VersionData.length > 0){
                    let presets = state.VersionData.filter(d => {
                        return d.StateName.trim().toLowerCase() == stateName.toLowerCase().trim() && !d.DistrictName && !d.response_coordinates
                    })
                    if(presets && presets.length > 0){
                        presets.map(d => {
                            if(!d.Preset) return false
                        })
                    } else {
                        return true
                    }
                } else {
                    return true
                }
            }
            let data = res.data.map(d => {
                return {
                    ...d.Key,
                    StateName: `${d.Key.StateName.charAt(0).toUpperCase() + d.Key.StateName.slice(1).toLowerCase()}`,
                    BeneficiaryNames : getBenefNameList(d),
                    isPresetEnabled: getPresetStatus(d.Key.StateName)
                }
            })
            if(res.status.toLowerCase() == 'true'){
                commit('UPDATE_STATES_LIST', data)
            }
        }

        
    },
    async fnGetDistrictWiseSurveyResponse({commit, state}, payload){
        // Name :  /Map/GetDistrictWiseSurveyResponse

        // Parameters : SurveyID, StateName

        // Method Type : GET

        commit('UPDATE_DISTRICT_LIST', [])


        let url = `${window.SERVER_URL}/MapView/GetDistrictWiseSurveyResponse`

        let formdata = new FormData();

        // instead of passing project and survey id as params, we are passing them from selectedfilters from state
        let SurveyID = state.SelectedFilters.Survey.SurveyID
        
        if(payload.SurveyID) {
            formdata.append("SurveyID", payload.SurveyID)
        }else {
            formdata.append("SurveyID", SurveyID);
        };
        formdata.append("StateName", payload.StateName);
        state.VersionUniqueId && formdata.append("versionUniqueId", state.VersionUniqueId)
        
        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()

        if(res.data && res.data.length > 0){
            // join res.data.key and res.data into one array
            function getBenefNameList(key){
                return key.BeneficiaryName?.split(',').filter(d => d != '')
            }
            function getPresetStatus(districtName){
                let VersionData = state.VersionData
                if(VersionData && VersionData.length > 0){
                    let presets = state.VersionData.filter(d => {
                        return d.DistrictName.trim().toLowerCase() == districtName.trim().toLowerCase() && !d.response_coordinates
                    })
                    if(presets && presets.length > 0){
                        presets.map(d => {
                            if(!d.Preset) return false
                        })
                    } else {
                        return true
                    }
                } else {
                    return true
                }
            }

            let xdata = res.data.map(d => {
                return {
                    ...d.Key,
                    DistrictName: `${d.Key.DistrictName.charAt(0).toUpperCase() + d.Key.DistrictName.slice(1).toLowerCase()}`,
                    BeneficiaryNames : getBenefNameList(d),
                    isPresetEnabled: getPresetStatus(d.Key.DistrictName) 
                }
            })
            // if(res.status.toLowerCase() == 'true'){
            //     commit('UPDATE_STATES_LIST', data)
            // }
            if(res.status.toLowerCase() == "true"){
                commit('UPDATE_DISTRICT_LIST', xdata)
            }
        }

        //if(res.status.toLowerCase() == "success" && stateName.toLowerCase() === res.stateName.toLowerCase()){
    },
    async fnGetResponseGeoCoordinates({commit, state}, payload){
        // Name :  /Map/GetResponseGeoCoordinates

        // Parameters : SurveyID, DistrictId

        // Method Type : GET
        commit('UPDATE_GEODATALIST', [])

        let url = `${window.SERVER_URL}/MapView/GetResponseGeoCoordinates`
        let VersionData = state.VersionData

        let formdata = new FormData();

        let SurveyID = state.SelectedFilters.Survey.SurveyID
        formdata.append("DistrictName", payload.DistrictName);
        formdata.append("SurveyID", payload.SurveyID ? payload.SurveyID : SurveyID);

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()
        // if(res.status.toLowerCase() == "true" && res.districtName.toLowerCase() == DistrictName.toLowerCase() ){
        if(VersionData && VersionData.length > 0){
            let omittedCoords = VersionData.forEach(f => {
                if(f.response_coordinates && payload.DistrictName.toLowerCase() == f.DistrictName.trim().toLowerCase()) {
                    res.data = res.data.filter(x => {
                        return f.response_coordinates != x.Geo_coordinates
                    }) 
                }
            })
        }
        if(res.status.toLowerCase() == "true"){
            commit('UPDATE_GEODATALIST', res.data)
        }

    },
    async fnGetPivotPresetList({state, commit}){
        commit('UPDATE_PIVOT_PRESET_LIST', [])
        let url = `${window.SERVER_URL}/MapView/GetPivotPresetList`

        let SurveyID = state.SelectedFilters.Survey.SurveyID
        let UserID = state.VersionUserID ?? state.CurrentUser
        let formdata = new FormData();
        formdata.append('SurveyID', SurveyID);
        formdata.append('UserID', UserID);

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()

        if(res.status.toLowerCase() == 'success' && res.data && res.data.length > 0){
            commit('UPDATE_PIVOT_PRESET_LIST', res.data)
        }

    },
    async fnGetPresetResponseDetails({state, commit}, payload){
        commit('UPDATE_PRESET_RESPONSE_DATA', [])

        let url = `${window.SERVER_URL}/MapView/GetPresetResponseDetails`
        let {
            stateName,
            districtName, 
            syncDBID
        } = payload

        let UserID = state.VersionUserID ?? state.CurrentUser
        let SurveyID = state.SelectedFilters.Survey.SurveyID

        let formdata = new FormData();
        formdata.append('SurveyID', SurveyID);
        formdata.append('UserID', UserID);
        formdata.append('StateName', stateName);
        syncDBID && formdata.append('SyncDBID', syncDBID);
        districtName && formdata.append('DistrictName', districtName);

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()

        if(res.data && res.data.length > 0){
            commit('UPDATE_PRESET_RESPONSE_DATA', res.data)
        }
    },
    async fnGetVersionDetails({state,commit}){
        // Name :  /Map/GetVersionDetails

        // Parameters : UserId, WorkspaceID --as an object

        // Method Type : POST
        
        commit('UPDATE_VERSIONLIST', [])

        let url = `${window.SERVER_URL}/MapView/GetVersionDetails`

        let formdata = new FormData();
        formdata.append('UserId', state.CurrentUser);
        formdata.append('WorkspaceID', state.WsID);

        let data = await fetch(url,{
            method: "POST",
            body: formdata
        })

        let res = await data.json()

        if(res.status.toLowerCase() == "success"){
            commit('UPDATE_VERSIONLIST', res.data)
        }
    },
    async fnSaveShareConfig({commit}, payload){

        // Name:  /Map/SaveSharelinkVersion

        // Method Type: POST

        // Parameters: {
        //    ConfigName : String,
        //    VersionUniqueId : uuidv4,
        //    WorkspaceID : number,
        //    ConfigData : Array in string format, 
        // }
        // console.log(payload)
        // console.log('config', JSON.parse(payload.ConfigData))
        let url = `${window.SERVER_URL}/MapView/SaveSharelinkVersion`

        let formdata = new FormData()
        formdata.append('VersionName', payload.ConfigName)
        formdata.append('VersionUniqueId', payload.VersionUniqueID)
        formdata.append('WorkspaceID', payload.WorkspaceID)
        formdata.append('linkVersionDetails', payload.ConfigData)
        formdata.append('loggedUserId', payload.loggedUserId)
        formdata.append('ProjectID', state.SelectedFilters.Project.ProjectID)
        formdata.append('SurveyID', state.SelectedFilters.Survey.SurveyID)

        let data = await fetch(url, {
            method: 'POST',
            body: formdata,
        })
        let response = await data.json()
        
        if(response.status.toLowerCase() == "success") {
            commit('UPDATE_SNACK_STATUS', response.Message)
        }
    },
    fnClearShareConfig({commit}){
        commit('CLEAR_SHARECONFIG')
    },
    // Map Feature Functions
    fnFilterStateGeoJson({state, dispatch}){

        state.Map.data.setStyle({
            fillColor: "rgba(0,0,0,0.1)",
            strokeWeight: 1,
            strokeColor: "rgb(133,0,0)",
        });

        let data = {
            features: [],
            type: "FeatureCollection"
        }
        state.StatesList.forEach(s => {
            state.IndiaBounds.features.forEach(f => {
                if(f.properties.TYPE_1.toLowerCase() == 'state' || f.properties.TYPE_1.toLowerCase().includes('union')){
                    if(f.properties.NAME_1.toLowerCase() == s.StateName.trim().toLowerCase()){ 
                        data.features.push(f)
                    }
                }
            })
        })
        dispatch('fnGetCenterPoints', data)
    },
    fnFilterDistrictGeoJson({state, dispatch}, source){
        let data = {
            features: [],
            type: "FeatureCollection"
        }

        state.DistrictList.forEach(d => {
            source.features.forEach(f => {
                if(d.DistrictName){
                    if((f.properties.NAME_2.toLowerCase()) == d.DistrictName.trim().toLowerCase()){
                        data.features.push(f)
                    }
                }
            })
        })

        dispatch('fnGetCenterPoints', data)
    },
    fnGetCenterPoints({commit, state}, val){
        if(state.Map){
            let tempmarkers = []
            commit('UPDATE_MARKERDATA', [])
    
            let tempData = new google.maps.Data()
            tempData.addGeoJson(val)
    
            tempData.forEach((feature)=> {
                let polygon, latlng, centerPoints;
                if (feature.getGeometry().getType() == "MultiPolygon") {
                    let arr = feature.getGeometry().getArray();
                    let len = arr.length - 1;
                    if (len) {
                        polygon = new google.maps.Polygon({
                            paths: arr[len].getAt(0).getArray(),
                        });
                        latlng = polygon.getBounds().getCenter().toJSON();
                    }
                }
                if (feature.getGeometry().getType() == "Polygon") {
                    polygon = new google.maps.Polygon({
                    paths: feature.getGeometry().getAt(0).getArray(),
                    });
                    latlng = polygon.getBounds().getCenter().toJSON();
                }
                let labelData = () => {
                    let st, dt;
                    if(feature.getProperty('TYPE_1')){
                        st = state.StatesList.find(s => {
                            let name = s.StateName.trim().toLowerCase()
                            return name == feature.getProperty('NAME_1').toLowerCase()
                        })
                        return st.ResponseCount
                    } else if(feature.getProperty('NAME_2')) {
                        dt = state.DistrictList.find(d => {
                            let name = d.DistrictName.trim().toLowerCase()
                            return name == feature.getProperty('NAME_2').toLowerCase()
                        })
                        return dt.ResponseCount
                    }
                }
                let nameData = () => {
                    if(feature.getProperty('TYPE_1') && (feature.getProperty('TYPE_1').toLowerCase() == 'state' || feature.getProperty('TYPE_1').toLowerCase().includes('union'))){
                        return feature.getProperty('NAME_1')
                    } else if(feature.getProperty('NAME_2')) {
                        return feature.getProperty('NAME_2')
                    }
                }
                centerPoints = {
                    Name: nameData(),
                    num: labelData(),
                    coords: latlng,
                };
               tempmarkers.push(centerPoints);
                
            })
            commit('UPDATE_MARKERDATA', tempmarkers);
    
        }
    },
    fnAddGeoMarkers({ commit, state, dispatch }, source) {
        commit('CALL_CARD', '')
        commit('UPDATE_MARKERS', [])
        let markers = []
        const svgMarker = {
            path: "M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z",
            scale: 1.4,
            strokeColor: "white",
            strokeWeight: 2,
            fillColor: "grey",
            fillOpacity: 1,
            labelOrigin: new google.maps.Point(11.5, 11.5),
        };
        
        const infowindow = new google.maps.InfoWindow({
            content: "",
        });
        
        source.forEach((x) => {
            let marker = new google.maps.Marker({
                position: x.coords,
                map: state.Map,
                label: {
                    text: x.num.toString(),
                    color: "white",
                },
                icon: svgMarker,
                anchor: new google.maps.Point(0, 0),
            });
            marker.addListener("click", () => {
                // state.Map.panTo(marker.getPosition());
                commit('CALL_CARD', x.Name)
                state.Step === 3 && commit('CALL_CARD', true)
            });
        
            marker.addListener("mouseover", () => {
                infowindow.setContent(x.Name);
                setTimeout(() => {
                    infowindow.open(state.Map, marker);
                }, 300);
            });
        
            marker.addListener("mouseout", () => {
                infowindow.close();
            });
            markers.push(marker)
        })
        commit('UPDATE_MARKERS', markers)
    },
    fnAddStateBounds({commit, dispatch, state}, StateName){

        // needs Revision
        // current Revision 0.2
        
        let st = state.StatesList.find((x) => {
            return x.StateName.trim() == StateName
        })
        let stateName = st.StateName.trim()
        dispatch('fnRemoveMapFeatures')
        state.IndiaBounds.features.forEach((f)=>{
            if(f.properties.NAME_1.toLowerCase() == stateName.toLowerCase()){
                state.Map.data.addGeoJson(f)
                state.MarkerData.filter((y)=>{
                    if((y.Name) && y.Name.toLowerCase() == stateName.toLowerCase()){
                        state.Map.setCenter(y.coords)
                    }
                })
            }
        })
    
    },
    async fnAddDistrictBounds({state, dispatch}, payload){
        dispatch('fnRemoveMapFeatures')

        payload.StateJson.features.forEach((x)=> {
            if( x.properties.NAME_2.toLowerCase() == payload.DistrictName.trim().toLowerCase()){
                state.Map.data.addGeoJson(x)
                state.MarkerData.filter(y => {
                    if(y.Name && y.Name.toLowerCase() == payload.DistrictName.trim().toLowerCase()){
                        state.Map.setCenter(y.coords)
                    }
                })
            }
        })
    },
    fnRemoveMapFeatures({state}){

        // implement parameterized reusability 

        state.Map.data.forEach((ft)=> {
            state.Map.data.remove(ft)
        })
        if(state.Markers.length) {
            state.Markers.forEach((m)=> {
                m.setMap(null);
            })
        }
        state.Map.setZoom(4.8)
        state.Map.setCenter(state.GmapOptions.center)
    },
    async fnLoadStateJson({state}){
        return await(await fetch(`/MapFiles/States/${state.CurrentStateName.replace(/\s+/g,'').toLowerCase()}.json`)).json()
    },
    //  End Map Features
    async getCurrentUser({commit}){
        let userID
        let user = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
        if(user && user.length > 0){
            userID = user[0].UserId
            commit('SET_CURRENTUSER', userID)
        }
    },
    fnSetCardState({commit}, payload){
        commit('SET_CARDSTATE', [])
        if(payload){
            commit('SET_CARDSTATE', payload)
        } else {
            commit('SET_CARDSTATE', [])
        }
    },
    async fnGetVersionData({commit}, VID){
        let url = `${window.SERVER_URL}/MapView/GetVersionDetailsByVersionUniqueID`

        let formData = new FormData();

        formData.append('VersionUniqueID', VID)

        let data = await fetch(url, {
            method: 'POST',
            body: formData,
        })

        let res = await data.json()

        if(res.status.toLowerCase() == 'success'){
            commit('SET_VERSIONUSERID', res.data.CreatedBy)
            commit('SET_VERSIONNAME', res.data.VersionName)
            return res.data
        }
        else return []
    },
    async fnGetShareVersionTree({commit, state}){
        commit('SET_SHAREVERSIONTREE', [])
        let url = `${window.SERVER_URL}/MapView/GetShareVersionDetails`

        let formData = new FormData();
        
        formData.append('SurveyID', state.SelectedFilters.Survey.SurveyID)
        formData.append('ProjectID', state.SelectedFilters.Project.ProjectID)

        let data = await fetch(url, {
            method: 'POST',
            body: formData,
        })

        let res = await data.json()

        let { lstState, lstDistricts, lstDistrictCordinates } = res.data

        

        let formattedRes = function getStates() {
            let ob = []
            lstState.forEach((y)=>{
                ob.push({
                    id: _id(),
                    name: y.StateName,
                    type: "state",
                    active: true,
                    children: getDistricts(y.StateName)
                })
            })
            return ob
        }
        function getDistricts(x) {
            let ob = [{
                id: _id(),
                name: "Preset",
                type: "preset",
                active: true
            }]
            lstDistricts.forEach((y)=>{
                if(y.StateName.toLowerCase() == x.toLowerCase()){
                    ob.push({
                        id: _id(),
                        name: y.DistrictName,
                        type: "district",
                        active: true,
                        children: [{
                            id: _id(),
                            name: "Preset",
                            active: true,
                            type: "preset"
                        },
                        {
                            id: _id(),
                            name: "Responses",
                            type: "responses",
                            active: true,
                            children: getCoordinatesByDistrict(y.DistrictName)
                        }]
                    })
                }
            })
            return ob
        }
        function getCoordinatesByDistrict(x) {
            let ob = []
            lstDistrictCordinates.forEach((y)=>{
                if(y.DistrictName.toLowerCase() == x.toLowerCase()){
                    ob.push({
                        id: _id(),
                        active: true,
                        name: y.Geo_coordinates,
                        type: "rootnode",
                    })
                }
            })
            return ob
        }

        let treedata = formattedRes()

        if(res.status.toLowerCase() == 'success'){
            commit('SET_SHAREVERSIONTREE', treedata)
            return treedata
        }
    },
    async fnGetSurveyResponseDetails({commit},payload){
        commit('SET_SURVEYRESPONSEDETAILS', [])

        let url = `${window.SERVER_URL}/MapView/GetSurveyResponseDetail?SyncTaskID=${payload.SyncDBID}&SurveyId=${payload.SurveyID}&SubmissionCount=${payload.SubmissionCount}`

        let data = await fetch(url, {
            method: 'GET',
        })

        let res = await data.json()

        if(res){
            commit('SET_SURVEYRESPONSEDETAILS', res)
        }
    },

}

const mutations = {
    SET_MAINTITLE(state, val){
        state.mainTitle = val
    },
    SET_STATUS(state, val){
        state.loadStatus = val
    },
    SET_CURRENTUSER(state, userID){
        state.CurrentUser = userID
    },
    SET_GUESTMODE(state, payload) {
        state.GuestMode = payload
    },
    C_populateEntityList(state, payload){
        state.StatesList = payload
        state.StatesCount = payload.length
    },
    SET_STATENAME(state, val){
        state.CurrentStateName = val
    },
    SET_GEOJSON(state, payload){
        state.IndiaBounds = payload
    },
    UPDATE_PROJECT_LIST(state, projectList){
        state.ProjectsList = projectList
    },
    UPDATE_SURVEY_LIST(state, surveyList){
        state.SurveyList = surveyList
    },
    UPDATE_PRESET_LIST(state, presetList){
        state.PresetList = presetList
    },
    UPDATE_STATES_LIST(state, statesList){
        state.StatesList = statesList
    },
    UPDATE_DISTRICT_LIST(state,districtList){
        state.DistrictList = districtList
    },
    UPDATE_STEP(state, step){
        state.Step = step
    },
    SET_FILTERS(state, payload){

        state.SelectedFilters = { 
            ...state.SelectedFilters,
            Project: !payload.project ? '': {
                ProjectID: state.ProjectsList.find((x)=> x.ProjectID === payload.project).ProjectID,
                ProjectName: state.ProjectsList.find((x)=> x.ProjectID === payload.project).ProjectName,
            },
            Survey: !payload.survey ? '': {
                SurveyID: state.SurveyList.find((x)=> x.SurveyID == payload.survey).SurveyID,
                SurveyName: state.SurveyList.find((x)=> x.SurveyID == payload.survey).SurveyName,
            },
            Preset: !payload.Preset ? '' : payload.Preset
        }
    },
    UPDATE_MAP(state, val){
        state.Map = val
    },
    UPDATE_MARKERDATA(state, data){
        state.MarkerData = data
    },
    UPDATE_MARKERS(state, markers){
        state.Markers = markers
    },
    UPDATE_GEODATALIST(state, val){
        state.Geo_coordinate_list = val
    },
    // Share Map Mutations
    SET_SHARECONFIG(state, payload){
        state.ShareConfig = {
            ...state.ShareConfig,
            ConfigName: payload.ConfigName,
            Project : !payload.Project ? '': state.ProjectsList.filter((x)=> x.ProjectID === payload.Project)[0].ProjectName,
            Survey : !payload.Survey ? '': state.SurveyList.filter((x)=> x.SurveyID == payload.Survey)[0].SurveyName
        }
    },
    CLEAR_SHARECONFIG(state){
        state.ShareConfig = {
            ...state.ShareConfig,
            ConfigName : '',
            Project : '',
            Survey : ''
        }
    },
    // Share Config Mutations
    UPDATE_SNACK_STATUS(state, message){
        state.SnackStatus = message
    },
    UPDATE_VERSIONLIST(state, payload){
        state.VersionList = payload
    },
    SET_VERSIONUNIQUEID(state, vID){
        state.VersionUniqueId = vID
    },
    UPDATE_VERSIONDATA(state, payload){
        state.VersionData = payload
    },
    UPDATE_PIVOT_PRESET_LIST(state, payload){
        state.PivotPresetList = payload
    },
    UPDATE_PRESET_RESPONSE_DATA(state, payload){
        state.PresetResponseData = payload
    },
    SET_CARDSTATE(state, payload){
        state.CardState = payload
    },
    SET_VERSIONUSERID(state, payload){
        state.VersionUserID = payload
    },
    SET_WSID(state, wsid){
        state.WsID = wsid
    },
    SET_SURVEYRESPONSEDETAILS(state, payload){
        state.SurveyResponseDetails = payload
    },
    SET_SHAREVERSIONTREE(state, payload){
        state.ShareVersionTree = payload
    },
    CALL_CARD(state, val){
        state.CallCardEvent = val
    },
    SET_VERSIONNAME(state, name){
        state.CurrentVersionName = name
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}
