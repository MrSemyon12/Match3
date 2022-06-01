using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Entities
{
    internal interface IGameEntity
    {       
        void Update();

        void Draw(SpriteBatch spriteBatch);
    }
}
