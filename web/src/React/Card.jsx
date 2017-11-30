import React from "react";

import Header from "./Card/Header.jsx";
import Effect from "./Card/Effect.jsx";
import Stats from "./Card/Stats.jsx";

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
