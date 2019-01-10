using System;
using System.Collections.Generic;

namespace Day9_MarbleMania
{
    public class Player
    {
        public Player(int playerNum)
        {
            PlayerNumber = playerNum;
            KeptMarbles = new List<Marble>();
        }

        public int PlayerNumber { get; }

        public List<Marble> KeptMarbles { get; }

        public ulong Score
        {
            get
            {
                ulong score = 0;
                foreach (var marb in KeptMarbles)
                    score += (ulong) marb.Value;
                return score;
            }
        }

        public void KeepMarble(Marble marble)
        {
            KeptMarbles.Add(marble);
        }
    }
}