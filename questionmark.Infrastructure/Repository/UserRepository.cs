using questionmark.Application.Users.Contracts;
using questionmark.Application.Users.DTO;
using questionmark.Domain.Data;
using Microsoft.EntityFrameworkCore;
using questionmark.Application.Users.Mappers;

namespace questionmark.Infrastructure.Repository
{
    public class UserRepository : IUser
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserRegistered(string auth)
        {
            byte[] bytes = Convert.FromHexString(auth);
            var result = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.auth == bytes);
            return result != null;
        }
        public async Task<UserCreateDtoResponse> RegisterUser(UserCreateDto user, CancellationToken cancellationToken)
        {
            var entity = user.ToEntity();
            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.ToCreateResponseDto();
        }
        public async Task<bool> IsLoginCorrect(string auth)
        {
            byte[] bytes = Convert.FromHexString(auth);
            var result = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.auth == bytes);
            return result != null;
        }
        public async Task<UserLoginDtoResponse> LoginUser(UserLoginDto user, CancellationToken cancellationToken)
        {
            byte[] bytes = Convert.FromHexString(user.auth);
            var result = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.auth == bytes);
            return result!.ToUserLoginDtoResponse();
        }
    }
}
