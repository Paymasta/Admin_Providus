var AdminGuid = "";
var id = "";
$(document).ready(function () {
    AdminGuid = sessionStorage.getItem("User1");
    debugger
    var articleId = 0;
    getUrlVars();

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        urlkey = hash[1];

    }

    if (urlkey !== undefined && urlkey !== null) {
        articleId = urlkey;
        getArticleById(articleId);
    }


    $('#txtArticleText').summernote(
        {
            placeholder: 'Please enter article text',
            spellCheck: false,
            height: 220,
            focus: true
        }
    );


    $(document).on('click', '#btnSaveArticle', function () {

        var isFormValidationError = false;
        debugger

        var articleText = $("#txtArticleText").val();
        var priceMoney = $("#txtPriceMoney").val();
        var option1Text = $("#txtOption1Text").val();
        var option2Text = $("#txtOption2Text").val();
        var option3Text = $("#txtOption3Text").val();
        var option4Text = $("#txtOption4Text").val();
        var correctOption = $("input:radio[name='options']:checked").val();

        if (articleText === undefined || articleText === null || articleText === '' || articleText.length < 15) {
            $('#txtArticleTextError').text('Please enter article text');
            isFormValidationError = true;
        }
        if (priceMoney === undefined || priceMoney === null || priceMoney === '') {
            $('#txtPriceMoneyError').text('Please enter price Money');
            isFormValidationError = true;
        }
        if (option1Text === undefined || option1Text === null || option1Text === '') {
            $('#txtOption1TextError').text('Please enter option1Text');
            isFormValidationError = true;
        }
        if (option2Text === undefined || option2Text === null || option2Text === '') {
            $('#txtOption2TextError').text('Please enter option2Text }');
            isFormValidationError = true;
        }
        if (option3Text === undefined || option3Text === null || option3Text === '') {
            $('#txtOption3TextError').text('Please enter option3Text');
            isFormValidationError = true;
        }
        if (option4Text === undefined || option4Text === null || option4Text === '') {
            $('#txtOption4TextError').text('Please enter option4Text');
            isFormValidationError = true;
        }

        if (correctOption === undefined || correctOption === null || correctOption === '' || correctOption === '0' || correctOption===0) {
            swal({
                title: "oops!",
                text: "Please choose correct option !",
                icon: "error",
                button: "ohh no!"
            });
            isFormValidationError = true;
        }


        if (isFormValidationError) {
            return false;
        }


        var formData = new FormData();
        formData.append("articleId", articleId);
        formData.append("articleText", articleText);
        formData.append("priceMoney", priceMoney);
        formData.append("option1Text", option1Text);
        formData.append("option2Text", option2Text);
        formData.append("option3Text", option3Text);
        formData.append("option4Text", option4Text);
        formData.append("correctOption", correctOption);


        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageArticle/SaveArticle",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey == 1 || response.RstKey == 2) {
                    swal({
                        title: "",
                        text: "Saved successfully.",
                        icon: "success",
                        button: ""
                    }, function succes(isDone) {
                        window.location.href = "/ManageArticle"
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
    });


    $(document).on('click', '#btnCancel, #backImng', function () {
        window.href = window.history.back();
    });


});

function getArticleById(articleId) {
    var formData = new FormData();
    formData.append("articleId", articleId);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageArticle/GetArticleById",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            debugger
            var data = response.Result;
            if (data != null) {
                $("#txtArticleText").val(data.ArticleText);
                $("#txtPriceMoney").val(data.PriceMoney);
                $("#txtOption1Text").val(data.Option1Text);
                $("#txtOption2Text").val(data.Option2Text);
                $("#txtOption3Text").val(data.Option3Text);
                $("#txtOption4Text").val(data.Option4Text);
                //$("input:radio[name='options']:checked").val(data.CorrectOption);
                //$(":radio[name='sectionRules'][value="+data.CorrectOption+"]").attr('checked', 'checked');
                let radBtnDefault = document.getElementById("option" + data.CorrectOption);
                radBtnDefault.checked = true;
            }
        }
    });

}
