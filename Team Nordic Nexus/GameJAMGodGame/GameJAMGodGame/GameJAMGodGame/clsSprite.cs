using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2

namespace GameJAMGodGame
{
    class clsSprite
    {
        public Texture2D texture { get; set; }      //  sprite texture, read-only property
        public Vector2 position { get; set; }  //  sprite position on screen
        public Vector2 size { get; set; }      //  sprite size in pixels
        public Vector2 scale { get; set; }    // the scale size of the sprite
        public float rotation { get; set; }    // the scale size of the sprite
        public Vector2 velocity { get; set; }
        private Vector2 screenSize { get; set; }


        public clsSprite(Texture2D newTexture, Vector2 newPosition, Vector2 newSize, Vector2 newscale, float newrotation,int ScreenWidth, int ScreenHeight)
        {
            texture = newTexture;
            position = newPosition;
            size = newSize;
            scale = newscale;
            rotation = newrotation;
            screenSize = new Vector2(ScreenWidth, ScreenHeight);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White,rotation, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public Rectangle BoundingBox
        {
            get{
                
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    Convert.ToInt32(texture.Width *scale.X),
                    Convert.ToInt32(texture.Height *scale.Y));
            }
        }


        public void Move()
        {
            //if (position.X + size.X + velocity.X > screenSize.X)
            //    velocity = new Vector2(-velocity.X, velocity.Y);
            //if (position.Y + size.Y + velocity.Y > screenSize.Y)
            //    velocity = new Vector2(velocity.X, -velocity.Y);
            //if (position.X + velocity.X < 0)
            //    velocity = new Vector2(-velocity.X, velocity.Y);
            //if (position.Y + velocity.Y < 0)
            //    velocity = new Vector2(velocity.X, -velocity.Y);

            position += velocity;
        }


    }
}
