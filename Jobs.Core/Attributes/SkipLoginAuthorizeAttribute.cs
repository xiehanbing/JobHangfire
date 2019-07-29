using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Jobs.Core.Attributes
{
    /// <summary>
    /// 跳过属性检查
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SkipLoginAuthorizeAttribute : Attribute, IFilterMetadata
    {

    }
}