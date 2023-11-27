using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
       

        [HttpGet]
        public IActionResult GetAllTeachers()
        {
            var teacherNames = new string[] { "Aditi", "Reshma", "Srishti" };

            return Ok(teacherNames);
        }



    }
}
