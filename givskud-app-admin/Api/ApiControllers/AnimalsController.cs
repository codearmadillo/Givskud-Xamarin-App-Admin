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
    public class AnimalsController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            
            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<Animal> Output = new List<Animal>();

                var Source = Umbraco.Content(1126).Children().Where("Visible");

                if (Source.Count() > 0)
                {

                    Int32.TryParse(HttpContext.Current.Request.QueryString["sortbyarea"], out int AreaId);

                    foreach (var Node in Source)
                    {
                        // Sort by area
                        IPublishedContent AnimalArea = Node.GetPropertyValue<IPublishedContent>("animalAreaId");
                        if(AreaId != 0 && AnimalArea.Id != AreaId)
                        {
                            continue;
                        }
                        // Get Animal quiz
                        int AnimalQuizId = -1;
                        var AnimalQuiz = Node.GetPropertyValue("animalQuizId");
                        if(AnimalQuiz != null)
                        {
                            IPublishedContent AnimalQuizNode = (IPublishedContent)AnimalQuiz;
                            AnimalQuizId = AnimalQuizNode.Id;
                        }

                        // Output object
                        Animal Out = new Animal
                        {
                            ID = Node.Id,
                            Name = (string)Node.GetPropertyValue("animalName"),

                            Content = Node.GetPropertyValue<string[]>("animalContent"),
                            
                            Height = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalHeight"), " - ", " cm"),
                            Length = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalLength"), " - ", " cm"),
                            Weight = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalWeight"), " - ", " kg"),

                            Descendants = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalDescendants"), " -  ", ""),
                            Lifetime = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalLifetime"), " - ", ""),

                            PregnancyTime = Node.GetPropertyValue<string>("animalPregnancyTime"), 
                            Continent = AnimalsControllerHelper.GroupedStringContent(Node.GetPropertyValue<string[]>("animalContinent"), ", ", ""), 
                            
                            /*
                            Icon = "https://" + HttpContext.Current.Request.Url.Host + Node.GetPropertyValue("animalIcon").Url,
                            Image = "https://" + HttpContext.Current.Request.Url.Host + Node.GetPropertyValue("animalImage").Url,
                            */
                            Icon    = "https://www.jirikralovec.cz/dev/givskud/images/demo-animalicon.png",
                            Image   = "https://www.jirikralovec.cz/dev/givskud/images/demo-animalimage.jpg",

                            Status = Node.GetPropertyValue<string>("animalStatus"),
                            Eats = Node.GetPropertyValue<string>("animalEats"),
                            Species = Node.GetPropertyValue("animalSpecies"),
                            
                            AreaID = AnimalArea.Id,
                            QuizID = AnimalQuizId
                        };
                        Output.Add(Out);
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
    }
    public class AnimalsControllerHelper
    {
        public static string GroupedStringContent(string[] input, string delimiter = " ", string after = "")
        {

            string Output = input.Length > 1 ? string.Empty : input[0];

            if(input.Length > 1)
            {
                for(int i = 0; i < input.Length; i++)
                {
                    Output += input[i];
                    if(i < input.Length - 1)
                    {
                        Output += delimiter;
                    }
                }
            }

            return Output + after;

        }
    }
}

/*

 */