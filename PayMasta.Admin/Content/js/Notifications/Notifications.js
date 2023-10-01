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
$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getUserList(UserGuid, pageNumber, PageSize, searchText);

    $(document).on('keyup', '#notifySearch', function () {
        searchText = $("#notifySearch").val();
        if (searchText.length >= 3) {
            getUserList(UserGuid, pageNumber, PageSize, searchText);
        }
        else if (searchText.length == 0) {
            getUserList(UserGuid, pageNumber, PageSize, searchText);
        }

    });
    $("#checkAll").click(function () {

        $("input[name='withdrawRequest']").attr("checked", this.checked);

    });
    $("#btnSubmit").click(function () {

        var arrItem = [];
        var commaSeparetedId = '';
        $("#userList_body td input[type=checkbox]").each(function (index, val) {

            var checkId = $(val).attr("Id");
            var arr = checkId.split('_');
            var currentCheckboxId = arr[1];
            var IsChecked = $("#" + checkId).is(":checked", true);
            if (IsChecked) {
                arrItem.push(currentCheckboxId);
            }
            //if (arrItem.length != 0) {

            //}

        });
        console.log(arrItem);
        if (arrItem.length != 0) {
            commaSeparetedId = arrItem.toString();
            var msg = sessionStorage.getItem('notifyMessage');
            var formData = new FormData();
            formData.append("AdminGuid", UserGuid);
            formData.append("NotificatiomMessage", msg);
            formData.append("UserIds", commaSeparetedId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/ManageNotifications/SendNotification",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    getUserList(UserGuid, pageNumber, PageSize, searchText);
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });

                        window.location.reload();
                    } else {
                        swal({
                            title: "Oops!",
                            text: 'Failed',
                            icon: "error",
                            button: "OK"
                        });
                    }
                }
            });
        }

    });
});


function getUserList(id, pageNumber, pageSize, searchText) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);

    //$("#noRecordFound").hide();

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageNotifications/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            $('#userList_body').empty();
            if (response.RstKey == 1) {
                $("#noRecordFound").hide();
                $(response.employeesListViewModel).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    var created_date = new Date(value.CreatedAt);
                    var dd = created_date.getDate();
                    var mm = created_date.getMonth() + 1;
                    var yyyy = created_date.getFullYear();
                    var dateFormat = dd + '/' + mm + '/' + yyyy;
                    var employerList = "";

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.EmployerName + '</td>';
                    employerList += '<td style="margin-left: 32px;"><input type="checkbox" value="" name="withdrawRequest" class="check" id="check_' + value.UserId + '"></td>';

                    $('#userList_body').append(employerList);

                });
                $('#btnSubmit').show();
                page_number(TotalRowCount, pageNumber);
            }
            else {
                $("#noRecordFound").show();
                $('#btnSubmit').hide();
                page_number('', pageNumber);
            }
        }
    });

}
function page_number(count, currentPage, FromDate, ToDate) {
    $('#notification_pagination').pagination({
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
                getUserList(UserGuid, options.current, PageSize, searchText, UserType, FromDate, ToDate);

            }
            else if (searchText.length == 0) {
                getUserList(UserGuid, options.current, PageSize, searchText, UserType, FromDate, ToDate);

            }




        }
    });
}