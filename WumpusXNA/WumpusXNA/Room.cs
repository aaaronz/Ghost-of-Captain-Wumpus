using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    public class Room
    {
        private int index = 0;
        private List<int> caves = new List<int>();
        private int numCaves = 0;

        //constructor
        public Room(int i)
        {
            index = i;
        }

        //mutators
        public void addCave(int c)
        {
            caves.Add(c);
            numCaves++;
        }
        public void deleteCave(int c)
        {
            caves.Remove(c);
            numCaves--;
        }

        //accessors
        public Boolean canAddCave()
        {
            return numCaves < 4;
        }
        public Boolean canDeleteCave()
        {
            return numCaves > 1;
        }
        public List<int> getCaves()
        {
            return caves;
        }
        public int getNumCaves()
        {
            return numCaves;
        }
        public int getIndex()
        {
            return index;
        }
        public String toString()
        {
            String str = "";
            str += index + ": ";
            foreach(int a in caves)
            {
                str+= a + ", ";

            }
            str += "   ";
            return str;
        }
    }
}
