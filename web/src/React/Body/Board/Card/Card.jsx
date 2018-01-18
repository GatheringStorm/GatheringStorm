import React from "react";

import Stats from "./Stats.jsx";
import { cardMove } from "../../../../controller/cardMove.js"
import { getCurrentGame } from "../../../../controller/currentBoard.js"
import { saveSelector } from "../../../../controller/playSaves.js"
import { getSavedSelector } from "../../../../controller/playSaves.js"
import { savePay } from "../../../../controller/playSaves.js"
import { getSavedPay } from "../../../../controller/playSaves.js"
import { saveEffect } from "../../../../controller/playSaves.js"
import { getSavedEffect } from "../../../../controller/playSaves.js"
import { flush } from "../../../../controller/playSaves.js"
import defaultWebAccess from "../../../../controller/webAccess.js"

class Card extends React.Component {
  constructor(props) {
    super(props);

    this.state = { effects: null, paid: false, effect: 0, countEffects: 0 }

    this.use = this.use.bind(this);
  }

  componentWillMount() {
    this.props.card.effects.forEach(element => {
      this.setState({
        countEffects: this.state.countEffects + 1
      });
    });
  }

  componentDidMount() {
    let effect = [];
    effect.push(...this.props.card.effects);
    let effects = (
      <div>{
        effect.map((item, index) => {
          return <p key={index}>{item.description}</p>
        })
      }</div>
    )

    this.setState({
      effects: effects
    })
  }

  use() {
    let response = cardMove(this.props.card.id, this.props.position, this.props.owner, this.props.card.cost, this.state.paid, this.props.card.effects[this.state.effect].targetsCount);

    if (response == "pending")
      return;

    switch (response.move) {
      case "attack":
        defaultWebAccess.attack(getCurrentGame(), this.props.card.id, response.targets[0]);
        break;
      case "pay":
        this.setState({
          paid: true,
        });
        savePay(response.targets);
        if (this.props.card.effects[this.state.effect].targetsCount == 0 || this.props.boardEmpty) {
          defaultWebAccess.playCard(getCurrentGame(), this.props.card.id, getSavedPay(), getSavedEffect());
          flush();
        }
        break;
      case "effect":
        saveEffect(this.state.effect, response.targets);
        if (this.state.effect == this.state.countEffects) {
          defaultWebAccess.playCard(getCurrentGame(), this.props.card.id, getSavedPay(), getSavedEffect());
          flush();
        } else {
          this.setState({
            effect: effect + 1
          })
        }
        this.setState({
          paid: false,
          pay: []
        });
        break;

      default:
        break;
    }
  }

  render() {
    return (
      <table onClick={this.use} className="card">
        <tbody>
          <tr>
            <td>
              <div>
                <h3>{this.props.card.name}</h3>
                <p>{this.props.card.cost}</p>
              </div>
              <p>{this.props.card.title}</p>
            </td>
          </tr>
          <tr>
            <td>
              {this.state.effects}
            </td>
          </tr>
          <tr>
            <td>
              <Stats attack={this.props.card.attack} hp={this.props.card.health} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default Card;
