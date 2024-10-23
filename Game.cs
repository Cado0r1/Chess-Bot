using System;
using System.Windows;
using System.Collections.Generic;
using ChessEngine.WindowsUI;
using static ChessEngine.Game.Moves;
using static ChessEngine.Game.FenService;
using static ChessEngine.Game.BitboardOperations;

namespace ChessEngine.Game
{
    public class ChessGame
    {
        public static int turn = 1;
        public static bool WhiteKingCheck = false;
        public static bool BlackKingCheck = false;
        public static ulong[] Positions = new ulong[12];
        public static List<Gamestate> Movelist = new List<Gamestate>();

        public void RunMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        public static bool Turn()
        {
            return turn % 2 != 0;
        }

        public static bool CanMove(int from, int to)
        {   
            from = IndexFlipper(from);
            to = IndexFlipper(to);
            ulong move = SortMove(IntToBitboard(from), Turn(), IntToBitboard(to), WhiteKingCheck, BlackKingCheck);
            if ((move & IntToBitboard(to)) != 0)
            {
                UpdateBitboard(IntToBitboard(from), IntToBitboard(to));
                return true;
            }
            return false;
        }

        public static void IncrementTurn()
        {
            IsCheckmate();
            Console.WriteLine(IsCheckmate());
            SetPositions();
            turn++;
        }

        public static bool IsCheckmate()
        {
            WhiteKingCheck = IsAttacked(WhiteKing, true);
            Console.WriteLine(WhiteKingCheck = IsAttacked(WhiteKing, true));
            BlackKingCheck = IsAttacked(BlackKing, false);
            Console.WriteLine(BlackKingCheck = IsAttacked(BlackKing, false));
            if (WhiteKingCheck || BlackKingCheck)
            {
                ulong checkMask = WhiteKingCheck? GetCheckMask(WhiteKing, WhiteKingCheck) : GetCheckMask(BlackKing,WhiteKingCheck);
                PrintBitBoard(checkMask,"checkMask");
                Console.WriteLine(GetCheckPiece(checkMask,WhiteKingCheck));
                Console.WriteLine(GetCheckPiece(checkMask,!WhiteKingCheck));
                if (!IsAttacked(GetCheckPiece(checkMask,WhiteKingCheck),!WhiteKingCheck,false,true))
                {
                    if (!IsAttacked(checkMask, !WhiteKingCheck, false, false))
                    {   
                        if (IsAttacked(GenerateKingMoves(WhiteKingCheck ? WhiteKing : BlackKing, WhiteKingCheck, 0UL, false), WhiteKingCheck))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Checkmate");
                            Console.WriteLine($"Game ended in {turn} turns");
                            string won = WhiteKingCheck ? "Black" : "White";
                            Console.WriteLine($"{won} Won");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    public class Gamestate
    {
        public int TurnNum { get; private set; }
        public ulong[] PiecesPositions { get; private set; }
        public string MoveMade { get; private set; }

        public Gamestate(int turnNum, ulong[] piecesPositions, string moveMade)
        {
            TurnNum = turnNum;
            PiecesPositions = (ulong[])piecesPositions.Clone();
            MoveMade = moveMade;
        }
    }
}
