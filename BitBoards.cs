namespace ChessEngine.Game
{
    public static class BitBoards{
        public static ulong WhitePawns = 0x000000000000FF00;
        public static ulong BlackPawns = 0x00FF000000000000;
        public static ulong WhiteBishops=0x0000000000000024;
        public static ulong BlackBishops=0x2400000000000000;
        public static ulong WhiteKnights=0x0000000000000042;
        public static ulong BlackKnights=0x4200000000000000;
        public static ulong WhiteRooks = 0x0000000000000081;
        public static ulong BlackRooks = 0x8100000000000000;
        public static ulong WhiteKing =  0x0000000000000010;
        public static ulong BlackKing =  0x1000000000000000;
        public static ulong WhiteQueen  =0x0000000000000008;
        public static ulong BlackQueen  =0x0800000000000000;
    }
}