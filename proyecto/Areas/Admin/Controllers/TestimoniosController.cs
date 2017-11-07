using proyecto.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Helper;

namespace proyecto.Areas.Admin.Controllers
{
    [Autenticado]
    public class TestimoniosController : Controller
    {
        private Testimonio testimonio = new Testimonio();
        // GET: Admin/Testimonios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crud(int id)
        {
            if (id != 0)
            {
                testimonio = testimonio.Obtener(id);
            }

            return View(testimonio);
        }

        public JsonResult Guardar(Testimonio model)
        {
            ResponseModel rm = new ResponseModel();
            rm = model.Guardar();

            if (rm.response)
            {
                rm.href = "/Admin/Testimonios";
            }

            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            ResponseModel rm = new ResponseModel();
            rm = testimonio.Eliminar(id);
            //if (rm.response) rm.href = "self";

            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(testimonio.Listar(grid, SessionHelper.GetUser()));
        }


    }
}