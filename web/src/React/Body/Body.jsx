import React from "react";

import { State } from "../../controller/statemachine.js"
import { Action } from "../../controller/statemachine.js"

class Body extends React.Component {
  constructor(props) {
    super(props);

    this.bodyChoice = this.bodyChoice.bind(this)
  }

  bodyChoice() {
    if(this.props.stateMachine.currentState.name == State.GAME)
    return (<div />);
    else if(this.props.stateMachine.currentState.name == State.GAMESELECTION)
    return (<div />);
    else if(this.props.stateMachine.currentState.name == State.LOGIN)
    return (<div className="g-signin2" data-onsuccess="onSignIn" />);
    else
    return <div />
  }

  render() {
    return this.bodyChoice();
  }
}

export default Body;
