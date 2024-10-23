using static ChessEngine.Game.ChessGame;

namespace ChessEngine.Game
{
	public class BitboardOperations
	{
		public static void PrintBitBoard(ulong Board, string Bitboard)
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

        public static ulong IntToBitboard(int number){
            return (ulong)1 << number;
        }

        public static int IndexFlipper(int index){
            Dictionary<int, int> Offset = new Dictionary<int, int>
            {
                {7, -7}, {6, -5}, {5, -3}, {4, -1},
                {3, 1}, {2, 3}, {1, 5}, {0, 7}
            };
            return index + Offset[index%8];
        }

        public static void PrintAll()
        {
            PrintBitBoard(WhitePawns, "White Pawns");
            PrintBitBoard(WhiteKnights, "White Knights");
            PrintBitBoard(WhiteBishops, "White Bishops");
            PrintBitBoard(WhiteRooks, "White Rooks");
            PrintBitBoard(WhiteQueen, "White Queen");
            PrintBitBoard(WhiteKing, "White King");

            PrintBitBoard(BlackPawns, "Black Pawns");
            PrintBitBoard(BlackKnights, "Black Knights");
            PrintBitBoard(BlackBishops, "Black Bishops");
            PrintBitBoard(BlackRooks, "Black Rooks");
            PrintBitBoard(BlackQueen, "Black Queen");
            PrintBitBoard(BlackKing, "Black King");
        }

        public static void resetBitboards()
        {
            WhitePawns = 0UL;
            WhiteKnights = 0UL;
            WhiteBishops = 0UL;
            WhiteRooks = 0UL;
            WhiteQueen = 0UL;
            WhiteKing = 0UL;
        
            BlackPawns = 0UL;
            BlackKnights = 0UL;
            BlackBishops = 0UL;
            BlackRooks = 0UL;
            BlackQueen = 0UL;
            BlackKing = 0UL;
        }

        public static void SortBitboards(string fen)
        {
            if(fen.IndexOf(' ') != -1)
            {
                fen = fen.Substring(0, fen.IndexOf(' '));
            }
            Console.WriteLine(fen);
            for(int i = 0; i < fen.Length; i++)
            {
                char piece;
                if(fen[i] != '1'){
                    piece = fen[i];
                    int position = i -63;
                    if(position < 0) position = -position;
                    position = IndexFlipper(position);
                    switch (piece)
                    {
                        case 'r':  
                            BlackRooks |= (ulong)1 << position;
                            break;
                        case 'n':  
                            BlackKnights |= (ulong)1 << position;
                            break;
                        case 'b':  
                            BlackBishops |= (ulong)1 << position;
                            break;
                        case 'q':  
                            BlackQueen |= (ulong)1 << position;
                            break;
                        case 'k':  
                            BlackKing |= (ulong)1 << position;
                            break;
                        case 'p':  
                            BlackPawns |= (ulong)1 << position;
                            break;
                        case 'R':  
                            WhiteRooks |= (ulong)1 << position;
                            break;
                        case 'N':  
                            WhiteKnights |= (ulong)1 << position;
                            break;
                        case 'B':  
                            WhiteBishops |= (ulong)1 << position;
                            break;
                        case 'Q':  
                            WhiteQueen |= (ulong)1 << position;
                            break;
                        case 'K':  
                            WhiteKing |= (ulong)1 << position;
                            break;
                        case 'P':  
                            WhitePawns |= (ulong)1 << position;
                            break;
                        default:  
                            Console.WriteLine("Invalid piece type");
                            break;
                    }
                }
            }
        }

        public static ulong WhitePawns,BlackPawns,WhiteBishops,BlackBishops,WhiteKnights,BlackKnights,WhiteRooks,BlackRooks,WhiteKing,BlackKing,WhiteQueen,BlackQueen;
        public static ulong Rank8 = 0xFF00000000000000;
        public static ulong Rank7 = 0x00FF000000000000;
        public static ulong Rank6 = 0x0000FF0000000000;
        public static ulong Rank5 = 0x000000FF00000000;
        public static ulong Rank4 = 0x00000000FF000000;
        public static ulong Rank3 = 0x0000000000FF0000;
        public static ulong Rank2 = 0x000000000000FF00;
        public static ulong Rank1 = 0x00000000000000FF;
        public static ulong FileA = 0x0101010101010101;
        public static ulong FileB = 0x0202020202020202;
        public static ulong FileC = 0x0404040404040404;
        public static ulong FileD = 0x0808080808080808;
        public static ulong FileE = 0x1010101010101010;
        public static ulong FileF = 0x2020202020202020;
        public static ulong FileG = 0x4040404040404040;
        public static ulong FileH = 0x8080808080808080;
        public static ulong WhitePin = 0UL;
        public static ulong BlackPin = 0UL;

        

        public static ulong GetWhitePieces()
        {
            return WhitePawns | WhiteBishops | WhiteKing | WhiteRooks | WhiteQueen | WhiteKnights;
        }

        public static ulong GetBlackPieces()
        {
            return BlackPawns | BlackBishops | BlackKing | BlackKnights | BlackQueen | BlackRooks;
        }

        public static ulong GetAllPieces()
        {
            return GetBlackPieces() | GetWhitePieces();
        }

        public static void UpdateBitboard(ulong From, ulong To)
        {
            bool isWhite = (From & GetWhitePieces()) != 0;
        
            if (isWhite)
            {
                if ((From & WhitePawns) != 0) WhitePawns = (WhitePawns & ~From) | To;
                if ((From & WhiteBishops) != 0) WhiteBishops = (WhiteBishops & ~From) | To;
                if ((From & WhiteKnights) != 0) WhiteKnights = (WhiteKnights & ~From) | To;
                if ((From & WhiteRooks) != 0) WhiteRooks = (WhiteRooks & ~From) | To;
                if ((From & WhiteQueen) != 0) WhiteQueen = (WhiteQueen & ~From) | To;
                if ((From & WhiteKing) != 0) WhiteKing = (WhiteKing & ~From) | To;
            }
            else
            {
                if ((From & BlackPawns) != 0) BlackPawns = (BlackPawns & ~From) | To;
                if ((From & BlackBishops) != 0) BlackBishops = (BlackBishops & ~From) | To;
                if ((From & BlackKnights) != 0) BlackKnights = (BlackKnights & ~From) | To;
                if ((From & BlackRooks) != 0) BlackRooks = (BlackRooks & ~From) | To;
                if ((From & BlackQueen) != 0) BlackQueen = (BlackQueen & ~From) | To;
                if ((From & BlackKing) != 0) BlackKing = (BlackKing & ~From) | To;
            }
        
            if (isWhite)
            {
                if ((To & BlackPawns) != 0) BlackPawns &= ~To;
                if ((To & BlackBishops) != 0) BlackBishops &= ~To;
                if ((To & BlackKnights) != 0) BlackKnights &= ~To;
                if ((To & BlackRooks) != 0) BlackRooks &= ~To;
                if ((To & BlackQueen) != 0) BlackQueen &= ~To;
                if ((To & BlackKing) != 0) BlackKing &= ~To;
            }
            else
            {
                if ((To & WhitePawns) != 0) WhitePawns &= ~To;
                if ((To & WhiteBishops) != 0) WhiteBishops &= ~To;
                if ((To & WhiteKnights) != 0) WhiteKnights &= ~To;
                if ((To & WhiteRooks) != 0) WhiteRooks &= ~To;
                if ((To & WhiteQueen) != 0) WhiteQueen &= ~To;
                if ((To & WhiteKing) != 0) WhiteKing &= ~To;
            }
        }

        public static void SetPositions()
        {
            Positions[0] = WhitePawns;
            Positions[1] = WhiteRooks;
            Positions[2] = WhiteKnights;
            Positions[3] = WhiteBishops;
            Positions[4] = WhiteQueen;
            Positions[5] = WhiteKing;
            Positions[6] = BlackPawns;
            Positions[7] = BlackRooks;
            Positions[8] = BlackKnights;
            Positions[9] = BlackBishops;
            Positions[10] = BlackQueen;
            Positions[11] = BlackKing;   
        }

        public static void SetBitboardsWPositions(bool white = true, bool black = true)
        {
            if(white == true){
                WhitePawns  = Positions[0];
                WhiteRooks = Positions[1];
                WhiteKnights = Positions[2];
                WhiteBishops = Positions[3];
                WhiteQueen = Positions[4];
                WhiteKing = Positions[5];
            }
            if(black == true){
                BlackPawns = Positions[6];
                BlackRooks = Positions[7];
                BlackKnights = Positions[8];
                BlackBishops = Positions[9];
                BlackQueen = Positions[10];
                BlackKing = Positions[11];
            }
        }
	}
}