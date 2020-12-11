/// <reference path="chatapp.base.js" />

$(document).ready(function () {
    if (CHATAPP) {
        CHATAPP.message.func.init();
    }
});

chatapp.prototype.message = {};
chatapp.prototype.message.func = {};

chatapp.prototype.selectors = {};

chatapp.prototype.message.func.init = function () {
    CHATAPP.message.func.addsubscribetobroadcastmessage();
    CHATAPP.message.func.addsubscriptionforconnecteddisconnectedusers();
}

chatapp.prototype.message.func.appendLine = function (uid, message) {
    let nameElement = document.createElement('strong');
    nameElement.innerText = `${uid}:`;
    let msgElement = document.createElement('em');
    msgElement.innerText = ` ${message}`;
    let li = document.createElement('li');
    li.appendChild(nameElement);
    li.appendChild(msgElement);
    $('#messageList').append(li);
}

chatapp.prototype.message.func.addsubscribetobroadcastmessage = function() {
    CHATAPP.globalvar.hubconnection.on("BroadcastMessage", function (user, message, timeMsgSent) {
        var finalMessage = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var displayMsg = timeMsgSent + " " + user + " : " + finalMessage;
        var li = document.createElement("li");
        li.textContent = displayMsg;
        document.getElementById("messageList").appendChild(li);
    });
}

chatapp.prototype.message.func.addsubscriptionforconnecteddisconnectedusers = function() {
    CHATAPP.globalvar.hubconnection.on('UserConnected', (ConnectionId, timeConnected) => {
        let _message = " Connected " + ConnectionId;
        var username = timeConnected + " " + document.getElementById("UserName").innerText;
        CHATAPP.message.func.appendLine(username, _message);
    });

    CHATAPP.globalvar.hubconnection.on('UserDisconnected', (ConnectionId, timeDisconnected) => {
        let _message = " Disconnected " + ConnectionId;
        var username = timeDisconnected + " " + document.getElementById("UserName").innerText;
        CHATAPP.message.func.appendLine(username, _message);
    });
}