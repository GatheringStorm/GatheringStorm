import React from "react";

import { State } from "../../controller/statemachine.js"
import { Action } from "../../controller/statemachine.js"

class Head extends React.Component {
  constructor(props) {
    super(props);

    this.headerChoice = this.headerChoice.bind(this)
  }

  headerChoice() {
    if(this.props.stateMachine.currentState == State.GAME)
    return (<div />);
    else if(this.props.stateMachine.currentState == State.GAMESELECTION)
    return (<div />);
    else
    return (<div />);
  }

  render() {
    return this.headerChoice();
  }
}

export default Head;
