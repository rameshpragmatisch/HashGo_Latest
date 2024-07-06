#region using

using System.Collections.Generic;

#endregion

namespace DinePlan.Common.Model.Point
{
    public class FlyFeedbackGroup
    {
        public FlyFeedbackGroup()
        {
            Questions = new List<FlyFeedbackQuestion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<FlyFeedbackQuestion> Questions { get; set; }
    }

    public class FlyFeedbackQuestion
    {
        public string Question { get; set; }
        public int QuestionType { get; set; }
        public string Answers { get; set; }
        public string DefaultAnswer { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class FlyFeedback
    {
        public FlyFeedback()
        {
            Answers = new List<FlyFeedbackAnswer>();
        }

        public int Group { get; set; }
        public string TicketNo { get; set; }
        public List<FlyFeedbackAnswer> Answers { get; set; }
    }

    public class FlyFeedbackAnswer
    {
        public int Question { get; set; }
        public string Answer { get; set; }
    }
}