using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Xps.Serialization;

namespace ChessEngine.Game
{
    public class Moves
    {

        public static ChessGame Board = new ChessGame();

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
        public static ulong SouthWall =  0x00000000000000FF;
        public static ulong NorthWall =  0xFF00000000000000;
        public static ulong EastWall =   0x8080808080808080;
        public static ulong WestWall =   0x0101010101010101;
        public static ulong Rank7 =      0x00FF000000000000;
        public static ulong Rank6 =      0x0000FF0000000000;
        public static ulong Rank5 =      0x000000FF00000000;
        public static ulong Rank4 =      0x00000000FF000000;
        public static ulong Rank3 =      0x0000000000FF0000;
        public static ulong Rank2 =      0x000000000000FF00;
        public static ulong FileB =      0x0202020202020202;
        public static ulong FileC =      0x0404040404040404;
        public static ulong FileD =      0x0808080808080808;
        public static ulong FileE =      0x1010101010101010;
        public static ulong FileF =      0x2020202020202020;
        public static ulong FileG =      0x4040404040404040;
        



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

        public static Tuple<ulong, bool> SortMove(ulong square, bool CheckAttack)
        {
            if ((square & GetWhitePieces()) != 0 )
            {
                if((square & WhiteKing) != 0) return Tuple.Create(GenerateKingMoves(square, true, CheckAttack),true);
                if((square & WhitePawns) != 0) return Tuple.Create(GeneratePawnMoves(square, true, false),false);
                if((square & WhiteRooks) != 0) return Tuple.Create(GenerateRookMoves(square, true),false);
                if((square & WhiteKnights) != 0) return Tuple.Create(GenerateKnightMoves(square, true),false);
                if((square & WhiteBishops) != 0) return Tuple.Create(GenerateBishopMoves(square, true),false);
                if((square & WhiteQueen) != 0) return Tuple.Create(GenerateQueenMoves(square, true),false);
                return Tuple.Create(0UL,false);
            }
            else
            {
                if((square & BlackKing) != 0) return Tuple.Create(GenerateKingMoves(square, false, CheckAttack),true);
                if((square & BlackPawns) != 0) return Tuple.Create(GeneratePawnMoves(square, false, false),false);
                if((square & BlackRooks) != 0) return Tuple.Create(GenerateRookMoves(square, false),false);
                if((square & BlackKnights) != 0) return Tuple.Create(GenerateKnightMoves(square, false),false);
                if((square & BlackBishops) != 0) return Tuple.Create(GenerateBishopMoves(square, false),false);
                if((square & BlackQueen) != 0) return Tuple.Create(GenerateQueenMoves(square, false),false);
                return Tuple.Create(0UL,false);
            }
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

        public static ulong GeneratePawnMoves(ulong square, bool isWhite, bool OnlyReturnAttack)
        {
            ulong moves = 0;
            ulong SinglePawnPush = isWhite ? square << 8 : square >> 8;
            if ((SinglePawnPush & GetAllPieces()) == 0)
            {
                moves = moves | SinglePawnPush;
            }
            ulong DoublePawnPush = isWhite ? square << 16 : square >> 16;
            if (((SinglePawnPush | DoublePawnPush) & GetAllPieces()) == 0 && (isWhite ? (square & 0x000000000000FF00UL) != 0 : (square & 0x00FF000000000000UL) != 0))
            {
                moves = moves | DoublePawnPush;
            }

            ulong LeftTake = isWhite ? square << 9 : square >> 7;
            if ((LeftTake & (isWhite ? GetBlackPieces() : GetWhitePieces())) != 0 && (square & 0x8080808080808080UL) == 0)
            {
                moves = moves | LeftTake;
            }
            ulong RightTake = isWhite ? square << 7 : square >> 9;
            if ((RightTake & (isWhite ? GetBlackPieces() : GetWhitePieces())) != 0 && (square & 0x0101010101010101UL) == 0)
            {
                moves = moves | RightTake;
            }
            if (OnlyReturnAttack)
            {
                ulong attacks = LeftTake | RightTake;
                return moves & attacks;
            }
            else
            {
                return moves;
            }
        }

        public static ulong NorthFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & NorthWall) != 0)
            {
                return moves;
            }
            ulong North = square << 8;
            while (true)
            {
                if ((North & TeamPieces) == 0)
                {
                    if ((North & OpposingPieces) != 0 || (North & NorthWall) != 0)
                    {
                        moves = moves | North;
                        return moves;
                    }
                    else
                    {
                        moves = moves | North;
                        North = North << 8;
                    }
                }
                else
                {
                    return moves;
                }
            }
        }

        public static ulong EastFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & EastWall) != 0)
            {
                return moves;
            }
            ulong East = square << 1;
            while (true)
            {
                if ((East & TeamPieces) == 0)
                {
                    if ((East & OpposingPieces) != 0 || (East & EastWall) != 0)
                    {
                        moves = moves | East;
                        return moves;
                    }
                    else
                    {
                        moves = moves | East;
                        East = East << 1;
                    }
                }
                else
                {
                    return moves;
                }
            }
        }

        public static ulong WestFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & WestWall) != 0)
            {
                return moves;
            }
            ulong West = square >> 1;
            while (true)
            {
                if ((West & TeamPieces) == 0)
                {
                    if ((West & OpposingPieces) != 0 || (West & WestWall) != 0)
                    {
                        moves = moves | West;
                        return moves;
                    }
                    else
                    {
                        moves = moves | West;
                        West = West >> 1;
                    }
                }
                else
                {
                    return moves;
                }
            }
        }

        public static ulong SouthFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & SouthWall) != 0)
            {
                return moves;
            }
            ulong South = square >> 8;
            while (true)
            {
                if ((South & TeamPieces) == 0)
                {
                    if ((South & OpposingPieces) != 0 || (South & SouthWall) != 0)
                    {
                        moves = moves | South;
                        return moves;
                    }
                    else
                    {
                        moves = moves | South;
                        South = South >> 8;
                    }
                }
                else
                {
                    return moves;
                }
            }
        }

        public static ulong NorthWestFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & (NorthWall | WestWall)) != 0)
            {
                return moves;
            }
            ulong NorthWest = square << 7;
            while (true)
            {
                if ((NorthWest & TeamPieces) == 0)
                {
                    if ((NorthWest & OpposingPieces) != 0 || (NorthWest & (NorthWall | WestWall)) != 0)
                    {
                        moves = moves | NorthWest;
                        return moves;
                    }
                    else
                    {
                        moves = moves | NorthWest;
                        NorthWest = NorthWest << 7;
                    }
                }
                else
                {
                    return moves;
                }
            }

        }

        public static ulong NorthEastFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & (NorthWall | EastWall)) != 0)
            {
                return moves;
            }
            ulong NorthEast = square << 9;
            while (true)
            {
                if ((NorthEast & TeamPieces) == 0)
                {
                    if ((NorthEast & OpposingPieces) != 0 || (NorthEast & (NorthWall | EastWall)) != 0)
                    {
                        moves = moves | NorthEast;
                        return moves;
                    }
                    else
                    {
                        moves = moves | NorthEast;
                        NorthEast = NorthEast << 9;
                    }
                }
                else
                {
                    return moves;
                }
            }

        }

        public static ulong SouthWestFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & (SouthWall | WestWall)) != 0)
            {
                return moves;
            }
            ulong SouthWest = square >> 9;
            while (true)
            {
                if ((SouthWest & TeamPieces) == 0)
                {
                    if ((SouthWest & OpposingPieces) != 0 || (SouthWest & (SouthWall | WestWall)) != 0)
                    {
                        moves = moves | SouthWest;
                        return moves;
                    }
                    else
                    {
                        moves = moves | SouthWest;
                        SouthWest = SouthWest >> 9;
                    }
                }
                else
                {
                    return moves;
                }
            }

        }

        public static ulong SouthEastFill(ulong square, bool IsWhite)
        {
            ulong moves = 0;
            (ulong OpposingPieces, ulong TeamPieces) = IsWhite ? (GetBlackPieces(), GetWhitePieces()) : (GetWhitePieces(), GetBlackPieces());
            if ((square & (SouthWall | EastWall)) != 0)
            {
                return moves;
            }
            ulong SouthEast = square >> 7;
            while (true)
            {
                if ((SouthEast & TeamPieces) == 0)
                {
                    if ((SouthEast & OpposingPieces) != 0 || (SouthEast & (SouthWall | EastWall)) != 0)
                    {
                        moves = moves | SouthEast;
                        return moves;
                    }
                    else
                    {
                        moves = moves | SouthEast;
                        SouthEast = SouthEast >> 7;
                    }
                }
                else
                {
                    return moves;
                }
            }

        }

        public static ulong GenerateRookMoves(ulong square, bool IsWhite){
            ulong NorthMoves = NorthFill(square, IsWhite);
            ulong EastMoves = EastFill(square, IsWhite);
            ulong WestMoves = WestFill(square, IsWhite);
            ulong SouthMoves  = SouthFill(square, IsWhite);
            return NorthMoves | EastMoves | WestMoves | SouthMoves;
        }

        public static ulong GenerateBishopMoves(ulong square, bool IsWhite){
            ulong NorthWestMoves = NorthWestFill(square, IsWhite);
            ulong NorthEastMoves = NorthEastFill(square, IsWhite);
            ulong SouthWestMoves = SouthWestFill(square, IsWhite);
            ulong SouthEastMoves  = SouthEastFill(square, IsWhite);
            return NorthWestMoves | NorthEastMoves | SouthEastMoves | SouthWestMoves;
        }

        public static ulong GenerateQueenMoves(ulong square, bool IsWhite){
            ulong NorthMoves = NorthFill(square, IsWhite);
            ulong EastMoves = EastFill(square, IsWhite);
            ulong WestMoves = WestFill(square, IsWhite);
            ulong SouthMoves  = SouthFill(square, IsWhite);
            ulong NorthWestMoves = NorthWestFill(square, IsWhite);
            ulong NorthEastMoves = NorthEastFill(square, IsWhite);
            ulong SouthWestMoves = SouthWestFill(square, IsWhite);
            ulong SouthEastMoves  = SouthEastFill(square, IsWhite);
            return NorthMoves | EastMoves | WestMoves | SouthMoves | NorthWestMoves | NorthEastMoves | SouthEastMoves | SouthWestMoves;
        }

        public static ulong GenerateKnightMoves(ulong square, bool IsWhite){
            ulong moves = 0;
            ulong TeamPieces = IsWhite ? GetWhitePieces() : GetBlackPieces();
            if ((square & (EastWall | Rank7 | NorthWall)) == 0 && ((square << 17) & TeamPieces) == 0)
            {
                moves = moves | (square << 17);
            }
            if ((square & (EastWall | NorthWall | FileG)) == 0 && ((square << 10) & TeamPieces) == 0)
            {
                moves = moves | (square << 10);
            }
            if ((square & (WestWall | NorthWall | FileB)) == 0 && ((square << 6) & TeamPieces) == 0)
            {
                moves = moves | (square << 6);
            }
            if ((square & (WestWall | Rank7 | NorthWall)) == 0 && ((square << 15) & TeamPieces) == 0)
            {
                moves = moves | (square << 15);
            }
            if ((square & (WestWall | SouthWall | Rank2)) == 0 && ((square >> 17) & TeamPieces) == 0)
            {
                moves = moves | (square >> 17);
            }
            if ((square & (WestWall | Rank2 | SouthWall)) == 0 && ((square >> 15) & TeamPieces) == 0)
            {
                moves = moves | (square >> 15);
            }
            if ((square & (WestWall | FileB | SouthWall)) == 0 && ((square >> 10) & TeamPieces) == 0)
            {
                moves = moves | (square >> 10);
            }
            if ((square & (SouthWall | EastWall | FileG)) == 0 && ((square >> 6) & TeamPieces) == 0)
            {
                moves = moves | (square >> 6);
            }
            return moves;
        }

        public static bool IsAttacked(ulong square, bool IsWhite){
            ulong NorthMoves = NorthFill(square, IsWhite);
            ulong EastMoves = EastFill(square, IsWhite);
            ulong WestMoves = WestFill(square, IsWhite);
            ulong SouthMoves  = SouthFill(square, IsWhite);
            ulong NorthWestMoves = NorthWestFill(square, IsWhite);
            ulong NorthEastMoves = NorthEastFill(square, IsWhite);
            ulong SouthWestMoves = SouthWestFill(square, IsWhite);
            ulong SouthEastMoves  = SouthEastFill(square, IsWhite);
            ulong HVmoves = NorthMoves | EastMoves | WestMoves | SouthMoves;
            ulong Dmoves = NorthWestMoves | SouthWestMoves | SouthEastMoves | NorthEastMoves;

            if( IsWhite ? (Dmoves & BlackBishops) != 0 || (Dmoves & BlackQueen) != 0 : (Dmoves & WhiteBishops) != 0 || (Dmoves & WhiteQueen) != 0)
            {
                return true;
            }

            if( IsWhite ? (HVmoves & BlackRooks) != 0 || (HVmoves & BlackQueen) != 0 : (HVmoves & WhiteRooks) != 0 || (HVmoves & WhiteQueen) != 0)
            {
                return true;
            }

            ulong KnightMoves = GenerateKnightMoves(square, IsWhite);
            if ( IsWhite ? (KnightMoves & BlackKnights) != 0 : (KnightMoves & WhiteKnights) != 0)
            {
                return true;
            }

            ulong PawnMoves = GeneratePawnMoves(square, IsWhite, true);
            if ( IsWhite ? (PawnMoves & BlackPawns) != 0 : (PawnMoves & WhitePawns) != 0)
            {
                return true;
            }

            ulong KingMoves = GenerateKingMoves(square, IsWhite, false);
            if ( IsWhite ? (KingMoves & BlackKing) != 0 : (PawnMoves & WhiteKing) != 0)
            {
                return true;
            }

            return false;
        }

        public static ulong GenerateKingMoves(ulong square, bool IsWhite, bool CheckAttack){
            ulong moves = 0;
            ulong TeamPieces = IsWhite ? GetWhitePieces() : GetBlackPieces();
            if(CheckAttack)
            {
                if(IsAttacked(square,IsWhite))
                {
                    return 0;
                }
                return 0xffffffffffffffff;
            }
            if ((square & NorthWall) == 0){
                moves = moves | (square << 9) | (square << 8) | (square << 7);
            }
            if ((square & EastWall) == 0){
            
                moves = moves | (square << 1);
            
            }
            if ((square & SouthWall) == 0){
                moves = moves | (square >> 9) | (square >> 8) | (square >> 7);
            }
            if ((square & WestWall) == 0){
                moves = moves | (square >> 1);
            }
            return moves ^ TeamPieces;
        }
    }
}
