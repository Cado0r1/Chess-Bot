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
            SetPositions();
            turn++;
        }
    }
}
