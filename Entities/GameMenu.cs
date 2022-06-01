using Match3.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Entities
{
    public class GameMenu : IGameEntity
    {
        private const int _WIDTH = 400;

        private const int _HEIGHT = 200;        

        private const int _X = 120;

        private const int _Y = 400;

        private static GameMenu? s_instance;

        private GameStage _gameStage;

        private List<Sprite> _buttonsSprites;

        private SpriteFont _spriteFont;

        private Rectangle _buttonPlate;

        private Rectangle _label1Position;

        private Rectangle _label2Position;

        private List<Sprite> _gameMenuSprite;

        private ScoreBoard _gameScore;

        private Vector2 _scorePosition;

        private bool _isButtonTriggered = false;

        private MouseState _mousePrev;

        private Grid _grid;

        protected GameMenu(GameStage gameStage, List<Texture2D> textures, SpriteFont font, ScoreBoard scoreBoard, MouseState mouse, Grid grid)
        {
            _gameStage = gameStage;
            _buttonPlate = new Rectangle(_X, _Y, _WIDTH, _HEIGHT);
            _label1Position = new Rectangle(50, 200, textures[2].Width / 2, textures[2].Height / 2);
            _label2Position = new Rectangle(0, 0, textures[3].Width, textures[3].Height);
            _spriteFont = font;
            _mousePrev = mouse;
            _grid = grid;
            _buttonsSprites = new List<Sprite> {
                new Sprite(textures[0]),
                new Sprite(textures[1]),
            };
            _gameMenuSprite = new List<Sprite> {
                new Sprite(textures[2]),
                new Sprite(textures[3]),
            };
            _gameScore = scoreBoard;
            _scorePosition = new Vector2(200, 0);            
        }

        public static GameMenu Initialize(GameStage gameStage, List<Texture2D> textures, SpriteFont font, ScoreBoard scoreBoard, MouseState mouse, Grid grid)
        {
            if (s_instance == null)
            {
                s_instance = new GameMenu(gameStage, textures, font, scoreBoard, mouse, grid);
            }
            return s_instance;
        }       

        public void Update()
        {
            MouseState mouseCurr = Mouse.GetState();

            if (mouseCurr.X > _X + 10 && mouseCurr.X < _X + _WIDTH - 10 && mouseCurr.Y > _Y + 15 && mouseCurr.Y < _Y + _HEIGHT - 15)
            {
                _isButtonTriggered = true;

                if (_mousePrev.LeftButton == ButtonState.Released && mouseCurr.LeftButton == ButtonState.Pressed)
                {
                    if (_gameStage.GetStage() == Stage.Menu)
                    {
                        _gameStage.SetStage(Stage.Game);
                    }
                    else if (_gameStage.GetStage() == Stage.GameOver)
                    {
                        _gameStage.SetStage(Stage.Menu);                        
                        _gameScore.DropPoints();
                        _grid.FillRightWay();
                    }
                    
                    _isButtonTriggered = false;
                }
            }
            else
            {
                _isButtonTriggered = false;
            }
            _mousePrev = mouseCurr;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_gameStage.GetStage() == Stage.Menu)
            {
                _gameMenuSprite[0].Draw(spriteBatch, _label1Position, Color.White);
            }
            else if (_gameStage.GetStage() == Stage.GameOver)
            {
                _gameMenuSprite[1].Draw(spriteBatch, _label2Position, Color.White);                              
                spriteBatch.DrawString(_spriteFont, _gameScore.GetPoints().ToString(), _scorePosition, Color.OrangeRed);
            }

            if (_isButtonTriggered)
            {
                _buttonsSprites[1].Draw(spriteBatch, _buttonPlate, Color.White);
            }
            else
            {
                _buttonsSprites[0].Draw(spriteBatch, _buttonPlate, Color.White);
            }                       
        }
    }
}
