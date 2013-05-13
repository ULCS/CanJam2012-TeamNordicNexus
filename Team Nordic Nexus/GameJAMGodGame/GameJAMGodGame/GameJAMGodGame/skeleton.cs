using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D

namespace GameJAMGodGame
{
    class skeleton
    {
        public Vector2 head { get; set; }
        public Vector2 shoulderCenter { get; set; }
        public Vector2 shoulderRight { get; set; }
        public Vector2 shoulderLeft { get; set; }
        public Vector2 elbowRight { get; set; }
        public Vector2 elbowLeft { get; set; }
        public Vector2 wristRight { get; set; }
        public Vector2 wristLeft { get; set; }
        public Vector2 handRight { get; set; }
        public Vector2 handLeft { get; set; }
        public Vector2 spine { get; set; }
        public Vector2 hipCenter { get; set; }
        public Vector2 hipRight { get; set; }
        public Vector2 hipLeft { get; set; }
        public Vector2 kneeRight { get; set; }
        public Vector2 kneeLeft { get; set; }
        public Vector2 ankleRight { get; set; }
        public Vector2 ankleLeft { get; set; }
        public Vector2 footRight { get; set; }
        public Vector2 footLeft { get; set; }
        public int Health = 20;


        public skeleton(Vector2 head,
Vector2 ishoulderCenter,
Vector2 ishoulderRight,
Vector2 ishoulderLeft,
Vector2 ielbowRight,
Vector2 ielbowLeft,
Vector2 iwristRight,
Vector2 iwristLeft,
Vector2 ihandRight,
Vector2 ihandLeft,
Vector2 ispine,
Vector2 ihipCenter,
Vector2 ihipRight,
Vector2 ihipLeft,
Vector2 ikneeRight,
Vector2 ikneeLeft,
Vector2 iankleRight,
Vector2 iankleLeft,
Vector2 ifootRight,
Vector2 ifootLeft
            )
        {
            shoulderCenter = ishoulderCenter;
            shoulderRight = ishoulderRight;
            shoulderLeft = ishoulderLeft;
            elbowRight = ielbowRight;
            elbowLeft = ielbowLeft;
            wristRight = iwristRight;
            wristLeft = iwristLeft;
            handRight = ihandLeft;
            handLeft = ihandLeft;
            spine = ispine;
            hipCenter = ihipCenter;
            hipRight = ihipRight;
            hipLeft = ihipLeft;
            kneeRight = ikneeRight;
            kneeLeft = ikneeLeft;
            ankleRight = iankleRight;
            ankleLeft = iankleLeft;
            footRight = ifootRight;
            footLeft = ifootLeft;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D iblank, Color color)
        {
            DrawLine(spriteBatch, iblank, 10, color, head, shoulderCenter);
            DrawLine(spriteBatch, iblank, 10, color, shoulderCenter, shoulderRight);
            DrawLine(spriteBatch, iblank, 10, color, shoulderCenter, shoulderLeft);
            DrawLine(spriteBatch, iblank, 10, color, shoulderLeft, elbowLeft);
            DrawLine(spriteBatch, iblank, 10, color, elbowLeft, wristLeft);
            DrawLine(spriteBatch, iblank, 10, color, wristLeft, handLeft);
            DrawLine(spriteBatch, iblank, 10, color, shoulderRight, elbowRight);
            DrawLine(spriteBatch, iblank, 10, color, elbowRight, wristRight);
            DrawLine(spriteBatch, iblank, 10, color, wristRight, handRight);
            DrawLine(spriteBatch, iblank, 40, color, shoulderCenter, spine);
            DrawLine(spriteBatch, iblank, 35, color, spine, hipCenter);
            DrawLine(spriteBatch, iblank, 10, color, hipCenter, hipRight);
            DrawLine(spriteBatch, iblank, 10, color, hipRight, kneeRight);
            DrawLine(spriteBatch, iblank, 10, color, kneeRight, ankleRight);
            DrawLine(spriteBatch, iblank, 10, color, ankleRight, footRight);
            DrawLine(spriteBatch, iblank, 10, color, hipCenter, hipLeft);
            DrawLine(spriteBatch, iblank, 10, color, hipLeft, kneeLeft);
            DrawLine(spriteBatch, iblank, 10, color, kneeLeft, ankleLeft);
            DrawLine(spriteBatch, iblank, 10, color, ankleLeft, footLeft);

        }

        public void UpdateHitBoxes()
        {



        }

        void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width),
                      SpriteEffects.None, 0);

        }
    }


}
