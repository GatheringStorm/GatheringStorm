import React from "react";

import Card from "./Card/Card.jsx"
import defaultWebAccess from "../../../controller/webAccess.js"
import { getCurrentGame } from "../../../controller/currentBoard.js"
import { cardMove } from "../../../controller/cardMove.js"

class Board extends React.Component {
    constructor(props) {
        super(props);

        this.state = { OpponentHand: null, OpponentBoard: null, PlayerBoard: null, PlayerHand: null }

        this.createCard = this.createCard.bind(this);
    }

    componentDidMount() {
        let oBoard = [];
        let pBoard = [];
        let pHand = [];
        oBoard.push(...this.props.board.opponent.boardCards);
        pBoard.push(...this.props.board.player.boardCards);
        pHand.push(...this.props.board.playerHandCards);

        this.setState({
            OpponentHand: <div>{this.props.board.opponentHandCardsCount}</div>,
            OpponentBoard: this.createCard(oBoard, "board", "opp"),
            PlayerBoard: this.createCard(pBoard, "board", "you"),
            PlayerHand: this.createCard(pHand, "hand", "you")
        })
    }

    endTurn() {
        defaultWebAccess.endTurn(getCurrentGame());
    }

    createCard(cardMap, pos, owner) {
        return (<div className="flex-container-horizontal">{
            cardMap.map((item, index) => {
                return <Card key={index} card={item} onClick={() => use(item.id, pos, owner, item.cost)} />
            })
        }</div>)
    }

    use(id, pofs, owner, cost) {
        let response = cardMove(id, pos, owner, cost)

        if (response == "pending")
            return;

        switch (response.move) {
            case "attack":
                defaultWebAccess.attack(getCurrentGame(), id, response.targets[0]);
                break;
            case "pay":
                defaultWebAccess.attack(getCurrentGame(), id, response.targets);
                break;
            case "effect":
                defaultWebAccess.attack(getCurrentGame(), id, response.targets[0]);
                break;

            default:
                break;
        }
    }

    render() {
        return (
            <div className="Layout flex-container">
                <div>
                    <button>{this.state.playerHP}</button>
                </div>
                <div className="OpponentHand flex-container-horizontal">
                    {this.state.OpponentHand}
                </div>
                <div className="spacer"></div>
                <div className="board">
                    <div className="OpponentBoard flex-container-horizontal">
                        {this.state.OpponentBoard}
                    </div>
                    <div className="boardseperator flex-container-horizontal">
                        <div className="spacer"></div>
                        <button onClick={this.endTurn}>End Turn</button>
                    </div>
                    <div className="PlayerBoard flex-container-horizontal">
                        {this.state.PlayerBoard}
                    </div>
                </div>
                <div className="spacer"></div>
                <div className="PlayerHand flex-container-horizontal">
                    {this.state.PlayerHand}
                </div>
                <div>
                    <button>{this.state.opponentHP}</button>
                </div>
            </div>
        )
    }
}

export default Board;