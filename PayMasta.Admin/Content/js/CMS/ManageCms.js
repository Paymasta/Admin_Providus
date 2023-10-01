var AdminGuid = "";
var id = "";
let QuestionError = true;
let AnswerError = true;
$(document).ready(function () {
    AdminGuid = sessionStorage.getItem("User1");
    getUrlVars();

    $("#btnSaveFaq").click(function () {
        var formData = new FormData();
        var updateQuestionAnswerDetails = new Array();
        var faq = $("#txtQuestion").val();
        var faqid = $("#txtfaqid").val();
        $(".txtAnswer").each(function () {
            var arr = (this.id).split('_');
            var data = $("#" + this.id).val();

            if (data == '' || data == "" || data == null || data == undefined) {
                QuestionError = false;
                swal({
                    title: "Error!",
                    text: "Please enter all answers",
                    icon: "error",
                    button: "Aww yiss!"
                });
            }
            else {
                QuestionError = true;
                updateQuestionAnswerDetails.push({
                    "FaqDetailId": parseInt(arr[1]),
                    "FaqDetail": data
                });
            }

        });

        if (faq == '' || faq == "" || faq == null || faq == undefined) {
            AnswerError = false;
            swal({
                title: "Error!",
                text: "Please enter question.",
                icon: "error",
                button: "Aww yiss!"
            });
        }
        else {
            AnswerError = true;
        }
        if (updateQuestionAnswerDetails.length != 0 && QuestionError == true && AnswerError == true) {
            formData.append("userGuid", AdminGuid);
            formData.append("Faq", faq);
            formData.append("FaqId", faqid);
            formData.append("UpdateQuestionAnswerDetailsString", JSON.stringify(updateQuestionAnswerDetails));
            console.log(updateQuestionAnswerDetails.toString());
            console.log(formData);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/ManageCMS/UpdateQuestionAnswer",
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
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    id = hash[1];
    getQuestionAnswerList(parseInt(id));
    return vars;
}

function getQuestionAnswerList(id) {


    var formData = new FormData();
    formData.append("faqId", id);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCMS/GetQuestionAnswerListForView/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response != null) {
                var data = "";
                var ans = "";
                $("#txtQuestion").val(response.QuestionText);
                $("#txtfaqid").val(response.Id);
                data += '<button type="button" class="btn btn-info" data-toggle="collapse" data-target="#demo' + response.Id + '">Q-: ' + response.QuestionText + '</button>';

                $(response.FaqDetails).each(function (index, value) {
                    data += '<div id="demo' + value.FaqId + '" class="collapse">' + value.Detail + '</div>';

                    ans += '<textarea class="txtAnswer" id="txtAnswer_' + value.Id + '" style="height:300px; width: 100%;margin-top:20px">' + value.Detail + '</textarea>';
                });
                $("#divQuestionAnswer").append(data);
                $("#divAnswer").append(ans);

            } else {

            }
        }
    });

}