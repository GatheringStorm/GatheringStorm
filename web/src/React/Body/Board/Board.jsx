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

        this.setState({
            OpponentHand: <div>{this.props.board.opponentHandCardsCount}</div>,
            OpponentBoard: this.createCard(oBoard),
            PlayerBoard: this.createCard(pBoard),
            PlayerHand: this.createCard(pHand)
        })
    }

    createCard(cardMap) {
        return (<div className="flex-container-horizontal">{
            cardMap.map((item, index) => {
                return <Card key={index} card={item} />
            })
        }</div>)
    }

    render() {
        return (
            <div className="Layout flex-container">
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
                    </div>
                    <div className="PlayerBoard flex-container-horizontal">
                        {this.state.PlayerBoard}
                    </div>
                </div>
                <div className="spacer"></div>
                <div className="PlayerHand flex-container-horizontal">
                    {this.state.PlayerHand}
                </div>
            </div>
        )
    }
}

export default Board;