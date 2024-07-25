using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ChessEngine.Game;


namespace ChessEngine.WindowsUI
{
    public partial class MainWindow : Window
    {

        private List<Button> Tiles = new List<Button>();
        Random random = new Random();
        ChessGame Board = new ChessGame();
        Moves moves = new Moves();
        private Button? CurrentTile = null;
        private object? CurrentTileContent = null;

        Dictionary<int, string> NumberToAlgebraicNotation = new Dictionary<int, string>
        {
            { 0, "h1" }, { 1, "g1" }, { 2, "f1" }, { 3, "e1" }, { 4, "d1" }, { 5, "c1" }, { 6, "b1" }, { 7, "a1" },
            { 8, "h2" }, { 9, "g2" }, { 10, "f2" }, { 11, "e2" }, { 12, "d2" }, { 13, "c2" }, { 14, "b2" }, { 15, "a2" },
            { 16, "h3" }, { 17, "g3" }, { 18, "f3" }, { 19, "e3" }, { 20, "d3" }, { 21, "c3" }, { 22, "b3" }, { 23, "a3" },
            { 24, "h4" }, { 25, "g4" }, { 26, "f4" }, { 27, "e4" }, { 28, "d4" }, { 29, "c4" }, { 30, "b4" }, { 31, "a4" },
            { 32, "h5" }, { 33, "g5" }, { 34, "f5" }, { 35, "e5" }, { 36, "d5" }, { 37, "c5" }, { 38, "b5" }, { 39, "a5" },
            { 40, "h6" }, { 41, "g6" }, { 42, "f6" }, { 43, "e6" }, { 44, "d6" }, { 45, "c6" }, { 46, "b6" }, { 47, "a6" },
            { 48, "h7" }, { 49, "g7" }, { 50, "f7" }, { 51, "e7" }, { 52, "d7" }, { 53, "c7" }, { 54, "b7" }, { 55, "a7" },
            { 56, "h8" }, { 57, "g8" }, { 58, "f8" }, { 59, "e8" }, { 60, "d8" }, { 61, "c8" }, { 62, "b8" }, { 63, "a8" }
        };
        

        public MainWindow()
        {
            InitializeComponent();
            CreateDynamicElements();
        }

        private void FenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FenComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                Moves.resetBitboards();
                string fen = selectedItem.Tag.ToString();
                SetupBoard(fen);
                Moves.SortBitboards(FenTo64(fen));
            }
        }

        private void CreateDynamicElements()
        {
            int rows = 8;
            int cols = 8;
            int count = 64;

            Grid chessboardGrid = new Grid();
            for (int i = 0; i < rows; i++)
            {
                chessboardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < cols; j++)
            {
                chessboardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    count -=1;
                    Button tile = new Button
                    {
                        Width = 100,
                        Height = 100,
                        Tag = $"{count}",
                        Background = (row + col) % 2 == 0 
                            ? Brushes.White
                            : new SolidColorBrush(GetRandomColor())
                    };
                    tile.Click += Tile_Click;
                    Grid.SetRow(tile, row);
                    Grid.SetColumn(tile, col);
                    chessboardGrid.Children.Add(tile);
                    Tiles.Add(tile);
                }
            }
            Chessboard.Children.Add(chessboardGrid);
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedTile = (Button)sender;
            if (CurrentTile == null && ClickedTile.Content != null)
            {
                CurrentTile = ClickedTile;
                CurrentTileContent = CurrentTile.Content;
                CurrentTile.BorderBrush = Brushes.Gold;
                CurrentTile.BorderThickness = new Thickness(5);
            }
            else if (CurrentTile != null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                if (ClickedTile == CurrentTile)
                {
                    CurrentTile.BorderThickness = new Thickness(0);
                    CurrentTile = null;
                    CurrentTileContent = null;
                }
                else if(Board.CanMove(Convert.ToInt32(CurrentTile.Tag), Convert.ToInt32(ClickedTile.Tag)))
                {
                    Console.WriteLine($"Move: {NumberToAlgebraicNotation[Convert.ToInt32(CurrentTile.Tag)]}:{NumberToAlgebraicNotation[Convert.ToInt32(ClickedTile.Tag)]}");
                    ClickedTile.Content = CurrentTileContent;
                    CurrentTile.Content = null;
                    CurrentTile.BorderThickness = new Thickness(0);
                    CurrentTile = null;
                    CurrentTileContent = null;
                    Board.IncrementTurn();
                }
                watch.Stop();
                Console.WriteLine($"Time Taken to check move: {watch.ElapsedMilliseconds}ms");
            }
        }

        public ulong NumberToBitBoard(int number)
        {
            ulong BitBoard = 0;
            ulong MaskedBitBoard = (ulong)1 << number;
            BitBoard |= MaskedBitBoard;
            return BitBoard;
        }



        private Color GetRandomColor()
        {
            byte r, g, b, a;
            r = (byte)random.Next(100,190);
            g = (byte)random.Next(100,190);
            b = (byte)random.Next(100,190);
            a = (byte)random.Next(190,256);
            return Color.FromArgb(a, r, g, b);
        }
        private Button? IndexToButton(int index)
        {
            if (index >= 0 && index < Tiles.Count)
            {
                return Tiles[index];
            }
            return null;
        }

        private void SetupBoard(string Fen)
        {
            string FormattedFen = FenTo64(Fen);
            for (int i = 0; i < Tiles.Count; i++)
            {
                Button? tile = IndexToButton(i);
                tile.Content = null;
                Image image = new Image();
                string imagePath = FenReader(i, FormattedFen);
                if (imagePath != "1" && tile != null)
                {
                    image.Source = new BitmapImage(new Uri(imagePath));
                    image.Stretch = Stretch.Fill;
                    tile.Content = image;
                }
            }
        }


        private string FenReader(int index, string Fen)
        {
            char piece;
            if (char.IsLetter(Fen[index])){
                piece = Fen[index];
            }
            else{
                return 1.ToString();
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

        private static string FenTo64(string fen)
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
                else if (fen[i] == '/')
                {
                    continue;
                }
                else
                {
                    newFen += fen[i];
                }
            }
            return newFen;
        }

    }
}