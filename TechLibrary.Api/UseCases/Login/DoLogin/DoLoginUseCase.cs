using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public ResponseRegisterUserJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLibraryDbContext();

        var user = dbContext.Users.FirstOrDefault(e => e.Email.Equals(request.Email));

        if (user is null)
            throw new InvalidLoginException();

        var cryptography = new BCryptAlgorithme();
        var passwordIsValid = cryptography.Verify(request.Password, user);
        if(passwordIsValid == false)
            throw new InvalidLoginException();

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user)
        };
    }
}
