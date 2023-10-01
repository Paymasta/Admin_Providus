var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";

$(document).ready(function () {
    $("#unblock_user").hide();
    $("#block_user").hide();

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
        getCategoryData(UserGuid)
        return vars;
    }
    $('body').on('click', '#delete_user', function (e) {
        e.preventDefault;

        swal({
            title: "Are you Sure you want to delete this category?",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", UserGuid);
            formData.append("AdminUserGuid", AdminUserGuid);
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
                        //getEmployerList();
                        //getEmployerList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employer not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $(document).on('click', '#editBtn', function () {
        var catName = $('#category_name').text();
        var subCatName = $('#subcategory_name').text();
        $("#sub_catName").val(subCatName);
        $("#catName").val(catName);
        $('.desc').hide();
        $('.btnfirst').hide();
        $('.proDesc').show();
        $('.btnSecond').show();
    });
    $(document).on('click', '#btnCancel', function () {
        $('.proDesc').hide();
        $('.btnSecond').hide();
        $('.desc').show();
        $('.btnfirst').show();
    });
    $(document).on('click', '#btnSave', function () {
        var sub_categoryName = $("#sub_catName").val();
        var categoryName = $("#catName").val();
        var formData = new FormData();
        formData.append("AdminUserGuid", AdminUserGuid);
        formData.append("SubCategoryGuid", UserGuid);
        formData.append("SubCategory", sub_categoryName);
        formData.append("MainCategory", categoryName);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageCategory/UpdateCategoryDetail",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey == 1) {
                    swal({
                        title: "",
                        text: "Data Updated successfully.",
                        icon: "success",
                        button: ""
                    }, function succes(isDone) { location.reload(); });
                } else {
                    swal({
                        title: "oops!",
                        text: "Category not Updated !",
                        icon: "error",
                        button: "ohh no!"
                    });
                }
            }
        });


    });
    $('body').on('click', '.removedata', function (e) {
        var formData = new FormData();
        formData.append("WalletServiceId", data);
        formData.append("AdminUserGuid", UserGuid);
        formData.append("DeleteOrBlock", 2);
        e.preventDefault;


        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageCategory/BlockUnBlockCategory",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey == 1) {
                    //getEmployerList();
                    //getEmployerList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
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


    });
    $(document).on('click', '#backImg', function () {
        window.href = window.history.back();
    });

});


function getCategoryData(guid) {

    var formData = new FormData();
    /*formData.append("EmployerGuid", guid);*/
    formData.append("SubCategoryGuid", guid);
    formData.append("AdminUserGuid", AdminUserGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCategory/ViewCategoryDetail",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response.RstKey == 1) {
                let isblock = "";
                var value = response.getCategoryDetailResponse;
                if (value.ImageUrl === null || value.ImageUrl == "") {
                    $("#profile_image").attr('src', "/Content/img/images/man-placeholder.png");
                }
                else {
                    $("#profile_image").attr('src', value.ImageUrl);

                }
                $("#category_name").text(value.MainCategoryName);
                $("#subcategory_name").text(value.SubCategoryName);

                

            } else {
                swal({
                    title: "oops!",
                    text: response.Message,
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });

}