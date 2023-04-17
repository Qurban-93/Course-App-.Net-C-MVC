using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MVCProject.Interfaces;
using MVCProject.Models;
using MVCProject.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MVCProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
    

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,
            ISendEmailService sendEmailService, IConfiguration config, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _sendEmailService = sendEmailService;
            _config = config;
            _logger = logger;
        }

        //Facebook External Identity Actions
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }
            else
            {
                // If the user does not have an account, ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                

                return View("ExternalLogin", new ExternalLoginViewModel { 
                    Email = email ,
                    Name = name,                             
                });
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
 


        //Google External Identity Actions
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Identity.External");

            if(result == null)
            {
                return RedirectToAction("Index","Home");
            }

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value 
            });
            return Json(claims);
        }

        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("login");
        }








        //Internal Registration Actions
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                return View(user);
            }
            return RedirectToAction("login");
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
                new { userId = user.Id, token , registerVM.RememmberMe},
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
            _sendEmailService.SendEmail("qurban231293@gmail.com", "olszimzdwkxyjwwz","smtp.gmail.com",email);
            //SmtpClient smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            //smtp.Send(email);
            //smtp.Disconnect(true);

            return RedirectToAction("VerifyEmail");
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<IActionResult> EmailConfirm(string token, string userId ,bool rememmberMe)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            AppUser? user = await _userManager.FindByIdAsync(userId);

            await _userManager.ConfirmEmailAsync(user, token);
            if (rememmberMe)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            if (!_roleManager.Roles.Any())
            {
                string[] roles = { "SuperAdmin", "User" };

                foreach (var item in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item });
                }
                await _userManager.AddToRoleAsync(user, "SuperAdmin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
         
            return RedirectToAction(nameof(SuccesfulReqistered));
        }
        public IActionResult SuccesfulReqistered()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if (loginVM == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Sehvlik var!");
                return View(); 
            }
               
            
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("UserNameOrEmail", "Sehvlik var !");
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
                    ModelState.AddModelError("UserNameOrEmail", "Username or Email or Password invalid!");
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
            _sendEmailService.SendEmail("qurban231293@gmail.com", "olszimzdwkxyjwwz", "smtp.gmail.com",email);
            //using SmtpClient smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            //smtp.Send(email);
            //smtp.Disconnect(true);

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
      


    }
}
