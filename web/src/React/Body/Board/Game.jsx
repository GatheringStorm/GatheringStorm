import React from "react";


import Board from "./Board.jsx"
import defaultWebAccess from "../../../controller/webAccess.js"

class Game extends React.Component {
    constructor(props) {
        super(props);

        this.state = { board: null }

    }

    async componentDidMount() {
        this.setState({
            board: await defaultWebAccess.getBoard("")
        })
    }

    render() {
        if (this.state.board == null)
            return <p>Loading ... </p>
        return (
            <table>
                <tbody>
                    <tr>
                        <td>
                            <p>{JSON.parse(localStorage.getItem("userToken")).profileObj.email} vs {this.state.board.opponentPlayer}</p>
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
