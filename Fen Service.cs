namespace ChessEngine.Game
{
    public static class FenService
    {
        public static string FenTo64(string fen)
        {
            string newFen = "";
            for (int i = 0; i < fen.Length; i++)
            {
                if (char.IsNumber(fen[i]))
                {
                    int emptySquares = int.Parse(fen[i].ToString());
                    for (int j = 0; j < emptySquares; j++)
                    {
                        newFen += '1'; 
                    }
                }
                else if (fen[i] != '/')
                {
                    newFen += fen[i];
                }
            }
            return newFen;
        }

        public static string FenReader(int index, string fen)
        {
            char piece;
            if (char.IsLetter(fen[index]))
            {
                piece = fen[index];
            }
            else
            {
                return "1";
            }

            string basePath = "C:/users/olive/Documents/ChessEngine/images/";
            return piece switch
            {
                'r' => basePath + "Chess_rdt60.png",
                'n' => basePath + "Chess_ndt60.png",
                'b' => basePath + "Chess_bdt60.png",
                'q' => basePath + "Chess_qdt60.png",
                'k' => basePath + "Chess_kdt60.png",
                'p' => basePath + "Chess_pdt60.png",
                'R' => basePath + "Chess_rlt60.png",
                'N' => basePath + "Chess_nlt60.png",
                'B' => basePath + "Chess_blt60.png",
                'Q' => basePath + "Chess_qlt60.png",
                'K' => basePath + "Chess_klt60.png",
                'P' => basePath + "Chess_plt60.png",
                _ => throw new ArgumentException("Invalid piece type"),
            };
        }
    }
}