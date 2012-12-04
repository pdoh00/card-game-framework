(function (namespace) {
    var functionName = 'gameClientHandlers';

    //creates an object to handle all client handlers
    var createClientHandlers = function (options) {

        var gameClient = options.gameClient;
        var playerName = options.playerName;
        var playerPosition = options.playerPosition;
        var playerId = options.playerId;
        var register = function () {

            //take seat handler
            $('.take-seat').click(function () {
                //var promptName = prompt('Please enter your name.', '');
                //if (promptName !== null && promptName !== "") {
                //    sessionStorage.playerName = promptName;
                //    gameClient.takeSeat({ playerName: promptName });
                //} else {
                //    alert('You must enter your name to take a seat.  Try again.');
                //}
                gameClient.takeSeat({ playerName: playerName, playerId: playerId });
            });

            //ready-up handler
            $('.ready-up').click(function () {
                gameClient.playerReady({ playerId: playerId });

                //show that the player is ready
                $('.avatar-contents .position[data-position="' + playerPosition + '"]').parent().addClass('player-ready', 500);

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