import React from "react";

import Header from "./Header.jsx";
import Body from "./Body.jsx";
import FooterButton from "./FooterButton.jsx";

class GameSelectionCard extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <table>
        <tbody>
          <tr>
            <td>
              <Header name={props.name} />
            </td>
          </tr>
          <tr>
            <td>
              <Body
                date={props.date}
              />
            </td>
          </tr>
          <tr>
            <td>
              <FooterButton mode={props.mode} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default GameSelectionCard;
