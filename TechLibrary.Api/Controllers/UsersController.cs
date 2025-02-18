﻿using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Create(RequestUserJson request)
    {
        try
        {
            var userCase = new RegisterUserUseCase();

            var response = userCase.Execute(request);

            return Created(string.Empty, response);
        }
        catch (TechLibraryException ex)
        {
            return BadRequest(new ResponseErrorMessagesJson
            {
                Errors = ex.GetErrorMessages()
            });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorMessagesJson
            {
                Errors = ["Erro Desconhecido."]
            });
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Created();
    }
}
