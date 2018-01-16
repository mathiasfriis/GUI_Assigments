// Music-lab2.js
var monthName = [' ', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

// Get all upcomming music arrangements
$.getJSON("/api/Musics", null, function (musics, textStatus, jqXHP) {
    var i;
    var htmlStr = "";
    for (i = 0; i < musics.length; i++) {
        htmlStr += '<h2>' + monthName[musics[i].Month] + '</h2>' +
        '<div class="row">' +
          '<div class="col-md-2 col-md-offset-2">' +
            '<a class="thumbLink" data-modal="' + musics[i].ImageUrl + 'Modal" href="/Images/' + musics[i].ImageUrl + '.jpg">' +
            '<img src="/Images/' + musics[i].ThumbNailUrl + '.jpg" alt="' + musics[i].Name + '" class="thumbnail" style="width:80px;height:80px">' +
            '</a>' +
          '</div>' +
          '<div class="col-md-6 "> <P>' + musics[i].Description + '</p> ' +
          '</div> ' +
        '</div>' +
        '<div id="' + musics[i].ImageUrl + 'Modal" class="modal fade" role="dialog">' +
        '   <div class="modal-dialog">' +
        '      <div class="modal-content">' +
        '         <div class="modal-header">' +
        '            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
        '            <h4 class="modal-title">' + musics[i].Name + '</h4>' +
        '         </div>' +
        '         <div class="modal-body">' +
        '            <img src="/Images/' + musics[i].ImageUrl + '.jpg" height="275" width="275" alt="' + musics[i].Name + '" />' +
        '         </div>' +
        '      </div>' +
        '    </div>' +
        ' </div>'
        ;
    }
    //console.log(htmlStr);
    $("#musicArrangements").html(htmlStr);

    //$('.modal').each(function () {
    //    alert($(this).attr('id'));
    //});

    $('.thumbLink').on('click', function (event) {
        event.preventDefault();
        var id = "#" + $(this).attr('data-modal');
        $(id).modal('show');
    });
});