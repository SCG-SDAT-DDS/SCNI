﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Negocio.WsFielServicios {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="FirmaCWSSoap", Namespace="http://contraloria.sonora.gob.mx")]
    public partial class FirmaCWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AutenticarCertificadoOperationCompleted;
        
        private System.Threading.SendOrPostCallback VerificarPkCS7OperationCompleted;
        
        private System.Threading.SendOrPostCallback VerificarPkCS7MultipleOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenerarEstampillaTiempoOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenerarMultipleEstampillaTiempoOperationCompleted;
        
        private System.Threading.SendOrPostCallback DecodificarPkcs7OperationCompleted;
        
        private System.Threading.SendOrPostCallback DecodificarArregloPkcs7OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public FirmaCWS() {
            this.Url = System.Configuration.ConfigurationManager.AppSettings[@"FielUrlServicios"];
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event AutenticarCertificadoCompletedEventHandler AutenticarCertificadoCompleted;
        
        /// <remarks/>
        public event VerificarPkCS7CompletedEventHandler VerificarPkCS7Completed;
        
        /// <remarks/>
        public event VerificarPkCS7MultipleCompletedEventHandler VerificarPkCS7MultipleCompleted;
        
        /// <remarks/>
        public event GenerarEstampillaTiempoCompletedEventHandler GenerarEstampillaTiempoCompleted;
        
        /// <remarks/>
        public event GenerarMultipleEstampillaTiempoCompletedEventHandler GenerarMultipleEstampillaTiempoCompleted;
        
        /// <remarks/>
        public event DecodificarPkcs7CompletedEventHandler DecodificarPkcs7Completed;
        
        /// <remarks/>
        public event DecodificarArregloPkcs7CompletedEventHandler DecodificarArregloPkcs7Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/AutenticarCertificado", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] AutenticarCertificado([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] certificado, string token) {
            object[] results = this.Invoke("AutenticarCertificado", new object[] {
                        certificado,
                        token});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void AutenticarCertificadoAsync(byte[] certificado, string token) {
            this.AutenticarCertificadoAsync(certificado, token, null);
        }
        
        /// <remarks/>
        public void AutenticarCertificadoAsync(byte[] certificado, string token, object userState) {
            if ((this.AutenticarCertificadoOperationCompleted == null)) {
                this.AutenticarCertificadoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAutenticarCertificadoOperationCompleted);
            }
            this.InvokeAsync("AutenticarCertificado", new object[] {
                        certificado,
                        token}, this.AutenticarCertificadoOperationCompleted, userState);
        }
        
        private void OnAutenticarCertificadoOperationCompleted(object arg) {
            if ((this.AutenticarCertificadoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AutenticarCertificadoCompleted(this, new AutenticarCertificadoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/VerificarPkCS7", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] VerificarPkCS7([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] pkcs7, string token) {
            object[] results = this.Invoke("VerificarPkCS7", new object[] {
                        pkcs7,
                        token});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void VerificarPkCS7Async(byte[] pkcs7, string token) {
            this.VerificarPkCS7Async(pkcs7, token, null);
        }
        
        /// <remarks/>
        public void VerificarPkCS7Async(byte[] pkcs7, string token, object userState) {
            if ((this.VerificarPkCS7OperationCompleted == null)) {
                this.VerificarPkCS7OperationCompleted = new System.Threading.SendOrPostCallback(this.OnVerificarPkCS7OperationCompleted);
            }
            this.InvokeAsync("VerificarPkCS7", new object[] {
                        pkcs7,
                        token}, this.VerificarPkCS7OperationCompleted, userState);
        }
        
        private void OnVerificarPkCS7OperationCompleted(object arg) {
            if ((this.VerificarPkCS7Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VerificarPkCS7Completed(this, new VerificarPkCS7CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/VerificarPkCS7Multiple", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ListaVerificacionPKCS7[] VerificarPkCS7Multiple(byte[][] pkcs7, string token) {
            object[] results = this.Invoke("VerificarPkCS7Multiple", new object[] {
                        pkcs7,
                        token});
            return ((ListaVerificacionPKCS7[])(results[0]));
        }
        
        /// <remarks/>
        public void VerificarPkCS7MultipleAsync(byte[][] pkcs7, string token) {
            this.VerificarPkCS7MultipleAsync(pkcs7, token, null);
        }
        
        /// <remarks/>
        public void VerificarPkCS7MultipleAsync(byte[][] pkcs7, string token, object userState) {
            if ((this.VerificarPkCS7MultipleOperationCompleted == null)) {
                this.VerificarPkCS7MultipleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnVerificarPkCS7MultipleOperationCompleted);
            }
            this.InvokeAsync("VerificarPkCS7Multiple", new object[] {
                        pkcs7,
                        token}, this.VerificarPkCS7MultipleOperationCompleted, userState);
        }
        
        private void OnVerificarPkCS7MultipleOperationCompleted(object arg) {
            if ((this.VerificarPkCS7MultipleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VerificarPkCS7MultipleCompleted(this, new VerificarPkCS7MultipleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/GenerarEstampillaTiempo", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] GenerarEstampillaTiempo([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] datos, string token) {
            object[] results = this.Invoke("GenerarEstampillaTiempo", new object[] {
                        datos,
                        token});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void GenerarEstampillaTiempoAsync(byte[] datos, string token) {
            this.GenerarEstampillaTiempoAsync(datos, token, null);
        }
        
        /// <remarks/>
        public void GenerarEstampillaTiempoAsync(byte[] datos, string token, object userState) {
            if ((this.GenerarEstampillaTiempoOperationCompleted == null)) {
                this.GenerarEstampillaTiempoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerarEstampillaTiempoOperationCompleted);
            }
            this.InvokeAsync("GenerarEstampillaTiempo", new object[] {
                        datos,
                        token}, this.GenerarEstampillaTiempoOperationCompleted, userState);
        }
        
        private void OnGenerarEstampillaTiempoOperationCompleted(object arg) {
            if ((this.GenerarEstampillaTiempoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerarEstampillaTiempoCompleted(this, new GenerarEstampillaTiempoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/GenerarMultipleEstampillaTiempo", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TimeStampingList[] GenerarMultipleEstampillaTiempo(byte[][] datos, string token) {
            object[] results = this.Invoke("GenerarMultipleEstampillaTiempo", new object[] {
                        datos,
                        token});
            return ((TimeStampingList[])(results[0]));
        }
        
        /// <remarks/>
        public void GenerarMultipleEstampillaTiempoAsync(byte[][] datos, string token) {
            this.GenerarMultipleEstampillaTiempoAsync(datos, token, null);
        }
        
        /// <remarks/>
        public void GenerarMultipleEstampillaTiempoAsync(byte[][] datos, string token, object userState) {
            if ((this.GenerarMultipleEstampillaTiempoOperationCompleted == null)) {
                this.GenerarMultipleEstampillaTiempoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerarMultipleEstampillaTiempoOperationCompleted);
            }
            this.InvokeAsync("GenerarMultipleEstampillaTiempo", new object[] {
                        datos,
                        token}, this.GenerarMultipleEstampillaTiempoOperationCompleted, userState);
        }
        
        private void OnGenerarMultipleEstampillaTiempoOperationCompleted(object arg) {
            if ((this.GenerarMultipleEstampillaTiempoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerarMultipleEstampillaTiempoCompleted(this, new GenerarMultipleEstampillaTiempoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/DecodificarPkcs7", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfString")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)]
        public string[][] DecodificarPkcs7([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] pkcs7, string token) {
            object[] results = this.Invoke("DecodificarPkcs7", new object[] {
                        pkcs7,
                        token});
            return ((string[][])(results[0]));
        }
        
        /// <remarks/>
        public void DecodificarPkcs7Async(byte[] pkcs7, string token) {
            this.DecodificarPkcs7Async(pkcs7, token, null);
        }
        
        /// <remarks/>
        public void DecodificarPkcs7Async(byte[] pkcs7, string token, object userState) {
            if ((this.DecodificarPkcs7OperationCompleted == null)) {
                this.DecodificarPkcs7OperationCompleted = new System.Threading.SendOrPostCallback(this.OnDecodificarPkcs7OperationCompleted);
            }
            this.InvokeAsync("DecodificarPkcs7", new object[] {
                        pkcs7,
                        token}, this.DecodificarPkcs7OperationCompleted, userState);
        }
        
        private void OnDecodificarPkcs7OperationCompleted(object arg) {
            if ((this.DecodificarPkcs7Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DecodificarPkcs7Completed(this, new DecodificarPkcs7CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://contraloria.sonora.gob.mx/DecodificarArregloPkcs7", RequestNamespace="http://contraloria.sonora.gob.mx", ResponseNamespace="http://contraloria.sonora.gob.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ListaInformacion[] DecodificarArregloPkcs7(string[] pkcs7, string token) {
            object[] results = this.Invoke("DecodificarArregloPkcs7", new object[] {
                        pkcs7,
                        token});
            return ((ListaInformacion[])(results[0]));
        }
        
        /// <remarks/>
        public void DecodificarArregloPkcs7Async(string[] pkcs7, string token) {
            this.DecodificarArregloPkcs7Async(pkcs7, token, null);
        }
        
        /// <remarks/>
        public void DecodificarArregloPkcs7Async(string[] pkcs7, string token, object userState) {
            if ((this.DecodificarArregloPkcs7OperationCompleted == null)) {
                this.DecodificarArregloPkcs7OperationCompleted = new System.Threading.SendOrPostCallback(this.OnDecodificarArregloPkcs7OperationCompleted);
            }
            this.InvokeAsync("DecodificarArregloPkcs7", new object[] {
                        pkcs7,
                        token}, this.DecodificarArregloPkcs7OperationCompleted, userState);
        }
        
        private void OnDecodificarArregloPkcs7OperationCompleted(object arg) {
            if ((this.DecodificarArregloPkcs7Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DecodificarArregloPkcs7Completed(this, new DecodificarArregloPkcs7CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://contraloria.sonora.gob.mx")]
    public partial class ListaVerificacionPKCS7 {
        
        private string mensajeField;
        
        private string respuestaField;
        
        /// <comentarios/>
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
            }
        }
        
        /// <comentarios/>
        public string respuesta {
            get {
                return this.respuestaField;
            }
            set {
                this.respuestaField = value;
            }
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://contraloria.sonora.gob.mx")]
    public partial class ListaInformacion {
        
        private string mensajeField;
        
        private string[][] firmanteField;
        
        /// <comentarios/>
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfString")]
        [System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)]
        public string[][] firmante {
            get {
                return this.firmanteField;
            }
            set {
                this.firmanteField = value;
            }
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://contraloria.sonora.gob.mx")]
    public partial class TimeStampingList {
        
        private string statusField;
        
        private string responseField;
        
        /// <comentarios/>
        public string status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <comentarios/>
        public string response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void AutenticarCertificadoCompletedEventHandler(object sender, AutenticarCertificadoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AutenticarCertificadoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AutenticarCertificadoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void VerificarPkCS7CompletedEventHandler(object sender, VerificarPkCS7CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VerificarPkCS7CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal VerificarPkCS7CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void VerificarPkCS7MultipleCompletedEventHandler(object sender, VerificarPkCS7MultipleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VerificarPkCS7MultipleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal VerificarPkCS7MultipleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ListaVerificacionPKCS7[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ListaVerificacionPKCS7[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GenerarEstampillaTiempoCompletedEventHandler(object sender, GenerarEstampillaTiempoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerarEstampillaTiempoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenerarEstampillaTiempoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GenerarMultipleEstampillaTiempoCompletedEventHandler(object sender, GenerarMultipleEstampillaTiempoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerarMultipleEstampillaTiempoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenerarMultipleEstampillaTiempoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TimeStampingList[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TimeStampingList[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void DecodificarPkcs7CompletedEventHandler(object sender, DecodificarPkcs7CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DecodificarPkcs7CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DecodificarPkcs7CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[][] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[][])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void DecodificarArregloPkcs7CompletedEventHandler(object sender, DecodificarArregloPkcs7CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DecodificarArregloPkcs7CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DecodificarArregloPkcs7CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ListaInformacion[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ListaInformacion[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591