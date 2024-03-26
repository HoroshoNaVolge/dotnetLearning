namespace Factories.WebApi.BLL.Authentification
{
    public class UpdatePasswordModel
    {
        public required string Login { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }

    public class LoginModel
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }

}
