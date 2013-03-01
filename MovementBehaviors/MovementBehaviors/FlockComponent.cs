using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//#define DEBUG_TEXT

namespace MovementBehaviors {

    public class FlockComponent : DrawableGameComponent {
        public List<Actor> Actors = new List<Actor>();
        public FlockTarget Target;
        public Rectangle Bounds;

        public Texture2D TextureActor;
        public Texture2D TextureCircle;
        public SpriteFont FontDebug;
        
        SpriteBatch _spriteBatch;
        public bool DoFlockSeparation = false;
        public bool DoFlockAlignment = false;
        public bool DrawEvadeRadius = false;
        public bool DrawWanderTarget = false;

        public FlockComponent(Game game)
            : base(game) {
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Bounds = GraphicsDevice.Viewport.Bounds;
            
            TextureActor = Game.Content.Load<Texture2D>("Actor");
            TextureCircle = Game.Content.Load<Texture2D>("Circle");
            FontDebug = Game.Content.Load<SpriteFont>("DebugFont");

            Vector2 size = new Vector2(TextureActor.Width / 4, TextureActor.Height / 4);
            Target = new FlockTarget(this, TextureActor, Bounds.RandomVector2(), size, Vector2.One);

            const int initialActorCount = 20;
            for (int x = 0; x < initialActorCount; ++x) {
                Actors.Add(new FlockMember(this, TextureActor, Bounds.RandomVector2(), size, Vector2.Zero));
            }
        }

        protected override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Initialize() {
            base.Initialize();
        }

        private KeyboardState _keyboardPrevious;
        public override void Update(GameTime gameTime) {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S) && _keyboardPrevious.IsKeyUp(Keys.S))
                DoFlockSeparation = !DoFlockSeparation;
            if (keyboard.IsKeyDown(Keys.A) && _keyboardPrevious.IsKeyUp(Keys.A))
                DoFlockAlignment = !DoFlockAlignment;
            if (keyboard.IsKeyDown(Keys.R) && _keyboardPrevious.IsKeyUp(Keys.R))
                DrawEvadeRadius = !DrawEvadeRadius;
            if (keyboard.IsKeyDown(Keys.W) && _keyboardPrevious.IsKeyUp(Keys.W))
                DrawWanderTarget = !DrawWanderTarget;

            _keyboardPrevious = keyboard;

            Target.Update(gameTime);

            foreach (Actor actor in Actors) {
                actor.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            string infoText = "Flock Separation [S]: " + DoFlockSeparation + Environment.NewLine;
            infoText += "Flock Alignment [A]: " + DoFlockAlignment + Environment.NewLine;
            infoText += "Draw Evade Radius [R]: " + DrawEvadeRadius + Environment.NewLine;
            infoText += "Draw Wander Target [W]: " + DrawWanderTarget + Environment.NewLine;
            
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            _spriteBatch.DrawString(FontDebug, infoText, new Vector2(20,20), Color.White);

            Target.Draw(_spriteBatch);

            foreach (Actor actor in Actors) {
                actor.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
