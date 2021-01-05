using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Act2U4_171G0250.Models;


namespace Act2U4_171G0250.Repositories
{
    public class ARepository : Repository<Alumno>
    {
        public ARepository(rolesusContext cxc) : base(cxc)
        {

        }

        public Alumno GetAlumnosByNControl(string nControl)
        {

            return Context.Alumno.FirstOrDefault(x => x.Ncontrol.ToUpper() == nControl.ToUpper());

        }


        public override bool Validar(Alumno entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("Introduzca el nombre del alumno");
            if (string.IsNullOrWhiteSpace(entidad.Ncontrol))
                throw new Exception("Introduzca el número de control del alumno");
            if (entidad.IdMaestro <= 0 )
                throw new Exception("Asigne al alumno su maestro");
            return true;
        }
    }
}