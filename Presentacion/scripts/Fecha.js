var meses       = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
var abvMeses    = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];
var nombreDias  = ["Domingo", "Lunes", "Martes", "Mi&eacute;rcoles", "Jueves", "Viernes", "S&aacute;bado"];
var abvDias     = ["Dom", "Lun", "Mar", "Mi&eacute;", "Juv", "Vie", "S&aacute;b"];
var minDias     = ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "S&aacute;"];

function InicializarControlesConFecha(listaControles) {

    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var ok = true;
        try {
            $.datepicker.parseDate('dd/mm/yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });

    $.each(listaControles, function (index, value) {
        value.datepicker();
    });

    $.datepicker.regional["es"] = {
        viewMode: 2,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        closeText: "Cerrar",
        prevText: "&laquo;",
        nextText: "&raquo;",
        currentText: "Hoy",
        monthNames: meses,
        monthNamesShort: abvMeses,
        dayNames: nombreDias,
        dayNamesShort: abvDias,
        dayNamesMin: minDias,
        weekHeader: "Sm",
        dateFormat: "dd/mm/yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: "",
        maxDate: 0
    };
    $.datepicker.setDefaults($.datepicker.regional["es"]);
}