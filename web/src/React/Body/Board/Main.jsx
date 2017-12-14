import React from "react";


import Board from "./Board.jsx"
import defaultWebAccess from "../../../../controller/webAccess.js";

class Main extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div>header</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <Board />
                        </td>
                    </tr>
                </tbody>
            </table>
        )
    }
}

export default Main;
