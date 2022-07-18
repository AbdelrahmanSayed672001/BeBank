using BeBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeBank.Controllers.UserControl
{
    public interface IUserAuth
    {
        
        public IActionResult UserRegister(User model);
        public IActionResult UserLogin(User model);
        public Boolean CheckUserRegister(User model);
        public Boolean CheckUserEmail(User model);
        public Boolean CheckUserPassword(User model);
    }
}
