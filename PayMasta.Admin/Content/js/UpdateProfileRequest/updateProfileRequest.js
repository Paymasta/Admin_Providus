var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = "";
let TotalRowCount = "";
var FromDate = null;
var ToDate = null;

$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getEmployersList(UserGuid, pageNumber, PageSize, searchText);

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
            getEmployersList(UserGuid, pageNumber, PageSize, searchText, fromDateSelected, toDateSelected);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#employerSearch', function () {
        searchText = $("#employerSearch").val();
        if (searchText.length >= 3) {
            getEmployersList(UserGuid, pageNumber, PageSize, searchText, FromDate, ToDate);
        }
        else if (searchText.length == 0) {
            getEmployersList(UserGuid, pageNumber, PageSize, searchText, FromDate, ToDate);
        }

    });

    $('body').on('click', 'span.view-employerProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "/UpdateProfileRequest/ViewUpdateProfileDetail/id=" + data;
    });

    $('body').on('click', 'span.delete-Request', function (e) {
        e.preventDefault;

        var data = $(this).data('val')
        //var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to delete this request?",
            text: "",
            icon: "",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("UpdateUserProfileRequestId", data);
            formData.append("AdminGuid", UserGuid);
           

            $.ajax({
                type: "POST",
                cache: false,
                url: "UpdateProfileRequest/DeleteProfileRequest",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    
                    if (response.RstKey == 1) {
                        //getEmployerList();
                        getEmployersList(UserGuid, pageNumber, PageSize, searchText);
                        swal({
                            title: "Good job!",
                            text:"Request deleted successfully",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Request not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("Todate", toDateSelected);
      
        $.ajax({
            type: "POST",
            cache: false,
            url: "/UpdateProfileRequest/ExportCsvReportForEmployees/",
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
                    link.download = "ProfileRequests.xlsx";
                    link.click();
                }
            }
        });
    });

});
function getEmployersList(userGuid, pageNumber, PageSize, SearchTest, FromDate, ToDate) {
    
    
    var formData = new FormData();
    formData.append("userGuid", userGuid);
    formData.append("SearchTest", SearchTest);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", FromDate);
    formData.append("Todate", ToDate);


    $.ajax({
        type: "POST",
        cache: false,
        url: "/UpdateProfileRequest/GetEmployeeList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            
            $('#requestList_body').empty();
            if (response.RstKey == 1) {
                $("#noRecordFound").hide();
                $(response.updateProfileRequestResponses).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    var created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    var employerList = "";

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + '</td>';
                    employerList += '<td>' + value.LastName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    employerList += '<td title = ' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '<td><span class="view-employerProfile" data-val=' + value.Guid + '><img class="img-eye" src="/Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '<span class="delete-Request" data-val=' + value.UpdateUserProfileRequestGuid + '><img class="img-eye" src="/Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';

                    $('#requestList_body').append(employerList);
                    page_number(TotalRowCount, pageNumber);

                });
               
            }
            else {
                $("#btnExportToCSV").attr('disabled', true);
                page_number(0, pageNumber);
                $("#noRecordFound").show();
            }
        }
    });
}

function page_number(count, currentPage) {
    $('#request_pagination').pagination({
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
                getEmployersList(UserGuid, options.current, PageSize, searchText, FromDate, ToDate);
            }
            else if (searchText.length == 0) {
                getEmployersList(UserGuid, options.current, PageSize, searchText, FromDate, ToDate);
            }
        }
    });
}