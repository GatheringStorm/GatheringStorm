import React from "react";

import Head from "./Head/Head.jsx"
import Body from "./Body/Body.jsx"

class Layout extends React.Component {
  constructor(props) {
    super(props);

    this.update = this.update.bind(this);
  }

  update() {
    this.setState({})
  }

  componentDidMount() {
    this.timer = setInterval(this.update, 100)
  }

  render() {
    return (
      <table className="maxWidth">
        <tbody>
          <tr>
            <td>
              <Head stateMachine={this.props.stateMachine} />
            </td>
          </tr>
          <tr>
            <td className="maxWidth flex-container">
              <Body stateMachine={this.props.stateMachine} />
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

export default Layout;
