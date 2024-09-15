using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Identity.Controllers
{
    public class ProjectsController : Controller
    {
        public ProjectsDbContext _projectsDbContext;
        public AppIdentityDbContext _appIdentityDbContext;
        public CadDbContext _cadDbContext;

        private readonly UserManager<AppUser> _userManager;

        public ProjectsController(ProjectsDbContext projectsDbContext, AppIdentityDbContext appIdentityDbContext, CadDbContext cadDbContext, UserManager<AppUser> userManager)
        {
            _projectsDbContext = projectsDbContext;
            _appIdentityDbContext = appIdentityDbContext;
            _cadDbContext = cadDbContext;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Index()
        {
            List<Project> projects;

            if (User.IsInRole("admin"))
            {
                projects = await _projectsDbContext.Projects.ToListAsync();
            }
            else
            {
                var user = await _appIdentityDbContext.Users.FindAsync(_userManager.GetUserId(User));
                projects = await _projectsDbContext.Projects.Where(p => p.NameEmployers == user.Email).ToListAsync();
            }

            return View(projects);
        }
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Project(int Id)
        {
            var p = await _projectsDbContext.Projects.FindAsync(Id);

            IEnumerable<CADFile> files = await _cadDbContext.CADFiles.Where(p => p.IdProject == Id)
            .ToListAsync(); 

            return View(new ProjectAndCad(p,files.Reverse(),Id));
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View(_appIdentityDbContext.Users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            Console.WriteLine(project.Name);
            _projectsDbContext.Projects.Add(project);
            await _projectsDbContext.SaveChangesAsync();
            return View("Index", await _projectsDbContext.Projects.ToListAsync());
        }

        public async Task<IActionResult> UploadFile(int Id)
        {
            Console.WriteLine(Id);
            ViewData["Id"] = Id;

            var p = await _projectsDbContext.Projects.FindAsync(Id);

            var files = await _cadDbContext.CADFiles.Where(p => p.Id == Id)
            .ToListAsync();

            return View(files);
        }
    }
}
