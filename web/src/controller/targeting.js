var Selector = null;
var selectedTargets = 0;
var targets = [];

export function target(card, max) {
    targets[selectedTargets] = card;
    if (max == selectedTargets)
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
}

export function getSelector(card) {
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