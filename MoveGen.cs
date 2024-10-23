using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Xps.Serialization;
using System.Linq;
using static ChessEngine.Game.ChessGame;
using static ChessEngine.Game.FenService;
using static ChessEngine.Game.BitboardOperations;
using static ChessEngine.Game.Fills;

namespace ChessEngine.Game
{
    public class Moves
    {

        public static ulong SortMove(ulong square, bool WhiteTurn, ulong Move, bool WhiteKingCheck, bool BlackKingCheck)
        {
            ulong moves = 0UL;
            if ((square & GetWhitePieces()) != 0 && WhiteTurn)
            {
                if((square & WhiteKing) != 0) moves = GenerateKingMoves(square, true, Move, true);
                if((square & WhitePawns) != 0) moves = GeneratePawnMoves(square, true, false, Move);
                if((square & WhiteRooks) != 0) moves = GenerateRookMoves(square, true);
                if((square & WhiteKnights) != 0) moves = GenerateKnightMoves(square, true);
                if((square & WhiteBishops) != 0) moves = GenerateBishopMoves(square, true);
                if((square & WhiteQueen) != 0) moves = GenerateQueenMoves(square, true);
                if(WhiteKingCheck == true && (square & WhiteKing) == 0)
                {
                    moves &= GetCheckMask(WhiteKing,true);
                }
                return moves;
            }
            else if(!WhiteTurn)
            {
                if((square & BlackKing) != 0) moves = GenerateKingMoves(square, false, Move, true);
                if((square & BlackPawns) != 0) moves = GeneratePawnMoves(square, false, false, Move);
                if((square & BlackRooks) != 0) moves = GenerateRookMoves(square, false);
                if((square & BlackKnights) != 0) moves = GenerateKnightMoves(square, false);
                if((square & BlackBishops) != 0) moves = GenerateBishopMoves(square, false);
                if((square & BlackQueen) != 0) moves = GenerateQueenMoves(square, false);
                if(BlackKingCheck == true && (square & BlackKing) == 0)
                {
                    moves &= GetCheckMask(BlackKing,false);
                }
                return moves;
            }
            return moves;
        }

        public static ulong GeneratePawnMoves(ulong square, bool isWhite, bool OnlyReturnAttack, ulong Move, bool OnlyReturnPush = false, bool CheckPieces = true)
        {
            PrintBitBoard(square,"square");
            ulong moves = 0;
            ulong SinglePawnPush = isWhite ? square << 8 : square >> 8;
            PrintBitBoard(SinglePawnPush,"spp");
            if (!CheckPieces || (SinglePawnPush & GetAllPieces()) == 0)
            {
                moves |= SinglePawnPush;
            }
            ulong DoublePawnPush = isWhite ? square << 16 : square >> 16;
            if (!CheckPieces || ((SinglePawnPush | DoublePawnPush) & GetAllPieces()) == 0 && (isWhite ? (square & Rank2) != 0 : (square & Rank7) != 0))
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
            else if(OnlyReturnPush)
            {
                return  moves & ~(LeftTake | RightTake);
            }
            else
            {
                return moves;
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

        public static bool IsAttacked(ulong square, bool IsWhite, bool KingInc = true, bool PawnAttack = true){
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
            PrintBitBoard(square,"square");
            if(PawnAttack)
            {
                ulong PawnMoves = GeneratePawnMoves(square, IsWhite, true, 0UL); 
                PrintBitBoard(PawnMoves, "pm");
                if ( IsWhite ? (PawnMoves & BlackPawns) != 0 : (PawnMoves & WhitePawns) != 0)
                {
                    return true;
                }
            }
            else{
                ulong PawnMoves = GeneratePawnMoves(square, IsWhite, false, 0UL, true, false); 
                PrintBitBoard(PawnMoves, "pm");
                if ( IsWhite ? (PawnMoves & BlackPawns) != 0 : (PawnMoves & WhitePawns) != 0)
                {
                    return true;
                }
            }
            if ( KingInc == true)
            {
                ulong KingMoves = GenerateKingMoves(square, IsWhite, 0UL, false);
                if ( IsWhite ? (KingMoves & BlackKing) != 0 : (KingMoves & WhiteKing) != 0)
                {
                    return true;
                }

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

        public static ulong GetCheckPiece(ulong CheckMask,bool IsWhite)
        {
            ulong CheckPiece = !IsWhite? (CheckMask & GetWhitePieces()) : (CheckMask & GetBlackPieces());
            return CheckPiece;
        }
    }
}
