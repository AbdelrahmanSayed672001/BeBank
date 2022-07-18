using BeBank.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using BeBank.Controllers.UserControl;

namespace BeBank.Controllers
{

    public class HomeController : Controller, IUserAuth, IUserData, IOTP
    {
        private Context db;

        public HomeController(Context context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Contact(int? id)
        {
            GetUserDataById(id);
            return View();
        }

        public IActionResult ShowInfo(int? id)
        {
            GetUserDataById(id);
            return View();
        }
        public IActionResult EditInfo(int? id)
        {
            GetUserDataById(id);
            return View();
        }

        public IActionResult Withdraw(int? id)
        {
            GetUserDataById(id);
            return View();
        }

        public IActionResult Deposit(int? id)
        {
            GetUserDataById(id);
            return View();
        }






        public IActionResult HomePage(int? id)
        {
            GetUserDataById(id);

            return View();
        }
        public IActionResult VerifyCode(int? id)
        {
            TempData["UserId"] = id;
            GetUserDataById(id);
            var AcountSID = "AC0ce23373c9b1eb412360af04042a826e";
            var AuthToken = "994d9f2a395a3bb182695e07fb9b471b";
            TwilioClient.Init(AcountSID, AuthToken);

            var Sender = new PhoneNumber("+19783916598");
            var recieverNumber = Convert.ToString(TempData["UserPhone"]);
            var reciever = new PhoneNumber(recieverNumber);
            TempData["RandomOTP"] = randomOTP();

            var message = MessageResource.Create(
                    to: reciever,
                    from: Sender,
                    body: "Your verification code is: " + randomOTP() + " for Be Bank"
                );
            return View();
        }





        [HttpPost]
        public IActionResult UserRegister(User model)
        {
            if (CheckUserRegister(model))
            {
                AddUser(model);
                return RedirectToAction("VerifyCode", new { id = model.Id });
            }
            else
            {
                TempData["RegisterError"] = model.Email + " is exits before";
                return RedirectToAction("SignUp");
            }

        }
        public bool CheckUserRegister(User model)
        {
            if (!db.users.Any(x => x.Email.Equals(model.Email)))
            {
                return true;
            }
            else
                return false;
        }
        public void AddUser(User model)
        {
            model.Balance = 0;
            model.WithDrawn = 0;
            model.Deposited = 0;
            db.users.Add(model);
            db.SaveChanges();
        }


        [HttpPost]
        public IActionResult UserLogin(User model)
        {
            if (CheckUserEmail(model))
            {
                if (CheckUserPassword(model))
                {
                    GetUserData(model);
                    return RedirectToAction("HomePage", "Home", new { id = TempData["UserId"] });

                }
                else
                {
                    TempData["PasswordError"] = "Your Password is incorrect";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["EmailError"] = "Your Email is incorrect or not found";
                return RedirectToAction("Index", "Home");
            }


        }

        public bool CheckUserEmail(User model)
        {
            if (db.users.Any(x => x.Email.Equals(model.Email)))
            {
                return true;
            }
            else
                return false;
        }

        public bool CheckUserPassword(User model)
        {
            if (db.users.Any(x => x.Password.Equals(model.Password)))
            {
                return true;
            }
            else
                return false;
        }

        public void GetUserDataById(int? id)
        {
            if (id != null)
            {
                foreach (var item in db.users)
                {
                    if (item.Id == id)
                    {
                        item.Id = (int)id;
                        TempData["UserId"] = item.Id;
                        TempData["UserUsername"] = item.Name;
                        TempData["UserEmail"] = item.Email;
                        TempData["UserPassword"] = item.Password;
                        TempData["UserPhone"] = "+2" + item.Phone;
                        TempData["UserPhoneNO"] = item.Phone;
                        TempData["UserAdress"] = item.Adress;
                        TempData["UserBalance"] = item.Balance;
                        TempData["UserWithdraw"] = item.WithDrawn;
                        TempData["UserDeposited"] = item.Deposited;
                        
                    }

                }
            }
        }

        public void GetUserData(User model)
        {
            int currentId;
            foreach (var item in db.users)
            {
                if (item.Email.Equals(model.Email))
                {
                    currentId = item.Id;
                    TempData["UserId"] = item.Id;
                    TempData["UserUsername"] = item.Name;
                    TempData["UserEmail"] = item.Email;
                    TempData["UserPassword"] = item.Password;
                    TempData["UserPhone"] = "+2" + item.Phone;
                    TempData["UserPhoneNO"] = item.Phone;
                    TempData["UserAdress"] = item.Adress;
                    TempData["UserBalance"] = item.Balance;
                    TempData["UserWithdraw"] = item.WithDrawn;
                    TempData["UserDeposited"] = item.Deposited;
                }
            }
        }

        public IActionResult addOTP(OTP oTP)
        {
            var rand = Convert.ToString(TempData["RandomOTP"]);
            oTP.UserId = Convert.ToInt32(TempData["UserId"]);

            if (oTP.VerifyCode != rand)
            {
                db.oTPs.Add(oTP);
                db.SaveChanges();
                TempData["WelcomeMSG"] = "Welcome to Be Bank, " + TempData["UserUsername"];
                return RedirectToAction("HomePage", "Home", new { id = TempData["UserId"] });

            }

            else
            {
                TempData["OTPError"] = "Your verification code is incorrect";
                return RedirectToAction("VerifyCode", "Home", new { id = oTP.UserId });

            }
        }

        public int randomOTP()
        {
            var randomNumber = new Random().Next(10000, 99999);
            return randomNumber;
        }

        public IActionResult SaveContact(Contact contact)
        {
            db.contacts.Add(contact);
            db.SaveChanges();

            return RedirectToAction("HomePage", "Home", new { id = TempData["UserId"] });
        }

        public IActionResult UpdateUserData(User user)
        {

            user.Id = Convert.ToInt32(TempData["UserId"]);
            user.Email = Convert.ToString(TempData["UserEmail"]);
            user.Balance = Convert.ToInt32(TempData["UserBalance"]);
            user.Deposited = Convert.ToInt32(TempData["UserDeposited"]);
            user.WithDrawn = Convert.ToInt32(TempData["UserWithdraw"]);

            if (user != null)
            {

                db.users.Update(user);
                db.SaveChanges();
                TempData["userId"] = user.Id;
                return RedirectToAction("ShowInfo", "Home", new { id = TempData["userId"] });
            }
            return RedirectToAction("EditInfo", new { id = TempData["UserId"] });

        }

        public IActionResult WithdrawMoney(User user)
        {

            user.Id = Convert.ToInt32(TempData["UserId"]);
            user.Adress = Convert.ToString(TempData["UserAdress"]);
            user.Email = Convert.ToString(TempData["UserEmail"]);
            user.Name = Convert.ToString(TempData["UserUsername"]);
            user.Password = Convert.ToString(TempData["UserPassword"]);
            user.Phone = Convert.ToString(TempData["UserPhoneNO"]);
            user.Balance = Convert.ToInt32(TempData["UserBalance"]);
            user.Deposited = Convert.ToInt32(TempData["UserDeposited"]);
            
            
            

            if (user.Balance == 0 )
            {
                TempData["WithdrawError"] = "Your balance is 0, you can not withdraw money";
                user.Balance = 0;
                
            }
            else
            {
                TempData["WithdrawSuccess"] = "you have withdrawn " + user.WithDrawn;
                user.Balance = user.Balance - user.WithDrawn;
           
            }
            db.users.Update(user);
            db.SaveChanges();
            TempData["UserBalance"] = user.Balance;
            TempData["UserId"]=user.Id;
            return RedirectToAction("Withdraw", new { id = TempData["UserId"] });
        }

        public IActionResult DepositMoney(User user)
        {
            GetUserData(user);
            user.Id = Convert.ToInt32(TempData["UserId"]);
            user.Adress=Convert.ToString(TempData["UserAdress"]);
            user.Email = Convert.ToString(TempData["UserEmail"]);
            user.Name = Convert.ToString(TempData["UserUsername"]);
            user.Password= Convert.ToString(TempData["UserPassword"]);
            user.Phone= Convert.ToString(TempData["UserPhoneNO"] );
            user.Balance = Convert.ToInt32(TempData["UserBalance"]);
            user.WithDrawn = Convert.ToInt32(TempData["UserWithdraw"]);

            user.Balance = user.Balance + user.Deposited;
            db.users.Update(user);
            db.SaveChanges();
            TempData["UserBalance"]=user.Balance;
            TempData["UserId"]=user.Id;
            
            TempData["DepositSuccess"] = "you have deposited " + user.Deposited;
            
            return RedirectToAction("Deposit", new { id = TempData["UserId"] });
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}