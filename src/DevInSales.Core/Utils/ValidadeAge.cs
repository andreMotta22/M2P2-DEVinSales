using System.ComponentModel.DataAnnotations;

namespace DevInSales.Core.Utils
{
    public class ValidadeAge:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime _idadeCliente = Convert.ToDateTime(value);
            if((DateTime.Now.Year - _idadeCliente.Year) > 18 ) 
                return ValidationResult.Success;
            else 
                return new ValidationResult("O usuario deve ter uma idade maior que 18 anos");    
        }
    }
}