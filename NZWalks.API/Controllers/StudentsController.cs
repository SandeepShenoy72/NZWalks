using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var studentNames = new string[] { "Ram", "Suhas", "Ritik", "Roshan", "Rahul" };

            return Ok(studentNames);
        }
    }
}
