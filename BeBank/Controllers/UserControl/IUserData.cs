using BeBank.Models;

namespace BeBank.Controllers.UserControl
{
    public interface IUserData
    {
        public void GetUserDataById(int? id);
        public void GetUserData(User model);
        public void AddUser(User model);
    }
}
