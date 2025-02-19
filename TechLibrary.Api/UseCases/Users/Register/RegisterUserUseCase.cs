using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
 

    public ResponseRegisterUserJson Execute(RequestUserJson request)
    {
        var dbContext = new TechLibraryDbContext();
        Validate(request, dbContext);

        var cryptography = new BCryptAlgorithme();

        var entity = new User
        {
            Email = request.Email,
            Name = request.Name,
            Password = cryptography.HashPassword(request.Password)
        };

        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseRegisterUserJson
        {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity)
        };
    }

    private void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var existUserWithEmail = dbContext.Users.Any(e => e.Email.Equals(request.Email));

        if (existUserWithEmail == true)
            result.Errors.Add(new ValidationFailure("E-mail", "E-mail já registrado na plataforma"));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
