using System;
using System.Web.Mvc;
using System.Web.Routing;
using Sistema;
using Datos;
using Datos.Enums;
using Datos.Repositorios.Configuracion;
using Datos.DTO.Infraestructura;
using Datos.Repositorios;
using Datos.Repositorios.Catalogos;
using Sistema.Extensiones;
using Microsoft.SqlServer.Server;

namespace Presentacion.Controllers
{
    public class ControladorBase : Controller
    {
        private MovimientoRepositorio _movimientoRepositorio;
        private UsuarioRepositorio _usuarioRepositorio;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //TFielLib.TPkcs1 pkcs1 = new TFielLib.TPkcs1();
            //pkcs1.Verify(certificate, cadenaOriginal, firma, TFielLib.TPkcs1.TDigest.Sha2);
            var controlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            var session = ObtenerSession();

            base.OnActionExecuting(filterContext);

            if (session != null)
            {
                var permiso = TienePermiso(controlador, action);
                if (!permiso)
                {
                    if (controlador == "Login" && action == "CerrarSesion") return;
                    filterContext.Result = ObtenerRedireccionamiento("Inicio", "Index");
                }
                else
                {
                    if (controlador != "Menu") LimpiarTemporalesMenu();
                    if (controlador == "Login") return;
                }

                //if (controlador != "Menu") LimpiarTemporalesMenu();
                //if (controlador == "Login") return;

                CargarBreadCrumbs(controlador, action);

            }
            else if (controlador != "Login")
            {
                filterContext.Result = ObtenerRedireccionamiento("Login", "Index");
            }
        }

        private bool TienePermiso(string controlador, string accion)
        {
            var menuRolRepositorio = new MenuRolRepositorio();
            return menuRolRepositorio.TienePermisoPagina(ObtenerSesion().IdRol, controlador, accion);
        }

        /// <summary>
        /// Obtener la Session
        /// </summary>
        /// <returns></returns>
        public SistemaUsuario ObtenerSession()
        {
            return ((SistemaUsuario)Session[Enumerados.VariablesSesion.Aplicacion.ToString()]);
        }

        /// <summary>
        /// Metodo Guardar Excepcion
        /// </summary>
        /// <param name="error"></param>
        public void EscribirError(string error)
        {
            var log = new Log();
            TempData[Enumerados.TempData.Error.ToString()] = error;
            log.AgregarError(error);
        }

        /// <summary>
        /// Metodo para redireccionar
        /// </summary>
        /// <param name="controlador"></param>
        /// <param name="accion"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private RedirectToRouteResult ObtenerRedireccionamiento(string controlador, string accion, string area = null)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = controlador,
                action = accion,
                area
            }));
        }

        /// <summary>
        /// Metodo para crear menu BreadCrumbs
        /// </summary>
        /// <param name="controlador"></param>
        /// <param name="accion"></param>
        private void CargarBreadCrumbs(string controlador, string accion)
        {
            using (var bd = new Contexto())
            {
                var menuRepositorio = new MenuRepositorio(bd);
                var idMenu = menuRepositorio.ObtenerId(controlador, accion);
                var menus = menuRepositorio.BuscarOpcionMenuYOpcionesSuperiores(idMenu);
                ObtenerSesion().MenuBreadcrumb = menus;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LimpiarTemporalesMenu()
        {
            LimpiarDatoTemporal(Enumerados.TempData.IdGestionarMenu);
            LimpiarDatoTemporal(Enumerados.TempData.TipoGestion);
            LimpiarDatoTemporal(Enumerados.TempData.NombreTipoGestion);
        }

        /// <summary>
        /// Metodo Obtener Session
        /// </summary>
        /// <returns></returns>
        public SistemaUsuario ObtenerSesion()
        {
            return ((SistemaUsuario)Session[Enumerados.VariablesSesion.Aplicacion.ToString()]);
        }

        /// <summary>
        /// Limpiar Dato Temporal
        /// </summary>
        /// <param name="tempData"></param>
        private void LimpiarDatoTemporal(Enumerados.TempData tempData)
        {
            TempData[tempData.ToString()] = null;
        }
        public void EscribirMensaje(string mensaje)
        {
            TempData[Enumerados.TempData.Mensaje.ToString()] = mensaje;
        }
        public void EnviarAlerta(string mensajeCorrecto, string mensajeError, bool correcto)
        {
            if (correcto)
            {
                EscribirMensaje(mensajeCorrecto);
            }
            else
            {
                EscribirError(mensajeError);
            }
        }

        //public T ObtenerDatoTemporal<T>(Enumerados.TempData tempData)
        //{
        //    T dataTemp = default(T);
        //    try
        //    {
        //        dataTemp = (T)TempData[tempData.ToString()];
        //    }
        //    catch (Exception ex)
        //    {
        //        var mensaje = ex.Message;
        //    }

        //    return dataTemp;
        //}
        public T ObtenerDatoTemporal<T>(Enumerados.TempData tempData)
        {
            return (T)TempData[tempData.ToString()];
        }

        public T ObtenerDatoTemporal<T>(Enumerados.TempData tempData, bool mantener)
        {
            var regresar = (T)TempData[tempData.ToString()];
            if (mantener) GuardarDatoTemporal(tempData, regresar, true);
            return regresar;
        }

        public int ObtenerParametroGetEnInt(Enumerados.Parametro parametro)
        {
            return Request.QueryString[parametro.ToString()].DecodeFrom64().TryToInt();
        }

        public void GuardarDatoTemporal<T>(Enumerados.TempData tempData, T datoGuardar, bool mantener)
        {
            TempData[tempData.ToString()] = datoGuardar;

            if (mantener)
            {
                TempData.Keep(tempData.ToString());
            }
        }

        public void LimpiarModelState()
        {
            foreach (var model in ModelState.Values)
            {
                model.Errors.Clear();
            }
        }

        protected void GuardarMovimiento(string _nombre, string _catalogo, int _id) {
            var _usuario = new Usuario();
            using (var bd = new Contexto()) {
                _usuarioRepositorio = new UsuarioRepositorio(bd);
                _movimientoRepositorio = new MovimientoRepositorio(bd);
                var movimiento = new Movmiento
                {
                    Nombre = _nombre,
                    Usuario = _usuarioRepositorio.BuscarPorId(ObtenerSesion().IdUsuario),
                    Habilitado = true,
                    Fecha = DateTime.Now,
                    Catalogo = _catalogo,
                    IDRegistro = _id
                };
                _movimientoRepositorio.Guardar(movimiento);

                bd.SaveChanges();
            }
        }

        protected string ValidarNulo(string campo)
        {
            if (campo == null) return "No disponible";
            if (campo.GetType() == typeof(string))
            {
                return string.IsNullOrEmpty(campo) ? "No disponible" : campo;
            }
            return campo;
        }

        protected string ObtenerUrlDescargar(string accion)
        {
            var urlBuilder = new UriBuilder("http://constanciasni.sonora.gob.mx/")
            {
                Path = Url.Action(accion, "Descargar", new {Area = string.Empty}),
                Query = null,
            };

            var url = urlBuilder.ToString();

            return url;
        }
    }
}