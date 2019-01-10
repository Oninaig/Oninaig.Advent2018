using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Day9_MarbleMania
{

    public class Player
    {
        public int PlayerNumber { get; private set; }

        public List<Marble> KeptMarbles { get; private set; }

        public UInt64 Score
        {
            get
            {
                UInt64 score = 0;
                foreach (var marb in KeptMarbles)
                    score += (ulong)marb.Value;
                return score;
            }
        }

        public Guid UniqueId { get; private set; }

        public Player(int playerNum)
        {
            PlayerNumber = playerNum;
            this.KeptMarbles = new List<Marble>();
            //this.UniqueId = Guid.NewGuid();
        }

        public void KeepMarble(Marble marble)
        {
            KeptMarbles.Add(marble);
        }


    }
    public class Marble
    {
        public bool IsCurrent { get; set; }
        public int Value { get; private set; }


        public Marble(int value)
        {
            Value = value;
        }
    }

    public static class LinkedListExtensions
    {
        public static LinkedListNode<T> CircularMoveBack<T>(this LinkedList<T> lList, LinkedListNode<T> element, int moveCount)
        {
            var currElement = element;
            while (moveCount > 0)
            {
                if (currElement == lList.First)
                    currElement = lList.Last;
                else
                    currElement = currElement.Previous;
                moveCount--;

            }

            return currElement;
        }
    }
    public class MarbleGameBoard
    {
        public LinkedList<Marble> Board { get; }
        public Marble CurrentMarble { get; private set; }
        public LinkedListNode<Marble> CurrentMarbleNode { get; private set; }
        public Dictionary<int, Player> PlayerDict { get; private set; }
        public InfiniteEnumerable<Player> PlayersPlayOrder { get; private set; }
        public readonly int LastMarble;

        private readonly IEnumerator<Player> _playerEnum;

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

            for (int i = 0; i < numPlayers; i++)
            {
                tempArray[i] = new Player(i + 1);
                PlayerDict[i + 1] = new Player(i + 1);
            }

            PlayersPlayOrder = new InfiniteEnumerable<Player>(tempArray);
        }

        private MarbleGameBoard()
        {
            this.Board = new LinkedList<Marble>();
        }

        #endregion


        public Player Play()
        {
            if (CurrentMarble == null)
                AddMarble(new Marble(0));
            Console.WriteLine($"Playing game with last marble worth {LastMarble} points.");
            for (int i = 1; i <= LastMarble; i++)
            {
                if (i % 100000 == 0)
                {
                    Console.WriteLine($"On turn {i}");
                }
                TakeTurn(i);
            }

            return FindWinner();
        }

        private Player FindWinner()
        {
            UInt64 highScore = 0;
            Player bestPlayer = null;
            foreach (var player in PlayerDict.Values)
            {
                if (player.Score > highScore)
                {
                    highScore = player.Score;
                    bestPlayer = player;
                }
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
            this.CurrentMarbleNode = marbleNode;
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
            {
                Console.Write($"{(!marble.IsCurrent ? $"{marble.Value} " : $"( {marble.Value} ) ")}");
            }
        }

    }
}
