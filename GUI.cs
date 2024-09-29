using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace Pacman
{
    public class GUI : Entity
    {
        private Text scoreText;
        private int maxHealth = 4;
        private int currentHealth;
        private int currentScore;
        
        public GUI() : base("pacman") {}
        public override void Create(Scene scene)
        {
            base.Create(scene);
            sprite.TextureRect = new IntRect(72, 36, 18, 18);
            scoreText = new Text();
            scoreText.Font = scene.Assets.LoadFont("pixel-font");
            scoreText.DisplayedString = "Score";
            scoreText.CharacterSize = 24;
            scoreText.Scale = scoreText.Scale/1.5f;
            currentHealth = maxHealth;
        }

        public override void Render(RenderTarget target)
        {
            sprite.Position = new Vector2f(36, 396);
            for (int i = 0; i < maxHealth; i++)
            {
                sprite.TextureRect = i < currentHealth
                    ? new IntRect(72, 36, 18, 18) // Full heart
                    : new IntRect(72, 0, 18, 18); // Empty heart

                base.Render(target);
                sprite.Position += new Vector2f(18, 0);
            }

            scoreText.DisplayedString = $"Score: {currentScore}";
            scoreText.Position = new Vector2f(414 - scoreText.GetGlobalBounds().Width, 396);
            
            target.Draw(scoreText);
        }
    }
}
