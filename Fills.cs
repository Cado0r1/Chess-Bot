using static ChessEngine.Game.Moves;
using static ChessEngine.Game.BitboardOperations;

namespace ChessEngine.Game
{
	public class Fills
	{
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
	}
}