using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AudiSoftPruebaTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Notas
    {
        private PruebaEstudiantesContext _context;

        public Notas(PruebaEstudiantesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DB.Notas> GetNotas() => _context.Notas.Include(n => n.Profesores).Include(n => n.Estudiantes).Where(notas => notas.status == true).ToList();

        [HttpPost]
        public async Task<IActionResult> PostNotas([FromBody] DB.Notas notas)
        {
            try
            {
                // Check if a Profesores record with the corresponding id_profesor exists
                if (!_context.Profesores.Any(p => p.id_profesor == notas.id_profesor))
                {
                    return new BadRequestObjectResult("The provided id_profesor does not exist in the Profesores table.");
                }

                _context.Notas.Add(notas);
                await _context.SaveChangesAsync();

                return new CreatedAtActionResult(nameof(GetNotas), nameof(Notas), new { id = notas.id_nota }, notas);
            }
            catch (DbUpdateException ex)
            {
                // Handle DbUpdateException (including potential duplicate key violation)
                if (ex.InnerException?.Message?.Contains("Duplicate entry") ?? false)
                {
                    return new BadRequestObjectResult("Duplicate entry for id_profesor. Please ensure a unique id_profesor is used.");
                }
                else
                {
                    // Handle other DbUpdateException scenarios (e.g., validation errors)
                    return new StatusCodeResult(500);
                }
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNota(int id)
        {
            if (id < 0)
            {
                return new NotFoundResult();
            }

            var nota = await _context.Notas.FindAsync(id);
            if (nota == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(nota);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNota(int id, [FromBody] DB.Notas nota)
        {
            if (id != nota.id_nota)
            {
                return new BadRequestResult();
            }
            var notaDB = await _context.Notas.FindAsync(id);
            if (notaDB == null)
            {
                return new NotFoundResult();
            }

            notaDB.nombre = nota.nombre;
            notaDB.id_profesor = nota.id_profesor;
            notaDB.id_estudiante = nota.id_estudiante;
            notaDB.valor = nota.valor;
            notaDB.status = nota.status;

            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetNota), nameof(Notas), new { id = id }, nota);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNota(int id)
        {
            var notaDB = await _context.Notas.FindAsync(id);
            if (notaDB == null)
            {
                return new NotFoundResult();
            }
            notaDB.status = false;
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetNota), nameof(Notas), new { id = id }, notaDB);
        }
    }
}
