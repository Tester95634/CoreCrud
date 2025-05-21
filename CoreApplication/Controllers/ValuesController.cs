using CoreApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public AppDbContext _Context;

        public IWebHostEnvironment _Environment;

        public ValuesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _Context = context;
            _Environment = environment;
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromForm] USERDATA data, IFormFile Image)
        {
            string folderPath = Path.Combine(_Environment.WebRootPath, "Uploaded");
            string filename =Image.FileName;
            string combinfolder = Path.Combine(folderPath, filename);
            var streafile = new FileStream(combinfolder, FileMode.Create);
            await Image.CopyToAsync(streafile);

            var alldata = new USERDATA
            {
                Name = data.Name,
                Email = data.Email,
                Password = data.Password,
                Address = data.Address,
                Image=filename
            };

            _Context.userdata.Add(alldata);
            await _Context.SaveChangesAsync();
            return Ok(new {message="All inserted successfully",alldata});
        
        }


    }
}
