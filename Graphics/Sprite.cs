using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Graphics
{
    public class Sprite
    {
        private Texture2D _texture;      

        public Sprite(Texture2D texture)
        {
            _texture = texture;            
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {            
            spriteBatch.Draw(_texture, rectangle, color);            
        }
    }
}
