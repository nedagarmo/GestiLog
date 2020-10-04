var flat = false;
var $el = {};

function load_tek_uploader(command, title, query, server, source, entity) {
    $("#uploaderbox-title").html(title + " - Administrador de Archivos");

    var initialData = {};

    $.ajax({
        url: query,
        data: 'command=' + command + '&entity=' + entity + '&source=' + source,
        type: "POST",
        async: false,
        success: function (data) {
            initialData = data;
        }
    });

    $el = $("#iuploader"), initPlugin = function () {
        $el.fileinput({
            language: "es",
            uploadUrl: server + '/?command=' + command + '&entity=' + entity + '&source=' + source,
            uploadAsync: true,
            maxFileCount: 5,
            initialPreviewAsData: true,
            overwriteInitial: false,
            initialPreview: initialData.initialPreview,
            initialPreviewConfig: initialData.initialPreviewConfig,
            previewFileIcon: '<i class="fa fa-file"></i>',
            preferIconicPreview: true, // this will force thumbnails to display icons for following file extensions
            previewFileIconSettings: { // configure your icon file extensions
                'doc': '<i class="fa fa-file-word-o text-primary"></i>',
                'xls': '<i class="fa fa-file-excel-o text-success"></i>',
                'ppt': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
                'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
                'zip': '<i class="fa fa-file-archive-o text-muted"></i>',
                'htm': '<i class="fa fa-file-code-o text-info"></i>',
                'txt': '<i class="fa fa-file-text-o text-info"></i>',
                'mov': '<i class="fa fa-file-movie-o text-warning"></i>',
                'mp3': '<i class="fa fa-file-audio-o text-warning"></i>',
                // note for these file types below no extension determination logic
                // has been configured (the keys itself will be used as extensions)
                'jpg': '<i class="fa fa-file-photo-o text-danger"></i>',
                'gif': '<i class="fa fa-file-photo-o text-warning"></i>',
                'png': '<i class="fa fa-file-photo-o text-primary"></i>'
            },
            previewFileExtSettings: { // configure the logic for determining icon file extensions
                'doc': function (ext) {
                    return ext.match(/(doc|docx)$/i);
                },
                'xls': function (ext) {
                    return ext.match(/(xls|xlsx)$/i);
                },
                'ppt': function (ext) {
                    return ext.match(/(ppt|pptx)$/i);
                },
                'zip': function (ext) {
                    return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
                },
                'htm': function (ext) {
                    return ext.match(/(htm|html)$/i);
                },
                'txt': function (ext) {
                    return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
                },
                'mov': function (ext) {
                    return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
                },
                'mp3': function (ext) {
                    return ext.match(/(mp3|wav)$/i);
                },
            }
        });
    };

    if (flat) {
        $el.fileinput('destroy');
    }
    
    initPlugin();
    flat = true;

    $("#modal-uploader-box").modal('show');
}

function send_notice(query, notice, entity, source, module)
{
    $("#iloading").show();
    $.ajax({
        url: query,
        data: 'notice=' + notice + '&entity=' + entity + '&source=' + source + '&module=' + module,
        type: "POST",
        success: function (data) {
            if (data == 1)
            {
                alert("Se ha enviado correctamente el comunicado.");
                var counter = $("#counter-" + notice).html();
                counter++;
                $("#counter-" + notice).html(counter);
            }
            else
            {
                alert("No se ha podido enviar el comunicado.");
            }

            $("#iloading").hide();
        }
    });
}

function save_selected_contacts(query, entity, source)
{
    $("#cloading").show();
    var contacts = "";

    $(".contact_check:checked").each(function () {
        contacts += "," + $(this).attr("name");
    });

    $.ajax({
        url: query,
        data: 'contacts=' + contacts.substring(1) + '&entity=' + entity + '&source=' + source,
        type: "POST",
        success: function (data) {
            if (data == 1) {
                $("#cloading").hide();
                $("#modal-contacts-box").modal('hide');
                $(".calert_notice").hide();
            }
            else
            {
                $("#cloading").hide();
                alert("No se ha podido guardar la información.  Por favor, inténtelo de nuevo!");
            }
        }
    });
}

$(document).ready(function () {
    $('.itouppercase').keyup(function () {
        this.value = this.value.toUpperCase();
    });

    $('.inumeric').keyup(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
});

function dropdownlist_cascading(query, from, to)
{
    $.getJSON(query, { country : $('#' + from).val() }, function (data) {
        var items = "";
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
        });
        $('#' + to).html(items);
    });
}