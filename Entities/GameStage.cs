
namespace Match3.Entities
{
    public enum Stage
    {
        Menu, Game, GameOver
    }

    public class GameStage
    {
        private Stage _currentStage;

        public GameStage(Stage stage = Stage.Menu)
        {
            _currentStage = stage;
        }

        public void SetStage(Stage stage)
        {
            _currentStage = stage;
        }

        public Stage GetStage()
        {
            return _currentStage;
        }
    }
}
