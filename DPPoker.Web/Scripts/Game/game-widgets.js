(function (namespace) {
    var functionName = 'gameWidgets';

    var createWidgets = function () {

        var initCardSortables = function () {

            $("#sortableSource1").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
            $("#sortableSource2").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
            $("#sortable1").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
            $("#sortable2").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
            $("#sortable3").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
        };

        //reveal public objects
        return {
            //METHODS
            initCardSortables: initCardSortables
        };
    };

    //assign create for the object
    namespace[functionName] = {
        create: createWidgets
    };
})(window.DEVGUYS = window.DEVGUYS || {});