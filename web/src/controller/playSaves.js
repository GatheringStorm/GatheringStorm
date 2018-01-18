var selector = null;
var pay = null;
var savedPay = 0;
var effect = [];
var savedEffect = 0;

export function saveSelector(value) {
    selector = value;
}

export function getSavedSelector() {
    return selector;
}

export function savePay(value) {
    pay[savedPay] = value;
    savedPay += 1;
}

export function getSavedPay() {
    return pay;
}

export function saveEffect(id, value) {
    effect[savedEffect] = {
        cardEffectId: id,
        targetsId: value
    };
    savedEffect += 1;
}

export function getSavedEffect() {
    return effect;
}

export function flush() {
    selector = null;
    pay = null;
    savedPay = 0;
    effect = [];
    savedEffect = 0;
}