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
            scoreText.Scale /= 1.5f;
            currentHealth = maxHealth;
            scene.Events.LoseHealth += OnLoseHealth;
            scene.Events.GainScore += OnGainScore;
        }
        
        public override void Destroy(Scene scene)
        {
            base.Destroy(scene);
            scene.Events.LoseHealth -= OnLoseHealth;
            scene.Events.GainScore -= OnGainScore;
        }

        private void OnLoseHealth(Scene scene, int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                DontDestroyOnLoad = false;
                scene.Loader.Reload();
            }
        }
        
        private void OnGainScore(Scene scene, int amount)
        {
            currentScore += amount;
            if (!scene.FindByType<Coin>(out _))
            {
                DontDestroyOnLoad = true;
                scene.Loader.Reload();
            }
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
