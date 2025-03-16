using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApiJwt.Model;
using TemplateApiJwt.ViewModel;

namespace TemplateApiJwt.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository ?? throw new System.ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
        {
            var filePath = Path.Combine("Storage", employeeView.Photo.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                employeeView.Photo.CopyTo(fileStream);
            }

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var employee = _employeeRepository.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/jpeg");
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber, int pageQuantity)
        {
            _logger.Log(LogLevel.Error, "Teste Log Error");

            throw new Exception("Teste Exception");

            var employees = _employeeRepository.GetAll(pageNumber, pageQuantity);

            _logger.LogInformation("Teste Log Information");

            return Ok(employees);
        }
    }
}
