(function (namespace) {
    var functionName = 'gameServerCallbacks';

    //creates an object to handle all server 2 clients callbacks
    var createGameServerCallbacks = function (options) {

        var gameClient = options.gameClient;
        var logger = options.logger;
        var register = function () {

            //any server broadcasted messages.  Usually debug info
            gameClient.broadcastToConsole(function (message) {
                logger.logInfo('-SERVER-' + message);
            });
            gameClient.dealCards(function (cards) {
                logger.logInfo(JSON.stringify(cards));

                $.each(cards, function (index, item) {

                    var element = $('.my-cards .my-card-pos' + index);

                    //clear out classes
                    element.attr('class', '');

                    //add classes back
                    element.addClass('drag');
                    element.addClass('my-card-pos' + index);

                    //set card class
                    $('.my-cards .my-card-pos' + index).addClass('_' + item);
                });
            });
            gameClient.playerTookSeat(function (name, position, avatar) {
                if (name !== sessionStorage.playerName) {

                    //update the seat and show the player info
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.name').text(name);
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.points').show();
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').removeClass('empty-avatar');
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.picture').attr('src', 'content/images/default-avatar-image.png');
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.picture').show();
                }
            });
            gameClient.playerLeftSeat(function (name, position) {
                if (name !== sessionStorage.playerName) {
                    //remove player
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.name').text('Empty');
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').children('.points').hide();
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.picture').hide();
                    $('.avatar-contents .position[data-position="' + position + '"]').parent().children('.name-container').addClass('empty-avatar');
                }
            });
            gameClient.setPlayerState(function (players) {

                var playersCount = players.length;


                var playerId = sessionStorage.playerId;
                var playerPosition = sessionStorage.playerPosition;
                if (playerId) {

                    for (var i = 1; i < 5; i++) {
                        if (i != playerPosition) {
                            $('.avatar-contents .position[data-position="' + i + '"]').parent().children('.name-container').children('.name').text('Empty');
                            $('.avatar-contents .position[data-position="' + i + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                            $('.avatar-contents .position[data-position="' + i + '"]').parent().children('.name-container').children('.points').hide();
                            $('.avatar-contents .position[data-position="' + i + '"]').parent().children('.picture').hide();
                            $('.avatar-contents .position[data-position="' + i + '"]').parent().children('.name-container').addClass('empty-avatar');
                        }
                    }

                    $(players).each(function (index, item) {
                        //update the seat and show the player info
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.name').text(players[index]);
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').show();
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').removeClass('empty-avatar');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.picture').attr('src', 'content/images/default-avatar-image.png');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.picture').show();
                    });
                } else {
                    //set remaining positions
                    for (var i = 1; i < players.length; i++) {

                        var spot = '';
                        switch (i) {
                            case 0:
                                $('.left .position').text('Position ' + (i + 1));
                                $('.left .position').attr('data-position', i);
                                spot = 'left';
                                break;
                            case 1:
                                $('.top .position').text('Position ' + (i + 1));
                                $('.top .position').attr('data-position', i);
                                spot = 'top';
                                break;
                            case 2:
                                $('.right .position').text('Position ' + (i + 1));
                                $('.right .position').attr('data-position', i);
                                spot = 'right';
                                break;
                            case 3:
                                $('.bottom .position').text('Position ' + (i + 1));
                                $('.bottom .position').attr('data-position', i);
                                $('.leave-game').hide();
                                spot = 'bottom';
                                break;
                            default:
                        }

                        $('.' + spot + ' .name-container .name').text(players[i]);
                        $('.' + spot + ' .name-container .points').text('(' + 0 + ')');
                        $('.' + spot + ' .name-container .points').show();
                        $('.' + spot + ' .name-container').removeClass('empty-avatar');
                        $('.' + spot + ' .picture').attr('src', 'content/images/default-avatar-image.png');
                        $('.' + spot + ' .picture').show();
                    }
                }
            });
            gameClient.playerSetCards(function (position) {
                //show that this player has set their cards
            });
            gameClient.playerRegistered(function (playerId, position) {
                //store player id
                sessionStorage.setItem('playerId', playerId);
                sessionStorage.setItem('playerPosition', position);

                //update ui
                var playerName = sessionStorage.getItem('playerName');
                updatePlayerPosition(playerName, position);

                //update remaining positions
                gameClient.getPlayerPositionsAtTable();
            });
            gameClient.dealCards(function (cards) {
                //display my cards
                $.each(cards, function (index, item) {

                    var element = $('.my-cards .my-card-pos' + index);

                    //clear out classes
                    element.attr('class', '');

                    //add classes back
                    element.addClass('drag');
                    element.addClass('my-card-pos' + index);

                    //set card class
                    $('.my-cards .my-card-pos' + index).addClass('_' + item);
                });
            });
            gameClient.playerReadiedUp(function (position) {
                //show that the player is ready
                $('.avatar-contents .position[data-position="' + position + '"]').parent().addClass('player-ready', 500);
            });
            gameClient.playerCardsRearranged(function (position, cards) {
                //update the cards at that position
            });
            gameClient.gameStarted(function (gameId, timestamp) {
                //alert that the game has started
                //start timer for the 1st round
            });
            gameClient.deckShuffled(function () {
                //play deck shuffle sound & animation
            });
        };

        var updatePlayerPosition = function (playerName, position) {
            //hide take seat button
            $('.take-seat').hide();

            //show leave table button
            $('.leave-game').show();

            //show ready up button
            $('.ready-up').show();

            //update user box
            $('.bottom .name-container .name').text(playerName);
            $('.bottom .name-container .points').text('(' + 0 + ')');
            $('.bottom .name-container .points').show();
            $('.bottom .name-container').removeClass('empty-avatar');
            $('.bottom .position').text('Position ' + position);
            $('.bottom .position').attr('data-position', position);
            $('.bottom .picture').attr('src', 'content/images/default-avatar-image.png');
            $('.bottom .picture').show();

            //set remaining positions
            for (var i = 1; i < 4; i++) {

                var pos = position;

                if (pos > 3) {
                    pos = position - 3;
                }

                switch (i) {
                    case 0:
                        $('.left .position').text('Position ' + pos);
                        $('.left .position').attr('data-position', pos);
                        break;
                    case 1:
                        $('.top .position').text('Position ' + pos);
                        $('.top .position').attr('data-position', pos);
                        break;
                    case 2:
                        $('.right .position').text('Position ' + pos);
                        $('.right .position').attr('data-position', pos);
                        break;
                    default:
                }
            }
        };

        //reveal public objects
        return {
            //METHODS
            register: register
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createGameServerCallbacks
    };
})(window.DEVGUYS = window.DEVGUYS || {});