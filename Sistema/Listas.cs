using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Sistema.Extensiones;

namespace Sistema
{
    public class Listas
    {
        public static List<SelectListItem> ObtenerListaGenero()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Femenino", Value = "1"},
                new SelectListItem{ Text = "Masculino", Value = "2"},
            };
        }

        public static List<SelectListItem> ObtenerListaCantidadPagina()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Text = "10", Value = "10"},
                new SelectListItem{ Text = "20", Value = "20"},
                new SelectListItem{ Text = "50", Value = "50"},
                new SelectListItem{ Text = "100", Value = "100"},
            };
        }

        public static List<SelectListItem> ObtenerListaTipoSolicitud()
        {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Personal", Value = "1"},
                new SelectListItem{ Text = "Dependencia-Entidad", Value = "2"},
            };
        }

        public static List<SelectListItem> ObtenerListaTipoIdentificacion() {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Credencial de Elector", Value = "1"},
                new SelectListItem{ Text = "Pasaporte Mexicano", Value = "2"},
                new SelectListItem{ Text = "Cartilla Militar", Value = "3"},
            };
        }

        public static List<SelectListItem> ObtenerListaTipoEntidadEstatal()
        {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Dependencia", Value = "1"},
                new SelectListItem{ Text = "Entidad", Value = "2"},
                new SelectListItem{ Text = "Administración Pública Municipal", Value = "4"}
            };
        }

        public static List<SelectListItem> ObtenerListaTipoEntidad()
        {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Dependencia", Value = "1"},
                new SelectListItem{ Text = "Entidad", Value = "2"},
                new SelectListItem{ Text = "Administración Pública Federal", Value = "3"},
                new SelectListItem{ Text = "Administración Pública Municipal", Value = "4"}
            };
        }

        public static List<SelectListItem> ObtenerListaTipoEntidadFederal()
        {
            return new List<SelectListItem> {
                new SelectListItem{ Text = "Seleccione...", Value = ""},
                new SelectListItem{ Text = "Administración Pública Federal", Value = "3"}
            };
        }

        public static List<SelectListItem> ObtenerListaSancionesFederal() {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Apercibimiento", Value = "1" },
                new SelectListItem { Text = "Suspensión", Value = "2" },
                new SelectListItem { Text = "Amonestación", Value = "3" },
                new SelectListItem { Text = "Inhabilitación para desempeñar empleos, cargos o comisiones en el SP", Value = "4" },
                new SelectListItem { Text = "Destitución del puesto", Value = "5" },
                new SelectListItem { Text = "Sanción Económica", Value = "6" },
                new SelectListItem { Text = "Dejar sin efecto", Value = "7" },
                new SelectListItem { Text = "Amonestación privada", Value = "8" },
                new SelectListItem { Text = "Amonestación pública", Value = "9" },
                new SelectListItem { Text = "Inhabilitación y para participar en adquisiciones, arrendamientos, servicios u OP", Value = "10" },
            };
        }

        public static List<SelectListItem> ObtenerListaSancionesEstatal()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Apercibimiento", Value = "1" },
                new SelectListItem { Text = "Suspensión", Value = "2" },
                new SelectListItem { Text = "Amonestación", Value = "3" },
                new SelectListItem { Text = "Inhabilitación para desempeñar empleos, cargos o comisiones en el SP", Value = "4" },
                new SelectListItem { Text = "Destitución del puesto", Value = "5" },
                new SelectListItem { Text = "Sanción Económica", Value = "6" },
                new SelectListItem { Text = "Dejar sin efecto", Value = "7" },
                new SelectListItem { Text = "Amonestación privada", Value = "8" },
                new SelectListItem { Text = "Amonestación pública", Value = "9" },
                new SelectListItem { Text = "Inhabilitación y para participar en adquisiciones, arrendamientos, servicios u OP", Value = "10" },
            };
        }

        public static List<SelectListItem> ObtenerListaTipoSancionEstatal()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Responsabilidad Oficial", Value = "1" },
                //new SelectListItem { Text = "Situación Patrimonial", Value = "2" },
                new SelectListItem { Text = "Administración Pública Municipal", Value = "3" },
            };
        }

        public static List<SelectListItem> ObtenerListaTipoSancionFederal()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Responsabilidad Oficial", Value = "1" },
                new SelectListItem { Text = "Situación Patrimonial", Value = "2" }
            };
        }

        public static List<SelectListItem> ObtenerListaConSinSancion()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Con sanción", Value = "1" },
                new SelectListItem { Text = "Sin sanción", Value = "2" }
            };
        }

        public static List<SelectListItem> ObtenerListaConSinObservacion()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccione...", Value = "" },
                new SelectListItem { Text = "Con observaciones", Value = "1" },
                new SelectListItem { Text = "Sin observaciones", Value = "2" }
            };
        }

        public static List<SelectListItem> ObtenerListaMeses(bool first_option = false) {

            var items = new List<SelectListItem>();

            if (first_option) items.Add(new SelectListItem { Text = "Seleccione...", Value = "" });

            for (var x = 1; x <= 12; x++)
            {
                var mes = new DateTime(DateTime.Now.Year, x, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                items.Add(new SelectListItem { Text = mes.UpperFirstLetter(), Value = x.ToString() });
            }
            return items;
        }
        public static List<SelectListItem> ObtenerListaAños(bool first_option = false) {
            return ObtenerListaAños(0, 0, first_option);
        }
        public static List<SelectListItem> ObtenerListaAños(int min = 0, int max = 0, bool first_option = false)
        {
            min = min == 0 ? 1900 : min;
            max = max == 0 ? DateTime.Now.Year : max;

            var items = new List<SelectListItem>();

            if (first_option) items.Add(new SelectListItem { Text = "Seleccione...", Value = "" });

            for (var x = max; x >= min; x--)
            {
                items.Add(new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            }

            return items;

        }
        public static List<SelectListItem> ObtenerListaNumerica(int min = 0, int max = 100)
        {
            
            var items = new List<SelectListItem>();

            for (var x = min; x <= max; x++)
            {
                if (x < max)
                {
                    items.Add(new SelectListItem { Text = x.ToString(), Value = x.ToString() });
                }
                else
                {
                    items.Add(new SelectListItem { Text = x.ToString(), Value = x.ToString() });
                }
            }

            return items;

        }

        public static List<SelectListItem> ObtenerListaTipoSolicitudes(bool incluirSeleccione = false)
        {
            var lista = new List<SelectListItem> {
                new SelectListItem { Text = "Presencial", Value = "1" },
                new SelectListItem { Text = "Web", Value = "2" },
            };

            if (incluirSeleccione)
            {
                lista.Insert(0, new SelectListItem
                {
                    Value = string.Empty,
                    Text = "Seleccione..."
                });
            }

            return lista;
        }

        public static List<SelectListItem> ObtenerListaMovimientos()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccionar...", Value = "" },
                new SelectListItem { Text = "Creación", Value = "Creación" },
                new SelectListItem { Text = "Modificación", Value = "Modificación" },
                new SelectListItem { Text = "Habilitar", Value = "Habilitar" },
                new SelectListItem { Text = "Deshabilitar", Value = "Deshabilitar" }
            };

        }

        public static List<SelectListItem> ObtenerListaCatalogos()
        {

            return new List<SelectListItem> {
                new SelectListItem { Text = "Seleccionar...", Value = "" },
                new SelectListItem { Text = "Entidad", Value = "Entidad" },
                new SelectListItem { Text = "Firma", Value = "Firma" },
                new SelectListItem { Text = "Menu", Value = "Menu" },
                new SelectListItem { Text = "Rol", Value = "Rol" },
                new SelectListItem { Text = "Sanción", Value = "Sanción" },
                new SelectListItem { Text = "Solicitud", Value = "Solicitud" },
                new SelectListItem { Text = "SolicitudWeb", Value = "SolicitudWeb" },
                new SelectListItem { Text = "Título", Value = "Título" },
                new SelectListItem { Text = "Usuario", Value = "Usuario" }
            };

        }

        public static string ObtenerValorDeLista(List<SelectListItem> Lista, int index) {
            return Lista[index].Text;
        }
    }
}
