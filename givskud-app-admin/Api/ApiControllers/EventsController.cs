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
    public class EventsController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {

                List<Event> Output = new List<Event>();
                var DataSource = Umbraco.Content(1124).Children().Where("Visible");

                if(DataSource.Count() > 0)
                {
                    foreach(var Node in DataSource)
                    {
                        Event Event = new Event
                        {
                            ID = Node.Id,
                            Title = Node.GetPropertyValue("title"), 
                            Desc = Node.GetPropertyValue("description"),
                            IsBoundToDate = Node.GetPropertyValue("isDateOnly"),
                            EventDate = Node.GetPropertyValue("eventDate"),
                            EventTime = Node.GetPropertyValue("eventTime")
                        };
                        if(Event.IsBoundToDate)
                        {
                            if(DateTime.Now != Event.EventDate)
                            {
                                continue;
                            }
                        }
                        Output.Add(Event);
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));

            }

            return Response.Get();

        }
    }
}