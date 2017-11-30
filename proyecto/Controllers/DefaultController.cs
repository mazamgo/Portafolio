using Model;
using proyecto.App_Start;
using proyecto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class DefaultController : Controller
    {
        private Usuario usuario = new Usuario();

        public ActionResult Index()
        {
            return View(usuario.Obtener(FrontOfficeStartUp.Usuariovisualizando(),true));
        }

        public JsonResult EnviarCorreo (ContactoViewModels model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var _usuario = usuario.Obtener(FrontOfficeStartUp.Usuariovisualizando());

                    var mail = new MailMessage();
                    mail.From = new MailAddress(model.Correo, model.Nombre);
                    mail.To.Add(_usuario.Email);
                    mail.Subject = "Correo desde Contacto.";
                    mail.IsBodyHtml = true;
                    mail.Body = model.Mensaje;

                    var SmtpServer = new SmtpClient("smtp.live.com"); //smtp.gmail.com
                    SmtpServer.Port = 587;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("mzambranag@hotmail.com", "xxx");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);


                }
                catch (Exception e)
                {
                    rm.SetResponse(false,e.Message);
                    return Json(rm);
                    throw;
                }

                rm.SetResponse(true);
                rm.function = "CerrarContacto();";
            }


            return Json(rm);
        }

        public ActionResult ExportAPDF()
        {
            return new Rotativa.MVC.ActionAsPdf("PDF");
        }

        public ActionResult PDF()
        {
            return View(usuario.Obtener(FrontOfficeStartUp.Usuariovisualizando(),true));
        }

    }
}