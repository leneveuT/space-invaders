using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Enemy
    {
        public Texture2D EnemyTexture;
        public Vector2 Position;
        public bool Active;
        public float Speed;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            EnemyTexture = texture;
            Position = position;
            Active = true;
            Speed = 50f;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EnemyTexture, Position, Color.White);
        }

        public int Width
        {
            get { return EnemyTexture.Width; }
        }

        public int Height
        {
            get { return EnemyTexture.Height; }
        }
    }
}
