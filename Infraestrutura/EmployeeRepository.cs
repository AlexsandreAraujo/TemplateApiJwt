using TemplateApiJwt.Model;

namespace TemplateApiJwt.Infraestrutura;

public class EmployeeRepository : IEmployeeRepository
{
  private readonly ConnectionContext _context = new ConnectionContext();

  public void Add(Employee employee)
  {
    _context.Employees.Add(employee);
    _context.SaveChanges();
  }

  public Employee? Get(int id)
  {
    return _context.Employees.Find(id);
  }

  public List<Employee> GetAll(int pageNumber, int pageQuantity)
  {
    return _context.Employees.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToList();
  }
}
