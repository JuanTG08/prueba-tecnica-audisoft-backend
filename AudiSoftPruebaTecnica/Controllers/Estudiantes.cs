using DB;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoftPruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Estudiantes
    {
        private PruebaEstudiantesContext _context;

        public Estudiantes(PruebaEstudiantesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DB.Estudiantes> GetEstudiantes() => _context.Estudiantes.Where(estudiante => estudiante.status == true).ToList();

        [HttpPost]
        public async Task<IActionResult> PostEstudiante([FromBody] DB.Estudiantes estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult(nameof(GetEstudiantes), nameof(Estudiantes), new { id = estudiante.id_estudiante }, estudiante);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstudiante(int id)
        {
            if (id < 0)
            {
                return new NotFoundResult();
            }

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(estudiante);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, [FromBody] DB.Estudiantes estudiante)
        {
            if (id != estudiante.id_estudiante)
            {
                return new BadRequestResult();
            }
            var estudianteDB = await _context.Estudiantes.FindAsync(id);
            if (estudianteDB == null)
            {
                return new NotFoundResult();
            }

            estudianteDB.nombre = estudiante.nombre;
            estudianteDB.status = estudiante.status;

            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetEstudiante), nameof(Estudiantes), new { id = id }, estudiante);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudianteDB = await _context.Estudiantes.FindAsync(id);
            if (estudianteDB == null)
            {
                return new NotFoundResult();
            }
            estudianteDB.status = false;
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetEstudiante), nameof(Estudiantes), new { id = id }, estudianteDB);
        }

    }
}
