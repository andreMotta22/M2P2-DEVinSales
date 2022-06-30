using System;

namespace DevInSales.Core.DTOs
{
    public class UserCadastroResponse
    {
        public List<string> Erros { get; set; }
        public bool Sucess { get; set; }

        public UserCadastroResponse(bool sucess = true)
        {
            Sucess = sucess;   
            Erros = new List<string>();
        }
        public void AdicionarErros(IEnumerable<string> erros) => Erros.AddRange(erros);
    }
}