using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Act2U4_171G0250.Models;


namespace Act2U4_171G0250.Repositories
{
    public class MRepository : Repository<Maestro>
    {
        public MRepository(rolesusContext cxc) : base(cxc)
        {

        }
        public Maestro GetMaestrosByUsuario(string usuario)
        {
            return Context.Maestro.FirstOrDefault(x => x.Usuario == usuario);
        }
        public Maestro GetAlumnosById(int id)
        {
            return Context.Maestro.Include(x => x.Alumno).FirstOrDefault(x => x.Id == id);
        }
        public override bool Validar(Maestro entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("Introduzca el nombre del maestro");
            if (string.IsNullOrWhiteSpace(entidad.Contrasena))
                throw new Exception("Introduzca la contraseña");
            if (entidad.Contrasena.Length < 6 )
                throw new Exception("La contraseña debe tener entre 6 y 12 caracteres");
            return true;
        }
    }
}

