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
    public class QuizController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            JsonResponse Response = new JsonResponse(true, new SecureContext());
            Response.Create();

            if (Response.IsAuthentic == true)
            {
                List<Quiz> Output = new List<Quiz>();

                var Source = Umbraco.Content(1123).Children().Where("Visible");

                // Sort parameter
                Int32.TryParse(HttpContext.Current.Request.QueryString["id"], out int QuizId);

                if(Source.Count() > 0)
                {
                    foreach(var Node in Source)
                    {
                        if(QuizId != 0 && Node.Id != QuizId)
                        {
                            continue;
                        }
                        Quiz Quiz = new Quiz
                        {
                            ID = Node.Id,
                            Title = Node.GetPropertyValue("title"),
                            IsLockedByDefault = (bool)Node.GetPropertyValue("isLockedByDefault"),
                            Questions = new List<QuizQuestion>(),
                            Image = "https://www.jirikralovec.cz/dev/givskud/images/demo-animalimage.jpg"
                            // Image = "https://" + HttpContext.Current.Request.Url.Host + Node.GetPropertyValue("image").Url
                        };
                        // Individual questions
                        IEnumerable<IPublishedContent>Questions = Node.GetPropertyValue("questions");
                        foreach(IPublishedContent Q in Questions)
                        {
                            QuizQuestion QQ = new QuizQuestion
                            {
                                Question = (string)Q.GetPropertyValue("questionText"),
                                Answers = new Dictionary<int, string>(),
                                CorrectAnswer = (int)Q.GetPropertyValue("correctAnswer")
                            };
                            string[] Answers = (string[])Q.GetPropertyValue("answers");
                            if(Answers.Count() > 0)
                            {
                                for(int i = 0; i < Answers.Count(); i++)
                                {
                                    QQ.Answers.Add(i, Answers[i]);
                                }
                            }
                            Quiz.Questions.Add(QQ);
                        }
                        Output.Add(Quiz);
                    }
                }

                Response.Set(new StringContent(JsonConvert.SerializeObject(Output), ApiContext.GetEncoding(), ApiContext.GetOutputType()));
            }

            return Response.Get();

        }
    }
}