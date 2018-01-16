import {
    addNewLogMessage
} from "./Log.js"

const baseUrl = "http://localhost:5000/api/games/";

class webAccess {
    async startNewGame(opponentMail, classChoices) {
        return await this.post('new', {
            opponentMail: opponentMail,
            classChoices: classChoices
        });
    }

    async joinGame(gameId, classChoices) {
        return await this.post('new', {
            gameId: gameId,
            classChoices: classChoices
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
            body: payload,
            headers: this.getHeaders()
        });
        return this.handleResponse(response);
    }

    async get(route) {
        let response = await fetch(baseUrl + route, {
            method: "GET",
            headers: this.getHeaders()
        });
        return this.handleResponse(response);
    }

    handleResponse(response) {
        if (!response.ok) {
            /* response.body format: 
            {
                errorMessage: 'errorMessage',
                result: enum // possible values: userError, serverError, ruleError
            }
            */
            console.log('Request error: ' + route, response.body);
            throw new Error('Request error, TODO: Error handling.');
        }
        return response.body;
    }

    getHeaders() {
        let token = localStorage.getItem("userToken");
        return {
            "Authorization": token.token_type + " " + token.id_token,
            "Accept": "application/json",
            "Content-Type": "application/json"
        };
    }
}

const defaultWebAccess = new webAccess();

export default defaultWebAccess;