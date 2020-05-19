using System.ComponentModel.DataAnnotations;

namespace MessaginApp.API.Dtos
{
    public class UserForRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength=4 , ErrorMessage="girmiş olduğunuz şifre 4 ve 8 karakter arasında olmalı.")]
        public string Password { get; set; }


    }
}