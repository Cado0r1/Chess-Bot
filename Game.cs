using System;
using ChessEngine.WindowsUI;

namespace ChessEngine.Game
{
    public class ChessGame
    {
        Moves MoveGen = new Moves();
        public static int turn = 1;
        public bool WhiteKingCheck = false;
        public bool BlackKingCheck = false;

        public void RunMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        public static bool Turn()
        {
            if(turn%2 == 0)
            {
                return false;
            }
            return true;
        }

        public void PrintBitBoard(ulong Board, string Bitboard)
        {
            Console.WriteLine($"{Bitboard}: ");
            Console.WriteLine("");
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;
                    ulong mask = (ulong)1 << square;
                    Console.Write((Board & mask) != 0 ? "1 " : "0 ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    
        public bool CanMove(int From, int To)
        {   
            From = IndexFlipper(From);
            To = IndexFlipper(To);
            ulong Move = Moves.SortMove(IntToBitboard(From), Turn(), IntToBitboard(To),WhiteKingCheck,BlackKingCheck);
            if((Move & IntToBitboard(To)) != 0)
            {
                Moves.UpdateBitboard(IntToBitboard(From), IntToBitboard(To));
                return true;
            }
            return false;
        }

        public void IncrementTurn()
        {
            WhiteKingCheck = Moves.IsAttacked(Moves.WhiteKing,true) ? true : false; 
            BlackKingCheck = Moves.IsAttacked(Moves.BlackKing,false) ? true : false;
            Console.WriteLine($"WhiteKingCheck:{WhiteKingCheck}    BlackKingCheck:{BlackKingCheck}");
            turn++;
        }

        public ulong IntToBitboard(int number){
            return (ulong)1 << number;
        }

        public int IndexFlipper(int index){
            Dictionary<int, int> Offset = new Dictionary<int, int>();
            Offset[7] = -7;
            Offset[6] = -5;
            Offset[5] = -3;
            Offset[4] = -1;
            Offset[3] = 1;
            Offset[2] = 3;
            Offset[1] = 5;
            Offset[0] = 7;

            return index + Offset[index%8];
        }
    }
}
