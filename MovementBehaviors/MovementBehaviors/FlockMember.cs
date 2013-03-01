using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MovementBehaviors {
    public class FlockMember : Actor {
        public FlockMember(FlockComponent parentFlock, Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity)
            : base(parentFlock, texture, position, size, velocity) {
            Tint = Color.Blue;
        }

        public override void Update(GameTime gameTime) {
            Vector2 newVelocity = Pursuit(ParentFlock.Target, 0);
            if (ParentFlock.DoFlockSeparation)
                newVelocity += FlockSeparate();
            if (ParentFlock.DoFlockAlignment)
                newVelocity += FlockAlign();

            Velocity = newVelocity;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // Draw evasion radius indicator
            if (ParentFlock.DrawEvadeRadius)
                spriteBatch.Draw(ParentFlock.TextureCircle,
                                 Position,
                                 null,
                                 new Color(255, 255, 255, 100),
                                 0,
                                 ParentFlock.TextureCircle.Bounds.Center.ToVector2(),
                                 (EvadeRadius*2f)/ParentFlock.TextureCircle.Width,
                                 SpriteEffects.None,
                                 1);

            base.Draw(spriteBatch);
        }
    }
}