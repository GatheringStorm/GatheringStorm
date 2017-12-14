import React from "react";

import GameSelectionCard from "./GameSelectionCard.jsx"
import defaultWebAccess from "../../../../controller/webAccess.js"

class Body extends React.Component {
    constructor(props) {
        super(props);

        this.state = { data: null }
    }

    async getOpenGames() {
        let response = await defaultWebAccess.getOpenGames();
        return response.map(_ => {
            _.mode = "exising";
            return _;
        });
    }

    async componentWillMount() {
        let testGames = [{ mode: "new", opponent: { email: "" }, beginDate: "" }];
        testGames.push(...await this.getOpenGames());
        let obj = (
            <div>{
                testGames.map((item, index) => {
                    <GameSelectionCard mode={item.mode} email={item.opponent.mail} date={item.beginDate} />
                })
            }</div>
        )

        this.setState({
            data: obj
        })

        console.log(obj)
    }

    render() {
        if (this.state.data == null)
            return <p>Loading ...</p>
        return this.state.data
    }
}

export default Body;
