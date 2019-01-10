using System;
using System.Collections.Generic;

namespace Day9_MarbleMania
{
    public class MarbleGameBoard
    {
        private readonly IEnumerator<Player> _playerEnum;
        public readonly int LastMarble;
        public LinkedList<Marble> Board { get; }
        public Marble CurrentMarble { get; private set; }
        public LinkedListNode<Marble> CurrentMarbleNode { get; private set; }
        public Dictionary<int, Player> PlayerDict { get; }
        public InfiniteEnumerable<Player> PlayersPlayOrder { get; }


        public Player Play()
        {
            if (CurrentMarble == null)
                AddMarble(new Marble(0));
            Console.WriteLine($"Playing game with last marble worth {LastMarble} points.");
            for (var i = 1; i <= LastMarble; i++)
            {
                if (i % 1000000 == 0) Console.WriteLine($"On turn {i}");
                TakeTurn(i);
            }

            return FindWinner();
        }

        private Player FindWinner()
        {
            ulong highScore = 0;
            Player bestPlayer = null;
            foreach (var player in PlayerDict.Values)
                if (player.Score > highScore)
                {
                    highScore = player.Score;
                    bestPlayer = player;
                }

            return bestPlayer;
        }

        public void TakeTurn(int marbleNum)
        {
            _playerEnum.MoveNext();
            var currPlayerOrder = _playerEnum.Current;


            var marbleToAdd = new Marble(marbleNum);
            if (marbleToAdd.Value % 23 == 0)
            {
                var currPlayer = PlayerDict[currPlayerOrder.PlayerNumber];
                currPlayer.KeepMarble(marbleToAdd);

                //remove marble 7 marbles counter-clockwise from current marble
                var marbleToRemove = Board.CircularMoveBack(CurrentMarbleNode, 7);
                currPlayer.KeepMarble(marbleToRemove.Value);

                if (Board.Last == marbleToRemove)
                    SetCurrentMarble(Board.First.Value, Board.First);
                else
                    SetCurrentMarble(marbleToRemove.Next.Value, marbleToRemove.Next);
                Board.Remove(marbleToRemove);
            }
            else
            {
                AddMarble(marbleToAdd);
            }
        }

        private void SetCurrentMarble(Marble marble)
        {
            if (CurrentMarble != null)
                CurrentMarble.IsCurrent = false;
            marble.IsCurrent = true;
            CurrentMarble = marble;
        }

        private void SetCurrentMarble(Marble marble, LinkedListNode<Marble> marbleNode)
        {
            CurrentMarbleNode = marbleNode;
            SetCurrentMarble(marble);
        }

        public void AddMarble(Marble marble)
        {
            if (CurrentMarble == null)
            {
                var added = Board.AddFirst(marble);
                SetCurrentMarble(marble, added);
            }
            else
            {
                if (CurrentMarble == Board.Last.Value)
                {
                    var added = Board.AddAfter(Board.First, marble);
                    SetCurrentMarble(marble, added);
                }
                else
                {
                    //var currentMarbleNode = Board.Find(CurrentMarble);
                    var added = Board.AddAfter(CurrentMarbleNode.Next, marble);
                    SetCurrentMarble(marble, added);
                }
            }
        }

        public void PrintBoard()
        {
            foreach (var marble in Board)
                Console.Write($"{(!marble.IsCurrent ? $"{marble.Value} " : $"( {marble.Value} ) ")}");
        }

        #region Constructors

        public MarbleGameBoard(int numPlayers, int lastMarble) : this(numPlayers)
        {
            LastMarble = lastMarble;
            _playerEnum = PlayersPlayOrder.GetEnumerator();
        }

        private MarbleGameBoard(int numPlayers) : this()
        {
            var tempArray = new Player[numPlayers];
            PlayerDict = new Dictionary<int, Player>();

            for (var i = 0; i < numPlayers; i++)
            {
                tempArray[i] = new Player(i + 1);
                PlayerDict[i + 1] = new Player(i + 1);
            }

            PlayersPlayOrder = new InfiniteEnumerable<Player>(tempArray);
        }

        private MarbleGameBoard()
        {
            Board = new LinkedList<Marble>();
        }

        #endregion
    }
}