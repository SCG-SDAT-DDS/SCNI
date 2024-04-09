//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile el proyecto de Visual Studio.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExpresionesRegulares {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExpresionesRegulares() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.ExpresionesRegulares", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Invalida la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z0-9\. ]+.
        /// </summary>
        internal static string Alfanumerico {
            get {
                return ResourceManager.GetString("Alfanumerico", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z0-9ñáéíóúüÑÁÉÍÓÚÜ\.\-_]+.
        /// </summary>
        internal static string AlfanumericoGuiones {
            get {
                return ResourceManager.GetString("AlfanumericoGuiones", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^\d{4}$.
        /// </summary>
        internal static string Anio {
            get {
                return ResourceManager.GetString("Anio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [0-9]{5}$.
        /// </summary>
        internal static string CodigoPostal {
            get {
                return ResourceManager.GetString("CodigoPostal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$.
        /// </summary>
        internal static string CodigoSeguridad {
            get {
                return ResourceManager.GetString("CodigoSeguridad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z0-9\&quot;\&apos;\. ÑñáéíóúüÁÉÍÓÚÜ]{1,80}$.
        /// </summary>
        internal static string ColoniaNombre {
            get {
                return ResourceManager.GetString("ColoniaNombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a #([a-fA-F0-9]{6}).
        /// </summary>
        internal static string ColorRgb {
            get {
                return ResourceManager.GetString("ColorRgb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(?!.*(.)\1{3})((?=.*[\d])(?=.*[a-z])(?=.*[A-Z])|(?=.*[a-z])(?=.*[A-Z])(?=.*[^\w\d\s])|(?=.*[\d])(?=.*[A-Z])(?=.*[^\w\d\s])|(?=.*[\d])(?=.*[a-z])(?=.*[^\w\d\s])).{7,30}$.
        /// </summary>
        internal static string Contrasena {
            get {
                return ResourceManager.GetString("Contrasena", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(?=.{5,50}$)\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$.
        /// </summary>
        internal static string CorreoElectronico {
            get {
                return ResourceManager.GetString("CorreoElectronico", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^\$ [0-9]+(\.[0-9]{1,2})?$.
        /// </summary>
        internal static string Costo {
            get {
                return ResourceManager.GetString("Costo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$.
        /// </summary>
        internal static string CURP {
            get {
                return ResourceManager.GetString("CURP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a (?s).*.
        /// </summary>
        internal static string Descripcion {
            get {
                return ResourceManager.GetString("Descripcion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[0-9]+(\.[0-9]{1,3})?$.
        /// </summary>
        internal static string Descuento {
            get {
                return ResourceManager.GetString("Descuento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a (([0-9])|([0-2][0-9])|([3][0-1])).
        /// </summary>
        internal static string Dia {
            get {
                return ResourceManager.GetString("Dia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\., #]+.
        /// </summary>
        internal static string Direccion {
            get {
                return ResourceManager.GetString("Direccion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a (/[A-Za-z]+)+|#$.
        /// </summary>
        internal static string DireccionMVC {
            get {
                return ResourceManager.GetString("DireccionMVC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[0-9][0-9]?$.
        /// </summary>
        internal static string EdadAnios {
            get {
                return ResourceManager.GetString("EdadAnios", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [2-9]|1[0-1]?.
        /// </summary>
        internal static string EdadMeses {
            get {
                return ResourceManager.GetString("EdadMeses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(([0-9])|([0-2][0-9])|([3][0-1]))\/(enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|octubre|noviembre|diciembre)(\/\d{4})?$.
        /// </summary>
        internal static string Fecha {
            get {
                return ResourceManager.GetString("Fecha", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a (19|20)\d\d([- /.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01]).
        /// </summary>
        internal static string FechaAnioMesDia {
            get {
                return ResourceManager.GetString("FechaAnioMesDia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(([0-9])|([0-2][0-9])|([3][0-1]))\/(([0][1-9])|([1][0-2]))\/(\d{4})$.
        /// </summary>
        internal static string FechaDiaMesAnio {
            get {
                return ResourceManager.GetString("FechaDiaMesAnio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(?:(?:0?[1-9]|1\d|2[0-8])(\/|-)(?:0?[1-9]|1[0-2]))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:31(\/|-)(?:0?[13578]|1[02]))|(?:(?:29|30)(\/|-)(?:0?[1,3-9]|1[0-2])))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(29(\/|-)0?2)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$.
        /// </summary>
        internal static string FechaNum {
            get {
                return ResourceManager.GetString("FechaNum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a .
        /// </summary>
        internal static string Georeferencia {
            get {
                return ResourceManager.GetString("Georeferencia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(([1-9])|([0][1-9])|([1][0-2]))?$.
        /// </summary>
        internal static string Horas {
            get {
                return ResourceManager.GetString("Horas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ([0-1][0-9]|2[0-3]):[0-5][0-9].
        /// </summary>
        internal static string HoraValida24hrs {
            get {
                return ResourceManager.GetString("HoraValida24hrs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [1-9][0-9]{1,2}(\.[0-9]{1,2})?.
        /// </summary>
        internal static string IVA {
            get {
                return ResourceManager.GetString("IVA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a /^[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$/.
        /// </summary>
        internal static string JSCURP {
            get {
                return ResourceManager.GetString("JSCURP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a /[a-zA-Z]{3,4}[1-9][0-9]{5}((\D|\d){3})?/.
        /// </summary>
        internal static string JSRFC {
            get {
                return ResourceManager.GetString("JSRFC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a /^[A-Za-z ÑñáéíóúÁÉÍÓÚ]{1,100}$/.
        /// </summary>
        internal static string JSSoloLetras {
            get {
                return ResourceManager.GetString("JSSoloLetras", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(\-?\d+(\.\d+)?),\w*(\-?\d+(\.\d+)?)$.
        /// </summary>
        internal static string LatitudLongitud {
            get {
                return ResourceManager.GetString("LatitudLongitud", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z0-9\., ÑñáéíóúüÁÉÍÓÚÜ]+.
        /// </summary>
        internal static string LetrasNumeros {
            get {
                return ResourceManager.GetString("LetrasNumeros", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [1-9][0-9]*.
        /// </summary>
        internal static string MayorCero {
            get {
                return ResourceManager.GetString("MayorCero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(([0-9])|([0][0-9])|([1-5][0-9]))?$.
        /// </summary>
        internal static string Minutos {
            get {
                return ResourceManager.GetString("Minutos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z ÑñáéíóúüÁÉÍÓÚÜ]{1,50}$.
        /// </summary>
        internal static string Nombre {
            get {
                return ResourceManager.GetString("Nombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z0-9\(\), ÑñáéíóúüÁÉÍÓÚÜ]+.
        /// </summary>
        internal static string NombreCauzaRechazo {
            get {
                return ResourceManager.GetString("NombreCauzaRechazo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [0-9]{1,9}$.
        /// </summary>
        internal static string NotaFolio {
            get {
                return ResourceManager.GetString("NotaFolio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [0-9]{1,2}$.
        /// </summary>
        internal static string NotaServicio {
            get {
                return ResourceManager.GetString("NotaServicio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[0-9]+(\.[0-9]{1,2})?$.
        /// </summary>
        internal static string NumDecimal {
            get {
                return ResourceManager.GetString("NumDecimal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [1-9][0-9]{0,4}([A-Za-z]*)?.
        /// </summary>
        internal static string NumeroCasa {
            get {
                return ResourceManager.GetString("NumeroCasa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [1-9][0-9]{1,6}(\.[0-9]{1,5})?.
        /// </summary>
        internal static string NumeroLocal {
            get {
                return ResourceManager.GetString("NumeroLocal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(?=.{1,50}$)[\w-]*\w$.
        /// </summary>
        internal static string OpcionCss {
            get {
                return ResourceManager.GetString("OpcionCss", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^(?=.{1,100}$)~/\w+/*(\w+/*)+\w+\.aspx|#$.
        /// </summary>
        internal static string OpcionDestino {
            get {
                return ResourceManager.GetString("OpcionDestino", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z ÑñáéíóúÁÉÍÓÚ]{1,50}$.
        /// </summary>
        internal static string OpcionNombre {
            get {
                return ResourceManager.GetString("OpcionNombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z0-9\. ÑñáéíóúüÁÉÍÓÚÜ]{1,50}$.
        /// </summary>
        internal static string OutsourcingNombre {
            get {
                return ResourceManager.GetString("OutsourcingNombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z0-9\. ÑñáéíóúüÁÉÍÓÚÜ]{1,200}$.
        /// </summary>
        internal static string PersonaPuesto {
            get {
                return ResourceManager.GetString("PersonaPuesto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^([0-9]{1,2}([\.][0-9]{1,2})?$|100([\.][0]{1,2})?)$.
        /// </summary>
        internal static string Porcentaje {
            get {
                return ResourceManager.GetString("Porcentaje", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a \$\s[0-9]{1,10}(\.[0-9]{0,2})?$.
        /// </summary>
        internal static string Precios {
            get {
                return ResourceManager.GetString("Precios", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[0-9]+(\.[0-9]{1,2})?$.
        /// </summary>
        internal static string RetiroCaja {
            get {
                return ResourceManager.GetString("RetiroCaja", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z]{3,4}[1-9][0-9]{5}((\D|\d){3})?.
        /// </summary>
        internal static string RFC {
            get {
                return ResourceManager.GetString("RFC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z0-9\.\-\/\#\?\$\(\)\=\+\- \*\;\,\% ÑñáéíóúÁÉÍÓÚ]{1,200}$.
        /// </summary>
        internal static string RolDescripcion {
            get {
                return ResourceManager.GetString("RolDescripcion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z ÑñáéíóúÁÉÍÓÚ]{1,45}$.
        /// </summary>
        internal static string RolNombre {
            get {
                return ResourceManager.GetString("RolNombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [a-zA-Z0-9\. ]+.
        /// </summary>
        internal static string SeguroSocial {
            get {
                return ResourceManager.GetString("SeguroSocial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^[A-Za-z ÑñáéíóúÁÉÍÓÚ]{1,100}$.
        /// </summary>
        internal static string Sololetras {
            get {
                return ResourceManager.GetString("Sololetras", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ^\d+$.
        /// </summary>
        internal static string SoloNumeros {
            get {
                return ResourceManager.GetString("SoloNumeros", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [1-9][0-9]{1,6}(\.[0-9]{1,2})?.
        /// </summary>
        internal static string SueldoPorHora {
            get {
                return ResourceManager.GetString("SueldoPorHora", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ([1-9][0-9]{2})?[1-9][0-9]{6}.
        /// </summary>
        internal static string Telefono {
            get {
                return ResourceManager.GetString("Telefono", resourceCulture);
            }
        }
    }
}