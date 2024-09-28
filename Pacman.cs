using SFML.Graphics;
using SFML.System;
using System.Linq;
using SFML.Window;
using static SFML.Window.Keyboard.Key;

// Add buffer to movement
// Make PacMan open and close mouth

namespace Pacman
{
    public class Pacman : Actor
    {
        public override void Create(Scene scene)
        {
            speed = 100.0f;
            base.Create(scene);
            sprite.TextureRect = new IntRect(0, 0, 18, 18);
        }

        protected override int PickDirection(Scene scene)
        {
            int dir = direction;

            if (Keyboard.IsKeyPressed(Right) || Keyboard.IsKeyPressed(D))
            {
                dir = 0;
                moving = true;
            }
            else if (Keyboard.IsKeyPressed(Up) || Keyboard.IsKeyPressed(W))
            {
                dir = 1;
                moving = true;
            }
            else if (Keyboard.IsKeyPressed(Left) || Keyboard.IsKeyPressed(A))
            {
                dir = 2;
                moving = true;
            }
            else if (Keyboard.IsKeyPressed(Down) || Keyboard.IsKeyPressed(S))
            {
                dir = 3;
                moving = true;
            }

            if (IsFree(scene, dir))
            {
                sprite.TextureRect = new IntRect(0, 18 * dir, 18, 18);
                return dir;
            }
            if (!IsFree(scene, direction)) moving = false;
            return direction;
        }
    }
}
