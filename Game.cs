namespace C_Learn
{
    internal class Game
    {
        static Game game = null!;
        ConsoleControler consoleControler;
        MainMenu MainMenu;
        MainGameScene MainGameScene;

        public Game()
        {
            consoleControler = new ConsoleControler();
            MainMenu = new MainMenu();
            MainGameScene = new MainGameScene();
        }

        static void Main(string[] args)
        {
            game = new Game();
            EventBroadcaster.Instance.AddEventListener(EventName.StartGame, game.StartNewGame);
            game.EnterScence(E_ScenceType.Menu);
        }

        #region GameLogic

        private void EnterScence(E_ScenceType type)
        {
            switch (type)
            {
                case E_ScenceType.Menu:
                    ToMainMenu();
                    break;
                case E_ScenceType.Game:
                    StartNewGame();
                    break;
                case E_ScenceType.EndGame:
                    EndGame();
                    break;
            }
        }

        private void ToMainMenu()
        {
            Console.Clear();
            game.consoleControler.SetDisplaySize();
            game.consoleControler.SetCursor(false);
            game.consoleControler.GetDisplaySize(out int width, out int height);
            game.MainMenu.SetTitle(width, height);
        }

        private void StartNewGame()
        {
            game.consoleControler.GetDisplaySize(out int width, out int height);
            game.MainGameScene.ReSetPlayer();
            game.MainGameScene.DrawGameScene(width, height);
        }

        private void EndGame()
        {
            Console.Clear();
        }
        #endregion

    }
}
