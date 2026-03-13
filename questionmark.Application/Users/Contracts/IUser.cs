using questionmark.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Contracts
{
    public interface IUser
    {
        Task<bool> IsUserRegistered(string auth);
        Task<UserCreateDtoResponse> RegisterUser(UserCreateDto user, CancellationToken cancellationToken);
        Task<bool> IsLoginCorrect(string auth);
        Task<UserLoginDtoResponse> LoginUser(UserLoginDto user, CancellationToken cancellationToken);
    }
}
