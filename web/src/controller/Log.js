var log = "";

export function addNewLogMessage(newLogMessage) {
  let date = new Date();
  log += date.today() + " @ " + date.timeNow() + " | " + newLogMessage + "\n";
  return true;
}

export function getLog() {
  return log;
}

// For todays date;
Date.prototype.today = function() {
  return (
    (this.getDate() < 10 ? "0" : "") +
    this.getDate() +
    "/" +
    (this.getMonth() + 1 < 10 ? "0" : "") +
    (this.getMonth() + 1) +
    "/" +
    this.getFullYear()
  );
};

// For the time now
Date.prototype.timeNow = function() {
  return (
    (this.getHours() < 10 ? "0" : "") +
    this.getHours() +
    ":" +
    (this.getMinutes() < 10 ? "0" : "") +
    this.getMinutes() +
    ":" +
    (this.getSeconds() < 10 ? "0" : "") +
    this.getSeconds()
  );
};
