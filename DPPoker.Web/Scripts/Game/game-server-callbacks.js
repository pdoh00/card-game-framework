/// <reference path="../Libs/jquery-1.8.2.js" />

(function (namespace) {
    var functionName = 'gameServerCallbacks';

    //creates an object to handle all server 2 clients callbacks
    var createGameServerCallbacks = function (options) {

        var playerId = options.playerId;
        var playerName = options.playerName;
        var gameClient = options.gameClient;
        var logger = options.logger;
        var tableId = options.tableId;

        var register = function () {

            //any server broadcasted messages.  Usually debug info
            gameClient.broadcastToConsole(function (message) {
                logger.logInfo('-SERVER-' + message);
            });
            gameClient.playerTookSeat(function (name, position, avatar) {
                gameClient.getTableState(tableId);
            });
            gameClient.playerLeftSeat(function (name, position) {
                gameClient.getTableState(tableId);
            });
            gameClient.setTableState(function (tableState) {

                var playerPosition = parseInt(sessionStorage.playerPosition);
                var playersCount = 0;

                //readjust position indexes
                adjustPositionIndexes();

                $(tableState.PlayerDetails).each(function (index, item) {

                    //only if the player has a name
                    if (item.PlayerName) {

                        //update the seat and show the player info
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.name').text(item.PlayerName);
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').show();
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').removeClass('empty-avatar');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.picture').attr('src', 'content/images/default-avatar-image.png');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.picture').show();

                        playersCount++;
                    } else {

                        //remove player
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.name').text('Empty');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').text('(' + 0 + ')');
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').children('.points').hide();
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.picture').hide();
                        $('.avatar-contents .position[data-position="' + index + '"]').parent().children('.name-container').addClass('empty-avatar');
                    }
                });

                if (playerPosition !== -1) {

                    //hide take seat button
                    $('.take-seat').hide();

                    //show leave table button
                    $('.leave-game').show();

                    //show ready up button
                    $('.ready-up').show();
                } else {

                    //if the table is full dont show the button
                    if (playersCount < 4) {
                        //hide take seat button
                        $('.take-seat').show();
                    }

                    //show leave table button
                    $('.leave-game').hide();

                    //show ready up button
                    $('.ready-up').hide();
                }

            });
            gameClient.playerSetCards(function (position) {
                //show that this player has set their cards
                var element = $('.small-card-set[data-position="' + position + '"]');
                for (var i = 0; i < 13; i++) {
                    var slot = $(element).find('.sm-card-pos' + i);
                    slot.attr('class', '');
                    slot.addClass('small-card-ready');
                }

            });
            gameClient.playerRegistered(function (playerId, position) {
                //store player id
                sessionStorage.playerPosition = position;

                //update remaining positions
                gameClient.getTableState(tableId);
            });
            gameClient.dealCards(function (cards) {
                logger.logInfo(JSON.stringify(cards));

                //show my cards
                $('.my-cards').show();

                //clear out all cards
                gameClient.widgets.clearCards();

                //enable sortables widget
                gameClient.widgets.enableSortables();

                //display my cards
                $.each(cards, function (index, item) {

                    var element = $('.my-cards .my-card-pos' + index);

                    //clear out classes
                    element.attr('class', '');

                    //add classes back
                    element.addClass('drag');
                    element.addClass('my-card-pos' + index);

                    //add card to data-card
                    $('.my-cards .my-card-pos' + index).attr('data-card', item);

                    //set card class
                    $('.my-cards .my-card-pos' + index).addClass('_' + item);
                });
            });
            gameClient.playerReadiedUp(function (position) {
                //show that the player is ready
                $('.avatar-contents .position[data-position="' + position + '"]').parent().addClass('player-ready');
            });
            gameClient.playerCardsRearranged(function (position, cards) {

                if (position === parseInt(sessionStorage.playerPosition)) {
                    return;
                }

                //update the cards
                var element = $('.small-card-set[data-position="' + position + '"]');
                var slotChangedA;
                var slotChangedB;

                for (var i = 0; i < 13; i++) {
                    var slot = $(element).find('.sm-card-pos' + i);
                    var hasCard = cards[i].HasCard;
                    var hasChanged = cards[i].HasChanged;

                    if (!hasCard) {
                        slot.removeClass('small-card-populated');
                        slot.addClass('small-card-empty');
                    } else {
                        slot.addClass('small-card-populated');
                        slot.removeClass('small-card-empty');
                    }

                    //set which slots have changed for animaiton
                    if (hasChanged) {
                        if (slotChangedA) {
                            slotChangedB = slot;
                        } else {
                            slotChangedA = slot;
                        }
                    }
                }

                //fire animation for slot A
                $(slotChangedA).removeClass("small-card-populated", 125, function () {
                    $(slotChangedA).addClass("small-card-empty", 125, function () {
                        $(slotChangedA).removeClass("small-card-empty", 125, function () {
                            $(slotChangedA).addClass("small-card-populated", 125, function () { });
                        });
                    });
                });

                //fire animaiton for slot B
                $(slotChangedB).removeClass("small-card-populated", 125, function () {
                    $(slotChangedB).addClass("small-card-empty", 125, function () {
                        $(slotChangedB).removeClass("small-card-empty", 125, function () {
                            $(slotChangedB).addClass("small-card-populated", 125, function () { });
                        });
                    });
                });

                return;
            });
            gameClient.gameStarted(function (gameId, timestamp) {
                //alert that the game has started
                alert('The game#' + gameId + ' has started');
                gameClient.gameId = gameId;

                //remove player ready indicators
                for (var i = 0; i < 4; i++) {
                    $('.avatar-contents .position[data-position="' + i + '"]').parent().removeClass('player-ready');
                }
            });
            gameClient.deckShuffled(function () {
                //play deck shuffle sound & animation
            });
        };
        var adjustPositionIndexes = function () {

            var positionArray = ['bottom', 'left', 'top', 'right'];
            var currentPosition = parseInt(sessionStorage.playerPosition);
            var adjustedPosition;

            for (var i = 0; i < 4; i++) {

                if (currentPosition > 3) {
                    currentPosition = (currentPosition - 4);
                }
                if (currentPosition === -1) {
                    currentPosition = 0;
                }

                adjustedPosition = currentPosition;

                switch (positionArray[i]) {
                    case 'bottom':
                        $('.bottom .position').text('Position ' + (adjustedPosition + 1));
                        $('.bottom .position').attr('data-position', adjustedPosition);
                        break;
                    case 'left':
                        $('.left .position').text('Position ' + (adjustedPosition + 1));
                        $('.left .position').attr('data-position', adjustedPosition);
                        $('.leftSet > .small-card-set').attr('data-position', adjustedPosition);
                        break;
                    case 'top':
                        $('.top .position').text('Position ' + (adjustedPosition + 1));
                        $('.top .position').attr('data-position', adjustedPosition);
                        $('.topSet > .small-card-set').attr('data-position', adjustedPosition);
                        break;
                    case 'right':
                        $('.right .position').text('Position ' + (adjustedPosition + 1));
                        $('.right .position').attr('data-position', adjustedPosition);
                        $('.rightSet > .small-card-set').attr('data-position', adjustedPosition);
                        break;
                }

                currentPosition++;
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