using FiiPracticFootball.Repositories.Dtos;

namespace FiiPracticFootball.Repositories
{
  public interface IUserRepository
  {
    void CreateUser(UserDto userDto);
    IEnumerable<UserDto> SearchByName(string searchTerm);
    void DeleteUser(int userId);
    List<UserDto> GetAll();

    void UpdateUser(UserDto userDto);
    UserDto? GetUser(int userId);
    UserDto? GetUserByEmail(string email);
    }
}
