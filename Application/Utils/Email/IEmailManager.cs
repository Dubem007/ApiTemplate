
namespace Application.Utils.Email
{
    public interface IEmailManager
    {
        string GetResetPasswordEmailTemplate(string emailLink, string email);
    }
}
