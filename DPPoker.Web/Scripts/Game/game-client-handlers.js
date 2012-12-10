(function (namespace) {
    var functionName = 'gameClientHandlers';

    //creates an object to handle all client handlers
    var createClientHandlers = function (options) {

        var gameClient = options.gameClient;
        var playerName = options.playerName;
        var playerId = options.playerId;
        var register = function () {

            //take seat handler
            $('.take-seat').click(function () {
                gameClient.takeSeat({ playerName: playerName, playerId: playerId });
            });

            //ready-up handler
            $('.ready-up').click(function () {

                var playerPosition = parseInt(sessionStorage.playerPosition);
                gameClient.playerReady({ playerId: playerId });

                //show that the player is ready
                $('.avatar-contents .position[data-position="' + playerPosition + '"]').parent().addClass('player-ready');

                //hide ready up button
                $('.ready-up').hide();
            });

            //leave table handler
            $('.leave-game').click(function () {

                gameClient.leaveGame({ playerId: playerId });

                //clear out session storage
                sessionStorage.clear();

                //show take seat button
                $('.take-seat').show();

                //hide leave table button
                $('.leave-game').hide();

                //hide ready up button
                $('.ready-up').hide();

                //update user box
                $('.bottom .name-container .name').text('Empty');
                $('.bottom .name-container .points').text('(' + 0 + ')');
                $('.bottom .name-container .points').hide();
                $('.bottom .picture').hide();
                $('.bottom .name-container').addClass('empty-avatar');
            });

            //set cards handler
            $('.set-hand').click(function () {

                $('.set-hand').hide();
                gameClient.widgets.disableSortables();

                gameClient.setHand({ tableId: gameClient.tableId, gameId: gameClient.gameId, playerId: playerId, cards: gameClient.widgets.getCards() });
            });
        };

        //reveal public objects
        return {
            //METHODS
            register: register
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createClientHandlers
    };
})(window.DEVGUYS = window.DEVGUYS || {});