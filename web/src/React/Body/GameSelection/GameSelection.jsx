import React from "react";

import Header from "./Header.jsx";
import Body from "./Body.jsx";

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
              <Header/>
            </td>
          </tr>
          <tr>
            <td>
              <Body
                cards={}
              />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default GameSelection;
