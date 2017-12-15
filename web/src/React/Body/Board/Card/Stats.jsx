import React from "react";

class Stats extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div>
                <label className="stats col-md-1 col-lg-1 col-sm-1 col-xs-1">{this.props.attack}</label>
                <label className="stats col-md-1 col-md-offset-10 col-lg-1 col-lg-offset-10 col-sm-1 col-sm-offset-10 col-xs-1 col-xs-offset-10">{this.props.hp}</label>
            </div>
        );
    }
}

export default Stats;
