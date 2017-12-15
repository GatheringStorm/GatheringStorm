import React from "react";

import Stats from "./Stats.jsx";

class Card extends React.Component {
  constructor(props) {
    super(props);

    this.state = { effects: null }
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

  render() {
    return (
      <table>
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