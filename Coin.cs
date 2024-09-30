using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class Coin : Entity
    {
        public Coin() : base("pacman") {}
        
        public override FloatRect Bounds {
            get {
                var bounds = base.Bounds;
                bounds.Left += 1;
                bounds.Width -= 1;
                bounds.Top += 1;
                bounds.Height -= 1;
                return bounds;
            }
        }
        
        public override void Create(Scene scene)
        {
            base.Create(scene);
            sprite.TextureRect = new IntRect(36, 36, 18, 18);
        }
        
        protected override void CollideWith(Scene scene, Entity e)
        {
            if (e is Pacman)
            {
                scene.Events.PublishGainScore(100);
                Dead = true;
            }
        }
    }
}
