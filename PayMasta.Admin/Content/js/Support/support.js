var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var FromDate = null;
var ToDate = null;
var monthNumber = 0
var UserType = 4;
let TotalRowCount;
let status = -1;
$("#noRecordFound").hide();

$(document).ready(function () {


    //$('#txtFromDate').datepicker();
    //$('#txtTodate').datepicker();
    var startDate = new Date('01/01/2012');
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    // ToEndDate.setDate(ToEndDate.getDate() + 365);
    ToEndDate.setDate(ToEndDate.getDate());

    $('#supp_txtFromDate').datepicker({
        weekStart: 1,
        startDate: '01/01/2012',
        endDate: FromEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        // fromDate = selected.date.valueOf();
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#supp_txtEnddate').datepicker('setStartDate', startDate);
    });
    $('#calender_from').on("click", function () {
        $('#supp_txtFromDate').focus();
    });

    $('#supp_txtEnddate').datepicker({
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        //toDate = FromEndDate.parse('dd-mm-yy') ;
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#supp_txtFromDate').datepicker('setEndDate', FromEndDate);
    });
    $('#calender_to').on("click", function () {
        $('#supp_txtEnddate').focus();
    });

    UserGuid = sessionStorage.getItem("User1");
    getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, FromDate, ToDate, status);

    $("#supp_btnFilter").click(function () {
        var fromDateSelected = $("#supp_txtFromDate").val();
        var toDateSelected = $("#supp_txtEnddate").val();
        var isFromdateTrue = true;
        var isTodateTrue = true;
        if (fromDateSelected == '' || fromDateSelected == "" || fromDateSelected == null) {
            isFromdateTrue = false
            $("#supp_fromdateError").show();
            $("#supp_fromdateError").text("Please select from date");
        }
        else {
            isFromdateTrue = true
            $("#supp_fromdateError").hide();
        }
        if (toDateSelected == '' || toDateSelected == "" || toDateSelected == null) {
            isTodateTrue = false;
            $("#supp_TodateError").show();
            $("#supp_TodateError").text("Please select to date");
        }
        else {
            isTodateTrue = true;
            $("#supp_TodateError").hide();
        }
        if (isFromdateTrue == true && isTodateTrue == true) {

            getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, fromDateSelected, toDateSelected, status);
        }
    });

    $("#supp_btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $('body').on('click', 'span.view-user-profile', function (e) {
        e.preventDefault;
        
        var data = $(this).data('val');
        var ticketId = $(this).attr('data-ticket-id');
        window.location.href = "/Support/ViewSupportTicket/id=" + data + "," + ticketId;
    });
    $('body').on('click', 'span.view-employerProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "Employers/GetEmployerProfile/id=" + data;
    });

    $(document).on('keyup', '#supportSearch', function () {
        searchText = $("#supportSearch").val();
        if (searchText.length >= 3) {
            getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, FromDate, ToDate, status);
        }
        else if (searchText.length == 0) {
            getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, FromDate, ToDate, status);
        }

    });
    $(document).on('click', '.listToggle', function () {
        UserType = $(this).data('val');
        console.log('UserType' + UserType);
        $('#supportList_body').empty();
        getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, FromDate, ToDate, status);

    });



    $('#cars').change(function () {
        status = $('select#cars option:selected').val();
        var monthName = $('select#cars option:selected').text();
        getSupportList(UserGuid, pageNumber, PageSize, searchText, UserType, FromDate, ToDate, status);
        // getEmployeeEWAList(userId, null, null, monthNumber)
    });

    $("#btnExportToCSV").click(function () {
        debugger
        var fromDateSelected1 = $("#supp_txtFromDate").val();
        var toDateSelected1 = $("#supp_txtEnddate").val();


        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("FromDate", fromDateSelected1);
        formData.append("ToDate", toDateSelected1);
        formData.append("StatusId", status);
       // formData.append("Month", status);
        formData.append("UserType", UserType);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Support/ExportCsvReportForEmployees/",
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
                    link.download = "Support.xlsx";
                    link.click();
                }
            }
        });
    });

});

function getSupportList(id, pageNumber, pageSize, searchText, UserType, FromDate, ToDate, status) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchText", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("UserType", UserType);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("StatusId", status);
    $("#noRecordFound").hide();

    $.ajax({
        type: "POST",
        cache: false,
        url: "Support/GetSupportTicketList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#supportList_body').empty();
            let employerList = '';
            if (UserType == "3") {
                $("tr").each(function () {
                    $(this).children("th:eq(1)").hide();

                });
            }
            else {
                $("tr").each(function () {
                    $(this).children("th:eq(1)").show();

                });
            }
            if (response.RstKey == 1) {


                $(response.supportViewModels).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    let statusColor = "";
                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';

                    if (value.EmployerName != null && UserType == "4") {
                        employerList += '<td>' + value.EmployerName + '</td>';
                    }
                    else if (UserType == "4") {
                        employerList += '<td>' + '' + '</td>';
                    }
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + ' ' + value.PhoneNumber + '</td>';
                    if (value.StatusId == 3) { statusColor = "closed"; }
                    else if (value.StatusId == 0) { statusColor = 'pending'; }
                    else if (value.StatusId == 5) { statusColor = 'reject'; }
                    else if (value.StatusId == 4) { statusColor = 'hold'; }
                    else if (value.StatusId == 2) { statusColor = 'in_progress'; }
                    employerList += '<td><div class=' + statusColor + '>' + value.Status + '</div></td>';
                    employerList += '<td title = ' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '<td><span class="view-user-profile" data-val=' + value.UserGuid + ' data-ticket-id=' + value.Id + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '</tr>';

                });
                $('#supportList_body').append(employerList);

                page_number(TotalRowCount, pageNumber, FromDate, ToDate);

               

            } else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#noRecordFound").show();
                page_number('', pageNumber, FromDate, ToDate)
            }
        }
    });

}
function page_number(count, currentPage, FromDate, ToDate) {
    $('#support_pagination').pagination({
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
            if (searchText.length >= 3) {
                getSupportList(UserGuid, options.current, PageSize, searchText, UserType, FromDate, ToDate);

            }
            else if (searchText.length == 0) {
                getSupportList(UserGuid, options.current, PageSize, searchText, UserType, FromDate, ToDate);

            }




        }
    });
}