(function () {
    window.spawn = window.spawn || function (gen) {
        function continuer(verb, arg) {
            var result;
            try {
                result = generator[verb](arg);
            } catch (err) {
                return Promise.reject(err);
            }
            if (result.done) {
                return result.value;
            } else {
                return Promise.resolve(result.value).then(onFulfilled, onRejected);
            }
        }
        var generator = gen();
        var onFulfilled = continuer.bind(continuer, 'next');
        var onRejected = continuer.bind(continuer, 'throw');
        return onFulfilled();
    };

    window.showModalDialog = window.showModalDialog || function (url, arg, opt) {
        url = url || ''; //URL of a dialog
        arg = arg || null; //arguments to a dialog
        opt = opt; //options: dialogTop;dialogLeft;dialogWidth;dialogHeight or CSS styles
        //opt = 'dialogWidth:650px;dialogHeight:550px;background-color:White !important;'; //options: dialogTop;dialogLeft;dialogWidth;dialogHeight or CSS styles
        //////debugger;
        var caller = showModalDialog.caller.toString();
        var dialog = document.body.appendChild(document.createElement('dialog'));

        dialog.setAttribute('style', opt.replace(/dialog/gi, '') + 'background-color:White !important;');
        dialog.innerHTML = '<a href="#" id="dialog-close" style="position: absolute; top: 0; right: 4px; font-size: 20px; color: #000; text-decoration: none; outline: none;">&times;</a><iframe id="dialog-body" src="' + url + '" style="border: 0; width: 100%; height: 100%;"></iframe>';

        document.getElementById('dialog-body').contentWindow.dialogArguments = arg;

        dialog.showModal();

        document.getElementById('dialog-close').addEventListener('click', function (e) {
            e.preventDefault();
            dialog.close();
        });

        $(document).ready(function () {
            $('#dialog-body').load(function () {
                var iframe = $('#dialog-body').contents();
                iframe.find("#btn_Ret").click(function (e) {
                    e.preventDefault();
                    var val = iframe.find("#Hid_Id").val();
                    window.returnValue = val;
                    if (val != '') {
                        dialog.close();
                    }
                    return false;
                });

                iframe.find("#btn_Cancel").click(function (e) {
                    e.preventDefault();
                    var val = iframe.find("#Hid_Id").val();
                    window.returnValue = "";
                    //if (val != '') {
                    //    dialog.close();
                    //}
                    //return false;
                    if (val == "") {
                        dialog.close();
                    }
                });

                iframe.find("#btn_Save").click(function (e) {
                    e.preventDefault();
                    var val = iframe.find("#Hid_Id").val();
                    window.returnValue = val;
                    if (val != "E") {
                        dialog.close();
                    }
                    return false;
                }); //btn_CalcRateClose

                iframe.find("#btn_Submit").click(function (e) {
                    e.preventDefault();
                    var val = iframe.find("#Hid_Id").val();
                    window.returnValue = val;
                    dialog.close();
                    return false;
                }); //btn_C

                iframe.find("#btn_Update").click(function (e) {
                    e.preventDefault();
                    var val = iframe.find("#Hid_Id").val();
                    window.returnValue = val;
                    dialog.close();
                    return false;
                });
            });
        });

        //if using yield
        if (caller.indexOf('yield') >= 0) {
            return new Promise(function (resolve, reject) {
                dialog.addEventListener('close', function () {
                    var returnValue = document.getElementById('dialog-body').contentWindow.returnValue;
                    document.body.removeChild(dialog);
                    resolve(returnValue);
                });
            });
        }

        //if using eval
        var isNext = false;
        var nextStmts = caller.split('\n').filter(function (stmt) {
            if (isNext || stmt.indexOf('showModalDialog(') >= 0)
                return isNext = true;
            return false;
        });

        dialog.addEventListener('close', function () {
            var returnValue = document.getElementById('dialog-body').contentWindow.returnValue;
            if (typeof (returnValue) != "undefined") {
                var ArrVal = returnValue.split(":");
                var first = ArrVal[1];
                if (first == "D") {
                    document.body.removeChild(dialog);
                    nextStmts[0] = nextStmts[0].replace(/(window\.)?showModalDialog\(.*\)/g, JSON.stringify(returnValue));
                    eval('{{\n' + nextStmts.join('\n'));
                }
                else {
                    document.body.removeChild(dialog);
                    nextStmts[0] = nextStmts[0].replace(/(window\.)?showModalDialog\(.*\)/g, JSON.stringify(returnValue));
                    eval('{\n' + nextStmts.join('\n'));
                }
            }
            else {
                document.body.removeChild(dialog);
                nextStmts[0] = nextStmts[0].replace(/(window\.)?showModalDialog\(.*\)/g, JSON.stringify(returnValue));
                eval('{{\n' + nextStmts.join('\n'));
            }
        });
        throw 'Execution stopped until showModalDialog is closed';
    };
})();