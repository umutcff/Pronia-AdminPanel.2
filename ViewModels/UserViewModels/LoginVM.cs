using System.ComponentModel.DataAnnotations;

namespace ProniaUmut.ViewModels.UserViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(64), MinLength(3), EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(3), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
