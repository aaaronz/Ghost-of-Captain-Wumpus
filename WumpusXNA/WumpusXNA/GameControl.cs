using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusXNA
{
    class GameControl
    {
        private Cave _cave;
        private HighScore _highscore;
        private Trivia _trivia;
        public Player _player;
        private Map _map;
        private GUI GUI;
        private Sound _sound;

        public GameControl(GUI form)
        {
            _cave = new Cave();
            _highscore = new HighScore();
            _trivia = new Trivia();
            _player = new Player();
            _map = new Map();
            GUI = form;
            _sound = new Sound();
            _sound.playLoop("GamePlay");
            _cave.toString();
        }

        public void moveDirection(int direction)
        {
            //check if move is possible, if not end method with "return statement"

            if (!_cave.checkForRoom(_player.getMapLocation(), direction))
            {
                throw new Exception("Couldn't make move lol");
                //return;
            }
            _player.newTurn();
            int newRoomNum = _cave.getAdjacentRooms(_player.getMapLocation())[direction];
            RoomInfo info = updateRoomInfo(newRoomNum, (direction + 3) % 6);
            GUI.updateRoom(info);
            _sound.playEffect("BubbleWhoosh");
            //set new location to adjacent cave
            _player.setMapLocation(newRoomNum);

            //update score
            updateGUIStats();

        }

        public int numArrows()
        {
            return _player.getNumArrows();
        }

        public void arrowsShot()
        {
            if (_player.arrowShot())
            {
                _sound.playEffect("Harpoon");
            }
        }

        public void addACoin()
        {
            _player.addCoins(1);
            _map.pickUpCoin(_player.getMapLocation());
            _sound.playEffect("Coins");
            updateGUIStats();
        }

        public void purchaseArrows(int numArrows)
        {
            //add arrows to inventory
            _player.addArrows(numArrows);
            //update values on GUI stub
            GUI.displayArrows(_player.getNumArrows());
            GUI.displayCoins(_player.getNumCoins());
        }

        public void highScoresRequest()
        {
            GUI.updateHighScore(_highscore.getHighScores());
        }

        public void initializeFirstRoom()
        {
            RoomInfo info = updateRoomInfo(1, 0);
            GUI.updateRoom(info);
            updateGUIStats();
        }

        public void minigame(bool win)
        {
            Random gen = new Random();
            if(win)
            {
                int roomNum = gen.Next(30) + 1;
                GUI.updateRoom(updateRoomInfo(roomNum, gen.Next(6)));
                _player.setMapLocation(roomNum);
            }
            else
            {
                endGame();
            }
        }

        public RoomInfo updateRoomInfo(int newRoomNum, int startingDirection)
        {
            //tell GUI to updateRoom. give it roomInfo object
            bool containsBats = _map.checkForHazards(newRoomNum) == 1;
            bool containsArmory = _map.checkForHazards(newRoomNum) == 2;
            bool containsWumpus = GUI.wumpusLocation == newRoomNum;
            int numCoins = _map.checkForCoins(newRoomNum);
            RoomInfo info = new RoomInfo(_cave, newRoomNum, numCoins, containsBats, containsArmory, containsWumpus, startingDirection, _cave.getLinkedRooms(newRoomNum));
            return info;
        }

        private void updateGUIStats()
        {
            GUI.displayRoomNumber(_player.getMapLocation());
            GUI.displayScore(_player.calculateScore());
            GUI.displayArrows(_player.getNumArrows());
            GUI.displayCoins(_player.getNumCoins());
            GUI.displayTurns(_player.getNumTurns());
        }

        public void endGame()
        {
            _highscore.addNewScore(new Score(_player.calculateScore(), "", _player.getNumTurns(), _player.getNumCoins(), _player.getNumArrows()));
            GUI.Exit();
        }
    }
}
