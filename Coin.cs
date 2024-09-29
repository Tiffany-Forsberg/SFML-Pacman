using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class Coin : Entity
    {
        public Coin() : base("pacman") {}
        
        public override void Create(Scene scene)
        {
            base.Create(scene);
            sprite.TextureRect = new IntRect(36, 36, 18, 18);
        }
    }
}
