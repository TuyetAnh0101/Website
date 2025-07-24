using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Services;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;

namespace SportsStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        // ========== LOGIN ==========
        public ViewResult Login(string returnUrl = "/") =>
            View(new LoginModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập không tồn tại.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                    return Redirect("/Admin");

                if (roles.Contains("User"))
                    return Redirect(model.ReturnUrl ?? "/Account");

                // Nếu không có vai trò phù hợp, đăng xuất và báo lỗi
                await _signInManager.SignOutAsync();
                ModelState.AddModelError("", "Tài khoản không có vai trò hợp lệ.");
                return View(model);
            }

            ModelState.AddModelError("", "Mật khẩu không đúng.");
            return View(model);
        }
        // GET: Hiển thị xác nhận nếu cần (hoặc không dùng)
        [HttpGet]
        [Authorize]
        public IActionResult LogoutConfirm()
        {
            _logger.LogInformation("Người dùng truy cập trang xác nhận đăng xuất.");
            return View();
        }

        // POST: Thực hiện logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Người dùng yêu cầu đăng xuất.");

            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("Đăng xuất thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng xuất.");
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Name,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address,
                BirthDate = model.BirthDate,
                IsAdmin = model.IsAdmin,
                EmailConfirmed = false // Bắt buộc xác nhận qua email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var roleName = user.IsAdmin ? "Admin" : "User";
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new IdentityRole(roleName));

                await _userManager.AddToRoleAsync(user, roleName);

                // ✅ Gửi email xác nhận
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Xác nhận Email",
                    $"Vui lòng xác nhận tài khoản của bạn bằng cách bấm vào liên kết sau: <a href='{confirmationLink}'>Xác nhận Email</a>");

                return View("RegisterConfirmation"); // View báo người dùng kiểm tra email
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Không tìm thấy người dùng có ID: {userId}");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("ConfirmEmailSuccess");

            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            string subject = "Đặt lại mật khẩu";
            string message = $"Bạn nhận được email này vì bạn (hoặc ai đó) đã yêu cầu đặt lại mật khẩu cho tài khoản. " +
                            $"Vui lòng nhấn vào <a href='{resetUrl}'>đây</a> để đặt lại mật khẩu.<br/>" +
                            "Nếu bạn không yêu cầu, vui lòng bỏ qua email này.";

            await _emailSender.SendEmailAsync(model.Email, subject, message);

            return RedirectToAction("ForgotPasswordConfirmation");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordViewModel { Token = token, Email = email });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation() => View();

        // ========== PROFILE ==========
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var roles = await _userManager.GetRolesAsync(user);
            var model = new ProfileModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Roles = roles.ToList()
            };

            return View(model);
        }

        // ========== EDIT PROFILE ==========
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new EditProfileModel
            {
                FullName = user.FullName,
                Address = user.Address,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.BirthDate = model.BirthDate;
            user.PhoneNumber = model.PhoneNumber;

            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarFile.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }

                user.AvatarUrl = "/uploads/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Profile");

            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(model);
        }
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi xác thực: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Tìm user theo login provider
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user != null)
            {
                // ⚠️ Kiểm tra nếu email chưa được xác nhận thì không cho đăng nhập
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token }, Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Xác nhận Email",
                        $"Bạn cần xác nhận tài khoản bằng cách bấm vào liên kết sau: <a href='{confirmationLink}'>Xác nhận Email</a>");

                    return View("RegisterConfirmation"); // View thông báo kiểm tra email
                }

                // ✅ Email đã xác nhận → đăng nhập
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            // 🔽 Nếu chưa có user, tạo user mới từ thông tin Google/Facebook
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = false // ⚠️ bắt buộc xác nhận email sau khi tạo
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                await _userManager.AddLoginAsync(newUser, info);

                // ✅ Gửi email xác nhận
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = newUser.Id, token }, Request.Scheme);

                await _emailSender.SendEmailAsync(newUser.Email, "Xác nhận Email",
                    $"Vui lòng xác nhận tài khoản của bạn bằng cách bấm vào liên kết sau: <a href='{confirmationLink}'>Xác nhận Email</a>");

                return View("RegisterConfirmation");
            }

            // Nếu có lỗi khi tạo user
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Login");
        }
    }
}
