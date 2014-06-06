using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WumpusXNA
{
    class HighScore
    {
	   
        public List<Score> HighScores = new List<Score>();

        public HighScore()
        {
            readHighScores();
        }
	
        // returns HighScores list to gamecontrol.
        public List<Score> getHighScores()
        {
            return HighScores;
        }

        // adds new score to the list HighScores.
        public void addNewScore(Score newScore)
        {
            HighScores.Add(newScore);
            HighScores.Sort();
        }

        // writes the HighScores list to a text file.
        private void writeHighScores(List<int> HighScores)
        {
            string path = @"..\..\HighScoresFile.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < HighScores.Count; i++)
                {
                    sw.WriteLine(HighScores[i]);
                }
                  
            }
        }

        // reads the HighScores from a text file and adds them in the list
	    private void readHighScores()
	    {
            HighScores.Clear();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\admin\Documents\Temp\WumpusXNA\WumpusXNA\HighScoresFile.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(' ');

                int scoreNum = int.Parse(tokens[0]);
                string name = tokens[1];
                int turns = int.Parse(tokens[2]);
                int coins = int.Parse(tokens[3]);
                int arrows = int.Parse(tokens[4]);
                HighScores.Add(new Score(scoreNum, name, turns, coins, arrows));
            }
            HighScores.Sort();
    	}

    }
}


