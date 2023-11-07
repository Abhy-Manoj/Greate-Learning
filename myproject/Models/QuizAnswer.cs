using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myproject.Models
{
    public class QuizAnswer
    {
        public int AnswerId { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int UserAnswer { get; set; }
    }
}
