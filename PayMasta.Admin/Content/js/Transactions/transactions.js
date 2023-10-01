var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
$("#trans-no-record").hide();
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);

    var startDate = new Date('01/01/2012');
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    // ToEndDate.setDate(ToEndDate.getDate() + 365);
    ToEndDate.setDate(ToEndDate.getDate());

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

    $("#btnFilter").click(function () {
        debugger
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
            FromDate = fromDateSelected;
            ToDate = toDateSelected;
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#transSearch', function () {
        searchParam = $("#transSearch").val();
        if (searchParam.length >= 3) {
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }
        else if (searchParam.length == 0) {
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }

    });

    $('body').on('click', 'span.view-employeeProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "Transactions/GetEmployeDetailByGuid/id=" + data;
    });

    $("#btnExportToCSV").click(function () {
        debugger
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", status);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/Transactions/ExportCsvReportForEmployees/",
            contentType: false,
            processData: false,
            data: formData,
            //beforeSend: function () {
            //    alert();
            //},
            success: function (response) {
                if (response != "") {
                    var bytes = new Uint8Array(response.FileContents);
                    var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = "Transactions.xlsx";
                    link.click();
                }
            }
        });
    });

});


function getTransactionList(id, searchText, pageNumber, pageSize, FromDate, ToDate) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);

    $.ajax({
        type: "POST",
        cache: false,
        url: "Transactions/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#transList_body').empty();

            let employerList = '';
            if (response.RstKey == 1) {
                $("#trans-no-record").hide();
                $(response.employeesListViewModel).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.EmployerName + '</td>';
                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '<td><span id="view-employeeProfile" class="view-employeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '</tr>';

                });
                $('#transList_body').append(employerList);
                page_number(TotalRowCount, pageNumber);
                
            } else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#trans-no-record").show();
                page_number(0, pageNumber);
            }
        }
    });

}

function page_number(count, currentPage) {
    $('#trans_pagination').pagination({
        total: count,
        current: currentPage,
        length: 10,
        size: 2,
        /**
         * [click description]
         * @param  {[object]} options = {
         *      current: options.current,
         *      length: options.length,
         *      total: options.total
         *  }
         * @param  {[object]} $target [description]
         * @return {[type]}         [description]
         */
        click: function (options, $target) {
            pageNumber = options.current;
            if (searchParam.length >= 3) {
                getTransactionList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
            else if (searchParam.length == 0) {
                getTransactionList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
        }
    });
}