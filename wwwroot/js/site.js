$('#btnAddRuleTable').click(function () {
    $('#AddRuleDivider').removeAttr('hidden');
});


$("#leftEquality").click(function () {
    if ($(this).val() === '3') {
        $("#leftEquality").prop('value', '1');
        $("#leftEquality").html('&lt;');
    }
    else if ($(this).val() === '1') {
        $("#leftEquality").prop('value', '3');
        $("#leftEquality").html('&le;');
    }
});

$("#rightEquality").click(function () {
    if ($(this).val() === '4') {
        $("#rightEquality").prop('value', '2');
        $("#rightEquality").html('&lt;');
    }
    else if ($(this).val() === '2') {
        $("#rightEquality").prop('value', '4');
        $("#rightEquality").html('&le;');
    }
});



$("#btnAddRule").click(function () {
    $.ajax({
        url: '/Condition/AddRule',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            Name: 'C1',
            LowerBound: $("#inputLowerBound").val(),
            UpperBound: $("#inputUpperBound").val(),
            LeftEquality: $("#leftEquality").val(),
            RightEquality: $("#rightEquality").val(),
            Result: $("#inputResult").val(),

        }),
        success: function (response) {
            $('#RuleTable').html(response);
        },
        error: function (xhr, status, error) {
            $('#RuleTable').html('<br>Error Occured While Loading Rule Table (Call Yusuf)</br>');
        }
    });
});




