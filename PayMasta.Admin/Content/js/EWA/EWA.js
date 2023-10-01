var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
var status = -1;
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    $("#noRecordFound").hide();
    UserGuid = sessionStorage.getItem("User1");
    getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);

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
            debugger
            FromDate = fromDateSelected;
            ToDate = toDateSelected;
            //getEmployeeEWAList(UserGuid, pageNumber, pageSize, searchParam, fromDateSelected, toDateSelected);
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#employeeSearch', function () {
        searchParam = $("#employeeSearch").val();
        if (searchParam.length >= 3) {
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        }
        else if (searchParam.length == 0) {
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        }

    });

    $('#ddlStatus').change(function () {
        status = $('select#ddlStatus option:selected').val();
        var monthName = $('select#ddlStatus option:selected').text();
        getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        // getEmployeeEWAList(userId, null, null, monthNumber)
    });

    $('body').on('click', 'span.view-user-profile', function (e) {
        e.preventDefault;
        debugger
        var data = $(this).data('val')

        window.location.href = "/Withdrawals/GetEmployeeEarningDetailByUserGuid/id=" + data;
    });
    $("#btnExportToCSV").click(function () {
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
            url: "/Withdrawals/ExportCsvReportForEmployees/",
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
                    link.download = "EWARequests.xlsx";
                    link.click();
                }
            }
        });
    });
});


function getEmployeeEWAList(id, searchText, pageNumber, pageSize, FromDate, ToDate, status) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("Status", status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "Withdrawals/GetEmployeesEWARequestList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#userList').empty();
            let employerList = '';
            $("#loader1").hide();
            if (response.RstKey == 1) {
                $("#noRecordFound").hide();
                $(response.accessAmountViewModels).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    var created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.EmployerName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    employerList += '<td>' + '₦' + value.AccessAmount + '</td>';
                    if (value.IsEwaApprovalAccess == true && (value.StatusId == 0 || value.StatusId == 2)) {
                        employerList += '<td style="color:#32A4FF">' + value.Status + '</td>';
                    }
                    else if (value.IsEwaApprovalAccess == true && value.StatusId == 1) {
                        employerList += '<td style="color:green">' + value.Status + '</td>';

                    } else if (value.IsEwaApprovalAccess == true && value.StatusId == 3) {
                        employerList += '<td style="color:red">' + value.Status + '</td>';
                    }
                    else if (value.IsEwaApprovalAccess == true && value.StatusId == 6) {
                        employerList += '<td style="color:orange">' + value.Status + '</td>';
                    } else {
                        employerList += '<td style="color:orange">N/A</td>';
                    }

                    if (value.AdminStatusId == 0 || value.AdminStatusId == 2 || value.AdminStatusId == null) {
                        employerList += '<td style="color:#32A4FF">' + value.AdminStatus + '</td>';
                    }
                    else if (value.AdminStatusId == 1) {
                        employerList += '<td style="color:green">' + value.AdminStatus + '</td>';

                    } else if (value.AdminStatusId == 3) {
                        employerList += '<td style="color:red">' + value.AdminStatus + '</td>';
                    }
                    else if (value.AdminStatusId == 6) {
                        employerList += '<td style="color:orange">' + value.AdminStatus + '</td>';
                    }
                    if (value.IsD2CUser === "D2CUser") {
                        employerList += '<td style="color:red">' + value.IsD2CUser + '</td>';
                    } else {
                        employerList += '<td style="color:green">' + value.IsD2CUser + '</td>';
                    }
                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    if (value.IsEwaApprovalAccess == true && (value.StatusId == 0 || value.StatusId == 2)) {
                        employerList += '<td><span class="" ><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" ></span></td>';
                    } else {
                        employerList += '<td><span class="view-user-profile" data-val=' + value.UserGuid + ',' + value.AccessAmountId + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span></td>';
                    }
                    /*count++;*/
                });
                $('#userList').append(employerList);
                page_number(TotalRowCount, pageNumber);

            } else {
                $("#btnExportToCSV").attr('disabled', true);
                page_number('', pageNumber);
                $("#noRecordFound").show();
            }
        }
    });

}

function page_number(count, currentPage) {
    $('#emplyr_pagination').pagination({
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
            console.log(options);
            pageNumber = options.current;
            if (searchParam.length >= 3) {
                getEmployeeEWAList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, status);
            }
            else if (searchParam.length == 0) {
                getEmployeeEWAList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, status);
            }
        }
    });
}

