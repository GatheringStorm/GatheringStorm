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
            _.mode = "existing";
            return _;
        });
    }

    async componentDidMount() {
        let testGames = [{ mode: "new", opponent: { email: "" }, beginDate: "" }];
        testGames.push(...await this.getOpenGames());
        let obj = (
            <div>{
                testGames.map((item, index) => {
                    return <GameSelectionCard key={index} mode={item.mode} email={item.opponent.email} date={item.beginDate} />
                })
            }</div>
        )
        console.log(testGames.map((item, index) => { console.log(item.mode); console.log(item.opponent.email); console.log(item.beginDate); }))

        this.setState({
            data: obj
        })
    }

    render() {
        if (this.state.data == null)
            return <p>Loading ...</p>
        return this.state.data
    }
}

export default Body;
