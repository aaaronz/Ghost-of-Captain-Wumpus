using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    class Player
    {
        private int mapLocation;
        private bool alive;
        private int numArrows;
        private int numCoins;
        private int numTurns;

        public Player()
        {
            mapLocation = 1;
            alive = true;
            numArrows = 3;
            numCoins = 0;
            numTurns = 0;    
        }
        public int getMapLocation()
        {
            return mapLocation;
        }
        public void setMapLocation(int cave)
        {
            mapLocation = cave;
        }
        public int getHealth(int health)
        {
            return health;
        }
        public bool isAlive()
        {
            return alive;
        }
        public void addArrows(int numNewArrows)
        {
            numArrows += numNewArrows;
        }
        /// <summary>
        /// Returns number of arrows in inventory.
        /// </summary>
        /// <returns></returns>
        public int getNumArrows()
        {
            return numArrows;
        }
        public bool arrowShot()
        {
            bool shot = true;
            numArrows--;
            if (numArrows < 0)
            {
                numArrows = 0;
                shot = false;
            }
            return shot;
        }
        /// <summary>
        /// Adds coins to player inventory.
        /// </summary>
        /// <param name="coins"></param>
        public void addCoins(int coins)
        {
            numCoins += coins;
        }
        public int getNumCoins()
        {
            return numCoins;
        }
        public int getNumTurns()
        {
            return numTurns;
        }
        public void newTurn()
        {
            numTurns++;
        }

        public int calculateScore()
        {
            int score = 100 - numTurns + numCoins + 10 * numArrows;
            return score;
        }
    }
}