
function StarSocket() {
    $.ajax({
        url: "/Operations/StartSocket",
        type: 'POST',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (response) { 
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}