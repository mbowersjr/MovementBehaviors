using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MovementBehaviors {
    public class Sprite {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }
        public float Rotation { get; protected set; }
        public Texture2D Texture { get; private set; }
        public Color Tint { get; protected set; }
        public Vector2 Origin { get; private set; }
        public Vector2 Scale { get; private set; }

        public Sprite(Texture2D texture, Vector2 position, Vector2 size, Color tint) {
            Texture = texture;
            Position = position;
            Size = size;
            Tint = tint;

            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            Scale = Vector2.Divide(Size, new Vector2(Texture.Width, Texture.Height));
        }

        public Sprite(Texture2D texture, Vector2 position, Vector2 size)
            : this(texture, position, size, Color.White) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture,
                             Position,
                             null,
                             Tint,
                             Rotation,
                             Origin,
                             Scale,
                             SpriteEffects.None,
                             0);
        }
    }
}
