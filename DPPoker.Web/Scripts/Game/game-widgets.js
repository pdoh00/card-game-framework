/// <reference path="../Libs/jquery-1.8.2.js" />
/// <reference path="game-engine.js" />

(function (namespace) {
    var functionName = 'gameWidgets';

    var createWidgets = function (options) {

        var gameClient = options.gameClient;
        var previousCardList = ['', '', '', '', '', '', '', '', '', '', '', '', ''];

        //set widgets on the game client to this
        gameClient.widgets = this;

        var initCardSortables = function () {
            $("#sortableSource1").sortable({
                connectWith: ".connectedSortable",
                delay: 150
            }).disableSelection();
            $("#sortableSource2").sortable({
                connectWith: ".connectedSortable",
                delay: 150
            }).disableSelection();
            $("#sortable1").sortable({
                connectWith: ".connectedSortable",
                delay: 150,
                update: function (event, ui) {
                    arrangeCards();
                },
                receive: function (event, ui) {
                    arrangeCards();
                }
            }).disableSelection();
            $("#sortable2").sortable({
                connectWith: ".connectedSortable",
                delay: 150,
                update: function (event, ui) {
                    arrangeCards();
                },
                receive: function (event, ui) {
                    arrangeCards();
                }
            }).disableSelection();
            $("#sortable3").sortable({
                connectWith: ".connectedSortable",
                delay: 150,
                update: function (event, ui) {
                    arrangeCards();
                },
                receive: function (event, ui) {
                    arrangeCards();
                }
            }).disableSelection();
        };

        var arrangeCards = function () {
            var currentCardList = getCards();
            var positionsArray = getPositionChanges(previousCardList, currentCardList);
            previousCardList = currentCardList;

            if (currentCardList.indexOf('') == -1) {
                $('.set-hand').show();
            } else {
                $('.set-hand').hide();
            }

            gameClient.rearrangeHand(positionsArray);
        };

        //reveal public objects
        return {
            //METHODS
            initCardSortables: initCardSortables
        };
    };

    //#region STATIC METHODS

    var getCards = function () {

        var retVal = [];

        //find cards
        var cards1 = $('#sortable1').children();
        var cards2 = $('#sortable2').children();
        var cards3 = $('#sortable3').children();

        for (var i = 0; i < 5; i++) {
            var item = cards1[i];
            if (item) {
                retVal.push($(item).attr('data-card'));
            } else {
                retVal.push('');
            }
        }

        for (var i = 0; i < 5; i++) {
            var item = cards2[i];
            if (item) {
                retVal.push($(item).attr('data-card'));
            } else {
                retVal.push('');
            }
        }

        for (var i = 0; i < 3; i++) {
            var item = cards3[i];
            if (item) {
                retVal.push($(item).attr('data-card'));
            } else {
                retVal.push('');
            }
        }

        return retVal;
    };
    var getPositionChanges = function (cardsA, cardsB) {
        var retVal = [];

        for (var i = 0; i < 13; i++) {

            //check if b has a card
            var hasCard = false;
            if (cardsB[i] !== '') {
                hasCard = true;
            }

            //check if card is different
            if (cardsA[i] == cardsB[i]) {
                hasCardChanged = false;
            } else {
                hasCardChanged = true;
            }

            retVal.push({ HasCard: hasCard, HasChanged: hasCardChanged });
        }

        return retVal;
    };
    var disableSortables = function () {
        $("#sortableSource1").sortable('disable');
        $("#sortableSource2").sortable('disable');
        $("#sortable1").sortable('disable');
        $("#sortable2").sortable('disable');
        $("#sortable3").sortable('disable');

        //hide my cards
        $('.my-cards').hide();
    };
    var enableSortables = function () {
        $("#sortableSource1").sortable('enable');
        $("#sortableSource2").sortable('enable');
        $("#sortable1").sortable('enable');
        $("#sortable2").sortable('enable');
        $("#sortable3").sortable('enable');
    };
    var clearCards = function () {
        $('#sortable1').children().remove();
        $('#sortable2').children().remove();
        $('#sortable3').children().remove();
        $("#sortableSource1").children().remove();
        $("#sortableSource2").children().remove();

        //repopulate default li's into lists
        for (var i = 0; i < 7; i++) {
            $("#sortableSource1").append("<li class='my-card-pos" + i + "'></li>")
        }
        for (var i = 7; i < 13; i++) {
            $("#sortableSource2").append("<li class='my-card-pos" + i + "'></li>")
        }
    };

    //#endregion

    //assign create for the object
    namespace[functionName] = {
        create: createWidgets,
        disableSortables: disableSortables,
        enableSortables: enableSortables,
        getCards: getCards,
        clearCards: clearCards
    };
})(window.DEVGUYS = window.DEVGUYS || {});