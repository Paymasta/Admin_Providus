var employeeGuid = "";
var UserGuid = "";
var accessRequestId = 0;
var faqId = 0;
var ppId = 0;
var tandcId = 0;
var pageNumber = 1;
var pageSize = 10;
var searchParam = "";
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getFaqQuestions(UserGuid);
    getppData(UserGuid);
    gettncData(UserGuid);
    getQuestionsAnswer(UserGuid, pageNumber, pageSize)
    $("#btnFaqSaveQuestions").click(function () {
        debugger
        var txtFaq = $('#txtfaqQuestion').val().trim();//$("#summernoteFaq").val();
        if (txtFaq != "") {
            var formData = new FormData();
            formData.append("TextContent", txtFaq);
            formData.append("AdminGuid", UserGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCMS/SaveFaqQuestions",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        window.location.reload();
                        swal({
                            title: "Good job!",
                            text: "Successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        } else {
            swal({
                title: "oops!",
                text: "Please enter faq!",
                icon: "error",
                button: "Aww yiss!"
            });
        }
    })


    $("#btnTandCSave").click(function () {
        debugger
        var txtFaq = $('#summernoteTandC').summernote('code');//$("#summernoteFaq").val();
        if (txtFaq != "") {
            var formData = new FormData();
            formData.append("TextContent", txtFaq);
            formData.append("AdminGuid", UserGuid);
            formData.append("ContentId", tandcId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCMS/SaveTandCData",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: "Successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        } else {
            swal({
                title: "oops!",
                text: "Please enter faq!",
                icon: "error",
                button: "Aww yiss!"
            });
        }
    })
    $("#btnPrivacyPolicySave").click(function () {
        debugger
        var txtFaq = $('#summernotePrivacyPolicy').summernote('code');//$("#summernoteFaq").val();
        if (txtFaq != "<p><br></p>") {
            var formData = new FormData();
            formData.append("TextContent", txtFaq);
            formData.append("AdminGuid", UserGuid);
            formData.append("ContentId", ppId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCMS/SavePrivacyPolicyData",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: "Successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        } else {
            swal({
                title: "oops!",
                text: "Please enter faq!",
                icon: "error",
                button: "Aww yiss!"
            });
        }
    })


    $('#ddlQuestions').change(function () {


        faqId = $('select#ddlQuestions option:selected').val();
        var faqText = $('select#ddlQuestions option:selected').text();
    });

    $("#btnFaqSaveAnswer").click(function () {
        var txtFaqAnsDetail = $('#txtfaqQuestionAnswer').val().trim();//$("#summernoteFaq").val();
        if (faqId > 0 && txtFaqAnsDetail != "") {
            var formData = new FormData();
            formData.append("FaqId", faqId);
            formData.append("TextContent", txtFaqAnsDetail);
            formData.append("AdminGuid", UserGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ManageCMS/SaveFaqQuestionsDetail",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        window.location.reload();
                        swal({
                            title: "Good job!",
                            text: "Successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        } else {
            swal({
                title: "oops!",
                text: "Please enter all required detail!",
                icon: "error",
                button: "Aww yiss!"
            });
        }
    })

    $('body').on('click', 'span.view-question', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "ManageCMS/EditQuestionAndAnswer/id=" + data;
    });
    $('body').on('click', 'span.delete-question', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        swal({
            title: "Are you sure you want to delete this question and answer ?",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        },
            function succes(isDone) {
                var formData = new FormData();
                formData.append("faqId", data);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "ManageCMS/DeleteQuestionAnswer",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        if (response.RstKey == 1) {
                            swal({
                                title: "Good job!",
                                text: "Successfully!",
                                icon: "success",
                                button: "Aww yiss!"
                            });
                        } else {
                            swal({
                                title: "oops!",
                                text: "Error!",
                                icon: "error",
                                button: "Aww yiss!"
                            });
                        }
                    }
                });
            })

    });
});


function getFaqQuestions(adminGuid) {
    var formData = new FormData();
    formData.append("AdminGuid", adminGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCMS/GetFaqList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                let ddOptions = "";//'<option value="">Please select country</option>'
                $.each(response.faqQuestionResponses, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    ddOptions += '<option value="' + value.Id + '">' + value.QuestionText + '</option>';
                });
                $('#ddlQuestions').append(ddOptions);
            } else {
                //swal({
                //    title: "oops!",
                //    text: response.Message,
                //    icon: "error",
                //    button: "ohh no!"
                //});
            }
        }
    });

}
function getppData(adminGuid) {
    var formData = new FormData();
    formData.append("AdminGuid", adminGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCMS/GetPrivacyPolicyById/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                ppId = response.getContent.Id;
                var markupStr = response.getContent.Detail;
                $('#summernotePrivacyPolicy').summernote('code', markupStr);

            } else {
                //swal({
                //    title: "oops!",
                //    text: response.Message,
                //    icon: "error",
                //    button: "ohh no!"
                //});
            }
        }
    });

}
function gettncData(adminGuid) {
    var formData = new FormData();
    formData.append("AdminGuid", adminGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCMS/GetTandCData/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                tandcId = response.getContent.Id;
                var markupStr = response.getContent.Detail;
                $('#summernoteTandC').summernote('code', markupStr);
                //  $("#btnFaq").trigger("click");
                //$("#summernoteFaq").val(response.getFaq.Detail);
            } else {

            }
        }
    });

}


function getQuestionsAnswer(adminGuid, pageNumber, PageSize) {
    var formData = new FormData();
    formData.append("userGuid", adminGuid);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageCMS/GetQuestionAnswerList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            $('#questionList').empty();

            let questionList = '';
            var number = 1;
            if (response.RstKey == 1) {
                $("#noRecordFound").hide();
                $(response.faqQuestionResponses).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    questionList += '<tr>';

                    questionList += '<td>' + value.RowNumber + '</td>';
                    questionList += '<td>' + value.QuestionText + '</td>';
                    questionList += '<td>' + value.Detail + '</td>';
                    questionList += '<td>' + value.CreatedAt + '</td>';
                    questionList += '<td><span class="view-question" data-val=' + value.FaqId + '><img class="img-eye" src="Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"/></span>';
                    questionList += '<span class="delete-question" data-val=' + value.FaqId + '><img class="img-eye delete-question" src="Content/img/delete.png" alt=""data-toggle="tooltip" title="Delete"/></span>';
                    questionList += '</td>';
                    questionList += '</tr>';

                });
                $('#questionList').append(questionList);
                page_number(TotalRowCount, pageNumber);

            } else {
                $("#noRecordFound").show();
                page_number(0, pageNumber);
            }
        }
    });

}
function page_number(count, currentPage) {
    $('#trans_pagination').pagination({
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
                getQuestionsAnswer(UserGuid, options.current, pageSize);
            }
            else if (searchParam.length == 0) {
                getQuestionsAnswer(UserGuid, options.current, pageSize);
            }
        }
    });
}