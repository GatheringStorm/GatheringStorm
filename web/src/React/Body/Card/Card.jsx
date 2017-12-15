import React from "react";

import Header from "./Header.jsx";
import Effect from "./Effect.jsx";
import Stats from "./Stats.jsx";

class Card extends React.Component {
  constructor(props) {
    super(props);

  }
  render() {
    return (
      <table>
        <tbody>
          <tr>
            <td>
              <Header name={this.props.name.name} title={this.props.name.title} cost={this.props.cost} />
            </td>
          </tr>
          <tr>
            <td>
              <Effect
                description={this.props.effect.description}
                targetCount={this.props.effect.targetCount}
              />
            </td>
          </tr>
          <tr>
            <td>
              <Stats attack={this.props.stats.attack} hp={this.props.stats.hp} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default Card;
