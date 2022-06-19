using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get;  set; }   
    }
}