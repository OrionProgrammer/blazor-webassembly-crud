using System.ComponentModel.DataAnnotations;

namespace EventSystem.Model
{
    public class UserModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email is Invalid!")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "User";
    }
}
