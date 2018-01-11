import {
    addNewLogMessage
} from "./Log.js"

const uRL = "http://localhost:5000/api/games/";

class webAccess {
    handleResponse(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response;
    }

    async sample(urlAdd, token, payload) {
        try {
            let response = await fetch(uRL + urlAdd, {
                headers: {
                    "Authorization": token.token_type + " " + token.id_token,
                    "Accept": "application/json",
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

    async sampleGET(urlAdd) {
        let token = JSON.parse(localStorage.getItem("userToken"));
        try {
            let response = await fetch(uRL + urlAdd, {
                headers: {
                    "Authorization": token.token_type + " " + token.id_token,
                    "Accept": "application/json",
                },
                method: "GET"
            })

            this.handleResponse(response);
            return response;
        } catch (err) {
            addNewLogMessage("@sampleRequest " + err)
        }
    }

    async getBoard(email) {}

    async startNewGame(email, prio, token) {
        let classes = [];
        classes[prio.quick - 1] = "quick";
        classes[prio.medium - 1] = "medium";
        classes[prio.slow - 1] = "slow";

        let payload = {
            "opponentMail": email,
            "classTypes": classes
        };

        this.sample("New", token, payload);
    }

    async getOpenGames() {
        return await this.sampleGET("");
    }

    async useCard(GUID) {
        return true;
    }
}

const defaultWebAccess = new webAccess();

export default defaultWebAccess;