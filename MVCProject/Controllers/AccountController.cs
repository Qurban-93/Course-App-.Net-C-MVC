using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MVCProject.Models;
using MVCProject.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace JuanProject.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                return View(user);
            }
            return View();
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser user = new();

            user.Email = registerVM.Email;
            user.UserName = registerVM.UserName;
            user.FullName = registerVM.FullName;

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View(registerVM);
                }
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string? link = Url.Action(nameof(EmailConfirm), "Account",
                new { userId = user.Id, token },
                Request.Scheme, Request.Host.ToString());


            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("qurban231293@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Email Confirmation";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/Verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", user.FullName);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("VerifyEmail");
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<IActionResult> EmailConfirm(string token, string userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            AppUser? user = await _userManager.FindByIdAsync(userId);

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.AddToRoleAsync(user, "User");


            return RedirectToAction(nameof(SuccesfulReqistered));
        }
        public IActionResult SuccesfulReqistered()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {
            if (loginVM == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "UserName or Email or Password !");
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Sehvlik var !");
                    return View(loginVM);
                }
            }

            if (user.EmailConfirmed == true)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync
               (user, loginVM.Password, loginVM.RememberMe, true);

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account bloked !");
                    return View(loginVM);
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or Email or Password invalid!");
                    return View(loginVM);
                }


                await _signInManager.SignInAsync(user, true);


                if (ReturnUrl != null) return Redirect(ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("UserNameOrEmail", "Bu email Tesdiqlenmeyib !");
            return View(loginVM);
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotVM)
        {
            if (forgotVM == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Duzgun daxil edin !");
                return View(forgotVM);
            }

            AppUser user = await _userManager.FindByEmailAsync(forgotVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Bu Email ile User yoxdur!");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string? link = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, token },
                Request.Scheme, Request.Host.ToString());

            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("qurban231293@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Email Confirmation";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/Verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", user.FullName);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("VerifyEmail");

        }
        public async Task<IActionResult> ResetPassword(string UserId, string token)
        {

            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(UserId);

            bool check = await _userManager.VerifyUserTokenAsync(user,
                _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!check) return NotFound();

            ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
            {
                Token = token,
                userId = UserId
            };

            return View(resetPasswordVM);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (resetPasswordVM == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Duzgun daxil ed !");
                return View(resetPasswordVM);
            }
            AppUser user = await _userManager.FindByIdAsync(resetPasswordVM.userId);

            if (user == null) { return NotFound(); }

            if (await _userManager.CheckPasswordAsync(user, resetPasswordVM.Password))
            {
                ModelState.AddModelError("", "This Password is your old password");
                return View(resetPasswordVM);
            }

            await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            await _userManager.UpdateSecurityStampAsync(user);
            TempData["SuccesChange"] = "Password Changed Succesfuly !";

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRoles()

        {
            string[] roles = { "SuperAdmin", "Admin", "USer", "Moderator" };

            foreach (var item in roles)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item });
            }
            return Content("elave edildi");
        }


    }
}
