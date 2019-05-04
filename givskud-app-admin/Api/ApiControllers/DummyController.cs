using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;

using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core.Models;

using Newtonsoft.Json;
using givskud_app_admin.ApiModels;
using givskud_app_admin.ApiServices;

namespace givskud_app_admin.ApiControllers
{
    public class DummyController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<string> Output = new List<string>();

                var Source = Umbraco.Content(0001).Children().Where("Visible");

                if (Source.Count() > 0)
                {
                    foreach (var Node in Source)
                    {

                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
    }
}