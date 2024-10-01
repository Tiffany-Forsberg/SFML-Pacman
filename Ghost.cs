using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class Ghost : Actor
    {
        private float frozenTimer;
        private Clock animationTimer = new Clock();
        private AnimationState currentSprite = AnimationState.State1;
        
        public override void Create(Scene scene)
        {
            direction = -1;
            speed = 100.0f;
            moving = true;
            base.Create(scene);
            sprite.TextureRect = new IntRect(36, 0, 18, 18);
            scene.Events.EatCandy += OnEatCandy;
        }
        
        public override void Destroy(Scene scene)
        {
            base.Destroy(scene);
            scene.Events.EatCandy -= OnEatCandy;
        }

        private void OnEatCandy(Scene scene, int amount)
        {
            frozenTimer = 5;
        }

        protected override int PickDirection(Scene scene)
        {
            List<int> validMoves = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if ((i + 2) % 4 == direction) continue;
                if (IsFree(scene, i)) validMoves.Add(i);
            }

            int r = new Random().Next(0, validMoves.Count);
            return validMoves[r];
        }

        protected override void CollideWith(Scene scene, Entity e)
        {
            if (e is not Pacman) return;
            if (e is Actor { resetTimer: > 0 }) // Checks if the collided entity is an actor with an active resetTimer
            {
                return;
            }
            if (resetTimer > 0f) return;
            if (frozenTimer <= 0)
            {
                scene.Events.PublishLoseHealth(1);
            }
            Reset();
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
            
            sprite.TextureRect = new IntRect(currentSprite == AnimationState.State1 ? 36 : 54, frozenTimer > 0 ? 18 : 0, 18, 18);
            animationTimer.Restart();
        }
        
        public override void Update(Scene scene, float deltaTime)
        {
            base.Update(scene, deltaTime);
            frozenTimer = MathF.Max(frozenTimer - deltaTime, 0.0f);
        }

        public override void Render(RenderTarget target)
        {
            HandleAnimation();
            base.Render(target);
        }
    }
}
