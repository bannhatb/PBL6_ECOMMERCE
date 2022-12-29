using System.ComponentModel.DataAnnotations;

namespace Website_Ecommerce.API.ModelDtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        [MaxLength(256)]
        public string VerifyPassword { get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        public bool Gender { get; set; }

    }

    public class ResetPasswordDto
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
        [Required]
        [MaxLength(256)]
        public string PasswordOld { get; set; }
        [Required]
        [MaxLength(256)]
        public string PasswordNew { get; set; }
        [Required]
        [MaxLength(256)]
        public string RePassword { get; set; }
    }

    public class ForgetPasswordDto
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
    }


    public class ProfileDto
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }
        public string UrlAvatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}