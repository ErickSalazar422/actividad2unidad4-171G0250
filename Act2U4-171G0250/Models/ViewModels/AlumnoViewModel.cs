using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Act2U4_171G0250.Models.ViewModels
{
    public class AlumnoViewModel
    {
        public Alumno Alumno { get; set; }
        public Maestro Maestro { get; set; }
        public IEnumerable<Maestro> Maestros { get; set; }
    }
}
