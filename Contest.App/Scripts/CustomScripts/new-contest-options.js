$(function () {
    $('#datetimepicker').datetimepicker({
        locale: 'en'
    });

    $('#multiple-winners').click(function () {
        var rewardType = $('#reward-type');
        var multipleWinnerDiv = $('<div>')
            .attr({ id: 'participants-section' })
            .addClass('form-group')
            .insertAfter(rewardType);

        $('<label>')
            .attr({ id: 'participants-number-label' })
            .addClass('col-md-2 control-label')
            .text('Winers number:')
            .appendTo(multipleWinnerDiv);

        var inputDiv = $('<div>')
            .addClass('col-md-10')
            .appendTo(multipleWinnerDiv);

        $('<input/>')
            .attr({ type: 'number', min: 2, max: 10, name: 'participants', autofocus: true, id: 'participants-input' })
            .addClass('form-control')
            .appendTo(inputDiv);
    });

    $('#single-winner').click(function () {
        $('#participants-section').remove();
        $('#participants-number-label').remove();
        $('#participants-input').remove();
    });

    $('#voting-close').click(function () {
        $.get('/Users/GetAllUsers',
            function (data) {
                if (data) {
                    var rewardType = $('#voting-type');
                    var manyVotersDiv = $('<div>')
                        .attr({ id: 'voters-section' })
                        .addClass('form-group')
                        .insertAfter(rewardType);

                    $('<label>')
                        .attr({ id: 'participants-label' })
                        .addClass('col-md-2 control-label')
                        .text('Participants:')
                        .appendTo(manyVotersDiv);

                    var inputDiv = $('<div>')
                        .addClass('col-md-10')
                        .appendTo(manyVotersDiv);

                    var votersUl = $('<ul>')
                        .addClass('nav nav-pills')
                        .appendTo(inputDiv);
                    data.forEach(function (user) {
                        var list = $('<li>').addClass('block-item');
                        $('<p>')
                            .text(user.Username)
                            .appendTo(list);

                        //$('<img>')
                        //    .attr('src', user.ProfileImage)
                        //    .appendTo(list);

                        list.appendTo(votersUl);
                    });
                }
            });

    });

    $('#voting-open').click(function () {
        $('#voters-section').remove();
    });
});