var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
var userStatus = -1
$("#noRecordFound").hide();
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getBillList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, userStatus);

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
            getBillList(UserGuid, searchParam, pageNumber, pageSize, fromDateSelected, toDateSelected, userStatus);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#employerSearch', function () {
        searchParam = $("#employerSearch").val();
        if (searchParam.length >= 3) {
            getBillList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }
        else if (searchParam.length == 0) {
            getBillList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }

    });

    $('#ddlStatus').change(function () {
        userStatus = $('select#ddlStatus option:selected').val();
        var value = $('select#ddlStatus option:selected').text();
        getBillList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, userStatus);

    });

    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", status);
        formData.append("pageNumber", 1);
        formData.append("PageSize", 10);
        $.ajax({
            type: "POST",
            cache: false,
            url: "Employers/ExportCsvReport",
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
                    link.download = "Employers.xlsx";
                    link.click();
                }
            }
        });
    });
});


function getBillList(id, searchText, pageNumber, pageSize, FromDate, ToDate, Status) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("Status", Status);
    $.ajax({
        type: "POST",
        cache: false,
        url: "BillPamentHistory/GetBillHistoryList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#employerList_body').empty();

            /*let count = 1;*/
            let employerList = '';

            if (response.RstKey == 1) {
                $("#noRecordFound").hide();

                $(response.getBillHistoryLists).each(function (index, value) {

                    TotalRowCount = value.TotalCount;

                    let isblock = "";
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.CustomerName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.TotalAmount + '</td>';
                    employerList += '<td>' + value.ServiceName + '</td>';
                    if (value.TransactionType =="DEBIT") {
                        employerList += '<td class="inActive">' + value.TransactionType + '</td>';
                    } else {
                        employerList += '<td class="active">' + value.TransactionType + '</td>';
                    }
                   
                    employerList += '<td>' + value.TransactionId + '</td>';
                    employerList += '<td>' + value.InvoiceNo + '</td>';
                    if (value.IsAmountPaid == true) {
                        employerList += '<td class="active">Yes</td>'; isblock = "block";
                    }
                    else if (value.IsAmountPaid == false) {
                        employerList += '<td class="inActive">No</td>'; isblock = "unblock";
                    }
                   
                    if (value.TransactionStatus == 1) {
                        employerList += '<td class="active">Success</td>'; isblock = "block";
                    }
                    else {
                        employerList += '<td class="inActive">Failed</td>'; isblock = "unblock";
                    }
                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    //if (value.Status == 'Pending') {
                    //    employerList += '<td><span id="view-employeeProfile" class="" data-val=' + value.Guid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    //    employerList += '<span id="delete-employer" class="" data-val=' + value.Guid + '><img class="img-eye" src="Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                    //    employerList += '<span id="block-employer" class="" data-val=' + value.Guid + ' data-msg = ' + isblock + '><img class="img-eye" class="" src="Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';
                    //}
                    //else {
                    //    employerList += '<td><span id="view-employeeProfile" class="view-employerProfile" data-val=' + value.Guid + " disabled" + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    //    employerList += '<span id="delete-employer" class="delete-employer" data-val=' + value.Guid + '><img class="img-eye" src="Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                    //    employerList += '<span id="block-employer" class="block-employer" data-val=' + value.Guid + ' data-msg = ' + isblock + '><img class="img-eye" class="" src="Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';
                    //}

                    employerList += '</tr>';
                });
                $('#employerList_body').append(employerList);
                page_number(TotalRowCount, pageNumber);

            } else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#noRecordFound").show();
                page_number('', pageNumber);
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
            pageNumber = options.current;
            if (searchParam.length >= 3) {
                getBillList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
            else if (searchParam.length == 0) {
                getBillList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
        }
    });
}