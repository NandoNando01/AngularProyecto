using Api.DTO;
using Api.Utils;

namespace Api.Domain.IService
{
  public interface IAuthService
  {
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
  }
}