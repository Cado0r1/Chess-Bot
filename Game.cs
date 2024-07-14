using System;
using ChessEngine.WindowsUI;

namespace ChessEngine.Game
{
    public class ChessGame
    {
        Moves MoveGen = new Moves();
        public void RunMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        public void PrintBitBoard(ulong Board)
        {
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

        public void PrintBitBoardindex()
        {
            int count = 0;
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    Console.Write(count+" ");
                    count++;
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    
        public bool CanMove(int From, int To)
        {   
            From = IndexFlipper(From);
            To = IndexFlipper(To);
            Tuple<ulong,bool> Move = Moves.SortMove(IntToBitboard(From), false);
            if(Move.Item2 == true)
            {
               if((Move.Item1 & IntToBitboard(To)) != 0)
                {
                    Moves.UpdateBitboard(IntToBitboard(From), IntToBitboard(To));
                    if(Moves.SortMove(IntToBitboard(To), true).Item1 == 0){
                        Moves.UpdateBitboard(IntToBitboard(To), IntToBitboard(From));
                        return false;
                    }
                    return true;
                } 
            }
            if((Move.Item1 & IntToBitboard(To)) != 0)
            {
                Moves.UpdateBitboard(IntToBitboard(From), IntToBitboard(To));
                return true;
            }
            return false;
        }

        public void PrintBB(){
            for(int i = 0;i<64;i++)
            {
                Console.WriteLine($"i was shifted by {i}");
                PrintBitBoard(IntToBitboard(i));
            }
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
