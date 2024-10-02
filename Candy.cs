using SFML.Graphics;

namespace Pacman
{
    public class Candy : Entity
    {
        public Candy() : base("pacman") {}
     
        // Thins collision
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
            sprite.TextureRect = new IntRect(36, 54, 18, 18);
        }
        
        protected override void CollideWith(Scene scene, Entity e)
        {
            if (e is Pacman)
            {
                scene.Events.PublishEatCandy(1);
                Dead = true;
            }
        }
    }
}
