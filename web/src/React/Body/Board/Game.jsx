import React from "react";


import Board from "./Board.jsx"
import defaultWebAccess from "../../../controller/webAccess.js"
import { Action } from "../../../controller/statemachine.js"
import { getCurrentGame } from "../../../controller/currentBoard.js"

class Game extends React.Component {
    constructor(props) {
        super(props);

        this.state = { board: null }

        this.returnToGameSelection = this.returnToGameSelection.bind(this);
    }

    async componentDidMount() {
        this.setState({
            board: await defaultWebAccess.getBoard(getCurrentGame())
        })
    }

    returnToGameSelection() {
        this.props.stateMachine.action(Action.BACK)
    }

    render() {
        if (this.state.board == null)
            return <p>Loading ... </p>
        return (
            <div className="Layout">
                <input type="button" value="< Game Selection" onClick={this.returnToGameSelection} />
                <p className="GameHeader">{JSON.parse(localStorage.getItem("userToken")).profileObj.email} vs {this.state.board.opponent.mail}</p>
                <Board board={this.state.board} playerHP={this.state.board.player.health} opponentHP={this.state.board.opponent.health} />
            </div>
        )
    }
}

export default Game;
