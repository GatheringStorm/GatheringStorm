import {
    addNewLogMessage
} from "./Log.js"

const uRL = "http://localhost:5000/";

class webAccess {
    handleResponse(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response;
    }

    async sample(token, payload) {
        try {
            let response = await fetch(uRL, {
                headers: {
                    "Authorization": token.token_type + " " + token.id_token,
                    "Accept": "application/josn",
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: payload
            })

            this.handleResponse(response);
            console.log(response);
        } catch (err) {
            addNewLogMessage("@sampleRequest " + err)
        }

    }

}

defaultWebAccess = new webAccess();

export default defaultWebAccess;