using System;
using System.Collections.Generic;
using System.IO;

namespace WumpusXNA
{
    public class Cave
    {
        //fields
        private static int NUMBER_OF_ROOMS = 30;
        private Room[] rooms = new Room[NUMBER_OF_ROOMS];
        //private TextReader caveFile = new StreamReader("CaveFile.txt");
        private int runs = 0;
        //constructors

        public Cave()
        {
            Random g = new Random();
            Boolean caveIsValid = false;
            while(caveIsValid == false)
            {
                formCave(g);
                caveIsValid = everyRoomHasCave();
                runs++;
            }
            string s = toString();
        }


        private void formCave(Random g)
        {
            //creates array of Rooms where each room has 0 caves
            for (int i = 0; i < NUMBER_OF_ROOMS; i++)
            {
                Room room = new Room(i+1);
                rooms[i] = room;
            }
            
            //Traverses the array of Rooms
            for (int i = 0; i < NUMBER_OF_ROOMS; i++)
            {

                    //randomly decides how many caves each room will have
                    int connections = g.Next(3) + 1;
                    int numValidCaves = getValidCaves(rooms[i]).Count;
                    if (numValidCaves == 0)
                    {
                        numValidCaves = 1;
                        
                    }

                    if (numValidCaves < connections)
                    {
                        connections = numValidCaves;
                    }         
                    //Find the number of caves that need to be added
                    int cavesAdded = connections - rooms[i].getNumCaves();
                    
                    //Number of caves added
                    int numConnections = 0;
                    while (numConnections < cavesAdded)
                    {
                          List<int> validCaves = getValidCaves(rooms[i]);
                            int r = validCaves[g.Next(validCaves.Count)];
                            
                            if (!rooms[i].getCaves().Contains(r) && rooms[r - 1].canAddCave())
                            {
                                rooms[i].addCave(r);
                                rooms[r - 1].addCave(i + 1);
                                numConnections++;
                            }  
                    }

                    
                }
            for (int i = 0; i < NUMBER_OF_ROOMS; i++)
            {
                while (rooms[i].getNumCaves() == 4)
                {
                    int caveToDelete = rooms[i].getCaves()[g.Next(4)];
                    if (rooms[caveToDelete - 1].canDeleteCave())
                    {
                        rooms[i].deleteCave(caveToDelete);
                        rooms[caveToDelete - 1].deleteCave(i + 1);
                    }
                }
            }
                
        }

        private static int getEvenFunction(int function, int i)
        {
            if (function == 0)
            {
                return i + 25;                
            }
            else if (function == 1)
            {
                return i + 2;
            }
            else if (function == 2)
            {
                return i + 8;
                
            }
            else if (function == 3)
            {
                return i + 7;
            }
            else if (function == 4)
            {
                return i + 6;
            }
            else if (function == 5)
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
        private static int getOddFunction(int function, int i)
        {
            if (function == 0)
            {
                return i + 25;
                
            }
            else if (function == 1)
            {
                return i + 26;                
            }
            else if (function == 2)
            {
                return i + 2;               
            }
            else if (function == 3)
            {
                return i + 7;
                
            }
            else if (function == 4)
            {
                return i;
            }
            else if (function == 5)
            {
                return i + 24;
            }
            else
            {
                return 0;
            }
        }
        private int getOddConnectingRow(int function, int i)
        {
            if (function == 0)
            {
                return i + 25;
                
            }
            else if (function == 1)
            {
                return i + 26;
                
            }
            else if (function == 2)
            {
                return i + 2;
                
            }
            else if (function == 3)
            {
                return i + 7;
               
            }
            else if (function == 4)
            {
                return i + 6;
                
            }
            else if (function == 5)
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
        private static int getEvenConnectingRow(int function, int i)
        {
            if (function == 0)
            {
                return i + 25;
            }
            else if (function == 1)
            {
                return i + 26;
            }
            else if (function == 2)
            {
                return i + 2;
            }
            else if (function == 3)
            {
                return i + 7;
            }
            else if (function == 4)
            {
                return i + 6;
            }
            else if (function == 5)
            {
                return i;
            }
            else
            {
                return 0;
            }
        }

        //accessors
        public List<int> getCavesPerRoom(int index)
        {
            return rooms[index - 1].getCaves();
        }

        public Room[] getAllRooms()
        {
            return rooms;
        }

        public bool checkForRoom(int index, int direction)
        {
            int i = index - 1;
            int cave = getPossibleCave(direction, i);            
            return (rooms[i].getCaves().Contains(cave));            
        }

        public bool[] getLinkedRooms(int index)
        {
            int j = index - 1;
            Boolean[] linkedRoom = new Boolean[6];
            for (int i = 0; i < linkedRoom.Length; i++)
            {
                linkedRoom[i] = rooms[j].getCaves().Contains(getPossibleCave(i, j));
            }

            return linkedRoom;
        }
        public int[] getAdjacentRooms(int index)
        {
            int[] adjacentRooms = new int[6];
            int i = index - 1;
            for (int j = 0; j < adjacentRooms.Length; j++)
            {
                adjacentRooms[j] = getPossibleCave(j, i);
            }
            return adjacentRooms;
        }
        public String toString()
        {
            String str = "";
            for (int i = 0; i < NUMBER_OF_ROOMS; i++)
            {
                str += rooms[i].toString();
            }
            str += "\n";
            for (int i = 1; i < NUMBER_OF_ROOMS + 1; i++)
            {
                str += (i) + ": ";
                Boolean[] linkedRooms = getLinkedRooms(i);
                for (int j = 0; j < 6; j++)
                {
                    if (linkedRooms[j])
                    {
                        str += "true, ";
                    }
                    else
                    {
                        str += "false, ";
                    }

                }

            }
            return str;
        }
               
        public Boolean everyRoomHasCave()
        {
            
            List<int> l = new List<int>();
            int size = 0;
            for (int i = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < rooms[i].getNumCaves(); j++)
                {
                    if(!l.Contains(rooms[i].getCaves()[j]))
                    {
                        l.Add(rooms[i].getCaves()[j]);
                        size++;
                    }

                }

            }
            return size == 30;

        }

        public List<int> getValidCaves(Room r)
        {
            List<int> validCaves = new List<int>();
            int index = r.getIndex() - 1;
            for (int i = 0; i < 6; i++)
            {
                int possibleCave = getPossibleCave(i, index);                
                if (rooms[possibleCave - 1].canAddCave())
                {
                    validCaves.Add(possibleCave);
                }
            }
            return validCaves;

        }
        private int getPossibleCave(int function, int index)
        {
            int possibleCave = -1;
            if ((index + 1) % 6 == 0)
            {
                possibleCave = getEvenConnectingRow(function, index);
            }
            //even row
            else if ((index + 1) % 2 == 0)
            {
                possibleCave = getEvenFunction(function, index);
            }
            //odd connecting row
            else if (index % 6 == 0)
            {
                possibleCave = getOddConnectingRow(function, index);
            }
            //odd row
            else
            {
                possibleCave = getOddFunction(function, index);
            }
            if (possibleCave <= 0)
            {
                possibleCave += 30;
            }
            if (possibleCave > 30)
            {
                possibleCave -= 30;
            }
            return possibleCave;
        }

    }
}
