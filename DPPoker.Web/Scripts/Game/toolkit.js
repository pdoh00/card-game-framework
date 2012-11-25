/// <reference path="../Libs/moment.js" />

(function (namespace) {
    var functionName = 'toolkit';

    //creates toolkit
    var createToolkit = function () {

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

        //reveal public objects
        return {
            //METHODS
            logging: logging
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createToolkit
    };
})(window.DEVGUYS = window.DEVGUYS || {});