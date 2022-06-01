using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Match3.Graphics;

namespace Match3.Entities
{
    public class ScoreBoard : IGameEntity
    {
        private const int _WIDTH = 440;

        private const int _HEIGHT = 100;        

        private int _X = 200;

        private int _Y = 35;

        private Rectangle _scorePlate;

        private Vector2 _scorePosition;

        private static ScoreBoard? s_instance;

        private Sprite _sprite;

        private SpriteFont _spriteFont;

        private int _counter;

        protected ScoreBoard(Texture2D texture, SpriteFont font)
        {            
            _sprite = new Sprite(texture);
            _spriteFont = font;
            _scorePlate = new Rectangle(_X, _Y, _WIDTH, _HEIGHT);
            _scorePosition = new Vector2(_X + 15, _Y - 15);
            _counter = 0;
        }

        public static ScoreBoard Initialize(Texture2D texture, SpriteFont font)
        {
            if (s_instance == null)
            {
                s_instance = new ScoreBoard(texture, font);
            }
            return s_instance;
        }  
        
        public int GetPoints()
        {
            return _counter;
        }

        public void DropPoints()
        {
            _counter = 0;
        }

        public void AddPoints(int points)
        {
            _counter += points;
        }

        public void Update()
        {
            // counter updates from GameGrid
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _scorePlate, Color.White);
            string score = _counter.ToString();
            while (score.Length < 7)
                score = "0" + score;            
            spriteBatch.DrawString(_spriteFont, score, _scorePosition, Color.White);            
        }
    }
}
