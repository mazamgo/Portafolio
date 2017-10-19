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
    public class ExperienciaController : Controller
    {
        // GET: Admin/Experiencia

        Experiencia experiencia = new Experiencia();
        public ActionResult Index(int tipo = 1)
        {
            ViewBag.tipo = tipo;
            ViewBag.Title = tipo == 1 ? "Trabajos realizados" : "Estudios previos";
            return View();
        }

        public ActionResult Crud (byte tipo=0, int id = 0)
        {
            if(id == 0)
            {
                if (tipo == 0) return Redirect("~/Admin/Experiencia/");

                experiencia.Tipo = tipo;
                experiencia.Usuario_id = SessionHelper.GetUser();
            }
            else
            {
                experiencia = experiencia.Obtener(id);
            }
            
            return View(experiencia);
        }

        public JsonResult Guardar(Experiencia model)
        {
            ResponseModel rm = new ResponseModel();
            rm = model.Guardar();

            if (rm.response)
            {
                rm.href = "/Admin/Experiencia/Index?tipo="+model.Tipo;          
            }

            return Json(rm);
        }
      
        public JsonResult Eliminar(int id)
        {
            ResponseModel rm = new ResponseModel();
            rm = experiencia.Eliminar(id);
            //if (rm.response) rm.href = "self";

            return Json(rm,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Listar(AnexGRID grid, int tipo)
        {
            return Json(experiencia.Listar(grid, tipo, SessionHelper.GetUser()));
        }

    }
}