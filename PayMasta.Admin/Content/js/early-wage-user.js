var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';


$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getEmployeesList(UserGuid, pageNumber, PageSize, searchText);
   
    


    $('body').on('click', '.delete-employee', function (e) {
        e.preventDefault;


        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you Sure you want to delete employee",
            text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("EmployeerUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 1);

            $.ajax({
                type: "POST",
                cache: false,
                url: "User/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        getEmployeesList(userGuid, pageNumber, PageSize, SearchTest)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employee not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.block-employee', function (e) {
        e.preventDefault;


        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you Sure you want to block the employee ?",
            text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("AdminUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 1);

            $.ajax({
                type: "POST",
                cache: false,
                url: "User/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        getEmployeesList(UserGuid, pageNumber, PageSize, searchText)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employee not blocked !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.view-user-profile', function (e) {
        e.preventDefault;
        var data = $(this).data('val')
        window.location.href = "UserProfile/ViewUserProfile/id=" + data;
    });


});



function getEmployeesList(userGuid, pageNumber, PageSize, SearchTest) {
    var UserGuid = "";
    UserGuid = sessionStorage.getItem("User1");
    var formData = new FormData();
    formData.append("userGuid", UserGuid);
    formData.append("SearchTest", "");
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);


    $.ajax({
        type: "POST",
        cache: false,
        url: "User/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log("eheuirheiurheur" + response);
          
            $('#userList_body').empty();
            var count = 1;
            $(response.employeesListViewModel).each(function (index, value) {
                var created_date = new Date(value.CreatedAt);
                let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
           
                var employerList = "";
                /* var pageStart = (pagination.pageNumber - 1) * pagination.pageSize;
                 var pageEnd = pageStart + pagination.pageSize;
                 var pageItems = response.slice(pageStart, pageEnd);*/
                employerList += '<tr>';
                employerList += '<td>' + count + '</td>';
                employerList += '<td>' + value.FirstName + value.LastName + '</td>';
                employerList += '<td>' + value.EmployerName + '</td>';
                employerList += '<td>' + value.Email + '</td>';
                employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                employerList += '<td>' + value.Status + '</td>';
                employerList += '<td>' + dateFormat + '</td>';
                employerList += '<td><span class="view-user-profile" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                employerList += '<span class="delete-employer" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                employerList += '<span class="block-employee" data-val=' + value.UserGuid + '><img class="img-eye" class="" src="Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';

                $('#userList_body').append(employerList);

/*                $('#userList_body').append('<tr><td>' + count + '</td><td>' + value.FirstName + " " + value.LastName + '</td><td>' + value.EmployerName + '</td><td>' + value.Email + '</td><td>' + value.CountryCode + "-" + value.PhoneNumber + '</td><td>' + value.Status + '</td><td>' + dateFormat + '</td><td><img src="../Content/img/view_eye_icon.svg"  class="img-eye" data-val=' + value.UserGuid + '><img src="../Content/img/delete.png"  class="img-eye delete-employee" data-val=' + value.UserGuid + '><img src="../Content/img/close.png"  class="img-eye block-employee" data-val=' + value.UserGuid + '></td></tr>');
*/                count++
            });


        }
    });
}