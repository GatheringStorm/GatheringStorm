import React from "react";

import GameSelectionCard from "./GameSelectionCard.jsx"
import defaultWebAccess from "../../../../controller/webAccess.js"

class Body extends React.Component {
    constructor(props) {
        super(props);

        this.state = { data: null }
    }

    async getOpenGames() {
        let response = await defaultWebAccess.getGames();
        if (response == undefined)
            return [];
        return response.map(_ => {
            return _;
        });
    }

    async componentDidMount() {
        let games = [{ status: "new" }];
        games.push(...await this.getOpenGames());
        let obj = (
            <div className="flex-container-horizontal-wrap">{
                games.map((item, index) => {
                    return <GameSelectionCard key={index} stateMachine={this.props.stateMachine} mode={item.status} ID={item.id} email={item.opponentMail} date={item.beginDate} currentTurnPlayer={item.currentTurnPlayer} gameState={item.state} />
                })
            }</div>
        )

        this.setState({
            data: obj
        })
    }

    render() {
        if (this.state.data == null)
            return <p>Loading ...</p>
        return this.state.data
    }
}

export default Body;
