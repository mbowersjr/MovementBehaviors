using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace MovementBehaviors {
    public static class HelperExtensions {
        static Random random = new Random();

        // Vector2 -> float
        public static float ToAngle(this Vector2 vector) {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        // float -> Vector2
        public static Vector2 ToVector2(this float angle) {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        
        // Point -> Vector2
        public static Vector2 ToVector2(this Point point) {
            return new Vector2(point.X, point.Y);
        }

        // Rectangle -> Random Point
        public static Point RandomPoint(this Rectangle bounds) {
            return new Point(random.Next(bounds.Width), random.Next(bounds.Height));
        }

        // Rectangle -> Random Vector2
        public static Vector2 RandomVector2(this Rectangle bounds) {
            return new Vector2((float)random.NextDouble() * bounds.Width, (float)random.NextDouble() * bounds.Height);
        }

        
    }
}
