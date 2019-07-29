using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobs.Core.Application;
using Jobs.Core.Attributes;
using Jobs.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jobs.Core.Controllers
{
    [SkipLoginAuthorize]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public LoginController(IConfiguration configuration, IUserService userService, IAuthService authService)
        {
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
        }
        /// <summary>
        /// login 登录
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(Jobs.Entity.User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult{Message = "请输入账号和密码" });
            }

            var data =await _userService.VerifyUser(user.Account, user.Password);
            if (data == null)
            {
                return Json(new AjaxResult { Message = "账号或密码错误" });
            }
            _authService.SignIn(user.Account);

            return Json(new AjaxResult()
            {
                Status = true
            });
        }
        public IActionResult Redirect()
        {       
            return Redirect(_configuration["WebRootUrl"] + "/hangfire");
        }
    }
}