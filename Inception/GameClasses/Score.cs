using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses
{
    public class Score
    {
        private static readonly Score gameScore = new Score();

        static Score() { }

        private Score() { }

        public static Score getInstance()
        {
           return gameScore;
        }

        public int points = 0;

        public void AddPoint()
        {
            points++;
        }

        public void ResetPoints()
        {
            points = 0;
        }
    }
}
