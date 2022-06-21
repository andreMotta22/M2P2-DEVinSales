using System;
using System.Globalization;

namespace DevInSales.Core.DTOs
{
    public class UserResponse
    {
        public UserResponse(int id, string nome, string userName, string email, DateTime birthDate)
        {
            Id = id;
            Nome = nome;
            UserName = userName;
            Email = email;
            BirthDate = birthDate.ToString("d",new CultureInfo("pt-BR"));
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        
    }
}