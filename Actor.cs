using SFML.Graphics;
using SFML.System;

namespace Pacman
{
    public class Actor : Entity
    {
        public float ResetTimer;
        
        private bool wasAligned;
        private Vector2f originalPosition;
        private float originalSpeed;
        
        protected float speed;
        protected int direction;
        protected bool moving;
        
        public Actor() : base("pacman") {}

        // Checks if actor is aligned to the grid all squares are 18 x 18 pixels
        protected bool IsAligned => 
            (int) MathF.Floor(Position.X) % 18 == 0 &&
            (int) MathF.Floor(Position.Y) % 18 == 0;

        protected void Reset()
        {
            wasAligned = false;
            Position = originalPosition;
            speed = originalSpeed;
            ResetTimer = 1f;
        }

        public override void Create(Scene scene)
        {
            base.Create(scene);
            originalPosition = Position;
            originalSpeed = speed;
            Reset();
        }

        // Checks if actor can move to square
        protected bool IsFree(Scene scene, int dir)
        {
            Vector2f at = Position + new Vector2f(9, 9); // Sets "at" to center of current square
            at += 18 * ToVector(dir); // Sets "at" to next square
            FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);

            return !scene.FindIntersects(rect).Any(e => e.Solid);
        }

        protected static Vector2f ToVector(int dir)
        {
            return dir switch
            {
                0 => new Vector2f(1, 0), // Right
                1 => new Vector2f(0, -1), // Up
                2 => new Vector2f(-1, 0), // Left
                _ => new Vector2f(0, 1) // Down
            };
        }
        
        protected virtual int PickDirection(Scene scene)
        {
            return 0; 
        }

        public override void Update(Scene scene, float deltaTime)
        {
            base.Update(scene, deltaTime);

            // Takes the higher value, can't be <0
            ResetTimer = MathF.Max(ResetTimer - deltaTime, 0.0f);

            if (ResetTimer > 0) return;
            
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
