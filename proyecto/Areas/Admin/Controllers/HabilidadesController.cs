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
    public class HabilidadesController : Controller
    {
        Habilidad habilidad = new Habilidad();

        // GET: Admin/Habilidades
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            if (id == 0)
            {
                // if (tipo == 0) return Redirect("~/Admin/Habilidad/");
                habilidad.Usuario_id = SessionHelper.GetUser();
            }
            else
            {
                habilidad = habilidad.Obtener(id);
            }

            return View(habilidad);
        }

        public JsonResult Guardar(Habilidad model)
        {
            ResponseModel rm = new ResponseModel();
            rm = model.Guardar();

            if (rm.response)
            {
                rm.href = "/Admin/Habilidades";
            }

            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            ResponseModel rm = new ResponseModel();
            rm = habilidad.Eliminar(id);
            //if (rm.response) rm.href = "self";

            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(habilidad.Listar(grid, SessionHelper.GetUser()));
        }


    }
}