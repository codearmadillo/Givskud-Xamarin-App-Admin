using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace givskud_app_admin.ApiServices
{
    public class JsonResponse
    {

        public readonly bool RequiresAuth;
        public readonly bool IsAuthentic;
        private HttpResponseMessage Message;
        private readonly SecureContext SecureContext;

        public JsonResponse(bool Auth, SecureContext Context)
        {
            RequiresAuth = Auth;
            SecureContext = Context;
            IsAuthentic = Verify();
        }
        public void Create(List<string> ResponseStringified = null)
        {

            Message = new HttpResponseMessage();
            Message.StatusCode = (HttpStatusCode)SecureContext.Context.Response.StatusCode;

            if (SecureContext.Context.Response.StatusCode != 200 && RequiresAuth == true)
            {
                Message.Content = new StringContent(JsonConvert.SerializeObject(new List<string>() { "Unauthorized" }), ApiContext.GetEncoding(), ApiContext.GetOutputType());
            }
            else
            {
                Message.Content = null;
            }

        }
        public HttpResponseMessage Get()
        {
            return Message;
        }
        public void Set(StringContent Input)
        {
            Message.Content = Input;
        }
        private bool Verify()
        {
            SecureContext Context = new SecureContext();
            return Context.Context.Response.StatusCode == 200;
        }
    }
}