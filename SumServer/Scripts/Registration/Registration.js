
function ServerRegistration()
{  
    $.ajax({
        url: "/Register/DoServerRegistration",
        type: 'POST', 
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            var register = response.Register;
            //$("#btnUnRegister").show();
            //($("#btnRegister")).toggle();
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}


function ServerUnRegistration() {
    $.ajax({
        url: "/Register/DoServerUnRegistration",
        type: 'POST',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            var register = response.Register;
            // $("#btnUnRegister").toggle();
            //($("#btnRegister")).show();
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}