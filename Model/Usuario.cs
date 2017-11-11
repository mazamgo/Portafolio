namespace Model
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Web;
    using System.IO;

    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Experiencia = new HashSet<Experiencia>();
            Habilidad = new HashSet<Habilidad>();
            Testimonio = new HashSet<Testimonio>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Column(TypeName = "text")]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Ciudad { get; set; }

        public int? Pais_id { get; set; }

        [StringLength(50)]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Twitter { get; set; }

        [StringLength(100)]
        public string YouTube { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        public virtual ICollection<Experiencia> Experiencia { get; set; }

        public virtual ICollection<Habilidad> Habilidad { get; set; }

        public virtual ICollection<Testimonio> Testimonio { get; set; }

        [NotMapped]
        public TablaDato pais { get; set; }

        public ResponseModel Acceder(string Email, string Password) 
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext()) 
                {
                    Password = HashHelper.MD5(Password);

                    var usuario = ctx.Usuario.Where(x => x.Email == Email)
                                             .Where(x => x.Password == Password)
                                             .SingleOrDefault();

                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.id.ToString());
                        rm.SetResponse(true);
                    }
                    else 
                    {
                        rm.SetResponse(false, "Correo o contraseña incorrecta");
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return rm;
        }

        public Usuario Obtener(int id, bool includes = false) 
        {
            var usuario = new Usuario();

            try
            {
                using (var ctx = new ProyectoContext()) 
                {
                    if (!includes)
                    {
                        usuario = ctx.Usuario.Where(x => x.id == id)
                                         .SingleOrDefault();
                    }
                    else
                    {
                        usuario = ctx.Usuario
                                     .Include("Experiencia")
                                     .Include("Habilidad")
                                     .Include("Testimonio")
                                     .Where(x => x.id == id)
                                     .SingleOrDefault();
                    }

                    // Trayendo un registro adicional, sin usar Include
                    usuario.pais = new TablaDato().Obtener("pais", usuario.Pais_id.ToString());
                    
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return usuario;
        }

        //Para HttpPostedFileBase: Se agrego Proyecto Model la referencia desde Assembly Sysmtem.Web
        public ResponseModel Guardar(HttpPostedFileBase Foto, String Password ) 
        {
            ResponseModel rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    var usr = ctx.Entry(this);
                    usr.State = EntityState.Modified;

                    if (this.Foto != null)
                    {
                        string archivo = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Foto.FileName);
                        Foto.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads" + archivo));
                        this.Foto = archivo;
                    }
                    else
                    {
                        usr.Property(x => x.Foto).IsModified = false;
                    }

                    if (this.Password == null)
                    {
                        usr.Property(x => x.Password).IsModified = false;
                    }
                    else
                    {
                        this.Password = Helper.HashHelper.MD5(Password);
                    }

                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }

            }
            catch (DbEntityValidationException e) 
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return rm;
                 
        }
    }
}
