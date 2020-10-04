CKEDITOR.plugins.add('ifield', {
    icons: 'ifield',
    init: function (editor) {
        editor.addCommand('ifield', new CKEDITOR.dialogCommand('ifieldDialog'));
        editor.ui.addButton('Ifield', {
            label: 'Insertar Campo',
            command: 'ifield',
            toolbar: 'tek'
        });

        CKEDITOR.dialog.add('ifieldDialog', this.path + 'dialogs/ifield.js');
    }
});