import React from "react";

import Header from "./Header/Header.jsx";
import Body from "./Body/Body.jsx";

class GameSelection extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="Layout">
        <Header stateMachine={this.props.stateMachine} />
        <Body stateMachine={this.props.stateMachine} />
      </div>
    );
  }
}

export default GameSelection;
