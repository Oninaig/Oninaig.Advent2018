using System;

namespace Day9_MarbleMania
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var board = new MarbleGameBoard(9, 25);
            //var bestPlayer =board.Play();
            //Console.WriteLine($"{board.LastMarble} - High score: {bestPlayer.Score}");

            //var board2 = new MarbleGameBoard(10, 1618);
            //var bestPlayer2 =board2.Play();
            //Console.WriteLine($"{board2.LastMarble} - High score: {bestPlayer2.Score}");

            //var board3 = new MarbleGameBoard(13,7999);
            //var bestPlayer3 = board3.Play();
            //Console.WriteLine($"{board3.LastMarble} - High score: {bestPlayer3.Score}");

            //var puzzleBoard = new MarbleGameBoard(428, 70825);
            //var puzzleWinner = puzzleBoard.Play();
            //Console.WriteLine($"{puzzleBoard.LastMarble} - High score: {puzzleWinner.Score}");

            var puzzleBoardP2 = new MarbleGameBoard(428, 70825 * 100);
            var puzzleWinnerP2 = puzzleBoardP2.Play();
            Console.WriteLine($"{puzzleBoardP2.LastMarble} - High score: {puzzleWinnerP2.Score}");
            Console.ReadLine();
        }
    }
}