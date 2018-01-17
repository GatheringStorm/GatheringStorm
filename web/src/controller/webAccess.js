import {
    addNewLogMessage
} from "./Log.js"

const baseUrl = "http://localhost:5000/api/games/";

class webAccess {
    async startNewGame(opponentMail, classTypes) {
        return await this.post('new', {
            opponentMail: opponentMail,
            classTypes: classTypes
        });
    }

    async joinGame(gameId, classTypes) {
        return await this.post('new', {
            gameId: gameId,
            classTypes: classTypes
        });
    }

    async getGames() {
        return await this.get('');
    }

    async getBoard(gameId) {
        return await this.get(gameId + '/board');
    }

    async endTurn(gameId) {
        return await this.post(gameId + '/endTurn');
    }

    async playCard(gameId, cardId, discardedCardIds, effectTargets) {
        return await this.post(gameId + '/playCard', {
            cardId: cardId,
            discardedCardIds: discardedCardIds,
            effectTargets: effectTargets
        });
    }

    async attack(gameId, attackerId, targetId) {
        return await this.post(gameId + '/attack', {
            attackerId: attackerId,
            targetId: targetId
        });
    }

    async post(route, payload) {
        let response = await fetch(baseUrl + route, {
            method: "POST",
            body: JSON.stringify(payload),
            headers: this.getHeaders()
        });
        return await this.handleResponse(response);
    }

    async get(route) {
        let response = await fetch(baseUrl + route, {
            method: "GET",
            headers: this.getHeaders()
        });
        return await this.handleResponse(response);
    }

    async handleResponse(response) {
        if (!response.ok) {
            /* response.body format: 
            {
                errorMessage: 'errorMessage',
                result: enum // possible values: userError, serverError, ruleError
            }
            */
            console.log('Request error: ' + route, await response.json());
            throw new Error('Request error, TODO: Error handling.');
        }
        return await response.json();
    }

    getHeaders() {
        let token = JSON.parse(localStorage.getItem("userToken"));
        return {
            "Authorization": token.Zi.token_type + " " + token.Zi.id_token,
            "Accept": "application/json",
            "Content-Type": "application/json"
        };
    }
}

const defaultWebAccess = new webAccess();

export default defaultWebAccess;