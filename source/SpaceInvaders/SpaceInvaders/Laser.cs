using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Laser
    {
        public Texture2D LaserTexture;
        public Vector2 Position;
        public float Speed = 500f;
        public bool Active;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            LaserTexture = texture;
            Position = position;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LaserTexture, Position, Color.White);
        }

        public int Width
        {
            get { return LaserTexture.Width; }
        }

        public int Height
        {
            get { return LaserTexture.Height; }
        }
    }
}
