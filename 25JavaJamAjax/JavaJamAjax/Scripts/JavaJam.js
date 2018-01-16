// JavaJam.js

// Convert links (<a...>) to ajax requests
$('#homeLink').click(function (event) {
    event.preventDefault();
    var url = $(this).attr('href');
    $('#content').load(url);
});

$('#menuLink').click(function (event) {
    event.preventDefault();
    var url = $(this).attr('href');
    $('#content').load(url);
});

$('#musicLink').click(function (event) {
    event.preventDefault();
    var url = $(this).attr('href');
    $('#content').load(url, function () {
        //$.getScript("/Scripts/music-lab1.js");
        $.getScript("/Scripts/music-lab2.js");
    });
});

$('#jobsLink').click(function (event) {
    event.preventDefault();
    var url = $(this).attr('href');
    //$('#content').load(url);

    // Using the core low level $.ajax() method

    $.ajax({
        // The URL for the request
        url: url,

        // Whether this is a POST or GET request
        type: "GET",

        // Code to run if the request succeeds;
        // the response is passed to the function
        success: function (content) {
            $("#content").html(content);
            // Must load jquery.validate. The time dalay (100ms) is needed, but I don't know why 
            setTimeout(function () {
                $.getScript("/Scripts/jquery.validate.js");
                $.getScript("/Scripts/jquery.validate.unobtrusive.js");
            }, 100);  
        },

        // Code to run if the request fails; the raw request and
        // status codes are passed to the function
        error: function (xhr, status, errorThrown) {
            alert("Sorry, there was a problem!");
            console.log("Error: " + errorThrown);
            console.log("Status: " + status);
            console.dir(xhr);
        },

        // Code to run regardless of success or failure
        //complete: function (xhr, status) {
        //    //alert("Ajax request is complete!");
            
        //}
    });
});

