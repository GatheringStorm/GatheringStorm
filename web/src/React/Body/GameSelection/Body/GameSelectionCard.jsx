import React from "react";

import defaultWebAccess from "../../../../controller/webAccess.js";
import { Action } from "../../../../controller/statemachine.js"

class GameSelectionCard extends React.Component {
  constructor(props) {
    super(props);

    this.play = this.play.bind(this);
  }

  play() {
    if (this.props.mode == "new") {
      defaultWebAccess.startNewGame(document.getElementById("newEnemy").value)
      return
    }
    defaultWebAccess.getGame(this.props.email)
    this.props.stateMachine.action(Action.STARTGAME);
  }

  render() {
    return (
      <table>
        <tbody>
          <tr>
            <td>
              <p>{this.props.mode == "new" ? "New Game" : this.props.email}</p>
            </td>
          </tr>
          <tr>
            <td>
              {this.props.mode == "new" ? <input type="email" id="newEnemy" /> : <p>Start: {this.props.date}</p>}
            </td>
          </tr>
          <tr>
            <td>
              <button onClick={this.play}>{this.props.mode == "new" ? "Start Game" : "Play"}</button>
            </td>
          </tr>
        </tbody>
      </table>)
  }
}

export default GameSelectionCard;
