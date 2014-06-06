using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    public class Score : IComparable
    {
        private int score;
        private String name;
        private int turns;
        private int coins;
        private int arrows;

        public Score()
        {
            score = 0;
            name = "No Name";
            turns = 0;
            coins = 0;
            arrows = 0;
        }

        public Score(int _score, String _name, int _turns, int _coins, int _arrows)
        {
            score = _score;
            name = _name;
            turns = _turns;
            coins = _coins;
            arrows = _arrows;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Score otherScore = obj as Score;
            if (otherScore != null)
                return this.score.CompareTo(otherScore.score);
            else
                throw new ArgumentException("Object is not a Score");
        }

        public int getScore()
        {
            return score;
        }
        public String getName()
        {
            return name;
        }
        public int getTurns()
        {
            return turns;
        }
        public int getCoins()
        {
            return coins;
        }
        public int getArrows()
        {
            return arrows;
        }
    }
}
