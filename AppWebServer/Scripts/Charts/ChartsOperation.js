var funClock;
var myData = new Array([0, 0]);
var myChart = new Object();
var dateStart = new Date();
var servers = new Array();

$(document).ready(function () {

    myChart = new JSChart('graph', 'line');
    myChart.setTitle('Sum Server Work');
    myChart.setTitleColor('#8E8E8E');
    myChart.setTitleFontSize(11);
    myChart.setAxisNameX('');
    myChart.setAxisNameY('');
    myChart.setAxisColor('#C4C4C4');
    myChart.setAxisValuesColor('#343434');
    myChart.setAxisPaddingLeft(100);
    myChart.setAxisPaddingRight(120);
    myChart.setAxisPaddingTop(50);
    myChart.setAxisPaddingBottom(40);
    myChart.setAxisValuesNumberX(6);
    myChart.setGraphExtend(true);
    myChart.setGridColor('#c2c2c2');
    myChart.setLineWidth(4);
    myChart.setLineColor('#9F0505');
    myChart.setSize(906, 321);
    myChart.setBackgroundImage('chart_bg.jpg');
    $("#divContainerNumbers").hide();
    $("#txtNumberManually").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            (e.keyCode >= 35 && e.keyCode <= 40)) { return; }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('#chkAutomatic').change(function () {
        var returnVal = $(this).is(':checked');

        if (returnVal == false) {
            $("#divContainerNumbers").hide();
            $("#ulNumbers li.clsDelete").remove();
        }
        else
            $("#divContainerNumbers").show();

        $('#chkAutomatic').val($(this).is(':checked'));
    });
})

function addNumber() {
    if ($("#txtNumberManually").val().length > 0) {
        var li = $("<li class='clsDelete'>" + $("#txtNumberManually").val() + "</li>");
        $("#ulNumbers").append(li);
        $("#txtNumberManually").val("");
    }
    else
        alert("You have to write a number");
}


function StartProcess() {

    var pNumebers = new Array();
    var pAutomatic = $("#chkAutomatic").is(':checked')

    if (pAutomatic) {
        $("#ulNumbers li").each(function (e) {
            pNumebers.push(parseFloat($($("#ulNumbers li")[e]).html()));
        });
    }

    funClock = setTimeout(GetRegisteredServers(pAutomatic, pNumebers), 1000);
}

function GetRegisteredServers(pAutomatic, pNumebers) {

    var paramDate = new Object();
    paramDate.pAutomatic = pAutomatic;
    paramDate.pNumbers = pNumebers;
    paramDate.pDate = dateStart;


    $.ajax({
        url: "/Register/GetSumValues",
        type: 'POST',
        data: JSON.stringify(paramDate),
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            if (response.Message) {
                alert(response.Message);
            }
            else {
                $("#drpServers li.clsDelete").remove();
                if (response.RegisteredServers.length > 0) {

                    NewServerConected(response.RegisteredServers);
                    servers = response.RegisteredServers;
                    $("#lblRegisteredServers").html("The application has been conected by " + response.RegisteredServers.length + " server(s)");

                    $(response.RegisteredServers).each(function (e) {
                        var itm = $("<li class='clsDelete'>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + response.RegisteredServers[e].IdRegister + "</div>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + response.RegisteredServers[e].SumServerName + "</div>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + response.RegisteredServers[e].SumServerAddress + "</div>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + response.RegisteredServers[e].SumServerPort + "</div>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + GetNameActionRegister(response.RegisteredServers[e].ActionRegister) + "</div>" +
                                        "<div class='col-sm-2 col-lg-2 col-md-2'>" + GetNameActionSumServer(response.RegisteredServers[e].ActionSumServer) + "</div>" +
                                   "</li>");
                        $("#drpServers").append(itm);
                    });

                    myData.push([response.dateEnd, response.Value]);

                    myChart.setTooltip([response.dateEnd, "Val " + response.Value]);
                    myChart.setLabelX([response.dateEnd, response.dateEnd]);
                    myChart.setDataArray(myData);
                    myChart.draw();
                    $("#diResultServer").html("X (time): " + response.dateEnd + " Y (value): " + response.Value);
                }
                else
                    $("#lblRegisteredServers").html("The application has been conected by 0 server(s)");
            }
            funClock = setTimeout(GetRegisteredServers(pAutomatic, pNumebers), 1000);
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

function GetNameActionRegister(id) {
    switch (id) {
        case 1: return "Register"; break;
        case 2: return "Unregister"; break;
    }
}
function GetNameActionSumServer(id) {
    switch (id) {
        case 1: return "Conected"; break;
        case 2: return "Disconected"; break;
    }
}

function NewServerConected(tmpServ) {
    var newServTmp = new Array();
    if (servers.length < 1)
        newServTmp = tmpServ;
    else {
        $(tmpServ).each(function (e2) {
            var grp = $.grep(servers, function (e1) {
                return e1.IdRegister == tmpServ[e2].IdRegister;
            });
            if (grp < 1)
                newServTmp.push(tmpServ[e2]);
        });
    }

    $(newServTmp).each(function (e) {
        toastr8.info("A new server:(" + newServTmp[e].ServerName + ") has conected in the system!", "Information");
    });
}