using System;
using System.Linq;
using Jobs.Core.Attributes;
using Jobs.Core.Common;
using Jobs.Core.Common.Extension;
using Jobs.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Jobs.Core.Filters
{
    /// <summary>
    /// 登录状态过滤器
    /// </summary>
    public class AdminAuthFilter :  IAuthorizationFilter
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authenticate = context.HttpContext.AuthenticateAsync(CookieJobsAuthInfo.AdminAuthCookieScheme);
            if (authenticate.Result.Succeeded || SkipUserAuthorize(context.ActionDescriptor))
            {
                return;
            }

            HttpRequest httpRequest = context.HttpContext.Request;
            if (httpRequest.IsAjaxRequest())
            {
                AjaxResult ajaxResul = new AjaxResult()
                {
                    Status = false,
                    ErorCode = 400001,
                    Message = "登录超时"
                };

                context.Result = new JsonResult(ajaxResul);
            }
            else
            {
                RedirectToRouteResult redirectResult = new RedirectToRouteResult("login");
                context.Result = redirectResult;
            }

            return;

        }
        /// <summary>
        /// 跳过检查
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected virtual bool SkipUserAuthorize(ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.FilterDescriptors.Any(a => a.Filter is SkipLoginAuthorizeAttribute);
        }



    }
}