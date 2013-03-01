using System;
using System.Collections.Generic;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MovementBehaviors {
    public class FlockTarget : Actor {
        double _lastUpdate;
        TimeSpan _updateRate = TimeSpan.FromSeconds(0.25);
        
        public FlockTarget(FlockComponent parentFlock, Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity)
            : base(parentFlock, texture, position, size, velocity) {
            Tint = Color.Red;
        }

        MouseState _mouseStateLastFrame;
        public override void Update(GameTime gameTime) {
            MouseState mouse = Mouse.GetState();
            if (_mouseStateLastFrame.LeftButton == ButtonState.Pressed) {// && mouse.LeftButton == ButtonState.Released) {
                Position = new Vector2(mouse.X, mouse.Y);
            }
            _mouseStateLastFrame = mouse;
            
            Vector2 newVelocity = Wander();

            Velocity = newVelocity;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // Draw wander target indicator
            if (ParentFlock.DrawWanderTarget)
                spriteBatch.Draw(ParentFlock.TextureCircle,
                                 Position + WanderTarget,
                                 null,
                                 new Color(0, 255, 0, 100),
                                 0f,
                                 ParentFlock.TextureCircle.Bounds.Center.ToVector2(),
                                 0.15f,
                                 SpriteEffects.None,
                                 0);

            base.Draw(spriteBatch);
        }
    }
}
