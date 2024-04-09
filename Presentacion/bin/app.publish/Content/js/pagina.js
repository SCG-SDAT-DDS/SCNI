function mostrarMensaje(mensaje) {
    alert(mensaje);
}

function mostrarAlert(mensaje, clase, error) {
    $('#alert').empty();
    var iconoClase = error ? "fa-warning" : "fa-check";
    var content = "<div class='text-center alert alert-dismissable alert-" + clase + "'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa " + iconoClase + "'></i> " + mensaje + "</div>";
    $(content).hide().appendTo('#alert').slideDown(1000);

    $("#alert").fadeTo(5000, 500).slideUp(500, function () {
        $("#alert").alert('close');
    });
}

function mostrarAlertEnModal(mensaje, clase) {
    $('#alert').empty();
    var content = "<div class='text-center alert alert-dismissable alert-" + clase + "'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + mensaje + "</div>";
    $(content).hide().appendTo('#alertModal').slideDown(1000);
    
    $("#alertModal").fadeTo(5000, 500).slideUp(500, function () {
        $("#alertModal").alert('close');
    });
}

function mostrarAlertDivEspecifico(idControl, mensaje, clase, iconoClase) {
    $('#' + idControl).empty();
    var content = "<div class='text-center alert alert-dismissable alert-" + clase + "'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa " + iconoClase + "'></i> " + mensaje + "</div>";
    $(content).hide().appendTo('#' + idControl).slideDown(1000);

    $('#' + idControl).fadeTo(5000, 500).slideUp(500, function () {
        $('#' + idControl).alert('close');
    });
}

function ConvertirFecha(fecha) {
    var fechaSplit = fecha.split('/');

    var nueva = new Date(fechaSplit[2], fechaSplit[1] - 1, fechaSplit[0]);
    var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
    return fechaSplit[0] + " / " + monthNames[nueva.getMonth()] + " / " + fechaSplit[2];
}

function mostrarAlertaCamposVaciosEnTabla(mensaje, clase) {
    var content = "<div class='alert alert-dismissable alert-" + clase + "'><button type='button' class='close' data-dismiss='alert'>x</button>" + mensaje + "</div>";
    $(content).hide().appendTo('#alertCamposVaciosEnTabla').fadeIn(1000);
}

function ConvertirFecha(fecha) {
    var fechaSplit = fecha.split('/');
    var nueva = new Date(fechaSplit[2], fechaSplit[1] - 1, fechaSplit[0]);
    var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
    return fechaSplit[0] + "/" + monthNames[nueva.getMonth()] + "/" + fechaSplit[2];
}

function soloNumeros(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function ObtenerParametro(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

String.prototype.format = function (o) {
    return this.replace(/{([^{}]*)}/g,
        function (a, b) {
            var r = o[b];
            return typeof r === 'string' || typeof r === 'number' ? r : a;
        }
    );
};

function MostrarDivCargando(texto) {
    var textoMostrar = texto != null ? texto : "Cargando...";
    var cargandoHtml = "<div id='divCargandoElementos'><img src='/Content/img/gears-anim.gif' alt='Cargando...'/><br /><label class='h1 font-white'>"
        + textoMostrar + "</label></div>";

    $("#divCargando").show();
    $("#divCargando").html(cargandoHtml);
}

function CerrarDivCargando() {
    $("#divCargando").hide();
    $("#divCargando").html("");
}

function MostrarMensajeCargando($control) {
    $control.after("<div id='msjCargando'>Cargando...</div>");
}

function CerrarMensajeCargando() {
    $("#msjCargando").remove();
}


function iniciarContadorText($text, $contador, max) {
    $text.keypress(function () {
        contarLetras($text, $contador, max);
    });

    $text.keyup(function () {
        contarLetras($text, $contador, max);
    });

    $text.change(function () {
        contarLetras($text, $contador, max);
    });
}

function contarLetras($text, $contador, max) {
    $contador.html("0/" + max);
    $contador.html($text.val().length + "/" + max);

    if (parseInt($text.val().length) > max) {
        $text.val($text.val().substring(0, max));
        $contador.html(max + "/" + max);
    }
}

function log(imprimir) {
    console.log(imprimir);
}

var ObtenerEnMoneda = function (cadena) {
    return cadena.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
};

function DeshabilitarDdl($control) {
    $control.empty();
    $control.attr("disabled", "disabled");
    $control.addClass("aspNetDisabled");
}

function CargarDdl($control, lista) {
    $control.removeAttr("disabled");
    $control.empty();

    $control.append($('<option></option>').val("").text("Seleccione..."));
    lista.forEach(function (objeto) {
        $control.append($('<option></option>').val(objeto.Value).text(objeto.Text));
    });
}

var habilitado = (function () {
    function obtenerHabilitado() {
        return $("#cbxHabilitados").is(":checked");
    }
    return {
        activado: obtenerHabilitado,
    };

})();