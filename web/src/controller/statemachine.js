var stateMachine = new StateMachine();

let State = {
   LOGIN: 'LOGIN',
   OPENED: 'OPENED',
   LOCKED: 'LOCKED'
};

let Action = {
   LOGOUT: 'LOGOUT',
   OPEN: 'OPEN',
   LOCK: 'LOCK',
   UNLOCK: 'UNLOCK'
};

let config = [
   {
       initial: true,
       name: State.LOGIN,
       transitions: [
           { action: Action.OPEN, target: State.OPENED },
           { action: Action.LOCK, target: State.LOCKED }
       ]
   },
   {
       name: State.GAMESELECTION,
       transitions: [
           { action: Action.LOGOUT, target: State.LOGIN }
       ]
   },
   {
       name: State.LOCKED,
       transitions: [
           { action: Action.UNLOCK, target: State.LOGIN }
       ]
   }
];