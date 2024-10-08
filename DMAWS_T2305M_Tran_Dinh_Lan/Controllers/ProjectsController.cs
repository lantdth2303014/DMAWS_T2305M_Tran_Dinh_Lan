using DMAWS_T2305M_Tran_Dinh_Lan.Data;
using DMAWS_T2305M_Tran_Dinh_Lan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly DMAWSContext _context;

    public ProjectsController(DMAWSContext context)
    {
        _context = context;
    }

    // GET: api/Projects
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projects
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employees)
            .ToListAsync();

        return Ok(projects.Select(p => new
        {
            p.ProjectId,
            p.ProjectName,
            p.ProjectStartDate,
            p.ProjectEndDate,
            Employees = p.ProjectEmployees.Select(pe => new
            {
                pe.Employees.EmployeeId,
                pe.Employees.EmployeeName,
                pe.Tasks
            })
        }));
    }

    // GET: api/Projects/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employees)
            .FirstOrDefaultAsync(p => p.ProjectId == id);

        if (project == null) return NotFound();

        return Ok(new
        {
            project.ProjectId,
            project.ProjectName,
            project.ProjectStartDate,
            project.ProjectEndDate,
            Employees = project.ProjectEmployees.Select(pe => new
            {
                pe.Employees.EmployeeId,
                pe.Employees.EmployeeName,
                pe.Tasks
            })
        });
    }

    // POST: api/Projects
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
    }

    // PUT: api/Projects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
    {
        if (id != project.ProjectId || !ModelState.IsValid)
            return BadRequest();

        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Projects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/Projects/search
    [HttpGet("search")]
    public async Task<IActionResult> SearchProjects(string projectName, bool? inProgress = null)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(projectName))
        {
            query = query.Where(p => p.ProjectName.Contains(projectName));
        }

        if (inProgress.HasValue)
        {
            if (inProgress.Value)
            {
                query = query.Where(p => !p.ProjectEndDate.HasValue || p.ProjectEndDate > DateTime.Now); // Đang thực hiện
            }
            else
            {
                query = query.Where(p => p.ProjectEndDate.HasValue && p.ProjectEndDate < DateTime.Now); // Đã hoàn thành
            }
        }

        var projects = await query.ToListAsync();
        return Ok(projects);
    }
}