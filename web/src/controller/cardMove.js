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

export function cardMove(card, pos, owner, payCost, paid, effectCount) {
    if (getSelector() == card) {
        resetTargets();
        setSelector(null);
        return true;
    }
    switch (pos) {
        case "board":
            if (paid) {
                resetTargets();
                if (target(card, effectCount))
                    return {
                        move: "effect",
                        targets: getTargets()
                    }
                return "pending";
            } else {
                // attack
                if (owner == "you") {
                    resetTargets();
                    setSelector(card);
                } else {
                    // maximum 1 target
                    if (target(card, 1))
                        return {
                            move: "attack",
                            targets: getTargets()
                        };
                    return "pending";
                }

            }
            break;
        case "hand":
            if (getSelector() == null) {
                resetTargets();
                setSelector(card);
                return;
            }
            // pay costs
            if (target(card, payCost))
                return {
                    move: "pay",
                    targets: getTargets()
                };
            console.log(getTargets());
            return "pending";
            break;
        default:
            break;
    }
}