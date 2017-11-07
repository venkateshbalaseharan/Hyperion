var MercDtl = {
    MerchantId : "",
    Channel: "",
    TxnRefnum: "",
    Amount: "",
    MerchantVpa: "",
    Expiry: "",
    PayerName: "",
    PayerVpa:"",
    RetUrl: ""
};

(function () {
    
    var $valVpaUrl = "api/merchantupi/validate";
    var $initCollectUrl = "api/merchantupi/collect";
    var $paymentStatUrl = "api/merchantupi/status";
    var statusErrorMap = {
        '400': "Server understood the request, but request content was invalid.",
        '401': "Unauthorized access.",
        '403': "Forbidden resource can't be accessed.",
        '500': "Internal server error.",
        '503': "Service unavailable."
    };

    function initiate_collect() {

         
        var $initCollectDto = {
            Channel: MercDtl.Channel,
            MerchantId: MercDtl.MerchantId,
            Payer: MercDtl.PayerVpa,
            Payee: MercDtl.MerchantVpa,
            PayerName: MercDtl.PayerName,
            Amount: MercDtl.Amount,
            TxnRefnum: MercDtl.TxnRefnum,
            Expiry: MercDtl.Expiry,
            ReturnUrl: MercDtl.RetUrl
        };


        synergy.PostData($initCollectDto, $initCollectUrl)
            .done(function (response) {
                var rData = $.parseJSON(response);
                if (rData.RespCode.toString() === "1") {


                    var $payStatDto = {
                        Channel: MercDtl.Channel,
                        MerchantId: MercDtl.MerchantId,
                        TxnRefnum: MercDtl.TxnRefnum
                                          };


                    var EXPIRY = MercDtl.Expiry * 60; //keep it to be a multiple of 60
                    var SLEEP_TIME = '10';
                    var newTimeInSec, timeDiff, currentTimeInSeconds;
                    var serverTime = new Date();
                    var currSeconds = new Date().getTime() / 1000;
                    var ajaxHandle = {};
                    var perSecondPulse = {};
                    $(document).ready(function () {
                        perSecondPulse = setInterval(updateTime, 1000);
                        checkTransactionStatusByPolling();
                    });
                    /**
                     * 
                     */
                    function checkTransactionStatusByPolling() {
                        ajaxHandle = jQuery.ajax({
                            type: "POST",
                            url: $paymentStatUrl,
                            data: $payStatDto,
                            dataType: 'json',
                            success: function (output) {
                                var mData = $.parseJSON(output);
                                if (mData.RespCode.toString() === "2") {
                                    newTimeInSec = new Date().getTime() / 1000;
                                    timeDiff = newTimeInSec - currSeconds;
                                    if (timeDiff < EXPIRY) {
                                        setTimeout(checkTransactionStatusByPolling, SLEEP_TIME * 1000);
                                    } else {
                                        finishTransactionAjax(output);
                                    }
                                } else {
                                    showLoader();
                                    finishTransactionAjax(output);
                                }
                            },
                            complete: function () {

                            }, error: function (xhr, error) {
                                setTimeout(checkTransactionStatusByPolling, SLEEP_TIME * 1000);
                            }
                        });
                    }
                    function finishTransactionAjax(output) {
                        synergy.dispAlert(1, "success");
                        //TODO:
                    }
                    function updateTime() {
                        /// Increment serverTime by 1 second and update the html for '#time'
                        serverTime = new Date(serverTime.getTime() + 1000);
                        currentTimeInSeconds = serverTime.getTime() / 1000;
                        if (currentTimeInSeconds - currSeconds >= EXPIRY) {
                            abortPreviousPolls();
                            showLoader();
                            clearInterval(perSecondPulse);
                            checkTransactionStatusByPolling();
                        }
                    }

                    function showLoader() {
                    }

                    function abortPreviousPolls() {
                        if (ajaxHandle.abort) {
                            ajaxHandle.abort();
                        }
                    }

                    var clock = function () {
                        function getTimeRemaining(endtime) {
                            var t = Date.parse(endtime) - Date.parse(new Date());
                            var seconds = Math.floor((t / 1000) % 60);
                            var minutes = Math.floor((t / 1000 / 60) % 60);
                            // var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
                            // var days = Math.floor(t / (1000 * 60 * 60 * 24));
                            return {
                                'total': t,
                                // 'days': days,
                                // 'hours': hours,
                                'minutes': minutes,
                                'seconds': seconds
                            };
                        }

                        function initializeClock(endtime) {
                            var minutesSpan = document.getElementById('timerMinutes');
                            var secondsSpan = document.getElementById('timerSeconds');

                            function updateClock() {
                                var t = getTimeRemaining(endtime);
                                minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
                                secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

                                if (t.total <= 0) {
                                    clearInterval(timeinterval);
                                }
                            }

                            updateClock();
                            var timeinterval = setInterval(updateClock, 1000);
                        }

                        return {
                            initializeClock: initializeClock
                        };

                    }

                    function startTimer() {
                        var myClock = clock();
                        var deadLineMinutes = EXPIRY / 60;
                        var deadline = new Date(Date.parse(new Date()) + deadLineMinutes * 60 * 1000);
                        myClock.initializeClock(deadline);
                    }

                    //TODO:

                    $("#pnlVpa").hide();
                    $("#pnlProcessing").show();

                    startTimer();
                    
                }
                synergy.unblockUI();
            }).fail(function (e, x, settings, exception) {
                if (x.status) {
                    synergy.ajaxResp.respmsg = statusErrorMap[x.status];
                    if (!synergy.ajaxResp.respmsg) {
                        synergy.ajaxResp.respmsg = "Unknown Error \n.";
                    }
                } else if (exception === 'parsererror') {
                    synergy.ajaxResp.respmsg = "Error.\nParsing JSON Request failed.";
                } else if (exception === 'timeout') {
                    synergy.ajaxResp.respmsg = "Request Time out.";
                } else if (exception === 'abort') {
                    synergy.ajaxResp.respmsg = "Request was aborted by the server";
                } else {
                    synergy.ajaxResp.respmsg = "Unknown Error \n.";
                }
                synergy.ajaxResp.stat = 2;
                synergy.unblockUI();
            });
        
    }


    function validate_vpa() {

        if (MercDtl.PayerVpa === "") {
            synergy.dispAlert(2, "Please Enter Your VPA");
            return;
        }

        if (MercDtl.PayerVpa.length < 4) {
            synergy.dispAlert(2, "Please Check VPA");
            return;
        }

        if (!synergy.validateVPA(MercDtl.PayerVpa)) {
            synergy.dispAlert(2, "Invalid VPA");
            return;
        }

        var $valVpaDto = {
            Channel: MercDtl.Channel,
            Mid: MercDtl.MerchantId,
            Vpa: MercDtl.PayerVpa
        };

        synergy.PostData($valVpaDto, $valVpaUrl)
            .done(function (response) {
                var rData = $.parseJSON(response);
                if (rData.RespCode.toString() === "1") {
                    MercDtl.PayerName = rData.RespMsg.toString();
                    $.confirm({
                        title: 'Confirm!',
                        content: 'The Collect request will be sent to the account linked with ' + MercDtl.PayerName,
                        buttons: {
                            confirm: {
                                btnClass: 'btn-green',
                                text: 'Proceed',
                                action: function () {
                                    synergy.blockUI();
                                    initiate_collect();

                                    
                                }
                            },
                            cancel: function () {
                                $.alert('Canceled!');
                            }
                        }
                    });
                }
                synergy.unblockUI();
                }).fail(function (e, x, settings, exception) {
                if (x.status) {
                    synergy.ajaxResp.respmsg = statusErrorMap[x.status];
                    if (!synergy.ajaxResp.respmsg) {
                        synergy.ajaxResp.respmsg = "Unknown Error \n.";
                    }
                } else if (exception === 'parsererror') {
                    synergy.ajaxResp.respmsg = "Error.\nParsing JSON Request failed.";
                } else if (exception === 'timeout') {
                    synergy.ajaxResp.respmsg = "Request Time out.";
                } else if (exception === 'abort') {
                    synergy.ajaxResp.respmsg = "Request was aborted by the server";
                } else {
                    synergy.ajaxResp.respmsg = "Unknown Error \n.";
                }
                synergy.ajaxResp.stat = 2;
                synergy.unblockUI();
            });

            }


    $(document).ready(function () {

        var qryString = synergy.GetQueryStringParams("vData");
        var clrTxt = $.base64.decode(qryString);
        
        var values = clrTxt.split('|');
        MercDtl.MerchantId = values[0];
        MercDtl.TxnRefnum = values[1];
        MercDtl.Amount = values[2];
        MercDtl.Channel = values[3];
        MercDtl.MerchantVpa = values[4];
        MercDtl.Expiry = values[5];
        MercDtl.RetUrl = values[6];

        synergy.logData(MercDtl);

        $("#lblTxnId").html(MercDtl.TxnRefnum);
        $("#totalAmount").html(MercDtl.Amount);

        $('#pay_button').click(function () {
            synergy.blockUI();
            MercDtl.PayerVpa = $.trim($("#vpa").val());

            validate_vpa();
           
        });


    });   
})();