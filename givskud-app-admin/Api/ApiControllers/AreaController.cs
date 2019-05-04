using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

using Umbraco.Web;
using Umbraco.Web.WebApi;

using Newtonsoft.Json;
using givskud_app_admin.ApiModels;
using givskud_app_admin.ApiServices;

namespace givskud_app_admin.ApiControllers
{
    public class AreaController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<Area> Output = new List<Area>();

                var Source = Umbraco.Content(1122).Children().Where("Visible");

                if(Source.Count() > 0)
                {
                    foreach(var Node in Source)
                    {
                        Output.Add(new Area {
                            ID = Node.Id,
                            Title = Node.GetPropertyValue("title")
                        });
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
    }
}