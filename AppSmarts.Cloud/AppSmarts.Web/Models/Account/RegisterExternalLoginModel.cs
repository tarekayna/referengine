using System.ComponentModel.DataAnnotations;

namespace AppSmarts.Web.Models.Account
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }

        public string FullName { get; set; }
        public string Link { get; set; }
    }
}