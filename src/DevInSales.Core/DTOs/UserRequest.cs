using System;
using System.ComponentModel.DataAnnotations;
using DevInSales.Core.Utils;

namespace DevInSales.Core.DTOs
{
    public class UserRequest
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [ValidadeAge]
        public DateTime BirthDate { get;  set; } 
        
        [EmailAddress(ErrorMessage = "Email Invalido")]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage ="O {0} deve ter no minimo{1} e deve ser menor que {2}")]
        public string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage = "As senhas devem ser iguais")]
        [Required]
        public string SamePassword { get; set; }
        
        public string? Role { get; set; }
    }
}