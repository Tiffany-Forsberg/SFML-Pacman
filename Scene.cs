using SFML.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public delegate void ValueChangedEvent(Scene scene, int value);
    public sealed class Scene
    {
        public event ValueChangedEvent GainScore;
        public event ValueChangedEvent LoseHealth;
        
        
        public readonly SceneLoader Loader = new SceneLoader();
        public readonly AssetManager Assets = new AssetManager();
        
        private List<Entity> entities = new List<Entity>();
        private int scoreGained;
        private int healthLost;

        public void PublishGainScore(int amount) => scoreGained += amount;
        public void PublishLoseHealth(int amount) => healthLost += amount;
        
        
        public void Spawn(Entity entity)
        {
            entities.Add(entity);
            entity.Create(this);
        }

        public void Clear()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Entity entity = entities[i];
                entities.RemoveAt(i);
                entity.Destroy(this);
            }
        }
        
        public bool FindByType<T>(out T found) where T : Entity
        {
            foreach (var entity in entities)
            {
                if (!entity.Dead && entity is T typed)
                {
                    found = typed;
                    return true;
                }
            }
            
            found = default(T);
            return false;
        }

        public IEnumerable<Entity> FindIntersects(FloatRect bounds)
        {
            int lastEntityIndex = entities.Count - 1;

            for (int i = lastEntityIndex; i >= 0; i--)
            {
                Entity entity = entities[i];
                if (entity.Dead) continue;
                if (entity.Bounds.Intersects(bounds))
                {
                    yield return entity;
                }
            }
        }
        
        public void UpdateAll(float deltaTime)
        {
            Loader.HandleSceneLoad(this);
            
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Entity entity = entities[i];
                entity.Update(this, deltaTime);
            }

            if (scoreGained != 0)
            {
                GainScore?.Invoke(this, scoreGained);
                scoreGained = 0;
            }
            
            if (healthLost != 0)
            {
                LoseHealth?.Invoke(this, healthLost);
                healthLost = 0;
            }

            for (int i = 0; i < entities.Count;)
            {
                Entity entity = entities[i];
                if (entity.Dead) entities.RemoveAt(i);
                else i++;
            }
        }

        public void RenderAll(RenderTarget target)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Render(target);
            }
        }
    }
}
