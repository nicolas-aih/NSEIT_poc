$(document).ready(function () {
    InitTicker();

    function InitTicker() {
        GetTicker();
        setInterval(GetTicker, 600000); //5 min...
    }

    function GetTicker() {
        $.ajax({
            type: "GET",
            url: "../Home/Ticker",
            success: function (msg) {
                $("#m1").html(msg);
            },
            error: function (msg) {

            }
        })
    }
 })
