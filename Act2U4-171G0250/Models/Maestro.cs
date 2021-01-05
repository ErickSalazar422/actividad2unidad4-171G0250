using System;
using System.Collections.Generic;

namespace Act2U4_171G0250.Models
{
    public partial class Maestro
    {
        public Maestro()
        {
            Alumno = new HashSet<Alumno>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Usuario { get; set; }
        public ulong Mact { get; set; }

        public virtual ICollection<Alumno> Alumno { get; set; }
    }
}
