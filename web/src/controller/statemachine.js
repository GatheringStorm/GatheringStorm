import StateMachine from "state-machine-js"

let stateMachine = new StateMachine();

export let State = {
    LOGIN: 'LOGIN',
    GAMESELECTION: 'GAMESELECTION',
    GAME: 'GAME'
};

export let Action = {
    LOGOUT: 'LOGOUT',
    LOGIN: 'OPEN',
    STARTGAME: 'STARTGAME',
    BACK: 'BACK'
};

let config = [{
        initial: true,
        name: State.LOGIN,
        transitions: [{
            action: Action.LOGIN,
            target: State.GAMESELECTION
        }]
    },
    {
        name: State.GAMESELECTION,
        transitions: [{
                action: Action.LOGOUT,
                target: State.LOGIN
            },
            {
                action: Action.STARTGAME,
                target: State.GAME
            }
        ]
    },
    {
        name: State.GAME,
        transitions: [{
            action: Action.BACK,
            target: State.GAMESELECTION
        }]
    }
];

stateMachine.onChange.add(function (state, data, action) {
    console.log('State has changed to:', state.name);
    console.log('Got data:', data);
    console.log('Got triggering action:', action);
});

stateMachine.create(config);
stateMachine.start();

export default stateMachine;