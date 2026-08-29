namespace FinTrack.Application.DTOs.Authentication
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }


    }
}