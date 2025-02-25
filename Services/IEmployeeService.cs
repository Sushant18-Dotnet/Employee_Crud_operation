using Employee_Crud_operation.Models;

namespace Employee_Crud_operation.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeMaster>> GetAllEmployeesData();
        Task<List<Dropdowns>> GetDropdownData();
        Task<EmployeeMaster> GetEmployeeDataById(int id);
        Task InsetEmployeeData(EmployeeMaster employee);
        Task UpdateEmployeeData(EmployeeMaster employee );
        Task DeleteEmployeedata(int id);
    }
}
