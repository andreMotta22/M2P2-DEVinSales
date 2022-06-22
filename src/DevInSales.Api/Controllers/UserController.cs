// using DevInSales.Api.Dtos;
using System.IdentityModel.Tokens.Jwt;
using DevInSales.Core.DTOs;
using DevInSales.Core.Entities;
// using DevInSales.EFCoreApi.Api.DTOs.Request;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using RegexExamples;

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
        [Authorize(Roles = "Admin,Usuario")]
        public ActionResult<List<UserResponse>> ObterUsers(string? nome, string? DataMin, string? DataMax)
        {

            var users = _userService.ObterUsers(nome, DataMin, DataMax);
            if (users == null || users.Count == 0)
                return NoContent();

            var listaDto = users.Select(user => new UserResponse(user.Id,user.Name,user.UserName,user.Email,user.BirthDate))
                                .ToList();

            return Ok(listaDto);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse?>> ObterUserPorId(int id)
        {
            var user = await _userService.ObterPorId(id);
            if (user == null)
                return NotFound();

            var UserDto = new UserResponse(user.Id,user.Name,user.UserName,user.Email,user.BirthDate); 
            return Ok(UserDto);
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
            return Unauthorized(result.Erro);    
        }

        // vou usar isso aqui depois
        [HttpGet("logado")] 
        public ActionResult<bool> Logado(){
            var email = User.Claims.FirstOrDefault(a => a.Type == "Email")?.Value;
            if(email != null) {
                return Ok(true);
            }
            return BadRequest(false);
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