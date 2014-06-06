using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    class Map
    {
        private int[] hazardLocations;
        private int[] coinsInRoom;
        public Map()
        {
            
            coinsInRoom = new int[30];
            //hazards
            hazardLocations = new int[30];
            hazardLocations[0] = hazardLocations[1] = 2;
            hazardLocations[2] = hazardLocations[3] = hazardLocations[4] = hazardLocations[5] = 1;
            for (int i = 6; i < 30; i++)
            {
                hazardLocations[i] = 0;
            }
            //coins
            Random gen = new Random();
            for (int i = 2; i < 30; i++)
            {
                coinsInRoom[i] = 1;
            }
            for (int i = 0; i < 70; i++)
            {
                coinsInRoom[gen.Next(28) + 2]++;
            }
            shuffle();
        }
        private void shuffle()
        {
            Random gen = new Random();
            
            for (int i = 0; i < hazardLocations.Length; i++)
            {
                int randIndex = gen.Next(hazardLocations.Length - i);
                int hazardHolder = hazardLocations[randIndex];
                int coinHolder = coinsInRoom[randIndex];
                hazardLocations[randIndex] = hazardLocations[hazardLocations.Length - 1 - i];
                coinsInRoom[randIndex] = coinsInRoom[coinsInRoom.Length - 1 - i];
                hazardLocations[hazardLocations.Length - 1 - i] = hazardHolder;
                coinsInRoom[coinsInRoom.Length - 1 - i] = coinHolder;
            }
        }
        /// <summary>
        /// Returns a number representing the type of obstacles at the current location
        /// 0: no hazard, 1: pit, 2: bats
        /// </summary>
        /// <returns></returns>
        public int checkForHazards(int mapLocation)
        {
            return hazardLocations[mapLocation - 1];
        }
        public int checkForCoins(int mapLocation)
        {
            int coins = coinsInRoom[mapLocation - 1];
            return coins;
        }

        public void pickUpCoin(int mapLocation)
        {
            coinsInRoom[mapLocation - 1]--;
        }
    }
}
