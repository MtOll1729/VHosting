using System.ComponentModel.DataAnnotations;

namespace VHosting.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nickname")]
        [DataType(DataType.Text)]
        public string Nickname { set; get; }

        [Required]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords are different")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { set; get; }

    }
}
