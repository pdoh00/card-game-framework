/// <reference path="../Libs/moment.js" />

(function (namespace) {
    var functionName = 'toolkit';

    //creates toolkit
    var createToolkit = function () {

        //#region Object Prototypes

        String.prototype.trimEnd = function (c) {
            if (c)
                return this.replace(new RegExp(c.escapeRegExp() + "*$"), '');
            return this.replace(/\s+$/, '');
        };
        String.prototype.trimStart = function (c) {
            if (c)
                return this.replace(new RegExp("^" + c.escapeRegExp() + "*"), '');
            return this.replace(/^\s+/, '');
        };
        String.prototype.escapeRegExp = function () {
            return this.replace(/[.*+?^${}()|[\]\/\\]/g, "\\$0");
        };

        //#endregion

        var logging = function (options) {

            //exit out if console doesn't exist
            if (!window.console) {
                return false;
            }

            var appName = options.appName || 'CLIENT APP';
            var showTimeStamp = options.showTimeStamp || true;
            var timeStamp = '<' + moment(Date.now()).format('hh:mm:ss') + '> ';
            if (!showTimeStamp) {
                timeStamp = '';
            }
            var prepender = timeStamp + appName + ':::==> ';

            var logError = function (message) {
                console.error(prepender + message);
            };
            var logWarning = function (message) {
                console.warn(prepender + message);
            };
            var logInfo = function (message) {
                console.info(prepender + message);
            };
            var logDebug = function (message) {
                if (console.debug) {
                    console.debug(prepender + message);
                } else {
                    console.log(prepender + message);
                }
            };
            
            //reveal public objects
            return {
                //METHODS
                logError: logError,
                logWarning: logWarning,
                logInfo: logInfo,
                logDebug: logDebug
            };
        };
        var urlHelpers = function () {

            var getUrlEncodedKey = function (key, query) {
                if (!query)
                    query = window.location.search;
                var re = new RegExp("[?|&]" + key + "=(.*?)&");
                var matches = re.exec(query + "&");
                if (!matches || matches.length < 2)
                    return "";
                return decodeURIComponent(matches[1].replace("+", " "));
            };

            var setUrlEncodedKey = function (key, value, query) {
                query = query || window.location.search;
                var q = query + "&";
                var re = new RegExp("[?|&]" + key + "=.*?&");
                if (!re.test(q))
                    q += key + "=" + encodeURI(value);
                else
                    q = q.replace(re, "&" + key + "=" + encodeURIComponent(value) + "&");
                q = q.trimStart("&").trimEnd("&");
                return q[0] == "?" ? q : q = "?" + q;
            };

            //reveal public objects
            return {
                //METHODS
                getUrlEncodedKey: getUrlEncodedKey,
                setUrlEncodedKey: setUrlEncodedKey
            };
        };

        //reveal public objects
        return {
            //METHODS
            logging: logging,
            urlHelpers: urlHelpers
        };
        
    };

    //assign create for the object
    namespace[functionName] = {
        create: createToolkit
    };
})(window.DEVGUYS = window.DEVGUYS || {});