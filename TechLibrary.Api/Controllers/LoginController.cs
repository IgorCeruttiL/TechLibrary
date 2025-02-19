using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Login.DoLogin;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
[ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
public class LoginController : ControllerBase
{
    [HttpPost]
    public IActionResult DoLogin(RequestLoginJson request)
    {
        var useCase = new DoLoginUseCase();

        var response = useCase.Execute(request);

        return Ok(response);
    }
}

