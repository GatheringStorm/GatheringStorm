import React from "react";


import Board from "./Board.jsx"
import defaultWebAccess from "../../../controller/webAccess.js"
import { Action } from "../../../controller/statemachine.js"

class Game extends React.Component {
    constructor(props) {
        super(props);

        this.state = { board: null }

        this.returnToGameSelection = this.returnToGameSelection.bind(this);
    }

    async componentDidMount() {
        this.setState({
            board: await defaultWebAccess.getBoard("")
        })
    }

    returnToGameSelection() {
        this.props.stateMachine.action(Action.BACK)
    }

    render() {
        if (this.state.board == null)
            return <p>Loading ... </p>
        return (
            <table className="maxWidth">
                <tbody>
                    <tr>
                        <td>
                            <input type="button" value="< Game Selection" onClick={this.returnToGameSelection} />
                            <p className="GameHeader">{JSON.parse(localStorage.getItem("userToken")).profileObj.email} vs {this.state.board.opponentPlayer}</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <Board board={this.state.board} />
                        </td>
                    </tr>
                </tbody>
            </table>
        )
    }
}

export default Game;
