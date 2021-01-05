using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Act2U4_171G0250.Models;

namespace Act2U4_171G0250.Repositories
{
    public class Repository<T> where T : class
    {
        public rolesusContext Context { get; set; }

        public Repository(rolesusContext cxc)
        {
            Context = cxc;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public T Get(object id)
        {
            return Context.Find<T>(id);
        }
        public virtual bool Validar(T entidad)
        {
            return true;
        }
        public virtual void Insert(T entidad)
        {
            if (Validar(entidad))
            {
                Context.Add(entidad);
                Context.SaveChanges();
            }
        }
        public virtual void Update(T entidad)
        {
            if (Validar(entidad))
            {
                Context.Update<T>(entidad);
                Context.SaveChanges();
            }
        }
        public virtual void Delete(T entidad)
        {
            Context.Remove<T>(entidad);
            Context.SaveChanges();
        }
    }
}
