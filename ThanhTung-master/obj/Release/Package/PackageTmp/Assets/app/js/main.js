var defaultNumber = 0;
var Main = {
    init: function () {

        Main.onEvent();
        Main.upEvent();
        Main.backLink();
    },
    upEvent: function (container) {
        if (Utils.isEmpty(container))
            container = jQuery(document);

        container.find(".useDragable").draggable({
            cursorAt: { left: 5 }
        });
        container.find(".editorSummernote").each(function () {
            if (!jQuery(this).hasClass("setSummernote")) {
                jQuery(this).addClass("setSummernote").summernote({
                    height: '200px'
                });
            }
        });
        container.find(".MySplitter ").each(function () {
            $('.MySplitter').splitter();
        });

        container.find(".treegridview ").each(function () {
            $('.treegridview').treegrid();
        });

        container.find(".nestable").each(function () {
            if (!jQuery(this).hasClass("setNestabled")) {
                var obj = jQuery(this);
                var maxDepth = obj.attr("data-max-depth") || 0;
                obj.addClass("setNestabled").nestable({
                    maxDepth: maxDepth
                }).on('change', function (e) {
                    var ids = [];
                    var idTheme = obj.attr("data-theme-id");
                    var idRegion = obj.attr("data-region-id");
                    var idPage = obj.attr("data-page-id");
                    var url = obj.attr("data-url");
                    var data = obj.nestable('serialize');

                    for (var i in data) {
                        var item = data[i];
                        ids.push(item.id);
                    }
                    if (!Utils.isEmpty(url)) {
                        var dataPost = {};
                        if (obj.hasClass("regions")) {
                            dataPost = {
                                IDTheme: idTheme,
                                IDPage: idPage || 0,
                                IDRegions: JSON.stringify(ids)
                            };
                        }
                        else {
                            dataPost = {
                                IDTheme: idTheme,
                                IDRegion: idRegion,
                                IDPage: idPage || 0,
                                IDBlocks: JSON.stringify(ids)
                            };
                        }

                        jQuery.ajax({
                            type: "POST",
                            async: true,
                            url: url,
                            data: dataPost,
                            success: function (response) {
                                obj.closest(".ui-dialog").addClass("refresh");
                            }
                        });
                    }
                });
            }
        });
        container.find("select.selectpicker").each(function () {
            $(this).selectpicker();
        });
        container.find(".useSortable").each(function () {
            if (jQuery(this).hasClass("inited")) {
                jQuery(this).sortable("destroy");
            }
            jQuery(this).sortable({
                items: ".sortitem"
            });
        });
        //container.find(".autoSelect2").each(function () {
        //    $(".autoSelect2").select2();
        //});

        Utils.preventScrollOutside($("div.preventScrollOutSide"));

        var hoverpopovers = $('[data-toggle=popover-hover]');
        $.each(hoverpopovers, function () {
            $(this)
                .popover({
                    html: true,
                    template: '<div class="popover ' + $(this)
                        .data("class") +
                        '"><div class="arrow"></div><h3 class="popover-title ' +
                        $(this)
                            .data("titleclass") + '">Popover right</h3><div class="popover-content"></div></div>',
                    trigger: "hover"
                });
        });
        jQuery(document).on('click', ".append_template", function () {
            var obj = jQuery(this);
            var form = jQuery(this).closest("form");
            var table = form.find("table:first");
            var target = jQuery(obj.attr("data-target"));
            var temp = jQuery(obj.attr("data-temp"));
            var tempPlate = $(temp.html());
            // Utils.destroyValidator(table);
            target.append(tempPlate);
            Utils.updateInputDate(form);
            Utils.bootstrapValidator(table);
            Utils.autoResize();
            form.find(".selectpicker").not(".inited").selectpicker();
            Utils.updateIsNumber(form);
            Main.upEvent();
        });
     
    },
    onEvent: function () {

        jQuery(document).on("click", ".close-flash", function () {
            jQuery(this).closest(".flash").fadeOut("slow");
        });
        jQuery(document).on("click", ".closeDialog", function () {
            Utils.closeOverlay(true);
        });
        jQuery(document).on("click", ".deleteItem", function () {
            var item = jQuery(this);
            if (item.hasClass("check")) {
                var val = item.attr("data-val");
                var targetChange = item.attr("data-target-change");
                var arrayJsons = $(targetChange).attr("data-value");
                var array = JSON.parse(arrayJsons);
                var newArray = Utils.removeElInArray(val, array);
                $(targetChange).attr("data-value", JSON.stringify(newArray));
            }
            item.closest(".item").remove();
        });
        jQuery(document).on("click", ".openDialog", function () {
            var data = jQuery(this).getData();
            var dialoger = jQuery(data.target);
            var maxH = jQuery(window).height();
            if (!dialoger.hasClass("ui-dialog-content")) {
                jQuery(".ui-dialog[aria-describedby='" + dialoger.attr("id") + "']").remove();
            }
            var data_with = 600;
            if (data.width != null && data.width != "") {
                data_with = data.width;
            }
            dialoger.dialog({
                width: data.width,
                resizable: false,
                open: function () {
                    if (maxH < dialoger.height()) {
                        dialoger.css("height", maxH - 50);
                    }
                    if (typeof data.formTarget != 'undefined') {
                        dialoger.attr("data-target", data.formTarget);
                    }
                    if (typeof data.formInsertType != 'undefined') {
                        dialoger.attr("data-insert-type", data.formInsertType);
                    }
                    if (typeof data.formClass != 'undefined') {
                        dialoger.addClass(data.formClass);
                    }
                    if (dialoger.hasClass("uhf50d")) {
                        dialoger.closest(".ui-dialog").addClass("hf50d");
                    }
                    if (dialoger.hasClass("bootstrapValidator")) {
                        dialoger
                            .bootstrapValidator('disableSubmitButtons', false)
                            .bootstrapValidator('resetForm', true);
                        dialoger.reset();

                        if (dialoger.hasClass("quickSubmit") && dialoger.hasClass("bootstrapValidator")) {
                            dialoger.removeClass("bootstrapValidator").bootstrapValidator('destroy');
                            dialoger.unbind();
                        }
                    }

                    Utils.openOverlay();
                    Utils.updateFormState(dialoger);
                    Utils.updateScrollBar(dialoger);
                    Autocomplete.init(dialoger);

                    if (typeof data.id != 'undefined') {
                        dialoger.find("input[name='ID']").val(data.id);
                    }
                    if (typeof data.parentId != 'undefined') {
                        dialoger.find("input[name='Parent']").val(data.parentId);
                    }
                    if (typeof data.parentName != 'undefined') {
                        dialoger.find("input[name='ParentName']").val(data.parentName);
                    }
                },
                close: function () {
                    Utils.closeOverlay();
                }
            });
            return false;
        });
        jQuery(document).on("click", ".clickViewer", function () {
            var data = jQuery(this).getDataUppername();
            if (jQuery(this).hasClass("tabOpen")) {
                jQuery("[href='" + data.TabOpen + "']").trigger("click");
            }

            Utils.viewer(data);
            return false;
        });
        jQuery(document).on('change', '.tickAllFormGroup', function () {
            var checked = jQuery(this).is(":checked");
            jQuery(this).closest(".form-group").find(".tickItem").each(function () {
                if (!jQuery(this).prop("disabled"))
                    jQuery(this).prop("checked", checked);
            });
        });
        jQuery(document).on('change', '.tickAll', function () {
            var checked = jQuery(this).is(":checked");
            jQuery(this).closest(".tickGroup").find(".tickItem, .tickKey").each(function () {
                if (!jQuery(this).prop("disabled"))
                    jQuery(this).prop("checked", checked);
            });
        });
        jQuery(document).on('change', '.group-checkable', function () {

            var table = jQuery(this).closest("table");
            var set = table.find(".checkboxes");
            var checked = jQuery(this).is(":checked");
            jQuery(set).each(function () {
                if (checked) {
                    jQuery(this).prop("checked", true);
                    jQuery(this).closest('tr').addClass("active");
                } else {
                    jQuery(this).prop("checked", false);
                    jQuery(this).closest('tr').removeClass("active");
                }
            });
            Utils.toggleMultiTicks(table);
        });
        jQuery(document).on('change', '.checkboxes', function () {
            jQuery(this).closest('tr').toggleClass("active");
            Utils.toggleMultiTicks(jQuery(this).closest('table'));
        });
        jQuery(document).on("change", ".changeRel", function () {
            var v = jQuery(this).prop("checked") ? 1 : 0;
            var data = jQuery(this).getDataUppername();
            jQuery(data.Rel).val(v);
            if (typeof data.RelVisible != 'undefined') {
                var dataOptions = jQuery(this).find("option:selected").getDataUppername();
                if (dataOptions.IsVisible.toLowerCase() == "true") {
                    jQuery(data.RelVisible).removeClass("hidden")
                } else {
                    jQuery(data.RelVisible).addClass("hidden")
                }
            }
        });
        jQuery(".changeRel").trigger("change");
        jQuery(document).on("keyup", ".moneyFormat", function () {
            var $input = $(this);
            var value = $input.val();
            var text = $(this).siblings("small");
            var maxVal = parseFloat($input.data("max-value"));
            var btnSubmit = $input.closest("form").find("button[type='submit']");
            var maxMss = $input.data("max-value-mss");
            if (value != null && value != "") {
                value = value.replace(/[^\d]/g, "");
                var num = parseFloat(value);
                if (!isNaN(maxVal) && num >= maxVal) {
                    if (text.length == 0) {
                        text = $("<small class=\"help-block\" data-bv-for=\"" + $(this).attr('name') + "\" data-bv-result=\"VALID\" style=\"color: rgb(228, 111, 97); display: none;\"></small>");
                        $(this).closest("div").append(text);
                    }
                    text.show();
                    text.text(maxMss);
                    text.attr("data-bv-result", "INVALID");
                    $(this).parents("div.form-group").removeClass("has-success");
                    $(this).parents("div.form-group").addClass("has-error");
                    btnSubmit.prop('disabled', true);
                    return false;
                } else {
                    text.hide();
                    text.text("");
                    text.attr("data-bv-result", "VALID");
                    $(this).parents("div.form-group").removeClass("has-error");
                    $(this).parents("div.form-group").addClass("has-success");
                    btnSubmit.removeAttr("disabled");
                }
                $input.val(num.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
            else
                $input.val(0);
        });
        jQuery(document).on("change", ".fieldRadio", function () {
            var obj = jQuery(this);
            if (obj.prop("checked")) {
                var data = obj.getDataUppername();
                obj.closest("form").find(".fieldRadio").each(function () {
                    if (obj.attr("name") != jQuery(this).attr("name")) {
                        if (data.Label == jQuery(this).attr("data-label")) {
                            jQuery(this).prop("checked", false);
                        }
                    }
                });
            }
        });
        jQuery(document).on("keydown", ".pressSubmit", function (e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key === 13) {
                try {
                    jQuery(this).closest("form").trigger("submit");
                } catch (ex) {
                }
                return false;
            }
            return true;
        });
        jQuery(document).on("submit", ".quickSearch", function () {
            try {
                var form = jQuery(this);
                var url = form.attr("action");
                var target = form.attr("data-target");
                var data = Utils.getSerialize(form);
                if (Utils.isEmpty(url)) {
                    return;
                }

                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: url,
                    data: data,
                    beforeSend: function () {
                        jQuery(target).addClass("loading").html("");
                    },
                    complete: function () {
                        jQuery(target).removeClass("loading");
                    },
                    error: function () {
                        jQuery(target).removeClass("loading");
                    },
                    success: function (response) {
                        try {
                            window.history.pushState(null, response.title, url + Utils.builderQString(data));
                            jQuery(document).prop("title", response.title);
                        } catch (e) {
                            console.log(e);
                        }

                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            jQuery(target).html(response.custHTML);
                        }

                        Utils.updateInputDate(jQuery(target));
                        Utils.updateFormState(jQuery(target));
                        Utils.updateScrollBar(jQuery(target));
                        Autocomplete.init(jQuery(target));
                        Main.upEvent();
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("submit", ".quickSubmit", function (e) {
            e.preventDefault();
            try {
                var form = jQuery(this);
                if (!$(form).hasClass('submiting')) {
                    $(form).addClass('submiting');
                    var url = form.attr("action");
                    var target = form.attr("data-target");
                    var containmes = form.find('#messeage_err');
                    var targetDelete = form.attr("data-target-delete");
                    var type = form.attr("data-insert-type");
                    var data = Utils.getSerialize(form);
                    if (Utils.isEmpty(url)) {
                        $(form).removeClass('submiting');
                        return false;
                    }
                    if (!Utils.validateDataForm(form)) {
                        $(form).removeClass('submiting');
                        return false;
                    }
                    if (!form.hasClass("bootstrapValidator")) {
                        form.addClass("bootstrapValidator").bootstrapValidator();
                    }
                    var bootstrapValidator = form.data('bootstrapValidator');
                    bootstrapValidator.validate(true);
                    if (!bootstrapValidator.isValid()) {
                        $(form).removeClass('submiting');
                        return false;
                    }
                    jQuery.ajax({
                        type: "POST",
                        async: true,
                        url: url,
                        data: data,
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        error: function () {
                        },
                        success: function (response) {
                            if (!response.isErr)
                                window.location.reload();
                            Utils.sectionBuilder(response, response.isErr);
                            if (response.hasOwnProperty("isCust")) {
                                if (type == "append") {
                                    jQuery(target).append(response.htCust);
                                }
                                else if (type == "prepend") {
                                    jQuery(target).prepend(response.htCust);
                                }
                                else if (type == "replaceWith") {
                                    jQuery(target).replaceWith(response.htCust);
                                }
                                else {
                                    jQuery(target).html(response.htCust);
                                }
                            }
                            if (containmes.length > 0)
                                containmes.removeClass("hidden").text(response.ctMeg);
                            Utils.updateInputDate(jQuery(target));
                            Utils.updateFormState(jQuery(target));
                            Utils.updateScrollBar(jQuery(target));
                            Autocomplete.init(jQuery(target));
                            Main.upEvent();
                            if (!Utils.isEmpty(targetDelete)) {
                                jQuery(targetDelete).fadeOut("fast", function () {
                                    jQuery(this).remove();
                                });
                            }
                            if (form.hasClass("closeOnSubmit")) {
                                Utils.closeOverlay(true);
                            }
                            form.find("[type='submit']").prop("disabled", false);
                            $(form).removeClass('submiting');
                        }
                    });
                }
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickMore", function () {
            try {
                var node = jQuery(this);
                var data = jQuery(this).getDataUppername();
                if (typeof data.Skip == 'undefined') {
                    data.Skip = 0;
                }
                if (typeof data.Take == 'undefined') {
                    data.Take = 10;
                }
                data.Skip = parseInt(data.Skip) + parseInt(data.Take);

                var target = data.Target;
                var type = data.InsertType;
                var url = node.attr("href").replace("#", "");
                if (Utils.isEmpty(url)) {
                    return;
                }
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: url,
                    data: data,
                    beforeSend: function () {
                        node.addClass("loading");
                    },
                    complete: function () {
                        node.removeClass("loading");
                    },
                    error: function () {
                        node.removeClass("loading");
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            if (type == "prepend") {
                                jQuery(target).prepend(response.htCust);
                            }
                            else {
                                jQuery(target).append(response.htCust);
                            }
                        }
                        node.attr("data-skip", data.Skip);
                        node.attr("data-take", data.Take);
                        if (response.htCust == "" || jQuery(response.htCust).find(".itemMore").length < data.Take) {
                            node.addClass("hidden")
                        }
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickLike", function () {
            try {
                var node = jQuery(this);
                var data = jQuery(this).getDataUppername();
                var target = data.Target;
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: node.attr("href"),
                    data: data,
                    beforeSend: function () {
                        node.addClass("loading");
                    },
                    complete: function () {
                        node.removeClass("loading");
                    },
                    error: function () {
                        node.removeClass("loading");
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        node.toggleClass("active");
                        if (response.hasOwnProperty("isCust")) {
                            jQuery(target).html(response.htCust);
                        }
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickView", function () {
            try {
                var node = jQuery(this);
                var url = node.attr("href").replace("#", "");
                var target = node.attr("data-target");
                if (Utils.isEmpty(url)) {
                    return;
                }
                if (window.history.pushState) {
                    jQuery.ajax({
                        type: "POST",
                        async: true,
                        url: url,
                        data: { RedirectPath: Utils.getRedirect() },
                        beforeSend: function () {
                            jQuery(target).addClass("loading").html("");
                        },
                        complete: function () {
                            jQuery(target).removeClass("loading");
                        },
                        error: function () {
                            jQuery(target).removeClass("loading");
                        },
                        success: function (response) {
                            window.history.pushState(null, response.title, url);
                            jQuery(document).prop("title", response.title);
                            Utils.sectionBuilder(response);
                            if (response.hasOwnProperty("isCust")) {
                                jQuery(target).html(response.custHTML);
                            }
                            Utils.updatePDFViewer(response);
                            Utils.updateChart(jQuery(target));
                            Utils.updateInputDate(jQuery(target));
                            Utils.updateFormState(jQuery(target));
                            Utils.updateScrollBar(jQuery(target));
                            Autocomplete.init(jQuery(target));
                            Main.upEvent();
                            Main.backLink();
                            Cust.fileViewer_height_fn();
                            Cust.Scroll_table();
                            Cust.Scroll_tab_group();
                            Cust.Table_sort();
                            Cust.dataTables_filter_col(); //Responsive dataTables_filter Col
                        }
                    });
                } else {
                    window.location.href = url;
                }
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickUpdate", function () {
            try {
                var obj = jQuery(this);
                var target = jQuery(this).attr("data-target");
                var datas = {};
                datas.RedirectPath = Utils.getRedirect();
                datas.ID = obj.attr("data-id");
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: jQuery(this).attr("href"),
                    data: datas,
                    beforeSend: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                        jQuery("#Overlay").addClass("loadingc").html("");
                    },
                    complete: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                        jQuery("#Overlay").removeClass("loadingc").html("");
                    },
                    error: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                        jQuery("#Overlay").removeClass("loadingc").html("");

                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(target).html(response.custHTML);
                        }
                        Utils.updateTab(jQuery(target));
                        Utils.updateInputDate(jQuery(document));
                        Utils.updateFormState(jQuery(document));
                        Utils.updateScrollBar(jQuery(target));
                        Autocomplete.init(jQuery(target));
                        Main.upEvent(jQuery(target));
                        jQuery(target).find(".selectpicker").selectpicker();
                        Cust.check_required_input();
                    }
                });
            } catch (e) { }
            return false;
        });
        jQuery(document).on("click", ".quickAppend", function () {
            try {
                var obj = jQuery(this);
                var target = jQuery(this)
                    .attr("data-target");
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: jQuery(this).attr("href"),
                    beforeSend: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                    },
                    complete: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                    },
                    error: function () {
                        if (!obj.hasClass("not-overlay")) {
                            Utils.openOverlay();
                        }
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(target).append(response.htCust);
                        }
                        Utils.updateTab(jQuery(target));
                        Utils.updateInputDate(jQuery(target));
                        Utils.updateFormState(jQuery(target));
                        Utils.updateScrollBar(jQuery(target));
                        Autocomplete.init(jQuery(target));
                        Main.upEvent();
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickDelete", function () {
            try {
                var data = jQuery(this).getDataUppername();
                if (typeof data.RedirectPath == "undefined")
                    data.RedirectPath = Utils.getRedirect();

                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: jQuery(this).attr("href"),
                    data: data,
                    beforeSend: function () {
                        Utils.openOverlay();
                    },
                    complete: function () {
                        Utils.openOverlay();
                    },
                    error: function () {
                        Utils.openOverlay();
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(data.Target).html(response.custHTML);
                        }
                        if (!Utils.isEmpty(data.TargetDeleteClick)) {
                            jQuery(data.TargetDeleteClick).fadeOut("fast", function () {
                                jQuery(this).remove();
                            });
                        }
                        Utils.updateFormState(jQuery(data.Target));
                        Utils.updateScrollBar(jQuery(data.Target));
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".quickDeletes, .quickConfirms", function () {
            try {
                var target = jQuery(this)
                    .attr("data-target");
                var href = jQuery(this)
                    .attr("data-href");
                var table = jQuery(this)
                    .closest(".dataTables_wrapper")
                    .find("table");

                var ids = [];
                var idFiles = [];
                table.find(".checkboxes").each(function () {
                    if (jQuery(this).prop("checked")) {
                        var id = jQuery(this).attr("data-id");
                        var idFile = jQuery(this).attr("data-id-file");
                        if (Utils.isInteger(id))
                            ids.push(id);
                        if (!Utils.isEmpty(idFile))
                            idFiles.push(idFile);
                    }
                });
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: href,
                    data: { ID: ids, IDFile: idFiles, RedirectPath: Utils.getRedirect() },
                    beforeSend: function () {
                        Utils.openOverlay();
                    },
                    complete: function () {
                        Utils.openOverlay();
                    },
                    error: function () {
                        Utils.openOverlay();
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(target).html(response.htCust);
                        }
                        Utils.updateFormState(jQuery(target));
                        Utils.updateScrollBar(jQuery(target));
                    }
                });
            } catch (e) {

            }
            return false;
        });

        jQuery(document).on("submit", ".quickCmt", function (e) {
            try {
                var form = jQuery(this);
                var attrs = jQuery(this).getDataUppername();
                var container = form.closest(".container-cmts");
                var target = container.find(".cmts").first();
                var data = Utils.getSerialize(form);
                if (Utils.isEmpty(data.Body))
                    return false;

                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: form.attr("action"),
                    data: data,
                    beforeSend: function () {
                    },
                    complete: function () {
                        form.reset();
                    },
                    error: function () {
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(target).append(response.htCust);
                            var dataInc = jQuery(attrs.IncTarget).getData();

                            var v = parseInt(dataInc.value) + 1;
                            jQuery(attrs.IncTarget).attr("data-value", v).val(v);
                        }
                        Utils.updateFormState(jQuery(target));
                        Utils.updateScrollBar(jQuery(target));
                        jQuery(target).scrollTop(jQuery(target).scrollHeight());
                    }
                });
            } catch (e) {
                console.log(e);
            }

            return false;
        });

        jQuery(document).on("click", ".nextChart", function () {

            var chartViewer = jQuery(this).closest(".chartViewer");
            var chart = chartViewer.highcharts();
            var data = chartViewer.getData();
            var from = parseInt(data.from);
            var to = parseInt(data.to);
            var max = parseInt(data.max);
            var step = parseInt(data.step);

            chart.xAxis[0].setExtremes(from + step, to + step);
            chartViewer.attr("data-from", from + step);
            chartViewer.attr("data-to", to + step);

            if (to + step >= max) {
                chartViewer.find(".nextChart").addClass("hidden");
            } else {
                chartViewer.find(".nextChart").removeClass("hidden");
            }
        });

        jQuery(document).on("click", ".prevChart", function () {
            var chartViewer = jQuery(this).closest(".chartViewer");
            var chart = chartViewer.highcharts();
            var data = chartViewer.getData();
            var from = parseInt(data.from);
            var to = parseInt(data.to);
            var max = parseInt(data.max);
            var step = parseInt(data.step);

            chart.xAxis[0].setExtremes(from - step, to - step);
            chartViewer.attr("data-from", from - step);
            chartViewer.attr("data-to", to - step);

            if (to - step >= max) {
                chartViewer.find(".nextChart").addClass("hidden");
            } else {
                chartViewer.find(".nextChart").removeClass("hidden");
            }
        });

        jQuery(document).on("click", ".quickShow", function () {
            try {
                var obj = jQuery(this);
                var form = obj.closest("form").first();
                var target = obj.attr("data-target");
                var href = obj.attr('href');
                var data = Utils.getSerialize(form);
                if (typeof data.RedirectPath == "undefined")
                    data.RedirectPath = Utils.getRedirect();

                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: href,
                    data: data,
                    beforeSend: function () {
                        Utils.openOverlay();
                    },
                    complete: function () {
                        Utils.openOverlay();
                    },
                    error: function () {
                        Utils.openOverlay();
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(data.Target).html(response.htCust);
                        }
                        if (!Utils.isEmpty(data.TargetDeleteClick)) {
                            jQuery(data.TargetDeleteClick).fadeOut("fast", function () {
                                jQuery(this).remove();
                            });
                        }
                        Utils.updateFormState(jQuery(target));
                        Utils.updateScrollBar(jQuery(target));
                        Main.upEvent();
                    }
                });
            } catch (e) {

            }
            return false;
        });
        jQuery(document).on("click", ".tabClick", function () {
            var tab = jQuery(this);
            var dataTab = tab.attr("data-tab-name");
            var currentUrl = window.location.href;
            currentUrl = Main.updateQueryStringParameter(currentUrl, "tab", dataTab);
            window.history.pushState(null, null, currentUrl);
        });

    },
    backLink: function () {
        try {
            var bcItems = jQuery(".breadcrumb").find("li");
            var len = bcItems.length;
            if (len > 2) {

                var a = jQuery(bcItems[len - 2]).find("a");
                var attr_href = a.attr("href");
                var data_target = a.attr("data-target");
                jQuery(".widget_back_btn")
                    .removeClass("hidden")
                    .attr("href", attr_href)
                    .attr("data-target", data_target);
                if (a.hasClass("quickView")) {
                    jQuery(".widget_back_btn").addClass("quickView");
                }
                else {
                    jQuery(".widget_back_btn").removeClass("quickView");
                }
            } else {
                jQuery(".widget_back_btn").addClass("hidden");
            }
        } catch (e) { }
    },
    updateQueryStringParameter: function (uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }
};
jQuery(document).ready(function () {
    Cdata.init();
    Main.init();
    Utils.autoCloseFlash();
    Utils.updateChart(jQuery(document));
    Utils.updateFormState(jQuery(document));
    Utils.updateInputDate(jQuery(document));
    Utils.updateScrollBar(jQuery(document));
    Autocomplete.init(jQuery(document));

});