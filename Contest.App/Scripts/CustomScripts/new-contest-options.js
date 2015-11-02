$(function () {
    $(document).on('click', '#endTime', function (e) {
        var date = new Date();
        var tomorrow = new Date(date.getTime() + 24 * 60 * 60 * 1000);
        $(this).datetimepicker({
            minDate: tomorrow
        });
    });

    $(document).on('click', '#deadline-by-participants', function (e) {
        $('#deadline-section').remove();
        $('#deadline-time-label').remove();
        $('#deadline-input').remove();

        var deadlineType = $('#deadline-type');
        var participantsDeadlineDiv = $('<div>')
            .attr({ id: 'deadline-section' })
            .addClass('form-group')
            .insertAfter(deadlineType);

        $('<label>')
            .attr({ id: 'deadline-participants-label' })
            .addClass('col-md-2 control-label')
            .text('Number of participants:')
            .appendTo(participantsDeadlineDiv);

        var inputDiv = $('<div>')
            .addClass('col-md-10')
            .appendTo(participantsDeadlineDiv);

        $('<input/>')
            .attr({ type: 'number', name: 'participantsNumberDeadline', autofocus: true, id: 'deadline-input' })
            .addClass('form-control')
            .appendTo(inputDiv);
    });

    $(document).on('click', '#deadline-by-time', function (e) {
        $('#deadline-section').remove();
        $('#deadline-participants-label').remove();
        $('#deadline-input').remove();

        var deadlineType = $('#deadline-type');
        var timeDeadlineDiv = $('<div>')
            .attr({ id: 'deadline-section' })
            .addClass('form-group')
            .insertAfter(deadlineType);

        $('<label>')
           .attr({ id: 'deadline-time-label' })
           .addClass('col-md-2 control-label')
           .text('End Date:')
           .appendTo(timeDeadlineDiv);

        var inputDiv = $('<div>')
           .attr({ id: "endTime" })
           .addClass('col-md-10 input-group date date-input')
           .appendTo(timeDeadlineDiv);

        $('<input/>')
            .attr({ type: 'text', name: 'deadline', autofocus: true, id: 'deadline-input' })
            .addClass('form-control')
            .appendTo(inputDiv);

        var iconSpan = $('<span>').addClass("input-group-addon pull-left datepicker-icon");
        $('<span>')
            .addClass('glyphicon glyphicon-calendar')
            .appendTo(iconSpan);
        iconSpan.appendTo(inputDiv);
    });

    //$('#multiple-winners').click(function () {
    //    var rewardType = $('#reward-type');
    //    var multipleWinnerDiv = $('<div>')
    //        .attr({ id: 'participants-section' })
    //        .addClass('form-group')
    //        .insertAfter(rewardType);

    //    $('<label>')
    //        .attr({ id: 'participants-number-label' })
    //        .addClass('col-md-2 control-label')
    //        .text('Winers number:')
    //        .appendTo(multipleWinnerDiv);

    //    var inputDiv = $('<div>')
    //        .addClass('col-md-10')
    //        .appendTo(multipleWinnerDiv);

    //    $('<input/>')
    //        .attr({ type: 'number', min: 2, max: 10, name: 'participants', autofocus: true, id: 'participants-input' })
    //        .addClass('form-control')
    //        .appendTo(inputDiv);
    //});

    //$('#single-winner').click(function () {
    //    $('#participants-section').remove();
    //    $('#participants-number-label').remove();
    //    $('#participants-input').remove();
    //});

    //$('#voting-close').click(function () {


    //    $.get('/Users/GetAllUsers',
    //        function (data) {
    //            if (data) {
    //                var rewardType = $('#voting-type');
    //                var manyVotersDiv = $('<div>')
    //                    .attr({ id: 'voters-section' })
    //                    .addClass('form-group')
    //                    .insertAfter(rewardType);

    //                $('<label>')
    //                    .attr({ id: 'participants-label' })
    //                    .addClass('col-md-2 control-label')
    //                    .text('Participants:')
    //                    .appendTo(manyVotersDiv);

    //                var inputDiv = $('<div>')
    //                    .addClass('col-md-10')
    //                    .appendTo(manyVotersDiv);




    //                var votersUl = $('<ul>')
    //                    .addClass('nav nav-pills')
    //                    .appendTo(inputDiv);
    //                data.forEach(function (user) {
    //                    var list = $('<li>').addClass('block-item');
    //                    $('<p>')
    //                        .text(user.Username)
    //                        .appendTo(list);

    //                    //$('<img>')
    //                    //    .attr('src', user.ProfileImage)
    //                    //    .appendTo(list);

    //                    list.appendTo(votersUl);
    //                });
    //            }
    //        });

    //});

    //$('#voting-open').click(function () {
    //    $('#voters-section').remove();
    //});

    ////

    //$(document).on('click', '#btnRight', function (e) {
    //    var selectedOpts = $('#lstBox1 option:selected');
    //    if (selectedOpts.length === 0) {
    //        alert("Nothing to move.");
    //        e.preventDefault();
    //    }

    //    $('#lstBox2').append($(selectedOpts).clone());
    //    $(selectedOpts).remove();
    //    e.preventDefault();
    //});

    //$(document).on('click', '#btnLeft', function (e) {
    //    var selectedOpts = $('#lstBox2 option:selected');
    //    if (selectedOpts.length === 0) {
    //        alert("Nothing to move.");
    //        e.preventDefault();
    //    }

    //    $('#lstBox1').append($(selectedOpts).clone());
    //    $(selectedOpts).remove();
    //    e.preventDefault();
    //});

    //function fillCategories() {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Users/GetAllUsers"
    //    })
    //        .done(function (data) {
    //            $('#categories').html(data);
    //        });
    //};
    //fillCategories();

    ////



});