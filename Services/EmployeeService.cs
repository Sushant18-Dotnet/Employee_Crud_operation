using Employee_Crud_operation.Data;
using Employee_Crud_operation.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace Employee_Crud_operation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyDbContext _context;

        public EmployeeService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Dropdowns>> GetDropdownData()
        {
            var response = new List<Dropdowns>();
            try
            {
                 response = await _context.Set<Dropdowns>()
                   .FromSqlRaw("EXEC stp_Emp_FetchAllDropdowndata")
                   .ToListAsync();

                return (response); 
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return response;
        }
        public async Task<List<EmployeeMaster>> GetAllEmployeesData()
        {
            
            var response = new List<EmployeeMaster>();
            try
            {
                string sSpName = "stp_Emp_FetchAllEmployeesData";
                response =
                await _context.EmployeeMaster
                .FromSqlRaw(sSpName)
                .ToListAsync();
              
                return (response); 
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return response;
        }

        public async Task<EmployeeMaster> GetEmployeeDataById(int id)
        {
            var response = new EmployeeMaster();

            try
            {
                var sql = "select Row_Id,ProfileImage, FirstName,LastName,CountryName,StateName,CityName,EmailAddress,MobileNumber,PanNumber,PassportNumber,Gender,1 as IsActive,CONVERT(VARCHAR, DateOfBirth, 103) DateOfBirth ,\r\nCONVERT(VARCHAR, DateOfJoinee, 103) DateOfJoinee ,c.CityId,cn.CountryId,s.StateId \r\nfrom EmployeeMaster e\r\nleft join CityN c on c.CityId=e.CityId\r\nleft join StateN s on s.StateId=e.StateId\r\nleft join CountryN cn on  cn.CountryId=e.CountryId WHERE Row_Id = {0}";
                var employee =
                await _context.EmployeeMaster
                   .FromSqlRaw(sql, id).FirstOrDefaultAsync();
                //.ToListAsync();
                if (employee != null)
                {
                    if (!string.IsNullOrEmpty(employee.DateOfBirth))
                    {
                        employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .ToString("yyyy-MM-dd");
                    }

                    if (!string.IsNullOrEmpty(employee.DateOfJoinee))
                    {
                        employee.DateOfJoinee = DateTime.ParseExact(employee.DateOfJoinee, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .ToString("yyyy-MM-dd");
                    }

                }
                if (employee == null)
                {
                    return null;
                }
                //return View(employee);
               return (employee); 
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return response;
        }

        public async  Task InsetEmployeeData(EmployeeMaster param)
        {
           var response = new EmployeeMaster();

            try
            {
               
                var sql = "EXEC stp_Emp_InsertEmployeeData "+
                    "@FirstName, @LastName,@CountryId,@StateId,@CityId, @EmailAddress,@MobileNumber,@PanNumber,@passportNumber,@Profilepicture,@Gender,@IsActive ,@DateOfBirth ,@DateOfJoinee";

                var parameters = new[]
                {
                    new SqlParameter("@FirstName", param.FirstName),
                    new SqlParameter("@LastName", param.LastName),
                    new SqlParameter("@CountryId", param.CountryId),
                    new SqlParameter("@StateId", param.StateId),
                       new SqlParameter("@CityId", param.CityId),
                    new SqlParameter("@EmailAddress", param.EmailAddress),
                    new SqlParameter("@MobileNumber", param.MobileNumber),
                    new SqlParameter("@PanNumber", param.PanNumber),
                    new SqlParameter("@passportNumber", param.PassportNumber),
                    new SqlParameter("@Profilepicture", param.ProfileImage),
                     new SqlParameter("@Gender", param.Gender),
                    new SqlParameter("@isActive", param.IsActive),
                         new SqlParameter("@DateOfBirth", param.DateOfBirth),
                    new SqlParameter("@DateOfJoinee", param.DateOfJoinee)
              
                };
                 _context.Database.ExecuteSqlRaw(sql, parameters);
              
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return;
            //return View(Json(new { success = true, message = "Employee saved successfully!" }));

        }

        public async Task UpdateEmployeeData(EmployeeMaster param )
        {
            
            //if (ModelState.IsValid)
            //{
            // Prepare the SQL query to call the stored procedure
            var sql = "EXEC stp_Emp_UpdateEmployeeData " +
                    " @Row_Id, @FirstName,@LastName ," +
                    "@CountryId,@StateId,@CityId," +
                    "@MobileNumber, @EmailAddress,@PanNumber," +
                    "@passportNumber,@ProfileImage ,@IsActive,@DateOfBirth,@DateOfJoinee ";

            // Execute the stored procedure using ExecuteSqlRaw
            var parameters = new[]
            {
               new SqlParameter("@Row_Id", param.Row_Id),
            new SqlParameter("@FirstName", param.FirstName),
            new SqlParameter("@LastName",param.LastName),
             new SqlParameter("@CountryId", param.CountryId),
            new SqlParameter("@StateId", param.StateId),
               new SqlParameter("@CityId", param.CityId),
           new SqlParameter("@MobileNumber", param.MobileNumber),
            new SqlParameter("@EmailAddress", param.EmailAddress),

            new SqlParameter("@PanNumber", param.PanNumber),
            new SqlParameter("@passportNumber", param.PassportNumber),
       //@Profilepicture,@Gender,@isActive ,@DateOfBirth ,@DateOfjoinee
            new SqlParameter("@ProfileImage", param.ProfileImage),
             //new SqlParameter("@Gender", param.Gender),
            new SqlParameter("@IsActive", param.IsActive),
              new SqlParameter("@DateOfBirth", param.DateOfBirth),
            new SqlParameter("@DateOfJoinee", param.DateOfJoinee)

        };

            _context.Database.ExecuteSqlRaw(sql, parameters);
            return;
        }

        public async Task DeleteEmployeedata(int id)
        {
            var sql = "exec stp_Emp_DeleteEmployeeData @Row_Id";//@Row_Id

            //var employee =
            //await _context.EmployeeMaster
            //   .FromSqlRaw(sql, id)
            //.ToListAsync();

            //ViewData["Employee"] = employee;

            var parameter = new[]
            {
               new SqlParameter("@Row_Id", id),
            };

            _context.Database.ExecuteSqlRaw(sql, parameter);
           
        }
    }
}
