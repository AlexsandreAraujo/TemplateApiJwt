using Microsoft.EntityFrameworkCore;
using TemplateApiJwt.Domain.Model.CompanyAggregate;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;

namespace TemplateApiJwt.Infraestrutura;

public class ConnectionContext : DbContext
{
  public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options) { }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Company> Companies { get; set; }

  // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  // {
  //   optionsBuilder.UseNpgsql(
  //     "Server=localhost;" +
  //     "Port=5432;" +
  //     "Database=HomeDelivery_Prod;" +
  //     "User Id=usr_postgres;" +
  //     "Password = usr_postgres;"
  //   );
  // }
}
