const css = require("./style/stylesheet.scss");

import React from "react";
import ReactDOM from "react-dom";
import Layout from "./React/Layout.jsx";

import stateMachine from "./controller/statemachine.js"

ReactDOM.render(<Layout stateMachine={stateMachine}/>, document.getElementById("Body"));
