var synergy = {
    ajaxResp: (
        stat = 0,
        respmsg = ""
    ),
    logData: function (val) {
        console.log(val);
    },
    blockUI: function () {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
    },
    unblockUI: function () {
        $.unblockUI();
    },
    validateVPA: function ($vpa) {
        var pattern = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+)+$/;

        return pattern.test($vpa);
    },

    PostData: function (data, url) {

        return $.ajax(
            {
                url: url,
                type: "POST",
                data: data,
                async: true,
                cache: false,
                crossDomain: false,
                timeout: 120000,
                dataType: "json"
            });
    },


    sendAjax: function (postUrl, jsonObject, hmacVal) {
        synergy.logData(postUrl);
        synergy.logData(jsonObject);

        synergy.PostData(jsonObject, postUrl)
            .done(function (response) {
                synergy.ajaxResp.respmsg = response;
                synergy.ajaxResp.stat = 1;
            }).fail(function (e, x, settings, exception) {
                var statusErrorMap = {
                    '400': "Server understood the request, but request content was invalid.",
                    '401': "Unauthorized access.",
                    '403': "Forbidden resource can't be accessed.",
                    '500': "Internal server error.",
                    '503': "Service unavailable."
                };
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
            });

    },
    GetQueryStringParams: function (name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    },

    dispAlert: function (type, val) {

        var $success = "green";
        var $successClass = "btn-green";
        var $failure = "red";
        var $failureClass = "btn-red";
        var $type, $btnClass;
        if (type === 1) {
            $type = $success;
            $btnClass = $successClass;
        } else {
            $type = $failure;
            $btnClass = $failureClass;
        }
        $.alert({
            title: 'Hyperion',
            content: val,
            animation: 'news',
            typeAnimated: true,
            type: $type,
            buttons: {
                Ok: {
                    btnClass: $btnClass,
                    action: function () { }
                }
            },

            closeAnimation: 'news'

        });
    }
};


function calcHash() {
    try {
        var hashInput = document.getElementById("hashInputText");
        var hashInputType = document.getElementById("hashInputType");
        var hashVariant = document.getElementById("hashVariant");
        var hashRounds = document.getElementById("hashRounds");
        var hashOutputType = document.getElementById("hashOutputType");
        var hashOutput = document.getElementById("hashOutputText");
        var hashObj = new jsSHA(
            hashVariant.options[hashVariant.selectedIndex].value,
            hashInputType.options[hashInputType.selectedIndex].value, { numRounds: parseInt(hashRounds.value, 10) }
        );
        hashObj.update(hashInput.value);
        hashOutput.value = hashObj.getHash(hashOutputType.options[hashOutputType.selectedIndex].value);
    } catch (e) {
        hashOutput.value = e.message;
    }
}

function calcHMAC(hmacText, hmacKeyInput, hmacVariant) {
    try {
        //var hmacText = document.getElementById("hmacInputText");
        var hmacTextType = "TEXT";
        //var hmacKeyInput = document.getElementById("hmacInputKey");
        var hmacKeyInputType = "TEXT";
        //var hmacVariant = document.getElementById("hmacVariant");
        var hmacOutputType = "HEX";
        var hmacOutput = document.getElementById("hmacOutputText");
        var hmacObj = new jsSHA(
            hmacVariant,
            hmacTextType.options[hmacTextType.selectedIndex].value
        );
        hmacObj.setHMACKey(
            hmacKeyInput.value,
            hmacKeyInputType.options[hmacKeyInputType.selectedIndex].value
        );
        hmacObj.update(hmacText.value);

        hmacOutput.value = hmacObj.getHMAC(hmacOutputType.options[hmacOutputType.selectedIndex].value);
    } catch (e) {
        hmacOutput.value = e.message;
    }
}