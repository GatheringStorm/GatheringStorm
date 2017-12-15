import React from "react";

import Card from "./Card/Card.jsx"

class Board extends React.Component {
    constructor(props) {
        super(props);

        this.state = { OpponentHand: null, OpponentBoard: null, PlayerBoard: null, PlayerHand: null }
    }

    componentDidMount() {
        let oBoard = [];
        let pBoard = [];
        let pHand = [];
        oBoard.push(...this.props.board.opponentBoardCards);
        pBoard.push(...this.props.board.playerBoardCards);
        pHand.push(...this.props.board.playerHandCards);
        let oH = (
            <div>{this.props.board.opponentHandCardsCount}</div>
        )
        let oB = (
            <div>{
                oBoard.map((item, index) => {
                    return <Card key={index} card={item} />
                })
            }</div>
        )
        let pB = (
            <div>{
                pBoard.map((item, index) => {
                    return <Card key={index} card={item} />
                })
            }</div>
        )
        let pH = (
            <div>{
                pHand.map((item, index) => {
                    return <Card key={index} card={item} />
                })
            }</div>
        )

        this.setState({
            OpponentHand: oH,
            OpponentBoard: oB,
            PlayerBoard: pB,
            PlayerHand: pH
        })
    }

    render() {
        return (
            <table className="board maxWidth">
                <tbody>
                    <tr>
                        <td className="OpponentHand">
                            {this.state.OpponentHand}
                        </td>
                    </tr>
                    <tr>
                        <td className="OpponentBoard">
                            {this.state.OpponentBoard}
                        </td>
                    </tr>
                    <tr>
                        <td className="PlayerHand">
                            {this.state.PlayerHand}
                        </td>
                    </tr>
                    <tr>
                        <td className="PlayerBoard">
                            {this.state.PlayerBoard}
                        </td>
                    </tr>
                </tbody>
            </table>

        )
    }
}

export default Board;