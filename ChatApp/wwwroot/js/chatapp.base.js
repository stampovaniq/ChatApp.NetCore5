if (!CHATAPP) {
    var CHATAPP = new chatapp();
}

function chatapp() {
    this.key = new Date();
}

$(document).ready(function () {
    if (CHATAPP) {
        CHATAPP.func.init();
    }
});

chatapp.prototype.func = {};

chatapp.prototype.selectors = {};

chatapp.prototype.globalvar = {};
chatapp.prototype.globalvar.hubconnection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

chatapp.prototype.func.init = function () {
    // starting signalR
    CHATAPP.globalvar.hubconnection.start().catch(err => alert(err.toString()));
};