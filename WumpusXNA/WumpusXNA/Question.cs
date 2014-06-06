using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WumpusXNA
{
    public class Question
    {
        private String question;
        private String[] answers;
        private bool[] usedAnswers;

        public Question(String q, String correctAnswer, String a1, String a2, String a3)
        {
            question = q;
            answers = new String[4] {correctAnswer, a1, a2, a3};
            usedAnswers = new bool[4] { true, true, true, true };
        }
        public String getQuestion()
        {
            return question;
        }
        public String getRandomAnswer()
        {
            Random gen = new Random();
            while (true)
            {
                int num = gen.Next(4);
                if (usedAnswers[num])
                {
                    usedAnswers[num] = false;
                    return answers[num];
                }
            }
        }
        public String getCorrectAnswer()
        {
            return answers[0];
        }
    }
}
