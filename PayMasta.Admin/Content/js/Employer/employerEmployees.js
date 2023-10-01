var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";

$(document).ready(function () {
    $("#employer-employees-no-record").hide();
    AdminUserGuid = sessionStorage.getItem("User1");
    getUrlVars();
    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        UserGuid = hash[1];
        getEmployeesList(UserGuid, pageNumber, PageSize)
        return vars;
    }

    $('body').on('click', 'span.view-employerEmployeeProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "/Employers/GetEmployeeProfile/id=" + data;
    });
    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
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
            url: "/Employers/ExportCsvReportForEmployees/",
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
                    link.download = "employees.xlsx";
                    link.click();
                }
            }
        });
    });


    $('body').on('click', 'span.block-employer-employee', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        var blk_msg = $(this).data('msg');

        swal({
            title: "Are you sure you want to " + blk_msg + " this user?",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        },
            function succes(isDone) {
                var formData = new FormData();
                formData.append("EmployeeUserGuid", data);
                formData.append("AdminUserGuid", UserGuid);
                formData.append("DeleteOrBlock", 1);

                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/Employers/BlockUnBlockEmployees",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        if (response.RstKey == 1) {
                            swal({
                                title: "Good job!",
                                text: response.Message,
                                icon: "success",
                                button: "Aww yiss!"
                            }, function succes(isDone) { location.reload(); });
                        } else {
                            swal({
                                title: "oops!",
                                text: "User not blocked !",
                                icon: "error",
                                button: "ohh no!"
                            });
                        }
                    }
                });
            })

    });
    $('body').on('click', 'span.delete-employer-employee', function (e) {
        e.preventDefault;

        var data = $(this).data('val')
        swal({
            title: "Are you sure you want to delete this user?",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("AdminUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 2);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Employers/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    if (response.RstKey == 1) {

                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        }, function succes(isDone) { location.reload(); });
                    } else {
                        swal({
                            title: "oops!",
                            text: "User not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });
});
function getEmployeesList(UserGuid, pageNumber, PageSize) {

    var formData = new FormData();
    formData.append("userGuid", UserGuid);
    formData.append("SearchTest", "");
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);

    $.ajax({
        type: "POST",
        cache: false,
        url: "",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            $("#employerEmp_List").empty();
            let li_company_name = '';
            var employerList = '';
            if (response.RstKey == 1) {
                $("#employer-employees-no-record").hide();
                $(response.employeesListViewModel).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dd = created_date.getDate();
                    let mm = created_date.getMonth() + 1;
                    let yyyy = created_date.getFullYear();
                    let dateFormat = dd + '/' + mm + '/' + yyyy;
                    let msg = "";
                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.StaffId + '</td>';
                    employerList += '<td>' + value.CountryCode + ' ' + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    if (value.StatusId == 1)
                    {
                        employerList += '<td class="active">' + value.Status + '</td>'; msg = "block";
                    }
                    else if (value.StatusId == 0) {
                        employerList += '<td class="in-active">' + value.Status + '</td>'; msg = "unblock";
                    }
                    employerList += '<td><span id="view-employerEmployeeProfile" class="view-employerEmployeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="/Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '<span id="delete-employer-employee" class="delete-employer-employee" data-val=' + value.UserGuid + '><img class="img-eye" src="/Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                    employerList += '<span id="block-employer-employee" class="block-employer-employee" data-val=' + value.UserGuid + ' data-msg='+msg+'><img class="img-eye" class="" src="/Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';

                    employerList += '</tr>';
                    li_company_name = value.EmployerName;
                });
                $('#list_company_name').text(li_company_name);
                $('#employerEmp_List').append(employerList);
                page_number(TotalRowCount, pageNumber);
                

            } else {
                $("#employer-employees-no-record").show();
                $("#btnExportToCSV").attr('disabled', true);
            }
        }
    });

}
function page_number(count, currentPage) {
    $('#employerEmployessPagination').pagination({
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
            getEmployeesList(UserGuid, options.current, PageSize);

        }
    });
}
