import React from "react";

import defaultWebAccess from "../../../../controller/webAccess.js";
import { Action } from "../../../../controller/statemachine.js"

class GameSelectionCard extends React.Component {
  constructor(props) {
    super(props);

    this.play = this.play.bind(this);

    this.createStyle = this.createStyle.bind(this);
    this.joinGame = this.joinGame.bind(this);
  }

  play() {
    defaultWebAccess.getBoard(this.props.ID)
    this.props.stateMachine.action(Action.STARTGAME);
  }

  joinGame() {
    let prio = {
      quick: document.getElementById("Prio-Quick").value,
      medium: document.getElementById("Prio-Medium").value,
      slow: document.getElementById("Prio-Slow").value
    };

    if (prio.quick == prio.medium || prio.quick == prio.slow || prio.medium == prio.slow || parseInt(prio.quick) + parseInt(prio.medium) + parseInt(prio.slow) != 6 || prio.quick == null || prio.medium == null || prio.slow == null) {
      return null;
    };

    let prioSorted = [];
    prioSorted[prio.quick - 1] = "quick";
    prioSorted[prio.medium - 1] = "medium";
    prioSorted[prio.slow - 1] = "slow";

    defaultWebAccess.joinGame(this.props.ID, prioSorted);

    window.location.reload();
  }

  createNewGame() {
    let enemy = document.getElementById("enemy-email").value;
    if (enemy == null)
      return;

    let prio = {
      quick: document.getElementById("Quick-Prio").value,
      medium: document.getElementById("Medium-Prio").value,
      slow: document.getElementById("Slow-Prio").value
    };

    if (prio.quick == prio.medium || prio.quick == prio.slow || prio.medium == prio.slow || parseInt(prio.quick) + parseInt(prio.medium) + parseInt(prio.slow) != 6 || prio.quick == null || prio.medium == null || prio.slow == null) {
      return null;
    };

    let prioSorted = [];
    prioSorted[prio.quick - 1] = "quick";
    prioSorted[prio.medium - 1] = "medium";
    prioSorted[prio.slow - 1] = "slow";

    defaultWebAccess.startNewGame(enemy, prioSorted);

    window.location.reload();
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
    if (this.props.mode == "new")
      return (
        <div>
          <button type="button" className="btn btn-primary" data-toggle="modal" data-target="#myModal">
            Start new Game
          </button>
          <div className="modal fade" id="myModal" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div className="modal-dialog" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title" id="exampleModalLabel">New Game</h5>
                </div>
                <div className="modal-body">
                  <form>
                    <div className="form-group">
                      <label htmlFor="enemy-email" className="form-control-label">Enemy:</label>
                      <input type="text" className="form-control" type="email" id="enemy-email" placeholder="example@example.com" />
                    </div>
                    <div className="spacer"></div>
                    <div className="form-group">
                      <label htmlFor="Quick-Prio" className="form-control-label">Priority Quick:</label>
                      <input id="Quick-Prio" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="1" />
                    </div>
                    <div className="form-group">
                      <label htmlFor="Medium-Prio" className="form-control-label">Priority Medium:</label>
                      <input id="Medium-Prio" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="2" />
                    </div>
                    <div className="form-group">
                      <label htmlFor="Slow-Prio" className="form-control-label">Priority Slow:</label>
                      <input id="Slow-Prio" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="3" />
                    </div>
                  </form>
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
                  <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.createNewGame}>Start Game</button>
                </div>
              </div>
            </div>
          </div>
        </div >
      );

    if (this.props.mode == "yourTurn" || this.props.mode == "opponentTurn")
      return (
        <table onClick={this.play} className={this.createStyle()}>
          <tbody>
            <tr>
              <td>
                <p>{this.props.email}</p>
              </td>
            </tr>
            <tr>
              <td>
                <p>Start: {this.props.date}</p>
              </td>
            </tr>
          </tbody>
        </table>
      );

    if (this.props.mode == "invitePending")
      return (
        <table className={this.createStyle()}>
          <tbody>
            <tr>
              <td>
                <p>{this.props.email}</p>
              </td>
            </tr>
            <tr>
              <td>
                <p>Invite Pending</p>
              </td>
            </tr>
          </tbody>
        </table>
      );

    if (this.props.mode == "won" || this.props.mode == "lost")
      return (
        <table className={this.createStyle()}>
          <tbody>
            <tr>
              <td>
                <p>{this.props.email}</p>
              </td>
            </tr>
            <tr>
              <td>
                <p>You {this.props.mode == "won" ? "won" : "lost"} the game</p>
              </td>
            </tr>
          </tbody>
        </table>
      );

    if (this.props.mode == "invited")
      return (
        <div>
          <button type="button" className="btn btn-primary" data-toggle="modal" data-target="#modalAccept">
            Accept Game
          </button>
          <div className="modal fade" id="modalAccept" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div className="modal-dialog" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title" id="exampleModalLabel">Accept Game</h5>
                </div>
                <div className="modal-body">
                  <form>
                    <div className="form-group">
                      <label htmlFor="enemy-email" className="form-control-label">Enemy:</label>
                      <input type="text" className="form-control" type="email" id="enemy-email" value={this.props.email} disabled={true} />
                    </div>
                    <div className="spacer"></div>
                    <div className="form-group">
                      <label htmlFor="Prio-Quick" className="form-control-label">Priority Quick:</label>
                      <input id="Prio-Quick" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="1" />
                    </div>
                    <div className="form-group">
                      <label htmlFor="Prio-Medium" className="form-control-label">Priority Medium:</label>
                      <input id="Prio-Medium" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="2" />
                    </div>
                    <div className="form-group">
                      <label htmlFor="Prio-Slow" className="form-control-label">Priority Slow:</label>
                      <input id="Prio-Slow" className="form-control" type="number" name="quantity" min="1" max="3" placeholder="3" />
                    </div>
                  </form>
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
                  <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.joinGame}>Join Game</button>
                </div>
              </div>
            </div>
          </div>
        </div >
      );
  }
}

export default GameSelectionCard;