import React from "react";

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
      <div className="Layout">
        <Body stateMachine={this.props.stateMachine} />
      </div>
    );
  }
}

export default Layout;
