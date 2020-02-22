using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public enum GenderEnum
    {
        [Display(Name ="Male")]
        M, 
        [Display(Name = "Female")]
        F,
        [Display(Name = "Other")]
        O
    }
    public class User
    {
        public int Id { get; set; }
        [Required, StringLength(20)]
        public string FirstName { get; set; }

        [Required, StringLength(80)]
        public string LastName { get; set; }//        [Required, EmailAddress, RegularExpression(@"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessage = "Please enter valid email address (e.g: example@example.com)")]
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum Gender { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
