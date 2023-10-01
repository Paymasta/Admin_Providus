var AdminGuid = "";
var id = "";
$(document).ready(function () {
    AdminGuid = sessionStorage.getItem("User1");
    
    getArticleList();

    $('body').on('click', 'span.view-employerProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val');
        window.location.href = "/ManageArticle/SaveArtical/id=" + data;
    });
});

function getArticleList() {
    var formData = new FormData();
    $.ajax({
        type: "POST",
        cache: false,
        url: "ManageArticle/GetArticleList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#articleList_body').empty();
            //alert(JSON.stringify(response))
            /*let count = 1;*/
            let articleList = '';

            if (response.RstKey == 1) {
                $("#noRecordFound").hide();

                $(response.Result).each(function (index, value) {

                   

                    articleList += '<tr>';
                    articleList += '<td>' + (index+1) + '</td>';
                    articleList += '<td>' + value.ArticleText + '</td>';
                    articleList += '<td>' + value.PriceMoney + '</td>';
                    articleList += '<td>' + value.Option1Text + '</td>';
                    articleList += '<td>' + value.Option2Text + '</td>';
                    articleList += '<td>' + value.Option3Text + '</td>';
                    articleList += '<td>' + value.Option4Text + '</td>';
                    //if (value.Status == 'Active') {
                    //    articleList += '<td class ="active">' + value.Status + '</td>'; isblock = "block";
                    //}
                    //else if (value.Status == 'Inactive') {
                    //    articleList += '<td class ="inactive">' + value.Status + '</td>'; isblock = "unblock";

                    //}
                   /* articleList += '<td title =' + dateFormat + '>' + dateFormat + '</td>';*/
                    articleList += '<td><span class="view-employerProfile" data-val=' + value.ArticleId + '><img class="img-eye" src="/Content/img/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    //articleList += '<span id="delete-employer" class="delete-category" data-val=' + value.ArticleId + '><img class="img-eye" src="Content/img/delete.png" alt="" data-toggle="tooltip" title="Delete"></span>';
                    //articleList += '<span id="block-employer" class="block-category" data-val=' + value.ArticleId + ' data-msg = ' + "block" + '><img class="img-eye" class="" src="Content/img/close.png" alt="" data-toggle="tooltip" title="Block"></span></td>';


                    articleList += '</tr>';


                });
                $('#articleList_body').append(articleList);
               

            } else {
                $("#noRecordFound").show();
            }
        }
    });

}