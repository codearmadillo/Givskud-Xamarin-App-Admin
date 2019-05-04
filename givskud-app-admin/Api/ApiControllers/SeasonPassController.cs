using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;

using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

using Newtonsoft.Json;
using givskud_app_admin.ApiModels;
using givskud_app_admin.ApiServices;
using givskud_app_admin.EncryptionService;

namespace givskud_app_admin.ApiControllers
{
    public class SeasonPassController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            SecureContext SC = new SecureContext();
            string PassParameter = SC.GetHeaderParam("PassID");

            JsonResponse Response = new JsonResponse(true, SC);
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<SeasonPass> Output = new List<SeasonPass>();

                var Source = Umbraco.Content(1121).Children().Where("Visible");

                if (Source.Count() > 0)
                {
                    foreach (var Node in Source)
                    {
                        if((string)Node.GetPropertyValue("passID") != PassParameter)
                        {
                            continue;
                        } else
                        {
                            Output.Add(new SeasonPass
                            {
                                ID = "n/a",
                                Holder = (string)Node.GetPropertyValue("holderName"),
                                ValidFrom = Convert.ToDateTime(Node.GetPropertyValue("validFrom")).ToString("dd-MM-yyyy"),
                                ValidTo = Convert.ToDateTime(Node.GetPropertyValue("validTo")).ToString("dd-MM-yyyy"),
                                AcquiredOn = Convert.ToDateTime(Node.GetPropertyValue("acquiredOn")).ToString("dd-MM-yyyy")
                            });
                        }
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
        [HttpPost]
        public HttpResponseMessage Post([FromBody] SeasonPass Data)
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {

                IContentService Service = Services.ContentService;

                var Pass = Service.CreateContent(EncDecService.Hash(Data.ID), 1121, "seasonPass");
                    Pass.SetValue("passID", EncDecService.Hash(Data.ID));
                    Pass.SetValue("holderName", EncDecService.Encrypt(Data.Holder));
                    Pass.SetValue("validFrom", Convert.ToDateTime(Data.ValidFrom));
                    Pass.SetValue("validTo", Convert.ToDateTime(Data.ValidTo));
                    Pass.SetValue("acquiredOn", Convert.ToDateTime(Data.AcquiredOn));

                string ResponseMessage;

                if(Service.SaveAndPublishWithStatus(Pass))
                {
                    ResponseMessage = "The content has been successfully saved.";
                } else
                {
                    ResponseMessage = "An error occured while saving the season pass.";
                }

                Response.Set(new StringContent(ResponseMessage, ApiContext.GetEncoding(), "text/plain"));

            }

            return Response.Get();

        }
    }
}   