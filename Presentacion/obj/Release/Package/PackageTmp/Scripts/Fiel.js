
var firma = new fielnet.Firma({
    subDirectory: "../../Scripts/Firma",
    ajaxAsync: false,
    controller: "../../Scripts/firma/Controlador.ashx"
});

function Firmar(nombreCertificado, strDigestion) {
    var firma = FirmarDigerido(nombreCertificado, strDigestion);

    if (ClicEnCancelar(firma))
        return false;

    return firma;
}

function ObtenerCertificadoPfx(nombreCertificado) {
    var strCertificado = TSigner.GetCertificate("MY", nombreCertificado);

    strCertificado = strCertificado.replace("-----BEGIN CERTIFICATE-----\r\n", "");
    strCertificado = strCertificado.replace("\r\n-----END CERTIFICATE-----", "");

    return strCertificado;
}

function FirmarDigerido(nombreCertificado, strDigestion) {
    var pkcs7 = TSigner.CreatePkcs7SignDigest(strDigestion, nombreCertificado);

    return LimpiarPkcs7(pkcs7);
}

function LimpiarPkcs7(pkcs7) {
    var index = pkcs7.lastIndexOf("=");

    if (index > 0) {
        pkcs7 = pkcs7.substr(0, pkcs7.lastIndexOf("=") + 1);
    }

    var regCaracterFinal = new RegExp("\\r\\n[a-z0-9A-Z]$");

    var finalIncorrecto = regCaracterFinal.test(pkcs7);

    if (finalIncorrecto) {
        pkcs7 = pkcs7.substring(0, pkcs7.length - 1);
    }

    return pkcs7;
}

function CargarCertificados(ddlCertificados, serieCertificado) {
    try {
        var certificados = TSigner.Certificates;

        var items = certificados.split("\n");

        ddlCertificados.append('<option value="">Seleccione...</option>');

        for (var n = 0; n < items.length - 1; n++) {
            if (items[n].indexOf(serieCertificado) >= 0) {
                ddlCertificados.append('<option value="' + items[n] + '" selected>' + items[n] + '</option>');
            } else {
                ddlCertificados.append('<option value="' + items[n] + '">' + items[n] + '</option>');
            }
        }
    } catch (err) {
        ddlCertificados.html('<option value="">' + 'Error al cargar los certificados: ' + err.message + '</option>');
    }
}

function ClicEnCancelar(pkcs7) {
    if (pkcs7.localeCompare("::-2") == 0)
        return true;
    else
        return false;
}