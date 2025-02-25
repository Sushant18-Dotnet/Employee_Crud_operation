using System.Diagnostics;
using Employee_Crud_operation.Data;
using Employee_Crud_operation.Models;
using Employee_Crud_operation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Crud_operation.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly MyDbContext _context;
        private readonly IEmployeeService _employeeService;

        public HomeController( MyDbContext Context, IEmployeeService employeeService)
        {
            _context = Context;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            //return View("InserEmployeeForm");
            return View("InserEmployeeForm");
        }
        public IActionResult Create()
        {
            return View("InserEmployeeForm"); // This will return the view with your form
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeData()
        {
            var employees = await _employeeService.GetAllEmployeesData();
            return Json(employees);

            //return View(employees); 

        }
        [HttpGet]
        public async Task<IActionResult> GetAllDropdownsData()
        {
            var employees = await _employeeService.GetDropdownData();
            //return Json(employees);
            var countries = employees.Where(d => d.Type == "Country").ToList().Distinct();
            var states = employees.Where(d => d.Type == "State").ToList().Distinct();
            var cities = employees.Where(d => d.Type == "City").ToList().Distinct();

            return Json(new { Countries = countries, States = states, Cities = cities });

        }
        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            await _employeeService.DeleteEmployeedata(id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployeeById(int id)
        {

            var employees = await _employeeService.GetEmployeeDataById(id);

            return View(employees);
        }

        [HttpPost]
        public IActionResult InsertEmployeeData(EmployeeMaster param) 
        {
            //if (ModelState.IsValid)
            //{
            if (param.ProfileImageFile != null)
            {
                //var fileName = Path.GetFileName(param.ProfileImageFile.FileName);
                //var filePath = Path.Combine("wwwroot/uploads", fileName);
                var originalFileName = Path.GetFileNameWithoutExtension(param.ProfileImageFile.FileName);
                var extension = Path.GetExtension(param.ProfileImageFile.FileName);
                var timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                var fileName = $"{originalFileName}_{timestamp}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                var filePathWithoutfilename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                bool fileExists =FileExistsInFolder(filePathWithoutfilename, fileName);

                if (!fileExists)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        param.ProfileImageFile.CopyTo(stream);
                    }
                }
                param.ProfileImage = $"/uploads/{fileName}";
            }
            var employees =  _employeeService.InsetEmployeeData(param);

            //return RedirectToAction("Index");
            //return  View(Json(new { success = true, message = "Employee saved successfully!"  }));
            return Json(new { success = true, message = "Employee saved successfully!" });
        }

        public static bool FileExistsInFolder(string folderPath, string fileNameToCheck)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == fileNameToCheck)
                {
                //    System.IO.File.Delete(file);
                    return true; 
                }
            }
            return false;
        }

        [HttpPost]
        public IActionResult UpdateEmployeeData(EmployeeMaster param, IFormFile ProfileImage)
        {
            if (ProfileImage != null)
            {
                //var fileName = Path.GetFileName(ProfileImage.FileName);
                //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                var originalFileName = Path.GetFileNameWithoutExtension(ProfileImage.FileName);
                var extension = Path.GetExtension(ProfileImage.FileName);
                var timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                var fileName = $"{originalFileName}_{timestamp}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                var filePathWithoutfilename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                var OldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", param.ProfileImage);

                if (System.IO.File.Exists(OldfilePath))
                {
                    // Delete the file
                    System.IO.File.Delete(OldfilePath);
                   
                }
                bool fileExists = FileExistsInFolder(filePathWithoutfilename, fileName);

                if (!fileExists)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(stream);
                    }
                }
                param.ProfileImage = "/uploads/" + fileName;
            }
            var employees = _employeeService.UpdateEmployeeData(param);

            return Json(new { success = true, message = "Employee updated successfully!" });

            // return View(param);
        }

        [HttpPost]
        public JsonResult CheckUniquePanAndPassport(string PanNumber, string PassportNumber ,string emailaddress , string mobilenumber)
        {
            bool isPanUnique = !_context.EmployeeMaster.Any(
                                     e => e.PanNumber == PanNumber);
            bool isPassportUnique = !_context.EmployeeMaster.Any(e => e.PassportNumber == PassportNumber);
            bool isEmailUnique = !_context.EmployeeMaster.Any(e => e.EmailAddress == emailaddress);
            bool isMobileUnique = !_context.EmployeeMaster.Any(e => e.MobileNumber == mobilenumber);

            if (isPanUnique && isPassportUnique && isEmailUnique && isMobileUnique)
            {
                return Json(new { isUnique = true });
            }
            else
            {
                string message = !isPanUnique && !isPassportUnique
                    ? "Both PAN and Passport numbers are already in use."
                    : !isPanUnique
                        ? "PANnumber is already used!!!!." : !isEmailUnique ? "Email is already used "
                        : !isMobileUnique ? "Mobile is already used!!"
                        : "Passportnumber is already used!!.";

                return Json(new { isUnique = false, message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
