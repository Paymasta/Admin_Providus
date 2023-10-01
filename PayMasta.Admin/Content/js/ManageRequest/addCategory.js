var AdminUserGuid = "";

$(document).ready(function () {
    AdminUserGuid = sessionStorage.getItem("User1");

    $(document).on('keyup', '#catName', function () {
        var catName = $('#catName').val();
        var regex = /^[A-Za-z ]+$/;
        if (catName.length > 0 && regex.test(catName)) {
            $('#savepopBtn').prop('disabled', false);
        }
        else {
            $('#savepopBtn').prop('disabled', true);
        }
    });
    $(document).on('keyup', '#sub_catName', function () {
        var subCat = $('#sub_catName').val();
        var regex = /^[A-Za-z ]+$/;
        if (subCat.length > 0 && regex.test(subCat)) {
            $('#savepopBtn').prop('disabled', false);
        }
        else {
            $('#savepopBtn').prop('disabled', true);
        }
    });
    function containsSpecialChars(str) {
        const specialChars = /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
        return specialChars.test(str);
    }
    $(document).on('click', '#btnSave', function () {
        
        var sub_categoryName = $("#sub_catName").val();
        var categoryName = $("#catName").val();
        console.log(containsSpecialChars('hello!'));
        console.log(containsSpecialChars('abc'));
        if (containsSpecialChars(categoryName) == false && containsSpecialChars(sub_categoryName) == false) {
            var formData = new FormData();
            formData.append("AdminUserGuid", AdminUserGuid);
            formData.append("SubCategory", sub_categoryName);
            formData.append("MainCategory", categoryName);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/ManageCategory/AddCategories",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        $('#saveChanges').modal('hide');
                        swal({
                            title: "",
                            text: "Category Added successfully.",
                            icon: "success",
                            button: ""
                        }, function succes(isDone) {
                            location.reload();
                        });
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
        }
        else {
            swal({
                title: "oops!",
                text: "Special characters are not allowed !",
                icon: "error",
                button: "ohh no!"
            });
        }
        


    });

    $(document).on('click', '#btnCancel, #backImng', function () {
        window.href = window.history.back();
    });


    $(function () {
        $('#catName').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });
    $(function () {
        $('#sub_catName').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });

    $('#catName').keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        else {
            e.preventDefault();
            alert('Please Enter Alphabet');
            return false;
        }
    });
    $('#sub_catName').keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        else {
            e.preventDefault();
            alert('Please Enter Alphabate');
            return false;
        }
    });
});