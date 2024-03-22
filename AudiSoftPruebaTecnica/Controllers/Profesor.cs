using DB;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoftPruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Profesor
    {
        private PruebaEstudiantesContext _context;
        public Profesor(PruebaEstudiantesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DB.Profesores> GetProfesores() => _context.Profesores.Where(profesor => profesor.status == true).ToList();
        
        [HttpPost]
        public async Task<IActionResult> PostProfesor([FromBody] DB.Profesores profesor)
        {
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult(nameof(GetProfesores), nameof(Profesor), new { id = profesor.id_profesor}, profesor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfesor(int id)
        {
            if (id < 0)
            {
                return new NotFoundResult();
            }

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(profesor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfesor(int id, [FromBody] DB.Profesores profesor)
        {
            if (id != profesor.id_profesor)
            {
                return new BadRequestResult();
            }
            var profesorDB = await _context.Profesores.FindAsync(id);
            if (profesorDB == null)
            {
                return new NotFoundResult();
            }

            profesorDB.nombre = profesor.nombre;
            profesorDB.status = profesor.status;

            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetProfesor), nameof(Profesor), new { id = id }, profesor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesor(int id)
        {
            var profesorDB = await _context.Profesores.FindAsync(id);
            if (profesorDB == null)
            {
                return new NotFoundResult();
            }
            profesorDB.status = false;
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetProfesor), nameof(Profesor), new { id = id }, profesorDB);
        }
    }
}
