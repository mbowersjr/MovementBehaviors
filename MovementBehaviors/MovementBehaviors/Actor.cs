using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MovementBehaviors {
    public class Actor : Sprite {
        static Random random = new Random();

        Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            protected set {
                _velocity = value;
                Rotation = _velocity.ToAngle();
            }
        }

        protected readonly FlockComponent ParentFlock;
        
        protected const float MaxVelocity = 50.0f;
        protected const float TurnRate = 0.03f;
        protected const int ArrivalSlowingRadius = 150;
        protected const int FlockRadius = 30;
        protected const int EvadeRadius = FlockRadius;
        
        protected Vector2 WanderTarget;
        protected float WanderTargetRateOfChange = 0.02f;
        protected const double WanderTargetChanceToReverse = 0.005;
        protected const float WanderTargetDistance = ArrivalSlowingRadius;

        public Actor(FlockComponent parentFlock, Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity)
            : base(texture, position, size) {
            ParentFlock = parentFlock;
            Velocity = velocity;
        }

        public virtual void Update(GameTime gameTime) {
            if (Velocity.Length() > MaxVelocity)
                Velocity = Vector2.Normalize(Velocity) * MaxVelocity;

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }

        protected Vector2 Seek(Vector2 target) {
            Vector2 desiredNormal = Vector2.Normalize(target - Position);
            float desiredAngle = desiredNormal.ToAngle();
            float currentAngle = Velocity.ToAngle();
            float angleDifference = MathHelper.WrapAngle(desiredAngle - currentAngle);
            angleDifference = MathHelper.Clamp(angleDifference, -TurnRate, TurnRate);
            float newAngle = MathHelper.WrapAngle(currentAngle + angleDifference);

            
            return newAngle.ToVector2();
        }

        protected Vector2 Flee(Vector2 target) {
            Vector2 desiredNormal = Vector2.Normalize(target - Position);
            float desiredAngle = desiredNormal.ToAngle();
            float currentAngle = Velocity.ToAngle();
            float angleDifference = MathHelper.WrapAngle(desiredAngle - currentAngle);
            angleDifference = MathHelper.Clamp(angleDifference, -TurnRate, TurnRate);
            float newAngle = MathHelper.WrapAngle(currentAngle - angleDifference);

            return newAngle.ToVector2();
        }

        protected Vector2 Pursuit(Actor target, float anticipation) {
            Vector2 anticipatedPosition = target.Position + (target.Velocity * anticipation);
            
            float distance = Vector2.Distance(anticipatedPosition, Position);
            float speed = distance < ArrivalSlowingRadius
                              ? MaxVelocity * (distance / ArrivalSlowingRadius)
                              : MaxVelocity;

            return Seek(anticipatedPosition) * speed;
        }

        protected Vector2 Evade(Actor target, float anticipation) {
            Vector2 anticipatedPosition = target.Position + (target.Velocity * anticipation);
        
            float distance = Vector2.Distance(anticipatedPosition, Position);
            float speed = distance < EvadeRadius
                              ? MaxVelocity / (distance / EvadeRadius)
                              : MaxVelocity;

            return Flee(anticipatedPosition) * speed;
        }

        protected Vector2 FlockSeparate() {
            Vector2 averagePosition = Vector2.Zero;
            int count = 0;
            foreach (Actor actor in ParentFlock.Actors) {
                if (actor != this && Vector2.Distance(Position, actor.Position) <= FlockRadius) {
                    //change += Evade(actor, 3);
                    averagePosition += actor.Position;
                    count++;
                }
            }
            if (count == 0)
                return Vector2.Zero;

            averagePosition = Vector2.Divide(averagePosition, count);
            float distance = Vector2.Distance(averagePosition, Position);
            float speed = distance < FlockRadius
                              ? MaxVelocity/(distance/FlockRadius)
                              : MaxVelocity;
            return Flee(averagePosition)*speed;
        }
        
        protected Vector2 FlockAlign() {
            Vector2 averageDirection = Vector2.Zero;
            int count = 0;
            foreach (Actor actor in ParentFlock.Actors) {
                if (actor != this && Vector2.Distance(Position, actor.Position) <= FlockRadius) {
                    averageDirection += Vector2.Normalize(actor.Velocity);
                    count++;
                }
            }
            if (count == 0)
                return Vector2.Zero;

            averageDirection = Vector2.Divide(averageDirection, count);
            
            return Seek(Position + (averageDirection * Velocity.Length()));
        }

        protected Vector2 Wander() {
            if (WanderTarget == Vector2.Zero)
                ResetWanderTarget();
            UpdateWanderTarget();

            return Seek(Position + WanderTarget) * MaxVelocity;
        }

        private float _wanderTargetAngle;

        private void UpdateWanderTarget() {
            if (random.NextDouble() < WanderTargetChanceToReverse)
                WanderTargetRateOfChange *= -1;

            _wanderTargetAngle = MathHelper.WrapAngle(_wanderTargetAngle + WanderTargetRateOfChange);
            WanderTarget = _wanderTargetAngle.ToVector2() * WanderTargetDistance;
        }

        private void ResetWanderTarget() {
            //WanderTarget = ((float)(random.NextDouble() * MathHelper.TwoPi - MathHelper.Pi)).ToVector2() *
            //                WanderTargetDistance;
            _wanderTargetAngle = (float) ((random.NextDouble()*MathHelper.TwoPi) - MathHelper.Pi);
        }
    }
}
