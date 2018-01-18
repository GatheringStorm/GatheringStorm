import {
    saveSelector
} from "./playSaves.js"

var Selector = null;
var selectedTargets = 0;
var targets = [];

export function target(card, max) {
    targets[selectedTargets] = card;
    selectedTargets += 1;
    if (max <= selectedTargets)
        return true;
    return false;
}

function removeTarget(card) {
    target.forEach((item, index, object) => {
        if (item == card) {
            object.splice(index, 1);
        }
    });
}

export function setSelector(card) {
    Selector = card;
    saveSelector(card);
}

export function getSelector() {
    return Selector;
}

export function getTargets() {
    return targets;
}

export function resetTargets() {
    Selector = null;
    selectedTargets = 0;
    targets = [];
}