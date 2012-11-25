/// <reference path="../Libs/jquery.signalR-0.5.3.js" />

//this object handles all client game interactions
(function (namespace) {
    var functionName = 'game';

    //creates a new client game object
    var createGame = function (options) {
        
        $.support.cors = options.supportsCors || true;
        $.connection.hub.url = options.hubUrl;

        var pokerServer = $.connection.pokerServer;
        var tableId = options.tableId || '';
        var logger = options.logger;

        //#region HUB GLOBAL CONNECTION HANDLERS

        //START
        $.connection.hub.start({ jsonp: true }, function () {
            logger.logInfo('Connection started!');
            pokerServer.getTableId();
        });
        //SENDING
        $.connection.hub.sending(function () {
            logger.logInfo('Establishing connection to server');
        });
        //RECEIVED
        $.connection.hub.received(function (data) {
            //logger.logInfo('Data received...' + JSON.stringify(data));
        });
        //ERROR
        $.connection.hub.error(function (error) {
            logger.logError(error);
        });
        //STATE CHANGED
        $.connection.hub.stateChanged(function (change) {
            if (change.newState === $.signalR.connectionState.reconnecting) {
                logger.logInfo('Re-connecting');
            }
            else if (change.newState === $.signalR.connectionState.connected) {
                logger.logInfo('The server is online');
            }
        });
        //RECONNECTED
        $.connection.hub.reconnected(function () {
            logger.logInfo('Reconnected');
        });

        //#endregion

        
        pokerServer.setTableId = function (id) {

            tableId = id;

            //request player positions
            pokerServer.getPlayerPositionsAtTable(tableId);
        }

        //#region PUBLIC METHODS

        //Seats the player at the table
        var takeSeat = function (args) {
            pokerServer.takeSeat(tableId, args.playerName);
        }
        //Indicates that the player is ready
        var playerReady = function (args) {
            pokerServer.playerReady(tableId, args.playerId);
        }
        //Will remove the player from the table and game
        var leaveGame = function (args) {
            pokerServer.leaveGame(tableId, args.playerId);
        }
        //Requests to get the player positions at the table
        var getPlayerPositionsAtTable = function () {
            pokerServer.getPlayerPositionsAtTable(tableId);
        }

        //#endregion

        //reveal public objects
        return {
            //FIELDS
            tableId: tableId,
            //METHODS
            takeSeat: takeSeat,
            playerReady: playerReady,
            leaveGame: leaveGame,
            //SERVER CALLBACKS
            broadcaseToConsole: pokerServer.broadcaseToConsole,
            dealCards: pokerServer.dealCards,
            playerTookSeat: pokerServer.playerTookSeat,
            playerLeftSeat: pokerServer.playerLeftSeat,
            setPlayerState: pokerServer.setPlayerState,
            playerSetCards: pokerServer.playerSetCards,
            playerRegistered: pokerServer.playerRegistered,
            dealCards: pokerServer.dealCards,
            playerReadiedUp: pokerServer.playerReadiedUp,
            playerCardsRearranged: pokerServer.playerCardsRearranged,
            gameStarted: pokerServer.gameStarted,
            deckShuffled: pokerServer.deckShuffled
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createGame
    };
})(window.DEVGUYS = window.DEVGUYS || {});