using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace GameJAMGodGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KinectSensor kinectSensor;
        string connectedStatus;
        Texture2D kinectRGBVideo;

        //Vector2 player1righthandPosition;
        //Vector2 player1lefthandPosition;
        //Vector2 player1HeadPosition = new Vector2 (0,0);

        //Vector2 player2righthandPosition;
        //Vector2 player2lefthandPosition;
        //Vector2 player2HeadPosition;

        skeleton player1 = new skeleton(new Vector2 (-100,-100),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0));
        skeleton player2 = new skeleton(new Vector2 (-100,-100),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0));

        clsSprite PlayerHelmet;
        clsSprite ForeGround;
        clsSprite Background;
        clsSprite Player1Head;
        clsSprite Player2Head;

        SpriteFont font1;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";
         
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            KinectSensor.KinectSensors.StatusChanged += new
                    EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();


            base.Initialize();
        }


        private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    kinectSensor = sensor;
                    break;
                }
            }
            if (this.kinectSensor == null)
            {
                connectedStatus = "Found none Kinect Sensors connected to USB";
                return;
            }
            // Init the kinect
            if (kinectSensor.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (this.kinectSensor == e.Sensor)
            {
                if (e.Status == KinectStatus.Disconnected ||
                        e.Status == KinectStatus.NotPowered)
                {
                    this.kinectSensor = null;
                    this.DiscoverKinectSensor();
                }
            }
        }





        private bool InitializeKinect()
        {
            // Color stream
            kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            kinectSensor.ColorFrameReady += new
                  EventHandler<ColorImageFrameReadyEventArgs>(kinectSensor_ColorFrameReady);
            // Skeleton Stream
            kinectSensor.SkeletonStream.Enable((new
TransformSmoothParameters(){ 
     Smoothing = 0.7f, 
Correction = 0.7f, 
Prediction = 0.5f, 
JitterRadius = 0.05f, 
MaxDeviationRadius = 0.04f}));
            kinectSensor.SkeletonFrameReady += new
                 EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);

            try
            {
                kinectSensor.Start();
            }
            catch
            {

                return false;
            }
            return true;
        }

        void kinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {
                    byte[] pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];
                    colorImageFrame.CopyPixelDataTo(pixelsFromFrame);
                    Color[] color = new Color[colorImageFrame.Height * colorImageFrame.Width];
                    kinectRGBVideo = new Texture2D(graphics.GraphicsDevice,
                                                   colorImageFrame.Width,
              colorImageFrame.Height);
                    // Go through each pixel and set the bytes correctly
                    // Remember, each pixel has Red, Green and Blue
                    int index = 0;
                    for (int y = 0; y < colorImageFrame.Height; y++)
                    {
                        for (int x = 0; x < colorImageFrame.Width; x++, index += 4)
                        {
                            color[y * colorImageFrame.Width + x] = new Color(pixelsFromFrame[index + 2],
                                             pixelsFromFrame[index + 1], pixelsFromFrame[index + 0]);
                        }
                    }
                    // Set pixeldata from the ColorImageFrame to a Texture2D
                    kinectRGBVideo.SetData(color);
                }
            }
        }

        void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                int player = 1;
                if (skeletonFrame != null)
                {

                    Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    player = 1;

                    foreach (Skeleton S in skeletonData)
                    {

                        
                        if (S.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            
                            if (player == 1)
                            {

                                Joint head = S.Joints[JointType.Head];
                                player2.head = new Vector2((((0.5f * head.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * head.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderCenter = S.Joints[JointType.ShoulderCenter];
                                player2.shoulderCenter = new Vector2((((0.5f * ShoulderCenter.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderCenter.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderRight = S.Joints[JointType.ShoulderRight];
                                player2.shoulderRight = new Vector2((((0.5f * ShoulderRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderRight.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderLeft = S.Joints[JointType.ShoulderLeft];
                                player2.shoulderLeft = new Vector2((((0.5f * ShoulderLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderLeft.Position.Y) + 0.5f) * (480)));

                                Joint ElbowRight = S.Joints[JointType.ElbowRight];
                                player2.elbowRight = new Vector2((((0.5f * ElbowRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ElbowRight.Position.Y) + 0.5f) * (480)));

                                Joint ElbowLeft = S.Joints[JointType.ElbowLeft];
                                player2.elbowLeft = new Vector2((((0.5f * ElbowLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ElbowLeft.Position.Y) + 0.5f) * (480)));

                                Joint WristRight = S.Joints[JointType.WristRight];
                                player2.wristRight = new Vector2((((0.5f * WristRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * WristRight.Position.Y) + 0.5f) * (480)));

                                Joint WristLeft = S.Joints[JointType.WristLeft];
                                player2.wristLeft = new Vector2((((0.5f * WristLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * WristLeft.Position.Y) + 0.5f) * (480)));

                                Joint rightHand = S.Joints[JointType.HandRight];
                                player2.handRight = new Vector2((((0.5f * rightHand.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * rightHand.Position.Y) + 0.5f) * (480)));

                                Joint leftHand = S.Joints[JointType.HandLeft];
                                player2.handLeft = new Vector2((((0.5f * leftHand.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * leftHand.Position.Y) + 0.5f) * (480)));

                                Joint spine = S.Joints[JointType.Spine];
                                player2.spine = new Vector2((((0.5f * spine.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * spine.Position.Y) + 0.5f) * (480)));

                                Joint hipCenter = S.Joints[JointType.HipCenter];
                                player2.hipCenter = new Vector2((((0.5f * hipCenter.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipCenter.Position.Y) + 0.5f) * (480)));

                                Joint hipRight = S.Joints[JointType.HipRight];
                                player2.hipRight = new Vector2((((0.5f * hipRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipRight.Position.Y) + 0.5f) * (480)));

                                Joint hipLeft = S.Joints[JointType.HipLeft];
                                player2.hipLeft = new Vector2((((0.5f * hipLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipLeft.Position.Y) + 0.5f) * (480)));

                                Joint kneeRight = S.Joints[JointType.KneeRight];
                                player2.kneeRight = new Vector2((((0.5f * kneeRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * kneeRight.Position.Y) + 0.5f) * (480)));

                                Joint kneeLeft = S.Joints[JointType.KneeLeft];
                                player2.kneeLeft = new Vector2((((0.5f * kneeLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * kneeLeft.Position.Y) + 0.5f) * (480)));

                                Joint ankleRight = S.Joints[JointType.AnkleRight];
                                player2.ankleRight = new Vector2((((0.5f * ankleRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ankleRight.Position.Y) + 0.5f) * (480)));

                                Joint ankleLeft = S.Joints[JointType.AnkleLeft];
                                player2.ankleLeft = new Vector2((((0.5f * ankleLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ankleLeft.Position.Y) + 0.5f) * (480)));

                                Joint footRight = S.Joints[JointType.FootRight];
                                player2.footRight = new Vector2((((0.5f * footRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * footRight.Position.Y) + 0.5f) * (480)));

                                Joint footLeft = S.Joints[JointType.FootLeft];
                                player2.footLeft = new Vector2((((0.5f * footLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * footLeft.Position.Y) + 0.5f) * (480)));
 
                            }
                            if (player == 2)
                            {


                                Joint head = S.Joints[JointType.Head];
                                player1.head = new Vector2((((0.5f * head.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * head.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderCenter = S.Joints[JointType.ShoulderCenter];
                                player1.shoulderCenter = new Vector2((((0.5f * ShoulderCenter.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderCenter.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderRight = S.Joints[JointType.ShoulderRight];
                                player1.shoulderRight = new Vector2((((0.5f * ShoulderRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderRight.Position.Y) + 0.5f) * (480)));

                                Joint ShoulderLeft = S.Joints[JointType.ShoulderLeft];
                                player1.shoulderLeft = new Vector2((((0.5f * ShoulderLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ShoulderLeft.Position.Y) + 0.5f) * (480)));

                                Joint ElbowRight = S.Joints[JointType.ElbowRight ];
                                player1.elbowRight = new Vector2((((0.5f * ElbowRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ElbowRight.Position.Y) + 0.5f) * (480)));

                                Joint ElbowLeft = S.Joints[JointType.ElbowLeft];
                                player1.elbowLeft = new Vector2((((0.5f * ElbowLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ElbowLeft.Position.Y) + 0.5f) * (480)));

                                Joint WristRight = S.Joints[JointType.WristRight];
                                player1.wristRight = new Vector2((((0.5f * WristRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * WristRight.Position.Y) + 0.5f) * (480)));

                                Joint WristLeft = S.Joints[JointType.WristLeft];
                                player1.wristLeft = new Vector2((((0.5f * WristLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * WristLeft.Position.Y) + 0.5f) * (480)));

                                Joint rightHand = S.Joints[JointType.HandRight];
                                player1.handRight= new Vector2((((0.5f * rightHand.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * rightHand.Position.Y) + 0.5f) * (480)));

                                Joint leftHand = S.Joints[JointType.HandLeft];
                                player1.handLeft  = new Vector2((((0.5f * leftHand.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * leftHand.Position.Y) + 0.5f) * (480)));

                                Joint spine = S.Joints[JointType.Spine];
                                player1.spine = new Vector2((((0.5f * spine.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * spine.Position.Y) + 0.5f) * (480)));

                                Joint hipCenter = S.Joints[JointType.HipCenter];
                                player1.hipCenter = new Vector2((((0.5f * hipCenter.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipCenter.Position.Y) + 0.5f) * (480)));

                                Joint hipRight = S.Joints[JointType.HipRight];
                                player1.hipRight = new Vector2((((0.5f * hipRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipRight.Position.Y) + 0.5f) * (480)));

                                Joint hipLeft = S.Joints[JointType.HipLeft];
                                player1.hipLeft = new Vector2((((0.5f * hipLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * hipLeft.Position.Y) + 0.5f) * (480)));

                                Joint kneeRight = S.Joints[JointType.KneeRight];
                                player1.kneeRight = new Vector2((((0.5f * kneeRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * kneeRight.Position.Y) + 0.5f) * (480)));

                                Joint kneeLeft = S.Joints[JointType.KneeLeft];
                                player1.kneeLeft = new Vector2((((0.5f * kneeLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * kneeLeft.Position.Y) + 0.5f) * (480)));

                                Joint ankleRight = S.Joints[JointType.AnkleRight];
                                player1.ankleRight = new Vector2((((0.5f * ankleRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ankleRight.Position.Y) + 0.5f) * (480)));

                                Joint ankleLeft = S.Joints[JointType.AnkleLeft];
                                player1.ankleLeft = new Vector2((((0.5f * ankleLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * ankleLeft.Position.Y) + 0.5f) * (480)));

                                Joint footRight = S.Joints[JointType.FootRight];
                                player1.footRight = new Vector2((((0.5f * footRight.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * footRight.Position.Y) + 0.5f) * (480)));

                                Joint footLeft = S.Joints[JointType.FootLeft];
                                player1.footLeft = new Vector2((((0.5f * footLeft.Position.X) + 0.5f) * (640)),
                                                          (((-0.5f * footLeft.Position.Y) + 0.5f) * (480)));
                            }
                        }
                        player = 2;
                        
                    }

                }

            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font1 = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here

            PlayerHelmet = new clsSprite(Content.Load<Texture2D>("Helmet"), new Vector2(0, 0), new Vector2(10, 10), new Vector2(0.3f, 0.3f), 3.14f, 640, 480);

            Player1Head = new clsSprite(Content.Load<Texture2D>("HEAD 1"), new Vector2(0, 0), new Vector2(10, 10), new Vector2(0.4f, 0.4f), 3.14f, 640, 480);
            Player2Head = new clsSprite(Content.Load<Texture2D>("thor head"), new Vector2(0, 0), new Vector2(10, 10), new Vector2(0.4f, 0.4f), 3.14f, 640, 480);

            ForeGround = new clsSprite(Content.Load<Texture2D>("ForeGround"), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-1, -1), 0.0f, 640, 480);
            Background = new clsSprite(Content.Load<Texture2D>("Background"), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-1, -1), 0.0f, 640, 480);

            ForeGround.position = new Vector2(640, 480);
            Background.position = new Vector2(640, 480);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            
            











          
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            spriteBatch.Begin();
            //spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            Background.Draw(spriteBatch);
            string player1located = "";
            string player2located = "";

            if (player1.spine.X > 0)
            {
                player1located = "Player1 Found!";
            }
            else
            {
                player1located = "Player1 not Found!";
            }

            if (player2.spine.X > 0)
            {
                player2located = "Player2 Found!";
            }
            else
            {
                player2located = "Player2 not Found!" + connectedStatus;
            }

            spriteBatch.DrawString(font1, player1located, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font1, player2located, new Vector2(200, 0), Color.White);

            player1.Draw(spriteBatch, blank, Color.Black);
            player2.Draw(spriteBatch, blank, Color.Black);

            player1.Draw(spriteBatch, blank, Color.Black);
            player2.Draw(spriteBatch, blank, Color.Black);




            Player1Head.scale = new Vector2(0.8f, 0.8f);
            Player1Head.position = new Vector2(player1.head.X + 30, player1.head.Y + 90);
            Player1Head.Draw(spriteBatch);

            Player2Head.scale = new Vector2(0.8f, 0.8f);
            Player2Head.position = new Vector2(player2.head.X + 30, player2.head.Y + 90);
            Player2Head.Draw(spriteBatch);
            ForeGround.Draw(spriteBatch);

            spriteBatch.End();


                      


            base.Draw(gameTime);
        }

   

        void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1,
          Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width),
                      SpriteEffects.None, 0);

        }
    }

}
