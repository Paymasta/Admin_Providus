var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
let TotalRowCount;
var FromDate = null;
var ToDate = null;
var userStatus = -1;
$("#user-no-record").hide();


$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getEmployeesList(pageNumber, PageSize, searchText, FromDate, ToDate, userStatus);

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
            FromDate = fromDateSelected;
            ToDate = toDateSelected;

            getEmployeesList(pageNumber, PageSize, searchText, fromDateSelected, toDateSelected, userStatus);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#userSearch', function () {
        searchText = $("#userSearch").val();
        if (searchText.length >= 3) {
            getEmployeesList(pageNumber, PageSize, searchText, FromDate, ToDate, userStatus);
        }
        else if (searchText.length == 0) {
            getEmployeesList(pageNumber, PageSize, searchText, FromDate, ToDate, userStatus);
        }

    });

    $('body').on('click', 'span.view-user-profile', function (e) {
        e.preventDefault;
        var data = $(this).data('val')
        window.location.href = "/User/GetEmployeDetailByGuid/id=" + data;
    });
    $('#ddlStatus').change(function () {
        userStatus = $('select#ddlStatus option:selected').val();
        var value = $('select#ddlStatus option:selected').text();
        getEmployeesList(pageNumber, PageSize, searchText, FromDate, ToDate, userStatus);

    });


    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", userStatus);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/User/ExportCsvReportForEmployees/",
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
                    link.download = "EmployeesList.xlsx";
                    link.click();
                }
            }
        });
    });


});
function getEmployeesList(pageNumber, PageSize, SearchTest, FromDate, ToDate, Status) {
    var UserGuid = "";
    UserGuid = sessionStorage.getItem("User1");
    var formData = new FormData();
    formData.append("userGuid", UserGuid);
    formData.append("SearchTest", SearchTest);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("Status", Status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "User/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            $('#userList_body').empty();
            if (response.RstKey == 1) {
                $("#user-no-record").hide();
                $(response.employeesListViewModel).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    var created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    var employerList = "";

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.EmployerName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    if (value.Status == 'Active') { employerList += '<td class="active">' + value.Status + '</td>'; }
                    else if (value.Status == 'Inactive') { employerList += '<td class="inActive">' + value.Status + '</td>'; }
                    else if (value.Status == 'Pending') {
                        employerList += '<td class="" style="color:#32A4FF">' + value.Status + '</td>';
                    }
                    else {
                        employerList += '<td class="">' + value.Status + '</td>';
                    }
                    if (value.IsD2CUser === "D2CUser") {
                        employerList += '<td class="inActive">' + value.IsD2CUser + '</td>';
                    } else {
                        employerList += '<td class="active">' + value.IsD2CUser + '</td>';
                    }

                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    if (value.Status == 'Pending') {
                        employerList += '<td><span class="" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span></td>';
                    }
                    else {
                        employerList += '<td><span class="view-user-profile" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span></td>';
                    }


                    $('#userList_body').append(employerList);

                });
                page_number(TotalRowCount, pageNumber);

            }
            else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#user-no-record").show();
                page_number(0, 1);

            }



        }
    });
}

function page_number(count, currentPage) {
    $('#user_pagination').pagination({

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
            if (searchText.length >= 3) {
                getEmployeesList(options.current, PageSize, searchText, FromDate, ToDate, userStatus);
            }
            else if (searchText.length == 0) {
                getEmployeesList(options.current, PageSize, searchText, FromDate, ToDate, userStatus);
            }

        }
    });
}


/*function download_csv(csv, filename) {
    var csvFile;
    var downloadLink;

    // CSV FILE
    csvFile = new Blob([csv], { type: "text/csv" });

    // Download link
    downloadLink = document.createElement("a");

    // File name
    downloadLink.download = filename;

    // We have to create a link to the file
    downloadLink.href = window.URL.createObjectURL(csvFile);

    // Make sure that the link is not displayed
    downloadLink.style.display = "none";

    // Add the link to your DOM
    document.body.appendChild(downloadLink);

    // Lanzamos
    downloadLink.click();
}

function export_table_to_csv(html, filename) {
    var csv = [];
    var rows = document.querySelectorAll("table tr");

    for (var i = 0; i < rows.length; i++) {
        var row = [], cols = rows[i].querySelectorAll("td, th");

        for (var j = 0; j < cols.length; j++)
            row.push(cols[j].innerText);

        csv.push(row.join(","));
    }

    // Download CSV
    download_csv(csv.join("\n"), filename);
}

function exportToCsv() {
    var html = document.querySelector(".csvexport").outerHTML;
    export_table_to_csv(html, "table.csv");
}*/
