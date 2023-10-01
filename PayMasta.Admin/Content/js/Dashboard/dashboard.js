
var fromDate = null;
var endDate = null;
var monthNumber = 0
$(document).ready(function () {
    //
    //$('#txtFromDate').datepicker();
    //$('#txtTodate').datepicker();
    var startDate = new Date('01/01/2012');
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    // ToEndDate.setDate(ToEndDate.getDate() + 365);
    ToEndDate.setDate(ToEndDate.getDate());

    $('#txtFromDate').datepicker({
        weekStart: 1,
        startDate: '01/01/2012',
        endDate: FromEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        // fromDate = selected.date.valueOf();
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#txtEnddate').datepicker('setStartDate', startDate);
    });
    $('#calender_from').on("click", function () {
        $('#txtFromDate').focus();
    });

    $('#txtEnddate').datepicker({
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        //toDate = FromEndDate.parse('dd-mm-yy') ;
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#txtFromDate').datepicker('setEndDate', FromEndDate);
    });
    $('#calender_to').on("click", function () {
        $('#txtEnddate').focus();
    });

    var userId = sessionStorage.getItem("User1");
    getDashboardData(userId, null, null, 0);

    $("#btnFilter").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();
        var isFromdateTrue = true;
        var isTodateTrue = true;
        if (fromDateSelected == '' || fromDateSelected == "" || fromDateSelected == null) {
            isFromdateTrue = false
            $("#fromdateError").show();
            $("#fromdateError").text("Please select from date");
        }
        else {
            isFromdateTrue = true
            $("#fromdateError").hide();
        }
        if (toDateSelected == '' || toDateSelected == "" || toDateSelected == null) {
            isTodateTrue = false;
            $("#TodateError").show();
            $("#TodateError").text("Please select to date");
        }
        else {
            isTodateTrue = true;
            $("#TodateError").hide();
        }
        if (isFromdateTrue == true && isTodateTrue == true) {

            getDashboardData(userId, fromDateSelected, toDateSelected, 0);
        }
    });

    $("#btnReset").click(function () {
        if (fromDate != "") {
            window.location.reload();
        }

    });


    $('#ddlMonth').change(function () {
        monthNumber = $('select#ddlMonth option:selected').val();
        var monthName = $('select#ddlMonth option:selected').text();
        getDashboardData(userId, null, null, monthNumber)
    });
});

function getDashboardData(userId, fromDate, toDate, monthNumber) {

    var formData = new FormData();
    formData.append("UserGuid", userId);
    formData.append("FromDate", fromDate);
    formData.append("ToDate", toDate);
    formData.append("Month", monthNumber);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.dashboard != null && response.RstKey == 1) {
                let value = response.dashboard;
                $("#total_employees").text(value.TotalEmployees)
                $("#total_users").text(value.TotalUser);
                $("#total_update_pro_req").text(value.TotalUpdateProfileRequest);
                $("#total_transactions").text(value.TotalTransactions);
                $("#total_withdraw_req").text(value.TotalWithdrawRequest);
                $("#total_queries").text(value.TotalQueries);
                $("#total_employer").text(value.TotalEmployer);
                $("#total_Earnings").text("₦ " + value.TotalCommisionEarning);
                $("#total_EWASent").text("₦ " + value.TotalEWASent);
                $("#total_EWAEarnings").text("₦ " + value.TotalEWACommisionEarned);
            }
            else {
                swal({
                    title: "oops!",
                    text: "Data Not Found !",
                    icon: "error",
                    button: "ohh no!"
                });
            }


        }
    });
}