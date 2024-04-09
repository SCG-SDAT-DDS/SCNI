using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Presentacion.Areas.Catalogo.Models;
//using Presentacion.Areas.Catalogo.Models.ViewModels;
using Presentacion.Controllers;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Enums;
using Datos.Repositorios.Catalogos;
using Sistema.Paginador;
using Resources;
using Sistema.Extensiones;

namespace Presentacion.Areas.Catalogo.Controllers
{
    public class EstadoController : ControladorBase
    {
        private EstadoRepositorio _estadoRepositorio;
        private Contexto db = new Contexto();


        // GET: Catalogo/Estado
        [HttpGet]
        public ActionResult Index(EstadoViewModel estadoViewModel)
        {
            try
            {
                estadoViewModel.Estado = new Estado();
                using (var bd = new Contexto())
                {
                    _estadoRepositorio = new EstadoRepositorio(bd);
                    estadoViewModel.Estados = _estadoRepositorio.Buscar(estadoViewModel);
                    estadoViewModel.TotalEncontrados = _estadoRepositorio.ObtenerTotalRegistros(estadoViewModel);
                    estadoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(estadoViewModel.TotalEncontrados, estadoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            //return View();
            return View(estadoViewModel);
            //return View(db.Colonia.ToList());
        }

        [HttpPost]
        public ActionResult Index(EstadoViewModel estadoViewModel, string pagina)
        {
            estadoViewModel.PaginaActual = pagina.TryToInt();
            if (estadoViewModel.PaginaActual <= 0) estadoViewModel.PaginaActual = 1;

            try
            {
                using (var bd = new Contexto())
                {
                    _estadoRepositorio = new EstadoRepositorio(bd);
                    estadoViewModel.Estados = _estadoRepositorio.Buscar(estadoViewModel);
                    estadoViewModel.TotalEncontrados = _estadoRepositorio.ObtenerTotalRegistros(estadoViewModel);
                    estadoViewModel.Paginas = Paginar.ObtenerCantidadPaginas(estadoViewModel.TotalEncontrados, estadoViewModel.TamanoPagina);
                }
            }
            catch (Exception ex)
            {
                //EscribirError(ex.Message);
            }
            LimpiarModelState();
            return View(estadoViewModel);
        }

        [HttpGet]
        public ActionResult Guardar(string idEstado)
        {
            var estado = new Estado { IDEstado = idEstado.DecodeFrom64().TryToInt() };

            if (estado.IDEstado > 0)
            {
                try
                {
                    using (var bd = new Contexto())
                    {
                        _estadoRepositorio = new EstadoRepositorio(bd);
                        estado = _estadoRepositorio.BuscarPorId(estado.IDEstado);
                    }
                }
                catch (Exception ex)
                {
                    EscribirError(ex.Message);
                }
            }
            return View(estado);
        }

        [HttpPost]
        public ActionResult Guardar(Estado estado)
        {
            var correcto = false;
            estado.IDEstado = Request.QueryString["idEstado"].DecodeFrom64().TryToInt();
            ModelState["idEstado"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    estado.IDEstado = ObtenerParametroGetEnInt(Enumerados.Parametro.IdEstado);
                    using (var bd = new Contexto())
                    {
                        _estadoRepositorio = new EstadoRepositorio(bd);
                        
                        if (estado.IDEstado > 0)
                        {
                            _estadoRepositorio.Modificar(estado);
                        }
                        else
                        {
                            _estadoRepositorio.Guardar(estado);
                        }
                        correcto = bd.SaveChanges() >= 1;
                    }
                }
                catch (Exception ex)
                {
                    //EscribirError(ex.Message);
                }
            }

            EnviarAlerta(General.GuardadoCorrecto, string.Format(General.FalloGuardado, General.Estado), correcto);
            return View(estado);
        }

        // GET: Catalogo/Estado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // GET: Catalogo/Estado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalogo/Estado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDEstado,Nombre,Abreviatura")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Estado.Add(estado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estado);
        }

        // GET: Catalogo/Estado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Catalogo/Estado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDEstado,Nombre,Abreviatura")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        // GET: Catalogo/Estado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estado.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Catalogo/Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estado estado = db.Estado.Find(id);
            db.Estado.Remove(estado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
