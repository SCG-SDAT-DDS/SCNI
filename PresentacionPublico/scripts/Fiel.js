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

    return pkcs7;
}

function CargarCertificados(ddlCertificados) {
    try {
        var certificados = TSigner.Certificates;

        var items = certificados.split("\n");

        ddlCertificados.append('<option value="">Seleccione...</option>');

        for (var n = 0; n < items.length - 1; n++) {
            ddlCertificados.append('<option value="' + items[n] + '">' + items[n] + '</option>');
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