var AdminGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var UserGuid = "";
var month = '';
$(document).ready(function () {
    AdminGuid = sessionStorage.getItem("User1");
    getUrlVars();
     
    $("#month_filter").on('change', function () {
        month = $(this).val();
        TotalRowCount = "";
        console.log(month);
        GetEmployeeTransactionByGuid(AdminGuid, UserGuid, pageNumber, month);
    });
    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
    });
});
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    UserGuid = hash[1];
    GetEmployeDetailByGuid(AdminGuid, UserGuid);
    GetEmployeeTransactionByGuid(AdminGuid, UserGuid, pageNumber);
    return vars;
}
function GetEmployeDetailByGuid(AdminGuid, UserGuid) {
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("AdminGuid", AdminGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Transactions/GetEmployeDetailByGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                var value = response.employeeDetail;
                $("#user_name").text(value.FirstName + ' ' + value.LastName);
                $("#mobile_no").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#working_hours").text(value.TotalWorkingHours);
                $("#current_available_amount").text('₦ '+value.AvailableAmount );
                $("#email_address").text(value.Email);
                $("#employer_name").text(value.EmployerName );
                $("#earned_wages").text('₦ '+value.EarnedAmount);

            } else {
                $("#trans-no-record").show();
            }
        }
    });

}
function GetEmployeeTransactionByGuid(AdminGuid, UserGuid, pageNumber, month) {
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("AdminGuid", AdminGuid);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("Month", month);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Transactions/GetEmployeeTransactionByGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            $('#transViewList_body').empty();
            let employerList = '';
            if (response.RstKey == 1) {
                $("#trans-view-no-record").hide();
                $(response.employeeTransactions).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' +'₦'+ value.TotalAmount + '</td>';
                    employerList += '<td>' + value.BillerName + '</td>';
                    employerList += '<td title = ' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '</tr>';

                });
                $('#transViewList_body').append(employerList);
                page_number(TotalRowCount, pageNumber);

            } else {
                $("#trans-view-no-record").show();
                page_number(0, pageNumber);
            }
        }
    });

}
function page_number(count, currentPage) {
    $('#trans_view_pagination').pagination({
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
            GetEmployeeTransactionByGuid(AdminGuid, UserGuid, options.current,month);
            
        }
    });
}