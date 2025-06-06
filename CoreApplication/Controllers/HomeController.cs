using System.Diagnostics;
using System.Threading.Tasks;
using CoreApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace CoreApplication.Controllers
{
    public class HomeController : Controller
    {    
        public AppDbContext _Context;
        public IWebHostEnvironment _Environment;
        public HomeController(AppDbContext context, IWebHostEnvironment environment)
        {
            _Context = context;
            _Environment = environment;
        }
        public IActionResult Index()
        {
            var alllist=_Context.tbl_Gender.ToList();

            var all_data = new CombineViewModal
            {
                tbl_Gender = alllist,
            };

            return View(all_data);
        }

        [HttpPost]
        public async Task<IActionResult> Save_user(USERDATA data, IFormFile Image)
        {
            string folderPath = Path.Combine(_Environment.WebRootPath, "Uploaded");
            string filename = Image.FileName;
            string combinfolder = Path.Combine(folderPath, filename);
            var streafile = new FileStream(combinfolder, FileMode.Create);
            await Image.CopyToAsync(streafile);
            data.Image = filename;

           

            _Context.userdata.Add(data);
            await _Context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult List()
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var data = _Context.userdata.ToList();

            var Name = HttpContext.Session.GetString("Name");
            var Email = HttpContext.Session.GetString("Email");

            ViewBag.Name = Name;
            ViewBag.Email=Email;

            return View(data);
        }

        public IActionResult Delete(int Id)
        {
            var filename = _Context.userdata.FirstOrDefault(x=>x.Id==Id);

            string folderpath = Path.Combine(_Environment.WebRootPath, "Uploaded",filename.Image);

            if(folderpath != null)
            {
                if (System.IO.File.Exists(folderpath))
                {
                    System.IO.File.Delete(folderpath);
                }
            }

            var data = _Context.userdata.Find(Id);
            _Context.userdata.Remove(data);
            _Context.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Edit(int Id)
        {
            var data = _Context.userdata.Find(Id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(USERDATA model, IFormFile Image)
        {
            var data = await _Context.userdata.FindAsync(model.Id);
            data.Name = model.Name;
            data.Email = model.Email;
            data.Password = model.Password;
            data.Address = model.Address;

            if (Image != null)
            {
                string folderPath = Path.Combine(_Environment.WebRootPath, "Uploaded");
                string fileName = Path.GetFileName(Image.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                string oldImage = Path.Combine(folderPath, data.Image);

                if(System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                data.Image = fileName;
            }

            await _Context.SaveChangesAsync();

            return RedirectToAction("List");
        }


        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email,string Password)
         {
            try
            {
                var data = _Context.userdata.First(x => x.Email == Email && x.Password == Password);
                if (data != null)
                {
                    HttpContext.Session.SetInt32("Id", data.Id);
                    HttpContext.Session.SetString("Email", data.Email);
                    HttpContext.Session.SetString("Name", data.Name);
                    HttpContext.Session.SetString("Password", data.Password);
                    HttpContext.Session.SetString("Address", data.Address);




                    return RedirectToAction("List");
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

    }
}
