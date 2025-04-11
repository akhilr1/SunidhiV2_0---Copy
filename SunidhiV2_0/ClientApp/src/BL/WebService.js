async function CallWebService(type, url, dataform) {
        var response, result;
        switch (type) {
            case "GET":
                response = await fetch(url, {
                    method: 'GET',
                    credentials: "include"
                });
                break;
            case "POST":
                response = await fetch(url, {
                    method: 'POST',
                    credentials: "include",
                    body: dataform
                });
                break;
            default:
            // code block
        }

        result = await response.json();

        return result;
}

export { CallWebService };

