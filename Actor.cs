using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class Actor : Entity
    {
        private bool wasAligned;
        
        protected float speed;
        protected int direction;
        protected bool moving;
        protected Vector2f originalPosition;
        protected float originalSpeed;
        
        public Actor() : base("pacman") {}

        protected bool IsAligned => 
            (int) MathF.Floor(Position.X) % 18 == 0 &&
            (int) MathF.Floor(Position.Y) % 18 == 0;

        protected void Reset()
        {
            wasAligned = false;
            originalPosition = Position;
            originalSpeed = speed;
        }

        public override void Create(Scene scene)
        {
            base.Create(scene);
            Reset();
        }

        protected bool IsFree(Scene scene, int direction)
        {
            Vector2f at = Position + new Vector2f(9, 9);
            at += 18 * ToVector(direction);
            FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);

            return !scene.FindIntersects(rect).Any(e => e.Solid);
        }

        protected static Vector2f ToVector(int direction)
        {
            if (direction == 0)
            {
                return new Vector2f(-1, 0);
            }

            if (direction == 1)
            {
                return new Vector2f(1, 0);
            }
            
            if (direction == 2)
            {
                return new Vector2f(0, -1);
            }
            
            return new Vector2f(0, 1);
        }

        protected int PickDirection(Scene scene)
        {
            return 0; 
        }

        public override void Update(Scene scene, float deltaTime)
        {
            base.Update(scene, deltaTime);

            if (IsAligned)
            {
                if (!wasAligned)
                {
                    direction = PickDirection(scene);
                }

                if (moving)
                {
                    wasAligned = true;
                }
            }
            else
            {
                wasAligned = false;
            }

            if (!moving) return;

            Position += ToVector(direction) * (speed * deltaTime);
            Position = MathF.Floor(Position.X) switch
            {
                < 0 => new Vector2f(432, Position.Y),
                > 432 => new Vector2f(0, Position.Y),
                _ => Position
            };
        }
    }
}
