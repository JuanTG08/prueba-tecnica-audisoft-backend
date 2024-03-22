using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Notas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_nota { get; set; }
        public string nombre { get; set; }
        public int id_profesor { get; set; }
        public int id_estudiante { get; set; }
        public int valor { get; set; }
        public bool status { get; set; }

        [ForeignKey("id_profesor")]
        public virtual Profesores? Profesores { get; set; }

        [ForeignKey("id_estudiante")]
        public virtual Estudiantes? Estudiantes { get; set; }
    }
}
