var UpfileNormal = {
    Init: function () {
        UpfileNormal.OnEvent();
        UpfileNormal.UpEvent();
    },
    OnEvent: function () {
        $("#form-up-file").on("submit", function (e) {
            try {
                e.preventDefault();
                var form = $(this);
                var url = form.attr("action");
                var formData = new FormData(this);
                var target = form.attr("data-target");
                var type = form.attr("data-type");
                var size = parseInt(form.attr("data-size"));
                formData.Target = target;
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    error: function (e) {
                        alert(e.message);
                    },
                    success: function (response) {
                        var link = "/UpFileNormal/Download?Path=" + response.FullPath + "&FileName=" + response.FileName;
                        var html =
                            '<span class="fileitem member sortitem">'
                            + '<a target="_blank" class="text-left openLink" href="' + link + '" title="">' + response.FileName + '</a>'
                            + '<input type="hidden" name="FileName" class="fileNames" type="text" value="' + response.FileName + '">'
                            + '<input name="Path" class="filePaths" type="hidden" value="' + response.Path + '">'
                            + '<button type="button" class="btn btn-xs btn-link close delMemberCust">x</button>'
                            + '</span>';
                        var eltarget = $(response.Target);
                        if (type == "append") {
                            var x = eltarget.find('.fileitem.member').length;
                            if (eltarget.find('.fileitem.member').length < size) {
                                eltarget.append(html);
                            } else {
                                alert("Bạn đã tải quá " + size + " file");
                            }
                        } else if (type == "replace") {
                            eltarget.html(html);
                        }

                    },
                    cache: false,
                    contentType: false,
                    processData: false
                });
            } catch (e) {
                alert("Đã xảy ra lỗi");
            }

        });
        $(document).on("click", ".delMemberCust", function (e) {
            $(this).closest("span").remove();
        });
        $("#upfile_normal").change(function () {
            try {
                var obj = $(this);
                var form = obj.closest("form");
                form.trigger("submit");
                return;
            } catch (e) {
                alert("Đã xảy ra lỗi");
            }
        });
      
    },
    UpEvent: function () {
        $(document).on("click", ".clickInputFile", function () {
            var target = $($(this).attr("data-click"));
            var idTarget = $(this).attr("data-target");
            var size = $(this).attr("data-size");
            var type = $(this).attr("data-type");
            var form = $("#form-up-file");
            form.attr("data-size", size);
            form.attr("data-type", type);
            $("#upfile_normal_Target").val(idTarget);
            $("#upfile_normal_Target").attr("data-value", idTarget);
            target.on("click");
            target.trigger("click");
        });
    }
};
$(document).ready(function () {
    UpfileNormal.Init();
});