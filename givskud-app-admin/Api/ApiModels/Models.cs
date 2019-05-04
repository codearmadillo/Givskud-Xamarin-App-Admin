using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace givskud_app_admin.ApiModels
{
    public class Area
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
    public class Quiz
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool IsLockedByDefault { get; set; }
        public List<QuizQuestion> Questions { get; set; }
    }
    public class QuizQuestion
    {
        public string Question { get; set; }
        public Dictionary<int, string> Answers { get; set; }
        public int CorrectAnswer { get; set; }
    }
    public class SeasonPass
    {
        public string ID { get; set; }
        public string Holder { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string AcquiredOn { get; set; }
    }
    public class Animal
    {
        public int ID { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string[] Content { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string PregnancyTime { get; set; }
        public string Descendants { get; set; }
        public string Lifetime { get; set; }
        public string Continent { get; set; }
        public string Status { get; set; }
        public string Eats { get; set; }
        public string Species { get; set; }
        public int QuizID { get; set; }
        public int AreaID { get; set; }
    }
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public bool IsBoundToDate { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventTime { get; set; }
    }
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public DateTime PublishedOn { get; set; }
        public string PublishedBy { get; set; }
    }
}