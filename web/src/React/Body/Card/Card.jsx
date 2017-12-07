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
              <Header name={props.name} title={props.title} cost={props.cost} />
            </td>
          </tr>
          <tr>
            <td>
              <Effect
                description={props.description}
                targetCount={props.targetCount}
              />
            </td>
          </tr>
          <tr>
            <td>
              <Stats attack={props.attack} hp={props.hp} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default Card;
