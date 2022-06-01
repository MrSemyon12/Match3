using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Match3.Graphics;

namespace Match3.Entities
{
    public class TimeBoard : IGameEntity
    {
        private const int _START_TIME_S = 60;

        private const int _SIZE = 160;

        private const int _X = 20;

        private const int _Y = 5;

        private Rectangle _timePlate;

        private Vector2 _timePosition;

        private static TimeBoard? s_instance = null;       

        private Sprite _sprite;        

        private SpriteFont _spriteFont;

        private static int _timePlateValue;

        private int _FPS;

        private GameStage _gameStage;

        protected TimeBoard(Texture2D texture, SpriteFont font, GameStage stage)
        {
            _timePlate = new Rectangle(_X, _Y, _SIZE, _SIZE);
            _gameStage = stage;
            _sprite = new Sprite(texture);
            _spriteFont = font;
            _timePlateValue = _START_TIME_S;
            _FPS = 60;
            _timePosition = new Vector2(_X + 70 - _timePlateValue.ToString().Length * 25, _Y + 15);
        }

        public static TimeBoard Initialize(Texture2D texture, SpriteFont font, GameStage stage)
        {
            if (s_instance == null)
            {
                s_instance = new TimeBoard(texture, font, stage);
            }
            return s_instance;
        }

        private void CheckRunning()
        {
            if (_timePlateValue == 0)
            {
                _gameStage.SetStage(Stage.GameOver);
                _timePlateValue = _START_TIME_S;
            }            
        }        

        public void Update()
        {
            _FPS--;
            if (_FPS == 0)
            {
                _FPS = 60;
                _timePlateValue--;
                _timePosition.X = _X + 70 - _timePlateValue.ToString().Length * 25;
                _timePosition.Y = _Y + 15;
            }
            CheckRunning();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _timePlate, Color.White);            
            spriteBatch.DrawString(_spriteFont, _timePlateValue.ToString(), _timePosition, Color.Red);
        }        
    }
}
