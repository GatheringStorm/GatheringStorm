import React from "react";

import { GoogleLogout } from "react-google-login";
import { Action } from "../../../../controller/statemachine.js"

class Header extends React.Component {
    constructor(props) {
        super(props);

        this.logout = this.logout.bind(this);
    }

    logout() {
        localStorage.removeItem("userToken")
        this.props.stateMachine.action(Action.LOGOUT);
    }

    render() {
        return (
            <div>
                <h1>Select a game</h1>
                <p>{JSON.parse(localStorage.getItem("userToken")).profileObj.email}</p>
                <GoogleLogout
                    buttonText="Logout"
                    onLogoutSuccess={this.logout}
                />
            </div>
        );
    }
}

export default Header;
