using SFML.Graphics;
using SFML.System;
using System.Linq;
using SFML.Window;
using static SFML.Window.Keyboard.Key;

// Add buffer to movement

namespace Pacman
{
    public class Pacman : Actor
    {
        private Clock animationTimer = new Clock();
        private AnimationState currentSprite = AnimationState.State1;
        
        public override void Create(Scene scene)
        {
            speed = 100.0f;
            base.Create(scene);
            sprite.TextureRect = new IntRect(0, 0, 18, 18);
            scene.LoseHealth += OnLoseHealth;
        }

        public override void Destroy(Scene scene)
        {
            base.Destroy(scene);
            scene.LoseHealth -= OnLoseHealth;
        }

        private void OnLoseHealth(Scene scene, int amount)
        {
            Reset();
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
                sprite.TextureRect = new IntRect((currentSprite == AnimationState.State1 ? 0 : 1) * 18, 18 * dir, 18, 18);
                return dir;
            }
            if (!IsFree(scene, direction)) moving = false;
            return direction;
        }

        private void HandleAnimation()
        {
            if (animationTimer.ElapsedTime.AsSeconds() < 0.2f) return;
            
            if (currentSprite == AnimationState.State1)
            {
                currentSprite = AnimationState.State2;
            }
            else
            {
                currentSprite = AnimationState.State1;
            }
            
            sprite.TextureRect = new IntRect((currentSprite == AnimationState.State1 ? 0 : 1) * 18, 18 * direction, 18, 18);
            animationTimer.Restart();
        }
        
        public override void Update(Scene scene, float deltaTime)
        {
            base.Update(scene, deltaTime);

            if (moving)
            {
                HandleAnimation();
            }
        }
    }
}
