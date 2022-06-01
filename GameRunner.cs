using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Match3.Entities;

namespace Match3
{
    public class GameRunner : Game
    {
        private const int WINDOW_HEIGHT = 800;
        private const int WINDOW_WIDTH = 640;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch; 

        private Texture2D _backgroundImg;
        private List<Texture2D> _charactersImg; 
        private List<Texture2D> _menuImg;        
        private Texture2D _timerImg;
        private Texture2D _scoreImg;
        private SpriteFont _spriteFont;        

        private GameMenu _gameMenu;        
        private TimeBoard _timer;
        private ScoreBoard _score;
        private Grid _grid;

        private Vector2 _backgroundOffset;
        private GameStage _gameStage;
        private MouseState _mouse;

        public GameRunner() : base()
        {
            _graphics = new GraphicsDeviceManager(this);             
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            IsMouseVisible = true;
            _backgroundOffset = new Vector2(0, 0);
            _gameStage = new GameStage();            
        }

        protected override void Initialize()
        {                       
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {        
            _spriteBatch = new SpriteBatch(GraphicsDevice);                
            _backgroundImg = Content.Load<Texture2D>("Images/background");
            _charactersImg = new List<Texture2D> {
                Content.Load<Texture2D>("Images/character_1"),
                Content.Load<Texture2D>("Images/character_2"),
                //Content.Load<Texture2D>("Images/character_3"),
                Content.Load<Texture2D>("Images/character_4"),
                Content.Load<Texture2D>("Images/character_5"),
                Content.Load<Texture2D>("Images/character_6"),
                //Content.Load<Texture2D>("Images/character_7")
            };
            _menuImg = new List<Texture2D> {
                Content.Load<Texture2D>("Images/button_up"),
                Content.Load<Texture2D>("Images/button_down"),
                Content.Load<Texture2D>("Images/note"),
                Content.Load<Texture2D>("Images/game_over")
        };                     
            _timerImg = Content.Load<Texture2D>("Images/timer");
            _scoreImg = Content.Load<Texture2D>("Images/score");
            _spriteFont = Content.Load<SpriteFont>("Fonts/MyFont");           

                       
            _timer = TimeBoard.Initialize(_timerImg, _spriteFont, _gameStage);
            _score = ScoreBoard.Initialize(_scoreImg, _spriteFont);
            _grid = Grid.Initialize(_charactersImg, _score, _mouse);
            _gameMenu = GameMenu.Initialize(_gameStage, _menuImg, _spriteFont, _score, _mouse, _grid);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_gameStage.GetStage())
            {
                case Stage.Menu:
                    _gameMenu.Update();
                    break;

                case Stage.Game:
                    _timer.Update();
                    _grid.Update();
                    break;

                case Stage.GameOver:
                    _gameMenu.Update();
                    break;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_backgroundImg, _backgroundOffset, Color.White);

            switch (_gameStage.GetStage())
            {
                case Stage.Menu:
                    _gameMenu.Draw(_spriteBatch);
                    break;

                case Stage.Game:
                    _grid.Draw(_spriteBatch);
                    _timer.Draw(_spriteBatch);
                    _score.Draw(_spriteBatch);
                    break;

                case Stage.GameOver:
                    _gameMenu.Draw(_spriteBatch);                    
                    break;
            }           

            _spriteBatch.End();            
            
            base.Draw(gameTime);
        }
    }
}
