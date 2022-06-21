using DevInSales.Api.Dtos;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;
using DevInSales.EFCoreApi.Api.DTOs.Request;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RegexExamples;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Busca uma lista de usuários.
        /// </summary>
        /// <remarks>
        /// Pesquisa opcional: nome,data minima, data máxima
        /// <para>
        /// Exemplo de resposta:
        /// [
        ///   {
        ///     "id": 1,
        ///     "email": "joao@hotmail.com",
        ///     "name": "João",
        ///     "birthDate": "01/01/2000"
        ///   }
        /// ]
        /// </para>
        /// </remarks>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Pesquisa realizada com sucesso porém não retornou nenhum resultado</response>

        [HttpGet]
        public ActionResult<List<User>> ObterUsers(string? nome, string? DataMin, string? DataMax)
        {

            // var users = _userService.ObterUsers(nome, DataMin, DataMax);
            // if (users == null || users.Count == 0)
            //     return NoContent();

            // var ListaDto = users.Select(user => UserResponse.ConverterParaEntidade(user)).ToList();

            return Ok(new List<User>());
        }


        /// <summary>
        /// Busca um usuário por id.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exemplo de resposta:
        /// [
        ///   {
        ///     "id": 1,
        ///     "email": "joao@hotmail.com",
        ///     "name": "João",
        ///     "birthDate": "01/01/2000"
        ///   }
        /// ]
        /// </para>
        /// </remarks>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="404">Not Found, estado não encontrado no stateId informado.</response>
        [HttpGet("{id}")]
        public ActionResult<User> ObterUserPorId(int id)
        {
            // var user = _userService.ObterPorId(1);
            string? user = null;
            if (user == null)
                return NotFound();

            // var UserDto = UserResponse.ConverterParaEntidade(user);

            return Ok();
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exemplo de resposta:
        /// [
        ///   {
        ///     "id": 1
        ///   }
        /// ]
        /// </para>
        /// </remarks>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Pesquisa realizada com sucesso porém não retornou nenhum resultado</response>
        /// <response code="400">Formato invalido</response>
        [HttpPost("cadastro")]
        public async Task<ActionResult<UserCadastroResponse>> CriarUser(UserRequest model)
        {
            var response = await _userService.CriarUser(model);
            // return CreatedAtAction(nameof(ObterUserPorId), new { id = id }, id);
            return Ok(response);
        }   

        [HttpPost("login") ]
        public async Task<ActionResult<UserLoginResponse>> LogarUser(UserLoginRequest user){
            var result = await _userService.LogarUser(user);
            if(result.Sucess)
                return Ok(result);
            return BadRequest(result.Erro);    
        }


        /// <summary>
        /// Deleta um usuário.
        /// </summary>
        /// <response code="204">Endereço deletado com sucesso</response>
        /// <response code="404">Not Found, endereço não encontrado.</response>
        /// <response code="500">Internal Server Error, erro interno do servidor.</response>

        [HttpDelete("{id}")]
        public ActionResult ExcluirUser(int id)
        {
            try
            {
                // _userService.RemoverUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("usuario não existe"))
                    return NotFound();

                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensagem = ex.Message });
            }
        }
    }
}