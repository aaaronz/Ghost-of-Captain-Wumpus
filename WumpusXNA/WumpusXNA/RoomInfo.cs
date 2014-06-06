using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    public class RoomInfo
    {
        private int number;
        private int coins;
        private bool bats;
        private bool armory;
        private bool wumpus;
        private int side;
        private bool[] doors;
        private Cave c;
        public RoomInfo(Cave cave, int roomNumber, int numCoins, bool containsBats, bool containsArmory, bool containsWumpus, int startingSide, bool[] existingDoors)
        {
            c = cave;
            number = roomNumber;
            coins = numCoins;
            bats = containsBats;
            armory = containsArmory;
            wumpus = containsWumpus;
            side = startingSide;
            doors = existingDoors;
        }
        public Cave getCave() { return c; }
        public int roomNumber() { return number; }
        public int numCoins() { return coins; }
        public bool containsBats() { return bats; }
        public bool containsWumpus() { return wumpus; }
        public int getSide() { return side; }
        public bool[] getDoors() { return doors; }
    }
}
