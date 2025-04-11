<template>
    <router-view />
</template>

<script>
    export default {
        async beforeRouteLeave (to, from, next) {
            async function FunctionToCheckNetwork() {
                var status = true;
                try {
                    var url = window.SERVER_URL + "/MobileWebService/FunctionToCheckNetwork";

                    let response = await fetch(url, {
                        method: 'GET',
                        credentials: "include"
                    });

                    let result = await response.json();

                } catch (err) {
                    status = false;
                    //console.log(err);
                }

                //console.log("fnIsAuthenticated=", IsAuthenticated, window.LoggedUserID);
                return status;
            }
            let status = true
            try {
                status = await FunctionToCheckNetwork()
            } catch (error) {
                status = false
            }

            if(!window.navigator.onLine || status === false){
                alert('It seems that you are currently offline, therefore navigating to other pages is currently unavailable. Please try once the network is back, Thank you.')
                next(false)
            } else {
                next()
            }
        }
    }
</script>