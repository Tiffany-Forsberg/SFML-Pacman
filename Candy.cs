using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class Candy : Entity
    {
        public Candy() : base("pacman") {}
        
        public override void Create(Scene scene)
        {
            base.Create(scene);
            sprite.TextureRect = new IntRect(36, 54, 18, 18);
        }
        
        protected override void CollideWith(Scene scene, Entity e)
        {
            if (e is Pacman)
            {
                scene.PublishEatCandy(1);
                Dead = true;
            }
        }
    }
}
