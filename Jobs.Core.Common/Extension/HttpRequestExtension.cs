using Microsoft.AspNetCore.Http;

namespace Jobs.Core.Common.Extension
{
    /// <summary>
    /// httprequest 扩展
    /// </summary>
    public static class HttpRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            bool result = false;
            var xreq = request.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = request.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            return result;
        }
    }
}