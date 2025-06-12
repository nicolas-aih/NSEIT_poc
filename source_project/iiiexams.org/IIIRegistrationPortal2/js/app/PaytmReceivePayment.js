﻿$(document).ready(function () {

    function openBlinkCheckoutPopup() {
        var orderId = $("#OrderId").val();
        var txnToken = $("#TraToken").val();
        var amount = $("#TraValue").val();

        var config = {
            "root": "",
            "flow": "DEFAULT",
            "data": {
                "orderId": orderId,
                "token": txnToken,
                "tokenType": "TXN_TOKEN",
                "amount": amount
            },
            "handler": {
                "notifyMerchant": function (eventName, data) {
                    //console.log("notifyMerchant handler function called");
                    //console.log("eventName => ", eventName);
                    //console.log("data => ", data);
                    location.reload();
                }
            }
        };
        if (window.Paytm && window.Paytm.CheckoutJS) {
            // initialze configuration using init method
            window.Paytm.CheckoutJS.init(config).then(function onSuccess() {
                // after successfully updating configuration, invoke checkoutjs
                window.Paytm.CheckoutJS.invoke();
            }).catch(function onError(error) {
                console.log("error => ", error);
            });
        }
    }

    setTimeout(function () {
        openBlinkCheckoutPopup();
    }, 1000);
});