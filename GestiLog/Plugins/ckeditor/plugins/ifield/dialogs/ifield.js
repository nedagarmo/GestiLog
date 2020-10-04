var entities = [];

switch (imodule.toUpperCase()) {
    // Importación Marítima
    case '1E4CAD29-F6C1-4C9B-99C8-06B65E8464D8':
        entities.push('mbl');
        entities.push('hbl');
        break;
    // Importación Aérea
    case '32916188-AD05-4BCB-A89B-467260729CB5':
        entities.push('mawb');
        entities.push('hawb');
        break;
    default:
        break;
}

CKEDITOR.dialog.add('ifieldDialog', function (editor) {
    return {
        title: 'GestiLog: Insertar Campo',
        minWidth: 400,
        minHeight: 200,
        contents: [
            {
                id: 'tab-mbl',
                label: entities[0].toUpperCase(),
                elements: [
                    {
                        type: 'select',
                        id: 'field',
                        label: 'Campos ' + entities[0].toUpperCase(),
                        items: []
                    }
                ]
            },
            {
                id: 'tab-hbl',
                label: entities[1].toUpperCase(),
                elements: [
                    {
                        type: 'select',
                        id: 'field',
                        label: 'Campos ' + entities[1].toUpperCase(),
                        items: []
                    }
                ]
            }
        ],
        onShow: function () {
            var fieldsListMBL = {};
            var fieldsListHBL = {};

            $.ajax({
                url: icontext + "/Tools/GetFields",
                data: 'entity=' + entities[0],
                type: "POST",
                async: false,
                success: function (data) {
                    fieldsListMBL = data;
                }
            });

            $.ajax({
                url: icontext + "/Tools/GetFields",
                data: 'entity=' + entities[1],
                type: "POST",
                async: false,
                success: function (data) {
                    fieldsListHBL = data;
                }
            });

            var dialog = this;

            var select_mbl = dialog.getContentElement("tab-mbl", "field");
            select_mbl.clear();
            fieldsListMBL.forEach(function (field) {
                select_mbl.add(field.label, field.value);
            });

            var select_hbl = dialog.getContentElement("tab-hbl", "field");
            select_hbl.clear();
            fieldsListHBL.forEach(function (field) {
                select_hbl.add(field.label, field.value);
            });
        },
        onOk: function () {
            var dialog = this;

            var element = editor.document.createElement('i');
            element.setAttribute('title', '@' + dialog.getValueOf(dialog.definition.dialog._.currentTabId, 'field'));
            element.setText('@' + dialog.getValueOf(dialog.definition.dialog._.currentTabId, 'field'));
            element.setAttribute('id', dialog.getValueOf(dialog.definition.dialog._.currentTabId, 'field'));

            editor.insertElement(element);
        }
    };
});