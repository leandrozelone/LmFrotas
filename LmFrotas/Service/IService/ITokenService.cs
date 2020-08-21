using LmFrotas.Models;

namespace LmFrotas.Service.IService
{
    public interface ITokenService
    {
        User GenerateToken(User user);
    }
}