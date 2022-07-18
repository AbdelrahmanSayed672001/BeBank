using BeBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeBank.Controllers.UserControl
{
    public interface IOTP
    {
        public IActionResult addOTP(OTP oTP);
        public int randomOTP();
    }
}
