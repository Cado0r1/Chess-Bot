using System.Windows;
using ChessEngine.WindowsUI;
using ChessEngine.Game;
using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("");
            Console.WriteLine("Ollie's Chess Engine");
            Console.WriteLine("");
            ChessGame Board = new ChessGame();
            Moves moves= new Moves();
            Board.RunMainWindow();
        }
    }
}
