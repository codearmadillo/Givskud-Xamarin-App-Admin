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
    public class NewsController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<Post> Output = new List<Post>();

                var Source = Umbraco.Content(1125).Children().Where("Visible");

                if (Source.Count() > 0)
                {
                    foreach (var Node in Source)
                    {
                        Output.Add(new Post
                        {
                            ID = Node.Id,
                            Title = (string)Node.GetPropertyValue("title"),
                            Body = (string)Node.GetPropertyValue("body"),
                            PublishedOn = Node.CreateDate,
                            PublishedBy = (string)Node.CreatorName,
                            Image = "https://www.jirikralovec.cz/dev/givskud/images/demo-animalimage.jpg"
                            // Image = "https://" + HttpContext.Current.Request.Url.Host + Node.GetPropertyValue("image").Url,
                        });
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
    }
}