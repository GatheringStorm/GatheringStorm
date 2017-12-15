import React from "react";

import defaultWebAccess from "../../../../controller/webAccess.js";
import { Action } from "../../../../controller/statemachine.js"

class GameSelectionCard extends React.Component {
  constructor(props) {
    super(props);

    this.play = this.play.bind(this);

    this.createStyle = this.createStyle.bind(this)
  }

  play() {
    if (this.props.mode == "new") {
      defaultWebAccess.startNewGame(document.getElementById("newEnemy").value)
      return
    }
    defaultWebAccess.getGame(this.props.email)
    this.props.stateMachine.action(Action.STARTGAME);
  }

  createStyle() {
    let style = "card ";
    if (this.props.mode == "new")
      style += "newGame ";
    else if (this.props.currentTurnPlayer == JSON.parse(localStorage.getItem("userToken")).profileObj.email)
      style += "yourTurn ";
    else
      style += "enemyTurn";

    return style;
  }

  render() {
    return (
      <table onClick={this.props.mode == "new" ? "" : this.play} className={this.createStyle()}>
        <tbody>
          <tr>
            <td>
              <p>{this.props.mode == "new" ? "New Game" : this.props.email}</p>
            </td>
          </tr>
          <tr>
            <td>
              {this.props.mode == "new" ? <input type="email" className="maxWidth" id="newEnemy" /> : <p>Start: {this.props.date}</p>}
            </td>
          </tr>
          <tr>
            <td>
              {this.props.mode == "new" ?
                <button className="maxWidth" onClick={this.play}>{this.props.mode == "new" ? "Start Game" : "Play"}</button> : null
              }
            </td>
          </tr>
        </tbody>
      </table>)
  }
}

export default GameSelectionCard;
