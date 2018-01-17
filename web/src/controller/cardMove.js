import {
    target
} from "./targeting.js"
import {
    setSelector
} from "./targeting.js"
import {
    getSelector
} from "./targeting.js"
import {
    resetTargets
} from "./targeting.js"
import {
    getTargets
} from "./targeting.js"

export function cardMove(card, pos, owner, payCost) {
    if (getSelector == card) {
        resetTargets();
        return true
    }
    switch (pos) {
        case "board":
            // attack
            if (owner == "you") {
                resetTargets();
                Selector(card);
            } else {
                // maximum 1 target
                if (target(card, 1))
                    return {
                        move: "attack",
                        targets: getTargets()
                    };
                return "pending";
            }
            break;
        case "hand":
            // pay costs
            if (target(card, payCost))
                return {
                    move: "pay",
                    targets: getTargets()
                };
            return "pending";
            break;
        default:
            break;
    }
}