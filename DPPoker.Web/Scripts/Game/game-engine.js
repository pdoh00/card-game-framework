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
            if (data.Result !== null && data.Error !== null && data.StackTrace !== null) {
                logger.logInfo('Data received...' + JSON.stringify(data));
            }
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

        //#region PUBLIC HANDLERS
        
        //playerRegistered
        var playerRegisteredHandler;
        setPlayerRegisteredHandler = function (handler) {
            playerRegisteredHandler = handler;
        }
        pokerServer.playerRegistered = function (playerId, position) {
            playerRegisteredHandler(playerId, position);
        }
        //broadcastToConsole
        var broadcastToConsoleHandler;
        setBroadcastToConsoleHandler = function (handler) {
            broadcastToConsoleHandler = handler;
        }
        pokerServer.broadcastToConsole = function (message) {
            broadcastToConsoleHandler(message);
        }
        //dealCards
        var dealCardsHandler;
        setDealCardsHandler = function (handler) {
            dealCardsHandler = handler;
        }
        pokerServer.dealCards = function (cards) {
            dealCardsHandler(cards);
        }
        //playerTookSeat
        var playerTookSeatHandler;
        setPlayerTookSeatHandler = function (handler) {
            playerTookSeatHandler = handler;
        }
        pokerServer.playerTookSeat = function (name, position, avatar) {
            playerTookSeatHandler(name, position, avatar);
        }
        //playerLeftSeat
        var playerLeftSeatHandler;
        setPlayerLeftSeatHandler = function (handler) {
            playerLeftSeatHandler = handler;
        }
        pokerServer.playerLeftSeat = function (name, position) {
            playerLeftSeatHandler(name, position);
        }
        //setPlayerState
        var setPlayerStateHandler;
        setSetPlayerStateHandler = function (handler) {
            setPlayerStateHandler = handler;
        }
        pokerServer.setPlayerState = function (players) {
            setPlayerStateHandler(players);
        }
        //playerSetCards
        var playerSetCardsHandler;
        setPlayerSetCardsHandler = function (handler) {
            playerSetCardsHandler = handler;
        }
        pokerServer.playerSetCards = function (position) {
            playerSetCardsHandler(position);
        }
        //dealCards
        var dealCardsHandler;
        setDealCardsHandler = function (handler) {
            dealCardsHandler = handler;
        }
        pokerServer.dealCards = function (cards) {
            dealCardsHandler(cards);
        }
        //playerReadiedUp
        var playerReadiedUpHandler;
        setPlayerReadiedUpHandler = function (handler) {
            playerReadiedUpHandler = handler;
        }
        pokerServer.playerReadiedUp = function (position) {
            playerReadiedUpHandler(position);
        }
        //playerCardsRearranged
        var playerCardsRearrangedHandler;
        setPlayerCardsRearrangedHandler = function (handler) {
            playerCardsRearrangedHandler = handler;
        }
        pokerServer.playerCardsRearranged = function (position, cards) {
            playerCardsRearrangedHandler(position, cards);
        }
        //gameStarted
        var gameStartedHandler;
        setGameStartedHandler = function (handler) {
            gameStartedHandler = handler;
        }
        pokerServer.gameStarted = function (gameId, timestamp) {
            gameStartedHandler(gameId, timestamp);
        }
        //deckShuffled
        var deckShuffledHandler;
        setDeckShuffledHandler = function (handler) {
            deckShuffledHandler = handler;
        }
        pokerServer.deckShuffled = function () {
            deckShuffledHandler();
        }

        //#endregion

        //#region PRIVATE HANDLERS

        pokerServer.setTableId = function (id) {

            tableId = id;

            //request player positions
            pokerServer.getPlayerPositionsAtTable(tableId);
        };

        //#endregion

        //#region PUBLIC METHODS

        //Seats the player at the table
        var takeSeat = function (args) {
            pokerServer.takeSeat(tableId, args.playerName);
        };
        //Indicates that the player is ready
        var playerReady = function (args) {
            pokerServer.playerReady(tableId, args.playerId);
        };
        //Will remove the player from the table and game
        var leaveGame = function (args) {
            pokerServer.leaveGame(tableId, args.playerId);
        };
        //Requests to get the player positions at the table
        var getPlayerPositionsAtTable = function () {
            pokerServer.getPlayerPositionsAtTable(tableId);
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
            //SERVER CALLBACK HANDLERS
            broadcastToConsole: setBroadcastToConsoleHandler,
            dealCards: setDealCardsHandler,
            playerTookSeat: setPlayerTookSeatHandler,
            playerLeftSeat: setPlayerLeftSeatHandler,
            setPlayerState: setSetPlayerStateHandler,
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