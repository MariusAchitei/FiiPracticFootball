using FIIPracticFootball.Services.Models;

namespace FIIPracticFootball.Services
{
  public interface ICryptographyService
  {
    HashedPassword HashPasswordWithSaltGeneration(string password);
    HashedPassword HashPassword(string password, string salt);
  }
}