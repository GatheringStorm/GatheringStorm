import React from "react";

class Stats extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div>
                <div className="stats">{this.props.attack}</div>
                <div className="stats">{this.props.hp}</div>
            </div>
        );
    }
}

export default Stats;
