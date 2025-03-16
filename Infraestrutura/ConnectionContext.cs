using Microsoft.EntityFrameworkCore;
using TemplateApiJwt.Model;

namespace TemplateApiJwt.Infraestrutura;

public class ConnectionContext : DbContext
{
  public DbSet<Employee> Employees { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql(
      "Server=localhost;" +
      "Port=5432;" +
      "Database=HomeDelivery_Prod;" +
      "User Id=usr_postgres;" +
      "Password = usr_postgres;"
    );
  }
}
