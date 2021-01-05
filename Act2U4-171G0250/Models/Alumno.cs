using System;
using System.Collections.Generic;

namespace Act2U4_171G0250.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ncontrol { get; set; }
        public int IdMaestro { get; set; }

        public virtual Maestro IdMaestroNavigation { get; set; }
    }
}
