using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Match3.Graphics;

namespace Match3.Entities
{
    public class Tile : ICloneable
    {
        private Sprite? _sprite;

        private Rectangle _tilePlate;

        private Color _color;

        private bool _isSelected;
                
        public Tile(Sprite sprite, int x, int y, int wigth, int height)
        {
            _sprite = sprite;
            _tilePlate = new Rectangle(x, y, wigth, height);
            _color = Color.White;
            _isSelected = false;            
        }

        public Sprite? GetSprite()
        {
            return _sprite;
        }

        public void SetSprite(Sprite? sprite)
        {
            _sprite = sprite;
        }

        public bool Clicked()
        {
            if (_isSelected)
            {
                Deselect();
                return false;
            }
            else
            {
                Select();
                return true;
            }
        }

        public void Select()
        {
            _isSelected = true;
            _color = Color.Gray;
        }

        public void Deselect()
        {
            _isSelected = false;
            _color = Color.White;
        }

        public void SwapSprites(Tile tile)
        {
            Tile tmp = (Tile)tile.Clone();
            tile.SetSprite(_sprite);
            _sprite = tmp.GetSprite();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_sprite != null)
                _sprite.Draw(spriteBatch, _tilePlate, _color);
        }        
    }
}
