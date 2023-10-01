var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
var Status = ''
var serviceId = 0;
$("#noRecordFound").hide();
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
    getCategories(UserGuid);
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
            getCategoryList(UserGuid, searchParam, pageNumber, pageSize, fromDateSelected, toDateSelected);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });
    $(document).on('keyup', '#categorySearch', function () {
        searchParam = $("#categorySearch").val();
        if (searchParam.length >= 3) {
            getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }
        else if (searchParam.length == 0) {
            getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate);
        }

    });
    $('body').on('click', 'span.block-category', function (e) {
        e.preventDefault;

        var data = $(this).data('val');
        var msg = $(this).data('msg');
        swal({
            title: "Are you sure you want to "+ msg +" this category?",
            text: "",
            icon: "",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("WalletServiceId", data);
            formData.append("AdminUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 1);

            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCategory/BlockUnBlockCategory",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        //getEmployerList();
                        getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Category not blocked !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.delete-category', function (e) {
        e.preventDefault;

        var data = $(this).data('val')
        //var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to delete this category?",
            text: "",
            icon: "",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("WalletServiceId", data);
            formData.append("AdminUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 2);

            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCategory/BlockUnBlockCategory",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    if (response.RstKey == 1) {
                        getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Category not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.view-categoryProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "ManageCategory/ViewManageCotegory/id=" + data;
    });
    $('body').on('click', '#addCat', function (e) {
        e.preventDefault;
        window.location.href = "ManageCategory/AddManageCotegory"
    });


    $('#ddlService').change(function () {
        debugger
        serviceId = $('select#ddlService option:selected').val();
        var state = $('select#ddlService option:selected').text();
        getCategoryList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, serviceId);

    });

    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", serviceId);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageCategory/ExportCsvReportCategory/",
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
                    link.download = "Categories.xlsx";
                    link.click();
                }
            }
        });
    });

});
function getCategoryList(id, searchText, pageNumber, pageSize, FromDate, ToDate, Status) {


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
        url: "ManageCategory/GetCategoryList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#categoryList_body').empty();

            /*let count = 1;*/
            let employerList = '';

            if (response.RstKey == 1) {
                $("#noRecordFound").hide();

                $(response.manageCategories).each(function (index, value) {

                    TotalRowCount = value.TotalCount;
                    let isblock = "";
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.MainCategoryName + '</td>';
                    employerList += '<td>' + value.SubCategoryName + '</td>';
                    employerList += '<td>' + value.ServiceName + '</td>';
                    if (value.Status == 'Active') {
                        employerList += '<td class ="active">' + value.Status + '</td>'; isblock = "block";
                    }
                    else if (value.Status == 'Inactive') {
                        employerList += '<td class ="inactive">' + value.Status + '</td>'; isblock = "unblock";

                    }
                    employerList += '<td title =' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '<td><span id="view-employeeProfile" class="view-categoryProfile" data-val=' + value.SubCategoryGuid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '<span id="delete-employer" class="delete-category" data-val=' + value.WalletServiceId + '><img class="img-eye" src="Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                    employerList += '<span id="block-employer" class="block-category" data-val=' + value.WalletServiceId + ' data-msg = ' + isblock + '><img class="img-eye" class="" src="Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';


                    employerList += '</tr>';
                });
                $('#categoryList_body').append(employerList);
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
    $('#category_pagination').pagination({
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
                getCategoryList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
            else if (searchParam.length == 0) {
                getCategoryList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate);
            }
        }
    });
}

function getCategories(id) {


    var formData = new FormData();
    formData.append("Id", id);

    $.ajax({
        type: "POST",
        cache: false,
        url: "ManageCategory/GetCategories",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {

                let ddlservice= "";// '<option value="">Please select city</option>';
                // $('#ddlCity').append(ddlCountryOptions);
                $.each(response.categoryResponse, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    ddlservice += '<option value="' + value.Id + '">' + value.CategoryName + '</option>';
                });
                $('#ddlService').append(ddlservice);
            } else {

            }
        }
    });

}