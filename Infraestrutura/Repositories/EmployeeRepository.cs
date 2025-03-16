using TemplateApiJwt.Domain.Model.EmployeeAggregate;

namespace TemplateApiJwt.Infraestrutura.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
  private readonly ConnectionContext _context;

  public EmployeeRepository(ConnectionContext context)
  {
    _context = context;
  }

  public void Add(Employee employee)
  {
    _context.Employees.Add(employee);
    _context.SaveChanges();
  }

  public List<Employee> GetAll(int pageNumber, int pageQuantity)
  {
    return _context.Employees.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToList();
  }

  public Employee? Get(int id)
  {
    return _context.Employees.Find(id);
  }
}
