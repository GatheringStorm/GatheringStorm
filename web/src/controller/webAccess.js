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

    async getGame(email) {
        return {
            "id": "GUID",
            "currentPlayer": "you@gmail.com",
            "opponent": {
                "mail": "test@gmail.com",
                "class": {
                    "id": 1,
                    "name": "Schnell"
                }
            },
            "player": {
                "mail": "you@gmail.com",
                "class": {
                    "id": 3,
                    "name": "Langsam"
                }
            },
            "beginDate": "2017-11-29T10:00.123+01:00",
            "isFinished": false
        }
    }

    async startNewGame(email) {}

    async getOpenGames() {
        return [{
            opponent: {
                email: "enemy@now.com"
            },
            beginDate: "20.11.2010"
        }, {
            opponent: {
                email: "test@gmail.com"
            },
            beginDate: "14.12.2014"
        }]
    }

}

const defaultWebAccess = new webAccess();

export default defaultWebAccess;