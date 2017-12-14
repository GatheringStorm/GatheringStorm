import React from "react";

import { State } from "../../controller/statemachine.js"
import { Action } from "../../controller/statemachine.js"
import { GoogleLogin } from "react-google-login";
import defaultWebAccess from "../../controller/webAccess.js"

class Body extends React.Component {
  constructor(props) {
    super(props);

    this.bodyChoice = this.bodyChoice.bind(this)
  }

  responseGoogle(response) {
    console.log(response);
    defaultWebAccess.sample(response.Zi, "");
  }

  bodyChoice() {
    if (this.props.stateMachine.currentState.name == State.GAME)
      return (<div />);
    else if (this.props.stateMachine.currentState.name == State.GAMESELECTION)
      return (<div />);
    else if (this.props.stateMachine.currentState.name == State.LOGIN)
      return (<GoogleLogin
        clientId="24931599658-o9q66rbqprq0lcgrtlbfhcs3kcfqs8rg.apps.googleusercontent.com"
        buttonText="Login"
        onSuccess={this.responseGoogle}
        onFailure={this.responseGoogle}
      />);
    else
      return <div />;
  }

  render() {
    return this.bodyChoice();
  }
}

export default Body;
