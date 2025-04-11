let SERVER_URL = "";
let LoggedUserID = 0; 
let EnteredWorkspaceID = 0;

if (SERVER_URL == "") {
    SERVER_URL = window.location.origin;
}
if (window.location.hostname == "localhost" && window.location.port.startsWith("808")) {
    SERVER_URL = "http://localhost:45417";
}
else {
    SERVER_URL = window.location.origin;
}

window.SERVER_URL = SERVER_URL;


async function fnIsAuthenticated() {
    var IsAuthenticated = false;
    var url = window.SERVER_URL + "/UserAccount/IsAuthenticated";

    let response = await fetch(url, {
        method: 'POST',
        credentials: "include"
    });

    let result = await response.json();
    if (result) {
        LoggedUserID = result;
        window.LoggedUserID = LoggedUserID;
        sessionStorage.setItem('LoggedUserID', LoggedUserID);
        IsAuthenticated = true;
    } else {
        IsAuthenticated = false;
        window.LoggedUserID = 0;
        sessionStorage.setItem('LoggedUserID', 0);
        sessionStorage.setItem('WorkspaceID', 0);//window.WorkspaceID = 0;
    }

    //console.log("fnIsAuthenticated=", IsAuthenticated, window.LoggedUserID);
    return IsAuthenticated;
}

export let IsAuthenticated = fnIsAuthenticated;
export default SERVER_URL;