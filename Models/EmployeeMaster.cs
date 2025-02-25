using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Employee_Crud_operation.Models
{
    public class EmployeeMaster
    {
        
        [Key]
        public int Row_Id { get; set; }//

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string PassportNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public int IsActive { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfJoinee { get; set; }
       
        [NotMapped]
        public IFormFile ProfileImageFile { get; set; }

        public string CountryName { get; set; } 
        public string StateName { get; set; } 
        public string CityName { get; set; }


        // public string CreatedDate { get; set; }
        //public string UpdatedDate { get; set; }
        //  public int IsDeleted { get; set; }
        //public string DeletedDate { get; set; }     
    }
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
    }
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
    }
    public class Dropdowns
    {
        public int Row_Id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Type { get; set; }
        //[Key]
        //public int Row_Id { get; set; }//
        //public int CountryId { get; set; }
        //public string CountryName { get; set; } // Optional
        //public int StateId { get; set; }
        //public string StateName { get; set; } // Optional
        //public int CityId { get; set; }
        //public string CityName { get; set; }

    }
}
