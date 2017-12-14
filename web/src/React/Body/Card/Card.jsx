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
              <Header name={this.props.name} title={this.props.title} cost={this.props.cost} />
            </td>
          </tr>
          <tr>
            <td>
              <Effect
                description={this.props.description}
                targetCount={this.props.targetCount}
              />
            </td>
          </tr>
          <tr>
            <td>
              <Stats attack={this.props.attack} hp={this.props.hp} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default Card;
