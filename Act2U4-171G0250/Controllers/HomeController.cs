using Act2U4_171G0250.Helpers;
using Act2U4_171G0250.Models;
using Act2U4_171G0250.Models.ViewModels;
using Act2U4_171G0250.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Act2U4_171G0250.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        

        [AllowAnonymous]
        public IActionResult InicioDeSesionDirector()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> InicioDeSesionDirector(Director D)
        {
            rolesusContext context = new rolesusContext();
            Repository<Director> repository = new Repository<Director>(context);
            var director = context.Director.FirstOrDefault(x => x.Usuario == D.Usuario);
            try
            {
                if (director != null && director.Contraseña == D.Contraseña)
                {
                    List<Claim> info = new List<Claim>();
                    info.Add(new Claim(ClaimTypes.Name, "Us" + director.Nombre));
                    info.Add(new Claim("Usuario", director.Usuario));
                    info.Add(new Claim(ClaimTypes.Role, "Director"));
                    info.Add(new Claim("Nombre", director.Nombre));

                    var ClaimsIdentity = new ClaimsIdentity(info, CookieAuthenticationDefaults.AuthenticationScheme);
                    var ClaimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ClaimsPrincipal, new AuthenticationProperties
                    { IsPersistent = true });
                    return RedirectToAction("Aprobado");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                    return View(D);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(D);
            }
        }


        [AllowAnonymous]
        public IActionResult InicioDeSesionMaestro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InicioDeSesionMaestro(Maestro M)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.GetMaestrosByUsuario(M.Usuario);
            try
            {
                if (maestro != null && maestro.Contrasena == HashingHelper.GetHash(M.Contrasena))
                {
                    if (maestro.Mact == 1)
                    {
                        List<Claim> info = new List<Claim>();
                        info.Add(new Claim(ClaimTypes.Name, "Us" + maestro.Nombre));
                        info.Add(new Claim("Usuario", maestro.Usuario.ToString()));
                        info.Add(new Claim(ClaimTypes.Role, "Maestro"));
                        info.Add(new Claim("Nombre", maestro.Nombre));
                        info.Add(new Claim("Id", maestro.Id.ToString()));

                        var ClaimsIdentity = new ClaimsIdentity(info, CookieAuthenticationDefaults.AuthenticationScheme);
                        var ClaimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ClaimsPrincipal);
                        return RedirectToAction("Aprobado");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cuenta inactiva, comuniquese  con el director para activarla.");
                        return View(M);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                    return View(M);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(M);
            }
        }


        [Authorize(Roles = "Director, Maestro")]
        public IActionResult Aprobado(int usuario)
        {
            return View();
        }



        [Authorize(Roles = "Director")]
        public IActionResult Maestros()
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var ListaDeMaestros = repository.GetAll();
            return View(ListaDeMaestros);
        }


        [Authorize(Roles = "Director")]
        public IActionResult AñadirMaestros()
        {
            return View();
        }




        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult AñadirMaestros(Maestro M)
        {
           rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            try
            {
                var verify = repository.GetMaestrosByUsuario(M.Usuario);
                if (verify != null)
                {
                    ModelState.AddModelError("", "Ya existe un profesor con esta clave");
                    return View(M);
                }
                else
                {
                    M.Mact = 1;
                    M.Contrasena = HashingHelper.GetHash(M.Contrasena);
                    repository.Insert(M);
                    return RedirectToAction("Maestros");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(M);
            }
        }




        [Authorize(Roles = "Director")]
        public IActionResult EditarDatosM(int id)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.Get(id);
            if (maestro == null)
            {
                return RedirectToAction("Maestros");
            }
            return View(maestro);
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult EditarDatosM(Maestro M)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.Get(M.Id);
            try
            {
                if (maestro != null)
                {
                    maestro.Nombre = M.Nombre;
                    maestro.Usuario = M.Usuario;
                   
                    repository.Update(maestro);
                }
                return RedirectToAction("Maestros");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(maestro);
            }
        }




        [Authorize(Roles = "Director")]
        public IActionResult CambiarContraseñaM(int id)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.Get(id);
            if (maestro == null)
            {
                return RedirectToAction("Maestros");
            }
            return View(maestro);
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult CambiarContraseñaM(Maestro p, string contraseña, string confcontraseña)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.Get(p.Id);
            try
            {
                if (maestro != null)
                {
                    if (contraseña == maestro.Contrasena)
                    {
                        ModelState.AddModelError("", "La nueva contraseña no puedo ser igual a la actual.");
                        return View(maestro);
                    }
                    else
                    {
                        if (contraseña == confcontraseña)
                        {
                            maestro.Contrasena = contraseña;
                            maestro.Contrasena = HashingHelper.GetHash(contraseña);
                            repository.Update(maestro);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Las contraseñas no coinciden");
                            return View(maestro);
                        }
                    }
                }
                return RedirectToAction("Maestros");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(maestro);
            }
        }

        [HttpPost]
        public IActionResult DesactivarM(Maestro M)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.Get(M.Id);
            if (maestro != null && maestro.Mact == 1)
            {
                maestro.Mact = 0;
                repository.Update(maestro);
            }
            else
            {
                maestro.Mact = 1;
                repository.Update(maestro);
            }
            return RedirectToAction("Maestros");
        }



        public async Task<IActionResult> CierreDeSesion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Director, Maestro")]
        public IActionResult Alumnos(int id)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            var maestro = repository.GetAlumnosById(id);
            if (maestro != null)
            {
                return View(maestro);
            }
            else
                return RedirectToAction("aprobado");
        }
        [Authorize(Roles = "Director, Maestro")]
        public IActionResult AñadirAlumno(int id)
        {
            rolesusContext context = new rolesusContext();
            MRepository repository = new MRepository(context);
            AlumnoViewModel AVM = new AlumnoViewModel();
            AVM.Maestro = repository.Get(id);
            return View(AVM);
        }
        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult AñadirAlumno(AlumnoViewModel AVM)
        {
            rolesusContext context = new rolesusContext();
            MRepository mrepository = new MRepository(context);
            ARepository arepository = new ARepository(context);
            try
            {
                var IdMaestro = mrepository.GetMaestrosByUsuario(AVM.Maestro.Usuario).Id;
                AVM.Alumno.IdMaestro = IdMaestro;
                arepository.Insert(AVM.Alumno);
                return RedirectToAction("Alumnos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(AVM);
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        public IActionResult EditarDatosAlumno(int id)
        {
            rolesusContext context = new rolesusContext();
            MRepository mrepository = new MRepository(context);
            ARepository arepository = new ARepository(context);
            AlumnoViewModel AVM = new AlumnoViewModel();
            AVM.Alumno = arepository.Get(id);
            AVM.Maestros = mrepository.GetAll();
            if (AVM.Alumno != null)
            {
                if (User.IsInRole("Maestro"))
                {
                    AVM.Maestro = mrepository.Get(AVM.Alumno.IdMaestro);
                    if (User.Claims.FirstOrDefault(x => x.Type == "Usuario").Value == AVM.Maestro.Usuario)
                    {
                        return View(AVM);
                    }
                    else
                    {
                        return RedirectToAction("Alumnos");
                    }
                }
                else return View(AVM);
            }
            else return RedirectToAction("Alumnos");
        }


        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult EditarDatosAlumno(AlumnoViewModel avm)
        {
            rolesusContext context = new rolesusContext();
            MRepository mrepository = new MRepository(context);
            ARepository arepository = new ARepository(context);
            try
            {
                var alumno = arepository.Get(avm.Alumno.Id);
                if (alumno != null)
                {
                    alumno.Nombre = avm.Alumno.Nombre;
                    if (User.IsInRole("Director"))
                    {
                        alumno.IdMaestro = avm.Alumno.IdMaestro;
                    }
                    arepository.Update(alumno);
                    return RedirectToAction("Alumnos");
                }
                else
                {
                    ModelState.AddModelError("", "Este alumno no se encuentra registrado");
                    avm.Maestro = mrepository.Get(avm.Alumno.IdMaestro);
                    avm.Maestros = mrepository.GetAll();
                    return View(avm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                avm.Maestro = mrepository.Get(avm.Alumno.IdMaestro);
                avm.Maestros = mrepository.GetAll();
                return View(avm);
            }
        }
        [Authorize(Roles = "Director, Maestro")]
        [HttpPost]
        public IActionResult EliminarAlumno(Alumno A)
        {
            rolesusContext context = new rolesusContext();
            ARepository repository = new ARepository(context);
            var alumno = repository.Get(A.Id);
            if (alumno != null)
            {
                repository.Delete(alumno);
            }
            else
            {
                ModelState.AddModelError("", "Este alumno no se encuentra registrado");
            }
            return RedirectToAction("Alumnos");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
