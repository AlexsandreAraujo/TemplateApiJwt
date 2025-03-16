using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateApiJwt.Domain.Model.CompanyAggregate;

[Table("company")]
public class Company
{
  [Key]
  public int id { get; set; }
  public string name { get; set; }
}
