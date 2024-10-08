using DMAWS_T2305M_Tran_Dinh_Lan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly DMAWSContext _context;

    public EmployeesController(DMAWSContext context)
    {
        _context = context;
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Projects)
            .ToListAsync();

        return Ok(employees.Select(e => new
        {
            e.EmployeeId,
            e.EmployeeName,
            e.EmployeeDOB,
            e.EmployeeDepartment,
            Projects = e.ProjectEmployees.Select(pe => new
            {
                pe.Projects.ProjectId,
                pe.Projects.ProjectName,
                pe.Tasks
            })
        }));
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Projects)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null) return NotFound();

        return Ok(new
        {
            employee.EmployeeId,
            employee.EmployeeName,
            employee.EmployeeDOB,
            employee.EmployeeDepartment,
            Projects = employee.ProjectEmployees.Select(pe => new
            {
                pe.Projects.ProjectId,
                pe.Projects.ProjectName,
                pe.Tasks
            })
        });
    }

    // POST: api/Employees
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        if (id != employee.EmployeeId || !ModelState.IsValid)
            return BadRequest();

        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/Employees/search
    [HttpGet("search")]
    public async Task<IActionResult> SearchEmployees(string employeeName, DateTime? employeeDOBFromDate = null, DateTime? employeeDOBToDate = null)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(employeeName))
        {
            query = query.Where(e => e.EmployeeName.Contains(employeeName));
        }

        if (employeeDOBFromDate.HasValue)
        {
            query = query.Where(e => e.EmployeeDOB >= employeeDOBFromDate);
        }

        if (employeeDOBToDate.HasValue)
        {
            query = query.Where(e => e.EmployeeDOB <= employeeDOBToDate);
        }

        var employees = await query.ToListAsync();

        return Ok(employees);
    }
}