function CallInputFile(id) {
    $(document).on('ready', function () {
        $(id).fileinput({
            language: 'es',
            showPreview: false,
            browseLabel: 'Examinar',
            removeLabel: '',
            previewFileType: "text",
            allowedFileExtensions: ["jpg", "png"],
            previewClass: "bg-warning",
            showRemove: true,
            showUpload: false,
            maxFileSize: 1000
        });
    });
}

function CallCertificateFile(id) {
    $(document).on('ready', function () {
        $(id).fileinput({
            language: 'es',
            showPreview: false,
            browseLabel: 'Examinar',
            removeLabel: '',
            previewFileType: "text",
            allowedFileExtensions: ["cer"],
            previewClass: "bg-warning",
            showRemove: true,
            showUpload: false
        });
    });
}