import React from "react";

import Header from "./Header/Header.jsx";
import Body from "./Body/Body.jsx";

class GameSelection extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <table>
        <tbody>
          <tr>
            <td>
              <Header stateMachine={this.props.stateMachine} />
            </td>
          </tr>
          <tr>
            <td>
              <Body />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default GameSelection;
