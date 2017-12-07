import React from "react";

import Head from "./Head/Head.jsx"
import Body from "./Body/Body.jsx"

class Layout extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <table>
        <tbody>
          <tr>
            <td>
              <Head />
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

export default Layout;
