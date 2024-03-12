using System.ComponentModel.DataAnnotations;

namespace TaskToRepoteqCompany.PL.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }

       // [EmailAddress(ErrorMessage = "Invalid Email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must contain @ symbol")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
