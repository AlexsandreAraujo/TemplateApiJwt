using AutoMapper;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;
using TemplateApiJwt.Domain.DTOs;

namespace TemplateApiJwt.Application.Mapping;

public class DomainToDTOMapping : Profile
{
  public DomainToDTOMapping()
  {
    CreateMap<Employee, EmployeeDTO>()
      .ForMember(dest => dest.NameEmployee, m => m.MapFrom(src => src.name));
  }
}
