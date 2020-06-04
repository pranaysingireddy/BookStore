using BookStore.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Repos
{
    public interface IAuthRepo
    {
        Task<User> GetUserFromAccessToken(string accessToken);

        bool ValidateRefreshToken(User user, string refreshToken);

        RefreshToken GenerateRefreshToken();

        string GenerateAccessToken(int userId);
    }
}
