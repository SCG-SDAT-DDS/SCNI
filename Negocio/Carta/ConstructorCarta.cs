using System;
using System.Collections.Generic;
using System.Linq;
using Datos;
using Datos.DTO;
using Sistema.Extensiones;

namespace Negocio.Carta
{
    internal class ConstructorCarta
    {
        private readonly int _idSolicitud;
        private Datos.Carta _carta;
        private readonly PersonaSancionesDto _personaSanciones;
        private readonly List<SancionDto> _sanciones;
        private readonly string _numeroExpediente;
        private Sancion _sancionVigente;

        public static readonly string[] EtiquetasCarta =
        {
            "<numero_oficio>",
            "<fecha_oficio>",
            "<tipo_constancia>",
            "<nombre_persona>",
            "<cuenta>",
            "<sanciones_estatal>",
            "<lugar_administracion>",
            "<texto_federal_no_sancion>",
            "<texto_federal>",
            "<nombre_firmante>",
            "<puesto_firmante>"
        };

        public static readonly string[] EtiquetasCarta2 =
        {
            "<numero_oficio>",
            "<fecha_oficio>",
            "<tipo_constancia>",
            "<nombre_persona>",
            "<si_no>",
            "<texto_sancion>",
            "<nombre_firmante>",
            "<puesto_firmante>"
        };

        public ConstructorCarta(int idSolicitud, PersonaSancionesDto personaSanciones, string numeroExpediente)
        {
            personaSanciones.Nombre = personaSanciones.Nombre.TrimEnd();

            _idSolicitud = idSolicitud;
            _personaSanciones = personaSanciones;
            _sanciones = _personaSanciones.Sanciones.ToList();
            _numeroExpediente = numeroExpediente;
        }

        public ConstructorCarta(PersonaCartaDto personaCarta, List<SancionDto> sanciones)
        {
            _personaSanciones = new PersonaSancionesDto
            {
                Nombre = personaCarta.Nombre.TrimEnd()
            };
            _carta = personaCarta.Carta;
            _sanciones = sanciones;
            _carta.Tipo = _ObtenerTipo(_sanciones);
            _carta.Estado = EstadosCarta.NoFirmada;
        }

        public Datos.Carta Generar()
        {
            if (_carta == null) _carta = _GenerarCarta();

            var carta = _carta;

            carta.Cadena = new CadenaCarta()
                .AgregarFolio(carta.NumeroExpediente)
                .AgregarFecha(carta.Fecha)
                .AgregarTipo(carta.Tipo)
                .AgregarNombre(_personaSanciones.Nombre)
                
                //Primera versión de cartas
                //.AgregarTextoEstatal(_GenerarTextoEstatal())
                //.AgregarTextoFederal(_GenerarTextoFederal())
                
                //Segunda versión de cartas
                .AgregarTextoEstatal(_GenerarTextoSiNo())
                .AgregarTextoFederal(_GenerarTextoSancion())

                .GenerarCadena();

            return carta;
        }

        private Datos.Carta _GenerarCarta()
        {
            var carta = new Datos.Carta
            {
                NumeroExpediente = _numeroExpediente,
                Tipo = _ObtenerTipo(_sanciones),
                IDSolicitud = _idSolicitud,
                Fecha = DateTime.Now,
                Estado = EstadosCarta.NoFirmada,
                Consultable = true
            };

            foreach (var sancionDto in _personaSanciones.Sanciones)
            {
                carta.Sanciones.Add(sancionDto.Sancion);
            }

            return carta;
        }

        private TiposCarta _ObtenerTipo(List<SancionDto> sancionesDto)
        {
            var tipo = TiposCarta.NoInhabilitacion;

            foreach (var sancionDto in sancionesDto)
            {
                if (sancionDto.Sancion.TipoSancion != (byte) TiposSancion.InhabilitaciónDesempeñar &&
                    sancionDto.Sancion.TipoSancion != (byte)TiposSancion.InhabilitaciónParticipar)
                    continue;
                
                if (_EstaVigente(sancionDto.Sancion))
                {
                    tipo = TiposCarta.Inhabilitacion;
                    break;
                }
            }

            return tipo;
        }

        private bool _EstaVigente(Sancion sancion)
        {
            if (sancion.TipoSancion != (byte)TiposSancion.InhabilitaciónDesempeñar &&
                sancion.TipoSancion != (byte)TiposSancion.InhabilitaciónParticipar &&
                sancion.TipoSancion != (byte)TiposSancion.Suspensión)
                return false;

            bool estaVigente;

            if (sancion.PeriodoInicial.HasValue && sancion.PeriodoFinal.HasValue)
            {
                estaVigente = sancion.PeriodoInicial <= DateTime.Now && DateTime.Now <= sancion.PeriodoFinal;

                if (estaVigente) _sancionVigente = sancion;
            }
            else
            {
                var fechaInicio = sancion.FechaEjecutoria.Date;
                var fechaFin = fechaInicio.AddYears(sancion.TiempoAños)
                    .AddMonths(sancion.TiempoMeses)
                    .AddDays(sancion.TiempoDias).Date;

                estaVigente = fechaInicio <= DateTime.Now && DateTime.Now <= fechaFin;
            }

            return estaVigente;
        }

        private string _GenerarTextoEstatal()
        {
            var textoMunicipalEstatal = _GenerarTextoSanciones(false);

            if (textoMunicipalEstatal != null)
            {
                textoMunicipalEstatal += ", dentro de la Administración Pública";

                var esMunicipal = _sanciones.Any(s => s.TipoEntidad == TiposEntidad.Municipal);
                var esEstatal = _sanciones.Any(s => s.TipoEntidad == TiposEntidad.Entidad ||
                                                    s.TipoEntidad == TiposEntidad.Dependencia);

                if (esMunicipal && esEstatal)
                    textoMunicipalEstatal += " Municipal y Estatal";
                else
                    textoMunicipalEstatal += esMunicipal ? " Municipal" : " Estatal";
            }
            else
            {
                textoMunicipalEstatal = "no cuenta con antecedente de inhabilitación dentro de la " +
                                        "Administración Pública Municipal y Estatal";
            }

            return textoMunicipalEstatal;
        }
        
        private string _GenerarTextoFederal()
        {
            var textoFederal = _GenerarTextoSanciones(true);

            if (textoFederal != null)
            {
                textoFederal += ", dentro de la Administración Pública Federal";
            }
            else
            {
                textoFederal = "no cuenta con sanción administrativa firme en la Administración Pública Federal";
            }

            return textoFederal;
        }

        private string _GenerarTextoSiNo()
        {
            return _carta.Tipo == TiposCarta.Inhabilitacion ? "SÍ se encontró" : "NO se encontró";
        }
        
        private string _GenerarTextoSancion()
        {
            if (_sancionVigente == null) return null;

            return "por el periodo " + _sancionVigente.PeriodoInicial.Value.ToString("dd/MM/yyyy") + " a " +
                   _sancionVigente.PeriodoFinal.Value.ToString("dd/MM/yyyy");
        }

        private string _GenerarTextoSanciones(bool esFederal)
        {
            var sanciones = _sanciones.Where(s => (!esFederal || s.TipoEntidad == TiposEntidad.Federal) &&
                                                  (esFederal || s.TipoEntidad != TiposEntidad.Federal))
                .OrderBy(s => s.Sancion.Tipo).ThenBy(s => s.Sancion.FechaEjecutoria)
                .ToList();

            if(sanciones.Count == 0) return null;

            var puntero = 0;
            var antecedentes = 0;
            for (var i = 0; i < sanciones.Count; i++)
            {
                var sancion = sanciones[i];

                if (_EstaVigente(sancion.Sancion))
                {
                    sanciones.Remove(sancion);
                    sanciones.Insert(puntero++, sancion);
                }
                else
                {
                    antecedentes++;
                }
            }

            var sancionesTomadas = new List<Sancion>();
            var cantidad = sanciones.Count;
            string texto = null;

            for (var i = 0; i < cantidad; i++)
            {
                var sancionDto = sanciones[i];
                var sancion = sancionDto.Sancion;

                if (sancionesTomadas.Contains(sancion)) continue;

                var estaVigente = _EstaVigente(sancion);

                var sancionesMismoTipo = estaVigente
                    ? new List<Sancion> {sancionDto.Sancion}
                    : sanciones.Where(
                        f => f.Sancion.TipoSancion == sancion.TipoSancion && !sancionesTomadas.Contains(f.Sancion))
                        .Select(f => f.Sancion)
                        .ToList();

                sancionesTomadas.AddRange(sancionesMismoTipo);

                var verboAntecedente = antecedentes > 1 ? "ANTECEDENTES DE" : "ANTECEDENTE DE";

                if (i == 0)
                {
                    texto = "cuenta con " + (estaVigente ? string.Empty : verboAntecedente);
                }

                var tipoSancion = ((TiposSancion)sancion.TipoSancion).ToString().AddSpaceBeforeUpper().ToUpper();
                var causaInhabilitacion = sancion.TipoSancion == (byte)TiposSancion.InhabilitaciónDesempeñar ||
                                          sancion.TipoSancion == (byte)TiposSancion.InhabilitaciónParticipar ||
                                          sancion.TipoSancion == (byte)TiposSancion.Suspensión;

                var descripcion = string.Empty;

                var cantidadMismoTipo = sancionesMismoTipo.Count;
                for (var j = 0; j < cantidadMismoTipo; j++)
                {
                    if (j > 0 && j < cantidadMismoTipo - 1)
                        descripcion += ",";
                    else if (j > 0 && j == cantidadMismoTipo - 1)
                        descripcion += " Y";

                    var mismo = sancionesMismoTipo[j];

                    if (causaInhabilitacion)
                    {
                        if (mismo.TiempoAños > 0)
                            descripcion += " POR " + mismo.TiempoAños + " " +
                                           (mismo.TiempoAños > 1 ? "AÑOS" : "AÑO");
                        else if (sancion.TiempoMeses > 0)
                            descripcion += " POR " + mismo.TiempoMeses + " " +
                                           (mismo.TiempoMeses > 1 ? "MESES" : "MES");
                        else
                            descripcion += " POR " + mismo.TiempoDias + " " +
                                           (mismo.TiempoDias > 1 ? "DÍAS" : "DÍA");
                    }

                    if(causaInhabilitacion || j == 0)
                        descripcion += " EN EL AÑO " + mismo.Año;
                    else
                        descripcion += " " + mismo.Año;
                }

                var union = string.Empty;

                if (i == 0)
                    union = string.Empty;
                else if (cantidad == i + 1 || cantidad == sancionesTomadas.Count)
                    union = tipoSancion[0] == 'I' ? " E" : " Y";
                else if (i > 0)
                    union = ",";

                texto += union + (!estaVigente && puntero == i && i > 0 ? " " + verboAntecedente : string.Empty) + " " +
                         tipoSancion + descripcion;

                if (estaVigente)
                    texto += " la cual se encuentra en periodo de cumplimiento";
            }

            return texto;
        }
    }
}
