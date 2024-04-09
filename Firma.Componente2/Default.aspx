<%@ Page Title="Firma" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #targetInfo {
            width: 200px;
            height: 200px;
        }
    </style>
    <script>
        jQuery(document).ready(function ($) {
            $('#tabs').tab();
        });
    </script>
    <style>
        .font-bold {
            font-weight: bold;
        }
    </style>
    <script>


        var firma = new fielnet.Firma({
            subDirectory: "Scripts",
            ajaxAsync: false,
            controller: "Controlador.ashx"
        });

        

        var certificado = "";

        var digestionArchivo = "";

        $(function () {

            //Verifica archivo

            var certValida = document.getElementById("archivoCertificado");
            certValida.onchange = function (e) {
                var readFile = new FileReader();
                readFile.onload = function (e) {
                    var dataItems = e.target.result.split("base64,");
                    if (dataItems.length == 2) {
                        certificado = dataItems[1];

                    }
                }
                readFile.readAsDataURL(certValida.files[0]);
            };


            var originalFile = document.getElementById("archivoOriginal");
            originalFile.onchange = function (e) {
                var file = e.target.files[0]
                if (file != null) {
                    firma.getFileDigest(file, 10000, fielnet.Digest.SHA1, function (data) {
    
                        document.getElementById("digestionAmparada").value = data;
                    }, function (e) { alert(e); });
                }
                else {
                    alert("No se ha seleccionado ningun archivo");
                }
            };




            //Par de llaves
            firma.readCertificate("fileCertificado");
            firma.readPrivateKey("filePrivada");

            //PFX para leer pfx
            firma.readPfx("pfx");


            var item = document.getElementById("fileCert");
            item.setAttribute("accept", ".cer");

            item.onchange = function (e) {
                var readFile = new FileReader();
                readFile.onload = function (e) {
                    var dataItems = e.target.result.split("base64,");
                    if (dataItems.length == 2) {
                        certificado = dataItems[1];

                    }
                }
                readFile.readAsDataURL(item.files[0]);
            };



            var item2 = document.getElementById("certificado");
            item2.setAttribute("accept", ".cer");

            item2.onchange = function (e) {
                var readFile = new FileReader();
                readFile.onload = function (e) {
                    var dataItems = e.target.result.split("base64,");
                    if (dataItems.length == 2) {
                        certificado = dataItems[1];

                    }
                }
                readFile.readAsDataURL(item2.files[0]);
            };

            //Para leer archivos
            document.getElementById("files").addEventListener('change', readAndReportFiles, false);
            document.getElementById("file1").addEventListener('change', readAndReportFiles, false);

            function readAndReportFiles(e) {
                var files = e.target.files;
                var objetivo = e.target.id;
                var algoritmo = 1;
                var checked = false;
                if (objetivo == "file1") {
                    checked = $("#chkFirma1").is(":checked");
                    algoritmo = $("[name='digestionArchivoPKCS7']:checked").val();
                }
                else {
                    checked = $("#chkPKCS1").is(":checked");
                    algoritmo = $("[name='digestionArchivoPKCS7SAT']:checked").val();
                }

                if (files.length > 0) {
                    var area = document.getElementById(e.target.id == "files" ? "showProgress" : "showProgress2");
                    while (area.firstChild) {
                        area.removeChild(area.firstChild);
                    }

                    for (var i = 0; i < files.length; i++) {
                        var f = files[i];
                        (function (file) {
                            var item = document.createElement("div");
                            item.style.borderBottom = "1px solid rgba(0,0,0,0.5)";
                            item.style.padding = "4px";
                            var meter = document.createElement("progress");
                            var p = document.createElement("p");
                            p.innerHTML = "<b>" + file.name + "</b> ";
                            meter.setAttribute("min", "0");
                            meter.setAttribute("max", "100");
                            p.appendChild(meter);
                            item.appendChild(p);
                            area.appendChild(item);

                            //Subir archivo 
                            if (!checked) {
                                firma.setReferencia("Prueba firma archivo pkcs7 con NOM");

                                firma.signPKCS7(file, 10000, parseInt(algoritmo), { tsa: { name: "tsa01", algorithm: fielnet.Digest.SHA1 }, nom: { name: "NA" } }, function (total) {
                                    console.log(total);
                                }, function (obj) {
                                    var divDetalles = document.createElement("div");
                                    var spanDetalles = document.createElement("span");
                                    if (obj.state == 0) {
                                        spanDetalles.innerHTML = "<ul><li>Transferencia: " + obj.transfer + "</li><li> Fecha: " + obj.date + "</li><li>Propietario: " + obj.commonName + "</li><li> Serie:  " + obj.hexSerie + "</li><li> Digestión: " + obj.digest + "</li><li>Firma:<br /><textarea style='width:100%; border:none; background-color:white;' readonly>" + obj.sign + "</textarea></li></ul>";

                                    }
                                    else {
                                        spanDetalles.innerHTML = "Error: " + obj.description;

                                    }
                                    divDetalles.appendChild(spanDetalles);
                                    item.appendChild(divDetalles);


                                }, function (error) {

                                    alert("Error leyendo archivo: " + error);
                                });
                            }
                            else {
                                firma.signFilePCKS1(file, 10000, fielnet.Digest.SHA1, function (total) {
                                    console.log(total);
                                }, function (obj) {
                                    var divDetalles = document.createElement("div");
                                    var spanDetalles = document.createElement("span");
                                    if (obj.state == 0) {
                                        spanDetalles.innerHTML = "<ul><li>Transferencia: " + obj.transfer + "</li><li> Fecha: " + obj.date + "</li><li>Propietario: " + obj.commonName + "</li><li> Serie:  " + obj.hexSerie + "</li><li> Digestión: " + obj.digest + "</li><li>Firma:<br /><textarea style='width:100%; border:none; background-color:white;' readonly>" + obj.sign + "</textarea></li></ul>";

                                    }
                                    else {
                                        spanDetalles.innerHTML = "Error: " + obj.description;

                                    }
                                    divDetalles.appendChild(spanDetalles);
                                    item.appendChild(divDetalles);


                                }, function (error) {

                                    alert("Error leyendo archivo: " + error);
                                });
                            }

                        })(f);
                    }
                }
            }

            $("[name='tipoOper']").change(function (e) {
                var valor = $(this).val();
                if (valor == 'archivo') {
                    $("#archivos").css({
                        "display": "block"
                    });
                    $("#cadena").css({
                        "display": "none"
                    });
                }
                else {
                    $("#archivos").css({
                        "display": "none"
                    });
                    $("#cadena").css({
                        "display": "block"
                    });
                }
            });

            $("[name='tipoOper3']").change(function (e) {
                var valor = $(this).val();
                if (valor == 'archivo') {
                    $("#verificaArchivo").css({
                        "display": "block"
                    });
                    $("#verificaCadena").css({
                        "display": "none"
                    });
                }
                else {
                    $("#verificaArchivo").css({
                        "display": "none"
                    });
                    $("#verificaCadena").css({
                        "display": "block"
                    });
                }
            });

            $("[name='tipoOper2']").change(function (e) {
                var valor = $(this).val();
                if (valor == 'archivo') {
                    $("#archivosPfx").css({
                        "display": "block"
                    });
                    $("#cadenaPfx").css({
                        "display": "none"
                    });
                }
                else {
                    $("#archivosPfx").css({
                        "display": "none"
                    });
                    $("#cadenaPfx").css({
                        "display": "block"
                    });
                }
            });

        });


        //Función para apertura de par de llaves
        function validateParDeLlaves() {
            var strPassword = $("#satPass").val();
            firma.validateKeyPairs(strPassword, function (data) {

                if (data.state == 0) {
                    $("#tablePardeLlaves").css({
                        "display": "none"
                    });
                    $("#opciones").css({
                        "display": "block"
                    });

                }
                else {
                    alert(data.description);
                }
            });
        }

        //Funcion para la apertura del pfx
        function openPfx() {
            var pass = $("#pfxPass").val();
            firma.openPfx(pass, function (data) {
                if (data.state == 0) {
                    $("#opciones2").css({
                        "display": "block"
                    });
                    $("#tblPfx").css({
                        "display": "none"
                    }); 
                }
                else {
                    alert(data.description);
                }
            });
        }
        function signText(strId) {
            var strToSign = $("#" + strId);
            var codificacion = "";
            var tipoDigestion = "";


            if (strId == 'strToSign') {
                codificacion = $("[name='tipoCodificacion']:checked").val();
                tipoDigestion = $("[name='tipoDigestion']:checked").val();

                 bNom = $("[id*='satNom']").is(":checked");
                 bTsa = $("[id*='satTsa']").is(":checked");

            }
            else {
                codificacion = $("[name='tipoCodificacionPfx']:checked").val();
                tipoDigestion = $("[name='tipoDigestionPfx']:checked").val();
                bNom = $("[id*='pfxNom']").is(":checked");
                bTsa = $("[id*='pfxTsa']").is(":checked");

            }

            firma.setReferencia("Prueba firma pkcs1");
            firma.signPKCS1(strToSign.val(), parseInt(tipoDigestion), parseInt(codificacion), { tsa: { name: "tsa01", algorithm: fielnet.Digest.SHA1 }, nom: { name: "NA" } }, function (data) {
                    if (data.state == 0) {
                        var modal = $("#modal");
                            $("#contenidoModal").html("<textarea style='border:none; background-color:white; width:100%;' disabled>Firma: " + data.sign + "</textarea> <p>transferencia: " + data.transfer + "</p><p> fecha de procedimiento " + data.date + "</p><p> Evidencia: " + data.evidence + "</p><p> Serie: " + data.hexSerie + " </p><p>Propietario: " + data.commonName + "</p>");
                        modal.modal();
                    }
                    else {
                        alert(data.description);
                    }

            });

            firma.setReferencia("Prueba firma pkcs1")
            firma.signPKCS1("Zbj7kMu+H3utH+lQqeWtwIZNJXs=", 2, 2, { tsa: { name: "tsa01", algorithm: fielnet.Digest.SHA1 }, nom: { name: "NA" } }, function (data) {
                if (data.state == 0) {
                    //var modal = $("#modal");
                    alert("<textarea style='border:none; background-color:white; width:100%;' disabled>Firma: " + data.sign + "</textarea> <p>transferencia: " + data.transfer + "</p><p> fecha de procedimiento " + data.date + "</p><p> Evidencia: " + data.evidence + "</p><p> Serie: " + data.hexSerie + " </p><p>Propietario: " + data.commonName + "</p>");
                    //modal.modal();
                }
                else {
                    alert(data.description);
                    alert('asdsadsad');
                }

            });
         
        }

        function decodeCert() {
            if (certificado != null) {
                var selecionado = document.getElementById("chkValidateOCSP").checked;
                firma.setReferencia("Prueba decodificar certificado: "+(selecionado==true?"con OCSP":"sin OCSP"));
                firma.decodeCertificate(certificado, selecionado, { tsa: { name: "tsa01", algorithm: fielnet.Digest.SHA1 } }, function (cert) {
                    if (cert.state == 0) {

                        var hexSerie = cert.hexSerie;
                        var fechaInicio = cert.notBefore;
                        var fechaFin = cert.notAfter;
                        var subjectNombre = cert.subjectName;
                        var subjectCorreo = cert.subjectEmail;
                        var subjectOrganizacion = cert.subjectOrganization;
                        var subjectDepartamento = cert.subjectDepartament;
                        var subjectEstado = cert.subjectState;
                        var subjectPais = cert.subjectCountry;
                        var subjectRfc = cert.subjectRFC;
                        var subjectCurp = cert.subjectCURP;
                        var issuerName = cert.issuerName;
                        var issuerCorreo = cert.issuerEmail;
                        var issuerOrganizacion = cert.issuerOrganization;
                        var issuerDepartamento = cert.issuerDepartament;
                        var issuerEstado = cert.issuerState;
                        var issuerPais = cert.issuerCountry;
                        var issuerRfc = cert.issuerRFC;
                        var issuerCurp = cert.issuerCURP;
                        var llavePublica = cert.publicKey;
                        var strTable = "<table class='table table-hover table-stripped'><tr><td>Serie Hexadicimal: " + hexSerie + " </td></tr><tr><td>Fecha inicio: " + fechaInicio + " </td></tr><tr><td>Fecha fin: " + fechaFin + " </td></tr><tr><td>Propietario : " + subjectNombre + " </td></tr><tr><td>Correo: " + subjectCorreo + " </td></tr><tr><td>Organización :" + subjectOrganizacion + " </td></tr><tr><td>Departamento: " + subjectDepartamento + " </td></tr><tr><td>Pais :" + subjectPais + " </td></tr><tr><td>Estado: " + subjectEstado + " </td></tr><tr><td>Rfc: " + subjectRfc + " </td></tr><tr><td>Curp:" + subjectCurp + " </td></tr><tr><td>Emisor : " + issuerName + " </td></tr><tr><td>Correo: " + issuerCorreo + " </td></tr><tr><td>Organización:" + issuerOrganizacion + " </td></tr><tr><td>Departamento:" + issuerDepartamento + " </td></tr><tr><td>Pais: " + issuerPais + " </td></tr><tr><td>Estado: " + issuerEstado + " </td></tr><tr><td>Rfc: " + issuerRfc + " </td></tr><tr><td>CURP: " + issuerCurp + " </td></tr><tr><td>LLave publica: " + llavePublica + " </td></tr></table>";
                        $("#contenidoModal").html(strTable);
                        $("#modal").modal();
                    }
                    else {
                        alert(cert.description);
                    }
                });
            }
            else {
                alert("Primero seleccione su certificado");
            }
        }
        function verifica() {
            var codificacion = $("[name*='tipoCodificacionValida']:checked").val();
            firma.verifySign(document.getElementById("txtCadenaOriginal").value, document.getElementById("txtFirma").value,certificado,codificacion, function (objData) {
                if (objData.state == 0) {
                    alert("Firma validada satisfactoriamente");
                }
                else {
                    alert(objData.description);
                }
            });

        }
        function validaFirmaArchivo() {
          
            firma.verifySign(document.getElementById("digestionAmparada").value, document.getElementById("firmaDigitalArchivo").value, certificado, fielnet.Encoding.B64, function (data) {
    
                if (data.state == 0) {
                    alert("Firma validada satisfactoriamente");
                }
                else {
                    alert(data.description);
                }
            });

        }
        
    </script>

    <script>

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <ul class="nav nav-tabs" id="tabs">
        <li><a href="#decodeCert1" data-toggle="tab" class="active">Decodifica Certificado</a></li>
        <li><a href="#firmaSat" data-toggle="tab">Firma par de llaves</a></li>
        <li><a href="#firmaPfx" data-toggle="tab">Firma PFX</a></li>
        <li><a href="#verificaFirma" data-toggle="tab">Verifica Firma</a></li>
        <!-- <li><a href="#decodifica" data-toggle="tab">Decodifica PKCS7</a></li> -->

    </ul>
    <div id="my-tab-content" class="tab-content">
        <div class="tab-pane active" id="decodeCert1">
            <header>
                <h1>Decodifica certificado</h1>
                <table class="table">
                    <tr>
                        <td>Seleccione certificado</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="file" id="fileCert" /></td>
                    </tr>
                    <tr>
                        <td>Validar OCSP
                            <input type="checkbox" id="chkValidateOCSP" /></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" class="btn btn-primary" value="Decodificar Certificado" onclick="decodeCert()" /></td>
                    </tr>
                </table>
            </header>
        </div>
        <div class="tab-pane" id="firmaSat">
            <header>
                <h1>Firma usando par de llaves</h1>
            </header>
            <table class="table" id="tablePardeLlaves">
                <tr>
                    <td class="font-bold">Seleccione su Certificado</td>
                </tr>
                <tr>
                    <td>
                        <input type="file" id="fileCertificado" /></td>
                </tr>
                <tr>
                    <td class="font-bold">Seleccione su llave privada</td>
                </tr>
                <tr>
                    <td>
                        <input type="file" id="filePrivada" /></td>
                </tr>
                <tr>
                    <td class="font-bold">Ingrese la frase de acceso</td>
                </tr>
                <tr>
                    <td>
                        <input type="password" id="satPass" class="form-control" /></td>
                </tr>
                <tr class="text-right">
                    <td>
                        <input type="button" value="Acceder" class="btn btn-primary" onclick="validateParDeLlaves()" /></td>
                </tr>
            </table>
            <div id="opciones">
                <div id="tipoOperacion">
                    <label for="pkcs1">Firma de cadena</label>
                    <input type="radio" name="tipoOper" value="pkcs1" id="pkcs1" checked />
                    <label for="archivo">Firma de archivos</label>
                    <input type="radio" name="tipoOper" value="archivo" id="archivo" />
                </div>
                <div id="cadena">
                    <br />
                    <table class="table">
                          <tr>
                    <td><span style="font-weight:bold;">Generar</span>: NOM <input type="checkbox" value="nom" id="satNom"  /> Tsa  <input type="checkbox" value="nom" id="satTsa" /></td>

                        </tr>
                        <tr>
                            <td>

                                <label for="utf8Cadena">UTF-8</label>
                                <input type="radio" name="tipoCodificacion" value="2" id="utf8Cadena" checked />
                                <label for="b64Cadena">Cadena base 64</label>
                                <input type="radio" name="tipoCodificacion" value="3" id="b64Cadena" />

                            </td>
                        </tr>
                        <tr>
                            <td>

                                <label for="md5">MD5</label>
                                <input type="radio" name="tipoDigestion" value="1" id="md5" />
                                <label for="sha1">SHA1</label>
                                <input type="radio" name="tipoDigestion" value="2" id="sha1" checked />
                                <label for="sha256">Sha256</label>
                                <input type="radio" name="tipoDigestion" value="3" id="sha256" />
                                 <label for="sha512">Sha512</label>
                                <input type="radio" name="tipoDigestion" value="4" id="sha512" />

                            </td>
                        </tr>

                        <tr>
                            <td class="font-bold">Ingrese la cadena a firmar</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="strToSign" class="form-control" /></td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <input type="button" class="btn btn-primary" onclick="signText('strToSign')" value="Firmar Cadena" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="archivos">
                    <table class="table">

                        <tr>
                            <td class="font-bold">Firmar como PKCS1
                                <input type="checkbox" id="chkPKCS1"/></td>
                        </tr>
                        <tr>
                            <td class="font-bold">Digestión: MD5 <input type="radio" name="digestionArchivoPKCS7SAT" value="1" /> SHA1 <input type="radio" name="digestionArchivoPKCS7SAT" value="2" /> SHA256 <input type="radio" name="digestionArchivoPKCS7SAT" value="3" checked /> SHA512 <input type="radio" name="digestionArchivoPKCS7SAT" value="4" checked /></td>
                        </tr>
                        <tr>
                            <td class="font-bold">Seleccione sus archivos a firmar
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="file" id="files" name="files" multiple />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <!--  <input type="button" value="Firmar archivos" class="btn btn-primary" onclick="firmaArchivos()" /> -->
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="showProgress">
                </div>
            </div>

        </div>

        <div class="tab-pane" id="firmaPfx">
            <header>
                <h1>Firma usando encapsulado</h1>
            </header>
            <table class="table" id="tblPfx">
                <tr>
                    <td class="font-bold">Seleccione su encapsulado PFX</td>
                </tr>
                <tr>
                    <td>
                        <input type="file" id="pfx" /></td>
                </tr>
                <tr>
                    <td class="font-bold">Ingrese la frase de acceso</td>
                </tr>
                <tr>
                    <td>
                        <input type="password" id="pfxPass" class="form-control" /></td>
                </tr>
                <tr>
                    <td class="text-right">
                        <input type="button" value="Acceder" class="btn btn-primary" onclick="openPfx()" /></td>
                </tr>
            </table>

            <div id="opciones2">
                <div id="tipoOperacion2">
                    <label for="Radio1">Firma de cadena</label>
                    <input type="radio" name="tipoOper2" value="pkcs1" id="Radio1" checked />
                    <label for="Radio2">Firma de archivos</label>
                    <input type="radio" name="tipoOper2" value="archivo" id="Radio2" />
                </div>
                <div id="cadenaPfx">
                    <table class="table">
                        <tr>
                    <td><span style="font-weight:bold;">Generar</span>: NOM <input type="checkbox" value="nom" id="pfxNom"  /> Tsa  <input type="checkbox" value="nom" id="pfxTsa" /></td>

                        </tr>
                        <tr>
                            <td>

                                <label for="utf8CadenaPfx">UTF-8</label>
                                <input type="radio" name="tipoCodificacionPfx" value="2" id="utf8CadenaPfx" checked />
                                <label for="b64CadenaPfx">Cadena base 64</label>
                                <input type="radio" name="tipoCodificacionPfx" value="3" id="b64CadenaPfx" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="md5Pfx">MD5</label>
                                <input type="radio" name="tipoDigestionPfx" value="1" id="md5Pfx" />
                                <label for="sha1">SHA1</label>
                                <input type="radio" name="tipoDigestionPfx" value="2" id="sha1Pfx" checked />
                                <label for="sha256Pfx">Sha256</label>
                                <input type="radio" name="tipoDigestionPfx" value="3" id="sha256Pfx" />
                                 <label for="sha512Pfx">Sha512</label>
                                <input type="radio" name="tipoDigestionPfx" value="4" id="sha512Pfx" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font-bold">Ingrese la cadena a firmar</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" id="textPfx" class="form-control" /></td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <input type="button" class="btn btn-primary" onclick="signText('textPfx')" value="Firmar Cadena" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="archivosPfx">
                    <table class="table">
                        <tr>
                            <td class="font-bold">Firmar como PKCS1
                                <input type="checkbox" id="chkFirma1" /></td>
                            <td>Digestión: MD5 <input type="radio" name="digestionArchivoPKCS7" value="1" /> SHA1 <input type="radio" name="digestionArchivoPKCS7" value="2" /> SHA256 <input type="radio" name="digestionArchivoPKCS7" value="3"  checked/> SHA512<input type="radio" name="digestionArchivoPKCS7" value="4"  checked/></td>
                        </tr>
                        <tr>
                            <td class="font-bold">Seleccione sus archivos a firmar
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="file" id="file1" name="files" multiple />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <!--  <input type="button" value="Firmar archivos" class="btn btn-primary" onclick="firmaArchivos()" /> -->
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="showProgress2">
                </div>
            </div>




        </div>

        <div class="tab-pane" id="verificaFirma">
            <header>
                <h1>Verificación de firma</h1>
            </header>
            <div id="Div1">
                <label for="Radio3">Verifica firma de cadena</label>
                <input type="radio" name="tipoOper3" value="cadena" id="Radio3" checked />
                <label for="Radio4">Verifica firma de archivos </label>
                <input type="radio" name="tipoOper3" value="archivo" id="Radio4" />
            </div>
            <div id="verificaCadena">
                <table class="table">
                    <tr>
                        <td>

                            
                                <label for="Radio5">UTF-8</label>
                                <input type="radio" name="tipoCodificacionValida" value="2" id="Radio5" checked />
                                <label for="Radio6">Cadena base 64</label>
                                <input type="radio" name="tipoCodificacionValida" value="3" id="Radio6" />
                        

                        </td>
                    </tr>
                    <tr>
                        <td>Seleccione su certificado</td>

                    </tr>
                    <tr>
                        <td>
                            <input type="file" id="certificado" /></td>
                    </tr>
                    <tr>
                        <td>Ingrese cadena original
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <textarea id="txtCadenaOriginal" class="form-control"></textarea></td>
                    </tr>
                    <tr>
                        <td>Ingrese firma</td>
                    </tr>
                    <tr>
                        <td>
                            <textarea id="txtFirma" class="form-control"></textarea></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="Verificar" class="btn btn-primary" onclick="verifica()" /></td>
                    </tr>
                </table>

            </div>
            <div id="verificaArchivo">
                <table class="table">
                    <tr>
                        <td>Seleccione Certificado</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="file" id="archivoCertificado" /></td>
                    </tr>
                    <tr>
                        <td>Seleccione archivo original</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="file" id="archivoOriginal" /></td>
                    </tr>
                    <tr>
                        <td><input type="text" id="digestionAmparada" readonly class="form-control"/></td>
                    </tr>
                    <tr>
                        <td>Ingrese firma digital</td>
                    </tr>
                    <tr>
                        <td>
                            <textarea id="firmaDigitalArchivo" class="form-control"></textarea></td>
                    </tr>
                    <tr>
                        <td class="te">
                            <input type="button" value="Validar" class="btn btn-primary" onclick="validaFirmaArchivo()" /></td>
                    </tr>
                </table>
            </div>

        </div>


        <div class="tab-pane" id="decodifica">
            <header>
                <h1>Decodifica PKCS7</h1>
            </header>
            <div id="Div5">
                <table class="table">
                    <tr>
                        <td>Seleccione su archivo PKCS7</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="file" id="fileDecodifica" /></td>
                    </tr>

                </table>
                <div id="targetInfo">
                </div>
            </div>

        </div>

    </div>



    <div class="modal fade" id="modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Certificado</h4>
                </div>
                <div class="modal-body">
                    <div id="contenidoModal">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
</asp:Content>

