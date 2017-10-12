using Helper;
using Model;
using proyecto.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Areas.Admin.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private TablaDato paises = new TablaDato();
        private Usuario usuario = new Usuario();

        // GET: Admin/Usuario
        public ActionResult Index()
        {
            ViewBag.Paises = paises.Listar("Pais");
            return View(usuario.Obtener(SessionHelper.GetUser()));
        }

        public JsonResult Guardar(Usuario usuario, HttpPostedFileBase Foto, String Password) 
        {
            var rm = new ResponseModel();

            ModelState.Remove("Password");

            if (ModelState.IsValid) 
            {
                rm = usuario.Guardar(Foto,Password);
                rm.SetResponse(true);
            }

            return Json(rm);

        }
    }
}