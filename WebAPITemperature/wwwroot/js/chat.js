"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dataHub").build();


connection.on("SendTemp", function (temp) {
    console.log(temp);

    var table = document.getElementById("tempTable");
    var row = table.insertRow(1);

    row.insertCell(0).innerHTML = temp[0].temperatureId;
    row.insertCell(1).innerHTML = temp[0].date;
    row.insertCell(2).innerHTML = temp[0].temperatureC;
    row.insertCell(3).innerHTML = temp[0].temperatureF;
    row.insertCell(4).innerHTML = temp[0].humidity;
    row.insertCell(5).innerHTML = temp[0].pressure;
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

