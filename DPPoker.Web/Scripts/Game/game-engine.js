﻿/// <reference path="../Libs/jquery.signalR-0.5.3.js" />

//this object handles all client game interactions
(function (namespace) {
    var functionName = 'game';

    //creates a new client game object
    var createGame = function (options) {
        
        $.support.cors = options.supportsCors || true;
        $.connection.hub.url = options.hubUrl;
        $.connection.hub.logging = true;
        
        var pokerServer = $.connection.pokerServer;
        var tableId = options.tableId;
        var logger = options.logger;
        var playerId = options.playerId;
        var playerName = options.playerName;
        var jsonp = options.jsonp || false;

        //#region HUB GLOBAL CONNECTION HANDLERS

        //START
        $.connection.hub.start({ jsonp: jsonp }, function () {
            logger.logInfo('Connection started!');

            //init the table
            pokerServer.initializeTable(tableId);
        });
        //SENDING
        $.connection.hub.sending(function () {
            logger.logInfo('Establishing connection to server');
        });
        //RECEIVED
        $.connection.hub.received(function (data) {
            if (data.Result !== null && data.Error !== null && data.StackTrace !== null) {
                logger.logInfo('Received:' + JSON.stringify(data));
            } else {
                logger.logError('Received Unknown:' + JSON.stringify(data));
            }
        });
        //ERROR
        $.connection.hub.error(function (error) {
            logger.logError(error);
        });
        //STATE CHANGED
        $.connection.hub.stateChanged(function (change) {
            if (change.newState === $.signalR.connectionState.reconnecting) {
                logger.logInfo('Reconnecting');
            }
            else if (change.newState === $.signalR.connectionState.connected) {
                logger.logInfo('Connected');
            } else if (change.newState === $.signalR.connectionState.connecting) {
                logger.logInfo('Connecting');
            } else if (change.newState === $.signalR.connectionState.disconnected) {
                logger.logInfo('Disconnected');
            }
        });
        //RECONNECTED
        $.connection.hub.reconnected(function () {
            logger.logInfo('Reconnected');
        });

        //#endregion

        //#region PUBLIC HANDLERS
        
        //playerRegistered
        var playerRegisteredHandler;
        setPlayerRegisteredHandler = function (handler) {
            playerRegisteredHandler = handler;
        };
        pokerServer.playerRegistered = function (playerId, position) {
            playerRegisteredHandler(playerId, position);
        };
        //broadcastToConsole
        var broadcastToConsoleHandler;
        setBroadcastToConsoleHandler = function (handler) {
            broadcastToConsoleHandler = handler;
        };
        pokerServer.broadcastToConsole = function (message) {
            broadcastToConsoleHandler(message);
        };
        //dealCards
        var dealCardsHandler;
        setDealCardsHandler = function (handler) {
            dealCardsHandler = handler;
        };
        pokerServer.dealCards = function (cards) {
            dealCardsHandler(cards);
        };
        //playerTookSeat
        var playerTookSeatHandler;
        setPlayerTookSeatHandler = function (handler) {
            playerTookSeatHandler = handler;
        };
        pokerServer.playerTookSeat = function (name, position, avatar) {
            playerTookSeatHandler(name, position, avatar);
        };
        //playerLeftSeat
        var playerLeftSeatHandler;
        setPlayerLeftSeatHandler = function (handler) {
            playerLeftSeatHandler = handler;
        };
        pokerServer.playerLeftSeat = function (name, position) {
            playerLeftSeatHandler(name, position);
        };
        //setPlayerState
        var setTableStateHandler;
        initSetTableStateHandler = function (handler) {
            setTableStateHandler = handler;
        };
        pokerServer.setTableState = function (tableState) {
            setTableStateHandler(tableState);
        };
        //playerSetCards
        var playerSetCardsHandler;
        setPlayerSetCardsHandler = function (handler) {
            playerSetCardsHandler = handler;
        };
        pokerServer.playerSetCards = function (position) {
            playerSetCardsHandler(position);
        };
        //dealCards
        var dealCardsHandler;
        setDealCardsHandler = function (handler) {
            dealCardsHandler = handler;
        };
        pokerServer.dealCards = function (cards) {
            dealCardsHandler(cards);
        };
        //playerReadiedUp
        var playerReadiedUpHandler;
        setPlayerReadiedUpHandler = function (handler) {
            playerReadiedUpHandler = handler;
        };
        pokerServer.playerReadiedUp = function (position) {
            playerReadiedUpHandler(position);
        };
        //playerCardsRearranged
        var playerCardsRearrangedHandler;
        setPlayerCardsRearrangedHandler = function (handler) {
            playerCardsRearrangedHandler = handler;
        };
        pokerServer.playerCardsRearranged = function (position, cards) {
            playerCardsRearrangedHandler(position, cards);
        };
        //gameStarted
        var gameStartedHandler;
        setGameStartedHandler = function (handler) {
            gameStartedHandler = handler;
        };
        pokerServer.gameStarted = function (gameId, timestamp) {
            gameStartedHandler(gameId, timestamp);
        };
        //deckShuffled
        var deckShuffledHandler;
        setDeckShuffledHandler = function (handler) {
            deckShuffledHandler = handler;
        };
        pokerServer.deckShuffled = function () {
            deckShuffledHandler();
        };

        //#endregion

        //#region PRIVATE HANDLERS

        pokerServer.tableInitialized = function () {
            logger.logInfo('Table initialized');

            //request player positions
            pokerServer.getTableState(tableId);
        };

        //#endregion

        //#region PUBLIC METHODS

        //Seats the player at the table
        var takeSeat = function () {
            pokerServer.takeSeat(tableId, playerId, playerName);
        };
        //Indicates that the player is ready
        var playerReady = function () {
            pokerServer.playerReady(tableId, playerId);
        };
        //Will remove the player from the table and game
        var leaveGame = function () {
            pokerServer.leaveGame(tableId, playerId);
        };
        //Requests to get the table state
        var getTableState = function () {
            pokerServer.getTableState(tableId);
        };

        //#endregion

        //reveal public objects
        return {
            //FIELDS
            tableId: tableId,
            //METHODS
            takeSeat: takeSeat,
            playerReady: playerReady,
            leaveGame: leaveGame,
            getTableState: getTableState,
            //SERVER CALLBACK HANDLERS
            broadcastToConsole: setBroadcastToConsoleHandler,
            dealCards: setDealCardsHandler,
            playerTookSeat: setPlayerTookSeatHandler,
            playerLeftSeat: setPlayerLeftSeatHandler,
            setTableState: initSetTableStateHandler,
            playerSetCards: setPlayerSetCardsHandler,
            playerRegistered: setPlayerRegisteredHandler,
            dealCards: setDealCardsHandler,
            playerReadiedUp: setPlayerReadiedUpHandler,
            playerCardsRearranged: setPlayerCardsRearrangedHandler,
            gameStarted: setGameStartedHandler,
            deckShuffled: setDeckShuffledHandler
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createGame
    };
})(window.DEVGUYS = window.DEVGUYS || {});