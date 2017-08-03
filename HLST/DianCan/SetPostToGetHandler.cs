using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DianCan
{
    public class SetPostToGetHandler:System.Net.Http.DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)

        {
            try
            {
                //如果不是POST请求，就不处理

                if (request.Method != HttpMethod.Post)

                    return base.SendAsync(request, cancellationToken);

                //必须显示的指定 ContentType是application/x-www-form-urlencoded,如果是application/json则不处理

                if (request.Content.Headers.ContentType == null || string.IsNullOrWhiteSpace(request.Content.Headers.ContentType.MediaType) || !request.Content.Headers.ContentType.MediaType.Contains("application/x-www-form-urlencoded"))

                    return base.SendAsync(request, cancellationToken);

                //获取POST请求过来的参数

                var formStr = request.Content.ReadAsFormDataAsync().Result.ToString();



                if (!string.IsNullOrWhiteSpace(formStr))

                {

                    var url = string.IsNullOrWhiteSpace(request.RequestUri.Query) ? string.Format("{0}?{1}", request.RequestUri.AbsoluteUri, formStr) : string.Format("{0}&{1}", request.RequestUri.AbsoluteUri, formStr);

                    //给request设置新的RequestUri对象

                    request.RequestUri = new Uri(url);

                }

                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                BasicLibHelper.LogHelper.AddError(ex, new System.Diagnostics.StackTrace());
                return null;
            }
        }
    }
}
