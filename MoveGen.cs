using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Xps.Serialization;
using System.Linq;

namespace ChessEngine.Game
{
    public class Moves
    {

        public static ChessGame Board = new ChessGame();

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
                    position = Board.IndexFlipper(position);
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

        public static void PrintAll()
        {
            Board.PrintBitBoard(WhitePawns, "White Pawns");
            Board.PrintBitBoard(WhiteKnights, "White Knights");
            Board.PrintBitBoard(WhiteBishops, "White Bishops");
            Board.PrintBitBoard(WhiteRooks, "White Rooks");
            Board.PrintBitBoard(WhiteQueen, "White Queen");
            Board.PrintBitBoard(WhiteKing, "White King");

            Board.PrintBitBoard(BlackPawns, "Black Pawns");
            Board.PrintBitBoard(BlackKnights, "Black Knights");
            Board.PrintBitBoard(BlackBishops, "Black Bishops");
            Board.PrintBitBoard(BlackRooks, "Black Rooks");
            Board.PrintBitBoard(BlackQueen, "Black Queen");
            Board.PrintBitBoard(BlackKing, "Black King");
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

        public static ulong SortMove(ulong square, bool WhiteTurn, ulong Move, bool WhiteKingCheck, bool BlackKingCheck)
        {
            ulong moves = 0UL;
            if ((square & GetWhitePieces()) != 0 && WhiteTurn)
            {
                if((square & WhiteKing) != 0) moves = GenerateKingMoves(square, true, Move, true);
                if((square & WhitePawns) != 0) moves = GeneratePawnMoves(square, true, false);
                if((square & WhiteRooks) != 0) moves = GenerateRookMoves(square, true);
                if((square & WhiteKnights) != 0) moves = GenerateKnightMoves(square, true);
                if((square & WhiteBishops) != 0) moves = GenerateBishopMoves(square, true);
                if((square & WhiteQueen) != 0) moves = GenerateQueenMoves(square, true);
                if(WhiteKingCheck == true)
                {;
                    moves = moves & GetCheckMask(WhiteKing,true);
                }
                return moves;
            }
            else if(!WhiteTurn)
            {
                if((square & BlackKing) != 0) moves = GenerateKingMoves(square, false, Move, true);
                if((square & BlackPawns) != 0) moves = GeneratePawnMoves(square, false, false);
                if((square & BlackRooks) != 0) moves = GenerateRookMoves(square, false);
                if((square & BlackKnights) != 0) moves = GenerateKnightMoves(square, false);
                if((square & BlackBishops) != 0) moves = GenerateBishopMoves(square, false);
                if((square & BlackQueen) != 0) moves = GenerateQueenMoves(square, false);
                if(BlackKingCheck == true)
                {
                    moves = moves & GetCheckMask(BlackKing,false);
                }
                return moves;
            }
            return moves;
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
                moves |= SinglePawnPush;
            }
            ulong DoublePawnPush = isWhite ? square << 16 : square >> 16;
            if (((SinglePawnPush | DoublePawnPush) & GetAllPieces()) == 0 && (isWhite ? (square & Rank2) != 0 : (square & Rank7) != 0))
            {
                moves |= DoublePawnPush;
            }

            ulong LeftTake = isWhite ? square << 7 : square >> 9;
            if ((LeftTake & (isWhite ? GetBlackPieces() : GetWhitePieces())) != 0 && (square & FileA) == 0)
            {
                moves |= LeftTake;
            }
            ulong RightTake = isWhite ? square << 9 : square >> 7;
            if ((RightTake & (isWhite ? GetBlackPieces() : GetWhitePieces())) != 0 && (square & FileH) == 0)
            {
                moves |= RightTake;
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
            if ((square & Rank8) != 0)
            {
                return moves;
            }
            ulong North = square << 8;
            while (true)
            {
                if ((North & TeamPieces) == 0)
                {
                    if ((North & OpposingPieces) != 0 || (North & Rank8) != 0)
                    {
                        moves |= North;
                        return moves;
                    }
                    else
                    {
                        moves |= North;
                        North <<= 8;
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
            if ((square & FileH) != 0)
            {
                return moves;
            }
            ulong East = square << 1;
            while (true)
            {
                if ((East & TeamPieces) == 0)
                {
                    if ((East & OpposingPieces) != 0 || (East & FileH) != 0)
                    {
                        moves |= East;
                        return moves;
                    }
                    else
                    {
                        moves |= East;
                        East <<= 1;
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
            if ((square & FileA) != 0)
            {
                return moves;
            }
            ulong West = square >> 1;
            while (true)
            {
                if ((West & TeamPieces) == 0)
                {
                    if ((West & OpposingPieces) != 0 || (West & FileA) != 0)
                    {
                        moves |= West;
                        return moves;
                    }
                    else
                    {
                        moves |= West;
                        West >>= 1;
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
            if ((square & Rank1) != 0)
            {
                return moves;
            }
            ulong South = square >> 8;
            while (true)
            {
                if ((South & TeamPieces) == 0)
                {
                    if ((South & OpposingPieces) != 0 || (South & Rank1) != 0)
                    {
                        moves |= South;
                        return moves;
                    }
                    else
                    {
                        moves |= South;
                        South >>= 8;
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
            if ((square &  (Rank8 | FileA)) != 0)
            {
                return moves;
            }
            ulong NorthWest = square << 7;
            while (true)
            {
                if ((NorthWest & TeamPieces) == 0)
                {
                    if ((NorthWest & OpposingPieces) != 0 || (NorthWest &  (Rank8 | FileA)) != 0)
                    {
                        moves |= NorthWest;
                        return moves;
                    }
                    else
                    {
                        moves |= NorthWest;
                        NorthWest <<= 7;
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
            if ((square &  (Rank8 | FileH)) != 0)
            {
                return moves;
            }
            ulong NorthEast = square << 9;
            while (true)
            {
                if ((NorthEast & TeamPieces) == 0)
                {
                    if ((NorthEast & OpposingPieces) != 0 || (NorthEast &  (Rank8 | FileH)) != 0)
                    {
                        moves |= NorthEast;
                        return moves;
                    }
                    else
                    {
                        moves |= NorthEast;
                        NorthEast <<= 9;
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
            if ((square & (Rank1 | FileA)) != 0)
            {
                return moves;
            }
            ulong SouthWest = square >> 9;
            while (true)
            {
                if ((SouthWest & TeamPieces) == 0)
                {
                    if ((SouthWest & OpposingPieces) != 0 || (SouthWest & (Rank1 | FileA)) != 0)
                    {
                        moves |= SouthWest;
                        return moves;
                    }
                    else
                    {
                        moves |= SouthWest;
                        SouthWest >>= 9;
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
            if ((square & (Rank1 | FileH)) != 0)
            {
                return moves;
            }
            ulong SouthEast = square >> 7;
            while (true)
            {
                if ((SouthEast & TeamPieces) == 0)
                {
                    if ((SouthEast & OpposingPieces) != 0 || (SouthEast & (Rank1 | FileH)) != 0)
                    {
                        moves |= SouthEast;
                        return moves;
                    }
                    else
                    {
                        moves |= SouthEast;
                        SouthEast >>= 7;
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
            if ((square & (FileH | Rank7 | Rank8)) == 0 && ((square << 17) & TeamPieces) == 0)
            {
                moves |= square << 17;
            }
            if ((square & (FileH | Rank8 | FileG)) == 0 && ((square << 10) & TeamPieces) == 0)
            {
                moves |= square << 10;
            }
            if ((square & (FileA | Rank8 | FileB)) == 0 && ((square << 6) & TeamPieces) == 0)
            {
                moves |= square << 6;
            }
            if ((square & (FileA | Rank7 | Rank8)) == 0 && ((square << 15) & TeamPieces) == 0)
            {
                moves |= square << 15;
            }
            if ((square & (FileA | Rank1 | Rank2)) == 0 && ((square >> 17) & TeamPieces) == 0)
            {
                moves |= square >> 17;
            }
            if ((square & (FileA | Rank2 | Rank1)) == 0 && ((square >> 15) & TeamPieces) == 0)
            {
                moves |= square >> 15;
            }
            if ((square & (FileA | FileB | Rank1)) == 0 && ((square >> 10) & TeamPieces) == 0)
            {
                moves |= square >> 10;
            }
            if ((square & (Rank1 | FileH | FileG)) == 0 && ((square >> 6) & TeamPieces) == 0)
            {
                moves |= square >> 6;
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

            ulong KingMoves = GenerateKingMoves(square, IsWhite, 0UL, false);
            if ( IsWhite ? (KingMoves & BlackKing) != 0 : (PawnMoves & WhiteKing) != 0)
            {
                return true;
            }

            return false;
        }

        public static ulong GenerateKingMoves(ulong square, bool IsWhite, ulong Move, bool CheckAttack){
            ulong moves = 0;
            ulong TeamPieces = IsWhite ? GetWhitePieces() : GetBlackPieces();
            if(CheckAttack)
            {
                if(IsAttacked(Move,IsWhite))
                {
                    return 0;
                }
            }
            if ((square & Rank8) == 0){
                moves |= (square << 9) | (square << 8) | (square << 7);
            }
            if ((square & FileH) == 0){
            
                moves |= square << 1;
            
            }
            if ((square & Rank1) == 0){
                moves |= (square >> 9) | (square >> 8) | (square >> 7);
            }
            if ((square & FileA) == 0){
                moves |= square >> 1;
            }
            return moves & ~TeamPieces;
        }

        public static ulong GetCheckMask(ulong square, bool IsWhite)
        {
            ulong CheckMask = 0UL;

            ulong NorthMoves = NorthFill(square, IsWhite);
            ulong EastMoves = EastFill(square, IsWhite);
            ulong WestMoves = WestFill(square, IsWhite);
            ulong SouthMoves = SouthFill(square, IsWhite);
            ulong NorthWestMoves = NorthWestFill(square, IsWhite);
            ulong NorthEastMoves = NorthEastFill(square, IsWhite);
            ulong SouthWestMoves = SouthWestFill(square, IsWhite);
            ulong SouthEastMoves = SouthEastFill(square, IsWhite); 

            ulong Dpieces = IsWhite? BlackQueen | BlackBishops : WhiteBishops | WhiteQueen;
            ulong HVpieces = IsWhite? BlackQueen | BlackRooks : WhiteQueen | WhiteRooks;

            if((NorthMoves & HVpieces) != 0) CheckMask |= NorthMoves;
            if((EastMoves & HVpieces) != 0) CheckMask |= EastMoves;
            if((WestMoves & HVpieces) != 0) CheckMask |= WestMoves;
            if((SouthMoves & HVpieces) != 0) CheckMask |= SouthMoves; 

            if((NorthWestMoves & Dpieces) != 0) CheckMask |= NorthWestMoves;
            if((NorthEastMoves & Dpieces) != 0) CheckMask |= NorthEastMoves;
            if((SouthWestMoves & Dpieces) != 0) CheckMask |= SouthWestMoves;
            if((SouthEastMoves & Dpieces) != 0) CheckMask |= SouthEastMoves;

            ulong Knight = IsWhite? BlackKnights : WhiteKnights;

            if ((square & (FileH | Rank7 | Rank8)) == 0 && ((square << 17) & Knight) != 0){CheckMask |= square << 17;}
            if ((square & (FileH | Rank8 | FileG)) == 0 && ((square << 10) & Knight) != 0){CheckMask |= square << 10;}
            if ((square & (FileA | Rank8 | FileB)) == 0 && ((square << 6) & Knight) != 0){CheckMask |= square << 6;}
            if ((square & (FileA | Rank7 | Rank8)) == 0 && ((square << 15) & Knight) != 0){CheckMask |= square << 15;}
            if ((square & (FileA | Rank1 | Rank2)) == 0 && ((square >> 17) & Knight) != 0){CheckMask |= square >> 17;}
            if ((square & (FileA | Rank2 | Rank1)) == 0 && ((square >> 15) & Knight) != 0){CheckMask |= square >> 15;}
            if ((square & (FileA | FileB | Rank1)) == 0 && ((square >> 10) & Knight) != 0){CheckMask |= square >> 10;}
            if ((square & (Rank1 | FileH | FileG)) == 0 && ((square >> 6) & Knight) != 0){CheckMask |= square >> 6;}

            ulong Pawn = IsWhite? BlackPawns : WhitePawns;

            ulong LeftTake = IsWhite ? square << 9 : square >> 7;
            if ((LeftTake & Pawn) != 0 && (square & FileA) == 0){CheckMask |= LeftTake;}
            ulong RightTake = IsWhite ? square << 7 : square >> 9;
            if ((RightTake & Pawn) != 0 && (square & FileH) == 0){CheckMask |= RightTake;}

            return CheckMask;
        }
    }
}
