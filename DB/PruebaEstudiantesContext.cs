using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class PruebaEstudiantesContext : DbContext
    {
        public PruebaEstudiantesContext(DbContextOptions<PruebaEstudiantesContext> options) : base(options)
        {

        }

        public DbSet<Notas> Notas { get; set; }
        public DbSet<Profesores> Profesores { get; set; }
        public DbSet<Estudiantes> Estudiantes { get; set; }
    }
}
