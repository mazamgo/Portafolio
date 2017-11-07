namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;
    

    [Table("Testimonio")]
    public partial class Testimonio
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required]
        [StringLength(50)]
        public string IP { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Comentario { get; set; }

        [Required]
        [StringLength(10)]
        public string Fecha { get; set; }

        public int estado { get; set; }

        public virtual Usuario Usuario { get; set; }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    if (this.id > 0)
                    {
                        ctx.Entry(this).State = EntityState.Modified;
                    }
                    else
                    { ctx.Entry(this).State = EntityState.Added; }

                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return (rm);
        }

        public ResponseModel Eliminar(int id)
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new ProyectoContext())
                {
                    if (id > 0)
                    {
                        this.id = id;
                        ctx.Entry(this).State = EntityState.Deleted;
                        ctx.SaveChanges();
                        rm.SetResponse(true);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return (rm);
        }

        public Testimonio Obtener(int id)
        {
            var testimonio = new Testimonio();
            try
            {
                using (var ctx = new ProyectoContext())
                {
                    testimonio = ctx.Testimonio.Where(x => x.id == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return testimonio;
        }

        public AnexGRIDResponde Listar(AnexGRID grid, int usuario_id)
        {
            try
            {
                using (var ctx = new ProyectoContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    
                    grid.Inicializar();

                    //var query = ctx.Testimonio.Where(x => x.Usuario_id == usuario_id);
                    var query = ctx.Testimonio
                                       .Join(
                                          ctx.TablaDato,
                                          t => t.estado.ToString(),
                                          d => (d.Valor),
                                          (t, d) =>
                                             new
                                             {
                                                 t = t,
                                                 d = d
                                             }
                                       )
                                       .Where(x => ((x.t.Usuario_id == usuario_id) && (x.d.Relacion == "testimonioestado")))
                                       .Select(
                                          x =>
                                             new
                                             {
                                                 id = x.t.id,
                                                 Usuario_id = x.t.Usuario_id,
                                                 IP = x.t.IP,
                                                 Nombre = x.t.Nombre,
                                                 Comentario = x.t.Comentario,
                                                 Fecha = x.t.Fecha,
                                                 Estado = x.d.Descripcion
                                             }
                                       );

                    //id, Usuario_id, IP, Nombre, Comentario, Fecha
                    if (grid.columna == "id") query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.id) : query.OrderBy(x => x.id);
                    if (grid.columna == "Nombre") query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Nombre) : query.OrderBy(x => x.Nombre);
                    if (grid.columna == "Estado") query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Estado) : query.OrderBy(x => x.Estado);
                    if (grid.columna == "IP") query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.IP) : query.OrderBy(x => x.IP);
                    if (grid.columna == "Fecha") query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Fecha) : query.OrderBy(x => x.Fecha);

                    var habilidad = query.Skip(grid.pagina).Take(grid.limite).ToList();
                    var total = query.Count();

                    grid.SetData(habilidad, total);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return grid.responde();
        }

    }
}
